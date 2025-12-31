namespace WG3000_COMM.Core
{
	// Token: 0x020001C7 RID: 455
	public partial class dfrmSelectColumnsShow : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x000DF531 File Offset: 0x000DE531
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000DF550 File Offset: 0x000DE550
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Core.dfrmSelectColumnsShow));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.chkListColumns = new global::System.Windows.Forms.CheckedListBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.chkListColumns, "chkListColumns");
			this.chkListColumns.CheckOnClick = true;
			this.chkListColumns.FormattingEnabled = true;
			this.chkListColumns.Name = "chkListColumns";
			this.chkListColumns.ThreeDCheckBoxes = true;
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.chkListColumns);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "dfrmSelectColumnsShow";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001871 RID: 6257
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001872 RID: 6258
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001873 RID: 6259
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001874 RID: 6260
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001875 RID: 6261
		public global::System.Windows.Forms.CheckedListBox chkListColumns;
	}
}
