using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D0 RID: 464
	public class GZip
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x000E455D File Offset: 0x000E355D
		public static GZipResult Compress(string lpSourceFolder, string lpDestFolder, string zipFileName)
		{
			return GZip.Compress(lpSourceFolder, "*.*", SearchOption.AllDirectories, lpDestFolder, zipFileName, true);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000E456E File Offset: 0x000E356E
		public static GZipResult Compress(FileInfo[] files, string lpBaseFolder, string lpDestFolder, string zipFileName)
		{
			return GZip.Compress(files, lpBaseFolder, lpDestFolder, zipFileName, true);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000E457C File Offset: 0x000E357C
		public static GZipResult Compress(FileInfo[] files, string lpBaseFolder, string lpDestFolder, string zipFileName, bool deleteTempFile)
		{
			GZipResult gzipResult = new GZipResult();
			try
			{
				if (!lpDestFolder.EndsWith("\\"))
				{
					lpDestFolder += "\\";
				}
				string text = lpDestFolder + zipFileName + ".tmp";
				string text2 = lpDestFolder + zipFileName;
				gzipResult.TempFile = text;
				gzipResult.ZipFile = text2;
				if (files == null || files.Length <= 0)
				{
					return gzipResult;
				}
				GZip.CreateTempFile(files, lpBaseFolder, text, gzipResult);
				if (gzipResult.FileCount > 0)
				{
					GZip.CreateZipFile(text, text2, gzipResult);
				}
				if (deleteTempFile)
				{
					File.Delete(text);
					gzipResult.TempFileDeleted = true;
				}
			}
			catch
			{
				gzipResult.Errors = true;
			}
			return gzipResult;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000E4624 File Offset: 0x000E3624
		public static GZipResult Compress(string lpSourceFolder, string searchPattern, SearchOption searchOption, string lpDestFolder, string zipFileName, bool deleteTempFile)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(lpSourceFolder);
			return GZip.Compress(directoryInfo.GetFiles("*.*", searchOption), lpSourceFolder, lpDestFolder, zipFileName, deleteTempFile);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000E4650 File Offset: 0x000E3650
		private static void CreateTempFile(FileInfo[] files, string lpBaseFolder, string lpTempFile, GZipResult result)
		{
			int num = 0;
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			if (files != null && files.Length > 0)
			{
				try
				{
					result.Files = new GZipFileInfo[files.Length];
					fileStream = new FileStream(lpTempFile, FileMode.Create, FileAccess.Write, FileShare.None);
					foreach (FileInfo fileInfo in files)
					{
						fileInfo.DirectoryName + "\\";
						try
						{
							GZipFileInfo gzipFileInfo = new GZipFileInfo();
							gzipFileInfo.Index = num;
							string fullName = fileInfo.FullName;
							gzipFileInfo.LocalPath = fullName;
							string text = fullName.Replace(lpBaseFolder, string.Empty).Replace("\\", "/");
							gzipFileInfo.RelativePath = text;
							fileStream2 = new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.Read);
							byte[] array = new byte[fileStream2.Length];
							fileStream2.Read(array, 0, array.Length);
							fileStream2.Close();
							fileStream2 = null;
							string text2 = fileInfo.LastWriteTimeUtc.ToString();
							gzipFileInfo.ModifiedDate = fileInfo.LastWriteTimeUtc;
							gzipFileInfo.Length = array.Length;
							string[] array2 = new string[]
							{
								num.ToString(),
								",",
								text,
								",",
								text2,
								",",
								array.Length.ToString(),
								"\n"
							};
							string text3 = string.Concat(array2);
							byte[] bytes = Encoding.Default.GetBytes(text3);
							fileStream.Write(bytes, 0, bytes.Length);
							fileStream.Write(array, 0, array.Length);
							fileStream.WriteByte(10);
							gzipFileInfo.AddedToTempFile = true;
							result.Files[num] = gzipFileInfo;
							num++;
						}
						catch
						{
							result.Errors = true;
						}
						finally
						{
							if (fileStream2 != null)
							{
								fileStream2.Close();
								fileStream2 = null;
							}
						}
						if (fileStream != null)
						{
							result.TempFileSize = fileStream.Length;
						}
					}
				}
				catch
				{
					result.Errors = true;
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
						fileStream = null;
					}
				}
			}
			result.FileCount = num;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000E48D4 File Offset: 0x000E38D4
		private static void CreateZipFile(string lpSourceFile, string lpZipFile, GZipResult result)
		{
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			GZipStream gzipStream = null;
			try
			{
				fileStream = new FileStream(lpZipFile, FileMode.Create, FileAccess.Write, FileShare.None);
				gzipStream = new GZipStream(fileStream, CompressionMode.Compress, true);
				fileStream2 = new FileStream(lpSourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
				byte[] array = new byte[fileStream2.Length];
				fileStream2.Read(array, 0, array.Length);
				fileStream2.Close();
				fileStream2 = null;
				gzipStream.Write(array, 0, array.Length);
				result.ZipFileSize = fileStream.Length;
				result.CompressionPercent = GZip.GetCompressionPercent(result.TempFileSize, result.ZipFileSize);
			}
			catch
			{
				result.Errors = true;
			}
			finally
			{
				if (gzipStream != null)
				{
					gzipStream.Close();
					gzipStream = null;
				}
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
				if (fileStream2 != null)
				{
					fileStream2.Close();
					fileStream2 = null;
				}
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000E49A0 File Offset: 0x000E39A0
		public static GZipResult Decompress(string lpSourceFolder, string lpDestFolder, string zipFileName)
		{
			return GZip.Decompress(lpSourceFolder, lpDestFolder, zipFileName, true, true, null, null, 4096);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000E49B3 File Offset: 0x000E39B3
		public static GZipResult Decompress(string lpSourceFolder, string lpDestFolder, string zipFileName, bool writeFiles, string addExtension)
		{
			return GZip.Decompress(lpSourceFolder, lpDestFolder, zipFileName, true, writeFiles, addExtension, null, 4096);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000E49C8 File Offset: 0x000E39C8
		public static GZipResult Decompress(string lpSrcFolder, string lpDestFolder, string zipFileName, bool deleteTempFile, bool writeFiles, string addExtension, Hashtable htFiles, int bufferSize)
		{
			GZipResult gzipResult = new GZipResult();
			if (!lpDestFolder.EndsWith("\\"))
			{
				lpDestFolder += "\\";
			}
			if (!lpSrcFolder.EndsWith("\\"))
			{
				lpSrcFolder += "\\";
			}
			string text = lpSrcFolder + zipFileName + ".tmp";
			string text2 = lpSrcFolder + zipFileName;
			gzipResult.TempFile = text;
			gzipResult.ZipFile = text2;
			FileStream fileStream = null;
			ArrayList arrayList = new ArrayList();
			if (string.IsNullOrEmpty(addExtension))
			{
				addExtension = string.Empty;
			}
			else if (!addExtension.StartsWith("."))
			{
				addExtension = "." + addExtension;
			}
			try
			{
				fileStream = GZip.UnzipToTempFile(text2, text, gzipResult);
				if (fileStream != null)
				{
					while (fileStream.Position != fileStream.Length)
					{
						string text3 = null;
						while (string.IsNullOrEmpty(text3) && fileStream.Position != fileStream.Length)
						{
							text3 = GZip.ReadLine(fileStream);
						}
						if (!string.IsNullOrEmpty(text3))
						{
							GZipFileInfo gzipFileInfo = new GZipFileInfo();
							if (gzipFileInfo.ParseFileInfo(text3) && gzipFileInfo.Length > 0)
							{
								arrayList.Add(gzipFileInfo);
								string text4 = lpDestFolder + gzipFileInfo.RelativePath;
								string folder = GZip.GetFolder(text4);
								gzipFileInfo.LocalPath = text4;
								bool flag = false;
								if (htFiles == null || htFiles.ContainsKey(gzipFileInfo.RelativePath))
								{
									gzipFileInfo.RestoreRequested = true;
									flag = writeFiles;
								}
								if (flag)
								{
									if (!Directory.Exists(folder))
									{
										Directory.CreateDirectory(folder);
									}
									gzipFileInfo.Restored = GZip.WriteFile(fileStream, gzipFileInfo.Length, text4 + addExtension, bufferSize);
								}
								else
								{
									fileStream.Position += (long)gzipFileInfo.Length;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				gzipResult.Errors = true;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
			}
			try
			{
				if (deleteTempFile)
				{
					File.Delete(text);
					gzipResult.TempFileDeleted = true;
				}
			}
			catch
			{
				gzipResult.Errors = true;
			}
			gzipResult.FileCount = arrayList.Count;
			gzipResult.Files = new GZipFileInfo[arrayList.Count];
			arrayList.CopyTo(gzipResult.Files);
			return gzipResult;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x000E4C54 File Offset: 0x000E3C54
		private static int GetCompressionPercent(long tempLen, long zipLen)
		{
			double num = (double)tempLen;
			double num2 = (double)zipLen;
			double num3 = 100.0;
			double num4 = (num - num2) / num;
			double num5 = num4 * num3;
			return (int)num5;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x000E4C80 File Offset: 0x000E3C80
		private static string GetFolder(string lpFilePath)
		{
			string text = lpFilePath;
			int num = text.LastIndexOf("\\");
			if (num != -1)
			{
				text = text.Substring(0, num + 1);
			}
			return text;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000E4CAC File Offset: 0x000E3CAC
		private static string ReadLine(FileStream fs)
		{
			byte[] array = new byte[4096];
			byte b = 0;
			byte b2 = 10;
			int num = 0;
			while (b != b2)
			{
				array[num] = (byte)fs.ReadByte();
				num++;
			}
			return Encoding.Default.GetString(array, 0, num - 1);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000E4CF0 File Offset: 0x000E3CF0
		private static FileStream UnzipToTempFile(string lpZipFile, string lpTempFile, GZipResult result)
		{
			FileStream fileStream = null;
			GZipStream gzipStream = null;
			FileStream fileStream2 = null;
			byte[] array = new byte[4096];
			try
			{
				fileStream = new FileStream(lpZipFile, FileMode.Open, FileAccess.Read, FileShare.Read);
				result.ZipFileSize = fileStream.Length;
				fileStream2 = new FileStream(lpTempFile, FileMode.Create, FileAccess.Write, FileShare.None);
				gzipStream = new GZipStream(fileStream, CompressionMode.Decompress, true);
				int num;
				do
				{
					num = gzipStream.Read(array, 0, 4096);
					if (num != 0)
					{
						fileStream2.Write(array, 0, num);
					}
				}
				while (num == 4096);
			}
			catch
			{
				result.Errors = true;
			}
			finally
			{
				if (gzipStream != null)
				{
					gzipStream.Close();
					gzipStream = null;
				}
				if (fileStream2 != null)
				{
					fileStream2.Close();
					fileStream2 = null;
				}
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream = null;
				}
			}
			FileStream fileStream3 = new FileStream(lpTempFile, FileMode.Open, FileAccess.Read, FileShare.None);
			if (fileStream3 != null)
			{
				result.TempFileSize = fileStream3.Length;
			}
			return fileStream3;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000E4DCC File Offset: 0x000E3DCC
		private static bool WriteFile(FileStream fs, int fileLength, string lpFile, int bufferSize)
		{
			bool flag = false;
			FileStream fileStream = null;
			if (bufferSize == 0 || fileLength < bufferSize)
			{
				bufferSize = fileLength;
			}
			int i = fileLength;
			try
			{
				byte[] array = new byte[bufferSize];
				FileInfo fileInfo = new FileInfo(lpFile);
				try
				{
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
				fileInfo.Refresh();
				if (fileInfo.Exists)
				{
					while (i > 0)
					{
						int num;
						if (i > bufferSize)
						{
							num = bufferSize;
						}
						else
						{
							num = i;
						}
						int num2 = fs.Read(array, 0, num);
						i -= num2;
						if (num2 == 0)
						{
							break;
						}
					}
					return false;
				}
				fileStream = new FileStream(lpFile, FileMode.Create, FileAccess.Write, FileShare.None);
				while (i > 0)
				{
					int num;
					if (i > bufferSize)
					{
						num = bufferSize;
					}
					else
					{
						num = i;
					}
					int num2 = fs.Read(array, 0, num);
					i -= num2;
					if (num2 == 0)
					{
						break;
					}
					fileStream.Write(array, 0, num2);
					fileStream.Flush();
				}
				fileStream.Flush();
				fileStream.Close();
				fileStream = null;
				flag = true;
			}
			catch
			{
				flag = false;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Flush();
					fileStream.Close();
					fileStream = null;
				}
			}
			return flag;
		}
	}
}
