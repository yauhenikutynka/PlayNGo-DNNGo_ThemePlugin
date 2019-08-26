using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_Mobile : BaseNavObjectBase
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

                LoadStyle("jquery.mmenu.css");
                //LoadScript("jquery.mmenu.min.js");
                

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
 
        }

        protected void OnViewMenuBind()
        {
            List<TabInfo> userTabs = GetUserTabs(base.PortalSettings, UserController.GetCurrentUserInfo());
            this.LiContent.Text = this.ViewMenuBind(userTabs);
        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="tabs"></param>
        /// <returns></returns>
        public string ViewMenuBind(List<TabInfo> tabs)
        {
            int firstTabId = 0;
            int lastTabId = 0;
            this.GetFirstLastTabId(tabs, -1, ref firstTabId, ref lastTabId);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat("<ul  class=\"{0}\">", "menu_list");
            
            //这里需要有个方法通过级别显示菜单的
            int TopTabID = GetTopTab(tabs, PortalSettings.ActiveTab);

            //构造好当前菜单的活动菜单
            List<TabInfo> ActiveTabs = GetActiveTabs(tabs, PortalSettings.ActiveTab);
            bool flag = false;
            int itemIndex = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                TabInfo tab = tabs[i];
                if (tab.ParentId == TopTabID && TopTabID!= 0)
                {

                    String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs,2);//输出子菜单代码

                    //构造三种样式
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? " current" : "";
                    String SelectClass = tab.TabID == PortalSettings.ActiveTab.TabID ? " mm-selected" : "";
                    String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                    //string itemIndex = this.GetItemIndex();
                    itemIndex++;

                    sb.AppendFormat("<li class=\"{0} {1} {2} Item-{3} {4}\">", dirClass, ActiveClass, CircularBeadStyle, itemIndex, SelectClass);
                    sb.Append(this.FormatAlink(tab ,tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                    sb.Append(Str1);
                    sb.Append("</li>");

                    flag = true;
                }
            }
            sb.Append("</ul>");
            if (!flag) return "";
            return sb.ToString();
        }


        /// <summary>
        /// 绑定子菜单
        /// </summary>
        /// <param name="tabs">菜单集合</param>
        /// <param name="ParentId">上级菜单编号</param>
        /// <param name="IsChildActive">子菜单是否选中</param>
        /// <returns></returns>
        public string BindChildMenu(List<TabInfo> tabs, int ParentId, List<TabInfo> ActiveTabs, Int32 ChildLevel)
        {
          
             StringBuilder str = new StringBuilder();
             if (ChildLevel <= MaxLevel || MaxLevel <= 0)
             {
                 int firstTabId = 0;
                 int lastTabId = 0;
                 this.GetFirstLastTabId(tabs, ParentId, ref firstTabId, ref lastTabId);

                 str.Append("<ul>");
                 bool flag = false;
                 int itemIndex = 0;
                 for (int i = 0; i < tabs.Count; i++)
                 {
                     if (tabs[i].ParentId == ParentId)
                     {
                         TabInfo tab = tabs[i];

                         String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs, ChildLevel+1);//输出子菜单代码

                         //构造三种样式
                         String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                         String ActiveClass = IsActiveTab(ActiveTabs, tab) ? " subcurrent" : "";
                         String SelectClass = tab.TabID == PortalSettings.ActiveTab.TabID ? " mm-selected" : "";
                         String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                         itemIndex++;

                         str.AppendFormat("<li class=\"{0} {1} {2} SunItem-{3} {4}\">", dirClass, ActiveClass, CircularBeadStyle, itemIndex, SelectClass);
                         str.Append(this.FormatAlink(tab, tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                         str.Append(Str1);
                         str.Append("</li>");

                         flag = true;
                     }
                 }
                 str.Append("</ul>");
                 if (!flag) return "";
             }

            return str.ToString();
        }

    }
}