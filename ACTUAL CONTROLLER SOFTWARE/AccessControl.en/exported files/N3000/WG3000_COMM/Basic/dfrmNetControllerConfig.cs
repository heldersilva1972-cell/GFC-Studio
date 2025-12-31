using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.Cloud2017;
using WG3000_COMM.ExtendFunc.Reader;
using WG3000_COMM.ExtendFunc.SMS;
using WG3000_COMM.ExtendFunc.WIFI2019;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WG3000_COMM.Security;
using WG3000_COMM.wgWS.app;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200001A RID: 26
	public partial class dfrmNetControllerConfig : frmN3000
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00029A94 File Offset: 0x00028A94
		public dfrmNetControllerConfig()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvFoundControllers);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00029BA4 File Offset: 0x00028BA4
		private void activeElevatorAsSwitchV899AboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
				if (!wgMjController.IsElevator(int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString())))
				{
					XMessageBox.Show("请选择 梯控设备进行设置.");
					return;
				}
			}
			string text = "";
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = "请输入相关的电梯控制器SN";
				dfrmInputNewName.label1.Text = "SN(用逗号分开)";
				dfrmInputNewName.strNewName = "";
				if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_ELEVATOR_SWITCH_VALIDSN"))))
				{
					dfrmInputNewName.strNewName = wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_ELEVATOR_SWITCH_VALIDSN"));
				}
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
				text = text.Replace("，", ",");
				text = text.Replace(" ", "");
				if (text.Length < 9)
				{
					return;
				}
			}
			string[] array = text.Split(new char[] { ',' });
			string text2 = "";
			int num = 0;
			string text3 = "";
			int num2 = 0;
			Cursor.Current = Cursors.WaitCursor;
			if (array.Length <= 16)
			{
				for (int j = 0; j < array.Length; j++)
				{
					int num3 = 0;
					if (!int.TryParse(array[j], out num3) || !wgMjController.IsElevator(num3))
					{
						XMessageBox.Show("输入的SN无效. 请输入有效的电梯控制器SN");
						return;
					}
					for (int k = 0; k < this.dgvFoundControllers.SelectedRows.Count; k++)
					{
						DataGridViewRow dataGridViewRow2 = this.dgvFoundControllers.SelectedRows[k];
						int num4 = int.Parse(dataGridViewRow2.Cells["f_ControllerSN"].Value.ToString());
						if (num3 == num4)
						{
							XMessageBox.Show("请输入被控制的电梯控制器SN, 不要包括作为交换器的设备SN: " + num4.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
					}
				}
				wgAppConfig.UpdateKeyVal("KEY_ELEVATOR_SWITCH_VALIDSN", text);
				using (icController icController = new icController())
				{
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					wgMjControllerConfigure.rs232_1_option = 130;
					wgMjControllerConfigure.rs232_1_extern = 32;
					int num5 = 0;
					for (int l = 0; l < this.dgvFoundControllers.SelectedRows.Count; l++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow3 = this.dgvFoundControllers.SelectedRows[l];
						icController.ControllerSN = int.Parse(dataGridViewRow3.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow3.Cells["f_PCIPAddr"].Value.ToString();
						num5++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num5.ToString();
						this.toolStripStatusLabel1.Text = num5.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num++;
						}
						else
						{
							text3 = text3 + icController.ControllerSN.ToString() + ",";
							num2++;
						}
						Application.DoEvents();
						WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
						wgpacketSSI_FLASH.type = 33;
						wgpacketSSI_FLASH.code = 48;
						wgpacketSSI_FLASH.iDevSnFrom = 0U;
						wgpacketSSI_FLASH.iDevSnTo = (uint)icController.ControllerSN;
						wgpacketSSI_FLASH.iCallReturn = 0;
						wgpacketSSI_FLASH.ucData = new byte[1024];
						wgUdpComm wgUdpComm = new wgUdpComm();
						string text4 = "";
						int num6 = 60000;
						byte[] array2 = null;
						Thread.Sleep(300);
						wgpacketSSI_FLASH.iStartFlashAddr = 5001216U;
						wgpacketSSI_FLASH.iEndFlashAddr = 5907U;
						for (int m = 0; m < 1024; m++)
						{
							wgpacketSSI_FLASH.ucData[m] = byte.MaxValue;
						}
						for (int n = 0; n < array.Length; n++)
						{
							int num7 = 0;
							int.TryParse(array[n], out num7);
							wgpacketSSI_FLASH.ucData[352 + n * 4] = (byte)(num7 & 255);
							wgpacketSSI_FLASH.ucData[353 + n * 4] = (byte)((num7 >> 8) & 255);
							wgpacketSSI_FLASH.ucData[354 + n * 4] = (byte)((num7 >> 16) & 255);
							wgpacketSSI_FLASH.ucData[355 + n * 4] = (byte)((num7 >> 24) & 255);
						}
						if (wgUdpComm.udp_get(wgpacketSSI_FLASH.ToBytes(wgUdpComm.udpPort), 300, wgpacketSSI_FLASH.xid, text4, num6, ref array2) > 0)
						{
							XMessageBox.Show("OK");
						}
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text3))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						"",
						CommonStr.strFailed,
						num2,
						text3
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num2,
						text3
					}));
				}
				if (!string.IsNullOrEmpty(text2))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						"",
						CommonStr.strSuccessfully,
						num,
						text2
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num,
						text2
					}));
					return;
				}
			}
			else
			{
				XMessageBox.Show("只允许最多输入16个. 请重新输入");
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0002A24C File Offset: 0x0002924C
		public void AddDiscoveryEntry(object o, object pcIP, object pcMac, object bPassword)
		{
			byte[] array = (byte[])o;
			string text = (string)pcIP;
			string text2 = (string)pcMac;
			bool flag = (string)bPassword == "1";
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			long num = (long)dfrmNetControllerConfig.getSNSearch(intPtr);
			Marshal.FreeHGlobal(intPtr);
			bool flag2 = false;
			if (num < -2L)
			{
				num = -num;
				flag2 = true;
			}
			if (wgTools.gWGYTJ && !wgMjController.validSN((int)num))
			{
				if (wgMjController.GetControllerType((int)num) >= 2)
				{
					this.invalidSNofWGYTJ = (int)num;
					return;
				}
			}
			else if (!wgMjController.IsFingerController((int)num))
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array, 20);
				string text3;
				if (wgMjControllerConfigure.webPort == 0 || wgMjControllerConfigure.webPort == 65535)
				{
					text3 = string.Format("{0},{1}", wgMjControllerConfigure.webDeviceName, CommonStr.strWEBDisabled);
				}
				else
				{
					text3 = string.Format("{0},{1},{2},{3}", new object[]
					{
						wgMjControllerConfigure.webDeviceName,
						wgMjControllerConfigure.webLanguage,
						(wgAppConfig.CultureInfoStr == "zh-CHS") ? wgMjControllerConfigure.webDateDisplayFormatCHS : wgMjControllerConfigure.webDateDisplayFormat,
						wgMjControllerConfigure.webPort.ToString()
					});
				}
				if (string.IsNullOrEmpty(this.foundControllerIPs) || this.foundControllerIPs.IndexOf(text + ",") < 0)
				{
					this.foundControllerIPs = this.foundControllerIPs + text + ",";
					wgAppConfig.UpdateKeyVal("KEY_FoundControllerIPs", this.foundControllerIPs);
				}
				int num2 = this.dgvFoundControllers.Rows.Count + 1;
				text3 = string.Format("#{0}" + (flag ? "P" : ""), num2.ToString().PadLeft(2, '0')) + text3;
				string[] array2 = new string[]
				{
					(this.dgvFoundControllers.Rows.Count + 1).ToString().PadLeft(4, '0'),
					wgMjControllerConfigure.controllerSN.ToString(),
					wgMjControllerConfigure.ip.ToString(),
					wgMjControllerConfigure.mask.ToString(),
					wgMjControllerConfigure.gateway.ToString(),
					wgMjControllerConfigure.port.ToString(),
					wgMjControllerConfigure.MACAddr,
					text,
					text3,
					text2
				};
				if (flag2 && (wgMjControllerConfigure.http_onlyquery & 24) != 16)
				{
					wgAppConfig.wgLog(".errSN_" + num.ToString() + "____" + BitConverter.ToString(array).Replace("-", ""));
					return;
				}
				if ((wgMjControllerConfigure.http_onlyquery & 24) == 16)
				{
					if (wgUdpComm.CommTimeoutMsMin < 1000L)
					{
						wgAppConfig.UpdateKeyVal("CommTimeoutMsMin", "1000");
						long.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommTimeoutMsMin")), out wgUdpComm.CommTimeoutMsMin);
					}
					if (!wgUdpComm.bWIFI)
					{
						wgUdpComm.bWIFI = true;
						wgAppConfig.UpdateKeyVal("WIFI_ACTIVE", "1");
					}
				}
				if (this.dgvFoundControllers.Rows.Count > 0)
				{
					for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
					{
						if (this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString() == array2[1] && this.dgvFoundControllers.Rows[i].Cells[7].Value.ToString() == array2[7])
						{
							return;
						}
					}
				}
				for (int j = 0; j < array2.Length; j++)
				{
					this.strControllers = this.strControllers + array2[j] + ",";
				}
				if (flag)
				{
					this.EncryptControllerSN = this.EncryptControllerSN + wgMjControllerConfigure.controllerSN.ToString() + ",";
				}
				this.dgvFoundControllers.Rows.Add(array2);
				if (this.dgvFoundControllers.Rows.Count == 1)
				{
					IPAddress ipaddress;
					IPAddress.TryParse(text, out ipaddress);
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0002A698 File Offset: 0x00029698
		private void addSelectedToSystemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				string text = "";
				int num = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					int num2 = int.Parse(this.dgvFoundControllers.SelectedRows[i].Cells[1].Value.ToString());
					if (num2 != -1)
					{
						if (wgMjController.GetControllerType(num2) == 4 && icConsumer.gTimeSecondEnabled)
						{
							XMessageBox.Show(num2.ToString() + "\r\n" + CommonStr.strTimeSecondAddControllerWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (!icController.IsExisted2SN(num2, 0))
						{
							text = text + num2.ToString() + ",";
							num++;
							this.lblSearchNow.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
							this.toolStripStatusLabel2.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
							using (dfrmController dfrmController = new dfrmController())
							{
								dfrmController.OperateNew = true;
								dfrmController.WindowState = FormWindowState.Minimized;
								dfrmController.Show();
								dfrmController.mtxtbControllerSN.Text = num2.ToString();
								dfrmController.btnNext.PerformClick();
								dfrmController.btnOK.PerformClick();
								Application.DoEvents();
							}
						}
					}
				}
				Cursor.Current = Cursors.Default;
				XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddController, num, text), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0002A898 File Offset: 0x00029898
		private void autoSetIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			icController icController = new icController();
			bool flag = sender == this.autoSetIPToolStripMenuItem;
			try
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				ArrayList arrayList = new ArrayList();
				using (dfrmTCPIPConfigure dfrmTCPIPConfigure = new dfrmTCPIPConfigure())
				{
					dfrmTCPIPConfigure.Text = CommonStr.strIPAddressStart;
					if (dfrmTCPIPConfigure.ShowDialog(this) == DialogResult.OK)
					{
						text = dfrmTCPIPConfigure.strIP;
						text2 = dfrmTCPIPConfigure.strMask;
						text3 = dfrmTCPIPConfigure.strGateway;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					IPAddress ipaddress = IPAddress.Parse(text);
					if (this.dgvFoundControllers.SelectedRows.Count > 0)
					{
						Cursor.Current = Cursors.WaitCursor;
						ArrayList arrayList2 = new ArrayList();
						ArrayList arrayList3 = new ArrayList();
						byte[] array = new byte[4];
						array = ipaddress.GetAddressBytes();
						for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
						{
							int num = int.Parse(this.dgvFoundControllers.SelectedRows[i].Cells[1].Value.ToString());
							if (num != -1)
							{
								this.toolStripStatusLabel2.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + num.ToString();
								Application.DoEvents();
								arrayList3.Add(num);
								if (flag)
								{
									string text4 = this.dgvFoundControllers.SelectedRows[i].Cells["f_IP"].Value.ToString();
									string text5;
									if ((text5 = text4) != null && (text5 == "192.0.0.0" || text5 == "192.168.0.0" || text5 == "192.168.168.0"))
									{
										text4 = "";
									}
									icController.ControllerSN = num;
									string text6 = text4;
									if (((string.IsNullOrEmpty(text3) && this.dgvFoundControllers.SelectedRows[i].Cells["f_gateway"].Value.ToString() == "0.0.0.0") || text3 == this.dgvFoundControllers.SelectedRows[i].Cells["f_gateway"].Value.ToString()) && text6 != "192.168.0.0" && text6 != "192.168.168.0" && text6 != "255.255.255.255" && text6 != "")
									{
										icController.IP = text6;
										if (icController.GetControllerRunInformationIP(-1) > 0)
										{
											arrayList.Add(text6.ToString());
											arrayList2.Add(text6.ToString());
										}
										icController.IP = "";
									}
								}
							}
						}
						for (int j = 0; j < this.dgvFoundControllers.Rows.Count; j++)
						{
							DataGridViewRow dataGridViewRow = this.dgvFoundControllers.Rows[j];
							int num = int.Parse(dataGridViewRow.Cells[1].Value.ToString());
							if (arrayList3.IndexOf(num) >= 0)
							{
								this.toolStripStatusLabel2.Text = this.dgvFoundControllers.Rows[j].Cells[0].Value.ToString() + "-" + num.ToString();
								Application.DoEvents();
								string text7 = dataGridViewRow.Cells["f_IP"].Value.ToString();
								string text8;
								if ((text8 = text7) != null && (text8 == "192.0.0.0" || text8 == "192.168.0.0" || text8 == "192.168.168.0"))
								{
									text7 = "";
								}
								icController.ControllerSN = num;
								string text9 = text7;
								if (arrayList2.IndexOf(text9) < 0)
								{
									byte[] array2 = new byte[4];
									array2 = ipaddress.GetAddressBytes();
									int num2 = -1;
									byte[] array3 = new byte[6];
									uint num3 = (uint)array3.Length;
									string text10 = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
									if (arrayList.IndexOf(ipaddress.ToString()) < 0)
									{
										if (flag)
										{
											num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text10)), array3, ref num3);
										}
									}
									else
									{
										num2 = 0;
									}
									if (num2 == 0)
									{
										array2[3] = array2[3] + 1;
										while (array2[3] != array[3])
										{
											if (array2[3] == 0 || array2[3] == 255)
											{
												array2[3] = array2[3] + 1;
											}
											else
											{
												ipaddress = new IPAddress(array2);
												num3 = (uint)array3.Length;
												if (arrayList.IndexOf(ipaddress.ToString()) < 0)
												{
													if (flag)
													{
														num2 = wgGlobal.SafeNativeMethods.SendARP(this.IPLng(ipaddress), this.IPLng(IPAddress.Parse(text10)), array3, ref num3);
													}
													else
													{
														num2 = -1;
													}
													if (num2 != 0)
													{
														break;
													}
												}
												array2[3] = array2[3] + 1;
											}
										}
									}
									if (num2 != 0)
									{
										arrayList.Add(ipaddress.ToString());
										byte[] array4 = new byte[1152];
										for (int k = 0; k < array4.Length; k++)
										{
											array4[k] = 0;
										}
										icController.NetIPConfigure(icController.ControllerSN.ToString(), dataGridViewRow.Cells["f_MACAddr"].Value.ToString(), ipaddress.ToString(), text2, text3, 60000.ToString(), text10);
									}
								}
							}
						}
					}
					Cursor.Current = Cursors.Default;
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
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0002AEC4 File Offset: 0x00029EC4
		private void batchUpdateSelectedIPInDBToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					int num = int.Parse(this.dgvFoundControllers.SelectedRows[i].Cells[1].Value.ToString());
					if (num != -1)
					{
						string text = num.ToString();
						string text2 = this.dgvFoundControllers.SelectedRows[i].Cells["f_IP"].Value.ToString();
						string text3;
						if ((text3 = text2) != null && (text3 == "192.0.0.0" || text3 == "192.168.0.0" || text3 == "192.168.168.0"))
						{
							text2 = "";
						}
						wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_Controller SET  f_IP = {0} WHERE  f_ControllerSN IN ({1}) ", wgTools.PrepareStrNUnicode(text2), text));
					}
				}
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0002AFD8 File Offset: 0x00029FD8
		private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				using (dfrmControllerZoneSelect dfrmControllerZoneSelect = new dfrmControllerZoneSelect())
				{
					if (dfrmControllerZoneSelect.ShowDialog(this) == DialogResult.OK)
					{
						string text = "";
						for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
						{
							int num = int.Parse(this.dgvFoundControllers.SelectedRows[i].Cells[1].Value.ToString());
							if (num != -1)
							{
								if (!string.IsNullOrEmpty(text))
								{
									text += ",";
								}
								text += num.ToString();
								wgAppConfig.runUpdateSql(string.Format(" UPDATE t_b_Controller SET f_ZoneID= {0} WHERE  f_ControllerSN IN ({1}) ", dfrmControllerZoneSelect.selectZoneId, text));
							}
						}
					}
				}
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0002B0D0 File Offset: 0x0002A0D0
		private void btnAddToSystem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				string text = "";
				int num = 0;
				for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
				{
					int num2 = int.Parse(this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString());
					if (num2 != -1)
					{
						if (!wgMjController.validSN(num2))
						{
							XMessageBox.Show(this, num2.ToString() + "\r\n" + CommonStr.strSNWrong, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (wgMjController.GetControllerType(num2) == 4 && icConsumer.gTimeSecondEnabled)
						{
							XMessageBox.Show(num2.ToString() + "\r\n" + CommonStr.strTimeSecondAddControllerWarn, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (!icController.IsExisted2SN(num2, 0))
						{
							text = text + num2.ToString() + ",";
							num++;
							this.lblSearchNow.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
							this.toolStripStatusLabel2.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
							using (dfrmController dfrmController = new dfrmController())
							{
								dfrmController.OperateNew = true;
								dfrmController.WindowState = FormWindowState.Minimized;
								dfrmController.Show();
								dfrmController.mtxtbControllerSN.Text = num2.ToString();
								dfrmController.btnNext.PerformClick();
								dfrmController.btnOK.PerformClick();
								Application.DoEvents();
							}
						}
					}
				}
				Cursor.Current = Cursors.Default;
				XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddController, num, text), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0002B300 File Offset: 0x0002A300
		private void btnConfigure_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (dfrmTCPIPConfigure dfrmTCPIPConfigure = new dfrmTCPIPConfigure())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				dfrmTCPIPConfigure.strSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
				dfrmTCPIPConfigure.strMac = dataGridViewRow.Cells["f_MACAddr"].Value.ToString();
				dfrmTCPIPConfigure.strIP = dataGridViewRow.Cells["f_IP"].Value.ToString();
				dfrmTCPIPConfigure.strMask = dataGridViewRow.Cells["f_Mask"].Value.ToString();
				dfrmTCPIPConfigure.strGateway = dataGridViewRow.Cells["f_Gateway"].Value.ToString();
				dfrmTCPIPConfigure.strTCPPort = dataGridViewRow.Cells["f_PORT"].Value.ToString();
				string text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				if (dfrmTCPIPConfigure.ShowDialog(this) == DialogResult.OK)
				{
					string strSN = dfrmTCPIPConfigure.strSN;
					string strMac = dfrmTCPIPConfigure.strMac;
					string strIP = dfrmTCPIPConfigure.strIP;
					string strMask = dfrmTCPIPConfigure.strMask;
					string strGateway = dfrmTCPIPConfigure.strGateway;
					string strTCPPort = dfrmTCPIPConfigure.strTCPPort;
					string text2 = dfrmTCPIPConfigure.Text;
					this.Refresh();
					Cursor.Current = Cursors.WaitCursor;
					using (icController icController = new icController())
					{
						icController.ControllerSN = int.Parse(strSN);
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						icController.UpdateConfigureIP(new wgMjControllerConfigure
						{
							dhcpEnable = 0
						}, -1);
					}
					if (!string.IsNullOrEmpty(this.EncryptControllerSN) && this.EncryptControllerSN.IndexOf(strSN) >= 0)
					{
						this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, text, true);
					}
					else
					{
						this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, text, false);
					}
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						text2, "  SN=", strSN, ", Mac=", strMac, ",IP =", strIP, ",Mask=", strMask, ",Gateway=",
						strGateway, ", Port = ", strTCPPort, ", PC IPAddr=", text
					}));
					if (this.chkSearchAgain.Checked)
					{
						Thread.Sleep(5000);
						this.btnSearch.PerformClick();
					}
					else
					{
						this.dgvFoundControllers.Rows.Remove(dataGridViewRow);
					}
				}
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0002B614 File Offset: 0x0002A614
		private void btnDefault_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnDefault.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				string text = "-1";
				string text2 = "";
				string text3 = "192.168.0.0";
				string text4 = "255.255.255.0";
				string text5 = "";
				string text6 = "60000";
				string text7 = this.btnDefault.Text;
				this.IPConfigureCPU(text, text2, text3, text4, text5, text6, "", true);
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					text7, "  SN=", text, ", Mac=", text2, ",IP =", text3, ",Mask=", text4, ",Gateway=",
					text5, ", Port = ", text6, ", PC IPAddr="
				}));
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0002B71A File Offset: 0x0002A71A
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0002B722 File Offset: 0x0002A722
		private void btnFormatSpecial_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0002B724 File Offset: 0x0002A724
		private void btnGetTypeInfo_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0002B728 File Offset: 0x0002A728
		private void btnIPAndWebConfigure_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			string text;
			string strSN;
			string strMac;
			string strIP;
			string strMask;
			string strGateway;
			string strTCPPort;
			string text2;
			using (dfrmTCPIPWEBConfigure dfrmTCPIPWEBConfigure = new dfrmTCPIPWEBConfigure())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				dfrmTCPIPWEBConfigure.strSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
				dfrmTCPIPWEBConfigure.strMac = dataGridViewRow.Cells["f_MACAddr"].Value.ToString();
				dfrmTCPIPWEBConfigure.strIP = dataGridViewRow.Cells["f_IP"].Value.ToString();
				dfrmTCPIPWEBConfigure.strMask = dataGridViewRow.Cells["f_Mask"].Value.ToString();
				dfrmTCPIPWEBConfigure.strGateway = dataGridViewRow.Cells["f_Gateway"].Value.ToString();
				dfrmTCPIPWEBConfigure.strTCPPort = dataGridViewRow.Cells["f_PORT"].Value.ToString();
				text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				dfrmTCPIPWEBConfigure.strPCAddress = text;
				dfrmTCPIPWEBConfigure.strSearchedIP = dataGridViewRow.Cells["f_IP"].Value.ToString();
				dfrmTCPIPWEBConfigure.strSearchedMask = dataGridViewRow.Cells["f_Mask"].Value.ToString();
				if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
				{
					dfrmTCPIPWEBConfigure.cboLanguage.SelectedIndex = 0;
					dfrmTCPIPWEBConfigure.cboLanguage2.SelectedIndex = 0;
				}
				else
				{
					dfrmTCPIPWEBConfigure.cboLanguage.SelectedIndex = 1;
					dfrmTCPIPWEBConfigure.cboLanguage2.SelectedIndex = 1;
				}
				dfrmTCPIPWEBConfigure.cboDateFormat.SelectedIndex = 0;
				if (this.bIPAndWEBConfigure)
				{
					dfrmTCPIPWEBConfigure.chkEditIP.Checked = this.bUpdateIPConfigure;
					dfrmTCPIPWEBConfigure.grpIP.Enabled = this.bUpdateIPConfigure;
					this.bOption = this.bOption || this.commPort != 60000;
					dfrmTCPIPWEBConfigure.btnOption.Enabled = !this.bOption;
					dfrmTCPIPWEBConfigure.lblPort.Visible = this.bOption;
					dfrmTCPIPWEBConfigure.nudPort.Visible = this.bOption;
					dfrmTCPIPWEBConfigure.nudPort.Value = decimal.Parse(this.commPort.ToString(), CultureInfo.InvariantCulture);
					dfrmTCPIPWEBConfigure.chkUpdateWebSet.Checked = this.bUpdateWEBConfigure;
					dfrmTCPIPWEBConfigure.grpWEBEnabled.Enabled = this.bUpdateWEBConfigure;
					dfrmTCPIPWEBConfigure.grpWEB.Enabled = this.bUpdateWEBConfigure && this.bWEBEnabled;
					dfrmTCPIPWEBConfigure.optWEBEnabled.Checked = this.bWEBEnabled;
					dfrmTCPIPWEBConfigure.cboLanguage.SelectedIndex = int.Parse(this.strWEBLanguage1);
					dfrmTCPIPWEBConfigure.txtSelectedFileName.Text = this.strSelectedFile1;
					this.bOptionWeb = this.bOptionWeb || this.HttpPort != 80;
					dfrmTCPIPWEBConfigure.btnOptionWEB.Enabled = !this.bOptionWeb;
					dfrmTCPIPWEBConfigure.lblHttpPort.Visible = this.bOptionWeb;
					dfrmTCPIPWEBConfigure.nudHttpPort.Visible = this.bOptionWeb;
					dfrmTCPIPWEBConfigure.nudHttpPort.Value = decimal.Parse(this.HttpPort.ToString(), CultureInfo.InvariantCulture);
					dfrmTCPIPWEBConfigure.cboDateFormat.SelectedIndex = this.webDateFormat;
					dfrmTCPIPWEBConfigure.chkAdjustTime.Checked = this.bAdjustTime;
					dfrmTCPIPWEBConfigure.chkWebOnlyQuery.Checked = this.bWebOnlyQuery;
					dfrmTCPIPWEBConfigure.chkUpdateSuperCard.Checked = this.bUpdateSuperCard_IPWEB;
					dfrmTCPIPWEBConfigure.grpSuperCards.Enabled = this.bUpdateSuperCard_IPWEB;
					dfrmTCPIPWEBConfigure.txtSuperCard1.Text = this.superCard1_IPWEB;
					dfrmTCPIPWEBConfigure.txtSuperCard2.Text = this.superCard2_IPWEB;
					dfrmTCPIPWEBConfigure.chkUpdateSpecialCard.Checked = this.bUpdateSpecialCard_IPWEB;
					dfrmTCPIPWEBConfigure.chkUpdateSpecialCard.Visible = this.bUpdateSpecialCard_IPWEB;
					dfrmTCPIPWEBConfigure.grpSpecialCards.Enabled = this.bUpdateSpecialCard_IPWEB;
					dfrmTCPIPWEBConfigure.grpSpecialCards.Visible = this.bUpdateSpecialCard_IPWEB;
					dfrmTCPIPWEBConfigure.txtSpecialCard1.Text = this.SpecialCard1_IPWEB;
					dfrmTCPIPWEBConfigure.txtSpecialCard2.Text = this.SpecialCard2_IPWEB;
					dfrmTCPIPWEBConfigure.cboLanguage2.SelectedIndex = int.Parse(this.strWEBLanguage2);
					dfrmTCPIPWEBConfigure.txtUsersFile.Text = this.strSelectedFile2;
					dfrmTCPIPWEBConfigure.chkAutoUploadWEBUsers.Checked = this.bAutoUploadUsers;
					dfrmTCPIPWEBConfigure.chkAutoUploadWEBUsers.Visible = this.bAutoUploadUsers;
					if (dfrmTCPIPWEBConfigure.strIP == "192.168.0.0")
					{
						dfrmTCPIPWEBConfigure.strIP = this.strIP_IPWEB;
						dfrmTCPIPWEBConfigure.strMask = this.strNETMASK_IPWEB;
						dfrmTCPIPWEBConfigure.strGateway = this.strGateway_IPWEB;
					}
				}
				if (dfrmTCPIPWEBConfigure.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				this.bIPAndWEBConfigure = true;
				this.bUpdateIPConfigure = dfrmTCPIPWEBConfigure.chkEditIP.Checked;
				this.commPort = int.Parse(dfrmTCPIPWEBConfigure.nudPort.Value.ToString());
				this.bOption = this.commPort != 60000;
				this.bUpdateWEBConfigure = dfrmTCPIPWEBConfigure.chkUpdateWebSet.Checked;
				this.bWEBEnabled = dfrmTCPIPWEBConfigure.optWEBEnabled.Checked;
				this.strWEBLanguage1 = dfrmTCPIPWEBConfigure.cboLanguage.SelectedIndex.ToString();
				this.strSelectedFile1 = dfrmTCPIPWEBConfigure.txtSelectedFileName.Text;
				this.HttpPort = int.Parse(dfrmTCPIPWEBConfigure.nudHttpPort.Value.ToString());
				this.bOptionWeb = this.HttpPort != 80;
				this.bAdjustTime = dfrmTCPIPWEBConfigure.chkAdjustTime.Checked;
				this.webDateFormat = dfrmTCPIPWEBConfigure.cboDateFormat.SelectedIndex;
				this.bWebOnlyQuery = dfrmTCPIPWEBConfigure.chkWebOnlyQuery.Checked;
				this.bUpdateSuperCard_IPWEB = dfrmTCPIPWEBConfigure.chkUpdateSuperCard.Checked;
				this.superCard1_IPWEB = dfrmTCPIPWEBConfigure.txtSuperCard1.Text;
				this.superCard2_IPWEB = dfrmTCPIPWEBConfigure.txtSuperCard2.Text;
				this.bUpdateSpecialCard_IPWEB = dfrmTCPIPWEBConfigure.chkUpdateSpecialCard.Checked;
				this.SpecialCard1_IPWEB = dfrmTCPIPWEBConfigure.txtSpecialCard1.Text;
				this.SpecialCard2_IPWEB = dfrmTCPIPWEBConfigure.txtSpecialCard2.Text;
				this.strWEBLanguage2 = dfrmTCPIPWEBConfigure.cboLanguage2.SelectedIndex.ToString();
				this.strSelectedFile2 = dfrmTCPIPWEBConfigure.txtUsersFile.Text;
				this.bAutoUploadUsers = dfrmTCPIPWEBConfigure.chkAutoUploadWEBUsers.Checked;
				this.strIP_IPWEB = dfrmTCPIPWEBConfigure.strIP;
				this.strNETMASK_IPWEB = dfrmTCPIPWEBConfigure.strMask;
				this.strGateway_IPWEB = dfrmTCPIPWEBConfigure.strGateway;
				if (dfrmTCPIPWEBConfigure.dtWebString != null)
				{
					this.dtWebStringAdvanced_IPWEB = dfrmTCPIPWEBConfigure.dtWebString.Copy();
				}
				else
				{
					this.dtWebStringAdvanced_IPWEB = null;
				}
				wgAppConfig.wgLog((sender as Button).Text + "  SN=" + dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				strSN = dfrmTCPIPWEBConfigure.strSN;
				strMac = dfrmTCPIPWEBConfigure.strMac;
				strIP = dfrmTCPIPWEBConfigure.strIP;
				strMask = dfrmTCPIPWEBConfigure.strMask;
				strGateway = dfrmTCPIPWEBConfigure.strGateway;
				strTCPPort = dfrmTCPIPWEBConfigure.strTCPPort;
				text2 = dfrmTCPIPWEBConfigure.Text;
			}
			try
			{
				this.Refresh();
				if (string.IsNullOrEmpty(strSN))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				if (this.bUpdateWEBConfigure || this.bUpdateSpecialCard_IPWEB || this.bUpdateSuperCard_IPWEB)
				{
					this.ipweb_webSet();
				}
				if (this.bAdjustTime)
				{
					using (icController icController = new icController())
					{
						icController.ControllerSN = int.Parse(strSN);
						if (icController.AdjustTimeIP(DateTime.Now, text) < 0)
						{
							XMessageBox.Show(CommonStr.strAdjustTime + " " + CommonStr.strFailed);
							return;
						}
						wgAppConfig.wgLog(strSN + " " + string.Format("{0}:{1}", CommonStr.strAdjustTimeOK, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
					}
				}
				if (this.bAutoUploadUsers)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow2 = this.dgvFoundControllers.SelectedRows[0];
				}
				if (this.bUpdateIPConfigure)
				{
					Cursor.Current = Cursors.WaitCursor;
					this.IPConfigureCPU(strSN, strMac, strIP, strMask, strGateway, strTCPPort, text, true);
					wgAppConfig.wgLog(string.Concat(new string[]
					{
						text2, "  SN=", strSN, ", Mac=", strMac, ",IP =", strIP, ",Mask=", strMask, ",Gateway=",
						strGateway, ", Port = ", strTCPPort, ", PC IPAddr=", text
					}));
				}
				else if (this.bUpdateWEBConfigure)
				{
					using (icController icController2 = new icController())
					{
						icController2.ControllerSN = int.Parse(strSN);
						icController2.PCIPAddress = text;
						icController2.RebootControllerIP();
					}
				}
			}
			catch (Exception)
			{
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0002C0DC File Offset: 0x0002B0DC
		private void btnSearch_Click(object sender, EventArgs e)
		{
			this.invalidSNofWGYTJ = 0;
			uint maxValue = uint.MaxValue;
			this.EncryptControllerSN = "";
			Cursor.Current = Cursors.WaitCursor;
			this.lblCount.Text = "0";
			this.toolStripStatusLabel1.Text = "0";
			this.dgvFoundControllers.Rows.Clear();
			this.btnConfigure.Enabled = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			this.foundControllerIPs = "";
			this.foundControllerIPs = wgAppConfig.GetKeyVal("KEY_FoundControllerIPs");
			this.btnSearch_Click_short(sender, e);
			this.btnSearch.Enabled = false;
			Thread.Sleep(100);
			this.Refresh();
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = maxValue;
			wgpacket.iCallReturn = 0;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			if (WGPacket.bCommP)
			{
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation.Address.ToString());
									this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
									Thread.Sleep(300);
									byte[] array2 = wgpacket.ToBytesNoPassword(this.wgudp.udpPort);
									if (array2 == null)
									{
										return;
									}
									byte[] array3 = null;
									this.wgudp.udp_get(array2, 300, 0U, null, 60000, ref array3);
									if (array3 != null)
									{
										long num4 = DateTime.Now.Ticks + 4000000L;
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array3,
											unicastIPAddressInformation.Address.ToString(),
											networkInterface.GetPhysicalAddress().ToString(),
											"0"
										});
										while (DateTime.Now.Ticks < num4)
										{
											if (this.wgudp.PacketCount > 0)
											{
												while (this.wgudp.PacketCount > 0)
												{
													array3 = this.wgudp.GetPacket();
													this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
													{
														array3,
														unicastIPAddressInformation.Address.ToString(),
														networkInterface.GetPhysicalAddress().ToString(),
														"0"
													});
												}
												num4 = DateTime.Now.Ticks + 4000000L;
											}
											else
											{
												Thread.Sleep(100);
											}
										}
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
			}
			foreach (NetworkInterface networkInterface2 in allNetworkInterfaces)
			{
				if (networkInterface2.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface2.OperationalStatus == OperationalStatus.Up)
				{
					num++;
					if (networkInterface2.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
					{
						num2++;
					}
					UnicastIPAddressInformationCollection unicastAddresses2 = networkInterface2.GetIPProperties().UnicastAddresses;
					if (unicastAddresses2.Count > 0)
					{
						Console.WriteLine(networkInterface2.Description);
						bool flag = true;
						foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in unicastAddresses2)
						{
							if (unicastIPAddressInformation2.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation2.Address.IsIPv6LinkLocal && unicastIPAddressInformation2.Address.ToString() != "127.0.0.1")
							{
								if (flag)
								{
									num3++;
									flag = false;
								}
								text += string.Format("{0}, ", unicastIPAddressInformation2.Address.ToString());
								Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation2.Address.ToString());
								this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
								Thread.Sleep(300);
								byte[] array5 = wgpacket.ToBytes(this.wgudp.udpPort);
								if (array5 == null)
								{
									return;
								}
								byte[] array6 = null;
								this.wgudp.udp_get(array5, 300, uint.MaxValue, null, 60000, ref array6);
								if (array6 != null)
								{
									Thread.Sleep(300);
									long ticks = DateTime.Now.Ticks;
									long num5 = ticks + 4000000L;
									int num6 = 0;
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
									{
										array6,
										unicastIPAddressInformation2.Address.ToString(),
										networkInterface2.GetPhysicalAddress().ToString(),
										WGPacket.bCommP ? "1" : "0"
									});
									num6++;
									while (DateTime.Now.Ticks < num5)
									{
										if (this.wgudp.PacketCount > 0)
										{
											while (this.wgudp.PacketCount > 0)
											{
												array6 = this.wgudp.GetPacket();
												this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
												{
													array6,
													unicastIPAddressInformation2.Address.ToString(),
													networkInterface2.GetPhysicalAddress().ToString(),
													WGPacket.bCommP ? "1" : "0"
												});
												num6++;
											}
											num5 = DateTime.Now.Ticks + 4000000L;
											long num7 = (DateTime.Now.Ticks - ticks) / 10000L;
											wgTools.WgDebugWrite(string.Format("搜索到控制器数={0}:所花时间={1}ms", num6.ToString(), num7.ToString()), new object[0]);
										}
										else
										{
											Thread.Sleep(100);
										}
									}
								}
							}
						}
						Console.WriteLine();
					}
				}
			}
			if (this.dgvFoundControllers.Rows.Count <= 0)
			{
				if (wgTools.gWGYTJ && this.invalidSNofWGYTJ > 0)
				{
					string text2 = string.Concat(new string[]
					{
						CommonStr.strControllerTypeUnsupport,
						"\r\n\r\n",
						CommonStr.strControllerTypeUnsupportSoftwareTitle,
						wgAppConfig.LoginTitle,
						"\r\n\r\n",
						CommonStr.strControllerTypeUnsupportControllerStart,
						this.invalidSNofWGYTJ.ToString().Substring(0, 4),
						"*****"
					});
					wgAppConfig.wgLog(text2);
					XMessageBox.Show(text2);
					this.btnSearch.Enabled = true;
					return;
				}
				foreach (NetworkInterface networkInterface3 in allNetworkInterfaces)
				{
					if (networkInterface3.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface3.OperationalStatus == OperationalStatus.Up)
					{
						num++;
						if (networkInterface3.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						{
							num2++;
						}
						UnicastIPAddressInformationCollection unicastAddresses3 = networkInterface3.GetIPProperties().UnicastAddresses;
						if (unicastAddresses3.Count > 0)
						{
							Console.WriteLine(networkInterface3.Description);
							bool flag2 = true;
							foreach (UnicastIPAddressInformation unicastIPAddressInformation3 in unicastAddresses3)
							{
								if (unicastIPAddressInformation3.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation3.Address.IsIPv6LinkLocal && unicastIPAddressInformation3.Address.ToString() != "127.0.0.1")
								{
									if (flag2)
									{
										num3++;
										flag2 = false;
									}
									text += string.Format("{0}, ", unicastIPAddressInformation3.Address.ToString());
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation3.Address.ToString());
									this.wgudp = new wgUdpComm(unicastIPAddressInformation3.Address);
									byte[] array7 = wgpacket.ToBytesNoPasswordAllProd(this.wgudp.udpPort);
									if (array7 == null)
									{
										return;
									}
									byte[] array8 = null;
									this.wgudp.udp_get(array7, 300, uint.MaxValue, null, 60000, ref array8);
									if (array8 != null && array8.Length == 1044 && (int)array8[18] != wgTools.gate && !wgMjController.IsFingerController((int)array8[8] + ((int)array8[9] << 8) + ((int)array8[10] << 16) + ((int)array8[11] << 24)))
									{
										string text3 = string.Concat(new string[]
										{
											CommonStr.strControllerTypeUnsupport,
											"\r\n\r\n",
											CommonStr.strControllerTypeUnsupportSoftwareTitle,
											wgAppConfig.LoginTitle,
											"\r\n\r\n",
											CommonStr.strControllerTypeUnsupportControllerStart,
											((ulong)((long)((int)array8[8] + ((int)array8[9] << 8) + ((int)array8[10] << 16) + ((int)array8[11] << 24)))).ToString().Substring(0, 4),
											"*****"
										});
										wgAppConfig.wgLog(text3);
										XMessageBox.Show(text3);
										this.btnSearch.Enabled = true;
										return;
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
			}
			if (this.dgvFoundControllers.Rows.Count <= 0)
			{
				foreach (NetworkInterface networkInterface4 in allNetworkInterfaces)
				{
					if (networkInterface4.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface4.OperationalStatus == OperationalStatus.Up)
					{
						num++;
						if (networkInterface4.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						{
							num2++;
						}
						UnicastIPAddressInformationCollection unicastAddresses4 = networkInterface4.GetIPProperties().UnicastAddresses;
						if (unicastAddresses4.Count > 0)
						{
							Console.WriteLine(networkInterface4.Description);
							bool flag3 = true;
							foreach (UnicastIPAddressInformation unicastIPAddressInformation4 in unicastAddresses4)
							{
								if (unicastIPAddressInformation4.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation4.Address.IsIPv6LinkLocal && unicastIPAddressInformation4.Address.ToString() != "127.0.0.1")
								{
									if (flag3)
									{
										num3++;
										flag3 = false;
									}
									text += string.Format("{0}, ", unicastIPAddressInformation4.Address.ToString());
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation4.Address.ToString());
									this.wgudp = new wgUdpComm(unicastIPAddressInformation4.Address);
									byte[] array9 = wgMjController.GetControlDataBroadcast(197504, 1427898384);
									if (array9 == null)
									{
										return;
									}
									byte[] array10 = null;
									this.wgudp.udp_get(array9, 300, uint.MaxValue, null, 60000, ref array10);
									if (array10 != null && array10.Length == 1052)
									{
										string text4 = string.Concat(new string[]
										{
											WGPacket.bCommP ? CommonStr.strNetControlWrongPassword : CommonStr.strControllerTypeCommunicationPassword,
											"\r\n\r\n",
											CommonStr.strControllerTypeUnsupportControllerStart,
											"  ",
											((ulong)((long)((int)array10[8] + ((int)array10[9] << 8) + ((int)array10[10] << 16) + ((int)array10[11] << 24)))).ToString().Substring(0, 4),
											"*****"
										});
										array9 = wgpacket.ToBytes(this.wgudp.udpPort);
										this.wgudp.udp_get(array9, 300, uint.MaxValue, null, 60000, ref array10);
										if (array10 == null)
										{
											wgAppConfig.wgLog(text4);
											XMessageBox.Show(text4);
											this.btnSearch.Enabled = true;
											return;
										}
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array10,
											unicastIPAddressInformation4.Address.ToString(),
											networkInterface4.GetPhysicalAddress().ToString(),
											WGPacket.bCommP ? "1" : "0"
										});
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
			}
			if (this.dgvFoundControllers.Rows.Count <= 0)
			{
				foreach (NetworkInterface networkInterface5 in allNetworkInterfaces)
				{
					if (networkInterface5.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface5.OperationalStatus == OperationalStatus.Up)
					{
						num++;
						if (networkInterface5.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						{
							num2++;
						}
						UnicastIPAddressInformationCollection unicastAddresses5 = networkInterface5.GetIPProperties().UnicastAddresses;
						if (unicastAddresses5.Count > 0)
						{
							Console.WriteLine(networkInterface5.Description);
							bool flag4 = true;
							foreach (UnicastIPAddressInformation unicastIPAddressInformation5 in unicastAddresses5)
							{
								if (unicastIPAddressInformation5.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation5.Address.IsIPv6LinkLocal && unicastIPAddressInformation5.Address.ToString() != "127.0.0.1")
								{
									if (flag4)
									{
										num3++;
										flag4 = false;
									}
									text += string.Format("{0}, ", unicastIPAddressInformation5.Address.ToString());
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation5.Address.ToString());
									this.wgudp = new wgUdpComm(unicastIPAddressInformation5.Address);
									byte[] array11 = new WGPacketBasicPCAllowedIPSetToSend(0, 0, 0, 0, null, null, null)
									{
										iDevSnTo = uint.MaxValue
									}.ToBytes(0);
									if (array11 == null)
									{
										return;
									}
									byte[] array12 = null;
									this.wgudp.udp_get(array11, 300, uint.MaxValue, null, 60000, ref array12);
									if (array12 != null && array12.Length == 76)
									{
										string text5 = this.iPFilterToolStripMenuItem.Text + "***\r\n" + CommonStr.strWrongVersionDatabaseTail;
										uint num8 = (uint)((int)array12[8] + ((int)array12[9] << 8) + ((int)array12[10] << 16) + ((int)array12[11] << 24));
										wgpacket.iDevSnTo = num8;
										array11 = wgpacket.ToBytes(this.wgudp.udpPort);
										this.wgudp.udp_get_notries(array11, 300, uint.MaxValue, null, 60000, ref array12);
										if (array12 == null)
										{
											wgAppConfig.wgLog(text5);
											XMessageBox.Show(text5);
											this.btnSearch.Enabled = true;
											return;
										}
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
			}
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				this.dgvFoundControllers.Sort(this.dgvFoundControllers.Columns[1], ListSortDirection.Ascending);
				for (int j = 0; j < this.dgvFoundControllers.Rows.Count; j++)
				{
					int num9 = j + 1;
					this.dgvFoundControllers.Rows[j].Cells[0].Value = num9.ToString().PadLeft(4, '0');
				}
			}
			this.btnSearch.Enabled = true;
			wgAppConfig.wgLog(string.Format("{0} Count = {1:d}  : {2}", this.Text, this.dgvFoundControllers.Rows.Count, this.strControllers));
			wgAppConfig.wgLog(string.Format("{0}: Up Adapter Count = {1:d}; Up Adapter Wireless Count = {2:d}; Up Adapter With IPV4 Count = {3:d}; All IP : {4}", new object[]
			{
				string.Empty.PadLeft(this.Text.Length * 2, ' '),
				num,
				num2,
				num3,
				text
			}));
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				this.btnConfigure.Enabled = true;
			}
			else
			{
				string text6 = CommonStr.strNoControllerInfo2Base;
				if (num == 0)
				{
					text6 = CommonStr.strNoControllerInfo3PCNotConnected;
				}
				else if (num >= 2 && num2 >= 1)
				{
					text6 = CommonStr.strNoControllerInfo1;
				}
				if (this.bFirstShowInfo)
				{
					this.bFirstShowInfo = false;
					XMessageBox.Show(text6);
				}
			}
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0002D208 File Offset: 0x0002C208
		private void btnSearch_Click_short(object sender, EventArgs e)
		{
			uint maxValue = uint.MaxValue;
			Cursor.Current = Cursors.WaitCursor;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			this.btnSearch.Enabled = false;
			Thread.Sleep(100);
			this.Refresh();
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 64;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = maxValue;
			wgpacket.iCallReturn = 0;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			if (WGPacket.bCommP)
			{
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation.Address.ToString());
									this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
									Thread.Sleep(300);
									byte[] array2 = wgpacket.ToBytesNoPassword(this.wgudp.udpPort);
									if (array2 == null)
									{
										return;
									}
									byte[] array3 = null;
									this.wgudp.udp_get(array2, 300, 0U, null, 60000, ref array3);
									if (array3 != null)
									{
										long num4 = DateTime.Now.Ticks + 4000000L;
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array3,
											unicastIPAddressInformation.Address.ToString(),
											networkInterface.GetPhysicalAddress().ToString(),
											"0"
										});
										while (DateTime.Now.Ticks < num4)
										{
											if (this.wgudp.PacketCount > 0)
											{
												while (this.wgudp.PacketCount > 0)
												{
													array3 = this.wgudp.GetPacket();
													this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
													{
														array3,
														unicastIPAddressInformation.Address.ToString(),
														networkInterface.GetPhysicalAddress().ToString(),
														"0"
													});
												}
												num4 = DateTime.Now.Ticks + 4000000L;
											}
											else
											{
												Thread.Sleep(100);
											}
										}
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
			}
			foreach (NetworkInterface networkInterface2 in allNetworkInterfaces)
			{
				if (networkInterface2.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface2.OperationalStatus == OperationalStatus.Up)
				{
					num++;
					if (networkInterface2.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
					{
						num2++;
					}
					UnicastIPAddressInformationCollection unicastAddresses2 = networkInterface2.GetIPProperties().UnicastAddresses;
					if (unicastAddresses2.Count > 0)
					{
						Console.WriteLine(networkInterface2.Description);
						bool flag = true;
						foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in unicastAddresses2)
						{
							if (unicastIPAddressInformation2.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation2.Address.IsIPv6LinkLocal && unicastIPAddressInformation2.Address.ToString() != "127.0.0.1")
							{
								if (flag)
								{
									num3++;
									flag = false;
								}
								text += string.Format("{0}, ", unicastIPAddressInformation2.Address.ToString());
								Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation2.Address.ToString());
								this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
								Thread.Sleep(300);
								byte[] array5 = wgpacket.ToBytes(this.wgudp.udpPort);
								if (array5 == null)
								{
									return;
								}
								byte[] array6 = null;
								this.wgudp.udp_get(array5, 300, uint.MaxValue, null, 60000, ref array6);
								if (array6 != null)
								{
									Thread.Sleep(300);
									long ticks = DateTime.Now.Ticks;
									long num5 = ticks + 4000000L;
									int num6 = 0;
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
									{
										array6,
										unicastIPAddressInformation2.Address.ToString(),
										networkInterface2.GetPhysicalAddress().ToString(),
										WGPacket.bCommP ? "1" : "0"
									});
									num6++;
									while (DateTime.Now.Ticks < num5)
									{
										if (this.wgudp.PacketCount > 0)
										{
											while (this.wgudp.PacketCount > 0)
											{
												array6 = this.wgudp.GetPacket();
												this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
												{
													array6,
													unicastIPAddressInformation2.Address.ToString(),
													networkInterface2.GetPhysicalAddress().ToString(),
													WGPacket.bCommP ? "1" : "0"
												});
												num6++;
											}
											num5 = DateTime.Now.Ticks + 4000000L;
											long num7 = (DateTime.Now.Ticks - ticks) / 10000L;
											wgTools.WgDebugWrite(string.Format("搜索到控制器数={0}:所花时间={1}ms", num6.ToString(), num7.ToString()), new object[0]);
										}
										else
										{
											Thread.Sleep(100);
										}
									}
								}
							}
						}
						Console.WriteLine();
					}
				}
			}
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0002D89C File Offset: 0x0002C89C
		private void btnSetProductType_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0002D89E File Offset: 0x0002C89E
		private void btnTestNewCustTypeController_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0002D8A0 File Offset: 0x0002C8A0
		private void clearSwipesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (!this.bInput5678)
			{
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.setPasswordChar('*');
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || dfrmInputNewName.strNewName != "5678")
					{
						return;
					}
				}
			}
			this.bInput5678 = true;
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				string text = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
				string text2 = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				string text3 = "";
				if (XMessageBox.Show(this, sender.ToString() + " " + text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
				{
					int num = 60000;
					if (this.wgudp != null)
					{
						this.wgudp = null;
					}
					IPAddress ipaddress;
					if (IPAddress.TryParse(text2, out ipaddress))
					{
						this.wgudp = new wgUdpComm(IPAddress.Parse(text2));
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
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
					wgpacketSSI_FLASH.type = 33;
					wgpacketSSI_FLASH.code = 48;
					wgpacketSSI_FLASH.iDevSnFrom = 0U;
					wgpacketSSI_FLASH.iDevSnTo = uint.Parse(text);
					wgpacketSSI_FLASH.iCallReturn = 0;
					wgpacketSSI_FLASH.ucData = new byte[1024];
					try
					{
						Thread.Sleep(300);
						wgpacketSSI_FLASH.iStartFlashAddr = 5017600U;
						wgpacketSSI_FLASH.iEndFlashAddr = wgpacketSSI_FLASH.iStartFlashAddr + 1024U - 1U;
						for (int i = 0; i < 1024; i++)
						{
							wgpacketSSI_FLASH.ucData[i] = byte.MaxValue;
						}
						byte[] array = null;
						while (wgpacketSSI_FLASH.iStartFlashAddr <= 5025792U && this.wgudp.udp_get(wgpacketSSI_FLASH.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH.xid, text3, num, ref array) >= 0)
						{
							wgpacketSSI_FLASH.iStartFlashAddr += 1024U;
						}
						using (icController icController = new icController())
						{
							icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
							icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
							if (icController.RestoreAllSwipeInTheControllersIP() > 0)
							{
								icController.UpdateFRamIP(9U, 0U);
								icController.RebootControllerIP();
							}
						}
					}
					catch (Exception)
					{
					}
					wgAppConfig.wgLog(sender.ToString() + "  SN=" + text);
					return;
				}
			}
			else
			{
				XMessageBox.Show(CommonStr.strSelectController);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0002DBF0 File Offset: 0x0002CBF0
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.dgvFoundControllers.Rows.Clear();
			this.lblCount.Text = "0";
			this.toolStripStatusLabel1.Text = "0";
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0002DC24 File Offset: 0x0002CC24
		private void cloudServerConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgMjControllerConfigure wgMjControllerConfigure = null;
			using (dfrmCloudServerSet dfrmCloudServerSet = new dfrmCloudServerSet())
			{
				if (dfrmCloudServerSet.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				if (this.dgvFoundControllers.SelectedRows.Count <= 0 && wgAppConfig.gRestart)
				{
					((frmADCT3000)base.Owner).mnuExit.PerformClick();
					return;
				}
				wgMjControllerConfigure = dfrmCloudServerSet.controlConfigure;
			}
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				string text = "";
				int num = 0;
				string text2 = "";
				int num2 = 0;
				Cursor.Current = Cursors.WaitCursor;
				using (icController icController = new icController())
				{
					int num3 = 0;
					for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
						icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						num3++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num3.ToString();
						this.toolStripStatusLabel1.Text = num3.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text = text + icController.ControllerSN.ToString() + ",";
							num++;
							icController.RebootControllerIP();
						}
						else
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num2++;
						}
						Application.DoEvents();
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text2))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strFailed,
						num2,
						text2
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num2,
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strSuccessfully,
						num,
						text
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num,
						text
					}));
				}
				if (wgAppConfig.gRestart)
				{
					((frmADCT3000)base.Owner).mnuExit.PerformClick();
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0002DFA0 File Offset: 0x0002CFA0
		private void communicationTestToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.Rows.Count > 0 && this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				icController.ControllerSN = -1;
				if (this.dgvFoundControllers.Rows.Count > 0)
				{
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				wgTools.WriteLine("control.SpecialPingIP Start");
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
				wgTools.WriteLine("control.SpecialPingIP End");
				wgUdpComm.triesTotal = 0L;
				wgTools.WriteLine("control.Test1024 Start");
				int num4 = 0;
				string text = "";
				if (icController.test1024Write() < 0)
				{
					text = text + CommonStr.strCommLargePacketWriteFailed + "\r\n";
				}
				int num5 = icController.test1024Read(100U, ref num4);
				if (num5 < 0)
				{
					text = text + CommonStr.strCommLargePacketReadFailed + num5.ToString() + "\r\n";
				}
				if (wgUdpComm.triesTotal > 0L)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						CommonStr.strCommLargePacketTryTimes,
						" = ",
						wgUdpComm.triesTotal.ToString(),
						"\r\n"
					});
				}
				wgTools.WriteLine("control.Test1024 End");
				if (num3 == 0)
				{
					if (text == "")
					{
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							sender.ToString(),
							"  SN=",
							icController.ControllerSN,
							" ",
							CommonStr.strCommOK
						}));
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strCommOK));
					}
					else
					{
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							sender.ToString(),
							"  SN=",
							icController.ControllerSN,
							" ",
							CommonStr.strCommLose,
							" ",
							text
						}));
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strCommLose,
							text
						}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				else
				{
					string text3 = string.Format(" {0}: {1}={2}, {3}={4}, {5} = {6}", new object[]
					{
						CommonStr.strCommPacket,
						CommonStr.strCommPacketSent,
						num,
						CommonStr.strCommPacketReceived,
						num2,
						CommonStr.strCommPacketLost,
						num3
					}) + "\r\n";
					wgAppConfig.wgLog(string.Concat(new object[]
					{
						sender.ToString(),
						"  SN=",
						icController.ControllerSN,
						" ",
						CommonStr.strCommLose,
						" ",
						text3,
						text
					}));
					XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n\r\n{3}\r\n{4}", new object[]
					{
						icController.ControllerSN,
						sender.ToString(),
						CommonStr.strCommLose,
						text3,
						text
					}), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0002E3D4 File Offset: 0x0002D3D4
		private void customTypeConfigureCPU(string strSN, string strMac, string strIP, string strMask, string strGateway, string strTCPPort, string PCIPAddr, bool bAllowUseCommPassword, long customType)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0002E3D8 File Offset: 0x0002D3D8
		private void deactiveLimit2HourOutForSwimmingPoolV898AboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			string text = "";
			int num = 0;
			string text2 = "";
			int num2 = 0;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.antiback_validtime = 0;
				int num3 = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					num3++;
					this.lblSearchNow.Text = icController.ControllerSN.ToString();
					this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
					this.lblCount.Text = num3.ToString();
					this.toolStripStatusLabel1.Text = num3.ToString();
					Application.DoEvents();
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						text = text + icController.ControllerSN.ToString() + ",";
						num++;
					}
					else
					{
						text2 = text2 + icController.ControllerSN.ToString() + ",";
						num2++;
					}
					Application.DoEvents();
				}
			}
			Cursor.Current = Cursors.Default;
			if (!string.IsNullOrEmpty(text2))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					"",
					CommonStr.strFailed,
					num2,
					text2
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strFailed,
					num2,
					text2
				}));
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					"",
					CommonStr.strSuccessfully,
					num,
					text
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strSuccessfully,
					num,
					text
				}));
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0002E6C8 File Offset: 0x0002D6C8
		private void dfrmNetControllerConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
			int.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("KEY_UDPCLOUDSERVER_ENABLE")), out wgTools.bUDPCloud);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0002E6F8 File Offset: 0x0002D6F8
		private void dfrmNetControllerConfig_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ((e.Control && e.KeyValue == 70) || e.KeyValue == 114)
				{
					if (this.dfrmFind1 == null)
					{
						this.dfrmFind1 = new dfrmFind();
					}
					this.dfrmFind1.setObjtoFind(this.dgvFoundControllers, null);
				}
				if (e.Control && e.KeyValue == 67)
				{
					string text = "";
					for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
					{
						for (int j = 0; j < this.dgvFoundControllers.ColumnCount; j++)
						{
							text = text + this.dgvFoundControllers.Rows[i].Cells[j].Value.ToString() + "\t";
						}
						text += "\r\n";
					}
					try
					{
						Clipboard.SetText(text);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						wgAppConfig.wgLogWithoutDB(text, EventLogEntryType.Information, null);
					}
				}
				if (e.Control && e.Shift && e.KeyValue == 81)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					this.funCtrlShiftQ();
				}
				if (e.Control && e.Shift && e.KeyValue == 84)
				{
					if (icOperator.OperatorID != 1)
					{
						XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.FuncControlShiftT();
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WriteLine(ex2.ToString());
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0002E8B4 File Offset: 0x0002D8B4
		private void dfrmNetControllerConfig_Load(object sender, EventArgs e)
		{
			wgTools.bUDPCloud = 0;
			this.bFindActive = false;
			this.btnConfigure.Enabled = false;
			if (dfrmNetControllerConfig.functionQ5678)
			{
				this.setFunctionQ5678();
			}
			this.search100FromTheSpecialSNToolStripMenuItem.Visible = false;
			this.get656InfoToolStripMenuItem.Visible = false;
			this.inputOldDeviceInformationToolStripMenuItem.Visible = false;
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.sMSConfigureToolStripMenuItem.Visible = false;
				this.masterClientControllerSetToolStripMenuItem.Visible = false;
				this.swipeInOrderToolStripMenuItem.Visible = false;
				this.pCControlSwipeToolStripMenuItem.Visible = false;
				this.rS232ConfigureToolStripMenuItem.Visible = false;
				this.wGQRReaderConfigureV892AboveToolStripMenuItem.Visible = false;
				this.setAlarmOffDelayToolStripMenuItem.Visible = false;
				this.limit2HourOutForSwimmingPoolToolStripMenuItem.Visible = false;
				this.get656InfoToolStripMenuItem.Visible = true;
				this.inputOldDeviceInformationToolStripMenuItem.Visible = true;
			}
			this.restoreAllSwipesToolStripMenuItem.Visible = true;
			this.readerInputFormatToolStripMenuItem.Visible = true;
			this.btnSearch.PerformClick();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0002E9AD File Offset: 0x0002D9AD
		private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.btnConfigure.Enabled)
			{
				this.btnConfigure.PerformClick();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0002E9C8 File Offset: 0x0002D9C8
		private void disableMasterClientToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			uint num = 0U;
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				if (sender == this.setAsClientToolStripMenuItem)
				{
					using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
					{
						dfrmInputNewName.Text = CommonStr.strMasterControllerSN;
						dfrmInputNewName.label1.Text = CommonStr.strControllerSN;
						dfrmInputNewName.strNewName = "";
						if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || !uint.TryParse(dfrmInputNewName.strNewName, out num))
						{
							return;
						}
					}
				}
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					if (sender == this.disableMasterClientToolStripMenuItem)
					{
						wgMjControllerConfigure.controllerServer = 0;
					}
					else if (sender == this.setAsMasterToolStripMenuItem)
					{
						wgMjControllerConfigure.controllerServer = 1;
					}
					else if (sender == this.setAsClientToolStripMenuItem)
					{
						wgMjControllerConfigure.controllerServer = (int)num;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							sender.ToString(),
							"  SN=",
							icController.ControllerSN,
							(wgMjControllerConfigure.controllerServer > 1) ? (" Main=" + wgMjControllerConfigure.controllerServer) : ""
						}));
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							""
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0002EC94 File Offset: 0x0002DC94
		private void disableMobileOpenDoorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			string text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(this.strIPListControllerSN);
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					byte[] array = new byte[1152];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
					int num = 176;
					array[num] = 254;
					array[1024 + (num >> 3)] = array[1024 + (num >> 3)] | (byte)(1 << (num & 7));
					int num2 = icController.UpdateConfigureCPUSuperIP(array, "", text);
					if (num2 < 0)
					{
						wgAppConfig.wgDebugWrite(".DisableMobileOpenDoor_ Err=" + num2.ToString());
						this.errInfo(num2);
					}
					else
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strSuccessfully));
					}
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0002EE8C File Offset: 0x0002DE8C
		private void disablePCControlSwipeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					if (sender == this.enabledPCControlSwipeToolStripMenuItem)
					{
						wgMjControllerConfigure.pcControlSwipeTimeout = 30;
					}
					else
					{
						wgMjControllerConfigure.pcControlSwipeTimeout = 0;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							""
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0002F068 File Offset: 0x0002E068
		private void disableSwipeInOrderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					wgMjControllerConfigure.swipeOrderMode = 0;
					if (sender == this.swipeInOrderMode2Reader134213ToolStripMenuItem)
					{
						wgMjControllerConfigure.swipeOrderMode = 2;
					}
					else if (sender == this.swipeInOrderMode1Reader1212ToolStripMenuItem)
					{
						wgMjControllerConfigure.swipeOrderMode = 1;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							""
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0002F260 File Offset: 0x0002E260
		private void enableDHCPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					if (sender == this.enableDHCPToolStripMenuItem)
					{
						wgMjControllerConfigure.dhcpEnable = 165;
					}
					else
					{
						wgMjControllerConfigure.dhcpEnable = 0;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						icController.RebootControllerIP();
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							""
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0002F454 File Offset: 0x0002E454
		private void enableInvalidSwipeOpenDoorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					if (sender == this.enableInvalidSwipeOpenDoorToolStripMenuItem)
					{
						wgMjControllerConfigure.invalid_swipe_opendoor = 165;
					}
					else
					{
						wgMjControllerConfigure.invalid_swipe_opendoor = 0;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							""
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0002F634 File Offset: 0x0002E634
		private void enableMobileOpenDoorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			string text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(this.strIPListControllerSN);
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					byte[] array = new byte[1152];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
					for (int j = 176; j < 192; j++)
					{
						array[j] = byte.MaxValue;
						array[1024 + (j >> 3)] = array[1024 + (j >> 3)] | (byte)(1 << (j & 7));
					}
					int num = icController.UpdateConfigureCPUSuperIP(array, "", text);
					if (num < 0)
					{
						wgAppConfig.wgDebugWrite(".EnableMobileOpenDoor_ Err=" + num.ToString());
						this.errInfo(num);
					}
					else
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strSuccessfully));
					}
				}
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0002F840 File Offset: 0x0002E840
		private void errInfo(int ret)
		{
			if (ret < 0)
			{
				if (ret < -1)
				{
					XMessageBox.Show(CommonStr.strErrPwd);
					return;
				}
				XMessageBox.Show(CommonStr.strFailed);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0002F864 File Offset: 0x0002E864
		private void findF3ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.dfrmFind1 == null)
				{
					this.dfrmFind1 = new dfrmFind();
				}
				this.dfrmFind1.setObjtoFind(this.dgvFoundControllers, null);
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0002F8B8 File Offset: 0x0002E8B8
		private void force10MHalfDuplexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					wgMjControllerConfigure.auto_try10M_disable = 0;
					if (sender == this.force10MHalfDuplexToolStripMenuItem)
					{
						wgMjControllerConfigure.auto_try10M_disable = 166;
					}
					else if (sender == this.force100MV552AboveToolStripMenuItem)
					{
						wgMjControllerConfigure.auto_try10M_disable = 165;
					}
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
						{
							icController.ControllerSN,
							sender.ToString(),
							CommonStr.strSuccessfully,
							CommonStr.strRebootController
						}));
					}
					else
					{
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0002FAB8 File Offset: 0x0002EAB8
		private void formatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (icController icController = new icController())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				byte[] array = new byte[1152];
				array[1027] = 165;
				array[1026] = 165;
				array[1025] = 165;
				array[1024] = 165;
				icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				icController.UpdateConfigureSuperIP(array);
				wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
				XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
				{
					icController.ControllerSN,
					sender.ToString(),
					CommonStr.strSuccessfully,
					CommonStr.strRebootController
				}));
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0002FC2C File Offset: 0x0002EC2C
		private void FuncControlShiftT()
		{
			uint num = 0U;
			uint num2 = 0U;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = "Start";
				dfrmInputNewName.label1.Text = CommonStr.strControllerSN;
				dfrmInputNewName.strNewName = "100190001";
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || !uint.TryParse(dfrmInputNewName.strNewName, out num) || wgMjController.GetControllerType((int)num) == 0)
				{
					return;
				}
			}
			this.dgvFoundControllers.Rows.Clear();
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
			Cursor.Current = Cursors.WaitCursor;
			this.btnConfigure.Enabled = false;
			this.btnSearch.Enabled = false;
			Thread.Sleep(100);
			this.Refresh();
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iCallReturn = 0;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			num2 = num + 100U - 1U;
			this.wgudp = null;
			num = num % 100000000U + 100000000U;
			uint num3 = num % 100000000U;
			num2 = num2 % 100000000U + 100000000U;
			while (num <= num2 || num2 < 400000000U)
			{
				if (num > num2)
				{
					if (num < 200000000U)
					{
						num = num3 + 200000000U;
						num2 = num2 % 100000000U + 200000000U;
					}
					else
					{
						num = num3 + 400000000U;
						num2 = num2 % 100000000U + 400000000U;
					}
				}
				if ((num2 - num) % 5U == 0U)
				{
					this.lblSearchNow.Text = num.ToString();
					this.toolStripStatusLabel2.Text = num.ToString();
					this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
					this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
					this.Refresh();
					Application.DoEvents();
					Cursor.Current = Cursors.WaitCursor;
				}
				wgpacket.iDevSnTo = num;
				num += 1U;
				if (WGPacket.bCommP)
				{
					foreach (NetworkInterface networkInterface in allNetworkInterfaces)
					{
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (!unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation.Address.ToString());
									if (this.wgudp == null)
									{
										this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
										Thread.Sleep(300);
									}
									else if (this.wgudp.localIP.ToString() != unicastIPAddressInformation.Address.ToString())
									{
										this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
										Thread.Sleep(300);
									}
									byte[] array2 = wgpacket.ToBytesNoPassword(this.wgudp.udpPort);
									if (array2 == null)
									{
										return;
									}
									byte[] array3 = null;
									this.wgudp.udp_get(array2, 300, 0U, null, 60000, ref array3);
									if (array3 != null && !this.isExisted(wgpacket.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
									{
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array3,
											unicastIPAddressInformation.Address.ToString(),
											networkInterface.GetPhysicalAddress().ToString(),
											"0"
										});
										long ticks = DateTime.Now.Ticks;
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
				foreach (NetworkInterface networkInterface2 in allNetworkInterfaces)
				{
					UnicastIPAddressInformationCollection unicastAddresses2 = networkInterface2.GetIPProperties().UnicastAddresses;
					if (unicastAddresses2.Count > 0)
					{
						Console.WriteLine(networkInterface2.Description);
						foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in unicastAddresses2)
						{
							if (!unicastIPAddressInformation2.Address.IsIPv6LinkLocal && unicastIPAddressInformation2.Address.ToString() != "127.0.0.1")
							{
								Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation2.Address.ToString());
								if (this.wgudp == null)
								{
									this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
									Thread.Sleep(300);
								}
								else if (this.wgudp.localIP.ToString() != unicastIPAddressInformation2.Address.ToString())
								{
									this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
									Thread.Sleep(300);
								}
								byte[] array5 = wgpacket.ToBytes(this.wgudp.udpPort);
								if (array5 == null)
								{
									return;
								}
								byte[] array6 = null;
								this.wgudp.udp_get(array5, 300, uint.MaxValue, null, 60000, ref array6);
								if (array6 != null && !this.isExisted(wgpacket.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
								{
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
									{
										array6,
										unicastIPAddressInformation2.Address.ToString(),
										networkInterface2.GetPhysicalAddress().ToString(),
										WGPacket.bCommP ? "1" : "0"
									});
								}
							}
						}
						Console.WriteLine();
					}
				}
			}
			this.btnSearch.Enabled = true;
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				this.btnConfigure.Enabled = true;
			}
			this.lblSearchNow.Text = num2.ToString();
			this.toolStripStatusLabel2.Text = num2.ToString();
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00030344 File Offset: 0x0002F344
		private void funCtrlShiftQ()
		{
			string text = null;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToUpper();
				if (text == "WGTEST" + (DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour).ToString())
				{
					this.dfrmTest = new frmTestController();
					this.dfrmTest.Show();
					return;
				}
				if (text == "5678custtype".ToUpper())
				{
					this.setCustType();
					return;
				}
				if (text == "5678custtypenew".ToUpper())
				{
					this.custTypeToolStripMenuItem.Text = "更改控制器产品信息(请慎用!!!)";
					this.custTypeToolStripMenuItem.Visible = true;
					return;
				}
				string text2;
				switch (text2 = text)
				{
				case "00000000":
					if (this.btnSetProductType.Visible)
					{
						this.btnResetProductType.Visible = true;
						return;
					}
					return;
				case "5678":
					dfrmNetControllerConfig.functionQ5678 = true;
					this.setFunctionQ5678();
					return;
				case "FORMAT":
				case "GSH":
				case "GSK":
					this.quickFormatToolStripMenuItem.Visible = true;
					return;
				case "IP":
					this.btnDefault.Visible = true;
					return;
				case "WEB":
					this.btnIPAndWebConfigure.Visible = true;
					return;
				case "WEB5678":
					this.btnIPAndWebConfigure.Visible = true;
					return;
				case "PARAM":
					this.restoreDefaultParamToolStripMenuItem.Visible = true;
					return;
				case "RECORD":
					this.restoreAllSwipesToolStripMenuItem.Visible = true;
					return;
				case "FORMAT5678":
					this.formatToolStripMenuItem.Visible = true;
					return;
				case "CSN":
				case "CSQ":
					this.dfrmTest = new frmTestController();
					this.dfrmTest.Show();
					return;
				case "P":
					this.frmProductFormat1 = new frmProductFormat();
					this.frmProductFormat1.Show();
					return;
				case "READER":
					this.readerInputFormatToolStripMenuItem.Visible = true;
					return;
				case "HIDE5678":
					this.hideControllerToolStripMenuItem.Visible = true;
					return;
				case "DES":
					XMessageBox.Show(wgAppConfig.dbConString);
					break;

					return;
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00030678 File Offset: 0x0002F678
		private void get656InfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "";
			try
			{
				wgControllerInfo wgControllerInfo = new wgControllerInfo();
				string text2;
				string text3;
				wgAppConfig.getSystemParamValue(48, out text2, out text2, out text3);
				if (!string.IsNullOrEmpty(text3))
				{
					if (text3.IndexOf("\r\n") >= 0)
					{
						text3 = text3.Substring(text3.IndexOf("\r\n") + "\r\n".Length);
					}
					text = wgControllerInfo.getPassInfoWithRSA(text3);
				}
				if (string.IsNullOrEmpty(text))
				{
					XMessageBox.Show(CommonStr.strFailed);
				}
				else
				{
					UTF8Encoding utf8Encoding = new UTF8Encoding();
					RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
					rsacryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>x9P3JYYMphmIFo5l1qCjU4wWogP1ORtuNrK+8mk9Z0aCljY/3eJP86gqcWdqnfiN4iTwSWoKdSYy2+YwMmLV1cZ1Ma0j6bRQLtQgFTcv2gpWkGomLYKCF3Ok1huyCdxNs6TDXdcGxOGJpQdL4TLDHRpfIKMcoLBGfiO/KZ5TI/2CPgc8TJfx9SCFf4C/07rnAq9CoTjK64ruhDgdOWBePcNNsz687eb1j5LUzr7jhl+mpuddk3bL8TZWDks48ueBIsdxhgEGlMmbFXQvrell0n9e7S8AYzVaVR4wrqAnU9TJje4B/vDL1de1qbKD+jYI5zIcNQjGVjXZro8mCI72fQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					byte[] array = Convert.FromBase64String(text);
					byte[] bytes = utf8Encoding.GetBytes(text3);
					if (rsacryptoServiceProvider.VerifyData(bytes, "SHA1", array))
					{
						wgAppConfig.wgLog("get656InfoToolStripMenuItem");
						wgAppConfig.setSystemParamValue(64, "special value 7.95", "0", wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
						wgAppConfig.setSystemParamValue(221, "ParamLoc_OldBoardInfo", DateTime.Now.ToString(wgTools.YMDHMSFormat), text3);
						wgAppConfig.setSystemParamValue(222, "ParamLoc_OldBoardInfoRSA", DateTime.Now.ToString(wgTools.YMDHMSFormat), text);
						XMessageBox.Show(this.get656InfoToolStripMenuItem.Text + "  " + CommonStr.strSuccessfully);
					}
					else
					{
						wgAppConfig.wgLog(this.get656InfoToolStripMenuItem.Text + "  " + CommonStr.strFailed + "???");
						XMessageBox.Show(CommonStr.strFailed + "???", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.ToString().IndexOf("znsmart.com") >= 0)
				{
					XMessageBox.Show(CommonStr.strFaiedConnectedToInternet, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					XMessageBox.Show(CommonStr.strFailed);
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00030874 File Offset: 0x0002F874
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

		// Token: 0x06000166 RID: 358
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getSNSearch(IntPtr pkt);

		// Token: 0x06000167 RID: 359 RVA: 0x00030978 File Offset: 0x0002F978
		private void hideToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				using (icController icController = new icController())
				{
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					wgMjControllerConfigure.hide_ffffffff = ((sender == this.hideToolStripMenuItem) ? 165 : 0);
					icController.ControllerSN = int.Parse(this.strIPListControllerSN);
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strSuccessfully));
					}
					else
					{
						wgAppConfig.wgLog(string.Concat(new object[]
						{
							sender.ToString(),
							"  SN=",
							icController.ControllerSN,
							"  ",
							CommonStr.strFailed
						}));
						XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
					}
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00030B74 File Offset: 0x0002FB74
		private void hTTPServerConfigureV882AboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (dfrmHTTPServerConfig dfrmHTTPServerConfig = new dfrmHTTPServerConfig())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				dfrmHTTPServerConfig.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				dfrmHTTPServerConfig.Text = string.Format("{0}  [{1}]", dfrmHTTPServerConfig.Text, dfrmHTTPServerConfig.ControllerSN);
				if (dfrmHTTPServerConfig.ShowDialog() == DialogResult.OK)
				{
					wgAppConfig.wgLog(sender.ToString() + "  SN=" + int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString()));
					XMessageBox.Show(string.Format("{0}: {1} -- {2}", int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString()), sender.ToString(), CommonStr.strSuccessfully));
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00030CB8 File Offset: 0x0002FCB8
		private void inputOldDeviceInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripItem).Text;
				dfrmInputNewName.strNewName = "";
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dfrmInputNewName.strNewName))
				{
					bool flag = false;
					UTF8Encoding utf8Encoding = new UTF8Encoding();
					RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
					rsacryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>x9P3JYYMphmIFo5l1qCjU4wWogP1ORtuNrK+8mk9Z0aCljY/3eJP86gqcWdqnfiN4iTwSWoKdSYy2+YwMmLV1cZ1Ma0j6bRQLtQgFTcv2gpWkGomLYKCF3Ok1huyCdxNs6TDXdcGxOGJpQdL4TLDHRpfIKMcoLBGfiO/KZ5TI/2CPgc8TJfx9SCFf4C/07rnAq9CoTjK64ruhDgdOWBePcNNsz687eb1j5LUzr7jhl+mpuddk3bL8TZWDks48ueBIsdxhgEGlMmbFXQvrell0n9e7S8AYzVaVR4wrqAnU9TJje4B/vDL1de1qbKD+jYI5zIcNQjGVjXZro8mCI72fQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					string strNewName = dfrmInputNewName.strNewName;
					string[] array = strNewName.Split(new char[] { ';' });
					byte[] array2 = Convert.FromBase64String(array[1]);
					byte[] bytes = utf8Encoding.GetBytes(array[0]);
					if (rsacryptoServiceProvider.VerifyData(bytes, "SHA1", array2))
					{
						wgAppConfig.wgLog("inputOldDeviceInformationToolStripMenuItem");
						wgAppConfig.setSystemParamValue(64, "special value 7.95", "0", wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
						wgAppConfig.setSystemParamValue(223, "ParamLoc_SN_RSA", DateTime.Now.ToString(wgTools.YMDHMSFormat), strNewName);
						wgAppConfig.wgLog("ParamLoc_SN_RSA:" + strNewName);
						XMessageBox.Show(this.inputOldDeviceInformationToolStripMenuItem.Text + "  " + CommonStr.strSuccessfully);
					}
					else if (!flag)
					{
						XMessageBox.Show(this, CommonStr.strInvalidValue + "\r\n\r\n", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00030E48 File Offset: 0x0002FE48
		private void IPConfigure(string strSN, string strMac, string strIP, string strMask, string strGateway, string strTCPPort, string PCIPAddr)
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
			wgpacketWith.type = 36;
			wgpacketWith.code = 32;
			wgpacketWith.iDevSnFrom = 0U;
			if (int.Parse(strSN) == -1)
			{
				wgpacketWith.iDevSnTo = uint.MaxValue;
			}
			else
			{
				wgpacketWith.iDevSnTo = uint.Parse(strSN);
			}
			wgpacketWith.iCallReturn = 0;
			int num = 116;
			IPAddress.Parse(strIP).GetAddressBytes().CopyTo(wgpacketWith.ucData, num);
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 120;
			IPAddress.Parse(strMask).GetAddressBytes().CopyTo(wgpacketWith.ucData, num);
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 124;
			IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(wgpacketWith.ucData, num);
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 128;
			wgpacketWith.ucData[num] = (byte)(int.Parse(strTCPPort) & 255);
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith.ucData[num] = (byte)((int.Parse(strTCPPort) >> 8) & 255);
			wgpacketWith.ucData[1024 + (num >> 3)] = wgpacketWith.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			byte[] array = wgpacketWith.ToBytes(this.wgudp.udpPort);
			if (array == null)
			{
				wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
				return;
			}
			byte[] array2 = null;
			this.wgudp.udp_get(array, 300, 2147483647U, null, 60000, ref array2);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00031228 File Offset: 0x00030228
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

		// Token: 0x0600016C RID: 364 RVA: 0x00031754 File Offset: 0x00030754
		private void iPFilterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(wgTools.CommPStr))
			{
				XMessageBox.Show(CommonStr.strIPFilterDisableAsCommunicatePasswordEnabled);
				this.iPFilterToolStripMenuItem.Enabled = false;
				return;
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			if (this.dgvFoundControllers.Rows.Count != 0)
			{
				if (this.dgvFoundControllers.SelectedRows.Count <= 0)
				{
					XMessageBox.Show(CommonStr.strSelectController);
					return;
				}
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
				text2 = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				text3 = dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			}
			using (dfrmSecuritySet dfrmSecuritySet = new dfrmSecuritySet())
			{
				dfrmSecuritySet.strIPListCurrentPassword = this.strIPListCurrentPassword;
				dfrmSecuritySet.strIPListNewPassword = this.strIPListNewPassword;
				dfrmSecuritySet.strIPList = this.strIPList;
				dfrmSecuritySet.strControllerSN = this.strIPListControllerSN;
				dfrmSecuritySet.strPCIP = text2;
				if (dfrmSecuritySet.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				this.strIPListCurrentPassword = dfrmSecuritySet.strIPListCurrentPassword;
				this.strIPListNewPassword = dfrmSecuritySet.strIPListNewPassword;
				this.strIPList = dfrmSecuritySet.strIPList;
				text = dfrmSecuritySet.strOperate;
				this.strIPListControllerSN = dfrmSecuritySet.strControllerSN;
				this.Refresh();
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Equals("ChangePassword"))
				{
					wgAppConfig.wgLog(".IPList_Change_" + WGPacket.Ept(this.strIPListNewPassword));
				}
				Cursor.Current = Cursors.WaitCursor;
				string text4 = this.strIPListCurrentPassword;
				string text5 = this.strIPListNewPassword;
				using (icController icController = new icController())
				{
					icController.ControllerSN = int.Parse(this.strIPListControllerSN);
					icController.PCIPAddress = text2;
					byte[] array;
					int num;
					if (text.Equals("ChangePassword"))
					{
						array = Encoding.GetEncoding("utf-8").GetBytes(text4);
						byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(text5);
						num = icController.IPListControlSet(7, 0, 0, 0, array, bytes, null);
						if (num < 0)
						{
							wgAppConfig.wgDebugWrite("IPListChange Err=" + num.ToString());
							this.errInfo(num);
							return;
						}
						text4 = text5;
						array = Encoding.GetEncoding("utf-8").GetBytes(text4);
						num = icController.IPListControlSet(6, 194, 0, 0, array, null, null);
						if (num < 0)
						{
							wgAppConfig.wgDebugWrite("IPListChange Err=" + num.ToString());
							this.errInfo(num);
							return;
						}
						wgAppConfig.wgDebugWrite("IPListChange OK");
					}
					array = Encoding.GetEncoding("utf-8").GetBytes(text4);
					num = icController.IPListControlSet(3, 0, 0, 0, array, null, null);
					if (num < 0)
					{
						wgAppConfig.wgDebugWrite("IPListClear Err=" + num.ToString());
						this.errInfo(num);
					}
					else
					{
						wgAppConfig.wgLog(".IPList_Update_" + WGPacket.Ept(this.strIPList));
						if (!string.IsNullOrEmpty(this.strIPList))
						{
							byte[] array2 = new byte[16];
							if (this.strIPList.IndexOf(text2) >= 0)
							{
								array2 = new byte[16];
								long num2 = 0L;
								IPAddress ipaddress;
								if (IPAddress.TryParse(text3, out ipaddress))
								{
									ipaddress.GetAddressBytes().CopyTo(array2, 0);
								}
								else if (long.TryParse(text3.Replace(":", ""), NumberStyles.AllowHexSpecifier, null, out num2))
								{
									array2[0] = (byte)((num2 >> 40) & 255L);
									array2[1] = (byte)((num2 >> 32) & 255L);
									array2[2] = (byte)((num2 >> 24) & 255L);
									array2[3] = (byte)((num2 >> 16) & 255L);
									array2[4] = (byte)((num2 >> 8) & 255L);
									array2[5] = (byte)(num2 & 255L);
								}
								num = icController.IPListControlSet(10, 0, 0, 0, array, null, array2);
								if (num < 0)
								{
									wgAppConfig.wgDebugWrite("IPListUpdate Err=" + num.ToString());
									this.errInfo(num);
									return;
								}
							}
							foreach (string text6 in this.strIPList.Split(new char[] { ';' }))
							{
								array2 = new byte[16];
								long num3 = 0L;
								IPAddress ipaddress;
								if (IPAddress.TryParse(text6, out ipaddress))
								{
									ipaddress.GetAddressBytes().CopyTo(array2, 0);
								}
								else if (long.TryParse(text6.Replace(":", ""), NumberStyles.AllowHexSpecifier, null, out num3))
								{
									array2[0] = (byte)((num3 >> 40) & 255L);
									array2[1] = (byte)((num3 >> 32) & 255L);
									array2[2] = (byte)((num3 >> 24) & 255L);
									array2[3] = (byte)((num3 >> 16) & 255L);
									array2[4] = (byte)((num3 >> 8) & 255L);
									array2[5] = (byte)(num3 & 255L);
								}
								num = icController.IPListControlSet(10, 0, 0, 0, array, null, array2);
								if (num < 0)
								{
									wgAppConfig.wgDebugWrite("IPListUpdate Err=" + num.ToString());
									this.errInfo(num);
									return;
								}
							}
						}
						wgAppConfig.wgDebugWrite("IPListUpdate OK");
						XMessageBox.Show(CommonStr.strSuccessfully);
					}
				}
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00031D08 File Offset: 0x00030D08
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00031D3C File Offset: 0x00030D3C
		private void ipweb_webDisable(ref byte[] snData)
		{
			for (int i = 0; i < snData.Length; i++)
			{
				snData[i] = 0;
			}
			for (int j = 96; j < 192; j++)
			{
				snData[j] = byte.MaxValue;
				snData[1024 + (j >> 3)] = snData[1024 + (j >> 3)] | (byte)(1 << (j & 7));
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00031D9C File Offset: 0x00030D9C
		private void ipweb_webSet()
		{
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			int num = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
			string text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			byte[] array = new byte[1152];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			if (this.bUpdateSuperCard_IPWEB)
			{
				ulong num2 = ulong.MaxValue;
				ulong num3 = ulong.MaxValue;
				ulong.TryParse(this.superCard1_IPWEB, out num2);
				ulong.TryParse(this.superCard2_IPWEB, out num3);
				if (num2 == 0UL)
				{
					num2 = ulong.MaxValue;
				}
				if (num3 == 0UL)
				{
					num3 = ulong.MaxValue;
				}
				wgAppConfig.wgLog("  SN=" + num.ToString() + string.Format("  Super Card1={0},Card2={1}", num2.ToString(), num3.ToString()));
				int num4 = 144;
				array[num4] = (byte)(num2 & 255UL);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 16);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 24);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 32);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 40);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 48);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num2 >> 56);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 & 255UL);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 16);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 24);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 32);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 40);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 48);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num3 >> 56);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
			}
			if (this.bUpdateSpecialCard_IPWEB)
			{
				ulong num5 = ulong.MaxValue;
				ulong num6 = ulong.MaxValue;
				ulong.TryParse(this.SpecialCard1_IPWEB, out num5);
				ulong.TryParse(this.SpecialCard2_IPWEB, out num6);
				wgAppConfig.wgLog("  SN=" + num.ToString() + string.Format("  Special Card1={0},Card2={1}", num5.ToString(), num6.ToString()));
				if (num5 == 0UL)
				{
					num5 = ulong.MaxValue;
				}
				if (num6 == 0UL)
				{
					num6 = ulong.MaxValue;
				}
				int num4 = 160;
				array[num4] = (byte)(num5 & 255UL);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 16);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 24);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 32);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 40);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 48);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num5 >> 56);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 & 255UL);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 16);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 24);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 32);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 40);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 48);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num6 >> 56);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
			}
			if (this.bUpdateWEBConfigure)
			{
				int num7 = 12288;
				if (wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
				{
					num7 = 8192;
				}
				int num8;
				if (!this.bWEBEnabled)
				{
					num8 = 0;
				}
				else
				{
					num8 = this.HttpPort;
					switch (int.Parse(this.strWEBLanguage1))
					{
					case 0:
						num7 = 8192;
						break;
					case 1:
						num7 = 12288;
						break;
					case 2:
						num7 = 229376;
						break;
					default:
						num7 = 12288;
						break;
					}
				}
				int num4 = 100;
				array[num4] = (byte)(num7 & 255);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num7 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num7 >> 16);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num7 >> 24);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4 = 96;
				array[num4] = (byte)(num8 & 255);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4++;
				array[num4] = (byte)(num8 >> 8);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4 = 98;
				array[num4] = (byte)(this.webDateFormat & 255);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
				num4 = 99;
				array[num4] = (byte)((this.bWebOnlyQuery ? 165 : 0) & 255);
				array[1024 + (num4 >> 3)] = array[1024 + (num4 >> 3)] | (byte)(1 << (num4 & 7));
			}
			if (this.bUpdateSuperCard_IPWEB || this.bUpdateWEBConfigure || this.bUpdateSpecialCard_IPWEB)
			{
				using (icController icController = new icController())
				{
					icController.ControllerSN = num;
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					icController.UpdateConfigureCPUSuperIP(array, "", text);
					wgAppConfig.wgLog("  SN=" + icController.ControllerSN + string.Format(" WEB Language={0}", this.strWEBLanguage1.ToString()));
				}
			}
			wgAppConfig.wgLog(this.btnIPAndWebConfigure.Text + "  SN=" + num);
			if (this.bUpdateWEBConfigure && this.bWEBEnabled && int.Parse(this.strWEBLanguage1) == 2)
			{
				byte[] array2 = new byte[4096];
				byte b = 0;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = b;
				}
				int num9 = 229376;
				int num10 = 230400;
				this.dv = new DataView(this.dtWebStringAdvanced_IPWEB);
				string text2 = this.dv[0][2].ToString();
				for (int k = 0; k <= this.dv.Count - 1; k++)
				{
					array2[num9 - 229376] = (byte)(num10 & 255);
					array2[num9 - 229376 + 1] = (byte)(num10 >> 8);
					array2[num9 - 229376 + 2] = (byte)(num10 >> 16);
					array2[num9 - 229376 + 3] = (byte)(num10 >> 24);
					num9 += 4;
					string text3 = wgTools.SetObjToStr(this.dv[k][2]).Trim();
					byte[] bytes = Encoding.GetEncoding(text2).GetBytes(text3);
					num10 = num10 + bytes.Length + 1;
				}
				num9 = 230400;
				for (int l = 0; l <= this.dv.Count - 1; l++)
				{
					string text3 = wgTools.SetObjToStr(this.dv[l][2]).Trim();
					byte[] bytes2 = Encoding.GetEncoding(text2).GetBytes(text3);
					for (int m = 0; m < bytes2.Length; m++)
					{
						array2[num9 - 229376 + m] = bytes2[m];
					}
					num9 = num9 + bytes2.Length + 1;
				}
				wgUdpComm wgUdpComm = null;
				try
				{
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
					wgpacketSSI_FLASH.type = 33;
					wgpacketSSI_FLASH.code = 48;
					wgpacketSSI_FLASH.iDevSnFrom = 0U;
					wgpacketSSI_FLASH.iDevSnTo = (uint)num;
					wgpacketSSI_FLASH.iCallReturn = 0;
					wgpacketSSI_FLASH.ucData = new byte[1024];
					IPAddress ipaddress;
					if (IPAddress.TryParse(text, out ipaddress))
					{
						wgUdpComm = new wgUdpComm(IPAddress.Parse(text));
					}
					else
					{
						wgUdpComm = new wgUdpComm();
					}
					Thread.Sleep(300);
					wgpacketSSI_FLASH.iStartFlashAddr = 8331264U;
					wgpacketSSI_FLASH.iEndFlashAddr = 8335359U;
					for (int n = 0; n < 1024; n++)
					{
						wgpacketSSI_FLASH.ucData[n] = byte.MaxValue;
					}
					byte[] array3 = null;
					while (wgpacketSSI_FLASH.iStartFlashAddr <= wgpacketSSI_FLASH.iEndFlashAddr)
					{
						for (int num11 = 0; num11 < 1024; num11++)
						{
							wgpacketSSI_FLASH.ucData[num11] = array2[(int)((IntPtr)((long)((ulong)(wgpacketSSI_FLASH.iStartFlashAddr - 8331264U) + (ulong)((long)num11))))];
						}
						wgUdpComm.udp_get_notries(wgpacketSSI_FLASH.ToBytes(wgUdpComm.udpPort), 300, wgpacketSSI_FLASH.xid, null, 60000, ref array3);
						if (array3 == null)
						{
							wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
						}
						wgpacketSSI_FLASH.iStartFlashAddr += 1024U;
					}
					wgUdpComm.Close();
					wgAppConfig.wgLog(this.btnIPAndWebConfigure.Text + "  SN=" + num.ToString() + "  OtherLanguage");
				}
				catch (Exception)
				{
				}
				finally
				{
					if (wgUdpComm != null)
					{
						wgUdpComm.Dispose();
					}
				}
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00032BB8 File Offset: 0x00031BB8
		public bool isExisted(string sn, string ip)
		{
			bool flag = false;
			try
			{
				if (this.dgvFoundControllers.Rows.Count <= 0)
				{
					return flag;
				}
				for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
				{
					if (sn == this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString() && ip == this.dgvFoundControllers.Rows[i].Cells[7].Value.ToString())
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
			return flag;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00032C80 File Offset: 0x00031C80
		private void limit2HourOutForSwimmingPoolToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			string text = "";
			int num = 0;
			string text2 = "";
			int num2 = 0;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.antiback_validtime = 130;
				int num3 = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					num3++;
					this.lblSearchNow.Text = icController.ControllerSN.ToString();
					this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
					this.lblCount.Text = num3.ToString();
					this.toolStripStatusLabel1.Text = num3.ToString();
					Application.DoEvents();
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						text = text + icController.ControllerSN.ToString() + ",";
						num++;
					}
					else
					{
						text2 = text2 + icController.ControllerSN.ToString() + ",";
						num2++;
					}
					Application.DoEvents();
				}
			}
			Cursor.Current = Cursors.Default;
			if (!string.IsNullOrEmpty(text2))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					"",
					CommonStr.strFailed,
					num2,
					text2
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strFailed,
					num2,
					text2
				}));
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					"",
					CommonStr.strSuccessfully,
					num,
					text
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strSuccessfully,
					num,
					text
				}));
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00032F74 File Offset: 0x00031F74
		private void multicardSwipeGapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			this.strIPListControllerSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			dataGridViewRow.Cells["pcMacAddr"].Value.ToString();
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
			{
				return;
			}
			int num = 10;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripItem).Text;
				dfrmInputNewName.strNewName = "10";
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(dfrmInputNewName.strNewName))
				{
					return;
				}
				bool flag = true;
				if (!int.TryParse(dfrmInputNewName.strNewName.Trim(), out num))
				{
					flag = false;
				}
				if (num < 10)
				{
					flag = false;
				}
				if (num > 60)
				{
					flag = false;
				}
				if (!flag)
				{
					XMessageBox.Show(this, CommonStr.strInvalidValue + "\r\n\r\n10 - 60", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			using (icController icController = new icController())
			{
				icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				if (icController.UpdateConfigureIP(new wgMjControllerConfigure
				{
					twoCardReadTimeout = num * 10
				}, -1) == 1)
				{
					wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
					XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
					{
						icController.ControllerSN,
						sender.ToString(),
						CommonStr.strSuccessfully,
						CommonStr.strRebootController
					}));
				}
				else
				{
					XMessageBox.Show(string.Format("{0}: {1} -- {2}", icController.ControllerSN, sender.ToString(), CommonStr.strFailed));
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000331F8 File Offset: 0x000321F8
		private void otherToolsToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000331FC File Offset: 0x000321FC
		private void pwd468CheckV890above_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			int num = 0;
			if (sender == this.pwd468CheckV890aboveDeactive)
			{
				num = 0;
			}
			else if (sender == this.pwd468CheckV890above4)
			{
				num = 31;
			}
			else if (sender == this.pwd468CheckV890above6)
			{
				num = 47;
			}
			else
			{
				if (sender != this.pwd468CheckV890above8)
				{
					if (sender == this.pwd468CheckV890aboveCustom)
					{
						num = 0;
						using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
						{
							dfrmInputNewName.Text = (sender as ToolStripItem).Text;
							if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dfrmInputNewName.strNewName))
							{
								int num2 = 0;
								if (int.TryParse(dfrmInputNewName.strNewName, NumberStyles.AllowHexSpecifier, null, out num2))
								{
									if (true)
									{
										num = num2;
										goto IL_00DB;
									}
									XMessageBox.Show(CommonStr.strInvalidValue);
								}
							}
						}
					}
					return;
				}
				num = 63;
			}
			IL_00DB:
			string text = "";
			int num3 = 0;
			string text2 = "";
			int num4 = 0;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.pwd468_Check = num;
				int num5 = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					num5++;
					this.lblSearchNow.Text = icController.ControllerSN.ToString();
					this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
					this.lblCount.Text = num5.ToString();
					this.toolStripStatusLabel1.Text = num5.ToString();
					Application.DoEvents();
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						text = text + icController.ControllerSN.ToString() + ",";
						num3++;
					}
					else
					{
						text2 = text2 + icController.ControllerSN.ToString() + ",";
						num4++;
					}
					Application.DoEvents();
				}
			}
			Cursor.Current = Cursors.Default;
			if (!string.IsNullOrEmpty(text2))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					num,
					CommonStr.strFailed,
					num4,
					text2
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strFailed,
					num4,
					text2
				}));
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					num,
					CommonStr.strSuccessfully,
					num3,
					text
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strSuccessfully,
					num3,
					text
				}));
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000335D0 File Offset: 0x000325D0
		private void quickFormatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
			string text = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
			string text2 = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
			string text3 = "";
			if (XMessageBox.Show(this, sender.ToString() + " " + text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				Cursor.Current = Cursors.WaitCursor;
				int num = 60000;
				int num2 = 0;
				try
				{
					if (this.wgudp != null)
					{
						this.wgudp = null;
					}
					IPAddress ipaddress;
					if (IPAddress.TryParse(text2, out ipaddress))
					{
						this.wgudp = new wgUdpComm(IPAddress.Parse(text2));
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
					WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
					wgpacketSSI_FLASH.type = 33;
					wgpacketSSI_FLASH.code = 48;
					wgpacketSSI_FLASH.iDevSnFrom = 0U;
					wgpacketSSI_FLASH.iDevSnTo = uint.Parse(text);
					wgpacketSSI_FLASH.iCallReturn = 0;
					wgpacketSSI_FLASH.ucData = new byte[1024];
					wgpacketSSI_FLASH.iStartFlashAddr = 5017600U;
					wgpacketSSI_FLASH.iEndFlashAddr = wgpacketSSI_FLASH.iStartFlashAddr + 1024U - 1U;
					for (int i = 0; i < 1024; i++)
					{
						wgpacketSSI_FLASH.ucData[i] = byte.MaxValue;
					}
					byte[] array = null;
					while (wgpacketSSI_FLASH.iStartFlashAddr <= 5025792U)
					{
						num2 = this.wgudp.udp_get(wgpacketSSI_FLASH.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH.xid, text3, num, ref array);
						if (num2 < 0)
						{
							break;
						}
						wgpacketSSI_FLASH.iStartFlashAddr += 1024U;
					}
					if (num2 >= 0)
					{
						using (icController icController = new icController())
						{
							icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
							if (num2 >= 0)
							{
								num2 = icController.RestoreAllSwipeInTheControllersIP();
							}
							if (num2 >= 0)
							{
								num2 = icController.UpdateFRamIP(9U, 0U);
							}
							if (num2 >= 0)
							{
								using (icPrivilege icPrivilege = new icPrivilege())
								{
									num2 = icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT);
								}
							}
							if (num2 >= 0 && !WGPacket.bCommP)
							{
								byte[] array2 = new byte[1152];
								this.ipweb_webDisable(ref array2);
								for (int j = 64; j < 80; j++)
								{
									array2[j] = byte.MaxValue;
									array2[1024 + (j >> 3)] = array2[1024 + (j >> 3)] | (byte)(1 << (j & 7));
								}
								if (IPAddress.TryParse(text2, out ipaddress))
								{
									icController.UpdateConfigureCPUSuperIP(array2, "", text2);
								}
								else
								{
									icController.UpdateConfigureCPUSuperIP(array2, "");
								}
							}
							if (num2 >= 0)
							{
								icController.RestoreDefaultConfigureIP();
							}
						}
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				Cursor.Current = Cursors.Default;
				wgAppConfig.wgLog(sender.ToString() + "  SN=" + text);
				if (num2 >= 0)
				{
					XMessageBox.Show(string.Format("{0}: {1} -- {2}", text, sender.ToString(), CommonStr.strSuccessfully));
					return;
				}
				XMessageBox.Show(string.Format("{0}: {1} -- {2}", text, sender.ToString(), CommonStr.strFailed));
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000339EC File Offset: 0x000329EC
		private void restoreAllSwipesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (icController icController = new icController())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				icController.RestoreAllSwipeInTheControllersIP();
				wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
				XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
				{
					icController.ControllerSN,
					sender.ToString(),
					CommonStr.strSuccessfully,
					CommonStr.strRebootController
				}));
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00033B14 File Offset: 0x00032B14
		private void restoreDefaultIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnDefault_Click(sender, e);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00033B20 File Offset: 0x00032B20
		private void restoreDefaultParamToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (icController icController = new icController())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				icController.RestoreDefaultConfigureIP();
				wgAppConfig.wgLog(sender.ToString() + "  SN=" + icController.ControllerSN);
				XMessageBox.Show(string.Format("{0}: {1} -- {2}\r\n{3}", new object[]
				{
					icController.ControllerSN,
					sender.ToString(),
					CommonStr.strSuccessfully,
					CommonStr.strRebootController
				}));
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00033C2C File Offset: 0x00032C2C
		private void rS232ConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgMjControllerConfigure wgMjControllerConfigure = null;
			using (dfrmRs232Config dfrmRs232Config = new dfrmRs232Config())
			{
				if (dfrmRs232Config.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				wgMjControllerConfigure = dfrmRs232Config.controlConfigure;
				bool bsm4pwd = dfrmRs232Config.bsm4pwd;
				string wgsm4pwd = dfrmRs232Config.wgsm4pwd;
			}
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				string text = "";
				int num = 0;
				string text2 = "";
				int num2 = 0;
				Cursor.Current = Cursors.WaitCursor;
				using (icController icController = new icController())
				{
					int num3 = 0;
					for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
						icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						num3++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num3.ToString();
						this.toolStripStatusLabel1.Text = num3.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text = text + icController.ControllerSN.ToString() + ",";
							icController.RebootControllerIP();
							num++;
						}
						else
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num2++;
						}
						Application.DoEvents();
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text2))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strFailed,
						num2,
						text2
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num2,
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strSuccessfully,
						num,
						text
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num,
						text
					}));
				}
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00033F68 File Offset: 0x00032F68
		private void search100FromTheSpecialSNToolStripMenuItem_Click(object sender, EventArgs e)
		{
			uint num = 0U;
			uint num2 = 0U;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripItem).Text;
				dfrmInputNewName.label1.Text = CommonStr.strControllerSN;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || !uint.TryParse(dfrmInputNewName.strNewName, out num) || wgMjController.GetControllerType((int)num) == 0)
				{
					return;
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			this.btnConfigure.Enabled = false;
			this.btnSearch.Enabled = false;
			Thread.Sleep(100);
			this.Refresh();
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iCallReturn = 0;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			num2 = num + 100U - 1U;
			if (sender == this.searchSpecialSNToolStripMenuItem)
			{
				num2 = num + 1U - 1U;
			}
			this.wgudp = null;
			while (num <= num2)
			{
				if ((num2 - num) % 5U == 0U)
				{
					this.lblSearchNow.Text = num.ToString();
					this.toolStripStatusLabel2.Text = num.ToString();
					this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
					this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
					this.Refresh();
					Application.DoEvents();
					Cursor.Current = Cursors.WaitCursor;
				}
				wgpacket.iDevSnTo = num;
				num += 1U;
				if (WGPacket.bCommP)
				{
					foreach (NetworkInterface networkInterface in allNetworkInterfaces)
					{
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (!unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation.Address.ToString());
									if (this.wgudp == null)
									{
										this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
										Thread.Sleep(300);
									}
									else if (this.wgudp.localIP.ToString() != unicastIPAddressInformation.Address.ToString())
									{
										this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
										Thread.Sleep(300);
									}
									byte[] array2 = wgpacket.ToBytesNoPassword(this.wgudp.udpPort);
									if (array2 == null)
									{
										return;
									}
									byte[] array3 = null;
									this.wgudp.udp_get(array2, 300, 0U, null, 60000, ref array3);
									if (array3 != null && !this.isExisted(wgpacket.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
									{
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array3,
											unicastIPAddressInformation.Address.ToString(),
											networkInterface.GetPhysicalAddress().ToString(),
											"0"
										});
										long ticks = DateTime.Now.Ticks;
									}
								}
							}
							Console.WriteLine();
						}
					}
				}
				foreach (NetworkInterface networkInterface2 in allNetworkInterfaces)
				{
					UnicastIPAddressInformationCollection unicastAddresses2 = networkInterface2.GetIPProperties().UnicastAddresses;
					if (unicastAddresses2.Count > 0)
					{
						Console.WriteLine(networkInterface2.Description);
						foreach (UnicastIPAddressInformation unicastIPAddressInformation2 in unicastAddresses2)
						{
							if (!unicastIPAddressInformation2.Address.IsIPv6LinkLocal && unicastIPAddressInformation2.Address.ToString() != "127.0.0.1")
							{
								Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation2.Address.ToString());
								if (this.wgudp == null)
								{
									this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
									Thread.Sleep(300);
								}
								else if (this.wgudp.localIP.ToString() != unicastIPAddressInformation2.Address.ToString())
								{
									this.wgudp = new wgUdpComm(unicastIPAddressInformation2.Address);
									Thread.Sleep(300);
								}
								byte[] array5 = wgpacket.ToBytes(this.wgudp.udpPort);
								if (array5 == null)
								{
									return;
								}
								byte[] array6 = null;
								this.wgudp.udp_get(array5, 300, uint.MaxValue, null, 60000, ref array6);
								if (array6 != null && !this.isExisted(wgpacket.iDevSnTo.ToString(), this.wgudp.localIP.ToString()))
								{
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
									{
										array6,
										unicastIPAddressInformation2.Address.ToString(),
										networkInterface2.GetPhysicalAddress().ToString(),
										WGPacket.bCommP ? "1" : "0"
									});
								}
							}
						}
						Console.WriteLine();
					}
				}
			}
			this.btnSearch.Enabled = true;
			if (this.dgvFoundControllers.Rows.Count > 0)
			{
				this.btnConfigure.Enabled = true;
			}
			this.lblSearchNow.Text = num2.ToString();
			this.toolStripStatusLabel2.Text = num2.ToString();
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000345C4 File Offset: 0x000335C4
		private void setAlarmOffDelay30SecDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			if (XMessageBox.Show(this, sender.ToString() + " " + this.strIPListControllerSN + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.Cancel)
			{
				string text = "";
				int num = 0;
				string text2 = "";
				int num2 = 0;
				Cursor.Current = Cursors.WaitCursor;
				using (icController icController = new icController())
				{
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
					wgMjControllerConfigure.ext_SetAlarmOffDelay = 30;
					if (sender == this.setAlarmOffDelay60SecToolStripMenuItem)
					{
						wgMjControllerConfigure.ext_SetAlarmOffDelay = 60;
					}
					int num3 = 0;
					for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
						icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						num3++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num3.ToString();
						this.toolStripStatusLabel1.Text = num3.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text = text + icController.ControllerSN.ToString() + ",";
							num++;
						}
						else
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num2++;
						}
						Application.DoEvents();
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text2))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						"",
						CommonStr.strFailed,
						num2,
						text2
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num2,
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						"",
						CommonStr.strSuccessfully,
						num,
						text
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num,
						text
					}));
				}
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000348F4 File Offset: 0x000338F4
		private void setCustType()
		{
			int num = 0;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = "请输入产品信息:";
				dfrmInputNewName.strNewName = "";
				dfrmInputNewName.setPasswordChar('*');
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(dfrmInputNewName.strNewName))
				{
					return;
				}
				bool flag = true;
				if (!int.TryParse(dfrmInputNewName.strNewName.Trim(), out num))
				{
					flag = false;
				}
				if (dfrmInputNewName.strNewName.Length != 8)
				{
					flag = false;
				}
				if (num < 0)
				{
					flag = false;
				}
				if (!flag)
				{
					XMessageBox.Show(this, CommonStr.strInvalidValue + "\r\n\r\n00000000 - 99999999", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (num == 0)
			{
				wgTools.gCustomProductType = "";
				wgAppConfig.UpdateKeyVal("KEY_CUSTOMTYPE", wgTools.gCustomProductType);
				if (XMessageBox.Show(this, CommonStr.strUpdateSuccessfully, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
				{
					wgAppConfig.gRestart = true;
					((frmADCT3000)base.Owner).mnuExit.PerformClick();
					return;
				}
			}
			else
			{
				uint num2 = wgCRC.calFRamCrc4Bit((uint)num);
				num2 = (num2 & 255U) + (num2 >> 24 << 8);
				uint num3 = num2;
				if (((num3 + 4U) & 255U) < 10U)
				{
					num3 = ((num3 + 4U) & 255U) | 16U;
				}
				else
				{
					num3 &= 255U;
				}
				wgTools.gCustomProductType = WGPacket.Ept((num3 + (num2 & 65280U)).ToString());
				wgTools.gPTC = wgTools.gCustomProductType;
				wgAppConfig.UpdateKeyVal("KEY_CUSTOMTYPE", wgTools.gCustomProductType);
				wgAppConfig.wgLogWithoutDB(string.Format("KEY_CUSTOMTYPE={0}", wgTools.gCustomProductType), EventLogEntryType.Information, null);
				XMessageBox.Show("OK");
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00034A9C File Offset: 0x00033A9C
		private void setFunctionQ5678()
		{
			this.btnDefault.Visible = true;
			this.restoreDefaultParamToolStripMenuItem.Visible = true;
			this.restoreAllSwipesToolStripMenuItem.Visible = true;
			this.clearSwipesToolStripMenuItem.Visible = true;
			this.setSpeacialToolStripMenuItem.Visible = true;
			this.search100FromTheSpecialSNToolStripMenuItem.Visible = true;
			this.otherToolStripMenuItem.Visible = true;
			this.otherToolsToolStripMenuItem.Visible = true;
			this.force100MV552AboveToolStripMenuItem.Visible = true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00034B18 File Offset: 0x00033B18
		private void setSpeacialToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (sender == this.setSpeacialToolStripMenuItem)
			{
				using (dfrmReaderConfig dfrmReaderConfig = new dfrmReaderConfig())
				{
					dfrmReaderConfig.readerFormat = this.readerFormatLast;
					if (dfrmReaderConfig.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(dfrmReaderConfig.readerFormat))
					{
						return;
					}
					string[] array = dfrmReaderConfig.readerFormat.Split(new char[] { ',' });
					bool flag = true;
					if (array.Length != 3)
					{
						flag = false;
					}
					if (flag)
					{
						if (!int.TryParse(array[0].Trim(), out num))
						{
							flag = false;
						}
						if (!int.TryParse(array[1].Trim(), out num2))
						{
							flag = false;
						}
						if (!int.TryParse(array[2].Trim(), out num3))
						{
							flag = false;
						}
					}
					if (!flag)
					{
						XMessageBox.Show(CommonStr.strInvalidValue);
						return;
					}
					this.readerFormatLast = dfrmReaderConfig.readerFormat;
					goto IL_018F;
				}
			}
			if (sender == this.setReaderFormatWG26NoCheckToolStripMenuItem)
			{
				num = 26;
				num2 = 2;
				num3 = 24;
				this.readerFormatLast = "";
			}
			else if (sender == this.setReaderFormat35BitsToolStripMenuItem)
			{
				num = 35;
				num2 = 11;
				num3 = 24;
				this.readerFormatLast = "";
			}
			else if (sender == this.setReaderFormatHID36BitsToolStripMenuItem)
			{
				num = 36;
				num2 = 16;
				num3 = 56;
				this.readerFormatLast = "";
			}
			else if (sender == this.setReaderFormatWG26FromWG34ToolStripMenuItem)
			{
				num = 34;
				num2 = 10;
				num3 = 24;
				this.readerFormatLast = "";
			}
			else
			{
				num = 0;
				num2 = 0;
				num3 = 0;
				this.readerFormatLast = "";
			}
			IL_018F:
			string text = "";
			int num4 = 0;
			string text2 = "";
			int num5 = 0;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.custom_cardformat_totalbits = num;
				wgMjControllerConfigure.custom_cardformat_startloc = num2;
				wgMjControllerConfigure.custom_cardformat_validbits = num3;
				wgMjControllerConfigure.custom_cardformat_sumcheck = (wgMjControllerConfigure.custom_cardformat_totalbits + wgMjControllerConfigure.custom_cardformat_startloc + wgMjControllerConfigure.custom_cardformat_validbits) & 255;
				int num6 = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
					num6++;
					this.lblSearchNow.Text = icController.ControllerSN.ToString();
					this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
					this.lblCount.Text = num6.ToString();
					this.toolStripStatusLabel1.Text = num6.ToString();
					Application.DoEvents();
					if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
					{
						text = text + icController.ControllerSN.ToString() + ",";
						num4++;
					}
					else
					{
						text2 = text2 + icController.ControllerSN.ToString() + ",";
						num5++;
					}
					Application.DoEvents();
				}
			}
			Cursor.Current = Cursors.Default;
			if (!string.IsNullOrEmpty(text2))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					this.readerFormatLast,
					CommonStr.strFailed,
					num5,
					text2
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strFailed,
					num5,
					text2
				}));
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					this.readerFormatLast,
					CommonStr.strSuccessfully,
					num4,
					text
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strSuccessfully,
					num4,
					text
				}));
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00034FD4 File Offset: 0x00033FD4
		private void sMSConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (icOperator.OperatorID != 1)
			{
				XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (dfrmSMSConfig dfrmSMSConfig = new dfrmSMSConfig())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				dfrmSMSConfig.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
				dfrmSMSConfig.Text = string.Format("{0}  [{1}]", dfrmSMSConfig.Text, dfrmSMSConfig.ControllerSN);
				if (dfrmSMSConfig.ShowDialog() == DialogResult.OK)
				{
					wgAppConfig.wgLog(sender.ToString() + "  SN=" + int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString()));
					XMessageBox.Show(string.Format("{0}: {1} -- {2}", int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString()), sender.ToString(), CommonStr.strSuccessfully));
				}
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00035118 File Offset: 0x00034118
		private void toolStripStatusLabel2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0003511C File Offset: 0x0003411C
		private void twoCardCheckV885AboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			int num = 0;
			if (sender == this.twoCardCheckRestoreDefaultToolStripMenuItem)
			{
				num = 0;
			}
			else if (sender == this.twoCardCheckOneByOneToolStripMenuItem)
			{
				num = 1;
			}
			else
			{
				if (sender != this.twoCardCheckMoreToolStripMenuItem)
				{
					if (sender == this.twoCardCheckCustomToolStripMenuItem)
					{
						num = 0;
						using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
						{
							dfrmInputNewName.Text = (sender as ToolStripItem).Text;
							if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dfrmInputNewName.strNewName))
							{
								int num2 = 0;
								if (int.TryParse(dfrmInputNewName.strNewName, NumberStyles.AllowHexSpecifier, null, out num2))
								{
									if (true)
									{
										num = num2;
										goto IL_00C8;
									}
									XMessageBox.Show(CommonStr.strInvalidValue);
								}
							}
						}
					}
					return;
				}
				num = 6;
			}
			IL_00C8:
			string text = "";
			int num3 = 0;
			string text2 = "";
			int num4 = 0;
			Cursor.Current = Cursors.WaitCursor;
			using (icController icController = new icController())
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure.check_two_cards = num;
				int num5 = 0;
				for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
					icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
					if (wgMjController.GetControllerType(icController.ControllerSN) < 4)
					{
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						num5++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num5.ToString();
						this.toolStripStatusLabel1.Text = num5.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text = text + icController.ControllerSN.ToString() + ",";
							num3++;
						}
						else
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num4++;
						}
						Application.DoEvents();
					}
				}
			}
			Cursor.Current = Cursors.Default;
			if (!string.IsNullOrEmpty(text2))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					num,
					CommonStr.strFailed,
					num4,
					text2
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strFailed,
					num4,
					text2
				}));
			}
			if (!string.IsNullOrEmpty(text))
			{
				wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
				{
					sender.ToString(),
					num,
					CommonStr.strSuccessfully,
					num3,
					text
				}));
				XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
				{
					sender.ToString(),
					CommonStr.strSuccessfully,
					num3,
					text
				}));
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000354F0 File Offset: 0x000344F0
		private void wGQRReaderConfigureV892AboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgMjControllerConfigure wgMjControllerConfigure = null;
			using (dfrmReaderQRConfig dfrmReaderQRConfig = new dfrmReaderQRConfig())
			{
				if (dfrmReaderQRConfig.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				wgMjControllerConfigure = dfrmReaderQRConfig.controlConfigure;
				bool bsm4pwd = dfrmRs232Config.bsm4pwd;
				string wgsm4pwd = dfrmRs232Config.wgsm4pwd;
			}
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				string text = "";
				int num = 0;
				string text2 = "";
				int num2 = 0;
				Cursor.Current = Cursors.WaitCursor;
				using (icController icController = new icController())
				{
					int num3 = 0;
					for (int i = 0; i < this.dgvFoundControllers.SelectedRows.Count; i++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[i];
						icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						num3++;
						this.lblSearchNow.Text = icController.ControllerSN.ToString();
						this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
						this.lblCount.Text = num3.ToString();
						this.toolStripStatusLabel1.Text = num3.ToString();
						Application.DoEvents();
						if (icController.UpdateConfigureIP(wgMjControllerConfigure, -1) == 1)
						{
							text = text + icController.ControllerSN.ToString() + ",";
							num++;
						}
						else
						{
							text2 = text2 + icController.ControllerSN.ToString() + ",";
							num2++;
						}
						Application.DoEvents();
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text2))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strFailed,
						num2,
						text2
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num2,
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strSuccessfully,
						num,
						text
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num,
						text
					}));
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00035824 File Offset: 0x00034824
		private void wIFIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "";
			string text2 = "";
			bool flag = false;
			using (dfrmWIFIConfig dfrmWIFIConfig = new dfrmWIFIConfig())
			{
				if (dfrmWIFIConfig.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmWIFIConfig.strSSID;
				text2 = dfrmWIFIConfig.strPassword;
				if (dfrmWIFIConfig.bWIFIDisable)
				{
					flag = true;
					wgAppConfig.wgLog(string.Format("SSID={0}, {1}, {2}", "STOP999\r\n", "STOPWIFI", " Stop WIFI"));
				}
				else if (text2.Length > 4)
				{
					wgAppConfig.wgLog(string.Format("SSID={0}, ****{1}, {2}", text, text2.Substring(4), Program.Ept4Database(text2)));
				}
				else
				{
					wgAppConfig.wgLog(string.Format("SSID={0}, {1}", text, text2));
				}
			}
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				byte[] array = new byte[1152];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
				int num = 192;
				byte[] array2 = new byte[1];
				if (!string.IsNullOrEmpty(text))
				{
					array2 = Encoding.ASCII.GetBytes(text);
				}
				if (flag)
				{
					array2 = Encoding.ASCII.GetBytes("STOP999\r\n");
				}
				for (int j = 0; j < 32; j++)
				{
					if (j < array2.Length)
					{
						array[j + num] = array2[j];
					}
					array[1024 + (num + j >> 3)] = array[1024 + (num + j >> 3)] | (byte)(1 << ((num + j) & 7));
				}
				num = 224;
				if (!string.IsNullOrEmpty(text2))
				{
					array2 = Encoding.ASCII.GetBytes(text2);
				}
				else
				{
					array2 = new byte[1];
				}
				if (flag)
				{
					array2 = Encoding.ASCII.GetBytes("STOPWIFI");
				}
				for (int k = 0; k < 16; k++)
				{
					if (k < array2.Length)
					{
						array[k + num] = array2[k];
					}
					array[1024 + (num + k >> 3)] = array[1024 + (num + k >> 3)] | (byte)(1 << ((num + k) & 7));
				}
				for (int l = 64; l < 80; l++)
				{
					array[l] = byte.MaxValue;
					array[1024 + (l >> 3)] = array[1024 + (l >> 3)] | (byte)(1 << (l & 7));
				}
				string text3 = "";
				int num2 = 0;
				string text4 = "";
				int num3 = 0;
				Cursor.Current = Cursors.WaitCursor;
				using (icController icController = new icController())
				{
					int num4 = 0;
					for (int m = 0; m < this.dgvFoundControllers.SelectedRows.Count; m++)
					{
						Cursor.Current = Cursors.WaitCursor;
						DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[m];
						icController.ControllerSN = int.Parse(dataGridViewRow.Cells["f_ControllerSN"].Value.ToString());
						icController.PCIPAddress = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
						int num5 = icController.ControllerSN / 1000000;
						if (wgAppConfig.IsAccessControlBlue && num5 != 125 && num5 != 225 && num5 != 425)
						{
							wgAppConfig.wgLog(".errWiFiSN_" + icController.ControllerSN.ToString());
							num4++;
							text4 = text4 + icController.ControllerSN.ToString() + ",";
							num3++;
						}
						else
						{
							num4++;
							this.lblSearchNow.Text = icController.ControllerSN.ToString();
							this.toolStripStatusLabel2.Text = icController.ControllerSN.ToString();
							this.lblCount.Text = num4.ToString();
							this.toolStripStatusLabel1.Text = num4.ToString();
							Application.DoEvents();
							if (icController.UpdateConfigureCPUSuperIP(array, "", icController.PCIPAddress) == 1)
							{
								text3 = text3 + icController.ControllerSN.ToString() + ",";
								num2++;
							}
							else
							{
								text4 = text4 + icController.ControllerSN.ToString() + ",";
								num3++;
							}
							Application.DoEvents();
						}
					}
				}
				Cursor.Current = Cursors.Default;
				if (!string.IsNullOrEmpty(text4))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strFailed,
						num3,
						text4
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strFailed,
						num3,
						text4
					}));
				}
				if (!string.IsNullOrEmpty(text3))
				{
					wgAppConfig.wgLog(string.Format("{0} {1}: {2} [{3}] SN= {4}", new object[]
					{
						sender.ToString(),
						this.readerFormatLast,
						CommonStr.strSuccessfully,
						num2,
						text3
					}));
					XMessageBox.Show(string.Format("{0}: {1} [{2}] \r\n {3}", new object[]
					{
						sender.ToString(),
						CommonStr.strSuccessfully,
						num2,
						text3
					}));
				}
			}
		}

		// Token: 0x040002F8 RID: 760
		private bool bAdjustTime;

		// Token: 0x040002F9 RID: 761
		private bool bAutoUploadUsers;

		// Token: 0x040002FA RID: 762
		private bool bInput5678;

		// Token: 0x040002FB RID: 763
		private bool bIPAndWEBConfigure;

		// Token: 0x040002FC RID: 764
		private bool bOption;

		// Token: 0x040002FD RID: 765
		private bool bOptionWeb;

		// Token: 0x040002FE RID: 766
		private bool bUpdateIPConfigure;

		// Token: 0x040002FF RID: 767
		private bool bUpdateSpecialCard_IPWEB;

		// Token: 0x04000300 RID: 768
		private bool bUpdateSuperCard_IPWEB;

		// Token: 0x04000301 RID: 769
		private bool bUpdateWEBConfigure;

		// Token: 0x04000302 RID: 770
		private bool bWEBEnabled;

		// Token: 0x04000303 RID: 771
		private bool bWebOnlyQuery;

		// Token: 0x04000304 RID: 772
		private frmTestController dfrmTest;

		// Token: 0x04000305 RID: 773
		private DataView dv;

		// Token: 0x04000306 RID: 774
		private frmProductFormat frmProductFormat1;

		// Token: 0x04000307 RID: 775
		private int invalidSNofWGYTJ;

		// Token: 0x04000308 RID: 776
		private int webDateFormat;

		// Token: 0x0400030A RID: 778
		private bool bFirstShowInfo = true;

		// Token: 0x0400030B RID: 779
		private int commPort = 60000;

		// Token: 0x0400030C RID: 780
		private string EncryptControllerSN = "";

		// Token: 0x0400030D RID: 781
		private string foundControllerIPs = "";

		// Token: 0x0400030E RID: 782
		private static bool functionQ5678;

		// Token: 0x0400030F RID: 783
		private int HttpPort = 80;

		// Token: 0x04000310 RID: 784
		private string readerFormatLast = "";

		// Token: 0x04000311 RID: 785
		private string SpecialCard1_IPWEB = "";

		// Token: 0x04000312 RID: 786
		private string SpecialCard2_IPWEB = "";

		// Token: 0x04000313 RID: 787
		private string strControllers = "";

		// Token: 0x04000314 RID: 788
		private string strGateway_IPWEB = "";

		// Token: 0x04000315 RID: 789
		private string strIP_IPWEB = "";

		// Token: 0x04000316 RID: 790
		private string strIPList = "";

		// Token: 0x04000317 RID: 791
		private string strIPListControllerSN = "";

		// Token: 0x04000318 RID: 792
		private string strIPListCurrentPassword = "";

		// Token: 0x04000319 RID: 793
		private string strIPListNewPassword = "";

		// Token: 0x0400031A RID: 794
		private string strNETMASK_IPWEB = "";

		// Token: 0x0400031B RID: 795
		private string strSelectedFile1 = "";

		// Token: 0x0400031C RID: 796
		private string strSelectedFile2 = "";

		// Token: 0x0400031D RID: 797
		private string strWEBLanguage1 = "";

		// Token: 0x0400031E RID: 798
		private string strWEBLanguage2 = "";

		// Token: 0x0400031F RID: 799
		private string superCard1_IPWEB = "";

		// Token: 0x04000320 RID: 800
		private string superCard2_IPWEB = "";

		// Token: 0x04000344 RID: 836
		private DataTable dtWebStringAdvanced_IPWEB;

		// Token: 0x0200001B RID: 27
		// (Invoke) Token: 0x06000187 RID: 391
		public delegate void AddTolstDiscoveredDevices(object o, object pcIP, object pcMac, object bPassword);

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x0600018B RID: 395
		public delegate void AsyncCallback(IAsyncResult ar);
	}
}
