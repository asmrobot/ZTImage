namespace ZTImage.TaskQueue
{
    /// <summary>
    /// 单元任务
    /// </summary>
    public abstract class UnitTask
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public UnitTask(string name) => this.Name = name;

        public UnitTask()
        {
            this.Name = "未命名";
        }

        public override string ToString() => $"任务：{this.Name}";

        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void Execute();
    }
}
