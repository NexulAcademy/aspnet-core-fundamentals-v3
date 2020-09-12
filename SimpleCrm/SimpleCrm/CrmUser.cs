using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm
{
    public class CrmUser : IdentityUser
    {
        [MaxLength(256)]
        public string DisplayName { get; set; }
    }
}
