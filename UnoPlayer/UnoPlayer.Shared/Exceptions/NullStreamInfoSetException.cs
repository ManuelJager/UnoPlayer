using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.Exceptions
{
    public class NullStreamInfoSetException : Exception
    {
        public NullStreamInfoSetException(string message = "No stream info sets found")
            : base(message)
        {
        }
    }
}
