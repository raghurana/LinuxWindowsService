using System;
using System.Threading.Tasks;

namespace LinuxWindowsService.App
{
    public class MyBusinessLogicAsync
    {
        public async Task RunAsServiceAsync()
        {
            await FakeDelay();

            Console.WriteLine("Running async Method.");
        }

        public async Task CleanupAsync()
        {
            await FakeDelay();

            Console.WriteLine("{0}Cleanup performed.", Environment.NewLine);
        }

        private static Task FakeDelay()
        {
            return Task.Delay(500);
        }
    }
}