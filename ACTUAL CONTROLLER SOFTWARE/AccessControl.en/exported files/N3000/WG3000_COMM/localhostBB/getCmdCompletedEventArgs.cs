using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x02000339 RID: 825
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class getCmdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x0021E734 File Offset: 0x0021D734
		internal getCmdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0021E747 File Offset: 0x0021D747
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034C3 RID: 13507
		private object[] results;
	}
}
