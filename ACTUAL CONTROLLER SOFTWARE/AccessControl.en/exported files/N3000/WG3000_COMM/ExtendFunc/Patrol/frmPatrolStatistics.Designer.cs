namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000310 RID: 784
	public partial class frmPatrolStatistics : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060017F5 RID: 6133 RVA: 0x001F29DA File Offset: 0x001F19DA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x001F29FC File Offset: 0x001F19FC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.frmPatrolStatistics));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DayRealWork = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LeaveEarlyCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LateCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_AbsenceDays = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ManualReadTimesCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryNormalShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryOtherShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.lblLog = new global::System.Windows.Forms.ToolStripLabel();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFindSecond();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
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
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_RecID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_DayRealWork, this.f_LeaveEarlyCount, this.f_LateCount, this.f_AbsenceDays, this.f_ManualReadTimesCount });
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
			componentResourceManager.ApplyResources(this.f_DayRealWork, "f_DayRealWork");
			this.f_DayRealWork.Name = "f_DayRealWork";
			this.f_DayRealWork.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LeaveEarlyCount, "f_LeaveEarlyCount");
			this.f_LeaveEarlyCount.Name = "f_LeaveEarlyCount";
			this.f_LeaveEarlyCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LateCount, "f_LateCount");
			this.f_LateCount.Name = "f_LateCount";
			this.f_LateCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_AbsenceDays, "f_AbsenceDays");
			this.f_AbsenceDays.Name = "f_AbsenceDays";
			this.f_AbsenceDays.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ManualReadTimesCount, "f_ManualReadTimesCount");
			this.f_ManualReadTimesCount.Name = "f_ManualReadTimesCount";
			this.f_ManualReadTimesCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem, this.cmdQueryNormalShift, this.cmdQueryOtherShift });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
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
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnPrint, this.btnExportToExcel, this.btnExit });
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
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmPatrolStatistics";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmShiftAttStatistics_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400311C RID: 12572
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400311D RID: 12573
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x0400311E RID: 12574
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x0400311F RID: 12575
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04003120 RID: 12576
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04003121 RID: 12577
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryNormalShift;

		// Token: 0x04003122 RID: 12578
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryOtherShift;

		// Token: 0x04003123 RID: 12579
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04003124 RID: 12580
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04003125 RID: 12581
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AbsenceDays;

		// Token: 0x04003126 RID: 12582
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04003127 RID: 12583
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04003128 RID: 12584
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DayRealWork;

		// Token: 0x04003129 RID: 12585
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x0400312A RID: 12586
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LateCount;

		// Token: 0x0400312B RID: 12587
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LeaveEarlyCount;

		// Token: 0x0400312C RID: 12588
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ManualReadTimesCount;

		// Token: 0x0400312D RID: 12589
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x0400312E RID: 12590
		private global::System.Windows.Forms.ToolStripLabel lblLog;

		// Token: 0x0400312F RID: 12591
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x04003130 RID: 12592
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x04003131 RID: 12593
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04003132 RID: 12594
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04003133 RID: 12595
		private global::WG3000_COMM.Core.UserControlFindSecond userControlFind1;
	}
}
