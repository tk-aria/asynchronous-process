using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AriaSDK.Runtime.ProcessUtil;

namespace AriaSDK.Runtime.AsynchronousProcess
{

    /// <summary>
    ///
    /// </summary>
    public static class AsyncProcess
    {

        /// <summary>
        ///
        /// </summary>
        public static Task StartAsync(this Process self, ProcessEventHandler events = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            bool started = false;
            self.Exited += (sender, args) => {
                tcs.SetResult(true);
                events?.onExit(sender, args);
            };

            self.OutputDataReceived +=
                (sender, args) => {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        events?.onIOData(sender, args);
                    }
                };

            self.ErrorDataReceived +=
                (sender, args) => {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        events?.onIOError(sender, args);
                    }
                };

            started = self.Start();
            self.BeginOutputReadLine();
            self.BeginErrorReadLine();

            return tcs.Task;
        }
    }
}
