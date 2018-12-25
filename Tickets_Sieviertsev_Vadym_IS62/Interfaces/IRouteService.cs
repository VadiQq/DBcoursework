using System.Threading.Tasks;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Interfaces
{
    public interface IRouteService
    {
        Task AddRouteAsync(string startPoint, string finishPoint);
        Task<RouteViewModel[]> GetRoutesAsync();
        Task<RoutesListViewModel[]> GetRoutesListAsync();
    }
}