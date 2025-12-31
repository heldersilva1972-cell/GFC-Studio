using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003AA RID: 938
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	public class validateStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060021CD RID: 8653 RVA: 0x0027F128 File Offset: 0x0027E128
		internal validateStringCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x0027F13B File Offset: 0x0027E13B
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A3A RID: 14906
		private object[] results;
	}
}
