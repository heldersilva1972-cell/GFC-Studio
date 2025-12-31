using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.appSpecailBLong
{
	// Token: 0x0200039A RID: 922
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06002177 RID: 8567 RVA: 0x0027E863 File Offset: 0x0027D863
		internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x0027E876 File Offset: 0x0027D876
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A27 RID: 14887
		private object[] results;
	}
}
