namespace WG3000_COMM.Basic
{
	// Token: 0x02000024 RID: 36
	public partial class dfrmOptionAdvanced : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00047818 File Offset: 0x00046818
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00047838 File Offset: 0x00046838
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmOptionAdvanced));
			this.folderBrowserDialog1 = new global::System.Windows.Forms.FolderBrowserDialog();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.chkAllowUploadUserName = new global::System.Windows.Forms.CheckBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.nudValidSwipeGap = new global::System.Windows.Forms.NumericUpDown();
			this.chkValidSwipeGap = new global::System.Windows.Forms.CheckBox();
			this.btnBrowse = new global::System.Windows.Forms.Button();
			this.txtPhotoDirectory = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidSwipeGap).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.chkAllowUploadUserName);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.chkAllowUploadUserName, "chkAllowUploadUserName");
			this.chkAllowUploadUserName.BackColor = global::System.Drawing.Color.Transparent;
			this.chkAllowUploadUserName.ForeColor = global::System.Drawing.Color.White;
			this.chkAllowUploadUserName.Name = "chkAllowUploadUserName";
			this.chkAllowUploadUserName.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.nudValidSwipeGap);
			this.groupBox1.Controls.Add(this.chkValidSwipeGap);
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtPhotoDirectory);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.nudValidSwipeGap, "nudValidSwipeGap");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudValidSwipeGap;
			int[] array = new int[4];
			array[0] = 2;
			numericUpDown.Increment = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudValidSwipeGap;
			int[] array2 = new int[4];
			array2[0] = 86400;
			numericUpDown2.Maximum = new decimal(array2);
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudValidSwipeGap;
			int[] array3 = new int[4];
			array3[0] = 6;
			numericUpDown3.Minimum = new decimal(array3);
			this.nudValidSwipeGap.Name = "nudValidSwipeGap";
			this.nudValidSwipeGap.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudValidSwipeGap;
			int[] array4 = new int[4];
			array4[0] = 30;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.chkValidSwipeGap, "chkValidSwipeGap");
			this.chkValidSwipeGap.BackColor = global::System.Drawing.Color.Transparent;
			this.chkValidSwipeGap.ForeColor = global::System.Drawing.Color.White;
			this.chkValidSwipeGap.Name = "chkValidSwipeGap";
			this.chkValidSwipeGap.UseVisualStyleBackColor = false;
			this.chkValidSwipeGap.CheckedChanged += new global::System.EventHandler(this.chkValidSwipeGap_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.BackColor = global::System.Drawing.Color.Transparent;
			this.btnBrowse.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnBrowse.ForeColor = global::System.Drawing.Color.White;
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new global::System.EventHandler(this.btnBrowse_Click);
			componentResourceManager.ApplyResources(this.txtPhotoDirectory, "txtPhotoDirectory");
			this.txtPhotoDirectory.Name = "txtPhotoDirectory";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Name = "dfrmOptionAdvanced";
			base.Load += new global::System.EventHandler(this.dfrmOptionAdvanced_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmOptionAdvanced_KeyDown);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidSwipeGap).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x040004BB RID: 1211
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040004BC RID: 1212
		private global::System.Windows.Forms.Button btnBrowse;

		// Token: 0x040004BD RID: 1213
		private global::System.Windows.Forms.CheckBox chkAllowUploadUserName;

		// Token: 0x040004BE RID: 1214
		private global::System.Windows.Forms.CheckBox chkValidSwipeGap;

		// Token: 0x040004BF RID: 1215
		private global::System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;

		// Token: 0x040004C0 RID: 1216
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040004C1 RID: 1217
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040004C2 RID: 1218
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040004C3 RID: 1219
		private global::System.Windows.Forms.NumericUpDown nudValidSwipeGap;

		// Token: 0x040004C4 RID: 1220
		private global::System.Windows.Forms.TextBox txtPhotoDirectory;

		// Token: 0x040004C5 RID: 1221
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x040004C6 RID: 1222
		internal global::System.Windows.Forms.Button cmdOK;
	}
}
