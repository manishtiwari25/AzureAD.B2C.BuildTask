namespace B2C.ADOExtension.Helpers
{
    using B2C.ADOExtension.Commons;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    public class JsonHelper
    {
        private readonly string _directoryPath;
        public JsonHelper(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public string GetJsonFileName()
        {
            try
            {
                string[] files = Directory.GetFiles(_directoryPath, "*.json");
                if (files.Length == 0)
                {
                    Common.RaiseConsoleMessage(LogType.ERROR, "No Json File in the directory, Please add polcysettings file", false);
                    return null;
                }
                return files[0];
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
            return null;
        }
        public dynamic GetAllPropertiesFromJson(string jsonFileName)
        {
            try
            {
                var jsonStream = new StreamReader($"{jsonFileName}");

                using (jsonStream)
                {
                    string json = jsonStream.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    return array;
                }
            }
            catch (FileNotFoundException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Json File Not Found", false);
            }
            catch (DirectoryNotFoundException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Directory Not Found", false);
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
            return null;
        }
        public string GetValueFromJson(string jsonFileName, string propertyName)
        {
            try
            {
                var jsonStream = new StreamReader($"{jsonFileName}");

                using (jsonStream)
                {
                    string json = jsonStream.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    return array[propertyName];
                }
            }
            catch (FileNotFoundException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR,"Json File Not Found",false);
            }
            catch (DirectoryNotFoundException)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, "Directory Not Found", false);
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
            return null;
        }

    }
}
