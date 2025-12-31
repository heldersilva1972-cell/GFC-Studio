namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022D RID: 557
	public partial class dfrmCameraConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010A1 RID: 4257 RVA: 0x0012E38C File Offset: 0x0012D38C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0012E3AC File Offset: 0x0012D3AC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.CameraWatch.dfrmCameraConfigure));
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.txtCameraName = new global::System.Windows.Forms.TextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			this.radioButton4 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.radioButton0 = new global::System.Windows.Forms.RadioButton();
			this.label6 = new global::System.Windows.Forms.Label();
			this.nudChannel = new global::System.Windows.Forms.NumericUpDown();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.txtPassword = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtUser = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.nudCameraPort = new global::System.Windows.Forms.NumericUpDown();
			this.txtCameraIP = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.cboChannel = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.label26 = new global::System.Windows.Forms.Label();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.grpbController.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudChannel).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudCameraPort).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.grpbController, "grpbController");
			this.grpbController.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbController.Controls.Add(this.txtCameraName);
			this.grpbController.Controls.Add(this.groupBox1);
			this.grpbController.Controls.Add(this.label6);
			this.grpbController.Controls.Add(this.nudChannel);
			this.grpbController.Controls.Add(this.checkBox1);
			this.grpbController.Controls.Add(this.txtPassword);
			this.grpbController.Controls.Add(this.label2);
			this.grpbController.Controls.Add(this.txtUser);
			this.grpbController.Controls.Add(this.label5);
			this.grpbController.Controls.Add(this.label8);
			this.grpbController.Controls.Add(this.nudCameraPort);
			this.grpbController.Controls.Add(this.txtCameraIP);
			this.grpbController.Controls.Add(this.label4);
			this.grpbController.Controls.Add(this.label3);
			this.grpbController.Controls.Add(this.cboChannel);
			this.grpbController.Controls.Add(this.label25);
			this.grpbController.Controls.Add(this.chkActive);
			this.grpbController.Controls.Add(this.label26);
			this.grpbController.Controls.Add(this.txtNote);
			this.grpbController.Controls.Add(this.label1);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			componentResourceManager.ApplyResources(this.txtCameraName, "txtCameraName");
			this.txtCameraName.Name = "txtCameraName";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.radioButton3);
			this.groupBox1.Controls.Add(this.radioButton4);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Controls.Add(this.radioButton0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.groupBox1.Enter += new global::System.EventHandler(this.groupBox1_Enter);
			componentResourceManager.ApplyResources(this.radioButton3, "radioButton3");
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton4, "radioButton4");
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton0, "radioButton0");
			this.radioButton0.Checked = true;
			this.radioButton0.Name = "radioButton0";
			this.radioButton0.TabStop = true;
			this.radioButton0.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.nudChannel, "nudChannel");
			this.nudChannel.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudChannel;
			int[] array = new int[4];
			array[0] = 65534;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudChannel;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudChannel.Name = "nudChannel";
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudChannel;
			int[] array3 = new int[4];
			array3[0] = 1;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			componentResourceManager.ApplyResources(this.txtPassword, "txtPassword");
			this.txtPassword.Name = "txtPassword";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtUser, "txtUser");
			this.txtUser.Name = "txtUser";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.nudCameraPort, "nudCameraPort");
			this.nudCameraPort.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudCameraPort;
			int[] array4 = new int[4];
			array4[0] = 65534;
			numericUpDown4.Maximum = new decimal(array4);
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudCameraPort;
			int[] array5 = new int[4];
			array5[0] = 20;
			numericUpDown5.Minimum = new decimal(array5);
			this.nudCameraPort.Name = "nudCameraPort";
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudCameraPort;
			int[] array6 = new int[4];
			array6[0] = 8000;
			numericUpDown6.Value = new decimal(array6);
			componentResourceManager.ApplyResources(this.txtCameraIP, "txtCameraIP");
			this.txtCameraIP.Name = "txtCameraIP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.cboChannel, "cboChannel");
			this.cboChannel.BackColor = global::System.Drawing.Color.White;
			this.cboChannel.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChannel.FormattingEnabled = true;
			this.cboChannel.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboChannel.Items"),
				componentResourceManager.GetString("cboChannel.Items1"),
				componentResourceManager.GetString("cboChannel.Items2"),
				componentResourceManager.GetString("cboChannel.Items3"),
				componentResourceManager.GetString("cboChannel.Items4"),
				componentResourceManager.GetString("cboChannel.Items5"),
				componentResourceManager.GetString("cboChannel.Items6"),
				componentResourceManager.GetString("cboChannel.Items7"),
				componentResourceManager.GetString("cboChannel.Items8"),
				componentResourceManager.GetString("cboChannel.Items9"),
				componentResourceManager.GetString("cboChannel.Items10"),
				componentResourceManager.GetString("cboChannel.Items11"),
				componentResourceManager.GetString("cboChannel.Items12"),
				componentResourceManager.GetString("cboChannel.Items13"),
				componentResourceManager.GetString("cboChannel.Items14"),
				componentResourceManager.GetString("cboChannel.Items15")
			});
			this.cboChannel.Name = "cboChannel";
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.Checked = true;
			this.chkActive.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkActive.Name = "chkActive";
			this.chkActive.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label26, "label26");
			this.label26.Name = "label26";
			componentResourceManager.ApplyResources(this.txtNote, "txtNote");
			this.txtNote.Name = "txtNote";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.grpbController);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCameraConfigure";
			base.Load += new global::System.EventHandler(this.dfrmCameraConfigure_Load);
			this.grpbController.ResumeLayout(false);
			this.grpbController.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudChannel).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudCameraPort).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001D82 RID: 7554
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001D83 RID: 7555
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001D84 RID: 7556
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001D85 RID: 7557
		private global::System.Windows.Forms.ComboBox cboChannel;

		// Token: 0x04001D86 RID: 7558
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04001D87 RID: 7559
		private global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04001D88 RID: 7560
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001D89 RID: 7561
		private global::System.Windows.Forms.GroupBox grpbController;

		// Token: 0x04001D8A RID: 7562
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001D8B RID: 7563
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001D8C RID: 7564
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04001D8D RID: 7565
		private global::System.Windows.Forms.Label label26;

		// Token: 0x04001D8E RID: 7566
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001D8F RID: 7567
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04001D90 RID: 7568
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04001D91 RID: 7569
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04001D92 RID: 7570
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04001D93 RID: 7571
		private global::System.Windows.Forms.NumericUpDown nudCameraPort;

		// Token: 0x04001D94 RID: 7572
		private global::System.Windows.Forms.NumericUpDown nudChannel;

		// Token: 0x04001D95 RID: 7573
		private global::System.Windows.Forms.RadioButton radioButton0;

		// Token: 0x04001D96 RID: 7574
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x04001D97 RID: 7575
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x04001D98 RID: 7576
		private global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x04001D99 RID: 7577
		private global::System.Windows.Forms.RadioButton radioButton4;

		// Token: 0x04001D9A RID: 7578
		private global::System.Windows.Forms.TextBox txtCameraName;

		// Token: 0x04001D9B RID: 7579
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x04001D9C RID: 7580
		public global::System.Windows.Forms.TextBox txtCameraIP;

		// Token: 0x04001D9D RID: 7581
		public global::System.Windows.Forms.TextBox txtPassword;

		// Token: 0x04001D9E RID: 7582
		public global::System.Windows.Forms.TextBox txtUser;
	}
}
