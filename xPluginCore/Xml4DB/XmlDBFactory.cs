namespace DNNGo.Modules.ThemePlugin.Xml4DB
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    public class XmlDBFactory
    {
        public static XmlDB CreatXmlDB(string mDBName, string mDBFileName, int mDBVersion)
        {
            if (!File.Exists(mDBFileName))
            {
                if (!Directory.Exists(new FileInfo(mDBFileName).DirectoryName)) Directory.CreateDirectory(new FileInfo(mDBFileName).DirectoryName);
                new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new object[] { new XElement("XmlDB", new object[] { new XAttribute("DBName", mDBName), new XAttribute("DBVersion", mDBVersion) }) }).Save(mDBFileName);
            }
            return new XmlDB(mDBName, mDBFileName, mDBVersion);
        }

        public static XmlDB LoadXmlDB(string mDBFileName)
        {
            return new XmlDB(mDBFileName);
        }

        public static XmlDB UpdateXmlDB(string mDBName, string mDBFileName, int mDBVersion)
        {
            XmlDB ldb = null;
            if (File.Exists(mDBFileName))
            {
                XmlDB ldb2 = new XmlDB(mDBName, mDBFileName, mDBVersion);
                if (LoadXmlDB(mDBFileName).DBVersion < ldb2.DBVersion) ldb = CreatXmlDB(mDBName, mDBFileName, mDBVersion);
            }
            return ldb;
        }
    }
}
