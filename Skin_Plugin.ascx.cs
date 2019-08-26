using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using System.IO;
 

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_Plugin : BaseSkin
    {
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

   



        #endregion

        #region "==属性=="




        /// <summary>
        /// 站点编号
        /// </summary>
        public Int32 PortalId
        {
            get { return PortalSettings.PortalId; }
        }


        /// <summary>
        /// 皮肤插件模块编号
        /// </summary>
        public Int32 ThemePlugin_Init_ModuleID
        {
            get { return GetSetting("Init_ThemePlugin_ModuleID", 0); }
        }

        /// <summary>
        /// 皮肤插件页面编号
        /// </summary>
        public Int32 ThemePlugin_Init_TabID
        {
            get { return GetSetting("Init_ThemePlugin_TabID", 0); }
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
                    FileInfo file = new FileInfo(MapPath(PortalSettings.ActiveTab.SkinSrc));
                    if (file.Exists && !String.IsNullOrEmpty(file.Name))
                    {
                        _SkinFileName = file.Name.Replace(String.Format("{0}", file.Extension), ""); ;
                    }
                }
                return _SkinFileName;
            }
        }






        #endregion


        #region "==方法=="




        /// <summary>
        /// 绑定元素到页面
        /// </summary>
        private void BindItemToPage()
        {
            hlLink.NavigateUrl = xUrl("","", "Skin_Options", "SkinFileName="+ SkinFileName,"SkinPath=" + HttpUtility.UrlEncode(SkinPath));
            //ThemePlugin_Panel.Visible = hlLink.Visible = IsAdmin || DemoLock;//管理员和超级管理员均可见
            //ThemePlugin_Panel.Visible = hlLink.Visible = IsHost || DemoLock;//只有超级管理员可见
            Boolean ShowHost = Convert.ToBoolean(GetSetting("ShowIcon_Host", "true"));
            Boolean ShowAdmin = Convert.ToBoolean(GetSetting("ShowIcon_Admin", "true"));

            ThemePlugin_Panel.Visible = hlLink.Visible = false;
            if (ShowHost && ShowAdmin)
            {
                ThemePlugin_Panel.Visible = hlLink.Visible = IsAdmin || DemoLock;//管理员和超级管理员均可见
            }
            else if (ShowHost)
            {
                ThemePlugin_Panel.Visible = hlLink.Visible = IsHost || DemoLock;//只有超级管理员可见
            }
            else if (ShowAdmin)
            {
                ThemePlugin_Panel.Visible = hlLink.Visible = IsAdministrator || DemoLock;//只有普通管理员可见
            }



          

        }




        /// <summary>
        /// 打印引用到的字体库
        /// </summary>
        /// <returns></returns>
        public void ViewFamilyFonts_link()
        {
      

            List<FontDB> fonts = new FontDBHelper(this).FindAll();

            //遍历打印所有需要引用的字体库
            foreach (FontDB font in fonts)
            {
                if (!String.IsNullOrEmpty(font.PrimaryGuid) && font.Enable)
                {
                    if (font.IsFontLink.HasValue && font.IsFontLink.Value)
                    {
                        if (!String.IsNullOrEmpty(font.FontUrl))
                        {
                            LoadStyle(String.Format("fontcss_{0}", font.PrimaryGuid), font.FontUrl);
                            //sb.AppendFormat("<link href='{0}' rel='stylesheet' type='text/css'>", font.FontUrl).AppendLine();
                        }
                    }
                }
            }
        }


        #endregion


        #region "==事件=="
 
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (String.IsNullOrEmpty(skin_Error))
                    {




                    }
                }




            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessPageLoadException(ex, Request.RawUrl);
            }
        }


        protected void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (String.IsNullOrEmpty(skin_Error))
                    {

                        //初始化模块
                        Inits();
                        //绑定皮肤对象上的菜单
                        BindItemToPage();


                    }
                }
                //打印引用到的字体库
                ViewFamilyFonts_link();
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessPageLoadException(ex, Request.RawUrl);
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

            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);

            sb.AppendFormat("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", ModulePath, PortalId, ThemePlugin_Init_TabID, ThemePlugin_Init_ModuleID, language);

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










    }
}