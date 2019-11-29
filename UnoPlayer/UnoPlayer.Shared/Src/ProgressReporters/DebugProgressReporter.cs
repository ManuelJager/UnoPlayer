using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.ProgressReporters
{
    internal class DebugReporter : IProgress<double>, IDisposable
    {
        string prefix;
        public DebugReporter(string prefix = "")
        {
            this.prefix = prefix;
        }
        public void Report(double progress)
        {
            System.Diagnostics.Debug.WriteLine($"{prefix} {progress:P1}");
        }

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine($"{prefix} Completed ✓");
        }
    }
}
