using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.UI.Skins;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Entities.Users;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Common.Utilities;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 皮肤对象&导航栏基类
    /// </summary>
    public class BaseSkin : NavObjectBase
    {

 

        /// <summary>
        /// 页面操作类
        /// </summary>
        public TabController objTabs = new TabController();


        #region "初始化的代码"
        /// <summary>
        /// 初始化的代码
        /// </summary>
        public void Inits()
        {
            Initialization init = new Initialization(this);
            init.Init();
        }


        /// <summary>
        /// 错误信息
        /// </summary>
        public String skin_Error = WebHelper.GetStringParam(HttpContext.Current.Request, "error", "");


        #endregion



        #region "基础属性定义"

 

        private String _SkinPath = String.Empty;
        /// <summary>
        /// 皮肤文件名
        /// </summary>
        public String SkinPath
        {
            get
            {
                if (String.IsNullOrEmpty(_SkinPath))
                {

                    _SkinPath = PortalSettings.ActiveTab.SkinPath;

                }
                return _SkinPath;
            }
        }


        /// <summary>
        /// 是否管理员
        /// </summary>
        public Boolean IsAdmin
        {
            get
            {
                UserInfo u = PortalSettings.UserInfo;
                return u != null && u.UserID > 0 && (u.IsSuperUser || u.IsInRole("Administrators"));
               
            }
        }


        /// <summary>
        /// 是否普通管理员
        /// </summary>
        public Boolean IsAdministrator
        {
            get
            {
                UserInfo u = PortalSettings.UserInfo;
                return u != null && u.UserID > 0 && u.IsInRole("Administrators") && !u.IsSuperUser;
            }
        }

        /// <summary>
        /// 是否Host管理员
        /// </summary>
        public Boolean IsHost
        {
            get
            {
                UserInfo u = PortalSettings.UserInfo;
                return u != null && u.UserID > 0 && u.IsSuperUser;
            }
        }


        /// <summary>
        /// 演示锁(true为demo状态)
        /// (检索目录下有无demo.lock)
        /// </summary>
        public Boolean DemoLock
        {
            get { return File.Exists(MapPath(String.Format("{0}demo.lock", ModulePath))); }
        }

        public string ControlPath
        {
            get { return this.TemplateSourceDirectory + "/"; }
        }

        private string _localResourceFile;
        public string LocalResourceFile
        {
            get
            {
                string fileRoot;
                if (string.IsNullOrEmpty(_localResourceFile))
                {
                    fileRoot = Path.Combine(ControlPath, DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/" + this.ID);
                }
                else
                {
                    fileRoot = _localResourceFile;
                }
                return fileRoot;
            }
            set { _localResourceFile = value; }
        }


        private Dictionary<string, string> _Portal_Settings = new Dictionary<string, string>();
        /// <summary>
        /// 模块的设置
        /// </summary>
        public Dictionary<string, string> Portal_Settings
        {
            get
            {
                if (!(_Portal_Settings != null && _Portal_Settings.Count > 0))
                {

                    _Portal_Settings = PortalController.Instance.GetPortalSettings(PortalSettings.PortalId);
                }
                return _Portal_Settings;
            }
        }

        #endregion


        #region "关于图标处理的代码"


        /// <summary>
        /// 根据后缀名显示图标
        /// </summary>
        /// <param name="FileExtension">文件后缀</param>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public String GetPhotoExtension(String FileExtension, String FilePath)
        {
            //先判断是否是图片格式的
            if (FileExtension == "jpg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "jpeg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "png")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "gif")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "bmp")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "mp3")
                return GetFileIcon("audio.png");
            else if (FileExtension == "wma")
                return GetFileIcon("audio.png");
            else if (FileExtension == "zip")
                return GetFileIcon("archive.png");
            else if (FileExtension == "rar")
                return GetFileIcon("archive.png");
            else if (FileExtension == "7z")
                return GetFileIcon("archive.png");
            else if (FileExtension == "xls")
                return GetFileIcon("spreadsheet.png");
            else if (FileExtension == "txt")
                return GetFileIcon("text.png");
            else if (FileExtension == "cs")
                return GetFileIcon("code.png");
            else if (FileExtension == "html")
                return GetFileIcon("code.png");
            else if (FileExtension == "doc")
                return GetFileIcon("document.png");
            else if (FileExtension == "docx")
                return GetFileIcon("document.png");
            else
                return GetFileIcon("default.png");
        }

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath)
        {
            return String.Format("{0}{1}", PortalSettings.HomeDirectory, FilePath);
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="IconName">图标文件</param>
        /// <returns></returns>
        public String GetFileIcon(String IconName)
        {
            return String.Format("{0}Resource/images/crystal/{1}", BasePath, IconName);
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

        #region "站点设置更新与获取"
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public String GetSetting(string SettingName)
        {
            return GetSetting(SettingName, String.Empty);
        }
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public String GetSetting(string SettingName, String defaultValue)
        {
            return PortalController.GetPortalSetting(SettingsFormat(SettingName),PortalSettings.PortalId, defaultValue);
        }
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Int32 GetSetting(string SettingName, Int32 defaultValue)
        {
            return PortalController.GetPortalSettingAsInteger(SettingsFormat(SettingName), PortalSettings.PortalId, defaultValue);
        }
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Boolean GetSetting(string SettingName, Boolean defaultValue)
        {
            return PortalController.GetPortalSettingAsBoolean(SettingsFormat(SettingName), PortalSettings.PortalId, defaultValue);
        }

        /// <summary>
        /// 更新当前模块的设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateSetting(string SettingName, string SettingValue)
        {
            UpdateSetting(PortalSettings.PortalId, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateSetting(int PortalId, string SettingName, string SettingValue)
        {
            PortalController.UpdatePortalSetting(PortalId, SettingsFormat(SettingName), SettingValue);
        }




        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <returns></returns>
        public String SettingsFormat( String Name)
        {
            return String.Format("ThemePlugin_{0}", Name);
        }

        #endregion

        #region "获取版本号"




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
                    _CrmVersion = DateTime.Now.ToString("yyyyMMdd");
                    string setting = GetHostSetting("CrmVersion");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        _CrmVersion = String.Format("{0}.{1}", _CrmVersion, setting);
                    }
                }
                return _CrmVersion;
            }
        }

        public string GetHostSetting(string key, string defaultValue = "")
        {
            return HostController.Instance.GetString(key, defaultValue); ;
        }


        #endregion

        #region "引用脚本和样式文件"

        /// <summary>
        /// 引用脚本文件
        /// </summary>
        /// <param name="JsName"></param>
        public void LoadScript(String JsName)
        {
            if (HttpContext.Current.Items[JsName] == null)
            {
                HttpContext.Current.Items.Add(JsName, "true");
                DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //Page.ClientScript.RegisterClientScriptInclude(JsName, String.Format("{0}Resource/js/{1}?cdv={2}", BasePath, JsName, CrmVersion));
                ClientResourceManager.RegisterScript(this.Page, String.Format("{0}Resource/js/{1}", BasePath, JsName));

            }
        }

        /// <summary>
        /// 引用样式表文件
        /// </summary>
        /// <param name="cssName"></param>
        public void LoadStyle(String cssName)
        {
            //System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            //if ((objCSS != null))
            //{
                if (HttpContext.Current.Items[cssName] == null)
                {
                    HttpContext.Current.Items.Add(cssName, "true");
                    ClientResourceManager.RegisterStyleSheet(this.Page, String.Format("{0}Resource/css/{1}", BasePath, cssName));
                    //Literal litLink = new Literal();
                    //litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}Resource/css/{1}?cdv={2}\" />", BasePath, cssName, CrmVersion);
                    //HttpContext.Current.Items.Add(cssName, "true");
                    //objCSS.Controls.Add(litLink);
                }
            //}
        }

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void LoadStyle(String ThemeName, String ThemePath)
        {
            //System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            //if ((objCSS != null))
            //{
            string ItemKey = String.Format("DNNGo_ThemePlugin_{0}_font", ThemeName);
            if (HttpContext.Current.Items[ItemKey] == null)
            {
                HttpContext.Current.Items.Add(ItemKey, "true");
                ClientResourceManager.RegisterStyleSheet(this.Page, ThemePath);

                //Literal litLink = new Literal();
                //litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", ThemePath);

                //HttpContext.Current.Items.Add(ItemKey, "true");
                //objCSS.Controls.Add(litLink);
            }
            // }
        }


        #endregion


        #region "==共用字段与属性定义=="


        private String _Effect = "HSlide";
        /// <summary>
        /// 效果
        /// HSlide：是默认的值,导航横向显示，菜单向下弹出。
        /// VSlide：导航垂直显示，菜单往右边弹出
        /// Accordion： 手风琴效果
        /// Html：输出的是HTML，不带任何的脚本
        /// MegaMenu:输出带横向二级菜单的
        /// </summary>
        public String Effect
        {
            get { return _Effect; }
            set { _Effect = value; }
        }


        private String _ShowUI = "Normal";
        /// <summary>
        /// UI的显示方式
        /// Normal
        /// Extension
        /// </summary>
        public String ShowUI
        {
            get { return _ShowUI; }
            set { _ShowUI = value; }
        }


        private Int32 _ViewLevel = 0;
        /// <summary>
        /// 显示级别(0从顶级显示,1从第二级显示)
        /// </summary>
        public Int32 ViewLevel
        {
            get { return _ViewLevel; }
            set { _ViewLevel = value; }
        }



        private String _CssClass = "";
        /// <summary>
        /// 菜单样式
        /// </summary>
        public String CssClass
        {
            get { return _CssClass; }
            set { _CssClass = value; }
        }


        private Boolean _ShowTitle = false;
        /// <summary>
        /// 显示标题
        /// </summary>
        public Boolean ShowTitle
        {
            get { return _ShowTitle; }
            set { _ShowTitle = value; }
        }

        private Boolean _ShowTooltip = true;
        /// <summary>
        /// 显示菜单的提示
        /// </summary>
        public Boolean ShowTooltip
        {
            get { return _ShowTooltip; }
            set { _ShowTooltip = value; }
        }


        private String _XmlName = "tabs.xml";
        /// <summary>
        /// XML数据名称
        /// </summary>
        public String XmlName
        {
            get { return _XmlName; }
            set { _XmlName = value; }
        }


        private String _XmlWebPath = String.Empty;
        /// <summary>
        /// 指定的XML路径
        /// </summary>
        public String XmlWebPath
        {
            get { return _XmlWebPath; }
            set { _XmlWebPath = value; }
        }





        private Boolean _ShowXml = false;
        /// <summary>
        /// 显示XML菜单的数据
        /// </summary>
        public Boolean ShowXml
        {
            get { return _ShowXml; }
            set { _ShowXml = value; }
        }


        private Boolean _ShowDNN = true;
        /// <summary>
        /// 显示DNN菜单的数据
        /// </summary>
        public Boolean ShowDNN
        {
            get { return _ShowDNN; }
            set { _ShowDNN = value; }
        }


        private Int32 _MaxLevel = 999;
        /// <summary>
        /// 最大显示级别
        /// </summary>
        public Int32 MaxLevel
        {
            get { return _MaxLevel; }
            set { _MaxLevel = value; }
        }

        private Int32 _RootParent = Null.NullInteger;
        /// <summary>
        /// 顶级根目录的编号(默认为根目录)
        /// </summary>
        public Int32 RootParent
        {
            get { return _RootParent; }
            set { _RootParent = value; }
        }

        #endregion


        #region "==权限与菜单定义=="

        /// <summary>
        /// 获取当前用户可以看到的菜单
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="uInfo"></param>
        /// <returns></returns>
        public List<TabInfo> GetUserTabs(PortalSettings ps, UserInfo uInfo)
        {
            int portalId = ps.PortalId;
            new TabController();

            List<TabInfo> list2 = new List<TabInfo>();
            if (ShowDNN)
            {

                var desktopTabs = TabController.GetPortalTabs(ps.PortalId, Null.NullInteger, false, "none available", false, false, true, true, false);

                list2.AddRange(desktopTabs);
                //List<TabInfo> desktopTabs = TabController.GetPortalTabs(portalId, Null.NullInteger, false, false);

                //int num = 0;
                //int portalID = uInfo.PortalID;
                //if (uInfo.IsSuperUser)
                //    num = 999;//超级管理员权限
                //else if (uInfo.IsInRole("Administrators"))
                //    num = 1;//管理员权限
                //else if (uInfo.UserID > 0)
                //    num = 0;//登录用户权限
                //else
                //    num = -1;//匿名用户权限
                //foreach (TabInfo item in desktopTabs)
                //{






                //    if (!item.IsDeleted && item.IsVisible)
                //    {
                //        switch (num)
                //        {
                //            case 999:
                //                {
                //                    list2.Add(item);
                //                    continue;
                //                }
                //            case 1:
                //                {
                //                    if (item.PortalID == portalID && (item.AuthorizedRoles.Contains("All Users") || item.AuthorizedRoles.Contains("Administrators"))) list2.Add(item);
                //                    continue;
                //                }
                //            case 0:
                //                {
                //                    if (item.PortalID == portalID && !Refusal(uInfo, item.TabPermissions) && (item.AuthorizedRoles.Contains("All Users") || IsRoles(uInfo.Roles, item.AuthorizedRoles) || IsUsers(uInfo, item.TabPermissions))) list2.Add(item);
                //                    continue;
                //                }
                //        }
                //        if (num == -1 && !Refusal(uInfo, item.TabPermissions) && (item.AuthorizedRoles.Contains("All Users") || item.AuthorizedRoles.Contains("Unauthenticated Users"))) list2.Add(item);
                //    }
                //}
            }

            if (ShowXml)
            {
                //加入XML中的数据
                list2.AddRange(GetXmlTabs());
            }

            return list2;
        }


        /// <summary>
        /// 是否有角色权限
        /// </summary>
        /// <param name="UserRoles"></param>
        /// <param name="TabAuthorizedRoles"></param>
        /// <returns></returns>
        public bool IsRoles(string[] UserRoles, string TabAuthorizedRoles)
        {
            List<String> listTabAuthorizedRoles = Common.GetList(TabAuthorizedRoles, ";");
            foreach (string str in UserRoles)
            {
                if (listTabAuthorizedRoles.Exists(r1 => r1.ToLower() == str.ToLower())) return true;
            }
            return false;
        }
        /// <summary>
        /// 当前用户是否有权限
        /// </summary>
        /// <param name="uInfo"></param>
        /// <param name="Permissions"></param>
        /// <returns></returns>
        public bool IsUsers(UserInfo uInfo, TabPermissionCollection Permissions)
        {
            foreach (TabPermissionInfo tp in Permissions)
            {
                if (tp.UserID == uInfo.UserID) return true;
            }

            return false;
        }


        /// <summary>
        /// 是否为拒绝权限用户或角色
        /// </summary>
        /// <param name="uInfo">当前用户</param>
        /// <param name="Permissions">菜单权限列表</param>
        /// <returns></returns>
        public bool Refusal(UserInfo uInfo, TabPermissionCollection Permissions)
        {
            bool i = false;
            foreach (TabPermissionInfo tp in Permissions)
            {
                if (!(uInfo != null && uInfo.UserID > 0) && !tp.AllowAccess)//匿名用户
                {
                    if (tp.UserID == -1 && tp.RoleID == -1)
                    {
                        i = true;
                    }
                }
                else//登陆用户
                {
                    if (!tp.AllowAccess)
                    {
                        if (tp.UserID > 0 && tp.UserID == uInfo.UserID)
                        {
                            i = true;//判断用户是否有拒绝的
                        }

                        if (tp.RoleID > 0 && uInfo.IsInRole(tp.RoleName))
                        {
                            i = true;//判断角色是否有拒绝的
                        }

                        if (tp.UserID == -1 && tp.RoleID == -1)
                        {
                            i = true;//判断所有用户是否有拒绝的
                        }

                    }

                }
            }
            return i;
        }


        #endregion

        #region "==XML菜单数据的读取=="

        /// <summary>
        /// XML文件的路径
        /// </summary>
        public String XmlPath
        {
            get
            {

                String _XmlPath = MapPath(String.Format("{0}{1}", PortalSettings.ActiveTab.SkinPath, XmlName));
                if (!String.IsNullOrEmpty(XmlWebPath))
                {
                    _XmlPath = MapPath(String.Format("{0}{1}", XmlWebPath, XmlName));
                }
                return _XmlPath;

            }



        }

        /// <summary>
        /// 获取XML文件中的页面数据
        /// </summary>
        /// <returns></returns>
        public List<TabInfo> GetXmlTabs()
        {
            List<TabInfo> tabs = new List<TabInfo>();
            FileInfo file = new FileInfo(XmlPath);
            if (file.Exists)
            {
                XmlFormat xf = new XmlFormat(file.FullName);

                tabs = ConvertToTab(xf.ToList<TabEntity>());


            }
            return tabs;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<TabInfo> ConvertToTab(List<TabEntity> list)
        {
            List<TabInfo> tabs = new List<TabInfo>();
            //TabInfo baseTab = new DotNetNuke.Entities.Tabs.TabController().GetTab(PortalSettings.HomeTabId);
            foreach (TabEntity tabItem in list)
            {
                TabInfo tab = new TabInfo();
                tab.TabID = tabItem.TabID;
                tab.Url = tabItem.FullUrl;
                tab.ParentId = tabItem.ParentId;
                tab.PortalID = tabItem.PortalID;
                tab.TabName = tabItem.TabName;
                tab.TabOrder = tabItem.TabOrder;
                tab.Title = tabItem.Title;


                if (string.IsNullOrEmpty(tabItem.FullUrl))
                {
                    tab.DisableLink = true;
                }

                tabs.Add(tab);
            }
            return tabs;
        }

        #endregion






        #region "配置信息"
        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="ValueList"></param>
        /// <param name="Name"></param>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public List<KeyValueEntity> UpdateSettings(List<KeyValueEntity> ValueList, String Name, String StrValue)
        {
            if (ValueList.Exists(r => r.Key == "Name"))
            {
                ValueList[ValueList.FindIndex(r => r.Key == "Name")].Value = StrValue;
            }
            else
            {
                ValueList.Add(new KeyValueEntity() { Key = Name, Value = StrValue });
            }


            return ValueList;
        }



        #region "菜单设置"

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSetting(DNNGo_ThemePlugin_Menu DataItem, String Name, object DefaultValue)
        {
            return GetSettingByStatic(DataItem, Name, DefaultValue);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T GetSettingT<T>(DNNGo_ThemePlugin_Menu DataItem, String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(GetSetting(DataItem, Name, DefaultValue), typeof(T));
        }

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSettingByStatic(DNNGo_ThemePlugin_Menu DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = DataItem.Options.ToList<KeyValueEntity>();
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                        o = KeyValue.Value;
                    }
                }
                catch
                {

                }
            }
            return o;
        }
        #endregion

        #region "列设置"

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSetting(DNNGo_ThemePlugin_MenuPane DataItem, String Name, object DefaultValue)
        {
            return GetSettingByStatic(DataItem, Name, DefaultValue);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T GetSettingT<T>(DNNGo_ThemePlugin_MenuPane DataItem, String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(GetSetting(DataItem, Name, DefaultValue), typeof(T));
        }

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSettingByStatic(DNNGo_ThemePlugin_MenuPane DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = DataItem.Options.ToList<KeyValueEntity>();
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                        o = KeyValue.Value;
                    }
                }
                catch
                {

                }
            }
            return o;
        }
        #endregion

        #region "行设置"

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSetting(DNNGo_ThemePlugin_MenuRowItem DataItem, String Name, object DefaultValue)
        {
            return GetSettingByStatic(DataItem, Name, DefaultValue);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T GetSettingT<T>(DNNGo_ThemePlugin_MenuRowItem DataItem, String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(GetSetting(DataItem, Name, DefaultValue), typeof(T));
        }

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object GetSettingByStatic(DNNGo_ThemePlugin_MenuRowItem DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = DataItem.Options.ToList<KeyValueEntity>();
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                        o = KeyValue.Value;
                    }
                }
                catch
                {

                }
            }
            return o;
        }
        #endregion

        #endregion



        #region "DNN 920 的支持"

        #region "获取模块信息属性DNN920"

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="m">模块信息</param>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(ModuleInfo m, String Name)
        {
            bool propertyNotFound = false;
            return m.GetProperty(Name, "", System.Globalization.CultureInfo.CurrentCulture, null, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
        }

        ///// <summary>
        ///// 获取模块信息属性DNN920
        ///// </summary>
        ///// <param name="Name">属性名</param>
        ///// <returns></returns>
        //public String ModuleProperty(String Name)
        //{
        //    return ModuleProperty(ModuleConfiguration, Name);
        //}


        /// <summary>
        /// 获取站点信息属性DNN920
        /// </summary>
        /// <param name="p">模块信息</param>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String PortalProperty(PortalSettings p, String Name)
        {
            bool propertyNotFound = false;
            return p.GetProperty(Name, "", null, null, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
        }

        /// <summary>
        /// 获取站点信息属性DNN920
        /// </summary>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String PortalProperty(String Name)
        {
            return PortalProperty(PortalSettings, Name);
        }

        #endregion

        #region "模块路径"
        /// <summary>
        /// 模块路径
        /// </summary>
        public String ModulePath
        {
            get { return ControlPath; }
        }

        #endregion

        #endregion



    }
}