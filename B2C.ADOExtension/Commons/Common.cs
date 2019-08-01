namespace B2C.ADOExtension.Commons
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.INFO:
                    logMessage += "INFO : ";
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.WARN:
                    logMessage += "WARN : ";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                case LogType.DEBUG:
                    Console.ResetColor();
                    break;
            }
            Console.WriteLine(string.Concat(logMessage, message));
            Console.ResetColor();
        }
    }
}
