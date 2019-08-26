using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.UI.Skins;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Portals;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using System.Reflection;
using System.IO;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 皮肤对象基类
    /// </summary>
    public class BaseNavObjectBase : BaseSkin
    {
        private String _ClientID = String.Empty;
        /// <summary>
        /// 控件的用户编号
        /// </summary>
        public new String ClientID
        {
            get
            {
                if (String.IsNullOrEmpty(_ClientID))
                {
                    _ClientID = Guid.NewGuid().ToString("N").Substring(0, 10);
                }
                return _ClientID;
            }

        }


        #region "===字段与属性定义==="


    


        #region "==Accordion效果字段与属性定义=="


        private String _autoHeight = "false";
        /// <summary>
        /// 自动高度
        /// </summary>
        public String autoHeight
        {
            get { return _autoHeight; }
            set { _autoHeight = value; }
        }

        private String _animated = "slide";
        /// <summary>
        /// 效果
        /// </summary>
        public String animated
        {
            get { return _animated; }
            set { _animated = value; }
        }


        private String _toggle = "slow";
        /// <summary>
        /// 滑动速度
        /// </summary>
        public String toggle
        {
            get { return _toggle; }
            set { _toggle = value; }
        }



        #endregion

        #region "==AccordionPro效果字段与属性定义=="

        private Int32 _AccordionPro_Sensitivity = 1;
        /// <summary>
        /// 灵敏度
        /// </summary>
        public Int32 AccordionPro_Sensitivity
        {
            get { return _AccordionPro_Sensitivity; }
            set { _AccordionPro_Sensitivity = value; }
        }

        private Int32 _AccordionPro_Interval = 100;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public Int32 AccordionPro_Interval
        {
            get { return _AccordionPro_Interval; }
            set { _AccordionPro_Interval = value; }
        }


        private Int32 _AccordionPro_Timeout = 100;
        /// <summary>
        /// 延时
        /// </summary>
        public Int32 AccordionPro_Timeout
        {
            get { return _AccordionPro_Timeout; }
            set { _AccordionPro_Timeout = value; }
        }



        private Int32 _AccordionPro_AnimateSpeed = 250;
        /// <summary>
        /// 速度
        /// </summary>
        public Int32 AccordionPro_AnimateSpeed
        {
            get { return _AccordionPro_AnimateSpeed; }
            set { _AccordionPro_AnimateSpeed = value; }
        }

 

        #endregion

        #region "==MegaMenu效果字段与属性定义=="

        private Int32 _MegaMenu_Column = 2;
        /// <summary>
        /// 纵列
        /// </summary>
        public Int32 MegaMenu_Column
        {
            get { return _MegaMenu_Column; }
            set { _MegaMenu_Column = value; }
        }


        private Int32 _MegaMenu_Sensitivity = 2;
        /// <summary>
        /// 触觉
        /// </summary>
        public Int32 MegaMenu_Sensitivity
        {
            get { return _MegaMenu_Sensitivity; }
            set { _MegaMenu_Sensitivity = value; }
        }

        private Int32 _MegaMenu_Interval = 100;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public Int32 MegaMenu_Interval
        {
            get { return _MegaMenu_Interval; }
            set { _MegaMenu_Interval = value; }
        }

        private Int32 _MegaMenu_Timeout = 500;
        /// <summary>
        /// 延迟时间
        /// </summary>
        public Int32 MegaMenu_Timeout
        {
            get { return _MegaMenu_Timeout; }
            set { _MegaMenu_Timeout = value; }
        }


        #endregion

        #region "==MultiMenu效果字段与属性定义=="



        private String _MultiMenuAction = "multi";
        /// <summary>
        /// 菜单动作
        /// </summary>
        public String MultiMenuAction
        {
            get { return _MultiMenuAction; }
            set { _MultiMenuAction = value; }
        }


        #endregion

        #region "==定制专用字段与属性定义=="


        private Boolean _TopMeunTitle = false;
        /// <summary>
        /// 是否显示顶级菜单的小标题(定制选项)
        /// </summary>
        public Boolean TopMeunTitle
        {
            get { return _TopMeunTitle; }
            set { _TopMeunTitle = value; }
        }


        private Boolean _FastSubcurrent = false;
        /// <summary>
        /// 首个下级选中样式
        /// </summary>
        public Boolean FastSubcurrent
        {
            get { return _FastSubcurrent; }
            set { _FastSubcurrent = value; }
        }




        private Boolean _StandardMenu = false;
        /// <summary>
        /// HTML菜单的顶级样式
        /// </summary>
        public Boolean StandardMenu
        {
            get { return _StandardMenu; }
            set { _StandardMenu = value; }
        }

 

        #endregion



        #region "==Mobile效果字段与属性定义=="


        private String _slidingSubmenus = "true";
        /// <summary>
        /// 
        /// </summary>
        public String slidingSubmenus
        {
            get { return _slidingSubmenus; }
            set { _slidingSubmenus = value; }
        }


        private String _counters = "true";
        /// <summary>
        /// 
        /// </summary>
        public String counters
        {
            get { return _counters; }
            set { _counters = value; }
        }

        private String _navbartitle = "Menu";
        /// <summary>
        /// 
        /// </summary>
        public String navbartitle
        {
            get { return _navbartitle; }
            set { _navbartitle = value; }
        }

        private String _headerbox = ".menu_header";
        /// <summary>
        /// 
        /// </summary>
        public String headerbox
        {
            get { return _headerbox; }
            set { _headerbox = value; }
        }





        private String _footerbox = ".menu_footer";
        /// <summary>
        /// 
        /// </summary>
        public String footerbox
        {
            get { return _footerbox; }
            set { _footerbox = value; }
        }

        #endregion




        #endregion






        #region "===方法函数定义==="


        #region "==基本=="




        /// <summary>
        /// 加载脚本
        /// </summary>
        public void LoadScriptForJqueryAndUI()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if (objCSS != null)
            {
                String jQueryUrl = String.Format("{0}/Resource/js/jquery.min.js?cdv={1}", ModulePath, CrmVersion);
                String jQueryUIUrl = String.Format("{0}/Resource/js/jquery-ui.min.js?cdv={1}", ModulePath, CrmVersion);


                if (!HttpContext.Current.Items.Contains("DNNGo_jQueryUI"))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUIUrl);
                    HttpContext.Current.Items.Add("DNNGo_jQueryUI", "true");
                    if (!HttpContext.Current.Items.Contains("jQueryUIRequested")) HttpContext.Current.Items.Add("jQueryUIRequested", "true");
                    objCSS.Controls.AddAt(0, litLink);
                }

                if (!HttpContext.Current.Items.Contains("DNNGo_jQuery"))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUrl);
                    HttpContext.Current.Items.Add("DNNGo_jQuery", "true");
                    if (!HttpContext.Current.Items.Contains("jquery_registered")) HttpContext.Current.Items.Add("jquery_registered", "true");
                    if (!HttpContext.Current.Items.Contains("jQueryRequested")) HttpContext.Current.Items.Add("jQueryRequested", "true");

                    objCSS.Controls.AddAt(0, litLink);
                }

            }
        }


        /// <summary>
        /// 模块/皮肤对象路径
        /// </summary>
        public string ModulePath
        {
            get { return TemplateSourceDirectory; }
        }

        private string _CrmVersion = String.Empty;
        /// <summary>
        /// 引用文件版本
        /// </summary>
        public string CrmVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_CrmVersion))
                {
                    _CrmVersion = "0";
                    string setting = GetHostSetting("CrmVersion");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        _CrmVersion = setting;
                    }
                }
                return _CrmVersion;
            }
        }

        public string GetHostSetting(string key, string defaultValue = "")
        {
            return DotNetNuke.Entities.Controllers.HostController.Instance.GetString(key, defaultValue); ;
        }


        #endregion




        #region "==菜单的样式控制=="


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
            //string tabName = string.Format("<span><span class=\"gomenuIcon tabid{0}\"></span>{1}</span>", tab.TabID, LocalizedTabName);//http://arabaviation.com 这个客户才需要的
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
            //if (!String.IsNullOrEmpty(tab.IconFile))
            //{
            //    IconFile = tab.IconFile;

            //    if (IconFile.IndexOf("~/", StringComparison.CurrentCultureIgnoreCase) < 0 && Common.ConvertVersion(PortalSettings.Version) <= Common.ConvertVersion("5.4.4"))
            //    {
            //        if (tab.IsAdminTab)
            //        {
            //            IconFile = String.Format("~/images/{0}", IconFile);
            //        }
            //        else 
            //        {
            //            IconFile = PortalSettings.HomeDirectory + IconFile;
            //        }
            //    }
            //    IconFile = String.Format("<img src=\"{0}\" class=\"menuicon\"  /> ", ResolveUrl(IconFile));
            //}

            String IconSettingName = String.Format("ThemePlugin_TabIcon_{0}", tab.TabID);//Portal_Settings
            if (Portal_Settings != null && Portal_Settings.ContainsKey(IconSettingName))
            {
                String IconCss = Convert.ToString(Portal_Settings[IconSettingName]);
                if (!String.IsNullOrEmpty(IconCss))
                {
                    IconFile = String.Format("<i class=\"{0}\"></i>", IconCss);
                }
            }
            return IconFile;
        }

        /// <summary>
        /// 获取当前选中菜单样式
        /// </summary>
        /// <param name="Tab"></param>
        /// <returns></returns>
        public string GetActiveTabStyle(TabInfo Tab)
        {
            if (base.PortalSettings.ActiveTab.TabID == Tab.TabID) return string.Format(" {0}", "current");
            return "";
        }

        /// <summary>
        /// 获取当前选中菜单样式
        /// </summary>
        /// <param name="Tab"></param>
        /// <param name="IsChildActive"></param>
        /// <returns></returns>
        public string GetActiveTabStyle(TabInfo Tab, bool IsChildActive)
        {
            return GetActiveTabStyle(Tab, IsChildActive, "current");
        }

        /// <summary>
        /// 获取当前选中菜单样式
        /// </summary>
        /// <param name="Tab"></param>
        /// <param name="IsChildActive"></param>
        /// <returns></returns>
        public string GetActiveTabStyle(TabInfo Tab, bool IsChildActive,String CssClass)
        {
            if (base.PortalSettings.ActiveTab.TabID != Tab.TabID && !IsChildActive) return "";
            return string.Format(" {0}", CssClass);
        }
        /// <summary>
        /// 获取前后圆弧的样式
        /// </summary>
        /// <param name="TabId">当前菜单编号</param>
        /// <param name="FirstTabId">最前面菜单编号</param>
        /// <param name="LastTabId">最后面菜单编号</param>
        /// <returns>输出圆弧的样式</returns>
        public string GetCircularBeadStyle(int TabId, int FirstTabId, int LastTabId)
        {
            if (LastTabId == FirstTabId && TabId == LastTabId) return " last-item";
            if (FirstTabId != 0 && TabId == FirstTabId) return " first-item";
            if (LastTabId != 0 && TabId == LastTabId) return " last-item";
            return "";
        }

        /// <summary>
        /// 获取最前和最后的页面
        /// </summary>
        /// <param name="tabs"></param>
        /// <param name="ParentId"></param>
        /// <param name="FirstTabId"></param>
        /// <param name="LastTabId"></param>
        public void GetFirstLastTabId(List<TabInfo> tabs, int ParentId, ref int FirstTabId, ref int LastTabId)
        {
            int tabID = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].ParentId == ParentId)
                {
                    if (tabID == 0) FirstTabId = tabs[i].TabID;
                    tabID = tabs[i].TabID;
                }
            }
            LastTabId = tabID;
        }
        ///// <summary>
        ///// 获取项的索引
        ///// </summary>
        ///// <returns></returns>
        //public string GetItemIndex()
        //{
        //    this.ItemIndex++;
        //    return string.Format(" Item-{0}", this.ItemIndex);
        //}
        //public int ItemIndex
        //{
        //    get
        //    {
        //        if (this.ViewState["ItemIndex"] == null) return 0;
        //        return Convert.ToInt32(this.ViewState["ItemIndex"]);
        //    }
        //    set { this.ViewState["ItemIndex"] = value; }
        //}
        #endregion



        #region "==菜单筛选=="

        /// <summary>
        /// 获取打头的顶级菜单
        /// </summary>
        /// <param name="Tabs"></param>
        /// <returns></returns>
        public Int32 GetTopTab(List<TabInfo> Tabs, TabInfo ActiveTab)
        {
            Int32 TopID = RootParent;
            if (ViewLevel > 0  )
            {
                    TopID = ActiveTab.TabID;//当前活动菜单，正好是显示的这一级时
              
                    TabInfo tempTab = ActiveTab;

                    if (tempTab.Level >= ViewLevel )
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

        /// <summary>
        /// 找到自己的顶级菜单
        /// </summary>
        /// <param name="Tabs"></param>
        /// <param name="ThisTab"></param>
        /// <returns></returns>
        public TabInfo GetTopTabInfo(List<TabInfo> Tabs, TabInfo ThisTab)
        {
            TabInfo tempTab = ThisTab;
            while (tempTab.ParentId != -1)
            {
                foreach (TabInfo t in Tabs)//找到当前菜单的上级菜单
                {
                    if (tempTab.ParentId == t.TabID)
                    {
                        //找到上级菜单的情况
                        tempTab = t;
                        break;
                    }
                }
            }
            return tempTab;
        }



        /// <summary>
        /// 查找活动菜单集合
        /// </summary>
        /// <param name="Tabs">所有菜单</param>
        /// <param name="ActiveTab">当前点击菜单</param>
        /// <returns>活动菜单集合</returns>
        public List<TabInfo> GetActiveTabs(List<TabInfo> Tabs, TabInfo ActiveTab)
        {
            List<TabInfo> ActiveTabs = new List<TabInfo>();
            ActiveTabs.Add(ActiveTab);
            TabInfo tempTab = ActiveTab;
            while (tempTab.ParentId > -1)
            {
                bool r = false;
                foreach (TabInfo tab in Tabs)
                {
                    if (tempTab.ParentId == tab.TabID)
                    {
                        ActiveTabs.Add(tab);
                        tempTab = tab;
                        r = true;
                        break;
                    }
                }
                if (!r) break;
            }
            return ActiveTabs;
        }

      



        /// <summary>
        /// 是否活动菜单
        /// </summary>
        /// <param name="Tabs">活动菜单集合</param>
        /// <param name="ActiveTab">当前菜单</param>
        /// <returns></returns>
        public Boolean IsActiveTab(List<TabInfo> Tabs, TabInfo ActiveTab)
        {
            Boolean _IsActiveTab = false;
            foreach (TabInfo tab in Tabs)
            {
                if (tab.TabID == ActiveTab.TabID)
                {
                    _IsActiveTab = true;
                    break;
                }

            }
            return _IsActiveTab;
        }

        /// <summary>
        /// 是否显示首级菜单样式
        /// </summary>
        /// <param name="Tabs"></param>
        /// <param name="ThisTab"></param>
        /// <param name="ActiveTab"></param>
        public Boolean IsFastSubcurrent(List<TabInfo> tabs, List<TabInfo> ActiveTabs, TabInfo ThisTab, Int32 firstTabId)
        {
            Boolean IsFast = false;
            //当前菜单要是第一个菜单
            if (ThisTab.TabID == firstTabId)
            {
                //当前菜单要是第二级   
                if (ThisTab.Level == 1)
                {
                    //自己的顶级不是活动菜单时
                    TabInfo TopTabInfo = GetTopTabInfo(tabs, ThisTab);

                    //如果自己就是顶级菜单时
                    if (PortalSettings.ActiveTab.TabID == TopTabInfo.TabID)
                    {
                        return true;
                    }


                    if (!IsActiveTab(ActiveTabs, TopTabInfo))
                    {

                        return true;
                    }
                }
            }

            return IsFast;
        }

        #endregion







        /// <summary>
        /// 加载脚本
        /// </summary>
        public void LoadScriptForJqueryAndUI(string modulePath)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
         
                //Int32 version = Common.ConvertVersion(PortalSettings.Version);
 
                //if (version < Common.ConvertVersion("6.0.0"))
                //{

                //    if (HttpContext.Current.Items["DNNGo_jQueryUI"] == null)
                //    {
                //        Literal litLink = new Literal();

                //        litLink.Text = Microsoft.VisualBasic.Constants.vbCrLf + "<script src=\" http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js\" type=\"text/javascript\"></script>" + Microsoft.VisualBasic.Constants.vbCrLf;
                //        HttpContext.Current.Items.Add("DNNGo_jQueryUI", "true");
                //        objCSS.Controls.AddAt(0, litLink);
                //    }
                //}

                //if (version < Common.ConvertVersion("6.0.0") && HttpContext.Current.Items["DNNGo_jQuery"] == null && HttpContext.Current.Items["jquery_registered"] == null)
                //{
                //    Literal litLink = new Literal();
                //    //litLink.Text =Microsoft.VisualBasic.Constants.vbCrLf + "<script type=\"text/javascript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js\"></script>" +Microsoft.VisualBasic.Constants.vbCrLf;
                //    litLink.Text = Microsoft.VisualBasic.Constants.vbCrLf + "<script type=\"text/javascript\" src=\"" + modulePath + "Resource/js/jquery-1.7.min.js\"></script>" + Microsoft.VisualBasic.Constants.vbCrLf;
                //    HttpContext.Current.Items.Add("DNNGo_jQuery", "true");
                //    HttpContext.Current.Items.Add("jquery_registered", "true");
                //    objCSS.Controls.AddAt(0,litLink);
                //}


            }
        }


        /// <summary>
        /// 绑定JS文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String JsName,String modulePath)
        {
 
            string ItemKey = String.Format("DNNGo_GoMenu_{0}_js", JsName);
            if (HttpContext.Current.Items[ItemKey] == null)
            {
                HttpContext.Current.Items.Add(ItemKey, "true");
                DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                ClientResourceManager.RegisterScript(this.Page, String.Format("{0}Resource/js/{1}", modulePath, JsName));

            }
        }

        #endregion


    }
}