using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200001D RID: 29
	public partial class dfrmNetControllerFaultDeal : frmN3000
	{
		// Token: 0x0600018E RID: 398 RVA: 0x0003874C File Offset: 0x0003774C
		public dfrmNetControllerFaultDeal()
		{
			this.InitializeComponent();
			this.optIPSmall.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00038814 File Offset: 0x00037814
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			int num = -3;
			using (icController icController = new icController())
			{
				icController.ControllerSN = -1;
				if (icController.GetControllerRunInformationIP_TCP(this.ip4TCP) >= 1)
				{
					num = (int)icController.runinfo.CurrentControllerSN;
				}
			}
			this.snGetByTCP = num;
			e.Result = num;
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000388A8 File Offset: 0x000378A8
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000388AA File Offset: 0x000378AA
		private void btnClear_Click(object sender, EventArgs e)
		{
			this.richTextBox1.Clear();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000388B8 File Offset: 0x000378B8
		private void btnEditController_Click(object sender, EventArgs e)
		{
			using (dfrmController dfrmController = new dfrmController())
			{
				dfrmController.OperateNew = false;
				dfrmController.ControllerID = this.ControllerID;
				dfrmController.ShowDialog(this);
				this.updateInfoByID();
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00038908 File Offset: 0x00037908
		private void btnIPCommTestStart_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.specialCheck();
			Cursor.Current = Cursors.Default;
			base.Activate();
			Application.DoEvents();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00038930 File Offset: 0x00037930
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00038940 File Offset: 0x00037940
		private void btnSearchController_Click(object sender, EventArgs e)
		{
			using (dfrmNetControllerConfig dfrmNetControllerConfig = new dfrmNetControllerConfig())
			{
				dfrmNetControllerConfig.ShowDialog(this);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00038978 File Offset: 0x00037978
		private void btnSwitchSmallNetwork_Click(object sender, EventArgs e)
		{
			this.btnSwitchSmallNetwork.Visible = false;
			wgAppConfig.runSql(string.Format("UPDATE t_b_Controller SET f_IP={0}, f_PORT = 60000 WHERE f_ControllerID={1}", wgTools.PrepareStrNUnicode(""), this.ControllerID));
			this.updateInfoByID();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000389B1 File Offset: 0x000379B1
		private void btnTryAutoConfigureIP_Click(object sender, EventArgs e)
		{
			this.tryWEB_ByARP();
			base.Activate();
			Application.DoEvents();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000389C8 File Offset: 0x000379C8
		private void btnTryConnect_Click(object sender, EventArgs e)
		{
			using (icController icController = new icController())
			{
				icController.GetInfoFromDBByControllerID(this.ControllerID);
				if (icController.GetControllerRunInformationIP(-1) <= 0)
				{
					this.logInfo(string.Format("SN= {0} {1}", icController.ControllerSN, CommonStr.strCommFail), false);
				}
				else
				{
					this.logInfo(string.Format("SN= {0} {1}", icController.ControllerSN, CommonStr.strCommOK), true);
				}
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00038A54 File Offset: 0x00037A54
		private void button1_Click(object sender, EventArgs e)
		{
			base.Size = new Size(base.Size.Width, this.grpbIP.Location.Y + this.grpbIP.Size.Height * 2);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00038AA4 File Offset: 0x00037AA4
		private void checkParam(string shouldBe, string inFact, string title, string desc)
		{
			wgTools.WriteLine(title);
			if (shouldBe != inFact)
			{
				string text = string.Concat(new string[]
				{
					title,
					": ",
					CommonStr.strShouldBe,
					shouldBe,
					CommonStr.strInfact,
					inFact,
					"\r\n",
					desc
				});
				this.logInfo(text, false);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00038B08 File Offset: 0x00037B08
		private bool CommunicteSocketTcpIsValid(string ipdest, int port)
		{
			Socket socket = null;
			bool flag = false;
			try
			{
				IPAddress ipaddress = IPAddress.Parse(ipdest);
				IPEndPoint ipendPoint = new IPEndPoint(ipaddress, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.SendTimeout = 1000;
				socket.ReceiveTimeout = 1000;
				socket.Connect(ipendPoint);
				if (socket.Connected)
				{
					flag = true;
				}
				socket.Close();
				socket = null;
			}
			catch
			{
				if (socket != null)
				{
					socket.Close();
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00038B8C File Offset: 0x00037B8C
		private void dfrmNetControllerFaultDeal_Load(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.updateInfoByID();
			Application.DoEvents();
			Cursor.Current = Cursors.WaitCursor;
			if (this.specialCheck() > 0)
			{
				base.WindowState = FormWindowState.Normal;
				base.StartPosition = FormStartPosition.CenterScreen;
				if (this.errtype == "ERR_WIFI")
				{
					this.lblDisableWifi.Visible = true;
					this.lblDisconnectCable.Visible = false;
				}
				else if (this.errtype == "ERR_CABLE")
				{
					this.lblDisableWifi.Visible = false;
					this.lblDisconnectCable.Visible = true;
				}
				else
				{
					this.lblDisableWifi.Visible = true;
					this.lblDisconnectCable.Visible = false;
				}
				if (string.IsNullOrEmpty(this.m_Controller.IP))
				{
					this.tabControl1.SelectedTab = this.tabPage1;
				}
				else
				{
					this.tabControl1.SelectedTab = this.tabPage2;
				}
				Cursor.Current = Cursors.Default;
				base.Activate();
				Application.DoEvents();
				return;
			}
			base.DialogResult = DialogResult.Yes;
			Cursor.Current = Cursors.Default;
			base.Close();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00038CA8 File Offset: 0x00037CA8
		private void getMaskGateway(string pcIPAddress, ref string mask, ref string gateway)
		{
			foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				IPInterfaceProperties ipproperties = networkInterface.GetIPProperties();
				UnicastIPAddressInformationCollection unicastAddresses = ipproperties.UnicastAddresses;
				if (unicastAddresses.Count > 0)
				{
					Console.WriteLine(networkInterface.Description);
					foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
					{
						if (!unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1" && unicastIPAddressInformation.Address.ToString() == pcIPAddress)
						{
							mask = unicastIPAddressInformation.IPv4Mask.ToString();
							if (ipproperties.GatewayAddresses.Count > 0)
							{
								gateway = ipproperties.GatewayAddresses[0].Address.ToString();
								break;
							}
							break;
						}
					}
					Console.WriteLine();
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00038DAC File Offset: 0x00037DAC
		private int getSnByTcp()
		{
			try
			{
				if (this.backgroundWorker1.IsBusy)
				{
					this.backgroundWorker1.CancelAsync();
					Thread.Sleep(200);
				}
				this.ip4TCP = this.m_Controller.IP;
				this.snGetByTCP = -2;
				this.backgroundWorker1.RunWorkerAsync();
				long num = DateTime.Now.Ticks + 4000000L;
				while (DateTime.Now.Ticks < num && this.snGetByTCP == -2)
				{
					Thread.Sleep(100);
				}
				if (this.snGetByTCP > 0)
				{
					return this.snGetByTCP;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return 0;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00038E74 File Offset: 0x00037E74
		private void IPConfigureCPU(string strSN, string strMac, string strIP, string strMask, string strGateway, string strTCPPort, string PCIPAddr, bool bAllowUseCommPassword)
		{
			if (this.wgudp != null)
			{
				this.wgudp = null;
			}
			IPAddress ipaddress;
			if (IPAddress.TryParse(PCIPAddr, out ipaddress))
			{
				this.wgudp = new wgUdpComm(IPAddress.Parse(PCIPAddr));
			}
			else
			{
				this.wgudp = new wgUdpComm();
			}
			Thread.Sleep(300);
			WGPacketWith1152 wgpacketWith = new WGPacketWith1152();
			wgpacketWith.type = 37;
			wgpacketWith.code = 32;
			wgpacketWith.iDevSnFrom = 0U;
			wgpacketWith.iCallReturn = 0;
			if (int.Parse(strSN) == -1)
			{
				wgpacketWith.iDevSnTo = uint.MaxValue;
			}
			else
			{
				wgpacketWith.iDevSnTo = uint.Parse(strSN);
			}
			byte[] array = new byte[1152];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			WGPacketWith1152 wgpacketWith2 = new WGPacketWith1152();
			int num = 116;
			IPAddress.Parse(strIP).GetAddressBytes().CopyTo(wgpacketWith2.ucData, num);
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 120;
			IPAddress.Parse(strMask).GetAddressBytes().CopyTo(wgpacketWith2.ucData, num);
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 124;
			if (string.IsNullOrEmpty(strGateway))
			{
				wgpacketWith2.ucData[num] = 0;
				wgpacketWith2.ucData[num + 1] = 0;
				wgpacketWith2.ucData[num + 2] = 0;
				wgpacketWith2.ucData[num + 3] = 0;
			}
			else
			{
				IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(wgpacketWith2.ucData, num);
			}
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 128;
			wgpacketWith2.ucData[num] = (byte)(int.Parse(strTCPPort) & 255);
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith2.ucData[num] = (byte)((int.Parse(strTCPPort) >> 8) & 255);
			wgpacketWith2.ucData[1024 + (num >> 3)] = wgpacketWith2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 0;
			for (int j = 0; j < 16; j++)
			{
				array[num] = wgpacketWith2.ucData[116 + j] & byte.MaxValue;
				array[1024 + (num >> 3)] = array[1024 + (num >> 3)] | (byte)(1 << (num & 7));
				num++;
			}
			array.CopyTo(wgpacketWith.ucData, 0);
			byte[] array2;
			if (bAllowUseCommPassword)
			{
				array2 = wgpacketWith.ToBytes(this.wgudp.udpPort);
			}
			else
			{
				array2 = wgpacketWith.ToBytesNoPassword(this.wgudp.udpPort);
			}
			if (array2 == null)
			{
				wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
				return;
			}
			byte[] array3 = null;
			this.wgudp.udp_get(array2, 300, 2147483647U, null, 60000, ref array3);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000393A0 File Offset: 0x000383A0
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000393D4 File Offset: 0x000383D4
		public bool isIPAddress(string ipstr)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(ipstr))
				{
					return flag;
				}
				string[] array = ipstr.Split(new char[] { '.' });
				if (array.Length != 4)
				{
					return flag;
				}
				flag = true;
				for (int i = 0; i <= 3; i++)
				{
					int num;
					if (!int.TryParse(array[i], out num))
					{
						flag = false;
						break;
					}
					if (num < 0 || num > 255)
					{
						flag = false;
						break;
					}
				}
				if (int.Parse(array[0]) == 0)
				{
					return false;
				}
				if (int.Parse(array[3]) == 255)
				{
					flag = false;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0003947C File Offset: 0x0003847C
		private void logInfo(string info, bool bOKColor)
		{
			this.infoNo++;
			int textLength = this.richTextBox1.TextLength;
			string text = "";
			this.richTextBox1.AppendText(this.infoNo.ToString() + ". ");
			text = text + this.infoNo.ToString() + ". ";
			this.richTextBox1.AppendText(info);
			wgAppConfig.wgLog("NetFaultDeal:  " + text + info);
			this.richTextBox1.AppendText("\r\n");
			int textLength2 = this.richTextBox1.TextLength;
			this.richTextBox1.Select(textLength, textLength2 - textLength);
			if (bOKColor)
			{
				this.richTextBox1.SelectionColor = Color.Blue;
			}
			else
			{
				this.richTextBox1.SelectionColor = Color.Black;
			}
			this.richTextBox1.Refresh();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00039558 File Offset: 0x00038558
		public int pingControllerIP(string ip)
		{
			byte[] array = new byte[6];
			uint num = (uint)array.Length;
			try
			{
				int num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(IPAddress.Parse(ip)), 0, array, ref num);
				if (num2 != 0)
				{
					num2 = -1;
				}
				Ping ping = new Ping();
				PingOptions pingOptions = new PingOptions();
				pingOptions.DontFragment = true;
				string text = "12345678901234567890123456789012";
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				int num3 = 500;
				DateTime now = DateTime.Now;
				if (ping.Send(ip, num3, bytes, pingOptions).Status == IPStatus.Success)
				{
					this.logInfo("Ping Timeout = " + (DateTime.Now - now).Milliseconds.ToString() + "ms", false);
					return 1;
				}
				return num2;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return -1;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00039644 File Offset: 0x00038644
		private void searchCurrentSNController()
		{
			string keyVal = wgAppConfig.GetKeyVal("KEY_FoundControllerIPs");
			if (!string.IsNullOrEmpty(keyVal))
			{
				using (icController icController = new icController())
				{
					icController.GetInfoFromDBByControllerID(this.ControllerID);
					if (string.IsNullOrEmpty(icController.IP))
					{
						NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int num5 = 0;
						foreach (NetworkInterface networkInterface in allNetworkInterfaces)
						{
							if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
							{
								num++;
								if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
								{
									num2++;
								}
								UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
								if (unicastAddresses.Count > 0)
								{
									Console.WriteLine(networkInterface.Description);
									bool flag = true;
									foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
									{
										if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
										{
											if (flag)
											{
												num3++;
												flag = false;
											}
											if (keyVal.IndexOf(unicastIPAddressInformation.Address + ",") >= 0)
											{
												this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
												Thread.Sleep(300);
												byte[] array2 = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
												byte[] array3 = null;
												this.wgudp.udp_get_notries(array2, 300, uint.MaxValue, null, 60000, ref array3);
												if (array3 == null)
												{
													continue;
												}
												if (string.IsNullOrEmpty(this.currentFoundControllerPCIPs) || this.currentFoundControllerPCIPs.IndexOf(unicastIPAddressInformation.Address + ",") < 0)
												{
													this.currentFoundControllerPCIPs = this.currentFoundControllerPCIPs + unicastIPAddressInformation.Address + ",";
												}
												if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
												{
													num4++;
												}
												else
												{
													num5++;
												}
											}
											Console.WriteLine();
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000398B4 File Offset: 0x000388B4
		private int specialCheck()
		{
			this.currentFoundControllerPCIPs = "";
			this.wgpktQuerySearch.type = 36;
			this.wgpktQuerySearch.code = 64;
			this.wgpktQuerySearch.iDevSnFrom = 0U;
			this.wgpktQuerySearch.iDevSnTo = (uint)this.m_Controller.ControllerSN;
			this.wgpktQuerySearch.iCallReturn = 0;
			int num;
			using (icController icController = new icController())
			{
				icController.GetInfoFromDBByControllerID(this.ControllerID);
				if (string.IsNullOrEmpty(icController.IP))
				{
					string keyVal = wgAppConfig.GetKeyVal("KEY_FoundControllerIPs");
					if (string.IsNullOrEmpty(keyVal))
					{
						num = 0;
					}
					else
					{
						NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int num5 = 0;
						int num6 = 0;
						foreach (NetworkInterface networkInterface in allNetworkInterfaces)
						{
							if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
							{
								num2++;
								if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
								{
									num3++;
								}
								UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
								if (unicastAddresses.Count > 0)
								{
									Console.WriteLine(networkInterface.Description);
									bool flag = true;
									foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
									{
										if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
										{
											if (flag)
											{
												num4++;
												flag = false;
											}
											if (keyVal.IndexOf(unicastIPAddressInformation.Address + ",") >= 0)
											{
												this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
												Thread.Sleep(300);
												byte[] array2 = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
												if (array2 == null)
												{
													return 0;
												}
												byte[] array3 = null;
												int num7 = this.wgudp.udp_get_notries(array2, 300, uint.MaxValue, null, 60000, ref array3);
												if (array3 == null)
												{
													continue;
												}
												wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array3, 20);
												this.strMac = wgMjControllerConfigure.MACAddr;
												this.strMask = wgMjControllerConfigure.mask.ToString();
												this.strGateway = wgMjControllerConfigure.gateway.ToString();
												if (string.IsNullOrEmpty(this.currentFoundControllerPCIPs) || this.currentFoundControllerPCIPs.IndexOf(unicastIPAddressInformation.Address + ",") < 0)
												{
													this.currentFoundControllerPCIPs = this.currentFoundControllerPCIPs + unicastIPAddressInformation.Address + ",";
												}
												if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
												{
													num5++;
												}
												else
												{
													num6++;
												}
											}
											Console.WriteLine();
										}
									}
								}
							}
						}
						if (num5 > 0 || num6 > 0)
						{
							this.logInfo(string.Format(CommonStr.strFoundController + this.m_Controller.ControllerSN, new object[0]), true);
							if (num2 < 2)
							{
								num = -1;
							}
							else
							{
								if (num3 >= 1 && num6 > 0)
								{
									this.logInfo(CommonStr.strSuggestionDisableWIFI, false);
									this.errtype = "ERR_WIFI";
								}
								else if (num3 >= 1 && num5 > 0 && num6 == 0)
								{
									this.logInfo(CommonStr.strSuggestionDisconnectNetworkCable, false);
									this.errtype = "ERR_CABLE";
								}
								else
								{
									this.logInfo(CommonStr.strSuggestionConfigureComplex, false);
								}
								num = 1;
							}
						}
						else
						{
							num = 0;
						}
					}
				}
				else
				{
					int controllerSN = icController.ControllerSN;
					icController.ControllerSN = -1;
					int num7 = icController.GetControllerRunInformationIP(-1);
					icController.ControllerSN = controllerSN;
					if (num7 <= 0)
					{
						this.logInfo(string.Format("SN= {0}  {1}", icController.ControllerSN, CommonStr.strCommFail), false);
					}
					else
					{
						if ((long)controllerSN != (long)((ulong)icController.runinfo.CurrentControllerSN))
						{
							this.checkParam(controllerSN.ToString(), icController.runinfo.CurrentControllerSN.ToString(), string.Format("IP " + icController.IP.ToString() + CommonStr.strControllerSN, new object[0]), CommonStr.strSuggestionSNIP);
							return 1;
						}
						if (icController.GetControllerRunInformationIP(-1) > 0)
						{
							this.logInfo(string.Format("SN= {0}  {1}", icController.ControllerSN, CommonStr.strCommOK), true);
							return -1;
						}
					}
					bool flag2 = false;
					string keyVal2 = wgAppConfig.GetKeyVal("KEY_FoundControllerIPs");
					if (!string.IsNullOrEmpty(keyVal2))
					{
						NetworkInterface[] allNetworkInterfaces2 = NetworkInterface.GetAllNetworkInterfaces();
						int num8 = 0;
						int num9 = 0;
						int num10 = 0;
						int num11 = 0;
						int num12 = 0;
						foreach (NetworkInterface networkInterface2 in allNetworkInterfaces2)
						{
							if (networkInterface2.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface2.OperationalStatus == OperationalStatus.Up)
							{
								num8++;
								if (networkInterface2.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
								{
									num9++;
								}
								UnicastIPAddressInformationCollection unicastAddresses2 = networkInterface2.GetIPProperties().UnicastAddresses;
								if (unicastAddresses2.Count > 0)
								{
									Console.WriteLine(networkInterface2.Description);
									bool flag3 = true;
									foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in unicastAddresses2)
									{
										if (unicastIPAddressInformation2.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation2.Address.IsIPv6LinkLocal && unicastIPAddressInformation2.Address.ToString() != "127.0.0.1")
										{
											if (flag3)
											{
												num10++;
												flag3 = false;
											}
											if (keyVal2.IndexOf(unicastIPAddressInformation2.Address + ",") >= 0)
											{
												this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
												Thread.Sleep(300);
												byte[] array5 = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
												if (array5 == null)
												{
													break;
												}
												byte[] array6 = null;
												num7 = this.wgudp.udp_get_notries(array5, 300, uint.MaxValue, null, 60000, ref array6);
												if (array6 == null)
												{
													continue;
												}
												wgMjControllerConfigure wgMjControllerConfigure2 = new wgMjControllerConfigure(array6, 20);
												this.strMac = wgMjControllerConfigure2.MACAddr;
												this.strMask = wgMjControllerConfigure2.mask.ToString();
												this.strGateway = wgMjControllerConfigure2.gateway.ToString();
												if (string.IsNullOrEmpty(this.currentFoundControllerPCIPs) || this.currentFoundControllerPCIPs.IndexOf(unicastIPAddressInformation2.Address + ",") < 0)
												{
													this.currentFoundControllerPCIPs = this.currentFoundControllerPCIPs + unicastIPAddressInformation2.Address + ",";
												}
												flag2 = true;
												if (!string.IsNullOrEmpty(icController.IP) && icController.IP != wgMjControllerConfigure2.ip.ToString())
												{
													this.checkParam(icController.IP.ToString(), wgMjControllerConfigure2.ip.ToString(), string.Format(CommonStr.strControllerSN + icController.ControllerSN.ToString() + "  IP ", new object[0]), CommonStr.strSuggestionSNIP);
													this.wgudp = new wgUdpComm();
													Thread.Sleep(300);
													array5 = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
													array6 = null;
													num7 = this.wgudp.udp_get_notries(array5, 300, uint.MaxValue, null, 60000, ref array6);
													if (array6 != null)
													{
														this.btnSwitchSmallNetwork.Visible = true;
														this.logInfo(string.Format("SN= {0}  {1}  {2}\r\n {3} {4}", new object[]
														{
															icController.ControllerSN,
															this.tabPage1.Text,
															CommonStr.strCommOK,
															CommonStr.strSuggestion,
															this.btnSwitchSmallNetwork.Text
														}), true);
													}
													return 1;
												}
												if (networkInterface2.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
												{
													num11++;
												}
												else
												{
													num12++;
												}
											}
											Console.WriteLine();
										}
									}
								}
							}
						}
					}
					this.wgudp = new wgUdpComm();
					Thread.Sleep(300);
					byte[] array7 = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
					byte[] array8 = null;
					num7 = this.wgudp.udp_get_notries(array7, 300, uint.MaxValue, null, 60000, ref array8);
					if (array8 != null)
					{
						flag2 = true;
						wgMjControllerConfigure wgMjControllerConfigure3 = new wgMjControllerConfigure(array8, 20);
						this.strMac = wgMjControllerConfigure3.MACAddr;
						this.strMask = wgMjControllerConfigure3.mask.ToString();
						this.strGateway = wgMjControllerConfigure3.gateway.ToString();
						if (!string.IsNullOrEmpty(icController.IP) && icController.IP != wgMjControllerConfigure3.ip.ToString())
						{
							this.checkParam(icController.IP.ToString(), wgMjControllerConfigure3.ip.ToString(), string.Format(CommonStr.strControllerSN + icController.ControllerSN.ToString() + "  IP ", new object[0]), CommonStr.strSuggestionSNIP);
						}
						this.btnSwitchSmallNetwork.Visible = true;
						this.logInfo(string.Format("SN= {0}  {1}  {2}\r\n {3} {4}", new object[]
						{
							icController.ControllerSN,
							this.tabPage1.Text,
							CommonStr.strCommOK,
							CommonStr.strSuggestion,
							this.btnSwitchSmallNetwork.Text
						}), true);
					}
					int num13 = this.pingControllerIP(icController.IP);
					if (num13 > 0)
					{
						this.logInfo(string.Format(" PING  {0} {1}", icController.IP, CommonStr.strCommOK), false);
					}
					else if (num13 == 0)
					{
						this.logInfo(string.Format(" PING  {0}    {1}. ({2})", icController.IP, CommonStr.strCommFail, CommonStr.strIPExisted), false);
					}
					else
					{
						this.logInfo(string.Format(" PING  {0}    {1}.", icController.IP, CommonStr.strCommFail), false);
					}
					int snByTcp;
					bool flag4;
					if (flag4 = (snByTcp = this.getSnByTcp()) > 0)
					{
						if (snByTcp.ToString() != icController.ControllerSN.ToString())
						{
							this.checkParam(icController.ControllerSN.ToString(), snByTcp.ToString(), string.Format("TCP/IP " + icController.IP.ToString() + CommonStr.strControllerSN, new object[0]), CommonStr.strSuggestionSNIP);
						}
						else
						{
							this.logInfo(string.Format("  TCP/IP  {0} {1}", icController.IP, CommonStr.strCommOK), false);
						}
					}
					else
					{
						this.logInfo(string.Format(" TCP/IP  {0}    {1}", icController.IP, CommonStr.strCommFail), false);
					}
					if (!flag2)
					{
						if (num13 >= 0 && !flag2 && !flag4)
						{
							this.logInfo(CommonStr.strSuggestionCheckSNIPCorrect, false);
						}
						else if (!flag2 && flag4)
						{
							this.logInfo(CommonStr.strSuggestionAllowUDP60000, false);
						}
						else
						{
							this.logInfo(CommonStr.strSuggestionMoreComplex, false);
						}
					}
					num = 1;
				}
			}
			return num;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0003A3C8 File Offset: 0x000393C8
		private int testGet(int ControllerSN, string IP, string PCIP, ref byte[] recv)
		{
			this.wgpktQuerySearch.type = 36;
			this.wgpktQuerySearch.code = 64;
			this.wgpktQuerySearch.iDevSnFrom = 0U;
			this.wgpktQuerySearch.iDevSnTo = (uint)ControllerSN;
			this.wgpktQuerySearch.iCallReturn = 0;
			this.wgudp = new wgUdpComm(IPAddress.Parse(PCIP));
			Thread.Sleep(300);
			byte[] array = this.wgpktQuerySearch.ToBytes(this.wgudp.udpPort);
			if (array == null)
			{
				return 0;
			}
			this.wgudp.udp_get_notries(array, 300, uint.MaxValue, null, 60000, ref recv);
			if (recv == null)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0003A470 File Offset: 0x00039470
		private int tryWEB_ByARP()
		{
			Cursor.Current = Cursors.WaitCursor;
			string text = "";
			if (string.IsNullOrEmpty(this.currentFoundControllerPCIPs))
			{
				this.searchCurrentSNController();
			}
			if (!string.IsNullOrEmpty(this.currentFoundControllerPCIPs))
			{
				string[] array = this.currentFoundControllerPCIPs.Split(new char[] { ',' });
				if (array.Length >= 1)
				{
					text = array[0];
				}
			}
			int num;
			if (!int.TryParse(this.txtf_ControllerSN.Text, out num))
			{
				XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return 0;
			}
			using (icController icController = new icController())
			{
				try
				{
					byte[] array2 = null;
					icController.ControllerSN = this.m_Controller.ControllerSN;
					if (this.testGet(this.m_Controller.ControllerSN, "", text, ref array2) > 0 && array2 != null)
					{
						wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array2, 20);
						string text2 = wgMjControllerConfigure.ip.ToString();
						bool flag = false;
						if (text2 != "192.168.0.0" && text2 != "192.168.168.0" && text2 != "255.255.255.255" && text2 != "")
						{
							icController.IP = text2;
							if (icController.GetControllerRunInformationIP(-1) > 0 && wgAppConfig.getValBySql(string.Format("Select f_ControllerSN from t_b_controller Where f_IP={0} and (not (f_controllerID ={1}))", wgTools.PrepareStrNUnicode(text2.ToString()), this.m_Controller.ControllerID)) <= 0)
							{
								flag = true;
							}
							icController.IP = "";
						}
						if (!flag)
						{
							IPAddress ipaddress = IPAddress.Parse(text);
							byte[] array3 = new byte[4];
							array3 = ipaddress.GetAddressBytes();
							if (array3[3] != 123)
							{
								array3[3] = 123;
								ipaddress = new IPAddress(array3);
							}
							byte[] array4 = new byte[6];
							uint num2 = (uint)array4.Length;
							int num3;
							if (wgAppConfig.getValBySql(string.Format("Select f_ControllerSN from t_b_controller Where f_IP={0} and (not (f_controllerID ={1}))", wgTools.PrepareStrNUnicode(ipaddress.ToString()), this.m_Controller.ControllerID)) <= 0)
							{
								num3 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text)), array4, ref num2);
							}
							else
							{
								num3 = 0;
							}
							if (num3 == 0)
							{
								array3[3] = array3[3] + 1;
								while (array3[3] != 123)
								{
									if (array3[3] == 0 || array3[3] == 255)
									{
										array3[3] = array3[3] + 1;
									}
									else
									{
										ipaddress = new IPAddress(array3);
										num2 = (uint)array4.Length;
										if (wgAppConfig.getValBySql(string.Format("Select f_ControllerSN from t_b_controller Where f_IP={0} and (not (f_controllerID ={1}))", wgTools.PrepareStrNUnicode(ipaddress.ToString()), this.m_Controller.ControllerID)) <= 0)
										{
											num3 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text)), array4, ref num2);
										}
										else
										{
											num3 = 0;
										}
										if (num3 != 0)
										{
											break;
										}
										array3[3] = array3[3] + 1;
									}
								}
							}
							if (num3 != 0)
							{
								byte[] array5 = new byte[1152];
								for (int i = 0; i < array5.Length; i++)
								{
									array5[i] = 0;
								}
								string text3 = "";
								string text4 = "";
								this.getMaskGateway(text, ref text3, ref text4);
								this.strMac = wgMjControllerConfigure.MACAddr;
								this.strMask = text3;
								this.strGateway = text4;
								this.IPConfigureCPU(this.m_Controller.ControllerSN.ToString(), this.strMac, ipaddress.ToString(), this.strMask, this.strGateway, 60000.ToString(), text, true);
								Thread.Sleep(2000);
							}
							int num4 = 3;
							num2 = (uint)array4.Length;
							num3 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text)), array4, ref num2);
							while (num3 != 0 && num4-- > 0)
							{
								Thread.Sleep(500);
								num2 = (uint)array4.Length;
								num3 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text)), array4, ref num2);
							}
							if (num3 == 0)
							{
								text2 = ipaddress.ToString();
								flag = true;
							}
						}
						if (flag)
						{
							icController.IP = text2;
							if (icController.GetControllerRunInformationIP(text, -1) <= 0)
							{
								this.logInfo(string.Format("{0} {1}", this.btnTryAutoConfigureIP.Text, CommonStr.strFailed) + string.Format("{0} {1} {2}", CommonStr.strController, this.m_Controller.ControllerSN.ToString(), CommonStr.strCommFail), false);
								return 0;
							}
							wgAppConfig.runSql(string.Format("UPDATE t_b_Controller SET f_IP={0}, f_PORT = 60000 WHERE f_ControllerID={1}", wgTools.PrepareStrNUnicode(text2), this.ControllerID));
							this.updateInfoByID();
							this.logInfo(string.Format("{0} {1}", this.btnTryAutoConfigureIP.Text, CommonStr.strSuccessfully), true);
							Cursor.Current = Cursors.WaitCursor;
							this.specialCheck();
							Cursor.Current = Cursors.Default;
						}
						else
						{
							this.logInfo(string.Format("{0} {1}", this.btnTryAutoConfigureIP.Text, CommonStr.strFailed) + string.Format("{0} {1} {2}", CommonStr.strController, this.m_Controller.ControllerSN.ToString(), CommonStr.strCommFail), false);
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			Cursor.Current = Cursors.Default;
			return 0;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0003A9E0 File Offset: 0x000399E0
		private void updateInfoByID()
		{
			this.m_Controller.GetInfoFromDBByControllerID(this.ControllerID);
			this.m_Controller.ControllerID = this.ControllerID;
			this.txtf_ControllerSN.Text = this.m_Controller.ControllerSN.ToString();
			if (this.m_Controller.IP == "")
			{
				this.optIPSmall.Checked = true;
				this.grpbIP.Visible = false;
				this.tabPage1.Parent = this.tabControl1;
				this.tabPage2.Parent = null;
				this.lblIPComplex.Visible = true;
				this.lblManual.Visible = true;
				return;
			}
			this.optIPLarge.Checked = true;
			this.grpbIP.Visible = true;
			this.txtControllerIP.Text = this.m_Controller.IP;
			this.nudPort.Value = this.m_Controller.PORT;
			this.lblIP.ForeColor = Color.White;
			this.tabPage2.Parent = this.tabControl1;
			this.tabPage1.Parent = null;
			this.lblIPComplex.Visible = false;
			this.lblManual.Visible = false;
		}

		// Token: 0x0400039A RID: 922
		private int infoNo;

		// Token: 0x0400039C RID: 924
		public int ControllerID;

		// Token: 0x0400039D RID: 925
		private string currentFoundControllerPCIPs = "";

		// Token: 0x0400039E RID: 926
		private string errtype = "";

		// Token: 0x0400039F RID: 927
		private string ip4TCP = "";

		// Token: 0x040003A1 RID: 929
		private int snGetByTCP = -2;

		// Token: 0x040003A2 RID: 930
		public string strGateway = "";

		// Token: 0x040003A3 RID: 931
		public string strMac = "";

		// Token: 0x040003A4 RID: 932
		public string strMask = "";

		// Token: 0x040003A5 RID: 933
		private WGPacket wgpktQuerySearch = new WGPacket();
	}
}
