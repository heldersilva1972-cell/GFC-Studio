using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F9 RID: 505
	public class WGPacket
	{
		// Token: 0x06000E1D RID: 3613 RVA: 0x001068BC File Offset: 0x001058BC
		public WGPacket()
		{
			this.driverVer = 129;
			this.reserved18 = (byte)wgTools.gPTC_internal;
			WGPacket._Global_xid += 1U;
			this._xid = WGPacket._Global_xid;
			this.GetCommP();
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x001068F8 File Offset: 0x001058F8
		public WGPacket(byte[] rcvdata)
		{
			this.driverVer = 129;
			this.reserved18 = (byte)wgTools.gPTC_internal;
			this.type = rcvdata[0];
			this.code = rcvdata[1];
			this._xid = BitConverter.ToUInt32(rcvdata, 4);
			this.iDevSnFrom = BitConverter.ToUInt32(rcvdata, 8);
			this.iDevSnTo = BitConverter.ToUInt32(rcvdata, 12);
			this.iCallReturn = rcvdata[16];
			this.driverVer = rcvdata[17];
			this.reserved18 = rcvdata[18];
			this.reserved19 = rcvdata[19];
			this.GetCommP();
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0010698C File Offset: 0x0010598C
		public WGPacket(byte typePar, byte codePar, uint snToPar)
		{
			this.driverVer = 129;
			this.reserved18 = (byte)wgTools.gPTC_internal;
			this.type = typePar;
			this.code = codePar;
			this.iDevSnFrom = 0U;
			this.iDevSnTo = snToPar;
			this.iCallReturn = 0;
			this.driverVer = 0;
			this.reserved18 = (byte)wgTools.gPTC_internal;
			this.reserved19 = 0;
			WGPacket._Global_xid += 1U;
			this._xid = WGPacket._Global_xid;
			this.GetCommP();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00106A10 File Offset: 0x00105A10
		private static bool CrcCheck(byte[] rcvdata, int ud_srcp)
		{
			ushort num = BitConverter.ToUInt16(rcvdata, 2);
			byte b = rcvdata[2];
			byte b2 = rcvdata[3];
			rcvdata[2] = (byte)(ud_srcp & 255);
			rcvdata[3] = (byte)((ud_srcp >> 8) & 255);
			ushort num2 = wgCRC.CRC_16_IBM_CSharp((uint)rcvdata.Length, rcvdata);
			rcvdata[2] = b;
			rcvdata[3] = b2;
			return num2 == num;
		}

		// Token: 0x06000E21 RID: 3617
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int enc(IntPtr pwgPkt, int len, IntPtr k);

		// Token: 0x06000E22 RID: 3618 RVA: 0x00106A5C File Offset: 0x00105A5C
		protected internal void EncWGPacket(ref byte[] pwgPktBytes, int len)
		{
			if (WGPacket.bCommP && len >= 4)
			{
				if (len >= 20)
				{
					int num = ((int)pwgPktBytes[15] << 24) + ((int)pwgPktBytes[14] << 16) + ((int)pwgPktBytes[13] << 8) + (int)pwgPktBytes[12];
					if (num >= 160100000 && num <= 160999999)
					{
						return;
					}
				}
				byte[] array = new byte[16];
				char[] array2 = WGPacket.Dpt(WGPacket.m_strCommP).PadRight(16, '\0').ToCharArray();
				for (int i = 0; i < 16; i++)
				{
					array[i] = (byte)(array2[i] & 'ÿ');
				}
				IntPtr intPtr = Marshal.AllocHGlobal(len);
				IntPtr intPtr2 = Marshal.AllocHGlobal(16);
				Marshal.Copy(pwgPktBytes, 0, intPtr, len);
				Marshal.Copy(array, 0, intPtr2, 16);
				WGPacket.enc(intPtr, len, intPtr2);
				Marshal.Copy(intPtr, pwgPktBytes, 0, len);
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
		}

		// Token: 0x06000E23 RID: 3619
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int dec(IntPtr pwgPkt, int len, IntPtr k);

		// Token: 0x06000E24 RID: 3620 RVA: 0x00106B38 File Offset: 0x00105B38
		public static void DecWGPacket(ref byte[] pwgPktBytes, int len)
		{
			if (WGPacket.bCommP && len >= 4 && (pwgPktBytes[0] & 128) != 0)
			{
				byte[] array = new byte[16];
				char[] array2 = WGPacket.Dpt(WGPacket.m_strCommP).PadRight(16, '\0').ToCharArray();
				for (int i = 0; i < 16; i++)
				{
					array[i] = (byte)(array2[i] & 'ÿ');
				}
				IntPtr intPtr = Marshal.AllocHGlobal(len);
				IntPtr intPtr2 = Marshal.AllocHGlobal(16);
				Marshal.Copy(pwgPktBytes, 0, intPtr, len);
				Marshal.Copy(array, 0, intPtr2, 16);
				WGPacket.dec(intPtr, len, intPtr2);
				Marshal.Copy(intPtr, pwgPktBytes, 0, len);
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00106BE8 File Offset: 0x00105BE8
		public static string Ept(string StrInput)
		{
			string text = "";
			try
			{
				byte[] bytes = Encoding.Default.GetBytes(StrInput);
				if (WGPacket.Key == null)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(16);
					IntPtr intPtr2 = Marshal.AllocHGlobal(16);
					WGPacket.getK(intPtr);
					WGPacket.getIV(intPtr2);
					WGPacket.Key = new byte[16];
					Marshal.Copy(intPtr, WGPacket.Key, 0, 16);
					WGPacket.IV = new byte[16];
					Marshal.Copy(intPtr2, WGPacket.IV, 0, 16);
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
					{
						CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(WGPacket.Key, WGPacket.IV), CryptoStreamMode.Write);
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
						text = Convert.ToBase64String(memoryStream.ToArray());
					}
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00106D00 File Offset: 0x00105D00
		public static string Dpt(string StrInput)
		{
			string text = "";
			try
			{
				byte[] array = Convert.FromBase64String(StrInput);
				if (WGPacket.Key == null)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(16);
					IntPtr intPtr2 = Marshal.AllocHGlobal(16);
					WGPacket.getK(intPtr);
					WGPacket.getIV(intPtr2);
					WGPacket.Key = new byte[16];
					Marshal.Copy(intPtr, WGPacket.Key, 0, 16);
					WGPacket.IV = new byte[16];
					Marshal.Copy(intPtr2, WGPacket.IV, 0, 16);
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
					{
						CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(WGPacket.Key, WGPacket.IV), CryptoStreamMode.Write);
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						text = Encoding.Default.GetString(memoryStream.ToArray());
					}
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00106E18 File Offset: 0x00105E18
		private void GetCommP()
		{
			if (string.IsNullOrEmpty(wgTools.CommPStr))
			{
				WGPacket.m_bCommPassword = false;
				WGPacket.m_strCommP = "";
				return;
			}
			WGPacket.m_bCommPassword = true;
			WGPacket.m_strCommP = wgTools.CommPStr;
		}

		// Token: 0x06000E28 RID: 3624
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getIV(IntPtr k);

		// Token: 0x06000E29 RID: 3625
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int getK(IntPtr k);

		// Token: 0x06000E2A RID: 3626 RVA: 0x00106E47 File Offset: 0x00105E47
		public void GetNewXid()
		{
			WGPacket._Global_xid += 1U;
			this._xid = WGPacket._Global_xid;
		}

		// Token: 0x06000E2B RID: 3627
		[DllImport("n3k_comm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		public static extern int gettexCPBcVbYG8tfyU();

		// Token: 0x06000E2C RID: 3628 RVA: 0x00106E60 File Offset: 0x00105E60
		internal static int Parsing(ref byte[] rcvdata, int srcPort)
		{
			if (rcvdata.Length < WGPacket.MinSize)
			{
				return -101;
			}
			if ((rcvdata[0] & 128) > 0)
			{
				WGPacket.DecWGPacket(ref rcvdata, rcvdata.Length);
			}
			if (rcvdata.Length == 64 && wgMjController.WGPacketShort.dec64(ref rcvdata) == 0)
			{
				return -103;
			}
			if (!WGPacket.CrcCheck(rcvdata, srcPort))
			{
				if ((rcvdata.Length != 64 || rcvdata[0] != 25) && (rcvdata.Length != 64 || rcvdata[0] != 26))
				{
					if ((rcvdata.Length == 64 && rcvdata[0] == 19) || (rcvdata.Length == 64 && rcvdata[0] == 20))
					{
						return 1;
					}
					if ((rcvdata.Length == 64 && rcvdata[0] == 17) || (rcvdata.Length == 64 && rcvdata[0] == 18))
					{
						return 1;
					}
					if ((rcvdata.Length == 64 && rcvdata[0] == 21) || (rcvdata.Length == 64 && rcvdata[0] == 22))
					{
						return 1;
					}
					if ((rcvdata.Length != 64 || rcvdata[0] != 23) && (rcvdata.Length != 64 || rcvdata[0] != 24))
					{
						return -103;
					}
				}
				return 1;
			}
			switch (rcvdata[0] & 127)
			{
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
				return 1;
			default:
				return -105;
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00106F93 File Offset: 0x00105F93
		public static int Parsing4Watching(ref byte[] rcvdata, int srcPort)
		{
			return WGPacket.Parsing(ref rcvdata, srcPort);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00106F9C File Offset: 0x00105F9C
		public byte[] ToBytes(ushort srcPort)
		{
			byte[] array = new byte[WGPacket.MinSize];
			array[0] = this.type;
			array[1] = this.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this.xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnTo), 0, array, 12, 4);
			array[16] = this.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp((uint)WGPacket.MinSize, array)), 0, array, 2, 2);
			this.EncWGPacket(ref array, array.Length);
			return array;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00107060 File Offset: 0x00106060
		public byte[] ToBytesNoPassword(ushort srcPort)
		{
			byte[] array = new byte[WGPacket.MinSize];
			array[0] = this.type;
			array[1] = this.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this.xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnTo), 0, array, 12, 4);
			array[16] = this.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)wgTools.gPTC_internal;
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp((uint)WGPacket.MinSize, array)), 0, array, 2, 2);
			return array;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00107118 File Offset: 0x00106118
		public byte[] ToBytesNoPasswordAllProd(ushort srcPort)
		{
			byte[] array = new byte[WGPacket.MinSize];
			array[0] = this.type;
			array[1] = this.code;
			Array.Copy(BitConverter.GetBytes(srcPort), 0, array, 2, 2);
			Array.Copy(BitConverter.GetBytes(this.xid), 0, array, 4, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnFrom), 0, array, 8, 4);
			Array.Copy(BitConverter.GetBytes(this.iDevSnTo), 0, array, 12, 4);
			array[16] = this.iCallReturn;
			array[17] = this.driverVer;
			array[18] = (byte)WGPacket.gettexCPBcVbYG8tfyU();
			array[19] = this.reserved19;
			Array.Copy(BitConverter.GetBytes(wgCRC.CRC_16_IBM_CSharp((uint)WGPacket.MinSize, array)), 0, array, 2, 2);
			return array;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x001071D0 File Offset: 0x001061D0
		// (set) Token: 0x06000E32 RID: 3634 RVA: 0x001071D7 File Offset: 0x001061D7
		public static bool bCommP
		{
			get
			{
				return WGPacket.m_bCommPassword;
			}
			set
			{
				WGPacket.m_bCommPassword = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x001071DF File Offset: 0x001061DF
		// (set) Token: 0x06000E34 RID: 3636 RVA: 0x001071E7 File Offset: 0x001061E7
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x001071F0 File Offset: 0x001061F0
		// (set) Token: 0x06000E36 RID: 3638 RVA: 0x00107202 File Offset: 0x00106202
		public byte iCallReturn
		{
			get
			{
				return (byte)((int)this.m_iCallReturn_real | ((int)this.m_iCallReturn_loginID << 1));
			}
			set
			{
				this.m_iCallReturn_real = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0010720B File Offset: 0x0010620B
		// (set) Token: 0x06000E38 RID: 3640 RVA: 0x00107213 File Offset: 0x00106213
		public byte iCallReturn_loginID
		{
			get
			{
				return this.m_iCallReturn_loginID;
			}
			set
			{
				this.m_iCallReturn_loginID = value & 7;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x0010721F File Offset: 0x0010621F
		// (set) Token: 0x06000E3A RID: 3642 RVA: 0x00107227 File Offset: 0x00106227
		public uint iDevSnFrom
		{
			get
			{
				return this._iDevSnFrom;
			}
			set
			{
				this._iDevSnFrom = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00107230 File Offset: 0x00106230
		// (set) Token: 0x06000E3C RID: 3644 RVA: 0x00107238 File Offset: 0x00106238
		public uint iDevSnTo
		{
			get
			{
				return this._iDevSnTo;
			}
			set
			{
				this._iDevSnTo = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00107241 File Offset: 0x00106241
		public static int MinSize
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x1700017E RID: 382
		// (set) Token: 0x06000E3E RID: 3646 RVA: 0x00107245 File Offset: 0x00106245
		public static string strCommP
		{
			set
			{
				WGPacket.m_strCommP = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0010724D File Offset: 0x0010624D
		// (set) Token: 0x06000E40 RID: 3648 RVA: 0x00107255 File Offset: 0x00106255
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

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0010725E File Offset: 0x0010625E
		public uint xid
		{
			get
			{
				return this._xid;
			}
		}

		// Token: 0x04001B6B RID: 7019
		private const int SUCCESS = 1;

		// Token: 0x04001B6C RID: 7020
		private const int WGP_INVALID = -101;

		// Token: 0x04001B6D RID: 7021
		private const int WGP_ALLOC_ERR = -102;

		// Token: 0x04001B6E RID: 7022
		private const int WGP_CRC_ERR = -103;

		// Token: 0x04001B6F RID: 7023
		private const int WGP_CODE_ERR = -104;

		// Token: 0x04001B70 RID: 7024
		private const int WGP_TYPE_ERR = -105;

		// Token: 0x04001B71 RID: 7025
		private const int WGP_SELF_SEND = -106;

		// Token: 0x04001B72 RID: 7026
		private const int WGP_NOT_ME = -107;

		// Token: 0x04001B73 RID: 7027
		private const int WGP_INVALID_AES = -108;

		// Token: 0x04001B74 RID: 7028
		private static uint _Global_xid = 0U;

		// Token: 0x04001B75 RID: 7029
		private static byte[] IV = null;

		// Token: 0x04001B76 RID: 7030
		private static byte[] Key = null;

		// Token: 0x04001B77 RID: 7031
		private static bool m_bCommPassword = false;

		// Token: 0x04001B78 RID: 7032
		private static string m_strCommP = "";

		// Token: 0x04001B79 RID: 7033
		private byte _type;

		// Token: 0x04001B7A RID: 7034
		private byte _code;

		// Token: 0x04001B7B RID: 7035
		private uint _iDevSnFrom;

		// Token: 0x04001B7C RID: 7036
		private uint _iDevSnTo;

		// Token: 0x04001B7D RID: 7037
		private byte m_iCallReturn_loginID;

		// Token: 0x04001B7E RID: 7038
		private byte m_iCallReturn_real;

		// Token: 0x04001B7F RID: 7039
		internal uint _xid;

		// Token: 0x04001B80 RID: 7040
		protected internal byte driverVer;

		// Token: 0x04001B81 RID: 7041
		protected internal byte reserved18;

		// Token: 0x04001B82 RID: 7042
		protected internal byte reserved19;
	}
}
