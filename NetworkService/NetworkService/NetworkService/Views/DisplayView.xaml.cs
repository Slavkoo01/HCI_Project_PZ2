using NetworkService.Helper;
using NetworkService.Repositories;
using NetworkService.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    /// 
    public partial class DisplayView : UserControl
    {
        private DisplayViewModel _displayVM = new DisplayViewModel();
        public DisplayViewModel DisplayViewModel { get { return _displayVM; } }

        public Canvas Canvas { get { return canvas; } }

        private Point _start;
        private Point _origin;
        private const int GridSpacing = 60;
        private bool Isloaded = false;
        private MainWindow mw;
        
        public DisplayView(MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
            DrawGrid();
            CenterCanvas();
            DataContext = _displayVM;
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform());
            transformGroup.Children.Add(new TranslateTransform());
            canvas.RenderTransform = transformGroup;
            
            //Zooming
            canvas.MouseWheel += Canvas_MouseWheel;

            //panning
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;

            //Drag and drop
            canvas.Drop += Canvas_Drop;
            canvas.DragOver += Canvas_DragOver;
            canvas.AllowDrop = true;
            canvas.MouseRightButtonUp += RemoveLineByRightClick;
            treeView.MouseMove += TreeView_ItemDrag;

            DragDrop.AddGiveFeedbackHandler(this, Canvas_GiveFeedback);
            XMLFiles.LoadUserControls(Canvas, this);

            GlobalVar.buttons.Add(HelpButton);

        }

        

       
        private void RemoveLineByRightClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(canvas);
            var hitTestResult = VisualTreeHelper.HitTest(canvas, position);
            if (hitTestResult.VisualHit is Line line && !(line.X1 == 0 || line.Y1 == 0))
            {
               
               canvas.Children.Remove(line);
               var hitTestDock1 = VisualTreeHelper.HitTest(canvas, new Point(line.X1, line.Y1));
               var hitTestDock2 = VisualTreeHelper.HitTest(canvas, new Point(line.X2, line.Y2));
               if(hitTestDock1.VisualHit is Ellipse ellipseDock1 && hitTestDock2.VisualHit is Ellipse ellipseDock2)
               {
                   var Card1 = XMLFiles.FindParent<DragDropCardView>(ellipseDock1);
                   var Card2 = XMLFiles.FindParent<DragDropCardView>(ellipseDock2);
                    Card1.RemoveNodeByLine(line);
                    Card2.RemoveNodeByLine(line);

               }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Canvas_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.Hand);
            e.Handled = true;
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragDropCardView)))
            {
                DragDropCardView draggedControl = e.Data.GetData(typeof(DragDropCardView)) as DragDropCardView;
                if (draggedControl != null)
                {
                    Canvas canvas = sender as Canvas;
                    Point dropPosition = e.GetPosition(canvas);

                    Point initialMouseOffset = draggedControl.InitialMouseOffset;

                   
                    GeneralTransform transformLeft = draggedControl.DockLeft.TransformToVisual(canvas);
                    Point canvasPointLeft = transformLeft.Transform(new Point(0, 0));


                    GeneralTransform transformRight = draggedControl.DockRight.TransformToVisual(canvas);
                    Point canvasPointRight = transformRight.Transform(new Point(0, 0)); 

                    double newLeft = dropPosition.X - initialMouseOffset.X;
                    double newTop = dropPosition.Y - initialMouseOffset.Y;

                    var parent = VisualTreeHelper.GetParent(draggedControl) as Canvas;
                    if (parent != null)
                    {
                        parent.Children.Remove(draggedControl);
                    }

                    // Update positions of the connected NodeLines
                    foreach (var item in draggedControl.NodeLines)
                    {
                        if (item.Dock == Helper.Dock.Left) 
                        {
                            item.MoveLine(canvasPointLeft.X + (draggedControl.DockLeft.ActualWidth / 2), canvasPointLeft.Y + (draggedControl.DockLeft.ActualHeight / 2));
                        }
                        else
                        item.MoveLine(canvasPointRight.X + (draggedControl.DockLeft.ActualWidth / 2), canvasPointRight.Y + (draggedControl.DockLeft.ActualHeight / 2));
                    }

                    canvas.Children.Add(draggedControl);
                    Canvas.SetLeft(draggedControl, newLeft);
                    Canvas.SetTop(draggedControl, newTop);
                }
            }
        }

        private void TreeView_ItemDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TreeViewItem treeViewItem = FindTreeViewItem(e.OriginalSource as DependencyObject);

                try
                {
                    if (treeViewItem != null)
                    {
                        ServerViewModel selectedServer = (ServerViewModel)treeViewItem.Header;
                        DataObject dragData = new DataObject(typeof(ServerViewModel), selectedServer);
                        DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);

                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ServerViewModel)))
            {

                ServerViewModel droppedServer = e.Data.GetData(typeof(ServerViewModel)) as ServerViewModel;

                UserControl userControl = new DragDropCardView(droppedServer, this);

                Point dropPosition = e.GetPosition(canvas);


                Canvas.SetLeft(userControl, dropPosition.X);
                Canvas.SetTop(userControl, dropPosition.Y);


                canvas.Children.Add(userControl);
                _displayVM.RemoveNode(droppedServer);
            }
            
        }
        #region Helping methods
        private TreeViewItem FindTreeViewItem(DependencyObject source)
        {
            if (source == null)
                return null;

            DependencyObject parent = VisualTreeHelper.GetParent(source);

            if (parent is TreeViewItem treeViewItem)
            {
                return treeViewItem;
            }
            else
            {
                return FindTreeViewItem(parent);
            }
        }
        
        #endregion

        #region Canvas Grid



        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        var position = e.GetPosition(canvas);
            var transformGroup = (TransformGroup)canvas.RenderTransform;
            var scaleTransform = (ScaleTransform)transformGroup.Children[0];
            double zoom = e.Delta > 0 ? .2 : -.2;
               
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                e.Handled = true;
                if ((scaleTransform.ScaleX + zoom) >= 0.25 && (scaleTransform.ScaleX + zoom) <= 3)
                {
                    scaleTransform.ScaleX += zoom;
                    scaleTransform.ScaleY += zoom;
                    scaleTransform.CenterX = position.X;
                    scaleTransform.CenterY = position.Y;
                }
               
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Mouse.OverrideCursor = Cursors.Hand;
                _start = e.GetPosition(scrollViewer);
                _origin = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
                canvas.CaptureMouse();
            }
        }
        
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas.IsMouseCaptured)
            {
                
                Vector v = _start - e.GetPosition(scrollViewer);
                scrollViewer.ScrollToHorizontalOffset(_origin.X + v.X);
                scrollViewer.ScrollToVerticalOffset(_origin.Y + v.Y);
                
            }
            var position = e.GetPosition(canvas);
            
            
           

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            canvas.ReleaseMouseCapture();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void CenterCanvas()
        {
            
            double centerX = (canvas.Width - scrollViewer.ViewportWidth) / 2;
            double centerY = (canvas.Height - scrollViewer.ViewportHeight) / 2;

            
            scrollViewer.ScrollToHorizontalOffset(centerX);
            scrollViewer.ScrollToVerticalOffset(centerY);
        }


        private void DrawGrid()
        {
            SolidColorBrush brush = ((SolidColorBrush)Application.Current.Resources["Background"]);
            for (int i = 0; i <= canvas.Width; i += GridSpacing)
            {
                Line verticalLine = new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = canvas.Height,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(verticalLine);
            }

            for (int i = 0; i <= canvas.Height; i += GridSpacing)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = i,
                    X2 = canvas.Width,
                    Y2 = i,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(horizontalLine);
            }
        }


        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVar.IsCanvasLoaded = true;
            if (!Isloaded) 
            XMLFiles.LoadLines(Canvas);
            Isloaded = true;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalVar.IsHelpOpen) 
            {
                mw.CloseHelpAnimation();
                GlobalVar.IsHelpOpen = !GlobalVar.IsHelpOpen;
            }
            else
            {
                mw.OpenHelpAnimation();
                GlobalVar.IsHelpOpen = !GlobalVar.IsHelpOpen;
            }
        }
    }
}
