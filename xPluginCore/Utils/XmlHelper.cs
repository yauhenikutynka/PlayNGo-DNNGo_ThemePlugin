using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Data;
using System.IO;
 

namespace DNNGo.Modules.ThemePlugin
{
    public class XmlHelper
    {
        /// <summary>
        /// 获取文件处以应用程序中的根目录
        /// </summary>
        /// <param name="fileName">文件全名称</param>
        /// <returns>文件的全路径</returns>
        public static string MapPath(string fileName)
        {
            Assembly Asm = Assembly.GetExecutingAssembly();

            string codeBase = Asm.CodeBase;

            return codeBase.Substring(8, (codeBase.LastIndexOf("/") - 11)) + fileName;//获取配置文件的路径
        }

        /// <summary>
        /// 返回用户操作的SQL语句
        /// </summary>
        /// <param name="sqlNodeName">XML的操作节点</param>
        /// <returns>操作的sql语句</returns>
        public static string GetSQLCommand(string filePath, string fullNodeName)
        {
            string sql = null;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNode xmlNode = xmlDoc.SelectSingleNode(fullNodeName);
                sql = xmlNode.InnerText;  //返回节点 的文字
            }
            catch (Exception err)
            {
                throw err;
            }

            return sql;
        }

        /// <summary>
        /// 返回用户操作的SQL语句
        /// </summary>
        /// <param name="sqlNodeName">XML的操作节点</param>
        /// <returns>操作的sql语句</returns>
        public string GetXMLPath(string strXMlFileName)
        {
            string m_strFullPath = "";
            Assembly Asm = Assembly.GetExecutingAssembly();
            //获取文件的路径                  
            //m_strFullPath = Asm.Location.Substring(0, (Asm.Location.LastIndexOf("\\") + 1)) + "XMLSql.xml";
            m_strFullPath = AppDomain.CurrentDomain.BaseDirectory + "XMLLibrary\\" + strXMlFileName;
            return m_strFullPath;
        }
        /// <summary>
        /// 获取XML数据库中的数据的方法
        /// </summary>
        /// <param name="strFilePath">传入文件路径</param>
        /// <returns>返回一个数据集</returns>
        public DataSet GetAllDataFromXML(string strFilePath)
        {
            DataSet ds = new DataSet();
            FileInfo fileInfo = new FileInfo(strFilePath);
            if (fileInfo.Exists)
            {
                try
                {
                    ds.ReadXml(strFilePath);
                }
                catch { }
            }
            else
            {
                ds = null;
            }
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count < 1)
                    ds = null;
            }
            return ds;
        }

        /// <summary>
        /// 获取指定目录下所有子节点的值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeDir">节点目录</param>
        /// <returns></returns>
        public Hashtable GetNodeList(string strFileName, string nodeDir)
        {
            Hashtable strNodeList = new Hashtable();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 

                foreach (XmlNode xn in nodeList)     //遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 
                    strNodeList.Add(xe.GetAttribute("id").ToString(), xe.InnerText.Trim());
                }

            }
            catch (Exception)
            {

                throw;
            }
            return strNodeList;
        }


        /// <summary>
        /// 获取指定目录下所有子节点的值
        /// </summary>
        /// <param name="StrXml">XML字符串</param>
        /// <param name="nodeDir">节点目录</param>
        /// <returns></returns>
        public List<Hashtable> GetNodeItemList(string StrXml, string nodeDir)
        {
            List<Hashtable> strNodeList = new  List<Hashtable>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(StrXml);
                
                XmlNodeList nodeList = xmlDoc.SelectNodes(nodeDir);

                foreach (XmlNode xn in nodeList)
                {
                    Hashtable ChildNode = new Hashtable();
                    foreach (XmlNode item in xn.ChildNodes)     //遍历所有子节点 
                    {
                        XmlElement xe = (XmlElement)item;  //将子节点类型转换为XmlElement类型 

                        ChildNode.Add(xe.Name, xe.InnerText.Trim());
                    }
                    strNodeList.Add(ChildNode);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return strNodeList;
        }


        /// <summary>
        /// 获取指定节点的值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="value">设置后的值</param>
        /// <param name="nodeDir">指定节点所在的节点目录</param>
        /// <returns></returns>
        public string GetNodeValue(string strFileName, string nodeName, string nodeDir)
        {
            string value = null;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 

                foreach (XmlNode xn in nodeList)    //遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 

                    if (xe.Name == nodeName)
                    {
                        value = xe.InnerText.Trim();

                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return value;
        }

        /// <summary>
        /// 获取指定节点下面对应属性的值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeDir">指定节点所在的节点目录</param>
        /// <param name="attribute">节点对应的属性名称</param>
        /// <returns></returns>
        public string GetNodeValue(string strFileName, string nodeName, string nodeDir, string attribute)
        {
            string value = null;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 

                foreach (XmlNode xn in nodeList)    //遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 

                    if (xe.Name == nodeName)
                    {
                        //value = xe.InnerText.Trim();
                        value = (xe).Attributes[attribute].Value;
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return value;
        }

        /// <summary>
        /// 修改指定结点值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="value">设置后的值</param>
        /// <param name="nodeDir">指定节点所在的节点目录</param>
        /// <returns></returns>
        public bool UpdateNoteValue(string strFileName, string nodeName, string value, string nodeDir)
        {
            bool isSucceed = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 

                foreach (XmlNode xn in nodeList)    //遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 

                    if (xe.Name == nodeName)
                    {
                        xe.InnerText = value;

                        isSucceed = true;
                        break;
                    }
                }

                xmlDoc.Save(strFileName);
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return isSucceed;
        }

        /// <summary>
        /// 修改指定结点值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="value">设置后的值</param>
        /// <param name="nodeDir">指定节点所在的节点目录</param>
        /// <returns></returns>
        public bool UpdateNoteValue(string strFileName, string nodeName, string value, string nodeDir, string attribute, string attributeValue)
        {
            bool isSucceed = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 

                foreach (XmlNode xn in nodeList)    //遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 

                    if (xe.Name == nodeName)
                    {
                        xe.InnerText = value;
                        (xe).Attributes[attribute].Value = attributeValue;
                        isSucceed = true;
                        break;
                    }
                }

                xmlDoc.Save(strFileName);
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return isSucceed;
        }


        /// <summary>
        /// 修改指定结点值
        /// </summary>
        /// <param name="strFileName">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="value">设置后的值</param>
        /// <param name="nodeDir">指定节点所在的节点目录</param>
        /// <returns></returns>
        public bool UpdateNoteValue(string strFileName, Hashtable hstable, string nodeDir)
        {
            bool isSucceed = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFileName);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(nodeDir).ChildNodes;//获取bookstore节点的所有子节点 
                foreach (DictionaryEntry item in hstable)
                {
                    foreach (XmlNode xn in nodeList)    //遍历所有子节点 
                    {
                        XmlElement xe = (XmlElement)xn;  //将子节点类型转换为XmlElement类型 

                        if (xe.Name == item.Key.ToString())
                        {
                            xe.InnerText = item.Value.ToString();

                            isSucceed = true;
                            break;
                        }
                    }
                }




                xmlDoc.Save(strFileName);
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return isSucceed;
        }

    }
}