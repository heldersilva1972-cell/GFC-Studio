namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024D RID: 589
	public partial class frmUsers4Elevator : global::System.Windows.Forms.Form
	{
		// Token: 0x060012C5 RID: 4805 RVA: 0x0016A49A File Offset: 0x0016949A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0016A4BC File Offset: 0x001694BC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Elevator.frmUsers4Elevator));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
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
			this.floorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MoreFloor = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FloorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnBatchUpdate = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditPrivilege = new global::System.Windows.Forms.ToolStripButton();
			this.btnAutoAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.btnUpload = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.batchUpdateSelectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.batchUpdateToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportAllPrivilegeToExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dgvFloorPrivileges = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Department2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Device = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Floor = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFloorPrivileges).BeginInit();
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
				this.floorName, this.TimeProfile, this.MoreFloor, this.FloorID
			});
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsers.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUsers_CellFormatting);
			this.dgvUsers.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvUsers_Scroll);
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.dgvUsers_DoubleClick);
			this.dgvUsers.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
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
			componentResourceManager.ApplyResources(this.Deptname, "Deptname");
			this.Deptname.Name = "Deptname";
			this.Deptname.ReadOnly = true;
			this.floorName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.floorName, "floorName");
			this.floorName.Name = "floorName";
			this.floorName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TimeProfile, "TimeProfile");
			this.TimeProfile.Name = "TimeProfile";
			this.TimeProfile.ReadOnly = true;
			componentResourceManager.ApplyResources(this.MoreFloor, "MoreFloor");
			this.MoreFloor.Name = "MoreFloor";
			this.MoreFloor.ReadOnly = true;
			componentResourceManager.ApplyResources(this.FloorID, "FloorID");
			this.FloorID.Name = "FloorID";
			this.FloorID.ReadOnly = true;
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnBatchUpdate, this.btnEditPrivilege, this.btnAutoAdd, this.btnPrint, this.btnExport, this.btnUpload, this.btnFind, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
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
			componentResourceManager.ApplyResources(this.btnAutoAdd, "btnAutoAdd");
			this.btnAutoAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAutoAdd.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Auto;
			this.btnAutoAdd.Name = "btnAutoAdd";
			this.btnAutoAdd.Click += new global::System.EventHandler(this.btnAutoAdd_Click);
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
			componentResourceManager.ApplyResources(this.btnUpload, "btnUpload");
			this.btnUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnUpload.Image = global::WG3000_COMM.Properties.Resources.pConsole_Upload;
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Click += new global::System.EventHandler(this.btnUpload_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.btnExit.Name = "btnExit";
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.batchUpdateSelectToolStripMenuItem, this.batchUpdateToolStripMenuItem, this.exportAllPrivilegeToExcelToolStripMenuItem, this.toolStripSeparator2, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
			this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
			this.batchUpdateSelectToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.batchUpdateToolStripMenuItem, "batchUpdateToolStripMenuItem");
			this.batchUpdateToolStripMenuItem.Name = "batchUpdateToolStripMenuItem";
			this.batchUpdateToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportAllPrivilegeToExcelToolStripMenuItem, "exportAllPrivilegeToExcelToolStripMenuItem");
			this.exportAllPrivilegeToExcelToolStripMenuItem.Name = "exportAllPrivilegeToExcelToolStripMenuItem";
			this.exportAllPrivilegeToExcelToolStripMenuItem.Click += new global::System.EventHandler(this.exportAllPrivilegeToExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dgvFloorPrivileges, "dgvFloorPrivileges");
			this.dgvFloorPrivileges.AllowUserToAddRows = false;
			this.dgvFloorPrivileges.AllowUserToDeleteRows = false;
			this.dgvFloorPrivileges.AllowUserToOrderColumns = true;
			this.dgvFloorPrivileges.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvFloorPrivileges.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvFloorPrivileges.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.Department2, this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.Device, this.Floor });
			this.dgvFloorPrivileges.EnableHeadersVisualStyles = false;
			this.dgvFloorPrivileges.Name = "dgvFloorPrivileges";
			this.dgvFloorPrivileges.ReadOnly = true;
			this.dgvFloorPrivileges.RowTemplate.Height = 23;
			this.dgvFloorPrivileges.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Department2, "Department2");
			this.Department2.Name = "Department2";
			this.Department2.ReadOnly = true;
			this.dataGridViewTextBoxColumn8.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Device, "Device");
			this.Device.Name = "Device";
			this.Device.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Floor, "Floor");
			this.Floor.Name = "Floor";
			this.Floor.ReadOnly = true;
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvFloorPrivileges);
			base.Controls.Add(this.dgvUsers);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			base.KeyPreview = true;
			base.Name = "frmUsers4Elevator";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmUsers4Elevator_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmUsers_FormClosed);
			base.Load += new global::System.EventHandler(this.frmUsers_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmUsers_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvFloorPrivileges).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400222F RID: 8751
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002230 RID: 8752
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Attend;

		// Token: 0x04002231 RID: 8753
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002232 RID: 8754
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04002233 RID: 8755
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateToolStripMenuItem;

		// Token: 0x04002234 RID: 8756
		private global::System.Windows.Forms.ToolStripButton btnAutoAdd;

		// Token: 0x04002235 RID: 8757
		private global::System.Windows.Forms.ToolStripButton btnBatchUpdate;

		// Token: 0x04002236 RID: 8758
		private global::System.Windows.Forms.ToolStripButton btnEditPrivilege;

		// Token: 0x04002237 RID: 8759
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x04002238 RID: 8760
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04002239 RID: 8761
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x0400223A RID: 8762
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x0400223B RID: 8763
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x0400223C RID: 8764
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x0400223D RID: 8765
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x0400223E RID: 8766
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerNO;

		// Token: 0x0400223F RID: 8767
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002240 RID: 8768
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002241 RID: 8769
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002242 RID: 8770
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002243 RID: 8771
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04002244 RID: 8772
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04002245 RID: 8773
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Department2;

		// Token: 0x04002246 RID: 8774
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Deptname;

		// Token: 0x04002247 RID: 8775
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Device;

		// Token: 0x04002248 RID: 8776
		private global::System.Windows.Forms.DataGridView dgvFloorPrivileges;

		// Token: 0x04002249 RID: 8777
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x0400224A RID: 8778
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Door;

		// Token: 0x0400224B RID: 8779
		private global::System.Windows.Forms.DataGridViewTextBoxColumn End;

		// Token: 0x0400224C RID: 8780
		private global::System.Windows.Forms.ToolStripMenuItem exportAllPrivilegeToExcelToolStripMenuItem;

		// Token: 0x0400224D RID: 8781
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Floor;

		// Token: 0x0400224E RID: 8782
		private global::System.Windows.Forms.DataGridViewTextBoxColumn FloorID;

		// Token: 0x0400224F RID: 8783
		private global::System.Windows.Forms.DataGridViewTextBoxColumn floorName;

		// Token: 0x04002250 RID: 8784
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MoreFloor;

		// Token: 0x04002251 RID: 8785
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04002252 RID: 8786
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x04002253 RID: 8787
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x04002254 RID: 8788
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Shift;

		// Token: 0x04002255 RID: 8789
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Start;

		// Token: 0x04002256 RID: 8790
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;

		// Token: 0x04002257 RID: 8791
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04002258 RID: 8792
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04002259 RID: 8793
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;

		// Token: 0x0400225A RID: 8794
		public global::System.Windows.Forms.ToolStripButton btnUpload;
	}
}
