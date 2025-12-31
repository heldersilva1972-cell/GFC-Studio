namespace WG3000_COMM.Basic
{
	// Token: 0x0200004E RID: 78
	public partial class frmControlSegs : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x0009DB38 File Offset: 0x0009CB38
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0009DB58 File Offset: 0x0009CB58
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmControlSegs));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvControlSegs = new global::System.Windows.Forms.DataGridView();
			this.f_ControlSegIDBak = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Monday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Tuesday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Wednesday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Thursday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Friday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Saturday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Sunday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_BeginHMS1A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EndHMS1A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_BeginHMS2A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EndHMS2A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_BeginHMS3A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EndHMS3A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegIDLinked = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_BeginYMD = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EndYMD = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlByHoliday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnHolidayControl = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvControlSegs).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dgvControlSegs, "dgvControlSegs");
			this.dgvControlSegs.AllowUserToAddRows = false;
			this.dgvControlSegs.AllowUserToDeleteRows = false;
			this.dgvControlSegs.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvControlSegs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvControlSegs.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvControlSegs.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_ControlSegIDBak, this.f_ControlSegID, this.f_Monday, this.f_Tuesday, this.f_Wednesday, this.f_Thursday, this.f_Friday, this.f_Saturday, this.f_Sunday, this.f_BeginHMS1A,
				this.f_EndHMS1A, this.f_BeginHMS2A, this.f_EndHMS2A, this.f_BeginHMS3A, this.f_EndHMS3A, this.f_ControlSegIDLinked, this.f_BeginYMD, this.f_EndYMD, this.f_ControlByHoliday
			});
			this.dgvControlSegs.EnableHeadersVisualStyles = false;
			this.dgvControlSegs.Name = "dgvControlSegs";
			this.dgvControlSegs.ReadOnly = true;
			this.dgvControlSegs.RowTemplate.Height = 23;
			this.dgvControlSegs.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvControlSegs.DoubleClick += new global::System.EventHandler(this.dgvControlSegs_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ControlSegIDBak, "f_ControlSegIDBak");
			this.f_ControlSegIDBak.Name = "f_ControlSegIDBak";
			this.f_ControlSegIDBak.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlSegID, "f_ControlSegID");
			this.f_ControlSegID.Name = "f_ControlSegID";
			this.f_ControlSegID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Monday, "f_Monday");
			this.f_Monday.Name = "f_Monday";
			this.f_Monday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Tuesday, "f_Tuesday");
			this.f_Tuesday.Name = "f_Tuesday";
			this.f_Tuesday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Wednesday, "f_Wednesday");
			this.f_Wednesday.Name = "f_Wednesday";
			this.f_Wednesday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Thursday, "f_Thursday");
			this.f_Thursday.Name = "f_Thursday";
			this.f_Thursday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Friday, "f_Friday");
			this.f_Friday.Name = "f_Friday";
			this.f_Friday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Saturday, "f_Saturday");
			this.f_Saturday.Name = "f_Saturday";
			this.f_Saturday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Sunday, "f_Sunday");
			this.f_Sunday.Name = "f_Sunday";
			this.f_Sunday.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_BeginHMS1A.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_BeginHMS1A, "f_BeginHMS1A");
			this.f_BeginHMS1A.Name = "f_BeginHMS1A";
			this.f_BeginHMS1A.ReadOnly = true;
			this.f_BeginHMS1A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_BeginHMS1A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_EndHMS1A, "f_EndHMS1A");
			this.f_EndHMS1A.Name = "f_EndHMS1A";
			this.f_EndHMS1A.ReadOnly = true;
			this.f_EndHMS1A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_EndHMS1A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_BeginHMS2A.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_BeginHMS2A, "f_BeginHMS2A");
			this.f_BeginHMS2A.Name = "f_BeginHMS2A";
			this.f_BeginHMS2A.ReadOnly = true;
			this.f_BeginHMS2A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_BeginHMS2A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_EndHMS2A, "f_EndHMS2A");
			this.f_EndHMS2A.Name = "f_EndHMS2A";
			this.f_EndHMS2A.ReadOnly = true;
			this.f_EndHMS2A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_EndHMS2A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_BeginHMS3A.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_BeginHMS3A, "f_BeginHMS3A");
			this.f_BeginHMS3A.Name = "f_BeginHMS3A";
			this.f_BeginHMS3A.ReadOnly = true;
			this.f_BeginHMS3A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_BeginHMS3A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_EndHMS3A, "f_EndHMS3A");
			this.f_EndHMS3A.Name = "f_EndHMS3A";
			this.f_EndHMS3A.ReadOnly = true;
			this.f_EndHMS3A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_EndHMS3A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_ControlSegIDLinked, "f_ControlSegIDLinked");
			this.f_ControlSegIDLinked.Name = "f_ControlSegIDLinked";
			this.f_ControlSegIDLinked.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_BeginYMD, "f_BeginYMD");
			this.f_BeginYMD.Name = "f_BeginYMD";
			this.f_BeginYMD.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_EndYMD, "f_EndYMD");
			this.f_EndYMD.Name = "f_EndYMD";
			this.f_EndYMD.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlByHoliday, "f_ControlByHoliday");
			this.f_ControlByHoliday.Name = "f_ControlByHoliday";
			this.f_ControlByHoliday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel1 });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExportToExcel, this.btnHolidayControl, this.btnFind });
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
			componentResourceManager.ApplyResources(this.btnHolidayControl, "btnHolidayControl");
			this.btnHolidayControl.ForeColor = global::System.Drawing.Color.White;
			this.btnHolidayControl.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Child;
			this.btnHolidayControl.Name = "btnHolidayControl";
			this.btnHolidayControl.Click += new global::System.EventHandler(this.btnHolidayControl_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvControlSegs);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmControlSegs";
			base.Load += new global::System.EventHandler(this.frmControlSegs_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmControlSegs_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvControlSegs).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000A90 RID: 2704
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000A91 RID: 2705
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04000A92 RID: 2706
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04000A93 RID: 2707
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04000A94 RID: 2708
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04000A95 RID: 2709
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04000A96 RID: 2710
		private global::System.Windows.Forms.DataGridView dgvControlSegs;

		// Token: 0x04000A97 RID: 2711
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_BeginHMS1A;

		// Token: 0x04000A98 RID: 2712
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_BeginHMS2A;

		// Token: 0x04000A99 RID: 2713
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_BeginHMS3A;

		// Token: 0x04000A9A RID: 2714
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_BeginYMD;

		// Token: 0x04000A9B RID: 2715
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_ControlByHoliday;

		// Token: 0x04000A9C RID: 2716
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegID;

		// Token: 0x04000A9D RID: 2717
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegIDBak;

		// Token: 0x04000A9E RID: 2718
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegIDLinked;

		// Token: 0x04000A9F RID: 2719
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EndHMS1A;

		// Token: 0x04000AA0 RID: 2720
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EndHMS2A;

		// Token: 0x04000AA1 RID: 2721
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EndHMS3A;

		// Token: 0x04000AA2 RID: 2722
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EndYMD;

		// Token: 0x04000AA3 RID: 2723
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Friday;

		// Token: 0x04000AA4 RID: 2724
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Monday;

		// Token: 0x04000AA5 RID: 2725
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Saturday;

		// Token: 0x04000AA6 RID: 2726
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Sunday;

		// Token: 0x04000AA7 RID: 2727
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Thursday;

		// Token: 0x04000AA8 RID: 2728
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Tuesday;

		// Token: 0x04000AA9 RID: 2729
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Wednesday;

		// Token: 0x04000AAA RID: 2730
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000AAB RID: 2731
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04000AAC RID: 2732
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel1;

		// Token: 0x04000AAD RID: 2733
		public global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04000AAE RID: 2734
		public global::System.Windows.Forms.ToolStripButton btnHolidayControl;
	}
}
