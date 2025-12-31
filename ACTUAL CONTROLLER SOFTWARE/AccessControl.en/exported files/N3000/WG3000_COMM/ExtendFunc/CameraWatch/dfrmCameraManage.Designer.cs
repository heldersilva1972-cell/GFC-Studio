namespace WG3000_COMM.ExtendFunc.CameraWatch
{
	// Token: 0x0200022E RID: 558
	public partial class dfrmCameraManage : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010C1 RID: 4289 RVA: 0x0013076C File Offset: 0x0012F76C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0013078C File Offset: 0x0012F78C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.CameraWatch.dfrmCameraManage));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.folderBrowserDialog1 = new global::System.Windows.Forms.FolderBrowserDialog();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.setAVIJPGSaveDirectoryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setAVIJPGViewDirectoryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreAVIJPGSaveDirectoryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreAVIJPGViewDirectoryToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.directoryInformationToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dontCaputreOnThisPCToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip2 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.specialSetToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip3 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.videoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.forcedDisableToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.autoSelectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.clientDemoToolToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.optNotPreview = new global::System.Windows.Forms.RadioButton();
			this.optBoth = new global::System.Windows.Forms.RadioButton();
			this.optOnlyCapture = new global::System.Windows.Forms.RadioButton();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.comboBox1 = new global::System.Windows.Forms.ComboBox();
			this.chkDeleteOldAviJpg = new global::System.Windows.Forms.CheckBox();
			this.dgvReaderCamera = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnEditReaderCamera = new global::System.Windows.Forms.Button();
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.btnView = new global::System.Windows.Forms.Button();
			this.dgvCameras = new global::System.Windows.Forms.DataGridView();
			this.f_CameraID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CameraName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Channel = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.chkAutoSyncTime = new global::System.Windows.Forms.CheckBox();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.optQuality0 = new global::System.Windows.Forms.RadioButton();
			this.optQuality2 = new global::System.Windows.Forms.RadioButton();
			this.optQuality1 = new global::System.Windows.Forms.RadioButton();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.optResolution2 = new global::System.Windows.Forms.RadioButton();
			this.optResolution0 = new global::System.Windows.Forms.RadioButton();
			this.optResolution1 = new global::System.Windows.Forms.RadioButton();
			this.contextMenuStrip1.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.contextMenuStrip3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvReaderCamera).BeginInit();
			this.grpbController.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvCameras).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
			this.folderBrowserDialog1.ShowNewFolderButton = false;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.setAVIJPGSaveDirectoryToolStripMenuItem, this.setAVIJPGViewDirectoryToolStripMenuItem, this.restoreAVIJPGSaveDirectoryToolStripMenuItem, this.restoreAVIJPGViewDirectoryToolStripMenuItem, this.directoryInformationToolStripMenuItem, this.dontCaputreOnThisPCToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.setAVIJPGSaveDirectoryToolStripMenuItem, "setAVIJPGSaveDirectoryToolStripMenuItem");
			this.setAVIJPGSaveDirectoryToolStripMenuItem.Name = "setAVIJPGSaveDirectoryToolStripMenuItem";
			this.setAVIJPGSaveDirectoryToolStripMenuItem.Click += new global::System.EventHandler(this.setAVIJPGSaveDirectoryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setAVIJPGViewDirectoryToolStripMenuItem, "setAVIJPGViewDirectoryToolStripMenuItem");
			this.setAVIJPGViewDirectoryToolStripMenuItem.Name = "setAVIJPGViewDirectoryToolStripMenuItem";
			this.setAVIJPGViewDirectoryToolStripMenuItem.Click += new global::System.EventHandler(this.setAVIJPGViewDirectoryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreAVIJPGSaveDirectoryToolStripMenuItem, "restoreAVIJPGSaveDirectoryToolStripMenuItem");
			this.restoreAVIJPGSaveDirectoryToolStripMenuItem.Name = "restoreAVIJPGSaveDirectoryToolStripMenuItem";
			this.restoreAVIJPGSaveDirectoryToolStripMenuItem.Click += new global::System.EventHandler(this.restoreAVIJPGSaveDirectoryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreAVIJPGViewDirectoryToolStripMenuItem, "restoreAVIJPGViewDirectoryToolStripMenuItem");
			this.restoreAVIJPGViewDirectoryToolStripMenuItem.Name = "restoreAVIJPGViewDirectoryToolStripMenuItem";
			this.restoreAVIJPGViewDirectoryToolStripMenuItem.Click += new global::System.EventHandler(this.restoreAVIJPGViewDirectoryToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.directoryInformationToolStripMenuItem, "directoryInformationToolStripMenuItem");
			this.directoryInformationToolStripMenuItem.Name = "directoryInformationToolStripMenuItem";
			this.directoryInformationToolStripMenuItem.Click += new global::System.EventHandler(this.directoryInformationToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dontCaputreOnThisPCToolStripMenuItem, "dontCaputreOnThisPCToolStripMenuItem");
			this.dontCaputreOnThisPCToolStripMenuItem.Name = "dontCaputreOnThisPCToolStripMenuItem";
			this.dontCaputreOnThisPCToolStripMenuItem.Click += new global::System.EventHandler(this.dontCaputreOnThisPCToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
			this.contextMenuStrip2.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.specialSetToolStripMenuItem });
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			componentResourceManager.ApplyResources(this.specialSetToolStripMenuItem, "specialSetToolStripMenuItem");
			this.specialSetToolStripMenuItem.Name = "specialSetToolStripMenuItem";
			this.specialSetToolStripMenuItem.Click += new global::System.EventHandler(this.specialSetToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.contextMenuStrip3, "contextMenuStrip3");
			this.contextMenuStrip3.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.videoToolStripMenuItem, this.toolStripMenuItem2, this.clientDemoToolToolStripMenuItem });
			this.contextMenuStrip3.Name = "contextMenuStrip3";
			componentResourceManager.ApplyResources(this.videoToolStripMenuItem, "videoToolStripMenuItem");
			this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
			this.videoToolStripMenuItem.Click += new global::System.EventHandler(this.videoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem2.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.forcedDisableToolStripMenuItem, this.autoSelectToolStripMenuItem });
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			componentResourceManager.ApplyResources(this.forcedDisableToolStripMenuItem, "forcedDisableToolStripMenuItem");
			this.forcedDisableToolStripMenuItem.Name = "forcedDisableToolStripMenuItem";
			this.forcedDisableToolStripMenuItem.Click += new global::System.EventHandler(this.forcedDisableToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.autoSelectToolStripMenuItem, "autoSelectToolStripMenuItem");
			this.autoSelectToolStripMenuItem.Name = "autoSelectToolStripMenuItem";
			this.autoSelectToolStripMenuItem.Click += new global::System.EventHandler(this.autoSelectToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.clientDemoToolToolStripMenuItem, "clientDemoToolToolStripMenuItem");
			this.clientDemoToolToolStripMenuItem.Name = "clientDemoToolToolStripMenuItem";
			this.clientDemoToolToolStripMenuItem.Click += new global::System.EventHandler(this.clientDemoToolToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.optNotPreview);
			this.groupBox2.Controls.Add(this.optBoth);
			this.groupBox2.Controls.Add(this.optOnlyCapture);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.optNotPreview, "optNotPreview");
			this.optNotPreview.Checked = true;
			this.optNotPreview.ForeColor = global::System.Drawing.Color.White;
			this.optNotPreview.Name = "optNotPreview";
			this.optNotPreview.TabStop = true;
			this.optNotPreview.UseVisualStyleBackColor = true;
			this.optNotPreview.CheckedChanged += new global::System.EventHandler(this.optBoth_CheckedChanged);
			componentResourceManager.ApplyResources(this.optBoth, "optBoth");
			this.optBoth.ForeColor = global::System.Drawing.Color.White;
			this.optBoth.Name = "optBoth";
			this.optBoth.UseVisualStyleBackColor = true;
			this.optBoth.CheckedChanged += new global::System.EventHandler(this.optBoth_CheckedChanged);
			componentResourceManager.ApplyResources(this.optOnlyCapture, "optOnlyCapture");
			this.optOnlyCapture.ForeColor = global::System.Drawing.Color.White;
			this.optOnlyCapture.Name = "optOnlyCapture";
			this.optOnlyCapture.UseVisualStyleBackColor = true;
			this.optOnlyCapture.CheckedChanged += new global::System.EventHandler(this.optBoth_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.chkDeleteOldAviJpg);
			this.groupBox1.Controls.Add(this.dgvReaderCamera);
			this.groupBox1.Controls.Add(this.btnEditReaderCamera);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.comboBox1, "comboBox1");
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("comboBox1.Items"),
				componentResourceManager.GetString("comboBox1.Items1"),
				componentResourceManager.GetString("comboBox1.Items2"),
				componentResourceManager.GetString("comboBox1.Items3"),
				componentResourceManager.GetString("comboBox1.Items4"),
				componentResourceManager.GetString("comboBox1.Items5"),
				componentResourceManager.GetString("comboBox1.Items6"),
				componentResourceManager.GetString("comboBox1.Items7")
			});
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.SelectedIndexChanged += new global::System.EventHandler(this.comboBox1_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.chkDeleteOldAviJpg, "chkDeleteOldAviJpg");
			this.chkDeleteOldAviJpg.Name = "chkDeleteOldAviJpg";
			this.chkDeleteOldAviJpg.UseVisualStyleBackColor = true;
			this.chkDeleteOldAviJpg.CheckedChanged += new global::System.EventHandler(this.chkDeleteOldAviJpg_CheckedChanged);
			componentResourceManager.ApplyResources(this.dgvReaderCamera, "dgvReaderCamera");
			this.dgvReaderCamera.AllowUserToAddRows = false;
			this.dgvReaderCamera.AllowUserToDeleteRows = false;
			this.dgvReaderCamera.AllowUserToOrderColumns = true;
			this.dgvReaderCamera.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvReaderCamera.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvReaderCamera.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvReaderCamera.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2 });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvReaderCamera.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvReaderCamera.EnableHeadersVisualStyles = false;
			this.dgvReaderCamera.MultiSelect = false;
			this.dgvReaderCamera.Name = "dgvReaderCamera";
			this.dgvReaderCamera.ReadOnly = true;
			this.dgvReaderCamera.RowTemplate.Height = 23;
			this.dgvReaderCamera.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvReaderCamera.DoubleClick += new global::System.EventHandler(this.dgvReaderCamera_DoubleClick);
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnEditReaderCamera, "btnEditReaderCamera");
			this.btnEditReaderCamera.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditReaderCamera.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditReaderCamera.ForeColor = global::System.Drawing.Color.White;
			this.btnEditReaderCamera.Name = "btnEditReaderCamera";
			this.btnEditReaderCamera.UseVisualStyleBackColor = false;
			this.btnEditReaderCamera.Click += new global::System.EventHandler(this.btnEditReaderCamera_Click);
			componentResourceManager.ApplyResources(this.grpbController, "grpbController");
			this.grpbController.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbController.Controls.Add(this.btnView);
			this.grpbController.Controls.Add(this.dgvCameras);
			this.grpbController.Controls.Add(this.btnEdit);
			this.grpbController.Controls.Add(this.btnAdd);
			this.grpbController.Controls.Add(this.btnDel);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			componentResourceManager.ApplyResources(this.btnView, "btnView");
			this.btnView.BackColor = global::System.Drawing.Color.Transparent;
			this.btnView.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnView.ForeColor = global::System.Drawing.Color.White;
			this.btnView.Name = "btnView";
			this.btnView.UseVisualStyleBackColor = false;
			this.btnView.Click += new global::System.EventHandler(this.videoToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.dgvCameras, "dgvCameras");
			this.dgvCameras.AllowUserToAddRows = false;
			this.dgvCameras.AllowUserToDeleteRows = false;
			this.dgvCameras.AllowUserToOrderColumns = true;
			this.dgvCameras.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvCameras.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvCameras.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvCameras.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_CameraID, this.f_CameraName, this.f_IP, this.f_Port, this.f_Channel, this.f_Notes });
			this.dgvCameras.ContextMenuStrip = this.contextMenuStrip3;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvCameras.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvCameras.EnableHeadersVisualStyles = false;
			this.dgvCameras.MultiSelect = false;
			this.dgvCameras.Name = "dgvCameras";
			this.dgvCameras.ReadOnly = true;
			this.dgvCameras.RowTemplate.Height = 23;
			this.dgvCameras.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCameras.DoubleClick += new global::System.EventHandler(this.dgvCameras_DoubleClick);
			componentResourceManager.ApplyResources(this.f_CameraID, "f_CameraID");
			this.f_CameraID.Name = "f_CameraID";
			this.f_CameraID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_CameraName, "f_CameraName");
			this.f_CameraName.Name = "f_CameraName";
			this.f_CameraName.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_IP.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.f_IP, "f_IP");
			this.f_IP.Name = "f_IP";
			this.f_IP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Port, "f_Port");
			this.f_Port.Name = "f_Port";
			this.f_Port.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Channel, "f_Channel");
			this.f_Channel.Name = "f_Channel";
			this.f_Channel.ReadOnly = true;
			this.f_Notes.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.chkAutoSyncTime, "chkAutoSyncTime");
			this.chkAutoSyncTime.Checked = true;
			this.chkAutoSyncTime.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAutoSyncTime.ForeColor = global::System.Drawing.Color.White;
			this.chkAutoSyncTime.Name = "chkAutoSyncTime";
			this.chkAutoSyncTime.UseVisualStyleBackColor = true;
			this.chkAutoSyncTime.CheckedChanged += new global::System.EventHandler(this.chkAutoSyncTime_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.optQuality0);
			this.groupBox4.Controls.Add(this.optQuality2);
			this.groupBox4.Controls.Add(this.optQuality1);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.optQuality0, "optQuality0");
			this.optQuality0.Checked = true;
			this.optQuality0.ForeColor = global::System.Drawing.Color.White;
			this.optQuality0.Name = "optQuality0";
			this.optQuality0.TabStop = true;
			this.optQuality0.UseVisualStyleBackColor = true;
			this.optQuality0.CheckedChanged += new global::System.EventHandler(this.optQuality0_CheckedChanged);
			componentResourceManager.ApplyResources(this.optQuality2, "optQuality2");
			this.optQuality2.ForeColor = global::System.Drawing.Color.White;
			this.optQuality2.Name = "optQuality2";
			this.optQuality2.UseVisualStyleBackColor = true;
			this.optQuality2.CheckedChanged += new global::System.EventHandler(this.optQuality0_CheckedChanged);
			componentResourceManager.ApplyResources(this.optQuality1, "optQuality1");
			this.optQuality1.ForeColor = global::System.Drawing.Color.White;
			this.optQuality1.Name = "optQuality1";
			this.optQuality1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.ContextMenuStrip = this.contextMenuStrip2;
			this.groupBox3.Controls.Add(this.optResolution2);
			this.groupBox3.Controls.Add(this.optResolution0);
			this.groupBox3.Controls.Add(this.optResolution1);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.optResolution2, "optResolution2");
			this.optResolution2.Checked = true;
			this.optResolution2.ForeColor = global::System.Drawing.Color.White;
			this.optResolution2.Name = "optResolution2";
			this.optResolution2.TabStop = true;
			this.optResolution2.UseVisualStyleBackColor = true;
			this.optResolution2.CheckedChanged += new global::System.EventHandler(this.optResolution2_CheckedChanged);
			componentResourceManager.ApplyResources(this.optResolution0, "optResolution0");
			this.optResolution0.ForeColor = global::System.Drawing.Color.White;
			this.optResolution0.Name = "optResolution0";
			this.optResolution0.UseVisualStyleBackColor = true;
			this.optResolution0.CheckedChanged += new global::System.EventHandler(this.optResolution2_CheckedChanged);
			componentResourceManager.ApplyResources(this.optResolution1, "optResolution1");
			this.optResolution1.ForeColor = global::System.Drawing.Color.White;
			this.optResolution1.Name = "optResolution1";
			this.optResolution1.UseVisualStyleBackColor = true;
			this.optResolution1.CheckedChanged += new global::System.EventHandler(this.optResolution2_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.grpbController);
			base.Controls.Add(this.chkAutoSyncTime);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.btnClose);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCameraManage";
			base.Load += new global::System.EventHandler(this.dfrmCameraManage_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCameraManage_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.contextMenuStrip3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvReaderCamera).EndInit();
			this.grpbController.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvCameras).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001D9F RID: 7583
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001DA0 RID: 7584
		private global::System.Windows.Forms.ToolStripMenuItem autoSelectToolStripMenuItem;

		// Token: 0x04001DA1 RID: 7585
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04001DA2 RID: 7586
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001DA3 RID: 7587
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04001DA4 RID: 7588
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04001DA5 RID: 7589
		private global::System.Windows.Forms.Button btnEditReaderCamera;

		// Token: 0x04001DA6 RID: 7590
		private global::System.Windows.Forms.Button btnView;

		// Token: 0x04001DA7 RID: 7591
		private global::System.Windows.Forms.CheckBox chkAutoSyncTime;

		// Token: 0x04001DA8 RID: 7592
		private global::System.Windows.Forms.CheckBox chkDeleteOldAviJpg;

		// Token: 0x04001DA9 RID: 7593
		private global::System.Windows.Forms.ToolStripMenuItem clientDemoToolToolStripMenuItem;

		// Token: 0x04001DAA RID: 7594
		private global::System.Windows.Forms.ComboBox comboBox1;

		// Token: 0x04001DAB RID: 7595
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04001DAC RID: 7596
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip2;

		// Token: 0x04001DAD RID: 7597
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip3;

		// Token: 0x04001DAE RID: 7598
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04001DAF RID: 7599
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04001DB0 RID: 7600
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04001DB1 RID: 7601
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04001DB2 RID: 7602
		private global::System.Windows.Forms.DataGridView dgvCameras;

		// Token: 0x04001DB3 RID: 7603
		private global::System.Windows.Forms.DataGridView dgvReaderCamera;

		// Token: 0x04001DB4 RID: 7604
		private global::System.Windows.Forms.ToolStripMenuItem directoryInformationToolStripMenuItem;

		// Token: 0x04001DB5 RID: 7605
		private global::System.Windows.Forms.ToolStripMenuItem dontCaputreOnThisPCToolStripMenuItem;

		// Token: 0x04001DB6 RID: 7606
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CameraID;

		// Token: 0x04001DB7 RID: 7607
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CameraName;

		// Token: 0x04001DB8 RID: 7608
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Channel;

		// Token: 0x04001DB9 RID: 7609
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04001DBA RID: 7610
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04001DBB RID: 7611
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Port;

		// Token: 0x04001DBC RID: 7612
		private global::System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;

		// Token: 0x04001DBD RID: 7613
		private global::System.Windows.Forms.ToolStripMenuItem forcedDisableToolStripMenuItem;

		// Token: 0x04001DBE RID: 7614
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001DBF RID: 7615
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04001DC0 RID: 7616
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04001DC1 RID: 7617
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04001DC2 RID: 7618
		private global::System.Windows.Forms.GroupBox grpbController;

		// Token: 0x04001DC3 RID: 7619
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001DC4 RID: 7620
		private global::System.Windows.Forms.RadioButton optBoth;

		// Token: 0x04001DC5 RID: 7621
		private global::System.Windows.Forms.RadioButton optNotPreview;

		// Token: 0x04001DC6 RID: 7622
		private global::System.Windows.Forms.RadioButton optOnlyCapture;

		// Token: 0x04001DC7 RID: 7623
		private global::System.Windows.Forms.RadioButton optQuality0;

		// Token: 0x04001DC8 RID: 7624
		private global::System.Windows.Forms.RadioButton optQuality1;

		// Token: 0x04001DC9 RID: 7625
		private global::System.Windows.Forms.RadioButton optQuality2;

		// Token: 0x04001DCA RID: 7626
		private global::System.Windows.Forms.RadioButton optResolution0;

		// Token: 0x04001DCB RID: 7627
		private global::System.Windows.Forms.RadioButton optResolution1;

		// Token: 0x04001DCC RID: 7628
		private global::System.Windows.Forms.RadioButton optResolution2;

		// Token: 0x04001DCD RID: 7629
		private global::System.Windows.Forms.ToolStripMenuItem restoreAVIJPGSaveDirectoryToolStripMenuItem;

		// Token: 0x04001DCE RID: 7630
		private global::System.Windows.Forms.ToolStripMenuItem restoreAVIJPGViewDirectoryToolStripMenuItem;

		// Token: 0x04001DCF RID: 7631
		private global::System.Windows.Forms.ToolStripMenuItem setAVIJPGSaveDirectoryToolStripMenuItem;

		// Token: 0x04001DD0 RID: 7632
		private global::System.Windows.Forms.ToolStripMenuItem setAVIJPGViewDirectoryToolStripMenuItem;

		// Token: 0x04001DD1 RID: 7633
		private global::System.Windows.Forms.ToolStripMenuItem specialSetToolStripMenuItem;

		// Token: 0x04001DD2 RID: 7634
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

		// Token: 0x04001DD3 RID: 7635
		private global::System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
	}
}
