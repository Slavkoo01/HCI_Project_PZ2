﻿using NetworkService.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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

        private Point _start;
        private Point _origin;
        private const int GridSpacing = 60;

        public DisplayView()
        {

            InitializeComponent();
            
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
            canvas.AllowDrop = true;
            treeView.MouseMove += TreeView_ItemDrag;

        }

        private void TreeView_ItemDrag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TreeView treeView = (TreeView)sender;
                TreeViewItem treeViewItem = FindTreeViewItem(e.OriginalSource as DependencyObject);

                try
                {
                    if (treeViewItem != null)
                    {
                        ServerViewModel selectedBook = (ServerViewModel)treeViewItem.Header;
                        DataObject dragData = new DataObject(typeof(ServerViewModel), selectedBook);
                        DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);

                    }
                }
                catch (Exception)
                {

                }
            }
        }

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
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ServerViewModel)))
            {
                
                ServerViewModel droppedServer = e.Data.GetData(typeof(ServerViewModel)) as ServerViewModel;
                MessageBox.Show(droppedServer.ToString());

                
                    string itemData = e.Data.GetData(DataFormats.Text) as string;

                // Create a UserControl based on the dropped data
                    UserControl userControl = new DragDropCardView(droppedServer);

                    // Get the drop position relative to the Canvas
                    Point dropPosition = e.GetPosition(canvas);

                    // Set the position of the UserControl on the Canvas
                    Canvas.SetLeft(userControl, dropPosition.X);
                    Canvas.SetTop(userControl, dropPosition.Y);

                    // Add the UserControl to the Canvas
                    canvas.Children.Add(userControl);
                
            }
        }


        #region Canvas Grid

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var position = e.GetPosition(canvas);
            var transformGroup = (TransformGroup)canvas.RenderTransform;
            var scaleTransform = (ScaleTransform)transformGroup.Children[0];
            double zoom = e.Delta > 0 ? .2 : -.2;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if ((scaleTransform.ScaleX + zoom) >= 1 && (scaleTransform.ScaleX + zoom) <= 3)
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
            for (int i = 0; i < canvas.Width; i += GridSpacing)
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

            for (int i = 0; i < canvas.Height; i += GridSpacing)
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
            CenterCanvas();
            DrawGrid();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
