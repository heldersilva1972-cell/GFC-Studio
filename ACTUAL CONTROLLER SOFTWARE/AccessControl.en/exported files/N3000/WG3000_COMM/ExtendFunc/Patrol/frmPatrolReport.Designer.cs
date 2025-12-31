namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030D RID: 781
	public partial class frmPatrolReport : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060017CA RID: 6090 RVA: 0x001F00EE File Offset: 0x001EF0EE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x001F0110 File Offset: 0x001EF110
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.frmPatrolReport));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnSelectColumns = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdCreateWithSomeConsumer = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftDateShort = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty1Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty1Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.lblLog = new global::System.Windows.Forms.ToolStripLabel();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnPatrolSetup = new global::System.Windows.Forms.ToolStripButton();
			this.btnPatrolRoute = new global::System.Windows.Forms.ToolStripButton();
			this.btnPatrolTask = new global::System.Windows.Forms.ToolStripButton();
			this.btnCreateReport = new global::System.Windows.Forms.ToolStripButton();
			this.btnStatistics = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFindOption = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnSelectColumns, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem, this.cmdCreateWithSomeConsumer });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.btnSelectColumns, "btnSelectColumns");
			this.btnSelectColumns.Name = "btnSelectColumns";
			this.btnSelectColumns.Click += new global::System.EventHandler(this.btnSelectColumns_Click);
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.cmdCreateWithSomeConsumer, "cmdCreateWithSomeConsumer");
			this.cmdCreateWithSomeConsumer.Name = "cmdCreateWithSomeConsumer";
			this.cmdCreateWithSomeConsumer.Click += new global::System.EventHandler(this.cmdCreateWithSomeConsumer_Click);
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.AllowUserToOrderColumns = true;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_RecID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_ShiftDateShort, this.f_OnDuty1Short, this.f_OffDuty1Short, this.f_Desc1, this.f_Desc2, this.f_Desc3 });
			this.dgvMain.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
			this.f_DepartmentName.Name = "f_DepartmentName";
			this.f_DepartmentName.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
			this.f_ConsumerNO.Name = "f_ConsumerNO";
			this.f_ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftDateShort, "f_ShiftDateShort");
			this.f_ShiftDateShort.Name = "f_ShiftDateShort";
			this.f_ShiftDateShort.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty1Short, "f_OnDuty1Short");
			this.f_OnDuty1Short.Name = "f_OnDuty1Short";
			this.f_OnDuty1Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty1Short, "f_OffDuty1Short");
			this.f_OffDuty1Short.Name = "f_OffDuty1Short";
			this.f_OffDuty1Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc1, "f_Desc1");
			this.f_Desc1.Name = "f_Desc1";
			this.f_Desc1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc2, "f_Desc2");
			this.f_Desc2.Name = "f_Desc2";
			this.f_Desc2.ReadOnly = true;
			this.f_Desc3.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Desc3, "f_Desc3");
			this.f_Desc3.Name = "f_Desc3";
			this.f_Desc3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_third_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.lblLog });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.lblLog, "lblLog");
			this.lblLog.ForeColor = global::System.Drawing.Color.White;
			this.lblLog.Name = "lblLog";
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this.toolStrip3, "toolStrip3");
			this.toolStrip3.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip3.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel2, this.toolStripLabel3 });
			this.toolStrip3.Name = "toolStrip3";
			componentResourceManager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
			this.toolStripLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel2.Name = "toolStripLabel2";
			componentResourceManager.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
			this.toolStripLabel3.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel3.Name = "toolStripLabel3";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnPatrolSetup, this.btnPatrolRoute, this.btnPatrolTask, this.btnCreateReport, this.btnStatistics, this.btnPrint, this.btnExportToExcel, this.btnFindOption, this.btnFind, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnPatrolSetup, "btnPatrolSetup");
			this.btnPatrolSetup.ForeColor = global::System.Drawing.Color.White;
			this.btnPatrolSetup.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnPatrolSetup.Name = "btnPatrolSetup";
			this.btnPatrolSetup.Click += new global::System.EventHandler(this.btnPatrolSetup_Click);
			componentResourceManager.ApplyResources(this.btnPatrolRoute, "btnPatrolRoute");
			this.btnPatrolRoute.ForeColor = global::System.Drawing.Color.White;
			this.btnPatrolRoute.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit_Batch;
			this.btnPatrolRoute.Name = "btnPatrolRoute";
			this.btnPatrolRoute.Click += new global::System.EventHandler(this.btnPatrolRoute_Click);
			componentResourceManager.ApplyResources(this.btnPatrolTask, "btnPatrolTask");
			this.btnPatrolTask.ForeColor = global::System.Drawing.Color.White;
			this.btnPatrolTask.Image = global::WG3000_COMM.Properties.Resources.pTools_EditPrivielge;
			this.btnPatrolTask.Name = "btnPatrolTask";
			this.btnPatrolTask.Click += new global::System.EventHandler(this.btnPatrolTask_Click);
			componentResourceManager.ApplyResources(this.btnCreateReport, "btnCreateReport");
			this.btnCreateReport.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateReport.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.btnCreateReport.Name = "btnCreateReport";
			this.btnCreateReport.Click += new global::System.EventHandler(this.btnCreateReport_Click);
			componentResourceManager.ApplyResources(this.btnStatistics, "btnStatistics");
			this.btnStatistics.ForeColor = global::System.Drawing.Color.White;
			this.btnStatistics.Image = global::WG3000_COMM.Properties.Resources.pTools_StatisticsReport;
			this.btnStatistics.Name = "btnStatistics";
			this.btnStatistics.Click += new global::System.EventHandler(this.btnStatistics_Click);
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
			componentResourceManager.ApplyResources(this.btnFindOption, "btnFindOption");
			this.btnFindOption.ForeColor = global::System.Drawing.Color.White;
			this.btnFindOption.Image = global::WG3000_COMM.Properties.Resources.pTools_QueryOption;
			this.btnFindOption.Name = "btnFindOption";
			this.btnFindOption.Click += new global::System.EventHandler(this.btnFindOption_Click);
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
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmPatrolReport";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmSwipeRecords_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040030DF RID: 12511
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040030E0 RID: 12512
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040030E1 RID: 12513
		private global::System.Windows.Forms.ToolStripButton btnCreateReport;

		// Token: 0x040030E2 RID: 12514
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x040030E3 RID: 12515
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x040030E4 RID: 12516
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x040030E5 RID: 12517
		private global::System.Windows.Forms.ToolStripButton btnFindOption;

		// Token: 0x040030E6 RID: 12518
		private global::System.Windows.Forms.ToolStripButton btnPatrolRoute;

		// Token: 0x040030E7 RID: 12519
		private global::System.Windows.Forms.ToolStripButton btnPatrolSetup;

		// Token: 0x040030E8 RID: 12520
		private global::System.Windows.Forms.ToolStripButton btnPatrolTask;

		// Token: 0x040030E9 RID: 12521
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x040030EA RID: 12522
		private global::System.Windows.Forms.ToolStripMenuItem btnSelectColumns;

		// Token: 0x040030EB RID: 12523
		private global::System.Windows.Forms.ToolStripButton btnStatistics;

		// Token: 0x040030EC RID: 12524
		private global::System.Windows.Forms.ToolStripMenuItem cmdCreateWithSomeConsumer;

		// Token: 0x040030ED RID: 12525
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040030EE RID: 12526
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040030F1 RID: 12529
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x040030F2 RID: 12530
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x040030F3 RID: 12531
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x040030F4 RID: 12532
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc1;

		// Token: 0x040030F5 RID: 12533
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc2;

		// Token: 0x040030F6 RID: 12534
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc3;

		// Token: 0x040030F7 RID: 12535
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty1Short;

		// Token: 0x040030F8 RID: 12536
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty1Short;

		// Token: 0x040030F9 RID: 12537
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x040030FA RID: 12538
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftDateShort;

		// Token: 0x040030FB RID: 12539
		private global::System.Windows.Forms.ToolStripLabel lblLog;

		// Token: 0x040030FC RID: 12540
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x040030FD RID: 12541
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x040030FE RID: 12542
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x040030FF RID: 12543
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04003100 RID: 12544
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x04003101 RID: 12545
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04003102 RID: 12546
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x04003103 RID: 12547
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;
	}
}
