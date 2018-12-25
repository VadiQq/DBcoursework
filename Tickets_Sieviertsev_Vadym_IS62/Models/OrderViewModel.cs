using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Required]
        public int TicketId { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}