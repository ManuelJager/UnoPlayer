using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using Windows.UI.Xaml;

namespace UnoPlayer.Helpers
{
    public class ActualSizePropertyProxyHeader : ActualSizePropertyProxy
    {
        public new double ActualWidthValue
            => base.ActualWidthValue - 60;
    }
}
