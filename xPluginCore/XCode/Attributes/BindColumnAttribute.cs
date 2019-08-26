using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// ����ָ���������������󶨵������ݱ���ֶ���
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BindColumnAttribute : Attribute
    {
        private String _Name;
        /// <summary>�ֶ���</summary>
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

        private String _DefaultValue;
        /// <summary>Ĭ��ֵ</summary>
        public String DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }

        private Int32 _Order;
        /// <summary>˳��</summary>
        public Int32 Order
        {
            get { return _Order; }
            set { _Order = value; }
        }



        private String _RawType;
        /// <summary>
        /// ԭʼ��������
        /// </summary>
        public String RawType
        {
            get { return _RawType; }
            set { _RawType = value; }
        }

        private Int32 _Precision;
        /// <summary>����</summary>
        public Int32 Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }

        private Int32 _Scale;
        /// <summary>λ��</summary>
        public Int32 Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }

        private Boolean _IsUnicode;
        /// <summary>�Ƿ�Unicode</summary>
        public Boolean IsUnicode
        {
            get { return _IsUnicode; }
            set { _IsUnicode = value; }
        }




        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name">�ֶ���</param>
        public BindColumnAttribute(String name)
        {
            Name = name;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="order">˳��</param>
        /// <param name="name">�ֶ���</param>
        /// <param name="description">����</param>
        ///  <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="rawType">ԭʼ��������</param>
        ///  <param name="precision">����</param>
        ///  <param name="scale">λ��</param>
        ///  <param name="isUnicode">�Ƿ�Unicode</param>
        public BindColumnAttribute(int order, string name, string description, string defaultValue, string rawType, int precision, int scale, bool isUnicode)
        {
            Name = name;
            Order = order;
            Description = description;
            DefaultValue = defaultValue;
            RawType = rawType;
            Precision = precision;
            Scale = scale;
            IsUnicode = isUnicode;
        }





        /// <summary>
        /// ����Ӧ�������ͳ�Ա���Զ������ԡ�
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static BindColumnAttribute GetCustomAttribute(MemberInfo element)
        {
            return GetCustomAttribute(element, typeof(BindColumnAttribute)) as BindColumnAttribute;
        }
    }
}