namespace WG3000_COMM.Basic
{
	// Token: 0x0200004F RID: 79
	public partial class frmDepartments : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x000A076E File Offset: 0x0009F76E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000A0790 File Offset: 0x0009F790
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmDepartments));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.exportToExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.importFromExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.GroupNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.trvDepartments = new global::System.Windows.Forms.TreeView();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.txtDeptName = new global::System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel1 = new global::System.Windows.Forms.ToolStripLabel();
			this.txtSelectedDept = new global::System.Windows.Forms.ToolStripTextBox();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAddSuper = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSplitButton1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton2 = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnEditDept = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnMoveDept = new global::System.Windows.Forms.ToolStripButton();
			this.btnDeleteDept = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.exportToExcelToolStripMenuItem, this.importFromExcelToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.exportToExcelToolStripMenuItem, "exportToExcelToolStripMenuItem");
			this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
			this.exportToExcelToolStripMenuItem.Click += new global::System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.importFromExcelToolStripMenuItem, "importFromExcelToolStripMenuItem");
			this.importFromExcelToolStripMenuItem.Name = "importFromExcelToolStripMenuItem";
			this.importFromExcelToolStripMenuItem.Click += new global::System.EventHandler(this.importFromExcelToolStripMenuItem_Click);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.GroupNO, this.Column1 });
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 23;
			componentResourceManager.ApplyResources(this.GroupNO, "GroupNO");
			this.GroupNO.Name = "GroupNO";
			this.GroupNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.trvDepartments, "trvDepartments");
			this.trvDepartments.Name = "trvDepartments";
			this.trvDepartments.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.trvDepartments_AfterSelect);
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel2, this.txtDeptName, this.toolStripLabel1, this.txtSelectedDept });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
			this.toolStripLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel2.Name = "toolStripLabel2";
			componentResourceManager.ApplyResources(this.txtDeptName, "txtDeptName");
			this.txtDeptName.ForeColor = global::System.Drawing.SystemColors.WindowText;
			this.txtDeptName.Name = "txtDeptName";
			this.txtDeptName.TextChanged += new global::System.EventHandler(this.txtDeptName_TextChanged);
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.txtSelectedDept, "txtSelectedDept");
			this.txtSelectedDept.BackColor = global::System.Drawing.SystemColors.Control;
			this.txtSelectedDept.Name = "txtSelectedDept";
			this.txtSelectedDept.ReadOnly = true;
			this.txtSelectedDept.TextChanged += new global::System.EventHandler(this.txtSelectedDept_TextChanged);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.ContextMenuStrip = this.contextMenuStrip1;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.btnAddSuper, this.toolStripSplitButton1, this.btnAdd, this.toolStripSeparator1, this.toolStripButton2, this.toolStripSeparator3, this.btnEditDept, this.toolStripSeparator2, this.btnMoveDept, this.btnDeleteDept,
				this.btnExport
			});
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnAddSuper, "btnAddSuper");
			this.btnAddSuper.ForeColor = global::System.Drawing.Color.White;
			this.btnAddSuper.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Top;
			this.btnAddSuper.Name = "btnAddSuper";
			this.btnAddSuper.Click += new global::System.EventHandler(this.btnAddSuper_Click);
			componentResourceManager.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Child;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.toolStripButton2, "toolStripButton2");
			this.toolStripButton2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton2.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Click += new global::System.EventHandler(this.copySelectedDeptToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.btnEditDept, "btnEditDept");
			this.btnEditDept.ForeColor = global::System.Drawing.Color.White;
			this.btnEditDept.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit;
			this.btnEditDept.Name = "btnEditDept";
			this.btnEditDept.Click += new global::System.EventHandler(this.btnEditDept_Click);
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.btnMoveDept, "btnMoveDept");
			this.btnMoveDept.ForeColor = global::System.Drawing.Color.White;
			this.btnMoveDept.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit_Batch;
			this.btnMoveDept.Name = "btnMoveDept";
			this.btnMoveDept.Click += new global::System.EventHandler(this.btnMoveDept_Click);
			componentResourceManager.ApplyResources(this.btnDeleteDept, "btnDeleteDept");
			this.btnDeleteDept.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteDept.Image = global::WG3000_COMM.Properties.Resources.pTools_Del;
			this.btnDeleteDept.Name = "btnDeleteDept";
			this.btnDeleteDept.Click += new global::System.EventHandler(this.btnDeleteDept_Click);
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExport.Name = "btnExport";
			this.btnExport.Click += new global::System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.trvDepartments);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmDepartments";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmDepartments_FormClosed);
			base.Load += new global::System.EventHandler(this.frmDepartments_Load);
			base.TextChanged += new global::System.EventHandler(this.txtDeptName_TextChanged);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000AB7 RID: 2743
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000AB8 RID: 2744
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04000AB9 RID: 2745
		private global::System.Windows.Forms.ToolStripButton btnAddSuper;

		// Token: 0x04000ABA RID: 2746
		private global::System.Windows.Forms.ToolStripButton btnDeleteDept;

		// Token: 0x04000ABB RID: 2747
		private global::System.Windows.Forms.ToolStripButton btnEditDept;

		// Token: 0x04000ABC RID: 2748
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04000ABD RID: 2749
		private global::System.Windows.Forms.ToolStripButton btnMoveDept;

		// Token: 0x04000ABE RID: 2750
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04000ABF RID: 2751
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000AC0 RID: 2752
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000AC1 RID: 2753
		private global::System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;

		// Token: 0x04000AC2 RID: 2754
		private global::System.Windows.Forms.DataGridViewTextBoxColumn GroupNO;

		// Token: 0x04000AC3 RID: 2755
		private global::System.Windows.Forms.ToolStripMenuItem importFromExcelToolStripMenuItem;

		// Token: 0x04000AC4 RID: 2756
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04000AC5 RID: 2757
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000AC6 RID: 2758
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04000AC7 RID: 2759
		private global::System.Windows.Forms.ToolStripButton toolStripButton2;

		// Token: 0x04000AC8 RID: 2760
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel1;

		// Token: 0x04000AC9 RID: 2761
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04000ACA RID: 2762
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000ACB RID: 2763
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000ACC RID: 2764
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x04000ACD RID: 2765
		private global::System.Windows.Forms.ToolStripSeparator toolStripSplitButton1;

		// Token: 0x04000ACE RID: 2766
		private global::System.Windows.Forms.TreeView trvDepartments;

		// Token: 0x04000ACF RID: 2767
		private global::System.Windows.Forms.ToolStripTextBox txtDeptName;

		// Token: 0x04000AD0 RID: 2768
		private global::System.Windows.Forms.ToolStripTextBox txtSelectedDept;
	}
}
