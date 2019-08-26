using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// ��־��
    /// </summary>
    public static class XTrace
    {
        #region ��ʼ������
        private static StreamWriter LogWriter;
        private static String _LogDir;
        private static Object _LogDir_Lock = new object();
        /// <summary>
        /// ��־Ŀ¼
        /// </summary>
        public static String LogPath
        {
            get
            {
                if (!String.IsNullOrEmpty(_LogDir)) return _LogDir;
                lock (_LogDir_Lock)
                {
                    if (!String.IsNullOrEmpty(_LogDir)) return _LogDir;

                    String dir = String.Empty;
                    if (ConfigurationManager.AppSettings["LogPath"] != null) //��ȡ����
                    {
                        dir = ConfigurationManager.AppSettings["LogPath"].ToString();
                        if (HttpContext.Current != null && dir.Substring(1, 1) != @":")
                            dir = HttpContext.Current.Server.MapPath(dir);
                    }
                    else if (HttpContext.Current != null) //��վʹ��Ĭ����־Ŀ¼
                    {
                        dir = HttpContext.Current.Server.MapPath("~/Log/");
                    }
                    else //ʹ��Ӧ�ó���Ŀ¼
                    {
                        dir = AppDomain.CurrentDomain.BaseDirectory;
                        if (!String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                            dir = Path.Combine(dir, AppDomain.CurrentDomain.RelativeSearchPath);
                        //����Ƿ�����վ�е�BinĿ¼�£����̵߳�ʱ�򣬾��޷�ȡ��HttpContext.Current
                        //�Ӷ���֪����ǰ��WinForm������վ
                        if (dir.ToLower().EndsWith(@"\bin"))
                        {
                            String str = dir.Substring(0, dir.Length - @"bin".Length);
                            if (File.Exists(Path.Combine(str, "web.config"))) dir = str;
                        }
                        dir = Path.Combine(dir, @"Log\");
                    }

                    //��֤\��β
                    if (!String.IsNullOrEmpty(dir) && dir.Substring(dir.Length - 1, 1) != @"\") dir += @"\";

                    _LogDir = dir;
                    return _LogDir;
                }
            }
        }

        /// <summary>
        /// ��ʼ����־��¼�ļ�
        /// </summary>
        private static void InitLog()
        {
            String path = LogPath;
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
            if (OnWriteLog != null)
            {
                OnWriteLog(null, new WriteLogEventArgs(msg));
                return;
            }

            String str = DateTime.Now.ToString("HH:mm:ss.fff");
            str += " �̣߳�" + Thread.CurrentThread.ManagedThreadId;
            if (!String.IsNullOrEmpty(Thread.CurrentThread.Name)) str += "(" + Thread.CurrentThread.Name + ")";
            if (Thread.CurrentThread.IsThreadPoolThread) str += "(�̳߳�)";

            // ��ʱ�����쳣�߳��У�����ʹ��HttpContext��ǿ�Ƴ�ʼ����־Ŀ¼
            String s = LogPath;

            // ʹ���̳߳��߳�д����־
            //ThreadPool.QueueUserWorkItem(new WaitCallback(PerformWriteLog), str + "\t" + msg);
            PerformWriteLog(str + "\t" + msg);
        }

        /// <summary>
        /// ��ջ���ԡ�
        /// �����ջ��Ϣ�����ڵ���ʱ������������ġ�
        /// ����������ɴ�����־�������á�
        /// </summary>
        public static void DebugStack()
        {
            DebugStack(int.MaxValue);
        }

        /// <summary>
        /// ��ջ���ԡ�
        /// </summary>
        /// <param name="maxNum">��󲶻��ջ������</param>
        public static void DebugStack(int maxNum)
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
            }
            WriteLine(sb.ToString());
        }

        /// <summary>
        /// д��־�¼����󶨸��¼���XTrace�����ٰ���־д����־�ļ���ȥ��
        /// </summary>
        public static event EventHandler<WriteLogEventArgs> OnWriteLog;

        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLine(String format, params Object[] args)
        {
            //����ʱ��ĸ�ʽ��
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != null && args[i].GetType() == typeof(DateTime)) args[i] = ((DateTime)args[i]).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            WriteLine(String.Format(format, args));
        }
        #endregion

        #region ����
        private static Boolean? _Debug;
        /// <summary>�Ƿ���ԡ��������ָ����ֵ����ֻ��ʹ�ô���ָ����ֵ������ÿ�ζ���ȡ���á�</summary>
        public static Boolean Debug
        {
            get
            {
                if (_Debug != null) return _Debug.Value;
                String str = ConfigurationManager.AppSettings["Debug"];
                if (String.IsNullOrEmpty(str)) return false;
                if (str == "1") return true;
                if (str == "0") return false;
                if (str.Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase)) return false;
                if (str.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) return true;
                return false;
            }
            set { _Debug = value; }
        }
        #endregion
    }
}