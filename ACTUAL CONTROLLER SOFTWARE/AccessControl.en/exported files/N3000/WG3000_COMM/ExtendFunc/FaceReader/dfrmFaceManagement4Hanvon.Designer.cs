namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D2 RID: 722
	public partial class dfrmFaceManagement4Hanvon : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600139F RID: 5023 RVA: 0x00175F54 File Offset: 0x00174F54
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00175F74 File Offset: 0x00174F74
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.FaceReader.dfrmFaceManagement4Hanvon));
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
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.label1 = new global::System.Windows.Forms.Label();
			this.dgvRunInfo = new global::System.Windows.Forms.DataGridView();
			this.f_Category = new global::System.Windows.Forms.DataGridViewImageColumn();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Time = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Info = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Detail = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MjRecStr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.btnNoFace = new global::System.Windows.Forms.Button();
			this.btnHaveFace = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column10 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column7 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.label4 = new global::System.Windows.Forms.Label();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.btnCheck = new global::System.Windows.Forms.Button();
			this.chkDeleteUsersNotInDB = new global::System.Windows.Forms.CheckBox();
			this.btnDeviceManage = new global::System.Windows.Forms.Button();
			this.btnDownloadAllUsers = new global::System.Windows.Forms.Button();
			this.btnUploadAllUsers = new global::System.Windows.Forms.Button();
			this.btnAdjustTime = new global::System.Windows.Forms.Button();
			this.btnDeleteAll = new global::System.Windows.Forms.Button();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnDownloadSelectedUsers = new global::System.Windows.Forms.Button();
			this.btnUploadSelectedUsers = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.FromArgb(64, 64, 64);
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvRunInfo, "dgvRunInfo");
			this.dgvRunInfo.AllowUserToAddRows = false;
			this.dgvRunInfo.AllowUserToDeleteRows = false;
			this.dgvRunInfo.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvRunInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvRunInfo.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvRunInfo.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_Category, this.f_RecID, this.f_Time, this.f_Desc, this.f_Info, this.f_Detail, this.f_MjRecStr });
			this.dgvRunInfo.EnableHeadersVisualStyles = false;
			this.dgvRunInfo.MultiSelect = false;
			this.dgvRunInfo.Name = "dgvRunInfo";
			this.dgvRunInfo.ReadOnly = true;
			this.dgvRunInfo.RowHeadersVisible = false;
			this.dgvRunInfo.RowTemplate.Height = 23;
			this.dgvRunInfo.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvRunInfo, componentResourceManager.GetString("dgvRunInfo.ToolTip"));
			this.dgvRunInfo.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRunInfo_CellFormatting);
			this.dgvRunInfo.DataError += new global::System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRunInfo_DataError);
			componentResourceManager.ApplyResources(this.f_Category, "f_Category");
			this.f_Category.Name = "f_Category";
			this.f_Category.ReadOnly = true;
			this.f_Category.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Category.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Time, "f_Time");
			this.f_Time.Name = "f_Time";
			this.f_Time.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc, "f_Desc");
			this.f_Desc.Name = "f_Desc";
			this.f_Desc.ReadOnly = true;
			this.f_Info.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Info, "f_Info");
			this.f_Info.Name = "f_Info";
			this.f_Info.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Detail, "f_Detail");
			this.f_Detail.Name = "f_Detail";
			this.f_Detail.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MjRecStr, "f_MjRecStr");
			this.f_MjRecStr.Name = "f_MjRecStr";
			this.f_MjRecStr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.toolTip1.SetToolTip(this.progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.btnNoFace);
			this.groupBox1.Controls.Add(this.btnHaveFace);
			this.groupBox1.Controls.Add(this.cbof_GroupID);
			this.groupBox1.Controls.Add(this.lblWait);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.dgvSelectedUsers);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.Controls.Add(this.btnDelAllUsers);
			this.groupBox1.Controls.Add(this.btnDelOneUser);
			this.groupBox1.Controls.Add(this.btnAddOneUser);
			this.groupBox1.Controls.Add(this.btnAddAllUsers);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnNoFace, "btnNoFace");
			this.btnNoFace.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNoFace.Name = "btnNoFace";
			this.toolTip1.SetToolTip(this.btnNoFace, componentResourceManager.GetString("btnNoFace.ToolTip"));
			this.btnNoFace.UseVisualStyleBackColor = true;
			this.btnNoFace.Click += new global::System.EventHandler(this.btnHaveFace_Click);
			componentResourceManager.ApplyResources(this.btnHaveFace, "btnHaveFace");
			this.btnHaveFace.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnHaveFace.Name = "btnHaveFace";
			this.toolTip1.SetToolTip(this.btnHaveFace, componentResourceManager.GetString("btnHaveFace.ToolTip"));
			this.btnHaveFace.UseVisualStyleBackColor = true;
			this.btnHaveFace.Click += new global::System.EventHandler(this.btnHaveFace_Click);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
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
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup, this.Column8, this.Column9, this.Column10 });
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.Enter += new global::System.EventHandler(this.dgvSelectedUsers_Enter);
			this.dgvSelectedUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column8, "Column8");
			this.Column8.Name = "Column8";
			this.Column8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column9, "Column9");
			this.Column9.Name = "Column9";
			this.Column9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column10, "Column10");
			this.Column10.Name = "Column10";
			this.Column10.ReadOnly = true;
			this.Column10.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.Column10.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.UserID, this.UserName, this.CardNO, this.f_SelectedUsers, this.f_GroupID, this.Column5, this.Column6, this.Column7 });
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle7;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.Enter += new global::System.EventHandler(this.dgvUsers_Enter);
			this.dgvUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle9;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.UserName, "UserName");
			this.UserName.Name = "UserName";
			this.UserName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column5, "Column5");
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column6, "Column6");
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column7, "Column7");
			this.Column7.Name = "Column7";
			this.Column7.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.btnCheck);
			this.groupBox2.Controls.Add(this.chkDeleteUsersNotInDB);
			this.groupBox2.Controls.Add(this.btnDeviceManage);
			this.groupBox2.Controls.Add(this.btnDownloadAllUsers);
			this.groupBox2.Controls.Add(this.btnUploadAllUsers);
			this.groupBox2.Controls.Add(this.btnAdjustTime);
			this.groupBox2.Controls.Add(this.btnDeleteAll);
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
			componentResourceManager.ApplyResources(this.btnCheck, "btnCheck");
			this.btnCheck.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCheck.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCheck.ForeColor = global::System.Drawing.Color.White;
			this.btnCheck.Name = "btnCheck";
			this.toolTip1.SetToolTip(this.btnCheck, componentResourceManager.GetString("btnCheck.ToolTip"));
			this.btnCheck.UseVisualStyleBackColor = false;
			this.btnCheck.Click += new global::System.EventHandler(this.btnCheck_Click);
			componentResourceManager.ApplyResources(this.chkDeleteUsersNotInDB, "chkDeleteUsersNotInDB");
			this.chkDeleteUsersNotInDB.Checked = true;
			this.chkDeleteUsersNotInDB.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDeleteUsersNotInDB.Name = "chkDeleteUsersNotInDB";
			this.toolTip1.SetToolTip(this.chkDeleteUsersNotInDB, componentResourceManager.GetString("chkDeleteUsersNotInDB.ToolTip"));
			this.chkDeleteUsersNotInDB.UseVisualStyleBackColor = true;
			this.chkDeleteUsersNotInDB.CheckedChanged += new global::System.EventHandler(this.chkDeleteUsersNotInDB_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnDeviceManage, "btnDeviceManage");
			this.btnDeviceManage.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeviceManage.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeviceManage.ForeColor = global::System.Drawing.Color.White;
			this.btnDeviceManage.Name = "btnDeviceManage";
			this.toolTip1.SetToolTip(this.btnDeviceManage, componentResourceManager.GetString("btnDeviceManage.ToolTip"));
			this.btnDeviceManage.UseVisualStyleBackColor = false;
			this.btnDeviceManage.Click += new global::System.EventHandler(this.btnDeviceManage_Click);
			componentResourceManager.ApplyResources(this.btnDownloadAllUsers, "btnDownloadAllUsers");
			this.btnDownloadAllUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDownloadAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDownloadAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnDownloadAllUsers.Name = "btnDownloadAllUsers";
			this.toolTip1.SetToolTip(this.btnDownloadAllUsers, componentResourceManager.GetString("btnDownloadAllUsers.ToolTip"));
			this.btnDownloadAllUsers.UseVisualStyleBackColor = false;
			this.btnDownloadAllUsers.Click += new global::System.EventHandler(this.btnDownloadAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnUploadAllUsers, "btnUploadAllUsers");
			this.btnUploadAllUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnUploadAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnUploadAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnUploadAllUsers.Name = "btnUploadAllUsers";
			this.toolTip1.SetToolTip(this.btnUploadAllUsers, componentResourceManager.GetString("btnUploadAllUsers.ToolTip"));
			this.btnUploadAllUsers.UseVisualStyleBackColor = false;
			this.btnUploadAllUsers.Click += new global::System.EventHandler(this.btnUploadAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnAdjustTime, "btnAdjustTime");
			this.btnAdjustTime.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdjustTime.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdjustTime.ForeColor = global::System.Drawing.Color.White;
			this.btnAdjustTime.Name = "btnAdjustTime";
			this.toolTip1.SetToolTip(this.btnAdjustTime, componentResourceManager.GetString("btnAdjustTime.ToolTip"));
			this.btnAdjustTime.UseVisualStyleBackColor = false;
			this.btnAdjustTime.Click += new global::System.EventHandler(this.btnAdjustTime_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
			this.btnDeleteAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAll.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAll.Name = "btnDeleteAll";
			this.toolTip1.SetToolTip(this.btnDeleteAll, componentResourceManager.GetString("btnDeleteAll.ToolTip"));
			this.btnDeleteAll.UseVisualStyleBackColor = false;
			this.btnDeleteAll.Click += new global::System.EventHandler(this.btnDeleteAll_Click);
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.AllowUserToOrderColumns = true;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle10.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle10.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.Column1, this.Column2, this.Column4 });
			dataGridViewCellStyle11.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle11.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle11.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle11.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle11;
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			dataGridViewCellStyle12.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle12.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle12.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle12.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.dgvSelectedDoors.RowTemplate.Height = 23;
			this.dgvSelectedDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedDoors, componentResourceManager.GetString("dgvSelectedDoors.ToolTip"));
			this.dgvSelectedDoors.Enter += new global::System.EventHandler(this.dgvSelectedDoors_Enter);
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
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.AllowUserToOrderColumns = true;
			this.dgvDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle13.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle13.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle13.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle13.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle13.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle13.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle13.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.IP, this.Port, this.Column3 });
			dataGridViewCellStyle14.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle14.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle14.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle14.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle14.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle14;
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			dataGridViewCellStyle15.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle15.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle15.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle15.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle15.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle15.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle15.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
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
			componentResourceManager.ApplyResources(this.IP, "IP");
			this.IP.Name = "IP";
			this.IP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Port, "Port");
			this.Port.Name = "Port";
			this.Port.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.btnDownloadSelectedUsers, "btnDownloadSelectedUsers");
			this.btnDownloadSelectedUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDownloadSelectedUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDownloadSelectedUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnDownloadSelectedUsers.Name = "btnDownloadSelectedUsers";
			this.toolTip1.SetToolTip(this.btnDownloadSelectedUsers, componentResourceManager.GetString("btnDownloadSelectedUsers.ToolTip"));
			this.btnDownloadSelectedUsers.UseVisualStyleBackColor = false;
			this.btnDownloadSelectedUsers.Click += new global::System.EventHandler(this.btnDownloadSelectedUsers_Click);
			componentResourceManager.ApplyResources(this.btnUploadSelectedUsers, "btnUploadSelectedUsers");
			this.btnUploadSelectedUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnUploadSelectedUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnUploadSelectedUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnUploadSelectedUsers.Name = "btnUploadSelectedUsers";
			this.toolTip1.SetToolTip(this.btnUploadSelectedUsers, componentResourceManager.GetString("btnUploadSelectedUsers.ToolTip"));
			this.btnUploadSelectedUsers.UseVisualStyleBackColor = false;
			this.btnUploadSelectedUsers.Click += new global::System.EventHandler(this.btnUploadSelectedUsers_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label1);
			base.Controls.Add(this.dgvRunInfo);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.btnDownloadSelectedUsers);
			base.Controls.Add(this.btnUploadSelectedUsers);
			base.Name = "dfrmFaceManagement4Hanvon";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmFaceManage_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmFaceManagement_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmFaceManage_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFaceManage_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040029A1 RID: 10657
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040029A2 RID: 10658
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040029A3 RID: 10659
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x040029A4 RID: 10660
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x040029A5 RID: 10661
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x040029A6 RID: 10662
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x040029A7 RID: 10663
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x040029A8 RID: 10664
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x040029A9 RID: 10665
		private global::System.Windows.Forms.Button btnDeleteAll;

		// Token: 0x040029AA RID: 10666
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x040029AB RID: 10667
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x040029AC RID: 10668
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x040029AD RID: 10669
		private global::System.Windows.Forms.Button btnHaveFace;

		// Token: 0x040029AE RID: 10670
		private global::System.Windows.Forms.Button btnNoFace;

		// Token: 0x040029AF RID: 10671
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x040029B0 RID: 10672
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x040029B1 RID: 10673
		private global::System.Windows.Forms.CheckBox chkDeleteUsersNotInDB;

		// Token: 0x040029B2 RID: 10674
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x040029B3 RID: 10675
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Column10;

		// Token: 0x040029B4 RID: 10676
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x040029B5 RID: 10677
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x040029B6 RID: 10678
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x040029B7 RID: 10679
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column5;

		// Token: 0x040029B8 RID: 10680
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column6;

		// Token: 0x040029B9 RID: 10681
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Column7;

		// Token: 0x040029BA RID: 10682
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column8;

		// Token: 0x040029BB RID: 10683
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column9;

		// Token: 0x040029BC RID: 10684
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x040029BD RID: 10685
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x040029BE RID: 10686
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x040029BF RID: 10687
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x040029C0 RID: 10688
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x040029C1 RID: 10689
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x040029C2 RID: 10690
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x040029C3 RID: 10691
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x040029C4 RID: 10692
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x040029C5 RID: 10693
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x040029C6 RID: 10694
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x040029C7 RID: 10695
		private global::System.Windows.Forms.DataGridView dgvRunInfo;

		// Token: 0x040029C8 RID: 10696
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x040029C9 RID: 10697
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x040029CA RID: 10698
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x040029CB RID: 10699
		private global::System.Windows.Forms.DataGridViewImageColumn f_Category;

		// Token: 0x040029CC RID: 10700
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x040029CD RID: 10701
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Detail;

		// Token: 0x040029CE RID: 10702
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x040029CF RID: 10703
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Info;

		// Token: 0x040029D0 RID: 10704
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MjRecStr;

		// Token: 0x040029D1 RID: 10705
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x040029D2 RID: 10706
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x040029D3 RID: 10707
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x040029D4 RID: 10708
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x040029D5 RID: 10709
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x040029D6 RID: 10710
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Time;

		// Token: 0x040029D7 RID: 10711
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040029D8 RID: 10712
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040029D9 RID: 10713
		private global::System.Windows.Forms.DataGridViewTextBoxColumn IP;

		// Token: 0x040029DA RID: 10714
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040029DB RID: 10715
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040029DC RID: 10716
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040029DD RID: 10717
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x040029DE RID: 10718
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Port;

		// Token: 0x040029DF RID: 10719
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x040029E0 RID: 10720
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040029E1 RID: 10721
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x040029E2 RID: 10722
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x040029E3 RID: 10723
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserName;

		// Token: 0x040029E4 RID: 10724
		internal global::System.Windows.Forms.Button btnAdjustTime;

		// Token: 0x040029E5 RID: 10725
		internal global::System.Windows.Forms.Button btnCheck;

		// Token: 0x040029E6 RID: 10726
		internal global::System.Windows.Forms.Button btnDeviceManage;

		// Token: 0x040029E7 RID: 10727
		internal global::System.Windows.Forms.Button btnDownloadAllUsers;

		// Token: 0x040029E8 RID: 10728
		internal global::System.Windows.Forms.Button btnDownloadSelectedUsers;

		// Token: 0x040029E9 RID: 10729
		internal global::System.Windows.Forms.Button btnUploadAllUsers;

		// Token: 0x040029EA RID: 10730
		internal global::System.Windows.Forms.Button btnUploadSelectedUsers;
	}
}
