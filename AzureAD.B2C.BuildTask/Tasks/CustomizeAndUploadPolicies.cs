namespace AzureAD.B2C.BuildTask.Tasks
{
    using AzureAD.B2C.BuildTask.Commons;
    using AzureAD.B2C.BuildTask.Helpers;
    using AzureAD.B2C.BuildTask.Modles;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    public class CustomizeAndUploadPolicies : Task
    {
        private readonly JsonHelper _jsonHelper;
        private readonly XMLHelper _xmlHelper;
        private readonly string _json;
        private readonly GraphClientHelper _graphHelper;
        private readonly KeysetHelper _keysetHelper;
        private readonly Error _error;

        public CustomizeAndUploadPolicies(string[] arguments)
        {
            _error = new Error();
            ValidateArguments(arguments);
            _jsonHelper = new JsonHelper(arguments[0]);
            _xmlHelper = new XMLHelper(arguments[0]);
            _json = arguments[4];
            Common.RaiseConsoleMessage(LogType.INFO, $"Please Note We Are Using Beta Version of Graph api", false);
            _graphHelper = new GraphClientHelper(arguments[1], arguments[2], arguments[3], "beta", "https://graph.microsoft.com/");
            _keysetHelper = new KeysetHelper(_graphHelper);
        }

        public Error UpdateValues()
        {
            if (!_error.AnyError)
            {
                try
                {
                    _keysetHelper.HandleKeysets();
                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Updating the values", false);
                    var jsonProperties = _jsonHelper.GetAllPropertiesFromJson(_json);
                    if (!string.IsNullOrEmpty(_json))
                    {
                        var policies = _xmlHelper.FetchXmlFromDirectory();
                        _xmlHelper.MoveFirst(policies);
                        foreach (var policy in policies)
                        {
                            var tempPolicyPathArray = policy.Split('\\');
                            var policyName = tempPolicyPathArray[tempPolicyPathArray.Length - 1].Split('.')[0];
                            Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Updating B2C Custom Policy {policyName}", false);
                            var xmlData = _xmlHelper.ReadFromXML(policy);
                            var tenantInfo = _jsonHelper.GetValueFromJson(_json, "Tenant");
                            var updatedPolicy = Regex.Replace(xmlData, "{Settings:Tenant}", tenantInfo);
                            if (jsonProperties != null)
                            {
                                foreach (JProperty property in jsonProperties)
                                {
                                    var propertyValue = (string)property.Value;
                                    updatedPolicy = Regex.Replace(updatedPolicy, "{Settings:" + property.Name + "}", propertyValue);
                                }
                            }

                            var api = string.Format("/trustFramework/policies/B2C_1A_{0}/$value", policyName);

                            Common.RaiseConsoleMessage(LogType.INFO, $"Update Policy Values : Successfully Updated B2C Custom Policy {policyName}", false);
                            Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Uploading Policy {policyName} in B2C tenant", false);
                            var resp = _graphHelper.SendGraphPostRequest(HttpMethod.Put, api, updatedPolicy, isJson: false).GetAwaiter().GetResult();
                            Common.RaiseConsoleMessage(LogType.INFO, $"Update Policy Values : Successfully Uploaded", false);
                        }
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

        public override void ValidateArguments(string[] arguments)
        {
            if (string.IsNullOrEmpty(arguments[5]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Task Type (Build/Release)", false);
                _error.AnyError = true;
            }
            else
            {
                //directory uri
                if (arguments[5] == "Build" && string.IsNullOrEmpty(arguments[0]))
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Path Value", false);
                    _error.AnyError = true;
                }
                //domainName (B2C Domain)
                if (arguments[5] == "Release" && string.IsNullOrEmpty(arguments[1]))
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide B2C Domain", false);
                    _error.AnyError = true;
                }
                //clientid
                if (arguments[5] == "Release" && string.IsNullOrEmpty(arguments[2]))
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Client Id of Application you have registered in b2c tenat", false);
                    _error.AnyError = true;
                }
                //client secret
                if (arguments[5] == "Release" && string.IsNullOrEmpty(arguments[3]))
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Client Secret of Application you have registered in b2c tenat", false);
                    _error.AnyError = true;
                }
                //json
                if (arguments[5] == "Build" && string.IsNullOrEmpty(arguments[4]))
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide JSON", false);
                    _error.AnyError = true;
                }
            }

            _error.ErrorMessage = _error.AnyError ? "Input Validation Error" :string.Empty;
        }
    }
}
