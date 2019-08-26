using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using System.Drawing;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.ThemePlugin
{
    public class Resource_jQueryFileUploadHandler : IHttpHandler
    {
		private readonly JavaScriptSerializer js = new JavaScriptSerializer();


        #region "��ȡDNN����"

        public Int32 PortalId = WebHelper.GetIntParam(HttpContext.Current.Request, "PortalId", 0);
        /// <summary>
        /// ģ����
        /// </summary>
        public Int32 ModuleId = WebHelper.GetIntParam(HttpContext.Current.Request, "ModuleId", 0);
        /// <summary>
        /// ҳ����
        /// </summary>
        public Int32 TabId = WebHelper.GetIntParam(HttpContext.Current.Request, "TabId", 0);

        /// <summary>
        /// ·��
        /// </summary>
        public String ModulePath = WebHelper.GetStringParam(HttpContext.Current.Request, "ModulePath", "");


        /// <summary>
        /// �û���Ϣ
        /// </summary>
        public UserInfo UserInfo
        {
            get { return UserController.GetCurrentUserInfo(); }
        }




        /// <summary>
        /// �Ƿ���и�ģ��ı༭Ȩ��
        /// </summary>
        public Boolean IsEdit
        {
            get { return UserInfo != null && UserInfo.UserID > 0 && (IsAdministrator || DotNetNuke.Security.Permissions.ModulePermissionController.HasModuleAccess(DotNetNuke.Security.SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration)); }
            //get { return UserId > 0 && (IsAdministrator ||  PortalSecurity.HasEditPermissions(ModuleId,TabId)); }
        }


        /// <summary>
        /// �Ƿ񳬼�����Ա
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"); }
        }

        private ModuleInfo _ModuleConfiguration = new ModuleInfo();
        /// <summary>
        /// ģ����Ϣ
        /// </summary>
        public ModuleInfo ModuleConfiguration
        {
            get
            {
                if (!(_ModuleConfiguration != null && _ModuleConfiguration.ModuleID > 0) && ModuleId > 0)
                {
                    ModuleController mc = new ModuleController();
                    _ModuleConfiguration = mc.GetModule(ModuleId, TabId);

                }
                return _ModuleConfiguration;
            }
        }

        private PortalSettings _portalSettings;
        /// <summary>
        /// վ������
        /// </summary>
        public PortalSettings PortalSettings
        {
            get
            {
                if (!(_portalSettings != null && _portalSettings.PortalId != Null.NullInteger))
                {
                    PortalAliasInfo objPortalAliasInfo = new PortalAliasInfo();
                    objPortalAliasInfo.PortalID = PortalId;
                    _portalSettings = new PortalSettings(TabId, objPortalAliasInfo);
                }
                return _portalSettings;
            }
        }



        private TabInfo _tabInfo;
        /// <summary>
        /// ҳ����Ϣ
        /// </summary>
        public TabInfo TabInfo
        {
            get
            {
                if (!(_tabInfo != null && _tabInfo.TabID > 0) && TabId > 0)
                {
                    TabController tc = new TabController();
                    _tabInfo = tc.GetTab(TabId);

                }

                return _tabInfo;


            }
        }

        #endregion

 



 
		public bool IsReusable { get { return false; } }

		public void ProcessRequest (HttpContext context) {
			context.Response.AddHeader("Pragma", "no-cache");
			context.Response.AddHeader("Cache-Control", "private, no-cache");
			HandleMethod(context);
           
		}
        
		// Handle request based on method
		private void HandleMethod (HttpContext context) {

            //You are not allowed to keep executing without the permission
            if (!IsEdit) throw new HttpRequestValidationException("You are not permitted to access this page! :(");

            switch (context.Request.HttpMethod) {
				case "HEAD":
				case "GET":
					if (GivenFilename(context)) DeliverFile(context);
					else ListCurrentFiles(context);
					break;

				case "POST":
				case "PUT":
					UploadFile(context);
					break;

				case "DELETE":
					DeleteFile(context);
					break;

				case "OPTIONS":
					ReturnOptions(context);
					break;

				default:
					context.Response.ClearHeaders();
					context.Response.StatusCode = 405;
					break;
			}
		}

		private static void ReturnOptions(HttpContext context) {
			context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
			context.Response.StatusCode = 200;
		}

		// Delete file from the server
		private void DeleteFile (HttpContext context) {

            Int32 PhotoID = WebHelper.GetIntParam(context.Request, "ID", 0);
            DNNGo_ThemePlugin_Multimedia PhotoItem = DNNGo_ThemePlugin_Multimedia.FindByKeyForEdit(PhotoID);
            if (PhotoItem != null && PhotoItem.ID > 0)
            {

                //Ҫɾ��ʵ�ʵ��ļ�
                String DeletePath = HttpContext.Current.Server.MapPath(GetPhotoPath(PhotoItem.FilePath));

                //ɾ��������
                //DNNGo_ThemePlugin_Articles_Files.DeleteByMultimedia(PhotoItem.ID);

                //ɾ���ļ�
                if (File.Exists(DeletePath))
                {
                    try
                    {
                        File.Delete(DeletePath);
                    }
                    catch { }
                }
                //ɾ����¼
                PhotoItem.Delete();
            }
		}

        /// <summary>
        /// ��ȡͼƬ��·��
        /// </summary>
        /// <param name="media">ý���ļ���ʵ��</param>
        /// <returns></returns>
        public String GetPhotoPath(DNNGo_ThemePlugin_Multimedia media)
        {
            String PhotoPath  = String.Empty;
            if (media != null && media.ID > 0)
            {
               PhotoPath = GetPhotoPath(media.FilePath);
            }
            return PhotoPath;

        }


        /// <summary>
        /// ��ȡͼƬ��·��
        /// </summary>
        /// <param name="FilePath">ͼƬ·��</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath)
        {
            return String.Format("{0}{1}", PortalSettings.HomeDirectory, FilePath);
        }

		// Upload file to the server
		private void UploadFile (HttpContext context) {
            var statuses = new List<Resource_FilesStatus>();
			var headers = context.Request.Headers;

            String a= WebHelper.GetStringParam(context.Request,"type","");

            if (!String.IsNullOrEmpty(a) && a == "DELETE")
            {
                DeleteFile(context);
            }
            else
            {

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(context, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], context, statuses);
                }

                WriteJsonIframeSafe(context, statuses);
            }
		}

		// Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<Resource_FilesStatus> statuses)
        {
			if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = context.Request.Files[0];

            if (file != null && !String.IsNullOrEmpty(file.FileName) && file.ContentLength > 0)
            {

                //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                bool Valid = FileSystemUtils.CheckValidFileName(file.FileName);
                if (Valid)
                {
                    DNNGo_ThemePlugin_Multimedia PhotoItem = new DNNGo_ThemePlugin_Multimedia();

                    PhotoItem.ModuleId = WebHelper.GetIntParam(context.Request, "ModuleId", 0);
                    PhotoItem.PortalId = WebHelper.GetIntParam(context.Request, "PortalId", 0);


                    PhotoItem.FileName = file.FileName;
                    PhotoItem.FileSize = file.ContentLength / 1024;
                    PhotoItem.FileMate = FileSystemUtils.GetContentType(Path.GetExtension(PhotoItem.FileName).Replace(".", ""));

                    PhotoItem.FileExtension = System.IO.Path.GetExtension(PhotoItem.FileName).Replace(".", "");
                    PhotoItem.Name = System.IO.Path.GetFileName(file.FileName).Replace(Path.GetExtension(PhotoItem.FileName), "");

                    PhotoItem.Status = (Int32)EnumFileStatus.Approved;

                    try
                    {
                        if (("png,gif,jpg,jpeg,bmp").IndexOf(PhotoItem.FileExtension) >= 0)
                        {
                            //ͼƬ����
                            Image image = Image.FromStream(file.InputStream);
                            PhotoItem.ImageWidth = image.Width;
                            PhotoItem.ImageHeight = image.Height;

                            PhotoItem.Exif = Common.Serialize<EXIFMetaData.Metadata>(new EXIFMetaData().GetEXIFMetaData(image));
                        }
                    }
                    catch
                    {

                    }

                    PhotoItem.LastTime = xUserTime.UtcTime();
                    PhotoItem.LastIP = WebHelper.UserHost;
                    PhotoItem.LastUser = UserInfo.UserID;

                    //���ļ��洢��·�������
                    fileName = System.IO.Path.GetFileName(file.FileName).Replace("." + PhotoItem.FileExtension, ""); //�ļ�����
                    String WebPath = String.Format("ThemePlugin/uploads/{0}/{1}/{2}/", PhotoItem.LastTime.Year, PhotoItem.LastTime.Month, PhotoItem.LastTime.Day);
                    //����ļ��洢·���Ƿ�����ص��ļ�
                    FileInfo fInfo = new FileInfo(HttpContext.Current.Server.MapPath(String.Format("{0}{1}{2}.{3}", PortalSettings.HomeDirectory, WebPath, fileName, PhotoItem.FileExtension)));

                    //����ļ����Ƿ����
                    if (!System.IO.Directory.Exists(fInfo.Directory.FullName))
                    {
                        System.IO.Directory.CreateDirectory(fInfo.Directory.FullName);
                    }
                    else
                    {
                        Int32 j = 1;
                        while (fInfo.Exists)
                        {
                            //�ļ��Ѿ�������
                            fileName = String.Format("{0}_{1}", PhotoItem.Name, j);
                            fInfo = new FileInfo(HttpContext.Current.Server.MapPath(String.Format("{0}{1}{2}.{3}", PortalSettings.HomeDirectory, WebPath, fileName, PhotoItem.FileExtension)));
                            j++;
                        }
                    }

                    PhotoItem.FilePath = String.Format("{0}{1}.{2}", WebPath, fileName, PhotoItem.FileExtension);
                    PhotoItem.FileName = String.Format("{0}.{1}", fileName, PhotoItem.FileExtension);

                    try
                    {

                        if (!fInfo.Directory.Exists)
                        {
                            fInfo.Directory.Create();

                            // FileSystemUtils.AddFolder(PortalSettings, String.Format("{0}DNNGo_PhotoAlbums/{0}/{1}/"), String.Format("{0}DNNGo_PhotoAlbums/{0}/{1}/"), (int)DotNetNuke.Services.FileSystem.FolderController.StorageLocationTypes.InsecureFileSystem);
                        }



                        //����ָ���洢·��
                        file.SaveAs(fInfo.FullName);
                        //FileSystemUtils.AddFile(PhotoItem.FileName, PhotoItem.PortalId, String.Format("DNNGo_PhotoAlbums\\{0}\\{1}\\", PhotoItem.ModuleId, PhotoItem.AlbumID), PortalSettings.HomeDirectoryMapPath, PhotoItem.FileMeta);
                    }
                    catch (Exception ex)
                    {

                    }

                    //���ϴ�����Ƭ���ó�ʼ��˳��
                    QueryParam qp = new QueryParam();
                    qp.ReturnFields = qp.Orderfld = DNNGo_ThemePlugin_Multimedia._.Sort;
                    qp.OrderType = 1;
                    qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, PhotoItem.PortalId, SearchType.Equal));
                    PhotoItem.Sort = Convert.ToInt32(DNNGo_ThemePlugin_Multimedia.FindScalar(qp)) + 2;
                    Int32 PhotoId = PhotoItem.Insert();


                    statuses.Add(new Resource_FilesStatus(PhotoItem, PortalSettings, ModulePath));
                }
            }
		}

		// Upload entire file
        private void UploadWholeFile(HttpContext context, List<Resource_FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];

                if (file != null && !String.IsNullOrEmpty(file.FileName) && file.ContentLength > 0)
                {
                    //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                    bool Valid = FileSystemUtils.CheckValidFileName(file.FileName);
                    if (Valid)
                    {
                        DNNGo_ThemePlugin_Multimedia PhotoItem = new DNNGo_ThemePlugin_Multimedia();

                        PhotoItem.ModuleId = WebHelper.GetIntParam(context.Request, "ModuleId", 0);
                        PhotoItem.PortalId = WebHelper.GetIntParam(context.Request, "PortalId", 0);


                        PhotoItem.FileName = file.FileName;
                        PhotoItem.FileSize = file.ContentLength / 1024;
                        PhotoItem.FileMate = FileSystemUtils.GetContentType(Path.GetExtension(PhotoItem.FileName).Replace(".", ""));

                        PhotoItem.FileExtension = System.IO.Path.GetExtension(PhotoItem.FileName).Replace(".", "");
                        PhotoItem.Name = System.IO.Path.GetFileName(file.FileName).Replace(Path.GetExtension(PhotoItem.FileName), "");
                        PhotoItem.Status = (Int32)EnumFileStatus.Approved;

                        try
                        {
                            if (("png,gif,jpg,jpeg,bmp").IndexOf(PhotoItem.FileExtension) >= 0)
                            {
                                //ͼƬ����
                                Image image = Image.FromStream(file.InputStream);
                                PhotoItem.ImageWidth = image.Width;
                                PhotoItem.ImageHeight = image.Height;

                                PhotoItem.Exif = Common.Serialize<EXIFMetaData.Metadata>(new EXIFMetaData().GetEXIFMetaData(image));
                            }
                        }
                        catch
                        {

                        }

                        PhotoItem.LastTime = xUserTime.UtcTime();
                        PhotoItem.LastIP = WebHelper.UserHost;
                        PhotoItem.LastUser = UserInfo.UserID;


                        //���ļ��洢��·�������
                        String fileName = System.IO.Path.GetFileName(file.FileName).Replace("." + PhotoItem.FileExtension, ""); //�ļ�����
                        String WebPath = String.Format("ThemePlugin/uploads/{0}/{1}/{2}/", PhotoItem.LastTime.Year, PhotoItem.LastTime.Month, PhotoItem.LastTime.Day);
                        //����ļ��洢·���Ƿ�����ص��ļ�
                        FileInfo fInfo = new FileInfo(HttpContext.Current.Server.MapPath(String.Format("{0}{1}{2}.{3}", PortalSettings.HomeDirectory, WebPath, fileName, PhotoItem.FileExtension)));

                        //����ļ����Ƿ����
                        if (!System.IO.Directory.Exists(fInfo.Directory.FullName))
                        {
                            System.IO.Directory.CreateDirectory(fInfo.Directory.FullName);
                        }
                        else
                        {
                            Int32 j = 1;
                            while (fInfo.Exists)
                            {
                                //�ļ��Ѿ�������
                                fileName = String.Format("{0}_{1}", PhotoItem.Name, j);
                                fInfo = new FileInfo(HttpContext.Current.Server.MapPath(String.Format("{0}{1}{2}.{3}", PortalSettings.HomeDirectory, WebPath, fileName, PhotoItem.FileExtension)));
                                j++;
                            }
                        }

                        PhotoItem.FilePath = String.Format("{0}{1}.{2}", WebPath, fileName, PhotoItem.FileExtension);
                        PhotoItem.FileName = String.Format("{0}.{1}", fileName, PhotoItem.FileExtension);
                        try
                        {

                            if (!fInfo.Directory.Exists)
                            {
                                fInfo.Directory.Create();

                                // FileSystemUtils.AddFolder(PortalSettings, String.Format("{0}DNNGo_PhotoAlbums/{0}/{1}/"), String.Format("{0}DNNGo_PhotoAlbums/{0}/{1}/"), (int)DotNetNuke.Services.FileSystem.FolderController.StorageLocationTypes.InsecureFileSystem);
                            }

                            //����ָ���洢·��
                            file.SaveAs(fInfo.FullName);
                            //FileSystemUtils.AddFile(PhotoItem.FileName, PhotoItem.PortalId, String.Format("DNNGo_PhotoAlbums\\{0}\\{1}\\", PhotoItem.ModuleId, PhotoItem.AlbumID), PortalSettings.HomeDirectoryMapPath, PhotoItem.FileMeta);
                        }
                        catch (Exception ex)
                        {

                        }

                        //���ϴ�����Ƭ���ó�ʼ��˳��
                        QueryParam qp = new QueryParam();
                        qp.ReturnFields = qp.Orderfld = DNNGo_ThemePlugin_Multimedia._.Sort;
                        qp.OrderType = 1;
                        qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, PhotoItem.PortalId, SearchType.Equal));
                        PhotoItem.Sort = Convert.ToInt32(DNNGo_ThemePlugin_Multimedia.FindScalar(qp)) + 2;
                        Int32 PhotoId = PhotoItem.Insert();



                        statuses.Add(new Resource_FilesStatus(PhotoItem, PortalSettings, ModulePath));
                    }

                }
            }
		}

        private void WriteJsonIframeSafe(HttpContext context, List<Resource_FilesStatus> statuses)
        {
			context.Response.AddHeader("Vary", "Accept");
			try {
				if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
					context.Response.ContentType = "application/json";
				else
					context.Response.ContentType = "text/plain";
			} catch {
				context.Response.ContentType = "text/plain";
			}

			var jsonObj = js.Serialize(statuses.ToArray());
			context.Response.Write(jsonObj);
		}

		private static bool GivenFilename (HttpContext context) {
            return !string.IsNullOrEmpty(context.Request["PhotoID"]);
		}

		private void DeliverFile (HttpContext context) {

            DNNGo_ThemePlugin_Multimedia DataItem = DNNGo_ThemePlugin_Multimedia.FindByKeyForEdit(WebHelper.GetStringParam(context.Request, "PhotoID", "0"));
            if (DataItem != null && DataItem.ID > 0)
            {
                String Picture = GetPhotoPath(DataItem);

                if (!String.IsNullOrEmpty(Picture) && File.Exists(context.Server.MapPath(Picture)))
               {
                   context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + DataItem.FileName + "\"");
                   context.Response.ContentType = "application/octet-stream";
                   context.Response.ClearContent();
                   context.Response.WriteFile(context.Server.MapPath(Picture));
               }
               else
                   context.Response.StatusCode = 404;
            }else
                context.Response.StatusCode = 404;



            //var filename = context.Request["PhotoID"];
            //var filePath = StorageRoot + filename;

            //if (File.Exists(filePath)) {
            //    context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            //    context.Response.ContentType = "application/octet-stream";
            //    context.Response.ClearContent();
            //    context.Response.WriteFile(filePath);
            //} else
            //    context.Response.StatusCode = 404;
		}

		private void ListCurrentFiles (HttpContext context) {
 
            //QueryParam qp = new QueryParam();
            //qp.PageSize = 0;

            //int RecordCount = 0;
            //qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.AlbumID, WebHelper.GetStringParam(context.Request, "AlbumID", "0"), SearchType.Equal));

            //var files = DNNGo_ThemePlugin_Multimedia.FindAll(qp, out RecordCount).Select(f => new Resource_FilesStatus(f, PortalSettings)).ToArray();
            var files = new List<Resource_FilesStatus>();
 
            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
		}
	}
}