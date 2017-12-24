using System;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;

namespace ZTImage.Database.DBProvider
{
    public class MySqlProvider : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return MySqlClientFactory.Instance;
        }

        public string GetLastIdSql()
        {
            return "SELECT LAST_INSERT_ID()";
        }

        public DbParameter MakeParam(string parameterName, DbType parameterType, Int32 size)
        {
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = parameterName;
            param.DbType = parameterType;
            
            if (size > 0)
            {
                param.Size = size;
                //param = new MySqlParameter(parameterName, MapDbType(parameterType), size);
            }
            //else
            //{
            //    param = new MySqlParameter(parameterName, MapDbType(parameterType));
            //}
            return param;
        }


        /// <summary>
        /// 分页得到数据
        /// </summary>
        /// <param name="pagecount">页大小,默认为10</param>
        /// <param name="pageindex">页索引,默认从1开始</param>
        /// <param name="maintable">主表,不可以为空</param>
        /// <param name="mainkey">索引字段,不可为空</param>
        /// <param name="allrecord">要取的字段，默认为*，如果是多表查询请加上表名如:[Table].record</param>
        /// <param name="where">条件,默认为空，不用加where关键字</param>
        /// <param name="innerhashtable">inner join字段,默认为空,存储在hashTable里，键存储表名，值存储联接条件</param>
        /// <param name="asc">排序方式，默认为ASC方式排序，asc为True,Desc为false</param>
        /// <returns></returns>
        public string[] GetPageingSql(int pagecount, int pageindex, string maintable, string mainkey, string allrecord, string where, Hashtable innerhashtable, bool asc,string orderfield)
        {
            return null;
        }


        ///// <summary>
        ///// 对应DbType至MySqlDbType
        ///// </summary>
        ///// <param name="dbType"></param>
        ///// <returns></returns>
        //private static MySqlDbType MapDbType(DbType _dbtype)
        //{
            
        //    switch (_dbtype)
        //    {
        //        case DbType.AnsiString: return MySqlDbType.String;
        //        case DbType.AnsiStringFixedLength: return MySqlDbType.VarString;
        //        case DbType.Binary: return MySqlDbType.Binary;
        //        case DbType.Boolean: return MySqlDbType.Bit;
        //        case DbType.Byte: return MySqlDbType.Byte;
        //        case DbType.Currency: return MySqlDbType.Int32;
        //        case DbType.Date: return MySqlDbType.Date;
        //        case DbType.DateTime: return MySqlDbType.DateTime;
        //        case DbType.DateTime2: return MySqlDbType.DateTime;
        //        case DbType.DateTimeOffset: return MySqlDbType.Timestamp;
        //        case DbType.Decimal: return MySqlDbType.Decimal;
        //        case DbType.Double: return MySqlDbType.Double;
        //        case DbType.Guid: return MySqlDbType.VarChar;
        //        case DbType.Int16: return MySqlDbType.Int16;
        //        case DbType.Int32: return MySqlDbType.Int32 ;
        //        case DbType.Int64: return MySqlDbType.Int64;
        //        case DbType.Object: return MySqlDbType.Blob;
        //        case DbType.SByte: return MySqlDbType.Byte;
        //        case DbType.Single: return MySqlDbType.UByte;
        //        case DbType.String: return MySqlDbType.VarChar;
        //        case DbType.StringFixedLength: return MySqlDbType.Int32;
        //        case DbType.Time: return MySqlDbType.Time;
        //        case DbType.UInt16: return MySqlDbType.UInt16;
        //        case DbType.UInt32: return MySqlDbType.UInt32;
        //        case DbType.UInt64: return MySqlDbType.UInt64;
        //        case DbType.VarNumeric: return MySqlDbType.Int32;
        //        case DbType.Xml: return MySqlDbType.String;
        //    }
        //    return MySqlDbType.String;
        //}


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
