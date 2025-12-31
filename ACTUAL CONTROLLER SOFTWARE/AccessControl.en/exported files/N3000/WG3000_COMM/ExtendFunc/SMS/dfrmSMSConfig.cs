using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.SMS
{
	// Token: 0x02000325 RID: 805
	public partial class dfrmSMSConfig : frmN3000
	{
		// Token: 0x06001938 RID: 6456 RVA: 0x00212720 File Offset: 0x00211720
		public dfrmSMSConfig()
		{
			this.InitializeComponent();
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x002127A2 File Offset: 0x002117A2
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x002127B4 File Offset: 0x002117B4
		private void btnDefaultParameter_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnDefaultParameter.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				wgAppConfig.setSystemParamValue(179, "SMS Configuration 2015-05-30 22:57:28", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "  ");
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x00212868 File Offset: 0x00211868
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
			this.saveKeyval();
			base.DialogResult = DialogResult.Cancel;
			wgAppConfig.UpdateKeyVal("AllowUploadUserMobile", this.chkUploadMobile.Checked ? "1" : "0");
			wgAppConfig.UpdateKeyVal("AllowUploadUserName", this.chkUploadMobile.Checked ? "1" : "0");
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
				IPAddress.Parse(this.txtHostIP.Text).GetAddressBytes().CopyTo(array, 24);
				array[28] = (byte)(int.Parse(this.txtPort.Text) & 255);
				array[29] = (byte)((int.Parse(this.txtPort.Text) >> 8) & 255);
				array[33] = 1;
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
					int num2 = 60000;
					try
					{
						Thread.Sleep(300);
						wgpacketSSI_FLASH.iStartFlashAddr = 5001216U;
						wgpacketSSI_FLASH.iEndFlashAddr = 5907U;
						for (int i = 0; i < 1024; i++)
						{
							wgpacketSSI_FLASH.ucData[i] = byte.MaxValue;
						}
						string text2 = string.Format("POST {0} HTTP/1.1\r\nHost: {1}\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: 000\r\n\r\n{2}&{3}=%s&{4}=%s", new object[]
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
						int num3 = 0;
						num3 += (this.chkSpecialEvent0.Checked ? 1 : 0);
						num3 += (this.chkSpecialEvent1.Checked ? 2 : 0);
						num3 += (this.chkSpecialEvent2.Checked ? 4 : 0);
						num3 += (this.chkSpecialEvent3.Checked ? 8 : 0);
						num3 += (this.chkSpecialEvent4.Checked ? 16 : 0);
						num3 += (this.chkSpecialEvent5.Checked ? 32 : 0);
						num3 += (this.chkSpecialEvent6.Checked ? 64 : 0);
						num3 += (this.chkSpecialEvent7.Checked ? 128 : 0);
						wgpacketSSI_FLASH.ucData[258] = (byte)num3;
						wgpacketSSI_FLASH.ucData[259] = (byte)(num3 >> 8);
						wgpacketSSI_FLASH.ucData[260] = Encoding.GetEncoding("utf-8").GetBytes(this.cboSeparator.Text)[0];
						int num4 = 0;
						num4 += (this.chkContent0.Checked ? 1 : 0);
						num4 += (this.chkContent1.Checked ? 2 : 0);
						num4 += (this.chkContent2.Checked ? 4 : 0);
						num4 += (this.chkContent3.Checked ? 8 : 0);
						num4 += (this.chkContent4.Checked ? 16 : 0);
						num4 += (this.chkContent5.Checked ? 32 : 0);
						num4 += (this.chkContent6.Checked ? 64 : 0);
						wgpacketSSI_FLASH.ucData[261] = (byte)num4;
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
						wgAppConfig.wgLog("SMS Configure: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "\r\n");
						int num5 = wgUdpComm.udp_get(wgpacketSSI_FLASH.ToBytes(wgUdpComm.udpPort), 300, wgpacketSSI_FLASH.xid, text, num2, ref array2);
						if (num5 > 0)
						{
							int num6 = 12288;
							byte[] array4 = new byte[1152];
							for (int m = 0; m < array4.Length; m++)
							{
								array4[m] = 0;
							}
							if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
							{
								num6 = 8192;
							}
							bool flag = false;
							wgMjControllerConfigure wgMjControllerConfigure = null;
							icController.GetConfigureIP(ref wgMjControllerConfigure);
							if (wgMjControllerConfigure != null)
							{
								if (wgMjControllerConfigure.webLanguage == "中文[zh-CHS]" && num6 != 8192)
								{
									flag = true;
								}
								if (wgMjControllerConfigure.webLanguage == "English" && num6 != 12288)
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
							int num7 = 100;
							array4[num7] = (byte)(num6 & 255);
							array4[1024 + (num7 >> 3)] = array4[1024 + (num7 >> 3)] | (byte)(1 << (num7 & 7));
							num7++;
							array4[num7] = (byte)(num6 >> 8);
							array4[1024 + (num7 >> 3)] = array4[1024 + (num7 >> 3)] | (byte)(1 << (num7 & 7));
							num7++;
							array4[num7] = (byte)(num6 >> 16);
							array4[1024 + (num7 >> 3)] = array4[1024 + (num7 >> 3)] | (byte)(1 << (num7 & 7));
							num7++;
							array4[num7] = (byte)(num6 >> 24);
							array4[1024 + (num7 >> 3)] = array4[1024 + (num7 >> 3)] | (byte)(1 << (num7 & 7));
							icController.ControllerSN = this.ControllerSN;
							if (flag)
							{
								icController.UpdateConfigureCPUSuperIP(array4, "", text);
								icController.RebootControllerIP();
							}
						}
						if (num5 < 0)
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

		// Token: 0x0600193C RID: 6460 RVA: 0x002132E4 File Offset: 0x002122E4
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

		// Token: 0x0600193D RID: 6461 RVA: 0x002133D9 File Offset: 0x002123D9
		private void dfrmSMSConfig_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.txtPassword.PasswordChar = '\0';
				this.exportToolStripMenuItem.Visible = true;
			}
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0021340D File Offset: 0x0021240D
		private void dfrmSMSConfig_Load(object sender, EventArgs e)
		{
			this.cboSeparator.Text = "-";
			this.loadControlSegData();
			this.loadKeyval();
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0021342C File Offset: 0x0021242C
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

		// Token: 0x06001940 RID: 6464 RVA: 0x0021364C File Offset: 0x0021264C
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

		// Token: 0x06001941 RID: 6465 RVA: 0x002136E8 File Offset: 0x002126E8
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

		// Token: 0x06001942 RID: 6466 RVA: 0x00213780 File Offset: 0x00212780
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

		// Token: 0x06001943 RID: 6467 RVA: 0x00213828 File Offset: 0x00212828
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
				wgAppConfig.setSystemParamValue(179, "SMS Configuration 2015-05-30 22:57:28", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text2.ToString());
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

		// Token: 0x06001944 RID: 6468 RVA: 0x00213958 File Offset: 0x00212958
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

		// Token: 0x06001945 RID: 6469 RVA: 0x00213AAC File Offset: 0x00212AAC
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

		// Token: 0x06001946 RID: 6470 RVA: 0x00213BE0 File Offset: 0x00212BE0
		public void loadKeyval()
		{
			try
			{
				string systemParamNotes = wgAppConfig.getSystemParamNotes(179);
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
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00213DE4 File Offset: 0x00212DE4
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

		// Token: 0x06001948 RID: 6472 RVA: 0x00213F40 File Offset: 0x00212F40
		public void saveKeyval()
		{
			this.ds = new DataSet();
			this.ds.Tables.Add(this.dtParamWrite);
			StringBuilder stringBuilder = new StringBuilder();
			this.ds.WriteXml(new StringWriter(stringBuilder));
			wgAppConfig.setSystemParamValue(179, "SMS Configuration 2015-05-30 22:57:28", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stringBuilder.ToString());
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00213FB0 File Offset: 0x00212FB0
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

		// Token: 0x0600194A RID: 6474 RVA: 0x0021406C File Offset: 0x0021306C
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

		// Token: 0x0600194B RID: 6475 RVA: 0x00214124 File Offset: 0x00213124
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

		// Token: 0x040033E2 RID: 13282
		private SqlConnection cn;

		// Token: 0x040033E3 RID: 13283
		private DataTable dtParamRead;

		// Token: 0x040033E4 RID: 13284
		private DataTable dtParamWrite;

		// Token: 0x040033E5 RID: 13285
		public int ControllerSN;

		// Token: 0x040033E6 RID: 13286
		private int[] controlSegIDList = new int[256];

		// Token: 0x040033E7 RID: 13287
		private DataSet ds = new DataSet();
	}
}
