using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_SkinNew : BaseModule
    {


        #region "====属性===="

        /// <summary>
        /// 配置XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        public String ItemSettingXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Options.xml", SkinPath, SkinFileName)); }
        }
        /// <summary>
        /// 储值XML路径(需要定位到当前皮肤的位置)
        /// </summary>
        public String ItemValueXmlPath
        {
            get { return MapPath(String.Format("{0}xTemplate/{1}.Storage.xml", SkinPath, SkinFileName)); }
        }


        private List<SettingEntity> _OptionList = new List<SettingEntity>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SettingEntity> OptionList
        {
            get
            {
                if (!(_OptionList != null && _OptionList.Count > 0))
                {
                    XmlFormat xf = new XmlFormat(ItemSettingXmlPath);
                    _OptionList = xf.ToList<SettingEntity>();
                }
                return _OptionList;
            }
        }

        private List<KeyValueEntity> _ItemValues = new List<KeyValueEntity>();
        /// <summary>
        /// 存储的键值列表
        /// </summary>
        public List<KeyValueEntity> ItemValues
        {
            get
            {
                if (!(_ItemValues != null && _ItemValues.Count > 0))
                {
                    if (File.Exists(ItemValueXmlPath))
                    {
                        using (StreamReader sr = new StreamReader(ItemValueXmlPath, System.Text.Encoding.UTF8))
                        {

                            _ItemValues = ConvertTo.Deserialize<List<KeyValueEntity>>(sr.ReadToEnd());
                            sr.Close();
                            sr.Dispose();

                        }
                    }

                }
                return _ItemValues;
            }
        }





        #endregion


        #region "====方法===="

        /// <summary>
        /// 绑定数据到列表
        /// </summary>
        public void BindDataList()
        { //读取当前皮肤目录下的皮肤列表
            DirectoryInfo SkinDir = new DirectoryInfo(MapPath(String.Format("{0}xTemplate/", SkinPath)));
            if (SkinDir.Exists)
            {
                FileInfo[] skinFiles = SkinDir.GetFiles("*.ascx", SearchOption.TopDirectoryOnly);
                List<KeyValueEntity> list = new List<KeyValueEntity>();
                foreach (FileInfo file in skinFiles)
                {
                    String Name = file.Name.Replace(file.Extension, "");
                    list.Add(new KeyValueEntity(Name, Name));
                }
                //绑定皮肤列表
                WebHelper.BindList<KeyValueEntity>(ddlSkinFiles, list, "Key", "Value");

                WebHelper.SelectedListByValue(ddlSkinFiles, SkinFileName);

            }

        }

        /// <summary>
        /// 绑定元素到页面
        /// </summary>
        private void BindItemToPage()
        {

        }



        /// <summary>
        /// 更新模版
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="ItemValues"></param>
        public void UpdateTemplate(string templateName)
        {

            //以当前模版文件的参数为主,将当前接受的参数对比，有的就更新。

            List<SettingEntity> x_OptionList = _OptionList(templateName);
            List<KeyValueEntity> x_ItemValues = _ItemValues(templateName);


            Hashtable Puts = new Hashtable();
            TemplateFormat xf = new TemplateFormat(this);
            xf.ItemValues = x_ItemValues;
            xf.OptionList = x_OptionList;


            Puts.Add("ItemValues", x_ItemValues);
            Puts.Add("OptionList", x_OptionList);
            Puts.Add("SkinName", SkinName);
            Puts.Add("SkinFileName", templateName);

            //更新模版
            String ascxFileName = String.Format("{0}.ascx", templateName);
            String ascxContent = HttpUtility.HtmlDecode(ViewTemplate(ascxFileName, Puts, xf));
            WriteTemplate(ascxFileName, ascxContent);

            //更新CSS
            String cssFileName = String.Format("{0}.css", templateName);
            string cssContent = HttpUtility.HtmlDecode(ViewTemplate(cssFileName, Puts, xf));
            WriteTemplate(cssFileName, cssContent);
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
                String SourceSkin = WebHelper.GetStringParam(Request, ddlSkinFiles.UniqueID, "");
                String NewSkin = txtNewSkin.Text.Trim();

                //演示站点需要锁定功能
                if (IsAdministrator || !DemoLock)
                {
                    //将源皮肤及文件更名后复制到目标路径
                    FileSystemUtils.CopyTemplateToNew(MapPath(SkinPath), SourceSkin, NewSkin);


                    //从模版中复制到皮肤目录
                    FileSystemUtils.CopyTemplate(MapPath(SkinPath), NewSkin);

                    //生成新的模版
                    UpdateTemplate(NewSkin);

                }
                mTips.LoadMessage("CopySkinSuccess", EnumTips.Success, this, new String[] { });

                //String GoUrl = String.Format("{0}?PortalId={1}&TabID={2}&Token={3}&SkinFileName={4}#tabs-box-{3}", "Resource_Options.aspx", PortalId, TabId, "Options", NewSkin);

                Response.Redirect(xUrl("", "", "Skin_Options", "SkinFileName=" + HttpUtility.UrlEncode(NewSkin), "SkinPath=" + HttpUtility.UrlEncode(SkinPath)), false);


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
                Response.Redirect(xUrl("Skin_Options"), false);
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }

        #endregion







    }
}