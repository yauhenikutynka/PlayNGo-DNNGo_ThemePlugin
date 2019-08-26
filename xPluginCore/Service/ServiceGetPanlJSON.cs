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
    public class ServiceGetPanlJSON : iService
    {
        public ServiceGetPanlJSON()
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
                return "Get Panl JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> jsonPanes = new Dictionary<string, Object>();

            Int32 EditTabID = WebHelper.GetIntParam(Context.Request, "EditTabID", 0);


            Int32 Record = 0;
            QueryParam qp = new QueryParam();
            qp.Orderfld = DNNGo_ThemePlugin_MenuPane._.Sort;
            qp.OrderType = 0;

            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_MenuPane._.PortalId, Context.PortalId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_MenuPane._.TabID, EditTabID, SearchType.Equal));

            List<DNNGo_ThemePlugin_MenuPane> MenuPanes = DNNGo_ThemePlugin_MenuPane.FindAll(qp,out Record);

            foreach (var MenuPane in MenuPanes)
            {
                if (MenuPane != null && MenuPane.ID > 0)
                {
                    //Dictionary<String, Object> jsonPane = new Dictionary<string, Object>();

                    //foreach (var Field in DNNGo_ThemePlugin_MenuPane.Meta.Fields)
                    //{
                    //    jsonPane.Add(Field.ColumnName, MenuPane[Field.ColumnName]);
                    //}
                    //筛选出列记录的选项集合
                    if (!String.IsNullOrEmpty(MenuPane.Options))
                    {
                        //List<KeyValueEntity> Options = MenuPane.SettingItems;

                    }


                    //查找当前容器下所有的行数据
                    QueryParam qpRows = new QueryParam();
                    qpRows.Where.Add(new SearchParam(DNNGo_ThemePlugin_MenuRowItem._.PortalId, Context.PortalId, SearchType.Equal));
                    qpRows.Where.Add(new SearchParam(DNNGo_ThemePlugin_MenuRowItem._.PaneID, MenuPane.ID, SearchType.Equal));
                    qpRows.Orderfld = DNNGo_ThemePlugin_MenuRowItem._.Sort;
                    qpRows.OrderType = 0;

                    List<DNNGo_ThemePlugin_MenuRowItem> MenuRows = DNNGo_ThemePlugin_MenuRowItem.FindAll(qpRows, out Record);

                    MenuPane.Rows = MenuRows;


                    //Dictionary<String, Object> jsonRows = new Dictionary<string, Object>();
                    //foreach (var MenuRow in MenuRows)
                    //{
                    //    if (MenuRow != null && MenuRow.ID > 0)
                    //    {
                    //        Dictionary<String, Object> jsonRow = new Dictionary<string, Object>();

                    //        foreach (var RowField in DNNGo_ThemePlugin_MenuRowItem.Meta.Fields)
                    //        {
                    //            jsonRow.Add(RowField.ColumnName, MenuRow[RowField.ColumnName]);
                    //        }
                    //        //筛选出行记录的选项集合
                    //        if (!String.IsNullOrEmpty(MenuRow.Options))
                    //        { 

                    //        }


                    //        jsonRows.Add( MenuRow.ID.ToString(), jsonRow);
                    //    }
                    //}

                    //jsonPane.Add("Rows", jsonRows);


                    //jsonPanes.Add( MenuPane.ID.ToString(), jsonPane);

                }




            }
            //转换数据为json
            ResponseString = MenuPanes.ToJson();
        }
         



    }
}