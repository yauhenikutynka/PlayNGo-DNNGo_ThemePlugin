using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace DNNGo.Modules.ThemePlugin 
{
    /// <summary>
    /// XML实体的扩展属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class XmlEntityAttributes : Attribute
    {

        private String _xPath = String.Empty;
        /// <summary>
        /// XML实体的解析路径
        /// </summary>
        public String xPath
        {
            get { return _xPath; }
            set { _xPath = value; }
        }





        /// <summary>
        /// 实体特性
        /// </summary>
        /// <param name="__xPath">XML路径</param>
        public XmlEntityAttributes(String __xPath)
        {
            _xPath = __xPath;
        }




        /// <summary>
        /// 检索应用于类型成员的自定义属性。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XmlEntityAttributes GetCustomAttribute(MemberInfo element)
        {
            return GetCustomAttribute(element, typeof(XmlEntityAttributes)) as XmlEntityAttributes;
        }

    }
}