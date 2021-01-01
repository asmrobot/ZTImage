using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ZTImage.DbLite.DbProvider;

namespace ZTImage.DbLite
{
    /// <summary>
    /// 数据库连接生成器
    /// </summary>
    internal class DbConnectionGenerate
    {

        internal DbConnectionOptions Option;
        public DbConnectionGenerate(DbConnectionOptions option)
        {
            this.Option = option;
        }





        #region 私有变量

        /// <summary>
        /// DbProviderFactory实例
        /// </summary>
        private DbProviderFactory m_factory = null;
        /// <summary>
        /// AsmInfoCode数据提供实例
        /// </summary>
        private IDbProvider m_provider = null;
        /// <summary>
        /// 辅助锁定
        /// </summary>
        private static object lockHelper = new object();
        #endregion

        #region 属性

        /// <summary>
        /// 数据提供
        /// </summary>
        public IDbProvider Provider
        {
            get
            {
                if (m_provider == null)
                {
                    lock (lockHelper)
                    {
                        if (m_provider == null)
                        {
                            try
                            {
                                m_provider = (IDbProvider)Activator.CreateInstance(Type.GetType(string.Format("ZTImage.DbLite.DbProvider.{0}Provider,ZTImage.DbLite", this.Option.DbType), false, true));
                            }
                            catch
                            {
                                throw new DbLiteException("请确认配置字段配置是否正确!");
                            }
                        }
                    }
                }
                return m_provider;
            }
        }

        /// <summary>
        /// 数据工厂
        /// </summary>
        public DbProviderFactory Factory
        {
            get
            {
                if (m_factory == null)
                {
                    m_factory = Provider.Instance();
                }
                return m_factory;
            }
        }

        /// <summary>
        /// 重置数据类型
        /// </summary>
        public void RsetProvider()
        {
            m_provider = null;
            m_factory = null;

        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            DbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = this.Option.ConnectionString;
            return connection;
        }
        #endregion
    }
}
