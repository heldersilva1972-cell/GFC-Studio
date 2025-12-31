using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003A4 RID: 932
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class getPassInfoWithRSACompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060021BB RID: 8635 RVA: 0x0027F0B0 File Offset: 0x0027E0B0
		internal getPassInfoWithRSACompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x0027F0C3 File Offset: 0x0027E0C3
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A37 RID: 14903
		private object[] results;
	}
}
