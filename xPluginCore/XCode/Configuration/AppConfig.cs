using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 配置信息读取
    /// </summary>
    public class AppConfig
    {

        #region "获取各种配置信息"

        /// <summary>
        /// 是否调试(默认:true)
        /// </summary>
        public static string OrmDebug
        {
            get { return GetApp("OrmDebug", "false"); }
        }

        /// <summary>
        /// 缓存相对有效期。
        /// -2	关闭缓存
        /// -1	非独占数据库，有外部系统操作数据库，使用请求级缓存；
        ///  0	永久静态缓存；
        /// >0	静态缓存时间，单位是秒；
        /// </summary>
        public static string XCacheExpiration
        {
            get { return GetApp("XCacheExpiration", "300"); }
        }

        /// <summary>
        /// 维护定时器的检查周期，默认5秒
        /// </summary>
        public static string XCacheCheckPeriod
        {
            get { return GetApp("XCacheCheckPeriod", "5"); }
        }
        /// <summary>
        /// 是否启用数据架构
        /// </summary>
        public static string DatabaseSchema_Enable
        {
            get { return GetApp("DatabaseSchema_Enable", "false"); }
        }
        /// <summary>
        /// 是否启用不删除字段，默认不启用。删除字段的操作过于危险，这里可以通过设为true关闭
        /// </summary>
        public static string DatabaseSchema_NoDelete
        {
            get { return GetApp("DatabaseSchema_NoDelete", "true"); }
        }

        /// <summary>
        /// 要排除的链接名
        /// </summary>
        public static string DatabaseSchema_Exclude
        {
            get { return GetApp("DatabaseSchema_Exclude", ""); }
        }

        
        
        

        #endregion





        #region "读取配置信息基础函数"
        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">配置名</param>
        /// <returns>返回配置值</returns>
        public static string GetApp(string key)
        {
            return GetApp(key, string.Empty);
        }
        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回配置值</returns>
        public static string GetApp(string key, string defaultValue)
        {
            key = WebConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(key)) return key;
            return defaultValue;
        }
        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回配置值</returns>
        public static int GetAppInt(string key, int defaultValue)
        {
            int result = 0;
            if (int.TryParse(GetApp(key), out result) && result != 0) return result;
            return defaultValue;
        }
        #endregion

 



    }
}
