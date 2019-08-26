using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetNuke.Framework.Providers;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 用于指定数据类所绑定到的数据表的表名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class BindTableAttribute : Attribute
    {
        private String _Name;
        /// <summary>表名</summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private String _Description;
        /// <summary>描述</summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private String _ConnName;
        /// <summary>连接名</summary>
        public String ConnName
        {
            get { return !String.IsNullOrEmpty(_ConnName) ? _ConnName : GetConnName(); }
            set { _ConnName = value; }
        }

        private String _Prefix = String.Empty;
        /// <summary>
        /// 前缀
        /// </summary>
        public String Prefix
        {
            get { return !String.IsNullOrEmpty(_Prefix) ? _Prefix : GetPrefix(); }
            set { _Prefix = value; }
        }


        private String _Owner = String.Empty;
        /// <summary>
        /// 数据库所有者
        /// </summary>
        public String Owner
        {
            get { return !String.IsNullOrEmpty(_Owner) ? _Owner : GetOwner(); }
            set { _Owner = value; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">表名</param>
        public BindTableAttribute(String name)
        {
            Name = name;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="description">描述</param>
        public BindTableAttribute(String name, String description)
        {
            Name = name;
            Description = description;
            ConnName = GetConnName();
        }

        private const string providerType = "data";
        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        /// <summary>
        /// 获取链接名
        /// </summary>
        /// <returns></returns>
        private String GetConnName()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];


            //设置链接名
            _ConnName = provider.Attributes["connectionStringName"];
            if (String.IsNullOrEmpty(_ConnName))
            {
                _ConnName = "SiteSqlServer";
            }
            return _ConnName;
        }

        /// <summary>
        /// 获取表前缀
        /// </summary>
        /// <returns></returns>
        private String GetPrefix()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            String __Prefix = provider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(__Prefix) && !(__Prefix.IndexOf("_") >= 0))
            {
                __Prefix = String.Format("{0}_", provider.Attributes["objectQualifier"]);
            }
            //设置表名前缀
            return __Prefix;
        }

        /// <summary>
        /// 获取数据库所有者
        /// </summary>
        /// <returns></returns>
        private String GetOwner()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];

            //设置表名前缀
            _Owner = provider.Attributes["databaseOwner"];
            if (String.IsNullOrEmpty(_Owner))
            {
                _Owner = "dbo";
            }
            else if (!String.IsNullOrEmpty(_Owner) && _Owner.IndexOf(".") >= 0)
            {
                _Owner = _Owner.Replace(".", "");
            }

            return _Owner;
        }


        /// <summary>
        /// 检索应用于类型成员的自定义属性。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static BindTableAttribute GetCustomAttribute(MemberInfo element)
        {
            return GetCustomAttribute(element, typeof(BindTableAttribute)) as BindTableAttribute;
        }
    }
}