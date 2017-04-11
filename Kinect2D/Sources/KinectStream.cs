using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D
{
    [Serializable]
    public class DisconnectedEventArgs : EventArgs
    {
        public bool IsError;
    }

    public class KinectStream
    {
        private Stream stream;

        IFormatter formatter;

        bool listening;

        public KinectStream(Stream stream)
        {
            this.stream = stream;

            this.formatter = new BinaryFormatter();
        }

        public event EventHandler<KinectData> DataArrived;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        private void SendObject(object obj)
        {
            try
            {
                formatter.Serialize(stream, obj);
            }
            catch (SerializationException)
            {
                onDisconnect(true);
            }
            catch (IOException)
            {
                onDisconnect(true);
            }
        }

        public void SendData(KinectData data)
        {
            SendObject(data);
        }

        public void SendDisconnect()
        {
            SendObject(new DisconnectedEventArgs());
        }

        public void StartListening()
        {
            listening = true;

            new Thread(() =>
            {
                while (listening)
                {
                    try
                    {
                        object obj = formatter.Deserialize(stream);

                        if (obj is KinectData)
                            DataArrived?.Invoke(this, (KinectData)obj);
                        else if (obj is DisconnectedEventArgs)
                            Disconnected?.Invoke(this, (DisconnectedEventArgs)obj);
                    }
                    catch (ObjectDisposedException)
                    {

                    }
                    catch (SerializationException)
                    {
                        onDisconnect(true);
                    }
                    catch (IOException)
                    {
                        onDisconnect(true);
                    }
                }
            }).Start();
        }

        public void StopListening()
        {
            listening = false;
        }

        public void Close()
        {
            stream.Close();
        }

        protected void onDisconnect(bool error)
        {
            listening = false;

            Disconnected?.Invoke(this, new DisconnectedEventArgs { IsError = error });
        }
    }
}