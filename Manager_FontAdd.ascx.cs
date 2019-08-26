
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;


namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_FontAdd : BaseModule
    {


        #region "====属性===="

        /// <summary>
        /// 主键
        /// </summary>
        public String PrimaryGuid = WebHelper.GetStringParam(HttpContext.Current.Request, "PrimaryGuid", "");






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

            FontDB FontItem = new FontDBHelper(this).Find(PrimaryGuid);

            txtFontAlias.Text = FontItem.Alias;
            txtFontUrl.Text = FontItem.FontUrl;
            txtFontBold.Text = FontItem.Bold;
            txtFontFamily.Text = FontItem.Family;
            txtFontSubset.Text = FontItem.Subset;
            cbEnable.Checked = FontItem.Enable;

            if (FontItem.IsSystem.HasValue && FontItem.IsSystem.Value)
            {
                cmdUpdate.Visible = false;
            }




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

                    BindItemToPage();
 
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
            

                //演示站点需要锁定功能
                if (IsAdministrator || !DemoLock)
                {


                    FontDB FontItem = new FontDBHelper(this).Find(PrimaryGuid);

                    FontItem.Alias = txtFontAlias.Text;
                    FontItem.FontUrl = txtFontUrl.Text;
                    FontItem.Bold = txtFontBold.Text;
                    FontItem.Family = txtFontFamily.Text;
                    FontItem.Subset = txtFontSubset.Text;
                    FontItem.Enable = cbEnable.Checked;

                    if (String.IsNullOrEmpty(FontItem.PrimaryGuid))
                    {
                        FontItem.IsSystem = false;
                        FontItem.IsFontLink = true;
                    }

                    new FontDBHelper(this).SaveCommit(FontItem);








                }
                mTips.LoadMessage("CopySkinSuccess", EnumTips.Success, this, new String[] { });

     

                Response.Redirect(xUrl("", "", "Fonts"), false);


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
                Response.Redirect(xUrl("", "", "Fonts"), false);
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }

        #endregion







    }
}