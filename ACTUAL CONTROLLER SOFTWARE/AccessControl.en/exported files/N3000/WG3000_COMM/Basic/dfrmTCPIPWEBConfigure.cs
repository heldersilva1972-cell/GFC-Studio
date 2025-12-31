using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000030 RID: 48
	public partial class dfrmTCPIPWEBConfigure : frmN3000
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0006011C File Offset: 0x0005F11C
		public dfrmTCPIPWEBConfigure()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView3);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000601A4 File Offset: 0x0005F1A4
		private void btnDownloadUsers_Click(object sender, EventArgs e)
		{
			try
			{
				int num = int.Parse(this.txtf_ControllerSN.Text);
				Cursor.Current = Cursors.WaitCursor;
				using (wgMjControllerPrivilege wgMjControllerPrivilege = new wgMjControllerPrivilege())
				{
					wgMjControllerPrivilege.AllowDownload();
					if (this.dtPrivilege != null)
					{
						this.dtPrivilege.Rows.Clear();
						this.dtPrivilege.Dispose();
						this.dtPrivilege = null;
						GC.Collect();
					}
					if (this.dtPrivilege == null)
					{
						this.dtPrivilege = new DataTable(wgAppConfig.dbWEBUserName);
						this.dtPrivilege.Columns.Add("f_CardNO", Type.GetType("System.Int64"));
						this.dtPrivilege.Columns.Add("f_BeginYMD", Type.GetType("System.DateTime"));
						this.dtPrivilege.Columns.Add("f_EndYMD", Type.GetType("System.DateTime"));
						this.dtPrivilege.Columns.Add("f_PIN", Type.GetType("System.String"));
						this.dtPrivilege.Columns.Add("f_ControlSegID1", Type.GetType("System.Byte"));
						this.dtPrivilege.Columns["f_ControlSegID1"].DefaultValue = 0;
						this.dtPrivilege.Columns.Add("f_ControlSegID2", Type.GetType("System.Byte"));
						this.dtPrivilege.Columns["f_ControlSegID2"].DefaultValue = 0;
						this.dtPrivilege.Columns.Add("f_ControlSegID3", Type.GetType("System.Byte"));
						this.dtPrivilege.Columns["f_ControlSegID3"].DefaultValue = 0;
						this.dtPrivilege.Columns.Add("f_ControlSegID4", Type.GetType("System.Byte"));
						this.dtPrivilege.Columns["f_ControlSegID4"].DefaultValue = 0;
						this.dtPrivilege.Columns.Add("f_AllowFloors", Type.GetType("System.UInt64"));
						this.dtPrivilege.Columns["f_AllowFloors"].DefaultValue = 1099511627775L;
						this.dtPrivilege.Columns.Add("f_ConsumerName", Type.GetType("System.String"));
						this.dtPrivilege.Columns.Add("f_IsDeleted", Type.GetType("System.UInt32"));
					}
					if (wgMjControllerPrivilege.DownloadIP(num, null, 60000, "INCLUDEDELETED", ref this.dtPrivilege, this.strPCAddress) > 0)
					{
						if (this.dtPrivilege.Rows.Count >= 0)
						{
							this.dtPrivilege.Columns.Remove("f_BeginYMD");
							this.dtPrivilege.Columns.Remove("f_EndYMD");
							this.dtPrivilege.Columns.Remove("f_PIN");
							this.dtPrivilege.Columns.Remove("f_ControlSegID1");
							this.dtPrivilege.Columns.Remove("f_ControlSegID2");
							this.dtPrivilege.Columns.Remove("f_ControlSegID3");
							this.dtPrivilege.Columns.Remove("f_ControlSegID4");
							this.dtPrivilege.AcceptChanges();
							this.dv = new DataView(this.dtPrivilege);
							this.dv.RowFilter = "f_IsDeleted = 1";
							if (this.dv.Count > 0)
							{
								for (int i = this.dv.Count - 1; i >= 0; i--)
								{
									this.dv.Delete(i);
								}
							}
							this.dtPrivilege.AcceptChanges();
							this.dtPrivilege.Columns.Remove("f_IsDeleted");
							this.dtPrivilege.AcceptChanges();
							string text = string.Concat(new string[]
							{
								wgAppConfig.Path4Doc(),
								wgAppConfig.dbWEBUserName,
								"_",
								DateTime.Now.ToString("yyyyMMddHHmmss"),
								".xml"
							});
							using (StringWriter stringWriter = new StringWriter())
							{
								this.dtPrivilege.WriteXml(stringWriter, XmlWriteMode.WriteSchema, true);
								using (StreamWriter streamWriter = new StreamWriter(text, false))
								{
									streamWriter.Write(stringWriter.ToString());
								}
							}
							XMessageBox.Show((sender as Button).Text + "\r\n\r\n" + text);
						}
						else
						{
							XMessageBox.Show(string.Concat(new string[]
							{
								(sender as Button).Text,
								" ",
								num.ToString(),
								" ",
								CommonStr.strFailed
							}));
						}
					}
					else
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							(sender as Button).Text,
							" ",
							num.ToString(),
							" ",
							CommonStr.strFailed
						}));
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00060760 File Offset: 0x0005F760
		private void btnEditUsers_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00060764 File Offset: 0x0005F764
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!this.txtf_ControllerSN.ReadOnly)
			{
				this.txtf_ControllerSN.Text = this.txtf_ControllerSN.Text.Trim();
				int num;
				if (!int.TryParse(this.txtf_ControllerSN.Text, out num))
				{
					XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (wgMjController.GetControllerType(int.Parse(this.txtf_ControllerSN.Text)) == 0)
				{
					XMessageBox.Show(this, CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (this.chkEditIP.Checked)
			{
				if (string.IsNullOrEmpty(this.txtf_IP.Text))
				{
					XMessageBox.Show(this, CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", "");
				if (!this.isIPAddress(this.txtf_IP.Text))
				{
					XMessageBox.Show(this, this.txtf_IP.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", "");
				if (!this.isIPAddress(this.txtf_mask.Text))
				{
					XMessageBox.Show(this, this.txtf_mask.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", "");
				if (!string.IsNullOrEmpty(this.txtf_gateway.Text) && !this.isIPAddress(this.txtf_gateway.Text))
				{
					XMessageBox.Show(this, this.txtf_gateway.Text + "  " + CommonStr.strIPAddrWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (this.chkUpdateWebSet.Checked && (this.nudHttpPort.Value == 60000m || this.nudHttpPort.Value == this.nudPort.Value))
			{
				XMessageBox.Show(this, this.lblHttpPort.Text + "  " + CommonStr.strHttpWEBWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.chkUpdateWebSet.Checked && this.cboLanguage.SelectedIndex == 2)
			{
				if (string.IsNullOrEmpty(this.txtSelectedFileName.Text))
				{
					XMessageBox.Show(CommonStr.strTranslateFileSelect);
					return;
				}
				if (this.dtWebString == null)
				{
					bool flag = false;
					string text = this.txtSelectedFileName.Text;
					if (File.Exists(text))
					{
						try
						{
							this.tb1 = new DataTable();
							this.tb1.TableName = "WEBString";
							this.tb1.Columns.Add("f_NO");
							this.tb1.Columns.Add("f_Name");
							this.tb1.Columns.Add("f_Value");
							this.tb1.Columns.Add("f_CName");
							this.tb1.ReadXml(text);
							this.tb1.AcceptChanges();
							if (this.tb1.Rows.Count == 174)
							{
								bool flag2 = true;
								for (int i = 0; i < this.tb1.Rows.Count; i++)
								{
									if (string.IsNullOrEmpty(this.tb1.Rows[i]["f_Value"].ToString()))
									{
										XMessageBox.Show(string.Format("{0} {1}", this.tb1.Rows[i]["f_NO"].ToString(), CommonStr.strTranslateValueInvavid));
										return;
									}
								}
								if (flag2)
								{
									flag = true;
									this.dtWebString = this.tb1.Copy();
								}
							}
						}
						catch
						{
						}
					}
					if (flag)
					{
						goto IL_0411;
					}
					XMessageBox.Show(CommonStr.strTranslateFileInvalid);
					return;
				}
			}
			IL_0411:
			if (this.chkAutoUploadWEBUsers.Checked)
			{
				if (string.IsNullOrEmpty(this.txtUsersFile.Text))
				{
					XMessageBox.Show(CommonStr.strUserFileSelect);
					return;
				}
				if (!File.Exists(this.txtUsersFile.Text))
				{
					XMessageBox.Show(CommonStr.strUserFileSelect);
					return;
				}
			}
			this.strSN = this.txtf_ControllerSN.Text;
			this.strMac = this.txtf_MACAddr.Text;
			this.strIP = this.txtf_IP.Text;
			this.strMask = this.txtf_mask.Text;
			this.strGateway = this.txtf_gateway.Text;
			this.strTCPPort = this.nudPort.Value.ToString();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00060C64 File Offset: 0x0005FC64
		private void btnOption_Click(object sender, EventArgs e)
		{
			this.btnOption.Enabled = false;
			this.lblPort.Visible = true;
			this.nudPort.Visible = true;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00060C8A File Offset: 0x0005FC8A
		private void btnOptionWEB_Click(object sender, EventArgs e)
		{
			this.btnOptionWEB.Enabled = false;
			this.lblHttpPort.Visible = true;
			this.nudHttpPort.Visible = true;
			this.label6.Visible = true;
			this.cboDateFormat.Visible = true;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00060CC8 File Offset: 0x0005FCC8
		private void btnOtherLanguage_Click(object sender, EventArgs e)
		{
			using (dfrmTranslate dfrmTranslate = new dfrmTranslate())
			{
				dfrmTranslate.ShowDialog();
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00060D00 File Offset: 0x0005FD00
		private void btnRestoreNameAndPassword_Click(object sender, EventArgs e)
		{
			try
			{
				if (XMessageBox.Show(CommonStr.strRebootController4Restore, (sender as Button).Text, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					using (icController icController = new icController())
					{
						icController.ControllerSN = int.Parse(this.txtf_ControllerSN.Text);
						icController.UpdateFRamIP(268435457U, 0U);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00060D98 File Offset: 0x0005FD98
		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			try
			{
				this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				this.openFileDialog1.Title = (sender as Button).Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					bool flag = false;
					string text = fileName;
					try
					{
						if (File.Exists(text))
						{
							this.tb2 = new DataTable();
							this.tb2.TableName = "WEBString";
							this.tb2.Columns.Add("f_NO");
							this.tb2.Columns.Add("f_Name");
							this.tb2.Columns.Add("f_Value");
							this.tb2.Columns.Add("f_CName");
							this.tb2.ReadXml(text);
							this.tb2.AcceptChanges();
							if (this.tb2.Rows.Count == 174)
							{
								bool flag2 = true;
								for (int i = 0; i < this.tb2.Rows.Count; i++)
								{
									if (string.IsNullOrEmpty(this.tb2.Rows[i]["f_Value"].ToString()))
									{
										XMessageBox.Show(string.Format(CommonStr.strTranslateValueInvavid, this.tb2.Rows[i]["f_NO"].ToString()));
										return;
									}
								}
								if (flag2)
								{
									flag = true;
									this.dtWebString = this.tb2.Copy();
								}
							}
						}
						if (!flag)
						{
							XMessageBox.Show(CommonStr.strTranslateFileInvalid);
							return;
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
					this.txtSelectedFileName.Text = fileName;
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00060FE0 File Offset: 0x0005FFE0
		private void btnSelectUserFile_Click(object sender, EventArgs e)
		{
			try
			{
				this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				this.openFileDialog1.Title = (sender as Button).Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					bool flag = false;
					string text = fileName;
					try
					{
						if (File.Exists(text))
						{
							this.tb = new DataTable();
							this.tb.TableName = wgAppConfig.dbWEBUserName;
							this.tb.Columns.Add("f_CardNO");
							this.tb.Columns.Add("f_ConsumerName");
							this.tb.ReadXml(text);
							this.tb.AcceptChanges();
							flag = true;
							this.dtUsers = this.tb.Copy();
						}
						if (!flag)
						{
							XMessageBox.Show(CommonStr.strUsersFileInvalid);
							return;
						}
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
					this.txtUsersFile.Text = fileName;
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0006115C File Offset: 0x0006015C
		private void btnTryWEB_Click(object sender, EventArgs e)
		{
			this.tryWEB_ByARP();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00061164 File Offset: 0x00060164
		private void btnuploadUser_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtUsersFile.Text))
			{
				XMessageBox.Show(CommonStr.strUserFileSelect);
				return;
			}
			if (!File.Exists(this.txtUsersFile.Text))
			{
				XMessageBox.Show(CommonStr.strUserFileSelect);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000611C1 File Offset: 0x000601C1
		private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000611C3 File Offset: 0x000601C3
		private void chkEditIP_CheckedChanged(object sender, EventArgs e)
		{
			this.grpIP.Enabled = this.chkEditIP.Checked;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000611DB File Offset: 0x000601DB
		private void chkUpdateSpecialCard_CheckedChanged(object sender, EventArgs e)
		{
			this.grpSpecialCards.Visible = this.chkUpdateSpecialCard.Checked;
			this.grpSpecialCards.Enabled = this.chkUpdateSpecialCard.Checked;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00061209 File Offset: 0x00060209
		private void chkUpdateSuperCard_CheckedChanged(object sender, EventArgs e)
		{
			this.grpSuperCards.Enabled = this.chkUpdateSuperCard.Checked;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00061221 File Offset: 0x00060221
		private void chkUpdateWebSet_CheckedChanged(object sender, EventArgs e)
		{
			this.grpWEBEnabled.Enabled = this.chkUpdateWebSet.Checked;
			this.grpWEB.Enabled = this.grpWEBEnabled.Enabled && this.optWEBEnabled.Checked;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00061260 File Offset: 0x00060260
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

		// Token: 0x06000359 RID: 857 RVA: 0x000612E4 File Offset: 0x000602E4
		private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsActivateCard19)
			{
				this.txtSuperCard1.Mask = "9999999999999999999";
				this.txtSuperCard2.Mask = "9999999999999999999";
			}
			else
			{
				this.txtSuperCard1.Mask = "9999999999";
				this.txtSuperCard2.Mask = "9999999999";
			}
			this.grpWEBUsers.Visible = false;
			this.btnuploadUser.Visible = false;
			this.chkAutoUploadWEBUsers.Checked = false;
			this.chkAutoUploadWEBUsers.Visible = false;
			this.txtf_ControllerSN.Text = this.strSN;
			this.txtf_MACAddr.Text = this.strMac;
			this.txtf_IP.Text = this.strIP;
			this.txtf_mask.Text = this.strMask;
			this.txtf_gateway.Text = this.strGateway;
			if (string.IsNullOrEmpty(this.strTCPPort))
			{
				this.strTCPPort = 60000.ToString();
			}
			else if (int.Parse(this.strTCPPort) < this.nudPort.Minimum || int.Parse(this.strTCPPort) >= 65535)
			{
				this.strTCPPort = 60000.ToString();
			}
			this.nudPort.Value = int.Parse(this.strTCPPort);
			if (this.txtf_IP.Text == "255.255.255.255")
			{
				this.txtf_IP.Text = "192.168.0.0";
			}
			if (this.txtf_mask.Text == "255.255.255.255")
			{
				this.txtf_mask.Text = "255.255.255.0";
			}
			if (this.txtf_gateway.Text == "255.255.255.255")
			{
				this.txtf_gateway.Text = "";
			}
			if (this.txtf_gateway.Text == "0.0.0.0")
			{
				this.txtf_gateway.Text = "";
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000614E0 File Offset: 0x000604E0
		private void dfrmTCPIPWEBConfigure_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.funCtrlShiftQ();
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00061540 File Offset: 0x00060540
		private void funCtrlShiftQ()
		{
			if (this.btnRestoreNameAndPassword.Visible)
			{
				this.chkUpdateSpecialCard.Visible = true;
			}
			this.chkAutoUploadWEBUsers.Visible = true;
			this.btnRestoreNameAndPassword.Visible = true;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00061574 File Offset: 0x00060574
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

		// Token: 0x0600035D RID: 861 RVA: 0x00061678 File Offset: 0x00060678
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000616AC File Offset: 0x000606AC
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

		// Token: 0x0600035F RID: 863 RVA: 0x00061754 File Offset: 0x00060754
		private void optWEBEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.grpWEB.Enabled = this.optWEBEnabled.Checked;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0006176C File Offset: 0x0006076C
		private void tryWEB_ByARP()
		{
			Cursor.Current = Cursors.WaitCursor;
			this.btnTryWEB.Enabled = false;
			icController icController = new icController();
			try
			{
				string text = "";
				icController.ControllerSN = int.Parse(this.strSN);
				if (icController.GetControllerRunInformationIP(this.strPCAddress, -1) <= 0)
				{
					XMessageBox.Show(string.Format("{0} {1} {2}", CommonStr.strController, this.strSN, CommonStr.strCommFail));
				}
				else
				{
					wgMjControllerConfigure wgMjControllerConfigure = null;
					icController.GetConfigureIP(ref wgMjControllerConfigure);
					if (wgMjControllerConfigure != null)
					{
						text = wgMjControllerConfigure.ip.ToString();
					}
					bool flag = false;
					if (text != "192.168.0.0" && text != "192.168.168.0" && text != "255.255.255.255" && text != "")
					{
						icController.IP = text;
						if (icController.GetControllerRunInformationIP(this.strPCAddress, -1) > 0)
						{
							flag = true;
						}
						icController.IP = "";
					}
					if (!flag)
					{
						IPAddress ipaddress = IPAddress.Parse(this.strPCAddress);
						byte[] array = new byte[4];
						array = ipaddress.GetAddressBytes();
						if (array[3] != 123)
						{
							array[3] = 123;
							ipaddress = new IPAddress(array);
						}
						byte[] array2 = new byte[6];
						uint num = (uint)array2.Length;
						int num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(this.strPCAddress)), array2, ref num);
						if (num2 == 0)
						{
							array[3] = array[3] + 1;
							while (array[3] != 123)
							{
								if (array[3] == 0 || array[3] == 255)
								{
									array[3] = array[3] + 1;
								}
								else
								{
									ipaddress = new IPAddress(array);
									num = (uint)array2.Length;
									num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(this.strPCAddress)), array2, ref num);
									if (num2 != 0)
									{
										break;
									}
									array[3] = array[3] + 1;
								}
							}
						}
						if (num2 != 0)
						{
							byte[] array3 = new byte[1152];
							for (int i = 0; i < array3.Length; i++)
							{
								array3[i] = 0;
							}
							int num3 = 80;
							int num4 = 12288;
							if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
							{
								num4 = 8192;
							}
							int num5 = 100;
							array3[num5] = (byte)(num4 & 255);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							num5++;
							array3[num5] = (byte)(num4 >> 8);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							num5++;
							array3[num5] = (byte)(num4 >> 16);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							num5++;
							array3[num5] = (byte)(num4 >> 24);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							num5 = 96;
							array3[num5] = (byte)(num3 & 255);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							num5++;
							array3[num5] = (byte)(num3 >> 8);
							array3[1024 + (num5 >> 3)] = array3[1024 + (num5 >> 3)] | (byte)(1 << (num5 & 7));
							icController.UpdateConfigureCPUSuperIP(array3, "", this.strPCAddress);
							string text2 = "";
							string text3 = "";
							this.getMaskGateway(this.strPCAddress, ref text2, ref text3);
							icController.NetIPConfigure(icController.ControllerSN.ToString(), wgMjControllerConfigure.MACAddr, ipaddress.ToString(), text2, text3, 60000.ToString(), this.strPCAddress);
							Thread.Sleep(2000);
						}
						int num6 = 3;
						num = (uint)array2.Length;
						num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(this.strPCAddress)), array2, ref num);
						while (num2 != 0 && num6-- > 0)
						{
							Thread.Sleep(500);
							num = (uint)array2.Length;
							num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(this.strPCAddress)), array2, ref num);
						}
						if (num2 == 0)
						{
							text = ipaddress.ToString();
							flag = true;
						}
					}
					if (flag && !this.CommunicteSocketTcpIsValid(text, 80))
					{
						byte[] array4 = new byte[1152];
						for (int j = 0; j < array4.Length; j++)
						{
							array4[j] = 0;
						}
						int num7 = 80;
						int num8 = 12288;
						if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
						{
							num8 = 8192;
						}
						int num9 = 100;
						array4[num9] = (byte)(num8 & 255);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						num9++;
						array4[num9] = (byte)(num8 >> 8);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						num9++;
						array4[num9] = (byte)(num8 >> 16);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						num9++;
						array4[num9] = (byte)(num8 >> 24);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						num9 = 96;
						array4[num9] = (byte)(num7 & 255);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						num9++;
						array4[num9] = (byte)(num7 >> 8);
						array4[1024 + (num9 >> 3)] = array4[1024 + (num9 >> 3)] | (byte)(1 << (num9 & 7));
						icController.UpdateConfigureCPUSuperIP(array4, "", this.strPCAddress);
						icController.RebootControllerIP(this.strPCAddress);
						Thread.Sleep(2000);
					}
					if (flag)
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = "HTTP://" + text,
							UseShellExecute = true
						});
					}
					else
					{
						XMessageBox.Show(string.Format("{0} {1} {2}", CommonStr.strController, this.strSN, CommonStr.strFailed));
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				if (icController != null)
				{
					icController.Dispose();
				}
				Cursor.Current = Cursors.Default;
				this.btnTryWEB.Enabled = true;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00061E50 File Offset: 0x00060E50
		private void txtSuperCard1_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtSuperCard1);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00061E5D File Offset: 0x00060E5D
		private void txtSuperCard1_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtSuperCard1);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00061E6A File Offset: 0x00060E6A
		private void txtSuperCard2_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtSuperCard2);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00061E77 File Offset: 0x00060E77
		private void txtSuperCard2_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtSuperCard2);
		}

		// Token: 0x0400064D RID: 1613
		private const int webStringCount = 174;

		// Token: 0x0400064E RID: 1614
		private DataTable dtPrivilege;

		// Token: 0x0400064F RID: 1615
		private DataView dv;

		// Token: 0x04000650 RID: 1616
		private DataTable tb;

		// Token: 0x04000651 RID: 1617
		private DataTable tb1;

		// Token: 0x04000652 RID: 1618
		private DataTable tb2;

		// Token: 0x04000653 RID: 1619
		public DataTable dtUsers;

		// Token: 0x04000654 RID: 1620
		public DataTable dtWebString;

		// Token: 0x04000655 RID: 1621
		public string strGateway = "";

		// Token: 0x04000656 RID: 1622
		public string strIP = "";

		// Token: 0x04000657 RID: 1623
		public string strMac = "";

		// Token: 0x04000658 RID: 1624
		public string strMask = "";

		// Token: 0x04000659 RID: 1625
		public string strPCAddress = "";

		// Token: 0x0400065A RID: 1626
		public string strSearchedIP = "";

		// Token: 0x0400065B RID: 1627
		public string strSearchedMask = "";

		// Token: 0x0400065C RID: 1628
		public string strSN = "";

		// Token: 0x0400065D RID: 1629
		public string strTCPPort = "";
	}
}
