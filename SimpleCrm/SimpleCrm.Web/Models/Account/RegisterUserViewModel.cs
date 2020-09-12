using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.Web.Models.Account
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(256), DisplayName("Email Address"), DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required, MaxLength(256), DisplayName("Name")]
        public string DisplayName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
