using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DotNetNuke.Framework.Providers;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// ����ָ�����������󶨵������ݱ�ı���
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class BindTableAttribute : Attribute
    {
        private String _Name;
        /// <summary>����</summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private String _Description;
        /// <summary>����</summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private String _ConnName;
        /// <summary>������</summary>
        public String ConnName
        {
            get { return !String.IsNullOrEmpty(_ConnName) ? _ConnName : GetConnName(); }
            set { _ConnName = value; }
        }

        private String _Prefix = String.Empty;
        /// <summary>
        /// ǰ׺
        /// </summary>
        public String Prefix
        {
            get { return !String.IsNullOrEmpty(_Prefix) ? _Prefix : GetPrefix(); }
            set { _Prefix = value; }
        }


        private String _Owner = String.Empty;
        /// <summary>
        /// ���ݿ�������
        /// </summary>
        public String Owner
        {
            get { return !String.IsNullOrEmpty(_Owner) ? _Owner : GetOwner(); }
            set { _Owner = value; }
        }


        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name">����</param>
        public BindTableAttribute(String name)
        {
            Name = name;
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="description">����</param>
        public BindTableAttribute(String name, String description)
        {
            Name = name;
            Description = description;
            ConnName = GetConnName();
        }

        private const string providerType = "data";
        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        private String GetConnName()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];


            //����������
            _ConnName = provider.Attributes["connectionStringName"];
            if (String.IsNullOrEmpty(_ConnName))
            {
                _ConnName = "SiteSqlServer";
            }
            return _ConnName;
        }

        /// <summary>
        /// ��ȡ��ǰ׺
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
            //���ñ���ǰ׺
            return __Prefix;
        }

        /// <summary>
        /// ��ȡ���ݿ�������
        /// </summary>
        /// <returns></returns>
        private String GetOwner()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];

            //���ñ���ǰ׺
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
        /// ����Ӧ�������ͳ�Ա���Զ������ԡ�
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static BindTableAttribute GetCustomAttribute(MemberInfo element)
        {
            return GetCustomAttribute(element, typeof(BindTableAttribute)) as BindTableAttribute;
        }
    }
}