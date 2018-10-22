using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Models
{
    public static class XmlSave
    {
        public static string path = "data.xml";

        public static void Save(Connection[] array)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Connection[]));
            TextWriter writer = new StreamWriter(path);
            ser.Serialize(writer, array);
            writer.Close();
        }

        public static Connection[] Read()
        {
            if (File.Exists(path))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Connection[]));
                FileStream stream = new FileStream(path, FileMode.Open);
                return (Connection[])ser.Deserialize(stream);
            }
            else
            {
                return new Connection[0];
            }
        }
    }
}
