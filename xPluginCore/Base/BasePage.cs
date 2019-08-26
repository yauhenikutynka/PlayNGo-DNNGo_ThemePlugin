using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Host;

using DotNetNuke.Entities.Tabs;
using System.IO;
using DotNetNuke.Services.Localization;
using System.Collections;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security;
using System.Web.UI.WebControls;
using DotNetNuke.Services.FileSystem;

namespace DNNGo.Modules.ThemePlugin
{
    public class BasePage : DotNetNuke.Framework.PageBase
    {


        #region "获取DNN对象"

        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId = WebHelper.GetIntParam(HttpContext.Current.Request, "ModuleId", 0);

        public Int32 PortalId = WebHelper.GetIntParam(HttpContext.Current.Request, "PortalId", 0);
        public Int32 TabId = WebHelper.GetIntParam(HttpContext.Current.Request, "TabId", 0);


        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return UserController.GetCurrentUserInfo(); }
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    return UserInfo.UserID;
                }
                else
                {
                    return Null.NullInteger;
                }
            }
        }


        public String QueryString
        {
            get { return String.Format("{0}&ModulePath={1}", WebHelper.GetScriptNameQueryString, HttpUtility.UrlEncode(ModulePath)); }
        }



        //private PortalSettings _portalSettings;
        ///// <summary>
        ///// 站点设置
        ///// </summary>
        //public PortalSettings PortalSettings
        //{
        //    get
        //    {
        //        if (!(_portalSettings != null && _portalSettings.PortalId != Null.NullInteger))
        //        {
        //            PortalAliasInfo objPortalAliasInfo = new PortalAliasInfo();
        //            objPortalAliasInfo.PortalID = PortalId;
        //            _portalSettings = new PortalSettings(TabId, objPortalAliasInfo);
        //        }
        //        return _portalSettings;
        //    }
        //}



        private TabInfo _tabInfo;
        /// <summary>
        /// 页面信息
        /// </summary>
        public TabInfo TabInfo
        {
            get
            {
                if (!(_tabInfo != null && _tabInfo.TabID > 0) && TabId > 0)
                {
                    TabController tc = new TabController();
                    _tabInfo = tc.GetTab(TabId);

                }

                return _tabInfo;


            }
        }


        private ModuleInfo _ModuleConfiguration = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo ModuleConfiguration
        {
            get
            {
                if (!(_ModuleConfiguration != null && _ModuleConfiguration.ModuleID > 0) && ModuleId > 0)
                {
                    ModuleController mc = new ModuleController();
                    _ModuleConfiguration = mc.GetModule(ModuleId);//,TabId);

                }
                return _ModuleConfiguration;
            }
        }

        private String _BaseModuleName = String.Empty;
        /// <summary>
        /// 基础模块名
        /// </summary>
        public String BaseModuleName
        {
            get
            {
                if (String.IsNullOrEmpty(_BaseModuleName))
                {
                    _BaseModuleName = ModuleProperty("ModuleName");
                }
                return _BaseModuleName;
            }
            set { _BaseModuleName = value; }
        }

        /// <summary>
        /// 模块地址
        /// </summary>
        public string ModulePath
        {
            get { return this.TemplateSourceDirectory + "/"; }
        }


        private Hashtable _settings = new Hashtable();
        /// <summary>
        /// 模块设置
        /// </summary>
        public Hashtable Settings
        {
            get
            {
                ModuleController controller = new ModuleController();
                if (!(_settings != null && _settings.Count >0))
                {
                    _settings = new Hashtable(controller.GetModuleSettings(ModuleId));
                }
                return _settings;
            }
        }

        private Hashtable _ThemePlugin_Settings = new Hashtable();
        /// <summary>
        /// 博客主模块设置
        /// </summary>
        public Hashtable ThemePlugin_Settings
        {
            get
            {
                if (!(_ThemePlugin_Settings != null && _ThemePlugin_Settings.Count > 0))
                {
                    _ThemePlugin_Settings = Settings;
                }
                return _ThemePlugin_Settings;
            }
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"); }
        }

        /// <summary>
        /// 管理员锁
        /// (检索目录下有无admindisplay.lock)
        /// </summary>
        public Boolean AdministratorLock
        {
            get { return File.Exists(MapPath(String.Format("{0}admindisplay.lock", ModulePath))); }
        }
        /// <summary>
        /// 显示管理员选项
        /// </summary>
        public Boolean DisplayAdminOption
        {
            get
            {
                Boolean display = true;
                if (AdministratorLock && !IsAdministrator)
                {
                    display = false;
                }
                return display;
            }
        }

      
        /// <summary>
        /// 语言
        /// </summary>
        public String language
        {
            get { return WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage); }
        }



        /// <summary>
        /// 验证登陆状态(没有登陆跳转到登陆页面)
        /// </summary>
        public void VerificationLogin()
        {
            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)));

            }
        }

        /// <summary>
        /// 验证作者状态(不是作者跳转到登陆页面)
        /// </summary>
        public void VerificationAuthor()
        {
            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)));
            }
            else if ( !ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration))
            {
                Response.Redirect(Globals.NavigateURL(TabId));
            }



          
        }


        private String _SkinPath = String.Empty;
        /// <summary>
        /// 皮肤名称
        /// </summary>
        public String SkinPath
        {
            get
            {
                if (String.IsNullOrEmpty(_SkinPath))
                {
                    _SkinPath = PortalSettings.ActiveTab.SkinPath;
                    String __SkinPath = WebHelper.GetStringParam(HttpContext.Current.  Request, "SkinPath", "");
                    if (!String.IsNullOrEmpty(__SkinPath))
                    {
                        _SkinPath = __SkinPath;
                    }

                }
                return _SkinPath;
            }
        }





        private String _SkinName = String.Empty;
        /// <summary>
        /// 皮肤名称
        /// </summary>
        public String SkinName
        {
            get
            {
                if (String.IsNullOrEmpty(_SkinName))
                {
                    DirectoryInfo dir = new DirectoryInfo(MapPath(SkinPath));
                    if (dir.Exists && !String.IsNullOrEmpty(dir.Name))
                    {
                        _SkinName = dir.Name;
                    }
                    
                }
                return _SkinName;
            }
        }

        private String _SkinFileName = String.Empty;
        /// <summary>
        /// 皮肤文件名
        /// </summary>
        public String SkinFileName
        {
            get
            {
                if (String.IsNullOrEmpty(_SkinFileName))
                {
                    String __SkinFileName = WebHelper.GetStringParam(HttpContext.Current.Request, "SkinFileName", "");
                    if (!String.IsNullOrEmpty(__SkinFileName))
                    {
                        _SkinFileName = __SkinFileName;
                    }
                    else
                    {
                        var file = new System.IO.FileInfo(MapPath(PortalSettings.ActiveTab.SkinSrc));
                        if (file.Exists && !String.IsNullOrEmpty(file.Name))
                        {
                            _SkinFileName = file.Name.Replace(String.Format("{0}", file.Extension), ""); ;
                        }
                    }
                }
                return _SkinFileName;
            }
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
                    var ModuleVersion = ModuleProperty("Version");
                    string setting = GetHostSetting("CrmVersion");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        _CrmVersion = String.Format("{0}.{1}", ModuleVersion, setting);
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

        #region "====参数设置属性===="

        /// <summary>
        /// 配置XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        public String ItemSettingXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Options.xml", SkinPath, SkinFileName)); }
        }
        /// <summary>
        /// 储值XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        public String ItemValueXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Storage.xml", SkinPath, SkinFileName)); }
        }

        /// <summary>
        /// 配置XML路径(全局)
        /// </summary>
        public String GlobalSettingXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Options.xml", SkinPath, "Global")); }
        }
        /// <summary>
        /// 储值XML路径(全局)
        /// </summary>
        public String GlobalValueXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Storage.xml", SkinPath, "Global")); }
        }


        /// <summary>
        /// 配置XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        /// <param name="_SkinFileName"></param>
        /// <returns></returns>
        public String _ItemSettingXmlPath(String _SkinFileName)
        {
            return MapPath(String.Format("{0}xTemplate/{1}.Options.xml", SkinPath, _SkinFileName));
        }
        /// <summary>
        /// 储值XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        /// <param name="_SkinFileName"></param>
        /// <returns></returns>
        public String _ItemValueXmlPath(String _SkinFileName)
        {
            return MapPath(String.Format("{0}xTemplate/{1}.Storage.xml", SkinPath, _SkinFileName));
        }
        /// <summary>
        /// 参数列表
        /// </summary>
        /// <param name="_SkinFileName"></param>
        /// <returns></returns>
        public List<SettingEntity> _OptionList(String _SkinFileName)
        {
            XmlFormat xf = new XmlFormat(_ItemSettingXmlPath(_SkinFileName));
            return xf.ToList<SettingEntity>();
        }

        /// <summary>
        /// 存储的键值列表
        /// </summary>
        /// <param name="_SkinFileName"></param>
        /// <returns></returns>
        public List<KeyValueEntity> _ItemValues(String _SkinFileName)
        {
            List<KeyValueEntity> values = new List<KeyValueEntity>();
            if (File.Exists(_ItemValueXmlPath(_SkinFileName)))
            {
                using (StreamReader sr = new StreamReader(_ItemValueXmlPath(_SkinFileName), System.Text.Encoding.UTF8))
                {

                    values = ConvertTo.Deserialize<List<KeyValueEntity>>(sr.ReadToEnd());
                    sr.Close();
                    sr.Dispose();

                }
            }
            return values;
        }


        private List<SettingEntity> __OptionList = new List<SettingEntity>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SettingEntity> OptionList
        {
            get
            {
                if (!(__OptionList != null && __OptionList.Count > 0))
                {
                    XmlFormat xf = new XmlFormat(ItemSettingXmlPath);
                    __OptionList = xf.ToList<SettingEntity>();
                }
                return __OptionList;
            }
        }

        private List<KeyValueEntity> __ItemValues = new List<KeyValueEntity>();
        /// <summary>
        /// 存储的键值列表
        /// </summary>
        public List<KeyValueEntity> ItemValues
        {
            get
            {
                if (!(__ItemValues != null && __ItemValues.Count > 0))
                {
                    if (File.Exists(ItemValueXmlPath))
                    {
                        using (StreamReader sr = new StreamReader(ItemValueXmlPath, System.Text.Encoding.UTF8))
                        {

                            __ItemValues = ConvertTo.Deserialize<List<KeyValueEntity>>(sr.ReadToEnd());
                            sr.Close();
                            sr.Dispose();

                        }
                    }

                }
                return __ItemValues;
            }
        }



        private List<SettingEntity> __GlobalOptionList = new List<SettingEntity>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SettingEntity> GlobalOptionList
        {
            get
            {
                if (!(__GlobalOptionList != null && __GlobalOptionList.Count > 0))
                {
                    XmlFormat xf = new XmlFormat(GlobalSettingXmlPath);
                    __GlobalOptionList = xf.ToList<SettingEntity>();
                }
                return __GlobalOptionList;
            }
        }

        private List<KeyValueEntity> __GlobalValues = new List<KeyValueEntity>();
        /// <summary>
        /// 存储的键值列表
        /// </summary>
        public List<KeyValueEntity> GlobalValues
        {
            get
            {
                if (!(__GlobalValues != null && __GlobalValues.Count > 0))
                {
                    if (File.Exists(GlobalValueXmlPath))
                    {
                        using (StreamReader sr = new StreamReader(GlobalValueXmlPath, System.Text.Encoding.UTF8))
                        {

                            __GlobalValues = ConvertTo.Deserialize<List<KeyValueEntity>>(sr.ReadToEnd());
                            sr.Close();
                            sr.Dispose();

                        }
                    }

                }
                return __GlobalValues;
            }
            set { __GlobalValues = value; }
        }



 

        #endregion

        #region "SEO配置属性"

        /// <summary>
        /// SEO URL参数,可以设置ArticleID
        /// </summary>
        public String Settings_Seo_UrlParameterName
        {
            get { return ThemePlugin_Settings["ThemePlugin_Seo_UrlParameterName"] != null && !string.IsNullOrEmpty(ThemePlugin_Settings["ThemePlugin_Seo_UrlParameterName"].ToString()) ? Convert.ToString(ThemePlugin_Settings["ThemePlugin_Seo_UrlParameterName"]) : "ArticleID"; }
        }


        /// <summary>
        /// SEO URL参数,FriendlyUrl 开关
        /// </summary>
        public EnumYesNo Settings_Seo_FriendlyUrl
        {
            get { return ThemePlugin_Settings["ThemePlugin_FriendlyUrl"] != null && !string.IsNullOrEmpty(ThemePlugin_Settings["ThemePlugin_FriendlyUrl"].ToString()) ? (EnumYesNo)Convert.ToInt32(ThemePlugin_Settings["ThemePlugin_FriendlyUrl"]) : EnumYesNo.Yes; }
        }

        #endregion

        #region "绑定页面标题和帮助"

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewTitle(Title, DefaultValue, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName)
        {
            return ViewTitle(Title, DefaultValue, ControlName, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
            return ViewSpan(Content, ControlName, ClassName);
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue)
        {
            String Content = ViewResourceText(Title, DefaultValue, "Help");
            return ViewSpan(Content, "", "span_help");
        }

        /// <summary>
        /// 显示内容框
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ControlName"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewSpan(String Content, String ControlName, String ClassName)
        {
            if (!String.IsNullOrEmpty(ControlName))
            {
                System.Web.UI.Control c = FindControl(ControlName);
                if (c != null && !String.IsNullOrEmpty(c.ClientID))
                {
                    ControlName = c.ClientID;
                }
                else
                {
                    ControlName = String.Empty;
                }
            }

            return String.Format("<label  {2}><span {1} >{0}</span></label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                );
        }




        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title)
        {
            return ViewResourceText(Title, "");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue, "Text");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="TextType"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue, String TextType)
        {
            String _Title = Localization.GetString(String.Format("{0}.{1}", Title, TextType), this.LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }




        /// <summary>
        /// 计算页面执行的时间
        /// </summary>
        /// <param name="TimeStart">开始时间</param>
        public String InitTimeSpan(DateTime TimeStart)
        {
            //查询数据库所花的时间
            System.DateTime endTime = DateTime.Now;
            System.TimeSpan ts = endTime - TimeStart;
            String RunTime = string.Format("{0}秒{1}毫秒", ts.Seconds, ts.Milliseconds);
            TimeStart = endTime = DateTime.Now;
            return RunTime;
        }






        /// <summary>
        /// 显示字段标题
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ClassName"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitleSpan(String Content, String ClassName, String ControlName)
        {

            return String.Format("<label  {2}><span {1} >{0}</span></label>",
                        Content,
                        !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
                      !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                        );
        }
        #endregion


        #region "新的后台URL"

        /// <summary>
        /// URL转换默认名
        /// </summary>
        /// <returns></returns>
        public String xUrlToken()
        {
            return "Pages";
        }


        public string xUrl()
        {
            return xUrl("", "", xUrlToken());
        }
        public string xUrl(string ControlKey)
        {
            return xUrl("", "", ControlKey);
        }
        public string xUrl(string KeyName, string KeyValue)
        {
            return xUrl(KeyName, KeyValue, xUrlToken());
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey)
        {
            string[] parameters = new string[] { };
            return xUrl(KeyName, KeyValue, ControlKey, parameters);
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey, params string[] AddParameters)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);

            sb.AppendFormat("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", ModulePath, PortalId, TabId, ModuleId, language);

            string key = ControlKey;
            if (string.IsNullOrEmpty(key))
            {
                sb.AppendFormat("&Token={0}", xUrlToken());
            }
            else
            {
                sb.AppendFormat("&Token={0}", key);
            }
            if (!string.IsNullOrEmpty(KeyName) && !string.IsNullOrEmpty(KeyValue))
            {
                sb.AppendFormat("&{0}={1}", KeyName, KeyValue);
            }

            if (AddParameters != null && AddParameters.Length > 0)
            {
                foreach (String parameter in AddParameters)
                {
                    sb.AppendFormat("&{0}", parameter);
                }
            }
            return sb.ToString();

        }





        #endregion

        #region "验证脚本多语言"
        /// <summary>
        /// 获取当前验证引擎语言文件的URL
        /// </summary>
        /// <returns></returns>
        public String ViewValidationEngineLanguage()
        {
            String VEL = String.Format("{0}Resource/js/jquery.validationEngine-en.js?cdv={1}", ModulePath, CrmVersion);
            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage).ToLower(); ;
            if (!String.IsNullOrEmpty(language) && language != "en-us")
            {
                //先判断这个语言文件是否存在
                String webJS = String.Format("{0}Resource/plugins/validation/jquery.validationEngine-{1}.js", ModulePath, language);
                String serverJS = MapPath(webJS);
                if (File.Exists(serverJS))
                {
                    VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                }
                else if (language.IndexOf("-") >= 0)
                {
                    String lTemp = language.Remove(language.IndexOf("-"));
                    webJS = String.Format("{0}Resource/plugins/validation/jquery.validationEngine-{1}.js", ModulePath, lTemp);
                    serverJS = MapPath(webJS);
                    if (File.Exists(serverJS))
                    {
                        VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                    }
                }
            }
            return VEL;
        }

        #endregion

        #region "获取文件后缀名和路径"

        /// <summary>
        /// 根据后缀名显示图标
        /// </summary>
        /// <param name="FileExtension">文件后缀</param>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public String GetPhotoExtension(String FileExtension, String FilePath)
        {
            FileExtension = FileExtension.ToLower();

            //先判断是否是图片格式的
            if (FileExtension == "jpg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "png")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "jpeg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "gif")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "bmp")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "mp4")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "ogv")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "webm")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "mp3")
                return GetFileIcon("audio.jpg");
            else if (FileExtension == "wma")
                return GetFileIcon("audio.jpg");
            else if (FileExtension == "zip")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "rar")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "7z")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "xls")
                return GetFileIcon("Document.jpg");
            else if (FileExtension == "txt")
                return GetFileIcon("text.jpg");
            else if (FileExtension == "cs")
                return GetFileIcon("code.jpg");
            else if (FileExtension == "html")
                return GetFileIcon("code.jpg");
            else if (FileExtension == "pdf")
                return GetFileIcon("pdf.jpg");
            else if (FileExtension == "doc")
                return GetFileIcon("Document.jpg");
            else if (FileExtension == "docx")
                return GetFileIcon("Document.jpg");
            else
                return GetFileIcon("Unknown type.jpg");
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
            return String.Format("{0}Resource/images/crystal/{1}", ModulePath, IconName);
        }

        #endregion

        #region "验证用户有无模块权限"
        /// <summary>
        /// 验证用户有无模块权限
        /// </summary>
        /// <param name="AccessLevel"></param>
        /// <param name="permissionKey"></param>
        /// <param name="__ModuleConfiguration"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static bool HasModuleAccess(SecurityAccessLevel AccessLevel, string permissionKey, ModuleInfo __ModuleConfiguration, UserInfo objUser)
        {
            bool blnAuthorized = false;
            if (objUser != null && objUser.IsSuperUser)
            {
                blnAuthorized = true;
            }
            else
            {
                switch (AccessLevel)
                {
                    case SecurityAccessLevel.Anonymous:
                        blnAuthorized = true;
                        break;
                    case SecurityAccessLevel.View:
                        if (TabPermissionController.CanViewPage() || ModulePermissionController.CanViewModule(__ModuleConfiguration))
                        {
                            blnAuthorized = true;
                        }
                        break;
                    case SecurityAccessLevel.Edit:
                        if (TabPermissionController.CanAddContentToPage())
                        {
                            blnAuthorized = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(permissionKey))
                            {
                                permissionKey = "CONTENT,DELETE,EDIT,EXPORT,IMPORT,MANAGE";
                            }
                            if (__ModuleConfiguration != null && ModulePermissionController.CanViewModule(__ModuleConfiguration) && (ModulePermissionController.HasModulePermission(__ModuleConfiguration.ModulePermissions, permissionKey) || ModulePermissionController.HasModulePermission(__ModuleConfiguration.ModulePermissions, "EDIT")))
                            {
                                blnAuthorized = true;
                            }
                        }
                        break;
                    case SecurityAccessLevel.Admin:
                        if (TabPermissionController.CanAddContentToPage())
                        {
                            blnAuthorized = true;
                        }
                        break;
                    case SecurityAccessLevel.Host:
                        break;
                }
            }
            return blnAuthorized;
        }
        #endregion


        #region "载入模块"
        /// <summary>
        /// 载入模块
        /// </summary>
        /// <param name="ModuleSrc"></param>
        /// <param name="phContainer"></param>
        public void LoadModule(String ModuleSrc,ref PlaceHolder phContainer)
        {
            BaseModule ManageContent = new BaseModule();
            ManageContent.ID = ModuleSrc.Replace(".ascx", "");
            String ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, ModuleSrc));
            ManageContent = (BaseModule)LoadControl(ContentSrc);
            ManageContent.ModuleConfiguration = this.ModuleConfiguration;
            ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", ModuleSrc));
            phContainer.Controls.Add(ManageContent);
        }
        #endregion

        #region "更新模块设置"


        /// <summary>
        /// 更新当前模块的设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(string SettingName, string SettingValue)
        {
            UpdateModuleSetting(ModuleId, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(int ModuleId, string SettingName, string SettingValue)
        {
            ModuleController controller = new ModuleController();

            controller.UpdateModuleSetting(ModuleId, SettingName, SettingValue);

        }

        /// <summary>
        /// 主题参数保存名称格式化
        /// </summary>
        /// <param name="ThemeFile"></param>
        /// <param name="ThemeName"></param>
        /// <param name="ThemeID"></param>
        /// <returns></returns>
        public String ThemeXmlSettingsFormat(String ThemeFile, String ThemeName, Int32 ThemeID)
        {
            return String.Format("ThemePluginViews_{0}_{1}_{2}", ThemeID, ThemeName, ThemeFile);
        }


        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="EffectName">效果名</param>
        /// <param name="ThemeName">主题名</param>
        /// <returns></returns>
        public String SettingsFormat(String EffectName, String ThemeName)
        {
            return String.Format("ThemePlugin{0}_{1}", EffectName, ThemeName);
        }





        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="ValueList"></param>
        /// <param name="Name"></param>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public List<KeyValueEntity> UpdateSettings(List<KeyValueEntity> ValueList, String Name, String StrValue)
        {
            if (ValueList.Exists(r => r.Key == Name))
            {
                ValueList[ValueList.FindIndex(r => r.Key == Name)].Value = StrValue;
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

        #region "显示图片的URL"



        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue)
        {
            return ViewLinkUrl(UrlValue, true);
        }

        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <param name="IsPhotoExtension">是否显示扩展名图片</param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, Boolean IsPhotoExtension)
        {
            String DefaultValue = String.Empty;
            if (!String.IsNullOrEmpty(UrlValue) && UrlValue != "0")
            {
                if (UrlValue.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int FileID = 0;
                    if (int.TryParse(UrlValue.Replace("FileID=", ""), out FileID) && FileID > 0)
                    {

                        var fi = FileManager.Instance.GetFile(FileID);
                        if (fi != null && fi.FileId > 0)
                        {
                            DefaultValue = string.Format("{0}{1}{2}", PortalSettings.HomeDirectory, fi.Folder, Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = String.Format("{0}Resource/images/no_image.png", ModulePath);

                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_ThemePlugin_Multimedia Multimedia = DNNGo_ThemePlugin_Multimedia.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            if (IsPhotoExtension)
                            {
                                DefaultValue = Server.UrlPathEncode(GetPhotoExtension(Multimedia.FileExtension, Multimedia.FilePath));// String.Format("{0}{1}", bpm.DNNGalleryPro_PortalSettings.HomeDirectory, Multimedia.FilePath);
                            }
                            else
                            {
                                DefaultValue = Server.UrlPathEncode(GetPhotoPath(Multimedia.FilePath));
                            }
                        }
                    }
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {

                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")), false, PortalSettings, Null.NullString, "", "");

                }
                else
                {
                    DefaultValue = UrlValue;
                }
            }
            return DefaultValue;

        }
        #endregion

        #region "Page_Init 权限验证"
        /// <summary>
        /// 关于权限验证
        /// </summary>
        protected virtual void Page_Init(System.Object sender, System.EventArgs e)
        {

            if (!String.IsNullOrEmpty(BaseModuleName))
            {
                //如果不是此模块,则会抛出异常,提示非法入侵
                if (!(("DNNGo.ThemePlugin").IndexOf(BaseModuleName, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    Response.Redirect(Globals.NavigateURL(TabId), true);
                }
            }

            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)), true);
            }
            else if (!ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration))
            {
                Response.Redirect(Globals.NavigateURL(TabId), true);
            }



        }
        #endregion


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
            return m.GetProperty(Name, "", null, UserInfo, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
        }

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(String Name)
        {
            return ModuleProperty(ModuleConfiguration, Name);
        }


        #endregion
    }
}