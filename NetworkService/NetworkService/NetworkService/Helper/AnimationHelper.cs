using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

public static class AnimationHelper
{
    public static readonly DependencyProperty TriggerOnHeightChangeProperty =
        DependencyProperty.RegisterAttached(
            "TriggerOnHeightChange",
            typeof(bool),
            typeof(AnimationHelper),
            new PropertyMetadata(false, OnTriggerOnHeightChange));

    public static bool GetTriggerOnHeightChange(DependencyObject obj)
    {
        return (bool)obj.GetValue(TriggerOnHeightChangeProperty);
    }

    public static void SetTriggerOnHeightChange(DependencyObject obj, bool value)
    {
        obj.SetValue(TriggerOnHeightChangeProperty, value);
    }

    private static void OnTriggerOnHeightChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Rectangle rectangle && (bool)e.NewValue)
        {
            rectangle.SizeChanged += (sender, args) =>
            {
                var animation = new DoubleAnimation
                {
                    Duration = TimeSpan.FromSeconds(0.5),
                    From = 30,
                    To = rectangle.ActualHeight, // Assumes Height is set to Auto or changes dynamically
                    EasingFunction = new QuadraticEase()
                };
                rectangle.BeginAnimation(Rectangle.HeightProperty, animation);
            };
        }
    }
}