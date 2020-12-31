using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;

namespace ZTImage.DbLite.DbProvider
{
    public class SqlServerProvider : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return SqlClientFactory.Instance;
        }

        public string GetLastIdSql()
        {
            return "SELECT SCOPE_IDENTITY()";
        }

        public DbParameter MakeParam(string parameterName, DbType parameterType, Int32 size)
        {
            SqlParameter param;
            if (size > 0)
            {
                param = new SqlParameter(parameterName, MapDbType(parameterType), size);
            }
            else
            {
                param = new SqlParameter(parameterName, MapDbType(parameterType));
            }
            return param;
        }

        /// <summary>
        /// 对应DbType至MySqlDbType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private static SqlDbType MapDbType(DbType _dbtype)
        {
            switch (_dbtype)
            {
                case DbType.AnsiString: return SqlDbType.VarChar ;
                case DbType.AnsiStringFixedLength: return SqlDbType.Char;
                case DbType.Binary: return SqlDbType.Binary;
                case DbType.Boolean: return SqlDbType.Bit;
                case DbType.Byte: return SqlDbType.TinyInt;
                case DbType.Currency: return SqlDbType.Money;
                case DbType.Date: return SqlDbType.Date;
                case DbType.DateTime: return SqlDbType.DateTime;
                case DbType.DateTime2: return SqlDbType.DateTime;
                case DbType.DateTimeOffset: return SqlDbType.Timestamp;
                case DbType.Decimal: return SqlDbType.Decimal;
                case DbType.Double: return SqlDbType.Float ;
                case DbType.Guid: return SqlDbType.VarChar;
                case DbType.Int16: return SqlDbType.SmallInt;
                case DbType.Int32: return SqlDbType.Int;
                case DbType.Int64: return SqlDbType.BigInt;
                case DbType.Object: return SqlDbType.Binary;
                case DbType.SByte: return SqlDbType.TinyInt;
                case DbType.Single: return SqlDbType.TinyInt;
                case DbType.String: return SqlDbType.VarChar;
                case DbType.StringFixedLength: return SqlDbType.VarChar;
                case DbType.Time: return SqlDbType.Time;
                case DbType.UInt16: return SqlDbType.SmallInt ;
                case DbType.UInt32: return SqlDbType.Int;
                case DbType.UInt64: return SqlDbType.BigInt;
                case DbType.VarNumeric: return SqlDbType.Decimal;
                case DbType.Xml: return SqlDbType.VarChar;
            }
            return SqlDbType.VarChar;
        }



        /// <summary>
        /// 得到数据表架构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetMetaSchemaSql(string tableName)
        {
            return string.Format("select top 1 * from `{0}`", tableName);
        }

        /// <summary>
        /// 得到数据库表
        /// </summary>
        /// <returns></returns>
        public string GetTableSql()
        {
            return "select name from sysobjects where type='U'";
        }
    }
}
