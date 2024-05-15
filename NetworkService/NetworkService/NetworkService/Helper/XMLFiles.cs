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

namespace NetworkService.Repositories
{
    public class XMLFiles
    {
        public ObservableCollection<ServerViewModel> serializableObject = EntitiesViewModel.EntityColection;

        public void SerializeObject()
        {
            string directoryPath = MyPath.MyXMLData();
            string fileName = Path.Combine(directoryPath, "ServerCollection.xml");

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
            string fileName = Path.Combine(MyPath.MyXMLData(), "ServerCollection.xml");
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



    }
}
