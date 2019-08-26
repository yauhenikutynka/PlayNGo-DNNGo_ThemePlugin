using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_TabEdit : BaseModule
    {
        /// <summary>
        /// 编辑页面的编号
        /// </summary>
        public Int32 EditTabID = WebHelper.GetIntParam(HttpContext.Current.Request, "EditTabID", 0);


        private TabInfo _TabItem = new TabInfo();
        /// <summary>
        /// 页面项
        /// </summary>
        public TabInfo TabItem
        {
            get {
                if (!(_TabItem != null && _TabItem.TabID > 0))
                {
                    if (int.MaxValue == EditTabID)
                    {
                        _TabItem = InitGlobalTab();
                    }
                    else
                    {
                        _TabItem = objTabs.GetTab(EditTabID);
                    }
                }
                return _TabItem; }
        }


        /// <summary>
        /// 当前标签
        /// </summary>
        public String Token
        {
            get { return WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "Background"); }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                liHeader_Title.Text = ViewResourceText(String.Format("Header_Title_{0}", Token), String.Format("{0} Page Settings", Token));
            }



            String ContentFileName = "Manager_TabPictures.ascx";

            if (Token.IndexOf("Breadcrumb", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ContentFileName = "Manager_TabPictures.ascx";
            }
            else if (Token.IndexOf("MegaMenu", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ContentFileName = "Manager_TabMegamenu.ascx";
            }


            //加载相应的控件
            BaseModule ManageContent = new BaseModule();
            string ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, ContentFileName));
            if (System.IO.File.Exists(MapPath(ContentSrc)))
            {
                ManageContent = (BaseModule)LoadControl(ContentSrc);
                ManageContent.ModuleConfiguration = ModuleConfiguration;
                ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", ContentFileName));
                phPlaceHolder.Controls.Add(ManageContent);
            }
        }
    }
}