using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.Cloud2017;
using WG3000_COMM.ExtendFunc.Reader;
using WG3000_COMM.ExtendFunc.SMS;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WG3000_COMM.Security;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000004 RID: 4
	public partial class dfrmBLELockSearch : frmN3000
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002CE4 File Offset: 0x00001CE4
		public dfrmBLELockSearch()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvFoundControllers);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002E64 File Offset: 0x00001E64
		public void AddDiscoveryEntry(object o, object pcIP, object pcMac, object bPassword)
		{
			byte[] array = (byte[])o;
			string text = (string)pcIP;
			string text2 = (string)pcMac;
			bool flag = (string)bPassword == "1";
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			long num = (long)dfrmBLELockSearch.getSNSearch(intPtr);
			Marshal.FreeHGlobal(intPtr);
			if (num < -2L)
			{
				wgAppConfig.wgLog(".errSN_" + num.ToString() + "____" + BitConverter.ToString(array).Replace("-", ""));
				return;
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

		// Token: 0x06000017 RID: 23 RVA: 0x00003234 File Offset: 0x00002234
		public void AddDiscoveryLockEntry(object data, object len)
		{
			bool flag = false;
			if (this.dgvFoundControllers.CurrentCell == null)
			{
				flag = true;
			}
			else if (this.dgvFoundControllers.CurrentCell.RowIndex == this.dgvFoundControllers.Rows.Count - 1)
			{
				flag = true;
			}
			int num = (int)len;
			byte[] array = (byte[])data;
			this.ilogo++;
			this.runLogo = " " + this.allLogo[this.ilogo & 7];
			int num2 = 0;
			while (num2 < num && !this.bExit)
			{
				if (num2 + 8 <= num)
				{
					string text = string.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}", new object[]
					{
						array[num2 + 5],
						array[num2 + 4],
						array[num2 + 3],
						array[num2 + 2],
						array[num2 + 1],
						array[num2]
					});
					string text2 = string.Format("WG_LOCK_{3:X2}{4:X2}{5:X2}", new object[]
					{
						array[num2 + 5],
						array[num2 + 4],
						array[num2 + 3],
						array[num2 + 2],
						array[num2 + 1],
						array[num2]
					});
					if (array[num2 + 7] == 2)
					{
						text2 = string.Format("WG_READ_{3:X2}{4:X2}{5:X2}", new object[]
						{
							array[num2 + 5],
							array[num2 + 4],
							array[num2 + 3],
							array[num2 + 2],
							array[num2 + 1],
							array[num2]
						});
						if (!this.chkReader.Checked)
						{
							goto IL_03E8;
						}
					}
					else if (!this.chkLock.Checked)
					{
						goto IL_03E8;
					}
					int num3 = (int)array[num2 + 6];
					if ((num3 & 128) > 0)
					{
						num3 = -(256 - num3);
					}
					if (this.arrMacExclude.Count <= 0 || this.arrMacExclude.IndexOf(text) < 0)
					{
						string[] array2 = new string[]
						{
							(this.dgvFoundControllers.Rows.Count + 1).ToString().PadLeft(4, '0'),
							text2,
							text,
							num3.ToString(),
							(array[num2 + 7] == 1).ToString(),
							"",
							"",
							"",
							DateTime.Now.ToString("HH:mm:ss"),
							this.runLogo
						};
						bool flag2 = false;
						if (this.dgvFoundControllers.Rows.Count > 0)
						{
							for (int i = 0; i < this.dgvFoundControllers.Rows.Count; i++)
							{
								if (this.dgvFoundControllers.Rows[i].Cells[1].Value.ToString() == array2[1])
								{
									for (int j = 2; j < array2.Length; j++)
									{
										this.dgvFoundControllers.Rows[i].Cells[j].Value = array2[j];
									}
									this.dgvFoundControllers.Rows[i].Cells[array2.Length - 1].Value = this.runLogo;
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							this.dgvFoundControllers.Rows.Add(array2);
							if (this.arrMac.IndexOf(text) < 0)
							{
								this.arrMac.Add(text);
								this.wgLogProduct4BasicInfo(text, text2);
							}
						}
					}
				}
				IL_03E8:
				num2 += 8;
			}
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			if (this.dgvFoundControllers.Rows.Count > 0 && flag)
			{
				this.dgvFoundControllers.CurrentCell = this.dgvFoundControllers[1, this.dgvFoundControllers.Rows.Count - 1];
			}
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000036BB File Offset: 0x000026BB
		private void addMacToExclude(string mac)
		{
			this.arrMacExclude.Add(mac);
			this.listBox1.Items.Add(mac);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000036DC File Offset: 0x000026DC
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

		// Token: 0x0600001A RID: 26 RVA: 0x000038DC File Offset: 0x000028DC
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

		// Token: 0x0600001B RID: 27 RVA: 0x00003F08 File Offset: 0x00002F08
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

		// Token: 0x0600001C RID: 28 RVA: 0x0000401C File Offset: 0x0000301C
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

		// Token: 0x0600001D RID: 29 RVA: 0x00004114 File Offset: 0x00003114
		private void btnAddAllToExclude_Click(object sender, EventArgs e)
		{
			DataGridView dataGridView = this.dgvFoundControllers;
			try
			{
				int num;
				if (dataGridView.SelectedRows.Count <= 0)
				{
					if (dataGridView.SelectedCells.Count <= 0)
					{
						return;
					}
					num = dataGridView.SelectedCells[0].RowIndex;
				}
				else
				{
					num = dataGridView.SelectedRows[0].Index;
				}
				if (dataGridView.SelectedRows.Count > 0)
				{
					int count = dataGridView.SelectedRows.Count;
					string[] array = new string[count];
					for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
					{
						array[i] = (string)dataGridView.SelectedRows[i].Cells[2].Value;
					}
					for (int j = 0; j < count; j++)
					{
						string text = array[j];
						for (int k = 0; k < dataGridView.Rows.Count; k++)
						{
							if (text == (string)dataGridView.Rows[k].Cells[2].Value)
							{
								this.addMacToExclude((string)dataGridView.Rows[k].Cells[2].Value);
								this.wgLogProduct4BasicInfoExclude((string)dataGridView.Rows[k].Cells[2].Value, (string)dataGridView.Rows[k].Cells[1].Value);
								dataGridView.Rows.RemoveAt(k);
								break;
							}
						}
					}
				}
				else
				{
					this.addMacToExclude((string)dataGridView.Rows[num].Cells[2].Value);
					this.wgLogProduct4BasicInfoExclude((string)dataGridView.Rows[num].Cells[2].Value, (string)dataGridView.Rows[num].Cells[1].Value);
					dataGridView.Rows.RemoveAt(num);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000436C File Offset: 0x0000336C
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

		// Token: 0x0600001F RID: 31 RVA: 0x0000459C File Offset: 0x0000359C
		private void btnClearAllExclude_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnClearAllExclude.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				this.wgLogProduct4BasicInfoExcludeClear();
				this.listBox1.Items.Clear();
				this.arrMacExclude.Clear();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000045FC File Offset: 0x000035FC
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

		// Token: 0x06000021 RID: 33 RVA: 0x00004898 File Offset: 0x00003898
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

		// Token: 0x06000022 RID: 34 RVA: 0x0000499E File Offset: 0x0000399E
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.bExit = true;
			base.Close();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000049B0 File Offset: 0x000039B0
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
						icController2.RebootControllerIP();
					}
				}
			}
			catch (Exception)
			{
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00005358 File Offset: 0x00004358
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
										this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
													this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
												this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
										this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x06000025 RID: 37 RVA: 0x00006484 File Offset: 0x00005484
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
										this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
													this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
												this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x06000026 RID: 38 RVA: 0x00006B18 File Offset: 0x00005B18
		private void button1_Click(object sender, EventArgs e)
		{
			this.dgvFoundControllers.Rows.Clear();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00006B2C File Offset: 0x00005B2C
		private void button148_Click(object sender, EventArgs e)
		{
			string[] portNames = SerialPort.GetPortNames();
			this.cboUART.Items.Clear();
			for (int i = 0; i <= portNames.Length - 1; i++)
			{
				this.cboUART.Items.Add(portNames[i]);
			}
			if (this.cboUART.Items.Count > 0)
			{
				this.cboUART.Text = this.cboUART.Items[0] as string;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00006BA8 File Offset: 0x00005BA8
		private void button149_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.cboUART.Items.Count > 0)
				{
					if (this.serialPort1.IsOpen)
					{
						this.serialPort1.Close();
					}
					this.serialPort1.PortName = this.cboUART.Text;
					this.serialPort1.Open();
					this.cboUART.Enabled = false;
					this.bExit = false;
					this.button149.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				XMessageBox.Show("串口已被其他软件占用, 请关闭后再尝试打开.");
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00006C4C File Offset: 0x00005C4C
		private void button150_Click(object sender, EventArgs e)
		{
			this.serialPort1.Close();
			this.bExit = true;
			this.button149.Enabled = true;
			this.cboUART.Enabled = true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00006C78 File Offset: 0x00005C78
		private void cboUART_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00006C7C File Offset: 0x00005C7C
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.btnClearAllExclude.Visible = this.checkBox1.Checked;
			this.btnAddAllToExclude.Visible = this.checkBox1.Checked;
			this.listBox1.Visible = this.checkBox1.Checked;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00006CCB File Offset: 0x00005CCB
		private void chkLock_CheckedChanged(object sender, EventArgs e)
		{
			this.button1.PerformClick();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00006CD8 File Offset: 0x00005CD8
		private void chkReader_CheckedChanged(object sender, EventArgs e)
		{
			this.button1.PerformClick();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00006CE8 File Offset: 0x00005CE8
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

		// Token: 0x0600002F RID: 47 RVA: 0x00007018 File Offset: 0x00006018
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.dgvFoundControllers.Rows.Clear();
			this.lblCount.Text = "0";
			this.toolStripStatusLabel1.Text = "0";
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000704C File Offset: 0x0000604C
		private void cloudServerConfigureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgMjControllerConfigure wgMjControllerConfigure = null;
			using (dfrmCloudServerSet dfrmCloudServerSet = new dfrmCloudServerSet())
			{
				if (dfrmCloudServerSet.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				if (wgAppConfig.gRestart)
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

		// Token: 0x06000031 RID: 49 RVA: 0x00007370 File Offset: 0x00006370
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

		// Token: 0x06000032 RID: 50 RVA: 0x00007784 File Offset: 0x00006784
		private void dfrmNetControllerConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.bExit = true;
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000077A0 File Offset: 0x000067A0
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

		// Token: 0x06000034 RID: 52 RVA: 0x0000795C File Offset: 0x0000695C
		private void dfrmNetControllerConfig_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.btnConfigure.Enabled = false;
			this.search100FromTheSpecialSNToolStripMenuItem.Visible = false;
			this.dgvFoundControllers.RowsDefaultCellStyle.BackColor = Color.White;
			this.dgvFoundControllers.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
			this.wgLogLoad();
			this.button148.PerformClick();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000079C3 File Offset: 0x000069C3
		private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.btnConfigure.Enabled)
			{
				this.btnConfigure.PerformClick();
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000079E0 File Offset: 0x000069E0
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

		// Token: 0x06000037 RID: 55 RVA: 0x00007C8C File Offset: 0x00006C8C
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

		// Token: 0x06000038 RID: 56 RVA: 0x00007E58 File Offset: 0x00006E58
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

		// Token: 0x06000039 RID: 57 RVA: 0x00008014 File Offset: 0x00007014
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

		// Token: 0x0600003A RID: 58 RVA: 0x000081E0 File Offset: 0x000071E0
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

		// Token: 0x0600003B RID: 59 RVA: 0x000083A8 File Offset: 0x000073A8
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

		// Token: 0x0600003C RID: 60 RVA: 0x00008588 File Offset: 0x00007588
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

		// Token: 0x0600003D RID: 61 RVA: 0x000085AC File Offset: 0x000075AC
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

		// Token: 0x0600003E RID: 62 RVA: 0x00008600 File Offset: 0x00007600
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

		// Token: 0x0600003F RID: 63 RVA: 0x000087D4 File Offset: 0x000077D4
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
				byte[] array = new byte[1152];
				array[1027] = 165;
				array[1026] = 165;
				array[1025] = 165;
				array[1024] = 165;
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

		// Token: 0x06000040 RID: 64 RVA: 0x000088FC File Offset: 0x000078FC
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
										this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x06000041 RID: 65 RVA: 0x00009014 File Offset: 0x00008014
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
				string text2;
				switch (text2 = text)
				{
				case "5678":
					this.btnDefault.Visible = true;
					this.restoreDefaultParamToolStripMenuItem.Visible = true;
					this.restoreAllSwipesToolStripMenuItem.Visible = true;
					this.clearSwipesToolStripMenuItem.Visible = true;
					this.setSpeacialToolStripMenuItem.Visible = true;
					this.search100FromTheSpecialSNToolStripMenuItem.Visible = true;
					this.otherToolStripMenuItem.Visible = true;
					this.otherToolsToolStripMenuItem.Visible = true;
					this.force100MV552AboveToolStripMenuItem.Visible = true;
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

		// Token: 0x06000042 RID: 66 RVA: 0x00009338 File Offset: 0x00008338
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

		// Token: 0x06000043 RID: 67
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getSNSearch(IntPtr pkt);

		// Token: 0x06000044 RID: 68 RVA: 0x0000943C File Offset: 0x0000843C
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

		// Token: 0x06000045 RID: 69 RVA: 0x0000960C File Offset: 0x0000860C
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

		// Token: 0x06000046 RID: 70 RVA: 0x000099EC File Offset: 0x000089EC
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

		// Token: 0x06000047 RID: 71 RVA: 0x00009F18 File Offset: 0x00008F18
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

		// Token: 0x06000048 RID: 72 RVA: 0x0000A4C4 File Offset: 0x000094C4
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000A4F8 File Offset: 0x000094F8
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

		// Token: 0x0600004A RID: 74 RVA: 0x0000A558 File Offset: 0x00009558
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

		// Token: 0x0600004B RID: 75 RVA: 0x0000B350 File Offset: 0x0000A350
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

		// Token: 0x0600004C RID: 76 RVA: 0x0000B418 File Offset: 0x0000A418
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

		// Token: 0x0600004D RID: 77 RVA: 0x0000B678 File Offset: 0x0000A678
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

		// Token: 0x0600004E RID: 78 RVA: 0x0000BA4C File Offset: 0x0000AA4C
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

		// Token: 0x0600004F RID: 79 RVA: 0x0000BB54 File Offset: 0x0000AB54
		private void restoreDefaultIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnDefault_Click(sender, e);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000BB60 File Offset: 0x0000AB60
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

		// Token: 0x06000051 RID: 81 RVA: 0x0000BC4C File Offset: 0x0000AC4C
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
										this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x06000052 RID: 82 RVA: 0x0000C2A8 File Offset: 0x0000B2A8
		private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (this.serialPort1.BytesToRead >= 5)
			{
				byte[] array = new byte[this.serialPort1.BytesToRead];
				this.serialPort1.Read(array, 0, array.Length);
				if (array.Length > 0 && !this.bExit)
				{
					this.dgvFoundControllers.Invoke(new dfrmBLELockSearch.AddTolstDiscoveredDevicesLock(this.AddDiscoveryLockEntry), new object[] { array, array.Length });
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000C322 File Offset: 0x0000B322
		private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000C324 File Offset: 0x0000B324
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
					goto IL_014E;
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
			else
			{
				num = 0;
				num2 = 0;
				num3 = 0;
				this.readerFormatLast = "";
			}
			IL_014E:
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

		// Token: 0x06000055 RID: 85 RVA: 0x0000C780 File Offset: 0x0000B780
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

		// Token: 0x06000056 RID: 86 RVA: 0x0000C8C4 File Offset: 0x0000B8C4
		private void toolStripStatusLabel1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000C8C6 File Offset: 0x0000B8C6
		private void toolStripStatusLabel2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000C8C8 File Offset: 0x0000B8C8
		private void wgLogLoad()
		{
			string text = "n3k_lockers";
			try
			{
				using (StreamReader streamReader = new StreamReader(Application.StartupPath + "\\" + text + ".log"))
				{
					string text2 = streamReader.ReadLine();
					while (!string.IsNullOrEmpty(text2))
					{
						this.arrMac.Add(text2.Substring(0, "xx:xx:xx:xx:xx:xx".Length));
						text2 = streamReader.ReadLine();
					}
				}
			}
			catch (Exception)
			{
			}
			string text3 = "n3k_lockersExclude";
			try
			{
				using (StreamReader streamReader2 = new StreamReader(Application.StartupPath + "\\" + text3 + ".log"))
				{
					string text4 = streamReader2.ReadLine();
					while (!string.IsNullOrEmpty(text4))
					{
						this.addMacToExclude(text4.Substring(0, "xx:xx:xx:xx:xx:xx".Length));
						text4 = streamReader2.ReadLine();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000C9DC File Offset: 0x0000B9DC
		private void wgLogProduct4BasicInfo(string mac, string sn)
		{
			string text = "n3k_lockers";
			try
			{
				string text2 = string.Format("{0}, {1}, {2}\r\n", mac, sn, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + text + ".log", true))
				{
					streamWriter.Write(text2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000CA60 File Offset: 0x0000BA60
		private void wgLogProduct4BasicInfoExclude(string mac, string sn)
		{
			string text = "n3k_lockersExclude";
			try
			{
				string text2 = string.Format("{0}, {1}, {2}\r\n", mac, sn, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + text + ".log", true))
				{
					streamWriter.Write(text2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000CAE4 File Offset: 0x0000BAE4
		private void wgLogProduct4BasicInfoExcludeClear()
		{
			string text = "n3k_lockersExclude";
			try
			{
				string text2 = "";
				using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\" + text + ".log", false))
				{
					streamWriter.Write(text2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000012 RID: 18
		private bool bAdjustTime;

		// Token: 0x04000013 RID: 19
		private bool bAutoUploadUsers;

		// Token: 0x04000014 RID: 20
		private bool bExit;

		// Token: 0x04000015 RID: 21
		private bool bInput5678;

		// Token: 0x04000016 RID: 22
		private bool bIPAndWEBConfigure;

		// Token: 0x04000017 RID: 23
		private bool bOption;

		// Token: 0x04000018 RID: 24
		private bool bOptionWeb;

		// Token: 0x04000019 RID: 25
		private bool bUpdateIPConfigure;

		// Token: 0x0400001A RID: 26
		private bool bUpdateSpecialCard_IPWEB;

		// Token: 0x0400001B RID: 27
		private bool bUpdateSuperCard_IPWEB;

		// Token: 0x0400001C RID: 28
		private bool bUpdateWEBConfigure;

		// Token: 0x0400001D RID: 29
		private bool bWEBEnabled;

		// Token: 0x0400001E RID: 30
		private bool bWebOnlyQuery;

		// Token: 0x0400001F RID: 31
		private frmTestController dfrmTest;

		// Token: 0x04000020 RID: 32
		private DataTable dtWebStringAdvanced_IPWEB;

		// Token: 0x04000021 RID: 33
		private DataView dv;

		// Token: 0x04000022 RID: 34
		private frmProductFormat frmProductFormat1;

		// Token: 0x04000023 RID: 35
		private int ilogo;

		// Token: 0x04000024 RID: 36
		private int invalidSNofWGYTJ;

		// Token: 0x04000025 RID: 37
		private int webDateFormat;

		// Token: 0x04000027 RID: 39
		private string[] allLogo = new string[] { "|", "/", "-", "\\", "|", "/", "-", "\\" };

		// Token: 0x04000028 RID: 40
		private ArrayList arrMac = new ArrayList();

		// Token: 0x04000029 RID: 41
		private ArrayList arrMacExclude = new ArrayList();

		// Token: 0x0400002A RID: 42
		private bool bFirstShowInfo = true;

		// Token: 0x0400002B RID: 43
		private int commPort = 60000;

		// Token: 0x0400002C RID: 44
		private string EncryptControllerSN = "";

		// Token: 0x0400002D RID: 45
		private string foundControllerIPs = "";

		// Token: 0x0400002E RID: 46
		private int HttpPort = 80;

		// Token: 0x0400002F RID: 47
		private string readerFormatLast = "";

		// Token: 0x04000030 RID: 48
		private string runLogo = "*";

		// Token: 0x04000031 RID: 49
		private string SpecialCard1_IPWEB = "";

		// Token: 0x04000032 RID: 50
		private string SpecialCard2_IPWEB = "";

		// Token: 0x04000033 RID: 51
		private string strControllers = "";

		// Token: 0x04000034 RID: 52
		private string strGateway_IPWEB = "";

		// Token: 0x04000035 RID: 53
		private string strIP_IPWEB = "";

		// Token: 0x04000036 RID: 54
		private string strIPList = "";

		// Token: 0x04000037 RID: 55
		private string strIPListControllerSN = "";

		// Token: 0x04000038 RID: 56
		private string strIPListCurrentPassword = "";

		// Token: 0x04000039 RID: 57
		private string strIPListNewPassword = "";

		// Token: 0x0400003A RID: 58
		private string strNETMASK_IPWEB = "";

		// Token: 0x0400003B RID: 59
		private string strSelectedFile1 = "";

		// Token: 0x0400003C RID: 60
		private string strSelectedFile2 = "";

		// Token: 0x0400003D RID: 61
		private string strWEBLanguage1 = "";

		// Token: 0x0400003E RID: 62
		private string strWEBLanguage2 = "";

		// Token: 0x0400003F RID: 63
		private string superCard1_IPWEB = "";

		// Token: 0x04000040 RID: 64
		private string superCard2_IPWEB = "";

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x0600005F RID: 95
		public delegate void AddTolstDiscoveredDevices(object o, object pcIP, object pcMac, object bPassword);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000063 RID: 99
		public delegate void AddTolstDiscoveredDevicesLock(object o, object pcIP);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000067 RID: 103
		public delegate void AsyncCallback(IAsyncResult ar);
	}
}
