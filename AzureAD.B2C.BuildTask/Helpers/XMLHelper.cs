namespace AzureAD.B2C.BuildTask.Helpers
{
    using AzureAD.B2C.BuildTask.Commons;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    public class XMLHelper
    {
        public readonly string _directoryPath;
        public XMLHelper(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public string ReadFromXML(string xmlPath)
        {
            var jsonStream = new StreamReader(xmlPath);

            using (jsonStream)
            {
                string xml = jsonStream.ReadToEnd();
                return xml;
            }
        }
        public List<string> FetchXmlFromDirectory()
        {
            try
            {
                string[] files = Directory.GetFiles(_directoryPath, "*.xml");
                if (files.Length == 0)
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "No XML File in the directory, Please add custom policies", false);
                    return default;
                }
                return files.ToList();
            }
            catch (UnauthorizedAccessException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Can't read file from the directory, Access Denied", false);
            }
            catch (PathTooLongException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Directory path is too long, please reduce the path and try again", false);
            }
            catch (DirectoryNotFoundException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Directory Not Found", false);
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
            return default;
        }

        public void MoveFirst(List<string> policies)
        {
            var indexOfBase = policies.FindIndex(x => x.Contains("TrustFrameworkBase"));
            var indexOfExtension = policies.FindIndex(x => x.Contains("TrustFrameworkExtensions"));

            if (indexOfBase != -1)
            {
                //Move Base
                var itemBase = policies[indexOfBase];
                policies[indexOfBase] = policies[0];
                policies[0] = itemBase;
            }

            if (indexOfExtension != -1)
            {
                //Move Exyension
                var itemExtension = policies[indexOfExtension];
                policies[indexOfExtension] = policies[1];
                policies[1] = itemExtension;
            }
        }
    }
}
