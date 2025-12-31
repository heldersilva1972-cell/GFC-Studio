using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostAA
{
	// Token: 0x02000336 RID: 822
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060019E5 RID: 6629 RVA: 0x0021E293 File Offset: 0x0021D293
		internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0021E2A6 File Offset: 0x0021D2A6
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034BB RID: 13499
		private object[] results;
	}
}
