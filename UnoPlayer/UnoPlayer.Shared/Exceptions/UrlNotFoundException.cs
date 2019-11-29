using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPlayer.Shared.Exceptions
{
    public class URLNotFoundException : Exception
    {
        public URLNotFoundException(string message = "URL Not Found")
            : base(message)
        {
        }
    }
}
