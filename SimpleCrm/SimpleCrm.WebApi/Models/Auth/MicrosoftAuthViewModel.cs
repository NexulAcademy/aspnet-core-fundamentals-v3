namespace SimpleCrm.WebApi.Models.Auth
{
    public class MicrosoftAuthViewModel
    {
        public string AccessToken { get; set; }
        public string State { get; set; }
        public string BaseHref { get; set; }
    }
}
