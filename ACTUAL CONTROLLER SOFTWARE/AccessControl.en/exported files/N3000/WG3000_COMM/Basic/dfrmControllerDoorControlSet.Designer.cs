namespace WG3000_COMM.Basic
{
	// Token: 0x0200000B RID: 11
	public partial class dfrmControllerDoorControlSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000189BD File Offset: 0x000179BD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000189DC File Offset: 0x000179DC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmControllerDoorControlSet));
			this.btnNormalClose = new global::System.Windows.Forms.Button();
			this.btnNormalOpen = new global::System.Windows.Forms.Button();
			this.btnOnline = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudDoorDelay1D = new global::System.Windows.Forms.NumericUpDown();
			this.label20 = new global::System.Windows.Forms.Label();
			this.btnUpdateDoorDelay = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1D).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnNormalClose, "btnNormalClose");
			this.btnNormalClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnNormalClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNormalClose.ForeColor = global::System.Drawing.Color.White;
			this.btnNormalClose.Name = "btnNormalClose";
			this.btnNormalClose.UseVisualStyleBackColor = false;
			this.btnNormalClose.Click += new global::System.EventHandler(this.btnOnline_Click);
			componentResourceManager.ApplyResources(this.btnNormalOpen, "btnNormalOpen");
			this.btnNormalOpen.BackColor = global::System.Drawing.Color.Transparent;
			this.btnNormalOpen.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNormalOpen.ForeColor = global::System.Drawing.Color.White;
			this.btnNormalOpen.Name = "btnNormalOpen";
			this.btnNormalOpen.UseVisualStyleBackColor = false;
			this.btnNormalOpen.Click += new global::System.EventHandler(this.btnOnline_Click);
			componentResourceManager.ApplyResources(this.btnOnline, "btnOnline");
			this.btnOnline.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOnline.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOnline.ForeColor = global::System.Drawing.Color.White;
			this.btnOnline.Name = "btnOnline";
			this.btnOnline.UseVisualStyleBackColor = false;
			this.btnOnline.Click += new global::System.EventHandler(this.btnOnline_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.Red;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.nudDoorDelay1D, "nudDoorDelay1D");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudDoorDelay1D;
			int[] array = new int[4];
			array[0] = 6000;
			numericUpDown.Maximum = new decimal(array);
			this.nudDoorDelay1D.Name = "nudDoorDelay1D";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudDoorDelay1D;
			int[] array2 = new int[4];
			array2[0] = 3;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.label20, "label20");
			this.label20.ForeColor = global::System.Drawing.Color.White;
			this.label20.Name = "label20";
			componentResourceManager.ApplyResources(this.btnUpdateDoorDelay, "btnUpdateDoorDelay");
			this.btnUpdateDoorDelay.BackColor = global::System.Drawing.Color.Transparent;
			this.btnUpdateDoorDelay.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnUpdateDoorDelay.ForeColor = global::System.Drawing.Color.White;
			this.btnUpdateDoorDelay.Name = "btnUpdateDoorDelay";
			this.btnUpdateDoorDelay.UseVisualStyleBackColor = false;
			this.btnUpdateDoorDelay.Click += new global::System.EventHandler(this.btnUpdateDoorDelay_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.White;
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnUpdateDoorDelay);
			base.Controls.Add(this.nudDoorDelay1D);
			base.Controls.Add(this.label20);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOnline);
			base.Controls.Add(this.btnNormalClose);
			base.Controls.Add(this.btnNormalOpen);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmControllerDoorControlSet";
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1D).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400017F RID: 383
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000180 RID: 384
		private global::System.Windows.Forms.Button btnNormalOpen;

		// Token: 0x04000181 RID: 385
		private global::System.Windows.Forms.Button btnOnline;

		// Token: 0x04000182 RID: 386
		private global::System.Windows.Forms.Button btnUpdateDoorDelay;

		// Token: 0x04000183 RID: 387
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000184 RID: 388
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000185 RID: 389
		private global::System.Windows.Forms.Label label20;

		// Token: 0x04000186 RID: 390
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay1D;

		// Token: 0x04000187 RID: 391
		public global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000188 RID: 392
		public global::System.Windows.Forms.Button btnNormalClose;
	}
}
