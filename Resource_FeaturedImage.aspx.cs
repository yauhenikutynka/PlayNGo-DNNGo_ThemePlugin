using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Resource_FeaturedImage1 : BasePage
    {

        public String Display = WebHelper.GetStringParam(HttpContext.Current.Request, "Display", "list");

        /// <summary>
        /// 插入状态
        /// </summary>
        public Boolean InsertType = Convert.ToBoolean(WebHelper.GetStringParam(HttpContext.Current.Request, "Insert", "false"));

        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            base.Page_Init(sender, e);
 
            if (!IsPostBack)
            {
                

                hlNavigation_add.NavigateUrl = String.Format("{0}Resource_FeaturedImage.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&Insert={5}&Display=add", ModulePath, PortalId, TabId, ModuleId, language, InsertType);
                hlNavigation_list.NavigateUrl = String.Format("{0}Resource_FeaturedImage.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&Insert={5}&Display=list", ModulePath, PortalId, TabId, ModuleId, language, InsertType);

              
            }
            if (Display == "list")
            {
                panel_tab3_example2.CssClass = "tab-pane in active";
                liNavigation_list.Attributes.Add("class", "active");
                //Resource_DropzoneUpload1.Visible = false;
                //载入模块
                LoadModule("Resource_FeaturedImage.ascx", ref phFeaturedImage);

            }
            else
            {
                panel_tab3_example1.CssClass = "tab-pane in active";
                liNavigation_add.Attributes.Add("class", "active");
                //Resource_FeaturedImage2.Visible = false;
                //载入模块
                LoadModule("Resource_DropzoneUpload.ascx", ref phDropzoneUpload);
            }
        }
    }
}