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
    public partial class Skin_MegaMenu_Old : BaseNavObjectBase
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

                //LoadScript("jquery.hoverIntent.min.js");

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

            sb.AppendFormat("<ul class=\"dnngo_mega\">");
            
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

                    String Str1 = BindTwoMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码

                    //构造三种样式
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "current" : "";
                    String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                    //string itemIndex = this.GetItemIndex();
                    itemIndex++;

                    sb.AppendFormat("<li class=\"{0} dgn-root {1} {2}  Item-{3}\">", dirClass, ActiveClass, CircularBeadStyle, itemIndex);
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
        public string BindTwoMenu(List<TabInfo> tabs, int ParentId, List<TabInfo> ActiveTabs)
        {
            int firstTabId = 0;
            int lastTabId = 0;
            this.GetFirstLastTabId(tabs, ParentId, ref firstTabId, ref lastTabId);
            StringBuilder str = new StringBuilder();
           
       
            //检索出当前所有的菜单
            List<TabInfo> AllTabs = tabs.FindAll(r1=>r1.ParentId == ParentId);
            bool flag = AllTabs != null && AllTabs.Count > 0;
            int itemIndex = 0;
            if (flag)
            {
              
                int ColumnCount = AllTabs.Count / MegaMenu_Column;
                int RemainderCount = AllTabs.Count % MegaMenu_Column;
                int xColumn = ColumnCount + (RemainderCount > 0 ? 1 : 0);

                if (xColumn > 0)
                {
                    int Css_Column = 0;

                    StringBuilder RowStr = new StringBuilder();
                    for (int i = 0; i < MegaMenu_Column; i++)
                    {
                        //xColumn = ColumnCount + ( i < RemainderCount ? 1 : 0);

                        List<TabInfo> tempTabs = Common.Split<TabInfo>(AllTabs, i+1, xColumn);

                        if (tempTabs != null && tempTabs.Count >0)
                        {
                            
                            RowStr.AppendFormat("<li><ul>");
                            for (int j = 0; j < tempTabs.Count; j++)
                            {
                                TabInfo tab = tempTabs[j];

                                String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码
                                //构造三种样式
                                String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                                String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "subcurrent" : "";
                                String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                                itemIndex++;

                                RowStr.AppendFormat("<li class=\"{0} {1} {2} SunItem-{3}\">", dirClass, ActiveClass, CircularBeadStyle, itemIndex);
                                RowStr.Append(this.FormatAlink(tab, tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                                RowStr.Append(Str1);
                                RowStr.Append("</li>");

                            }
                            Css_Column++;
                            RowStr.AppendFormat("</ul></li>");
                          
                        }
              
               
                        if (tempTabs.Count >= AllTabs.Count) break;

                    }
                    str.AppendFormat("<div class=\"mega_submenu Column{0}\" style=\"display: none;\"><ul class=\"mega_submenu_ul\">", Css_Column);
                    str.Append(RowStr);
                    str.Append("</ul></div>");
                }
                
            }

            //for (int j = 0; j < 3; j++)
            //{
            //    StringBuilder RowStr = new StringBuilder();
            //    RowStr.AppendFormat("<li><ul>");

            //    for (int i = 0; i < tabs.Count; i++)
            //    {
            //        if (tabs[i].ParentId == ParentId)
            //        {
            //            TabInfo tab = tabs[i];

            //            String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码
            //            //构造三种样式
            //            String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
            //            String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "subcurrent" : "";
            //            String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);

            //            str.AppendFormat("<li class=\"{0} {1} {2}\">", dirClass, ActiveClass, CircularBeadStyle);
            //            str.Append(this.FormatAlink(tab, tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
            //            str.Append(Str1);
            //            str.Append("</li>");

            //            flag = true;
            //        }
            //    }

            //    RowStr.AppendFormat("<ul></li>");
            //    str.Append(RowStr);
            //}
              
          
            if (!flag) return "";
            return str.ToString();
        }


        /// <summary>
        /// 绑定子菜单
        /// </summary>
        /// <param name="tabs">菜单集合</param>
        /// <param name="ParentId">上级菜单编号</param>
        /// <param name="IsChildActive">子菜单是否选中</param>
        /// <returns></returns>
        public string BindChildMenu(List<TabInfo> tabs, int ParentId, List<TabInfo> ActiveTabs)
        {
            int firstTabId = 0;
            int lastTabId = 0;
            this.GetFirstLastTabId(tabs, ParentId, ref firstTabId, ref lastTabId);
             StringBuilder str = new StringBuilder();
            str.Append( "<ul>");
            bool flag = false;
            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].ParentId == ParentId)
                {
                    TabInfo tab = tabs[i];
 
                    String Str1 =BindChildMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码

                    //构造三种样式
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "subcurrent" : "";
                    String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);

                    str.AppendFormat("<li class=\"{0} {1} {2}\">", dirClass, ActiveClass, CircularBeadStyle);
                    str.Append(this.FormatAlink(tab, tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                    str.Append(Str1);
                    str.Append("</li>");

               
                    //string str3 = tab.TabID == base.PortalSettings.ActiveTab.TabID ? string.Format("class=\"{0}{1}\"", "subcurrent", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId)) : string.Format("class=\"{0}\"", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId));
                    //string str4 = (str2 != "") ? string.Format("class=\"{0}{1}{2}\"", "dir", this.GetActiveTabStyle(tab, IsActiveTab(ActiveTabs,PortalSettings.ActiveTab), "subcurrent"), this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId)) : str3;
                    ////string str5 = (str2 != "") ? string.Format("<div class=\"menuslide\">{0}</div>", str2) : str2;
                    //str = str + string.Format("<li {0}>{1}{2}</li>", (str4 != "") ? str4 : string.Format("class=\"{0}{1}\"", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId)), this.FormatAlink(tab), str2);
                    flag = true;
                }
            }
            str.Append("</ul>");
            if (!flag) return "";
            return str.ToString();
        }

    }
}