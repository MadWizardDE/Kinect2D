using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MadWizard.Kinect2D
{
    public enum SourceType
    {
        Unknown = 0, Local, Remote
    }

    public enum ConnectionType
    {
        Unknown = 0, USB, TCPIP
    }

    [Flags]
    public enum FrameType
    {
        None = 0,
        Position = 1 << 0,
        Image = 2 << 1
    }

    [Serializable]
    public struct Buddy
    {
        public ulong ID;

        public TimeSpan Time;

        public Point Position;

        public HandState HandLeftState;
        public HandState HandRightState;

        // TODO Image

        public Buddy(ulong id)
        {
            ID = id;

            Time = default(TimeSpan);

            Position = default(Point);

            HandLeftState = HandRightState = HandState.Unknown;
        }
    }

    [Serializable]
    public abstract class KinectData : EventArgs
    {

    }

    [Serializable]
    public class KinectFrameData : KinectData
    {
        public Buddy[] buddies;

        public Point leftEdge, rightEdge;
    }

    [Serializable]
    public class KinectStateData : KinectData
    {
        public string ID;

        public bool IsAvailable;

        public KinectStateData(string id, bool available)
        {
            ID = id;
            IsAvailable = available;
        }
    }

    public interface IKinectSource
    {
        string ID { get; }
        string Name { get; }

        SourceType SourceType { get; }
        ConnectionType ConnectionType { get; }
        FrameType FrameTypes { get; set; }

        bool IsAvailable { get; }

        event EventHandler<KinectStateData> IsAvailableChanged;
        event EventHandler<KinectFrameData> FrameArrived;
    }
}
