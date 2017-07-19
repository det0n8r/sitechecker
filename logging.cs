using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace XDSiteChecker
{
    public partial class logging : Form
    {
        public string server { get; set; }
        public string service { get; set; }
        private BackgroundWorker loggingOn = new BackgroundWorker();        

        public logging()
        {
            InitializeComponent();
            string path = Program.installPath(server);
            outputDirectory.KeyDown += new KeyEventHandler(outputDirectory_KeyDown);
            loggingOn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loggingOn_RunWorkerCompleted);
            loggingOn.DoWork += new DoWorkEventHandler(loggingOn_DoWork);
        }
        private void browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog logPath = new FolderBrowserDialog();
            logPath.ShowNewFolderButton = false;
            logPath.Description = "Select log file output path:";
            if (logPath.ShowDialog() == DialogResult.OK)
            {
                string outPath = logPath.SelectedPath;
                bool canWrite = Program.writeAccess(outPath);
                if (canWrite)
                {
                    outputDirectory.Text = outPath;
                }
                else
                {
                    MessageBox.Show(this, "Unable to write to " + outPath + ". Please confirm write permission to path, or select another path and try again.", "Unable to write to " + outPath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void outputDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                doLogging();
            }
        }
        private void enableLogging_Click(object sender, EventArgs e)
        {
            doLogging();
        }
        private void doLogging()
        {
            if (outputDirectory.Text.Length > 0)
            {
                string fqdn = Program.GetFQDN();
                string[] hostName = server.Split('.');
                    bool isLocal = false;
                    if (server.Contains(fqdn))
                    {
                        isLocal = true;
                    }         
                bool canWrite = Program.writeAccess(outputDirectory.Text);
                if (!isLocal)
                {
                    canWrite = true;
                    DialogResult q = MessageBox.Show(this, "Logs will be saved to " + outputDirectory.Text + " on " + hostName[0] + ". Please make sure the directory exists before applying log settings. \r\nContinue?", "Remote server logging", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (q == DialogResult.No)
                    {
                        return;
                    }
                }
                if (canWrite)
                {
                    string fullPath = Program.fullPath(server, service);
                    string[] logPathSplit = outputDirectory.Text.Split(':');     
                    string configFile = Program.configFile(service, fullPath);
                    
                               
                    bool exists = File.Exists(configFile);
                    if (exists)
                    {
                        string configBackup = configFile + ".backup";
                        try
                        {
                            File.Copy(configFile, configBackup);
                        }
                        catch { }
                    }
                    DialogResult d = MessageBox.Show(this, "This operation will modify " + configFile + ", and restart the " + service + " on " + hostName[0] + " to apply logging. Would you like to continue?", "Restart " + service + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (d == DialogResult.Yes)
                    {
                        //enableLogging 
                        overwriteLog.Hide();
                        overwriteLabel.Hide();
                        progressBar1.Show();
                        statusLabel.Text = "Enabling logging...";
                        statusLabel.Show();
                        browse.Enabled = false;
                        outputDirectory.Enabled = false;
                        overwriteLog.Enabled = false;
                        enableLogging.Enabled = false;
                        logging.ActiveForm.Text = "Please wait, enabling logging..";
                        loggingOn.RunWorkerAsync(configFile);                        
                    }                    
                    if (d == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(this, "Unable to write to " + outputDirectory.Text + ". Please confirm write permission to path, or select another path and try again.", "Unable to write to " + outputDirectory.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Please specify an output directory.", "Output directory not specified!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loggingOn_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string arg = e.Argument as string;
            
            Program.configLogging("enable", arg, outputDirectory.Text, server, service, overwriteLog.Checked);            
        }
        private void loggingOn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string fqdn = Program.GetFQDN();
            string[] hostName = server.Split('.');
            bool isLocal = false;
            if (server.Contains(fqdn))
            {
                isLocal = true;
            }  
            if (isLocal)
            {
                progressBar1.Visible = false;
                statusLabel.Visible = false;
                MessageBox.Show(this, "Logging has been enabled for the " + service + ". \r\nLog file location: " + outputDirectory.Text + " on " + hostName[0], "Logging enabled", MessageBoxButtons.OK);
                this.Close();
            }
            else if (!isLocal)
            {
                statusLabel.Visible = false;
                progressBar1.Visible = false;
                MessageBox.Show(this, "Logging has been enabled for the " + service + ". \r\nLog file location: " + outputDirectory.Text + " on " + hostName[0], "Logging enabled", MessageBoxButtons.OK);
                this.Close();
            }
            
        }

        private void logging_Load(object sender, EventArgs e)
        {
            string[] hostName = server.Split('.');
            string[] serv = service.Split(new string[] { "Citrix" }, StringSplitOptions.RemoveEmptyEntries);
            serverLabel.Text = hostName[0];
            serviceLabel.Text = serv[0].TrimStart(' ');
        }       
    }
}
