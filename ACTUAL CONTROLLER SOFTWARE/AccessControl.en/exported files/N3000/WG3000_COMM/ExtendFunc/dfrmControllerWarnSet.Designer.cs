namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000240 RID: 576
	public partial class dfrmControllerWarnSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060011A8 RID: 4520 RVA: 0x0014ADBA File Offset: 0x00149DBA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmFind1 != null)
			{
				this.dfrmFind1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0014ADF0 File Offset: 0x00149DF0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerWarnSet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_ControllerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_InterLock123 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_InterLock34 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_ForcedOpen = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_InterLock1234 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Doors = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnExtension = new global::System.Windows.Forms.Button();
			this.btnChangeThreatPassword = new global::System.Windows.Forms.Button();
			this.lblThreatPassword = new global::System.Windows.Forms.Label();
			this.lblOpenDoorTimeout = new global::System.Windows.Forms.Label();
			this.nudOpenDoorTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.chkActiveFireSignalShare = new global::System.Windows.Forms.CheckBox();
			this.chkGrouped = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOpenDoorTimeout).BeginInit();
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
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ControllerID, this.f_ControllerSN, this.f_InterLock123, this.f_InterLock34, this.f_ForcedOpen, this.f_InterLock1234, this.f_Doors });
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.DoubleClick += new global::System.EventHandler(this.dataGridView1_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ControllerID, "f_ControllerID");
			this.f_ControllerID.Name = "f_ControllerID";
			this.f_ControllerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_InterLock123, "f_InterLock123");
			this.f_InterLock123.Name = "f_InterLock123";
			componentResourceManager.ApplyResources(this.f_InterLock34, "f_InterLock34");
			this.f_InterLock34.Name = "f_InterLock34";
			componentResourceManager.ApplyResources(this.f_ForcedOpen, "f_ForcedOpen");
			this.f_ForcedOpen.Name = "f_ForcedOpen";
			this.f_ForcedOpen.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_ForcedOpen.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_InterLock1234, "f_InterLock1234");
			this.f_InterLock1234.Name = "f_InterLock1234";
			this.f_Doors.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Doors, "f_Doors");
			this.f_Doors.Name = "f_Doors";
			this.f_Doors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnExtension, "btnExtension");
			this.btnExtension.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExtension.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExtension.ForeColor = global::System.Drawing.Color.White;
			this.btnExtension.Name = "btnExtension";
			this.btnExtension.UseVisualStyleBackColor = false;
			this.btnExtension.Click += new global::System.EventHandler(this.btnExtension_Click);
			componentResourceManager.ApplyResources(this.btnChangeThreatPassword, "btnChangeThreatPassword");
			this.btnChangeThreatPassword.BackColor = global::System.Drawing.Color.Transparent;
			this.btnChangeThreatPassword.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnChangeThreatPassword.ForeColor = global::System.Drawing.Color.White;
			this.btnChangeThreatPassword.Name = "btnChangeThreatPassword";
			this.btnChangeThreatPassword.UseVisualStyleBackColor = false;
			this.btnChangeThreatPassword.Click += new global::System.EventHandler(this.btnChangeThreatPassword_Click);
			componentResourceManager.ApplyResources(this.lblThreatPassword, "lblThreatPassword");
			this.lblThreatPassword.BackColor = global::System.Drawing.Color.Transparent;
			this.lblThreatPassword.ForeColor = global::System.Drawing.Color.White;
			this.lblThreatPassword.Name = "lblThreatPassword";
			componentResourceManager.ApplyResources(this.lblOpenDoorTimeout, "lblOpenDoorTimeout");
			this.lblOpenDoorTimeout.BackColor = global::System.Drawing.Color.Transparent;
			this.lblOpenDoorTimeout.ForeColor = global::System.Drawing.Color.White;
			this.lblOpenDoorTimeout.Name = "lblOpenDoorTimeout";
			componentResourceManager.ApplyResources(this.nudOpenDoorTimeout, "nudOpenDoorTimeout");
			this.nudOpenDoorTimeout.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudOpenDoorTimeout;
			int[] array = new int[4];
			array[0] = 6500;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudOpenDoorTimeout;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudOpenDoorTimeout.Name = "nudOpenDoorTimeout";
			this.nudOpenDoorTimeout.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudOpenDoorTimeout;
			int[] array3 = new int[4];
			array3[0] = 25;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkActiveFireSignalShare, "chkActiveFireSignalShare");
			this.chkActiveFireSignalShare.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveFireSignalShare.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveFireSignalShare.Name = "chkActiveFireSignalShare";
			this.chkActiveFireSignalShare.UseVisualStyleBackColor = false;
			this.chkActiveFireSignalShare.CheckedChanged += new global::System.EventHandler(this.chkActiveFireSignalShare_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkGrouped, "chkGrouped");
			this.chkGrouped.BackColor = global::System.Drawing.Color.Transparent;
			this.chkGrouped.ForeColor = global::System.Drawing.Color.White;
			this.chkGrouped.Name = "chkGrouped";
			this.chkGrouped.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkGrouped);
			base.Controls.Add(this.chkActiveFireSignalShare);
			base.Controls.Add(this.nudOpenDoorTimeout);
			base.Controls.Add(this.lblOpenDoorTimeout);
			base.Controls.Add(this.lblThreatPassword);
			base.Controls.Add(this.btnChangeThreatPassword);
			base.Controls.Add(this.btnExtension);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmControllerWarnSet";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerWarnSet_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerInterLock_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerWarnSet_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudOpenDoorTimeout).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001FA9 RID: 8105
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001FAA RID: 8106
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001FAB RID: 8107
		private global::System.Windows.Forms.Button btnChangeThreatPassword;

		// Token: 0x04001FAC RID: 8108
		private global::System.Windows.Forms.Button btnExtension;

		// Token: 0x04001FAD RID: 8109
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001FAE RID: 8110
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001FAF RID: 8111
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerID;

		// Token: 0x04001FB0 RID: 8112
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001FB1 RID: 8113
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Doors;

		// Token: 0x04001FB2 RID: 8114
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_ForcedOpen;

		// Token: 0x04001FB3 RID: 8115
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock123;

		// Token: 0x04001FB4 RID: 8116
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock1234;

		// Token: 0x04001FB5 RID: 8117
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock34;

		// Token: 0x04001FB6 RID: 8118
		private global::System.Windows.Forms.Label lblOpenDoorTimeout;

		// Token: 0x04001FB7 RID: 8119
		private global::System.Windows.Forms.Label lblThreatPassword;

		// Token: 0x04001FB8 RID: 8120
		private global::System.Windows.Forms.NumericUpDown nudOpenDoorTimeout;

		// Token: 0x04001FB9 RID: 8121
		internal global::System.Windows.Forms.CheckBox chkActiveFireSignalShare;

		// Token: 0x04001FBA RID: 8122
		internal global::System.Windows.Forms.CheckBox chkGrouped;
	}
}
