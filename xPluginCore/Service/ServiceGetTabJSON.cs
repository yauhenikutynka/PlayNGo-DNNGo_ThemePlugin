using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    public class ServiceGetTabJSON : iService
    {
        public ServiceGetTabJSON()
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
                return "Get Tabs JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> jsonPanes = new Dictionary<string, Object>();

           var Tabs = DotNetNuke.Entities.Tabs.TabController.GetPortalTabs(Context.PortalId, -1, true, true, false, true);
            if (Tabs != null && Tabs.Count > 0)
            {
                foreach (var tab in Tabs)
                {
                    if ((tab.IsDeleted == false))
                    {
                        jsonPanes.Add(tab.TabID.ToString(), new { TabID = tab.TabID, TabName = tab.LocalizedTabName, ParentId = tab.ParentId });
                    }
                       
                }
            }

 
            //转换数据为json
            ResponseString = jsSerializer.Serialize(jsonPanes);
        }




    }
}