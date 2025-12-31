using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x0200033D RID: 829
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x0021E784 File Offset: 0x0021D784
		internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0021E797 File Offset: 0x0021D797
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034C5 RID: 13509
		private object[] results;
	}
}
