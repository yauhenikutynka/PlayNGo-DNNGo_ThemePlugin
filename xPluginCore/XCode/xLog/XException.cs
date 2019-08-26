using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// X异常。写入XLog日志。
    /// </summary>
    public class XException : Exception
    {
        /// <summary>
        /// X异常。写入XLog日志。
        /// </summary>
        /// <param name="message">异常信息</param>
        public XException(String message)
            : base(message)
        {
            Trace.WriteLine(message);
        }

        /// <summary>
        /// X异常。写入XLog日志。
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="ex">异常对象</param>
        public XException(String message, Exception ex)
            : base(message, ex)
        {
            if (ex == null || String.IsNullOrEmpty(ex.Message))
                Trace.WriteLine(message);
            else
                Trace.WriteLine(message + " 内部异常：" + ex.Message);
        }
    }
}