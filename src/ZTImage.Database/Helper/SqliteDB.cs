using ZTImage.Database.HelperBase;
using System;
using ZTImage.Configuration;

namespace ZTImage.Database.Helper
{
    public class SqliteDB : SQLDBHelper
    {
        private SqliteDB()
        {}

        public override string ConnectionString
        {
            get 
            {
                //return ConfigHelper.GetInstance<DBConfigInfo>()["sqlitedb"].ConnectionString;
                throw new NotImplementedException("return connection string");
            }
        }

        

        public override string DbType
        {
            get
            {
                return "Sqlite";
            }
        }


        private static object _lockHelper = new object();
        private static SqliteDB _instance;
        public static SqliteDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new SqliteDB();
                        }
                    }
                }
                return _instance;
            }
        }

        public static void Reset()
        {
            _instance = null;
        }



    }
}
