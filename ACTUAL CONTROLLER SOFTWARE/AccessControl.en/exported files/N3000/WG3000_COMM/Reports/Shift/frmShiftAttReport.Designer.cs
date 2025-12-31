namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200037B RID: 891
	public partial class frmShiftAttReport : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001D86 RID: 7558 RVA: 0x0027056E File Offset: 0x0026F56E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00270590 File Offset: 0x0026F590
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.frmShiftAttReport));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnSelectColumns = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryNormalShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdQueryOtherShift = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cmdCreateWithSomeConsumer = new global::System.Windows.Forms.ToolStripMenuItem();
			this.printPageBreakForEveryOneToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportOver65535ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftDateShort = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Addr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadTimes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty1Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty1Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty2Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty2Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty3Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty3Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty4Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty4Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LateMinutes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LeaveEarlyMinutes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OvertimeHours = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_AbsenceDays = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_NotReadCardCount = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.lblLog = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnStatistics = new global::System.Windows.Forms.ToolStripButton();
			this.btnCreateReport = new global::System.Windows.Forms.ToolStripButton();
			this.btnFindOption = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind4Shift();
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
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnSelectColumns, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem, this.cmdQueryNormalShift, this.cmdQueryOtherShift, this.cmdCreateWithSomeConsumer, this.printPageBreakForEveryOneToolStripMenuItem, this.displayAllToolStripMenuItem, this.exportOver65535ToolStripMenuItem });
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
			componentResourceManager.ApplyResources(this.cmdQueryNormalShift, "cmdQueryNormalShift");
			this.cmdQueryNormalShift.Name = "cmdQueryNormalShift";
			this.cmdQueryNormalShift.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.cmdQueryOtherShift, "cmdQueryOtherShift");
			this.cmdQueryOtherShift.Name = "cmdQueryOtherShift";
			this.cmdQueryOtherShift.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.cmdCreateWithSomeConsumer, "cmdCreateWithSomeConsumer");
			this.cmdCreateWithSomeConsumer.Name = "cmdCreateWithSomeConsumer";
			this.cmdCreateWithSomeConsumer.Click += new global::System.EventHandler(this.cmdCreateWithSomeConsumer_Click);
			componentResourceManager.ApplyResources(this.printPageBreakForEveryOneToolStripMenuItem, "printPageBreakForEveryOneToolStripMenuItem");
			this.printPageBreakForEveryOneToolStripMenuItem.Name = "printPageBreakForEveryOneToolStripMenuItem";
			this.printPageBreakForEveryOneToolStripMenuItem.Click += new global::System.EventHandler(this.printPageBreakForEveryOneToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
			this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
			this.displayAllToolStripMenuItem.Click += new global::System.EventHandler(this.displayAllToolStripMenuItem_Click);
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
				this.f_RecID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_ShiftDateShort, this.f_Addr, this.f_ReadTimes, this.f_OnDuty1Short, this.f_Desc1, this.f_OffDuty1Short,
				this.f_Desc2, this.f_OnDuty2Short, this.f_Desc3, this.f_OffDuty2Short, this.f_Desc4, this.f_OnDuty3Short, this.f_Desc5, this.f_OffDuty3Short, this.f_Desc6, this.f_OnDuty4Short,
				this.f_Desc7, this.f_OffDuty4Short, this.f_Desc8, this.f_LateMinutes, this.f_LeaveEarlyMinutes, this.f_OvertimeHours, this.f_AbsenceDays, this.f_NotReadCardCount
			});
			this.dgvMain.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.RowTemplate.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
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
			componentResourceManager.ApplyResources(this.f_Addr, "f_Addr");
			this.f_Addr.Name = "f_Addr";
			this.f_Addr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReadTimes, "f_ReadTimes");
			this.f_ReadTimes.Name = "f_ReadTimes";
			this.f_ReadTimes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty1Short, "f_OnDuty1Short");
			this.f_OnDuty1Short.Name = "f_OnDuty1Short";
			this.f_OnDuty1Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc1, "f_Desc1");
			this.f_Desc1.Name = "f_Desc1";
			this.f_Desc1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty1Short, "f_OffDuty1Short");
			this.f_OffDuty1Short.Name = "f_OffDuty1Short";
			this.f_OffDuty1Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc2, "f_Desc2");
			this.f_Desc2.Name = "f_Desc2";
			this.f_Desc2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty2Short, "f_OnDuty2Short");
			this.f_OnDuty2Short.Name = "f_OnDuty2Short";
			this.f_OnDuty2Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc3, "f_Desc3");
			this.f_Desc3.Name = "f_Desc3";
			this.f_Desc3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty2Short, "f_OffDuty2Short");
			this.f_OffDuty2Short.Name = "f_OffDuty2Short";
			this.f_OffDuty2Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc4, "f_Desc4");
			this.f_Desc4.Name = "f_Desc4";
			this.f_Desc4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty3Short, "f_OnDuty3Short");
			this.f_OnDuty3Short.Name = "f_OnDuty3Short";
			this.f_OnDuty3Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc5, "f_Desc5");
			this.f_Desc5.Name = "f_Desc5";
			this.f_Desc5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty3Short, "f_OffDuty3Short");
			this.f_OffDuty3Short.Name = "f_OffDuty3Short";
			this.f_OffDuty3Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc6, "f_Desc6");
			this.f_Desc6.Name = "f_Desc6";
			this.f_Desc6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty4Short, "f_OnDuty4Short");
			this.f_OnDuty4Short.Name = "f_OnDuty4Short";
			this.f_OnDuty4Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc7, "f_Desc7");
			this.f_Desc7.Name = "f_Desc7";
			this.f_Desc7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty4Short, "f_OffDuty4Short");
			this.f_OffDuty4Short.Name = "f_OffDuty4Short";
			this.f_OffDuty4Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc8, "f_Desc8");
			this.f_Desc8.Name = "f_Desc8";
			this.f_Desc8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LateMinutes, "f_LateMinutes");
			this.f_LateMinutes.Name = "f_LateMinutes";
			this.f_LateMinutes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LeaveEarlyMinutes, "f_LeaveEarlyMinutes");
			this.f_LeaveEarlyMinutes.Name = "f_LeaveEarlyMinutes";
			this.f_LeaveEarlyMinutes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OvertimeHours, "f_OvertimeHours");
			this.f_OvertimeHours.Name = "f_OvertimeHours";
			this.f_OvertimeHours.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_AbsenceDays, "f_AbsenceDays");
			this.f_AbsenceDays.Name = "f_AbsenceDays";
			this.f_AbsenceDays.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_NotReadCardCount, "f_NotReadCardCount");
			this.f_NotReadCardCount.Name = "f_NotReadCardCount";
			this.f_NotReadCardCount.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_third_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.lblLog });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.lblLog, "lblLog");
			this.lblLog.ForeColor = global::System.Drawing.Color.White;
			this.lblLog.Name = "lblLog";
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
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnPrint, this.btnExportToExcel, this.btnStatistics, this.btnCreateReport, this.btnFindOption, this.btnFind });
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
			componentResourceManager.ApplyResources(this.btnStatistics, "btnStatistics");
			this.btnStatistics.ForeColor = global::System.Drawing.Color.White;
			this.btnStatistics.Image = global::WG3000_COMM.Properties.Resources.pTools_StatisticsReport;
			this.btnStatistics.Name = "btnStatistics";
			this.btnStatistics.Click += new global::System.EventHandler(this.btnStatistics_Click);
			componentResourceManager.ApplyResources(this.btnCreateReport, "btnCreateReport");
			this.btnCreateReport.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateReport.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.btnCreateReport.Name = "btnCreateReport";
			this.btnCreateReport.Click += new global::System.EventHandler(this.btnCreateReport_Click);
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
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmShiftAttReport";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmSwipeRecords_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmShiftAttReport_KeyDown);
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

		// Token: 0x0400386F RID: 14447
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003870 RID: 14448
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04003871 RID: 14449
		private global::System.Windows.Forms.ToolStripButton btnCreateReport;

		// Token: 0x04003872 RID: 14450
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04003873 RID: 14451
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04003874 RID: 14452
		private global::System.Windows.Forms.ToolStripButton btnFindOption;

		// Token: 0x04003875 RID: 14453
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04003876 RID: 14454
		private global::System.Windows.Forms.ToolStripMenuItem btnSelectColumns;

		// Token: 0x04003877 RID: 14455
		private global::System.Windows.Forms.ToolStripButton btnStatistics;

		// Token: 0x04003878 RID: 14456
		private global::System.Windows.Forms.ToolStripMenuItem cmdCreateWithSomeConsumer;

		// Token: 0x04003879 RID: 14457
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryNormalShift;

		// Token: 0x0400387A RID: 14458
		private global::System.Windows.Forms.ToolStripMenuItem cmdQueryOtherShift;

		// Token: 0x0400387B RID: 14459
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400387C RID: 14460
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x0400387D RID: 14461
		private global::System.Windows.Forms.ToolStripMenuItem displayAllToolStripMenuItem;

		// Token: 0x04003880 RID: 14464
		private global::System.Windows.Forms.ToolStripMenuItem exportOver65535ToolStripMenuItem;

		// Token: 0x04003881 RID: 14465
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AbsenceDays;

		// Token: 0x04003882 RID: 14466
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Addr;

		// Token: 0x04003883 RID: 14467
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04003884 RID: 14468
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04003885 RID: 14469
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x04003886 RID: 14470
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc1;

		// Token: 0x04003887 RID: 14471
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc2;

		// Token: 0x04003888 RID: 14472
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc3;

		// Token: 0x04003889 RID: 14473
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc4;

		// Token: 0x0400388A RID: 14474
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc5;

		// Token: 0x0400388B RID: 14475
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc6;

		// Token: 0x0400388C RID: 14476
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc7;

		// Token: 0x0400388D RID: 14477
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc8;

		// Token: 0x0400388E RID: 14478
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LateMinutes;

		// Token: 0x0400388F RID: 14479
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LeaveEarlyMinutes;

		// Token: 0x04003890 RID: 14480
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NotReadCardCount;

		// Token: 0x04003891 RID: 14481
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty1Short;

		// Token: 0x04003892 RID: 14482
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty2Short;

		// Token: 0x04003893 RID: 14483
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty3Short;

		// Token: 0x04003894 RID: 14484
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty4Short;

		// Token: 0x04003895 RID: 14485
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty1Short;

		// Token: 0x04003896 RID: 14486
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty2Short;

		// Token: 0x04003897 RID: 14487
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty3Short;

		// Token: 0x04003898 RID: 14488
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty4Short;

		// Token: 0x04003899 RID: 14489
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OvertimeHours;

		// Token: 0x0400389A RID: 14490
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReadTimes;

		// Token: 0x0400389B RID: 14491
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x0400389C RID: 14492
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftDateShort;

		// Token: 0x0400389D RID: 14493
		private global::System.Windows.Forms.ToolStripLabel lblLog;

		// Token: 0x0400389E RID: 14494
		private global::System.Windows.Forms.ToolStripMenuItem printPageBreakForEveryOneToolStripMenuItem;

		// Token: 0x0400389F RID: 14495
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x040038A0 RID: 14496
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x040038A1 RID: 14497
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x040038A2 RID: 14498
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x040038A3 RID: 14499
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x040038A4 RID: 14500
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x040038A5 RID: 14501
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x040038A6 RID: 14502
		private global::WG3000_COMM.Core.UserControlFind4Shift userControlFind1;
	}
}
