using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AriaSDK.Runtime.ProcessUtil
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ProcessEventHandler
    {
        public Action<object, EventArgs> onIOData;
        public Action<object, EventArgs> onIOError;
        public Action<object, EventArgs> onExit;
    }
}
