using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WG3000_COMM
{
	// Token: 0x02000331 RID: 817
	public partial class frmTestController : Form
	{
		// Token: 0x060019D4 RID: 6612 RVA: 0x0021E198 File Offset: 0x0021D198
		public frmTestController()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0021E1A6 File Offset: 0x0021D1A6
		private void frmTestController_Load(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0021E1AE File Offset: 0x0021D1AE
		public void onlyProduce()
		{
		}
	}
}
