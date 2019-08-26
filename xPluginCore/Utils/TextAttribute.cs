using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 键值集合
    /// </summary>
    public class EnumEntity
    {
        private string _Text;
        /// <summary>
        /// 枚举名
        /// </summary>
        public string Text
        {
            set { _Text = value; }
            get { return _Text; }
        }


        private int _Value;
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value
        {
            set { _Value = value; }
            get { return _Value; }
        }



    }


    /// <summary>
    /// TextAttribute
    /// </summary>
    public class TextAttribute : Attribute
    {
        string _Text;
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        public TextAttribute(string text)
        {
            this._Text = text;
        }

    }
}
