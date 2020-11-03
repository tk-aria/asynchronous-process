using AriaSDK.Runtime.ProcessUtil;

/// <summary>
///
/// </summary>
public sealed class SleepCommand : CmdProcess, ICmdLineProcess
{
    public SleepCommand(string arguments)
        : base("sleep", arguments)
    {

    }
}
