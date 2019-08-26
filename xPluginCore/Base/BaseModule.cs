using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;

using DotNetNuke.Entities.Tabs;
using System.IO;


using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using System.Globalization;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public class BaseModule : PortalModuleBase
    {
        #region "基本属性"

 

        /// <summary>
        /// 提示操作类
        /// </summary>
        public MessageTips mTips = new MessageTips();
        /// <summary>
        /// 页面操作类
        /// </summary>
        public TabController objTabs = new TabController();




        /// <summary>
        /// 为当前 Web 请求获取与服务器控件关联的 System.Web.HttpContext 对象
        /// </summary>
        public HttpContext ModuleContext
        {
            get { return Context; }
        }





        /// <summary>
        /// 是否管理员
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
                    

        #region "有关效果的设置"

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
            return HostController.Instance.GetString(key, defaultValue); ;
        }
        #endregion



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



        /// <summary>
        /// 作用范围
        /// </summary>
        public EnumScope Settings_Scope
        {
            get { return GetPortalSetting("ThemePlugin_Scope", ((Int32)EnumScope.Single).ToString()) == ((Int32)EnumScope.Global).ToString() ? EnumScope.Global : EnumScope.Single; }
        }

        #endregion

        #region "字体方案属性"

        private List<FontDB> _FontDBs;
        /// <summary>
        /// 所有字体方案
        /// </summary>
        public List<FontDB> FontDBs
        {
            get {
                if (!(_FontDBs != null && _FontDBs.Count > 0))
                {
                    _FontDBs = new FontDBHelper(this).FindAll().FindAll(r=>r.Enable);

                    _FontDBs.Sort(delegate (FontDB x, FontDB y)
                    {
                        if (!x.IsFontLink.HasValue || !y.IsFontLink.HasValue) return 0;
                        else return y.IsFontLink.Value.CompareTo(x.IsFontLink.Value);
                    });

                }
                return _FontDBs; }
            set { _FontDBs = value; }

        }


        #endregion


        #region "获取DNN对象"



        /// <summary>
        /// 默认的菜单
        /// </summary>
        public String Token = WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "Options");



 


        private String _SkinPath = String.Empty;
        /// <summary>
        /// 皮肤路径
        /// </summary>
        public String SkinPath
        {
            get
            {
                if (String.IsNullOrEmpty(_SkinPath))
                {

                    _SkinPath = PortalSettings. ActiveTab.SkinPath;
                    String __SkinPath = WebHelper.GetStringParam(HttpContext.Current.Request, "SkinPath", "");
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
                        FileInfo file = new FileInfo(MapPath(PortalSettings.ActiveTab.SkinSrc));
                        if (file.Exists && !String.IsNullOrEmpty(file.Name))
                        {
                            _SkinFileName = file.Name.Replace(String.Format("{0}", file.Extension), "");
                        }
                    }
                }
                return _SkinFileName;
            }
        }

 

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
 

        /// <summary>
        /// 演示锁(true为demo状态)
        /// (检索目录下有无demo.lock)
        /// </summary>
        public Boolean DemoLock
        {
            get { return File.Exists(MapPath(String.Format("{0}demo.lock", ModulePath))); }
        }

        #endregion

        #region "DNN公用对象调用"

 

        /// <summary>
        /// 更新引用文件版本
        /// </summary>
        public void IncrementCrmVersion()
        {
            var currentVersion = Convert.ToInt32(GetHostSetting("CrmVersion"));
            var newVersion = currentVersion + 1;
            DotNetNuke.Data.DataProvider.Instance().UpdateHostSetting("CrmVersion", newVersion.ToString(CultureInfo.InvariantCulture), false, UserId);
        }


        /// <summary>
        /// 更新站点配置
        /// </summary>
        public void UpdatePortalSetting(string SettingName, string SettingValue)
        {

            PortalController.UpdatePortalSetting(PortalSettings.PortalId, SettingName, SettingValue);
        }

        /// <summary>
        /// 获取站点配置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public String GetPortalSetting(string SettingName, string defaultValue)
        {
            return DotNetNuke.Entities.Portals.PortalController.GetPortalSetting(SettingName, PortalSettings.PortalId, defaultValue);
        }




        #endregion


        #region "操纵读写模版文件"

        /// <summary>
        /// 写入文件到文件
        /// </summary>
        /// <param name="WriteFilePath">文件路径</param>
        /// <param name="ContentText">文件内容</param>
        public void WriteTextToFile(String WriteFilePath, String ContentText)
        {
            using (StreamWriter sw = new StreamWriter(WriteFilePath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(ContentText);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 写入模版文件
        /// </summary>
        /// <param name="WriteFileName">要写入的文件名</param>
        /// <param name="ContentText">写入的内容</param>
        public void WriteTemplate(String WriteFileName, String ContentText)
        {
            String filePath = MapPath(String.Format("{0}{1}", SkinPath, WriteFileName));

            WriteTextToFile(filePath, ContentText);
        }



        #endregion


        #region "绑定模版文件"


        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="ThemeFile"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(String ThemeFile, Hashtable Puts)
        {
            TemplateFormat xf = new TemplateFormat(this);
            return ViewTemplate(ThemeFile, Puts, xf);
        }

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="xf"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(String ThemeFile, Hashtable Puts, TemplateFormat xf)
        {
            VelocityHelper vltContext = new VelocityHelper(this);


            vltContext.Put("xf", xf);//模版格式化共用方法
            vltContext.Put("PortalId", PortalId);//绑定的主模块编号
            vltContext.Put("TabID", TabId);//绑定的主模块页面编号

            if (Puts != null && Puts.Count > 0)
            {
                foreach (String key in Puts.Keys)
                {
                    vltContext.Put(key, Puts[key]);
                }
            }
            return vltContext.Display(ThemeFile);
        }





        #endregion








        #region "jQuery配置属性"

        /// <summary>
        /// 开始模块jQuery
        /// </summary>
        public Boolean Settings_jQuery_Enable
        {
            get { return Settings["ThemePlugin_jQuery_Enable"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_Enable"].ToString()) ? Convert.ToBoolean(Settings["ThemePlugin_jQuery_Enable"]) : false; }
        }

        /// <summary>
        /// 使用jQuery库
        /// </summary>
        public Boolean Settings_jQuery_UseHosted
        {
            get { return Settings["ThemePlugin_jQuery_UseHosted"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_UseHosted"].ToString()) ? Convert.ToBoolean(Settings["ThemePlugin_jQuery_UseHosted"]) : false; }
        }

        /// <summary>
        /// jQuery库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQuery
        {
            get { return Settings["ThemePlugin_jQuery_HostedjQuery"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_HostedjQuery"].ToString()) ? Convert.ToString(Settings["ThemePlugin_jQuery_HostedjQuery"]) : "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"; }
        }

        /// <summary>
        /// jQueryUI库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQueryUI
        {
            get { return Settings["ThemePlugin_jQuery_HostedjQueryUI"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_HostedjQueryUI"].ToString()) ? Convert.ToString(Settings["ThemePlugin_jQuery_HostedjQueryUI"]) : "https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"; }
        }


        #endregion

        #region "加载样式表"


        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindStyleFile(String ThemeName, String ThemePath)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                string ItemKey = String.Format("DNNGo_ThemePlugin_Theme_{0}_css", ThemeName);
                if (HttpContext.Current.Items[ItemKey] == null)
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", ThemePath);

                    HttpContext.Current.Items.Add(ItemKey, "true");
                    objCSS.Controls.Add(litLink);
                }
            }
        }

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindStyleFile(String ThemeName)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                string ItemKey = String.Format("DNNGo_ThemePlugin_Theme_{0}_css", ThemeName);
                if (HttpContext.Current.Items[ItemKey] == null)
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}Resource/Themes/{1}/style.css\" />", ModulePath, ThemeName);

                    HttpContext.Current.Items.Add(ItemKey, "true");
                    objCSS.Controls.Add(litLink);
                }
            }
        }

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String ThemeName)
        {
            
                string ItemKey = String.Format("DNNGo_ThemePlugin_Theme_{0}_js", ThemeName);
                if (HttpContext.Current.Items[ItemKey] == null)
                {
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(this.Page, ResolveUrl(String.Format("{0}Resource/Themes/{1}/common.js?cdv={2}",ModulePath, ThemeName, CrmVersion)));
                }

 
           
        }



        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String Name,String FileName)
        {
          
                string ItemKey = String.Format("DNNGo_ThemePlugin_Theme_Custom_{0}_js", Name);
                if (HttpContext.Current.Items[ItemKey] == null)
                {
 
                    HttpContext.Current.Items.Add(ItemKey, "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(this.Page, FileName);

                }


           
        }

     
        #endregion


        #region "GlobalTab初始化"
        /// <summary>
        /// GlobalTab初始化
        /// </summary>
        /// <returns></returns>
        public TabInfo InitGlobalTab()
        {
            TabInfo GlobalTab = new TabInfo();
            GlobalTab.TabID = int.MaxValue;
            GlobalTab.TabName = "Globals";
            GlobalTab.Title = "Apply to all pages";

            return GlobalTab;
        }



        #endregion

      


        #region "加载界面脚本样式表"







        /// <summary>
        /// 加载显示界面脚本样式表
        /// </summary>
        public void LoadViewScript()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                LoadScriptForJqueryAndUI(ModulePath);

 


                //if (HttpContext.Current.Items["jquery-ui-CSS"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/jquery-ui-1.7.custom.css\" />";

                //    HttpContext.Current.Items.Add("jquery-ui-CSS", "true");
                //    objCSS.Controls.Add(litLink);
                //}



                //if (HttpContext.Current.Items["DNNGo_ThemePlugin_Modules_css"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/Modules.css\" />";

                //    HttpContext.Current.Items.Add("DNNGo_ThemePlugin_Modules_css", "true");
                //    objCSS.Controls.Add(litLink);
                //}


                if (HttpContext.Current.Items["jquery.validationEngine-en.js"] == null)
                {

                    HttpContext.Current.Items.Add("jquery.validationEngine-en.js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    //Page.ClientScript.RegisterClientScriptInclude("jquery.validationEngine-en.js", String.Format("{0}Resource/js/jquery.validationEngine-en.js", ModulePath));
                    ClientResourceManager.RegisterScript(this.Page, String.Format("{0}Resource/js/jquery.validationEngine-en.js", ModulePath));
                }


                if (HttpContext.Current.Items["jquery.validationEngine.js"] == null)
                {

                    HttpContext.Current.Items.Add("jquery.validationEngine.js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(this.Page, String.Format("{0}Resource/js/jquery.validationEngine.js", ModulePath));
                }


                //if (HttpContext.Current.Items["Vanadium_js"] == null)
                //{

                //    HttpContext.Current.Items.Add("Vanadium_js", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("Vanadium_js", String.Format("{0}Resource/js/vanadium.js", ModulePath));
 
                //}

 


                //if (HttpContext.Current.Items["jquery.category.js"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text =
                //         Microsoft.VisualBasic.Constants.vbCrLf + "<script type=\"text/javascript\" src=\"" + ModulePath + "Resource/js/jquery.category.js\"></script>" +
                //        Microsoft.VisualBasic.Constants.vbCrLf;
                //    HttpContext.Current.Items.Add("jquery.category.js", "true");
                //    objCSS.Controls.Add(litLink);
                //}


                //if (HttpContext.Current.Items["DNNGo.Common.js"] == null)
                //{

                //    HttpContext.Current.Items.Add("DNNGo.Common.js", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("DNNGo.Common.js", String.Format("{0}Resource/js/DNNGo.Common.js", ModulePath));
                //}
            }
        }


        /// <summary>
        /// 加载管理界面脚本样式表
        /// </summary>
        public void LoadManagerScript()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                LoadScriptForJqueryAndUI(ModulePath);

                if (HttpContext.Current.Items["thickbox_CSS"] == null)
                {
                    Literal litLink = new Literal();
                    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/thickbox.css\" />";

                    HttpContext.Current.Items.Add("thickbox_CSS", "true");
                    objCSS.Controls.Add(litLink);
                }

                if (HttpContext.Current.Items["thickbox_js"] == null)
                {
 
                    HttpContext.Current.Items.Add("thickbox_js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(this.Page, String.Format("{0}Resource/js/thickbox.js", ModulePath));
                }

     
         

         

                //if (HttpContext.Current.Items["jquery-ui-timepicker-addon"] == null)
                //{
    
                //    HttpContext.Current.Items.Add("jquery-ui-timepicker-addon", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("jquery-ui-timepicker-addon", String.Format("{0}Resource/js/jquery-ui-timepicker-addon.js", ModulePath));
                //}



                if (HttpContext.Current.Items["jquery-ui-CSS"] == null)
                {
                    Literal litLink = new Literal();
                    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/jquery-ui-1.7.custom.css\" />";

                    HttpContext.Current.Items.Add("jquery-ui-CSS", "true");
                    objCSS.Controls.Add(litLink);
                }

 


                //if (HttpContext.Current.Items["ManagementCenterCSS"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/ManagementCenter.css\" />";

                //    HttpContext.Current.Items.Add("ManagementCenterCSS", "true");
                //    objCSS.Controls.Add(litLink);
                //}

                if (HttpContext.Current.Items["DNNGo_ThemePlugin_Modules_css"] == null)
                {
                    Literal litLink = new Literal();
                    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/Modules.css\" />";

                    HttpContext.Current.Items.Add("DNNGo_ThemePlugin_Modules_css", "true");
                    objCSS.Controls.Add(litLink);
                }



                if (HttpContext.Current.Items["jquery.validationEngine-en_js"] == null)
                {
 
                    HttpContext.Current.Items.Add("jquery.validationEngine-en_js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(Page, String.Format("{0}Resource/js/jquery.validationEngine-en.js", ModulePath));
                }


                if (HttpContext.Current.Items["validationEngine_js"] == null)
                {
 
                    HttpContext.Current.Items.Add("validationEngine_js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(Page, String.Format("{0}Resource/js/jquery.validationEngine.js", ModulePath));
                }

                

                if (HttpContext.Current.Items["Vanadium_js"] == null)
                {
 
                    HttpContext.Current.Items.Add("Vanadium_js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(Page, String.Format("{0}Resource/js/vanadium.js", ModulePath));
                }

                if (HttpContext.Current.Items["jquery.autoGrowInput.js"] == null)
                {
                    HttpContext.Current.Items.Add("jquery.autoGrowInput.js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(Page, String.Format("{0}Resource/js/jquery.autoGrowInput.js", ModulePath));
                }

                if (HttpContext.Current.Items["jquery.tagedit.js"] == null)
                {
 
                    HttpContext.Current.Items.Add("jquery.tagedit.js", "true");
                    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                    ClientResourceManager.RegisterScript(Page, String.Format("{0}Resource/js/jquery.tagedit.js", ModulePath));
                }
                

                //if (HttpContext.Current.Items["DNNGo.Common.js"] == null)
                //{
                //    HttpContext.Current.Items.Add("DNNGo.Common.js", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("DNNGo.Common.js", String.Format("{0}Resource/js/DNNGo.Common.js", ModulePath));
                //}
            }
        }


        /// <summary>
        /// 加载脚本
        /// </summary>
        public void LoadScriptForJqueryAndUI(string modulePath)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                String jQueryUrl = String.Format("{0}Resource/js/jquery.min.js?cdv={1}", ModulePath, CrmVersion);
                String jQueryUIUrl = String.Format("{0}Resource/js/jquery-ui.min.js?cdv={1}", ModulePath, CrmVersion);
                if (Settings_jQuery_UseHosted)//使用指定的jQuery库的地址
                {
                    jQueryUrl = Settings_jQuery_HostedjQuery;
                    jQueryUIUrl = Settings_jQuery_HostedjQueryUI;
                }





                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jQueryUIRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQueryUI")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUIUrl);
                    //if (!Settings_jQuery_Enable)
                    //{

                    if (!HttpContext.Current.Items.Contains("jQueryUIRequested")) HttpContext.Current.Items.Add("jQueryUIRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQueryUI")) HttpContext.Current.Items.Add("DNNGo_jQueryUI", "true");
                    objCSS.Controls.AddAt(0, litLink);
                }

                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jquery_registered") && !HttpContext.Current.Items.Contains("jQueryRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQuery")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUrl);
                    //if (!Settings_jQuery_Enable)
                    //{
                    if (!HttpContext.Current.Items.Contains("jquery_registered")) HttpContext.Current.Items.Add("jquery_registered", "true");
                    if (!HttpContext.Current.Items.Contains("jQueryRequested")) HttpContext.Current.Items.Add("jQueryRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQuery")) HttpContext.Current.Items.Add("DNNGo_jQuery", "true");

                    objCSS.Controls.AddAt(0, litLink);
                }
            }
        }

        #endregion

        #region "加载XML配置文件中的脚本与样式表"
        /// <summary>
        /// XmlDB
        /// </summary>
        /// <param name="XmlDB">配置文件</param>
        /// <param name="XmlName">效果/皮肤</param>
        public void BindXmlDBToPage(XmlDBEntity XmlDB, String XmlName)
        {
            //绑定全局附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.GlobalScript))
            {
                List<String> GlobalScripts = WebHelper.GetList(XmlDB.GlobalScript);

                foreach (String Script in GlobalScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/css/{1}", ModulePath, Script);
                            BindStyleFile(Script.Replace(".css", ""), FullFileName);
                        }
                        else //if (Script.IndexOf(".js", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/js/{1}", ModulePath, Script);
                            BindJavaScriptFile(Script.Replace(".js", ""), FullFileName);
                        }
                    }
                }
            }
            //绑定效果附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.EffectScript))
            {
                List<String> EffectScripts = WebHelper.GetList(XmlDB.EffectScript);

                foreach (String Script in EffectScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/css/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindStyleFile(Script.Replace(".css", ""), FullFileName);
                        }
                        else
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/js/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindJavaScriptFile(Script.Replace(".js", ""), FullFileName);
                        }
                    }
                }
            }
        }




        #endregion

        #region "加载提示语句"

        /// <summary>
        /// 显示未绑定模版的语句
        /// </summary>
        /// <returns></returns>
        public String ViewNoTemplate()
        {
            String NoTemplate = Localization.GetString("NoTemplate.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));


            return NoTemplate + ViewThemeGoUrl();
        }
        /// <summary>
        /// 显示未绑定主题时的跳转链接
        /// </summary>
        /// <returns></returns>
        public String ViewThemeGoUrl()
        {
            String ThemeGoUrl = String.Empty;
            //有编辑权限的时候，显示跳转到模版加载页
            if (IsAdministrator)
            {
                ThemeGoUrl = Localization.GetString("ThemeGoUrl.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
                ThemeGoUrl = ThemeGoUrl.Replace("[ThemeUrl]", xUrl("Skin_Skin"));
            }
            return ThemeGoUrl;
        }

        /// <summary>
        /// 未设置模块的绑定
        /// </summary>
        /// <returns></returns>
        public String ViewNoSettingBind()
        {
            return Localization.GetString("NoModuleSetting.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
        }



        /// <summary>
        /// 显示列表无数据的提示
        /// </summary>
        /// <returns></returns>
        public String ViewGridViewEmpty()
        {
            return Localization.GetString("GridViewEmpty.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
        }


        /// <summary>
        /// 绑定GridView的空信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gvList"></param>
        public void BindGridViewEmpty<T>(GridView gvList)
             where T : new()
        {
            BindGridViewEmpty<T>(gvList, new T());
        }

        /// <summary>
        /// 绑定GridView的空信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gvList"></param>
        /// <param name="t"></param>
        public void BindGridViewEmpty<T>(GridView gvList, T t)
        {
            String EmptyDataText = ViewGridViewEmpty();
            if (gvList.Rows.Count == 0 || gvList.Rows[0].Cells[0].Text == EmptyDataText)
            {
                List<T> ss = new List<T>();
                ss.Add(t);
                gvList.DataSource = ss;
                gvList.DataBind();

                gvList.Rows[0].Cells.Clear();
                gvList.Rows[0].Cells.Add(new TableCell());
                gvList.Rows[0].Cells[0].ColumnSpan = gvList.HeaderRow.Cells.Count;
                gvList.Rows[0].Cells[0].Text = EmptyDataText;
                gvList.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
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

            String language = WebHelper.GetStringParam(Request,"language",PortalSettings.DefaultLanguage);

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

            Boolean is_SkinFileName = false;
            Boolean is_SkinPath = false;


            if (AddParameters != null && AddParameters.Length > 0)
            {
                foreach (String parameter in AddParameters)
                {

                    sb.AppendFormat("&{0}", parameter);

                    if (!String.IsNullOrEmpty(parameter))
                    {
                         if(parameter.IndexOf("SkinFileName=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            is_SkinFileName= true;
                        }

                         if (parameter.IndexOf("SkinPath=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                         {
                             is_SkinPath = true;
                         }
                    }
                }
            }


            String _SkinFileName_ = WebHelper.GetStringParam(Request, "SkinFileName", SkinFileName);
            if (!String.IsNullOrEmpty(_SkinFileName_) && !is_SkinFileName)
            {
                sb.AppendFormat("&SkinFileName={0}", HttpUtility.UrlEncode(_SkinFileName_));
            }

            String _SkinPath_ = WebHelper.GetStringParam(Request, "SkinPath", SkinPath);
            if (!String.IsNullOrEmpty(_SkinPath_) && !is_SkinPath)
            {
                sb.AppendFormat("&SkinPath={0}", HttpUtility.UrlEncode(_SkinPath_));
            }

            return sb.ToString();

        }


      


        #endregion

        #region "错误捕获"

        /// <summary>
        /// 错误捕获
        /// </summary>
        /// <param name="exc">错误</param>
        public void ProcessModuleLoadException(Exception exc)
        {
            if (HttpContext.Current.Session["Exception"] != null)
            {
                HttpContext.Current.Session.Remove("Exception");
            }
            //增加当前序列化的内容到Session
            HttpContext.Current.Session.Add("Exception", exc);

            if (WebHelper.GetStringParam(Request, "Token", "").ToLower() != "error")
            {
                Response.Redirect(xUrl("ReturnUrl", HttpUtility.UrlEncode(WebHelper.GetScriptUrl), "Error"), false);
            }

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

            //refresh cache
            SynchronizeModule();
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

        #region "关于用户处理"

        /// <summary>
        /// 显示用户头像
        /// </summary>
        /// <param name="uInfo"></param>
        /// <returns></returns>
        public String ViewUserPic(UserInfo uInfo)
        {
          String  Photo = String.Format("{0}Resource/images/no_user.png", ModulePath);
          String UserPic = uInfo.Profile.GetPropertyValue("Photo");
          Int32 result = 0;
          if (!String.IsNullOrEmpty(UserPic) && int.TryParse(UserPic.Trim(), out result) && result > 0)
          {
              Photo = Globals.LinkClick(String.Format("fileid={0}", result), DotNetNuke.Common.Utilities.Null.NullInteger, DotNetNuke.Common.Utilities.Null.NullInteger);
          }
          return Photo;
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
            return String.Format("{0}Resource/images/crystal/{1}", ModulePath, IconName);
        }

        #endregion

        #region "绑定页面标题和帮助"

        /// <summary>
        /// 显示控件标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <param name="Suffix"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewControlTitle(String Title, String DefaultValue, String ControlName,String Suffix, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
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

            return String.Format("<label  {2} {1}>{0}{3}</label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : "",
              Suffix
                );
        }




        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewTitle(Title, DefaultValue,"");
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
        public String ViewTitle(String Title, String DefaultValue, String ControlName,String ClassName)
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
            return ViewHelp(Title, DefaultValue,"");
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue, String ControlName)
        {
            String Content = ViewResourceText(Title, DefaultValue, "Help");
            return ViewSpan( Content, ControlName, "span_help");
           // return ViewSpan(String.Format("<i class=\"fa fa-info-circle\"></i>{0}",   Content), ControlName, "span_help");
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

            return String.Format("<label  {2} {1}><span {1} >{0}</span></label>",
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
        /// 显示菜单的文本
        /// </summary>
        /// <param name="MenuItem">菜单项</param>
        /// <returns></returns>
        public String ShowMenuText(TokenItem MenuItem)
        {
            return ViewResourceText(MenuItem.Token, MenuItem.Title, "MenuText");
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



        /// <summary>
        /// 显示标题和帮助
        /// </summary>
        /// <param name="ControlItem"></param>
        /// <returns></returns>
        public String ViewTitleAndHelp(SettingEntity ControlItem)
        {
            ControlHelper ctl = new ControlHelper(PortalId);

            String ControlName = String.Empty;
            System.Web.UI.Control c = FindControl(ctl.ViewControlID(ControlItem));
            if (c != null && !String.IsNullOrEmpty(c.ClientID))
            {
                ControlName = c.ClientID;
            }
            return String.Format("<label class=\"col-sm-3 control-label\" {1}>{0} :</label>",ControlItem.Alias,!String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : "");
        }



        #endregion

        #region "名称格式化"
        /// <summary>
        /// 搜索条件格式化
        /// </summary>
        /// <param name="Search">搜索条件</param>
        /// <returns></returns>
        public String SearchFormat(String Search)
        {
            return String.Format("{0}-{1}-{2}-{3}", Search, ModuleId, ClientID, TabId);
        }


        /// <summary>
        /// 格式化名称
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns></returns>
        public String FormatName(object _Name)
        {
            String Name = String.Empty;
            if (_Name != null && !String.IsNullOrEmpty(Convert.ToString(_Name)))
            {
                Name = Convert.ToString(_Name).Replace(" ", "");
                Name = Name.Replace("-", "");
                Name = Name.Replace(",", "");
                Name = Name.Replace(".", "");
            }
            return Name;
        }

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
            return m.GetProperty(Name, "", System.Globalization.CultureInfo.CurrentCulture, UserInfo, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
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