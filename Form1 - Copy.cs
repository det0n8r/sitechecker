using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.ServiceProcess;
using System.Diagnostics;

namespace XDServiceChecker
{
    public partial class Form1 : Form
    {
        private BackgroundWorker serviceRestart = new BackgroundWorker();
        private BackgroundWorker serviceReset = new BackgroundWorker();
        private BackgroundWorker disableLogging = new BackgroundWorker();
        private BackgroundWorker buildTree = new BackgroundWorker();
        private BackgroundWorker runScript = new BackgroundWorker();
        public Form1()
        {            
            InitializeComponent();                 
            Font font = new Font(this.treeView1.Font, FontStyle.Bold);
            treeView1.Font = font;
            ServiceProgressBar.Style = ProgressBarStyle.Marquee;            
            serviceRestart.WorkerReportsProgress = true;
            serviceRestart.WorkerSupportsCancellation = true;
            serviceRestart.ProgressChanged += new ProgressChangedEventHandler(restartService_ProgressChanged);
            serviceRestart.RunWorkerCompleted += new RunWorkerCompletedEventHandler(restartService_RunWorkerCompleted);
            serviceRestart.DoWork += new DoWorkEventHandler(restartService_DoWork);
            serviceReset.DoWork += new DoWorkEventHandler(serviceReset_DoWork);
            serviceReset.RunWorkerCompleted += new RunWorkerCompletedEventHandler(serviceReset_RunWorkerCompleted);
            disableLogging.RunWorkerCompleted += new RunWorkerCompletedEventHandler(disableLogging_RunWorkerCompleted);
            disableLogging.DoWork += new DoWorkEventHandler(disableLogging_DoWork);
            buildTree.DoWork += new DoWorkEventHandler(buildTree_DoWork);
            buildTree.RunWorkerCompleted += new RunWorkerCompletedEventHandler(buildTree_RunWorkerCompleted);
            runScript.DoWork += new DoWorkEventHandler(runScript_DoWork);
            runScript.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runScript_RunWorkerCompleted);
        }       

        
        
#region Build Tree

        public delegate void AddNode(string name, Color color);
        public delegate void NodeColor(string node, Color color);
        public delegate void SetLabels(string[] content);
        public delegate void SetLabel(string content, Label labelName);
        public delegate void CheckServices(string[] services, List<string> servers);
        public delegate void AddSiteNode(string name);
        public delegate void PopUp(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        public delegate void RunScript(string script, string param);
        public delegate void SetCursor(string status);
        TreeNode TopNode;
        TreeNode brokerNode;
        TreeNode serviceNode;
        TreeNode instanceNode;
        /// <summary>
        /// Adds a root node.
        /// </summary>
        /// <param name="i">Label of the new node.</param>
        public void addSiteNode(string name)
        {
            treeView1.TopNode.Remove();            
            TopNode = treeView1.Nodes.Add(name);
        }
        public void setColor(string node, Color color)
        {
            
        }
        public void AddBrokerNode(string name, Color color)
        {            
            brokerNode = TopNode.Nodes.Add(name);
            brokerNode.BackColor = color;
            TopNode.Expand();
        }
        public void AddServiceNode(string name, Color color)
        {            
            serviceNode = brokerNode.Nodes.Add(name);
            serviceNode.BackColor = color;
            brokerNode.Expand();            
        }
        public void AddServiceInstanceNode(string name, Color color)
        {            
            instanceNode = serviceNode.Nodes.Add(name);
            instanceNode.BackColor = color;            
        }
        public void setCursor(string status)
        {
            if (status == "busy")
            {
                this.Cursor = Cursors.WaitCursor;
            }
            if (status == "ready")
            {
                this.Cursor = Cursors.Default;
            }
        }
        public void setLabels(string[] content)
        {
            string[] values = content;            
            this.siteNamel.Text = content[0];
            this.controllerCount.Text = content[1];
            this.totalServiceInstances.Text = content[2];
            this.dbServer.Text = content[3];
            this.dbName.Text = content[4];
        }
        public void setLabel(string content, Label labelName)
        {
            labelName.Text = content;
        }
        public DialogResult popUp(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult dr = MessageBox.Show(message, title, buttons, icon);
            return dr;
        }
        public void BuildTree()
        {
            //Get SQL information
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            char[] csdelim = new char[] { '=', ';' };
            char[] sqldelim = new char[] { ',', '\\' };  
            string[] dbConn = Program.RunScript(Properties.Resources.GetDBConn, null).Split(csdelim, StringSplitOptions.RemoveEmptyEntries);
            if (dbConn.Length == 0)
            {
                MessageBox.Show(this, "This broker is not a member of a XenDesktop site. Please join a site and try again.", "Not connected to a site", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            string[] sqlServer = dbConn[1].Split(sqldelim, StringSplitOptions.RemoveEmptyEntries);
            treeView1.Invoke(new SetLabel(setLabel), new object[] { sqlServer[0].TrimStart(' '), dbServer });
            treeView1.Invoke(new SetLabel(setLabel), new object[] { dbConn[3].TrimStart(' '), dbName });
            string[] serviceNames = { "AD Identity Service", "Configuration Service", "Broker Service", "Host Service", "Machine Creation Service", "Machine Identity Service" };                                                    
                     
            int licServ = 0;         
            
            //Get site name
            string[] siteName = GetBrokerSite();
            treeView1.Invoke(new AddSiteNode(addSiteNode), new object[] { siteName[0] }); //add top node
            treeView1.Invoke(new SetLabel(setLabel), new object[] { siteName[0].ToString(), siteNamel });            

            //Get site brokers
            string[] brokers = GetBrokerControllers();
            List<string> badBrokers = new List<string>();
            List<string> goodBrokers = new List<string>();
            foreach (string broker in brokers)
            {                               
                bool reply = Ping(broker);
                if (!reply)
                {
                    badBrokers.Add(broker);
                }
                else
                {
                    goodBrokers.Add(broker);
                }
            }
            treeView1.Invoke(new CheckServices(checkServices), new object[] { serviceNames, goodBrokers });   
            //Get site service instances
            string[] services = GetConfigRegisteredServiceInstance();
            
            foreach (string service in services)
            {
                if (service.Contains("Licensing"))
                {
                    licServ++;
                }
            }                 

            int totalDDCs = brokers.Length;
            int totalServices = services.Length - licServ;
            int expected = (12 * totalDDCs);
                   
            //Check that all site service instances are registered; 12 per ddc
            Color siteColor = Color.White;            
            if (totalServices < (12 * totalDDCs))
            {
                siteColor = Color.IndianRed;
            }
            
            treeView1.Invoke(new NodeColor(setColor), new object[] { "site", siteColor });            
            treeView1.Invoke(new SetLabel(setLabel), new object[] { totalDDCs.ToString(), controllerCount });
            treeView1.Invoke(new SetLabel(setLabel), new object[] { totalServices.ToString() + " of " + expected, totalServiceInstances });
            

            //Routine to add a node for each DDC under the root node and populate services and registered service instances            
            foreach (string broker in brokers)
            {                
                int serviceCount = 0;
                bool brokerOnline = true;
                Color ddcColor = Color.White;
                if (goodBrokers.Contains(broker))
                {
                    ddcColor = Color.LightGreen;
                    brokerOnline = true;
                }
                if (badBrokers.Contains(broker))
                {
                    ddcColor = Color.Yellow;
                    brokerOnline = false;
                }
                foreach (string service in services) //Count the registered service instances per DDC
                {
                    if (service.IndexOf(broker, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        serviceCount++;
                    }
                }
                if (serviceCount < 12) //Color the node red if there are missing service instances
                {
                    ddcColor = Color.IndianRed;
                }

                treeView1.Invoke(new AddNode(AddBrokerNode), new object[] { broker, ddcColor});
                //mainNode.Nodes.Add(ddc); //Add the node                        

                // Declare a list for each service to collect service instances
                List<string> hostServices = new List<string>();
                List<string> brokerServices = new List<string>();
                List<string> adidentityServices = new List<string>();
                List<string> configServices = new List<string>();
                List<string> machineCreationServices = new List<string>();
                List<string> machineIdentityServices = new List<string>();

                // Add service instances to corresponding service list
                foreach (string service in services)
                {
                    if (service.IndexOf(broker, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        if (service.IndexOf("HostingUnit", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            hostServices.Add(service);
                        }
                        if (service.IndexOf("ConfigurationService", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            configServices.Add(service);
                        }
                        if (service.IndexOf("ADIDentity", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            adidentityServices.Add(service);
                        }
                        if (service.IndexOf("Broker", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            brokerServices.Add(service);
                        }
                        if (service.IndexOf("MachinePersonality", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            machineIdentityServices.Add(service);
                        }
                        if (service.IndexOf("DesktopUpdate", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            machineCreationServices.Add(service);
                        }
                    }
                }

                // Check each service for total instances, set node to LightGreen if good; IndianRed if bad
                Color serviceColor = Color.LightGreen;
                if (!brokerOnline)
                {
                    serviceColor = Color.Yellow;
                }
                Color adIdentityServiceBackColor = serviceColor;
                Color brokerServiceBackColor = serviceColor;
                Color hostServiceBackColor = serviceColor;
                Color configServiceBackColor = serviceColor;
                Color machineCreationServiceBackColor = serviceColor;
                Color machineIdentityServiceBackColor = serviceColor;
                if (adidentityServices.Count < 3)
                {
                    adIdentityServiceBackColor = Color.IndianRed;
                }                
                if (brokerServices.Count < 1)
                {
                    brokerServiceBackColor = Color.IndianRed;
                }                
                if (hostServices.Count < 3)
                {
                    hostServiceBackColor = Color.IndianRed;
                }                
                if (configServices.Count < 2)
                {
                    configServiceBackColor = Color.IndianRed;
                }                
                if (machineCreationServices.Count < 1)
                {
                    machineCreationServiceBackColor = Color.IndianRed;
                }                
                if (machineIdentityServices.Count < 2)
                {
                    machineIdentityServiceBackColor = Color.IndianRed;
                }                

                // Add each service node and corresponding service instances
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[0], adIdentityServiceBackColor });
                foreach (string service in adidentityServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[1], configServiceBackColor });
                foreach (string service in configServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[2], brokerServiceBackColor });
                foreach (string service in brokerServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }                
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[3], hostServiceBackColor });
                
                foreach (string service in hostServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[4], machineCreationServiceBackColor });
                foreach (string service in machineCreationServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }
                treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[5], machineIdentityServiceBackColor });
                foreach (string service in machineIdentityServices)
                {
                    treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                }
            }
            if (totalServices < expected)
            {
                string message;
                int missing = expected - totalServices;
                if (expected > 1)
                {
                    message = "Instances";
                }
                else
                {
                    message = "Instance";
                }
                List<string> args = new List<string>();
                args.Add(totalServices.ToString());
                args.Add(expected.ToString());
                args.Add(missing.ToString());
                args.Add(message);
                args.Add("site instances");                
                //serviceReset.RunWorkerAsync(args);                
            }            
        }
        
        public static bool Ping(string entry)
        {
            try
            {                
                var pingSender = new Ping();
                var options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "--------------------------------";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(entry, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void checkServices(string[] services, List<string> servers)
        {
            string citrix = "Citrix ";
            string[] serviceNames = services;
            string[] processNames = { "Citrix.ADIdentity.SdkWcfEndpoint", "Citrix.Configuration.SdkWcfEndpoint", "BrokerService", "Citrix.Host.SdkWcfEndpoint", "Citrix.MachineCreation.SdkWcfEndpoint", "Citrix.MachineIdentity.SdkWcfEndpoint" };
            
            int i;
            int o;

            foreach (string server in servers)
            {
                try
                {
                    Process[] running = Process.GetProcesses(server);
                    string[] runningProcs = new string[running.Length];
                    o = 0; i = 0;
                    string[] result = null;
                    foreach (Process proc in running)
                    {
                        runningProcs[o] = proc.ToString();
                        o++;
                    }
                    foreach (string process in processNames)
                    {
                        result = Array.FindAll(runningProcs, s => s.Contains(process));
                        if (result.Length == 0)
                        {
                            DialogResult d = MessageBox.Show(this, serviceNames[i].ToString() + " is not running on " + server + ". Attempt to start the service now?", serviceNames[i] + " not started!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            if (d == DialogResult.Yes)
                            {
                                ServiceController sc = new ServiceController(citrix + serviceNames[i], server);
                                Program.startService(sc);
                            }
                        }
                        i++;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, "Unable to connect to " + server + ". Exception: " + e, "Unable to connect to " + server, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public Process[] getServerProcesses(string serverName)
        {            
            return Process.GetProcesses(serverName);
        }
        public static string[] GetBrokerSite()
        {
            string[] poshSite = Program.RunScript(Properties.Resources.GetBrokersite, null).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return poshSite;
        }
        public static string[] GetBrokerControllers()
        {
            string[] brokers = Program.RunScript(Properties.Resources.GetBrokers, null).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return brokers;
        }
        public string[] GetConfigRegisteredServiceInstance()
        {
            string[] serviceinstances = Program.RunScript(Properties.Resources.GetServices, null).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);            
            return serviceinstances;
        }                           
        
        #endregion
        #region Workers

        private void buildTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ServiceProgressBar.Hide();
            this.Cursor = Cursors.Default;
            statusLabel.Hide();
            button1.Enabled = true;
            button2.Enabled = true;
        }
        private void buildTree_DoWork(object sender, DoWorkEventArgs e)
        {            
            BuildTree();
        }
        private void executePS(string scriptText, string param)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            // open it
            runspace.Open();
            if (param != null)
            {
                runspace.SessionStateProxy.SetVariable("var", param);                
            }            
            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);
            pipeline.Commands.Add("Out-String");
            // execute the script
            try
            {
                Collection<PSObject> results = pipeline.Invoke();
            }
            catch
            {
                
            }
        }        
        private void runScript_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Program.RunScript(Properties.Resources.TestInstance, nodeclicked.Text);
        }
        private void runScript_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            this.ServiceProgressBar.Visible = false;            
            this.statusLabel.Visible = false;
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            treeView1.Show();
            MessageBoxIcon mb = new MessageBoxIcon();
            string result = e.Result.ToString();
            if (result == "Responding\r\n") { mb = MessageBoxIcon.Information; }
            else { mb = MessageBoxIcon.Warning; }
            MessageBox.Show(this, nodeclicked.Text + " is " + e.Result.ToString(), "Service instance is " + result, MessageBoxButtons.OK, mb);
        }
        private void startupServiceResetPrompt(List<string> args)
        {
            DialogResult dr = MessageBox.Show(this, args[0] + " of " + args[1] + " service instances are registered. Would you like to reset all service instances now?", "Missing " + args[2] + " Service " + args[3] + "!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                serviceReset.RunWorkerAsync(args);                
            }
        }
        private void serviceReset_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.    
            string arg = e.Argument as string;       
            
            BackgroundWorker bwAsync = sender as BackgroundWorker;                
            if (arg == "site instances")
            {
                executePS(Properties.Resources.ResetInstances, null);
            }
            if (arg == "ddc instances")
            {
                executePS(Properties.Resources.resetbrokerserviceinstances, nodeclicked.Text);
            }
            
        }
        private void serviceReset_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.


            // Check to see if an error occurred in the
            // background process.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }

            // Check to see if the background process was cancelled.
            if (e.Cancelled)
            {
                MessageBox.Show("Canceled");
            }
            else
            {
                // Everything completed normally.
                // process the response using e.Result
                RefreshThread();
                statusLabel.Text = "Service instance registrations reset.";
                ServiceProgressBar.Hide();
            }
            ServiceProgressBar.Visible = false;
            TreeNode mainNode = new TreeNode();
            this.treeView1.Nodes.Add(mainNode);
            buildTree.RunWorkerAsync();   
        }
        private void restartService_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            BackgroundWorker bwAsync = sender as BackgroundWorker;            
            string hostName = nodeclicked.Parent.Text;
            string serviceName = "Citrix " + nodeclicked.Text;
            string arg = e.Argument as string;
            ServiceController service = new ServiceController(serviceName, hostName);
            if (arg == "restart")
            {
                Program.stopService(service);
                Program.startService(service);
            }
            if (arg == "start")
            {
                Program.startService(service);
            }
            if (arg == "stop")
            {
                Program.stopService(service);
                
            }                         
        }        
        private void restartService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;                
            }
            if (e.Cancelled)
            {
                MessageBox.Show("Canceled");                
            }
            else
            {
                // Everything completed normally.
                // process the response using e.Result
                statusLabel.Text = "Done.";
            }
            ServiceProgressBar.Visible = false;            
        }
        private void restartService_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ServiceProgressBar.Value = e.ProgressPercentage;
        }
        private void resetTopNode(string message)
        {            
            TreeNode mainNode = new TreeNode();
            mainNode.Text = message;
            this.treeView1.Nodes.Add(mainNode);
        }
        private void disableLogging_DoWork(object sender, DoWorkEventArgs e)
        {            
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string arg = e.Argument as string;
            Program.configLogging("disable", arg, "foo", nodeclicked.Parent.Text, "Citrix " + nodeclicked.Text, false);
        }
        private void disableLogging_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string server = nodeclicked.Parent.Text;
            string service = nodeclicked.Text;
            string[] hostName = server.Split('.');

            ServiceProgressBar.Visible = false;
            statusLabel.Visible = false;
            treeView1.Show();
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            MessageBox.Show(this, "Logging has been disabled for the " + service + " on " + hostName[0], "Logging disabled", MessageBoxButtons.OK);
        }
        private void RefreshThread()
        {
            Invoke(new RefreshDelegate(BuildTree));
        }
        public delegate void RefreshDelegate();  

        #endregion
#region Event Handlers
        // Form Buttons and Load Events
        private void Form1_Load(object sender, EventArgs e)
        {            
            resetTopNode("Loading...");            
            button1.Enabled = false;
            button2.Enabled = false;
            ServiceProgressBar.Visible = true;
            this.statusLabel.Text = "Gathering site details...";
            this.statusLabel.Show();
            buildTree.RunWorkerAsync();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.TopNode.Remove();
            resetTopNode("Refreshing...");
            button1.Enabled = false;
            button2.Enabled = false;    
            ServiceProgressBar.Visible = true;            
            this.statusLabel.Text = "Gathering site details...";
            this.statusLabel.Show();
            buildTree.RunWorkerAsync();
        }        
        private void button2_Click(object sender, EventArgs e)
        {
            ServiceProgressBar.Show();
            treeView1.TopNode.Remove();
            button1.Enabled = false;
            button2.Enabled = false;
            TreeNode mainNode = new TreeNode();
            mainNode.Text = "Resetting Service Instances...";
            this.statusLabel.Text = "Resetting all site service instances...";
            this.statusLabel.Show();
            serviceReset.RunWorkerAsync("all services");                          
        }
        
        // TreeView context menu event handlers
        private void siteAction1_Click(object sender, EventArgs e)
        {
            detailView siteDetail = new detailView();
            siteDetail.Show();
        }
        private void siteAction2_Click(object sender, EventArgs e)
        {
            treeView1.TopNode.Remove();       
            Thread thdWorker = new Thread(new ThreadStart(BuildTree));
            thdWorker.Start();
        }
        private void ddcAction1_Click(object sender, EventArgs e)
        {
            controllerDetail ddcDetail = new controllerDetail();
            ddcDetail.selectedNode = nodeclicked.Text;
            ddcDetail.Show();
        }
        private void serviceAction1_Click(object sender, EventArgs e)
        {            
        }        
        private void serviceInstanceAction1_Click(object sender, EventArgs e)
        {
            this.ServiceProgressBar.Visible = true;
            this.statusLabel.Text = "Testing " + nodeclicked.Text;
            this.statusLabel.Visible = true;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            treeView1.Hide();
            runScript.RunWorkerAsync();
        }        
        private void resetServices_Click(object sender, EventArgs e)
        {

        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //restart service    
            this.ServiceProgressBar.Visible = true;
            this.statusLabel.Text = "Restarting the " + nodeclicked.Text;
            this.statusLabel.Visible = true;
            if (serviceRestart.IsBusy)
            {                
                serviceRestart.CancelAsync();
            }
            else
            {
                // Kickoff the worker thread to begin it's DoWork function.
                serviceRestart.RunWorkerAsync("restart");
            }
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //start service    
            this.ServiceProgressBar.Visible = true;
            this.statusLabel.Text = "Starting " + nodeclicked.Text;
            this.statusLabel.Visible = true;
            if (serviceRestart.IsBusy)
            {               
                serviceRestart.CancelAsync();
            }
            else
            {                
                serviceRestart.RunWorkerAsync("start");                
            }
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //stop service    
            this.ServiceProgressBar.Visible = true;
            this.statusLabel.Text = "Stopping " + nodeclicked.Text;
            this.statusLabel.Visible = true;
            if (serviceRestart.IsBusy)
            {
                serviceRestart.CancelAsync();
            }
            else
            {
                serviceRestart.RunWorkerAsync("stop");
            }
        }
        private void enableLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //enable logging            
            string service = "Citrix " + nodeclicked.Text;
            logging enablelogging = new logging();
            enablelogging.server = nodeclicked.Parent.Text;
            enablelogging.service = service;
            enablelogging.Show();
        }
        private void disableLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string server = nodeclicked.Parent.Text;
            string service = nodeclicked.Text;                
            string fullPath = Program.fullPath(server, service);            
            string configFile = Program.configFile("Citrix " + service, fullPath);
            string fqdn = Program.GetFQDN();
            string[] hostName = server.Split('.');   
                          
            bool exists = File.Exists(configFile);
            if (exists)
            {
                string configBackup = configFile + ".backup";
                try { File.Copy(configFile, configBackup); }  catch { }
            }
            DialogResult d = MessageBox.Show(this, "This operation will modify " + configFile + ", and restart the " + service + " on " + hostName[0] + " to disable logging. Would you like to continue?", "Update config file and restart the " + service + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.Yes)
            {
                //enableLogging 
                ServiceProgressBar.Show();
                statusLabel.Text = "Disabling logging...";                
                statusLabel.Show();
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                treeView1.Hide();
                disableLogging.RunWorkerAsync(configFile);                               
            }                    
            if (d == DialogResult.No)
            {
                return;
            }         
        }        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Point toolTipPoint = this.PointToClient(Cursor.Position);
            //toolTipPoint.Y -= 20;            
        }
        private void ddcAction2_Click(object sender, EventArgs e)
        {            
            if (serviceReset.IsBusy)
            {                
                serviceReset.CancelAsync();
            }
            else
            {
                // Kickoff the worker thread to begin it's DoWork function.
                ServiceProgressBar.Show();
                string[] ddc = nodeclicked.Text.Split(new char[] { '.' });
                statusLabel.Text = "Resetting service instances on " + ddc[0];
                statusLabel.Show();
                nodeclicked.Collapse();
                //resetTopNode("Resetting service instances..");
                serviceReset.RunWorkerAsync("ddc instances");
            }
        }
#endregion     
    }
}