using MadWizard.Kinect2D.Tools;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MadWizard.Kinect2D.Processing
{
    public class SceneChangedEventArgs : EventArgs
    {
        public Scene scene;
    }

    public class Coordinator
    {
        private IDictionary<IKinectSource, SourceConfig> sources;

        private ISet<VirtualBuddy> buddies;

        private FrameWorker worker;

        private ulong nextID = 1;

        public Coordinator()
        {
            // init Collections
            sources = new Dictionary<IKinectSource, SourceConfig>();
            buddies = new HashSet<VirtualBuddy>();

            worker = new FrameWorker(this);

            Config = new CoordinatorConfig();
            Synchronizer = new Synchronizer(this);
            Calibrator = new Calibrator();
        }

        public Scene Scene
        {
            get
            {
                Scene scene = new Scene();
                scene.CaptureArea = Calibrator.CaptureArea;
                scene.Cameras = Synchronizer.Cameras;
                scene.Buddies = buddies.ToArray();
                return scene;
            }
        }

        public bool IsTracking
        {
            get
            {
                return worker.IsProcessing;
            }

            set
            {
                if (value != IsTracking)
                {
                    if (value)
                    {
                        if (Synchronizer.State != SyncState.Ready)
                            throw new InvalidOperationException();

                        new Thread(worker.Start).Start();
                    }
                    else
                    {
                        worker.Stop(true);

                        buddies.Clear();

                        UpdateScene();
                    }

                    Debug.WriteLine(String.Format("tracking = {0}", value), "Coordinator");
                }
            }
        }

        public IKinectSource MainSource
        {
            get
            {
                foreach (IKinectSource source in sources.Keys)
                    if (sources[source].main)
                        return source;

                return null;
            }

            set
            {
                if (value != MainSource)
                {
                    if (!sources.ContainsKey(value))
                        throw new ArgumentException();

                    if (MainSource != null)
                        sources[MainSource].main = false;
                    if (value != null)
                        sources[value].main = true;

                    Synchronizer.StopSynchronizing();
                }
            }
        }

        public IKinectSource[] Sources
        {
            get
            {
                return sources.Keys.ToArray();
            }
        }

        public IKinectSource[] AvailableSources
        {
            get
            {
                IList<IKinectSource> list = new List<IKinectSource>();
                foreach (IKinectSource source in sources.Keys)
                    if (source.IsAvailable)
                        list.Add(source);
                return list.ToArray();
            }
        }

        public CoordinatorConfig Config { get; private set; }
        public Synchronizer Synchronizer { get; private set; }
        public Calibrator Calibrator { get; private set; }

        public event EventHandler<SceneChangedEventArgs> SceneChanged;

        public event VirtualBuddyCallback VirtualBuddyEntered;
        public event VirtualBuddyCallback VirtualBuddyMoved;
        public event VirtualBuddyCallback VirtualBuddyLeft;

        public void Add(IKinectSource source)
        {
            if (sources.ContainsKey(source))
                throw new ArgumentException();

            sources[source] = new SourceConfig(sources.Count == 0, Config.InputBufferSize);

            // Event-Listener
            source.IsAvailableChanged += Source_IsAvailableChanged;
            source.FrameArrived += Source_FrameArrived;

            source.FrameTypes = FrameType.Position;

            Debug.WriteLine("Coordinator: Source added ('" + source.Name + "')");
        }

        public void Remove(IKinectSource source)
        {
            source.FrameTypes = FrameType.None;

            // Event-Listener
            source.FrameArrived -= Source_FrameArrived;
            source.IsAvailableChanged -= Source_IsAvailableChanged;

            sources.Remove(source);

            Debug.WriteLine("Coordinator: Source removed ('" + source.Name + "')");
        }

        internal void ResetQueues()
        {
            foreach (SourceConfig data in sources.Values)
            {
                data.Clear(Config.InputBufferSize);
            }
        }

        private void Source_FrameArrived(object sender, KinectFrameData data)
        {
            IKinectSource source = (IKinectSource)sender;

            try
            {
                sources[source].inputQueue.Enqueue(data);
            }
            catch (KeyNotFoundException)
            {

            }
        }

        private void Source_IsAvailableChanged(object sender, EventArgs e)
        {
            //IKinectSource source = (IKinectSource)sender;

            //if (Synchronizer.State == SyncState.Synchronizing)
            //    Synchronizer.StartSynchronizing();
        }

        private void Source_Disconnect(object sender, EventArgs e)
        {
            IKinectSource source = (IKinectSource)sender;

            if (!sources.ContainsKey(source))
            {
                SourceConfig config = sources[source];

                Remove(source);

                if (config.main && sources.Count > 0)
                    sources.First().Value.main = true;
            }
        }

        internal KinectFrameData DequeueFrame(IKinectSource source)
        {
            int waitTime = 0;

            while (waitTime < Config.SourceTimeout)
            {
                KinectFrameData frameData;
                if (sources[source].inputQueue.TryDequeue(out frameData))
                    return frameData;

                Thread.Sleep(10);

                waitTime += 10;
            }

            throw new FrameTimeoutException(source);
        }

        protected void AddVirtualBuddy(VirtualBuddy vBuddy)
        {
            buddies.Add(vBuddy);

            VirtualBuddyEntered?.Invoke(vBuddy);
        }

        protected void RemoveVirtualBuddy(VirtualBuddy vBuddy)
        {
            buddies.Remove(vBuddy);

            VirtualBuddyLeft?.Invoke(vBuddy);
        }

        protected void UpdateScene()
        {
            SceneChanged?.Invoke(this, new SceneChangedEventArgs { scene = Scene });
        }

        public static Coordinator operator +(Coordinator sync, IKinectSource source)
        {
            sync.Add(source);

            return sync;
        }

        public static Coordinator operator -(Coordinator sync, IKinectSource source)
        {
            sync.Remove(source);

            return sync;
        }

        public class CoordinatorConfig
        {
            public int InputBufferSize { get; set; } = 2;

            public int SourceTimeout { get; set; } = 500;
        }

        private class SourceConfig
        {
            internal bool main;

            internal IDictionary<ulong, VirtualBuddy> buddyMapping;

            internal FixedSizedQueue<KinectFrameData> inputQueue;

            internal SourceConfig(bool main, int bufferSize)
            {
                this.main = main;

                buddyMapping = new Dictionary<ulong, VirtualBuddy>();

                Clear(bufferSize);
            }

            internal void Clear(int bufferSize)
            {
                buddyMapping.Clear();

                inputQueue = new FixedSizedQueue<KinectFrameData>(bufferSize);
            }
        }

        private class FrameWorker : FrameWorker<Coordinator>
        {
            const double MATCHING_THRESHOLD = 0.5;

            const int FRAME_DROP = 0;
            int framesDropped = 0;

            internal FrameWorker(Coordinator parent) : base(parent)
            {

            }

            internal override void Start()
            {
                parent.ResetQueues();

                base.Start();
            }

            protected override void ProcessData()
            {
                FrameData frame = new FrameData();

                IKinectSource[] sources = parent.AvailableSources;

                if (sources.Length == 0)
                    Thread.Sleep(parent.Config.SourceTimeout);

                foreach (IKinectSource source in sources)
                {
                    KinectFrameData data = parent.DequeueFrame(source);

                    parent.Synchronizer.Sync(source, data);

                    foreach (Buddy tBuddy in data.buddies)
                    {
                        VirtualBuddy vBuddy = SearchBuddy(source, frame, tBuddy);

                        frame.Insert(vBuddy, tBuddy);
                    }

                    parent.Synchronizer.UpdateCamera(source, data);
                }

                if (framesDropped++ == FRAME_DROP)
                {
                    CommitFrame(frame);
                    framesDropped = 0;
                }
            }

            private VirtualBuddy SearchBuddy(IKinectSource source, FrameData frame, Buddy tBuddy)
            {
                VirtualBuddy vBuddy;

                SourceConfig config = parent.sources[source];
                if (!config.buddyMapping.TryGetValue(tBuddy.ID, out vBuddy))
                {
                    vBuddy = FindMatchingBuddy(parent.buddies, tBuddy);

                    if (vBuddy == null)
                        vBuddy = FindMatchingBuddy(frame.Buddies, tBuddy);
                    if (vBuddy == null)
                        vBuddy = new VirtualBuddy(tBuddy.Position, parent.nextID++);

                    config.buddyMapping[tBuddy.ID] = vBuddy;
                }

                return vBuddy;
            }

            private VirtualBuddy FindMatchingBuddy(ICollection<VirtualBuddy> buddies, Buddy buddy)
            {
                foreach (VirtualBuddy v in buddies)
                {
                    double d = Maths.Distance(v.position, buddy.Position);

                    if (d <= MATCHING_THRESHOLD)
                    {
                        return v;
                    }
                }

                return null;
            }

            private void CommitFrame(FrameData frame)
            {
                frame.UpdateAndTransform(parent.Calibrator);

                // (re-)move buddies
                foreach (VirtualBuddy vBuddy in parent.buddies.ToArray())
                {
                    if (!frame.Buddies.Contains(vBuddy))
                    {
                        parent.RemoveVirtualBuddy(vBuddy);
                    }
                    else
                    {
                        parent.VirtualBuddyMoved?.Invoke(vBuddy);
                    }
                }

                // add new buddies
                foreach (VirtualBuddy vBuddy in frame.Buddies)
                {
                    if (!parent.buddies.Contains(vBuddy))
                    {
                        parent.AddVirtualBuddy(vBuddy);
                    }
                }

                parent.UpdateScene();
            }

            private class FrameData
            {
                internal Dictionary<VirtualBuddy, IList<Buddy>> tracked = new Dictionary<VirtualBuddy, IList<Buddy>>();

                internal ICollection<VirtualBuddy> Buddies
                {
                    get
                    {
                        return tracked.Keys;
                    }
                }

                internal void Insert(VirtualBuddy vBuddy, Buddy tBuddy)
                {
                    IList<Buddy> trackings;
                    if (!tracked.TryGetValue(vBuddy, out trackings))
                        tracked[vBuddy] = trackings = new List<Buddy>();
                    trackings.Add(tBuddy);
                }

                internal void UpdateAndTransform(Calibrator calibrator)
                {
                    foreach (VirtualBuddy buddy in Buddies)
                    {
                        Update(buddy);

                        buddy.calibratedPosition = calibrator.Transform(buddy.position);
                    }
                }

                private void Update(VirtualBuddy vBuddy)
                {
                    IList<Buddy> buddies = tracked[vBuddy];

                    HandState handLeftState = HandState.Unknown,
                              handRightState = HandState.Unknown;

                    double addX = 0.0, addY = 0.0;
                    foreach (Buddy tBuddy in buddies)
                    {
                        addX += tBuddy.Position.X;
                        addY += tBuddy.Position.Y;

                        if (tBuddy.HandLeftState > handLeftState)
                            handLeftState = tBuddy.HandLeftState;
                        if (tBuddy.HandRightState > handRightState)
                            handRightState = tBuddy.HandRightState;
                    }

                    // Position
                    Point oldPosition = vBuddy.position;
                    vBuddy.position.X = addX / (double)buddies.Count;
                    vBuddy.position.Y = addY / (double)buddies.Count;

                    // Hände
                    vBuddy.HandLeftState = handLeftState;
                    vBuddy.HandRightState = handRightState;

                    // Speed & Fokus
                    Vector victor = vBuddy.position - oldPosition;

                    long mili = vBuddy.watch.ElapsedMilliseconds;
                    vBuddy.Speed = victor.Length / (mili / 1000.0);
                    vBuddy.watch.Restart();

                    if (vBuddy.Speed > 0 && vBuddy.Speed < double.PositiveInfinity)
                    {
                        victor.Normalize();

                        Vector diff = victor - vBuddy.FocusVector;
                        diff = diff / 15.0 * vBuddy.Speed;
                        vBuddy.FocusVector += diff;
                    }
                }
            }
        }
    }
}