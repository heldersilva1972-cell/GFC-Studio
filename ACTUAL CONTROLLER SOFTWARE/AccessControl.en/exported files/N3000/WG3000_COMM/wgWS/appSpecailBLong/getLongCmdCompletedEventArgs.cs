using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.wgWS.appSpecailBLong
{
	// Token: 0x02000398 RID: 920
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class getLongCmdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06002171 RID: 8561 RVA: 0x0027E83B File Offset: 0x0027D83B
		internal getLongCmdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x0027E84E File Offset: 0x0027D84E
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04003A26 RID: 14886
		private object[] results;
	}
}
