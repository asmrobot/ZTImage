using System;
using ZTImage.Database.HelperBase;
using ZTImage.Configuration;

namespace ZTImage.Database.Helper
{
    public class SqlServerDB :SQLDBHelper
    {
        private SqlServerDB()
        {
            
        }

        public override string ConnectionString
        {
            get
            {
                //return ConfigHelper.GetInstance<DBConfigInfo>()["sqlserverdb"].ConnectionString;
                throw new NotImplementedException("return connection string");

            }
        }

        public override string DbType
        {
            get { return "SqlServer"; }
        }


        private static object _lockHelper = new object();
        private static SqlServerDB _instance;
        public static SqlServerDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new SqlServerDB();
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
