using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 保存或者新建行数据
    /// </summary>
    public class ServiceDeleteRowJSON : iService
    {
        public ServiceDeleteRowJSON()
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
                return "Save Row JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            //Dictionary<String, Object> jsonPaneFields = new Dictionary<string, Object>();

            Int32 RowID = WebHelper.GetIntParam(Context.Request, "RowID", 0);
            Int32 DeleteRow = 0;
            Boolean Success = false;

            DNNGo_ThemePlugin_MenuRowItem MenuRow = DNNGo_ThemePlugin_MenuRowItem.FindByKeyForEdit(RowID);
            if (MenuRow != null && MenuRow.ID > 0)
            {
                if (MenuRow.Delete() > 0)
                {
                    DeleteRow = 1;
                    Success = true;


                }

            }
            //jsonPaneFields.Add("DeleteRow", DeleteRow);
            //jsonPaneFields.Add("Success", Success);

            //转换数据为json
            //ResponseString = jsSerializer.Serialize(jsonPaneFields);
            ResponseString = new { DeleteRow = DeleteRow,  Success = Success }.ToJson();
        }

         




    }
}