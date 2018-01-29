#if NET45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;


namespace ZTImage.Services.Daemons
{
    public class ZTServiceBase : ServiceBase 
    {
        private IServiceAction _service;
        public ZTServiceBase(IServiceAction action)
        {
            _service = action;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this._service.Start();
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                this._service.Stop();
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }

        protected override void OnPause()
        {
            try
            {
                this._service.Pause();
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }


        protected override void OnContinue()
        {
            try
            {
                this._service.Continue();
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }


        protected override void Dispose(bool disposing)
        {
            try
            {
                this._service.Stop();
                this._service = null;
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }


        protected override void OnShutdown()
        {
            try
            {
                this._service.Stop();
            }
            catch
            {
                ExitCode = 1064;
                throw;
            }
        }

    }

}
#endif