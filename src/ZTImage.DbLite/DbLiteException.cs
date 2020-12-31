using System;
using System.Collections.Generic;
using System.Text;

namespace ZTImage.DbLite
{
    public class DbLiteException:Exception
    {
        public DbLiteException():base()
        {

        }

        public DbLiteException(string message):base(message)
        {

        }


        public DbLiteException(string message,Exception exception):base(message,exception)
        {

        }
    }
}
