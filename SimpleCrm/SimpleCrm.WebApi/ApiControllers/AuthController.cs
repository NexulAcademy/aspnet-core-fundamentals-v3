using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Models.Auth;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.ApiControllers
{
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        { // TODO: create a CredentialsViewModel class in the next assignment
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            // TODO: add Authenticate method (see lesson below)
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
    }
}
