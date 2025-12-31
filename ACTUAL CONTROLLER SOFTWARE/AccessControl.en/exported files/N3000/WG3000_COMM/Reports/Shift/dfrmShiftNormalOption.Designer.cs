namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000372 RID: 882
	public partial class dfrmShiftNormalOption : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001CE8 RID: 7400 RVA: 0x00261E56 File Offset: 0x00260E56
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x00261E78 File Offset: 0x00260E78
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftNormalOption));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.chkInvalidSwipe = new global::System.Windows.Forms.CheckBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.cboLeaveAbsenceTimeout = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.chkOnlyTwoTimes = new global::System.Windows.Forms.CheckBox();
			this.chkOnlyOnDuty = new global::System.Windows.Forms.CheckBox();
			this.dtpOffduty0 = new global::System.Windows.Forms.DateTimePicker();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.chkEarliest = new global::System.Windows.Forms.CheckBox();
			this.chkMoreCardShift = new global::System.Windows.Forms.CheckBox();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.chkFullAttendanceSpecialA = new global::System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.chkInvalidSwipe, "chkInvalidSwipe");
			this.chkInvalidSwipe.ForeColor = global::System.Drawing.Color.White;
			this.chkInvalidSwipe.Name = "chkInvalidSwipe";
			this.toolTip1.SetToolTip(this.chkInvalidSwipe, componentResourceManager.GetString("chkInvalidSwipe.ToolTip"));
			this.chkInvalidSwipe.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.cboLeaveAbsenceTimeout);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chkOnlyTwoTimes);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.cboLeaveAbsenceTimeout, "cboLeaveAbsenceTimeout");
			this.cboLeaveAbsenceTimeout.DisplayMember = "8.0";
			this.cboLeaveAbsenceTimeout.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLeaveAbsenceTimeout.FormattingEnabled = true;
			this.cboLeaveAbsenceTimeout.Name = "cboLeaveAbsenceTimeout";
			this.toolTip1.SetToolTip(this.cboLeaveAbsenceTimeout, componentResourceManager.GetString("cboLeaveAbsenceTimeout.ToolTip"));
			this.cboLeaveAbsenceTimeout.ValueMember = "8.0";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.chkOnlyTwoTimes, "chkOnlyTwoTimes");
			this.chkOnlyTwoTimes.ForeColor = global::System.Drawing.Color.White;
			this.chkOnlyTwoTimes.Name = "chkOnlyTwoTimes";
			this.toolTip1.SetToolTip(this.chkOnlyTwoTimes, componentResourceManager.GetString("chkOnlyTwoTimes.ToolTip"));
			this.chkOnlyTwoTimes.UseVisualStyleBackColor = true;
			this.chkOnlyTwoTimes.CheckedChanged += new global::System.EventHandler(this.chkOnlyTwoTimes_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkOnlyOnDuty, "chkOnlyOnDuty");
			this.chkOnlyOnDuty.ForeColor = global::System.Drawing.Color.White;
			this.chkOnlyOnDuty.Name = "chkOnlyOnDuty";
			this.toolTip1.SetToolTip(this.chkOnlyOnDuty, componentResourceManager.GetString("chkOnlyOnDuty.ToolTip"));
			this.chkOnlyOnDuty.UseVisualStyleBackColor = true;
			this.chkOnlyOnDuty.CheckedChanged += new global::System.EventHandler(this.chkOnlyOnDuty_CheckedChanged);
			componentResourceManager.ApplyResources(this.dtpOffduty0, "dtpOffduty0");
			this.dtpOffduty0.Name = "dtpOffduty0";
			this.dtpOffduty0.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dtpOffduty0, componentResourceManager.GetString("dtpOffduty0.ToolTip"));
			this.dtpOffduty0.Value = new global::System.DateTime(2004, 7, 18, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.ForeColor = global::System.Drawing.Color.White;
			this.Label7.Name = "Label7";
			this.toolTip1.SetToolTip(this.Label7, componentResourceManager.GetString("Label7.ToolTip"));
			componentResourceManager.ApplyResources(this.chkEarliest, "chkEarliest");
			this.chkEarliest.ForeColor = global::System.Drawing.Color.White;
			this.chkEarliest.Name = "chkEarliest";
			this.toolTip1.SetToolTip(this.chkEarliest, componentResourceManager.GetString("chkEarliest.ToolTip"));
			this.chkEarliest.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkMoreCardShift, "chkMoreCardShift");
			this.chkMoreCardShift.ForeColor = global::System.Drawing.Color.White;
			this.chkMoreCardShift.Name = "chkMoreCardShift";
			this.toolTip1.SetToolTip(this.chkMoreCardShift, componentResourceManager.GetString("chkMoreCardShift.ToolTip"));
			this.chkMoreCardShift.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkFullAttendanceSpecialA, "chkFullAttendanceSpecialA");
			this.chkFullAttendanceSpecialA.ForeColor = global::System.Drawing.Color.White;
			this.chkFullAttendanceSpecialA.Name = "chkFullAttendanceSpecialA";
			this.toolTip1.SetToolTip(this.chkFullAttendanceSpecialA, componentResourceManager.GetString("chkFullAttendanceSpecialA.ToolTip"));
			this.chkFullAttendanceSpecialA.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.chkFullAttendanceSpecialA);
			base.Controls.Add(this.chkMoreCardShift);
			base.Controls.Add(this.chkOnlyOnDuty);
			base.Controls.Add(this.chkInvalidSwipe);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.chkEarliest);
			base.Controls.Add(this.dtpOffduty0);
			base.Controls.Add(this.Label7);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftNormalOption";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Load += new global::System.EventHandler(this.dfrmShiftNormalOption_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003785 RID: 14213
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003786 RID: 14214
		private global::System.Windows.Forms.ComboBox cboLeaveAbsenceTimeout;

		// Token: 0x04003787 RID: 14215
		private global::System.Windows.Forms.CheckBox chkEarliest;

		// Token: 0x04003788 RID: 14216
		private global::System.Windows.Forms.CheckBox chkFullAttendanceSpecialA;

		// Token: 0x04003789 RID: 14217
		private global::System.Windows.Forms.CheckBox chkInvalidSwipe;

		// Token: 0x0400378A RID: 14218
		private global::System.Windows.Forms.CheckBox chkMoreCardShift;

		// Token: 0x0400378B RID: 14219
		private global::System.Windows.Forms.CheckBox chkOnlyOnDuty;

		// Token: 0x0400378C RID: 14220
		private global::System.Windows.Forms.CheckBox chkOnlyTwoTimes;

		// Token: 0x0400378D RID: 14221
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400378E RID: 14222
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400378F RID: 14223
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003790 RID: 14224
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003791 RID: 14225
		internal global::System.Windows.Forms.DateTimePicker dtpOffduty0;

		// Token: 0x04003792 RID: 14226
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x04003793 RID: 14227
		internal global::System.Windows.Forms.Label Label7;
	}
}
