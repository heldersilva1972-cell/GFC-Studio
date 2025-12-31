using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000017 RID: 23
	public partial class dfrmInputNewName : frmN3000
	{
		// Token: 0x06000119 RID: 281 RVA: 0x0002832C File Offset: 0x0002732C
		public dfrmInputNewName()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0002834C File Offset: 0x0002734C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0002835C File Offset: 0x0002735C
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.bNotAllowNull && string.IsNullOrEmpty(this.txtNewName.Text))
			{
				this.label2.Visible = true;
				return;
			}
			if (string.IsNullOrEmpty(this.txtNewName.Text))
			{
				this.strNewName = "";
			}
			else
			{
				this.strNewName = this.txtNewName.Text.Trim();
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000283D2 File Offset: 0x000273D2
		private void dfrmInputNewName_Load(object sender, EventArgs e)
		{
			this.txtNewName.Text = this.strNewName;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000283E5 File Offset: 0x000273E5
		public void setPasswordChar(char val)
		{
			this.txtNewName.PasswordChar = val;
		}

		// Token: 0x040002D5 RID: 725
		public bool bNotAllowNull = true;

		// Token: 0x040002D6 RID: 726
		public string strNewName = "";
	}
}
