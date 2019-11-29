using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.Exceptions
{
    class FfmpegConversionException : Exception
    {
        private string From { get; }
        private string To { get; }
        public FfmpegConversionException(string from, string to, string message = "Could not convert between")
            : base(message)
        {
            this.From = from;
            this.To = to;
        }
        public override string ToString()
            => $"Message > {Message}, From > {From}, To > {To}";
    }
}
