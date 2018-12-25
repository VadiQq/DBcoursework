using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly string connectionString;

        public CustomerService()
        {
            connectionString = System.Configuration.ConfigurationManager.
                               ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public async Task CreateCustomerAsync(string firstName, string lastName, string code)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "insert into customer(first_name,last_name,code) " +
                                       "values(@fn,@ln,@cd)";
                command.Parameters.AddWithValue("@fn", firstName);
                command.Parameters.AddWithValue("@ln", lastName);
                command.Parameters.AddWithValue("@cd", code);
                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public async Task<CustomerViewModel> GetCustomerAsync(string firstName, string lastName, string code)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                CustomerViewModel customer = new CustomerViewModel();
                customer = null;
                command.CommandText = "select customer_id, first_name, last_name, code from customer " +
                                      "where first_name=@fn and last_name=@ln and code=@cd";
                command.Parameters.AddWithValue("@fn", firstName);
                command.Parameters.AddWithValue("@ln", lastName);
                command.Parameters.AddWithValue("@cd", code);
                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    customer = new CustomerViewModel
                    {
                        CustomerId = Convert.ToInt32(dataReader[0].ToString()),
                        FirstName = dataReader[1].ToString(),
                        LastName = dataReader[2].ToString(),
                        Code = dataReader[3].ToString()
                    };
                }

                connection.Close();
                return customer;
            }
        }

        public async Task<CustomersListViewModel[]> GetCustomersListAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                var customers = new List<CustomersListViewModel>();
                command.CommandText = "select c.first_name, c.last_name,c.code," +
                                      "sum(iif(ca.cancellation_id is not null, 1 ,0)) as Cancellations, count(t.ticket_id) as [Purchased tickets] " +
                                      "from customer c " +
                                      "inner join[order] o on c.customer_id = o.customer_id " +
                                      "inner join ticket t on o.ticket_id = t.ticket_id " +
                                      "left  join cancellation ca on o.order_id = ca.order_id " +
                                      "group by c.first_name,c.last_name,c.code";

                connection.Open();
                var dataReader = await command.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    customers.Add(new CustomersListViewModel
                    {
                        FirstName = dataReader[0].ToString(),
                        LastName = dataReader[1].ToString(),
                        Code = dataReader[2].ToString(),
                        Cancellation = Convert.ToInt32(dataReader[3].ToString()),
                        PurchasedTickets = Convert.ToInt32(dataReader[4].ToString())
                    });
                }

                connection.Close();
                return customers.ToArray();
            }
        }
    }
}