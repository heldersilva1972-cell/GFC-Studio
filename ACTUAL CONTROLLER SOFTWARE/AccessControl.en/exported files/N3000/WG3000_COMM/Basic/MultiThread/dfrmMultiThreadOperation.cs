using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic.MultiThread
{
	// Token: 0x0200005A RID: 90
	public partial class dfrmMultiThreadOperation : frmN3000
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060006CC RID: 1740 RVA: 0x000BF264 File Offset: 0x000BE264
		// (remove) Token: 0x060006CD RID: 1741 RVA: 0x000BF298 File Offset: 0x000BE298
		private static event dfrmMultiThreadOperation.appRunInfoCommStatusHandler appRunInfoCommStatus;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006CE RID: 1742 RVA: 0x000BF2CB File Offset: 0x000BE2CB
		// (remove) Token: 0x060006CF RID: 1743 RVA: 0x000BF2D3 File Offset: 0x000BE2D3
		public static event dfrmMultiThreadOperation.appRunInfoCommStatusHandler evAppRunInfoCommStatus
		{
			add
			{
				dfrmMultiThreadOperation.appRunInfoCommStatus += value;
			}
			remove
			{
				dfrmMultiThreadOperation.appRunInfoCommStatus -= value;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000BF2DC File Offset: 0x000BE2DC
		public dfrmMultiThreadOperation()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
			dfrmMultiThreadOperation.tickdfrmMultiThreadOperation = DateTime.Now.Ticks;
			dfrmMultiThreadOperation.tickdfrmMultiThreadOperationStart = dfrmMultiThreadOperation.tickdfrmMultiThreadOperation;
			icSwipeRecord.tickOperation = DateTime.Now.Ticks;
			icSwipeRecord.tickOperationStart = icSwipeRecord.tickOperation;
			int num = 0;
			int num2 = 0;
			ThreadPool.GetMinThreads(out num, out num2);
			wgTools.WgDebugWrite(string.Format("ThreadPool.GetMinThreads: {0},{1}", num, num2), new object[0]);
			ThreadPool.SetMaxThreads(10, 10);
			int num3 = 0;
			int.TryParse(wgAppConfig.GetKeyVal("KEY_MaxThreadNum"), out num3);
			if (num3 > 0)
			{
				ThreadPool.SetMaxThreads(num3, num3);
			}
			ThreadPool.GetMaxThreads(out num, out num2);
			wgTools.WgDebugWrite(string.Format("ThreadPool.GetMaxThreads: {0},{1}", num, num2), new object[0]);
			this.ThreadPoolMaxThreadNum = num;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000BF558 File Offset: 0x000BE558
		private void addEventToLog1(InfoRow info)
		{
			lock (this.qEvent.SyncRoot)
			{
				this.qEvent.Enqueue(info);
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000BF59C File Offset: 0x000BE59C
		private void AdjustTimeConsoleThread()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.AdjustTimeConsoleThread_P64();
				return;
			}
			bool flag = false;
			this.arrDealtSN = null;
			this.arrDealtDoors = null;
			this.arrDealtSN = new ArrayList();
			this.arrDealtDoors = new ArrayList();
			this.waitDoors.Clear();
			this.arrDoorsInfo.Clear();
			new ArrayList();
			new ArrayList();
			new icController();
			DataView dataView = new DataView(this.dtDoors);
			for (int i = 0; i < this.arrSelectedDoors.Count; i++)
			{
				this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
				dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
				if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
				{
					this.arrDoorsInfo.Add(new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"], (int)((byte)dataView[0]["f_DoorNO"])));
					if (string.IsNullOrEmpty(dataView[0]["f_IP"] as string))
					{
						flag = true;
					}
					this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
				}
			}
			wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
			wgTools.WriteLine("发送校准时间指令");
			int num = 0;
			int num2 = 3;
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			DateTime dateTime2 = DateTime.Now;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			byte[] array = null;
			DateTime dateTime3 = DateTime.Now;
			new ArrayList();
			WGPacketBasicAdjustTimeToSend wgpacketBasicAdjustTimeToSend = new WGPacketBasicAdjustTimeToSend(DateTime.Now);
			wgpacketBasicAdjustTimeToSend.type = 32;
			wgpacketBasicAdjustTimeToSend.code = 48;
			wgpacketBasicAdjustTimeToSend.iDevSnFrom = 0U;
			wgpacketBasicAdjustTimeToSend.iCallReturn = 0;
			if (flag)
			{
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					if (this.bStopComm())
					{
						return;
					}
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (string.IsNullOrEmpty(clsDoorsInfo.IP))
					{
						wgpacketBasicAdjustTimeToSend.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
						wgpacketBasicAdjustTimeToSend.GetNewXid();
						wgpacketBasicAdjustTimeToSend.datetimeAdjust = DateTime.Now;
						byte[] array2 = wgpacketBasicAdjustTimeToSend.ToBytes(wgUdpComm.udpPort);
						this.wait4Accelerate();
						wgUdpComm.udp_get_onlySend(array2, 300, wgpacketBasicAdjustTimeToSend.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
						arrayList5.Add(i);
						arrayList4.Add(wgUdpComm.getXidOfCommand(array2));
					}
				}
				Thread.Sleep(10);
				if (wgTools.bUDPCloud > 0)
				{
					Thread.Sleep(300);
				}
				for (;;)
				{
					array = wgUdpComm.GetPacket();
					if (array == null && wgTools.bUDPCloud > 0)
					{
						array = wgTools.wgcloud.GetPacket4get();
					}
					if (array == null)
					{
						goto IL_0474;
					}
					num++;
					if (array[0] == 32 && array[1] == 49 && arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[(int)arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
						{
							clsDoorsInfo2.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							arrayList2.Add(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]);
							arrayList.Add(clsDoorsInfo2);
							dateTime2 = DateTime.Now;
							if (arrayList2.Count == this.arrDoorsInfo.Count)
							{
								break;
							}
						}
					}
				}
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				IL_0474:
				for (int j = 0; j < arrayList.Count; j++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = arrayList[j] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo3.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo3.DoorName, clsDoorsInfo3.ControllerSN),
							information = string.Format("{0}", CommonStr.strAdjustTimeOK)
						});
						this.updateDoorProcess(clsDoorsInfo3.DoorName, CommonStr.strAdjustTimeOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList.Clear();
			}
			dateTime3 = DateTime.Now;
			for (int i = 0; i < this.arrDoorsInfo.Count; i++)
			{
				if (this.bStopComm())
				{
					return;
				}
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
				if (!string.IsNullOrEmpty(clsDoorsInfo4.IP))
				{
					wgpacketBasicAdjustTimeToSend.iDevSnTo = (uint)clsDoorsInfo4.ControllerSN;
					wgpacketBasicAdjustTimeToSend.GetNewXid();
					wgpacketBasicAdjustTimeToSend.datetimeAdjust = DateTime.Now;
					byte[] array3 = wgpacketBasicAdjustTimeToSend.ToBytes(wgUdpComm.udpPort);
					this.wait4Accelerate();
					wgUdpComm.udp_get_onlySend(array3, 300, wgpacketBasicAdjustTimeToSend.xid, clsDoorsInfo4.IP, clsDoorsInfo4.Port, ref array);
					arrayList5.Add(i);
					arrayList4.Add(wgUdpComm.getXidOfCommand(array3));
				}
			}
			wgTools.WriteLine("发送校准时间指令完成");
			while (arrayList2.Count != this.arrDoorsInfo.Count)
			{
				do
				{
					array = wgUdpComm.GetPacket();
					if (array != null)
					{
						num++;
						if (arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo5 = this.arrDoorsInfo[(int)arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo5.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								clsDoorsInfo5.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
								arrayList2.Add(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]);
								arrayList.Add(clsDoorsInfo5);
								dateTime2 = DateTime.Now;
								if (arrayList2.Count == this.arrDoorsInfo.Count)
								{
									goto Block_30;
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < arrayList.Count; k++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo6 = arrayList[k] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo6.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								this.addEventToLog1(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", clsDoorsInfo6.DoorName, clsDoorsInfo6.ControllerSN),
									information = string.Format("{0}", CommonStr.strAdjustTimeOK)
								});
								this.updateDoorProcess(clsDoorsInfo6.DoorName, CommonStr.strAdjustTimeOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
							}
						}
						arrayList.Clear();
						Thread.Sleep(10);
					}
				}
				while (dateTime >= DateTime.Now);
				IL_07EF:
				wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList2.Count.ToString());
				if (!this.bComplete4Cardlost)
				{
					num2--;
					if (num2 > 0)
					{
						dateTime3 = DateTime.Now;
						for (int l = 0; l < this.arrDoorsInfo.Count; l++)
						{
							if (this.bStopComm())
							{
								return;
							}
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo7 = this.arrDoorsInfo[l] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo7.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								wgpacketBasicAdjustTimeToSend.iDevSnTo = (uint)clsDoorsInfo7.ControllerSN;
								wgpacketBasicAdjustTimeToSend.datetimeAdjust = DateTime.Now;
								wgpacketBasicAdjustTimeToSend.GetNewXid();
								byte[] array4 = wgpacketBasicAdjustTimeToSend.ToBytes(wgUdpComm.udpPort);
								this.wait4Accelerate();
								wgUdpComm.udp_get_onlySend(array4, 300, wgpacketBasicAdjustTimeToSend.xid, clsDoorsInfo7.IP, clsDoorsInfo7.Port, ref array);
								arrayList5.Add(l);
								arrayList4.Add(wgUdpComm.getXidOfCommand(array4));
							}
						}
						dateTime = DateTime.Now.AddSeconds(1.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num2);
						if (!this.bComplete4Cardlost)
						{
							continue;
						}
					}
				}
				IL_0934:
				wgTools.WriteLine(string.Format("rcvpktCount =" + num.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDoorAdjusted.Count =" + arrayList3.Count.ToString(), new object[0]));
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				for (int m = 0; m < arrayList.Count; m++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo8 = arrayList[m] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo8.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo8.DoorName, clsDoorsInfo8.ControllerSN),
							information = string.Format("{0}", CommonStr.strAdjustTimeOK)
						});
						this.updateDoorProcess(clsDoorsInfo8.DoorName, CommonStr.strAdjustTimeOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList.Clear();
				for (int n = 0; n < this.arrDoorsInfo.Count; n++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo9 = this.arrDoorsInfo[n] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo9.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						clsDoorsInfo9.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo9.DoorName, clsDoorsInfo9.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo9.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				return;
				Block_30:
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				goto IL_07EF;
			}
			this.bComplete4Cardlost = true;
			this.bCompleteFullOK4Cardlost = true;
			goto IL_0934;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000C0090 File Offset: 0x000BF090
		private void AdjustTimeConsoleThread_P64()
		{
			this.arrDealtSN = null;
			this.arrDealtDoors = null;
			this.arrDealtSN = new ArrayList();
			this.arrDealtDoors = new ArrayList();
			this.waitDoors.Clear();
			this.arrDoorsInfo.Clear();
			new ArrayList();
			new ArrayList();
			new icController();
			DataView dataView = new DataView(this.dtDoors);
			for (int i = 0; i < this.arrSelectedDoors.Count; i++)
			{
				this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
				dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
				if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
				{
					this.arrDoorsInfo.Add(new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"], (int)((byte)dataView[0]["f_DoorNO"])));
					string.IsNullOrEmpty(dataView[0]["f_IP"] as string);
					this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
				}
			}
			wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
			wgTools.WriteLine("发送校准时间指令");
			int num = 0;
			int num2 = 3;
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			DateTime dateTime2 = DateTime.Now;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			byte[] array = null;
			DateTime dateTime3 = DateTime.Now;
			new ArrayList();
			DateTime now = DateTime.Now;
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 48;
			DateTime dateTime4 = now;
			wgpacketShort.data[0] = (byte)wgTools.GetHex((dateTime4.Year - dateTime4.Year % 100) / 100);
			wgpacketShort.data[1] = (byte)wgTools.GetHex(dateTime4.Year % 100);
			wgpacketShort.data[2] = (byte)wgTools.GetHex(dateTime4.Month);
			wgpacketShort.data[3] = (byte)wgTools.GetHex(dateTime4.Day);
			wgpacketShort.data[4] = (byte)wgTools.GetHex(dateTime4.Hour);
			wgpacketShort.data[5] = (byte)wgTools.GetHex(dateTime4.Minute);
			wgpacketShort.data[6] = (byte)wgTools.GetHex(dateTime4.Second);
			dateTime3 = DateTime.Now;
			for (int i = 0; i < this.arrDoorsInfo.Count; i++)
			{
				if (this.bStopComm())
				{
					return;
				}
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
				if (!string.IsNullOrEmpty(clsDoorsInfo.IP))
				{
					wgpacketShort.iDevSn = (uint)clsDoorsInfo.ControllerSN;
					dateTime4 = DateTime.Now;
					wgpacketShort.data[0] = (byte)wgTools.GetHex((dateTime4.Year - dateTime4.Year % 100) / 100);
					wgpacketShort.data[1] = (byte)wgTools.GetHex(dateTime4.Year % 100);
					wgpacketShort.data[2] = (byte)wgTools.GetHex(dateTime4.Month);
					wgpacketShort.data[3] = (byte)wgTools.GetHex(dateTime4.Day);
					wgpacketShort.data[4] = (byte)wgTools.GetHex(dateTime4.Hour);
					wgpacketShort.data[5] = (byte)wgTools.GetHex(dateTime4.Minute);
					wgpacketShort.data[6] = (byte)wgTools.GetHex(dateTime4.Second);
					byte[] array2 = wgpacketShort.ToBytes();
					this.wait4Accelerate();
					wgUdpComm.udp_get_onlySend(array2, 300, wgpacketShort.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
					arrayList5.Add(i);
					arrayList4.Add(wgUdpComm.getXidOfCommand(array2));
				}
			}
			wgTools.WriteLine("发送校准时间指令完成");
			while (arrayList2.Count != this.arrDoorsInfo.Count)
			{
				do
				{
					array = wgUdpServer.getAdjustTimePacket();
					if (array != null)
					{
						num++;
						if (arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[(int)arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								clsDoorsInfo2.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
								arrayList2.Add(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]);
								arrayList.Add(clsDoorsInfo2);
								dateTime2 = DateTime.Now;
								if (arrayList2.Count == this.arrDoorsInfo.Count)
								{
									goto Block_12;
								}
							}
						}
					}
					else
					{
						for (int j = 0; j < arrayList.Count; j++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = arrayList[j] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo3.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								this.addEventToLog1(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", clsDoorsInfo3.DoorName, clsDoorsInfo3.ControllerSN),
									information = string.Format("{0}", CommonStr.strAdjustTimeOK)
								});
								this.updateDoorProcess(clsDoorsInfo3.DoorName, CommonStr.strAdjustTimeOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
							}
						}
						arrayList.Clear();
						Thread.Sleep(10);
					}
				}
				while (dateTime >= DateTime.Now);
				IL_0652:
				wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList2.Count.ToString());
				if (!this.bComplete4Cardlost)
				{
					num2--;
					if (num2 > 0)
					{
						dateTime3 = DateTime.Now;
						for (int k = 0; k < this.arrDoorsInfo.Count; k++)
						{
							if (this.bStopComm())
							{
								return;
							}
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[k] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo4.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								wgpacketShort.iDevSn = (uint)clsDoorsInfo4.ControllerSN;
								dateTime4 = DateTime.Now;
								wgpacketShort.data[0] = (byte)wgTools.GetHex((dateTime4.Year - dateTime4.Year % 100) / 100);
								wgpacketShort.data[1] = (byte)wgTools.GetHex(dateTime4.Year % 100);
								wgpacketShort.data[2] = (byte)wgTools.GetHex(dateTime4.Month);
								wgpacketShort.data[3] = (byte)wgTools.GetHex(dateTime4.Day);
								wgpacketShort.data[4] = (byte)wgTools.GetHex(dateTime4.Hour);
								wgpacketShort.data[5] = (byte)wgTools.GetHex(dateTime4.Minute);
								wgpacketShort.data[6] = (byte)wgTools.GetHex(dateTime4.Second);
								byte[] array3 = wgpacketShort.ToBytes();
								this.wait4Accelerate();
								wgUdpComm.udp_get_onlySend(array3, 300, wgpacketShort.xid, clsDoorsInfo4.IP, clsDoorsInfo4.Port, ref array);
								arrayList5.Add(k);
								arrayList4.Add(wgUdpComm.getXidOfCommand(array3));
							}
						}
						dateTime = DateTime.Now.AddSeconds(1.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num2);
						if (!this.bComplete4Cardlost)
						{
							continue;
						}
					}
				}
				IL_082B:
				wgTools.WriteLine(string.Format("rcvpktCount =" + num.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDoorAdjusted.Count =" + arrayList3.Count.ToString(), new object[0]));
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				for (int l = 0; l < arrayList.Count; l++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo5 = arrayList[l] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo5.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo5.DoorName, clsDoorsInfo5.ControllerSN),
							information = string.Format("{0}", CommonStr.strAdjustTimeOK)
						});
						this.updateDoorProcess(clsDoorsInfo5.DoorName, CommonStr.strAdjustTimeOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList.Clear();
				for (int m = 0; m < this.arrDoorsInfo.Count; m++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo6 = this.arrDoorsInfo[m] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo6.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						clsDoorsInfo6.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo6.DoorName, clsDoorsInfo6.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo6.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				return;
				Block_12:
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				goto IL_0652;
			}
			this.bComplete4Cardlost = true;
			this.bCompleteFullOK4Cardlost = true;
			goto IL_082B;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000C0A78 File Offset: 0x000BFA78
		private bool bStopComm()
		{
			return dfrmMultiThreadOperation.tickdfrmMultiThreadOperationStart != dfrmMultiThreadOperation.tickdfrmMultiThreadOperation;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000C0A8C File Offset: 0x000BFA8C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.btnCancel.Enabled = false;
			this.btnRetryMultithreadOperation.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			dfrmMultiThreadOperation.tickdfrmMultiThreadOperation = 0L;
			wgMjControllerPrivilege.StopDownload();
			wgMjControllerPrivilege.StopUpload();
			wgMjControllerSwipeOperate.StopGetRecord();
			lock (this.waitDoors.SyncRoot)
			{
				this.waitDoors.Clear();
			}
			Thread.Sleep(300);
			Cursor.Current = Cursors.Default;
			base.Close();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000C0B24 File Offset: 0x000BFB24
		private void btnOtherInfo_Click(object sender, EventArgs e)
		{
			this.grpControllers.Visible = true;
			this.btnOtherInfo.Visible = false;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000C0B40 File Offset: 0x000BFB40
		private void btnRetryMultithreadOperation_Click(object sender, EventArgs e)
		{
			this.btnRetryMultithreadOperation.Enabled = false;
			this.arrDoorsInfo.Clear();
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			DataTable dataTable = this.dataGridView1.DataSource as DataTable;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if ((int)dataTable.Rows[i]["f_RunStatus"] == -1 || (int)dataTable.Rows[i]["f_RunStatus"] == 0)
				{
					this.arrSelectedDoors.Add(dataTable.Rows[i]["f_DoorName"] as string);
					dataTable.Rows[i]["f_RunStatus"] = 0;
				}
			}
			if (this.arrSelectedDoors.Count == 0)
			{
				this.btnRetryMultithreadOperation.Enabled = true;
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.updateLabelInfo(ref num, ref num2, ref num3);
			if (this.dfrmWait1 == null || this.dfrmWait1.IsDisposed)
			{
				this.dfrmWait1 = null;
				this.dfrmWait1 = new dfrmWait();
			}
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			Thread thread;
			switch (this.op)
			{
			case dfrmMultiThreadOperation.Operation.GetSwipes:
				thread = new Thread(new ThreadStart(this.OperateControllerConsoleThread));
				break;
			case dfrmMultiThreadOperation.Operation.Download:
				thread = new Thread(new ThreadStart(this.OperateControllerConsoleThread));
				break;
			case dfrmMultiThreadOperation.Operation.RemoteOpen:
				thread = new Thread(new ThreadStart(this.RemoteOpenConsoleThread));
				break;
			case dfrmMultiThreadOperation.Operation.SetDoorControl:
				thread = new Thread(new ThreadStart(this.SetDoorControlConsoleThread));
				break;
			case dfrmMultiThreadOperation.Operation.CardLost:
			case dfrmMultiThreadOperation.Operation.DelPri:
			case dfrmMultiThreadOperation.Operation.AddPri:
				return;
			case dfrmMultiThreadOperation.Operation.AdjustTime:
				thread = new Thread(new ThreadStart(this.AdjustTimeConsoleThread));
				break;
			default:
				return;
			}
			thread.Start();
			this.btnRetryMultithreadOperation.Enabled = true;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000C0D34 File Offset: 0x000BFD34
		private void dfrmMultiThreadOperation_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000C0D6C File Offset: 0x000BFD6C
		private void dfrmMultiThreadOperation_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				dfrmMultiThreadOperation.tickdfrmMultiThreadOperation = 0L;
				icSwipeRecord.tickOperation = 0L;
				wgMjControllerPrivilege.StopDownload();
				wgMjControllerPrivilege.StopUpload();
				wgMjControllerSwipeOperate.StopGetRecord();
				this.dtDoors.Dispose();
				this.dvDoors4updateDoorProcess.Dispose();
				this.dvDoors4updateDownloadInfo.Dispose();
			}
			catch
			{
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000C0DCC File Offset: 0x000BFDCC
		private void dfrmMultiThreadOperation_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyValue == 81 && e.Shift)
			{
				this.dataGridView1.Columns["f_RunStatus"].Visible = true;
				this.dataGridView1.Columns["f_RecID"].Visible = true;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000C0E2C File Offset: 0x000BFE2C
		private void dfrmMultiThreadOperation_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessControlBlue)
			{
				base.Close();
				return;
			}
			if (wgTools.bUDPOnly64 > 0)
			{
				this.statSoftStarttime.Text = wgTools.softwareStartTime.ToString(wgTools.DisplayFormat_DateYMDHMSWeek) + ">>";
			}
			dfrmMultiThreadOperation.evAppRunInfoCommStatus += this.updateDoorProcess;
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.loadDoorInfo();
			this.dataGridView1.Columns["f_IP"].Visible = wgTools.bUDPCloud <= 0;
			this.dataGridView1.Columns["f_cloudipinfo"].Visible = wgTools.bUDPCloud > 0;
			this.dataGridView1.Columns["f_time"].Visible = wgTools.bUDPCloud > 0;
			this.dvDoors4updateDoorProcess = new DataView(this.dtDoors);
			this.dvDoors4updateDownloadInfo = new DataView(this.dtDoors);
			this.txtTotalDoors.Text = this.dtDoors.Rows.Count.ToString();
			this.txtAllControllerCnt.Text = this.getControllerCount(new DataView(this.dtDoors)).ToString();
			this.txtFailedControllers.BackColor = Color.White;
			this.txtFailedDoors.BackColor = Color.White;
			this.txtDealingControllers.BackColor = Color.White;
			this.txtDealingDoors.BackColor = Color.White;
			this.lblDealingInfo.Text = "";
			this.toolStripStatusLabel2.Text = "";
			this.writeDisplayedInfo(CommonStr.strStart);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000C102C File Offset: 0x000C002C
		private void DownloadAddDelPriUserThreadMainWithParameters(object obj)
		{
			int num = -1;
			dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
			string doorName = clsDoorsInfo.DoorName;
			int num2 = 0;
			string text = "";
			int num3 = 60000;
			long num4 = -1L;
			using (icPrivilege icPrivilege = new icPrivilege())
			{
				using (icController icController = new icController())
				{
					for (;;)
					{
						string doorName2 = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						icController.ControllerID = clsDoorsInfo.ControllerID;
						num = icController.GetControllerRunInformationIP(-1);
						if (num > 0 && (icController.runinfo.registerCardNum != 0U || (this.op != dfrmMultiThreadOperation.Operation.DelPri && (num = icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT)) >= 0)))
						{
							for (int i = 0; i < this.arrConsumerID.Count; i++)
							{
								MjRegisterCard privilegeOfOneCardByDB;
								lock (this.qLock4MultithreadDBOperation.SyncRoot)
								{
									privilegeOfOneCardByDB = icPrivilege.GetPrivilegeOfOneCardByDB(icController.ControllerID, (int)this.arrConsumerID[i], ref num4, ref num2, ref text, ref num3);
								}
								if (privilegeOfOneCardByDB == null)
								{
									break;
								}
								if (privilegeOfOneCardByDB.CardID > 0L)
								{
									num = icPrivilege.AddPrivilegeOfOneCardIP(num2, text, num3, privilegeOfOneCardByDB);
								}
								else if (this.op == dfrmMultiThreadOperation.Operation.DelPri)
								{
									num = icPrivilege.DelPrivilegeOfOneCardIP(num2, text, num3, num4);
								}
							}
						}
						if (num >= 0)
						{
							lock (this.qSuccess4AddDelPri.SyncRoot)
							{
								this.qSuccess4AddDelPri.Enqueue(icController.ControllerSN);
								goto IL_01C3;
							}
							goto IL_018F;
						}
						goto IL_018F;
						IL_01C3:
						clsDoorsInfo = this.getWaitDoorInfo();
						if (clsDoorsInfo == null)
						{
							break;
						}
						continue;
						IL_018F:
						lock (this.qFail4AddDelPri.SyncRoot)
						{
							this.qFail4AddDelPri.Enqueue(icController.ControllerSN);
						}
						goto IL_01C3;
					}
					icController.Dispose();
				}
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000C12A8 File Offset: 0x000C02A8
		private void DownloadCardLostUserThreadMainWithParameters(object obj)
		{
			int num = -1;
			dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
			string doorName = clsDoorsInfo.DoorName;
			using (icPrivilege icPrivilege = new icPrivilege())
			{
				using (icController icController = new icController())
				{
					for (;;)
					{
						string doorName2 = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						icController.ControllerID = clsDoorsInfo.ControllerID;
						if ((string.IsNullOrEmpty(this.oldCardNO4Cardlost) || (num = icPrivilege.DelPrivilegeOfOneCardIP(icController.ControllerSN, icController.IP, icController.PORT, long.Parse(this.oldCardNO4Cardlost))) >= 0) && !string.IsNullOrEmpty(this.newCardNO4Cardlost))
						{
							num = icController.GetControllerRunInformationIP(-1);
							if (num > 0 && (icController.runinfo.registerCardNum != 0U || (num = icPrivilege.ClearAllPrivilegeIP(icController.ControllerSN, icController.IP, icController.PORT)) >= 0))
							{
								MjRegisterCard mjrc = clsDoorsInfo.Mjrc;
								long iCardID = clsDoorsInfo.iCardID;
								int controllerSN = clsDoorsInfo.ControllerSN;
								string ip = clsDoorsInfo.IP;
								int port = clsDoorsInfo.Port;
								if (mjrc != null)
								{
									if (mjrc.CardID > 0L)
									{
										num = icPrivilege.AddPrivilegeOfOneCardIP(controllerSN, ip, port, mjrc);
									}
									else if (this.delPrivilegeController.Count > 0 && this.delPrivilegeController.IndexOf(icController.ControllerID) >= 0)
									{
										num = icPrivilege.DelPrivilegeOfOneCardIP(controllerSN, ip, port, iCardID);
									}
								}
							}
						}
						if (num >= 0)
						{
							lock (this.qSuccess4Cardlost.SyncRoot)
							{
								this.qSuccess4Cardlost.Enqueue(icController.ControllerSN);
								goto IL_01DE;
							}
							goto IL_01AA;
						}
						goto IL_01AA;
						IL_01DE:
						clsDoorsInfo = this.getWaitDoorInfo();
						if (clsDoorsInfo == null)
						{
							break;
						}
						continue;
						IL_01AA:
						lock (this.qFail4Cardlost.SyncRoot)
						{
							this.qFail4Cardlost.Enqueue(icController.ControllerSN);
						}
						goto IL_01DE;
					}
					icController.Dispose();
				}
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000C1528 File Offset: 0x000C0528
		private void downloadPrivilegeAndConfigure()
		{
			using (dfrmUploadOption dfrmUploadOption = new dfrmUploadOption())
			{
				dfrmUploadOption.TopMost = true;
				dfrmUploadOption.ShowDialog(this);
				if (dfrmUploadOption.checkVal == 0)
				{
					base.Close();
					return;
				}
				this.CommOperateOption = dfrmUploadOption.checkVal;
			}
			if (wgAppConfig.getParamValBoolByNO(121))
			{
				this.bActivateTimeProfile = true;
				this.controlTimeSegList.fillByDB();
			}
			for (int i = 0; i < this.fingerCard.Length; i++)
			{
				this.fingerCard[i] = byte.MaxValue;
			}
			for (int j = 0; j < this.fingerInfo.Length; j++)
			{
				this.fingerInfo[j] = byte.MaxValue;
			}
			string text = "";
			string text2 = "";
			int num = 0;
			try
			{
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						dbConnection.Open();
						text = "SELECT t_b_Consumer_Fingerprint.*, t_b_Consumer.f_CardNO  From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer.f_CardNO>0 AND t_b_Consumer_Fingerprint.f_ConsumerID = t_b_Consumer.f_ConsumerID  ORDER BY f_FingerNO ASC";
						dbCommand.CommandText = text;
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							num = (int)dbDataReader["f_FingerNO"];
							if (num <= 0 || num >= 1024)
							{
								break;
							}
							Array.Copy(BitConverter.GetBytes(long.Parse(dbDataReader["f_CardNO"].ToString())), 0, this.fingerCard, (num - 1) * 4, 4);
							text2 = dbDataReader["f_FingerInfo"] as string;
							if (!string.IsNullOrEmpty(text2))
							{
								text2 = text2.Replace("\r\n", "");
								for (int k = 0; k < text2.Length; k += 2)
								{
									try
									{
										this.fingerInfo[(num - 1) * 512 + k / 2] = byte.Parse(text2.Substring(k, 2), NumberStyles.AllowHexSpecifier);
									}
									catch (Exception ex)
									{
										wgAppConfig.wgLog(string.Concat(new object[]
										{
											ex.ToString(),
											"\r\nfingerNO=",
											num,
											",strFinger=",
											text2
										}));
									}
								}
							}
							if (this.fingerCnt < num)
							{
								this.fingerCnt = num;
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
					num,
					",strFinger=",
					text2
				}));
			}
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			new Thread(new ThreadStart(this.OperateControllerConsoleThread)).Start();
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000C188C File Offset: 0x000C088C
		private int downloadPrivilegeNow(int Option, string doorName, icController control4uploadPrivilege)
		{
			if (wgTools.bUDPCloud > 0)
			{
				return this.downloadPrivilegeNow4Cloud(Option, doorName, control4uploadPrivilege);
			}
			int num = -1;
			try
			{
				dfrmMultiThreadOperation.icPrivilegeMutithread icPrivilegeMutithread = new dfrmMultiThreadOperation.icPrivilegeMutithread();
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure wgMjControllerConfigure2 = new wgMjControllerConfigure();
				wgMjControllerTaskList wgMjControllerTaskList = new wgMjControllerTaskList();
				wgMjControllerHolidaysList wgMjControllerHolidaysList = new wgMjControllerHolidaysList();
				wgMjControllerConfigure.RestoreDefault();
				wgMjControllerConfigure2.Clear();
				wgMjControllerTaskList.Clear();
				wgMjControllerHolidaysList.Clear();
				icPrivilegeMutithread.bDisplayProcess = false;
				icPrivilegeMutithread.AllowUpload();
				control4uploadPrivilege.runinfo.Clear();
				string text = "";
				string text2 = "";
				string text3 = null;
				int num2 = 3;
				int num3 = 300;
				int num4 = 0;
				while (num4 < num2 && !this.bStopComm() && control4uploadPrivilege.GetControllerRunInformationIP(-1) <= 0)
				{
					Thread.Sleep(num3);
					num4++;
				}
				if (!this.bStopComm())
				{
					if (control4uploadPrivilege.runinfo.wgcticks <= 0U)
					{
						wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
						icPrivilegeMutithread.Dispose();
						return -13;
					}
					if (control4uploadPrivilege.runinfo.netSpeedCode != 0)
					{
						wgUdpComm.CommTimeoutMsMin = 2000L;
					}
					int num5 = 0;
					while (num5 < num2 && !this.bStopComm())
					{
						text3 = control4uploadPrivilege.GetProductInfoIP(ref text, ref text2, -1);
						if (!string.IsNullOrEmpty(text3))
						{
							break;
						}
						Thread.Sleep(num3);
						num5++;
					}
					if (string.IsNullOrEmpty(text3))
					{
						wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
						icPrivilegeMutithread.Dispose();
						return -13;
					}
					lock (this.qProductsInfo4DownloadPriviege.SyncRoot)
					{
						this.qProductsInfo4DownloadPriviege.Enqueue(text);
					}
					if (!this.bStopComm())
					{
						if (wgMjController.IsFingerController(control4uploadPrivilege.ControllerSN))
						{
							num = control4uploadPrivilege.UpdateFingerprintListIP(this.fingerCnt, this.fingerCard, this.fingerInfo, doorName, -1);
							if (num <= 0)
							{
								wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " UpdateFingerprintListIP Failed num =" + num.ToString(), new object[0]);
								num = -13;
								icPrivilegeMutithread.Dispose();
								return num;
							}
							this.updateDisplayTotal(doorName, this.fingerCnt.ToString());
						}
						else
						{
							if ((Option & 1) > 0)
							{
								lock (this.qLock4MultithreadDBOperation.SyncRoot)
								{
									icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(control4uploadPrivilege.ControllerID, ref wgMjControllerConfigure2, ref wgMjControllerTaskList, ref wgMjControllerHolidaysList);
								}
								num = control4uploadPrivilege.UpdateConfigureIP(wgMjControllerConfigure2, -1);
								if (num <= 0)
								{
									wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateConfigureIP Failed num =" + num.ToString(), new object[0]);
									num = -13;
									icPrivilegeMutithread.Dispose();
									return num;
								}
								if (this.bStopComm())
								{
									goto IL_0616;
								}
								if (wgMjControllerConfigure2.controlTaskList_enabled > 0 && (num = control4uploadPrivilege.UpdateControlTaskListIP(wgMjControllerTaskList, -1)) <= 0)
								{
									wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateControlTaskListIP Failed num =" + num.ToString(), new object[0]);
									num = -13;
									icPrivilegeMutithread.Dispose();
									return num;
								}
								if (this.bStopComm())
								{
									goto IL_0616;
								}
								if (this.bActivateTimeProfile)
								{
									num = control4uploadPrivilege.UpdateControlTimeSegListIP(this.controlTimeSegList, -1);
									if (num <= 0)
									{
										wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateControlTimeSegListIP Failed num =" + num.ToString(), new object[0]);
										num = -13;
										icPrivilegeMutithread.Dispose();
										return num;
									}
									if (this.bStopComm())
									{
										goto IL_0616;
									}
									num = control4uploadPrivilege.UpdateHolidayListIP(wgMjControllerHolidaysList.ToByte(), -1);
									if (num <= 0)
									{
										wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " UpdateHolidayListIP Failed num =" + num.ToString(), new object[0]);
										num = -13;
										icPrivilegeMutithread.Dispose();
										return num;
									}
								}
							}
							if (!this.bStopComm())
							{
								if ((Option & 2) > 0)
								{
									int controllerID = control4uploadPrivilege.ControllerID;
									if (controllerID > 0)
									{
										lock (this.qLock4MultithreadDBOperation.SyncRoot)
										{
											num = icPrivilegeMutithread.getPrivilegeByID(controllerID);
										}
										if (num < 0)
										{
											wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.getPrivilegeByID Failed num =" + num.ToString(), new object[0]);
											icPrivilegeMutithread.Dispose();
											return num;
										}
										if (this.bStopComm())
										{
											goto IL_0616;
										}
										if (icPrivilegeMutithread.PrivilegTotal > control4uploadPrivilege.ControllerSN % 100 && wgTools.bFindFalseACont)
										{
											if (control4uploadPrivilege.UpdateConfigureUnvalidIP(wgMjControllerConfigure, -1) == 1)
											{
												wgAppConfig.wgLog(".UpdateConfigureIP_E1_" + control4uploadPrivilege.ControllerSN.ToString());
											}
											else
											{
												wgTools.bFindFalseACont = false;
											}
										}
										this.updateDisplayTotal(doorName, icPrivilegeMutithread.ConsumersTotal.ToString());
										if (this.bStopComm())
										{
											goto IL_0616;
										}
										num = icPrivilegeMutithread.upload(control4uploadPrivilege.ControllerSN, control4uploadPrivilege.IP, control4uploadPrivilege.PORT, doorName, -1);
										if (num < 0)
										{
											wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.upload Failed num =" + num.ToString(), new object[0]);
											icPrivilegeMutithread.Dispose();
											return num;
										}
										string text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
										string text5 = string.Format(text4, new object[]
										{
											wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
											icPrivilegeMutithread.ConsumersTotal,
											icPrivilegeMutithread.PrivilegTotal,
											icPrivilegeMutithread.ValidPrivilege,
											controllerID
										});
										lock (this.qLock4MultithreadDBOperation.SyncRoot)
										{
											wgAppConfig.runUpdateSql(text5);
										}
									}
								}
								if (!this.bStopComm() && (Option & 1) > 0 && wgMjControllerTaskList.taskCount > 0)
								{
									num = control4uploadPrivilege.RenewControlTaskListIP(-1);
									if (num < 0)
									{
										wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.renewControlTaskListIP Failed num =" + num.ToString(), new object[0]);
									}
								}
							}
						}
					}
				}
				IL_0616:
				icPrivilegeMutithread.Dispose();
			}
			catch (Exception ex)
			{
				num = -1;
				wgAppConfig.wgLog(ex.ToString());
			}
			return num;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000C1F4C File Offset: 0x000C0F4C
		private int downloadPrivilegeNow4Cloud(int Option, string doorName, icController control4uploadPrivilege)
		{
			int num = -1;
			wgUdpComm wgUdpComm = null;
			int controllerSN = control4uploadPrivilege.ControllerSN;
			wgTools.getUdpComm(ref wgUdpComm, wgTools.wgcloud.getUDPQueue4Multithread((uint)controllerSN));
			try
			{
				dfrmMultiThreadOperation.icPrivilegeMutithread icPrivilegeMutithread = new dfrmMultiThreadOperation.icPrivilegeMutithread();
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				wgMjControllerConfigure wgMjControllerConfigure2 = new wgMjControllerConfigure();
				wgMjControllerTaskList wgMjControllerTaskList = new wgMjControllerTaskList();
				wgMjControllerHolidaysList wgMjControllerHolidaysList = new wgMjControllerHolidaysList();
				wgMjControllerConfigure.RestoreDefault();
				wgMjControllerConfigure2.Clear();
				wgMjControllerTaskList.Clear();
				wgMjControllerHolidaysList.Clear();
				icPrivilegeMutithread.bDisplayProcess = false;
				icPrivilegeMutithread.AllowUpload();
				control4uploadPrivilege.runinfo.Clear();
				string text = "";
				string text2 = "";
				wgTools.WgDebugWrite("111111111111111111111111111111111111111111111111111111111", new object[0]);
				string text3 = null;
				int num2 = 3;
				int num3 = 300;
				int num4 = 0;
				while (num4 < num2 && !this.bStopComm() && control4uploadPrivilege.GetControllerRunInformationIP(wgUdpComm.UDPQueue4MultithreadIndex) <= 0)
				{
					Thread.Sleep(num3);
					num4++;
				}
				if (!this.bStopComm())
				{
					wgTools.WgDebugWrite("22222222222222222222222", new object[0]);
					if (control4uploadPrivilege.runinfo.wgcticks <= 0U)
					{
						wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
						icPrivilegeMutithread.Dispose();
						return -13;
					}
					if (control4uploadPrivilege.runinfo.netSpeedCode != 0)
					{
						wgUdpComm.CommTimeoutMsMin = 2000L;
					}
					if (wgTools.bUDPOnly64 <= 0)
					{
						int num5 = 0;
						while (num5 < num2 && !this.bStopComm())
						{
							text3 = control4uploadPrivilege.GetProductInfoIP(ref text, ref text2, wgUdpComm.UDPQueue4MultithreadIndex);
							if (!string.IsNullOrEmpty(text3))
							{
								break;
							}
							Thread.Sleep(num3);
							num5++;
						}
						wgTools.WgDebugWrite("333333333111111111111111111111111111111111111111111111111111111111", new object[0]);
						if (string.IsNullOrEmpty(text3))
						{
							wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.GetProductInfoIP Failed num =" + num.ToString(), new object[0]);
							icPrivilegeMutithread.Dispose();
							return -13;
						}
						lock (this.qProductsInfo4DownloadPriviege.SyncRoot)
						{
							this.qProductsInfo4DownloadPriviege.Enqueue(text);
						}
					}
					if (!this.bStopComm())
					{
						wgTools.WgDebugWrite("44444444444444111111111111111111111111111111111111111111111111111111111", new object[0]);
						if (wgMjController.IsFingerController(control4uploadPrivilege.ControllerSN))
						{
							num = control4uploadPrivilege.UpdateFingerprintListIP(this.fingerCnt, this.fingerCard, this.fingerInfo, doorName, wgUdpComm.UDPQueue4MultithreadIndex);
							if (num <= 0)
							{
								wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " UpdateFingerprintListIP Failed num =" + num.ToString(), new object[0]);
								num = -13;
								icPrivilegeMutithread.Dispose();
								return num;
							}
							this.updateDisplayTotal(doorName, this.fingerCnt.ToString());
						}
						else
						{
							if ((Option & 1) > 0)
							{
								lock (this.qLock4MultithreadDBOperation.SyncRoot)
								{
									icControllerConfigureFromDB.getControllerConfigureFromDBByControllerID(control4uploadPrivilege.ControllerID, ref wgMjControllerConfigure2, ref wgMjControllerTaskList, ref wgMjControllerHolidaysList);
								}
								num = control4uploadPrivilege.UpdateConfigureIP(wgMjControllerConfigure2, wgUdpComm.UDPQueue4MultithreadIndex);
								if (num <= 0)
								{
									wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateConfigureIP Failed num =" + num.ToString(), new object[0]);
									num = -13;
									icPrivilegeMutithread.Dispose();
									return num;
								}
								wgTools.WgDebugWrite("5555555555555111111111111111111111111111111111111111111111111111111111", new object[0]);
								if (this.bStopComm())
								{
									goto IL_06DD;
								}
								if (wgMjControllerConfigure2.controlTaskList_enabled > 0 && (num = control4uploadPrivilege.UpdateControlTaskListIP(wgMjControllerTaskList, wgUdpComm.UDPQueue4MultithreadIndex)) <= 0)
								{
									wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateControlTaskListIP Failed num =" + num.ToString(), new object[0]);
									num = -13;
									icPrivilegeMutithread.Dispose();
									return num;
								}
								if (this.bStopComm())
								{
									goto IL_06DD;
								}
								if (this.bActivateTimeProfile)
								{
									num = control4uploadPrivilege.UpdateControlTimeSegListIP(this.controlTimeSegList, wgUdpComm.UDPQueue4MultithreadIndex);
									if (num <= 0)
									{
										wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " updateControlTimeSegListIP Failed num =" + num.ToString(), new object[0]);
										num = -13;
										icPrivilegeMutithread.Dispose();
										return num;
									}
									if (this.bStopComm())
									{
										goto IL_06DD;
									}
									num = control4uploadPrivilege.UpdateHolidayListIP(wgMjControllerHolidaysList.ToByte(), wgUdpComm.UDPQueue4MultithreadIndex);
									if (num <= 0)
									{
										wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " UpdateHolidayListIP Failed num =" + num.ToString(), new object[0]);
										num = -13;
										icPrivilegeMutithread.Dispose();
										return num;
									}
								}
							}
							if (!this.bStopComm())
							{
								if ((Option & 2) > 0)
								{
									wgTools.WgDebugWrite("666666666666111111111111111111111111111111111111111111111111111111111", new object[0]);
									int controllerID = control4uploadPrivilege.ControllerID;
									if (controllerID > 0)
									{
										lock (this.qLock4MultithreadDBOperation.SyncRoot)
										{
											num = icPrivilegeMutithread.getPrivilegeByID(controllerID);
										}
										if (num < 0)
										{
											wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.getPrivilegeByID Failed num =" + num.ToString(), new object[0]);
											icPrivilegeMutithread.Dispose();
											return num;
										}
										if (this.bStopComm())
										{
											goto IL_06DD;
										}
										if (wgTools.bUDPOnly64 <= 0 && icPrivilegeMutithread.PrivilegTotal > control4uploadPrivilege.ControllerSN % 100 && wgTools.bFindFalseACont)
										{
											if (control4uploadPrivilege.UpdateConfigureUnvalidIP(wgMjControllerConfigure, wgUdpComm.UDPQueue4MultithreadIndex) == 1)
											{
												wgAppConfig.wgLog(".UpdateConfigureIP_E1_" + control4uploadPrivilege.ControllerSN.ToString());
											}
											else
											{
												wgTools.bFindFalseACont = false;
											}
										}
										this.updateDisplayTotal(doorName, icPrivilegeMutithread.ConsumersTotal.ToString());
										if (this.bStopComm())
										{
											goto IL_06DD;
										}
										num = icPrivilegeMutithread.upload(control4uploadPrivilege.ControllerSN, control4uploadPrivilege.IP, control4uploadPrivilege.PORT, doorName, wgUdpComm.UDPQueue4MultithreadIndex);
										if (num < 0)
										{
											wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " pr4uploadPrivilege.upload Failed num =" + num.ToString(), new object[0]);
											icPrivilegeMutithread.Dispose();
											return num;
										}
										string text4 = "UPDATE t_b_Controller SET f_lastDelAddDateTime ={0}, f_lastDelAddConsuemrsTotal =0,  f_lastConsoleUploadDateTime ={0}, f_lastConsoleUploadConsuemrsTotal ={1:d}, f_lastConsoleUploadPrivilege ={2:d}, f_lastConsoleUploadValidPrivilege ={3:d} WHERE f_ControllerID ={4:d}";
										string text5 = string.Format(text4, new object[]
										{
											wgTools.PrepareStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")),
											icPrivilegeMutithread.ConsumersTotal,
											icPrivilegeMutithread.PrivilegTotal,
											icPrivilegeMutithread.ValidPrivilege,
											controllerID
										});
										lock (this.qLock4MultithreadDBOperation.SyncRoot)
										{
											wgAppConfig.runUpdateSql(text5);
										}
									}
								}
								if (!this.bStopComm() && (Option & 1) > 0)
								{
									wgTools.WgDebugWrite("777777777111111111111111111111111111111111111111111111111111111111", new object[0]);
									if (wgMjControllerTaskList.taskCount > 0)
									{
										num = control4uploadPrivilege.RenewControlTaskListIP(wgUdpComm.UDPQueue4MultithreadIndex);
										if (num < 0)
										{
											wgTools.WgDebugWrite(control4uploadPrivilege.ControllerSN.ToString() + " control4uploadPrivilege.renewControlTaskListIP Failed num =" + num.ToString(), new object[0]);
										}
									}
								}
							}
						}
					}
				}
				IL_06DD:
				icPrivilegeMutithread.Dispose();
			}
			catch (Exception ex)
			{
				num = -1;
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				if (wgTools.bUDPCloud > 0)
				{
					wgTools.wgcloud.removeUDPQueue4Multithread(wgUdpComm.UDPQueue4MultithreadIndex, (uint)controllerSN);
					wgUdpComm.Dispose();
					wgUdpComm = null;
				}
			}
			return num;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000C270C File Offset: 0x000C170C
		private void DownloadThreadMainWithParameters(object obj)
		{
			if (!this.bStopComm())
			{
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
				string text = clsDoorsInfo.DoorName;
				using (icController icController = new icController())
				{
					do
					{
						text = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						icController.ControllerID = clsDoorsInfo.ControllerID;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", text.ToString(), icController.ControllerSN),
							information = string.Format("{0}", CommonStr.strUploadStart)
						});
						this.updateDoorProcess(text, CommonStr.strUploadStart, dfrmMultiThreadOperation.RunStatus.Start);
						this.updateStartTime(text, DateTime.Now.ToString("HH:mm:ss"));
						int num = this.downloadPrivilegeNow(this.CommOperateOption, text, icController);
						if (num >= 0)
						{
							InfoRow infoRow = new InfoRow();
							infoRow.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
							if ((this.CommOperateOption & 3) == 3)
							{
								infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadAllOK, num);
								this.updateDoorProcess(text, CommonStr.strUploadAllOK, dfrmMultiThreadOperation.RunStatus.CompleteOK);
							}
							else if ((this.CommOperateOption & 1) > 0)
							{
								infoRow = new InfoRow();
								infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadBasicConfigureOK, num);
								this.updateDoorProcess(text, CommonStr.strUploadBasicConfigureOK, dfrmMultiThreadOperation.RunStatus.CompleteOK);
							}
							else if ((this.CommOperateOption & 2) > 0)
							{
								infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strUploadPrivilegesOK, num);
								this.updateDoorProcess(text, CommonStr.strUploadPrivilegesOK, dfrmMultiThreadOperation.RunStatus.CompleteOK);
							}
							this.addEventToLog1(infoRow);
							clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
						}
						else
						{
							InfoRow infoRow;
							if (num == wgGlobal.ERR_PRIVILEGES_OVER200K)
							{
								infoRow = new InfoRow();
								infoRow.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
								infoRow.information = string.Format("{0}--[{1:d}]", wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K), text);
								this.updateDoorProcess(text, wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K), dfrmMultiThreadOperation.RunStatus.CommFail);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.Download_Err_PRIVILEGES_OVER200K;
							}
							else
							{
								infoRow = new InfoRow();
								infoRow.category = 101;
								infoRow.desc = text;
								infoRow.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
								{
									CommonStr.strCommFail + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								infoRow.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
								{
									text,
									CommonStr.strCommFail + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								this.updateDoorProcess(text, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
							}
							this.addEventToLog1(infoRow);
						}
						this.updateEndTime(text, DateTime.Now.ToString("HH:mm:ss"));
						clsDoorsInfo = this.getWaitDoorInfo();
					}
					while (clsDoorsInfo != null);
					icController.Dispose();
				}
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000C2AA4 File Offset: 0x000C1AA4
		private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.exportToExcel(this.dataGridView1, this.Text);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000C2AB8 File Offset: 0x000C1AB8
		private void FingerSingleUpdateThread()
		{
			bool flag = false;
			this.arrDealtSN = null;
			this.arrDealtDoors = null;
			this.arrDealtSN = new ArrayList();
			this.arrDealtDoors = new ArrayList();
			this.waitDoors.Clear();
			this.arrDoorsInfo.Clear();
			new ArrayList();
			new ArrayList();
			new icController();
			DataView dataView = new DataView(this.dtDoors);
			for (int i = 0; i < this.arrSelectedDoors.Count; i++)
			{
				this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
				dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
				if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
				{
					this.arrDoorsInfo.Add(new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"], (int)byte.Parse(wgTools.SetObjToStr(dataView[0]["f_DoorNO"]))));
					if (string.IsNullOrEmpty(dataView[0]["f_IP"] as string))
					{
						flag = true;
					}
					this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
				}
			}
			wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
			wgTools.WriteLine("发送指纹指令");
			int num = 0;
			int num2 = 3;
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			DateTime dateTime2 = DateTime.Now;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			byte[] array = null;
			DateTime dateTime3 = DateTime.Now;
			new ArrayList();
			DateTime now = DateTime.Now;
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 96;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iCallReturn = 0;
			this.fingerDataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			if (flag)
			{
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					if (this.bStopComm())
					{
						return;
					}
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (string.IsNullOrEmpty(clsDoorsInfo.IP))
					{
						wgpacketWith1152_internal.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
						wgpacketWith1152_internal.GetNewXid();
						byte[] array2 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
						this.wait4Accelerate();
						wgUdpComm.udp_get_onlySend(array2, 300, wgpacketWith1152_internal.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
						arrayList5.Add(i);
						arrayList4.Add(wgUdpComm.getXidOfCommand(array2));
					}
				}
				Thread.Sleep(10);
				for (;;)
				{
					array = wgUdpComm.GetPacket();
					if (array == null && wgTools.bUDPCloud > 0)
					{
						array = wgTools.wgcloud.GetPacket4get();
					}
					if (array == null)
					{
						goto IL_0454;
					}
					num++;
					if (arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[(int)arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
						{
							clsDoorsInfo2.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							arrayList2.Add(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]);
							arrayList.Add(clsDoorsInfo2);
							dateTime2 = DateTime.Now;
							if (arrayList2.Count == this.arrDoorsInfo.Count)
							{
								break;
							}
						}
					}
				}
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				this.bComplete4FingerSingleUpdate = true;
				IL_0454:
				for (int j = 0; j < arrayList.Count; j++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = arrayList[j] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo3.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo3.DoorName, clsDoorsInfo3.ControllerSN),
							information = string.Format("{0}", CommonStr.strFingerprintUploadFingerprintOK)
						});
						this.updateDoorProcess(clsDoorsInfo3.DoorName, CommonStr.strFingerprintUploadFingerprintOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList.Clear();
			}
			dateTime3 = DateTime.Now;
			for (int i = 0; i < this.arrDoorsInfo.Count; i++)
			{
				if (this.bStopComm())
				{
					return;
				}
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
				if (!string.IsNullOrEmpty(clsDoorsInfo4.IP))
				{
					wgpacketWith1152_internal.iDevSnTo = (uint)clsDoorsInfo4.ControllerSN;
					wgpacketWith1152_internal.GetNewXid();
					byte[] array3 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
					this.wait4Accelerate();
					wgUdpComm.udp_get_onlySend(array3, 300, wgpacketWith1152_internal.xid, clsDoorsInfo4.IP, clsDoorsInfo4.Port, ref array);
					arrayList5.Add(i);
					arrayList4.Add(wgUdpComm.getXidOfCommand(array3));
				}
			}
			wgTools.WriteLine("发送指令完成");
			while (arrayList2.Count != this.arrDoorsInfo.Count)
			{
				do
				{
					array = wgUdpComm.GetPacket();
					if (array != null)
					{
						num++;
						if (arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo5 = this.arrDoorsInfo[(int)arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo5.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								clsDoorsInfo5.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
								arrayList2.Add(arrayList5[arrayList4.IndexOf(wgUdpComm.getXidOfCommand(array))]);
								arrayList.Add(clsDoorsInfo5);
								dateTime2 = DateTime.Now;
								if (arrayList2.Count == this.arrDoorsInfo.Count)
								{
									goto Block_26;
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < arrayList.Count; k++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo6 = arrayList[k] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo6.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								this.addEventToLog1(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", clsDoorsInfo6.DoorName, clsDoorsInfo6.ControllerSN),
									information = string.Format("{0}", CommonStr.strFingerprintUploadFingerprintOK)
								});
								this.updateDoorProcess(clsDoorsInfo6.DoorName, CommonStr.strFingerprintUploadFingerprintOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
							}
						}
						arrayList.Clear();
						Thread.Sleep(10);
					}
				}
				while (dateTime >= DateTime.Now);
				IL_07CE:
				wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList2.Count.ToString());
				if (!this.bComplete4Cardlost)
				{
					num2--;
					if (num2 > 0)
					{
						dateTime3 = DateTime.Now;
						for (int l = 0; l < this.arrDoorsInfo.Count; l++)
						{
							if (this.bStopComm())
							{
								return;
							}
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo7 = this.arrDoorsInfo[l] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo7.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								wgpacketWith1152_internal.iDevSnTo = (uint)clsDoorsInfo7.ControllerSN;
								wgpacketWith1152_internal.GetNewXid();
								byte[] array4 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
								this.wait4Accelerate();
								wgUdpComm.udp_get_onlySend(array4, 300, wgpacketWith1152_internal.xid, clsDoorsInfo7.IP, clsDoorsInfo7.Port, ref array);
								arrayList5.Add(l);
								arrayList4.Add(wgUdpComm.getXidOfCommand(array4));
							}
						}
						dateTime = DateTime.Now.AddSeconds(1.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num2);
						if (!this.bComplete4Cardlost)
						{
							continue;
						}
					}
				}
				IL_0904:
				wgTools.WriteLine(string.Format("rcvpktCount =" + num.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDoorAdjusted.Count =" + arrayList3.Count.ToString(), new object[0]));
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				for (int m = 0; m < arrayList.Count; m++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo8 = arrayList[m] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo8.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo8.DoorName, clsDoorsInfo8.ControllerSN),
							information = string.Format("{0}", CommonStr.strFingerprintUploadFingerprintOK)
						});
						this.updateDoorProcess(clsDoorsInfo8.DoorName, CommonStr.strFingerprintUploadFingerprintOK, dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime3.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList.Clear();
				for (int n = 0; n < this.arrDoorsInfo.Count; n++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo9 = this.arrDoorsInfo[n] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo9.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						clsDoorsInfo9.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo9.DoorName, clsDoorsInfo9.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo9.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				return;
				Block_26:
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				this.bComplete4FingerSingleUpdate = true;
				goto IL_07CE;
			}
			this.bComplete4Cardlost = true;
			this.bCompleteFullOK4Cardlost = true;
			this.bComplete4FingerSingleUpdate = true;
			goto IL_0904;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000C357C File Offset: 0x000C257C
		private int getControllerCount(DataView dv)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < dv.Count; i++)
			{
				if (arrayList.IndexOf(dv[i]["f_ControllerSN"]) < 0)
				{
					arrayList.Add(dv[i]["f_ControllerSN"]);
				}
			}
			return arrayList.Count;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000C35D8 File Offset: 0x000C25D8
		private void GetSwipeThreadMainWithParameters(object obj)
		{
			dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
			if (!this.bStopComm())
			{
				string text = clsDoorsInfo.DoorName;
				using (icController icController = new icController())
				{
					do
					{
						text = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", text.ToString(), icController.ControllerSN),
							information = string.Format("{0}", CommonStr.strGetCardRecords)
						});
						this.updateDoorProcess(text, CommonStr.strGetCardRecords, dfrmMultiThreadOperation.RunStatus.Start);
						this.updateStartTime(text, DateTime.Now.ToString("HH:mm:ss"));
						int num = -1;
						wgTools.WgDebugWrite(" using (icSwipeRecordMutithread swipe4GetRecords = new icSwipeRecordMutithread()) start  ", new object[0]);
						using (dfrmMultiThreadOperation.icSwipeRecordMutithread icSwipeRecordMutithread = new dfrmMultiThreadOperation.icSwipeRecordMutithread())
						{
							if (this.bStopComm())
							{
								return;
							}
							icSwipeRecordMutithread.Clear();
							icSwipeRecordMutithread.bDisplayProcess = false;
							num = icSwipeRecordMutithread.GetSwipeRecordsByDoorNameMultithread(icController.ControllerSN, icController.IP, icController.PORT, text);
						}
						if (num >= 0)
						{
							this.updateDisplayTotal(text, num.ToString());
							InfoRow infoRow = new InfoRow();
							infoRow.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
							infoRow.information = string.Format("{0}--[{1:d}]", CommonStr.strGetSwipeRecordOK, num);
							this.updateDoorProcess(text, string.Format("{0}[{1:d}]", CommonStr.strGetSwipeRecordOK, num), dfrmMultiThreadOperation.RunStatus.CompleteOK);
							this.addEventToLog1(infoRow);
							clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
						}
						else
						{
							InfoRow infoRow = new InfoRow();
							infoRow.category = 101;
							infoRow.desc = text;
							switch (num)
							{
							case -3:
							case -2:
								infoRow.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
								{
									CommonStr.strFailedToWriteIntoDatabase + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								infoRow.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
								{
									text,
									CommonStr.strFailedToWriteIntoDatabase + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								this.updateDoorProcess(text, CommonStr.strFailedToWriteIntoDatabase, dfrmMultiThreadOperation.RunStatus.CommFail);
								break;
							default:
								infoRow.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
								{
									CommonStr.strCommFail + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								infoRow.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
								{
									text,
									CommonStr.strCommFail + "  [" + num.ToString() + "]",
									CommonStr.strControllerSN,
									icController.ControllerSN,
									icController.IP
								});
								this.updateDoorProcess(text, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
								break;
							}
							this.addEventToLog1(infoRow);
							clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						}
						this.updateEndTime(text, DateTime.Now.ToString("HH:mm:ss"));
						clsDoorsInfo = this.getWaitDoorInfo();
					}
					while (clsDoorsInfo != null);
					icController.Dispose();
				}
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000C39BC File Offset: 0x000C29BC
		private int getTotalItems(DataView dv)
		{
			ArrayList arrayList = new ArrayList();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < dv.Count; i++)
			{
				if (arrayList.IndexOf(dv[i]["f_ControllerSN"]) < 0)
				{
					arrayList.Add(dv[i]["f_ControllerSN"]);
					num2 = 0;
					if (!string.IsNullOrEmpty(dv[i]["f_Total"].ToString()))
					{
						int.TryParse(dv[i]["f_Total"].ToString(), out num2);
					}
					num = num2 + num;
				}
			}
			return num;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000C3A58 File Offset: 0x000C2A58
		private int getWaitControllerID()
		{
			int num = -1;
			try
			{
				if (this.waitDoors.Count == 0)
				{
					return num;
				}
				lock (this.waitDoors.SyncRoot)
				{
					num = (int)this.waitDoors.Dequeue();
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000C3AC8 File Offset: 0x000C2AC8
		private dfrmMultiThreadOperation.clsDoorsInfo getWaitDoorInfo()
		{
			dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = null;
			try
			{
				if (this.waitDoors.Count == 0)
				{
					return clsDoorsInfo;
				}
				lock (this.waitDoors.SyncRoot)
				{
					clsDoorsInfo = this.waitDoors.Dequeue() as dfrmMultiThreadOperation.clsDoorsInfo;
				}
			}
			catch
			{
			}
			return clsDoorsInfo;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000C3B38 File Offset: 0x000C2B38
		private bool IsIPCanConnect(string ip)
		{
			return (this.arrIPPingOK.Count > 0 && this.arrIPPingOK.IndexOf(ip) >= 0) || (this.arrIPARPOK.Count > 0 && this.arrIPARPOK.IndexOf(ip) >= 0);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000C3B88 File Offset: 0x000C2B88
		private void loadDoorInfo()
		{
			string text = "SELECT t_b_Door.f_DoorID,  t_b_Door.f_DoorName, '' as f_Process,'' as f_Total, '' as f_StartTime, '' as f_EndTime, t_b_Door.f_DoorEnabled,t_b_Door.f_DoorDelay,t_b_Controller.f_ControllerNO, t_b_Controller.f_ControllerSN, t_b_Door.f_DoorNO, f_IP, f_Port, f_ZoneName ";
			text += " , 0 as f_RunStatus, t_b_Door.f_DoorControl, t_b_Controller.f_ControllerID, 0 as f_RecID , '' as f_cloudipInfo , '' as f_controllerTime  FROM (t_b_Door INNER JOIN t_b_Controller ON t_b_door.f_controllerid= t_b_controller.f_controllerid) LEFT OUTER JOIN t_b_Controller_Zone ON t_b_Controller.f_ZoneID = t_b_Controller_Zone.f_ZoneID where  t_b_Door.f_DoorEnabled >0 And  t_b_controller.f_Enabled >0 ";
			try
			{
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				this.dataGridView1.AutoGenerateColumns = false;
				this.dataGridView1.DataSource = dataSet.Tables[0];
				int i = 0;
				while (i < dataTable.Columns.Count && i < this.dataGridView1.ColumnCount)
				{
					this.dataGridView1.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
					i++;
				}
				this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dataGridView1.Columns[0].Visible = false;
				int num = 1;
				for (i = 0; i < dataTable.Rows.Count; i++)
				{
					if (this.arrSelectedDoors.Contains(dataTable.Rows[i]["f_DoorName"]))
					{
						dataTable.Rows[i]["f_RecID"] = num;
						num++;
					}
					else
					{
						dataTable.Rows[i]["f_DoorEnabled"] = 0;
					}
				}
				dataTable.AcceptChanges();
				DataView dataView = new DataView(dataTable);
				dataView.RowFilter = "f_DoorEnabled = 0";
				if (dataView.Count > 0)
				{
					for (i = dataView.Count - 1; i >= 0; i--)
					{
						dataView.Delete(i);
					}
				}
				dataTable.AcceptChanges();
				this.dtDoors = dataTable;
				dataView.RowFilter = "";
				dataView.Sort = "f_DoorNO ASC";
				ArrayList arrayList = new ArrayList();
				for (i = 0; i < dataView.Count; i++)
				{
					if (arrayList.IndexOf(dataView[i]["f_ControllerSN"]) < 0)
					{
						arrayList.Add(dataView[i]["f_ControllerSN"]);
						this.arrFirstDoor.Add(dataView[i]["f_DoorName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000C3EBC File Offset: 0x000C2EBC
		private void loadFingerDeviceInfo()
		{
			string text = "SELECT f_ControllerID,  f_FingerPrintName as f_DoorName, '' as f_Process,'' as f_Total, '' as f_StartTime, '' as f_EndTime, 1 as f_DoorEnabled,3 as f_DoorDelay,0 as f_ControllerNO, f_ControllerSN,1 as f_DoorNO, f_IP, f_Port,'' as  f_ZoneName ";
			text += " , 0 as f_RunStatus, 3 as f_DoorControl, 0 as f_ControllerID, 0 as f_RecID  FROM t_b_Controller_Fingerprint";
			try
			{
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				this.dataGridView1.AutoGenerateColumns = false;
				this.dataGridView1.DataSource = dataSet.Tables[0];
				int i = 0;
				while (i < dataTable.Columns.Count && i < this.dataGridView1.ColumnCount)
				{
					this.dataGridView1.Columns[i].DataPropertyName = dataTable.Columns[i].ColumnName;
					i++;
				}
				this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
				this.dataGridView1.Columns[0].Visible = false;
				int num = 1;
				for (i = 0; i < dataTable.Rows.Count; i++)
				{
					if (this.arrSelectedDoors.Contains(dataTable.Rows[i]["f_DoorName"]))
					{
						dataTable.Rows[i]["f_RecID"] = num;
						num++;
					}
					else
					{
						dataTable.Rows[i]["f_DoorEnabled"] = 0;
					}
				}
				dataTable.AcceptChanges();
				DataView dataView = new DataView(dataTable);
				dataView.RowFilter = "f_DoorEnabled = 0";
				if (dataView.Count > 0)
				{
					for (i = dataView.Count - 1; i >= 0; i--)
					{
						dataView.Delete(i);
					}
				}
				dataTable.AcceptChanges();
				this.dtDoors = dataTable;
				dataView.RowFilter = "";
				dataView.Sort = "f_DoorNO ASC";
				ArrayList arrayList = new ArrayList();
				for (i = 0; i < dataView.Count; i++)
				{
					if (arrayList.IndexOf(dataView[i]["f_ControllerSN"]) < 0)
					{
						arrayList.Add(dataView[i]["f_ControllerSN"]);
						this.arrFirstDoor.Add(dataView[i]["f_DoorName"]);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000C41F0 File Offset: 0x000C31F0
		private void OperateControllerAddDelPriConsoleThread()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.OperateControllerAddDelPriConsoleThread_P64();
				return;
			}
			this.arrDealtSN = null;
			this.arrDealtSN = new ArrayList();
			this.waitDoors.Clear();
			string text = "0";
			for (int i = 0; i < this.arrConsumerID.Count; i++)
			{
				text = text + "," + this.arrConsumerID[i].ToString();
			}
			string text2 = " SELECT a.* ";
			text2 += " FROM t_b_Controller a";
			ArrayList arrayList = new ArrayList();
			WGPacketPrivilege wgpacketPrivilege = new WGPacketPrivilege();
			wgpacketPrivilege.type = 35;
			wgpacketPrivilege.code = 32;
			wgpacketPrivilege.iDevSnFrom = 0U;
			wgpacketPrivilege.iCallReturn = 0;
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			byte[] array = null;
			DateTime now = DateTime.Now;
			new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			wgTools.WriteLine("OperateControllerCardLostConsoleThread Start");
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				dbCommand.CommandText = text2;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				new ArrayList();
				icPrivilege icPrivilege = new icPrivilege();
				int num = 0;
				string text3 = "";
				int num2 = 60000;
				long num3 = -1L;
				while (dbDataReader.Read())
				{
					if (!wgMjController.IsElevator((int)dbDataReader["f_ControllerSN"]) && this.arrSelectedDoors.IndexOf((int)dbDataReader["f_ControllerID"]) >= 0)
					{
						for (int j = 0; j < this.arrConsumerID.Count; j++)
						{
							MjRegisterCard privilegeOfOneCardByDB = icPrivilege.GetPrivilegeOfOneCardByDB((int)dbDataReader["f_ControllerID"], (int)this.arrConsumerID[j], ref num3, ref num, ref text3, ref num2);
							if (privilegeOfOneCardByDB != null && num3 > 0L)
							{
								if (privilegeOfOneCardByDB.CardID <= 0L && this.op == dfrmMultiThreadOperation.Operation.DelPri)
								{
									privilegeOfOneCardByDB.IsDeleted = true;
								}
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], privilegeOfOneCardByDB, num3);
								this.arrDoorsInfo.Add(clsDoorsInfo);
								if (arrayList4.IndexOf((int)dbDataReader["f_ControllerSN"]) < 0)
								{
									arrayList4.Add((int)dbDataReader["f_ControllerSN"]);
									arrayList5.Add((int)dbDataReader["f_ControllerID"]);
									wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
									wgpacketPrivilege.mjrc = privilegeOfOneCardByDB;
									wgpacketPrivilege.GetNewXid();
									byte[] array2 = wgpacketPrivilege.ToBytes(wgUdpComm.udpPort);
									this.wait4Accelerate();
									wgUdpComm.udp_get_onlySend(array2, 300, wgpacketPrivilege.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
									arrayList3.Add(this.arrDoorsInfo.Count - 1);
									arrayList2.Add(wgUdpComm.getXidOfCommand(array2));
								}
							}
						}
					}
				}
				dbDataReader.Close();
				dbConnection.Close();
				wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Completed");
				int num4 = 0;
				int num5 = 3;
				DateTime dateTime = DateTime.Now.AddSeconds((double)(2 * (1 + text.Length / 10)));
				DateTime now2 = DateTime.Now;
				ArrayList arrayList6 = new ArrayList();
				for (;;)
				{
					int num6 = 0;
					do
					{
						array = wgUdpComm.GetPacket();
						if (array == null && wgTools.bUDPCloud > 0)
						{
							array = wgTools.wgcloud.GetPacket4get();
						}
						if (array != null)
						{
							num6 = 0;
							num4++;
							if (arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList.IndexOf(arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
								{
									clsDoorsInfo2.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
									arrayList.Add(arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]);
									DateTime now3 = DateTime.Now;
									if (arrayList.Count == this.arrDoorsInfo.Count)
									{
										goto Block_21;
									}
									bool flag = true;
									for (int k = 0; k < this.arrDoorsInfo.Count; k++)
									{
										dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = this.arrDoorsInfo[k] as dfrmMultiThreadOperation.clsDoorsInfo;
										if (clsDoorsInfo3.ControllerSN == clsDoorsInfo2.ControllerSN && clsDoorsInfo3.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
										{
											flag = false;
											wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo3.ControllerSN;
											wgpacketPrivilege.mjrc = clsDoorsInfo3.Mjrc;
											wgpacketPrivilege.GetNewXid();
											byte[] array3 = wgpacketPrivilege.ToBytes(wgUdpComm.udpPort);
											wgUdpComm.udp_get_onlySend(array3, 300, wgpacketPrivilege.xid, clsDoorsInfo3.IP, clsDoorsInfo3.Port, ref array);
											arrayList3.Add(k);
											arrayList2.Add(wgUdpComm.getXidOfCommand(array3));
											break;
										}
									}
									if (flag)
									{
										arrayList4.Remove(clsDoorsInfo2.ControllerSN);
									}
								}
							}
						}
						else
						{
							Thread.Sleep(1);
							num6++;
							if (num6 > 2000)
							{
								break;
							}
						}
					}
					while (dateTime >= DateTime.Now);
					IL_05FE:
					wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
					if (this.bComplete4Cardlost)
					{
						break;
					}
					num5--;
					if (num5 <= 0)
					{
						break;
					}
					arrayList6.Clear();
					for (int l = 0; l < arrayList4.Count; l++)
					{
						arrayList6.Add(arrayList4[l]);
					}
					for (int m = 0; m < this.arrDoorsInfo.Count; m++)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[m] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (arrayList6.IndexOf(clsDoorsInfo4.ControllerSN) >= 0 && clsDoorsInfo4.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
						{
							arrayList6.Remove(clsDoorsInfo4.ControllerSN);
							wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo4.ControllerSN;
							wgpacketPrivilege.mjrc = clsDoorsInfo4.Mjrc;
							wgpacketPrivilege.GetNewXid();
							byte[] array4 = wgpacketPrivilege.ToBytes(wgUdpComm.udpPort);
							this.wait4Accelerate();
							wgUdpComm.udp_get_onlySend(array4, 300, wgpacketPrivilege.xid, clsDoorsInfo4.IP, clsDoorsInfo4.Port, ref array);
							arrayList3.Add(m);
							arrayList2.Add(wgUdpComm.getXidOfCommand(array4));
						}
					}
					dateTime = DateTime.Now.AddSeconds(2.0);
					wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num5);
					if (this.bComplete4Cardlost)
					{
						break;
					}
					continue;
					Block_21:
					this.bComplete4Cardlost = true;
					this.bCompleteFullOK4Cardlost = true;
					goto IL_05FE;
				}
				wgTools.WriteLine(string.Format("rcvpktCount =" + num4.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDoorOpened.Count =" + arrayList.Count.ToString(), new object[0]));
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				this.bCompleteFullOK4AddDelPri = this.bComplete4Cardlost;
				this.bComplete4AddDelPri = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.bComplete4Cardlost = true;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000C4A50 File Offset: 0x000C3A50
		private void OperateControllerAddDelPriConsoleThread_P64()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.arrDealtSN = null;
				this.arrDealtSN = new ArrayList();
				this.waitDoors.Clear();
				string text = "0";
				for (int i = 0; i < this.arrConsumerID.Count; i++)
				{
					text = text + "," + this.arrConsumerID[i].ToString();
				}
				string text2 = " SELECT a.* ";
				text2 += " FROM t_b_Controller a";
				ArrayList arrayList = new ArrayList();
				wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
				wgUdpComm wgUdpComm = new wgUdpComm();
				Thread.Sleep(300);
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				byte[] array = null;
				DateTime now = DateTime.Now;
				new ArrayList();
				ArrayList arrayList4 = new ArrayList();
				ArrayList arrayList5 = new ArrayList();
				wgTools.WriteLine("OperateControllerCardLostConsoleThread Start");
				try
				{
					DbConnection dbConnection;
					DbCommand dbCommand;
					if (wgAppConfig.IsAccessDB)
					{
						dbConnection = new OleDbConnection(wgAppConfig.dbConString);
						dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
					}
					else
					{
						dbConnection = new SqlConnection(wgAppConfig.dbConString);
						dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					}
					dbConnection.Open();
					dbCommand.CommandText = text2;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					new ArrayList();
					icPrivilege icPrivilege = new icPrivilege();
					int num = 0;
					string text3 = "";
					int num2 = 60000;
					long num3 = -1L;
					while (dbDataReader.Read())
					{
						if (!wgMjController.IsElevator((int)dbDataReader["f_ControllerSN"]) && this.arrSelectedDoors.IndexOf((int)dbDataReader["f_ControllerID"]) >= 0)
						{
							for (int j = 0; j < this.arrConsumerID.Count; j++)
							{
								MjRegisterCard privilegeOfOneCardByDB = icPrivilege.GetPrivilegeOfOneCardByDB((int)dbDataReader["f_ControllerID"], (int)this.arrConsumerID[j], ref num3, ref num, ref text3, ref num2);
								if (privilegeOfOneCardByDB != null && num3 > 0L)
								{
									if (privilegeOfOneCardByDB.CardID <= 0L && this.op == dfrmMultiThreadOperation.Operation.DelPri)
									{
										privilegeOfOneCardByDB.IsDeleted = true;
									}
									privilegeOfOneCardByDB.CardID = num3;
									dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], privilegeOfOneCardByDB, num3);
									this.arrDoorsInfo.Add(clsDoorsInfo);
									if (arrayList4.IndexOf((int)dbDataReader["f_ControllerSN"]) < 0)
									{
										arrayList4.Add((int)dbDataReader["f_ControllerSN"]);
										arrayList5.Add((int)dbDataReader["f_ControllerID"]);
										wgpacketShort.code = 80;
										if (privilegeOfOneCardByDB.IsDeleted)
										{
											wgpacketShort.code = 82;
										}
										wgpacketShort.iDevSn = (uint)clsDoorsInfo.ControllerSN;
										Array.Copy(privilegeOfOneCardByDB.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
										Array.Copy(privilegeOfOneCardByDB.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
										this.wait4Accelerate();
										byte[] array2 = wgpacketShort.ToBytes();
										wgUdpComm.udp_get_onlySend(array2, 300, wgpacketShort.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
										arrayList3.Add(this.arrDoorsInfo.Count - 1);
										arrayList2.Add(wgUdpComm.getXidOfCommand(array2));
									}
								}
							}
						}
					}
					dbDataReader.Close();
					dbConnection.Close();
					wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Completed");
					int num4 = 0;
					int num5 = 3;
					DateTime dateTime = DateTime.Now.AddSeconds(30.0);
					DateTime now2 = DateTime.Now;
					ArrayList arrayList6 = new ArrayList();
					for (;;)
					{
						int num6 = 0;
						do
						{
							array = wgUdpComm.GetPacket();
							if (array == null && wgTools.bUDPCloud > 0)
							{
								array = wgTools.wgcloud.GetPacket4get();
							}
							if (array != null)
							{
								num6 = 0;
								num4++;
								if (arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList.IndexOf(arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
								{
									dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo;
									if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
									{
										clsDoorsInfo2.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
										arrayList.Add(arrayList3[arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array))]);
										DateTime now3 = DateTime.Now;
										if (arrayList.Count == this.arrDoorsInfo.Count)
										{
											goto Block_22;
										}
										bool flag = true;
										for (int k = 0; k < this.arrDoorsInfo.Count; k++)
										{
											dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = this.arrDoorsInfo[k] as dfrmMultiThreadOperation.clsDoorsInfo;
											if (clsDoorsInfo3.ControllerSN == clsDoorsInfo2.ControllerSN && clsDoorsInfo3.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
											{
												flag = false;
												wgpacketShort.code = 80;
												if (clsDoorsInfo3.Mjrc.IsDeleted)
												{
													wgpacketShort.code = 82;
												}
												wgpacketShort.iDevSn = (uint)clsDoorsInfo3.ControllerSN;
												Array.Copy(clsDoorsInfo3.Mjrc.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
												Array.Copy(clsDoorsInfo3.Mjrc.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
												wgTools.delay4SendNextCommand_P64();
												byte[] array3 = wgpacketShort.ToBytes();
												wgUdpComm.udp_get_onlySend(array3, 300, wgpacketShort.xid, clsDoorsInfo2.IP, clsDoorsInfo2.Port, ref array);
												arrayList3.Add(k);
												arrayList2.Add(wgUdpComm.getXidOfCommand(array3));
												break;
											}
										}
										if (flag)
										{
											arrayList4.Remove(clsDoorsInfo2.ControllerSN);
										}
									}
								}
							}
							else
							{
								Thread.Sleep(1);
								num6++;
								if (num6 > 6000)
								{
									break;
								}
							}
						}
						while (dateTime >= DateTime.Now);
						IL_0663:
						wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
						if (this.bComplete4Cardlost)
						{
							break;
						}
						num5--;
						if (num5 <= 0)
						{
							break;
						}
						arrayList6.Clear();
						for (int l = 0; l < arrayList4.Count; l++)
						{
							arrayList6.Add(arrayList4[l]);
						}
						for (int m = 0; m < this.arrDoorsInfo.Count; m++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[m] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (arrayList6.IndexOf(clsDoorsInfo4.ControllerSN) >= 0 && clsDoorsInfo4.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								arrayList6.Remove(clsDoorsInfo4.ControllerSN);
								wgpacketShort.code = 80;
								if (clsDoorsInfo4.Mjrc.IsDeleted)
								{
									wgpacketShort.code = 82;
								}
								wgpacketShort.iDevSn = (uint)clsDoorsInfo4.ControllerSN;
								Array.Copy(clsDoorsInfo4.Mjrc.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
								Array.Copy(clsDoorsInfo4.Mjrc.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
								byte[] array4 = wgpacketShort.ToBytes();
								this.wait4Accelerate();
								wgTools.delay4SendNextCommand_P64();
								wgUdpComm.udp_get_onlySend(array4, 300, wgpacketShort.xid, clsDoorsInfo4.IP, clsDoorsInfo4.Port, ref array);
								arrayList3.Add(m);
								arrayList2.Add(wgUdpComm.getXidOfCommand(array4));
							}
						}
						dateTime = DateTime.Now.AddSeconds(30.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num5);
						if (this.bComplete4Cardlost)
						{
							break;
						}
						continue;
						Block_22:
						this.bComplete4Cardlost = true;
						this.bCompleteFullOK4Cardlost = true;
						goto IL_0663;
					}
					wgTools.WriteLine(string.Format("rcvpktCount =" + num4.ToString(), new object[0]));
					wgTools.WriteLine(string.Format("arrDoorOpened.Count =" + arrayList.Count.ToString(), new object[0]));
					wgUdpComm.Close();
					wgUdpComm.Dispose();
					this.bCompleteFullOK4AddDelPri = this.bComplete4Cardlost;
					this.bComplete4AddDelPri = true;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				finally
				{
					this.bComplete4Cardlost = true;
				}
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000C5360 File Offset: 0x000C4360
		private void OperateControllerAddDelPriConsoleThread2222()
		{
			this.arrDealtSN = null;
			this.arrDealtSN = new ArrayList();
			this.waitDoors.Clear();
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				string text = " SELECT a.* ";
				text += " FROM t_b_Controller a";
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					if (this.arrSelectedDoors.IndexOf((int)dbDataReader["f_ControllerID"]) >= 0)
					{
						lock (this.waitDoors.SyncRoot)
						{
							this.waitDoors.Enqueue(new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"]));
						}
					}
				}
				dbDataReader.Close();
				dbConnection.Close();
				int count = this.waitDoors.Count;
				if (this.waitDoors.Count > 0)
				{
					int num = 0;
					for (;;)
					{
						dfrmMultiThreadOperation.clsDoorsInfo waitDoorInfo = this.getWaitDoorInfo();
						if (waitDoorInfo == null)
						{
							break;
						}
						num++;
						switch (this.op)
						{
						case dfrmMultiThreadOperation.Operation.DelPri:
						case dfrmMultiThreadOperation.Operation.AddPri:
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.DownloadAddDelPriUserThreadMainWithParameters), waitDoorInfo);
							break;
						}
					}
				}
				int num2 = -1;
				int num3 = 15;
				DateTime dateTime = DateTime.Now.AddSeconds((double)num3);
				do
				{
					if (count == 0)
					{
						this.bComplete4AddDelPri = true;
						this.bCompleteFullOK4AddDelPri = true;
					}
					else
					{
						if (this.qSuccess4AddDelPri.Count + this.qFail4AddDelPri.Count == count)
						{
							this.bComplete4AddDelPri = true;
							if (this.qFail4AddDelPri.Count == 0)
							{
								this.bComplete4AddDelPri = true;
								this.bCompleteFullOK4AddDelPri = true;
							}
						}
						if (num2 != this.qSuccess4AddDelPri.Count + this.qFail4AddDelPri.Count)
						{
							num2 = this.qSuccess4AddDelPri.Count + this.qFail4AddDelPri.Count;
							dateTime = DateTime.Now.AddSeconds((double)num3);
						}
					}
					if (this.bComplete4AddDelPri)
					{
						break;
					}
					Thread.Sleep(100);
				}
				while (dateTime >= DateTime.Now);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.bComplete4AddDelPri = true;
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000C5650 File Offset: 0x000C4650
		private void OperateControllerCardLostConsoleThread()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.OperateControllerCardLostConsoleThread_P64();
				return;
			}
			this.arrDealtSN = null;
			this.arrDealtSN = new ArrayList();
			this.waitDoors.Clear();
			string text = " SELECT a.* ";
			text = text + " FROM t_b_Controller a, t_d_Privilege b WHERE b.f_ConsumerID= " + this.ConsumerID4Cardlost.ToString() + " AND  a.f_ControllerID = b.f_ControllerID  ";
			ArrayList arrayList = new ArrayList();
			WGPacketPrivilege wgpacketPrivilege = new WGPacketPrivilege();
			wgpacketPrivilege.type = 35;
			wgpacketPrivilege.code = 32;
			wgpacketPrivilege.iDevSnFrom = 0U;
			wgpacketPrivilege.iCallReturn = 0;
			this.wgudpOperateControllerCardLost = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			DateTime now = DateTime.Now;
			new ArrayList();
			MjRegisterCard mjRegisterCard = new MjRegisterCard();
			if (!string.IsNullOrEmpty(this.oldCardNO4Cardlost))
			{
				mjRegisterCard.CardID = long.Parse(this.oldCardNO4Cardlost);
				mjRegisterCard.IsDeleted = true;
			}
			MjRegisterCard mjRegisterCard2 = new MjRegisterCard();
			if (!string.IsNullOrEmpty(this.newCardNO4Cardlost))
			{
				mjRegisterCard2.CardID = long.Parse(this.newCardNO4Cardlost);
				mjRegisterCard2.IsDeleted = true;
			}
			wgTools.WriteLine("OperateControllerCardLostConsoleThread Start");
			try
			{
				DbConnection dbConnection;
				DbCommand dbCommand;
				if (wgAppConfig.IsAccessDB)
				{
					dbConnection = new OleDbConnection(wgAppConfig.dbConString);
					dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
				}
				else
				{
					dbConnection = new SqlConnection(wgAppConfig.dbConString);
					dbCommand = new SqlCommand("", dbConnection as SqlConnection);
				}
				dbConnection.Open();
				dbCommand.CommandText = text;
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				ArrayList arrayList4 = new ArrayList();
				icPrivilege icPrivilege = new icPrivilege();
				int num = 0;
				string text2 = "";
				int num2 = 60000;
				long num3 = -1L;
				DateTime dateTime = DateTime.Now.AddSeconds(3.0);
				while (dbDataReader.Read())
				{
					if (arrayList4.IndexOf((int)dbDataReader["f_ControllerID"]) < 0)
					{
						arrayList4.Add((int)dbDataReader["f_ControllerID"]);
						if (!wgMjController.IsElevator((int)dbDataReader["f_ControllerSN"]))
						{
							MjRegisterCard privilegeOfOneCardByDB = icPrivilege.GetPrivilegeOfOneCardByDB((int)dbDataReader["f_ControllerID"], this.ConsumerID4Cardlost, ref num3, ref num, ref text2, ref num2);
							if (!string.IsNullOrEmpty(this.oldCardNO4Cardlost))
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], mjRegisterCard, mjRegisterCard.CardID);
								this.arrDoorsInfo.Add(clsDoorsInfo);
								wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
								wgpacketPrivilege.mjrc = mjRegisterCard;
								wgpacketPrivilege.GetNewXid();
								byte[] array = wgpacketPrivilege.ToBytes(this.wgudpOperateControllerCardLost.udpPort);
								this.wait4Accelerate();
								dfrmMultiThreadOperation.clsControlInfo clsControlInfo = new dfrmMultiThreadOperation.clsControlInfo();
								clsControlInfo.bytToSend = array;
								clsControlInfo.IP = clsDoorsInfo.IP;
								clsControlInfo.Port = clsDoorsInfo.Port;
								ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo);
								arrayList3.Add(this.arrDoorsInfo.Count - 1);
								arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array));
							}
							if (num3 > 0L)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], privilegeOfOneCardByDB, num3);
								this.arrDoorsInfo.Add(clsDoorsInfo2);
								wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo2.ControllerSN;
								wgpacketPrivilege.mjrc = privilegeOfOneCardByDB;
								wgpacketPrivilege.GetNewXid();
								byte[] array = wgpacketPrivilege.ToBytes(this.wgudpOperateControllerCardLost.udpPort);
								this.wait4Accelerate();
								dfrmMultiThreadOperation.clsControlInfo clsControlInfo2 = new dfrmMultiThreadOperation.clsControlInfo();
								clsControlInfo2.bytToSend = array;
								clsControlInfo2.IP = clsDoorsInfo2.IP;
								clsControlInfo2.Port = clsDoorsInfo2.Port;
								ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo2);
								arrayList3.Add(this.arrDoorsInfo.Count - 1);
								arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array));
							}
						}
					}
				}
				dbDataReader.Close();
				ArrayList arrayList5 = new ArrayList();
				if (this.delPrivilegeController.Count > 0)
				{
					for (int i = 0; i < this.delPrivilegeController.Count; i++)
					{
						if (arrayList4.IndexOf(this.delPrivilegeController[i]) < 0 && arrayList5.IndexOf(this.delPrivilegeController[i]) < 0)
						{
							arrayList5.Add(this.delPrivilegeController[i]);
						}
					}
				}
				this.delPrivilegeController = arrayList5;
				if (this.delPrivilegeController.Count > 0)
				{
					if (num3 < 0L)
					{
						text = " SELECT f_CardNO, f_ConsumerName ";
						num3 = (long)wgAppConfig.getValBySql(text + " FROM t_b_Consumer WHERE f_ConsumerID =  " + this.ConsumerID4Cardlost.ToString());
						if (num3 > 0L)
						{
							mjRegisterCard2.CardID = num3;
						}
					}
					if (num3 > 0L)
					{
						text = " SELECT a.* ";
						text += " FROM t_b_Controller a";
						dbCommand.CommandText = text;
						dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							if (this.delPrivilegeController.IndexOf((int)dbDataReader["f_ControllerID"]) >= 0)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], mjRegisterCard2, num3);
								this.arrDoorsInfo.Add(clsDoorsInfo3);
								wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo3.ControllerSN;
								wgpacketPrivilege.mjrc = clsDoorsInfo3.Mjrc;
								wgpacketPrivilege.GetNewXid();
								byte[] array2 = wgpacketPrivilege.ToBytes(this.wgudpOperateControllerCardLost.udpPort);
								this.wait4Accelerate();
								dfrmMultiThreadOperation.clsControlInfo clsControlInfo3 = new dfrmMultiThreadOperation.clsControlInfo();
								clsControlInfo3.bytToSend = array2;
								clsControlInfo3.IP = clsDoorsInfo3.IP;
								clsControlInfo3.Port = clsDoorsInfo3.Port;
								ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo3);
								arrayList3.Add(this.arrDoorsInfo.Count - 1);
								arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array2));
							}
						}
						dbDataReader.Close();
					}
				}
				dbConnection.Close();
				wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Completed");
				int num4 = 0;
				int num5 = 3;
				DateTime now2 = DateTime.Now;
				if (num3 > 0L || !string.IsNullOrEmpty(this.oldCardNO4Cardlost))
				{
					do
					{
						byte[] array3 = this.wgudpOperateControllerCardLost.GetPacket();
						if (array3 == null && wgTools.bUDPCloud > 0)
						{
							array3 = wgTools.wgcloud.GetPacket4get();
						}
						if (array3 != null)
						{
							num4++;
							if (arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3)) >= 0 && arrayList.IndexOf(arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]) < 0 && (this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]] as dfrmMultiThreadOperation.clsDoorsInfo).commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								(this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]] as dfrmMultiThreadOperation.clsDoorsInfo).commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
								arrayList.Add(arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]);
								DateTime now3 = DateTime.Now;
								if (arrayList.Count == this.arrDoorsInfo.Count)
								{
									this.bComplete4Cardlost = true;
									this.bCompleteFullOK4Cardlost = true;
									goto IL_0841;
								}
							}
						}
						else
						{
							Thread.Sleep(10);
						}
						if (dateTime >= DateTime.Now)
						{
							continue;
						}
						IL_0841:
						wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
						if (this.bComplete4Cardlost)
						{
							break;
						}
						num5--;
						if (num5 <= 0)
						{
							break;
						}
						for (int j = 0; j < this.arrDoorsInfo.Count; j++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[j] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo4.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								wgpacketPrivilege.iDevSnTo = (uint)clsDoorsInfo4.ControllerSN;
								wgpacketPrivilege.mjrc = clsDoorsInfo4.Mjrc;
								wgpacketPrivilege.GetNewXid();
								byte[] array4 = wgpacketPrivilege.ToBytes(this.wgudpOperateControllerCardLost.udpPort);
								this.wait4Accelerate();
								dfrmMultiThreadOperation.clsControlInfo clsControlInfo4 = new dfrmMultiThreadOperation.clsControlInfo();
								clsControlInfo4.bytToSend = array4;
								clsControlInfo4.IP = clsDoorsInfo4.IP;
								clsControlInfo4.Port = clsDoorsInfo4.Port;
								ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo4);
								arrayList3.Add(j);
								arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array4));
							}
						}
						dateTime = DateTime.Now.AddSeconds(1.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num5);
					}
					while (!this.bComplete4Cardlost);
				}
				wgTools.WriteLine(string.Format("rcvpktCount =" + num4.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDealtItemOK.Count =" + arrayList.Count.ToString(), new object[0]));
				this.wgudpOperateControllerCardLost.Close();
				this.wgudpOperateControllerCardLost.Dispose();
				wgTools.WgDebugWrite(string.Format("Privilege Send {0}, Receive {1}", this.arrDoorsInfo.Count, arrayList.Count), new object[0]);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.bComplete4Cardlost = true;
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000C60DC File Offset: 0x000C50DC
		private void OperateControllerCardLostConsoleThread_P64()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.arrDealtSN = null;
				this.arrDealtSN = new ArrayList();
				this.waitDoors.Clear();
				string text = " SELECT a.* ";
				text = text + " FROM t_b_Controller a, t_d_Privilege b WHERE b.f_ConsumerID= " + this.ConsumerID4Cardlost.ToString() + " AND  a.f_ControllerID = b.f_ControllerID  ";
				ArrayList arrayList = new ArrayList();
				wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
				this.wgudpOperateControllerCardLost = new wgUdpComm();
				Thread.Sleep(300);
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = new ArrayList();
				DateTime now = DateTime.Now;
				new ArrayList();
				MjRegisterCard mjRegisterCard = new MjRegisterCard();
				if (!string.IsNullOrEmpty(this.oldCardNO4Cardlost))
				{
					mjRegisterCard.CardID = long.Parse(this.oldCardNO4Cardlost);
					mjRegisterCard.IsDeleted = true;
				}
				MjRegisterCard mjRegisterCard2 = new MjRegisterCard();
				if (!string.IsNullOrEmpty(this.newCardNO4Cardlost))
				{
					mjRegisterCard2.CardID = long.Parse(this.newCardNO4Cardlost);
					mjRegisterCard2.IsDeleted = true;
				}
				wgTools.WriteLine("OperateControllerCardLostConsoleThread Start");
				try
				{
					DbConnection dbConnection;
					DbCommand dbCommand;
					if (wgAppConfig.IsAccessDB)
					{
						dbConnection = new OleDbConnection(wgAppConfig.dbConString);
						dbCommand = new OleDbCommand("", dbConnection as OleDbConnection);
					}
					else
					{
						dbConnection = new SqlConnection(wgAppConfig.dbConString);
						dbCommand = new SqlCommand("", dbConnection as SqlConnection);
					}
					dbConnection.Open();
					dbCommand.CommandText = text;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					ArrayList arrayList4 = new ArrayList();
					icPrivilege icPrivilege = new icPrivilege();
					int num = 0;
					string text2 = "";
					int num2 = 60000;
					long num3 = -1L;
					DateTime dateTime = DateTime.Now.AddSeconds(3.0);
					while (dbDataReader.Read())
					{
						if (arrayList4.IndexOf((int)dbDataReader["f_ControllerID"]) < 0)
						{
							arrayList4.Add((int)dbDataReader["f_ControllerID"]);
							if (!wgMjController.IsElevator((int)dbDataReader["f_ControllerSN"]))
							{
								MjRegisterCard mjRegisterCard3 = icPrivilege.GetPrivilegeOfOneCardByDB((int)dbDataReader["f_ControllerID"], this.ConsumerID4Cardlost, ref num3, ref num, ref text2, ref num2);
								if (!string.IsNullOrEmpty(this.oldCardNO4Cardlost))
								{
									dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], mjRegisterCard, mjRegisterCard.CardID);
									this.arrDoorsInfo.Add(clsDoorsInfo);
									wgpacketShort.code = 80;
									if (mjRegisterCard.IsDeleted)
									{
										wgpacketShort.code = 82;
									}
									wgpacketShort.iDevSn = (uint)clsDoorsInfo.ControllerSN;
									Array.Copy(mjRegisterCard.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
									Array.Copy(mjRegisterCard.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
									byte[] array = wgpacketShort.ToBytes();
									this.wait4Accelerate();
									dfrmMultiThreadOperation.clsControlInfo clsControlInfo = new dfrmMultiThreadOperation.clsControlInfo();
									clsControlInfo.bytToSend = array;
									clsControlInfo.IP = clsDoorsInfo.IP;
									clsControlInfo.Port = clsDoorsInfo.Port;
									ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo);
									arrayList3.Add(this.arrDoorsInfo.Count - 1);
									arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array));
								}
								if (num3 > 0L)
								{
									dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], mjRegisterCard3, num3);
									this.arrDoorsInfo.Add(clsDoorsInfo2);
									wgpacketShort.code = 80;
									if (mjRegisterCard.IsDeleted)
									{
										wgpacketShort.code = 82;
									}
									wgpacketShort.iDevSn = (uint)clsDoorsInfo2.ControllerSN;
									Array.Copy(mjRegisterCard3.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
									Array.Copy(mjRegisterCard3.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
									byte[] array = wgpacketShort.ToBytes();
									this.wait4Accelerate();
									dfrmMultiThreadOperation.clsControlInfo clsControlInfo2 = new dfrmMultiThreadOperation.clsControlInfo();
									clsControlInfo2.bytToSend = array;
									clsControlInfo2.IP = clsDoorsInfo2.IP;
									clsControlInfo2.Port = clsDoorsInfo2.Port;
									ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo2);
									arrayList3.Add(this.arrDoorsInfo.Count - 1);
									arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array));
								}
							}
						}
					}
					dbDataReader.Close();
					ArrayList arrayList5 = new ArrayList();
					if (this.delPrivilegeController.Count > 0)
					{
						for (int i = 0; i < this.delPrivilegeController.Count; i++)
						{
							if (arrayList4.IndexOf(this.delPrivilegeController[i]) < 0 && arrayList5.IndexOf(this.delPrivilegeController[i]) < 0)
							{
								arrayList5.Add(this.delPrivilegeController[i]);
							}
						}
					}
					this.delPrivilegeController = arrayList5;
					if (this.delPrivilegeController.Count > 0)
					{
						if (num3 < 0L)
						{
							text = " SELECT f_CardNO, f_ConsumerName ";
							num3 = (long)wgAppConfig.getValBySql(text + " FROM t_b_Consumer WHERE f_ConsumerID =  " + this.ConsumerID4Cardlost.ToString());
							if (num3 > 0L)
							{
								mjRegisterCard2.CardID = num3;
							}
						}
						if (num3 > 0L)
						{
							text = " SELECT a.* ";
							text += " FROM t_b_Controller a";
							dbCommand.CommandText = text;
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								if (this.delPrivilegeController.IndexOf((int)dbDataReader["f_ControllerID"]) >= 0)
								{
									dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = new dfrmMultiThreadOperation.clsDoorsInfo((int)dbDataReader["f_ControllerID"], "DoorName", (int)dbDataReader["f_ControllerSN"], dbDataReader["f_IP"] as string, (int)dbDataReader["f_PORT"], mjRegisterCard2, num3);
									this.arrDoorsInfo.Add(clsDoorsInfo3);
									MjRegisterCard mjRegisterCard3 = clsDoorsInfo3.Mjrc;
									wgpacketShort.code = 80;
									if (mjRegisterCard.IsDeleted)
									{
										wgpacketShort.code = 82;
									}
									wgpacketShort.iDevSn = (uint)clsDoorsInfo3.ControllerSN;
									Array.Copy(mjRegisterCard3.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
									Array.Copy(mjRegisterCard3.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
									byte[] array2 = wgpacketShort.ToBytes();
									this.wait4Accelerate();
									dfrmMultiThreadOperation.clsControlInfo clsControlInfo3 = new dfrmMultiThreadOperation.clsControlInfo();
									clsControlInfo3.bytToSend = array2;
									clsControlInfo3.IP = clsDoorsInfo3.IP;
									clsControlInfo3.Port = clsDoorsInfo3.Port;
									ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo3);
									arrayList3.Add(this.arrDoorsInfo.Count - 1);
									arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array2));
								}
							}
							dbDataReader.Close();
						}
					}
					dbConnection.Close();
					wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Completed");
					int num4 = 0;
					int num5 = 3;
					DateTime now2 = DateTime.Now;
					if (num3 > 0L || !string.IsNullOrEmpty(this.oldCardNO4Cardlost))
					{
						do
						{
							byte[] array3 = this.wgudpOperateControllerCardLost.GetPacket();
							if (array3 == null && wgTools.bUDPCloud > 0)
							{
								array3 = wgTools.wgcloud.GetPacket4get();
							}
							if (array3 != null)
							{
								num4++;
								if (arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3)) >= 0 && arrayList.IndexOf(arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]) < 0 && (this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]] as dfrmMultiThreadOperation.clsDoorsInfo).commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
								{
									(this.arrDoorsInfo[(int)arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]] as dfrmMultiThreadOperation.clsDoorsInfo).commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
									arrayList.Add(arrayList3[arrayList2.IndexOf(this.wgudpOperateControllerCardLost.getXidOfCommand(array3))]);
									DateTime now3 = DateTime.Now;
									if (arrayList.Count == this.arrDoorsInfo.Count)
									{
										this.bComplete4Cardlost = true;
										this.bCompleteFullOK4Cardlost = true;
										goto IL_08C5;
									}
								}
							}
							else
							{
								Thread.Sleep(10);
							}
							if (dateTime >= DateTime.Now)
							{
								continue;
							}
							IL_08C5:
							wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
							if (this.bComplete4Cardlost)
							{
								break;
							}
							num5--;
							if (num5 <= 0)
							{
								break;
							}
							for (int j = 0; j < this.arrDoorsInfo.Count; j++)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[j] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo4.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
								{
									MjRegisterCard mjRegisterCard3 = clsDoorsInfo4.Mjrc;
									wgpacketShort.code = 80;
									if (mjRegisterCard.IsDeleted)
									{
										wgpacketShort.code = 82;
									}
									wgpacketShort.iDevSn = (uint)clsDoorsInfo4.ControllerSN;
									Array.Copy(mjRegisterCard3.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
									Array.Copy(mjRegisterCard3.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
									byte[] array4 = wgpacketShort.ToBytes();
									this.wait4Accelerate();
									dfrmMultiThreadOperation.clsControlInfo clsControlInfo4 = new dfrmMultiThreadOperation.clsControlInfo();
									clsControlInfo4.bytToSend = array4;
									clsControlInfo4.IP = clsDoorsInfo4.IP;
									clsControlInfo4.Port = clsDoorsInfo4.Port;
									ThreadPool.QueueUserWorkItem(new WaitCallback(this.SendData), clsControlInfo4);
									arrayList3.Add(j);
									arrayList2.Add(this.wgudpOperateControllerCardLost.getXidOfCommand(array4));
								}
							}
							dateTime = DateTime.Now.AddSeconds(1.0);
							wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num5);
						}
						while (!this.bComplete4Cardlost);
					}
					wgTools.WriteLine(string.Format("rcvpktCount =" + num4.ToString(), new object[0]));
					wgTools.WriteLine(string.Format("arrDealtItemOK.Count =" + arrayList.Count.ToString(), new object[0]));
					this.wgudpOperateControllerCardLost.Close();
					this.wgudpOperateControllerCardLost.Dispose();
					wgTools.WgDebugWrite(string.Format("Privilege Send {0}, Receive {1}", this.arrDoorsInfo.Count, arrayList.Count), new object[0]);
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				finally
				{
					this.bComplete4Cardlost = true;
				}
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000C6C24 File Offset: 0x000C5C24
		private void OperateControllerConsoleThread()
		{
			try
			{
				this.arrDealtSN = null;
				this.arrDealtSN = new ArrayList();
				this.waitDoors.Clear();
				this.arrDoorsInfo.Clear();
				DataView dataView = new DataView(this.dtDoors);
				for (int i = 0; i < this.arrSelectedDoors.Count; i++)
				{
					dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
					if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
					{
						this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((int)dataView[0]["f_ControllerID"], (string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"]);
						this.arrDoorsInfo.Add(clsDoorsInfo);
					}
				}
				if (!this.bStopComm())
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < this.arrDoorsInfo.Count; i++)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
						arrayList.Add(clsDoorsInfo2.ControllerSN);
						if (this.bStopComm())
						{
							return;
						}
						switch (this.op)
						{
						case dfrmMultiThreadOperation.Operation.GetSwipes:
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetSwipeThreadMainWithParameters), clsDoorsInfo2);
							break;
						case dfrmMultiThreadOperation.Operation.Download:
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.DownloadThreadMainWithParameters), clsDoorsInfo2);
							break;
						}
					}
					dataView.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000C6E44 File Offset: 0x000C5E44
		public bool pingControllerIP(string ip)
		{
			if (this.IsIPCanConnect(ip))
			{
				return true;
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.pingControllerIPThread), ip);
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.pingControllerIPARPThread), ip);
			return false;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000C6E78 File Offset: 0x000C5E78
		public void pingControllerIPARPThread(object ip)
		{
			byte[] array = new byte[6];
			uint num = (uint)array.Length;
			try
			{
				if (wgGlobal.SafeNativeMethods.SendARP(wgTools.IPLng(IPAddress.Parse(ip as string)), 0, array, ref num) == 0)
				{
					if (this.arrIPARPOK.IndexOf(ip as string) >= 0)
					{
						return;
					}
					lock (this.arrIPARPOK)
					{
						this.arrIPARPOK.Add(ip as string);
						return;
					}
				}
				this.arrIPARPFailed.Add(ip as string);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000C6F30 File Offset: 0x000C5F30
		public void pingControllerIPThread(object ip)
		{
			bool flag = false;
			try
			{
				Ping ping = new Ping();
				PingOptions pingOptions = new PingOptions();
				pingOptions.DontFragment = true;
				string text = "12345678901234567890123456789012";
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				int num = 300;
				int num2 = 0;
				while (num2++ < 3)
				{
					if (ping.Send(ip as string, num, bytes, pingOptions).Status == IPStatus.Success)
					{
						flag = true;
						if (this.arrIPPingOK.IndexOf(ip as string) >= 0)
						{
							break;
						}
						lock (this.arrIPPingOK)
						{
							this.arrIPPingOK.Add(ip as string);
							break;
						}
					}
					num = 1000;
				}
				if (!flag)
				{
					this.arrIPPingFailed.Add(ip as string);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000C7024 File Offset: 0x000C6024
		public static void raiseAppRunInfoCommStatus(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc)
		{
			if (dfrmMultiThreadOperation.appRunInfoCommStatus != null)
			{
				dfrmMultiThreadOperation.appRunInfoCommStatus(doorName, info, proc);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000C703C File Offset: 0x000C603C
		private void RemoteOpenConsoleThread()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.RemoteOpenConsoleThread_P64();
				return;
			}
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			this.arrDealtSN = null;
			this.arrDealtDoors = null;
			this.arrDealtSN = new ArrayList();
			this.arrDealtDoors = new ArrayList();
			this.waitDoors.Clear();
			this.arrDoorsInfo.Clear();
			new ArrayList();
			new ArrayList();
			DataView dataView = new DataView(this.dtDoors);
			for (int i = 0; i < this.arrSelectedDoors.Count; i++)
			{
				this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
				dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
				if (dataView.Count > 0)
				{
					this.arrDoorsInfo.Add(new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"], (int)((byte)dataView[0]["f_DoorNO"])));
					if (string.IsNullOrEmpty(dataView[0]["f_IP"] as string))
					{
						flag = true;
					}
					if (this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
					{
						this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
					}
				}
			}
			wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
			wgTools.WriteLine("发送开门指令");
			new ArrayList();
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(1);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = 1U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = 0L;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			byte[] array = null;
			DateTime dateTime = DateTime.Now;
			int num = 0;
			int num2 = 3;
			DateTime dateTime2 = DateTime.Now;
			ArrayList arrayList4 = new ArrayList();
			if (flag)
			{
				dateTime = DateTime.Now;
				for (int j = 1; j <= 4; j++)
				{
					bool flag2 = false;
					for (int i = 0; i < this.arrDoorsInfo.Count; i++)
					{
						if (this.bStopComm())
						{
							return;
						}
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (string.IsNullOrEmpty(clsDoorsInfo.IP) && clsDoorsInfo.DoorNO == j)
						{
							flag2 = true;
							wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
							wgpacketBasicRemoteOpenDoorToSend.specialDoorNo4ping(clsDoorsInfo.DoorNO);
							wgpacketBasicRemoteOpenDoorToSend.GetNewXid();
							byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(wgUdpComm.udpPort);
							this.wait4Accelerate();
							wgUdpComm.udp_get_onlySend(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
							arrayList3.Add(i);
							arrayList2.Add(wgUdpComm.getXidOfCommand(array2));
						}
					}
					if (flag2)
					{
						Thread.Sleep(100);
					}
				}
				Thread.Sleep(10);
				if (wgTools.bUDPCloud > 0)
				{
					Thread.Sleep(300);
				}
				for (;;)
				{
					array = wgUdpComm.GetPacket();
					if (array == null && wgTools.bUDPCloud > 0)
					{
						array = wgTools.wgcloud.GetPacket4get();
					}
					if (array == null)
					{
						goto IL_0473;
					}
					num++;
					int num3 = arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array));
					if (num3 >= 0)
					{
						int num4 = (int)arrayList3[num3];
						if (arrayList.IndexOf(num4) < 0)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[num4] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
								arrayList.Add(num4);
								arrayList4.Add(clsDoorsInfo);
								dateTime2 = DateTime.Now;
								if (arrayList.Count == this.arrDoorsInfo.Count)
								{
									break;
								}
							}
						}
						else
						{
							wgTools.WriteLine("arrDealtItemOK.IndexOf(doorIndex) >= 0, doorIndex =  " + num4);
						}
					}
				}
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				IL_0473:
				if (arrayList4.Count > 0)
				{
					for (int k = 0; k < arrayList4.Count; k++)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = arrayList4[k] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
						{
							this.addEventToLog1(new InfoRow
							{
								desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
								information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
							});
							this.updateDoorProcess(clsDoorsInfo.DoorName, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
						}
					}
					arrayList4.Clear();
					Application.DoEvents();
				}
			}
			dateTime = DateTime.Now;
			for (int l = 1; l <= 4; l++)
			{
				bool flag3 = false;
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					if (this.bStopComm())
					{
						return;
					}
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (!string.IsNullOrEmpty(clsDoorsInfo.IP) && clsDoorsInfo.DoorNO == l)
					{
						flag3 = true;
						wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
						wgpacketBasicRemoteOpenDoorToSend.specialDoorNo4ping(clsDoorsInfo.DoorNO);
						wgpacketBasicRemoteOpenDoorToSend.GetNewXid();
						byte[] array3 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(wgUdpComm.udpPort);
						this.wait4Accelerate();
						wgUdpComm.udp_get_onlySend(array3, 300, wgpacketBasicRemoteOpenDoorToSend.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
						arrayList3.Add(i);
						arrayList2.Add(wgUdpComm.getXidOfCommand(array3));
					}
				}
				if (flag3)
				{
					Thread.Sleep(100);
				}
			}
			wgTools.WriteLine("发送开门指令完成");
			DateTime dateTime3 = DateTime.Now.AddSeconds(2.0);
			while (arrayList.Count != this.arrDoorsInfo.Count)
			{
				do
				{
					array = wgUdpComm.GetPacket();
					if (array != null)
					{
						num++;
						int num3 = arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array));
						if (num3 >= 0)
						{
							int num4 = (int)arrayList3[num3];
							if (arrayList.IndexOf(num4) < 0)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[num4] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
								{
									clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
									arrayList.Add(num4);
									arrayList4.Add(clsDoorsInfo);
									dateTime2 = DateTime.Now;
									if (arrayList.Count == this.arrDoorsInfo.Count)
									{
										goto Block_35;
									}
								}
							}
							else
							{
								wgTools.WriteLine("arrDealtItemOK.IndexOf(doorIndex) >= 0, doorIndex =  " + num4);
							}
						}
					}
					else if (arrayList4.Count > 0)
					{
						for (int m = 0; m < arrayList4.Count; m++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = arrayList4[m] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								this.addEventToLog1(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
									information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
								});
								this.updateDoorProcess(clsDoorsInfo.DoorName, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
							}
						}
						arrayList4.Clear();
						Application.DoEvents();
					}
					else
					{
						Thread.Sleep(10);
					}
				}
				while (dateTime3 >= DateTime.Now);
				IL_0844:
				wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
				if (!this.bComplete4Cardlost)
				{
					num2--;
					if (num2 > 0)
					{
						dateTime = DateTime.Now;
						for (int n = 1; n <= 4; n++)
						{
							bool flag4 = true;
							for (int i = 0; i < this.arrDoorsInfo.Count; i++)
							{
								if (this.bStopComm())
								{
									return;
								}
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo.DoorNO == n)
								{
									flag4 = true;
									if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
									{
										wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)clsDoorsInfo.ControllerSN;
										wgpacketBasicRemoteOpenDoorToSend.specialDoorNo4ping(clsDoorsInfo.DoorNO);
										wgpacketBasicRemoteOpenDoorToSend.GetNewXid();
										byte[] array4 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(wgUdpComm.udpPort);
										this.wait4Accelerate();
										wgUdpComm.udp_get_onlySend(array4, 300, wgpacketBasicRemoteOpenDoorToSend.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
										arrayList3.Add(i);
										arrayList2.Add(wgUdpComm.getXidOfCommand(array4));
									}
								}
							}
							if (flag4)
							{
								Thread.Sleep(100);
							}
						}
						dateTime3 = DateTime.Now.AddSeconds(1.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num2);
						if (!this.bComplete4Cardlost)
						{
							continue;
						}
					}
				}
				IL_09AF:
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				for (int num5 = 0; num5 < arrayList4.Count; num5++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = arrayList4[num5] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
							information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
						});
						this.updateDoorProcess(clsDoorsInfo.DoorName, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList4.Clear();
				for (int num6 = 0; num6 < this.arrDoorsInfo.Count; num6++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[num6] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				return;
				Block_35:
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				goto IL_0844;
			}
			this.bComplete4Cardlost = true;
			this.bCompleteFullOK4Cardlost = true;
			goto IL_09AF;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000C7B5C File Offset: 0x000C6B5C
		private void RemoteOpenConsoleThread_P64()
		{
			ArrayList arrayList = new ArrayList();
			this.arrDealtSN = null;
			this.arrDealtDoors = null;
			this.arrDealtSN = new ArrayList();
			this.arrDealtDoors = new ArrayList();
			this.waitDoors.Clear();
			this.arrDoorsInfo.Clear();
			new ArrayList();
			new ArrayList();
			DataView dataView = new DataView(this.dtDoors);
			for (int i = 0; i < this.arrSelectedDoors.Count; i++)
			{
				this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
				dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
				if (dataView.Count > 0)
				{
					this.arrDoorsInfo.Add(new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"], (int)((byte)dataView[0]["f_DoorNO"])));
					string.IsNullOrEmpty(dataView[0]["f_IP"] as string);
					if (this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
					{
						this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
					}
				}
			}
			wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
			wgTools.WriteLine("发送开门指令");
			new ArrayList();
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 64;
			wgpacketShort.data[20] = 1;
			long num = 1L;
			Array.Copy(BitConverter.GetBytes(num), 0, wgpacketShort.data, 12, 4);
			Array.Copy(BitConverter.GetBytes(num), 4, wgpacketShort.data, 16, 4);
			wgpacketShort.data[24] = 90;
			wgUdpComm wgUdpComm = new wgUdpComm();
			Thread.Sleep(300);
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			byte[] array = null;
			DateTime dateTime = DateTime.Now;
			int num2 = 0;
			int num3 = 3;
			DateTime dateTime2 = DateTime.Now;
			ArrayList arrayList4 = new ArrayList();
			int bUDPOnly = wgTools.bUDPOnly64;
			dateTime = DateTime.Now;
			for (int j = 1; j <= 4; j++)
			{
				bool flag = false;
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					if (this.bStopComm())
					{
						return;
					}
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.DoorNO == j)
					{
						flag = true;
						wgpacketShort.iDevSn = (uint)clsDoorsInfo.ControllerSN;
						wgpacketShort.data[0] = (byte)clsDoorsInfo.DoorNO;
						byte[] array2 = wgpacketShort.ToBytes();
						this.wait4Accelerate();
						wgUdpComm.udp_get_onlySend(array2, 300, wgpacketShort.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
						arrayList3.Add(i);
						arrayList2.Add(wgUdpComm.getXidOfCommand(array2));
					}
				}
				wgTools.delay4SendNextCommand_P64();
				if (flag)
				{
					Thread.Sleep(100);
				}
			}
			wgTools.WriteLine("发送开门指令完成");
			DateTime dateTime3 = DateTime.Now.AddSeconds(6.0);
			wgAppConfig.wgLogWithoutDB("p64-AAA: send data finish");
			while (arrayList.Count != this.arrDoorsInfo.Count)
			{
				do
				{
					array = wgUdpServer.getRemoteOpenPacket();
					if (array != null)
					{
						wgAppConfig.wgLogWithoutDB("p64-AAA: " + BitConverter.ToString(array));
						num2++;
						int num4 = arrayList2.IndexOf(wgUdpComm.getXidOfCommand(array));
						if (num4 >= 0)
						{
							int num5 = (int)arrayList3[num4];
							if (arrayList.IndexOf(num5) < 0)
							{
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[num5] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
								{
									clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
									arrayList.Add(num5);
									arrayList4.Add(clsDoorsInfo);
									dateTime2 = DateTime.Now;
									if (arrayList.Count == this.arrDoorsInfo.Count)
									{
										goto Block_14;
									}
								}
							}
							else
							{
								wgTools.WriteLine("arrDealtItemOK.IndexOf(doorIndex) >= 0, doorIndex =  " + num5);
							}
						}
					}
					else if (arrayList4.Count > 0)
					{
						for (int k = 0; k < arrayList4.Count; k++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = arrayList4[k] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
							{
								this.addEventToLog1(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
									information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
								});
								this.updateDoorProcess(clsDoorsInfo.DoorName, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
							}
						}
						arrayList4.Clear();
						Application.DoEvents();
					}
					else
					{
						Thread.Sleep(100);
					}
				}
				while (dateTime3 >= DateTime.Now);
				IL_0577:
				wgTools.WriteLine("arrDealtItemOK.Count = " + arrayList.Count.ToString());
				if (!this.bComplete4Cardlost)
				{
					num3--;
					if (num3 > 0)
					{
						dateTime = DateTime.Now;
						for (int l = 1; l <= 4; l++)
						{
							bool flag2 = true;
							for (int i = 0; i < this.arrDoorsInfo.Count; i++)
							{
								if (this.bStopComm())
								{
									return;
								}
								dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
								if (clsDoorsInfo.DoorNO == l)
								{
									flag2 = true;
									if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
									{
										wgpacketShort.iDevSn = (uint)clsDoorsInfo.ControllerSN;
										wgpacketShort.data[0] = (byte)clsDoorsInfo.DoorNO;
										byte[] array3 = wgpacketShort.ToBytes();
										this.wait4Accelerate();
										wgUdpComm.udp_get_onlySend(array3, 300, wgpacketShort.xid, clsDoorsInfo.IP, clsDoorsInfo.Port, ref array);
										arrayList3.Add(i);
										arrayList2.Add(wgUdpComm.getXidOfCommand(array3));
									}
								}
							}
							if (flag2)
							{
								Thread.Sleep(100);
							}
						}
						dateTime3 = DateTime.Now.AddSeconds(15.0);
						wgTools.WriteLine("OperateControllerCardLostConsoleThread First Send Tries =" + num3);
						if (!this.bComplete4Cardlost)
						{
							continue;
						}
					}
				}
				IL_06D7:
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				for (int m = 0; m < arrayList4.Count; m++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = arrayList4[m] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
							information = string.Format("{0}", wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK))
						});
						this.updateDoorProcess(clsDoorsInfo.DoorName, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strRemoteOpenDoorOK), dfrmMultiThreadOperation.RunStatus.CompleteOK, dateTime.ToString("HH:mm:ss"), dateTime2.ToString("HH:mm:ss"));
					}
				}
				arrayList4.Clear();
				for (int n = 0; n < this.arrDoorsInfo.Count; n++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[n] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo.DoorName, clsDoorsInfo.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				return;
				Block_14:
				this.bComplete4Cardlost = true;
				this.bCompleteFullOK4Cardlost = true;
				goto IL_0577;
			}
			this.bComplete4Cardlost = true;
			this.bCompleteFullOK4Cardlost = true;
			goto IL_06D7;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000C83A4 File Offset: 0x000C73A4
		private void SendData(object info)
		{
			dfrmMultiThreadOperation.clsControlInfo clsControlInfo = info as dfrmMultiThreadOperation.clsControlInfo;
			byte[] array = null;
			try
			{
				this.wgudpOperateControllerCardLost.udp_get_onlySend(clsControlInfo.bytToSend, 300, 0U, clsControlInfo.IP, clsControlInfo.Port, ref array);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000C8408 File Offset: 0x000C7408
		private void SetDoorControlConsoleThread()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.SetDoorControlConsoleThread_P64();
				return;
			}
			try
			{
				bool flag = false;
				bool flag2 = false;
				this.arrDealtSN = null;
				this.arrDealtDoors = null;
				this.arrDealtSN = new ArrayList();
				this.arrDealtDoors = new ArrayList();
				this.waitDoors.Clear();
				this.arrDoorsInfo.Clear();
				ArrayList arrayList = new ArrayList();
				new ArrayList();
				new icController();
				DateTime dateTime = DateTime.Now.AddSeconds(3.0);
				wgTools.WriteLine("先获取总的SN数量");
				DataView dataView = new DataView(this.dtDoors);
				for (int i = 0; i < this.arrSelectedDoors.Count; i++)
				{
					this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
					dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
					if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
					{
						this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
						dataView.RowFilter = "f_ControllerSN = " + dataView[0]["f_ControllerSN"];
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"]);
						for (int j = 0; j < dataView.Count; j++)
						{
							if (this.doorControl >= 0)
							{
								clsDoorsInfo.setDoorControl((int)((byte)dataView[j]["f_DoorNO"]), this.doorControl);
							}
							else if (this.doorDelay >= 0)
							{
								clsDoorsInfo.setDoorDelay((int)((byte)dataView[j]["f_DoorNO"]), this.doorDelay);
							}
						}
						this.arrDoorsInfo.Add(clsDoorsInfo);
						if (string.IsNullOrEmpty(dataView[0]["f_IP"] as string))
						{
							flag = true;
						}
						else
						{
							flag2 = true;
							this.pingControllerIP(dataView[0]["f_IP"] as string);
						}
					}
				}
				wgAppConfig.wgLog(this.Text + "..." + this.arrSelectedDoors.Count);
				wgTools.WriteLine("发送设置门控制方式指令");
				ArrayList arrayList2 = new ArrayList();
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
				wgpacketWith1152_internal.type = 36;
				wgpacketWith1152_internal.code = 32;
				wgpacketWith1152_internal.iDevSnFrom = 0U;
				wgpacketWith1152_internal.iCallReturn = 0;
				wgUdpComm wgUdpComm = new wgUdpComm();
				Thread.Sleep(300);
				ArrayList arrayList3 = new ArrayList();
				ArrayList arrayList4 = new ArrayList();
				byte[] array = null;
				DateTime now = DateTime.Now;
				new ArrayList();
				if (flag)
				{
					wgTools.WriteLine("bExistSmallNet");
					for (int i = 0; i < this.arrDoorsInfo.Count; i++)
					{
						if (this.bStopComm())
						{
							return;
						}
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
						if (string.IsNullOrEmpty(clsDoorsInfo2.IP))
						{
							wgMjControllerConfigure.Clear();
							for (int k = 1; k <= wgMjController.GetControllerType(clsDoorsInfo2.ControllerSN); k++)
							{
								if (clsDoorsInfo2.getDoorNeedUpdate(k) > 0)
								{
									if (this.doorControl >= 0)
									{
										wgMjControllerConfigure.DoorControlSet(k, clsDoorsInfo2.getDoorControl(k));
									}
									else if (this.doorDelay >= 0)
									{
										wgMjControllerConfigure.DoorDelaySet(k, clsDoorsInfo2.getDoorDelay(k));
									}
								}
							}
							wgpacketWith1152_internal.iDevSnTo = (uint)clsDoorsInfo2.ControllerSN;
							wgMjControllerConfigure.paramData.CopyTo(wgpacketWith1152_internal.ucData, 0);
							wgMjControllerConfigure.needUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
							wgpacketWith1152_internal.GetNewXid();
							byte[] array2 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
							this.wait4Accelerate();
							wgUdpComm.udp_get_onlySend(array2, 300, wgpacketWith1152_internal.xid, clsDoorsInfo2.IP, clsDoorsInfo2.Port, ref array);
							arrayList4.Add(i);
							arrayList3.Add(wgUdpComm.getXidOfCommand(array2));
							wgTools.WriteLine("dealtDoorIndex =" + i);
						}
					}
				}
				if (flag2)
				{
					wgTools.WriteLine("bExistIPNet");
					Thread.Sleep(100);
					while (DateTime.Now < dateTime && this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
					{
						Thread.Sleep(100);
					}
					wgTools.WriteLine("(DateTime.Now < dtPingEnd)");
					if (this.arrIPARPOK.Count != 0 || this.arrIPPingOK.Count != 0)
					{
						for (int i = 0; i < this.arrDoorsInfo.Count; i++)
						{
							if (this.bStopComm())
							{
								return;
							}
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
							if (!string.IsNullOrEmpty(clsDoorsInfo3.IP) && this.IsIPCanConnect(clsDoorsInfo3.IP))
							{
								wgMjControllerConfigure.Clear();
								for (int l = 1; l <= wgMjController.GetControllerType(clsDoorsInfo3.ControllerSN); l++)
								{
									if (clsDoorsInfo3.getDoorNeedUpdate(l) > 0)
									{
										if (this.doorControl >= 0)
										{
											wgMjControllerConfigure.DoorControlSet(l, clsDoorsInfo3.getDoorControl(l));
										}
										else if (this.doorDelay >= 0)
										{
											wgMjControllerConfigure.DoorDelaySet(l, clsDoorsInfo3.getDoorDelay(l));
										}
									}
								}
								wgpacketWith1152_internal.iDevSnTo = (uint)clsDoorsInfo3.ControllerSN;
								wgMjControllerConfigure.paramData.CopyTo(wgpacketWith1152_internal.ucData, 0);
								wgMjControllerConfigure.needUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
								wgpacketWith1152_internal.GetNewXid();
								byte[] array3 = wgpacketWith1152_internal.ToBytes(wgUdpComm.udpPort);
								this.wait4Accelerate();
								wgUdpComm.udp_get_onlySend(array3, 300, wgpacketWith1152_internal.xid, clsDoorsInfo3.IP, clsDoorsInfo3.Port, ref array);
								wgTools.WriteLine("wgudp.udp_get_onlySend" + clsDoorsInfo3.IP);
								arrayList4.Add(i);
								arrayList3.Add(wgUdpComm.getXidOfCommand(array3));
							}
						}
					}
				}
				wgTools.WriteLine("发送设置控制方式指令完成");
				int num = 0;
				DateTime dateTime2 = DateTime.Now.AddSeconds(3.0);
				DateTime dateTime3 = DateTime.Now;
				do
				{
					array = wgUdpComm.GetPacket();
					if (array == null && wgTools.bUDPCloud > 0)
					{
						array = wgTools.wgcloud.GetPacket4get();
					}
					if (array != null)
					{
						num++;
						if (arrayList3.IndexOf(wgUdpComm.getXidOfCommand(array)) >= 0 && arrayList2.IndexOf(arrayList4[arrayList3.IndexOf(wgUdpComm.getXidOfCommand(array))]) < 0)
						{
							(this.arrDoorsInfo[(int)arrayList4[arrayList3.IndexOf(wgUdpComm.getXidOfCommand(array))]] as dfrmMultiThreadOperation.clsDoorsInfo).commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							arrayList2.Add(arrayList4[arrayList3.IndexOf(wgUdpComm.getXidOfCommand(array))]);
							dateTime3 = DateTime.Now;
							if (arrayList2.Count == this.arrDoorsInfo.Count)
							{
								break;
							}
						}
					}
					else
					{
						Thread.Sleep(10);
					}
				}
				while (dateTime2 >= DateTime.Now);
				wgTools.WriteLine(string.Format("rcvpktCount =" + num.ToString(), new object[0]));
				wgTools.WriteLine(string.Format("arrDoorOpened.Count =" + arrayList2.Count.ToString(), new object[0]));
				wgUdpComm.Close();
				wgUdpComm.Dispose();
				int num2 = 0;
				for (int m = 0; m < this.arrDoorsInfo.Count; m++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo4 = this.arrDoorsInfo[m] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo4.commStatus == dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						InfoRow infoRow = new InfoRow();
						infoRow.desc = string.Format("{0}[{1:d}]", clsDoorsInfo4.DoorName, clsDoorsInfo4.ControllerSN);
						if (this.doorControl >= 0)
						{
							infoRow.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl));
							this.updateDoorProcess(clsDoorsInfo4.DoorName, string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl)), dfrmMultiThreadOperation.RunStatus.CompleteOK, now.ToString("HH:mm:ss"), dateTime3.ToString("HH:mm:ss"));
						}
						if (this.doorDelay >= 0)
						{
							infoRow.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString());
							this.updateDoorProcess(clsDoorsInfo4.DoorName, string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString()), dfrmMultiThreadOperation.RunStatus.CompleteOK, now.ToString("HH:mm:ss"), dateTime3.ToString("HH:mm:ss"));
						}
						this.addEventToLog1(infoRow);
					}
					else
					{
						num2++;
					}
				}
				if (num2 != 0 && !this.bStopComm())
				{
					new ArrayList();
					new ArrayList();
					ArrayList arrayList5 = new ArrayList();
					for (int i = 0; i < this.arrDoorsInfo.Count; i++)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo5 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
						arrayList5.Add(clsDoorsInfo5.ControllerSN);
						if (clsDoorsInfo5.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
						{
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.SetDoorControlThreadMainWithParameters), clsDoorsInfo5);
							this.updateStartTime(clsDoorsInfo5.DoorName, DateTime.Now.ToString("HH:mm:ss"));
						}
					}
					if (arrayList.Count > 0)
					{
						for (int i = 0; i < arrayList.Count; i++)
						{
							dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo6 = arrayList[i] as dfrmMultiThreadOperation.clsDoorsInfo;
							this.addEventToLog1(new InfoRow
							{
								desc = string.Format("{0}[{1:d}]", clsDoorsInfo6.DoorName, clsDoorsInfo6.ControllerSN),
								information = string.Format("{0}", CommonStr.strCommFail)
							});
							this.updateDoorProcess(clsDoorsInfo6.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
						}
					}
					dataView.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000C8EC8 File Offset: 0x000C7EC8
		private void SetDoorControlConsoleThread_P64()
		{
			try
			{
				this.arrDealtSN = null;
				this.arrDealtDoors = null;
				this.arrDealtSN = new ArrayList();
				this.arrDealtDoors = new ArrayList();
				this.waitDoors.Clear();
				this.arrDoorsInfo.Clear();
				ArrayList arrayList = new ArrayList();
				new ArrayList();
				new icController();
				DateTime.Now.AddSeconds(3.0);
				wgTools.WriteLine("先获取总的SN数量");
				DataView dataView = new DataView(this.dtDoors);
				for (int i = 0; i < this.arrSelectedDoors.Count; i++)
				{
					this.arrDealtDoors.Add(this.arrSelectedDoors[i]);
					dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(this.arrSelectedDoors[i]);
					if (dataView.Count > 0 && this.arrDealtSN.IndexOf(dataView[0]["f_ControllerSN"]) < 0)
					{
						this.arrDealtSN.Add(dataView[0]["f_ControllerSN"]);
						dataView.RowFilter = "f_ControllerSN = " + dataView[0]["f_ControllerSN"];
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = new dfrmMultiThreadOperation.clsDoorsInfo((string)dataView[0]["f_DoorName"], (int)dataView[0]["f_ControllerSN"], dataView[0]["f_IP"] as string, (int)dataView[0]["f_PORT"]);
						for (int j = 0; j < dataView.Count; j++)
						{
							if (this.doorControl >= 0)
							{
								clsDoorsInfo.setDoorControl((int)((byte)dataView[j]["f_DoorNO"]), this.doorControl);
							}
							else if (this.doorDelay >= 0)
							{
								clsDoorsInfo.setDoorDelay((int)((byte)dataView[j]["f_DoorNO"]), this.doorDelay);
							}
						}
						this.arrDoorsInfo.Add(clsDoorsInfo);
					}
				}
				new ArrayList();
				new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo2 = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					arrayList2.Add(clsDoorsInfo2.ControllerSN);
					if (clsDoorsInfo2.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.SetDoorControlThreadMainWithParameters), clsDoorsInfo2);
						this.updateStartTime(clsDoorsInfo2.DoorName, DateTime.Now.ToString("HH:mm:ss"));
					}
				}
				if (arrayList.Count > 0)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo3 = arrayList[i] as dfrmMultiThreadOperation.clsDoorsInfo;
						this.addEventToLog1(new InfoRow
						{
							desc = string.Format("{0}[{1:d}]", clsDoorsInfo3.DoorName, clsDoorsInfo3.ControllerSN),
							information = string.Format("{0}", CommonStr.strCommFail)
						});
						this.updateDoorProcess(clsDoorsInfo3.DoorName, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
					}
				}
				dataView.Dispose();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000C9234 File Offset: 0x000C8234
		private void SetDoorControlThreadMainWithParameters(object obj)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.SetDoorControlThreadMainWithParameters_P64(obj);
				return;
			}
			try
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
				string text = clsDoorsInfo.DoorName;
				using (icController icController = new icController())
				{
					do
					{
						text = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						this.updateStartTime(text, DateTime.Now.ToString("HH:mm:ss"));
						int num = -1;
						int num2 = 3;
						bool flag = false;
						while (num2-- > 0)
						{
							if (this.arrIPARPOK.Count == 0 && this.arrIPPingOK.Count == 0)
							{
								Thread.Sleep(100);
							}
							else if (this.IsIPCanConnect(clsDoorsInfo.IP))
							{
								flag = true;
								break;
							}
						}
						if (!this.bStopComm() && flag)
						{
							wgMjControllerConfigure.Clear();
							for (int i = 1; i <= wgMjController.GetControllerType(clsDoorsInfo.ControllerSN); i++)
							{
								if (clsDoorsInfo.getDoorNeedUpdate(i) > 0)
								{
									if (this.doorControl >= 0)
									{
										wgMjControllerConfigure.DoorControlSet(i, clsDoorsInfo.getDoorControl(i));
									}
									else if (this.doorDelay >= 0)
									{
										wgMjControllerConfigure.DoorDelaySet(i, clsDoorsInfo.getDoorDelay(i));
									}
								}
							}
							num = icController.UpdateConfigureIP(wgMjControllerConfigure, -1);
						}
						if (num >= 0)
						{
							if (this.doorControl >= 0)
							{
								InfoRow infoRow = new InfoRow();
								infoRow.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
								infoRow.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl));
								this.updateDoorProcess(text, string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl)), dfrmMultiThreadOperation.RunStatus.CompleteOK);
								this.addEventToLog1(infoRow);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							}
							if (this.doorDelay >= 0)
							{
								InfoRow infoRow2 = new InfoRow();
								infoRow2.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
								infoRow2.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString());
								this.updateDoorProcess(text, string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString()), dfrmMultiThreadOperation.RunStatus.CompleteOK);
								this.addEventToLog1(infoRow2);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							}
						}
						else
						{
							InfoRow infoRow3 = new InfoRow();
							infoRow3.category = 101;
							infoRow3.desc = text;
							infoRow3.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
							{
								CommonStr.strCommFail + "  [" + num.ToString() + "]",
								CommonStr.strControllerSN,
								icController.ControllerSN,
								icController.IP
							});
							infoRow3.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
							{
								text,
								CommonStr.strCommFail + "  [" + num.ToString() + "]",
								CommonStr.strControllerSN,
								icController.ControllerSN,
								icController.IP
							});
							this.updateDoorProcess(text, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
							this.addEventToLog1(infoRow3);
							clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						}
						this.updateEndTime(text, DateTime.Now.ToString("HH:mm:ss"));
						clsDoorsInfo = this.getWaitDoorInfo();
					}
					while (clsDoorsInfo != null && !this.bStopComm());
					icController.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000C95F8 File Offset: 0x000C85F8
		private void SetDoorControlThreadMainWithParameters_P64(object obj)
		{
			try
			{
				wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
				dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = obj as dfrmMultiThreadOperation.clsDoorsInfo;
				string text = clsDoorsInfo.DoorName;
				using (icController icController = new icController())
				{
					do
					{
						text = clsDoorsInfo.DoorName;
						icController.ControllerSN = clsDoorsInfo.ControllerSN;
						icController.IP = clsDoorsInfo.IP;
						icController.PORT = clsDoorsInfo.Port;
						this.updateStartTime(text, DateTime.Now.ToString("HH:mm:ss"));
						wgMjControllerConfigure.Clear();
						for (int i = 1; i <= wgMjController.GetControllerType(clsDoorsInfo.ControllerSN); i++)
						{
							if (clsDoorsInfo.getDoorNeedUpdate(i) > 0)
							{
								if (this.doorControl >= 0)
								{
									wgMjControllerConfigure.DoorControlSet(i, clsDoorsInfo.getDoorControl(i));
								}
								else if (this.doorDelay >= 0)
								{
									wgMjControllerConfigure.DoorDelaySet(i, clsDoorsInfo.getDoorDelay(i));
								}
							}
						}
						int num = icController.UpdateConfigureIP(wgMjControllerConfigure, -1);
						if (num >= 0)
						{
							if (this.doorControl >= 0)
							{
								InfoRow infoRow = new InfoRow();
								infoRow.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
								infoRow.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl));
								this.updateDoorProcess(text, string.Format("{0}{1}", CommonStr.strDirectSetDoorControl, icDesc.doorControlDesc(this.doorControl)), dfrmMultiThreadOperation.RunStatus.CompleteOK);
								this.addEventToLog1(infoRow);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							}
							if (this.doorDelay >= 0)
							{
								InfoRow infoRow2 = new InfoRow();
								infoRow2.desc = string.Format("{0}[{1:d}]", text, icController.ControllerSN);
								infoRow2.information = string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString());
								this.updateDoorProcess(text, string.Format("{0}{1}", CommonStr.strDirectSetDoorOpenDelay, this.doorDelay.ToString()), dfrmMultiThreadOperation.RunStatus.CompleteOK);
								this.addEventToLog1(infoRow2);
								clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CompleteOK;
							}
						}
						else
						{
							InfoRow infoRow3 = new InfoRow();
							infoRow3.category = 101;
							infoRow3.desc = text;
							infoRow3.information = string.Format("{0}--{1}:{2:d}--IP:{3}", new object[]
							{
								CommonStr.strCommFail + "  [" + num.ToString() + "]",
								CommonStr.strControllerSN,
								icController.ControllerSN,
								icController.IP
							});
							infoRow3.detail = string.Format("{0}\r\n{1}\r\n{2}:\t{3:d}\r\nIP:\t{4}\r\n", new object[]
							{
								text,
								CommonStr.strCommFail + "  [" + num.ToString() + "]",
								CommonStr.strControllerSN,
								icController.ControllerSN,
								icController.IP
							});
							this.updateDoorProcess(text, CommonStr.strCommFail, dfrmMultiThreadOperation.RunStatus.CommFail);
							this.addEventToLog1(infoRow3);
							clsDoorsInfo.commStatus = dfrmMultiThreadOperation.RunStatus.CommFail;
						}
						this.updateEndTime(text, DateTime.Now.ToString("HH:mm:ss"));
						clsDoorsInfo = this.getWaitDoorInfo();
					}
					while (clsDoorsInfo != null && !this.bStopComm());
					icController.Dispose();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000C9958 File Offset: 0x000C8958
		public void startAdjustTime()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.Text = string.Format("{0}-{1}", this.Text, CommonStr.strFunctionDisplayName_mnuAdjustTime);
			this.Refresh();
			if (XMessageBox.Show(CommonStr.strAdjustTime + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.AdjustTime;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			new Thread(new ThreadStart(this.AdjustTimeConsoleThread)).Start();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000C9A4C File Offset: 0x000C8A4C
		public void startDownload()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.Download;
			this.Text = string.Format("{0}-{1}", this.Text, CommonStr.strUpload);
			this.Refresh();
			this.downloadPrivilegeAndConfigure();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000C9AF4 File Offset: 0x000C8AF4
		public void startDownloadAddDelPri(bool bAdd)
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			if (bAdd)
			{
				this.op = dfrmMultiThreadOperation.Operation.AddPri;
			}
			else
			{
				this.op = dfrmMultiThreadOperation.Operation.DelPri;
			}
			new Thread(new ThreadStart(this.OperateControllerAddDelPriConsoleThread)).Start();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000C9B98 File Offset: 0x000C8B98
		public void startDownloadCardLostUser()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.CardLost;
			new Thread(new ThreadStart(this.OperateControllerCardLostConsoleThread)).Start();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000C9C30 File Offset: 0x000C8C30
		public void startFingerSingleUpdate()
		{
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.FingerSingleUpdate;
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj2 in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj2);
			}
			this.loadFingerDeviceInfo();
			this.dvDoors4updateDoorProcess = new DataView(this.dtDoors);
			this.dvDoors4updateDownloadInfo = new DataView(this.dtDoors);
			new Thread(new ThreadStart(this.FingerSingleUpdateThread)).Start();
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000C9D44 File Offset: 0x000C8D44
		public void startRemoteOpen()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.Text = string.Format("{0}-{1}", this.Text, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strFunctionDisplayName_TotalControl_RemoteOpen));
			this.Refresh();
			if (XMessageBox.Show(wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strFunctionDisplayName_TotalControl_RemoteOpen) + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.RemoteOpen;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			new Thread(new ThreadStart(this.RemoteOpenConsoleThread)).Start();
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000C9E44 File Offset: 0x000C8E44
		public void startSetDoorControl()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.doorControl = -1;
			this.doorDelay = -1;
			using (dfrmControllerDoorControlSet dfrmControllerDoorControlSet = new dfrmControllerDoorControlSet())
			{
				this.Text = string.Format("{0}-{1}", this.Text, dfrmControllerDoorControlSet.Text);
				this.Refresh();
				if (this.arrSelectedDoorsOnConsole.Count == 1)
				{
					dfrmControllerDoorControlSet.Text = dfrmControllerDoorControlSet.Text + "--" + this.arrSelectedDoorsOnConsole[0];
				}
				else
				{
					dfrmControllerDoorControlSet.Text = string.Concat(new string[]
					{
						dfrmControllerDoorControlSet.Text,
						"--",
						CommonStr.strDoorsNum,
						" = ",
						this.arrSelectedDoorsOnConsole.Count.ToString()
					});
				}
				if (dfrmControllerDoorControlSet.ShowDialog(this) == DialogResult.OK)
				{
					this.doorControl = dfrmControllerDoorControlSet.doorControl;
					this.doorDelay = dfrmControllerDoorControlSet.doorOpenDelay;
				}
				if (this.doorControl < 0 && this.doorDelay < 0)
				{
					base.Close();
					return;
				}
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.SetDoorControl;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			this.dataGridView1.DataSource = null;
			string text = "";
			for (int i = 0; i < this.dtDoors.Rows.Count; i++)
			{
				if (this.doorControl >= 0)
				{
					this.dtDoors.Rows[i]["f_DoorControl"] = this.doorControl;
				}
				if (this.doorDelay >= 0)
				{
					this.dtDoors.Rows[i]["f_DoorDelay"] = this.doorDelay;
				}
				if (string.IsNullOrEmpty(text))
				{
					text += this.dtDoors.Rows[i]["f_DoorID"].ToString();
				}
				else
				{
					text = text + "," + this.dtDoors.Rows[i]["f_DoorID"].ToString();
				}
			}
			this.dtDoors.AcceptChanges();
			if (this.doorControl >= 0)
			{
				wgAppConfig.runSql(string.Format("Update t_b_Door SET f_DoorControl = {0} WHERE f_DoorID IN ({1})", this.doorControl, text));
			}
			if (this.doorDelay >= 0)
			{
				wgAppConfig.runSql(string.Format("Update t_b_Door SET f_DoorDelay = {0} WHERE f_DoorID IN ({1})", this.doorDelay, text));
			}
			this.dataGridView1.DataSource = this.dtDoors;
			new Thread(new ThreadStart(this.SetDoorControlConsoleThread)).Start();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000CA160 File Offset: 0x000C9160
		public void startUpload()
		{
			if (!wgAppConfig.IsAcceleratorActive)
			{
				base.Close();
				return;
			}
			this.Text = string.Format("{0}-{1}", this.Text, CommonStr.strGetCardRecords);
			this.Refresh();
			if (XMessageBox.Show(CommonStr.strGetCardRecords + "? ", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				base.Close();
				return;
			}
			this.arrSelectedDoors = null;
			this.arrSelectedDoors = new ArrayList();
			foreach (object obj in this.arrSelectedDoorsOnConsole)
			{
				this.arrSelectedDoors.Add(obj);
			}
			this.op = dfrmMultiThreadOperation.Operation.GetSwipes;
			this.dfrmWait1.Show();
			this.dfrmWait1.Refresh();
			icSwipeRecord.GetSwipeRecords_MaxRecIdOfDB();
			new Thread(new ThreadStart(this.OperateControllerConsoleThread)).Start();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000CA25C File Offset: 0x000C925C
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			this.statTimeDate.Text = DateTime.Now.ToString(wgTools.DisplayFormat_DateYMDHMSWeek);
			if (this.qOperateProcess.Count > 0 || this.qOperateInfo.Count > 0 || this.qEvent.Count > 0 || this.qProductsInfo4DownloadPriviege.Count > 0)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				this.updateBuffInfo();
				try
				{
					this.updateLabelInfo(ref num, ref num2, ref num3);
					if (this.qOperateProcess.Count <= 0 && this.qOperateInfo.Count <= 0 && this.qEvent.Count <= 0 && this.qProductsInfo4DownloadPriviege.Count <= 0)
					{
						this.updateBuffInfo();
						this.updateLabelInfo(ref num, ref num2, ref num3);
						bool flag = false;
						try
						{
							flag = this.dfrmWait1.Visible;
						}
						catch
						{
						}
						if (num > 0 && num == num2 && flag)
						{
							try
							{
								this.dfrmWait1.Location = new Point(0, 0);
								this.dfrmWait1.Hide();
								Application.DoEvents();
							}
							catch
							{
							}
							if (num3 == 0)
							{
								this.btnRetryMultithreadOperation.Visible = false;
							}
							this.lblDealingInfo.Text = "";
							this.toolStripStatusLabel2.Text = "";
							this.writeDisplayedInfo(CommonStr.strOperationComplete);
						}
					}
					while (this.qEvent.Count > 0)
					{
						InfoRow infoRow;
						lock (this.qEvent.SyncRoot)
						{
							infoRow = this.qEvent.Dequeue() as InfoRow;
						}
						if (infoRow != null)
						{
							wgRunInfoLog.addEventToLog1(infoRow, true);
						}
					}
					goto IL_02CF;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					goto IL_02CF;
				}
			}
			if (this.arrDoorsInfo.Count > 0)
			{
				bool flag2 = true;
				bool flag3 = true;
				for (int i = 0; i < this.arrDoorsInfo.Count; i++)
				{
					dfrmMultiThreadOperation.clsDoorsInfo clsDoorsInfo = this.arrDoorsInfo[i] as dfrmMultiThreadOperation.clsDoorsInfo;
					if (clsDoorsInfo.commStatus == dfrmMultiThreadOperation.RunStatus.Start)
					{
						flag2 = false;
						break;
					}
					if (clsDoorsInfo.commStatus != dfrmMultiThreadOperation.RunStatus.CompleteOK)
					{
						flag3 = false;
					}
				}
				if (flag2)
				{
					bool flag4 = false;
					for (int j = 0; j < this.dtDoors.Rows.Count; j++)
					{
						if ((int)this.dtDoors.Rows[j]["f_RunStatus"] == 0)
						{
							if (flag3)
							{
								this.dtDoors.Rows[j]["f_RunStatus"] = 1;
							}
							else
							{
								this.dtDoors.Rows[j]["f_RunStatus"] = -1;
							}
							flag4 = true;
						}
					}
					if (flag4)
					{
						this.writeDisplayedInfo(CommonStr.strOperationComplete);
					}
				}
			}
			IL_02CF:
			if (wgTools.bUDPCloud > 0)
			{
				try
				{
					for (int k = 0; k < this.dtDoors.Rows.Count; k++)
					{
						string text = wgTools.SetObjToStr(this.dtDoors.Rows[k]["f_controllerTime"]);
						int num4 = int.Parse(wgTools.SetObjToStr(this.dtDoors.Rows[k]["f_ControllerSN"]));
						int l = 0;
						while (l < wgTools.arrSNReceived.Count)
						{
							if ((long)num4 == (long)((ulong)((uint)wgTools.arrSNReceived[l])))
							{
								if (string.IsNullOrEmpty(text) || !text.Equals(((DateTime)wgTools.arrRefreshTime[l]).ToString(wgTools.DisplayFormat_DateYMDHMS)))
								{
									this.dtDoors.Rows[k]["f_controllerTime"] = ((DateTime)wgTools.arrRefreshTime[l]).ToString(wgTools.DisplayFormat_DateYMDHMS);
								}
								string text2 = string.Format("{0}:{1}", wgTools.arrSNIP[l], wgTools.arrSNPort[l]);
								if (!text2.Equals(this.dtDoors.Rows[k]["f_cloudipInfo"]))
								{
									this.dtDoors.Rows[k]["f_cloudipInfo"] = text2;
									break;
								}
								break;
							}
							else
							{
								l++;
							}
						}
					}
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLogWithoutDB(ex2.ToString());
				}
			}
			if (wgTools.bUDPOnly64 <= 0)
			{
				int num5 = 0;
				int num6 = 0;
				ThreadPool.GetAvailableThreads(out num5, out num6);
				if (this.usingThreadNum < this.ThreadPoolMaxThreadNum - num5)
				{
					this.usingThreadNum = this.ThreadPoolMaxThreadNum - num5;
					wgAppConfig.wgLog(string.Format("Using Thread Num ={0}", this.usingThreadNum));
				}
			}
			this.timer1.Enabled = true;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000CA7AC File Offset: 0x000C97AC
		private void updateBuffInfo()
		{
			int firstDisplayedScrollingRowIndex = this.dataGridView1.FirstDisplayedScrollingRowIndex;
			this.dataGridView1.DataSource = null;
			string text = "";
			while (this.qOperateProcess.Count > 0)
			{
				dfrmMultiThreadOperation.clsOperateInfo clsOperateInfo;
				lock (this.qOperateProcess.SyncRoot)
				{
					clsOperateInfo = this.qOperateProcess.Dequeue() as dfrmMultiThreadOperation.clsOperateInfo;
				}
				this.updateDoorProcess_ByTimer1(clsOperateInfo.DoorName, clsOperateInfo.Info, clsOperateInfo.Proc, clsOperateInfo.StartTime, clsOperateInfo.EndTime);
				text = string.Format("{0}_{1}", clsOperateInfo.DoorName, clsOperateInfo.Info);
			}
			while (this.qOperateInfo.Count > 0)
			{
				dfrmMultiThreadOperation.clsOperateInfo clsOperateInfo2;
				lock (this.qOperateInfo.SyncRoot)
				{
					clsOperateInfo2 = this.qOperateInfo.Dequeue() as dfrmMultiThreadOperation.clsOperateInfo;
				}
				this.updateOperateInfo_ByTimer1(clsOperateInfo2.DoorName, clsOperateInfo2.FieldName, clsOperateInfo2.Info);
			}
			this.dataGridView1.DataSource = this.dtDoors;
			if (!string.IsNullOrEmpty(text))
			{
				this.lblDealingInfo.Text = text;
				this.toolStripStatusLabel2.Text = text;
			}
			if (firstDisplayedScrollingRowIndex >= 0)
			{
				this.dataGridView1.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
			}
			if (this.qProductsInfo4DownloadPriviege.Count > 0)
			{
				string text2 = "";
				string text3;
				wgAppConfig.getSystemParamValue(48, out text3, out text3, out text2);
				while (this.qProductsInfo4DownloadPriviege.Count > 0)
				{
					lock (this.qProductsInfo4DownloadPriviege.SyncRoot)
					{
						text3 = this.qProductsInfo4DownloadPriviege.Dequeue() as string;
					}
					if (text2.IndexOf(text3) < 0)
					{
						if (text2.IndexOf("SN") < 0)
						{
							text2 += "\r\n";
						}
						text2 += text3;
					}
				}
				wgAppConfig.setSystemParamValue(48, "ConInfo", "", text2);
			}
			while (this.qEvent.Count > 0)
			{
				InfoRow infoRow;
				lock (this.qEvent.SyncRoot)
				{
					infoRow = this.qEvent.Dequeue() as InfoRow;
				}
				if (infoRow != null)
				{
					wgRunInfoLog.addEventToLog1(infoRow, true);
				}
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000CAA14 File Offset: 0x000C9A14
		private void updateDisplayTotal(string doorName, string info)
		{
			this.updateDoorProcess(doorName, info, dfrmMultiThreadOperation.RunStatus.UpdateTotal);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000CAA20 File Offset: 0x000C9A20
		private void updateDoorProcess(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc)
		{
			dfrmMultiThreadOperation.clsOperateInfo clsOperateInfo = new dfrmMultiThreadOperation.clsOperateInfo(doorName, info, proc);
			lock (this.qOperateProcess.SyncRoot)
			{
				this.qOperateProcess.Enqueue(clsOperateInfo);
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000CAA70 File Offset: 0x000C9A70
		private void updateDoorProcess(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc, string startTime, string endTime)
		{
			dfrmMultiThreadOperation.clsOperateInfo clsOperateInfo = new dfrmMultiThreadOperation.clsOperateInfo(doorName, info, proc, startTime, endTime);
			lock (this.qOperateProcess.SyncRoot)
			{
				this.qOperateProcess.Enqueue(clsOperateInfo);
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000CAAC4 File Offset: 0x000C9AC4
		private void updateDoorProcess_ByTimer1(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc, string startTime, string endTime)
		{
			try
			{
				lock (this.dtDoors)
				{
					using (DataView dataView = new DataView(this.dtDoors))
					{
						dataView.RowFilter = "f_DoorName = " + wgTools.PrepareStr(doorName);
						if (dataView.Count > 0)
						{
							if (this.op != dfrmMultiThreadOperation.Operation.RemoteOpen)
							{
								dataView.RowFilter = "f_ControllerSN = " + int.Parse(dataView[0]["f_ControllerSN"].ToString()).ToString();
							}
							int count = dataView.Count;
							for (int i = 0; i < dataView.Count; i++)
							{
								if (proc == dfrmMultiThreadOperation.RunStatus.UpdateTotal)
								{
									if (this.arrFirstDoor.Count > 0 && this.arrFirstDoor.IndexOf(dataView[i]["f_DoorName"]) >= 0)
									{
										dataView[i]["f_Total"] = info;
									}
								}
								else
								{
									dataView[i]["f_Process"] = info;
									if (!string.IsNullOrEmpty(startTime))
									{
										dataView[i]["f_StartTime"] = startTime;
									}
									if (!string.IsNullOrEmpty(endTime))
									{
										dataView[i]["f_EndTime"] = endTime;
									}
									if (proc == dfrmMultiThreadOperation.RunStatus.CompleteOK)
									{
										dataView[i]["f_RunStatus"] = 1;
										if (this.arrFirstDoor.Count > 0 && this.arrFirstDoor.IndexOf(dataView[i]["f_DoorName"]) < 0)
										{
											if (this.op == dfrmMultiThreadOperation.Operation.Download)
											{
												dataView[i]["f_Process"] = CommonStr.strAlreadyUploadPrivileges;
											}
											if (this.op == dfrmMultiThreadOperation.Operation.GetSwipes)
											{
												dataView[i]["f_Process"] = CommonStr.strAlreadyGotSwipeRecord;
											}
										}
									}
									if (proc == dfrmMultiThreadOperation.RunStatus.CommFail)
									{
										dataView[i]["f_RunStatus"] = -1;
									}
								}
							}
						}
					}
					this.dtDoors.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000CAD24 File Offset: 0x000C9D24
		private void updateEndTime(string doorName, string info)
		{
			this.updateOperateInfo(doorName, "f_EndTime", info);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000CAD34 File Offset: 0x000C9D34
		private void updateLabelInfo(ref int dealtDoorsTemp, ref int totalDoors, ref int failedDoors)
		{
			DataView dataView = new DataView(this.dtDoors);
			failedDoors = 0;
			dealtDoorsTemp = 0;
			totalDoors = dataView.Count;
			dataView.RowFilter = "f_RunStatus = -1";
			this.txtFailedDoors.Text = dataView.Count.ToString();
			failedDoors = dataView.Count;
			this.txtFailedControllers.Text = this.getControllerCount(dataView).ToString();
			dealtDoorsTemp += dataView.Count;
			dataView.RowFilter = "f_RunStatus = 1";
			this.txtDownloadedDoors.Text = dataView.Count.ToString();
			this.txtCompleteOK.Text = this.getControllerCount(dataView).ToString();
			dealtDoorsTemp += dataView.Count;
			dataView.RowFilter = "f_RunStatus = 0";
			this.txtDealingDoors.Text = dataView.Count.ToString();
			this.txtDealingControllers.Text = this.getControllerCount(dataView).ToString();
			if (this.op == dfrmMultiThreadOperation.Operation.Download || this.op == dfrmMultiThreadOperation.Operation.GetSwipes)
			{
				dataView.RowFilter = "f_RunStatus = 1";
				this.txtTotalItems.Text = this.getTotalItems(dataView).ToString();
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x000CAE70 File Offset: 0x000C9E70
		private void updateOperateInfo(string doorName, string fieldName, string info)
		{
			lock (this.qOperateInfo.SyncRoot)
			{
				dfrmMultiThreadOperation.clsOperateInfo clsOperateInfo = new dfrmMultiThreadOperation.clsOperateInfo(doorName, fieldName, info);
				this.qOperateInfo.Enqueue(clsOperateInfo);
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000CAEC0 File Offset: 0x000C9EC0
		private void updateOperateInfo_ByTimer1(string doorName, string fieldName, string info)
		{
			try
			{
				lock (this.dtDoors)
				{
					this.dvDoors4updateDownloadInfo.RowFilter = "f_DoorName = " + wgTools.PrepareStr(doorName);
					byte b = (byte)this.dvDoors4updateDownloadInfo[0]["f_DoorNO"];
					if (this.dvDoors4updateDownloadInfo.Count > 0)
					{
						int num = int.Parse(this.dvDoors4updateDownloadInfo[0]["f_ControllerSN"].ToString());
						for (int i = 0; i < this.dtDoors.Rows.Count; i++)
						{
							if ((int)this.dtDoors.Rows[i]["f_ControllerSN"] == num)
							{
								if ((this.op == dfrmMultiThreadOperation.Operation.Download || this.op == dfrmMultiThreadOperation.Operation.GetSwipes) && fieldName.ToUpper() == "f_Total".ToUpper())
								{
									if (this.arrFirstDoor.Count > 0 && this.arrFirstDoor.IndexOf(this.dtDoors.Rows[i]["f_DoorName"]) >= 0)
									{
										this.dtDoors.Rows[i][fieldName] = info;
									}
								}
								else
								{
									this.dtDoors.Rows[i][fieldName] = info;
								}
							}
						}
					}
					this.dtDoors.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000CB078 File Offset: 0x000CA078
		private void updateStartTime(string doorName, string info)
		{
			this.updateOperateInfo(doorName, "f_StartTime", info);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000CB087 File Offset: 0x000CA087
		private void wait4Accelerate()
		{
			Thread.Sleep(2);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000CB090 File Offset: 0x000CA090
		private void writeDisplayedInfo(string info)
		{
			InfoRow infoRow = new InfoRow();
			infoRow.desc = string.Format("{0},{1}\r\n", this.Text, info);
			infoRow.desc += string.Format("{0}\t", this.grpDoors.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label6.Text, this.txtTotalDoors.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label4.Text, this.txtDownloadedDoors.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label5.Text, this.txtFailedDoors.Text);
			infoRow.desc += string.Format("\r\n", new object[0]);
			infoRow.desc += string.Format("{0}\t", this.btnOtherInfo.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label1.Text, this.txtAllControllerCnt.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label3.Text, this.txtCompleteOK.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label2.Text, this.txtFailedControllers.Text);
			infoRow.desc += string.Format("{0}:{1}\t", this.label8.Text, this.txtTotalItems.Text);
			infoRow.information = string.Format("{0}", info);
			this.addEventToLog1(infoRow);
		}

		// Token: 0x04000C9F RID: 3231
		private bool bActivateTimeProfile;

		// Token: 0x04000CA0 RID: 3232
		private int CommOperateOption;

		// Token: 0x04000CA1 RID: 3233
		private DataView dvDoors4updateDoorProcess;

		// Token: 0x04000CA2 RID: 3234
		private DataView dvDoors4updateDownloadInfo;

		// Token: 0x04000CA3 RID: 3235
		private dfrmMultiThreadOperation.Operation op;

		// Token: 0x04000CA4 RID: 3236
		private int ThreadPoolMaxThreadNum;

		// Token: 0x04000CA5 RID: 3237
		private int usingThreadNum;

		// Token: 0x04000CA7 RID: 3239
		public ArrayList arrConsumerID = new ArrayList();

		// Token: 0x04000CA8 RID: 3240
		private ArrayList arrDealtDoors = new ArrayList();

		// Token: 0x04000CA9 RID: 3241
		private ArrayList arrDealtSN = new ArrayList();

		// Token: 0x04000CAA RID: 3242
		private ArrayList arrDoorsInfo = new ArrayList();

		// Token: 0x04000CAB RID: 3243
		private ArrayList arrFirstDoor = new ArrayList();

		// Token: 0x04000CAC RID: 3244
		private ArrayList arrIPARPFailed = new ArrayList();

		// Token: 0x04000CAD RID: 3245
		private ArrayList arrIPARPOK = new ArrayList();

		// Token: 0x04000CAE RID: 3246
		private ArrayList arrIPPingFailed = new ArrayList();

		// Token: 0x04000CAF RID: 3247
		private ArrayList arrIPPingOK = new ArrayList();

		// Token: 0x04000CB0 RID: 3248
		private ArrayList arrSelectedDoors = new ArrayList();

		// Token: 0x04000CB1 RID: 3249
		public ArrayList arrSelectedDoorsItem = new ArrayList();

		// Token: 0x04000CB2 RID: 3250
		public ArrayList arrSelectedDoorsOnConsole = new ArrayList();

		// Token: 0x04000CB3 RID: 3251
		public bool bComplete4AddDelPri;

		// Token: 0x04000CB4 RID: 3252
		public bool bComplete4Cardlost;

		// Token: 0x04000CB5 RID: 3253
		public bool bComplete4FingerSingleUpdate;

		// Token: 0x04000CB6 RID: 3254
		public bool bCompleteFullOK4AddDelPri;

		// Token: 0x04000CB7 RID: 3255
		public bool bCompleteFullOK4Cardlost;

		// Token: 0x04000CB8 RID: 3256
		public int ConsumerID4AddDelPri;

		// Token: 0x04000CB9 RID: 3257
		public int ConsumerID4Cardlost;

		// Token: 0x04000CBA RID: 3258
		private icControllerTimeSegList controlTimeSegList = new icControllerTimeSegList();

		// Token: 0x04000CBB RID: 3259
		public ArrayList delPrivilegeController = new ArrayList();

		// Token: 0x04000CBC RID: 3260
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04000CBD RID: 3261
		private int doorControl = -1;

		// Token: 0x04000CBE RID: 3262
		private int doorDelay = -1;

		// Token: 0x04000CBF RID: 3263
		private DataTable dtDoors = new DataTable();

		// Token: 0x04000CC0 RID: 3264
		private byte[] fingerCard = new byte[4096];

		// Token: 0x04000CC1 RID: 3265
		private int fingerCnt = -1;

		// Token: 0x04000CC2 RID: 3266
		public byte[] fingerDataParam;

		// Token: 0x04000CC3 RID: 3267
		private byte[] fingerInfo = new byte[524288];

		// Token: 0x04000CC4 RID: 3268
		public string newCardNO4AddDelPri = "";

		// Token: 0x04000CC5 RID: 3269
		public string newCardNO4Cardlost = "";

		// Token: 0x04000CC6 RID: 3270
		public string oldCardNO4AddDelPri = "";

		// Token: 0x04000CC7 RID: 3271
		public string oldCardNO4Cardlost = "";

		// Token: 0x04000CC8 RID: 3272
		private Queue q4Syn10 = new Queue();

		// Token: 0x04000CC9 RID: 3273
		private Queue qEvent = new Queue();

		// Token: 0x04000CCA RID: 3274
		private Queue qFail4AddDelPri = new Queue();

		// Token: 0x04000CCB RID: 3275
		private Queue qFail4Cardlost = new Queue();

		// Token: 0x04000CCC RID: 3276
		private Queue qLock4GetInfoFromDBByDoorName = new Queue();

		// Token: 0x04000CCD RID: 3277
		private Queue qLock4MultithreadDBOperation = new Queue();

		// Token: 0x04000CCE RID: 3278
		private Queue qOperateInfo = new Queue();

		// Token: 0x04000CCF RID: 3279
		private Queue qOperateProcess = new Queue();

		// Token: 0x04000CD0 RID: 3280
		private Queue qProductsInfo4DownloadPriviege = new Queue();

		// Token: 0x04000CD1 RID: 3281
		private Queue qSuccess4AddDelPri = new Queue();

		// Token: 0x04000CD2 RID: 3282
		private Queue qSuccess4Cardlost = new Queue();

		// Token: 0x04000CD3 RID: 3283
		private static long tickdfrmMultiThreadOperation = 0L;

		// Token: 0x04000CD4 RID: 3284
		private static long tickdfrmMultiThreadOperationStart = 0L;

		// Token: 0x04000CD5 RID: 3285
		private Queue waitDoors = new Queue();

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x06000716 RID: 1814
		public delegate void appRunInfoCommStatusHandler(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc);

		// Token: 0x0200005C RID: 92
		private class clsControlInfo
		{
			// Token: 0x04000D0C RID: 3340
			public byte[] bytToSend;

			// Token: 0x04000D0D RID: 3341
			public string IP;

			// Token: 0x04000D0E RID: 3342
			public int Port;
		}

		// Token: 0x0200005D RID: 93
		private class clsDoorsInfo
		{
			// Token: 0x0600071A RID: 1818 RVA: 0x000CC750 File Offset: 0x000CB750
			public clsDoorsInfo(string doorName, int SN, string IP, int Port)
			{
				this.m_port = 60000;
				this.m_doorControl = new int[] { 3, 3, 3, 3 };
				this.m_doorDelay = new int[] { 30, 30, 30, 30 };
				this.m_doorNeedUpdate = new int[4];
				this.m_iCardID = -1L;
				this.m_doorName = doorName;
				this.m_sn = SN;
				this.m_ip = IP;
				this.m_port = Port;
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x000CC7F0 File Offset: 0x000CB7F0
			public clsDoorsInfo(int controllerID, string doorName, int SN, string IP, int Port)
			{
				this.m_port = 60000;
				this.m_doorControl = new int[] { 3, 3, 3, 3 };
				this.m_doorDelay = new int[] { 30, 30, 30, 30 };
				this.m_doorNeedUpdate = new int[4];
				this.m_iCardID = -1L;
				this.m_doorName = doorName;
				this.m_sn = SN;
				this.m_ip = IP;
				this.m_port = Port;
				this.m_controllerID = controllerID;
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x000CC898 File Offset: 0x000CB898
			public clsDoorsInfo(string doorName, int SN, string IP, int Port, int doorNO)
			{
				this.m_port = 60000;
				this.m_doorControl = new int[] { 3, 3, 3, 3 };
				this.m_doorDelay = new int[] { 30, 30, 30, 30 };
				this.m_doorNeedUpdate = new int[4];
				this.m_iCardID = -1L;
				this.m_doorName = doorName;
				this.m_sn = SN;
				this.m_ip = IP;
				this.m_port = Port;
				this.m_doorNO = doorNO;
			}

			// Token: 0x0600071D RID: 1821 RVA: 0x000CC940 File Offset: 0x000CB940
			public clsDoorsInfo(int controllerID, string doorName, int SN, string IP, int Port, MjRegisterCard mjrc, long iCardID)
			{
				this.m_port = 60000;
				this.m_doorControl = new int[] { 3, 3, 3, 3 };
				this.m_doorDelay = new int[] { 30, 30, 30, 30 };
				this.m_doorNeedUpdate = new int[4];
				this.m_iCardID = -1L;
				this.m_doorName = doorName;
				this.m_sn = SN;
				this.m_ip = IP;
				this.m_port = Port;
				this.m_controllerID = controllerID;
				this.m_mjrc = mjrc;
				this.m_iCardID = iCardID;
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x000CC9D5 File Offset: 0x000CB9D5
			public int getDoorControl(int doorNO)
			{
				return this.m_doorControl[doorNO - 1];
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x000CC9E1 File Offset: 0x000CB9E1
			public int getDoorDelay(int doorNO)
			{
				return this.m_doorDelay[doorNO - 1];
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x000CC9ED File Offset: 0x000CB9ED
			public int getDoorNeedUpdate(int doorNO)
			{
				return this.m_doorNeedUpdate[doorNO - 1];
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x000CC9F9 File Offset: 0x000CB9F9
			public void setDoorControl(int doorNO, int val)
			{
				this.m_doorControl[doorNO - 1] = val;
				this.m_doorNeedUpdate[doorNO - 1] = 1;
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x000CCA11 File Offset: 0x000CBA11
			public void setDoorDelay(int doorNO, int val)
			{
				this.m_doorDelay[doorNO - 1] = val;
				this.m_doorNeedUpdate[doorNO - 1] = 1;
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000723 RID: 1827 RVA: 0x000CCA29 File Offset: 0x000CBA29
			public int ControllerID
			{
				get
				{
					return this.m_controllerID;
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000724 RID: 1828 RVA: 0x000CCA31 File Offset: 0x000CBA31
			public int ControllerSN
			{
				get
				{
					return this.m_sn;
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000725 RID: 1829 RVA: 0x000CCA39 File Offset: 0x000CBA39
			public string DoorName
			{
				get
				{
					return this.m_doorName;
				}
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000726 RID: 1830 RVA: 0x000CCA41 File Offset: 0x000CBA41
			public int DoorNO
			{
				get
				{
					return this.m_doorNO;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000727 RID: 1831 RVA: 0x000CCA49 File Offset: 0x000CBA49
			public long iCardID
			{
				get
				{
					return this.m_iCardID;
				}
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000728 RID: 1832 RVA: 0x000CCA51 File Offset: 0x000CBA51
			public string IP
			{
				get
				{
					return this.m_ip;
				}
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000729 RID: 1833 RVA: 0x000CCA59 File Offset: 0x000CBA59
			public MjRegisterCard Mjrc
			{
				get
				{
					return this.m_mjrc;
				}
			}

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600072A RID: 1834 RVA: 0x000CCA61 File Offset: 0x000CBA61
			public int Port
			{
				get
				{
					return this.m_port;
				}
			}

			// Token: 0x04000D0F RID: 3343
			public dfrmMultiThreadOperation.RunStatus commStatus;

			// Token: 0x04000D10 RID: 3344
			private int[] m_doorControl;

			// Token: 0x04000D11 RID: 3345
			private int[] m_doorDelay;

			// Token: 0x04000D12 RID: 3346
			private int[] m_doorNeedUpdate;

			// Token: 0x04000D13 RID: 3347
			private int m_controllerID;

			// Token: 0x04000D14 RID: 3348
			private string m_doorName;

			// Token: 0x04000D15 RID: 3349
			private int m_doorNO;

			// Token: 0x04000D16 RID: 3350
			private long m_iCardID;

			// Token: 0x04000D17 RID: 3351
			private string m_ip;

			// Token: 0x04000D18 RID: 3352
			private MjRegisterCard m_mjrc;

			// Token: 0x04000D19 RID: 3353
			private int m_port;

			// Token: 0x04000D1A RID: 3354
			private int m_sn;
		}

		// Token: 0x0200005E RID: 94
		private class clsOperateInfo
		{
			// Token: 0x0600072B RID: 1835 RVA: 0x000CCA69 File Offset: 0x000CBA69
			public clsOperateInfo(string doorName, string fieldName, string info)
			{
				this.m_startTime = "";
				this.m_EndTime = "";
				this.m_doorName = doorName;
				this.m_fieldName = fieldName;
				this.m_info = info;
			}

			// Token: 0x0600072C RID: 1836 RVA: 0x000CCA9C File Offset: 0x000CBA9C
			public clsOperateInfo(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc)
			{
				this.m_startTime = "";
				this.m_EndTime = "";
				this.m_doorName = doorName;
				this.m_info = info;
				this.m_proc = proc;
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x000CCAD0 File Offset: 0x000CBAD0
			public clsOperateInfo(string doorName, string info, dfrmMultiThreadOperation.RunStatus proc, string startTime, string EndTime)
			{
				this.m_startTime = "";
				this.m_EndTime = "";
				this.m_doorName = doorName;
				this.m_info = info;
				this.m_proc = proc;
				this.m_startTime = startTime;
				this.m_EndTime = EndTime;
			}

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600072E RID: 1838 RVA: 0x000CCB1E File Offset: 0x000CBB1E
			public string DoorName
			{
				get
				{
					return this.m_doorName;
				}
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x0600072F RID: 1839 RVA: 0x000CCB26 File Offset: 0x000CBB26
			public string EndTime
			{
				get
				{
					return this.m_EndTime;
				}
			}

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x06000730 RID: 1840 RVA: 0x000CCB2E File Offset: 0x000CBB2E
			public string FieldName
			{
				get
				{
					return this.m_fieldName;
				}
			}

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000731 RID: 1841 RVA: 0x000CCB36 File Offset: 0x000CBB36
			public string Info
			{
				get
				{
					return this.m_info;
				}
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000732 RID: 1842 RVA: 0x000CCB3E File Offset: 0x000CBB3E
			public dfrmMultiThreadOperation.RunStatus Proc
			{
				get
				{
					return this.m_proc;
				}
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000733 RID: 1843 RVA: 0x000CCB46 File Offset: 0x000CBB46
			public string StartTime
			{
				get
				{
					return this.m_startTime;
				}
			}

			// Token: 0x04000D1B RID: 3355
			private string m_doorName;

			// Token: 0x04000D1C RID: 3356
			private string m_EndTime;

			// Token: 0x04000D1D RID: 3357
			private string m_fieldName;

			// Token: 0x04000D1E RID: 3358
			private string m_info;

			// Token: 0x04000D1F RID: 3359
			private dfrmMultiThreadOperation.RunStatus m_proc;

			// Token: 0x04000D20 RID: 3360
			private string m_startTime;
		}

		// Token: 0x02000061 RID: 97
		private class icPrivilegeMutithread : icPrivilege
		{
			// Token: 0x0600076B RID: 1899 RVA: 0x000D2D1C File Offset: 0x000D1D1C
			protected override void DisplayProcessInfo(string info, int infoCode, int specialInfo)
			{
				if (infoCode == -100001)
				{
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, wgTools.gADCT ? CommonStr.strUploadFail_200K : (wgTools.gWGYTJ ? CommonStr.strUploadFail_500 : CommonStr.strUploadFail_40K), dfrmMultiThreadOperation.RunStatus.CommFail);
					return;
				}
				switch (infoCode)
				{
				case 100001:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, CommonStr.strUploadPreparing, dfrmMultiThreadOperation.RunStatus.Start);
					return;
				case 100002:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strUploadingPrivileges, specialInfo), dfrmMultiThreadOperation.RunStatus.Downloading);
					return;
				case 100003:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strUploadingPrivileges, specialInfo), dfrmMultiThreadOperation.RunStatus.CompleteOK);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x02000064 RID: 100
		private class icSwipeRecordMutithread : icSwipeRecord
		{
			// Token: 0x06000797 RID: 1943 RVA: 0x000D847C File Offset: 0x000D747C
			protected override void DisplayProcessInfo(string info, int infoCode, int specialInfo)
			{
				switch (infoCode)
				{
				case 1:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strGotRecords, specialInfo), dfrmMultiThreadOperation.RunStatus.GetSwiping);
					return;
				case 2:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strWritingRecordsToDB, specialInfo), dfrmMultiThreadOperation.RunStatus.WriteSwipingToDB);
					return;
				case 3:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strGetSwipeRecordOK, specialInfo), dfrmMultiThreadOperation.RunStatus.CompleteOK);
					return;
				case 4:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, specialInfo.ToString(), dfrmMultiThreadOperation.RunStatus.UpdateTotal);
					return;
				case 5:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, CommonStr.strWatingForWritingRecordsToDB, dfrmMultiThreadOperation.RunStatus.WriteSwipingToDB);
					return;
				default:
					dfrmMultiThreadOperation.raiseAppRunInfoCommStatus(info, string.Format("{0}[{1:d}]", CommonStr.strGetSwipeRecordOK, specialInfo), dfrmMultiThreadOperation.RunStatus.CompleteOK);
					return;
				}
			}
		}

		// Token: 0x02000065 RID: 101
		public enum Operation
		{
			// Token: 0x04000D4D RID: 3405
			GetSwipes,
			// Token: 0x04000D4E RID: 3406
			Download,
			// Token: 0x04000D4F RID: 3407
			RemoteOpen,
			// Token: 0x04000D50 RID: 3408
			SetDoorControl,
			// Token: 0x04000D51 RID: 3409
			CardLost,
			// Token: 0x04000D52 RID: 3410
			DelPri,
			// Token: 0x04000D53 RID: 3411
			AddPri,
			// Token: 0x04000D54 RID: 3412
			AdjustTime,
			// Token: 0x04000D55 RID: 3413
			FingerSingleUpdate
		}

		// Token: 0x02000066 RID: 102
		public enum RunStatus
		{
			// Token: 0x04000D57 RID: 3415
			Start,
			// Token: 0x04000D58 RID: 3416
			CommFail,
			// Token: 0x04000D59 RID: 3417
			CompleteOK,
			// Token: 0x04000D5A RID: 3418
			Downloading,
			// Token: 0x04000D5B RID: 3419
			Download_Err_PRIVILEGES_OVER200K,
			// Token: 0x04000D5C RID: 3420
			GetSwiping,
			// Token: 0x04000D5D RID: 3421
			WriteSwipingToDB,
			// Token: 0x04000D5E RID: 3422
			UpdateTotal
		}
	}
}
