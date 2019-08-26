using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Skins;

using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 菜单显示部分
    /// </summary>
    public partial class Skin_Menus : BaseNavObjectBase
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(skin_Error))
            {
                if (!IsPostBack)
                {
                    //页面首次加载
                    Inits();
                }
            }
        }

        protected void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(skin_Error))
                {
                    //绑定容器内的值
                    BindContainer();

                    LoadScript("dnngo-ThemePlugin.js");
                }

                //LoadScriptForJqueryAndUI();
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }




        /// <summary>
        /// 绑定容器
        /// </summary>
        private void BindContainer()
        {
            String SkinObjectControl = String.Empty;
            if (Effect.Equals("VSlide", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_VSlide.ascx";
            }
            else if (Effect.Equals("Accordion", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_Accordion.ascx";
            }
            else if (Effect.Equals("AccordionPro", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_AccordionPro.ascx";
            }
            else if (Effect.Equals("AccordionPro_2", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_AccordionPro_2.ascx";
            }
            else if (Effect.Equals("AccordionSub", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_AccordionSub.ascx";
            }
            else if (Effect.Equals("Html", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_Html.ascx";
            }
            else if (Effect.Equals("MegaMenu", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_MegaMenu_Old.ascx";
            }
            else if (Effect.Equals("MultiMenu", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_MultiMenu.ascx";
            }
            else if (Effect.Equals("DropDown", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_DropDownList.ascx";
            }
            else if (Effect.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase))
            {
                SkinObjectControl = "Skin_Mobile.ascx";
            }
            else
            {
                SkinObjectControl = "Skin_HSlide.ascx";
            }
            //加载相应的控件
            BaseNavObjectBase ManageContent = this;
            string ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, SkinObjectControl));
            phContainer.Controls.Add(Convert((BaseNavObjectBase)LoadControl(ContentSrc)));

             
        }

        /// <summary>
        /// 将属性转换到下级去
        /// </summary>
        /// <param name="ManageContent"></param>
        /// <returns></returns>
        private BaseNavObjectBase Convert(BaseNavObjectBase ManageContent)
        {
            ManageContent.ViewLevel = ViewLevel;
            ManageContent.RootParent = RootParent;
            ManageContent.MaxLevel = MaxLevel;
            ManageContent.Effect = Effect;
            ManageContent.CssClass = CssClass;
            ManageContent.ShowTitle = ShowTitle;
            ManageContent.ShowTooltip = ShowTooltip;
            ManageContent.ShowDNN = ShowDNN;
            ManageContent.ShowXml = ShowXml;
            ManageContent.XmlName = XmlName;
            ManageContent.XmlWebPath = XmlWebPath;


            ManageContent.toggle = toggle;
            ManageContent.animated = animated;
            ManageContent.autoHeight = autoHeight;

            ManageContent.TopMeunTitle = TopMeunTitle;
            ManageContent.FastSubcurrent = FastSubcurrent;
            ManageContent.StandardMenu = StandardMenu;




            ManageContent.AccordionPro_AnimateSpeed = AccordionPro_AnimateSpeed;
            ManageContent.AccordionPro_Interval = AccordionPro_Interval;
            ManageContent.AccordionPro_Sensitivity = AccordionPro_Sensitivity;
            ManageContent.AccordionPro_Timeout = AccordionPro_Timeout;


            ManageContent.MegaMenu_Column = MegaMenu_Column;
            ManageContent.MegaMenu_Interval = MegaMenu_Interval;
            ManageContent.MegaMenu_Sensitivity = MegaMenu_Sensitivity;
            ManageContent.MegaMenu_Timeout = MegaMenu_Timeout;


            ManageContent.MultiMenuAction = MultiMenuAction;


            ManageContent.slidingSubmenus = slidingSubmenus;
            ManageContent.counters = counters;
            ManageContent.navbartitle = navbartitle;
            ManageContent.headerbox = headerbox;
            ManageContent.footerbox = footerbox;


            return ManageContent;
        }



    }
}
