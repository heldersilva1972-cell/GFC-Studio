using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WG3000_COMM.Core
{
	// Token: 0x02000213 RID: 531
	public class wgTools
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x001096E8 File Offset: 0x001086E8
		private wgTools()
		{
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x001096F0 File Offset: 0x001086F0
		public static bool checkNetLinked()
		{
			string text = "";
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						num++;
						if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						{
							num2++;
						}
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							bool flag = true;
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									if (flag)
									{
										num3++;
										flag = false;
									}
									text += string.Format("{0}, ", unicastIPAddressInformation.Address.ToString());
									Console.WriteLine();
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			return !string.IsNullOrEmpty(text);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0010985C File Offset: 0x0010885C
		public static int CmpProductVersion(string newVersion, string productVersion)
		{
			try
			{
				if (wgTools.SetObjToStr(newVersion) == wgTools.SetObjToStr(productVersion))
				{
					return 0;
				}
				if (wgTools.SetObjToStr(newVersion) != "")
				{
					if (wgTools.SetObjToStr(productVersion) == "")
					{
						return 1;
					}
					if (newVersion == productVersion)
					{
						return 0;
					}
					string[] array = newVersion.Split(new char[] { '.' });
					string[] array2 = productVersion.Split(new char[] { '.' });
					for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
					{
						if (int.Parse(array[i]) > int.Parse(array2[i]))
						{
							return 1;
						}
						if (int.Parse(array[i]) < int.Parse(array2[i]))
						{
							return -1;
						}
					}
					if (array.Length == array2.Length)
					{
						return 0;
					}
					if (array.Length > array2.Length)
					{
						return 1;
					}
				}
				return -1;
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("cmpProductVersion" + ex.ToString());
			}
			return -2;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00109984 File Offset: 0x00108984
		public static void Compress(FileInfo fi)
		{
			using (FileStream fileStream = fi.OpenRead())
			{
				if (((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) & (fi.Extension != ".gz"))
				{
					using (FileStream fileStream2 = File.Create(fi.FullName + ".gz"))
					{
						using (GZipStream gzipStream = new GZipStream(fileStream2, CompressionMode.Compress))
						{
							wgTools.CopyStream(fileStream, gzipStream);
						}
					}
				}
			}
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00109A30 File Offset: 0x00108A30
		private static void CopyStream(Stream input, Stream output)
		{
			byte[] array = new byte[32768];
			for (;;)
			{
				int num = input.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				output.Write(array, 0, num);
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00109A64 File Offset: 0x00108A64
		public static void Decompress(FileInfo fi)
		{
			using (FileStream fileStream = fi.OpenRead())
			{
				string fullName = fi.FullName;
				using (FileStream fileStream2 = File.Create(fullName.Remove(fullName.Length - fi.Extension.Length)))
				{
					using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
					{
						wgTools.CopyStream(gzipStream, fileStream2);
					}
				}
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00109AF8 File Offset: 0x00108AF8
		public static void delay4SendNextCommand_P64()
		{
			if (wgTools.bUDPOnly64 > 0)
			{
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00109B0C File Offset: 0x00108B0C
		public static string deleInvalid(string dateval)
		{
			string text = dateval.ToUpper();
			try
			{
				char[] array = dateval.ToCharArray();
				string text2 = "";
				char c = '\0';
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] >= '0' && array[i] <= '9')
					{
						c = '\0';
						text2 += array[i];
					}
					else if (!array[i].Equals(c) && (array[i].Equals(' ') || array[i].Equals('/') || array[i].Equals('-') || array[i].Equals('.') || array[i].Equals(':')))
					{
						c = array[i];
						text2 += array[i];
					}
				}
				text = text2;
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00109C00 File Offset: 0x00108C00
		public static double doubleParse(string val)
		{
			double num = 0.0;
			try
			{
				if (!string.IsNullOrEmpty(val))
				{
					num = double.Parse(val, CultureInfo.InvariantCulture);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				try
				{
					wgAppConfig.wgLog("val=" + val);
				}
				catch (Exception ex2)
				{
					wgAppConfig.wgLog("val Invalid");
					wgAppConfig.wgLog(ex2.ToString());
				}
			}
			return num;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00109C84 File Offset: 0x00108C84
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

		// Token: 0x06000EAF RID: 3759 RVA: 0x00109CB8 File Offset: 0x00108CB8
		public static string getDate(string strDate)
		{
			string text = strDate;
			try
			{
				DateTime dateTime;
				if (DateTime.TryParse(strDate, out dateTime))
				{
					text = dateTime.ToString("yyyy-MM-dd");
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00109CF4 File Offset: 0x00108CF4
		public static double GetHardDiskFreeSpace(string DiskName)
		{
			double num = 0.0;
			DiskName = DiskName.ToUpper() + ":\\";
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.Name == DiskName)
				{
					num = (double)driveInfo.TotalFreeSpace / 1073741824.0;
				}
			}
			return num;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00109D58 File Offset: 0x00108D58
		public static double GetHardDiskSpace(string DiskName)
		{
			double num = 0.0;
			DiskName = DiskName.ToUpper() + ":\\";
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.Name == DiskName)
				{
					num = (double)driveInfo.TotalSize / 1073741824.0;
				}
			}
			return num;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00109DBA File Offset: 0x00108DBA
		public static int GetHex(int val)
		{
			return val % 10 + (val - val % 10) / 10 % 10 * 16;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00109DD0 File Offset: 0x00108DD0
		public static void getUdpComm(ref wgUdpComm wgudp, int UDPQueue4MultithreadIndexParam = -1)
		{
			if (wgudp == null)
			{
				wgudp = new wgUdpComm(UDPQueue4MultithreadIndexParam);
				if (wgTools.bUDPCloud <= 0)
				{
					Thread.Sleep(300);
				}
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00109DF0 File Offset: 0x00108DF0
		private static bool i2cchecktime(int Year, int Month, int Day, int Hour, int Minute, int Second)
		{
			return Second <= 59 && Minute <= 59 && Hour <= 23 && Day <= 31 && (Day != 0 && Month <= 12) && (Month != 0 && Year <= 99) && Year != 0;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00109E28 File Offset: 0x00108E28
		public static int IPLng(IPAddress ip)
		{
			byte[] array = new byte[4];
			array = ip.GetAddressBytes();
			return ((int)array[3] << 24) + ((int)array[2] << 16) + ((int)array[1] << 8) + (int)array[0];
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00109E5C File Offset: 0x00108E5C
		public static bool IsAllFF(byte[] arrbyt, int startIndex, int len)
		{
			if (startIndex >= arrbyt.Length || startIndex + len > arrbyt.Length)
			{
				return false;
			}
			for (int i = startIndex; i < startIndex + len; i++)
			{
				if (arrbyt[i] != 255)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00109E94 File Offset: 0x00108E94
		public static bool IsValidDateTimeFormat(string dateTimeFormat)
		{
			bool flag = false;
			try
			{
				if (string.IsNullOrEmpty(dateTimeFormat))
				{
					return true;
				}
				DateTime dateTime;
				if (DateTime.TryParse(DateTime.Now.ToString(dateTimeFormat), out dateTime))
				{
					flag = true;
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00109EE0 File Offset: 0x00108EE0
		public static void LongTo4Bytes(ref byte[] outBytes, int startIndex, long val)
		{
			Array.Copy(BitConverter.GetBytes(val), 0, outBytes, startIndex, 4);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00109EF4 File Offset: 0x00108EF4
		internal static uint MsDateToWgDate(DateTime dt)
		{
			int num = dt.Year % 100 << 9;
			num += dt.Month << 5;
			num += dt.Day;
			num += dt.Second >> 1 << 16;
			num += dt.Minute << 21;
			return (uint)(num + (dt.Hour << 27));
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00109F50 File Offset: 0x00108F50
		internal static uint MsDateToWgDateHMS(DateTime dt)
		{
			int num = 0;
			num += dt.Second >> 1;
			num += dt.Minute << 5;
			return (uint)(num + (dt.Hour << 11));
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00109F88 File Offset: 0x00108F88
		internal static uint MsDateToWgDateYMD(DateTime dt)
		{
			int num = dt.Year % 100 << 9;
			num += dt.Month << 5;
			return (uint)(num + dt.Day);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00109FBC File Offset: 0x00108FBC
		public static string PrepareStr(object obj)
		{
			if (obj == null)
			{
				return "NULL";
			}
			if (obj == DBNull.Value)
			{
				return "NULL";
			}
			if (obj.ToString().Trim() == "")
			{
				return "NULL";
			}
			string text = obj.ToString();
			if (wgTools.IsSqlServer)
			{
				text = obj.ToString().Replace("'", "''");
				return "'" + text + "'";
			}
			text = obj.ToString().Replace("'", "''");
			return "'" + text + "'";
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0010A058 File Offset: 0x00109058
		public static string PrepareStr(object obj, bool bDate, string dateFormat)
		{
			if (obj == null)
			{
				return "NULL";
			}
			if (obj == DBNull.Value)
			{
				return "NULL";
			}
			if (obj.ToString().Trim() == "")
			{
				return "NULL";
			}
			string text = obj.ToString();
			if (bDate)
			{
				if (wgTools.IsSqlServer)
				{
					if (dateFormat == "")
					{
						return "CONVERT( datetime, '" + wgTools.deleInvalid(text.Trim()) + "',120)";
					}
					try
					{
						return "CONVERT( datetime, '" + wgTools.deleInvalid(((DateTime)obj).ToString(dateFormat)) + "',120)";
					}
					catch
					{
					}
					return "CONVERT( datetime, '" + wgTools.deleInvalid(DateTime.Parse(obj.ToString()).ToString(dateFormat)) + "',120)";
				}
				else
				{
					if (dateFormat == "")
					{
						return "#" + wgTools.deleInvalid(text.Trim()) + "#";
					}
					try
					{
						return "#" + wgTools.deleInvalid(((DateTime)obj).ToString(dateFormat)) + "#";
					}
					catch
					{
					}
					return "#" + wgTools.deleInvalid(DateTime.Parse(obj.ToString()).ToString(dateFormat)) + "#";
				}
				string text2;
				return text2;
			}
			if (!wgTools.IsSqlServer)
			{
				return "'" + text + "'";
			}
			if (dateFormat == "")
			{
				text = obj.ToString().Replace("'", "''");
				return "'" + text.Trim() + "'";
			}
			return "'" + DateTime.Parse(obj.ToString()).ToString(dateFormat) + "'";
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0010A248 File Offset: 0x00109248
		public static string PrepareStrNUnicode(object obj)
		{
			if (obj == null)
			{
				return "NULL";
			}
			if (obj == DBNull.Value)
			{
				return "NULL";
			}
			if (obj.ToString().Trim() == "")
			{
				return "NULL";
			}
			string text = obj.ToString();
			if (wgTools.IsSqlServer)
			{
				text = obj.ToString().Replace("'", "''");
				return "N'" + text + "'";
			}
			text = obj.ToString().Replace("'", "''");
			return "'" + text + "'";
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0010A2E4 File Offset: 0x001092E4
		public static string ReadTextFile(string path)
		{
			string text;
			using (StreamReader streamReader = new StreamReader(path, new UnicodeEncoding()))
			{
				text = streamReader.ReadToEnd();
			}
			return text;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0010A324 File Offset: 0x00109324
		public static string SetObjToStr(object valA)
		{
			string text = "";
			try
			{
				if (valA != null && DBNull.Value != valA)
				{
					text = valA.ToString().Trim();
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0010A364 File Offset: 0x00109364
		public static long SignedCard(long CardNo)
		{
			if (CardNo >= (long)((ulong)(-1)))
			{
				return CardNo - 4294967296L;
			}
			return CardNo;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0010A378 File Offset: 0x00109378
		public static int UDPCloudGet(byte[] cmd, ref string ip, ref int port)
		{
			if (wgTools.bUDPCloud == 1)
			{
				if (wgTools.arrSNReceived.Count == 0)
				{
					return 0;
				}
				if (wgTools.wgcloud == null)
				{
					return 0;
				}
				uint num;
				if (cmd.Length == 64)
				{
					num = BitConverter.ToUInt32(cmd, 4);
				}
				else
				{
					num = BitConverter.ToUInt32(cmd, 12);
					if ((cmd[0] & 128) > 0)
					{
						byte[] array = new byte[cmd.Length];
						cmd.CopyTo(array, 0);
						WGPacket.DecWGPacket(ref array, array.Length);
						num = BitConverter.ToUInt32(array, 12);
					}
				}
				if (wgTools.GetControllerType((int)num) != 0)
				{
					int num2 = wgTools.arrSNReceived.IndexOf(num);
					if (num2 >= 0)
					{
						ip = (string)wgTools.arrSNIP[num2];
						port = (int)wgTools.arrSNPort[num2];
						return 1;
					}
				}
			}
			return 0;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0010A438 File Offset: 0x00109438
		public static EndPoint UDPCloudGetListenPoint()
		{
			if (wgTools.bUDPCloud == 1)
			{
				try
				{
					if (!string.IsNullOrEmpty(wgTools.UDPCloudIP))
					{
						return new IPEndPoint(IPAddress.Parse(wgTools.UDPCloudIP), wgTools.UDPCloudShortPort);
					}
					return new IPEndPoint(IPAddress.Any, wgTools.UDPCloudShortPort);
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			return null;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0010A4AC File Offset: 0x001094AC
		public static DateTime UDPCloudGetRefreshTime(int sn)
		{
			int num = wgTools.arrSNReceived.IndexOf((uint)sn);
			if (num >= 0)
			{
				return (DateTime)wgTools.arrRefreshTime[num];
			}
			return DateTime.Parse("1970-1-1");
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0010A4EC File Offset: 0x001094EC
		public static void UDPCloudUpdate(uint sn, string ip, int port)
		{
			if (wgTools.bUDPCloud == 1 && wgTools.GetControllerType((int)sn) != 0)
			{
				if (port == 60000)
				{
					IPAddress ipaddress = IPAddress.Parse(wgTools.UDPCloudIP);
					IPAddress ipaddress2 = IPAddress.Parse(ip);
					if (((long)BitConverter.ToInt32(ipaddress.GetAddressBytes(), 0) & (long)((ulong)(-16777217))) == ((long)BitConverter.ToInt32(ipaddress2.GetAddressBytes(), 0) & (long)((ulong)(-16777217))))
					{
						return;
					}
				}
				int num = wgTools.arrSNReceived.IndexOf(sn);
				if (num < 0)
				{
					wgTools.arrSNReceived.Add(sn);
					wgTools.arrSNIP.Add(ip);
					wgTools.arrSNPort.Add(port);
					wgTools.arrRefreshTime.Add(DateTime.Now);
					wgTools.arrCreateTime.Add(DateTime.Now);
					return;
				}
				wgTools.arrSNIP[num] = ip;
				wgTools.arrSNPort[num] = port;
				wgTools.arrRefreshTime[num] = DateTime.Now;
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0010A5F4 File Offset: 0x001095F4
		public static void UDPCloudUpdateByBytes(byte[] rcvbyt2, IPEndPoint senderRemote)
		{
			if (wgTools.bUDPCloud == 1)
			{
				uint num;
				if (rcvbyt2.Length == 64)
				{
					num = BitConverter.ToUInt32(rcvbyt2, 4);
				}
				else
				{
					if (rcvbyt2.Length < 20)
					{
						return;
					}
					num = BitConverter.ToUInt32(rcvbyt2, 8);
				}
				if (num > 100000000U && num < 500000000U)
				{
					wgTools.UDPCloudUpdate(num, senderRemote.Address.ToString(), senderRemote.Port);
				}
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0010A653 File Offset: 0x00109653
		public static long UnsignedCard(long CardNo)
		{
			if (CardNo < 0L)
			{
				return CardNo + 4294967296L;
			}
			return CardNo;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0010A668 File Offset: 0x00109668
		public static string validCloudServerIP(string ip)
		{
			string text = ip;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				ArrayList arrayList = new ArrayList();
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						num++;
						if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
						{
							num2++;
						}
						UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
						if (unicastAddresses.Count > 0)
						{
							Console.WriteLine(networkInterface.Description);
							bool flag = true;
							foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
							{
								if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIPAddressInformation.Address.IsIPv6LinkLocal && unicastIPAddressInformation.Address.ToString() != "127.0.0.1")
								{
									if (flag)
									{
										num3++;
										flag = false;
									}
									arrayList.Add(unicastIPAddressInformation.Address.ToString());
									Console.WriteLine("  IP ............................. : {0}", unicastIPAddressInformation.Address.ToString());
								}
							}
							Console.WriteLine();
						}
					}
				}
				if (arrayList.Count > 0 && arrayList.IndexOf(ip) < 0)
				{
					text = (string)arrayList[0];
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			return text;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0010A80C File Offset: 0x0010980C
		public static DateTime wgDateTimeParse(object obj)
		{
			try
			{
				if (obj is DateTime)
				{
					return (DateTime)obj;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			try
			{
				return DateTime.Parse(obj.ToString());
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			return DateTime.Parse("2000-1-13");
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0010A88C File Offset: 0x0010988C
		internal static DateTime WgDateToMsDate(byte[] dts, int StartIndex)
		{
			ushort num = BitConverter.ToUInt16(dts, StartIndex);
			ushort num2 = BitConverter.ToUInt16(dts, StartIndex + 2);
			int num3 = (int)(num2 & 31) << 1;
			int num4 = (num2 >> 5) & 63;
			int num5 = num2 >> 11;
			int num6 = (int)(num & 31);
			int num7 = (num >> 5) & 15;
			int num8 = num >> 9;
			DateTime dateTime;
			try
			{
				if (wgTools.i2cchecktime(num8, num7, num6, num5, num4, num3))
				{
					dateTime = new DateTime(num8 + 2000, num7, num6, num5, num4, num3);
				}
				else
				{
					dateTime = new DateTime(2009, 1, 1, 0, 0, 0);
				}
			}
			catch (Exception)
			{
				dateTime = new DateTime(2009, 1, 1, 0, 0, 0);
			}
			return dateTime;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0010A938 File Offset: 0x00109938
		public static void WgDebugWrite(string info, params object[] args)
		{
			try
			{
				if (!string.IsNullOrEmpty(wgTools.strLogIntoFileName))
				{
					try
					{
						string text;
						if (args != null)
						{
							text = "WgDebugWrite****." + string.Format(info, args);
						}
						else
						{
							text = "WgDebugWrite****." + info;
						}
						text = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + " \t" + text;
						using (StreamWriter streamWriter = new StreamWriter(wgTools.strLogIntoFileName, true))
						{
							streamWriter.WriteLine(text);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0010A9E4 File Offset: 0x001099E4
		public static void WriteLine(string info)
		{
			wgTools.dtLast = DateTime.Now;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0010A9F0 File Offset: 0x001099F0
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x0010A9F7 File Offset: 0x001099F7
		public static string CommPStr
		{
			get
			{
				return wgTools.m_CommPasswordStr;
			}
			set
			{
				if (value != null)
				{
					wgTools.m_CommPasswordStr = value;
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0010AA02 File Offset: 0x00109A02
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0010AA09 File Offset: 0x00109A09
		public static string DisplayFormat_DateYMD
		{
			get
			{
				return wgTools.m_DisplayFormat_DateYMD;
			}
			set
			{
				wgTools.m_DisplayFormat_DateYMD = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0010AA11 File Offset: 0x00109A11
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x0010AA18 File Offset: 0x00109A18
		public static string DisplayFormat_DateYMDHMS
		{
			get
			{
				return wgTools.m_DisplayFormat_DateYMDHMS;
			}
			set
			{
				wgTools.m_DisplayFormat_DateYMDHMS = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0010AA20 File Offset: 0x00109A20
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x0010AA27 File Offset: 0x00109A27
		public static string DisplayFormat_DateYMDHMSWeek
		{
			get
			{
				return wgTools.m_DisplayFormat_DateYMDHMSWeek;
			}
			set
			{
				wgTools.m_DisplayFormat_DateYMDHMSWeek = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0010AA2F File Offset: 0x00109A2F
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0010AA36 File Offset: 0x00109A36
		public static string DisplayFormat_DateYMDWeek
		{
			get
			{
				return wgTools.m_DisplayFormat_DateYMDWeek;
			}
			set
			{
				wgTools.m_DisplayFormat_DateYMDWeek = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0010AA3E File Offset: 0x00109A3E
		public static bool gADCT
		{
			get
			{
				return wgTools.gPTC_internal == 1;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0010AA48 File Offset: 0x00109A48
		internal static bool gADCT_internal
		{
			get
			{
				return wgTools.gPTC_internal == 1;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0010AA52 File Offset: 0x00109A52
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0010AA59 File Offset: 0x00109A59
		public static int gate
		{
			get
			{
				return wgTools.m_gPTC;
			}
			set
			{
				wgTools.m_gPTC = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0010AA61 File Offset: 0x00109A61
		public static int gCRC19
		{
			get
			{
				return wgTools.m_crc19;
			}
		}

		// Token: 0x17000198 RID: 408
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x0010AA68 File Offset: 0x00109A68
		public static string gPTC
		{
			set
			{
				try
				{
					wgTools.m_gPTC = int.Parse(WGPacket.Dpt(value)) & 255;
					wgTools.m_crc19 = (int.Parse(WGPacket.Dpt(value)) >> 8) & 240;
				}
				catch
				{
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x0010AAB8 File Offset: 0x00109AB8
		internal static int gPTC_internal
		{
			get
			{
				return wgTools.m_gPTC;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x0010AABF File Offset: 0x00109ABF
		public static bool gWGYTJ
		{
			get
			{
				return wgAppConfig.ProductTypeOfApp == "AGYTJ";
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0010AAD0 File Offset: 0x00109AD0
		public static string MSGTITLE
		{
			get
			{
				return wgTools.m_MSGTITLE;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0010AAD7 File Offset: 0x00109AD7
		public static DateTime softwareStartTime
		{
			get
			{
				return wgTools.m_dtSoftwareStartTime;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0010AADE File Offset: 0x00109ADE
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0010AAE5 File Offset: 0x00109AE5
		public static string YMDHMSFormat
		{
			get
			{
				return wgTools.m_YMDHMSFormat;
			}
			set
			{
				wgTools.m_YMDHMSFormat = value;
			}
		}

		// Token: 0x04001BE1 RID: 7137
		public const string logDateTimeFormat = "yyyy-MM-dd HH-mm-ss.fff";

		// Token: 0x04001BE2 RID: 7138
		public const int COMM_UDP_PORT = 60000;

		// Token: 0x04001BE3 RID: 7139
		public const int ERR_PRIVILEGES_OVER200K = -100001;

		// Token: 0x04001BE4 RID: 7140
		public const int ERR_PRIVILEGES_STOPUPLOAD = -100002;

		// Token: 0x04001BE5 RID: 7141
		public const int ERR_SWIPERECORD_STOPGET = -200002;

		// Token: 0x04001BE6 RID: 7142
		public const int PRIVILEGES_UPLOADED = 100003;

		// Token: 0x04001BE7 RID: 7143
		public const int PRIVILEGES_UPLOADING = 100002;

		// Token: 0x04001BE8 RID: 7144
		public const int PRIVILEGES_UPLOADPREPARING = 100001;

		// Token: 0x04001BE9 RID: 7145
		public static ArrayList arrCreateTime = new ArrayList();

		// Token: 0x04001BEA RID: 7146
		public static ArrayList arrRefreshTime = new ArrayList();

		// Token: 0x04001BEB RID: 7147
		public static ArrayList arrSNIP = new ArrayList();

		// Token: 0x04001BEC RID: 7148
		public static ArrayList arrSNPort = new ArrayList();

		// Token: 0x04001BED RID: 7149
		public static ArrayList arrSNReceived = new ArrayList();

		// Token: 0x04001BEE RID: 7150
		public static bool bFindFalseACont = true;

		// Token: 0x04001BEF RID: 7151
		public static int bUDPCloud = 0;

		// Token: 0x04001BF0 RID: 7152
		public static int bUDPCloudSpecial = 0;

		// Token: 0x04001BF1 RID: 7153
		public static int bUDPNeedCheckNetRunning = 0;

		// Token: 0x04001BF2 RID: 7154
		public static int bUDPOnly64 = 0;

		// Token: 0x04001BF3 RID: 7155
		private static DateTime dtLast = DateTime.Now;

		// Token: 0x04001BF4 RID: 7156
		public static bool gbAutoLockInterface = false;

		// Token: 0x04001BF5 RID: 7157
		public static bool gbHideCardNO = false;

		// Token: 0x04001BF6 RID: 7158
		public static bool gbInputKeyPasswordControl = false;

		// Token: 0x04001BF7 RID: 7159
		public static string gCustomProductType = "";

		// Token: 0x04001BF8 RID: 7160
		public static int gReaderBrokenWarnActive = 0;

		// Token: 0x04001BF9 RID: 7161
		public static bool IsSqlServer = true;

		// Token: 0x04001BFA RID: 7162
		private static int m_crc19 = 0;

		// Token: 0x04001BFB RID: 7163
		private static string m_DisplayFormat_DateYMD = "yyyy-MM-dd";

		// Token: 0x04001BFC RID: 7164
		private static string m_DisplayFormat_DateYMDHMS = "yyyy-MM-dd HH:mm:ss";

		// Token: 0x04001BFD RID: 7165
		private static string m_DisplayFormat_DateYMDHMSWeek = "yyyy-MM-dd HH:mm:ss dddd";

		// Token: 0x04001BFE RID: 7166
		private static string m_DisplayFormat_DateYMDWeek = "yyyy-MM-dd dddd";

		// Token: 0x04001BFF RID: 7167
		private static DateTime m_dtSoftwareStartTime = DateTime.Now;

		// Token: 0x04001C00 RID: 7168
		private static int m_gPTC = 0;

		// Token: 0x04001C01 RID: 7169
		private static string m_MSGTITLE = "";

		// Token: 0x04001C02 RID: 7170
		private static string m_YMDHMSFormat = "yyyy-MM-dd HH:mm:ss";

		// Token: 0x04001C03 RID: 7171
		private static string m_CommPasswordStr = "";

		// Token: 0x04001C04 RID: 7172
		public static int p64_gprs_refreshCycleMax = 300;

		// Token: 0x04001C05 RID: 7173
		public static int p64_gprs_watchingSendCycle = 30;

		// Token: 0x04001C06 RID: 7174
		public static string strLogIntoFileName = "";

		// Token: 0x04001C07 RID: 7175
		public static string UDPCloudIP = "";

		// Token: 0x04001C08 RID: 7176
		public static string UDPCloudIPSpecial = "";

		// Token: 0x04001C09 RID: 7177
		public static int UDPCloudPort = 61001;

		// Token: 0x04001C0A RID: 7178
		public static int UDPCloudShortPort = 61005;

		// Token: 0x04001C0B RID: 7179
		public static int UDPCloudShortPortSpecial = 61009;

		// Token: 0x04001C0C RID: 7180
		public static wgUdpServer wgcloud;
	}
}
