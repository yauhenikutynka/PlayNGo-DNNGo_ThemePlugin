using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Data;
using System.Collections;
using System.Reflection;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 转换
    /// </summary>
    public class ConvertTo
    {

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T t, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());

                return (T)xz.Deserialize(sr);
            }
        }


        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>( string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));

                return (T)xz.Deserialize(sr);
            }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object FormatValue(string _value, Type t)
        {
            object o = new object();

            if (!String.IsNullOrEmpty(_value))
            {

                if (t == typeof(DateTime))
                {
                    string[] expectedFormats = { "G", "g", "f", "F" };

                    DateTime oTime = DateTime.Now;

                    //当前系统的语言
                    if (DateTime.TryParseExact(_value, expectedFormats, System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out oTime))
                    {
                        o = oTime;
                    }
                    else if (DateTime.TryParseExact(_value, expectedFormats, new CultureInfo("en-US", true), DateTimeStyles.AllowWhiteSpaces, out oTime))//英语
                    {
                        o = oTime;
                    }
                    else
                    {
                        //德语、中文、法语、俄语、希腊语、西班牙语
                        string[] Cultures = { "de-DE", "zh-CN", "fr-FR", "ru-RU", "el-GR", "es-ES" };
                        foreach (String Culture in Cultures)
                        {
                            if (DateTime.TryParseExact(_value, expectedFormats, new CultureInfo(Culture, true), DateTimeStyles.AllowWhiteSpaces, out oTime))
                            {
                                o = oTime;
                            }
                        }
                    }
                }
                else if (t == typeof(Int32))
                {
                    o = int.Parse(_value);
                }
                else if (t == typeof(Double))
                {
                    o = Double.Parse(_value);
                }
                else if (t == typeof(Boolean))
                {
                    o = Boolean.Parse(_value.ToLower());
                }
                else if (t == typeof(Enum))
                {
                    o = Enum.Parse(t, _value);
                }
                else
                {
                    o = _value;
                }
            }
            return o;
        }


        /// <summary>
        /// 获取XML配置文件
        /// </summary>
        /// <param name="dirs">文件夹列表</param>
        /// <returns></returns>
        public static List<XmlDBEntity> GetXmlDB(List<DirectoryInfo> dirs)
        {
            return GetXmlDB(dirs.ToArray());
        }



        /// <summary>
        /// 获取XML配置文件
        /// </summary>
        /// <param name="dirs">文件夹列表</param>
        /// <returns></returns>
        public static List<XmlDBEntity> GetXmlDB(DirectoryInfo[] dirs)
        {
            List<XmlDBEntity> XmlDBs = new List<XmlDBEntity>();
            foreach (DirectoryInfo EffectDir in dirs)
            {
                if (EffectDir.Exists)
                {
                    //获取效果数据的XML
                    String XmlDBPath = String.Format("{0}\\EffectDB.xml", EffectDir.FullName);
                    if (File.Exists(XmlDBPath))
                    {
                        XmlFormat xf = new XmlFormat(XmlDBPath);
                        XmlDBs.Add(xf.ToItem<XmlDBEntity>());
                    }

                }
            }
            return XmlDBs;
        }


        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


        /// <summary>
        /// Ilist<T> 转换成 DataSet
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ConvertToDataSet<T>(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                   
                    string name = pi.Name;

                    if (name != "Item" && pi.PropertyType != typeof(Object))
                    {

                        if (dt.Columns[name] == null)
                        {
                            column = new DataColumn(name, pi.PropertyType);
                            dt.Columns.Add(column);
                        }

                        row[name] = pi.GetValue(t, null);
                    }
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ConvertTo.ToDataTable<T>(list, null);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.Name != "Item")
                    {
                        if (propertyNameList.Count == 0)
                        {
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                                result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.Name != "Item")
                        {
                            if (propertyNameList.Count == 0)
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                            else
                            {
                                if (propertyNameList.Contains(pi.Name))
                                {
                                    object obj = pi.GetValue(list[i], null);
                                    tempList.Add(obj);
                                }
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}