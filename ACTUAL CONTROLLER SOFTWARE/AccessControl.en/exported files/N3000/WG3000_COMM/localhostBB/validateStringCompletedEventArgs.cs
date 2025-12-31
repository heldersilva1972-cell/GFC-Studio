using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x02000341 RID: 833
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	public class validateStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A1C RID: 6684 RVA: 0x0021E7D4 File Offset: 0x0021D7D4
		internal validateStringCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x0021E7E7 File Offset: 0x0021D7E7
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034C7 RID: 13511
		private object[] results;
	}
}
