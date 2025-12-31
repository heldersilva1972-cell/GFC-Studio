namespace WG3000_COMM.Basic
{
	// Token: 0x02000059 RID: 89
	public partial class frmZones : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x000BEB5D File Offset: 0x000BDB5D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000BEB7C File Offset: 0x000BDB7C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmZones));
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAddSuper = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSplitButton1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnEditDept = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.btnMoveDept = new global::System.Windows.Forms.ToolStripButton();
			this.btnDeleteDept = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new global::System.Windows.Forms.ToolStripLabel();
			this.txtSelectedDept = new global::System.Windows.Forms.ToolStripTextBox();
			this.trvDepartments = new global::System.Windows.Forms.TreeView();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.ZoneNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAddSuper, this.toolStripSplitButton1, this.btnAdd, this.toolStripSeparator1, this.btnEditDept, this.toolStripSeparator2, this.btnMoveDept, this.btnDeleteDept, this.btnExport });
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
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel1, this.txtSelectedDept });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.txtSelectedDept, "txtSelectedDept");
			this.txtSelectedDept.BackColor = global::System.Drawing.SystemColors.Control;
			this.txtSelectedDept.Name = "txtSelectedDept";
			this.txtSelectedDept.ReadOnly = true;
			this.txtSelectedDept.TextChanged += new global::System.EventHandler(this.txtSelectedDept_TextChanged);
			componentResourceManager.ApplyResources(this.trvDepartments, "trvDepartments");
			this.trvDepartments.Name = "trvDepartments";
			this.trvDepartments.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.trvDepartments_AfterSelect);
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ZoneNO, this.Column1 });
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 23;
			componentResourceManager.ApplyResources(this.ZoneNO, "ZoneNO");
			this.ZoneNO.Name = "ZoneNO";
			this.ZoneNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.trvDepartments);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmZones";
			base.Load += new global::System.EventHandler(this.frmDepartments_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000C8D RID: 3213
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000C8E RID: 3214
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04000C8F RID: 3215
		private global::System.Windows.Forms.ToolStripButton btnAddSuper;

		// Token: 0x04000C90 RID: 3216
		private global::System.Windows.Forms.ToolStripButton btnDeleteDept;

		// Token: 0x04000C91 RID: 3217
		private global::System.Windows.Forms.ToolStripButton btnEditDept;

		// Token: 0x04000C92 RID: 3218
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04000C93 RID: 3219
		private global::System.Windows.Forms.ToolStripButton btnMoveDept;

		// Token: 0x04000C94 RID: 3220
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04000C95 RID: 3221
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000C96 RID: 3222
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000C97 RID: 3223
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04000C98 RID: 3224
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel1;

		// Token: 0x04000C99 RID: 3225
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000C9A RID: 3226
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000C9B RID: 3227
		private global::System.Windows.Forms.ToolStripSeparator toolStripSplitButton1;

		// Token: 0x04000C9C RID: 3228
		private global::System.Windows.Forms.TreeView trvDepartments;

		// Token: 0x04000C9D RID: 3229
		private global::System.Windows.Forms.ToolStripTextBox txtSelectedDept;

		// Token: 0x04000C9E RID: 3230
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ZoneNO;
	}
}
