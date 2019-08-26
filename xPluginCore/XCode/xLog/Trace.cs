using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading;
using System.Diagnostics;

using System.Web;
using System.Configuration;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// ��־��
    /// </summary>
    public static class Trace
    {
        #region ��ʼ������
        private static StreamWriter LogWriter;
        private static String _LogDir;
        private static Object _LogDir_Lock = new object();
        /// <summary>
        /// ��־Ŀ¼
        /// </summary>
        public static String LogDir
        {
            get
            {
                if (!String.IsNullOrEmpty(_LogDir)) return _LogDir;
                lock (_LogDir_Lock)
                {
                    if (!String.IsNullOrEmpty(_LogDir)) return _LogDir;
                    
                    if (ConfigurationManager.AppSettings["XLogDir"] != null) //��ȡ����
                    {
                        _LogDir = ConfigurationManager.AppSettings["XLogDir"].ToString();
                        if (HttpContext.Current != null && _LogDir.Substring(1, 1) != @":")
                            _LogDir = HttpContext.Current.Server.MapPath(_LogDir);
                    }
                    else if (HttpContext.Current != null) //��վʹ��Ĭ����־Ŀ¼
                    {
                        _LogDir = HttpContext.Current.Server.MapPath("~/Log/");
                    }
                    else //ʹ��Ӧ�ó���Ŀ¼
                    {
                        _LogDir = AppDomain.CurrentDomain.BaseDirectory;
                        if (!String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                            _LogDir = Path.Combine(_LogDir, AppDomain.CurrentDomain.RelativeSearchPath);
                        //����Ƿ�����վ�е�BinĿ¼�£����̵߳�ʱ�򣬾��޷�ȡ��HttpContext.Current
                        //�Ӷ���֪����ǰ��WinForm������վ
                        if (_LogDir.ToLower().EndsWith(@"\bin"))
                        {
                            String str = _LogDir.Substring(0, _LogDir.Length - @"bin".Length);
                            if (File.Exists(Path.Combine(str, "web.config"))) _LogDir = str;
                        }
                        _LogDir = Path.Combine(_LogDir, @"Log\");
                    }

                    //��֤\��β
                    if (!String.IsNullOrEmpty(_LogDir) && LogDir.Substring(_LogDir.Length - 1, 1) != @"\") _LogDir += @"\";

                    return _LogDir;
                }
            }
        }

        /// <summary>
        /// ��ʼ����־��¼�ļ�
        /// </summary>
        private static void InitLog()
        {
            String path = LogDir;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            if (path.Substring(path.Length - 2) != @"\") path += @"\";
            String logfile = path + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
            int i = 0;
            while (i < 10)
            {
                try
                {
                    LogWriter = new StreamWriter(logfile, true, Encoding.UTF8);
                    LogWriter.AutoFlush = true;
                    break;
                }
                catch
                {
                    if (logfile.EndsWith("_" + i + ".log"))
                        logfile = logfile.Replace("_" + i + ".log", "_" + (++i) + ".log");
                    else
                        logfile = logfile.Replace(@".log", @"_0.log");
                }
            }
            if (i >= 10) throw new Exception("Unable to write to the log!");
            LogWriter.WriteLine("\r\n\r\n");
            LogWriter.WriteLine(new String('*', 80));

            String str = DateTime.Now.ToString("HH:mm:ss");
            str += " ���̣�" + Process.GetCurrentProcess().Id;
            str += " �̣߳�" + Thread.CurrentThread.ManagedThreadId;
            if (Thread.CurrentThread.IsThreadPoolThread) str += "(�̳߳�)";
            LogWriter.WriteLine(str + "\t��ʼ��¼");
        }

        /// <summary>
        /// ֹͣ��־
        /// </summary>
        private static void CloseWriter(Object obj)
        {
            if (LogWriter == null) return;
            lock (Log_Lock)
            {
                try
                {
                    if (LogWriter == null) return;
                    LogWriter.Close();
                    LogWriter.Dispose();
                    LogWriter = null;
                }
                catch { }
            }
        }
        #endregion

        #region �첽д��־
        private static Timer AutoCloseWriterTimer;
        private static object Log_Lock = new object();
        /// <summary>
        /// ʹ���̳߳��߳��첽ִ����־д�붯��
        /// </summary>
        /// <param name="obj"></param>
        private static void PerformWriteLog(Object obj)
        {
            lock (Log_Lock)
            {
                try
                {
                    // ��ʼ����־��д��
                    if (LogWriter == null) InitLog();
                    // д��־
                    LogWriter.WriteLine((String)obj);
                    // �����Զ��ر���־��д���Ķ�ʱ���������ӳ�ʱ�䣬ʵ���ϲ�����
                    if (AutoCloseWriterTimer == null) AutoCloseWriterTimer = new Timer(new TimerCallback(CloseWriter), null, Timeout.Infinite, Timeout.Infinite);
                    // �ı䶨ʱ��Ϊ5��󴥷�һ�Ρ����5�����ж��д��־���������ƶ�ʱ�����ᴥ����ֱ����������Ϊֹ
                    AutoCloseWriterTimer.Change(5000, Timeout.Infinite);
                }
                catch { }
            }
        }
        #endregion

        #region д��־
        /// <summary>
        /// �����־
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void WriteLine(String msg)
        {
            String str = DateTime.Now.ToString("HH:mm:ss.fff");
            str += " �̣߳�" + Thread.CurrentThread.ManagedThreadId;
            if (Thread.CurrentThread.IsThreadPoolThread) str += "(�̳߳�)";

            // ��ʱ�����쳣�߳��У�����ʹ��HttpContext��ǿ�Ƴ�ʼ����־Ŀ¼
            String s = LogDir;

            // ʹ���̳߳��߳�д����־
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformWriteLog), str + "\t" + msg);
        }

        /// <summary>
        /// ���ԡ�
        /// �����ջ��Ϣ�����ڵ���ʱ������������ġ�
        /// ����������ɴ�����־�������á�
        /// </summary>
        public static void Debug()
        {
            Debug(int.MaxValue);
        }

        /// <summary>
        /// ���ԡ�
        /// </summary>
        /// <param name="maxNum">��󲶻��ջ������</param>
        public static void Debug(int maxNum)
        {
            int skipFrames = 1;
            if (maxNum == int.MaxValue) skipFrames = 2;
            StackTrace st = new StackTrace(skipFrames, true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("���ö�ջ��");
            int count = Math.Min(maxNum, st.FrameCount);
            for (int i = 0; i < count; i++)
            {
                StackFrame sf = st.GetFrame(i);
                sb.AppendFormat("{0}->{1}", sf.GetMethod().DeclaringType.FullName, sf.GetMethod().ToString());
                if (i < count - 1) sb.AppendLine();
                //sb.AppendLine(sf.ToString());
            }
            WriteLine(sb.ToString());
        }
        #endregion
    }
}