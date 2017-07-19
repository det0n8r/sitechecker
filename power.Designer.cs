using System.Windows.Forms;
namespace XDSiteChecker
{
    partial class power
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(power));
            this.peakBufferSizeTextBox = new System.Windows.Forms.TextBox();
            this.peakDisconnectTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.automaticPowerOnCombo = new System.Windows.Forms.ComboBox();
            this.shutdownAfterUseCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.peakDisconnectActionCombo = new System.Windows.Forms.ComboBox();
            this.peakLogoffActionCombo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.peakLogoffTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.offpeakLogoffTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.offpeakLogoffActionCombo = new System.Windows.Forms.ComboBox();
            this.offpeakDisconnectActionCombo = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.offpeakDisconnectTimeout = new System.Windows.Forms.TextBox();
            this.offpeakBufferSizeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.colorDepthCombo = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // peakBufferSizeTextBox
            // 
            this.peakBufferSizeTextBox.Location = new System.Drawing.Point(112, 26);
            this.peakBufferSizeTextBox.Name = "peakBufferSizeTextBox";
            this.peakBufferSizeTextBox.Size = new System.Drawing.Size(70, 20);
            this.peakBufferSizeTextBox.TabIndex = 1;
            // 
            // peakDisconnectTimeoutTextBox
            // 
            this.peakDisconnectTimeoutTextBox.Location = new System.Drawing.Point(112, 79);
            this.peakDisconnectTimeoutTextBox.Name = "peakDisconnectTimeoutTextBox";
            this.peakDisconnectTimeoutTextBox.Size = new System.Drawing.Size(70, 20);
            this.peakDisconnectTimeoutTextBox.TabIndex = 3;
            // 
            // automaticPowerOnCombo
            // 
            this.automaticPowerOnCombo.FormattingEnabled = true;
            this.automaticPowerOnCombo.Items.AddRange(new object[] {
            "True",
            "False"});
            this.automaticPowerOnCombo.Location = new System.Drawing.Point(128, 23);
            this.automaticPowerOnCombo.Name = "automaticPowerOnCombo";
            this.automaticPowerOnCombo.Size = new System.Drawing.Size(100, 21);
            this.automaticPowerOnCombo.TabIndex = 11;
            // 
            // shutdownAfterUseCombo
            // 
            this.shutdownAfterUseCombo.FormattingEnabled = true;
            this.shutdownAfterUseCombo.Items.AddRange(new object[] {
            "True",
            "False"});
            this.shutdownAfterUseCombo.Location = new System.Drawing.Point(128, 50);
            this.shutdownAfterUseCombo.Name = "shutdownAfterUseCombo";
            this.shutdownAfterUseCombo.Size = new System.Drawing.Size(100, 21);
            this.shutdownAfterUseCombo.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Idle Buffer Size %";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Disconnect Action";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Disconnect Timeout";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Logoff Action";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Automatic Power On";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Shutdown After Use";
            // 
            // peakDisconnectActionCombo
            // 
            this.peakDisconnectActionCombo.FormattingEnabled = true;
            this.peakDisconnectActionCombo.Items.AddRange(new object[] {
            "Nothing",
            "Suspend",
            "Shutdown"});
            this.peakDisconnectActionCombo.Location = new System.Drawing.Point(112, 52);
            this.peakDisconnectActionCombo.Name = "peakDisconnectActionCombo";
            this.peakDisconnectActionCombo.Size = new System.Drawing.Size(70, 21);
            this.peakDisconnectActionCombo.TabIndex = 2;
            // 
            // peakLogoffActionCombo
            // 
            this.peakLogoffActionCombo.FormattingEnabled = true;
            this.peakLogoffActionCombo.Items.AddRange(new object[] {
            "Nothing",
            "Suspend",
            "Shutdown"});
            this.peakLogoffActionCombo.Location = new System.Drawing.Point(112, 105);
            this.peakLogoffActionCombo.Name = "peakLogoffActionCombo";
            this.peakLogoffActionCombo.Size = new System.Drawing.Size(70, 21);
            this.peakLogoffActionCombo.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Logoff Timeout";
            // 
            // peakLogoffTimeoutTextBox
            // 
            this.peakLogoffTimeoutTextBox.Location = new System.Drawing.Point(112, 132);
            this.peakLogoffTimeoutTextBox.Name = "peakLogoffTimeoutTextBox";
            this.peakLogoffTimeoutTextBox.Size = new System.Drawing.Size(70, 20);
            this.peakLogoffTimeoutTextBox.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Logoff Timeout";
            // 
            // offpeakLogoffTimeoutTextBox
            // 
            this.offpeakLogoffTimeoutTextBox.Location = new System.Drawing.Point(111, 130);
            this.offpeakLogoffTimeoutTextBox.Name = "offpeakLogoffTimeoutTextBox";
            this.offpeakLogoffTimeoutTextBox.Size = new System.Drawing.Size(70, 20);
            this.offpeakLogoffTimeoutTextBox.TabIndex = 10;
            // 
            // offpeakLogoffActionCombo
            // 
            this.offpeakLogoffActionCombo.FormattingEnabled = true;
            this.offpeakLogoffActionCombo.Items.AddRange(new object[] {
            "Nothing",
            "Suspend",
            "Shutdown"});
            this.offpeakLogoffActionCombo.Location = new System.Drawing.Point(111, 103);
            this.offpeakLogoffActionCombo.Name = "offpeakLogoffActionCombo";
            this.offpeakLogoffActionCombo.Size = new System.Drawing.Size(70, 21);
            this.offpeakLogoffActionCombo.TabIndex = 9;
            // 
            // offpeakDisconnectActionCombo
            // 
            this.offpeakDisconnectActionCombo.FormattingEnabled = true;
            this.offpeakDisconnectActionCombo.Items.AddRange(new object[] {
            "Nothing",
            "Suspend",
            "Shutdown"});
            this.offpeakDisconnectActionCombo.Location = new System.Drawing.Point(111, 50);
            this.offpeakDisconnectActionCombo.Name = "offpeakDisconnectActionCombo";
            this.offpeakDisconnectActionCombo.Size = new System.Drawing.Size(70, 21);
            this.offpeakDisconnectActionCombo.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Logoff Action";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Disconnect Timeout";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Disconnect Action";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Idle Buffer Size %";
            // 
            // offpeakDisconnectTimeout
            // 
            this.offpeakDisconnectTimeout.Location = new System.Drawing.Point(111, 77);
            this.offpeakDisconnectTimeout.Name = "offpeakDisconnectTimeout";
            this.offpeakDisconnectTimeout.Size = new System.Drawing.Size(70, 20);
            this.offpeakDisconnectTimeout.TabIndex = 8;
            // 
            // offpeakBufferSizeTextBox
            // 
            this.offpeakBufferSizeTextBox.Location = new System.Drawing.Point(111, 24);
            this.offpeakBufferSizeTextBox.Name = "offpeakBufferSizeTextBox";
            this.offpeakBufferSizeTextBox.Size = new System.Drawing.Size(70, 20);
            this.offpeakBufferSizeTextBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.offpeakBufferSizeTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.offpeakDisconnectTimeout);
            this.groupBox1.Controls.Add(this.offpeakLogoffTimeoutTextBox);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.offpeakLogoffActionCombo);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.offpeakDisconnectActionCombo);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(207, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 158);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Off-Peak Power Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.peakBufferSizeTextBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.peakDisconnectTimeoutTextBox);
            this.groupBox2.Controls.Add(this.peakLogoffTimeoutTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.peakLogoffActionCombo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.peakDisconnectActionCombo);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(6, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 159);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Peak Power Settings";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.colorDepthCombo);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.automaticPowerOnCombo);
            this.groupBox3.Controls.Add(this.shutdownAfterUseCombo);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(6, 169);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 105);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other Settings";
            // 
            // colorDepthCombo
            // 
            this.colorDepthCombo.FormattingEnabled = true;
            this.colorDepthCombo.Items.AddRange(new object[] {
            "FourBit",
            "EightBit",
            "SixteenBit",
            "TwentyFourBit"});
            this.colorDepthCombo.Location = new System.Drawing.Point(128, 78);
            this.colorDepthCombo.Name = "colorDepthCombo";
            this.colorDepthCombo.Size = new System.Drawing.Size(100, 21);
            this.colorDepthCombo.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 13;
            this.label13.Text = "Color Depth";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(273, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.statusLabel.Location = new System.Drawing.Point(261, 249);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(102, 13);
            this.statusLabel.TabIndex = 29;
            this.statusLabel.Text = "Settings applied.";
            this.statusLabel.Visible = false;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(257, 250);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(138, 13);
            this.errorLabel.TabIndex = 30;
            this.errorLabel.Text = "Failed to apply settings";
            this.errorLabel.Visible = false;
            // 
            // power
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 269);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(413, 307);
            this.MinimumSize = new System.Drawing.Size(413, 307);
            this.Name = "power";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Assignment Power Settings";
            this.Load += new System.EventHandler(this.power_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        
        #endregion

        private System.Windows.Forms.TextBox peakBufferSizeTextBox;
        private System.Windows.Forms.TextBox peakDisconnectTimeoutTextBox;
        private System.Windows.Forms.ComboBox automaticPowerOnCombo;
        private System.Windows.Forms.ComboBox shutdownAfterUseCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox peakDisconnectActionCombo;
        private System.Windows.Forms.ComboBox peakLogoffActionCombo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox peakLogoffTimeoutTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox offpeakLogoffTimeoutTextBox;
        private System.Windows.Forms.ComboBox offpeakLogoffActionCombo;
        private System.Windows.Forms.ComboBox offpeakDisconnectActionCombo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox offpeakDisconnectTimeout;
        private System.Windows.Forms.TextBox offpeakBufferSizeTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox colorDepthCombo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Label statusLabel;
        private Label errorLabel;
    }
}