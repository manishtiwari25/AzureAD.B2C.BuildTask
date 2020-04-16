using AzureAD.B2C.BuildTask.Commons;
using AzureAD.B2C.BuildTask.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace AzureAD.B2C.BuildTask.Tasks
{
    public class BuildPolicies : Task
    {
        private readonly string _json;
        private readonly Error _error;
        private readonly JsonHelper _jsonHelper;
        private readonly string _artifactPublishPath;
        private readonly string _directoryPath;
        public BuildPolicies(string directoryPath, string jSON, string artifactPublishPath)
        {
            _error = new Error();
            ValidateArguments(new string[] { directoryPath, jSON });
            _json = jSON;
            _jsonHelper = new JsonHelper();
            _directoryPath = directoryPath;
            _artifactPublishPath = artifactPublishPath;
        }

        public Error Build()
        {
            if (!_error.AnyError)
            {
                try
                {

                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Updating the values", false);
                    var jsonProperties = _jsonHelper.GetAllPropertiesFromJson(_json);
                    if (!string.IsNullOrEmpty(_json))
                    {
                        Common.RaiseConsoleMessage(LogType.DEBUG, $"Fetch Policies : Fetching Policies", false);
                        var policies = XMLHelper.FetchXmlFromDirectory(_directoryPath, _error);
                        Common.RaiseConsoleMessage(LogType.DEBUG, $"Fetch Policies : Successfully Fetched", false);
                        XMLHelper.MoveFirst(policies);
                        foreach (var policy in policies)
                        {
                            var tempPolicyPathArray = policy.Split('\\');
                            var policyName = tempPolicyPathArray[tempPolicyPathArray.Length - 1].Split('.')[0];
                            Common.RaiseConsoleMessage(LogType.DEBUG, $"Update Policy Values : Updating B2C Custom Policy {policyName}", false);
                            var xmlData = XMLHelper.ReadFromXML(policy, _error);
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
                            Common.RaiseConsoleMessage(LogType.DEBUG, $"Save Policy in Artifect Folder", false);
                            XMLHelper.WriteXMLInFolder(_artifactPublishPath, policyName, updatedPolicy, _error);
                            Common.RaiseConsoleMessage(LogType.DEBUG, $"SuccessFully Saved", false);
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

        public void ValidateArguments(string[] arguments)
        {
            if (string.IsNullOrEmpty(arguments[0]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Path Value", false);
                _error.AnyError = true;
            }

            if (string.IsNullOrEmpty(arguments[1]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide JSON", false);
                _error.AnyError = true;
            }
            _error.ErrorMessage = _error.AnyError ? "Input Validation Error" : string.Empty;
        }
    }
}
