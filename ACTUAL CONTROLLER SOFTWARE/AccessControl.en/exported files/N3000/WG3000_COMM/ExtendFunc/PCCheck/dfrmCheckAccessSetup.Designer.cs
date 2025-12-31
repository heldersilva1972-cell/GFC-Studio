namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000315 RID: 789
	public partial class dfrmCheckAccessSetup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001840 RID: 6208 RVA: 0x001F94C4 File Offset: 0x001F84C4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x001F94E4 File Offset: 0x001F84E4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PCCheck.dfrmCheckAccessSetup));
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.txtGroupName = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtFileName = new global::System.Windows.Forms.TextBox();
			this.nudMoreCards = new global::System.Windows.Forms.NumericUpDown();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label14 = new global::System.Windows.Forms.Label();
			this.btnBrowse = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.GroupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudMoreCards).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.txtGroupName, "txtGroupName");
			this.txtGroupName.Name = "txtGroupName";
			this.txtGroupName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActive.ForeColor = global::System.Drawing.Color.White;
			this.chkActive.Name = "chkActive";
			this.chkActive.UseVisualStyleBackColor = false;
			this.chkActive.CheckedChanged += new global::System.EventHandler(this.chkActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.txtFileName);
			this.GroupBox1.Controls.Add(this.nudMoreCards);
			this.GroupBox1.Controls.Add(this.label1);
			this.GroupBox1.Controls.Add(this.label14);
			this.GroupBox1.Controls.Add(this.btnBrowse);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.txtFileName, "txtFileName");
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.nudMoreCards, "nudMoreCards");
			this.nudMoreCards.BackColor = global::System.Drawing.Color.White;
			this.nudMoreCards.Name = "nudMoreCards";
			this.nudMoreCards.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.BackColor = global::System.Drawing.Color.Transparent;
			this.btnBrowse.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnBrowse.ForeColor = global::System.Drawing.Color.White;
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new global::System.EventHandler(this.btnBrowse_Click);
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
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtGroupName);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.chkActive);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCheckAccessSetup";
			base.Load += new global::System.EventHandler(this.dfrmCheckAccessSetup_Load);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudMoreCards).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040031B7 RID: 12727
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040031B8 RID: 12728
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040031B9 RID: 12729
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040031BA RID: 12730
		private global::System.Windows.Forms.TextBox txtFileName;

		// Token: 0x040031BB RID: 12731
		private global::System.Windows.Forms.TextBox txtGroupName;

		// Token: 0x040031BC RID: 12732
		internal global::System.Windows.Forms.Button btnBrowse;

		// Token: 0x040031BD RID: 12733
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040031BE RID: 12734
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040031BF RID: 12735
		internal global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x040031C0 RID: 12736
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x040031C1 RID: 12737
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x040031C2 RID: 12738
		internal global::System.Windows.Forms.Label label14;

		// Token: 0x040031C3 RID: 12739
		internal global::System.Windows.Forms.NumericUpDown nudMoreCards;
	}
}
