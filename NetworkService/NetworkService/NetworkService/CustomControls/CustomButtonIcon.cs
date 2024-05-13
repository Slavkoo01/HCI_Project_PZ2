using System.Windows;
using System.Windows.Controls;
using FontAwesome5;
namespace NetworkService.CustomControls
{
    public class CustomButtonIcon : Button
    {
        public static readonly DependencyProperty IconProperty =
           DependencyProperty.Register("Icon", typeof(EFontAwesomeIcon), typeof(CustomButtonIcon));

        public EFontAwesomeIcon Icon
        {
            get { return (EFontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(CustomButtonIcon),
                new PropertyMetadata(new Thickness(25, 0, 0, 0)));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(CustomButtonIcon),
                new PropertyMetadata(30.0));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register("TextFontSize", typeof(double), typeof(CustomButtonIcon),
                new PropertyMetadata(20.0));

        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(CustomButtonIcon),
                new PropertyMetadata(new Thickness(30, -2, 0, 0)));

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        static CustomButtonIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButtonIcon), new FrameworkPropertyMetadata(typeof(CustomButtonIcon)));
        }
    }
}