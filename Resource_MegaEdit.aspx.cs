using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Resource_MegaEdit : BasePage
    {

        public String Display = WebHelper.GetStringParam(HttpContext.Current.Request, "Display", "html");

        /// <summary>
        /// 页面的编号
        /// </summary>
        private Int32 EditTabID = WebHelper.GetIntParam(HttpContext.Current.Request, "EditTabID", 0);


        /// <summary>
        /// 方位
        /// </summary>
        private Int32 Position = WebHelper.GetIntParam(HttpContext.Current.Request, "Position", 0);


        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              

                hlNavigation_html.NavigateUrl = ViewIframeUrl("html");
                hlNavigation_module.NavigateUrl = ViewIframeUrl("module");


            }
          
        }

        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            base.Page_Init(sender, e);
  
            try
            {
                if (Display == "module")
                {
                    panel_tab3_example2.CssClass = "tab-pane in active";
                    liNavigation_module.Attributes.Add("class", "active");

                    //载入模块
                    LoadModule("Resource_MegaEdit_Module.ascx", ref phModule);

                }
                else
                {
                    panel_tab3_example1.CssClass = "tab-pane in active";
                    liNavigation_html.Attributes.Add("class", "active");

                    //载入模块
                    LoadModule("Resource_MegaEdit_Html.ascx", ref phHTML);

                }

            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }




        /// <summary>
        /// 显示框架的网址
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        public String ViewIframeUrl(String _Display)
        {
            return String.Format("{0}Resource_MegaEdit.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&EditTabID={5}&Position={6}&Display={7}", ModulePath, PortalId, TabId, ModuleId, language, EditTabID, Position, _Display);
        }
    }
}