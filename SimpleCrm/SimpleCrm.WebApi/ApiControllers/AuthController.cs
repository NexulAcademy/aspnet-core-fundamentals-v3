using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleCrm.WebApi.Auth;
using SimpleCrm.WebApi.Models.Auth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<CrmUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly MicrosoftAuthSettings _microsoftAuthSettings;

        public AuthController(
            UserManager<CrmUser> userManager,
            IJwtFactory jwtFactory,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IOptions<MicrosoftAuthSettings> microsoftAuthSettings
        )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _configuration = configuration;
            _logger = logger;
            _microsoftAuthSettings = microsoftAuthSettings.Value;
        }

        [HttpGet("external/microsoft")]
        public IActionResult GetMicrosoft()
        {
            return Ok(new
            {   //this is the public application id, don't return the secret 'Password' here!
                client_id = _microsoftAuthSettings.ClientId,
                scope = "https://graph.microsoft.com/user.read",
                state = "" //arbitrary state to return again for this user
            });
        }

        [HttpPost("external/microsoft")]
        public async Task<IActionResult> PostMicrosoft([FromBody] MicrosoftAuthViewModel model)
        {
            var verifier = new MicrosoftAuthVerifier<AuthController>(_microsoftAuthSettings, _configuration["HttpHost"] + (model.BaseHref ?? "/"), _logger);
            var profile = await verifier.AcquireUser(model.AccessToken);

            if (!profile.IsSuccessful)
            {
                _logger.LogWarning("ExternalLoginCallback() unknown error at external login provider, {profile.Error.Message}", profile.Error.Message);
                return StatusCode(StatusCodes.Status400BadRequest, profile.Error.Message);
            }
            var info = new UserLoginInfo("Microsoft", profile.Id, "Microsoft");
            if (info == null || info.ProviderKey == null || info.LoginProvider == null)
            {
                _logger.LogWarning("ExternalLoginCallback() unknown error at external login provider");
                return StatusCode(StatusCodes.Status400BadRequest, "Unknown error at external login provider");
            }

            if (string.IsNullOrWhiteSpace(profile.Mail))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Email address not available from Login provider, cannot proceed.");
            }

            // ready to create the local user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(profile.Mail);
            if (user == null)
            {
                var appUser = new CrmUser
                {
                    DisplayName = profile.DisplayName,
                    Email = profile.Mail,
                    UserName = profile.Mail,
                    PhoneNumber = profile.MobilePhone
                };

                var password = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8) + "#1aA";
                // #1aA ensures all required character types will be in the random password
                var identityResult = await _userManager.CreateAsync(appUser, password);
                if (!identityResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Could not create user.");
                }

                user = await _userManager.FindByEmailAsync(profile.Mail);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Failed to create local user account.");
                }
            }

            var userModel = await GetUserData(user);
            return Ok(userModel);
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

            var userModel = await GetUserData(user);
            return Ok(userModel);
        }

        [Authorize(Policy = "ApiUser")]
        [HttpPost] // POST api/auth/verify
        [Route("verify")]
        public async Task<IActionResult> Verify()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.Claims.Single(c => c.Type == "id");
                var user = _userManager.Users.FirstOrDefault(x => x.Id.ToString() == userIdClaim.Value);
                if (user == null)
                    return Forbid();

                var userModel = await GetUserData(user);
                return new ObjectResult(userModel);
            }

            return Forbid();
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

        private async Task<UserSummaryViewModel> GetUserData(CrmUser user)
        {
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                roles.Add("prospect");
            }

            // generate the jwt for the local user...
            var jwt = await _jwtFactory.GenerateEncodedToken(user.UserName,
                _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString()));
            var userModel = new UserSummaryViewModel
            {   //JWT could inject all these properties instead of creating a model,
                //but a model is a little easier to access from client code without
                //decoding the token. When this user model starts to contain arrays
                //of complex data, including it all in the JWT value can get complicated.
                Id = user.Id,
                Name = user.DisplayName,
                EmailAddress = user.Email,
                JwtToken = jwt,
                Roles = roles.ToArray(), //each role could be a separate claim in the JWT
                AccountId = 0 //TODO: load this from registration data
            };
            return userModel;
        }
    }
}
