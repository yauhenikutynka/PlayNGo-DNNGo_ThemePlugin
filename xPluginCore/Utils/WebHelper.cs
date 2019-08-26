using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Resources;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 网页工具类
    /// </summary>
    public static class WebHelper
    {
        static Page Page { get { return HttpContext.Current.Handler as Page; } }

        #region 辅助
        /// <summary>
        /// 输出脚本
        /// </summary>
        /// <param name="script"></param>
        public static void WriteScript(String script)
        {
            HttpContext.Current.Response.Write(String.Format("<script type=\"text/javascript\">\n{0}\n</script>", script));
        }

        /// <summary>
        /// 按字节截取
        /// </summary>
        /// <param name="Str">字符串</param>
        /// <param name="StartIndex">开始位置</param>
        /// <param name="Len">长度</param>
        /// <returns></returns>
        public static String GetSubString(String Str, Int32 StartIndex, Int32 Len)
        {
            int j = 0;
            Int32 RLength = 0;
            Int32 SIndex = 0;
            char[] arr = Str.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                j += (arr[i] > 0 && arr[i] < 255) ? 1 : 2;
                if (j <= StartIndex)
                    SIndex++;
                else
                {
                    if (j > Len + StartIndex) break;
                    RLength++;
                }

            }

            return RLength >= Str.Length ? Str : Str.Substring(StartIndex, RLength);
        }
        #endregion

        #region 弹出信息
        /// <summary>
        /// Js脚本编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String JsEncode(String str)
        {
            if (String.IsNullOrEmpty(str)) return null;

            str = str.Replace(@"\", @"\\");
            str = str.Replace("'", @"\'");
            str = str.Replace(Environment.NewLine, @"\n");
            str = str.Replace("\r", @"\n");
            str = str.Replace("\n", @"\n");

            return str;
        }

        /// <summary>
        /// 弹出页面提示
        /// </summary>
        /// <param name="msg"></param>
        public static void Alert(String msg)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('" + JsEncode(msg) + "');", true);
        }

        /// <summary>
        /// 弹出页面提示并停止输出后退一步！
        /// </summary>
        /// <param name="msg"></param>
        public static void AlertAndEnd(String msg)
        {
            WriteScript("alert('" + JsEncode(msg) + "');history.go(-1);");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出页面提示，并刷新该页面
        /// </summary>
        /// <param name="msg"></param>
        public static void AlertAndRefresh(String msg)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('" + msg + "');location.href = location.href;", true);

            WriteScript("alert('" + JsEncode(msg) + "');location.href = location.href;");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出页面提示并重定向到另外的页面
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public static void AlertAndRedirect(String msg, String url)
        {
            if (!url.Contains("?"))
                url += "?";
            else
                url += "&";

            url += "rnd=";
            url += DateTime.Now.Ticks.ToString();

            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('" + msg + "');location.href = '" + url + "';", true);

            WriteScript("alert('" + JsEncode(msg) + "');location.href = '" + url + "';");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出页面提示并关闭当前页面
        /// </summary>
        /// <param name="msg"></param>
        public static void AlertAndClose(String msg)
        {
            WriteScript("alert('" + JsEncode(msg) + "');window.close();");
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 输入检查
        /// <summary>
        /// 检查控件值是否为空，若为空，显示错误信息，并聚焦到控件上
        /// </summary>
        /// <param name="control">要检查的控件</param>
        /// <param name="errmsg">错误信息。若为空，将使用ToolTip</param>
        /// <returns></returns>
        public static Boolean CheckEmptyAndFocus(Control control, String errmsg)
        {
            if (control == null) throw new ArgumentNullException("control");

            if (control is WebControl && String.IsNullOrEmpty(errmsg)) errmsg = (control as WebControl).ToolTip;

            if (control is TextBox)
            {
                TextBox box = control as TextBox;
                if (!String.IsNullOrEmpty(box.Text)) return true;
            }
            else if (control is ListControl)
            {
                ListControl box = control as ListControl;
                if (!String.IsNullOrEmpty(box.Text)) return true;
            }
            

            control.Focus();
            if (!String.IsNullOrEmpty(errmsg)) Alert(errmsg);
            return false;
        }
        #endregion

        #region 用户主机
        /// <summary>
        /// 用户主机
        /// </summary>
        public static String UserHost
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    String str = (String)HttpContext.Current.Items["UserHostAddress"];
                    if (!String.IsNullOrEmpty(str)) return str;

                    if (HttpContext.Current.Request != null)
                    {
                        str = HttpContext.Current.Request.UserHostName;
                        if (String.IsNullOrEmpty(str)) str = HttpContext.Current.Request.UserHostAddress;
                        HttpContext.Current.Items["UserHostAddress"] = str;
                        return str;
                    }
                }
                return null;
            }
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="filename"></param>
        /// <param name="max"></param>
        public static void ExportExcel(GridView gv, String filename, Int32 max)
        {
            ExportExcel(gv, filename, max, Encoding.Default);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="filename"></param>
        /// <param name="max"></param>
        /// <param name="encoding"></param>
        public static void ExportExcel(GridView gv, String filename, Int32 max, Encoding encoding)
        {
            HttpResponse Response = HttpContext.Current.Response;

            //去掉所有列的排序
            foreach (DataControlField item in gv.Columns)
            {
                if (item is DataControlField) (item as DataControlField).SortExpression = null;
            }
            if (max > 0) gv.PageSize = max;
            gv.DataBind();

            // 新建页面
            Page page = new Page();
            HtmlForm form = new HtmlForm();

            page.EnableEventValidation = false;
            page.Controls.Add(form);
            form.Controls.Add(gv);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = encoding.WebName;
            Response.ContentEncoding = encoding;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename, encoding));
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            page.RenderControl(htw);

            String html = sw.ToString();
            //if (html.StartsWith("<div>")) html = html.SubString("<div>".Length);
            //if (html.EndsWith("</div>")) html = html.SubString(0, html.Length - "</div>".Length);

            html = String.Format("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset={0}\"/>", encoding.WebName) + Environment.NewLine + html;

            Response.Output.Write(html);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 请求相关
        /// <summary>
        /// 获取整型参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Int32 RequestInt(String name)
        {
            String str = HttpContext.Current.Request[name];
            if (String.IsNullOrEmpty(str)) return 0;

            Int32 n = 0;
            if (!Int32.TryParse(str, out n)) n = 0;

            return n;
        }

        /// <summary>
        /// 接收布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool RequestBool(String name)
        {
            return ConvertBool(HttpContext.Current.Request[name]);
        }

        /// <summary>
        /// 接收时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DateTime RequestDateTime(String name)
        {
            return ConvertDateTime(HttpContext.Current.Request[name]);
        }

        /// <summary>
        /// 接收Double
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Double RequestDouble(String name)
        {
            return ConvertDouble(HttpContext.Current.Request[name]);
        }

        /// <summary>
        /// 字符转换为数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Int32 ConvertInt(String val)
        {
            Int32 r = 0;
            if (String.IsNullOrEmpty(val)) return r;
            Int32.TryParse(val, out r);
            return r;
        }

        /// <summary>
        /// 字符转换为布尔
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool ConvertBool(String val)
        {
            bool r = false;
            if (String.IsNullOrEmpty(val)) return r;

            String trimVal = val.Trim();

            if ("True".Equals(trimVal, StringComparison.OrdinalIgnoreCase) || "1".Equals(trimVal))
            {
                return true;
            }
            else if ("False".Equals(trimVal, StringComparison.OrdinalIgnoreCase) || "0".Equals(trimVal))
            {
                return false;
            }

            return r;
        }

        /// <summary>
        /// 字符转换为时间
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTime(String val)
        {
            DateTime r = DateTime.MinValue;
            if (String.IsNullOrEmpty(val)) return r;
            DateTime.TryParse(val, out r);
            return r;
        }

        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Double ConvertDouble(String val)
        {
            Double r = 0;
            if (String.IsNullOrEmpty(val)) return r;
            Double.TryParse(val, out r);
            return r;
        }
        #endregion

        #region HTML字符串缩略方法
        private const int N_STRINGRESTRIC_Length = 14;
        /// <summary>
        ///  HTML字符串缩略方法
        /// </summary>
        /// <param name="strVal">需要缩略的字符串</param>
        /// <returns>缩略后的结果(String...)</returns>
        public static string StringRestric(string strVal)
        {
            return WebHelper.StringRestric(strVal, N_STRINGRESTRIC_Length);
        }

        /// <summary>
        /// HTML字符串缩略方法
        /// </summary>
        /// <param name="strVal">需要缩略的字符串</param>
        /// <param name="len">缩略起始索引</param>
        /// <returns>缩略后的结果(String...)</returns>
        public static string StringRestricSub(string strVal, int len)
        {
            string result = "";

            string strDecode = HttpUtility.HtmlDecode(strVal);

            if (strDecode != null && strDecode.Length > len)
            {
                if (strDecode.Length > 300)
                {
                    result = "<span title=\"" + strVal.Substring(0, 300) + "..." + "\">" + strDecode.Substring(0, len) + "...</span>";
                }
                else
                {
                    result = "<span title=\"" + strVal + "\">" + strDecode.Substring(0, len) + "...</span>";
                }
            }
            else
            {
                result = strDecode;
            }

            return result;
        }

        /// <summary>
        /// HTML字符串缩略方法
        /// </summary>
        /// <param name="strVal">需要缩略的字符串</param>
        /// <param name="len">缩略起始索引</param>
        /// <returns>缩略后的结果(String...)</returns>
        public static string StringRestric(string strVal, int len)
        {
            string result = "";

            string strDecode = HttpUtility.HtmlDecode(strVal);

            //if (strDecode != null && strDecode.Length > len)
            //{
            result = string.Concat("<span class='spanBreak' title='", strVal, "' style='width:", 0, "px;'>", strVal, "</span>");
            //if (strDecode.Length > 300)
            //{
            //    result = "<span title=\"" + strVal.Substring(0, 300) + "..." + "\">" + strDecode.Substring(0, len) + "...</span>";
            //}
            //else
            //{
            //    result = "<span title=\"" + strVal + "\">" + strDecode.Substring(0, len) + "...</span>";
            //}
            //}
            //else
            //{
            //    result = strDecode;
            //}

            return result;
        }

        /// <summary>
        /// HTML字符串缩略方法
        /// </summary>
        /// <param name="strVal">需要缩略的字符串</param>
        /// <param name="len">缩略起始索引</param>
        /// <returns>缩略后的结果(String...)</returns>
        public static string StringRestric(string strVal, int len, int leftWidth)
        {
            string result = "";
            string strDecode = HttpUtility.HtmlDecode(strVal);
            result = string.Concat("<span class='spanBreak' title='", strVal, "' leftwidth='", leftWidth, "' style='width:", 0, "px;'>", strVal, "</span>");
            return result;
        }
        /// <summary>
        /// HTML字符串缩略方法
        /// </summary>
        /// <param name="strVal">需要缩略的字符串</param>
        /// <param name="len">缩略起始索引</param>
        /// <param name="title">指定提示信息</param>
        /// <returns>缩略后的结果(String...)</returns>
        public static string StringRestric(string strVal, int len, string title)
        {
            string result = "";

            string strDecode = HttpUtility.HtmlDecode(strVal);

            if (strDecode != null && strDecode.Length > len)
            {
                string tempStr = String.Empty;
                if (title.Length > 300)
                {
                    tempStr = strDecode.Substring(0, len);
                    if (tempStr.LastIndexOf('<') <= tempStr.LastIndexOf('>'))
                    {
                        result = "<span title=\"" + title.Substring(0, 300) + "..." + "\">" + tempStr + "...</span>";
                    }
                    else
                    {
                        tempStr = tempStr.Substring(0, tempStr.LastIndexOf('<'));
                        result = "<span title=\"" + title.Substring(0, 300) + "..." + "\">" + tempStr + "...</span>";
                    }
                }
                else
                {
                    tempStr = strDecode.Substring(0, len);
                    if (tempStr.LastIndexOf('<') <= tempStr.LastIndexOf('>'))
                    {
                        result = "<span title=\"" + title + "\">" + tempStr + "...</span>";
                    }
                    else
                    {
                        tempStr = tempStr.Substring(0, tempStr.LastIndexOf('<'));
                        result = "<span title=\"" + title + "\">" + tempStr + "...</span>";
                    }
                }
            }
            else
            {
                result = strDecode;
            }

            return result;
        }
        #endregion

        #region HTML字符串换行方法
        private const int N_AutoBreakWordLength = 120;
        /// <summary>
        /// HTML字符串换行方法
        /// </summary>
        /// <param name="str">需要自动换行字符串</param>
        /// <returns>返回自动换行后的值</returns>
        public static string AutoBreakWord(string str)
        {
            return AutoBreakWord(str, N_AutoBreakWordLength);
        }

        /// <summary>
        ///  HTML字符串换行方法      
        /// </summary>
        /// <param name="str">需要自动换行字符串</param>
        /// <param name="length">位数</param>
        /// <returns>返回自动换行后的值</returns>
        public static string AutoBreakWord(string str, int length)
        {
            if ((str == null) || (str == ""))
                return str;
            str = str.Replace("\r", "");
            int nSpaceIndex = str.IndexOf(" ");
            int nLastIndex = str.LastIndexOf("\n");
            int nLength = str.Length;
            StringBuilder sbResult = new StringBuilder();
            string[] straStr = str.Split(new string[1] { "\n" }, StringSplitOptions.None);
            if (straStr.Length > 0)
            {
                if (nSpaceIndex > 0 || nLastIndex > 0)
                {
                    bool IsEnter = true;
                    for (int m = 0; m < straStr.Length; m++)
                    {
                        if (straStr[m].Length > length)
                        {
                            int count = straStr[m].Length / length;
                            for (int i = 0; i < count; i++)
                            {
                                string strTemp = straStr[m].Substring(i * length, length);
                                int nTempSpaceIndex = strTemp.LastIndexOf(" ");
                                if (nTempSpaceIndex > 0)
                                {
                                    strTemp = strTemp.Insert(nTempSpaceIndex, "\n");
                                    sbResult.Append(strTemp);
                                }
                                else
                                {
                                    sbResult.Append(strTemp);
                                    sbResult.Append("\n");
                                    IsEnter = false;
                                }
                            }
                            if (straStr[m].Length % length != 0)
                            {
                                string strOther = straStr[m].Substring(count * length);
                                if (!IsEnter)
                                {
                                    int nOtherSpaceIndex = strOther.LastIndexOf(" ");
                                    if (nOtherSpaceIndex > 0)
                                        strOther = strOther.Insert(nOtherSpaceIndex, "\n");
                                }
                                sbResult.Append(strOther);
                            }
                        }
                        else
                        {
                            sbResult.Append(straStr[m]);
                        }
                        sbResult.Append("\n");
                    }
                }
                else
                {
                    for (int m = 0; m < straStr.Length; m++)
                    {
                        if (straStr[m].Length > length)
                        {
                            int count = straStr[m].Length / length;
                            for (int i = 0; i < count; i++)
                            {
                                sbResult.Append(straStr[m].Substring(i * length, length))
                                    .Append("\n");
                            }
                            if (straStr[m].Length % length != 0)
                            {
                                sbResult.Append(straStr[m].Substring(count * length));
                            }
                        }
                        else
                        {
                            sbResult.Append(straStr[m]);
                        }
                        sbResult.Append("\n");
                    }
                }
                if (nLastIndex < nLength - 1)
                {
                    sbResult.Remove(sbResult.Length - 1, 1);
                }
            }
            return sbResult.ToString().Replace(" ", "&nbsp;").Replace("\n", "<br>");
        }

        #endregion

        #region 格式相关
        /// <summary>
        ///将字符串文本格式转换为HTML格式
        /// </summary>
        /// <param name="Content">需要转换的文本</param>
        /// <returns>转换后的文本</returns>
        public static string FormatStringToHTML(string Content)
        {
            string text = Content;
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("'", "''");
            text = text.Replace(" ", "&nbsp;");
            text = text.Replace("\n", "<br/>");
            text = text.Replace("\r\n", "<br/>");
            text = text.Trim();
            return text;

        }

        /// <summary>
        /// 返回指定个数个HTML空格
        /// </summary>
        /// <param name="num">指定个数</param>
        /// <returns>HTML结果</returns>
        public static string GetSpace(int num)
        {
            string space = "";
            for (int i = 0; i < num; i++)
            {
                space += "&nbsp;";
            }
            return space;
        }

        #endregion

        #region 数据绑定方法

        private static void BindList(ListControl list, object dataSource, string textField, string valueField)
        {
            list.DataSource = dataSource;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
        private static string FormatUrl(string path)
        {
            if (path[0] == '\\')
            {
                path = path.TrimStart('\\');
            }
            path = path.Replace('\\', '/');
            if (path[path.Length - 1] != '/')
            {
                path += "/";
            }
            return path;
        }

        #region 绑定DropDownList、ListBox等

        /// <summary>
        /// 绑定DropDownList、ListBox等
        /// modify by wsliu
        /// 修改记录：方法内部实现是自己调用自己，为死循环，导致IIS错误
        /// </summary>
        /// <param name="list">控件ID</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="textField">显示的文本字段</param>
        /// <param name="valueField">值字段</param>
        public static void BindList(ListControl list, DataSet dataSource, string textField, string valueField)
        {
            //BindList(list, dataSource, textField, valueField);
            BindList(list, (object)dataSource, textField, valueField);
        }
        /// <summary>
        /// 绑定DropDownList、ListBox等
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource">枚举类型</param>
        public static void BindList(ListControl list, Type dataSource)
        {
            System.Data.DataTable dt = EnumHelper.GetEnumTable(dataSource);

            BindList(list, dt.DefaultView, "Text", "Value");
        }

        /// <summary>
        /// 绑定DropDownList、ListBox等
        /// modify by wsliu
        /// 修改记录：方法内部实现是自己调用自己，为死循环，导致IIS错误
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="textField">显示的文本字段</param>
        /// <param name="valueField">值字段</param>
        public static void BindList<T>(ListControl list, List<T> dataSource, string textField, string valueField)
        {
            //BindList(list, dataSource, textField, valueField);
            BindList(list, (object)dataSource, textField, valueField);
        }

        /// <summary>
        /// 绑定DropDownList、ListBox等
        /// modify by wsliu
        /// 修改记录：方法内部实现是自己调用自己，为死循环，导致IIS错误
        /// </summary>
        /// <param name="list">控件ID</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="textField">显示的文本字段</param>
        /// <param name="valueField">值字段</param>
        public static void BindList(ListControl list, DataView dataSource, string textField, string valueField)
        {
            //BindList(list, dataSource, textField, valueField);
            BindList(list, (object)dataSource, textField, valueField);
        }


        /// <summary>
        /// 绑定DropDownList、ListBox等
        /// modify by wsliu
        /// 修改记录：方法内部实现是自己调用自己，为死循环，导致IIS错误
        /// </summary>
        /// <param name="list">控件ID</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="textField">显示的文本字段</param>
        /// <param name="valueField">值字段</param>
        public static void BindList(ListControl list, ArrayList dataSource, string textField, string valueField)
        {
            //BindList(list, dataSource, textField, valueField);
            BindList(list, (object)dataSource, textField, valueField);
        }


        /// <summary>
        /// DataList数据绑定, 设置当DataList中的项目数为0时是否自动隐藏该DataList
        /// </summary>
        /// <param name="list">DataList</param>
        /// <param name="dv">DataView</param>
        /// <param name="keyField">绑定的键字段</param>
        /// <param name="sortText">排序表达式</param>
        /// <param name="isAutoHidden">是否自动隐藏</param>
        public static void BindList(DataList list, DataView dv, string keyField,
            string sortText, bool isAutoHidden)
        {
            if (sortText != null)
            {
                dv.Sort = sortText;
            }

            list.DataSource = dv;

            if (keyField != null)
            {
                list.DataKeyField = keyField;
            }

            list.DataBind();

            //如果没有物品则将DataList置为不可见
            if (isAutoHidden)
            {
                if (list.Items.Count < 1)
                {
                    list.Visible = false;
                }
                else
                {
                    list.Visible = true;
                }
            }
        }


        /// <summary>
        /// DataList数据绑定, 默认不自动隐藏
        /// </summary>
        /// <param name="list">DataList</param>
        /// <param name="dv">DataView</param>
        /// <param name="keyField">绑定的键字段</param>
        /// <param name="sortText">排序表达式</param>
        public static void BindList(DataList list, DataView dv, string keyField, string sortText)
        {
            BindList(list, dv, keyField, sortText, false);
        }

        /// <summary>
        /// 绑定选择项
        /// </summary>
        /// <param name="list">DataList控件</param>
        /// <param name="textField">绑定字段名</param>
        /// <param name="valueField">绑定字段值</param>
        public static void BindItem(ListControl list, string textField, string valueField)
        {
            list.Items.Insert(0, new ListItem(textField, valueField, true));
        }
        /// <summary>
        /// 绑定选择项
        /// </summary>
        /// <param name="list">DataList控件</param>
        public static void BindItem(ListControl list)
        {
            BindItem(list, "[请选择]", "0");
        }


        #endregion


        #region List 控件的赋值取值，包括可以多选的List控件


        /// <summary>
        /// Help to set value to be selected
        /// </summary>
        /// <param name="list">list control</param>
        /// <param name="value">value</param>
        public static void SelectedListByValue(ListControl list, object value)
        {
            if (value == DBNull.Value || value == null)
            {
                list.SelectedIndex = -1;
            }
            else
            {
                list.SelectedIndex = list.Items.IndexOf(list.Items.FindByValue(value.ToString()));
            }
        }


        /// <summary>
        /// Help to set value to be selected
        /// </summary>
        /// <param name="list">list control</param>
        /// <param name="text">text</param>
        public static void SelectedListByText(ListControl list, string text)
        {
            list.SelectedIndex = -1;
            list.SelectedIndex = list.Items.IndexOf(list.Items.FindByText(text.ToString()));
        }

        /// <summary>
        /// 获取列表控件被多项选择的文本和值，文本格式为“a,b,c,d”，值格式为“1,2,3,4”
        /// </summary>
        /// <param name="list"></param>
        /// <param name="textStr"></param>
        /// <param name="idStr"></param>
        public static void GetSelected(ListControl list, out string textStr, out string idStr)
        {
            textStr = "";
            idStr = ",";
            foreach (ListItem item in list.Items)
            {
                if (item.Selected)
                {
                    textStr += item.Text + ",";
                    idStr += item.Value + ",";
                }
            }
            if (textStr.Length > 0)
            {
                textStr = textStr.TrimEnd(',');
            }
        }


        /// <summary>
        /// 根据值的字符串，选定多个项目
        /// alert by huzhang 2006-1-5
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value">值格式为“1,2,3,4”</param>
        public static void SelectedListMultiByValue(ListControl list, string value)
        {
            if (IsNotNull(value))
            {
                list.SelectedIndex = -1;
                value = "," + value + ",";
                string strSelected;
                foreach (ListItem item in list.Items)
                {
                    strSelected = "," + item.Value + ",";
                    if (value.IndexOf(strSelected) > -1)
                    {
                        item.Selected = true;
                        value.Replace(strSelected, ",");
                        if (value == ",")
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// 根据值的字符串，选定多个项目
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="splitFlag"></param>
        public static void SelectedListMultiByValue(ListControl list, string value, string splitFlag)
        {
            if (IsNotNull(value))
            {
                list.SelectedIndex = -1;
                value = splitFlag + value + splitFlag;
                string strSelected;
                foreach (ListItem item in list.Items)
                {
                    strSelected = splitFlag + item.Value + splitFlag;
                    if (value.IndexOf(strSelected) > -1)
                    {
                        item.Selected = true;
                        value.Replace(strSelected, splitFlag);
                        if (value == splitFlag)
                            break;
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// 判断时候为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNull(string str)
        {
            return string.IsNullOrEmpty(str);
        }
        #endregion



        #region Helper Methods

        /// <summary>
        /// Gets the string param.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="errorReturn">The error return.</param>
        /// <returns>The param value.</returns>
        public static string GetStringParam(System.Web.HttpRequest request, string paramName, string errorReturn, Boolean IsLostHTML = true)
        {
            string retStr = errorReturn;
            if (request.QueryString[paramName] != null)
            {
                /* 过滤掉传参中的HTML和JS,避免XSS攻击 */
                retStr = IsLostHTML ? Common.LostXSS(request.QueryString[paramName]) : request.QueryString[paramName];
            }

            //string retStr = request.QueryString[paramName];
            if (request.Form[paramName] != null)
            {
                retStr = request.Form[paramName];
            }

            return retStr;
        }

        /// <summary>
        /// Gets the int param.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="errorReturn">The error return.</param>
        /// <returns>The param value.</returns>
        public static int GetIntParam(System.Web.HttpRequest request, string paramName, int errorReturn)
        {
            string retStr = request.Form[paramName];
            if (retStr == null)
            {
                retStr = request.QueryString[paramName];
            }
            if (retStr == null || retStr.Trim() == string.Empty)
            {
                return errorReturn;
            }
            try
            {
                return Convert.ToInt32(retStr);
            }
            catch
            {
                return errorReturn;
            }
        }

        /// <summary>
        /// Gets the date time param.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="errorReturn">The error return.</param>
        /// <returns>The param value.</returns>
        public static DateTime GetDateTimeParam(System.Web.HttpRequest request, string paramName, DateTime errorReturn)
        {
            string retStr = request.Form[paramName];
            if (retStr == null)
            {
                retStr = request.QueryString[paramName];
            }
            if (retStr == null || retStr.Trim() == string.Empty)
            {
                return errorReturn;
            }
            try
            {
                return Convert.ToDateTime(HttpUtility.UrlDecode(retStr));
            }
            catch
            {
                return errorReturn;
            }
        }



        /// <summary>
        /// Gets the Boolean param.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="errorReturn">The error return.</param>
        /// <returns>The param value.</returns>
        public static Boolean GetBooleanParam(System.Web.HttpRequest request, string paramName, Boolean errorReturn)
        {
            string retStr = request.Form[paramName];
            if (retStr == null)
            {
                retStr = request.QueryString[paramName];
            }
            if (retStr == null || retStr.Trim() == string.Empty)
            {
                return errorReturn;
            }
            try
            {
                return Convert.ToBoolean(retStr);
            }
            catch
            {
                return errorReturn;
            }
        }

        /// <summary>
        /// Strongs the typed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The strong typed instance.</returns>
        public static ObjectType StrongTyped<ObjectType>(object obj)
        {
            return (ObjectType)obj;
        }

        /// <summary>
        /// Toes the js single quote safe string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>The formated str.</returns>
        public static string ToJsSingleQuoteSafeString(string str)
        {
            return str.Replace("'", "\\'");
        }

        /// <summary>
        /// Toes the js double quote safe string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>The formated str.</returns>
        public static string ToJsDoubleQuoteSafeString(string str)
        {
            return str.Replace("\"", "\\\"");
        }

        /// <summary>
        /// Toes the VBS quote safe string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>The formated str.</returns>
        public static string ToVbsQuoteSafeString(string str)
        {
            return str.Replace("\"", "\"\"");
        }

        /// <summary>
        /// Toes the SQL quote safe string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>The formated str.</returns>
        public static string ToSqlQuoteSafeString(string str)
        {
            return str.Replace("'", "''");
        }

        /// <summary>
        /// Texts to HTML.
        /// </summary>
        /// <param name="txtStr">The TXT STR.</param>
        /// <returns>The formated str.</returns>
        public static string TextToHtml(string txtStr)
        {
            return txtStr.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").
                Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        /// <summary>
        /// 根据字符串，利用“,”分隔获取集合
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static List<string> GetList(string strs)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(strs))
            {
                strs = strs.Replace("，", ",");
                strs = strs.TrimStart(',').TrimEnd(',');
                string[] strArray = strs.Split(',');
                list.AddRange(strArray);
            }
            return list;
        }


        /// <summary>
        /// 根据字符串，利用“,”分隔获取集合
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static List<string> GetList(string strs, string separator)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(strs))
            {
                string[] strArray = strs.Split(new string[]{ separator},  StringSplitOptions.None);
                list.AddRange(strArray);
            }
            return list;
        }
        #endregion

        #region Resource

        private static Dictionary<string, Hashtable> stringResources = new Dictionary<string, Hashtable>();

        private static System.Globalization.CultureInfo defaultCulture = null;

        /// <summary>
        /// Gets or sets the default culture.
        /// </summary>
        /// <value>The default culture.</value>
        public static System.Globalization.CultureInfo DefaultCulture
        {
            get
            {
                return defaultCulture ?? System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                defaultCulture = value;
            }
        }

        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="ci">The ci.</param>
        public static void LoadResources(string resourceName, System.Globalization.CultureInfo ci)
        {
            string resFileName = System.Web.HttpRuntime.BinDirectory + resourceName + "." + ci.ToString() + ".resources";
            if (System.IO.File.Exists(resFileName))
            {
                lock (stringResources)
                {
                    if (!stringResources.ContainsKey(ci.ToString()))
                    {
                        stringResources.Add(ci.ToString(), new Hashtable());

                        try
                        {
                            ResourceReader reader = new ResourceReader(resFileName);
                            IDictionaryEnumerator en = reader.GetEnumerator();
                            while (en.MoveNext())
                            {
                                stringResources[ci.ToString()].Add(en.Key, en.Value);
                            }
                            reader.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the resources.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public static void LoadResources(string resourceName)
        {
            LoadResources(resourceName, DefaultCulture);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The resouce value.</returns>
        public static string GetString(string key)
        {
            return GetString(key, WebHelper.DefaultCulture);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ci">The ci.</param>
        /// <returns>The resouce value.</returns>
        public static string GetString(string key, System.Globalization.CultureInfo ci)
        {
            if (stringResources.ContainsKey(ci.ToString()))
            {
                if (stringResources[ci.ToString()].Contains(key))
                {
                    return stringResources[ci.ToString()][key].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 清除集合中重复的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> ListTrimRepeated(List<string> list)
        {
            List<string> returnList = new List<string>();
            if (list != null && list.Count > 0)
            {
                returnList.Add(list[0]);
                foreach (string str in list)
                {
                    if (!returnList.Contains(str))
                    {
                        returnList.Add(str);
                    }
                }
            }
            return returnList;

        }

        /// <summary>
        /// 根据集合拼成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetStrings(List<string> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string str in list)
                {
                    sb.Append(str).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region 验证数据(使用到DataSet和DataTable取数据的时候使用)
        /// <summary>
        /// 验证数据集
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool CheckDataSet(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0);
        }

        /// <summary>
        /// 验证数据表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CheckDataTable(DataTable dt)
        {
            return (dt != null && dt.Rows.Count > 0);
        }

        /// <summary>
        /// 验证Object数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CheckObject(object obj)
        {
            return (obj != null && obj != DBNull.Value);
        }
        #endregion


        #region "字符串截取"
        /// <summary>
        /// 按字节位数截取字符串
        /// </summary>
        /// <param name="str_value">待截取的字符串</param>
        /// <param name="str_len">长度</param>
        /// <returns></returns>
        public static string leftx(string str_value, int str_len)
        {
            int p_num = 0;
            int i;
            string New_Str_value = str_value;

            if (string.IsNullOrEmpty(   str_value))
            {
                New_Str_value = "";
            }
            else
            {
                int Len_Num = str_value.Length;

                if (Len_Num > str_len)
                {

                    for (i = 0; i <= Len_Num - 1; i++)
                    {
                        if (i > Len_Num) break;
                        char c = Convert.ToChar(str_value.Substring(i, 1));
                        if (((int)c > 255) || ((int)c < 0))
                            p_num = p_num + 2;
                        else
                            p_num = p_num + 1;



                        if (p_num >= str_len)
                        {

                            New_Str_value = str_value.Substring(0, i + 1);
                            break;
                        }
                        else
                        {
                            New_Str_value = str_value;
                        }

                    }
                }

            }
            return New_Str_value;
        }

        /// <summary>
        /// 按字节位数截取字符串
        /// </summary>
        /// <param name="str_value">待截取的字符串</param>
        /// <param name="str_len">长度</param>
        /// <param name="Suffix">后缀</param>
        /// <returns></returns>
        public static string leftx(string str_value, int str_len, string Suffix)
        {
            string tx = leftx(str_value, str_len);
            //当字符串进行截取后,可匹配增加后缀的缩略符号
            if ( !String.IsNullOrEmpty(str_value) &&  tx.Length < str_value.Length)
            {
                tx = tx + Suffix;
            }

            return tx;
        }


        #endregion


        #region "获取页面url"
        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// 检测当前url是否包含指定的字符
        /// </summary>
        /// <param name="sChar">要检测的字符</param>
        /// <returns></returns>
        public static bool CheckScriptNameChar(string sChar)
        {
            bool rBool = false;
            if (GetScriptName.ToLower().LastIndexOf(sChar) >= 0)
                rBool = true;
            return rBool;
        }

        /// <summary>
        /// 获取当前页面的扩展名
        /// </summary>
        public static string GetScriptNameExt
        {
            get
            {
                return GetScriptName.Substring(GetScriptName.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// 获取当前访问页面地址参数
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
            }
        }

        /// <summary>
        /// 获得页面文件名和参数名
        /// </summary>
        public static string GetScriptNameUrl
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                Script_Name += "?" + GetScriptNameQueryString;
                return Script_Name;
            }
        }

        /// <summary>
        /// 获得当前页面的文件名
        /// </summary>
        public static string GetScriptFileName
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                return Script_Name;
            }
        }

        /// <summary>
        /// 获取当前访问页面Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return GetScriptNameQueryString == "" ? GetScriptName : string.Format("{0}?{1}", GetScriptName, GetScriptNameQueryString);
            }
        }

        /// <summary>
        /// 返回当前页面目录的url
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static string GetHomeBaseUrl(string FileName)
        {
            string Script_Name = GetScriptName;
            return string.Format("{0}/{1}", Script_Name.Remove(Script_Name.LastIndexOf("/")), FileName);
        }

        /// <summary>
        /// 返回当前网站网址
        /// </summary>
        /// <returns></returns>
        public static string GetHomeUrl()
        {
            return HttpContext.Current.Request.Url.Authority;
        }

        /// <summary>
        /// 获取当前访问文件物理目录
        /// </summary>
        /// <returns>路径</returns>
        public static string GetScriptPath
        {
            get
            {
                string Paths = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"].ToString();
                return Paths.Remove(Paths.LastIndexOf("\\"));
            }
        }
        #endregion
    }
}