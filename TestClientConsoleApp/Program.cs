using System;
using Hangfire;
using Hangfire.SQLite;

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
            client.Schedule(() => Console.WriteLine("Raghu Test"), TimeSpan.FromSeconds(5));

            Console.WriteLine("Press return to exit.");
            Console.ReadLine();
        }
    }
}
