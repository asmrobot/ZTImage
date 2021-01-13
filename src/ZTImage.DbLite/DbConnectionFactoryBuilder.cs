using System;
using System.Collections.Generic;
using System.Text;

namespace ZTImage.DbLite
{

    /// <summary>
    /// DbConnectionFactory Builder
    /// for dapper map:Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    /// </summary>
    public class DbConnectionFactoryBuilder
    {
        public DbConnectionFactoryBuilder()
        {
            this.options = new List<DbConnectionOptions>();
        }
        private List<DbConnectionOptions> options;

        /// <summary>
        /// 添加连接配置
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DbConnectionFactoryBuilder AddDbConnectionOption(DbConnectionOptions option)
        {
            if (option == null)
            {
                throw new DbLiteException("options is null");
            }

            if (this.options.Contains(option))
            {
                return this;
            }
            this.options.Add(option);
            return this;
        }

        /// <summary>
        /// 添加连接配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DbConnectionFactoryBuilder AddDbConnectionOptions(IEnumerable<DbConnectionOptions> options)
        {
            foreach (var item in options)
            {
                if (this.options.Contains(item))
                {
                    continue;
                }
                this.options.Add(item);
            }
            return this;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public DbConnectionFactory Build()
        {
            DbConnectionFactory factory = new DbConnectionFactory();
            
            foreach (var item in options)
            {
                DbConnectionGenerate generate = new DbConnectionGenerate(item);
                factory.InsertGenerate(generate);
            }
            return factory;
        }

    }
}
