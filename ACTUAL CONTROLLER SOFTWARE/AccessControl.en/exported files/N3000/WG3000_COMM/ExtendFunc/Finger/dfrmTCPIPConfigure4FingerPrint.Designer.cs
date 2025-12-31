namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002EF RID: 751
	public partial class dfrmTCPIPConfigure4FingerPrint : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x001B39B0 File Offset: 0x001B29B0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x001B39D0 File Offset: 0x001B29D0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmTCPIPConfigure4FingerPrint));
			this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
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
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOption = new global::System.Windows.Forms.Button();
			this.label6 = new global::System.Windows.Forms.Label();
			this.grpPort = new global::System.Windows.Forms.GroupBox();
			this.nudPort = new global::System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpPort.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.BackColor = global::System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtf_ControllerSN, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtf_MACAddr, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtf_IP, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtf_mask, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtf_gateway, 1, 4);
			this.tableLayoutPanel1.ForeColor = global::System.Drawing.Color.White;
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
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
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtf_IP, "txtf_IP");
			this.txtf_IP.Name = "txtf_IP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.txtf_mask, "txtf_mask");
			this.txtf_mask.Name = "txtf_mask";
			componentResourceManager.ApplyResources(this.label5, "label5");
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
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.btnOption.TabStop = false;
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.grpPort, "grpPort");
			this.grpPort.BackColor = global::System.Drawing.Color.Transparent;
			this.grpPort.Controls.Add(this.nudPort);
			this.grpPort.Controls.Add(this.label6);
			this.grpPort.Name = "grpPort";
			this.grpPort.TabStop = false;
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
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.grpPort);
			base.Controls.Add(this.btnOption);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmTCPIPConfigure4FingerPrint";
			base.Load += new global::System.EventHandler(this.dfrmTCPIPConfigure_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpPort.ResumeLayout(false);
			this.grpPort.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002CCD RID: 11469
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002CCE RID: 11470
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002CCF RID: 11471
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002CD0 RID: 11472
		private global::System.Windows.Forms.Button btnOption;

		// Token: 0x04002CD1 RID: 11473
		private global::System.Windows.Forms.GroupBox grpPort;

		// Token: 0x04002CD2 RID: 11474
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002CD3 RID: 11475
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002CD4 RID: 11476
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002CD5 RID: 11477
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002CD6 RID: 11478
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04002CD7 RID: 11479
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04002CD8 RID: 11480
		private global::System.Windows.Forms.NumericUpDown nudPort;

		// Token: 0x04002CD9 RID: 11481
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		// Token: 0x04002CDA RID: 11482
		private global::System.Windows.Forms.TextBox txtf_ControllerSN;

		// Token: 0x04002CDB RID: 11483
		private global::System.Windows.Forms.TextBox txtf_gateway;

		// Token: 0x04002CDC RID: 11484
		private global::System.Windows.Forms.TextBox txtf_IP;

		// Token: 0x04002CDD RID: 11485
		private global::System.Windows.Forms.TextBox txtf_MACAddr;

		// Token: 0x04002CDE RID: 11486
		private global::System.Windows.Forms.TextBox txtf_mask;
	}
}
