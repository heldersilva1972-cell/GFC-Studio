using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x0200033F RID: 831
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class validateCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x0021E7AC File Offset: 0x0021D7AC
		internal validateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0021E7BF File Offset: 0x0021D7BF
		public int Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (int)this.results[0];
			}
		}

		// Token: 0x040034C6 RID: 13510
		private object[] results;
	}
}
