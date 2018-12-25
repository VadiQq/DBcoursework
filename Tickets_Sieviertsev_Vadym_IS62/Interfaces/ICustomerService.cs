using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerViewModel> GetCustomerAsync(string firstName, string lastName, string code);
        Task CreateCustomerAsync(string firstName, string lastName, string code);
        Task<CustomersListViewModel[]> GetCustomersListAsync();
    }
}