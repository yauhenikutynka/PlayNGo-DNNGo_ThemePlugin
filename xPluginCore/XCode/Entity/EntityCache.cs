using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 实体缓存
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class EntityCache<TEntity> where TEntity : Entity<TEntity>, new()
    {
        private IList<TEntity> _Entities;
        /// <summary>实体集合</summary>
        public List<TEntity> Entities
        {
            get
            {
                if (DateTime.Now > CacheTime.AddSeconds(Expriod))
                {
                    lock (this)
                    {
                        if (DateTime.Now > CacheTime.AddSeconds(Expriod))
                        {

                            CacheTime = DateTime.Now;
                            if (Asynchronous)
                                ThreadPool.QueueUserWorkItem(new WaitCallback(FillWaper));
                            else
                                _Entities = FillListMethod();
                        }
                    }
                }
                if (_Entities == null || _Entities.Count < 1) return null;
                return _Entities as List<TEntity>;
            }
            //set { _Entities = value; }
        }

        private void FillWaper(Object state)
        {
            _Entities = FillListMethod();
            CacheTime = DateTime.Now;
        }

        private DateTime _CacheTime = DateTime.Now.AddDays(-100);
        /// <summary>缓存时间</summary>
        public DateTime CacheTime
        {
            get { return _CacheTime; }
            set { _CacheTime = value; }
        }

        private Int32 _Expriod = 60;
        /// <summary>过期时间。单位是秒，默认60秒</summary>
        public Int32 Expriod
        {
            get { return _Expriod; }
            set { _Expriod = value; }
        }

        private FillListDelegate<TEntity> _FillListMethod;
        /// <summary>填充数据的方法</summary>
        public FillListDelegate<TEntity> FillListMethod
        {
            get
            {
                if (_FillListMethod == null) _FillListMethod = Entity<TEntity>.FindAll;
                return _FillListMethod;
            }
            set { _FillListMethod = value; }
        }

        private Boolean _Asynchronous;
        /// <summary>异步更新</summary>
        public Boolean Asynchronous
        {
            get { return _Asynchronous; }
            set { _Asynchronous = value; }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void Clear()
        {
            CacheTime = DateTime.Now.AddDays(-100);
        }
    }

    /// <summary>
    /// 填充数据的方法
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns></returns>
    public delegate IList<TEntity> FillListDelegate<TEntity>() where TEntity : Entity<TEntity>, new();
}