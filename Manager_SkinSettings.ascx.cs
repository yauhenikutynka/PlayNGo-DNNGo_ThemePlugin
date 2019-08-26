using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Entities.Portals;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_SkinSettings : BaseModule
    {


        #region "====属性===="

 




        #endregion


        #region "====方法===="

        /// <summary>
        /// 绑定数据到列表
        /// </summary>
        public void BindDataList()
        {

        }

        /// <summary>
        /// 绑定元素到页面
        /// </summary>
        private void BindItemToPage()
        {
            WebHelper.BindList(rblScope, typeof(EnumScope));
            WebHelper.SelectedListByValue(rblScope, GetPortalSetting("ThemePlugin_Scope", ((Int32)EnumScope.Single).ToString()));


            cbShowIconAdmin.Checked = Convert.ToBoolean(GetPortalSetting("ThemePlugin_ShowIcon_Admin", "true"));
            cbShowIconHost.Checked = Convert.ToBoolean(GetPortalSetting("ThemePlugin_ShowIcon_Host", "true"));

        }








        #endregion




        #region "====事件===="


        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {


                    //BindItemToPage();

                    BindDataList();

                    //演示站点需要锁定功能
                    if (!IsAdministrator && DemoLock)
                    {
                        cmdUpdate.Enabled = false;
                    }
                }

            }
            catch (Exception exc) //Module failed to load
            {
                ProcessModuleLoadException(exc);
            }
        }
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindItemToPage();
                }
                //加载脚本
                //LoadViewScript();

            }
            catch (Exception exc) //Module failed to load
            {
                ProcessModuleLoadException(exc);
            }
        }


        /// <summary>
        /// 更新内容
        /// </summary>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                PortalController.UpdatePortalSetting(PortalId, "ThemePlugin_ShowIcon_Host", cbShowIconHost.Checked.ToString());
                PortalController.UpdatePortalSetting(PortalId, "ThemePlugin_ShowIcon_Admin", cbShowIconAdmin.Checked.ToString());


                UpdatePortalSetting("ThemePlugin_Scope", WebHelper.GetStringParam(Request, rblScope.UniqueID, ((Int32)EnumScope.Global).ToString()));

                mTips.LoadMessage("SaveOptionsSuccess", EnumTips.Success, this, new String[] { });

                Response.Redirect(xUrl("Skin_Settings"), false);


            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }




        /// <summary>
        /// 取消
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(xUrl("Skin_Settings"), false);
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }



        /// <summary>
        /// 重新初始化
        /// </summary>
        protected void cmdReInit_Click(object sender, EventArgs e)
        {
            try
            {

                PortalController.UpdatePortalSetting(PortalId, "ThemePlugin_Init_ThemePlugin_TabID", TabId.ToString());
                PortalController.UpdatePortalSetting(PortalId, "ThemePlugin_Init_ThemePlugin_ModuleID", ModuleId.ToString());

                mTips.LoadMessage("ReInitSuccess", EnumTips.Success, this, new String[] { });

                Response.Redirect(xUrl("Skin_Settings"), false);

   
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }


        #endregion







    }
}