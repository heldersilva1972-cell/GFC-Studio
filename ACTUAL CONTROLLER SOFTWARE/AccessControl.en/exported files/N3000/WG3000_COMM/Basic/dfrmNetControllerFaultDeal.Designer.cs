namespace WG3000_COMM.Basic
{
	// Token: 0x0200001D RID: 29
	public partial class dfrmNetControllerFaultDeal : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0003AB24 File Offset: 0x00039B24
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.wgudp != null)
			{
				this.wgudp.Close();
			}
			if (disposing && this.m_Controller != null)
			{
				this.m_Controller.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0003AB7C File Offset: 0x00039B7C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmNetControllerFaultDeal));
			this.txtf_ControllerSN = new global::System.Windows.Forms.TextBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.label9 = new global::System.Windows.Forms.Label();
			this.lblDisconnectCable = new global::System.Windows.Forms.Label();
			this.lblDisableWifi = new global::System.Windows.Forms.Label();
			this.label13 = new global::System.Windows.Forms.Label();
			this.btnTryConnect = new global::System.Windows.Forms.Button();
			this.btnTryAutoConfigureIP = new global::System.Windows.Forms.Button();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.txtSuggestion = new global::System.Windows.Forms.TextBox();
			this.txtTestResult = new global::System.Windows.Forms.TextBox();
			this.label14 = new global::System.Windows.Forms.Label();
			this.label11 = new global::System.Windows.Forms.Label();
			this.btnIPCommTestStart = new global::System.Windows.Forms.Button();
			this.btnSwitchSmallNetwork = new global::System.Windows.Forms.Button();
			this.lblIPComplex = new global::System.Windows.Forms.Label();
			this.lblManual = new global::System.Windows.Forms.Label();
			this.btnSearchController = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.richTextBox1 = new global::System.Windows.Forms.RichTextBox();
			this.btnClear = new global::System.Windows.Forms.Button();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.grpbIP = new global::System.Windows.Forms.GroupBox();
			this.nudPort = new global::System.Windows.Forms.NumericUpDown();
			this.txtControllerIP = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.lblIP = new global::System.Windows.Forms.Label();
			this.optIPLarge = new global::System.Windows.Forms.RadioButton();
			this.optIPSmall = new global::System.Windows.Forms.RadioButton();
			this.btnEditController = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.grpbIP.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
			this.txtf_ControllerSN.BackColor = global::System.Drawing.Color.White;
			this.txtf_ControllerSN.Name = "txtf_ControllerSN";
			this.txtf_ControllerSN.TabStop = false;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.lblDisconnectCable);
			this.tabPage1.Controls.Add(this.lblDisableWifi);
			this.tabPage1.Controls.Add(this.label13);
			this.tabPage1.Controls.Add(this.btnTryConnect);
			this.tabPage1.Controls.Add(this.btnTryAutoConfigureIP);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.lblDisconnectCable, "lblDisconnectCable");
			this.lblDisconnectCable.BackColor = global::System.Drawing.Color.Transparent;
			this.lblDisconnectCable.ForeColor = global::System.Drawing.Color.White;
			this.lblDisconnectCable.Name = "lblDisconnectCable";
			componentResourceManager.ApplyResources(this.lblDisableWifi, "lblDisableWifi");
			this.lblDisableWifi.BackColor = global::System.Drawing.Color.Transparent;
			this.lblDisableWifi.ForeColor = global::System.Drawing.Color.White;
			this.lblDisableWifi.Name = "lblDisableWifi";
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.BackColor = global::System.Drawing.Color.Transparent;
			this.label13.ForeColor = global::System.Drawing.Color.White;
			this.label13.Name = "label13";
			componentResourceManager.ApplyResources(this.btnTryConnect, "btnTryConnect");
			this.btnTryConnect.BackColor = global::System.Drawing.Color.Transparent;
			this.btnTryConnect.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnTryConnect.ForeColor = global::System.Drawing.Color.White;
			this.btnTryConnect.Name = "btnTryConnect";
			this.btnTryConnect.UseVisualStyleBackColor = false;
			this.btnTryConnect.Click += new global::System.EventHandler(this.btnTryConnect_Click);
			componentResourceManager.ApplyResources(this.btnTryAutoConfigureIP, "btnTryAutoConfigureIP");
			this.btnTryAutoConfigureIP.BackColor = global::System.Drawing.Color.Transparent;
			this.btnTryAutoConfigureIP.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnTryAutoConfigureIP.ForeColor = global::System.Drawing.Color.White;
			this.btnTryAutoConfigureIP.Name = "btnTryAutoConfigureIP";
			this.btnTryAutoConfigureIP.UseVisualStyleBackColor = false;
			this.btnTryAutoConfigureIP.Click += new global::System.EventHandler(this.btnTryAutoConfigureIP_Click);
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.txtSuggestion);
			this.tabPage2.Controls.Add(this.txtTestResult);
			this.tabPage2.Controls.Add(this.label14);
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.btnIPCommTestStart);
			this.tabPage2.Controls.Add(this.btnSwitchSmallNetwork);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtSuggestion, "txtSuggestion");
			this.txtSuggestion.BackColor = global::System.Drawing.Color.White;
			this.txtSuggestion.Name = "txtSuggestion";
			this.txtSuggestion.TabStop = false;
			componentResourceManager.ApplyResources(this.txtTestResult, "txtTestResult");
			this.txtTestResult.BackColor = global::System.Drawing.Color.White;
			this.txtTestResult.Name = "txtTestResult";
			this.txtTestResult.TabStop = false;
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.btnIPCommTestStart, "btnIPCommTestStart");
			this.btnIPCommTestStart.BackColor = global::System.Drawing.Color.Transparent;
			this.btnIPCommTestStart.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnIPCommTestStart.ForeColor = global::System.Drawing.Color.White;
			this.btnIPCommTestStart.Name = "btnIPCommTestStart";
			this.btnIPCommTestStart.UseVisualStyleBackColor = false;
			this.btnIPCommTestStart.Click += new global::System.EventHandler(this.btnIPCommTestStart_Click);
			componentResourceManager.ApplyResources(this.btnSwitchSmallNetwork, "btnSwitchSmallNetwork");
			this.btnSwitchSmallNetwork.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSwitchSmallNetwork.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSwitchSmallNetwork.ForeColor = global::System.Drawing.Color.White;
			this.btnSwitchSmallNetwork.Name = "btnSwitchSmallNetwork";
			this.btnSwitchSmallNetwork.UseVisualStyleBackColor = false;
			this.btnSwitchSmallNetwork.Click += new global::System.EventHandler(this.btnSwitchSmallNetwork_Click);
			componentResourceManager.ApplyResources(this.lblIPComplex, "lblIPComplex");
			this.lblIPComplex.BackColor = global::System.Drawing.Color.Transparent;
			this.lblIPComplex.ForeColor = global::System.Drawing.Color.White;
			this.lblIPComplex.Name = "lblIPComplex";
			componentResourceManager.ApplyResources(this.lblManual, "lblManual");
			this.lblManual.BackColor = global::System.Drawing.Color.Transparent;
			this.lblManual.ForeColor = global::System.Drawing.Color.White;
			this.lblManual.Name = "lblManual";
			componentResourceManager.ApplyResources(this.btnSearchController, "btnSearchController");
			this.btnSearchController.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSearchController.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSearchController.ForeColor = global::System.Drawing.Color.White;
			this.btnSearchController.Name = "btnSearchController";
			this.btnSearchController.UseVisualStyleBackColor = false;
			this.btnSearchController.Click += new global::System.EventHandler(this.btnSearchController_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.Name = "richTextBox1";
			componentResourceManager.ApplyResources(this.btnClear, "btnClear");
			this.btnClear.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClear.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClear.ForeColor = global::System.Drawing.Color.White;
			this.btnClear.Name = "btnClear";
			this.btnClear.UseVisualStyleBackColor = false;
			this.btnClear.Click += new global::System.EventHandler(this.btnClear_Click);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.grpbIP, "grpbIP");
			this.grpbIP.Controls.Add(this.nudPort);
			this.grpbIP.Controls.Add(this.txtControllerIP);
			this.grpbIP.Controls.Add(this.label6);
			this.grpbIP.Controls.Add(this.lblIP);
			this.grpbIP.ForeColor = global::System.Drawing.Color.White;
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
			this.nudPort.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudPort;
			int[] array3 = new int[4];
			array3[0] = 60000;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.txtControllerIP, "txtControllerIP");
			this.txtControllerIP.Name = "txtControllerIP";
			this.txtControllerIP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.lblIP, "lblIP");
			this.lblIP.Name = "lblIP";
			componentResourceManager.ApplyResources(this.optIPLarge, "optIPLarge");
			this.optIPLarge.ForeColor = global::System.Drawing.Color.White;
			this.optIPLarge.Name = "optIPLarge";
			this.optIPLarge.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optIPSmall, "optIPSmall");
			this.optIPSmall.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.optIPSmall.Checked = true;
			this.optIPSmall.ForeColor = global::System.Drawing.Color.White;
			this.optIPSmall.Name = "optIPSmall";
			this.optIPSmall.TabStop = true;
			this.optIPSmall.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnEditController, "btnEditController");
			this.btnEditController.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditController.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditController.ForeColor = global::System.Drawing.Color.White;
			this.btnEditController.Name = "btnEditController";
			this.btnEditController.UseVisualStyleBackColor = false;
			this.btnEditController.Click += new global::System.EventHandler(this.btnEditController_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lblIPComplex);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.lblManual);
			base.Controls.Add(this.grpbIP);
			base.Controls.Add(this.optIPLarge);
			base.Controls.Add(this.optIPSmall);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.btnClear);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.txtf_ControllerSN);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnEditController);
			base.Controls.Add(this.btnSearchController);
			base.Controls.Add(this.richTextBox1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmNetControllerFaultDeal";
			base.WindowState = global::System.Windows.Forms.FormWindowState.Minimized;
			base.Load += new global::System.EventHandler(this.dfrmNetControllerFaultDeal_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.grpbIP.ResumeLayout(false);
			this.grpbIP.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400039B RID: 923
		private global::WG3000_COMM.Core.wgUdpComm wgudp;

		// Token: 0x040003A0 RID: 928
		private global::WG3000_COMM.DataOper.icController m_Controller = new global::WG3000_COMM.DataOper.icController();

		// Token: 0x040003A6 RID: 934
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040003A7 RID: 935
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040003A8 RID: 936
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040003A9 RID: 937
		private global::System.Windows.Forms.GroupBox grpbIP;

		// Token: 0x040003AA RID: 938
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040003AB RID: 939
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040003AC RID: 940
		private global::System.Windows.Forms.Label label14;

		// Token: 0x040003AD RID: 941
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040003AE RID: 942
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040003AF RID: 943
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040003B0 RID: 944
		private global::System.Windows.Forms.Label lblIP;

		// Token: 0x040003B1 RID: 945
		private global::System.Windows.Forms.NumericUpDown nudPort;

		// Token: 0x040003B2 RID: 946
		private global::System.Windows.Forms.RadioButton optIPSmall;

		// Token: 0x040003B3 RID: 947
		private global::System.Windows.Forms.RichTextBox richTextBox1;

		// Token: 0x040003B4 RID: 948
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040003B5 RID: 949
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040003B6 RID: 950
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040003B7 RID: 951
		private global::System.Windows.Forms.TextBox txtf_ControllerSN;

		// Token: 0x040003B8 RID: 952
		private global::System.Windows.Forms.TextBox txtSuggestion;

		// Token: 0x040003B9 RID: 953
		private global::System.Windows.Forms.TextBox txtTestResult;

		// Token: 0x040003BA RID: 954
		internal global::System.Windows.Forms.Button btnClear;

		// Token: 0x040003BB RID: 955
		internal global::System.Windows.Forms.Button btnEditController;

		// Token: 0x040003BC RID: 956
		internal global::System.Windows.Forms.Button btnIPCommTestStart;

		// Token: 0x040003BD RID: 957
		internal global::System.Windows.Forms.Button btnSearchController;

		// Token: 0x040003BE RID: 958
		internal global::System.Windows.Forms.Button btnSwitchSmallNetwork;

		// Token: 0x040003BF RID: 959
		internal global::System.Windows.Forms.Button btnTryAutoConfigureIP;

		// Token: 0x040003C0 RID: 960
		internal global::System.Windows.Forms.Button btnTryConnect;

		// Token: 0x040003C1 RID: 961
		internal global::System.Windows.Forms.Button button1;

		// Token: 0x040003C2 RID: 962
		internal global::System.Windows.Forms.Label label13;

		// Token: 0x040003C3 RID: 963
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x040003C4 RID: 964
		internal global::System.Windows.Forms.Label lblDisableWifi;

		// Token: 0x040003C5 RID: 965
		internal global::System.Windows.Forms.Label lblDisconnectCable;

		// Token: 0x040003C6 RID: 966
		internal global::System.Windows.Forms.Label lblIPComplex;

		// Token: 0x040003C7 RID: 967
		internal global::System.Windows.Forms.Label lblManual;

		// Token: 0x040003C8 RID: 968
		public global::System.Windows.Forms.RadioButton optIPLarge;

		// Token: 0x040003C9 RID: 969
		public global::System.Windows.Forms.TextBox txtControllerIP;
	}
}
