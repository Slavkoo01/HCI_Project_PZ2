using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NetworkService.CustomControls
{
    public class CustomRectangle : Button
    {

       
        public static readonly RoutedEvent HeightChangedEvent =
            EventManager.RegisterRoutedEvent(
                "HeightChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(CustomRectangle));

        
        public static readonly RoutedEvent ContentChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ContentChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(CustomRectangle));

        
        public event RoutedEventHandler HeightChanged
        {
            add { AddHandler(HeightChangedEvent, value); }
            remove { RemoveHandler(HeightChangedEvent, value); }
        }

        
        public event RoutedEventHandler ContentChanged
        {
            add { AddHandler(ContentChangedEvent, value); }
            remove { RemoveHandler(ContentChangedEvent, value); }
        }

        
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == HeightProperty)
            {
                RoutedEventArgs heightArgs = new RoutedEventArgs(HeightChangedEvent);
                RaiseEvent(heightArgs);
            }
            else if (e.Property == ContentProperty)
            {
                RoutedEventArgs contentArgs = new RoutedEventArgs(ContentChangedEvent);
                RaiseEvent(contentArgs);
            }
        }
    }
}
