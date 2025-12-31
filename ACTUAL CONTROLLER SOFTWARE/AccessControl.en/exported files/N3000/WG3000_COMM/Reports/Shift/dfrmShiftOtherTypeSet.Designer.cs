namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000375 RID: 885
	public partial class dfrmShiftOtherTypeSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001D13 RID: 7443 RVA: 0x00267238 File Offset: 0x00266238
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00267258 File Offset: 0x00266258
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftOtherTypeSet));
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.chkBOvertimeShift1 = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.chkBOvertimeShift = new global::System.Windows.Forms.CheckBox();
			this.txtName = new global::System.Windows.Forms.TextBox();
			this.cbof_ShiftID = new global::System.Windows.Forms.ComboBox();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label8 = new global::System.Windows.Forms.Label();
			this.cbof_Readtimes = new global::System.Windows.Forms.ComboBox();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.chkBOvertimeShift2 = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.label3 = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.chkBOvertimeShift3 = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.label5 = new global::System.Windows.Forms.Label();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.chkBOvertimeShift4 = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.label10 = new global::System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.Label6);
			this.groupBox1.Controls.Add(this.chkBOvertimeShift1);
			this.groupBox1.Controls.Add(this.dateBeginHMS1);
			this.groupBox1.Controls.Add(this.dateEndHMS1);
			this.groupBox1.Controls.Add(this.Label7);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.Label6, "Label6");
			this.Label6.Name = "Label6";
			componentResourceManager.ApplyResources(this.chkBOvertimeShift1, "chkBOvertimeShift1");
			this.chkBOvertimeShift1.Name = "chkBOvertimeShift1";
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this.chkBOvertimeShift, "chkBOvertimeShift");
			this.chkBOvertimeShift.BackColor = global::System.Drawing.Color.Transparent;
			this.chkBOvertimeShift.ForeColor = global::System.Drawing.Color.White;
			this.chkBOvertimeShift.Name = "chkBOvertimeShift";
			this.chkBOvertimeShift.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			componentResourceManager.ApplyResources(this.cbof_ShiftID, "cbof_ShiftID");
			this.cbof_ShiftID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ShiftID.Name = "cbof_ShiftID";
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.Label8, "Label8");
			this.Label8.BackColor = global::System.Drawing.Color.Transparent;
			this.Label8.ForeColor = global::System.Drawing.Color.White;
			this.Label8.Name = "Label8";
			componentResourceManager.ApplyResources(this.cbof_Readtimes, "cbof_Readtimes");
			this.cbof_Readtimes.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_Readtimes.Name = "cbof_Readtimes";
			this.cbof_Readtimes.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Readtimes_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.BackColor = global::System.Drawing.Color.Transparent;
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.chkBOvertimeShift2);
			this.groupBox2.Controls.Add(this.dateBeginHMS2);
			this.groupBox2.Controls.Add(this.dateEndHMS2);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.chkBOvertimeShift2, "chkBOvertimeShift2");
			this.chkBOvertimeShift2.Name = "chkBOvertimeShift2";
			componentResourceManager.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
			this.dateBeginHMS2.Name = "dateBeginHMS2";
			this.dateBeginHMS2.ShowUpDown = true;
			this.dateBeginHMS2.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
			this.dateEndHMS2.Name = "dateEndHMS2";
			this.dateEndHMS2.ShowUpDown = true;
			this.dateEndHMS2.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.chkBOvertimeShift3);
			this.groupBox3.Controls.Add(this.dateBeginHMS3);
			this.groupBox3.Controls.Add(this.dateEndHMS3);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.chkBOvertimeShift3, "chkBOvertimeShift3");
			this.chkBOvertimeShift3.Name = "chkBOvertimeShift3";
			componentResourceManager.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
			this.dateBeginHMS3.Name = "dateBeginHMS3";
			this.dateBeginHMS3.ShowUpDown = true;
			this.dateBeginHMS3.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
			this.dateEndHMS3.Name = "dateEndHMS3";
			this.dateEndHMS3.ShowUpDown = true;
			this.dateEndHMS3.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.chkBOvertimeShift4);
			this.groupBox4.Controls.Add(this.dateBeginHMS4);
			this.groupBox4.Controls.Add(this.dateEndHMS4);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.chkBOvertimeShift4, "chkBOvertimeShift4");
			this.chkBOvertimeShift4.Name = "chkBOvertimeShift4";
			componentResourceManager.ApplyResources(this.dateBeginHMS4, "dateBeginHMS4");
			this.dateBeginHMS4.Name = "dateBeginHMS4";
			this.dateBeginHMS4.ShowUpDown = true;
			this.dateBeginHMS4.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS4, "dateEndHMS4");
			this.dateEndHMS4.Name = "dateEndHMS4";
			this.dateEndHMS4.ShowUpDown = true;
			this.dateEndHMS4.Value = new global::System.DateTime(2010, 2, 28, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.chkBOvertimeShift);
			base.Controls.Add(this.txtName);
			base.Controls.Add(this.cbof_ShiftID);
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.Label8);
			base.Controls.Add(this.cbof_Readtimes);
			base.Controls.Add(this.Label11);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox4);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftOtherTypeSet";
			base.Load += new global::System.EventHandler(this.dfrmShiftOtherTypeSet_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040037E2 RID: 14306
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040037E3 RID: 14307
		internal global::System.Windows.Forms.ComboBox cbof_Readtimes;

		// Token: 0x040037E4 RID: 14308
		internal global::System.Windows.Forms.ComboBox cbof_ShiftID;

		// Token: 0x040037E5 RID: 14309
		internal global::System.Windows.Forms.CheckBox chkBOvertimeShift;

		// Token: 0x040037E6 RID: 14310
		internal global::System.Windows.Forms.CheckBox chkBOvertimeShift1;

		// Token: 0x040037E7 RID: 14311
		internal global::System.Windows.Forms.CheckBox chkBOvertimeShift2;

		// Token: 0x040037E8 RID: 14312
		internal global::System.Windows.Forms.CheckBox chkBOvertimeShift3;

		// Token: 0x040037E9 RID: 14313
		internal global::System.Windows.Forms.CheckBox chkBOvertimeShift4;

		// Token: 0x040037EA RID: 14314
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x040037EB RID: 14315
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x040037EC RID: 14316
		internal global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040037ED RID: 14317
		internal global::System.Windows.Forms.DateTimePicker dateBeginHMS2;

		// Token: 0x040037EE RID: 14318
		internal global::System.Windows.Forms.DateTimePicker dateBeginHMS3;

		// Token: 0x040037EF RID: 14319
		internal global::System.Windows.Forms.DateTimePicker dateBeginHMS4;

		// Token: 0x040037F0 RID: 14320
		internal global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x040037F1 RID: 14321
		internal global::System.Windows.Forms.DateTimePicker dateEndHMS2;

		// Token: 0x040037F2 RID: 14322
		internal global::System.Windows.Forms.DateTimePicker dateEndHMS3;

		// Token: 0x040037F3 RID: 14323
		internal global::System.Windows.Forms.DateTimePicker dateEndHMS4;

		// Token: 0x040037F4 RID: 14324
		internal global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040037F5 RID: 14325
		internal global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040037F6 RID: 14326
		internal global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x040037F7 RID: 14327
		internal global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x040037F8 RID: 14328
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040037F9 RID: 14329
		internal global::System.Windows.Forms.Label label10;

		// Token: 0x040037FA RID: 14330
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x040037FB RID: 14331
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x040037FC RID: 14332
		internal global::System.Windows.Forms.Label label3;

		// Token: 0x040037FD RID: 14333
		internal global::System.Windows.Forms.Label label4;

		// Token: 0x040037FE RID: 14334
		internal global::System.Windows.Forms.Label label5;

		// Token: 0x040037FF RID: 14335
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x04003800 RID: 14336
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x04003801 RID: 14337
		internal global::System.Windows.Forms.Label Label8;

		// Token: 0x04003802 RID: 14338
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x04003803 RID: 14339
		internal global::System.Windows.Forms.TextBox txtName;
	}
}
