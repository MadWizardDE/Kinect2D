using MadWizard.Kinect2D.Discovery;
using MadWizard.Kinect2D.Processing;
using MadWizard.Kinect2D.Tools;
using MadWizard.Kinect2D.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (TestBench.Test())
                return;

            Test();

            Application.Run();
        }

        static void Test()
        {
            SourceDetector detector = new SourceDetector();
            Coordinator coordinator = new Coordinator();

            Controller controller = new Controller(detector, coordinator);
            controller.ShowConfig();
            //controller.Detector.IsDiscovering = true;
            controller.ExitOnClose = true;
        }
    }
}