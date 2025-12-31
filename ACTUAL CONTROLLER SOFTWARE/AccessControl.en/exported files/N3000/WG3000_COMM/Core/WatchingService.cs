using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using WG3000_COMM.DataOper;

namespace WG3000_COMM.Core
{
	// Token: 0x020001E9 RID: 489
	public class WatchingService : MarshalByRefObject, IDisposable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000B93 RID: 2963 RVA: 0x000F0330 File Offset: 0x000EF330
		// (remove) Token: 0x06000B94 RID: 2964 RVA: 0x000F0368 File Offset: 0x000EF368
		public event OnEventHandler EventHandler;

		// Token: 0x06000B95 RID: 2965 RVA: 0x000F03A0 File Offset: 0x000EF3A0
		public WatchingService()
		{
			new Thread(new ThreadStart(this.WatchController))
			{
				Name = "Watching Service"
			}.Start();
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000F03EC File Offset: 0x000EF3EC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000F03FB File Offset: 0x000EF3FB
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.udpserver != null)
			{
				this.udpserver.Close();
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x000F0414 File Offset: 0x000EF414
		public ControllerRunInformation GetRunInfo(int ControllerSN)
		{
			return this.udpserver.GetRunInfo(ControllerSN);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x000F0422 File Offset: 0x000EF422
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x000F0428 File Offset: 0x000EF428
		private void PublishEvent(string message)
		{
			wgTools.WgDebugWrite("Publishing \"{0}\"...", new object[] { message });
			if (this.EventHandler != null)
			{
				this.EventHandler(message);
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000F045F File Offset: 0x000EF45F
		public void StopWatch()
		{
			if (wgTools.bUDPOnly64 <= 0)
			{
				Interlocked.Exchange(ref this.m_bStopWatch, 1);
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000F0476 File Offset: 0x000EF476
		public void StopWatchByForce()
		{
			Interlocked.Exchange(ref this.m_bStopWatch, 1);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000F0488 File Offset: 0x000EF488
		private void udpserver_evNewRecord(string info)
		{
			if (this.EventHandler != null)
			{
				OnEventHandler onEventHandler = null;
				int num = 1;
				foreach (Delegate @delegate in this.EventHandler.GetInvocationList())
				{
					try
					{
						onEventHandler = (OnEventHandler)@delegate;
						onEventHandler(info);
					}
					catch (Exception ex)
					{
						wgTools.WriteLine(ex.ToString());
						wgTools.WgDebugWrite("事件订阅者" + num.ToString() + "发生错误,系统将取消事件订阅!", new object[0]);
						this.EventHandler -= onEventHandler;
					}
					num++;
				}
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x000F0524 File Offset: 0x000EF524
		private void WatchController()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.WatchController_P64();
				return;
			}
			wgTools.WgDebugWrite("watchController= {0:d}", new object[] { 111111111 });
			if (this.m_bHaveServer <= 0)
			{
				Interlocked.Increment(ref this.m_bHaveServer);
				this.udpserver = new wgUdpServer();
				WGPacketBasicRunInformation4ServerToSend wgpacketBasicRunInformation4ServerToSend = new WGPacketBasicRunInformation4ServerToSend();
				wgpacketBasicRunInformation4ServerToSend.type = 32;
				wgpacketBasicRunInformation4ServerToSend.code = 32;
				wgpacketBasicRunInformation4ServerToSend.iDevSnFrom = 0U;
				wgpacketBasicRunInformation4ServerToSend.iDevSnTo = 0U;
				wgpacketBasicRunInformation4ServerToSend.iCallReturn = 0;
				this.udpserver.evNewRecord += this.udpserver_evNewRecord;
				byte[] array = null;
				wgTools.WgDebugWrite("m_bStopWatch= {0:d}", new object[] { this.m_bStopWatch });
				DateTime now = DateTime.Now;
				int num = -1;
				bool flag = false;
				long num2 = 0L;
				ArrayList arrayList = new ArrayList();
				int num3 = 0;
				bool flag2 = true;
				ArrayList arrayList2 = new ArrayList();
				while (this.m_bStopWatch < 1)
				{
					if (num != this.updateCnt)
					{
						this.m_NowWatching = null;
						if (this.m_WantWatching != null)
						{
							Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_NowWatching, this.m_WantWatching);
							this.udpserver.curNowWatching = this.m_NowWatching;
							flag = false;
							arrayList2.Clear();
							foreach (KeyValuePair<int, icController> keyValuePair in this.m_NowWatching)
							{
								if (string.IsNullOrEmpty(keyValuePair.Value.IP))
								{
									flag = true;
									arrayList2.Add(keyValuePair.Value.ControllerSN);
								}
							}
						}
						Interlocked.Exchange(ref num, this.updateCnt);
					}
					else if (this.m_NowWatching == null)
					{
						Thread.Sleep(100);
					}
					else
					{
						lock (this.udpserver.arrControllerConnected)
						{
							this.udpserver.arrControllerConnected.Clear();
						}
						long ticks = DateTime.Now.Ticks;
						flag2 = true;
						if (this.m_NowWatching.Count <= 5 || !flag)
						{
							num2 = 0L;
							using (Dictionary<int, icController>.Enumerator enumerator2 = this.m_NowWatching.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<int, icController> keyValuePair2 = enumerator2.Current;
									if (string.IsNullOrEmpty(keyValuePair2.Value.IP) || arrayList.IndexOf(string.Format("{0}:{1}", keyValuePair2.Value.IP, keyValuePair2.Value.PORT)) < 0)
									{
										wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair2.Value.ControllerSN;
										if (array != null)
										{
											wgpacketBasicRunInformation4ServerToSend.GetNewXid();
										}
										array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
										DateTime dateTime = DateTime.Now;
										this.udpserver.UDP_OnlySend(array, 300, keyValuePair2.Value.IP, keyValuePair2.Value.PORT);
										DateTime dateTime2 = DateTime.Now;
										if (dateTime.AddSeconds(2.0) < dateTime2)
										{
											flag2 = false;
											arrayList.Add(string.Format("{0}:{1}", keyValuePair2.Value.IP, keyValuePair2.Value.PORT));
										}
										if (!flag)
										{
											Thread.Sleep(WatchingService.ip_packet_interval_ms);
										}
										else
										{
											Thread.Sleep(WatchingService.broadcast_packet_interval_ms);
										}
									}
								}
								goto IL_04F5;
							}
							goto IL_0349;
						}
						goto IL_0349;
						IL_04F5:
						if (flag2 && arrayList.Count > 0)
						{
							if (num3 >= arrayList.Count)
							{
								num3 = 0;
							}
							else
							{
								foreach (KeyValuePair<int, icController> keyValuePair3 in this.m_NowWatching)
								{
									if (!string.IsNullOrEmpty(keyValuePair3.Value.IP) && (string)arrayList[num3] == string.Format("{0}:{1}", keyValuePair3.Value.IP, keyValuePair3.Value.PORT))
									{
										wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair3.Value.ControllerSN;
										if (array != null)
										{
											wgpacketBasicRunInformation4ServerToSend.GetNewXid();
										}
										array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
										DateTime dateTime = DateTime.Now;
										this.udpserver.UDP_OnlySend(array, 300, keyValuePair3.Value.IP, keyValuePair3.Value.PORT);
										DateTime dateTime2 = DateTime.Now;
										if (dateTime.AddSeconds(2.0) < dateTime2)
										{
											num3++;
											break;
										}
										arrayList.RemoveAt(num3);
										break;
									}
								}
							}
						}
						long num4 = DateTime.Now.Ticks;
						this.m_lastGetInfoDateTime = DateTime.Now;
						if (num4 > ticks && num4 - ticks < (long)(WatchingService.m_watching_cycle_ms * 1000 * 10))
						{
							Thread.Sleep(400);
							int num5 = 2;
							while (this.udpserver.arrControllerConnected.Count < this.m_NowWatching.Count)
							{
								int num6 = 400;
								foreach (KeyValuePair<int, icController> keyValuePair4 in this.m_NowWatching)
								{
									if (this.udpserver.arrControllerConnected.IndexOf(keyValuePair4.Value.ControllerSN) < 0 && arrayList.IndexOf(string.Format("{0}:{1}", keyValuePair4.Value.IP, keyValuePair4.Value.PORT)) < 0)
									{
										wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair4.Value.ControllerSN;
										if (array != null)
										{
											wgpacketBasicRunInformation4ServerToSend.GetNewXid();
										}
										array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
										this.udpserver.UDP_OnlySend(array, 300, keyValuePair4.Value.IP, keyValuePair4.Value.PORT);
										if (string.IsNullOrEmpty(keyValuePair4.Value.IP))
										{
											Thread.Sleep(3);
										}
										else
										{
											Thread.Sleep(WatchingService.ip_packet_interval_ms);
										}
									}
									if (num4 - ticks >= (long)(WatchingService.m_watching_cycle_ms * 1000 * 10))
									{
										break;
									}
									num4 = DateTime.Now.Ticks;
								}
								if (num4 - ticks >= (long)((WatchingService.m_watching_cycle_ms - num6) * 1000 * 10))
								{
									break;
								}
								num4 = DateTime.Now.Ticks;
								num5--;
								if (num5 <= 0)
								{
									break;
								}
								Thread.Sleep(num6);
							}
							num4 = DateTime.Now.Ticks;
							Thread.Sleep(Math.Max(0, WatchingService.m_watching_cycle_ms - Math.Max(0, (int)(num4 - ticks) / 10000)));
							continue;
						}
						continue;
						IL_0349:
						wgpacketBasicRunInformation4ServerToSend.iDevSnTo = uint.MaxValue;
						if (array != null)
						{
							wgpacketBasicRunInformation4ServerToSend.GetNewXid();
						}
						array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
						this.udpserver.UDP_OnlySend(array, 300, "", 60000);
						Thread.Sleep(30);
						DateTime now2 = DateTime.Now;
						if ((long)(WatchingService.m_watching_cycle_ms * 10000) < now2.Ticks - num2)
						{
							num2 = now2.Ticks + 10L;
							foreach (KeyValuePair<int, icController> keyValuePair5 in this.m_NowWatching)
							{
								if (!string.IsNullOrEmpty(keyValuePair5.Value.IP) && (string.IsNullOrEmpty(keyValuePair5.Value.IP) || arrayList.IndexOf(string.Format("{0}:{1}", keyValuePair5.Value.IP, keyValuePair5.Value.PORT)) < 0))
								{
									wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair5.Value.ControllerSN;
									if (array != null)
									{
										wgpacketBasicRunInformation4ServerToSend.GetNewXid();
									}
									array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
									DateTime dateTime = DateTime.Now;
									this.udpserver.UDP_OnlySend(array, 300, keyValuePair5.Value.IP, keyValuePair5.Value.PORT);
									DateTime dateTime2 = DateTime.Now;
									if (dateTime.AddSeconds(2.0) < dateTime2)
									{
										flag2 = false;
										arrayList.Add(string.Format("{0}:{1}", keyValuePair5.Value.IP, keyValuePair5.Value.PORT));
									}
									Thread.Sleep(WatchingService.ip_packet_interval_ms);
								}
							}
							goto IL_04F5;
						}
						goto IL_04F5;
					}
				}
				wgTools.WgDebugWrite("udpserver.Dispose() ", new object[0]);
				this.udpserver.evNewRecord -= this.udpserver_evNewRecord;
				this.udpserver.Dispose();
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000F0E3C File Offset: 0x000EFE3C
		private void WatchController_P64()
		{
			wgTools.WgDebugWrite("watchController= {0:d}", new object[] { 111111111 });
			if (this.m_bHaveServer <= 0)
			{
				Interlocked.Increment(ref this.m_bHaveServer);
				this.udpserver = new wgUdpServer();
				this.udpserver.evNewRecord += this.udpserver_evNewRecord;
				wgTools.WgDebugWrite("m_bStopWatch= {0:d}", new object[] { this.m_bStopWatch });
				DateTime now = DateTime.Now;
				int num = -1;
				wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
				wgpacketShort.code = 32;
				ArrayList arrayList = new ArrayList();
				new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				while (this.m_bStopWatch < 1)
				{
					if (num != this.updateCnt)
					{
						this.m_NowWatching = null;
						if (this.m_WantWatching != null)
						{
							Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_NowWatching, this.m_WantWatching);
							this.udpserver.curNowWatching = this.m_NowWatching;
							DateTime dateTime = DateTime.Now.AddSeconds((double)(-(double)wgTools.p64_gprs_watchingSendCycle - 5));
							for (int i = 0; i < arrayList2.Count; i++)
							{
								arrayList2[i] = dateTime;
							}
						}
						Interlocked.Exchange(ref num, this.updateCnt);
					}
					else
					{
						lock (this.udpserver.arrControllerConnected)
						{
							this.udpserver.arrControllerConnected.Clear();
						}
						long ticks = DateTime.Now.Ticks;
						for (int j = 0; j < wgTools.arrSNReceived.Count; j++)
						{
							int num2 = (int)((uint)wgTools.arrSNReceived[j]);
							DateTime dateTime2 = wgTools.UDPCloudGetRefreshTime(num2);
							if (dateTime2 != DateTime.Parse("1970-1-1") && DateTime.Now <= dateTime2.AddSeconds((double)wgTools.p64_gprs_refreshCycleMax))
							{
								if (arrayList.IndexOf(num2) < 0)
								{
									arrayList.Add(num2);
									arrayList2.Add(DateTime.Now);
								}
								else
								{
									int num3 = arrayList.IndexOf(num2);
									if (num3 < 0)
									{
										goto IL_0288;
									}
									DateTime dateTime3 = (DateTime)arrayList2[num3];
									if (DateTime.Now < dateTime3.AddSeconds((double)wgTools.p64_gprs_watchingSendCycle))
									{
										goto IL_0288;
									}
									arrayList2[num3] = DateTime.Now;
								}
								wgpacketShort.iDevSn = (uint)num2;
								byte[] array = wgpacketShort.ToBytes();
								this.udpserver.UDP_OnlySend(array, 300, "", 60000);
								Thread.Sleep(WatchingService.ip_packet_interval_ms);
							}
							IL_0288:;
						}
						if (wgTools.bUDPOnly64 > 0)
						{
							Thread.Sleep(Math.Max(0, WatchingService.m_watching_cycle_ms - Math.Max(0, 0)));
						}
					}
				}
				wgTools.WgDebugWrite("udpserver.Dispose() ", new object[0]);
				this.udpserver.evNewRecord -= this.udpserver_evNewRecord;
				this.udpserver.Dispose();
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000F1158 File Offset: 0x000F0158
		private void WatchController_V5_21Before()
		{
			wgTools.WgDebugWrite("watchController= {0:d}", new object[] { 111111111 });
			if (this.m_bHaveServer <= 0)
			{
				Interlocked.Increment(ref this.m_bHaveServer);
				this.udpserver = new wgUdpServer();
				WGPacketBasicRunInformation4ServerToSend wgpacketBasicRunInformation4ServerToSend = new WGPacketBasicRunInformation4ServerToSend();
				wgpacketBasicRunInformation4ServerToSend.type = 32;
				wgpacketBasicRunInformation4ServerToSend.code = 32;
				wgpacketBasicRunInformation4ServerToSend.iDevSnFrom = 0U;
				wgpacketBasicRunInformation4ServerToSend.iDevSnTo = 0U;
				wgpacketBasicRunInformation4ServerToSend.iCallReturn = 0;
				this.udpserver.evNewRecord += this.udpserver_evNewRecord;
				byte[] array = null;
				wgTools.WgDebugWrite("m_bStopWatch= {0:d}", new object[] { this.m_bStopWatch });
				DateTime now = DateTime.Now;
				int num = -1;
				while (this.m_bStopWatch < 1)
				{
					if (num != this.updateCnt)
					{
						this.m_NowWatching = null;
						if (this.m_WantWatching != null)
						{
							Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_NowWatching, this.m_WantWatching);
						}
						Interlocked.Exchange(ref num, this.updateCnt);
					}
					else if (this.m_NowWatching == null)
					{
						Thread.Sleep(100);
					}
					else
					{
						long ticks = DateTime.Now.Ticks;
						foreach (KeyValuePair<int, icController> keyValuePair in this.m_NowWatching)
						{
							wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair.Value.ControllerSN;
							if (array != null)
							{
								wgpacketBasicRunInformation4ServerToSend.GetNewXid();
							}
							array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
							this.udpserver.UDP_OnlySend(array, 300, keyValuePair.Value.IP, keyValuePair.Value.PORT);
							Thread.Sleep(1);
						}
						long ticks2 = DateTime.Now.Ticks;
						this.m_lastGetInfoDateTime = DateTime.Now;
						if (ticks2 > ticks && ticks2 - ticks < (long)(WatchingService.m_watching_cycle_ms * 1000 * 10))
						{
							Thread.Sleep(WatchingService.m_watching_cycle_ms - Math.Max(0, (int)(ticks2 - ticks) / 10000));
						}
					}
				}
				this.udpserver.evNewRecord -= this.udpserver_evNewRecord;
				this.udpserver.Dispose();
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000F1390 File Offset: 0x000F0390
		private void WatchController_V7_53()
		{
			wgTools.WgDebugWrite("watchController= {0:d}", new object[] { 111111111 });
			if (this.m_bHaveServer <= 0)
			{
				Interlocked.Increment(ref this.m_bHaveServer);
				this.udpserver = new wgUdpServer();
				WGPacketBasicRunInformation4ServerToSend wgpacketBasicRunInformation4ServerToSend = new WGPacketBasicRunInformation4ServerToSend();
				wgpacketBasicRunInformation4ServerToSend.type = 32;
				wgpacketBasicRunInformation4ServerToSend.code = 32;
				wgpacketBasicRunInformation4ServerToSend.iDevSnFrom = 0U;
				wgpacketBasicRunInformation4ServerToSend.iDevSnTo = 0U;
				wgpacketBasicRunInformation4ServerToSend.iCallReturn = 0;
				this.udpserver.evNewRecord += this.udpserver_evNewRecord;
				byte[] array = null;
				wgTools.WgDebugWrite("m_bStopWatch= {0:d}", new object[] { this.m_bStopWatch });
				DateTime now = DateTime.Now;
				int num = -1;
				bool flag = false;
				long num2 = 0L;
				ArrayList arrayList = new ArrayList();
				int num3 = 0;
				bool flag2 = true;
				while (this.m_bStopWatch < 1)
				{
					if (num != this.updateCnt)
					{
						this.m_NowWatching = null;
						if (this.m_WantWatching != null)
						{
							Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_NowWatching, this.m_WantWatching);
						}
						Interlocked.Exchange(ref num, this.updateCnt);
					}
					else if (this.m_NowWatching == null)
					{
						Thread.Sleep(100);
					}
					else
					{
						long ticks = DateTime.Now.Ticks;
						flag = false;
						foreach (KeyValuePair<int, icController> keyValuePair in this.m_NowWatching)
						{
							if (string.IsNullOrEmpty(keyValuePair.Value.IP))
							{
								flag = true;
								break;
							}
						}
						flag2 = true;
						if (this.m_NowWatching.Count <= 5 || !flag)
						{
							num2 = 0L;
							using (Dictionary<int, icController>.Enumerator enumerator2 = this.m_NowWatching.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<int, icController> keyValuePair2 = enumerator2.Current;
									if (string.IsNullOrEmpty(keyValuePair2.Value.IP) || arrayList.IndexOf(string.Format("{0}:{1}", keyValuePair2.Value.IP, keyValuePair2.Value.PORT)) < 0)
									{
										wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair2.Value.ControllerSN;
										if (array != null)
										{
											wgpacketBasicRunInformation4ServerToSend.GetNewXid();
										}
										array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
										DateTime dateTime = DateTime.Now;
										this.udpserver.UDP_OnlySend(array, 300, keyValuePair2.Value.IP, keyValuePair2.Value.PORT);
										DateTime dateTime2 = DateTime.Now;
										if (dateTime.AddSeconds(2.0) < dateTime2)
										{
											flag2 = false;
											arrayList.Add(string.Format("{0}:{1}", keyValuePair2.Value.IP, keyValuePair2.Value.PORT));
										}
										if (!flag)
										{
											Thread.Sleep(WatchingService.ip_packet_interval_ms);
										}
										else
										{
											Thread.Sleep(WatchingService.broadcast_packet_interval_ms);
										}
									}
								}
								goto IL_047D;
							}
							goto IL_02D1;
						}
						goto IL_02D1;
						IL_047D:
						if (flag2 && arrayList.Count > 0)
						{
							if (num3 >= arrayList.Count)
							{
								num3 = 0;
							}
							else
							{
								foreach (KeyValuePair<int, icController> keyValuePair3 in this.m_NowWatching)
								{
									if (!string.IsNullOrEmpty(keyValuePair3.Value.IP) && (string)arrayList[num3] == string.Format("{0}:{1}", keyValuePair3.Value.IP, keyValuePair3.Value.PORT))
									{
										wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair3.Value.ControllerSN;
										if (array != null)
										{
											wgpacketBasicRunInformation4ServerToSend.GetNewXid();
										}
										array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
										DateTime dateTime = DateTime.Now;
										this.udpserver.UDP_OnlySend(array, 300, keyValuePair3.Value.IP, keyValuePair3.Value.PORT);
										DateTime dateTime2 = DateTime.Now;
										if (dateTime.AddSeconds(2.0) < dateTime2)
										{
											num3++;
											break;
										}
										arrayList.RemoveAt(num3);
										break;
									}
								}
							}
						}
						long ticks2 = DateTime.Now.Ticks;
						this.m_lastGetInfoDateTime = DateTime.Now;
						if (ticks2 <= ticks || ticks2 - ticks >= (long)(WatchingService.m_watching_cycle_ms * 1000 * 10))
						{
							continue;
						}
						if (!flag)
						{
							Thread.Sleep(WatchingService.m_watching_cycle_ms - Math.Max(0, (int)(ticks2 - ticks) / 10000));
							continue;
						}
						int i = WatchingService.m_watching_cycle_ms - Math.Max(0, (int)(ticks2 - ticks) / 10000);
						int num4 = 300;
						int num5 = 30;
						int num6 = 0;
						while (i > 0)
						{
							num6 = 0;
							if (this.m_NowWatching.Count <= 5)
							{
								using (Dictionary<int, icController>.Enumerator enumerator4 = this.m_NowWatching.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										KeyValuePair<int, icController> keyValuePair4 = enumerator4.Current;
										if (string.IsNullOrEmpty(keyValuePair4.Value.IP))
										{
											wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair4.Value.ControllerSN;
											if (array != null)
											{
												wgpacketBasicRunInformation4ServerToSend.GetNewXid();
											}
											array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
											this.udpserver.UDP_OnlySend(array, 300, keyValuePair4.Value.IP, keyValuePair4.Value.PORT);
											Thread.Sleep(num5);
											num6 += num5;
											i -= num5;
										}
									}
									goto IL_0749;
								}
								goto IL_0701;
							}
							goto IL_0701;
							IL_0749:
							if (i < 0)
							{
								continue;
							}
							if (i <= num4)
							{
								Thread.Sleep(i);
								continue;
							}
							if (num6 >= num4)
							{
								Thread.Sleep(50);
								i -= 50;
								continue;
							}
							Thread.Sleep(num4 - num6);
							i -= num4 - num6;
							continue;
							IL_0701:
							wgpacketBasicRunInformation4ServerToSend.iDevSnTo = uint.MaxValue;
							if (array != null)
							{
								wgpacketBasicRunInformation4ServerToSend.GetNewXid();
							}
							array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
							this.udpserver.UDP_OnlySend(array, 300, "", 60000);
							Thread.Sleep(num5);
							num6 += num5;
							i -= num5;
							goto IL_0749;
						}
						continue;
						IL_02D1:
						wgpacketBasicRunInformation4ServerToSend.iDevSnTo = uint.MaxValue;
						if (array != null)
						{
							wgpacketBasicRunInformation4ServerToSend.GetNewXid();
						}
						array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
						this.udpserver.UDP_OnlySend(array, 300, "", 60000);
						Thread.Sleep(30);
						DateTime now2 = DateTime.Now;
						if ((long)(WatchingService.m_watching_cycle_ms * 10000) < now2.Ticks - num2)
						{
							num2 = now2.Ticks + 10L;
							foreach (KeyValuePair<int, icController> keyValuePair5 in this.m_NowWatching)
							{
								if (!string.IsNullOrEmpty(keyValuePair5.Value.IP) && (string.IsNullOrEmpty(keyValuePair5.Value.IP) || arrayList.IndexOf(string.Format("{0}:{1}", keyValuePair5.Value.IP, keyValuePair5.Value.PORT)) < 0))
								{
									wgpacketBasicRunInformation4ServerToSend.iDevSnTo = (uint)keyValuePair5.Value.ControllerSN;
									if (array != null)
									{
										wgpacketBasicRunInformation4ServerToSend.GetNewXid();
									}
									array = wgpacketBasicRunInformation4ServerToSend.ToBytes();
									DateTime dateTime = DateTime.Now;
									this.udpserver.UDP_OnlySend(array, 300, keyValuePair5.Value.IP, keyValuePair5.Value.PORT);
									DateTime dateTime2 = DateTime.Now;
									if (dateTime.AddSeconds(2.0) < dateTime2)
									{
										flag2 = false;
										arrayList.Add(string.Format("{0}:{1}", keyValuePair5.Value.IP, keyValuePair5.Value.PORT));
									}
									Thread.Sleep(WatchingService.ip_packet_interval_ms);
								}
							}
							goto IL_047D;
						}
						goto IL_047D;
					}
				}
				wgTools.WgDebugWrite("udpserver.Dispose() ", new object[0]);
				this.udpserver.evNewRecord -= this.udpserver_evNewRecord;
				this.udpserver.Dispose();
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000F1BE8 File Offset: 0x000F0BE8
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x000F1BEF File Offset: 0x000F0BEF
		public static int broadcast_packet_interval_ms
		{
			get
			{
				return WatchingService.m_broadcast_packet_interval_ms;
			}
			set
			{
				WatchingService.m_broadcast_packet_interval_ms = Math.Max(25, value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x000F1BFE File Offset: 0x000F0BFE
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x000F1C05 File Offset: 0x000F0C05
		public static int ip_packet_interval_ms
		{
			get
			{
				return WatchingService.m_ip_packet_interval_ms;
			}
			set
			{
				WatchingService.m_ip_packet_interval_ms = Math.Max(1, value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x000F1C13 File Offset: 0x000F0C13
		public DateTime lastGetInfoDateTime
		{
			get
			{
				return this.m_lastGetInfoDateTime;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x000F1C1B File Offset: 0x000F0C1B
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x000F1C42 File Offset: 0x000F0C42
		public static int unconnect_timeout_sec
		{
			get
			{
				if (WatchingService.Watching_Cycle_ms > WatchingService.m_unconnect_timeout_sec * 1000)
				{
					return WatchingService.Watching_Cycle_ms / 1000 + 1;
				}
				return WatchingService.m_unconnect_timeout_sec;
			}
			set
			{
				if (value > 0 && value < 3600)
				{
					WatchingService.m_unconnect_timeout_sec = value;
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000F1C56 File Offset: 0x000F0C56
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x000F1C5D File Offset: 0x000F0C5D
		public static int Watching_Cycle_ms
		{
			get
			{
				return WatchingService.m_watching_cycle_ms;
			}
			set
			{
				if (value > 0 && value < 3600000)
				{
					WatchingService.m_watching_cycle_ms = value;
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000F1C71 File Offset: 0x000F0C71
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x000F1C7C File Offset: 0x000F0C7C
		public Dictionary<int, icController> WatchingController
		{
			get
			{
				return this.m_NowWatching;
			}
			set
			{
				if (this.m_WantWatching != null)
				{
					this.m_WantWatching = null;
				}
				if (value != null)
				{
					Dictionary<int, icController> dictionary = new Dictionary<int, icController>(value);
					Interlocked.Exchange<Dictionary<int, icController>>(ref this.m_WantWatching, dictionary);
				}
				this.m_ControllerUpdateTime = DateTime.Now;
				if (this.updateCnt == 2147483647)
				{
					Interlocked.Exchange(ref this.updateCnt, 0);
				}
				Interlocked.Increment(ref this.updateCnt);
			}
		}

		// Token: 0x04001A77 RID: 6775
		private int m_bHaveServer;

		// Token: 0x04001A78 RID: 6776
		private static int m_broadcast_packet_interval_ms = 25;

		// Token: 0x04001A79 RID: 6777
		private int m_bStopWatch;

		// Token: 0x04001A7A RID: 6778
		private DateTime m_ControllerUpdateTime = DateTime.Now;

		// Token: 0x04001A7B RID: 6779
		private static int m_ip_packet_interval_ms = 1;

		// Token: 0x04001A7C RID: 6780
		private DateTime m_lastGetInfoDateTime = DateTime.Now;

		// Token: 0x04001A7D RID: 6781
		private Dictionary<int, icController> m_NowWatching;

		// Token: 0x04001A7E RID: 6782
		private static int m_unconnect_timeout_sec = 6;

		// Token: 0x04001A7F RID: 6783
		private Dictionary<int, icController> m_WantWatching;

		// Token: 0x04001A80 RID: 6784
		private static int m_watching_cycle_ms = 5000;

		// Token: 0x04001A81 RID: 6785
		private wgUdpServer udpserver;

		// Token: 0x04001A82 RID: 6786
		private int updateCnt;
	}
}
