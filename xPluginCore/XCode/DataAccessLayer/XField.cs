using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 字段构架
    /// </summary>
    [DebuggerDisplay("ID={ID} Name={Name} FieldType={FieldType} Readme={Readme}")]
    [Serializable]
    public class XField
    {
        #region 属性
        private Int32 _ID;
        /// <summary>
        /// 顺序编号
        /// </summary>
        [XmlAttribute]
        public Int32 ID { get { return _ID; } set { _ID = value; } }

        private String _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public String Name { get { return _Name; } set { _Name = value; } }

        private String _FieldType;
        /// <summary>
        /// 字段类型
        /// </summary>
        [XmlAttribute]
        public String FieldType { get { return _FieldType; } set { _FieldType = value; } }

        private Boolean _Identity;
        /// <summary>
        /// 标识
        /// </summary>
        [XmlAttribute]
        public Boolean Identity { get { return _Identity; } set { _Identity = value; } }

        private Boolean _PrimaryKey;
        /// <summary>
        /// 主键
        /// </summary>
        [XmlAttribute]
        public Boolean PrimaryKey { get { return _PrimaryKey; } set { _PrimaryKey = value; } }

        private Boolean _ForeignKey;
        /// <summary>
        /// 外键
        /// </summary>
        [XmlAttribute]
        public Boolean ForeignKey { get { return _ForeignKey; } set { _ForeignKey = value; } }

        private Int32 _Length;
        /// <summary>
        /// 长度
        /// </summary>
        [XmlAttribute]
        public Int32 Length { get { return _Length; } set { _Length = value; } }

        private Int32 _NumOfByte;
        /// <summary>
        /// 字节数
        /// </summary>
        [XmlAttribute]
        public Int32 NumOfByte { get { return _NumOfByte; } set { _NumOfByte = value; } }

        private Int32 _Digit;
        /// <summary>
        /// 位数
        /// </summary>
        [XmlAttribute]
        public Int32 Digit { get { return _Digit; } set { _Digit = value; } }

        private Boolean _Nullable;
        /// <summary>
        /// 允许空
        /// </summary>
        [XmlAttribute]
        public Boolean Nullable { get { return _Nullable; } set { _Nullable = value; } }

        private String _Default;
        /// <summary>
        /// 默认值
        /// </summary>
        [XmlAttribute]
        public String Default { get { return _Default; } set { _Default = value; } }

        private String _Readme;
        /// <summary>
        /// 说明
        /// </summary>
        [XmlAttribute]
        public String Readme { get { return _Readme; } set { _Readme = value; } }
        #endregion

        #region 中英对照表
        /// <summary>
        /// 英文名
        /// </summary>
        protected static readonly String[] eNames = new String[] { "ID", "Name", "FieldType", "Identity", "PrimaryKey", "ForeignKey", "ForeignTableName", "ForeignTablePrimaryName", "Length", "NumOfByte", "Digit", "Nullable", "Default", "Readme" };
        /// <summary>
        /// 中文名
        /// </summary>
        protected static readonly String[] cNames = new String[] { "字段序号", "字段名", "类型", "标识", "主键", "外键", "外键表", "外键表主键名", "长度", "占用字节数", "小数位数", "允许空", "默认值", "字段说明" };
        #endregion

        #region 属性信息
        private static IList<PropertyInfo> _PropertyInfos;
        /// <summary>
        /// 属性信息
        /// </summary>
        public static IList<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_PropertyInfos != null) return _PropertyInfos;
                _PropertyInfos = new List<PropertyInfo>(typeof(XField).GetProperties());
                return _PropertyInfos;
            }
        }
        #endregion

        #region 外键
        private String _ForeignTableName;
        /// <summary>
        /// 外键表
        /// </summary>
        [XmlAttribute]
        public String ForeignTableName { get { return _ForeignTableName; } set { _ForeignTableName = value; } }

        private String _ForeignTablePrimaryName;
        /// <summary>
        /// 外键表主键名
        /// </summary>
        [XmlAttribute]
        public String ForeignTablePrimaryName { get { return _ForeignTablePrimaryName; } set { _ForeignTablePrimaryName = value; } }
        #endregion

        #region 加载数据
        ///// <summary>
        /////  从DataRow加载数据
        ///// </summary>
        ///// <param name="dr"></param>
        //public void LoadDataRow(DataRow dr)
        //{
        //    //foreach (PropertyInfo pi in PropertyInfos)
        //    //{
        //    //    if (dr.Table.Columns.Contains(pi.Name) && dr[pi.Name] != DBNull.Value ||
        //    //         dr.Table.Columns.Contains(CNameByEName(pi.Name)) && dr[CNameByEName(pi.Name)] != DBNull.Value)
        //    //    {
        //    //        pi.SetValue(this, dr[pi.Name], null);
        //    //    }
        //    //}
        //}

        /// <summary>
        /// 英文名转中文名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String CNameByEName(String name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            for (Int32 i = 0; i < eNames.Length; i++)
            {
                if (eNames[i] == name) return cNames[i];
            }
            return null;
        }

        /// <summary>
        /// 中文名转英文名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String ENameByCName(String name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            for (Int32 i = 0; i < cNames.Length; i++)
            {
                if (cNames[i] == name) return eNames[i];
            }
            return null;
        }
        #endregion

        #region 比较
        /// <summary>
        /// 重载相等操作符
        /// </summary>
        public static bool operator ==(XField field1, XField field2)
        {
            return Object.Equals(field1, field2);
        }
        /// <summary>
        /// 重载不等操作符
        /// </summary>
        public static bool operator !=(XField field1, XField field2)
        {
            return !(field1 == field2);//调用==，取反
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 确定指定的 Object 是否等于当前的 Object。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            XField field = obj as XField;
            if (field == null) return false;

            if (this.Name != field.Name) return false;
            if (this.FieldType != field.FieldType) return false;
            if (this.Identity != field.Identity) return false;
            if (this.PrimaryKey != field.PrimaryKey) return false;
            if (this.Length != field.Length) return false;
            if (this.NumOfByte != field.NumOfByte) return false;
            if (this.Digit != field.Digit) return false;
            if (this.Nullable != field.Nullable) return false;
            if (this.Default != field.Default) return false;
            if (this.Readme != field.Readme) return false;

            return true;
        }
        #endregion
    }
}