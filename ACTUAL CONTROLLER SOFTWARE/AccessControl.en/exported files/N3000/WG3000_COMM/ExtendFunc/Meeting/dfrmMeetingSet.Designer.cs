namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FF RID: 767
	public partial class dfrmMeetingSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060016B3 RID: 5811 RVA: 0x001D208B File Offset: 0x001D108B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x001D20AC File Offset: 0x001D10AC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.dfrmMeetingSet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.txtf_Notes = new global::System.Windows.Forms.TextBox();
			this.lblNotes = new global::System.Windows.Forms.Label();
			this.txtf_Content = new global::System.Windows.Forms.TextBox();
			this.cbof_MeetingAdr = new global::System.Windows.Forms.ComboBox();
			this.lblContent = new global::System.Windows.Forms.Label();
			this.btnAddMeetingAdr = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtSeat = new global::System.Windows.Forms.TextBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.cboIdentity = new global::System.Windows.Forms.ComboBox();
			this.lblControlTimeSeg = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Identity = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IdentityStr2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MoreCards_GrpID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Identity1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IdentityStr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SeatNO1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.btnAddAll = new global::System.Windows.Forms.Button();
			this.Label9 = new global::System.Windows.Forms.Label();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.txtf_MeetingNo = new global::System.Windows.Forms.TextBox();
			this.lblMeetingName = new global::System.Windows.Forms.Label();
			this.txtf_MeetingName = new global::System.Windows.Forms.TextBox();
			this.lblMeetingAddr = new global::System.Windows.Forms.Label();
			this.lblMeetingDateTime = new global::System.Windows.Forms.Label();
			this.dtpMeetingDate = new global::System.Windows.Forms.DateTimePicker();
			this.dtpMeetingTime = new global::System.Windows.Forms.DateTimePicker();
			this.lblSignBegin = new global::System.Windows.Forms.Label();
			this.lblSignEnd = new global::System.Windows.Forms.Label();
			this.dtpStartTime = new global::System.Windows.Forms.DateTimePicker();
			this.dtpEndTime = new global::System.Windows.Forms.DateTimePicker();
			this.btnCreateInfo = new global::System.Windows.Forms.Button();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnFind = new global::System.Windows.Forms.Button();
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
			componentResourceManager.ApplyResources(this.txtf_Notes, "txtf_Notes");
			this.txtf_Notes.Name = "txtf_Notes";
			this.toolTip1.SetToolTip(this.txtf_Notes, componentResourceManager.GetString("txtf_Notes.ToolTip"));
			componentResourceManager.ApplyResources(this.lblNotes, "lblNotes");
			this.lblNotes.BackColor = global::System.Drawing.Color.Transparent;
			this.lblNotes.ForeColor = global::System.Drawing.Color.White;
			this.lblNotes.Name = "lblNotes";
			this.toolTip1.SetToolTip(this.lblNotes, componentResourceManager.GetString("lblNotes.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Content, "txtf_Content");
			this.txtf_Content.Name = "txtf_Content";
			this.toolTip1.SetToolTip(this.txtf_Content, componentResourceManager.GetString("txtf_Content.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_MeetingAdr, "cbof_MeetingAdr");
			this.cbof_MeetingAdr.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_MeetingAdr.Name = "cbof_MeetingAdr";
			this.toolTip1.SetToolTip(this.cbof_MeetingAdr, componentResourceManager.GetString("cbof_MeetingAdr.ToolTip"));
			this.cbof_MeetingAdr.DropDown += new global::System.EventHandler(this.cbof_MeetingAdr_DropDown);
			this.cbof_MeetingAdr.SelectedIndexChanged += new global::System.EventHandler(this.cbof_MeetingAdr_SelectedIndexChanged);
			this.cbof_MeetingAdr.Enter += new global::System.EventHandler(this.cbof_MeetingAdr_Enter);
			componentResourceManager.ApplyResources(this.lblContent, "lblContent");
			this.lblContent.BackColor = global::System.Drawing.Color.Transparent;
			this.lblContent.ForeColor = global::System.Drawing.Color.White;
			this.lblContent.Name = "lblContent";
			this.toolTip1.SetToolTip(this.lblContent, componentResourceManager.GetString("lblContent.ToolTip"));
			componentResourceManager.ApplyResources(this.btnAddMeetingAdr, "btnAddMeetingAdr");
			this.btnAddMeetingAdr.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddMeetingAdr.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddMeetingAdr.ForeColor = global::System.Drawing.Color.White;
			this.btnAddMeetingAdr.Name = "btnAddMeetingAdr";
			this.toolTip1.SetToolTip(this.btnAddMeetingAdr, componentResourceManager.GetString("btnAddMeetingAdr.ToolTip"));
			this.btnAddMeetingAdr.UseVisualStyleBackColor = false;
			this.btnAddMeetingAdr.Click += new global::System.EventHandler(this.btnAddMeetingAdr_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.txtSeat);
			this.groupBox1.Controls.Add(this.lblWait);
			this.groupBox1.Controls.Add(this.cboIdentity);
			this.groupBox1.Controls.Add(this.lblControlTimeSeg);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.dgvSelectedUsers);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.Controls.Add(this.btnDelAllUsers);
			this.groupBox1.Controls.Add(this.btnDelOneUser);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.btnAddAll);
			this.groupBox1.Controls.Add(this.Label9);
			this.groupBox1.Controls.Add(this.cbof_GroupID);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.txtSeat, "txtSeat");
			this.txtSeat.BackColor = global::System.Drawing.Color.White;
			this.txtSeat.Name = "txtSeat";
			this.toolTip1.SetToolTip(this.txtSeat, componentResourceManager.GetString("txtSeat.ToolTip"));
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.cboIdentity, "cboIdentity");
			this.cboIdentity.BackColor = global::System.Drawing.Color.White;
			this.cboIdentity.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIdentity.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboIdentity.Items"),
				componentResourceManager.GetString("cboIdentity.Items1"),
				componentResourceManager.GetString("cboIdentity.Items2"),
				componentResourceManager.GetString("cboIdentity.Items3"),
				componentResourceManager.GetString("cboIdentity.Items4"),
				componentResourceManager.GetString("cboIdentity.Items5")
			});
			this.cboIdentity.Name = "cboIdentity";
			this.toolTip1.SetToolTip(this.cboIdentity, componentResourceManager.GetString("cboIdentity.ToolTip"));
			componentResourceManager.ApplyResources(this.lblControlTimeSeg, "lblControlTimeSeg");
			this.lblControlTimeSeg.BackColor = global::System.Drawing.Color.Transparent;
			this.lblControlTimeSeg.ForeColor = global::System.Drawing.Color.White;
			this.lblControlTimeSeg.Name = "lblControlTimeSeg";
			this.toolTip1.SetToolTip(this.lblControlTimeSeg, componentResourceManager.GetString("lblControlTimeSeg.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
			this.dgvSelectedUsers.AllowUserToAddRows = false;
			this.dgvSelectedUsers.AllowUserToDeleteRows = false;
			this.dgvSelectedUsers.AllowUserToOrderColumns = true;
			this.dgvSelectedUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.Identity, this.IdentityStr2, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.f_MoreCards_GrpID, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.DoubleClick += new global::System.EventHandler(this.btnDelOneUser_Click);
			this.dgvSelectedUsers.Enter += new global::System.EventHandler(this.cbof_MeetingAdr_Enter);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Identity, "Identity");
			this.Identity.Name = "Identity";
			this.Identity.ReadOnly = true;
			componentResourceManager.ApplyResources(this.IdentityStr2, "IdentityStr2");
			this.IdentityStr2.Name = "IdentityStr2";
			this.IdentityStr2.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MoreCards_GrpID, "f_MoreCards_GrpID");
			this.f_MoreCards_GrpID.Name = "f_MoreCards_GrpID";
			this.f_MoreCards_GrpID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.Identity1, this.IdentityStr, this.UserID, this.ConsumerName, this.CardNO, this.SeatNO1, this.f_SelectedUsers, this.f_GroupID });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.btnAdd_Click);
			this.dgvUsers.Enter += new global::System.EventHandler(this.cbof_MeetingAdr_Enter);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Identity1, "Identity1");
			this.Identity1.Name = "Identity1";
			this.Identity1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.IdentityStr, "IdentityStr");
			this.IdentityStr.Name = "IdentityStr";
			this.IdentityStr.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.SeatNO1, "SeatNO1");
			this.SeatNO1.Name = "SeatNO1";
			this.SeatNO1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.Name = "btnAdd";
			this.toolTip1.SetToolTip(this.btnAdd, componentResourceManager.GetString("btnAdd.ToolTip"));
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnAddAll, "btnAddAll");
			this.btnAddAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAll.Name = "btnAddAll";
			this.toolTip1.SetToolTip(this.btnAddAll, componentResourceManager.GetString("btnAddAll.ToolTip"));
			this.btnAddAll.UseVisualStyleBackColor = true;
			this.btnAddAll.Click += new global::System.EventHandler(this.btnAddAll_Click);
			componentResourceManager.ApplyResources(this.Label9, "Label9");
			this.Label9.BackColor = global::System.Drawing.Color.Transparent;
			this.Label9.ForeColor = global::System.Drawing.Color.White;
			this.Label9.Name = "Label9";
			this.toolTip1.SetToolTip(this.Label9, componentResourceManager.GetString("Label9.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			this.cbof_GroupID.Enter += new global::System.EventHandler(this.cbof_MeetingAdr_Enter);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			this.toolTip1.SetToolTip(this.Label1, componentResourceManager.GetString("Label1.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_MeetingNo, "txtf_MeetingNo");
			this.txtf_MeetingNo.Name = "txtf_MeetingNo";
			this.txtf_MeetingNo.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtf_MeetingNo, componentResourceManager.GetString("txtf_MeetingNo.ToolTip"));
			componentResourceManager.ApplyResources(this.lblMeetingName, "lblMeetingName");
			this.lblMeetingName.BackColor = global::System.Drawing.Color.Transparent;
			this.lblMeetingName.ForeColor = global::System.Drawing.Color.White;
			this.lblMeetingName.Name = "lblMeetingName";
			this.toolTip1.SetToolTip(this.lblMeetingName, componentResourceManager.GetString("lblMeetingName.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_MeetingName, "txtf_MeetingName");
			this.txtf_MeetingName.Name = "txtf_MeetingName";
			this.toolTip1.SetToolTip(this.txtf_MeetingName, componentResourceManager.GetString("txtf_MeetingName.ToolTip"));
			componentResourceManager.ApplyResources(this.lblMeetingAddr, "lblMeetingAddr");
			this.lblMeetingAddr.BackColor = global::System.Drawing.Color.Transparent;
			this.lblMeetingAddr.ForeColor = global::System.Drawing.Color.White;
			this.lblMeetingAddr.Name = "lblMeetingAddr";
			this.toolTip1.SetToolTip(this.lblMeetingAddr, componentResourceManager.GetString("lblMeetingAddr.ToolTip"));
			componentResourceManager.ApplyResources(this.lblMeetingDateTime, "lblMeetingDateTime");
			this.lblMeetingDateTime.BackColor = global::System.Drawing.Color.Transparent;
			this.lblMeetingDateTime.ForeColor = global::System.Drawing.Color.White;
			this.lblMeetingDateTime.Name = "lblMeetingDateTime";
			this.toolTip1.SetToolTip(this.lblMeetingDateTime, componentResourceManager.GetString("lblMeetingDateTime.ToolTip"));
			componentResourceManager.ApplyResources(this.dtpMeetingDate, "dtpMeetingDate");
			this.dtpMeetingDate.Name = "dtpMeetingDate";
			this.toolTip1.SetToolTip(this.dtpMeetingDate, componentResourceManager.GetString("dtpMeetingDate.ToolTip"));
			this.dtpMeetingDate.Value = new global::System.DateTime(2008, 2, 21, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dtpMeetingTime, "dtpMeetingTime");
			this.dtpMeetingTime.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpMeetingTime.Name = "dtpMeetingTime";
			this.dtpMeetingTime.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dtpMeetingTime, componentResourceManager.GetString("dtpMeetingTime.ToolTip"));
			this.dtpMeetingTime.Value = new global::System.DateTime(2008, 2, 21, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.lblSignBegin, "lblSignBegin");
			this.lblSignBegin.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSignBegin.ForeColor = global::System.Drawing.Color.White;
			this.lblSignBegin.Name = "lblSignBegin";
			this.toolTip1.SetToolTip(this.lblSignBegin, componentResourceManager.GetString("lblSignBegin.ToolTip"));
			componentResourceManager.ApplyResources(this.lblSignEnd, "lblSignEnd");
			this.lblSignEnd.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSignEnd.ForeColor = global::System.Drawing.Color.White;
			this.lblSignEnd.Name = "lblSignEnd";
			this.toolTip1.SetToolTip(this.lblSignEnd, componentResourceManager.GetString("lblSignEnd.ToolTip"));
			componentResourceManager.ApplyResources(this.dtpStartTime, "dtpStartTime");
			this.dtpStartTime.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpStartTime.Name = "dtpStartTime";
			this.dtpStartTime.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dtpStartTime, componentResourceManager.GetString("dtpStartTime.ToolTip"));
			this.dtpStartTime.Value = new global::System.DateTime(2003, 3, 10, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dtpEndTime, "dtpEndTime");
			this.dtpEndTime.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpEndTime.Name = "dtpEndTime";
			this.dtpEndTime.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dtpEndTime, componentResourceManager.GetString("dtpEndTime.ToolTip"));
			this.dtpEndTime.Value = new global::System.DateTime(2003, 3, 10, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.btnCreateInfo, "btnCreateInfo");
			this.btnCreateInfo.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCreateInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCreateInfo.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateInfo.Name = "btnCreateInfo";
			this.toolTip1.SetToolTip(this.btnCreateInfo, componentResourceManager.GetString("btnCreateInfo.ToolTip"));
			this.btnCreateInfo.UseVisualStyleBackColor = false;
			this.btnCreateInfo.Click += new global::System.EventHandler(this.btnCreateInfo_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.toolTip1.SetToolTip(this.btnFind, componentResourceManager.GetString("btnFind.ToolTip"));
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtf_Notes);
			base.Controls.Add(this.lblNotes);
			base.Controls.Add(this.txtf_Content);
			base.Controls.Add(this.cbof_MeetingAdr);
			base.Controls.Add(this.lblContent);
			base.Controls.Add(this.btnAddMeetingAdr);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.lblMeetingName);
			base.Controls.Add(this.txtf_MeetingName);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lblMeetingAddr);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.lblMeetingDateTime);
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.dtpMeetingDate);
			base.Controls.Add(this.txtf_MeetingNo);
			base.Controls.Add(this.dtpMeetingTime);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.btnCreateInfo);
			base.Controls.Add(this.lblSignBegin);
			base.Controls.Add(this.dtpEndTime);
			base.Controls.Add(this.lblSignEnd);
			base.Controls.Add(this.dtpStartTime);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMeetingSet";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmMeetingSet_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmMeetingSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmMeetingSet_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002EDB RID: 11995
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002EDC RID: 11996
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002EDD RID: 11997
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04002EDE RID: 11998
		private global::System.Windows.Forms.Button btnAddAll;

		// Token: 0x04002EDF RID: 11999
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04002EE0 RID: 12000
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04002EE1 RID: 12001
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04002EE2 RID: 12002
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04002EE3 RID: 12003
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04002EE4 RID: 12004
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04002EE5 RID: 12005
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002EE6 RID: 12006
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002EE7 RID: 12007
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002EE8 RID: 12008
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002EE9 RID: 12009
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002EEA RID: 12010
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x04002EEB RID: 12011
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04002EEC RID: 12012
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04002EED RID: 12013
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MoreCards_GrpID;

		// Token: 0x04002EEE RID: 12014
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x04002EEF RID: 12015
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x04002EF0 RID: 12016
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002EF1 RID: 12017
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Identity;

		// Token: 0x04002EF2 RID: 12018
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Identity1;

		// Token: 0x04002EF3 RID: 12019
		private global::System.Windows.Forms.DataGridViewTextBoxColumn IdentityStr;

		// Token: 0x04002EF4 RID: 12020
		private global::System.Windows.Forms.DataGridViewTextBoxColumn IdentityStr2;

		// Token: 0x04002EF5 RID: 12021
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002EF6 RID: 12022
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002EF7 RID: 12023
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x04002EF8 RID: 12024
		private global::System.Windows.Forms.DataGridViewTextBoxColumn SeatNO1;

		// Token: 0x04002EF9 RID: 12025
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04002EFA RID: 12026
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04002EFB RID: 12027
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x04002EFC RID: 12028
		internal global::System.Windows.Forms.Button btnAddMeetingAdr;

		// Token: 0x04002EFD RID: 12029
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002EFE RID: 12030
		internal global::System.Windows.Forms.Button btnCreateInfo;

		// Token: 0x04002EFF RID: 12031
		internal global::System.Windows.Forms.Button btnFind;

		// Token: 0x04002F00 RID: 12032
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002F01 RID: 12033
		internal global::System.Windows.Forms.ComboBox cbof_MeetingAdr;

		// Token: 0x04002F02 RID: 12034
		internal global::System.Windows.Forms.ComboBox cboIdentity;

		// Token: 0x04002F03 RID: 12035
		internal global::System.Windows.Forms.DateTimePicker dtpEndTime;

		// Token: 0x04002F04 RID: 12036
		internal global::System.Windows.Forms.DateTimePicker dtpMeetingDate;

		// Token: 0x04002F05 RID: 12037
		internal global::System.Windows.Forms.DateTimePicker dtpMeetingTime;

		// Token: 0x04002F06 RID: 12038
		internal global::System.Windows.Forms.DateTimePicker dtpStartTime;

		// Token: 0x04002F07 RID: 12039
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x04002F08 RID: 12040
		internal global::System.Windows.Forms.Label Label9;

		// Token: 0x04002F09 RID: 12041
		internal global::System.Windows.Forms.Label lblContent;

		// Token: 0x04002F0A RID: 12042
		internal global::System.Windows.Forms.Label lblControlTimeSeg;

		// Token: 0x04002F0B RID: 12043
		internal global::System.Windows.Forms.Label lblMeetingAddr;

		// Token: 0x04002F0C RID: 12044
		internal global::System.Windows.Forms.Label lblMeetingDateTime;

		// Token: 0x04002F0D RID: 12045
		internal global::System.Windows.Forms.Label lblMeetingName;

		// Token: 0x04002F0E RID: 12046
		internal global::System.Windows.Forms.Label lblNotes;

		// Token: 0x04002F0F RID: 12047
		internal global::System.Windows.Forms.Label lblSignBegin;

		// Token: 0x04002F10 RID: 12048
		internal global::System.Windows.Forms.Label lblSignEnd;

		// Token: 0x04002F11 RID: 12049
		internal global::System.Windows.Forms.TextBox txtf_Content;

		// Token: 0x04002F12 RID: 12050
		internal global::System.Windows.Forms.TextBox txtf_MeetingName;

		// Token: 0x04002F13 RID: 12051
		internal global::System.Windows.Forms.TextBox txtf_MeetingNo;

		// Token: 0x04002F14 RID: 12052
		internal global::System.Windows.Forms.TextBox txtf_Notes;

		// Token: 0x04002F15 RID: 12053
		internal global::System.Windows.Forms.TextBox txtSeat;
	}
}
