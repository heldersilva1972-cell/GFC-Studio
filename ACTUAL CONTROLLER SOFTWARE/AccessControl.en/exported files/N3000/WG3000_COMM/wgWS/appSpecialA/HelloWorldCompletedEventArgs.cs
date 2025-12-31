using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.appSpecialA
{
	// Token: 0x0200039F RID: 927
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600219C RID: 8604 RVA: 0x0027ED2C File Offset: 0x0027DD2C
		internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600219D RID: 8605 RVA: 0x0027ED3F File Offset: 0x0027DD3F
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A30 RID: 14896
		private object[] results;
	}
}
