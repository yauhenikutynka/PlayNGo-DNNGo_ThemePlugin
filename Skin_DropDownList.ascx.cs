using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_DropDownList : BaseNavObjectBase
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    //绑定数据项到前台
                    this.OnViewMenuBind();
                }
                LoadScript("dnngo-ThemePlugin.js");

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
 

        }

        protected void OnViewMenuBind()
        {
            List<TabInfo> userTabs = GetUserTabs(base.PortalSettings, UserController.GetCurrentUserInfo());
          
            List<KeyValueEntity> KeyValueList = new List<KeyValueEntity>();

            ViewMenuBind(userTabs, -1, 0, ref KeyValueList);

            ddlSelectMenu.DataSource = KeyValueList;
            ddlSelectMenu.DataTextField = "Key";
            ddlSelectMenu.DataValueField = "Value";
            ddlSelectMenu.DataBind();


            WebHelper.SelectedListByValue(ddlSelectMenu, FormatTabUrl(PortalSettings.ActiveTab));
        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="tabs"></param>
        /// <returns></returns>
        public void ViewMenuBind(List<TabInfo> tabs,Int32 ParentId,Int32 Level,ref List<KeyValueEntity> KeyValueList)
        {
            foreach (TabInfo tab in tabs)
            {
                if (tab.ParentId == ParentId)
                {
                    KeyValueEntity KeyValue = new KeyValueEntity();
                    KeyValue.Value =  tab.DisableLink ?"" :  FormatTabUrl(tab);
                    KeyValue.Key =  Common.FillVacancy( FormatTabName(tab),Level,"- ");
                    KeyValueList.Add(KeyValue);
                    ViewMenuBind(tabs, tab.TabID, Level + 1, ref KeyValueList);
                }
            }
             
        }
    }
}