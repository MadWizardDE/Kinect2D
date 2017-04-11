using MadWizard.Kinect2D.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static MadWizard.Kinect2D.Processing.Coordinator;

namespace MadWizard.Kinect2D.Processing
{
    public enum SyncState
    {
        NotAvailable = 0,
        Synchronizing,
        Ready
    }

    public class SyncStateChangedEventArgs : EventArgs
    {
        public SyncState state;
    }

    public class Synchronizer
    {
        private IDictionary<IKinectSource, SourceConfig> sources;

        private SyncWorker worker;

        internal Synchronizer(Coordinator coordinator)
        {
            Coordinator = coordinator;

            sources = new Dictionary<IKinectSource, SourceConfig>();

            worker = new SyncWorker(this);
        }

        public SyncState this[IKinectSource source]
        {
            get
            {
                try
                {
                    return sources[source].State;
                }
                catch (KeyNotFoundException)
                {
                    return SyncState.NotAvailable;
                }
            }
        }

        public Coordinator Coordinator { get; private set; }

        public SyncState State { get; private set; } = SyncState.NotAvailable;

        public VirtualCamera[] Cameras
        {
            get
            {
                IList<VirtualCamera> list = new List<VirtualCamera>();

                foreach (IKinectSource source in sources.Keys)
                    if (sources[source].State == SyncState.Ready)
                        list.Add(sources[source].camera);

                return list.ToArray();
            }
        }

        public event EventHandler<SyncStateChangedEventArgs> SyncStateChanged;

        public void StartSynchronizing()
        {
            Debug.WriteLine("Synchronizer: StartSynchronizing()");

            if (State != SyncState.Synchronizing)
            {
                Coordinator.IsTracking = false;

                new Thread(worker.Start).Start();
            }
        }

        public void StopSynchronizing()
        {
            Debug.WriteLine("Synchronizer: StopSynchronizing()");

            if (State != SyncState.NotAvailable)
            {
                Coordinator.IsTracking = false;

                worker.Stop(true);

                sources.Clear();

                SyncStateChanged?.Invoke(this, new SyncStateChangedEventArgs { state = State = SyncState.NotAvailable });
            }
        }

        internal void Sync(IKinectSource source, KinectFrameData frame)
        {
            SourceConfig config = sources[source];

            for (int i = 0; i < frame.buddies.Length; i++)
            {
                frame.buddies[i].Position = config.ApplyTransform(frame.buddies[i].Position);
            }

            frame.leftEdge = config.ApplyTransform(frame.leftEdge);
            frame.rightEdge = config.ApplyTransform(frame.rightEdge);
        }

        internal VirtualCamera GetCamera(IKinectSource source)
        {
            return sources[source].camera;
        }

        internal void UpdateCamera(IKinectSource source, KinectFrameData data)
        {
            if (data.leftEdge.IsReal())
                sources[source].camera.LeftEdge = data.leftEdge;
            if (data.rightEdge.IsReal())
                sources[source].camera.RightEdge = data.rightEdge;
        }

        protected void onStateChange(SyncState state)
        {
            Debug.WriteLine(String.Format("state = {0}", state.ToString()), "Synchronizer");

            SyncStateChanged?.Invoke(this, new SyncStateChangedEventArgs { state = State = state });
        }

        private class SourceConfig
        {
            internal VirtualCamera camera;

            internal Point? p1, p2, p3;

            internal int frameDrop;

            internal Point ApplyTransform(Point position)
            {
                return camera.Transform(position);
            }

            internal SyncState State
            {
                get
                {
                    return camera != null ? SyncState.Ready : SyncState.Synchronizing;
                }
            }

            internal bool IsVisible
            {
                get
                {
                    return p1 != null && p2 != null && p3 != null;
                }
            }
        }

        private class SyncWorker : FrameWorker<Synchronizer>
        {
            const int FRAME_DROP = 20;

            internal SyncWorker(Synchronizer parent) : base(parent)
            {

            }

            internal override void Start()
            {
                parent.onStateChange(SyncState.Synchronizing);

                parent.sources.Clear();

                parent.Coordinator.ResetQueues();

                Debug.WriteLine(String.Format("try to sync {0} sources ... ", parent.Coordinator.AvailableSources.Length),
                    "Synchronizer");

                base.Start();
            }

            protected override void ProcessData()
            {
                IKinectSource[] sources = parent.Coordinator.Sources;

                foreach (IKinectSource source in sources)
                {
                    SourceConfig config;
                    if (!parent.sources.TryGetValue(source, out config))
                    {
                        config = new SourceConfig();

                        if (parent.Coordinator.MainSource == source)
                            config.camera = VirtualCamera.CreateDefault(source);

                        parent.sources[source] = config;
                    }

                    KinectFrameData data = parent.Coordinator.DequeueFrame(source);

                    if (config.frameDrop++ == FRAME_DROP)
                    {
                        config.p1 = config.p2;
                        config.p2 = config.p3;
                        if (data.buddies.Length == 1)
                            config.p3 = data.buddies[0].Position;
                        else
                            config.p3 = null;

                        config.frameDrop = 0;
                    }
                }

                int ready = 0;
                foreach (IKinectSource source in sources)
                {
                    SourceConfig config = parent.sources[source];

                    if (config.camera == null)
                    {
                        config.camera = TrySync(source);
                    }

                    if (config.State == SyncState.Ready)
                        ready++;
                }


                if (sources.Length == ready)
                {
                    Debug.WriteLine(String.Format("done ({0} ready)", ready));

                    parent.onStateChange(ready > 0 ? SyncState.Ready : SyncState.NotAvailable);

                    Stop(false);
                }
            }

            private VirtualCamera TrySync(IKinectSource source)
            {
                SourceConfig config = parent.sources[source];

                Point r1, r2, r3;
                if (config.IsVisible && SearchReference(out r1, out r2, out r3))
                {
                    Point p1 = config.p1.Value, p2 = config.p2.Value, p3 = config.p3.Value;

                    // Entfernung zum Ursprung = Radius
                    double d1 = Maths.Distance(new Point(0, 0), p1);
                    double d2 = Maths.Distance(new Point(0, 0), p2);
                    double d3 = Maths.Distance(new Point(0, 0), p3);

                    Point i1, i2;
                    if (!Maths.CircleCircleIntersection(r1, d1, r2, d2, out i1, out i2))
                        return null; // keine Lösung

                    Point i3, i4;
                    if (!Maths.CircleCircleIntersection(r1, d1, r3, d3, out i3, out i4))
                        return null; // keine Lösung

                    double d13 = Maths.Distance(i1, i3);
                    double d24 = Maths.Distance(i2, i4);

                    Point position;
                    double accuracy;
                    if (d13 < d24)
                    {
                        position = new Point((i1.X + i3.X) / 2.0, (i1.Y + i3.Y) / 2.0);
                        accuracy = d13;
                    }
                    else
                    {
                        position = new Point((i2.X + i4.X) / 2.0, (i2.Y + i4.Y) / 2.0);
                        accuracy = d24;
                    }

                    if (accuracy > 0.1)
                    {
                        Debug.WriteLine("accuracy too low: {0:0.00}", accuracy);
                        return null;
                    }

                    double rotation;
                    try
                    {
                        rotation = ApproximateRotation(position, p1, r1, accuracy);
                    }
                    catch (ArgumentException)
                    {
                        Debug.WriteLine("rotation non-determinable");
                        return null;
                    }

                    return new VirtualCamera(source, position, rotation, accuracy);
                }

                return null;
            }

            private bool SearchReference(out Point r1, out Point r2, out Point r3)
            {
                r1 = r2 = r3 = default(Point);

                foreach (SourceConfig config in parent.sources.Values)
                    if (config.State == SyncState.Ready && config.IsVisible)
                    {
                        r1 = config.ApplyTransform(config.p1.Value);
                        r2 = config.ApplyTransform(config.p2.Value);
                        r3 = config.ApplyTransform(config.p3.Value);
                        return true;
                    }

                return false;
            }

            private double ApproximateRotation(Point camera, Point source, Point target, double tolerance)
            {
                const double step = .1;

                for (double rotation = 0.0; rotation < 360.0; rotation += step)
                {
                    Point t = camera + (Vector)source;

                    Matrix matrix = new Matrix();
                    matrix.RotateAt(rotation, camera.X, camera.Y);

                    t = matrix.Transform(t);

                    if (Maths.Distance(t, target) < tolerance)
                        return rotation;
                }

                throw new ArgumentException("not rotationally symmetric");
            }
        }
    }
}