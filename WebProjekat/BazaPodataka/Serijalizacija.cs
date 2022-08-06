using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WebProjekat.BazaPodataka
{ 
    //kod sa HCIja i od prosle godine
    public class Serijalizacija
    {
        public static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";

        public static void SacuvajListu<T>(List<T> serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }
            string file = path + fileName;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(file);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        //Deserijalizacija
        public static List<T> UcitajListu<T>(string fileName)
        {
            string file = path + fileName;
            if (string.IsNullOrEmpty(file)) { return default(List<T>); }

            List<T> objectOut = default(List<T>);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<T>);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (List<T>)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }
    }
}