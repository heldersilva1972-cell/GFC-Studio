namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000314 RID: 788
	public partial class dfrmCheckAccessConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001838 RID: 6200 RVA: 0x001F76FB File Offset: 0x001F66FB
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x001F771C File Offset: 0x001F671C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PCCheck.dfrmCheckAccessConfigure));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.moreCardsInsideToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MoreCardsInsidetoolStripComboBox2 = new global::System.Windows.Forms.ToolStripComboBox();
			this.outsideSwipingToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripComboBox1 = new global::System.Windows.Forms.ToolStripComboBox();
			this.videoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripComboBox2 = new global::System.Windows.Forms.ToolStripComboBox();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip2 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.doorsMoreCardsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.oneCardToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.twoCardToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.threeCardToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.fourCardsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.fiveCardsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sixCardsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearAllCardsConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.displayConfigureToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnOption = new global::System.Windows.Forms.Button();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.dgvGroups = new global::System.Windows.Forms.DataGridView();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.GroupName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_active = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.MoreCards = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SoundFile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			this.contextMenuStrip2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroups).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.moreCardsInsideToolStripMenuItem, this.outsideSwipingToolStripMenuItem, this.videoToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.moreCardsInsideToolStripMenuItem, "moreCardsInsideToolStripMenuItem");
			this.moreCardsInsideToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.MoreCardsInsidetoolStripComboBox2 });
			this.moreCardsInsideToolStripMenuItem.Name = "moreCardsInsideToolStripMenuItem";
			componentResourceManager.ApplyResources(this.MoreCardsInsidetoolStripComboBox2, "MoreCardsInsidetoolStripComboBox2");
			this.MoreCardsInsidetoolStripComboBox2.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("MoreCardsInsidetoolStripComboBox2.Items"),
				componentResourceManager.GetString("MoreCardsInsidetoolStripComboBox2.Items1"),
				componentResourceManager.GetString("MoreCardsInsidetoolStripComboBox2.Items2"),
				componentResourceManager.GetString("MoreCardsInsidetoolStripComboBox2.Items3"),
				componentResourceManager.GetString("MoreCardsInsidetoolStripComboBox2.Items4")
			});
			this.MoreCardsInsidetoolStripComboBox2.Name = "MoreCardsInsidetoolStripComboBox2";
			componentResourceManager.ApplyResources(this.outsideSwipingToolStripMenuItem, "outsideSwipingToolStripMenuItem");
			this.outsideSwipingToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripComboBox1 });
			this.outsideSwipingToolStripMenuItem.Name = "outsideSwipingToolStripMenuItem";
			componentResourceManager.ApplyResources(this.toolStripComboBox1, "toolStripComboBox1");
			this.toolStripComboBox1.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("toolStripComboBox1.Items"),
				componentResourceManager.GetString("toolStripComboBox1.Items1")
			});
			this.toolStripComboBox1.Name = "toolStripComboBox1";
			this.toolStripComboBox1.Click += new global::System.EventHandler(this.toolStripComboBox1_Click);
			componentResourceManager.ApplyResources(this.videoToolStripMenuItem, "videoToolStripMenuItem");
			this.videoToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripComboBox2 });
			this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
			componentResourceManager.ApplyResources(this.toolStripComboBox2, "toolStripComboBox2");
			this.toolStripComboBox2.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("toolStripComboBox2.Items"),
				componentResourceManager.GetString("toolStripComboBox2.Items1")
			});
			this.toolStripComboBox2.Name = "toolStripComboBox2";
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.cbof_ZoneID);
			this.groupBox2.Controls.Add(this.label25);
			this.groupBox2.Controls.Add(this.dgvSelectedDoors);
			this.groupBox2.Controls.Add(this.dgvDoors);
			this.groupBox2.Controls.Add(this.btnDelAllDoors);
			this.groupBox2.Controls.Add(this.btnDelOneDoor);
			this.groupBox2.Controls.Add(this.btnAddOneDoor);
			this.groupBox2.Controls.Add(this.btnAddAllDoors);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox2, componentResourceManager.GetString("groupBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.toolTip1.SetToolTip(this.cbof_ZoneID, componentResourceManager.GetString("cbof_ZoneID.ToolTip"));
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			this.toolTip1.SetToolTip(this.label25, componentResourceManager.GetString("label25.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.AllowUserToOrderColumns = true;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.Column1 });
			this.dgvSelectedDoors.ContextMenuStrip = this.contextMenuStrip2;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSelectedDoors.RowTemplate.Height = 23;
			this.dgvSelectedDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedDoors, componentResourceManager.GetString("dgvSelectedDoors.ToolTip"));
			this.dgvSelectedDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected2, "f_Selected2");
			this.f_Selected2.Name = "f_Selected2";
			this.f_Selected2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
			this.contextMenuStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.doorsMoreCardsToolStripMenuItem, this.displayConfigureToolStripMenuItem });
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.toolTip1.SetToolTip(this.contextMenuStrip2, componentResourceManager.GetString("contextMenuStrip2.ToolTip"));
			componentResourceManager.ApplyResources(this.doorsMoreCardsToolStripMenuItem, "doorsMoreCardsToolStripMenuItem");
			this.doorsMoreCardsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.oneCardToolStripMenuItem, this.twoCardToolStripMenuItem, this.threeCardToolStripMenuItem, this.fourCardsToolStripMenuItem, this.fiveCardsToolStripMenuItem, this.sixCardsToolStripMenuItem, this.clearAllCardsConfigureToolStripMenuItem });
			this.doorsMoreCardsToolStripMenuItem.Name = "doorsMoreCardsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.oneCardToolStripMenuItem, "oneCardToolStripMenuItem");
			this.oneCardToolStripMenuItem.Name = "oneCardToolStripMenuItem";
			this.oneCardToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.twoCardToolStripMenuItem, "twoCardToolStripMenuItem");
			this.twoCardToolStripMenuItem.Name = "twoCardToolStripMenuItem";
			this.twoCardToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.threeCardToolStripMenuItem, "threeCardToolStripMenuItem");
			this.threeCardToolStripMenuItem.Name = "threeCardToolStripMenuItem";
			this.threeCardToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.fourCardsToolStripMenuItem, "fourCardsToolStripMenuItem");
			this.fourCardsToolStripMenuItem.Name = "fourCardsToolStripMenuItem";
			this.fourCardsToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.fiveCardsToolStripMenuItem, "fiveCardsToolStripMenuItem");
			this.fiveCardsToolStripMenuItem.Name = "fiveCardsToolStripMenuItem";
			this.fiveCardsToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.sixCardsToolStripMenuItem, "sixCardsToolStripMenuItem");
			this.sixCardsToolStripMenuItem.Name = "sixCardsToolStripMenuItem";
			this.sixCardsToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.clearAllCardsConfigureToolStripMenuItem, "clearAllCardsConfigureToolStripMenuItem");
			this.clearAllCardsConfigureToolStripMenuItem.Name = "clearAllCardsConfigureToolStripMenuItem";
			this.clearAllCardsConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.oneCardToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.displayConfigureToolStripMenuItem, "displayConfigureToolStripMenuItem");
			this.displayConfigureToolStripMenuItem.Name = "displayConfigureToolStripMenuItem";
			this.displayConfigureToolStripMenuItem.Click += new global::System.EventHandler(this.displayConfigureToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.AllowUserToOrderColumns = true;
			this.dgvDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.f_ZoneID });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvDoors.RowTemplate.Height = 23;
			this.dgvDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvDoors, componentResourceManager.GetString("dgvDoors.ToolTip"));
			this.dgvDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected, "f_Selected");
			this.f_Selected.Name = "f_Selected";
			this.f_Selected.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ZoneID, "f_ZoneID");
			this.f_ZoneID.Name = "f_ZoneID";
			this.f_ZoneID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
			this.btnDelAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllDoors.Name = "btnDelAllDoors";
			this.toolTip1.SetToolTip(this.btnDelAllDoors, componentResourceManager.GetString("btnDelAllDoors.ToolTip"));
			this.btnDelAllDoors.UseVisualStyleBackColor = true;
			this.btnDelAllDoors.Click += new global::System.EventHandler(this.btnDelAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.toolTip1.SetToolTip(this.btnDelOneDoor, componentResourceManager.GetString("btnDelOneDoor.ToolTip"));
			this.btnDelOneDoor.UseVisualStyleBackColor = true;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
			this.btnAddOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneDoor.Name = "btnAddOneDoor";
			this.toolTip1.SetToolTip(this.btnAddOneDoor, componentResourceManager.GetString("btnAddOneDoor.ToolTip"));
			this.btnAddOneDoor.UseVisualStyleBackColor = true;
			this.btnAddOneDoor.Click += new global::System.EventHandler(this.btnAddOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.toolTip1.SetToolTip(this.btnAddAllDoors, componentResourceManager.GetString("btnAddAllDoors.ToolTip"));
			this.btnAddAllDoors.UseVisualStyleBackColor = true;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.toolTip1.SetToolTip(this.btnOption, componentResourceManager.GetString("btnOption.ToolTip"));
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.toolTip1.SetToolTip(this.btnEdit, componentResourceManager.GetString("btnEdit.ToolTip"));
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.dgvGroups, "dgvGroups");
			this.dgvGroups.AllowUserToAddRows = false;
			this.dgvGroups.AllowUserToDeleteRows = false;
			this.dgvGroups.AllowUserToOrderColumns = true;
			this.dgvGroups.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvGroups.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvGroups.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_GroupID, this.GroupName, this.f_active, this.MoreCards, this.f_SoundFile });
			this.dgvGroups.ContextMenuStrip = this.contextMenuStrip1;
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvGroups.DefaultCellStyle = dataGridViewCellStyle8;
			this.dgvGroups.EnableHeadersVisualStyles = false;
			this.dgvGroups.MultiSelect = false;
			this.dgvGroups.Name = "dgvGroups";
			this.dgvGroups.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroups.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dgvGroups.RowTemplate.Height = 23;
			this.dgvGroups.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvGroups, componentResourceManager.GetString("dgvGroups.ToolTip"));
			this.dgvGroups.DoubleClick += new global::System.EventHandler(this.dgvGroups_DoubleClick);
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.GroupName, "GroupName");
			this.GroupName.Name = "GroupName";
			this.GroupName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_active, "f_active");
			this.f_active.Name = "f_active";
			this.f_active.ReadOnly = true;
			componentResourceManager.ApplyResources(this.MoreCards, "MoreCards");
			this.MoreCards.Name = "MoreCards";
			this.MoreCards.ReadOnly = true;
			this.f_SoundFile.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_SoundFile, "f_SoundFile");
			this.f_SoundFile.Name = "f_SoundFile";
			this.f_SoundFile.ReadOnly = true;
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnOption);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.dgvGroups);
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.label4);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCheckAccessConfigure";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmCheckAccessConfigure_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmCheckAccessSetup_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCheckAccessConfigure_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			this.contextMenuStrip2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroups).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003180 RID: 12672
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003181 RID: 12673
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04003182 RID: 12674
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x04003183 RID: 12675
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003184 RID: 12676
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x04003185 RID: 12677
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x04003186 RID: 12678
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04003187 RID: 12679
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003188 RID: 12680
		private global::System.Windows.Forms.Button btnOption;

		// Token: 0x04003189 RID: 12681
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x0400318A RID: 12682
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x0400318B RID: 12683
		private global::System.Windows.Forms.ToolStripMenuItem clearAllCardsConfigureToolStripMenuItem;

		// Token: 0x0400318C RID: 12684
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x0400318D RID: 12685
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x0400318E RID: 12686
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip2;

		// Token: 0x0400318F RID: 12687
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04003190 RID: 12688
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04003191 RID: 12689
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04003192 RID: 12690
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04003193 RID: 12691
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04003194 RID: 12692
		private global::System.Windows.Forms.DataGridView dgvGroups;

		// Token: 0x04003195 RID: 12693
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x04003196 RID: 12694
		private global::System.Windows.Forms.ToolStripMenuItem displayConfigureToolStripMenuItem;

		// Token: 0x04003197 RID: 12695
		private global::System.Windows.Forms.ToolStripMenuItem doorsMoreCardsToolStripMenuItem;

		// Token: 0x04003198 RID: 12696
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_active;

		// Token: 0x04003199 RID: 12697
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x0400319A RID: 12698
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x0400319B RID: 12699
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x0400319C RID: 12700
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SoundFile;

		// Token: 0x0400319D RID: 12701
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x0400319E RID: 12702
		private global::System.Windows.Forms.ToolStripMenuItem fiveCardsToolStripMenuItem;

		// Token: 0x0400319F RID: 12703
		private global::System.Windows.Forms.ToolStripMenuItem fourCardsToolStripMenuItem;

		// Token: 0x040031A0 RID: 12704
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040031A1 RID: 12705
		private global::System.Windows.Forms.DataGridViewTextBoxColumn GroupName;

		// Token: 0x040031A2 RID: 12706
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040031A3 RID: 12707
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040031A4 RID: 12708
		private global::System.Windows.Forms.Label label25;

		// Token: 0x040031A5 RID: 12709
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040031A6 RID: 12710
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MoreCards;

		// Token: 0x040031A7 RID: 12711
		private global::System.Windows.Forms.ToolStripComboBox MoreCardsInsidetoolStripComboBox2;

		// Token: 0x040031A8 RID: 12712
		private global::System.Windows.Forms.ToolStripMenuItem moreCardsInsideToolStripMenuItem;

		// Token: 0x040031A9 RID: 12713
		private global::System.Windows.Forms.ToolStripMenuItem oneCardToolStripMenuItem;

		// Token: 0x040031AA RID: 12714
		private global::System.Windows.Forms.ToolStripMenuItem outsideSwipingToolStripMenuItem;

		// Token: 0x040031AB RID: 12715
		private global::System.Windows.Forms.ToolStripMenuItem sixCardsToolStripMenuItem;

		// Token: 0x040031AC RID: 12716
		private global::System.Windows.Forms.ToolStripMenuItem threeCardToolStripMenuItem;

		// Token: 0x040031AD RID: 12717
		private global::System.Windows.Forms.ToolStripComboBox toolStripComboBox1;

		// Token: 0x040031AE RID: 12718
		private global::System.Windows.Forms.ToolStripComboBox toolStripComboBox2;

		// Token: 0x040031AF RID: 12719
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x040031B0 RID: 12720
		private global::System.Windows.Forms.ToolStripMenuItem twoCardToolStripMenuItem;

		// Token: 0x040031B1 RID: 12721
		private global::System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
	}
}
