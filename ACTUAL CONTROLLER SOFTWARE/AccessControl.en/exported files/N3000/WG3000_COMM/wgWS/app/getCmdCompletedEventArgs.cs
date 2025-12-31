using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003A2 RID: 930
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class getCmdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060021B5 RID: 8629 RVA: 0x0027F088 File Offset: 0x0027E088
		internal getCmdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x0027F09B File Offset: 0x0027E09B
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A36 RID: 14902
		private object[] results;
	}
}
