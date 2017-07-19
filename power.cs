using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XDSiteChecker
{
    public partial class power : Form
    {
        public string assignment { get; set; }
        public string targetDDC { get; set; }
        public string service { get; set; }
        List<assignmentDetails> _assignmentDetails = new List<assignmentDetails>();
        List<assignmentDetails> _originalDetails = new List<assignmentDetails>();
        public class assignmentDetails
        {
            public string detailLabel { get; set; }
            public string detailValue { get; set; }
            public int detailInt { get; set; }
        }
        public power()
        {
            InitializeComponent();            
        }

        private void power_Load(object sender, EventArgs e)
        {            
            this.KeyDown += new KeyEventHandler(power_KeyDown);
            this.Text = "'" + assignment + "' " + "Settings";
            string[] parms = new string[] { assignment, targetDDC };
            string details = Program.RunScript2(Properties.Resources.GetBrokerDesktopGroup, parms);
            string[] detailsArray = details.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string detail in detailsArray)
            {
                string[] detailSplit = detail.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                _assignmentDetails.Add(new assignmentDetails { detailLabel = detailSplit[0].Trim(), detailValue = detailSplit[1].Trim() });
            }
            foreach (assignmentDetails property in _assignmentDetails)
            {
                if (property.detailLabel == "AutomaticPowerOnForAssigned")
                {
                    automaticPowerOnCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "ColorDepth")
                {
                    colorDepthCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }                    
                if (property.detailLabel == "OffPeakBufferSizePercent")
                {
                    offpeakBufferSizeTextBox.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "OffPeakDisconnectAction")
                {
                    offpeakDisconnectActionCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "OffPeakDisconnectTimeout")
                {
                    offpeakDisconnectTimeout.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "OffPeakLogOffAction")
                {
                    offpeakLogoffActionCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "OffPeakLogOffTimeout")
                {
                    offpeakLogoffTimeoutTextBox.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "PeakBufferSizePercent")
                {
                    peakBufferSizeTextBox.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "PeakDisconnectAction")
                {
                    peakDisconnectActionCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "PeakDisconnectTimeout")
                {
                    peakDisconnectTimeoutTextBox.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "PeakLogOffAction")
                {
                    peakLogoffActionCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "PeakLogOffTimeout")
                {
                    peakLogoffTimeoutTextBox.Text = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }
                if (property.detailLabel == "ShutdownDesktopsAfterUse")
                {
                    shutdownAfterUseCombo.SelectedText = property.detailValue;
                    _originalDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = property.detailValue });
                }    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void power_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.setAssignment();
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.statusLabel.Visible = false;
            setAssignment();
        }
        public void setAssignment()
        {
            List<assignmentDetails> _newDetails = new List<assignmentDetails>(); 
            string[] parms = new string[20];
            //string parm;
            bool apply = true;
            foreach (assignmentDetails property in _originalDetails)
            {
                if (property.detailLabel == "AutomaticPowerOnForAssigned")
                {                  
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = automaticPowerOnCombo.Text });
                }
                if (property.detailLabel == "ColorDepth")
                {      
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = colorDepthCombo.Text });                                        
                }
                if (property.detailLabel == "OffPeakBufferSizePercent")
                {                    
                    if (Convert.ToInt32(offpeakBufferSizeTextBox.Text) >= 0 && Convert.ToInt32(offpeakBufferSizeTextBox.Text) < 101)
                    {
                        _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailInt = Convert.ToInt32(offpeakBufferSizeTextBox.Text) });
                    }
                    else
                    {
                        MessageBox.Show("Please enter a value between 0-100%", "Idle Pool Buffer % Out of range", MessageBoxButtons.OK);
                        apply = false;
                    }
                }
                if (property.detailLabel == "OffPeakDisconnectAction")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = offpeakDisconnectActionCombo.Text });
                }
                if (property.detailLabel == "OffPeakDisconnectTimeout")
                {
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailInt = Convert.ToInt32(offpeakDisconnectTimeout.Text) });                    
                }
                if (property.detailLabel == "OffPeakLogOffAction")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = offpeakLogoffActionCombo.Text });
                }
                if (property.detailLabel == "OffPeakLogOffTimeout")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = offpeakLogoffTimeoutTextBox.Text });
                }
                if (property.detailLabel == "PeakBufferSizePercent")
                {
                    if (Convert.ToInt32(peakBufferSizeTextBox.Text) >= 0 && Convert.ToInt32(peakBufferSizeTextBox.Text) < 101)
                    {
                        _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = peakBufferSizeTextBox.Text });                                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter a value between 0-100 %", "Idle Pool Buffer % Out of range", MessageBoxButtons.OK);
                        apply = false;
                    }
                }
                if (property.detailLabel == "PeakDisconnectAction")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = peakDisconnectActionCombo.Text });
                }
                if (property.detailLabel == "PeakDisconnectTimeout")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = peakDisconnectTimeoutTextBox.Text });
                }
                if (property.detailLabel == "PeakLogOffAction")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = peakLogoffActionCombo.Text });
                }
                if (property.detailLabel == "PeakLogOffTimeout")
                {                    
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = peakLogoffTimeoutTextBox.Text });
                }
                if (property.detailLabel == "ShutdownDesktopsAfterUse")
                {
                    _newDetails.Add(new assignmentDetails { detailLabel = property.detailLabel, detailValue = shutdownAfterUseCombo.Text });                 
                } 
            }
            int i = 0;
            foreach (assignmentDetails detail in _newDetails)
            {
                if (detail.detailValue != null)
                {
                    parms[i] = detail.detailValue.ToString();
                }
                else
                {
                    parms[i] = detail.detailInt.ToString();
                }
                i++;                
            }
            parms[i] = assignment;
            if (apply)
            {
                string result = Program.RunScript2(Properties.Resources.SetBrokerDesktopGroup, parms);
                if (result == "\r\n")
                {
                    this.errorLabel.Visible = false;
                    this.statusLabel.Visible = true;
                }
                else
                {
                    //failed to apply
                    this.statusLabel.Visible = false;
                    this.errorLabel.Visible = true;                    
                }
            }            
        }
    }
}
