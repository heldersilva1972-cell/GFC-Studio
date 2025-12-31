namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000247 RID: 583
	public partial class dfrmPersonsInside : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001244 RID: 4676 RVA: 0x0015D5B0 File Offset: 0x0015C5B0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0015D5D0 File Offset: 0x0015C5D0
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmPersonsInside));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.label5 = new global::System.Windows.Forms.Label();
			this.btnQuery2 = new global::System.Windows.Forms.Button();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.chkAutoRefresh = new global::System.Windows.Forms.CheckBox();
			this.nudCycleSecs = new global::System.Windows.Forms.NumericUpDown();
			this.label4 = new global::System.Windows.Forms.Label();
			this.txtPersonsOutSide = new global::System.Windows.Forms.TextBox();
			this.txtPersons = new global::System.Windows.Forms.TextBox();
			this.nudDays = new global::System.Windows.Forms.NumericUpDown();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.lblIndex = new global::System.Windows.Forms.Label();
			this.btnSelectNone = new global::System.Windows.Forms.Button();
			this.btnSelectAll = new global::System.Windows.Forms.Button();
			this.cboZone = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.chkListDoors = new global::System.Windows.Forms.CheckedListBox();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnQuery = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.dgvEnterIn = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.DeactivateDate = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip3 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.SelectAlltoolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.BatchUpdateSelectedtoolStripMenuItem4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.DelayValidDatetoolStripMenuItem3 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ClearSelectedtoolStripMenuItem2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deleteSelectedPersonsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.activeExportToXMLToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.dgvOutSide = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn15 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn2 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.dgvGroupSubTotal = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SwipeOut = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.dgvSwipe = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn18 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn19 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn3 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage5 = new global::System.Windows.Forms.TabPage();
			this.dgvNotSwipe = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn21 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn23 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn4 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.backgroundWorker2 = new global::System.ComponentModel.BackgroundWorker();
			this.chkReaderIn = new global::System.Windows.Forms.CheckBox();
			this.chkReaderOut = new global::System.Windows.Forms.CheckBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCycleSecs).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDays).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvEnterIn).BeginInit();
			this.contextMenuStrip3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOutSide).BeginInit();
			this.tabPage3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroupSubTotal).BeginInit();
			this.tabPage4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipe).BeginInit();
			this.tabPage5.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvNotSwipe).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Enabled = true;
			this.timer1.Interval = 200;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.Yellow;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.btnQuery2, "btnQuery2");
			this.btnQuery2.BackColor = global::System.Drawing.Color.Transparent;
			this.btnQuery2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnQuery2.ForeColor = global::System.Drawing.Color.White;
			this.btnQuery2.Name = "btnQuery2";
			this.btnQuery2.UseVisualStyleBackColor = false;
			this.btnQuery2.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.chkAutoRefresh);
			this.groupBox2.Controls.Add(this.nudCycleSecs);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.chkAutoRefresh, "chkAutoRefresh");
			this.chkAutoRefresh.Name = "chkAutoRefresh";
			this.chkAutoRefresh.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.nudCycleSecs, "nudCycleSecs");
			this.nudCycleSecs.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudCycleSecs;
			int[] array = new int[4];
			array[0] = 60000;
			numericUpDown.Maximum = new decimal(array);
			this.nudCycleSecs.Name = "nudCycleSecs";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudCycleSecs;
			int[] array2 = new int[4];
			array2[0] = 10;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.txtPersonsOutSide, "txtPersonsOutSide");
			this.txtPersonsOutSide.BackColor = global::System.Drawing.Color.White;
			this.txtPersonsOutSide.Name = "txtPersonsOutSide";
			this.txtPersonsOutSide.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtPersons, "txtPersons");
			this.txtPersons.BackColor = global::System.Drawing.Color.White;
			this.txtPersons.Name = "txtPersons";
			this.txtPersons.ReadOnly = true;
			componentResourceManager.ApplyResources(this.nudDays, "nudDays");
			this.nudDays.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudDays;
			int[] array3 = new int[4];
			array3[0] = 6000;
			numericUpDown3.Maximum = new decimal(array3);
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudDays;
			int[] array4 = new int[4];
			array4[0] = 1;
			numericUpDown4.Minimum = new decimal(array4);
			this.nudDays.Name = "nudDays";
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudDays;
			int[] array5 = new int[4];
			array5[0] = 1;
			numericUpDown5.Value = new decimal(array5);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.lblIndex, "lblIndex");
			this.lblIndex.BackColor = global::System.Drawing.Color.Transparent;
			this.lblIndex.ForeColor = global::System.Drawing.Color.White;
			this.lblIndex.Name = "lblIndex";
			componentResourceManager.ApplyResources(this.btnSelectNone, "btnSelectNone");
			this.btnSelectNone.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectNone.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectNone.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectNone.Name = "btnSelectNone";
			this.btnSelectNone.UseVisualStyleBackColor = false;
			this.btnSelectNone.Click += new global::System.EventHandler(this.btnSelectNone_Click);
			componentResourceManager.ApplyResources(this.btnSelectAll, "btnSelectAll");
			this.btnSelectAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectAll.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.UseVisualStyleBackColor = false;
			this.btnSelectAll.Click += new global::System.EventHandler(this.btnSelectAll_Click);
			componentResourceManager.ApplyResources(this.cboZone, "cboZone");
			this.cboZone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZone.FormattingEnabled = true;
			this.cboZone.Name = "cboZone";
			this.cboZone.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this.chkListDoors, "chkListDoors");
			this.chkListDoors.CheckOnClick = true;
			this.chkListDoors.FormattingEnabled = true;
			this.chkListDoors.MultiColumn = true;
			this.chkListDoors.Name = "chkListDoors";
			this.chkListDoors.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.chkListDoors_KeyDown);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnQuery, this.btnPrint, this.btnExportToExcel });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnQuery, "btnQuery");
			this.btnQuery.ForeColor = global::System.Drawing.Color.White;
			this.btnQuery.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Click += new global::System.EventHandler(this.btnQuery_Click);
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
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Controls.Add(this.dgvEnterIn);
			this.tabPage1.ForeColor = global::System.Drawing.Color.White;
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvEnterIn, "dgvEnterIn");
			this.dgvEnterIn.AllowUserToAddRows = false;
			this.dgvEnterIn.AllowUserToDeleteRows = false;
			this.dgvEnterIn.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvEnterIn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvEnterIn.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvEnterIn.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn5, this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.dataGridViewCheckBoxColumn1, this.DeactivateDate });
			this.dgvEnterIn.ContextMenuStrip = this.contextMenuStrip3;
			this.dgvEnterIn.EnableHeadersVisualStyles = false;
			this.dgvEnterIn.Name = "dgvEnterIn";
			this.dgvEnterIn.ReadOnly = true;
			this.dgvEnterIn.RowHeadersVisible = false;
			this.dgvEnterIn.RowTemplate.Height = 23;
			this.dgvEnterIn.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvEnterIn.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvEnterIn_KeyDown);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			this.dataGridViewCheckBoxColumn1.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewCheckBoxColumn1.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.DeactivateDate, "DeactivateDate");
			this.DeactivateDate.Name = "DeactivateDate";
			this.DeactivateDate.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip3, "contextMenuStrip3");
			this.contextMenuStrip3.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.SelectAlltoolStripMenuItem1, this.BatchUpdateSelectedtoolStripMenuItem4, this.DelayValidDatetoolStripMenuItem3, this.ClearSelectedtoolStripMenuItem2, this.deleteSelectedPersonsToolStripMenuItem, this.activeExportToXMLToolStripMenuItem });
			this.contextMenuStrip3.Name = "contextMenuStrip2";
			componentResourceManager.ApplyResources(this.SelectAlltoolStripMenuItem1, "SelectAlltoolStripMenuItem1");
			this.SelectAlltoolStripMenuItem1.Name = "SelectAlltoolStripMenuItem1";
			this.SelectAlltoolStripMenuItem1.Click += new global::System.EventHandler(this.SelectAlltoolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.BatchUpdateSelectedtoolStripMenuItem4, "BatchUpdateSelectedtoolStripMenuItem4");
			this.BatchUpdateSelectedtoolStripMenuItem4.Name = "BatchUpdateSelectedtoolStripMenuItem4";
			this.BatchUpdateSelectedtoolStripMenuItem4.Click += new global::System.EventHandler(this.BatchUpdateSelectedtoolStripMenuItem4_Click);
			componentResourceManager.ApplyResources(this.DelayValidDatetoolStripMenuItem3, "DelayValidDatetoolStripMenuItem3");
			this.DelayValidDatetoolStripMenuItem3.Name = "DelayValidDatetoolStripMenuItem3";
			this.DelayValidDatetoolStripMenuItem3.Click += new global::System.EventHandler(this.DelayValidDateStripMenuItem3_Click);
			componentResourceManager.ApplyResources(this.ClearSelectedtoolStripMenuItem2, "ClearSelectedtoolStripMenuItem2");
			this.ClearSelectedtoolStripMenuItem2.Name = "ClearSelectedtoolStripMenuItem2";
			this.ClearSelectedtoolStripMenuItem2.Click += new global::System.EventHandler(this.DeletePrivilegetoolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.deleteSelectedPersonsToolStripMenuItem, "deleteSelectedPersonsToolStripMenuItem");
			this.deleteSelectedPersonsToolStripMenuItem.Name = "deleteSelectedPersonsToolStripMenuItem";
			this.deleteSelectedPersonsToolStripMenuItem.Click += new global::System.EventHandler(this.deleteSelectedPersonsToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.activeExportToXMLToolStripMenuItem, "activeExportToXMLToolStripMenuItem");
			this.activeExportToXMLToolStripMenuItem.Name = "activeExportToXMLToolStripMenuItem";
			this.activeExportToXMLToolStripMenuItem.Click += new global::System.EventHandler(this.activeExportToXMLToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Controls.Add(this.dgvOutSide);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvOutSide, "dgvOutSide");
			this.dgvOutSide.AllowUserToAddRows = false;
			this.dgvOutSide.AllowUserToDeleteRows = false;
			this.dgvOutSide.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvOutSide.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvOutSide.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvOutSide.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID2, this.dataGridViewTextBoxColumn12, this.dataGridViewTextBoxColumn13, this.dataGridViewTextBoxColumn14, this.dataGridViewTextBoxColumn15, this.dataGridViewTextBoxColumn16, this.dataGridViewCheckBoxColumn2, this.Column1 });
			this.dgvOutSide.ContextMenuStrip = this.contextMenuStrip3;
			this.dgvOutSide.EnableHeadersVisualStyles = false;
			this.dgvOutSide.Name = "dgvOutSide";
			this.dgvOutSide.ReadOnly = true;
			this.dgvOutSide.RowHeadersVisible = false;
			this.dgvOutSide.RowTemplate.Height = 23;
			this.dgvOutSide.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvOutSide.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvOutSide_KeyDown);
			componentResourceManager.ApplyResources(this.ConsumerID2, "ConsumerID2");
			this.ConsumerID2.Name = "ConsumerID2";
			this.ConsumerID2.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn12.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			this.dataGridViewTextBoxColumn14.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
			this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
			this.dataGridViewCheckBoxColumn2.ReadOnly = true;
			this.dataGridViewCheckBoxColumn2.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewCheckBoxColumn2.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Controls.Add(this.dgvGroupSubTotal);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvGroupSubTotal, "dgvGroupSubTotal");
			this.dgvGroupSubTotal.AllowUserToAddRows = false;
			this.dgvGroupSubTotal.AllowUserToDeleteRows = false;
			this.dgvGroupSubTotal.AllowUserToOrderColumns = true;
			this.dgvGroupSubTotal.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroupSubTotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvGroupSubTotal.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvGroupSubTotal.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.SwipeOut });
			this.dgvGroupSubTotal.EnableHeadersVisualStyles = false;
			this.dgvGroupSubTotal.Name = "dgvGroupSubTotal";
			this.dgvGroupSubTotal.ReadOnly = true;
			this.dgvGroupSubTotal.RowHeadersVisible = false;
			this.dgvGroupSubTotal.RowTemplate.Height = 23;
			this.dgvGroupSubTotal.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvGroupSubTotal.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvGroupSubTotal_KeyDown);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.SwipeOut, "SwipeOut");
			this.SwipeOut.Name = "SwipeOut";
			this.SwipeOut.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Controls.Add(this.dgvSwipe);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvSwipe, "dgvSwipe");
			this.dgvSwipe.AllowUserToAddRows = false;
			this.dgvSwipe.AllowUserToDeleteRows = false;
			this.dgvSwipe.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSwipe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvSwipe.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSwipe.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID3, this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11, this.dataGridViewTextBoxColumn17, this.dataGridViewTextBoxColumn18, this.dataGridViewTextBoxColumn19, this.dataGridViewCheckBoxColumn3, this.Column2 });
			this.dgvSwipe.ContextMenuStrip = this.contextMenuStrip3;
			this.dgvSwipe.EnableHeadersVisualStyles = false;
			this.dgvSwipe.Name = "dgvSwipe";
			this.dgvSwipe.ReadOnly = true;
			this.dgvSwipe.RowHeadersVisible = false;
			this.dgvSwipe.RowTemplate.Height = 23;
			this.dgvSwipe.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvSwipe.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvSwipe_KeyDown);
			componentResourceManager.ApplyResources(this.ConsumerID3, "ConsumerID3");
			this.ConsumerID3.Name = "ConsumerID3";
			this.ConsumerID3.ReadOnly = true;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle7;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			this.dataGridViewTextBoxColumn17.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn18, "dataGridViewTextBoxColumn18");
			this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			this.dataGridViewTextBoxColumn18.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn19, "dataGridViewTextBoxColumn19");
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn3, "dataGridViewCheckBoxColumn3");
			this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
			this.dataGridViewCheckBoxColumn3.ReadOnly = true;
			this.dataGridViewCheckBoxColumn3.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewCheckBoxColumn3.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Controls.Add(this.dgvNotSwipe);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvNotSwipe, "dgvNotSwipe");
			this.dgvNotSwipe.AllowUserToAddRows = false;
			this.dgvNotSwipe.AllowUserToDeleteRows = false;
			this.dgvNotSwipe.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvNotSwipe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvNotSwipe.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvNotSwipe.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID5, this.dataGridViewTextBoxColumn20, this.dataGridViewTextBoxColumn21, this.dataGridViewTextBoxColumn22, this.dataGridViewTextBoxColumn23, this.dataGridViewTextBoxColumn24, this.dataGridViewCheckBoxColumn4, this.Column3 });
			this.dgvNotSwipe.ContextMenuStrip = this.contextMenuStrip3;
			this.dgvNotSwipe.EnableHeadersVisualStyles = false;
			this.dgvNotSwipe.Name = "dgvNotSwipe";
			this.dgvNotSwipe.ReadOnly = true;
			this.dgvNotSwipe.RowHeadersVisible = false;
			this.dgvNotSwipe.RowTemplate.Height = 23;
			this.dgvNotSwipe.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvNotSwipe.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvNotSwipe_KeyDown);
			componentResourceManager.ApplyResources(this.ConsumerID5, "ConsumerID5");
			this.ConsumerID5.Name = "ConsumerID5";
			this.ConsumerID5.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn20.DefaultCellStyle = dataGridViewCellStyle9;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn20, "dataGridViewTextBoxColumn20");
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			this.dataGridViewTextBoxColumn20.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn21, "dataGridViewTextBoxColumn21");
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			this.dataGridViewTextBoxColumn22.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn22, "dataGridViewTextBoxColumn22");
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			this.dataGridViewTextBoxColumn22.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn23, "dataGridViewTextBoxColumn23");
			this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
			this.dataGridViewTextBoxColumn23.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn24, "dataGridViewTextBoxColumn24");
			this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
			this.dataGridViewTextBoxColumn24.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn4, "dataGridViewCheckBoxColumn4");
			this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
			this.dataGridViewCheckBoxColumn4.ReadOnly = true;
			this.dataGridViewCheckBoxColumn4.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewCheckBoxColumn4.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			this.backgroundWorker2.WorkerSupportsCancellation = true;
			this.backgroundWorker2.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
			this.backgroundWorker2.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.chkReaderIn, "chkReaderIn");
			this.chkReaderIn.Checked = true;
			this.chkReaderIn.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkReaderIn.ForeColor = global::System.Drawing.Color.White;
			this.chkReaderIn.Name = "chkReaderIn";
			componentResourceManager.ApplyResources(this.chkReaderOut, "chkReaderOut");
			this.chkReaderOut.Checked = true;
			this.chkReaderOut.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkReaderOut.ForeColor = global::System.Drawing.Color.White;
			this.chkReaderOut.Name = "chkReaderOut";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.chkReaderIn);
			this.groupBox1.Controls.Add(this.chkReaderOut);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.btnQuery2);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.txtPersonsOutSide);
			base.Controls.Add(this.txtPersons);
			base.Controls.Add(this.nudDays);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.lblIndex);
			base.Controls.Add(this.btnSelectNone);
			base.Controls.Add(this.btnSelectAll);
			base.Controls.Add(this.cboZone);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.chkListDoors);
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.groupBox1);
			base.Name = "dfrmPersonsInside";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmPersonsInside_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmPersonsInside_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPersonsInside_KeyDown);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCycleSecs).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDays).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvEnterIn).EndInit();
			this.contextMenuStrip3.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvOutSide).EndInit();
			this.tabPage3.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroupSubTotal).EndInit();
			this.tabPage4.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSwipe).EndInit();
			this.tabPage5.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvNotSwipe).EndInit();
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002108 RID: 8456
		public global::System.Windows.Forms.ComboBox cboZone;

		// Token: 0x0400211B RID: 8475
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400211C RID: 8476
		private global::System.Windows.Forms.ToolStripMenuItem activeExportToXMLToolStripMenuItem;

		// Token: 0x0400211D RID: 8477
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x0400211E RID: 8478
		private global::System.ComponentModel.BackgroundWorker backgroundWorker2;

		// Token: 0x0400211F RID: 8479
		private global::System.Windows.Forms.ToolStripMenuItem BatchUpdateSelectedtoolStripMenuItem4;

		// Token: 0x04002120 RID: 8480
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04002121 RID: 8481
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04002122 RID: 8482
		private global::System.Windows.Forms.ToolStripButton btnQuery;

		// Token: 0x04002123 RID: 8483
		private global::System.Windows.Forms.Button btnQuery2;

		// Token: 0x04002124 RID: 8484
		private global::System.Windows.Forms.Button btnSelectAll;

		// Token: 0x04002125 RID: 8485
		private global::System.Windows.Forms.Button btnSelectNone;

		// Token: 0x04002126 RID: 8486
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04002127 RID: 8487
		private global::System.Windows.Forms.CheckBox chkAutoRefresh;

		// Token: 0x04002128 RID: 8488
		private global::System.Windows.Forms.CheckedListBox chkListDoors;

		// Token: 0x04002129 RID: 8489
		private global::System.Windows.Forms.ToolStripMenuItem ClearSelectedtoolStripMenuItem2;

		// Token: 0x0400212A RID: 8490
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x0400212B RID: 8491
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x0400212C RID: 8492
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x0400212D RID: 8493
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x0400212E RID: 8494
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID2;

		// Token: 0x0400212F RID: 8495
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID3;

		// Token: 0x04002130 RID: 8496
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID5;

		// Token: 0x04002131 RID: 8497
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip3;

		// Token: 0x04002132 RID: 8498
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002133 RID: 8499
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;

		// Token: 0x04002134 RID: 8500
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;

		// Token: 0x04002135 RID: 8501
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;

		// Token: 0x04002136 RID: 8502
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002137 RID: 8503
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		// Token: 0x04002138 RID: 8504
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		// Token: 0x04002139 RID: 8505
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

		// Token: 0x0400213A RID: 8506
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

		// Token: 0x0400213B RID: 8507
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

		// Token: 0x0400213C RID: 8508
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

		// Token: 0x0400213D RID: 8509
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

		// Token: 0x0400213E RID: 8510
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

		// Token: 0x0400213F RID: 8511
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;

		// Token: 0x04002140 RID: 8512
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;

		// Token: 0x04002141 RID: 8513
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002142 RID: 8514
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;

		// Token: 0x04002143 RID: 8515
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;

		// Token: 0x04002144 RID: 8516
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;

		// Token: 0x04002145 RID: 8517
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;

		// Token: 0x04002146 RID: 8518
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;

		// Token: 0x04002147 RID: 8519
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002148 RID: 8520
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002149 RID: 8521
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		// Token: 0x0400214A RID: 8522
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x0400214B RID: 8523
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x0400214C RID: 8524
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x0400214D RID: 8525
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x0400214E RID: 8526
		private global::System.Windows.Forms.DataGridViewTextBoxColumn DeactivateDate;

		// Token: 0x0400214F RID: 8527
		private global::System.Windows.Forms.ToolStripMenuItem DelayValidDatetoolStripMenuItem3;

		// Token: 0x04002150 RID: 8528
		private global::System.Windows.Forms.ToolStripMenuItem deleteSelectedPersonsToolStripMenuItem;

		// Token: 0x04002151 RID: 8529
		private global::System.Windows.Forms.DataGridView dgvEnterIn;

		// Token: 0x04002152 RID: 8530
		private global::System.Windows.Forms.DataGridView dgvGroupSubTotal;

		// Token: 0x04002153 RID: 8531
		private global::System.Windows.Forms.DataGridView dgvNotSwipe;

		// Token: 0x04002154 RID: 8532
		private global::System.Windows.Forms.DataGridView dgvOutSide;

		// Token: 0x04002155 RID: 8533
		private global::System.Windows.Forms.DataGridView dgvSwipe;

		// Token: 0x04002156 RID: 8534
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002157 RID: 8535
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04002158 RID: 8536
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002159 RID: 8537
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400215A RID: 8538
		private global::System.Windows.Forms.Label label25;

		// Token: 0x0400215B RID: 8539
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400215C RID: 8540
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400215D RID: 8541
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400215E RID: 8542
		private global::System.Windows.Forms.Label label6;

		// Token: 0x0400215F RID: 8543
		private global::System.Windows.Forms.Label lblIndex;

		// Token: 0x04002160 RID: 8544
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x04002161 RID: 8545
		private global::System.Windows.Forms.ToolStripMenuItem SelectAlltoolStripMenuItem1;

		// Token: 0x04002162 RID: 8546
		private global::System.Windows.Forms.DataGridViewTextBoxColumn SwipeOut;

		// Token: 0x04002163 RID: 8547
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002164 RID: 8548
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002165 RID: 8549
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002166 RID: 8550
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04002167 RID: 8551
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04002168 RID: 8552
		private global::System.Windows.Forms.TabPage tabPage5;

		// Token: 0x04002169 RID: 8553
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400216B RID: 8555
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x0400216C RID: 8556
		private global::System.Windows.Forms.TextBox txtPersons;

		// Token: 0x0400216D RID: 8557
		private global::System.Windows.Forms.TextBox txtPersonsOutSide;

		// Token: 0x0400216E RID: 8558
		internal global::System.Windows.Forms.CheckBox chkReaderIn;

		// Token: 0x0400216F RID: 8559
		internal global::System.Windows.Forms.CheckBox chkReaderOut;

		// Token: 0x04002170 RID: 8560
		internal global::System.Windows.Forms.NumericUpDown nudCycleSecs;

		// Token: 0x04002171 RID: 8561
		internal global::System.Windows.Forms.NumericUpDown nudDays;
	}
}
