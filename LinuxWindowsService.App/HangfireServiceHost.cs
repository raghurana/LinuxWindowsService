using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.SQLite;
using NLog;

namespace LinuxWindowsService.App
{
    partial class HangfireServiceHost : ServiceBase
    {
        private BackgroundJobServer hangfireServer;

        public HangfireServiceHost()
        {
            InitializeComponent();

            const string dbPath = @"C:\Code\Learning\LinuxWindowsService\TestClientConsoleApp\bin\Debug\HangfireQueueDb.sqlite";
            string dbConnString = $"Data Source={dbPath};Version=3;";

            GlobalConfiguration.Configuration.UseSQLiteStorage(dbConnString);
            GlobalConfiguration.Configuration.UseNLogLogProvider();
        }

        protected async override void OnStart(string[] args)
        {
            try
            {
                var options = new BackgroundJobServerOptions();
                options.WorkerCount = 1;

                hangfireServer = new BackgroundJobServer(options);
            }

            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        protected override void OnStop()
        {
            hangfireServer.Dispose();
        }

        public static void Main(string[] args)
        {
            var service = new HangfireServiceHost();

            Log("============================================");
            Log("Starting hangfire server. Press enter to stop.");
            Log("============================================");

            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.ReadLine();
                service.OnStop();
            }

            else
            {
                ServiceBase.Run(service);
            }
        }

        public static void Log(string message)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine(message);
            }
            else
            {
                LogManager.GetCurrentClassLogger().Log(LogLevel.Debug, message);
            }
        }
       
    }
}
