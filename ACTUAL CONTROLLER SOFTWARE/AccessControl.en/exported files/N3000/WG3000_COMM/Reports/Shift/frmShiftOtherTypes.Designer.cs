namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000381 RID: 897
	public partial class frmShiftOtherTypes : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001DEA RID: 7658 RVA: 0x00277B7E File Offset: 0x00276B7E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00277BA0 File Offset: 0x00276BA0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.frmShiftOtherTypes));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_ShiftID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadTimes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_bOvertimeShift = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_OnDuty1t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty1t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty2t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty2t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty3t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty3t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty4t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OffDuty4t = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip2.SuspendLayout();
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
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_ShiftID, this.f_ShiftName, this.f_ReadTimes, this.f_bOvertimeShift, this.f_OnDuty1t, this.f_OffDuty1t, this.f_OnDuty2t, this.f_OffDuty2t, this.f_OnDuty3t, this.f_OffDuty3t,
				this.f_OnDuty4t, this.f_OffDuty4t
			});
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.DoubleClick += new global::System.EventHandler(this.dgvControlSegs_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ShiftID, "f_ShiftID");
			this.f_ShiftID.Name = "f_ShiftID";
			this.f_ShiftID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ShiftName.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_ShiftName, "f_ShiftName");
			this.f_ShiftName.Name = "f_ShiftName";
			this.f_ShiftName.ReadOnly = true;
			this.f_ShiftName.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_ShiftName.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_ReadTimes, "f_ReadTimes");
			this.f_ReadTimes.Name = "f_ReadTimes";
			this.f_ReadTimes.ReadOnly = true;
			this.f_ReadTimes.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_ReadTimes.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_bOvertimeShift, "f_bOvertimeShift");
			this.f_bOvertimeShift.Name = "f_bOvertimeShift";
			this.f_bOvertimeShift.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_OnDuty1t.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_OnDuty1t, "f_OnDuty1t");
			this.f_OnDuty1t.Name = "f_OnDuty1t";
			this.f_OnDuty1t.ReadOnly = true;
			this.f_OnDuty1t.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_OnDuty1t.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_OffDuty1t, "f_OffDuty1t");
			this.f_OffDuty1t.Name = "f_OffDuty1t";
			this.f_OffDuty1t.ReadOnly = true;
			this.f_OffDuty1t.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_OffDuty1t.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_OnDuty2t.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_OnDuty2t, "f_OnDuty2t");
			this.f_OnDuty2t.Name = "f_OnDuty2t";
			this.f_OnDuty2t.ReadOnly = true;
			this.f_OnDuty2t.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_OnDuty2t.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_OffDuty2t, "f_OffDuty2t");
			this.f_OffDuty2t.Name = "f_OffDuty2t";
			this.f_OffDuty2t.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty3t, "f_OnDuty3t");
			this.f_OnDuty3t.Name = "f_OnDuty3t";
			this.f_OnDuty3t.ReadOnly = true;
			this.f_OnDuty3t.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_OnDuty3t.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_OffDuty3t, "f_OffDuty3t");
			this.f_OffDuty3t.Name = "f_OffDuty3t";
			this.f_OffDuty3t.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty4t, "f_OnDuty4t");
			this.f_OnDuty4t.Name = "f_OnDuty4t";
			this.f_OnDuty4t.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OffDuty4t, "f_OffDuty4t");
			this.f_OffDuty4t.Name = "f_OffDuty4t";
			this.f_OffDuty4t.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel1 });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExportToExcel, this.btnFind });
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
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmShiftOtherTypes";
			base.Load += new global::System.EventHandler(this.frmShiftOtherTypes_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400391E RID: 14622
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400391F RID: 14623
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04003920 RID: 14624
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04003921 RID: 14625
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04003922 RID: 14626
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04003923 RID: 14627
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04003924 RID: 14628
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04003925 RID: 14629
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04003926 RID: 14630
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_bOvertimeShift;

		// Token: 0x04003927 RID: 14631
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty1t;

		// Token: 0x04003928 RID: 14632
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty2t;

		// Token: 0x04003929 RID: 14633
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty3t;

		// Token: 0x0400392A RID: 14634
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OffDuty4t;

		// Token: 0x0400392B RID: 14635
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty1t;

		// Token: 0x0400392C RID: 14636
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty2t;

		// Token: 0x0400392D RID: 14637
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty3t;

		// Token: 0x0400392E RID: 14638
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty4t;

		// Token: 0x0400392F RID: 14639
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReadTimes;

		// Token: 0x04003930 RID: 14640
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID;

		// Token: 0x04003931 RID: 14641
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftName;

		// Token: 0x04003932 RID: 14642
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04003933 RID: 14643
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04003934 RID: 14644
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel1;
	}
}
