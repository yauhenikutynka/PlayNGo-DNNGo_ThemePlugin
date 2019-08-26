using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;

namespace DNNGo.Modules.ThemePlugin
{


    /// <summary>
    /// 选项实体
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("选项实体")]
    [XmlEntityAttributes("DNNGo_SkinPlugin//Settings//Setting")]
    public class SettingEntity
    {
        private String _Name = String.Empty;
        /// <summary>
        /// 参数名
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private String _Alias = String.Empty;
        /// <summary>
        /// 别名
        /// </summary>
        public String Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }

        private Int32 _Width = 100;
        /// <summary>
        /// 文本框的长度
        /// </summary>
        public Int32 Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private Int32 _Rows = 1;
        /// <summary>
        /// 文本框的高度
        /// </summary>
        public Int32 Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        private String _DefaultValue = String.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        public String DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }


        private String _ControlType = "text";
        /// <summary>
        /// 是否列表控件()
        /// </summary>
        public String ControlType
        {
            get { return _ControlType; }
            set { _ControlType = value; }
        }


        private String _Direction = "Vertical";
        /// <summary>
        /// 控件布局(Horizontal/Vertical)
        /// </summary>
        public String Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }


        private String _ListContent = String.Empty;
        /// <summary>
        /// 列表内容
        /// </summary>
        public String ListContent
        {
            get { return _ListContent; }
            set { _ListContent = value; }
        }
 
        private String _Description = String.Empty;
        /// <summary>
        /// 参数描述
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private Boolean _Required = false;
        /// <summary>
        /// 是否必填
        /// </summary>
        public Boolean Required
        {
            get { return _Required; }
            set { _Required = value; }
        }

        private String _Verification = String.Empty;
        /// <summary>
        /// 验证类型
        /// </summary>
        public String Verification
        {
            get { return _Verification; }
            set { _Verification = value; }
        }


        private String _Group = "Basic Options";
        /// <summary>
        /// 分组
        /// </summary>
        public String Group
        {
            get { return _Group; }
            set { _Group = value; }
        }


        private String _Categories = "Basic Options";
        /// <summary>
        /// 类别
        /// </summary>
        public String Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }



        private String _Layout = "Left";
        /// <summary>
        /// 布局(Left,Right)
        /// </summary>
        public String Layout
        {
            get { return _Layout; }
            set { _Layout = value; }
        }


        public SettingEntity Clone()
        {
            return this.MemberwiseClone() as SettingEntity;
        }

    }
}