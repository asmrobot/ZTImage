using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.Database
{
    public class SqlWhereBuilder
    {
        private StringBuilder sql_Builder = new StringBuilder();
        public bool HasCondition { get; set; }

        public SqlWhereBuilder()
        {
            HasCondition = false;
        }

        public SqlWhereBuilder(string condition)
        {
            HasCondition = true;
            And(condition);
        }


        /// <summary>
        /// And条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string And(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                return "";
            }
            
            if (HasCondition)
            {
                sql_Builder.Append(" and ");
            }
            else
            {
                sql_Builder.Append(" ");
            }

            HasCondition = true;

            sql_Builder.Append(condition);
            return condition;
        }

        /// <summary>
        /// Or条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string Or(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                return "";
            }
            HasCondition = true;
            if (HasCondition)
            {
                sql_Builder.Append(" or ");
            }
            else
            {
                sql_Builder.Append(" ");
            }

            sql_Builder.Append(condition);
            return condition;
        }


        public override string ToString()
        {
            return sql_Builder.ToString();
        }



    }
}
