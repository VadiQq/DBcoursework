using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tickets_Sieviertsev_Vadym_IS62.Interfaces;
using Tickets_Sieviertsev_Vadym_IS62.Models;
using Tickets_Sieviertsev_Vadym_IS62.Services;

namespace Tickets_Sieviertsev_Vadym_IS62.Controllers
{
    public class CashierController : Controller
    {
        public readonly IRouteService _routeService;
        public readonly ITicketService _ticketService;
        public readonly ICustomerService _customerService;
        public readonly IOrderService _orderService;
        public readonly IViewListMapper _listMapper;

        public CashierController()
        {
            _routeService = new RouteService();
            _ticketService = new TicketService();
            _orderService = new OrderService();
            _customerService = new CustomerService();
            _listMapper = new ViewListMapper();
        }

        [HttpGet]
        public async Task<ActionResult> RoutesList()
        {
            var routes = await _routeService.GetRoutesListAsync();

            return View(routes);
        }

        [HttpGet]
        public async Task<ActionResult> CustomersList()
        {
            var customers = await _customerService.GetCustomersListAsync();

            return View(customers);
        }

        [HttpGet]
        public async Task<ActionResult> SetTicketsOptions()
        {
            var routes = await _routeService.GetRoutesAsync();
            ViewData["Routes"] = _listMapper.MapRoutesList(routes);
            ViewData["CarriageTypes"] = _listMapper.MapCarriageTypesList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SetTicketsOptions(TicketFilterViewModel model)
        {

            model.RouteId = Convert.ToInt32(Request.Params["Routes"]);
            model.CarriageType = Request.Params["CarriageTypes"];
            if (ModelState.IsValid)
            {
                var tickets = await _ticketService.GetTicketsAsync(model.Date, model.RouteId, model.CarriageType);
                var ticketsIds = tickets.Select(t => t.TicketId).ToArray();
                if (tickets != null && tickets.Length != 0)
                {
                    await _ticketService.LockTicketsAsync(ticketsIds);
                }

                TempData["Model"] = new TicketSellViewModel
                {
                    Tickets = tickets
                };
                return RedirectToAction("SellTicket", "Cashier");
            }

            return View();
        }

        [HttpGet]
        public ActionResult SellTicket()
        {
            var model = TempData["Model"] as TicketSellViewModel;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SellTicket(string ticket, string[] blocked, TicketSellViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ticket != null)
                {
                    var customer = await _customerService.GetCustomerAsync(model.FirstName, model.LastName, model.Code);
                    if (customer != null)
                    {
                        await _orderService.CreateOrderAsync(Convert.ToInt32(ticket), customer.CustomerId);
                    }
                    else
                    {
                        await _customerService.CreateCustomerAsync(model.FirstName, model.LastName, model.Code);
                        customer = await _customerService.GetCustomerAsync(model.FirstName, model.LastName, model.Code);
                        await _orderService.CreateOrderAsync(Convert.ToInt32(ticket), customer.CustomerId);
                    }
                }                          
            }

            await _ticketService.FreeLockedTicketsAsync(Array.ConvertAll(blocked, int.Parse));
            return RedirectToAction("SetTicketsOptions");
        }

        [HttpGet]
        public ActionResult CheckOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CheckOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _orderService.GetOrderAsync(model.FirstName, model.LastName, model.Code, model.TicketId);
                return RedirectToAction("CheckTicket", "Cashier", new { id = order.TicketId });
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CheckTicket(int id)
        {
           var ticket = await _ticketService.GetTicketAsync(id);

           return View(ticket);
        }

        [HttpGet]
        public async Task<ActionResult> TicketsList(string[] blocked)
        {
            if(blocked != null)
            {
                await _ticketService.FreeLockedTicketsAsync(Array.ConvertAll(blocked, int.Parse));
            }
            var ticket = await _ticketService.GetTicketsListAsync();

            return View(ticket);
        }

        [HttpGet]
        public async Task<ActionResult> CancelOrder(int ticketId)
        {
            var ticket = await _ticketService.GetTicketForCancellationAsync(ticketId);
            await _orderService.CancelOrderAsync(ticket.TicketId, ticket.OrderId, ticket.Price);
            return RedirectToAction("TicketsList");
        }

        [HttpGet]
        public async Task<ActionResult> CreateTicket()
        {
            var routes = await _routeService.GetRoutesAsync();
            ViewData["Routes"] = _listMapper.MapRoutesList(routes);
            ViewData["CarriageTypes"] = _listMapper.MapCarriageTypesList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var route = Convert.ToInt32(Request.Params["Routes"]);
                var carriageType = Request.Params["CarriageTypes"];
                await _ticketService.CreateTicketAsync(route, carriageType, model.Price, model.TripDate, model.CarriageNumber, model.PositionNumber, model.IsPurchased, model.ArrivalDate);
            }

            var routes = await _routeService.GetRoutesAsync();
            ViewData["Routes"] = _listMapper.MapRoutesList(routes);
            ViewData["CarriageTypes"] = _listMapper.MapCarriageTypesList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CreateRoute()
        {
            ViewData["Routes"] = await _routeService.GetRoutesAsync();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoute(RouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _routeService.AddRouteAsync(model.StartPoint, model.FinishPoint);
            }
            ViewData["Routes"] = await _routeService.GetRoutesAsync();
            return View();
        }

    }
}