using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Services
{
    public class OrderService : IOrderService
    {
        private readonly string connectionString;

        public OrderService()
        {
            connectionString = System.Configuration.ConfigurationManager.
                               ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public async Task CreateOrderAsync(int ticketId, int customerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "insert into [order](ticket_id, customer_id) " +
                                      "values(@id, @cid) " +
                                      "update ticket set isPurchased = 1 where ticket_id = @id";
                command.Parameters.AddWithValue("@cid", customerId);
                command.Parameters.AddWithValue("@id", ticketId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task<OrderViewModel> GetOrderAsync(string firtsName, string lastName, string code, int ticketId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                OrderViewModel order = new OrderViewModel();
                order = null;
                command.CommandText = "select first_name, last_name, code, ticket_id " +
                                      "from [order] o " +
                                      "inner join customer c on o.customer_id = c.customer_id " +
                                      "where first_name = @fn and last_name = @ln and code = @cd and ticket_id=@id and isCancelled = 0";
                command.Parameters.AddWithValue("@fn", firtsName);
                command.Parameters.AddWithValue("@ln", lastName);
                command.Parameters.AddWithValue("@cd", code);
                command.Parameters.AddWithValue("@id", ticketId);
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    order = new OrderViewModel
                    {
                        FirstName = dataReader[0].ToString(),
                        LastName = dataReader[1].ToString(),
                        Code = dataReader[2].ToString(),
                        TicketId = Convert.ToInt32(dataReader[3].ToString())
                    };
                }

                connection.Close();
                return order;
            }
        }

        public async Task CancelOrderAsync(int ticketId, int orderId, decimal price)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                OrderViewModel order = new OrderViewModel();
                order = null;
                command.CommandText = "update [order] set isCancelled = 1 where ticket_id =@id " +
                                      "update ticket set isPurchased = 0 where ticket_id=@id " +
                                      "insert into cancellation(order_id,price,penalty) " +
                                      "values(@oid,@pr,@pn) ";
                command.Parameters.AddWithValue("@id", ticketId);
                command.Parameters.AddWithValue("@oid", orderId);
                command.Parameters.AddWithValue("@pr", price);
                command.Parameters.AddWithValue("@pn", 0.03);
                connection.Open();
                await command.ExecuteNonQueryAsync();         
                connection.Close();            
            }
        }
    }
}