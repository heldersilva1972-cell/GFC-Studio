namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000371 RID: 881
	public partial class dfrmShiftEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001CDF RID: 7391 RVA: 0x002617E8 File Offset: 0x002607E8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x00261808 File Offset: 0x00260808
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftEdit));
			this.cbof_shift = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cbof_shift, "cbof_shift");
			this.cbof_shift.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_shift.Name = "cbof_shift";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
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
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.cbof_shift);
			base.Controls.Add(this.Label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftEdit";
			base.Load += new global::System.EventHandler(this.dfrmShiftEdit_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x04003780 RID: 14208
		private global::System.ComponentModel.Container components;

		// Token: 0x04003781 RID: 14209
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003782 RID: 14210
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003783 RID: 14211
		internal global::System.Windows.Forms.ComboBox cbof_shift;

		// Token: 0x04003784 RID: 14212
		internal global::System.Windows.Forms.Label Label3;
	}
}
