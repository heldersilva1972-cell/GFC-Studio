namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000373 RID: 883
	public partial class dfrmShiftNormalParamSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001CFE RID: 7422 RVA: 0x00263BAC File Offset: 0x00262BAC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x00263BCC File Offset: 0x00262BCC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftNormalParamSet));
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.tabPage5 = new global::System.Windows.Forms.TabPage();
			this.tabPage6 = new global::System.Windows.Forms.TabPage();
			this.tabPage7 = new global::System.Windows.Forms.TabPage();
			this.tabPage8 = new global::System.Windows.Forms.TabPage();
			this.btnOption = new global::System.Windows.Forms.Button();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.cboLeaveAbsenceDay = new global::System.Windows.Forms.ComboBox();
			this.Label16 = new global::System.Windows.Forms.Label();
			this.Label15 = new global::System.Windows.Forms.Label();
			this.Label13 = new global::System.Windows.Forms.Label();
			this.Label12 = new global::System.Windows.Forms.Label();
			this.nudOvertimeTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.cboLateAbsenceDay = new global::System.Windows.Forms.ComboBox();
			this.nudLeaveAbsenceTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.nudLeaveTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.nudLateAbsenceTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.nudLateTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label14 = new global::System.Windows.Forms.Label();
			this.Label17 = new global::System.Windows.Forms.Label();
			this.dtpOffduty0 = new global::System.Windows.Forms.DateTimePicker();
			this.dtpOnduty0 = new global::System.Windows.Forms.DateTimePicker();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.grpbTwoTimes = new global::System.Windows.Forms.GroupBox();
			this.optReadCardTwoTimes = new global::System.Windows.Forms.RadioButton();
			this.optReadCardFourTimes = new global::System.Windows.Forms.RadioButton();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.dtpOnduty2 = new global::System.Windows.Forms.DateTimePicker();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.grpbFourtimes = new global::System.Windows.Forms.GroupBox();
			this.Label8 = new global::System.Windows.Forms.Label();
			this.dtpOnduty1 = new global::System.Windows.Forms.DateTimePicker();
			this.dtpOffduty1 = new global::System.Windows.Forms.DateTimePicker();
			this.Label9 = new global::System.Windows.Forms.Label();
			this.dtpOffduty2 = new global::System.Windows.Forms.DateTimePicker();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.GroupBox2 = new global::System.Windows.Forms.GroupBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.normalShiftABCToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.disableDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.activeSwipeTwiceToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.activeSwipeFourTimesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveAbsenceTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateAbsenceTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateTimeout).BeginInit();
			this.grpbTwoTimes.SuspendLayout();
			this.grpbFourtimes.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage7);
			this.tabControl1.Controls.Add(this.tabPage8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.SelectedIndexChanged += new global::System.EventHandler(this.tabControl1_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage1.Name = "tabPage1";
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage6, "tabPage6");
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage7, "tabPage7");
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage8, "tabPage8");
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.cboLeaveAbsenceDay);
			this.GroupBox1.Controls.Add(this.Label16);
			this.GroupBox1.Controls.Add(this.Label15);
			this.GroupBox1.Controls.Add(this.Label13);
			this.GroupBox1.Controls.Add(this.Label12);
			this.GroupBox1.Controls.Add(this.nudOvertimeTimeout);
			this.GroupBox1.Controls.Add(this.cboLateAbsenceDay);
			this.GroupBox1.Controls.Add(this.nudLeaveAbsenceTimeout);
			this.GroupBox1.Controls.Add(this.nudLeaveTimeout);
			this.GroupBox1.Controls.Add(this.nudLateAbsenceTimeout);
			this.GroupBox1.Controls.Add(this.nudLateTimeout);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.Label14);
			this.GroupBox1.Controls.Add(this.Label17);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.cboLeaveAbsenceDay, "cboLeaveAbsenceDay");
			this.cboLeaveAbsenceDay.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLeaveAbsenceDay.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboLeaveAbsenceDay.Items"),
				componentResourceManager.GetString("cboLeaveAbsenceDay.Items1"),
				componentResourceManager.GetString("cboLeaveAbsenceDay.Items2")
			});
			this.cboLeaveAbsenceDay.Name = "cboLeaveAbsenceDay";
			componentResourceManager.ApplyResources(this.Label16, "Label16");
			this.Label16.Name = "Label16";
			componentResourceManager.ApplyResources(this.Label15, "Label15");
			this.Label15.Name = "Label15";
			componentResourceManager.ApplyResources(this.Label13, "Label13");
			this.Label13.Name = "Label13";
			componentResourceManager.ApplyResources(this.Label12, "Label12");
			this.Label12.Name = "Label12";
			componentResourceManager.ApplyResources(this.nudOvertimeTimeout, "nudOvertimeTimeout");
			this.nudOvertimeTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudOvertimeTimeout;
			int[] array = new int[4];
			array[0] = 600;
			numericUpDown.Maximum = new decimal(array);
			this.nudOvertimeTimeout.Name = "nudOvertimeTimeout";
			this.nudOvertimeTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudOvertimeTimeout;
			int[] array2 = new int[4];
			array2[0] = 60;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.cboLateAbsenceDay, "cboLateAbsenceDay");
			this.cboLateAbsenceDay.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLateAbsenceDay.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboLateAbsenceDay.Items"),
				componentResourceManager.GetString("cboLateAbsenceDay.Items1"),
				componentResourceManager.GetString("cboLateAbsenceDay.Items2")
			});
			this.cboLateAbsenceDay.Name = "cboLateAbsenceDay";
			componentResourceManager.ApplyResources(this.nudLeaveAbsenceTimeout, "nudLeaveAbsenceTimeout");
			this.nudLeaveAbsenceTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudLeaveAbsenceTimeout;
			int[] array3 = new int[4];
			array3[0] = 600;
			numericUpDown3.Maximum = new decimal(array3);
			this.nudLeaveAbsenceTimeout.Name = "nudLeaveAbsenceTimeout";
			this.nudLeaveAbsenceTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudLeaveAbsenceTimeout;
			int[] array4 = new int[4];
			array4[0] = 120;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.nudLeaveTimeout, "nudLeaveTimeout");
			this.nudLeaveTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudLeaveTimeout;
			int[] array5 = new int[4];
			array5[0] = 600;
			numericUpDown5.Maximum = new decimal(array5);
			this.nudLeaveTimeout.Name = "nudLeaveTimeout";
			this.nudLeaveTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudLeaveTimeout;
			int[] array6 = new int[4];
			array6[0] = 5;
			numericUpDown6.Value = new decimal(array6);
			componentResourceManager.ApplyResources(this.nudLateAbsenceTimeout, "nudLateAbsenceTimeout");
			this.nudLateAbsenceTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudLateAbsenceTimeout;
			int[] array7 = new int[4];
			array7[0] = 600;
			numericUpDown7.Maximum = new decimal(array7);
			this.nudLateAbsenceTimeout.Name = "nudLateAbsenceTimeout";
			this.nudLateAbsenceTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.nudLateAbsenceTimeout;
			int[] array8 = new int[4];
			array8[0] = 120;
			numericUpDown8.Value = new decimal(array8);
			componentResourceManager.ApplyResources(this.nudLateTimeout, "nudLateTimeout");
			this.nudLateTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown9 = this.nudLateTimeout;
			int[] array9 = new int[4];
			array9[0] = 600;
			numericUpDown9.Maximum = new decimal(array9);
			this.nudLateTimeout.Name = "nudLateTimeout";
			this.nudLateTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown10 = this.nudLateTimeout;
			int[] array10 = new int[4];
			array10[0] = 5;
			numericUpDown10.Value = new decimal(array10);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.Label4, "Label4");
			this.Label4.Name = "Label4";
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.Name = "Label5";
			componentResourceManager.ApplyResources(this.Label14, "Label14");
			this.Label14.Name = "Label14";
			componentResourceManager.ApplyResources(this.Label17, "Label17");
			this.Label17.Name = "Label17";
			componentResourceManager.ApplyResources(this.dtpOffduty0, "dtpOffduty0");
			this.dtpOffduty0.Name = "dtpOffduty0";
			this.dtpOffduty0.ShowUpDown = true;
			this.dtpOffduty0.Value = new global::System.DateTime(2004, 7, 18, 17, 30, 0, 0);
			componentResourceManager.ApplyResources(this.dtpOnduty0, "dtpOnduty0");
			this.dtpOnduty0.Name = "dtpOnduty0";
			this.dtpOnduty0.ShowUpDown = true;
			this.dtpOnduty0.Value = new global::System.DateTime(2004, 7, 18, 8, 30, 0, 0);
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this.Label6, "Label6");
			this.Label6.Name = "Label6";
			componentResourceManager.ApplyResources(this.grpbTwoTimes, "grpbTwoTimes");
			this.grpbTwoTimes.Controls.Add(this.Label6);
			this.grpbTwoTimes.Controls.Add(this.dtpOnduty0);
			this.grpbTwoTimes.Controls.Add(this.dtpOffduty0);
			this.grpbTwoTimes.Controls.Add(this.Label7);
			this.grpbTwoTimes.Name = "grpbTwoTimes";
			this.grpbTwoTimes.TabStop = false;
			componentResourceManager.ApplyResources(this.optReadCardTwoTimes, "optReadCardTwoTimes");
			this.optReadCardTwoTimes.Checked = true;
			this.optReadCardTwoTimes.Name = "optReadCardTwoTimes";
			this.optReadCardTwoTimes.TabStop = true;
			this.optReadCardTwoTimes.CheckedChanged += new global::System.EventHandler(this.optReadCardTwoTimes_CheckedChanged);
			componentResourceManager.ApplyResources(this.optReadCardFourTimes, "optReadCardFourTimes");
			this.optReadCardFourTimes.Name = "optReadCardFourTimes";
			this.optReadCardFourTimes.CheckedChanged += new global::System.EventHandler(this.optReadCardFourTimes_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.dtpOnduty2, "dtpOnduty2");
			this.dtpOnduty2.Name = "dtpOnduty2";
			this.dtpOnduty2.ShowUpDown = true;
			this.dtpOnduty2.Value = new global::System.DateTime(2004, 7, 18, 13, 30, 0, 0);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.grpbFourtimes, "grpbFourtimes");
			this.grpbFourtimes.Controls.Add(this.Label8);
			this.grpbFourtimes.Controls.Add(this.dtpOnduty1);
			this.grpbFourtimes.Controls.Add(this.dtpOffduty1);
			this.grpbFourtimes.Controls.Add(this.Label9);
			this.grpbFourtimes.Controls.Add(this.dtpOffduty2);
			this.grpbFourtimes.Controls.Add(this.Label10);
			this.grpbFourtimes.Controls.Add(this.Label11);
			this.grpbFourtimes.Controls.Add(this.dtpOnduty2);
			this.grpbFourtimes.Name = "grpbFourtimes";
			this.grpbFourtimes.TabStop = false;
			componentResourceManager.ApplyResources(this.Label8, "Label8");
			this.Label8.Name = "Label8";
			componentResourceManager.ApplyResources(this.dtpOnduty1, "dtpOnduty1");
			this.dtpOnduty1.Name = "dtpOnduty1";
			this.dtpOnduty1.ShowUpDown = true;
			this.dtpOnduty1.Value = new global::System.DateTime(2004, 7, 18, 8, 30, 0, 0);
			componentResourceManager.ApplyResources(this.dtpOffduty1, "dtpOffduty1");
			this.dtpOffduty1.Name = "dtpOffduty1";
			this.dtpOffduty1.ShowUpDown = true;
			this.dtpOffduty1.Value = new global::System.DateTime(2004, 7, 18, 12, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label9, "Label9");
			this.Label9.Name = "Label9";
			componentResourceManager.ApplyResources(this.dtpOffduty2, "dtpOffduty2");
			this.dtpOffduty2.Name = "dtpOffduty2";
			this.dtpOffduty2.ShowUpDown = true;
			this.dtpOffduty2.Value = new global::System.DateTime(2004, 7, 18, 17, 30, 0, 0);
			componentResourceManager.ApplyResources(this.Label10, "Label10");
			this.Label10.Name = "Label10";
			componentResourceManager.ApplyResources(this.GroupBox2, "GroupBox2");
			this.GroupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox2.Controls.Add(this.grpbFourtimes);
			this.GroupBox2.Controls.Add(this.grpbTwoTimes);
			this.GroupBox2.Controls.Add(this.optReadCardTwoTimes);
			this.GroupBox2.Controls.Add(this.optReadCardFourTimes);
			this.GroupBox2.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.normalShiftABCToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.normalShiftABCToolStripMenuItem, "normalShiftABCToolStripMenuItem");
			this.normalShiftABCToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.disableDefaultToolStripMenuItem, this.activeSwipeTwiceToolStripMenuItem, this.activeSwipeFourTimesToolStripMenuItem });
			this.normalShiftABCToolStripMenuItem.Name = "normalShiftABCToolStripMenuItem";
			componentResourceManager.ApplyResources(this.disableDefaultToolStripMenuItem, "disableDefaultToolStripMenuItem");
			this.disableDefaultToolStripMenuItem.Name = "disableDefaultToolStripMenuItem";
			this.disableDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.disableDefaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.activeSwipeTwiceToolStripMenuItem, "activeSwipeTwiceToolStripMenuItem");
			this.activeSwipeTwiceToolStripMenuItem.Name = "activeSwipeTwiceToolStripMenuItem";
			this.activeSwipeTwiceToolStripMenuItem.Click += new global::System.EventHandler(this.activeSwipeTwiceToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.activeSwipeFourTimesToolStripMenuItem, "activeSwipeFourTimesToolStripMenuItem");
			this.activeSwipeFourTimesToolStripMenuItem.Name = "activeSwipeFourTimesToolStripMenuItem";
			this.activeSwipeFourTimesToolStripMenuItem.Click += new global::System.EventHandler(this.activeSwipeFourTimesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.btnOption);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.GroupBox2);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftNormalParamSet";
			base.Load += new global::System.EventHandler(this.dfrmShiftNormalParamSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmShiftNormalParamSet_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveAbsenceTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateAbsenceTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateTimeout).EndInit();
			this.grpbTwoTimes.ResumeLayout(false);
			this.grpbFourtimes.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04003796 RID: 14230
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003797 RID: 14231
		private global::System.Windows.Forms.ToolStripMenuItem activeSwipeFourTimesToolStripMenuItem;

		// Token: 0x04003798 RID: 14232
		private global::System.Windows.Forms.ToolStripMenuItem activeSwipeTwiceToolStripMenuItem;

		// Token: 0x04003799 RID: 14233
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400379A RID: 14234
		private global::System.Windows.Forms.ToolStripMenuItem disableDefaultToolStripMenuItem;

		// Token: 0x0400379B RID: 14235
		private global::System.Windows.Forms.ToolStripMenuItem normalShiftABCToolStripMenuItem;

		// Token: 0x0400379C RID: 14236
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x0400379D RID: 14237
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x0400379E RID: 14238
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x0400379F RID: 14239
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x040037A0 RID: 14240
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x040037A1 RID: 14241
		private global::System.Windows.Forms.TabPage tabPage5;

		// Token: 0x040037A2 RID: 14242
		private global::System.Windows.Forms.TabPage tabPage6;

		// Token: 0x040037A3 RID: 14243
		private global::System.Windows.Forms.TabPage tabPage7;

		// Token: 0x040037A4 RID: 14244
		private global::System.Windows.Forms.TabPage tabPage8;

		// Token: 0x040037A5 RID: 14245
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040037A6 RID: 14246
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040037A7 RID: 14247
		internal global::System.Windows.Forms.Button btnOption;

		// Token: 0x040037A8 RID: 14248
		internal global::System.Windows.Forms.ComboBox cboLateAbsenceDay;

		// Token: 0x040037A9 RID: 14249
		internal global::System.Windows.Forms.ComboBox cboLeaveAbsenceDay;

		// Token: 0x040037AA RID: 14250
		internal global::System.Windows.Forms.DateTimePicker dtpOffduty0;

		// Token: 0x040037AB RID: 14251
		internal global::System.Windows.Forms.DateTimePicker dtpOffduty1;

		// Token: 0x040037AC RID: 14252
		internal global::System.Windows.Forms.DateTimePicker dtpOffduty2;

		// Token: 0x040037AD RID: 14253
		internal global::System.Windows.Forms.DateTimePicker dtpOnduty0;

		// Token: 0x040037AE RID: 14254
		internal global::System.Windows.Forms.DateTimePicker dtpOnduty1;

		// Token: 0x040037AF RID: 14255
		internal global::System.Windows.Forms.DateTimePicker dtpOnduty2;

		// Token: 0x040037B0 RID: 14256
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x040037B1 RID: 14257
		internal global::System.Windows.Forms.GroupBox GroupBox2;

		// Token: 0x040037B2 RID: 14258
		internal global::System.Windows.Forms.GroupBox grpbFourtimes;

		// Token: 0x040037B3 RID: 14259
		internal global::System.Windows.Forms.GroupBox grpbTwoTimes;

		// Token: 0x040037B4 RID: 14260
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040037B5 RID: 14261
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x040037B6 RID: 14262
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x040037B7 RID: 14263
		internal global::System.Windows.Forms.Label Label12;

		// Token: 0x040037B8 RID: 14264
		internal global::System.Windows.Forms.Label Label13;

		// Token: 0x040037B9 RID: 14265
		internal global::System.Windows.Forms.Label Label14;

		// Token: 0x040037BA RID: 14266
		internal global::System.Windows.Forms.Label Label15;

		// Token: 0x040037BB RID: 14267
		internal global::System.Windows.Forms.Label Label16;

		// Token: 0x040037BC RID: 14268
		internal global::System.Windows.Forms.Label Label17;

		// Token: 0x040037BD RID: 14269
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x040037BE RID: 14270
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040037BF RID: 14271
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x040037C0 RID: 14272
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x040037C1 RID: 14273
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x040037C2 RID: 14274
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x040037C3 RID: 14275
		internal global::System.Windows.Forms.Label Label8;

		// Token: 0x040037C4 RID: 14276
		internal global::System.Windows.Forms.Label Label9;

		// Token: 0x040037C5 RID: 14277
		internal global::System.Windows.Forms.NumericUpDown nudLateAbsenceTimeout;

		// Token: 0x040037C6 RID: 14278
		internal global::System.Windows.Forms.NumericUpDown nudLateTimeout;

		// Token: 0x040037C7 RID: 14279
		internal global::System.Windows.Forms.NumericUpDown nudLeaveAbsenceTimeout;

		// Token: 0x040037C8 RID: 14280
		internal global::System.Windows.Forms.NumericUpDown nudLeaveTimeout;

		// Token: 0x040037C9 RID: 14281
		internal global::System.Windows.Forms.NumericUpDown nudOvertimeTimeout;

		// Token: 0x040037CA RID: 14282
		internal global::System.Windows.Forms.RadioButton optReadCardFourTimes;

		// Token: 0x040037CB RID: 14283
		internal global::System.Windows.Forms.RadioButton optReadCardTwoTimes;
	}
}
