using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;
using Xabe.FFmpeg;

namespace UnoPlayer.Shared
{
    public partial class Downloader
    {
        public static readonly string songFolder;
        static Downloader()
        {
            songFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        }
        /// <summary>
        /// Get a task wrapper representing a youtube to mp3 download and conversion operation
        /// This method will not perform any actions itself. It will just return the body of the function
        /// </summary>
        /// <param name="youtubeID">Youtube ID</param>
        /// <param name="taskArgs">Additional task arguments</param>
        /// <returns></returns>
        public static async Task<Models.SongModel> DownloadSongAsync(
            Models.YoutubeIdModel youtubeId,
            DownloadTaskArgs taskArgs,
            bool logTime = false,
            bool deleteTemp = true)
        {
            System.Diagnostics.Stopwatch operationStopwatch = null;
            if (logTime)
                operationStopwatch = System.Diagnostics.Stopwatch.StartNew();

            var client = new YoutubeClient();

            // create wrapper task for getting video metadata
            YoutubeExplode.Models.Video video = null;
            var videoMetaTask = Task.Run(async () =>
            {
                video = await client.GetVideoAsync(youtubeId.ToString());
            });

            // create wrapper task for getting the media stream info
            YoutubeExplode.Models.MediaStreams.MediaStreamInfo selectedStreamInfo = null;
            var selectedStreamInfoTask = Task.Run(async () =>
            {
                selectedStreamInfo = await SelectMediaStreamInfoByID(client, youtubeId);
            });

            await Task.WhenAll(videoMetaTask, selectedStreamInfoTask);

            // Get temp folder
            var tempFolder = System.IO.Path.GetTempPath();

            // Temp file is saved as mp4
            // Compose temp file name, based on metadata
            var tempVidFileExtension = selectedStreamInfo.Container.GetFileExtension();
            var tempVidFileName = System.IO.Path.Combine(tempFolder, $"{youtubeId.ToString()}.{tempVidFileExtension}");

            // If file already exists, delete.
            await FileManager.TryDeleteFile(tempVidFileName);

            taskArgs.OnStartDownload();

            // Log download progress
            using ((IDisposable)taskArgs.downloadProgressReporter)
                await client.DownloadMediaStreamAsync(
                    selectedStreamInfo,
                    tempVidFileName,
                    (IProgress<double>)taskArgs.downloadProgressReporter,
                    taskArgs.token);

            var tempSongFileName = System.IO.Path.Combine(tempFolder, $"{youtubeId.ToString()}.mp3");

            // If file already exists, delete.
            await FileManager.TryDeleteFile(tempSongFileName);

            taskArgs.OnStartConversion();

            using ((IDisposable)taskArgs.conversionProgressReporter)
            {
                var conversionTask = Conversion.ExtractAudio(tempVidFileName, tempSongFileName);
                conversionTask.OnProgress +=
                    (object sender, Xabe.FFmpeg.Events.ConversionProgressEventArgs args) =>
                    ((IProgress<double>)taskArgs.conversionProgressReporter).Report(args.Percent / 100f);
                await conversionTask.Start();
            }

            var outputPath = await FileManager.CreateFileAsync(songFolder, $"{youtubeId.ToString()}.mp3");

            var tempReadStream = System.IO.File.OpenRead(tempSongFileName);
            var outputWriteStream = await FileManager.OpenFileForWriteAsync(outputPath);

            // Copy stream from the temp file to the output
            await tempReadStream.CopyToAsync(outputWriteStream);

            // Close file streams
            tempReadStream.Close();
            outputWriteStream.Close();

            if (deleteTemp)
            {
                // Delete temp files
                await FileManager.TryDeleteFile(tempVidFileName);
                await FileManager.TryDeleteFile(tempSongFileName);
            }

            // Build song model
            var song = new Models.SongModel(outputPath, video);

            // Fire complete event
            taskArgs.OnComplete();

            if (logTime)
            {
                operationStopwatch.Stop();
                System.Diagnostics.Debug.WriteLine($"Elapsed operation > {operationStopwatch.Elapsed}");
            }

            return song;
        }
        /// <summary>
        /// Select a suitable mediaStreamInfo by id to download a video for later conversion
        /// </summary>
        /// <param name="client">Youtube http client</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal static async Task<MediaStreamInfo> SelectMediaStreamInfoByID(YoutubeClient client, Models.YoutubeIdModel youtubeId)
        {
            // Get media stream info set
            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(youtubeId.ToString());
            var streamInfoSetList = streamInfoSet.Muxed;

            // If there are no info sets to be downloaded
            if (streamInfoSetList == null)
                throw new Exceptions.NullStreamInfoSetException();

            MediaStreamInfo selectedStreamInfo = null;

            // Select 360p stream
            foreach (var streamInfo in streamInfoSetList)
                if (streamInfo.Resolution.Height == 360)
                    selectedStreamInfo = streamInfo;

            // If there was no 360p stream, just select the best stream
            if (selectedStreamInfo == null)
                selectedStreamInfo = streamInfoSetList.WithHighestVideoQuality();

            return selectedStreamInfo;
        }
    }
}
