using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            DownloadTest();

            // FileTest();
        }

        private async void DownloadTest()
        {
            var songId = Shared.Models.YoutubeIdModel.FromURL("https://music.youtube.com/watch?v=tccGwQsQmHc&list=LM");

            var args = new Shared.Downloader.DownloadTaskArgs();

            args.SetDownloadProgressReporter(new Shared.ProgressReporters.DebugReporter("Download progress :"));
            args.SetConversionProgressReporter(new Shared.ProgressReporters.DebugReporter("Conversion progress :"));
            args.StartDownload += OnStartDownload;
            args.DownloadingFFmpeg += OnDownloadingFFmpeg;
            args.CompletedDownloadingFFmpeg += OnCompletedDownloadingFFmpeg;
            args.StartConversion += OnStartConversion;
            args.Complete += OnComplete;

            await Shared.Downloader.DownloadSongAsync(songId, 
                args, 
                logTime: true, 
                deleteTemp: false);
        }
        static void OnStartDownload(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Download has been started");
        }
        static void OnDownloadingFFmpeg(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Download FFmpeg has started");
        }
        static void OnCompletedDownloadingFFmpeg(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Download FFmpeg has completed");
        }
        static void OnStartConversion(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Start conversion");
        }
        static void OnComplete(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("On complete");
        }


        private async void FileTest()
        {
            var path = await FileManager.CreateFileAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "test.txt");
            var writeStream = await FileManager.OpenFileForWriteAsync(path);

            using (var streamWriter = new StreamWriter(writeStream))
            {
                streamWriter.WriteLine("Write");
            }

            var stream = await FileManager.OpenFileForReadAsync(path);
            var content = new System.IO.StreamReader(stream).ReadToEnd();
            System.Diagnostics.Debug.WriteLine($"content is : {content}");
        }
    }
}
