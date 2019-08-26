using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 相片列表的数据
    /// </summary>
    public class ServiceDeletePanlJSON : iService
    {
        public ServiceDeletePanlJSON()
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
                return "Save Panl JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            Int32 DeleteRow = 0;
            Int32 DeletePane = 0;
            Boolean Success = false;

            Int32 PaneID = WebHelper.GetIntParam(Context.Request, "PaneID", 0);
            if (PaneID > 0)
            {
                DNNGo_ThemePlugin_MenuPane MenuPane = DNNGo_ThemePlugin_MenuPane.FindByKeyForEdit(PaneID);
                if (MenuPane != null && MenuPane.ID > 0)
                {
                    
                    List<DNNGo_ThemePlugin_MenuRowItem> RowItems = DNNGo_ThemePlugin_MenuRowItem.FindAll(DNNGo_ThemePlugin_MenuRowItem._.PaneID, PaneID);
                    foreach (var RowItem in RowItems)
                    {

                        if (RowItem.Delete() > 0)
                        {
                            DeleteRow++;
                        }

                    }
                 

                    if (MenuPane.Delete() > 0)
                    {
                        DeleteRow = 1;
                        Success = true;
                    }

                }
                else
                {
                    Success = false;
                }


            } else
            {
                Success = false;
            }

            //转换数据为json
            ResponseString = new  { DeletePane = DeletePane, DeleteRow= DeleteRow, Success= Success }.ToJson();
        }

         




    }
}