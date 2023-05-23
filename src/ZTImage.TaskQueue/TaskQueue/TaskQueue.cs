using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.TaskQueue
{
    public class TaskQueue : TaskQueueV2<DefaultTask>
    {
        public TaskQueue(string name) : base(name)
        {
        }

        /// <summary>
        /// 任务入队
        /// </summary>
        /// <param name="task"></param>
        public void Enqueue(Action task)
        {
            if (task == null)
                return;

            Enqueue(new DefaultTask(task));
        }
    }
}
