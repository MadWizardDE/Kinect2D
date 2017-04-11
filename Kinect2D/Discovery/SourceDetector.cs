using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace MadWizard.Kinect2D.Discovery
{
    public class SourceDetectedEventArgs : EventArgs
    {
        public IKinectSource Source;

        public SourceDetectedEventArgs(IKinectSource source)
        {
            this.Source = source;
        }
    }

    public class SourceDisconnectedEventArgs : EventArgs
    {
        public IKinectSource Source;

        public bool IsError;

        public SourceDisconnectedEventArgs(IKinectSource source, bool error)
        {
            this.Source = source;
            this.IsError = error;
        }
    }

    public partial class SourceDetector
    {
        private bool discovering;

        private IList<IKinectSource> sources;

        public SourceDetector()
        {
            sources = new List<IKinectSource>();

            DefaultSensor_Init();
        }

        public bool IsDiscovering
        {
            get
            {
                return false;
            }

            set
            {
                if (false && value != discovering)
                {
                    discovering = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsDiscovering"));
                }
            }
        }

        public IKinectSource[] AvailableSources
        {
            get
            {
                return sources.ToArray();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<SourceDetectedEventArgs> SourceDetected;
        public event EventHandler<SourceDisconnectedEventArgs> SourceDisconnected;

    }
}