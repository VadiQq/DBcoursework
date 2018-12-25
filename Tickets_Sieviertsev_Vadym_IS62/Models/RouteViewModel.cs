using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class RouteViewModel
    {
        public int RouteId { get; set; }
        [Required]
        [Display(Name ="Start city")]
        public string StartPoint { get; set; }
        [Display(Name = "Finish city")]
        [Required]
        public string FinishPoint { get; set; }
    }
}