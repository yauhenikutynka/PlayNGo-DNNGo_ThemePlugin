using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 文件关系表
	/// </summary>
	public partial class DNNGo_ThemePlugin_Relationships : Entity<DNNGo_ThemePlugin_Relationships>
	{
		#region 对象操作
		//基类Entity中包含三个对象操作：Insert、Update、Delete
		//你可以重载它们，以改变它们的行为
		//如：
		/*
		/// <summary>
		/// 已重载。把该对象插入到数据库。这里可以做数据插入前的检查
		/// </summary>
		/// <returns>影响的行数</returns>
		public override Int32 Insert()
		{
			return base.Insert();
		}
		 * */
		#endregion
		
		#region 扩展属性
		//TODO: 本类与哪些类有关联，可以在这里放置一个属性，使用延迟加载的方式获取关联对象

		/*
		private Category _Category;
		/// <summary>该商品所对应的类别</summary>
		public Category Category
		{
			get
			{
				if (_Category == null && CategoryID > 0 && !Dirtys.ContainKey("Category"))
				{
					_Category = Category.FindByKey(CategoryID);
					Dirtys.Add("Category", true);
				}
				return _Category;
			}
			set { _Category = value; }
		}
		 * */
		#endregion

		#region 扩展查询
		/// <summary>
		/// 根据主键查询一个文件关系表实体对象用于表单编辑
		/// </summary>
		///<param name="__ID">编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static DNNGo_ThemePlugin_Relationships FindByKeyForEdit(Int32 __ID)
		{
			DNNGo_ThemePlugin_Relationships entity=Find(new String[]{_.ID}, new Object[]{__ID});
			if (entity == null)
			{
				entity = new DNNGo_ThemePlugin_Relationships();
			}
			return entity;
		}     

		/// <summary>
		/// 根据编号查找
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DNNGo_ThemePlugin_Relationships FindByID(Int32 id)
		{
			return Find(_.ID, id);
			// 实体缓存
			//return Meta.Cache.Entities.Find(_.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
		}



        /// <summary>
        /// 根据文章编号查找对象的分类关系
        /// </summary>
        /// <param name="ArticleID">文章编号</param>
        /// <returns></returns>
        public static List<DNNGo_ThemePlugin_Relationships> FindAllByID(Int32 TabID, Int32 PortalId, Int32 FileType)
        {
            Int32 RecordCount = 0;
            QueryParam qp = new QueryParam();
            qp.OrderType = 0;
            qp.Orderfld = _.Sort;
            qp.Where.Add(new SearchParam(_.TabID, TabID, SearchType.Equal));
            qp.Where.Add(new SearchParam(_.PortalId, PortalId, SearchType.Equal));

            if (FileType >= 0)
            {
                qp.Where.Add(new SearchParam(_.Type, FileType, SearchType.Equal));
            }

            return FindAll(qp, out RecordCount);
        }


        /// <summary>
        /// 根据文章编号查找对象的分类关系
        /// </summary>
        /// <param name="ArticleID">文章编号</param>
        /// <returns></returns>
        public static List<DNNGo_ThemePlugin_Relationships> FindAllViewByID(Int32 TabID, Int32 PortalId, Int32 FileType, Boolean isGlobal)
        {
            Int32 RecordCount = 0;
            QueryParam qp = new QueryParam();
            qp.OrderType = 0;
            qp.Orderfld = _.Sort;
            qp.Where.Add(new SearchParam(_.PortalId, PortalId, SearchType.Equal));

            qp.WhereSql.Append(" ( ");
            //公开的
            qp.WhereSql.Append(new SearchParam(_.TabID, TabID, SearchType.Equal).ToSql());

            if (isGlobal)//是否显示全局
            {
                qp.WhereSql.Append(" OR ");
                qp.WhereSql.Append(new SearchParam(_.TabID, int.MaxValue, SearchType.Equal).ToSql());
            }
            qp.WhereSql.Append(" ) ");


            if (FileType >= 0)
            {
                qp.Where.Add(new SearchParam(_.Type, FileType, SearchType.Equal));
            }

            return FindAll(qp, out RecordCount);
        }

		#endregion

		#region 高级查询
		/// <summary>
		/// 查询满足条件的记录集，分页、排序
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>实体集</returns>
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static List<DNNGo_ThemePlugin_Relationships> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
		}

		/// <summary>
		/// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>记录数</returns>
		public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindCount(SearchWhere(key), null, null, 0, 0);
		}

		/// <summary>
		/// 构造搜索条件
		/// </summary>
		/// <param name="key">关键字</param>
		/// <returns></returns>
		private static String SearchWhere(String key)
		{
            if (String.IsNullOrEmpty(key)) return null;
            key = key.Replace("'", "''");
            String[] keys = key.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		    StringBuilder sb = new StringBuilder();
		    sb.Append("1=1");

            //if (!String.IsNullOrEmpty(name)) sb.AppendFormat(" And {0} like '%{1}%'", _.Name, name.Replace("'", "''"));

            for (int i = 0; i < keys.Length; i++)
            {
                sb.Append(" And ");

                if (keys.Length > 1) sb.Append("(");
                Int32 n = 0;
                foreach (FieldItem item in Meta.Fields)
                {
                    if (item.Property.PropertyType != typeof(String)) continue;
                    // 只要前五项
                    if (++n > 5) break;

                    if (n > 1) sb.Append(" Or ");
                    sb.AppendFormat("{0} like '%{1}%'", item.Name, keys[i]);
                }
                if (keys.Length > 1) sb.Append(")");
            }

            if (sb.Length == "1=1".Length)
                return null;
            else
                return sb.ToString();
		}
		#endregion

		#region 扩展操作


        public static Int32 Update(Int32 TabID,Int32 PortalId, String PictureIDs, Int32 FileTye)
        {
            Int32 Result = 0;

            //查出当前文章关联的所有分类数据
            List<DNNGo_ThemePlugin_Relationships> CategoryList = FindAllByID(TabID,PortalId, FileTye);

            if (!String.IsNullOrEmpty(PictureIDs))
            {
                //分类编号
                List<String> PictureIDList = WebHelper.GetList(PictureIDs);

                //制造临时变量
                String[] tempPictureIDList = PictureIDList.ToArray();
                PictureIDList.CopyTo(tempPictureIDList);
                DNNGo_ThemePlugin_Relationships[] tempCategoryList = CategoryList.ToArray(); CategoryList.CopyTo(tempCategoryList);

                foreach (String PictureID in tempPictureIDList)
                {
                    //对比当前分类
                    foreach (DNNGo_ThemePlugin_Relationships Picture in tempCategoryList)
                    {
                        if (Picture.FileID.ToString() == PictureID)
                        {
                            //移除两者
                            CategoryList.Remove(Picture);
                            PictureIDList.Remove(PictureID);
                        }
                    }
                }

                //剩下的列表删除
                foreach (String PictureID in PictureIDList)
                {
                    DNNGo_ThemePlugin_Relationships Category = new DNNGo_ThemePlugin_Relationships();
                    Category.FileID = Convert.ToInt32(PictureID);
                    Category.TabID = TabID;
                    Category.PortalId = PortalId;
                    Category.Type = FileTye;

                    QueryParam Sqp = new QueryParam();
                    Sqp.ReturnFields = Sqp.Orderfld = DNNGo_ThemePlugin_Relationships._.Sort;
                    Sqp.OrderType = 1;
                    Sqp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Relationships._.TabID, TabID, SearchType.Equal));
                    Sqp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Relationships._.PortalId, PortalId, SearchType.Equal));
                    Sqp.Where.Add(new SearchParam(DNNGo_ThemePlugin_Relationships._.Type, FileTye, SearchType.Equal));
                    Category.Sort = Convert.ToInt32(DNNGo_ThemePlugin_Relationships.FindScalar(Sqp)) + 2;
                    Category.Insert();
                    Result += 1;
                }

                //剩下的ID添加
                foreach (DNNGo_ThemePlugin_Relationships Category in CategoryList)
                {
                    Category.Delete();
                    Result += 1;
                }

            }
            else
            {
                //全部删除
                foreach (DNNGo_ThemePlugin_Relationships Category in CategoryList)
                {
                    Category.Delete();

                }
            }

            return Result;
        }


		#endregion

		#region 业务
		#endregion
	}
}