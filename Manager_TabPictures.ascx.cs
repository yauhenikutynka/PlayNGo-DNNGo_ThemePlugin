using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DotNetNuke.Entities.Tabs;
using System.Web.Script.Serialization;
 

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_TabPictures :BaseModule
    {
        public String Display = WebHelper.GetStringParam(HttpContext.Current.Request, "Display", "list");

        /// <summary>
        /// 文件类型
        /// </summary>
        public Int32 FileType = WebHelper.GetIntParam(HttpContext.Current.Request, "FileType", (Int32)EnumRelationshipsFileType.Background);


        /// <summary>
        /// 当前标签
        /// </summary>
        public String Token
        {
            get { return WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "Background"); }
        }

  


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



        private DNNGo_ThemePlugin_Menu _ContentItem;
        /// <Description>
        /// 文章项
        /// </Description>
        public DNNGo_ThemePlugin_Menu ContentItem
        {
            get
            {
                if (!(_ContentItem != null && _ContentItem.ID > 0))
                {
                    if (EditTabID > 0)
                    {
                        QueryParam qp = new QueryParam();
                        qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.TabID, EditTabID, SearchType.Equal));
                        qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.PortalId, PortalId, SearchType.Equal));
                        qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.Language, "en-US", SearchType.Equal));
                        _ContentItem = DNNGo_ThemePlugin_Menu.FindAllByItem(qp);
                    }
                    else
                        _ContentItem = new DNNGo_ThemePlugin_Menu();
                }
                return _ContentItem;
            }
        }



        /// <summary>
        /// 绑定相册集
        /// </summary>
        private void BindPictures(TabInfo Article)
        {
            List<String> SelectList = new List<String>();
            if (Article != null && Article.TabID > 0)
            {

               

                //填充相册集的关系
                List<DNNGo_ThemePlugin_Relationships> Relationships = DNNGo_ThemePlugin_Relationships.FindAllByID(EditTabID, PortalId, FileType);

                RepeaterFields.DataSource = Relationships;
                RepeaterFields.DataBind();

                foreach (DNNGo_ThemePlugin_Relationships Albums in Relationships)
                {
                    SelectList.Add(Albums.FileID.ToString());
                }
            }

            if (Article != null && Article.TabID > 0 && SelectList.Count > 0)
            {


                //绑定所有分类到页面
                QueryParam qp = new QueryParam();
                int RecordCount = 0;
                qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, PortalId, SearchType.Equal));
                qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.ID, Common.GetStringByList(SelectList), SearchType.In));
                List<DNNGo_ThemePlugin_Multimedia> lst = DNNGo_ThemePlugin_Multimedia.FindAll(qp, out RecordCount);

                //拼接顶级分类的方法
                StringBuilder sb = new StringBuilder();
                StringBuilder sbIDs = new StringBuilder();
                foreach (String FileID in SelectList)
                {
                    if (lst.Exists(r => r.ID.ToString() == FileID))
                    {
                        DNNGo_ThemePlugin_Multimedia PictureItem = lst.Find(r => r.ID.ToString() == FileID);
                        sb.AppendFormat("<tr data-value='{0}'>", PictureItem.ID).AppendLine();
                        sb.AppendFormat("<td>{0}<br/><img src=\"{1}\" style=\"max-width:120px; margin-right:15px;\"/></td>", PictureItem.FileName, GetPhotoExtension(PictureItem.FileExtension, PictureItem.FilePath)).AppendLine();
                        sb.AppendFormat("<td class=\"center\"><a  data-value='{0}' class=\"btn btn-xs btn-bricky tooltips\" href=\"javascript:;\" data-placement=\"top\" data-original-title=\"Delete\"><i class=\"fa fa-times fa fa-white\"></i></a></td>", PictureItem.ID).AppendLine();
                        sb.Append("</tr>").AppendLine();

                        sbIDs.AppendFormat("{0},", PictureItem.ID);
                    }
                }
                hfPictures.Value = sbIDs.ToString();
                liPictures.Text = sb.ToString();
            }

        }


        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetSortList()
        {

            //查询出当前字段的列表
            String jsonFields = WebHelper.GetStringParam(Request, nestable_output.UniqueID, "");
            if (!String.IsNullOrEmpty(jsonFields))
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<DNNGo_ThemePlugin_Relationships> json_fields = json.Deserialize<List<DNNGo_ThemePlugin_Relationships>>(jsonFields);
                if (json_fields != null && json_fields.Count > 0)
                {

                    List<DNNGo_ThemePlugin_Relationships> FieldLists = DNNGo_ThemePlugin_Relationships.FindAllByID(EditTabID,PortalId, FileType);

                    for (int i = 0; i < json_fields.Count; i++)
                    {
                        DNNGo_ThemePlugin_Relationships DBField = FieldLists.Find(r => r.ID == json_fields[i].ID);
                        if (DBField != null && DBField.ID > 0 && DBField.Sort != i)
                        {
                            DBField.Sort = i;
                            DBField.Update();

                        }


                    }
                }

            }




        }

        /// <summary>
        /// 绑定全局选项
        /// </summary>
        public void BindGlobal()
        {

            if (EditTabID != Int32.MaxValue)
            {
                DNNGo_ThemePlugin_Menu Item = ContentItem;


                div_GlobalSettings.Visible = true;
                WebHelper.BindList(rblUseGlobalSettings, typeof(EnumTrueFalse));

                if (Item != null && Item.ID > 0)
                {
                    if (Token.IndexOf("Background", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        WebHelper.SelectedListByValue(rblUseGlobalSettings, Item.Globals_Background);
                    }
                    else
                    {
                        WebHelper.SelectedListByValue(rblUseGlobalSettings, Item.Globals_Breadcrumb);
                    }
                }
                else
                {
                    WebHelper.SelectedListByValue(rblUseGlobalSettings, 1);
                }
            }
            else
            {
                div_GlobalSettings.Visible = false;
            }

  
        }

        /// <summary>
        /// 设置全局选项
        /// </summary>
        public void SetGlobal()
        {
            if (EditTabID != Int32.MaxValue)
            {
                DNNGo_ThemePlugin_Menu Item = ContentItem;


                if (Token.IndexOf("Background", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    Item.Globals_Background = WebHelper.GetIntParam(Request, rblUseGlobalSettings.UniqueID, 1);
                }
                else
                {
                    Item.Globals_Breadcrumb = WebHelper.GetIntParam(Request, rblUseGlobalSettings.UniqueID, 1);
                }


                Item.LastIP = WebHelper.UserHost;
                Item.LastTime = DateTime.Now;
                Item.LastUser = UserId;


                if (Item != null && Item.ID > 0)
                {
                    Item.Update();

                }
                else
                {
                    Item.PortalId = PortalId;
                    Item.Language = "en-US";
                    Item.ModuleId = ModuleId;
                    //Item.Position = (Int32)EnumPosition.Top;
                    Item.TabID = EditTabID;
                    Item.Insert();
                }
            }
            
        }


        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {

                    

                    if (TabItem != null && TabItem.TabID > 0)
                    {
                        
                        BindGlobal();

                        BindPictures(TabItem);
                        hlPictures.Attributes.Add("data-href", String.Format("{0}Resource_GalleryImage.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", ModulePath, PortalId, TabId, ModuleId, language));
                    }
                    else
                    {
                        mTips.LoadMessage("PageEmpty", EnumTips.Success, this, new String[] { "" });
                        Response.Redirect(xUrl("Pages"), false);
                    }

                    hlNavigation_PictureList.NavigateUrl = xUrl("EditTabID", EditTabID.ToString(), Token, "Display=list", "FileType=" + FileType.ToString());
                    hlNavigation_PictureSort.NavigateUrl = xUrl("EditTabID", EditTabID.ToString(), Token, "Display=sort", "FileType=" + FileType.ToString());
                }

                if (Display == "list")
                {
                    PanelPictureList.CssClass = "tab-pane in active";
                    liNavigation_PictureList.Attributes.Add("class", "active");
                }
                else
                {
                    PanelPictureSort.CssClass = "tab-pane in active";
                    liNavigation_PictureSort.Attributes.Add("class", "active");

                }

            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
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

                if (Display.ToLower() == "list")
                {
                    //设置全局选项
                    SetGlobal();
                    //更新附加图片集
                    String AlbumIDs = WebHelper.GetStringParam(Request, hfPictures.UniqueID, "");
                    DNNGo_ThemePlugin_Relationships.Update(TabItem.TabID,PortalId, AlbumIDs, FileType);
                    mTips.LoadMessage("SavePictureSuccess", EnumTips.Success, this, new String[] { TabItem.TabName });
                }
                else
                {
                    SetSortList();
                    mTips.LoadMessage("SortPictureSuccess", EnumTips.Success, this, new String[] { "" });
                }

         

                Response.Redirect(xUrl("EditTabID", EditTabID.ToString(), Token, "Display=" + Display, "FileType=" + FileType.ToString()), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl("Pages"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }


        /// <summary>
        /// 字段绑定事件
        /// </summary>
        protected void RepeaterFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DNNGo_ThemePlugin_Relationships FieldItem = e.Item.DataItem as DNNGo_ThemePlugin_Relationships;

                DNNGo_ThemePlugin_Multimedia PictureItem = DNNGo_ThemePlugin_Multimedia.FindByID(FieldItem.FileID);
                if (PictureItem != null && PictureItem.ID > 0)
                {
                    Literal liName = e.Item.FindControl("liName") as Literal;
                    liName.Text = PictureItem.FileName;

                    Image imgPicture = e.Item.FindControl("imgPicture") as Image;
                    imgPicture.ImageUrl = GetPhotoExtension(PictureItem.FileExtension, PictureItem.FilePath);

                }
            }
        }


        #endregion
    }
}