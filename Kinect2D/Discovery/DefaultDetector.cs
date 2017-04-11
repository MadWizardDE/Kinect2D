using MadWizard.Kinect2D.Sources;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Kinect2D.Discovery
{
    public partial class SourceDetector
    {
        private DefaultKinect defaultKinect;

        private void DefaultSensor_Init()
        {
            // DefaultSensor einbinden
            KinectSensor sensor = KinectSensor.GetDefault();

            defaultKinect = new DefaultKinect(sensor);

            sources.Add(defaultKinect);
        }
    }
}
