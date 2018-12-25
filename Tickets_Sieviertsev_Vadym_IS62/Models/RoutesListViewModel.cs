using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class RoutesListViewModel
    {
        [Display(Name = "Start city")]
        public string StartPoint { get; set; }
        [Display(Name = "Finish city")]
        public string FinishPoint { get; set; }
        [Display(Name = "Amount of tickets for route")]
        public int TicketsNumber { get; set; }
        [Display(Name = "Amount of sold tickets for route")]
        public int SoldTicketsNumber { get; set; }
    }
}