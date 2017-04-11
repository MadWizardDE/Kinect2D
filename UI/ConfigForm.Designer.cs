namespace MadWizard.Kinect2D.UI
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuItem menuItemSeperator;
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Lokal", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Netzwerk", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Default Kinect",
            "USB",
            "Ja"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "A",
            "TCP/IP",
            "Ja"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "B",
            "TCP/IP",
            "Nein"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Default Kinect",
            "Bereit"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.Color.Empty, null);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "A",
            "Nicht verfügbar",
            "Ja"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "B",
            "Warte"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSource = new System.Windows.Forms.TabPage();
            this.listViewSources = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTransport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAvailable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageSync = new System.Windows.Forms.TabPage();
            this.buttonStartSync = new System.Windows.Forms.Button();
            this.buttonStopSync = new System.Windows.Forms.Button();
            this.listViewSyncSources = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelMainSource = new System.Windows.Forms.Label();
            this.comboBoxMainSource = new System.Windows.Forms.ComboBox();
            this.tabPageCalibrate = new System.Windows.Forms.TabPage();
            this.buttonStartCalibration = new System.Windows.Forms.Button();
            this.buttonStopCalibration = new System.Windows.Forms.Button();
            this.groupBoxAnchors = new System.Windows.Forms.GroupBox();
            this.labelCalibrationImpossible = new System.Windows.Forms.Label();
            this.checkBoxAnchorA = new System.Windows.Forms.CheckBox();
            this.checkBoxAnchorC = new System.Windows.Forms.CheckBox();
            this.checkBoxAnchorB = new System.Windows.Forms.CheckBox();
            this.tabPagePreview = new System.Windows.Forms.TabPage();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.groupBoxPreviewOutput = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonPreviewScene = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviewRectangle = new System.Windows.Forms.RadioButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.contextMenuSources = new System.Windows.Forms.ContextMenu();
            this.menuItemConnect = new System.Windows.Forms.MenuItem();
            this.menuItemDisconnect = new System.Windows.Forms.MenuItem();
            this.menuItemDiscovery = new System.Windows.Forms.MenuItem();
            this.toolTipCalibrationImpossible = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipAnchorInfo = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuAnchor = new System.Windows.Forms.ContextMenu();
            this.menuItemAnchorName = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemCalibrate = new System.Windows.Forms.MenuItem();
            this.menuItemSetPosition = new System.Windows.Forms.MenuItem();
            this.menuItemResetPosition = new System.Windows.Forms.MenuItem();
            this.contextMenuCalibration = new System.Windows.Forms.ContextMenu();
            this.menuItemCalibrationReset = new System.Windows.Forms.MenuItem();
            menuItemSeperator = new System.Windows.Forms.MenuItem();
            this.tabControl.SuspendLayout();
            this.tabPageSource.SuspendLayout();
            this.tabPageSync.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageCalibrate.SuspendLayout();
            this.groupBoxAnchors.SuspendLayout();
            this.tabPagePreview.SuspendLayout();
            this.groupBoxPreviewOutput.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItemSeperator
            // 
            menuItemSeperator.Index = 2;
            menuItemSeperator.Text = "-";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageSource);
            this.tabControl.Controls.Add(this.tabPageSync);
            this.tabControl.Controls.Add(this.tabPageCalibrate);
            this.tabControl.Controls.Add(this.tabPagePreview);
            this.tabControl.Location = new System.Drawing.Point(7, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(302, 376);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSource
            // 
            this.tabPageSource.Controls.Add(this.listViewSources);
            this.tabPageSource.Location = new System.Drawing.Point(4, 22);
            this.tabPageSource.Name = "tabPageSource";
            this.tabPageSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSource.Size = new System.Drawing.Size(294, 350);
            this.tabPageSource.TabIndex = 0;
            this.tabPageSource.Text = "Quellen";
            this.tabPageSource.UseVisualStyleBackColor = true;
            // 
            // listViewSources
            // 
            this.listViewSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSources.CheckBoxes = true;
            this.listViewSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderTransport,
            this.columnHeaderAvailable});
            this.listViewSources.FullRowSelect = true;
            listViewGroup1.Header = "Lokal";
            listViewGroup1.Name = "listViewGroupLocal";
            listViewGroup2.Header = "Netzwerk";
            listViewGroup2.Name = "listViewGroupRemote";
            this.listViewSources.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            listViewItem1.Checked = true;
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 1;
            listViewItem2.Group = listViewGroup2;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.Group = listViewGroup2;
            listViewItem3.StateImageIndex = 0;
            this.listViewSources.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.listViewSources.LabelWrap = false;
            this.listViewSources.Location = new System.Drawing.Point(6, 6);
            this.listViewSources.Name = "listViewSources";
            this.listViewSources.Size = new System.Drawing.Size(282, 339);
            this.listViewSources.TabIndex = 0;
            this.listViewSources.UseCompatibleStateImageBehavior = false;
            this.listViewSources.View = System.Windows.Forms.View.Details;
            this.listViewSources.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewSources_ItemCheck);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 111;
            // 
            // columnHeaderTransport
            // 
            this.columnHeaderTransport.Text = "Verbindung";
            this.columnHeaderTransport.Width = 101;
            // 
            // columnHeaderAvailable
            // 
            this.columnHeaderAvailable.Text = "Verfügbar";
            // 
            // tabPageSync
            // 
            this.tabPageSync.Controls.Add(this.buttonStartSync);
            this.tabPageSync.Controls.Add(this.buttonStopSync);
            this.tabPageSync.Controls.Add(this.listViewSyncSources);
            this.tabPageSync.Controls.Add(this.groupBox1);
            this.tabPageSync.Location = new System.Drawing.Point(4, 22);
            this.tabPageSync.Name = "tabPageSync";
            this.tabPageSync.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSync.Size = new System.Drawing.Size(294, 350);
            this.tabPageSync.TabIndex = 1;
            this.tabPageSync.Text = "Synchronisation";
            this.tabPageSync.UseVisualStyleBackColor = true;
            // 
            // buttonStartSync
            // 
            this.buttonStartSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartSync.Location = new System.Drawing.Point(132, 322);
            this.buttonStartSync.Name = "buttonStartSync";
            this.buttonStartSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStartSync.TabIndex = 5;
            this.buttonStartSync.Text = "Starten";
            this.buttonStartSync.UseVisualStyleBackColor = true;
            this.buttonStartSync.Click += new System.EventHandler(this.buttonStartSync_Click);
            // 
            // buttonStopSync
            // 
            this.buttonStopSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStopSync.Enabled = false;
            this.buttonStopSync.Location = new System.Drawing.Point(213, 322);
            this.buttonStopSync.Name = "buttonStopSync";
            this.buttonStopSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStopSync.TabIndex = 4;
            this.buttonStopSync.Text = "Stoppen";
            this.buttonStopSync.UseVisualStyleBackColor = true;
            this.buttonStopSync.Click += new System.EventHandler(this.buttonStopSync_Click);
            // 
            // listViewSyncSources
            // 
            this.listViewSyncSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSyncSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.listViewSyncSources.Enabled = false;
            this.listViewSyncSources.FullRowSelect = true;
            listViewItem4.Checked = true;
            listViewItem4.StateImageIndex = 1;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            this.listViewSyncSources.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.listViewSyncSources.LabelWrap = false;
            this.listViewSyncSources.Location = new System.Drawing.Point(6, 62);
            this.listViewSyncSources.Name = "listViewSyncSources";
            this.listViewSyncSources.Size = new System.Drawing.Size(282, 254);
            this.listViewSyncSources.TabIndex = 3;
            this.listViewSyncSources.UseCompatibleStateImageBehavior = false;
            this.listViewSyncSources.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 141;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Status";
            this.columnHeader3.Width = 132;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelMainSource);
            this.groupBox1.Controls.Add(this.comboBoxMainSource);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optionen";
            // 
            // labelMainSource
            // 
            this.labelMainSource.AutoSize = true;
            this.labelMainSource.Location = new System.Drawing.Point(6, 22);
            this.labelMainSource.Name = "labelMainSource";
            this.labelMainSource.Size = new System.Drawing.Size(88, 13);
            this.labelMainSource.TabIndex = 1;
            this.labelMainSource.Text = "Referenzkamera:";
            // 
            // comboBoxMainSource
            // 
            this.comboBoxMainSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMainSource.DisplayMember = "Name";
            this.comboBoxMainSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMainSource.FormattingEnabled = true;
            this.comboBoxMainSource.Location = new System.Drawing.Point(100, 19);
            this.comboBoxMainSource.Name = "comboBoxMainSource";
            this.comboBoxMainSource.Size = new System.Drawing.Size(176, 21);
            this.comboBoxMainSource.TabIndex = 0;
            // 
            // tabPageCalibrate
            // 
            this.tabPageCalibrate.Controls.Add(this.buttonStartCalibration);
            this.tabPageCalibrate.Controls.Add(this.buttonStopCalibration);
            this.tabPageCalibrate.Controls.Add(this.groupBoxAnchors);
            this.tabPageCalibrate.Location = new System.Drawing.Point(4, 22);
            this.tabPageCalibrate.Name = "tabPageCalibrate";
            this.tabPageCalibrate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCalibrate.Size = new System.Drawing.Size(294, 350);
            this.tabPageCalibrate.TabIndex = 2;
            this.tabPageCalibrate.Text = "Kalibrierung";
            this.tabPageCalibrate.UseVisualStyleBackColor = true;
            // 
            // buttonStartCalibration
            // 
            this.buttonStartCalibration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartCalibration.Enabled = false;
            this.buttonStartCalibration.Location = new System.Drawing.Point(132, 322);
            this.buttonStartCalibration.Name = "buttonStartCalibration";
            this.buttonStartCalibration.Size = new System.Drawing.Size(75, 23);
            this.buttonStartCalibration.TabIndex = 7;
            this.buttonStartCalibration.Text = "Starten";
            this.buttonStartCalibration.UseVisualStyleBackColor = true;
            this.buttonStartCalibration.Click += new System.EventHandler(this.buttonStartCalibration_Click);
            // 
            // buttonStopCalibration
            // 
            this.buttonStopCalibration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStopCalibration.Enabled = false;
            this.buttonStopCalibration.Location = new System.Drawing.Point(213, 322);
            this.buttonStopCalibration.Name = "buttonStopCalibration";
            this.buttonStopCalibration.Size = new System.Drawing.Size(75, 23);
            this.buttonStopCalibration.TabIndex = 6;
            this.buttonStopCalibration.Text = "Abbrechen";
            this.buttonStopCalibration.UseVisualStyleBackColor = true;
            this.buttonStopCalibration.Click += new System.EventHandler(this.buttonCancelCalibration_Click);
            // 
            // groupBoxAnchors
            // 
            this.groupBoxAnchors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAnchors.Controls.Add(this.labelCalibrationImpossible);
            this.groupBoxAnchors.Controls.Add(this.checkBoxAnchorA);
            this.groupBoxAnchors.Controls.Add(this.checkBoxAnchorC);
            this.groupBoxAnchors.Controls.Add(this.checkBoxAnchorB);
            this.groupBoxAnchors.Enabled = false;
            this.groupBoxAnchors.Location = new System.Drawing.Point(6, 6);
            this.groupBoxAnchors.Name = "groupBoxAnchors";
            this.groupBoxAnchors.Size = new System.Drawing.Size(282, 310);
            this.groupBoxAnchors.TabIndex = 4;
            this.groupBoxAnchors.TabStop = false;
            this.groupBoxAnchors.Text = "Ankerpunkte";
            // 
            // labelCalibrationImpossible
            // 
            this.labelCalibrationImpossible.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelCalibrationImpossible.Location = new System.Drawing.Point(6, 126);
            this.labelCalibrationImpossible.Name = "labelCalibrationImpossible";
            this.labelCalibrationImpossible.Size = new System.Drawing.Size(270, 52);
            this.labelCalibrationImpossible.TabIndex = 4;
            this.labelCalibrationImpossible.Text = "Kalibrierung nicht möglich";
            this.labelCalibrationImpossible.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTipCalibrationImpossible.SetToolTip(this.labelCalibrationImpossible, resources.GetString("labelCalibrationImpossible.ToolTip"));
            // 
            // checkBoxAnchorA
            // 
            this.checkBoxAnchorA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAnchorA.AutoSize = true;
            this.checkBoxAnchorA.Checked = true;
            this.checkBoxAnchorA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAnchorA.Location = new System.Drawing.Point(6, 287);
            this.checkBoxAnchorA.Name = "checkBoxAnchorA";
            this.checkBoxAnchorA.Size = new System.Drawing.Size(33, 17);
            this.checkBoxAnchorA.TabIndex = 2;
            this.checkBoxAnchorA.Tag = "A";
            this.checkBoxAnchorA.Text = "A";
            this.checkBoxAnchorA.Click += new System.EventHandler(this.checkBoxAnchor_Click);
            // 
            // checkBoxAnchorC
            // 
            this.checkBoxAnchorC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAnchorC.AutoSize = true;
            this.checkBoxAnchorC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxAnchorC.Location = new System.Drawing.Point(243, 287);
            this.checkBoxAnchorC.Name = "checkBoxAnchorC";
            this.checkBoxAnchorC.Size = new System.Drawing.Size(33, 17);
            this.checkBoxAnchorC.TabIndex = 3;
            this.checkBoxAnchorC.Tag = "C";
            this.checkBoxAnchorC.Text = "C";
            this.checkBoxAnchorC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxAnchorC.UseVisualStyleBackColor = true;
            this.checkBoxAnchorC.Click += new System.EventHandler(this.checkBoxAnchor_Click);
            // 
            // checkBoxAnchorB
            // 
            this.checkBoxAnchorB.AutoSize = true;
            this.checkBoxAnchorB.Checked = true;
            this.checkBoxAnchorB.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.checkBoxAnchorB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAnchorB.Location = new System.Drawing.Point(6, 19);
            this.checkBoxAnchorB.Name = "checkBoxAnchorB";
            this.checkBoxAnchorB.Size = new System.Drawing.Size(34, 17);
            this.checkBoxAnchorB.TabIndex = 1;
            this.checkBoxAnchorB.Tag = "B";
            this.checkBoxAnchorB.Text = "B";
            this.checkBoxAnchorB.UseVisualStyleBackColor = true;
            this.checkBoxAnchorB.Click += new System.EventHandler(this.checkBoxAnchor_Click);
            // 
            // tabPagePreview
            // 
            this.tabPagePreview.Controls.Add(this.panelPreview);
            this.tabPagePreview.Controls.Add(this.groupBoxPreviewOutput);
            this.tabPagePreview.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePreview.Size = new System.Drawing.Size(294, 350);
            this.tabPagePreview.TabIndex = 3;
            this.tabPagePreview.Text = "Vorschau";
            this.tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // panelPreview
            // 
            this.panelPreview.BackColor = System.Drawing.Color.Black;
            this.panelPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelPreview.Location = new System.Drawing.Point(6, 62);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(282, 282);
            this.panelPreview.TabIndex = 4;
            // 
            // groupBoxPreviewOutput
            // 
            this.groupBoxPreviewOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPreviewOutput.Controls.Add(this.flowLayoutPanel);
            this.groupBoxPreviewOutput.Enabled = false;
            this.groupBoxPreviewOutput.Location = new System.Drawing.Point(6, 6);
            this.groupBoxPreviewOutput.Name = "groupBoxPreviewOutput";
            this.groupBoxPreviewOutput.Size = new System.Drawing.Size(282, 50);
            this.groupBoxPreviewOutput.TabIndex = 3;
            this.groupBoxPreviewOutput.TabStop = false;
            this.groupBoxPreviewOutput.Text = "Ausgabe";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.radioButtonPreviewScene);
            this.flowLayoutPanel.Controls.Add(this.radioButtonPreviewRectangle);
            this.flowLayoutPanel.Location = new System.Drawing.Point(7, 16);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(269, 24);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // radioButtonPreviewScene
            // 
            this.radioButtonPreviewScene.AutoSize = true;
            this.radioButtonPreviewScene.Checked = true;
            this.radioButtonPreviewScene.Location = new System.Drawing.Point(3, 3);
            this.radioButtonPreviewScene.Name = "radioButtonPreviewScene";
            this.radioButtonPreviewScene.Size = new System.Drawing.Size(100, 17);
            this.radioButtonPreviewScene.TabIndex = 0;
            this.radioButtonPreviewScene.TabStop = true;
            this.radioButtonPreviewScene.Text = "Gesamte Szene";
            this.radioButtonPreviewScene.UseVisualStyleBackColor = true;
            this.radioButtonPreviewScene.CheckedChanged += new System.EventHandler(this.radioButtonPreview_CheckedChanged);
            // 
            // radioButtonPreviewRectangle
            // 
            this.radioButtonPreviewRectangle.AutoSize = true;
            this.radioButtonPreviewRectangle.Location = new System.Drawing.Point(109, 3);
            this.radioButtonPreviewRectangle.Name = "radioButtonPreviewRectangle";
            this.radioButtonPreviewRectangle.Size = new System.Drawing.Size(105, 17);
            this.radioButtonPreviewRectangle.TabIndex = 1;
            this.radioButtonPreviewRectangle.Text = "Erfasster Bereich";
            this.radioButtonPreviewRectangle.UseVisualStyleBackColor = true;
            this.radioButtonPreviewRectangle.CheckedChanged += new System.EventHandler(this.radioButtonPreview_CheckedChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgressBar});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 387);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(313, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(93, 17);
            this.statusLabel.Text = "Synchronisiere...";
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.statusProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // contextMenuSources
            // 
            this.contextMenuSources.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConnect,
            this.menuItemDisconnect,
            menuItemSeperator,
            this.menuItemDiscovery});
            this.contextMenuSources.Popup += new System.EventHandler(this.contextMenuSources_Popup);
            // 
            // menuItemConnect
            // 
            this.menuItemConnect.DefaultItem = true;
            this.menuItemConnect.Index = 0;
            this.menuItemConnect.Text = "Verbinden...";
            this.menuItemConnect.Click += new System.EventHandler(this.menuItemConnect_Click);
            // 
            // menuItemDisconnect
            // 
            this.menuItemDisconnect.DefaultItem = true;
            this.menuItemDisconnect.Index = 1;
            this.menuItemDisconnect.Text = "Trennen";
            this.menuItemDisconnect.Click += new System.EventHandler(this.menuItemDisconnect_Click);
            // 
            // menuItemDiscovery
            // 
            this.menuItemDiscovery.Enabled = false;
            this.menuItemDiscovery.Index = 3;
            this.menuItemDiscovery.Text = "Suchen";
            this.menuItemDiscovery.Click += new System.EventHandler(this.contextMenuDiscovery_Click);
            // 
            // toolTipCalibrationImpossible
            // 
            this.toolTipCalibrationImpossible.AutoPopDelay = 10000;
            this.toolTipCalibrationImpossible.InitialDelay = 500;
            this.toolTipCalibrationImpossible.ReshowDelay = 100;
            this.toolTipCalibrationImpossible.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipCalibrationImpossible.ToolTipTitle = "Automatische Kalibrierung";
            // 
            // toolTipAnchorInfo
            // 
            this.toolTipAnchorInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipAnchorInfo.ToolTipTitle = "Ankerpunkt-Koordinaten";
            // 
            // contextMenuAnchor
            // 
            this.contextMenuAnchor.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAnchorName,
            this.menuItem4,
            this.menuItemCalibrate,
            this.menuItemSetPosition,
            this.menuItemResetPosition});
            this.contextMenuAnchor.Popup += new System.EventHandler(this.contextMenuAnchor_Popup);
            // 
            // menuItemAnchorName
            // 
            this.menuItemAnchorName.Enabled = false;
            this.menuItemAnchorName.Index = 0;
            this.menuItemAnchorName.Text = "Ankerpunkt";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // menuItemCalibrate
            // 
            this.menuItemCalibrate.DefaultItem = true;
            this.menuItemCalibrate.Index = 2;
            this.menuItemCalibrate.Text = "Kalibrieren";
            this.menuItemCalibrate.Click += new System.EventHandler(this.menuItemCalibrate_Click);
            // 
            // menuItemSetPosition
            // 
            this.menuItemSetPosition.Index = 3;
            this.menuItemSetPosition.Text = "Position eingeben...";
            this.menuItemSetPosition.Click += new System.EventHandler(this.menuItemSetPosition_Click);
            // 
            // menuItemResetPosition
            // 
            this.menuItemResetPosition.Index = 4;
            this.menuItemResetPosition.Text = "Löschen";
            this.menuItemResetPosition.Click += new System.EventHandler(this.menuItemResetPosition_Click);
            // 
            // contextMenuCalibration
            // 
            this.contextMenuCalibration.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCalibrationReset});
            // 
            // menuItemCalibrationReset
            // 
            this.menuItemCalibrationReset.Index = 0;
            this.menuItemCalibrationReset.Text = "Zurücksetzen";
            this.menuItemCalibrationReset.Click += new System.EventHandler(this.menuItemCalibrationReset_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 409);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kinect2D Konfiguration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigForm_FormClosed);
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageSource.ResumeLayout(false);
            this.tabPageSync.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageCalibrate.ResumeLayout(false);
            this.groupBoxAnchors.ResumeLayout(false);
            this.groupBoxAnchors.PerformLayout();
            this.tabPagePreview.ResumeLayout(false);
            this.groupBoxPreviewOutput.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSource;
        private System.Windows.Forms.TabPage tabPageSync;
        private System.Windows.Forms.TabPage tabPageCalibrate;
        private System.Windows.Forms.ListView listViewSources;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderTransport;
        private System.Windows.Forms.ColumnHeader columnHeaderAvailable;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
        private System.Windows.Forms.Label labelMainSource;
        private System.Windows.Forms.ComboBox comboBoxMainSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonStartSync;
        private System.Windows.Forms.Button buttonStopSync;
        private System.Windows.Forms.ListView listViewSyncSources;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBoxAnchors;
        private System.Windows.Forms.CheckBox checkBoxAnchorA;
        private System.Windows.Forms.CheckBox checkBoxAnchorC;
        private System.Windows.Forms.CheckBox checkBoxAnchorB;
        private System.Windows.Forms.Button buttonStartCalibration;
        private System.Windows.Forms.Button buttonStopCalibration;
        private System.Windows.Forms.TabPage tabPagePreview;
        private System.Windows.Forms.GroupBox groupBoxPreviewOutput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.RadioButton radioButtonPreviewScene;
        private System.Windows.Forms.RadioButton radioButtonPreviewRectangle;
        private System.Windows.Forms.ContextMenu contextMenuSources;
        private System.Windows.Forms.MenuItem menuItemDiscovery;
        private System.Windows.Forms.Label labelCalibrationImpossible;
        private System.Windows.Forms.ToolTip toolTipCalibrationImpossible;
        private System.Windows.Forms.ToolTip toolTipAnchorInfo;
        private System.Windows.Forms.ContextMenu contextMenuAnchor;
        private System.Windows.Forms.MenuItem menuItemSetPosition;
        private System.Windows.Forms.MenuItem menuItemResetPosition;
        private System.Windows.Forms.MenuItem menuItemAnchorName;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItemCalibrate;
        private System.Windows.Forms.ContextMenu contextMenuCalibration;
        private System.Windows.Forms.MenuItem menuItemCalibrationReset;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.MenuItem menuItemConnect;
        private System.Windows.Forms.MenuItem menuItemDisconnect;
    }
}

