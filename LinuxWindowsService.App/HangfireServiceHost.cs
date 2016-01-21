using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.SQLite;
using LinuxWindowsService.SharedTypes;
using NLog;

namespace LinuxWindowsService.App
{
    partial class HangfireServiceHost : ServiceBase
    {
        private BackgroundJobServer hangfireServer;

        public HangfireServiceHost()
        {
            InitializeComponent();

            string dbFileName   = "HangfireQueueDb.sqlite";
            string dbConnString = $"Data Source={dbFileName};Version=3;";

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

        private static void Log(string message)
        {
            if (Environment.UserInteractive)
                LogHelper.LogToConsole(message);
            else
                LogHelper.LogToNLog(message);
        }
    }
}
