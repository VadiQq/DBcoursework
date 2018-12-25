using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Services
{
    public class TicketService : ITicketService
    {
        private readonly string connectionString;

        public TicketService()
        {
            connectionString = System.Configuration.ConfigurationManager.
                               ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public async Task CreateTicketAsync(int routeId,
                                    string carriageType,
                                    decimal price,
                                    DateTime tripDate,
                                    int carriageNumber,
                                    int positionNumber,
                                    int isPurchased,
                                    DateTime arrivalDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var tickets = new List<TicketViewModel>();

                command.CommandText = "insert into ticket(route_id, carriage_type, trip_date, price,carriage_number,position_number, isPurchased, arrival_date)" +
                                      "values(@rt, @cr,@dt,@pr,@cn,@pn,@st,@ad)";
                command.Parameters.AddWithValue("@rt", routeId);
                command.Parameters.AddWithValue("@cr", carriageType);
                command.Parameters.AddWithValue("@dt", tripDate);
                command.Parameters.AddWithValue("@pr", price);
                command.Parameters.AddWithValue("@cn", carriageNumber);
                command.Parameters.AddWithValue("@pn", positionNumber);
                command.Parameters.AddWithValue("@st", isPurchased);
                command.Parameters.AddWithValue("@ad", arrivalDate);

                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task LockTicketsAsync(int[] ids)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var query = "update ticket set inProccess = 1 where ticket_id in ({0})";
                var idParameterList = new List<string>();
                var index = 0;
                foreach (var id in ids)
                {
                    var paramName = "@idParam" + index;
                    command.Parameters.AddWithValue(paramName, id);
                    idParameterList.Add(paramName);
                    index++;
                }
                command.CommandText = string.Format(query, string.Join(",", idParameterList));
                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task FreeLockedTicketsAsync(int[] ids)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var query = "update ticket set inProccess = 0 where ticket_id in ({0})";
                var idParameterList = new List<string>();
                var index = 0;
                foreach (var id in ids)
                {
                    var paramName = "@idParam" + index;
                    command.Parameters.AddWithValue(paramName, id);
                    idParameterList.Add(paramName);
                    index++;
                }
                command.CommandText = string.Format(query, string.Join(",", idParameterList));
                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task<TicketViewModel[]> GetTicketsAsync(DateTime date, int routeId, string carriageType)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var tickets = new List<TicketViewModel>();
                command.CommandText = "select  t.ticket_id, t.trip_date, t.price, t.carriage_number," +
                                      " t.position_number, t.carriage_type, r.start_point, r.finish_point, t.arrival_date " +
                                      "from ticket t " +
                                      "inner join route r on t.route_id = r.route_id " +
                                      "where t.isPurchased = 0 and datediff(day, t.trip_date, @dt) = 0 and t.route_id =@rt and t.carriage_type=@ct and t.inProccess = 0";
                command.Parameters.AddWithValue("@dt", date);
                command.Parameters.AddWithValue("@rt", routeId);
                command.Parameters.AddWithValue("@ct", carriageType);
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    tickets.Add(new TicketViewModel
                    {
                        TicketId = Convert.ToInt32(dataReader[0].ToString()),
                        TripDate = Convert.ToDateTime(dataReader[1].ToString()),
                        Price = Convert.ToDecimal(dataReader[2].ToString()),
                        CarriageNumber = Convert.ToInt32(dataReader[3].ToString()),
                        PositionNumber = Convert.ToInt32(dataReader[4].ToString()),
                        CarriageType = dataReader[5].ToString(),
                        Route = $"{dataReader[6].ToString()} - {dataReader[7].ToString()}",
                        ArrivalDate = Convert.ToDateTime(dataReader[8].ToString())
                    });
                }

                return tickets.ToArray();
            }
        }

        public async Task<TicketViewModel[]> GetTicketsListAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var tickets = new List<TicketViewModel>();
                command.CommandText = "select t.ticket_id, t.trip_date, t.price, t.carriage_number," +
                                      "t.position_number, t.carriage_type, r.start_point, r.finish_point, t.isPurchased, t.arrival_date " +
                                      "from ticket t " +
                                      "inner join route r on t.route_id = r.route_id ";
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    tickets.Add(new TicketViewModel
                    {
                        TicketId = Convert.ToInt32(dataReader[0].ToString()),
                        TripDate = Convert.ToDateTime(dataReader[1].ToString()),
                        Price = Convert.ToDecimal(dataReader[2].ToString()),
                        CarriageNumber = Convert.ToInt32(dataReader[3].ToString()),
                        PositionNumber = Convert.ToInt32(dataReader[4].ToString()),
                        CarriageType = dataReader[5].ToString(),
                        Route = $"{dataReader[6].ToString()} - {dataReader[7].ToString()}",
                        IsPurchased = dataReader.GetBoolean(8) == true ? 1 : 0,
                        ArrivalDate = Convert.ToDateTime(dataReader[9].ToString())
                    });
                }

                return tickets.ToArray();
            }
        }

        public async Task<TicketViewModel> GetTicketAsync(int ticketId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var ticket = new TicketViewModel();
                ticket = null;
                command.CommandText = "select t.trip_date, t.price, t.carriage_number, t.position_number," +
                                      "r.start_point, r.finish_point, t.carriage_type, t.ticket_id, t.arrival_date " +
                                      "from ticket t "+
                                      "inner join route r on t.route_id = r.route_id "+
                                      "where t.ticket_id = @id";
                command.Parameters.AddWithValue("@id", ticketId);
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    ticket = new TicketViewModel
                    {
                        TripDate = Convert.ToDateTime(dataReader[0].ToString()),
                        Price = Convert.ToDecimal(dataReader[1].ToString()),
                        CarriageNumber = Convert.ToInt32(dataReader[2].ToString()),
                        PositionNumber = Convert.ToInt32(dataReader[3].ToString()),
                        Route = $"{dataReader[4].ToString()} - {dataReader[5].ToString()}",
                        CarriageType = dataReader[6].ToString(),
                        TicketId = Convert.ToInt32(dataReader[7].ToString()),
                        ArrivalDate = Convert.ToDateTime(dataReader[8].ToString())
                    };
                };

                connection.Close();
                return ticket;
            }
        }

        public async Task<TicketViewModel> GetTicketForCancellationAsync(int ticketId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var ticket = new TicketViewModel();
                ticket = null;
                command.CommandText = "select t.trip_date, t.price, t.carriage_number, t.position_number, t.carriage_type, t.ticket_id, o.order_id, t.arrival_date " +
                                      "from ticket t " +
                                      "inner join [order] o on t.ticket_id = o.ticket_id " +
                                      "where t.ticket_id = @id and o.isCancelled = 0";
                command.Parameters.AddWithValue("@id", ticketId);
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    ticket = new TicketViewModel
                    {
                        TripDate = Convert.ToDateTime(dataReader[0].ToString()),
                        Price = Convert.ToDecimal(dataReader[1].ToString()),
                        CarriageNumber = Convert.ToInt32(dataReader[2].ToString()),
                        PositionNumber = Convert.ToInt32(dataReader[3].ToString()),
                        CarriageType = dataReader[4].ToString(),
                        TicketId = Convert.ToInt32(dataReader[5].ToString()),
                        OrderId = Convert.ToInt32(dataReader[6].ToString()),
                        ArrivalDate = Convert.ToDateTime(dataReader[7].ToString())
                    };
                };

                connection.Close();
                return ticket;
            }
        }

    }
}