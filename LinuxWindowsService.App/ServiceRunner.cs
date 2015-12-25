using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinuxWindowsService.App
{
    public class ServiceRunner
    {
        private CancellationTokenSource cancellationTokenSource;

        private event EventHandler<Task> ExecutionEnded = delegate {};
        public  event EventHandler<Exception> ExecutionException = delegate { };

        public void StartExecution(Action methodToExecute, TimeSpan delayBetweenExecutionCycles)
        {
            StartExecution(async () => { methodToExecute.Invoke(); await Task.FromResult(0); }, delayBetweenExecutionCycles );
        }

        public void StartExecution(Func<Task> asyncMethodToExecute, TimeSpan delayBetweenExecutionCycles)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var cancelToken         = cancellationTokenSource.Token;
            var pollerTask          = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await asyncMethodToExecute.Invoke();
                    }
                    catch (Exception exception)
                    {
                        ExecutionException(this, exception);
                    }

                    if (cancelToken.WaitHandle.WaitOne(delayBetweenExecutionCycles))
                        break;
                }
            }, cancelToken);

            pollerTask.ContinueWith(task => ExecutionEnded(this, task));
        }

        public Task EndExecutionRequest()
        {
            var tcs = new TaskCompletionSource<Task>();
            ExecutionEnded += (sender, task) => tcs.SetResult(task);
            cancellationTokenSource?.Cancel();
            return tcs.Task;
        }
    }
}
