using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MadWizard.Kinect2D.Processing
{
    public abstract class VirtualObject
    {
        internal Point position;

        protected VirtualObject(Point position)
        {
            this.position = position;
        }

        public Point Position
        {
            get
            {
                return position;
            }
        }
    }
}
