using System.ComponentModel.DataAnnotations;

namespace Tickets_Sieviertsev_Vadym_IS62.Models
{
    public class CustomersListViewModel
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Cancellations")]
        public int Cancellation { get; set; }
        [Display(Name = "Purchased tickets")]
        public int PurchasedTickets { get; set; }
    }
}