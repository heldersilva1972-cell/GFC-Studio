namespace WG3000_COMM.Basic
{
	// Token: 0x02000039 RID: 57
	public partial class dfrmUserBatchUpdate : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x000709D6 File Offset: 0x0006F9D6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000709F8 File Offset: 0x0006F9F8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUserBatchUpdate));
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.opt1a = new global::System.Windows.Forms.RadioButton();
			this.opt1b = new global::System.Windows.Forms.RadioButton();
			this.chk1 = new global::System.Windows.Forms.CheckBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.chk2 = new global::System.Windows.Forms.CheckBox();
			this.chk3 = new global::System.Windows.Forms.CheckBox();
			this.chk4 = new global::System.Windows.Forms.CheckBox();
			this.GroupBox2 = new global::System.Windows.Forms.GroupBox();
			this.cmbNormalShift = new global::System.Windows.Forms.ComboBox();
			this.opt2a = new global::System.Windows.Forms.RadioButton();
			this.opt2b = new global::System.Windows.Forms.RadioButton();
			this.GroupBox3 = new global::System.Windows.Forms.GroupBox();
			this.opt3a = new global::System.Windows.Forms.RadioButton();
			this.opt3b = new global::System.Windows.Forms.RadioButton();
			this.cbof_GroupNew = new global::System.Windows.Forms.ComboBox();
			this.chk5 = new global::System.Windows.Forms.CheckBox();
			this.dtpEnd = new global::System.Windows.Forms.DateTimePicker();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.GroupBox4 = new global::System.Windows.Forms.GroupBox();
			this.lblActivateTime = new global::System.Windows.Forms.Label();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.chkEnd = new global::System.Windows.Forms.CheckBox();
			this.chkStart = new global::System.Windows.Forms.CheckBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dtpBegin = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.txtf_PIN = new global::System.Windows.Forms.MaskedTextBox();
			this.chk6 = new global::System.Windows.Forms.CheckBox();
			this.chkIncludeAllBranch = new global::System.Windows.Forms.CheckBox();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			this.GroupBox4.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.opt1a);
			this.GroupBox1.Controls.Add(this.opt1b);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox1, componentResourceManager.GetString("GroupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.opt1a, "opt1a");
			this.opt1a.Checked = true;
			this.opt1a.Name = "opt1a";
			this.opt1a.TabStop = true;
			this.toolTip1.SetToolTip(this.opt1a, componentResourceManager.GetString("opt1a.ToolTip"));
			componentResourceManager.ApplyResources(this.opt1b, "opt1b");
			this.opt1b.Name = "opt1b";
			this.toolTip1.SetToolTip(this.opt1b, componentResourceManager.GetString("opt1b.ToolTip"));
			componentResourceManager.ApplyResources(this.chk1, "chk1");
			this.chk1.BackColor = global::System.Drawing.Color.Transparent;
			this.chk1.ForeColor = global::System.Drawing.Color.White;
			this.chk1.Name = "chk1";
			this.toolTip1.SetToolTip(this.chk1, componentResourceManager.GetString("chk1.ToolTip"));
			this.chk1.UseVisualStyleBackColor = false;
			this.chk1.CheckedChanged += new global::System.EventHandler(this.chk1_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			this.toolTip1.SetToolTip(this.Label3, componentResourceManager.GetString("Label3.ToolTip"));
			componentResourceManager.ApplyResources(this.chk2, "chk2");
			this.chk2.BackColor = global::System.Drawing.Color.Transparent;
			this.chk2.ForeColor = global::System.Drawing.Color.White;
			this.chk2.Name = "chk2";
			this.toolTip1.SetToolTip(this.chk2, componentResourceManager.GetString("chk2.ToolTip"));
			this.chk2.UseVisualStyleBackColor = false;
			this.chk2.CheckedChanged += new global::System.EventHandler(this.chk2_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk3, "chk3");
			this.chk3.BackColor = global::System.Drawing.Color.Transparent;
			this.chk3.ForeColor = global::System.Drawing.Color.White;
			this.chk3.Name = "chk3";
			this.toolTip1.SetToolTip(this.chk3, componentResourceManager.GetString("chk3.ToolTip"));
			this.chk3.UseVisualStyleBackColor = false;
			this.chk3.CheckedChanged += new global::System.EventHandler(this.chk3_CheckedChanged);
			componentResourceManager.ApplyResources(this.chk4, "chk4");
			this.chk4.BackColor = global::System.Drawing.Color.Transparent;
			this.chk4.ForeColor = global::System.Drawing.Color.White;
			this.chk4.Name = "chk4";
			this.toolTip1.SetToolTip(this.chk4, componentResourceManager.GetString("chk4.ToolTip"));
			this.chk4.UseVisualStyleBackColor = false;
			this.chk4.CheckedChanged += new global::System.EventHandler(this.chk4_CheckedChanged);
			componentResourceManager.ApplyResources(this.GroupBox2, "GroupBox2");
			this.GroupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox2.Controls.Add(this.cmbNormalShift);
			this.GroupBox2.Controls.Add(this.opt2a);
			this.GroupBox2.Controls.Add(this.opt2b);
			this.GroupBox2.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox2, componentResourceManager.GetString("GroupBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.cmbNormalShift, "cmbNormalShift");
			this.cmbNormalShift.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNormalShift.FormattingEnabled = true;
			this.cmbNormalShift.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cmbNormalShift.Items"),
				componentResourceManager.GetString("cmbNormalShift.Items1"),
				componentResourceManager.GetString("cmbNormalShift.Items2"),
				componentResourceManager.GetString("cmbNormalShift.Items3"),
				componentResourceManager.GetString("cmbNormalShift.Items4"),
				componentResourceManager.GetString("cmbNormalShift.Items5"),
				componentResourceManager.GetString("cmbNormalShift.Items6"),
				componentResourceManager.GetString("cmbNormalShift.Items7")
			});
			this.cmbNormalShift.Name = "cmbNormalShift";
			this.toolTip1.SetToolTip(this.cmbNormalShift, componentResourceManager.GetString("cmbNormalShift.ToolTip"));
			componentResourceManager.ApplyResources(this.opt2a, "opt2a");
			this.opt2a.Checked = true;
			this.opt2a.Name = "opt2a";
			this.opt2a.TabStop = true;
			this.toolTip1.SetToolTip(this.opt2a, componentResourceManager.GetString("opt2a.ToolTip"));
			componentResourceManager.ApplyResources(this.opt2b, "opt2b");
			this.opt2b.Name = "opt2b";
			this.toolTip1.SetToolTip(this.opt2b, componentResourceManager.GetString("opt2b.ToolTip"));
			componentResourceManager.ApplyResources(this.GroupBox3, "GroupBox3");
			this.GroupBox3.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox3.Controls.Add(this.opt3a);
			this.GroupBox3.Controls.Add(this.opt3b);
			this.GroupBox3.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox3, componentResourceManager.GetString("GroupBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.opt3a, "opt3a");
			this.opt3a.Checked = true;
			this.opt3a.Name = "opt3a";
			this.opt3a.TabStop = true;
			this.toolTip1.SetToolTip(this.opt3a, componentResourceManager.GetString("opt3a.ToolTip"));
			componentResourceManager.ApplyResources(this.opt3b, "opt3b");
			this.opt3b.Name = "opt3b";
			this.toolTip1.SetToolTip(this.opt3b, componentResourceManager.GetString("opt3b.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_GroupNew, "cbof_GroupNew");
			this.cbof_GroupNew.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupNew.Name = "cbof_GroupNew";
			this.toolTip1.SetToolTip(this.cbof_GroupNew, componentResourceManager.GetString("cbof_GroupNew.ToolTip"));
			this.cbof_GroupNew.DropDown += new global::System.EventHandler(this.cbof_GroupNew_DropDown);
			this.cbof_GroupNew.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupNew_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.chk5, "chk5");
			this.chk5.BackColor = global::System.Drawing.Color.Transparent;
			this.chk5.ForeColor = global::System.Drawing.Color.White;
			this.chk5.Name = "chk5";
			this.toolTip1.SetToolTip(this.chk5, componentResourceManager.GetString("chk5.ToolTip"));
			this.chk5.UseVisualStyleBackColor = false;
			this.chk5.CheckedChanged += new global::System.EventHandler(this.chk5_CheckedChanged);
			componentResourceManager.ApplyResources(this.dtpEnd, "dtpEnd");
			this.dtpEnd.Name = "dtpEnd";
			this.toolTip1.SetToolTip(this.dtpEnd, componentResourceManager.GetString("dtpEnd.ToolTip"));
			this.dtpEnd.Value = new global::System.DateTime(2099, 12, 31, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.Name = "Label1";
			this.toolTip1.SetToolTip(this.Label1, componentResourceManager.GetString("Label1.ToolTip"));
			componentResourceManager.ApplyResources(this.GroupBox4, "GroupBox4");
			this.GroupBox4.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox4.Controls.Add(this.lblActivateTime);
			this.GroupBox4.Controls.Add(this.dateBeginHMS1);
			this.GroupBox4.Controls.Add(this.chkEnd);
			this.GroupBox4.Controls.Add(this.chkStart);
			this.GroupBox4.Controls.Add(this.label9);
			this.GroupBox4.Controls.Add(this.dateEndHMS1);
			this.GroupBox4.Controls.Add(this.dtpBegin);
			this.GroupBox4.Controls.Add(this.Label5);
			this.GroupBox4.Controls.Add(this.dtpEnd);
			this.GroupBox4.Controls.Add(this.Label1);
			this.GroupBox4.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox4.Name = "GroupBox4";
			this.GroupBox4.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox4, componentResourceManager.GetString("GroupBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.lblActivateTime, "lblActivateTime");
			this.lblActivateTime.Name = "lblActivateTime";
			this.toolTip1.SetToolTip(this.lblActivateTime, componentResourceManager.GetString("lblActivateTime.ToolTip"));
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateBeginHMS1, componentResourceManager.GetString("dateBeginHMS1.ToolTip"));
			this.dateBeginHMS1.Value = new global::System.DateTime(2016, 9, 2, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.chkEnd, "chkEnd");
			this.chkEnd.BackColor = global::System.Drawing.Color.Transparent;
			this.chkEnd.Checked = true;
			this.chkEnd.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkEnd.ForeColor = global::System.Drawing.Color.White;
			this.chkEnd.Name = "chkEnd";
			this.toolTip1.SetToolTip(this.chkEnd, componentResourceManager.GetString("chkEnd.ToolTip"));
			this.chkEnd.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkStart, "chkStart");
			this.chkStart.BackColor = global::System.Drawing.Color.Transparent;
			this.chkStart.Checked = true;
			this.chkStart.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkStart.ForeColor = global::System.Drawing.Color.White;
			this.chkStart.Name = "chkStart";
			this.toolTip1.SetToolTip(this.chkStart, componentResourceManager.GetString("chkStart.ToolTip"));
			this.chkStart.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.toolTip1.SetToolTip(this.label9, componentResourceManager.GetString("label9.ToolTip"));
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateEndHMS1, componentResourceManager.GetString("dateEndHMS1.ToolTip"));
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			this.dateEndHMS1.ValueChanged += new global::System.EventHandler(this.dateEndHMS1_ValueChanged);
			componentResourceManager.ApplyResources(this.dtpBegin, "dtpBegin");
			this.dtpBegin.Name = "dtpBegin";
			this.toolTip1.SetToolTip(this.dtpBegin, componentResourceManager.GetString("dtpBegin.ToolTip"));
			this.dtpBegin.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.Name = "Label5";
			this.toolTip1.SetToolTip(this.Label5, componentResourceManager.GetString("Label5.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_PIN, "txtf_PIN");
			this.txtf_PIN.Name = "txtf_PIN";
			this.toolTip1.SetToolTip(this.txtf_PIN, componentResourceManager.GetString("txtf_PIN.ToolTip"));
			componentResourceManager.ApplyResources(this.chk6, "chk6");
			this.chk6.BackColor = global::System.Drawing.Color.Transparent;
			this.chk6.ForeColor = global::System.Drawing.Color.White;
			this.chk6.Name = "chk6";
			this.toolTip1.SetToolTip(this.chk6, componentResourceManager.GetString("chk6.ToolTip"));
			this.chk6.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkIncludeAllBranch, "chkIncludeAllBranch");
			this.chkIncludeAllBranch.BackColor = global::System.Drawing.Color.Transparent;
			this.chkIncludeAllBranch.Checked = true;
			this.chkIncludeAllBranch.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkIncludeAllBranch.ForeColor = global::System.Drawing.Color.White;
			this.chkIncludeAllBranch.Name = "chkIncludeAllBranch";
			this.toolTip1.SetToolTip(this.chkIncludeAllBranch, componentResourceManager.GetString("chkIncludeAllBranch.ToolTip"));
			this.chkIncludeAllBranch.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkIncludeAllBranch);
			base.Controls.Add(this.chk6);
			base.Controls.Add(this.txtf_PIN);
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.GroupBox4);
			base.Controls.Add(this.Label3);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.chk1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.chk3);
			base.Controls.Add(this.chk4);
			base.Controls.Add(this.GroupBox2);
			base.Controls.Add(this.GroupBox3);
			base.Controls.Add(this.cbof_GroupNew);
			base.Controls.Add(this.chk5);
			base.Controls.Add(this.chk2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUserBatchUpdate";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Load += new global::System.EventHandler(this.dfrmUserBatchUpdate_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmUserBatchUpdate_KeyDown);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox3.ResumeLayout(false);
			this.GroupBox4.ResumeLayout(false);
			this.GroupBox4.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000787 RID: 1927
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000788 RID: 1928
		private global::System.Windows.Forms.ComboBox cmbNormalShift;

		// Token: 0x04000789 RID: 1929
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x0400078A RID: 1930
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x0400078B RID: 1931
		private global::System.Windows.Forms.Label label9;

		// Token: 0x0400078C RID: 1932
		private global::System.Windows.Forms.Label lblActivateTime;

		// Token: 0x0400078D RID: 1933
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400078E RID: 1934
		private global::System.Windows.Forms.MaskedTextBox txtf_PIN;

		// Token: 0x0400078F RID: 1935
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000790 RID: 1936
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000791 RID: 1937
		internal global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04000792 RID: 1938
		internal global::System.Windows.Forms.ComboBox cbof_GroupNew;

		// Token: 0x04000793 RID: 1939
		internal global::System.Windows.Forms.CheckBox chk1;

		// Token: 0x04000794 RID: 1940
		internal global::System.Windows.Forms.CheckBox chk2;

		// Token: 0x04000795 RID: 1941
		internal global::System.Windows.Forms.CheckBox chk3;

		// Token: 0x04000796 RID: 1942
		internal global::System.Windows.Forms.CheckBox chk4;

		// Token: 0x04000797 RID: 1943
		internal global::System.Windows.Forms.CheckBox chk5;

		// Token: 0x04000798 RID: 1944
		internal global::System.Windows.Forms.CheckBox chk6;

		// Token: 0x04000799 RID: 1945
		internal global::System.Windows.Forms.CheckBox chkEnd;

		// Token: 0x0400079A RID: 1946
		internal global::System.Windows.Forms.CheckBox chkIncludeAllBranch;

		// Token: 0x0400079B RID: 1947
		internal global::System.Windows.Forms.CheckBox chkStart;

		// Token: 0x0400079C RID: 1948
		internal global::System.Windows.Forms.DateTimePicker dtpBegin;

		// Token: 0x0400079D RID: 1949
		internal global::System.Windows.Forms.DateTimePicker dtpEnd;

		// Token: 0x0400079E RID: 1950
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x0400079F RID: 1951
		internal global::System.Windows.Forms.GroupBox GroupBox2;

		// Token: 0x040007A0 RID: 1952
		internal global::System.Windows.Forms.GroupBox GroupBox3;

		// Token: 0x040007A1 RID: 1953
		internal global::System.Windows.Forms.GroupBox GroupBox4;

		// Token: 0x040007A2 RID: 1954
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040007A3 RID: 1955
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040007A4 RID: 1956
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x040007A5 RID: 1957
		internal global::System.Windows.Forms.RadioButton opt1a;

		// Token: 0x040007A6 RID: 1958
		internal global::System.Windows.Forms.RadioButton opt1b;

		// Token: 0x040007A7 RID: 1959
		internal global::System.Windows.Forms.RadioButton opt2a;

		// Token: 0x040007A8 RID: 1960
		internal global::System.Windows.Forms.RadioButton opt2b;

		// Token: 0x040007A9 RID: 1961
		internal global::System.Windows.Forms.RadioButton opt3a;

		// Token: 0x040007AA RID: 1962
		internal global::System.Windows.Forms.RadioButton opt3b;
	}
}
