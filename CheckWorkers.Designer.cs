using System;
using System.ComponentModel;
using DGVColumnSelector;
namespace XDSiteChecker
{
    partial class CheckWorkers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckWorkers));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.exportCSVButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgListBox = new System.Windows.Forms.ListBox();
            this.poweredonOnly = new System.Windows.Forms.CheckBox();
            this.unregOnly = new System.Windows.Forms.CheckBox();
            this.lastDeregLabel = new System.Windows.Forms.Label();
            this.maxRecordLabel = new System.Windows.Forms.Label();
            this.deregInterval = new System.Windows.Forms.NumericUpDown();
            this.maxRecordCount = new System.Windows.Forms.NumericUpDown();
            this.buildGridButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartDesktopServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startDesktopServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deregInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecordCount)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.exportCSVButton);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.lastDeregLabel);
            this.splitContainer1.Panel1.Controls.Add(this.maxRecordLabel);
            this.splitContainer1.Panel1.Controls.Add(this.deregInterval);
            this.splitContainer1.Panel1.Controls.Add(this.maxRecordCount);
            this.splitContainer1.Panel1.Controls.Add(this.buildGridButton);
            this.splitContainer1.Panel1.Controls.Add(this.stopButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(834, 412);
            this.splitContainer1.SplitterDistance = 60;
            this.splitContainer1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(736, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Select Columns";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // exportCSVButton
            // 
            this.exportCSVButton.Location = new System.Drawing.Point(736, 33);
            this.exportCSVButton.Name = "exportCSVButton";
            this.exportCSVButton.Size = new System.Drawing.Size(95, 23);
            this.exportCSVButton.TabIndex = 13;
            this.exportCSVButton.Text = "Export to CSV";
            this.exportCSVButton.UseVisualStyleBackColor = true;
            this.exportCSVButton.Click += new System.EventHandler(this.exportCSVButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dgListBox);
            this.groupBox1.Controls.Add(this.poweredonOnly);
            this.groupBox1.Controls.Add(this.unregOnly);
            this.groupBox1.Location = new System.Drawing.Point(328, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 58);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Desktop Groups:";
            // 
            // dgListBox
            // 
            this.dgListBox.FormattingEnabled = true;
            this.dgListBox.Location = new System.Drawing.Point(232, 10);
            this.dgListBox.Name = "dgListBox";
            this.dgListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.dgListBox.Size = new System.Drawing.Size(163, 43);
            this.dgListBox.TabIndex = 16;
            // 
            // poweredonOnly
            // 
            this.poweredonOnly.AutoSize = true;
            this.poweredonOnly.Location = new System.Drawing.Point(10, 35);
            this.poweredonOnly.Name = "poweredonOnly";
            this.poweredonOnly.Size = new System.Drawing.Size(109, 17);
            this.poweredonOnly.TabIndex = 13;
            this.poweredonOnly.Text = "Only Powered On";
            this.poweredonOnly.UseVisualStyleBackColor = true;
            // 
            // unregOnly
            // 
            this.unregOnly.AutoSize = true;
            this.unregOnly.Location = new System.Drawing.Point(10, 14);
            this.unregOnly.Name = "unregOnly";
            this.unregOnly.Size = new System.Drawing.Size(110, 17);
            this.unregOnly.TabIndex = 12;
            this.unregOnly.Text = "Only Unregistered";
            this.unregOnly.UseVisualStyleBackColor = true;
            // 
            // lastDeregLabel
            // 
            this.lastDeregLabel.AutoSize = true;
            this.lastDeregLabel.Location = new System.Drawing.Point(179, 37);
            this.lastDeregLabel.Name = "lastDeregLabel";
            this.lastDeregLabel.Size = new System.Drawing.Size(146, 13);
            this.lastDeregLabel.TabIndex = 5;
            this.lastDeregLabel.Text = "Last Deregistered (in minutes)";
            // 
            // maxRecordLabel
            // 
            this.maxRecordLabel.AutoSize = true;
            this.maxRecordLabel.Location = new System.Drawing.Point(179, 10);
            this.maxRecordLabel.Name = "maxRecordLabel";
            this.maxRecordLabel.Size = new System.Drawing.Size(94, 13);
            this.maxRecordLabel.TabIndex = 4;
            this.maxRecordLabel.Text = "Maximum Records";
            // 
            // deregInterval
            // 
            this.deregInterval.Location = new System.Drawing.Point(108, 34);
            this.deregInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.deregInterval.Name = "deregInterval";
            this.deregInterval.Size = new System.Drawing.Size(71, 20);
            this.deregInterval.TabIndex = 3;
            // 
            // maxRecordCount
            // 
            this.maxRecordCount.Location = new System.Drawing.Point(108, 7);
            this.maxRecordCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.maxRecordCount.Name = "maxRecordCount";
            this.maxRecordCount.Size = new System.Drawing.Size(71, 20);
            this.maxRecordCount.TabIndex = 2;
            // 
            // buildGridButton
            // 
            this.buildGridButton.Location = new System.Drawing.Point(7, 5);
            this.buildGridButton.Name = "buildGridButton";
            this.buildGridButton.Size = new System.Drawing.Size(95, 23);
            this.buildGridButton.TabIndex = 1;
            this.buildGridButton.Text = "Check Workers";
            this.buildGridButton.UseVisualStyleBackColor = true;
            this.buildGridButton.Click += new System.EventHandler(this.buildGridButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(7, 34);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(95, 23);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Stop/Cancel";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.checkedListBox1);
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer2.Size = new System.Drawing.Size(834, 348);
            this.splitContainer2.SplitterDistance = 322;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 1;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 92);
            this.checkedListBox1.TabIndex = 1;
            this.checkedListBox1.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(834, 322);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.restartDesktopServiceToolStripMenuItem,
            this.startDesktopServiceToolStripMenuItem,
            this.turnOnToolStripMenuItem,
            this.turnOffToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.manageToolStripMenuItem});
            this.contextMenuStrip1.Name = "copyMenuStripItem";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(191, 158);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click_1);
            // 
            // restartDesktopServiceToolStripMenuItem
            // 
            this.restartDesktopServiceToolStripMenuItem.Name = "restartDesktopServiceToolStripMenuItem";
            this.restartDesktopServiceToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.restartDesktopServiceToolStripMenuItem.Text = "Restart Desktop Service";
            this.restartDesktopServiceToolStripMenuItem.Click += new System.EventHandler(this.restartDesktopServiceToolStripMenuItem_Click);
            // 
            // startDesktopServiceToolStripMenuItem
            // 
            this.startDesktopServiceToolStripMenuItem.Name = "startDesktopServiceToolStripMenuItem";
            this.startDesktopServiceToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.startDesktopServiceToolStripMenuItem.Text = "Start Desktop Service";
            this.startDesktopServiceToolStripMenuItem.Click += new System.EventHandler(this.startDesktopServiceToolStripMenuItem_Click);
            // 
            // turnOnToolStripMenuItem
            // 
            this.turnOnToolStripMenuItem.Name = "turnOnToolStripMenuItem";
            this.turnOnToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.turnOnToolStripMenuItem.Text = "Turn On";
            this.turnOnToolStripMenuItem.Click += new System.EventHandler(this.turnOnToolStripMenuItem_Click);
            // 
            // turnOffToolStripMenuItem
            // 
            this.turnOffToolStripMenuItem.Name = "turnOffToolStripMenuItem";
            this.turnOffToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.turnOffToolStripMenuItem.Text = "Turn Off";
            this.turnOffToolStripMenuItem.Click += new System.EventHandler(this.turnOffToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.manageToolStripMenuItem.Text = "Manage";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.manageToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 3);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(834, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(250, 16);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(109, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // CheckWorkers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 412);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CheckWorkers";
            this.Text = "WorkerDiag";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckWorkers_FormClosing);
            this.Load += new System.EventHandler(this.CheckWorkers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckWorkers_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deregInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecordCount)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }          
        void dataGridView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            new DataGridViewColumnSelector(dataGridView1);
            try
            {                
                currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                currentMouseOverColumn = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (dataGridView1.SelectedCells.Count == 1)
                    {
                        if (currentMouseOverColumn >= 0 && currentMouseOverRow >= 0)
                        {
                            if (dataGridView1.Rows[currentMouseOverRow].Cells[currentMouseOverColumn].Value != null)
                            {
                                //dataGridView1.ClearSelection();
                                dataGridView1.CurrentCell = dataGridView1[ currentMouseOverColumn, currentMouseOverRow ];
                            }
                        }                        
                    }
                }
            }
            catch { currentMouseOverRow = 100000; currentMouseOverColumn = 100000; }
            if (currentMouseOverColumn < 0 && currentMouseOverRow < 0)
            {
                //new DataGridViewColumnSelector(dataGridView1);   
            }
        }
        void dataGridView1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            new DataGridViewColumnSelector(dataGridView1);         
            //currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            //currentMouseOverColumn = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;         
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            clearContext();
            if (currentMouseOverColumn >= 0)
            {                
                if (currentMouseOverRow >= 0)
                {
                    clearContext();
                    this.contextMenuStrip1.Items[0].Visible = true;
                    string fqdn = dataGridView1["ComputerName", currentMouseOverRow].Value.ToString();
                    string[] computerName = fqdn.Split('.');
                    string[] desktopService = dataGridView1["DesktopService", currentMouseOverRow].Value.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string powerState = dataGridView1["PowerState", currentMouseOverRow].Value.ToString();
                    if (currentMouseOverRow >= 0)
                    {
                        if (desktopService.Length > 0)
                        {
                            if (desktopService[0] == "Running")
                            {
                                this.contextMenuStrip1.Items[1].Text = "Restart Desktop Service on " + computerName[0];
                                this.contextMenuStrip1.Items[1].Visible = true;
                            }
                            if (desktopService[0] == "Stopped")
                            {
                                this.contextMenuStrip1.Items[2].Text = "Start Desktop Service on " + computerName[0];
                                this.contextMenuStrip1.Items[2].Visible = true;
                            }
                        }
                        if (powerState != null)
                        {
                            if (powerState == "On")
                            {                                
                                this.contextMenuStrip1.Items[4].Text = "Turn Off " + computerName[0];
                                this.contextMenuStrip1.Items[4].Visible = true;
                                this.contextMenuStrip1.Items[5].Text = "Reset " + computerName[0];
                                this.contextMenuStrip1.Items[5].Visible = true;
                            }
                            if (powerState == "Off")
                            {
                                this.contextMenuStrip1.Items[3].Text = "Turn On " + computerName[0];
                                this.contextMenuStrip1.Items[3].Visible = true;
                            }
                        }
                        if (fqdn.Length > 0)
                        {
                            if (desktopService != null)
                            {
                                if (desktopService.Length > 0)
                                {
                                    if (desktopService[0].Length > 1 && desktopService[0] != "N/A" && desktopService[0] != "Pending" && desktopService[0] != "Offline")
                                    {
                                        this.contextMenuStrip1.Items[6].Text = "Manage " + computerName[0];
                                        this.contextMenuStrip1.Items[6].Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void clearContext()
        {
            int i = contextMenuStrip1.Items.Count;
            while (i > 0)
            {
                i--;
                this.contextMenuStrip1.Items[i].Visible = false;
            }
        }
        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buildGridButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.NumericUpDown deregInterval;
        private System.Windows.Forms.NumericUpDown maxRecordCount;
        private System.Windows.Forms.Label lastDeregLabel;
        private System.Windows.Forms.Label maxRecordLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox poweredonOnly;
        private System.Windows.Forms.CheckBox unregOnly;
        private System.Windows.Forms.ListBox dgListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exportCSVButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;        
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartDesktopServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startDesktopServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        public static int currentMouseOverRow;
        public static int currentMouseOverColumn;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button1;
    }
}