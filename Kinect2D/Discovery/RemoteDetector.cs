using MadWizard.Kinect2D.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Discovery
{
    public partial class SourceDetector
    {
        public void Connect(IPEndPoint ip)
        {
            RemoteKinect remote = new RemoteKinect(ip);

            try
            {
                foreach (IKinectSource source in sources)
                    if (remote.IsAvailable == false || String.IsNullOrWhiteSpace(remote.ID))
                        throw new NotAvailableException(remote);
                    else if (remote.ID == source.ID)
                        throw new AlreadyConnectedException(remote);
            }
            catch (ConnectException e)
            {
                (e.KinectSource as RemoteKinect).Disconnect();

                throw;
            }

            remote.Disconnected += RemoteKinect_Disconnected;
            sources.Add(remote);

            SourceDetected?.Invoke(this, new SourceDetectedEventArgs(remote));
        }

        public void Disconnect(IKinectSource source)
        {
            if (source is RemoteKinect)
            {
                RemoteKinect remote = (RemoteKinect)source;

                remote.Disconnect();
            }
            else
                throw new ArgumentException();
        }

        private void RemoteKinect_Disconnected(object sender, DisconnectedEventArgs e)
        {
            IKinectSource source = (IKinectSource)sender;

            if (sources.Remove(source))
            {
                SourceDisconnected?.Invoke(this, new SourceDisconnectedEventArgs(source, e.IsError));
            }
        }
    }

    public abstract class ConnectException : Exception
    {
        protected ConnectException(IKinectSource source)
        {
            KinectSource = source;
        }

        public IKinectSource KinectSource { get; private set; }
    }

    public class AlreadyConnectedException : ConnectException
    {
        internal AlreadyConnectedException(IKinectSource source) : base(source)
        {
        }
    }

    public class NotAvailableException : ConnectException
    {
        internal NotAvailableException(IKinectSource source) : base(source)
        {
        }
    }
}