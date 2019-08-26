using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 相片列表的数据
    /// </summary>
    public class ServicePictureListJSON : iService
    {
        public ServicePictureListJSON()
        {
            IsResponseWrite = true;
        }


        /// <summary>
        /// 是否写入输出
        /// </summary>
        public bool IsResponseWrite
        {
            get;
            set;
        }


        private String _ResponseString;
        /// <summary>
        /// 输出字符串
        /// </summary>
        public string ResponseString
        {
            get
            {
                return _ResponseString;
            }
            set
            {
                _ResponseString = value;
            }
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get
            {
                return "PictureList JSON";
            }
        }

        public void Execute(BasePage Context)
        {

            QueryParam qp = new QueryParam();
            qp.Orderfld = DNNGo_ThemePlugin_Multimedia._.ID;

            qp.PageIndex = WebHelper.GetIntParam(Context.Request, "PageIndex", 1);
            qp.PageSize = WebHelper.GetIntParam(Context.Request, "PageSize", Int32.MaxValue);
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.Status, (Int32)EnumFileStatus.Approved, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, Context.PortalId, SearchType.Equal));

            int RecordCount = 0;
            List<DNNGo_ThemePlugin_Multimedia> fileList = DNNGo_ThemePlugin_Multimedia.FindAll(qp, out RecordCount);

            Dictionary<String, Object> jsonPictures = new Dictionary<string, Object>();

            TemplateFormat xf = new TemplateFormat();
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();


            foreach (var fileItem in fileList)
            {
                int index = fileList.IndexOf(fileItem); //index 为索引值

                Dictionary<String, Object> jsonPicture = new Dictionary<String, Object>();

                jsonPicture.Add("Pages", qp.Pages);


                jsonPicture.Add("ID", fileItem.ID);

                jsonPicture.Add("CreateTime", fileItem.LastTime);

                jsonPicture.Add("Name", WebHelper.leftx(fileItem.Name, 20, "..."));
                jsonPicture.Add("Extension", fileItem.FileExtension);


                String ThumbnailUrl = Context.ViewLinkUrl(String.Format("MediaID={0}", fileItem.ID));
                jsonPicture.Add("ThumbnailUrl", ThumbnailUrl);
                jsonPicture.Add("FileUrl", Context.GetPhotoPath(fileItem.FilePath));

                jsonPicture.Add("Thumbnail", String.Format("<img style=\"border-width:0px; max-height:60px;max-width:80px;\"  src=\"{0}\"  /> ", ThumbnailUrl));
                //判断当前文件是否为图片
                if (!String.IsNullOrEmpty(fileItem.FileExtension) && ("gif,jpg,jpeg,bmp,png").IndexOf(fileItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    jsonPicture.Add("IsVideo", false);
                    jsonPicture.Add("IsPicture", true);

                }
                else if (!String.IsNullOrEmpty(fileItem.FileExtension) && ("mp4,mkv,avi,ogv,webm,m4v").IndexOf(fileItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    jsonPicture.Add("IsVideo", true);
                    jsonPicture.Add("IsPicture", false);


                    switch (fileItem.FileExtension)
                    {
                        case "mp4": jsonPicture.Add("IsMp4", true); break;
                        case "m4v": jsonPicture.Add("IsM4v", true); break;
                        case "ogv": jsonPicture.Add("IsOgv", true); break;
                        case "webm": jsonPicture.Add("IsWebm", true); break;
                        default: break;
                    }

                }
                else if (!String.IsNullOrEmpty(fileItem.FileExtension) && ("flv,swf").IndexOf(fileItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    jsonPicture.Add("IsFlash", true);
                    jsonPicture.Add("IsPicture", false);
                    jsonPicture.Add("Guid", Guid.NewGuid().ToString());

                }
                else
                {
                    jsonPicture.Add("IsVideo", false);
                    jsonPicture.Add("IsPicture", false);
                }

                jsonPicture.Add("Json", jsSerializer.Serialize(jsonPicture));

                jsonPictures.Add(index.ToString(), jsonPicture);

            }

            //转换数据为json
     
            ResponseString = jsSerializer.Serialize(jsonPictures);
        }


    }
}