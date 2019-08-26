using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Common;
using DotNetNuke.Entities.Host;

namespace DNNGo.Modules.ThemePlugin
{
    public class FileSystemUtils : DotNetNuke.Common.Utilities.FileSystemUtils
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="FileLoc">文件真实路径</param>
        /// <param name="FileName">显示文件名</param>
        public static void DownloadFile(string FileLoc, string FileName)
        {
            System.IO.FileInfo objFile = new System.IO.FileInfo(FileLoc);
            System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
            string truefilename = objFile.Name;
            if (HttpContext.Current.Request.UserAgent.IndexOf("; MSIE ") > 0)
            {
                truefilename = HttpUtility.UrlEncode(truefilename, System.Text.Encoding.UTF8);
            }
            if (objFile.Exists)
            {
                objResponse.ClearContent();
                objResponse.ClearHeaders();
                objResponse.AppendHeader("content-disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(FileName) + "\"");
                objResponse.AppendHeader("Content-Length", objFile.Length.ToString());
                objResponse.ContentType = GetContentType(objFile.Extension.Replace(".", ""));
                WriteFile(objFile.FullName);
                objResponse.Flush();
                objResponse.End();
            }
        }

        public static void WriteFile(string strFileName)
        {
            System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
            System.IO.Stream objStream = null;
            try
            {
                objStream = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                WriteStream(objResponse, objStream);
            }
            catch (Exception ex)
            {
                objResponse.Write("Error : " + ex.Message);
            }
            finally
            {
                if (objStream != null)
                {
                    objStream.Close();
                    objStream.Dispose();
                }
            }
        }

        private static void WriteStream(HttpResponse objResponse, Stream objStream)
        {
            byte[] bytBuffer = new byte[10000];
            int intLength;
            long lngDataToRead;
            try
            {
                lngDataToRead = objStream.Length;
                while (lngDataToRead > 0)
                {
                    if (objResponse.IsClientConnected)
                    {
                        intLength = objStream.Read(bytBuffer, 0, 10000);
                        objResponse.OutputStream.Write(bytBuffer, 0, intLength);
                        objResponse.Flush();

                        lngDataToRead = lngDataToRead - intLength;
                    }
                    else
                    {
                        lngDataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.Write("Error : " + ex.Message);
            }
            finally
            {
                if (objStream != null)
                {
                    objStream.Close();
                    objStream.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取资源文件夹中的内容
        /// </summary>
        /// <param name="PathName">文件夹名</param>
        /// <param name="FileName">文件名</param>
        /// <param name="pmb">当前模块对象</param>
        /// <returns></returns>
        public static String Resource(String PathName, String FileName, BaseModule pmb)
        {
            return String.Format("{0}{1}/{2}", pmb.ModulePath, PathName, FileName);
        }

        /// <summary>
        /// 创建空文本文件
        /// </summary>
        /// <param name="FullFileName"></param>
        public static void CreateText(String FullFileName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(FullFileName);
            if (!file.Exists)
            {
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }


                File.CreateText(FullFileName);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hpFile"></param>
        /// <param name="pmb"></param>
        /// <returns></returns>
        public static String UploadFile(HttpPostedFile httpFile, BaseModule pmb)
        {

            String FileName = httpFile.FileName;
            if (FileName.IndexOf(@"\") >= 0) FileName = FileName.Substring(FileName.LastIndexOf(@"\"), FileName.Length - FileName.LastIndexOf(@"\")).Replace(@"\", "");

            String Extension = Path.GetExtension(FileName).Replace(".", "");


            //构造保存路径
            String FileUrl = FileName;
            System.IO.FileInfo file = new System.IO.FileInfo(pmb.MapPath(String.Format("{0}FileUploads/{1}/{2}", pmb.SkinPath, pmb.PortalId, FileName)));
            if (!file.Directory.Exists) file.Directory.Create();

            int ExistsCount = 1;
            //检测文件名是否存在
            while (file.Exists)
            {
                FileUrl = String.Format("{0}_{1}.{2}", FileName.Replace("." + Extension, ""), ExistsCount, Extension);
                file = new System.IO.FileInfo(pmb.MapPath(String.Format("{0}FileUploads/{1}/{2}", pmb.SkinPath, pmb.PortalId, FileUrl)));
                ExistsCount++;
            }

            //保存文件到文件夹
            httpFile.SaveAs(file.FullName);

            return String.Format("FileUploads/{0}/{1}",pmb.PortalId, FileUrl);
        }


        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        /// <param name="overwrite">覆盖</param>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                if (File.Exists(sourceFileName))
                {
                    File.Copy(sourceFileName, destFileName, overwrite);
                }
            }
            catch { }
        }

        /// <summary>
        /// 复制模版
        /// </summary>
        /// <param name="TemplateDir"></param>
        public static void CopyTemplate(String TemplateDir, String TemplateName)
        {
            //HomePage.doctype.xml
            //HomePage.jpg
            String jpg1 = String.Format("{0}\\xTemplate\\{1}.jpg", TemplateDir, TemplateName);
            String jpg2 = String.Format("{0}\\{1}.jpg", TemplateDir, TemplateName);
            Copy(jpg1, jpg2, true);

            String doctype1 = String.Format("{0}\\xTemplate\\{1}.doctype.xml", TemplateDir, TemplateName);
            String doctype2 = String.Format("{0}\\{1}.doctype.xml", TemplateDir, TemplateName);
            Copy(doctype1, doctype2, true);
        }

        /// <summary>
        /// 复制新建模版
        /// </summary>
        /// <param name="TemplateDir"></param>
        /// <param name="SourceTemplate"></param>
        /// <param name="NewTemplate"></param>
        public static void CopyTemplateToNew(String TemplateDir, String SourceTemplate, String NewTemplate)
        {

            String jpg1 = String.Format("{0}\\xTemplate\\{1}.jpg", TemplateDir, SourceTemplate);
            String jpg2 = String.Format("{0}\\xTemplate\\{1}.jpg", TemplateDir, NewTemplate);
            Copy(jpg1, jpg2, true);

            String doctype1 = String.Format("{0}\\xTemplate\\{1}.doctype.xml", TemplateDir, SourceTemplate);
            String doctype2 = String.Format("{0}\\xTemplate\\{1}.doctype.xml", TemplateDir, NewTemplate);
            Copy(doctype1, doctype2, true);

            String ascx1 = String.Format("{0}\\xTemplate\\{1}.ascx", TemplateDir, SourceTemplate);
            String ascx2 = String.Format("{0}\\xTemplate\\{1}.ascx", TemplateDir, NewTemplate);
            Copy(ascx1, ascx2, true);

            String css1 = String.Format("{0}\\xTemplate\\{1}.css", TemplateDir, SourceTemplate);
            String css2 = String.Format("{0}\\xTemplate\\{1}.css", TemplateDir, NewTemplate);
            Copy(css1, css2, true);

            String options1 = String.Format("{0}\\xTemplate\\{1}.options.xml", TemplateDir, SourceTemplate);
            String options2 = String.Format("{0}\\xTemplate\\{1}.options.xml", TemplateDir, NewTemplate);
            Copy(options1, options2, true);


            String Storage1 = String.Format("{0}\\xTemplate\\{1}.Storage.xml", TemplateDir, SourceTemplate);
            String Storage2 = String.Format("{0}\\xTemplate\\{1}.Storage.xml", TemplateDir, NewTemplate);
            Copy(Storage1, Storage2, true); 

        }

        public new static bool CheckValidFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            //regex matches a dot followed by 1 or more chars followed by a semi-colon
            //regex is meant to block files like "foo.asp;.png" which can take advantage
            //of a vulnerability in IIS6 which treasts such files as .asp, not .png
            return !string.IsNullOrEmpty(extension)
                   && Host.AllowedExtensionWhitelist.IsAllowedExtension(extension)
                   && !Globals.FileExtensionRegex.IsMatch(fileName);
        }

        public new static string GetContentType(string extension)
        {
            return new FileManager().GetContentType(extension);
        }


    }
}
