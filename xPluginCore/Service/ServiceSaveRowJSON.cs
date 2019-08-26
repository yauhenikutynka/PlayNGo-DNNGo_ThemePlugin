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
    public class ServiceSaveRowJSON : iService
    {
        public ServiceSaveRowJSON()
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

            DNNGo_ThemePlugin_MenuRowItem MenuRow = new DNNGo_ThemePlugin_MenuRowItem();

            MenuRow.ID = WebHelper.GetIntParam(Context.Request, "RowID", 0);
            if (MenuRow.ID > 0)
            {
                MenuRow = DNNGo_ThemePlugin_MenuRowItem.FindByKeyForEdit(MenuRow.ID);
                MenuRow = UpdatePane(Context, MenuRow);
            }
            else
            {
                MenuRow = CreateNewPane(Context);
            }


            //foreach (var Field in DNNGo_ThemePlugin_MenuRowItem.Meta.Fields)
            //{
            //    jsonPaneFields.Add(Field.ColumnName, MenuRow[Field.ColumnName]);
            //}

            //转换数据为json
            //ResponseString = jsSerializer.Serialize(jsonPaneFields);
            ResponseString = MenuRow.ToJson();
        }


        public DNNGo_ThemePlugin_MenuRowItem UpdatePane(BasePage Context, DNNGo_ThemePlugin_MenuRowItem RowItem)
        {

            RowItem.TagList = WebHelper.GetStringParam(Context.Request, "TagList", RowItem.TagList);
            RowItem.Title = WebHelper.GetStringParam(Context.Request, "Title", RowItem.Title);
            //RowItem.RowType = WebHelper.GetIntParam(Context.Request, "RowType", 0);
            //RowItem.PaneID = WebHelper.GetIntParam(Context.Request, "PaneID", 0);
            RowItem.Sort = WebHelper.GetIntParam(Context.Request, "Sort", RowItem.Sort);
            //RowItem.TabID = WebHelper.GetIntParam(Context.Request, "EditTabID", 0);


            //绑定菜单
            RowItem.MenuLevel = WebHelper.GetIntParam(Context.Request, "MenuLevel", RowItem.MenuLevel);
            RowItem.MenuSytle = WebHelper.GetStringParam(Context.Request, "MenuSytle", RowItem.MenuSytle);
            RowItem.MenuDisplayTitle = WebHelper.GetIntParam(Context.Request, "MenuDisplayTitle", RowItem.MenuDisplayTitle);
            RowItem.MenuID = WebHelper.GetIntParam(Context.Request, "MenuID", RowItem.MenuID);
            //绑定HTML
            RowItem.HTML_Title = WebHelper.GetStringParam(Context.Request, "HTML_Title", RowItem.HTML_Title);
            RowItem.HTML_Content = WebHelper.GetStringParam(Context.Request, "HTML_Content", RowItem.HTML_Content);
            //绑定模块
            RowItem.BindModuleID = WebHelper.GetIntParam(Context.Request, "BindModuleID", RowItem.BindModuleID);
            RowItem.BindTabID = WebHelper.GetIntParam(Context.Request, "BindTabID", RowItem.BindTabID);




  
            RowItem.LastIP = WebHelper.UserHost;
            RowItem.LastTime = DateTime.Now;
            RowItem.LastUser = Context.UserId;

            RowItem.Update();

            return RowItem;
        }



        /// <summary>
        /// 创建新的容器
        /// </summary>
        /// <returns></returns>
        public DNNGo_ThemePlugin_MenuRowItem CreateNewPane(BasePage Context)
        {
            DNNGo_ThemePlugin_MenuRowItem RowItem = new DNNGo_ThemePlugin_MenuRowItem();
            RowItem.TagList = WebHelper.GetStringParam(Context.Request, "TagList", "");
            RowItem.Title = WebHelper.GetStringParam(Context.Request, "Title", "");
            RowItem.RowType = WebHelper.GetIntParam(Context.Request, "RowType", 0);
            RowItem.PaneID = WebHelper.GetIntParam(Context.Request, "PaneID", 0);

            RowItem.Sort = WebHelper.GetIntParam(Context.Request, "Sort",999);
            RowItem.TabID = WebHelper.GetIntParam(Context.Request, "EditTabID", 0);


            RowItem.PortalId = Context.PortalId;
            RowItem.ModuleId = Context.ModuleId;

           

            RowItem.LastIP = WebHelper.UserHost;
            RowItem.LastTime = DateTime.Now;
            RowItem.LastUser = Context.UserId;

            RowItem.ID = RowItem.Insert();
            return RowItem;
        }




    }
}