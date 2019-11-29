using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnoPlayer.Shared
{
    public partial class Downloader
    {
        public class DownloadTaskArgs
        {
            // Implement IProgress<double> And IDisposeable
            public object downloadProgressReporter { private set; get; }
            public object conversionProgressReporter { private set; get; }

            CancellationTokenSource source = new CancellationTokenSource();
            public CancellationToken token
                => source.Token;

            // Event handlers
            public event EventHandler StartDownload;
            public event EventHandler DownloadingFFmpeg;
            public event EventHandler CompletedDownloadingFFmpeg;
            public event EventHandler StartConversion;
            public event EventHandler Complete;

            // Setters for progressReporters that must implement the IProgress<double> and IDisposeable interfaces
            public void SetDownloadProgressReporter<ArgType>(ArgType value)
                where ArgType : IProgress<double>,
                                IDisposable
                => downloadProgressReporter = value;
            public void SetConversionProgressReporter<ArgType>(ArgType value)
                where ArgType : IProgress<double>,
                                IDisposable
                => conversionProgressReporter = value;

            public virtual void OnStartDownload(EventArgs e = default)
                => StartDownload?.Invoke(this, e);

            public virtual void OnDownloadingFFmpeg(EventArgs e = default)
                => DownloadingFFmpeg?.Invoke(this, e);

            public virtual void OnCompletedDownloadingFFmpeg(EventArgs e = default)
                => CompletedDownloadingFFmpeg?.Invoke(this, e);

            public virtual void OnStartConversion(EventArgs e = default)
                => StartConversion?.Invoke(this, e);

            public virtual void OnComplete(EventArgs e = default)
                => Complete?.Invoke(this, e);
        }
    }
}
