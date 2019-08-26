using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Common;


namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_TabMegamenu : BaseModule
    {



        #region "属性"


        private List<SettingEntity> _XmlSettings = new List<SettingEntity>();
        /// <summary>
        /// 获取参数列表
        /// </summary>
        public List<SettingEntity> XmlSettings
        {
            get
            {
                if (!(_XmlSettings != null && _XmlSettings.Count > 0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Resource/xml/MegamenuSettings.xml", ModulePath));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _XmlSettings = xf.ToList<SettingEntity>();
                    }
                }
                return _XmlSettings;
            }
        }



        private DNNGo_ThemePlugin_MenuOptions _ArticleItem;
        /// <Description>
        /// 文章项
        /// </Description>
        public DNNGo_ThemePlugin_MenuOptions ArticleItem
        {
            get
            {
                if (!(_ArticleItem != null && _ArticleItem.ID > 0))
                {
                    if (EditTabID > 0)
                    {
                        QueryParam qp = new QueryParam();
                        qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_MenuOptions._.TabID, EditTabID, SearchType.Equal));
                        _ArticleItem = DNNGo_ThemePlugin_MenuOptions.FindAllByItem(qp);
                    }
                    else
                        _ArticleItem = new DNNGo_ThemePlugin_MenuOptions();
                }
                return _ArticleItem;
            }
        }

        private List<KeyValueEntity> _ItemSettings;
        /// <Description>
        /// 封装的参数集合
        /// </Description>
        public List<KeyValueEntity> ItemSettings
        {
            get
            {
                if (!(_ItemSettings != null && _ItemSettings.Count > 0))
                {
                    if (ArticleItem != null && ArticleItem.ID > 0 && !String.IsNullOrEmpty(ArticleItem.Options))
                    {
                        try
                        {
                            _ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(ArticleItem.Options);
                        }
                        catch
                        {
                            _ItemSettings = new List<KeyValueEntity>();
                        }
                    }
                    else
                        _ItemSettings = new List<KeyValueEntity>();
                }
                return _ItemSettings;
            }
        }




        /// <summary>
        /// 页面的编号
        /// </summary>
        private Int32 EditTabID = WebHelper.GetIntParam(HttpContext.Current.Request, "EditTabID", 0);




    




        #endregion





        #region "方法"



        /// <summary>
        /// 绑定小窗口链接
        /// </summary>
        public void BingDataItems()
        {
            hlMegamenuModule_Top.Attributes.Add("data-href", ViewIframeUrl((Int32)EnumPosition.Top));
            hlMegamenuModule_Left.Attributes.Add("data-href", ViewIframeUrl((Int32)EnumPosition.Left));
            hlMegamenuModule_Right.Attributes.Add("data-href", ViewIframeUrl((Int32)EnumPosition.Right));
            hlMegamenuModule_Bottom.Attributes.Add("data-href", ViewIframeUrl((Int32)EnumPosition.Bottom));


            WebHelper.BindList(ddlTabType, typeof(EnumTabType));


            DNNGo_ThemePlugin_MenuOptions OptionItem = ArticleItem;

            WebHelper.SelectedListByValue(ddlTabType, OptionItem.TabType);


            
        }



        /// <summary>
        /// 绑定列表的方法
        /// </summary>
        public void BindDataList()
        {

            BingGroups(RepeaterOptions_Bottom);
            BingGroups(RepeaterOptions_Top);
            BingGroups(RepeaterOptions_Left);
            BingGroups(RepeaterOptions_Right);
            BingGroups(RepeaterOptions_Center);




        }


        /// <summary>
        /// 绑定分组
        /// </summary>
        public void BingGroups(Repeater repeater)
        {
            String Layout = repeater.ID.Replace("RepeaterOptions_", "");
            repeater.DataSource = XmlSettings.FindAll(r => r.Layout == Layout);
            repeater.DataBind();

        }

        /// <Description>
        /// 拼接数据项的设置参数
        /// </Description>
        /// <returns></returns>
        public String SetItemSettings()
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = XmlSettings;
            List<KeyValueEntity> list = new List<KeyValueEntity>();

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ControlHelper ControlItem = new ControlHelper(PortalId);

                foreach (SettingEntity ri in ItemSettingDB)
                {
                    KeyValueEntity item = new KeyValueEntity();
                    item.Key = ri.Name;
                    item.Value = ControlHelper.GetWebFormValue(ri, this);
                    list.Add(item);
                }
            }
            return ConvertTo.Serialize<List<KeyValueEntity>>(list);
        }

        #endregion





        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                try
                {
                    BindDataList();

                    if (!IsPostBack)
                    {
                        BingDataItems();
                    }
                }
                catch (Exception ex)
                {
                    DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
                }

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
                
                DNNGo_ThemePlugin_MenuOptions OptionItem = ArticleItem;

                OptionItem.TabType = WebHelper.GetIntParam(Request, ddlTabType.UniqueID, (Int32)EnumTabType.SliderNenu);
               
                OptionItem.Options = SetItemSettings();


                OptionItem.LastIP = WebHelper.UserHost;
                OptionItem.LastTime = DateTime.Now;
                OptionItem.LastUser = UserId;

                if (OptionItem.ID > 0)
                {
                    OptionItem.Update();
                }
                else
                {
                    OptionItem.TabID = EditTabID;
                    OptionItem.ModuleId = ModuleId;
                    OptionItem.PortalId = PortalId;
                    OptionItem.Insert();
                }



                mTips.IsPostBack = false;
                mTips.LoadMessage("UpdateSettingsSuccess", EnumTips.Success, this, new String[] { "" });

                Response.Redirect(  xUrl("EditTabID",EditTabID.ToString(),"MegaMenu"), false);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl(), false);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void RepeaterOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SettingEntity ThemeSetting = e.Item.DataItem as SettingEntity;

                KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key == ThemeSetting.Name);
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
                liTitle.Text = String.Format("<label class=\"col-sm-3 control-label\" for=\"{1}\">{0}:</label>", !String.IsNullOrEmpty(ThemeSetting.Alias) ? ThemeSetting.Alias : ThemeSetting.Name, ctl.ViewControlID(ThemeSetting));

                Literal liHelp = e.Item.FindControl("liHelp") as Literal;
                liHelp.Text = ThemeSetting.Description.Trim();
            }
        }

        /// <summary>
        /// 显示框架的网址
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        public String ViewIframeUrl(Int32 Position)
        {
            return String.Format("{0}Resource_MegaEdit.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&EditTabID={5}&Position={6}&Display=html", ModulePath, PortalId, TabId, ModuleId, language, EditTabID, Position);
        }

        #endregion



    }
}