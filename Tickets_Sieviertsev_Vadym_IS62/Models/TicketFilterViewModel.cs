using System;
using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class TicketFilterViewModel
    {       
        public int RouteId { get; set; }
        [Display(Name = "Carriage type")]
        public string CarriageType { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}