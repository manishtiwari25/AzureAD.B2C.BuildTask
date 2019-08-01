namespace B2C.ADOExtension.Tasks
{
    using B2C.ADOExtension.Commons;
    using B2C.ADOExtension.Helpers;
    using System;
    using System.Text.RegularExpressions;
    public class CustomizeAndUploadPolicies : Task
    {
        private readonly JsonHelper _jsonHelper;
        private readonly XMLHelper _xmlHelper;
        private readonly GraphClientHelper _graphHelper;

        public CustomizeAndUploadPolicies(string[] arguments)
        {
            ValidateArguments(arguments);
            _jsonHelper = new JsonHelper(arguments[0]);
            _xmlHelper = new XMLHelper(arguments[0]);
            _graphHelper = new GraphClientHelper(arguments[1], arguments[2], "beta", "https://graph.microsoft.com/");
        }

        public void UpdateValues()
        {
            try
            {
                Console.WriteLine("Updating the values");
                string fileName = _jsonHelper.GetJsonFileName();
                Console.WriteLine("tRYING TO FETCH DATA FROM JSON");
                var jsonProperties = _jsonHelper.GetAllPropertiesFromJson(fileName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("USING FILE "+ fileName);
                    var policies = _xmlHelper.FetchXmlFromDirectory();
                    
                    foreach (var policy in policies)
                    {
                        Console.WriteLine("USING POLICY " + policy);
                        var xmlData = _xmlHelper.ReadFromXML(policy);
                        var tenantInfo = _jsonHelper.GetValueFromJson(fileName, "Tenant");
                        var updatedPolicy = Regex.Replace(xmlData, "{Settings:Tenant}", tenantInfo);
                        if (jsonProperties.PolicySettings != null)
                        {
                            foreach (var property in jsonProperties.PolicySettings)
                            {
                                var propertyValue = _jsonHelper.GetValueFromJson(fileName, property);
                                updatedPolicy = Regex.Replace(updatedPolicy, "{Settings:" + property + "}", propertyValue);
                            }
                        }
                        var resp = _graphHelper.SendGraphPostRequest("/trustFramework/policies", updatedPolicy).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
        }

        public override void ValidateArguments(string[] arguments)
        {
            if (arguments.Length != 3)
            {
                Common.RaiseConsoleMessage(LogType.ERROR,"Some Arguments are missing",false);
            }
            //directory uri
            if (string.IsNullOrEmpty(arguments[0]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Path Value", false);
            }
            //domainName (B2C Domain)
            if (string.IsNullOrEmpty(arguments[1]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide B2C Domain", false);
            }
            //clientid
            if (string.IsNullOrEmpty(arguments[2]))
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Please Provide Client Id of Application you have registered in b2c tenat", false);
            }
           
        }
    }
}
