namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x02000248 RID: 584
	public partial class dfrmElevatorGroup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x00160C72 File Offset: 0x0015FC72
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00160C94 File Offset: 0x0015FC94
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Elevator.dfrmElevatorGroup));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.lblOptional = new global::System.Windows.Forms.Label();
			this.lblSeleted = new global::System.Windows.Forms.Label();
			this.nudGroupToAdd = new global::System.Windows.Forms.NumericUpDown();
			this.lblControlTimeSeg = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGroupToAdd).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Image = global::WG3000_COMM.Properties.Resources.Rec1Pass;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
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
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.f_Selected2, this.dataGridViewTextBoxColumn9, this.Column1, this.TimeProfile, this.f_ControlSegName });
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
			componentResourceManager.ApplyResources(this.f_Selected2, "f_Selected2");
			this.f_Selected2.Name = "f_Selected2";
			this.f_Selected2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TimeProfile, "TimeProfile");
			this.TimeProfile.Name = "TimeProfile";
			this.TimeProfile.ReadOnly = true;
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
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.f_Selected, this.dataGridViewTextBoxColumn7, this.f_ZoneID, this.Column2, this.f_ControlSegName1 });
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
			componentResourceManager.ApplyResources(this.f_Selected, "f_Selected");
			this.f_Selected.Name = "f_Selected";
			this.f_Selected.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ZoneID, "f_ZoneID");
			this.f_ZoneID.Name = "f_ZoneID";
			this.f_ZoneID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControlSegName1, "f_ControlSegName1");
			this.f_ControlSegName1.Name = "f_ControlSegName1";
			this.f_ControlSegName1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
			this.btnDelAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllDoors.Name = "btnDelAllDoors";
			this.toolTip1.SetToolTip(this.btnDelAllDoors, componentResourceManager.GetString("btnDelAllDoors.ToolTip"));
			this.btnDelAllDoors.UseVisualStyleBackColor = false;
			this.btnDelAllDoors.Click += new global::System.EventHandler(this.btnDelAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.toolTip1.SetToolTip(this.btnDelOneDoor, componentResourceManager.GetString("btnDelOneDoor.ToolTip"));
			this.btnDelOneDoor.UseVisualStyleBackColor = false;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
			this.btnAddOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneDoor.Name = "btnAddOneDoor";
			this.toolTip1.SetToolTip(this.btnAddOneDoor, componentResourceManager.GetString("btnAddOneDoor.ToolTip"));
			this.btnAddOneDoor.UseVisualStyleBackColor = false;
			this.btnAddOneDoor.Click += new global::System.EventHandler(this.btnAddOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.toolTip1.SetToolTip(this.btnAddAllDoors, componentResourceManager.GetString("btnAddAllDoors.ToolTip"));
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.lblOptional, "lblOptional");
			this.lblOptional.BackColor = global::System.Drawing.Color.Transparent;
			this.lblOptional.ForeColor = global::System.Drawing.Color.White;
			this.lblOptional.Name = "lblOptional";
			this.toolTip1.SetToolTip(this.lblOptional, componentResourceManager.GetString("lblOptional.ToolTip"));
			componentResourceManager.ApplyResources(this.lblSeleted, "lblSeleted");
			this.lblSeleted.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSeleted.ForeColor = global::System.Drawing.Color.White;
			this.lblSeleted.Name = "lblSeleted";
			this.toolTip1.SetToolTip(this.lblSeleted, componentResourceManager.GetString("lblSeleted.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGroupToAdd, "nudGroupToAdd");
			this.nudGroupToAdd.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudGroupToAdd;
			int[] array = new int[4];
			array[0] = 10000;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudGroupToAdd;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudGroupToAdd.Name = "nudGroupToAdd";
			this.nudGroupToAdd.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGroupToAdd, componentResourceManager.GetString("nudGroupToAdd.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudGroupToAdd;
			int[] array3 = new int[4];
			array3[0] = 1;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.lblControlTimeSeg, "lblControlTimeSeg");
			this.lblControlTimeSeg.ForeColor = global::System.Drawing.Color.White;
			this.lblControlTimeSeg.Name = "lblControlTimeSeg";
			this.toolTip1.SetToolTip(this.lblControlTimeSeg, componentResourceManager.GetString("lblControlTimeSeg.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label1);
			base.Controls.Add(this.nudGroupToAdd);
			base.Controls.Add(this.lblControlTimeSeg);
			base.Controls.Add(this.lblSeleted);
			base.Controls.Add(this.lblOptional);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.cbof_ZoneID);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.dgvSelectedDoors);
			base.Controls.Add(this.dgvDoors);
			base.Controls.Add(this.btnAddAllDoors);
			base.Controls.Add(this.btnDelAllDoors);
			base.Controls.Add(this.btnAddOneDoor);
			base.Controls.Add(this.btnDelOneDoor);
			base.Name = "dfrmElevatorGroup";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeSingle_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGroupToAdd).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002180 RID: 8576
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002181 RID: 8577
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04002182 RID: 8578
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x04002183 RID: 8579
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x04002184 RID: 8580
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x04002185 RID: 8581
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002186 RID: 8582
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002187 RID: 8583
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x04002188 RID: 8584
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002189 RID: 8585
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x0400218A RID: 8586
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x0400218B RID: 8587
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x0400218C RID: 8588
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x0400218D RID: 8589
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x0400218E RID: 8590
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x0400218F RID: 8591
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x04002190 RID: 8592
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName;

		// Token: 0x04002191 RID: 8593
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName1;

		// Token: 0x04002192 RID: 8594
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002193 RID: 8595
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x04002194 RID: 8596
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x04002195 RID: 8597
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04002196 RID: 8598
		private global::System.Windows.Forms.Label lblOptional;

		// Token: 0x04002197 RID: 8599
		private global::System.Windows.Forms.Label lblSeleted;

		// Token: 0x04002198 RID: 8600
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;

		// Token: 0x04002199 RID: 8601
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400219A RID: 8602
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x0400219B RID: 8603
		internal global::System.Windows.Forms.Label lblControlTimeSeg;

		// Token: 0x0400219C RID: 8604
		internal global::System.Windows.Forms.NumericUpDown nudGroupToAdd;
	}
}
