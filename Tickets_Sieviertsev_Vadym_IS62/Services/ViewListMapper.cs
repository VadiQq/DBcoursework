using System.Linq;
using System.Web.Mvc;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;

namespace Tickets_Sieviertsev_Vadym_IS62.Services
{
    public class ViewListMapper : IViewListMapper
    {
        public SelectListItem[] MapRoutesList(RouteViewModel[] routes, int? selectedId = null)
        {
            return routes.Select(r => new SelectListItem
            {
                Text = $"{r.StartPoint} - {r.FinishPoint}",
                Value = r.RouteId.ToString(),
                Selected = r.RouteId == selectedId
            }).ToArray();
        }


        public SelectListItem[] MapCarriageTypesList(string selectedType = null)
        {
            return new SelectListItem[]
            {
                new SelectListItem
                {
                    Text = "Close",
                    Value = "Close",
                    Selected = "Close" == selectedType
                },
                new SelectListItem
                {
                    Text = "Open",
                    Value = "Open",
                    Selected = "Open" == selectedType
                },
                new SelectListItem
                {
                    Text = "Private",
                    Value = "Private",
                    Selected = "Private" == selectedType
                }
            };
        }
    }
}