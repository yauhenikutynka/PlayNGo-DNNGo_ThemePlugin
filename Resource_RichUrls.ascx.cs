using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Resource_RichUrls : BaseModule
    {

        #region "属性"

        private SettingEntity _FieldItem;
        /// <summary>
        /// 字段设置
        /// </summary>
        public SettingEntity FieldItem
        {
            get { return _FieldItem; }
            set { _FieldItem = value; }
        }

        public String QueryString
        {
            get { return String.Format("{0}&ModulePath={1}", WebHelper.GetScriptNameQueryString, HttpUtility.UrlEncode(ModulePath)); }
        }


        private String _ClientName = "RichUrls";
        /// <summary>
        /// 控件的名称
        /// </summary>
        public String ClientName
        {
            get {
                if (_ClientName == "RichUrls" && FieldItem != null && !String.IsNullOrEmpty(FieldItem.Name))
                {
                    _ClientName = ControlHelper.GetRichUrlsName(FieldItem);
                }
                return _ClientName; }
        }


        private String _ShowDefault = "U";
        /// <summary>
        /// 显示默认类型
        /// </summary>
        public String ShowDefault
        {
            get { return _ShowDefault; }
            set { _ShowDefault = value; }
        }


        #endregion

        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindPageItem();
                }
                else
                {
                    String urllink = GetValue();
                    if (Context.Items.Contains(ClientName))
                    {
                        Context.Items[ClientName] = urllink;
                    }
                    else
                    {
                        Context.Items.Add(ClientName, urllink);
                    }
                }
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }
        #endregion

        #region "方法"
        /// <Description>
        /// 绑定页面项
        /// </Description>
        private void BindPageItem()
        {

            //链接
            String Element_UrlLink = FieldItem != null ? FieldItem.DefaultValue : String.Empty;

            //imgUrlLink.Attributes.Add("onError", String.Format("this.src='{0}Resource/images/1-1.png'", ModulePath));

            WebHelper.BindList<TabInfo>(ddlUrlLink, TabController.GetPortalTabs(PortalId, Null.NullInteger, true, true, false, false), "IndentedTabName", "TabId");

            List<EnumEntity> EnumList = EnumHelper.GetEnumList(typeof(EnumUrlControls));

            
                //设置和选择哪些类型可以显示出来
           
                if (!String.IsNullOrEmpty(FieldItem.ListContent))
                {
                    if (!FindUrlType(FieldItem.ListContent, "U")) EnumList.RemoveAll(r => r.Value == 1);
                    if (!FindUrlType(FieldItem.ListContent, "P")) EnumList.RemoveAll(r => r.Value == 2);
                    if (!FindUrlType(FieldItem.ListContent, "F")) EnumList.RemoveAll(r => r.Value == 3);

                    if (EnumList.Count == 1) rblUrlLink.Visible = false;

                    WebHelper.BindList<EnumEntity>(rblUrlLink, EnumList, "Text", "Value");

                    String defaultType = WebHelper.leftx(FieldItem.ListContent, 1).ToUpper();
                    if (!String.IsNullOrEmpty(defaultType))
                    {
                        ShowHideControl(defaultType);
                    }
                }
                else
                {
                    WebHelper.BindList<EnumEntity>(rblUrlLink, EnumList, "Text", "Value");
                    ShowHideControl("U");
                }
          
      
            



            if (!String.IsNullOrEmpty(Element_UrlLink) && Element_UrlLink.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) == 0)
            {

                WebHelper.SelectedListByValue(ddlUrlLink, Element_UrlLink.Replace("TabID=", ""));
                //WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Page);
                //txtUrlLink.Attributes.Add("style", "display:none");
                //panUrlLink.Attributes.Add("style", "display:none");
                ShowHideControl("P");
            }
            else if (!String.IsNullOrEmpty(Element_UrlLink) && Element_UrlLink.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                TemplateFormat xf = new TemplateFormat(this);
                hfUrlLink.Value = Element_UrlLink;
                //imgUrlLink.ImageUrl = xf.ViewLinkUrl(Element_UrlLink);

                div_Image.Attributes.Add("data-MediaID", Element_UrlLink);

                ShowHideControl("F");
                //hlRemoveUrlLink.Attributes.Add("style", "display:;");
                //WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Files);
                //txtUrlLink.Attributes.Add("style", "display:none");
                //ddlUrlLink.Attributes.Add("style", "display:none");
            }
            else
            {
                if (!String.IsNullOrEmpty(Element_UrlLink) )
                {
                    if (Element_UrlLink.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        int FileID = 0;
                        if (int.TryParse(Element_UrlLink.Replace("FileID=", ""), out FileID) && FileID > 0)
                        {
                            var fi = FileManager.Instance.GetFile(FileID);
                            if (fi != null && fi.FileId > 0)
                            {
                                txtUrlLink.Text = string.Format("{0}{1}{2}", PortalSettings.HomeDirectory, fi.Folder, Server.UrlPathEncode(fi.FileName));
                            }
                        }
                    }
                    else
                    {
                        txtUrlLink.Text = Element_UrlLink;
                    }
                    ShowHideControl("U");
                }
              
                //WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Url);
                //ddlUrlLink.Attributes.Add("style", "display:none");
                //panUrlLink.Attributes.Add("style", "display:none");
            }


        }

        public void ShowHideControl(String defaultType)
        {
            ShowDefault = defaultType;


            if (defaultType == "P")
            {
                WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Page);
                txtUrlLink.Attributes.Add("style", "display:none");
                panUrlLink.Attributes.Add("style", "display:none");
            }
            else if (defaultType == "F")
            {
              
                WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Files);
                txtUrlLink.Attributes.Add("style", "display:none");
                ddlUrlLink.Attributes.Add("style", "display:none");
            }
            else
            {
                WebHelper.SelectedListByValue(rblUrlLink, (Int32)EnumUrlControls.Url);
                ddlUrlLink.Attributes.Add("style", "display:none");
                panUrlLink.Attributes.Add("style", "display:none");
            }
        }



        /// <summary>
        /// 查找URL类型是否存在
        /// </summary>
        /// <param name="ListContent"></param>
        /// <param name="UrlType"></param>
        /// <returns></returns>
        public Boolean FindUrlType(String ListContent,String UrlType)
        {
            Boolean Result = true;
            if (!String.IsNullOrEmpty(ListContent))
            {
                Result = ListContent.IndexOf(UrlType, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
            return Result;
        }




        /// <summary>
        /// 获取当前空间的值
        /// </summary>
        /// <returns></returns>
        public String GetValue()
        {

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                String defaultType = WebHelper.leftx(FieldItem.ListContent, 1).ToUpper();
                if (!String.IsNullOrEmpty(defaultType))
                {
                    ShowHideControl(defaultType);
                }
            }


            String UrlLink = String.Empty;
            if (WebHelper.GetIntParam(Request, rblUrlLink.UniqueID, 1) == (Int32)EnumUrlControls.Page || ShowDefault == "P"  )
            {
                UrlLink = String.Format("TabID={0}",WebHelper.GetStringParam(Request,    ddlUrlLink.UniqueID,""));
            }
            else if (WebHelper.GetIntParam(Request, rblUrlLink.UniqueID, 1) == (Int32)EnumUrlControls.Files || ShowDefault == "F")
            {
                UrlLink = WebHelper.GetStringParam(Request,    hfUrlLink.UniqueID,"");
            }
            else
            {

                UrlLink = WebHelper.GetStringParam(Request, txtUrlLink.UniqueID, "");  
            }
        
            return UrlLink;
        }


        #endregion

    }
}