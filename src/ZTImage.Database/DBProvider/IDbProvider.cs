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
