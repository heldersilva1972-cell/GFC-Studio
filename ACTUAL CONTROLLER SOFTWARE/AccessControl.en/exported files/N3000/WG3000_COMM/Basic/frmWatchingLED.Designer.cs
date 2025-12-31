namespace WG3000_COMM.Basic
{
	// Token: 0x02000057 RID: 87
	public partial class frmWatchingLED : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x000B94AC File Offset: 0x000B84AC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000B94CC File Offset: 0x000B84CC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmWatchingLED));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.flowLayoutPanel3 = new global::System.Windows.Forms.FlowLayoutPanel();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnReset = new global::System.Windows.Forms.Button();
			this.btnAddTempCard = new global::System.Windows.Forms.Button();
			this.dataGridViewSelected = new global::System.Windows.Forms.DataGridView();
			this.Column6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn18 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn19 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn21 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvMainGroups = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn15 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grpWEBEnabled = new global::System.Windows.Forms.GroupBox();
			this.optWEBEnabled = new global::System.Windows.Forms.RadioButton();
			this.optWEBDisable = new global::System.Windows.Forms.RadioButton();
			this.label3 = new global::System.Windows.Forms.Label();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.btnGroupDown = new global::System.Windows.Forms.Button();
			this.btnGroupUp = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.checkBox4 = new global::System.Windows.Forms.CheckBox();
			this.checkBox3 = new global::System.Windows.Forms.CheckBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.checkBox2 = new global::System.Windows.Forms.CheckBox();
			this.chkName = new global::System.Windows.Forms.CheckBox();
			this.chkCard = new global::System.Windows.Forms.CheckBox();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.modeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.defaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.userGongzhongTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe0 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe3 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPageSwipe5 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayAllPersonsInDoorssecToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.deactiveDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ledSwitchCycleToolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ledSwitchCycleToolStripMenuItem2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ledSwitchCycleToolStripMenuItem3 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ledSwitchCycleToolStripMenuItem4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ledSwitchCycleToolStripMenuItem5 = new global::System.Windows.Forms.ToolStripMenuItem();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridViewSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMainGroups).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.grpWEBEnabled.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.flowLayoutPanel3.BackColor = global::System.Drawing.Color.Blue;
			this.flowLayoutPanel3.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.toolTip1.SetToolTip(this.flowLayoutPanel3, componentResourceManager.GetString("flowLayoutPanel3.ToolTip"));
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.toolTip1.SetToolTip(this.btnAddAllDoors, componentResourceManager.GetString("btnAddAllDoors.ToolTip"));
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnReset, "btnReset");
			this.btnReset.BackColor = global::System.Drawing.Color.Transparent;
			this.btnReset.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnReset.ForeColor = global::System.Drawing.Color.White;
			this.btnReset.Name = "btnReset";
			this.toolTip1.SetToolTip(this.btnReset, componentResourceManager.GetString("btnReset.ToolTip"));
			this.btnReset.UseVisualStyleBackColor = false;
			this.btnReset.Click += new global::System.EventHandler(this.btnReset_Click);
			componentResourceManager.ApplyResources(this.btnAddTempCard, "btnAddTempCard");
			this.btnAddTempCard.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddTempCard.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddTempCard.ForeColor = global::System.Drawing.Color.White;
			this.btnAddTempCard.Name = "btnAddTempCard";
			this.toolTip1.SetToolTip(this.btnAddTempCard, componentResourceManager.GetString("btnAddTempCard.ToolTip"));
			this.btnAddTempCard.UseVisualStyleBackColor = false;
			this.btnAddTempCard.Click += new global::System.EventHandler(this.btnAddTempCard_Click);
			componentResourceManager.ApplyResources(this.dataGridViewSelected, "dataGridViewSelected");
			this.dataGridViewSelected.AllowUserToAddRows = false;
			this.dataGridViewSelected.AllowUserToDeleteRows = false;
			this.dataGridViewSelected.AllowUserToOrderColumns = true;
			this.dataGridViewSelected.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridViewSelected.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridViewSelected.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.Column6, this.dataGridViewTextBoxColumn17, this.dataGridViewTextBoxColumn18, this.dataGridViewTextBoxColumn19, this.dataGridViewTextBoxColumn20, this.dataGridViewTextBoxColumn21, this.dataGridViewTextBoxColumn22, this.Column7 });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewSelected.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewSelected.EnableHeadersVisualStyles = false;
			this.dataGridViewSelected.Name = "dataGridViewSelected";
			this.dataGridViewSelected.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewSelected.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewSelected.RowTemplate.Height = 23;
			this.dataGridViewSelected.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dataGridViewSelected, componentResourceManager.GetString("dataGridViewSelected.ToolTip"));
			componentResourceManager.ApplyResources(this.Column6, "Column6");
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			this.Column6.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			this.dataGridViewTextBoxColumn18.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn18, "dataGridViewTextBoxColumn18");
			this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			this.dataGridViewTextBoxColumn18.ReadOnly = true;
			this.dataGridViewTextBoxColumn18.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn19, "dataGridViewTextBoxColumn19");
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn20, "dataGridViewTextBoxColumn20");
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			this.dataGridViewTextBoxColumn20.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn21, "dataGridViewTextBoxColumn21");
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn22, "dataGridViewTextBoxColumn22");
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			this.dataGridViewTextBoxColumn22.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column7, "Column7");
			this.Column7.Name = "Column7";
			this.Column7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvMainGroups, "dgvMainGroups");
			this.dgvMainGroups.AllowUserToAddRows = false;
			this.dgvMainGroups.AllowUserToDeleteRows = false;
			this.dgvMainGroups.AllowUserToOrderColumns = true;
			this.dgvMainGroups.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMainGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvMainGroups.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMainGroups.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn11, this.dataGridViewTextBoxColumn12, this.dataGridViewTextBoxColumn13, this.dataGridViewTextBoxColumn14, this.dataGridViewTextBoxColumn15, this.dataGridViewTextBoxColumn16 });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvMainGroups.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvMainGroups.EnableHeadersVisualStyles = false;
			this.dgvMainGroups.Name = "dgvMainGroups";
			this.dgvMainGroups.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMainGroups.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvMainGroups.RowTemplate.Height = 23;
			this.dgvMainGroups.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvMainGroups, componentResourceManager.GetString("dgvMainGroups.ToolTip"));
			this.dgvMainGroups.CellDoubleClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMainGroups_CellDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			this.dataGridViewTextBoxColumn12.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected, this.f_ZoneID, this.dataGridViewTextBoxColumn10, this.f_ControlSegName1 });
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dataGridView1, componentResourceManager.GetString("dataGridView1.ToolTip"));
			this.dataGridView1.CellDoubleClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			this.dataGridViewTextBoxColumn9.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected, "f_Selected");
			this.f_Selected.Name = "f_Selected";
			this.f_Selected.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ZoneID, "f_ZoneID");
			this.f_ZoneID.Name = "f_ZoneID";
			this.f_ZoneID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlSegName1, "f_ControlSegName1");
			this.f_ControlSegName1.Name = "f_ControlSegName1";
			this.f_ControlSegName1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.grpWEBEnabled, "grpWEBEnabled");
			this.grpWEBEnabled.Controls.Add(this.optWEBEnabled);
			this.grpWEBEnabled.Controls.Add(this.optWEBDisable);
			this.grpWEBEnabled.ForeColor = global::System.Drawing.Color.White;
			this.grpWEBEnabled.Name = "grpWEBEnabled";
			this.grpWEBEnabled.TabStop = false;
			this.toolTip1.SetToolTip(this.grpWEBEnabled, componentResourceManager.GetString("grpWEBEnabled.ToolTip"));
			componentResourceManager.ApplyResources(this.optWEBEnabled, "optWEBEnabled");
			this.optWEBEnabled.ForeColor = global::System.Drawing.Color.White;
			this.optWEBEnabled.Name = "optWEBEnabled";
			this.toolTip1.SetToolTip(this.optWEBEnabled, componentResourceManager.GetString("optWEBEnabled.ToolTip"));
			this.optWEBEnabled.UseVisualStyleBackColor = true;
			this.optWEBEnabled.CheckedChanged += new global::System.EventHandler(this.optWEBEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.optWEBDisable, "optWEBDisable");
			this.optWEBDisable.Checked = true;
			this.optWEBDisable.ForeColor = global::System.Drawing.Color.White;
			this.optWEBDisable.Name = "optWEBDisable";
			this.optWEBDisable.TabStop = true;
			this.toolTip1.SetToolTip(this.optWEBDisable, componentResourceManager.GetString("optWEBDisable.ToolTip"));
			this.optWEBDisable.UseVisualStyleBackColor = true;
			this.optWEBDisable.CheckedChanged += new global::System.EventHandler(this.optWEBDisable_CheckedChanged);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.toolTip1.SetToolTip(this.cmdCancel, componentResourceManager.GetString("cmdCancel.ToolTip"));
			this.cmdCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.toolTip1.SetToolTip(this.cmdOK, componentResourceManager.GetString("cmdOK.ToolTip"));
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this.btnGroupDown, "btnGroupDown");
			this.btnGroupDown.BackColor = global::System.Drawing.Color.Transparent;
			this.btnGroupDown.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnGroupDown.ForeColor = global::System.Drawing.Color.White;
			this.btnGroupDown.Name = "btnGroupDown";
			this.toolTip1.SetToolTip(this.btnGroupDown, componentResourceManager.GetString("btnGroupDown.ToolTip"));
			this.btnGroupDown.UseVisualStyleBackColor = false;
			this.btnGroupDown.Click += new global::System.EventHandler(this.btnGroupDown_Click);
			componentResourceManager.ApplyResources(this.btnGroupUp, "btnGroupUp");
			this.btnGroupUp.BackColor = global::System.Drawing.Color.Transparent;
			this.btnGroupUp.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnGroupUp.ForeColor = global::System.Drawing.Color.White;
			this.btnGroupUp.Name = "btnGroupUp";
			this.toolTip1.SetToolTip(this.btnGroupUp, componentResourceManager.GetString("btnGroupUp.ToolTip"));
			this.btnGroupUp.UseVisualStyleBackColor = false;
			this.btnGroupUp.Click += new global::System.EventHandler(this.btnGroupUp_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.AutoEllipsis = true;
			this.label1.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.ForeColor = global::System.Drawing.Color.Yellow;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.checkBox4);
			this.groupBox1.Controls.Add(this.checkBox3);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.chkName);
			this.groupBox1.Controls.Add(this.chkCard);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.textBox2, "textBox2");
			this.textBox2.Name = "textBox2";
			this.toolTip1.SetToolTip(this.textBox2, componentResourceManager.GetString("textBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBox4, "checkBox4");
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox4.ForeColor = global::System.Drawing.Color.White;
			this.checkBox4.Name = "checkBox4";
			this.toolTip1.SetToolTip(this.checkBox4, componentResourceManager.GetString("checkBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBox3, "checkBox3");
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox3.ForeColor = global::System.Drawing.Color.White;
			this.checkBox3.Name = "checkBox3";
			this.toolTip1.SetToolTip(this.checkBox3, componentResourceManager.GetString("checkBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox1.ForeColor = global::System.Drawing.Color.White;
			this.checkBox1.Name = "checkBox1";
			this.toolTip1.SetToolTip(this.checkBox1, componentResourceManager.GetString("checkBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBox2, "checkBox2");
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox2.ForeColor = global::System.Drawing.Color.White;
			this.checkBox2.Name = "checkBox2";
			this.toolTip1.SetToolTip(this.checkBox2, componentResourceManager.GetString("checkBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.chkName, "chkName");
			this.chkName.Checked = true;
			this.chkName.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkName.ForeColor = global::System.Drawing.Color.White;
			this.chkName.Name = "chkName";
			this.toolTip1.SetToolTip(this.chkName, componentResourceManager.GetString("chkName.ToolTip"));
			componentResourceManager.ApplyResources(this.chkCard, "chkCard");
			this.chkCard.Checked = true;
			this.chkCard.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkCard.ForeColor = global::System.Drawing.Color.White;
			this.chkCard.Name = "chkCard";
			this.toolTip1.SetToolTip(this.chkCard, componentResourceManager.GetString("chkCard.ToolTip"));
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.toolTip1.SetToolTip(this.textBox1, componentResourceManager.GetString("textBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.modeToolStripMenuItem, this.toolStripMenuItem1, this.displayAllPersonsInDoorssecToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.modeToolStripMenuItem, "modeToolStripMenuItem");
			this.modeToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.defaultToolStripMenuItem, this.userGongzhongTimeToolStripMenuItem });
			this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
			componentResourceManager.ApplyResources(this.defaultToolStripMenuItem, "defaultToolStripMenuItem");
			this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			this.defaultToolStripMenuItem.Click += new global::System.EventHandler(this.defaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.userGongzhongTimeToolStripMenuItem, "userGongzhongTimeToolStripMenuItem");
			this.userGongzhongTimeToolStripMenuItem.Name = "userGongzhongTimeToolStripMenuItem";
			this.userGongzhongTimeToolStripMenuItem.Click += new global::System.EventHandler(this.userGongzhongTimeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripMenuItemPageSwipe0, this.toolStripMenuItemPageSwipe1, this.toolStripMenuItemPageSwipe2, this.toolStripMenuItemPageSwipe3, this.toolStripMenuItemPageSwipe4, this.toolStripMenuItemPageSwipe5 });
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe0, "toolStripMenuItemPageSwipe0");
			this.toolStripMenuItemPageSwipe0.Name = "toolStripMenuItemPageSwipe0";
			this.toolStripMenuItemPageSwipe0.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe1, "toolStripMenuItemPageSwipe1");
			this.toolStripMenuItemPageSwipe1.Name = "toolStripMenuItemPageSwipe1";
			this.toolStripMenuItemPageSwipe1.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe2, "toolStripMenuItemPageSwipe2");
			this.toolStripMenuItemPageSwipe2.Name = "toolStripMenuItemPageSwipe2";
			this.toolStripMenuItemPageSwipe2.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe3, "toolStripMenuItemPageSwipe3");
			this.toolStripMenuItemPageSwipe3.Name = "toolStripMenuItemPageSwipe3";
			this.toolStripMenuItemPageSwipe3.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe4, "toolStripMenuItemPageSwipe4");
			this.toolStripMenuItemPageSwipe4.Name = "toolStripMenuItemPageSwipe4";
			this.toolStripMenuItemPageSwipe4.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItemPageSwipe5, "toolStripMenuItemPageSwipe5");
			this.toolStripMenuItemPageSwipe5.Name = "toolStripMenuItemPageSwipe5";
			this.toolStripMenuItemPageSwipe5.Click += new global::System.EventHandler(this.toolStripMenuItemPageSwipe0_Click);
			componentResourceManager.ApplyResources(this.displayAllPersonsInDoorssecToolStripMenuItem, "displayAllPersonsInDoorssecToolStripMenuItem");
			this.displayAllPersonsInDoorssecToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.deactiveDefaultToolStripMenuItem, this.ledSwitchCycleToolStripMenuItem1, this.ledSwitchCycleToolStripMenuItem2, this.ledSwitchCycleToolStripMenuItem3, this.ledSwitchCycleToolStripMenuItem4, this.ledSwitchCycleToolStripMenuItem5 });
			this.displayAllPersonsInDoorssecToolStripMenuItem.Name = "displayAllPersonsInDoorssecToolStripMenuItem";
			componentResourceManager.ApplyResources(this.deactiveDefaultToolStripMenuItem, "deactiveDefaultToolStripMenuItem");
			this.deactiveDefaultToolStripMenuItem.Name = "deactiveDefaultToolStripMenuItem";
			this.deactiveDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ledSwitchCycleToolStripMenuItem1, "ledSwitchCycleToolStripMenuItem1");
			this.ledSwitchCycleToolStripMenuItem1.Name = "ledSwitchCycleToolStripMenuItem1";
			this.ledSwitchCycleToolStripMenuItem1.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ledSwitchCycleToolStripMenuItem2, "ledSwitchCycleToolStripMenuItem2");
			this.ledSwitchCycleToolStripMenuItem2.Name = "ledSwitchCycleToolStripMenuItem2";
			this.ledSwitchCycleToolStripMenuItem2.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ledSwitchCycleToolStripMenuItem3, "ledSwitchCycleToolStripMenuItem3");
			this.ledSwitchCycleToolStripMenuItem3.Name = "ledSwitchCycleToolStripMenuItem3";
			this.ledSwitchCycleToolStripMenuItem3.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ledSwitchCycleToolStripMenuItem4, "ledSwitchCycleToolStripMenuItem4");
			this.ledSwitchCycleToolStripMenuItem4.Name = "ledSwitchCycleToolStripMenuItem4";
			this.ledSwitchCycleToolStripMenuItem4.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ledSwitchCycleToolStripMenuItem5, "ledSwitchCycleToolStripMenuItem5");
			this.ledSwitchCycleToolStripMenuItem5.Name = "ledSwitchCycleToolStripMenuItem5";
			this.ledSwitchCycleToolStripMenuItem5.Click += new global::System.EventHandler(this.ledSwitchCycleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.btnAddAllDoors);
			base.Controls.Add(this.btnReset);
			base.Controls.Add(this.btnAddTempCard);
			base.Controls.Add(this.dataGridViewSelected);
			base.Controls.Add(this.dgvMainGroups);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.grpWEBEnabled);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.btnGroupDown);
			base.Controls.Add(this.btnGroupUp);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.textBox1);
			base.MinimizeBox = false;
			base.Name = "frmWatchingLED";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmWatchingMoreRecords_FormClosing);
			base.Load += new global::System.EventHandler(this.frmWatchingMoreRecords_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridViewSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMainGroups).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.grpWEBEnabled.ResumeLayout(false);
			this.grpWEBEnabled.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000C19 RID: 3097
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000C1A RID: 3098
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04000C1B RID: 3099
		private global::System.Windows.Forms.Button btnAddTempCard;

		// Token: 0x04000C1C RID: 3100
		private global::System.Windows.Forms.Button btnGroupDown;

		// Token: 0x04000C1D RID: 3101
		private global::System.Windows.Forms.Button btnGroupUp;

		// Token: 0x04000C1E RID: 3102
		private global::System.Windows.Forms.Button btnReset;

		// Token: 0x04000C1F RID: 3103
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column6;

		// Token: 0x04000C20 RID: 3104
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column7;

		// Token: 0x04000C21 RID: 3105
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000C22 RID: 3106
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000C23 RID: 3107
		private global::System.Windows.Forms.DataGridView dataGridViewSelected;

		// Token: 0x04000C24 RID: 3108
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		// Token: 0x04000C25 RID: 3109
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		// Token: 0x04000C26 RID: 3110
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

		// Token: 0x04000C27 RID: 3111
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

		// Token: 0x04000C28 RID: 3112
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

		// Token: 0x04000C29 RID: 3113
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

		// Token: 0x04000C2A RID: 3114
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

		// Token: 0x04000C2B RID: 3115
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

		// Token: 0x04000C2C RID: 3116
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;

		// Token: 0x04000C2D RID: 3117
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;

		// Token: 0x04000C2E RID: 3118
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;

		// Token: 0x04000C2F RID: 3119
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;

		// Token: 0x04000C30 RID: 3120
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;

		// Token: 0x04000C31 RID: 3121
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04000C32 RID: 3122
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04000C33 RID: 3123
		private global::System.Windows.Forms.ToolStripMenuItem deactiveDefaultToolStripMenuItem;

		// Token: 0x04000C34 RID: 3124
		private global::System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;

		// Token: 0x04000C35 RID: 3125
		private global::System.Windows.Forms.DataGridView dgvMainGroups;

		// Token: 0x04000C36 RID: 3126
		private global::System.Windows.Forms.ToolStripMenuItem displayAllPersonsInDoorssecToolStripMenuItem;

		// Token: 0x04000C37 RID: 3127
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName1;

		// Token: 0x04000C38 RID: 3128
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04000C39 RID: 3129
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x04000C3A RID: 3130
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;

		// Token: 0x04000C3B RID: 3131
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000C3C RID: 3132
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000C3D RID: 3133
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000C3E RID: 3134
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000C3F RID: 3135
		private global::System.Windows.Forms.ToolStripMenuItem ledSwitchCycleToolStripMenuItem1;

		// Token: 0x04000C40 RID: 3136
		private global::System.Windows.Forms.ToolStripMenuItem ledSwitchCycleToolStripMenuItem2;

		// Token: 0x04000C41 RID: 3137
		private global::System.Windows.Forms.ToolStripMenuItem ledSwitchCycleToolStripMenuItem3;

		// Token: 0x04000C42 RID: 3138
		private global::System.Windows.Forms.ToolStripMenuItem ledSwitchCycleToolStripMenuItem4;

		// Token: 0x04000C43 RID: 3139
		private global::System.Windows.Forms.ToolStripMenuItem ledSwitchCycleToolStripMenuItem5;

		// Token: 0x04000C44 RID: 3140
		private global::System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;

		// Token: 0x04000C45 RID: 3141
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000C46 RID: 3142
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x04000C47 RID: 3143
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x04000C48 RID: 3144
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe0;

		// Token: 0x04000C49 RID: 3145
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe1;

		// Token: 0x04000C4A RID: 3146
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe2;

		// Token: 0x04000C4B RID: 3147
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe3;

		// Token: 0x04000C4C RID: 3148
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe4;

		// Token: 0x04000C4D RID: 3149
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPageSwipe5;

		// Token: 0x04000C4E RID: 3150
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04000C4F RID: 3151
		private global::System.Windows.Forms.ToolStripMenuItem userGongzhongTimeToolStripMenuItem;

		// Token: 0x04000C50 RID: 3152
		internal global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04000C51 RID: 3153
		internal global::System.Windows.Forms.CheckBox checkBox2;

		// Token: 0x04000C52 RID: 3154
		internal global::System.Windows.Forms.CheckBox checkBox3;

		// Token: 0x04000C53 RID: 3155
		internal global::System.Windows.Forms.CheckBox checkBox4;

		// Token: 0x04000C54 RID: 3156
		internal global::System.Windows.Forms.CheckBox chkCard;

		// Token: 0x04000C55 RID: 3157
		internal global::System.Windows.Forms.CheckBox chkName;

		// Token: 0x04000C56 RID: 3158
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x04000C57 RID: 3159
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x04000C58 RID: 3160
		public global::System.Windows.Forms.GroupBox grpWEBEnabled;

		// Token: 0x04000C59 RID: 3161
		public global::System.Windows.Forms.RadioButton optWEBDisable;

		// Token: 0x04000C5A RID: 3162
		public global::System.Windows.Forms.RadioButton optWEBEnabled;
	}
}
