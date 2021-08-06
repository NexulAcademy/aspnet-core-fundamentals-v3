using System;

namespace SimpleCrm.WebApi.Models.Auth
{
    public class UserSummaryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string JwtToken { get; set; }
        public string[] Roles { get; set; }
        //TODO: add other app specific capabilities for the user
        // one option...
        public int AccountId { get; set; }
    }
}
