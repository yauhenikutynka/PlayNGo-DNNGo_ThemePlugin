using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using System.Text;
using System.Web.Script.Serialization;
using DotNetNuke.Entities.Portals;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_TabIconEdit : BaseModule
    {


        #region "====属性===="





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
            get
            {
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
                return _TabItem;
            }
        }


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
          

            ControlHelper ctl = new ControlHelper(this);
            SettingEntity ThemeSetting = new SettingEntity();
            ThemeSetting.Name = "Icons";
            ThemeSetting.Width = 250;
            ThemeSetting.ControlType = EnumControlType.IconPicker.ToString();
            ThemeSetting.DefaultValue = GetPortalSetting(String.Format("ThemePlugin_TabIcon_{0}", EditTabID), "");
            phControl.Controls.Add((Control)ctl.ViewControl(ThemeSetting));
        

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
                 SettingEntity ThemeSetting = new SettingEntity();
            ThemeSetting.Name = "Icons";
            ThemeSetting.ControlType = EnumControlType.IconPicker.ToString();


                UpdatePortalSetting(String.Format("ThemePlugin_TabIcon_{0}", EditTabID), ControlHelper.GetWebFormValue(ThemeSetting,this));

                mTips.LoadMessage("SaveIconSuccess", EnumTips.Success, this, new String[] { });

                Response.Redirect(xUrl("Pages"), false);


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
                Response.Redirect(xUrl("Pages"), false);
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }


 

        #endregion







    }
}