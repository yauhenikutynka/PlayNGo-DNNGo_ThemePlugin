using System;
using System.IO;
using DotNetNuke.Entities.Portals;

namespace DNNGo.Modules.ThemePlugin {
	public class Resource_FilesStatus {
		public const string HandlerPath = "/";

		public string group { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public int size { get; set; }
		public string progress { get; set; }
		public string url { get; set; }
		public string thumbnail_url { get; set; }
		public string delete_url { get; set; }
		public string delete_type { get; set; }
		public string error { get; set; }

		public Resource_FilesStatus () {
            delete_type = "DELETE";
        }

        public Resource_FilesStatus(DNNGo_ThemePlugin_Multimedia PhotoItem, PortalSettings ps, String ModulePath) { SetValues(PhotoItem, PhotoItem.FileSize, ps, ModulePath); }

        public Resource_FilesStatus(DNNGo_ThemePlugin_Multimedia PhotoItem, int fileLength, PortalSettings ps, String ModulePath) { SetValues(PhotoItem, fileLength, ps, ModulePath); }

        private void SetValues(DNNGo_ThemePlugin_Multimedia PhotoItem, int fileLength, PortalSettings ps, String ModulePath)
        {
			name = WebHelper.leftx( PhotoItem.Name,35,"...");
			type = "image/png";
            size = PhotoItem.FileSize * 1024;
			progress = "1.0";
            url = ViewPicture(PhotoItem, ps, ModulePath);
            thumbnail_url = GetPhotoExtension(PhotoItem.FileExtension, PhotoItem.FilePath, ps,ModulePath);
            delete_url = String.Format("{2}Resource_jQueryFileUpload.ashx?ID={0}&type=DELETE&{1}", PhotoItem.ID, WebHelper.GetScriptNameQueryString, ModulePath);
            delete_type = "POST";
		}


        public String ViewPicture(DNNGo_ThemePlugin_Multimedia DataItem, PortalSettings ps, String ModulePath)
        {
            String Picture = String.Format("{0}Resource/images/DefaultPhoto.png", ModulePath);
            if (DataItem != null && DataItem.ID > 0)
            {
                Picture = GetPhotoPath(DataItem, ps);
            }
            return Picture;

        }


        public String ThumbnailUrl(DNNGo_ThemePlugin_Multimedia DataItem, PortalSettings ps, String ModulePath)
        {
            return String.Format("{0}Resource_Service.aspx?Token=thumbnail&PortalId={1}&TabId={2}&ID={3}&width=40&height=40&mode=WH",ModulePath,  ps.PortalId,ps.ActiveTab.TabID, DataItem.ID);
        }

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="media">媒体文件的实体</param>
        /// <returns></returns>
        public String GetPhotoPath(DNNGo_ThemePlugin_Multimedia media, PortalSettings ps)
        {
            String PhotoPath = String.Empty;
            if (media != null && media.ID > 0)
            {
                PhotoPath = GetPhotoPath(media.FilePath,ps);
            }
            return PhotoPath;

        }


        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath,PortalSettings ps)
        {
            return String.Format("{0}{1}", ps.HomeDirectory, FilePath);
        }

        private String GetPhotoExtension(String FileExtension, String FilePath, PortalSettings ps, String ModulePath)
        {
            //先判断是否是图片格式的
            if (FileExtension == "jpg")
                return GetPhotoPath(FilePath, ps);
            else if (FileExtension == "jpeg")
                return GetPhotoPath(FilePath, ps);
            else if (FileExtension == "png")
                return GetPhotoPath(FilePath, ps);
            else if (FileExtension == "gif")
                return GetPhotoPath(FilePath, ps);
            else if (FileExtension == "bmp")
                return GetPhotoPath(FilePath, ps);
            else if (FileExtension == "mp3")
                return GetFileIcon("audio.png", ModulePath);
            else if (FileExtension == "wma")
                return GetFileIcon("audio.png", ModulePath);
            else if (FileExtension == "zip")
                return GetFileIcon("archive.png", ModulePath);
            else if (FileExtension == "rar")
                return GetFileIcon("archive.png", ModulePath);
            else if (FileExtension == "7z")
                return GetFileIcon("archive.png", ModulePath);
            else if (FileExtension == "xls")
                return GetFileIcon("spreadsheet.png", ModulePath);
            else if (FileExtension == "txt")
                return GetFileIcon("text.png", ModulePath);
            else if (FileExtension == "cs")
                return GetFileIcon("code.png", ModulePath);
            else if (FileExtension == "html")
                return GetFileIcon("code.png", ModulePath);
            else if (FileExtension == "doc")
                return GetFileIcon("document.png", ModulePath);
            else if (FileExtension == "docx")
                return GetFileIcon("document.png", ModulePath);
            else
                return GetFileIcon("default.png", ModulePath);
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="IconName">图标文件</param>
        /// <returns></returns>
        public String GetFileIcon(String IconName,String ModulePath)
        {
            return String.Format("{1}Resource/images/crystal/{0}", IconName, ModulePath);
        }
	}
}