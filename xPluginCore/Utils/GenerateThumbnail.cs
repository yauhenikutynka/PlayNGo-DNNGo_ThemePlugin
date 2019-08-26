using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DNNGo.Modules.ThemePlugin
{
 
    /// <summary> 
    /// **生成高质量缩略图程序** 
    /// </summary> 
    public class GenerateThumbnail
    {


        /// <summary>
        /// 推送缩略图到页面流
        /// </summary>
        /// <param name="pathImageFrom"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void PushThumbnail(string pathImageFrom, int width, int height, string mode)
        {

            MemoryStream s = MakeThumbnail(pathImageFrom, width, height, mode);
            try
            {
                if (s != null && s.Length > 0)
                {

                    //byte[] bytes = new byte[s.Length];
                    //s.Write(bytes, 0, bytes.Length);

                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ContentType = FileSystemUtils.GetContentType(Path.GetExtension(pathImageFrom).Replace(".", ""));
                    HttpContext.Current.Response.BinaryWrite(s.ToArray());
                }
            }catch
            {

            }
            finally
            {
                if (s != null && s.Length > 0)
                {
                    s.Close();
                    s.Dispose();
                }

                HttpContext.Current.Response.End();
            }
           
        }




        #region 方法 -- 生成缩略图
        /**/
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static MemoryStream MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
             FileInfo fi = new FileInfo(originalImagePath);
             if (fi.Exists)
             {
                 System.IO.MemoryStream ms = new System.IO.MemoryStream();
             

                 System.Drawing.Image originalImage = System.Drawing.Image.FromStream(fi.OpenRead());

                 int towidth = width;
                 int toheight = height;

                 int x = 0;
                 int y = 0;
                 int ow = originalImage.Width;
                 int oh = originalImage.Height;

                 if (ow < towidth && oh < toheight)
                 {
                     originalImage.Save(ms, ConvertImageFormat(originalImagePath));
                 }
                 else
                 {

                     switch (mode.ToUpper())
                     {
                         case "HW"://指定高宽缩放（可能变形）           
                             break;
                         case "W"://指定宽，高按比例             
                             toheight = originalImage.Height * width / originalImage.Width;
                             break;
                         case "H"://指定高，宽按比例
                             towidth = originalImage.Width * height / originalImage.Height;
                             break;
                         case "CUT"://指定高宽裁减（不变形）           
                             if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                             {
                                 oh = originalImage.Height;
                                 ow = originalImage.Height * towidth / toheight;
                                 y = 0;
                                 x = (originalImage.Width - ow) / 2;
                             }
                             else
                             {
                                 ow = originalImage.Width;
                                 oh = originalImage.Width * height / towidth;
                                 x = 0;
                                 y = (originalImage.Height - oh) / 2;
                             }
                             break;
                         case "AUTO": //自动适应高度
                             if (ow > oh)
                             {
                                 //newwidth = 200;
                                 toheight = (int)((double)oh / (double)ow * (double)towidth);
                             }
                             else
                             {
                                 //newheight = 200;
                                 towidth = (int)((double)ow / (double)oh * (double)toheight);
                             }
                             break;
                         default:
                             break;
                     }

                     //进行缩图
                     Bitmap img = new Bitmap(towidth, toheight);
                     img.SetResolution(72f, 72f);
                     //img.MakeTransparent();
                     
                     Graphics gdiobj = Graphics.FromImage(img);
                  
                     gdiobj.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                     gdiobj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                     gdiobj.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                     gdiobj.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                     gdiobj.FillRectangle(new SolidBrush(Color.White), 0, 0,
                                     towidth, toheight);
 
                     Rectangle destrect = new Rectangle(0, 0,
                                     towidth, toheight);
                     gdiobj.DrawImage(originalImage, destrect, 0, 0, ow,
                                     oh, GraphicsUnit.Pixel);
                     System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                     ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                     System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                     try
                     {
                         if (ici != null)
                         {
                             img.Save(ms, ici, ep);
                         }
                         else
                         {
                             img.Save(ms, ConvertImageFormat(originalImagePath));
                         }
                     }
                     catch (System.Exception e)
                     {
                         throw e;
                     }
                     finally
                     {
                         gdiobj.Dispose();
                         img.Dispose();
                     }
                 }
                 originalImage.Dispose();
                 return ms;
             }
             return null;
        }


        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        #endregion



 
        /// <summary> 
        /// 生成缩略图 静态方法 
        /// </summary> 
        /// <param name="pathImageFrom"> 源图的路径(含文件名及扩展名) </param> 
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param> 
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param> 
        public static Stream GenThumbnail(string pathImageFrom, int width, int height)
        {
            FileInfo fi = new FileInfo(pathImageFrom);
            if (fi.Exists)
            {
 
                Stream ImageStream = fi.OpenRead() as Stream;

                Image imageFrom = null;

                try
                {
                    imageFrom = Image.FromStream(ImageStream);
                }
                catch
                {
                    //throw; 
                }
                if (imageFrom == null)
                {
                    return ImageStream;
                }
                // 源图宽度及高度 
                int imageFromWidth = imageFrom.Width;
                int imageFromHeight = imageFrom.Height;
                // 生成的缩略图实际宽度及高度 
                int bitmapWidth = width;
                int bitmapHeight = height;
                // 生成的缩略图在上述"画布"上的位置 
                int X = 0;
                int Y = 0;
                // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
                if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
                {
                    bitmapHeight = imageFromHeight * width / imageFromWidth;
                    Y = (height - bitmapHeight) / 2;
                }
                else
                {
                    bitmapWidth = imageFromWidth * height / imageFromHeight;
                    X = (width - bitmapWidth) / 2;
                }
                // 创建画布 
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                // 用白色清空 
                g.Clear(Color.White);
                // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 指定高质量、低速度呈现。 
                g.SmoothingMode = SmoothingMode.HighQuality;
                // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
                try
                {
                    //经测试 .jpg 格式缩略图大小与质量等最优 
                    //bmp.Save(pathImageTo, ImageFormat.Jpeg);
                    bmp.Save(ImageStream, ConvertImageFormat(pathImageFrom));
                    
                }
                catch
                {
                }
                finally
                {
                    //显示释放资源 
                    imageFrom.Dispose();
                    bmp.Dispose();
                    g.Dispose();
                }
                return ImageStream;
            }
            return null;

        }

        /// <summary>
        /// 转换数据格式(文件类型)
        /// </summary>
        /// <param name="pathImageFrom"></param>
        /// <returns></returns>
        public static ImageFormat ConvertImageFormat(String pathImageFrom)
        {
           String Extension = Path.GetExtension(pathImageFrom).Replace(".", "").ToUpper();
           ImageFormat imagef = ImageFormat.Jpeg;

           switch (Extension)
           {
               case "JPG": imagef = ImageFormat.Jpeg; break;
               case "JPEG": imagef = ImageFormat.Jpeg; break;
               case "PNG": imagef = ImageFormat.Png; break;
               case "GIF": imagef = ImageFormat.Gif; break;
               case "BMP": imagef = ImageFormat.Bmp; break;
               case "EMF": imagef = ImageFormat.Emf; break;
               case "EXIF": imagef = ImageFormat.Exif; break;
               case "ICON": imagef = ImageFormat.Icon; break;
               case "TIFF": imagef = ImageFormat.Tiff; break;
               default: imagef = ImageFormat.Jpeg; break;
 
           }
           return imagef;


        }

    }
}