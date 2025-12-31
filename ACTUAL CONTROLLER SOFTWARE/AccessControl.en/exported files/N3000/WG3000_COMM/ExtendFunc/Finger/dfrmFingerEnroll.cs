using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WgiCCard;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002DF RID: 735
	public partial class dfrmFingerEnroll : frmN3000
	{
		// Token: 0x060014BE RID: 5310 RVA: 0x00198390 File Offset: 0x00197390
		public dfrmFingerEnroll()
		{
			this.InitializeComponent();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0019842D File Offset: 0x0019742D
		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (this.bSuccessOne)
			{
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00198444 File Offset: 0x00197444
		private void btnCancel2_Click(object sender, EventArgs e)
		{
			this.bStopEnroll = true;
			if (this.bSuccessOne)
			{
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00198464 File Offset: 0x00197464
		private void btnClearAll_Click(object sender, EventArgs e)
		{
			if (this.optUSBReader.Checked)
			{
				dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
				string text = string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice));
				if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					wgAppConfig.wgLog(string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice)));
					this.bStopEnroll = false;
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_ClearAll;
					try
					{
						dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
						if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
						}
						byte[] array = new byte[1152];
						this.wgSerial.ClosePort();
						string text2 = dfrmFingerEnroll.selectedFingerInputDevice;
						this.wgSerial.Baudrate = 57600L;
						int num;
						if (int.TryParse(text2.Replace("COM", ""), out num))
						{
							this.wgSerial.CommPort = byte.Parse(text2.Replace("COM", ""));
						}
						else
						{
							this.wgSerial.CommPort = 1;
						}
						this.wgSerial.ClosePort();
						long num2 = this.wgSerial.OpenPort();
						if (num2 == 0L)
						{
							while (!this.bStopEnroll)
							{
								this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_ClearAll;
								num2 = (long)this.dealFingerEnroll(ref array);
								if (num2 > 0L)
								{
									break;
								}
							}
							if (num2 > 0L)
							{
								XMessageBox.Show(this.btnClearAll.Text + " " + CommonStr.strSuccessfully);
							}
						}
						else
						{
							this.dfrmWait1.Hide();
							this.dfrmWait1.Refresh();
							XMessageBox.Show(dfrmFingerEnroll.selectedFingerInputDevice + "\t" + CommonStr.strOpenFailed);
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
					finally
					{
						this.wgSerial.ClosePort();
					}
				}
				return;
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00198694 File Offset: 0x00197694
		private void btnDownloadAll_Click(object sender, EventArgs e)
		{
			if (this.optUSBReader.Checked)
			{
				dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
				string text = string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice));
				if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					wgAppConfig.wgLog(string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice)));
					this.bStopEnroll = false;
					try
					{
						dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
						if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
						}
						int num = 0;
						byte[] array = new byte[1152];
						this.wgSerial.ClosePort();
						string text2 = dfrmFingerEnroll.selectedFingerInputDevice;
						this.wgSerial.Baudrate = 57600L;
						int num2;
						if (int.TryParse(text2.Replace("COM", ""), out num2))
						{
							this.wgSerial.CommPort = byte.Parse(text2.Replace("COM", ""));
						}
						else
						{
							this.wgSerial.CommPort = 1;
						}
						this.wgSerial.ClosePort();
						if (this.wgSerial.OpenPort() != 0L)
						{
							this.dfrmWait1.Hide();
							this.dfrmWait1.Refresh();
							XMessageBox.Show(dfrmFingerEnroll.selectedFingerInputDevice + "\t" + CommonStr.strOpenFailed);
						}
						else
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_DownloadAll;
							this.syncFingerIndex = 0;
							int num3 = 0;
							while (!this.bStopEnroll)
							{
								try
								{
									if (this.syncFingerIndex >= 1000)
									{
										XMessageBox.Show(string.Concat(new object[]
										{
											this.btnDownloadAll.Text,
											CommonStr.strSuccessfully,
											"\r\n\r\n",
											num3
										}));
										break;
									}
									int num4 = 99999999;
									this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_DownloadAll;
									long num5 = (long)this.dealFingerEnroll(ref array);
									if (num5 > 0L)
									{
										if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMGetOK)
										{
											int num6 = this.syncFingerIndex;
											if (num6 >= 0 && num6 < 1024)
											{
												int valBySql = wgAppConfig.getValBySql(string.Format("SELECT t_b_Consumer.f_ConsumerID From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num6 + 1));
												if (valBySql > 0 && valBySql != num4)
												{
													num4 = valBySql;
												}
												if (valBySql > 0 && valBySql == num4)
												{
												}
											}
											else
											{
												num6 = -1;
											}
											if (BitConverter.ToString(array).Replace("-", "").Substring(0, 1024)
												.Replace("00", "")
												.Length == 0)
											{
												this.syncFingerIndex++;
												continue;
											}
											num3++;
											icConsumer icConsumer = new icConsumer();
											BitConverter.ToString(array).Replace("-", "").Substring(0, 1024);
											icConsumer.fingerprintNew(num4, num6, BitConverter.ToString(array).Replace("-", "").Substring(0, 1024));
										}
										this.syncFingerIndex++;
									}
									else
									{
										num++;
										if (num > 3)
										{
											this.btnErrConnect.Visible = true;
										}
										Application.DoEvents();
										if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
										{
											break;
										}
									}
								}
								catch (Exception ex)
								{
									wgAppConfig.wgDebugWrite(ex.ToString());
								}
							}
						}
					}
					catch (Exception ex2)
					{
						wgAppConfig.wgDebugWrite(ex2.ToString());
					}
					finally
					{
						this.wgSerial.ClosePort();
					}
				}
				return;
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00198A7C File Offset: 0x00197A7C
		private void btnErrConnect_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00198A80 File Offset: 0x00197A80
		private void btnInstallUSBDriver_Click(object sender, EventArgs e)
		{
			try
			{
				string text = wgAppConfig.Path4PhotoDefault() + "\\Other\\USB_FTDI\\setup.exe";
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo.Exists)
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = text,
						UseShellExecute = true
					});
					wgAppConfig.wgLog("Install USB Driver");
				}
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00198B18 File Offset: 0x00197B18
		private void btnNext_Click(object sender, EventArgs e)
		{
			if ((!this.optController.Checked || !string.IsNullOrEmpty(this.cboDoors.Text)) && (!this.optUSBReader.Checked || !string.IsNullOrEmpty(this.cboUART.Text)))
			{
				this.groupBox5.Visible = true;
				this.btnCancel.Visible = false;
				this.btnNext.Visible = false;
				this.groupBox1.Visible = false;
				this.button1.Visible = true;
				this.btnOK.Visible = true;
				this.btnExit.Visible = true;
				if (this.optUSBReader.Checked)
				{
					dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
				}
				if (this.optController.Checked)
				{
					dfrmFingerEnroll.selectedFingerInputDevice = this.cboDoors.Text;
				}
				dfrmFingerEnroll.usbOption = this.optUSBReader.Enabled;
				this.bStopEnroll = false;
				this.timer1.Enabled = true;
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00198C1A File Offset: 0x00197C1A
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_none;
			this.bStopEnroll = false;
			this.timer1.Enabled = true;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00198C38 File Offset: 0x00197C38
		private void btnUploadAllUsers_Click(object sender, EventArgs e)
		{
			if (!this.optUSBReader.Checked)
			{
				return;
			}
			dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
			string text = string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice));
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			wgAppConfig.wgLog(string.Format("{0}  \"{1}\"", (sender as Button).Text, wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice)));
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			this.bStopEnroll = false;
			this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_ClearAll;
			try
			{
				dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
				if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
				{
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
				}
				int num = 0;
				byte[] array = new byte[1152];
				this.wgSerial.ClosePort();
				string text2 = dfrmFingerEnroll.selectedFingerInputDevice;
				this.wgSerial.Baudrate = 57600L;
				int num2;
				if (int.TryParse(text2.Replace("COM", ""), out num2))
				{
					this.wgSerial.CommPort = byte.Parse(text2.Replace("COM", ""));
				}
				else
				{
					this.wgSerial.CommPort = 1;
				}
				this.wgSerial.ClosePort();
				if (this.wgSerial.OpenPort() != 0L)
				{
					this.dfrmWait1.Hide();
					this.dfrmWait1.Refresh();
					XMessageBox.Show(dfrmFingerEnroll.selectedFingerInputDevice + "\t" + CommonStr.strOpenFailed);
					return;
				}
				while (!this.bStopEnroll)
				{
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_ClearAll;
					long num3 = (long)this.dealFingerEnroll(ref array);
					if (num3 > 0L)
					{
						break;
					}
				}
				byte[] array2 = new byte[4096];
				byte[] array3 = new byte[524288];
				int num4 = 0;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = byte.MaxValue;
				}
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = byte.MaxValue;
				}
				string text3 = "";
				string text4 = "";
				int num5 = 0;
				try
				{
					using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
					{
						using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text3, (OleDbConnection)dbConnection) : new SqlCommand(text3, (SqlConnection)dbConnection)))
						{
							dbConnection.Open();
							text3 = "SELECT t_b_Consumer_Fingerprint.*, t_b_Consumer.f_CardNO  From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID  ORDER BY f_FingerNO ASC";
							dbCommand.CommandText = text3;
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								num5 = (int)dbDataReader["f_FingerNO"];
								if (num5 <= 0 || num5 >= 1024)
								{
									break;
								}
								Array.Copy(BitConverter.GetBytes(long.Parse(dbDataReader["f_CardNO"].ToString())), 0, array2, (num5 - 1) * 4, 4);
								text4 = dbDataReader["f_FingerInfo"] as string;
								if (!string.IsNullOrEmpty(text4))
								{
									text4 = text4.Replace("\r\n", "");
									for (int k = 0; k < text4.Length; k += 2)
									{
										try
										{
											array3[(num5 - 1) * 512 + k / 2] = byte.Parse(text4.Substring(k, 2), NumberStyles.AllowHexSpecifier);
										}
										catch (Exception ex)
										{
											wgAppConfig.wgLog(string.Concat(new object[]
											{
												ex.ToString(),
												"\r\nfingerNO=",
												num5,
												",strFinger=",
												text4
											}));
										}
									}
								}
								if (num4 < num5)
								{
									num4 = num5;
								}
							}
							dbDataReader.Close();
						}
					}
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog(string.Concat(new object[]
					{
						ex2.ToString(),
						"\r\nfingerNO=",
						num5,
						",strFinger=",
						text4
					}));
				}
				if (num4 <= 0)
				{
					this.dfrmWait1.Hide();
					this.dfrmWait1.Refresh();
					return;
				}
				this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_UploadAll;
				this.syncFingerIndex = 0;
				int num6 = 0;
				while (!this.bStopEnroll)
				{
					try
					{
						while (this.syncFingerIndex < num4)
						{
							if (array2[this.syncFingerIndex * 4] != 0 || array2[this.syncFingerIndex * 4 + 1] != 0 || array2[this.syncFingerIndex * 4 + 2] != 0 || array2[this.syncFingerIndex * 4 + 3] != 0)
							{
								Array.Copy(array3, this.syncFingerIndex * 512, array, 0, 512);
								this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_UploadAll;
								break;
							}
							this.syncFingerIndex++;
						}
						if (this.syncFingerIndex >= num4)
						{
							this.dfrmWait1.Hide();
							this.dfrmWait1.Refresh();
							wgAppConfig.wgLog(string.Concat(new object[]
							{
								this.btnUploadAllUsers.Text,
								CommonStr.strSuccessfully,
								". cnt =",
								num6
							}));
							XMessageBox.Show(string.Concat(new object[]
							{
								this.btnUploadAllUsers.Text,
								CommonStr.strSuccessfully,
								"\r\n\r\n",
								num6
							}));
							return;
						}
						long num3 = (long)this.dealFingerEnroll(ref array);
						if (num3 > 0L)
						{
							num6++;
							this.syncFingerIndex++;
						}
						else
						{
							num++;
							if (num > 3)
							{
								this.btnErrConnect.Visible = true;
							}
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}[{1}]", CommonStr.strFingerprintSyncFingerprint, num6.ToString() + "/" + num4.ToString()));
						Application.DoEvents();
						if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
						{
							break;
						}
					}
					catch (Exception ex3)
					{
						wgAppConfig.wgDebugWrite(ex3.ToString());
					}
				}
			}
			catch (Exception ex4)
			{
				wgAppConfig.wgDebugWrite(ex4.ToString());
			}
			finally
			{
				this.wgSerial.ClosePort();
			}
			wgAppConfig.wgLog(this.btnUploadAllUsers.Text + CommonStr.strFailed);
			this.dfrmWait1.Hide();
			this.dfrmWait1.Refresh();
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00199348 File Offset: 0x00198348
		private void button1_Click(object sender, EventArgs e)
		{
			this.bStopEnroll = true;
			this.groupBox5.Visible = false;
			this.btnCancel.Visible = true;
			this.btnNext.Visible = true;
			this.groupBox1.Visible = true;
			this.button1.Visible = false;
			this.btnOK.Visible = false;
			this.btnExit.Visible = false;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x001993B0 File Offset: 0x001983B0
		private void cboDoors_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cboDoors);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x001993C0 File Offset: 0x001983C0
		private int CmdComm(ref byte[] p, int len)
		{
			int num = this.CmdComm_Realize(ref p, len);
			if (num < 0)
			{
				wgTools.WriteLine("CmdComm  ???");
			}
			return num;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x001993E8 File Offset: 0x001983E8
		private int CmdComm_Realize(ref byte[] p, int len)
		{
			byte[] array = new byte[1];
			if (len > 128)
			{
				return this.CmdCommPut128(ref p, len);
			}
			if (len == 0 || len > 11)
			{
				return -1;
			}
			long ticks = DateTime.Now.Ticks;
			byte[] array2 = new byte[this.BAGHEAD.Length + len];
			Array.Copy(this.BAGHEAD, 0, array2, 0, this.BAGHEAD.Length);
			Array.Copy(p, 0, array2, this.BAGHEAD.Length, len);
			this.wgSerial.wgRs232.Write(array2);
			if (Math.Abs(Convert.ToInt32(DateTime.Now.Ticks - ticks) / 10000) > 1000)
			{
				wgTools.WriteLine("wgSerial.wgRs232.Write(p);  ???");
				return -1;
			}
			int num = 1000;
			long num2 = this.msToTicks((long)num);
			long num3 = Convert.ToInt64(DateTime.Now.Ticks + num2);
			for (long num4 = 0L; num4 < 16L; num4 += 1L)
			{
				this.RBF[(int)((IntPtr)num4)] = 0;
			}
			for (long num4 = 0L; num4 < 9L; num4 += 1L)
			{
				int num5 = 0;
				while (num5 == 0)
				{
					if (DateTime.Now.Ticks > num3)
					{
						wgTools.WriteLine("接收数据  ???");
						return -1;
					}
					if (this.wgSerial.wgRs232.Read(1) > 0)
					{
						array[0] = this.wgSerial.wgRs232.InputStream[0];
						num5 = 1;
					}
				}
				this.RBF[(int)((IntPtr)num4)] = array[0];
			}
			if (this.RBF[0] != 239)
			{
				return -1;
			}
			if (this.RBF[1] != 1)
			{
				return -1;
			}
			if (this.RBF[2] != 255)
			{
				return -1;
			}
			if (this.RBF[3] != 255)
			{
				return -1;
			}
			if (this.RBF[4] != 255)
			{
				return -1;
			}
			if (this.RBF[5] != 255)
			{
				return -1;
			}
			if (this.RBF[6] != 7)
			{
				return -1;
			}
			int num6 = (int)this.RBF[7] * 256 + (int)this.RBF[8];
			for (long num4 = 0L; num4 < (long)num6; num4 += 1L)
			{
				int num5 = 0;
				while (num5 == 0)
				{
					if (DateTime.Now.Ticks > num3)
					{
						return -1;
					}
					if (this.wgSerial.wgRs232.Read(1) > 0)
					{
						array[0] = this.wgSerial.wgRs232.InputStream[0];
						num5 = 1;
					}
				}
				this.RBF[(int)((IntPtr)(num4 + 9L))] = array[0];
			}
			int sum = this.GetSum(this.RBF, 6, num6 + 1);
			if (((sum >> 8) & 255) != (int)this.RBF[num6 + 7])
			{
				return -1;
			}
			if ((sum & 255) != (int)this.RBF[num6 + 8])
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x001996B8 File Offset: 0x001986B8
		private int CmdCommGet512(ref byte[] p, int len, ref byte[] data)
		{
			if (p[0] == 0)
			{
				return -1;
			}
			if (len == 0 || len > 11)
			{
				return -1;
			}
			long ticks = DateTime.Now.Ticks;
			byte[] array = new byte[this.BAGHEAD.Length + len];
			Array.Copy(this.BAGHEAD, 0, array, 0, this.BAGHEAD.Length);
			Array.Copy(p, 0, array, this.BAGHEAD.Length, len);
			this.wgSerial.wgRs232.Write(array);
			if (Math.Abs(Convert.ToInt32(DateTime.Now.Ticks - ticks) / 10000) > 1000)
			{
				return -1;
			}
			byte[] array2 = new byte[1];
			byte[] array3 = new byte[148];
			int num = 0;
			this.RBF[9] = 0;
			this.RBF[7] = (byte)(num & 255);
			this.RBF[8] = (byte)((num >> 8) & 255);
			long num2 = this.msToTicks(1000L);
			long num3 = Convert.ToInt64(DateTime.Now.Ticks + num2);
			for (;;)
			{
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i] = 0;
				}
				for (int i = 0; i < 9; i++)
				{
					int num4 = 0;
					while (num4 == 0)
					{
						if (DateTime.Now.Ticks > num3)
						{
							return -1;
						}
						if (this.wgSerial.wgRs232.Read(1) > 0)
						{
							array2[0] = this.wgSerial.wgRs232.InputStream[0];
							num4 = 1;
						}
					}
					array3[i] = array2[0];
				}
				if (array3[0] != 239 || array3[1] != 1 || array3[2] != 255 || array3[3] != 255 || array3[4] != 255 || array3[5] != 255 || (array3[6] != 7 && array3[6] != 2 && array3[6] != 8))
				{
					return -1;
				}
				int num5 = (int)array3[7] * 256 + (int)array3[8];
				for (int i = 0; i < num5; i++)
				{
					int num4 = 0;
					while (num4 == 0)
					{
						if (DateTime.Now.Ticks > num3)
						{
							return -1;
						}
						if (this.wgSerial.wgRs232.Read(1) > 0)
						{
							array2[0] = this.wgSerial.wgRs232.InputStream[0];
							num4 = 1;
						}
					}
					array3[i + 9] = array2[0];
				}
				int sum = this.GetSum(array3, 6, num5 + 1);
				if (((sum >> 8) & 255) != (int)array3[num5 + 7] || (sum & 255) != (int)array3[num5 + 8])
				{
					return -1;
				}
				if (array3[6] != 7 || array3[9] != 0)
				{
					if (array3[6] == 7 && array3[9] != 0)
					{
						goto Block_25;
					}
					Array.Copy(array3, 9, data, num, num5 - 2);
					num = num + num5 - 2;
					this.RBF[7] = (byte)(num & 255);
					this.RBF[8] = (byte)((num >> 8) & 255);
					if (array3[6] == 8)
					{
						goto IL_0304;
					}
				}
			}
			return -1;
			Block_25:
			this.RBF[9] = array3[9];
			IL_0304:
			if (num == 512)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x001999D8 File Offset: 0x001989D8
		private int CmdCommPut128(ref byte[] p, int len)
		{
			byte[] array = new byte[this.BAGHEAD.Length + len];
			Array.Copy(this.BAGHEAD, 0, array, 0, this.BAGHEAD.Length);
			Array.Copy(p, 0, array, this.BAGHEAD.Length, len);
			this.wgSerial.wgRs232.Write(array);
			return 1;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00199A30 File Offset: 0x00198A30
		private int dealFingerEnroll(ref byte[] resultData)
		{
			byte[] array = new byte[11];
			if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
			{
				return -1;
			}
			wgTools.WriteLine("fingerEnrollStatus= " + this.fingerEnrollStatus);
			if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_getImage1 || this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_getImage1b)
			{
				array[0] = 1;
				array[1] = 0;
				array[2] = 3;
				array[3] = 1;
				int num = this.GetSum(array, 0, 4);
				array[4] = (byte)((num >> 8) & 255);
				array[5] = (byte)(num & 255);
				int num2 = this.CmdComm(ref array, 6);
				wgTools.WriteLine("tmp = CmdComm(ref buf,5+1); ");
				if (num2 == 1 && this.RBF[9] == 0)
				{
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzM1;
					return num2;
				}
				this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
				return num2;
			}
			else
			{
				if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzM1)
				{
					array[0] = 1;
					array[1] = 0;
					array[2] = 4;
					array[3] = 2;
					array[4] = 1;
					int num = this.GetSum(array, 0, 5);
					array[5] = (byte)((num >> 8) & 255);
					array[6] = (byte)(num & 255);
					int num2 = this.CmdComm(ref array, 7);
					if (num2 == 1 && this.RBF[9] == 0)
					{
						this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage2;
						SystemSounds.Beep.Play();
					}
					else
					{
						this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
					}
					Thread.Sleep(500);
					return num2;
				}
				if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_getImage2 || this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_getImage2b)
				{
					array[0] = 1;
					array[1] = 0;
					array[2] = 3;
					array[3] = 1;
					int num = this.GetSum(array, 0, 4);
					array[4] = (byte)((num >> 8) & 255);
					array[5] = (byte)(num & 255);
					int num2 = this.CmdComm(ref array, 6);
					if (num2 == 1 && this.RBF[9] == 0)
					{
						this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzM2;
						return num2;
					}
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage2;
					return num2;
				}
				else if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzM2)
				{
					array[0] = 1;
					array[1] = 0;
					array[2] = 4;
					array[3] = 2;
					array[4] = 2;
					int num = this.GetSum(array, 0, 5);
					array[5] = (byte)((num >> 8) & 255);
					array[6] = (byte)(num & 255);
					int num2 = this.CmdComm(ref array, 7);
					if (num2 == 1 && this.RBF[9] == 0)
					{
						this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzMCreate;
						return num2;
					}
					Thread.Sleep(500);
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage2;
					return num2;
				}
				else if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMCreate)
				{
					array[0] = 1;
					array[1] = 0;
					array[2] = 3;
					array[3] = 5;
					int num = this.GetSum(array, 0, 4);
					array[4] = (byte)((num >> 8) & 255);
					array[5] = (byte)(num & 255);
					int num2 = this.CmdComm(ref array, 6);
					if (num2 == 1 && this.RBF[9] == 0)
					{
						this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzMSearch;
						SystemSounds.Beep.Play();
						SystemSounds.Beep.Play();
						return num2;
					}
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
					SystemSounds.Beep.Play();
					Thread.Sleep(500);
					return num2;
				}
				else
				{
					int num;
					int num2;
					if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMSearch)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 8;
						array[3] = 4;
						array[4] = 1;
						array[5] = 0;
						array[6] = 0;
						array[7] = 3;
						array[8] = byte.MaxValue;
						num = this.GetSum(array, 0, 9);
						array[9] = (byte)((num >> 8) & 255);
						array[10] = (byte)(num & 255);
						num2 = this.CmdComm(ref array, 11);
						this.foundIndexNew = -1;
						if (num2 == 1 && this.RBF[9] == 0)
						{
							this.foundIndexNew = ((int)this.RBF[10] << 8) + (int)this.RBF[11];
						}
						if (num2 > 0)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzMWaitGet;
						}
						return num2;
					}
					if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMWaitGet)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 4;
						array[3] = 8;
						array[4] = 1;
						num = this.GetSum(array, 0, 5);
						array[5] = (byte)((num >> 8) & 255);
						array[6] = (byte)(num & 255);
						if (this.CmdCommGet512(ref array, 7, ref resultData) == 1)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzMGetOK;
							return 1;
						}
					}
					if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMGetOK)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 6;
						array[3] = 6;
						array[4] = 1;
						array[5] = (byte)((this.foundIndexNew >> 8) & 255);
						array[6] = (byte)(this.foundIndexNew & 255);
						num = this.GetSum(array, 0, 7);
						array[7] = (byte)((num >> 8) & 255);
						array[8] = (byte)(num & 255);
						if (this.CmdComm(ref array, 9) == 1)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_none;
							return 1;
						}
					}
					if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_ClearAll)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 3;
						array[3] = 13;
						num = this.GetSum(array, 0, 4);
						array[4] = (byte)((num >> 8) & 255);
						array[5] = (byte)(num & 255);
						num2 = this.CmdComm(ref array, 6);
						if (num2 == 1 && this.RBF[9] == 0)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_none;
							SystemSounds.Beep.Play();
							wgAppConfig.wgLog(string.Format("{0}  \"{1}\"", "Clear All FingerPrint", wgTools.SetObjToStr(dfrmFingerEnroll.selectedFingerInputDevice)) + CommonStr.strSuccessfully);
						}
						return num2;
					}
					if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_UploadAll)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 4;
						array[3] = 9;
						array[4] = 1;
						num = this.GetSum(array, 0, 5);
						array[5] = (byte)((num >> 8) & 255);
						array[6] = (byte)(num & 255);
						if (this.CmdComm(ref array, 7) == 1 && this.RBF[9] == 0)
						{
							byte[] array2 = new byte[133];
							for (int i = 0; i < 4; i++)
							{
								array2[0] = 2;
								if (i == 3)
								{
									array2[0] = 8;
								}
								array2[1] = 0;
								array2[2] = 130;
								for (int j = 0; j < 128; j++)
								{
									array2[3 + j] = resultData[j + 128 * i];
								}
								num = this.GetSum(array2, 0, 131);
								array2[131] = (byte)((num >> 8) & 255);
								array2[132] = (byte)(num & 255);
								num2 = this.CmdCommPut128(ref array2, 133);
							}
							array[0] = 1;
							array[1] = 0;
							array[2] = 6;
							array[3] = 6;
							array[4] = 1;
							array[5] = (byte)((this.syncFingerIndex >> 8) & 255);
							array[6] = (byte)(this.syncFingerIndex & 255);
							num = this.GetSum(array, 0, 7);
							array[7] = (byte)((num >> 8) & 255);
							array[8] = (byte)(num & 255);
							if (this.CmdComm(ref array, 9) == 1 && this.RBF[9] == 0)
							{
								return 1;
							}
						}
						return 0;
					}
					if (this.fingerEnrollStatus != dfrmFingerEnroll.finger_status.fs_DownloadAll)
					{
						return 1;
					}
					array[0] = 1;
					array[1] = 0;
					array[2] = 6;
					array[3] = 7;
					array[4] = 1;
					array[5] = (byte)((this.syncFingerIndex >> 8) & 255);
					array[6] = (byte)(this.syncFingerIndex & 255);
					num = this.GetSum(array, 0, 7);
					array[7] = (byte)((num >> 8) & 255);
					array[8] = (byte)(num & 255);
					num2 = this.CmdComm(ref array, 9);
					if (num2 == 1 && this.RBF[9] == 0)
					{
						array[0] = 1;
						array[1] = 0;
						array[2] = 4;
						array[3] = 8;
						array[4] = 1;
						num = this.GetSum(array, 0, 5);
						array[5] = (byte)((num >> 8) & 255);
						array[6] = (byte)(num & 255);
						num2 = this.CmdCommGet512(ref array, 7, ref resultData);
						if (num2 == 1)
						{
							this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_tzMGetOK;
							return 1;
						}
					}
					return num2;
				}
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0019A129 File Offset: 0x00199129
		private void dfrmFingerEnroll_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.bStopEnroll = true;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0019A134 File Offset: 0x00199134
		private void dfrmUserAutoAdd_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 81 && e.Control && e.Shift)
			{
				if (this.btnClearAll.Visible)
				{
					this.btnDownloadAll.Visible = true;
				}
				this.btnClearAll.Visible = true;
				this.btnUploadAllUsers.Visible = true;
			}
			if (this.inputMode == 1)
			{
				foreach (object obj in base.Controls)
				{
					try
					{
						(obj as Control).ImeMode = ImeMode.Off;
					}
					catch
					{
					}
				}
				if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
				{
					if (this.inputCard.Length == 0)
					{
						this.timer1.Interval = 500;
						this.timer1.Enabled = true;
					}
					this.inputCard += (e.KeyValue - 48).ToString();
				}
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0019A26C File Offset: 0x0019926C
		private void dfrmUserAutoAdd_Load(object sender, EventArgs e)
		{
			this.loadDoorData();
			this.btnCancel.Visible = true;
			this.btnNext.Visible = true;
			this.groupBox1.Visible = true;
			this.button1.Visible = false;
			this.btnOK.Visible = false;
			this.btnExit.Visible = false;
			this.optController.Checked = true;
			this.optUSBReader.Visible = true;
			this.cboUART.Visible = true;
			this.optUSBReader.Enabled = false;
			this.cboUART.Enabled = false;
			string[] portNames = SerialPort.GetPortNames();
			for (int i = 0; i <= portNames.Length - 1; i++)
			{
				if (string.IsNullOrEmpty(frmADCT3000.portsNotUSB) || frmADCT3000.portsNotUSB.IndexOf("(" + portNames[i] + ")") < 0)
				{
					this.cboUART.Items.Add(portNames[i]);
				}
			}
			if (this.cboUART.Items.Count > 0)
			{
				this.optUSBReader.Enabled = true;
				this.cboUART.Visible = true;
				this.optUSBReader.Checked = true;
				this.cboUART.Enabled = true;
				this.cboUART.SelectedIndex = 0;
			}
			else
			{
				try
				{
					FileInfo fileInfo = new FileInfo(wgAppConfig.Path4PhotoDefault() + "\\Other\\USB_FTDI\\setup.exe");
					if (fileInfo.Exists)
					{
						this.btnInstallUSBDriver.Visible = true;
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			if (!string.IsNullOrEmpty(dfrmFingerEnroll.selectedFingerInputDevice))
			{
				if (dfrmFingerEnroll.usbOption != this.optUSBReader.Enabled)
				{
					dfrmFingerEnroll.selectedFingerInputDevice = null;
					return;
				}
				for (int j = 0; j < this.cboUART.Items.Count; j++)
				{
					if (wgTools.SetObjToStr(this.cboUART.Items[j]) == dfrmFingerEnroll.selectedFingerInputDevice)
					{
						this.optUSBReader.Checked = true;
						this.cboUART.SelectedIndex = j;
						this.btnNext.PerformClick();
						return;
					}
				}
				for (int k = 0; k < this.cboDoors.Items.Count; k++)
				{
					if (wgTools.SetObjToStr(this.cboDoors.Items[k]) == dfrmFingerEnroll.selectedFingerInputDevice)
					{
						this.optController.Checked = true;
						this.cboDoors.SelectedIndex = k;
						this.btnNext.PerformClick();
						return;
					}
				}
			}
			if (this.cboUART.Items.Count > 0)
			{
				this.optUSBReader.Checked = true;
				this.optUSBReader.Enabled = true;
				this.cboUART.Enabled = true;
				this.cboUART.SelectedIndex = 0;
				return;
			}
			if (this.bAutoAddBySwiping && this.dvDoors.Count > 0)
			{
				int num = -1;
				bool flag = true;
				for (int l = 0; l < this.dvDoors.Count; l++)
				{
					if (num == -1)
					{
						num = (int)this.dvDoors[l]["f_ControllerSN"];
					}
					else if (num != (int)this.dvDoors[l]["f_ControllerSN"])
					{
						flag = false;
						break;
					}
				}
				this.optController.Checked = true;
				if (flag)
				{
					this.btnNext.PerformClick();
				}
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0019A5EC File Offset: 0x001995EC
		private int fingerInput()
		{
			return 1;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0019A5F0 File Offset: 0x001995F0
		private int GetSum(byte[] p, int startloc, int len)
		{
			int num = (int)p[startloc];
			for (int i = 1; i < len; i++)
			{
				num += (int)p[i + startloc];
			}
			return num;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0019A616 File Offset: 0x00199616
		private void groupBox5_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0019A618 File Offset: 0x00199618
		private void loadDoorData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDoorData_Acc();
				return;
			}
			string text = " SELECT a.f_ControllerID, a.f_FingerPrintName , a.f_ControllerID, a.f_ControllerSN, a.f_IP,a.f_PORT, 0 as f_ConnectState, 0 as f_ZoneID ";
			text += " FROM t_b_Controller_FingerPrint a ORDER BY  a.f_FingerPrintName ";
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						sqlDataAdapter.Fill(this.dt);
						new icControllerZone();
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.cboDoors.Items.Clear();
						if (this.dvDoors.Count > 0)
						{
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_FingerPrintName"]));
							}
							if (this.cboDoors.Items.Count > 0)
							{
								this.cboDoors.SelectedIndex = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0019A7E8 File Offset: 0x001997E8
		private void loadDoorData_Acc()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID and b.f_ControllerSN>160100000 and b.f_ControllerSN<161000000 ORDER BY  a.f_DoorName ";
			text = " SELECT a.f_ControllerID, a.f_FingerPrintName , a.f_ControllerID, a.f_ControllerSN, a.f_IP,a.f_PORT, 0 as f_ConnectState, 0 as f_ZoneID ";
			text += " FROM t_b_Controller_FingerPrint a ORDER BY  a.f_FingerPrintName ";
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
				{
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.dt = new DataTable();
						this.dvDoors = new DataView(this.dt);
						this.dvDoors4Watching = new DataView(this.dt);
						oleDbDataAdapter.Fill(this.dt);
						new icControllerZone();
						try
						{
							this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
						this.cboDoors.Items.Clear();
						if (this.dvDoors.Count > 0)
						{
							for (int i = 0; i < this.dvDoors.Count; i++)
							{
								this.cboDoors.Items.Add(wgTools.SetObjToStr(this.dvDoors[i]["f_FingerPrintName"]));
							}
							if (this.cboDoors.Items.Count > 0)
							{
								this.cboDoors.SelectedIndex = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0019A9BC File Offset: 0x001999BC
		private long msToTicks(long ms)
		{
			return Convert.ToInt64(Convert.ToInt32(ms * 1000L) * 10);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0019A9D3 File Offset: 0x001999D3
		private void optController_CheckedChanged(object sender, EventArgs e)
		{
			this.cboDoors.Enabled = this.optController.Checked;
			this.cboUART.Enabled = this.optUSBReader.Checked;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0019AA01 File Offset: 0x00199A01
		private void optUSBReader_CheckedChanged(object sender, EventArgs e)
		{
			this.cboDoors.Enabled = this.optController.Checked;
			this.cboUART.Enabled = this.optUSBReader.Checked;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0019AA2F File Offset: 0x00199A2F
		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.bStopEnroll = true;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0019AA38 File Offset: 0x00199A38
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			this.bEnroll = false;
			if (this.optUSBReader.Checked)
			{
				this.usbReaderEnroll();
				return;
			}
			try
			{
				icController icController = new icController();
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					if ((string)this.dt.Rows[i]["f_FingerPrintName"] == this.cboDoors.Text)
					{
						icController.IP = wgTools.SetObjToStr(this.dt.Rows[i]["f_IP"]);
						icController.PORT = (int)this.dt.Rows[i]["f_Port"];
						icController.ControllerSN = (int)this.dt.Rows[i]["f_ControllerSN"];
						break;
					}
				}
				if (icController.ControllerSN != 0)
				{
					byte[] array = new byte[1152];
					byte[] array2 = new byte[1152];
					int num = 1;
					int num2 = 2;
					int num3 = 0;
					for (int j = 0; j < 512; j++)
					{
						this.dataFingerPrint[j] = 0;
					}
					array[0] = 241;
					array[2] = 15;
					array[3] = 238;
					array[4] = byte.MaxValue;
					int num4 = icController.FingerConfigureGetImageIP(array, array2);
					while (!this.bStopEnroll)
					{
						try
						{
							if (num == 1)
							{
								for (int k = 0; k < 1152; k++)
								{
									array[k] = 0;
									array2[k] = 0;
								}
								array[0] = 241;
								array[2] = 15;
								if (icController.FingerConfigureGetImageIPNotries(array, array2) > 0)
								{
									this.btnErrConnect.Visible = false;
									num3 = 0;
									if (array2[3] == 16)
									{
										int num5 = -1;
										bool flag = true;
										int num6 = 0;
										if (array2[num2 + 4] != 255 || array2[num2 + 5] != 255)
										{
											num5 = (int)array2[num2 + 4] + (int)array2[num2 + 5] * 256;
											if (num5 >= 0 && num5 < 1024)
											{
												int valBySql = wgAppConfig.getValBySql(string.Format("SELECT t_b_Consumer.f_ConsumerID From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num5 + 1));
												if (valBySql > 0 && valBySql != this.consumerID)
												{
													string valStringBySql = wgAppConfig.getValStringBySql(string.Format("SELECT t_b_Consumer.f_ConsumerName From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num5 + 1));
													XMessageBox.Show(string.Format("{0} {1}.", CommonStr.strFingerprintIsEnrolled, valStringBySql), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
													break;
												}
												if (valBySql > 0 && valBySql == this.consumerID)
												{
													num6 = wgAppConfig.getValBySql(string.Format("SELECT t_b_Consumer_Fingerprint.f_fingerID From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num5 + 1));
													flag = false;
												}
											}
											else
											{
												num5 = -1;
											}
										}
										if (BitConverter.ToString(array2).Replace("-", "").Substring(256, 1024)
											.Replace("00", "")
											.Length == 0)
										{
											array[0] = 241;
											array[2] = 15;
											array[3] = 238;
											array[4] = byte.MaxValue;
											num4 = icController.FingerConfigureGetImageIP(array, array2);
											XMessageBox.Show(this, CommonStr.strFingerprintEnrolledAddFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
											base.Close();
											base.DialogResult = DialogResult.Cancel;
											if (this.bSuccessOne)
											{
												base.DialogResult = DialogResult.OK;
											}
											return;
										}
										icConsumer icConsumer = new icConsumer();
										int num7 = 0;
										string text = BitConverter.ToString(array2).Replace("-", "").Substring(256, 1024);
										num7 = icConsumer.fingerprintNew(this.consumerID, num5, BitConverter.ToString(array2).Replace("-", "").Substring(256, 1024));
										if (num7 <= 0)
										{
											break;
										}
										for (int l = 0; l < 1152; l++)
										{
											array[l] = 0;
										}
										array[0] = 241;
										array[2] = 5;
										int num8 = num7 - 1;
										array[3] = (byte)(num8 & 255);
										array[4] = (byte)((num8 >> 8) & 255);
										long num9 = this.consumerCardNO;
										array[5] = (byte)(num9 & 255L);
										array[6] = (byte)((num9 >> 8) & 255L);
										array[7] = (byte)((num9 >> 16) & 255L);
										array[8] = (byte)((num9 >> 24) & 255L);
										for (int m = 128; m < 640; m++)
										{
											array[m] = array2[m];
										}
										for (int n = 0; n < 1152; n++)
										{
											array2[n] = 0;
										}
										if (this.cboDoors.Items.Count > 1)
										{
											ArrayList arrayList = new ArrayList();
											for (int num10 = 0; num10 < this.cboDoors.Items.Count; num10++)
											{
												if (num10 != this.cboDoors.SelectedIndex)
												{
													arrayList.Add(this.cboDoors.Items[num10]);
												}
											}
											lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
											{
												frmADCT3000.qfingerEnrollInfo.Enqueue(array);
												frmADCT3000.qfingerEnrollarrController.Enqueue(arrayList);
											}
										}
										num4 = icController.FingerConfigureIP(array, array2);
										int count = this.cboDoors.Items.Count;
										array[0] = 241;
										array[2] = 15;
										array[3] = 238;
										array[4] = byte.MaxValue;
										num4 = icController.FingerConfigureGetImageIP(array, array2);
										wgAppConfig.wgDebugWrite("停止注册\r\n");
										if (num4 <= 0)
										{
											break;
										}
										if (!flag)
										{
											string valStringBySql2 = wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", this.consumerID.ToString()));
											wgAppConfig.wgLogWithoutDB(CommonStr.strFingerprintEnrolledEdit + string.Format(" fingerprint Edit: ConsumerID={0},f_FingerNO={1}, info={2} ", this.consumerID.ToString() + " " + valStringBySql2, num7, text), EventLogEntryType.Information, null);
											string text2 = wgAppConfig.getValStringBySql(string.Format("SELECT f_Description From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num7.ToString()));
											if (string.IsNullOrEmpty(text2))
											{
												text2 = "ZW-" + wgAppConfig.getValStringBySql(string.Format("SELECT f_FingerID From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num7.ToString())).PadLeft(4, '0');
												text2 = "ZW-" + num6.ToString().PadLeft(4, '0') + " => " + text2;
											}
											string text3 = string.Format("{0}  \"{1}:{2}\".", CommonStr.strFingerprintEnrolledEdit, valStringBySql2, text2);
											using (dfrmFingerEnrollNext dfrmFingerEnrollNext = new dfrmFingerEnrollNext())
											{
												dfrmFingerEnrollNext.txtInfo.Text = text3;
												if (dfrmFingerEnrollNext.ShowDialog(this) == DialogResult.Retry)
												{
													this.bStopEnroll = false;
													this.timer1.Enabled = true;
													this.bSuccessOne = true;
													return;
												}
												goto IL_07E9;
											}
										}
										string valStringBySql3 = wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", this.consumerID.ToString()));
										wgAppConfig.wgLogWithoutDB(CommonStr.strFingerprintEnrolledEdit + string.Format(" fingerprint Add: ConsumerID={0}, UserName ={1}, f_FingerNO={2}, info={3} ", new object[]
										{
											this.consumerID.ToString(),
											valStringBySql3,
											num7,
											text
										}), EventLogEntryType.Information, null);
										string text4 = wgAppConfig.getValStringBySql(string.Format("SELECT f_Description From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num7.ToString()));
										if (string.IsNullOrEmpty(text4))
										{
											text4 = "ZW-" + wgAppConfig.getValStringBySql(string.Format("SELECT f_FingerID From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num7.ToString())).PadLeft(4, '0');
										}
										string text5 = string.Format("{0}  \"{1}:{2}\".", CommonStr.strFingerprintEnrolledAdd, valStringBySql3, text4);
										int valBySql2 = wgAppConfig.getValBySql(string.Format("SELECT count(*) FRom t_b_Consumer_Fingerprint WHERE  t_b_Consumer_Fingerprint.f_ConsumerID = {0}  ", this.consumerID));
										int valBySql3 = wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid");
										if (valBySql2 >= 10 || valBySql3 >= 1000)
										{
											XMessageBox.Show(text5);
										}
										else
										{
											using (dfrmFingerEnrollNext dfrmFingerEnrollNext2 = new dfrmFingerEnrollNext())
											{
												dfrmFingerEnrollNext2.txtInfo.Text = text5;
												if (dfrmFingerEnrollNext2.ShowDialog(this) == DialogResult.Retry)
												{
													this.bSuccessOne = true;
													this.bStopEnroll = false;
													this.timer1.Enabled = true;
													return;
												}
											}
										}
										IL_07E9:
										base.DialogResult = DialogResult.OK;
										if (this.bSuccessOne)
										{
											base.DialogResult = DialogResult.OK;
										}
										base.Close();
										return;
									}
								}
								else
								{
									num3++;
									if (num3 > 5)
									{
										this.btnErrConnect.Visible = true;
									}
								}
								Application.DoEvents();
								Thread.Sleep(100);
								Application.DoEvents();
								Thread.Sleep(100);
								Application.DoEvents();
								Thread.Sleep(100);
								Application.DoEvents();
								Thread.Sleep(100);
								Application.DoEvents();
								Thread.Sleep(100);
							}
							else
							{
								if (num == 1)
								{
									for (int num11 = 0; num11 < 1152; num11++)
									{
										array[num11] = 0;
										array2[num11] = 0;
									}
									array[0] = 241;
									num4 = icController.FingerConfigureGetImageIP(array, array2);
									if (num4 > 0)
									{
										string text6 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("读取指纹图像图像1");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text6.Substring(0, 30));
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("读取指纹图像失败\r\n");
									}
									if (num4 == 1 && array2[0] == 241 && array2[1] == 1 && array2[2] == 0)
									{
										num = 2;
									}
									Application.DoEvents();
								}
								if (num == 2)
								{
									for (int num12 = 0; num12 < 1152; num12++)
									{
										array[num12] = 0;
										array2[num12] = 0;
									}
									array[0] = 241;
									array[2] = 1;
									num4 = icController.FingerConfigureIP(array, array2);
									if (num4 > 0)
									{
										string text7 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("生成指纹特征1");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text7.Substring(0, 30));
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("生成指纹特征1失败\r\n");
									}
									if (num4 != 1 || array2[0] != 241 || array2[1] != 1 || array2[2] != 0)
									{
										num = 1;
									}
									else
									{
										for (int num13 = 0; num13 < 1152; num13++)
										{
											array[num13] = 0;
											array2[num13] = 0;
										}
										array[0] = 241;
										array[2] = 14;
										array[3] = 100;
										array[4] = 0;
										num4 = icController.FingerConfigureIP(array, array2);
										Thread.Sleep(500);
										num = 3;
									}
									Application.DoEvents();
								}
								if (num == 3)
								{
									for (int num14 = 0; num14 < 1152; num14++)
									{
										array[num14] = 0;
										array2[num14] = 0;
									}
									array[0] = 241;
									num4 = icController.FingerConfigureGetImageIP(array, array2);
									if (num4 > 0)
									{
										string text8 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("读取指纹图像图像2");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text8.Substring(0, 30));
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("读取指纹图像2失败\r\n");
									}
									if (num4 != 1 || array2[0] != 241 || array2[1] != 1 || array2[2] != 0)
									{
										num = 3;
									}
									else
									{
										num = 4;
									}
									Application.DoEvents();
								}
								if (num == 4)
								{
									for (int num15 = 0; num15 < 1152; num15++)
									{
										array[num15] = 0;
										array2[num15] = 0;
									}
									array[0] = 241;
									array[2] = 2;
									num4 = icController.FingerConfigureIP(array, array2);
									if (num4 > 0)
									{
										string text9 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("生成指纹特征2");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text9.Substring(0, 30));
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("生成指纹特征2失败\r\n");
									}
									if (num4 != 1 || array2[0] != 241 || array2[1] != 1 || array2[2] != 0)
									{
										num = 3;
									}
									else
									{
										num = 5;
									}
									Application.DoEvents();
								}
								if (num == 5)
								{
									for (int num16 = 0; num16 < 1152; num16++)
									{
										array[num16] = 0;
										array2[num16] = 0;
									}
									array[0] = 241;
									array[2] = 3;
									num4 = icController.FingerConfigureIP(array, array2);
									if (num4 > 0)
									{
										string text10 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("特征合成模板RegModel");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text10.Substring(0, 30));
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("特征合成模板RegModel失败\r\n");
									}
									if (num4 != 1 || array2[0] != 241 || array2[1] != 1 || array2[2] != 0)
									{
										num = 1;
										for (int num17 = 0; num17 < 1152; num17++)
										{
											array[num17] = 0;
											array2[num17] = 0;
										}
										array[0] = 241;
										array[2] = 14;
										array[3] = 100;
										array[4] = 0;
										num4 = icController.FingerConfigureIP(array, array2);
										Thread.Sleep(2000);
									}
									else
									{
										num = 6;
									}
									Application.DoEvents();
								}
								if (num == 6)
								{
									for (int num18 = 0; num18 < 1152; num18++)
									{
										array[num18] = 0;
										array2[num18] = 0;
									}
									array[0] = 241;
									array[2] = 4;
									num4 = icController.FingerConfigureIP(array, array2);
									if (num4 > 0)
									{
										string text11 = BitConverter.ToString(array2);
										wgAppConfig.wgDebugWrite("读取模板RegModel");
										wgAppConfig.wgDebugWrite("\r\n");
										wgAppConfig.wgDebugWrite(DateTime.Now.ToString("HH:mm:ss.ffff recv: ") + text11);
										wgAppConfig.wgDebugWrite("\r\n");
									}
									else
									{
										wgAppConfig.wgDebugWrite("读取模板RegModel失败\r\n");
									}
									if (num4 != 1 || array2[0] != 241 || array2[1] != 1 || array2[2] != 0)
									{
										XMessageBox.Show("读取模板RegModel失败");
									}
									else
									{
										for (int num19 = 0; num19 < 512; num19++)
										{
											this.dataFingerPrint[num19] = array2[128 + num19];
										}
										this.bEnroll = true;
										if (this.bEnroll)
										{
											wgTools.WgDebugWrite("bEnroll Start", new object[0]);
										}
										for (int num20 = 0; num20 < 1152; num20++)
										{
											array[num20] = 0;
											array2[num20] = 0;
										}
										array[0] = 241;
										array[2] = 14;
										array[3] = 200;
										array[4] = 1;
										num4 = icController.FingerConfigureIP(array, array2);
										XMessageBox.Show("读取模板RegModel 成功");
									}
									break;
								}
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgDebugWrite(ex.ToString());
						}
					}
					array[0] = 241;
					array[2] = 15;
					array[3] = 238;
					array[4] = byte.MaxValue;
					num4 = icController.FingerConfigureGetImageIP(array, array2);
					icController.Dispose();
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0019B918 File Offset: 0x0019A918
		private void usbReaderEnroll()
		{
			try
			{
				dfrmFingerEnroll.selectedFingerInputDevice = this.cboUART.Text;
				if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
				{
					this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_getImage1;
				}
				int num = 0;
				byte[] array = new byte[1152];
				this.wgSerial.ClosePort();
				string text = dfrmFingerEnroll.selectedFingerInputDevice;
				this.wgSerial.Baudrate = 57600L;
				int num2;
				if (int.TryParse(text.Replace("COM", ""), out num2))
				{
					this.wgSerial.CommPort = byte.Parse(text.Replace("COM", ""));
				}
				else
				{
					this.wgSerial.CommPort = 1;
				}
				this.wgSerial.ClosePort();
				if (this.wgSerial.OpenPort() != 0L)
				{
					this.dfrmWait1.Hide();
					this.dfrmWait1.Refresh();
					XMessageBox.Show(dfrmFingerEnroll.selectedFingerInputDevice + "\t" + CommonStr.strOpenFailed);
				}
				else
				{
					while (!this.bStopEnroll)
					{
						try
						{
							long num3 = (long)this.dealFingerEnroll(ref array);
							if (num3 > 0L)
							{
								this.btnErrConnect.Visible = false;
								num = 0;
								if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_tzMGetOK)
								{
									int num4 = -1;
									int num5 = 0;
									bool flag = true;
									if (this.foundIndexNew >= 0)
									{
										num4 = this.foundIndexNew;
										if (num4 >= 0 && num4 < 1024)
										{
											int valBySql = wgAppConfig.getValBySql(string.Format("SELECT t_b_Consumer.f_ConsumerID From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num4 + 1));
											if (valBySql > 0 && valBySql != this.consumerID)
											{
												string valStringBySql = wgAppConfig.getValStringBySql(string.Format("SELECT t_b_Consumer.f_ConsumerName From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num4 + 1));
												string[] array2 = new string[]
												{
													string.Format("{0} {1}.", CommonStr.strFingerprintIsEnrolled, valStringBySql),
													"\r\nf_FingerNO =",
													(num4 + 1).ToString(),
													",userID= ",
													valBySql.ToString()
												};
												wgAppConfig.wgLog(string.Concat(array2));
												this.bStopEnroll = true;
												this.fingerEnrollStatus = dfrmFingerEnroll.finger_status.fs_none;
												XMessageBox.Show(string.Format("{0} {1}.", CommonStr.strFingerprintIsEnrolled, valStringBySql), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
												break;
											}
											if (valBySql > 0 && valBySql == this.consumerID)
											{
												num5 = wgAppConfig.getValBySql(string.Format("SELECT t_b_Consumer_Fingerprint.f_fingerID From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num4 + 1));
												flag = false;
											}
										}
										else
										{
											num4 = -1;
										}
									}
									if (BitConverter.ToString(array).Replace("-", "").Substring(0, 1024)
										.Replace("00", "")
										.Length == 0)
									{
										XMessageBox.Show(this, CommonStr.strFingerprintEnrolledAddFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
										base.Close();
										base.DialogResult = DialogResult.Cancel;
										if (this.bSuccessOne)
										{
											base.DialogResult = DialogResult.OK;
										}
										break;
									}
									icConsumer icConsumer = new icConsumer();
									int num6 = 0;
									string text2 = BitConverter.ToString(array).Replace("-", "").Substring(0, 1024);
									num6 = icConsumer.fingerprintNew(this.consumerID, num4, BitConverter.ToString(array).Replace("-", "").Substring(0, 1024));
									if (num6 <= 0)
									{
										break;
									}
									byte[] array3 = new byte[1152];
									for (int i = 0; i < 1152; i++)
									{
										array3[i] = 0;
									}
									array3[0] = 241;
									array3[2] = 5;
									int num7 = num6 - 1;
									array3[3] = (byte)(num7 & 255);
									array3[4] = (byte)((num7 >> 8) & 255);
									long num8 = this.consumerCardNO;
									array3[5] = (byte)(num8 & 255L);
									array3[6] = (byte)((num8 >> 8) & 255L);
									array3[7] = (byte)((num8 >> 16) & 255L);
									array3[8] = (byte)((num8 >> 24) & 255L);
									for (int j = 128; j < 640; j++)
									{
										array3[j] = array[j - 128];
									}
									for (int k = 0; k < 1152; k++)
									{
										array[k] = 0;
									}
									if (this.cboDoors.Items.Count > 0)
									{
										ArrayList arrayList = new ArrayList();
										for (int l = 0; l < this.cboDoors.Items.Count; l++)
										{
											arrayList.Add(this.cboDoors.Items[l]);
										}
										lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
										{
											frmADCT3000.qfingerEnrollInfo.Enqueue(array3);
											frmADCT3000.qfingerEnrollarrController.Enqueue(arrayList);
										}
									}
									this.foundIndexNew = num7;
									num3 = (long)this.dealFingerEnroll(ref array);
									if (!flag)
									{
										string valStringBySql2 = wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", this.consumerID.ToString()));
										wgAppConfig.wgLogWithoutDB(CommonStr.strFingerprintEnrolledEdit + string.Format(" fingerprint Edit: ConsumerID={0},f_FingerNO={1}, info={2} ", this.consumerID.ToString() + " " + valStringBySql2, num6, text2), EventLogEntryType.Information, null);
										string text3 = wgAppConfig.getValStringBySql(string.Format("SELECT f_Description From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num6.ToString()));
										if (string.IsNullOrEmpty(text3))
										{
											text3 = "ZW-" + wgAppConfig.getValStringBySql(string.Format("SELECT f_FingerID From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num6.ToString())).PadLeft(4, '0');
											text3 = "ZW-" + num5.ToString().PadLeft(4, '0') + " => " + text3;
										}
										string text4 = string.Format("{0}  \"{1}:{2}\".", CommonStr.strFingerprintEnrolledEdit, valStringBySql2, text3);
										using (dfrmFingerEnrollNext dfrmFingerEnrollNext = new dfrmFingerEnrollNext())
										{
											dfrmFingerEnrollNext.txtInfo.Text = text4;
											if (dfrmFingerEnrollNext.ShowDialog(this) == DialogResult.Retry)
											{
												this.bStopEnroll = false;
												this.timer1.Enabled = true;
												this.bSuccessOne = true;
												break;
											}
											goto IL_0725;
										}
									}
									string valStringBySql3 = wgAppConfig.getValStringBySql(string.Format("SELECT f_ConsumerName From t_b_Consumer WHERE  f_ConsumerID ={0}", this.consumerID.ToString()));
									wgAppConfig.wgLogWithoutDB(CommonStr.strFingerprintEnrolledEdit + string.Format(" fingerprint Add: ConsumerID={0}, UserName ={1}, f_FingerNO={2}, info={3} ", new object[]
									{
										this.consumerID.ToString(),
										valStringBySql3,
										num6,
										text2
									}), EventLogEntryType.Information, null);
									string text5 = wgAppConfig.getValStringBySql(string.Format("SELECT f_Description From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num6.ToString()));
									if (string.IsNullOrEmpty(text5))
									{
										text5 = "ZW-" + wgAppConfig.getValStringBySql(string.Format("SELECT f_FingerID From t_b_Consumer_Fingerprint WHERE  f_FingerNO ={0}", num6.ToString())).PadLeft(4, '0');
									}
									string text6 = string.Format("{0}  \"{1}:{2}\".", CommonStr.strFingerprintEnrolledAdd, valStringBySql3, text5);
									int valBySql2 = wgAppConfig.getValBySql(string.Format("SELECT count(*) FRom t_b_Consumer_Fingerprint WHERE  t_b_Consumer_Fingerprint.f_ConsumerID = {0}", this.consumerID));
									int valBySql3 = wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid");
									if (valBySql2 >= 10 || valBySql3 >= 1000)
									{
										XMessageBox.Show(text6);
									}
									else
									{
										using (dfrmFingerEnrollNext dfrmFingerEnrollNext2 = new dfrmFingerEnrollNext())
										{
											dfrmFingerEnrollNext2.txtInfo.Text = text6;
											if (dfrmFingerEnrollNext2.ShowDialog(this) == DialogResult.Retry)
											{
												this.bStopEnroll = false;
												this.timer1.Enabled = true;
												this.bSuccessOne = true;
												break;
											}
										}
									}
									IL_0725:
									base.DialogResult = DialogResult.OK;
									if (this.bSuccessOne)
									{
										base.DialogResult = DialogResult.OK;
									}
									base.Close();
									break;
								}
							}
							else
							{
								num++;
								if (num > 5)
								{
									this.btnErrConnect.Visible = true;
								}
							}
							Application.DoEvents();
							if (this.fingerEnrollStatus == dfrmFingerEnroll.finger_status.fs_none)
							{
								break;
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgDebugWrite(ex.ToString());
						}
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgDebugWrite(ex2.ToString());
			}
			finally
			{
				this.wgSerial.ClosePort();
			}
		}

		// Token: 0x04002B5B RID: 11099
		private const int Mode_ControllerReader = 2;

		// Token: 0x04002B5C RID: 11100
		private const int Mode_ManualInput = 3;

		// Token: 0x04002B5D RID: 11101
		private const int Mode_USBReader = 1;

		// Token: 0x04002B5E RID: 11102
		private const int RBUF_LEN = 16;

		// Token: 0x04002B5F RID: 11103
		private bool bEnroll;

		// Token: 0x04002B60 RID: 11104
		private bool bStopEnroll;

		// Token: 0x04002B61 RID: 11105
		private bool bSuccessOne;

		// Token: 0x04002B62 RID: 11106
		private DataTable dt;

		// Token: 0x04002B63 RID: 11107
		private DataView dvDoors;

		// Token: 0x04002B64 RID: 11108
		private DataView dvDoors4Watching;

		// Token: 0x04002B65 RID: 11109
		private dfrmFingerEnroll.finger_status fingerEnrollStatus;

		// Token: 0x04002B66 RID: 11110
		private int inputMode;

		// Token: 0x04002B67 RID: 11111
		private int syncFingerIndex;

		// Token: 0x04002B68 RID: 11112
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04002B69 RID: 11113
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04002B6A RID: 11114
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04002B6B RID: 11115
		private byte[] BAGHEAD = new byte[] { 239, 1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue };

		// Token: 0x04002B6C RID: 11116
		public bool bAutoAddBySwiping = true;

		// Token: 0x04002B6D RID: 11117
		public long consumerCardNO;

		// Token: 0x04002B6E RID: 11118
		public int consumerID;

		// Token: 0x04002B6F RID: 11119
		public byte[] dataFingerPrint = new byte[512];

		// Token: 0x04002B71 RID: 11121
		private int foundIndexNew = -1;

		// Token: 0x04002B72 RID: 11122
		public Form frmCall;

		// Token: 0x04002B73 RID: 11123
		private string inputCard = "";

		// Token: 0x04002B74 RID: 11124
		private byte[] RBF = new byte[16];

		// Token: 0x04002B75 RID: 11125
		private static string selectedFingerInputDevice = "";

		// Token: 0x04002B76 RID: 11126
		private static bool usbOption = false;

		// Token: 0x04002B77 RID: 11127
		private wgSerialComm wgSerial = new wgSerialComm();

		// Token: 0x020002E0 RID: 736
		private enum finger_status
		{
			// Token: 0x04002B90 RID: 11152
			fs_ClearAll = 128,
			// Token: 0x04002B91 RID: 11153
			fs_DownloadAll = 130,
			// Token: 0x04002B92 RID: 11154
			fs_END = 255,
			// Token: 0x04002B93 RID: 11155
			fs_getImage1 = 1,
			// Token: 0x04002B94 RID: 11156
			fs_getImage1b = 18,
			// Token: 0x04002B95 RID: 11157
			fs_getImage2 = 3,
			// Token: 0x04002B96 RID: 11158
			fs_getImage2b = 50,
			// Token: 0x04002B97 RID: 11159
			fs_none = 0,
			// Token: 0x04002B98 RID: 11160
			fs_tzM1 = 2,
			// Token: 0x04002B99 RID: 11161
			fs_tzM2 = 4,
			// Token: 0x04002B9A RID: 11162
			fs_tzMCreate,
			// Token: 0x04002B9B RID: 11163
			fs_tzMGetOK = 16,
			// Token: 0x04002B9C RID: 11164
			fs_tzMSearch = 7,
			// Token: 0x04002B9D RID: 11165
			fs_tzMWaitGet = 6,
			// Token: 0x04002B9E RID: 11166
			fs_UploadAll = 129
		}
	}
}
