using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AzureAD.B2C.BuildTask.CMDLets
{
    [Cmdlet(VerbsCommon.New, "BuildPolicies")]
    public class BuildPolicies : PSCmdlet
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
            HelpMessage = "JSON Values")]
        [Alias("JSON Values")]
        public string JSON = null;

        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "")]
        [Alias("artifact publish path")]
        public string artifactPublishPath = null;

        #endregion
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            base.WriteVerbose("starting");
            base.WriteVerbose("Starting Build Task");
            base.WriteVerbose($"Directory Path {directoryPath}");
            base.WriteVerbose($"JSON String {JSON}");
            artifactPublishPath += "policies";
            base.WriteVerbose($"Artifect Publish Path {artifactPublishPath}");
            var buildPolicies = new Tasks.BuildPolicies(directoryPath, JSON, artifactPublishPath);
            var error = buildPolicies.Build();
            if (error.AnyError)
            {
                throw new Exception(error.ErrorMessage);
            }
            base.WriteVerbose("Finishing Build Task");
            base.WriteVerbose("Finished");
        }
    }
}
