using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int ticketId,int customerId);
        Task<OrderViewModel> GetOrderAsync(string firtsName, string lastName, string code, int ticketId);
        Task CancelOrderAsync(int ticketId, int orderId, decimal price);
    }
}