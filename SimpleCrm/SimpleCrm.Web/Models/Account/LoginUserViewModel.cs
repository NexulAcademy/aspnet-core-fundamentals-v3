using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.Web.Models.Account
{
    public class LoginUserViewModel
    {
        [Required, MaxLength(256), DisplayName("Email Address"), DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
