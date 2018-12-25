using System;
using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        [Display(Name = "Carriage type")]
        public string CarriageType { get; set; }
        [Display(Name = "Trip date")]
        public DateTime TripDate { get; set; }
        [Display(Name = "Arrival date")]
        public DateTime ArrivalDate { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Sold")]
        public int IsPurchased { get;set; }
        [Display(Name = "Carriage number")]
        public int CarriageNumber { get; set; }
        [Display(Name = "Position number")]
        public int PositionNumber { get; set; }
        public string Route { get; set; }
        public int OrderId { get; set; }
    }
}