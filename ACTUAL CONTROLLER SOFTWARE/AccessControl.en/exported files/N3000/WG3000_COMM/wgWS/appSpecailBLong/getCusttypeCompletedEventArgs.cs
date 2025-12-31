using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.appSpecailBLong
{
	// Token: 0x02000396 RID: 918
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	public class getCusttypeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600216B RID: 8555 RVA: 0x0027E813 File Offset: 0x0027D813
		internal getCusttypeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x0027E826 File Offset: 0x0027D826
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A25 RID: 14885
		private object[] results;
	}
}
