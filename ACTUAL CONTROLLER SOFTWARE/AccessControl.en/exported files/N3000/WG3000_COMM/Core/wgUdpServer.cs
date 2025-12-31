using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WG3000_COMM.DataOper;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x02000215 RID: 533
	public class wgUdpServer : IDisposable
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000EFF RID: 3839 RVA: 0x0010BBE0 File Offset: 0x0010ABE0
		// (remove) Token: 0x06000F00 RID: 3840 RVA: 0x0010BBE9 File Offset: 0x0010ABE9
		public event wgUdpServer.newRecordHandler evNewRecord
		{
			add
			{
				this.evNewRecords += value;
			}
			remove
			{
				this.evNewRecords -= value;
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000F01 RID: 3841 RVA: 0x0010BBF4 File Offset: 0x0010ABF4
		// (remove) Token: 0x06000F02 RID: 3842 RVA: 0x0010BC2C File Offset: 0x0010AC2C
		private event wgUdpServer.newRecordHandler evNewRecords;

		// Token: 0x06000F03 RID: 3843 RVA: 0x0010BC64 File Offset: 0x0010AC64
		public wgUdpServer()
		{
			try
			{
				this.create4Multithread();
				this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				wgUdpComm.SockSpecialSet(ref this.UdpSocket);
				this.UdpSocket.EnableBroadcast = true;
				this.UdpSocket.ReceiveBufferSize = 16777216;
				this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
				this.UDPListenThread.Name = "wgUdpServer";
				this.UDPListenThread.IsBackground = true;
				this.UDPListenThread.Start();
				this.DealRuninfoPacketThread = new Thread(new ThreadStart(this.DealRuninfoPacketProc));
				this.DealRuninfoPacketThread.Name = "Deal Run InfoPacket";
				this.DealRuninfoPacketThread.IsBackground = true;
				this.DealRuninfoPacketThread.Start();
				Thread.Sleep(10);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0010BDA0 File Offset: 0x0010ADA0
		public bool Close()
		{
			try
			{
				this.bUDPListenStop = true;
				Thread.Sleep(20);
				if (this.UDPListenThread != null)
				{
					this.UDPListenThread.Abort();
				}
				if (this.UDPQueue != null)
				{
					this.UDPQueue.Clear();
					this.UDPQueue = null;
				}
				if (this.UDPQueue4get != null)
				{
					this.UDPQueue4get.Clear();
					this.UDPQueue4get = null;
				}
				this.close4Multithread();
				if (this.DealRuninfoPacketThread != null)
				{
					this.DealRuninfoPacketThread.Abort();
				}
				if (this.UdpSocket != null)
				{
					this.UdpSocket.Close();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return true;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0010BE44 File Offset: 0x0010AE44
		public void close4Multithread()
		{
			if (wgTools.bUDPCloud > 0)
			{
				for (int i = 0; i < this.arrSN4Multithread.Length; i++)
				{
					if (this.arrUDPQueue4Multithread[i] != null)
					{
						this.arrUDPQueue4Multithread[i].Clear();
						this.arrUDPQueue4Multithread[i] = null;
					}
				}
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0010BE8C File Offset: 0x0010AE8C
		public void create4Multithread()
		{
			if (wgTools.bUDPCloud > 0)
			{
				this.arrUDPQueue4Multithread = new Queue[100];
				this.arrSN4Multithread = new uint[100];
				this.arrSNStartTime4Multithread = new DateTime[100];
				for (int i = 0; i < this.arrSN4Multithread.Length; i++)
				{
					this.arrSN4Multithread[i] = 0U;
				}
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0010BEE4 File Offset: 0x0010AEE4
		private void DealRuninfoPacketProc()
		{
			try
			{
				ControllerRunInformation controllerRunInformation = null;
				do
				{
					if (this.UDPQueue.Count > 0)
					{
						byte[] array;
						lock (this.UDPQueue.SyncRoot)
						{
							array = (byte[])this.UDPQueue.Dequeue();
						}
						wgAppConfig.glngReceiveCount = this.lngReceiveCount;
						if (wgTools.bUDPOnly64 > 0 && array.Length == 64 && array[0] == 23 && array[1] == 32)
						{
							this.DealRuninfoPacketProc64(array, controllerRunInformation);
						}
						else if (array.Length == wgMjControllerRunInformation.pktlen && array[0] == 32 && array[1] == 33)
						{
							uint num = BitConverter.ToUInt32(array, 8);
							if (this.curNowWatching != null && this.curNowWatching.Count > 0 && this.curNowWatching.ContainsKey((int)num))
							{
								if (this.arrControllerConnected.IndexOf(num) < 0)
								{
									lock (this.arrControllerConnected)
									{
										this.arrControllerConnected.Add(num);
									}
								}
								if (!this.watchedControllers.ContainsKey(num))
								{
									this.watchedControllers.Add(num, new wgUdpServer.udpController(num));
								}
								controllerRunInformation = this.watchedControllers[num].runinfo;
								controllerRunInformation.update(array, 20, num);
								if (controllerRunInformation.newSwipes[0].IndexInDataFlash != 4294967295U)
								{
									if (controllerRunInformation.newSwipes[0].IndexInDataFlash > this.watchedControllers[num].lastRecordIndex || this.watchedControllers[num].lastRecordIndex == 4294967295U)
									{
										for (int i = 9; i >= 0; i--)
										{
											if (controllerRunInformation.newSwipes[i].IndexInDataFlash != 4294967295U)
											{
												if (controllerRunInformation.newSwipes[i].IndexInDataFlash > this.watchedControllers[num].lastRecordIndex || this.watchedControllers[num].lastRecordIndex == 4294967295U)
												{
													this.watchedControllers[num].lastRecordIndex = controllerRunInformation.newSwipes[i].IndexInDataFlash;
													this.watchedControllers[num].lastStringRaw = controllerRunInformation.newSwipes[i].ToStringRaw();
													if (!this.watchedControllers[num].isFirstComm)
													{
														this.RaiseEvNewRecord(controllerRunInformation.newSwipes[i].ToStringRaw());
														this.iNewRecordsCnt += 1L;
													}
												}
												else if (controllerRunInformation.newSwipes[i].IndexInDataFlash == this.watchedControllers[num].lastRecordIndex && this.watchedControllers[num].lastStringRaw.CompareTo(controllerRunInformation.newSwipes[i].ToStringRaw()) != 0)
												{
													this.watchedControllers[num].lastRecordIndex = controllerRunInformation.newSwipes[i].IndexInDataFlash;
													this.watchedControllers[num].lastStringRaw = controllerRunInformation.newSwipes[i].ToStringRaw();
													if (!this.watchedControllers[num].isFirstComm)
													{
														this.RaiseEvNewRecord(controllerRunInformation.newSwipes[i].ToStringRaw());
														this.iNewRecordsCnt += 1L;
													}
												}
											}
										}
									}
									else if (!string.IsNullOrEmpty(this.watchedControllers[num].lastStringRaw) && this.watchedControllers[num].lastStringRaw.CompareTo(controllerRunInformation.newSwipes[0].ToStringRaw()) != 0)
									{
										int num2 = 10;
										for (int j = 9; j >= 0; j--)
										{
											if (controllerRunInformation.newSwipes[j].IndexInDataFlash != 4294967295U && this.watchedControllers[num].lastStringRaw.CompareTo(controllerRunInformation.newSwipes[j].ToStringRaw()) == 0)
											{
												num2 = j;
												break;
											}
										}
										if (num2 > 0)
										{
											for (int k = num2 - 1; k >= 0; k--)
											{
												if (controllerRunInformation.newSwipes[k].IndexInDataFlash != 4294967295U)
												{
													this.watchedControllers[num].lastRecordIndex = controllerRunInformation.newSwipes[k].IndexInDataFlash;
													this.watchedControllers[num].lastStringRaw = controllerRunInformation.newSwipes[k].ToStringRaw();
													if (!this.watchedControllers[num].isFirstComm)
													{
														this.RaiseEvNewRecord(controllerRunInformation.newSwipes[k].ToStringRaw());
														this.iNewRecordsCnt += 1L;
													}
												}
											}
										}
									}
								}
								if (this.watchedControllers[num].isFirstComm)
								{
									this.watchedControllers[num].isFirstComm = false;
								}
							}
						}
					}
					else
					{
						Thread.Sleep(1);
					}
				}
				while (!this.bUDPListenStop);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0010C3D8 File Offset: 0x0010B3D8
		private void DealRuninfoPacketProc64(byte[] recv, ControllerRunInformation m_runinfo)
		{
			uint num = BitConverter.ToUInt32(recv, 4);
			if (this.curNowWatching != null && this.curNowWatching.Count > 0 && this.curNowWatching.ContainsKey((int)num))
			{
				if (this.arrControllerConnected.IndexOf(num) < 0)
				{
					lock (this.arrControllerConnected)
					{
						this.arrControllerConnected.Add(num);
					}
				}
				if (!this.watchedControllers.ContainsKey(num))
				{
					this.watchedControllers.Add(num, new wgUdpServer.udpController(num));
				}
				m_runinfo = this.watchedControllers[num].runinfo;
				m_runinfo.UpdateInfo_internal64(recv, 0, num);
				m_runinfo.updateFirstP64(recv, 0, num);
				if (m_runinfo.newSwipes[0] != null && m_runinfo.newSwipes[0].IndexInDataFlash != 4294967295U)
				{
					if (m_runinfo.newSwipes[0].IndexInDataFlash > this.watchedControllers[num].lastRecordIndex || this.watchedControllers[num].lastRecordIndex == 4294967295U)
					{
						for (int i = 0; i >= 0; i--)
						{
							if (m_runinfo.newSwipes[i].IndexInDataFlash != 4294967295U)
							{
								if (m_runinfo.newSwipes[i].IndexInDataFlash > this.watchedControllers[num].lastRecordIndex || this.watchedControllers[num].lastRecordIndex == 4294967295U)
								{
									this.watchedControllers[num].lastRecordIndex = m_runinfo.newSwipes[i].IndexInDataFlash;
									this.watchedControllers[num].lastStringRaw = m_runinfo.newSwipes[i].ToStringRaw();
									if (!this.watchedControllers[num].isFirstComm)
									{
										this.RaiseEvNewRecord(m_runinfo.newSwipes[i].ToStringRaw());
										this.iNewRecordsCnt += 1L;
									}
								}
								else if (m_runinfo.newSwipes[i].IndexInDataFlash == this.watchedControllers[num].lastRecordIndex && this.watchedControllers[num].lastStringRaw.CompareTo(m_runinfo.newSwipes[i].ToStringRaw()) != 0)
								{
									this.watchedControllers[num].lastRecordIndex = m_runinfo.newSwipes[i].IndexInDataFlash;
									this.watchedControllers[num].lastStringRaw = m_runinfo.newSwipes[i].ToStringRaw();
									if (!this.watchedControllers[num].isFirstComm)
									{
										this.RaiseEvNewRecord(m_runinfo.newSwipes[i].ToStringRaw());
										this.iNewRecordsCnt += 1L;
									}
								}
							}
						}
					}
					else if (!string.IsNullOrEmpty(this.watchedControllers[num].lastStringRaw) && this.watchedControllers[num].lastStringRaw.CompareTo(m_runinfo.newSwipes[0].ToStringRaw()) != 0)
					{
						int num2 = 10;
						for (int j = 0; j >= 0; j--)
						{
							if (m_runinfo.newSwipes[j].IndexInDataFlash != 4294967295U && this.watchedControllers[num].lastStringRaw.CompareTo(m_runinfo.newSwipes[j].ToStringRaw()) == 0)
							{
								num2 = j;
								break;
							}
						}
						if (num2 > 0)
						{
							for (int k = num2 - 1; k >= 0; k--)
							{
								if (m_runinfo.newSwipes[k].IndexInDataFlash != 4294967295U)
								{
									this.watchedControllers[num].lastRecordIndex = m_runinfo.newSwipes[k].IndexInDataFlash;
									this.watchedControllers[num].lastStringRaw = m_runinfo.newSwipes[k].ToStringRaw();
									if (!this.watchedControllers[num].isFirstComm)
									{
										this.RaiseEvNewRecord(m_runinfo.newSwipes[k].ToStringRaw());
										this.iNewRecordsCnt += 1L;
									}
								}
							}
						}
					}
				}
				if (this.watchedControllers[num].isFirstComm)
				{
					this.watchedControllers[num].isFirstComm = false;
				}
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0010C7E8 File Offset: 0x0010B7E8
		private void dispatchToUDPQueue4Multithread(byte[] recv)
		{
			if (wgTools.bUDPCloud > 0 && this.workingCnt > 0)
			{
				uint num;
				if (recv.Length == 64)
				{
					num = BitConverter.ToUInt32(recv, 4);
				}
				else
				{
					num = BitConverter.ToUInt32(recv, 8);
				}
				for (int i = 0; i < this.workingCnt; i++)
				{
					if (this.arrSN4Multithread[i] == num)
					{
						if (this.arrUDPQueue4Multithread[i] == null)
						{
							return;
						}
						lock (this.arrUDPQueue4Multithread[i].SyncRoot)
						{
							this.arrUDPQueue4Multithread[i].Enqueue(recv);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0010C884 File Offset: 0x0010B884
		private void dispatchToUDPQueue4Remoteopen(byte[] recv)
		{
			if (wgTools.bUDPCloud > 0 && recv.Length == 64)
			{
				BitConverter.ToUInt32(recv, 4);
				if (recv[1] == 64)
				{
					lock (wgUdpServer.UDPQueue4Remoteopen.SyncRoot)
					{
						wgUdpServer.UDPQueue4Remoteopen.Enqueue(recv);
						return;
					}
				}
				if (recv[1] == 48 || recv[1] == 50)
				{
					lock (wgUdpServer.UDPQueue4AdjustTime.SyncRoot)
					{
						wgUdpServer.UDPQueue4AdjustTime.Enqueue(recv);
						return;
					}
				}
				if (recv[1] == 128)
				{
					lock (wgUdpServer.UDPQueue4SetDoorControl.SyncRoot)
					{
						wgUdpServer.UDPQueue4SetDoorControl.Enqueue(recv);
					}
				}
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0010C964 File Offset: 0x0010B964
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0010C973 File Offset: 0x0010B973
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0010C980 File Offset: 0x0010B980
		public static byte[] getAdjustTimePacket()
		{
			if (wgUdpServer.UDPQueue4AdjustTime.Count <= 0)
			{
				return null;
			}
			byte[] array;
			lock (wgUdpServer.UDPQueue4AdjustTime.SyncRoot)
			{
				array = (byte[])wgUdpServer.UDPQueue4AdjustTime.Dequeue();
			}
			return array;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0010C9D8 File Offset: 0x0010B9D8
		public static int GetIPEndByIPAddr(string ipAddr, int ipPort, ref IPEndPoint endp)
		{
			int num = -1;
			if (ipAddr == null)
			{
				endp = new IPEndPoint(IPAddress.Broadcast, 60000);
				num = 0;
			}
			else if (ipAddr.Length == 0)
			{
				endp = new IPEndPoint(IPAddress.Broadcast, 60000);
				num = 0;
			}
			else
			{
				try
				{
					if (ipAddr == "192.0.0.0" || ipAddr == "192.168.0.0" || ipAddr == "192.168.168.0")
					{
						endp = new IPEndPoint(IPAddress.Broadcast, ipPort);
						num = 0;
					}
					else
					{
						endp = new IPEndPoint(IPAddress.Parse(ipAddr), ipPort);
						num = 1;
					}
				}
				catch
				{
				}
			}
			if (wgTools.bUDPOnly64 > 0)
			{
				wgUdpComm.timeourMsInternet = 2000;
				num = 2;
			}
			return num;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0010CA90 File Offset: 0x0010BA90
		public byte[] GetPacket4get()
		{
			byte[] array = null;
			if (this.UDPQueue4get.Count <= 0)
			{
				return array;
			}
			byte[] array2;
			lock (this.UDPQueue4get.SyncRoot)
			{
				array2 = (byte[])this.UDPQueue4get.Dequeue();
			}
			return array2;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0010CAEC File Offset: 0x0010BAEC
		public static byte[] getRemoteOpenPacket()
		{
			if (wgUdpServer.UDPQueue4Remoteopen.Count <= 0)
			{
				return null;
			}
			byte[] array;
			lock (wgUdpServer.UDPQueue4Remoteopen.SyncRoot)
			{
				array = (byte[])wgUdpServer.UDPQueue4Remoteopen.Dequeue();
			}
			return array;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0010CB44 File Offset: 0x0010BB44
		public ControllerRunInformation GetRunInfo(int controllerSN)
		{
			if (this.watchedControllers.ContainsKey((uint)controllerSN))
			{
				return this.watchedControllers[(uint)controllerSN].runinfo;
			}
			return null;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0010CB68 File Offset: 0x0010BB68
		public static byte[] getSetDoorControlPacket()
		{
			if (wgUdpServer.UDPQueue4SetDoorControl.Count <= 0)
			{
				return null;
			}
			byte[] array;
			lock (wgUdpServer.UDPQueue4SetDoorControl.SyncRoot)
			{
				array = (byte[])wgUdpServer.UDPQueue4SetDoorControl.Dequeue();
			}
			return array;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0010CBC0 File Offset: 0x0010BBC0
		public int getUDPQueue4Multithread(uint sn)
		{
			int num = -1;
			for (int i = 0; i < this.arrSN4Multithread.Length; i++)
			{
				if (this.arrSN4Multithread[i] == 0U)
				{
					if (this.arrUDPQueue4Multithread[i] == null)
					{
						this.arrUDPQueue4Multithread[i] = new Queue();
					}
					lock (this.arrUDPQueue4Multithread[i].SyncRoot)
					{
						num = i;
						this.arrSN4Multithread[i] = sn;
						this.arrSNStartTime4Multithread[i] = DateTime.Now;
						this.arrUDPQueue4Multithread[i].Clear();
					}
					if (this.workingCnt <= i)
					{
						this.workingCnt = i + 1;
					}
					return num;
				}
			}
			return num;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0010CC7C File Offset: 0x0010BC7C
		public long getXidOfCommand(byte[] cmd)
		{
			long num = -1L;
			try
			{
				if (cmd.Length == 64)
				{
					num = (long)((ulong)cmd[43]);
					num <<= 24;
					return num + (long)((int)cmd[40] | ((int)cmd[41] << 8) | ((int)cmd[42] << 16));
				}
				if (cmd.Length >= WGPacket.MinSize)
				{
					num = (long)((ulong)cmd[7]);
					num <<= 24;
					num += (long)((int)cmd[4] | ((int)cmd[5] << 8) | ((int)cmd[6] << 16));
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0010CCF8 File Offset: 0x0010BCF8
		private void listenStartRun()
		{
			this.listenStarted = true;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0010CD01 File Offset: 0x0010BD01
		private void RaiseEvNewRecord(string info)
		{
			if (this.evNewRecords != null)
			{
				this.evNewRecords(info);
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0010CD17 File Offset: 0x0010BD17
		public void removeUDPQueue4Multithread(int index, uint sn)
		{
			if (index >= 0 && this.arrSN4Multithread[index] == sn)
			{
				this.arrSN4Multithread[index] = 0U;
				this.arrSNStartTime4Multithread[index] = DateTime.Now;
				this.arrUDPQueue4Multithread[index] = null;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0010CD50 File Offset: 0x0010BD50
		public int udp_get(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			this.bUdpGetting = true;
			try
			{
				IPEndPoint ipendPoint = null;
				int ipendByIPAddr = wgUdpServer.GetIPEndByIPAddr(ipAddr, ipPort, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				else if (wgTools.bUDPCloud > 0)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.UDPQueue4get.SyncRoot)
				{
					this.UDPQueue4get.Clear();
				}
				if (ipendPoint != null)
				{
					EndPoint endPoint = ipendPoint;
					int num2 = 3;
					while (num2-- > 0)
					{
						this.UdpSocket.SendTo(cmd, endPoint);
						this.listenStartRun();
						long ticks = DateTime.Now.Ticks;
						long num3 = ticks + wgUdpServer.CommTimeoutMsMin * 1000L * 10L;
						if (wgUdpServer.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
						{
							num3 = ticks + (long)(parWaitMs * 1000 * 10);
						}
						if (ticks > num3)
						{
							return num;
						}
						long num4 = 0L;
						num3 += 300000L;
						while (num3 > DateTime.Now.Ticks)
						{
							if (this.UDPQueue4get.Count > 0)
							{
								byte[] array;
								lock (this.UDPQueue4get.SyncRoot)
								{
									array = (byte[])this.UDPQueue4get.Dequeue();
								}
								if (xid == 0U || (ulong)xid == (ulong)this.getXidOfCommand(array) || xid == 4294967295U)
								{
									int num5 = 1;
									if (!wgUdpComm.isValidRecvCommand(cmd, array))
									{
										num5 = 0;
									}
									if (num5 > 0)
									{
										recv = array;
										return 1;
									}
								}
							}
							else if (ticks + 10000L <= DateTime.Now.Ticks)
							{
								if (num4 > 10L)
								{
									Thread.Sleep(30);
								}
								else
								{
									num4 += 1L;
									Thread.Sleep(1);
									if (wgTools.bUDPCloud > 0)
									{
										Thread.Sleep(10);
									}
								}
							}
						}
						wgTools.WgDebugWrite(string.Format("tries = {0:d} cmd={1}", 3 - num2, BitConverter.ToString(cmd)), new object[0]);
						if (xid == 4294967295U)
						{
							return 1;
						}
						wgUdpServer.triesTotal += 1L;
					}
				}
			}
			catch (Exception)
			{
			}
			this.bUdpGetting = false;
			return num;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0010CFDC File Offset: 0x0010BFDC
		public int udp_get(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv, int UDPQueue4MultithreadIndex)
		{
			if (UDPQueue4MultithreadIndex < 0)
			{
				return this.udp_get(cmd, parWaitMs, xid, ipAddr, ipPort, ref recv);
			}
			int num = -13;
			recv = null;
			this.bUdpGetting = true;
			try
			{
				IPEndPoint ipendPoint = null;
				int ipendByIPAddr = wgUdpServer.GetIPEndByIPAddr(ipAddr, ipPort, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				else if (wgTools.bUDPCloud > 0)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].SyncRoot)
				{
					this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Clear();
				}
				if (ipendPoint != null)
				{
					EndPoint endPoint = ipendPoint;
					int num2 = 3;
					while (num2-- > 0)
					{
						this.UdpSocket.SendTo(cmd, endPoint);
						this.listenStartRun();
						long ticks = DateTime.Now.Ticks;
						long num3 = ticks + wgUdpServer.CommTimeoutMsMin * 1000L * 10L;
						if (wgUdpServer.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
						{
							num3 = ticks + (long)(parWaitMs * 1000 * 10);
						}
						if (ticks > num3)
						{
							return num;
						}
						long num4 = 0L;
						num3 += 300000L;
						while (num3 > DateTime.Now.Ticks || this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Count > 0)
						{
							if (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Count > 0)
							{
								byte[] array;
								lock (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].SyncRoot)
								{
									array = (byte[])this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Dequeue();
								}
								if (xid == 0U || (ulong)xid == (ulong)this.getXidOfCommand(array) || xid == 4294967295U)
								{
									wgTools.WgDebugWrite(string.Format("?????this.arrUDPQueue4Multithread[{0}] = ", UDPQueue4MultithreadIndex) + BitConverter.ToString(array), new object[0]);
									int num5 = 1;
									if (!wgUdpComm.isValidRecvCommand(cmd, array))
									{
										num5 = 0;
									}
									if (num5 > 0)
									{
										recv = array;
										return 1;
									}
								}
							}
							else if (ticks + 10000L <= DateTime.Now.Ticks)
							{
								if (num4 > 10L)
								{
									Thread.Sleep(30);
								}
								else
								{
									num4 += 1L;
									Thread.Sleep(1);
								}
							}
						}
						wgTools.WriteLine(string.Format("tries = {0:d} cmd={1}", 3 - num2, BitConverter.ToString(cmd)));
						if (xid == 4294967295U)
						{
							return 1;
						}
						wgUdpServer.triesTotal += 1L;
					}
				}
			}
			catch (Exception)
			{
			}
			this.bUdpGetting = false;
			return num;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0010D2B8 File Offset: 0x0010C2B8
		public int udp_get_notries(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			this.bUdpGetting = true;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				wgTools.UDPCloudGet(cmd, ref text, ref num2);
				int ipendByIPAddr = wgUdpServer.GetIPEndByIPAddr(text, num2, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.UDPQueue4get.SyncRoot)
				{
					this.UDPQueue4get.Clear();
				}
				if (ipendPoint != null)
				{
					EndPoint endPoint = ipendPoint;
					int num3 = 3;
					if (num3-- > 0)
					{
						this.UdpSocket.SendTo(cmd, endPoint);
						this.listenStartRun();
						long ticks = DateTime.Now.Ticks;
						long num4 = ticks + wgUdpServer.CommTimeoutMsMin * 1000L * 10L;
						if (wgUdpServer.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
						{
							num4 = ticks + (long)(parWaitMs * 1000 * 10);
						}
						if (ticks > num4)
						{
							return num;
						}
						long num5 = 0L;
						num4 += 300000L;
						while (num4 > DateTime.Now.Ticks)
						{
							if (this.UDPQueue4get.Count > 0)
							{
								byte[] array;
								lock (this.UDPQueue4get.SyncRoot)
								{
									array = (byte[])this.UDPQueue4get.Dequeue();
								}
								if (xid == 0U || (ulong)xid == (ulong)this.getXidOfCommand(array) || xid == 4294967295U)
								{
									int num6 = 1;
									if (!wgUdpComm.isValidRecvCommand(cmd, array))
									{
										num6 = 0;
									}
									if (num6 > 0)
									{
										recv = array;
										return 1;
									}
								}
							}
							else if (ticks + 10000L <= DateTime.Now.Ticks)
							{
								if (num5 > 10L)
								{
									Thread.Sleep(30);
								}
								else
								{
									num5 += 1L;
									Thread.Sleep(1);
								}
							}
						}
						wgTools.WriteLine(string.Format("tries = {0:d} cmd={1}", 3 - num3, BitConverter.ToString(cmd)));
						if (xid == 2147483647U)
						{
							return 1;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			this.bUdpGetting = false;
			return num;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0010D51C File Offset: 0x0010C51C
		public int udp_get_notries(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv, int UDPQueue4MultithreadIndex = -1)
		{
			if (UDPQueue4MultithreadIndex < 0)
			{
				return this.udp_get_notries(cmd, parWaitMs, xid, ipAddr, ipPort, ref recv);
			}
			int num = -13;
			recv = null;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				wgTools.UDPCloudGet(cmd, ref text, ref num2);
				int ipendByIPAddr = wgUdpServer.GetIPEndByIPAddr(text, num2, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].SyncRoot)
				{
					this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Clear();
				}
				if (ipendPoint == null)
				{
					return num;
				}
				EndPoint endPoint = ipendPoint;
				int num3 = 3;
				if (num3-- > 0)
				{
					this.UdpSocket.SendTo(cmd, endPoint);
					this.listenStartRun();
					long ticks = DateTime.Now.Ticks;
					long num4 = ticks + wgUdpServer.CommTimeoutMsMin * 1000L * 10L;
					if (wgUdpServer.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
					{
						num4 = ticks + (long)(parWaitMs * 1000 * 10);
					}
					if (ticks > num4)
					{
						return num;
					}
					long num5 = 0L;
					num4 += 300000L;
					while (num4 > DateTime.Now.Ticks)
					{
						if (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Count > 0)
						{
							byte[] array;
							lock (this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].SyncRoot)
							{
								array = (byte[])this.arrUDPQueue4Multithread[UDPQueue4MultithreadIndex].Dequeue();
							}
							if (xid == 0U || (ulong)xid == (ulong)this.getXidOfCommand(array) || xid == 4294967295U)
							{
								int num6 = 1;
								if (!wgUdpComm.isValidRecvCommand(cmd, array))
								{
									num6 = 0;
								}
								if (num6 > 0)
								{
									recv = array;
									return 1;
								}
							}
						}
						else if (ticks + 10000L <= DateTime.Now.Ticks)
						{
							if (num5 > 10L)
							{
								Thread.Sleep(30);
							}
							else
							{
								num5 += 1L;
								Thread.Sleep(1);
							}
						}
					}
					wgTools.WriteLine(string.Format("tries = {0:d} cmd={1}", 3 - num3, BitConverter.ToString(cmd)));
					if (xid != 2147483647U)
					{
						return num;
					}
					return 1;
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0010D7A0 File Offset: 0x0010C7A0
		public int udp_get_onlySend(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			this.bUdpGetting = true;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				wgTools.UDPCloudGet(cmd, ref text, ref num2);
				int ipendByIPAddr = wgUdpServer.GetIPEndByIPAddr(text, num2, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				if (ipendPoint != null)
				{
					EndPoint endPoint = ipendPoint;
					this.UdpSocket.SendTo(cmd, endPoint);
					this.listenStartRun();
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0010D828 File Offset: 0x0010C828
		public int UDP_OnlySend(byte[] cmd, int parWaitMs, string ipAddr, int ipPort)
		{
			int num = -13;
			try
			{
				IPEndPoint ipendPoint = null;
				if (this.endp4broadcast == null)
				{
					this.endp4broadcast = new IPEndPoint(IPAddress.Broadcast, 60000);
				}
				if (wgTools.bUDPCloud > 0)
				{
					string text = ipAddr;
					int num2 = ipPort;
					wgTools.UDPCloudGet(cmd, ref text, ref num2);
					wgUdpComm.GetIPEndByIPAddr(text, num2, ref ipendPoint);
					int num3 = parWaitMs + wgUdpComm.timeourMsInternet;
				}
				else if (string.IsNullOrEmpty(ipAddr))
				{
					ipendPoint = this.endp4broadcast;
				}
				else
				{
					int ipendByIPAddr = wgUdpComm.GetIPEndByIPAddr(ipAddr, ipPort, ref ipendPoint);
					if (ipendByIPAddr < 0)
					{
						return num;
					}
					if (ipendByIPAddr == 2)
					{
						int num3 = parWaitMs + wgUdpComm.timeourMsInternet;
					}
				}
				if (ipendPoint == null)
				{
					return num;
				}
				this.cmdtemp = new byte[cmd.Length];
				Array.Copy(cmd, this.cmdtemp, cmd.Length);
				EndPoint endPoint = ipendPoint;
				try
				{
					if (this.UdpSocket == null)
					{
						Thread.Sleep(20);
					}
					if (this.UdpSocket != null)
					{
						this.UdpSocket.SendTo(cmd, endPoint);
						this.listenStartRun();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite("ex2.ToString()=" + ex.ToString() + "\r\n lastSendIP = " + string.Format("{0}:{1}", ipAddr, ipPort), new object[0]);
					Thread.Sleep(20);
					if (this.UdpSocket != null)
					{
						this.UdpSocket.SendTo(cmd, endPoint);
						this.listenStartRun();
					}
				}
				num = 1;
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0010D9C8 File Offset: 0x0010C9C8
		private void UDPListenProc()
		{
			try
			{
				if (this.UDPQueue == null)
				{
					this.UDPQueue = new Queue();
				}
				IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
				IPEndPoint ipendPoint2 = new IPEndPoint(IPAddress.Broadcast, 60000);
				byte[] array = new byte[12];
				array[0] = 13;
				array[1] = 13;
				byte[] array2 = array;
				EndPoint endPoint = ipendPoint;
				EndPoint endPoint2 = ipendPoint2;
				try
				{
					if (wgTools.bUDPCloud > 0)
					{
						EndPoint endPoint3 = new IPEndPoint(string.IsNullOrEmpty(wgTools.UDPCloudIP) ? IPAddress.Any : IPAddress.Parse(wgTools.UDPCloudIP), wgTools.UDPCloudPort);
						this.UdpSocket.Bind(endPoint3);
						wgTools.wgcloud = this;
						this.UdpSocket.SendTo(array2, endPoint2);
						this.listenStartRun();
						InfoRow infoRow = new InfoRow();
						if (wgTools.bUDPOnly64 > 0)
						{
							infoRow.desc = string.Format("{0}[{1},{2}]", string.Format(CommonStr.strCloudServer + "-P64", new object[0]), wgTools.p64_gprs_watchingSendCycle, wgTools.p64_gprs_refreshCycleMax);
						}
						else
						{
							infoRow.desc = string.Format(CommonStr.strCloudServer + "-1!", new object[0]);
						}
						infoRow.information = string.Format("{0} ={1}", "IP", endPoint3.ToString());
						wgAppConfig.wgLog(infoRow.desc + infoRow.information);
						wgRunInfoLog.addEvent(infoRow);
					}
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				int num = 0;
				byte[] array3 = new byte[1500];
				do
				{
					if (!this.listenStarted)
					{
						Thread.Sleep(1);
					}
					else
					{
						try
						{
							try
							{
								if (this.UdpSocket.Available < 0)
								{
									Thread.Sleep(10);
									goto IL_04C7;
								}
								num = this.UdpSocket.ReceiveFrom(array3, ref endPoint);
							}
							catch
							{
								if (this.UdpSocket.Available < 0)
								{
									Thread.Sleep(10);
									goto IL_04C7;
								}
								num = this.UdpSocket.ReceiveFrom(array3, ref endPoint);
							}
						}
						catch (SocketException ex2)
						{
							wgAppConfig.wgLog(ex2.ToString());
							this.UdpSocket = null;
							this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
							wgUdpComm.SockSpecialSet(ref this.UdpSocket);
							this.UdpSocket.EnableBroadcast = true;
							this.UdpSocket.ReceiveBufferSize = 16777216;
							this.listenStarted = false;
							try
							{
								if (wgTools.bUDPCloud > 0)
								{
									EndPoint endPoint4 = new IPEndPoint(string.IsNullOrEmpty(wgTools.UDPCloudIP) ? IPAddress.Any : IPAddress.Parse(wgTools.UDPCloudIP), wgTools.UDPCloudPort);
									this.UdpSocket.Bind(endPoint4);
									wgTools.wgcloud = this;
									this.UdpSocket.SendTo(array2, endPoint2);
									this.listenStartRun();
									InfoRow infoRow2 = new InfoRow();
									if (wgTools.bUDPOnly64 > 0)
									{
										infoRow2.desc = string.Format("{0}[{1},{2}]", string.Format(CommonStr.strCloudServer + "-P64", new object[0]), wgTools.p64_gprs_watchingSendCycle, wgTools.p64_gprs_refreshCycleMax);
									}
									else
									{
										infoRow2.desc = string.Format(CommonStr.strCloudServer + "-1!", new object[0]);
									}
									infoRow2.information = string.Format("{0} ={1}", "IP", endPoint4.ToString());
									wgRunInfoLog.addEvent(infoRow2);
								}
							}
							catch (Exception ex3)
							{
								wgAppConfig.wgLog(ex3.ToString());
							}
							goto IL_04C7;
						}
						catch (Exception ex4)
						{
							throw ex4;
						}
						byte[] array4 = new byte[num];
						Array.Copy(array3, 0, array4, 0, num);
						wgTools.WgDebugWrite("Array.Copy(bytReceiveBuffer,0, rcvbyt2, 0, bytLen)" + BitConverter.ToString(array4), new object[0]);
						if (WGPacket.Parsing4Watching(ref array4, ((IPEndPoint)endPoint).Port) >= 0)
						{
							if (array4.Length == 64)
							{
								uint num2 = BitConverter.ToUInt32(array4, 4);
								if (num2 > 100000000U && num2 < 500000000U)
								{
									wgTools.UDPCloudUpdate(num2, ((IPEndPoint)endPoint).Address.ToString(), ((IPEndPoint)endPoint).Port);
									this.dispatchToUDPQueue4Multithread(array4);
									this.dispatchToUDPQueue4Remoteopen(array4);
								}
							}
							else if (array4[0] == 32 && array4[1] == 33)
							{
								uint num2 = BitConverter.ToUInt32(array4, 8);
								if (num2 > 100000000U && num2 < 500000000U)
								{
									wgTools.UDPCloudUpdate(num2, ((IPEndPoint)endPoint).Address.ToString(), ((IPEndPoint)endPoint).Port);
								}
							}
							else
							{
								this.dispatchToUDPQueue4Multithread(array4);
							}
							lock (this.UDPQueue.SyncRoot)
							{
								this.UDPQueue.Enqueue(array4);
								this.lngReceiveCount += 1L;
							}
							if (this.bUdpGetting && this.UDPQueue4get.Count < 2000)
							{
								lock (this.UDPQueue4get.SyncRoot)
								{
									this.UDPQueue4get.Enqueue(array4);
								}
							}
						}
					}
					IL_04C7:;
				}
				while (!this.bUDPListenStop);
			}
			catch (Exception ex5)
			{
				wgTools.WgDebugWrite(ex5.ToString(), new object[0]);
			}
		}

		// Token: 0x04001C1B RID: 7195
		public ArrayList arrControllerConnected = new ArrayList();

		// Token: 0x04001C1C RID: 7196
		private ArrayList arrlstController = new ArrayList();

		// Token: 0x04001C1D RID: 7197
		private ArrayList arrlstLastRecordIndex = new ArrayList();

		// Token: 0x04001C1E RID: 7198
		public uint[] arrSN4Multithread;

		// Token: 0x04001C1F RID: 7199
		public DateTime[] arrSNStartTime4Multithread;

		// Token: 0x04001C20 RID: 7200
		public Queue[] arrUDPQueue4Multithread;

		// Token: 0x04001C21 RID: 7201
		private bool bUdpGetting;

		// Token: 0x04001C22 RID: 7202
		private bool bUDPListenStop;

		// Token: 0x04001C23 RID: 7203
		private byte[] cmdtemp = new byte[1052];

		// Token: 0x04001C24 RID: 7204
		public static long CommTimeoutMsMin = 300L;

		// Token: 0x04001C25 RID: 7205
		public Dictionary<int, icController> curNowWatching;

		// Token: 0x04001C26 RID: 7206
		private IPEndPoint endp4broadcast;

		// Token: 0x04001C27 RID: 7207
		private long iNewRecordsCnt;

		// Token: 0x04001C28 RID: 7208
		private bool listenStarted;

		// Token: 0x04001C29 RID: 7209
		private long lngReceiveCount;

		// Token: 0x04001C2A RID: 7210
		public static long triesTotal = 0L;

		// Token: 0x04001C2B RID: 7211
		private Thread UDPListenThread;

		// Token: 0x04001C2C RID: 7212
		private Thread DealRuninfoPacketThread;

		// Token: 0x04001C2D RID: 7213
		private Queue UDPQueue = new Queue();

		// Token: 0x04001C2E RID: 7214
		private Queue UDPQueue4get = new Queue();

		// Token: 0x04001C2F RID: 7215
		private static Queue UDPQueue4AdjustTime = new Queue();

		// Token: 0x04001C30 RID: 7216
		private static Queue UDPQueue4Remoteopen = new Queue();

		// Token: 0x04001C31 RID: 7217
		private static Queue UDPQueue4SetDoorControl = new Queue();

		// Token: 0x04001C32 RID: 7218
		private Socket UdpSocket;

		// Token: 0x04001C33 RID: 7219
		private Dictionary<uint, wgUdpServer.udpController> watchedControllers = new Dictionary<uint, wgUdpServer.udpController>();

		// Token: 0x04001C34 RID: 7220
		public int workingCnt;

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x06000F21 RID: 3873
		public delegate void newRecordHandler(string info);

		// Token: 0x02000217 RID: 535
		private class udpController
		{
			// Token: 0x06000F24 RID: 3876 RVA: 0x0010DFB6 File Offset: 0x0010CFB6
			public udpController(uint SN)
			{
				this.ControllerSN = SN;
				this.runinfo = new ControllerRunInformation();
			}

			// Token: 0x04001C36 RID: 7222
			public uint ControllerSN;

			// Token: 0x04001C37 RID: 7223
			public bool isConnected;

			// Token: 0x04001C38 RID: 7224
			public bool isFirstComm = true;

			// Token: 0x04001C39 RID: 7225
			public uint lastRecordIndex = uint.MaxValue;

			// Token: 0x04001C3A RID: 7226
			public string lastStringRaw;

			// Token: 0x04001C3B RID: 7227
			public ControllerRunInformation runinfo;
		}
	}
}
