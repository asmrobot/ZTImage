using System;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ZTImage.Database.DBProvider
{
    public interface IDbProvider
    {
        /// <summary>
        /// DbProviderFactory的实例
        /// </summary>
        /// <returns></returns>
        DbProviderFactory Instance();

        /// <summary>
        /// 得到最后一个插入记录的ID
        /// </summary>
        /// <returns></returns>
        string GetLastIdSql();

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        DbParameter MakeParam(string parameterName, DbType parameterType, Int32 size);

        
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
        string[] GetPageingSql(int pagecount, int pageindex, string maintable, string mainkey, string allrecord, string where, Hashtable innerhashtable, bool asc,string orderfield);


        /// <summary>
        /// 得到数据库表SQL
        /// </summary>
        /// <returns></returns>
        string GetTableSql();

        /// <summary>
        /// 得到数据表架构SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetMetaSchemaSql(string tableName);

    }
}
