namespace AzureAD.B2C.BuildTask.Helpers
{
    using AzureAD.B2C.BuildTask.Commons;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    public class GraphClientHelper
    {
        private readonly string _graphVersion;
        private readonly string _graphResourceName;
        private readonly AuthenticationContext _authContext;
        private readonly ClientCredential _credential;

        public GraphClientHelper(string domainName, string clientid, string clientSecret, string graphVersion, string graphResourceName)
        {
            domainName += ".onmicrosoft.com";
            _graphVersion = graphVersion;
            _graphResourceName = graphResourceName;
            _graphVersion = graphVersion;
            _graphResourceName = graphResourceName;
            _authContext = new AuthenticationContext("https://login.microsoftonline.com/" + domainName);
            _credential = new ClientCredential(clientid, clientSecret);
        }

        public async Task<string> SendGraphPostRequest(string api, string xml)
        {
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Graph Client : Fetching Graph Access Token", false);
            var accessToken = GetToken(_authContext, _credential, _graphResourceName);
            Common.RaiseConsoleMessage(LogType.INFO, $"Graph Client : Successfully Fetched Access Token", false);
            using (HttpClient http = new HttpClient())
            {
                string url = _graphResourceName + _graphVersion + api;
                Common.RaiseConsoleMessage(LogType.INFO, $"Graph Client : Graph URL {url}", false);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
                Common.RaiseConsoleMessage(LogType.INFO, $"Graph Client : Calling Graph API", false);
                using (HttpResponseMessage response = await http.SendAsync(request))
                { 
                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        object formatted = JsonConvert.DeserializeObject(error);
                        throw new Exception("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
                    }
                    Common.RaiseConsoleMessage(LogType.INFO, $"Graph Client : Successfully Called", false);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private string GetToken(AuthenticationContext authContext, ClientCredential credential, string graphResourceName)
        {
            AuthenticationResult result = authContext.AcquireTokenAsync(graphResourceName, credential).GetAwaiter().GetResult();
            var accessToken = result.AccessToken;
            return accessToken;
        }
    }
}
