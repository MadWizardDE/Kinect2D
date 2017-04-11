using MadWizard.Kinect2D.Discovery;
using MadWizard.Kinect2D.Processing;
using Microsoft.Kinect;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace MadWizard.Kinect2D.UI
{
    public class Controller
    {
        private ConfigForm configForm;

        private char? calibrationAnchor;
        private VirtualBuddy calibrationBuddy;
        private bool calibrationTriggerRemote;

        public Controller() : this(new SourceDetector(), new Coordinator())
        {

        }

        public Controller(SourceDetector detector, Coordinator coordinator)
        {
            Detector = detector;
            Detector.PropertyChanged += SourceDetector_PropertyChanged;
            Detector.SourceDetected += SourceDetector_SourceDetected;
            Detector.SourceDisconnected += SourceDetector_SourceDisconnected;
            foreach (IKinectSource source in Detector.AvailableSources)
                source.IsAvailableChanged += Source_IsAvailableChanged;

            Coordinator = coordinator;
            Coordinator.SceneChanged += Coordinator_SceneChanged;
            Coordinator.Synchronizer.SyncStateChanged += Synchronizer_StateChanged;
            Coordinator.Calibrator.CalbrationStateChanged += Calibrator_StateChanged;
            Coordinator.Calibrator.CalibrationConfigChanged += Calibrator_ConfigChanged;
        }


        public SourceDetector Detector { get; }
        public Coordinator Coordinator { get; }

        public CalibrationConfig? LastConfig { get; private set; }

        public char? CalibrationAnchor
        {
            get
            {
                return calibrationAnchor;
            }

            set
            {
                if (Coordinator.Calibrator.State == CalibrationState.Calibrating)
                {
                    calibrationAnchor = value;
                }

                configForm?.RefreshCalibration();
            }
        }
        public VirtualBuddy CalibrationBuddy
        {
            get
            {
                return calibrationBuddy;
            }
        }

        public bool ExitOnClose { get; set; }

        public void ShowConfig()
        {
            if (configForm == null || !configForm.Visible)
            {
                configForm = new ConfigForm(this);
                configForm.Show();
            }
        }

        public void AddSource(IKinectSource source)
        {
            if (!Coordinator.Sources.Contains(source))
            {
                Coordinator.Add(source);

                if (Coordinator.MainSource == null)
                    Coordinator.MainSource = source;

                configForm?.RefreshData();
            }
        }

        public void RemoveSource(IKinectSource source)
        {
            if (Coordinator.Sources.Contains(source))
            {
                Coordinator.Remove(source);

                if (Coordinator.MainSource == null && Coordinator.Sources.Length > 0)
                    Coordinator.MainSource = Coordinator.Sources[0];

                configForm?.RefreshData();
            }
        }

        public void CalibrateNow()
        {
            if (calibrationAnchor != null && calibrationBuddy != null)
            {
                char anchor = (char)calibrationAnchor;

                Coordinator.Calibrator[anchor] = calibrationBuddy.Position;

                configForm?.RefreshCalibration();
            }
        }

        #region EventHandlers

        private void UpdateUI(MethodInvoker invoker)
        {
            if (configForm != null && !configForm.IsDisposed)
                configForm?.BeginInvoke(invoker);
        }

        private void SourceDetector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateUI(() => configForm.RefreshData());
        }

        private void SourceDetector_SourceDetected(object sender, SourceDetectedEventArgs e)
        {
            e.Source.IsAvailableChanged += Source_IsAvailableChanged;

            UpdateUI(() => configForm.RefreshData());
        }

        private void SourceDetector_SourceDisconnected(object sender, SourceDisconnectedEventArgs e)
        {
            e.Source.IsAvailableChanged -= Source_IsAvailableChanged;

            if (Coordinator.Sources.Contains(e.Source))
            {
                Coordinator.Synchronizer.StopSynchronizing();

                Coordinator.Remove(e.Source);
            }

            UpdateUI(() =>
            {
                if (e.IsError)
                    if (configForm != null)
                        MessageBox.Show("Verbindung zur Quelle '" + e.Source.Name + "' verloren.", "Netzwerk",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error,
                           MessageBoxDefaultButton.Button1);

                configForm.RefreshData();
            });
        }

        private void Source_IsAvailableChanged(object sender, EventArgs e)
        {
            UpdateUI(() => configForm.RefreshData());
        }

        private void Synchronizer_StateChanged(object sender, SyncStateChangedEventArgs e)
        {
            if (e.state == SyncState.Ready)
                Coordinator.IsTracking = true;

            UpdateUI(() => configForm.RefreshData());
        }

        #region Calibratior

        private void Calibrator_StateChanged(object sender, CalibrationStateChangedEventArgs e)
        {
            if (e.state == CalibrationState.Calibrating)
            {
                LastConfig = Coordinator.Calibrator.Config;

                // reset
                if (LastConfig == CalibrationConfig.Default)
                {
                    Coordinator.Calibrator['A'] = null;
                    Coordinator.Calibrator['B'] = null;
                    Coordinator.Calibrator['C'] = null;

                    calibrationAnchor = 'A';
                }
            }
            else
            {
                calibrationAnchor = null;
                calibrationBuddy = null;
                LastConfig = null;
            }

            configForm?.RefreshData();
        }

        private void Calibrator_ConfigChanged(object sender, CalibrationConfigChangedEventArgs e)
        {
            if (Coordinator.Calibrator.State == CalibrationState.Calibrating)
            {
                if (calibrationAnchor == e.anchor)
                {
                    char[] achorPoints = { 'A', 'B', 'C' };

                    calibrationAnchor = null;
                    foreach (char anchorPoint in achorPoints)
                        if (Coordinator.Calibrator[anchorPoint] == null)
                        {
                            calibrationAnchor = anchorPoint; break;
                        }
                }
            }
        }

        #endregion

        #region Scene

        private void Coordinator_SceneChanged(object sender, SceneChangedEventArgs e)
        {
            Scene scene = e.scene;

            UpdateUI(() =>
            {
                if (Coordinator.Calibrator.State == CalibrationState.Calibrating)
                {
                    calibrationBuddy = null;
                    if (scene.Buddies.Length == 1)
                        calibrationBuddy = scene.Buddies[0];

                    if (calibrationBuddy != null)
                    {
                        if (calibrationBuddy.HandRightState == HandState.Lasso)
                        {
                            if (!calibrationTriggerRemote)
                            {
                                calibrationTriggerRemote = true;

                                CalibrateNow();
                            }
                        }
                        else if (calibrationBuddy.HandRightState == HandState.Closed)
                        {
                            calibrationTriggerRemote = false;
                        }
                    }

                    configForm?.RefreshCalibration();
                }

                configForm?.DrawPreview(scene);

            });
        }
        #endregion

        #endregion
    }
}
