using System.Windows.Forms;
using System.Linq;

namespace XDSiteChecker
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;        
        public static TreeNode nodeclicked;
        public static TreeNode nodeexpanded;
        public static ListViewItem listViewClicked;
        public static string[] siteRoles= new string[] {"ControllerReaper", "ControllerNameCacheRefresh", "Licensing", "BrokerReaper", "RegistrationHardening", "WorkerNameCacheRefresh", "AccountNameCacheRefresh", "PowerPolicy", "GroupUsage", "AddressNameResolver"};
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

        private void clearContext()
        {
            int i = contextMenuStrip1.Items.Count;
            while (i > 0)
            {
                i--;
                this.contextMenuStrip1.Items[i].Visible = false;                
            }            
        }
    
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 
        public int Panel1MinSize { get; set; }
        public int Panel2MinSize { get; set; }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.registerServiceInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchCheckWorker = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enumerateSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.siteAction1 = new System.Windows.Forms.ToolStripMenuItem();
            this.siteAction2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ddcAction1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ddcAction2 = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceAction1 = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.serviceAction2 = new System.Windows.Forms.ToolStripMenuItem();
            this.enableLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceInstanceAction1 = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableMaintModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.powerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createEvictScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkUnregisteredWorkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceRestartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceRestartToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.suspendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOffAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.clearLog = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnNodeTextSearch = new System.Windows.Forms.Button();
            this.txtNodeTextSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runInPoSHcarefulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.siteInfoListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nodeInfoListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(50, 50, 50, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.LightGray;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel2.Controls.Add(this.statusLabel);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(692, 563);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 541);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(349, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerServiceInstanceToolStripMenuItem,
            this.launchCheckWorker,
            this.refreshToolStripMenuItem,
            this.enumerateSiteToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // registerServiceInstanceToolStripMenuItem
            // 
            this.registerServiceInstanceToolStripMenuItem.Name = "registerServiceInstanceToolStripMenuItem";
            this.registerServiceInstanceToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.registerServiceInstanceToolStripMenuItem.Text = "Register Service Instances";
            this.registerServiceInstanceToolStripMenuItem.Click += new System.EventHandler(this.registerServiceInstanceToolStripMenuItem_Click);
            // 
            // launchCheckWorker
            // 
            this.launchCheckWorker.Name = "launchCheckWorker";
            this.launchCheckWorker.Size = new System.Drawing.Size(208, 22);
            this.launchCheckWorker.Text = "Check Workers";
            this.launchCheckWorker.Click += new System.EventHandler(this.launchCheckWorker_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // enumerateSiteToolStripMenuItem
            // 
            this.enumerateSiteToolStripMenuItem.Name = "enumerateSiteToolStripMenuItem";
            this.enumerateSiteToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.enumerateSiteToolStripMenuItem.Text = "Gather Site Details";
            this.enumerateSiteToolStripMenuItem.Click += new System.EventHandler(this.enumerateSiteToolStripMenuItem_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(280, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList2;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.Size = new System.Drawing.Size(349, 563);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeview1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.siteAction1,
            this.siteAction2,
            this.ddcAction1,
            this.ddcAction2,
            this.serviceAction1,
            this.toolStripSeparator1,
            this.serviceAction2,
            this.serviceInstanceAction1,
            this.moveToToolStripMenuItem,
            this.changeDDCToolStripMenuItem,
            this.removeStorageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.stopTaskToolStripMenuItem,
            this.enableMaintModeToolStripMenuItem,
            this.toolStripSeparator2,
            this.powerSettingsToolStripMenuItem,
            this.removeTasksToolStripMenuItem,
            this.stopTasksToolStripMenuItem,
            this.createEvictScriptToolStripMenuItem,
            this.checkUnregisteredWorkersToolStripMenuItem,
            this.powerToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.logOffToolStripMenuItem,
            this.disconnectAllToolStripMenuItem,
            this.logOffAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 520);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // siteAction1
            // 
            this.siteAction1.Name = "siteAction1";
            this.siteAction1.Size = new System.Drawing.Size(195, 22);
            this.siteAction1.Text = "Site Details";
            // 
            // siteAction2
            // 
            this.siteAction2.Name = "siteAction2";
            this.siteAction2.Padding = new System.Windows.Forms.Padding(0);
            this.siteAction2.Size = new System.Drawing.Size(195, 20);
            this.siteAction2.Text = "Refresh";
            this.siteAction2.Click += new System.EventHandler(this.siteAction2_Click);
            // 
            // ddcAction1
            // 
            this.ddcAction1.Name = "ddcAction1";
            this.ddcAction1.Size = new System.Drawing.Size(195, 22);
            this.ddcAction1.Text = "Controller Details";
            // 
            // ddcAction2
            // 
            this.ddcAction2.Name = "ddcAction2";
            this.ddcAction2.Size = new System.Drawing.Size(195, 22);
            this.ddcAction2.Text = "Reset Service Instances";
            this.ddcAction2.Click += new System.EventHandler(this.ddcAction2_Click);
            // 
            // serviceAction1
            // 
            this.serviceAction1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.serviceAction1.Name = "serviceAction1";
            this.serviceAction1.Size = new System.Drawing.Size(195, 22);
            this.serviceAction1.Text = "Service Control";
            this.serviceAction1.Click += new System.EventHandler(this.serviceAction1_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.resetToolStripMenuItem.Text = "Restart";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // serviceAction2
            // 
            this.serviceAction2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableLoggingToolStripMenuItem,
            this.disableLoggingToolStripMenuItem});
            this.serviceAction2.Name = "serviceAction2";
            this.serviceAction2.Size = new System.Drawing.Size(195, 22);
            this.serviceAction2.Text = "Logging";
            // 
            // enableLoggingToolStripMenuItem
            // 
            this.enableLoggingToolStripMenuItem.Name = "enableLoggingToolStripMenuItem";
            this.enableLoggingToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.enableLoggingToolStripMenuItem.Text = "Enable Logging";
            this.enableLoggingToolStripMenuItem.Click += new System.EventHandler(this.enableLoggingToolStripMenuItem_Click);
            // 
            // disableLoggingToolStripMenuItem
            // 
            this.disableLoggingToolStripMenuItem.Name = "disableLoggingToolStripMenuItem";
            this.disableLoggingToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.disableLoggingToolStripMenuItem.Text = "Disable Logging";
            this.disableLoggingToolStripMenuItem.Click += new System.EventHandler(this.disableLoggingToolStripMenuItem_Click);
            // 
            // serviceInstanceAction1
            // 
            this.serviceInstanceAction1.Name = "serviceInstanceAction1";
            this.serviceInstanceAction1.Size = new System.Drawing.Size(195, 22);
            this.serviceInstanceAction1.Text = "Check Service Instance";
            this.serviceInstanceAction1.Click += new System.EventHandler(this.serviceInstanceAction1_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.moveToToolStripMenuItem.Text = "Clear Preferred DDC";
            this.moveToToolStripMenuItem.Click += new System.EventHandler(this.moveToToolStripMenuItem_Click);
            // 
            // changeDDCToolStripMenuItem
            // 
            this.changeDDCToolStripMenuItem.Name = "changeDDCToolStripMenuItem";
            this.changeDDCToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.changeDDCToolStripMenuItem.Text = "Change Preferred DDC";
            this.changeDDCToolStripMenuItem.Click += new System.EventHandler(this.changeDDCToolStripMenuItem_Click);
            // 
            // removeStorageToolStripMenuItem
            // 
            this.removeStorageToolStripMenuItem.Name = "removeStorageToolStripMenuItem";
            this.removeStorageToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.removeStorageToolStripMenuItem.Text = "Remove Storage";
            this.removeStorageToolStripMenuItem.Click += new System.EventHandler(this.removeStorageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem1.Text = "Remove Task";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // stopTaskToolStripMenuItem
            // 
            this.stopTaskToolStripMenuItem.Name = "stopTaskToolStripMenuItem";
            this.stopTaskToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.stopTaskToolStripMenuItem.Text = "Stop Task";
            this.stopTaskToolStripMenuItem.Click += new System.EventHandler(this.stopTaskToolStripMenuItem_Click);
            // 
            // enableMaintModeToolStripMenuItem
            // 
            this.enableMaintModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem});
            this.enableMaintModeToolStripMenuItem.Name = "enableMaintModeToolStripMenuItem";
            this.enableMaintModeToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.enableMaintModeToolStripMenuItem.Text = "Maintenance Mode";
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // powerSettingsToolStripMenuItem
            // 
            this.powerSettingsToolStripMenuItem.Name = "powerSettingsToolStripMenuItem";
            this.powerSettingsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.powerSettingsToolStripMenuItem.Text = "Edit Advanced Settings";
            this.powerSettingsToolStripMenuItem.Click += new System.EventHandler(this.powerSettingsToolStripMenuItem_Click);
            // 
            // removeTasksToolStripMenuItem
            // 
            this.removeTasksToolStripMenuItem.Name = "removeTasksToolStripMenuItem";
            this.removeTasksToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.removeTasksToolStripMenuItem.Text = "Remove Tasks";
            this.removeTasksToolStripMenuItem.Click += new System.EventHandler(this.removeTasksToolStripMenuItem_Click);
            // 
            // stopTasksToolStripMenuItem
            // 
            this.stopTasksToolStripMenuItem.Name = "stopTasksToolStripMenuItem";
            this.stopTasksToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.stopTasksToolStripMenuItem.Text = "Stop Tasks";
            this.stopTasksToolStripMenuItem.Click += new System.EventHandler(this.stopTasksToolStripMenuItem_Click);
            // 
            // createEvictScriptToolStripMenuItem
            // 
            this.createEvictScriptToolStripMenuItem.Name = "createEvictScriptToolStripMenuItem";
            this.createEvictScriptToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.createEvictScriptToolStripMenuItem.Text = "Create Evict Script";
            this.createEvictScriptToolStripMenuItem.Click += new System.EventHandler(this.createEvictScriptToolStripMenuItem_Click);
            // 
            // checkUnregisteredWorkersToolStripMenuItem
            // 
            this.checkUnregisteredWorkersToolStripMenuItem.Name = "checkUnregisteredWorkersToolStripMenuItem";
            this.checkUnregisteredWorkersToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.checkUnregisteredWorkersToolStripMenuItem.Text = "Check Workers";
            this.checkUnregisteredWorkersToolStripMenuItem.Click += new System.EventHandler(this.checkUnregisteredWorkersToolStripMenuItem_Click);
            // 
            // powerToolStripMenuItem
            // 
            this.powerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turnOnToolStripMenuItem,
            this.turnOffToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.forceRestartToolStripMenuItem,
            this.forceRestartToolStripMenuItem1,
            this.suspendToolStripMenuItem,
            this.resumeToolStripMenuItem});
            this.powerToolStripMenuItem.Name = "powerToolStripMenuItem";
            this.powerToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.powerToolStripMenuItem.Text = "Power Action";
            // 
            // turnOnToolStripMenuItem
            // 
            this.turnOnToolStripMenuItem.Name = "turnOnToolStripMenuItem";
            this.turnOnToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.turnOnToolStripMenuItem.Text = "Turn On";
            this.turnOnToolStripMenuItem.Click += new System.EventHandler(this.turnOnToolStripMenuItem_Click);
            // 
            // turnOffToolStripMenuItem
            // 
            this.turnOffToolStripMenuItem.Name = "turnOffToolStripMenuItem";
            this.turnOffToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.turnOffToolStripMenuItem.Text = "Turn Off";
            this.turnOffToolStripMenuItem.Click += new System.EventHandler(this.turnOffToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.restartToolStripMenuItem.Text = "Shutdown";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // forceRestartToolStripMenuItem
            // 
            this.forceRestartToolStripMenuItem.Name = "forceRestartToolStripMenuItem";
            this.forceRestartToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.forceRestartToolStripMenuItem.Text = "Restart";
            this.forceRestartToolStripMenuItem.Click += new System.EventHandler(this.forceRestartToolStripMenuItem_Click);
            // 
            // forceRestartToolStripMenuItem1
            // 
            this.forceRestartToolStripMenuItem1.Name = "forceRestartToolStripMenuItem1";
            this.forceRestartToolStripMenuItem1.Size = new System.Drawing.Size(128, 22);
            this.forceRestartToolStripMenuItem1.Text = "Reset";
            this.forceRestartToolStripMenuItem1.Click += new System.EventHandler(this.forceRestartToolStripMenuItem1_Click);
            // 
            // suspendToolStripMenuItem
            // 
            this.suspendToolStripMenuItem.Name = "suspendToolStripMenuItem";
            this.suspendToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.suspendToolStripMenuItem.Text = "Suspend";
            this.suspendToolStripMenuItem.Click += new System.EventHandler(this.suspendToolStripMenuItem_Click);
            // 
            // resumeToolStripMenuItem
            // 
            this.resumeToolStripMenuItem.Name = "resumeToolStripMenuItem";
            this.resumeToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.resumeToolStripMenuItem.Text = "Resume";
            this.resumeToolStripMenuItem.Click += new System.EventHandler(this.resumeToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // logOffToolStripMenuItem
            // 
            this.logOffToolStripMenuItem.Name = "logOffToolStripMenuItem";
            this.logOffToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.logOffToolStripMenuItem.Text = "Log Off";
            this.logOffToolStripMenuItem.Click += new System.EventHandler(this.logOffToolStripMenuItem_Click);
            // 
            // disconnectAllToolStripMenuItem
            // 
            this.disconnectAllToolStripMenuItem.Name = "disconnectAllToolStripMenuItem";
            this.disconnectAllToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.disconnectAllToolStripMenuItem.Text = "Disconnect All";
            this.disconnectAllToolStripMenuItem.Click += new System.EventHandler(this.disconnectAllToolStripMenuItem_Click);
            // 
            // logOffAllToolStripMenuItem
            // 
            this.logOffAllToolStripMenuItem.Name = "logOffAllToolStripMenuItem";
            this.logOffAllToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.logOffAllToolStripMenuItem.Text = "Log Off All";
            this.logOffAllToolStripMenuItem.Click += new System.EventHandler(this.logOffAllToolStripMenuItem_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "000_EastArrow_all.ico");
            this.imageList2.Images.SetKeyName(1, "005_VIPFolder_h32bit_32.ico");
            this.imageList2.Images.SetKeyName(2, "000_AppFolder_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(3, "000_appliance_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(4, "000_appliances_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(5, "000_FileIndexs_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(6, "000_NetworkServices_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(7, "mcs.ico");
            this.imageList2.Images.SetKeyName(8, "000_Server_combo_all.ico");
            this.imageList2.Images.SetKeyName(9, "005_SelectTree_Combo_all.ico");
            this.imageList2.Images.SetKeyName(10, "002_Users_Combo_All.ico");
            this.imageList2.Images.SetKeyName(11, "015_User_h_16.ico");
            this.imageList2.Images.SetKeyName(12, "000_Network_Combo_All.ico");
            this.imageList2.Images.SetKeyName(13, "015_DashNetwork_combo.ico");
            this.imageList2.Images.SetKeyName(14, "mcs.ico");
            this.imageList2.Images.SetKeyName(15, "000_VDiskPool_Combo_All.ico");
            this.imageList2.Images.SetKeyName(16, "000_VDisk_Combo_All.ico");
            this.imageList2.Images.SetKeyName(17, "015_URLGroup_l16_16.ico");
            this.imageList2.Images.SetKeyName(18, "015_URLGroups_l16_16.ico");
            this.imageList2.Images.SetKeyName(19, "Running.ico");
            this.imageList2.Images.SetKeyName(20, "Stopped.ico");
            this.imageList2.Images.SetKeyName(21, "Suspended.ico");
            this.imageList2.Images.SetKeyName(22, "000_ConfigTool_Combo_All.ico");
            this.imageList2.Images.SetKeyName(23, "000_SMB_Combo_All.ico");
            this.imageList2.Images.SetKeyName(24, "000_XenDesktop_Combo_All.ico");
            this.imageList2.Images.SetKeyName(25, "000_StorageLink_Combo_All.ico");
            this.imageList2.Images.SetKeyName(26, "000_XenDesktopDis_Combo_Vista.ico");
            this.imageList2.Images.SetKeyName(27, "000_XenDesktop_Combo_All.ico");
            this.imageList2.Images.SetKeyName(28, "000_Computer_Combo_All.ico");
            this.imageList2.Images.SetKeyName(29, "001_BladeServer_Combo_All.ico");
            this.imageList2.Images.SetKeyName(30, "000_Error_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(31, "000_ok_Combo_2k.ico");
            this.imageList2.Images.SetKeyName(32, "000_StartMachine_h32bit_16.ico");
            this.imageList2.Images.SetKeyName(33, "000_Machine_h32bit_16.ico");
            this.imageList2.Images.SetKeyName(34, "000_ForceShutdownMachine_h32bit_16.ico");
            this.imageList2.Images.SetKeyName(35, "000_search_Combo_All.ico");
            this.imageList2.Images.SetKeyName(36, "000_EnablePowerControl_Combo_All.ico");
            this.imageList2.Images.SetKeyName(37, "000_PowerMgmtConfig_Combo_All.ico");
            this.imageList2.Images.SetKeyName(38, "000_DisablePowerControl_Combo_All.ico");
            this.imageList2.Images.SetKeyName(39, "000_ActiveSession_Combo_All.ico");
            this.imageList2.Images.SetKeyName(40, "000_SessionActive_Combo_All.ico");
            this.imageList2.Images.SetKeyName(41, "000_SessionDisconnected_Combo_All.ico");
            this.imageList2.Images.SetKeyName(42, "000_SessionErrors_Combo_All.ico");
            this.imageList2.Images.SetKeyName(43, "000_Sessions_Combo_All.ico");
            this.imageList2.Images.SetKeyName(44, "75.png");
            this.imageList2.Images.SetKeyName(45, "000_Add_AD_Object_To_Group_Combo_All.ico");
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 424);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pictureBox2);
            this.splitContainer3.Panel1.Controls.Add(this.clearLog);
            this.splitContainer3.Panel1.Controls.Add(this.label4);
            this.splitContainer3.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer3.Panel1.Controls.Add(this.btnNodeTextSearch);
            this.splitContainer3.Panel1.Controls.Add(this.txtNodeTextSearch);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            this.splitContainer3.Panel1MinSize = 15;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.logTextBox);
            this.splitContainer3.Size = new System.Drawing.Size(339, 139);
            this.splitContainer3.SplitterDistance = 20;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 39;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(82, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 16);
            this.pictureBox2.TabIndex = 45;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click_1);
            // 
            // clearLog
            // 
            this.clearLog.AutoSize = true;
            this.clearLog.Location = new System.Drawing.Point(105, 4);
            this.clearLog.Name = "clearLog";
            this.clearLog.Size = new System.Drawing.Size(31, 13);
            this.clearLog.TabIndex = 44;
            this.clearLog.TabStop = true;
            this.clearLog.Text = "Clear";
            this.clearLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearLog_LinkClicked_2);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Powershell Log";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(191, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 21);
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // btnNodeTextSearch
            // 
            this.btnNodeTextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNodeTextSearch.Location = new System.Drawing.Point(296, 0);
            this.btnNodeTextSearch.Name = "btnNodeTextSearch";
            this.btnNodeTextSearch.Size = new System.Drawing.Size(44, 21);
            this.btnNodeTextSearch.TabIndex = 41;
            this.btnNodeTextSearch.Text = "Go!";
            this.btnNodeTextSearch.UseVisualStyleBackColor = true;
            // 
            // txtNodeTextSearch
            // 
            this.txtNodeTextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNodeTextSearch.Location = new System.Drawing.Point(214, 2);
            this.txtNodeTextSearch.Name = "txtNodeTextSearch";
            this.txtNodeTextSearch.Size = new System.Drawing.Size(76, 20);
            this.txtNodeTextSearch.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(418, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Find:";
            // 
            // logTextBox
            // 
            this.logTextBox.ContextMenuStrip = this.contextMenuStrip3;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.ForeColor = System.Drawing.Color.Black;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(339, 116);
            this.logTextBox.TabIndex = 35;
            this.logTextBox.Text = "";
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.runInPoSHcarefulToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(166, 48);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // runInPoSHcarefulToolStripMenuItem
            // 
            this.runInPoSHcarefulToolStripMenuItem.Name = "runInPoSHcarefulToolStripMenuItem";
            this.runInPoSHcarefulToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.runInPoSHcarefulToolStripMenuItem.Text = "Open in Notepad";
            this.runInPoSHcarefulToolStripMenuItem.Click += new System.EventHandler(this.runInPoSHcarefulToolStripMenuItem_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(0, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(54, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Loading...";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.siteInfoListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.nodeInfoListView);
            this.splitContainer2.Size = new System.Drawing.Size(339, 424);
            this.splitContainer2.SplitterDistance = 191;
            this.splitContainer2.SplitterIncrement = 2;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 26;
            // 
            // siteInfoListView
            // 
            this.siteInfoListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.siteInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.siteInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.siteInfoListView.GridLines = true;
            this.siteInfoListView.Location = new System.Drawing.Point(0, 0);
            this.siteInfoListView.Name = "siteInfoListView";
            this.siteInfoListView.Scrollable = false;
            this.siteInfoListView.Size = new System.Drawing.Size(335, 187);
            this.siteInfoListView.TabIndex = 22;
            this.siteInfoListView.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 115;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = -1;
            // 
            // nodeInfoListView
            // 
            this.nodeInfoListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.nodeInfoListView.ContextMenuStrip = this.contextMenuStrip2;
            this.nodeInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeInfoListView.GridLines = true;
            this.nodeInfoListView.Location = new System.Drawing.Point(0, 0);
            this.nodeInfoListView.Name = "nodeInfoListView";
            this.nodeInfoListView.Size = new System.Drawing.Size(335, 224);
            this.nodeInfoListView.TabIndex = 23;
            this.nodeInfoListView.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 115;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = -1;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip2.Size = new System.Drawing.Size(103, 24);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click_1);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(692, 563);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(699, 588);
            this.Name = "Main";
            this.Text = "XenDesktop SiteDiag";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void pictureBox2_MouseLeave(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void pictureBox2_MouseEnter(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
                      
        private void treeview1_MouseDown(object sender, MouseEventArgs e)
        {
            nodeclicked = this.treeView1.GetNodeAt(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                this.treeView1.SelectedNode = nodeclicked;
                Application.DoEvents();
            }
            if (nodeclicked != null && e.Button != MouseButtons.Right)
            {
                if (nodeclicked.IsExpanded == true)
                {
                    nodeclicked.Collapse();
                }
                else 
                {
                    nodeclicked.Expand();
                }                
            }
        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (nodeclicked != null)
            {
                if (nodeclicked.Level == 0)
                {
                    clearContext();
                    this.contextMenuStrip1.Items[1].Visible = true;
                }
                if (nodeclicked.Level == 1)
                {
                    clearContext();
                    this.contextMenuStrip1.Items[1].Visible = true;
                    if (nodeclicked.Text.Contains("Assignments"))
                    {
                        this.contextMenuStrip1.Items[19].Visible = true; 
                    }
                }
                if (nodeclicked.Level == 2)
                {
                    clearContext();
                    //this.contextMenuStrip1.Items[1].Visible = true;
                    if (nodeclicked.Parent.Text.Contains("Services"))
                    {
                        this.contextMenuStrip1.Items[3].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Assignments"))
                    {
                        this.contextMenuStrip1.Items[13].Visible = true;
                        this.contextMenuStrip1.Items[19].Visible = true;                                              
                    }
                }
                if (nodeclicked.Level == 3)
                {
                    clearContext();
                    int childCount = nodeclicked.GetNodeCount(false);
                    if (childCount > 0)
                    {
                        if (nodeclicked.Text == "Running")
                        {
                            this.contextMenuStrip1.Items[17].Visible = true;                            
                        }
                        if (nodeclicked.Text.Contains("Completed") | nodeclicked.Text.Contains("Terminated"))
                        {
                            this.contextMenuStrip1.Items[16].Visible = true;
                        }
                    }
                    if (nodeclicked.Parent.Text == "Desktop Groups" | nodeclicked.Parent.Text == "Delivery Groups")
                    {
                        this.contextMenuStrip1.Items[13].Visible = true;
                        this.contextMenuStrip1.Items[15].Visible = true;
                    }
                    if (nodeclicked.Text.Contains("Un-Registered"))
                    {
                        this.contextMenuStrip1.Items[19].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Hosts"))
                    {                        
                        this.contextMenuStrip1.Items[9].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Controllers"))
                    {
                        this.contextMenuStrip1.Items[18].Visible = true;
                    }
                    if (nodeclicked.Text == "Active Sessions")
                    {
                        if (activeSessionsNode.Nodes.Count > 0)
                        {
                            this.contextMenuStrip1.Items[23].Visible = true;
                            this.contextMenuStrip1.Items[24].Visible = true;
                        }                        
                    }
                    if (nodeclicked.Text == "Inactive Sessions")
                    {
                        if (inactiveSessionsNode.Nodes.Count > 0)
                        {
                            this.contextMenuStrip1.Items[24].Visible = true;
                        }                        
                    }
                }
                if (nodeclicked.Level == 4)
                {
                    clearContext();
                    if (nodeclicked.Parent.Parent.Text.Contains("Controllers") && !nodeclicked.Text.Contains("Site Roles"))
                    {
                        this.contextMenuStrip1.Items[4].Visible = true;
                        this.contextMenuStrip1.Items[5].Visible = true;
                        this.contextMenuStrip1.Items[6].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Completed") || nodeclicked.Parent.Text.Contains("Terminated"))
                    {
                        this.contextMenuStrip1.Items[11].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Running"))
                    {
                        this.contextMenuStrip1.Items[12].Visible = true;
                    }
                    if (nodeclicked.Parent.Parent.Text.Contains("ions") && nodeclicked.Text.Contains("Active"))
                    {
                        this.contextMenuStrip1.Items[21].Visible = true;
                        this.contextMenuStrip1.Items[22].Visible = true;
                    }
                    if (nodeclicked.Parent.Parent.Text.Contains("ions") && nodeclicked.Text.Contains("Disconnected"))
                    {
                        this.contextMenuStrip1.Items[22].Visible = true;
                    }
                    if (nodeclicked.Parent.Parent.Text.Contains("Catalogs"))
                    {
                        this.contextMenuStrip1.Items[13].Visible = true;
                        if (nodeclicked.Parent.Tag != null && nodeclicked.Parent.Tag.ToString().Contains("Power"))
                        {
                            this.contextMenuStrip1.Items[20].Visible = true;
                        }                        
                    }
                }
                if (nodeclicked.Level == 5)
                {
                    clearContext();
                    if (nodeclicked.Parent.Parent.Parent.Text == "Desktop Groups")
                    {
                        this.contextMenuStrip1.Items[13].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Service"))
                    {
                        this.contextMenuStrip1.Items[7].Visible = true;
                    }
                    if (nodeclicked.Parent.Text.Contains("Storage"))
                    {                        
                        this.contextMenuStrip1.Items[10].Visible = true;
                    }
                    if (nodeclicked.Parent.Text == "Registered")
                    {
                        this.contextMenuStrip1.Items[21].Visible = true;
                        this.contextMenuStrip1.Items[22].Visible = true;
                    }
                    if (nodeclicked.Parent.Text == "Site Roles")
                    {
                        int matches = 0;
                        foreach (string x in siteRoles)
                        {
                            if (x.Contains(nodeclicked.Text))
                            {
                                matches++;
                            }
                        }
                        if (matches == 0)
                        {
                            this.contextMenuStrip1.Items[9].Visible = true;
                        }
                    }                    
                }
            }
            else
            {
                clearContext();
            }
        }        

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem siteAction1;
        private ToolStripMenuItem siteAction2;
        private ToolStripMenuItem ddcAction1;
        private ToolStripMenuItem ddcAction2;
        private ToolStripMenuItem serviceAction1;
        private ToolStripMenuItem serviceAction2;
        private ToolStripMenuItem serviceInstanceAction1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem enableLoggingToolStripMenuItem;
        private ToolStripMenuItem disableLoggingToolStripMenuItem;
        private ToolStripMenuItem moveToToolStripMenuItem;
        private ToolStripMenuItem changeDDCToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem removeStorageToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip3;
        private ToolStripMenuItem copyToolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem stopTaskToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem registerServiceInstanceToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private Label statusLabel;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem enableMaintModeToolStripMenuItem;
        private ToolTip toolTip1;
        private ToolStripMenuItem runInPoSHcarefulToolStripMenuItem;
        private ToolStripMenuItem enableToolStripMenuItem;
        private ToolStripMenuItem disableToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem powerSettingsToolStripMenuItem;
        private ImageList imageList2;
        private ToolStripMenuItem removeTasksToolStripMenuItem;
        private ToolStripMenuItem stopTasksToolStripMenuItem;
        private ToolStripMenuItem createEvictScriptToolStripMenuItem;
        private ToolStripMenuItem checkUnregisteredWorkersToolStripMenuItem;
        private ToolStripMenuItem powerToolStripMenuItem;
        private ToolStripMenuItem turnOnToolStripMenuItem;
        private ToolStripMenuItem turnOffToolStripMenuItem;
        private ToolStripMenuItem restartToolStripMenuItem;
        private ToolStripMenuItem forceRestartToolStripMenuItem;
        private ToolStripMenuItem forceRestartToolStripMenuItem1;
        private ToolStripMenuItem suspendToolStripMenuItem;
        private ToolStripMenuItem resumeToolStripMenuItem;
        private SplitContainer splitContainer2;
        private ListView siteInfoListView;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ListView nodeInfoListView;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private RichTextBox logTextBox;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private ToolStripMenuItem logOffToolStripMenuItem;
        private ToolStripMenuItem disconnectAllToolStripMenuItem;
        private ToolStripMenuItem logOffAllToolStripMenuItem;
        private ToolStripMenuItem launchCheckWorker;
        private ToolStripMenuItem enumerateSiteToolStripMenuItem;
        private SplitContainer splitContainer3;
        private PictureBox pictureBox2;
        private LinkLabel clearLog;
        private Label label4;
        private PictureBox pictureBox1;
        private Button btnNodeTextSearch;
        private TextBox txtNodeTextSearch;
        private Label label3;
    }
}

