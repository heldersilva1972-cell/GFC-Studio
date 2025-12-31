namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002FA RID: 762
	public partial class frmMeal : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001667 RID: 5735 RVA: 0x001C97E1 File Offset: 0x001C87E1
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x001C9800 File Offset: 0x001C8800
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meal.frmMeal));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.editPrintersFooterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.columnsConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultLayoutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.dgvSwipeRecords = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadDate = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MealName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Cost = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Addr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReaderID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.dgvSubtotal = new global::System.Windows.Forms.DataGridView();
			this.f_ReaderID2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.dgvGroupSubTotal = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.dgvStatistics = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Morning = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Lunch = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Evening = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Other = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ProgressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnMealSetup = new global::System.Windows.Forms.ToolStripButton();
			this.btnCreateReport = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			this.contextMenuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipeRecords).BeginInit();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSubtotal).BeginInit();
			this.tabPage4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroupSubTotal).BeginInit();
			this.tabPage3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvStatistics).BeginInit();
			this.toolStrip3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.editPrintersFooterToolStripMenuItem, this.columnsConfigureToolStripMenuItem, this.saveLayoutToolStripMenuItem, this.restoreDefaultLayoutToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.editPrintersFooterToolStripMenuItem, "editPrintersFooterToolStripMenuItem");
			this.editPrintersFooterToolStripMenuItem.Name = "editPrintersFooterToolStripMenuItem";
			this.editPrintersFooterToolStripMenuItem.Click += new global::System.EventHandler(this.editPrintersFooterToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.columnsConfigureToolStripMenuItem, "columnsConfigureToolStripMenuItem");
			this.columnsConfigureToolStripMenuItem.Name = "columnsConfigureToolStripMenuItem";
			this.columnsConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.columnsConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.saveLayoutToolStripMenuItem, "saveLayoutToolStripMenuItem");
			this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
			this.saveLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.saveLayoutToolStripMenuItem_Click_1);
			componentResourceManager.ApplyResources(this.restoreDefaultLayoutToolStripMenuItem, "restoreDefaultLayoutToolStripMenuItem");
			this.restoreDefaultLayoutToolStripMenuItem.Name = "restoreDefaultLayoutToolStripMenuItem";
			this.restoreDefaultLayoutToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultLayoutToolStripMenuItem_Click_1);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.dgvSwipeRecords);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
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
			this.dgvSwipeRecords.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_RecID, this.f_ConsumerID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_ReadDate, this.MealName, this.f_Cost, this.f_Addr, this.f_ReaderID });
			this.dgvSwipeRecords.EnableHeadersVisualStyles = false;
			this.dgvSwipeRecords.Name = "dgvSwipeRecords";
			this.dgvSwipeRecords.ReadOnly = true;
			this.dgvSwipeRecords.RowHeadersVisible = false;
			this.dgvSwipeRecords.RowTemplate.Height = 23;
			this.dgvSwipeRecords.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSwipeRecords.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSwipeRecords_CellFormatting);
			this.dgvSwipeRecords.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvSwipeRecords_Scroll);
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerID.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
			this.f_ConsumerID.Name = "f_ConsumerID";
			this.f_ConsumerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
			this.f_DepartmentName.Name = "f_DepartmentName";
			this.f_DepartmentName.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
			this.f_ConsumerNO.Name = "f_ConsumerNO";
			this.f_ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReadDate, "f_ReadDate");
			this.f_ReadDate.Name = "f_ReadDate";
			this.f_ReadDate.ReadOnly = true;
			componentResourceManager.ApplyResources(this.MealName, "MealName");
			this.MealName.Name = "MealName";
			this.MealName.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_Cost.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.f_Cost, "f_Cost");
			this.f_Cost.Name = "f_Cost";
			this.f_Cost.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Addr, "f_Addr");
			this.f_Addr.Name = "f_Addr";
			this.f_Addr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReaderID, "f_ReaderID");
			this.f_ReaderID.Name = "f_ReaderID";
			this.f_ReaderID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.dgvSubtotal);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvSubtotal, "dgvSubtotal");
			this.dgvSubtotal.AllowUserToAddRows = false;
			this.dgvSubtotal.AllowUserToDeleteRows = false;
			this.dgvSubtotal.AllowUserToOrderColumns = true;
			this.dgvSubtotal.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSubtotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvSubtotal.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSubtotal.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ReaderID2, this.dataGridViewTextBoxColumn9, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn5 });
			this.dgvSubtotal.EnableHeadersVisualStyles = false;
			this.dgvSubtotal.Name = "dgvSubtotal";
			this.dgvSubtotal.ReadOnly = true;
			this.dgvSubtotal.RowHeadersVisible = false;
			this.dgvSubtotal.RowTemplate.Height = 23;
			this.dgvSubtotal.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_ReaderID2, "f_ReaderID2");
			this.f_ReaderID2.Name = "f_ReaderID2";
			this.f_ReaderID2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle7;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle8;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage4.Controls.Add(this.dgvGroupSubTotal);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvGroupSubTotal, "dgvGroupSubTotal");
			this.dgvGroupSubTotal.AllowUserToAddRows = false;
			this.dgvGroupSubTotal.AllowUserToDeleteRows = false;
			this.dgvGroupSubTotal.AllowUserToOrderColumns = true;
			this.dgvGroupSubTotal.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle9.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle9.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroupSubTotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dgvGroupSubTotal.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvGroupSubTotal.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.Column2, this.Column3, this.Column4, this.Column1, this.dataGridViewTextBoxColumn7, this.dataGridViewTextBoxColumn8 });
			this.dgvGroupSubTotal.EnableHeadersVisualStyles = false;
			this.dgvGroupSubTotal.Name = "dgvGroupSubTotal";
			this.dgvGroupSubTotal.ReadOnly = true;
			this.dgvGroupSubTotal.RowHeadersVisible = false;
			this.dgvGroupSubTotal.RowTemplate.Height = 23;
			this.dgvGroupSubTotal.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Column2.DefaultCellStyle = dataGridViewCellStyle10;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			dataGridViewCellStyle11.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Column3.DefaultCellStyle = dataGridViewCellStyle11;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			dataGridViewCellStyle12.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Column4.DefaultCellStyle = dataGridViewCellStyle12;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			dataGridViewCellStyle13.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Column1.DefaultCellStyle = dataGridViewCellStyle13;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			dataGridViewCellStyle14.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle14;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			dataGridViewCellStyle15.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle15;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.dgvStatistics);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvStatistics, "dgvStatistics");
			this.dgvStatistics.AllowUserToAddRows = false;
			this.dgvStatistics.AllowUserToDeleteRows = false;
			this.dgvStatistics.AllowUserToOrderColumns = true;
			this.dgvStatistics.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle16.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle16.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle16.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle16.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle16.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle16.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvStatistics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
			this.dgvStatistics.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvStatistics.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.f_DepartmentName2, this.f_ConsumerNO2, this.dataGridViewTextBoxColumn6, this.Morning, this.Lunch, this.Evening, this.Other, this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11 });
			this.dgvStatistics.EnableHeadersVisualStyles = false;
			this.dgvStatistics.Name = "dgvStatistics";
			this.dgvStatistics.ReadOnly = true;
			this.dgvStatistics.RowHeadersVisible = false;
			this.dgvStatistics.RowTemplate.Height = 23;
			this.dgvStatistics.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dataGridViewCellStyle17.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle17;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DepartmentName2, "f_DepartmentName2");
			this.f_DepartmentName2.Name = "f_DepartmentName2";
			this.f_DepartmentName2.ReadOnly = true;
			dataGridViewCellStyle18.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO2.DefaultCellStyle = dataGridViewCellStyle18;
			componentResourceManager.ApplyResources(this.f_ConsumerNO2, "f_ConsumerNO2");
			this.f_ConsumerNO2.Name = "f_ConsumerNO2";
			this.f_ConsumerNO2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			dataGridViewCellStyle19.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Morning.DefaultCellStyle = dataGridViewCellStyle19;
			componentResourceManager.ApplyResources(this.Morning, "Morning");
			this.Morning.Name = "Morning";
			this.Morning.ReadOnly = true;
			dataGridViewCellStyle20.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Lunch.DefaultCellStyle = dataGridViewCellStyle20;
			componentResourceManager.ApplyResources(this.Lunch, "Lunch");
			this.Lunch.Name = "Lunch";
			this.Lunch.ReadOnly = true;
			dataGridViewCellStyle21.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Evening.DefaultCellStyle = dataGridViewCellStyle21;
			componentResourceManager.ApplyResources(this.Evening, "Evening");
			this.Evening.Name = "Evening";
			this.Evening.ReadOnly = true;
			dataGridViewCellStyle22.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.Other.DefaultCellStyle = dataGridViewCellStyle22;
			componentResourceManager.ApplyResources(this.Other, "Other");
			this.Other.Name = "Other";
			this.Other.ReadOnly = true;
			dataGridViewCellStyle23.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle23;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			dataGridViewCellStyle24.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle24;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ProgressBar1, "ProgressBar1");
			this.ProgressBar1.Name = "ProgressBar1";
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
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
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnMealSetup, this.btnCreateReport, this.btnPrint, this.btnExportToExcel, this.btnFind, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnMealSetup, "btnMealSetup");
			this.btnMealSetup.ForeColor = global::System.Drawing.Color.White;
			this.btnMealSetup.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnMealSetup.Name = "btnMealSetup";
			this.btnMealSetup.Click += new global::System.EventHandler(this.btnMealSetup_Click);
			componentResourceManager.ApplyResources(this.btnCreateReport, "btnCreateReport");
			this.btnCreateReport.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateReport.Image = global::WG3000_COMM.Properties.Resources.pTools_CreateShiftReport;
			this.btnCreateReport.Name = "btnCreateReport";
			this.btnCreateReport.Click += new global::System.EventHandler(this.btnCreateReport_Click);
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
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.btnExit.Name = "btnExit";
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.ProgressBar1);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmMeal";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmSwipeRecords_FormClosing);
			base.Load += new global::System.EventHandler(this.frmSwipeRecords_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmSwipeRecords_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipeRecords).EndInit();
			this.tabPage2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSubtotal).EndInit();
			this.tabPage4.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroupSubTotal).EndInit();
			this.tabPage3.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvStatistics).EndInit();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002E4C RID: 11852
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002E4D RID: 11853
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002E4E RID: 11854
		private global::System.Windows.Forms.ToolStripButton btnCreateReport;

		// Token: 0x04002E4F RID: 11855
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x04002E50 RID: 11856
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04002E51 RID: 11857
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04002E52 RID: 11858
		private global::System.Windows.Forms.ToolStripButton btnMealSetup;

		// Token: 0x04002E53 RID: 11859
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04002E54 RID: 11860
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002E55 RID: 11861
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04002E56 RID: 11862
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04002E57 RID: 11863
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x04002E58 RID: 11864
		private global::System.Windows.Forms.ToolStripMenuItem columnsConfigureToolStripMenuItem;

		// Token: 0x04002E59 RID: 11865
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002E5A RID: 11866
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002E5B RID: 11867
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		// Token: 0x04002E5C RID: 11868
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		// Token: 0x04002E5D RID: 11869
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002E5E RID: 11870
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002E5F RID: 11871
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002E60 RID: 11872
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		// Token: 0x04002E61 RID: 11873
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002E62 RID: 11874
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002E63 RID: 11875
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04002E64 RID: 11876
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04002E65 RID: 11877
		private global::System.Windows.Forms.DataGridView dgvGroupSubTotal;

		// Token: 0x04002E66 RID: 11878
		private global::System.Windows.Forms.DataGridView dgvStatistics;

		// Token: 0x04002E67 RID: 11879
		private global::System.Windows.Forms.DataGridView dgvSubtotal;

		// Token: 0x04002E68 RID: 11880
		private global::System.Windows.Forms.DataGridView dgvSwipeRecords;

		// Token: 0x04002E6B RID: 11883
		private global::System.Windows.Forms.ToolStripMenuItem editPrintersFooterToolStripMenuItem;

		// Token: 0x04002E6C RID: 11884
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Evening;

		// Token: 0x04002E6D RID: 11885
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Addr;

		// Token: 0x04002E6E RID: 11886
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerID;

		// Token: 0x04002E6F RID: 11887
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04002E70 RID: 11888
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x04002E71 RID: 11889
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO2;

		// Token: 0x04002E72 RID: 11890
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Cost;

		// Token: 0x04002E73 RID: 11891
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x04002E74 RID: 11892
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName2;

		// Token: 0x04002E75 RID: 11893
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReadDate;

		// Token: 0x04002E76 RID: 11894
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderID;

		// Token: 0x04002E77 RID: 11895
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderID2;

		// Token: 0x04002E78 RID: 11896
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04002E79 RID: 11897
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Lunch;

		// Token: 0x04002E7A RID: 11898
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MealName;

		// Token: 0x04002E7B RID: 11899
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Morning;

		// Token: 0x04002E7C RID: 11900
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Other;

		// Token: 0x04002E7D RID: 11901
		private global::System.Windows.Forms.ProgressBar ProgressBar1;

		// Token: 0x04002E7E RID: 11902
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultLayoutToolStripMenuItem;

		// Token: 0x04002E7F RID: 11903
		private global::System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

		// Token: 0x04002E80 RID: 11904
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002E82 RID: 11906
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002E83 RID: 11907
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002E84 RID: 11908
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04002E85 RID: 11909
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04002E86 RID: 11910
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04002E87 RID: 11911
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04002E88 RID: 11912
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x04002E89 RID: 11913
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04002E8A RID: 11914
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x04002E8B RID: 11915
		private global::WG3000_COMM.Core.UserControlFind userControlFind1;
	}
}
