using System;
using Microsoft.Win32;

namespace WindowsStartup.Utils
{
	// Token: 0x020003AD RID: 941
	public class RegisterTool
	{
		// Token: 0x060021F8 RID: 8696 RVA: 0x0027F818 File Offset: 0x0027E818
		private static RegistryKey Create(string key)
		{
			string text;
			string text2;
			string text3;
			if (RegisterTool.ExtractInfo(key, out text, out text2) && (text3 = text) != null)
			{
				if (text3 == "HKEY_CLASSES_ROOT")
				{
					return Registry.ClassesRoot.CreateSubKey(text2);
				}
				if (text3 == "HKEY_CURRENT_USER")
				{
					return Registry.CurrentUser.CreateSubKey(text2);
				}
				if (text3 == "HKEY_LOCAL_MACHINE")
				{
					return Registry.LocalMachine.CreateSubKey(text2);
				}
				if (text3 == "HKEY_USERS")
				{
					return Registry.Users.CreateSubKey(text2);
				}
				if (text3 == "HKEY_CURRENT_CONFIG")
				{
					return Registry.CurrentConfig.CreateSubKey(text2);
				}
			}
			return Registry.CurrentUser.CreateSubKey(text2);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x0027F8C4 File Offset: 0x0027E8C4
		public static bool DeleteValue(string key, string name)
		{
			bool flag;
			try
			{
				using (RegistryKey registryKey = RegisterTool.Open(key, true))
				{
					if (registryKey != null)
					{
						registryKey.DeleteValue(name);
					}
				}
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x0027F918 File Offset: 0x0027E918
		private static bool ExtractInfo(string key, out string reg, out string sub)
		{
			reg = "";
			sub = "";
			int num = key.IndexOf('\\');
			if (num > 0)
			{
				reg = key.Substring(0, num);
				sub = key.Substring(num + 1);
				return true;
			}
			return false;
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0027F95C File Offset: 0x0027E95C
		public static string GetValue(string key, string name)
		{
			try
			{
				using (RegistryKey registryKey = RegisterTool.Open(key, false))
				{
					if (registryKey != null)
					{
						return (registryKey.GetValue(name) != null) ? registryKey.GetValue(name).ToString() : "";
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0027F9C4 File Offset: 0x0027E9C4
		private static RegistryKey Open(string key, bool writable)
		{
			string text;
			string text2;
			string text3;
			if (RegisterTool.ExtractInfo(key, out text, out text2) && (text3 = text) != null)
			{
				if (text3 == "HKEY_CLASSES_ROOT")
				{
					return Registry.ClassesRoot.OpenSubKey(text2, writable);
				}
				if (text3 == "HKEY_CURRENT_USER")
				{
					return Registry.CurrentUser.OpenSubKey(text2, writable);
				}
				if (text3 == "HKEY_LOCAL_MACHINE")
				{
					return Registry.LocalMachine.OpenSubKey(text2, writable);
				}
				if (text3 == "HKEY_USERS")
				{
					return Registry.Users.OpenSubKey(text2, writable);
				}
				if (text3 == "HKEY_CURRENT_CONFIG")
				{
					return Registry.CurrentConfig.OpenSubKey(text2, writable);
				}
			}
			return Registry.CurrentUser.OpenSubKey(text2, writable);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0027FA78 File Offset: 0x0027EA78
		public static bool SetValue(string key, string name, string value)
		{
			bool flag;
			try
			{
				using (RegistryKey registryKey = RegisterTool.Create(key))
				{
					registryKey.SetValue(name, value);
				}
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}
