namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031E RID: 798
	public partial class frmPrivilegeTypeManagement : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060018D4 RID: 6356 RVA: 0x00206B98 File Offset: 0x00205B98
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00206BB8 File Offset: 0x00205BB8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PrivilegeType.frmPrivilegeTypeManagement));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvPrivilegeTypes = new global::System.Windows.Forms.DataGridView();
			this.f_ControlSegIDBak = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PrivilegeTypeName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Active = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Doors = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Users = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditUsers = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditDoors = new global::System.Windows.Forms.ToolStripButton();
			this.btnQueryDoors = new global::System.Windows.Forms.ToolStripButton();
			this.btnSyncPrivileges = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrivilege = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dgvPrivilegeTypes, "dgvPrivilegeTypes");
			this.dgvPrivilegeTypes.AllowUserToAddRows = false;
			this.dgvPrivilegeTypes.AllowUserToDeleteRows = false;
			this.dgvPrivilegeTypes.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvPrivilegeTypes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvPrivilegeTypes.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvPrivilegeTypes.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ControlSegIDBak, this.f_PrivilegeTypeName, this.f_Active, this.f_Doors, this.f_Users, this.f_Notes });
			this.dgvPrivilegeTypes.EnableHeadersVisualStyles = false;
			this.dgvPrivilegeTypes.Name = "dgvPrivilegeTypes";
			this.dgvPrivilegeTypes.ReadOnly = true;
			this.dgvPrivilegeTypes.RowTemplate.Height = 23;
			this.dgvPrivilegeTypes.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvPrivilegeTypes.DoubleClick += new global::System.EventHandler(this.dgvControlSegs_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ControlSegIDBak, "f_ControlSegIDBak");
			this.f_ControlSegIDBak.Name = "f_ControlSegIDBak";
			this.f_ControlSegIDBak.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_PrivilegeTypeName, "f_PrivilegeTypeName");
			this.f_PrivilegeTypeName.Name = "f_PrivilegeTypeName";
			this.f_PrivilegeTypeName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Active, "f_Active");
			this.f_Active.Name = "f_Active";
			this.f_Active.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Doors, "f_Doors");
			this.f_Doors.Name = "f_Doors";
			this.f_Doors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Users, "f_Users");
			this.f_Users.Name = "f_Users";
			this.f_Users.ReadOnly = true;
			this.f_Notes.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExportToExcel, this.btnEditUsers, this.btnEditDoors, this.btnQueryDoors, this.btnSyncPrivileges, this.btnPrivilege,
				this.btnFind
			});
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Image = global::WG3000_COMM.Properties.Resources.pTools_Add;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.ForeColor = global::System.Drawing.Color.White;
			this.btnDelete.Image = global::WG3000_COMM.Properties.Resources.pTools_Del;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Click += new global::System.EventHandler(this.btnDelete_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Image = global::WG3000_COMM.Properties.Resources.pTools_Print;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
			this.btnExportToExcel.ForeColor = global::System.Drawing.Color.White;
			this.btnExportToExcel.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExportToExcel.Name = "btnExportToExcel";
			this.btnExportToExcel.Click += new global::System.EventHandler(this.btnExportToExcel_Click);
			componentResourceManager.ApplyResources(this.btnEditUsers, "btnEditUsers");
			this.btnEditUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnEditUsers.Image = global::WG3000_COMM.Properties.Resources.pTools_Operator_Group;
			this.btnEditUsers.Name = "btnEditUsers";
			this.btnEditUsers.Click += new global::System.EventHandler(this.btnEditUser_Click);
			componentResourceManager.ApplyResources(this.btnEditDoors, "btnEditDoors");
			this.btnEditDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnEditDoors.Image = global::WG3000_COMM.Properties.Resources.pTools_Operator_Zone;
			this.btnEditDoors.Name = "btnEditDoors";
			this.btnEditDoors.Click += new global::System.EventHandler(this.btnEditDoors_Click);
			componentResourceManager.ApplyResources(this.btnQueryDoors, "btnQueryDoors");
			this.btnQueryDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnQueryDoors.Image = global::WG3000_COMM.Properties.Resources.pTools_SetPwd;
			this.btnQueryDoors.Name = "btnQueryDoors";
			this.btnQueryDoors.Click += new global::System.EventHandler(this.btnQueryDoors_Click);
			componentResourceManager.ApplyResources(this.btnSyncPrivileges, "btnSyncPrivileges");
			this.btnSyncPrivileges.ForeColor = global::System.Drawing.Color.White;
			this.btnSyncPrivileges.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.btnSyncPrivileges.Name = "btnSyncPrivileges";
			this.btnSyncPrivileges.Click += new global::System.EventHandler(this.btnSyncPrivileges_Click);
			componentResourceManager.ApplyResources(this.btnPrivilege, "btnPrivilege");
			this.btnPrivilege.ForeColor = global::System.Drawing.Color.White;
			this.btnPrivilege.Image = global::WG3000_COMM.Properties.Resources.pTools_ChangePrivilege;
			this.btnPrivilege.Name = "btnPrivilege";
			this.btnPrivilege.Click += new global::System.EventHandler(this.btnPrivilege_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvPrivilegeTypes);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmPrivilegeTypeManagement";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmPrivilegeTypeManagement_FormClosing);
			base.Load += new global::System.EventHandler(this.frmControlSegs_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmControlSegs_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040032AC RID: 12972
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040032AD RID: 12973
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x040032AE RID: 12974
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x040032AF RID: 12975
		private global::System.Windows.Forms.ToolStripButton btnEditDoors;

		// Token: 0x040032B0 RID: 12976
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x040032B1 RID: 12977
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x040032B2 RID: 12978
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x040032B3 RID: 12979
		private global::System.Windows.Forms.ToolStripButton btnPrivilege;

		// Token: 0x040032B4 RID: 12980
		private global::System.Windows.Forms.ToolStripButton btnSyncPrivileges;

		// Token: 0x040032B5 RID: 12981
		private global::System.Windows.Forms.DataGridView dgvPrivilegeTypes;

		// Token: 0x040032B6 RID: 12982
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Active;

		// Token: 0x040032B7 RID: 12983
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegIDBak;

		// Token: 0x040032B8 RID: 12984
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Doors;

		// Token: 0x040032B9 RID: 12985
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x040032BA RID: 12986
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PrivilegeTypeName;

		// Token: 0x040032BB RID: 12987
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Users;

		// Token: 0x040032BC RID: 12988
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040032BD RID: 12989
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x040032BE RID: 12990
		public global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x040032BF RID: 12991
		public global::System.Windows.Forms.ToolStripButton btnEditUsers;

		// Token: 0x040032C0 RID: 12992
		public global::System.Windows.Forms.ToolStripButton btnQueryDoors;
	}
}
