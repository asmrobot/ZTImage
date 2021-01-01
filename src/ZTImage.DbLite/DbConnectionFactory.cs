using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ZTImage.DbLite.DbProvider;

namespace ZTImage.DbLite
{
    public class DbConnectionFactory
    {
        private Dictionary<string, DbConnectionGenerate> _generates = new Dictionary<string, DbConnectionGenerate>();
        private DbConnectionGenerate _defaultGenerate;


        public IDbConnection CreateConnection(string db=null)
        {
            if (string.IsNullOrEmpty(db))
            {
                if (_defaultGenerate == null)
                {
                    throw new DbLiteException("默认数据库连接不存在");
                }
                return this._defaultGenerate.CreateConnection();
            }

            if (_generates.ContainsKey(db))
            {
                return this._generates[db].CreateConnection();
            }

            throw new DbLiteException("不存在的数据库连接");
        }

        /// <summary>
        /// 添加连接生成器
        /// </summary>
        /// <param name="generate"></param>
        internal void InsertGenerate(DbConnectionGenerate generate)
        {
            if (!this._generates.ContainsKey(generate.Option.DbID))
            {
                this._generates.Add(generate.Option.DbID, generate);
                if (_defaultGenerate == null || generate.Option.Default)
                {
                    _defaultGenerate = generate;
                }
            }
        }

    }
}
