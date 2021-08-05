namespace SimpleCrm.WebApi.Auth
{
    public static class Constants
    {
        public static class JwtClaimIdentifiers
        {
            /// <summary>
            /// Represents the users role, see JwtClaims for possible role values.
            /// </summary>
            public const string Rol = "rol";
            /// <summary>
            /// Represents the user's unique identity
            /// </summary>
            public const string Id = "id";
        }
        public static class JwtClaims
        {
            /// <summary>
            /// Claim gives user access to basic API access.
            /// </summary>
            public const string ApiAccess = "api_access";
        }
    }
}
