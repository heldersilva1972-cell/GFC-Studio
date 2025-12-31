using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EB RID: 747
	public partial class dfrmFingerSupercard : frmN3000
	{
		// Token: 0x06001565 RID: 5477 RVA: 0x001A89BC File Offset: 0x001A79BC
		public dfrmFingerSupercard()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x001A89CA File Offset: 0x001A79CA
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x001A89D9 File Offset: 0x001A79D9
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x001A89E8 File Offset: 0x001A79E8
		private void dfrmFingerSupercard_Load(object sender, EventArgs e)
		{
			string systemParamNotes = wgAppConfig.getSystemParamNotes(189);
			try
			{
				if (!string.IsNullOrEmpty(systemParamNotes))
				{
					string[] array = systemParamNotes.Split(new char[] { ',' });
					if (array.Length == 1)
					{
						this.txtSuperCard1.Text = array[0];
					}
					if (array.Length == 2)
					{
						this.txtSuperCard1.Text = array[0];
						this.txtSuperCard2.Text = array[1];
					}
				}
			}
			catch
			{
			}
		}
	}
}
