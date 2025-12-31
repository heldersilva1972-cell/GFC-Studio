namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x02000319 RID: 793
	public partial class dfrmPrivilegeTypeDoors : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001880 RID: 6272 RVA: 0x001FF1C2 File Offset: 0x001FE1C2
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

		// Token: 0x06001881 RID: 6273 RVA: 0x001FF1F8 File Offset: 0x001FE1F8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PrivilegeType.dfrmPrivilegeTypeDoors));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.mnuDoorTypeSelect = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.lblSeleted = new global::System.Windows.Forms.Label();
			this.lblOptional = new global::System.Windows.Forms.Label();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnOKAndUpload = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.progressBar2 = new global::System.Windows.Forms.ProgressBar();
			this.btnOnlyUpdateDoors = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mnuDoorTypeSelect, "mnuDoorTypeSelect");
			this.mnuDoorTypeSelect.Name = "mnuDoorTypeSelect";
			this.toolTip1.SetToolTip(this.mnuDoorTypeSelect, componentResourceManager.GetString("mnuDoorTypeSelect.ToolTip"));
			this.mnuDoorTypeSelect.ItemClicked += new global::System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuDoorTypeSelect_ItemClicked);
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.toolTip1.SetToolTip(this.progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
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
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.toolTip1.SetToolTip(this.cbof_ZoneID, componentResourceManager.GetString("cbof_ZoneID.ToolTip"));
			this.cbof_ZoneID.DropDown += new global::System.EventHandler(this.cbof_ZoneID_DropDown);
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
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
			componentResourceManager.ApplyResources(this.btnOKAndUpload, "btnOKAndUpload");
			this.btnOKAndUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOKAndUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOKAndUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnOKAndUpload.Image = global::WG3000_COMM.Properties.Resources.wg16UploadPass;
			this.btnOKAndUpload.Name = "btnOKAndUpload";
			this.toolTip1.SetToolTip(this.btnOKAndUpload, componentResourceManager.GetString("btnOKAndUpload.ToolTip"));
			this.btnOKAndUpload.UseVisualStyleBackColor = false;
			this.btnOKAndUpload.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
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
			this.dgvDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.toolTip1.SetToolTip(this.btnAddAllDoors, componentResourceManager.GetString("btnAddAllDoors.ToolTip"));
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.toolTip1.SetToolTip(this.btnDelOneDoor, componentResourceManager.GetString("btnDelOneDoor.ToolTip"));
			this.btnDelOneDoor.UseVisualStyleBackColor = false;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
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
			componentResourceManager.ApplyResources(this.progressBar2, "progressBar2");
			this.progressBar2.Name = "progressBar2";
			this.toolTip1.SetToolTip(this.progressBar2, componentResourceManager.GetString("progressBar2.ToolTip"));
			componentResourceManager.ApplyResources(this.btnOnlyUpdateDoors, "btnOnlyUpdateDoors");
			this.btnOnlyUpdateDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOnlyUpdateDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOnlyUpdateDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnOnlyUpdateDoors.Name = "btnOnlyUpdateDoors";
			this.toolTip1.SetToolTip(this.btnOnlyUpdateDoors, componentResourceManager.GetString("btnOnlyUpdateDoors.ToolTip"));
			this.btnOnlyUpdateDoors.UseVisualStyleBackColor = false;
			this.btnOnlyUpdateDoors.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.progressBar2);
			base.Controls.Add(this.lblSeleted);
			base.Controls.Add(this.cbof_ControlSegID);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.lblOptional);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cbof_ZoneID);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.dgvSelectedDoors);
			base.Controls.Add(this.btnOKAndUpload);
			base.Controls.Add(this.dgvDoors);
			base.Controls.Add(this.btnAddAllDoors);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnOnlyUpdateDoors);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnDelOneDoor);
			base.Controls.Add(this.btnDelAllDoors);
			base.Controls.Add(this.btnAddOneDoor);
			base.Name = "dfrmPrivilegeTypeDoors";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPrivilegeTypeDoors_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeTypeDoors_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003225 RID: 12837
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x04003227 RID: 12839
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003228 RID: 12840
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04003229 RID: 12841
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x0400322A RID: 12842
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x0400322B RID: 12843
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x0400322C RID: 12844
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x0400322D RID: 12845
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x0400322E RID: 12846
		private global::System.Windows.Forms.Button btnOKAndUpload;

		// Token: 0x0400322F RID: 12847
		private global::System.Windows.Forms.Button btnOnlyUpdateDoors;

		// Token: 0x04003230 RID: 12848
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x04003231 RID: 12849
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x04003232 RID: 12850
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04003233 RID: 12851
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04003234 RID: 12852
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04003235 RID: 12853
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04003236 RID: 12854
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04003237 RID: 12855
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04003238 RID: 12856
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04003239 RID: 12857
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x0400323A RID: 12858
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName;

		// Token: 0x0400323B RID: 12859
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName1;

		// Token: 0x0400323C RID: 12860
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x0400323D RID: 12861
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x0400323E RID: 12862
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x0400323F RID: 12863
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003240 RID: 12864
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04003241 RID: 12865
		private global::System.Windows.Forms.Label lblOptional;

		// Token: 0x04003242 RID: 12866
		private global::System.Windows.Forms.Label lblSeleted;

		// Token: 0x04003243 RID: 12867
		private global::System.Windows.Forms.ContextMenuStrip mnuDoorTypeSelect;

		// Token: 0x04003244 RID: 12868
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x04003245 RID: 12869
		private global::System.Windows.Forms.ProgressBar progressBar2;

		// Token: 0x04003246 RID: 12870
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;

		// Token: 0x04003247 RID: 12871
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
