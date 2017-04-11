using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MadWizard.Kinect2D.UI
{
    partial class InputPositionBox : Form
    {
        private Point? position;

        private InputPositionBox(Point? position)
        {
            InitializeComponent();

            if ((this.position = position).HasValue)
            {
                textBoxX.Text = String.Format("{0:0.00}", position.Value.X);
                textBoxY.Text = String.Format("{0:0.00}", position.Value.Y);
            }
        }

        public static Point? show(Point? position)
        {
            InputPositionBox box = new InputPositionBox(position);

            DialogResult result = box.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (String.IsNullOrWhiteSpace(box.textBoxX.Text) || String.IsNullOrWhiteSpace(box.textBoxY.Text))
                    return null;

                Point point = new Point();
                point.X = Convert.ToDouble(box.textBoxX.Text);
                point.Y = Convert.ToDouble(box.textBoxY.Text);
                return point;
            }
            else
            {
                return position;
            }
        }
    }
}