using System;
using System.Data;
using System.Text;
using System.Threading;

namespace WG3000_COMM.Core
{
	// Token: 0x0200005F RID: 95
	public class wgMjControllerPrivilege : IDisposable
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x000CCB50 File Offset: 0x000CBB50
		public int AddPrivilegeOfOneCardIP(int ControllerSN, string IP, int Port, MjRegisterCard mjrc)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.GetInfo_AddPrivilegeOfOneCardIP_IP64(ControllerSN, IP, Port, mjrc);
			}
			int num = -1;
			WGPacketPrivilege wgpacketPrivilege = new WGPacketPrivilege(mjrc);
			wgpacketPrivilege.type = 35;
			wgpacketPrivilege.code = 32;
			wgpacketPrivilege.iDevSnFrom = 0U;
			if (ControllerSN == -1)
			{
				wgpacketPrivilege.iDevSnTo = uint.MaxValue;
			}
			else
			{
				wgpacketPrivilege.iDevSnTo = (uint)ControllerSN;
			}
			wgpacketPrivilege.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketPrivilege.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return num;
			}
			int num2 = this.wgudp.udp_get(array2, this.m_priWaitMs, wgpacketPrivilege.xid, IP, Port, ref array);
			if (array != null)
			{
				return num2;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return num;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000CCC30 File Offset: 0x000CBC30
		public int AddPrivilegeWithUsernameOfOneCardIP(int ControllerSN, string IP, int Port, MjRegisterCard mjrc)
		{
			int num = -1;
			WGPacketPrivilegeWithUserName wgpacketPrivilegeWithUserName = new WGPacketPrivilegeWithUserName(mjrc);
			wgpacketPrivilegeWithUserName.type = 35;
			wgpacketPrivilegeWithUserName.code = 64;
			wgpacketPrivilegeWithUserName.iDevSnFrom = 0U;
			if (ControllerSN == -1)
			{
				wgpacketPrivilegeWithUserName.iDevSnTo = uint.MaxValue;
			}
			else
			{
				wgpacketPrivilegeWithUserName.iDevSnTo = (uint)ControllerSN;
			}
			wgpacketPrivilegeWithUserName.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketPrivilegeWithUserName.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return num;
			}
			int num2 = this.wgudp.udp_get(array2, this.m_priWaitMs, wgpacketPrivilegeWithUserName.xid, IP, Port, ref array);
			if (array != null)
			{
				return num2;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return num;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000CCCFA File Offset: 0x000CBCFA
		public void AllowDownload()
		{
			wgMjControllerPrivilege.m_bStopDownloadPrivilege = false;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000CCD02 File Offset: 0x000CBD02
		public void AllowUpload()
		{
			wgMjControllerPrivilege.m_bStopUploadPrivilege = false;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000CCD0C File Offset: 0x000CBD0C
		public int ClearAllPrivilegeIP(int ControllerSN, string IP, int Port)
		{
			byte[] array = null;
			MjRegisterCardsParam mjRegisterCardsParam = new MjRegisterCardsParam();
			int num = 0;
			uint num2 = 0U;
			mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = num2;
			mjRegisterCardsParam.newPrivilegePage4KAddr = num2 + (uint)((num >> 9) * 4 * 1024);
			mjRegisterCardsParam.freeNewPrivilegePageAddr = mjRegisterCardsParam.newPrivilegePage4KAddr + 4096U + (uint)(num % 512 * 8);
			mjRegisterCardsParam.bOrderInfreePrivilegePage = 1U;
			mjRegisterCardsParam.totalPrivilegeCount = (uint)num;
			mjRegisterCardsParam.deletedPrivilegeCount = 0U;
			WGPacketWith1024 wgpacketWith = new WGPacketWith1024();
			wgpacketWith.type = 35;
			wgpacketWith.code = 48;
			wgpacketWith.iDevSnFrom = 0U;
			wgpacketWith.iDevSnTo = (uint)ControllerSN;
			wgpacketWith.iCallReturn = 0;
			int num3 = 0;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.newPrivilegePage4KAddr), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.freeNewPrivilegePageAddr), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.bOrderInfreePrivilegePage), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.totalPrivilegeCount), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam.deletedPrivilegeCount), 0, wgpacketWith.ucData, num3, 4);
			num3 += 4;
			if (num > 92160)
			{
				Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
				Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
			}
			else if (mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0U)
			{
				Array.Copy(BitConverter.GetBytes(819200), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
				Array.Copy(BitConverter.GetBytes(823296), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
			}
			else
			{
				Array.Copy(BitConverter.GetBytes(0), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
				Array.Copy(BitConverter.GetBytes(4096), 0, wgpacketWith.ucData, num3, 4);
				num3 += 4;
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array2 = wgpacketWith.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("出错");
				return -1;
			}
			int num4 = this.wgudp.udp_get(array2, this.m_priWaitMs, wgpacketWith.xid, IP, Port, ref array);
			if (array == null)
			{
				return -1;
			}
			return num4;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000CCF90 File Offset: 0x000CBF90
		public int DelPrivilegeOfOneCardIP(int ControllerSN, string IP, int Port, long CardID)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.GetInfo_DelPrivilegeOfOneCardIP_IP64(ControllerSN, IP, Port, CardID);
			}
			int num = -1;
			WGPacketPrivilege wgpacketPrivilege = new WGPacketPrivilege(new MjRegisterCard
			{
				CardID = CardID,
				IsDeleted = true
			});
			wgpacketPrivilege.type = 35;
			wgpacketPrivilege.code = 32;
			wgpacketPrivilege.iDevSnFrom = 0U;
			wgpacketPrivilege.iDevSnTo = (uint)ControllerSN;
			wgpacketPrivilege.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketPrivilege.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return num;
			}
			int num2 = this.wgudp.udp_get(array2, this.m_priWaitMs, wgpacketPrivilege.xid, IP, Port, ref array);
			if (array != null)
			{
				return num2;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return num;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000CD078 File Offset: 0x000CC078
		protected virtual void DisplayProcessInfo(string info, int infoCode, int specialInfo)
		{
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000CD07A File Offset: 0x000CC07A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000CD089 File Offset: 0x000CC089
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.wgudp != null)
			{
				this.wgudp.Close();
				this.wgudp.Dispose();
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000CD0AD File Offset: 0x000CC0AD
		public int DownloadIP(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege)
		{
			return this.DownloadIP_internal(ControllerSN, IP, Port, doorName, ref dtPrivilege, "");
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000CD0C1 File Offset: 0x000CC0C1
		public int DownloadIP(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege, string PCIPAddr)
		{
			return this.DownloadIP_internal(ControllerSN, IP, Port, doorName, ref dtPrivilege, PCIPAddr);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000CD0D4 File Offset: 0x000CC0D4
		internal int DownloadIP_internal(int ControllerSN, string IP, int Port, string doorName, ref DataTable dtPrivilege, string PCIPAddr)
		{
			if (wgMjControllerPrivilege.m_bStopDownloadPrivilege)
			{
				return -1;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
			{
				flag = true;
			}
			if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
			{
				flag2 = true;
			}
			if (dtPrivilege.Columns.Contains("f_IsDeleted"))
			{
				flag3 = true;
			}
			else
			{
				dtPrivilege.Columns.Add("f_IsDeleted", Type.GetType("System.UInt32"));
			}
			if (dtPrivilege.Columns.Contains("f_ConsumerName"))
			{
				flag4 = true;
			}
			wgTools.WriteLine("Download Privilege Start");
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 35;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array == null)
			{
				return -1;
			}
			byte[] array2 = null;
			this.wgudp.udp_get(array, 300, wgpacket.xid, IP, Port, ref array2);
			MjRegisterCardsParam mjRegisterCardsParam = new MjRegisterCardsParam();
			if (array2 == null)
			{
				return -1;
			}
			mjRegisterCardsParam.updateParam(array2, 20);
			long num = 0L;
			WGPacketSSI_FLASH_QUERY_internal wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal();
			wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)ControllerSN, (uint)num, (uint)(num + 1024L - 1L));
			long num2 = 0L;
			while (num2 < (long)((ulong)(mjRegisterCardsParam.totalPrivilegeCount - mjRegisterCardsParam.deletedPrivilegeCount)))
			{
				if ((ulong)((mjRegisterCardsParam.newPrivilegePage4KAddr - mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8U) > (ulong)num2)
				{
					num = (long)((ulong)mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR + (ulong)(num2 * 8L));
				}
				else
				{
					num = (long)((ulong)(mjRegisterCardsParam.newPrivilegePage4KAddr + 4096U) + (ulong)((num2 - (long)((ulong)((mjRegisterCardsParam.newPrivilegePage4KAddr - mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR) / 8U))) * 8L));
				}
				wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr = (uint)num;
				wgpacketSSI_FLASH_QUERY_internal.iEndFlashAddr = (uint)(num + 1024L - 1L);
				byte[] array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.wgudp.udpPort);
				if (array3 == null)
				{
					return -1;
				}
				byte[] array4 = null;
				if (this.wgudp.udp_get(array3, 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array4) < 0)
				{
					return -1;
				}
				MjRegisterCard mjRegisterCard = new MjRegisterCard();
				byte[] array5 = new byte[24];
				if (array4 != null)
				{
					Array.Copy(array4, 28, array5, 0, 8);
				}
				num = num * 2L + 1654784L;
				wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr = (uint)num;
				wgpacketSSI_FLASH_QUERY_internal.iEndFlashAddr = (uint)(num + 1024L - 1L);
				array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.wgudp.udpPort);
				if (array3 == null)
				{
					return -1;
				}
				byte[] array6 = null;
				if (this.wgudp.udp_get(array3, 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array6) < 0)
				{
					return -1;
				}
				if (array6 != null)
				{
					for (int i = 0; i < 64; i++)
					{
						Array.Copy(array4, 28 + i * 8, array5, 0, 8);
						Array.Copy(array6, 28 + i * 16, array5, 8, 16);
						int count = dtPrivilege.Rows.Count;
						if (mjRegisterCard.Update(array5, (uint)(num2 + (long)i)) <= 0)
						{
							break;
						}
						if (wgMjControllerPrivilege.bStopDownloadPrivilege)
						{
							return -100002;
						}
						DataRow dataRow = dtPrivilege.NewRow();
						dataRow["f_CardNO"] = mjRegisterCard.CardID;
						dataRow["f_PIN"] = mjRegisterCard.Password.ToString();
						dataRow["f_BeginYMD"] = mjRegisterCard.ymdStart;
						dataRow["f_EndYMD"] = mjRegisterCard.ymdEnd;
						dataRow["f_ControlSegID1"] = mjRegisterCard.ControlSegIndexGet(1);
						dataRow["f_ControlSegID2"] = mjRegisterCard.ControlSegIndexGet(2);
						dataRow["f_ControlSegID3"] = mjRegisterCard.ControlSegIndexGet(3);
						dataRow["f_ControlSegID4"] = mjRegisterCard.ControlSegIndexGet(4);
						if (flag)
						{
							dataRow["f_DoorFirstCard_1"] = mjRegisterCard.FirstCardGet(1);
							dataRow["f_DoorFirstCard_2"] = mjRegisterCard.FirstCardGet(2);
							dataRow["f_DoorFirstCard_3"] = mjRegisterCard.FirstCardGet(3);
							dataRow["f_DoorFirstCard_4"] = mjRegisterCard.FirstCardGet(4);
						}
						if (flag2)
						{
							dataRow["f_MoreCards_GrpID_1"] = mjRegisterCard.MoreCardGroupIndexGet(1);
							dataRow["f_MoreCards_GrpID_2"] = mjRegisterCard.MoreCardGroupIndexGet(2);
							dataRow["f_MoreCards_GrpID_3"] = mjRegisterCard.MoreCardGroupIndexGet(3);
							dataRow["f_MoreCards_GrpID_4"] = mjRegisterCard.MoreCardGroupIndexGet(4);
						}
						dataRow["f_IsDeleted"] = (mjRegisterCard.IsDeleted ? 1 : 0);
						dataRow["f_AllowFloors"] = mjRegisterCard.AllowFloors;
						dtPrivilege.Rows.Add(dataRow);
					}
					dtPrivilege.AcceptChanges();
				}
				num2 += 64L;
				if (wgMjControllerPrivilege.m_bStopDownloadPrivilege)
				{
					return -1;
				}
			}
			if ((ulong)mjRegisterCardsParam.totalPrivilegeCount > (ulong)((long)this.m_uploadUserNameMaxlen))
			{
				flag4 = false;
			}
			if (flag4)
			{
				int userNamePageStart = this.m_userNamePageStart;
				wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr = (uint)(userNamePageStart * 1024);
				wgpacketSSI_FLASH_QUERY_internal.iEndFlashAddr = wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr + 1024U - 1U;
				if (mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR > 0U)
				{
					wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr = (uint)(this.m_userNamePageHalfStart * 1024);
				}
				int num3 = 0;
				while (wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr <= 4956159U)
				{
					if (this.wgudp.udp_get(wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH_QUERY_internal.xid, IP, Port, ref array2) < 0)
					{
						return -1;
					}
					if (array2 != null)
					{
						for (int j = 0; j < 1024; j += 32)
						{
							byte[] array7 = new byte[33];
							int num4 = 0;
							array7[32] = 0;
							for (int k = 0; k < 32; k++)
							{
								array7[k] = array2[28 + j + k];
								if (array7[k] == 0)
								{
									break;
								}
								num4++;
							}
							if ((array7[0] != 255 || array7[1] != 255) && array7[0] != 0)
							{
								if (num3 >= dtPrivilege.Rows.Count)
								{
									break;
								}
								string @string = Encoding.GetEncoding("utf-8").GetString(array7, 0, num4);
								dtPrivilege.Rows[num3]["f_ConsumerName"] = @string;
							}
							num3++;
						}
					}
					else
					{
						num3 += 32;
					}
					if (num3 >= dtPrivilege.Rows.Count)
					{
						break;
					}
					wgpacketSSI_FLASH_QUERY_internal.iStartFlashAddr += 1024U;
				}
				dtPrivilege.AcceptChanges();
			}
			DataView dataView = new DataView(dtPrivilege);
			dataView.RowFilter = "f_IsDeleted = 1";
			if (dataView.Count > 0)
			{
				for (int l = dataView.Count - 1; l >= 0; l--)
				{
					dataView.Delete(l);
				}
			}
			dtPrivilege.AcceptChanges();
			if (!flag3)
			{
				dtPrivilege.Columns.Remove("f_IsDeleted");
				dtPrivilege.AcceptChanges();
			}
			return 1;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000CD810 File Offset: 0x000CC810
		public int GetInfo_AddPrivilegeOfOneCardIP_IP64(int ControllerSN, string IP, int Port, MjRegisterCard mjrc)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 80;
			wgpacketShort.iDevSn = (uint)ControllerSN;
			Array.Copy(mjrc.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
			Array.Copy(mjrc.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
			byte[] array = null;
			return this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, IP, Port, -1);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000CD884 File Offset: 0x000CC884
		public int GetInfo_BaseComm_IP64(ref byte[] recvInfo, byte[] bytToSend, uint xid, string IP, int Port, int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			int num = this.wgudp.udp_get(bytToSend, 300, xid, IP, Port, ref array);
			if (array != null && array.Length == 64)
			{
				recvInfo = array;
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return num;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000CD8E0 File Offset: 0x000CC8E0
		public int GetInfo_DelPrivilegeOfOneCardIP_IP64(int ControllerSN, string IP, int Port, long CardID)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 82;
			wgpacketShort.iDevSn = (uint)ControllerSN;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, CardID);
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 36, CardID >> 32);
			byte[] array = null;
			return this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, IP, Port, -1);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000CD93B File Offset: 0x000CC93B
		public static void StopDownload()
		{
			wgMjControllerPrivilege.m_bStopDownloadPrivilege = true;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000CD943 File Offset: 0x000CC943
		public static void StopUpload()
		{
			wgMjControllerPrivilege.m_bStopUploadPrivilege = true;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000CD94B File Offset: 0x000CC94B
		public int UploadIP(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, int UDPQueue4MultithreadIndex = -1)
		{
			return this.UploadIP_internal(ControllerSN, IP, Port, doorName, dtPrivilege, "", UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000CD961 File Offset: 0x000CC961
		public int UploadIP(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, string PCIPAddr)
		{
			return this.UploadIP_internal(ControllerSN, IP, Port, doorName, dtPrivilege, PCIPAddr, -1);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000CD974 File Offset: 0x000CC974
		internal int UploadIP_internal(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.UploadIP_internal_IP64(ControllerSN, IP, Port, doorName, dtPrivilege, PCIPAddr, UDPQueue4MultithreadIndex);
			}
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return -100002;
			}
			if (dtPrivilege.Rows.Count >= 2)
			{
				long num = 0L;
				for (int i = 0; i < dtPrivilege.Rows.Count - 1; i++)
				{
					if ((long)dtPrivilege.Rows[i]["f_CardNO"] <= num)
					{
						return -200;
					}
					num = (long)dtPrivilege.Rows[i]["f_CardNO"];
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
			{
				flag = true;
			}
			if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
			{
				flag2 = true;
			}
			bool flag3 = this.bAllowUploadUserName;
			if (dtPrivilege.Rows.Count > this.m_uploadUserNameMaxlen)
			{
				flag3 = false;
			}
			if (!dtPrivilege.Columns.Contains("f_ConsumerName"))
			{
				flag3 = false;
			}
			wgAppConfig.getParamValBoolByNO(200);
			wgTools.WriteLine("upload Start");
			SIIFlash siiflash = new SIIFlash();
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 35;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array == null)
			{
				return -202;
			}
			byte[] array2 = null;
			int num2 = 3;
			int num3 = 300;
			for (int j = 0; j < num2; j++)
			{
				int num4 = this.wgudp.udp_get(array, 300, wgpacket.xid, IP, Port, ref array2);
				if (array2 != null)
				{
					break;
				}
				Thread.Sleep(num3);
			}
			if (array2 == null)
			{
				return -203;
			}
			MjRegisterCardsParam mjRegisterCardsParam = new MjRegisterCardsParam();
			if (array2 == null)
			{
				return -207;
			}
			mjRegisterCardsParam.updateParam(array2, 20);
			string text = "";
			text = string.Concat(new string[]
			{
				text,
				string.Format("权限起始页 = 0x{0:X8}\r\n", mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR),
				string.Format("存储新有序权限的4K的页面 = 0x{0:X8}\r\n", mjRegisterCardsParam.newPrivilegePage4KAddr),
				string.Format("自由的记录页面(无序的) = 0x{0:X8}\r\n", mjRegisterCardsParam.freeNewPrivilegePageAddr),
				string.Format("是否有序的(自由页面中) = {0:d}\r\n", mjRegisterCardsParam.bOrderInfreePrivilegePage),
				string.Format("总权限数 = {0:d}\r\n", mjRegisterCardsParam.totalPrivilegeCount),
				string.Format("已删除的权限数 = {0:d}\r\n", mjRegisterCardsParam.deletedPrivilegeCount)
			});
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return -100002;
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			int num5 = 0;
			uint num6 = 0U;
			if (mjRegisterCardsParam.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0U)
			{
				num6 = 819200U;
			}
			if (mjRegisterCardsParam.totalPrivilegeCount == 0U)
			{
				num6 = 0U;
			}
			int count = dtPrivilege.Rows.Count;
			if (wgTools.gWGYTJ && count > 500)
			{
				this.DisplayProcessInfo(doorName, -100001, count);
				return -100001;
			}
			if ((long)count > (long)((ulong)mjRegisterCardsParam.MaxPrivilegesNum))
			{
				this.DisplayProcessInfo(doorName, -100001, count);
				return -100001;
			}
			if (count >= 92160)
			{
				num6 = 0U;
				int num4 = this.ClearAllPrivilegeIP(ControllerSN, IP, Port);
				if (num4 < 0)
				{
					wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed clearAllPrivilegeIP ret= " + num4.ToString(), new object[0]);
					return num4;
				}
			}
			MjRegisterCardsParam mjRegisterCardsParam2 = new MjRegisterCardsParam();
			mjRegisterCardsParam2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR = num6;
			mjRegisterCardsParam2.newPrivilegePage4KAddr = num6 + (uint)((count >> 9) * 4 * 1024);
			mjRegisterCardsParam2.freeNewPrivilegePageAddr = mjRegisterCardsParam2.newPrivilegePage4KAddr + 4096U + (uint)(count % 512 * 8);
			mjRegisterCardsParam2.bOrderInfreePrivilegePage = 1U;
			mjRegisterCardsParam2.totalPrivilegeCount = (uint)count;
			mjRegisterCardsParam2.deletedPrivilegeCount = 0U;
			if (count != 0)
			{
				WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
				wgpacketSSI_FLASH_internal.type = 33;
				wgpacketSSI_FLASH_internal.code = 32;
				wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
				wgpacketSSI_FLASH_internal.iDevSnTo = (uint)ControllerSN;
				wgpacketSSI_FLASH_internal.iCallReturn = 0;
				wgpacketSSI_FLASH_internal.ucData = new byte[1024];
				this.DisplayProcessInfo(doorName, 100001, 0);
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)num5;
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				for (int k = 0; k < 1024; k++)
				{
					wgpacketSSI_FLASH_internal.ucData[k] = byte.MaxValue;
				}
				MjRegisterCard mjRegisterCard = new MjRegisterCard();
				mjRegisterCard.CardID = 28004L;
				long num7 = (long)((ulong)num6);
				long num8 = num7;
				long num9 = 1654784L;
				long num10 = num9 + (long)((ulong)(num6 * 2U));
				long num11 = num10;
				long num12 = ((num7 == 0L) ? ((long)(this.m_userNamePageStart * 1024)) : ((long)(this.m_userNamePageHalfStart * 1024)));
				long num13 = num12;
				int l = 0;
				while ((l & 268434944) < (count & 268434944))
				{
					if (wgMjControllerPrivilege.bStopUploadPrivilege)
					{
						return -100002;
					}
					mjRegisterCard.CardID = long.Parse(dtPrivilege.Rows[l]["f_CardNO"].ToString());
					mjRegisterCard.Password = uint.Parse(dtPrivilege.Rows[l]["f_PIN"].ToString());
					mjRegisterCard.ymdStart = (DateTime)dtPrivilege.Rows[l]["f_BeginYMD"];
					mjRegisterCard.ymdEnd = (DateTime)dtPrivilege.Rows[l]["f_EndYMD"];
					mjRegisterCard.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[l]["f_ControlSegID1"]);
					mjRegisterCard.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[l]["f_ControlSegID2"]);
					mjRegisterCard.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[l]["f_ControlSegID3"]);
					mjRegisterCard.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[l]["f_ControlSegID4"]);
					if (flag)
					{
						mjRegisterCard.FirstCardSet(1, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_1"] > 0);
						mjRegisterCard.FirstCardSet(2, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_2"] > 0);
						mjRegisterCard.FirstCardSet(3, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_3"] > 0);
						mjRegisterCard.FirstCardSet(4, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_4"] > 0);
					}
					if (flag2)
					{
						mjRegisterCard.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_1"]);
						mjRegisterCard.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_2"]);
						mjRegisterCard.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_3"]);
						mjRegisterCard.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_4"]);
					}
					if (wgMjController.IsElevator_Internal(ControllerSN))
					{
						mjRegisterCard.AllowFloors_internal = (ulong)dtPrivilege.Rows[l]["f_AllowFloors"];
					}
					Array.Copy(mjRegisterCard.ToBytes(), 4L, siiflash.data, num8, 8L);
					num8 += 8L;
					Array.Copy(mjRegisterCard.ToBytes(), 12L, siiflash.data, num11, 16L);
					num11 += 16L;
					if (flag3)
					{
						byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dtPrivilege.Rows[l]["f_ConsumerName"]));
						int num14 = 0;
						while (num14 < 32 && num14 < bytes.Length)
						{
							siiflash.data[(int)((IntPtr)((long)num14 + num13))] = bytes[num14];
							num14++;
						}
						if (bytes.Length > 0 && bytes.Length < 32)
						{
							siiflash.data[(int)((IntPtr)((long)bytes.Length + num13))] = 0;
						}
						num13 += 32L;
					}
					l++;
				}
				long num15 = 4096L + num8;
				long num16 = 8192L + num11;
				while (l < count)
				{
					if (wgMjControllerPrivilege.bStopUploadPrivilege)
					{
						return -100002;
					}
					mjRegisterCard.CardID = long.Parse(dtPrivilege.Rows[l]["f_CardNO"].ToString());
					mjRegisterCard.Password = uint.Parse(dtPrivilege.Rows[l]["f_PIN"].ToString());
					mjRegisterCard.ymdStart = (DateTime)dtPrivilege.Rows[l]["f_BeginYMD"];
					mjRegisterCard.ymdEnd = (DateTime)dtPrivilege.Rows[l]["f_EndYMD"];
					mjRegisterCard.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[l]["f_ControlSegID1"]);
					mjRegisterCard.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[l]["f_ControlSegID2"]);
					mjRegisterCard.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[l]["f_ControlSegID3"]);
					mjRegisterCard.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[l]["f_ControlSegID4"]);
					if (flag)
					{
						mjRegisterCard.FirstCardSet(1, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_1"] > 0);
						mjRegisterCard.FirstCardSet(2, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_2"] > 0);
						mjRegisterCard.FirstCardSet(3, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_3"] > 0);
						mjRegisterCard.FirstCardSet(4, (byte)dtPrivilege.Rows[l]["f_DoorFirstCard_4"] > 0);
					}
					if (flag2)
					{
						mjRegisterCard.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_1"]);
						mjRegisterCard.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_2"]);
						mjRegisterCard.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_3"]);
						mjRegisterCard.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[l]["f_MoreCards_GrpID_4"]);
					}
					if (wgMjController.IsElevator_Internal(ControllerSN))
					{
						mjRegisterCard.AllowFloors_internal = (ulong)dtPrivilege.Rows[l]["f_AllowFloors"];
					}
					Array.Copy(mjRegisterCard.ToBytes(), 4L, siiflash.data, num15, 8L);
					num15 += 8L;
					Array.Copy(mjRegisterCard.ToBytes(), 12L, siiflash.data, num16, 16L);
					num16 += 16L;
					if (flag3)
					{
						byte[] bytes2 = Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(dtPrivilege.Rows[l]["f_ConsumerName"]));
						int num17 = 0;
						while (num17 < 32 && num17 < bytes2.Length)
						{
							siiflash.data[(int)((IntPtr)((long)num17 + num13))] = bytes2[num17];
							num17++;
						}
						if (bytes2.Length > 0 && bytes2.Length < 32)
						{
							siiflash.data[(int)((IntPtr)((long)bytes2.Length + num13))] = 0;
						}
						num13 += 32L;
					}
					l++;
				}
				wgTools.WriteLine("下传卡号[4K+自由区] Start");
				int num18 = 0;
				array = null;
				while (num7 < num15 + 4096L)
				{
					if (wgMjControllerPrivilege.bStopUploadPrivilege)
					{
						return -100002;
					}
					wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)num7;
					wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
					Array.Copy(siiflash.data, num7, wgpacketSSI_FLASH_internal.ucData, 0L, 1024L);
					for (int m = 0; m < num2; m++)
					{
						if (array != null)
						{
							wgpacketSSI_FLASH_internal.GetNewXid();
						}
						array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
						wgTools.WriteLine(string.Format("{0:d}[{0:X4}]  {1:X8}", num7, (int)array[28] + ((int)array[29] << 8) + ((int)array[30] << 16) + ((int)array[31] << 24)));
						int num4 = this.wgudp.udp_get(array, this.m_priWaitMs, wgpacketSSI_FLASH_internal.xid, IP, Port, ref array2);
						if (array2 != null)
						{
							bool flag4 = true;
							int num19 = 0;
							while (num19 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num19 < array2.Length)
							{
								if (wgpacketSSI_FLASH_internal.ucData[num19] != array2[28 + num19])
								{
									wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardID = " + num7.ToString(), new object[0]);
									flag4 = false;
									break;
								}
								num19++;
							}
							if (flag4)
							{
								break;
							}
						}
						wgTools.WgDebugWrite(string.Concat(new string[]
						{
							ControllerSN.ToString(),
							" Upload Privileg startAdr4KCardID = ",
							num7.ToString(),
							", tries = ",
							m.ToString(),
							", m_priWaitMs = ",
							this.m_priWaitMs.ToString()
						}), new object[0]);
						Thread.Sleep(num3);
					}
					if (array2 == null)
					{
						wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed startAdr4KCardID = " + num7.ToString(), new object[0]);
						return -203;
					}
					num7 += 1024L;
					num18 += 64;
					if (num18 > count)
					{
						num18 = count;
					}
					this.DisplayProcessInfo(doorName, 100002, num18);
				}
				if (flag3)
				{
					while (num12 < num13 + 4096L)
					{
						if (wgMjControllerPrivilege.bStopUploadPrivilege)
						{
							return -100002;
						}
						wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)num12;
						wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
						Array.Copy(siiflash.data, num12, wgpacketSSI_FLASH_internal.ucData, 0L, 1024L);
						for (int n = 0; n < num2; n++)
						{
							if (array != null)
							{
								wgpacketSSI_FLASH_internal.GetNewXid();
							}
							array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
							wgTools.WriteLine(string.Format("{0:d}[{0:X4}]  {1:X8}", num12, (int)array[28] + ((int)array[29] << 8) + ((int)array[30] << 16) + ((int)array[31] << 24)));
							int num4 = this.wgudp.udp_get(array, this.m_priWaitMs, wgpacketSSI_FLASH_internal.xid, IP, Port, ref array2);
							if (array2 != null)
							{
								bool flag5 = true;
								int num20 = 0;
								while (num20 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num20 < array2.Length)
								{
									if (wgpacketSSI_FLASH_internal.ucData[num20] != array2[28 + num20])
									{
										wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg UserName Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardIDUserName = " + num12.ToString(), new object[0]);
										flag5 = false;
										break;
									}
									num20++;
								}
								if (flag5)
								{
									break;
								}
							}
							wgTools.WgDebugWrite(string.Concat(new string[]
							{
								ControllerSN.ToString(),
								" Upload Privileg UserName startAdr4KCardIDUserName = ",
								num12.ToString(),
								", tries = ",
								n.ToString(),
								", m_priWaitMs = ",
								this.m_priWaitMs.ToString()
							}), new object[0]);
							Thread.Sleep(num3);
						}
						if (array2 == null)
						{
							wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg UserName Failed startAdr4KCardIDUserName = " + num12.ToString(), new object[0]);
							return -203;
						}
						num12 += 1024L;
						num18 += 8;
						if (num18 > count)
						{
							num18 = count;
						}
						this.DisplayProcessInfo(doorName, 100002, num18);
					}
				}
				wgTools.WriteLine("下传卡信息区 Start");
				while (num10 < num16 + 4096L)
				{
					if (wgMjControllerPrivilege.bStopUploadPrivilege)
					{
						return -100002;
					}
					wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)num10;
					wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
					Array.Copy(siiflash.data, num10, wgpacketSSI_FLASH_internal.ucData, 0L, 1024L);
					for (int num21 = 0; num21 < num2; num21++)
					{
						if (array != null)
						{
							wgpacketSSI_FLASH_internal.GetNewXid();
						}
						array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
						wgTools.WriteLine(string.Format("startAdr4KCardIDInfo {0:d}[{0:X4}]", num10));
						int num4 = this.wgudp.udp_get(array, this.m_priWaitMs, wgpacketSSI_FLASH_internal.xid, IP, Port, ref array2);
						if (array2 != null)
						{
							bool flag6 = true;
							int num22 = 0;
							while (num22 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num22 < array2.Length)
							{
								if (wgpacketSSI_FLASH_internal.ucData[num22] != array2[28 + num22])
								{
									wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgpktWrite.ucData[i]!=recv[28+i] startAdr4KCardID = " + num7.ToString(), new object[0]);
									flag6 = false;
									break;
								}
								num22++;
							}
							if (flag6)
							{
								break;
							}
						}
						wgTools.WgDebugWrite(string.Concat(new string[]
						{
							ControllerSN.ToString(),
							" Upload Privileg startAdr4KCardIDInfo = ",
							num10.ToString(),
							", tries = ",
							num21.ToString(),
							", m_priWaitMs = ",
							this.m_priWaitMs.ToString()
						}), new object[0]);
						Thread.Sleep(num3);
					}
					if (array2 == null)
					{
						wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed startAdr4KCardIDInfo = " + num10.ToString(), new object[0]);
						return -203;
					}
					num10 += 1024L;
					num18 += 32;
					if (num18 > count)
					{
						num18 = count;
					}
					this.DisplayProcessInfo(doorName, 100002, num18);
				}
			}
			wgTools.WriteLine(string.Format("{0:d}权限上传完成", count));
			WGPacketWith1024 wgpacketWith = new WGPacketWith1024();
			wgpacketWith.type = 35;
			wgpacketWith.code = 48;
			wgpacketWith.iDevSnFrom = 0U;
			wgpacketWith.iDevSnTo = (uint)ControllerSN;
			wgpacketWith.iCallReturn = 0;
			int num23 = 0;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.newPrivilegePage4KAddr), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.freeNewPrivilegePageAddr), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.bOrderInfreePrivilegePage), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.totalPrivilegeCount), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			Array.Copy(BitConverter.GetBytes(mjRegisterCardsParam2.deletedPrivilegeCount), 0, wgpacketWith.ucData, num23, 4);
			num23 += 4;
			if (count > 92160)
			{
				Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
				Array.Copy(BitConverter.GetBytes(uint.MaxValue), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
			}
			else if (mjRegisterCardsParam2.iPrivilegeFirstIndexSSI_FLASH_PRIVILEGE_STARTADDR == 0U)
			{
				Array.Copy(BitConverter.GetBytes(819200), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
				Array.Copy(BitConverter.GetBytes(823296), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
			}
			else
			{
				Array.Copy(BitConverter.GetBytes(0), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
				Array.Copy(BitConverter.GetBytes(4096), 0, wgpacketWith.ucData, num23, 4);
				num23 += 4;
			}
			array = wgpacketWith.ToBytes(this.wgudp.udpPort);
			if (array == null)
			{
				wgTools.WriteLine("出错");
				return -206;
			}
			for (int num24 = 0; num24 < num2; num24++)
			{
				int num4 = this.wgudp.udp_get(array, this.m_priWaitMs, wgpacketWith.xid, IP, Port, ref array2);
				if (array2 != null)
				{
					break;
				}
				wgTools.WgDebugWrite(string.Concat(new string[]
				{
					ControllerSN.ToString(),
					" Upload Privileg wgparam , tries = ",
					num24.ToString(),
					", m_priWaitMs = ",
					this.m_priWaitMs.ToString()
				}), new object[0]);
				Thread.Sleep(num3);
			}
			if (array2 == null)
			{
				wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed wgparam ", new object[0]);
				return -203;
			}
			wgTools.WriteLine("权限表更新完成");
			this.DisplayProcessInfo(doorName, 100003, count);
			int num25 = count;
			wgTools.WriteLine("upload End");
			return num25;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000CEEEC File Offset: 0x000CDEEC
		internal int UploadIP_internal_IP64(int ControllerSN, string IP, int Port, string doorName, DataTable dtPrivilege, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return -100002;
			}
			if (dtPrivilege.Rows.Count >= 2)
			{
				long num = 0L;
				for (int i = 0; i < dtPrivilege.Rows.Count - 1; i++)
				{
					if ((long)dtPrivilege.Rows[i]["f_CardNO"] <= num)
					{
						return -200;
					}
					num = (long)dtPrivilege.Rows[i]["f_CardNO"];
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (dtPrivilege.Columns.Contains("f_DoorFirstCard_1"))
			{
				flag = true;
			}
			if (dtPrivilege.Columns.Contains("f_MoreCards_GrpID_1"))
			{
				flag2 = true;
			}
			int count = dtPrivilege.Rows.Count;
			dtPrivilege.Columns.Contains("f_ConsumerName");
			wgTools.WriteLine("upload Start");
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			MjRegisterCardsParam mjRegisterCardsParam = new MjRegisterCardsParam();
			if (wgMjControllerPrivilege.bStopUploadPrivilege)
			{
				return -100002;
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			int count2 = dtPrivilege.Rows.Count;
			if ((long)count2 > (long)((ulong)mjRegisterCardsParam.MaxPrivilegesNum))
			{
				this.DisplayProcessInfo(doorName, -100001, count2);
				return -100001;
			}
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 86;
			wgpacketShort.iDevSn = (uint)ControllerSN;
			byte[] array = null;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			if (count2 == 0)
			{
				wgpacketShort.code = 84;
				wgTools.LongTo4Bytes(ref wgpacketShort.data, 4, 1437248085L);
				byte[] array2 = wgpacketShort.ToBytes();
				for (int j = 0; j < 2; j++)
				{
					this.wgudp.udp_get(array2, this.m_priWaitMs, wgpacketShort.xid, IP, Port, ref array);
					if (array != null)
					{
						bool flag3 = false;
						if (array[8] == 1)
						{
							flag3 = true;
						}
						else if (array[8] == 225)
						{
							break;
						}
						if (flag3)
						{
							break;
						}
					}
					string[] array3 = new string[]
					{
						ControllerSN.ToString(),
						" Upload Privileg startAdr4KCardID = ",
						9999.ToString(),
						", tries = ",
						j.ToString(),
						", m_priWaitMs = ",
						this.m_priWaitMs.ToString()
					};
					wgTools.WgDebugWrite(string.Concat(array3), new object[0]);
					Thread.Sleep(300);
				}
			}
			else
			{
				int k = 0;
				MjRegisterCard mjRegisterCard = new MjRegisterCard();
				while (k < count2)
				{
					if (wgMjControllerPrivilege.bStopUploadPrivilege)
					{
						return -100002;
					}
					mjRegisterCard.CardID = long.Parse(dtPrivilege.Rows[k]["f_CardNO"].ToString());
					mjRegisterCard.Password = uint.Parse(dtPrivilege.Rows[k]["f_PIN"].ToString());
					mjRegisterCard.ymdStart = (DateTime)dtPrivilege.Rows[k]["f_BeginYMD"];
					mjRegisterCard.ymdEnd = (DateTime)dtPrivilege.Rows[k]["f_EndYMD"];
					mjRegisterCard.ControlSegIndexSet(1, (byte)dtPrivilege.Rows[k]["f_ControlSegID1"]);
					mjRegisterCard.ControlSegIndexSet(2, (byte)dtPrivilege.Rows[k]["f_ControlSegID2"]);
					mjRegisterCard.ControlSegIndexSet(3, (byte)dtPrivilege.Rows[k]["f_ControlSegID3"]);
					mjRegisterCard.ControlSegIndexSet(4, (byte)dtPrivilege.Rows[k]["f_ControlSegID4"]);
					if (flag)
					{
						mjRegisterCard.FirstCardSet(1, (byte)dtPrivilege.Rows[k]["f_DoorFirstCard_1"] > 0);
						mjRegisterCard.FirstCardSet(2, (byte)dtPrivilege.Rows[k]["f_DoorFirstCard_2"] > 0);
						mjRegisterCard.FirstCardSet(3, (byte)dtPrivilege.Rows[k]["f_DoorFirstCard_3"] > 0);
						mjRegisterCard.FirstCardSet(4, (byte)dtPrivilege.Rows[k]["f_DoorFirstCard_4"] > 0);
					}
					if (flag2)
					{
						mjRegisterCard.MoreCardGroupIndexSet(1, (byte)dtPrivilege.Rows[k]["f_MoreCards_GrpID_1"]);
						mjRegisterCard.MoreCardGroupIndexSet(2, (byte)dtPrivilege.Rows[k]["f_MoreCards_GrpID_2"]);
						mjRegisterCard.MoreCardGroupIndexSet(3, (byte)dtPrivilege.Rows[k]["f_MoreCards_GrpID_3"]);
						mjRegisterCard.MoreCardGroupIndexSet(4, (byte)dtPrivilege.Rows[k]["f_MoreCards_GrpID_4"]);
					}
					if (wgMjController.IsElevator_Internal(ControllerSN))
					{
						mjRegisterCard.AllowFloors_internal = (ulong)dtPrivilege.Rows[k]["f_AllowFloors"];
					}
					Array.Copy(mjRegisterCard.ToBytes(), 4, wgpacketShort.data, 0, MjRegisterCard.byteLen);
					Array.Copy(mjRegisterCard.ToBytes(), 12, wgpacketShort.data, 40, MjRegisterCard.byteLen - 8);
					wgpacketShort.data[24] = (byte)(count2 & 255);
					wgpacketShort.data[25] = (byte)((count2 >> 8) & 255);
					wgpacketShort.data[26] = (byte)((count2 >> 16) & 255);
					wgpacketShort.data[27] = (byte)((k + 1) & 255);
					wgpacketShort.data[28] = (byte)((k + 1 >> 8) & 255);
					wgpacketShort.data[29] = (byte)((k + 1 >> 16) & 255);
					byte[] array4 = wgpacketShort.ToBytes();
					for (int l = 0; l < 3; l++)
					{
						this.wgudp.udp_get(array4, 6000, wgpacketShort.xid, IP, Port, ref array);
						if (array != null)
						{
							bool flag4 = false;
							if (array[8] == 1)
							{
								flag4 = true;
							}
							else if (array[8] == 225)
							{
								break;
							}
							if (flag4)
							{
								break;
							}
						}
						string[] array5 = new string[]
						{
							ControllerSN.ToString(),
							" Upload Privileg startAdr4KCardID = ",
							9999.ToString(),
							", tries = ",
							l.ToString(),
							", m_priWaitMs = ",
							this.m_priWaitMs.ToString()
						};
						wgTools.WgDebugWrite(string.Concat(array5), new object[0]);
						Thread.Sleep(300);
						Thread.Sleep(12000 * (1 + l));
					}
					if (array == null)
					{
						wgTools.WgDebugWrite(ControllerSN.ToString() + " Upload Privileg Failed startAdr4KCardID = " + 9999.ToString(), new object[0]);
						string[] array6 = new string[]
						{
							ControllerSN.ToString(),
							" Upload Privileg Failed startAdr4KCardID = ",
							9999.ToString(),
							"  recIndex=",
							k.ToString()
						};
						wgAppConfig.wgLogWithoutDB(string.Concat(array6));
						return -203;
					}
					k++;
					this.DisplayProcessInfo(doorName, 100002, k);
					wgTools.delay4SendNextCommand_P64();
				}
			}
			this.DisplayProcessInfo(doorName, 100003, count2);
			int num2 = count2;
			wgTools.WriteLine("upload End");
			return num2;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x000CF693 File Offset: 0x000CE693
		public static bool bStopDownloadPrivilege
		{
			get
			{
				return wgMjControllerPrivilege.m_bStopDownloadPrivilege;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x000CF69A File Offset: 0x000CE69A
		public static bool bStopUploadPrivilege
		{
			get
			{
				return wgMjControllerPrivilege.m_bStopUploadPrivilege;
			}
		}

		// Token: 0x04000D21 RID: 3361
		private const int iPrivilegeCountOn4KPage = 512;

		// Token: 0x04000D22 RID: 3362
		public bool bAllowUploadUserName;

		// Token: 0x04000D23 RID: 3363
		private static bool m_bStopDownloadPrivilege;

		// Token: 0x04000D24 RID: 3364
		private static bool m_bStopUploadPrivilege;

		// Token: 0x04000D25 RID: 3365
		private int m_priWaitMs = 1000;

		// Token: 0x04000D26 RID: 3366
		private readonly int m_uploadUserNameMaxlen = 3700;

		// Token: 0x04000D27 RID: 3367
		private readonly int m_userNamePageHalfStart = 4720;

		// Token: 0x04000D28 RID: 3368
		private readonly int m_userNamePageStart = 4600;

		// Token: 0x04000D29 RID: 3369
		private wgUdpComm wgudp;
	}
}
