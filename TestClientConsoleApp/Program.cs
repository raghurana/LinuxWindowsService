using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SQLite;
using LinuxWindowsService.SharedTypes;

namespace TestClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbFileName   = "HangfireQueueDb.sqlite";
            var dbConnString = $"Data Source={dbFileName};Version=3";
            var dbOptions    = new SQLiteStorageOptions {PrepareSchemaIfNecessary = false};

            GlobalConfiguration.Configuration.UseSQLiteStorage(dbConnString, dbOptions);
            GlobalConfiguration.Configuration.UseColouredConsoleLogProvider();

            var client = new BackgroundJobClient();
            client.Enqueue(() => LogHelper.LogToNLog("!!! Raghu test nlog !!!"));

            Console.WriteLine("Press return to exit.");
            Console.ReadLine();
        }
    }
}
