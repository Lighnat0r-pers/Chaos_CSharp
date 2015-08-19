using System;
using System.Xml;
using System.Diagnostics;

namespace GTAVC_Chaos
{
    static class FileLoader
    {
        public static MemoryAddress[] LoadMemoryAddresses(string filename)
        {
            XmlTextReader reader = new XmlTextReader(filename);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                       Debug.WriteLine("<" + reader.Name);
                       Debug.WriteLine(">");
                       break;
                    case XmlNodeType.Text:
                       Debug.WriteLine(reader.Value);
                       break;
                    case XmlNodeType.EndElement:
                       Debug.WriteLine("</" + reader.Name);
                       Debug.WriteLine(">");
                       break;
                }
            }

            return new MemoryAddress[0];
        }
    }
}
