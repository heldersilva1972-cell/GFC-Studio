namespace WG3000_COMM.ExtendFunc.Cloud2017
{
	// Token: 0x02000231 RID: 561
	public partial class dfrmCloudServerSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010D6 RID: 4310 RVA: 0x0013353E File Offset: 0x0013253E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00133560 File Offset: 0x00132560
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Cloud2017.dfrmCloudServerSet));
			this.btnReset = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtPortShort2 = new global::System.Windows.Forms.TextBox();
			this.txtHostIP2 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.chkDHCP = new global::System.Windows.Forms.CheckBox();
			this.chkActiveCloud = new global::System.Windows.Forms.CheckBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.numericUpDown43 = new global::System.Windows.Forms.NumericUpDown();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtPortShort = new global::System.Windows.Forms.TextBox();
			this.label199 = new global::System.Windows.Forms.Label();
			this.txtPort = new global::System.Windows.Forms.TextBox();
			this.txtHostIP = new global::System.Windows.Forms.TextBox();
			this.label197 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown43).BeginInit();
			base.SuspendLayout();
			this.btnReset.BackColor = global::System.Drawing.Color.Transparent;
			this.btnReset.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			componentResourceManager.ApplyResources(this.btnReset, "btnReset");
			this.btnReset.ForeColor = global::System.Drawing.Color.White;
			this.btnReset.Name = "btnReset";
			this.btnReset.UseVisualStyleBackColor = false;
			this.btnReset.Click += new global::System.EventHandler(this.btnDeactivate_Click);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtPortShort2);
			this.groupBox1.Controls.Add(this.txtHostIP2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtPortShort2, "txtPortShort2");
			this.txtPortShort2.Name = "txtPortShort2";
			componentResourceManager.ApplyResources(this.txtHostIP2, "txtHostIP2");
			this.txtHostIP2.Name = "txtHostIP2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.chkDHCP, "chkDHCP");
			this.chkDHCP.BackColor = global::System.Drawing.Color.Transparent;
			this.chkDHCP.Checked = true;
			this.chkDHCP.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDHCP.ForeColor = global::System.Drawing.Color.White;
			this.chkDHCP.Name = "chkDHCP";
			this.chkDHCP.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.chkActiveCloud, "chkActiveCloud");
			this.chkActiveCloud.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveCloud.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveCloud.Name = "chkActiveCloud";
			this.chkActiveCloud.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.numericUpDown43, "numericUpDown43");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.numericUpDown43;
			int[] array = new int[4];
			array[0] = 253;
			numericUpDown.Maximum = new decimal(array);
			this.numericUpDown43.Name = "numericUpDown43";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.numericUpDown43;
			int[] array2 = new int[4];
			array2[0] = 4;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtPortShort, "txtPortShort");
			this.txtPortShort.Name = "txtPortShort";
			componentResourceManager.ApplyResources(this.label199, "label199");
			this.label199.ForeColor = global::System.Drawing.Color.White;
			this.label199.Name = "label199";
			componentResourceManager.ApplyResources(this.txtPort, "txtPort");
			this.txtPort.Name = "txtPort";
			componentResourceManager.ApplyResources(this.txtHostIP, "txtHostIP");
			this.txtHostIP.Name = "txtHostIP";
			componentResourceManager.ApplyResources(this.label197, "label197");
			this.label197.ForeColor = global::System.Drawing.Color.White;
			this.label197.Name = "label197";
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnReset);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.chkDHCP);
			base.Controls.Add(this.chkActiveCloud);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.numericUpDown43);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.txtPortShort);
			base.Controls.Add(this.label199);
			base.Controls.Add(this.txtPort);
			base.Controls.Add(this.txtHostIP);
			base.Controls.Add(this.label197);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "dfrmCloudServerSet";
			base.Load += new global::System.EventHandler(this.dfrmCloudServerSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCloudServerSet_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown43).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001DE7 RID: 7655
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001DE8 RID: 7656
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001DE9 RID: 7657
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001DEA RID: 7658
		private global::System.Windows.Forms.Button btnReset;

		// Token: 0x04001DEB RID: 7659
		private global::System.Windows.Forms.CheckBox chkActiveCloud;

		// Token: 0x04001DEC RID: 7660
		private global::System.Windows.Forms.CheckBox chkDHCP;

		// Token: 0x04001DED RID: 7661
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001DEE RID: 7662
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001DEF RID: 7663
		private global::System.Windows.Forms.Label label197;

		// Token: 0x04001DF0 RID: 7664
		private global::System.Windows.Forms.Label label199;

		// Token: 0x04001DF1 RID: 7665
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001DF2 RID: 7666
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001DF3 RID: 7667
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04001DF4 RID: 7668
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04001DF5 RID: 7669
		private global::System.Windows.Forms.NumericUpDown numericUpDown43;

		// Token: 0x04001DF6 RID: 7670
		private global::System.Windows.Forms.TextBox txtHostIP;

		// Token: 0x04001DF7 RID: 7671
		private global::System.Windows.Forms.TextBox txtHostIP2;

		// Token: 0x04001DF8 RID: 7672
		private global::System.Windows.Forms.TextBox txtPort;

		// Token: 0x04001DF9 RID: 7673
		private global::System.Windows.Forms.TextBox txtPortShort;

		// Token: 0x04001DFA RID: 7674
		private global::System.Windows.Forms.TextBox txtPortShort2;
	}
}
