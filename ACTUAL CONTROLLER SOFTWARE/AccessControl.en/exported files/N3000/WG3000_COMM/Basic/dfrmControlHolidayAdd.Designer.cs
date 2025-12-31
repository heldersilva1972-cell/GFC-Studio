namespace WG3000_COMM.Basic
{
	// Token: 0x02000008 RID: 8
	public partial class dfrmControlHolidayAdd : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600006F RID: 111 RVA: 0x0000F0FC File Offset: 0x0000E0FC
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000F108 File Offset: 0x0000E108
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmControlHolidayAdd));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.dtpStartDate = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.dtpEndDate = new global::System.Windows.Forms.DateTimePicker();
			this.txtf_Notes = new global::System.Windows.Forms.TextBox();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.txtHolidayName = new global::System.Windows.Forms.TextBox();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.Label4, "Label4");
			this.Label4.BackColor = global::System.Drawing.Color.Transparent;
			this.Label4.ForeColor = global::System.Drawing.Color.White;
			this.Label4.Name = "Label4";
			componentResourceManager.ApplyResources(this.dtpStartDate, "dtpStartDate");
			this.dtpStartDate.Name = "dtpStartDate";
			this.dtpStartDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			this.dtpStartDate.ValueChanged += new global::System.EventHandler(this.dtpStartDate_ValueChanged);
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
			componentResourceManager.ApplyResources(this.txtf_Notes, "txtf_Notes");
			this.txtf_Notes.Name = "txtf_Notes";
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.BackColor = global::System.Drawing.Color.Transparent;
			this.Label7.ForeColor = global::System.Drawing.Color.White;
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this.txtHolidayName, "txtHolidayName");
			this.txtHolidayName.Name = "txtHolidayName";
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dateEndHMS1);
			base.Controls.Add(this.dateBeginHMS1);
			base.Controls.Add(this.txtHolidayName);
			base.Controls.Add(this.txtf_Notes);
			base.Controls.Add(this.dtpStartDate);
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.Label6);
			base.Controls.Add(this.dtpEndDate);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.Label4);
			base.Controls.Add(this.Label7);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmControlHolidayAdd";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.Load += new global::System.EventHandler(this.dfrmLeave_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000A4 RID: 164
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040000A5 RID: 165
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x040000A6 RID: 166
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040000A7 RID: 167
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040000A8 RID: 168
		internal global::System.Windows.Forms.DateTimePicker dtpEndDate;

		// Token: 0x040000A9 RID: 169
		internal global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x040000AA RID: 170
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x040000AB RID: 171
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x040000AC RID: 172
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x040000AD RID: 173
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x040000AE RID: 174
		internal global::System.Windows.Forms.TextBox txtf_Notes;

		// Token: 0x040000AF RID: 175
		internal global::System.Windows.Forms.TextBox txtHolidayName;
	}
}
