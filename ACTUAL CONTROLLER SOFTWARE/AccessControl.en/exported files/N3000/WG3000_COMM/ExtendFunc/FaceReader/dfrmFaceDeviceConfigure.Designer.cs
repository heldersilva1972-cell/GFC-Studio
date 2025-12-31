namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D0 RID: 720
	public partial class dfrmFaceDeviceConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x0016D45C File Offset: 0x0016C45C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0016D47C File Offset: 0x0016C47C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.FaceReader.dfrmFaceDeviceConfigure));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.cboReader = new global::System.Windows.Forms.ComboBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.numericUpDown1 = new global::System.Windows.Forms.NumericUpDown();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.txtDeviceType = new global::System.Windows.Forms.TextBox();
			this.txtCameraName = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
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
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.label26 = new global::System.Windows.Forms.Label();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.grpbController.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudCameraPort).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.grpbController, "grpbController");
			this.grpbController.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbController.Controls.Add(this.cboReader);
			this.grpbController.Controls.Add(this.label7);
			this.grpbController.Controls.Add(this.groupBox1);
			this.grpbController.Controls.Add(this.txtDeviceType);
			this.grpbController.Controls.Add(this.txtCameraName);
			this.grpbController.Controls.Add(this.label6);
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
			this.grpbController.Controls.Add(this.chkActive);
			this.grpbController.Controls.Add(this.label26);
			this.grpbController.Controls.Add(this.txtNote);
			this.grpbController.Controls.Add(this.label1);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			componentResourceManager.ApplyResources(this.cboReader, "cboReader");
			this.cboReader.FormattingEnabled = true;
			this.cboReader.Name = "cboReader";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.numericUpDown1, "numericUpDown1");
			this.numericUpDown1.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = -1;
			numericUpDown.Maximum = new decimal(array);
			this.numericUpDown1.Name = "numericUpDown1";
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new global::System.EventHandler(this.radioButton2_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Checked = true;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabStop = true;
			this.radioButton1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDeviceType, "txtDeviceType");
			this.txtDeviceType.Name = "txtDeviceType";
			componentResourceManager.ApplyResources(this.txtCameraName, "txtCameraName");
			this.txtCameraName.Name = "txtCameraName";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
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
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudCameraPort;
			int[] array2 = new int[4];
			array2[0] = 65534;
			numericUpDown2.Maximum = new decimal(array2);
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudCameraPort;
			int[] array3 = new int[4];
			array3[0] = 20;
			numericUpDown3.Minimum = new decimal(array3);
			this.nudCameraPort.Name = "nudCameraPort";
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudCameraPort;
			int[] array4 = new int[4];
			array4[0] = 8000;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.txtCameraIP, "txtCameraIP");
			this.txtCameraIP.Name = "txtCameraIP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.grpbController);
			base.Name = "dfrmFaceDeviceConfigure";
			base.Load += new global::System.EventHandler(this.dfrmDeviceConfigure_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFaceDeviceConfigure_KeyDown);
			this.grpbController.ResumeLayout(false);
			this.grpbController.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudCameraPort).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002960 RID: 10592
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002961 RID: 10593
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002962 RID: 10594
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002963 RID: 10595
		private global::System.Windows.Forms.ComboBox cboReader;

		// Token: 0x04002964 RID: 10596
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04002965 RID: 10597
		private global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04002966 RID: 10598
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002967 RID: 10599
		private global::System.Windows.Forms.GroupBox grpbController;

		// Token: 0x04002968 RID: 10600
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002969 RID: 10601
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400296A RID: 10602
		private global::System.Windows.Forms.Label label26;

		// Token: 0x0400296B RID: 10603
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400296C RID: 10604
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400296D RID: 10605
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400296E RID: 10606
		private global::System.Windows.Forms.Label label6;

		// Token: 0x0400296F RID: 10607
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04002970 RID: 10608
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04002971 RID: 10609
		private global::System.Windows.Forms.NumericUpDown nudCameraPort;

		// Token: 0x04002972 RID: 10610
		private global::System.Windows.Forms.NumericUpDown numericUpDown1;

		// Token: 0x04002973 RID: 10611
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x04002974 RID: 10612
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x04002975 RID: 10613
		private global::System.Windows.Forms.TextBox txtCameraName;

		// Token: 0x04002976 RID: 10614
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x04002977 RID: 10615
		public global::System.Windows.Forms.TextBox txtCameraIP;

		// Token: 0x04002978 RID: 10616
		public global::System.Windows.Forms.TextBox txtDeviceType;

		// Token: 0x04002979 RID: 10617
		public global::System.Windows.Forms.TextBox txtPassword;

		// Token: 0x0400297A RID: 10618
		public global::System.Windows.Forms.TextBox txtUser;
	}
}
