using System;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;

#if NET45
using System.Data.SQLite;
#else
using Microsoft.Data.Sqlite;
#endif

namespace ZTImage.Database.DBProvider
{
    public class SqliteProvider : IDbProvider
    {
        public DbProviderFactory Instance()
        {
#if NET45
            return SQLiteFactory.Instance;
#else
            return SqliteFactory.Instance;
#endif

        }

        public string GetLastIdSql()
        {
            return "select last_insert_rowid()";
        }

        public DbParameter MakeParam(string parameterName, DbType parameterType, Int32 size)
        {
#if NET45
            SQLiteParameter param;
            
            if (size > 0)
            {
                param = new SQLiteParameter(parameterName, parameterType, size);
            }
            else
            {
                param = new SQLiteParameter(parameterName, parameterType);
            }
            return param;
#else
            SqliteParameter param;

            if (size > 0)
            {
                
                param = new SqliteParameter(parameterName, Map(parameterType), size);
            }
            else
            {
                
                param = new SqliteParameter(parameterName, parameterType);
            }
            return param;
#endif
        }

#if NETSTANDARD
        private SqliteType Map(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return SqliteType.Text;
                case DbType.AnsiStringFixedLength:
                    return SqliteType.Text;
                case DbType.Binary:
                    return SqliteType.Blob;
                case DbType.Boolean:
                    return SqliteType.Integer;
                case DbType.Byte:
                    return SqliteType.Integer;
                case DbType.Currency:
                    return SqliteType.Real;
                case DbType.Date:
                    return SqliteType.Integer;
                case DbType.DateTime:
                    return SqliteType.Integer;
                case DbType.DateTime2:
                    return SqliteType.Integer;
                case DbType.DateTimeOffset:
                    return SqliteType.Integer;
                case DbType.Decimal:
                    return SqliteType.Real;
                case DbType.Double:
                    return SqliteType.Real;
                case DbType.Guid:
                    return SqliteType.Text;
                case DbType.Int16:
                    return SqliteType.Integer;
                case DbType.Int32:
                    return SqliteType.Integer;
                case DbType.Int64:
                    return SqliteType.Integer;
                case DbType.Object:
                    return SqliteType.Blob;
                case DbType.SByte:
                    return SqliteType.Integer;
                case DbType.Single:
                    return SqliteType.Real;
                case DbType.String:
                    return SqliteType.Text;
                case DbType.StringFixedLength:
                    return SqliteType.Text;
                case DbType.Time:
                    return SqliteType.Integer;
                case DbType.UInt16:
                    return SqliteType.Integer;
                case DbType.UInt32:
                    return SqliteType.Integer;
                case DbType.UInt64:
                    return SqliteType.Integer;
                case DbType.VarNumeric:
                    return SqliteType.Integer;
                case DbType.Xml:
                    return SqliteType.Text;
            }

            return SqliteType.Blob;
        }
#endif

        
        /// <summary>
        /// 得到数据库表
        /// </summary>
        /// <returns></returns>
        public string GetTableSql()
        {
            return "show tables";
        }

        /// <summary>
        /// 得到数据表架构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetMetaSchemaSql(string tableName)
        {
            return string.Format ("select * from `{0}` limit 1",tableName );
        }
    }
}
