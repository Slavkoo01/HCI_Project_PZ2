using NetworkService.ViewModel;
using NetworkService.Views;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NetworkService.Helper
{
    public class ToastNotification
    {
        public NotificationContent CreateSuccessToastNotification()
        {
            var notificationContent = new NotificationContent
            {
                Title = "Success",
                Message = "Server successfully added.",
                Type = NotificationType.Success,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                //LeftButtonAction = () => SomeAction(), // Action on left button click, button will not show if it null 
                //RightButtonAction = () => SomeAction(), // Action on right button click,  button will not show if it null
                //LeftButtonContent, // Left button content (string or what u want)
                //RightButtonContent, // Right button content (string or what u want)
                CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

                Background = new SolidColorBrush(Colors.Green),
                Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral"),

                // FontAwesome5 by Codinion NuGet paket ti treba da bi radilo ovo sa ikonicama
                // Icon = new SvgAwesome()
                // {
                //      Icon = EFontAwesomeIcon.Regular_Star,
                //      Height = 25,
                //      Foreground = new SolidColorBrush(Colors.Yellow)
                // },

                // Image = new NotificationImage()
                // {
                //      Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
                //      Position = ImagePosition.Top
                // }
            };

            return notificationContent;
        }
        public NotificationContent CreateAnouncementToastNotification()
        {
            var notificationContent = new NotificationContent
            {
                Title = "Notification",
                Message = "Please select at least one server to delete.",
                Type = NotificationType.Notification,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                //LeftButtonAction = () => SomeAction(), // Action on left button click, button will not show if it null 
                //RightButtonAction = () => SomeAction(), // Action on right button click,  button will not show if it null
                //LeftButtonContent, // Left button content (string or what u want)
                //RightButtonContent, // Right button content (string or what u want)
                CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

                Background = (SolidColorBrush)Application.Current.FindResource("Neutral"),
                Foreground = new SolidColorBrush(Colors.Black),

                // FontAwesome5 by Codinion NuGet paket ti treba da bi radilo ovo sa ikonicama
                // Icon = new SvgAwesome()
                // {
                //      Icon = EFontAwesomeIcon.Regular_Star,
                //      Height = 25,
                //      Foreground = new SolidColorBrush(Colors.Yellow)
                // },

                // Image = new NotificationImage()
                // {
                //      Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
                //      Position = ImagePosition.Top
                // }
            };

            return notificationContent;
        }
        public NotificationContent CreateWarningToastNotification(string message)
        {
            var notificationContent = new NotificationContent
            {
                Title = "Warning",
                Message = message,
                Type = NotificationType.Warning,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                //LeftButtonAction = () => SomeAction(), // Action on left button click, button will not show if it null 
                //RightButtonAction = () => SomeAction(), // Action on right button click,  button will not show if it null
                //LeftButtonContent, // Left button content (string or what u want)
                //RightButtonContent, // Right button content (string or what u want)
                CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

                Background = new SolidColorBrush(Colors.DarkGoldenrod),
                Foreground = new SolidColorBrush(Colors.White),

                // FontAwesome5 by Codinion NuGet paket ti treba da bi radilo ovo sa ikonicama
                // Icon = new SvgAwesome()
                // {
                //      Icon = EFontAwesomeIcon.Regular_Star,
                //      Height = 25,
                //      Foreground = new SolidColorBrush(Colors.Yellow)
                // },

                // Image = new NotificationImage()
                // {
                //      Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
                //      Position = ImagePosition.Top
                // }
            };

            return notificationContent;
        }
        public NotificationContent CreateFailToastNotification()
        {
            var notificationContent = new NotificationContent
            {
                Title = "Failure",
                Message = "Server could not be added.",
                Type = NotificationType.Error,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                //LeftButtonAction = () => SomeAction(), // Action on left button click, button will not show if it null 
                //RightButtonAction = () => SomeAction(), // Action on right button click,  button will not show if it null
                //LeftButtonContent, // Left button content (string or what u want)
                //RightButtonContent, // Right button content (string or what u want)
                CloseOnClick = true, // Set true if u want close message when left mouse button click on message (base = true)

                Background = (SolidColorBrush)Application.Current.FindResource("Rose"),
                Foreground = (SolidColorBrush)Application.Current.FindResource("Neutral"),

                // FontAwesome5 by Codinion NuGet paket ti treba da bi radilo ovo sa ikonicama
                // Icon = new SvgAwesome()
                // {
                //      Icon = EFontAwesomeIcon.Regular_Star,
                //      Height = 25,
                //      Foreground = new SolidColorBrush(Colors.Yellow)
                // },

                // Image = new NotificationImage()
                // {
                //      Source = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Test image.png")));,
                //      Position = ImagePosition.Top
                // }
            };

            return notificationContent;
        }
        public NotificationContent CreateDeleteToastNotification(EntitiesView entities, string message)
        {
            var notificationContent = new NotificationContent
            {
                Title = "Confirmation",
                Message = message,
                Type = NotificationType.Notification,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                CloseOnClick = true, // Set true if you want to close the message when left mouse button clicks on the message (default = true)

                Background = (SolidColorBrush)Application.Current.FindResource("Blue"),
                Foreground = new SolidColorBrush(Colors.Black),

                // Icon and Image can be added if required
            };

            // Adding Yes and No buttons with actions
            notificationContent.LeftButtonContent = "Yes";
            notificationContent.LeftButtonAction = () => entities.Delete(); // Define the YesAction method for the Yes button

            notificationContent.RightButtonContent = "No";
            notificationContent.RightButtonAction = () => NoAction(); // Define the NoAction method for the No button

            return notificationContent;
        }
        public NotificationContent CreateDeleteCardToastNotification(DragDropCardViewModel card, string message)
        {
            var notificationContent = new NotificationContent
            {
                Title = "Confirmation",
                Message = message,
                Type = NotificationType.Notification,
                TrimType = NotificationTextTrimType.AttachIfMoreRows, // Will show attach button on message
                RowsCount = 2, // Will show 3 rows and trim after
                CloseOnClick = true, // Set true if you want to close the message when left mouse button clicks on the message (default = true)

                Background = (SolidColorBrush)Application.Current.FindResource("Blue"),
                Foreground = new SolidColorBrush(Colors.Black),

                // Icon and Image can be added if required
            };

            // Adding Yes and No buttons with actions
            notificationContent.LeftButtonContent = "Yes";
            notificationContent.LeftButtonAction = () => card.Delete(); // Define the YesAction method for the Yes button

            notificationContent.RightButtonContent = "No";
            notificationContent.RightButtonAction = () => NoAction(); // Define the NoAction method for the No button

            return notificationContent;
        }


        // Define the NoAction method
        private void NoAction()
        {
            
            
        }
    }
}
