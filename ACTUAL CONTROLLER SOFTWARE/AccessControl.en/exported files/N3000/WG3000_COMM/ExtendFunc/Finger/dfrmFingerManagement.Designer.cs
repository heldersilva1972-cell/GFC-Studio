namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E2 RID: 738
	public partial class dfrmFingerManagement : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001534 RID: 5428 RVA: 0x001A323C File Offset: 0x001A223C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x001A325C File Offset: 0x001A225C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmFingerManagement));
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
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.txtTotalFingerprint = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
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
			this.btnUploadOneuserFingerprints = new global::System.Windows.Forms.Button();
			this.btnAddCardNO = new global::System.Windows.Forms.Button();
			this.btnEditDescription = new global::System.Windows.Forms.Button();
			this.txtFingerDescription = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtf_CardNO = new global::System.Windows.Forms.MaskedTextBox();
			this.txtf_ConsumerName = new global::System.Windows.Forms.TextBox();
			this.btnFingerDel = new global::System.Windows.Forms.Button();
			this.btnFingerClear = new global::System.Windows.Forms.Button();
			this.btnFingerAdd = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.Column7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_RegisterTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
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
			this.Column11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column12 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column13 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDownloadSelectedUsers = new global::System.Windows.Forms.Button();
			this.btnUploadSelectedUsers = new global::System.Windows.Forms.Button();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.label4 = new global::System.Windows.Forms.Label();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.f_ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column10 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.btnCheck = new global::System.Windows.Forms.Button();
			this.btnConfigure = new global::System.Windows.Forms.Button();
			this.btnDeviceManage = new global::System.Windows.Forms.Button();
			this.btnDownloadAllUsers = new global::System.Windows.Forms.Button();
			this.btnUploadAllUsers = new global::System.Windows.Forms.Button();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.bkUploadAndGetRecords = new global::System.ComponentModel.BackgroundWorker();
			this.bkUploadConfigure = new global::System.ComponentModel.BackgroundWorker();
			this.bkCheck = new global::System.ComponentModel.BackgroundWorker();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
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
			componentResourceManager.ApplyResources(this.txtTotalFingerprint, "txtTotalFingerprint");
			this.txtTotalFingerprint.Name = "txtTotalFingerprint";
			this.txtTotalFingerprint.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtTotalFingerprint, componentResourceManager.GetString("txtTotalFingerprint.ToolTip"));
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
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
			this.groupBox1.Controls.Add(this.btnUploadOneuserFingerprints);
			this.groupBox1.Controls.Add(this.btnAddCardNO);
			this.groupBox1.Controls.Add(this.btnEditDescription);
			this.groupBox1.Controls.Add(this.txtFingerDescription);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtf_CardNO);
			this.groupBox1.Controls.Add(this.txtf_ConsumerName);
			this.groupBox1.Controls.Add(this.btnFingerDel);
			this.groupBox1.Controls.Add(this.btnFingerClear);
			this.groupBox1.Controls.Add(this.btnFingerAdd);
			this.groupBox1.Controls.Add(this.dataGridView1);
			this.groupBox1.Controls.Add(this.btnNoFace);
			this.groupBox1.Controls.Add(this.btnHaveFace);
			this.groupBox1.Controls.Add(this.cbof_GroupID);
			this.groupBox1.Controls.Add(this.lblWait);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.dgvSelectedUsers);
			this.groupBox1.Controls.Add(this.btnDownloadSelectedUsers);
			this.groupBox1.Controls.Add(this.btnUploadSelectedUsers);
			this.groupBox1.Controls.Add(this.btnDelAllUsers);
			this.groupBox1.Controls.Add(this.btnDelOneUser);
			this.groupBox1.Controls.Add(this.btnAddOneUser);
			this.groupBox1.Controls.Add(this.btnAddAllUsers);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			this.groupBox1.Enter += new global::System.EventHandler(this.groupBox1_Enter);
			componentResourceManager.ApplyResources(this.btnUploadOneuserFingerprints, "btnUploadOneuserFingerprints");
			this.btnUploadOneuserFingerprints.BackColor = global::System.Drawing.Color.Transparent;
			this.btnUploadOneuserFingerprints.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnUploadOneuserFingerprints.ForeColor = global::System.Drawing.Color.White;
			this.btnUploadOneuserFingerprints.Name = "btnUploadOneuserFingerprints";
			this.toolTip1.SetToolTip(this.btnUploadOneuserFingerprints, componentResourceManager.GetString("btnUploadOneuserFingerprints.ToolTip"));
			this.btnUploadOneuserFingerprints.UseVisualStyleBackColor = false;
			this.btnUploadOneuserFingerprints.Click += new global::System.EventHandler(this.btnUploadOneuserFingerprints_Click);
			componentResourceManager.ApplyResources(this.btnAddCardNO, "btnAddCardNO");
			this.btnAddCardNO.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddCardNO.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddCardNO.ForeColor = global::System.Drawing.Color.White;
			this.btnAddCardNO.Name = "btnAddCardNO";
			this.toolTip1.SetToolTip(this.btnAddCardNO, componentResourceManager.GetString("btnAddCardNO.ToolTip"));
			this.btnAddCardNO.UseVisualStyleBackColor = false;
			this.btnAddCardNO.Click += new global::System.EventHandler(this.btnAddCardNO_Click);
			componentResourceManager.ApplyResources(this.btnEditDescription, "btnEditDescription");
			this.btnEditDescription.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditDescription.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditDescription.ForeColor = global::System.Drawing.Color.White;
			this.btnEditDescription.Name = "btnEditDescription";
			this.toolTip1.SetToolTip(this.btnEditDescription, componentResourceManager.GetString("btnEditDescription.ToolTip"));
			this.btnEditDescription.UseVisualStyleBackColor = false;
			this.btnEditDescription.Click += new global::System.EventHandler(this.btnEditDescription_Click);
			componentResourceManager.ApplyResources(this.txtFingerDescription, "txtFingerDescription");
			this.txtFingerDescription.Name = "txtFingerDescription";
			this.toolTip1.SetToolTip(this.txtFingerDescription, componentResourceManager.GetString("txtFingerDescription.ToolTip"));
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_CardNO, "txtf_CardNO");
			this.txtf_CardNO.Name = "txtf_CardNO";
			this.txtf_CardNO.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtf_CardNO, componentResourceManager.GetString("txtf_CardNO.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
			this.txtf_ConsumerName.Name = "txtf_ConsumerName";
			this.txtf_ConsumerName.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtf_ConsumerName, componentResourceManager.GetString("txtf_ConsumerName.ToolTip"));
			componentResourceManager.ApplyResources(this.btnFingerDel, "btnFingerDel");
			this.btnFingerDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFingerDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFingerDel.ForeColor = global::System.Drawing.Color.White;
			this.btnFingerDel.Name = "btnFingerDel";
			this.toolTip1.SetToolTip(this.btnFingerDel, componentResourceManager.GetString("btnFingerDel.ToolTip"));
			this.btnFingerDel.UseVisualStyleBackColor = false;
			this.btnFingerDel.Click += new global::System.EventHandler(this.btnFingerDel_Click);
			componentResourceManager.ApplyResources(this.btnFingerClear, "btnFingerClear");
			this.btnFingerClear.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFingerClear.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFingerClear.ForeColor = global::System.Drawing.Color.White;
			this.btnFingerClear.Name = "btnFingerClear";
			this.toolTip1.SetToolTip(this.btnFingerClear, componentResourceManager.GetString("btnFingerClear.ToolTip"));
			this.btnFingerClear.UseVisualStyleBackColor = false;
			this.btnFingerClear.Click += new global::System.EventHandler(this.btnFingerClear_Click);
			componentResourceManager.ApplyResources(this.btnFingerAdd, "btnFingerAdd");
			this.btnFingerAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFingerAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFingerAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnFingerAdd.Name = "btnFingerAdd";
			this.toolTip1.SetToolTip(this.btnFingerAdd, componentResourceManager.GetString("btnFingerAdd.ToolTip"));
			this.btnFingerAdd.UseVisualStyleBackColor = false;
			this.btnFingerAdd.Click += new global::System.EventHandler(this.btnFingerAdd_Click);
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.Column7, this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11, this.f_RegisterTime });
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dataGridView1, componentResourceManager.GetString("dataGridView1.ToolTip"));
			this.dataGridView1.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			this.dataGridView1.DoubleClick += new global::System.EventHandler(this.dataGridView1_DoubleClick);
			componentResourceManager.ApplyResources(this.Column7, "Column7");
			this.Column7.Name = "Column7";
			this.Column7.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			this.f_RegisterTime.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.f_RegisterTime.DataPropertyName = "f_RegisterTime";
			componentResourceManager.ApplyResources(this.f_RegisterTime, "f_RegisterTime");
			this.f_RegisterTime.Name = "f_RegisterTime";
			this.f_RegisterTime.ReadOnly = true;
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
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup, this.Column11, this.Column12, this.Column13 });
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle7;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.Enter += new global::System.EventHandler(this.dgvSelectedUsers_Enter);
			this.dgvSelectedUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle9;
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
			componentResourceManager.ApplyResources(this.Column11, "Column11");
			this.Column11.Name = "Column11";
			this.Column11.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column12, "Column12");
			this.Column12.Name = "Column12";
			this.Column12.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column13, "Column13");
			this.Column13.Name = "Column13";
			this.Column13.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle10.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle10.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ConsumerID, this.UserID, this.UserName, this.CardNO, this.f_SelectedUsers, this.f_GroupID, this.Column8, this.Column9, this.Column10 });
			dataGridViewCellStyle11.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle11.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle11.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle11.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle11;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			dataGridViewCellStyle12.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle12.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle12.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle12.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
			this.dgvUsers.Enter += new global::System.EventHandler(this.dgvUsers_Enter);
			this.dgvUsers.MouseClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseClick);
			this.dgvUsers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvUsers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
			this.f_ConsumerID.Name = "f_ConsumerID";
			this.f_ConsumerID.ReadOnly = true;
			dataGridViewCellStyle13.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle13;
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
			componentResourceManager.ApplyResources(this.Column8, "Column8");
			this.Column8.Name = "Column8";
			this.Column8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column9, "Column9");
			this.Column9.Name = "Column9";
			this.Column9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column10, "Column10");
			this.Column10.Name = "Column10";
			this.Column10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.btnCheck);
			this.groupBox2.Controls.Add(this.btnConfigure);
			this.groupBox2.Controls.Add(this.btnDeviceManage);
			this.groupBox2.Controls.Add(this.btnDownloadAllUsers);
			this.groupBox2.Controls.Add(this.btnUploadAllUsers);
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
			componentResourceManager.ApplyResources(this.btnConfigure, "btnConfigure");
			this.btnConfigure.BackColor = global::System.Drawing.Color.Transparent;
			this.btnConfigure.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnConfigure.ForeColor = global::System.Drawing.Color.White;
			this.btnConfigure.Name = "btnConfigure";
			this.toolTip1.SetToolTip(this.btnConfigure, componentResourceManager.GetString("btnConfigure.ToolTip"));
			this.btnConfigure.UseVisualStyleBackColor = false;
			this.btnConfigure.Click += new global::System.EventHandler(this.btnConfigure_Click);
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
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.AllowUserToOrderColumns = true;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle14.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle14.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle14.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle14.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle14.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.Column6, this.Column1, this.Column2, this.Column4 });
			dataGridViewCellStyle15.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle15.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle15.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle15.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle15.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle15.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle15.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle15;
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			dataGridViewCellStyle16.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle16.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle16.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle16.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle16.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle16.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
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
			componentResourceManager.ApplyResources(this.Column6, "Column6");
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column4.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.AllowUserToOrderColumns = true;
			this.dgvDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle17.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle17.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle17.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle17.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle17.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle17.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle17.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.Column5, this.IP, this.Port, this.Column3 });
			dataGridViewCellStyle18.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle18.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle18.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle18.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle18.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle18.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle18.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle18;
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			dataGridViewCellStyle19.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle19.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle19.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle19.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle19.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle19.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle19.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
			this.dgvDoors.RowTemplate.Height = 23;
			this.dgvDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvDoors, componentResourceManager.GetString("dgvDoors.ToolTip"));
			this.dgvDoors.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDoors_CellContentClick);
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
			componentResourceManager.ApplyResources(this.Column5, "Column5");
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.IP, "IP");
			this.IP.Name = "IP";
			this.IP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Port, "Port");
			this.Port.Name = "Port";
			this.Port.ReadOnly = true;
			this.Column3.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
			this.bkUploadAndGetRecords.WorkerSupportsCancellation = true;
			this.bkUploadAndGetRecords.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkUploadAndGetRecords_DoWork);
			this.bkUploadConfigure.WorkerSupportsCancellation = true;
			this.bkUploadConfigure.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkUploadConfigure_DoWork);
			this.bkCheck.WorkerSupportsCancellation = true;
			this.bkCheck.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.bkCheck_DoWork);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtTotalFingerprint);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.dgvRunInfo);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox2);
			base.Name = "dfrmFingerManagement";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmFaceManage_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmFingerManagement_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmFaceManage_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFaceManage_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.groupBox2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002BBD RID: 11197
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002BBE RID: 11198
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002BBF RID: 11199
		private global::System.ComponentModel.BackgroundWorker bkCheck;

		// Token: 0x04002BC0 RID: 11200
		private global::System.ComponentModel.BackgroundWorker bkUploadAndGetRecords;

		// Token: 0x04002BC1 RID: 11201
		private global::System.ComponentModel.BackgroundWorker bkUploadConfigure;

		// Token: 0x04002BC2 RID: 11202
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04002BC3 RID: 11203
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x04002BC4 RID: 11204
		private global::System.Windows.Forms.Button btnAddCardNO;

		// Token: 0x04002BC5 RID: 11205
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x04002BC6 RID: 11206
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x04002BC7 RID: 11207
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x04002BC8 RID: 11208
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04002BC9 RID: 11209
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x04002BCA RID: 11210
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04002BCB RID: 11211
		private global::System.Windows.Forms.Button btnEditDescription;

		// Token: 0x04002BCC RID: 11212
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002BCD RID: 11213
		private global::System.Windows.Forms.Button btnFingerAdd;

		// Token: 0x04002BCE RID: 11214
		private global::System.Windows.Forms.Button btnFingerClear;

		// Token: 0x04002BCF RID: 11215
		private global::System.Windows.Forms.Button btnFingerDel;

		// Token: 0x04002BD0 RID: 11216
		private global::System.Windows.Forms.Button btnHaveFace;

		// Token: 0x04002BD1 RID: 11217
		private global::System.Windows.Forms.Button btnNoFace;

		// Token: 0x04002BD2 RID: 11218
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04002BD3 RID: 11219
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04002BD4 RID: 11220
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002BD5 RID: 11221
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Column10;

		// Token: 0x04002BD6 RID: 11222
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column11;

		// Token: 0x04002BD7 RID: 11223
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column12;

		// Token: 0x04002BD8 RID: 11224
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column13;

		// Token: 0x04002BD9 RID: 11225
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04002BDA RID: 11226
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04002BDB RID: 11227
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x04002BDC RID: 11228
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column5;

		// Token: 0x04002BDD RID: 11229
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column6;

		// Token: 0x04002BDE RID: 11230
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column7;

		// Token: 0x04002BDF RID: 11231
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column8;

		// Token: 0x04002BE0 RID: 11232
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column9;

		// Token: 0x04002BE1 RID: 11233
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04002BE2 RID: 11234
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002BE3 RID: 11235
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002BE4 RID: 11236
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		// Token: 0x04002BE5 RID: 11237
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		// Token: 0x04002BE6 RID: 11238
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002BE7 RID: 11239
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002BE8 RID: 11240
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002BE9 RID: 11241
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002BEA RID: 11242
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002BEB RID: 11243
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04002BEC RID: 11244
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04002BED RID: 11245
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04002BEE RID: 11246
		private global::System.Windows.Forms.DataGridView dgvRunInfo;

		// Token: 0x04002BEF RID: 11247
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x04002BF0 RID: 11248
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x04002BF1 RID: 11249
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04002BF2 RID: 11250
		private global::System.Windows.Forms.DataGridViewImageColumn f_Category;

		// Token: 0x04002BF3 RID: 11251
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerID;

		// Token: 0x04002BF4 RID: 11252
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x04002BF5 RID: 11253
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Detail;

		// Token: 0x04002BF6 RID: 11254
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04002BF7 RID: 11255
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Info;

		// Token: 0x04002BF8 RID: 11256
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MjRecStr;

		// Token: 0x04002BF9 RID: 11257
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04002BFA RID: 11258
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RegisterTime;

		// Token: 0x04002BFB RID: 11259
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002BFC RID: 11260
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x04002BFD RID: 11261
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x04002BFE RID: 11262
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x04002BFF RID: 11263
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Time;

		// Token: 0x04002C00 RID: 11264
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002C01 RID: 11265
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04002C02 RID: 11266
		private global::System.Windows.Forms.DataGridViewTextBoxColumn IP;

		// Token: 0x04002C03 RID: 11267
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002C04 RID: 11268
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002C05 RID: 11269
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002C06 RID: 11270
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002C07 RID: 11271
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04002C08 RID: 11272
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04002C09 RID: 11273
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x04002C0A RID: 11274
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Port;

		// Token: 0x04002C0B RID: 11275
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x04002C0C RID: 11276
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04002C0D RID: 11277
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04002C0E RID: 11278
		private global::System.Windows.Forms.MaskedTextBox txtf_CardNO;

		// Token: 0x04002C0F RID: 11279
		private global::System.Windows.Forms.TextBox txtf_ConsumerName;

		// Token: 0x04002C10 RID: 11280
		private global::System.Windows.Forms.TextBox txtFingerDescription;

		// Token: 0x04002C11 RID: 11281
		private global::System.Windows.Forms.TextBox txtTotalFingerprint;

		// Token: 0x04002C12 RID: 11282
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x04002C13 RID: 11283
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserName;

		// Token: 0x04002C14 RID: 11284
		internal global::System.Windows.Forms.Button btnCheck;

		// Token: 0x04002C15 RID: 11285
		internal global::System.Windows.Forms.Button btnConfigure;

		// Token: 0x04002C16 RID: 11286
		internal global::System.Windows.Forms.Button btnDeviceManage;

		// Token: 0x04002C17 RID: 11287
		internal global::System.Windows.Forms.Button btnDownloadAllUsers;

		// Token: 0x04002C18 RID: 11288
		internal global::System.Windows.Forms.Button btnDownloadSelectedUsers;

		// Token: 0x04002C19 RID: 11289
		internal global::System.Windows.Forms.Button btnUploadAllUsers;

		// Token: 0x04002C1A RID: 11290
		internal global::System.Windows.Forms.Button btnUploadOneuserFingerprints;

		// Token: 0x04002C1B RID: 11291
		internal global::System.Windows.Forms.Button btnUploadSelectedUsers;
	}
}
