using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 效果设置实体(XML & 序列化)
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("相册")]
    [XmlEntityAttributes("DNNGo_ThemePlugin//GallerySettingsEntityList//GallerySettingsEntityItem")]
    public class GallerySettingsEntity
    {
        #region 属性

        /// <summary>配置名</summary>
        public String SettingName { get; set; }


        /// <summary>配置值</summary>
        public String SettingValue { get; set; }


        public GallerySettingsEntity()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_SettingName">配置名</param>
        /// <param name="_SettingValue">配置值</param>
        public GallerySettingsEntity(String _SettingName, String _SettingValue)
        {
            SettingName = _SettingName;
            SettingValue = _SettingValue;
        }


        #endregion
 
    }
}