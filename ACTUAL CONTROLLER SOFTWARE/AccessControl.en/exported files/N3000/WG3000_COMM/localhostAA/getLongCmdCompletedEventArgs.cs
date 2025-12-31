using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostAA
{
	// Token: 0x02000334 RID: 820
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class getLongCmdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060019DF RID: 6623 RVA: 0x0021E26B File Offset: 0x0021D26B
		internal getLongCmdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060019E0 RID: 6624 RVA: 0x0021E27E File Offset: 0x0021D27E
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034BA RID: 13498
		private object[] results;
	}
}
