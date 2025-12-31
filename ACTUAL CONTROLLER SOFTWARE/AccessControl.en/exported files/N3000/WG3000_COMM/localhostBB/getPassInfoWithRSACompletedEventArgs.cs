using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x0200033B RID: 827
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class getPassInfoWithRSACompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A0A RID: 6666 RVA: 0x0021E75C File Offset: 0x0021D75C
		internal getPassInfoWithRSACompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0021E76F File Offset: 0x0021D76F
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034C4 RID: 13508
		private object[] results;
	}
}
