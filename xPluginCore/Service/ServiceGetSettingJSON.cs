using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    public class ServiceGetSettingJSON : iService
    {
        public ServiceGetSettingJSON()
        {
            IsResponseWrite = true;
        }


        /// <summary>
        /// 是否写入输出
        /// </summary>
        public bool IsResponseWrite
        {
            get;
            set;
        }


        private String _ResponseString;
        /// <summary>
        /// 输出字符串
        /// </summary>
        public string ResponseString
        {
            get
            {
                return _ResponseString;
            }
            set
            {
                _ResponseString = value;
            }
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get
            {
                return "Get Settings JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            List<SettingEntity> OptionList =  Context.OptionList;
            List<KeyValueEntity> ItemValues = Context.ItemValues;
            if(OptionList!= null && OptionList.Count >0)
            {
                foreach (var OptionItemValue in OptionList)
                {
                    if (ItemValues.Exists(r => r.Key == OptionItemValue.Name))
                    {
                        KeyValueEntity ItemValue = ItemValues.Find(r => r.Key == OptionItemValue.Name);
                        if (ItemValue != null && !String.IsNullOrEmpty(ItemValue.Key))
                        {
                            OptionItemValue.DefaultValue = ItemValue.Value;
                        }
                    }
                }
            }
            ResponseString = OptionList.ToJson();

        }




    }
}