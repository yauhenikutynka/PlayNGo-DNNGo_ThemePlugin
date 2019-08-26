using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_Option_jQuery : BaseModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();




        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {




            #region "jQuery设置选项"

            cbjQueryEnable.Checked = Settings["ThemePlugin_jQuery_Enable"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_Enable"].ToString()) ? Convert.ToBoolean(Settings["ThemePlugin_jQuery_Enable"]) : false;
            cbUseHostedjQuery.Checked = Settings["ThemePlugin_jQuery_UseHosted"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_UseHosted"].ToString()) ? Convert.ToBoolean(Settings["ThemePlugin_jQuery_UseHosted"]) : false;


            txtHostedjQuery.Text = Settings["ThemePlugin_jQuery_HostedjQuery"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_HostedjQuery"].ToString()) ? Convert.ToString(Settings["ThemePlugin_jQuery_HostedjQuery"]) : "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js";
            txtHostedjQueryUI.Text = Settings["ThemePlugin_jQuery_HostedjQueryUI"] != null && !string.IsNullOrEmpty(Settings["ThemePlugin_jQuery_HostedjQueryUI"].ToString()) ? Convert.ToString(Settings["ThemePlugin_jQuery_HostedjQueryUI"]) : "https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js";


            #endregion

        }



        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {



            #region "验证码相关设置"
            UpdateModuleSetting("ThemePlugin_jQuery_Enable", cbjQueryEnable.Checked.ToString());
            UpdateModuleSetting("ThemePlugin_jQuery_UseHosted", cbUseHostedjQuery.Checked.ToString());
            UpdateModuleSetting("ThemePlugin_jQuery_HostedjQuery", txtHostedjQuery.Text.Trim());
            UpdateModuleSetting("ThemePlugin_jQuery_HostedjQueryUI", txtHostedjQueryUI.Text.Trim());




            #endregion

        }








        #endregion


        #region "==事件=="


        /// <summary>
        /// 页面加载事件
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //绑定数据
                    BindDataToPage();
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException( ex);
            }
        }


        /// <summary>
        /// 更新绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // 设置需要绑定的方案项
                SetDataItem();

                mTips.LoadMessage("UpdateSettingsSuccess", EnumTips.Success, this, new String[] { "" });

                //refresh cache
                SynchronizeModule();

                Response.Redirect(xUrl("jQuerySettings"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException( ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl("Setting"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException( ex);
            }
        }


        #endregion









    }
}