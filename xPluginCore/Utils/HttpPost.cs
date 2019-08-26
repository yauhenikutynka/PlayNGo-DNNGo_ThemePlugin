using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Specialized;
using System.Web;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// Http请求类
    /// </summary>
    public class HttpPost
    {

        private Uri _Url;
        /// <summary>
        /// 访问Url(不带参数)
        /// </summary>
        public Uri Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// 访问的URL(带参数)
        /// </summary>
        public String FormatUrl
        {
            get {
                StringBuilder Result = new StringBuilder(Url.AbsoluteUri);
                String s = FormatQueryString;
                if (!String.IsNullOrEmpty(s))
                {
                    Result.Append("?").Append(s);
                }
                return Result.ToString();
            }
        }

        private NameValueCollection _Form;
        /// <summary>
        /// Form取值
        /// </summary>
        public NameValueCollection Form
        {
            get { return _Form; }
            set { _Form = value; }
        }


        private NameValueCollection _QueryString;
        /// <summary>
        /// 查询字符串
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _QueryString; }
            set { _QueryString = value; }
        }

        public String FormatQueryString
        {
            get
            {
                StringBuilder Result = new StringBuilder();
                if (QueryString != null && QueryString.Count > 0)
                {
                    foreach (String Name in QueryString.AllKeys)
                    {
                        if (Result.Length > 0)
                        {
                            Result.Append("&");
                        }
                        Result.AppendFormat("{0}={1}", Name, HttpUtility.UrlEncode(QueryString[Name]));
                    }
                }
                return Result.ToString();
            }
        }


        /// <summary>
        /// Form字符串
        /// </summary>
        public String FormatFormString
        {
            get {
                StringBuilder Result = new StringBuilder();
                if (Form != null && Form.Count > 0)
                {
                    foreach (String Name in Form.AllKeys)
                    {
                        if (Result.Length > 0)
                        {
                            Result.Append("&");
                        }
                        Result.AppendFormat("{0}={1}", Name, HttpUtility.UrlEncode(Form[Name]));
                    }
                }else
                {
                    Result.Append(_FormString);
                }
                return Result.ToString();
            }
        }

        private String _FormString;
        /// <summary>
        /// Form字符串
        /// </summary>
        public String FormString
        {
            set { _FormString = value; }
            get { return _FormString; }
        }

      


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns></returns>
        public String Post()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FormatUrl);
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可
            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链
            //request.Referer = “http://localhost/index.php”;
            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers["Accept-Language"] = "en-US,en;q=0.";
            request.Headers["Accept-Charset"] = "utf-8;q=0.7,*;q=0.3";
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
            request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
            byte[] postData = encoding.GetBytes(FormatFormString);//FormString即为发送的数据
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            return retString;

        }

    }
}
