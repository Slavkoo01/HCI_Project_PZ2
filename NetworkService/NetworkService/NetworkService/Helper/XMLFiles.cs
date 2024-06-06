using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using NetworkService.ViewModel;
using NetworkService.Helper;
using System.Windows;
using NetworkService.Views;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Linq.Expressions;

namespace NetworkService.Repositories
{
    [Serializable]
    [XmlRoot("UserControls")]
    public class UserControlInfoCollection
    {
        [XmlElement("UserControlInfo")]
        public List<UserControlInfo> UserControls { get; set; } = new List<UserControlInfo>();
    }
    [Serializable]
    [XmlRoot("Lines")]
    public class LineInfoCollection
    {
        [XmlElement("LineInfo")]
        public List<LineInfo> Lines { get; set; } = new List<LineInfo>();
    }
    public class XMLFiles
    {
        #region ServerViewModel
        public ObservableCollection<ServerViewModel> serializableObject = EntitiesViewModel.EntityColection;

        public void SerializeObject()
        {
            string directoryPath = MyPath.MyXMLData();
            string fileName = System.IO.Path.Combine(directoryPath, "ServerCollection.xml");

            if (serializableObject == null) { return; }

            try
            {
                // Create the directory if it doesn't exist
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




        public static T DeSerializeObject<T>()
        {
            string fileName = System.IO.Path.Combine(MyPath.MyXMLData(), "ServerCollection.xml");
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception)
            {
                // Log exception here
            }

            return objectOut;
        }

        public static void LoadDataFromXML()
        {
            ObservableCollection<ServerViewModel> list = DeSerializeObject<ObservableCollection<ServerViewModel>>();
            if(list != null)
            {
                EntitiesViewModel.EntityColection.Clear();
                foreach (ServerViewModel item in list)
                {
                    EntitiesViewModel.EntityColection.Add(item);
                }
            }

        }

        #endregion

        #region DragDropCard
        public static void ExportUserControls(Canvas canvas)
        {
            string filePath = "CanvasChildern.xml";
            var userControlsInfo = new UserControlInfoCollection();

            foreach (var child in canvas.Children)
            {
                if (child is DragDropCardView userControl)
                {
                    var left = Canvas.GetLeft(userControl);
                    var top = Canvas.GetTop(userControl);
                    var ServerId = userControl.ServerViewModel.Id;

                    userControlsInfo.UserControls.Add(new UserControlInfo
                    {

                        Left = left,
                        Top = top,
                        Id = ServerId
                    }); ;
                }
            }

            var serializer = new XmlSerializer(typeof(UserControlInfoCollection));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, userControlsInfo);
            }
        }
        public static void LoadUserControls(Canvas canvas, DisplayView displayView)
        {
            string filePath = "CanvasChildern.xml";
            var serializer = new XmlSerializer(typeof(UserControlInfoCollection));
            UserControlInfoCollection userControlsInfo;

            using (var reader = new StreamReader(filePath))
            {
                userControlsInfo = (UserControlInfoCollection)serializer.Deserialize(reader);
            }

            foreach (var info in userControlsInfo.UserControls)
            {
                ServerViewModel tempServer = null;

                foreach (var server in EntitiesViewModel.EntityColection)
                {
                    if (server.Id == info.Id)
                    {
                        tempServer = server;
                        break;
                    }
                }
                if (tempServer != null)
                {
                    DragDropCardView temp = new DragDropCardView(tempServer, displayView);

                    Canvas.SetLeft(temp, info.Left);
                    Canvas.SetTop(temp, info.Top);

                    canvas.Children.Add(temp);
                    displayView.DisplayViewModel.RemoveNode(tempServer);

                }

            }
        }
        #endregion

        #region CanvasLines 
        public static void ExportLines(Canvas canvas)
        {
            if (GlobalVar.IsCanvasLoaded)
            {
                string filePath = "CanvasLines.xml";
                var lineInfoCollection = new LineInfoCollection();

                foreach (var child in canvas.Children)
                {
                    if (child is Line line && !(line.X1 == 0 || line.Y1 == 0))
                    {
                        lineInfoCollection.Lines.Add(new LineInfo(line.X1,line.Y1, line.X2, line.Y2));
                    }
                }

                var serializer = new XmlSerializer(typeof(LineInfoCollection));
                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, lineInfoCollection);
                }
            }
        }
        public static void LoadLines(Canvas canvas)
        {
            try
            {

                string filePath = "CanvasLines.xml";
                var serializer = new XmlSerializer(typeof(LineInfoCollection));
                LineInfoCollection lineInfoCollection;

                using (var reader = new StreamReader(filePath))
                {
                    lineInfoCollection = (LineInfoCollection)serializer.Deserialize(reader);
                }
                foreach (var info in lineInfoCollection.Lines)
                {
                    var _tempLine = new Line
                    {
                        Stroke = ((SolidColorBrush)Application.Current.Resources["Neutral"]),
                        StrokeThickness = 5
                    };

                    _tempLine.X1 = info.X1;
                    _tempLine.Y1 = info.Y1;

                    _tempLine.X2 = info.X2;
                    _tempLine.Y2 = info.Y2;


                    var hitTestDock1 = VisualTreeHelper.HitTest(canvas, new Point(_tempLine.X1, _tempLine.Y1));
                    var hitTestDock2 = VisualTreeHelper.HitTest(canvas, new Point(_tempLine.X2, _tempLine.Y2));

                    if (hitTestDock1.VisualHit is Ellipse ellipseDock1 && hitTestDock2.VisualHit is Ellipse ellipseDock2)
                    {

                        canvas.Children.Insert(79,_tempLine);
                        var Card1 = XMLFiles.FindParent<DragDropCardView>(ellipseDock1);
                        var Card2 = XMLFiles.FindParent<DragDropCardView>(ellipseDock2);

                        var _nodeLine = new NodeLine(_tempLine, true, Card1.ServerViewModel.Id, Card2.ServerViewModel.Id);

                        if (ellipseDock1.Name == "DockLeft")
                            _nodeLine.Dock = Helper.Dock.Left;
                        else
                            _nodeLine.Dock = Helper.Dock.Right;

                        Card1.NodeLines.Add(_nodeLine);

                        _nodeLine = new NodeLine(_tempLine, false, Card1.ServerViewModel.Id, Card2.ServerViewModel.Id);

                        if (ellipseDock2.Name == "DockLeft")
                            _nodeLine.Dock = Helper.Dock.Left;
                        else
                            _nodeLine.Dock = Helper.Dock.Right;

                        Card2.NodeLines.Add(_nodeLine);

                    }

                }



            }
            catch (Exception) { }

           


        }
        #endregion
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
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
