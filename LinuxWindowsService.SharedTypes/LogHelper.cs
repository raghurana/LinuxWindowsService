using System;
using NLog;

namespace LinuxWindowsService.SharedTypes
{
    public static class LogHelper
    {
        public static void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogToNLog(string message)
        {
            LogManager.GetCurrentClassLogger().Log(LogLevel.Debug, message);
        }
    }
}
