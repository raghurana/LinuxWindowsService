using System;
using Hangfire;
using Hangfire.SQLite;
using LinuxWindowsService.SharedTypes;

namespace TestClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbOptions = new SQLiteStorageOptions {PrepareSchemaIfNecessary = false};

            GlobalConfiguration.Configuration.UseSQLiteStorage("HangfireServerDb", dbOptions);
            GlobalConfiguration.Configuration.UseColouredConsoleLogProvider();

            var client = new BackgroundJobClient();
            client.Enqueue(() => LogHelper.LogToNLog("!!! Raghu test NLog !!!"));

            Console.WriteLine("Press return to exit.");
            Console.ReadLine();
        }
    }
}
