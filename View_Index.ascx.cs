using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;

using DotNetNuke.Common;

using DotNetNuke.Security.Permissions;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class View_Index : BaseModule, DotNetNuke.Entities.Modules.IActionable
    {



        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    panNavigation.Visible = UserId > 0 && ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration);
                    hlManager.NavigateUrl = xUrl();
 
                }
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {

   
                LoadViewScript();

                //绑定Tabs和容器中的控件
                BindContainer();

            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }



        /// <summary>
        /// 页面最后加载
        /// </summary>
        protected void Page_PreRender(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
 
                }
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        


        #endregion

        #region "方法"

 

        /// <summary>
        /// 绑定列表数据到容器
        /// </summary>
        private void BindContainer()
        {
 
        }

        #endregion

    

        #region Optional Interfaces
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();

  

                return Actions;
            }
        }
        #endregion
    }
}