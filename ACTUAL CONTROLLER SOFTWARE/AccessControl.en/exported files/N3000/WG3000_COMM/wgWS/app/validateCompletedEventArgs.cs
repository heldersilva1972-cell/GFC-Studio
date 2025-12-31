using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003A8 RID: 936
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DebuggerStepThrough]
	public class validateCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060021C7 RID: 8647 RVA: 0x0027F100 File Offset: 0x0027E100
		internal validateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x0027F113 File Offset: 0x0027E113
		public int Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (int)this.results[0];
			}
		}

		// Token: 0x04003A39 RID: 14905
		private object[] results;
	}
}
