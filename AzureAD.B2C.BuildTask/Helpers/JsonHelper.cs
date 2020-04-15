namespace AzureAD.B2C.BuildTask.Helpers
{
    using AzureAD.B2C.BuildTask.Commons;
    using Newtonsoft.Json;
    using System;
    public class JsonHelper
    {
        public dynamic GetAllPropertiesFromJson(string json)
        {
            try
            {
                dynamic array = JsonConvert.DeserializeObject(json);
                return array;
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
            return null;
        }
        public string GetValueFromJson(string json, string propertyName)
        {
            try
            {
                dynamic array = JsonConvert.DeserializeObject(json);
                return array[propertyName];
            }
            catch (Exception ex)
            {
                Common.RaiseConsoleMessage(LogType.ERROR, $"Something Went Wrong : {ex.Message}", false);
            }
            return null;
        }

    }
}
