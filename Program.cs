using System;
using System.Windows.Forms;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Net;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Threading;
using System.Xml;
using System.Text;
using Microsoft.VisualBasic;
using Security.Windows.Forms;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace XDSiteChecker
{
    public enum SECURITY_IMPERSONATION_LEVEL : int
    {
        SecurityAnonymous = 0,
        SecurityIdentification = 1,
        SecurityImpersonation = 2,
        SecurityDelegation = 3
    }
	public static class Program
	{       
		/// <summary>
		/// The main entry point for the application.
		/// </summary>    
        /// 
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
                string lpszUsername,
                string lpszDomain,
                string lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                out IntPtr phToken);
        
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(
            IntPtr handle);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        
		[STAThread]        
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            //string[] areModulesLoaded = RunScript(Properties.Resources.CheckNeededSnapins, null, "\r\n");            
            string[] areModulesLoaded = new string[] { "Yes" };
            if (areModulesLoaded[0] == "Yes")
            {
                string[] parm = new string[] { "localhost" };
                string[] ddcLocation = RunScript(Properties.Resources.TestSiteConn, parm, "\r\n");
                while (ddcLocation[0].Contains("Failed to connect") && XDSiteChecker.Main.targetDDC != "")
                {
                    XDSiteChecker.Main.targetDDC = Interaction.InputBox("XenDesktop SDK Installed. Specify target DDC:", "SiteDiag - Remote connect", "DDC Name/IP");
                    if (XDSiteChecker.Main.targetDDC.Contains("DDC FQDN"))
                    {
                        XDSiteChecker.Main.targetDDC = Interaction.InputBox("Please specify target DDC FQDN or IP address:", "Remote connect", "");
                    }
                    parm[0] = XDSiteChecker.Main.targetDDC;
                    ddcLocation[0] = RunScript(Properties.Resources.TestSiteConn, parm, "\r\n")[0];
                    XDSiteChecker.Main.runlocal = false;
                }
                if (ddcLocation[0].Contains("Insufficient"))
                {                    
                    using (UserCredentialsDialog dialog = new UserCredentialsDialog())
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            string[] cred = new string[] { dialog.User, dialog.Domain, dialog.PasswordToString() };
                            
                            IntPtr userToken = IntPtr.Zero;                            
                            bool success = LogonUser(
                              dialog.User,
                              dialog.Domain,
                              dialog.PasswordToString(),
                              2,
                              0,
                              out userToken);

                            if (!success)
                            {
                                //throw new SecurityException("Logon user failed");
                            }

                            using (WindowsIdentity.Impersonate(userToken))
                            {
                                // RunAs
                                Application.Run(new Main());
                            }
                            //WindowsIdentity altUser = ImpersonateUser(cred[0], cred[1], cred[2]);
                            //using (altUser)
                            //{
                            //    using (WindowsImpersonationContext impersonatedUser = altUser.Impersonate())
                            //    {
                            //        Application.Run(new Main());
                            //    }
                            //}
                        }
                    }
                }
                if (ddcLocation[0].Contains("Local"))
                {
                    //targetDDC = "localhost";
                    Application.Run(new Main());
                }
            }
            if (areModulesLoaded[0] == "No")
            {
                //Exit with error                
                string missing = string.Join("\r\n", areModulesLoaded);
                DialogResult d = MessageBox.Show("Install the following PowerShell snapins to use this tool:\r\n\r\n" + missing, "Missing XenDesktop Powershell SDK Snapins!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (XDSiteChecker.Main.targetDDC == "")
            {
                //canceled
                Application.Exit();
            }			
		}
        public static bool ContainsNoCase(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
        public static string installPath (string hostname) 
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
		public static string configFile (string service, string path)
		{
			string fullPath = path;
			if (service == "Citrix AD Identity Service")
			{
				return fullPath + "\\ADIdentity\\Service\\Citrix.ADIdentity.SdkWcfEndpoint.exe.config";
			}
			if (service == "Citrix Broker Service")
			{
				return fullPath + "\\Broker\\Service\\BrokerService.exe.Config";
			}
			if (service == "Citrix Configuration Service")
			{
				return fullPath + "\\Configuration\\Service\\Citrix.Configuration.SdkWcfEndpoint.exe.config";
			}
            if (service == "Citrix Environment Test Service")
            {
                return fullPath + "\\EnvTest\\Service\\Citrix.EnvTest.SdkWcfEndpoint.exe.config";
            }
			if (service == "Citrix Host Service")
			{
				return fullPath + "\\Host\\Service\\Citrix.Host.SdkWcfEndpoint.exe.config";
			}
			if (service == "Citrix Machine Creation Service")
			{
				return fullPath + "\\MachineCreation\\Service\\Citrix.MachineCreation.SdkWcfEndpoint.exe.config";
			}
			if (service == "Citrix Machine Identity Service")
			{
				return fullPath + "\\MachineIdentity\\Service\\Citrix.MachineIdentity.SdkWcfEndpoint.exe.config";
			}
            if (service == "Citrix Monitor Service")
            {
                return fullPath + "\\Monitor\\Service\\Citrix.Monitor.SdkWcfEndpoint.exe.config";
            }
            if (service == "Citrix StoreFront Service")
            {
                return fullPath + "\\StoreFront\\Service\\Citrix.StoreFront.SdkWcfEndpoint.exe.config";
            }
			else
			{
				return null;
			}        
		}
		public static string fullPath (string server, string service)
		{                 
			string rootPath = Program.installPath(server);
			string[] pathSplit = rootPath.Split(':');            
			string fullPath;
			string fqdn = GetFQDN();
			if (!server.Contains(fqdn))
			{                
				fullPath = "\\\\" + server + "\\" + pathSplit[0] + "$" + pathSplit[1];                
				return fullPath;
			}
			else
			{
				fullPath = Program.installPath(server);
				return fullPath;
			}  
		}        
		public static void configLogging (string action, string configFile, string logdir, string server, string service, bool overwrite)
		{
			Thread thdWorker = new Thread(new ParameterizedThreadStart(serviceStop));
			string p1 = service;            
			string p2 = server;
			object[] parameters = new object[] { p1, p2 };

			thdWorker.Start(parameters);            
			thdWorker.Join();
			XmlDocument config = new XmlDocument();
			
			config.Load(configFile);
			XmlNode xNode = config.CreateNode(XmlNodeType.Element, "add", "");
			XmlAttribute xKey = config.CreateAttribute("key");
			XmlAttribute xValue = config.CreateAttribute("value");
			xKey.Value = "LogFileName";
            if (logdir != null)
            {
                xValue.Value = logdir.TrimEnd('\\') + "\\" + server + " " + service + ".log";
            }
			xNode.Attributes.Append(xKey);
			xNode.Attributes.Append(xValue);
			XmlNodeList nodeList = config.GetElementsByTagName("appSettings");

			foreach (XmlNode node in nodeList)
				{
					foreach (XmlNode childNode in node)
					{
						if (childNode.OuterXml.Contains("key=\"LogFileName\""))
						{
							node.RemoveChild(childNode);
						}
						if (childNode.OuterXml.Contains("key=\"OverwriteLogFile\""))
						{
							if (!overwrite)
							{
								node.RemoveChild(childNode);
							}
						}
					}
				}
			if (action == "disable")
			{
				config.Save(configFile);
				Thread thdWorker3 = new Thread(new ParameterizedThreadStart(serviceStart));
				thdWorker3.Start(parameters);
				thdWorker3.Join();                
				return;
			}

			if (action == "enable")
			{
				config.GetElementsByTagName("appSettings")[0].InsertAfter(xNode, config.GetElementsByTagName("appSettings")[0].LastChild);
				config.Save(configFile);
				if (overwrite)
				{
					config.Load(configFile);
					xNode = config.CreateNode(XmlNodeType.Element, "add", "");
					XmlAttribute yKey = config.CreateAttribute("key");
					XmlAttribute yValue = config.CreateAttribute("value");
					yKey.Value = "OverwriteLogFile";
					yValue.Value = "1";
					xNode.Attributes.Append(yKey);
					xNode.Attributes.Append(yValue);
					config.GetElementsByTagName("appSettings")[0].InsertAfter(xNode, config.GetElementsByTagName("appSettings")[0].LastChild);
					config.Save(configFile);
				}
			}
			
			Thread thdWorker2 = new Thread(new ParameterizedThreadStart(serviceStart));
			thdWorker2.Start(parameters);            
			thdWorker2.Join();
		}                    
		private static void serviceStop (object parameters)
		{
			object[] args = (object[]) parameters;
			ServiceController sc = new ServiceController((string)args[0], (string)args[1]);
			Program.stopService(sc);
		}
		private static void serviceStart (object parameters)
		{
			object[] args = (object[])parameters;
			ServiceController sc = new ServiceController((string)args[0], (string)args[1]);
			Program.startService(sc);
		}
        public static string[] RunScript(string scriptText, string[] param, string delimiter)
        {
            // create Powershell runspace            
            // open it
            //Invoke(new LogText(scriptText));
            Collection<PSObject> results;
            string returnString = "Null";

            Runspace runspace = RunspaceFactory.CreateRunspace();
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

            using (Pipeline pipeline = runspace.CreatePipeline())
            {
                pipeline.Commands.AddScript(scriptText);
                pipeline.Commands.Add("Out-String");
                // execute the script
                try
                {
                    results = pipeline.Invoke();
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (PSObject obj in results)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }
                    returnString = stringBuilder.ToString();
                    // return results as string                       
                }
                catch (Exception e)
                {
                    returnString = "Not Found";
                }
            }
            string[] resultArray = returnString.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            return resultArray;
        }

		public static string RunScript2(string scriptText, string[] parameters)
		{
			// create Powershell runspace
			Runspace runspace = RunspaceFactory.CreateRunspace();
			// open it
			runspace.Open();
			if (parameters != null)
			{
				int i = 1;
				string arg = "var" + i;
				foreach (string param in parameters) 
				{
					runspace.SessionStateProxy.SetVariable(arg, param);
					i++;
					arg = "var" + i;
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
				MessageBox.Show("Powershell command could not be executed: " + e,
				"Exception executing script",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				return "Not Found";
			}
		}
        public static string RunScript1(string scriptText, string param)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            // open it
            //Invoke(new LogText(scriptText));
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
                // return results as string
                return results[0].BaseObject.ToString();
            }
            catch 
            {
                //treeView1.Invoke(new LogText(logText), new object[] { e.StackTrace.ToString() });
                return "Not Found";
            }
        }
		public static bool startService(ServiceController service)
		{
			try
			{
				int timeoutMilliseconds = 100000;
				int millisec1 = Environment.TickCount;
				TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
				service.Start();
				service.WaitForStatus(ServiceControllerStatus.Running, timeout);                
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false;
			}
		}
		public static bool stopService(ServiceController service)
		{
			try
			{
				int timeoutMilliseconds = 100000;
				int millisec1 = Environment.TickCount;
				TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
				service.Stop();
				service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false;
			}
		}
		public static bool writeAccess(string folderPath)
		{            
			try
			{
				AuthorizationRuleCollection collection = Directory.GetAccessControl(folderPath).GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
				foreach (FileSystemAccessRule rule in collection)
				{
					if (rule.AccessControlType == AccessControlType.Allow)
					{
						return true;
					}
				}
			}
			catch 
			{
				return false;
			}
			return false;
		}
		public static string GetFQDN()
		{
			var domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
			var hostName = Dns.GetHostName();
			if (!hostName.Contains(domainName))
				return string.Format("{0}.{1}", hostName, domainName);
			else
				return hostName;
		}
	}
}