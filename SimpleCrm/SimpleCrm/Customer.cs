// /SimpleCrm/Customer.cs
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm
{
    public class Customer
    {
        public int Id { get; set; }
        [MaxLength(50)] // <-- new
        [Required()] // <-- new
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(50)] // <-- new
        [Required()] // <-- new
        public string LastName { get; set; }
        [MinLength(7), MaxLength(12)] // <-- new
        public string PhoneNumber { get; set; }
        public bool OptInNewsletter { get; set; } //New
        public CustomerType Type { get; set; } //New
    }
}
