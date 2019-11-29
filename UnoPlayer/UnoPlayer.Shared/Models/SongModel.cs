using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.Models
{
    public class SongModel
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Artist { get; set; }

        public TimeSpan Duration;

        public SongModel(string path)
        {
            this.Title = "DefaultTitle";
            this.Path = path;
            this.Artist = "DefaultArtist";
            this.Duration = default;
        }

        public SongModel(string path, YoutubeExplode.Models.Video video)
        {
            this.Title = video.Title;
            this.Path = path;
            this.Artist = video.Author;
            this.Duration = video.Duration;
        }
    }
}
