using System;
using System.Collections.Generic;
using System.Text;

namespace ZTImage.DbLite
{
    public class DbConnectionOptions:IEquatable<DbConnectionOptions>
    {
        /// <summary>
        /// 是否默认数据库
        /// </summary>
        public bool Default { get; set; } = false;

        /// <summary>
        /// 数据库编号
        /// </summary>
        public string DbID { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }


        #region interface implements

        public bool Equals(DbConnectionOptions other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.DbID.Equals(other.DbID))
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DbConnectionOptions);
        }

        public override int GetHashCode()
        {
            return this.DbID.GetHashCode() ^
                this.DbType.GetHashCode() ^
                this.ConnectionString.GetHashCode();
        }
        #endregion
    }
}
