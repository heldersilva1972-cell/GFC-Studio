namespace WG3000_COMM.Basic
{
	// Token: 0x0200000D RID: 13
	public partial class dfrmControlSeg : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x0001B23C File Offset: 0x0001A23C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0001B25C File Offset: 0x0001A25C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmControlSeg));
			this.chkNotAllowInHolidays = new global::System.Windows.Forms.CheckBox();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.txtf_ControlSegName = new global::System.Windows.Forms.TextBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.nudf_LimitedTimesOfMonth = new global::System.Windows.Forms.NumericUpDown();
			this.optReaderCount = new global::System.Windows.Forms.RadioButton();
			this.optControllerCount = new global::System.Windows.Forms.RadioButton();
			this.nudf_LimitedTimesOfDay = new global::System.Windows.Forms.NumericUpDown();
			this.nudf_LimitedTimesOfHMS3 = new global::System.Windows.Forms.NumericUpDown();
			this.label93 = new global::System.Windows.Forms.Label();
			this.nudf_LimitedTimesOfHMS2 = new global::System.Windows.Forms.NumericUpDown();
			this.label92 = new global::System.Windows.Forms.Label();
			this.nudf_LimitedTimesOfHMS1 = new global::System.Windows.Forms.NumericUpDown();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label94 = new global::System.Windows.Forms.Label();
			this.label91 = new global::System.Windows.Forms.Label();
			this.chkf_ReaderCount = new global::System.Windows.Forms.CheckBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label84 = new global::System.Windows.Forms.Label();
			this.cbof_ControlSegIDLinked = new global::System.Windows.Forms.ComboBox();
			this.groupBox11 = new global::System.Windows.Forms.GroupBox();
			this.label89 = new global::System.Windows.Forms.Label();
			this.label90 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.label87 = new global::System.Windows.Forms.Label();
			this.label88 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.label86 = new global::System.Windows.Forms.Label();
			this.label85 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.groupBox10 = new global::System.Windows.Forms.GroupBox();
			this.chkMonday = new global::System.Windows.Forms.CheckBox();
			this.chkSunday = new global::System.Windows.Forms.CheckBox();
			this.chkTuesday = new global::System.Windows.Forms.CheckBox();
			this.chkSaturday = new global::System.Windows.Forms.CheckBox();
			this.chkWednesday = new global::System.Windows.Forms.CheckBox();
			this.chkFriday = new global::System.Windows.Forms.CheckBox();
			this.chkThursday = new global::System.Windows.Forms.CheckBox();
			this.label83 = new global::System.Windows.Forms.Label();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.dtpEnd = new global::System.Windows.Forms.DateTimePicker();
			this.dtpBegin = new global::System.Windows.Forms.DateTimePicker();
			this.label81 = new global::System.Windows.Forms.Label();
			this.label82 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.groupBox3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfMonth).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfDay).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS3).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS2).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS1).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkNotAllowInHolidays, "chkNotAllowInHolidays");
			this.chkNotAllowInHolidays.Checked = true;
			this.chkNotAllowInHolidays.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkNotAllowInHolidays.ForeColor = global::System.Drawing.Color.White;
			this.chkNotAllowInHolidays.Name = "chkNotAllowInHolidays";
			this.chkNotAllowInHolidays.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			this.cmdCancel.Click += new global::System.EventHandler(this.cmdCancel_Click);
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.txtf_ControlSegName, "txtf_ControlSegName");
			this.txtf_ControlSegName.Name = "txtf_ControlSegName";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfMonth);
			this.groupBox3.Controls.Add(this.optReaderCount);
			this.groupBox3.Controls.Add(this.optControllerCount);
			this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfDay);
			this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS3);
			this.groupBox3.Controls.Add(this.label93);
			this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS2);
			this.groupBox3.Controls.Add(this.label92);
			this.groupBox3.Controls.Add(this.nudf_LimitedTimesOfHMS1);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label94);
			this.groupBox3.Controls.Add(this.label91);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfMonth, "nudf_LimitedTimesOfMonth");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudf_LimitedTimesOfMonth;
			int[] array = new int[4];
			array[0] = 254;
			numericUpDown.Maximum = new decimal(array);
			this.nudf_LimitedTimesOfMonth.Name = "nudf_LimitedTimesOfMonth";
			this.nudf_LimitedTimesOfMonth.ReadOnly = true;
			componentResourceManager.ApplyResources(this.optReaderCount, "optReaderCount");
			this.optReaderCount.Name = "optReaderCount";
			this.optReaderCount.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optControllerCount, "optControllerCount");
			this.optControllerCount.Checked = true;
			this.optControllerCount.Name = "optControllerCount";
			this.optControllerCount.TabStop = true;
			this.optControllerCount.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfDay, "nudf_LimitedTimesOfDay");
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudf_LimitedTimesOfDay;
			int[] array2 = new int[4];
			array2[0] = 254;
			numericUpDown2.Maximum = new decimal(array2);
			this.nudf_LimitedTimesOfDay.Name = "nudf_LimitedTimesOfDay";
			this.nudf_LimitedTimesOfDay.ReadOnly = true;
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfHMS3, "nudf_LimitedTimesOfHMS3");
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudf_LimitedTimesOfHMS3;
			int[] array3 = new int[4];
			array3[0] = 31;
			numericUpDown3.Maximum = new decimal(array3);
			this.nudf_LimitedTimesOfHMS3.Name = "nudf_LimitedTimesOfHMS3";
			this.nudf_LimitedTimesOfHMS3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label93, "label93");
			this.label93.Name = "label93";
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfHMS2, "nudf_LimitedTimesOfHMS2");
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudf_LimitedTimesOfHMS2;
			int[] array4 = new int[4];
			array4[0] = 31;
			numericUpDown4.Maximum = new decimal(array4);
			this.nudf_LimitedTimesOfHMS2.Name = "nudf_LimitedTimesOfHMS2";
			this.nudf_LimitedTimesOfHMS2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label92, "label92");
			this.label92.Name = "label92";
			componentResourceManager.ApplyResources(this.nudf_LimitedTimesOfHMS1, "nudf_LimitedTimesOfHMS1");
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudf_LimitedTimesOfHMS1;
			int[] array5 = new int[4];
			array5[0] = 31;
			numericUpDown5.Maximum = new decimal(array5);
			this.nudf_LimitedTimesOfHMS1.Name = "nudf_LimitedTimesOfHMS1";
			this.nudf_LimitedTimesOfHMS1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label94, "label94");
			this.label94.Name = "label94";
			componentResourceManager.ApplyResources(this.label91, "label91");
			this.label91.Name = "label91";
			componentResourceManager.ApplyResources(this.chkf_ReaderCount, "chkf_ReaderCount");
			this.chkf_ReaderCount.Name = "chkf_ReaderCount";
			this.chkf_ReaderCount.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.label84);
			this.groupBox2.Controls.Add(this.cbof_ControlSegIDLinked);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label84, "label84");
			this.label84.Name = "label84";
			componentResourceManager.ApplyResources(this.cbof_ControlSegIDLinked, "cbof_ControlSegIDLinked");
			this.cbof_ControlSegIDLinked.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegIDLinked.FormattingEnabled = true;
			this.cbof_ControlSegIDLinked.Name = "cbof_ControlSegIDLinked";
			componentResourceManager.ApplyResources(this.groupBox11, "groupBox11");
			this.groupBox11.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox11.Controls.Add(this.label89);
			this.groupBox11.Controls.Add(this.label90);
			this.groupBox11.Controls.Add(this.dateBeginHMS3);
			this.groupBox11.Controls.Add(this.dateEndHMS3);
			this.groupBox11.Controls.Add(this.label87);
			this.groupBox11.Controls.Add(this.label88);
			this.groupBox11.Controls.Add(this.dateBeginHMS2);
			this.groupBox11.Controls.Add(this.dateEndHMS2);
			this.groupBox11.Controls.Add(this.label86);
			this.groupBox11.Controls.Add(this.label85);
			this.groupBox11.Controls.Add(this.dateEndHMS1);
			this.groupBox11.Controls.Add(this.dateBeginHMS1);
			this.groupBox11.ForeColor = global::System.Drawing.Color.White;
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.TabStop = false;
			componentResourceManager.ApplyResources(this.label89, "label89");
			this.label89.Name = "label89";
			componentResourceManager.ApplyResources(this.label90, "label90");
			this.label90.Name = "label90";
			componentResourceManager.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
			this.dateBeginHMS3.Name = "dateBeginHMS3";
			this.dateBeginHMS3.ShowUpDown = true;
			this.dateBeginHMS3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
			this.dateEndHMS3.Name = "dateEndHMS3";
			this.dateEndHMS3.ShowUpDown = true;
			this.dateEndHMS3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label87, "label87");
			this.label87.Name = "label87";
			componentResourceManager.ApplyResources(this.label88, "label88");
			this.label88.Name = "label88";
			componentResourceManager.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
			this.dateBeginHMS2.Name = "dateBeginHMS2";
			this.dateBeginHMS2.ShowUpDown = true;
			this.dateBeginHMS2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
			this.dateEndHMS2.Name = "dateEndHMS2";
			this.dateEndHMS2.ShowUpDown = true;
			this.dateEndHMS2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label86, "label86");
			this.label86.Name = "label86";
			componentResourceManager.ApplyResources(this.label85, "label85");
			this.label85.Name = "label85";
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.groupBox10, "groupBox10");
			this.groupBox10.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox10.Controls.Add(this.chkMonday);
			this.groupBox10.Controls.Add(this.chkSunday);
			this.groupBox10.Controls.Add(this.chkTuesday);
			this.groupBox10.Controls.Add(this.chkSaturday);
			this.groupBox10.Controls.Add(this.chkWednesday);
			this.groupBox10.Controls.Add(this.chkFriday);
			this.groupBox10.Controls.Add(this.chkThursday);
			this.groupBox10.ForeColor = global::System.Drawing.Color.White;
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.TabStop = false;
			componentResourceManager.ApplyResources(this.chkMonday, "chkMonday");
			this.chkMonday.Checked = true;
			this.chkMonday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkMonday.Name = "chkMonday";
			this.chkMonday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSunday, "chkSunday");
			this.chkSunday.Checked = true;
			this.chkSunday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSunday.Name = "chkSunday";
			this.chkSunday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkTuesday, "chkTuesday");
			this.chkTuesday.Checked = true;
			this.chkTuesday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkTuesday.Name = "chkTuesday";
			this.chkTuesday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSaturday, "chkSaturday");
			this.chkSaturday.Checked = true;
			this.chkSaturday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSaturday.Name = "chkSaturday";
			this.chkSaturday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkWednesday, "chkWednesday");
			this.chkWednesday.Checked = true;
			this.chkWednesday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkWednesday.Name = "chkWednesday";
			this.chkWednesday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkFriday, "chkFriday");
			this.chkFriday.Checked = true;
			this.chkFriday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkFriday.Name = "chkFriday";
			this.chkFriday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkThursday, "chkThursday");
			this.chkThursday.Checked = true;
			this.chkThursday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkThursday.Name = "chkThursday";
			this.chkThursday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label83, "label83");
			this.label83.BackColor = global::System.Drawing.Color.Transparent;
			this.label83.ForeColor = global::System.Drawing.Color.White;
			this.label83.Name = "label83";
			componentResourceManager.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
			this.cbof_ControlSegID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegID.FormattingEnabled = true;
			this.cbof_ControlSegID.Name = "cbof_ControlSegID";
			componentResourceManager.ApplyResources(this.dtpEnd, "dtpEnd");
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Value = new global::System.DateTime(2099, 12, 31, 14, 44, 0, 0);
			componentResourceManager.ApplyResources(this.dtpBegin, "dtpBegin");
			this.dtpBegin.Name = "dtpBegin";
			this.dtpBegin.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.label81, "label81");
			this.label81.Name = "label81";
			componentResourceManager.ApplyResources(this.label82, "label82");
			this.label82.Name = "label82";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.dtpBegin);
			this.groupBox1.Controls.Add(this.label82);
			this.groupBox1.Controls.Add(this.label81);
			this.groupBox1.Controls.Add(this.dtpEnd);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkNotAllowInHolidays);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txtf_ControlSegName);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.chkf_ReaderCount);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox11);
			base.Controls.Add(this.groupBox10);
			base.Controls.Add(this.label83);
			base.Controls.Add(this.cbof_ControlSegID);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmControlSeg";
			base.Load += new global::System.EventHandler(this.dfrmControlSeg_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControlSeg_KeyDown);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfMonth).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfDay).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS3).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS2).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudf_LimitedTimesOfHMS1).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000194 RID: 404
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000195 RID: 405
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x04000196 RID: 406
		private global::System.Windows.Forms.ComboBox cbof_ControlSegIDLinked;

		// Token: 0x04000197 RID: 407
		private global::System.Windows.Forms.CheckBox chkf_ReaderCount;

		// Token: 0x04000198 RID: 408
		private global::System.Windows.Forms.CheckBox chkFriday;

		// Token: 0x04000199 RID: 409
		private global::System.Windows.Forms.CheckBox chkMonday;

		// Token: 0x0400019A RID: 410
		private global::System.Windows.Forms.CheckBox chkNotAllowInHolidays;

		// Token: 0x0400019B RID: 411
		private global::System.Windows.Forms.CheckBox chkSaturday;

		// Token: 0x0400019C RID: 412
		private global::System.Windows.Forms.CheckBox chkSunday;

		// Token: 0x0400019D RID: 413
		private global::System.Windows.Forms.CheckBox chkThursday;

		// Token: 0x0400019E RID: 414
		private global::System.Windows.Forms.CheckBox chkTuesday;

		// Token: 0x0400019F RID: 415
		private global::System.Windows.Forms.CheckBox chkWednesday;

		// Token: 0x040001A0 RID: 416
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040001A1 RID: 417
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS2;

		// Token: 0x040001A2 RID: 418
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS3;

		// Token: 0x040001A3 RID: 419
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x040001A4 RID: 420
		private global::System.Windows.Forms.DateTimePicker dateEndHMS2;

		// Token: 0x040001A5 RID: 421
		private global::System.Windows.Forms.DateTimePicker dateEndHMS3;

		// Token: 0x040001A6 RID: 422
		private global::System.Windows.Forms.DateTimePicker dtpBegin;

		// Token: 0x040001A7 RID: 423
		private global::System.Windows.Forms.DateTimePicker dtpEnd;

		// Token: 0x040001A8 RID: 424
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040001A9 RID: 425
		private global::System.Windows.Forms.GroupBox groupBox10;

		// Token: 0x040001AA RID: 426
		private global::System.Windows.Forms.GroupBox groupBox11;

		// Token: 0x040001AB RID: 427
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040001AC RID: 428
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x040001AD RID: 429
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040001AE RID: 430
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040001AF RID: 431
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040001B0 RID: 432
		private global::System.Windows.Forms.Label label81;

		// Token: 0x040001B1 RID: 433
		private global::System.Windows.Forms.Label label82;

		// Token: 0x040001B2 RID: 434
		private global::System.Windows.Forms.Label label83;

		// Token: 0x040001B3 RID: 435
		private global::System.Windows.Forms.Label label84;

		// Token: 0x040001B4 RID: 436
		private global::System.Windows.Forms.Label label85;

		// Token: 0x040001B5 RID: 437
		private global::System.Windows.Forms.Label label86;

		// Token: 0x040001B6 RID: 438
		private global::System.Windows.Forms.Label label87;

		// Token: 0x040001B7 RID: 439
		private global::System.Windows.Forms.Label label88;

		// Token: 0x040001B8 RID: 440
		private global::System.Windows.Forms.Label label89;

		// Token: 0x040001B9 RID: 441
		private global::System.Windows.Forms.Label label90;

		// Token: 0x040001BA RID: 442
		private global::System.Windows.Forms.Label label91;

		// Token: 0x040001BB RID: 443
		private global::System.Windows.Forms.Label label92;

		// Token: 0x040001BC RID: 444
		private global::System.Windows.Forms.Label label93;

		// Token: 0x040001BD RID: 445
		private global::System.Windows.Forms.Label label94;

		// Token: 0x040001BE RID: 446
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfDay;

		// Token: 0x040001BF RID: 447
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfHMS1;

		// Token: 0x040001C0 RID: 448
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfHMS2;

		// Token: 0x040001C1 RID: 449
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfHMS3;

		// Token: 0x040001C2 RID: 450
		private global::System.Windows.Forms.NumericUpDown nudf_LimitedTimesOfMonth;

		// Token: 0x040001C3 RID: 451
		private global::System.Windows.Forms.RadioButton optControllerCount;

		// Token: 0x040001C4 RID: 452
		private global::System.Windows.Forms.RadioButton optReaderCount;

		// Token: 0x040001C5 RID: 453
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x040001C6 RID: 454
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x040001C7 RID: 455
		internal global::System.Windows.Forms.TextBox txtf_ControlSegName;
	}
}
