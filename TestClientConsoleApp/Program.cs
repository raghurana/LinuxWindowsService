using System;
using Hangfire;
using Hangfire.Redis;
using LinuxWindowsService.SharedTypes;

namespace TestClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["azureredis"].ConnectionString;

            Console.WriteLine(connString);

            GlobalConfiguration.Configuration.UseStorage(new RedisStorage(connString));

            var client = new BackgroundJobClient();
            client.Schedule(() => LogHelper.LogToNLog($"{DateTime.Now} !!! Raghu test NLog !!!"), TimeSpan.FromSeconds(5));

            Console.WriteLine("Press return to exit.");
            Console.ReadLine();
        }
    }
}
