namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022F RID: 559
	public partial class dfrmCameraSelect : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010C8 RID: 4296 RVA: 0x001323A4 File Offset: 0x001313A4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x001323C4 File Offset: 0x001313C4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.CameraWatch.dfrmCameraSelect));
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.cboCamera = new global::System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			this.cmdCancel.Click += new global::System.EventHandler(this.cmdCancel_Click);
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.cboCamera);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.cboCamera, "cboCamera");
			this.cboCamera.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCamera.FormattingEnabled = true;
			this.cboCamera.Name = "cboCamera";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.groupBox1);
			base.Name = "dfrmCameraSelect";
			base.Load += new global::System.EventHandler(this.dfrmCameraSelect_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04001DD6 RID: 7638
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001DD7 RID: 7639
		private global::System.Windows.Forms.ComboBox cboCamera;

		// Token: 0x04001DD8 RID: 7640
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001DD9 RID: 7641
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04001DDA RID: 7642
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x04001DDB RID: 7643
		internal global::System.Windows.Forms.Button cmdOK;
	}
}
