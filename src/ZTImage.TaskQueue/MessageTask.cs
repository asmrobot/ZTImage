using System;
using System.Threading;

namespace ZTImage.TaskQueue
{
    public class MessageTask : UnitTask
    {
        public MessageTask(string name) : base(name) { }

        public override void Execute()
        {
            Thread.Sleep(new Random().Next(1000));
            Console.WriteLine($"[{this.Name}] 执行任务完成！");
        }

    }
}
