using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace XDSiteChecker
{
    public partial class changeddc : Form
    {
        public string[] DDCs { get; set; }
        public string service { get; set; }
        public string currentDDC { get; set; }
        private BackgroundWorker changeDDC = new BackgroundWorker();
        public changeddc()
        {
            InitializeComponent();
            changeDDC.RunWorkerCompleted += new RunWorkerCompletedEventHandler(changeDDC_RunWorkerCompleted);
            changeDDC.DoWork += new DoWorkEventHandler(changeDDC_DoWork);
        }
        private void changeddc_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.AddRange(DDCs);
            this.currentDDCLabel.Text = currentDDC;            
            this.roleLabel.Text = service;
        }
        private void changeDDC_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string[] arg = e.Argument as string[];
            Program.RunScript2(Properties.Resources.SetBrokerHypervisorConnection, arg);
        }
        private void changeDDC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string newddc = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            MessageBox.Show(this, "Changed the preferred broker for " + service + " from " + currentDDC + " to " + newddc, "Changed the preferred DDC", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void doChange()
        {
            string[] parms = new string[] { this.comboBox1.GetItemText(this.comboBox1.SelectedItem), service };            
            changeDDC.RunWorkerAsync(parms);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            doChange();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            doChange();
        }        
    }
}
