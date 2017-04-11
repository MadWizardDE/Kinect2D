using MadWizard.Kinect2D.Discovery;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MadWizard.Kinect2D.Sources
{
    public class DefaultKinect : IKinectSource
    {
        private KinectSensor sensor;

        private FrameType frameTypes;

        private MultiSourceFrameReader frameReader;

        public DefaultKinect(KinectSensor sensor)
        {
            this.sensor = sensor;

            frameReader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.Depth);
            frameReader.MultiSourceFrameArrived += this.Sensor_MultiSourceFrameArrived;

            sensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            sensor.Open();
        }

        public string ID { get; private set; }

        public virtual string Name
        {
            get
            {
                return "Default Kinect";
            }
        }

        public SourceType SourceType
        {
            get
            {
                return SourceType.Local;
            }
        }

        public ConnectionType ConnectionType
        {
            get
            {
                return ConnectionType.USB;
            }
        }

        public FrameType FrameTypes
        {
            get
            {
                return frameTypes;
            }

            set
            {
                frameReader.IsPaused = (frameTypes = value) == FrameType.None;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return sensor.IsAvailable;
            }
        }

        public event EventHandler<KinectFrameData> FrameArrived;
        public event EventHandler<KinectStateData> IsAvailableChanged;

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            IsAvailableChanged?.Invoke(this, new KinectStateData(ID = sensor.UniqueKinectId, e.IsAvailable));
        }

        void Sensor_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // If the Frame has expired by the time we process this event, return.
            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
            if (multiSourceFrame == null)
                return;

            KinectFrameData data = new KinectFrameData();

            IList<Buddy> buddies = new List<Buddy>();
            using (BodyFrame bodyFrame = multiSourceFrame.BodyFrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];

                    // Skelett-Daten lesen
                    bodyFrame.GetAndRefreshBodyData(bodies);

                    foreach (Body body in bodies)
                        if (body.IsTracked)
                        {
                            Joint spine = body.Joints[JointType.SpineMid];

                            if (spine != null)
                            {
                                Buddy buddy = new Buddy(body.TrackingId);
                                buddy.Position.X = -spine.Position.X;
                                buddy.Position.Y = spine.Position.Z;

                                if (body.HandLeftConfidence == TrackingConfidence.High)
                                    buddy.HandLeftState = body.HandLeftState;
                                if (body.HandRightConfidence == TrackingConfidence.High)
                                    buddy.HandRightState = body.HandRightState;

                                buddies.Add(buddy);
                            }
                        }
                }
            }
            data.buddies = buddies.ToArray();

            using (DepthFrame frame = multiSourceFrame.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var desc = frame.FrameDescription;
                    int height = desc.Height;
                    int width = desc.Width;

                    ushort[] frameData = new ushort[width * height];
                    frame.CopyFrameDataToArray(frameData);

                    DepthSpacePoint[] depthPoints = new DepthSpacePoint[2];
                    depthPoints[0].X = 2;
                    depthPoints[0].Y = height / 2.0f;
                    depthPoints[1].X = width - 2;
                    depthPoints[1].Y = depthPoints[0].Y;

                    ushort[] depths = new ushort[2];
                    depths[0] = frameData[(int)(depthPoints[0].Y * width + depthPoints[0].X)];
                    depths[1] = frameData[(int)(depthPoints[1].Y * width + depthPoints[1].X)];

                    CameraSpacePoint[] csPoints = new CameraSpacePoint[2];

                    sensor.CoordinateMapper.MapDepthPointsToCameraSpace(depthPoints, depths, csPoints);

                    data.leftEdge = new System.Windows.Point(-csPoints[0].X, csPoints[0].Z);
                    data.rightEdge = new System.Windows.Point(-csPoints[1].X, csPoints[1].Z);
                }
            }

            FrameArrived?.Invoke(this, data);
        }
    }
}