using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZTImage.Configuration;

namespace ZTImage.SchedulerDaemon
{
    [ConfigPath("configs","sortout.config")]
    public class SortOutConfigInfo
    {
        [XmlArray]
        public JobInfo[] Jobs
        {
            get;
            set;

        }

        public JobInfo this[string Name]
        {
            get
            {
                if (Jobs == null || Jobs.Length <= 0)
                {
                    throw new ArgumentOutOfRangeException("jobs");
                }

                for (int i = 0;  i < Jobs.Length;i++)
                {
                    if (this.Jobs[i].Name == Name)
                    {
                        return this.Jobs[i];
                    }
                }
                throw new ArgumentNullException("jobs return ");
            }
        }
    }

    [Serializable]
    public class JobInfo
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// 任务程序集
        /// </summary>
        public string JobAssembly { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public string Data{ get; set; }
    }
}
