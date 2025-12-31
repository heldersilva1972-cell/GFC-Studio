namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036C RID: 876
	public partial class dfrmLeaveEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C99 RID: 7321 RVA: 0x0025C01C File Offset: 0x0025B01C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x0025C03C File Offset: 0x0025B03C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmLeaveEdit));
			this.txtf_ConsumerName = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.txtf_Notes = new global::System.Windows.Forms.TextBox();
			this.dtpStartDate = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.dtpEndDate = new global::System.Windows.Forms.DateTimePicker();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cboHolidayType = new global::System.Windows.Forms.ComboBox();
			this.cboStart = new global::System.Windows.Forms.ComboBox();
			this.cboEnd = new global::System.Windows.Forms.ComboBox();
			this.Label7 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
			this.txtf_ConsumerName.Name = "txtf_ConsumerName";
			this.txtf_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.txtf_Notes, "txtf_Notes");
			this.txtf_Notes.Name = "txtf_Notes";
			componentResourceManager.ApplyResources(this.dtpStartDate, "dtpStartDate");
			this.dtpStartDate.Name = "dtpStartDate";
			this.dtpStartDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.BackColor = global::System.Drawing.Color.Transparent;
			this.Label5.ForeColor = global::System.Drawing.Color.White;
			this.Label5.Name = "Label5";
			componentResourceManager.ApplyResources(this.Label6, "Label6");
			this.Label6.BackColor = global::System.Drawing.Color.Transparent;
			this.Label6.ForeColor = global::System.Drawing.Color.White;
			this.Label6.Name = "Label6";
			componentResourceManager.ApplyResources(this.dtpEndDate, "dtpEndDate");
			this.dtpEndDate.Name = "dtpEndDate";
			this.dtpEndDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cboHolidayType, "cboHolidayType");
			this.cboHolidayType.DisplayMember = "f_GroupName";
			this.cboHolidayType.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboHolidayType.Name = "cboHolidayType";
			this.cboHolidayType.ValueMember = "f_GroupID";
			componentResourceManager.ApplyResources(this.cboStart, "cboStart");
			this.cboStart.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStart.Name = "cboStart";
			componentResourceManager.ApplyResources(this.cboEnd, "cboEnd");
			this.cboEnd.DisplayMember = "f_GroupName";
			this.cboEnd.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEnd.Name = "cboEnd";
			this.cboEnd.ValueMember = "f_GroupID";
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.BackColor = global::System.Drawing.Color.Transparent;
			this.Label7.ForeColor = global::System.Drawing.Color.White;
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtf_ConsumerName);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.txtf_Notes);
			base.Controls.Add(this.dtpStartDate);
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.Label6);
			base.Controls.Add(this.dtpEndDate);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cboHolidayType);
			base.Controls.Add(this.cboStart);
			base.Controls.Add(this.cboEnd);
			base.Controls.Add(this.Label7);
			base.Name = "dfrmLeaveEdit";
			base.Load += new global::System.EventHandler(this.dfrmLeaveEdit_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400370E RID: 14094
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400370F RID: 14095
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003710 RID: 14096
		internal global::System.Windows.Forms.Button btnClose;

		// Token: 0x04003711 RID: 14097
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003712 RID: 14098
		public global::System.Windows.Forms.ComboBox cboEnd;

		// Token: 0x04003713 RID: 14099
		public global::System.Windows.Forms.ComboBox cboHolidayType;

		// Token: 0x04003714 RID: 14100
		public global::System.Windows.Forms.ComboBox cboStart;

		// Token: 0x04003715 RID: 14101
		public global::System.Windows.Forms.DateTimePicker dtpEndDate;

		// Token: 0x04003716 RID: 14102
		public global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x04003717 RID: 14103
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x04003718 RID: 14104
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x04003719 RID: 14105
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x0400371A RID: 14106
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x0400371B RID: 14107
		public global::System.Windows.Forms.TextBox txtf_ConsumerName;

		// Token: 0x0400371C RID: 14108
		public global::System.Windows.Forms.TextBox txtf_Notes;
	}
}
