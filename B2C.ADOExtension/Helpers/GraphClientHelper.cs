namespace B2C.ADOExtension.Helpers
{
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    public class GraphClientHelper
    {
        private readonly string _clientId;
        private readonly string _domainName;
        private readonly string _graphVersion;
        private readonly string _graphResourceName;
        public GraphClientHelper(string domainName, string clientid, string graphVersion, string graphResourceName)
        {
            // The client_id, client_secret, and tenant are pulled in from the App.config file
            _domainName = domainName + ".onmicrosoft.com";
            _clientId = clientid;
            _graphVersion = graphVersion;
            _graphResourceName = graphResourceName;
        }
        private string GetToken()
        {
            AuthenticationContext ADALIdentityClientApp = new AuthenticationContext("https://login.microsoftonline.com/" + _domainName);
            var authResult = ADALIdentityClientApp.AcquireTokenAsync("https://graph.microsoft.com", _clientId, new Uri("https://b2capi.com"), new PlatformParameters(PromptBehavior.Auto)).GetAwaiter().GetResult();
            return authResult.AccessToken;
        }

        public async Task<string> SendGraphPostRequest(string api, string xml)
        {
            var accessToken = GetToken();
            HttpClient http = new HttpClient();
            string url = _graphResourceName + _graphVersion + api;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new Exception("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
