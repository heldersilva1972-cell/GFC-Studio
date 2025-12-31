using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.appSpecialA
{
	// Token: 0x0200039D RID: 925
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	public class getCmdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06002196 RID: 8598 RVA: 0x0027ED04 File Offset: 0x0027DD04
		internal getCmdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x0027ED17 File Offset: 0x0027DD17
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A2F RID: 14895
		private object[] results;
	}
}
