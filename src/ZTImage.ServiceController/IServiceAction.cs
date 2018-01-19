using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZTImage.ServiceController
{
    public interface IServiceAction
    {
        void Start();

        void Stop();

        void Pause();

        void Continue();
    }
}