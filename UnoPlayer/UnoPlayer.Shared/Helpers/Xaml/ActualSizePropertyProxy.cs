using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using Windows.UI.Xaml;

namespace UnoPlayer.Shared.Helpers.Xaml
{
    public class ActualSizePropertyProxy : FrameworkElement, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        public FrameworkElement Element
        {
            get => (FrameworkElement)GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        public double ActualHeightValue
            => Element == null ? 0 : Element.ActualHeight;


        public double ActualWidthValue
            => Element == null ? 0 : Element.ActualWidth;


        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element",
                typeof(FrameworkElement),
                typeof(ActualSizePropertyProxy),
                new PropertyMetadata(null,
                    OnElementPropertyChanged));

        private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ActualSizePropertyProxy)d).OnElementChanged(e);
        }

        private void OnElementChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldElement = (FrameworkElement)e.OldValue;
            var newElement = (FrameworkElement)e.NewValue;

            newElement.SizeChanged += OnElementSizeChanged;
            if (oldElement != null)
            {
                oldElement.SizeChanged -= OnElementSizeChanged;
            }

            NotifyPropChange();
        }

        private void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyPropChange();
        }

        private void NotifyPropChange()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ActualWidthValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("ActualHeightValue"));
            }
        }
    }
}
