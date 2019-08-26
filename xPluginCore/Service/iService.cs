using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface iService
    {

 
 

        /// <summary>
        /// 待输出的字符串
        /// </summary>
        String ResponseString { get; set; }

        /// <summary>
        /// 是否输出写入的数据
        /// </summary>
        Boolean IsResponseWrite { get; set; }


        /// <summary>
        /// 服务执行
        /// </summary>
        /// <param name="Context"></param>
        void Execute(BasePage Context);


    }
}