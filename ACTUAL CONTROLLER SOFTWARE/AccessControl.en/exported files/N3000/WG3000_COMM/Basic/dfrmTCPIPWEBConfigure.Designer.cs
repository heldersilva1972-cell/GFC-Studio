namespace WG3000_COMM.Basic
{
	// Token: 0x02000030 RID: 48
	public partial class dfrmTCPIPWEBConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00061E84 File Offset: 0x00060E84
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00061EA4 File Offset: 0x00060EA4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmTCPIPWEBConfigure));
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.grpWEBEnabled = new global::System.Windows.Forms.GroupBox();
			this.optWEBEnabled = new global::System.Windows.Forms.RadioButton();
			this.optWEBDisable = new global::System.Windows.Forms.RadioButton();
			this.grpWEBUsers = new global::System.Windows.Forms.GroupBox();
			this.txtUsersFile = new global::System.Windows.Forms.TextBox();
			this.dataGridView3 = new global::System.Windows.Forms.DataGridView();
			this.chkAutoUploadWEBUsers = new global::System.Windows.Forms.CheckBox();
			this.cboLanguage2 = new global::System.Windows.Forms.ComboBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.btnEditUsers = new global::System.Windows.Forms.Button();
			this.btnDownloadUsers = new global::System.Windows.Forms.Button();
			this.btnSelectUserFile = new global::System.Windows.Forms.Button();
			this.btnuploadUser = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtf_ControllerSN = new global::System.Windows.Forms.TextBox();
			this.txtf_MACAddr = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtf_IP = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.txtf_mask = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.txtf_gateway = new global::System.Windows.Forms.TextBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnOption = new global::System.Windows.Forms.Button();
			this.lblPort = new global::System.Windows.Forms.Label();
			this.grpIP = new global::System.Windows.Forms.GroupBox();
			this.nudPort = new global::System.Windows.Forms.NumericUpDown();
			this.chkEditIP = new global::System.Windows.Forms.CheckBox();
			this.grpWEB = new global::System.Windows.Forms.GroupBox();
			this.chkWebOnlyQuery = new global::System.Windows.Forms.CheckBox();
			this.cboDateFormat = new global::System.Windows.Forms.ComboBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.btnOptionWEB = new global::System.Windows.Forms.Button();
			this.lblHttpPort = new global::System.Windows.Forms.Label();
			this.nudHttpPort = new global::System.Windows.Forms.NumericUpDown();
			this.cboLanguage = new global::System.Windows.Forms.ComboBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.btnSelectFile = new global::System.Windows.Forms.Button();
			this.btnOtherLanguage = new global::System.Windows.Forms.Button();
			this.txtSelectedFileName = new global::System.Windows.Forms.TextBox();
			this.chkUpdateWebSet = new global::System.Windows.Forms.CheckBox();
			this.chkUpdateSuperCard = new global::System.Windows.Forms.CheckBox();
			this.grpSuperCards = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.txtSuperCard2 = new global::System.Windows.Forms.MaskedTextBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtSuperCard1 = new global::System.Windows.Forms.MaskedTextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.chkAdjustTime = new global::System.Windows.Forms.CheckBox();
			this.btnTryWEB = new global::System.Windows.Forms.Button();
			this.btnRestoreNameAndPassword = new global::System.Windows.Forms.Button();
			this.chkUpdateSpecialCard = new global::System.Windows.Forms.CheckBox();
			this.grpSpecialCards = new global::System.Windows.Forms.GroupBox();
			this.txtSpecialCard2 = new global::System.Windows.Forms.MaskedTextBox();
			this.label13 = new global::System.Windows.Forms.Label();
			this.txtSpecialCard1 = new global::System.Windows.Forms.MaskedTextBox();
			this.label14 = new global::System.Windows.Forms.Label();
			this.grpWEBEnabled.SuspendLayout();
			this.grpWEBUsers.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView3).BeginInit();
			this.grpIP.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).BeginInit();
			this.grpWEB.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudHttpPort).BeginInit();
			this.grpSuperCards.SuspendLayout();
			this.grpSpecialCards.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.grpWEBEnabled, "grpWEBEnabled");
			this.grpWEBEnabled.Controls.Add(this.optWEBEnabled);
			this.grpWEBEnabled.Controls.Add(this.optWEBDisable);
			this.grpWEBEnabled.ForeColor = global::System.Drawing.Color.White;
			this.grpWEBEnabled.Name = "grpWEBEnabled";
			this.grpWEBEnabled.TabStop = false;
			componentResourceManager.ApplyResources(this.optWEBEnabled, "optWEBEnabled");
			this.optWEBEnabled.Checked = true;
			this.optWEBEnabled.ForeColor = global::System.Drawing.Color.White;
			this.optWEBEnabled.Name = "optWEBEnabled";
			this.optWEBEnabled.TabStop = true;
			this.optWEBEnabled.UseVisualStyleBackColor = true;
			this.optWEBEnabled.CheckedChanged += new global::System.EventHandler(this.optWEBEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.optWEBDisable, "optWEBDisable");
			this.optWEBDisable.ForeColor = global::System.Drawing.Color.White;
			this.optWEBDisable.Name = "optWEBDisable";
			this.optWEBDisable.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.grpWEBUsers, "grpWEBUsers");
			this.grpWEBUsers.Controls.Add(this.txtUsersFile);
			this.grpWEBUsers.Controls.Add(this.dataGridView3);
			this.grpWEBUsers.Controls.Add(this.chkAutoUploadWEBUsers);
			this.grpWEBUsers.Controls.Add(this.cboLanguage2);
			this.grpWEBUsers.Controls.Add(this.label12);
			this.grpWEBUsers.Controls.Add(this.btnEditUsers);
			this.grpWEBUsers.Controls.Add(this.btnDownloadUsers);
			this.grpWEBUsers.Controls.Add(this.btnSelectUserFile);
			this.grpWEBUsers.Controls.Add(this.btnuploadUser);
			this.grpWEBUsers.ForeColor = global::System.Drawing.Color.White;
			this.grpWEBUsers.Name = "grpWEBUsers";
			this.grpWEBUsers.TabStop = false;
			componentResourceManager.ApplyResources(this.txtUsersFile, "txtUsersFile");
			this.txtUsersFile.Name = "txtUsersFile";
			this.txtUsersFile.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridView3, "dataGridView3");
			this.dataGridView3.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView3.Name = "dataGridView3";
			this.dataGridView3.RowTemplate.Height = 23;
			componentResourceManager.ApplyResources(this.chkAutoUploadWEBUsers, "chkAutoUploadWEBUsers");
			this.chkAutoUploadWEBUsers.ForeColor = global::System.Drawing.Color.White;
			this.chkAutoUploadWEBUsers.Name = "chkAutoUploadWEBUsers";
			this.chkAutoUploadWEBUsers.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cboLanguage2, "cboLanguage2");
			this.cboLanguage2.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLanguage2.FormattingEnabled = true;
			this.cboLanguage2.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboLanguage2.Items"),
				componentResourceManager.GetString("cboLanguage2.Items1"),
				componentResourceManager.GetString("cboLanguage2.Items2")
			});
			this.cboLanguage2.Name = "cboLanguage2";
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.BackColor = global::System.Drawing.Color.Transparent;
			this.label12.ForeColor = global::System.Drawing.Color.White;
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.btnEditUsers, "btnEditUsers");
			this.btnEditUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnEditUsers.Name = "btnEditUsers";
			this.btnEditUsers.UseVisualStyleBackColor = false;
			this.btnEditUsers.Click += new global::System.EventHandler(this.btnEditUsers_Click);
			componentResourceManager.ApplyResources(this.btnDownloadUsers, "btnDownloadUsers");
			this.btnDownloadUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDownloadUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDownloadUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnDownloadUsers.Name = "btnDownloadUsers";
			this.btnDownloadUsers.UseVisualStyleBackColor = false;
			this.btnDownloadUsers.Click += new global::System.EventHandler(this.btnDownloadUsers_Click);
			componentResourceManager.ApplyResources(this.btnSelectUserFile, "btnSelectUserFile");
			this.btnSelectUserFile.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectUserFile.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectUserFile.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectUserFile.Name = "btnSelectUserFile";
			this.btnSelectUserFile.UseVisualStyleBackColor = false;
			this.btnSelectUserFile.Click += new global::System.EventHandler(this.btnSelectUserFile_Click);
			componentResourceManager.ApplyResources(this.btnuploadUser, "btnuploadUser");
			this.btnuploadUser.BackColor = global::System.Drawing.Color.Transparent;
			this.btnuploadUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnuploadUser.ForeColor = global::System.Drawing.Color.White;
			this.btnuploadUser.Name = "btnuploadUser";
			this.btnuploadUser.UseVisualStyleBackColor = false;
			this.btnuploadUser.Click += new global::System.EventHandler(this.btnuploadUser_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
			this.txtf_ControllerSN.Name = "txtf_ControllerSN";
			this.txtf_ControllerSN.ReadOnly = true;
			this.txtf_ControllerSN.TabStop = false;
			componentResourceManager.ApplyResources(this.txtf_MACAddr, "txtf_MACAddr");
			this.txtf_MACAddr.Name = "txtf_MACAddr";
			this.txtf_MACAddr.ReadOnly = true;
			this.txtf_MACAddr.TabStop = false;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtf_IP, "txtf_IP");
			this.txtf_IP.Name = "txtf_IP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.txtf_mask, "txtf_mask");
			this.txtf_mask.Name = "txtf_mask";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.txtf_gateway, "txtf_gateway");
			this.txtf_gateway.Name = "txtf_gateway";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.btnOption.TabStop = false;
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this.lblPort, "lblPort");
			this.lblPort.ForeColor = global::System.Drawing.Color.White;
			this.lblPort.Name = "lblPort";
			componentResourceManager.ApplyResources(this.grpIP, "grpIP");
			this.grpIP.BackColor = global::System.Drawing.Color.Transparent;
			this.grpIP.Controls.Add(this.nudPort);
			this.grpIP.Controls.Add(this.lblPort);
			this.grpIP.Controls.Add(this.label3);
			this.grpIP.Controls.Add(this.txtf_gateway);
			this.grpIP.Controls.Add(this.label5);
			this.grpIP.Controls.Add(this.txtf_IP);
			this.grpIP.Controls.Add(this.txtf_mask);
			this.grpIP.Controls.Add(this.label4);
			this.grpIP.Controls.Add(this.btnOption);
			this.grpIP.Name = "grpIP";
			this.grpIP.TabStop = false;
			componentResourceManager.ApplyResources(this.nudPort, "nudPort");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudPort;
			int[] array = new int[4];
			array[0] = 65534;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudPort;
			int[] array2 = new int[4];
			array2[0] = 1024;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudPort.Name = "nudPort";
			this.nudPort.TabStop = false;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudPort;
			int[] array3 = new int[4];
			array3[0] = 60000;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkEditIP, "chkEditIP");
			this.chkEditIP.ForeColor = global::System.Drawing.Color.White;
			this.chkEditIP.Name = "chkEditIP";
			this.chkEditIP.UseVisualStyleBackColor = true;
			this.chkEditIP.CheckedChanged += new global::System.EventHandler(this.chkEditIP_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpWEB, "grpWEB");
			this.grpWEB.Controls.Add(this.chkWebOnlyQuery);
			this.grpWEB.Controls.Add(this.cboDateFormat);
			this.grpWEB.Controls.Add(this.label6);
			this.grpWEB.Controls.Add(this.btnOptionWEB);
			this.grpWEB.Controls.Add(this.lblHttpPort);
			this.grpWEB.Controls.Add(this.nudHttpPort);
			this.grpWEB.Controls.Add(this.cboLanguage);
			this.grpWEB.Controls.Add(this.label8);
			this.grpWEB.Controls.Add(this.btnSelectFile);
			this.grpWEB.Controls.Add(this.btnOtherLanguage);
			this.grpWEB.Controls.Add(this.txtSelectedFileName);
			this.grpWEB.Name = "grpWEB";
			this.grpWEB.TabStop = false;
			componentResourceManager.ApplyResources(this.chkWebOnlyQuery, "chkWebOnlyQuery");
			this.chkWebOnlyQuery.ForeColor = global::System.Drawing.Color.White;
			this.chkWebOnlyQuery.Name = "chkWebOnlyQuery";
			this.chkWebOnlyQuery.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cboDateFormat, "cboDateFormat");
			this.cboDateFormat.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDateFormat.FormattingEnabled = true;
			this.cboDateFormat.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboDateFormat.Items"),
				componentResourceManager.GetString("cboDateFormat.Items1"),
				componentResourceManager.GetString("cboDateFormat.Items2"),
				componentResourceManager.GetString("cboDateFormat.Items3"),
				componentResourceManager.GetString("cboDateFormat.Items4"),
				componentResourceManager.GetString("cboDateFormat.Items5")
			});
			this.cboDateFormat.Name = "cboDateFormat";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.BackColor = global::System.Drawing.Color.Transparent;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.btnOptionWEB, "btnOptionWEB");
			this.btnOptionWEB.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOptionWEB.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOptionWEB.ForeColor = global::System.Drawing.Color.White;
			this.btnOptionWEB.Name = "btnOptionWEB";
			this.btnOptionWEB.TabStop = false;
			this.btnOptionWEB.UseVisualStyleBackColor = false;
			this.btnOptionWEB.Click += new global::System.EventHandler(this.btnOptionWEB_Click);
			componentResourceManager.ApplyResources(this.lblHttpPort, "lblHttpPort");
			this.lblHttpPort.BackColor = global::System.Drawing.Color.Transparent;
			this.lblHttpPort.ForeColor = global::System.Drawing.Color.White;
			this.lblHttpPort.Name = "lblHttpPort";
			componentResourceManager.ApplyResources(this.nudHttpPort, "nudHttpPort");
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudHttpPort;
			int[] array4 = new int[4];
			array4[0] = 65535;
			numericUpDown4.Maximum = new decimal(array4);
			this.nudHttpPort.Name = "nudHttpPort";
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudHttpPort;
			int[] array5 = new int[4];
			array5[0] = 80;
			numericUpDown5.Value = new decimal(array5);
			componentResourceManager.ApplyResources(this.cboLanguage, "cboLanguage");
			this.cboLanguage.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLanguage.FormattingEnabled = true;
			this.cboLanguage.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboLanguage.Items"),
				componentResourceManager.GetString("cboLanguage.Items1"),
				componentResourceManager.GetString("cboLanguage.Items2")
			});
			this.cboLanguage.Name = "cboLanguage";
			this.cboLanguage.SelectedIndexChanged += new global::System.EventHandler(this.cboLanguage_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.BackColor = global::System.Drawing.Color.Transparent;
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.btnSelectFile, "btnSelectFile");
			this.btnSelectFile.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectFile.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectFile.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.UseVisualStyleBackColor = false;
			this.btnSelectFile.Click += new global::System.EventHandler(this.btnSelectFile_Click);
			componentResourceManager.ApplyResources(this.btnOtherLanguage, "btnOtherLanguage");
			this.btnOtherLanguage.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOtherLanguage.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOtherLanguage.ForeColor = global::System.Drawing.Color.White;
			this.btnOtherLanguage.Name = "btnOtherLanguage";
			this.btnOtherLanguage.UseVisualStyleBackColor = false;
			this.btnOtherLanguage.Click += new global::System.EventHandler(this.btnOtherLanguage_Click);
			componentResourceManager.ApplyResources(this.txtSelectedFileName, "txtSelectedFileName");
			this.txtSelectedFileName.Name = "txtSelectedFileName";
			this.txtSelectedFileName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.chkUpdateWebSet, "chkUpdateWebSet");
			this.chkUpdateWebSet.ForeColor = global::System.Drawing.Color.White;
			this.chkUpdateWebSet.Name = "chkUpdateWebSet";
			this.chkUpdateWebSet.UseVisualStyleBackColor = true;
			this.chkUpdateWebSet.CheckedChanged += new global::System.EventHandler(this.chkUpdateWebSet_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkUpdateSuperCard, "chkUpdateSuperCard");
			this.chkUpdateSuperCard.ForeColor = global::System.Drawing.Color.White;
			this.chkUpdateSuperCard.Name = "chkUpdateSuperCard";
			this.chkUpdateSuperCard.UseVisualStyleBackColor = true;
			this.chkUpdateSuperCard.CheckedChanged += new global::System.EventHandler(this.chkUpdateSuperCard_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpSuperCards, "grpSuperCards");
			this.grpSuperCards.Controls.Add(this.label9);
			this.grpSuperCards.Controls.Add(this.txtSuperCard2);
			this.grpSuperCards.Controls.Add(this.label10);
			this.grpSuperCards.Controls.Add(this.txtSuperCard1);
			this.grpSuperCards.Controls.Add(this.label11);
			this.grpSuperCards.ForeColor = global::System.Drawing.Color.White;
			this.grpSuperCards.Name = "grpSuperCards";
			this.grpSuperCards.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.txtSuperCard2, "txtSuperCard2");
			this.txtSuperCard2.Name = "txtSuperCard2";
			this.txtSuperCard2.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtSuperCard2_KeyPress);
			this.txtSuperCard2.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtSuperCard2_KeyUp);
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.BackColor = global::System.Drawing.Color.Transparent;
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.txtSuperCard1, "txtSuperCard1");
			this.txtSuperCard1.Name = "txtSuperCard1";
			this.txtSuperCard1.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtSuperCard1_KeyPress);
			this.txtSuperCard1.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtSuperCard1_KeyUp);
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.BackColor = global::System.Drawing.Color.Transparent;
			this.label11.ForeColor = global::System.Drawing.Color.White;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.chkAdjustTime, "chkAdjustTime");
			this.chkAdjustTime.ForeColor = global::System.Drawing.Color.White;
			this.chkAdjustTime.Name = "chkAdjustTime";
			this.chkAdjustTime.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnTryWEB, "btnTryWEB");
			this.btnTryWEB.BackColor = global::System.Drawing.Color.Transparent;
			this.btnTryWEB.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnTryWEB.ForeColor = global::System.Drawing.Color.White;
			this.btnTryWEB.Image = global::WG3000_COMM.Properties.Resources.web;
			this.btnTryWEB.Name = "btnTryWEB";
			this.btnTryWEB.UseVisualStyleBackColor = false;
			this.btnTryWEB.Click += new global::System.EventHandler(this.btnTryWEB_Click);
			componentResourceManager.ApplyResources(this.btnRestoreNameAndPassword, "btnRestoreNameAndPassword");
			this.btnRestoreNameAndPassword.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRestoreNameAndPassword.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRestoreNameAndPassword.ForeColor = global::System.Drawing.Color.White;
			this.btnRestoreNameAndPassword.Name = "btnRestoreNameAndPassword";
			this.btnRestoreNameAndPassword.UseVisualStyleBackColor = false;
			this.btnRestoreNameAndPassword.Click += new global::System.EventHandler(this.btnRestoreNameAndPassword_Click);
			componentResourceManager.ApplyResources(this.chkUpdateSpecialCard, "chkUpdateSpecialCard");
			this.chkUpdateSpecialCard.ForeColor = global::System.Drawing.Color.White;
			this.chkUpdateSpecialCard.Name = "chkUpdateSpecialCard";
			this.chkUpdateSpecialCard.UseVisualStyleBackColor = true;
			this.chkUpdateSpecialCard.CheckedChanged += new global::System.EventHandler(this.chkUpdateSpecialCard_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpSpecialCards, "grpSpecialCards");
			this.grpSpecialCards.Controls.Add(this.txtSpecialCard2);
			this.grpSpecialCards.Controls.Add(this.label13);
			this.grpSpecialCards.Controls.Add(this.txtSpecialCard1);
			this.grpSpecialCards.Controls.Add(this.label14);
			this.grpSpecialCards.ForeColor = global::System.Drawing.Color.White;
			this.grpSpecialCards.Name = "grpSpecialCards";
			this.grpSpecialCards.TabStop = false;
			componentResourceManager.ApplyResources(this.txtSpecialCard2, "txtSpecialCard2");
			this.txtSpecialCard2.Name = "txtSpecialCard2";
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.BackColor = global::System.Drawing.Color.Transparent;
			this.label13.ForeColor = global::System.Drawing.Color.White;
			this.label13.Name = "label13";
			componentResourceManager.ApplyResources(this.txtSpecialCard1, "txtSpecialCard1");
			this.txtSpecialCard1.Name = "txtSpecialCard1";
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.BackColor = global::System.Drawing.Color.Transparent;
			this.label14.ForeColor = global::System.Drawing.Color.White;
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.chkUpdateSpecialCard);
			base.Controls.Add(this.grpSpecialCards);
			base.Controls.Add(this.btnRestoreNameAndPassword);
			base.Controls.Add(this.btnTryWEB);
			base.Controls.Add(this.chkAdjustTime);
			base.Controls.Add(this.grpWEBEnabled);
			base.Controls.Add(this.grpWEBUsers);
			base.Controls.Add(this.chkUpdateSuperCard);
			base.Controls.Add(this.grpSuperCards);
			base.Controls.Add(this.chkUpdateWebSet);
			base.Controls.Add(this.grpWEB);
			base.Controls.Add(this.chkEditIP);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.grpIP);
			base.Controls.Add(this.txtf_ControllerSN);
			base.Controls.Add(this.txtf_MACAddr);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmTCPIPWEBConfigure";
			base.Load += new global::System.EventHandler(this.dfrmTCPIPConfigure_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmTCPIPWEBConfigure_KeyDown);
			this.grpWEBEnabled.ResumeLayout(false);
			this.grpWEBEnabled.PerformLayout();
			this.grpWEBUsers.ResumeLayout(false);
			this.grpWEBUsers.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView3).EndInit();
			this.grpIP.ResumeLayout(false);
			this.grpIP.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).EndInit();
			this.grpWEB.ResumeLayout(false);
			this.grpWEB.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudHttpPort).EndInit();
			this.grpSuperCards.ResumeLayout(false);
			this.grpSuperCards.PerformLayout();
			this.grpSpecialCards.ResumeLayout(false);
			this.grpSpecialCards.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400065E RID: 1630
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400065F RID: 1631
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000660 RID: 1632
		private global::System.Windows.Forms.Button btnDownloadUsers;

		// Token: 0x04000661 RID: 1633
		private global::System.Windows.Forms.Button btnEditUsers;

		// Token: 0x04000662 RID: 1634
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000663 RID: 1635
		private global::System.Windows.Forms.Button btnOtherLanguage;

		// Token: 0x04000664 RID: 1636
		private global::System.Windows.Forms.Button btnRestoreNameAndPassword;

		// Token: 0x04000665 RID: 1637
		private global::System.Windows.Forms.Button btnSelectFile;

		// Token: 0x04000666 RID: 1638
		private global::System.Windows.Forms.Button btnSelectUserFile;

		// Token: 0x04000667 RID: 1639
		private global::System.Windows.Forms.Button btnTryWEB;

		// Token: 0x04000668 RID: 1640
		private global::System.Windows.Forms.Button btnuploadUser;

		// Token: 0x04000669 RID: 1641
		private global::System.Windows.Forms.DataGridView dataGridView3;

		// Token: 0x0400066A RID: 1642
		private global::System.Windows.Forms.GroupBox grpWEBUsers;

		// Token: 0x0400066B RID: 1643
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400066C RID: 1644
		private global::System.Windows.Forms.Label label10;

		// Token: 0x0400066D RID: 1645
		private global::System.Windows.Forms.Label label11;

		// Token: 0x0400066E RID: 1646
		private global::System.Windows.Forms.Label label12;

		// Token: 0x0400066F RID: 1647
		private global::System.Windows.Forms.Label label13;

		// Token: 0x04000670 RID: 1648
		private global::System.Windows.Forms.Label label14;

		// Token: 0x04000671 RID: 1649
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000672 RID: 1650
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000673 RID: 1651
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000674 RID: 1652
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000675 RID: 1653
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000676 RID: 1654
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04000677 RID: 1655
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04000678 RID: 1656
		private global::System.Windows.Forms.TextBox txtf_ControllerSN;

		// Token: 0x04000679 RID: 1657
		private global::System.Windows.Forms.TextBox txtf_gateway;

		// Token: 0x0400067A RID: 1658
		private global::System.Windows.Forms.TextBox txtf_IP;

		// Token: 0x0400067B RID: 1659
		private global::System.Windows.Forms.TextBox txtf_MACAddr;

		// Token: 0x0400067C RID: 1660
		private global::System.Windows.Forms.TextBox txtf_mask;

		// Token: 0x0400067D RID: 1661
		public global::System.Windows.Forms.Button btnOption;

		// Token: 0x0400067E RID: 1662
		public global::System.Windows.Forms.Button btnOptionWEB;

		// Token: 0x0400067F RID: 1663
		public global::System.Windows.Forms.ComboBox cboDateFormat;

		// Token: 0x04000680 RID: 1664
		public global::System.Windows.Forms.ComboBox cboLanguage;

		// Token: 0x04000681 RID: 1665
		public global::System.Windows.Forms.ComboBox cboLanguage2;

		// Token: 0x04000682 RID: 1666
		public global::System.Windows.Forms.CheckBox chkAdjustTime;

		// Token: 0x04000683 RID: 1667
		public global::System.Windows.Forms.CheckBox chkAutoUploadWEBUsers;

		// Token: 0x04000684 RID: 1668
		public global::System.Windows.Forms.CheckBox chkEditIP;

		// Token: 0x04000685 RID: 1669
		public global::System.Windows.Forms.CheckBox chkUpdateSpecialCard;

		// Token: 0x04000686 RID: 1670
		public global::System.Windows.Forms.CheckBox chkUpdateSuperCard;

		// Token: 0x04000687 RID: 1671
		public global::System.Windows.Forms.CheckBox chkUpdateWebSet;

		// Token: 0x04000688 RID: 1672
		public global::System.Windows.Forms.CheckBox chkWebOnlyQuery;

		// Token: 0x04000689 RID: 1673
		public global::System.Windows.Forms.GroupBox grpIP;

		// Token: 0x0400068A RID: 1674
		public global::System.Windows.Forms.GroupBox grpSpecialCards;

		// Token: 0x0400068B RID: 1675
		public global::System.Windows.Forms.GroupBox grpSuperCards;

		// Token: 0x0400068C RID: 1676
		public global::System.Windows.Forms.GroupBox grpWEB;

		// Token: 0x0400068D RID: 1677
		public global::System.Windows.Forms.GroupBox grpWEBEnabled;

		// Token: 0x0400068E RID: 1678
		public global::System.Windows.Forms.Label label6;

		// Token: 0x0400068F RID: 1679
		public global::System.Windows.Forms.Label lblHttpPort;

		// Token: 0x04000690 RID: 1680
		public global::System.Windows.Forms.Label lblPort;

		// Token: 0x04000691 RID: 1681
		public global::System.Windows.Forms.NumericUpDown nudHttpPort;

		// Token: 0x04000692 RID: 1682
		public global::System.Windows.Forms.NumericUpDown nudPort;

		// Token: 0x04000693 RID: 1683
		public global::System.Windows.Forms.RadioButton optWEBDisable;

		// Token: 0x04000694 RID: 1684
		public global::System.Windows.Forms.RadioButton optWEBEnabled;

		// Token: 0x04000695 RID: 1685
		public global::System.Windows.Forms.TextBox txtSelectedFileName;

		// Token: 0x04000696 RID: 1686
		public global::System.Windows.Forms.MaskedTextBox txtSpecialCard1;

		// Token: 0x04000697 RID: 1687
		public global::System.Windows.Forms.MaskedTextBox txtSpecialCard2;

		// Token: 0x04000698 RID: 1688
		public global::System.Windows.Forms.MaskedTextBox txtSuperCard1;

		// Token: 0x04000699 RID: 1689
		public global::System.Windows.Forms.MaskedTextBox txtSuperCard2;

		// Token: 0x0400069A RID: 1690
		public global::System.Windows.Forms.TextBox txtUsersFile;
	}
}
