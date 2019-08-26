using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 参数项
    /// </summary>
    [XmlEntityAttributes("OptionItems//OptionItem")]
    public class OptionItem
    {
        private String _Text = String.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        public String Text
        {
            get { return _Text; }
            set { _Text = value; }
        }


        private String _Value = String.Empty;
        /// <summary>
        /// 值
        /// </summary>
        public String Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}