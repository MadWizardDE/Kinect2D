using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Processing
{
    public struct Scene
    {
        public CaptureArea? CaptureArea;

        public VirtualCamera[] Cameras;

        public VirtualBuddy[] Buddies;
    }
}
