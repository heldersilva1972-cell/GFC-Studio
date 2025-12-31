using System;
using System.Windows.Forms;

namespace WG3000_COMM.Core
{
	// Token: 0x0200021C RID: 540
	public class XTextBox : TextBox
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x0010F27C File Offset: 0x0010E27C
		public int LinesCount()
		{
			Message message = Message.Create(base.Handle, 186, IntPtr.Zero, IntPtr.Zero);
			base.DefWndProc(ref message);
			return message.Result.ToInt32();
		}
	}
}
