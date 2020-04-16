using System;
using System.Management.Automation;

namespace AzureAD.B2C.BuildTask.CMDLets
{
    [Cmdlet(VerbsCommon.New, "BuildPolicies")]
    public class BuildPolicies : PSCmdlet
    {
        #region Parameters
        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("Policy Directory Path")]
        public string directoryPath = null;


        [Parameter(Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("JSON Values")]
        public string JSON = null;

        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0)]
        [Alias("artifact publish path")]
        public string artifactPublishPath = null;

        #endregion
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            Console.WriteLine("starting");
            Console.WriteLine("Starting Build Task");
            Console.WriteLine($"Directory Path {directoryPath}");
            Console.WriteLine($"JSON String {JSON}");
            Console.WriteLine($"Artifect Publish Path {artifactPublishPath}");
            var buildPolicies = new Tasks.BuildPolicies(directoryPath, JSON, artifactPublishPath);
            var error = buildPolicies.Build();
            if (error.AnyError)
            {
                throw new Exception(error.ErrorMessage);
            }
            Console.WriteLine("Finishing Build Task");
            Console.WriteLine("Finished");
        }
    }
}
