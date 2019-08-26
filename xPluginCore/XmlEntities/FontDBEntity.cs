using DNNGo.Modules.ThemePlugin.Xml4DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{ 
    /// <summary>
    /// 字体数据实体
    /// </summary>
    public class FontDB 
    {

        private String _PrimaryGuid;
        /// <summary>
        /// 主键
        /// </summary>
        public String PrimaryGuid
        {
            get { return _PrimaryGuid; }
           set { _PrimaryGuid = value; }
        }


        private String _Alias;
        /// <summary>
        /// 别名
        /// </summary>
        public String Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }







        private String _Family;
        /// <summary>
        /// 字体名
        /// </summary>
        public String Family
        {
            get { return _Family; }
            set { _Family = value; }
        }



        private String _Bold;
        /// <summary>
        /// 粗细
        /// </summary>
        public String Bold
        {
            get { return _Bold; }
            set { _Bold = value; }
        }



        private String _Subset;
        /// <summary>
        /// 字体语言
        /// </summary>
        public String Subset
        {
            get { return _Subset; }
            set { _Subset = value; }
        }





        private String _FontUrl;
        /// <summary>
        /// 字体链接,链接到外部字体库
        /// </summary>
        public String FontUrl
        {
            get { return _FontUrl; }
            set { _FontUrl = value; }
        }


        private String _Font_File_Eot;
        /// <summary>
        /// (.woff)内部字体文件链接
        /// </summary>
        public String Font_File_Eot
        {
            get { return _Font_File_Eot; }
            set { _Font_File_Eot = value; }
        }


        private String _Font_File_Svg;
        /// <summary>
        /// (.woff)内部字体文件链接
        /// </summary>
        public String Font_File_Svg
        {
            get { return _Font_File_Svg; }
            set { _Font_File_Svg = value; }
        }


        private String _Font_File_Ttf;
        /// <summary>
        /// 内部字体文件链接
        /// </summary>
        public String Font_File_Ttf
        {
            get { return _Font_File_Ttf; }
            set { _Font_File_Ttf = value; }
        }


        private String _Font_File_Woff;
        /// <summary>
        /// (.woff)内部字体文件链接
        /// </summary>
        public String Font_File_Woff
        {
            get { return _Font_File_Woff; }
            set { _Font_File_Woff = value; }
        }


        /// <summary>
        /// 字体链接,如谷歌等字体库引用
        /// </summary>
        public Boolean? IsFontLink
        {
            get;
            set;
        }

        /// <summary>
        /// 是否系统内置字体
        /// </summary>
        public Boolean? IsSystem
        {
            get;
            set;
        }


        private Boolean _Enable;


        /// <summary>
        /// 启用
        /// </summary>
        public Boolean Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }





        private DateTime _CreateTime;
        /// <summary>
        /// 字体创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }


        private DateTime _UpdateTime;
        /// <summary>
        /// 字体更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }


    }




    /// <summary>
    /// 设置操作类
    /// </summary>
    public class FontDBHelper
    {



        public FontDBHelper(BaseModule baseModule)
        {
            String XMLPath = baseModule.MapPath(String.Format("{0}xTemplate/FontDB.xml", baseModule. SkinPath)); 
            mDB = XmlDBFactory.CreatXmlDB("FontDB", XMLPath, 1);
        }

        


        public FontDBHelper(BasePage basePage)
        {
            String XMLPath = basePage.MapPath(String.Format("{0}xTemplate/FontDB.xml", basePage.SkinPath));
            mDB = XmlDBFactory.CreatXmlDB("FontDB", XMLPath, 1);
        }





        public FontDBHelper(BaseSkin baseSkin)
        {
            String XMLPath = baseSkin.MapPath(String.Format("{0}xTemplate/FontDB.xml", baseSkin.SkinPath));
            mDB = XmlDBFactory.CreatXmlDB("FontDB", XMLPath, 1);
        }


        XmlDB mDB;

        /// <summary>
        /// 查找单个设置
        /// </summary>
        /// <param name="PrimaryGuid"></param>
        /// <returns></returns>
        public FontDB Find(String PrimaryGuid)
        {
            FontDB item = new FontDB() { Enable = true };
            if (!String.IsNullOrEmpty(PrimaryGuid))
            {
                item = mDB.Read(new FontDB() { PrimaryGuid = PrimaryGuid }).FirstOrDefault<FontDB>();
                if (!(item != null && !String.IsNullOrEmpty(item.PrimaryGuid)))
                {
                    item = new FontDB() { Enable=true };
                }
            }
            return item;
        }

        /// <summary>
        /// 读取全部设置
        /// </summary>
        /// <returns></returns>
        public List<FontDB> FindAll()
        {
            return mDB.Read(new FontDB());
        }

        /// <summary>
        /// 按条件读取设置
        /// </summary>
        /// <returns></returns>
        public List<FontDB> FindAll(FontDB FontSearch, Int32 PageIndex,Int32 PageSize,out Int32 RecordCount)
        {
            List<FontDB> DBs = mDB.Read(FontSearch);
            RecordCount = DBs.Count;
            return Common.Split<FontDB>(DBs, PageIndex, PageSize);
        }
        /// <summary>
        /// 保存单个设置
        /// </summary>
        /// <param name="i"></param>
        public void Save(FontDB i)
        {

            if ( !String.IsNullOrEmpty(i.PrimaryGuid))
            {
                i.UpdateTime = DateTime.Now;
         

                mDB.Update(new FontDB() { PrimaryGuid = i.PrimaryGuid }, i);
            }
            else
            {
                i.PrimaryGuid = Guid.NewGuid().ToString("N");
                i.UpdateTime = i.CreateTime = DateTime.Now;
                mDB.Insert(i);
            }
        }
        /// <summary>
        /// 保存并提交
        /// </summary>
        /// <param name="i"></param>
        public void SaveCommit(FontDB i)
        {
            Save(i);
            Commit();
        }

        /// <summary>
        /// 保存多个设置并提交
        /// </summary>
        /// <param name="Settings"></param>
        public void Save(List<FontDB> Settings)
        {
            foreach (FontDB s in Settings)
            {
                Save(s);
            }
            mDB.Commit();
        }
        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            mDB.Commit();
        }





        /// <summary>
        /// 删除一个配置
        /// </summary>
        /// <param name="PrimaryGuid"></param>
        public void Delete(String PrimaryGuid)
        {
            mDB.Delete(new FontDB() { PrimaryGuid = PrimaryGuid });
            mDB.Commit();
        }

 


    }


}