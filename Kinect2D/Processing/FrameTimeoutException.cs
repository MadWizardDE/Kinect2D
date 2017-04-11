using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Processing
{
    [Serializable]
    internal class FrameTimeoutException : Exception
    {
        public IKinectSource KinectSource { get; private set; }

        internal FrameTimeoutException(IKinectSource source)
        {
            KinectSource = source;
        }
    }
}
