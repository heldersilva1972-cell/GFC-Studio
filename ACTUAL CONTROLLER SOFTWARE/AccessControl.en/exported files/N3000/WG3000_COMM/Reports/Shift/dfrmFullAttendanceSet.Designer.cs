namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000367 RID: 871
	public partial class dfrmFullAttendanceSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C5E RID: 7262 RVA: 0x00255AAE File Offset: 0x00254AAE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00255AD0 File Offset: 0x00254AD0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmFullAttendanceSet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn16 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvHolidayType = new global::System.Windows.Forms.DataGridView();
			this.f_No = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.chkManualRecordAsFullAttendance = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvHolidayType).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
			this.btnDelAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllUsers.Name = "btnDelAllUsers";
			this.btnDelAllUsers.UseVisualStyleBackColor = true;
			this.btnDelAllUsers.Click += new global::System.EventHandler(this.btnDelAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
			this.btnDelOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneUser.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneUser.Name = "btnDelOneUser";
			this.btnDelOneUser.UseVisualStyleBackColor = true;
			this.btnDelOneUser.Click += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
			this.btnAddOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneUser.Name = "btnAddOneUser";
			this.btnAddOneUser.UseVisualStyleBackColor = true;
			this.btnAddOneUser.Click += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
			this.btnAddAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllUsers.Name = "btnAddAllUsers";
			this.btnAddAllUsers.UseVisualStyleBackColor = true;
			this.btnAddAllUsers.Click += new global::System.EventHandler(this.btnAddAllUsers_Click);
			componentResourceManager.ApplyResources(this.dgvSelected, "dgvSelected");
			this.dgvSelected.AllowUserToAddRows = false;
			this.dgvSelected.AllowUserToDeleteRows = false;
			this.dgvSelected.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelected.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelected.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn16, this.dataGridViewTextBoxColumn17 });
			this.dgvSelected.EnableHeadersVisualStyles = false;
			this.dgvSelected.Name = "dgvSelected";
			this.dgvSelected.ReadOnly = true;
			this.dgvSelected.RowTemplate.Height = 23;
			this.dgvSelected.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSelected.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelected_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.ReadOnly = true;
			this.dataGridViewTextBoxColumn17.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvHolidayType, "dgvHolidayType");
			this.dgvHolidayType.AllowUserToAddRows = false;
			this.dgvHolidayType.AllowUserToDeleteRows = false;
			this.dgvHolidayType.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvHolidayType.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvHolidayType.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvHolidayType.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_No, this.f_DoorName });
			this.dgvHolidayType.EnableHeadersVisualStyles = false;
			this.dgvHolidayType.Name = "dgvHolidayType";
			this.dgvHolidayType.ReadOnly = true;
			this.dgvHolidayType.RowTemplate.Height = 23;
			this.dgvHolidayType.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvHolidayType.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvHolidayType_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.f_No, "f_No");
			this.f_No.Name = "f_No";
			this.f_No.ReadOnly = true;
			this.f_DoorName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.chkManualRecordAsFullAttendance, "chkManualRecordAsFullAttendance");
			this.chkManualRecordAsFullAttendance.Checked = true;
			this.chkManualRecordAsFullAttendance.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkManualRecordAsFullAttendance.ForeColor = global::System.Drawing.Color.White;
			this.chkManualRecordAsFullAttendance.Name = "chkManualRecordAsFullAttendance";
			this.chkManualRecordAsFullAttendance.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkManualRecordAsFullAttendance);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnDelAllUsers);
			base.Controls.Add(this.btnDelOneUser);
			base.Controls.Add(this.btnAddOneUser);
			base.Controls.Add(this.btnAddAllUsers);
			base.Controls.Add(this.dgvSelected);
			base.Controls.Add(this.dgvHolidayType);
			base.Name = "dfrmFullAttendanceSet";
			base.Load += new global::System.EventHandler(this.dfrmFullAttendanceSet_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvHolidayType).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400368F RID: 13967
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003690 RID: 13968
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x04003691 RID: 13969
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x04003692 RID: 13970
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04003693 RID: 13971
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04003694 RID: 13972
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04003695 RID: 13973
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003696 RID: 13974
		private global::System.Windows.Forms.CheckBox chkManualRecordAsFullAttendance;

		// Token: 0x04003697 RID: 13975
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

		// Token: 0x04003698 RID: 13976
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

		// Token: 0x04003699 RID: 13977
		private global::System.Windows.Forms.DataGridView dgvHolidayType;

		// Token: 0x0400369A RID: 13978
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x0400369B RID: 13979
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x0400369C RID: 13980
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_No;
	}
}
