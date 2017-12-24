using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;

namespace ZTImage.Database.DBProvider
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
            #region 初始化所有参数

            if (string.IsNullOrEmpty(maintable))
            {
                throw new Exception("参数错误");
            }
            else
            {
                maintable = maintable.Trim();
            }
            if (string.IsNullOrEmpty(mainkey))
            {
                throw new Exception("参数错误");
            }
            else
            {
                mainkey = mainkey.Trim();
            }

            if (pagecount <= 0)
            {
                pagecount = 10;
            }

            if (pageindex <= 0)
            {
                pageindex = 0;
            }
            else
            {
                pageindex--;
            }

            if (string.IsNullOrEmpty(allrecord))
            {
                allrecord = "*";
            }
            else
            {
                allrecord = allrecord.Trim();
            }


            if (!string.IsNullOrEmpty(where))
            {
                where = where.Trim();
            }

            string innerTable = string.Empty;

            if (innerhashtable != null)
            {
                IDictionaryEnumerator enu = innerhashtable.GetEnumerator();

                while (enu.MoveNext())
                {
                    innerTable += " inner join [" + enu.Key.ToString() + "] on " + enu.Value.ToString() + " ";
                }

                innerTable = innerTable.Trim();
            }


            string bj = string.Empty;
            string px = string.Empty;
            string function = string.Empty;

            if (string.IsNullOrEmpty(orderfield))
            {
                orderfield = "[" + maintable + "]." + mainkey + "";
            }
            if (asc)
            {
                px = " order by "+orderfield  + "  ASC ";
                bj = " > ";
                function = "max";
            }
            else
            {
                px = " order by "+orderfield + " DESC ";
                bj = " < ";
                function = " min ";
            }

            #endregion
            string sql = string.Empty;
            if (pageindex <= 0)
            {
                sql = "select top " + pagecount + " " + allrecord + " from " + maintable + " " + innerTable + (string.IsNullOrEmpty(where) ? "" : " where " + where) + " " + px;

            }
            else
            {
                sql = "select top " + pagecount + " " + allrecord + " from " + maintable + " " + innerTable + " where [" + maintable + "]." + mainkey + " " + bj + " (select " + function + "(" + mainkey + ") from (select top " + (pagecount * pageindex) + " [" + maintable + "]." + mainkey + " from " + maintable + " " + innerTable + (string.IsNullOrEmpty(where) ? "" : " where " + where) + " " + px + ") as T) " + (string.IsNullOrEmpty(where) ? "" : " and " + where) + " " + px;

            }
            string selSql = "select count(" + "[" + maintable + "]." + mainkey + ") from " + maintable + " " + innerTable + " " + (string.IsNullOrEmpty(where) ? "" : " where " + where);
            string[] retsql = { sql, selSql };

            return retsql;
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
