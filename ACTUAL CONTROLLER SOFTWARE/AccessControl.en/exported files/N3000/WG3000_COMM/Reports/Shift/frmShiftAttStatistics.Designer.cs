namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200037D RID: 893
	public partial class frmShiftAttStatistics : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001DB5 RID: 7605 RVA: 0x002742C8 File Offset: 0x002732C8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x002742E8 File Offset: 0x002732E8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.frmShiftAttStatistics));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.columnsConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.columnsSortToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryNormalShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryOtherShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportOver65535WithRecIDToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.editPrintersFooterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportOver65535ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DayShouldWork = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DayRealWork = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LateMinutes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LateCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LeaveEarlyMinutes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LeaveEarlyCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OvertimeHours = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_AbsenceDays = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_NotReadCardCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ManualReadTimesCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.lblLog = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFindSecond4Shift();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.columnsConfigureToolStripMenuItem, this.columnsSortToolStripMenuItem, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem, this.cmdQueryNormalShift, this.cmdQueryOtherShift, this.exportOver65535WithRecIDToolStripMenuItem, this.displayAllToolStripMenuItem, this.editPrintersFooterToolStripMenuItem, this.exportOver65535ToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.columnsConfigureToolStripMenuItem, "columnsConfigureToolStripMenuItem");
			this.columnsConfigureToolStripMenuItem.Name = "columnsConfigureToolStripMenuItem";
			this.columnsConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.columnsConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.columnsSortToolStripMenuItem, "columnsSortToolStripMenuItem");
			this.columnsSortToolStripMenuItem.Name = "columnsSortToolStripMenuItem";
			this.columnsSortToolStripMenuItem.Click += new global::System.EventHandler(this.columnsSortToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.cmdQueryNormalShift, "cmdQueryNormalShift");
			this.cmdQueryNormalShift.Name = "cmdQueryNormalShift";
			this.cmdQueryNormalShift.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.cmdQueryOtherShift, "cmdQueryOtherShift");
			this.cmdQueryOtherShift.Name = "cmdQueryOtherShift";
			this.cmdQueryOtherShift.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.exportOver65535WithRecIDToolStripMenuItem, "exportOver65535WithRecIDToolStripMenuItem");
			this.exportOver65535WithRecIDToolStripMenuItem.Name = "exportOver65535WithRecIDToolStripMenuItem";
			this.exportOver65535WithRecIDToolStripMenuItem.Click += new global::System.EventHandler(this.exportOver65535WithRecIDToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
			this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
			this.displayAllToolStripMenuItem.Click += new global::System.EventHandler(this.displayAllToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.editPrintersFooterToolStripMenuItem, "editPrintersFooterToolStripMenuItem");
			this.editPrintersFooterToolStripMenuItem.Name = "editPrintersFooterToolStripMenuItem";
			this.editPrintersFooterToolStripMenuItem.Click += new global::System.EventHandler(this.editPrintersFooterToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportOver65535ToolStripMenuItem, "exportOver65535ToolStripMenuItem");
			this.exportOver65535ToolStripMenuItem.Name = "exportOver65535ToolStripMenuItem";
			this.exportOver65535ToolStripMenuItem.Click += new global::System.EventHandler(this.exportOver65535ToolStripMenuItem_Click);
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
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_RecID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_DayShouldWork, this.f_DayRealWork, this.f_LateMinutes, this.f_LateCount, this.f_LeaveEarlyMinutes, this.f_LeaveEarlyCount,
				this.f_OvertimeHours, this.f_AbsenceDays, this.f_NotReadCardCount, this.f_ManualReadTimesCount
			});
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
			dataGridViewCellStyle4.NullValue = null;
			this.f_DayShouldWork.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_DayShouldWork, "f_DayShouldWork");
			this.f_DayShouldWork.Name = "f_DayShouldWork";
			this.f_DayShouldWork.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DayRealWork, "f_DayRealWork");
			this.f_DayRealWork.Name = "f_DayRealWork";
			this.f_DayRealWork.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LateMinutes, "f_LateMinutes");
			this.f_LateMinutes.Name = "f_LateMinutes";
			this.f_LateMinutes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LateCount, "f_LateCount");
			this.f_LateCount.Name = "f_LateCount";
			this.f_LateCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LeaveEarlyMinutes, "f_LeaveEarlyMinutes");
			this.f_LeaveEarlyMinutes.Name = "f_LeaveEarlyMinutes";
			this.f_LeaveEarlyMinutes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LeaveEarlyCount, "f_LeaveEarlyCount");
			this.f_LeaveEarlyCount.Name = "f_LeaveEarlyCount";
			this.f_LeaveEarlyCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OvertimeHours, "f_OvertimeHours");
			this.f_OvertimeHours.Name = "f_OvertimeHours";
			this.f_OvertimeHours.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_AbsenceDays, "f_AbsenceDays");
			this.f_AbsenceDays.Name = "f_AbsenceDays";
			this.f_AbsenceDays.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_NotReadCardCount, "f_NotReadCardCount");
			this.f_NotReadCardCount.Name = "f_NotReadCardCount";
			this.f_NotReadCardCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ManualReadTimesCount, "f_ManualReadTimesCount");
			this.f_ManualReadTimesCount.Name = "f_ManualReadTimesCount";
			this.f_ManualReadTimesCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_third_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.lblLog });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.lblLog, "lblLog");
			this.lblLog.ForeColor = global::System.Drawing.Color.White;
			this.lblLog.Name = "lblLog";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnPrint, this.btnExportToExcel, this.btnFind, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
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
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
			this.userControlFind1.Name = "userControlFind1";
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmShiftAttStatistics";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmShiftAttStatistics_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040038B8 RID: 14520
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040038B9 RID: 14521
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040038BA RID: 14522
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x040038BB RID: 14523
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x040038BC RID: 14524
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x040038BD RID: 14525
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x040038BE RID: 14526
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryNormalShift;

		// Token: 0x040038BF RID: 14527
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryOtherShift;

		// Token: 0x040038C0 RID: 14528
		private global::System.Windows.Forms.ToolStripMenuItem columnsConfigureToolStripMenuItem;

		// Token: 0x040038C1 RID: 14529
		private global::System.Windows.Forms.ToolStripMenuItem columnsSortToolStripMenuItem;

		// Token: 0x040038C2 RID: 14530
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040038C4 RID: 14532
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040038C5 RID: 14533
		private global::System.Windows.Forms.ToolStripMenuItem displayAllToolStripMenuItem;

		// Token: 0x040038C6 RID: 14534
		private global::System.Windows.Forms.ToolStripMenuItem editPrintersFooterToolStripMenuItem;

		// Token: 0x040038C7 RID: 14535
		private global::System.Windows.Forms.ToolStripMenuItem exportOver65535ToolStripMenuItem;

		// Token: 0x040038C8 RID: 14536
		private global::System.Windows.Forms.ToolStripMenuItem exportOver65535WithRecIDToolStripMenuItem;

		// Token: 0x040038C9 RID: 14537
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AbsenceDays;

		// Token: 0x040038CA RID: 14538
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x040038CB RID: 14539
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x040038CC RID: 14540
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DayRealWork;

		// Token: 0x040038CD RID: 14541
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DayShouldWork;

		// Token: 0x040038CE RID: 14542
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x040038CF RID: 14543
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LateCount;

		// Token: 0x040038D0 RID: 14544
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LateMinutes;

		// Token: 0x040038D1 RID: 14545
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LeaveEarlyCount;

		// Token: 0x040038D2 RID: 14546
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LeaveEarlyMinutes;

		// Token: 0x040038D3 RID: 14547
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ManualReadTimesCount;

		// Token: 0x040038D4 RID: 14548
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NotReadCardCount;

		// Token: 0x040038D5 RID: 14549
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OvertimeHours;

		// Token: 0x040038D6 RID: 14550
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x040038D7 RID: 14551
		private global::System.Windows.Forms.ToolStripLabel lblLog;

		// Token: 0x040038D8 RID: 14552
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x040038D9 RID: 14553
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x040038DA RID: 14554
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040038DB RID: 14555
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x040038DC RID: 14556
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x040038DD RID: 14557
		private global::WG3000_COMM.Core.UserControlFindSecond4Shift userControlFind1;
	}
}
