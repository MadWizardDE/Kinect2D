using MadWizard.Kinect2D.Discovery;
using MadWizard.Kinect2D.Processing;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace MadWizard.Kinect2D.UI
{
    internal enum PreviewType
    {
        Scene,
        Rectangle
    }

    public partial class ConfigForm : Form
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private Controller controller;

        private PreviewPainter previewPainter;

        private ListViewGroup listViewSourcesGroupLocal, listViewSourcesGroupRemote;
        private ListViewGroup listViewSyncSourcesGroupReady, listViewSyncSourcesGroupWaiting;

        public ConfigForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();
            InitializeAdditionalControls();

            TewakUI();
        }

        private void InitializeAdditionalControls()
        {
            listViewSources.Groups.Clear();
            listViewSources.Groups.Add(listViewSourcesGroupLocal = new ListViewGroup("Lokal"));
            listViewSources.Groups.Add(listViewSourcesGroupRemote = new ListViewGroup("Netzwerk"));

            listViewSyncSources.Groups.Clear();
            listViewSyncSources.Groups.Add(listViewSyncSourcesGroupReady = new ListViewGroup("Synchronisiert"));
            listViewSyncSources.Groups.Add(listViewSyncSourcesGroupWaiting = new ListViewGroup("Nicht Synchronisiert"));

            previewPainter = new PreviewPainter();
            panelPreview.BackColor = Color.Transparent;
            panelPreview.Paint += previewPainter.PaintScene;
        }

        private void TewakUI()
        {
            listViewSources.ContextMenu = contextMenuSources;

            SetWindowTheme(listViewSources.Handle, "Explorer", null);
            SetWindowTheme(listViewSyncSources.Handle, "Explorer", null);

            labelCalibrationImpossible.Image = new Icon(SystemIcons.Warning, 32, 32).ToBitmap();

            EnableDoubleBuffering(tabControl);
            EnableDoubleBuffering(panelPreview);

            //listView3.ColumnWidthChanging += (e, sender) =>
            //{
            //    ColumnWidthChangingEventArgs arg = (ColumnWidthChangingEventArgs)sender;
            //    arg.Cancel = true;
            //    arg.NewWidth = listView3.Columns[arg.ColumnIndex].Width;
            //};
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (controller.ExitOnClose)
            {
                foreach (IKinectSource source in controller.Detector.AvailableSources.ToArray())
                    if (source.SourceType == SourceType.Remote)
                        controller.Detector.Disconnect(source);

                controller.Coordinator.Synchronizer.StopSynchronizing();

                Application.Exit();
            }
        }

        #region Events

        #region SourcesTab

        private void contextMenuSources_Popup(object sender, EventArgs e)
        {
            SelectedListViewItemCollection selectedItems = listViewSources.SelectedItems;

            if (selectedItems.Count > 0)
            {
                menuItemConnect.Visible = false;
                menuItemDisconnect.Visible = true;

                menuItemDisconnect.Enabled = true;
                foreach (ListViewItem item in selectedItems)
                    if ((item.Tag as IKinectSource)?.SourceType == SourceType.Local)
                        menuItemDisconnect.Enabled = false;
            }
            else
            {
                menuItemConnect.Visible = true;
                menuItemDisconnect.Visible = false;
            }
        }

        private void menuItemConnect_Click(object sender, EventArgs args)
        {
            IPEndPoint ip = ConnectServerBox.show(12345);

            if (ip != null)
            {
                bool retry = true;
                while (retry)
                    try
                    {
                        controller.Detector.Connect(ip);

                        retry = false;
                    }
                    catch (SocketException)
                    {
                        DialogResult result = MessageBox.Show("Keine Verbindung zu '" + ip + "' möglich.", "Netzwerk",
                             MessageBoxButtons.RetryCancel,
                             MessageBoxIcon.Error,
                             MessageBoxDefaultButton.Button1);

                        retry = result == DialogResult.Retry;
                    }
                    catch (NotAvailableException e)
                    {
                        DialogResult result = MessageBox.Show("Kamera nicht angeschlossen.", "Netzwerk",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning,
                             MessageBoxDefaultButton.Button1);

                        retry = false;
                    }
                    catch (AlreadyConnectedException e)
                    {
                        DialogResult result = MessageBox.Show("Kamera mit id='" + e.KinectSource.ID + "' bereits verbunden.", "Netzwerk",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning,
                             MessageBoxDefaultButton.Button1);

                        retry = false;
                    }
            }
        }

        private void menuItemDisconnect_Click(object sender, EventArgs e)
        {
            SelectedListViewItemCollection selectedItems = listViewSources.SelectedItems;

            foreach (ListViewItem item in selectedItems)
                controller.Detector.Disconnect(item.Tag as IKinectSource);
        }

        private void contextMenuDiscovery_Click(object sender, EventArgs e)
        {
            controller.Detector.IsDiscovering ^= true;
        }

        private void listViewSources_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            IKinectSource source = listViewSources.Items[e.Index].Tag as IKinectSource;

            if (source != null)
                if (e.NewValue == CheckState.Checked)
                    controller.AddSource(source);
                else
                    controller.RemoveSource(source);
        }

        #endregion

        #region SyncTab

        private void buttonStartSync_Click(object sender, EventArgs e)
        {
            controller.Coordinator.Synchronizer.StartSynchronizing();
        }

        private void buttonStopSync_Click(object sender, EventArgs e)
        {
            controller.Coordinator.Synchronizer.StopSynchronizing();
        }

        #endregion

        #region CalibrationTab

        private void checkBoxAnchor_Click(object sender, EventArgs e)
        {
            string a = (sender as CheckBox)?.Tag as string;

            if (a != null)
            {
                char anchor = a[0];

                if (controller.CalibrationAnchor != anchor)
                    controller.CalibrationAnchor = anchor;
                else
                    controller.CalibrateNow();
            }
        }

        private void buttonStartCalibration_Click(object sender, EventArgs e)
        {
            if (controller.Coordinator.Calibrator.State != CalibrationState.Calibrating)
                controller.Coordinator.Calibrator.StartCalibrating();
            else
                controller.Coordinator.Calibrator.StopCalibrating();
        }

        private void buttonCancelCalibration_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Auf letzte Einstellung zurücksetzen?",
                                                  "Kalibrieren abbrechen",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning,
                                                  MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                controller.Coordinator.Calibrator.Config = (CalibrationConfig)controller.LastConfig;
            }
        }

        private void menuItemCalibrationReset_Click(object sender, EventArgs e)
        {
            controller.Coordinator.Calibrator.Config = CalibrationConfig.Default;
        }

        private void contextMenuAnchor_Popup(object sender, EventArgs e)
        {
            string a = ((sender as ContextMenu).SourceControl as CheckBox)?.Tag as string;

            if (a != null)
            {
                char anchor = a[0];

                menuItemAnchorName.Text = "Ankerpunkt " + anchor;

                menuItemCalibrate.Visible = controller.CalibrationAnchor == anchor;
                menuItemCalibrate.Enabled = controller.CalibrationBuddy != null;
                menuItemCalibrate.DefaultItem = menuItemCalibrate.Visible;

                menuItemSetPosition.DefaultItem = !menuItemCalibrate.DefaultItem;

                menuItemResetPosition.Enabled = controller.Coordinator.Calibrator[anchor] != null;
            }
        }

        private void menuItemCalibrate_Click(object sender, EventArgs e)
        {
            controller.CalibrateNow();
        }

        private void menuItemSetPosition_Click(object sender, EventArgs e)
        {
            string a = (contextMenuAnchor.SourceControl as CheckBox)?.Tag as string;

            if (a != null)
            {
                char anchor = a[0];

                System.Windows.Point? position = controller.Coordinator.Calibrator[anchor];

                position = InputPositionBox.show(position);

                controller.Coordinator.Calibrator[anchor] = position;
            }
        }

        private void menuItemResetPosition_Click(object sender, EventArgs e)
        {
            string a = (contextMenuAnchor.SourceControl as CheckBox)?.Tag as string;

            if (a != null)
            {
                char anchor = a[0];

                controller.Coordinator.Calibrator[anchor] = null;
            }
        }

        #endregion

        #region PreviewTab

        private void radioButtonPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == radioButtonPreviewScene)
                if (radioButtonPreviewScene.Checked)
                    previewPainter.type = PreviewType.Scene;
            if (sender == radioButtonPreviewRectangle)
                if (radioButtonPreviewRectangle.Checked)
                    previewPainter.type = PreviewType.Rectangle;
        }

        #endregion

        #endregion

        #region Refresh

        internal void RefreshData()
        {
            RefreshStatus();
            RefreshSources();
            RefreshSyncSources();
            RefreshCalibration();
            RefreshPreview();
        }

        internal void RefreshStatus()
        {
            if (controller.Coordinator.Sources.Length == 0)
            {
                statusLabel.Text = "Nicht verbunden";
                statusProgressBar.Visible = false;
            }
            else
            {
                SyncState syncState = controller.Coordinator.Synchronizer.State;

                if (syncState == SyncState.NotAvailable)
                {
                    statusLabel.Text = "Nicht synchronisiert";
                    statusProgressBar.Visible = false;
                }
                else if (syncState == SyncState.Synchronizing)
                {
                    statusLabel.Text = "Synchronisiere...";
                    statusProgressBar.Style = ProgressBarStyle.Marquee;
                }
                else if (controller.Coordinator.Synchronizer.State == SyncState.Ready)
                {
                    CalibrationState caliState = controller.Coordinator.Calibrator.State;

                    if (caliState == CalibrationState.Uncalibrated)
                    {
                        statusLabel.Text = "Nicht kalibriert";
                        statusProgressBar.Visible = false;
                    }
                    else if (caliState == CalibrationState.Calibrating)
                    {
                        statusLabel.Text = "Kalibrieren...";
                        statusProgressBar.Style = ProgressBarStyle.Marquee;
                        statusProgressBar.Visible = true;
                    }
                    else if (caliState == CalibrationState.Calibrated)
                    {
                        statusLabel.Text = "Bereit";
                        statusProgressBar.Visible = false;
                    }
                }
            }
        }

        internal void RefreshSources()
        {
            SyncState syncState = controller.Coordinator.Synchronizer.State;

            listViewSources.Items.Clear();
            foreach (IKinectSource source in controller.Detector.AvailableSources)
            {
                String connectionName;
                switch (source.ConnectionType)
                {
                    case ConnectionType.USB:
                        connectionName = "USB"; break;
                    case ConnectionType.TCPIP:
                        connectionName = "TCP/IP"; break;
                    default:
                        connectionName = "Unbekannt"; break;
                }

                String availableName = source.IsAvailable ? "Ja" : "Nein";

                ListViewItem item =
                    new ListViewItem(new string[] { source.Name, connectionName, availableName });

                item.ForeColor = SystemColors.WindowText;
                item.BackColor = Color.Empty;

                switch (source.SourceType)
                {
                    case SourceType.Local:
                        item.Group = listViewSourcesGroupLocal; break;
                    case SourceType.Remote:
                        item.Group = listViewSourcesGroupRemote; break;
                    default:
                        item.Group = null; break;
                }

                item.Checked = controller.Coordinator.Sources.Contains(source);
                item.Tag = source;

                listViewSources.Items.Add(item);
            }
            listViewSources.Enabled = syncState == SyncState.NotAvailable;

            menuItemDiscovery.Checked = controller.Detector.IsDiscovering;
        }

        internal void RefreshSyncSources()
        {
            SyncState state = controller.Coordinator.Synchronizer.State;
            CalibrationState caliState = controller.Coordinator.Calibrator.State;

            IKinectSource[] sources = controller.Coordinator.Sources;

            comboBoxMainSource.Items.Clear();
            comboBoxMainSource.Items.AddRange(sources);
            comboBoxMainSource.SelectedItem = controller.Coordinator.MainSource;
            comboBoxMainSource.Enabled = sources.Length > 0 && state == SyncState.NotAvailable;

            listViewSyncSources.Items.Clear();
            foreach (IKinectSource source in sources)
            {
                SyncState sourceState = controller.Coordinator.Synchronizer[source];

                String statusName;
                if (source.IsAvailable)
                {
                    statusName = "Bereit";
                }
                else
                    statusName = "Nicht verfügbar";

                ListViewItem item =
                new ListViewItem(new string[] { source.Name, statusName });

                item.Group = sourceState == SyncState.Ready
                    ? listViewSyncSourcesGroupReady
                    : listViewSyncSourcesGroupWaiting;

                // TODO weitermachen!
                item.Tag = source;

                listViewSyncSources.Items.Add(item);
            }

            buttonStartSync.Enabled = state == SyncState.NotAvailable && controller.Coordinator.AvailableSources.Length > 0;
            buttonStopSync.Enabled = state != SyncState.NotAvailable && caliState != CalibrationState.Calibrating;
        }

        internal void RefreshCalibration()
        {
            bool ready = controller.Coordinator.Synchronizer.State == SyncState.Ready;

            CalibrationState state = controller.Coordinator.Calibrator.State;
            CalibrationConfig caliConfig = controller.Coordinator.Calibrator.Config;

            groupBoxAnchors.Enabled = ready;
            {
                Action<CheckBox> RefreshAnchor = checkBoxAnchor =>
                {
                    string a = checkBoxAnchor.Tag as string;

                    if (a != null)
                    {
                        char anchor = a[0];

                        System.Windows.Point? position = controller.Coordinator.Calibrator[anchor];

                        if (controller.CalibrationAnchor == anchor)
                        {
                            checkBoxAnchor.CheckState = CheckState.Indeterminate;
                            checkBoxAnchor.Font = new System.Drawing.Font(checkBoxAnchor.Font, FontStyle.Bold);
                        }
                        else if (state == CalibrationState.Uncalibrated || !position.HasValue)
                        {
                            checkBoxAnchor.CheckState = CheckState.Unchecked;
                            checkBoxAnchor.Font = new System.Drawing.Font(checkBoxAnchor.Font, FontStyle.Regular);
                        }
                        else
                        {
                            checkBoxAnchor.CheckState = CheckState.Checked;
                            checkBoxAnchor.Font = new System.Drawing.Font(checkBoxAnchor.Font, FontStyle.Regular);
                        }

                        if (controller.CalibrationAnchor == anchor)
                        {
                            position = controller.CalibrationBuddy?.Position;
                        }

                        if (position.HasValue)
                        {
                            string tooltip = String.Format("(X = {0:0.00}, Y = {1:0.00})", position.Value.X, position.Value.Y);

                            toolTipAnchorInfo.SetToolTip(checkBoxAnchor, tooltip);
                        }
                        else
                            toolTipAnchorInfo.SetToolTip(checkBoxAnchor, null);

                        if (state == CalibrationState.Calibrating)
                            checkBoxAnchor.ContextMenu = contextMenuAnchor;
                        else
                            checkBoxAnchor.ContextMenu = null;
                    }
                };

                RefreshAnchor(checkBoxAnchorA);
                RefreshAnchor(checkBoxAnchorB);
                RefreshAnchor(checkBoxAnchorC);
            }

            labelCalibrationImpossible.Visible =
                state == CalibrationState.Calibrating
                && controller.CalibrationBuddy == null;

            buttonStartCalibration.Enabled =
                   state != CalibrationState.Calibrating && ready
                || state == CalibrationState.Calibrating && caliConfig.IsComplete;

            if (state == CalibrationState.Calibrating && caliConfig.IsComplete)
                buttonStartCalibration.Text = "OK";
            else if (state == CalibrationState.Uncalibrated)
                buttonStartCalibration.Text = "Starten";
            else if (state == CalibrationState.Calibrated)
                buttonStartCalibration.Text = "Bearbeiten";

            buttonStopCalibration.Enabled = state == CalibrationState.Calibrating;

            groupBoxAnchors.ContextMenu = contextMenuCalibration;
        }

        internal void RefreshPreview()
        {
            groupBoxPreviewOutput.Enabled = controller.Coordinator.Synchronizer.State == SyncState.Ready;

            radioButtonPreviewScene.Checked = previewPainter.type == PreviewType.Scene;
            radioButtonPreviewRectangle.Checked = previewPainter.type == PreviewType.Rectangle;
            radioButtonPreviewRectangle.Enabled = controller.Coordinator.Calibrator.State != CalibrationState.Calibrating;
        }

        #endregion

        #region Drawing

        internal void DrawPreview(Scene scene)
        {
            if (panelPreview.Visible)
            {
                previewPainter.scene = scene;

                panelPreview.Refresh();
            }
        }

        #endregion

    }
}
