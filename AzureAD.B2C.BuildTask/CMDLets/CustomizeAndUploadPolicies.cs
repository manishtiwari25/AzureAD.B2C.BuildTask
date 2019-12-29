namespace AzureAD.B2C.BuildTask.CMDLets
{
    using System;
    using System.Management.Automation;
    using AzureAD.B2C.BuildTask.Commons;

    [Cmdlet(VerbsCommon.New, "CustomizeAndUploadPolicies")]
    public class CustomizeAndUploadPolicies : PSCmdlet
    {
        #region Parameters
        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Please select File path for b2c policies and check if policySetting.json is availeble or not")]
        [Alias("Policy Directory Path")]
        public string directoryPath = null;
        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "You can find the tenant name from b2c tenant")]
        [Alias("B2C Domaine")]
        public string b2CDomain = null;

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Application Id of the application you have registered in b2c tenant")]
        [Alias("Application Id")]
        public string clientId = null;

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Application Secret of the application you have registered in b2c tenant")]
        [Alias("Client Secret")]
        public string clientSecret = null;

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "JSON Values")]
        [Alias("JSON Values")]
        public string JSON = null;

        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Task Type: Build / Release")]
        [Alias("task type")]
        public string taskType = null;

        #endregion

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            base.WriteVerbose("starting CustomizeAndUploadPolicies");


            Console.WriteLine($"Task Type {taskType}");
            if (taskType == "Build")
            {
                Console.WriteLine($"Directory Path {directoryPath}");
                Console.WriteLine($"JSON String {JSON}");
            }
            else if (taskType == "Release")
            {
                Console.WriteLine($"B2C Domain {b2CDomain}");
                Console.WriteLine($"Client ID {clientId}");
                Console.WriteLine($"Client Secret {clientSecret}");
            }
            else
            {
                throw new InvalidOperationException($"Task Type {taskType}  Not Supportad!!");
            }
            var args = new string[] { directoryPath, b2CDomain, clientId, clientSecret, JSON, taskType };
            var obj = new Tasks.CustomizeAndUploadPolicies(args);
            var error = obj.UpdateValues();
            if (error.AnyError)
            {
                throw new Exception(error.ErrorMessage);
            }


            base.WriteVerbose("Finished CopyAccessTeamTemplate");
        }
    }
}
