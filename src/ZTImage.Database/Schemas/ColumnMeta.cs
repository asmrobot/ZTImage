using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Database.Schemas
{
    /// <summary>
    /// 列原数据
    /// </summary>
    public class ColumnMeta
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnOridinal
        {
            get;
            set;
        }


        /// <summary>
        /// 字段占用空间大小
        /// </summary>
        public int ColumnSize
        {
            get;
            set;
        }


        /// <summary>
        /// 是否唯一标识
        /// </summary>
        public bool IsUnique
        {
            get;
            set;
        }

        /// <summary>
        /// 是否主键或外键
        /// </summary>
        public bool IsKey
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool AllowDBNull
        {
            get;
            set;
        }

        /// <summary>
        /// 字段类型名
        /// </summary>
        public string FieldTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType
        {
            get;
            set;
        }

        

        
    }
}
