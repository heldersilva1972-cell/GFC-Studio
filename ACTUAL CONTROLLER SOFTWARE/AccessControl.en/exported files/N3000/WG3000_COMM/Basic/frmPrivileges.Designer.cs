namespace WG3000_COMM.Basic
{
	// Token: 0x02000051 RID: 81
	public partial class frmPrivileges : global::System.Windows.Forms.Form
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x000A4248 File Offset: 0x000A3248
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000A4268 File Offset: 0x000A3268
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmPrivileges));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvPrivileges = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSeg = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.cardNOFuzzyQueryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportOver65535ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.toolStrip2 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new global::System.Windows.Forms.ToolStripLabel();
			this.cboDoor = new global::System.Windows.Forms.ToolStripComboBox();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrivilegeCopy = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditSinglePrivilege = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivileges).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dgvPrivileges, "dgvPrivileges");
			this.dgvPrivileges.AllowUserToAddRows = false;
			this.dgvPrivileges.AllowUserToDeleteRows = false;
			this.dgvPrivileges.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvPrivileges.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvPrivileges.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvPrivileges.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_RecID, this.f_DoorName, this.f_ConsumerNO, this.f_ConsumerName, this.f_CardNO, this.f_ControlSeg, this.f_ControlSegName, this.f_ConsumerID });
			this.dgvPrivileges.ContextMenuStrip = this.contextMenuStrip1;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvPrivileges.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvPrivileges.EnableHeadersVisualStyles = false;
			this.dgvPrivileges.Name = "dgvPrivileges";
			this.dgvPrivileges.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvPrivileges.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvPrivileges.RowTemplate.Height = 23;
			this.dgvPrivileges.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvPrivileges.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPrivileges_CellFormatting);
			this.dgvPrivileges.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvPrivileges_Scroll);
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
			this.f_ConsumerNO.Name = "f_ConsumerNO";
			this.f_ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_CardNO, "f_CardNO");
			this.f_CardNO.Name = "f_CardNO";
			this.f_CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlSeg, "f_ControlSeg");
			this.f_ControlSeg.Name = "f_ControlSeg";
			this.f_ControlSeg.ReadOnly = true;
			this.f_ControlSegName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_ControlSegName, "f_ControlSegName");
			this.f_ControlSegName.Name = "f_ControlSegName";
			this.f_ControlSegName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
			this.f_ConsumerID.Name = "f_ConsumerID";
			this.f_ConsumerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.cardNOFuzzyQueryToolStripMenuItem, this.displayAllToolStripMenuItem, this.exportOver65535ToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.cardNOFuzzyQueryToolStripMenuItem, "cardNOFuzzyQueryToolStripMenuItem");
			this.cardNOFuzzyQueryToolStripMenuItem.Name = "cardNOFuzzyQueryToolStripMenuItem";
			this.cardNOFuzzyQueryToolStripMenuItem.Click += new global::System.EventHandler(this.cardNOFuzzyQueryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayAllToolStripMenuItem, "displayAllToolStripMenuItem");
			this.displayAllToolStripMenuItem.Name = "displayAllToolStripMenuItem";
			this.displayAllToolStripMenuItem.Click += new global::System.EventHandler(this.displayAllToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportOver65535ToolStripMenuItem, "exportOver65535ToolStripMenuItem");
			this.exportOver65535ToolStripMenuItem.Name = "exportOver65535ToolStripMenuItem";
			this.exportOver65535ToolStripMenuItem.Click += new global::System.EventHandler(this.exportOver65535ToolStripMenuItem_Click);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.toolStrip2, "toolStrip2");
			this.toolStrip2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel1, this.cboDoor });
			this.toolStrip2.Name = "toolStrip2";
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.cboDoor, "cboDoor");
			this.cboDoor.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoor.Name = "cboDoor";
			this.cboDoor.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.cboDoor_KeyPress);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnEdit, this.btnPrint, this.btnExport, this.toolStripButton2, this.btnPrivilegeCopy, this.btnEditSinglePrivilege, this.btnFind });
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.ItemClicked += new global::System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Image = global::WG3000_COMM.Properties.Resources.pTools_ChangePrivilege;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Image = global::WG3000_COMM.Properties.Resources.pTools_Print;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExport.Name = "btnExport";
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.toolStripButton2, "toolStripButton2");
			this.toolStripButton2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripButton2.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Click += new global::System.EventHandler(this.toolStripButton1_Click);
			componentResourceManager.ApplyResources(this.btnPrivilegeCopy, "btnPrivilegeCopy");
			this.btnPrivilegeCopy.ForeColor = global::System.Drawing.Color.White;
			this.btnPrivilegeCopy.Image = global::WG3000_COMM.Properties.Resources.pTools_CopyPrivilege;
			this.btnPrivilegeCopy.Name = "btnPrivilegeCopy";
			this.btnPrivilegeCopy.Click += new global::System.EventHandler(this.btnPrivilegeCopy_Click);
			componentResourceManager.ApplyResources(this.btnEditSinglePrivilege, "btnEditSinglePrivilege");
			this.btnEditSinglePrivilege.ForeColor = global::System.Drawing.Color.White;
			this.btnEditSinglePrivilege.Image = global::WG3000_COMM.Properties.Resources.pTools_EditPrivielge;
			this.btnEditSinglePrivilege.Name = "btnEditSinglePrivilege";
			this.btnEditSinglePrivilege.Click += new global::System.EventHandler(this.btnEditSinglePrivilege_Click);
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
			base.Controls.Add(this.dgvPrivileges);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip2);
			base.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			base.KeyPreview = true;
			base.Name = "frmPrivileges";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmPrivileges_FormClosing);
			base.Load += new global::System.EventHandler(this.frmPrivileges_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrm_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivileges).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000AF7 RID: 2807
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000AF8 RID: 2808
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04000AF9 RID: 2809
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04000AFA RID: 2810
		private global::System.Windows.Forms.ToolStripButton btnEditSinglePrivilege;

		// Token: 0x04000AFB RID: 2811
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04000AFC RID: 2812
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04000AFD RID: 2813
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04000AFE RID: 2814
		private global::System.Windows.Forms.ToolStripButton btnPrivilegeCopy;

		// Token: 0x04000AFF RID: 2815
		private global::System.Windows.Forms.ToolStripMenuItem cardNOFuzzyQueryToolStripMenuItem;

		// Token: 0x04000B00 RID: 2816
		private global::System.Windows.Forms.ToolStripComboBox cboDoor;

		// Token: 0x04000B01 RID: 2817
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000B02 RID: 2818
		private global::System.Windows.Forms.DataGridView dgvPrivileges;

		// Token: 0x04000B03 RID: 2819
		private global::System.Windows.Forms.ToolStripMenuItem displayAllToolStripMenuItem;

		// Token: 0x04000B04 RID: 2820
		private global::System.Windows.Forms.ToolStripMenuItem exportOver65535ToolStripMenuItem;

		// Token: 0x04000B05 RID: 2821
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CardNO;

		// Token: 0x04000B06 RID: 2822
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerID;

		// Token: 0x04000B07 RID: 2823
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04000B08 RID: 2824
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04000B09 RID: 2825
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSeg;

		// Token: 0x04000B0A RID: 2826
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName;

		// Token: 0x04000B0B RID: 2827
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x04000B0C RID: 2828
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04000B0D RID: 2829
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000B0E RID: 2830
		private global::System.Windows.Forms.ToolStrip toolStrip2;

		// Token: 0x04000B0F RID: 2831
		private global::System.Windows.Forms.ToolStripButton toolStripButton2;

		// Token: 0x04000B10 RID: 2832
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel1;

		// Token: 0x04000B11 RID: 2833
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;
	}
}
