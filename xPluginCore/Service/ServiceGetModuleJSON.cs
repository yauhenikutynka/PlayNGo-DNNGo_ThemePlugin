using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    public class ServiceGetModuleJSON : iService
    {
        public ServiceGetModuleJSON()
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
                return "Get Modules JSON";
            }
        }

        


        public void Execute(BasePage Context)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> jsonModules = new Dictionary<string, Object>();

            Int32 ByTabID = WebHelper.GetIntParam(Context.Request, "ByTabID", 0);

            if(ByTabID > 0)
            {
                ModuleController objModules = new ModuleController();
                foreach (KeyValuePair<int, ModuleInfo> pair in objModules.GetTabModules(ByTabID))
                {
                    ModuleInfo objModule = pair.Value;
                    if (objModule.IsDeleted == false)
                    {
                        bool propertyNotFound = false;
                        string ModuleName = objModule.GetProperty("ModuleName", "", System.Globalization.CultureInfo.CurrentCulture, null, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);

                 

                        jsonModules.Add(objModule.ModuleID.ToString(), new { ModuleID = objModule.ModuleID, ModuleName = ModuleName, ModuleTitle = objModule.ModuleTitle });
                    }
                }
            }
 
 
            //转换数据为json
            ResponseString = jsSerializer.Serialize(jsonModules);
        }




    }
}