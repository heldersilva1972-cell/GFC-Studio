namespace WG3000_COMM.Basic
{
	// Token: 0x02000055 RID: 85
	public partial class frmUsers : global::System.Windows.Forms.Form
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x000B0FF4 File Offset: 0x000AFFF4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.dv != null)
				{
					this.dv.Dispose();
				}
				if (this.dv4loadUserData != null)
				{
					this.dv4loadUserData.Dispose();
				}
				if (this.tb4loadUserData != null)
				{
					this.tb4loadUserData.Dispose();
				}
				if (this.userControlFind1 != null)
				{
					this.userControlFind1.Dispose();
				}
				if (disposing && this.dfrmWait1 != null)
				{
					this.dfrmWait1.Dispose();
				}
			}
			if (disposing && this.watching != null && global::WG3000_COMM.Core.wgTools.bUDPOnly64 <= 0)
			{
				this.watching.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000B10A4 File Offset: 0x000B00A4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmUsers));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Attend = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Shift = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Door = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Start = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.End = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Deptname = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PrivilegeType = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AccessTypeID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Mobile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Cert = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SecondCard = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAutoAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.btnImportFromExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnRegisterLostCard = new global::System.Windows.Forms.ToolStripButton();
			this.btnBatchUpdate = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditPrivilege = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditPrivilegeType = new global::System.Windows.Forms.ToolStripButton();
			this.btnFindBySFZID = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuPrivilegeTypeSet = new global::System.Windows.Forms.ToolStripMenuItem();
			this.editPrivilegeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrivilegeTypeManage = new global::System.Windows.Forms.ToolStripMenuItem();
			this.createQRCodeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.queryUsersWithoutPrivilegeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.batchUpdateSelectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.importFromExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deleteUserFromExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deletedUserManageToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.extendReaderSettingToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.importFromExcelWithDateToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.ConsumerID, this.ConsumerNO, this.ConsumerName, this.CardNO, this.Attend, this.Shift, this.Door, this.Start, this.End, this.Deptname,
				this.PrivilegeType, this.AccessTypeID, this.Mobile, this.Cert, this.SecondCard
			});
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsers.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvUsers_Scroll);
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.dgvUsers_DoubleClick);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ConsumerNO.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.ConsumerNO, "ConsumerNO");
			this.ConsumerNO.Name = "ConsumerNO";
			this.ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.CardNO.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Attend, "Attend");
			this.Attend.Name = "Attend";
			this.Attend.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Shift, "Shift");
			this.Shift.Name = "Shift";
			this.Shift.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Door, "Door");
			this.Door.Name = "Door";
			this.Door.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Start, "Start");
			this.Start.Name = "Start";
			this.Start.ReadOnly = true;
			componentResourceManager.ApplyResources(this.End, "End");
			this.End.Name = "End";
			this.End.ReadOnly = true;
			this.Deptname.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Deptname.FillWeight = 200f;
			componentResourceManager.ApplyResources(this.Deptname, "Deptname");
			this.Deptname.Name = "Deptname";
			this.Deptname.ReadOnly = true;
			this.PrivilegeType.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.PrivilegeType, "PrivilegeType");
			this.PrivilegeType.Name = "PrivilegeType";
			this.PrivilegeType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.AccessTypeID, "AccessTypeID");
			this.AccessTypeID.Name = "AccessTypeID";
			this.AccessTypeID.ReadOnly = true;
			this.Mobile.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Mobile, "Mobile");
			this.Mobile.Name = "Mobile";
			this.Mobile.ReadOnly = true;
			this.Cert.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Cert, "Cert");
			this.Cert.Name = "Cert";
			this.Cert.ReadOnly = true;
			componentResourceManager.ApplyResources(this.SecondCard, "SecondCard");
			this.SecondCard.Name = "SecondCard";
			this.SecondCard.ReadOnly = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.btnAutoAdd, this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExport, this.btnImportFromExcel, this.btnRegisterLostCard, this.btnBatchUpdate, this.btnEditPrivilege,
				this.btnEditPrivilegeType, this.btnFindBySFZID, this.btnFind
			});
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnAutoAdd, "btnAutoAdd");
			this.btnAutoAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAutoAdd.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Auto;
			this.btnAutoAdd.Name = "btnAutoAdd";
			this.btnAutoAdd.Click += new global::System.EventHandler(this.btnAutoAdd_Click);
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
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExport.Name = "btnExport";
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.btnImportFromExcel, "btnImportFromExcel");
			this.btnImportFromExcel.ForeColor = global::System.Drawing.Color.White;
			this.btnImportFromExcel.Image = global::WG3000_COMM.Properties.Resources.pTools_ImportFromExcel;
			this.btnImportFromExcel.Name = "btnImportFromExcel";
			this.btnImportFromExcel.Click += new global::System.EventHandler(this.btnImportFromExcel_Click);
			componentResourceManager.ApplyResources(this.btnRegisterLostCard, "btnRegisterLostCard");
			this.btnRegisterLostCard.ForeColor = global::System.Drawing.Color.White;
			this.btnRegisterLostCard.Image = global::WG3000_COMM.Properties.Resources.pTools_CardLost;
			this.btnRegisterLostCard.Name = "btnRegisterLostCard";
			this.btnRegisterLostCard.Click += new global::System.EventHandler(this.btnRegisterLostCard_Click);
			componentResourceManager.ApplyResources(this.btnBatchUpdate, "btnBatchUpdate");
			this.btnBatchUpdate.ForeColor = global::System.Drawing.Color.White;
			this.btnBatchUpdate.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit_Batch;
			this.btnBatchUpdate.Name = "btnBatchUpdate";
			this.btnBatchUpdate.Click += new global::System.EventHandler(this.btnBatchUpdate_Click);
			componentResourceManager.ApplyResources(this.btnEditPrivilege, "btnEditPrivilege");
			this.btnEditPrivilege.ForeColor = global::System.Drawing.Color.White;
			this.btnEditPrivilege.Image = global::WG3000_COMM.Properties.Resources.pTools_EditPrivielge;
			this.btnEditPrivilege.Name = "btnEditPrivilege";
			this.btnEditPrivilege.Click += new global::System.EventHandler(this.btnEditPrivilege_Click);
			componentResourceManager.ApplyResources(this.btnEditPrivilegeType, "btnEditPrivilegeType");
			this.btnEditPrivilegeType.ForeColor = global::System.Drawing.Color.White;
			this.btnEditPrivilegeType.Image = global::WG3000_COMM.Properties.Resources.pTools_ChangePrivilege;
			this.btnEditPrivilegeType.Name = "btnEditPrivilegeType";
			this.btnEditPrivilegeType.Click += new global::System.EventHandler(this.mnuPrivilegeTypeSet_Click);
			componentResourceManager.ApplyResources(this.btnFindBySFZID, "btnFindBySFZID");
			this.btnFindBySFZID.ForeColor = global::System.Drawing.Color.White;
			this.btnFindBySFZID.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnFindBySFZID.Name = "btnFindBySFZID";
			this.btnFindBySFZID.Click += new global::System.EventHandler(this.btnFindBySFZID_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.mnuPrivilegeTypeSet, this.editPrivilegeToolStripMenuItem, this.mnuPrivilegeTypeManage, this.createQRCodeToolStripMenuItem, this.toolStripSeparator1, this.toolStripMenuItem1, this.queryUsersWithoutPrivilegeToolStripMenuItem, this.displayAllToolStripMenuItem, this.toolStripSeparator2, this.saveLayoutToolStripMenuItem,
				this.restoreDefaultLayoutToolStripMenuItem, this.toolStripSeparator3, this.batchUpdateSelectToolStripMenuItem, this.importFromExcelToolStripMenuItem, this.deleteUserFromExcelToolStripMenuItem, this.deletedUserManageToolStripMenuItem, this.extendReaderSettingToolStripMenuItem, this.importFromExcelWithDateToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.mnuPrivilegeTypeSet, "mnuPrivilegeTypeSet");
			this.mnuPrivilegeTypeSet.Name = "mnuPrivilegeTypeSet";
			this.mnuPrivilegeTypeSet.Click += new global::System.EventHandler(this.mnuPrivilegeTypeSet_Click);
			componentResourceManager.ApplyResources(this.editPrivilegeToolStripMenuItem, "editPrivilegeToolStripMenuItem");
			this.editPrivilegeToolStripMenuItem.Name = "editPrivilegeToolStripMenuItem";
			this.editPrivilegeToolStripMenuItem.Click += new global::System.EventHandler(this.editPrivilegeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.mnuPrivilegeTypeManage, "mnuPrivilegeTypeManage");
			this.mnuPrivilegeTypeManage.Name = "mnuPrivilegeTypeManage";
			this.mnuPrivilegeTypeManage.Click += new global::System.EventHandler(this.mnuPrivilegeTypeManage_Click);
			componentResourceManager.ApplyResources(this.createQRCodeToolStripMenuItem, "createQRCodeToolStripMenuItem");
			this.createQRCodeToolStripMenuItem.Name = "createQRCodeToolStripMenuItem";
			this.createQRCodeToolStripMenuItem.Click += new global::System.EventHandler(this.createQRCodeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Click += new global::System.EventHandler(this.toolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.queryUsersWithoutPrivilegeToolStripMenuItem, "queryUsersWithoutPrivilegeToolStripMenuItem");
			this.queryUsersWithoutPrivilegeToolStripMenuItem.Name = "queryUsersWithoutPrivilegeToolStripMenuItem";
			this.queryUsersWithoutPrivilegeToolStripMenuItem.Click += new global::System.EventHandler(this.queryUsersWithoutPrivilegeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
			this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
			this.displayAllToolStripMenuItem.Click += new global::System.EventHandler(this.displayAllToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
			this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
			this.batchUpdateSelectToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.importFromExcelToolStripMenuItem, "importFromExcelToolStripMenuItem");
			this.importFromExcelToolStripMenuItem.Name = "importFromExcelToolStripMenuItem";
			this.importFromExcelToolStripMenuItem.Click += new global::System.EventHandler(this.btnImportFromExcel_Click);
			componentResourceManager.ApplyResources(this.deleteUserFromExcelToolStripMenuItem, "deleteUserFromExcelToolStripMenuItem");
			this.deleteUserFromExcelToolStripMenuItem.Name = "deleteUserFromExcelToolStripMenuItem";
			this.deleteUserFromExcelToolStripMenuItem.Click += new global::System.EventHandler(this.deleteUserFromExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.deletedUserManageToolStripMenuItem, "deletedUserManageToolStripMenuItem");
			this.deletedUserManageToolStripMenuItem.Name = "deletedUserManageToolStripMenuItem";
			this.deletedUserManageToolStripMenuItem.Click += new global::System.EventHandler(this.deletedUserManageToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.extendReaderSettingToolStripMenuItem, "extendReaderSettingToolStripMenuItem");
			this.extendReaderSettingToolStripMenuItem.Name = "extendReaderSettingToolStripMenuItem";
			this.extendReaderSettingToolStripMenuItem.Click += new global::System.EventHandler(this.extendReaderSettingToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.importFromExcelWithDateToolStripMenuItem, "importFromExcelWithDateToolStripMenuItem");
			this.importFromExcelWithDateToolStripMenuItem.Name = "importFromExcelWithDateToolStripMenuItem";
			this.importFromExcelWithDateToolStripMenuItem.Click += new global::System.EventHandler(this.importFromExcelWithDateToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvUsers);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			base.KeyPreview = true;
			base.Name = "frmUsers";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmUsers_FormClosed);
			base.Load += new global::System.EventHandler(this.frmUsers_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000B5D RID: 2909
		private global::System.Data.DataView dv;

		// Token: 0x04000B5E RID: 2910
		private global::System.Data.DataView dv4loadUserData;

		// Token: 0x04000B62 RID: 2914
		private global::System.Data.DataTable tb4loadUserData;

		// Token: 0x04000B66 RID: 2918
		public global::System.Windows.Forms.ToolStripButton btnFindBySFZID;

		// Token: 0x04000B67 RID: 2919
		public global::System.Windows.Forms.ToolStripButton btnRegisterLostCard;

		// Token: 0x04000B68 RID: 2920
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x04000B70 RID: 2928
		public global::WG3000_COMM.Core.WatchingService watching;

		// Token: 0x04000B71 RID: 2929
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000B72 RID: 2930
		private global::System.Windows.Forms.DataGridViewTextBoxColumn AccessTypeID;

		// Token: 0x04000B73 RID: 2931
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Attend;

		// Token: 0x04000B74 RID: 2932
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04000B75 RID: 2933
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04000B76 RID: 2934
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04000B77 RID: 2935
		private global::System.Windows.Forms.ToolStripButton btnAutoAdd;

		// Token: 0x04000B78 RID: 2936
		private global::System.Windows.Forms.ToolStripButton btnBatchUpdate;

		// Token: 0x04000B79 RID: 2937
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04000B7A RID: 2938
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04000B7B RID: 2939
		private global::System.Windows.Forms.ToolStripButton btnEditPrivilege;

		// Token: 0x04000B7C RID: 2940
		private global::System.Windows.Forms.ToolStripButton btnEditPrivilegeType;

		// Token: 0x04000B7D RID: 2941
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04000B7E RID: 2942
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04000B7F RID: 2943
		private global::System.Windows.Forms.ToolStripButton btnImportFromExcel;

		// Token: 0x04000B80 RID: 2944
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04000B81 RID: 2945
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04000B82 RID: 2946
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Cert;

		// Token: 0x04000B83 RID: 2947
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04000B84 RID: 2948
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04000B85 RID: 2949
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerNO;

		// Token: 0x04000B86 RID: 2950
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000B87 RID: 2951
		private global::System.Windows.Forms.ToolStripMenuItem createQRCodeToolStripMenuItem;

		// Token: 0x04000B88 RID: 2952
		private global::System.Windows.Forms.ToolStripMenuItem deletedUserManageToolStripMenuItem;

		// Token: 0x04000B89 RID: 2953
		private global::System.Windows.Forms.ToolStripMenuItem deleteUserFromExcelToolStripMenuItem;

		// Token: 0x04000B8A RID: 2954
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Deptname;

		// Token: 0x04000B8B RID: 2955
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04000B8C RID: 2956
		private global::System.Windows.Forms.ToolStripMenuItem displayAllToolStripMenuItem;

		// Token: 0x04000B8D RID: 2957
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Door;

		// Token: 0x04000B8E RID: 2958
		private global::System.Windows.Forms.ToolStripMenuItem editPrivilegeToolStripMenuItem;

		// Token: 0x04000B8F RID: 2959
		private global::System.Windows.Forms.DataGridViewTextBoxColumn End;

		// Token: 0x04000B90 RID: 2960
		private global::System.Windows.Forms.ToolStripMenuItem extendReaderSettingToolStripMenuItem;

		// Token: 0x04000B91 RID: 2961
		private global::System.Windows.Forms.ToolStripMenuItem importFromExcelToolStripMenuItem;

		// Token: 0x04000B92 RID: 2962
		private global::System.Windows.Forms.ToolStripMenuItem importFromExcelWithDateToolStripMenuItem;

		// Token: 0x04000B93 RID: 2963
		private global::System.Windows.Forms.ToolStripMenuItem mnuPrivilegeTypeManage;

		// Token: 0x04000B94 RID: 2964
		private global::System.Windows.Forms.ToolStripMenuItem mnuPrivilegeTypeSet;

		// Token: 0x04000B95 RID: 2965
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Mobile;

		// Token: 0x04000B96 RID: 2966
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04000B97 RID: 2967
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PrivilegeType;

		// Token: 0x04000B98 RID: 2968
		private global::System.Windows.Forms.ToolStripMenuItem queryUsersWithoutPrivilegeToolStripMenuItem;

		// Token: 0x04000B99 RID: 2969
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x04000B9A RID: 2970
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x04000B9B RID: 2971
		private global::System.Windows.Forms.DataGridViewTextBoxColumn SecondCard;

		// Token: 0x04000B9C RID: 2972
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Shift;

		// Token: 0x04000B9D RID: 2973
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Start;

		// Token: 0x04000B9E RID: 2974
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000B9F RID: 2975
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x04000BA0 RID: 2976
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000BA1 RID: 2977
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000BA2 RID: 2978
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x04000BA3 RID: 2979
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;
	}
}
