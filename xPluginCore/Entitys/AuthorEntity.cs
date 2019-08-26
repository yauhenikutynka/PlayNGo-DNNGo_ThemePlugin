using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 作者实体
    /// </summary>
    public class AuthorEntity
    {

        private Int32 _CreateUser = 0;
        /// <summary>
        /// 作者
        /// </summary>
        public Int32 CreateUser
        {
            get { return _CreateUser; }
            set { _CreateUser = value; }
        }



        private String _CreateUserName = String.Empty;
        /// <summary>
        /// 作者用户名
        /// </summary>
        public String CreateUserName
        {
            get {
                if (String.IsNullOrEmpty(_CreateUserName))
                {
                    UserInfo uInfo = new DotNetNuke.Entities.Users.UserController().GetUser(PortalId, CreateUser);
                    if (uInfo != null && uInfo.UserID > 0)
                    {
                        _CreateUserName = uInfo.DisplayName;
                    }
                }
                return _CreateUserName; }
        }



        private Int32 _Count = 0;
        /// <summary>
        /// 数量
        /// </summary>
        public Int32 Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        private Int32 _ModuleId = 0;
        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId
        {
            get { return _ModuleId; }
            set { _ModuleId = value; }
        }

        private Int32 _PortalId = 0;
        /// <summary>
        /// 站点编号
        /// </summary>
        public Int32 PortalId
        {
            get { return _PortalId; }
            set { _PortalId = value; }
        }


    }
}