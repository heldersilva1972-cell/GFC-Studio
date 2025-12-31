namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000374 RID: 884
	public partial class dfrmShiftOtherParamSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001D0A RID: 7434 RVA: 0x002655D2 File Offset: 0x002645D2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x002655F4 File Offset: 0x002645F4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmShiftOtherParamSet));
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.nudAheadMinutes = new global::System.Windows.Forms.NumericUpDown();
			this.Label16 = new global::System.Windows.Forms.Label();
			this.Label15 = new global::System.Windows.Forms.Label();
			this.Label13 = new global::System.Windows.Forms.Label();
			this.Label12 = new global::System.Windows.Forms.Label();
			this.nudOvertimeTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.nudLeaveTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.nudLateTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label17 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.nudOvertimeMinutes = new global::System.Windows.Forms.NumericUpDown();
			this.Label14 = new global::System.Windows.Forms.Label();
			this.GroupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudAheadMinutes).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeMinutes).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.nudAheadMinutes);
			this.GroupBox1.Controls.Add(this.Label16);
			this.GroupBox1.Controls.Add(this.Label15);
			this.GroupBox1.Controls.Add(this.Label13);
			this.GroupBox1.Controls.Add(this.Label12);
			this.GroupBox1.Controls.Add(this.nudOvertimeTimeout);
			this.GroupBox1.Controls.Add(this.nudLeaveTimeout);
			this.GroupBox1.Controls.Add(this.nudLateTimeout);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.Label17);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.nudAheadMinutes, "nudAheadMinutes");
			this.nudAheadMinutes.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudAheadMinutes;
			int[] array = new int[4];
			array[0] = 600;
			numericUpDown.Maximum = new decimal(array);
			this.nudAheadMinutes.Name = "nudAheadMinutes";
			this.nudAheadMinutes.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudAheadMinutes;
			int[] array2 = new int[4];
			array2[0] = 60;
			numericUpDown2.Value = new decimal(array2);
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
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudOvertimeTimeout;
			int[] array3 = new int[4];
			array3[0] = 600;
			numericUpDown3.Maximum = new decimal(array3);
			this.nudOvertimeTimeout.Name = "nudOvertimeTimeout";
			this.nudOvertimeTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudOvertimeTimeout;
			int[] array4 = new int[4];
			array4[0] = 60;
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
			componentResourceManager.ApplyResources(this.nudLateTimeout, "nudLateTimeout");
			this.nudLateTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudLateTimeout;
			int[] array7 = new int[4];
			array7[0] = 600;
			numericUpDown7.Maximum = new decimal(array7);
			this.nudLateTimeout.Name = "nudLateTimeout";
			this.nudLateTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.nudLateTimeout;
			int[] array8 = new int[4];
			array8[0] = 5;
			numericUpDown8.Value = new decimal(array8);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.Label4, "Label4");
			this.Label4.Name = "Label4";
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.Name = "Label5";
			componentResourceManager.ApplyResources(this.Label17, "Label17");
			this.Label17.Name = "Label17";
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
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.nudOvertimeMinutes, "nudOvertimeMinutes");
			global::System.Windows.Forms.NumericUpDown numericUpDown9 = this.nudOvertimeMinutes;
			int[] array9 = new int[4];
			array9[0] = 600;
			numericUpDown9.Maximum = new decimal(array9);
			this.nudOvertimeMinutes.Name = "nudOvertimeMinutes";
			global::System.Windows.Forms.NumericUpDown numericUpDown10 = this.nudOvertimeMinutes;
			int[] array10 = new int[4];
			array10[0] = 360;
			numericUpDown10.Value = new decimal(array10);
			componentResourceManager.ApplyResources(this.Label14, "Label14");
			this.Label14.BackColor = global::System.Drawing.Color.Transparent;
			this.Label14.ForeColor = global::System.Drawing.Color.White;
			this.Label14.Name = "Label14";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.nudOvertimeMinutes);
			base.Controls.Add(this.Label14);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmShiftOtherParamSet";
			base.Load += new global::System.EventHandler(this.dfrmShiftOtherParamSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmShiftOtherParamSet_KeyDown);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudAheadMinutes).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLeaveTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLateTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOvertimeMinutes).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040037CC RID: 14284
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040037CD RID: 14285
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040037CE RID: 14286
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040037CF RID: 14287
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x040037D0 RID: 14288
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040037D1 RID: 14289
		internal global::System.Windows.Forms.Label Label12;

		// Token: 0x040037D2 RID: 14290
		internal global::System.Windows.Forms.Label Label13;

		// Token: 0x040037D3 RID: 14291
		internal global::System.Windows.Forms.Label Label14;

		// Token: 0x040037D4 RID: 14292
		internal global::System.Windows.Forms.Label Label15;

		// Token: 0x040037D5 RID: 14293
		internal global::System.Windows.Forms.Label Label16;

		// Token: 0x040037D6 RID: 14294
		internal global::System.Windows.Forms.Label Label17;

		// Token: 0x040037D7 RID: 14295
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x040037D8 RID: 14296
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040037D9 RID: 14297
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x040037DA RID: 14298
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x040037DB RID: 14299
		internal global::System.Windows.Forms.NumericUpDown nudAheadMinutes;

		// Token: 0x040037DC RID: 14300
		internal global::System.Windows.Forms.NumericUpDown nudLateTimeout;

		// Token: 0x040037DD RID: 14301
		internal global::System.Windows.Forms.NumericUpDown nudLeaveTimeout;

		// Token: 0x040037DE RID: 14302
		internal global::System.Windows.Forms.NumericUpDown nudOvertimeMinutes;

		// Token: 0x040037DF RID: 14303
		internal global::System.Windows.Forms.NumericUpDown nudOvertimeTimeout;
	}
}
