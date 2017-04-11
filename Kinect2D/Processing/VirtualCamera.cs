using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MadWizard.Kinect2D.Processing
{
    public class VirtualCamera : VirtualObject
    {
        private IKinectSource source;

        private Matrix matrix;

        internal VirtualCamera(IKinectSource source, Point position, double rotation, double accuracy) : base(position)
        {
            this.source = source;

            Accuracy = accuracy;
            Rotation = rotation;

            matrix = new Matrix();
            matrix.RotateAt(rotation, position.X, position.Y);
        }

        string ID
        {
            get
            {
                return source.ID;
            }
        }

        public double Accuracy { get; internal set; }
        public double Rotation { get; internal set; }

        public Point? LeftEdge { get; internal set; }
        public Point? RightEdge { get; internal set; }

        internal Point Transform(Point p)
        {
            return matrix.Transform(position + (Vector)p);
        }

        internal static VirtualCamera CreateDefault(IKinectSource source)
        {
            Point origin = new Point(0.0, 0.0);

            return new VirtualCamera(source, origin, 0.0, 0.0);
        }
    }
}
