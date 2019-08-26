using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.UI.Skins;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using System.Collections;
using System.Reflection;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 导航&皮肤对象基类
    /// </summary>
    public class BaseNavObject : BaseSkin
    {


        #region "参数属性"
 



         
 





        #endregion


        /// <summary>
        /// 用户信息
        /// </summary>
        private UserInfo Userinfo
        {
            get { return UserController.GetCurrentUserInfo(); }
        }


        /// <summary>
        /// 语言
        /// </summary>
        public String language
        {
            get
            {
                //return "en-US";

                return WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage); 
            }
        }


         


        #region "==菜单的处理与格式化=="

        /// <summary>
        /// 格式化链接
        /// </summary>
        /// <param name="tab">菜单</param>
        /// <returns></returns>
        public string FormatAlink(TabInfo tab)
        {
            return FormatAlink(tab, "");
        }


        /// <summary>
        /// 格式化链接
        /// </summary>
        /// <param name="tab">菜单</param>
        /// <param name="CssClass">样式</param>
        /// <returns></returns>
        public string FormatAlink(TabInfo tab, String CssClass, Boolean EnableLink = true)
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

            String TabTooltip = ShowTooltip ? String.Format(" title=\"{0}\" ", String.IsNullOrEmpty(tab.Title) ? LocalizedTabName : tab.Title) : "";

            return string.Format("<a href=\"{0}\" {1} {2} {3}>{4}</a>", EnableLink ? url : "javascript:;", CssClass, TabTooltip, TabTarget, tabName);

        }


        /// <summary>
        /// 格式化标签的打开方式
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public String FormatTabTarget(TabInfo tab)
        {
            //是否打开新页面
            Boolean LinkNewWindow = false;
            PropertyInfo pTabSettings = typeof(TabInfo).GetProperty("TabSettings");
            if (pTabSettings != null && pTabSettings.PropertyType == typeof(Hashtable))
            {
                Hashtable TabSettings = pTabSettings.GetValue(tab, null) as Hashtable;
                if (TabSettings["LinkNewWindow"] != null && !String.IsNullOrEmpty(Convert.ToString(TabSettings["LinkNewWindow"])))
                {
                    LinkNewWindow = Convert.ToBoolean(TabSettings["LinkNewWindow"]);
                }
            }
            return LinkNewWindow ? "target=\"_blank\"" : "";
        }



        /// <summary>
        /// 格式化页面链接
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public string FormatTabUrl(TabInfo tab)
        {
            string url = Globals.NavigateURL(tab.TabID);
            if (tab.TabType == TabType.Url)
                url = tab.Url;
            else if (tab.TabType == TabType.Tab)
            {
                if (tab.Url != string.Empty) url = tab.FullUrl;
            }
            else if (tab.TabType == TabType.File) url = tab.FullUrl;

            return url;
        }


        /// <summary>
        /// 读取菜单的标题(适合多语言的)
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public String FormatTabName(TabInfo tab)
        {
            Type TabTypes = tab.GetType();
            System.Reflection.PropertyInfo pi = TabTypes.GetProperty("LocalizedTabName");
            String LocalizedTabName = String.Empty;
            if (pi != null && !String.IsNullOrEmpty(pi.Name))
            {
                LocalizedTabName = Convert.ToString(pi.GetValue(tab, null));
            }
            else
            {
                LocalizedTabName = tab.TabName;
            }
            return LocalizedTabName;
        }

        /// <summary>
        /// 格式化图标
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public String FormatIconFile(TabInfo tab)
        {
            String IconFile = String.Empty;
            if (!String.IsNullOrEmpty(tab.IconFile))
            {
                IconFile = tab.IconFile;

                if (IconFile.IndexOf("~/", StringComparison.CurrentCultureIgnoreCase) < 0 && Common.ConvertVersion(PortalProperty("Version")) <= Common.ConvertVersion("5.4.4"))
                {
                    //if (tab.IsAdminTab)
                    //{
                    //    IconFile = String.Format("~/images/{0}", IconFile);
                    //}
                    //else
                    //{
                    //    IconFile = PortalSettings.HomeDirectory + IconFile;
                    //}

                    IconFile = PortalSettings.HomeDirectory + IconFile;
                }
                IconFile = String.Format("<img src=\"{0}\" class=\"menuicon\"  /> ", ResolveUrl(IconFile));
            }
            String IconSettingName = String.Format("ThemePlugin_TabIcon_{0}", tab.TabID);//Portal_Settings
            if (Portal_Settings != null && Portal_Settings.ContainsKey(IconSettingName))
            {
                String IconCss = Convert.ToString(Portal_Settings[IconSettingName]);
                if (!String.IsNullOrEmpty(IconCss))
                {
                    IconFile += String.Format("<i class=\"{0}\"></i>", IconCss);
                }
            }
            return IconFile;
        }


        #endregion


        #region "当前皮肤对象目录地址"


        /// <summary>
        /// 当前皮肤对象目录地址
        /// </summary>
        public string BasePath
        {
            get { return this.TemplateSourceDirectory + "/"; }
        }

        #endregion





        #region "显示mega存储的参数"

        public T ViewSetting<T>(String Name, T DefaultValue,List<KeyValueEntity> TabValues, List<SettingEntity> XmlSettings)
        {
            //取出所需要的值
            T o = DefaultValue;
            if (TabValues.Exists(r => r.Key == Name))
            {
                o = (T)ConvertTo.FormatValue(TabValues.Find(r => r.Key == Name).Value.ToString(), DefaultValue.GetType());
            }
            else if (XmlSettings.Exists(r => r.Name == Name))
            {
                o = (T)ConvertTo.FormatValue(XmlSettings.Find(r => r.Name == Name).DefaultValue, DefaultValue.GetType());
            }
            return o;
        }


        #endregion


        #region "找到当前活动菜单的顶级菜单"

        /// <summary>
        /// 找到当前活动菜单的顶级菜单
        /// </summary>
        /// <param name="allTabs"></param>
        /// <returns></returns>
        public TabInfo FindTopTabByActive(List<TabInfo> allTabs)
        {
            TabInfo TopTab = PortalSettings.ActiveTab;
            Int32 i = 0;
            if (allTabs!= null && allTabs.Count >0 &&   allTabs.Exists(r => r.TabID == TopTab.ParentId))
            {
                while (!(TopTab != null && TopTab.TabID > 0 && TopTab.ParentId == Null.NullInteger) && i < 20)
                {
                    i++;
                    TabInfo tempTab = allTabs.Find(r => r.TabID == TopTab.ParentId);
                    if (TopTab != null && TopTab.TabID > 0 && tempTab != null && tempTab.TabID > 0)
                    {
                        TopTab = tempTab;
                    }
                    else
                    {
                        TopTab = PortalSettings.ActiveTab;
                        break;
                    }
                }
            }
            return TopTab;
        }

        /// <summary>
        /// 找到活动菜单上层所有的
        /// 
        /// </summary>
        /// <param name="allTabs"></param>
        /// <returns></returns>
        public List<TabInfo> FindAllTabByActive(List<TabInfo> allTabs)
        {
            List<TabInfo> TempAllTabs = new List<TabInfo>();
            TempAllTabs.Add(PortalSettings.ActiveTab);
            TabInfo TopTab = PortalSettings.ActiveTab;
            Int32 i = 0;
            if (allTabs != null && allTabs.Count > 0 && allTabs.Exists(r => r.TabID == TopTab.ParentId))
            {
                while (!(TopTab != null && TopTab.TabID > 0 && TopTab.ParentId == Null.NullInteger) && i < 20)
                {
                    i++;
                    TabInfo tempTab = allTabs.Find(r => r.TabID == TopTab.ParentId);
                    if (TopTab != null && TopTab.TabID > 0 && tempTab != null && tempTab.TabID > 0)
                    {
                        TopTab = tempTab;
                        TempAllTabs.Add(tempTab);
                    }
                    else
                    {
                        TopTab = PortalSettings.ActiveTab;
                        break;
                    }
                }
            }
            return TempAllTabs;
        }



        /// <summary>
        /// 获取打头的顶级菜单
        /// </summary>
        /// <param name="Tabs"></param>
        /// <returns></returns>
        public Int32 GetTopTab(List<TabInfo> Tabs, TabInfo ActiveTab)
        {
            Int32 TopID = RootParent;
            if (ViewLevel > 0)
            {
                TopID = ActiveTab.TabID;//当前活动菜单，正好是显示的这一级时

                TabInfo tempTab = ActiveTab;

                if (tempTab.Level >= ViewLevel)
                {
                    String TabCacheName = String.Format("PId_{0}-TID_{1}-L_{2}_uid{3}", PortalSettings.PortalId, ActiveTab.TabID, ViewLevel, PortalSettings.UserId);
                    System.Web.Caching.Cache _Cache = HttpRuntime.Cache;

                    if (!(_Cache[TabCacheName] != null && Convert.ToInt32(_Cache[TabCacheName]) > 0))
                    {
                        for (int i = 0; i < 9 && tempTab.Level != ViewLevel && tempTab.ParentId > 0; i++)
                        {
                            TabInfo isParentTAB = Tabs.Find(r1 => r1.TabID == tempTab.ParentId);
                            if (isParentTAB != null && isParentTAB.TabID > 0)
                            {
                                tempTab = isParentTAB;

                            }
                        }
                        TopID = tempTab.ParentId;
                        _Cache.Insert(TabCacheName, TopID);
                    }
                    else
                    {
                        TopID = Convert.ToInt32(_Cache[TabCacheName]);
                    }
                }
                else if (tempTab.Level == 0 && 2 <= ViewLevel)
                {
                    TopID = 0;
                }
                else
                {
                    TopID = tempTab.TabID;
                }

            }
            return TopID;
        }


        #endregion




    }
}