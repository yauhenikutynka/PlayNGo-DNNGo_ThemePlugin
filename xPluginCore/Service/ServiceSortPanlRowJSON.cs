using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 读取容器列表的数据
    /// </summary>
    public class ServiceSortPanlRowJSON : iService
    {
        public ServiceSortPanlRowJSON()
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
                return "Sort Panl&Row JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> jsonPanes = new Dictionary<string, Object>();

            Int32 EditTabID = WebHelper.GetIntParam(Context.Request, "EditTabID", 0);
            Int32 PaneID = WebHelper.GetIntParam(Context.Request, "PaneID", 0);

            Int32 SortCount = 0;
            Boolean Success = false;

            String SortJson = HttpUtility.UrlDecode( WebHelper.GetStringParam(Context.Request, "SortJson", "[]"));
            if (!String.IsNullOrEmpty(SortJson))
            {
 
                List<DNNGo_ThemePlugin_MenuPane> SortDicts = SortJson.ToObject<List<DNNGo_ThemePlugin_MenuPane>>();
                if (SortDicts != null && SortDicts.Count > 0)
                {
                    foreach(var SortDict in SortDicts)
                    {
                        if (PaneID > 0)
                        {
                            //设置的是行记录的排序
 
                            if (DNNGo_ThemePlugin_MenuRowItem.Update(new string[] { "Sort", "PaneID" }, new object[] { SortDict.Sort, PaneID }, new string[] { "ID" }, new object[] { SortDict.ID }) > 0)
                            {
                                SortCount++;

                            }

                        }
                        else
                        {
                            //设置的是列的排序
                            if (DNNGo_ThemePlugin_MenuPane.Update(String.Format("{0}={1}", DNNGo_ThemePlugin_MenuPane._.Sort, SortDict.Sort), String.Format("{0}={1}", DNNGo_ThemePlugin_MenuPane._.ID, SortDict.ID)) > 0)
                            {

                                SortCount++;
                            }
                        }
                        if (SortCount > 0)
                        {
                            Success = true;
                        }

                    }


                  
                }


                

            }

            jsonPanes.Add("SortCount", SortCount);
            jsonPanes.Add("Success", Success);


            //转换数据为json
            ResponseString = jsSerializer.Serialize(jsonPanes);
        }
         



    }
}