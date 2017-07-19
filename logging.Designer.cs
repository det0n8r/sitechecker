namespace XDSiteChecker
{
    partial class logging
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(logging));
            this.outputDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.enableLogging = new System.Windows.Forms.Button();
            this.overwriteLog = new System.Windows.Forms.CheckBox();
            this.overwriteLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // outputDirectory
            // 
            this.outputDirectory.Location = new System.Drawing.Point(104, 37);
            this.outputDirectory.Name = "outputDirectory";
            this.outputDirectory.Size = new System.Drawing.Size(100, 20);
            this.outputDirectory.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log Output Path:";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(226, 35);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 1;
            this.browse.Text = "Browse:";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // enableLogging
            // 
            this.enableLogging.Location = new System.Drawing.Point(209, 68);
            this.enableLogging.Name = "enableLogging";
            this.enableLogging.Size = new System.Drawing.Size(92, 23);
            this.enableLogging.TabIndex = 3;
            this.enableLogging.Text = "Enable Logging";
            this.enableLogging.UseVisualStyleBackColor = true;
            this.enableLogging.Click += new System.EventHandler(this.enableLogging_Click);
            // 
            // overwriteLog
            // 
            this.overwriteLog.AutoSize = true;
            this.overwriteLog.Location = new System.Drawing.Point(185, 73);
            this.overwriteLog.Name = "overwriteLog";
            this.overwriteLog.Size = new System.Drawing.Size(15, 14);
            this.overwriteLog.TabIndex = 2;
            this.overwriteLog.UseVisualStyleBackColor = true;
            // 
            // overwriteLabel
            // 
            this.overwriteLabel.AutoSize = true;
            this.overwriteLabel.Location = new System.Drawing.Point(91, 73);
            this.overwriteLabel.Name = "overwriteLabel";
            this.overwriteLabel.Size = new System.Drawing.Size(91, 13);
            this.overwriteLabel.TabIndex = 5;
            this.overwriteLabel.Text = "Overwrite log file?";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 101);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(292, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(7, 80);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Status";
            this.statusLabel.Visible = false;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(38, 9);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(51, 13);
            this.serverLabel.TabIndex = 8;
            this.serverLabel.Text = "Loading..";
            // 
            // serviceLabel
            // 
            this.serviceLabel.AutoSize = true;
            this.serviceLabel.Location = new System.Drawing.Point(180, 9);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(51, 13);
            this.serviceLabel.TabIndex = 9;
            this.serviceLabel.Text = "Loading..";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "DDC:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Service:";
            // 
            // logging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 132);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.serviceLabel);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.overwriteLabel);
            this.Controls.Add(this.overwriteLog);
            this.Controls.Add(this.enableLogging);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputDirectory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "logging";
            this.Text = "Enable Logging";
            this.Load += new System.EventHandler(this.logging_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Button enableLogging;
        private System.Windows.Forms.CheckBox overwriteLog;
        private System.Windows.Forms.Label overwriteLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}