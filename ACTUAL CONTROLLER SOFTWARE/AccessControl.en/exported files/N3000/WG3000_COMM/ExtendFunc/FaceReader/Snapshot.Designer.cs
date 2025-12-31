namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002DC RID: 732
	public partial class Snapshot : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060014B1 RID: 5297 RVA: 0x00197B5D File Offset: 0x00196B5D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00197B7C File Offset: 0x00196B7C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.FaceReader.Snapshot));
			this.toolTip = new global::System.Windows.Forms.ToolTip(this.components);
			this.label3 = new global::System.Windows.Forms.Label();
			this.snapshotResolutionsCombo = new global::System.Windows.Forms.ComboBox();
			this.triggerButton = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.label1 = new global::System.Windows.Forms.Label();
			this.devicesCombo = new global::System.Windows.Forms.ComboBox();
			this.disconnectButton = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.connectButton = new global::System.Windows.Forms.Button();
			this.videoResolutionsCombo = new global::System.Windows.Forms.ComboBox();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.videoSourcePlayer = new global::AForge.Controls.VideoSourcePlayer();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.BackColor = global::System.Drawing.Color.FromArgb(192, 255, 192);
			this.toolTip.InitialDelay = 100;
			this.toolTip.ReshowDelay = 100;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.snapshotResolutionsCombo, "snapshotResolutionsCombo");
			this.snapshotResolutionsCombo.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.snapshotResolutionsCombo.FormattingEnabled = true;
			this.snapshotResolutionsCombo.Name = "snapshotResolutionsCombo";
			this.toolTip.SetToolTip(this.snapshotResolutionsCombo, componentResourceManager.GetString("snapshotResolutionsCombo.ToolTip"));
			componentResourceManager.ApplyResources(this.triggerButton, "triggerButton");
			this.triggerButton.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.triggerButton.ForeColor = global::System.Drawing.Color.White;
			this.triggerButton.Name = "triggerButton";
			this.toolTip.SetToolTip(this.triggerButton, componentResourceManager.GetString("triggerButton.ToolTip"));
			this.triggerButton.UseVisualStyleBackColor = true;
			this.triggerButton.Click += new global::System.EventHandler(this.triggerButton_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.toolTip.SetToolTip(this.tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.toolTip.SetToolTip(this.tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.devicesCombo);
			this.tabPage2.Controls.Add(this.disconnectButton);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.connectButton);
			this.tabPage2.Controls.Add(this.videoResolutionsCombo);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.snapshotResolutionsCombo);
			this.tabPage2.Name = "tabPage2";
			this.toolTip.SetToolTip(this.tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.devicesCombo, "devicesCombo");
			this.devicesCombo.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.devicesCombo.FormattingEnabled = true;
			this.devicesCombo.Name = "devicesCombo";
			this.toolTip.SetToolTip(this.devicesCombo, componentResourceManager.GetString("devicesCombo.ToolTip"));
			this.devicesCombo.SelectedIndexChanged += new global::System.EventHandler(this.devicesCombo_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.disconnectButton, "disconnectButton");
			this.disconnectButton.Name = "disconnectButton";
			this.toolTip.SetToolTip(this.disconnectButton, componentResourceManager.GetString("disconnectButton.ToolTip"));
			this.disconnectButton.UseVisualStyleBackColor = true;
			this.disconnectButton.Click += new global::System.EventHandler(this.disconnectButton_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.connectButton, "connectButton");
			this.connectButton.Name = "connectButton";
			this.toolTip.SetToolTip(this.connectButton, componentResourceManager.GetString("connectButton.ToolTip"));
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new global::System.EventHandler(this.connectButton_Click);
			componentResourceManager.ApplyResources(this.videoResolutionsCombo, "videoResolutionsCombo");
			this.videoResolutionsCombo.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.videoResolutionsCombo.FormattingEnabled = true;
			this.videoResolutionsCombo.Name = "videoResolutionsCombo";
			this.toolTip.SetToolTip(this.videoResolutionsCombo, componentResourceManager.GetString("videoResolutionsCombo.ToolTip"));
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.videoSourcePlayer);
			this.panel1.Name = "panel1";
			this.toolTip.SetToolTip(this.panel1, componentResourceManager.GetString("panel1.ToolTip"));
			componentResourceManager.ApplyResources(this.videoSourcePlayer, "videoSourcePlayer");
			this.videoSourcePlayer.AutoSizeControl = true;
			this.videoSourcePlayer.BackColor = global::System.Drawing.SystemColors.ControlDark;
			this.videoSourcePlayer.ForeColor = global::System.Drawing.Color.DarkRed;
			this.videoSourcePlayer.Name = "videoSourcePlayer";
			this.toolTip.SetToolTip(this.videoSourcePlayer, componentResourceManager.GetString("videoSourcePlayer.ToolTip"));
			this.videoSourcePlayer.VideoSource = null;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.triggerButton);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.panel1);
			base.Name = "Snapshot";
			this.toolTip.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.TopMost = true;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.Snapshot_FormClosed);
			base.Load += new global::System.EventHandler(this.MainForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04002B42 RID: 11074
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002B43 RID: 11075
		private global::System.Windows.Forms.Button connectButton;

		// Token: 0x04002B44 RID: 11076
		private global::System.Windows.Forms.ComboBox devicesCombo;

		// Token: 0x04002B45 RID: 11077
		private global::System.Windows.Forms.Button disconnectButton;

		// Token: 0x04002B46 RID: 11078
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002B47 RID: 11079
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002B48 RID: 11080
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002B49 RID: 11081
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04002B4A RID: 11082
		private global::System.Windows.Forms.ComboBox snapshotResolutionsCombo;

		// Token: 0x04002B4B RID: 11083
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002B4C RID: 11084
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002B4D RID: 11085
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002B4E RID: 11086
		private global::System.Windows.Forms.ToolTip toolTip;

		// Token: 0x04002B4F RID: 11087
		private global::System.Windows.Forms.Button triggerButton;

		// Token: 0x04002B50 RID: 11088
		private global::System.Windows.Forms.ComboBox videoResolutionsCombo;

		// Token: 0x04002B51 RID: 11089
		private global::AForge.Controls.VideoSourcePlayer videoSourcePlayer;
	}
}
