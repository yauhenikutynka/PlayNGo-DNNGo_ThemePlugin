using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Skins;

using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_HSlide : SlideNavObjectBase
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    //绑定数据项到前台
                    this.OnViewMenuBind();
                }
                //LoadScript("Slide.js");
                LoadScript("dnngo-ThemePlugin.js");
                
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }


 



          
        }

        protected void OnViewMenuBind()
        {
            List<TabInfo> userTabs = GetUserTabs(base.PortalSettings, UserController.GetCurrentUserInfo());
            this.LiContent.Text = this.ViewMenuBind(userTabs);
        }


    }
}