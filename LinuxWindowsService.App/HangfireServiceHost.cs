using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.MemoryStorage;
using NLog;

namespace LinuxWindowsService.App
{
    partial class HangfireServiceHost : ServiceBase
    {
        private BackgroundJobServer hangfireServer;

        public HangfireServiceHost()
        {
            InitializeComponent();

            GlobalConfiguration.Configuration.UseMemoryStorage();
            GlobalConfiguration.Configuration.UseNLogLogProvider();
        }

        protected override void OnStart(string[] args)
        {
            var options = new BackgroundJobServerOptions();
            hangfireServer = new BackgroundJobServer(options);

            try
            {
                RecurringJob.AddOrUpdate(() => Log("Running..."), Cron.Minutely);
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
