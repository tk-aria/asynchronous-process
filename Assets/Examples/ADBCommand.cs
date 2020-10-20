using AriaSDK.Runtime.ProcessUtil;

/// <summary>
///
/// </summary>
public sealed class ADBCommand : CmdProcess, ICmdLineProcess
{
    public ADBCommand(string arguments)
        : base("adb", arguments)
    {

    }
}