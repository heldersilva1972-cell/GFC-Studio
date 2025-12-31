using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WG3000_COMM.Properties
{
	// Token: 0x0200035B RID: 859
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x00225158 File Offset: 0x00224158
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x0022515F File Offset: 0x0022415F
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		[DefaultSettingValue("https://app.znsmart.com/wgConInfo/wsWGController.asmx")]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string N3000_cn_com_wiegand_app_wgControllerInfo
		{
			get
			{
				return (string)this["N3000_cn_com_wiegand_app_wgControllerInfo"];
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x00225171 File Offset: 0x00224171
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		[DebuggerNonUserCode]
		[ApplicationScopedSetting]
		[DefaultSettingValue("https://app.znsmart.com/wgShortPacketConf/wsWGShortPacketConf.asmx")]
		public string N3000_cn_com_wiegand_app1_wgShortPacketConf
		{
			get
			{
				return (string)this["N3000_cn_com_wiegand_app1_wgShortPacketConf"];
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x00225183 File Offset: 0x00224183
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		[ApplicationScopedSetting]
		[DefaultSettingValue("http://localhost:21114/wsWGLongPacketConf.asmx")]
		[DebuggerNonUserCode]
		public string N3000_localhostAA_wsWGLongPacketConf
		{
			get
			{
				return (string)this["N3000_localhostAA_wsWGLongPacketConf"];
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001B93 RID: 7059 RVA: 0x00225195 File Offset: 0x00224195
		[DefaultSettingValue("http://localhost:21114/wsWGController.asmx")]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		public string N3000_localhostBB_wgControllerInfo
		{
			get
			{
				return (string)this["N3000_localhostBB_wgControllerInfo"];
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x002251A7 File Offset: 0x002241A7
		[DebuggerNonUserCode]
		[ApplicationScopedSetting]
		[DefaultSettingValue("https://app.znsmart.com/wgShortPacketConf/wsWGLongPacketConf.asmx")]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		public string N3000_wgWS_appSpecailBLong_wsWGLongPacketConf
		{
			get
			{
				return (string)this["N3000_wgWS_appSpecailBLong_wsWGLongPacketConf"];
			}
		}

		// Token: 0x04003560 RID: 13664
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
