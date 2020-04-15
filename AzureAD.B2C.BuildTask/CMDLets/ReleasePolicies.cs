using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AzureAD.B2C.BuildTask.CMDLets
{
    [Cmdlet(VerbsCommon.New, "ReleasePolicies")]
    public class ReleasePolicies: PSCmdlet
    {
        #region Parameters
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
            base.WriteVerbose("Starting Release Task");
            base.WriteVerbose($"B2C Domain {b2CDomain}");
            base.WriteVerbose($"Client ID {clientId}");
            base.WriteVerbose($"Client Secret {clientSecret}");
            artifactPublishPath += "policies";
            base.WriteVerbose($"Artifect Publish Path {artifactPublishPath}");
            var relasePolicies = new Tasks.ReleasePolicies(b2CDomain, clientId, clientSecret, artifactPublishPath);
            var error = relasePolicies.Release();
            if (error.AnyError)
            {
                throw new Exception(error.ErrorMessage);
            }
            base.WriteVerbose("Finishing Release Task");
            base.WriteVerbose("Finished");
        }
    }
}
