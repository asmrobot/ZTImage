using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZTImage.TaskQueue;

namespace Demo
{
    public class TaskQueueDemo:DemoUnit
    {
        public override void Do()
        {
            TaskQueue tasks = new TaskQueue("demo");
            tasks.Start();


            //测试并发任务入队
            Console.WriteLine("MessageQueue 并行入队 1000 个任务...");
            Parallel.For(0, 1000, new Action<int>(index =>
            {
                tasks.Enqueue(() => {
                    Console.WriteLine($"{DateTime.Now},执行任务完成！----{index}");
                    //Thread.Sleep(1000);
                    work1().Wait();
                });
            }));
            Console.WriteLine($"MessageQueue 队列内任务数量：{tasks.TaskCount}");
            //tasks.Stop();
        }

        private async Task work1()
        {
            await Task.Delay(1000);
        }
    }
}
