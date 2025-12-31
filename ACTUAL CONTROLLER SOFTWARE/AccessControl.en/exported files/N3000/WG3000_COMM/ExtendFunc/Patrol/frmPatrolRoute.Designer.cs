namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030F RID: 783
	public partial class frmPatrolRoute : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060017DF RID: 6111 RVA: 0x001F1622 File Offset: 0x001F0622
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x001F1644 File Offset: 0x001F0644
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.frmPatrolRoute));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_ShiftID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadTimes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
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
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ShiftID, this.f_ReadTimes });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.DoubleClick += new global::System.EventHandler(this.dgvControlSegs_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ShiftID, "f_ShiftID");
			this.f_ShiftID.Name = "f_ShiftID";
			this.f_ShiftID.ReadOnly = true;
			this.f_ReadTimes.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_ReadTimes, "f_ReadTimes");
			this.f_ReadTimes.Name = "f_ReadTimes";
			this.f_ReadTimes.ReadOnly = true;
			this.f_ReadTimes.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_ReadTimes.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExportToExcel, this.btnExit });
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
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.btnExit.Name = "btnExit";
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmPatrolRoute";
			base.Load += new global::System.EventHandler(this.frmShiftOtherTypes_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003107 RID: 12551
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003108 RID: 12552
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04003109 RID: 12553
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x0400310A RID: 12554
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x0400310B RID: 12555
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x0400310C RID: 12556
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x0400310D RID: 12557
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x0400310E RID: 12558
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x0400310F RID: 12559
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReadTimes;

		// Token: 0x04003110 RID: 12560
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID;

		// Token: 0x04003111 RID: 12561
		private global::System.Windows.Forms.ToolStrip toolStrip1;
	}
}
