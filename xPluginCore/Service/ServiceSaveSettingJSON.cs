using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    public class ServiceSaveSettingJSON : iService
    {
        public ServiceSaveSettingJSON()
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
                return "Save Settings JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            List<SettingEntity> OptionList =  Context.OptionList;
            String SettingsJson = HttpUtility.UrlDecode(WebHelper.GetStringParam(Context.Request, "SettingsJson", "[]"));
            if (!String.IsNullOrEmpty(SettingsJson))
            {
                try
                {
                    List<KeyValueEntity> ItemValues = SettingsJson.ToList<KeyValueEntity>();
                    if (ItemValues != null && ItemValues.Count > 0)
                    {
                        //配置项存在的时候才可以保存值
                        if (System.IO.File.Exists(Context.ItemSettingXmlPath))
                        {
                            WriteTextToFile(Context.ItemValueXmlPath, ConvertTo.Serialize<List<KeyValueEntity>>(ItemValues));
                        }
                    }
                    else
                    {
                        ResponseString = new { Success = false }.ToJson();
                    }
                   

                }
                catch(Exception ex)
                {
                    ResponseString = new { Success = false, Message = ex.Message}.ToJson();
                }





            }
            else
            {
                ResponseString = new { Success = false }.ToJson();
            }
              

        }

        /// <summary>
        /// 写入文件到文件
        /// </summary>
        /// <param name="WriteFilePath">文件路径</param>
        /// <param name="ContentText">文件内容</param>
        public void WriteTextToFile(String WriteFilePath, String ContentText)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(WriteFilePath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(ContentText);
                sw.Flush();
                sw.Close();
            }
        }


    }
}