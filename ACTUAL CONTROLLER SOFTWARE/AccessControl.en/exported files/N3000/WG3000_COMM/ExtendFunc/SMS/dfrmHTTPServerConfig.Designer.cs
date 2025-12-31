namespace WG3000_COMM.ExtendFunc.SMS
{
	// Token: 0x02000324 RID: 804
	public partial class dfrmHTTPServerConfig : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001936 RID: 6454 RVA: 0x00210B9B File Offset: 0x0020FB9B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00210BBC File Offset: 0x0020FBBC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.SMS.dfrmHTTPServerConfig));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.importToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.deactivetoolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip2 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.nudCycle = new global::System.Windows.Forms.NumericUpDown();
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtPortShort = new global::System.Windows.Forms.TextBox();
			this.txtUDPServer = new global::System.Windows.Forms.TextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.txtMarkContent = new global::System.Windows.Forms.TextBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label6 = new global::System.Windows.Forms.Label();
			this.btnDefaultParameter = new global::System.Windows.Forms.Button();
			this.txtMarkMobile = new global::System.Windows.Forms.TextBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.txtContentTest = new global::System.Windows.Forms.TextBox();
			this.button132 = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtMobileTest = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label187 = new global::System.Windows.Forms.Label();
			this.txtUserName = new global::System.Windows.Forms.TextBox();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.label200 = new global::System.Windows.Forms.Label();
			this.label188 = new global::System.Windows.Forms.Label();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.chkCheckPhoneValid = new global::System.Windows.Forms.CheckBox();
			this.chkOnlyOnce60Second = new global::System.Windows.Forms.CheckBox();
			this.chkUploadMobile = new global::System.Windows.Forms.CheckBox();
			this.groupBox29 = new global::System.Windows.Forms.GroupBox();
			this.chkSpecialEvent7 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent0 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent1 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent2 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent3 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent4 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent5 = new global::System.Windows.Forms.CheckBox();
			this.chkSpecialEvent6 = new global::System.Windows.Forms.CheckBox();
			this.label202 = new global::System.Windows.Forms.Label();
			this.label201 = new global::System.Windows.Forms.Label();
			this.txtSpecialMobile = new global::System.Windows.Forms.TextBox();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.label3 = new global::System.Windows.Forms.Label();
			this.cboSeparator = new global::System.Windows.Forms.ComboBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.chkContent6 = new global::System.Windows.Forms.CheckBox();
			this.chkContent5 = new global::System.Windows.Forms.CheckBox();
			this.chkContent0 = new global::System.Windows.Forms.CheckBox();
			this.chkContent1 = new global::System.Windows.Forms.CheckBox();
			this.chkContent2 = new global::System.Windows.Forms.CheckBox();
			this.chkContent3 = new global::System.Windows.Forms.CheckBox();
			this.chkContent4 = new global::System.Windows.Forms.CheckBox();
			this.txtContentSignInfo = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label199 = new global::System.Windows.Forms.Label();
			this.txtPort = new global::System.Windows.Forms.TextBox();
			this.label198 = new global::System.Windows.Forms.Label();
			this.txtPostInfo = new global::System.Windows.Forms.TextBox();
			this.label197 = new global::System.Windows.Forms.Label();
			this.txtHostIP = new global::System.Windows.Forms.TextBox();
			this.label196 = new global::System.Windows.Forms.Label();
			this.txtHostDomain = new global::System.Windows.Forms.TextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.chkSwipe = new global::System.Windows.Forms.CheckBox();
			this.chkIncludeSN = new global::System.Windows.Forms.CheckBox();
			this.btnDeactive = new global::System.Windows.Forms.Button();
			this.contextMenuStrip2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCycle).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox29.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.importToolStripMenuItem, "importToolStripMenuItem");
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Click += new global::System.EventHandler(this.importToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Click += new global::System.EventHandler(this.exportToolStripMenuItem_Click);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.deactivetoolStripMenuItem1, "deactivetoolStripMenuItem1");
			this.deactivetoolStripMenuItem1.Name = "deactivetoolStripMenuItem1";
			this.deactivetoolStripMenuItem1.Click += new global::System.EventHandler(this.deactivetoolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
			this.contextMenuStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.importToolStripMenuItem, this.exportToolStripMenuItem, this.deactivetoolStripMenuItem1 });
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.nudCycle);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.txtPortShort);
			this.groupBox4.Controls.Add(this.txtUDPServer);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.nudCycle, "nudCycle");
			this.nudCycle.ForeColor = global::System.Drawing.Color.Black;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudCycle;
			int[] array = new int[4];
			array[0] = 253;
			numericUpDown.Maximum = new decimal(array);
			this.nudCycle.Name = "nudCycle";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudCycle;
			int[] array2 = new int[4];
			array2[0] = 4;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.txtPortShort, "txtPortShort");
			this.txtPortShort.ForeColor = global::System.Drawing.Color.Black;
			this.txtPortShort.Name = "txtPortShort";
			componentResourceManager.ApplyResources(this.txtUDPServer, "txtUDPServer");
			this.txtUDPServer.ForeColor = global::System.Drawing.Color.Black;
			this.txtUDPServer.Name = "txtUDPServer";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = global::System.Drawing.Color.White;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.txtMarkContent, "txtMarkContent");
			this.txtMarkContent.Name = "txtMarkContent";
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.btnDefaultParameter, "btnDefaultParameter");
			this.btnDefaultParameter.Name = "btnDefaultParameter";
			this.btnDefaultParameter.UseVisualStyleBackColor = true;
			this.btnDefaultParameter.Click += new global::System.EventHandler(this.btnDefaultParameter_Click);
			componentResourceManager.ApplyResources(this.txtMarkMobile, "txtMarkMobile");
			this.txtMarkMobile.Name = "txtMarkMobile";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.cbof_ControlSegID);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.label187);
			this.tabPage1.Controls.Add(this.txtUserName);
			this.tabPage1.Controls.Add(this.txtPassword);
			this.tabPage1.Controls.Add(this.label200);
			this.tabPage1.Controls.Add(this.label188);
			this.tabPage1.Name = "tabPage1";
			componentResourceManager.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
			this.cbof_ControlSegID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegID.FormattingEnabled = true;
			this.cbof_ControlSegID.Name = "cbof_ControlSegID";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.txtContentTest);
			this.groupBox2.Controls.Add(this.button132);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.txtMobileTest);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.txtContentTest, "txtContentTest");
			this.txtContentTest.Name = "txtContentTest";
			this.txtContentTest.ReadOnly = true;
			componentResourceManager.ApplyResources(this.button132, "button132");
			this.button132.ForeColor = global::System.Drawing.Color.Black;
			this.button132.Name = "button132";
			this.button132.UseVisualStyleBackColor = true;
			this.button132.Click += new global::System.EventHandler(this.button132_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtMobileTest, "txtMobileTest");
			this.txtMobileTest.Name = "txtMobileTest";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label187, "label187");
			this.label187.ForeColor = global::System.Drawing.Color.White;
			this.label187.Name = "label187";
			componentResourceManager.ApplyResources(this.txtUserName, "txtUserName");
			this.txtUserName.Name = "txtUserName";
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			componentResourceManager.ApplyResources(this.label200, "label200");
			this.label200.ForeColor = global::System.Drawing.Color.White;
			this.label200.Name = "label200";
			componentResourceManager.ApplyResources(this.label188, "label188");
			this.label188.ForeColor = global::System.Drawing.Color.White;
			this.label188.Name = "label188";
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.chkCheckPhoneValid);
			this.tabPage2.Controls.Add(this.chkOnlyOnce60Second);
			this.tabPage2.Controls.Add(this.chkUploadMobile);
			this.tabPage2.Controls.Add(this.groupBox29);
			this.tabPage2.Controls.Add(this.label202);
			this.tabPage2.Controls.Add(this.label201);
			this.tabPage2.Controls.Add(this.txtSpecialMobile);
			this.tabPage2.Name = "tabPage2";
			componentResourceManager.ApplyResources(this.chkCheckPhoneValid, "chkCheckPhoneValid");
			this.chkCheckPhoneValid.Checked = true;
			this.chkCheckPhoneValid.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkCheckPhoneValid.ForeColor = global::System.Drawing.Color.White;
			this.chkCheckPhoneValid.Name = "chkCheckPhoneValid";
			this.chkCheckPhoneValid.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkOnlyOnce60Second, "chkOnlyOnce60Second");
			this.chkOnlyOnce60Second.Checked = true;
			this.chkOnlyOnce60Second.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkOnlyOnce60Second.ForeColor = global::System.Drawing.Color.White;
			this.chkOnlyOnce60Second.Name = "chkOnlyOnce60Second";
			this.chkOnlyOnce60Second.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkUploadMobile, "chkUploadMobile");
			this.chkUploadMobile.Checked = true;
			this.chkUploadMobile.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkUploadMobile.ForeColor = global::System.Drawing.Color.White;
			this.chkUploadMobile.Name = "chkUploadMobile";
			this.chkUploadMobile.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox29, "groupBox29");
			this.groupBox29.Controls.Add(this.chkSpecialEvent7);
			this.groupBox29.Controls.Add(this.chkSpecialEvent0);
			this.groupBox29.Controls.Add(this.chkSpecialEvent1);
			this.groupBox29.Controls.Add(this.chkSpecialEvent2);
			this.groupBox29.Controls.Add(this.chkSpecialEvent3);
			this.groupBox29.Controls.Add(this.chkSpecialEvent4);
			this.groupBox29.Controls.Add(this.chkSpecialEvent5);
			this.groupBox29.Controls.Add(this.chkSpecialEvent6);
			this.groupBox29.ForeColor = global::System.Drawing.Color.White;
			this.groupBox29.Name = "groupBox29";
			this.groupBox29.TabStop = false;
			componentResourceManager.ApplyResources(this.chkSpecialEvent7, "chkSpecialEvent7");
			this.chkSpecialEvent7.Name = "chkSpecialEvent7";
			this.chkSpecialEvent7.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent0, "chkSpecialEvent0");
			this.chkSpecialEvent0.Name = "chkSpecialEvent0";
			this.chkSpecialEvent0.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent1, "chkSpecialEvent1");
			this.chkSpecialEvent1.Name = "chkSpecialEvent1";
			this.chkSpecialEvent1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent2, "chkSpecialEvent2");
			this.chkSpecialEvent2.Name = "chkSpecialEvent2";
			this.chkSpecialEvent2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent3, "chkSpecialEvent3");
			this.chkSpecialEvent3.Name = "chkSpecialEvent3";
			this.chkSpecialEvent3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent4, "chkSpecialEvent4");
			this.chkSpecialEvent4.Name = "chkSpecialEvent4";
			this.chkSpecialEvent4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent5, "chkSpecialEvent5");
			this.chkSpecialEvent5.Name = "chkSpecialEvent5";
			this.chkSpecialEvent5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkSpecialEvent6, "chkSpecialEvent6");
			this.chkSpecialEvent6.Name = "chkSpecialEvent6";
			this.chkSpecialEvent6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label202, "label202");
			this.label202.ForeColor = global::System.Drawing.Color.White;
			this.label202.Name = "label202";
			componentResourceManager.ApplyResources(this.label201, "label201");
			this.label201.ForeColor = global::System.Drawing.Color.White;
			this.label201.Name = "label201";
			componentResourceManager.ApplyResources(this.txtSpecialMobile, "txtSpecialMobile");
			this.txtSpecialMobile.Name = "txtSpecialMobile";
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.cboSeparator);
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Controls.Add(this.txtContentSignInfo);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Name = "tabPage3";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.cboSeparator, "cboSeparator");
			this.cboSeparator.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSeparator.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboSeparator.Items"),
				componentResourceManager.GetString("cboSeparator.Items1"),
				componentResourceManager.GetString("cboSeparator.Items2"),
				componentResourceManager.GetString("cboSeparator.Items3"),
				componentResourceManager.GetString("cboSeparator.Items4"),
				componentResourceManager.GetString("cboSeparator.Items5"),
				componentResourceManager.GetString("cboSeparator.Items6")
			});
			this.cboSeparator.Name = "cboSeparator";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.chkContent6);
			this.groupBox3.Controls.Add(this.chkContent5);
			this.groupBox3.Controls.Add(this.chkContent0);
			this.groupBox3.Controls.Add(this.chkContent1);
			this.groupBox3.Controls.Add(this.chkContent2);
			this.groupBox3.Controls.Add(this.chkContent3);
			this.groupBox3.Controls.Add(this.chkContent4);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.chkContent6, "chkContent6");
			this.chkContent6.Name = "chkContent6";
			this.chkContent6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent5, "chkContent5");
			this.chkContent5.Name = "chkContent5";
			this.chkContent5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent0, "chkContent0");
			this.chkContent0.Checked = true;
			this.chkContent0.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkContent0.Name = "chkContent0";
			this.chkContent0.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent1, "chkContent1");
			this.chkContent1.Name = "chkContent1";
			this.chkContent1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent2, "chkContent2");
			this.chkContent2.Checked = true;
			this.chkContent2.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkContent2.Name = "chkContent2";
			this.chkContent2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent3, "chkContent3");
			this.chkContent3.Checked = true;
			this.chkContent3.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkContent3.Name = "chkContent3";
			this.chkContent3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkContent4, "chkContent4");
			this.chkContent4.Checked = true;
			this.chkContent4.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkContent4.Name = "chkContent4";
			this.chkContent4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtContentSignInfo, "txtContentSignInfo");
			this.txtContentSignInfo.Name = "txtContentSignInfo";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label199, "label199");
			this.label199.ForeColor = global::System.Drawing.Color.White;
			this.label199.Name = "label199";
			componentResourceManager.ApplyResources(this.txtPort, "txtPort");
			this.txtPort.Name = "txtPort";
			componentResourceManager.ApplyResources(this.label198, "label198");
			this.label198.ForeColor = global::System.Drawing.Color.White;
			this.label198.Name = "label198";
			componentResourceManager.ApplyResources(this.txtPostInfo, "txtPostInfo");
			this.txtPostInfo.Name = "txtPostInfo";
			componentResourceManager.ApplyResources(this.label197, "label197");
			this.label197.ForeColor = global::System.Drawing.Color.White;
			this.label197.Name = "label197";
			componentResourceManager.ApplyResources(this.txtHostIP, "txtHostIP");
			this.txtHostIP.Name = "txtHostIP";
			componentResourceManager.ApplyResources(this.label196, "label196");
			this.label196.ForeColor = global::System.Drawing.Color.White;
			this.label196.Name = "label196";
			componentResourceManager.ApplyResources(this.txtHostDomain, "txtHostDomain");
			this.txtHostDomain.Name = "txtHostDomain";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.chkSwipe);
			this.groupBox1.Controls.Add(this.chkIncludeSN);
			this.groupBox1.Controls.Add(this.txtHostDomain);
			this.groupBox1.Controls.Add(this.label199);
			this.groupBox1.Controls.Add(this.label196);
			this.groupBox1.Controls.Add(this.txtPort);
			this.groupBox1.Controls.Add(this.txtHostIP);
			this.groupBox1.Controls.Add(this.label197);
			this.groupBox1.Controls.Add(this.txtPostInfo);
			this.groupBox1.Controls.Add(this.label198);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.chkSwipe, "chkSwipe");
			this.chkSwipe.Name = "chkSwipe";
			this.chkSwipe.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkIncludeSN, "chkIncludeSN");
			this.chkIncludeSN.Name = "chkIncludeSN";
			this.chkIncludeSN.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnDeactive, "btnDeactive");
			this.btnDeactive.Name = "btnDeactive";
			this.btnDeactive.UseVisualStyleBackColor = true;
			this.btnDeactive.Click += new global::System.EventHandler(this.deactivetoolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip2;
			base.Controls.Add(this.btnDeactive);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.txtMarkContent);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.btnDefaultParameter);
			base.Controls.Add(this.txtMarkMobile);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.groupBox1);
			base.Name = "dfrmHTTPServerConfig";
			base.Load += new global::System.EventHandler(this.dfrmSMSConfig_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmSMSConfig_KeyDown);
			this.contextMenuStrip2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCycle).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox29.ResumeLayout(false);
			this.groupBox29.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003394 RID: 13204
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003395 RID: 13205
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003396 RID: 13206
		private global::System.Windows.Forms.Button btnDeactive;

		// Token: 0x04003397 RID: 13207
		private global::System.Windows.Forms.Button btnDefaultParameter;

		// Token: 0x04003398 RID: 13208
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003399 RID: 13209
		private global::System.Windows.Forms.Button button132;

		// Token: 0x0400339A RID: 13210
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x0400339B RID: 13211
		private global::System.Windows.Forms.CheckBox chkCheckPhoneValid;

		// Token: 0x0400339C RID: 13212
		private global::System.Windows.Forms.CheckBox chkContent0;

		// Token: 0x0400339D RID: 13213
		private global::System.Windows.Forms.CheckBox chkContent1;

		// Token: 0x0400339E RID: 13214
		private global::System.Windows.Forms.CheckBox chkContent2;

		// Token: 0x0400339F RID: 13215
		private global::System.Windows.Forms.CheckBox chkContent3;

		// Token: 0x040033A0 RID: 13216
		private global::System.Windows.Forms.CheckBox chkContent4;

		// Token: 0x040033A1 RID: 13217
		private global::System.Windows.Forms.CheckBox chkContent5;

		// Token: 0x040033A2 RID: 13218
		private global::System.Windows.Forms.CheckBox chkContent6;

		// Token: 0x040033A3 RID: 13219
		private global::System.Windows.Forms.CheckBox chkIncludeSN;

		// Token: 0x040033A4 RID: 13220
		private global::System.Windows.Forms.CheckBox chkOnlyOnce60Second;

		// Token: 0x040033A5 RID: 13221
		private global::System.Windows.Forms.CheckBox chkSpecialEvent0;

		// Token: 0x040033A6 RID: 13222
		private global::System.Windows.Forms.CheckBox chkSpecialEvent1;

		// Token: 0x040033A7 RID: 13223
		private global::System.Windows.Forms.CheckBox chkSpecialEvent2;

		// Token: 0x040033A8 RID: 13224
		private global::System.Windows.Forms.CheckBox chkSpecialEvent3;

		// Token: 0x040033A9 RID: 13225
		private global::System.Windows.Forms.CheckBox chkSpecialEvent4;

		// Token: 0x040033AA RID: 13226
		private global::System.Windows.Forms.CheckBox chkSpecialEvent5;

		// Token: 0x040033AB RID: 13227
		private global::System.Windows.Forms.CheckBox chkSpecialEvent6;

		// Token: 0x040033AC RID: 13228
		private global::System.Windows.Forms.CheckBox chkSpecialEvent7;

		// Token: 0x040033AD RID: 13229
		private global::System.Windows.Forms.CheckBox chkSwipe;

		// Token: 0x040033AE RID: 13230
		private global::System.Windows.Forms.CheckBox chkUploadMobile;

		// Token: 0x040033AF RID: 13231
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040033B0 RID: 13232
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip2;

		// Token: 0x040033B1 RID: 13233
		private global::System.Windows.Forms.ToolStripMenuItem deactivetoolStripMenuItem1;

		// Token: 0x040033B2 RID: 13234
		private global::System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;

		// Token: 0x040033B3 RID: 13235
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040033B4 RID: 13236
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040033B5 RID: 13237
		private global::System.Windows.Forms.GroupBox groupBox29;

		// Token: 0x040033B6 RID: 13238
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x040033B7 RID: 13239
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x040033B8 RID: 13240
		private global::System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;

		// Token: 0x040033B9 RID: 13241
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040033BA RID: 13242
		private global::System.Windows.Forms.Label label10;

		// Token: 0x040033BB RID: 13243
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040033BC RID: 13244
		private global::System.Windows.Forms.Label label187;

		// Token: 0x040033BD RID: 13245
		private global::System.Windows.Forms.Label label188;

		// Token: 0x040033BE RID: 13246
		private global::System.Windows.Forms.Label label196;

		// Token: 0x040033BF RID: 13247
		private global::System.Windows.Forms.Label label197;

		// Token: 0x040033C0 RID: 13248
		private global::System.Windows.Forms.Label label198;

		// Token: 0x040033C1 RID: 13249
		private global::System.Windows.Forms.Label label199;

		// Token: 0x040033C2 RID: 13250
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040033C3 RID: 13251
		private global::System.Windows.Forms.Label label200;

		// Token: 0x040033C4 RID: 13252
		private global::System.Windows.Forms.Label label201;

		// Token: 0x040033C5 RID: 13253
		private global::System.Windows.Forms.Label label202;

		// Token: 0x040033C6 RID: 13254
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040033C7 RID: 13255
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040033C8 RID: 13256
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040033C9 RID: 13257
		private global::System.Windows.Forms.Label label7;

		// Token: 0x040033CA RID: 13258
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040033CB RID: 13259
		private global::System.Windows.Forms.Label label9;

		// Token: 0x040033CC RID: 13260
		private global::System.Windows.Forms.NumericUpDown nudCycle;

		// Token: 0x040033CD RID: 13261
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040033CE RID: 13262
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040033CF RID: 13263
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040033D0 RID: 13264
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040033D1 RID: 13265
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x040033D2 RID: 13266
		private global::System.Windows.Forms.TextBox txtContentSignInfo;

		// Token: 0x040033D3 RID: 13267
		private global::System.Windows.Forms.TextBox txtContentTest;

		// Token: 0x040033D4 RID: 13268
		private global::System.Windows.Forms.TextBox txtHostDomain;

		// Token: 0x040033D5 RID: 13269
		private global::System.Windows.Forms.TextBox txtHostIP;

		// Token: 0x040033D6 RID: 13270
		private global::System.Windows.Forms.TextBox txtMarkContent;

		// Token: 0x040033D7 RID: 13271
		private global::System.Windows.Forms.TextBox txtMarkMobile;

		// Token: 0x040033D8 RID: 13272
		private global::System.Windows.Forms.TextBox txtMobileTest;

		// Token: 0x040033D9 RID: 13273
		private global::System.Windows.Forms.TextBox txtPassword;

		// Token: 0x040033DA RID: 13274
		private global::System.Windows.Forms.TextBox txtPort;

		// Token: 0x040033DB RID: 13275
		private global::System.Windows.Forms.TextBox txtPortShort;

		// Token: 0x040033DC RID: 13276
		private global::System.Windows.Forms.TextBox txtPostInfo;

		// Token: 0x040033DD RID: 13277
		private global::System.Windows.Forms.TextBox txtSpecialMobile;

		// Token: 0x040033DE RID: 13278
		private global::System.Windows.Forms.TextBox txtUDPServer;

		// Token: 0x040033DF RID: 13279
		private global::System.Windows.Forms.TextBox txtUserName;

		// Token: 0x040033E0 RID: 13280
		internal global::System.Windows.Forms.ComboBox cboSeparator;

		// Token: 0x040033E1 RID: 13281
		internal global::System.Windows.Forms.Label label3;
	}
}
