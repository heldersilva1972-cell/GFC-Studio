using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;

namespace WG3000_COMM
{
	// Token: 0x0200032D RID: 813
	public partial class frmProductFormat : frmN3000
	{
		// Token: 0x060019AB RID: 6571 RVA: 0x00218E74 File Offset: 0x00217E74
		public frmProductFormat()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0021954C File Offset: 0x0021854C
		private void autoFormatLogEntry(icController control)
		{
			this.logID++;
			wgAppConfig.wgLogWithoutDB(this.logID.ToString() + ".格式化: " + control.ControllerSN.ToString(), EventLogEntryType.Warning, null);
			frmProductFormat.wgLogProduct(this.logID.ToString() + ".格式化: " + control.ControllerSN.ToString(), "n3k_Format");
			this.wgLogProduct4BasicInfo(control);
			this.label12.Visible = true;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x002195D4 File Offset: 0x002185D4
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			icController icController = new icController();
			try
			{
				Thread.Sleep(300);
				for (;;)
				{
					if (!this.checkBox1.Checked)
					{
						try
						{
							icController.ControllerSN = -1;
							icController.runinfo.Clear();
							icController.GetControllerRunInformationIPNoTries();
							base.Invoke(new frmProductFormat.dispDoorStatusByIPComm(this.dispDoorStatusByIPCommEntry), new object[] { icController });
							if (this.chkAutoFormat.Checked && icController.runinfo.wgcticks > 0U && (Math.Abs(DateTime.Now.Subtract(icController.runinfo.dtNow).TotalMinutes) < 5.0 || wgMjController.IsFingerController((int)icController.runinfo.CurrentControllerSN)) && (icController.runinfo.reservedBytes[0] == 0 || wgMjController.IsFingerController((int)icController.runinfo.CurrentControllerSN)) && icController.runinfo.appError == 0 && this.validSN((int)icController.runinfo.CurrentControllerSN, icController.ControllerProductTypeCode))
							{
								icController.ControllerSN = (int)icController.runinfo.CurrentControllerSN;
								if (this.lastErrController == (long)icController.ControllerSN)
								{
									continue;
								}
								if ((long)this.lastFormatController == (long)((ulong)icController.runinfo.CurrentControllerSN) && icController.runinfo.registerCardNumTotal == 0U && icController.runinfo.swipeEndIndex == 0U && icController.runinfo.swipeStartIndex == 0U)
								{
									this.lastErrController = 0L;
									continue;
								}
								int num = 0;
								int num2 = 0;
								int num3 = 0;
								for (int i = 0; i < 200; i++)
								{
									num++;
									if (icController.SpecialPingIP() == 1)
									{
										num2++;
									}
									else
									{
										num3++;
									}
								}
								if (num != num2)
								{
									if (this.lastErrController != (long)icController.ControllerSN)
									{
										this.lastErrController = (long)icController.ControllerSN;
										string text = string.Format("SN{3} 有故障: 通信丢包\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, icController.ControllerSN }) + "\r\n";
										base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text });
									}
								}
								else
								{
									wgUdpComm.triesTotal = 0L;
									wgTools.WriteLine("control.Test1024 Start");
									int num4 = 0;
									string text2 = "";
									if (icController.test1024Write() < 0)
									{
										text2 += "大数据包写入失败\r\n";
									}
									int num5 = icController.test1024Read(100U, ref num4);
									if (num5 < 0)
									{
										text2 = text2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
									}
									if (wgUdpComm.triesTotal > 0L)
									{
										text2 = text2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
									}
									if (text2 != "")
									{
										if (this.lastErrController != (long)icController.ControllerSN)
										{
											this.lastErrController = (long)icController.ControllerSN;
											string text3 = string.Concat(new string[]
											{
												"SN",
												icController.ControllerSN.ToString(),
												"通信有故障: : ",
												text2,
												"\r\n"
											});
											base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text3 });
										}
									}
									else
									{
										string text4 = "";
										base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
										this.lastErrController = 0L;
										wgMjControllerConfigure wgMjControllerConfigure = null;
										icController.GetConfigureIP(ref wgMjControllerConfigure);
										byte[] array = new byte[1152];
										array[1027] = 165;
										array[1026] = 165;
										array[1025] = 165;
										array[1024] = 165;
										icController.UpdateConfigureSuperIP(array);
										this.lastFormatController = icController.ControllerSN;
										base.Invoke(new frmProductFormat.autoFormatLog(this.autoFormatLogEntry), new object[] { icController });
									}
								}
							}
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[0]);
							break;
						}
					}
					Thread.Sleep(300);
				}
			}
			catch (Exception)
			{
			}
			icController.Dispose();
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00219A80 File Offset: 0x00218A80
		private void btnAdjustTime_Click(object sender, EventArgs e)
		{
			int num;
			if (int.TryParse(this.txtSN.Text, out num))
			{
				icController icController = new icController();
				try
				{
					icController.ControllerSN = num;
					icController.AdjustTimeIP(DateTime.Now);
					frmProductFormat.wgLogProduct("校准时间: " + icController.ControllerSN.ToString(), "n3k_Format");
				}
				catch (Exception)
				{
				}
				icController.Dispose();
			}
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00219AF8 File Offset: 0x00218AF8
		private void btnEditCode_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
				{
					return;
				}
			}
			using (dfrmPrintCodeInfo dfrmPrintCodeInfo = new dfrmPrintCodeInfo())
			{
				if (dfrmPrintCodeInfo.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO")))
				{
					this.productTypeInfo = wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO").Split(new char[] { ',' });
				}
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00219BA8 File Offset: 0x00218BA8
		private void btnPing_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				try
				{
					icController.ControllerSN = -1;
					icController.runinfo.Clear();
					icController.GetControllerRunInformationIPNoTries();
					if (icController.runinfo.wgcticks > 0U)
					{
						icController.ControllerSN = (int)icController.runinfo.CurrentControllerSN;
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						for (int i = 0; i < 200; i++)
						{
							num++;
							if (icController.SpecialPingIP() == 1)
							{
								num2++;
							}
							else
							{
								num3++;
							}
						}
						if (num != num2)
						{
							string text = DateTime.Now.ToString("HH:mm:ss ") + string.Format("SN{3} 有故障: 通信丢包\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, icController.ControllerSN }) + "\r\n";
							base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text });
						}
						else
						{
							wgUdpComm.triesTotal = 0L;
							wgTools.WriteLine("control.Test1024 Start");
							int num4 = 0;
							string text2 = "";
							if (icController.test1024Write() < 0)
							{
								text2 += "大数据包写入失败\r\n";
							}
							int num5 = icController.test1024Read(100U, ref num4);
							if (num5 < 0)
							{
								text2 = text2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
							}
							if (wgUdpComm.triesTotal > 0L)
							{
								text2 = text2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
							}
							if (text2 != "")
							{
								string text3 = string.Concat(new string[]
								{
									DateTime.Now.ToString("HH:mm:ss "),
									"SN",
									icController.ControllerSN.ToString(),
									"通信有故障: : ",
									text2,
									"\r\n"
								});
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text3 });
							}
							else
							{
								string text4 = DateTime.Now.ToString("HH:mm:ss ") + string.Format("SN{3} 通信正常\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, icController.ControllerSN }) + "\r\n";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text2 = "大数据包测试成功(测试100次)";
								text4 = icController.ControllerSN.ToString() + ": " + text2 + "\r\n";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								this.lastErrController = 0L;
							}
						}
					}
					else
					{
						string text5 = "通信不上";
						base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text5 });
						text5 = "";
						base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text5 });
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00219FE8 File Offset: 0x00218FE8
		private void btnStop_Click(object sender, EventArgs e)
		{
			this.bStopRemoteEvalator = true;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00219FF4 File Offset: 0x00218FF4
		private void button1_Click(object sender, EventArgs e)
		{
			this.txtTime.Text = "";
			this.txtSN.Text = "";
			this.txtRunInfo.Text = "";
			this.lastErrController = 0L;
			this.lastFormatController = 0;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0021A040 File Offset: 0x00219040
		private void button2_Click(object sender, EventArgs e)
		{
			icController icController = new icController();
			try
			{
				icController.ControllerSN = -1;
				byte[] array = new byte[1152];
				array[1027] = 165;
				array[1026] = 165;
				array[1025] = 165;
				array[1024] = 165;
				icController.UpdateConfigureSuperIP(array);
				this.label12.Visible = true;
			}
			catch (Exception)
			{
			}
			icController.Dispose();
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0021A0C4 File Offset: 0x002190C4
		private void button57_Click(object sender, EventArgs e)
		{
			icController icController = new icController();
			try
			{
				icController.ControllerSN = -1;
				icController.runinfo.Clear();
				icController.GetControllerRunInformationIP(-1);
				if (icController.runinfo.wgcticks <= 0U)
				{
					this.txtRunInfo.AppendText("???控制器未连接\r\n");
					SystemSounds.Hand.Play();
				}
				else
				{
					if (this.checkBox116.Checked)
					{
						MjRegisterCard mjRegisterCard = new MjRegisterCard();
						mjRegisterCard.IsActivated = true;
						mjRegisterCard.Password = uint.Parse(345678.ToString());
						mjRegisterCard.ymdStart = DateTime.Parse("2010-1-1");
						mjRegisterCard.ymdEnd = DateTime.Parse("2099-12-31");
						mjRegisterCard.ControlSegIndexSet(1, 1);
						mjRegisterCard.ControlSegIndexSet(2, 1);
						mjRegisterCard.ControlSegIndexSet(3, 1);
						mjRegisterCard.ControlSegIndexSet(4, 1);
						icPrivilege icPrivilege = new icPrivilege();
						try
						{
							string text = this.textBox32.Text;
							if (!string.IsNullOrEmpty(text))
							{
								string[] array = text.Split(new char[] { ',' });
								if (array.Length > 0)
								{
									for (int i = 0; i < array.Length; i++)
									{
										uint num;
										if (uint.TryParse(array[i].Trim(), NumberStyles.Integer, null, out num) && num > 0U)
										{
											mjRegisterCard.CardID = (long)((ulong)num);
											icPrivilege.AddPrivilegeOfOneCardIP(-1, "", 60000, mjRegisterCard);
										}
									}
								}
							}
						}
						catch (Exception)
						{
						}
						icPrivilege.Dispose();
					}
					if (this.checkBox117.Checked)
					{
						wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
						wgMjControllerConfigure.RestoreDefault();
						wgMjControllerConfigure.Ext_doorSet(0, 0);
						wgMjControllerConfigure.Ext_doorSet(1, 1);
						wgMjControllerConfigure.Ext_doorSet(2, 2);
						wgMjControllerConfigure.Ext_doorSet(3, 3);
						wgMjControllerConfigure.Ext_controlSet(0, 4);
						wgMjControllerConfigure.Ext_controlSet(1, 4);
						wgMjControllerConfigure.Ext_controlSet(2, 4);
						wgMjControllerConfigure.Ext_controlSet(3, 4);
						wgMjControllerConfigure.Ext_warnSignalEnabled2Set(0, 2);
						wgMjControllerConfigure.Ext_warnSignalEnabled2Set(1, 2);
						wgMjControllerConfigure.Ext_warnSignalEnabled2Set(2, 2);
						wgMjControllerConfigure.Ext_warnSignalEnabled2Set(3, 2);
						icController.UpdateConfigureIP(wgMjControllerConfigure, -1);
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				icController.Dispose();
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0021A310 File Offset: 0x00219310
		private void button70_Click(object sender, EventArgs e)
		{
			this.button70.Enabled = false;
			this.button72.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			icController icController = new icController();
			try
			{
				try
				{
					this.bStopRemoteEvalator = false;
					uint num = 0U;
					long num2 = -1L;
					icController.ControllerSN = int.Parse(this.txtSN.Text);
					int num3 = 0;
					int num4 = 1;
					if (sender == this.button72)
					{
						num3 = 20;
						if ((int)this.numericUpDown2.Value >= 21 && (int)this.numericUpDown2.Value <= 40)
						{
							num4 = (int)this.numericUpDown2.Value - num3;
						}
					}
					else if ((int)this.numericUpDown1.Value >= 1 && (int)this.numericUpDown1.Value <= 20)
					{
						num4 = (int)this.numericUpDown1.Value - num3;
					}
					int num5 = int.Parse(this.numericUpDown22.Value.ToString());
					while (num4 <= 20 && !this.bStopRemoteEvalator)
					{
						if (this.optNO.Checked)
						{
							icController.RemoteOpenFoorIP(num4 + num3, num, num2);
						}
						else
						{
							icController.RemoteOpenFoorIP(num4 + 40 + num3, num, num2);
						}
						this.lblFloor.Text = (num4 + num3).ToString();
						Application.DoEvents();
						int num6 = 0;
						while (num6 < num5 && !this.bStopRemoteEvalator)
						{
							Application.DoEvents();
							Thread.Sleep(300);
							num6 += 300;
						}
						num4++;
					}
				}
				catch (Exception)
				{
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				XMessageBox.Show(ex.ToString());
			}
			finally
			{
				icController.Dispose();
			}
			this.button70.Enabled = true;
			this.button72.Enabled = true;
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0021A538 File Offset: 0x00219538
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.btnFormat.Visible = this.checkBox1.Checked;
			if (this.checkBox1.Checked)
			{
				this.btnConnected.BackColor = Color.Red;
				this.btnConnected.Visible = true;
			}
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0021A584 File Offset: 0x00219584
		private bool compareProductInfo(string str1, string str2)
		{
			string text = str1.Replace("*", "");
			string text2 = str2.Replace("*", "");
			if (text.Length > text2.Length)
			{
				text = text.Substring(0, text2.Length);
			}
			else
			{
				text2 = text2.Substring(0, text.Length);
			}
			return text.Length != 0 && text.CompareTo(text2) == 0;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0021A5F4 File Offset: 0x002195F4
		private void dispDoorStatusByIPCommEntry(icController control)
		{
			if (control.runinfo.wgcticks <= 0U)
			{
				this.btnConnected.BackColor = Color.Red;
				this.btnConnected.Visible = true;
				return;
			}
			if (string.Compare(this.txtSN.Text, control.runinfo.CurrentControllerSN.ToString()) != 0)
			{
				this.txtSN.Text = control.runinfo.CurrentControllerSN.ToString();
			}
			int currentControllerSN = (int)control.runinfo.CurrentControllerSN;
			if (currentControllerSN < 100000000 || currentControllerSN > 500000000)
			{
				XMessageBox.Show(this, "序列号无效", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (currentControllerSN >= 300000000 && currentControllerSN < 400000000)
			{
				XMessageBox.Show(this, "序列号无效", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int num = currentControllerSN / 10000000 % 10;
			int num2 = currentControllerSN / 1000000 % 100;
			bool flag = true;
			switch (control.ControllerProductTypeCode)
			{
			case 1:
				if (num != 1 && (num2 != 71 || currentControllerSN < 171200001))
				{
					XMessageBox.Show(this, "序列号 与控制器当前的型号(ADCT) 不匹配!!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					flag = false;
					goto IL_01D1;
				}
				goto IL_01D1;
			case 2:
				if (num != 2)
				{
					XMessageBox.Show(this, "序列号 与控制器当前的型号(中性) 不匹配!!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					flag = false;
					goto IL_01D1;
				}
				goto IL_01D1;
			case 3:
				if ((currentControllerSN < 171100001 || currentControllerSN >= 171100099) && (currentControllerSN < 160100000 || currentControllerSN >= 160999999) && num != 3 && num2 != 73)
				{
					XMessageBox.Show(this, "序列号 与控制器当前的型号(Adroitor) 不匹配!!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					flag = false;
					goto IL_01D1;
				}
				goto IL_01D1;
			case 5:
				if (num != 5 && num2 != 75)
				{
					XMessageBox.Show(this, "序列号 与控制器当前的型号(WGAccess) 不匹配!!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					flag = false;
					goto IL_01D1;
				}
				goto IL_01D1;
			}
			XMessageBox.Show(this, "控制器当前的型号 不存在...!!", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			flag = false;
			IL_01D1:
			if (!flag)
			{
				wgTools.WgDebugWrite("bContinue = false", new object[0]);
			}
			this.txtTime.Text = control.runinfo.dtNow.ToString("yyyy-MM-dd HH:mm:ss");
			bool flag2 = false;
			if (Math.Abs(DateTime.Now.Subtract(control.runinfo.dtNow).TotalMinutes) >= 5.0)
			{
				this.txtTime.BackColor = Color.Red;
				this.label8.Text = "时钟: 有问题";
				flag2 = true;
			}
			else
			{
				this.txtTime.BackColor = Color.White;
				this.label8.Text = "时钟:";
			}
			if (control.runinfo.reservedBytes[0] > 0)
			{
				this.btnConnected.BackColor = Color.Yellow;
				if (this.label3.Text != control.runinfo.reservedBytes[0].ToString())
				{
					this.label3.Text = control.runinfo.reservedBytes[0].ToString();
					this.lblFailDetail.Text = icDesc.failedPinDesc((int)control.runinfo.reservedBytes[0]);
					if (control.runinfo.reservedBytes[0] == 88)
					{
						this.label3.Text = "";
					}
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(this.label3.Text))
				{
					this.label3.Text = "";
					this.lblFailDetail.Text = "";
				}
				if (!string.IsNullOrEmpty(this.lblFailDetail.Text))
				{
					this.label3.Text = "";
					this.lblFailDetail.Text = "";
				}
			}
			if (control.runinfo.appError > 0)
			{
				this.btnConnected.BackColor = Color.Yellow;
				if (this.label4.Text != control.runinfo.appError.ToString())
				{
					this.label4.Text = control.runinfo.appError.ToString();
				}
			}
			else if (!string.IsNullOrEmpty(this.label4.Text))
			{
				this.label4.Text = "";
			}
			if (control.runinfo.reservedBytes[0] <= 0 && control.runinfo.appError == 0 && !flag2 && this.validSN((int)control.runinfo.CurrentControllerSN, control.ControllerProductTypeCode))
			{
				this.btnConnected.BackColor = Color.Green;
			}
			if (wgMjController.IsElevator((int)control.runinfo.CurrentControllerSN) && control.runinfo.mutliInput40 != 0UL)
			{
				string text = "";
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = (int)((control.runinfo.mutliInput40 >> 24) & 16777215UL);
				if (num6 != 0 && (num6 & 8388608) > 0)
				{
					for (int i = 0; i < 20; i++)
					{
						if ((num6 & (1 << i)) > 0)
						{
							num4++;
						}
						else
						{
							text = text + (i + 1).ToString() + ",";
							num3++;
							num5 = i + 1;
						}
					}
				}
				num6 = (int)(control.runinfo.mutliInput40 & 16777215UL);
				if (num6 != 0 && (num6 & 8388608) > 0)
				{
					for (int j = 0; j < 20; j++)
					{
						if ((num6 & (1 << j)) > 0)
						{
							num4++;
						}
						else
						{
							text = text + (20 + j + 1).ToString() + ",";
							num3++;
							num5 = 20 + j + 1;
						}
					}
				}
				this.lblMultiInputInfo4Connected.Text = text;
				if (num3 == 1)
				{
					if (this.lastOpenNOIndex != num5)
					{
						this.lastOpenNOIndex = num5;
						if (this.checkBox2.Checked)
						{
							this.openNO2018(num5);
						}
					}
				}
				else
				{
					this.lastOpenNOIndex = 0;
				}
			}
			if (control.ControllerDriverMainVer.ToString() != this.label7.Text)
			{
				this.label7.Text = control.runinfo.driverVersion;
			}
			this.btnConnected.Visible = !this.btnConnected.Visible;
			if (this.lastControllerInfo != control.runinfo.CurrentControllerSN.ToString() + control.runinfo.reservedBytes[0].ToString())
			{
				this.lastControllerInfo = control.runinfo.CurrentControllerSN.ToString() + control.runinfo.reservedBytes[0].ToString();
				string text2 = "";
				if (control.runinfo.reservedBytes[0] == 0)
				{
					text2 += string.Format("管脚没问题\t", new object[0]);
				}
				else
				{
					text2 = text2 + string.Format("failedPin 问题管脚号: {0}\r\n\t", control.runinfo.reservedBytes[0]) + icDesc.failedPinDesc((int)control.runinfo.reservedBytes[0]);
					if ((control.runinfo.reservedBytes[1] & 240) == 0)
					{
						text2 += string.Format("\tfailedPinDesc 问题管脚PORT号: G{0:X}\r\n", control.runinfo.reservedBytes[1]);
					}
					else
					{
						text2 += string.Format("\tfailedPinDesc 问题管脚PORT号: {0:X2}\r\n", control.runinfo.reservedBytes[1]);
					}
					text2 += string.Format("\tfailedPinDiffPortType 问题管脚PORT类: {0:X2}\r\n", control.runinfo.reservedBytes[2]);
					string text3 = "";
					switch (control.runinfo.reservedBytes[2] >> 4)
					{
					case 1:
						text3 = "初始默认就有问题";
						break;
					case 2:
						text3 = "管脚高平设置时 就有问题";
						break;
					case 3:
						text3 = "管脚高平设置时 此脚 就有问题";
						break;
					case 4:
						text3 = "管脚低平设置时 就有问题";
						break;
					case 5:
						text3 = "管脚低平设置时 此脚 就有问题";
						break;
					}
					if ((control.runinfo.reservedBytes[2] & 15) == 0)
					{
						text2 += string.Format("\t产生问题的另一端口PORT= PORTG\r\n", new object[0]);
					}
					else
					{
						text2 += string.Format("\t产生问题的另一端口PORT: PORT{0:X}\r\n", (int)(control.runinfo.reservedBytes[2] & 15));
					}
					if (text3 != "")
					{
						text2 = text2 + text3 + "\r\n";
					}
					text2 += string.Format("\tfailedPinDiff 存在不同: {0:X2}\r\n", control.runinfo.reservedBytes[3]);
				}
				frmProductFormat.wgLogProduct(this.lastControllerInfo + ":" + text2 + string.Format("\t所有数据: {0}", control.runinfo.BytesDataStr), "n3k_Product");
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0021AEAB File Offset: 0x00219EAB
		private void frmProductFormat_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0021AEB0 File Offset: 0x00219EB0
		private void frmProductFormat_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				this.btnEditCode_Click(null, null);
			}
			if ((e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 65 && e.KeyValue <= 90))
			{
				if (this.inputCard.Length == 0)
				{
					this.timer2.Interval = 500;
					this.timer2.Enabled = true;
				}
				if (e.KeyValue >= 48 && e.KeyValue <= 57)
				{
					this.inputCard += (e.KeyValue - 48).ToString();
					return;
				}
				this.inputCard += e.KeyCode.ToString();
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0021AFEC File Offset: 0x00219FEC
		private void frmProductFormat_Load(object sender, EventArgs e)
		{
			this.Text = this.Text + " V" + Application.ProductVersion;
			string text = "中性";
			if (wgTools.gWGYTJ)
			{
				text = "AGYTJ";
			}
			string productTypeOfApp = wgAppConfig.ProductTypeOfApp;
			if (productTypeOfApp != null)
			{
				if (!(productTypeOfApp == "CGACCESS"))
				{
					if (productTypeOfApp == "AGACCESS")
					{
						text = "ACCESS";
					}
					else if (productTypeOfApp == "XGACCESS")
					{
						text = "XG";
					}
				}
				else
				{
					text = "CG";
				}
			}
			this.Text += string.Format(" -{0}-", text);
			this.btnConnected.Text = text;
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO")))
			{
				this.productTypeInfo = wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO").Split(new char[] { ',' });
			}
			this.panel1.Visible = false;
			if (!this.backgroundWorker1.IsBusy)
			{
				this.backgroundWorker1.RunWorkerAsync();
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0021B0EC File Offset: 0x0021A0EC
		private void openNO2018(int floorNO)
		{
			Cursor.Current = Cursors.WaitCursor;
			icController icController = new icController();
			try
			{
				try
				{
					this.bStopRemoteEvalator = false;
					uint num = 0U;
					long num2 = -1L;
					icController.ControllerSN = int.Parse(this.txtSN.Text);
					int num3 = 0;
					int num4;
					if (floorNO > 20)
					{
						num3 = 20;
						num4 = floorNO - num3;
					}
					else
					{
						num4 = floorNO - num3;
					}
					int.Parse(this.numericUpDown22.Value.ToString());
					if (!this.bStopRemoteEvalator)
					{
						icController.RemoteOpenFoorIP(num4 + num3, num, num2);
						this.lblFloor.Text = (num4 + num3).ToString();
						Application.DoEvents();
					}
				}
				catch (Exception)
				{
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				XMessageBox.Show(ex.ToString());
			}
			finally
			{
				icController.Dispose();
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0021B1F0 File Offset: 0x0021A1F0
		private void pingErrLogEntry(string ErrInfo)
		{
			if (string.IsNullOrEmpty(ErrInfo))
			{
				this.lblCommLose.Visible = false;
				return;
			}
			this.btnConnected.BackColor = Color.Yellow;
			this.lblCommLose.Visible = true;
			this.txtRunInfo.AppendText(ErrInfo);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0021B230 File Offset: 0x0021A230
		private void test100LosePacketToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				try
				{
					icController.ControllerSN = -1;
					icController.runinfo.Clear();
					icController.GetControllerRunInformationIPNoTries();
					if (icController.runinfo.wgcticks > 0U)
					{
						icController.ControllerSN = (int)icController.runinfo.CurrentControllerSN;
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						for (int i = 0; i < 20000; i++)
						{
							num++;
							if (icController.SpecialPingIP() == 1)
							{
								num2++;
							}
							else
							{
								num3++;
								if (i >= 200)
								{
									break;
								}
							}
						}
						if (num != num2)
						{
							string text = DateTime.Now.ToString("HH:mm:ss ") + string.Format("SN{3} 有故障: 通信丢包\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, icController.ControllerSN }) + "\r\n";
							base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text });
						}
						else
						{
							wgUdpComm.triesTotal = 0L;
							wgTools.WriteLine("control.Test1024 Start");
							int num4 = 0;
							string text2 = "";
							if (icController.test1024Write() < 0)
							{
								text2 += "大数据包写入失败\r\n";
							}
							int num5 = icController.test1024Read(100U, ref num4);
							if (num5 < 0)
							{
								text2 = text2 + "大数据包读取失败: " + num5.ToString() + "\r\n";
							}
							if (wgUdpComm.triesTotal > 0L)
							{
								text2 = text2 + "测试中重试次数 = " + wgUdpComm.triesTotal.ToString() + "\r\n";
							}
							if (text2 != "")
							{
								string text3 = string.Concat(new string[]
								{
									DateTime.Now.ToString("HH:mm:ss "),
									"SN",
									icController.ControllerSN.ToString(),
									"通信有故障: : ",
									text2,
									"\r\n"
								});
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text3 });
							}
							else
							{
								string text4 = DateTime.Now.ToString("HH:mm:ss ") + string.Format("SN{3} 通信正常\r\n 已发送={0}, 已接收={1}, 丢失 = {2}", new object[] { num, num2, num3, icController.ControllerSN }) + "\r\n";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text2 = "大数据包测试成功(测试100次)";
								text4 = icController.ControllerSN.ToString() + ": " + text2 + "\r\n";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								text4 = "";
								base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text4 });
								this.lastErrController = 0L;
							}
						}
					}
					else
					{
						string text5 = "通信不上";
						base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text5 });
						text5 = "";
						base.Invoke(new frmProductFormat.pingErrLog(this.pingErrLogEntry), new object[] { text5 });
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0021B678 File Offset: 0x0021A678
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			this.lblTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			this.timer1.Enabled = true;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0021B6BA File Offset: 0x0021A6BA
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timer2.Enabled = false;
			if (this.inputCard.Length == "KT0L01*******".Length)
			{
				this.wgLogProduct4BasicInfoCheck(this.inputCard);
			}
			this.inputCard = "";
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0021B6F8 File Offset: 0x0021A6F8
		private bool validSN(int sn, int typecode)
		{
			if (sn >= 100000000 && sn <= 500000000)
			{
				if (sn >= 300000000 && sn < 400000000)
				{
					return false;
				}
				if (sn >= 171100001 && sn < 171100099 && typecode == 3)
				{
					return true;
				}
				if (sn >= 160100000 && sn < 160999999 && typecode == 3)
				{
					return true;
				}
				int num = sn / 10000000 % 10;
				int num2 = sn / 1000000 % 100;
				bool flag = true;
				switch (typecode)
				{
				case 1:
					return (num == 1 || (num2 == 71 && sn >= 171200001)) && flag;
				case 2:
					if (num != 2)
					{
						flag = false;
					}
					return flag;
				case 3:
					if (sn < 171100001 || sn >= 171100099)
					{
						if (sn >= 160100000 && sn < 160999999)
						{
							return flag;
						}
						if (num != 3 && num2 != 73)
						{
							flag = false;
						}
					}
					return flag;
				case 5:
					if (num != 5 && num2 != 75)
					{
						flag = false;
					}
					return flag;
				}
			}
			return false;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0021B7F0 File Offset: 0x0021A7F0
		public static void wgLogProduct(string strMsg, string filename)
		{
			try
			{
				string text = string.Concat(new object[]
				{
					icOperator.OperatorID,
					".",
					icOperator.OperatorName,
					".",
					strMsg
				});
				text = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "\t" + text;
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + filename + ".log", true))
				{
					streamWriter.WriteLine(text);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0021B8A4 File Offset: 0x0021A8A4
		private void wgLogProduct4BasicInfo(icController control)
		{
			string text = "n3k_product_format";
			try
			{
				string text2 = string.Format("\r\n{0}\t{1}\t{2}\t", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), control.ControllerSN.ToString(), control.runinfo.driverVersion);
				this.txtRunInfo.AppendText(string.Concat(new string[]
				{
					"\r\n",
					this.logID.ToString(),
					".格式化: ",
					control.ControllerSN.ToString(),
					"\t",
					control.runinfo.driverVersion
				}));
				frmProductFormat.lastControllerSN = control.ControllerSN;
				frmProductFormat.lastControllerMac = control.MACAddr;
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + text + ".log", true))
				{
					streamWriter.Write(text2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0021B9BC File Offset: 0x0021A9BC
		private void wgLogProduct4BasicInfoCheck(string printCode)
		{
			string text = "n3k_product_format";
			this.txtRunInfo.AppendText("\t" + this.inputCard);
			try
			{
				if (frmProductFormat.lastControllerSN == 0)
				{
					this.txtRunInfo.AppendText("\r\n");
				}
				else
				{
					string text2 = "没有找到此序列号对应的产品型号....";
					string text3 = "没有找到此序列号对应的产品型号....";
					bool flag = false;
					if (this.formatedPrintCode.Count > 0)
					{
						int num = this.formatedPrintCode.IndexOf(printCode);
						if (num >= 0)
						{
							text2 = string.Format("条码 {0}\r\n\r\n已被SN: {1}使用了.", printCode, this.formatedSN[num].ToString());
							XMessageBox.Show(this, text2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
					}
					if (this.formatedSN.Count > 0)
					{
						int num2 = this.formatedSN.IndexOf(frmProductFormat.lastControllerSN);
						if (num2 >= 0)
						{
							text2 = string.Format("控制器SN: {0}\r\n\r\n使用了条码 {1}\r\n\r\n不能再使用条码 {2}.", this.formatedSN[num2].ToString(), this.formatedPrintCode[num2].ToString(), printCode);
							XMessageBox.Show(this, text2, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
					}
					int num3 = 0;
					while (num3 < this.productTypeInfo.Length && !string.IsNullOrEmpty(this.productTypeInfo[num3 + this.PRODUCT_SN]))
					{
						if (this.compareProductInfo(this.productTypeInfo[num3 + this.PRODUCT_SN], frmProductFormat.lastControllerSN.ToString()))
						{
							text3 = string.Concat(new string[]
							{
								"产品类别: ",
								this.productTypeInfo[num3 + this.PRODUCT_BRAND],
								"\r\n\r\n型号: ",
								this.productTypeInfo[num3 + this.PRODUCT_TYPE],
								"\r\n\r\n序列号: ",
								frmProductFormat.lastControllerSN.ToString(),
								"\r\n\r\nMAC: ",
								frmProductFormat.lastControllerMac.ToString(),
								"\r\n\r\n\r\n[要求模块芯片]: ",
								this.productTypeInfo[num3 + this.PRODUCT_MODEL]
							});
							if (this.compareProductInfo("00-69-", frmProductFormat.lastControllerMac.ToString()))
							{
								text3 += "\r\n\r\n实际模块芯片: 6911";
							}
							else if (this.compareProductInfo("00-66-", frmProductFormat.lastControllerMac.ToString()))
							{
								text3 += "\r\n\r\n实际模块芯片: 1766";
							}
							else
							{
								text3 += "\r\n\r\n实际模块芯片: (未知)";
							}
							text3 = string.Concat(new string[]
							{
								text3,
								"\r\n\r\n[要求条码]: ",
								this.productTypeInfo[num3 + this.PRODUCT_CODE],
								"\r\n\r\n实际条码: ",
								printCode
							});
							if (!this.compareProductInfo(this.productTypeInfo[num3 + this.PRODUCT_MAC], frmProductFormat.lastControllerMac.ToString()))
							{
								text2 = "模块芯片 不匹配";
								text3 = text3 + "\r\n\r\n\r\n问题: " + text2;
							}
							else if (!this.compareProductInfo(this.productTypeInfo[num3 + this.PRODUCT_CODE], printCode.ToString()))
							{
								text2 = "条码  不匹配";
								text3 = text3 + "\r\n\r\n\r\n问题: " + text2;
							}
							else
							{
								flag = true;
							}
						}
						num3 += this.INFO_UNIT_LENGTH;
					}
					string text4;
					if (flag)
					{
						this.formatedPrintCode.Add(printCode);
						this.formatedSN.Add(frmProductFormat.lastControllerSN);
						text4 = printCode + "\tOK\t";
						this.txtRunInfo.AppendText("\tOK");
					}
					else
					{
						text4 = printCode + "\t有问题:" + text2 + "\t";
						this.txtRunInfo.AppendText("\t有问题");
					}
					using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + text + ".log", true))
					{
						streamWriter.Write(text4);
					}
					if (!flag)
					{
						XMessageBox.Show(this, text3, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0400346A RID: 13418
		private bool bStopRemoteEvalator;

		// Token: 0x0400346B RID: 13419
		private long lastErrController;

		// Token: 0x0400346C RID: 13420
		private int lastFormatController;

		// Token: 0x0400346D RID: 13421
		private int lastOpenNOIndex;

		// Token: 0x0400346E RID: 13422
		private int logID;

		// Token: 0x0400346F RID: 13423
		private int PRODUCT_BRAND;

		// Token: 0x04003470 RID: 13424
		private ArrayList formatedPrintCode = new ArrayList();

		// Token: 0x04003471 RID: 13425
		private ArrayList formatedSN = new ArrayList();

		// Token: 0x04003472 RID: 13426
		private int INFO_UNIT_LENGTH = 6;

		// Token: 0x04003473 RID: 13427
		private string inputCard = "";

		// Token: 0x04003474 RID: 13428
		private string lastControllerInfo = "";

		// Token: 0x04003475 RID: 13429
		private static string lastControllerMac = "";

		// Token: 0x04003476 RID: 13430
		private static int lastControllerSN = 0;

		// Token: 0x04003477 RID: 13431
		private int PRODUCT_CODE = 2;

		// Token: 0x04003478 RID: 13432
		private int PRODUCT_MAC = 5;

		// Token: 0x04003479 RID: 13433
		private int PRODUCT_MODEL = 4;

		// Token: 0x0400347A RID: 13434
		private int PRODUCT_SN = 3;

		// Token: 0x0400347B RID: 13435
		private int PRODUCT_TYPE = 1;

		// Token: 0x0400347C RID: 13436
		private string[] productTypeInfo = new string[]
		{
			"中性蓝板", "L01", "KT0L01*******", "122******", "6911", "00-69-********", "中性蓝板", "L01", "KT0L01*******", "123******",
			"1766", "00-66-********", "中性蓝板", "L02", "KT0L02*******", "222******", "6911", "00-69-********", "中性蓝板", "L02",
			"KT0L02*******", "223******", "1766", "00-66-********", "中性蓝板", "L04", "KT0L04*******", "422******", "6911", "00-69-********",
			"中性蓝板", "L04", "KT0L04*******", "423******", "1766", "00-66-********", "Adroitor金板", "AT8001", "KT8001*******", "131******",
			"6911", "00-69-********", "Adroitor金板", "AT8001", "KT8001*******", "133******", "1766", "00-66-********", "Adroitor金板", "AT8002",
			"KT8002*******", "231******", "6911", "00-69-********", "Adroitor金板", "AT8002", "KT8002*******", "233******", "1766", "00-66-********",
			"Adroitor金板", "AT8004", "KT8004*******", "431******", "6911", "00-69-********", "Adroitor金板", "AT8004", "KT8004*******", "433******",
			"1766", "00-66-********", "品牌绿板", "WG2001.NET-12", "KW2101*******", "151******", "6911", "00-69-********", "品牌绿板", "WG2001.NET-12",
			"KW2101*******", "153******", "1766", "00-66-********", "品牌绿板", "WG2002.NET-12", "KW2102*******", "251******", "6911", "00-69-********",
			"品牌绿板", "WG2002.NET-12", "KW2102*******", "253******", "1766", "00-66-********", "品牌绿板", "WG2004.NET-12", "KW2104*******", "451******",
			"6911", "00-69-********", "品牌绿板", "WG2004.NET-12", "KW2104*******", "453******", "1766", "00-66-********", "ADCT黑板", "ADCT3000-1",
			"KA3001*******", "111******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-1", "KA3001*******", "113******", "1766", "00-66-********",
			"ADCT黑板", "ADCT3000-2", "KA3002*******", "211******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-2", "KA3002*******", "213******",
			"1766", "00-66-********", "ADCT黑板", "ADCT3000-4", "KA3004*******", "411******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-4",
			"KA3004*******", "413******", "1766", "00-66-********", "梯控绿板", "DT20M", "KTDT20*******", "171******", "6911", "00-69-********",
			"梯控绿板", "DT20M", "KTDT20*******", "173******", "1766", "00-66-********", "", "", "", "",
			"", "", "", "", "", "", "", ""
		};

		// Token: 0x0200032E RID: 814
		// (Invoke) Token: 0x060019C9 RID: 6601
		private delegate void autoFormatLog(icController control);

		// Token: 0x0200032F RID: 815
		// (Invoke) Token: 0x060019CD RID: 6605
		private delegate void dispDoorStatusByIPComm(icController control);

		// Token: 0x02000330 RID: 816
		// (Invoke) Token: 0x060019D1 RID: 6609
		private delegate void pingErrLog(string ErrInfo);
	}
}
