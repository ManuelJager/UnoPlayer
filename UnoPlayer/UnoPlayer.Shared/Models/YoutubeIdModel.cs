using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.Models
{
    /// <summary>
    /// provides a wrapper for a youtube video id
    /// </summary>
    public struct YoutubeIdModel
    {
        public string Id { get; }
        public YoutubeIdModel(string id)
        {
            this.Id = id;
        }
        public static YoutubeIdModel FromURL(string URL)
        {
            string id;

            if (Downloader.TryParseVideoId(URL, out id))
                return new YoutubeIdModel(id);
            else
                throw new Exceptions.URLNotFoundException();
        }
        public override string ToString()
            => Id;
    }
}
