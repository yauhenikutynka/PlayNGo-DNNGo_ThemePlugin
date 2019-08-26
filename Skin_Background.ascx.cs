using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_Background : BaseSkin
    {

        #region "属性"



        private String _CssClass = "pic_tab";
        /// <summary>
        /// 样式表
        /// </summary>
        public String CssClass
        {
            get { return _CssClass; }
            set { _CssClass = value; }
        }


        private String _Token = "Background";
        /// <summary>
        /// 当前标记
        /// </summary>
        public String Token
        {
            get { return _Token; }
            set { _Token = value; }
        }



        private String _SkinClientID = String.Empty;
        /// <summary>
        /// 编号
        /// </summary>
        public String SkinClientID
        {
            get {
                if (String.IsNullOrEmpty(_SkinClientID))
                {
                    _SkinClientID = WebHelper.leftx(Guid.NewGuid().ToString("N"), 10);
                }
                return _SkinClientID; 
            }
        }


        private Int32 _Switchtime = 5000;

        public Int32 Switchtime
        {
            get { return _Switchtime; }
            set { _Switchtime = value; }
        }


        private Int32 _Animationtime = 1000;

        public Int32 Animationtime
        {
            get { return _Animationtime; }
            set { _Animationtime = value; }
        }


        private Int32 _Startpic = 0;

        public Int32 Startpic
        {
            get { return _Startpic; }
            set { _Startpic = value; }
        }



        private String _autoPaly = "true";

        public String autoPaly
        {
            get { return _autoPaly; }
            set { _autoPaly = value; }
        }


        private String _showArrow = "true";

        public String showArrow
        {
            get { return _showArrow; }
            set { _showArrow = value; }
        }
        #endregion


        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(skin_Error))
                {

                    if (!IsPostBack)
                    {
                        BindPageLoad();
                        Inits();
                    }

                    LoadScript("dnngo-ThemePlugin.js");
                    //LoadScript("phototabs.js");
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        /// <summary>
        /// 页面加载需要绑定的方法
        /// </summary>
        public void BindPageLoad()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<ul id=\"{0}\" class=\"{1}\">", SkinClientID, CssClass).AppendLine();

              DNNGo_ThemePlugin_Menu Item = GetGlobalSettings();

            EnumRelationshipsFileType FileType = Token.IndexOf("Breadcrumb", StringComparison.CurrentCultureIgnoreCase) >= 0 ? EnumRelationshipsFileType.Breadcrumb : EnumRelationshipsFileType.Background;
            Boolean isGlobal = (Token.IndexOf("Breadcrumb", StringComparison.CurrentCultureIgnoreCase) >= 0 ? Item.Globals_Breadcrumb : Item.Globals_Background) == 1;
 
            List<String> FileIDs = new List<String>();
            List<DNNGo_ThemePlugin_Relationships> Relationships = DNNGo_ThemePlugin_Relationships.FindAllViewByID(PortalSettings.ActiveTab.TabID, PortalSettings.PortalId, (Int32)FileType, isGlobal);
            foreach (DNNGo_ThemePlugin_Relationships Relationship in Relationships)
            {
                FileIDs.Add(Relationship.FileID.ToString());
            }


            if (FileIDs != null && FileIDs.Count > 0)
            {

                //绑定所有分类到页面
                QueryParam qp = new QueryParam();
                int RecordCount = 0;
                qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, PortalSettings.PortalId, SearchType.Equal));
                qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.ID, Common.GetStringByList(FileIDs), SearchType.In));
                List<DNNGo_ThemePlugin_Multimedia> lst = DNNGo_ThemePlugin_Multimedia.FindAll(qp, out RecordCount);

                foreach (String FileID in FileIDs)
                {
                    if (lst.Exists(r => r.ID.ToString() == FileID))
                    {
                        DNNGo_ThemePlugin_Multimedia PictureItem = lst.Find(r => r.ID.ToString() == FileID);
                        if (PictureItem != null && PictureItem.ID > 0)
                        {
                            sb.AppendFormat("<li style=\"background-image:url('{0}')\"></li>", GetPhotoPath( PictureItem.FilePath)).AppendLine();
                        }
                    }
                }
            }


            sb.Append("</ul>").AppendLine();
            liHTML.Text = sb.ToString();


            

        }

        /// <summary>
        /// 获取全局设置
        /// </summary>
        /// <returns></returns>
        public DNNGo_ThemePlugin_Menu GetGlobalSettings()
        {
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.TabID, PortalSettings.ActiveTab.TabID, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.Language, "en-US", SearchType.Equal));
            return  DNNGo_ThemePlugin_Menu.FindAllByItem(qp);
        }


    }
}