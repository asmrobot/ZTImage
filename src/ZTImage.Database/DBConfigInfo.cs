using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using ZTImage.Configuration;
using ZTImage;

namespace ZTImage.Database
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [ConfigPath ("configs","db.config")]
    public　sealed class DBConfigInfo
    {
        /// <summary>
        /// 数据库连接列表
        /// </summary>
        [XmlArray]
        public Connection[] Connections;

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Connection this[string id]
        {
            get
            {
                if (Connections == null || Connections.Length <= 0)
                {
                    throw new DatabaseException("数据库配置文件中一个数据库都没有配置");
                }

                for (int i = 0, len = this.Connections.Length; i < len; i++)
                {
                    if (this.Connections[i].ID == id)
                    {
                        return this.Connections[i];
                    }
                }

                throw new DatabaseException("数据库配置文件中没有找到符合的配置");
            }
        }

        /// <summary>
        /// 添加
        /// 相同ID的将被替换
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public void AddConnection(Connection connection)
        {
            if (Connections == null)
            {
                Connections = new Connection[0];
            }

            int id = -1;
            for (int i = 0, len = this.Connections.Length; i < len; i++)
            {
                if (this.Connections[i].ID == connection .ID )
                {
                    id = i;
                    break;
                }
            }

            if (id == -1)
            {
                Connection[] temp = new Connection[this.Connections.Length+1];
                for (int i = 0, len = this.Connections.Length; i < len; i++)
                {
                    temp[i] = Connections[i];
                }
                this.Connections = temp;
                id = this.Connections.Length - 1;
            }

            this.Connections[id]=connection;
        }
    }

    /// <summary>
    /// 数据库连接
    /// </summary>
    [Serializable]
    public class Connection
    {
        /// <summary>
        /// 标识
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }


        /// <summary>
        /// 数据库类型
        /// </summary>
        [XmlAttribute]
        public string DBType { get; set; }


        /// <summary>
        /// 数据库连接串
        /// </summary>
        [XmlAttribute]
        public string ConnectionString { get; set; }
    }
}
