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
        private static object lockHelper = new object();
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

        public abstract string ConnectionString
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

        /// <summary>
        /// 命令预处理
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="mustCloseConnection"></param>
        private static void PrepareCommand(DbCommand command, DbConnection connection, CommandType commandType, string commandText, DbParameter[] parameters, out bool mustCloseConnection)
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
            PrepareCommand(command, connection, commandType, commandText, parameters);
        }

        /// <summary>
        /// 命令预处理
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns>true:要关闭链接，false:不用关闭链接</returns>
        private async static Task<bool> PrepareCommandAsync(DbCommand command, DbConnection connection, CommandType commandType, string commandText, DbParameter[] parameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText.Length == 0 || commandText == null) throw new ArgumentNullException("commandText");

            bool mustCloseConnection = false;
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                await connection.OpenAsync();
            }
            else
            {
                mustCloseConnection = false;
            }

            PrepareCommand(command, connection, commandType, commandText, parameters);
            return mustCloseConnection;
        }

        private static void PrepareCommand(DbCommand command, DbConnection connection, CommandType commandType, string commandText, DbParameter[] parameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText.Length == 0 || commandText == null) throw new ArgumentNullException("commandText");



            //设置命令文本
            command.CommandText = commandText;

            //设置命令类型
            command.CommandType = commandType;

            //设置命令连接
            command.Connection = connection;
            //if (transaction != null)
            //{
            //    if (transaction.Connection == null) throw new ArgumentException("transaction connection is no seting");
            //    command.Transaction = transaction;
            //}


            if (parameters != null)
            {
                AttachParameter(command, parameters);
            }

            return;

        }

        //给命令添加参数
        private static void AttachParameter(DbCommand command, DbParameter[] parameters)
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
                        columnMeta.ColumnOridinal = TypeConverter.ObjectToInt(dt.Rows[i][1], 0);
                        columnMeta.ColumnSize = TypeConverter.ObjectToInt(dt.Rows[i][2], 4);
                        columnMeta.IsUnique = TypeConverter.ObjectToBool(dt.Rows[i][5], false);
                        columnMeta.IsKey = TypeConverter.ObjectToBool(dt.Rows[i][6], false);


                        columnMeta.FieldTypeName = ((Type)dt.Rows[i][11]).Name;
                        columnMeta.FieldType = (Type)dt.Rows[i][11];
                        columnMeta.AllowDBNull = TypeConverter.ObjectToBool(dt.Rows[i][12], true);


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
            DataTable dt = null;
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
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<Int32> ExecuteNonQueryAsync(string commandText)
        {
            return await ExecuteNonQueryAsync(commandText, CommandType.Text, (DbParameter[])null); ;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<Int32> ExecuteNonQueryAsync(string commandText, CommandType commandType)
        {
            return await ExecuteNonQueryAsync(commandText, commandType, (DbParameter[])null); ;
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
                return ExecuteNonQuery(commandText, commandType, parameters, connection);
            }
        }


        public async Task<Int32> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectiong String");
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                return await ExecuteNonQueryAsync(commandText, commandType, parameters, connection);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, DbConnection connection)
        {
            return ExecuteNonQuery(commandText, commandType, (DbParameter[])null, connection);
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
            try
            {
                DbCommand cmd = Factory.CreateCommand();
                PrepareCommand(cmd, connection, commandType, commandText, parameters, out mustCloseConnection);

                int result = cmd.ExecuteNonQuery();
                query_count++;
                cmd.Parameters.Clear();

                return result;
            }
            finally
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
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
        public async Task<Int32> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;

            try
            {
                mustCloseConnection = await PrepareCommandAsync(cmd, connection, commandType, commandText, parameters);
                int result = await cmd.ExecuteNonQueryAsync();
                query_count++;
                cmd.Parameters.Clear();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
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
            try
            {
                DbCommand cmd = Factory.CreateCommand();
                PrepareCommand(cmd, connection, commandType, commandText, parameters, out mustCloseConnection);
                int result = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                query_count++;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Provider.GetLastIdSql();
                id = cmd.ExecuteScalar();

                query_count++;
                return result;
            }
            finally
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
            }
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
            return ExecuteDataTable(commandText, CommandType.Text, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameter"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, DbParameter[] commandParameter)
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
            return ExecuteDataTable(commandText, commandType, (DbParameter[])null, commandCon);
        }




        public DataTable ExecuteDataTable(out int allCount, string commandText, CommandType commandType, DbParameter[] commandParameter, DbConnection commandCon)
        {
            if (commandCon == null) throw new ArgumentNullException("commandCon");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            DbCommand command = Factory.CreateCommand();
            bool mustCloseConn = false;
            PrepareCommand(command, commandCon, commandType, commandText, commandParameter, out mustCloseConn);

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
            PrepareCommand(command, commandCon, commandType, commandText, commandParameter, out mustCloseConn);

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
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text, (DbParameter[])null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText)
        {
            return await ExecuteReaderAsync(commandText, CommandType.Text, (DbParameter[])null);
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
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, CommandType commandType)
        {
            return await ExecuteReaderAsync(commandText, commandType, (DbParameter[])null);
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
            return ExecuteReader(commandText, commandType, commandPara, con);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("ConnectionString is Null");

            DbConnection con = Factory.CreateConnection();
            con.ConnectionString = ConnectionString;
            return await ExecuteReaderAsync(commandText, commandType, commandPara, con);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <param name="connection"></param>
        /// <param name="ownership"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType, DbParameter[] commandPara, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("ConnectionString is Null");

            bool mustCloseConection = false;

            DbCommand cmd = Factory.CreateCommand();
            try
            {
                PrepareCommand(cmd, connection, commandType, commandText, commandPara, out mustCloseConection);
                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

                return reader;
            }
            catch
            {
                if (mustCloseConection)
                {
                    connection.Close();
                }

                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <param name="connection"></param>
        /// <param name="ownership"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, DbParameter[] commandPara, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("ConnectionString is Null");

            bool mustCloseConection = false;
            DbCommand cmd = Factory.CreateCommand();
            try
            {
                mustCloseConection = await PrepareCommandAsync(cmd, connection, commandType, commandText, commandPara);

                DbDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
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

                return reader;
            }
            catch
            {
                if (mustCloseConection)
                {
                    connection.Close();
                }

                throw;
            }

        }

        #endregion ExecuteReader方法结束

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
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string commandText)
        {
            return await ExecuteScalarAsync(commandText, CommandType.Text, (DbParameter[])null);
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
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType)
        {
            return await ExecuteScalarAsync(commandText, commandType, (DbParameter[])null);
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
                return await ExecuteScalarAsync(commandText, commandType, commandParas, con);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandparas"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] commandparas, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("Connection");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText");

            bool mustCloseConnection = false;
            DbCommand cmd = Factory.CreateCommand();
            try
            {

                PrepareCommand(cmd, connection, commandType, commandText, commandparas, out mustCloseConnection);
                return cmd.ExecuteScalar();
            }
            finally
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParas"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] commandParas, DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("Connection");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("CommandText");

            bool mustCloseConnection = false;
            try
            {
                DbCommand cmd = Factory.CreateCommand();
                mustCloseConnection = await PrepareCommandAsync(cmd, connection, commandType, commandText, commandParas);
                object retval = await cmd.ExecuteScalarAsync();
                return retval;
            }
            finally
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
            }
        }

        #endregion ExecuteScalar方法结束  

        #region Fill Reader

        /// <summary>
        /// 填充dbreader list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public List<T> FillList<T>(DbDataReader ddr)
        {
            List<T> list = new List<T>();
            while (!ddr.IsClosed && ddr.Read())
            {
                list.Add(FillObject<T>(ddr, 0, -1));
            }
            return list;
        }


        /// <summary>
        /// 填充dbreader list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public async Task<List<T>> FillListAsync<T>(DbDataReader ddr)
        {
            List<T> list = new List<T>();
            while (!ddr.IsClosed && await ddr.ReadAsync())
            {
                list.Add(FillObject<T>(ddr, 0, -1));
            }
            return list;
        }

        /// <summary>
        /// 填充reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public T FillObject<T>(DbDataReader ddr)
        {
            return FillObject<T>(ddr, 0, -1);
        }


        /// <summary>
        /// 填充dbreader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ddr"></param>
        /// <returns></returns>
        public T FillObject<T>(DbDataReader ddr, int start, int end)
        {
            if (end == 0)
            {
                return default(T);
            }

            if (ddr.IsClosed)
            {
                return default(T);
            }
            if (end < 0)
            {
                end = ddr.FieldCount;
            }

            if (start > end)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            
            if (typeof(T) == typeof(ZTObject))
            {
                ZTObject entity = new ZTObject();
                string fieldName = "";
                for (int i = start; i < end; i++)
                {
                    fieldName = ddr.GetName(i);
                    entity.Add(fieldName, ddr.GetValue(i));
                }

                object obj = entity;
                return (T)obj;
            }
            else
            {
                ZTReflector reflector = ZTReflector.Cache(typeof(T), true);
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TResult>(string commandText)
        {
            return await QueryAsync<TResult>(commandText, CommandType.Text);
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
            DbDataReader ddr = null;
            try
            {
                ddr = ExecuteReader(commandText, commandType, commandPara);
                List<TResult> list = FillList<TResult>(ddr);
                return list;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TResult>(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = null;
            try
            {
                ddr = await ExecuteReaderAsync(commandText, commandType, commandPara);
                List<TResult> list = await FillListAsync<TResult>(ddr);
                return list;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public MultiList<TResult1, TResult2> Query<TResult1, TResult2>(string commandText, string sepChar, CommandType commandType, params DbParameter[] commandPara)
        {

            DbDataReader ddr = null;
            try
            {
                ddr = ExecuteReader(commandText, commandType, commandPara);
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

                    TResult1 entity1 = FillObject<TResult1>(ddr, 0, sepIndex);
                    TResult2 entity2 = FillObject<TResult2>(ddr, sepIndex, -1);

                    list.Add(entity1, entity2);
                }
                return list;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }


        }



        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public async Task<MultiList<TResult1, TResult2>> QueryAsync<TResult1, TResult2>(string commandText, string sepChar, CommandType commandType, params DbParameter[] commandPara)
        {

            DbDataReader ddr = null;
            try
            {
                ddr = await ExecuteReaderAsync(commandText, commandType, commandPara);
                MultiList<TResult1, TResult2> list = new MultiList<TResult1, TResult2>();
                int sepIndex = 0;

                bool isinit = false;
                while (!ddr.IsClosed && await ddr.ReadAsync())
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

                    TResult1 entity1 = FillObject<TResult1>(ddr, 0, sepIndex);
                    TResult2 entity2 = FillObject<TResult2>(ddr, sepIndex, -1);

                    list.Add(entity1, entity2);
                }
                return list;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<TResult> QueryOneAsync<TResult>(string commandText)
        {
            return await QueryOneAsync<TResult>(commandText, CommandType.Text);
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
            DbDataReader ddr = null;
            try
            {
                ddr = ExecuteReader(commandText, commandType, commandPara);
                T entity = default(T);
                if (!ddr.IsClosed && ddr.Read())
                {
                    entity = FillObject<T>(ddr, 0, -1);
                }
                return entity;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }
        }


        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public async Task<T> QueryOneAsync<T>(string commandText, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = null;
            try
            {
                ddr =await ExecuteReaderAsync(commandText, commandType, commandPara);
                T entity = default(T);
                if (!ddr.IsClosed && await ddr.ReadAsync())
                {
                    entity = FillObject<T>(ddr, 0, -1);
                }
                return entity;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }
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
            DbDataReader ddr = null;
            try
            {
                ddr = ExecuteReader(commandText, commandType, commandPara);
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

                    TResult1 entity1 = FillObject<TResult1>(ddr, 0, sepIndex);
                    TResult2 entity2 = FillObject<TResult2>(ddr, sepIndex, -1);


                    entity = new MultiObject<TResult1, TResult2>(entity1, entity2);
                }

                return entity;
            }
            finally
            {
                if (ddr!=null&&!ddr.IsClosed)
                {
                    ddr.Close();
                }
            }

        }

        /// <summary>
        /// 查询某个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandPara"></param>
        /// <returns></returns>
        public async  Task<MultiObject<TResult1, TResult2>> QueryOneAsync<TResult1, TResult2>(string commandText, string sepChar, CommandType commandType, params DbParameter[] commandPara)
        {
            DbDataReader ddr = null;
            try
            {
                ddr =await ExecuteReaderAsync(commandText, commandType, commandPara);
                MultiObject<TResult1, TResult2> entity = new MultiObject<TResult1, TResult2>(default(TResult1), default(TResult2));
                if (!ddr.IsClosed && await ddr.ReadAsync())
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

                    TResult1 entity1 = FillObject<TResult1>(ddr, 0, sepIndex);
                    TResult2 entity2 = FillObject<TResult2>(ddr, sepIndex, -1);


                    entity = new MultiObject<TResult1, TResult2>(entity1, entity2);
                }

                return entity;
            }
            finally
            {
                if (ddr != null && !ddr.IsClosed)
                {
                    ddr.Close();
                }
            }

        }
        #endregion


    }
}
