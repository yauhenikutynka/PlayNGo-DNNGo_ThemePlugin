using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Permissions;

using DotNetNuke.Entities.Modules.Definitions;
using System.Collections;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 初始化类
    /// </summary>
    public class Initialization
    {




        private BaseSkin _bSkin = new BaseSkin();
        /// <summary>
        /// 皮肤对象基类
        /// </summary>
        public BaseSkin BSkin
        {
            get { return _bSkin; }
            set { _bSkin = value; }
        }



        /// <summary>
        /// 皮肤插件初始化标志
        /// </summary>
        public Boolean ThemePlugin_Init
        {
            get { return BSkin.GetSetting("Init_ThemePlugin", false); }
        }

        /// <summary>
        /// 皮肤插件模块编号
        /// </summary>
        public Int32 ThemePlugin_Init_ModuleID
        {
            get { return BSkin.GetSetting("Init_ThemePlugin_ModuleID", 0); }
        }

        /// <summary>
        /// 皮肤插件页面编号
        /// </summary>
        public Int32 ThemePlugin_Init_TabID
        {
            get { return BSkin.GetSetting("Init_ThemePlugin_TabID", 0); }
        }

        /// <summary>
        /// 构造类
        /// </summary>
        /// <param name="__bSkin"></param>
        public Initialization(BaseSkin __bSkin)
        {
            BSkin = __bSkin;
        }

        /// <summary>
        /// 初始化的方法
        /// </summary>
        public void Init()
        {
            if (!ThemePlugin_Init)
            {


                Int32 x_ModuleID = 0;

                TabInfo parentTab = BSkin.objTabs.GetTabByName("Admin", BSkin.PortalSettings.PortalId);

                if(parentTab != null && parentTab.TabID > 0)
                {
                    TabInfo dnnTab = BSkin.objTabs.GetTabByName("ThemePlugin", BSkin.PortalSettings.PortalId, parentTab.TabID);
                    if (!(dnnTab != null && dnnTab.TabID > 0))
                    {
                        dnnTab = new TabInfo();
                        dnnTab.PortalID = BSkin.PortalSettings.PortalId;
                        dnnTab.TabName = "ThemePlugin";
                        dnnTab.Title = "ThemePlugin";
                        dnnTab.IsVisible = true;
                        dnnTab.DisableLink = false;
                        dnnTab.IsDeleted = false;

                        if (parentTab != null && parentTab.TabID > 0)
                        {
                            dnnTab.PortalID = parentTab.PortalID;
                            dnnTab.ParentId = parentTab.TabID;
                            dnnTab.Level = parentTab.Level + 1;
                            dnnTab.TabPermissions.Clear();
                            dnnTab.TabPermissions.AddRange(parentTab.TabPermissions);//增加权限

                        }
                        else
                        {
                            dnnTab.ParentId = Null.NullInteger;
                            dnnTab.Level = 0;
                        }
                        dnnTab.TabPath = DotNetNuke.Common.Globals.GenerateTabPath(dnnTab.ParentId, dnnTab.TabName);
                        dnnTab.TabID = BSkin.objTabs.AddTab(dnnTab);

                        if (dnnTab.TabID > 0)
                        {
                            x_ModuleID = AddNewModule(dnnTab);
                        }
                    }

                    if (!(ThemePlugin_Init_ModuleID > 0 && ThemePlugin_Init_TabID > 0))
                    {
                        if (!(x_ModuleID > 0))
                        {
                            x_ModuleID = AddNewModule(dnnTab);
                        }


                        BSkin.UpdateSetting("Init_ThemePlugin_TabID", dnnTab.TabID.ToString());
                        BSkin.UpdateSetting("Init_ThemePlugin_ModuleID", x_ModuleID.ToString());
                    }




                    BSkin.UpdateSetting("Init_ThemePlugin", "true");


                }

                
            }
        }

        public void InitSettings()
        {
 
        }



        #region "模块业务共享的方法"

        /// <summary>
        /// 添加新的模块
        /// </summary>
        public Int32 AddNewModule(TabInfo CurrentTab)
        {
            //找到HTML模块的编号
            int desktopModuleId = 0;
            int ModuleId = 0;
            DesktopModuleInfo deskModule = DesktopModuleController.GetDesktopModuleByModuleName("DNNGo.ThemePlugin", BSkin.PortalSettings.PortalId);
            if (deskModule != null && deskModule.DesktopModuleID > 0)
            {
                desktopModuleId = deskModule.DesktopModuleID;



                var list = TabModulesController.Instance.GetTabModules(new TabInfo() { TabID = CurrentTab.TabID }).Cast<ModuleInfo>().ToList();
                if (list != null && list.Count > 0)
                {

                    ModuleInfo mInfo = list.Find(r => BSkin.ModuleProperty(r, "ModuleName") == "DNNGo.ThemePlugin");

                    if (!(mInfo != null && mInfo.ModuleID > 0))
                    {
                        ModuleId = AddNewModule("DNNGo ThemePlugin", desktopModuleId, CurrentTab, "ContentPane", -1, ViewPermissionType.View, "");
                    }
                    else
                    {
                        ModuleId = mInfo.ModuleID;
                    }
                }
                else
                {
                    ModuleId = AddNewModule("DNNGo ThemePlugin", desktopModuleId, CurrentTab, "ContentPane", -1, ViewPermissionType.View, "");
                }

            }
            return ModuleId;
        }


        public Int32 AddNewModule(string title, int desktopModuleId, TabInfo ActiveTab, string paneName, int position, ViewPermissionType permissionType, string align)
        {
            int ModuleId = 0;
            TabPermissionCollection objTabPermissions = ActiveTab.TabPermissions;
            PermissionController objPermissionController = new PermissionController();
            ModuleController objModules = new ModuleController();
            DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
            int j;
            try
            {
                DesktopModuleInfo desktopModule = null;
                if (!DesktopModuleController.GetDesktopModules(BSkin.PortalSettings.PortalId).TryGetValue(desktopModuleId, out desktopModule))
                {
                    throw new ArgumentException("desktopModuleId");
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
            int UserId = -1;
            if (BSkin.Request.IsAuthenticated)
            {
                UserInfo objUserInfo = UserController.GetCurrentUserInfo();
                UserId = objUserInfo.UserID;
            }
            foreach (ModuleDefinitionInfo objModuleDefinition in ModuleDefinitionController.GetModuleDefinitionsByDesktopModuleID(desktopModuleId).Values)
            {
                ModuleInfo objModule = new ModuleInfo();
                objModule.Initialize(BSkin.PortalSettings.PortalId);
                objModule.PortalID = BSkin.PortalSettings.PortalId;
                objModule.TabID = ActiveTab.TabID;
                objModule.ModuleOrder = position;
                if (String.IsNullOrEmpty(title))
                {
                    objModule.ModuleTitle = objModuleDefinition.FriendlyName;
                }
                else
                {
                    objModule.ModuleTitle = title;
                }
                objModule.PaneName = paneName;
                objModule.ModuleDefID = objModuleDefinition.ModuleDefID;
                if (objModuleDefinition.DefaultCacheTime > 0)
                {
                    objModule.CacheTime = objModuleDefinition.DefaultCacheTime;
                    if (BSkin.PortalSettings.DefaultModuleId > Null.NullInteger && BSkin.PortalSettings.DefaultTabId > Null.NullInteger)
                    {
                        ModuleInfo defaultModule = objModules.GetModule(BSkin.PortalSettings.DefaultModuleId, BSkin.PortalSettings.DefaultTabId, true);
                        if (defaultModule != null)
                        {
                            objModule.CacheTime = defaultModule.CacheTime;
                        }
                    }
                }
                switch (permissionType)
                {
                    case ViewPermissionType.View:
                        objModule.InheritViewPermissions = true;
                        break;
                    case ViewPermissionType.Edit:
                        objModule.InheritViewPermissions = false;
                        break;
                }
                ArrayList arrSystemModuleViewPermissions = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_MODULE_DEFINITION", "VIEW");
                foreach (TabPermissionInfo objTabPermission in objTabPermissions)
                {
                    if (objTabPermission.PermissionKey == "VIEW" && permissionType == ViewPermissionType.View)
                    {
                        continue;
                    }
                    ArrayList arrSystemModulePermissions = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_MODULE_DEFINITION", objTabPermission.PermissionKey);
                    for (j = 0; j <= arrSystemModulePermissions.Count - 1; j++)
                    {
                        PermissionInfo objSystemModulePermission;
                        objSystemModulePermission = (PermissionInfo)arrSystemModulePermissions[j];
                        if (objSystemModulePermission.PermissionKey == "VIEW" && permissionType == ViewPermissionType.Edit && objTabPermission.PermissionKey != "EDIT")
                        {
                            continue;
                        }
                        ModulePermissionInfo objModulePermission = AddModulePermission(objModule, objSystemModulePermission, objTabPermission.RoleID, objTabPermission.UserID, objTabPermission.AllowAccess);
                        if (objModulePermission.PermissionKey == "EDIT" && objModulePermission.AllowAccess)
                        {
                            ModulePermissionInfo objModuleViewperm = AddModulePermission(objModule, (PermissionInfo)arrSystemModuleViewPermissions[0], objModulePermission.RoleID, objModulePermission.UserID, true);
                        }
                    }
                    if (objTabPermission.PermissionKey == "EDIT")
                    {
                        ArrayList arrCustomModulePermissions = objPermissionController.GetPermissionsByModuleDefID(objModule.ModuleDefID);
                        for (j = 0; j <= arrCustomModulePermissions.Count - 1; j++)
                        {
                            PermissionInfo objCustomModulePermission;
                            objCustomModulePermission = (PermissionInfo)arrCustomModulePermissions[j];
                            AddModulePermission(objModule, objCustomModulePermission, objTabPermission.RoleID, objTabPermission.UserID, objTabPermission.AllowAccess);
                        }
                    }
                }

 
                objModule.AllTabs = false;
                objModule.Alignment = align;
                ModuleId = objModules.AddModule(objModule);
            }
            return ModuleId;
        }

        private ModulePermissionInfo AddModulePermission(ModuleInfo objModule, PermissionInfo permission, int roleId, int userId, bool allowAccess)
        {
            ModulePermissionInfo objModulePermission = new ModulePermissionInfo();
            objModulePermission.ModuleID = objModule.ModuleID;
            objModulePermission.PermissionID = permission.PermissionID;
            objModulePermission.RoleID = roleId;
            objModulePermission.UserID = userId;
            objModulePermission.PermissionKey = permission.PermissionKey;
            objModulePermission.AllowAccess = allowAccess;
            if (!objModule.ModulePermissions.Contains(objModulePermission))
            {
                objModule.ModulePermissions.Add(objModulePermission);
            }
            return objModulePermission;
        }

 
        #endregion

    }


    public enum ViewPermissionType
    {
        View = 0,
        Edit = 1
    }
}