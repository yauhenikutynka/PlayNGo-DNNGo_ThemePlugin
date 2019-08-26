using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 效果展示数据
    /// </summary>
    [XmlEntityAttributes("DNNGo_ThemePlugin//XmlDB")]
    public class XmlDBEntity
    {

        private String _Name = String.Empty;
        /// <summary>
        /// 效果名称
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private String _Description = String.Empty;
        /// <summary>
        /// 效果描述
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        private String _Version = String.Empty;
        /// <summary>
        /// 版本号
        /// </summary>
        public String Version
        {
            get { return _Version; }
            set { _Version = value; }
        }


        private String _Thumbnails = String.Empty;
        /// <summary>
        /// 缩略图
        /// </summary>
        public String Thumbnails
        {
            get { return _Thumbnails; }
            set { _Thumbnails = value; }
        }


        private String _EffectScript = String.Empty;
        /// <summary>
        /// 效果附带脚本
        /// </summary>
        public String EffectScript
        {
            get { return _EffectScript; }
            set { _EffectScript = value; }
        }


        private String _GlobalScript = String.Empty;
        /// <summary>
        /// 全局附带脚本
        /// </summary>
        public String GlobalScript
        {
            get { return _GlobalScript; }
            set { _GlobalScript = value; }
        }



        private String _DisplayByNormal = String.Empty;
        /// <summary>
        /// 显示普通界面的控件集合
        /// </summary>
        public String DisplayByNormal
        {
            get { return _DisplayByNormal; }
            set { _DisplayByNormal = value; }
        }


        private String _DisplayByModule = String.Empty;
        /// <summary>
        /// 显示模块界面的控件集合
        /// </summary>
        public String DisplayByModule
        {
            get { return _DisplayByModule; }
            set { _DisplayByModule = value; }
        }


        private String _DemoUrl = String.Empty;
        /// <summary>
        /// 演示地址
        /// </summary>
        public String DemoUrl
        {
            get { return _DemoUrl; }
            set { _DemoUrl = value; }
        }

        private String _SettingName = "Skin";
        /// <summary>
        /// 配置名(皮肤/效果/功能)
        /// </summary>
        public String SettingName
        {
            get { return _SettingName; }
            set { _SettingName = value; }
        }



        private Boolean _DisplayEffect = false;
        /// <summary>
        /// 是否效果页面显示
        /// </summary>
        public Boolean DisplayEffect
        {
            get { return _DisplayEffect; }
            set { _DisplayEffect = value; }
        }


        private String _DataListOption = "Effect";
        /// <summary>
        /// 数据列表选项(筛选数据用的)
        /// 这里我们会做多套数据筛选的选项面板
        /// 通过这个参数来切换
        /// </summary>
        public String DataListOption
        {
            get { return _DataListOption; }
            set { _DataListOption = value; }
        }

 
    }
}