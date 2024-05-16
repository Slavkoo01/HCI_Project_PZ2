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

        public event RoutedEventHandler HeightChanged
        {
            add { AddHandler(HeightChangedEvent, value); }
            remove { RemoveHandler(HeightChangedEvent, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == HeightProperty)
            {
                RoutedEventArgs args = new RoutedEventArgs(HeightChangedEvent);
                RaiseEvent(args);
            }
        }
    }
}
