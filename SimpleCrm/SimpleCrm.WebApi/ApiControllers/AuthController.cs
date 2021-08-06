using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Models.Auth;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.ApiControllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<CrmUser> _userManager;

        public AuthController(UserManager<CrmUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var user = await Authenticate(credentials.EmailAddress, credentials.Password);
            if (user == null)
            {
                return UnprocessableEntity("Invalid username or password.");
            }

            // TODO: add GetUserData method (see lesson below)
            var userModel = await GetUserData(user);
            // returns a UserSummaryViewModel containing a JWT and other user info
            return Ok(userModel);
        }

        private async Task<CrmUser> Authenticate(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
                return await Task.FromResult<CrmUser>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(emailAddress);

            if (userToVerify == null) return await Task.FromResult<CrmUser>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(userToVerify);
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<CrmUser>(null);
        }
    }
}
