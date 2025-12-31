namespace WG3000_COMM.Basic
{
	// Token: 0x02000028 RID: 40
	public partial class dfrmPrivilegeSingle : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x000595EA File Offset: 0x000585EA
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

		// Token: 0x060002F7 RID: 759 RVA: 0x00059620 File Offset: 0x00058620
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmPrivilegeSingle));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.mnuDoorTypeSelect = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnFind = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.lblSeleted = new global::System.Windows.Forms.Label();
			this.lblOptional = new global::System.Windows.Forms.Label();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.btnOKAndUpload = new global::System.Windows.Forms.Button();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnLimitReset = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mnuDoorTypeSelect, "mnuDoorTypeSelect");
			this.mnuDoorTypeSelect.Name = "mnuDoorTypeSelect";
			this.toolTip1.SetToolTip(this.mnuDoorTypeSelect, componentResourceManager.GetString("mnuDoorTypeSelect.ToolTip"));
			this.mnuDoorTypeSelect.ItemClicked += new global::System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuDoorTypeSelect_ItemClicked);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.toolTip1.SetToolTip(this.btnFind, componentResourceManager.GetString("btnFind.ToolTip"));
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.lblSeleted, "lblSeleted");
			this.lblSeleted.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSeleted.ForeColor = global::System.Drawing.Color.White;
			this.lblSeleted.Name = "lblSeleted";
			this.toolTip1.SetToolTip(this.lblSeleted, componentResourceManager.GetString("lblSeleted.ToolTip"));
			componentResourceManager.ApplyResources(this.lblOptional, "lblOptional");
			this.lblOptional.BackColor = global::System.Drawing.Color.Transparent;
			this.lblOptional.ForeColor = global::System.Drawing.Color.White;
			this.lblOptional.Name = "lblOptional";
			this.toolTip1.SetToolTip(this.lblOptional, componentResourceManager.GetString("lblOptional.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
			this.cbof_ControlSegID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegID.FormattingEnabled = true;
			this.cbof_ControlSegID.Name = "cbof_ControlSegID";
			this.toolTip1.SetToolTip(this.cbof_ControlSegID, componentResourceManager.GetString("cbof_ControlSegID.ToolTip"));
			this.cbof_ControlSegID.Enter += new global::System.EventHandler(this.cbof_ZoneID_Enter);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.toolTip1.SetToolTip(this.cbof_ZoneID, componentResourceManager.GetString("cbof_ZoneID.ToolTip"));
			this.cbof_ZoneID.DropDown += new global::System.EventHandler(this.cbof_ZoneID_DropDown);
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			this.cbof_ZoneID.Enter += new global::System.EventHandler(this.cbof_ZoneID_Enter);
			componentResourceManager.ApplyResources(this.btnOKAndUpload, "btnOKAndUpload");
			this.btnOKAndUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOKAndUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOKAndUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnOKAndUpload.Image = global::WG3000_COMM.Properties.Resources.wg16UploadPass;
			this.btnOKAndUpload.Name = "btnOKAndUpload";
			this.toolTip1.SetToolTip(this.btnOKAndUpload, componentResourceManager.GetString("btnOKAndUpload.ToolTip"));
			this.btnOKAndUpload.UseVisualStyleBackColor = false;
			this.btnOKAndUpload.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			this.toolTip1.SetToolTip(this.label25, componentResourceManager.GetString("label25.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.AllowUserToOrderColumns = true;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.Column1, this.TimeProfile, this.f_ControlSegName });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSelectedDoors.RowTemplate.Height = 23;
			this.dgvSelectedDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedDoors, componentResourceManager.GetString("dgvSelectedDoors.ToolTip"));
			this.dgvSelectedDoors.Enter += new global::System.EventHandler(this.cbof_ZoneID_Enter);
			this.dgvSelectedDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvSelectedDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected2, "f_Selected2");
			this.f_Selected2.Name = "f_Selected2";
			this.f_Selected2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TimeProfile, "TimeProfile");
			this.TimeProfile.Name = "TimeProfile";
			this.TimeProfile.ReadOnly = true;
			this.f_ControlSegName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_ControlSegName, "f_ControlSegName");
			this.f_ControlSegName.Name = "f_ControlSegName";
			this.f_ControlSegName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.AllowUserToOrderColumns = true;
			this.dgvDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.f_ZoneID, this.Column2, this.f_ControlSegName1 });
			this.dgvDoors.ContextMenuStrip = this.mnuDoorTypeSelect;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvDoors.RowTemplate.Height = 23;
			this.dgvDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvDoors, componentResourceManager.GetString("dgvDoors.ToolTip"));
			this.dgvDoors.Enter += new global::System.EventHandler(this.cbof_ZoneID_Enter);
			this.dgvDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected, "f_Selected");
			this.f_Selected.Name = "f_Selected";
			this.f_Selected.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ZoneID, "f_ZoneID");
			this.f_ZoneID.Name = "f_ZoneID";
			this.f_ZoneID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlSegName1, "f_ControlSegName1");
			this.f_ControlSegName1.Name = "f_ControlSegName1";
			this.f_ControlSegName1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.toolTip1.SetToolTip(this.btnAddAllDoors, componentResourceManager.GetString("btnAddAllDoors.ToolTip"));
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
			this.btnDelAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllDoors.Name = "btnDelAllDoors";
			this.toolTip1.SetToolTip(this.btnDelAllDoors, componentResourceManager.GetString("btnDelAllDoors.ToolTip"));
			this.btnDelAllDoors.UseVisualStyleBackColor = false;
			this.btnDelAllDoors.Click += new global::System.EventHandler(this.btnDelAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
			this.btnAddOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneDoor.Name = "btnAddOneDoor";
			this.toolTip1.SetToolTip(this.btnAddOneDoor, componentResourceManager.GetString("btnAddOneDoor.ToolTip"));
			this.btnAddOneDoor.UseVisualStyleBackColor = false;
			this.btnAddOneDoor.Click += new global::System.EventHandler(this.btnAddOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.toolTip1.SetToolTip(this.btnDelOneDoor, componentResourceManager.GetString("btnDelOneDoor.ToolTip"));
			this.btnDelOneDoor.UseVisualStyleBackColor = false;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnLimitReset, "btnLimitReset");
			this.btnLimitReset.BackColor = global::System.Drawing.Color.Transparent;
			this.btnLimitReset.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnLimitReset.ForeColor = global::System.Drawing.Color.White;
			this.btnLimitReset.Image = global::WG3000_COMM.Properties.Resources.wg16UploadPass;
			this.btnLimitReset.Name = "btnLimitReset";
			this.toolTip1.SetToolTip(this.btnLimitReset, componentResourceManager.GetString("btnLimitReset.ToolTip"));
			this.btnLimitReset.UseVisualStyleBackColor = false;
			this.btnLimitReset.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnLimitReset);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lblSeleted);
			base.Controls.Add(this.lblOptional);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.cbof_ControlSegID);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.cbof_ZoneID);
			base.Controls.Add(this.btnOKAndUpload);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.dgvSelectedDoors);
			base.Controls.Add(this.dgvDoors);
			base.Controls.Add(this.btnAddAllDoors);
			base.Controls.Add(this.btnDelOneDoor);
			base.Controls.Add(this.btnDelAllDoors);
			base.Controls.Add(this.btnAddOneDoor);
			base.Name = "dfrmPrivilegeSingle";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeSingle_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPrivilegeSingle_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeSingle_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPrivilegeSingle_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040005A8 RID: 1448
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x040005AC RID: 1452
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040005AD RID: 1453
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x040005AE RID: 1454
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x040005AF RID: 1455
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x040005B0 RID: 1456
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x040005B1 RID: 1457
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x040005B2 RID: 1458
		private global::System.Windows.Forms.Button btnFind;

		// Token: 0x040005B3 RID: 1459
		private global::System.Windows.Forms.Button btnLimitReset;

		// Token: 0x040005B4 RID: 1460
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040005B5 RID: 1461
		private global::System.Windows.Forms.Button btnOKAndUpload;

		// Token: 0x040005B6 RID: 1462
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x040005B7 RID: 1463
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x040005B8 RID: 1464
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x040005B9 RID: 1465
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x040005BA RID: 1466
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x040005BB RID: 1467
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x040005BC RID: 1468
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x040005BD RID: 1469
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x040005BE RID: 1470
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x040005BF RID: 1471
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x040005C0 RID: 1472
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName;

		// Token: 0x040005C1 RID: 1473
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName1;

		// Token: 0x040005C2 RID: 1474
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x040005C3 RID: 1475
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x040005C4 RID: 1476
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x040005C5 RID: 1477
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040005C6 RID: 1478
		private global::System.Windows.Forms.Label label25;

		// Token: 0x040005C7 RID: 1479
		private global::System.Windows.Forms.Label lblOptional;

		// Token: 0x040005C8 RID: 1480
		private global::System.Windows.Forms.Label lblSeleted;

		// Token: 0x040005C9 RID: 1481
		private global::System.Windows.Forms.ContextMenuStrip mnuDoorTypeSelect;

		// Token: 0x040005CA RID: 1482
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;

		// Token: 0x040005CB RID: 1483
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
