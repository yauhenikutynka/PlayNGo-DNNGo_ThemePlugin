using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI;
using DotNetNuke.UI.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Skin_Megamenu : BaseNavObject
    {


        #region "属性"

        /// <summary>
        /// 自定义编号
        /// </summary>
        public String MenuID = WebHelper.leftx(Guid.NewGuid().ToString("N"), 10);


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
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Resource/xml/Settings.xml", BasePath));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _XmlSettings = xf.ToList<SettingEntity>();
                    }
                }
                return _XmlSettings;
            }
        }


        private String _slide_speed = "200";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String slide_speed
        {
            get { return _slide_speed; }
            set { _slide_speed = value; }
        }


        private String _delay_disappear = "500";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String delay_disappear
        {
            get { return _delay_disappear; }
            set { _delay_disappear = value; }
        }


        private String _popUp = "vertical";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String popUp
        {
            get { return _popUp; }
            set { _popUp = value; }
        }





        private String _direction = "ltr";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private String _megamenuwidth = "box";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String megamenuwidth
        {
            get { return _megamenuwidth; }
            set { _megamenuwidth = value; }
        }

        private String _WidthBoxClassName = ".dnn_layout";
        /// <summary>
        /// JS打印到页面的参数
        /// </summary>
        public String WidthBoxClassName
        {
            get { return _WidthBoxClassName; }
            set { _WidthBoxClassName = value; }
        }

        private List<TabInfo> _AllTabs = new List<TabInfo>();
        /// <summary>
        /// 所有菜单
        /// </summary>
        public List<TabInfo> AllTabs
        {
            get
            {
                if (!(_AllTabs != null && _AllTabs.Count > 0))
                {
                    _AllTabs = GetUserTabs(PortalSettings, PortalSettings.UserInfo);
                }
                return _AllTabs;
            }
        }


        private List<TabInfo> _AllActiveTab = new List<TabInfo>();
        /// <summary>
        /// 所有激活的菜单
        /// </summary>
        public List<TabInfo> AllActiveTab
        {
            get
            {
                if (!(_AllActiveTab != null && _AllActiveTab.Count > 0))
                {
                    _AllActiveTab = FindAllTabByActive(AllTabs);
                }
                return _AllActiveTab;
            }
        }


        #endregion

        #region "方法"

        /// <summary>
        /// 绑定顶级菜单
        /// </summary>
        public void BindTopMenu()
        {
            List<TabInfo> allTabs = AllTabs;



            //查找当前活动菜单的顶级菜单
            TabInfo ActiveTopTab = FindTopTabByActive(allTabs);

            Int32 TabTabID = GetTopTab(allTabs, PortalSettings.ActiveTab);

            BindString(" <ul class=\"primary_structure\">");
            //循环绑定一级菜单
            List<TabInfo> TopTabs = allTabs.FindAll(r => r.ParentId == TabTabID && r.IsVisible);
            foreach (TabInfo TopTab in TopTabs)
            {


                DNNGo_ThemePlugin_Menu MenuOptions = new DNNGo_ThemePlugin_Menu();
                Boolean trueMegaType = TrueMegaType(TopTab, out MenuOptions);

                //检测有无下级目录
                Boolean TrueSub = allTabs.Exists(r => r.ParentId == TopTab.TabID && r.IsVisible);



                BindString(String.Format(" <li class=\"{0} {1}\">", TrueSub || trueMegaType ? "dir" : "", ActiveTopTab != null && ActiveTopTab.TabID == TopTab.TabID ? "current" : ""));

                //打印顶级菜单的链接
                BindString(FormatAlink(TopTab));

                //这里插入下级菜单的代码
                #region "这里插入下级菜单的代码"


                if (trueMegaType && MenuOptions != null && MenuOptions.ID > 0)
                {
                    //插入MegaMenu
                    BindMegaMenu(allTabs, TopTab, MenuOptions, 2);


                }
                else
                {
                    //有下级菜单的时候才进入
                    if (TrueSub)
                    {
                        //插入普通菜单
                        //ContentTHML.Append(BindGeneralMenu(allTabs, TopTab)).AppendLine();
                        BindGeneralMenu(allTabs, TopTab, 2);
                    }
                }


                #endregion




                //ContentTHML.AppendFormat(" </li>").AppendLine();
                BindString("</li>");
            }

            //ContentTHML.Append("</ul>").AppendLine();
            BindString("</ul>");
            //liContentHTML.Text = ContentTHML.ToString();

        }


        /// <summary>
        /// 绑定MEGA菜单第一层
        /// </summary>
        /// <param name="allTabs"></param>
        /// <param name="ParentTab"></param>
        /// <returns></returns>
        public void BindMegaMenu(List<TabInfo> allTabs, TabInfo MegaTab, DNNGo_ThemePlugin_Menu MenuOptions, Int32 ChildLevel)
        {
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {

                //筛选当前所要打印的菜单
                //List<TabInfo> MegaTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID);
                BindString("<div class=\"dnngo_menuslide\"  data-width=\"{0}\" data-position=\"{1}\">", MenuOptions.MenuWidth, GetSettingT<Int32>(MenuOptions, "MegaPosition", 0));
                BindString("<div class=\"dnngo_boxslide\">");

                ////循环调用每个顶级菜单的的二级菜单

                //查找菜单的设置
                List<KeyValueEntity> TabValues = FindTabValues(MenuOptions);

                //循环打印所有的列
                List<DNNGo_ThemePlugin_MenuPane> Panes = DNNGo_ThemePlugin_MenuPane.FindAllByTabID(MegaTab.TabID);

                if (Panes != null && Panes.Count > 0)
                {
                    BindString("<div class=\"dnngo_custommenu\">");

                    //循环打印所有的列到页面上
                    for (Int32 PaneIndex = 0; PaneIndex < Panes.Count; PaneIndex++)
                    {
                        var PaneItem = Panes[PaneIndex];

                        //BindString("<div class=\"menupane pane_{0} {2} {3} {4} {5}\"  style=\"width:{1}%;padding-top:{6}px;padding-right:{7}px;padding-bottom:{8}px;padding-left:{9}px;\">",
                        //    PaneItem.ID,
                        //    PaneItem.PaneWidth,
                        //    GetSettingT<Boolean>(PaneItem, "PaneTopLine", false) ? "topline" : "",
                        //    GetSettingT<Boolean>(PaneItem, "PaneBottomLine", false) ? "bottomline" : "",
                        //    GetSettingT<Boolean>(PaneItem, "PaneLeftLine", false) ? "leftline" : "",
                        //    GetSettingT<Boolean>(PaneItem, "PaneRightLine", false) ? "rightline" : "",
                        //    GetSettingT<Int32>(PaneItem, "PaneTopSpacing", 10),
                        //    GetSettingT<Int32>(PaneItem, "PaneRightSpacing", 10),
                        //    GetSettingT<Int32>(PaneItem, "PaneBottomSpacing", 10),
                        //    GetSettingT<Int32>(PaneItem, "PaneLeftSpacing", 10)
                        // );

                        //BindString("<div class=\"menupane pane_{0}\"  style=\"width:{1}%;\">",
                        //    PaneItem.ID,
                        //    PaneItem.PaneWidth
                        //    );

                        BindString("<div class=\"menupane pane_{0} {2} {3} {4} {5}\"  style=\"width:{1}%;\">",
                            PaneItem.ID,
                            PaneItem.PaneWidth,
                            GetSettingT<String>(PaneItem, "PaneTopLine", "0") == "1" ? "topline" : "",
                            GetSettingT<String>(PaneItem, "PaneBottomLine", "0") == "1" ? "bottomline" : "",
                            GetSettingT<String>(PaneItem, "PaneLeftLine", "0") == "1" ? "leftline" : "",
                            GetSettingT<String>(PaneItem, "PaneRightLine", "0") == "1" ? "rightline" : ""
                            
                         );


                        BindString("<div class=\"pane_space\"  style=\"padding-top:{0}px;padding-right:{1}px;padding-bottom:{2}px;padding-left:{3}px;\">",
                             GetSettingT<Int32>(PaneItem, "PaneTopSpacing", 10),
                             GetSettingT<Int32>(PaneItem, "PaneRightSpacing", 10),
                             GetSettingT<Int32>(PaneItem, "PaneBottomSpacing", 10),
                             GetSettingT<Int32>(PaneItem, "PaneLeftSpacing", 10)
                            );

                        //打印行结构到列中
                        BindMegaMenuRow(allTabs, MegaTab, PaneItem, ChildLevel + 1);

                        BindString("</div>");
                        BindString("</div>");


                    }

                    BindString("</div>");
                    BindString("<div class=\"clear\"></div>");

                }

                BindString("</div>");
                BindString("</div>");
            }

            //return ContentTHML.ToString();
        }


        /// <summary>
        /// 绑定菜单中的行
        /// </summary>
        /// <param name="MegaTab"></param>
        /// <param name="MenuPane"></param>
        public void BindMegaMenuRow(List<TabInfo> allTabs, TabInfo MegaTab, DNNGo_ThemePlugin_MenuPane MenuPane, Int32 ChildLevel)
        {
            //读取当前列中得行集合
            List<DNNGo_ThemePlugin_MenuRowItem> RowList = DNNGo_ThemePlugin_MenuRowItem.FindAllByPaneID(MenuPane.ID);

            if (RowList != null && RowList.Count > 0)
            {
                //循环打印所有的行到页面上
                for (Int32 RowIndex = 0; RowIndex < RowList.Count; RowIndex++)
                {
                    var RowItem = RowList[RowIndex];

                    //载入不同的类型填充到行记录中

                    
                    if (RowItem.RowType == (Int32)EnumRowType.HTML)
                    {
                        //载入HTML内容到菜单
                        BindString("<div class=\"submodule {0}\">", EnumHelper.GetEnumTextVal(RowItem.RowType, typeof(EnumRowType)));

                        BindHTMLModule(RowItem);

                        BindString("</div>");

                    }
                    else if (RowItem.RowType == (Int32)EnumRowType.Module)
                    {
                        //载入第三方模块到菜单
                        BindString("<div class=\"submodule {0}\">",EnumHelper.GetEnumTextVal(RowItem.RowType, typeof(EnumRowType)));
 
                        BindModule(RowItem);

                        BindString("</div>");

                    }
                    else if (RowItem.RowType == (Int32)EnumRowType.Menu)
                    {
                        //载入下级菜单到菜单
                        BindString("<div class=\"submenu {0} {1}\">", EnumHelper.GetEnumTextVal(RowItem.RowType, typeof(EnumRowType)), RowItem.MenuSytle);

                        BindRowMenu(allTabs, MegaTab, RowItem);

                        BindString("</div>");
                    }

                }
            }

        }





        /// <summary>
        /// 绑定MegaMenu下级菜单
        /// </summary>
        /// <param name="allTabs"></param>
        /// <param name="ParentTab"></param>
        /// <param name="TabValues"></param>
        /// <returns></returns>
        public void BindMegaMenuSub(List<TabInfo> allTabs, TabInfo ParentTab, Int32 WidthSubMenu, Boolean IsFast, Int32 ChildLevel)
        {
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {
                //筛选当前所要打印的菜单
                List<TabInfo> MegaTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID && r.IsVisible);


                BindString("<ul>");

                //循环调用子菜单
                foreach (TabInfo SubMegaTab in MegaTabs)
                {
                    //检测有无下级目录
                    Boolean TrueSub = allTabs.Exists(r => r.ParentId == SubMegaTab.TabID && r.IsVisible);
                    //当前菜单是否激活
                    Boolean TrueCurrent = AllActiveTab.Exists(r => r.TabID == SubMegaTab.TabID);
                    String CurrentClass = TrueCurrent ? " class=\"subcurrent\" " : "";

                    if (IsFast)//只有第一级需要分开列
                    {

                        BindString(" <li style=\"width:{0}%;\" {1}>", WidthSubMenu, CurrentClass);
                    }
                    else
                    {
                        BindString("<li {0}>", CurrentClass);
                    }

                    BindString(FormatAlink(SubMegaTab));

                    if (TrueSub) BindMegaMenuSub(allTabs, SubMegaTab, WidthSubMenu, false, ChildLevel + 1);

                    BindString("</li>");


                }



                BindString("</ul>");
            }

        }



        /// <summary>
        /// 绑定MegaMenu下级菜单
        /// </summary>
        /// <param name="allTabs"></param>
        /// <param name="ParentTab"></param>
        /// <param name="TabValues"></param>
        /// <returns></returns>
        public String BindMegaMenuExtensionSub(List<TabInfo> allTabs, TabInfo ParentTab, Int32 WidthSubMenu, Int32 ColumnsSubMenu, Boolean IsFast, Int32 ChildLevel)
        {
            StringBuilder SubSB = new StringBuilder();
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {
                //筛选当前所要打印的菜单
                List<TabInfo> MegaTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID && r.IsVisible);

                ColumnsSubMenu = MegaTabs.Count > ColumnsSubMenu ? ColumnsSubMenu : MegaTabs.Count;

                QueryParam qp = new QueryParam();
                qp.PageSize = ColumnsSubMenu;
                qp.RecordCount = MegaTabs.Count;
                StringBuilder[] sbs = new StringBuilder[ColumnsSubMenu];


                for (int Pages = 0; Pages < qp.Pages; Pages++)
                {
                    List<TabInfo> currentMegaTabs = Common.Split<TabInfo>(MegaTabs, Pages + 1, qp.PageSize);
                    for (int j = 0; j < currentMegaTabs.Count; j++)
                    {
                        if (sbs[j] == null)
                        {
                            sbs[j] = new StringBuilder();
                            sbs[j].AppendFormat("<ul style=\"width:{0}%\">", WidthSubMenu);

                        }

                        //当前菜单是否激活
                        Boolean TrueCurrent = AllActiveTab.Exists(r => r.TabID == currentMegaTabs[j].TabID);
                        String CurrentClass = TrueCurrent ? " class=\"subcurrent\" " : "";


                        sbs[j].AppendFormat("<li {0}>", CurrentClass).AppendLine();
                        sbs[j].Append(FormatAlink(currentMegaTabs[j])).AppendLine();

                        //检测有无下级目录
                        Boolean TrueSub = allTabs.Exists(r => r.ParentId == currentMegaTabs[j].TabID && r.IsVisible);
                        if (TrueSub) sbs[j].Append(BindMegaMenuExtensionSub_(allTabs, currentMegaTabs[j], WidthSubMenu, ChildLevel + 1));

                        sbs[j].Append("</li>").AppendLine();
                    }



                }


                for (int j = 0; j < ColumnsSubMenu; j++)
                {
                    sbs[j].Append("</ul>");
                    SubSB.Append(sbs[j].ToString());
                }
            }
            return SubSB.ToString();

        }



        /// <summary>
        /// 绑定MegaMenu下级菜单
        /// </summary>
        /// <param name="allTabs"></param>
        /// <param name="ParentTab"></param>
        /// <param name="TabValues"></param>
        /// <returns></returns>
        public String BindMegaMenuExtensionSub_(List<TabInfo> allTabs, TabInfo ParentTab, Int32 WidthSubMenu, int ChildLevel)
        {
            StringBuilder sb = new StringBuilder();
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {
                //筛选当前所要打印的菜单
                List<TabInfo> MegaTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID && r.IsVisible);


                sb.Append("<ul>");

                //循环调用子菜单
                foreach (TabInfo SubMegaTab in MegaTabs)
                {
                    //检测有无下级目录
                    Boolean TrueSub = allTabs.Exists(r => r.ParentId == SubMegaTab.TabID && r.IsVisible);
                    //当前菜单是否激活
                    Boolean TrueCurrent = AllActiveTab.Exists(r => r.TabID == SubMegaTab.TabID);
                    String CurrentClass = TrueCurrent ? " class=\"subcurrent\" " : "";

                    sb.AppendFormat("<li {0}>", CurrentClass);


                    sb.Append(FormatAlink(SubMegaTab));

                    if (TrueSub) sb.Append(BindMegaMenuExtensionSub_(allTabs, SubMegaTab, WidthSubMenu, ChildLevel + 1));

                    sb.Append("</li>");


                }



                sb.Append("</ul>");
            }

            return sb.ToString();
        }




        /// <summary>
        /// 绑定普通菜单
        /// </summary>
        /// <param name="allTabs"></param>
        /// <param name="ParentTab"></param>
        /// <returns></returns>
        public void BindGeneralMenu(List<TabInfo> allTabs, TabInfo ParentTab, Int32 ChildLevel)
        {
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {
                //筛选当前所要打印的菜单
                List<TabInfo> GeneralTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID && r.IsVisible);

                BindString(" <div class=\"dnngo_menuslide\">");
                BindString(" <ul class=\"dnngo_slide_menu \">");


                foreach (TabInfo GeneralTab in GeneralTabs)
                {
                    //检测有无下级目录
                    Boolean TrueSub = allTabs.Exists(r => r.ParentId == GeneralTab.TabID && ((ChildLevel + 1) <= MaxLevel || MaxLevel <= 0) && r.IsVisible);

                    //当前菜单是否激活
                    Boolean TrueCurrent = AllActiveTab.Exists(r => r.TabID == GeneralTab.TabID);
                    String CurrentClass = TrueCurrent ? "subcurrent" : "";


                    BindString(" <li class=\"{0} {1}\">", TrueSub ? "dir" : "", CurrentClass);

                    //打印顶级菜单的链接

                    BindString(FormatAlink(GeneralTab));

                    //有下级菜单的时候才进入
                    if (TrueSub) BindGeneralSubMenu(allTabs, GeneralTab, ChildLevel + 1);

                    BindString("</li>");
                }

                BindString("</ul>");
                BindString("</div>");
            }
        }

        public void BindGeneralSubMenu(List<TabInfo> allTabs, TabInfo ParentTab, Int32 ChildLevel)
        {
            if (ChildLevel <= MaxLevel || MaxLevel <= 0)
            {
                //筛选当前所要打印的菜单
                List<TabInfo> GeneralTabs = allTabs.FindAll(r => r.ParentId == ParentTab.TabID && r.IsVisible);

                BindString(" <div class=\"dnngo_submenu\">");
                BindString(" <ul>");


                foreach (TabInfo GeneralTab in GeneralTabs)
                {
                    //检测有无下级目录
                    Boolean TrueSub = allTabs.Exists(r => r.ParentId == GeneralTab.TabID && r.IsVisible);
                    //当前菜单是否激活
                    Boolean TrueCurrent = AllActiveTab.Exists(r => r.TabID == GeneralTab.TabID);
                    String CurrentClass = TrueCurrent ? "subcurrent" : "";


                    BindString(" <li class=\"{0} {1}\">", TrueSub ? "dir" : "", CurrentClass);

                    //打印顶级菜单的链接

                    BindString(FormatAlink(GeneralTab));

                    //有下级菜单的时候才进入
                    if (TrueSub) BindGeneralSubMenu(allTabs, GeneralTab, ChildLevel + 1);

                    BindString("</li>");
                }

                BindString("</ul>");
                BindString("</div>");
            }
        }




        public void BindString(String ContentText)
        {
            BindString(ContentText, null);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContentText"></param>
        public void BindString(String ContentText, params object[] args)
        {
            Literal li = new Literal();
            if (args != null && args.Length > 0)
            {
                li.Text = String.Format("{0}\r\n", String.Format(ContentText, args));
            }
            else
            {
                li.Text = String.Format("{0}\r\n", ContentText);
            }
            phContentHTML.Controls.Add(li);
        }




        /// <summary>
        /// 判断当前菜单是否为MegaMenu类型
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        public Boolean TrueMegaType(TabInfo tabInfo, out DNNGo_ThemePlugin_Menu MenuOptions)
        {
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.TabID, tabInfo.TabID, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.MenuType, (Int32)EnumTabType.MegaMenu, SearchType.Equal));
            MenuOptions = DNNGo_ThemePlugin_Menu.FindAllByItem(qp);
            return MenuOptions != null && MenuOptions.ID > 0;
        }

        /// <summary>
        /// 查找当前页面设置的参数列表
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        public DNNGo_ThemePlugin_Menu FindTabOptions(TabInfo tabInfo)
        {
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Menu._.TabID, tabInfo.TabID, SearchType.Equal));
            return DNNGo_ThemePlugin_Menu.FindAllByItem(qp);
        }
        /// <summary>
        /// 查找当前页面设置的值列表
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        public List<KeyValueEntity> FindTabValues(DNNGo_ThemePlugin_Menu MenuOptions)
        {
            List<KeyValueEntity> ItemSettings = new List<KeyValueEntity>();
            if (MenuOptions != null && MenuOptions.ID > 0 && !String.IsNullOrEmpty(MenuOptions.Options))
            {
                try
                {
                    ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(MenuOptions.Options);
                }
                catch
                {

                }
            }
            return ItemSettings;
        }




        /// <summary>
        /// 绑定页面上需要加载的模块
        /// </summary>
        public void BindModule(DNNGo_ThemePlugin_MenuRowItem RowItem)
        {
            ModuleController objModules = new ModuleController();
            ModuleInfo ModuleConfiguration = objModules.GetModule(RowItem.BindModuleID, RowItem.BindTabID);

            if (ModuleConfiguration != null && ModuleConfiguration.ModuleID > 0)
            {
                //这里可以加上代码控制标题是否显示
                if (RowItem.MenuDisplayTitle == 1)
                {
                    BindString("<h3 class=\"submenu_title\">{0}</h3>", ModuleConfiguration.ModuleTitle);
                }

                BindString("<div class=\"menucontent\"> ");

                try
                {
                    //phContentHTML.Controls.Add(ModuleControlFactory.LoadModuleControl(Page, ModuleConfiguration));

                    //PortalModuleBase _Control = ControlUtilities.LoadControl<PortalModuleBase>(this, ModuleConfiguration.ModuleControl.ControlSrc);
                    //_Control.ID = String.Format("{0}_{1}", ModuleConfiguration.ModuleID, Path.GetFileNameWithoutExtension(ModuleConfiguration.ModuleControl.ControlSrc));
                    //_Control.ModuleContext.Configuration = ModuleConfiguration;
                    //_Control.ModuleConfiguration = ModuleConfiguration;
                    //phContentHTML.Controls.Add(_Control);

                    var control = ModuleControlFactory.LoadModuleControl(Page, ModuleConfiguration);
                    if (control != null )
                    {
                        control.ID = String.Format("{0}_{1}", ModuleConfiguration.ModuleID, Path.GetFileNameWithoutExtension(ModuleConfiguration.ModuleControl.ControlSrc));
                        phContentHTML.Controls.Add(control);
                       
                    }


                }
                catch
                { }




                BindString("</div>");

            }
        }

        /// <summary>
        /// 绑定行设置中的HTML内容
        /// </summary>
        /// <param name="RowItem"></param>
        public void BindHTMLModule(DNNGo_ThemePlugin_MenuRowItem RowItem)
        {
            if (RowItem.MenuDisplayTitle == 1)
            {
                BindString("<h3 class=\"submenu_title\">{0}</h3>", RowItem.HTML_Title);
            }

            BindString("<div class=\"menucontent\">{0}</div>", RowItem.HTML_Content);
        }

        /// <summary>
        /// 绑定行中的菜单项
        /// </summary>
        public void BindRowMenu(List<TabInfo> allTabs, TabInfo MegaTab, DNNGo_ThemePlugin_MenuRowItem MenuRowItem)
        {
            //设置了菜单，并且所有集合中包含了菜单才会打印
            if (MenuRowItem.MenuID > 0 && allTabs.Exists(r => r.TabID == MenuRowItem.MenuID))
            {
                TabInfo BindTabInfo = allTabs.Find(r => r.TabID == MenuRowItem.MenuID);


                if(MenuRowItem.MenuDisplayTitle == 1 && BindTabInfo.IsVisible)
                {
                    BindString("<h3 class=\"submenu_title\">{0}</h3>", FormatAlink(BindTabInfo));
                }


                Int32 ChildLevel = 1;

                BindRowSubMenu(allTabs, BindTabInfo, MenuRowItem, ChildLevel );

            }


        }

        /// <summary>
        /// 绑定行中得菜单
        /// </summary>
        /// <param name="allTabs">所有菜单</param>
        /// <param name="MegaTab">当前菜单</param>
        /// <param name="MenuRowItem">行的设置</param>
        /// <param name="ChildLevel">当前级别</param>
        public void BindRowSubMenu(List<TabInfo> allTabs, TabInfo MegaTab, DNNGo_ThemePlugin_MenuRowItem MenuRowItem, Int32 ChildLevel)
        {
            List<TabInfo> SubList = allTabs.FindAll(r => r.ParentId == MegaTab.TabID);
            if (SubList != null && SubList.Count > 0 && (MenuRowItem.MenuLevel ==0 || MenuRowItem.MenuLevel >=  ChildLevel ))
            {
                BindString("<ul>");
                for (Int32 SubIndex = 0; SubIndex < SubList.Count; SubIndex++)
                {
                    var SubMenu = SubList[SubIndex];

                    if(SubMenu.IsVisible)
                    {

                        BindString("<li class=\"{0}\">", SubMenu.TabID == PortalSettings.ActiveTab.TabID ? "current" : "");
                        //绑定菜单链接
                        BindString(FormatAlink(SubMenu));
                        //绑定下级菜单
                        BindRowSubMenu(allTabs, SubMenu, MenuRowItem, ChildLevel + 1);

                        BindString("</li>");
                    }

                
                }
                BindString("</ul>");
            }

        }




        #endregion


        #region "事件"



        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(skin_Error))
                {
                    BindTopMenu();


                    if (!IsPostBack)
                    {
                        Inits();
                    }

                    /* 2017.02.22 胡贻伟 要求加上这个JS引用*/
                    LoadScript("dnngo-ThemePlugin.js");
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessPageLoadException(ex, Request.RawUrl);
            }
        }



        #endregion





    }
}