using System;
using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Interfaces
{
    public interface ITicketService
    {
        Task CreateTicketAsync(int routeId, string carriageType, decimal price, DateTime tripDate,int carriageNumber,int positionNumber, int isPurchased, DateTime arrivalDate);
        Task<TicketViewModel[]> GetTicketsAsync(DateTime date, int routeId, string carriageType);
        Task LockTicketsAsync(int[] ids);
        Task FreeLockedTicketsAsync(int[] ids);
        Task<TicketViewModel> GetTicketAsync(int id);
        Task<TicketViewModel> GetTicketForCancellationAsync(int ticketId);
        Task<TicketViewModel[]> GetTicketsListAsync();
    }
}