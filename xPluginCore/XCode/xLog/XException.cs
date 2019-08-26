using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// X�쳣��д��XLog��־��
    /// </summary>
    public class XException : Exception
    {
        /// <summary>
        /// X�쳣��д��XLog��־��
        /// </summary>
        /// <param name="message">�쳣��Ϣ</param>
        public XException(String message)
            : base(message)
        {
            Trace.WriteLine(message);
        }

        /// <summary>
        /// X�쳣��д��XLog��־��
        /// </summary>
        /// <param name="message">�쳣��Ϣ</param>
        /// <param name="ex">�쳣����</param>
        public XException(String message, Exception ex)
            : base(message, ex)
        {
            if (ex == null || String.IsNullOrEmpty(ex.Message))
                Trace.WriteLine(message);
            else
                Trace.WriteLine(message + " �ڲ��쳣��" + ex.Message);
        }
    }
}