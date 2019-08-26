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
    public partial class Skin_MultiMenu : BaseNavObjectBase
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
                if (!String.IsNullOrEmpty(MultiMenuAction) && MultiMenuAction.IndexOf("hover", StringComparison.CurrentCultureIgnoreCase)>=0)
                {
                    //LoadScript("accordion-hover.js");
                }
                else
                {
                    //LoadScript("accordion.js");
                }
   
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
 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<ul  id=\"gomenu{0}\" class=\"dropdown {1}\">", ClientID, CssClass);

            //这里需要有个方法通过级别显示菜单的
            int TopTabID = GetTopTab(tabs, PortalSettings.ActiveTab);

            //构造好当前菜单的活动菜单
            List<TabInfo> ActiveTabs = GetActiveTabs(tabs, PortalSettings.ActiveTab);
            bool flag = false;
            int itemIndex = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                TabInfo tab = tabs[i];
                if (tab.ParentId == TopTabID && TopTabID != 0)
                {

                    String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs,2);//输出子菜单代码
                 
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式

                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "current" : "";
                  
                    itemIndex++;
                    sb.AppendFormat("<li class=\"{0} Item-{1} {2}\">", ActiveClass, itemIndex, dirClass);

                    String aLinkClass = String.Format("class=\"menuitem {0}\"", tab.TabID == PortalSettings.ActiveTab.TabID ? "active" : "");
                    sb.Append(this.FormatAlinkByMulti(tab, aLinkClass, true, !String.IsNullOrEmpty(Str1), !String.IsNullOrEmpty(ActiveClass)));

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
                str.Append("<ul>");
                bool flag = false;
                int itemIndex = 0;
                for (int i = 0; i < tabs.Count; i++)
                {

                    if (tabs[i].ParentId == ParentId)
                    {
                        TabInfo tab = tabs[i];

                        String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs, ChildLevel + 1);//输出子菜单代码
                        String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "current" : "";
                        String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                        itemIndex++;

                        str.AppendFormat("<li class=\"{0} SunItem-{1} {2}\">", ActiveClass, itemIndex, dirClass);
                        str.Append(this.FormatAlinkByMulti(tab, "", true, !String.IsNullOrEmpty(Str1), !String.IsNullOrEmpty(ActiveClass)));
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


        /// <summary>
        /// 格式化链接
        /// </summary>
        /// <param name="tab">菜单</param>
        /// <param name="CssClass">样式</param>
        /// <returns></returns>
        public string FormatAlinkByMulti(TabInfo tab, String CssClass, Boolean EnableLink = true, Boolean EnableSub = false, Boolean Active = false)
        {

            String LocalizedTabName = FormatTabName(tab);

            //显示标题
            String PageTitle = String.Empty;
            if (!String.IsNullOrEmpty(tab.Title) && ShowTitle)
            {
                PageTitle = String.Format(" <b>{0}</b>", tab.Title);
            }

            string tabName = string.Format("<span>{0}{1}{2}</span>", FormatIconFile(tab), LocalizedTabName, PageTitle);

             string url = FormatTabUrl(tab);
            //是否可以跳转到URL
            if (tab.DisableLink) url = "javascript:;";
            //格式化标签的打开方式
            String TabTarget = FormatTabTarget(tab);

            String SunSpan = EnableSub ? String.Format("<span class='menu_arrow arrow_{0}'>{1}</span>", Active ? "closed" : "opened", Active ? "-" : "+") : "";

            String TabTooltip = ShowTooltip ? String.Format(" title=\"{0}\" ", String.IsNullOrEmpty(tab.Title) ? LocalizedTabName : tab.Title) : "";


            return string.Format("<a href=\"{0}\" {1} {2} {3}>{5}{4}</a>", EnableLink ? url : "javascript:;", CssClass, TabTooltip, TabTarget, tabName, SunSpan);
           
        }
    }
}