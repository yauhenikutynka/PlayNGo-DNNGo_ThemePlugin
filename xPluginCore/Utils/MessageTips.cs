using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Services.Localization;
using System.Web.UI;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 消息提示
    /// </summary>
    [Serializable]
    public class MessageTips
    {

        #region "属性"

        private String _Content;
        /// <summary>
        /// 消息内容
        /// </summary>
        public String Content
        {
            get { return _Content; }
            set { _Content = value; }
        }


        private EnumTips _MsgType = EnumTips.Alert;
        /// <summary>
        /// 消息类型
        /// </summary>
        public EnumTips MsgType
        {
            get { return _MsgType; }
            set { _MsgType = value; }
        }

        private Boolean _IsPostBack = false;
        /// <summary>
        /// 是否回发时响应
        /// </summary>
        public Boolean IsPostBack
        {
            get { return _IsPostBack; }
            set { _IsPostBack = value; }
        }



        private String _GoUrl;
        /// <summary>
        /// 跳转URL
        /// </summary>
        public String GoUrl
        {
            get { return _GoUrl; }
            set { _GoUrl = value; }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public String Prefix { get;set; }


 
        

        #endregion



        #region "构造"

        /// <summary>
        /// 默认构造
        /// </summary>
        public MessageTips()
        {

        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public MessageTips(String _Prefix)
        {
            Prefix = _Prefix;
        }


        #endregion


        #region "方法"


        public String SessionName()
        {
            return String.Format("Tips_{0}", Prefix);
        }

        /// <summary>
        /// 保存需要提示的内容
        /// </summary>
        public void Put()
        {
            if (!String.IsNullOrEmpty(_Content))
            {
                //将当前的内容序列化到个人缓存

                String s = ConvertTo.Serialize<MessageTips>((MessageTips)this);
                //如果有Session的话需要先清除
                if (HttpContext.Current.Session[SessionName()] != null)
                {
                    HttpContext.Current.Session.Remove(SessionName());
                }
                //增加当前序列化的内容到Session
                HttpContext.Current.Session.Add(SessionName(), s);
            }
            //跳转
            if (!String.IsNullOrEmpty(_GoUrl))
            {
                HttpContext.Current.Response.Redirect(_GoUrl, true);
            }
        }

        /// <summary>
        /// 向页面注册提示信息
        /// </summary>
        public String Post(Page p)
        {

            if (HttpContext.Current.Session[SessionName()] != null)
            {

                String s = Convert.ToString(HttpContext.Current.Session[SessionName()]);
                if (!String.IsNullOrEmpty(s))
                {
                    MessageTips mt = ConvertTo.Deserialize<MessageTips>(s);
                    if (mt != null && (mt.IsPostBack || !p.IsPostBack))
                    {
                        HttpContext.Current.Session.Remove(SessionName());
                        this._Content = mt.Content;
                        this._MsgType = mt.MsgType;
                        this._GoUrl = mt.GoUrl;
                    }
                }
            }

            if (!String.IsNullOrEmpty(_Content))
            {
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<script type=\"text/javascript\">");
                //sb.AppendFormat("asyncbox.tips('{0}','{1}');", _Content, EnumHelper.GetEnumTextVal((Int32)_MsgType, typeof(EnumTips)));
                //sb.Append("</script>");
                //Pmb.Page.RegisterStartupScript("asyncbox.tips", sb.ToString());
            }
            return _Content;

        }

        


        /// <summary>
        /// 向页面注册提示信息
        /// </summary>
        public String Post(DotNetNuke.Entities.Modules.PortalModuleBase Pmb)
        {
            return Post(Pmb.Page);
        }


        public String PostHtml(Page p)
        {
            if (HttpContext.Current.Session[SessionName()] != null)
            {

                String s = Convert.ToString(HttpContext.Current.Session[SessionName()]);
                if (!String.IsNullOrEmpty(s))
                {
                    MessageTips mt = ConvertTo.Deserialize<MessageTips>(s);
                    if (mt != null && (mt.IsPostBack || !p.IsPostBack))
                    {
                        HttpContext.Current.Session.Remove(SessionName());
                        this._Content = mt.Content;
                        this._MsgType = mt.MsgType;
                        this._GoUrl = mt.GoUrl;
                    }
                }
            }

            if (!String.IsNullOrEmpty(_Content))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (MsgType == EnumTips.Alert)
                {
                    sb.Append("<div class=\"alert alert-info\">").AppendLine();
                    sb.Append("<button data-dismiss=\"alert\" class=\"close\">×</button>").AppendLine();
                    sb.AppendFormat("<i class=\"fa fa-info-circle\"></i> {0}", _Content).AppendLine();
                    sb.Append("</div>").AppendLine();
                }
                else if (MsgType == EnumTips.Error)
                {
                    sb.Append("<div class=\"alert alert-danger\">").AppendLine();
                    sb.Append("<button data-dismiss=\"alert\" class=\"close\">×</button>").AppendLine();
                    sb.AppendFormat("<i class=\"fa fa-times-circle\"></i> {0}", _Content).AppendLine();
                    sb.Append("</div>").AppendLine();
                }
                else if (MsgType == EnumTips.Warning)
                {
                    sb.Append("<div class=\"alert alert-warning\">").AppendLine();
                    sb.Append("<button data-dismiss=\"alert\" class=\"close\">×</button>").AppendLine();
                    sb.AppendFormat("<i class=\"fa fa-exclamation-triangle\"></i> {0}", _Content).AppendLine();
                    sb.Append("</div>").AppendLine();
                }
                else if (MsgType == EnumTips.Success)
                {
                    sb.Append("<div class=\"alert alert-success\">").AppendLine();
                    sb.Append("<button data-dismiss=\"alert\" class=\"close\">×</button>").AppendLine();
                    sb.AppendFormat("<i class=\"fa fa-check-circle\"></i> {0}", _Content).AppendLine();
                    sb.Append("</div>").AppendLine();
                }
                return sb.ToString();
            }
            return _Content;
        }



        /// <summary>
        /// 载入提示信息（带提交）
        /// </summary>
        /// <param name="pmb"></param>
        /// <param name="key"></param>
        public void LoadMessage(String key, DotNetNuke.Entities.Modules.PortalModuleBase Pmb, params object[] args)
        {
            LoadMessage(key, _MsgType, Pmb, args);
        }
        /// <summary>
        /// 载入提示信息（带提交）
        /// </summary>
        /// <param name="key"></param>
        public void LoadMessage(String key, DotNetNuke.Entities.Modules.PortalModuleBase Pmb)
        {
            LoadMessage(key, _MsgType, Pmb, new String[] { "" });
        }
        /// <summary>
        ///  载入提示信息（带提交）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="__MsgType"></param>
        public void LoadMessage(String key, EnumTips __MsgType, DotNetNuke.Entities.Modules.PortalModuleBase Pmb)
        {
            _MsgType = __MsgType;
            LoadMessage(key, _MsgType, Pmb, new String[] { "" });
        }

        /// <summary>
        /// 载入提示信息（带提交）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="__MsgType"></param>
        public void LoadMessage(String key, EnumTips __MsgType, DotNetNuke.Entities.Modules.PortalModuleBase Pmb, params object[] args)
        {
            _MsgType = __MsgType;

            key = key.Replace(".Message", "");

            _Content = Localization.GetString(String.Format("{0}.Message", key), Localization.GetResourceFile(Pmb, "Message.ascx.resx"));
            if (!String.IsNullOrEmpty(_Content) && args != null && args.Length > 0)
            {
                _Content = String.Format(_Content, args);
            }
            Put();
        }




        #endregion












    }
}