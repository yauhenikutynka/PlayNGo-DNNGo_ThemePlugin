using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using System.Web;
using DotNetNuke.Services.Search;
using DotNetNuke.Common.Utilities;
using System.Collections;

namespace DNNGo.Modules.ThemePlugin
{
    public class BaseController : ISearchable, IPortable
    {
        #region "Optional Interfaces"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {

            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
 
            return SearchItemCollection;

        }





        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public string ExportModule(int ModuleID)
        {
            string strXML = String.Empty;

          
            return strXML;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The ID of the Module being imported</param>
        /// <param name="Content">The Content being imported</param>
        /// <param name="Version">The Version of the Module Content being imported</param>
        /// <param name="UserID">The UserID of the User importing the Content</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {
           

           
        }


        #endregion


    }
}