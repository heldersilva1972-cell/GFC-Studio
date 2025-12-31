namespace WG3000_COMM.Basic
{
	// Token: 0x02000027 RID: 39
	public partial class dfrmPrivilegeDoorCopy : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x000554DC File Offset: 0x000544DC
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

		// Token: 0x060002D4 RID: 724 RVA: 0x00055514 File Offset: 0x00054514
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmPrivilegeDoorCopy));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnFind = new global::System.Windows.Forms.Button();
			this.statusStrip1 = new global::System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn14 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn15 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvSelectedDoors4Copy = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.f_DoorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorNo = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnAddPass = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnDeleteOneUser4Copy = new global::System.Windows.Forms.Button();
			this.btnAddOneUser4Copy = new global::System.Windows.Forms.Button();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors4Copy).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			base.SuspendLayout();
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.toolTip1.SetToolTip(this.btnFind, componentResourceManager.GetString("btnFind.ToolTip"));
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.statusStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.statusStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabel1, this.toolStripStatusLabel2 });
			this.statusStrip1.Name = "statusStrip1";
			this.toolTip1.SetToolTip(this.statusStrip1, componentResourceManager.GetString("statusStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripStatusLabel1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel1.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			componentResourceManager.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
			this.toolStripStatusLabel2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Spring = true;
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.toolTip1.SetToolTip(this.cbof_ZoneID, componentResourceManager.GetString("cbof_ZoneID.ToolTip"));
			this.cbof_ZoneID.DropDown += new global::System.EventHandler(this.cbof_ZoneID_DropDown);
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_ZoneID_SelectedIndexChanged);
			this.cbof_ZoneID.Enter += new global::System.EventHandler(this.dgvDoors_Enter);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			this.toolTip1.SetToolTip(this.label25, componentResourceManager.GetString("label25.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn14, this.dataGridViewTextBoxColumn15, this.dataGridViewTextBoxColumn16, this.dataGridViewTextBoxColumn17 });
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			this.dgvSelectedDoors.RowTemplate.Height = 23;
			this.dgvSelectedDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedDoors, componentResourceManager.GetString("dgvSelectedDoors.ToolTip"));
			this.dgvSelectedDoors.Enter += new global::System.EventHandler(this.dgvDoors_Enter);
			this.dgvSelectedDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.ReadOnly = true;
			this.dataGridViewTextBoxColumn17.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvSelectedDoors4Copy, "dgvSelectedDoors4Copy");
			this.dgvSelectedDoors4Copy.AllowUserToAddRows = false;
			this.dgvSelectedDoors4Copy.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors4Copy.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors4Copy.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedDoors4Copy.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors4Copy.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11, this.dataGridViewTextBoxColumn12, this.dataGridViewTextBoxColumn13 });
			this.dgvSelectedDoors4Copy.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors4Copy.Name = "dgvSelectedDoors4Copy";
			this.dgvSelectedDoors4Copy.ReadOnly = true;
			this.dgvSelectedDoors4Copy.RowTemplate.Height = 23;
			this.dgvSelectedDoors4Copy.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedDoors4Copy, componentResourceManager.GetString("dgvSelectedDoors4Copy.ToolTip"));
			this.dgvSelectedDoors4Copy.Enter += new global::System.EventHandler(this.dgvDoors_Enter);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			this.dataGridViewTextBoxColumn13.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_DoorID, this.f_ControllerSN, this.f_DoorNo, this.f_DoorName });
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			this.dgvDoors.RowTemplate.Height = 23;
			this.dgvDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvDoors, componentResourceManager.GetString("dgvDoors.ToolTip"));
			this.dgvDoors.Enter += new global::System.EventHandler(this.dgvDoors_Enter);
			this.dgvDoors.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvDoors_KeyDown);
			this.dgvDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.f_DoorID, "f_DoorID");
			this.f_DoorID.Name = "f_DoorID";
			this.f_DoorID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorNo, "f_DoorNo");
			this.f_DoorNo.Name = "f_DoorNo";
			this.f_DoorNo.ReadOnly = true;
			this.f_DoorName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.toolTip1.SetToolTip(this.progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnAddPass, "btnAddPass");
			this.btnAddPass.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddPass.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddPass.ForeColor = global::System.Drawing.Color.White;
			this.btnAddPass.Image = global::WG3000_COMM.Properties.Resources.Rec1Pass;
			this.btnAddPass.Name = "btnAddPass";
			this.toolTip1.SetToolTip(this.btnAddPass, componentResourceManager.GetString("btnAddPass.ToolTip"));
			this.btnAddPass.UseVisualStyleBackColor = false;
			this.btnAddPass.Click += new global::System.EventHandler(this.btnAddPass_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnDeleteOneUser4Copy, "btnDeleteOneUser4Copy");
			this.btnDeleteOneUser4Copy.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOneUser4Copy.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOneUser4Copy.Name = "btnDeleteOneUser4Copy";
			this.toolTip1.SetToolTip(this.btnDeleteOneUser4Copy, componentResourceManager.GetString("btnDeleteOneUser4Copy.ToolTip"));
			this.btnDeleteOneUser4Copy.UseVisualStyleBackColor = true;
			this.btnDeleteOneUser4Copy.Click += new global::System.EventHandler(this.btnDeleteOneUser4Copy_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser4Copy, "btnAddOneUser4Copy");
			this.btnAddOneUser4Copy.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser4Copy.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneUser4Copy.Name = "btnAddOneUser4Copy";
			this.toolTip1.SetToolTip(this.btnAddOneUser4Copy, componentResourceManager.GetString("btnAddOneUser4Copy.ToolTip"));
			this.btnAddOneUser4Copy.UseVisualStyleBackColor = true;
			this.btnAddOneUser4Copy.Click += new global::System.EventHandler(this.btnAddOneUser4Copy_Click);
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.ForeColor = global::System.Drawing.Color.White;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
			this.btnDelAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllUsers.Name = "btnDelAllUsers";
			this.toolTip1.SetToolTip(this.btnDelAllUsers, componentResourceManager.GetString("btnDelAllUsers.ToolTip"));
			this.btnDelAllUsers.UseVisualStyleBackColor = true;
			this.btnDelAllUsers.Click += new global::System.EventHandler(this.btnDelAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
			this.btnDelOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneUser.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneUser.Name = "btnDelOneUser";
			this.toolTip1.SetToolTip(this.btnDelOneUser, componentResourceManager.GetString("btnDelOneUser.ToolTip"));
			this.btnDelOneUser.UseVisualStyleBackColor = true;
			this.btnDelOneUser.Click += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
			this.btnAddOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneUser.Name = "btnAddOneUser";
			this.toolTip1.SetToolTip(this.btnAddOneUser, componentResourceManager.GetString("btnAddOneUser.ToolTip"));
			this.btnAddOneUser.UseVisualStyleBackColor = true;
			this.btnAddOneUser.Click += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.toolTip1.SetToolTip(this.button1, componentResourceManager.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
			this.btnAddAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllUsers.Name = "btnAddAllUsers";
			this.toolTip1.SetToolTip(this.btnAddAllUsers, componentResourceManager.GetString("btnAddAllUsers.ToolTip"));
			this.btnAddAllUsers.UseVisualStyleBackColor = true;
			this.btnAddAllUsers.Click += new global::System.EventHandler(this.btnAddAllUsers_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.cbof_ZoneID);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.dgvSelectedDoors);
			base.Controls.Add(this.dgvSelectedDoors4Copy);
			base.Controls.Add(this.dgvDoors);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.btnAddPass);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnDeleteOneUser4Copy);
			base.Controls.Add(this.btnAddOneUser4Copy);
			base.Controls.Add(this.lblWait);
			base.Controls.Add(this.btnDelAllUsers);
			base.Controls.Add(this.btnDelOneUser);
			base.Controls.Add(this.btnAddOneUser);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.btnAddAllUsers);
			base.Name = "dfrmPrivilegeDoorCopy";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeCopy_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPrivilegeDoorCopy_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeCopy_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPrivilegeCopy_KeyDown);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors4Copy).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400056A RID: 1386
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x0400056C RID: 1388
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400056D RID: 1389
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x0400056E RID: 1390
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x0400056F RID: 1391
		private global::System.Windows.Forms.Button btnAddOneUser4Copy;

		// Token: 0x04000570 RID: 1392
		private global::System.Windows.Forms.Button btnAddPass;

		// Token: 0x04000571 RID: 1393
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04000572 RID: 1394
		private global::System.Windows.Forms.Button btnDeleteOneUser4Copy;

		// Token: 0x04000573 RID: 1395
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04000574 RID: 1396
		private global::System.Windows.Forms.Button btnFind;

		// Token: 0x04000575 RID: 1397
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000576 RID: 1398
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x04000577 RID: 1399
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		// Token: 0x04000578 RID: 1400
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		// Token: 0x04000579 RID: 1401
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

		// Token: 0x0400057A RID: 1402
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

		// Token: 0x0400057B RID: 1403
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

		// Token: 0x0400057C RID: 1404
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

		// Token: 0x0400057D RID: 1405
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

		// Token: 0x0400057E RID: 1406
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

		// Token: 0x0400057F RID: 1407
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04000580 RID: 1408
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x04000581 RID: 1409
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors4Copy;

		// Token: 0x04000582 RID: 1410
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04000583 RID: 1411
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorID;

		// Token: 0x04000584 RID: 1412
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x04000585 RID: 1413
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNo;

		// Token: 0x04000586 RID: 1414
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000587 RID: 1415
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000588 RID: 1416
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04000589 RID: 1417
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x0400058A RID: 1418
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400058B RID: 1419
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x0400058C RID: 1420
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400058D RID: 1421
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

		// Token: 0x0400058E RID: 1422
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

		// Token: 0x0400058F RID: 1423
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
