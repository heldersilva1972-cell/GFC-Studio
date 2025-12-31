namespace WG3000_COMM.Basic
{
	// Token: 0x02000025 RID: 37
	public partial class dfrmPrivilege : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0004EC6A File Offset: 0x0004DC6A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmWait1 != null)
			{
				this.dfrmWait1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0004ECA0 File Offset: 0x0004DCA0
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmPrivilege));
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
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnAddPass = new global::System.Windows.Forms.Button();
			this.btnDeletePass = new global::System.Windows.Forms.Button();
			this.btnDeletePassAndUpload = new global::System.Windows.Forms.Button();
			this.btnAddPassAndUpload = new global::System.Windows.Forms.Button();
			this.btnFind = new global::System.Windows.Forms.Button();
			this.mnuDoorTypeManage = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.manageCommonDoorControlGroup = new global::System.Windows.Forms.ToolStripMenuItem();
			this.mnuDoorTypeSelect = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.label5 = new global::System.Windows.Forms.Label();
			this.lblOver1000 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.btnConsoleUpload = new global::System.Windows.Forms.Button();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.mnuDoorTypeManage.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.btnAddPass, "btnAddPass");
			this.btnAddPass.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddPass.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddPass.ForeColor = global::System.Drawing.Color.White;
			this.btnAddPass.Image = global::WG3000_COMM.Properties.Resources.Rec1Pass;
			this.btnAddPass.Name = "btnAddPass";
			this.toolTip1.SetToolTip(this.btnAddPass, componentResourceManager.GetString("btnAddPass.ToolTip"));
			this.btnAddPass.UseVisualStyleBackColor = false;
			this.btnAddPass.Click += new global::System.EventHandler(this.btnAddPass_Click);
			componentResourceManager.ApplyResources(this.btnDeletePass, "btnDeletePass");
			this.btnDeletePass.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeletePass.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeletePass.ForeColor = global::System.Drawing.Color.White;
			this.btnDeletePass.Image = global::WG3000_COMM.Properties.Resources.Rec2NoPass;
			this.btnDeletePass.Name = "btnDeletePass";
			this.toolTip1.SetToolTip(this.btnDeletePass, componentResourceManager.GetString("btnDeletePass.ToolTip"));
			this.btnDeletePass.UseVisualStyleBackColor = false;
			this.btnDeletePass.Click += new global::System.EventHandler(this.btnDeletePass_Click);
			componentResourceManager.ApplyResources(this.btnDeletePassAndUpload, "btnDeletePassAndUpload");
			this.btnDeletePassAndUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeletePassAndUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeletePassAndUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnDeletePassAndUpload.Image = global::WG3000_COMM.Properties.Resources.wg16UploadNoPass;
			this.btnDeletePassAndUpload.Name = "btnDeletePassAndUpload";
			this.toolTip1.SetToolTip(this.btnDeletePassAndUpload, componentResourceManager.GetString("btnDeletePassAndUpload.ToolTip"));
			this.btnDeletePassAndUpload.UseVisualStyleBackColor = false;
			this.btnDeletePassAndUpload.Click += new global::System.EventHandler(this.btnDeletePassAndUpload_Click);
			componentResourceManager.ApplyResources(this.btnAddPassAndUpload, "btnAddPassAndUpload");
			this.btnAddPassAndUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddPassAndUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddPassAndUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnAddPassAndUpload.Image = global::WG3000_COMM.Properties.Resources.wg16UploadPass;
			this.btnAddPassAndUpload.Name = "btnAddPassAndUpload";
			this.toolTip1.SetToolTip(this.btnAddPassAndUpload, componentResourceManager.GetString("btnAddPassAndUpload.ToolTip"));
			this.btnAddPassAndUpload.UseVisualStyleBackColor = false;
			this.btnAddPassAndUpload.Click += new global::System.EventHandler(this.btnAddPassAndUpload_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.toolTip1.SetToolTip(this.btnFind, componentResourceManager.GetString("btnFind.ToolTip"));
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.mnuDoorTypeManage, "mnuDoorTypeManage");
			this.mnuDoorTypeManage.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.manageCommonDoorControlGroup });
			this.mnuDoorTypeManage.Name = "mnuDoorTypeManage";
			this.toolTip1.SetToolTip(this.mnuDoorTypeManage, componentResourceManager.GetString("mnuDoorTypeManage.ToolTip"));
			componentResourceManager.ApplyResources(this.manageCommonDoorControlGroup, "manageCommonDoorControlGroup");
			this.manageCommonDoorControlGroup.Name = "manageCommonDoorControlGroup";
			this.manageCommonDoorControlGroup.Click += new global::System.EventHandler(this.manageCommonDoorControlGroup_Click);
			componentResourceManager.ApplyResources(this.mnuDoorTypeSelect, "mnuDoorTypeSelect");
			this.mnuDoorTypeSelect.Name = "mnuDoorTypeSelect";
			this.toolTip1.SetToolTip(this.mnuDoorTypeSelect, componentResourceManager.GetString("mnuDoorTypeSelect.ToolTip"));
			this.mnuDoorTypeSelect.ItemClicked += new global::System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuDoorTypeSelect_ItemClicked);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.Yellow;
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.lblOver1000, "lblOver1000");
			this.lblOver1000.ForeColor = global::System.Drawing.Color.White;
			this.lblOver1000.Name = "lblOver1000";
			this.toolTip1.SetToolTip(this.lblOver1000, componentResourceManager.GetString("lblOver1000.ToolTip"));
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.cbof_ZoneID);
			this.groupBox2.Controls.Add(this.label25);
			this.groupBox2.Controls.Add(this.btnConsoleUpload);
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
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.toolTip1.SetToolTip(this.cbof_ZoneID, componentResourceManager.GetString("cbof_ZoneID.ToolTip"));
			this.cbof_ZoneID.DropDown += new global::System.EventHandler(this.cbof_ZoneID_DropDown);
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			this.cbof_ZoneID.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			this.toolTip1.SetToolTip(this.label25, componentResourceManager.GetString("label25.ToolTip"));
			componentResourceManager.ApplyResources(this.btnConsoleUpload, "btnConsoleUpload");
			this.btnConsoleUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnConsoleUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnConsoleUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnConsoleUpload.Image = global::WG3000_COMM.Properties.Resources.pConsole_Upload;
			this.btnConsoleUpload.Name = "btnConsoleUpload";
			this.toolTip1.SetToolTip(this.btnConsoleUpload, componentResourceManager.GetString("btnConsoleUpload.ToolTip"));
			this.btnConsoleUpload.UseVisualStyleBackColor = false;
			this.btnConsoleUpload.Click += new global::System.EventHandler(this.btnConsoleUpload_Click);
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
			this.dgvSelectedDoors.ContextMenuStrip = this.mnuDoorTypeManage;
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
			this.dgvSelectedDoors.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
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
			this.dgvDoors.ContextMenuStrip = this.mnuDoorTypeSelect;
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
			this.dgvDoors.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			this.dgvDoors.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvDoors_KeyDown);
			this.dgvDoors.MouseClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseClick);
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
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.toolTip1.SetToolTip(this.progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.cbof_ControlSegID);
			this.groupBox1.Controls.Add(this.cbof_GroupID);
			this.groupBox1.Controls.Add(this.lblWait);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.dgvSelectedUsers);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.Controls.Add(this.btnDelAllUsers);
			this.groupBox1.Controls.Add(this.btnDelOneUser);
			this.groupBox1.Controls.Add(this.btnAddOneUser);
			this.groupBox1.Controls.Add(this.btnAddAllUsers);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
			this.cbof_ControlSegID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegID.FormattingEnabled = true;
			this.cbof_ControlSegID.Name = "cbof_ControlSegID";
			this.toolTip1.SetToolTip(this.cbof_ControlSegID, componentResourceManager.GetString("cbof_ControlSegID.ToolTip"));
			this.cbof_ControlSegID.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			this.cbof_GroupID.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.ForeColor = global::System.Drawing.Color.White;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
			this.dgvSelectedUsers.AllowUserToAddRows = false;
			this.dgvSelectedUsers.AllowUserToDeleteRows = false;
			this.dgvSelectedUsers.AllowUserToOrderColumns = true;
			this.dgvSelectedUsers.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.f_SelectedGroup, this.dataGridViewCheckBoxColumn1 });
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle8;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			this.dgvSelectedUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle10;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle11.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle11.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle11.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle11.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.UserID, this.UserName, this.CardNO, this.f_GroupID, this.f_SelectedUsers });
			dataGridViewCellStyle12.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle12.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle12.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle12.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle12;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			dataGridViewCellStyle13.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle13.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle13.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle13.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle13.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle13.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle13.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.Enter += new global::System.EventHandler(this.cbof_GroupID_Enter);
			this.dgvUsers.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvUsers_KeyDown);
			this.dgvUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle14.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle14;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.UserName, "UserName");
			this.UserName.Name = "UserName";
			this.UserName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
			this.btnDelAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllUsers.Name = "btnDelAllUsers";
			this.toolTip1.SetToolTip(this.btnDelAllUsers, componentResourceManager.GetString("btnDelAllUsers.ToolTip"));
			this.btnDelAllUsers.UseVisualStyleBackColor = true;
			this.btnDelAllUsers.Click += new global::System.EventHandler(this.btnDelAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
			this.btnDelOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneUser.Name = "btnDelOneUser";
			this.toolTip1.SetToolTip(this.btnDelOneUser, componentResourceManager.GetString("btnDelOneUser.ToolTip"));
			this.btnDelOneUser.UseVisualStyleBackColor = true;
			this.btnDelOneUser.Click += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
			this.btnAddOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser.Name = "btnAddOneUser";
			this.toolTip1.SetToolTip(this.btnAddOneUser, componentResourceManager.GetString("btnAddOneUser.ToolTip"));
			this.btnAddOneUser.UseVisualStyleBackColor = true;
			this.btnAddOneUser.Click += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
			this.btnAddAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllUsers.Name = "btnAddAllUsers";
			this.toolTip1.SetToolTip(this.btnAddAllUsers, componentResourceManager.GetString("btnAddAllUsers.ToolTip"));
			this.btnAddAllUsers.UseVisualStyleBackColor = true;
			this.btnAddAllUsers.Click += new global::System.EventHandler(this.btnAddAllUsers_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.label5);
			base.Controls.Add(this.lblOver1000);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnAddPass);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.btnDeletePass);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.btnDeletePassAndUpload);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnAddPassAndUpload);
			base.Controls.Add(this.btnFind);
			base.Name = "dfrmPrivilege";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilege_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPrivilege_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPrivilege_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPrivilege_KeyDown);
			this.mnuDoorTypeManage.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040004E3 RID: 1251
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x040004E8 RID: 1256
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040004E9 RID: 1257
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040004EA RID: 1258
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x040004EB RID: 1259
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x040004EC RID: 1260
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x040004ED RID: 1261
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x040004EE RID: 1262
		private global::System.Windows.Forms.Button btnAddPass;

		// Token: 0x040004EF RID: 1263
		private global::System.Windows.Forms.Button btnAddPassAndUpload;

		// Token: 0x040004F0 RID: 1264
		private global::System.Windows.Forms.Button btnConsoleUpload;

		// Token: 0x040004F1 RID: 1265
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x040004F2 RID: 1266
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x040004F3 RID: 1267
		private global::System.Windows.Forms.Button btnDeletePass;

		// Token: 0x040004F4 RID: 1268
		private global::System.Windows.Forms.Button btnDeletePassAndUpload;

		// Token: 0x040004F5 RID: 1269
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x040004F6 RID: 1270
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x040004F7 RID: 1271
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x040004F8 RID: 1272
		private global::System.Windows.Forms.Button btnFind;

		// Token: 0x040004F9 RID: 1273
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x040004FA RID: 1274
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x040004FB RID: 1275
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x040004FC RID: 1276
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x040004FD RID: 1277
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x040004FE RID: 1278
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x040004FF RID: 1279
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04000500 RID: 1280
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04000501 RID: 1281
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04000502 RID: 1282
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04000503 RID: 1283
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04000504 RID: 1284
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04000505 RID: 1285
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04000506 RID: 1286
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04000507 RID: 1287
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04000508 RID: 1288
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04000509 RID: 1289
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x0400050A RID: 1290
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x0400050B RID: 1291
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x0400050C RID: 1292
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x0400050D RID: 1293
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x0400050E RID: 1294
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x0400050F RID: 1295
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x04000510 RID: 1296
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x04000511 RID: 1297
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x04000512 RID: 1298
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000513 RID: 1299
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000514 RID: 1300
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000515 RID: 1301
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04000516 RID: 1302
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000517 RID: 1303
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000518 RID: 1304
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000519 RID: 1305
		private global::System.Windows.Forms.Label lblOver1000;

		// Token: 0x0400051A RID: 1306
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x0400051B RID: 1307
		private global::System.Windows.Forms.ToolStripMenuItem manageCommonDoorControlGroup;

		// Token: 0x0400051C RID: 1308
		private global::System.Windows.Forms.ContextMenuStrip mnuDoorTypeManage;

		// Token: 0x0400051D RID: 1309
		private global::System.Windows.Forms.ContextMenuStrip mnuDoorTypeSelect;

		// Token: 0x0400051E RID: 1310
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400051F RID: 1311
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000520 RID: 1312
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04000521 RID: 1313
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x04000522 RID: 1314
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserName;
	}
}
