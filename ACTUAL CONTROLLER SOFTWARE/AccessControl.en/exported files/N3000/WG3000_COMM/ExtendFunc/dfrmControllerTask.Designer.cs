namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x0200023C RID: 572
	public partial class dfrmControllerTask : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001169 RID: 4457 RVA: 0x0014286C File Offset: 0x0014186C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0014288C File Offset: 0x0014188C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerTask));
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cboAccessMethod = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cboDoors = new global::System.Windows.Forms.ComboBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.checkBox49 = new global::System.Windows.Forms.CheckBox();
			this.checkBox48 = new global::System.Windows.Forms.CheckBox();
			this.checkBox47 = new global::System.Windows.Forms.CheckBox();
			this.checkBox46 = new global::System.Windows.Forms.CheckBox();
			this.checkBox45 = new global::System.Windows.Forms.CheckBox();
			this.checkBox44 = new global::System.Windows.Forms.CheckBox();
			this.checkBox43 = new global::System.Windows.Forms.CheckBox();
			this.label45 = new global::System.Windows.Forms.Label();
			this.dtpTime = new global::System.Windows.Forms.DateTimePicker();
			this.label43 = new global::System.Windows.Forms.Label();
			this.label44 = new global::System.Windows.Forms.Label();
			this.dtpEnd = new global::System.Windows.Forms.DateTimePicker();
			this.dtpBegin = new global::System.Windows.Forms.DateTimePicker();
			this.chk5 = new global::System.Windows.Forms.CheckBox();
			this.chk3 = new global::System.Windows.Forms.CheckBox();
			this.chk4 = new global::System.Windows.Forms.CheckBox();
			this.chk1 = new global::System.Windows.Forms.CheckBox();
			this.chk2 = new global::System.Windows.Forms.CheckBox();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.txtTaskIDs = new global::System.Windows.Forms.TextBox();
			this.lblTaskID = new global::System.Windows.Forms.Label();
			this.chk6 = new global::System.Windows.Forms.CheckBox();
			this.groupBox3.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
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
			componentResourceManager.ApplyResources(this.txtNote, "txtNote");
			this.txtNote.Name = "txtNote";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cboAccessMethod, "cboAccessMethod");
			this.cboAccessMethod.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAccessMethod.FormattingEnabled = true;
			this.cboAccessMethod.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboAccessMethod.Items"),
				componentResourceManager.GetString("cboAccessMethod.Items1"),
				componentResourceManager.GetString("cboAccessMethod.Items2"),
				componentResourceManager.GetString("cboAccessMethod.Items3"),
				componentResourceManager.GetString("cboAccessMethod.Items4"),
				componentResourceManager.GetString("cboAccessMethod.Items5"),
				componentResourceManager.GetString("cboAccessMethod.Items6"),
				componentResourceManager.GetString("cboAccessMethod.Items7"),
				componentResourceManager.GetString("cboAccessMethod.Items8"),
				componentResourceManager.GetString("cboAccessMethod.Items9"),
				componentResourceManager.GetString("cboAccessMethod.Items10"),
				componentResourceManager.GetString("cboAccessMethod.Items11"),
				componentResourceManager.GetString("cboAccessMethod.Items12")
			});
			this.cboAccessMethod.Name = "cboAccessMethod";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.cboDoors, "cboDoors");
			this.cboDoors.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoors.FormattingEnabled = true;
			this.cboDoors.Name = "cboDoors";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox3.Controls.Add(this.checkBox49);
			this.groupBox3.Controls.Add(this.checkBox48);
			this.groupBox3.Controls.Add(this.checkBox47);
			this.groupBox3.Controls.Add(this.checkBox46);
			this.groupBox3.Controls.Add(this.checkBox45);
			this.groupBox3.Controls.Add(this.checkBox44);
			this.groupBox3.Controls.Add(this.checkBox43);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBox49, "checkBox49");
			this.checkBox49.Checked = true;
			this.checkBox49.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox49.Name = "checkBox49";
			this.checkBox49.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox48, "checkBox48");
			this.checkBox48.Checked = true;
			this.checkBox48.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox48.Name = "checkBox48";
			this.checkBox48.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox47, "checkBox47");
			this.checkBox47.Checked = true;
			this.checkBox47.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox47.Name = "checkBox47";
			this.checkBox47.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox46, "checkBox46");
			this.checkBox46.Checked = true;
			this.checkBox46.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox46.Name = "checkBox46";
			this.checkBox46.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox45, "checkBox45");
			this.checkBox45.Checked = true;
			this.checkBox45.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox45.Name = "checkBox45";
			this.checkBox45.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox44, "checkBox44");
			this.checkBox44.Checked = true;
			this.checkBox44.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox44.Name = "checkBox44";
			this.checkBox44.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox43, "checkBox43");
			this.checkBox43.Checked = true;
			this.checkBox43.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox43.Name = "checkBox43";
			this.checkBox43.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label45, "label45");
			this.label45.BackColor = global::System.Drawing.Color.Transparent;
			this.label45.ForeColor = global::System.Drawing.Color.White;
			this.label45.Name = "label45";
			componentResourceManager.ApplyResources(this.dtpTime, "dtpTime");
			this.dtpTime.Name = "dtpTime";
			this.dtpTime.ShowUpDown = true;
			this.dtpTime.Value = new global::System.DateTime(2011, 11, 30, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label43, "label43");
			this.label43.BackColor = global::System.Drawing.Color.Transparent;
			this.label43.ForeColor = global::System.Drawing.Color.White;
			this.label43.Name = "label43";
			componentResourceManager.ApplyResources(this.label44, "label44");
			this.label44.BackColor = global::System.Drawing.Color.Transparent;
			this.label44.ForeColor = global::System.Drawing.Color.White;
			this.label44.Name = "label44";
			componentResourceManager.ApplyResources(this.dtpEnd, "dtpEnd");
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Value = new global::System.DateTime(2099, 12, 31, 14, 44, 0, 0);
			componentResourceManager.ApplyResources(this.dtpBegin, "dtpBegin");
			this.dtpBegin.Name = "dtpBegin";
			this.dtpBegin.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.chk5, "chk5");
			this.chk5.BackColor = global::System.Drawing.Color.Transparent;
			this.chk5.ForeColor = global::System.Drawing.Color.White;
			this.chk5.Name = "chk5";
			this.chk5.UseVisualStyleBackColor = false;
			this.chk5.CheckedChanged += new global::System.EventHandler(this.chk5_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk3, "chk3");
			this.chk3.BackColor = global::System.Drawing.Color.Transparent;
			this.chk3.ForeColor = global::System.Drawing.Color.White;
			this.chk3.Name = "chk3";
			this.chk3.UseVisualStyleBackColor = false;
			this.chk3.CheckedChanged += new global::System.EventHandler(this.chk3_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk4, "chk4");
			this.chk4.BackColor = global::System.Drawing.Color.Transparent;
			this.chk4.ForeColor = global::System.Drawing.Color.White;
			this.chk4.Name = "chk4";
			this.chk4.UseVisualStyleBackColor = false;
			this.chk4.CheckedChanged += new global::System.EventHandler(this.chk4_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk1, "chk1");
			this.chk1.BackColor = global::System.Drawing.Color.Transparent;
			this.chk1.ForeColor = global::System.Drawing.Color.White;
			this.chk1.Name = "chk1";
			this.chk1.UseVisualStyleBackColor = false;
			this.chk1.CheckedChanged += new global::System.EventHandler(this.chk1_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk2, "chk2");
			this.chk2.BackColor = global::System.Drawing.Color.Transparent;
			this.chk2.ForeColor = global::System.Drawing.Color.White;
			this.chk2.Name = "chk2";
			this.chk2.UseVisualStyleBackColor = false;
			this.chk2.CheckedChanged += new global::System.EventHandler(this.chk2_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox6.Controls.Add(this.txtNote);
			this.groupBox6.Controls.Add(this.label3);
			this.groupBox6.ForeColor = global::System.Drawing.Color.White;
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.dtpBegin);
			this.groupBox1.Controls.Add(this.dtpEnd);
			this.groupBox1.Controls.Add(this.label44);
			this.groupBox1.Controls.Add(this.label43);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.dtpTime);
			this.groupBox2.Controls.Add(this.label45);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox5.Controls.Add(this.cboAccessMethod);
			this.groupBox5.Controls.Add(this.label1);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox4.Controls.Add(this.cboDoors);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.txtTaskIDs, "txtTaskIDs");
			this.txtTaskIDs.Name = "txtTaskIDs";
			this.txtTaskIDs.ReadOnly = true;
			componentResourceManager.ApplyResources(this.lblTaskID, "lblTaskID");
			this.lblTaskID.BackColor = global::System.Drawing.Color.Transparent;
			this.lblTaskID.ForeColor = global::System.Drawing.Color.White;
			this.lblTaskID.Name = "lblTaskID";
			componentResourceManager.ApplyResources(this.chk6, "chk6");
			this.chk6.BackColor = global::System.Drawing.Color.Transparent;
			this.chk6.ForeColor = global::System.Drawing.Color.White;
			this.chk6.Name = "chk6";
			this.chk6.UseVisualStyleBackColor = false;
			this.chk6.CheckedChanged += new global::System.EventHandler(this.chk6_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtTaskIDs);
			base.Controls.Add(this.lblTaskID);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox5);
			base.Controls.Add(this.groupBox6);
			base.Controls.Add(this.chk6);
			base.Controls.Add(this.chk5);
			base.Controls.Add(this.chk3);
			base.Controls.Add(this.chk1);
			base.Controls.Add(this.chk2);
			base.Controls.Add(this.chk4);
			base.Name = "dfrmControllerTask";
			base.Load += new global::System.EventHandler(this.dfrmControllerTask_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001EFD RID: 7933
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001EFE RID: 7934
		private global::System.Windows.Forms.ComboBox cboAccessMethod;

		// Token: 0x04001EFF RID: 7935
		private global::System.Windows.Forms.ComboBox cboDoors;

		// Token: 0x04001F00 RID: 7936
		private global::System.Windows.Forms.CheckBox checkBox43;

		// Token: 0x04001F01 RID: 7937
		private global::System.Windows.Forms.CheckBox checkBox44;

		// Token: 0x04001F02 RID: 7938
		private global::System.Windows.Forms.CheckBox checkBox45;

		// Token: 0x04001F03 RID: 7939
		private global::System.Windows.Forms.CheckBox checkBox46;

		// Token: 0x04001F04 RID: 7940
		private global::System.Windows.Forms.CheckBox checkBox47;

		// Token: 0x04001F05 RID: 7941
		private global::System.Windows.Forms.CheckBox checkBox48;

		// Token: 0x04001F06 RID: 7942
		private global::System.Windows.Forms.CheckBox checkBox49;

		// Token: 0x04001F07 RID: 7943
		private global::System.Windows.Forms.DateTimePicker dtpBegin;

		// Token: 0x04001F08 RID: 7944
		private global::System.Windows.Forms.DateTimePicker dtpEnd;

		// Token: 0x04001F09 RID: 7945
		private global::System.Windows.Forms.DateTimePicker dtpTime;

		// Token: 0x04001F0A RID: 7946
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001F0B RID: 7947
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001F0C RID: 7948
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001F0D RID: 7949
		private global::System.Windows.Forms.Label label43;

		// Token: 0x04001F0E RID: 7950
		private global::System.Windows.Forms.Label label44;

		// Token: 0x04001F0F RID: 7951
		private global::System.Windows.Forms.Label label45;

		// Token: 0x04001F10 RID: 7952
		private global::System.Windows.Forms.Label lblTaskID;

		// Token: 0x04001F11 RID: 7953
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x04001F12 RID: 7954
		internal global::System.Windows.Forms.CheckBox chk1;

		// Token: 0x04001F13 RID: 7955
		internal global::System.Windows.Forms.CheckBox chk2;

		// Token: 0x04001F14 RID: 7956
		internal global::System.Windows.Forms.CheckBox chk3;

		// Token: 0x04001F15 RID: 7957
		internal global::System.Windows.Forms.CheckBox chk4;

		// Token: 0x04001F16 RID: 7958
		internal global::System.Windows.Forms.CheckBox chk5;

		// Token: 0x04001F17 RID: 7959
		internal global::System.Windows.Forms.CheckBox chk6;

		// Token: 0x04001F18 RID: 7960
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x04001F19 RID: 7961
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x04001F1A RID: 7962
		internal global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001F1B RID: 7963
		internal global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04001F1C RID: 7964
		internal global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04001F1D RID: 7965
		internal global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04001F1E RID: 7966
		internal global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04001F1F RID: 7967
		internal global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x04001F20 RID: 7968
		public global::System.Windows.Forms.TextBox txtTaskIDs;
	}
}
