namespace AzureAD.B2C.BuildTask.Commons
{
    using System;
    public class Common
    {
        /// <summary>
        /// print console messages
        /// </summary>
        /// <param name="logType">Log Type</param>
        /// <param name="message">custom message</param>
        /// <param name="subMessage">flag for sub message</param>
        public static void RaiseConsoleMessage(LogType logType, string message, bool subMessage)
        {
            var logMessage = string.Empty;

            if (subMessage)
            {
                logMessage = "      ";
            }
            switch (logType)
            {
                case LogType.ERROR:
                    logMessage += "ERROR : ";
                    break;
                case LogType.INFO:
                    logMessage += "INFO : ";
                    break;
                case LogType.WARN:
                    logMessage += "WARN : ";
                    break;
                default:
                case LogType.DEBUG:
                    break;
            }
            Console.WriteLine(string.Concat(logMessage, message));
            Console.ResetColor();
        }
    }
}
