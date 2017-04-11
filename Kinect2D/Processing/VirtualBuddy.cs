using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MadWizard.Kinect2D.Processing
{
    public delegate void VirtualBuddyCallback(VirtualBuddy buddy);

    public class VirtualBuddy : VirtualObject
    {
        internal Point? calibratedPosition;

        internal Stopwatch watch = new Stopwatch();

        internal VirtualBuddy(Point position, ulong id) : base(position)
        {
            ID = id;
        }

        public ulong ID { get; internal set; }

        public Point? CalibratedPosition
        {
            get
            {
                return calibratedPosition;
            }
        }

        public Vector FocusVector { get; internal set; }

        public double Speed { get; internal set; }

        public HandState HandLeftState { get; internal set; }
        public HandState HandRightState { get; internal set; }
    }
}
