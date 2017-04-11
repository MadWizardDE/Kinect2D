using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MadWizard.Kinect2D.UI
{
    partial class ConnectServerBox : Form
    {
        private ConnectServerBox(int? defaultPort)
        {
            InitializeComponent();

            //textBoxHost.Text = "127.0.0.1";
            textBoxHost.Text = "192.168.128.10";
            //textBoxHost.Text = "192.168.128.30";

            if (defaultPort != null)
                textBoxPort.Text = defaultPort.ToString();
        }

        public static IPEndPoint show(int? defaultPort)
        {
            ConnectServerBox box = new ConnectServerBox(defaultPort);

            DialogResult result = box.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (String.IsNullOrWhiteSpace(box.textBoxHost.Text) || String.IsNullOrWhiteSpace(box.textBoxPort.Text))
                    return null;

                return new IPEndPoint(IPAddress.Parse(box.textBoxHost.Text), Int32.Parse(box.textBoxPort.Text));
            }
            else
            {
                return null;
            }
        }
    }
}