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
    public class ServiceSavePanlJSON : iService
    {
        public ServiceSavePanlJSON()
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
            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            //Dictionary<String, Object> jsonPaneFields = new Dictionary<string, Object>();
            Int32 PaneID =  WebHelper.GetIntParam(Context.Request, "PaneID", 0);

            DNNGo_ThemePlugin_MenuPane MenuPane = DNNGo_ThemePlugin_MenuPane.FindByKeyForEdit(PaneID);
            MenuPane.PaneName = WebHelper.GetStringParam(Context.Request, "PaneName", MenuPane.PaneName);
            MenuPane.TagPane = WebHelper.GetStringParam(Context.Request, "TagPane", MenuPane.TagPane);
            MenuPane.PaneWidth = WebHelper.GetStringParam(Context.Request, "PaneWidth", MenuPane.PaneWidth);
            MenuPane.Sort = WebHelper.GetIntParam(Context.Request, "Sort", MenuPane.Sort);
            MenuPane.TabID = WebHelper.GetIntParam(Context.Request, "EditTabID", MenuPane.TabID);

            //MenuPane.Options = WebHelper.GetStringParam(Context.Request, "Options", MenuPane.Options);

            List<KeyValueEntity> Options = MenuPane.SettingItems;

            Options = Context.UpdateSettings(Options, "PaneTopSpacing", WebHelper.GetStringParam(Context.Request, "PaneTopSpacing", "0"));
            Options = Context.UpdateSettings(Options, "PaneRightSpacing", WebHelper.GetStringParam(Context.Request, "PaneRightSpacing", "0"));
            Options = Context.UpdateSettings(Options, "PaneBottomSpacing", WebHelper.GetStringParam(Context.Request, "PaneBottomSpacing", "0"));
            Options = Context.UpdateSettings(Options, "PaneLeftSpacing", WebHelper.GetStringParam(Context.Request, "PaneLeftSpacing", "0"));
            Options = Context.UpdateSettings(Options, "PaneTopLine", WebHelper.GetStringParam(Context.Request, "PaneTopLine", "false"));
            Options = Context.UpdateSettings(Options, "PaneRightLine", WebHelper.GetStringParam(Context.Request, "PaneRightLine", "false"));
            Options = Context.UpdateSettings(Options, "PaneBottomLine", WebHelper.GetStringParam(Context.Request, "PaneBottomLine", "false"));
            Options = Context.UpdateSettings(Options, "PaneLeftLine", WebHelper.GetStringParam(Context.Request, "PaneLeftLine", "false"));

            MenuPane.Options = Options.ToJson();


            if (MenuPane.ID > 0)
            {
      
                MenuPane = UpdatePane(Context, MenuPane);
            }
            else
            {
                MenuPane = CreateNewPane(Context, MenuPane);
            }


            //foreach (var Field in DNNGo_ThemePlugin_MenuPane.Meta.Fields)
            //{
            //    jsonPaneFields.Add(Field.ColumnName, MenuPane[Field.ColumnName]);
            //}

            //转换数据为json
            //ResponseString = jsSerializer.Serialize(jsonPaneFields);

            ResponseString = MenuPane.ToJson();
        }


        public DNNGo_ThemePlugin_MenuPane UpdatePane(BasePage Context, DNNGo_ThemePlugin_MenuPane MenuPane)
        {
 

 


            MenuPane.LastIP = WebHelper.UserHost;
            MenuPane.LastTime = DateTime.Now;
            MenuPane.LastUser = Context.UserId;

            MenuPane.Update();


            return MenuPane;
        }



        /// <summary>
        /// 创建新的容器
        /// </summary>
        /// <returns></returns>
        public DNNGo_ThemePlugin_MenuPane CreateNewPane(BasePage Context, DNNGo_ThemePlugin_MenuPane MenuPane)
        {
 
            MenuPane.PortalId = Context.PortalId;
            MenuPane.ModuleId = Context.ModuleId;
          


            MenuPane.LastIP = WebHelper.UserHost;
            MenuPane.LastTime = DateTime.Now;
            MenuPane.LastUser = Context.UserId;

            MenuPane.ID = MenuPane.Insert();
            return MenuPane;
        }




    }
}