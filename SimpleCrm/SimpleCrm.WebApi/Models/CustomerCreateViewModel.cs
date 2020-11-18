using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.WebApi.Models
{
    public class CustomerCreateViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required, EmailAddress]
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
    }
}
