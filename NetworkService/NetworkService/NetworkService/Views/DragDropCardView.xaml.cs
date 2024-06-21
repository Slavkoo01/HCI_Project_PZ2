using MVVMLight.Messaging;
using NetworkService.CustomControls;
using NetworkService.Helper;
using NetworkService.ViewModel;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for DragDropCardView.xaml
    /// </summary>
    public partial class DragDropCardView : UserControl
    {
        private ServerViewModel _serverViewModel;
        private DragDropCardViewModel _dragDropCardViewModel;
        private Point _initialMouseOffset;
        private ToastNotification _toaNotification = new ToastNotification();
        static bool isDrawing = false;

        private Canvas _canvas;
        private Line _tempLine;
        private NodeLine _nodeLine;
        public static int checkOneNotification = 1;
        public List<NodeLine> NodeLines { get; set; } = new List<NodeLine>(20); 
        private Ellipse _startEllipse;
        public ServerViewModel ServerViewModel { get { return _serverViewModel; } set { _serverViewModel = value; } }

        public Point InitialMouseOffset { get { return _initialMouseOffset; } set {  _initialMouseOffset = value; } }

        private DisplayView displayView;

        public DragDropCardView(ServerViewModel serverViewModel,DisplayView displayView)
        {
            InitializeComponent();
            ServerViewModel = serverViewModel;
            _dragDropCardViewModel = new DragDropCardViewModel(this, displayView);
            closeButton.DataContext = _dragDropCardViewModel;
            DataContext = ServerViewModel;
            this.displayView = displayView;
            
           
            this.MouseDown += UserControl_MouseDown;
            cardBar.MouseMove += UserControl_MouseMove;
            DockLeft.MouseLeftButtonDown += Elipse_MouseLeftButtonDown;
            DockRight.MouseLeftButtonDown += Elipse_MouseLeftButtonDown;

            _canvas = displayView.Canvas;
        }

        public void RemoveNodeByLine(Line line)
        {
            var itemsToRemove = new List<NodeLine>();

            foreach (NodeLine nodeLine in NodeLines)
            {
                if (nodeLine.Line == line)
                {
                    itemsToRemove.Add(nodeLine);
                }
            }

            foreach (NodeLine nodeLine in itemsToRemove)
            {
                NodeLines.Remove(nodeLine);
            }
        }
        private void RemoveLinesEvent(object sender, RoutedEventArgs e)
        {
            RemoveLines();
        }

        public void RemoveLines()
        {
            List<int?> IDs = new List<int?>();

            foreach(NodeLine line in NodeLines)
            {
                //Deletes lines
                _canvas.Children.Remove(line.Line);
                //gathers all the ids of cards need to deleatf from
                if(line.StartServerId != ServerViewModel.Id)
                {
                    IDs.Add(line.StartServerId);
                }
                else if(line.EndServerId != ServerViewModel.Id)
                {
                    IDs.Add(line.EndServerId);
                }
            }
            var itemsToRemove = new List<NodeLine>();

            foreach (NodeLine nodeLine in NodeLines)
            {
                if (nodeLine.StartServerId == ServerViewModel.Id || nodeLine.EndServerId == ServerViewModel.Id)
                {
                    
                    itemsToRemove.Add(nodeLine);
                }
            }

            foreach (NodeLine nodeLine in itemsToRemove)
            {
                NodeLines.Remove(nodeLine);
            }

            foreach (var Id in IDs)
            {
                foreach (var item in _canvas.Children)
                {
                    if (item is DragDropCardView cardToDeleatNodeFrom && cardToDeleatNodeFrom.ServerViewModel.Id == Id)
                    {
                        
                        var itemsToRemoveFromCard = new List<NodeLine>();

                        foreach (NodeLine nodeLine in cardToDeleatNodeFrom.NodeLines)
                        {
                            if (nodeLine.StartServerId == ServerViewModel.Id || nodeLine.EndServerId == ServerViewModel.Id)
                            {
                                
                                itemsToRemoveFromCard.Add(nodeLine);
                            }
                        }
                        foreach (NodeLine nodeLine in itemsToRemoveFromCard)
                        {
                            
                            cardToDeleatNodeFrom.NodeLines.Remove(nodeLine);
                        }
                    }
                }
            }
            
        }

        private void Elipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Ellipse ellipse)
            {
                isDrawing = true;
                _startEllipse = ellipse;

                _tempLine = new Line
                {
                    Stroke = ((SolidColorBrush)Application.Current.Resources["Neutral"]),
                    StrokeThickness = 5
                };
                var startPoint = ellipse.TranslatePoint(new Point(ellipse.Width/2, ellipse.Height/2),_canvas);
                _tempLine.X1 = _tempLine.X2 = startPoint.X;
                _tempLine.Y1 = _tempLine.Y2 = startPoint.Y;       
                _nodeLine = new NodeLine(null, true, ServerViewModel.Id,null);

                if(ellipse.Name == "DockLeft")
                    _nodeLine.Dock = Helper.Dock.Left;
                else
                    _nodeLine.Dock = Helper.Dock.Right;

                _canvas.Children.Insert(79,_tempLine);
                _canvas.MouseMove += Canvas_MouseMove;
                _canvas.PreviewMouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            
            }
        }
        
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_tempLine != null)
            {
                _canvas.MouseMove -= Canvas_MouseMove;
                _canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonUp;

                var position = e.GetPosition(_canvas);
                position.X += 1;
                position.Y += 1;
                var hitTestResult = VisualTreeHelper.HitTest(_canvas, position);


                if (hitTestResult.VisualHit is Ellipse endEllipse && endEllipse != this.DockLeft && endEllipse != this.DockRight)
                {
                    var dragDropCardView = FindParent<DragDropCardView>(endEllipse);
                    

                    var endPoint = endEllipse.TranslatePoint(new Point(endEllipse.Width / 2, endEllipse.Height / 2), _canvas);
                    _tempLine.X2 = endPoint.X;
                    _tempLine.Y2 = endPoint.Y;

                    _nodeLine.Line = _tempLine;
                    _nodeLine.EndServerId = dragDropCardView.ServerViewModel.Id;
                    
                    AddNodeLine(_nodeLine);
                    _nodeLine = new NodeLine(_tempLine, false, ServerViewModel.Id, dragDropCardView.ServerViewModel.Id);
                    
                    if (endEllipse.Name == "DockLeft")
                        _nodeLine.Dock = Helper.Dock.Left;
                    else
                        _nodeLine.Dock = Helper.Dock.Right;
                    
                    dragDropCardView.AddNodeLine(_nodeLine);
                }
                else
                {
                    var notification = _toaNotification.CreateWarningToastNotification("Not a valid spot for creating a line!");
                    Messenger.Default.Send<NotificationContent>(notification);
                    _canvas.Children.Remove(_tempLine);
                }
                isDrawing = false;
                _nodeLine = null;
                _tempLine = null;
                _startEllipse = null;
            }
        }
        private void AddNodeLine(NodeLine nodeLine)
        {
            foreach(NodeLine line in NodeLines)
            {
                if ((line.StartServerId == nodeLine.StartServerId && line.EndServerId == nodeLine.EndServerId) ||
                    (line.StartServerId == nodeLine.EndServerId && line.EndServerId == nodeLine.StartServerId)) 
                {
                    _canvas.Children.Remove(_tempLine);
                    if(checkOneNotification % 2 == 0)
                    {
                    var notification = _toaNotification.CreateWarningToastNotification("Cards are already connected!");
                    Messenger.Default.Send<NotificationContent>(notification);
                    }
                    checkOneNotification++;
                    return;
                   
                }
            }
            NodeLines.Add(nodeLine);
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tempLine != null)
            {
                var position = e.GetPosition(_canvas);
                _tempLine.X2 = position.X;
                _tempLine.Y2 = position.Y;
            }
        }


        #region DragDrop 
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                InitialMouseOffset = e.GetPosition(this);
            }
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isDrawing)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

       
        #endregion

        #region ValueColors
        private void AnimateBackgroundColor(Border border, Color toColor)
        {
            SolidColorBrush originalBrush = (SolidColorBrush)border.Background;
            SolidColorBrush animatedBrush = new SolidColorBrush(originalBrush.Color);
            border.Background = animatedBrush;

            ColorAnimation animation = new ColorAnimation(toColor, TimeSpan.FromSeconds(0.2));
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private void AnimateForegroundColor(Button button, Color toColor)
        {
            SolidColorBrush originalBrush = (SolidColorBrush)button.Foreground;
            SolidColorBrush animatedBrush = new SolidColorBrush(originalBrush.Color);
            button.Foreground = animatedBrush;

            ColorAnimation animation = new ColorAnimation(toColor, TimeSpan.FromSeconds(0.2));
            animatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
        private Color GetColorBasedOnHeight(double height)
        {
            if (height < 267)
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Blue"]).Color;
            }
            else if (height >= 267 && height <= 437)
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Background"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color;
            }
                
            else
            {
                AnimateForegroundColor(valuePresenter, ((SolidColorBrush)Application.Current.Resources["Neutral"]).Color);
                return ((SolidColorBrush)Application.Current.Resources["Rose"]).Color;
            }
        }
        private void CustomRectangle_ContentChanged(object sender, RoutedEventArgs e)
        {
            AnimateBackgroundColor(valueBorder, GetColorBasedOnHeight(ServerViewModel.Value1));
        }
        #endregion

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

    }
}
