using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;

namespace DNNGo.Modules.ThemePlugin
{


    /// <summary>
    /// 键值对实体
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("键值对实体")]
    public class KeyValueEntity
    {

        private String _Key = String.Empty;
        /// <summary>
        /// 键
        /// </summary>
        public String Key
        {
            get { return _Key; }
            set { _Key = value; }
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



        private String _Parent = String.Empty;
        /// <summary>
        /// 上级
        /// </summary>
        public String Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }


        public KeyValueEntity()
        { }

        public KeyValueEntity(String __Key, String __Value)
        {
            _Key = __Key;
            _Value = __Value;
        }

        public KeyValueEntity(String __Key, String __Value, String __Parent)
        {
            _Key = __Key;
            _Value = __Value;
            _Parent = __Parent;
        }

    }
}