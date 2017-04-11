using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace MadWizard.Kinect2D.Processing
{
    public enum CalibrationState
    {
        Uncalibrated = 0,
        Calibrating,
        Calibrated
    }

    public class CalibrationStateChangedEventArgs : EventArgs
    {
        public CalibrationState state;
    }

    public class CalibrationConfigChangedEventArgs : EventArgs
    {
        public CalibrationConfig config;

        public char? anchor;
    }

    public struct CalibrationConfig
    {
        public const double MAX_RANGE = 4.5;

        public Point? A, B, C;

        public bool IsComplete
        {
            get
            {
                return A.HasValue && B.HasValue && C.HasValue;
            }
        }

        public override bool Equals(object that)
        {
            return that is CalibrationConfig ? this == (CalibrationConfig)that : false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(A, B, C).GetHashCode();
        }

        public static bool operator ==(CalibrationConfig c1, CalibrationConfig c2)
        {
            return c1.A == c2.A && c1.B == c2.B && c1.C == c2.C;
        }

        public static bool operator !=(CalibrationConfig c1, CalibrationConfig c2)
        {
            return !(c1 == c2);
        }

        public static CalibrationConfig Default
        {
            get
            {
                CalibrationConfig config;
                config.A = new Point { X = -MAX_RANGE / 2.0, Y = 0.0 };
                config.B = new Point { X = -MAX_RANGE / 2.0, Y = MAX_RANGE };
                config.C = new Point { X = MAX_RANGE / 2.0, Y = 0.0 };
                return config;
            }
        }
    }

    public struct CaptureArea
    {
        public Point A, B, C, D;

        public IEnumerable<Point> Points
        {
            get
            {
                yield return A;
                yield return B;
                yield return C;
                yield return D;
            }
        }

        internal CaptureArea(CalibrationConfig config, Point d)
        {
            if (!config.IsComplete)
                throw new ArgumentException("config incomplete");

            A = (Point)config.A;
            B = (Point)config.B;
            C = d;
            D = (Point)config.C;
        }
    }

    public class Calibrator
    {
        private CalibrationConfig config;

        private Matrix matrix;

        internal Calibrator()
        {
            Config = CalibrationConfig.Default;
        }

        internal Calibrator(CalibrationConfig config)
        {
            Config = config;
        }

        public Point? this[char anchor]
        {
            get
            {
                switch (anchor)
                {
                    case 'A':
                        return config.A;
                    case 'B':
                        return config.B;
                    case 'C':
                        return config.C;
                    default:
                        throw new ArgumentException("unknown anchor");
                }
            }

            set
            {
                if (State != CalibrationState.Calibrating)
                    throw new InvalidOperationException();

                switch (anchor)
                {
                    case 'A':
                        config.A = value; break;
                    case 'B':
                        config.B = value; break;
                    case 'C':
                        config.C = value; break;
                    default:
                        throw new ArgumentException("unknown anchor");
                }

                onConfigChange(anchor);
            }
        }

        public CalibrationState State { get; private set; }

        public CalibrationConfig Config
        {
            get
            {
                return config;
            }

            set
            {
                if (!value.IsComplete)
                    throw new ArgumentException("config incomplete");

                matrix = BuildMatrix(config = value);

                onConfigChange(null);

                if (value == CalibrationConfig.Default)
                    onStateChange(State = CalibrationState.Uncalibrated);
                else
                    onStateChange(State = CalibrationState.Calibrated);
            }
        }

        public CaptureArea? CaptureArea
        {
            get
            {
                if (!config.IsComplete)
                    return null;

                Matrix inversion = BuildMatrix(config);
                inversion.Invert();

                return new CaptureArea(config, inversion.Transform(new Point(1.0, 1.0)));
            }
        }

        public event EventHandler<CalibrationStateChangedEventArgs> CalbrationStateChanged;
        public event EventHandler<CalibrationConfigChangedEventArgs> CalibrationConfigChanged;

        public void StartCalibrating()
        {
            onStateChange(State = CalibrationState.Calibrating);
        }

        public void StopCalibrating()
        {
            Config = Config;
        }

        internal Point? Transform(Point point)
        {
            if (State == CalibrationState.Calibrating)
                return null;

            return matrix.Transform(point);
        }

        protected void onStateChange(CalibrationState state)
        {
            Debug.WriteLine(String.Format("state = {0}", state.ToString()), "Calibrator");

            CalbrationStateChanged?.Invoke(this, new CalibrationStateChangedEventArgs { state = State = state });
        }

        protected void onConfigChange(char? anchor)
        {
            CalibrationConfigChanged?.Invoke(this, new CalibrationConfigChangedEventArgs { config = Config, anchor = anchor });
        }

        private static Matrix BuildMatrix(CalibrationConfig config)
        {
            Point A = (Point)config.A, B = (Point)config.B, C = (Point)config.C;

            Matrix matrix = new Matrix();
            {
                // Verschiebung
                matrix.Translate(-A.X, -A.Y);

                // Drehung
                Point c1 = matrix.Transform(C);
                Point c2 = new Point(c1.X, 0.0);
                matrix.Rotate(Vector.AngleBetween((Vector)c1, (Vector)c2));

                // Scherung
                Point b1 = matrix.Transform(B);
                Point b2 = new Point(0.0, b1.Y);
                matrix.Skew(-Vector.AngleBetween((Vector)b1, (Vector)b2), 0.0);

                // Skalierung
                Point b = matrix.Transform(B);
                Point c = matrix.Transform(C);
                matrix.Scale(1.0 / c.X, 1.0 / b.Y);
            }

            return matrix;
        }
    }
}