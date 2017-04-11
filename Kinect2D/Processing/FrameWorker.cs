using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Processing
{
    abstract class FrameWorker<T>
    {
        protected T parent;

        private Thread thread;

        private volatile bool procesing;

        internal FrameWorker(T parent)
        {
            this.parent = parent;
        }

        public bool IsProcessing
        {
            get
            {
                return procesing;
            }
        }

        internal virtual void Start()
        {
            thread = Thread.CurrentThread;

            procesing = true;

            while (procesing)
            {
                try
                {
                    ProcessData();
                }
                catch (FrameTimeoutException)
                {
                    // Stop(false);
                }
            }
        }

        protected abstract void ProcessData();

        internal virtual void Stop(bool join)
        {
            if (procesing)
            {
                procesing = false;

                if (join)
                {
                    thread.Join();
                }

                thread = null;
            }
        }
    }
}