using MadWizard.Kinect2D.Processing;
using MadWizard.Kinect2D.Tools;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MadWizard.Kinect2D.UI
{
    class PreviewPainter
    {
        internal Scene? scene;

        internal PreviewType type;

        Pen areaPen;
        Pen buddyRotationPen;
        Pen buddyRotationVectorPen;
        Pen fieldOfViewPen;

        Brush fieldOfViewBrush;

        internal PreviewPainter()
        {
            areaPen = new Pen(Color.Turquoise, 2.0f);

            buddyRotationPen = new Pen(Color.Pink, 2.0f);
            buddyRotationVectorPen = new Pen(Color.Purple, 2.0f);

            fieldOfViewPen = new Pen(Color.Orange, 2.0f);
            fieldOfViewBrush = new SolidBrush(Color.FromArgb(15, fieldOfViewPen.Color));
        }

        internal void PaintScene(object sender, PaintEventArgs e)
        {
            if (scene != null)
            {
                Scene scene = (Scene)this.scene;

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                switch (type)
                {
                    case PreviewType.Scene:
                        DrawScene(e.Graphics, scene); break;

                    case PreviewType.Rectangle:
                        if (scene.CaptureArea != null)
                            DrawRectangle(e.Graphics, scene);
                        break;
                }
            }
        }

        private void DrawScene(Graphics g, Scene scene)
        {
            DrawBackground(g);

            float width = g.VisibleClipBounds.Width,
                  height = g.VisibleClipBounds.Height;

            float maxRange = (float)CalibrationConfig.MAX_RANGE;

            Matrix matrix = new Matrix();
            matrix.Translate(width / 2.0f, height);
            matrix.Scale(width / maxRange, -height / maxRange);
            // TODO echten bereich ermiteln

            if (scene.CaptureArea != null)
                DrawCaptureArea(g, matrix, scene.CaptureArea.Value);
            foreach (VirtualCamera camera in scene.Cameras)
                DrawCamera(g, matrix, camera);
            foreach (VirtualBuddy buddy in scene.Buddies)
                DrawBuddy(g, matrix, buddy, false);
        }

        private void DrawRectangle(Graphics g, Scene scene)
        {
            DrawBackground(g);

            float width = g.VisibleClipBounds.Width,
                  height = g.VisibleClipBounds.Height;

            Matrix matrix = new Matrix();
            matrix.Translate(0.0f, height);
            matrix.Scale(width, -height);

            foreach (VirtualBuddy buddy in scene.Buddies)
                DrawBuddy(g, matrix, buddy, true);
        }

        private void DrawBackground(Graphics g)
        {
            g.FillRectangle(Brushes.Black, g.VisibleClipBounds);
        }

        private void DrawCaptureArea(Graphics g, Matrix m, CaptureArea area)
        {
            System.Drawing.PointF[] pts = new System.Drawing.PointF[5];

            int i = 0;
            foreach (System.Windows.Point p in area.Points)
                pts[i++] = p.ToPointF();
            pts[i] = area.A.ToPointF();

            m.TransformPoints(pts);

            g.DrawLines(areaPen, pts);
        }

        private void DrawCamera(Graphics g, Matrix m, VirtualCamera camera)
        {
            DrawFieldOfView(g, m, camera);

            System.Drawing.PointF[] pts = { camera.Position.ToPointF() };
            m.TransformPoints(pts);
            g.FillRectangle(Brushes.Yellow, CenterRectangle(pts[0], new SizeF(15, 15)));
        }

        private void DrawFieldOfView(Graphics g, Matrix m, VirtualCamera camera)
        {
            if (camera.LeftEdge != null && camera.RightEdge != null)
            {
                var left = camera.LeftEdge.Value;
                var right = camera.RightEdge.Value;
                var leftMax = CalcMaxRangeOfView(camera, (Vector)left);
                var rightMax = CalcMaxRangeOfView(camera, (Vector)right);

                System.Drawing.PointF[] pts = { camera.Position.ToPointF(), leftMax.ToPointF(), rightMax.ToPointF() };
                m.TransformPoints(pts);
                g.FillPolygon(fieldOfViewBrush, pts);

                DrawFieldOfViewEdge(g, m, camera, left);
                DrawFieldOfViewEdge(g, m, camera, right);
            }
        }

        private void DrawFieldOfViewEdge(Graphics g, Matrix m, VirtualCamera camera, System.Windows.Point edge)
        {
            var p = camera.Position;

            System.Drawing.PointF[] pts = { p.ToPointF(), edge.ToPointF() };
            m.TransformPoints(pts);
            g.DrawLine(fieldOfViewPen, pts[0], pts[1]);
        }

        private System.Windows.Point CalcMaxRangeOfView(VirtualCamera camera, Vector edge)
        {
            edge -= (Vector)camera.Position;

            double length = edge.Length;

            double a = Math.Sqrt(Math.Pow(8, 2) / (Math.Pow(edge.X, 2) + Math.Pow(edge.Y, 2)));

            edge = edge * a;

            edge += (Vector)camera.Position;

            length = edge.Length;

            return (System.Windows.Point)edge;
        }

        private void DrawBuddy(Graphics g, Matrix m, VirtualBuddy buddy, bool relative)
        {
            var position = relative ? buddy.CalibratedPosition : buddy.Position;

            if (position != null)
            {
                var p = position.Value;

                System.Drawing.PointF[] pts = { p.ToPointF() };
                m.TransformPoints(pts);

                Brush brush;
                switch (buddy.HandRightState)
                {
                    case HandState.Closed:
                        brush = Brushes.Red; break;
                    case HandState.Open:
                        brush = Brushes.Green; break;
                    case HandState.Lasso:
                        brush = Brushes.Blue; break;

                    default:
                        brush = Brushes.Gray; break;
                }

                g.FillEllipse(brush, CenterRectangle(pts[0], new SizeF(15, 15)));

                {
                    System.Drawing.PointF[] pts2 = new System.Drawing.PointF[] { p.ToPointF(), (p + buddy.FocusVector).ToPointF() };
                    m.TransformPoints(pts2);
                    g.DrawLine(buddyRotationVectorPen, pts2[0], pts2[1]);
                }
            }
        }

        private RectangleF CenterRectangle(System.Drawing.PointF location, SizeF size)
        {
            location.X -= size.Width / 2.0f;
            location.Y -= size.Height / 2.0f;

            return new RectangleF(location, size);
        }
    }
}
