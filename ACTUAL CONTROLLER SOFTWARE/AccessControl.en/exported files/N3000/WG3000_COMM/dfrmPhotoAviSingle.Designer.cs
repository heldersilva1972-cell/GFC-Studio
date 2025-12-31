namespace WG3000_COMM
{
	// Token: 0x0200022B RID: 555
	public partial class dfrmPhotoAviSingle : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001092 RID: 4242 RVA: 0x0012AFA4 File Offset: 0x00129FA4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.CapturePhotoMemoryStream != null)
				{
					try
					{
						this.CapturePhotoMemoryStream.Dispose();
					}
					catch (global::System.Exception)
					{
					}
				}
				if (this.components != null)
				{
					this.components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0012AFF8 File Offset: 0x00129FF8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.dfrmPhotoAviSingle));
			this.Timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.Timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.Timer3 = new global::System.Windows.Forms.Timer(this.components);
			this.TimerStartSlow = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.viewJPEGByDefaultProgramToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.viewVideoByDefaultProgramToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.Panel2 = new global::System.Windows.Forms.Panel();
			this.TabControl2 = new global::System.Windows.Forms.TabControl();
			this.tabP4PanelA = new global::System.Windows.Forms.TabPage();
			this.P4Player6 = new global::System.Windows.Forms.Panel();
			this.P4Player4 = new global::System.Windows.Forms.Panel();
			this.P4Player5 = new global::System.Windows.Forms.Panel();
			this.P4Player3 = new global::System.Windows.Forms.Panel();
			this.P4Player2 = new global::System.Windows.Forms.Panel();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.TabControl1 = new global::System.Windows.Forms.TabControl();
			this.TabPage1 = new global::System.Windows.Forms.TabPage();
			this.btnFullVideo = new global::System.Windows.Forms.Button();
			this.picJpg = new global::System.Windows.Forms.PictureBox();
			this.TabPage2 = new global::System.Windows.Forms.TabPage();
			this.PanelMp4 = new global::System.Windows.Forms.Panel();
			this.btnRefreshAVI = new global::System.Windows.Forms.Button();
			this.Panel1 = new global::System.Windows.Forms.Panel();
			this.grpCaptured = new global::System.Windows.Forms.GroupBox();
			this.grpTop = new global::System.Windows.Forms.GroupBox();
			this.grpPhoto = new global::System.Windows.Forms.GroupBox();
			this.picPhoto = new global::System.Windows.Forms.PictureBox();
			this.PanelCapturePhoto = new global::System.Windows.Forms.Panel();
			this.richTxtCardInfo = new global::System.Windows.Forms.RichTextBox();
			this.lblErr = new global::System.Windows.Forms.Label();
			this.txtTime = new global::System.Windows.Forms.Label();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.txtCardInfo = new global::System.Windows.Forms.TextBox();
			this.btnPrev = new global::System.Windows.Forms.Button();
			this.btnFirst = new global::System.Windows.Forms.Button();
			this.btnNext = new global::System.Windows.Forms.Button();
			this.btnEnd = new global::System.Windows.Forms.Button();
			this.lblCount = new global::System.Windows.Forms.Label();
			this.txtPhotoInfo = new global::System.Windows.Forms.TextBox();
			this.P4Player1 = new global::System.Windows.Forms.Panel();
			this.contextMenuStrip1.SuspendLayout();
			this.TabControl2.SuspendLayout();
			this.tabP4PanelA.SuspendLayout();
			this.TabControl1.SuspendLayout();
			this.TabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.picJpg).BeginInit();
			this.TabPage2.SuspendLayout();
			this.PanelMp4.SuspendLayout();
			this.Panel1.SuspendLayout();
			this.grpTop.SuspendLayout();
			this.grpPhoto.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.picPhoto).BeginInit();
			base.SuspendLayout();
			this.Timer1.Enabled = true;
			this.Timer1.Interval = 300;
			this.Timer1.Tick += new global::System.EventHandler(this.Timer1_Tick);
			this.Timer2.Enabled = true;
			this.Timer2.Interval = 500;
			this.Timer2.Tick += new global::System.EventHandler(this.Timer2_Tick);
			this.Timer3.Interval = 3000;
			this.Timer3.Tick += new global::System.EventHandler(this.Timer3_Tick);
			this.TimerStartSlow.Enabled = true;
			this.TimerStartSlow.Interval = 300;
			this.TimerStartSlow.Tick += new global::System.EventHandler(this.TimerStartSlow_Tick);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.viewJPEGByDefaultProgramToolStripMenuItem, this.viewVideoByDefaultProgramToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.viewJPEGByDefaultProgramToolStripMenuItem, "viewJPEGByDefaultProgramToolStripMenuItem");
			this.viewJPEGByDefaultProgramToolStripMenuItem.Name = "viewJPEGByDefaultProgramToolStripMenuItem";
			this.viewJPEGByDefaultProgramToolStripMenuItem.Click += new global::System.EventHandler(this.viewJPEGByDefaultProgramToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.viewVideoByDefaultProgramToolStripMenuItem, "viewVideoByDefaultProgramToolStripMenuItem");
			this.viewVideoByDefaultProgramToolStripMenuItem.Name = "viewVideoByDefaultProgramToolStripMenuItem";
			this.viewVideoByDefaultProgramToolStripMenuItem.Click += new global::System.EventHandler(this.viewVideoByDefaultProgramToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.Panel2, "Panel2");
			this.Panel2.Name = "Panel2";
			this.toolTip1.SetToolTip(this.Panel2, componentResourceManager.GetString("Panel2.ToolTip"));
			componentResourceManager.ApplyResources(this.TabControl2, "TabControl2");
			this.TabControl2.Controls.Add(this.tabP4PanelA);
			this.TabControl2.Controls.Add(this.tabPage3);
			this.TabControl2.Controls.Add(this.tabPage4);
			this.TabControl2.DrawMode = global::System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.TabControl2.Multiline = true;
			this.TabControl2.Name = "TabControl2";
			this.TabControl2.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.TabControl2, componentResourceManager.GetString("TabControl2.ToolTip"));
			this.TabControl2.DrawItem += new global::System.Windows.Forms.DrawItemEventHandler(this.TabControl2_DrawItem);
			componentResourceManager.ApplyResources(this.tabP4PanelA, "tabP4PanelA");
			this.tabP4PanelA.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabP4PanelA.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.tabP4PanelA.Controls.Add(this.P4Player6);
			this.tabP4PanelA.Controls.Add(this.P4Player4);
			this.tabP4PanelA.Controls.Add(this.P4Player5);
			this.tabP4PanelA.Controls.Add(this.P4Player3);
			this.tabP4PanelA.Controls.Add(this.P4Player2);
			this.tabP4PanelA.Name = "tabP4PanelA";
			this.toolTip1.SetToolTip(this.tabP4PanelA, componentResourceManager.GetString("tabP4PanelA.ToolTip"));
			this.tabP4PanelA.SizeChanged += new global::System.EventHandler(this.tabP4PanelA_SizeChanged);
			this.tabP4PanelA.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			componentResourceManager.ApplyResources(this.P4Player6, "P4Player6");
			this.P4Player6.BackColor = global::System.Drawing.Color.Black;
			this.P4Player6.Name = "P4Player6";
			this.toolTip1.SetToolTip(this.P4Player6, componentResourceManager.GetString("P4Player6.ToolTip"));
			this.P4Player6.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player6.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this.P4Player4, "P4Player4");
			this.P4Player4.BackColor = global::System.Drawing.Color.Black;
			this.P4Player4.Name = "P4Player4";
			this.toolTip1.SetToolTip(this.P4Player4, componentResourceManager.GetString("P4Player4.ToolTip"));
			this.P4Player4.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player4.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this.P4Player5, "P4Player5");
			this.P4Player5.BackColor = global::System.Drawing.Color.Black;
			this.P4Player5.Name = "P4Player5";
			this.toolTip1.SetToolTip(this.P4Player5, componentResourceManager.GetString("P4Player5.ToolTip"));
			this.P4Player5.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player5.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this.P4Player3, "P4Player3");
			this.P4Player3.BackColor = global::System.Drawing.Color.Black;
			this.P4Player3.Name = "P4Player3";
			this.toolTip1.SetToolTip(this.P4Player3, componentResourceManager.GetString("P4Player3.ToolTip"));
			this.P4Player3.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player3.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this.P4Player2, "P4Player2");
			this.P4Player2.BackColor = global::System.Drawing.Color.Black;
			this.P4Player2.Name = "P4Player2";
			this.toolTip1.SetToolTip(this.P4Player2, componentResourceManager.GetString("P4Player2.ToolTip"));
			this.P4Player2.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player2.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.toolTip1.SetToolTip(this.tabPage3, componentResourceManager.GetString("tabPage3.ToolTip"));
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.toolTip1.SetToolTip(this.tabPage4, componentResourceManager.GetString("tabPage4.ToolTip"));
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.TabControl1, "TabControl1");
			this.TabControl1.ContextMenuStrip = this.contextMenuStrip1;
			this.TabControl1.Controls.Add(this.TabPage1);
			this.TabControl1.Controls.Add(this.TabPage2);
			this.TabControl1.Multiline = true;
			this.TabControl1.Name = "TabControl1";
			this.TabControl1.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.TabControl1, componentResourceManager.GetString("TabControl1.ToolTip"));
			this.TabControl1.SelectedIndexChanged += new global::System.EventHandler(this.TabControl1_SelectedIndexChanged);
			this.TabControl1.DoubleClick += new global::System.EventHandler(this.picJpg_DoubleClick);
			componentResourceManager.ApplyResources(this.TabPage1, "TabPage1");
			this.TabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.TabPage1.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.TabPage1.Controls.Add(this.btnFullVideo);
			this.TabPage1.Controls.Add(this.picJpg);
			this.TabPage1.ForeColor = global::System.Drawing.Color.Black;
			this.TabPage1.Name = "TabPage1";
			this.toolTip1.SetToolTip(this.TabPage1, componentResourceManager.GetString("TabPage1.ToolTip"));
			this.TabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnFullVideo, "btnFullVideo");
			this.btnFullVideo.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFullVideo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFullVideo.ForeColor = global::System.Drawing.Color.White;
			this.btnFullVideo.Name = "btnFullVideo";
			this.toolTip1.SetToolTip(this.btnFullVideo, componentResourceManager.GetString("btnFullVideo.ToolTip"));
			this.btnFullVideo.UseVisualStyleBackColor = false;
			this.btnFullVideo.Click += new global::System.EventHandler(this.btnFullVideo_Click);
			componentResourceManager.ApplyResources(this.picJpg, "picJpg");
			this.picJpg.BackColor = global::System.Drawing.Color.White;
			this.picJpg.ContextMenuStrip = this.contextMenuStrip1;
			this.picJpg.Name = "picJpg";
			this.picJpg.TabStop = false;
			this.toolTip1.SetToolTip(this.picJpg, componentResourceManager.GetString("picJpg.ToolTip"));
			this.picJpg.Click += new global::System.EventHandler(this.picJpg_DoubleClick);
			this.picJpg.DoubleClick += new global::System.EventHandler(this.picJpg_DoubleClick);
			componentResourceManager.ApplyResources(this.TabPage2, "TabPage2");
			this.TabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.TabPage2.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.TabPage2.Controls.Add(this.PanelMp4);
			this.TabPage2.ForeColor = global::System.Drawing.Color.Black;
			this.TabPage2.Name = "TabPage2";
			this.toolTip1.SetToolTip(this.TabPage2, componentResourceManager.GetString("TabPage2.ToolTip"));
			this.TabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.PanelMp4, "PanelMp4");
			this.PanelMp4.Controls.Add(this.btnRefreshAVI);
			this.PanelMp4.Name = "PanelMp4";
			this.toolTip1.SetToolTip(this.PanelMp4, componentResourceManager.GetString("PanelMp4.ToolTip"));
			this.PanelMp4.Click += new global::System.EventHandler(this.picJpg_DoubleClick);
			this.PanelMp4.DoubleClick += new global::System.EventHandler(this.picJpg_DoubleClick);
			componentResourceManager.ApplyResources(this.btnRefreshAVI, "btnRefreshAVI");
			this.btnRefreshAVI.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRefreshAVI.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRefreshAVI.ForeColor = global::System.Drawing.Color.White;
			this.btnRefreshAVI.Name = "btnRefreshAVI";
			this.toolTip1.SetToolTip(this.btnRefreshAVI, componentResourceManager.GetString("btnRefreshAVI.ToolTip"));
			this.btnRefreshAVI.UseVisualStyleBackColor = false;
			this.btnRefreshAVI.Click += new global::System.EventHandler(this.btnRefreshAVI_Click);
			componentResourceManager.ApplyResources(this.Panel1, "Panel1");
			this.Panel1.Controls.Add(this.grpCaptured);
			this.Panel1.Controls.Add(this.grpTop);
			this.Panel1.Controls.Add(this.P4Player1);
			this.Panel1.Name = "Panel1";
			this.toolTip1.SetToolTip(this.Panel1, componentResourceManager.GetString("Panel1.ToolTip"));
			componentResourceManager.ApplyResources(this.grpCaptured, "grpCaptured");
			this.grpCaptured.ForeColor = global::System.Drawing.Color.White;
			this.grpCaptured.Name = "grpCaptured";
			this.grpCaptured.TabStop = false;
			this.toolTip1.SetToolTip(this.grpCaptured, componentResourceManager.GetString("grpCaptured.ToolTip"));
			this.grpCaptured.Enter += new global::System.EventHandler(this.grpCaptured_Enter);
			componentResourceManager.ApplyResources(this.grpTop, "grpTop");
			this.grpTop.ContextMenuStrip = this.contextMenuStrip1;
			this.grpTop.Controls.Add(this.grpPhoto);
			this.grpTop.Controls.Add(this.richTxtCardInfo);
			this.grpTop.Controls.Add(this.lblErr);
			this.grpTop.Controls.Add(this.txtTime);
			this.grpTop.Controls.Add(this.btnClose);
			this.grpTop.Controls.Add(this.txtCardInfo);
			this.grpTop.Controls.Add(this.btnPrev);
			this.grpTop.Controls.Add(this.btnFirst);
			this.grpTop.Controls.Add(this.btnNext);
			this.grpTop.Controls.Add(this.btnEnd);
			this.grpTop.Controls.Add(this.lblCount);
			this.grpTop.Controls.Add(this.txtPhotoInfo);
			this.grpTop.Name = "grpTop";
			this.grpTop.TabStop = false;
			this.toolTip1.SetToolTip(this.grpTop, componentResourceManager.GetString("grpTop.ToolTip"));
			componentResourceManager.ApplyResources(this.grpPhoto, "grpPhoto");
			this.grpPhoto.Controls.Add(this.picPhoto);
			this.grpPhoto.Controls.Add(this.PanelCapturePhoto);
			this.grpPhoto.ForeColor = global::System.Drawing.Color.White;
			this.grpPhoto.Name = "grpPhoto";
			this.grpPhoto.TabStop = false;
			this.toolTip1.SetToolTip(this.grpPhoto, componentResourceManager.GetString("grpPhoto.ToolTip"));
			componentResourceManager.ApplyResources(this.picPhoto, "picPhoto");
			this.picPhoto.BackColor = global::System.Drawing.Color.Transparent;
			this.picPhoto.Name = "picPhoto";
			this.picPhoto.TabStop = false;
			this.toolTip1.SetToolTip(this.picPhoto, componentResourceManager.GetString("picPhoto.ToolTip"));
			componentResourceManager.ApplyResources(this.PanelCapturePhoto, "PanelCapturePhoto");
			this.PanelCapturePhoto.BackColor = global::System.Drawing.Color.White;
			this.PanelCapturePhoto.BorderStyle = global::System.Windows.Forms.BorderStyle.Fixed3D;
			this.PanelCapturePhoto.Name = "PanelCapturePhoto";
			this.toolTip1.SetToolTip(this.PanelCapturePhoto, componentResourceManager.GetString("PanelCapturePhoto.ToolTip"));
			componentResourceManager.ApplyResources(this.richTxtCardInfo, "richTxtCardInfo");
			this.richTxtCardInfo.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTxtCardInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTxtCardInfo.Name = "richTxtCardInfo";
			this.toolTip1.SetToolTip(this.richTxtCardInfo, componentResourceManager.GetString("richTxtCardInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.lblErr, "lblErr");
			this.lblErr.ForeColor = global::System.Drawing.Color.Transparent;
			this.lblErr.Name = "lblErr";
			this.toolTip1.SetToolTip(this.lblErr, componentResourceManager.GetString("lblErr.ToolTip"));
			componentResourceManager.ApplyResources(this.txtTime, "txtTime");
			this.txtTime.BackColor = global::System.Drawing.Color.Transparent;
			this.txtTime.ForeColor = global::System.Drawing.Color.White;
			this.txtTime.Name = "txtTime";
			this.toolTip1.SetToolTip(this.txtTime, componentResourceManager.GetString("txtTime.ToolTip"));
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.toolTip1.SetToolTip(this.btnClose, componentResourceManager.GetString("btnClose.ToolTip"));
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.txtCardInfo, "txtCardInfo");
			this.txtCardInfo.Name = "txtCardInfo";
			this.txtCardInfo.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtCardInfo, componentResourceManager.GetString("txtCardInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.btnPrev, "btnPrev");
			this.btnPrev.BackColor = global::System.Drawing.Color.Transparent;
			this.btnPrev.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnPrev.ForeColor = global::System.Drawing.Color.White;
			this.btnPrev.Name = "btnPrev";
			this.toolTip1.SetToolTip(this.btnPrev, componentResourceManager.GetString("btnPrev.ToolTip"));
			this.btnPrev.UseVisualStyleBackColor = false;
			this.btnPrev.Click += new global::System.EventHandler(this.btnPrev_Click);
			componentResourceManager.ApplyResources(this.btnFirst, "btnFirst");
			this.btnFirst.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFirst.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFirst.ForeColor = global::System.Drawing.Color.White;
			this.btnFirst.Name = "btnFirst";
			this.toolTip1.SetToolTip(this.btnFirst, componentResourceManager.GetString("btnFirst.ToolTip"));
			this.btnFirst.UseVisualStyleBackColor = false;
			this.btnFirst.Click += new global::System.EventHandler(this.btnFirst_Click);
			componentResourceManager.ApplyResources(this.btnNext, "btnNext");
			this.btnNext.BackColor = global::System.Drawing.Color.Transparent;
			this.btnNext.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNext.ForeColor = global::System.Drawing.Color.White;
			this.btnNext.Name = "btnNext";
			this.toolTip1.SetToolTip(this.btnNext, componentResourceManager.GetString("btnNext.ToolTip"));
			this.btnNext.UseVisualStyleBackColor = false;
			this.btnNext.Click += new global::System.EventHandler(this.btnNext_Click);
			componentResourceManager.ApplyResources(this.btnEnd, "btnEnd");
			this.btnEnd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEnd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEnd.ForeColor = global::System.Drawing.Color.White;
			this.btnEnd.Name = "btnEnd";
			this.toolTip1.SetToolTip(this.btnEnd, componentResourceManager.GetString("btnEnd.ToolTip"));
			this.btnEnd.UseVisualStyleBackColor = false;
			this.btnEnd.Click += new global::System.EventHandler(this.btnEnd_Click);
			componentResourceManager.ApplyResources(this.lblCount, "lblCount");
			this.lblCount.BackColor = global::System.Drawing.Color.Transparent;
			this.lblCount.ForeColor = global::System.Drawing.Color.White;
			this.lblCount.Name = "lblCount";
			this.toolTip1.SetToolTip(this.lblCount, componentResourceManager.GetString("lblCount.ToolTip"));
			componentResourceManager.ApplyResources(this.txtPhotoInfo, "txtPhotoInfo");
			this.txtPhotoInfo.Name = "txtPhotoInfo";
			this.txtPhotoInfo.ReadOnly = true;
			this.toolTip1.SetToolTip(this.txtPhotoInfo, componentResourceManager.GetString("txtPhotoInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.P4Player1, "P4Player1");
			this.P4Player1.BackColor = global::System.Drawing.Color.Black;
			this.P4Player1.Name = "P4Player1";
			this.toolTip1.SetToolTip(this.P4Player1, componentResourceManager.GetString("P4Player1.ToolTip"));
			this.P4Player1.Click += new global::System.EventHandler(this.tabP4PanelA_Click);
			this.P4Player1.DoubleClick += new global::System.EventHandler(this.P4Player_DoubleClick);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.Panel2);
			base.Controls.Add(this.TabControl2);
			base.Controls.Add(this.TabControl1);
			base.Controls.Add(this.Panel1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmPhotoAviSingle";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Closing += new global::System.ComponentModel.CancelEventHandler(this.dfrmPhotoAvi_Closing);
			base.Closed += new global::System.EventHandler(this.dfrmPhotoAvi_Closed);
			base.Load += new global::System.EventHandler(this.dfrmPhotoAvi_Load);
			base.SizeChanged += new global::System.EventHandler(this.dfrmPhotoAvi_SizeChanged);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPhotoAvi_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.TabControl2.ResumeLayout(false);
			this.tabP4PanelA.ResumeLayout(false);
			this.TabControl1.ResumeLayout(false);
			this.TabPage1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.picJpg).EndInit();
			this.TabPage2.ResumeLayout(false);
			this.PanelMp4.ResumeLayout(false);
			this.Panel1.ResumeLayout(false);
			this.grpTop.ResumeLayout(false);
			this.grpTop.PerformLayout();
			this.grpPhoto.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.picPhoto).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001D27 RID: 7463
		private global::System.IO.MemoryStream CapturePhotoMemoryStream;

		// Token: 0x04001D40 RID: 7488
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001D41 RID: 7489
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04001D42 RID: 7490
		private global::System.Windows.Forms.GroupBox grpCaptured;

		// Token: 0x04001D43 RID: 7491
		private global::System.Windows.Forms.GroupBox grpPhoto;

		// Token: 0x04001D45 RID: 7493
		private global::System.Windows.Forms.RichTextBox richTxtCardInfo;

		// Token: 0x04001D47 RID: 7495
		private global::System.Windows.Forms.TabControl TabControl1;

		// Token: 0x04001D48 RID: 7496
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04001D49 RID: 7497
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04001D4B RID: 7499
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04001D4C RID: 7500
		private global::System.Windows.Forms.ToolStripMenuItem viewJPEGByDefaultProgramToolStripMenuItem;

		// Token: 0x04001D4D RID: 7501
		private global::System.Windows.Forms.ToolStripMenuItem viewVideoByDefaultProgramToolStripMenuItem;

		// Token: 0x04001D4E RID: 7502
		internal global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001D4F RID: 7503
		internal global::System.Windows.Forms.Button btnEnd;

		// Token: 0x04001D50 RID: 7504
		internal global::System.Windows.Forms.Button btnFirst;

		// Token: 0x04001D51 RID: 7505
		internal global::System.Windows.Forms.Button btnFullVideo;

		// Token: 0x04001D52 RID: 7506
		internal global::System.Windows.Forms.Button btnNext;

		// Token: 0x04001D53 RID: 7507
		internal global::System.Windows.Forms.Button btnPrev;

		// Token: 0x04001D54 RID: 7508
		internal global::System.Windows.Forms.Button btnRefreshAVI;

		// Token: 0x04001D55 RID: 7509
		internal global::System.Windows.Forms.GroupBox grpTop;

		// Token: 0x04001D56 RID: 7510
		internal global::System.Windows.Forms.Label lblCount;

		// Token: 0x04001D57 RID: 7511
		internal global::System.Windows.Forms.Label lblErr;

		// Token: 0x04001D58 RID: 7512
		internal global::System.Windows.Forms.Panel P4Player1;

		// Token: 0x04001D59 RID: 7513
		internal global::System.Windows.Forms.Panel P4Player2;

		// Token: 0x04001D5A RID: 7514
		internal global::System.Windows.Forms.Panel P4Player3;

		// Token: 0x04001D5B RID: 7515
		internal global::System.Windows.Forms.Panel P4Player4;

		// Token: 0x04001D5C RID: 7516
		internal global::System.Windows.Forms.Panel P4Player5;

		// Token: 0x04001D5D RID: 7517
		internal global::System.Windows.Forms.Panel P4Player6;

		// Token: 0x04001D5E RID: 7518
		internal global::System.Windows.Forms.Panel Panel1;

		// Token: 0x04001D5F RID: 7519
		internal global::System.Windows.Forms.Panel Panel2;

		// Token: 0x04001D60 RID: 7520
		internal global::System.Windows.Forms.Panel PanelCapturePhoto;

		// Token: 0x04001D61 RID: 7521
		internal global::System.Windows.Forms.Panel PanelMp4;

		// Token: 0x04001D62 RID: 7522
		internal global::System.Windows.Forms.PictureBox picJpg;

		// Token: 0x04001D63 RID: 7523
		internal global::System.Windows.Forms.PictureBox picPhoto;

		// Token: 0x04001D64 RID: 7524
		internal global::System.Windows.Forms.TabControl TabControl2;

		// Token: 0x04001D65 RID: 7525
		internal global::System.Windows.Forms.TabPage tabP4PanelA;

		// Token: 0x04001D66 RID: 7526
		internal global::System.Windows.Forms.TabPage TabPage1;

		// Token: 0x04001D67 RID: 7527
		internal global::System.Windows.Forms.TabPage TabPage2;

		// Token: 0x04001D68 RID: 7528
		internal global::System.Windows.Forms.Timer Timer1;

		// Token: 0x04001D69 RID: 7529
		internal global::System.Windows.Forms.Timer Timer2;

		// Token: 0x04001D6A RID: 7530
		internal global::System.Windows.Forms.Timer Timer3;

		// Token: 0x04001D6B RID: 7531
		internal global::System.Windows.Forms.Timer TimerStartSlow;

		// Token: 0x04001D6C RID: 7532
		internal global::System.Windows.Forms.TextBox txtCardInfo;

		// Token: 0x04001D6D RID: 7533
		internal global::System.Windows.Forms.TextBox txtPhotoInfo;

		// Token: 0x04001D6E RID: 7534
		internal global::System.Windows.Forms.Label txtTime;
	}
}
