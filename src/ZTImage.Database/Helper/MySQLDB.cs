using ZTImage.Database.HelperBase;
using System;
using ZTImage.Configuration;

namespace ZTImage.Database.Helper
{
    public class MySQLDB :SQLDBHelper
    {
        private MySQLDB()
        {}

        public override string ConnectionString
        {
            get 
            {
                return ConfigHelper.GetInstance<DBConfigInfo>()["mysqldb"].ConnectionString;
            }
        }

        

        public override string DbType
        {
            get
            {
                return "MySql";
            }
        }


        private static object _lockHelper = new object();
        private static MySQLDB _instance;
        public static MySQLDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new MySQLDB();
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
