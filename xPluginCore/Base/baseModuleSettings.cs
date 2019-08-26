﻿using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Modules;
//using DotNetNuke.UI.Modules;
using System.ComponentModel;
using System.Collections;

namespace DNNGo.Modules.ThemePlugin
{
    public class baseModuleSettings : BaseModule//, ISettingsControl
    {
        private Hashtable _moduleSettings;
        private Hashtable _settings;
        private Hashtable _tabModuleSettings;
        public Hashtable ModuleSettings
        {
            get
            {
                if (_moduleSettings == null)
                {
                    _moduleSettings = new ModuleController().GetModuleSettings(ModuleId);
                }
                return _moduleSettings;
            }
        }
        public Hashtable TabModuleSettings
        {
            get
            {
                if (_tabModuleSettings == null)
                {
                    _tabModuleSettings = new ModuleController().GetTabModuleSettings(TabModuleId);
                }
                return _tabModuleSettings;
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Hashtable Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new Hashtable();
                    foreach (string strKey in TabModuleSettings.Keys)
                    {
                        _settings[strKey] = TabModuleSettings[strKey];
                    }
                    foreach (string strKey in ModuleSettings.Keys)
                    {
                        _settings[strKey] = ModuleSettings[strKey];
                    }
                }
                return _settings;
            }
        }
        public virtual void LoadSettings()
        {
        }
        public virtual void UpdateSettings()
        {
        }
    }
}
