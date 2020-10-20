using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


public interface ICmdLineProcess
{
    Process Invoke();
}

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

public sealed class ADBCommand : CmdProcess, ICmdLineProcess
{
    public ADBCommand(string arguments)
        : base("adb", arguments)
    {

    }
}

public sealed class SleepCommand : CmdProcess, ICmdLineProcess
{
    public SleepCommand(string arguments)
        : base("sleep", arguments)
    {

    }
}

public sealed class ProcessEventHandler
{
    public Action<object, EventArgs> onIOData;

    public Action<object, EventArgs> onIOError;

    public Action<object, EventArgs> onExit;
}

public static class Util
{
    public static Task StartAsync(this Process self, ProcessEventHandler events = null)
    {
        var tcs = new TaskCompletionSource<bool>();
        bool started = false;
        self.Exited += (sender, args) => {
            tcs.SetResult(true);
            events?.onExit(sender, args);
        };
        self.OutputDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    //this.m_FCResult += $"{args.Data}\n";
                    events?.onIOData(sender, args);
                }
            };
        self.ErrorDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    //this.m_FCResult += $"Error : {args.Data}\n";
                    events?.onIOError(sender, args);
                }
            };

        //プロセスからの情報を受け取る変数の初期化
        //this.m_FCResult = "";

        //プロセスの開始
        started = self.Start();
        self.BeginOutputReadLine();
        self.BeginErrorReadLine();

        return tcs.Task;
    }
}

public class AsyncProcess
{
    string m_FCResult;

    private async Task StartCommandAsync(string sourceFileName, string destinationFileName)
    {
        //Processを非同期に実行
        using (Process process = this.CreateFCProcess(sourceFileName, destinationFileName))
        {
            await this.StartCommandAsync(process);
        }
    }

    public Task StartCommandAsync(Process process)
    {
        var tcs = new TaskCompletionSource<bool>();
        bool started = false;
        process.Exited += (sender, args) => {
            tcs.SetResult(true);
        };
        process.OutputDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    this.m_FCResult += $"{args.Data}\n";
                }
            };
        process.ErrorDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data)) {
                    this.m_FCResult += $"Error : {args.Data}\n";
                }
            };

        //プロセスからの情報を受け取る変数の初期化
        this.m_FCResult = "";

        //プロセスの開始
        started = process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return tcs.Task;
    }

    private Process CreateFCProcess(string sourceFileName,string destinationFileName) {
        ProcessStartInfo info = new ProcessStartInfo() {
            FileName = "FC",
            Arguments = $"\"{sourceFileName}\" \"{destinationFileName}\"",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        Process process = new Process() {
            StartInfo = info,
            EnableRaisingEvents = true,
        };
        return process;
    }

}