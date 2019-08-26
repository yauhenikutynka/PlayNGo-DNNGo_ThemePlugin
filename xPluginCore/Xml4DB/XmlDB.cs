namespace DNNGo.Modules.ThemePlugin.Xml4DB
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public class XmlDB
    {
        private string mDBFileName;
        private string mDBName;
        private int mDBVersion;
        private XElement mXElement;

        public XmlDB(string mDBFileName)
        {
            this.mDBFileName = mDBFileName;
            if (File.Exists(mDBFileName)) this.mXElement = XElement.Load(mDBFileName);
        }

        public XmlDB(string mDBName, string mDBFileName, int mDBVersion)
        {
            this.mDBName = mDBName;
            this.mDBFileName = mDBFileName;
            this.mDBVersion = mDBVersion;
            if (File.Exists(mDBFileName)) this.mXElement = XElement.Load(mDBFileName);
        }

        public void Commit()
        {
            this.mXElement.Save(this.mDBFileName);
        }

        public void Delete(object mObject)
        {
            IEnumerable<XElement> source = Dynamic.LoadAndRules(this.mXElement, mObject);
            if (source.Count<XElement>() > 0) source.Remove<XElement>();
        }

        public void Insert(object mObject)
        {
            Type type = mObject.GetType();
            PropertyInfo[] properties = type.GetProperties();
            object[] content = new object[properties.Length];
            for (int i = 0; i < content.Length; i++)
            {
                object obj2 = properties[i].GetValue(mObject, null);

                if (properties[i].PropertyType == typeof(DateTime))
                {
                    content[i] = new XElement(properties[i].Name, obj2 != null ? Convert.ToDateTime(obj2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) : "");
                }
                else {
                    content[i] = new XElement(properties[i].Name, obj2 != null ? obj2.ToString() : "");
                }

                
            }
            this.mXElement.Add(new XElement(type.Name, content));
        }

        public List<T> Read<T>(T mObject)
        {
            List<T> list = new List<T>();
            Type type = mObject.GetType();
            string name = type.Name;
            PropertyInfo[] properties = type.GetProperties();
            IEnumerable<XElement> enumerable = Dynamic.LoadAndRules(this.mXElement, mObject);
            foreach (XElement element in enumerable)
            {
                object obj2 = Activator.CreateInstance(type);
                PropertyInfo[] infoArray2 = obj2.GetType().GetProperties();
                for (int i = 0; i < infoArray2.Length; i++)
                {
                    XElement xelement = element.Element(infoArray2[i].Name);
                    if(xelement!= null )
                    {

                        object obj3 = xelement.Value;

                        if (infoArray2[i].PropertyType == typeof(int))
                        {
                            infoArray2[i].SetValue(obj2, Convert.ToInt32(obj3), null);

                        }
                        else if (infoArray2[i].PropertyType == typeof(string))
                        {
                            infoArray2[i].SetValue(obj2, Convert.ToString(obj3), null);
                        }
                        else if (infoArray2[i].PropertyType == typeof(bool))
                        {
                            infoArray2[i].SetValue(obj2, Convert.ToBoolean(obj3), null);
                        }
                        else if (infoArray2[i].PropertyType.Name == "Nullable`1")
                        {
                            infoArray2[i].SetValue(obj2, Convert.ToBoolean(obj3), null);
                        }
                        else if (infoArray2[i].PropertyType == typeof(DateTime))
                        {
                            //infoArray2[i].SetValue(obj2, Convert.ToDateTime(obj3, new CultureInfo("en-US", false)), null);
                            infoArray2[i].SetValue(obj2, Convert.ToDateTime(obj3, CultureInfo.InvariantCulture), null);
                        }
                        else
                        {
                            infoArray2[i].SetValue(obj2, obj3, null);
                        }


                    }

                }
                list.Add((T) obj2);
            }
            return list;
        }

        public void Update(object mObject1, object mObject2)
        {
            IEnumerable<XElement> source = Dynamic.LoadAndRules(this.mXElement, mObject1);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type type = mObject2.GetType();
            string name = type.Name;
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object obj2 = info.GetValue(mObject2, null);
                if (obj2 != null)
                {
                    if (info.PropertyType == typeof(int))
                    {
                        if (Convert.ToInt32(obj2) != 0) dictionary.Add(info.Name, Convert.ToString(obj2));
                    }
                    else if (info.PropertyType == typeof(string))
                    {
                        dictionary.Add(info.Name, Convert.ToString(obj2));
                    }
                    else if (info.PropertyType == typeof(bool))
                    {
                        dictionary.Add(info.Name, Convert.ToString(obj2));
                    }
                    else if (info.PropertyType == typeof(DateTime))
                    {
                        //dictionary.Add(info.Name,  Convert.ToString(obj2, new CultureInfo("en-US", false)));
                        dictionary.Add(info.Name,Convert.ToDateTime( obj2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
                    }

                }
            }
            if (source.Count<XElement>() > 0)
            {
                foreach (XElement element in source)
                {
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        var xelement = element.Element(pair.Key);
                        if (xelement != null)
                        {
                            element.Element(pair.Key).SetValue(pair.Value);
                        }
                        else
                        {
                            element.Add(new XElement(pair.Key, pair.Value));
                        }
                        
                    }
                }
            }
        }

        public string DBFileName
        {
            get
            {
                return Convert.ToString(this.mXElement.Element("XmlDB").Attribute("DBName").Value);
            }
            set
            {
                this.mXElement.Element("XmlDB").Attribute("DBName").Value = value;
            }
        }

        public string DBName
        {
            get
            {
                return this.mDBName;
            }
        }

        public int DBVersion
        {
            get
            {
                return Convert.ToInt32(this.mXElement.Element("XmlDB").Attribute("DBVersion").Value);
            }
            set
            {
                this.mXElement.Element("XmlDB").Attribute("DBVersion").Value = value.ToString();
            }
        }
    }
}
