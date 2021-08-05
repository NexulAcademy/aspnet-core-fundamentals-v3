using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.Auth
{
    public interface IJwtFactory
    {
        /// <summary>
        /// Generates a full JWT token
        /// </summary>
        /// <param name="userName">The name of the user ("sub" or subject of the token)</param>
        /// <param name="identity">An identity, usually created with GenerateClaimsIdentity.</param>
        /// <returns></returns>
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        /// <summary>
        /// Generate an identity to be placed in a token that contains the ApiAccess role claim.
        /// </summary>
        /// <param name="userName">The identity or 'user name' of the authenticated user.</param>
        /// <param name="id">The user's unique identity.</param>
        /// <returns></returns>
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
