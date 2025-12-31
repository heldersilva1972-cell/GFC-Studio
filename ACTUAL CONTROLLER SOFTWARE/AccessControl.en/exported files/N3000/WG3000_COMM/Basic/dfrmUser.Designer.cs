namespace WG3000_COMM.Basic
{
	// Token: 0x02000034 RID: 52
	public partial class dfrmUser : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00069C6B File Offset: 0x00068C6B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00069C8C File Offset: 0x00068C8C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUser));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.createQRToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.snapShotToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clearPhotoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnReadSFZ = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnAddNext = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.cmbNormalShift = new global::System.Windows.Forms.ComboBox();
			this.chkDoorEnabled = new global::System.Windows.Forms.CheckBox();
			this.chkAttendance = new global::System.Windows.Forms.CheckBox();
			this.grpbMainInfo = new global::System.Windows.Forms.GroupBox();
			this.btnTakePhoto = new global::System.Windows.Forms.Button();
			this.btnCreateQR = new global::System.Windows.Forms.Button();
			this.lblFingerCnt = new global::System.Windows.Forms.Label();
			this.btnFingerClear = new global::System.Windows.Forms.Button();
			this.btnFingerAdd = new global::System.Windows.Forms.Button();
			this.label8 = new global::System.Windows.Forms.Label();
			this.txtf_CardNO = new global::System.Windows.Forms.MaskedTextBox();
			this.btnSelectPhoto = new global::System.Windows.Forms.Button();
			this.txtf_ConsumerName = new global::System.Windows.Forms.TextBox();
			this.txtf_ConsumerNO = new global::System.Windows.Forms.TextBox();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.grpbAccessControl = new global::System.Windows.Forms.GroupBox();
			this.lblActivateTime = new global::System.Windows.Forms.Label();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.label9 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.txtf_PIN = new global::System.Windows.Forms.MaskedTextBox();
			this.dtpDeactivate = new global::System.Windows.Forms.DateTimePicker();
			this.dtpActivate = new global::System.Windows.Forms.DateTimePicker();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.grpbAttendance = new global::System.Windows.Forms.GroupBox();
			this.optShift = new global::System.Windows.Forms.RadioButton();
			this.optNormal = new global::System.Windows.Forms.RadioButton();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.txtf_Sex = new global::System.Windows.Forms.ComboBox();
			this.txtf_Addr = new global::System.Windows.Forms.TextBox();
			this.Label18 = new global::System.Windows.Forms.Label();
			this.Label16 = new global::System.Windows.Forms.Label();
			this.Label20 = new global::System.Windows.Forms.Label();
			this.Label21 = new global::System.Windows.Forms.Label();
			this.txtf_Postcode = new global::System.Windows.Forms.TextBox();
			this.txtf_JoinDate = new global::System.Windows.Forms.TextBox();
			this.txtf_LeaveDate = new global::System.Windows.Forms.TextBox();
			this.Label13 = new global::System.Windows.Forms.Label();
			this.txtf_Political = new global::System.Windows.Forms.TextBox();
			this.Label14 = new global::System.Windows.Forms.Label();
			this.txtf_CertificateType = new global::System.Windows.Forms.TextBox();
			this.Label15 = new global::System.Windows.Forms.Label();
			this.txtf_CertificateID = new global::System.Windows.Forms.TextBox();
			this.Label17 = new global::System.Windows.Forms.Label();
			this.txtf_Telephone = new global::System.Windows.Forms.TextBox();
			this.Label19 = new global::System.Windows.Forms.Label();
			this.txtf_Mobile = new global::System.Windows.Forms.TextBox();
			this.Label22 = new global::System.Windows.Forms.Label();
			this.txtf_Email = new global::System.Windows.Forms.TextBox();
			this.Label23 = new global::System.Windows.Forms.Label();
			this.Label24 = new global::System.Windows.Forms.Label();
			this.txtf_Culture = new global::System.Windows.Forms.TextBox();
			this.txtf_TechGrade = new global::System.Windows.Forms.TextBox();
			this.Label25 = new global::System.Windows.Forms.Label();
			this.Label26 = new global::System.Windows.Forms.Label();
			this.Label27 = new global::System.Windows.Forms.Label();
			this.txtf_Hometown = new global::System.Windows.Forms.TextBox();
			this.txtf_Title = new global::System.Windows.Forms.TextBox();
			this.txtf_CorporationName = new global::System.Windows.Forms.TextBox();
			this.Label28 = new global::System.Windows.Forms.Label();
			this.txtf_Birthday = new global::System.Windows.Forms.TextBox();
			this.Label30 = new global::System.Windows.Forms.Label();
			this.txtf_Marriage = new global::System.Windows.Forms.TextBox();
			this.Label31 = new global::System.Windows.Forms.Label();
			this.txtf_SocialInsuranceNo = new global::System.Windows.Forms.TextBox();
			this.Label32 = new global::System.Windows.Forms.Label();
			this.Label33 = new global::System.Windows.Forms.Label();
			this.txtf_HomePhone = new global::System.Windows.Forms.TextBox();
			this.Label34 = new global::System.Windows.Forms.Label();
			this.txtf_Nationality = new global::System.Windows.Forms.TextBox();
			this.Label35 = new global::System.Windows.Forms.Label();
			this.txtf_Religion = new global::System.Windows.Forms.TextBox();
			this.txtf_EnglishName = new global::System.Windows.Forms.TextBox();
			this.Label36 = new global::System.Windows.Forms.Label();
			this.txtf_Note = new global::System.Windows.Forms.TextBox();
			this.Label37 = new global::System.Windows.Forms.Label();
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.timer3 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.grpbMainInfo.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			this.grpbAccessControl.SuspendLayout();
			this.grpbAttendance.SuspendLayout();
			this.tabPage2.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.createQRToolStripMenuItem, this.snapShotToolStripMenuItem, this.clearPhotoToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.createQRToolStripMenuItem, "createQRToolStripMenuItem");
			this.createQRToolStripMenuItem.Name = "createQRToolStripMenuItem";
			this.createQRToolStripMenuItem.Click += new global::System.EventHandler(this.btnCreateQR_Click);
			componentResourceManager.ApplyResources(this.snapShotToolStripMenuItem, "snapShotToolStripMenuItem");
			this.snapShotToolStripMenuItem.Name = "snapShotToolStripMenuItem";
			this.snapShotToolStripMenuItem.Click += new global::System.EventHandler(this.snapShotToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.clearPhotoToolStripMenuItem, "clearPhotoToolStripMenuItem");
			this.clearPhotoToolStripMenuItem.Name = "clearPhotoToolStripMenuItem";
			this.clearPhotoToolStripMenuItem.Click += new global::System.EventHandler(this.clearPhotoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnReadSFZ, "btnReadSFZ");
			this.btnReadSFZ.BackColor = global::System.Drawing.Color.Transparent;
			this.btnReadSFZ.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnReadSFZ.ForeColor = global::System.Drawing.Color.White;
			this.btnReadSFZ.Name = "btnReadSFZ";
			this.toolTip1.SetToolTip(this.btnReadSFZ, componentResourceManager.GetString("btnReadSFZ.ToolTip"));
			this.btnReadSFZ.UseVisualStyleBackColor = false;
			this.btnReadSFZ.Click += new global::System.EventHandler(this.btnReadSFZ_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnAddNext, "btnAddNext");
			this.btnAddNext.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddNext.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddNext.ForeColor = global::System.Drawing.Color.White;
			this.btnAddNext.Name = "btnAddNext";
			this.toolTip1.SetToolTip(this.btnAddNext, componentResourceManager.GetString("btnAddNext.ToolTip"));
			this.btnAddNext.UseVisualStyleBackColor = false;
			this.btnAddNext.Click += new global::System.EventHandler(this.btnAddNext_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage1.Controls.Add(this.cmbNormalShift);
			this.tabPage1.Controls.Add(this.chkDoorEnabled);
			this.tabPage1.Controls.Add(this.chkAttendance);
			this.tabPage1.Controls.Add(this.grpbMainInfo);
			this.tabPage1.Controls.Add(this.grpbAccessControl);
			this.tabPage1.Controls.Add(this.grpbAttendance);
			this.tabPage1.ForeColor = global::System.Drawing.Color.White;
			this.tabPage1.Name = "tabPage1";
			this.toolTip1.SetToolTip(this.tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
			componentResourceManager.ApplyResources(this.cmbNormalShift, "cmbNormalShift");
			this.cmbNormalShift.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNormalShift.FormattingEnabled = true;
			this.cmbNormalShift.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cmbNormalShift.Items"),
				componentResourceManager.GetString("cmbNormalShift.Items1"),
				componentResourceManager.GetString("cmbNormalShift.Items2"),
				componentResourceManager.GetString("cmbNormalShift.Items3"),
				componentResourceManager.GetString("cmbNormalShift.Items4"),
				componentResourceManager.GetString("cmbNormalShift.Items5"),
				componentResourceManager.GetString("cmbNormalShift.Items6"),
				componentResourceManager.GetString("cmbNormalShift.Items7")
			});
			this.cmbNormalShift.Name = "cmbNormalShift";
			this.toolTip1.SetToolTip(this.cmbNormalShift, componentResourceManager.GetString("cmbNormalShift.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorEnabled, "chkDoorEnabled");
			this.chkDoorEnabled.Checked = true;
			this.chkDoorEnabled.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorEnabled.Name = "chkDoorEnabled";
			this.toolTip1.SetToolTip(this.chkDoorEnabled, componentResourceManager.GetString("chkDoorEnabled.ToolTip"));
			this.chkDoorEnabled.UseVisualStyleBackColor = true;
			this.chkDoorEnabled.CheckedChanged += new global::System.EventHandler(this.chkDoorEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkAttendance, "chkAttendance");
			this.chkAttendance.Checked = true;
			this.chkAttendance.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttendance.Name = "chkAttendance";
			this.toolTip1.SetToolTip(this.chkAttendance, componentResourceManager.GetString("chkAttendance.ToolTip"));
			this.chkAttendance.UseVisualStyleBackColor = true;
			this.chkAttendance.CheckedChanged += new global::System.EventHandler(this.chkAttendance_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpbMainInfo, "grpbMainInfo");
			this.grpbMainInfo.Controls.Add(this.btnTakePhoto);
			this.grpbMainInfo.Controls.Add(this.btnCreateQR);
			this.grpbMainInfo.Controls.Add(this.lblFingerCnt);
			this.grpbMainInfo.Controls.Add(this.btnFingerClear);
			this.grpbMainInfo.Controls.Add(this.btnFingerAdd);
			this.grpbMainInfo.Controls.Add(this.label8);
			this.grpbMainInfo.Controls.Add(this.txtf_CardNO);
			this.grpbMainInfo.Controls.Add(this.btnSelectPhoto);
			this.grpbMainInfo.Controls.Add(this.txtf_ConsumerName);
			this.grpbMainInfo.Controls.Add(this.txtf_ConsumerNO);
			this.grpbMainInfo.Controls.Add(this.cbof_GroupID);
			this.grpbMainInfo.Controls.Add(this.pictureBox1);
			this.grpbMainInfo.Controls.Add(this.label1);
			this.grpbMainInfo.Controls.Add(this.label2);
			this.grpbMainInfo.Controls.Add(this.label3);
			this.grpbMainInfo.Controls.Add(this.label4);
			this.grpbMainInfo.Name = "grpbMainInfo";
			this.grpbMainInfo.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbMainInfo, componentResourceManager.GetString("grpbMainInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.btnTakePhoto, "btnTakePhoto");
			this.btnTakePhoto.BackColor = global::System.Drawing.Color.Transparent;
			this.btnTakePhoto.BackgroundImage = global::WG3000_COMM.Properties.Resources.camera24;
			this.btnTakePhoto.ForeColor = global::System.Drawing.Color.White;
			this.btnTakePhoto.Name = "btnTakePhoto";
			this.toolTip1.SetToolTip(this.btnTakePhoto, componentResourceManager.GetString("btnTakePhoto.ToolTip"));
			this.btnTakePhoto.UseVisualStyleBackColor = false;
			this.btnTakePhoto.Click += new global::System.EventHandler(this.snapShotToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnCreateQR, "btnCreateQR");
			this.btnCreateQR.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCreateQR.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCreateQR.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateQR.Name = "btnCreateQR";
			this.toolTip1.SetToolTip(this.btnCreateQR, componentResourceManager.GetString("btnCreateQR.ToolTip"));
			this.btnCreateQR.UseVisualStyleBackColor = false;
			this.btnCreateQR.Click += new global::System.EventHandler(this.btnCreateQR_Click);
			componentResourceManager.ApplyResources(this.lblFingerCnt, "lblFingerCnt");
			this.lblFingerCnt.Name = "lblFingerCnt";
			this.toolTip1.SetToolTip(this.lblFingerCnt, componentResourceManager.GetString("lblFingerCnt.ToolTip"));
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
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, componentResourceManager.GetString("label8.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_CardNO, "txtf_CardNO");
			this.txtf_CardNO.Name = "txtf_CardNO";
			this.toolTip1.SetToolTip(this.txtf_CardNO, componentResourceManager.GetString("txtf_CardNO.ToolTip"));
			this.txtf_CardNO.TextChanged += new global::System.EventHandler(this.txtf_CardNO_TextChanged);
			this.txtf_CardNO.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtf_CardNO_KeyPress);
			this.txtf_CardNO.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtf_CardNO_KeyUp);
			componentResourceManager.ApplyResources(this.btnSelectPhoto, "btnSelectPhoto");
			this.btnSelectPhoto.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectPhoto.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectPhoto.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectPhoto.Name = "btnSelectPhoto";
			this.toolTip1.SetToolTip(this.btnSelectPhoto, componentResourceManager.GetString("btnSelectPhoto.ToolTip"));
			this.btnSelectPhoto.UseVisualStyleBackColor = false;
			this.btnSelectPhoto.Click += new global::System.EventHandler(this.btnSelectPhoto_Click);
			componentResourceManager.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
			this.txtf_ConsumerName.Name = "txtf_ConsumerName";
			this.toolTip1.SetToolTip(this.txtf_ConsumerName, componentResourceManager.GetString("txtf_ConsumerName.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_ConsumerNO, "txtf_ConsumerNO");
			this.txtf_ConsumerNO.Name = "txtf_ConsumerNO";
			this.toolTip1.SetToolTip(this.txtf_ConsumerNO, componentResourceManager.GetString("txtf_ConsumerNO.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox1, componentResourceManager.GetString("pictureBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.grpbAccessControl, "grpbAccessControl");
			this.grpbAccessControl.Controls.Add(this.lblActivateTime);
			this.grpbAccessControl.Controls.Add(this.dateBeginHMS1);
			this.grpbAccessControl.Controls.Add(this.label9);
			this.grpbAccessControl.Controls.Add(this.dateEndHMS1);
			this.grpbAccessControl.Controls.Add(this.txtf_PIN);
			this.grpbAccessControl.Controls.Add(this.dtpDeactivate);
			this.grpbAccessControl.Controls.Add(this.dtpActivate);
			this.grpbAccessControl.Controls.Add(this.label5);
			this.grpbAccessControl.Controls.Add(this.label7);
			this.grpbAccessControl.Controls.Add(this.label6);
			this.grpbAccessControl.Name = "grpbAccessControl";
			this.grpbAccessControl.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbAccessControl, componentResourceManager.GetString("grpbAccessControl.ToolTip"));
			this.grpbAccessControl.Enter += new global::System.EventHandler(this.grpbAccessControl_Enter);
			componentResourceManager.ApplyResources(this.lblActivateTime, "lblActivateTime");
			this.lblActivateTime.Name = "lblActivateTime";
			this.toolTip1.SetToolTip(this.lblActivateTime, componentResourceManager.GetString("lblActivateTime.ToolTip"));
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateBeginHMS1, componentResourceManager.GetString("dateBeginHMS1.ToolTip"));
			this.dateBeginHMS1.Value = new global::System.DateTime(2016, 9, 2, 0, 0, 0, 0);
			this.dateBeginHMS1.ValueChanged += new global::System.EventHandler(this.dateBeginHMS1_ValueChanged);
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.toolTip1.SetToolTip(this.label9, componentResourceManager.GetString("label9.ToolTip"));
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.toolTip1.SetToolTip(this.dateEndHMS1, componentResourceManager.GetString("dateEndHMS1.ToolTip"));
			this.dateEndHMS1.Value = new global::System.DateTime(2016, 9, 2, 23, 59, 0, 0);
			this.dateEndHMS1.ValueChanged += new global::System.EventHandler(this.dateEndHMS1_ValueChanged);
			componentResourceManager.ApplyResources(this.txtf_PIN, "txtf_PIN");
			this.txtf_PIN.Name = "txtf_PIN";
			this.txtf_PIN.PasswordChar = '*';
			this.toolTip1.SetToolTip(this.txtf_PIN, componentResourceManager.GetString("txtf_PIN.ToolTip"));
			componentResourceManager.ApplyResources(this.dtpDeactivate, "dtpDeactivate");
			this.dtpDeactivate.Name = "dtpDeactivate";
			this.toolTip1.SetToolTip(this.dtpDeactivate, componentResourceManager.GetString("dtpDeactivate.ToolTip"));
			this.dtpDeactivate.Value = new global::System.DateTime(2099, 12, 31, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dtpActivate, "dtpActivate");
			this.dtpActivate.Name = "dtpActivate";
			this.toolTip1.SetToolTip(this.dtpActivate, componentResourceManager.GetString("dtpActivate.ToolTip"));
			this.dtpActivate.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			this.toolTip1.SetToolTip(this.label7, componentResourceManager.GetString("label7.ToolTip"));
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
			componentResourceManager.ApplyResources(this.grpbAttendance, "grpbAttendance");
			this.grpbAttendance.Controls.Add(this.optShift);
			this.grpbAttendance.Controls.Add(this.optNormal);
			this.grpbAttendance.Name = "grpbAttendance";
			this.grpbAttendance.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbAttendance, componentResourceManager.GetString("grpbAttendance.ToolTip"));
			componentResourceManager.ApplyResources(this.optShift, "optShift");
			this.optShift.Name = "optShift";
			this.toolTip1.SetToolTip(this.optShift, componentResourceManager.GetString("optShift.ToolTip"));
			this.optShift.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNormal, "optNormal");
			this.optNormal.Checked = true;
			this.optNormal.Name = "optNormal";
			this.optNormal.TabStop = true;
			this.toolTip1.SetToolTip(this.optNormal, componentResourceManager.GetString("optNormal.ToolTip"));
			this.optNormal.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage2.Controls.Add(this.txtf_Sex);
			this.tabPage2.Controls.Add(this.txtf_Addr);
			this.tabPage2.Controls.Add(this.Label18);
			this.tabPage2.Controls.Add(this.Label16);
			this.tabPage2.Controls.Add(this.Label20);
			this.tabPage2.Controls.Add(this.Label21);
			this.tabPage2.Controls.Add(this.txtf_Postcode);
			this.tabPage2.Controls.Add(this.txtf_JoinDate);
			this.tabPage2.Controls.Add(this.txtf_LeaveDate);
			this.tabPage2.Controls.Add(this.Label13);
			this.tabPage2.Controls.Add(this.txtf_Political);
			this.tabPage2.Controls.Add(this.Label14);
			this.tabPage2.Controls.Add(this.txtf_CertificateType);
			this.tabPage2.Controls.Add(this.Label15);
			this.tabPage2.Controls.Add(this.txtf_CertificateID);
			this.tabPage2.Controls.Add(this.Label17);
			this.tabPage2.Controls.Add(this.txtf_Telephone);
			this.tabPage2.Controls.Add(this.Label19);
			this.tabPage2.Controls.Add(this.txtf_Mobile);
			this.tabPage2.Controls.Add(this.Label22);
			this.tabPage2.Controls.Add(this.txtf_Email);
			this.tabPage2.Controls.Add(this.Label23);
			this.tabPage2.Controls.Add(this.Label24);
			this.tabPage2.Controls.Add(this.txtf_Culture);
			this.tabPage2.Controls.Add(this.txtf_TechGrade);
			this.tabPage2.Controls.Add(this.Label25);
			this.tabPage2.Controls.Add(this.Label26);
			this.tabPage2.Controls.Add(this.Label27);
			this.tabPage2.Controls.Add(this.txtf_Hometown);
			this.tabPage2.Controls.Add(this.txtf_Title);
			this.tabPage2.Controls.Add(this.txtf_CorporationName);
			this.tabPage2.Controls.Add(this.Label28);
			this.tabPage2.Controls.Add(this.txtf_Birthday);
			this.tabPage2.Controls.Add(this.Label30);
			this.tabPage2.Controls.Add(this.txtf_Marriage);
			this.tabPage2.Controls.Add(this.Label31);
			this.tabPage2.Controls.Add(this.txtf_SocialInsuranceNo);
			this.tabPage2.Controls.Add(this.Label32);
			this.tabPage2.Controls.Add(this.Label33);
			this.tabPage2.Controls.Add(this.txtf_HomePhone);
			this.tabPage2.Controls.Add(this.Label34);
			this.tabPage2.Controls.Add(this.txtf_Nationality);
			this.tabPage2.Controls.Add(this.Label35);
			this.tabPage2.Controls.Add(this.txtf_Religion);
			this.tabPage2.Controls.Add(this.txtf_EnglishName);
			this.tabPage2.Controls.Add(this.Label36);
			this.tabPage2.Controls.Add(this.txtf_Note);
			this.tabPage2.Controls.Add(this.Label37);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.toolTip1.SetToolTip(this.tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Sex, "txtf_Sex");
			this.txtf_Sex.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("txtf_Sex.Items"),
				componentResourceManager.GetString("txtf_Sex.Items1")
			});
			this.txtf_Sex.Name = "txtf_Sex";
			this.toolTip1.SetToolTip(this.txtf_Sex, componentResourceManager.GetString("txtf_Sex.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Addr, "txtf_Addr");
			this.txtf_Addr.Name = "txtf_Addr";
			this.toolTip1.SetToolTip(this.txtf_Addr, componentResourceManager.GetString("txtf_Addr.ToolTip"));
			componentResourceManager.ApplyResources(this.Label18, "Label18");
			this.Label18.Name = "Label18";
			this.toolTip1.SetToolTip(this.Label18, componentResourceManager.GetString("Label18.ToolTip"));
			componentResourceManager.ApplyResources(this.Label16, "Label16");
			this.Label16.Name = "Label16";
			this.toolTip1.SetToolTip(this.Label16, componentResourceManager.GetString("Label16.ToolTip"));
			componentResourceManager.ApplyResources(this.Label20, "Label20");
			this.Label20.Name = "Label20";
			this.toolTip1.SetToolTip(this.Label20, componentResourceManager.GetString("Label20.ToolTip"));
			componentResourceManager.ApplyResources(this.Label21, "Label21");
			this.Label21.Name = "Label21";
			this.toolTip1.SetToolTip(this.Label21, componentResourceManager.GetString("Label21.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Postcode, "txtf_Postcode");
			this.txtf_Postcode.Name = "txtf_Postcode";
			this.toolTip1.SetToolTip(this.txtf_Postcode, componentResourceManager.GetString("txtf_Postcode.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_JoinDate, "txtf_JoinDate");
			this.txtf_JoinDate.Name = "txtf_JoinDate";
			this.toolTip1.SetToolTip(this.txtf_JoinDate, componentResourceManager.GetString("txtf_JoinDate.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_LeaveDate, "txtf_LeaveDate");
			this.txtf_LeaveDate.Name = "txtf_LeaveDate";
			this.toolTip1.SetToolTip(this.txtf_LeaveDate, componentResourceManager.GetString("txtf_LeaveDate.ToolTip"));
			componentResourceManager.ApplyResources(this.Label13, "Label13");
			this.Label13.Name = "Label13";
			this.toolTip1.SetToolTip(this.Label13, componentResourceManager.GetString("Label13.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Political, "txtf_Political");
			this.txtf_Political.Name = "txtf_Political";
			this.toolTip1.SetToolTip(this.txtf_Political, componentResourceManager.GetString("txtf_Political.ToolTip"));
			componentResourceManager.ApplyResources(this.Label14, "Label14");
			this.Label14.Name = "Label14";
			this.toolTip1.SetToolTip(this.Label14, componentResourceManager.GetString("Label14.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_CertificateType, "txtf_CertificateType");
			this.txtf_CertificateType.Name = "txtf_CertificateType";
			this.toolTip1.SetToolTip(this.txtf_CertificateType, componentResourceManager.GetString("txtf_CertificateType.ToolTip"));
			componentResourceManager.ApplyResources(this.Label15, "Label15");
			this.Label15.Name = "Label15";
			this.toolTip1.SetToolTip(this.Label15, componentResourceManager.GetString("Label15.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_CertificateID, "txtf_CertificateID");
			this.txtf_CertificateID.Name = "txtf_CertificateID";
			this.toolTip1.SetToolTip(this.txtf_CertificateID, componentResourceManager.GetString("txtf_CertificateID.ToolTip"));
			componentResourceManager.ApplyResources(this.Label17, "Label17");
			this.Label17.Name = "Label17";
			this.toolTip1.SetToolTip(this.Label17, componentResourceManager.GetString("Label17.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Telephone, "txtf_Telephone");
			this.txtf_Telephone.Name = "txtf_Telephone";
			this.toolTip1.SetToolTip(this.txtf_Telephone, componentResourceManager.GetString("txtf_Telephone.ToolTip"));
			componentResourceManager.ApplyResources(this.Label19, "Label19");
			this.Label19.Name = "Label19";
			this.toolTip1.SetToolTip(this.Label19, componentResourceManager.GetString("Label19.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Mobile, "txtf_Mobile");
			this.txtf_Mobile.Name = "txtf_Mobile";
			this.toolTip1.SetToolTip(this.txtf_Mobile, componentResourceManager.GetString("txtf_Mobile.ToolTip"));
			componentResourceManager.ApplyResources(this.Label22, "Label22");
			this.Label22.Name = "Label22";
			this.toolTip1.SetToolTip(this.Label22, componentResourceManager.GetString("Label22.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Email, "txtf_Email");
			this.txtf_Email.Name = "txtf_Email";
			this.toolTip1.SetToolTip(this.txtf_Email, componentResourceManager.GetString("txtf_Email.ToolTip"));
			componentResourceManager.ApplyResources(this.Label23, "Label23");
			this.Label23.Name = "Label23";
			this.toolTip1.SetToolTip(this.Label23, componentResourceManager.GetString("Label23.ToolTip"));
			componentResourceManager.ApplyResources(this.Label24, "Label24");
			this.Label24.Name = "Label24";
			this.toolTip1.SetToolTip(this.Label24, componentResourceManager.GetString("Label24.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Culture, "txtf_Culture");
			this.txtf_Culture.Name = "txtf_Culture";
			this.toolTip1.SetToolTip(this.txtf_Culture, componentResourceManager.GetString("txtf_Culture.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_TechGrade, "txtf_TechGrade");
			this.txtf_TechGrade.Name = "txtf_TechGrade";
			this.toolTip1.SetToolTip(this.txtf_TechGrade, componentResourceManager.GetString("txtf_TechGrade.ToolTip"));
			componentResourceManager.ApplyResources(this.Label25, "Label25");
			this.Label25.Name = "Label25";
			this.toolTip1.SetToolTip(this.Label25, componentResourceManager.GetString("Label25.ToolTip"));
			componentResourceManager.ApplyResources(this.Label26, "Label26");
			this.Label26.Name = "Label26";
			this.toolTip1.SetToolTip(this.Label26, componentResourceManager.GetString("Label26.ToolTip"));
			componentResourceManager.ApplyResources(this.Label27, "Label27");
			this.Label27.Name = "Label27";
			this.toolTip1.SetToolTip(this.Label27, componentResourceManager.GetString("Label27.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Hometown, "txtf_Hometown");
			this.txtf_Hometown.Name = "txtf_Hometown";
			this.toolTip1.SetToolTip(this.txtf_Hometown, componentResourceManager.GetString("txtf_Hometown.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Title, "txtf_Title");
			this.txtf_Title.Name = "txtf_Title";
			this.toolTip1.SetToolTip(this.txtf_Title, componentResourceManager.GetString("txtf_Title.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_CorporationName, "txtf_CorporationName");
			this.txtf_CorporationName.Name = "txtf_CorporationName";
			this.toolTip1.SetToolTip(this.txtf_CorporationName, componentResourceManager.GetString("txtf_CorporationName.ToolTip"));
			componentResourceManager.ApplyResources(this.Label28, "Label28");
			this.Label28.Name = "Label28";
			this.toolTip1.SetToolTip(this.Label28, componentResourceManager.GetString("Label28.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Birthday, "txtf_Birthday");
			this.txtf_Birthday.Name = "txtf_Birthday";
			this.toolTip1.SetToolTip(this.txtf_Birthday, componentResourceManager.GetString("txtf_Birthday.ToolTip"));
			componentResourceManager.ApplyResources(this.Label30, "Label30");
			this.Label30.Name = "Label30";
			this.toolTip1.SetToolTip(this.Label30, componentResourceManager.GetString("Label30.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Marriage, "txtf_Marriage");
			this.txtf_Marriage.Name = "txtf_Marriage";
			this.toolTip1.SetToolTip(this.txtf_Marriage, componentResourceManager.GetString("txtf_Marriage.ToolTip"));
			componentResourceManager.ApplyResources(this.Label31, "Label31");
			this.Label31.Name = "Label31";
			this.toolTip1.SetToolTip(this.Label31, componentResourceManager.GetString("Label31.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_SocialInsuranceNo, "txtf_SocialInsuranceNo");
			this.txtf_SocialInsuranceNo.Name = "txtf_SocialInsuranceNo";
			this.toolTip1.SetToolTip(this.txtf_SocialInsuranceNo, componentResourceManager.GetString("txtf_SocialInsuranceNo.ToolTip"));
			componentResourceManager.ApplyResources(this.Label32, "Label32");
			this.Label32.Name = "Label32";
			this.toolTip1.SetToolTip(this.Label32, componentResourceManager.GetString("Label32.ToolTip"));
			componentResourceManager.ApplyResources(this.Label33, "Label33");
			this.Label33.Name = "Label33";
			this.toolTip1.SetToolTip(this.Label33, componentResourceManager.GetString("Label33.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_HomePhone, "txtf_HomePhone");
			this.txtf_HomePhone.Name = "txtf_HomePhone";
			this.toolTip1.SetToolTip(this.txtf_HomePhone, componentResourceManager.GetString("txtf_HomePhone.ToolTip"));
			componentResourceManager.ApplyResources(this.Label34, "Label34");
			this.Label34.Name = "Label34";
			this.toolTip1.SetToolTip(this.Label34, componentResourceManager.GetString("Label34.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Nationality, "txtf_Nationality");
			this.txtf_Nationality.Name = "txtf_Nationality";
			this.toolTip1.SetToolTip(this.txtf_Nationality, componentResourceManager.GetString("txtf_Nationality.ToolTip"));
			componentResourceManager.ApplyResources(this.Label35, "Label35");
			this.Label35.Name = "Label35";
			this.toolTip1.SetToolTip(this.Label35, componentResourceManager.GetString("Label35.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Religion, "txtf_Religion");
			this.txtf_Religion.Name = "txtf_Religion";
			this.toolTip1.SetToolTip(this.txtf_Religion, componentResourceManager.GetString("txtf_Religion.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_EnglishName, "txtf_EnglishName");
			this.txtf_EnglishName.Name = "txtf_EnglishName";
			this.toolTip1.SetToolTip(this.txtf_EnglishName, componentResourceManager.GetString("txtf_EnglishName.ToolTip"));
			componentResourceManager.ApplyResources(this.Label36, "Label36");
			this.Label36.Name = "Label36";
			this.toolTip1.SetToolTip(this.Label36, componentResourceManager.GetString("Label36.ToolTip"));
			componentResourceManager.ApplyResources(this.txtf_Note, "txtf_Note");
			this.txtf_Note.Name = "txtf_Note";
			this.toolTip1.SetToolTip(this.txtf_Note, componentResourceManager.GetString("txtf_Note.ToolTip"));
			componentResourceManager.ApplyResources(this.Label37, "Label37");
			this.Label37.Name = "Label37";
			this.toolTip1.SetToolTip(this.Label37, componentResourceManager.GetString("Label37.ToolTip"));
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			this.timer3.Interval = 300;
			this.timer3.Tick += new global::System.EventHandler(this.timer3_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.btnReadSFZ);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnAddNext);
			base.Controls.Add(this.tabControl1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUser";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmUser_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmUser_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.grpbMainInfo.ResumeLayout(false);
			this.grpbMainInfo.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			this.grpbAccessControl.ResumeLayout(false);
			this.grpbAccessControl.PerformLayout();
			this.grpbAttendance.ResumeLayout(false);
			this.grpbAttendance.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040006DB RID: 1755
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040006DC RID: 1756
		private global::System.Windows.Forms.Button btnAddNext;

		// Token: 0x040006DD RID: 1757
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040006DE RID: 1758
		private global::System.Windows.Forms.Button btnCreateQR;

		// Token: 0x040006DF RID: 1759
		private global::System.Windows.Forms.Button btnFingerAdd;

		// Token: 0x040006E0 RID: 1760
		private global::System.Windows.Forms.Button btnFingerClear;

		// Token: 0x040006E1 RID: 1761
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040006E2 RID: 1762
		private global::System.Windows.Forms.Button btnReadSFZ;

		// Token: 0x040006E3 RID: 1763
		private global::System.Windows.Forms.Button btnSelectPhoto;

		// Token: 0x040006E4 RID: 1764
		private global::System.Windows.Forms.Button btnTakePhoto;

		// Token: 0x040006E5 RID: 1765
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x040006E6 RID: 1766
		private global::System.Windows.Forms.CheckBox chkAttendance;

		// Token: 0x040006E7 RID: 1767
		private global::System.Windows.Forms.CheckBox chkDoorEnabled;

		// Token: 0x040006E8 RID: 1768
		private global::System.Windows.Forms.ToolStripMenuItem clearPhotoToolStripMenuItem;

		// Token: 0x040006E9 RID: 1769
		private global::System.Windows.Forms.ComboBox cmbNormalShift;

		// Token: 0x040006EA RID: 1770
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040006EB RID: 1771
		private global::System.Windows.Forms.ToolStripMenuItem createQRToolStripMenuItem;

		// Token: 0x040006EC RID: 1772
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040006ED RID: 1773
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x040006EE RID: 1774
		private global::System.Windows.Forms.DateTimePicker dtpActivate;

		// Token: 0x040006EF RID: 1775
		private global::System.Windows.Forms.DateTimePicker dtpDeactivate;

		// Token: 0x040006F0 RID: 1776
		private global::System.Windows.Forms.GroupBox grpbAccessControl;

		// Token: 0x040006F1 RID: 1777
		private global::System.Windows.Forms.GroupBox grpbAttendance;

		// Token: 0x040006F2 RID: 1778
		private global::System.Windows.Forms.GroupBox grpbMainInfo;

		// Token: 0x040006F3 RID: 1779
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040006F4 RID: 1780
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040006F5 RID: 1781
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040006F6 RID: 1782
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040006F7 RID: 1783
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040006F8 RID: 1784
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040006F9 RID: 1785
		private global::System.Windows.Forms.Label label7;

		// Token: 0x040006FA RID: 1786
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040006FB RID: 1787
		private global::System.Windows.Forms.Label label9;

		// Token: 0x040006FC RID: 1788
		private global::System.Windows.Forms.Label lblActivateTime;

		// Token: 0x040006FD RID: 1789
		private global::System.Windows.Forms.Label lblFingerCnt;

		// Token: 0x040006FE RID: 1790
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040006FF RID: 1791
		private global::System.Windows.Forms.RadioButton optNormal;

		// Token: 0x04000700 RID: 1792
		private global::System.Windows.Forms.RadioButton optShift;

		// Token: 0x04000701 RID: 1793
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000702 RID: 1794
		private global::System.Windows.Forms.ToolStripMenuItem snapShotToolStripMenuItem;

		// Token: 0x04000703 RID: 1795
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04000704 RID: 1796
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000705 RID: 1797
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04000706 RID: 1798
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000707 RID: 1799
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x04000708 RID: 1800
		private global::System.Windows.Forms.Timer timer3;

		// Token: 0x04000709 RID: 1801
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400070A RID: 1802
		private global::System.Windows.Forms.MaskedTextBox txtf_CardNO;

		// Token: 0x0400070B RID: 1803
		private global::System.Windows.Forms.TextBox txtf_ConsumerName;

		// Token: 0x0400070C RID: 1804
		private global::System.Windows.Forms.TextBox txtf_ConsumerNO;

		// Token: 0x0400070D RID: 1805
		private global::System.Windows.Forms.MaskedTextBox txtf_PIN;

		// Token: 0x0400070E RID: 1806
		internal global::System.Windows.Forms.Label Label13;

		// Token: 0x0400070F RID: 1807
		internal global::System.Windows.Forms.Label Label14;

		// Token: 0x04000710 RID: 1808
		internal global::System.Windows.Forms.Label Label15;

		// Token: 0x04000711 RID: 1809
		internal global::System.Windows.Forms.Label Label16;

		// Token: 0x04000712 RID: 1810
		internal global::System.Windows.Forms.Label Label17;

		// Token: 0x04000713 RID: 1811
		internal global::System.Windows.Forms.Label Label18;

		// Token: 0x04000714 RID: 1812
		internal global::System.Windows.Forms.Label Label19;

		// Token: 0x04000715 RID: 1813
		internal global::System.Windows.Forms.Label Label20;

		// Token: 0x04000716 RID: 1814
		internal global::System.Windows.Forms.Label Label21;

		// Token: 0x04000717 RID: 1815
		internal global::System.Windows.Forms.Label Label22;

		// Token: 0x04000718 RID: 1816
		internal global::System.Windows.Forms.Label Label23;

		// Token: 0x04000719 RID: 1817
		internal global::System.Windows.Forms.Label Label24;

		// Token: 0x0400071A RID: 1818
		internal global::System.Windows.Forms.Label Label25;

		// Token: 0x0400071B RID: 1819
		internal global::System.Windows.Forms.Label Label26;

		// Token: 0x0400071C RID: 1820
		internal global::System.Windows.Forms.Label Label27;

		// Token: 0x0400071D RID: 1821
		internal global::System.Windows.Forms.Label Label28;

		// Token: 0x0400071E RID: 1822
		internal global::System.Windows.Forms.Label Label30;

		// Token: 0x0400071F RID: 1823
		internal global::System.Windows.Forms.Label Label31;

		// Token: 0x04000720 RID: 1824
		internal global::System.Windows.Forms.Label Label32;

		// Token: 0x04000721 RID: 1825
		internal global::System.Windows.Forms.Label Label33;

		// Token: 0x04000722 RID: 1826
		internal global::System.Windows.Forms.Label Label34;

		// Token: 0x04000723 RID: 1827
		internal global::System.Windows.Forms.Label Label35;

		// Token: 0x04000724 RID: 1828
		internal global::System.Windows.Forms.Label Label36;

		// Token: 0x04000725 RID: 1829
		internal global::System.Windows.Forms.Label Label37;

		// Token: 0x04000726 RID: 1830
		internal global::System.Windows.Forms.TextBox txtf_Addr;

		// Token: 0x04000727 RID: 1831
		internal global::System.Windows.Forms.TextBox txtf_Birthday;

		// Token: 0x04000728 RID: 1832
		internal global::System.Windows.Forms.TextBox txtf_CertificateID;

		// Token: 0x04000729 RID: 1833
		internal global::System.Windows.Forms.TextBox txtf_CertificateType;

		// Token: 0x0400072A RID: 1834
		internal global::System.Windows.Forms.TextBox txtf_CorporationName;

		// Token: 0x0400072B RID: 1835
		internal global::System.Windows.Forms.TextBox txtf_Culture;

		// Token: 0x0400072C RID: 1836
		internal global::System.Windows.Forms.TextBox txtf_Email;

		// Token: 0x0400072D RID: 1837
		internal global::System.Windows.Forms.TextBox txtf_EnglishName;

		// Token: 0x0400072E RID: 1838
		internal global::System.Windows.Forms.TextBox txtf_HomePhone;

		// Token: 0x0400072F RID: 1839
		internal global::System.Windows.Forms.TextBox txtf_Hometown;

		// Token: 0x04000730 RID: 1840
		internal global::System.Windows.Forms.TextBox txtf_JoinDate;

		// Token: 0x04000731 RID: 1841
		internal global::System.Windows.Forms.TextBox txtf_LeaveDate;

		// Token: 0x04000732 RID: 1842
		internal global::System.Windows.Forms.TextBox txtf_Marriage;

		// Token: 0x04000733 RID: 1843
		internal global::System.Windows.Forms.TextBox txtf_Mobile;

		// Token: 0x04000734 RID: 1844
		internal global::System.Windows.Forms.TextBox txtf_Nationality;

		// Token: 0x04000735 RID: 1845
		internal global::System.Windows.Forms.TextBox txtf_Note;

		// Token: 0x04000736 RID: 1846
		internal global::System.Windows.Forms.TextBox txtf_Political;

		// Token: 0x04000737 RID: 1847
		internal global::System.Windows.Forms.TextBox txtf_Postcode;

		// Token: 0x04000738 RID: 1848
		internal global::System.Windows.Forms.TextBox txtf_Religion;

		// Token: 0x04000739 RID: 1849
		internal global::System.Windows.Forms.ComboBox txtf_Sex;

		// Token: 0x0400073A RID: 1850
		internal global::System.Windows.Forms.TextBox txtf_SocialInsuranceNo;

		// Token: 0x0400073B RID: 1851
		internal global::System.Windows.Forms.TextBox txtf_TechGrade;

		// Token: 0x0400073C RID: 1852
		internal global::System.Windows.Forms.TextBox txtf_Telephone;

		// Token: 0x0400073D RID: 1853
		internal global::System.Windows.Forms.TextBox txtf_Title;
	}
}
