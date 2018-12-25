using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Services
{
    public class RouteService : IRouteService
    {
        private readonly string connectionString;

        public RouteService()
        {
            connectionString = System.Configuration.ConfigurationManager.
                               ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public async Task AddRouteAsync(string startPoint, string finishPoint)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "insert into route(start_point,finish_point) VALUES(@st, @fn)";
                command.Parameters.AddWithValue("@st", startPoint);
                command.Parameters.AddWithValue("@fn", finishPoint);
                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task<RouteViewModel[]> GetRoutesAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var routes = new List<RouteViewModel>();

                command.CommandText = "select * from route";

                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();             
                while (dataReader.Read())
                {
                    routes.Add(new RouteViewModel
                    {
                        RouteId = Convert.ToInt32(dataReader[0].ToString()),
                        StartPoint = dataReader[1].ToString(),
                        FinishPoint = dataReader[2].ToString()
                });
                }

                connection.Close();
                return routes.ToArray();
            }
        }

        public async Task<RoutesListViewModel[]> GetRoutesListAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var routes = new List<RoutesListViewModel>();
                command.CommandText = "select r.start_point, r.finish_point, count(t.ticket_id) as Tickets, sum(iif(isPurchased is null,0,iif(isPurchased = 0,0,1))) as [Sold Tickets] " +
                                      "from[route] r " +
                                      "left join ticket t on r.route_id = t.route_id " +
                                      "group by r.finish_point, r.start_point";
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    routes.Add(new RoutesListViewModel
                    {
                        StartPoint = dataReader[0].ToString(),
                        FinishPoint = dataReader[1].ToString(),
                        TicketsNumber = Convert.ToInt32(dataReader[2].ToString()),
                        SoldTicketsNumber = Convert.ToInt32(dataReader[3].ToString())
                    });
                }

                connection.Close();
                return routes.ToArray();
            }
        }
        
    }
}