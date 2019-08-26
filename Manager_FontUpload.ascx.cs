using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_FontUpload : BaseModule
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
         
            txtFontBold.Text = FontItem.Bold;
            txtFontFamily.Text = FontItem.Family;
            txtFontSubset.Text = FontItem.Subset;
            cbEnable.Checked = FontItem.Enable;

            //绑定字体
            BindFileUploadStyle("FontEot", FontItem.Font_File_Eot);
            BindFileUploadStyle("FontSvg", FontItem.Font_File_Svg);
            BindFileUploadStyle("FontTtf", FontItem.Font_File_Ttf);
            BindFileUploadStyle("FontWoff", FontItem.Font_File_Woff);

            
        }


        public void BindFileUploadStyle(String CName,String FontFileName)
        {
            Literal liFileStyle = this.FindControl(String.Format("li{0}Style", CName)) as Literal;
            if (liFileStyle != null)
            {
                if (!String.IsNullOrEmpty(FontFileName))
                {
                    liFileStyle.Text = "exists";
                }
                else
                {
                    liFileStyle.Text = "new";
                }
            }

            Literal liFile = this.FindControl(String.Format("li{0}", CName)) as Literal;
            if (liFile != null)
            {
                liFile.Text = FontFileName;
            }


            HiddenField hfFile = this.FindControl(String.Format("hf{0}", CName)) as HiddenField;
            if (hfFile != null)
            {
                hfFile.Value = FontFileName;
            }
        }

        /// <summary>
        /// 设置图片上传的控件
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="FontFileName"></param>
        /// <returns></returns>
        public String SetFileUpload(String CName, String FontFileName)
        {

            FileUpload fileup = this.FindControl(String.Format("file{0}", CName)) as FileUpload;
            if (fileup != null && fileup.HasFile)
            {
                //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                bool Valid = FileSystemUtils.CheckValidFileName(fileup.FileName);
                if (Valid)
                {
                    //存储图片并返回地址
                    String FontName = fileup.FileName.Replace(" ", "_");
                    String FontPath = MapPath(String.Format("{0}Fonts/{1}", SkinPath, FontName));
                    if (!new FileInfo(FontPath).Directory.Exists)
                    {
                        new FileInfo(FontPath).Directory.Create();
                    }


                    try
                    {
                        fileup.SaveAs(FontPath);
                        FontFileName = FontName;
                    }
                    catch
                    {

                    }
                }
            }
            else if (Request.Form[fileup.UniqueID] != null)
            {
                FontFileName = String.Empty;
            }


            return FontFileName;
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
               
                    FontItem.Bold = txtFontBold.Text;
                    FontItem.Family = txtFontFamily.Text;
                    FontItem.Subset = txtFontSubset.Text;

                    FontItem.Font_File_Eot= SetFileUpload("FontEot", FontItem.Font_File_Eot);
                    FontItem.Font_File_Svg = SetFileUpload("FontSvg", FontItem.Font_File_Svg);
                    FontItem.Font_File_Ttf = SetFileUpload("FontTtf", FontItem.Font_File_Ttf);
                    FontItem.Font_File_Woff = SetFileUpload("FontWoff", FontItem.Font_File_Woff);
                    FontItem.Enable = cbEnable.Checked;

                    if (String.IsNullOrEmpty(FontItem.PrimaryGuid))
                    {
                        FontItem.IsSystem = false;
                        FontItem.IsFontLink = false;
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