using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;

namespace XDSiteChecker
{
	public partial class CheckWorkers : Form
	{
		private BackgroundWorker updateDataGrid = new BackgroundWorker();
		private BackgroundWorker powerAction = new BackgroundWorker();
		private BackgroundWorker exportToCSV = new BackgroundWorker();
		private BackgroundWorker restartService = new BackgroundWorker();
		private bool wantClose = false;
		public delegate void AddWorker(string[] worker);
		public delegate void UpdateWorker(string[] workerdata, string machine);
		public delegate void SetLabel(string worker);
		public delegate void RemoveWorkers();
		public delegate void RefreshForm();
		SortableBindingList<worker> workers = new SortableBindingList<worker>();  
		public CheckWorkers()
		{
			InitializeComponent();
			toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
			toolStripProgressBar1.Visible = false;
			statusLabel.Visible = false;
			updateDataGrid.DoWork += new DoWorkEventHandler(updateDataGrid_DoWork);
			updateDataGrid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(updateDataGrid_RunWorkerCompleted);
			updateDataGrid.WorkerSupportsCancellation = true;
			exportToCSV.DoWork += new DoWorkEventHandler(exportToCSV_DoWork);
			exportToCSV.RunWorkerCompleted += new RunWorkerCompletedEventHandler(exportToCSV_RunWorkerCompleted);
			restartService.DoWork += new DoWorkEventHandler(restartService_DoWork);
			restartService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(restartService_RunWorkerCompleted);
			powerAction.DoWork +=new DoWorkEventHandler(powerAction_DoWork);
			powerAction.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(powerAction_RunWorkerCompleted);
			deregInterval.Value = 600;
			maxRecordCount.Value = 40;
			dataGridView1.DataSource = workers;
			foreach (DataGridViewColumn column in dataGridView1.Columns)
			{
				dataGridView1.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
			}
		}
		void exportToCSV_DoWork(object sender, DoWorkEventArgs e)
		{
			string arg = e.Argument as string;
			DateTime now = DateTime.Now;
			string filePath = arg + "\\" + now.ToFileTime() + ".csv";
			var strValue = new StringBuilder();
			string rowData = "";
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					rowData = rowData + cell.EditedFormattedValue.ToString().Replace("\r\n", "") + ",";
				}
				rowData = rowData.TrimEnd(',');
				strValue.AppendLine(rowData);
				rowData = "";
			}
			string strFile = filePath; //File Path
			if (!string.IsNullOrEmpty(strValue.ToString()))
			{
				File.WriteAllText(strFile, strValue.ToString(), Encoding.UTF8);
			}
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
			{
				FileName = arg,
				UseShellExecute = true,
				Verb = "open"
			});
		}
		void exportToCSV_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			statusLabel.Visible = false;
		}
		void updateDataGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (wantClose == true)
			{
				try { this.Close(); }
				catch (Exception exc) { string inner = exc.InnerException.ToString(); }
			}
			else
			{
				toolStripProgressBar1.Visible = false;
				statusLabel.Visible = false;
				buildGridButton.Enabled = true;
				Refresh();
			}            
		}
		void updateDataGrid_DoWork(object sender, DoWorkEventArgs e)
		{            
			string[] arg = e.Argument as string[];
			List<string> machines = new List<string> { };
			Invoke(new SetLabel(setLabel), new object[] { "Enumerating workers.." });
			Application.DoEvents();
			string[] workersToCheck = Program.RunScript2(Properties.Resources.CheckWorkers, arg).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string workerToCheck in workersToCheck)
			{
				string[] workerData = workerToCheck.Split(',');
				if (workerData[0] != "Not Found")
				{
					Invoke(new AddWorker(addWorker), new object[] { workerData });
					if (workerData[0].Length > 0)
					{
						machines.Add(workerData[0]);
					}  
				}
				if (updateDataGrid.CancellationPending)
				{
					// Set the e.Cancel flag so that the WorkerCompleted event
					// knows that the process was cancelled.
					e.Cancel = true;
					return;
				}                
			}
			List<string> machineQueue = new List<string> { };            
			foreach (string machine in machines)
			{
				if (updateDataGrid.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					foreach (DataGridViewCell cell in row.Cells)
					{
						if (cell.Value.ToString() == machine)
						{
							foreach (DataGridViewCell column in row.Cells)
							{
								if (column.OwningColumn.Name == "PowerState")
								{
									if (column.Value.ToString() != "Off")
									{
										if (isOnline(machine) == true)
										{
											Invoke(new SetLabel(setLabel), new object[] { "Getting details for " + machine });
											Application.DoEvents();
											TimeSpan up = new TimeSpan();
											DateTime last = new DateTime();
											string os = "";
											string uptime = "";
											string desktopServiceStatus = "";
											string icaServiceStatus = "";
											string listOfDDCs = "";
											
											ManagementClass class1 = new ManagementClass(@"\\" + machine + "\\root\\cimv2:Win32_Service");
											ManagementScope scope = new ManagementScope(@"\\" + machine + "\\root\\CIMV2");                                            
											ManagementClass mc = new ManagementClass(scope, new ManagementPath("StdRegProv"), null);
											try
											{
												ManagementBaseObject inParams = mc.GetMethodParameters("GetStringValue");
												inParams["sSubKeyName"] = "Software\\Policies\\Citrix\\VirtualDesktopAgent";
												inParams["sValueName"] = "ListOfDDCs";
												ManagementBaseObject outParams = mc.InvokeMethod("GetStringValue", inParams, null);
												try
												{
													listOfDDCs = outParams["sValue"].ToString();
												}
												catch
												{
													inParams["sSubKeyName"] = "Software\\Citrix\\VirtualDesktopAgent";
													ManagementBaseObject outParams2 = mc.InvokeMethod("GetStringValue", inParams, null);
													try
													{
														listOfDDCs = outParams2["sValue"].ToString();
													}
													catch
													{
														inParams["sSubKeyName"] = "Software\\Wow6432Node\\Policies\\Citrix\\VirtualDesktopAgent";
														ManagementBaseObject outParams64p = mc.InvokeMethod("GetStringValue", inParams, null);
														try { listOfDDCs = outParams64p["sValue"].ToString(); }
														catch
														{
															inParams["sSubKeyName"] = "Software\\Wow6432Node\\Citrix\\VirtualDesktopAgent";
															ManagementBaseObject outParams64 = mc.InvokeMethod("GetStringValue", inParams, null);
															try { listOfDDCs = outParams64["sValue"].ToString(); }
															catch { listOfDDCs = "Not Found"; }
														}                                                        
													}
												}
											}
											catch { listOfDDCs = "Access Denied"; }
											
											try
											{
												scope.Connect();
												ObjectQuery osQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
												ObjectQuery serviceQuery = new ObjectQuery("SELECT * FROM Win32_Service");
												ManagementObjectSearcher osSearcher = new ManagementObjectSearcher(scope, osQuery);
												ManagementObjectSearcher serviceSearcher = new ManagementObjectSearcher(scope, osQuery);
												foreach (ManagementObject queryObj in osSearcher.Get())
												{
													last = ParseCIM(queryObj["LastBootUpTime"].ToString());
													if (last != DateTime.MinValue)
														//get the diff between dates
														up = DateTime.Now - last;
													os = queryObj["Caption"].ToString();
												}
												//uptime = up.ToString();
												if (up.Days > 0)
												{
													uptime += up.Days.ToString() + " days ";
												}
												if (up.Hours > 0)
												{
													uptime += up.Hours.ToString() + " hours ";
												}
												if (up.Minutes > 0)
												{
													uptime += up.Minutes.ToString() + " minutes";
												}
												foreach (ManagementObject ob in class1.GetInstances())
												{
													var name = ob.GetPropertyValue("Name");
													var status = ob.GetPropertyValue("State");
													if (name != null)
													{
														if (name.ToString() == "WorkstationAgent")
														{
															desktopServiceStatus = status.ToString();
														}
														if (name.ToString() == "PorticaService")
														{
															icaServiceStatus = status.ToString();
														}
													}
												}
												if (desktopServiceStatus == "")
												{
													desktopServiceStatus = "Not Found!";
												}
												if (icaServiceStatus == "")
												{
													icaServiceStatus = "Not Found!";
												}
											}
											catch
											{
												string ex = "Access Denied";
												os = ex;
												uptime = ex;
												desktopServiceStatus = ex;
												icaServiceStatus = ex;
											}
											string[] parms = new string[] { machine };
											//Invoke(new RefreshForm(refreshForm), new object[] { });
											string workerResults = ",," + icaServiceStatus + ",,,,,,,," + last.ToString() + "," + uptime + "," + desktopServiceStatus + "," + listOfDDCs;
											//string workerResults = Program.RunScript2(Properties.Resources.CheckWorker, parms);
											string[] workerSplit = workerResults.Split(',');
											Invoke(new UpdateWorker(updateWorker), new object[] { workerSplit, machine });
										}
										else
										{
											string workerResults = ",,Offline,,,,,,,,Offline,Offline,Offline,Offline";
											string[] workerSplit = workerResults.Split(',');
											Invoke(new UpdateWorker(updateWorker), new object[] { workerSplit, machine });
										}
									}
									else
									{
										string workerResults = ",,Off,,,,,,,,Off,Off,Off,Offline";
										string[] workerSplit = workerResults.Split(',');
										Invoke(new UpdateWorker(updateWorker), new object[] { workerSplit, machine });
									}
								}
							}
						}
					}
				}                                               
			}                
		}
		private DateTime ParseCIM(string date)
		{
			//datetime object to store the return value
			DateTime parsed = DateTime.MinValue;

			//check date integrity
			if (date != null && date.IndexOf('.') != -1)
			{
				//obtain the date with miliseconds
				string newDate = date.Substring(0, date.IndexOf('.') + 4);

				//check the lenght
				if (newDate.Length == 18)
				{
					//extract each date component
					int y = Convert.ToInt32(newDate.Substring(0, 4));
					int m = Convert.ToInt32(newDate.Substring(4, 2));
					int d = Convert.ToInt32(newDate.Substring(6, 2));
					int h = Convert.ToInt32(newDate.Substring(8, 2));
					int mm = Convert.ToInt32(newDate.Substring(10, 2));
					int s = Convert.ToInt32(newDate.Substring(12, 2));
					int ms = Convert.ToInt32(newDate.Substring(15, 3));

					//compose the new datetime object
					parsed = new DateTime(y, m, d, h, mm, s, ms);
				}
			}

			//return datetime
			return parsed;
		}
		private bool isOnline (string strhost)
		{
			if (strhost.Length > 0)
			{
				Ping pingSender = new Ping();
				PingOptions options = new PingOptions();
				options.DontFragment = true;
				// Create a buffer of 32 bytes of data to be transmitted.  
				string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				int timeout = 120;
				try
				{
					PingReply reply = pingSender.Send(strhost, timeout, buffer, options);
					if (reply.Status == IPStatus.Success)
					{
						return true;
					}
					else
					{ return false; }
				}
				catch
				{ return false; }
			}
			else
			{ return false; }
		}
		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			DataGridViewCell dgv = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] ;            
			colorCell(dgv);            
		}
		private void colorCell(DataGridViewCell e)
		{
			try
			{
				if (e.ColumnIndex == this.dataGridView1.Columns["LastDeregReason"].Index)
				{
					string LastDeregReason = e.Value.ToString();
					if (LastDeregReason != null)
					{
						switch (LastDeregReason)
						{
							case "N/A":
								e.Style.BackColor = Color.DarkGray;
								e.Style.ForeColor = Color.White;
								break;
							case "AgentShutdown":
								e.Style.BackColor = Color.Green;
								e.Style.ForeColor = Color.Black;
								break;
							case "AgentSuspended":
								e.Style.BackColor = Color.Orange;
								e.Style.ForeColor = Color.Black;
								break;
							case "AgentRequested":
								e.Style.BackColor = Color.IndianRed;
								e.Style.ForeColor = Color.Black;
								break;
							case "BrokerError":
								e.Style.BackColor = Color.OrangeRed;
								e.Style.ForeColor = Color.Black;
								break;
							case "BrokerRegistrationLimitReached":
								e.Style.BackColor = Color.Red;
								e.Style.ForeColor = Color.Black;
								break;
							case "CommunicationFailure":
								e.Style.BackColor = Color.Red;
								e.Style.ForeColor = Color.Black;
								break;
							case "ContactLost":
								e.Style.BackColor = Color.OrangeRed;
								e.Style.ForeColor = Color.Black;
								break;
						}
					}
				}
				if (e.ColumnIndex == this.dataGridView1.Columns["LastPowerAction"].Index)
				{
					string LastPowerAction = e.Value.ToString();
					if (LastPowerAction != null)
					{
						switch (LastPowerAction)
						{
							case "TurnOn":
								e.Style.BackColor = Color.Green;
								e.Style.ForeColor = Color.Black;
								break;
							case "N/A":
								e.Style.BackColor = Color.DarkGray;
								e.Style.ForeColor = Color.White;
								break;
							case "TurnOff":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.Red;
								break;
							case "Reset":
								e.Style.BackColor = Color.OrangeRed;
								e.Style.ForeColor = Color.Black;
								break;
							case "Shutdown":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.OrangeRed;
								break; 
							case "Suspend":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.Yellow;
								break;   
						}
					}
				}
				if (e.ColumnIndex == this.dataGridView1.Columns["DesktopService"].Index | e.ColumnIndex == this.dataGridView1.Columns["ICAService"].Index)
				{
					string DesktopServiceStatus = e.Value.ToString().Replace("\r\n", "");
					if (DesktopServiceStatus != null)
					{
						switch (DesktopServiceStatus)
						{
							case "Running":
								e.Style.BackColor = Color.Green;
								e.Style.ForeColor = Color.Black;
								break;
							case "Stopped":
								e.Style.BackColor = Color.Red;
								e.Style.ForeColor = Color.Black;
								break;
							case "Not Found!":
								e.Style.BackColor = Color.Red;
								e.Style.ForeColor = Color.Yellow;
								break;
							case "N/A":
								e.Style.BackColor = Color.DarkGray;
								e.Style.ForeColor = Color.White;
								break;
							case "Pending":
								e.Style.BackColor = Color.Yellow;
								e.Style.ForeColor = Color.Black;
								break;
							case "":
								e.Style.BackColor = Color.White;
								e.Style.ForeColor = Color.Black;
								break;
							case "Off":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.Gray;
								break;
							case "Offline":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.Red;
								break;
						}
					}
				}
				if (e.ColumnIndex == this.dataGridView1.Columns["PowerState"].Index)
				{
					string PowerState = e.Value.ToString();
					if (PowerState != null)
					{
						switch (PowerState)
						{
							case "On":
								e.Style.BackColor = Color.Green;
								e.Style.ForeColor = Color.Black;
								break;
							case "Off":
								e.Style.BackColor = Color.Black;
								e.Style.ForeColor = Color.Gray;
								break;
							case "Unmanaged":
								e.Style.BackColor = Color.Orange;
								e.Style.ForeColor = Color.Black;
								break;
						}
					}
				}
				if (e.ColumnIndex == this.dataGridView1.Columns["RegistrationStatus"].Index)
				{
					string RegStatus = e.Value.ToString();
					if (RegStatus != null)
					{
						switch (RegStatus)
						{
							case "Available":
								e.Style.ForeColor = Color.Black;
								e.Style.BackColor = Color.Green;// .Black;
								break;
							case "InUse":
								e.Style.ForeColor = Color.Orange;
								e.Style.BackColor = Color.Green;
								break;
							case "Disconnected":
								e.Style.ForeColor = Color.Yellow;
								e.Style.BackColor = Color.Green;
								break;
							case "Unregistered":
								e.Style.ForeColor = Color.Black;
								e.Style.BackColor = Color.Red;
								break;
							case "Off":
								e.Style.ForeColor = Color.Gray;
								e.Style.BackColor = Color.Black;
								break;
							case "Preparing":
								e.Style.ForeColor = Color.Black;
								e.Style.BackColor = Color.Orange;
								break;
						}
					}
				}
				if (e.ColumnIndex == this.dataGridView1.Columns["LastPowerReason"].Index)
				{
					string LastPowerReason = e.Value.ToString();
					if (LastPowerReason != null)
					{
						if (LastPowerReason.Contains("Failed"))
						{
							e.Style.BackColor = Color.Red;
						}
						if (LastPowerReason.Contains("Completed"))
						{
							e.Style.BackColor = Color.Green;
						}
						else
						{
							switch (e.Value.ToString())
							{
								case "N/A":
									e.Style.BackColor = Color.DarkGray;
									e.Style.ForeColor = Color.White;
									break;
								case "Admin":
									e.Style.BackColor = Color.Blue;
									e.Style.ForeColor = Color.White;
									break;
								case "Power Policy":
									e.Style.BackColor = Color.Navy;
									e.Style.ForeColor = Color.White;
									break;
								case "Idle Pool":
									e.Style.BackColor = Color.Brown;
									e.Style.ForeColor = Color.White;
									break;
								case "/'User Driven Restart/'":
									e.Style.BackColor = Color.Orange;
									e.Style.ForeColor = Color.Black;
									break;
								case "Session Launch":
									e.Style.BackColor = Color.Green;
									e.Style.ForeColor = Color.Black;
									break;
								case "PVD":
									e.Style.BackColor = Color.Honeydew;
									break;
								case "/'Un-taint Worker/'":
									e.Style.BackColor = Color.IndianRed;
									break;
							}
						}
					}
				}
			}
			catch {            }
		}
		public void updateWorker (string[] workerdata, string machine)
		{
			
			foreach (worker worker in workers)
			{
				if (worker.ComputerName == machine)
				{
					worker.ICAService = workerdata[2].Trim();
					worker.LastBootTime = workerdata[10];
					worker.Uptime = workerdata[11];
					worker.DesktopService = workerdata[12];
					worker.ListOfDDCs = workerdata[13];
					this.dataGridView1.Refresh();
				}
			}
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{                    
					if (cell.Value.ToString() == machine)
					{
						foreach (DataGridViewCell column in row.Cells)
						{
							if (column.OwningColumn.Name == "DesktopService")
							{
								colorCell(column);
							}
						}                    
					}
				}
			} 
		}    
		private int GetIndexOfRowWithId(DataGridView dataGrid, string search)
		{
			for (int i = 0; i < dataGrid.Rows.Count; i += 1)
			{
				DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[i].Tag; // or.DataBoundItem;
				if (row.Cells[i].Value.ToString().Contains(search))
				{
					return i;
				}
			}

			throw new ArgumentException("No item with specified id exists in the dataGrid.", "id");
		}
		public void addWorker (string[] workerSplit)
		{
			workers.Add(new worker
			{                
				ComputerName = workerSplit[0],
				DesktopGroup = workerSplit[1],
				ICAService = workerSplit[2],
				RegistrationStatus = workerSplit[3],
				PowerState = workerSplit[4],
				LastPowerAction = workerSplit[5],
				LastPowerReason = workerSplit[6],
				LastPowerTime = workerSplit[7],
				LastDeregReason = workerSplit[8],
				LastDeregTime = workerSplit[9],
				LastBootTime = workerSplit[10],
				Uptime = workerSplit[11],
				DesktopService = workerSplit[12],
				ListOfDDCs = workerSplit[13]
			}); //build a worker                
		}        
		public void removeWorkers ()
		{
			workers.Clear();
		}
		public void setLabel (string content)
		{
			this.statusLabel.Text = content;
			Application.DoEvents();
		}
		public class worker
		{            
			public string ComputerName { get; set; }
			public string DesktopGroup { get; set; }  
			public string PowerState { get; set; }
			public string LastPowerAction { get; set; }
			public string LastPowerReason { get; set; }
            public string LastUser { get; set; }
            public string LastConnectionTime { get; set; }
			public string RegistrationStatus { get; set; }
			public string LastDeregReason { get; set; }
			public string LastDeregTime { get; set; }
			public string LastPowerTime { get; set; }
			public string LastBootTime { get; set; }
			public string DesktopService { get; set; }
			public string ICAService { get; set; }
			public string Uptime { get; set; }
			public string ListOfDDCs { get; set; }   
		}

		private void buildGridButton_Click(object sender, EventArgs e)
		{
			buildGrid();
		}
		private void buildGrid()
		{
			if (!updateDataGrid.IsBusy)
			{
				string powered = null;
				string reg = null;
				StringBuilder dgs = new StringBuilder();
				string dg = null;
				if (poweredonOnly.Checked == true)
				{
					powered = "PoweredOn";
				}
				else { powered = "All"; }
				if (unregOnly.Checked == true)
				{
					reg = "UnregOnly";
				}
				else { reg = "All"; }
				if (workers.Count > 0)
				{
					removeWorkers();
				}
				foreach (Object item in dgListBox.SelectedItems)
				{
					dgs.Append(item).Append(',');
				}
				dg = dgs.ToString().TrimEnd(',');
				string[] parms = new string[] { Convert.ToString(maxRecordCount.Value), Convert.ToString(deregInterval.Value), powered, reg, dg };
				toolStripProgressBar1.Visible = true;
				statusLabel.Visible = true;
				buildGridButton.Enabled = false;
				updateDataGrid.RunWorkerAsync(parms);
			}
		}
		public void refreshForm()
		{
			Refresh();
		}
		private void stopButton_Click(object sender, EventArgs e)
		{
			if (updateDataGrid.IsBusy)
			{
				setLabel("Cancelling task..");
				updateDataGrid.CancelAsync();
			}
		}
		void CheckWorkers_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (updateDataGrid.IsBusy)
			{
				setLabel("Cancelling task..");
				updateDataGrid.CancelAsync();
				e.Cancel = true;
				wantClose = true;
			}            
		}
		void dataGridView1_ColumnHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
		{
			this.dataGridView1.Columns[e.ColumnIndex].SortMode = DataGridViewColumnSortMode.Automatic;
		}
		private void CheckWorkers_Load(object sender, EventArgs e)
		{
			string[] parm = new string[] {  };
			string[] desktopgroups = Program.RunScript2(Properties.Resources.GetBrokerDesktopGroupNames, parm).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			if (desktopgroups != null)
			{
				foreach (string group in desktopgroups)
				{
					dgListBox.Items.Add(group);
				}
			}
		}
		void CheckWorkers_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F5:
					buildGrid();
					break;
				case Keys.Enter:
					buildGrid();
					break;
			}     
		}

		private void exportCSVButton_Click(object sender, EventArgs e)
		{
			string folderName;
			DialogResult result = folderBrowserDialog1.ShowDialog();
			if( result == DialogResult.OK )
			{
				folderName = folderBrowserDialog1.SelectedPath;
				statusLabel.Text = "Exporting to " + folderName + "..";
				statusLabel.Visible = true;
				exportToCSV.RunWorkerAsync(folderName);
			}
		}     
		private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			StringBuilder cbText = new StringBuilder();

			foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
			{
				cbText.Append(cell.Value.ToString()+",");
			}
			Clipboard.SetText(cbText.ToString());
		}
		private void turnOnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] parms = { dataGridView1["ComputerName", currentMouseOverRow].Value.ToString(), "TurnOn" };
			powerAction.RunWorkerAsync(parms);
		}
		private void turnOffToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] parms = { dataGridView1["ComputerName", currentMouseOverRow].Value.ToString(), "TurnOff" };
			powerAction.RunWorkerAsync(parms);
		}
		private void resetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] parms = { dataGridView1["ComputerName", currentMouseOverRow].Value.ToString(), "TurnOff" };
			powerAction.RunWorkerAsync(parms);
		}
		private void restartDesktopServiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string fqdn = dataGridView1["ComputerName", currentMouseOverRow].Value.ToString();
			string[] computerName = fqdn.Split('.');
			statusLabel.Text = "Restarting the Desktop Service on " + computerName[0];
			string[] parms = new string[] { fqdn, "Restart" };
			restartService.RunWorkerAsync(parms);
		}
		private void startDesktopServiceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string fqdn = dataGridView1["ComputerName", currentMouseOverRow].Value.ToString();
			string[] computerName = fqdn.Split('.');
			statusLabel.Text = "Starting the Desktop Service on " + computerName[0];
			string[] parms = new string[] { fqdn, "Start" };
			restartService.RunWorkerAsync(parms);
		}
		private void manageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (Process process = new Process())
				{
					process.StartInfo.FileName = "compmgmt.msc";
					process.StartInfo.Arguments = "/computer:" + dataGridView1["ComputerName", currentMouseOverRow].Value.ToString();
					process.Start();
				 
				}
			}
			catch { }
		}   
		void powerAction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//string arg = e.Result.ToString();            
		}

		void powerAction_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] args = e.Argument as string[];
			string results = Program.RunScript2(Properties.Resources.PowerAction, args);
			//foreach (string result in results)
			//{ 
				//treeView1.Invoke(new LogText(logText), new object[] { result });
			//}
		}
		private void restartService_DoWork(object sender, DoWorkEventArgs e)
		{
			// The sender is the BackgroundWorker object we need it to
			// report progress and check for cancellation.
			string[] args = e.Argument as string[];            
			string serviceName = "WorkstationAgent";            
			ServiceController service = new ServiceController(serviceName, args[0]);
			if (args[1] == "Restart")
			{
				Invoke(new SetLabel(setLabel), new object[] { "Stopping Desktop Service on " + args[0] });
				Program.stopService(service);
				Invoke(new SetLabel(setLabel), new object[] { "Starting Desktop Service on " + args[0] });
				Program.startService(service);
			}
			if (args[1] == "Start")
			{
				Invoke(new SetLabel(setLabel), new object[] { "Starting Desktop Service on " + args[0] });
				Program.startService(service);
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
			//toolStripProgressBar1.Visible = false;
		}                              
	}
}
