namespace WG3000_COMM.Basic
{
	// Token: 0x02000056 RID: 86
	public partial class frmWatchingLCD : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x000B4BAA File Offset: 0x000B3BAA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000B4BCC File Offset: 0x000B3BCC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmWatchingLCD));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.otherToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enlargeInfoDisplayToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ReduceInfoDisplaytoolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enlargeFontToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ReduceFontToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveDisplayStyleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.optionToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.rGOValidTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.defaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.onlyDisplayDepartmentToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.departmentOrderToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.departmentOrderEditToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.optionForUser20191008ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.flowLayoutPanel1 = new global::System.Windows.Forms.FlowLayoutPanel();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox5 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox5 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox1 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox2 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox2 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox3 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox3 = new global::System.Windows.Forms.RichTextBox();
			this.richTextBox6 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.dgvRunInfo = new global::System.Windows.Forms.DataGridView();
			this.Column1GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Info = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1Out = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1Swipe = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pictureBox4 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox4 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox6 = new global::System.Windows.Forms.PictureBox();
			this.lblCompanyName = new global::System.Windows.Forms.Label();
			this.lblZone = new global::System.Windows.Forms.Label();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.flowLayoutPanel2 = new global::System.Windows.Forms.FlowLayoutPanel();
			this.lblTime = new global::System.Windows.Forms.Label();
			this.lblAllTotal = new global::System.Windows.Forms.Label();
			this.lblTempCardTotal = new global::System.Windows.Forms.Label();
			this.flowLayoutPanel3 = new global::System.Windows.Forms.FlowLayoutPanel();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.panel4 = new global::System.Windows.Forms.Panel();
			this.splitter3 = new global::System.Windows.Forms.Splitter();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.splitter2 = new global::System.Windows.Forms.Splitter();
			this.splitter1 = new global::System.Windows.Forms.Splitter();
			this.panel5 = new global::System.Windows.Forms.Panel();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
			this.groupBox3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
			this.groupBox6.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
			this.panel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.otherToolStripMenuItem, this.optionToolStripMenuItem, this.exitToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.otherToolStripMenuItem, "otherToolStripMenuItem");
			this.otherToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.enlargeInfoDisplayToolStripMenuItem, this.ReduceInfoDisplaytoolStripMenuItem, this.enlargeFontToolStripMenuItem, this.ReduceFontToolStripMenuItem, this.saveDisplayStyleToolStripMenuItem, this.restoreDefaultToolStripMenuItem });
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			this.otherToolStripMenuItem.Click += new global::System.EventHandler(this.otherToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enlargeInfoDisplayToolStripMenuItem, "enlargeInfoDisplayToolStripMenuItem");
			this.enlargeInfoDisplayToolStripMenuItem.Name = "enlargeInfoDisplayToolStripMenuItem";
			this.enlargeInfoDisplayToolStripMenuItem.Click += new global::System.EventHandler(this.enlargeInfoDisplayToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ReduceInfoDisplaytoolStripMenuItem, "ReduceInfoDisplaytoolStripMenuItem");
			this.ReduceInfoDisplaytoolStripMenuItem.Name = "ReduceInfoDisplaytoolStripMenuItem";
			this.ReduceInfoDisplaytoolStripMenuItem.Click += new global::System.EventHandler(this.ReduceInfoDisplaytoolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enlargeFontToolStripMenuItem, "enlargeFontToolStripMenuItem");
			this.enlargeFontToolStripMenuItem.Name = "enlargeFontToolStripMenuItem";
			this.enlargeFontToolStripMenuItem.Click += new global::System.EventHandler(this.enlargeFontToolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.ReduceFontToolStripMenuItem, "ReduceFontToolStripMenuItem");
			this.ReduceFontToolStripMenuItem.Name = "ReduceFontToolStripMenuItem";
			this.ReduceFontToolStripMenuItem.Click += new global::System.EventHandler(this.ReduceFontToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.saveDisplayStyleToolStripMenuItem, "saveDisplayStyleToolStripMenuItem");
			this.saveDisplayStyleToolStripMenuItem.Name = "saveDisplayStyleToolStripMenuItem";
			this.saveDisplayStyleToolStripMenuItem.Click += new global::System.EventHandler(this.saveDisplayStyleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultToolStripMenuItem, "restoreDefaultToolStripMenuItem");
			this.restoreDefaultToolStripMenuItem.Name = "restoreDefaultToolStripMenuItem";
			this.restoreDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.optionToolStripMenuItem, "optionToolStripMenuItem");
			this.optionToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.rGOValidTimeToolStripMenuItem, this.defaultToolStripMenuItem, this.onlyDisplayDepartmentToolStripMenuItem, this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem, this.optionForUser20191008ToolStripMenuItem });
			this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
			componentResourceManager.ApplyResources(this.rGOValidTimeToolStripMenuItem, "rGOValidTimeToolStripMenuItem");
			this.rGOValidTimeToolStripMenuItem.Name = "rGOValidTimeToolStripMenuItem";
			this.rGOValidTimeToolStripMenuItem.Click += new global::System.EventHandler(this.rGOValidTimeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.defaultToolStripMenuItem, "defaultToolStripMenuItem");
			this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			this.defaultToolStripMenuItem.Click += new global::System.EventHandler(this.defaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.onlyDisplayDepartmentToolStripMenuItem, "onlyDisplayDepartmentToolStripMenuItem");
			this.onlyDisplayDepartmentToolStripMenuItem.Name = "onlyDisplayDepartmentToolStripMenuItem";
			this.onlyDisplayDepartmentToolStripMenuItem.Click += new global::System.EventHandler(this.onlyDisplayDepartmentToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem, "departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem");
			this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.departmentOrderToolStripMenuItem, this.departmentOrderEditToolStripMenuItem });
			this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem.Name = "departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem";
			componentResourceManager.ApplyResources(this.departmentOrderToolStripMenuItem, "departmentOrderToolStripMenuItem");
			this.departmentOrderToolStripMenuItem.Name = "departmentOrderToolStripMenuItem";
			this.departmentOrderToolStripMenuItem.TextChanged += new global::System.EventHandler(this.departmentOrderToolStripMenuItem_TextChanged);
			componentResourceManager.ApplyResources(this.departmentOrderEditToolStripMenuItem, "departmentOrderEditToolStripMenuItem");
			this.departmentOrderEditToolStripMenuItem.Name = "departmentOrderEditToolStripMenuItem";
			this.departmentOrderEditToolStripMenuItem.Click += new global::System.EventHandler(this.departmentOrderEditToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.optionForUser20191008ToolStripMenuItem, "optionForUser20191008ToolStripMenuItem");
			this.optionForUser20191008ToolStripMenuItem.Name = "optionForUser20191008ToolStripMenuItem";
			this.optionForUser20191008ToolStripMenuItem.Click += new global::System.EventHandler(this.optionForUser20191008ToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Click += new global::System.EventHandler(this.exitToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.groupBox5);
			this.flowLayoutPanel1.Controls.Add(this.groupBox1);
			this.flowLayoutPanel1.Controls.Add(this.groupBox2);
			this.flowLayoutPanel1.Controls.Add(this.groupBox3);
			this.flowLayoutPanel1.Controls.Add(this.richTextBox6);
			this.flowLayoutPanel1.Controls.Add(this.groupBox4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.pictureBox5);
			this.groupBox5.Controls.Add(this.richTextBox5);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBox5, "pictureBox5");
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.richTextBox5, "richTextBox5");
			this.richTextBox5.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox5.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox5.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox5.Name = "richTextBox5";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.richTextBox1);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox1.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox1.Name = "richTextBox1";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.pictureBox2);
			this.groupBox2.Controls.Add(this.richTextBox2);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBox2, "pictureBox2");
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.richTextBox2, "richTextBox2");
			this.richTextBox2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox2.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox2.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox2.Name = "richTextBox2";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.pictureBox3);
			this.groupBox3.Controls.Add(this.richTextBox3);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBox3, "pictureBox3");
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.richTextBox3, "richTextBox3");
			this.richTextBox3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox3.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox3.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox3.Name = "richTextBox3";
			componentResourceManager.ApplyResources(this.richTextBox6, "richTextBox6");
			this.richTextBox6.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox6.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox6.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox6.Name = "richTextBox6";
			this.richTextBox6.TextChanged += new global::System.EventHandler(this.richTextBox6_TextChanged);
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			this.groupBox4.Paint += new global::System.Windows.Forms.PaintEventHandler(this.groupBox4_Paint);
			componentResourceManager.ApplyResources(this.dgvRunInfo, "dgvRunInfo");
			this.dgvRunInfo.AllowUserToAddRows = false;
			this.dgvRunInfo.AllowUserToDeleteRows = false;
			this.dgvRunInfo.AutoSizeRowsMode = global::System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgvRunInfo.BackgroundColor = global::System.Drawing.Color.Blue;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 18f);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvRunInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvRunInfo.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvRunInfo.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.Column1GroupID, this.f_Desc, this.f_Info, this.Column1Out, this.Column1Swipe });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.Color.Blue;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvRunInfo.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvRunInfo.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgvRunInfo.EnableHeadersVisualStyles = false;
			this.dgvRunInfo.MultiSelect = false;
			this.dgvRunInfo.Name = "dgvRunInfo";
			this.dgvRunInfo.ReadOnly = true;
			this.dgvRunInfo.RowHeadersVisible = false;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 36f);
			this.dgvRunInfo.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvRunInfo.RowTemplate.DefaultCellStyle.BackColor = global::System.Drawing.Color.Blue;
			this.dgvRunInfo.RowTemplate.DefaultCellStyle.ForeColor = global::System.Drawing.Color.White;
			this.dgvRunInfo.RowTemplate.Height = 50;
			this.dgvRunInfo.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvRunInfo.ShowCellErrors = false;
			this.dgvRunInfo.ShowCellToolTips = false;
			this.dgvRunInfo.ShowEditingIcon = false;
			this.dgvRunInfo.ShowRowErrors = false;
			componentResourceManager.ApplyResources(this.Column1GroupID, "Column1GroupID");
			this.Column1GroupID.Name = "Column1GroupID";
			this.Column1GroupID.ReadOnly = true;
			this.f_Desc.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Desc, "f_Desc");
			this.f_Desc.Name = "f_Desc";
			this.f_Desc.ReadOnly = true;
			this.f_Info.FillWeight = 20f;
			componentResourceManager.ApplyResources(this.f_Info, "f_Info");
			this.f_Info.Name = "f_Info";
			this.f_Info.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1Out, "Column1Out");
			this.Column1Out.Name = "Column1Out";
			this.Column1Out.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1Swipe, "Column1Swipe");
			this.Column1Swipe.Name = "Column1Swipe";
			this.Column1Swipe.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoSizeRowsMode = global::System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridView1.BackgroundColor = global::System.Drawing.Color.Blue;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 18f);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.Column1, this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.Column2, this.Column3 });
			this.dataGridView1.EditMode = global::System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 36f);
			this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = global::System.Drawing.Color.Blue;
			this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = global::System.Drawing.Color.White;
			this.dataGridView1.RowTemplate.Height = 50;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.ShowCellErrors = false;
			this.dataGridView1.ShowCellToolTips = false;
			this.dataGridView1.ShowEditingIcon = false;
			this.dataGridView1.ShowRowErrors = false;
			this.dataGridView1.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.FillWeight = 20f;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.pictureBox4, "pictureBox4");
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.TabStop = false;
			this.pictureBox4.Click += new global::System.EventHandler(this.pictureBox4_Click);
			componentResourceManager.ApplyResources(this.richTextBox4, "richTextBox4");
			this.richTextBox4.BackColor = global::System.Drawing.Color.Blue;
			this.richTextBox4.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox4.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox4.Name = "richTextBox4";
			this.richTextBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Controls.Add(this.pictureBox6);
			this.groupBox6.ForeColor = global::System.Drawing.Color.White;
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			this.groupBox6.Paint += new global::System.Windows.Forms.PaintEventHandler(this.groupBox6_Paint);
			componentResourceManager.ApplyResources(this.pictureBox6, "pictureBox6");
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.TabStop = false;
			componentResourceManager.ApplyResources(this.lblCompanyName, "lblCompanyName");
			this.lblCompanyName.BackColor = global::System.Drawing.Color.Blue;
			this.lblCompanyName.FlatStyle = global::System.Windows.Forms.FlatStyle.Popup;
			this.lblCompanyName.ForeColor = global::System.Drawing.Color.Red;
			this.lblCompanyName.Name = "lblCompanyName";
			this.lblCompanyName.Click += new global::System.EventHandler(this.label1_Click);
			componentResourceManager.ApplyResources(this.lblZone, "lblZone");
			this.lblZone.BackColor = global::System.Drawing.Color.Blue;
			this.lblZone.FlatStyle = global::System.Windows.Forms.FlatStyle.Popup;
			this.lblZone.ForeColor = global::System.Drawing.Color.Yellow;
			this.lblZone.Name = "lblZone";
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = global::System.Drawing.Color.Blue;
			this.panel1.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.flowLayoutPanel1);
			this.panel1.Controls.Add(this.lblZone);
			this.panel1.Controls.Add(this.lblCompanyName);
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
			this.flowLayoutPanel2.BackColor = global::System.Drawing.Color.Blue;
			this.flowLayoutPanel2.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.flowLayoutPanel2.Controls.Add(this.lblTime);
			this.flowLayoutPanel2.Controls.Add(this.lblAllTotal);
			this.flowLayoutPanel2.Controls.Add(this.lblTempCardTotal);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Paint += new global::System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel2_Paint);
			componentResourceManager.ApplyResources(this.lblTime, "lblTime");
			this.lblTime.AutoEllipsis = true;
			this.lblTime.ForeColor = global::System.Drawing.Color.White;
			this.lblTime.Name = "lblTime";
			this.lblTime.Click += new global::System.EventHandler(this.label3_Click);
			componentResourceManager.ApplyResources(this.lblAllTotal, "lblAllTotal");
			this.lblAllTotal.AutoEllipsis = true;
			this.lblAllTotal.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAllTotal.ForeColor = global::System.Drawing.Color.Yellow;
			this.lblAllTotal.Name = "lblAllTotal";
			componentResourceManager.ApplyResources(this.lblTempCardTotal, "lblTempCardTotal");
			this.lblTempCardTotal.AutoEllipsis = true;
			this.lblTempCardTotal.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTempCardTotal.ForeColor = global::System.Drawing.Color.Yellow;
			this.lblTempCardTotal.Name = "lblTempCardTotal";
			componentResourceManager.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
			this.flowLayoutPanel3.BackColor = global::System.Drawing.Color.Blue;
			this.flowLayoutPanel3.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Controls.Add(this.panel4);
			this.panel2.Controls.Add(this.splitter3);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.splitter2);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.panel5);
			this.panel2.Controls.Add(this.flowLayoutPanel2);
			this.panel2.Name = "panel2";
			componentResourceManager.ApplyResources(this.panel4, "panel4");
			this.panel4.BackColor = global::System.Drawing.Color.Blue;
			this.panel4.Controls.Add(this.dataGridView1);
			this.panel4.Name = "panel4";
			componentResourceManager.ApplyResources(this.splitter3, "splitter3");
			this.splitter3.Name = "splitter3";
			this.splitter3.TabStop = false;
			componentResourceManager.ApplyResources(this.panel3, "panel3");
			this.panel3.Controls.Add(this.dgvRunInfo);
			this.panel3.Name = "panel3";
			componentResourceManager.ApplyResources(this.splitter2, "splitter2");
			this.splitter2.Name = "splitter2";
			this.splitter2.TabStop = false;
			componentResourceManager.ApplyResources(this.splitter1, "splitter1");
			this.splitter1.Name = "splitter1";
			this.splitter1.TabStop = false;
			componentResourceManager.ApplyResources(this.panel5, "panel5");
			this.panel5.BackColor = global::System.Drawing.Color.Blue;
			this.panel5.Controls.Add(this.pictureBox4);
			this.panel5.Controls.Add(this.groupBox6);
			this.panel5.Controls.Add(this.richTextBox4);
			this.panel5.Name = "panel5";
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.MinimizeBox = false;
			base.Name = "frmWatchingLCD";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmWatchingMoreRecords_FormClosing);
			base.Load += new global::System.EventHandler(this.frmWatchingMoreRecords_Load);
			base.SizeChanged += new global::System.EventHandler(this.frmWatchingMoreRecords_SizeChanged);
			this.contextMenuStrip1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
			this.groupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			this.groupBox2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
			this.groupBox3.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
			this.groupBox6.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
			this.panel1.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000BBE RID: 3006
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000BBF RID: 3007
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04000BC0 RID: 3008
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1GroupID;

		// Token: 0x04000BC1 RID: 3009
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1Out;

		// Token: 0x04000BC2 RID: 3010
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1Swipe;

		// Token: 0x04000BC3 RID: 3011
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04000BC4 RID: 3012
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04000BC5 RID: 3013
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000BC6 RID: 3014
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000BC7 RID: 3015
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04000BC8 RID: 3016
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04000BC9 RID: 3017
		private global::System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;

		// Token: 0x04000BCA RID: 3018
		private global::System.Windows.Forms.ToolStripMenuItem departmentOrderEditToolStripMenuItem;

		// Token: 0x04000BCB RID: 3019
		private global::System.Windows.Forms.ToolStripMenuItem departmentOrderToolStripMenuItem;

		// Token: 0x04000BCC RID: 3020
		private global::System.Windows.Forms.ToolStripMenuItem departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem;

		// Token: 0x04000BCD RID: 3021
		private global::System.Windows.Forms.DataGridView dgvRunInfo;

		// Token: 0x04000BCE RID: 3022
		private global::System.Windows.Forms.ToolStripMenuItem enlargeFontToolStripMenuItem;

		// Token: 0x04000BCF RID: 3023
		private global::System.Windows.Forms.ToolStripMenuItem enlargeInfoDisplayToolStripMenuItem;

		// Token: 0x04000BD0 RID: 3024
		private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

		// Token: 0x04000BD1 RID: 3025
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x04000BD2 RID: 3026
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Info;

		// Token: 0x04000BD3 RID: 3027
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

		// Token: 0x04000BD4 RID: 3028
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;

		// Token: 0x04000BD5 RID: 3029
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;

		// Token: 0x04000BD6 RID: 3030
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000BD7 RID: 3031
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000BD8 RID: 3032
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000BD9 RID: 3033
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000BDA RID: 3034
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04000BDB RID: 3035
		private global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x04000BDC RID: 3036
		private global::System.Windows.Forms.Label lblAllTotal;

		// Token: 0x04000BDD RID: 3037
		private global::System.Windows.Forms.Label lblCompanyName;

		// Token: 0x04000BDE RID: 3038
		private global::System.Windows.Forms.Label lblTempCardTotal;

		// Token: 0x04000BDF RID: 3039
		private global::System.Windows.Forms.Label lblTime;

		// Token: 0x04000BE0 RID: 3040
		private global::System.Windows.Forms.Label lblZone;

		// Token: 0x04000BE1 RID: 3041
		private global::System.Windows.Forms.ToolStripMenuItem onlyDisplayDepartmentToolStripMenuItem;

		// Token: 0x04000BE2 RID: 3042
		private global::System.Windows.Forms.ToolStripMenuItem optionForUser20191008ToolStripMenuItem;

		// Token: 0x04000BE3 RID: 3043
		private global::System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;

		// Token: 0x04000BE4 RID: 3044
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x04000BE5 RID: 3045
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000BE6 RID: 3046
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000BE7 RID: 3047
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000BE8 RID: 3048
		private global::System.Windows.Forms.Panel panel4;

		// Token: 0x04000BE9 RID: 3049
		private global::System.Windows.Forms.Panel panel5;

		// Token: 0x04000BEA RID: 3050
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000BEB RID: 3051
		private global::System.Windows.Forms.PictureBox pictureBox2;

		// Token: 0x04000BEC RID: 3052
		private global::System.Windows.Forms.PictureBox pictureBox3;

		// Token: 0x04000BED RID: 3053
		private global::System.Windows.Forms.PictureBox pictureBox4;

		// Token: 0x04000BEE RID: 3054
		private global::System.Windows.Forms.PictureBox pictureBox5;

		// Token: 0x04000BEF RID: 3055
		private global::System.Windows.Forms.PictureBox pictureBox6;

		// Token: 0x04000BF0 RID: 3056
		private global::System.Windows.Forms.ToolStripMenuItem ReduceFontToolStripMenuItem;

		// Token: 0x04000BF1 RID: 3057
		private global::System.Windows.Forms.ToolStripMenuItem ReduceInfoDisplaytoolStripMenuItem;

		// Token: 0x04000BF2 RID: 3058
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultToolStripMenuItem;

		// Token: 0x04000BF3 RID: 3059
		private global::System.Windows.Forms.ToolStripMenuItem rGOValidTimeToolStripMenuItem;

		// Token: 0x04000BF4 RID: 3060
		private global::System.Windows.Forms.RichTextBox richTextBox1;

		// Token: 0x04000BF5 RID: 3061
		private global::System.Windows.Forms.RichTextBox richTextBox2;

		// Token: 0x04000BF6 RID: 3062
		private global::System.Windows.Forms.RichTextBox richTextBox3;

		// Token: 0x04000BF7 RID: 3063
		private global::System.Windows.Forms.RichTextBox richTextBox4;

		// Token: 0x04000BF8 RID: 3064
		private global::System.Windows.Forms.RichTextBox richTextBox5;

		// Token: 0x04000BF9 RID: 3065
		private global::System.Windows.Forms.RichTextBox richTextBox6;

		// Token: 0x04000BFA RID: 3066
		private global::System.Windows.Forms.ToolStripMenuItem saveDisplayStyleToolStripMenuItem;

		// Token: 0x04000BFB RID: 3067
		private global::System.Windows.Forms.Splitter splitter1;

		// Token: 0x04000BFC RID: 3068
		private global::System.Windows.Forms.Splitter splitter2;

		// Token: 0x04000BFD RID: 3069
		private global::System.Windows.Forms.Splitter splitter3;

		// Token: 0x04000BFE RID: 3070
		private global::System.Windows.Forms.Timer timer1;
	}
}
