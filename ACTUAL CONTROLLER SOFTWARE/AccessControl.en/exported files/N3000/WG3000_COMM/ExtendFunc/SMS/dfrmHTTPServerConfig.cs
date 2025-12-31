using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.SMS
{
	// Token: 0x02000324 RID: 804
	public partial class dfrmHTTPServerConfig : frmN3000
	{
		// Token: 0x06001920 RID: 6432 RVA: 0x0020EE7C File Offset: 0x0020DE7C
		public dfrmHTTPServerConfig()
		{
			this.InitializeComponent();
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0020EEFE File Offset: 0x0020DEFE
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0020EF10 File Offset: 0x0020DF10
		private void btnDefaultParameter_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnDefaultParameter.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				wgAppConfig.setSystemParamValue(196, "HTTPServer Configuration 2017-12-10 13:57:49", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "  ");
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0020EFC8 File Offset: 0x0020DFC8
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.setKeyval(this.txtUserName);
			this.setKeyval(this.txtPassword);
			this.setKeyval(this.txtHostDomain);
			this.setKeyval(this.txtHostIP);
			this.setKeyval(this.txtPostInfo);
			this.setKeyval(this.txtPort);
			this.setKeyval(this.txtMarkContent);
			this.setKeyval(this.txtMarkMobile);
			this.setKeyval(this.txtSpecialMobile);
			this.setKeyval(this.txtContentSignInfo);
			this.setKeyval(this.chkContent0);
			this.setKeyval(this.chkContent1);
			this.setKeyval(this.chkContent2);
			this.setKeyval(this.chkContent3);
			this.setKeyval(this.chkContent4);
			this.setKeyval(this.chkContent5);
			this.setKeyval(this.chkContent6);
			this.setKeyval(this.chkSpecialEvent0);
			this.setKeyval(this.chkSpecialEvent1);
			this.setKeyval(this.chkSpecialEvent2);
			this.setKeyval(this.chkSpecialEvent3);
			this.setKeyval(this.chkSpecialEvent4);
			this.setKeyval(this.chkSpecialEvent5);
			this.setKeyval(this.chkSpecialEvent6);
			this.setKeyval(this.chkSpecialEvent7);
			this.setKeyval(this.chkUploadMobile);
			this.setKeyval(this.chkOnlyOnce60Second);
			this.setKeyval(this.chkCheckPhoneValid);
			this.setKeyval(this.cbof_ControlSegID);
			this.setKeyval(this.cboSeparator);
			this.setKeyval(this.chkIncludeSN);
			this.setKeyval(this.txtUDPServer);
			this.setKeyval(this.txtPortShort);
			this.setKeyval(this.nudCycle);
			this.setKeyval(this.chkSwipe);
			this.saveKeyval();
			base.DialogResult = DialogResult.Cancel;
			byte[] array = new byte[]
			{
				25, 144, 0, 0, 177, 152, 167, 25, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			Array.Copy(BitConverter.GetBytes(this.ControllerSN), 0, array, 4, 4);
			icController icController = new icController();
			try
			{
				byte[] array2 = null;
				icController.ControllerSN = this.ControllerSN;
				int num = icController.ControllerSN / 10000000;
				switch (num % 10)
				{
				case 1:
					array[0] = 17;
					break;
				case 2:
					array[0] = 25;
					break;
				case 3:
				case 7:
					array[0] = 19;
					break;
				case 5:
					array[0] = 21;
					break;
				}
				if (icController.ControllerSN >= 171100001 && icController.ControllerSN < 171100099)
				{
					array[0] = 19;
				}
				else if (icController.ControllerSN >= 171200001 && icController.ControllerSN < 172000000)
				{
					array[0] = 17;
				}
				else if (icController.ControllerSN >= 172000001 && icController.ControllerSN < 173000000)
				{
					array[0] = 25;
				}
				else if (icController.ControllerSN >= 175000001 && icController.ControllerSN < 176000000)
				{
					array[0] = 21;
				}
				byte[] array3 = new byte[64];
				array.CopyTo(array3, 0);
				array[1] = 146;
				icController.ShortPacketSend(array, ref array2);
				if (array2 == null)
				{
					XMessageBox.Show(CommonStr.strFailed);
					return;
				}
				array3.CopyTo(array, 0);
				Array.Copy(array2, 8, array, 8, 17);
				Array.Copy(array2, 44, array, 44, 6);
				array[1] = 144;
				icController.ControllerSN = this.ControllerSN;
				IPAddress ipaddress = IPAddress.Parse("0.0.0.0");
				IPAddress.TryParse(this.txtHostIP.Text, out ipaddress);
				ipaddress.GetAddressBytes().CopyTo(array, 24);
				array[28] = (byte)(int.Parse(this.txtPort.Text) & 255);
				array[29] = (byte)((int.Parse(this.txtPort.Text) >> 8) & 255);
				array[30] = (this.chkSwipe.Checked ? 1 : 0);
				array[33] = 1;
				array[34] = 1;
				array[35] = (this.chkIncludeSN.Checked ? 1 : 0);
				if (this.groupBox4.Visible)
				{
					if (!string.IsNullOrEmpty(this.txtUDPServer.Text))
					{
						Array.Copy(IPAddress.Parse(this.txtUDPServer.Text).GetAddressBytes(), 0, array, 8, 4);
					}
					int num2 = int.Parse(this.txtPortShort.Text);
					array[12] = (byte)(num2 & 255);
					array[13] = (byte)((num2 >> 8) & 255);
					array[14] = (byte)this.nudCycle.Value;
				}
				icController.ShortPacketSend(array, ref array2);
				if (array2 != null)
				{
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
					wgpacketSSI_FLASH.type = 33;
					wgpacketSSI_FLASH.code = 48;
					wgpacketSSI_FLASH.iDevSnFrom = 0U;
					wgpacketSSI_FLASH.iDevSnTo = (uint)this.ControllerSN;
					wgpacketSSI_FLASH.iCallReturn = 0;
					wgpacketSSI_FLASH.ucData = new byte[1024];
					wgUdpComm wgUdpComm = new wgUdpComm();
					string text = "";
					int num3 = 60000;
					try
					{
						wgTools.getUdpComm(ref wgUdpComm, -1);
						wgpacketSSI_FLASH.iStartFlashAddr = 5001216U;
						wgpacketSSI_FLASH.iEndFlashAddr = 5907U;
						for (int i = 0; i < 1024; i++)
						{
							wgpacketSSI_FLASH.ucData[i] = byte.MaxValue;
						}
						string text2 = string.Format("POST {0} HTTP/1.1\r\nHost: {1}\r\nContent-Type: application/json; charset=utf-8\r\nContent-Length: 000\r\n\r\n{2}&{3}=%s&{4}=%s", new object[]
						{
							this.txtPostInfo.Text.Trim(),
							this.txtHostDomain.Text.Trim(),
							this.txtUserName.Text.Trim() + "&" + this.txtPassword.Text.Trim(),
							this.txtMarkMobile.Text,
							this.txtMarkContent.Text
						});
						byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(text2);
						for (int j = 0; j < bytes.Length; j++)
						{
							wgpacketSSI_FLASH.ucData[j] = bytes[j];
						}
						wgpacketSSI_FLASH.ucData[bytes.Length] = 0;
						wgpacketSSI_FLASH.ucData[256] = (byte)this.controlSegIDList[this.cbof_ControlSegID.SelectedIndex];
						wgpacketSSI_FLASH.ucData[257] = (byte)((this.chkOnlyOnce60Second.Checked ? 1 : 0) + (this.chkCheckPhoneValid.Checked ? 2 : 0));
						int num4 = 0;
						num4 += (this.chkSpecialEvent0.Checked ? 1 : 0);
						num4 += (this.chkSpecialEvent1.Checked ? 2 : 0);
						num4 += (this.chkSpecialEvent2.Checked ? 4 : 0);
						num4 += (this.chkSpecialEvent3.Checked ? 8 : 0);
						num4 += (this.chkSpecialEvent4.Checked ? 16 : 0);
						num4 += (this.chkSpecialEvent5.Checked ? 32 : 0);
						num4 += (this.chkSpecialEvent6.Checked ? 64 : 0);
						num4 += (this.chkSpecialEvent7.Checked ? 128 : 0);
						wgpacketSSI_FLASH.ucData[258] = (byte)num4;
						wgpacketSSI_FLASH.ucData[259] = (byte)(num4 >> 8);
						wgpacketSSI_FLASH.ucData[260] = Encoding.GetEncoding("utf-8").GetBytes(this.cboSeparator.Text)[0];
						int num5 = 0;
						num5 += (this.chkContent0.Checked ? 1 : 0);
						num5 += (this.chkContent1.Checked ? 2 : 0);
						num5 += (this.chkContent2.Checked ? 4 : 0);
						num5 += (this.chkContent3.Checked ? 8 : 0);
						num5 += (this.chkContent4.Checked ? 16 : 0);
						num5 += (this.chkContent5.Checked ? 32 : 0);
						num5 += (this.chkContent6.Checked ? 64 : 0);
						wgpacketSSI_FLASH.ucData[261] = (byte)num5;
						string text3 = this.txtContentSignInfo.Text;
						if (!string.IsNullOrEmpty(text3) && text3.IndexOf(this.cboSeparator.Text) != 0)
						{
							text3 = this.cboSeparator.Text + text3;
						}
						byte[] bytes2 = Encoding.GetEncoding("utf-8").GetBytes(text3);
						for (int k = 0; k < bytes2.Length; k++)
						{
							wgpacketSSI_FLASH.ucData[288 + k] = bytes2[k];
						}
						wgpacketSSI_FLASH.ucData[288 + bytes2.Length] = 0;
						byte[] bytes3 = Encoding.GetEncoding("utf-8").GetBytes(this.txtSpecialMobile.Text);
						for (int l = 0; l < bytes3.Length; l++)
						{
							wgpacketSSI_FLASH.ucData[320 + l] = bytes3[l];
						}
						wgpacketSSI_FLASH.ucData[320 + bytes3.Length] = 0;
						wgAppConfig.wgLog("HTTP Server Configure: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "\r\n");
						if (wgUdpComm.udp_get(wgpacketSSI_FLASH.ToBytes(wgUdpComm.udpPort), 300, wgpacketSSI_FLASH.xid, text, num3, ref array2) < 0)
						{
							XMessageBox.Show(CommonStr.strFailed);
						}
						else
						{
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
					}
					catch (Exception ex)
					{
						XMessageBox.Show(ex.ToString());
					}
					wgUdpComm.Dispose();
				}
				else
				{
					XMessageBox.Show(CommonStr.strFailed);
				}
			}
			catch (Exception)
			{
			}
			icController.Dispose();
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0020F950 File Offset: 0x0020E950
		private void button132_Click(object sender, EventArgs e)
		{
			string text = string.Format("http://{0}", this.txtHostDomain.Text.Trim()) + this.txtPostInfo.Text.Trim();
			string text2 = string.Format("{0}&{1}&{2}={3}{4}&{5}={6}", new object[]
			{
				this.txtUserName.Text.Trim(),
				this.txtPassword.Text.Trim(),
				this.txtMarkContent.Text,
				this.txtContentTest.Text.Trim() + " " + DateTime.Now.ToString(wgTools.YMDHMSFormat),
				this.txtContentSignInfo.Text,
				this.txtMarkMobile.Text,
				this.txtMobileTest.Text.Trim()
			});
			XMessageBox.Show(this.PushToWeb(text, text2, Encoding.UTF8));
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0020FA88 File Offset: 0x0020EA88
		private void deactivetoolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnDeactive.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				base.DialogResult = DialogResult.Cancel;
				byte[] array = new byte[]
				{
					25, 144, 0, 0, 177, 152, 167, 25, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0
				};
				Array.Copy(BitConverter.GetBytes(this.ControllerSN), 0, array, 4, 4);
				icController icController = new icController();
				try
				{
					byte[] array2 = null;
					icController.ControllerSN = this.ControllerSN;
					int num = icController.ControllerSN / 10000000;
					switch (num % 10)
					{
					case 1:
						array[0] = 17;
						break;
					case 2:
						array[0] = 25;
						break;
					case 3:
					case 7:
						array[0] = 19;
						break;
					case 5:
						array[0] = 21;
						break;
					}
					if (icController.ControllerSN >= 171100001 && icController.ControllerSN < 171100099)
					{
						array[0] = 19;
					}
					else if (icController.ControllerSN >= 171200001 && icController.ControllerSN < 172000000)
					{
						array[0] = 17;
					}
					else if (icController.ControllerSN >= 172000001 && icController.ControllerSN < 173000000)
					{
						array[0] = 25;
					}
					else if (icController.ControllerSN >= 175000001 && icController.ControllerSN < 176000000)
					{
						array[0] = 21;
					}
					byte[] array3 = new byte[64];
					array.CopyTo(array3, 0);
					array[1] = 146;
					icController.ShortPacketSend(array, ref array2);
					if (array2 == null)
					{
						XMessageBox.Show(CommonStr.strFailed);
						return;
					}
					array3.CopyTo(array, 0);
					Array.Copy(array2, 8, array, 8, 16);
					Array.Copy(array2, 44, array, 44, 6);
					array[1] = 144;
					icController.ControllerSN = this.ControllerSN;
					icController.ShortPacketSend(array, ref array2);
					if (array2 != null)
					{
						base.DialogResult = DialogResult.OK;
						base.Close();
					}
				}
				catch (Exception)
				{
				}
				icController.Dispose();
			}
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0020FC8C File Offset: 0x0020EC8C
		private void dfrmSMSConfig_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.functionQ5678Set();
				dfrmHTTPServerConfig.functionQ5678 = true;
			}
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0020FCB4 File Offset: 0x0020ECB4
		private void dfrmSMSConfig_Load(object sender, EventArgs e)
		{
			this.cboSeparator.Text = "-";
			this.loadControlSegData();
			this.loadKeyval();
			if (dfrmHTTPServerConfig.functionQ5678)
			{
				this.functionQ5678Set();
			}
			string hostName = Dns.GetHostName();
			bool flag = false;
			string text = "";
			foreach (IPAddress ipaddress in Dns.GetHostEntry(hostName).AddressList)
			{
				if (ipaddress.AddressFamily == AddressFamily.InterNetwork && !ipaddress.IsIPv6LinkLocal && !(ipaddress.ToString() == "127.0.0.1"))
				{
					if (flag)
					{
						break;
					}
					flag = true;
					text = ipaddress.ToString();
				}
			}
			if (flag)
			{
				if (string.IsNullOrEmpty(this.txtHostDomain.Text))
				{
					this.txtHostDomain.Text = text;
				}
				if (string.IsNullOrEmpty(this.txtHostIP.Text))
				{
					this.txtHostIP.Text = text;
				}
			}
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0020FD8C File Offset: 0x0020ED8C
		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dtParamWrite != null)
			{
				this.dtParamWrite.Clear();
			}
			this.setKeyval(this.txtUserName);
			this.setKeyval(this.txtPassword);
			this.setKeyval(this.txtHostDomain);
			this.setKeyval(this.txtHostIP);
			this.setKeyval(this.txtPostInfo);
			this.setKeyval(this.txtPort);
			this.setKeyval(this.txtMarkContent);
			this.setKeyval(this.txtMarkMobile);
			this.setKeyval(this.txtSpecialMobile);
			this.setKeyval(this.txtContentSignInfo);
			this.setKeyval(this.chkContent0);
			this.setKeyval(this.chkContent1);
			this.setKeyval(this.chkContent2);
			this.setKeyval(this.chkContent3);
			this.setKeyval(this.chkContent4);
			this.setKeyval(this.chkContent5);
			this.setKeyval(this.chkContent6);
			this.setKeyval(this.chkSpecialEvent0);
			this.setKeyval(this.chkSpecialEvent1);
			this.setKeyval(this.chkSpecialEvent2);
			this.setKeyval(this.chkSpecialEvent3);
			this.setKeyval(this.chkSpecialEvent4);
			this.setKeyval(this.chkSpecialEvent5);
			this.setKeyval(this.chkSpecialEvent6);
			this.setKeyval(this.chkSpecialEvent7);
			this.setKeyval(this.chkUploadMobile);
			this.setKeyval(this.chkOnlyOnce60Second);
			this.setKeyval(this.chkCheckPhoneValid);
			this.setKeyval(this.cbof_ControlSegID);
			this.setKeyval(this.cboSeparator);
			this.ds = new DataSet();
			this.ds.Tables.Add(this.dtParamWrite);
			StringBuilder stringBuilder = new StringBuilder();
			this.ds.WriteXml(new StringWriter(stringBuilder));
			using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\smsConfig.txt", false))
			{
				streamWriter.WriteLine(stringBuilder.ToString());
			}
			this.dtParamWrite.Clear();
			this.ds.Tables.Remove(this.dtParamWrite);
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0020FFAC File Offset: 0x0020EFAC
		private void functionQ5678Set()
		{
			this.groupBox4.Visible = true;
			base.Size = new Size(600, 340);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0020FFD0 File Offset: 0x0020EFD0
		public void getKeyval(object obj)
		{
			if (this.dtParamRead != null)
			{
				foreach (object obj2 in this.dtParamRead.Rows)
				{
					DataRow dataRow = (DataRow)obj2;
					if (dataRow[0].ToString() == (obj as Control).Name.ToString())
					{
						(obj as Control).Text = dataRow[1].ToString();
						break;
					}
				}
			}
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0021006C File Offset: 0x0020F06C
		public void getKeyval(CheckBox obj)
		{
			if (this.dtParamRead != null)
			{
				foreach (object obj2 in this.dtParamRead.Rows)
				{
					DataRow dataRow = (DataRow)obj2;
					if (dataRow[0].ToString() == obj.Name.ToString())
					{
						obj.Checked = bool.Parse(dataRow[1].ToString());
						break;
					}
				}
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00210104 File Offset: 0x0020F104
		public void getKeyval(ComboBox obj)
		{
			if (this.dtParamRead != null)
			{
				foreach (object obj2 in this.dtParamRead.Rows)
				{
					DataRow dataRow = (DataRow)obj2;
					if (dataRow[0].ToString() == obj.Name.ToString())
					{
						try
						{
							obj.SelectedIndex = int.Parse(dataRow[1].ToString());
							break;
						}
						catch (Exception)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x002101AC File Offset: 0x0020F1AC
		private void importToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.openFileDialog1.Filter = " (*.txt)|*.txt| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				try
				{
					this.openFileDialog1.InitialDirectory = ".\\DOC";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.openFileDialog1.Title = sender.ToString();
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				string fileName = this.openFileDialog1.FileName;
				string text = fileName;
				FileStream fileStream = new FileStream(text, FileMode.Open);
				string text2 = new StreamReader(fileStream).ReadToEnd();
				wgAppConfig.setSystemParamValue(196, "HTTPServer Configuration 2017-12-10 13:57:49", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text2.ToString());
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
				Cursor.Current = Cursors.Default;
			}
			this.loadKeyval();
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x002102DC File Offset: 0x0020F2DC
		private void loadControlSegData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadControlSegData_Acc();
				return;
			}
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			try
			{
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				string text = " SELECT ";
				using (SqlCommand sqlCommand = new SqlCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,    CASE WHEN [f_ControlSegName] IS NULL THEN CONVERT(nvarchar(50),       ([t_b_ControlSeg].[f_ControlSegID])) ELSE (CONVERT(nvarchar(50),      ([t_b_ControlSeg].[f_ControlSegID])) + ' [' + [f_ControlSegName] + ']')     END AS f_ControlSegID    FROM [t_b_ControlSeg] WHERE  [t_b_ControlSeg].[f_ControlSegID]>1 ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", this.cn))
				{
					this.cn.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					int num = 1;
					while (sqlDataReader.Read())
					{
						this.cbof_ControlSegID.Items.Add(sqlDataReader["f_ControlSegID"]);
						this.controlSegIDList[num] = (int)sqlDataReader["f_ControlSegIDBak"];
						num++;
					}
					sqlDataReader.Close();
					if (this.cbof_ControlSegID.Items.Count > 0)
					{
						this.cbof_ControlSegID.SelectedIndex = 0;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				if (this.cn != null)
				{
					this.cn.Dispose();
				}
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00210430 File Offset: 0x0020F430
		private void loadControlSegData_Acc()
		{
			this.cbof_ControlSegID.Items.Clear();
			this.cbof_ControlSegID.Items.Add(CommonStr.strFreeTime);
			this.controlSegIDList[0] = 1;
			OleDbConnection oleDbConnection = null;
			try
			{
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				string text = " SELECT ";
				using (OleDbCommand oleDbCommand = new OleDbCommand(text + " [t_b_ControlSeg].[f_ControlSegID] as f_ControlSegIDBak,   IIF(ISNULL([f_ControlSegName]), CSTR([t_b_ControlSeg].[f_ControlSegID]), CSTR([t_b_ControlSeg].[f_ControlSegID]) & ' [' & [f_ControlSegName] & ']') AS f_ControlSegID   FROM [t_b_ControlSeg]  WHERE  [t_b_ControlSeg].[f_ControlSegID]>1  ORDER BY [t_b_ControlSeg].[f_ControlSegID] ASC  ", oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
					int num = 1;
					while (oleDbDataReader.Read())
					{
						this.cbof_ControlSegID.Items.Add(oleDbDataReader["f_ControlSegID"]);
						this.controlSegIDList[num] = (int)oleDbDataReader["f_ControlSegIDBak"];
						num++;
					}
					oleDbDataReader.Close();
					if (this.cbof_ControlSegID.Items.Count > 0)
					{
						this.cbof_ControlSegID.SelectedIndex = 0;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				if (oleDbConnection != null)
				{
					oleDbConnection.Dispose();
				}
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00210564 File Offset: 0x0020F564
		public void loadKeyval()
		{
			try
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(196);
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					this.dtParamRead = null;
					this.ds.Tables.Clear();
					this.ds.ReadXml(new StringReader(systemParamNotes));
					this.dtParamRead = this.ds.Tables[0];
					this.getKeyval(this.txtUserName);
					this.getKeyval(this.txtPassword);
					this.getKeyval(this.txtHostDomain);
					this.getKeyval(this.txtHostIP);
					this.getKeyval(this.txtPostInfo);
					this.getKeyval(this.txtPort);
					this.getKeyval(this.txtContentTest);
					this.getKeyval(this.txtMobileTest);
					this.getKeyval(this.txtSpecialMobile);
					this.getKeyval(this.txtContentSignInfo);
					this.getKeyval(this.txtMarkContent);
					this.getKeyval(this.txtMarkMobile);
					this.getKeyval(this.chkContent0);
					this.getKeyval(this.chkContent1);
					this.getKeyval(this.chkContent2);
					this.getKeyval(this.chkContent3);
					this.getKeyval(this.chkContent4);
					this.getKeyval(this.chkContent5);
					this.getKeyval(this.chkContent6);
					this.getKeyval(this.chkSpecialEvent0);
					this.getKeyval(this.chkSpecialEvent1);
					this.getKeyval(this.chkSpecialEvent2);
					this.getKeyval(this.chkSpecialEvent3);
					this.getKeyval(this.chkSpecialEvent4);
					this.getKeyval(this.chkSpecialEvent5);
					this.getKeyval(this.chkSpecialEvent6);
					this.getKeyval(this.chkSpecialEvent7);
					this.getKeyval(this.chkUploadMobile);
					this.getKeyval(this.chkOnlyOnce60Second);
					this.getKeyval(this.chkCheckPhoneValid);
					this.getKeyval(this.cbof_ControlSegID);
					this.getKeyval(this.cboSeparator);
					this.getKeyval(this.chkIncludeSN);
					this.getKeyval(this.txtUDPServer);
					this.getKeyval(this.txtPortShort);
					this.getKeyval(this.nudCycle);
					this.getKeyval(this.chkSwipe);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x002107A4 File Offset: 0x0020F7A4
		public string PushToWeb(string weburl, string data, Encoding encode)
		{
			byte[] bytes = encode.GetBytes(data);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
			httpWebRequest.SendChunked = false;
			httpWebRequest.KeepAlive = false;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)bytes.Length;
			Stream requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			string text = new StreamReader(httpWebResponse.GetResponseStream(), encode).ReadToEnd();
			int num = 0;
			if (!int.TryParse(text, out num))
			{
				return text;
			}
			int num2 = num;
			if (num2 <= -41)
			{
				int num3 = num2;
				if (num3 == -51)
				{
					return "短信签名格式不正确 接口签名格式为：【签名内容】";
				}
				switch (num3)
				{
				case -42:
					return "短信内容为空";
				case -41:
					return "手机号码为空";
				default:
					return text;
				}
			}
			else
			{
				if (num2 != -21)
				{
					switch (num2)
					{
					case -14:
						return "短信内容出现非法字符";
					case -11:
						return "该用户被禁用";
					case -10:
					case -9:
					case -8:
					case -7:
					case -5:
						return text;
					case -6:
						return "IP限制";
					case -4:
						return "手机号格式不正确";
					case -3:
						return "短信数量不足";
					case -2:
						return "接口密钥不正确 [查看密钥] 不是账户登陆密码";
					case -1:
						return "没有该用户账户";
					}
					return text;
				}
				return "MD5接口密钥加密不正确";
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00210900 File Offset: 0x0020F900
		public void saveKeyval()
		{
			this.ds = new DataSet();
			this.ds.Tables.Add(this.dtParamWrite);
			StringBuilder stringBuilder = new StringBuilder();
			this.ds.WriteXml(new StringWriter(stringBuilder));
			wgAppConfig.setSystemParamValue(196, "HTTPServer Configuration 2017-12-10 13:57:49", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stringBuilder.ToString());
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00210970 File Offset: 0x0020F970
		public void setKeyval(object obj)
		{
			if (this.dtParamWrite == null)
			{
				this.dtParamWrite = new DataTable("DataRow");
				this.dtParamWrite.Columns.Add("key", Type.GetType("System.String"));
				this.dtParamWrite.Columns.Add("value", Type.GetType("System.String"));
				this.dtParamWrite.AcceptChanges();
			}
			DataRow dataRow = this.dtParamWrite.NewRow();
			dataRow[0] = (obj as Control).Name;
			dataRow[1] = (obj as Control).Text;
			this.dtParamWrite.Rows.Add(dataRow);
			this.dtParamWrite.AcceptChanges();
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00210A2C File Offset: 0x0020FA2C
		public void setKeyval(CheckBox obj)
		{
			if (this.dtParamWrite == null)
			{
				this.dtParamWrite = new DataTable("DataRow");
				this.dtParamWrite.Columns.Add("key", Type.GetType("System.String"));
				this.dtParamWrite.Columns.Add("value", Type.GetType("System.String"));
				this.dtParamWrite.AcceptChanges();
			}
			DataRow dataRow = this.dtParamWrite.NewRow();
			dataRow[0] = obj.Name;
			dataRow[1] = obj.Checked;
			this.dtParamWrite.Rows.Add(dataRow);
			this.dtParamWrite.AcceptChanges();
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00210AE4 File Offset: 0x0020FAE4
		public void setKeyval(ComboBox obj)
		{
			if (this.dtParamWrite == null)
			{
				this.dtParamWrite = new DataTable("DataRow");
				this.dtParamWrite.Columns.Add("key", Type.GetType("System.String"));
				this.dtParamWrite.Columns.Add("value", Type.GetType("System.String"));
				this.dtParamWrite.AcceptChanges();
			}
			DataRow dataRow = this.dtParamWrite.NewRow();
			dataRow[0] = obj.Name;
			dataRow[1] = obj.SelectedIndex;
			this.dtParamWrite.Rows.Add(dataRow);
			this.dtParamWrite.AcceptChanges();
		}

		// Token: 0x0400338D RID: 13197
		private SqlConnection cn;

		// Token: 0x0400338E RID: 13198
		private DataTable dtParamRead;

		// Token: 0x0400338F RID: 13199
		private DataTable dtParamWrite;

		// Token: 0x04003390 RID: 13200
		public int ControllerSN;

		// Token: 0x04003391 RID: 13201
		private int[] controlSegIDList = new int[256];

		// Token: 0x04003392 RID: 13202
		private DataSet ds = new DataSet();

		// Token: 0x04003393 RID: 13203
		private static bool functionQ5678;
	}
}
