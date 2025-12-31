namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000242 RID: 578
	public partial class dfrmFirstCard : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x0014F56F File Offset: 0x0014E56F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0014F590 File Offset: 0x0014E590
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmFirstCard));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.grpWeekdayControl = new global::System.Windows.Forms.GroupBox();
			this.chkMonday = new global::System.Windows.Forms.CheckBox();
			this.chkSunday = new global::System.Windows.Forms.CheckBox();
			this.chkTuesday = new global::System.Windows.Forms.CheckBox();
			this.chkSaturday = new global::System.Windows.Forms.CheckBox();
			this.chkWednesday = new global::System.Windows.Forms.CheckBox();
			this.chkFriday = new global::System.Windows.Forms.CheckBox();
			this.chkThursday = new global::System.Windows.Forms.CheckBox();
			this.grpEnd = new global::System.Windows.Forms.GroupBox();
			this.cboEndControlStatus = new global::System.Windows.Forms.ComboBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.label8 = new global::System.Windows.Forms.Label();
			this.grpUsers = new global::System.Windows.Forms.GroupBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.grpBegin = new global::System.Windows.Forms.GroupBox();
			this.cboBeginControlStatus = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.grpWeekdayControl.SuspendLayout();
			this.grpEnd.SuspendLayout();
			this.grpUsers.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			this.grpBegin.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.grpWeekdayControl, "grpWeekdayControl");
			this.grpWeekdayControl.BackColor = global::System.Drawing.Color.Transparent;
			this.grpWeekdayControl.Controls.Add(this.chkMonday);
			this.grpWeekdayControl.Controls.Add(this.chkSunday);
			this.grpWeekdayControl.Controls.Add(this.chkTuesday);
			this.grpWeekdayControl.Controls.Add(this.chkSaturday);
			this.grpWeekdayControl.Controls.Add(this.chkWednesday);
			this.grpWeekdayControl.Controls.Add(this.chkFriday);
			this.grpWeekdayControl.Controls.Add(this.chkThursday);
			this.grpWeekdayControl.ForeColor = global::System.Drawing.Color.White;
			this.grpWeekdayControl.Name = "grpWeekdayControl";
			this.grpWeekdayControl.TabStop = false;
			this.toolTip1.SetToolTip(this.grpWeekdayControl, componentResourceManager.GetString("grpWeekdayControl.ToolTip"));
			componentResourceManager.ApplyResources(this.chkMonday, "chkMonday");
			this.chkMonday.Checked = true;
			this.chkMonday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkMonday.Name = "chkMonday";
			this.toolTip1.SetToolTip(this.chkMonday, componentResourceManager.GetString("chkMonday.ToolTip"));
			this.chkMonday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSunday, "chkSunday");
			this.chkSunday.Checked = true;
			this.chkSunday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSunday.Name = "chkSunday";
			this.toolTip1.SetToolTip(this.chkSunday, componentResourceManager.GetString("chkSunday.ToolTip"));
			this.chkSunday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkTuesday, "chkTuesday");
			this.chkTuesday.Checked = true;
			this.chkTuesday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkTuesday.Name = "chkTuesday";
			this.toolTip1.SetToolTip(this.chkTuesday, componentResourceManager.GetString("chkTuesday.ToolTip"));
			this.chkTuesday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSaturday, "chkSaturday");
			this.chkSaturday.Checked = true;
			this.chkSaturday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkSaturday.Name = "chkSaturday";
			this.toolTip1.SetToolTip(this.chkSaturday, componentResourceManager.GetString("chkSaturday.ToolTip"));
			this.chkSaturday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkWednesday, "chkWednesday");
			this.chkWednesday.Checked = true;
			this.chkWednesday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkWednesday.Name = "chkWednesday";
			this.toolTip1.SetToolTip(this.chkWednesday, componentResourceManager.GetString("chkWednesday.ToolTip"));
			this.chkWednesday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkFriday, "chkFriday");
			this.chkFriday.Checked = true;
			this.chkFriday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkFriday.Name = "chkFriday";
			this.toolTip1.SetToolTip(this.chkFriday, componentResourceManager.GetString("chkFriday.ToolTip"));
			this.chkFriday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkThursday, "chkThursday");
			this.chkThursday.Checked = true;
			this.chkThursday.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkThursday.Name = "chkThursday";
			this.toolTip1.SetToolTip(this.chkThursday, componentResourceManager.GetString("chkThursday.ToolTip"));
			this.chkThursday.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.grpEnd, "grpEnd");
			this.grpEnd.BackColor = global::System.Drawing.Color.Transparent;
			this.grpEnd.Controls.Add(this.cboEndControlStatus);
			this.grpEnd.Controls.Add(this.label2);
			this.grpEnd.Controls.Add(this.label6);
			this.grpEnd.Controls.Add(this.dateEndHMS1);
			this.grpEnd.Controls.Add(this.label8);
			this.grpEnd.ForeColor = global::System.Drawing.Color.White;
			this.grpEnd.Name = "grpEnd";
			this.grpEnd.TabStop = false;
			this.toolTip1.SetToolTip(this.grpEnd, componentResourceManager.GetString("grpEnd.ToolTip"));
			componentResourceManager.ApplyResources(this.cboEndControlStatus, "cboEndControlStatus");
			this.cboEndControlStatus.AutoCompleteCustomSource.AddRange(new string[]
			{
				componentResourceManager.GetString("cboEndControlStatus.AutoCompleteCustomSource"),
				componentResourceManager.GetString("cboEndControlStatus.AutoCompleteCustomSource1"),
				componentResourceManager.GetString("cboEndControlStatus.AutoCompleteCustomSource2")
			});
			this.cboEndControlStatus.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEndControlStatus.FormattingEnabled = true;
			this.cboEndControlStatus.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboEndControlStatus.Items"),
				componentResourceManager.GetString("cboEndControlStatus.Items1"),
				componentResourceManager.GetString("cboEndControlStatus.Items2"),
				componentResourceManager.GetString("cboEndControlStatus.Items3")
			});
			this.cboEndControlStatus.Name = "cboEndControlStatus";
			this.toolTip1.SetToolTip(this.cboEndControlStatus, componentResourceManager.GetString("cboEndControlStatus.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateEndHMS1, componentResourceManager.GetString("dateEndHMS1.ToolTip"));
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 8, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, componentResourceManager.GetString("label8.ToolTip"));
			componentResourceManager.ApplyResources(this.grpUsers, "grpUsers");
			this.grpUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.grpUsers.Controls.Add(this.lblWait);
			this.grpUsers.Controls.Add(this.label3);
			this.grpUsers.Controls.Add(this.dgvSelectedUsers);
			this.grpUsers.Controls.Add(this.dgvUsers);
			this.grpUsers.Controls.Add(this.btnDelAllUsers);
			this.grpUsers.Controls.Add(this.btnDelOneUser);
			this.grpUsers.Controls.Add(this.btnAddOneUser);
			this.grpUsers.Controls.Add(this.btnAddAllUsers);
			this.grpUsers.Controls.Add(this.cbof_GroupID);
			this.grpUsers.Controls.Add(this.label4);
			this.grpUsers.ForeColor = global::System.Drawing.Color.White;
			this.grpUsers.Name = "grpUsers";
			this.grpUsers.TabStop = false;
			this.toolTip1.SetToolTip(this.grpUsers, componentResourceManager.GetString("grpUsers.ToolTip"));
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
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
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup });
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
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
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
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.UserID, this.ConsumerName, this.CardNO, this.f_SelectedUsers, this.f_GroupID });
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
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
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
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActive.ForeColor = global::System.Drawing.Color.White;
			this.chkActive.Name = "chkActive";
			this.toolTip1.SetToolTip(this.chkActive, componentResourceManager.GetString("chkActive.ToolTip"));
			this.chkActive.UseVisualStyleBackColor = false;
			this.chkActive.CheckedChanged += new global::System.EventHandler(this.chkActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpBegin, "grpBegin");
			this.grpBegin.BackColor = global::System.Drawing.Color.Transparent;
			this.grpBegin.Controls.Add(this.cboBeginControlStatus);
			this.grpBegin.Controls.Add(this.label1);
			this.grpBegin.Controls.Add(this.label7);
			this.grpBegin.Controls.Add(this.dateBeginHMS1);
			this.grpBegin.Controls.Add(this.Label5);
			this.grpBegin.ForeColor = global::System.Drawing.Color.White;
			this.grpBegin.Name = "grpBegin";
			this.grpBegin.TabStop = false;
			this.toolTip1.SetToolTip(this.grpBegin, componentResourceManager.GetString("grpBegin.ToolTip"));
			componentResourceManager.ApplyResources(this.cboBeginControlStatus, "cboBeginControlStatus");
			this.cboBeginControlStatus.AutoCompleteCustomSource.AddRange(new string[]
			{
				componentResourceManager.GetString("cboBeginControlStatus.AutoCompleteCustomSource"),
				componentResourceManager.GetString("cboBeginControlStatus.AutoCompleteCustomSource1"),
				componentResourceManager.GetString("cboBeginControlStatus.AutoCompleteCustomSource2")
			});
			this.cboBeginControlStatus.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBeginControlStatus.FormattingEnabled = true;
			this.cboBeginControlStatus.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboBeginControlStatus.Items"),
				componentResourceManager.GetString("cboBeginControlStatus.Items1"),
				componentResourceManager.GetString("cboBeginControlStatus.Items2")
			});
			this.cboBeginControlStatus.Name = "cboBeginControlStatus";
			this.toolTip1.SetToolTip(this.cboBeginControlStatus, componentResourceManager.GetString("cboBeginControlStatus.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			this.toolTip1.SetToolTip(this.label7, componentResourceManager.GetString("label7.ToolTip"));
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateBeginHMS1, componentResourceManager.GetString("dateBeginHMS1.ToolTip"));
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 8, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.Name = "Label5";
			this.toolTip1.SetToolTip(this.Label5, componentResourceManager.GetString("Label5.ToolTip"));
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.grpWeekdayControl);
			base.Controls.Add(this.grpEnd);
			base.Controls.Add(this.grpBegin);
			base.Controls.Add(this.chkActive);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.grpUsers);
			base.Name = "dfrmFirstCard";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrm_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmFirstCard_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrm_KeyDown);
			this.grpWeekdayControl.ResumeLayout(false);
			this.grpWeekdayControl.PerformLayout();
			this.grpEnd.ResumeLayout(false);
			this.grpEnd.PerformLayout();
			this.grpUsers.ResumeLayout(false);
			this.grpUsers.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.grpBegin.ResumeLayout(false);
			this.grpBegin.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001FF6 RID: 8182
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001FF7 RID: 8183
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001FF8 RID: 8184
		internal global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04001FFB RID: 8187
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x04001FFC RID: 8188
		internal global::System.Windows.Forms.Label label8;

		// Token: 0x04002000 RID: 8192
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002001 RID: 8193
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002002 RID: 8194
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x04002003 RID: 8195
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x04002004 RID: 8196
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04002005 RID: 8197
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04002006 RID: 8198
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04002007 RID: 8199
		private global::System.Windows.Forms.ComboBox cboBeginControlStatus;

		// Token: 0x04002008 RID: 8200
		private global::System.Windows.Forms.ComboBox cboEndControlStatus;

		// Token: 0x04002009 RID: 8201
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x0400200A RID: 8202
		private global::System.Windows.Forms.CheckBox chkFriday;

		// Token: 0x0400200B RID: 8203
		private global::System.Windows.Forms.CheckBox chkMonday;

		// Token: 0x0400200C RID: 8204
		private global::System.Windows.Forms.CheckBox chkSaturday;

		// Token: 0x0400200D RID: 8205
		private global::System.Windows.Forms.CheckBox chkSunday;

		// Token: 0x0400200E RID: 8206
		private global::System.Windows.Forms.CheckBox chkThursday;

		// Token: 0x0400200F RID: 8207
		private global::System.Windows.Forms.CheckBox chkTuesday;

		// Token: 0x04002010 RID: 8208
		private global::System.Windows.Forms.CheckBox chkWednesday;

		// Token: 0x04002011 RID: 8209
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04002012 RID: 8210
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04002013 RID: 8211
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002014 RID: 8212
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002015 RID: 8213
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002016 RID: 8214
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002017 RID: 8215
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002018 RID: 8216
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x04002019 RID: 8217
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x0400201A RID: 8218
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x0400201B RID: 8219
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x0400201C RID: 8220
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x0400201D RID: 8221
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x0400201E RID: 8222
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x0400201F RID: 8223
		private global::System.Windows.Forms.GroupBox grpBegin;

		// Token: 0x04002020 RID: 8224
		private global::System.Windows.Forms.GroupBox grpEnd;

		// Token: 0x04002021 RID: 8225
		private global::System.Windows.Forms.GroupBox grpUsers;

		// Token: 0x04002022 RID: 8226
		private global::System.Windows.Forms.GroupBox grpWeekdayControl;

		// Token: 0x04002023 RID: 8227
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002024 RID: 8228
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002025 RID: 8229
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002026 RID: 8230
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002027 RID: 8231
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04002028 RID: 8232
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04002029 RID: 8233
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x0400202A RID: 8234
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400202B RID: 8235
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400202C RID: 8236
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;
	}
}
