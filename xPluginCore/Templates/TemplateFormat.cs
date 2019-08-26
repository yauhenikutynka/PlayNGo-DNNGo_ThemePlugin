using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;

using DotNetNuke.Entities.Modules;
using System.IO;
using System.Web.UI;
using System.Threading;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;

namespace DNNGo.Modules.ThemePlugin
{
    public class TemplateFormat
    {


        #region "属性"
        /// <summary>
        /// 模块基类
        /// </summary>
        private BaseModule bpm = new BaseModule();

     

        private Button _ViewButton;
        /// <summary>
        /// 触发按钮
        /// </summary>
        public Button ViewButton
        {
            get { return _ViewButton; }
            set { _ViewButton = value; }
        }



        private String _ThemeXmlName = String.Empty;
        /// <summary>
        /// 主题XML名称
        /// </summary>
        public String ThemeXmlName
        {
            get { return _ThemeXmlName; }
            set { _ThemeXmlName = value; }
        }

        private PlaceHolder _PhContent = new PlaceHolder();

        public PlaceHolder PhContent
        {
            get { return _PhContent; }
            set { _PhContent = value; }
        }





        private List<SettingEntity> _OptionList = new List<SettingEntity>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SettingEntity> OptionList
        {
            get { return _OptionList; }
            set { _OptionList = value; }
        }


        private List<KeyValueEntity> _ItemValues = new List<KeyValueEntity>();
        /// <summary>
        /// 
        /// </summary>
        public List<KeyValueEntity> ItemValues
        {
            get { return _ItemValues; }
            set { _ItemValues = value; }
        }



        private List<SettingEntity> _GlobalOptionList = new List<SettingEntity>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SettingEntity> GlobalOptionList
        {
            get { return _GlobalOptionList; }
            set { _GlobalOptionList = value; }
        }


        private List<KeyValueEntity> _GlobalValues = new List<KeyValueEntity>();
        /// <summary>
        /// 
        /// </summary>
        public List<KeyValueEntity> GlobalValues
        {
            get { return _GlobalValues; }
            set { _GlobalValues = value; }
        }


        #endregion



        #region "方法"

        #region "--关于内容与标题--"

        /// <summary>
        /// 显示标题(通过资源文件)
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="DefaultValue">资源文件未定义时默认值</param>
        /// <returns>返回值</returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue);
        }
 

 

 
 
 

       
        #endregion

        #region "--关于链接跳转--"
         

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <returns></returns>
        public String GoLogin()
        {
            return  Globals.NavigateURL(bpm.PortalSettings.LoginTabId, "Login", "returnurl=" +  HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
        }
 

        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl(String goUrl)
        {
            return String.Format("http://{0}{1}",WebHelper.GetHomeUrl(), goUrl);
        }
        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl()
        {
            return String.Format("http://{0}", WebHelper.GetHomeUrl());
        }





        #endregion


        #region "--XML参数读取--"

        /// <summary>
        /// 字体Family
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewFontFamily(String Name, String DefaultValue)
        {
            String FontString = Settings(Name,"").ToString();
            if (!String.IsNullOrEmpty(FontString))
            {
                List<String> defaultFonts = Common.GetList(FontString, ":");
                if (defaultFonts != null && defaultFonts.Count == 3)
                {
                    DefaultValue = defaultFonts[1];
                }
            }
            return DefaultValue;
        }


        /// <summary>
        /// 字体Bold
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewFontBold(String Name, String DefaultValue)
        {
            String FontString = Settings(Name, "").ToString();
            if (!String.IsNullOrEmpty(FontString))
            {
                List<String> defaultFonts = Common.GetList(FontString, ":");
                if (defaultFonts != null && defaultFonts.Count == 3)
                {
                    DefaultValue = defaultFonts[2];
                }
            }
            return DefaultValue;
        }



        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object Settings(String Name, object DefaultValue)
        {
            return ViewXmlSetting(Name, DefaultValue);
        }



        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            String o = String.Empty;
            if (!String.IsNullOrEmpty(Name) && ItemValues != null && ItemValues.Count > 0)
            {
                KeyValueEntity item = ItemValues.Find(r1 => r1.Key == Name);
                if (item != null && item.Key == Name)
                {
                    o = item.Value.ToString();
                }
            }
            return !String.IsNullOrEmpty(o) ? ConvertTo.FormatValue(o, DefaultValue.GetType()) : DefaultValue;
        }




        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewGlobalSetting(String Name, object DefaultValue)
        {
            String o = String.Empty;
            if (!String.IsNullOrEmpty(Name) && GlobalValues != null && GlobalValues.Count > 0)
            {
                KeyValueEntity item = GlobalValues.Find(r1 => r1.Key == Name);
                if (item != null && item.Key == Name)
                {
                    o = item.Value.ToString();
                }
            }
            return !String.IsNullOrEmpty(o) ? ConvertTo.FormatValue(o, DefaultValue.GetType()) : DefaultValue;
        }


        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <param name="oldString">要替换的字符</param>
        /// <param name="newString">被替换的字符</param>
        /// <returns></returns>
        public String ViewXmlSetting(String Name, object DefaultValue, String oldString, String newString)
        {
            return SettingReplace(Name, DefaultValue, oldString, newString);
        }

        /// <summary>
        /// 将字符串转化为列表,逗号为分隔符
        /// </summary>
        /// <param name="Items"></param>
        /// <returns></returns>
        public List<String> ToList(String Items)
        {
            List<String> list = new List<String>();
            if(!String.IsNullOrEmpty(Items))
            {
                list = WebHelper.GetList(Items);
            }
            return list;
        }

        /// <summary>
        /// 显示上传的图片
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewPicture(String Name, String DefaultValue)
        {
            String Picture =Convert.ToString( ViewXmlSetting(Name,DefaultValue));
            return bpm.ResolveUrl(String.Format("{0}{1}",bpm.SkinPath, Picture));
        }


        public String ViewGlobalPicture(String Name, String DefaultValue)
        {
            String Picture = Convert.ToString(ViewGlobalSetting(Name, DefaultValue));
            return bpm.ResolveUrl(String.Format("{0}{1}", bpm.SkinPath, Picture));
        }


        /// <summary>
        /// 显示URLs控件的内容
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewUrls(String Name, String DefaultValue)
        {
            String Picture = Convert.ToString(Settings(Name, DefaultValue));
            return ViewLinkUrl(Picture, DefaultValue);
        }




        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, String DefaultValue, int PortalId)
        {
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
                            DefaultValue = string.Format("{0}{1}{2}", bpm.PortalSettings.HomeDirectory, fi.Folder, bpm.Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {


                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_ThemePlugin_Multimedia Multimedia = DNNGo_ThemePlugin_Multimedia.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            DefaultValue = bpm.Server.UrlPathEncode(bpm.GetPhotoPath(Multimedia.FilePath));// String.Format("{0}{1}", bpm.MemberGroup_PortalSettings.HomeDirectory, Multimedia.FilePath);
                        }

                        if (!String.IsNullOrEmpty(DefaultValue))
                        {
                            if (DefaultValue.ToLower().IndexOf("http://") < 0)
                            {
                                DefaultValue = string.Format("http://{0}{1}", WebHelper.GetHomeUrl(), DefaultValue);
                            }

                        }
                    }
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {

                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")), false, bpm.PortalSettings, Null.NullString, "", "");

                }
                else
                {
                    DefaultValue = UrlValue;
                }
            }
            return DefaultValue;

        }


        public String ViewLinkUrl(String UrlValue, String DefaultValue)
        {
            return ViewLinkUrl(UrlValue, DefaultValue, bpm.PortalId);
        }


        public String ViewLinkUrl(String UrlValue)
        {
            String DefaultValue = String.Empty;
            if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                DefaultValue = String.Format("{0}Resource/images/no_image.png", bpm.ModulePath);
            }
            return ViewLinkUrl(UrlValue, DefaultValue, bpm.PortalId);
        }

     

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="DefaultValue"></param>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public String Replace(String DefaultValue, String oldString, String newString)
        {
            return DefaultValue.Replace(oldString, newString);
        }

        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="FirstNumber">第一个数</param>
        /// <param name="LastNumber">第二个数</param>
        /// <returns></returns>
        public String Division(String FirstNumber, String LastNumber)
        {
            String d = "0";
            if (!String.IsNullOrEmpty(FirstNumber) && !String.IsNullOrEmpty(LastNumber))
            {
                if(FirstNumber.IndexOf(",")>=0) FirstNumber = FirstNumber.Replace(",", ".");
                if(LastNumber.IndexOf(",") >= 0) LastNumber = LastNumber.Replace(",", ".");

                float f = float.Parse(FirstNumber) / float.Parse(LastNumber);

                if(f > 0f)
                {
                    d = f.ToString();
                    if(!String.IsNullOrEmpty(d) && d.IndexOf(",")>=0)
                    {
                         d = d.Replace(",", ".");
                    }

                }


            }
            return d;
        }

        /// <summary>
        /// 生成HTML引入字符串
        /// (为了避免与模板引擎关键字冲突)
        /// </summary>
        /// <param name="_FileString"></param>
        /// <returns></returns>
        public String ViewInclude(String _FileString)
        {
            return String.Format("<!--#include file=\"{0}\"-->", _FileString);
        }

        /// <summary>
        /// 配置替换
        /// </summary>
        /// <param name="Name">配置名</param>
        /// <param name="DefaultValue">配置值</param>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public String SettingReplace(String Name, object DefaultValue, String oldString, String newString)
        {
            String __value = Settings(Name, DefaultValue).ToString();

            if (!String.IsNullOrEmpty(__value))
            {
                __value = Replace(__value, oldString, newString);
            }
            return __value;
        }

        #endregion



 


        #region "字体库引用的方式"



        /// <summary>
        /// 获取所有用到了的字体库
        /// </summary>
        /// <returns></returns>
        public List<FontDB> GetFonts()
        {
             return new FontDBHelper(bpm).FindAll();
        }

        public List<String> GetFontUrls()
        {
            List<String> FontUrls = new List<string>();
            List<FontDB> fonts = GetFonts();

            //遍历打印所有需要引用的字体库
            foreach (FontDB font in fonts)
            {
                if (!String.IsNullOrEmpty(font.PrimaryGuid))
                {
                    if (font.IsFontLink.HasValue && font.IsFontLink.Value)
                    {
                        if (!String.IsNullOrEmpty(font.FontUrl))
                        {
                            FontUrls.Add(font.FontUrl);
                            //sb.AppendFormat("@import url({0});", font.FontUrl).AppendLine();
                        }
                    }
                    else
                    {
                        //字体文件夹的路径获取
                        String FontPath = String.Format("{0}Fonts/", bpm.SkinPath);
                       

                        //如果是上传的字体
                        if (!String.IsNullOrEmpty(font.Font_File_Eot))
                        {
                            FontUrls.Add(String.Format("{0}{1}", FontPath, font.Font_File_Eot));
                        }
                        if (!String.IsNullOrEmpty(font.Font_File_Svg))
                        {
                            FontUrls.Add(String.Format("{0}{1}", FontPath, font.Font_File_Svg));
                        }
                        if (!String.IsNullOrEmpty(font.Font_File_Ttf))
                        {
                            FontUrls.Add(String.Format("{0}{1}", FontPath, font.Font_File_Ttf));
                        }
                        if (!String.IsNullOrEmpty(font.Font_File_Woff))
                        {
                            FontUrls.Add(String.Format("{0}{1}", FontPath, font.Font_File_Woff));
                        }
                   

                    }


                    //sb.AppendFormat("@import url(https://fonts.googleapis.com/css?family={0});", HttpUtility.UrlEncode(font.Value)).AppendLine();
                }
            }

            return FontUrls;
        }



        /// <summary>
        /// 打印引用到的字体库
        /// </summary>
        /// <returns></returns>
        public String ViewFamilyFonts()
        {
            StringBuilder sb = new StringBuilder();

            List<FontDB> fonts = GetFonts();

            //fonts.Sort(delegate (FontDB x, FontDB y)
            //{
            //    if (!x.IsFontLink.HasValue || !y.IsFontLink.HasValue) return 0;
            //    else return y.IsFontLink.Value.CompareTo(x.IsFontLink.Value);
            //});


            //遍历打印所有需要引用的字体库
            foreach (FontDB font in fonts)
            {
                if (!String.IsNullOrEmpty(font.PrimaryGuid) && font.Enable)
                {
                    if (font.IsFontLink.HasValue && font.IsFontLink.Value)
                    {
                        if (!String.IsNullOrEmpty(font.FontUrl))
                        {
                            //sb.AppendFormat("@import url({0});", font.FontUrl).AppendLine();
                        }
                    }
                    else
                    {
                        //字体文件夹的路径获取
                        String FontPath = String.Format("{0}Fonts/", bpm.SkinPath);
                        sb.Append("@font-face {").AppendLine();
                        sb.AppendFormat("font-family: '{0}';", font.Family).AppendLine();

                        sb.AppendFormat("src: url({0}{1});", FontPath, font.Font_File_Eot).AppendLine();
                        sb.AppendFormat("src: url({0}{1}) format('embedded-opentype'),", FontPath, font.Font_File_Eot).AppendLine();
                        sb.AppendFormat("url({0}{1}) format('svg'),", FontPath, font.Font_File_Svg).AppendLine();
                        sb.AppendFormat("url({0}{1}) format('truetype'),", FontPath, font.Font_File_Ttf).AppendLine();
                        sb.AppendFormat("url({0}{1}) format('woff');", FontPath, font.Font_File_Woff).AppendLine();


                        sb.Append("}").AppendLine();
                    }
                }
            }

            

            return sb.ToString();
        }






        #endregion

        #region "--关于模版--"

        private String _SkinPath = String.Empty;
        /// <summary>
        /// 当前模版路径
        /// </summary>
        public String SkinPath
        {
            get {
                if (String.IsNullOrEmpty(_SkinPath))
                {


                    _SkinPath = bpm.SkinPath;

                }
                return _SkinPath;
            }
        }


        #endregion



        #endregion



        #region "构造"

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            String _Title = Localization.GetString(String.Format("{0}.Text", Title), bpm.LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }


        public TemplateFormat(BaseModule _bpm)
        {
            bpm = _bpm;
        }

        public TemplateFormat()
        {
            
        }

        #endregion

    }
}