using System;

namespace LinuxWindowsService.App
{
    public class MyBusinessLogic
    {
        private int counter;

        public MyBusinessLogic()
        {
            counter = 1;
        }

        public void RunAsService()
        {
            Console.WriteLine($"Running {counter++} sync Method.");

            if (counter%7 == 0)
            {
                counter++;
                throw new Exception("Lucky 7 Exception !!");
            }
        }

        public void CleanUp()
        {
            Console.WriteLine("{0}Cleanup performed.{0}", Environment.NewLine);
        }
    }
}