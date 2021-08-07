using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleCrm.WebApi.Auth
{
    public class MicrosoftAuthVerifier<T>
    {
        private readonly MicrosoftAuthSettings _microsoftAuthSettings;
        private readonly string _host;
        private readonly ILogger<T> _logger;

        public MicrosoftAuthVerifier(MicrosoftAuthSettings microsoftAuthSettings, string host,
            ILogger<T> logger)
        {
            _microsoftAuthSettings = microsoftAuthSettings;
            _host = host;
            _logger = logger;
        }
        public async Task<MicrosoftUserProfile> AcquireUser(string token)
        {
            try
            {
                var client = new HttpClient();

                var tokenRequestParameters = new Dictionary<string, string>()
                {
                    { "client_id", _microsoftAuthSettings.ClientId },
                    { "client_secret", _microsoftAuthSettings.ClientSecret },
                    { "redirect_uri", _host + "signin-microsoft" },
                    { "code", token },
                    { "grant_type", "authorization_code" }
                };
                var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                    "https://login.microsoftonline.com/common/oauth2/v2.0/token");
                requestMessage.Headers.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Content = requestContent;
                var response = await client.SendAsync(requestMessage);
                var payloadStr = await response.Content.ReadAsStringAsync();
                var payload = JObject.Parse(payloadStr);
                //{"token_type":"Bearer","scope":"https://graph.microsoft.com/User.Read","expires_in":3599,"ext_expires_in":0,"access_token":"eyJ...Apg"}

                if (payload["error"] != null)
                {   // malformed request
                    // //{"error": "invalid_client","error_description": "AADSTS70002: Error validating credentials. AADSTS50011: The reply address 'localhost:56798' does not match the reply address 'http://localhost:56798/signin-microsoft' provided when requesting Authorization code.\r\nTrace ID: eb13c239-544a-463c-b954-b966177a4d00\r\nCorrelation ID: 5972eaab-9256-4de3-8604-82409ef00f28\r\nTimestamp: 2018-04-11 16:44:56Z","error_codes": [70002,50011],"timestamp": "2018-04-11 16:44:56Z","trace_id": "eb13c239-544a-463c-b954-b966177a4d00","correlation_id": "5972eaab-9256-4de3-8604-82409ef00f28"}
                    var err = payload["error"];
                    _logger.LogWarning("Microsoft token error response: {0}", payloadStr);
                    return new MicrosoftUserProfile
                    {
                        IsSuccessful = false,
                        Error = new OAuthError
                        {
                            Code = payload.Value<string>("error"),
                            Message = payload.Value<string>("error_description")
                        }
                    };
                }

                _logger.LogInformation("Payload details: {0}", payloadStr);


                var graphMessage = new HttpRequestMessage(HttpMethod.Get,
                    "https://graph.microsoft.com/v1.0/me");
                graphMessage.Headers.Add("Authorization", "Bearer " + payload["access_token"]);
                var graphResponse = await client.SendAsync(graphMessage);
                var graphPayloadStr = await graphResponse.Content.ReadAsStringAsync();
                var graphPayload = JObject.Parse(graphPayloadStr);
                //{"@odata.context":"https://graph.microsoft.com/v1.0/$metadata#users/$entity","id":"ea0...070","businessPhones":[],"displayName":"Michael Lang","givenName":"Michael","jobTitle":null,"mail":"michael.lang@nexulacademy.com","mobilePhone":null,"officeLocation":null,"preferredLanguage":null,"surname":"Lang","userPrincipalName":"michael.lang@nexulacademy.com"}

                if (graphPayload["error"] != null)
                {   // malformed request or bad data
                    //{"error": {"code": "BadRequest","message": "Write requests (excluding DELETE) must contain the Content-Type header declaration.","innerError": {"request-id": "66839673-a267-46d9-ba42-5091b57ecc28","date": "2018-04-11T16:47:30"}}}
                    var err = graphPayload["error"];
                    _logger.LogWarning("Microsoft graph error response: {0}", graphPayloadStr);
                    return new MicrosoftUserProfile
                    {
                        IsSuccessful = false,
                        Error = new OAuthError
                        {
                            Code = err.Value<string>("code"),
                            Message = err.Value<string>("message")
                        }
                    };
                }

                var profile = new MicrosoftUserProfile
                {
                    IsSuccessful = true,
                    Context = graphPayload.Value<string>("@odata.context"),
                    Id = graphPayload.Value<string>("id"),
                    //BusinessPhones = graphPayload.Value<string[]>("businessPhones"),
                    // TODO: ^ "Cannot cast Newtonsoft.Json.Linq.JArray to Newtonsoft.Json.Linq.JToken."
                    DisplayName = graphPayload.Value<string>("displayName"),
                    GivenName = graphPayload.Value<string>("givenName"),
                    JobTitle = graphPayload.Value<string>("jobTitle"),
                    Mail = graphPayload.Value<string>("mail"),
                    MobilePhone = graphPayload.Value<string>("mobilePhone"),
                    OfficeLocation = graphPayload.Value<string>("officeLocation"),
                    PreferredLanguage = graphPayload.Value<string>("preferredLanguage"),
                    Surname = graphPayload.Value<string>("surname"),
                    UserPrincipalName = graphPayload.Value<string>("userPrincipalName")
                };
                if (string.IsNullOrWhiteSpace(profile.Mail))
                {
                    profile.Mail = profile.UserPrincipalName;
                }
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {0} Details: {1}", ex, ex.StackTrace);
                throw;
            }
        }
    }

    public interface IOAuthUserProfile
    {
        bool IsSuccessful { get; set; }
        OAuthError Error { get; set; }
        string Id { get; set; }
        string Mail { get; set; }
        string JobTitle { get; set; }
        string DisplayName { get; set; }
        string MobilePhone { get; set; }
    }
    public class MicrosoftUserProfile : IOAuthUserProfile
    {
        public bool IsSuccessful { get; set; }
        public OAuthError Error { get; set; }
        public string Context { get; set; }
        public string Id { get; set; }
        public string[] BusinessPhones { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string JobTitle { get; set; }
        public string Mail { get; set; }
        public string MobilePhone { get; set; }
        public string OfficeLocation { get; set; }
        public string PreferredLanguage { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
    }

    public class OAuthError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
