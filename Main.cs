using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace XDSiteChecker
{
    public partial class Main : Form
    {
        #region Threads
        private BackgroundWorker serviceRestart = new BackgroundWorker();
        private BackgroundWorker serviceReset = new BackgroundWorker();
        private BackgroundWorker disableLogging = new BackgroundWorker();
        private BackgroundWorker buildTree = new BackgroundWorker();
        private BackgroundWorker nodeClick = new BackgroundWorker();
        private BackgroundWorker runScript = new BackgroundWorker();
        private BackgroundWorker getDesktopStats = new BackgroundWorker();
        private BackgroundWorker workProgress = new BackgroundWorker();
        private BackgroundWorker populateTree = new BackgroundWorker();
        private BackgroundWorker powerAction = new BackgroundWorker();
        private BackgroundWorker siteEnum = new BackgroundWorker();
        private TreeNode lastNodeClicked = new TreeNode();
        public delegate void AddNode(string name, Color color);
        public delegate void AddCatalogNodes(string name, bool power);
        public delegate void PowerAction(string action, string target);
        public delegate void AddNodeTo(string name, TreeNode node);
        public delegate void ClearLogText();
        public delegate void AddNodes(TreeNode node, string name, TreeNode parent, int imageindex);
        public delegate void AddToSelectedNode(TreeViewCollection _nodes);
        public delegate void RemoveNodes(TreeNode node);
        public delegate void AddNodes2(TreeNode node, string name, TreeNode parent, int imageindex);
        public delegate void NodeColor(string node, Color color);
        public delegate void SetLabels(string[] content);
        public delegate void SetLabel(string content, Label labelName);
        public delegate void CheckServices(string[] services, List<string> servers, string[] processes);
        public delegate void AddSiteNode(string name);
        public delegate void AddImage(Bitmap icon, string name);
        public delegate void AddSiteNodeIcon(string name, int iconIndex);
        public delegate void AddTreeNodes(string name);
        public delegate void ScriptDelegate(string script, string param);
        public delegate void SetCursor(string status);
        public delegate void ResetTextBox();
        public delegate void SetNode(string info, string cmd);
        public delegate void SetNodeInfo(string[] info);
        public delegate void RefreshDelegate(string args);
        public delegate void LogText(string message);
        public delegate void ProgressBars(bool status, string message);
        #endregion
        #region Inits
        public Main()
        {
            InitializeComponent();
            Font font = new Font(this.treeView1.Font, FontStyle.Bold);
            treeView1.Font = font;
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
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
            nodeClick.WorkerSupportsCancellation = true;
            runScript.DoWork += new DoWorkEventHandler(runScript_DoWork);
            runScript.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runScript_RunWorkerCompleted);
            siteEnum.DoWork += new DoWorkEventHandler(siteEnum_DoWork);
            siteEnum.RunWorkerCompleted += new RunWorkerCompletedEventHandler(siteEnum_RunWorkerCompleted);
            nodeClick.DoWork += new DoWorkEventHandler(nodeClick_DoWork);
            nodeClick.RunWorkerCompleted += new RunWorkerCompletedEventHandler(nodeClick_RunWorkerCompleted);
            nodeClick.WorkerSupportsCancellation = true;
            getDesktopStats.DoWork += new DoWorkEventHandler(getDesktopStats_DoWork);
            getDesktopStats.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getDesktopStats_RunWorkerCompleted);
            workProgress.DoWork += new DoWorkEventHandler(workProgress_DoWork);
            workProgress.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workProgress_RunWorkerCompleted);
            workProgress.WorkerSupportsCancellation = true;
            populateTree.DoWork += new DoWorkEventHandler(populateTree_DoWork);
            populateTree.RunWorkerCompleted += new RunWorkerCompletedEventHandler(populateTree_RunWorkerCompleted);
            populateTree.WorkerSupportsCancellation = true;
            powerAction.DoWork += new DoWorkEventHandler(powerAction_DoWork);
            powerAction.RunWorkerCompleted += new RunWorkerCompletedEventHandler(powerAction_RunWorkerCompleted);
            powerAction.WorkerSupportsCancellation = true;
        }
        #endregion
        #region Variables
        List<assignmentDesktopList> _desktops = new List<assignmentDesktopList>();
        List<siteDetails> _siteInfo = new List<siteDetails>();
        List<serviceGroupNames> _badServiceGroup = new List<serviceGroupNames>();
        List<brokerDetails> _brokerInfo = new List<brokerDetails>();
        List<brokerDetails> _appIcons = new List<brokerDetails>();
        List<brokerDetails> _desktopInfo = new List<brokerDetails>();
        List<provTask> _provTasks = new List<provTask>();
        List<string> badBrokers = new List<string>();
        public static bool runlocal = true;
        Size oldSize;
        public static string targetDDC = "localhost";
        public string[] RunPSScript(string script, string[] parms, string delimiter)
        {
            if (parms != null)
            {
                if (parms.Length == 1)
                {
                    treeView1.Invoke(new LogText(logText), new object[] { "$var1 = " + "'" + parms[0] + "'" });
                }
                if (parms.Length > 1)
                {
                    int i = 1;
                    foreach (string parm in parms)
                    {
                        treeView1.Invoke(new LogText(logText), new object[] { "$var" + i + " = " + "'" + parm + "'" });
                        i++;
                    }
                }
            }
            treeView1.Invoke(new LogText(logText), new object[] { script });
            string results = RunScript(script, parms);
            string[] resultArray = results.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            return resultArray;
        }
        string[] storages;
        string[] pvdStorages;
        string[] hostingUnits;
        string[] brokers;
        string[] services;
        string[] DBSchema;
        string[] connectionStrings;
        string[] rhoneserviceNames = { "Citrix AD Identity Service", "Citrix Broker Service", "Citrix Configuration Service", "Citrix Host Service", "Citrix Machine Creation Service", "Citrix Machine Identity Service" };
        string[] rhonebrokerProcesses = { "Citrix.ADIdentity.SdkWcfEndpoint", "BrokerService", "Citrix.Configuration.SdkWcfEndpoint", "Citrix.Host.SdkWcfEndpoint", "Citrix.MachineCreation.SdkWcfEndpoint", "Citrix.MachineIdentity.SdkWcfEndpoint" };
        string[] jasperserviceNames = { "Citrix AD Identity Service", "Citrix Broker Service", "Citrix Configuration Logging Service", "Citrix Configuration Service", "Citrix Delegated Administration Service", "Citrix Environment Test Service", "Citrix Host Service", "Citrix Machine Creation Service", "Citrix Monitor Service", "Citrix StoreFront Service" };
        string[] jasperbrokerProcesses = { "Citrix.ADIdentity", "Citrix.EnvTest", "Citrix.Monitor", "BrokerService", "Citrix.ConfigurationLogging", "Citrix.ConfigurationService", "Citrix.DelegatedAdmin", "Citrix.Host", "Citrix.MachineCreation", "Citrix.StoreFront" };
        string[] serviceNames;
        string[] brokerProcesses;
        string[] vdaservices = { "WorkstationAgent", "PorticaService", "CitrixCseEngine" };
        string[] vdaProcesses = { "WorkstationAsitegent", "PicaSvc", "CitrixCseEngine" };
        string[] statuses = { "Completed", "Running", "Terminated" };
        string[] siteDetailsLabel = { "Site Name", "Database Server", "Database Name", "Site Version", "# of Controllers", "Service Instances" };
        string siteVersion = "5.6";
        private string controllerVersion()
        {
            string[] parms = new string[] { targetDDC };
            string[] version = RunScript(Properties.Resources.ControllerVersion, parms).Split(csdelim, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            foreach (string value in version)
            {
                builder.Append(value);
                builder.Append('.');
            }
            siteVersion = builder.ToString();
            return siteVersion;
        }
        char[] csdelim = new char[] { '=', ';', '.' };
        public string[] siteName;
        public string[] dbConn()
        {
            string[] parms = new string[] { targetDDC };
            treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.GetDBConn });
            string[] dbConn = RunScript(Properties.Resources.GetDBConn, parms).Split(csdelim, StringSplitOptions.RemoveEmptyEntries);
            return dbConn;
        }
        string fqdn = Program.GetFQDN();
        private string RunScript(string scriptText, string[] param)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            // open it
            //Invoke(new LogText(scriptText));
            runspace.Open();
            if (param != null)
            {
                if (param.Length == 1)
                {
                    runspace.SessionStateProxy.SetVariable("var1", param[0]);
                }
                if (param.Length > 1)
                {
                    int i = 1;
                    string arg;
                    foreach (string parm in param)
                    {
                        arg = "var" + i;
                        runspace.SessionStateProxy.SetVariable(arg, parm);
                        i++;
                    }
                }
            }
            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);
            pipeline.Commands.Add("Out-String");
            // execute the script
            try
            {
                Collection<PSObject> results = pipeline.Invoke();
                // return results as string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject obj in results)
                {
                    stringBuilder.AppendLine(obj.ToString());
                }
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                treeView1.Invoke(new LogText(logText), new object[] { e.StackTrace.ToString() });
                return "Not Found";
            }
        }

        int expected;

        bool needsRefresh = false;
        // working on organizing this mess..
        TreeNode TopNode; //top
        TreeNode serviceNode; TreeNode MCSNode; TreeNode desktopGroupsNode; // Level 1
        TreeNode brokerNode; TreeNode hostingUnitNode; TreeNode desktopGroupNode; TreeNode catalogViewNode; // Level 2
        TreeNode activeSiteServiceNode; TreeNode instanceNode; TreeNode preferredHostingUnitNode; // Level 3
        TreeNode serviceRegistrationNode; TreeNode acctIdentityPoolNode;
        TreeNode activeSiteServicesBrokerNode;
        TreeNode assignments;
        TreeNode applications;
        TreeNode applicationsNode;
        TreeNode applicationInstance;
        TreeNode activeServicesNode;
        TreeNode hostingUnitDetailsNode;
        TreeNode storageNode;
        TreeNode storageViewNode;
        TreeNode networkNode;
        TreeNode networkViewNode;
        TreeNode tasksViewNode;
        TreeNode tasksNode;
        TreeNode taskNode;
        TreeNode acctIdentities;
        TreeNode serviceGroupNode;
        TreeNode catalogNode;
        TreeNode pvdViewNode;
        TreeNode pvdNode;
        TreeNode configNode;
        TreeNode provSchemeViewNode;
        TreeNode provSchemeNode;
        TreeNode unregistered;
        TreeNode registered;
        TreeNode desktopNode;
        TreeNode onNode;
        TreeNode offNode;
        TreeNode powerActions;
        TreeNode runningPowerActions;
        TreeNode failedPowerActions;
        TreeNode runningPowerAction;
        TreeNode failedPowerAction;
        TreeNode sessionsNode;
        TreeNode activeSessionNode;
        TreeNode inactiveSessionNode;
        TreeNode activeSessionsNode;
        TreeNode inactiveSessionsNode;

        #endregion
        #region Functions
        // Main Site build
        public bool buildSite(bool isnew, string[] dbconn)
        {
            //build site
            string[] parms = new string[] { targetDDC };
            Invoke(new SetLabel(setLabel), new object[] { "Building Site Node..", statusLabel });
            //connectionStrings = getRegValue("localhost", "\\SOFTWARE\\Citrix\\XDservices\\ADIdentitySchema\\DataStore\\Connections", "ConnectionString");
            brokers = RunPSScript(Properties.Resources.GetBrokers, parms, "\r\n");
            expected = expected * brokers.Length;
            services = RunPSScript(Properties.Resources.GetServices, parms, "\r\n");
            _siteInfo.Clear();
            siteName = RunPSScript(Properties.Resources.GetBrokersite, parms, "\r\n");
            treeView1.Invoke(new AddSiteNode(addSiteNode), new object[] { siteName[0] }); //add top node                             
            int licServ = 0;
            foreach (string service in services)
            {
                if (service.Contains("Licensing"))
                {
                    licServ++;
                }
            }
            if (dbconn.Length == 0)
            {
                MessageBox.Show(this, "This broker is not a member of a XenDesktop site. Please join a site and try again.", "Not connected to a site", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            if (siteName.Contains("Not Found"))
            {

            }
            int totalServices = services.Length - licServ;
            //string[] sqlServer;
            //char[] sqldelim = new char[] { ',', '\\' };
            //sqlServer = dbconn[1].Split(sqldelim, StringSplitOptions.RemoveEmptyEntries);
            //}
            //else
            //{
            //    char[] sqldelim = new char[] { '.' };
            //    sqlServer = dbconn[1].Split(sqldelim, StringSplitOptions.RemoveEmptyEntries);
            //}
            string[] siteDetailsValues = new string[] { siteName[0].ToString(), dbconn[1].TrimStart(' '), dbconn[3].TrimStart(' '), siteVersion, brokers.Length.ToString(), totalServices.ToString() + " of " + expected };
            int x = 0;
            foreach (string label in siteDetailsLabel)
            {
                _siteInfo.Add(new siteDetails { detailLabel = label, detailValue = siteDetailsValues[x] });
                x++;
            }
            if (totalServices < expected)
            {
                DialogResult result = MessageBox.Show(this, "Attempt to register all service instances now?", "Warning! Only " + totalServices.ToString() + " of " + expected + " service instances found.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    this.Invoke(new LogText(logTextWarning), new object[] { "Warning! Only " + totalServices.ToString() + " of " + expected + " service instances found, attempting to resolve by registering all service instances." });
                    this.Invoke(new SetLabel(setLabel), new object[] { "Warning! Only " + totalServices.ToString() + " of " + expected + " service instances registered. Registering all service instances..", statusLabel });
                    //serviceReset.RunWorkerAsync("site instances");
                    treeView1.Invoke(new LogText(logText), new object[] { "Attempting to register missing service instances" });
                    executePS(Properties.Resources.ResetInstances, null);
                    return false;
                }
                //}
            }
            return true;
        }
        public bool buildServices(bool isnew)
        {
            this.Invoke(new SetLabel(setLabel), new object[] { "Building Configuration Node..", statusLabel });
            if (!isnew)
            {
                string[] parms = new string[] { targetDDC };
                brokers = RunPSScript(Properties.Resources.GetBrokers, parms, "\r\n");
                services = RunPSScript(Properties.Resources.GetServices, parms, "\r\n");
            }
            treeView1.Invoke(new AddSiteNode(AddConfigNode), new object[] { "Configuration" });
            treeView1.Invoke(new AddSiteNode(AddServiceRegistrationNode), new object[] { "Controllers" });
            treeView1.Invoke(new AddSiteNode(AddPreferredHostingUnitNode), new object[] { "Hosts" });
            //treeView1.Invoke(new AddSiteNode(AddServiceGroupNode), new object[] { "Service Groups" });            

            List<KeyValuePair<string, string>> brokerSiteServicesKVP = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> hostingUnitPreference = new List<KeyValuePair<string, string>>();
            List<string> goodBrokers = new List<string>();
            List<string> badServiceGroupName = new List<string>();
            this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating Site Services..", statusLabel });
            int totalDDCs = brokers.Length;
            int licServ = 0;
            foreach (string service in services)
            {
                if (service.Contains("Licensing"))
                {
                    licServ++;
                }
            }
            int totalServices = services.Length - licServ;
            int instances = 0;
            if (siteVersion.Contains("5"))
            {
                instances = 12;
            }
            if (siteVersion.Contains("7"))
            {
                //calculate the expected amount of service instances
                instances = 43;
            }
            int expected = (instances * totalDDCs);

            this.Invoke(new SetLabel(setLabel), new object[] { "Checking DDCs..", statusLabel });
            foreach (string broker in brokers)
            {
                if (!broker.Contains(fqdn) && runlocal)
                {
                    treeView1.Invoke(new LogText(logText), new object[] { "Attempting to ping '" + broker + "'" });
                    bool reply = Ping(broker);
                    if (!reply)
                    {
                        treeView1.Invoke(new LogText(logText), new object[] { "Unable to ping '" + broker + "'!" });
                        badBrokers.Add(broker);
                    }
                    else
                    {
                        treeView1.Invoke(new LogText(logText), new object[] { "Successfully pinged '" + broker + "'." });
                        goodBrokers.Add(broker);
                    }
                }
            }
            goodBrokers.Add(fqdn);
            foreach (string broker in goodBrokers)
            {
                if (!broker.Contains(fqdn))
                {
                    treeView1.Invoke(new CheckServices(checkServices), new object[] { serviceNames, goodBrokers, brokerProcesses });
                }
            }

            //Check that all site service instances are registered; 12 per ddc
            Color siteColor = Color.White;
            if (totalServices < (instances * totalDDCs))
            {
                siteColor = Color.IndianRed;
            }
            treeView1.Invoke(new NodeColor(setColor), new object[] { "site", siteColor });

            //Get hosting units and create nodes


            //Routine to add a node for each DDC under the service management node and populate services and registered service instances   
            this.Invoke(new SetLabel(setLabel), new object[] { "Building Services Nodes..", statusLabel });
            foreach (string broker in brokers)
            {
                //treeView1.Invoke(new AddSiteNode(AddActiveSiteBrokersNode), new object[] { broker });
                int serviceCount = 0;
                bool brokerOnline = true;
                Color ddcColor = Color.White;
                if (goodBrokers.Contains(broker) | goodBrokers.Contains(fqdn))
                {
                    ddcColor = Color.Azure;// .AliceBlue;
                    brokerOnline = true;
                }
                if (badBrokers.Contains(broker))
                {
                    ddcColor = Color.IndianRed;
                    brokerOnline = false;
                }
                foreach (string service in services) //Count the registered service instances per DDC
                {
                    if (service.IndexOf(broker, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        serviceCount++;
                    }
                }
                if (serviceCount < instances) //Color the node red if there are missing service instances
                {
                    ddcColor = Color.IndianRed;
                }
                treeView1.Invoke(new AddNode(AddBrokerNode), new object[] { broker, ddcColor });
                treeView1.Invoke(new AddSiteNode(AddActiveSiteServicesNode), new object[] { "Site Roles" });
                //mainNode.Nodes.Add(ddc); //Add the node                        

                // Declare a list for each service to collect service instances
                List<string> hostServices = new List<string>();
                List<string> brokerServices = new List<string>();
                List<string> adidentityServices = new List<string>();
                List<string> configServices = new List<string>();
                List<string> machineCreationServices = new List<string>();
                List<string> machineIdentityServices = new List<string>();
                List<string> configLoggingServices = new List<string>();
                List<string> delegatedAdminServices = new List<string>();
                List<string> storeFrontServices = new List<string>();
                List<string> envTestServices = new List<string>();
                List<string> monitorServices = new List<string>();
                string[] activeSiteServices;

                treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.GetBrokerController });
                string[] serviceParm = new string[] { broker, targetDDC };
                activeSiteServices = RunScript(Properties.Resources.GetBrokerController, serviceParm).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string service in activeSiteServices)
                {
                    brokerSiteServicesKVP.Add(new KeyValuePair<string, string>(broker, service));
                }

                // Add service instances to corresponding service list
                foreach (string instance in services)
                {
                    string[] service = instance.Split(',');
                    if (service[1].IndexOf(broker, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        if (service[0] == "Hyp")
                        {
                            hostServices.Add(service[1]);
                        }
                        if (service[0] == "Config")
                        {
                            configServices.Add(service[1]);
                        }
                        if (service[0] == "Log")
                        {
                            configLoggingServices.Add(service[1]);
                        }
                        if (service[0] == "Acct")
                        {
                            adidentityServices.Add(service[1]);
                        }
                        if (service[0] == "Broker")
                        {
                            brokerServices.Add(service[1]);
                        }
                        if (service[0] == "PvsVm")
                        {
                            machineIdentityServices.Add(service[1]);
                        }
                        if (service[0] == "Prov")
                        {
                            machineCreationServices.Add(service[1]);
                        }
                        if (service[0] == "Admin")
                        {
                            delegatedAdminServices.Add(service[1]);
                        }
                        if (service[0] == "Monitor")
                        {
                            monitorServices.Add(service[1]);
                        }
                        if (service[0] == "EnvTest")
                        {
                            envTestServices.Add(service[1]);
                        }
                        if (service[0] == "Sf")
                        {
                            storeFrontServices.Add(service[1]);
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
                Color configLoggingServiceBackColor = serviceColor;
                Color delegatedAdminServiceBackColor = serviceColor;
                Color envTestServiceBackColor = serviceColor;
                Color monitorServiceBackColor = serviceColor;
                Color storeFrontServiceBackColor = serviceColor;
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
                if (siteVersion.Contains('5'))
                {
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
                }
                if (siteVersion.Contains("7.0"))
                {
                    if (configServices.Count < 3)
                    {
                        configServiceBackColor = Color.IndianRed;
                    }
                    if (machineCreationServices.Count < 2)
                    {
                        machineCreationServiceBackColor = Color.IndianRed;
                    }
                    if (configLoggingServices.Count < 2)
                    {
                        configLoggingServiceBackColor = Color.IndianRed;
                    }
                    if (delegatedAdminServices.Count < 3)
                    {
                        delegatedAdminServiceBackColor = Color.IndianRed;
                    }
                    if (delegatedAdminServices.Count < 3)
                    {
                        delegatedAdminServiceBackColor = Color.IndianRed;
                    }
                    if (envTestServices.Count < 2)
                    {
                        envTestServiceBackColor = Color.IndianRed;
                    }
                    if (monitorServices.Count < 5)
                    {
                        monitorServiceBackColor = Color.IndianRed;
                    }
                    if (storeFrontServices.Count < 3)
                    {
                        storeFrontServiceBackColor = Color.IndianRed;
                    }
                }
                // Add each service node and corresponding service instances

                foreach (KeyValuePair<string, string> service in brokerSiteServicesKVP)
                {
                    if (service.Key.Contains(broker))
                    {
                        treeView1.Invoke(new AddSiteNode(AddActiveServicesNode), new object[] { service.Value.ToString() });
                    }
                }
                if (siteVersion.Contains('5'))
                {
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[0], adIdentityServiceBackColor });
                    foreach (string service in adidentityServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[1], brokerServiceBackColor });
                    foreach (string service in brokerServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[2], configServiceBackColor });
                    foreach (string service in configServices)
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
                if (siteVersion.Contains("7.0"))
                {
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[0], adIdentityServiceBackColor });
                    foreach (string service in adidentityServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[1], brokerServiceBackColor });
                    foreach (string service in brokerServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[2], configLoggingServiceBackColor });
                    foreach (string service in configLoggingServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[3], configServiceBackColor });
                    foreach (string service in configServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[4], delegatedAdminServiceBackColor });
                    foreach (string service in delegatedAdminServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[7], machineCreationServiceBackColor });
                    foreach (string service in machineCreationServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[6], hostServiceBackColor });
                    foreach (string service in hostServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[5], hostServiceBackColor });
                    foreach (string service in envTestServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[8], hostServiceBackColor });
                    foreach (string service in monitorServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                    treeView1.Invoke(new AddNode(AddServiceNode), new object[] { serviceNames[9], hostServiceBackColor });
                    foreach (string service in storeFrontServices)
                    {
                        treeView1.Invoke(new AddNode(AddServiceInstanceNode), new object[] { service, Color.White });
                    }
                }
            }
            return true;
        }
        public bool buildMCS(bool isnew)
        {
            _provTasks.Clear();
            this.Invoke(new SetLabel(setLabel), new object[] { "Building Machines Node..", statusLabel });
            treeView1.Invoke(new AddSiteNode(AddMCSNode), new object[] { "Machines" });
            treeView1.Invoke(new AddSiteNode(AddAcctIdentityPoolNode), new object[] { "AD Identity Pools" });
            treeView1.Invoke(new AddSiteNode(AddCatalogViewNode), new object[] { "Catalogs" });
            treeView1.Invoke(new AddSiteNode(AddPowerActionNode), new object[] { "Power Actions" });
            treeView1.Invoke(new AddSiteNode(AddRunningPowerActionsNode), new object[] { "Active Power Actions" });
            treeView1.Invoke(new AddSiteNode(AddFailedPowerActionsNode), new object[] { "Completed Power Actions" });
            treeView1.Invoke(new AddSiteNode(AddProvSchemeViewNode), new object[] { "Provisioning Schemes" });
            treeView1.Invoke(new AddSiteNode(AddTasksViewNode), new object[] { "Provisioning Tasks" });
            return true;
        }
        public bool buildAssignments(bool isnew)
        {
            string[] parms = new string[] { targetDDC };
            this.Invoke(new SetLabel(setLabel), new object[] { "Building Assignments Node..", statusLabel });
            treeView1.Invoke(new AddSiteNode(AddAssignmentsNode), new object[] { "Assignments" });
            if (siteVersion.Contains("7"))
            {
                treeView1.Invoke(new AddSiteNode(AddDesktopGroupsNode), new object[] { "Delivery Groups" });
                treeView1.Invoke(new AddSiteNode(AddAppsGroupsNode), new object[] { "Applications" });
                string[] appNames = RunPSScript(Properties.Resources.GetBrokerAppNames, parms, "\r\n");
                foreach (string application in appNames)
                {
                    Bitmap bmpReturn = null;
                    string[] parms2 = new string[] { targetDDC, application };
                    string[] iconData = RunPSScript(Properties.Resources.GetAppIcon, parms2, "\r\n");
                    byte[] byteBuffer = Convert.FromBase64String(iconData[0]);
                    MemoryStream memoryStream = new MemoryStream(byteBuffer);
                    memoryStream.Position = 0;
                    bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
                    treeView1.Invoke(new AddImage(AddAppIcon), new object[] { bmpReturn, application });
                    byteBuffer = null;
                    memoryStream.Close();
                    memoryStream = null;
                    treeView1.Invoke(new AddNodes(AddChildNode), new object[] { applications, application, applicationsNode, 0 });
                }
            }
            else { treeView1.Invoke(new AddSiteNode(AddDesktopGroupsNode), new object[] { "Desktop Groups" }); }
            treeView1.Invoke(new AddSiteNode(AddSessionsNode), new object[] { "Sessions" });
            treeView1.Invoke(new AddSiteNode(AddActiveSessionsNode), new object[] { "Active Sessions" });
            treeView1.Invoke(new AddSiteNode(AddInactiveSessionsNode), new object[] { "Inactive Sessions" });
            string[] groupNames = RunPSScript(Properties.Resources.GetBrokerDesktopGroupNames, parms, "\r\n");
            foreach (string group in groupNames)
            {
                treeView1.Invoke(new AddSiteNode(AddDesktopGroupNode), new object[] { group });
            }
            return true;
        }
        // Regex Search 
        private void btnNodeTextSearch_Click(object sender, EventArgs e)
        {
            ClearBackColor();
            FindByText();
        }
        private void FindByText()
        {
            TreeNodeCollection nodes = treeView1.Nodes;
            foreach (TreeNode n in nodes)
            {
                FindRecursive(n);
            }
        }
        private void ClearBackColor()
        {
            TreeNodeCollection nodes = treeView1.Nodes;
            foreach (TreeNode n in nodes)
            {
                ClearRecursive(n);
            }
        }
        private void ClearRecursive(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                tn.BackColor = Color.White;
                ClearRecursive(tn);
            }
        }
        private void FindRecursive(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                // if the text properties match, color the item
                if (Regex.Match(tn.Text, txtNodeTextSearch.Text, RegexOptions.IgnoreCase).Success)
                {
                    tn.BackColor = Color.Yellow;
                    if (!tn.Parent.IsExpanded)
                    {
                        if (tn.Parent.Parent != null)
                        {
                            if (!tn.Parent.Parent.IsExpanded)
                            {
                                if (tn.Parent.Parent.Parent != null)
                                {
                                    if (!tn.Parent.Parent.Parent.IsExpanded)
                                    {
                                        tn.Parent.Parent.Parent.Expand();
                                    }
                                }
                            }
                            tn.Parent.Parent.Expand();
                        }
                        tn.Parent.Expand();
                    }
                }
                FindRecursive(tn);
            }
        }
        // TreeNodes
        public void addSiteNode(string name)
        {
            treeView1.TopNode.Remove();
            TopNode = treeView1.Nodes.Add(name);
            TopNode.ImageIndex = 24;
        }
        public void AddAssignmentsNode(string name)
        {
            assignments = TopNode.Nodes.Add(name);
            assignments.ImageIndex = 10;
            //desktopGroupsNode.Expand();
        }
        public void AddChildNode(TreeNode node, string name, TreeNode parent, int imageindex)
        {
            node = parent.Nodes.Add(name);
            foreach (brokerDetails app in _appIcons)
            {
                if (app.detailLabel.Contains(name))
                {
                    node.ImageIndex = Convert.ToInt16(app.detailValue);
                }
            }
        }
        public int AddAppIcon(string name)
        {
            Bitmap bmpReturn = null;
            string[] parms = new string[] { targetDDC, name };
            string[] iconData = RunPSScript(Properties.Resources.GetAppIcon, parms, "\r\n");
            byte[] byteBuffer = Convert.FromBase64String(iconData[0]);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
            this.imageList2.Images.Add(bmpReturn);
            byteBuffer = null;
            memoryStream.Close();
            memoryStream = null;
            return imageList2.Images.Count - 1;
        }
        public void AddAppInstance(TreeNode node, string name, TreeNode parent, int imageindex)
        {
            applicationInstance = parent.Nodes.Add(name);
            applicationInstance.ImageIndex = imageindex;
        }
        public void AddDesktopGroupsNode(string name)
        {
            desktopGroupsNode = assignments.Nodes.Add(name);
            desktopGroupsNode.ImageIndex = 45;
            //desktopGroupsNode.Expand();
        }
        public void AddAppsGroupsNode(string name)
        {
            applicationsNode = assignments.Nodes.Add(name);
            applicationsNode.ImageIndex = 2;
        }
        public void AddSessionsNode(string name)
        {
            sessionsNode = assignments.Nodes.Add(name);
            sessionsNode.ImageIndex = 43;
        }
        public void AddActiveSessionsNode(string name)
        {
            activeSessionsNode = sessionsNode.Nodes.Add(name);
            activeSessionsNode.ImageIndex = 40;
        }
        public void AddInactiveSessionsNode(string name)
        {
            inactiveSessionsNode = sessionsNode.Nodes.Add(name);
            inactiveSessionsNode.ImageIndex = 41;
        }
        public void AddActiveSessionNode(string name)
        {
            activeSessionNode = activeSessionsNode.Nodes.Add(name);
            activeSessionNode.ImageIndex = 39;
        }
        public void AddInactiveSessionNode(string name)
        {
            inactiveSessionNode = inactiveSessionsNode.Nodes.Add(name);
            inactiveSessionNode.ImageIndex = 42;
            //desktopGroupsNode.Expand();
        }
        public void AddAppIcon(Bitmap icon, string name)
        {
            imageList2.Images.Add(icon);
            try { _appIcons.Add(new brokerDetails { detailLabel = name, detailValue = (imageList2.Images.Count - 1).ToString() }); }//add the pair of values to the node detail list }
            catch { }
        }
        public void AddDesktopGroupNode(string name)
        {
            desktopGroupNode = desktopGroupsNode.Nodes.Add(name);
            desktopGroupNode.ImageIndex = 11;
            desktopGroupNode.Tag = "Click to enumerate desktops";
            //desktopGroupsNode.Expand();
        }
        public void AddRegisteredNode(string name)
        {
            registered = nodeclicked.Nodes.Add(name);
            registered.ImageIndex = 31;
            nodeclicked.Expand();
        }
        public void AddCatalogMachineNode(string name, int iconIndex)
        {
            catalogNode = nodeclicked.Nodes.Add(name);
            catalogNode.ImageIndex = iconIndex;
            nodeclicked.Expand();
        }
        public void AddUnRegisteredNode(string name)
        {
            unregistered = nodeclicked.Nodes.Add(name);
            unregistered.ImageIndex = 30;
            nodeclicked.Expand();
        }
        public void AddDesktopNode(string name)
        {
            desktopNode = nodeclicked.Nodes.Add(name);
            desktopNode.ImageIndex = nodeclicked.ImageIndex;
            nodeclicked.Expand();
        }
        public void AddTreeNode(TreeNode node, string name, TreeNode parent, int imageIndex)
        {
            node = parent.Nodes.Add(name);
            node.ImageIndex = imageIndex;
        }
        public void AddMCSNode(string name)
        {
            MCSNode = TopNode.Nodes.Add(name);
            MCSNode.ImageIndex = 28;
            TopNode.Expand();
        }
        public void AddPowerActionNode(string name)
        {
            powerActions = MCSNode.Nodes.Add(name);
            powerActions.ImageIndex = 37;
            //desktopGroupsNode.Expand();
        }
        public void AddRunningPowerActionsNode(string name)
        {
            runningPowerActions = powerActions.Nodes.Add(name);
            runningPowerActions.ImageIndex = 36;
            //desktopGroupsNode.Expand();
        }
        public void AddFailedPowerActionsNode(string name)
        {
            failedPowerActions = powerActions.Nodes.Add(name);
            failedPowerActions.ImageIndex = 38;
            //desktopGroupsNode.Expand();
        }
        public void AddRunningPowerActionNode(string name)
        {
            runningPowerAction = runningPowerActions.Nodes.Add(name);
            runningPowerAction.ImageIndex = 36;
            //desktopGroupsNode.Expand();
        }
        public void AddFailedPowerActionNode(string name)
        {
            failedPowerAction = failedPowerActions.Nodes.Add(name);
            failedPowerAction.ImageIndex = 38;
            if (name.Contains("Failed"))
            {
                failedPowerAction.BackColor = Color.BurlyWood;
            }
            if (name.Contains("Canceled"))
            {
                failedPowerAction.BackColor = Color.Yellow;
            }
            if (name.Contains("Started"))
            {
                failedPowerAction.BackColor = Color.LightGreen;
            }
            if (name.Contains("Deleted"))
            {
                failedPowerAction.BackColor = Color.Orange;
            }
            if (name.Contains("Completed"))
            {
                failedPowerAction.BackColor = Color.LightGreen;
            }
        }
        public void OnNode(string name, TreeNode parent)
        {
            onNode = parent.Nodes.Add(name);
            onNode.ImageIndex = 28;
        }
        public void OffNode(string name, TreeNode parent)
        {
            offNode = parent.Nodes.Add(name);
            offNode.ImageIndex = 28;
        }
        public void AddAcctIdentityPoolNode(string name)
        {
            acctIdentityPoolNode = MCSNode.Nodes.Add(name);
            acctIdentityPoolNode.ImageIndex = 10;
        }
        public void AddCatalogViewNode(string name)
        {
            catalogViewNode = MCSNode.Nodes.Add(name);
            catalogViewNode.ImageIndex = 23;

            //preferredHostingUnitNode.Expand();
        }
        public void AddPreferredHostingUnitNode(string name)
        {
            preferredHostingUnitNode = configNode.Nodes.Add(name);
            preferredHostingUnitNode.ImageIndex = 7;
            //TopNode.Expand();
        }
        public void AddTasksViewNode(string name)
        {
            tasksViewNode = MCSNode.Nodes.Add(name);
            tasksViewNode.ImageIndex = 18;
        }
        public void AddProvSchemeViewNode(string name)
        {
            provSchemeViewNode = MCSNode.Nodes.Add(name);
            provSchemeViewNode.ImageIndex = 18;
        }
        public void AddProvSchemeNode(string name)
        {
            provSchemeNode = provSchemeViewNode.Nodes.Add(name);
            provSchemeNode.ImageIndex = 18;
        }
        public void AddAcctIdentitiesNode(string name)
        {
            acctIdentities = acctIdentityPoolNode.Nodes.Add(name);
            acctIdentities.ImageIndex = 11;
        }
        public void progressBar(bool status, string message)
        {
            this.toolStripProgressBar1.Visible = status;
            this.statusLabel.Text = message;
            if (status == true)
            {
                this.statusLabel.Show();
            }
            if (status == false)
            {
                this.statusLabel.Hide();
            }
        }
        private void resetTopNode(string message)
        {
            TreeNode mainNode = new TreeNode();
            mainNode.Text = message;
            this.treeView1.Nodes.Add(mainNode);
        }
        public void logText(string message)
        {
            this.logTextBox.Text = logTextBox.Text + "\r\n" + message;
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }
        public void logTextWarning(string message)
        {
            int start = logTextBox.TextLength;
            Font font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            Color color = Color.Red;
            this.logTextBox.Text = logTextBox.Text + "\r\n" + message;
            int end = logTextBox.TextLength;
            logTextBox.Select(start, end - start);
            {
                logTextBox.SelectionColor = color;
                logTextBox.SelectionFont = font;
            }
            //logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }
        private void RefreshThread()
        {
            Invoke(new RefreshDelegate(BuildTree), new object[] { "all" });
        }
        public void setNode(string info, string cmd)
        {
            if (cmd == "rt")
            {
                //nodeInfoRichTextBox.Text = info;
            }
            if (cmd == "ni")
            {

            }
        }
        public void resetTextBox()
        {

        }
        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append('.');
            }
            return builder.ToString();
        }
        public void AddServiceRegistrationNode(string name)
        {
            serviceRegistrationNode = configNode.Nodes.Add(name);
            serviceRegistrationNode.ImageIndex = 29;
            //TopNode.Expand();
        }
        public void AddConfigNode(string name)
        {
            configNode = TopNode.Nodes.Add(name);
            configNode.ImageIndex = 22;
            TopNode.Expand();
        }
        public void AddServiceGroupNode(string name)
        {
            serviceGroupNode = serviceRegistrationNode.Nodes.Add(name);
            serviceGroupNode.ImageIndex = 5;
            //TopNode.Expand();
        }
        public void AddActiveSiteBrokersNode(string name)
        {
            activeSiteServicesBrokerNode = activeSiteServiceNode.Nodes.Add(name);
            activeSiteServicesBrokerNode.ImageIndex = 8;
            //activeSiteServiceNode.Expand();
        }
        public void AddServiceNode(string name, Color color)
        {
            serviceNode = brokerNode.Nodes.Add(name);
            serviceNode.BackColor = color;
            serviceNode.ImageIndex = 6;
            //brokerNode.Expand();            
        }
        public void AddServiceInstanceNode(string name, Color color)
        {
            instanceNode = serviceNode.Nodes.Add(name);
            instanceNode.ImageIndex = 9;
            instanceNode.BackColor = color;
        }
        public void AddActiveSiteServicesNode(string name)
        {
            activeSiteServiceNode = brokerNode.Nodes.Add(name);
            activeSiteServiceNode.ImageIndex = 5;
            //TopNode.Expand();
        }
        public void AddActiveServicesNode(string name)
        {
            activeServicesNode = activeSiteServiceNode.Nodes.Add(name);
            activeServicesNode.ImageIndex = 6;
        }
        public void AddHostingUnitNode(string name)
        {
            hostingUnitNode = preferredHostingUnitNode.Nodes.Add(name);
            hostingUnitNode.ImageIndex = 14;
            //preferredHostingUnitNode.Expand();
        }
        public void AddCatalogNode(string name, bool power)
        {
            catalogNode = catalogViewNode.Nodes.Add(name);
            catalogNode.ImageIndex = 25;
            if (power == true)
            {
                catalogNode.Tag = "Power Managed";
            }
            //preferredHostingUnitNode.Expand();
        }
        public void AddHostingUnitDetailNode(string name)
        {
            hostingUnitDetailsNode = hostingUnitNode.Nodes.Add(name);
            hostingUnitDetailsNode.ImageIndex = 3;
        }
        public void AddStorageNode(string name, TreeNode node)
        {
            storageNode = node.Nodes.Add(name);
            storageNode.ImageIndex = 16;
        }
        public void AddStorageViewNode(string name)
        {
            storageViewNode = hostingUnitNode.Nodes.Add(name);
            storageViewNode.ImageIndex = 4;
        }
        public void AddPvdViewNode(string name)
        {
            pvdViewNode = hostingUnitNode.Nodes.Add(name);
            pvdViewNode.ImageIndex = 4;
        }
        public void AddPvdNode(string name, TreeNode node)
        {
            pvdNode = node.Nodes.Add(name);
            pvdNode.ImageIndex = 16;
        }
        public void AddNetworkNode(string name, TreeNode node)
        {
            networkNode = node.Nodes.Add(name);
            networkNode.ImageIndex = 13;
        }
        public void AddNetworkViewNode(string name)
        {
            networkViewNode = hostingUnitNode.Nodes.Add(name);
            networkViewNode.ImageIndex = 12;
        }
        public void AddTasksNode(string name)
        {
            tasksNode = tasksViewNode.Nodes.Add(name);
            int imageIndex = 0;
            switch (name)
            {
                case "Running":
                    imageIndex = 21;
                    break;
                case "Terminated":
                    imageIndex = 20;
                    break;
                case "Completed":
                    imageIndex = 19;
                    break;
            }
            tasksNode.ImageIndex = imageIndex;
        }
        public void AddTasks(string name)
        {
            taskNode = tasksNode.Nodes.Add(name);
            taskNode.ImageIndex = 17;
        }
        public void setColor(string node, Color color)
        {

        }
        public void AddBrokerNode(string name, Color color)
        {
            brokerNode = serviceRegistrationNode.Nodes.Add(name);
            brokerNode.BackColor = color;
            brokerNode.ImageIndex = 8;
            //serviceRegistrationNode.Expand();
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
        public void removeNodes(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Remove();
            }
        }
        public void clearLogtext()
        {
            this.logTextBox.Clear();
        }
        public void removeNode(TreeNode node)
        {
            node.Remove();
        }
        public void setLabel(string content, Label labelName)
        {
            labelName.Text = content;
            if (content.Contains("Warning") | content.Contains("Registering"))
            {
                labelName.ForeColor = Color.Red;
            }
            else
            {
                labelName.ForeColor = Color.Black;
            }
        }
        public class siteDetails
        {
            public string detailLabel { get; set; }
            public string detailValue { get; set; }
        }
        public class assignmentDesktopList
        {
            public string machineName { get; set; }
            public string dnsName { get; set; }
            public string registrationStatus { get; set; }
            public string powerStatus { get; set; }
            public string desktopGroupName { get; set; }
        }
        public class TreeViewCollection
        {
            public TreeNode newNode { get; set; }
            public TreeNode parentNode { get; set; }
            public string nodeName { get; set; }
            public int imageIndex { get; set; }
            public IEnumerator GetEnumerator()
            {
                yield return this.newNode;
                yield return this.parentNode;
                yield return this.nodeName;
                yield return this.imageIndex;
            }
        }
        public class provTask
        {
            public string taskID { get; set; }
            public string workFlowStatus { get; set; }
        }
        public class serviceGroupNames
        {
            public string serviceGroupName { get; set; }
            public string serviceInstanceUID { get; set; }
            public string serviceInstanceAddress { get; set; }
        }
        public class brokerDetails
        {
            public string detailLabel { get; set; }
            public string detailValue { get; set; }
        }
        public class nameIndexList
        {
            public string name { get; set; }
            public int indexValue { get; set; }
        }
        public Process[] getServerProcesses(string serverName)
        {
            return Process.GetProcesses(serverName);
        }
        public static string getRegValue(string hostname, string keyName, string valueName)
        {
            try
            {
                RegistryKey reg = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostname);
                return reg.OpenSubKey(keyName).GetValue(valueName).ToString();
            }
            catch
            {
                return "Not Found";
            }
        }
        #endregion
        #region Workers
        void siteEnum_DoWork(object sender, DoWorkEventArgs e)
        {
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating Site..", statusLabel });
            string[] parms = new string[] { targetDDC };
            string[] results = RunPSScript(Properties.Resources.EnumSite, parms, null);
            this.Invoke(new ClearLogText(clearLogtext));
            foreach (string result in results)
            {
                treeView1.Invoke(new LogText(logText), new object[] { result });
            }
        }
        void siteEnum_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string filename = string.Format("XDSC-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            File.WriteAllText(filename, logTextBox.Text.Replace("\n", Environment.NewLine), Encoding.Unicode);
            System.Diagnostics.Process.Start(filename);
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "ready" });
            this.Invoke(new SetLabel(setLabel), new object[] { "", statusLabel });
            this.statusLabel.Hide();
            //throw new NotImplementedException();
        }
        void nodeClick_DoWork(object sender, DoWorkEventArgs arg)
        {
            TreeNodeMouseClickEventArgs e = arg.Argument as TreeNodeMouseClickEventArgs;
            string[] target = new string[] { targetDDC };
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            if (lastNodeClicked.Text != e.Node.Text && lastNodeClicked.Text != null && e.Node != null && e.Node == nodeclicked)
            {
                //_brokerInfo.Clear();                
                //nodeInfoListView.HideSelection = false;
                if (e.Node.Level == 0)
                {
                    string[] siteDetails = RunPSScript(Properties.Resources.GetBrokersiteFull, target, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { siteDetails });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.SiteInfo, "rt" });
                }
                if (e.Node.Level == 1)
                {
                    if (e.Node.Text.Contains("Service"))
                    {
                        //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.ServicesInfo, "rt" });
                    }
                }

                if (e.Node.Level == 2)
                {
                    if (e.Node.Text.Contains("Active Site"))
                    {
                        //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.SiteServices, "rt" });
                    }
                    if (e.Node.Text.Contains("Host"))
                    {
                        //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                    }

                }
            }
            if (e.Node.Level == 3)
            {
                if (e.Node.Parent.Text.Contains("Desktop Groups") | e.Node.Parent.Text.Contains("Delivery Group")) //Check if the clicked node is a hosting unit
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), targetDDC };
                    string[] desktopGroupDetail = RunPSScript(Properties.Resources.GetBrokerDesktopGroup, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { desktopGroupDetail });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }
                if (e.Node.Parent.Text.Contains("Controllers"))// | e.Node.Parent.Text.Contains("Service")) //Check if the clicked node is a DDC
                {
                    if (!badBrokers.Contains(e.Node.Text))
                    {
                        //treeView1.Invoke(new LogText(logText), new object[] { "$var = " + "'" + e.Node.Text + "'" });
                        //treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.GetBroker });
                        string[] parms = new string[] { e.Node.Text.Trim(), e.Node.Parent.Text, targetDDC };
                        string[] controllerDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                        treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { controllerDetails });
                    }
                    if (e.Node.Parent.Text.Contains("Group"))
                    {
                        //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.ServicesInfo, "rt" });
                    }
                    if (e.Node.Parent.Text.Contains("Active"))
                    {
                        //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.SiteServices, "rt" });
                    }
                }
                if (e.Node.Parent.Text.Contains("Hosts")) //Check if the clicked node is a hosting unit
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), targetDDC };
                    string[] hostingUnits = RunPSScript(Properties.Resources.GetBrokerHypervisorConnection, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { hostingUnits });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }
                if (e.Node.Parent.Text.Contains("Catalogs")) //Check if the clicked node is a catalog
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), targetDDC };
                    string[] catalogDetail = RunPSScript(Properties.Resources.GetBrokerCatalog, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { catalogDetail });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }

                if (e.Node.Parent.Text.Contains("AD Identity")) //Check if the clicked node is ADIdentity scheme
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), targetDDC };
                    string[] identityPool = RunPSScript(Properties.Resources.GetAcctIdentityPoolDetails, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { identityPool });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }
                if (e.Node.Parent.Text.Contains("Schemes")) //Check if the clicked node is a provisioning scheme
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), targetDDC };
                    string[] scheme = RunPSScript(Properties.Resources.GetProvScheme, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { scheme });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }
                if (e.Node.Parent.Text.Contains("Applications"))
                {
                    string[] parms = new string[] { nodeclicked.Text, "application", targetDDC };
                    string[] appDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { appDetails });
                }
            }

            if (e.Node.Level == 4) //Check clicked node for service or active site service 
            {
                if (e.Node.Parent.Parent.Text.Contains("Provisioning"))
                {
                    string[] parms = new string[] { e.Node.Text, targetDDC };
                    string[] provTaskDetails = RunPSScript(Properties.Resources.GetProvTask, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { provTaskDetails });
                }
                if (e.Node.Parent.Parent.Text.Contains("ions"))
                {
                    string[] split = e.Node.Text.Split(':');
                    string[] split2 = split[1].Split(' ');
                    string[] parms = new string[] { split2[0], e.Node.Parent.Parent.Text, targetDDC };
                    string[] sessionDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { sessionDetails });
                }
                if (e.Node.Parent.Parent.Text.Contains("Catalogs"))
                {
                    string[] parms = new string[] { e.Node.Text, e.Node.Parent.Parent.Text, targetDDC };
                    string[] machineDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { machineDetails });
                }
                if (e.Node.Parent.Parent.Parent.Text.Contains("Hosting"))
                {
                    string[] parms = new string[] { e.Node.Parent.Parent.Text.Trim(), targetDDC };
                    string[] hostingUnits = RunPSScript(Properties.Resources.GetBrokerHypervisorConnection, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { hostingUnits });
                    //treeView1.Invoke(new SetNode(setNode), new object[] { Properties.Resources.HostServiceInfo, "rt" });
                }
                if (e.Node.Parent.Parent.Text.Contains("Controllers") && !e.Node.Text.Contains("Site Roles")) //Add the site services and roles descriptions
                {
                    string[] parms = new string[] { e.Node.Parent.Text.Trim(), e.Node.Text, targetDDC };
                    string[] serviceInstanceDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { serviceInstanceDetails });
                }
                if (e.Node.Parent.Parent.Text.Contains("Power")) //Add the site services and roles descriptions
                {
                    string[] splitDetails = nodeclicked.Text.Split(':');
                    string[] parms = new string[] { splitDetails[1], "Power", targetDDC };
                    string[] serviceInstanceDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { serviceInstanceDetails });
                }
            }
            if (e.Node.Level == 5) //Check clicked node for service instance
            {
                if (e.Node.Parent.Parent.Parent.Text.Contains("Groups"))
                {
                    string[] parms = new string[] { e.Node.Text.Trim(), e.Node.Parent.Parent.Parent.Parent.Text, targetDDC };
                    string[] desktopDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { desktopDetails });
                }
                if (e.Node.Text.Contains("http"))
                {
                    string[] parms = new string[] { e.Node.Text, "ServiceInstance", targetDDC };
                    string[] serviceInstanceDetails = RunPSScript(Properties.Resources.GetObject, parms, "\r\n");
                    treeView1.Invoke(new SetNodeInfo(setNodeInfo), new object[] { serviceInstanceDetails });
                }
            }
            lastNodeClicked = e.Node;
        }
        void nodeClick_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            nodeInfoListView.AutoResizeColumn(0,
                ColumnHeaderAutoResizeStyle.HeaderSize);
            nodeInfoListView.AutoResizeColumn(1,
                ColumnHeaderAutoResizeStyle.ColumnContent);
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "ready" });
        }
        void populateTree_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] arg = e.Argument as string[];
            this.Invoke(new ProgressBars(progressBar), new object[] { true, null });
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            string[] parms = new string[] { targetDDC };
            switch (arg[0])
            {
                case "Configuration":
                    this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating Hosts..", statusLabel });
                    hostingUnits = RunPSScript(Properties.Resources.GetHostsByName, parms, "\r\n");
                    storages = RunPSScript(Properties.Resources.GetStorages, parms, "\r\n");
                    foreach (string unit in hostingUnits)
                    {
                        foreach (TreeNode node in nodeexpanded.Nodes)
                        {
                            if (node.Text.Contains(unit))
                            {
                                node.Remove();
                            }
                        }
                        treeView1.Invoke(new AddSiteNode(AddHostingUnitNode), new object[] { unit });
                        treeView1.Invoke(new AddSiteNode(AddStorageViewNode), new object[] { "VM Storage" });
                        if (siteVersion.Contains("5.6"))
                        {
                            treeView1.Invoke(new AddSiteNode(AddPvdViewNode), new object[] { "PVD Storage" });
                        }
                        treeView1.Invoke(new AddSiteNode(AddNetworkViewNode), new object[] { "Network" });
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating storages and networks..", statusLabel });

                    foreach (TreeNode confignodes in nodeexpanded.Nodes)
                    {
                        if (confignodes.Text.Contains("Hosts"))
                        {
                            foreach (TreeNode node in confignodes.Nodes)
                            {
                                foreach (string storage in storages)
                                {
                                    string[] storagesplit = storage.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (storagesplit[2] == node.Text)
                                    {
                                        foreach (TreeNode childnode in node.Nodes)
                                        {
                                            if (childnode.Text == "VM Storage")
                                            {
                                                string[] store = storagesplit[3].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                                treeView1.Invoke(new AddNodeTo(AddStorageNode), new object[] { storagesplit[3], childnode });
                                            }
                                        }
                                    }
                                }
                                if (siteVersion.Contains("5.6"))
                                {

                                    pvdStorages = RunPSScript(Properties.Resources.GetPvdStorages, parms, "\r\n");
                                    foreach (string storage in pvdStorages)
                                    {
                                        string[] storagesplit = storage.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (storagesplit[2] == node.Text)
                                        {
                                            string[] store = storagesplit[3].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (TreeNode childnode in node.Nodes)
                                            {
                                                if (childnode.Text == "PVD Storage")
                                                {
                                                    treeView1.Invoke(new AddNodeTo(AddPvdNode), new object[] { storagesplit[3], childnode });
                                                }
                                            }
                                        }
                                    }
                                }
                                string[] networks = RunPSScript(Properties.Resources.GetNetworks, parms, "\r\n");
                                foreach (string network in networks)
                                {
                                    string[] networksplit = network.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (networksplit[2] == node.Text)
                                    {
                                        foreach (TreeNode childnode in node.Nodes)
                                        {
                                            if (childnode.Text == "Network")
                                            {
                                                string[] store = networksplit[3].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                                treeView1.Invoke(new AddNodeTo(AddNetworkNode), new object[] { store[0], childnode });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Sessions":
                    var activeSessions = RunPSScript(Properties.Resources.GetActiveSessions, null, "\r\n");
                    string[] inactiveSessions = RunPSScript(Properties.Resources.GetInactiveSessions, null, "\r\n");
                    if (activeSessions.Length > 0)
                    {
                        if (activeSessionsNode.Nodes.Count > 0)
                        {
                            var addedSessions = new string[activeSessionsNode.Nodes.Count];
                            int index = 0;
                            foreach (TreeNode node in activeSessionsNode.Nodes)
                            {
                                addedSessions[index] = node.Text;
                                index++;
                            }
                            var removeSessions = addedSessions.Except(activeSessions);
                            var newSessions = activeSessions.Except(addedSessions);
                            foreach (var session in removeSessions)
                            {
                                foreach (TreeNode node in activeSessionsNode.Nodes)
                                {
                                    if (node.Text == session.ToString())
                                    {
                                        treeView1.Invoke(new RemoveNodes(removeNode), new object[] { node });
                                    }
                                }
                            }
                            foreach (var session in newSessions)
                            {
                                treeView1.Invoke(new AddSiteNode(AddActiveSessionNode), new object[] { session.ToString() });
                            }
                        }
                        else
                        {
                            foreach (string session in activeSessions)
                            {
                                treeView1.Invoke(new AddSiteNode(AddActiveSessionNode), new object[] { session });
                            }
                        }
                    }
                    if (inactiveSessions.Length > 0)
                    {
                        if (inactiveSessionsNode.Nodes.Count > 0)
                        {
                            treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { inactiveSessionsNode });
                        }
                    }
                    foreach (string session in inactiveSessions)
                    {
                        treeView1.Invoke(new AddSiteNode(AddInactiveSessionNode), new object[] { session });
                    }
                    break;
                case "Applications":
                    string[] activeAppSessions = RunPSScript(Properties.Resources.GetAppInstances, null, "\r\n");
                    if (activeAppSessions.Length > 0)
                    {
                        foreach (TreeNode node in applicationsNode.Nodes)
                        {
                            foreach (string session in activeAppSessions)
                            {
                                string[] split = session.Split(',');
                                if (node.Text.Contains(split[0]))
                                {
                                    //TreeNode node, string name, TreeNode parent, int imageindex
                                    int imageindex = 42;
                                    if (split[1].Contains("Active"))
                                    {
                                        imageindex = 39;
                                    }
                                    treeView1.Invoke(new AddNodes(AddAppInstance), new object[] { node, split[1], node, imageindex });
                                }
                            }
                        }
                    }
                    break;
                case "Hosts":
                    break;
                case "Machines":
                    this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating Machine Details..", statusLabel });
                    string[] details = RunPSScript(Properties.Resources.GetMachineInfo, parms, "\r\n");
                    List<string> tasks = new List<string>();// = RunPSScript(Properties.Resources.GetProvTasks, null, "\r\n");
                    List<string> schemes = new List<string>();// = RunPSScript(Properties.Resources.GetProvSchemes, null, "\r\n");
                    List<string> catalogs = new List<string>();// = RunPSScript(Properties.Resources.GetBrokerCatalogs, null, "\r\n"); 
                    List<string> acctPools = new List<string>();
                    foreach (string detail in details)
                    {
                        string[] detailSplit = detail.Split(':');
                        switch (detailSplit[0])
                        {
                            case "ProvTask":
                                tasks.Add(detailSplit[1]);
                                break;
                            case "ProvScheme":
                                schemes.Add(detailSplit[1]);
                                break;
                            case "Catalogs":
                                catalogs.Add(detailSplit[1]);
                                break;
                            case "ADIDPool":
                                acctPools.Add(detailSplit[1]);
                                break;
                        }
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Adding Catalogs..", statusLabel });
                    foreach (string catalog in catalogs)
                    {
                        string[] catSplit = catalog.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                        bool powerManaged = true;
                        if (catSplit.Length > 1)
                        {
                            if (catSplit[1].Contains("Unmanaged"))
                            {
                                powerManaged = false;
                            }
                        }
                        else { powerManaged = false; }

                        treeView1.Invoke(new AddCatalogNodes(AddCatalogNode), new object[] { catSplit[0], powerManaged });
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Adding Provisioning Tasks..", statusLabel });
                    foreach (string task in tasks)
                    {
                        string[] taskSplit = task.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                        _provTasks.Add(new provTask { taskID = taskSplit[0], workFlowStatus = taskSplit[1] });
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Adding Provisioning Schemes..", statusLabel });
                    foreach (string scheme in schemes)
                    {
                        treeView1.Invoke(new AddSiteNode(AddProvSchemeNode), new object[] { scheme });

                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Adding AD Identity Pools..", statusLabel });
                    foreach (string identity in acctPools)
                    {
                        treeView1.Invoke(new AddNodes(AddTreeNode), new object[] { this.acctIdentities, identity, this.acctIdentityPoolNode, 11 });
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Sorting Provisioning Tasks..", statusLabel });
                    foreach (string status in statuses)
                    {
                        treeView1.Invoke(new AddSiteNode(AddTasksNode), new object[] { status });
                        foreach (provTask task in _provTasks)
                        {
                            if (task.workFlowStatus == status)
                            {
                                treeView1.Invoke(new AddSiteNode(AddTasks), new object[] { task.taskID });
                            }
                        }
                    }
                    //e.Result = "Catalogs";
                    break;
                case "AD Identity Pools":
                    break;
                case "Catalogs":
                    break;
                case "Provisioning Schemes":
                    break;
                case "Provisioning Tasks":
                    break;
                case "Assignments:":
                    break;
                case "Active Power Actions":
                    treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { runningPowerActions });
                    Thread.Sleep(100);
                    string[] pendingPowerActions = RunPSScript(Properties.Resources.GetPendingPowerActions, parms, "\r\n");
                    if (runningPowerActions.Nodes.Count > 0)
                    {
                        foreach (TreeNode node in runningPowerActions.Nodes)
                        {
                            treeView1.Invoke(new RemoveNodes(removeNode), new object[] { node });
                        }
                    }
                    foreach (string action in pendingPowerActions)
                    {
                        treeView1.Invoke(new AddSiteNode(AddRunningPowerActionNode), new object[] { action });
                    }
                    break;
                case "Power Actions":
                    treeView1.Invoke(new AddSiteNode(AddRunningPowerActionNode), new object[] { "Active" });
                    treeView1.Invoke(new AddSiteNode(AddFailedPowerActionNode), new object[] { "0:0:0:Completed" });
                    break;
                case "Completed Power Actions":
                    treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { failedPowerActions });
                    Thread.Sleep(100);
                    while (failedPowerActions.Nodes.Count > 1)
                    {
                        treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { failedPowerActions });
                    }
                    string[] completePowers = RunPSScript(Properties.Resources.GetCompletePowerActions, parms, "\r\n");
                    //completePowers = completePowers.OrderByDescending(d => d).ToArray();
                    //if (failedPowerActions.Nodes.Count > 0)
                    //{
                    //    foreach (TreeNode node in failedPowerActions.Nodes)
                    //    {
                    //        treeView1.Invoke(new RemoveNodes(removeNode), new object[] { node });
                    //    }
                    //}
                    foreach (string action in completePowers)
                    {
                        treeView1.Invoke(new AddSiteNode(AddFailedPowerActionNode), new object[] { action });
                    }
                    break;
            }
            e.Result = arg[0];
        }
        void populateTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                string arg = e.Result.ToString();
                //cleanups here
                if (e.Result.ToString() == "Catalogs")
                {
                    catalogViewNode.Expand();
                }
                if (e.Result.ToString().Contains("Power"))
                {
                    nodeclicked.Expand();
                }
            }
            setCursor("ready");
            toolStripProgressBar1.Visible = false;
            this.statusLabel.Hide();
        }
        void workProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new ProgressBars(progressBar), new object[] { true, null });
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            string message = "";
            string[] arg = e.Argument as string[];
            string script = "";
            string action = "";
            string[] parms = new string[10];
            int i = 1;

            while (i > 0 && i < arg.Length)
            {
                parms[i - 1] = arg[i];
                i++;
            }
            switch (arg[0])
            {
                case "removeStorage":
                    message = "Removing Storage..";
                    treeView1.Invoke(new LogText(logText), new object[] { "Removing '" + parms[1] + "' from '" + parms[0] + "'\r\n" + "$var1='" + parms[0] + "'; $var2='" + parms[1] + "'" });
                    if (nodeclicked.Parent.Text.Contains("PVD"))
                    {
                        script = Properties.Resources.RemovePvdStorage;
                    }
                    else
                    {
                        script = Properties.Resources.RemoveStorage;
                    }
                    action = "Machines";
                    break;
                case "checkService":
                    message = "Checking desktop service status for unregistered workers in " + arg[1].ToString() + "..";
                    script = Properties.Resources.GetUnregisteredPoweredOn;
                    action = arg[0];
                    break;
                case "checkAssignmentRegistration":
                    message = "Checking registration status for " + arg[1].ToString() + "..";
                    script = Properties.Resources.GetBrokerDesktopGroupRegistrations;
                    action = arg[0];
                    break;
                case "enumWorkers":
                    treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { nodeclicked });
                    message = "Getting desktops in " + arg[1].ToString() + " Assignment..";
                    if (nodeclicked.Text == "Registered")
                    {
                        script = Properties.Resources.GetRegisteredAssignmentDesktops;
                    }
                    if (nodeclicked.Text == "Un-Registered" | nodeclicked.Text.Contains("Never"))
                    {
                        script = Properties.Resources.GetUnregisteredAssignmentDesktop;
                    }
                    action = "getAssignmentDesktops";
                    break;
                case "enumCatalogWorkers":
                    treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { nodeclicked });
                    message = "Getting desktops in " + arg[1].ToString() + " Catalog..";
                    script = Properties.Resources.GetCatalogDesktop;
                    action = arg[0];
                    break;
                case "removeTask":
                    message = "Removing ProvTask..";
                    script = Properties.Resources.RemoveProvTask;
                    action = "Machines";
                    break;
                case "removeTasks":
                    if (arg[1] == "Completed")
                    {
                        message = "Removing All Completed Provisioning Tasks..";
                        script = Properties.Resources.RemoveProvTasks;
                    }
                    if (arg[1] == "Terminated")
                    {
                        message = "Removing All Terminated Provisioning Tasks..";
                        script = Properties.Resources.RemoveProvTasks;
                    }
                    action = "Machines";
                    break;
                case "stopTasks":
                    message = "Stopping All Running Provisioning Tasks..";
                    script = Properties.Resources.RemoveProvTasks;
                    action = "Machines";
                    break;
                case "stopTask":
                    message = "Stopping ProvTask..";
                    script = Properties.Resources.StopProvTask;
                    action = "Machines";
                    break;
                case "enableMaint":
                    message = "Enabling Maintenance Mode..";
                    script = Properties.Resources.EnableMaintDG;
                    action = "Assignments";
                    break;
                case "disableMaint":
                    message = "Disabling Maintenance Mode..";
                    script = Properties.Resources.DisableMaintDG;
                    action = "Assignments";
                    break;
                case "enableCatalogMaint":
                    message = "Enabling Maintenance Mode..";
                    script = Properties.Resources.EnableMaintCatalog;
                    action = "Catalogs";
                    break;
                case "disableCatalogMaint":
                    message = "Disabling Maintenance Mode..";
                    script = Properties.Resources.DisableMaintCatalog;
                    action = "Catalogs";
                    break;
                case "evictScript":
                    message = "Generating Evict Script..";
                    script = Properties.Resources.EvictBroker;
                    action = "Evict";
                    break;
            }

            this.Invoke(new SetLabel(setLabel), new object[] { message, this.statusLabel });
            treeView1.Invoke(new LogText(logText), new object[] { script });
            if (arg[2] == null)
            {
                string delim = "";
                if (action == "getAssignmentDesktops" | action == "checkService" | action == "enumCatalogWorkers")
                {
                    delim = "\r\n";
                }
                string[] parm = new string[] { arg[1], targetDDC };
                string[] results = RunPSScript(script, parm, delim);
                treeView1.Invoke(new LogText(logText), new object[] { results[0] });
                if (action == "checkService" && results[0] != "\r\n")
                {
                    List<string> machines = new List<string> { };
                    foreach (string result in results)
                    {
                        machines.Add(result);
                    }
                    treeView1.Invoke(new CheckServices(checkServices), new object[] { vdaservices, machines, vdaProcesses });
                }
                if (action == "enumCatalogWorkers")
                {
                    if (nodeclicked.Nodes.Count > 0)
                    {
                        treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { nodeclicked });
                    }
                    foreach (string machine in results)
                    {
                        int iconIndex = 33;
                        string[] detail = machine.Split(':');
                        if (detail[1] == "Off")
                        {
                            iconIndex = 34;
                        }
                        if (detail[1] == "On")
                        {
                            iconIndex = 32;
                        }
                        treeView1.Invoke(new AddSiteNodeIcon(AddCatalogMachineNode), new object[] { detail[0], iconIndex });
                    }
                }
                if (action == "checkAssignmentRegistration")
                {
                    string[] details = results[0].Split(':');
                    int total = Convert.ToInt16(details[1]);
                    int never = Convert.ToInt16(details[5]);
                    int totalUnreg = Convert.ToInt16(details[3]);
                    if (nodeclicked.Nodes.Count > 0)
                    {
                        treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { nodeclicked });
                    }
                    if (totalUnreg > 0)
                    {
                        treeView1.Invoke(new AddSiteNode(AddUnRegisteredNode), new object[] { "Un-Registered" });
                    }
                    if (total != totalUnreg && never != total)
                    {
                        treeView1.Invoke(new AddSiteNode(AddRegisteredNode), new object[] { "Registered" });
                    }
                    if (total == never)
                    {
                        treeView1.Invoke(new AddSiteNode(AddUnRegisteredNode), new object[] { "Never Registered" });
                    }
                }
                if (action == "getAssignmentDesktops")
                {
                    treeView1.Invoke(new RemoveNodes(removeNodes), new object[] { nodeclicked });

                    foreach (string desktop in results)
                    {
                        string[] details = desktop.Split(':');
                        if (details.Length > 1)
                        {
                            //_desktops.Add(new assignmentDesktopList { machineName = details[0], dnsName = details[1], registrationStatus = details[2], powerStatus = details[3], desktopGroupName = details[4] });
                        }
                        treeView1.Invoke(new AddSiteNode(AddDesktopNode), new object[] { details[0] });
                    }
                }
            }
            if (arg[2] != null)
            {
                //treeView1.Invoke(new LogText(logText), new object[] { "$var1='" + parms[0] + "'; $var2='" + parms[1] + "'; \r\n" + script });
                Program.RunScript2(script, parms);
            }
            e.Result = action;
        }
        void workProgress_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string arg = e.Result.ToString();
            if (arg.Contains("Evict"))
            {
                System.Diagnostics.Process.Start("evictscript.txt");
            }
            if (arg == "CheckUnregistered" | arg == "getAssignmentDesktops" | arg == "checkAssignmentRegistration" | arg == "enumCatalogWorkers" | arg.Contains("Maint"))
            {
                toolStripProgressBar1.Visible = false;
                this.statusLabel.Hide();
            }
            else
            {
                refreshTreeview(arg);
            }
            //toolStripProgressBar1.Invoke(new ProgressBars(progressBar), new object[] { false, null });
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "ready" });
            ////this.button1.Enabled = true;
            ////this.button2.Enabled = true;
        }
        void getDesktopStats_DoWork(object sender, DoWorkEventArgs e)
        {
            _desktopInfo.Clear();
            treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.GetBrokerDesktop });
            string[] parms = new string[] { targetDDC };
            string[] desktops = RunScript(Properties.Resources.GetBrokerDesktop, parms).Replace("\r\n", " ").Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string element in desktops)
            {
                if (element.Contains(":"))
                {
                    string[] detail = element.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries); //split at the first :
                    if (detail[1].Trim().Length > 0)
                    {
                        try { _desktopInfo.Add(new brokerDetails { detailLabel = detail[0].Trim(), detailValue = detail[1].Trim() }); }//add the pair of values to the node detail list }
                        catch { }
                    }
                }
            }
        }
        void getDesktopStats_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (brokerDetails detail in _desktopInfo)
            {
                ListViewItem lvi = new ListViewItem(detail.detailLabel);
                lvi.SubItems.Add(detail.detailValue);
                siteInfoListView.Items.Add(lvi);
            }
            siteInfoListView.AutoResizeColumn(0,
                ColumnHeaderAutoResizeStyle.HeaderSize);
            siteInfoListView.AutoResizeColumn(1,
                ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        void powerAction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //string arg = e.Result.ToString();            
        }

        void powerAction_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] arg = e.Argument as string[];
            string[] results = RunPSScript(Properties.Resources.PowerAction, arg, null);
            foreach (string result in results)
            { treeView1.Invoke(new LogText(logText), new object[] { result }); }
        }
        private void buildTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ServiceProgressBar.Hide();
            try
            {
                this.toolStripProgressBar1.Visible = false;
                this.siteInfoListView.View = View.Details;
                this.siteInfoListView.Items.Clear();
                this.siteInfoListView.HeaderStyle = ColumnHeaderStyle.None;
                this.getDesktopStats.RunWorkerAsync();

                foreach (siteDetails detail in _siteInfo)
                {
                    ListViewItem lvi = new ListViewItem(detail.detailLabel);
                    lvi.SubItems.Add(detail.detailValue);
                    this.siteInfoListView.Items.Add(lvi);
                }
                this.Cursor = Cursors.Default;
                this.statusLabel.Hide();
                //this.button1.Enabled = true;
                //this.button2.Enabled = true;
                //string[] services = GetConfigRegisteredServiceInstance();
                //string[] brokers = GetBrokerControllers();
                int licServ = 0;
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
                this.treeView1.Invoke(new SetCursor(setCursor), new object[] { "ready" });
                if (needsRefresh)
                {
                    Thread.Sleep(2000);
                    configNode.Remove();
                    buildServices(false);
                    needsRefresh = false;
                }
            }
            catch (Exception ex)
            {
                treeView1.Invoke(new LogText(logText), new object[] { ex.InnerException.ToString() });
                try
                {
                    if (needsRefresh)
                    {
                        Thread.Sleep(2000);
                        configNode.Remove();
                        buildServices(false);
                        needsRefresh = false;
                    }
                }
                catch { treeView1.Invoke(new LogText(logText), new object[] { "Can't build services nodes!" }); }
            }
        }
        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }
        public static string installPath(string hostname)
        {
            try
            {
                RegistryKey reg = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostname);
                return reg.OpenSubKey("SOFTWARE\\Citrix\\Citrix Desktop Delivery Controller").GetValue("InstallDir").ToString();
            }
            catch
            {

                return "Not Found";
            }
        }
        private void buildTree_DoWork(object sender, DoWorkEventArgs e)
        {
            string arg = e.Argument.ToString();
            List<string> localHost = new List<string> { targetDDC };
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = installPath(targetDDC);
            path = path.Replace(":", "$");
            try
            {
                //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo("\\\\" + targetDDC + "\\" + path + "\\Broker\\Service\\BrokerService.exe");
                string[] parms = new string[] { targetDDC }; ;
                siteVersion = RunScript(Properties.Resources.ControllerVersion, parms).Replace("\r\n", string.Empty);
                //string siteVersion = version[0]; 
                if (siteVersion.Length < 1)
                {
                    siteVersion = "5.6";
                }
                string majorVersion = Left(siteVersion, 1);
                if (majorVersion == "5")
                {
                    expected = 12;
                    serviceNames = rhoneserviceNames;
                    brokerProcesses = rhonebrokerProcesses;
                    treeView1.Invoke(new CheckServices(checkServices), new object[] { rhoneserviceNames, localHost, brokerProcesses });
                }
                else
                {
                    if (majorVersion == "7")
                    {
                        expected = 43;
                        serviceNames = jasperserviceNames;
                        brokerProcesses = jasperbrokerProcesses;
                        treeView1.Invoke(new CheckServices(checkServices), new object[] { jasperserviceNames, localHost, brokerProcesses });
                    }
                }
            }
            catch { }
            BuildTree(arg);
        }
        private void runScript_DoWork(object sender, DoWorkEventArgs e)
        {
            treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.TestInstance });
            string[] parms = new string[] { nodeclicked.Text, targetDDC };
            e.Result = RunScript(Properties.Resources.TestInstance, parms);
        }
        private void runScript_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.toolStripProgressBar1.Visible = false;
            this.statusLabel.Visible = false;
            //this.button1.Enabled = true;
            //this.button2.Enabled = true;
            treeView1.Show();
            MessageBoxIcon mb = new MessageBoxIcon();
            string result = e.Result.ToString();
            if (result == "Responding\r\n") { mb = MessageBoxIcon.Information; }
            else { mb = MessageBoxIcon.Warning; }
            MessageBox.Show(this, nodeclicked.Text + " is " + e.Result.ToString(), "Service instance is " + result, MessageBoxButtons.OK, mb);
        }
        private void serviceReset_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string arg = e.Argument as string;
            if (arg == "site instances")
            {
                treeView1.Invoke(new LogText(logText), new object[] { "Resetting all site service instances" });
                executePS(Properties.Resources.ResetInstances, null);
            }
            if (arg == "ddc instances")
            {
                treeView1.Invoke(new LogText(logText), new object[] { "Resetting all service instances for '" + nodeclicked.Text + "'." });
                executePS(Properties.Resources.resetbrokerserviceinstances, nodeclicked.Text);
            }
            if (arg == "clear preferred")
            {
                treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.ClearPreferredHypervisor });
                string[] parms = new string[] { nodeclicked.Text.Trim(), targetDDC };
                RunScript(Properties.Resources.ClearPreferredHypervisor, parms);
            }
        }
        private void serviceReset_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            needsRefresh = true;
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "ready" });

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
            }
        }
        private void restartService_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string hostName = nodeclicked.Parent.Text;
            string serviceName = nodeclicked.Text;
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
            //ServiceProgressBar.Visible = false;
            toolStripProgressBar1.Visible = false;
        }
        private void restartService_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.ServiceProgressBar.Value = e.ProgressPercentage;
        }
        private void disableLogging_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwAsync = sender as BackgroundWorker;
            string arg = e.Argument as string;
            Program.configLogging("disable", arg, null, nodeclicked.Parent.Text, nodeclicked.Text, false);
        }
        private void disableLogging_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string server = nodeclicked.Parent.Text;
            string service = nodeclicked.Text;
            string[] hostName = server.Split('.');

            //ServiceProgressBar.Visible = false;
            toolStripProgressBar1.Visible = false;
            statusLabel.Visible = false;
            treeView1.Show();
            ////this.button1.Enabled = true;
            ////this.button2.Enabled = true;
            MessageBox.Show(this, "Logging has been disabled for the " + service + " on " + hostName[0], "Logging disabled", MessageBoxButtons.OK);
        }
        #endregion
        #region Routines
        public void BuildTree(string args)
        {
            this.Invoke(new SetLabel(setLabel), new object[] { "Refreshing..", statusLabel });
            treeView1.Invoke(new SetCursor(setCursor), new object[] { "busy" });
            List<string> localHost = new List<string> { targetDDC };
            checkServices(serviceNames, localHost, brokerProcesses);
            string[] dbconn = dbConn();

            bool sitebuilt = true;
            bool groupsbuilt;
            bool mcsbuilt;
            bool servicesbuilt;
            switch (args)
            {
                case "all":
                    sitebuilt = buildSite(true, dbconn);
                    if (sitebuilt == false)
                    {
                        TopNode.Remove();
                        sitebuilt = buildSite(true, dbconn);
                    }
                    servicesbuilt = buildServices(true);
                    mcsbuilt = buildMCS(true);
                    groupsbuilt = buildAssignments(true);
                    break;
                case "Assignments":
                    groupsbuilt = buildAssignments(true);
                    break;
                case "Machines":
                    mcsbuilt = buildMCS(true);
                    break;
                case "Configuration":
                    servicesbuilt = buildServices(false);
                    break;
            }
        }
        private bool Ping(string entry)
        {
            try
            {
                this.Invoke(new SetLabel(setLabel), new object[] { "Pinging " + entry, statusLabel });
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
                this.Invoke(new SetLabel(setLabel), new object[] { "Cannot Ping " + entry, statusLabel });
                treeView1.Invoke(new LogText(logText), new object[] { e.InnerException.ToString() });
                return (false);
            }
        }
        public void checkServices(string[] serviceNames, List<string> servers, string[] processNames)
        {
            this.Invoke(new SetLabel(setLabel), new object[] { "Checking services ", statusLabel });
            int i;
            int o;

            foreach (string server in servers)
            {
                try
                {
                    this.Invoke(new SetLabel(setLabel), new object[] { "Enumerating processes on " + server, statusLabel });
                    Process[] running = Process.GetProcesses(server);
                    string[] runningProcs = new string[running.Length];
                    o = 0; i = 0;
                    string[] result = null;
                    foreach (Process proc in running)
                    {
                        runningProcs[o] = proc.ToString();
                        o++;
                    }
                    foreach (string process in runningProcs)
                    {
                        result = Array.FindAll(runningProcs, s => s.Contains(process));
                        if (result.Length == 0)
                        {
                            DialogResult d = MessageBox.Show(this, serviceNames[i].ToString() + " is not running on " + server + ". Attempt to start the service now?", serviceNames[i] + " not started!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                            if (d == DialogResult.Yes)
                            {
                                ServiceController sc = new ServiceController(serviceNames[i], server);
                                Program.startService(sc);
                            }
                        }
                        i++;
                    }
                }
                catch (Exception e)
                {
                    if (e.InnerException.StackTrace.Contains("The network path was not found"))
                    {
                        try
                        {
                            ServiceController sc = new ServiceController("RemoteRegistry", server);
                            Program.startService(sc);
                        }
                        catch
                        {

                        }
                    }
                    this.Invoke(new SetLabel(setLabel), new object[] { "Unable to connect to " + server, statusLabel });
                    treeView1.Invoke(new LogText(logText), new object[] { "Unable to connect to " + server + "." + "\r\nInnerException: " + "\r\n" + e.InnerException.ToString() + "\r\n" });
                }
            }
        }
        private void executePS(string scriptText, string param)
        {
            treeView1.Invoke(new LogText(logText), new object[] { scriptText });
            Runspace runspace = RunspaceFactory.CreateRunspace();
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
            catch (Exception e)
            {
                treeView1.Invoke(new LogText(logText), new object[] { e.InnerException.ToString() });
            }
        }
        #endregion
        #region Event Handlers
        // Form Buttons and Load Events
        private void powerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            power changePower = new power();
            changePower.assignment = nodeclicked.Text;
            changePower.targetDDC = targetDDC;
            changePower.Show();
        }
        private void clearLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.logTextBox.Clear();
        }
        private void removeStorageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodeclicked.Parent.GetNodeCount(true) > 1)
            {
                string[] parms = { "removeStorage", nodeclicked.Parent.Parent.Text.Trim(), nodeclicked.Text.Trim(), targetDDC };
                ////this.button1.Enabled = false;
                ////this.button2.Enabled = false;
                workProgress.RunWorkerAsync(parms);
            }
            else
            {
                MessageBox.Show("'" + nodeclicked.Text + "' is the last storage unit assigned to '" + nodeclicked.Parent.Parent.Text + "'. Please add another storage unit to '" + nodeclicked.Parent.Parent.Text + "' before removing '" + nodeclicked.Text + "'.", "Cannot remove the last storage unit!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            resetTopNode("Loading...");
            this.KeyPreview = true;
            //button1.Enabled = false;
            //button2.Enabled = false;
            //ServiceProgressBar.Visible = true;
            toolStripProgressBar1.Visible = true;            
            this.statusLabel.Text = "Gathering site details...";
            this.statusLabel.Show();
            buildTree.RunWorkerAsync("all");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            refreshTreeview("all");
        }
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    refreshTreeview("all");
                    break;
                case Keys.Enter:
                    ClearBackColor();
                    FindByText();
                    break;
                case Keys.Left:
                    if (nodeclicked.IsExpanded)
                    {
                        nodeclicked.Collapse();
                    }
                    break;
                case Keys.Right:
                    if (!nodeclicked.IsExpanded)
                    {
                        nodeclicked.Expand();
                    }
                    break;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            serviceReset.RunWorkerAsync("site instances");
            configNode.Remove();
            //button1.Enabled = false;
            //button2.Enabled = false;
            //TreeNode mainNode = new TreeNode();
            //mainNode.Text = "Resetting Service Instances...";
            this.statusLabel.Text = "Resetting all site service instances...";
            this.statusLabel.Show();
            //this.treeView1.Nodes.Add(mainNode);
            buildTree.RunWorkerAsync("Configuration");
        }
        // TreeView context menu event handlers
        private void siteAction2_Click(object sender, EventArgs e)
        {
            refreshTreeview(nodeclicked.Text);
        }
        private void refreshTreeview(string arg)
        {

            switch (arg)
            {
                case "all":
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        node.Remove();
                    }
                    resetTopNode("Refreshing...");
                    break;
                case "Assignments":
                    desktopGroupsNode.Remove();
                    break;
                case "Machines":
                    MCSNode.Remove();
                    break;
                case "Configuration":
                    configNode.Remove();
                    break;
            }
            toolStripProgressBar1.Visible = true;
            this.statusLabel.Text = "Gathering site details...";
            this.statusLabel.Show();
            buildTree.RunWorkerAsync(arg);
        }
        private void setNodeInfo(string[] details)
        {
            //Set the Node Information listview with cmdlet output in string[] format

            nodeInfoListView.Items.Clear();
            List<KeyValuePair<string, string>> nodeTable = new List<KeyValuePair<string, string>>();
            foreach (string element in details)
            {
                if (element.Contains(":"))
                {
                    string[] detail = element.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries); //split at the first :
                    if (detail[1].Trim().Length > 0)
                    {
                        nodeTable.Add(new KeyValuePair<string, string>(detail[0].Trim(), detail[1].Trim())); //add the pair of values to the node detail list
                    }
                }
            }

            nodeInfoListView.View = View.Details; //set list view to View.Details mode
            nodeInfoListView.HeaderStyle = ColumnHeaderStyle.None; //remove headers
            nodeInfoListView.FullRowSelect = true;

            foreach (KeyValuePair<string, string> detail in nodeTable)
            {
                ListViewItem lvi = new ListViewItem(detail.Key);
                lvi.SubItems.Add(detail.Value);
                nodeInfoListView.Items.Add(lvi);
            }
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) //Set node information and details on treenode NodeMouseClick event, where e is the clicked node
        {
            if (nodeClick.IsBusy)
            {
                nodeClick.CancelAsync();
            }
            else
            {
                nodeClick.RunWorkerAsync(e);
            }
            if (e.Button == MouseButtons.Right)
            {
                listViewClicked = this.nodeInfoListView.GetItemAt(e.X, e.Y);
                this.contextMenuStrip2.Items[0].Visible = true;
            }
        }
        private void serviceAction1_Click(object sender, EventArgs e)
        {
        }
        private void serviceInstanceAction1_Click(object sender, EventArgs e)
        {
            //this.ServiceProgressBar.Visible = true;
            toolStripProgressBar1.Visible = true;
            this.statusLabel.Text = "Testing " + nodeclicked.Text;
            this.statusLabel.Visible = true;
            ////this.button1.Enabled = false;
            ////this.button2.Enabled = false;
            treeView1.Hide();
            runScript.RunWorkerAsync();
        }
        private void resetServices_Click(object sender, EventArgs e)
        {

        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //restart service    
            //this.ServiceProgressBar.Visible = true;
            toolStripProgressBar1.Visible = true;
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
            //this.ServiceProgressBar.Visible = true;
            toolStripProgressBar1.Visible = true;
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
            //this.ServiceProgressBar.Visible = true;
            toolStripProgressBar1.Visible = true;
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
            string service = nodeclicked.Text;
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
            string configFile = Program.configFile(service, fullPath);

            string[] hostName = server.Split('.');

            bool exists = File.Exists(configFile);
            if (exists)
            {
                string configBackup = configFile + ".backup";
                try { File.Copy(configFile, configBackup); }
                catch (Exception exc)
                {
                    treeView1.Invoke(new LogText(logText), new object[] { exc.ToString() });
                }
            }
            DialogResult d = MessageBox.Show(this, "This operation will modify " + configFile + ", and restart the " + service + " on " + hostName[0] + " to disable logging. Would you like to continue?", "Update config file and restart the " + service + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.Yes)
            {
                //enableLogging 
                //ServiceProgressBar.Show();
                toolStripProgressBar1.Visible = true;
                statusLabel.Text = "Disabling logging...";
                statusLabel.Show();
                ////this.button1.Enabled = false;
                ////this.button2.Enabled = false;
                treeView1.Hide();
                disableLogging.RunWorkerAsync(configFile);
            }
            if (d == DialogResult.No)
            {
                return;
            }
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
                //ServiceProgressBar.Show();
                toolStripProgressBar1.Visible = true;
                string[] ddc = nodeclicked.Text.Split(new char[] { '.' });
                statusLabel.Text = "Resetting service instances on " + ddc[0];
                statusLabel.Show();
                nodeclicked.Collapse();
                //resetTopNode("Resetting service instances..");
                serviceReset.RunWorkerAsync("ddc instances");
            }
        }
        private void changeDDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeddc changePreferredDDC = new changeddc();
            changePreferredDDC.DDCs = brokers;
            changePreferredDDC.service = nodeclicked.Text.Trim();
            string[] parms = new string[] { nodeclicked.Text.Trim(), targetDDC };
            treeView1.Invoke(new LogText(logText), new object[] { Properties.Resources.GetBrokerHypervisorConnectionPreferredBroker });
            changePreferredDDC.currentDDC = RunScript(Properties.Resources.GetBrokerHypervisorConnectionPreferredBroker, parms).Trim();
            changePreferredDDC.Show();
        }
        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ServiceProgressBar.Show();
            toolStripProgressBar1.Visible = true;
            string[] ddc = nodeclicked.Text.Split(new char[] { '.' });
            statusLabel.Text = "Clearing preferred broker for " + nodeclicked.Text.Trim();
            statusLabel.Show();
            nodeclicked.Collapse();
            //resetTopNode("Resetting service instances..");
            serviceReset.RunWorkerAsync("clear preferred");
        }
        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(listViewClicked.SubItems[1].Text);
        }
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(logTextBox.SelectedText.Replace("\n", Environment.NewLine));
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string[] parms = { "removeTask", nodeclicked.Text, null };
            //this.button1.Enabled = false;
            //this.button2.Enabled = false;
            workProgress.RunWorkerAsync(parms);
        }
        private void stopTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { "stopTask", nodeclicked.Text, null };
            //this.button1.Enabled = false;
            //this.button2.Enabled = false;
            workProgress.RunWorkerAsync(parms);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string filename = string.Format("XDSC-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            File.WriteAllText(filename, logTextBox.Text.Replace("\n", Environment.NewLine), Encoding.Unicode);
            System.Diagnostics.Process.Start(filename);
        }
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.siteInfoListView.Width = (splitContainer1.Panel1.Width - 5);
            this.nodeInfoListView.Width = (splitContainer1.Panel1.Width - 5);
            //this.nodeInfoRichTextBox.Width = (splitContainer1.Panel1.Width - 5);
            this.logTextBox.Width = (splitContainer1.Panel1.Width - 5);
        }
        private void Main_Resize(object sender, System.EventArgs e)
        {

            Control control = (Control)sender;
            int txtdiff = 565;
            int treeviewdiff = 65;
            logTextBox.Height = control.Size.Height - txtdiff;
            treeView1.Height = control.Size.Height - treeviewdiff;
            statusLabel.Top = control.Size.Height - 45;
            oldSize = control.Size;
            Application.DoEvents();
        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshTreeview("all");
        }
        private void registerServiceInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serviceReset.RunWorkerAsync("site instances");
            configNode.Remove();
            this.statusLabel.Text = "Resetting all site service instances...";
            this.statusLabel.Show();
            buildTree.RunWorkerAsync("Configuration");
        }
        private void runInPoSHcarefulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = string.Format("XDSC-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            File.WriteAllText(filename, logTextBox.SelectedText.Replace("\n", Environment.NewLine), Encoding.Unicode);
            System.Diagnostics.Process.Start(filename);
        }
        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodeclicked.Parent.Text.Contains("Assignments"))
            {
                string[] parms = { "enableMaint", nodeclicked.Text.Trim(), null };
                if (!workProgress.IsBusy)
                {
                    workProgress.RunWorkerAsync(parms);
                }
            }
            if (nodeclicked.Level > 3)
            {
                if (nodeclicked.Parent.Parent.Text.Contains("Catalogs") | nodeclicked.Parent.Parent.Parent.Text.Contains("Assignments"))
                {
                    string[] parms = { "enableCatalogMaint", nodeclicked.Text.Trim(), null };
                    if (!workProgress.IsBusy)
                    {
                        workProgress.RunWorkerAsync(parms);
                    }
                }
            }
        }
        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodeclicked.Level > 0)
            {
                if (nodeclicked.Parent.Text.Contains("Assignments"))
                {
                    string[] parms = { "disableMaint", nodeclicked.Text.Trim(), null };
                    if (!workProgress.IsBusy)
                    {
                        workProgress.RunWorkerAsync(parms);
                    }
                }
                if (nodeclicked.Level > 3)
                {
                    if (nodeclicked.Parent.Parent.Text.Contains("Catalogs") | nodeclicked.Parent.Parent.Parent.Text.Contains("Assignments"))
                    {
                        string[] parms = { "disableCatalogMaint", nodeclicked.Text.Trim(), null };
                        if (!workProgress.IsBusy)
                        {
                            workProgress.RunWorkerAsync(parms);
                        }
                    }
                }
            }
        }
        private void removeTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { "removeTasks", nodeclicked.Text, null };
            if (!workProgress.IsBusy)
            {
                workProgress.RunWorkerAsync(parms);
            }
        }
        private void stopTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { "stopTasks", nodeclicked.Text, null };
            if (!workProgress.IsBusy)
            {
                workProgress.RunWorkerAsync(parms);
            }
        }
        private void createEvictScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] dbconn = dbConn();
            string[] scriptParms = new string[] { nodeclicked.Text, targetDDC };
            string[] brokerSID = RunPSScript(Properties.Resources.GetBrokerSid, scriptParms, "\r\n");
            string[] parms = { "evictScript", nodeclicked.Text, brokerSID[0], dbconn[3].Trim(), string.Format("XDSC-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now), targetDDC };
            if (!workProgress.IsBusy)
            {
                workProgress.RunWorkerAsync(parms);
            }
        }
        private void checkUnregisteredWorkersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckWorkers checkWorkers = new CheckWorkers();
            checkWorkers.Show();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == nodeclicked)
            {
                if (nodeclicked.Level == 10)
                {
                    if (nodeclicked.Text == "Configuration")
                    {
                        foreach (TreeNode node in nodeclicked.Nodes)
                        {
                            if (node.Text.Contains("Hosts"))
                            {
                                if (node.Nodes.Count == 0)
                                {
                                    string[] parms = { nodeclicked.Text };
                                    if (!populateTree.IsBusy)
                                    {
                                        populateTree.RunWorkerAsync(parms);
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.Node.Level > 1)
                {
                    if (e.Node.Parent.Text.Contains("Groups"))
                    {
                        string[] parms = { "checkAssignmentRegistration", e.Node.Text, null };
                        if (!workProgress.IsBusy)
                        {
                            workProgress.RunWorkerAsync(parms);
                        }
                    }
                    if (e.Node.Text == "Registered" | e.Node.Text == "Un-Registered" | e.Node.Text.Contains("Never"))
                    {
                        string[] parms = { "enumWorkers", e.Node.Parent.Text, null };
                        if (!workProgress.IsBusy)
                        {
                            workProgress.RunWorkerAsync(parms);
                        }
                    }
                    if (e.Node.Text == "Hosts")
                    {
                        foreach (TreeNode node in nodeclicked.Nodes)
                        {
                            string[] parms = { e.Node.Text };
                            if (!populateTree.IsBusy)
                            {
                                populateTree.RunWorkerAsync(parms);
                            }
                        }
                    }
                    if (e.Node.Parent.Text == "Catalogs")
                    {
                        if (nodeclicked.Nodes.Count == 0)
                        {
                            string[] parms = { "enumCatalogWorkers", e.Node.Text, null };
                            if (!workProgress.IsBusy)
                            {
                                workProgress.RunWorkerAsync(parms);
                            }
                        }
                    }
                    if (e.Node.Text == "Completed Power Actions")
                    {
                        removeNodes(failedPowerActions);
                        string[] parms = { e.Node.Text };
                        if (!populateTree.IsBusy)
                        {
                            nodeexpanded = e.Node;
                            populateTree.RunWorkerAsync(parms);
                        }
                        e.Node.Expand();
                    }
                    if (e.Node.Text == "Active Power Actions")
                    {
                        removeNodes(runningPowerActions);
                        string[] parms = { e.Node.Text };
                        if (!populateTree.IsBusy)
                        {
                            nodeexpanded = e.Node;
                            populateTree.RunWorkerAsync(parms);
                        }
                        e.Node.Expand();
                    }
                }
            }
        }
        void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                if (node.Text.Contains("Hosts"))
                {
                    foreach (TreeNode childnode in e.Node.Nodes)
                    {
                        if (childnode.Nodes.Count == 0)
                        {
                            string[] parms = { "Configuration" };
                            if (!populateTree.IsBusy)
                            {
                                nodeexpanded = e.Node;
                                populateTree.RunWorkerAsync(parms);
                            }
                        }
                    }
                }
            }
            if (e.Node.Text.Contains("Machines"))
            {
                foreach (TreeNode node in failedPowerActions.Nodes)
                {
                    node.Remove();
                }
                foreach (TreeNode node in runningPowerActions.Nodes)
                {
                    node.Remove();
                }
                foreach (TreeNode childnode in e.Node.Nodes)
                {
                    if (childnode.Nodes.Count == 0)
                    {
                        string[] parms = { e.Node.Text };
                        if (!populateTree.IsBusy)
                        {
                            nodeexpanded = e.Node;
                            populateTree.RunWorkerAsync(parms);
                        }
                    }
                }
            }
            if (e.Node.Text == "Sessions")
            {
                string[] parms = { e.Node.Text };
                if (!populateTree.IsBusy)
                {
                    nodeexpanded = e.Node;
                    populateTree.RunWorkerAsync(parms);
                }
            }
            if (e.Node.Text == "Applications")
            {
                if (applicationInstance != null)
                {
                    foreach (TreeNode node in applicationInstance.Parent.Parent.Nodes)
                    {
                        foreach (TreeNode child in node.Nodes)
                        {
                            child.Remove();
                        }
                    }
                }
                string[] parms = { e.Node.Text };
                if (!populateTree.IsBusy)
                {
                    nodeexpanded = e.Node;
                    populateTree.RunWorkerAsync(parms);
                }
            }
        }
        private void btnNodeTextSearch_Click_1(object sender, EventArgs e)
        {
            ClearBackColor();
            FindByText();
        }
        private void turnOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "TurnOn" };
            powerAction.RunWorkerAsync(parms);
        }
        private void turnOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "TurnOff" };
            powerAction.RunWorkerAsync(parms);
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "Shutdown" };
            powerAction.RunWorkerAsync(parms);
        }

        private void forceRestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "Restart" };
            powerAction.RunWorkerAsync(parms);
        }

        private void forceRestartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "Reset" };
            powerAction.RunWorkerAsync(parms);
        }

        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "Suspend" };
            powerAction.RunWorkerAsync(parms);
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] parms = { nodeclicked.Text, "Resume" };
            powerAction.RunWorkerAsync(parms);
        }
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodeclicked.Parent.Parent.Text == "Sessions" | nodeclicked.Parent.Parent.Text == "Applications")
            {
                string[] split = nodeclicked.Text.Split(':');
                string[] split2 = split[1].Split(' ');
                string[] param = new string[] { split2[0] };
                RunPSScript(Properties.Resources.DisconnectSession, param, null);
            }
            if (nodeclicked.Parent.Text == "Registered")
            {
                string[] param = new string[] { nodeclicked.Text };
                RunPSScript(Properties.Resources.DisconnectDesktopSession, param, null);
            }
        }
        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodeclicked.Parent.Parent.Text == "Sessions" | nodeclicked.Parent.Parent.Text == "Applications")
            {
                string[] split = nodeclicked.Text.Split(':');
                string[] split2 = split[1].Split(' ');
                string[] param = new string[] { split2[0] };
                RunPSScript(Properties.Resources.LogoffSession, param, null);
            }
            if (nodeclicked.Parent.Text == "Registered")
            {
                string[] param = new string[] { nodeclicked.Text };
                RunPSScript(Properties.Resources.LogoffDesktopSession, param, null);
            }
        }
        private void disconnectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunPSScript(Properties.Resources.DisconnectDesktopSessions, null, null);
        }
        private void logOffAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] param = new string[1];
            if (nodeclicked.Text == "Active Sessions")
            {
                param[0] = "active";
            }
            if (nodeclicked.Text == "Inactive Sessions")
            {
                param[0] = "inactive";
            }
            RunPSScript(Properties.Resources.LogoffDesktopSessions, param, null);
        }
        private void launchCheckWorker_Click(object sender, EventArgs e)
        {
            CheckWorkers checkWorkers = new CheckWorkers();
            checkWorkers.Show();
        }
        private void enumerateSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.statusLabel.Show();
            siteEnum.RunWorkerAsync();
        }
        private void clearLog_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.logTextBox.Clear();
        }
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(@".\log.txt", logTextBox.Text);
            System.Diagnostics.Process.Start(@".\log.txt");
        }
        private void clearLog_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.logTextBox.Clear();
        }
        #endregion
    }
}