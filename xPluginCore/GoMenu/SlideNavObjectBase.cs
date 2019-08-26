using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Common;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    public class SlideNavObjectBase : BaseNavObjectBase
    {




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
            sb.AppendFormat("<ul  class=\"{0}\">", "dropdown");

            //这里需要有个方法通过级别显示菜单的
            int TopTabID = GetTopTab(tabs, PortalSettings.ActiveTab);

            //构造好当前菜单的活动菜单
            List<TabInfo> ActiveTabs = GetActiveTabs(tabs, PortalSettings.ActiveTab);
            bool flag = false;
            Int32 itemIndex = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                TabInfo tab = tabs[i];

                if (tab.TabID > 980)
                {
                    int xxx = 0;
                }


                if (tab.ParentId == TopTabID && TopTabID != 0)
                {

                    String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码


                    //这里是一个定制的选项，需要加载一个小标题菜单的
                    if (TopMeunTitle && !String.IsNullOrEmpty(Str1))
                    {
                        Str1 = Str1.Remove(0, 4);//移除开头的UL
                        Str1 = String.Format("<ul><li class=\"TopMeunTitle\">{0}</li>", FormatAlink(tab)) + Str1;
                    }




                    //构造三种样式
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "current" : "";
                    String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                    //string itemIndex = this.GetItemIndex();
                    itemIndex++;

                    sb.AppendFormat("<li class=\"{0} {1} {2} Item-{3}\">", dirClass, ActiveClass, CircularBeadStyle, itemIndex);
                    sb.Append(this.FormatAlink(tab, tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                    if (!String.IsNullOrEmpty(Str1))
                        sb.AppendFormat("<div class=\"menuslide\">{0}</div>", Str1);
                    sb.Append("</li>");



                    //bool isChildActive = false;
                    //string itemIndex = this.GetItemIndex();
                    //string str3 = this.BindChildMenu(tabs, tab.TabID, ref isChildActive);
                    //string str4 = (base.PortalSettings.ActiveTab.TabID == tab.TabID) ? string.Format("class=\"{0}{1}{2}\"", "current", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId), itemIndex) : string.Format("class=\"{0}{1}\"", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId), itemIndex);
                    //string str5 = (str3 != "") ? string.Format("class=\"{0}{1}{2}{3}\"", new object[] { "dir", this.GetActiveTabStyle(tab, isChildActive), this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId), itemIndex }) : str4;
                    //string str6 = (str3 != "") ? string.Format("<div class=\"menuslide\">{0}</div>", str3) : str3;

                    //sb.AppendFormat("<li {0}>{1}{2}</li>", (str5 != "") ? str5 : string.Format("class=\"{0}{1}\"", this.GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId), itemIndex), this.FormatAlink(tab), str6);
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
        public string BindChildMenu(List<TabInfo> tabs, int ParentId, List<TabInfo> ActiveTabs)
        {
            int firstTabId = 0;
            int lastTabId = 0;
            this.GetFirstLastTabId(tabs, ParentId, ref firstTabId, ref lastTabId);
            StringBuilder str = new StringBuilder();
            str.Append("<ul>");
            bool flag = false;
            Int32 itemIndex = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                TabInfo tab = tabs[i];
                if (tab.ParentId == ParentId)
                {

                    String Str1 = BindChildMenu(tabs, tab.TabID, ActiveTabs);//输出子菜单代码

                    //构造三种样式
                    String dirClass = !String.IsNullOrEmpty(Str1) ? "dir" : "";//有下级目录的时候样式
                    String ActiveClass = IsActiveTab(ActiveTabs, tab) ? "subcurrent" : "";
                    String CircularBeadStyle = GetCircularBeadStyle(tab.TabID, firstTabId, lastTabId);
                    String FastSubcurrentClass = FastSubcurrent && String.IsNullOrEmpty(ActiveClass) && IsFastSubcurrent(tabs,ActiveTabs, tab, firstTabId) ? "subcurrent" : "";
                    itemIndex++;

                    str.AppendFormat("<li class=\"{0} {1} {2} {3} SunItem-{4}\">", dirClass, ActiveClass, CircularBeadStyle, FastSubcurrentClass, itemIndex);
                    str.Append(this.FormatAlink(tab ,tab.TabID == PortalSettings.ActiveTab.TabID ? "class=\"acurrent\"" : ""));
                    if (!String.IsNullOrEmpty(Str1))
                        str.AppendFormat("<div class=\"menuslide\">{0}</div>", Str1);
                    str.Append("</li>");

                 
                    flag = true;
                }
            }
            str.Append("</ul>");
            if (!flag) return "";
            return str.ToString();
        }


    }
}