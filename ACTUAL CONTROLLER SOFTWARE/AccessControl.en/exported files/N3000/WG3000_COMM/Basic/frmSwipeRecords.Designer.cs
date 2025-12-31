namespace WG3000_COMM.Basic
{
	// Token: 0x02000052 RID: 82
	public partial class frmSwipeRecords : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x000A8538 File Offset: 0x000A7538
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000A8558 File Offset: 0x000A7558
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmSwipeRecords));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.dgvSwipeRecords = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadDate = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Addr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Pass = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Desc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_RecordAll = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CapturedFileExisted = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deletedUsersSwipeQueryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportTextFileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportTextToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setOutputFormatToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportOver65535ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.cboStart = new global::System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.cboEnd = new global::System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel4 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel5 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFindOption = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnCameraView = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipeRecords).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.dgvSwipeRecords, "dgvSwipeRecords");
			this.dgvSwipeRecords.AllowUserToAddRows = false;
			this.dgvSwipeRecords.AllowUserToDeleteRows = false;
			this.dgvSwipeRecords.AllowUserToOrderColumns = true;
			this.dgvSwipeRecords.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSwipeRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSwipeRecords.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSwipeRecords.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_RecID, this.f_CardNO, this.f_ConsumerNO, this.f_ConsumerName, this.f_DepartmentName, this.f_ReadDate, this.f_Addr, this.f_Pass, this.f_Desc, this.f_RecordAll,
				this.f_CapturedFileExisted
			});
			this.dgvSwipeRecords.ContextMenuStrip = this.contextMenuStrip1;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSwipeRecords.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSwipeRecords.EnableHeadersVisualStyles = false;
			this.dgvSwipeRecords.Name = "dgvSwipeRecords";
			this.dgvSwipeRecords.ReadOnly = true;
			this.dgvSwipeRecords.RowHeadersVisible = false;
			this.dgvSwipeRecords.RowTemplate.Height = 23;
			this.dgvSwipeRecords.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSwipeRecords.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSwipeRecords_CellFormatting);
			this.dgvSwipeRecords.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvSwipeRecords_Scroll);
			this.dgvSwipeRecords.SelectionChanged += new global::System.EventHandler(this.dgvSwipeRecords_SelectionChanged);
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_RecID.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_CardNO.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_CardNO, "f_CardNO");
			this.f_CardNO.Name = "f_CardNO";
			this.f_CardNO.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
			this.f_ConsumerNO.Name = "f_ConsumerNO";
			this.f_ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
			this.f_DepartmentName.Name = "f_DepartmentName";
			this.f_DepartmentName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReadDate, "f_ReadDate");
			this.f_ReadDate.Name = "f_ReadDate";
			this.f_ReadDate.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Addr, "f_Addr");
			this.f_Addr.Name = "f_Addr";
			this.f_Addr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Pass, "f_Pass");
			this.f_Pass.Name = "f_Pass";
			this.f_Pass.ReadOnly = true;
			this.f_Pass.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Pass.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.f_Desc.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Desc, "f_Desc");
			this.f_Desc.Name = "f_Desc";
			this.f_Desc.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_RecordAll, "f_RecordAll");
			this.f_RecordAll.Name = "f_RecordAll";
			this.f_RecordAll.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_CapturedFileExisted, "f_CapturedFileExisted");
			this.f_CapturedFileExisted.Name = "f_CapturedFileExisted";
			this.f_CapturedFileExisted.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripMenuItem1, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem, this.deletedUsersSwipeQueryToolStripMenuItem, this.exportTextFileToolStripMenuItem, this.exportOver65535ToolStripMenuItem, this.loadAllToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Click += new global::System.EventHandler(this.toolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.deletedUsersSwipeQueryToolStripMenuItem, "deletedUsersSwipeQueryToolStripMenuItem");
			this.deletedUsersSwipeQueryToolStripMenuItem.Name = "deletedUsersSwipeQueryToolStripMenuItem";
			this.deletedUsersSwipeQueryToolStripMenuItem.Click += new global::System.EventHandler(this.deletedUsersSwipeQueryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportTextFileToolStripMenuItem, "exportTextFileToolStripMenuItem");
			this.exportTextFileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.exportTextToolStripMenuItem, this.setOutputFormatToolStripMenuItem });
			this.exportTextFileToolStripMenuItem.Name = "exportTextFileToolStripMenuItem";
			componentResourceManager.ApplyResources(this.exportTextToolStripMenuItem, "exportTextToolStripMenuItem");
			this.exportTextToolStripMenuItem.Name = "exportTextToolStripMenuItem";
			this.exportTextToolStripMenuItem.Click += new global::System.EventHandler(this.exportTextToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setOutputFormatToolStripMenuItem, "setOutputFormatToolStripMenuItem");
			this.setOutputFormatToolStripMenuItem.Name = "setOutputFormatToolStripMenuItem";
			this.setOutputFormatToolStripMenuItem.Click += new global::System.EventHandler(this.setOutputFormatToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportOver65535ToolStripMenuItem, "exportOver65535ToolStripMenuItem");
			this.exportOver65535ToolStripMenuItem.Name = "exportOver65535ToolStripMenuItem";
			this.exportOver65535ToolStripMenuItem.Click += new global::System.EventHandler(this.exportOver65535ToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.loadAllToolStripMenuItem, "loadAllToolStripMenuItem");
			this.loadAllToolStripMenuItem.Name = "loadAllToolStripMenuItem";
			this.loadAllToolStripMenuItem.Click += new global::System.EventHandler(this.loadAllToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStrip3, "toolStrip3");
			this.toolStrip3.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip3.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel2, this.cboStart, this.toolStripLabel3, this.cboEnd, this.toolStripSeparator1, this.toolStripLabel4, this.toolStripLabel5 });
			this.toolStrip3.Name = "toolStrip3";
			componentResourceManager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
			this.toolStripLabel2.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.toolStripLabel2.Name = "toolStripLabel2";
			componentResourceManager.ApplyResources(this.cboStart, "cboStart");
			this.cboStart.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboStart.Items"),
				componentResourceManager.GetString("cboStart.Items1")
			});
			this.cboStart.Name = "cboStart";
			this.cboStart.SelectedIndexChanged += new global::System.EventHandler(this.cboStart_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
			this.toolStripLabel3.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.toolStripLabel3.Name = "toolStripLabel3";
			componentResourceManager.ApplyResources(this.cboEnd, "cboEnd");
			this.cboEnd.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboEnd.Items"),
				componentResourceManager.GetString("cboEnd.Items1")
			});
			this.cboEnd.Name = "cboEnd";
			this.cboEnd.SelectedIndexChanged += new global::System.EventHandler(this.cboEnd_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.toolStripLabel4, "toolStripLabel4");
			this.toolStripLabel4.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.toolStripLabel4.Name = "toolStripLabel4";
			componentResourceManager.ApplyResources(this.toolStripLabel5, "toolStripLabel5");
			this.toolStripLabel5.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.toolStripLabel5.Name = "toolStripLabel5";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnPrint, this.btnExportToExcel, this.btnFindOption, this.btnDelete, this.btnCameraView, this.btnFind });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.btnPrint.Image = global::WG3000_COMM.Properties.Resources.pTools_Print;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
			this.btnExportToExcel.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.btnExportToExcel.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExportToExcel.Name = "btnExportToExcel";
			this.btnExportToExcel.Click += new global::System.EventHandler(this.btnExportToExcel_Click);
			componentResourceManager.ApplyResources(this.btnFindOption, "btnFindOption");
			this.btnFindOption.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.btnFindOption.Image = global::WG3000_COMM.Properties.Resources.pTools_QueryOption;
			this.btnFindOption.Name = "btnFindOption";
			this.btnFindOption.Click += new global::System.EventHandler(this.btnFindOption_Click);
			componentResourceManager.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.ForeColor = global::System.Drawing.Color.White;
			this.btnDelete.Image = global::WG3000_COMM.Properties.Resources.pTools_Del;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Click += new global::System.EventHandler(this.btnDelete_Click);
			componentResourceManager.ApplyResources(this.btnCameraView, "btnCameraView");
			this.btnCameraView.ForeColor = global::System.Drawing.Color.FromArgb(233, 241, 255);
			this.btnCameraView.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnCameraView.Name = "btnCameraView";
			this.btnCameraView.Click += new global::System.EventHandler(this.btnCameraView_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvSwipeRecords);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmSwipeRecords";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmSwipeRecords_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.frmSwipeRecords_FormClosed);
			base.Load += new global::System.EventHandler(this.frmSwipeRecords_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmSwipeRecords_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipeRecords).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000B22 RID: 2850
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000B23 RID: 2851
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04000B24 RID: 2852
		private global::System.Windows.Forms.ToolStripButton btnCameraView;

		// Token: 0x04000B25 RID: 2853
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04000B26 RID: 2854
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04000B27 RID: 2855
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04000B28 RID: 2856
		private global::System.Windows.Forms.ToolStripButton btnFindOption;

		// Token: 0x04000B29 RID: 2857
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04000B2A RID: 2858
		private global::System.Windows.Forms.ToolStripComboBox cboEnd;

		// Token: 0x04000B2B RID: 2859
		private global::System.Windows.Forms.ToolStripComboBox cboStart;

		// Token: 0x04000B2C RID: 2860
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000B2D RID: 2861
		private global::System.Windows.Forms.ToolStripMenuItem deletedUsersSwipeQueryToolStripMenuItem;

		// Token: 0x04000B2E RID: 2862
		private global::System.Windows.Forms.DataGridView dgvSwipeRecords;

		// Token: 0x04000B33 RID: 2867
		private global::System.Windows.Forms.ToolStripMenuItem exportOver65535ToolStripMenuItem;

		// Token: 0x04000B34 RID: 2868
		private global::System.Windows.Forms.ToolStripMenuItem exportTextFileToolStripMenuItem;

		// Token: 0x04000B35 RID: 2869
		private global::System.Windows.Forms.ToolStripMenuItem exportTextToolStripMenuItem;

		// Token: 0x04000B36 RID: 2870
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Addr;

		// Token: 0x04000B37 RID: 2871
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CapturedFileExisted;

		// Token: 0x04000B38 RID: 2872
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CardNO;

		// Token: 0x04000B39 RID: 2873
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04000B3A RID: 2874
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04000B3B RID: 2875
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x04000B3C RID: 2876
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x04000B3D RID: 2877
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Pass;

		// Token: 0x04000B3E RID: 2878
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReadDate;

		// Token: 0x04000B3F RID: 2879
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04000B40 RID: 2880
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecordAll;

		// Token: 0x04000B41 RID: 2881
		private global::System.Windows.Forms.ToolStripMenuItem loadAllToolStripMenuItem;

		// Token: 0x04000B42 RID: 2882
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x04000B43 RID: 2883
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x04000B44 RID: 2884
		private global::System.Windows.Forms.ToolStripMenuItem setOutputFormatToolStripMenuItem;

		// Token: 0x04000B45 RID: 2885
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000B46 RID: 2886
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04000B47 RID: 2887
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x04000B48 RID: 2888
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04000B49 RID: 2889
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x04000B4A RID: 2890
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel4;

		// Token: 0x04000B4B RID: 2891
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel5;

		// Token: 0x04000B4C RID: 2892
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x04000B4D RID: 2893
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000B4E RID: 2894
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;
	}
}
