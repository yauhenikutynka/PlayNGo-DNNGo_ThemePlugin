using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_GlobalOptions : BaseModule
    {


     


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
            
        }


        /// <summary>
        /// 绑定顶部的菜单列表
        /// </summary>
        public void BindMenuList(String Layout)
        {
            Literal liNavTabsHTML = FindControl(String.Format("liNavTabsHTML_{0}", Layout)) as Literal;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<String> TempList = new List<String>();

            List<SettingEntity> ItemSettingDB = GlobalOptionList.FindAll(r => r.Layout == Layout);
            if (Layout != "Right")
            {
                ItemSettingDB = GlobalOptionList.FindAll(r => r.Layout != "Right");
            }

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                foreach (SettingEntity item in ItemSettingDB)
                {
                    if (!TempList.Exists(r1 => r1 == item.Categories))
                    {
                        String active = TempList.Count == 0 ? "active" : "";
                        sb.AppendFormat("<li class=\"{0}\"><a href=\"#tabs-{3}-{1}\"  data-toggle=\"tab\"  title=\"{2}\">{2}</a></li>", active, FormatName(item.Categories), item.Categories, Layout).AppendLine();
                        TempList.Add(item.Categories);
                    }
                }
            }
            liNavTabsHTML.Text = sb.ToString();
        }


        /// <summary>
        /// 拼接数据项的设置参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public String SetItemSettings(ref List<KeyValueEntity> list)
        {
            return SetItemSettings(ref list, GlobalOptionList);
        }

        /// <summary>
        /// 拼接数据项的设置参数
        /// </summary>
        /// <returns></returns>
        public String SetItemSettings(ref List<KeyValueEntity> list, List<SettingEntity> x_OptionList)
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = x_OptionList;
             

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ControlHelper ControlItem = new ControlHelper(PortalId);

                foreach (SettingEntity ri in ItemSettingDB)
                {
                    KeyValueEntity OldKV = ItemValues.Find(r1=>r1.Key == ri.Name);
            
                    if (OldKV != null && !String.IsNullOrEmpty(OldKV.Value))
                    {
                        ri.DefaultValue = OldKV.Value;
                    }

                    KeyValueEntity item = new KeyValueEntity();
                    item.Key = ri.Name;
                    item.Value = ControlHelper.GetWebFormValue(ri,this);
                    list.Add(item);
                }
            }
            return ConvertTo.Serialize<List<KeyValueEntity>>(list);
        }


                /// <summary>
        /// 重置数据项参数
        /// </summary>
        /// <returns></returns>
        public String ResetItemSettings(ref List<KeyValueEntity> list)
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = GlobalOptionList;
            return ResetItemSettings(ref list, ItemSettingDB);
        }

        /// <summary>
        /// 重置数据项参数
        /// </summary>
        /// <returns></returns>
        public String ResetItemSettings(ref List<KeyValueEntity> list, List<SettingEntity> ItemSettingDB)
        {
            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ControlHelper ControlItem = new ControlHelper(PortalId);

                foreach (SettingEntity ri in ItemSettingDB)
                {
                    KeyValueEntity item = new KeyValueEntity();
                    item.Key = ri.Name;
                    item.Value = ri.DefaultValue;
                    list.Add(item);
                }
            }
            return ConvertTo.Serialize<List<KeyValueEntity>>(list);
        }



        /// <summary>
        /// 批量更新模版
        /// </summary>
        public void BulkUpdateTemplate(List<KeyValueEntity> __ItemValues)
        {
            //找出所有的模版批量的进行处理

            FileInfo file = new FileInfo(MapPath(String.Format("{0}xtemplate/", SkinPath)));
            FileInfo[] files = file.Directory.GetFiles("*.ascx");
            if (files != null && files.Length > 0)
            {
                BulkUpdateTemplate("Global", __ItemValues);

                foreach (FileInfo templatefile in files)
                {
                    if (templatefile.Exists)
                    {
                        String templateName = templatefile.Name.Replace(String.Format("{0}", templatefile.Extension), "");
                        if (!String.IsNullOrEmpty(templateName))
                        {
                            BulkUpdateTemplate(templateName, __ItemValues);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 批量更新模版
        /// </summary>
        /// <param name="templateName">模版名称</param>
        public void BulkUpdateTemplate(string templateName,  List<KeyValueEntity> objItemValues)
        {

            if (templateName == "Global")
            {
                //以当前模版文件的参数为主,将当前接受的参数对比，有的就更新。
                List<KeyValueEntity> new_ItemValues = new List<KeyValueEntity>();
                List<SettingEntity> x_OptionList = _OptionList(templateName);
                List<KeyValueEntity> x_ItemValues = _ItemValues(templateName);

                foreach (SettingEntity item in x_OptionList)
                {
                    KeyValueEntity new_Value = x_ItemValues.Find(r => r.Key == item.Name);
                    if (!(new_Value != null && !String.IsNullOrEmpty(new_Value.Key)))
                    {
                        new_Value = new KeyValueEntity();
                        new_Value.Key = item.Name;
                        new_Value.Value = item.DefaultValue;
                    }

                    if (objItemValues.Exists(r => r.Key == item.Name))
                    {
                        KeyValueEntity old_value = objItemValues.Find(r => r.Key == item.Name);
                        if (old_value != null && !String.IsNullOrEmpty(old_value.Key))
                        {
                            new_Value.Value = old_value.Value;
                        }
                    }

                    new_ItemValues.Add(new_Value);
                }

                GlobalValues = new_ItemValues;

                //保存参数到XML
                WriteTextToFile(_ItemValueXmlPath(templateName), ConvertTo.Serialize<List<KeyValueEntity>>(new_ItemValues));
            }
            else
            {

                Hashtable Puts = new Hashtable();
                TemplateFormat xf = new TemplateFormat(this);
                xf.GlobalValues = GlobalValues;
                xf.GlobalOptionList = GlobalOptionList;

                xf.ItemValues = ItemValues;
                xf.OptionList = OptionList;

                Puts.Add("ItemValues", ItemValues);
                Puts.Add("OptionList", OptionList);
                Puts.Add("SkinName", SkinName);
                Puts.Add("SkinFileName", templateName);


                Puts.Add("GlobalValues", GlobalValues);
                Puts.Add("GlobalOptionList", GlobalOptionList);

                //更新模版
                String ascxFileName = String.Format("{0}.ascx", templateName);
                String ascxContent = HttpUtility.HtmlDecode(ViewTemplate(ascxFileName, Puts, xf));
                WriteTemplate(ascxFileName, ascxContent);

                //更新CSS
                String cssFileName = String.Format("{0}.css", templateName);
                string cssContent = HttpUtility.HtmlDecode(ViewTemplate(cssFileName, Puts, xf));
                WriteTemplate(cssFileName, cssContent);
            }




          
          


        }

        /// <summary>
        /// 批量重置模版
        /// </summary>
        public void BulkResetTemplate()
        {
            //找出所有的模版批量的进行处理

            FileInfo file = new FileInfo(MapPath(String.Format("{0}xtemplate/", SkinPath)));
            FileInfo[] files = file.Directory.GetFiles("*.ascx");
            if (files != null && files.Length > 0)
            {
                BulkResetTemplate("Global");

                foreach (FileInfo templatefile in files)
                {
                    if (templatefile.Exists)
                    {
                        String templateName = templatefile.Name.Replace(String.Format("{0}", templatefile.Extension), "");
                     
                        BulkResetTemplate(templateName);
                 
                    }
                }
            }
        }

        public void BulkResetTemplate(String templateName)
        {
            List<KeyValueEntity> list = new List<KeyValueEntity>();
            List<SettingEntity> ItemSettingDB = _OptionList(templateName);
            if (templateName == "Global")
            {
                WriteTextToFile(_ItemValueXmlPath(templateName), ResetItemSettings(ref list, ItemSettingDB));
            }else
            {
                Hashtable Puts = new Hashtable();
                TemplateFormat xf = new TemplateFormat(this);
                xf.ItemValues = list;
                xf.OptionList = ItemSettingDB;

                xf.GlobalValues = GlobalValues;
                xf.GlobalOptionList = GlobalOptionList;


                Puts.Add("ItemValues", list);
                Puts.Add("OptionList", ItemSettingDB);
                Puts.Add("SkinName", SkinName);
                Puts.Add("SkinFileName", templateName);

                Puts.Add("GlobalValues", GlobalValues);
                Puts.Add("GlobalOptionList", GlobalOptionList);

                //更新模版
                String ascxFileName = String.Format("{0}.ascx", templateName);
                String ascxContent = HttpUtility.HtmlDecode(ViewTemplate(ascxFileName, Puts, xf));
                WriteTemplate(ascxFileName, ascxContent);

                //更新CSS
                String cssFileName = String.Format("{0}.css", templateName);
                string cssContent = HttpUtility.HtmlDecode(ViewTemplate(cssFileName, Puts, xf));
                WriteTemplate(cssFileName, cssContent);



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
                BindCategoriesToPage("Left");
                BindCategoriesToPage("Right");
 

                if (!IsPostBack)
                {

                    BindItemToPage();

                    BindMenuList("Left");
                    BindMenuList("Right");

                    BindDataList();

             

               

                    //演示站点需要锁定功能
                    if (!IsAdministrator && DemoLock)
                    {
                        cmdUpdate.Enabled = cmdReset.Enabled = false;
                    }

                    cmdReset.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("Reset_Confirm", "Are you sure to reset the skin settings?") + "');");

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


                    //保存参数到XML
                    List<KeyValueEntity> __ItemValues = new List<KeyValueEntity>();
                    String ItemSettings = SetItemSettings(ref __ItemValues);
                    WriteTextToFile(GlobalValueXmlPath, ItemSettings);

                    //批量更新模板
                    BulkUpdateTemplate(__ItemValues);

  


                    //更新引用文件版本
                    IncrementCrmVersion();

                    ////复制文件
                    //FileSystemUtils.CopyTemplate(MapPath(SkinPath), SkinFileName);
                }


                    mTips.LoadMessage("SaveOptionsSuccess", EnumTips.Success, this, new String[] { });

                    Response.Redirect(WebHelper.GetScriptUrl, false);
                    //String GoUrl = String.Format("{0}?PortalId={1}&TabID={2}&Token={3}&SkinFileName={4}#tabs-box-{3}", "Resource_Options.aspx", PortalId, TabId, "Options", "");

                    //Response.Redirect(GoUrl,false);
                
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                //演示站点需要锁定功能
                if (IsAdministrator || !DemoLock)
                {


                    //批量重置
                    BulkResetTemplate();

                }
                mTips.LoadMessage("ResetOptionsSuccess", EnumTips.Success, this, new String[] { });

                Response.Redirect(WebHelper.GetScriptUrl,false);
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
 
            }
            catch (Exception exc)
            {
                ProcessModuleLoadException(exc);
            }
        }

        #endregion


        #region "===参数列表三层嵌套代码==="


        protected void RepeaterOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SettingEntity ThemeSetting = (e.Item.DataItem as SettingEntity).Clone();

                KeyValueEntity KeyValue = GlobalValues.Find(r1 => r1.Key == ThemeSetting.Name);
                if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                {
                    ThemeSetting.DefaultValue = KeyValue.Value.ToString();
                }

                //构造输入控件
                PlaceHolder ThemePH = e.Item.FindControl("ThemePH") as PlaceHolder;

                #region "创建控件"
                ControlHelper ctl = new ControlHelper(this);

                ThemePH.Controls.Add((Control)ctl.ViewControl(ThemeSetting));
                #endregion

                Literal liTitle = e.Item.FindControl("liTitle") as Literal;
                liTitle.Text = ViewTitleAndHelp(ThemeSetting);


                if (!String.IsNullOrEmpty(ThemeSetting.Description))
                {
                    Literal liHelp = e.Item.FindControl("liHelp") as Literal;
                    liHelp.Text = String.Format("<span class=\"help-block\"><i class=\"fa fa-info-circle\"></i> {0}</span>", ThemeSetting.Description);
                }

 
           
            }
        }


        /// <summary>
        /// 分组绑定事件
        /// </summary>
        protected void RepeaterGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater RepeaterOptions = e.Item.FindControl("RepeaterOptions") as Repeater;
                KeyValueEntity GroupItem = e.Item.DataItem as KeyValueEntity;
                int OptionCount = 0;
                BindOptionsToPage(RepeaterOptions, GroupItem.Key, out OptionCount);
               
                if (OptionCount == 0)
                {
                    e.Item.Visible = false;
                }

            }
        }


        /// <summary>
        /// 分组绑定事件
        /// </summary>
        protected void RepeaterCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater RepeaterGroup = e.Item.FindControl("RepeaterGroup") as Repeater;
                KeyValueEntity CategoriesItem = e.Item.DataItem as KeyValueEntity;
                int OptionCount = 0;
                BindGroupToPage(RepeaterGroup, CategoriesItem.Key, out OptionCount);
                
                if (OptionCount == 0)
                {
                    e.Item.Visible = false;
                }

            }
        }





        /// <summary>
        /// 绑定选项分组框到页面
        /// </summary>
        private void BindCategoriesToPage(String Layout)
        {


            Repeater RepeaterCategories = FindControl(String.Format("RepeaterCategories_{0}", Layout)) as Repeater;
    

            if (RepeaterCategories != null)
            {
                List<SettingEntity> ItemSettingDB = GlobalOptionList.FindAll(r => r.Layout == Layout);
                if (Layout != "Right")
                {
                    ItemSettingDB = GlobalOptionList.FindAll(r => r.Layout != "Right");
                }
 

                List<KeyValueEntity> Items = new List<KeyValueEntity>();
                Items.Add(new KeyValueEntity("Basic Categories", ""));
                foreach (SettingEntity ItemSetting in ItemSettingDB)
                {
                    if (!Items.Exists(r1 => r1.Key == ItemSetting.Categories))
                    {
                        Items.Add(new KeyValueEntity(ItemSetting.Categories, ""));
                    }
                }

                if (Items != null && Items.Count > 0)
                {
                    //绑定参数项
                    RepeaterCategories.DataSource = Items;
                    RepeaterCategories.DataBind();
                }
            }
        }

 



        /// <summary>
        /// 绑定选项分组框到页面
        /// </summary>
        private void BindGroupToPage(Repeater RepeaterGroup, String Categories, out int OptionCount)
        {
            OptionCount = 0;
            //获取效果参数
            List<SettingEntity> ItemSettingDB = GlobalOptionList;

            List<KeyValueEntity> Items = new List<KeyValueEntity>();
            Items.Add(new KeyValueEntity("Basic Options", ""));

            ItemSettingDB = ItemSettingDB.FindAll(r1 => r1.Categories == Categories);
            OptionCount = ItemSettingDB.Count;

            foreach (SettingEntity ItemSetting in ItemSettingDB)
            {
                if (!Items.Exists(r1 => r1.Key == ItemSetting.Group))
                {
                    Items.Add(new KeyValueEntity(ItemSetting.Group, "", Categories));
                }
            }


            if (Items != null && Items.Count > 0)
            {
               
                //绑定参数项
                RepeaterGroup.DataSource = Items;
                RepeaterGroup.DataBind();
            }
        }




        /// <summary>
        /// 绑定选项集合到页面
        /// </summary>
        private void BindOptionsToPage(Repeater RepeaterOptions, String Group, out int OptionCount)
        {
            OptionCount = 0;
            //获取效果参数
            List<SettingEntity> ItemSettingDB = GlobalOptionList;

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ItemSettingDB = ItemSettingDB.FindAll(r1 => r1.Group == Group);
                OptionCount = ItemSettingDB.Count;
                //绑定参数项
                RepeaterOptions.DataSource = ItemSettingDB;
                RepeaterOptions.DataBind();
            }
        }

        #endregion



 
    }
}