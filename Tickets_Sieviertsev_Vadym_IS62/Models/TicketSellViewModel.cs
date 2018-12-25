using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class TicketSellViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        public string Code { get; set; }
        public TicketViewModel[] Tickets { get; set; }        
    }
}