using System;
using System.Management.Automation;

namespace AzureAD.B2C.BuildTask.CMDLets
{
    [Cmdlet(VerbsCommon.New, "ReleasePolicies")]
    public class ReleasePolicies : PSCmdlet
    {
        #region Parameters
        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("B2C Domaine")]
        public string b2CDomain = null;

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("Application Id")]
        public string clientId = null;

        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("Client Secret")]
        public string clientSecret = null;

        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("artifact publish path")]
        public string artifactPublishPath = null;

        #endregion
        protected override void ProcessRecord()
        {
            Console.WriteLine("Starting Release Task");
            Console.WriteLine($"B2C Domain {b2CDomain}");
            Console.WriteLine($"Client ID {clientId}");
            Console.WriteLine($"Client Secret {clientSecret}");
            Console.WriteLine($"Artifect Publish Path {artifactPublishPath}");
            var relasePolicies = new Tasks.ReleasePolicies(b2CDomain, clientId, clientSecret, artifactPublishPath);
            var error = relasePolicies.Release();
            if (error.AnyError)
            {
                throw new Exception(error.ErrorMessage);
            }
            Console.WriteLine("Finishing Release Task");
            Console.WriteLine("Finished");
        }
    }
}
