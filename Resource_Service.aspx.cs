
using System;
using System.Web;
using System.Collections.Generic;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 资源文件(主要用于一些请求的服务)
    /// 1.文件上传
    /// </summary>
    public partial class Resource_Service : BasePage
    {

        #region "属性"
        /// <summary>
        /// 功能
        /// 文件上传 FileUpload
        /// </summary>
        private String Token = WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "").ToLower();



        #endregion


        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            base.Page_Init(sender, e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!String.IsNullOrEmpty(Token))
                    {
                        String XmlPath = MapPath(string.Format("{0}Resource/xml/Service.xml", ModulePath));
                        XmlFormat xf = new XmlFormat(XmlPath);
                        List<ServiceDB> ServiceDBs = xf.ToList<ServiceDB>();
                        if (ServiceDBs != null && ServiceDBs.Count > 0 && ServiceDBs.Exists(r => r.Token.ToLower() == Token.ToLower()))
                        {
                            ServiceDB serDB = ServiceDBs.Find(r => r.Token.ToLower() == Token.ToLower());

                            if (serDB != null && !String.IsNullOrEmpty(serDB.Token))
                            {
                                //取出需要调用的服务
                                iService Ser = (iService)Activator.CreateInstance(serDB.assemblyName, serDB.typeName).Unwrap();
                                if (Ser != null && !String.IsNullOrEmpty(serDB.Name))
                                {
                                    //执行服务
                                    Ser.Execute(this);

                                    if (Ser.IsResponseWrite)
                                    {
                                        if (!String.IsNullOrEmpty(Ser.ResponseString))
                                        {
                                            //输出字符串
                                            Response.Clear();
                                            Response.Write(Ser.ResponseString);
                                        }
                                        else
                                        {
                                            //错误,没有输出

                                        }
                                    }
                                    else
                                    {
                                        //这里会用其他方式输出
                                    }

                                }
                                else
                                {
                                    //没有找到相应的服务
                                }
                            }
                            else
                            {
                                //没有找到相应的服务
                            }
                        }
                        else
                        {
                            //没有找到相应的服务
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(String.Format("Exception:{0}", ex.Source));
            }
            finally
            {
                Response.End();
            }








        }
 


      

  

    

 

    }
}