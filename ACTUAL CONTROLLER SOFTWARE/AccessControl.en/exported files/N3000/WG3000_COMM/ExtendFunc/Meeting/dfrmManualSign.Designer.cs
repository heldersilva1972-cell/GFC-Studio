namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FC RID: 764
	public partial class dfrmManualSign : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x001CBC20 File Offset: 0x001CAC20
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x001CBC40 File Offset: 0x001CAC40
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.dfrmManualSign));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.dtpMeetingDate = new global::System.Windows.Forms.DateTimePicker();
			this.dtpMeetingTime = new global::System.Windows.Forms.DateTimePicker();
			this.btnDelete = new global::System.Windows.Forms.Button();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Identity = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IdentityStr2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MoreCards_GrpID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnFind = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOk.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOk.ForeColor = global::System.Drawing.Color.White;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.Label4, "Label4");
			this.Label4.BackColor = global::System.Drawing.Color.Transparent;
			this.Label4.ForeColor = global::System.Drawing.Color.White;
			this.Label4.Name = "Label4";
			componentResourceManager.ApplyResources(this.dtpMeetingDate, "dtpMeetingDate");
			this.dtpMeetingDate.Name = "dtpMeetingDate";
			this.dtpMeetingDate.Value = new global::System.DateTime(2008, 2, 21, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dtpMeetingTime, "dtpMeetingTime");
			this.dtpMeetingTime.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpMeetingTime.Name = "dtpMeetingTime";
			this.dtpMeetingTime.ShowUpDown = true;
			this.dtpMeetingTime.Value = new global::System.DateTime(2008, 2, 21, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelete.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelete.ForeColor = global::System.Drawing.Color.White;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new global::System.EventHandler(this.btnDelete_Click);
			componentResourceManager.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
			this.dgvSelectedUsers.AllowUserToAddRows = false;
			this.dgvSelectedUsers.AllowUserToDeleteRows = false;
			this.dgvSelectedUsers.AllowUserToOrderColumns = true;
			this.dgvSelectedUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.Identity, this.IdentityStr2, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.f_MoreCards_GrpID, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Identity, "Identity");
			this.Identity.Name = "Identity";
			this.Identity.ReadOnly = true;
			componentResourceManager.ApplyResources(this.IdentityStr2, "IdentityStr2");
			this.IdentityStr2.Name = "IdentityStr2";
			this.IdentityStr2.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MoreCards_GrpID, "f_MoreCards_GrpID");
			this.f_MoreCards_GrpID.Name = "f_MoreCards_GrpID";
			this.f_MoreCards_GrpID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvSelectedUsers);
			base.Controls.Add(this.Label4);
			base.Controls.Add(this.dtpMeetingDate);
			base.Controls.Add(this.dtpMeetingTime);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.btnDelete);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmManualSign";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmManualSign_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmManualSign_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmManualSign_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002E91 RID: 11921
		private global::System.ComponentModel.Container components;

		// Token: 0x04002E92 RID: 11922
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002E93 RID: 11923
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002E94 RID: 11924
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002E95 RID: 11925
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002E96 RID: 11926
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002E97 RID: 11927
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x04002E98 RID: 11928
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MoreCards_GrpID;

		// Token: 0x04002E99 RID: 11929
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x04002E9A RID: 11930
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Identity;

		// Token: 0x04002E9B RID: 11931
		private global::System.Windows.Forms.DataGridViewTextBoxColumn IdentityStr2;

		// Token: 0x04002E9C RID: 11932
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002E9D RID: 11933
		internal global::System.Windows.Forms.Button btnDelete;

		// Token: 0x04002E9E RID: 11934
		internal global::System.Windows.Forms.Button btnFind;

		// Token: 0x04002E9F RID: 11935
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x04002EA0 RID: 11936
		internal global::System.Windows.Forms.DateTimePicker dtpMeetingDate;

		// Token: 0x04002EA1 RID: 11937
		internal global::System.Windows.Forms.DateTimePicker dtpMeetingTime;

		// Token: 0x04002EA2 RID: 11938
		internal global::System.Windows.Forms.Label Label4;
	}
}
