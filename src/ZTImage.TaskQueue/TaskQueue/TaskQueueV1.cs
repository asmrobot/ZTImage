using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace ZTImage.TaskQueue
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public class TaskQueueV1
        <T> : IDisposable where T : UnitTask
    {
        #region 事件

        /// <summary>
        /// 有任务入队事件
        /// </summary>
        public event EventHandler<T> TaskEnqueued;// { add { } remove { } }

        /// <summary>
        /// 有任务出队事件
        /// </summary>
        public event EventHandler TaskDequeued;// { add { } remove { } }

        /// <summary>
        /// 队列开始执行
        /// </summary>
        public event DoWorkEventHandler QueueStarted;// { add { } remove { } }

        /// <summary>
        /// 队列停止执行
        /// </summary>
        public event RunWorkerCompletedEventHandler QueueStoped;// { add { } remove { } }

        /// <summary>
        /// 队列进入空闲状态
        /// </summary>
        public event EventHandler Idle;// { add { } remove { } }
        #endregion

        #region 属性

        /// <summary>
        /// 任务队列名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 任务列表（线程安全）
        /// </summary>
        private ConcurrentQueue<T> Tasks { get; set; } = new ConcurrentQueue<T>();

        /// <summary>
        /// 任务列表（只读）
        /// </summary>
        public T[] ReadOnlyTasks { get => this.Tasks.ToArray(); }

        /// <summary>
        /// 队列内任务总数
        /// </summary>
        public int TaskCount { get => this.Tasks?.Count() ?? 0; }
        #endregion

        #region 变量

        /// <summary>
        /// 任务执行线程
        /// </summary>
        private readonly BackgroundWorker TaskWorker = new BackgroundWorker()
        {
            WorkerReportsProgress = false,
            WorkerSupportsCancellation = true
        };

        /// <summary>
        /// 任务控制信号量（防止队列循环空转）
        /// </summary>
        private readonly ManualResetEvent QueueEvent = new ManualResetEvent(false);
        #endregion

        public TaskQueueV1(string name)
        {
            this.Name = name;

            this.TaskWorker.DoWork += this.ExecuteTasks;
            this.TaskWorker.RunWorkerCompleted += this.ExecuteFinished;
        }

        /// <summary>
        /// 任务入队
        /// </summary>
        /// <param name="task"></param>
        public void Enqueue(T task)
        {
            if (task == null) return;

            if (this.TaskCount == 0 && this.TaskWorker.IsBusy)
            {
                Console.WriteLine($"<{this.Name}> 队列信号量 Enqueue-Set()");
                this.QueueEvent.Set();
            }
            this.Tasks.Enqueue(task);

            TaskEnqueued?.Invoke(this, task);
        }

        /// <summary>
        /// 任务出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            bool result = this.Tasks.TryDequeue(out T task);
            if (result) TaskDequeued?.Invoke(this, null);
            return task;
        }

        /// <summary>
        /// 任务队列开始执行
        /// </summary>
        public void Start()
        {
            if (this.TaskWorker.IsBusy) return;
            this.TaskWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 任务队列开始执行
        /// </summary>
        public void Start(object argument)
        {
            if (this.TaskWorker.IsBusy) return;
            this.TaskWorker.RunWorkerAsync(argument);
        }

        /// <summary>
        /// 任务队列停止执行
        /// </summary>
        public void Stop()
        {
            if (!this.TaskWorker.IsBusy) return;
            this.TaskWorker.CancelAsync();
            Console.WriteLine($"<{this.Name}> 队列信号量 Stop-Set()");
            this.QueueEvent.Set();
        }

        /// <summary>
        /// 开始轮询执行任务
        /// </summary>
        private void ExecuteTasks(object sender, DoWorkEventArgs e)
        {
            QueueStarted?.Invoke(this, e);
            Console.WriteLine($"<{this.Name}> 内 Worker 启动...");

            while (true)
            {
                try
                {
                    //Thread.Sleep(1000);
                    Console.WriteLine($"<{this.Name}> 队列内任务数：{this.TaskCount}");

                    if ((sender as BackgroundWorker).CancellationPending) return;
                    if (this.TaskCount == 0)
                    {
                        Console.WriteLine($"<{this.Name}> 队列信号量 Execute-WaitOne");
                        // 阻塞队列状态
                        this.QueueEvent.Reset();
                        this.QueueEvent.WaitOne();

                        //队列进入空闲状态，触发空闲事件
                        Idle?.Invoke(this, null);
                        //WaitOne 之后要先 continue 一次
                        continue;
                    }

                    T task = this.Dequeue();
                    if (task == null) continue;

                    task.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"<{this.Name}> 队列内发生异常：{ex.Message}");
                }
            }
        }

        /// <summary>
        /// 任务轮询结束
        /// </summary>
        private void ExecuteFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine($"<{this.Name}> 内 Worker 停止...");
            Console.WriteLine($"<{this.Name}> 队列内剩余任务数：{this.TaskCount}");

            if (e.Error != null) Console.WriteLine($"<{this.Name}> 队列内发生异常：{e.Error.Message}");
            QueueStoped?.Invoke(this, e);
        }

        #region IDisposable Support

        public void Dispose()
        {
            this.Stop();
            this.TaskWorker.Dispose();
            while (this.Tasks.TryDequeue(out T task)) { }
            this.Tasks = null;
            this.QueueEvent.Close();
            this.QueueEvent.Dispose();

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
