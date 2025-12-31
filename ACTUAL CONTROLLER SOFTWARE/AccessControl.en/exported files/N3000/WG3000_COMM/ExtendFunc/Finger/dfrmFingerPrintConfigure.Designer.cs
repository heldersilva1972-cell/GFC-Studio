namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002E9 RID: 745
	public partial class dfrmFingerPrintConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x001A7400 File Offset: 0x001A6400
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x001A7420 File Offset: 0x001A6420
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmFingerPrintConfigure));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.grpbIP = new global::System.Windows.Forms.GroupBox();
			this.nudPort = new global::System.Windows.Forms.NumericUpDown();
			this.txtControllerIP = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.optIPLarge = new global::System.Windows.Forms.RadioButton();
			this.optIPSmall = new global::System.Windows.Forms.RadioButton();
			this.mtxtbControllerSN = new global::System.Windows.Forms.MaskedTextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cboReader = new global::System.Windows.Forms.ComboBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.txtCameraName = new global::System.Windows.Forms.TextBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.label26 = new global::System.Windows.Forms.Label();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.grpbController.SuspendLayout();
			this.grpbIP.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).BeginInit();
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
			this.grpbController.Controls.Add(this.grpbIP);
			this.grpbController.Controls.Add(this.optIPLarge);
			this.grpbController.Controls.Add(this.optIPSmall);
			this.grpbController.Controls.Add(this.mtxtbControllerSN);
			this.grpbController.Controls.Add(this.label2);
			this.grpbController.Controls.Add(this.cboReader);
			this.grpbController.Controls.Add(this.label7);
			this.grpbController.Controls.Add(this.txtCameraName);
			this.grpbController.Controls.Add(this.label8);
			this.grpbController.Controls.Add(this.chkActive);
			this.grpbController.Controls.Add(this.label26);
			this.grpbController.Controls.Add(this.txtNote);
			this.grpbController.Controls.Add(this.label1);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			componentResourceManager.ApplyResources(this.grpbIP, "grpbIP");
			this.grpbIP.Controls.Add(this.nudPort);
			this.grpbIP.Controls.Add(this.txtControllerIP);
			this.grpbIP.Controls.Add(this.label4);
			this.grpbIP.Controls.Add(this.label3);
			this.grpbIP.Name = "grpbIP";
			this.grpbIP.TabStop = false;
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
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudPort;
			int[] array3 = new int[4];
			array3[0] = 60000;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.txtControllerIP, "txtControllerIP");
			this.txtControllerIP.Name = "txtControllerIP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.optIPLarge, "optIPLarge");
			this.optIPLarge.Name = "optIPLarge";
			this.optIPLarge.UseVisualStyleBackColor = true;
			this.optIPLarge.CheckedChanged += new global::System.EventHandler(this.optIPLarge_CheckedChanged);
			componentResourceManager.ApplyResources(this.optIPSmall, "optIPSmall");
			this.optIPSmall.Checked = true;
			this.optIPSmall.Name = "optIPSmall";
			this.optIPSmall.TabStop = true;
			this.optIPSmall.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mtxtbControllerSN, "mtxtbControllerSN");
			this.mtxtbControllerSN.Name = "mtxtbControllerSN";
			this.mtxtbControllerSN.RejectInputOnFirstFailure = true;
			this.mtxtbControllerSN.ResetOnSpace = false;
			this.mtxtbControllerSN.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.mtxtbControllerSN_KeyPress);
			this.mtxtbControllerSN.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.mtxtbControllerSN_KeyUp);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cboReader, "cboReader");
			this.cboReader.FormattingEnabled = true;
			this.cboReader.Name = "cboReader";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.txtCameraName, "txtCameraName");
			this.txtCameraName.Name = "txtCameraName";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
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
			base.Name = "dfrmFingerPrintConfigure";
			base.Load += new global::System.EventHandler(this.dfrmDeviceConfigure_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFaceDeviceConfigure_KeyDown);
			this.grpbController.ResumeLayout(false);
			this.grpbController.PerformLayout();
			this.grpbIP.ResumeLayout(false);
			this.grpbIP.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002C21 RID: 11297
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002C22 RID: 11298
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002C23 RID: 11299
		private global::System.Windows.Forms.ComboBox cboReader;

		// Token: 0x04002C24 RID: 11300
		private global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04002C25 RID: 11301
		private global::System.Windows.Forms.GroupBox grpbController;

		// Token: 0x04002C26 RID: 11302
		private global::System.Windows.Forms.GroupBox grpbIP;

		// Token: 0x04002C27 RID: 11303
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002C28 RID: 11304
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002C29 RID: 11305
		private global::System.Windows.Forms.Label label26;

		// Token: 0x04002C2A RID: 11306
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002C2B RID: 11307
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002C2C RID: 11308
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04002C2D RID: 11309
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04002C2E RID: 11310
		private global::System.Windows.Forms.NumericUpDown nudPort;

		// Token: 0x04002C2F RID: 11311
		private global::System.Windows.Forms.RadioButton optIPSmall;

		// Token: 0x04002C30 RID: 11312
		private global::System.Windows.Forms.TextBox txtCameraName;

		// Token: 0x04002C31 RID: 11313
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x04002C32 RID: 11314
		public global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002C33 RID: 11315
		public global::System.Windows.Forms.MaskedTextBox mtxtbControllerSN;

		// Token: 0x04002C34 RID: 11316
		public global::System.Windows.Forms.RadioButton optIPLarge;

		// Token: 0x04002C35 RID: 11317
		public global::System.Windows.Forms.TextBox txtControllerIP;
	}
}
