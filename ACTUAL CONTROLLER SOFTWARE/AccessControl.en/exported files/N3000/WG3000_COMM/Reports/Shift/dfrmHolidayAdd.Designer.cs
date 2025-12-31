namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000368 RID: 872
	public partial class dfrmHolidayAdd : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C65 RID: 7269 RVA: 0x0025660C File Offset: 0x0025560C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00256618 File Offset: 0x00255618
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmHolidayAdd));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.dtpStartDate = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.dtpEndDate = new global::System.Windows.Forms.DateTimePicker();
			this.cboStart = new global::System.Windows.Forms.ComboBox();
			this.cboEnd = new global::System.Windows.Forms.ComboBox();
			this.txtf_Notes = new global::System.Windows.Forms.TextBox();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.txtHolidayName = new global::System.Windows.Forms.TextBox();
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
			componentResourceManager.ApplyResources(this.cboStart, "cboStart");
			this.cboStart.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStart.Name = "cboStart";
			componentResourceManager.ApplyResources(this.cboEnd, "cboEnd");
			this.cboEnd.DisplayMember = "f_GroupName";
			this.cboEnd.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEnd.Name = "cboEnd";
			this.cboEnd.ValueMember = "f_GroupID";
			componentResourceManager.ApplyResources(this.txtf_Notes, "txtf_Notes");
			this.txtf_Notes.Name = "txtf_Notes";
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.BackColor = global::System.Drawing.Color.Transparent;
			this.Label7.ForeColor = global::System.Drawing.Color.White;
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this.txtHolidayName, "txtHolidayName");
			this.txtHolidayName.Name = "txtHolidayName";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtHolidayName);
			base.Controls.Add(this.txtf_Notes);
			base.Controls.Add(this.dtpStartDate);
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.Label6);
			base.Controls.Add(this.dtpEndDate);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.Label4);
			base.Controls.Add(this.cboStart);
			base.Controls.Add(this.cboEnd);
			base.Controls.Add(this.Label7);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmHolidayAdd";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.Load += new global::System.EventHandler(this.dfrmLeave_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400369F RID: 13983
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040036A0 RID: 13984
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040036A1 RID: 13985
		internal global::System.Windows.Forms.ComboBox cboEnd;

		// Token: 0x040036A2 RID: 13986
		internal global::System.Windows.Forms.ComboBox cboStart;

		// Token: 0x040036A3 RID: 13987
		internal global::System.Windows.Forms.DateTimePicker dtpEndDate;

		// Token: 0x040036A4 RID: 13988
		internal global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x040036A5 RID: 13989
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x040036A6 RID: 13990
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x040036A7 RID: 13991
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x040036A8 RID: 13992
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x040036A9 RID: 13993
		internal global::System.Windows.Forms.TextBox txtf_Notes;

		// Token: 0x040036AA RID: 13994
		internal global::System.Windows.Forms.TextBox txtHolidayName;
	}
}
