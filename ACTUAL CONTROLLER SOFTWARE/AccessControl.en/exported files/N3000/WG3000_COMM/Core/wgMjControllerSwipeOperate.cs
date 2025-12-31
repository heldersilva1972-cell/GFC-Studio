using System;
using System.Data;

namespace WG3000_COMM.Core
{
	// Token: 0x02000062 RID: 98
	public class wgMjControllerSwipeOperate : IDisposable
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x000D2DC3 File Offset: 0x000D1DC3
		public wgMjControllerSwipeOperate()
		{
			this.Clear();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000D2DDC File Offset: 0x000D1DDC
		public void Clear()
		{
			wgMjControllerSwipeOperate.m_bStopGetRecord = false;
			this.m_ControllerSN = -1;
			this.m_lastRecordFlashIndex = -1;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000D2DF2 File Offset: 0x000D1DF2
		protected virtual void DisplayProcessInfo(string info, int infoCode, int specialInfo)
		{
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000D2DF4 File Offset: 0x000D1DF4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000D2E03 File Offset: 0x000D1E03
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_wgudp != null)
				{
					this.m_wgudp.Close();
					this.m_wgudp.Dispose();
				}
				if (this.control != null)
				{
					this.control.Dispose();
				}
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000D2E3A File Offset: 0x000D1E3A
		protected internal int GetSwipeIndex(uint iSwipeLoc)
		{
			return (int)((iSwipeLoc - 5017600U) / 16U);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000D2E48 File Offset: 0x000D1E48
		protected internal int GetSwipeLoc(int iSwipeIndex)
		{
			int num = iSwipeIndex - iSwipeIndex % 204800;
			if (iSwipeIndex >= num)
			{
				return 5017600 + 256 * ((iSwipeIndex - num) / 16) + (iSwipeIndex - num) % 16 * 16;
			}
			return 8294399 - 256 * ((num - iSwipeIndex) / 16) + (num - iSwipeIndex) % 16 * 16;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000D2E9C File Offset: 0x000D1E9C
		public int GetSwipeRecords(int ControllerSN, string IP, int Port, ref DataTable dtSwipe)
		{
			wgTools.WriteLine("getSwipeRecords Start");
			this.control.ControllerSN = ControllerSN;
			this.control.IP = IP;
			this.control.PORT = Port;
			this.m_ControllerSN = ControllerSN;
			byte[] array = null;
			if (this.control.GetMjControllerRunInformationIP(ref array) < 0)
			{
				return -13;
			}
			wgMjControllerRunInformation wgMjControllerRunInformation = new wgMjControllerRunInformation();
			if (array == null)
			{
				return -1;
			}
			if (ControllerSN != -1)
			{
				wgMjControllerRunInformation.UpdateInfo_internal(array, 20, (uint)ControllerSN);
			}
			else
			{
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				wgMjControllerRunInformation.UpdateInfo_internal(array, 20, num);
			}
			if (wgMjControllerRunInformation.newRecordsNum == 0U)
			{
				this.m_lastRecordFlashIndex = (int)wgMjControllerRunInformation.lastGetRecordIndex;
				return 0;
			}
			wgTools.getUdpComm(ref this.m_wgudp, -1);
			byte[] array2 = null;
			WGPacketSSI_FLASH_QUERY_internal wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
			byte[] array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.m_wgudp.udpPort);
			if (array3 == null)
			{
				return -12;
			}
			array2 = null;
			if (this.m_wgudp.udp_get(array3, 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array2) < 0)
			{
				return -13;
			}
			wgTools.WriteLine(string.Format("\r\nBegin Sending Command:\t{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			string.Format("SSI_FLASH_{0}", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
			int num2 = 4096;
			int num3 = 0;
			int num4 = 0;
			int num5 = (int)wgMjControllerRunInformation.lastGetRecordIndex;
			num5 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.newRecordsNum);
			if (num5 > 0)
			{
				int swipeLoc = this.GetSwipeLoc(num5);
				wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)ControllerSN, (uint)(swipeLoc - swipeLoc % 1024), (uint)(swipeLoc - swipeLoc % 1024 + 1024 - 1));
				array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.m_wgudp.udpPort);
				if (array3 == null)
				{
					return -12;
				}
				array2 = null;
				if (this.m_wgudp.udp_get(array3, 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array2) < 0)
				{
					return -13;
				}
			}
			uint iStartFlashAddr = wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr;
			bool flag = false;
			uint num6 = 0U;
			wgTools.WriteLine(string.Format("First Page:\t ={0:d}", iStartFlashAddr / 1024U));
			int num7 = num5 - num5 % 204800;
			while (!wgMjControllerSwipeOperate.m_bStopGetRecord && array2 != null)
			{
				WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal(array2);
				uint num8 = (uint)(this.GetSwipeIndex(wgpacketSSI_FLASH_internal.iStartFlashAddr) + num7);
				for (uint num9 = 0U; num9 < 1024U; num9 += 16U)
				{
					wgMjControllerSwipeRecord wgMjControllerSwipeRecord = new wgMjControllerSwipeRecord(wgpacketSSI_FLASH_internal.ucData, num9, wgpacketSSI_FLASH_internal.iDevSnFrom, num8);
					if (wgMjControllerSwipeRecord.CardID == -1L || ((wgMjControllerSwipeRecord.bytRecOption_Internal == 0 || wgMjControllerSwipeRecord.bytRecOption_Internal == 255) && wgMjControllerSwipeRecord.CardID == 0L))
					{
						if (wgMjControllerRunInformation.swipeEndIndex <= num8)
						{
							break;
						}
						num6 += 1U;
						num8 += 1U;
					}
					else
					{
						if (num3 > 0 || (ulong)wgMjControllerSwipeRecord.indexInDataFlash_Internal >= (ulong)((long)num5))
						{
							DataRow dataRow = dtSwipe.NewRow();
							dataRow["f_Index"] = num3 + 1;
							dataRow["f_CardNO"] = wgMjControllerSwipeRecord.CardID;
							dataRow["f_DoorNO"] = wgMjControllerSwipeRecord.DoorNo;
							dataRow["f_InOut"] = (wgMjControllerSwipeRecord.IsEnterIn ? 1 : 0);
							dataRow["f_ReaderNO"] = wgMjControllerSwipeRecord.ReaderNo;
							dataRow["f_ReadDate"] = wgMjControllerSwipeRecord.ReadDate;
							dataRow["f_EventCategory"] = wgMjControllerSwipeRecord.eventCategory;
							dataRow["f_ReasonNo"] = wgMjControllerSwipeRecord.ReasonNo;
							dataRow["f_ControllerSN"] = wgMjControllerSwipeRecord.ControllerSN;
							if (dtSwipe.Columns.Contains("f_RecordAll"))
							{
								dataRow["f_RecordAll"] = wgMjControllerSwipeRecord.ToStringRaw();
							}
							dtSwipe.Rows.Add(dataRow);
							num3++;
							num4 = (int)wgMjControllerSwipeRecord.indexInDataFlash_Internal;
						}
						num8 += 1U;
					}
				}
				dtSwipe.AcceptChanges();
				if (wgMjControllerRunInformation.swipeEndIndex <= num8)
				{
					flag = true;
				}
				if (flag)
				{
					break;
				}
				wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)ControllerSN, wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr + 1024U, wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr + 1024U + 1024U - 1U);
				if (wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr > 8294399U)
				{
					wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)ControllerSN, 5017600U, 5018623U);
					num7 += 204800;
				}
				if (wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr == iStartFlashAddr)
				{
					break;
				}
				wgpacketSSI_FLASH_QUERY_internal.GetNewXid();
				array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.m_wgudp.udpPort);
				if (array3 == null || this.m_wgudp.udp_get(array3, 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array2) < 0 || --num2 <= 0)
				{
					break;
				}
			}
			if (wgMjControllerSwipeOperate.m_bStopGetRecord)
			{
				return -1;
			}
			wgTools.WriteLine(string.Format("Last Page:\t ={0:d}", wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr / 1024U));
			wgTools.WriteLine("Syn Data Info");
			if (num3 > 0)
			{
				if (this.control.GetMjControllerRunInformationIP(ref array) < 0)
				{
					return -13;
				}
				if (array != null)
				{
					if (ControllerSN != -1)
					{
						wgMjControllerRunInformation.UpdateInfo_internal(array, 20, (uint)ControllerSN);
					}
					else
					{
						uint num10 = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
						wgMjControllerRunInformation.UpdateInfo_internal(array, 20, num10);
					}
				}
				if ((long)(num4 % 204800) >= (long)((ulong)(wgMjControllerRunInformation.swipeEndIndex % 204800U)))
				{
					if (wgMjControllerRunInformation.swipeEndIndex > 204800U)
					{
						num5 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.swipeEndIndex % 204800U - 204800U + (uint)(num4 % 204800));
					}
					else
					{
						num5 = 0;
					}
				}
				else
				{
					num5 = (int)(wgMjControllerRunInformation.swipeEndIndex - wgMjControllerRunInformation.swipeEndIndex % 204800U + (uint)(num4 % 204800));
				}
				this.control.UpdateLastGetRecordLocationIP((uint)(num5 + 1));
				this.m_lastRecordFlashIndex = num5 + 1;
			}
			return num3;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000D3497 File Offset: 0x000D2497
		public static void StopGetRecord()
		{
			wgMjControllerSwipeOperate.m_bStopGetRecord = true;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000D349F File Offset: 0x000D249F
		protected internal static bool bStopGetRecord
		{
			get
			{
				return wgMjControllerSwipeOperate.m_bStopGetRecord;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000D34A6 File Offset: 0x000D24A6
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x000D34AE File Offset: 0x000D24AE
		public int ControllerSN
		{
			get
			{
				return this.m_ControllerSN;
			}
			protected internal set
			{
				this.m_ControllerSN = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000D34B7 File Offset: 0x000D24B7
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x000D34BF File Offset: 0x000D24BF
		public int lastRecordFlashIndex
		{
			get
			{
				return this.m_lastRecordFlashIndex;
			}
			protected internal set
			{
				this.m_lastRecordFlashIndex = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x000D34C8 File Offset: 0x000D24C8
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x000D34D0 File Offset: 0x000D24D0
		protected internal wgUdpComm wgudp
		{
			get
			{
				return this.m_wgudp;
			}
			set
			{
				this.m_wgudp = value;
			}
		}

		// Token: 0x04000D37 RID: 3383
		private const int SSI_FLASH_PAGE_SIZE = 256;

		// Token: 0x04000D38 RID: 3384
		protected internal const int SSI_FLASH_SWIPE_ENDADDR = 8294399;

		// Token: 0x04000D39 RID: 3385
		private const int SSI_FLASH_SWIPE_PAGES = 3200;

		// Token: 0x04000D3A RID: 3386
		protected internal const int SSI_FLASH_SWIPE_STARTADDR = 5017600;

		// Token: 0x04000D3B RID: 3387
		protected internal const int SWIPE_RECORDS_MAXLEN = 204800;

		// Token: 0x04000D3C RID: 3388
		private const int SWIPE_RECORDS_PER_PAGE = 16;

		// Token: 0x04000D3D RID: 3389
		protected internal const int SWIPE_SIZE = 16;

		// Token: 0x04000D3E RID: 3390
		private wgMjController control = new wgMjController();

		// Token: 0x04000D3F RID: 3391
		private static bool m_bStopGetRecord;

		// Token: 0x04000D40 RID: 3392
		private int m_ControllerSN;

		// Token: 0x04000D41 RID: 3393
		private int m_lastRecordFlashIndex;

		// Token: 0x04000D42 RID: 3394
		private wgUdpComm m_wgudp;
	}
}
