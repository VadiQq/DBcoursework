using System.Web.Mvc;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] MapRoutesList(RouteViewModel[] routes, int? selectedId = null);
        SelectListItem[] MapCarriageTypesList(string selectedType = null);
    }
}