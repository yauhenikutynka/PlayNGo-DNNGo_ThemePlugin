using System;
using System.Collections.Generic;
using System.Web;
using CNVelocity;
using CNVelocity.App;
using Commons.Collections;
using CNVelocity.Runtime;
using System.IO;
using CNVelocity.Context;
using System.Text;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// CNVelocity模板工具类 VelocityHelper
    /// </summary>
    public class VelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;
        private BaseModule bPage = new BaseModule();
        private XmlDBEntity Theme = new XmlDBEntity();
   

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pmb">集成模块的对象</param>
        public VelocityHelper(BaseModule _bpm)
        {
            bPage = _bpm;
            Init(_bpm);
        }

        public VelocityHelper(BaseModule _bpm, XmlDBEntity _Theme)
        {
            Theme = _Theme;
            bPage = _bpm;

            Init(_bpm);
        }
 

        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public VelocityHelper() { }

        /// <summary>
        /// 初始话CNVelocity模块
        /// </summary>
        public void Init(BaseModule _bpm)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();
 
         
            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();

            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(String.Format("{0}xTemplate/", _bpm.SkinPath)));
            //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.GetDirectoryName(HttpContext.Current.Request.PhysicalPath));
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");

            //模板的缓存设置
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, false);              //是否缓存
            props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)600);    //缓存时间(秒)

            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            if (context == null)
                context = new VelocityContext();
            context.Put(key, value);
        }

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <returns></returns>
        public String Display(String templatFileName)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //添加共用变量
             context.Put("Module", bPage);
             context.Put("UserInfo", bPage.UserInfo);
             context.Put("Portal", bPage.PortalSettings);
            context.Put("Host", HostController.Instance.GetSettingsDictionary());

            StringWriter writer = new StringWriter();
             lock (this)
             {
                 //合并模板
                 template.Merge(context, writer);
             }
            return writer.ToString();
        }
 
    }

}




