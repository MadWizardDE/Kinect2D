using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Sources
{
    class RemoteKinect : IKinectSource
    {
        const int READ_TIMEOUT = 1000;

        private TcpClient tcp;

        private string name;

        private string id;
        private bool available;

        private KinectStream kinectStream;

        internal RemoteKinect(IPEndPoint ip)
        {
            tcp = new TcpClient();

            if (!tcp.ConnectAsync(ip.Address, ip.Port).Wait(2000))
                throw new SocketException();

            Stream stream = tcp.GetStream();
            //stream.ReadTimeout = READ_TIMEOUT;

            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            writer.WriteLine("HELLO_KINECT_SERVER");

            string line = reader.ReadLine();
            if (line == null || line != "HELLO_KINECT_CLIENT")
                throw new ArgumentException(line);

            id = reader.ReadLine();
            name = reader.ReadLine();
            available = Boolean.Parse(reader.ReadLine());

            kinectStream = new KinectStream(stream);
            kinectStream.DataArrived += Stream_DataArrived;
            kinectStream.Disconnected += Stream_Disconnected;
            kinectStream.StartListening();
        }

        public SourceType SourceType
        {
            get
            {
                return SourceType.Remote;
            }
        }
        public ConnectionType ConnectionType
        {
            get
            {
                return ConnectionType.TCPIP;
            }
        }
        public FrameType FrameTypes
        {
            get
            {
                return FrameType.Position;
            }

            set
            {
                // throw new NotImplementedException();
            }
        }

        public string ID
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return available;
            }
        }

        public event EventHandler<KinectFrameData> FrameArrived;
        public event EventHandler<KinectStateData> IsAvailableChanged;

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public void Disconnect()
        {
            kinectStream.StopListening();
            kinectStream.SendDisconnect();
            kinectStream.Close();

            Disconnected?.Invoke(this, new DisconnectedEventArgs());
        }

        private void Stream_DataArrived(object sender, KinectData data)
        {
            if (data.GetType() == typeof(KinectFrameData))
                FrameArrived?.Invoke(this, (KinectFrameData)data);
            else if (data.GetType() == typeof(KinectStateData))
            {
                id = (data as KinectStateData).ID;
                available = (data as KinectStateData).IsAvailable;

                IsAvailableChanged?.Invoke(this, data as KinectStateData);
            }
        }

        private void Stream_Disconnected(object sender, DisconnectedEventArgs args)
        {
            Disconnected?.Invoke(this, args);
        }
    }
}