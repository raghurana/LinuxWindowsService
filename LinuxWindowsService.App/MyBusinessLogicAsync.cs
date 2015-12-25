using System;
using System.Threading.Tasks;

namespace LinuxWindowsService.App
{
    public class MyBusinessLogicAsync
    {
        private int counter;

        public MyBusinessLogicAsync()
        {
            counter = 1;
        }

        public async Task RunAsServiceAsync()
        {
            await FakeDelay();

            Console.WriteLine($"Running {counter++} async Method.");

            if (counter%7 == 0)
            {
                counter++;
                throw new Exception("Lucky 7 Exception !!");
            }
        }

        public async Task CleanupAsync()
        {
            await FakeDelay();

            Console.WriteLine("{0}Cleanup performed.{0}", Environment.NewLine);
        }

        private static Task FakeDelay()
        {
            return Task.Delay(500);
        }
    }
}