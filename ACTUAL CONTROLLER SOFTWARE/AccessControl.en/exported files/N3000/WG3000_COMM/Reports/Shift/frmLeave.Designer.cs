namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000377 RID: 887
	public partial class frmLeave : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001D36 RID: 7478 RVA: 0x0026A902 File Offset: 0x00269902
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0026A924 File Offset: 0x00269924
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.frmLeave));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_NO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_from = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.From1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_to = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.To2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_HolidayType = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind4Shift();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnTypeSetup = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_NO, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_from, this.From1, this.f_to, this.To2, this.f_HolidayType, this.f_Notes });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowHeadersVisible = false;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMain_CellFormatting);
			this.dgvMain.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_NO.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_NO, "f_NO");
			this.f_NO.Name = "f_NO";
			this.f_NO.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.f_from, "f_from");
			this.f_from.Name = "f_from";
			this.f_from.ReadOnly = true;
			componentResourceManager.ApplyResources(this.From1, "From1");
			this.From1.Name = "From1";
			this.From1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_to, "f_to");
			this.f_to.Name = "f_to";
			this.f_to.ReadOnly = true;
			componentResourceManager.ApplyResources(this.To2, "To2");
			this.To2.Name = "To2";
			this.To2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_HolidayType, "f_HolidayType");
			this.f_HolidayType.Name = "f_HolidayType";
			this.f_HolidayType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
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
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnTypeSetup, this.btnPrint, this.btnExportToExcel, this.btnFind });
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
			componentResourceManager.ApplyResources(this.btnTypeSetup, "btnTypeSetup");
			this.btnTypeSetup.ForeColor = global::System.Drawing.Color.White;
			this.btnTypeSetup.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnTypeSetup.Name = "btnTypeSetup";
			this.btnTypeSetup.Click += new global::System.EventHandler(this.btnTypeSetup_Click);
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
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmLeave";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmLeave_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003827 RID: 14375
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003828 RID: 14376
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04003829 RID: 14377
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x0400382A RID: 14378
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x0400382B RID: 14379
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x0400382C RID: 14380
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x0400382D RID: 14381
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x0400382E RID: 14382
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x0400382F RID: 14383
		private global::System.Windows.Forms.ToolStripButton btnTypeSetup;

		// Token: 0x04003830 RID: 14384
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04003833 RID: 14387
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04003834 RID: 14388
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04003835 RID: 14389
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x04003836 RID: 14390
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_from;

		// Token: 0x04003837 RID: 14391
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_HolidayType;

		// Token: 0x04003838 RID: 14392
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NO;

		// Token: 0x04003839 RID: 14393
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x0400383A RID: 14394
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_to;

		// Token: 0x0400383B RID: 14395
		private global::System.Windows.Forms.DataGridViewTextBoxColumn From1;

		// Token: 0x0400383C RID: 14396
		private global::System.Windows.Forms.DataGridViewTextBoxColumn To2;

		// Token: 0x0400383D RID: 14397
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x0400383E RID: 14398
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x0400383F RID: 14399
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04003840 RID: 14400
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x04003841 RID: 14401
		private global::WG3000_COMM.Core.UserControlFind4Shift userControlFind1;
	}
}
