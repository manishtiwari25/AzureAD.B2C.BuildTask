using AzureAD.B2C.BuildTask.Commons;
using AzureAD.B2C.BuildTask.Helpers;
using System;
using System.Net.Http;

namespace AzureAD.B2C.BuildTask.Tasks
{
    public class ReleasePolicies : Task
    {
        private readonly string artifactPublishPath;
        private readonly Error _error;
        private readonly GraphClientHelper _graphHelper;
        private readonly KeysetHelper _keysetHelper;
        public ReleasePolicies(string b2CDomain, string clientId, string clientSecret, string artifactPublishPath)
        {
            _error = new Error();
            ValidateArguments(new string[] { b2CDomain, clientId, clientSecret });
            this.artifactPublishPath = artifactPublishPath;

            Common.RaiseConsoleMessage(LogType.INFO, $"Please Note We Are Using Beta Version of Graph api", false);
            _graphHelper = new GraphClientHelper(b2CDomain, clientId, clientSecret, "beta", "https://graph.microsoft.com/");
            _keysetHelper = new KeysetHelper(_graphHelper);
        }
        public Error Release()
        {
            if (!_error.AnyError)
            {
                try
                {
                    _keysetHelper.HandleKeysets();
                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Fetch Policies : Fetching Policies", false);
                    var policies = XMLHelper.FetchXmlFromDirectory(artifactPublishPath, _error);
                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Fetch Policies : Successfully Fetched", false);
                    XMLHelper.MoveFirst(policies);
                    foreach (var policy in policies)
                    {
                        var tempPolicyPathArray = policy.Split('\\');
                        var policyName = tempPolicyPathArray[tempPolicyPathArray.Length - 1].Split('.')[0];
                        Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Updating B2C Custom Policy {policyName}", false);

                        var xmlData = XMLHelper.ReadFromXML(policy, _error);

                        var api = string.Format("/trustFramework/policies/B2C_1A_{0}/$value", policyName);

                        Common.RaiseConsoleMessage(LogType.INFO, $"Update Policy Values : Successfully Updated B2C Custom Policy {policyName}", false);
                        Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Uploading Policy {policyName} in B2C tenant", false);
                        var resp = _graphHelper.SendGraphPostRequest(HttpMethod.Put, api, xmlData, isJson: false).GetAwaiter().GetResult();
                        Common.RaiseConsoleMessage(LogType.INFO, $"Update Policy Values : Successfully Uploaded", false);
                    }
                }
                catch (Exception ex)
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
                    _error.ErrorMessage = $"Something Went Wrong : {ex.Message}";
                    _error.AnyError = true;
                }
            }
            return _error;
        }
        public void ValidateArguments(string[] arguments)
        {
            //domainName (B2C Domain)
            if (string.IsNullOrEmpty(arguments[0]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide B2C Domain", false);
                _error.AnyError = true;
            }
            //clientid
            if (string.IsNullOrEmpty(arguments[1]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Client Id of Application you have registered in b2c tenat", false);
                _error.AnyError = true;
            }
            //client secret
            if (string.IsNullOrEmpty(arguments[2]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Client Secret of Application you have registered in b2c tenat", false);
                _error.AnyError = true;
            }

            _error.ErrorMessage = _error.AnyError ? "Input Validation Error" : string.Empty;
        }
    }
}
