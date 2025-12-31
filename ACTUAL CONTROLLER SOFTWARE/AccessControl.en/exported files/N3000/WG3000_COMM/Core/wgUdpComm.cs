using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WG3000_COMM.Core
{
	// Token: 0x02000214 RID: 532
	public class wgUdpComm : IDisposable
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x0010AC28 File Offset: 0x00109C28
		public wgUdpComm()
		{
			this.UDPQueue = new Queue();
			this.UDPQueue4MultithreadIndex = -1;
			try
			{
				this.m_localIP = wgUdpComm.m_defaultNetworkIP;
				this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				wgUdpComm.SockSpecialSet(ref this.UdpSocket);
				this.UdpSocket.EnableBroadcast = true;
				this.UdpSocket.ReceiveBufferSize = 16777216;
				this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
				this.UDPListenThread.IsBackground = true;
				this.UDPListenThread.Start();
				Thread.Sleep(10);
				wgTools.WgDebugWrite("wgUdpComm()", new object[0]);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0010ACE8 File Offset: 0x00109CE8
		public wgUdpComm(int UDPQueue4MultithreadIndexParam)
		{
			this.UDPQueue = new Queue();
			this.UDPQueue4MultithreadIndex = -1;
			try
			{
				if (UDPQueue4MultithreadIndexParam >= 0)
				{
					this.UDPQueue4MultithreadIndex = UDPQueue4MultithreadIndexParam;
					wgTools.WgDebugWrite("wgUdpComm(int UDPQueue4MultithreadIndexParam)====1", new object[0]);
				}
				else if (wgTools.bUDPCloud > 0)
				{
					wgTools.WgDebugWrite("wgUdpComm(int UDPQueue4MultithreadIndexParam)=====2", new object[0]);
				}
				else
				{
					this.m_localIP = wgUdpComm.m_defaultNetworkIP;
					this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					wgUdpComm.SockSpecialSet(ref this.UdpSocket);
					this.UdpSocket.EnableBroadcast = true;
					this.UdpSocket.ReceiveBufferSize = 16777216;
					this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
					this.UDPListenThread.IsBackground = true;
					this.UDPListenThread.Start();
					Thread.Sleep(10);
					wgTools.WgDebugWrite("wgUdpComm(int UDPQueue4MultithreadIndexParam)", new object[0]);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0010ADE8 File Offset: 0x00109DE8
		public wgUdpComm(IPAddress localIP)
		{
			this.UDPQueue = new Queue();
			this.UDPQueue4MultithreadIndex = -1;
			try
			{
				this.m_localIP = localIP;
				this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				wgUdpComm.SockSpecialSet(ref this.UdpSocket);
				this.UdpSocket.EnableBroadcast = true;
				this.UdpSocket.ReceiveBufferSize = 16777216;
				this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
				this.UDPListenThread.IsBackground = true;
				this.UDPListenThread.Start();
				Thread.Sleep(10);
				wgTools.WgDebugWrite("wgUdpComm(IPAddress localIP)", new object[0]);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0010AEA4 File Offset: 0x00109EA4
		public bool ClearAllPacket()
		{
			lock (this.UDPQueue.SyncRoot)
			{
				this.UDPQueue.Clear();
			}
			return true;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0010AEE8 File Offset: 0x00109EE8
		public bool Close()
		{
			try
			{
				this.bUDPListenStop = true;
				if (this.UDPListenThread != null)
				{
					this.UDPListenThread.Abort();
				}
				if (this.UDPQueue != null)
				{
					this.UDPQueue.Clear();
					this.UDPQueue = null;
				}
				if (this.UdpSocket != null)
				{
					this.UdpSocket.Close();
				}
			}
			catch
			{
				throw;
			}
			return true;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0010AF54 File Offset: 0x00109F54
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0010AF63 File Offset: 0x00109F63
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.UdpSocket != null)
			{
				this.UdpSocket.Close();
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0010AF7C File Offset: 0x00109F7C
		public static int GetIPEndByIPAddr(string ipAddr, int ipPort, ref IPEndPoint endp)
		{
			int num = -1;
			if (ipAddr == null)
			{
				endp = new IPEndPoint(IPAddress.Broadcast, 60000);
				return 0;
			}
			if (ipAddr.Length == 0)
			{
				endp = new IPEndPoint(IPAddress.Broadcast, 60000);
				return 0;
			}
			try
			{
				if (ipAddr == "192.0.0.0" || ipAddr == "192.168.0.0" || ipAddr == "192.168.168.0")
				{
					endp = new IPEndPoint(IPAddress.Broadcast, ipPort);
					return 0;
				}
				endp = new IPEndPoint(IPAddress.Parse(ipAddr), ipPort);
				num = 1;
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0010B01C File Offset: 0x0010A01C
		public byte[] GetPacket()
		{
			byte[] array = null;
			if (this.UDPQueue.Count <= 0)
			{
				return array;
			}
			byte[] array2;
			lock (this.UDPQueue.SyncRoot)
			{
				array2 = (byte[])this.UDPQueue.Dequeue();
			}
			return array2;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0010B078 File Offset: 0x0010A078
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

		// Token: 0x06000EEE RID: 3822 RVA: 0x0010B0F4 File Offset: 0x0010A0F4
		private static bool isCryptedCommand(byte[] cmd)
		{
			return (cmd[0] & 128) > 0 || (cmd.Length == 64 && cmd[0] == 18) || (cmd.Length == 64 && cmd[0] == 20) || (cmd.Length == 64 && cmd[0] == 22) || (cmd.Length == 64 && cmd[0] == 24) || (cmd.Length == 64 && cmd[0] == 26);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0010B160 File Offset: 0x0010A160
		public static bool isValidRecvCommand(byte[] cmd, byte[] bytget)
		{
			if (wgUdpComm.isCryptedCommand(cmd))
			{
				return true;
			}
			if ((cmd.Length == 64 && cmd[0] == 17) || (cmd.Length == 64 && cmd[0] == 19) || (cmd.Length == 64 && cmd[0] == 21) || (cmd.Length == 64 && cmd[0] == 23) || (cmd.Length == 64 && cmd[0] == 25) || cmd.Length == 1024)
			{
				long num = 0L;
				num += (long)((ulong)((int)bytget[4] + ((int)bytget[4] << 8) + ((int)bytget[4] << 16) + ((int)bytget[4] << 24)));
				long num2 = 0L;
				num2 += (long)((ulong)((int)cmd[4] + ((int)cmd[4] << 8) + ((int)cmd[4] << 16) + ((int)cmd[4] << 24)));
				if (num2 != 0L && num2 != (long)((ulong)(-1)) && num2 != num)
				{
					return false;
				}
			}
			else if ((cmd[0] & 128) == 0)
			{
				long num3 = 0L;
				num3 += (long)((ulong)((int)bytget[8] + ((int)bytget[9] << 8) + ((int)bytget[10] << 16) + ((int)bytget[11] << 24)));
				long num4 = 0L;
				num4 += (long)((ulong)((int)cmd[12] + ((int)cmd[13] << 8) + ((int)cmd[14] << 16) + ((int)cmd[15] << 24)));
				if (num4 != 0L && num4 != (long)((ulong)(-1)))
				{
					if (num4 != num3)
					{
						return false;
					}
					if (wgTools.gCRC19 > 0 && (wgTools.gCRC19 & 240) != (int)(bytget[19] & 240) && (bytget[0] != 35 || bytget[1] != 49))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0010B2AD File Offset: 0x0010A2AD
		private void listenStartRun()
		{
			this.listenStarted = true;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0010B2B8 File Offset: 0x0010A2B8
		public static void SockSpecialSet(ref Socket udpSock)
		{
			uint num = 2147483648U;
			uint num2 = 402653184U;
			uint num3 = num | num2 | 12U;
			try
			{
				udpSock.IOControl((int)num3, new byte[] { Convert.ToByte(false) }, null);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString());
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0010B318 File Offset: 0x0010A318
		public void startListen()
		{
			try
			{
				this.m_localIP = wgUdpComm.m_defaultNetworkIP;
				this.UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				wgUdpComm.SockSpecialSet(ref this.UdpSocket);
				this.UdpSocket.EnableBroadcast = true;
				this.UdpSocket.ReceiveBufferSize = 16777216;
				this.UDPListenThread = new Thread(new ThreadStart(this.UDPListenProc));
				this.UDPListenThread.IsBackground = true;
				this.UDPListenThread.Start();
				Thread.Sleep(10);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0010B3B0 File Offset: 0x0010A3B0
		public int udp_get(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				if (wgTools.UDPCloudGet(cmd, ref text, ref num2) > 0)
				{
					return wgTools.wgcloud.udp_get(cmd, parWaitMs, xid, text, num2, ref recv, this.UDPQueue4MultithreadIndex);
				}
				if (this.UDPQueue4MultithreadIndex >= 0 && !this.listenStarted)
				{
					this.startListen();
				}
				if (this.UdpSocket == null)
				{
					this.startListen();
				}
				int ipendByIPAddr = wgUdpComm.GetIPEndByIPAddr(text, num2, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.UDPQueue.SyncRoot)
				{
					this.UDPQueue.Clear();
				}
				if (ipendPoint == null)
				{
					return num;
				}
				EndPoint endPoint = ipendPoint;
				int num3 = 3;
				while (num3-- > 0)
				{
					this.UdpSocket.SendTo(cmd, endPoint);
					this.listenStartRun();
					long ticks = DateTime.Now.Ticks;
					long num4 = ticks + wgUdpComm.CommTimeoutMsMin * 1000L * 10L;
					if (wgUdpComm.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
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
						if (this.UDPQueue.Count > 0)
						{
							byte[] array;
							lock (this.UDPQueue.SyncRoot)
							{
								array = (byte[])this.UDPQueue.Dequeue();
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
					if (xid == 4294967295U)
					{
						return 1;
					}
					wgUdpComm.triesTotal += 1L;
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0010B658 File Offset: 0x0010A658
		public int udp_get_notries(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				if (wgTools.UDPCloudGet(cmd, ref text, ref num2) > 0)
				{
					return wgTools.wgcloud.udp_get_notries(cmd, parWaitMs, xid, text, num2, ref recv);
				}
				int ipendByIPAddr = wgUdpComm.GetIPEndByIPAddr(text, num2, ref ipendPoint);
				if (ipendByIPAddr < 0)
				{
					return num;
				}
				if (ipendByIPAddr == 2)
				{
					parWaitMs += wgUdpComm.timeourMsInternet;
				}
				lock (this.UDPQueue.SyncRoot)
				{
					this.UDPQueue.Clear();
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
					long num4 = ticks + wgUdpComm.CommTimeoutMsMin * 1000L * 10L;
					if (wgUdpComm.CommTimeoutMsMin < (long)parWaitMs && parWaitMs > 300 && parWaitMs < 10000)
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
						if (this.UDPQueue.Count > 0)
						{
							byte[] array;
							lock (this.UDPQueue.SyncRoot)
							{
								array = (byte[])this.UDPQueue.Dequeue();
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

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0010B8D4 File Offset: 0x0010A8D4
		public int udp_get_onlySend(byte[] cmd, int parWaitMs, uint xid, string ipAddr, int ipPort, ref byte[] recv)
		{
			int num = -13;
			recv = null;
			try
			{
				IPEndPoint ipendPoint = null;
				string text = ipAddr;
				int num2 = ipPort;
				if (wgTools.UDPCloudGet(cmd, ref text, ref num2) > 0)
				{
					return wgTools.wgcloud.udp_get_onlySend(cmd, parWaitMs, xid, text, num2, ref recv);
				}
				int ipendByIPAddr = wgUdpComm.GetIPEndByIPAddr(text, num2, ref ipendPoint);
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

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0010B96C File Offset: 0x0010A96C
		private void UDPListenProc()
		{
			try
			{
				if (this.UDPQueue == null)
				{
					this.UDPQueue = new Queue();
				}
				IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
				if (this.m_localIP != null)
				{
					ipendPoint = new IPEndPoint(this.m_localIP, 0);
				}
				new IPEndPoint(IPAddress.Broadcast, 60000);
				EndPoint endPoint = ipendPoint;
				if (this.m_localIP != null)
				{
					this.UdpSocket.Bind(endPoint);
				}
				byte[] array = new byte[1500];
				do
				{
					if (!this.listenStarted)
					{
						Thread.Sleep(1);
					}
					else
					{
						int num;
						try
						{
							if (this.UdpSocket.Available < 0)
							{
								Thread.Sleep(10);
								goto IL_013A;
							}
							num = this.UdpSocket.ReceiveFrom(array, ref endPoint);
						}
						catch
						{
							if (this.UdpSocket.Available < 0)
							{
								Thread.Sleep(10);
								goto IL_013A;
							}
							num = this.UdpSocket.ReceiveFrom(array, ref endPoint);
						}
						byte[] array2 = new byte[num];
						Array.Copy(array, 0, array2, 0, num);
						this.m_receivedpaketTotal++;
						if (WGPacket.Parsing(ref array2, ((IPEndPoint)endPoint).Port) >= 0)
						{
							this.m_receivedpaketValidTotal++;
							lock (this.UDPQueue.SyncRoot)
							{
								this.UDPQueue.Enqueue(array2);
							}
						}
					}
					IL_013A:;
				}
				while (!this.bUDPListenStop);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0010BB10 File Offset: 0x0010AB10
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x0010BB18 File Offset: 0x0010AB18
		public static IPAddress defaultNetworkIP
		{
			get
			{
				return wgUdpComm.m_defaultNetworkIP;
			}
			set
			{
				try
				{
					wgUdpComm.m_defaultNetworkIP = value;
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0010BB40 File Offset: 0x0010AB40
		public IPAddress localIP
		{
			get
			{
				return this.m_localIP;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x0010BB48 File Offset: 0x0010AB48
		public int PacketCount
		{
			get
			{
				return this.UDPQueue.Count;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0010BB55 File Offset: 0x0010AB55
		public int ReceivedPacketTotal
		{
			get
			{
				return this.m_receivedpaketTotal;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x0010BB5D File Offset: 0x0010AB5D
		public int ReceivedPacketValidTotal
		{
			get
			{
				return this.m_receivedpaketValidTotal;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0010BB68 File Offset: 0x0010AB68
		public ushort udpPort
		{
			get
			{
				try
				{
					if (this.UdpSocket != null && this.UdpSocket.LocalEndPoint != null)
					{
						return (ushort)((IPEndPoint)this.UdpSocket.LocalEndPoint).Port;
					}
				}
				catch
				{
				}
				return 0;
			}
		}

		// Token: 0x04001C0D RID: 7181
		public static bool bWIFI = false;

		// Token: 0x04001C0E RID: 7182
		public static long CommTimeoutMsMin = 300L;

		// Token: 0x04001C0F RID: 7183
		public static int timeourMsInternet = 500;

		// Token: 0x04001C10 RID: 7184
		public static long triesTotal = 0L;

		// Token: 0x04001C11 RID: 7185
		private static IPAddress m_defaultNetworkIP;

		// Token: 0x04001C12 RID: 7186
		private Socket UdpSocket;

		// Token: 0x04001C13 RID: 7187
		private Thread UDPListenThread;

		// Token: 0x04001C14 RID: 7188
		private Queue UDPQueue;

		// Token: 0x04001C15 RID: 7189
		private bool listenStarted;

		// Token: 0x04001C16 RID: 7190
		private bool bUDPListenStop;

		// Token: 0x04001C17 RID: 7191
		private IPAddress m_localIP;

		// Token: 0x04001C18 RID: 7192
		private int m_receivedpaketTotal;

		// Token: 0x04001C19 RID: 7193
		private int m_receivedpaketValidTotal;

		// Token: 0x04001C1A RID: 7194
		public int UDPQueue4MultithreadIndex;
	}
}
