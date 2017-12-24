using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using ZTImage.Database.Schemas;
using ZTImage.Database.DBProvider;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using ZTImage;
using ZTImage.Reflection.Reflector;
using ZTImage.Collections;

namespace ZTImage.Database.HelperBase
{
    /// <summary>
    /// 数据库操作基类
    /// </summary>
    public abstract class SQLDBHelper
    {
        #region 私有变量
        /// <summary>
        /// 查询次数
        /// </summary>
        private int query_count = 0;
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
        private static object lockHelper=new object();
        #endregion

        #region 属性

        /// <summary>
        /// 查询次数
        /// </summary>
        public int QueryCount
        {
            get { return query_count; }
            set { query_count = value; }
        }

        public abstract  string ConnectionString
        {
            get;
        }


        /// <summary>
        /// 数据库类型
        /// </summary>
        public abstract string DbType
        {
            get;

        }




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
                                m_provider = (IDbProvider)Activator.CreateInstance(Type.GetType(string.Format("ZTImage.Database.DBProvider.{0}Provider,ZTImage.Database", DbType), false, true));
                            }
                            catch
                            {
                                throw new DatabaseException("请确认配置字段配置是否正确!");
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
        #endregion

        #region 私有方法

        //预处理命令
        private static void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] parameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText.Length == 0 || commandText == null) throw new ArgumentNullException("commandText");

            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            //设置命令文本
            command.CommandText = commandText;

            //设置命令类型
            command.CommandType = commandType;
            
            //设置命令连接
            command.Connection = connection;
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("transaction connection is no seting");
                command.Transaction = transaction;
            }
            if (parameters != null)
            {
                AttachParameter(command, parameters); 
            }

            return;

        }

        //给命令添加参数
        private static void AttachParameter(DbCommand command,DbParameter [] parameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    if ((p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput) && 
                        (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }
            return;
        }
        #endregion        

        #region 生成参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public DbParameter MakeInParam(string ParamName, DbType DbType, object Value)
        {
            return MakeParam(ParamName, DbType, 0, ParameterDirection.Input, Value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public DbParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Direction"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            DbParameter param;

            param = Provider.MakeParam(ParamName, DbType, Size);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        

        #endregion 生成参数结束

        #region 工具方法
        /// <summary>
        /// 判断库中表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool TableExists(string tableName)
        {
            try
            {
                string sql = string.Format("select 1 from {0}", tableName);
                object ret = ExecuteScalar(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到本连接库中所有的表
        /// </summary>
        /// <returns></returns>
        public string[] GetTables()
        {
            List<string> tables = new List<string>();
            DbDataReader ddr = ExecuteReader(Provider.GetTableSql());
            while (ddr.Read())
            {
                tables.Add(ddr.GetString(0));
            }

            ddr.Close();

            return tables.ToArray();
        }

        /// <summary>
        /// 得到表的字段名及类型字典
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ColumnMetaCollection GetTableSchema(string table)
        {
            ColumnMetaCollection metas = new ColumnMetaCollection();
            DbDataReader ddr = ExecuteReader(Provider.GetMetaSchemaSql(table));
            if (ddr != null && !ddr.IsClosed)
            {
                //得元数据
                DataTable dt = ddr.GetSchemaTable();
                for (int i = 0, len = dt.Rows.Count; i < len; i++)
                {
                    ColumnMeta columnMeta = new ColumnMeta();
                    
                    
                    if (Provider is MySqlProvider)
                    {
                        columnMeta.ColumnName = dt.Rows[i][0].ToString();
                        columnMeta.ColumnOridinal = TypeConverter.ObjectToInt(dt.Rows[i][1],0);
                        columnMeta.ColumnSize = TypeConverter.ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = TypeConverter.ObjectToBool(dt.Rows[i][5],false);
                        columnMeta.IsKey = TypeConverter.ObjectToBool(dt.Rows[i][6],false);
                        

                        columnMeta.FieldTypeName = ((Type)dt.Rows[i][11]).Name;
                        columnMeta.FieldType = (Type)dt.Rows[i][11];
                        columnMeta.AllowDBNull = TypeConverter.ObjectToBool(dt.Rows[i][12],true);

                        
                    }
                    else
                    {
                        columnMeta.ColumnName = dt.Rows[i][0].ToString();
                        columnMeta.ColumnOridinal = TypeConverter.ObjectToInt(dt.Rows[i][1], 0);
                        columnMeta.ColumnSize = TypeConverter.ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = TypeConverter.ObjectToBool(dt.Rows[i][5], false);
                        columnMeta.IsKey = TypeConverter.ObjectToBool(dt.Rows[i][6], false);


                        columnMeta.FieldTypeName = ((Type)dt.Rows[i][11]).Name;
                        columnMeta.FieldType = (Type)dt.Rows[i][11];
                        columnMeta.AllowDBNull = TypeConverter.ObjectToBool(dt.Rows[i][12], true);
                    }


                    if (dt.Rows[i][0].ToString().IndexOf(" ") > -1)
                    {
                        continue;
                    }

                    metas.Add(columnMeta);
                }


                ddr.Close();
            }
            return metas;
        }



        /// <summary>
        /// 得到表的字段名及类型字典
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetTableSchemaDataTable(string table)
        {
            Dictionary<string, string> meta = new Dictionary<string, string>();
            DbDataReader ddr = ExecuteReader(Provider.GetMetaSchemaSql(table));
            DataTable dt=null;
            if (ddr != null && !ddr.IsClosed)
            {
                //得元数据
                dt = ddr.GetSchemaTable();
                ddr.Close();
            }
            return dt;
        }

        #endregion


        #region 连接
        public DbConnection CreateConnection()
        { 
            DbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        public DbTransaction CreateTransaction()
        {
            DbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            return connection.BeginTransaction();
            
        }
        #endregion


        #region Async ExcuteNonQuery方法

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectiong String");
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                //connection.Open();
                return await ExecuteNonQueryAsync(commandText, commandType, parameters, connection);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;

            DbCommand cmd = Factory.CreateCommand();
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, parameters, out mustCloseConnection);

            try
            {
                int result = await cmd.ExecuteNonQueryAsync();
                query_count++;
                cmd.Parameters.Clear();
                if (mustCloseConnection)
                {
                    connection.Close();
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region ExecuteNonQuery方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, (DbParameter[])null); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out int id, string commandText)
        {
            return ExecuteNonQuery(out id, commandText,CommandType.Text, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery( string commandText,CommandType commandType)
        {
            return ExecuteNonQuery(commandText,commandType,(DbParameter[])null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out int id, string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(out id, commandText, commandType,(DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectiong String");
            using (DbConnection connection = Factory.CreateConnection())
            {                
                connection.ConnectionString = ConnectionString;
                //connection.Open();
                return ExecuteNonQuery( commandText, commandType, parameters,connection);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out int id, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectiong String");
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                //connection.Open();
                return ExecuteNonQuery(out id,commandText, commandType, parameters, connection);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out object id, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectiong String");
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                //connection.Open();
                return ExecuteNonQuery(out id, commandText, commandType, parameters, connection);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType,DbConnection connection)
        {
            return ExecuteNonQuery(commandText,commandType,(DbParameter [])null,connection);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;

            DbCommand cmd = Factory.CreateCommand();
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, parameters, out mustCloseConnection);

            int result = cmd.ExecuteNonQuery();
            query_count++;
            cmd.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }


            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out int id,string commandText, CommandType commandType, DbParameter[] parameters, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            commandText += ";" + Provider.GetLastIdSql();
            DbCommand cmd = Factory.CreateCommand();
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, parameters, out mustCloseConnection);

            id = TypeConverter.ObjectToInt(cmd.ExecuteScalar());
            

            cmd.Parameters.Clear();
            query_count++;

            if (mustCloseConnection)
            {
                connection.Close();
            }

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(out object id, string commandText, CommandType commandType, DbParameter[] parameters, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;

            DbCommand cmd = Factory.CreateCommand();
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, parameters, out mustCloseConnection);

            int result = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            query_count++;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIdSql();
            id = cmd.ExecuteScalar();

            query_count++;
            if (mustCloseConnection)
            {
                connection.Close();
            }

            return result;
        }

        /// <summary>
        /// 执行带事务的DbCommand.
        /// </summary>
        /// <remarks>
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回影响的行数/returns>
        public int ExecuteNonQuery(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(transaction, commandText, commandType, (DbParameter[])null);
        }


        /// <summary>
        /// 执行带事务的DbCommand.
        /// </summary>
        /// <remarks>
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回影响的行数/returns>
        public int ExecuteNonQuery(out int id, DbTransaction transaction, string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(out id, transaction, commandText, commandType, (DbParameter[])null);
        }


        /// <summary>
        /// 执行带事务的DbCommand(指定参数).
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">SqlParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理
            DbCommand cmd = Factory.CreateCommand();
            
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            
            // 执行DbCommand命令,并返回结果.
            int retval = cmd.ExecuteNonQuery();
            
            // 清除参数集,以便再次使用.
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// 执行带事务的DbCommand(指定参数).
        /// </summary>
        /// <remarks>
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的数据库连接对象</param>
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">SqlParamter参数数组</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(out int id, DbTransaction transaction, string commandText, CommandType commandType, params DbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            
            // 预处理
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行
            int retval = cmd.ExecuteNonQuery();
            // 清除参数,以便再次使用.
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIdSql();
            
            id = TypeConverter.ObjectToInt(cmd.ExecuteScalar().ToString());
            return retval;
        }



        #endregion

        #region ExcuteDataTable方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText,CommandType.Text,(DbParameter [])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText,commandType,(DbParameter [])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameter"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText,CommandType commandType,DbParameter [] commandParameter)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (DbConnection con = Factory.CreateConnection())
            {
                con.ConnectionString = ConnectionString;
                //con.Open();
                return ExecuteDataTable(commandText, commandType, commandParameter, con);
            }
        }


        public DataTable ExecuteDataTable(out int allCount, string commandText, CommandType commandType, DbParameter[] commandParameter)
        { 
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (DbConnection con = Factory.CreateConnection())
            {
                con.ConnectionString = ConnectionString;
                //con.Open();
                return ExecuteDataTable(out allCount, commandText, commandType, commandParameter, con);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, DbConnection commandCon)
        {
            return ExecuteDataTable(commandText,commandType,(DbParameter[])null,commandCon);
        }




        public DataTable ExecuteDataTable(out int allCount,string commandText, CommandType commandType, DbParameter[] commandParameter, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("commandCon");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            DbCommand command = Factory.CreateCommand();
            bool mustCloseConn = false;
            PrepareCommand(command, commandCon, (DbTransaction)null, commandType, commandText, commandParameter, out mustCloseConn);

            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                da.Fill(dt);
                query_count++;

                allCount = (Int32)da.SelectCommand.Parameters["@RowCount"].Value;
                command.Parameters.Clear();

                if (mustCloseConn)
                {
                    commandCon.Close();
                }

                return dt;
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameter"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, DbParameter[] commandParameter, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("commandCon");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            DbCommand command = Factory.CreateCommand();
            bool mustCloseConn = false;
            PrepareCommand(command, commandCon, (DbTransaction)null, commandType, commandText, commandParameter, out mustCloseConn);

            using (DbDataAdapter da = Factory.CreateDataAdapter())
            {
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                da.Fill(dt);
                query_count++;
                command.Parameters.Clear();

                if (mustCloseConn)
                {
                    commandCon.Close();
                }

                return dt;
            }


        }

        #endregion

        #region ExecuteReader方法



        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <param name="commandConnection"></param>
        /// <param name="ownership"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText,CommandType commandType,DbParameter[] commandPara,DbConnection commandConnection )
        {
            if (commandConnection == null) throw new ArgumentNullException("ConnectionString is Null");

            bool mustCloseConection = false;
            
            DbCommand cmd = Factory.CreateCommand();
            try
            {
                PrepareCommand(cmd, commandConnection, (DbTransaction)null, commandType, commandText, commandPara, out mustCloseConection);

                DbDataReader dataread;
                dataread = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                query_count++;

                bool canClear = true;
                foreach (DbParameter p in cmd.Parameters)
                {
                    if (p.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataread;
            }
            catch
            {
                if (mustCloseConection)
                {
                    commandConnection.Close();
                }
                
                throw;
            }  

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, (DbParameter[])null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString is Null");

            DbConnection con = Factory.CreateConnection();
            con.ConnectionString = ConnectionString;
            con.Open();
            return ExecuteReader(commandText, commandType, commandPara, con );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("commandCon");

            return ExecuteReader(commandText, commandType, (DbParameter[])null, commandCon );
        }


        #endregion ExecuteReader方法结束

        #region Query实体
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<TResult> Query<TResult>(string commandText)
        {
            return Query<TResult>(commandText, CommandType.Text);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<KubiuObject> Query(string commandText)
        {
            return Query<KubiuObject>(commandText, CommandType.Text);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public List<KubiuObject> Query(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            return Query<KubiuObject>(commandText, commandType, commandPara);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public List<TResult> Query<TResult>(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = ExecuteReader(commandText, commandType, commandPara);
            List<TResult> list = new List<TResult>();
            while (!ddr.IsClosed && ddr.Read())
            {
                list.Add(fillDbReader<TResult>(ddr,0,-1));
            }
            if (!ddr.IsClosed)
            {
                ddr.Close();
            }
            return list;
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public MultiList<TResult1,TResult2> Query<TResult1,TResult2>(string commandText,string sepChar, CommandType commandType, params DbParameter[] commandPara)
        {
            
            DbDataReader ddr = ExecuteReader(commandText, commandType, commandPara);
            MultiList<TResult1, TResult2> list = new MultiList<TResult1, TResult2>();
            int sepIndex = 0;

            bool isinit = false;
            while (!ddr.IsClosed && ddr.Read())
            {
                if (!isinit)
                {
                    try
                    {
                        sepIndex = ddr.GetOrdinal(sepChar);
                        if (sepIndex <= 0 || sepIndex > ddr.FieldCount)
                        {
                            throw new IndexOutOfRangeException("分隔符不存在或一开始就是分隔符");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new DatabaseException("SQL语句不包含分隔符,sql:" + commandText + ",分隔符:" + sepChar);
                    }
                    isinit = true;
                }

                TResult1 entity1 = fillDbReader<TResult1>(ddr,0,sepIndex);
                TResult2 entity2 = fillDbReader<TResult2>(ddr,sepIndex,-1);
                
                list.Add(entity1,entity2);
            }
            if (!ddr.IsClosed)
            {
                ddr.Close();
            }
            return list;
        }



        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public TResult QueryOne<TResult>(string commandText)
        {
            return QueryOne<TResult>(commandText, CommandType.Text);
        }

        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public KubiuObject QueryOne(string commandText)
        {
            return QueryOne<KubiuObject>(commandText, CommandType.Text);
        }

        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public KubiuObject QueryOne(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            return QueryOne<KubiuObject>(commandText, commandType, commandPara);
        }


        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public T QueryOne<T>(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = ExecuteReader(commandText, commandType, commandPara);
            T entity = default(T);
            if (!ddr.IsClosed && ddr.Read())
            {
                entity = fillDbReader<T>(ddr,0,-1);
            }
            if (!ddr.IsClosed)
            {
                ddr.Close();
            }
            return entity;
        }



        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public MultiObject<TResult1, TResult2> QueryOne<TResult1, TResult2>(string commandText, string sepChar, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = ExecuteReader(commandText, commandType, commandPara);
            MultiObject<TResult1, TResult2> entity = new MultiObject<TResult1, TResult2>(default(TResult1), default(TResult2));
            if (!ddr.IsClosed && ddr.Read())
            {
                int sepIndex = 0;

                try
                {
                    sepIndex = ddr.GetOrdinal(sepChar);
                    if (sepIndex <= 0 || sepIndex > ddr.FieldCount)
                    {
                        throw new IndexOutOfRangeException("分隔符不存在或一开始就是分隔符");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new DatabaseException("SQL语句不包含分隔符,sql:" + commandText + ",分隔符:" + sepChar);
                }

                TResult1 entity1 = fillDbReader<TResult1>(ddr, 0, sepIndex);
                TResult2 entity2 = fillDbReader<TResult2>(ddr, sepIndex, -1);

                
                entity = new MultiObject<TResult1, TResult2>(entity1, entity2);   
            }

            if (!ddr.IsClosed)
            {
                ddr.Close();
            }
            return entity;
        }

        /// <summary>
        /// 填充dbreader list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public List<T> FillReaderList<T>(DbDataReader ddr)
        {
            List<T> list = new List<T>();
            while (!ddr.IsClosed && ddr.Read())
            {
                list.Add(fillDbReader<T>(ddr, 0, -1));
            }
            return list;
        }

        /// <summary>
        /// 填充reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public T FillReader<T>(DbDataReader ddr)
        {
            return fillDbReader<T>(ddr, 0, -1);
        }

        /// <summary>
        /// 填充reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public T FillReader<T>(DbDataReader ddr, int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException("start");
            }
            return fillDbReader<T>(ddr, start, end);
        }
        /// <summary>
        /// 填充dbreader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        private T fillDbReader<T>(DbDataReader ddr,int start,int end)
        {
            if (ddr.IsClosed)
            {
                return default(T);
            }
            if (end < 0)
            {
                end = ddr.FieldCount;
            }
            if (typeof(T) == typeof(KubiuObject))
            {
                KubiuObject entity = new KubiuObject();
                string fieldName = "";
                for (int i = start; i < end; i++)
                {
                    fieldName = ddr.GetName(i);
                    entity .Add (fieldName,ddr.GetValue(i));
                }

                object obj = entity;
                return (T)obj;
            }
            else
            {
                KubiuReflector reflector = KubiuReflector.Cache(typeof(T), true);
                T entity = (T)reflector.NewObject();

                string fieldName = "";
                for (int i = start; i < end; i++)
                {
                    fieldName = ddr.GetName(i);
                    if (reflector.Properties.ContainsKey(fieldName))
                    {
                        reflector.Properties[fieldName].TrySetValue(entity, ddr.GetValue(i));
                    }

                }
                return entity;
            }
        }

        #endregion

        #region ExecuteScalar方法


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text, (DbParameter[])null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, (DbParameter[])null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParas"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] commandParas)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            using (DbConnection con = Factory.CreateConnection())
            {
                con.ConnectionString = ConnectionString;
                //con.Open();
                return ExecuteScalar(commandText, commandType, commandParas, con);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParas"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] commandParas)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString");
            using (DbConnection con = Factory.CreateConnection())
            {
                con.ConnectionString = ConnectionString;
                //con.Open();
                return await ExecuteScalarAsync(commandText, commandType, commandParas, con);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("ConnectionString");
            return ExecuteScalar(commandText, commandType, (DbParameter[])null, commandCon);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandparas"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] commandparas, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("Connection");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText");

            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, commandCon, (DbTransaction)null, commandType, commandText, commandparas, out mustCloseConnection);
            object retval = cmd.ExecuteScalar();
            
            if (mustCloseConnection)
            {
                commandCon.Close();
            }

            return retval;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandparas"></param>
        /// <param name="commandCon"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] commandparas, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("Connection");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText");

            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, commandCon, (DbTransaction)null, commandType, commandText, commandparas, out mustCloseConnection);
            object retval = await cmd.ExecuteScalarAsync();

            if (mustCloseConnection)
            {
                commandCon.Close();
            }

            return retval;
        }

        #endregion ExecuteScalar方法结束        
        
        #region 分页方法

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
        /// <param name="allcount">输出参数，输出总记录数</param>
        /// <returns></returns>
        public DataTable PagedDataTable(int pagesize, int pageindex,string maintable,string mainkey,string allrecord, string where, Hashtable innerhashtable, bool asc,out int allcount)
        {
            return PagedDataTable(pagesize, pageindex, maintable, mainkey, allrecord, where, innerhashtable, asc, string.Empty, out allcount);
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
        /// <param name="allcount">输出参数，输出总记录数</param>
        /// <returns></returns>
        public DataTable PagedDataTable(int pagesize, int pageindex, string maintable, string mainkey, string allrecord, string where, Hashtable innerhashtable, bool asc,string orderfield, out int allcount)
        {
            string[] sql = Provider.GetPageingSql(pagesize, pageindex, maintable, mainkey, allrecord, where, innerhashtable, asc, orderfield);

            DataTable dt = this.ExecuteDataTable(sql[0]);

            allcount = Convert.ToInt32(ExecuteScalar(sql[1]));
            return dt;
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
        /// <param name="allcount">输出参数，输出总记录数</param>
        /// <returns></returns>
        public IDataReader  PagedDataReader(int pagesize, int pageindex, string maintable, string mainkey, string allrecord, string where, Hashtable innerhashtable, bool asc, out int allcount)
        {
            return PagedDataReader(pagesize, pageindex, maintable, mainkey, allrecord, where, innerhashtable, asc, string.Empty, out allcount);
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
        /// <param name="allcount">输出参数，输出总记录数</param>
        /// <returns></returns>
        public IDataReader PagedDataReader(int pagesize, int pageindex, string maintable, string mainkey, string allrecord, string where, Hashtable innerhashtable, bool asc, string orderfield,out int allcount)
        {
            string[] sql = Provider.GetPageingSql(pagesize, pageindex, maintable, mainkey, allrecord, where, innerhashtable, asc,orderfield );

            IDataReader reader = ExecuteReader(sql[0]);
            
            allcount = Convert.ToInt32(ExecuteScalar(sql[1]));

            return reader;
        }
        #endregion
    }
}
