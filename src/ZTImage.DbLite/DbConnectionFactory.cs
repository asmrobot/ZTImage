using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ZTImage.DbLite.DbProvider;

namespace ZTImage.DbLite
{
    public class DbConnectionFactory
    {
        private Dictionary<string, DbConnectionGenerate> generates = new Dictionary<string, DbConnectionGenerate>();
        private DbConnectionGenerate defaultGenerate;



        public IDbConnection CreateConnection(string db=null)
        {
            if (string.IsNullOrEmpty(db))
            {
                if (defaultGenerate == null)
                {
                    throw new DbLiteException("默认数据库连接不存在");
                }
                return this.defaultGenerate.CreateConnection();
            }

            if (generates.ContainsKey(db))
            {
                return this.generates[db].CreateConnection();
            }

            throw new DbLiteException("不存在的数据库连接");
        }

        /// <summary>
        /// 添加连接生成器
        /// </summary>
        /// <param name="generate"></param>
        internal void InsertGenerate(DbConnectionGenerate generate)
        {
            if (!this.generates.ContainsKey(generate.Option.DbID))
            {
                this.generates.Add(generate.Option.DbID, generate);
                if (defaultGenerate == null || generate.Option.Default)
                {
                    defaultGenerate = generate;
                }
            }
        }

    }
}
