namespace B2C.ADOExtension.CMDLets
{
    using System;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.New, "CustomizeAndUploadPolicies")]
    public class CustomizeAndUploadPolicies : PSCmdlet
    {
        #region Parameters
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Please select File path for b2c policies and check if policySetting.json is availeble or not")]
        [Alias("Policy Directory Path")]
        public string directoryPath = null;
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "You can find the tenant name from b2c tenant")]
        [Alias("B2C Domaine")]
        public string b2CDomain = null;

        [Parameter(Mandatory = true,
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "Please Refer the documentation")]
        [Alias("Application Id")]
        public string clientId = null;
        #endregion

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            base.WriteVerbose("starting CustomizeAndUploadPolicies");
            try
            {
                Console.WriteLine($"Directory Path {directoryPath}");
                Console.WriteLine($"B2C Domain {b2CDomain}");
                Console.WriteLine($"Client ID {clientId}");
                var args = new string[] { directoryPath, b2CDomain, clientId };
                var obj = new Tasks.CustomizeAndUploadPolicies(args);
                obj.UpdateValues();
            }
            catch (Exception ex)
            {
                base.WriteVerbose("An Error occured due to " + ex.Message);
            }
            base.WriteVerbose("Finished CopyAccessTeamTemplate");
        }
    }
}
