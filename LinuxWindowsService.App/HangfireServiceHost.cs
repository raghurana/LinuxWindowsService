using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.Redis;
using LinuxWindowsService.SharedTypes;

namespace LinuxWindowsService.App
{
    partial class HangfireServiceHost : ServiceBase
    {
        private BackgroundJobServer hangfireServer;

        public HangfireServiceHost()
        {
            InitializeComponent();

            GlobalConfiguration.Configuration.UseNLogLogProvider();

            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["azureredis"].ConnectionString;
            GlobalConfiguration.Configuration.UseStorage(new RedisStorage(connString));
        }

        protected async override void OnStart(string[] args)
        {
            try
            {
                var options = new BackgroundJobServerOptions {WorkerCount = 1};
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
