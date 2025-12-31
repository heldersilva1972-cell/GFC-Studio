using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003A6 RID: 934
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060021C1 RID: 8641 RVA: 0x0027F0D8 File Offset: 0x0027E0D8
		internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x0027F0EB File Offset: 0x0027E0EB
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A38 RID: 14904
		private object[] results;
	}
}
