using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.MemoryStorage;

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
        }

        private void OnStarted()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Running...."), Cron.Minutely);
        }

        protected override void OnStop()
        {
            hangfireServer.Dispose();
        }

        public static void Main(string[] args)
        {
            var service = new HangfireServiceHost();

            if (Environment.UserInteractive)
            {
                Console.WriteLine("============================================");
                Console.WriteLine("Starting hangfire server. Press enter to stop.");
                Console.WriteLine("============================================");

                service.OnStart(args);
                service.OnStarted();
                Console.ReadLine();
                service.OnStop();
            }

            else
            {
                ServiceBase.Run(service);
            }
        }
       
    }
}
