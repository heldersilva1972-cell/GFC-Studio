using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F3 RID: 499
	public class wgMjController : IDisposable
	{
		// Token: 0x06000C79 RID: 3193 RVA: 0x000FB812 File Offset: 0x000FA812
		public int AdjustTimeIP(DateTime dateTimeNew)
		{
			return this.AdjustTimeIP_internal(dateTimeNew, "");
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000FB820 File Offset: 0x000FA820
		public int AdjustTimeIP(DateTime dateTimeNew, string PCIPAddr)
		{
			return this.AdjustTimeIP_internal(dateTimeNew, PCIPAddr);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000FB82C File Offset: 0x000FA82C
		internal int AdjustTimeIP_internal(DateTime dateTimeNew, string PCIPAddr)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.AdjustTimeIP_internal64(dateTimeNew, PCIPAddr);
			}
			WGPacketBasicAdjustTimeToSend wgpacketBasicAdjustTimeToSend = new WGPacketBasicAdjustTimeToSend(dateTimeNew);
			wgpacketBasicAdjustTimeToSend.type = 32;
			wgpacketBasicAdjustTimeToSend.code = 48;
			wgpacketBasicAdjustTimeToSend.iDevSnFrom = 0U;
			wgpacketBasicAdjustTimeToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicAdjustTimeToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicAdjustTimeToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicAdjustTimeToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000FB8EC File Offset: 0x000FA8EC
		public int AdjustTimeIP_internal64(DateTime dateTimeNew, string PCIPAddr)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 48;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			DateTime dateTime = dateTimeNew;
			wgpacketShort.data[0] = (byte)this.GetHex((dateTime.Year - dateTime.Year % 100) / 100);
			wgpacketShort.data[1] = (byte)this.GetHex(dateTime.Year % 100);
			wgpacketShort.data[2] = (byte)this.GetHex(dateTime.Month);
			wgpacketShort.data[3] = (byte)this.GetHex(dateTime.Day);
			wgpacketShort.data[4] = (byte)this.GetHex(dateTime.Hour);
			wgpacketShort.data[5] = (byte)this.GetHex(dateTime.Minute);
			wgpacketShort.data[6] = (byte)this.GetHex(dateTime.Second);
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketShort.ToBytes();
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketShort.sequenceId, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000FBA28 File Offset: 0x000FAA28
		public int AdjustTimeIP_TCP(DateTime dateTimeNew)
		{
			byte[] array = new WGPacketBasicAdjustTimeToSend(dateTimeNew)
			{
				type = 32,
				code = 48,
				iDevSnFrom = 0U,
				iDevSnTo = (uint)this.m_ControllerSN,
				iCallReturn = 0
			}.ToBytes((ushort)(this.tcp.Client.LocalEndPoint as IPEndPoint).Port);
			if (array == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			NetworkStream stream = this.tcp.GetStream();
			if (stream.CanWrite)
			{
				stream.Write(array, 0, array.Length);
			}
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			byte[] array2 = new byte[2000];
			int num = 0;
			while (dateTime > DateTime.Now)
			{
				if (stream.CanRead && stream.DataAvailable)
				{
					num = stream.Read(array2, 0, array2.Length);
					break;
				}
			}
			if (num > 0)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000FBB2C File Offset: 0x000FAB2C
		public int AutoIPSetIP(int cmdOption, string strIP, string strMask, string strGateway, int ipstart, int ipend)
		{
			WGPacketBasicAutoIPSetToSend wgpacketBasicAutoIPSetToSend = new WGPacketBasicAutoIPSetToSend((uint)cmdOption, strIP, strMask, strGateway, (uint)ipstart, (uint)ipend);
			wgpacketBasicAutoIPSetToSend.type = 32;
			wgpacketBasicAutoIPSetToSend.code = 192;
			wgpacketBasicAutoIPSetToSend.iDevSnFrom = 0U;
			wgpacketBasicAutoIPSetToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicAutoIPSetToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicAutoIPSetToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicAutoIPSetToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x000FBBE8 File Offset: 0x000FABE8
		public int chkFalseControlIP()
		{
			if (!wgAppConfig.IsAccessControlBlue || !wgAppConfig.checkRSAController((long)this.ControllerSN))
			{
				byte[] array = new byte[1644];
				int num = Math.Min(992 - (this.ControllerSN & 31), 895);
				if (this.GetControlDataIP_internal(197504 - num, 1427898384 + num, ref array) > 0)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, intPtr, array.Length);
					int falseCnt = wgMjController.getFalseCnt(intPtr, this.ControllerSN, num);
					Marshal.FreeHGlobal(intPtr);
					return falseCnt;
				}
			}
			return -1;
		}

		// Token: 0x06000C80 RID: 3200
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int dec(IntPtr pwgPkt, int len, IntPtr k);

		// Token: 0x06000C81 RID: 3201 RVA: 0x000FBC74 File Offset: 0x000FAC74
		private void DisplayProcessInfo(string info, int infoCode, string specialInfo)
		{
			switch (infoCode)
			{
			case 1:
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2}]", info, CommonStr.strFingerprintUploadCard, specialInfo));
				return;
			case 2:
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2}]", info, CommonStr.strFingerprintUploadFingerprint, specialInfo));
				return;
			case 3:
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}: {1}[{2}]", info, CommonStr.strFingerprintSyncFingerprint, specialInfo));
				return;
			default:
				wgAppRunInfo.raiseAppRunInfoCommStatus(info);
				return;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000FBCE4 File Offset: 0x000FACE4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000FBCF3 File Offset: 0x000FACF3
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.wgudp != null)
				{
					this.wgudp.Close();
					this.wgudp.Dispose();
				}
				if (this.tcp != null)
				{
					this.tcp.Close();
				}
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000FBD2C File Offset: 0x000FAD2C
		public int DrvUpgradeIP()
		{
			WGPacketBasicGetSingleSwipeRecordToSend wgpacketBasicGetSingleSwipeRecordToSend = new WGPacketBasicGetSingleSwipeRecordToSend(1437248085U);
			wgpacketBasicGetSingleSwipeRecordToSend.type = 32;
			wgpacketBasicGetSingleSwipeRecordToSend.code = 254;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnFrom = 0U;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicGetSingleSwipeRecordToSend.iCallReturn = 0;
			int num = -13;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicGetSingleSwipeRecordToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return num;
			}
			return this.wgudp.udp_get_notries(array2, 300, wgpacketBasicGetSingleSwipeRecordToSend.xid, this.m_IP, this.m_PORT, ref array);
		}

		// Token: 0x06000C85 RID: 3205
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int enc(IntPtr pwgPkt, int len, IntPtr k);

		// Token: 0x06000C86 RID: 3206 RVA: 0x000FBDC8 File Offset: 0x000FADC8
		private void EncWGPacket(ref byte[] pwgPktBytes, int len, string commP)
		{
			if (len >= 4)
			{
				byte[] array = new byte[16];
				char[] array2 = commP.PadRight(16, '\0').ToCharArray();
				for (int i = 0; i < 16; i++)
				{
					array[i] = (byte)(array2[i] & 'ÿ');
				}
				IntPtr intPtr = Marshal.AllocHGlobal(len);
				IntPtr intPtr2 = Marshal.AllocHGlobal(16);
				Marshal.Copy(pwgPktBytes, 0, intPtr, len);
				Marshal.Copy(array, 0, intPtr2, 16);
				wgMjController.enc(intPtr, len, intPtr2);
				Marshal.Copy(intPtr, pwgPktBytes, 0, len);
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000FBE50 File Offset: 0x000FAE50
		public int FingerConfigureGetImageIP(byte[] dataParam, byte[] dataReply)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 96;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			dataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 600, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				int num = 0;
				while (num < 1152 && num < dataReply.Length && num < array.Length - 20)
				{
					dataReply[num] = array[num + 20];
					num++;
				}
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000FBF34 File Offset: 0x000FAF34
		public int FingerConfigureGetImageIPNotries(byte[] dataParam, byte[] dataReply)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 96;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			dataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 600, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				int num = 0;
				while (num < 1152 && num < dataReply.Length && num < array.Length - 20)
				{
					dataReply[num] = array[num + 20];
					num++;
				}
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000FC018 File Offset: 0x000FB018
		public int FingerConfigureIP(byte[] dataParam, byte[] dataReply)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 96;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			dataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = null;
			for (int i = 0; i < this.priMaxTries; i++)
			{
				if (array != null)
				{
					wgpacketWith1152_internal.GetNewXid();
				}
				array = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
				if (array == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				if (this.bStopFingerprintComm)
				{
					return -1;
				}
				this.wgudp.udp_get(array, 1000, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array2);
				if (array2 != null)
				{
					break;
				}
				Thread.Sleep(this.sleep4Tries);
			}
			if (array2 != null)
			{
				int num = 0;
				while (num < 1152 && num < dataReply.Length && num < array2.Length - 20)
				{
					dataReply[num] = array2[num + 20];
					num++;
				}
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x000FC138 File Offset: 0x000FB138
		public int FingerGetCountIP()
		{
			byte[] array = new byte[1152];
			byte[] array2 = new byte[1152];
			for (int i = 0; i < 1152; i++)
			{
				array[i] = 0;
				array2[i] = 0;
			}
			array[0] = 241;
			array[2] = 8;
			int num = this.FingerConfigureIP(array, array2);
			if (num > 0)
			{
				this.fingerTotal = (int)array2[7] + ((int)array2[8] << 8) + ((int)array2[9] << 16) + ((int)array2[10] << 24);
				this.fingerTotalValid = (int)array2[11] + ((int)array2[12] << 8) + ((int)array2[13] << 16) + ((int)array2[14] << 24);
			}
			return num;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000FC1D0 File Offset: 0x000FB1D0
		public int FormatIP()
		{
			byte[] array = new byte[1152];
			array[1020] = 138;
			array[1022] = 154;
			array[1024] = 138;
			array[1026] = 154;
			array[1027] = 165;
			array[1026] = 165;
			array[1025] = 165;
			array[1024] = 165;
			array[1028] = 137;
			array[1029] = 152;
			array[1030] = 137;
			array[1031] = 152;
			if (wgMjController.GetControllerType(this.ControllerSN) > 0)
			{
				this.UpdateConfigureSuperIP_internal(array);
				return 1;
			}
			return -1;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x000FC291 File Offset: 0x000FB291
		private byte[] getByteOfPassword(string strPassword)
		{
			return Encoding.GetEncoding("utf-8").GetBytes(wgTools.SetObjToStr(strPassword));
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000FC2A8 File Offset: 0x000FB2A8
		public int GetConfigureInFlashIP(ref wgMjControllerConfigure controlConfigure)
		{
			try
			{
				byte[] array = null;
				if (this.GetConfigureIP(ref array, false) == 1 && array != null)
				{
					controlConfigure = new wgMjControllerConfigure(array, 20);
					this.m_MACAddr = controlConfigure.MACAddr;
					return 1;
				}
			}
			catch (Exception)
			{
			}
			return -1;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000FC2FC File Offset: 0x000FB2FC
		public int GetConfigureIP(ref wgMjControllerConfigure controlConfigure)
		{
			try
			{
				byte[] array = null;
				if (this.GetConfigureIP(ref array, true) == 1 && array != null)
				{
					controlConfigure = new wgMjControllerConfigure(array, 20);
					this.m_MACAddr = controlConfigure.MACAddr;
					return 1;
				}
			}
			catch (Exception)
			{
			}
			return -1;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000FC350 File Offset: 0x000FB350
		public int GetConfigureIP(ref byte[] controlConfigureData, bool defaultParam)
		{
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			if (!defaultParam)
			{
				wgpacket.code = 80;
			}
			if (wgpacket.code == 16 && wgTools.doubleParse(this.m_runinfo.driverVersion.Substring(1)) >= 6.64)
			{
				wgpacket.code = 19;
			}
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacket.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				BitConverter.ToString(array);
				controlConfigureData = array;
				return 1;
			}
			return -1;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x000FC444 File Offset: 0x000FB444
		public static byte[] GetControlDataBroadcast(int start, int len)
		{
			return new WGPacketCPU_CONFIG_READToSend((uint)start, (uint)len)
			{
				type = 37,
				code = 16,
				iDevSnFrom = 0U,
				iDevSnTo = uint.MaxValue,
				iCallReturn = 0
			}.ToBytesCPUConfigNoPassword(60000);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x000FC489 File Offset: 0x000FB489
		public int GetControlDataIP(int start, int len, ref byte[] data)
		{
			return this.GetControlDataIP_internal(start, len, ref data);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000FC494 File Offset: 0x000FB494
		private int GetControlDataIP_internal(int start, int len, ref byte[] data)
		{
			WGPacketCPU_CONFIG_READToSend wgpacketCPU_CONFIG_READToSend = new WGPacketCPU_CONFIG_READToSend((uint)start, (uint)len);
			wgpacketCPU_CONFIG_READToSend.type = 37;
			wgpacketCPU_CONFIG_READToSend.code = 16;
			wgpacketCPU_CONFIG_READToSend.iDevSnFrom = 0U;
			wgpacketCPU_CONFIG_READToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketCPU_CONFIG_READToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketCPU_CONFIG_READToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketCPU_CONFIG_READToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			int num = 0;
			while (num < array.Length && num < data.Length)
			{
				data[num] = array[num];
				num++;
			}
			return 1;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x000FC560 File Offset: 0x000FB560
		public int GetControlDataNoPasswordIP(int start, int len, ref byte[] data)
		{
			WGPacketCPU_CONFIG_READToSend wgpacketCPU_CONFIG_READToSend = new WGPacketCPU_CONFIG_READToSend((uint)start, (uint)len);
			wgpacketCPU_CONFIG_READToSend.type = 37;
			wgpacketCPU_CONFIG_READToSend.code = 16;
			wgpacketCPU_CONFIG_READToSend.iDevSnFrom = 0U;
			wgpacketCPU_CONFIG_READToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketCPU_CONFIG_READToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketCPU_CONFIG_READToSend.ToBytesCPUConfigNoPassword(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketCPU_CONFIG_READToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			int num = 0;
			while (num < array.Length && num < data.Length)
			{
				data[num] = array[num];
				num++;
			}
			return 1;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x000FC62C File Offset: 0x000FB62C
		public int GetControlFullDataNoPasswordIP(int start, int len, ref byte[] data)
		{
			int num = (((len >> 16) & 255) << 24) + (((len >> 24) & 255) << 16) + (len & 65535);
			WGPacketCPU_CONFIG_READToSend wgpacketCPU_CONFIG_READToSend = new WGPacketCPU_CONFIG_READToSend((uint)start, (uint)num);
			wgpacketCPU_CONFIG_READToSend.type = 37;
			wgpacketCPU_CONFIG_READToSend.code = 80;
			wgpacketCPU_CONFIG_READToSend.iDevSnFrom = 0U;
			wgpacketCPU_CONFIG_READToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketCPU_CONFIG_READToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketCPU_CONFIG_READToSend.ToBytesCPUConfigNoPassword(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketCPU_CONFIG_READToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			int num2 = 0;
			while (num2 < array.Length && num2 < data.Length)
			{
				data[num2] = array[num2];
				num2++;
			}
			return 1;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x000FC721 File Offset: 0x000FB721
		public static int GetControllerReaderNum(int controllerSN)
		{
			if (wgMjController.IsElevator_Internal(controllerSN))
			{
				return 1;
			}
			if (controllerSN >= 100000000)
			{
				if (controllerSN <= 199999999)
				{
					return 2;
				}
				if (controllerSN <= 299999999)
				{
					return 4;
				}
				if (controllerSN <= 399999999)
				{
					return 0;
				}
				if (controllerSN <= 499999999)
				{
					return 4;
				}
			}
			return 0;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x000FC760 File Offset: 0x000FB760
		public int GetControllerRunInformationIP64(ref byte[] recvInfo, string PCIPAddr, ref ControllerRunInformation icconRunInfo, int UDPQueue4MultithreadIndex = -1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 32;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = wgpacketShort.ToBytes();
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			int num = this.wgudp.udp_get(array2, 300, wgpacketShort.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null && array.Length == 64)
			{
				recvInfo = array;
				if (this.m_ControllerSN != -1)
				{
					this.m_runinfo.UpdateInfo_internal64(array, 0, (uint)this.m_ControllerSN);
					if (icconRunInfo != null)
					{
						icconRunInfo.UpdateInfo_internal64(array, 0, (uint)this.m_ControllerSN);
						icconRunInfo.updateFirstP64(array, 0, (uint)this.m_ControllerSN);
					}
				}
				else
				{
					uint num2 = (uint)((int)array[4] + ((int)array[5] << 8) + ((int)array[6] << 16) + ((int)array[7] << 24));
					this.m_runinfo.UpdateInfo_internal64(array, 0, num2);
					if (icconRunInfo != null)
					{
						icconRunInfo.UpdateInfo_internal64(array, 0, num2);
						icconRunInfo.updateFirstP64(array, 0, num2);
					}
				}
				wgpacketShort.code = 88;
				array2 = wgpacketShort.ToBytes();
				num = this.wgudp.udp_get(array2, 300, wgpacketShort.xid, this.m_IP, this.m_PORT, ref array);
			}
			else
			{
				wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			}
			if (array != null && array.Length == 64)
			{
				this.m_runinfo.UpdateInfo_registercard_internal64(array, 0, (uint)this.m_ControllerSN);
				if (icconRunInfo != null)
				{
					icconRunInfo.UpdateInfo_registercard_internal64(array, 0, (uint)this.m_ControllerSN);
				}
				wgpacketShort.code = 180;
				array2 = wgpacketShort.ToBytes();
				num = this.wgudp.udp_get(array2, 300, wgpacketShort.xid, this.m_IP, this.m_PORT, ref array);
				if (num > 0)
				{
					icconRunInfo.lastGetRecordIndex = BitConverter.ToUInt32(array, 8);
				}
			}
			return num;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000FC930 File Offset: 0x000FB930
		public static int GetControllerType(int controllerSN)
		{
			if (controllerSN > 100000000)
			{
				if (controllerSN <= 199999999)
				{
					return 1;
				}
				if (controllerSN <= 299999999)
				{
					return 2;
				}
				if (controllerSN <= 399999999)
				{
					return 0;
				}
				if (controllerSN <= 499999999)
				{
					return 4;
				}
			}
			return 0;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000FC964 File Offset: 0x000FB964
		public int GetControlTaskListIP(ref byte[] controlTaskListData)
		{
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = new byte[4096];
			for (int i = 0; i < 4096; i += 1024)
			{
				WGPacketSSI_FLASH_QUERY_internal wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)this.m_ControllerSN, (uint)(MjControlTaskItem.flashStartAddr_internal + i), (uint)(MjControlTaskItem.flashStartAddr_internal + i + 1024 - 1));
				byte[] array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.wgudp.udpPort);
				if (array3 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				this.wgudp.udp_get(array3, 1000, wgpacketSSI_FLASH_QUERY_internal.xid, this.m_IP, this.m_PORT, ref array);
				if (array == null)
				{
					return -1;
				}
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				Array.Copy(array, 28, array2, i, 1024);
			}
			controlTaskListData = array2;
			return 1;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000FCA44 File Offset: 0x000FBA44
		public string GetEthStatsIP(ref string Desc)
		{
			WGPacketBasicGetSingleSwipeRecordToSend wgpacketBasicGetSingleSwipeRecordToSend = new WGPacketBasicGetSingleSwipeRecordToSend(0U);
			wgpacketBasicGetSingleSwipeRecordToSend.type = 32;
			wgpacketBasicGetSingleSwipeRecordToSend.code = 252;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnFrom = 0U;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicGetSingleSwipeRecordToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicGetSingleSwipeRecordToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return "";
			}
			this.wgudp.udp_get_notries(array2, 300, wgpacketBasicGetSingleSwipeRecordToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				string text = "";
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				text += string.Format("控制器SN: {0}\r\n", num);
				int num2 = 28;
				text += string.Format("总的中断数: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 20;
				text += string.Format("接收中断数: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 24;
				text += string.Format("发送中断数: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 72;
				text += string.Format("接收的错误包rx_errors: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 68;
				text += string.Format("无效arp包数: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 76;
				text += string.Format("包长无效的包bad_plen: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 32;
				text += string.Format("冲突数coll1: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 36;
				text += string.Format("冲突数collx: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 40;
				text += string.Format("冲突数overrun: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 44;
				text += string.Format("发送队列max: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 48;
				text += string.Format("发送队列len当前: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 52;
				text += string.Format("接收队列 入: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 56;
				text += string.Format("接收队列 出: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 60;
				text += string.Format("发送队列 入: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				num2 = 64;
				text += string.Format("发送队列 出: {0}\r\n", (uint)((int)array[num2] + ((int)array[num2 + 1] << 8) + ((int)array[num2 + 2] << 16) + ((int)array[num2 + 3] << 24)));
				Desc = text;
				return text + "获取控制器网络统计信息:\r\n" + BitConverter.ToString(array);
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return "";
		}

		// Token: 0x06000C9A RID: 3226
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getFalseCnt(IntPtr pwgPkt, int sn, int val);

		// Token: 0x06000C9B RID: 3227
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getGotIndex(IntPtr pwgPkt, int sn, int val);

		// Token: 0x06000C9C RID: 3228 RVA: 0x000FCEDA File Offset: 0x000FBEDA
		private int GetHex(int val)
		{
			return val % 10 + (val - val % 10) / 10 % 10 * 16;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x000FCEF0 File Offset: 0x000FBEF0
		public int GetHolidayListIP(ref byte[] holidayListData)
		{
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = new byte[4096];
			for (int i = 0; i < 4096; i += 1024)
			{
				WGPacketSSI_FLASH_QUERY_internal wgpacketSSI_FLASH_QUERY_internal = new WGPacketSSI_FLASH_QUERY_internal(33, 16, (uint)this.m_ControllerSN, (uint)(MjControlHolidayTime.flashStartAddr_internal + i), (uint)(MjControlHolidayTime.flashStartAddr_internal + i + 1024 - 1));
				byte[] array3 = wgpacketSSI_FLASH_QUERY_internal.ToBytes(this.wgudp.udpPort);
				if (array3 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				this.wgudp.udp_get(array3, 1000, wgpacketSSI_FLASH_QUERY_internal.xid, this.m_IP, this.m_PORT, ref array);
				if (array == null)
				{
					return -1;
				}
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				Array.Copy(array, 28, array2, i, 1024);
			}
			holidayListData = array2;
			return 1;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000FCFD0 File Offset: 0x000FBFD0
		public int GetInfo_BaseComm_IP64(ref byte[] recvInfo, byte[] bytToSend, uint xid, int UDPQueue4MultithreadIndex = -1)
		{
			int num = -1;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			if (wgTools.bUDPOnly64 > 0)
			{
				for (int i = 0; i < 3; i++)
				{
					num = this.wgudp.udp_get(bytToSend, 6000, xid, this.m_IP, this.m_PORT, ref array);
					if (array != null)
					{
						break;
					}
					Thread.Sleep(300);
					Thread.Sleep(12000 * (1 + i));
				}
			}
			else
			{
				num = this.wgudp.udp_get(bytToSend, 300, xid, this.m_IP, this.m_PORT, ref array);
			}
			if (array != null && array.Length == 64)
			{
				recvInfo = array;
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return num;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x000FD088 File Offset: 0x000FC088
		public int GetInfo_RecordGotIndex_IP64(ref byte[] recvInfo, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 180;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			int num = this.GetInfo_BaseComm_IP64(ref recvInfo, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex);
			if (num > 0)
			{
				num = (int)BitConverter.ToUInt32(recvInfo, 8);
			}
			return num;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000FD0D8 File Offset: 0x000FC0D8
		public int GetInfo_Swipe_IP64(ref byte[] recvInfo, long recordIndexToGet, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 176;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.data[0] = (byte)(recordIndexToGet & 255L);
			wgpacketShort.data[1] = (byte)((recordIndexToGet >> 8) & 255L);
			wgpacketShort.data[2] = (byte)((recordIndexToGet >> 16) & 255L);
			wgpacketShort.data[3] = (byte)((recordIndexToGet >> 24) & 255L);
			return this.GetInfo_BaseComm_IP64(ref recvInfo, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000FD164 File Offset: 0x000FC164
		public int GetInfo_SwipeIndex_IP64(ref byte[] recvInfo, long recordIndexToGet, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 176;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.data[0] = (byte)(recordIndexToGet & 255L);
			wgpacketShort.data[1] = (byte)((recordIndexToGet >> 8) & 255L);
			wgpacketShort.data[2] = (byte)((recordIndexToGet >> 16) & 255L);
			wgpacketShort.data[3] = (byte)((recordIndexToGet >> 24) & 255L);
			int num = this.GetInfo_BaseComm_IP64(ref recvInfo, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex);
			if (num > 0)
			{
				num = (int)BitConverter.ToUInt32(recvInfo, 8);
			}
			return num;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x000FD200 File Offset: 0x000FC200
		public int GetInfo_UpdateLastGetRecordLocationIP_IP64(ref byte[] recvInfo, long recordIndexValidGet, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 178;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.data[0] = (byte)(recordIndexValidGet & 255L);
			wgpacketShort.data[1] = (byte)((recordIndexValidGet >> 8) & 255L);
			wgpacketShort.data[2] = (byte)((recordIndexValidGet >> 16) & 255L);
			wgpacketShort.data[3] = (byte)((recordIndexValidGet >> 24) & 255L);
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 4, 1437248085L);
			return this.GetInfo_BaseComm_IP64(ref recvInfo, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x000FD2A0 File Offset: 0x000FC2A0
		private int GetMjControllerRunInformationIP()
		{
			byte[] array = null;
			return this.GetMjControllerRunInformationIP(ref array);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000FD2B7 File Offset: 0x000FC2B7
		public int GetMjControllerRunInformationIP(ref byte[] recvInfo)
		{
			return this.GetMjControllerRunInformationIP_internal(ref recvInfo, "", -1);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x000FD2C6 File Offset: 0x000FC2C6
		public int GetMjControllerRunInformationIP(ref byte[] recvInfo, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			return this.GetMjControllerRunInformationIP_internal(ref recvInfo, PCIPAddr, UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000FD2D4 File Offset: 0x000FC2D4
		internal int GetMjControllerRunInformationIP_internal(ref byte[] recvInfo, string PCIPAddr, int UDPQueue4MultithreadIndex = -1)
		{
			WGPacketBasicRunInformationToSend wgpacketBasicRunInformationToSend = new WGPacketBasicRunInformationToSend();
			wgpacketBasicRunInformationToSend.type = 32;
			wgpacketBasicRunInformationToSend.code = 16;
			wgpacketBasicRunInformationToSend.iDevSnFrom = 0U;
			wgpacketBasicRunInformationToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRunInformationToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRunInformationToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRunInformationToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			recvInfo = array;
			if (this.m_ControllerSN != -1)
			{
				this.m_runinfo.UpdateInfo_internal(array, 20, (uint)this.m_ControllerSN);
			}
			else
			{
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				this.m_runinfo.UpdateInfo_internal(array, 20, num);
			}
			int num2 = 0;
			while (num2 < 10 && this.m_runinfo.newSwipes[num2].indexInDataFlash_Internal != 4294967295U)
			{
				num2++;
			}
			return 1;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000FD408 File Offset: 0x000FC408
		public int GetMjControllerRunInformationIP_TCP(string strIP, ref byte[] recvInfo)
		{
			if (this.tcp == null)
			{
				try
				{
					this.tcp = new TcpClient();
					this.tcp.Connect(IPAddress.Parse(strIP), this.PORT);
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					this.tcp = null;
				}
			}
			if (this.tcp != null)
			{
				WGPacketBasicRunInformationToSend wgpacketBasicRunInformationToSend = new WGPacketBasicRunInformationToSend();
				wgpacketBasicRunInformationToSend.type = 32;
				wgpacketBasicRunInformationToSend.code = 16;
				wgpacketBasicRunInformationToSend.iDevSnFrom = 0U;
				wgpacketBasicRunInformationToSend.iDevSnTo = (uint)this.m_ControllerSN;
				wgpacketBasicRunInformationToSend.iCallReturn = 0;
				byte[] array = null;
				this.tcp.ReceiveBufferSize = 2000;
				this.tcp.SendBufferSize = 2000;
				byte[] array2 = wgpacketBasicRunInformationToSend.ToBytes((ushort)(this.tcp.Client.LocalEndPoint as IPEndPoint).Port);
				if (array2 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				NetworkStream stream = this.tcp.GetStream();
				if (stream.CanWrite)
				{
					stream.Write(array2, 0, array2.Length);
				}
				DateTime dateTime = DateTime.Now.AddSeconds(2.0);
				byte[] array3 = new byte[2000];
				int num = 0;
				while (dateTime > DateTime.Now)
				{
					if (stream.CanRead && stream.DataAvailable)
					{
						num = stream.Read(array3, 0, array3.Length);
						break;
					}
				}
				if (num > 0)
				{
					array = new byte[array3.Length];
					array3.CopyTo(array, 0);
				}
				if (array != null)
				{
					this.m_ControllerDriverVer = (int)array[17];
					this.m_ControllerProductTypeCode = (int)array[18];
					recvInfo = array;
					return 1;
				}
				wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			}
			return -1;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000FD5BC File Offset: 0x000FC5BC
		public int GetMjControllerRunInformationIPNoTries(ref byte[] recvInfo)
		{
			WGPacketBasicRunInformationToSend wgpacketBasicRunInformationToSend = new WGPacketBasicRunInformationToSend();
			wgpacketBasicRunInformationToSend.type = 32;
			wgpacketBasicRunInformationToSend.code = 16;
			wgpacketBasicRunInformationToSend.iDevSnFrom = 0U;
			wgpacketBasicRunInformationToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRunInformationToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRunInformationToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 100, wgpacketBasicRunInformationToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				recvInfo = array;
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x000FD67D File Offset: 0x000FC67D
		private ushort GetPort()
		{
			return (ushort)(this.tcp.Client.LocalEndPoint as IPEndPoint).Port;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x000FD69C File Offset: 0x000FC69C
		public string GetProductInfoIP(ref string compactInfo, ref string Desc, int UDPQueue4MultithreadIndex = -1)
		{
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 32;
			wgpacket.code = 80;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return "";
			}
			this.wgudp.udp_get(array2, 300, wgpacket.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				this.m_ControllerDriverVer = (int)array[17];
				this.m_ControllerProductTypeCode = (int)array[18];
				string text = "";
				uint num = (uint)((int)array[8] + ((int)array[9] << 8) + ((int)array[10] << 16) + ((int)array[11] << 24));
				text = text + string.Format("控制器SN: {0}\r\n", num) + string.Format("驱动版本: {0}.{1}\r\n", array[31], array[30]) + string.Format("驱动日期: {0:X2}{1:X2}-{2:X2}-{3:X2}\r\n", new object[]
				{
					array[32],
					array[33],
					array[34],
					array[35]
				});
				Desc = text;
				text = text + "获取控制器信息:\r\n" + BitConverter.ToString(array);
				string text2 = "";
				text2 = string.Concat(new string[]
				{
					text2,
					string.Format("SN={0:d},", num),
					string.Format("VER={0}.{1},", array[31], array[30]),
					string.Format("DATE={0:X2}{1:X2}-{2:X2}-{3:X2},", new object[]
					{
						array[32],
						array[33],
						array[34],
						array[35]
					}),
					"DATA=",
					BitConverter.ToString(array).Replace("-", "").Substring(40, 80),
					BitConverter.ToString(array).Replace("-", "").Substring(1920, 256),
					";"
				});
				compactInfo = text2;
				return text;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return "";
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000FD91C File Offset: 0x000FC91C
		public string GetProductInfoIP_TCP()
		{
			byte[] array = new WGPacket
			{
				type = 32,
				code = 80,
				iDevSnFrom = 0U,
				iDevSnTo = (uint)this.m_ControllerSN,
				iCallReturn = 0
			}.ToBytes(this.GetPort());
			byte[] array2 = this.TCP_Send(array);
			if (array2 != null)
			{
				this.m_ControllerDriverVer = (int)array2[17];
				this.m_ControllerProductTypeCode = (int)array2[18];
				return "获取控制器信息: " + BitConverter.ToString(array2);
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return "";
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000FD9B0 File Offset: 0x000FC9B0
		public int GetSingleSwipeRecord(int swipeIndex, ref wgMjControllerSwipeRecord Swipe)
		{
			WGPacketBasicGetSingleSwipeRecordToSend wgpacketBasicGetSingleSwipeRecordToSend = new WGPacketBasicGetSingleSwipeRecordToSend((uint)swipeIndex);
			wgpacketBasicGetSingleSwipeRecordToSend.type = 32;
			wgpacketBasicGetSingleSwipeRecordToSend.code = 176;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnFrom = 0U;
			wgpacketBasicGetSingleSwipeRecordToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicGetSingleSwipeRecordToSend.iCallReturn = 0;
			Swipe = null;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicGetSingleSwipeRecordToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicGetSingleSwipeRecordToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				uint num = (uint)((int)array[40] + ((int)array[41] << 8) + ((int)array[42] << 16) + ((int)array[43] << 24));
				if (num < 4294967295U)
				{
					Swipe = new wgMjControllerSwipeRecord(array, 24U, (uint)this.m_ControllerSN, num);
				}
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000FDA98 File Offset: 0x000FCA98
		public int IPListControlSet(byte cmdOption, byte param1, byte param2, byte param3, byte[] password, byte[] passwordNew, byte[] param4)
		{
			WGPacketBasicPCAllowedIPSetToSend wgpacketBasicPCAllowedIPSetToSend = new WGPacketBasicPCAllowedIPSetToSend(cmdOption, param1, param2, param3, password, passwordNew, param4);
			wgpacketBasicPCAllowedIPSetToSend.iDevSnFrom = 0U;
			wgpacketBasicPCAllowedIPSetToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicPCAllowedIPSetToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicPCAllowedIPSetToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketBasicPCAllowedIPSetToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			if (array[20] == 1)
			{
				return 1;
			}
			if (array[20] == 0)
			{
				return -1;
			}
			return (int)(-(int)array[20]);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x000FDB56 File Offset: 0x000FCB56
		public static bool IsElevator(int controllerSN)
		{
			return wgMjController.IsElevator_Internal(controllerSN);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x000FDB5E File Offset: 0x000FCB5E
		internal static bool IsElevator_Internal(int controllerSN)
		{
			return controllerSN >= 170000000 && controllerSN <= 179999999;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000FDB75 File Offset: 0x000FCB75
		public static bool IsFingerController(int controllerSN)
		{
			return controllerSN >= 160100000 && controllerSN <= 160999999;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x000FDB8C File Offset: 0x000FCB8C
		private int JoinIP(byte[] currentPassword, ref int keycrc)
		{
			return this.mobileComm(1, 241, currentPassword, null, 0, 0L, ref keycrc, 0);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000FDBAC File Offset: 0x000FCBAC
		public int LoginVisitControlSet(byte cmdOption, int loginPassword, int loginManagePassword, int loginPasswordNew, int loginManagePasswordNew, int loginID)
		{
			WGPacketBasicLoginVisitControlSetToSend wgpacketBasicLoginVisitControlSetToSend = new WGPacketBasicLoginVisitControlSetToSend(cmdOption, loginPassword, loginManagePassword, loginPasswordNew, loginManagePasswordNew, loginID);
			wgpacketBasicLoginVisitControlSetToSend.iDevSnFrom = 0U;
			wgpacketBasicLoginVisitControlSetToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicLoginVisitControlSetToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicLoginVisitControlSetToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketBasicLoginVisitControlSetToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array == null)
			{
				return -1;
			}
			this.m_ControllerDriverVer = (int)array[17];
			this.m_ControllerProductTypeCode = (int)array[18];
			this.m_commLoginID = (int)(array[16] & 14);
			if (array[20] == 1)
			{
				return 1;
			}
			if (array[20] == 0)
			{
				return -1;
			}
			return (int)(-(int)array[20]);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000FDC78 File Offset: 0x000FCC78
		private int mobileComm(int doorNO, byte cmdoption, byte[] oldPassword, byte[] newPassword, int OpenKeyCrc, long cardNO, ref int InfoKeyCrc, int inOrOut)
		{
			int num = -13;
			try
			{
				byte[] array = null;
				wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
				wgpacketShort.code = 40;
				wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
				int num2 = (int)(wgpacketShort.iDevSn / 10000000U);
				switch (num2 % 10)
				{
				case 1:
					wgpacketShort.type = 17;
					break;
				case 2:
					wgpacketShort.type = 25;
					break;
				case 3:
				case 7:
					wgpacketShort.type = 19;
					break;
				case 5:
					wgpacketShort.type = 21;
					break;
				}
				if (wgpacketShort.iDevSn >= 171100001U && wgpacketShort.iDevSn < 171100099U)
				{
					wgpacketShort.type = 19;
				}
				else if (wgpacketShort.iDevSn >= 171200001U && wgpacketShort.iDevSn < 172000000U)
				{
					wgpacketShort.type = 17;
				}
				else if (wgpacketShort.iDevSn >= 172000001U && wgpacketShort.iDevSn < 173000000U)
				{
					wgpacketShort.type = 25;
				}
				else if (wgpacketShort.iDevSn >= 175000001U && wgpacketShort.iDevSn < 176000000U)
				{
					wgpacketShort.type = 21;
				}
				wgpacketShort.data[0] = (byte)doorNO;
				wgpacketShort.data[1] = cmdoption;
				wgpacketShort.data[2] = (byte)inOrOut;
				Array.Copy(BitConverter.GetBytes(cardNO), 0, wgpacketShort.data, 4, 8);
				if (oldPassword != null && oldPassword.Length > 0)
				{
					int num3 = 0;
					while (num3 < oldPassword.Length && num3 < 16)
					{
						wgpacketShort.data[12 + num3] = oldPassword[num3];
						num3++;
					}
				}
				if (newPassword != null && newPassword.Length > 0)
				{
					int num4 = 0;
					while (num4 < newPassword.Length && num4 < 16)
					{
						wgpacketShort.data[44 + num4 - 8] = newPassword[num4];
						num4++;
					}
				}
				DateTime now = DateTime.Now;
				if (OpenKeyCrc == 0)
				{
					wgpacketShort.data[28] = (byte)now.Ticks;
					wgpacketShort.data[29] = (byte)(now.Ticks >> 8);
					wgpacketShort.data[30] = (byte)(now.Ticks >> 16);
					wgpacketShort.data[31] = (byte)(now.Ticks >> 24);
				}
				else
				{
					wgpacketShort.data[28] = (byte)(OpenKeyCrc & 255);
					wgpacketShort.data[29] = (byte)((OpenKeyCrc >> 8) & 255);
					wgpacketShort.data[30] = (byte)((OpenKeyCrc >> 16) & 255);
					wgpacketShort.data[31] = (byte)((OpenKeyCrc >> 24) & 255);
				}
				for (int i = 0; i < 16; i++)
				{
					wgpacketShort.data[12 + i] = wgpacketShort.data[12 + i] ^ wgpacketShort.data[28 + (i & 3)];
				}
				wgpacketShort.data[32] = (byte)(this.tag & 255);
				wgpacketShort.data[33] = (byte)((this.tag >> 8) & 255);
				wgpacketShort.data[34] = (byte)((this.tag >> 16) & 255);
				wgpacketShort.data[35] = (byte)((this.tag >> 24) & 255);
				this.tag++;
				wgTools.getUdpComm(ref this.wgudp, -1);
				byte[] array2 = wgpacketShort.ToBytes();
				wgMjController.WGPacketShort.enc64(ref array2);
				num = this.wgudp.udp_get(array2, 300, uint.MaxValue, this.m_IP, this.m_PORT, ref array);
				if (array != null)
				{
					num = -1;
					if (array[8] == 1)
					{
						num = 1;
						InfoKeyCrc = (int)array[36] + ((int)array[37] << 8) + ((int)array[38] << 16) + ((int)array[39] << 24);
					}
					if (array[8] == 0)
					{
						num = -1;
					}
					if (array[8] == 2)
					{
						num = -2;
					}
					return num;
				}
				num = -13;
				wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x000FE04C File Offset: 0x000FD04C
		public void NetIPConfigure(string strSN, string strMac, string strIP, string strMask, string strGateway, string strPort, string PCIPAddr)
		{
			if (this.wgudp != null)
			{
				this.wgudp = null;
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 37;
			wgpacketWith1152_internal.code = 32;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iCallReturn = 0;
			if (int.Parse(strSN) == -1)
			{
				wgpacketWith1152_internal.iDevSnTo = uint.MaxValue;
			}
			else
			{
				wgpacketWith1152_internal.iDevSnTo = uint.Parse(strSN);
			}
			byte[] array = new byte[1152];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			WGPacketWith1152_internal wgpacketWith1152_internal2 = new WGPacketWith1152_internal();
			int num = 116;
			IPAddress.Parse(strIP).GetAddressBytes().CopyTo(wgpacketWith1152_internal2.ucData, num);
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 120;
			IPAddress.Parse(strMask).GetAddressBytes().CopyTo(wgpacketWith1152_internal2.ucData, num);
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 124;
			if (string.IsNullOrEmpty(strGateway))
			{
				wgpacketWith1152_internal2.ucData[num] = 0;
				wgpacketWith1152_internal2.ucData[num + 1] = 0;
				wgpacketWith1152_internal2.ucData[num + 2] = 0;
				wgpacketWith1152_internal2.ucData[num + 3] = 0;
			}
			else
			{
				IPAddress.Parse(strGateway).GetAddressBytes().CopyTo(wgpacketWith1152_internal2.ucData, num);
			}
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 128;
			wgpacketWith1152_internal2.ucData[num] = (byte)(int.Parse(strPort) & 255);
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num++;
			wgpacketWith1152_internal2.ucData[num] = (byte)((int.Parse(strPort) >> 8) & 255);
			wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] = wgpacketWith1152_internal2.ucData[1024 + (num >> 3)] | (byte)(1 << (num & 7));
			num = 0;
			for (int j = 0; j < 16; j++)
			{
				array[num] = wgpacketWith1152_internal2.ucData[116 + j] & byte.MaxValue;
				array[1024 + (num >> 3)] = array[1024 + (num >> 3)] | (byte)(1 << (num & 7));
				num++;
			}
			array.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WgDebugWrite("Err: IP Configure", new object[0]);
				return;
			}
			byte[] array3 = null;
			this.wgudp.udp_get(array2, 300, 2147483647U, null, 60000, ref array3);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000FE50C File Offset: 0x000FD50C
		public int RebootControllerIP()
		{
			return this.RebootControllerIP(null);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000FE518 File Offset: 0x000FD518
		public int RebootControllerIP(string PCIPAddr)
		{
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 32;
			wgpacket.code = 144;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, 0U, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("重启控制器 没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x000FE5C4 File Offset: 0x000FD5C4
		public int RebootControllerNOPasswordIP()
		{
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 32;
			wgpacket.code = 144;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytesNoPassword(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, 0U, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("重启控制器 没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x000FE66F File Offset: 0x000FD66F
		public int RemoteOpenDoorIP(int doorNO)
		{
			return this.RemoteOpenDoorIP_internal(doorNO, 1U, -1L);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000FE67C File Offset: 0x000FD67C
		public int RemoteOpenDoorIP(int doorNO, uint operatorId, long operatorCardNO)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.RemoteOpenDoorIP64(doorNO, operatorId, operatorCardNO, 1);
			}
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			if (wgTools.bUDPCloud == 1)
			{
				byte[] array = null;
				byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(0);
				if (array2 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				if (wgTools.UDPCloudGet(array2, ref this.m_IP, ref this.m_PORT) > 0)
				{
					wgTools.wgcloud.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
					if (array != null)
					{
						return 1;
					}
					return -1;
				}
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array3 = null;
			byte[] array4 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array4 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array4, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array3);
			if (array3 != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000FE7B8 File Offset: 0x000FD7B8
		internal int RemoteOpenDoorIP_internal(int doorNO, uint operatorId, long operatorCardNO)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x000FE87C File Offset: 0x000FD87C
		public int RemoteOpenDoorIP_Notries(int doorNO, uint operatorId, long operatorCardNO)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000FE940 File Offset: 0x000FD940
		public int RemoteOpenDoorIP_TCP(int doorNO, uint operatorId, uint operatorCardNO)
		{
			byte[] array = new WGPacketBasicRemoteOpenDoorToSend(doorNO)
			{
				type = 32,
				code = 64,
				iDevSnFrom = 0U,
				OperatorID = operatorId,
				OperatorCardNO = (long)((ulong)operatorCardNO),
				iDevSnTo = (uint)this.m_ControllerSN,
				iCallReturn = 0
			}.ToBytes(this.GetPort());
			if (this.TCP_Send(array) != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000FE9BC File Offset: 0x000FD9BC
		public int RemoteOpenDoorIP_V546(int doorNO, long cardNO)
		{
			int num = 0;
			if (this.JoinIP(this.m_pwd, ref num) == 1)
			{
				int num2 = 0;
				if (this.mobileComm(doorNO, 240, this.m_pwd, null, num, cardNO, ref num2, 0) == 1)
				{
					return 1;
				}
			}
			return -1;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000FE9FC File Offset: 0x000FD9FC
		public int RemoteOpenDoorIP64(int doorNO, uint operatorId, long operatorCardNO, int inOrOut = 1)
		{
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 64;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.data[0] = (byte)(doorNO & 255);
			wgpacketShort.data[20] = ((inOrOut == 1) ? 0 : 1);
			long num = (long)((ulong)Math.Max(operatorId, 1U));
			if (operatorCardNO > 0L)
			{
				num = operatorCardNO;
			}
			Array.Copy(BitConverter.GetBytes(num), 0, wgpacketShort.data, 12, 4);
			Array.Copy(BitConverter.GetBytes(num), 4, wgpacketShort.data, 16, 4);
			wgpacketShort.data[24] = 90;
			if (wgTools.bUDPCloud == 1)
			{
				byte[] array = null;
				byte[] array2 = wgpacketShort.ToBytes();
				if (array2 == null)
				{
					wgTools.WriteLine("\r\nError 1");
					return -1;
				}
				if (wgTools.UDPCloudGet(array2, ref this.m_IP, ref this.m_PORT) > 0)
				{
					wgTools.wgcloud.udp_get(array2, 300, wgpacketShort.sequenceId, this.m_IP, this.m_PORT, ref array);
					if (array != null)
					{
						return 1;
					}
					return -1;
				}
			}
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array3 = null;
			byte[] array4 = wgpacketShort.ToBytes();
			if (array4 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array4, 300, wgpacketShort.sequenceId, this.m_IP, this.m_PORT, ref array3);
			if (array3 != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x000FEB58 File Offset: 0x000FDB58
		public int RemoteOpenDoorIPNoStartDelay(int doorNO, uint operatorId, long operatorCardNO, int Exit)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(doorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			if (Exit > 0)
			{
				wgpacketBasicRemoteOpenDoorToSend.Exit = 1;
			}
			if (this.wgudp == null)
			{
				this.wgudp = new wgUdpComm();
			}
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x000FEC30 File Offset: 0x000FDC30
		public int RemoteOpenExtboardIPNoStartDelay(int doorNO, uint operatorId, long operatorCardNO, int Exit, int openDelay)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(doorNO & 3);
			wgpacketBasicRemoteOpenDoorToSend.specialDoorNo4ping(doorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.openDelay = openDelay;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			if (Exit > 0)
			{
				wgpacketBasicRemoteOpenDoorToSend.Exit = 1;
			}
			if (this.wgudp == null)
			{
				this.wgudp = new wgUdpComm();
			}
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000FED18 File Offset: 0x000FDD18
		public int RemoteOpenFoorIP(int floorNO, uint operatorId, long operatorCardNO)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(1, floorNO);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x000FEDDC File Offset: 0x000FDDDC
		public int RemoteOpenFoorIP(int floorNO, uint operatorId, long operatorCardNO, int FloorDelay)
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(1, floorNO, FloorDelay);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = Math.Max(operatorId, 1U);
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = operatorCardNO;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000FEEA4 File Offset: 0x000FDEA4
		public int RenewControlTaskListIP(int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				this.RenewControlTaskListIP_P64(UDPQueue4MultithreadIndex);
			}
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 32;
			wgpacket.code = 112;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacket.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000FEF64 File Offset: 0x000FDF64
		public int RenewControlTaskListIP_P64(int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			bool flag = true;
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.code = 172;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, 1437248085L);
			if (flag && this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) > 0)
			{
				if (array[8] != 1)
				{
					flag = false;
				}
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.9: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1));
			}
			if (flag)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000FF007 File Offset: 0x000FE007
		public int RestoreAllSwipeInTheControllersIP()
		{
			return this.UpdateLastGetRecordLocationIP(0U);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x000FF010 File Offset: 0x000FE010
		public int RestoreDefaultConfigureIP()
		{
			wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure();
			wgMjControllerConfigure.RestoreDefault();
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 32;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			wgMjControllerConfigure.paramData.CopyTo(wgpacketWith1152_internal.ucData, 0);
			wgMjControllerConfigure.needUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_onlySend(array2, 1000, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x000FF0F4 File Offset: 0x000FE0F4
		public void SearchControlers(ref ArrayList arrController)
		{
			uint maxValue = uint.MaxValue;
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = maxValue;
			wgpacket.iCallReturn = 0;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
				if (unicastAddresses.Count > 0)
				{
					foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
					{
						if (!unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
						{
							this.wgudp = new wgUdpComm(unicastIPAddressInformation.Address);
							Thread.Sleep(300);
							byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
							if (array2 == null)
							{
								return;
							}
							byte[] array3 = null;
							this.wgudp.udp_get(array2, 300, uint.MaxValue, null, 60000, ref array3);
							if (array3 != null)
							{
								long num = DateTime.Now.Ticks + 4000000L;
								wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array3, 20);
								if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
								{
									arrayList.Add(wgMjControllerConfigure.controllerSN);
									wgMjControllerConfigure.pcIPAddr = unicastIPAddressInformation.Address.ToString();
									arrayList2.Add(wgMjControllerConfigure);
								}
								while (DateTime.Now.Ticks < num)
								{
									if (this.wgudp.PacketCount > 0)
									{
										while (this.wgudp.PacketCount > 0)
										{
											wgMjControllerConfigure = new wgMjControllerConfigure(this.wgudp.GetPacket(), 20);
											if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
											{
												arrayList.Add(wgMjControllerConfigure.controllerSN);
												wgMjControllerConfigure.pcIPAddr = unicastIPAddressInformation.Address.ToString();
												arrayList2.Add(wgMjControllerConfigure);
											}
										}
										num = DateTime.Now.Ticks + 4000000L;
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
			arrController = arrayList2;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x000FF380 File Offset: 0x000FE380
		public void SearchControlersByDefaultAdapter(ref ArrayList arrControllerConfigureInfo, ref ArrayList arrControllerSN)
		{
			uint maxValue = uint.MaxValue;
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 16;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = maxValue;
			wgpacket.iCallReturn = 0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			if (this.wgudp == null)
			{
				this.wgudp = new wgUdpComm();
				Thread.Sleep(300);
			}
			byte[] array = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array != null)
			{
				byte[] array2 = null;
				this.wgudp.udp_get(array, 300, uint.MaxValue, null, 60000, ref array2);
				if (array2 != null)
				{
					long num = DateTime.Now.Ticks + 4000000L;
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array2, 20);
					if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
					{
						arrayList.Add(wgMjControllerConfigure.controllerSN);
						arrayList2.Add(wgMjControllerConfigure);
					}
					while (DateTime.Now.Ticks < num)
					{
						if (this.wgudp.PacketCount > 0)
						{
							while (this.wgudp.PacketCount > 0)
							{
								wgMjControllerConfigure = new wgMjControllerConfigure(this.wgudp.GetPacket(), 20);
								if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
								{
									arrayList.Add(wgMjControllerConfigure.controllerSN);
									arrayList2.Add(wgMjControllerConfigure);
								}
							}
							num = DateTime.Now.Ticks + 4000000L;
						}
						else
						{
							Thread.Sleep(100);
						}
					}
				}
				Console.WriteLine();
				arrControllerConfigureInfo = arrayList2;
				arrControllerSN = arrayList;
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x000FF524 File Offset: 0x000FE524
		public void SearchControlersShortByDefaultAdapter(ref ArrayList arrControllerConfigureInfo, ref ArrayList arrControllerSN)
		{
			uint maxValue = uint.MaxValue;
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 36;
			wgpacket.code = 64;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = maxValue;
			wgpacket.iCallReturn = 0;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			if (this.wgudp == null)
			{
				this.wgudp = new wgUdpComm();
				Thread.Sleep(300);
			}
			byte[] array = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array != null)
			{
				byte[] array2 = null;
				this.wgudp.udp_get(array, 300, uint.MaxValue, null, 60000, ref array2);
				if (array2 != null)
				{
					long num = DateTime.Now.Ticks + 4000000L;
					wgMjControllerConfigure wgMjControllerConfigure = new wgMjControllerConfigure(array2, 20);
					if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
					{
						arrayList.Add(wgMjControllerConfigure.controllerSN);
						arrayList2.Add(wgMjControllerConfigure);
					}
					while (DateTime.Now.Ticks < num)
					{
						if (this.wgudp.PacketCount > 0)
						{
							while (this.wgudp.PacketCount > 0)
							{
								wgMjControllerConfigure = new wgMjControllerConfigure(this.wgudp.GetPacket(), 20);
								if (arrayList.IndexOf(wgMjControllerConfigure.controllerSN) < 0)
								{
									arrayList.Add(wgMjControllerConfigure.controllerSN);
									arrayList2.Add(wgMjControllerConfigure);
								}
							}
							num = DateTime.Now.Ticks + 4000000L;
						}
						else
						{
							Thread.Sleep(100);
						}
					}
				}
				Console.WriteLine();
				arrControllerConfigureInfo = arrayList2;
				arrControllerSN = arrayList;
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000FF6C5 File Offset: 0x000FE6C5
		public void ShortPacketOptionPwdSet(string pass)
		{
			if (this.getByteOfPassword(pass).Length > 0)
			{
				this.m_pwd = this.getByteOfPassword(pass);
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x000FF6E0 File Offset: 0x000FE6E0
		public int ShortPacketSend(byte[] cmd, ref byte[] recv)
		{
			int num = -13;
			try
			{
				if (cmd.Length != 64 && cmd.Length != 1024)
				{
					return -2;
				}
				recv = null;
				wgTools.getUdpComm(ref this.wgudp, -1);
				num = this.wgudp.udp_get(cmd, 300, 0U, this.m_IP, this.m_PORT, ref recv);
				if (recv != null)
				{
					return num;
				}
				num = -13;
				wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
				return -1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x000FF784 File Offset: 0x000FE784
		public int SpecialPingIP()
		{
			WGPacketBasicRemoteOpenDoorToSend wgpacketBasicRemoteOpenDoorToSend = new WGPacketBasicRemoteOpenDoorToSend(1);
			wgpacketBasicRemoteOpenDoorToSend.specialDoorNo4ping(5);
			wgpacketBasicRemoteOpenDoorToSend.type = 32;
			wgpacketBasicRemoteOpenDoorToSend.code = 64;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnFrom = 0U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorID = 1U;
			wgpacketBasicRemoteOpenDoorToSend.OperatorCardNO = 0L;
			wgpacketBasicRemoteOpenDoorToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicRemoteOpenDoorToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicRemoteOpenDoorToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 100, wgpacketBasicRemoteOpenDoorToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				this.m_ControllerSN = ((int)array[11] << 24) + ((int)array[10] << 16) + ((int)array[9] << 8) + (int)array[8];
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x000FF865 File Offset: 0x000FE865
		public void TCP_Close()
		{
			if (this.tcp != null)
			{
				this.tcp.Close();
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x000FF87C File Offset: 0x000FE87C
		public void TCP_Open(string strIP)
		{
			if (this.tcp == null)
			{
				this.tcp = new TcpClient();
				this.tcp.Connect(IPAddress.Parse(strIP), 60000);
				this.tcp.ReceiveBufferSize = 2000;
				this.tcp.SendBufferSize = 2000;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000FF8D4 File Offset: 0x000FE8D4
		public byte[] TCP_Send(byte[] bytSend)
		{
			if (bytSend == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return null;
			}
			NetworkStream stream = this.tcp.GetStream();
			if (stream.CanWrite)
			{
				stream.Write(bytSend, 0, bytSend.Length);
			}
			DateTime dateTime = DateTime.Now.AddSeconds(2.0);
			byte[] array = new byte[2000];
			byte[] array2 = null;
			while (dateTime > DateTime.Now)
			{
				if (stream.CanRead && stream.DataAvailable)
				{
					int num = stream.Read(array, 0, array.Length);
					array2 = new byte[num];
					Array.Copy(array, array2, num);
					return array2;
				}
			}
			return array2;
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000FF978 File Offset: 0x000FE978
		public int test1024Read(uint times, ref int page)
		{
			WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY();
			wgTools.getUdpComm(ref this.wgudp, -1);
			int num = 0;
			try
			{
				int num2 = 4848;
				while (++num <= (int)times)
				{
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)this.m_ControllerSN, (uint)(num2 * 1024), (uint)(num2 * 1024 + 1024 - 1));
					byte[] array = wgpacketSSI_FLASH_QUERY.ToBytes(this.wgudp.udpPort);
					byte[] array2 = null;
					int num3 = this.wgudp.udp_get(array, 300, wgpacketSSI_FLASH_QUERY.xid, this.m_IP, this.m_PORT, ref array2);
					page = num2;
					if (num3 < 0)
					{
						return -num;
					}
					wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)this.m_ControllerSN, (uint)((num2 + 1) * 1024), (uint)((num2 + 1) * 1024 + 1024 - 1));
					array = wgpacketSSI_FLASH_QUERY.ToBytes(this.wgudp.udpPort);
					array2 = null;
					num3 = this.wgudp.udp_get(array, 300, wgpacketSSI_FLASH_QUERY.xid, this.m_IP, this.m_PORT, ref array2);
					page = num2 + 1;
					if (num3 < 0)
					{
						return -num;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000FFAC4 File Offset: 0x000FEAC4
		public int test1024Write()
		{
			WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
			wgpacketSSI_FLASH.type = 33;
			wgpacketSSI_FLASH.code = 48;
			wgpacketSSI_FLASH.iDevSnFrom = 0U;
			wgpacketSSI_FLASH.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH.iCallReturn = 0;
			wgpacketSSI_FLASH.ucData = new byte[1024];
			int num = -1;
			wgTools.getUdpComm(ref this.wgudp, -1);
			try
			{
				int num2 = 4848;
				int num3 = 1024;
				wgpacketSSI_FLASH.iStartFlashAddr = (uint)(num2 * 1024);
				wgpacketSSI_FLASH.iEndFlashAddr = wgpacketSSI_FLASH.iStartFlashAddr + 1024U - 1U;
				for (int i = 0; i < 1024; i++)
				{
					wgpacketSSI_FLASH.ucData[i] = byte.MaxValue;
				}
				for (int j = 0; j < num3; j++)
				{
					wgpacketSSI_FLASH.ucData[j] = 0;
				}
				byte[] array = null;
				num = this.wgudp.udp_get(wgpacketSSI_FLASH.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH.xid, this.m_IP, this.m_PORT, ref array);
				if (num < 0)
				{
					return -1;
				}
				if (array != null && array.Length == 1052)
				{
					for (int k = 0; k < num3; k++)
					{
						if (array[k + 28] != 0)
						{
							return -2;
						}
					}
				}
				wgpacketSSI_FLASH.iStartFlashAddr += 1024U;
				wgpacketSSI_FLASH.iEndFlashAddr = wgpacketSSI_FLASH.iStartFlashAddr + 1024U - 1U;
				for (int l = 0; l < 1024; l++)
				{
					wgpacketSSI_FLASH.ucData[l] = byte.MaxValue;
				}
				num = this.wgudp.udp_get(wgpacketSSI_FLASH.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH.xid, this.m_IP, this.m_PORT, ref array);
				if (num < 0)
				{
					return -1;
				}
				if (array == null || array.Length != 1052)
				{
					return num;
				}
				for (int m = 0; m < num3; m++)
				{
					if (array[m + 28] != 255)
					{
						return -3;
					}
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000FFCE4 File Offset: 0x000FECE4
		public int UpdateConfigureCPUSuperIP(byte[] data, string oldPassword)
		{
			return this.UpdateConfigureCPUSuperIP(data, oldPassword, "");
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000FFCF4 File Offset: 0x000FECF4
		public int UpdateConfigureCPUSuperIP(byte[] data, string oldPassword, string PCIPAddr)
		{
			WGPacketWith1152 wgpacketWith = new WGPacketWith1152();
			wgpacketWith.type = 37;
			wgpacketWith.code = 32;
			wgpacketWith.iDevSnFrom = 0U;
			wgpacketWith.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			data.CopyTo(wgpacketWith.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith.ToBytesNoPassword(this.wgudp.udpPort);
			if (!string.IsNullOrEmpty(oldPassword))
			{
				this.EncWGPacket(ref array2, array2.Length, oldPassword);
			}
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 1000, wgpacketWith.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000FFDD9 File Offset: 0x000FEDD9
		public int UpdateConfigureIP(wgMjControllerConfigure mjconf, int UDPQueue4MultithreadIndex = -1)
		{
			return this.UpdateConfigureIP(mjconf.paramData, mjconf.needUpdate, UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000FFDF0 File Offset: 0x000FEDF0
		public int UpdateConfigureIP(byte[] dataParam, byte[] dataNeedUpdate, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.UpdateConfigureIP_P64(dataParam, dataNeedUpdate, UDPQueue4MultithreadIndex);
			}
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 32;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			dataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			dataNeedUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000FFED0 File Offset: 0x000FEED0
		public int UpdateConfigureIP_P64(byte[] dataParam, byte[] dataNeedUpdate, int UDPQueue4MultithreadIndex = -1)
		{
			wgTools.delay4SendNextCommand_P64();
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.code = 242;
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, 1437248085L);
			int num = 0;
			bool flag = true;
			for (int i = 0; i < 1024; i++)
			{
				if (((int)dataNeedUpdate[i >> 3] & (1 << (i & 7))) > 0)
				{
					wgpacketShort.data[(num & 7) * 3 + 4] = (byte)((i + 16) & 255);
					wgpacketShort.data[(num & 7) * 3 + 4 + 1] = (byte)((i + 16 >> 8) & 255);
					wgpacketShort.data[(num & 7) * 3 + 4 + 2] = dataParam[i];
					num++;
					if ((num & 7) == 0)
					{
						byte[] array = null;
						if (this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) <= 0)
						{
							flag = false;
							break;
						}
						if (array[8] != 1)
						{
							flag = false;
							break;
						}
						for (int j = 4; j < 28; j++)
						{
							wgpacketShort.data[j] = 0;
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.1: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1 + i));
						wgTools.delay4SendNextCommand_P64();
					}
				}
			}
			if (flag && (num & 7) > 0 && (wgpacketShort.data[4] > 0 || wgpacketShort.data[5] > 0))
			{
				byte[] array2 = null;
				if (this.GetInfo_BaseComm_IP64(ref array2, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) > 0)
				{
					if (array2[8] == 1)
					{
						for (int k = 4; k < 28; k++)
						{
							wgpacketShort.data[k] = 0;
						}
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				for (int l = 0; l < 4; l++)
				{
					if (((int)dataNeedUpdate[2 + l * 2 >> 3] & (1 << ((2 + l * 2) & 7))) > 0 || ((int)dataNeedUpdate[3 + l * 2 >> 3] & (1 << ((3 + l * 2) & 7))) > 0 || ((int)dataNeedUpdate[10 + l >> 3] & (1 << ((10 + l) & 7))) > 0)
					{
						wgpacketShort.code = 128;
						wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
						wgpacketShort.data[0] = (byte)(l + 1);
						wgpacketShort.data[1] = dataParam[10 + l];
						int num2 = (int)dataParam[2 + l * 2] + ((int)dataParam[2 + l * 2] << 8);
						num2 /= 10;
						wgpacketShort.data[2] = (byte)num2;
						wgpacketShort.data[3] = (byte)(num2 >> 8);
						byte[] array3 = null;
						if (this.GetInfo_BaseComm_IP64(ref array3, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) <= 0)
						{
							flag = false;
							break;
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.2: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1 + l));
						wgTools.delay4SendNextCommand_P64();
					}
				}
			}
			if (flag)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x001001A8 File Offset: 0x000FF1A8
		public int UpdateConfigureSuperIP(byte[] data)
		{
			return this.UpdateConfigureSuperIP_internal(data);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x001001B4 File Offset: 0x000FF1B4
		internal int UpdateConfigureSuperIP_internal(byte[] data)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 48;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			data.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return -1;
			}
			this.wgudp.udp_get_onlySend(array2, 1000, uint.MaxValue, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00100280 File Offset: 0x000FF280
		public int UpdateConfigureSuperIP4Product(byte[] data)
		{
			return this.UpdateConfigureSuperIP4Product_internal(data);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0010028C File Offset: 0x000FF28C
		internal int UpdateConfigureSuperIP4Product_internal(byte[] data)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 48;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			data.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 1000, uint.MaxValue, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00100358 File Offset: 0x000FF358
		public int UpdateConfigureSuperIP4Product2(byte[] data)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 35;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			data.CopyTo(wgpacketWith1152_internal.ucData, 0);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine(string.Format("\r\n出错1:\t{0}", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒")));
				return -1;
			}
			this.wgudp.udp_get(array2, 1000, uint.MaxValue, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00100424 File Offset: 0x000FF424
		public int UpdateConfigureUnvalidIP(wgMjControllerConfigure mjconf, int UDPQueue4MultithreadIndex = -1)
		{
			return this.UpdateConfigurIP(mjconf.paramData, mjconf.needUpdate, UDPQueue4MultithreadIndex);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0010043C File Offset: 0x000FF43C
		private int UpdateConfigurIP(byte[] dataParam, byte[] dataNeedUpdate, int UDPQueue4MultithreadIndex = -1)
		{
			WGPacketWith1152_internal wgpacketWith1152_internal = new WGPacketWith1152_internal();
			wgpacketWith1152_internal.type = 36;
			wgpacketWith1152_internal.code = 32;
			wgpacketWith1152_internal.iDevSnFrom = 0U;
			wgpacketWith1152_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketWith1152_internal.iCallReturn = 1;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			dataParam.CopyTo(wgpacketWith1152_internal.ucData, 0);
			dataNeedUpdate.CopyTo(wgpacketWith1152_internal.ucData, 1024);
			byte[] array = null;
			byte[] array2 = wgpacketWith1152_internal.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get_notries(array2, 1000, wgpacketWith1152_internal.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00100508 File Offset: 0x000FF508
		public int UpdateControlTaskListIP(byte[] controlTaskListData, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.UpdateControlTaskListIP_P64(controlTaskListData, UDPQueue4MultithreadIndex);
			}
			WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
			wgpacketSSI_FLASH_internal.type = 33;
			wgpacketSSI_FLASH_internal.code = 32;
			wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
			wgpacketSSI_FLASH_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = null;
			for (int i = 0; i < 4096; i += 1024)
			{
				Array.Copy(controlTaskListData, i, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(MjControlTaskItem.flashStartAddr_internal + i);
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				for (int j = 0; j < this.priMaxTries; j++)
				{
					if (array != null)
					{
						wgpacketSSI_FLASH_internal.GetNewXid();
					}
					array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
					if (array == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
					if (array2 != null)
					{
						bool flag = true;
						int num = 0;
						while (num < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num < array2.Length)
						{
							if (wgpacketSSI_FLASH_internal.ucData[num] != array2[28 + num])
							{
								wgTools.WgDebugWrite("Upload updateControlTimeSegListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
								flag = false;
								array2 = null;
								break;
							}
							num++;
						}
						if (flag)
						{
							break;
						}
					}
					Thread.Sleep(this.sleep4Tries);
				}
				if (array2 == null)
				{
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					break;
				}
			}
			if (array2 != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x001006B4 File Offset: 0x000FF6B4
		public int UpdateControlTaskListIP_P64(byte[] controlTaskListData, int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			bool flag = true;
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.code = 166;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, 1437248085L);
			if (flag && this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) > 0)
			{
				if (array[8] != 1)
				{
					flag = false;
				}
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.3: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1));
			}
			if (flag)
			{
				bool flag2 = true;
				for (int i = 0; i < 32; i++)
				{
					if (controlTaskListData[i] != 255)
					{
						flag2 = false;
						break;
					}
				}
				if (!flag2)
				{
					wgpacketShort.code = 184;
					for (int j = 0; j < 4096; j += 32)
					{
						wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, (long)(MjControlTaskItem.flashStartAddr_internal + j));
						Array.Copy(controlTaskListData, j, wgpacketShort.data, 4, 28);
						Array.Copy(controlTaskListData, j + 28, wgpacketShort.data, 40, 4);
						wgpacketShort.data[36] = 32;
						if (this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) <= 0)
						{
							flag = false;
							break;
						}
						if (array[8] != 1)
						{
							flag = false;
							break;
						}
						bool flag3 = true;
						for (int k = 0; k < 32; k++)
						{
							if (controlTaskListData[k + j] != 255)
							{
								flag3 = false;
								break;
							}
						}
						if (flag3)
						{
							break;
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.4: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1 + j));
						wgTools.delay4SendNextCommand_P64();
					}
				}
			}
			if (flag)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00100878 File Offset: 0x000FF878
		public int UpdateControlTimeSegListIP(byte[] controlTimeSegListData, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.UpdateControlTimeSegListIP_P64(controlTimeSegListData, UDPQueue4MultithreadIndex);
			}
			WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
			wgpacketSSI_FLASH_internal.type = 33;
			wgpacketSSI_FLASH_internal.code = 32;
			wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
			wgpacketSSI_FLASH_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH_internal.iCallReturn = 0;
			int num = -1;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = null;
			for (int i = 0; i < 8192; i += 1024)
			{
				Array.Copy(controlTimeSegListData, i, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(MjControlTimeSeg.flashStartAddr + i);
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				for (int j = 0; j < this.priMaxTries; j++)
				{
					if (array != null)
					{
						wgpacketSSI_FLASH_internal.GetNewXid();
					}
					array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
					if (array == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					num = this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
					if (array2 != null)
					{
						bool flag = true;
						int num2 = 0;
						while (num2 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num2 < array2.Length)
						{
							if (wgpacketSSI_FLASH_internal.ucData[num2] != array2[28 + num2])
							{
								wgTools.WgDebugWrite("Upload updateControlTimeSegListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
								flag = false;
								array2 = null;
								break;
							}
							num2++;
						}
						if (flag)
						{
							break;
						}
					}
					Thread.Sleep(this.sleep4Tries);
				}
				if (num < 0 || array2 == null)
				{
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					break;
				}
			}
			if (array2 != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00100A30 File Offset: 0x000FFA30
		public int UpdateControlTimeSegListIP_P64(byte[] controlTimeSegListData, int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			bool flag = true;
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.code = 138;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, 1437248085L);
			if (flag && this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) > 0)
			{
				if (array[8] != 1)
				{
					flag = false;
				}
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.5: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1));
			}
			if (flag)
			{
				wgpacketShort.code = 184;
				for (int i = 0; i < 8192; i += 32)
				{
					bool flag2 = true;
					for (int j = 0; j < 32; j++)
					{
						if (controlTimeSegListData[j + i] != 255)
						{
							flag2 = false;
							break;
						}
					}
					if (!flag2)
					{
						wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, (long)(MjControlTimeSeg.flashStartAddr + i));
						Array.Copy(controlTimeSegListData, i, wgpacketShort.data, 4, 28);
						Array.Copy(controlTimeSegListData, i + 28, wgpacketShort.data, 40, 4);
						wgpacketShort.data[36] = 32;
						if (this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) <= 0)
						{
							flag = false;
							break;
						}
						if (array[8] != 1)
						{
							flag = false;
							break;
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.6: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1 + i));
						wgTools.delay4SendNextCommand_P64();
					}
				}
			}
			if (flag)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00100BCC File Offset: 0x000FFBCC
		public int UpdateDataFlashIP(byte[] data, int len, int startadr)
		{
			WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
			wgpacketSSI_FLASH_internal.type = 33;
			wgpacketSSI_FLASH_internal.code = 32;
			wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
			wgpacketSSI_FLASH_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH_internal.iCallReturn = 0;
			int num = -1;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = null;
			for (int i = 0; i < len; i += 1024)
			{
				if (i + 1024 < len)
				{
					Array.Copy(data, i, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
				}
				else
				{
					Array.Copy(data, i, wgpacketSSI_FLASH_internal.ucData, 0, len - i);
				}
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(startadr + i);
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				for (int j = 0; j < this.priMaxTries; j++)
				{
					if (array != null)
					{
						wgpacketSSI_FLASH_internal.GetNewXid();
					}
					array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
					if (array == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					num = this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
					if (array2 != null)
					{
						bool flag = true;
						int num2 = 0;
						while (num2 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num2 < array2.Length)
						{
							if (wgpacketSSI_FLASH_internal.ucData[num2] != array2[28 + num2])
							{
								wgTools.WgDebugWrite("Upload DataFlashIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
								flag = false;
								array2 = null;
								break;
							}
							num2++;
						}
						if (flag)
						{
							break;
						}
					}
					Thread.Sleep(this.sleep4Tries);
				}
				if (num < 0 || array2 == null)
				{
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					break;
				}
			}
			if (array2 != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00100D8C File Offset: 0x000FFD8C
		public int UpdateFingerprintListIP(int fingerLen, byte[] fingerCardNO, byte[] fingerInfoData, string DoorName, int UDPQueue4MultithreadIndex = -1)
		{
			WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
			wgpacketSSI_FLASH_internal.type = 33;
			wgpacketSSI_FLASH_internal.code = 32;
			wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
			wgpacketSSI_FLASH_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = null;
			this.DisplayProcessInfo(DoorName, 1, "");
			int i = 0;
			while (i < 4096)
			{
				Array.Copy(fingerCardNO, i, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(4182016 + i);
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				if (i <= 0)
				{
					goto IL_00C7;
				}
				bool flag = true;
				for (int j = 0; j < 1024; j++)
				{
					if (wgpacketSSI_FLASH_internal.ucData[j] != 255)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					goto IL_00C7;
				}
				IL_01C1:
				i += 1024;
				continue;
				IL_00C7:
				for (int k = 0; k < this.priMaxTries; k++)
				{
					if (array != null)
					{
						wgpacketSSI_FLASH_internal.GetNewXid();
					}
					array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
					if (array == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					if (this.bStopFingerprintComm)
					{
						return -1;
					}
					this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
					if (array2 != null)
					{
						bool flag2 = true;
						int num = 0;
						while (num < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num < array2.Length)
						{
							if (wgpacketSSI_FLASH_internal.ucData[num] != array2[28 + num])
							{
								wgTools.WgDebugWrite("Upload UpdateFingerprintListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
								flag2 = false;
								array2 = null;
								break;
							}
							num++;
						}
						if (flag2)
						{
							break;
						}
					}
					Thread.Sleep(this.sleep4Tries);
				}
				if (array2 == null)
				{
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					break;
				}
				goto IL_01C1;
			}
			if (array2 != null)
			{
				bool flag3 = false;
				if (this.chkFalseControlIP() == 0)
				{
					flag3 = true;
				}
				else
				{
					byte[] array3 = new byte[2048];
					byte[] array4 = new byte[2048];
					int num2 = 196608;
					if (this.GetControlDataNoPasswordIP(num2, 1427899392, ref array3) > 0 && this.GetControlDataNoPasswordIP(num2 + 1024, 1427899392, ref array4) > 0)
					{
						try
						{
							byte[] array5 = new byte[1024];
							byte[] array6 = new byte[1024];
							Array.Copy(array3, 28, array5, 0, 1024);
							Array.Copy(array4, 28, array6, 0, 1024);
							byte[] array7 = new byte[16];
							Array.Copy(array5, 896, array7, 0, 16);
							byte[] array8 = new byte[256];
							Array.Copy(array6, 0, array8, 0, 256);
							RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
							rsacryptoServiceProvider.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHsTQfsnD5oiAjsldv6Hn0+fk3soS8cQxkG25zIp3AI2So3IPxyOTe07QsB11kDSK4qpl7LpvApGN5plkBWQ449mnqTqGFHiPoEJq0SQmKIkCbwd9A1F724+9Vm3rCi+icvwRHHMf/+cmU63XyNFNFf8XebgH8Q2IbeQEQOVf5TIG6S5H07M98rQk6ww/wyf42TuIldTanZN4/ygz04yCJ/cDAoZ1/khyKgaQ4E8GTTJiInCTSvvlWN57uR/KdXBKRgVcJ82Gsswa0ObHulF/em1NRQPvSdO/xZYVnfGItyBoluecetfvzeRcwvrhBY4BUgvomBaiCJ4gW68gwJhbMQA6rCEbOTC0nF9ipfxmm9sVmil9X2/1AaK3jd0Fl6VCkKcUEhvqL6qaNepMZtv075WCsvHwDnvniQAlRl0Wsb4+6FT7LZ4PL8EHPBhPYZVPuwAWT/fj31GFsyDiign9lWrsALJIDEwTfKFTSO5IO9jUsUNZGvoaUbSlCVRt9OsISOX9xBUjrrjqO4Hia7FJseI="));
							if (!rsacryptoServiceProvider.VerifyData(array7, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), array8))
							{
								flag3 = true;
							}
							rsacryptoServiceProvider.Clear();
							if (flag3)
							{
								flag3 = false;
								Array.Copy(array5, 896, array7, 0, 16);
								Array.Copy(array6, 0, array8, 0, 256);
								RSACryptoServiceProvider rsacryptoServiceProvider2 = new RSACryptoServiceProvider();
								rsacryptoServiceProvider2.FromXmlString(Program.Dpt4Database("luSWeZHP2nqo3pdFCebDHn9E02cWiFP6UoW408FEGsKBr7prhHDtqEA7REbJZZCXghz5mchVOMJXWKpJnBohasJyXaGXuMy++WClCd/QGAy0odug3tDt6Qb1A5ZxLbRzsXnp6Mey0c4TrTZA2P6uPw1ZQt30CMF6PBPFcgCmBqas0ULjRgnRTGoJCQd5wZrk2tFhNssS7HXFbg2GDdUaGs38HE2dFWf5+9cmqVJdEVnO4YZQmCCFDxsL4m0oECVfpHRJDRTUJNTG4QrlreBOkQ+DojmkJJKlKXTV0BhmaxoUS1uBJNivkdAer4vTCTet6zLDN5+peRdngMDOgAPa10NPLr2lxrxVRjMjF9GxCW4bzXlAmbTqpgaZollzzD89gBf7vssE9CluavpybjKkBkKSr3dviGW+YqQhtTYyPi6Bi7FQhIIpxh1xiZ6tY6NRCoeX7dawowwfzCDDOREM9Hl4ZwSr7ZVzN7CKAbsIwMIIpoUXND4DGibauQj8FHJv5052exne7G3/r7FV2KsIvDjrBXQ9VnBPB05YyozMDwg="));
								if (!rsacryptoServiceProvider2.VerifyData(array7, Program.Dpt4Database("7TbkKQVZ4Al3BOP7opHTFQ=="), array8))
								{
									flag3 = true;
								}
							}
						}
						catch
						{
							flag3 = true;
						}
					}
				}
				int num3 = 0;
				if (fingerLen > 0)
				{
					num3 = fingerLen * 512;
				}
				for (int l = 0; l < num3; l += 1024)
				{
					if (l + 512 == num3)
					{
						for (int m = 0; m < 1024; m++)
						{
							wgpacketSSI_FLASH_internal.ucData[m] = byte.MaxValue;
						}
						Array.Copy(fingerInfoData, l, wgpacketSSI_FLASH_internal.ucData, 0, 512);
					}
					else
					{
						Array.Copy(fingerInfoData, l, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
					}
					wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(4186112 + l);
					wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
					this.DisplayProcessInfo(DoorName, 2, ((l / 1024 + 1) * 2).ToString());
					for (int n = 0; n < this.priMaxTries; n++)
					{
						if (array != null)
						{
							wgpacketSSI_FLASH_internal.GetNewXid();
						}
						array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
						if (array == null)
						{
							wgTools.WriteLine("\r\nError 1");
							return -1;
						}
						if (this.bStopFingerprintComm)
						{
							return -1;
						}
						this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
						if (array2 != null)
						{
							bool flag4 = true;
							int num4 = 0;
							while (num4 < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num4 < array2.Length)
							{
								if (wgpacketSSI_FLASH_internal.ucData[num4] != array2[28 + num4])
								{
									wgTools.WgDebugWrite("Upload UpdateFingerprintListIP2 Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + l.ToString(), new object[0]);
									flag4 = false;
									array2 = null;
									break;
								}
								num4++;
							}
							if (flag4)
							{
								break;
							}
						}
						Thread.Sleep(this.sleep4Tries);
					}
					if (array2 == null)
					{
						wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
						break;
					}
				}
				if (array2 == null)
				{
					return -1;
				}
				if (flag3)
				{
					return -1999;
				}
				byte[] array9 = new byte[1152];
				byte[] array10 = new byte[1152];
				for (int num5 = 0; num5 < 1152; num5++)
				{
					array9[num5] = 0;
					array10[num5] = 0;
				}
				array9[0] = 241;
				array9[2] = 7;
				if (this.bStopFingerprintComm)
				{
					return -1;
				}
				if (this.FingerConfigureIP(array9, array10) <= 0)
				{
					return -1;
				}
				for (int num6 = 0; num6 < 1152; num6++)
				{
					array9[num6] = 0;
					array10[num6] = 0;
				}
				this.DisplayProcessInfo(DoorName, 3, "");
				DateTime dateTime = DateTime.Now.AddMinutes(4.0);
				while (dateTime > DateTime.Now)
				{
					if (this.bStopFingerprintComm)
					{
						return -1;
					}
					array9[0] = 241;
					array9[2] = 8;
					if (this.FingerConfigureIP(array9, array10) <= 0)
					{
						return -1;
					}
					this.DisplayProcessInfo(DoorName, 3, (((int)array10[6] << 24) + ((int)array10[5] << 16) + ((int)array10[4] << 8) + (int)array10[3]).ToString() + "/" + (((int)array10[10] << 24) + ((int)array10[9] << 16) + ((int)array10[8] << 8) + (int)array10[7]).ToString());
					if (array10[2] == 0 && array10[3] == array10[7] && array10[4] == array10[8] && array10[5] == array10[9] && array10[6] == array10[10])
					{
						return 1;
					}
					Thread.Sleep(300);
				}
				if (array2 != null)
				{
					return 1;
				}
			}
			return -1;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00101450 File Offset: 0x00100450
		public int UpdateFRamIP(uint FRamIndex, uint newValue)
		{
			return this.UpdateFRamIP_internal(FRamIndex, newValue);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0010145C File Offset: 0x0010045C
		private int UpdateFRamIP_internal(uint FRamIndex, uint newValue)
		{
			WGPacketBasicFRamSetToSend wgpacketBasicFRamSetToSend = new WGPacketBasicFRamSetToSend(FRamIndex, newValue);
			wgpacketBasicFRamSetToSend.type = 32;
			wgpacketBasicFRamSetToSend.code = 128;
			wgpacketBasicFRamSetToSend.iDevSnFrom = 0U;
			wgpacketBasicFRamSetToSend.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketBasicFRamSetToSend.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacketBasicFRamSetToSend.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacketBasicFRamSetToSend.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00101510 File Offset: 0x00100510
		public int UpdateHolidayListIP(byte[] holidayListData, int UDPQueue4MultithreadIndex = -1)
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				return this.UpdateHolidayListIP_P64(holidayListData, UDPQueue4MultithreadIndex);
			}
			WGPacketSSI_FLASH_internal wgpacketSSI_FLASH_internal = new WGPacketSSI_FLASH_internal();
			wgpacketSSI_FLASH_internal.type = 33;
			wgpacketSSI_FLASH_internal.code = 32;
			wgpacketSSI_FLASH_internal.iDevSnFrom = 0U;
			wgpacketSSI_FLASH_internal.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacketSSI_FLASH_internal.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, UDPQueue4MultithreadIndex);
			byte[] array = null;
			byte[] array2 = null;
			for (int i = 0; i < 4096; i += 1024)
			{
				Array.Copy(holidayListData, i, wgpacketSSI_FLASH_internal.ucData, 0, 1024);
				wgpacketSSI_FLASH_internal.iStartFlashAddr = (uint)(MjControlHolidayTime.flashStartAddr_internal + i);
				wgpacketSSI_FLASH_internal.iEndFlashAddr = wgpacketSSI_FLASH_internal.iStartFlashAddr + 1024U - 1U;
				for (int j = 0; j < this.priMaxTries; j++)
				{
					if (array != null)
					{
						wgpacketSSI_FLASH_internal.GetNewXid();
					}
					array = wgpacketSSI_FLASH_internal.ToBytes(this.wgudp.udpPort);
					if (array == null)
					{
						wgTools.WriteLine("\r\nError 1");
						return -1;
					}
					this.wgudp.udp_get(array, 1000, wgpacketSSI_FLASH_internal.xid, this.m_IP, this.m_PORT, ref array2);
					if (array2 != null)
					{
						bool flag = true;
						int num = 0;
						while (num < wgpacketSSI_FLASH_internal.ucData.Length && 28 + num < array2.Length)
						{
							if (wgpacketSSI_FLASH_internal.ucData[num] != array2[28 + num])
							{
								wgTools.WgDebugWrite("Upload UpdateHolidayListIP Failed wgpktWrite.ucData[dataIndex]!=recv[28+dataIndex] i = " + i.ToString(), new object[0]);
								flag = false;
								array2 = null;
								break;
							}
							num++;
						}
						if (flag)
						{
							break;
						}
					}
					Thread.Sleep(this.sleep4Tries);
				}
				if (array2 == null)
				{
					wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
					break;
				}
			}
			if (array2 != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x001016BC File Offset: 0x001006BC
		public int UpdateHolidayListIP_P64(byte[] holidayListData, int UDPQueue4MultithreadIndex = -1)
		{
			byte[] array = null;
			bool flag = true;
			wgMjController.WGPacketShort wgpacketShort = new wgMjController.WGPacketShort();
			wgpacketShort.iDevSn = (uint)this.m_ControllerSN;
			wgpacketShort.code = 186;
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, 1437248085L);
			wgTools.LongTo4Bytes(ref wgpacketShort.data, 4, (long)MjControlHolidayTime.flashStartAddr_internal);
			if (flag && this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) > 0)
			{
				if (array[8] != 1)
				{
					flag = false;
				}
				wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.7: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1));
			}
			if (flag)
			{
				bool flag2 = true;
				for (int i = 0; i < 32; i++)
				{
					if (holidayListData[i] != 255)
					{
						flag2 = false;
						break;
					}
				}
				if (!flag2)
				{
					wgpacketShort.code = 184;
					for (int j = 0; j < 4096; j += 32)
					{
						wgTools.LongTo4Bytes(ref wgpacketShort.data, 0, (long)(MjControlHolidayTime.flashStartAddr_internal + j));
						Array.Copy(holidayListData, j, wgpacketShort.data, 4, 28);
						Array.Copy(holidayListData, j + 28, wgpacketShort.data, 40, 4);
						wgpacketShort.data[36] = 32;
						if (this.GetInfo_BaseComm_IP64(ref array, wgpacketShort.ToBytes(), wgpacketShort.xid, UDPQueue4MultithreadIndex) <= 0)
						{
							flag = false;
							break;
						}
						if (array[8] != 1)
						{
							flag = false;
							break;
						}
						bool flag3 = true;
						for (int k = 0; k < 32; k++)
						{
							if (holidayListData[k + j] != 255)
							{
								flag3 = false;
								break;
							}
						}
						if (flag3)
						{
							break;
						}
						wgAppRunInfo.raiseAppRunInfoCommStatus(string.Format("{0}.8: {1}[{2:d}]", CommonStr.strUpload, this.m_ControllerSN.ToString(), 1 + j));
						wgTools.delay4SendNextCommand_P64();
					}
				}
			}
			if (flag)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00101894 File Offset: 0x00100894
		public int UpdateLastGetRecordLocationIP(uint newValue)
		{
			byte[] array = new byte[1644];
			int num = (int)((newValue | 1U) & 255U);
			if (this.GetControlDataIP_internal(197504 - num, 1427898384 + num, ref array) > 0)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				int gotIndex = wgMjController.getGotIndex(intPtr, this.ControllerSN, (int)newValue);
				Marshal.FreeHGlobal(intPtr);
				uint num2 = (uint)gotIndex;
				return this.UpdateFRamIP_internal(8U, num2);
			}
			return -1;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00101908 File Offset: 0x00100908
		public static bool validSN(int controllerSN)
		{
			bool flag = true;
			if (controllerSN < 100000000 || controllerSN > 500000000)
			{
				flag = false;
			}
			if (controllerSN >= 300000000 && controllerSN < 400000000)
			{
				flag = false;
			}
			if (controllerSN >= 160000000 && controllerSN <= 160999999)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(wgTools.gCustomProductType))
			{
				return true;
			}
			int num = controllerSN / 10000000 % 10;
			int num2 = controllerSN / 1000000 % 100;
			switch (wgTools.gate)
			{
			case 1:
				if (num != 1 && (num2 != 71 || controllerSN < 171200001))
				{
					flag = false;
					goto IL_00CD;
				}
				goto IL_00CD;
			case 2:
				if (num != 2)
				{
					flag = false;
					goto IL_00CD;
				}
				goto IL_00CD;
			case 3:
				if ((controllerSN < 171100001 || controllerSN >= 171100099) && num != 3 && num2 != 73)
				{
					flag = false;
					goto IL_00CD;
				}
				goto IL_00CD;
			case 5:
				if (num != 5 && num2 != 75)
				{
					flag = false;
					goto IL_00CD;
				}
				goto IL_00CD;
			}
			flag = false;
			IL_00CD:
			if (wgTools.gWGYTJ && wgMjController.GetControllerType(controllerSN) != 1)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x001019F8 File Offset: 0x001009F8
		public int WarnResetIP()
		{
			WGPacket wgpacket = new WGPacket();
			wgpacket.type = 32;
			wgpacket.code = 160;
			wgpacket.iDevSnFrom = 0U;
			wgpacket.iDevSnTo = (uint)this.m_ControllerSN;
			wgpacket.iCallReturn = 0;
			wgTools.getUdpComm(ref this.wgudp, -1);
			byte[] array = null;
			byte[] array2 = wgpacket.ToBytes(this.wgudp.udpPort);
			if (array2 == null)
			{
				wgTools.WriteLine("\r\nError 1");
				return -1;
			}
			this.wgudp.udp_get(array2, 300, wgpacket.xid, this.m_IP, this.m_PORT, ref array);
			if (array != null)
			{
				return 1;
			}
			wgTools.WriteLine(string.Format("没有收到数据就退出={0:d}******************", 1));
			return -1;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00101AB8 File Offset: 0x00100AB8
		public int WriteDrvToDataFlash(string newImportFile)
		{
			int num = -1;
			try
			{
				FileStream fileStream = new FileStream(newImportFile, FileMode.Open);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				wgTools.getUdpComm(ref this.wgudp, -1);
				WGPacketSSI_FLASH wgpacketSSI_FLASH = new WGPacketSSI_FLASH();
				wgpacketSSI_FLASH.type = 33;
				wgpacketSSI_FLASH.code = 48;
				wgpacketSSI_FLASH.iDevSnFrom = 0U;
				wgpacketSSI_FLASH.iDevSnTo = (uint)this.m_ControllerSN;
				wgpacketSSI_FLASH.iCallReturn = 0;
				wgpacketSSI_FLASH.ucData = new byte[1024];
				this.wgudp = null;
				wgTools.getUdpComm(ref this.wgudp, -1);
				byte[] array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
				binaryReader.Close();
				byte[] array2 = new byte[]
				{
					85, 80, 71, 82, 65, 68, 50, 48, 49, 52,
					49, 50, 50, 53, 83, 78
				};
				int num2 = array.Length;
				num2 -= 512;
				IntPtr intPtr = Marshal.AllocHGlobal(num2);
				IntPtr intPtr2 = Marshal.AllocHGlobal(16);
				Marshal.Copy(array, 0, intPtr, num2);
				Marshal.Copy(array2, 0, intPtr2, 16);
				wgMjController.dec(intPtr, num2, intPtr2);
				Marshal.Copy(intPtr, array, 0, num2);
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
				uint num3 = 4440064U;
				int num4 = 0;
				int i = 0;
				while (i < array.Length)
				{
					wgpacketSSI_FLASH.iStartFlashAddr = num3;
					wgpacketSSI_FLASH.iEndFlashAddr = num3 + 1024U - 1U;
					int num5 = 0;
					while (num5 < 1024 && i + num5 < array.Length)
					{
						wgpacketSSI_FLASH.ucData[num5] = array[i + num5];
						num5++;
					}
					i += 1024;
					byte[] array3 = null;
					num = this.wgudp.udp_get(wgpacketSSI_FLASH.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH.xid, this.m_IP, this.m_PORT, ref array3);
					if (num < 0)
					{
						wgTools.WgDebugWrite("WriteDrvToDataFlash A Ret=: " + num.ToString(), new object[0]);
						break;
					}
					num4++;
					if (num4 >= 40)
					{
						num4 = 0;
						Application.DoEvents();
					}
					num3 += 1024U;
				}
				wgTools.WgDebugWrite("WriteDrvToDataFlash B Write End And Check Start:", new object[0]);
				num3 = 4440064U;
				num4 = 0;
				i = 0;
				WGPacketSSI_FLASH_QUERY wgpacketSSI_FLASH_QUERY = new WGPacketSSI_FLASH_QUERY(33, 16, (uint)this.m_ControllerSN, 0U, 1023U);
				while (i < array.Length)
				{
					wgpacketSSI_FLASH_QUERY.iStartFlashAddr = num3;
					wgpacketSSI_FLASH_QUERY.iEndFlashAddr = num3 + 1024U - 1U;
					byte[] array4 = null;
					num = this.wgudp.udp_get(wgpacketSSI_FLASH_QUERY.ToBytes(this.wgudp.udpPort), 300, wgpacketSSI_FLASH_QUERY.xid, this.m_IP, this.m_PORT, ref array4);
					if (num < 0)
					{
						wgTools.WgDebugWrite("WriteDrvToDataFlash C Ret=: " + num.ToString(), new object[0]);
						break;
					}
					WGPacketSSI_FLASH wgpacketSSI_FLASH2 = new WGPacketSSI_FLASH(array4);
					int num6 = 0;
					while (num6 < 1024 && i + num6 < array.Length)
					{
						if (wgpacketSSI_FLASH2.ucData[num6] != array[i + num6])
						{
							wgTools.WgDebugWrite("WriteDrvToDataFlash C 上传数据有错 loc = " + (i + num6).ToString(), new object[0]);
							num = -1;
							break;
						}
						num6++;
					}
					if (num < 0)
					{
						break;
					}
					i += 1024;
					num4++;
					if (num4 >= 40)
					{
						num4 = 0;
						Application.DoEvents();
					}
					num3 += 1024U;
				}
				wgTools.WgDebugWrite("WriteDrvToDataFlash D End ", new object[0]);
				fileStream.Dispose();
				this.DrvUpgradeIP();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00101E5C File Offset: 0x00100E5C
		public int ControllerDriverMainVer
		{
			get
			{
				return this.m_ControllerDriverVer;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00101E64 File Offset: 0x00100E64
		public int ControllerProductTypeCode
		{
			get
			{
				return this.m_ControllerProductTypeCode;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00101E6C File Offset: 0x00100E6C
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x00101E74 File Offset: 0x00100E74
		public int ControllerSN
		{
			get
			{
				return this.m_ControllerSN;
			}
			set
			{
				if (value == -1 || value == 999999999)
				{
					this.m_ControllerSN = -1;
					return;
				}
				if (wgMjController.GetControllerType(value) > 0)
				{
					this.m_ControllerSN = value;
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00101E9A File Offset: 0x00100E9A
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00101EA2 File Offset: 0x00100EA2
		public string IP
		{
			get
			{
				return this.m_IP;
			}
			set
			{
				this.m_IP = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00101EAB File Offset: 0x00100EAB
		public string MACAddr
		{
			get
			{
				return this.m_MACAddr;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00101EB3 File Offset: 0x00100EB3
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x00101EBC File Offset: 0x00100EBC
		public string PCIPAddress
		{
			get
			{
				return this.m_PCIPAddress;
			}
			set
			{
				if ((string.IsNullOrEmpty(this.m_PCIPAddress) || this.m_PCIPAddress != value) && !string.IsNullOrEmpty(value))
				{
					try
					{
						if (this.wgudp != null)
						{
							this.wgudp.Close();
							this.wgudp.Dispose();
						}
						this.wgudp = new wgUdpComm(IPAddress.Parse(value));
						Thread.Sleep(300);
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
					}
				}
				this.m_PCIPAddress = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00101F50 File Offset: 0x00100F50
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x00101F58 File Offset: 0x00100F58
		public int PORT
		{
			get
			{
				return this.m_PORT;
			}
			set
			{
				if (value >= 0 && value < 65535)
				{
					this.m_PORT = value;
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00101F6D File Offset: 0x00100F6D
		private wgMjControllerRunInformation RunInfo
		{
			get
			{
				return this.m_runinfo;
			}
		}

		// Token: 0x04001B2D RID: 6957
		private const int ControllerCallReturn_LOC = 16;

		// Token: 0x04001B2E RID: 6958
		private const int ControllerDriverVer_LOC = 17;

		// Token: 0x04001B2F RID: 6959
		private const int ControllerProductTypeCode_LOC = 18;

		// Token: 0x04001B30 RID: 6960
		public const int Door_Offline = 0;

		// Token: 0x04001B31 RID: 6961
		public const int Door_NO = 1;

		// Token: 0x04001B32 RID: 6962
		public const int Door_NC = 2;

		// Token: 0x04001B33 RID: 6963
		public const int Door_Online = 3;

		// Token: 0x04001B34 RID: 6964
		public const int Door_OnlyAllowFirstCard = 4;

		// Token: 0x04001B35 RID: 6965
		public bool bStopFingerprintComm;

		// Token: 0x04001B36 RID: 6966
		public int fingerTotal = -1;

		// Token: 0x04001B37 RID: 6967
		public int fingerTotalValid = -1;

		// Token: 0x04001B38 RID: 6968
		private int m_commLoginID;

		// Token: 0x04001B39 RID: 6969
		private int m_ControllerDriverVer;

		// Token: 0x04001B3A RID: 6970
		private int m_ControllerProductTypeCode;

		// Token: 0x04001B3B RID: 6971
		private int m_ControllerSN;

		// Token: 0x04001B3C RID: 6972
		private string m_IP = "";

		// Token: 0x04001B3D RID: 6973
		private string m_MACAddr = "";

		// Token: 0x04001B3E RID: 6974
		private string m_PCIPAddress = "";

		// Token: 0x04001B3F RID: 6975
		private int m_PORT = 60000;

		// Token: 0x04001B40 RID: 6976
		private byte[] m_pwd = new byte[] { 54, 53, 52, 51, 50, 49 };

		// Token: 0x04001B41 RID: 6977
		private wgMjControllerRunInformation m_runinfo = new wgMjControllerRunInformation();

		// Token: 0x04001B42 RID: 6978
		private int priMaxTries = 3;

		// Token: 0x04001B43 RID: 6979
		private int sleep4Tries = 300;

		// Token: 0x04001B44 RID: 6980
		private int tag;

		// Token: 0x04001B45 RID: 6981
		private TcpClient tcp;

		// Token: 0x04001B46 RID: 6982
		private wgUdpComm wgudp;

		// Token: 0x020001F4 RID: 500
		public class WGPacketShort
		{
			// Token: 0x06000CF9 RID: 3321 RVA: 0x00102004 File Offset: 0x00101004
			public WGPacketShort()
			{
				this.type = 23;
				this.code = 64;
				this.crcCheckVal = 0;
				this.iDevSn = 0U;
				for (int i = 0; i < this.data.Length; i++)
				{
					this.data[i] = 0;
				}
				wgMjController.WGPacketShort.gsequenceID += 1U;
				this.sequenceId = wgMjController.WGPacketShort.gsequenceID;
			}

			// Token: 0x06000CFA RID: 3322 RVA: 0x00102078 File Offset: 0x00101078
			private static ushort calCRC_WGPacket(ushort len, byte[] data)
			{
				IntPtr intPtr = Marshal.AllocHGlobal((int)len);
				Marshal.Copy(data, 0, intPtr, (int)len);
				int num = wgMjController.WGPacketShort.calCRC_WGPacketShort((int)len, intPtr);
				Marshal.FreeHGlobal(intPtr);
				return (ushort)num;
			}

			// Token: 0x06000CFB RID: 3323
			[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
			private static extern int calCRC_WGPacketShort(int len, IntPtr pwgPkt);

			// Token: 0x06000CFC RID: 3324 RVA: 0x001020A8 File Offset: 0x001010A8
			public static int dec64(ref byte[] data)
			{
				if (data == null)
				{
					return 1;
				}
				if (data.Length != 64)
				{
					return 1;
				}
				IntPtr intPtr = Marshal.AllocHGlobal(64);
				Marshal.Copy(data, 0, intPtr, 64);
				int num = wgMjController.WGPacketShort.decShort(intPtr);
				if (num > 0)
				{
					Marshal.Copy(intPtr, data, 0, 64);
				}
				Marshal.FreeHGlobal(intPtr);
				return num;
			}

			// Token: 0x06000CFD RID: 3325
			[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, EntryPoint = "dec64")]
			public static extern int decShort(IntPtr pwgPkt);

			// Token: 0x06000CFE RID: 3326 RVA: 0x001020F8 File Offset: 0x001010F8
			public static int enc64(ref byte[] data)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(64);
				Marshal.Copy(data, 0, intPtr, 64);
				int num = wgMjController.WGPacketShort.encShort(intPtr);
				if (num > 0)
				{
					Marshal.Copy(intPtr, data, 0, 64);
				}
				Marshal.FreeHGlobal(intPtr);
				return num;
			}

			// Token: 0x06000CFF RID: 3327
			[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, EntryPoint = "enc64")]
			private static extern int encShort(IntPtr pwgPkt);

			// Token: 0x06000D00 RID: 3328 RVA: 0x00102138 File Offset: 0x00101138
			public byte[] ToBytes()
			{
				byte[] array = new byte[64];
				array[0] = this.type;
				array[1] = this.code;
				this.crcCheckVal = 0;
				Array.Copy(BitConverter.GetBytes(this.crcCheckVal), 0, array, 2, 2);
				Array.Copy(BitConverter.GetBytes(this.iDevSn), 0, array, 4, 4);
				Array.Copy(this.data, 0, array, 8, 56);
				wgMjController.WGPacketShort.gsequenceID += 1U;
				this.sequenceId = wgMjController.WGPacketShort.gsequenceID;
				Array.Copy(BitConverter.GetBytes(this.sequenceId), 0, array, 40, 4);
				byte[] array2 = new byte[60];
				Array.Copy(array, 4, array2, 0, 60);
				Array.Copy(BitConverter.GetBytes(wgMjController.WGPacketShort.calCRC_WGPacket(60, array2)), 0, array, 2, 2);
				return array;
			}

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x06000D01 RID: 3329 RVA: 0x001021F4 File Offset: 0x001011F4
			// (set) Token: 0x06000D02 RID: 3330 RVA: 0x001021FC File Offset: 0x001011FC
			public byte code
			{
				get
				{
					return this._code;
				}
				set
				{
					this._code = value;
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00102205 File Offset: 0x00101205
			// (set) Token: 0x06000D04 RID: 3332 RVA: 0x0010220D File Offset: 0x0010120D
			public short crcCheckVal
			{
				get
				{
					return this._crcCheckVal;
				}
				set
				{
					this._crcCheckVal = value;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00102216 File Offset: 0x00101216
			// (set) Token: 0x06000D06 RID: 3334 RVA: 0x0010221E File Offset: 0x0010121E
			public uint iDevSn
			{
				get
				{
					return this._iDevSn;
				}
				set
				{
					this._iDevSn = value;
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00102227 File Offset: 0x00101227
			// (set) Token: 0x06000D08 RID: 3336 RVA: 0x0010222F File Offset: 0x0010122F
			public uint sequenceId
			{
				get
				{
					return this._sequenceId;
				}
				set
				{
					this._sequenceId = value;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00102238 File Offset: 0x00101238
			// (set) Token: 0x06000D0A RID: 3338 RVA: 0x00102240 File Offset: 0x00101240
			public byte type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._type = value;
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00102249 File Offset: 0x00101249
			// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00102251 File Offset: 0x00101251
			public uint xid
			{
				get
				{
					return this.sequenceId;
				}
				set
				{
					this.sequenceId = value;
				}
			}

			// Token: 0x04001B47 RID: 6983
			public const int PacketLen = 64;

			// Token: 0x04001B48 RID: 6984
			public const long SpecialFlag = 1437248085L;

			// Token: 0x04001B49 RID: 6985
			private byte _type;

			// Token: 0x04001B4A RID: 6986
			private byte _code;

			// Token: 0x04001B4B RID: 6987
			private short _crcCheckVal;

			// Token: 0x04001B4C RID: 6988
			private uint _iDevSn;

			// Token: 0x04001B4D RID: 6989
			public byte[] data = new byte[56];

			// Token: 0x04001B4E RID: 6990
			private uint _sequenceId;

			// Token: 0x04001B4F RID: 6991
			private static uint gsequenceID = 1U;
		}
	}
}
