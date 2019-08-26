using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_EditMegamenu : BaseModule
    {
        /// <summary>
        /// 编辑页面的编号
        /// </summary>
        public Int32 EditTabID = WebHelper.GetIntParam(HttpContext.Current.Request, "EditTabID", 0);


        private TabInfo _TabItem = new TabInfo();
        /// <summary>
        /// 页面项
        /// </summary>
        public TabInfo TabItem
        {
            get {
                if (!(_TabItem != null && _TabItem.TabID > 0))
                {
                    if (int.MaxValue == EditTabID)
                    {
                        _TabItem = InitGlobalTab();
                    }
                    else
                    {
                        _TabItem = objTabs.GetTab(EditTabID);
                    }
                }
                return _TabItem; }
        }



        private DNNGo_ThemePlugin_Menu _MenuItem = new DNNGo_ThemePlugin_Menu();
        /// <summary>
        /// 菜单项
        /// </summary>
        public DNNGo_ThemePlugin_Menu MenuItem
        {
            get
            {
                if (!(_MenuItem != null && _MenuItem.ID > 0))
                {
                    _MenuItem = DNNGo_ThemePlugin_Menu.Find(DNNGo_ThemePlugin_Menu._.TabID, EditTabID);
                    if (!(_MenuItem != null && _MenuItem.ID > 0))
                    {
                        _MenuItem = new DNNGo_ThemePlugin_Menu();
                    }

                }
                return _MenuItem;
            }
        }






        /// <summary>
        /// 当前标签
        /// </summary>
        public  String Token
        {
            get { return WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "Background"); }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {

     
                txtMegaMenuWidth.Text = MenuItem.MenuWidth.ToString();


                WebHelper.BindList(ddlMegaPosition, typeof(EnumMegaPosition));

                WebHelper.SelectedListByValue(ddlMegaPosition, GetSettingT<Int32>(MenuItem, "MegaPosition", (Int32)EnumMegaPosition.Left));
                


                WebHelper.BindList(ddlMenuType, typeof(EnumTabType));

                WebHelper.SelectedListByValue(ddlMenuType, MenuItem.MenuType);

                // liHeader_Title.Text = ViewResourceText(String.Format("Header_Title_{0}", Token), String.Format("{0} Page Settings", Token));
            }

             
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                DNNGo_ThemePlugin_Menu menu = MenuItem;

                menu.MenuWidth = WebHelper.GetIntParam(Request, txtMegaMenuWidth.UniqueID, 0);
                menu.MenuType = WebHelper.GetIntParam(Request, ddlMenuType.UniqueID, 0);

                List<KeyValueEntity> Options = menu.SettingItems;

                Options = UpdateSettings(Options, "MegaPosition",WebHelper.GetStringParam(Request, ddlMegaPosition.UniqueID, "0"));

                menu.Options = Options.ToJson();

                menu.LastIP = WebHelper.UserHost;
                menu.LastTime = DateTime.Now;
                menu.LastUser = UserId;

                if (menu.ID > 0)
                {
                    menu.Update();
                }
                else
                {
                    menu.PortalId = PortalId;
                    menu.TabID = EditTabID;
                    menu.ModuleId = ModuleId;
                    menu.Insert();
                }




                mTips.LoadMessage("SaveOptionsSuccess", EnumTips.Success, this, new String[] { });

                Response.Redirect(WebHelper.GetScriptUrl, false);
                //String GoUrl = String.Format("{0}?PortalId={1}&TabID={2}&Token={3}&SkinFileName={4}#tabs-box-{3}", "Resource_Options.aspx", PortalId, TabId, "Options", "");

                //Response.Redirect(GoUrl,false);

            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }

 


        /// <summary>
        /// 取消
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }



        /// <summary>
        /// 服务路径
        /// </summary>
        /// <param name="TokenName"></param>
        /// <returns></returns>
        public String ServiceUrl(String TokenName)
        {
            return String.Format("{0}Resource_Service.aspx?Token={1}&PortalId={2}&ModuleId={3}&TabId={4}&EditTabID={5}",ModulePath, TokenName,PortalId,ModuleId,TabId, EditTabID);
        }


    }
}