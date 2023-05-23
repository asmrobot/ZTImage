using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.TaskQueue
{
    public class DefaultTask : UnitTask
    {
        private Action _Action;
        public DefaultTask(Action action)
        {
            _Action = action;
        }
        public override void Execute()
        {
            try
            {
                _Action.Invoke();
            }
            catch (Exception ex)
            { }
        }
    }
}
