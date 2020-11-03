using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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