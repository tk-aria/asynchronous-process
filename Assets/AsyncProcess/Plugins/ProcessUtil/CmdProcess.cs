using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AriaSDK.Runtime.ProcessUtil
{
    public abstract class CmdProcess
    {
        protected ProcessStartInfo processInfo;

        public CmdProcess(string processName, string arguments)
            : this(new ProcessStartInfo()
            {
                FileName = processName,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }){}

        public CmdProcess(ProcessStartInfo param)
        {
            this.processInfo = param;
        }

        Process CreateProcess()
        {
            return new Process() {
                StartInfo = processInfo,
                EnableRaisingEvents = true,
            };
        }

        public Process Invoke()
        {
            return CreateProcess();
        }
    }
}