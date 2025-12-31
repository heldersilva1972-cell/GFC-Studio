using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostAA
{
	// Token: 0x02000332 RID: 818
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	public class getCusttypeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060019D9 RID: 6617 RVA: 0x0021E243 File Offset: 0x0021D243
		internal getCusttypeCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x0021E256 File Offset: 0x0021D256
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x040034B9 RID: 13497
		private object[] results;
	}
}
