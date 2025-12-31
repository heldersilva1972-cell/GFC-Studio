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
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.SMS;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WG3000_COMM.Security;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EC RID: 748
	public partial class dfrmNetControllerConfig4FingerPrint : frmN3000
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x001A8E2C File Offset: 0x001A7E2C
		public dfrmNetControllerConfig4FingerPrint()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvFoundControllers);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x001A8F3C File Offset: 0x001A7F3C
		public void AddDiscoveryEntry(object o, object pcIP, object pcMac, object bPassword)
		{
			byte[] array = (byte[])o;
			string text = (string)pcIP;
			string text2 = (string)pcMac;
			bool flag = (string)bPassword == "1";
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			long num = (long)dfrmNetControllerConfig4FingerPrint.getSNSearch(intPtr);
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
			else if (wgMjController.IsFingerController((int)num))
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

		// Token: 0x0600156D RID: 5485 RVA: 0x001A930C File Offset: 0x001A830C
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
					if (num2 != -1 && wgAppConfig.getValBySql(string.Format("Select f_ControllerID from t_b_Controller_FingerPrint where f_Controllerid <>{0} and ( f_ControllerSN = {1})", 0, num2)) == 0)
					{
						text = text + num2.ToString() + ",";
						num++;
						this.lblSearchNow.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
						this.toolStripStatusLabel2.Text = this.dgvFoundControllers.SelectedRows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
						using (dfrmFingerPrintConfigure dfrmFingerPrintConfigure = new dfrmFingerPrintConfigure())
						{
							dfrmFingerPrintConfigure.WindowState = FormWindowState.Minimized;
							dfrmFingerPrintConfigure.Show();
							dfrmFingerPrintConfigure.mtxtbControllerSN.Text = num2.ToString();
							dfrmFingerPrintConfigure.btnOK.PerformClick();
							Application.DoEvents();
						}
					}
				}
				Cursor.Current = Cursors.Default;
				XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddControllerDevice, num, text), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x001A94D8 File Offset: 0x001A84D8
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
				using (dfrmTCPIPConfigure4FingerPrint dfrmTCPIPConfigure4FingerPrint = new dfrmTCPIPConfigure4FingerPrint())
				{
					dfrmTCPIPConfigure4FingerPrint.Text = CommonStr.strIPAddressStart;
					if (dfrmTCPIPConfigure4FingerPrint.ShowDialog(this) == DialogResult.OK)
					{
						text = dfrmTCPIPConfigure4FingerPrint.strIP;
						text2 = dfrmTCPIPConfigure4FingerPrint.strMask;
						text3 = dfrmTCPIPConfigure4FingerPrint.strGateway;
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

		// Token: 0x0600156F RID: 5487 RVA: 0x001A9B04 File Offset: 0x001A8B04
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

		// Token: 0x06001570 RID: 5488 RVA: 0x001A9C15 File Offset: 0x001A8C15
		private void batchUpdateSelectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count > 0)
			{
				Cursor.Current = Cursors.WaitCursor;
				Cursor.Current = Cursors.Default;
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x001A9C40 File Offset: 0x001A8C40
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
					if (num2 != -1 && wgAppConfig.getValBySql(string.Format("Select f_ControllerID from t_b_Controller_FingerPrint where f_Controllerid <>{0} and ( f_ControllerSN = {1})", 0, num2)) == 0)
					{
						text = text + num2.ToString() + ",";
						num++;
						this.lblSearchNow.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
						this.toolStripStatusLabel2.Text = this.dgvFoundControllers.Rows[i].Cells[0].Value.ToString() + "-" + num2.ToString();
						using (dfrmFingerPrintConfigure dfrmFingerPrintConfigure = new dfrmFingerPrintConfigure())
						{
							dfrmFingerPrintConfigure.cameraID = 0;
							dfrmFingerPrintConfigure.WindowState = FormWindowState.Minimized;
							dfrmFingerPrintConfigure.Show();
							dfrmFingerPrintConfigure.mtxtbControllerSN.Text = num2.ToString();
							dfrmFingerPrintConfigure.btnOK.PerformClick();
							Application.DoEvents();
						}
					}
				}
				Cursor.Current = Cursors.Default;
				XMessageBox.Show(string.Format("{0}:[{1:d}]\r\n{2}  ", CommonStr.strAutoAddControllerDevice, num, text), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x001A9E14 File Offset: 0x001A8E14
		private void btnConfigure_Click(object sender, EventArgs e)
		{
			if (this.dgvFoundControllers.SelectedRows.Count <= 0)
			{
				XMessageBox.Show(CommonStr.strSelectController);
				return;
			}
			using (dfrmTCPIPConfigure4FingerPrint dfrmTCPIPConfigure4FingerPrint = new dfrmTCPIPConfigure4FingerPrint())
			{
				DataGridViewRow dataGridViewRow = this.dgvFoundControllers.SelectedRows[0];
				dfrmTCPIPConfigure4FingerPrint.strSN = dataGridViewRow.Cells["f_ControllerSN"].Value.ToString();
				dfrmTCPIPConfigure4FingerPrint.strMac = dataGridViewRow.Cells["f_MACAddr"].Value.ToString();
				dfrmTCPIPConfigure4FingerPrint.strIP = dataGridViewRow.Cells["f_IP"].Value.ToString();
				dfrmTCPIPConfigure4FingerPrint.strMask = dataGridViewRow.Cells["f_Mask"].Value.ToString();
				dfrmTCPIPConfigure4FingerPrint.strGateway = dataGridViewRow.Cells["f_Gateway"].Value.ToString();
				dfrmTCPIPConfigure4FingerPrint.strTCPPort = dataGridViewRow.Cells["f_PORT"].Value.ToString();
				string text = dataGridViewRow.Cells["f_PCIPAddr"].Value.ToString();
				if (dfrmTCPIPConfigure4FingerPrint.ShowDialog(this) == DialogResult.OK)
				{
					string strSN = dfrmTCPIPConfigure4FingerPrint.strSN;
					string strMac = dfrmTCPIPConfigure4FingerPrint.strMac;
					string strIP = dfrmTCPIPConfigure4FingerPrint.strIP;
					string strMask = dfrmTCPIPConfigure4FingerPrint.strMask;
					string strGateway = dfrmTCPIPConfigure4FingerPrint.strGateway;
					string strTCPPort = dfrmTCPIPConfigure4FingerPrint.strTCPPort;
					string text2 = dfrmTCPIPConfigure4FingerPrint.Text;
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

		// Token: 0x06001573 RID: 5491 RVA: 0x001AA0B0 File Offset: 0x001A90B0
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

		// Token: 0x06001574 RID: 5492 RVA: 0x001AA1B6 File Offset: 0x001A91B6
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x001AA1C0 File Offset: 0x001A91C0
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

		// Token: 0x06001576 RID: 5494 RVA: 0x001AAB68 File Offset: 0x001A9B68
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
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
													this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
												this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									byte[] array7 = wgMjController.GetControlDataBroadcast(197504, 1427898384);
									if (array7 == null)
									{
										return;
									}
									byte[] array8 = null;
									this.wgudp.udp_get(array7, 300, uint.MaxValue, null, 60000, ref array8);
									if (array8 != null && array8.Length == 1052)
									{
										string text2 = string.Concat(new string[]
										{
											WGPacket.bCommP ? CommonStr.strNetControlWrongPassword : CommonStr.strControllerTypeCommunicationPassword,
											"\r\n\r\n",
											CommonStr.strControllerTypeUnsupportControllerStart,
											"  ",
											((ulong)((long)((int)array8[8] + ((int)array8[9] << 8) + ((int)array8[10] << 16) + ((int)array8[11] << 24)))).ToString().Substring(0, 4),
											"*****"
										});
										array7 = wgpacket.ToBytes(this.wgudp.udpPort);
										this.wgudp.udp_get(array7, 300, uint.MaxValue, null, 60000, ref array8);
										if (array8 == null)
										{
											wgAppConfig.wgLog(text2);
											XMessageBox.Show(text2);
											this.btnSearch.Enabled = true;
											return;
										}
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
										{
											array8,
											unicastIPAddressInformation3.Address.ToString(),
											networkInterface3.GetPhysicalAddress().ToString(),
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
									byte[] array9 = new WGPacketBasicPCAllowedIPSetToSend(0, 0, 0, 0, null, null, null)
									{
										iDevSnTo = uint.MaxValue
									}.ToBytes(0);
									if (array9 == null)
									{
										return;
									}
									byte[] array10 = null;
									this.wgudp.udp_get(array9, 300, uint.MaxValue, null, 60000, ref array10);
									if (array10 != null && array10.Length == 76)
									{
										string text3 = this.iPFilterToolStripMenuItem.Text + "***\r\n" + CommonStr.strWrongVersionDatabaseTail;
										uint num8 = (uint)((int)array10[8] + ((int)array10[9] << 8) + ((int)array10[10] << 16) + ((int)array10[11] << 24));
										wgpacket.iDevSnTo = num8;
										array9 = wgpacket.ToBytes(this.wgudp.udpPort);
										this.wgudp.udp_get_notries(array9, 300, uint.MaxValue, null, 60000, ref array10);
										if (array10 == null)
										{
											wgAppConfig.wgLog(text3);
											XMessageBox.Show(text3);
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
				for (int k = 0; k < this.dgvFoundControllers.Rows.Count; k++)
				{
					int num9 = k + 1;
					this.dgvFoundControllers.Rows[k].Cells[0].Value = num9.ToString().PadLeft(4, '0');
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
				string text4 = CommonStr.strNoControllerInfo2BaseDevice;
				if (num == 0)
				{
					text4 = CommonStr.strNoControllerInfo3PCNotConnectedDevice;
				}
				else if (num >= 2 && num2 >= 1)
				{
					text4 = CommonStr.strNoControllerInfo1Device;
				}
				if (this.bFirstShowInfo)
				{
					this.bFirstShowInfo = false;
					XMessageBox.Show(text4);
				}
			}
			this.lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
			this.toolStripStatusLabel1.Text = this.dgvFoundControllers.Rows.Count.ToString();
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x001AB954 File Offset: 0x001AA954
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
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
													this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
												this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x06001578 RID: 5496 RVA: 0x001ABFE8 File Offset: 0x001AAFE8
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

		// Token: 0x06001579 RID: 5497 RVA: 0x001AC318 File Offset: 0x001AB318
		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.dgvFoundControllers.Rows.Clear();
			this.lblCount.Text = "0";
			this.toolStripStatusLabel1.Text = "0";
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x001AC34C File Offset: 0x001AB34C
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

		// Token: 0x0600157B RID: 5499 RVA: 0x001AC760 File Offset: 0x001AB760
		private void dfrmNetControllerConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dfrmFind1 != null)
			{
				this.dfrmFind1.ReallyCloseForm();
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x001AC778 File Offset: 0x001AB778
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

		// Token: 0x0600157D RID: 5501 RVA: 0x001AC934 File Offset: 0x001AB934
		private void dfrmNetControllerConfig_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.btnConfigure.Enabled = false;
			this.search100FromTheSpecialSNToolStripMenuItem.Visible = false;
			if (wgAppConfig.IsAccessControlBlue)
			{
				this.sMSConfigureToolStripMenuItem.Visible = false;
				this.masterClientControllerSetToolStripMenuItem.Visible = false;
				this.swipeInOrderToolStripMenuItem.Visible = false;
				this.pCControlSwipeToolStripMenuItem.Visible = false;
			}
			this.btnSearch.PerformClick();
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x001AC9A2 File Offset: 0x001AB9A2
		private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.btnConfigure.Enabled)
			{
				this.btnConfigure.PerformClick();
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x001AC9BC File Offset: 0x001AB9BC
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

		// Token: 0x06001580 RID: 5504 RVA: 0x001ACC68 File Offset: 0x001ABC68
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

		// Token: 0x06001581 RID: 5505 RVA: 0x001ACE34 File Offset: 0x001ABE34
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

		// Token: 0x06001582 RID: 5506 RVA: 0x001ACFF0 File Offset: 0x001ABFF0
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

		// Token: 0x06001583 RID: 5507 RVA: 0x001AD1BC File Offset: 0x001AC1BC
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

		// Token: 0x06001584 RID: 5508 RVA: 0x001AD384 File Offset: 0x001AC384
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

		// Token: 0x06001585 RID: 5509 RVA: 0x001AD564 File Offset: 0x001AC564
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

		// Token: 0x06001586 RID: 5510 RVA: 0x001AD588 File Offset: 0x001AC588
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

		// Token: 0x06001587 RID: 5511 RVA: 0x001AD5DC File Offset: 0x001AC5DC
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

		// Token: 0x06001588 RID: 5512 RVA: 0x001AD7B0 File Offset: 0x001AC7B0
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

		// Token: 0x06001589 RID: 5513 RVA: 0x001AD8D8 File Offset: 0x001AC8D8
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
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x0600158A RID: 5514 RVA: 0x001ADFF0 File Offset: 0x001ACFF0
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

		// Token: 0x0600158B RID: 5515 RVA: 0x001AE314 File Offset: 0x001AD314
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

		// Token: 0x0600158C RID: 5516
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getSNSearch(IntPtr pkt);

		// Token: 0x0600158D RID: 5517 RVA: 0x001AE418 File Offset: 0x001AD418
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

		// Token: 0x0600158E RID: 5518 RVA: 0x001AE5E8 File Offset: 0x001AD5E8
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

		// Token: 0x0600158F RID: 5519 RVA: 0x001AE9C8 File Offset: 0x001AD9C8
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

		// Token: 0x06001590 RID: 5520 RVA: 0x001AEEF4 File Offset: 0x001ADEF4
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

		// Token: 0x06001591 RID: 5521 RVA: 0x001AF4A0 File Offset: 0x001AE4A0
		private int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x001AF4D4 File Offset: 0x001AE4D4
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

		// Token: 0x06001593 RID: 5523 RVA: 0x001AF534 File Offset: 0x001AE534
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

		// Token: 0x06001594 RID: 5524 RVA: 0x001B032C File Offset: 0x001AF32C
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

		// Token: 0x06001595 RID: 5525 RVA: 0x001B03F4 File Offset: 0x001AF3F4
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

		// Token: 0x06001596 RID: 5526 RVA: 0x001B0654 File Offset: 0x001AF654
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

		// Token: 0x06001597 RID: 5527 RVA: 0x001B0A28 File Offset: 0x001AFA28
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

		// Token: 0x06001598 RID: 5528 RVA: 0x001B0B30 File Offset: 0x001AFB30
		private void restoreDefaultIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnDefault_Click(sender, e);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x001B0B3C File Offset: 0x001AFB3C
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

		// Token: 0x0600159A RID: 5530 RVA: 0x001B0C28 File Offset: 0x001AFC28
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
										this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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
									this.dgvFoundControllers.Invoke(new dfrmNetControllerConfig4FingerPrint.AddTolstDiscoveredDevices(this.AddDiscoveryEntry), new object[]
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

		// Token: 0x0600159B RID: 5531 RVA: 0x001B1284 File Offset: 0x001B0284
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
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = (sender as ToolStripItem).Text;
					dfrmInputNewName.strNewName = this.readerFormatLast;
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(dfrmInputNewName.strNewName))
					{
						return;
					}
					string[] array = dfrmInputNewName.strNewName.Split(new char[] { ',' });
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
					this.readerFormatLast = dfrmInputNewName.strNewName;
					goto IL_0160;
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
			IL_0160:
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

		// Token: 0x0600159C RID: 5532 RVA: 0x001B16F0 File Offset: 0x001B06F0
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

		// Token: 0x0600159D RID: 5533 RVA: 0x001B1834 File Offset: 0x001B0834
		private void toolStripStatusLabel2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x04002C4D RID: 11341
		private bool bAdjustTime;

		// Token: 0x04002C4E RID: 11342
		private bool bAutoUploadUsers;

		// Token: 0x04002C4F RID: 11343
		private bool bInput5678;

		// Token: 0x04002C50 RID: 11344
		private bool bIPAndWEBConfigure;

		// Token: 0x04002C51 RID: 11345
		private bool bOption;

		// Token: 0x04002C52 RID: 11346
		private bool bOptionWeb;

		// Token: 0x04002C53 RID: 11347
		private bool bUpdateIPConfigure;

		// Token: 0x04002C54 RID: 11348
		private bool bUpdateSpecialCard_IPWEB;

		// Token: 0x04002C55 RID: 11349
		private bool bUpdateSuperCard_IPWEB;

		// Token: 0x04002C56 RID: 11350
		private bool bUpdateWEBConfigure;

		// Token: 0x04002C57 RID: 11351
		private bool bWEBEnabled;

		// Token: 0x04002C58 RID: 11352
		private bool bWebOnlyQuery;

		// Token: 0x04002C59 RID: 11353
		private frmTestController dfrmTest;

		// Token: 0x04002C5A RID: 11354
		private DataTable dtWebStringAdvanced_IPWEB;

		// Token: 0x04002C5B RID: 11355
		private DataView dv;

		// Token: 0x04002C5C RID: 11356
		private int invalidSNofWGYTJ;

		// Token: 0x04002C5D RID: 11357
		private int webDateFormat;

		// Token: 0x04002C5F RID: 11359
		private bool bFirstShowInfo = true;

		// Token: 0x04002C60 RID: 11360
		private int commPort = 60000;

		// Token: 0x04002C61 RID: 11361
		private string EncryptControllerSN = "";

		// Token: 0x04002C62 RID: 11362
		private string foundControllerIPs = "";

		// Token: 0x04002C63 RID: 11363
		private int HttpPort = 80;

		// Token: 0x04002C64 RID: 11364
		private string readerFormatLast = "";

		// Token: 0x04002C65 RID: 11365
		private string SpecialCard1_IPWEB = "";

		// Token: 0x04002C66 RID: 11366
		private string SpecialCard2_IPWEB = "";

		// Token: 0x04002C67 RID: 11367
		private string strControllers = "";

		// Token: 0x04002C68 RID: 11368
		private string strGateway_IPWEB = "";

		// Token: 0x04002C69 RID: 11369
		private string strIP_IPWEB = "";

		// Token: 0x04002C6A RID: 11370
		private string strIPList = "";

		// Token: 0x04002C6B RID: 11371
		private string strIPListControllerSN = "";

		// Token: 0x04002C6C RID: 11372
		private string strIPListCurrentPassword = "";

		// Token: 0x04002C6D RID: 11373
		private string strIPListNewPassword = "";

		// Token: 0x04002C6E RID: 11374
		private string strNETMASK_IPWEB = "";

		// Token: 0x04002C6F RID: 11375
		private string strSelectedFile1 = "";

		// Token: 0x04002C70 RID: 11376
		private string strSelectedFile2 = "";

		// Token: 0x04002C71 RID: 11377
		private string strWEBLanguage1 = "";

		// Token: 0x04002C72 RID: 11378
		private string strWEBLanguage2 = "";

		// Token: 0x04002C73 RID: 11379
		private string superCard1_IPWEB = "";

		// Token: 0x04002C74 RID: 11380
		private string superCard2_IPWEB = "";

		// Token: 0x04002C9C RID: 11420
		private frmProductFormat frmProductFormat1;

		// Token: 0x020002ED RID: 749
		// (Invoke) Token: 0x060015A1 RID: 5537
		public delegate void AddTolstDiscoveredDevices(object o, object pcIP, object pcMac, object bPassword);

		// Token: 0x020002EE RID: 750
		// (Invoke) Token: 0x060015A5 RID: 5541
		public delegate void AsyncCallback(IAsyncResult ar);
	}
}
