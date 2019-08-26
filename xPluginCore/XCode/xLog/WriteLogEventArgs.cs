using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 写日志事件参数
    /// </summary>
    public class WriteLogEventArgs : EventArgs
    {
        private String _Message;
        /// <summary>日志信息</summary>
        public String Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private Exception _Exception;
        /// <summary>异常</summary>
        public Exception Exception
        {
            get { return _Exception; }
            set { _Exception = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">日志</param>
        public WriteLogEventArgs(String message)
        {
            Message = message;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">日志</param>
        /// <param name="exception">异常</param>
        public WriteLogEventArgs(String message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
    }
}