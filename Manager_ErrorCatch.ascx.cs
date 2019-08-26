using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_ErrorCatch : BaseModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //加载脚本
            if (!IsPostBack)
            {
                //接受报错信息
                if (HttpContext.Current.Session["Exception"] != null)
                {
                    Exception exc = HttpContext.Current.Session["Exception"] as Exception;
                    HttpContext.Current.Session.Remove("Exception");
                    liException.Text = exc.Message;
                  
                    DotNetNuke.Services.Exceptions.Exceptions.LogException(new ModuleLoadException(exc.Message.ToString(), exc, ModuleConfiguration));
                }
            }
        }


        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            try
            {
                String ReturnUrl = WebHelper.GetStringParam(Request, "ReturnUrl", "");
                Response.Redirect(ReturnUrl, false);
            }
            catch (Exception exc)
            {
                
            }
        }

    }
}