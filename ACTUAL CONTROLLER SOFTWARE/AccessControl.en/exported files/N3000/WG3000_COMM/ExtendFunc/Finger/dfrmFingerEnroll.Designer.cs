namespace WG3000_COMM.ExtendFunc.Finger
{
	// Token: 0x020002DF RID: 735
	public partial class dfrmFingerEnroll : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060014DD RID: 5341 RVA: 0x0019C160 File Offset: 0x0019B160
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmWait1 != null)
			{
				this.dfrmWait1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0019C198 File Offset: 0x0019B198
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Finger.dfrmFingerEnroll));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.stopToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.button1 = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnNext = new global::System.Windows.Forms.Button();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.btnErrConnect = new global::System.Windows.Forms.Button();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.btnInstallUSBDriver = new global::System.Windows.Forms.Button();
			this.btnDownloadAll = new global::System.Windows.Forms.Button();
			this.btnClearAll = new global::System.Windows.Forms.Button();
			this.btnUploadAllUsers = new global::System.Windows.Forms.Button();
			this.cboUART = new global::System.Windows.Forms.ComboBox();
			this.cboDoors = new global::System.Windows.Forms.ComboBox();
			this.optController = new global::System.Windows.Forms.RadioButton();
			this.optUSBReader = new global::System.Windows.Forms.RadioButton();
			this.contextMenuStrip1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.stopToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.stopToolStripMenuItem, "stopToolStripMenuItem");
			this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
			this.stopToolStripMenuItem.Click += new global::System.EventHandler(this.stopToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.toolTip1.SetToolTip(this.button1, componentResourceManager.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnCancel2_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnNext, "btnNext");
			this.btnNext.BackColor = global::System.Drawing.Color.Transparent;
			this.btnNext.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNext.ForeColor = global::System.Drawing.Color.White;
			this.btnNext.Name = "btnNext";
			this.toolTip1.SetToolTip(this.btnNext, componentResourceManager.GetString("btnNext.ToolTip"));
			this.btnNext.UseVisualStyleBackColor = false;
			this.btnNext.Click += new global::System.EventHandler(this.btnNext_Click);
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox5.Controls.Add(this.btnErrConnect);
			this.groupBox5.Controls.Add(this.label7);
			this.groupBox5.Controls.Add(this.label8);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox5, componentResourceManager.GetString("groupBox5.ToolTip"));
			this.groupBox5.Enter += new global::System.EventHandler(this.groupBox5_Enter);
			componentResourceManager.ApplyResources(this.btnErrConnect, "btnErrConnect");
			this.btnErrConnect.BackgroundImage = global::WG3000_COMM.Properties.Resources.eventlogError;
			this.btnErrConnect.FlatAppearance.BorderSize = 0;
			this.btnErrConnect.Name = "btnErrConnect";
			this.toolTip1.SetToolTip(this.btnErrConnect, componentResourceManager.GetString("btnErrConnect.ToolTip"));
			this.btnErrConnect.Click += new global::System.EventHandler(this.btnErrConnect_Click);
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			this.toolTip1.SetToolTip(this.label7, componentResourceManager.GetString("label7.ToolTip"));
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, componentResourceManager.GetString("label8.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.btnInstallUSBDriver);
			this.groupBox1.Controls.Add(this.btnDownloadAll);
			this.groupBox1.Controls.Add(this.btnClearAll);
			this.groupBox1.Controls.Add(this.btnUploadAllUsers);
			this.groupBox1.Controls.Add(this.cboUART);
			this.groupBox1.Controls.Add(this.cboDoors);
			this.groupBox1.Controls.Add(this.optController);
			this.groupBox1.Controls.Add(this.optUSBReader);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnInstallUSBDriver, "btnInstallUSBDriver");
			this.btnInstallUSBDriver.BackColor = global::System.Drawing.Color.Transparent;
			this.btnInstallUSBDriver.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnInstallUSBDriver.ForeColor = global::System.Drawing.Color.White;
			this.btnInstallUSBDriver.Name = "btnInstallUSBDriver";
			this.toolTip1.SetToolTip(this.btnInstallUSBDriver, componentResourceManager.GetString("btnInstallUSBDriver.ToolTip"));
			this.btnInstallUSBDriver.UseVisualStyleBackColor = false;
			this.btnInstallUSBDriver.Click += new global::System.EventHandler(this.btnInstallUSBDriver_Click);
			componentResourceManager.ApplyResources(this.btnDownloadAll, "btnDownloadAll");
			this.btnDownloadAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDownloadAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDownloadAll.ForeColor = global::System.Drawing.Color.White;
			this.btnDownloadAll.Name = "btnDownloadAll";
			this.toolTip1.SetToolTip(this.btnDownloadAll, componentResourceManager.GetString("btnDownloadAll.ToolTip"));
			this.btnDownloadAll.UseVisualStyleBackColor = false;
			this.btnDownloadAll.Click += new global::System.EventHandler(this.btnDownloadAll_Click);
			componentResourceManager.ApplyResources(this.btnClearAll, "btnClearAll");
			this.btnClearAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClearAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClearAll.ForeColor = global::System.Drawing.Color.White;
			this.btnClearAll.Name = "btnClearAll";
			this.toolTip1.SetToolTip(this.btnClearAll, componentResourceManager.GetString("btnClearAll.ToolTip"));
			this.btnClearAll.UseVisualStyleBackColor = false;
			this.btnClearAll.Click += new global::System.EventHandler(this.btnClearAll_Click);
			componentResourceManager.ApplyResources(this.btnUploadAllUsers, "btnUploadAllUsers");
			this.btnUploadAllUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnUploadAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnUploadAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnUploadAllUsers.Name = "btnUploadAllUsers";
			this.toolTip1.SetToolTip(this.btnUploadAllUsers, componentResourceManager.GetString("btnUploadAllUsers.ToolTip"));
			this.btnUploadAllUsers.UseVisualStyleBackColor = false;
			this.btnUploadAllUsers.Click += new global::System.EventHandler(this.btnUploadAllUsers_Click);
			componentResourceManager.ApplyResources(this.cboUART, "cboUART");
			this.cboUART.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUART.FormattingEnabled = true;
			this.cboUART.Name = "cboUART";
			this.toolTip1.SetToolTip(this.cboUART, componentResourceManager.GetString("cboUART.ToolTip"));
			componentResourceManager.ApplyResources(this.cboDoors, "cboDoors");
			this.cboDoors.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoors.FormattingEnabled = true;
			this.cboDoors.Name = "cboDoors";
			this.toolTip1.SetToolTip(this.cboDoors, componentResourceManager.GetString("cboDoors.ToolTip"));
			this.cboDoors.DropDown += new global::System.EventHandler(this.cboDoors_DropDown);
			componentResourceManager.ApplyResources(this.optController, "optController");
			this.optController.Name = "optController";
			this.toolTip1.SetToolTip(this.optController, componentResourceManager.GetString("optController.ToolTip"));
			this.optController.UseVisualStyleBackColor = true;
			this.optController.CheckedChanged += new global::System.EventHandler(this.optController_CheckedChanged);
			componentResourceManager.ApplyResources(this.optUSBReader, "optUSBReader");
			this.optUSBReader.Checked = true;
			this.optUSBReader.Name = "optUSBReader";
			this.optUSBReader.TabStop = true;
			this.toolTip1.SetToolTip(this.optUSBReader, componentResourceManager.GetString("optUSBReader.ToolTip"));
			this.optUSBReader.UseVisualStyleBackColor = true;
			this.optUSBReader.CheckedChanged += new global::System.EventHandler(this.optUSBReader_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.button1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnNext);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox5);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmFingerEnroll";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmFingerEnroll_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmUserAutoAdd_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmUserAutoAdd_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04002B70 RID: 11120
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x04002B78 RID: 11128
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002B79 RID: 11129
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002B7A RID: 11130
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002B7B RID: 11131
		private global::System.Windows.Forms.Button btnNext;

		// Token: 0x04002B7C RID: 11132
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002B7D RID: 11133
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04002B7E RID: 11134
		private global::System.Windows.Forms.ComboBox cboDoors;

		// Token: 0x04002B7F RID: 11135
		private global::System.Windows.Forms.ComboBox cboUART;

		// Token: 0x04002B80 RID: 11136
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002B81 RID: 11137
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002B82 RID: 11138
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04002B83 RID: 11139
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04002B84 RID: 11140
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04002B85 RID: 11141
		private global::System.Windows.Forms.RadioButton optController;

		// Token: 0x04002B86 RID: 11142
		private global::System.Windows.Forms.RadioButton optUSBReader;

		// Token: 0x04002B87 RID: 11143
		private global::System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;

		// Token: 0x04002B88 RID: 11144
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04002B89 RID: 11145
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04002B8A RID: 11146
		internal global::System.Windows.Forms.Button btnClearAll;

		// Token: 0x04002B8B RID: 11147
		internal global::System.Windows.Forms.Button btnDownloadAll;

		// Token: 0x04002B8C RID: 11148
		internal global::System.Windows.Forms.Button btnErrConnect;

		// Token: 0x04002B8D RID: 11149
		internal global::System.Windows.Forms.Button btnInstallUSBDriver;

		// Token: 0x04002B8E RID: 11150
		internal global::System.Windows.Forms.Button btnUploadAllUsers;
	}
}
