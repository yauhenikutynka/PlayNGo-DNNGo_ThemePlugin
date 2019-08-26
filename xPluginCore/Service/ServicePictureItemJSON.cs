using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 单个相片的数据
    /// </summary>
    public class ServicePictureItemJSON : iService
    {
        public ServicePictureItemJSON()
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
                return "PictureItem JSON";
            }
        }

        public void Execute(BasePage Context)
        {
            Dictionary<String, Object> jsonPicture = new Dictionary<String, Object>();

            Int32 MediaID = WebHelper.GetIntParam(Context.Request, "MediaID", 0);
            if (MediaID > 0)
            {

                DNNGo_ThemePlugin_Multimedia PictureItem = DNNGo_ThemePlugin_Multimedia.FindByKeyForEdit(MediaID);
                if (PictureItem != null && PictureItem.ID > 0)
                {
                    jsonPicture.Add("ID", PictureItem.ID);
                    jsonPicture.Add("CreateTime", PictureItem.LastTime);
                    jsonPicture.Add("Name", PictureItem.Name);
                    jsonPicture.Add("Extension", PictureItem.FileExtension);
                    String ThumbnailUrl = Context.ViewLinkUrl(String.Format("MediaID={0}", PictureItem.ID));
                    jsonPicture.Add("ThumbnailUrl", ThumbnailUrl);
                    jsonPicture.Add("FileUrl", Context.GetPhotoPath(PictureItem.FilePath));


                    jsonPicture.Add("Thumbnail", String.Format("<img style=\"border-width:0px; max-height:60px;max-width:80px;\"  src=\"{0}\"  /> ", ThumbnailUrl));

                    //判断当前文件是否为图片
                    if (!String.IsNullOrEmpty(PictureItem.FileExtension) && ("gif,jpg,jpeg,bmp,png").IndexOf(PictureItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        jsonPicture.Add("IsVideo", false);
                        jsonPicture.Add("IsPicture", true);

                    }
                    else if (!String.IsNullOrEmpty(PictureItem.FileExtension) && ("mp4,mkv,avi,ogv,webm,m4v").IndexOf(PictureItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        jsonPicture.Add("IsVideo", true);
                        jsonPicture.Add("IsPicture", false);


                        switch (PictureItem.FileExtension)
                        {
                            case "mp4": jsonPicture.Add("IsMp4", true); break;
                            case "m4v": jsonPicture.Add("IsM4v", true); break;
                            case "ogv": jsonPicture.Add("IsOgv", true); break;
                            case "webm": jsonPicture.Add("IsWebm", true); break;
                            default: break;
                        }

                    }
                    else if (!String.IsNullOrEmpty(PictureItem.FileExtension) && ("flv,swf").IndexOf(PictureItem.FileExtension, StringComparison.CurrentCultureIgnoreCase) >= 0)
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

                }

            }

            //转换数据为json
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            ResponseString = jsSerializer.Serialize(jsonPicture);
        }
    }
}