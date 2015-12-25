using System;
using System.ServiceProcess;

namespace LinuxWindowsService.App
{
    public partial class MyTestService : ServiceBase
    {
        private readonly MyBusinessLogic myClass = new MyBusinessLogic();
        //private readonly MyBusinessLogicAsync myClass = new MyBusinessLogicAsync();
        private readonly ServiceRunner runner = new ServiceRunner();
        private readonly TimeSpan delay = TimeSpan.FromSeconds(1);

        public static void Main(string[] args)
        {
            var service = new MyTestService();

            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                PromptEnterKey("Press enter to stop service.");
                service.OnStop();
                PromptEnterKey("Press enter to exit.");
            }

            else
            {
                ServiceBase.Run(service);
            }
        }

        public MyTestService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            runner.ExecutionException += OnExecutionException;
            runner.StartExecution(() => myClass.RunAsService(), delay);
            //runner.StartExecution(() => myClass.RunAsServiceAsync(), delay);
        }

        protected override void OnStop()
        {
            runner.EndExecutionRequest().Wait();
            myClass.CleanUp();
            //myClass.CleanupAsync().Wait();
        }

        private void OnExecutionException(object sender, Exception exception)
        {
            Console.WriteLine($"Exception occured with message {exception.Message}");
        }

        private static void PromptEnterKey(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
