namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000236 RID: 566
	public partial class dfrmControllerExtendFuncPasswordManage : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x001390E4 File Offset: 0x001380E4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmWait1 != null)
			{
				this.dfrmWait1.Dispose();
			}
			if (disposing && this.dfrmFind1 != null)
			{
				this.dfrmFind1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0013913C File Offset: 0x0013813C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerExtendFuncPasswordManage));
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
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.label5 = new global::System.Windows.Forms.Label();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_ReaderID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReaderNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReaderName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PasswordEnabled = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.label6 = new global::System.Windows.Forms.Label();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnChangePassword = new global::System.Windows.Forms.Button();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Deptname = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.strPwd = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.txtPasswordNew = new global::System.Windows.Forms.TextBox();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.label3 = new global::System.Windows.Forms.Label();
			this.cboReader = new global::System.Windows.Forms.ComboBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.dataGridView3 = new global::System.Windows.Forms.DataGridView();
			this.f_Id = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Password = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_AdaptTo = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.dataGridView4 = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			this.tabPage3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView3).BeginInit();
			this.tabPage4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView4).BeginInit();
			base.SuspendLayout();
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
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
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.dataGridView1);
			this.tabPage1.ForeColor = global::System.Drawing.Color.White;
			this.tabPage1.Name = "tabPage1";
			this.toolTip1.SetToolTip(this.tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.FromArgb(255, 255, 128);
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ReaderID, this.f_ControllerSN, this.f_ReaderNO, this.f_ReaderName, this.f_PasswordEnabled });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.toolTip1.SetToolTip(this.dataGridView1, componentResourceManager.GetString("dataGridView1.ToolTip"));
			componentResourceManager.ApplyResources(this.f_ReaderID, "f_ReaderID");
			this.f_ReaderID.Name = "f_ReaderID";
			this.f_ReaderID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReaderNO, "f_ReaderNO");
			this.f_ReaderNO.Name = "f_ReaderNO";
			this.f_ReaderNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReaderName, "f_ReaderName");
			this.f_ReaderName.Name = "f_ReaderName";
			this.f_ReaderName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_PasswordEnabled, "f_PasswordEnabled");
			this.f_PasswordEnabled.Name = "f_PasswordEnabled";
			this.f_PasswordEnabled.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_PasswordEnabled.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.cbof_GroupID);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.btnChangePassword);
			this.tabPage2.Controls.Add(this.dgvUsers);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.toolTip1.SetToolTip(this.tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.FromArgb(255, 255, 128);
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
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
			componentResourceManager.ApplyResources(this.btnChangePassword, "btnChangePassword");
			this.btnChangePassword.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnChangePassword.Name = "btnChangePassword";
			this.toolTip1.SetToolTip(this.btnChangePassword, componentResourceManager.GetString("btnChangePassword.ToolTip"));
			this.btnChangePassword.UseVisualStyleBackColor = true;
			this.btnChangePassword.Click += new global::System.EventHandler(this.btnChangePassword_Click);
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.ConsumerNO, this.ConsumerName, this.CardNO, this.Deptname, this.strPwd });
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvUsers_Scroll);
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.dgvUsers_DoubleClick);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.ConsumerNO.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.ConsumerNO, "ConsumerNO");
			this.ConsumerNO.Name = "ConsumerNO";
			this.ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.CardNO.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			this.Deptname.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Deptname, "Deptname");
			this.Deptname.Name = "Deptname";
			this.Deptname.ReadOnly = true;
			componentResourceManager.ApplyResources(this.strPwd, "strPwd");
			this.strPwd.Name = "strPwd";
			this.strPwd.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage3.Controls.Add(this.checkBox1);
			this.tabPage3.Controls.Add(this.txtPasswordNew);
			this.tabPage3.Controls.Add(this.btnDel);
			this.tabPage3.Controls.Add(this.btnAdd);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.cboReader);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.dataGridView3);
			this.tabPage3.ForeColor = global::System.Drawing.Color.White;
			this.tabPage3.Name = "tabPage3";
			this.toolTip1.SetToolTip(this.tabPage3, componentResourceManager.GetString("tabPage3.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Name = "checkBox1";
			this.toolTip1.SetToolTip(this.checkBox1, componentResourceManager.GetString("checkBox1.ToolTip"));
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			componentResourceManager.ApplyResources(this.txtPasswordNew, "txtPasswordNew");
			this.txtPasswordNew.Name = "txtPasswordNew";
			this.toolTip1.SetToolTip(this.txtPasswordNew, componentResourceManager.GetString("txtPasswordNew.ToolTip"));
			this.txtPasswordNew.UseSystemPasswordChar = true;
			this.txtPasswordNew.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtPasswordNew_KeyPress);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.Name = "btnDel";
			this.toolTip1.SetToolTip(this.btnDel, componentResourceManager.GetString("btnDel.ToolTip"));
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.Name = "btnAdd";
			this.toolTip1.SetToolTip(this.btnAdd, componentResourceManager.GetString("btnAdd.ToolTip"));
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.FromArgb(255, 255, 128);
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.cboReader, "cboReader");
			this.cboReader.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboReader.FormattingEnabled = true;
			this.cboReader.Name = "cboReader";
			this.toolTip1.SetToolTip(this.cboReader, componentResourceManager.GetString("cboReader.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.dataGridView3, "dataGridView3");
			this.dataGridView3.AllowUserToAddRows = false;
			this.dataGridView3.AllowUserToDeleteRows = false;
			this.dataGridView3.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dataGridView3.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView3.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_Id, this.f_Password, this.Column1, this.f_AdaptTo });
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView3.DefaultCellStyle = dataGridViewCellStyle8;
			this.dataGridView3.EnableHeadersVisualStyles = false;
			this.dataGridView3.Name = "dataGridView3";
			this.dataGridView3.ReadOnly = true;
			this.dataGridView3.RowTemplate.Height = 23;
			this.dataGridView3.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dataGridView3, componentResourceManager.GetString("dataGridView3.ToolTip"));
			componentResourceManager.ApplyResources(this.f_Id, "f_Id");
			this.f_Id.Name = "f_Id";
			this.f_Id.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Password, "f_Password");
			this.f_Password.Name = "f_Password";
			this.f_Password.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.f_AdaptTo.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_AdaptTo, "f_AdaptTo");
			this.f_AdaptTo.Name = "f_AdaptTo";
			this.f_AdaptTo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage4.Controls.Add(this.dataGridView4);
			this.tabPage4.ForeColor = global::System.Drawing.Color.White;
			this.tabPage4.Name = "tabPage4";
			this.toolTip1.SetToolTip(this.tabPage4, componentResourceManager.GetString("tabPage4.ToolTip"));
			componentResourceManager.ApplyResources(this.dataGridView4, "dataGridView4");
			this.dataGridView4.AllowUserToAddRows = false;
			this.dataGridView4.AllowUserToDeleteRows = false;
			this.dataGridView4.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle9.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle9.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle9.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView4.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridView4.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView4.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewCheckBoxColumn1 });
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle10.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle10.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView4.DefaultCellStyle = dataGridViewCellStyle10;
			this.dataGridView4.EnableHeadersVisualStyles = false;
			this.dataGridView4.Name = "dataGridView4";
			this.dataGridView4.RowTemplate.Height = 23;
			this.toolTip1.SetToolTip(this.dataGridView4, componentResourceManager.GetString("dataGridView4.ToolTip"));
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
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
			this.dataGridViewCheckBoxColumn1.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewCheckBoxColumn1.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.tabControl1);
			base.Name = "dfrmControllerExtendFuncPasswordManage";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerExtendFuncPasswordManage_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmControllerExtendFuncPasswordManage_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmControllerExtendFuncPasswordManage_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerExtendFuncPasswordManage_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView3).EndInit();
			this.tabPage4.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView4).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001E4C RID: 7756
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x04001E50 RID: 7760
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E51 RID: 7761
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04001E52 RID: 7762
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04001E53 RID: 7763
		private global::System.Windows.Forms.Button btnChangePassword;

		// Token: 0x04001E54 RID: 7764
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04001E55 RID: 7765
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04001E56 RID: 7766
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04001E57 RID: 7767
		private global::System.Windows.Forms.ComboBox cboReader;

		// Token: 0x04001E58 RID: 7768
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04001E59 RID: 7769
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04001E5A RID: 7770
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04001E5B RID: 7771
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04001E5C RID: 7772
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerNO;

		// Token: 0x04001E5D RID: 7773
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001E5E RID: 7774
		private global::System.Windows.Forms.DataGridView dataGridView3;

		// Token: 0x04001E5F RID: 7775
		private global::System.Windows.Forms.DataGridView dataGridView4;

		// Token: 0x04001E60 RID: 7776
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04001E61 RID: 7777
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04001E62 RID: 7778
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04001E63 RID: 7779
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04001E64 RID: 7780
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04001E65 RID: 7781
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Deptname;

		// Token: 0x04001E66 RID: 7782
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04001E67 RID: 7783
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AdaptTo;

		// Token: 0x04001E68 RID: 7784
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001E69 RID: 7785
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Id;

		// Token: 0x04001E6A RID: 7786
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Password;

		// Token: 0x04001E6B RID: 7787
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_PasswordEnabled;

		// Token: 0x04001E6C RID: 7788
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderID;

		// Token: 0x04001E6D RID: 7789
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderName;

		// Token: 0x04001E6E RID: 7790
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderNO;

		// Token: 0x04001E6F RID: 7791
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001E70 RID: 7792
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001E71 RID: 7793
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001E72 RID: 7794
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04001E73 RID: 7795
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04001E74 RID: 7796
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04001E75 RID: 7797
		private global::System.Windows.Forms.DataGridViewTextBoxColumn strPwd;

		// Token: 0x04001E76 RID: 7798
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04001E77 RID: 7799
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04001E78 RID: 7800
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04001E79 RID: 7801
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04001E7A RID: 7802
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04001E7B RID: 7803
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04001E7C RID: 7804
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001E7D RID: 7805
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001E7E RID: 7806
		internal global::System.Windows.Forms.TextBox txtPasswordNew;
	}
}
