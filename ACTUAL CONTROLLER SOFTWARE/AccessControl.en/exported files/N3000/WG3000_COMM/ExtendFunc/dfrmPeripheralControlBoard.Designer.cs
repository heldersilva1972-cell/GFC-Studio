namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000245 RID: 581
	public partial class dfrmPeripheralControlBoard : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001213 RID: 4627 RVA: 0x00157BF0 File Offset: 0x00156BF0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00157C10 File Offset: 0x00156C10
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmPeripheralControlBoard));
			this.txtf_ControllerSN = new global::System.Windows.Forms.TextBox();
			this.txtf_ControllerNO = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.grpExt = new global::System.Windows.Forms.GroupBox();
			this.grpSet = new global::System.Windows.Forms.GroupBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label71 = new global::System.Windows.Forms.Label();
			this.btnOption = new global::System.Windows.Forms.Button();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.radioButton25 = new global::System.Windows.Forms.RadioButton();
			this.radioButton13 = new global::System.Windows.Forms.RadioButton();
			this.radioButton12 = new global::System.Windows.Forms.RadioButton();
			this.radioButton11 = new global::System.Windows.Forms.RadioButton();
			this.radioButton10 = new global::System.Windows.Forms.RadioButton();
			this.grpEvent = new global::System.Windows.Forms.GroupBox();
			this.checkBox90 = new global::System.Windows.Forms.CheckBox();
			this.checkBox89 = new global::System.Windows.Forms.CheckBox();
			this.checkBox88 = new global::System.Windows.Forms.CheckBox();
			this.checkBox87 = new global::System.Windows.Forms.CheckBox();
			this.checkBox86 = new global::System.Windows.Forms.CheckBox();
			this.checkBox85 = new global::System.Windows.Forms.CheckBox();
			this.checkBox84 = new global::System.Windows.Forms.CheckBox();
			this.label70 = new global::System.Windows.Forms.Label();
			this.nudDelay = new global::System.Windows.Forms.NumericUpDown();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.grpExt.SuspendLayout();
			this.grpSet.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.grpEvent.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDelay).BeginInit();
			this.tabControl1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
			this.txtf_ControllerSN.BackColor = global::System.Drawing.Color.White;
			this.txtf_ControllerSN.Name = "txtf_ControllerSN";
			this.txtf_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtf_ControllerNO, "txtf_ControllerNO");
			this.txtf_ControllerNO.BackColor = global::System.Drawing.Color.White;
			this.txtf_ControllerNO.Name = "txtf_ControllerNO";
			this.txtf_ControllerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.grpExt, "grpExt");
			this.grpExt.BackColor = global::System.Drawing.Color.Transparent;
			this.grpExt.Controls.Add(this.grpSet);
			this.grpExt.Controls.Add(this.chkActive);
			this.grpExt.ForeColor = global::System.Drawing.Color.White;
			this.grpExt.Name = "grpExt";
			this.grpExt.TabStop = false;
			componentResourceManager.ApplyResources(this.grpSet, "grpSet");
			this.grpSet.Controls.Add(this.label4);
			this.grpSet.Controls.Add(this.label3);
			this.grpSet.Controls.Add(this.label71);
			this.grpSet.Controls.Add(this.btnOption);
			this.grpSet.Controls.Add(this.groupBox5);
			this.grpSet.Controls.Add(this.grpEvent);
			this.grpSet.Controls.Add(this.label70);
			this.grpSet.Controls.Add(this.nudDelay);
			this.grpSet.Name = "grpSet";
			this.grpSet.TabStop = false;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label71, "label71");
			this.label71.Name = "label71";
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.radioButton25);
			this.groupBox5.Controls.Add(this.radioButton13);
			this.groupBox5.Controls.Add(this.radioButton12);
			this.groupBox5.Controls.Add(this.radioButton11);
			this.groupBox5.Controls.Add(this.radioButton10);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButton25, "radioButton25");
			this.radioButton25.Name = "radioButton25";
			this.radioButton25.UseVisualStyleBackColor = true;
			this.radioButton25.CheckedChanged += new global::System.EventHandler(this.radioButton25_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton13, "radioButton13");
			this.radioButton13.Name = "radioButton13";
			this.radioButton13.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton12, "radioButton12");
			this.radioButton12.Name = "radioButton12";
			this.radioButton12.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton11, "radioButton11");
			this.radioButton11.Name = "radioButton11";
			this.radioButton11.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton10, "radioButton10");
			this.radioButton10.Checked = true;
			this.radioButton10.Name = "radioButton10";
			this.radioButton10.TabStop = true;
			this.radioButton10.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.grpEvent, "grpEvent");
			this.grpEvent.Controls.Add(this.checkBox90);
			this.grpEvent.Controls.Add(this.checkBox89);
			this.grpEvent.Controls.Add(this.checkBox88);
			this.grpEvent.Controls.Add(this.checkBox87);
			this.grpEvent.Controls.Add(this.checkBox86);
			this.grpEvent.Controls.Add(this.checkBox85);
			this.grpEvent.Controls.Add(this.checkBox84);
			this.grpEvent.Name = "grpEvent";
			this.grpEvent.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBox90, "checkBox90");
			this.checkBox90.Name = "checkBox90";
			this.checkBox90.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox89, "checkBox89");
			this.checkBox89.Name = "checkBox89";
			this.checkBox89.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox88, "checkBox88");
			this.checkBox88.Name = "checkBox88";
			this.checkBox88.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox87, "checkBox87");
			this.checkBox87.Name = "checkBox87";
			this.checkBox87.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox86, "checkBox86");
			this.checkBox86.Name = "checkBox86";
			this.checkBox86.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox85, "checkBox85");
			this.checkBox85.Name = "checkBox85";
			this.checkBox85.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox84, "checkBox84");
			this.checkBox84.Name = "checkBox84";
			this.checkBox84.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label70, "label70");
			this.label70.Name = "label70";
			componentResourceManager.ApplyResources(this.nudDelay, "nudDelay");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudDelay;
			int[] array = new int[4];
			array[0] = 6553;
			numericUpDown.Maximum = new decimal(array);
			this.nudDelay.Name = "nudDelay";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudDelay;
			int[] array2 = new int[4];
			array2[0] = 3;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.Name = "chkActive";
			this.chkActive.UseVisualStyleBackColor = true;
			this.chkActive.CheckedChanged += new global::System.EventHandler(this.chkActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.SelectedIndexChanged += new global::System.EventHandler(this.tabControl1_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage1.Name = "tabPage1";
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.grpExt);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.txtf_ControllerSN);
			base.Controls.Add(this.txtf_ControllerNO);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.Label1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmPeripheralControlBoard";
			base.Load += new global::System.EventHandler(this.dfrmPeripheralControlBoard_Load);
			this.grpExt.ResumeLayout(false);
			this.grpExt.PerformLayout();
			this.grpSet.ResumeLayout(false);
			this.grpSet.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.grpEvent.ResumeLayout(false);
			this.grpEvent.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDelay).EndInit();
			this.tabControl1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040020AF RID: 8367
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040020B0 RID: 8368
		private global::System.Windows.Forms.Button btnOption;

		// Token: 0x040020B1 RID: 8369
		private global::System.Windows.Forms.CheckBox checkBox84;

		// Token: 0x040020B2 RID: 8370
		private global::System.Windows.Forms.CheckBox checkBox85;

		// Token: 0x040020B3 RID: 8371
		private global::System.Windows.Forms.CheckBox checkBox86;

		// Token: 0x040020B4 RID: 8372
		private global::System.Windows.Forms.CheckBox checkBox87;

		// Token: 0x040020B5 RID: 8373
		private global::System.Windows.Forms.CheckBox checkBox88;

		// Token: 0x040020B6 RID: 8374
		private global::System.Windows.Forms.CheckBox checkBox89;

		// Token: 0x040020B7 RID: 8375
		private global::System.Windows.Forms.CheckBox checkBox90;

		// Token: 0x040020B8 RID: 8376
		private global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x040020B9 RID: 8377
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x040020BA RID: 8378
		private global::System.Windows.Forms.GroupBox grpEvent;

		// Token: 0x040020BB RID: 8379
		private global::System.Windows.Forms.GroupBox grpExt;

		// Token: 0x040020BC RID: 8380
		private global::System.Windows.Forms.GroupBox grpSet;

		// Token: 0x040020BD RID: 8381
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040020BE RID: 8382
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040020BF RID: 8383
		private global::System.Windows.Forms.Label label70;

		// Token: 0x040020C0 RID: 8384
		private global::System.Windows.Forms.Label label71;

		// Token: 0x040020C1 RID: 8385
		private global::System.Windows.Forms.NumericUpDown nudDelay;

		// Token: 0x040020C2 RID: 8386
		private global::System.Windows.Forms.RadioButton radioButton10;

		// Token: 0x040020C3 RID: 8387
		private global::System.Windows.Forms.RadioButton radioButton11;

		// Token: 0x040020C4 RID: 8388
		private global::System.Windows.Forms.RadioButton radioButton12;

		// Token: 0x040020C5 RID: 8389
		private global::System.Windows.Forms.RadioButton radioButton13;

		// Token: 0x040020C6 RID: 8390
		private global::System.Windows.Forms.RadioButton radioButton25;

		// Token: 0x040020C7 RID: 8391
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040020C8 RID: 8392
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040020C9 RID: 8393
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040020CA RID: 8394
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x040020CB RID: 8395
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x040020CC RID: 8396
		internal global::System.Windows.Forms.Button btnExit;

		// Token: 0x040020CD RID: 8397
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040020CE RID: 8398
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040020CF RID: 8399
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x040020D0 RID: 8400
		internal global::System.Windows.Forms.TextBox txtf_ControllerNO;

		// Token: 0x040020D1 RID: 8401
		internal global::System.Windows.Forms.TextBox txtf_ControllerSN;
	}
}
