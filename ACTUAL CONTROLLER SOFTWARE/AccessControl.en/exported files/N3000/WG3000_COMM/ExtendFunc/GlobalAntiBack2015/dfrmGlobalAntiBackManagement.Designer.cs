namespace WG3000_COMM.ExtendFunc.GlobalAntiBack2015
{
	// Token: 0x020002F0 RID: 752
	public partial class dfrmGlobalAntiBackManagement : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060015CA RID: 5578 RVA: 0x001B545A File Offset: 0x001B445A
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

		// Token: 0x060015CB RID: 5579 RVA: 0x001B5490 File Offset: 0x001B4490
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.GlobalAntiBack2015.dfrmGlobalAntiBackManagement));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.chkFirstInThenOut = new global::System.Windows.Forms.CheckBox();
			this.btnRestartAntiBack = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.dgvDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			this.lblSeleted = new global::System.Windows.Forms.Label();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.cbof_ZoneID = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.dgvSelectedDoors = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControlSegName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblOptional = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.nudTotal = new global::System.Windows.Forms.NumericUpDown();
			this.chkActiveAntibackShare = new global::System.Windows.Forms.CheckBox();
			this.chkActiveTimeSegments = new global::System.Windows.Forms.CheckBox();
			this.groupBoxOut = new global::System.Windows.Forms.GroupBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.dateBeginHMSExit6 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMSExit6 = new global::System.Windows.Forms.DateTimePicker();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label10 = new global::System.Windows.Forms.Label();
			this.dateBeginHMSExit5 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMSExit5 = new global::System.Windows.Forms.DateTimePicker();
			this.label11 = new global::System.Windows.Forms.Label();
			this.label12 = new global::System.Windows.Forms.Label();
			this.dateEndHMSExit4 = new global::System.Windows.Forms.DateTimePicker();
			this.dateBeginHMSExit4 = new global::System.Windows.Forms.DateTimePicker();
			this.label13 = new global::System.Windows.Forms.Label();
			this.label14 = new global::System.Windows.Forms.Label();
			this.dateBeginHMSExit3 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMSExit3 = new global::System.Windows.Forms.DateTimePicker();
			this.label15 = new global::System.Windows.Forms.Label();
			this.label16 = new global::System.Windows.Forms.Label();
			this.dateBeginHMSExit2 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMSExit2 = new global::System.Windows.Forms.DateTimePicker();
			this.label17 = new global::System.Windows.Forms.Label();
			this.label18 = new global::System.Windows.Forms.Label();
			this.dateEndHMSExit1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateBeginHMSExit1 = new global::System.Windows.Forms.DateTimePicker();
			this.groupBoxIn = new global::System.Windows.Forms.GroupBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS6 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS6 = new global::System.Windows.Forms.DateTimePicker();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS5 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS5 = new global::System.Windows.Forms.DateTimePicker();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.dateEndHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.dateBeginHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.label89 = new global::System.Windows.Forms.Label();
			this.label90 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.label87 = new global::System.Windows.Forms.Label();
			this.label88 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.label86 = new global::System.Windows.Forms.Label();
			this.label85 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).BeginInit();
			this.groupBoxOut.SuspendLayout();
			this.groupBoxIn.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkFirstInThenOut, "chkFirstInThenOut");
			this.chkFirstInThenOut.ForeColor = global::System.Drawing.Color.White;
			this.chkFirstInThenOut.Name = "chkFirstInThenOut";
			this.chkFirstInThenOut.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnRestartAntiBack, "btnRestartAntiBack");
			this.btnRestartAntiBack.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRestartAntiBack.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRestartAntiBack.ForeColor = global::System.Drawing.Color.White;
			this.btnRestartAntiBack.Name = "btnRestartAntiBack";
			this.btnRestartAntiBack.UseVisualStyleBackColor = false;
			this.btnRestartAntiBack.Click += new global::System.EventHandler(this.btnRestartAntiBack_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.dgvDoors);
			this.groupBox1.Controls.Add(this.btnAddAllDoors);
			this.groupBox1.Controls.Add(this.btnAddOneDoor);
			this.groupBox1.Controls.Add(this.lblSeleted);
			this.groupBox1.Controls.Add(this.btnDelAllDoors);
			this.groupBox1.Controls.Add(this.btnDelOneDoor);
			this.groupBox1.Controls.Add(this.cbof_ZoneID);
			this.groupBox1.Controls.Add(this.label25);
			this.groupBox1.Controls.Add(this.dgvSelectedDoors);
			this.groupBox1.Controls.Add(this.lblOptional);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.dgvDoors, "dgvDoors");
			this.dgvDoors.AllowUserToAddRows = false;
			this.dgvDoors.AllowUserToDeleteRows = false;
			this.dgvDoors.AllowUserToOrderColumns = true;
			this.dgvDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.f_ZoneID, this.Column2, this.f_ControlSegName1 });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDoors.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvDoors.EnableHeadersVisualStyles = false;
			this.dgvDoors.Name = "dgvDoors";
			this.dgvDoors.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvDoors.RowTemplate.Height = 23;
			this.dgvDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
			this.btnAddOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneDoor.Name = "btnAddOneDoor";
			this.btnAddOneDoor.UseVisualStyleBackColor = false;
			this.btnAddOneDoor.Click += new global::System.EventHandler(this.btnAddOneDoor_Click);
			componentResourceManager.ApplyResources(this.lblSeleted, "lblSeleted");
			this.lblSeleted.BackColor = global::System.Drawing.Color.Transparent;
			this.lblSeleted.ForeColor = global::System.Drawing.Color.White;
			this.lblSeleted.Name = "lblSeleted";
			componentResourceManager.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
			this.btnDelAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllDoors.Name = "btnDelAllDoors";
			this.btnDelAllDoors.UseVisualStyleBackColor = false;
			this.btnDelAllDoors.Click += new global::System.EventHandler(this.btnDelAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.btnDelOneDoor.UseVisualStyleBackColor = false;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
			componentResourceManager.ApplyResources(this.cbof_ZoneID, "cbof_ZoneID");
			this.cbof_ZoneID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ZoneID.FormattingEnabled = true;
			this.cbof_ZoneID.Name = "cbof_ZoneID";
			this.cbof_ZoneID.DropDown += new global::System.EventHandler(this.cbof_ZoneID_DropDown);
			this.cbof_ZoneID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this.dgvSelectedDoors, "dgvSelectedDoors");
			this.dgvSelectedDoors.AllowUserToAddRows = false;
			this.dgvSelectedDoors.AllowUserToDeleteRows = false;
			this.dgvSelectedDoors.AllowUserToOrderColumns = true;
			this.dgvSelectedDoors.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvSelectedDoors.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedDoors.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.Column1, this.TimeProfile, this.f_ControlSegName });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedDoors.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvSelectedDoors.EnableHeadersVisualStyles = false;
			this.dgvSelectedDoors.Name = "dgvSelectedDoors";
			this.dgvSelectedDoors.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedDoors.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvSelectedDoors.RowTemplate.Height = 23;
			this.dgvSelectedDoors.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
			componentResourceManager.ApplyResources(this.lblOptional, "lblOptional");
			this.lblOptional.BackColor = global::System.Drawing.Color.Transparent;
			this.lblOptional.ForeColor = global::System.Drawing.Color.White;
			this.lblOptional.Name = "lblOptional";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Name = "tabPage1";
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.nudTotal);
			this.tabPage2.Controls.Add(this.chkActiveAntibackShare);
			this.tabPage2.Controls.Add(this.chkActiveTimeSegments);
			this.tabPage2.Controls.Add(this.groupBoxOut);
			this.tabPage2.Controls.Add(this.groupBoxIn);
			this.tabPage2.Name = "tabPage2";
			componentResourceManager.ApplyResources(this.nudTotal, "nudTotal");
			this.nudTotal.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudTotal;
			int[] array = new int[4];
			array[0] = 1000;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudTotal;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudTotal.Name = "nudTotal";
			this.nudTotal.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudTotal;
			int[] array3 = new int[4];
			array3[0] = 2;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkActiveAntibackShare, "chkActiveAntibackShare");
			this.chkActiveAntibackShare.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveAntibackShare.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveAntibackShare.Name = "chkActiveAntibackShare";
			this.chkActiveAntibackShare.UseVisualStyleBackColor = false;
			this.chkActiveAntibackShare.CheckedChanged += new global::System.EventHandler(this.chkActiveAntibackShare_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActiveTimeSegments, "chkActiveTimeSegments");
			this.chkActiveTimeSegments.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveTimeSegments.Name = "chkActiveTimeSegments";
			this.chkActiveTimeSegments.UseVisualStyleBackColor = true;
			this.chkActiveTimeSegments.CheckedChanged += new global::System.EventHandler(this.chkActiveTimeSegments_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBoxOut, "groupBoxOut");
			this.groupBoxOut.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBoxOut.Controls.Add(this.label7);
			this.groupBoxOut.Controls.Add(this.label8);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit6);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit6);
			this.groupBoxOut.Controls.Add(this.label9);
			this.groupBoxOut.Controls.Add(this.label10);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit5);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit5);
			this.groupBoxOut.Controls.Add(this.label11);
			this.groupBoxOut.Controls.Add(this.label12);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit4);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit4);
			this.groupBoxOut.Controls.Add(this.label13);
			this.groupBoxOut.Controls.Add(this.label14);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit3);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit3);
			this.groupBoxOut.Controls.Add(this.label15);
			this.groupBoxOut.Controls.Add(this.label16);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit2);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit2);
			this.groupBoxOut.Controls.Add(this.label17);
			this.groupBoxOut.Controls.Add(this.label18);
			this.groupBoxOut.Controls.Add(this.dateEndHMSExit1);
			this.groupBoxOut.Controls.Add(this.dateBeginHMSExit1);
			this.groupBoxOut.ForeColor = global::System.Drawing.Color.White;
			this.groupBoxOut.Name = "groupBoxOut";
			this.groupBoxOut.TabStop = false;
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.dateBeginHMSExit6, "dateBeginHMSExit6");
			this.dateBeginHMSExit6.Name = "dateBeginHMSExit6";
			this.dateBeginHMSExit6.ShowUpDown = true;
			this.dateBeginHMSExit6.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMSExit6, "dateEndHMSExit6");
			this.dateEndHMSExit6.Name = "dateEndHMSExit6";
			this.dateEndHMSExit6.ShowUpDown = true;
			this.dateEndHMSExit6.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.dateBeginHMSExit5, "dateBeginHMSExit5");
			this.dateBeginHMSExit5.Name = "dateBeginHMSExit5";
			this.dateBeginHMSExit5.ShowUpDown = true;
			this.dateBeginHMSExit5.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMSExit5, "dateEndHMSExit5");
			this.dateEndHMSExit5.Name = "dateEndHMSExit5";
			this.dateEndHMSExit5.ShowUpDown = true;
			this.dateEndHMSExit5.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.dateEndHMSExit4, "dateEndHMSExit4");
			this.dateEndHMSExit4.Name = "dateEndHMSExit4";
			this.dateEndHMSExit4.ShowUpDown = true;
			this.dateEndHMSExit4.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dateBeginHMSExit4, "dateBeginHMSExit4");
			this.dateBeginHMSExit4.Name = "dateBeginHMSExit4";
			this.dateBeginHMSExit4.ShowUpDown = true;
			this.dateBeginHMSExit4.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.dateBeginHMSExit3, "dateBeginHMSExit3");
			this.dateBeginHMSExit3.Name = "dateBeginHMSExit3";
			this.dateBeginHMSExit3.ShowUpDown = true;
			this.dateBeginHMSExit3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMSExit3, "dateEndHMSExit3");
			this.dateEndHMSExit3.Name = "dateEndHMSExit3";
			this.dateEndHMSExit3.ShowUpDown = true;
			this.dateEndHMSExit3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label15, "label15");
			this.label15.Name = "label15";
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.Name = "label16";
			componentResourceManager.ApplyResources(this.dateBeginHMSExit2, "dateBeginHMSExit2");
			this.dateBeginHMSExit2.Name = "dateBeginHMSExit2";
			this.dateBeginHMSExit2.ShowUpDown = true;
			this.dateBeginHMSExit2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMSExit2, "dateEndHMSExit2");
			this.dateEndHMSExit2.Name = "dateEndHMSExit2";
			this.dateEndHMSExit2.ShowUpDown = true;
			this.dateEndHMSExit2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label17, "label17");
			this.label17.Name = "label17";
			componentResourceManager.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			componentResourceManager.ApplyResources(this.dateEndHMSExit1, "dateEndHMSExit1");
			this.dateEndHMSExit1.Name = "dateEndHMSExit1";
			this.dateEndHMSExit1.ShowUpDown = true;
			this.dateEndHMSExit1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dateBeginHMSExit1, "dateBeginHMSExit1");
			this.dateBeginHMSExit1.Name = "dateBeginHMSExit1";
			this.dateBeginHMSExit1.ShowUpDown = true;
			this.dateBeginHMSExit1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.groupBoxIn, "groupBoxIn");
			this.groupBoxIn.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBoxIn.Controls.Add(this.label1);
			this.groupBoxIn.Controls.Add(this.label2);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS6);
			this.groupBoxIn.Controls.Add(this.dateEndHMS6);
			this.groupBoxIn.Controls.Add(this.label3);
			this.groupBoxIn.Controls.Add(this.label4);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS5);
			this.groupBoxIn.Controls.Add(this.dateEndHMS5);
			this.groupBoxIn.Controls.Add(this.label5);
			this.groupBoxIn.Controls.Add(this.label6);
			this.groupBoxIn.Controls.Add(this.dateEndHMS4);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS4);
			this.groupBoxIn.Controls.Add(this.label89);
			this.groupBoxIn.Controls.Add(this.label90);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS3);
			this.groupBoxIn.Controls.Add(this.dateEndHMS3);
			this.groupBoxIn.Controls.Add(this.label87);
			this.groupBoxIn.Controls.Add(this.label88);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS2);
			this.groupBoxIn.Controls.Add(this.dateEndHMS2);
			this.groupBoxIn.Controls.Add(this.label86);
			this.groupBoxIn.Controls.Add(this.label85);
			this.groupBoxIn.Controls.Add(this.dateEndHMS1);
			this.groupBoxIn.Controls.Add(this.dateBeginHMS1);
			this.groupBoxIn.ForeColor = global::System.Drawing.Color.White;
			this.groupBoxIn.Name = "groupBoxIn";
			this.groupBoxIn.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.dateBeginHMS6, "dateBeginHMS6");
			this.dateBeginHMS6.Name = "dateBeginHMS6";
			this.dateBeginHMS6.ShowUpDown = true;
			this.dateBeginHMS6.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS6, "dateEndHMS6");
			this.dateEndHMS6.Name = "dateEndHMS6";
			this.dateEndHMS6.ShowUpDown = true;
			this.dateEndHMS6.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.dateBeginHMS5, "dateBeginHMS5");
			this.dateBeginHMS5.Name = "dateBeginHMS5";
			this.dateBeginHMS5.ShowUpDown = true;
			this.dateBeginHMS5.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS5, "dateEndHMS5");
			this.dateEndHMS5.Name = "dateEndHMS5";
			this.dateEndHMS5.ShowUpDown = true;
			this.dateEndHMS5.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.dateEndHMS4, "dateEndHMS4");
			this.dateEndHMS4.Name = "dateEndHMS4";
			this.dateEndHMS4.ShowUpDown = true;
			this.dateEndHMS4.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dateBeginHMS4, "dateBeginHMS4");
			this.dateBeginHMS4.Name = "dateBeginHMS4";
			this.dateBeginHMS4.ShowUpDown = true;
			this.dateBeginHMS4.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label89, "label89");
			this.label89.Name = "label89";
			componentResourceManager.ApplyResources(this.label90, "label90");
			this.label90.Name = "label90";
			componentResourceManager.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
			this.dateBeginHMS3.Name = "dateBeginHMS3";
			this.dateBeginHMS3.ShowUpDown = true;
			this.dateBeginHMS3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
			this.dateEndHMS3.Name = "dateEndHMS3";
			this.dateEndHMS3.ShowUpDown = true;
			this.dateEndHMS3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label87, "label87");
			this.label87.Name = "label87";
			componentResourceManager.ApplyResources(this.label88, "label88");
			this.label88.Name = "label88";
			componentResourceManager.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
			this.dateBeginHMS2.Name = "dateBeginHMS2";
			this.dateBeginHMS2.ShowUpDown = true;
			this.dateBeginHMS2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
			this.dateEndHMS2.Name = "dateEndHMS2";
			this.dateEndHMS2.ShowUpDown = true;
			this.dateEndHMS2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label86, "label86");
			this.label86.Name = "label86";
			componentResourceManager.ApplyResources(this.label85, "label85");
			this.label85.Name = "label85";
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.chkFirstInThenOut);
			base.Controls.Add(this.btnRestartAntiBack);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnExit);
			base.Name = "dfrmGlobalAntiBackManagement";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPrivilegeTypeDoors_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeTypeDoors_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmGlobalAntiBackManagement_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDoors).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedDoors).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).EndInit();
			this.groupBoxOut.ResumeLayout(false);
			this.groupBoxOut.PerformLayout();
			this.groupBoxIn.ResumeLayout(false);
			this.groupBoxIn.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002CEA RID: 11498
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x04002CEC RID: 11500
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002CED RID: 11501
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x04002CEE RID: 11502
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x04002CEF RID: 11503
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x04002CF0 RID: 11504
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x04002CF1 RID: 11505
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002CF2 RID: 11506
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002CF3 RID: 11507
		private global::System.Windows.Forms.Button btnRestartAntiBack;

		// Token: 0x04002CF4 RID: 11508
		private global::System.Windows.Forms.ComboBox cbof_ZoneID;

		// Token: 0x04002CF5 RID: 11509
		private global::System.Windows.Forms.CheckBox chkActiveTimeSegments;

		// Token: 0x04002CF6 RID: 11510
		private global::System.Windows.Forms.CheckBox chkFirstInThenOut;

		// Token: 0x04002CF7 RID: 11511
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002CF8 RID: 11512
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04002CF9 RID: 11513
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002CFA RID: 11514
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002CFB RID: 11515
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04002CFC RID: 11516
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04002CFD RID: 11517
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x04002CFE RID: 11518
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS2;

		// Token: 0x04002CFF RID: 11519
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS3;

		// Token: 0x04002D00 RID: 11520
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS4;

		// Token: 0x04002D01 RID: 11521
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS5;

		// Token: 0x04002D02 RID: 11522
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS6;

		// Token: 0x04002D03 RID: 11523
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit1;

		// Token: 0x04002D04 RID: 11524
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit2;

		// Token: 0x04002D05 RID: 11525
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit3;

		// Token: 0x04002D06 RID: 11526
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit4;

		// Token: 0x04002D07 RID: 11527
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit5;

		// Token: 0x04002D08 RID: 11528
		private global::System.Windows.Forms.DateTimePicker dateBeginHMSExit6;

		// Token: 0x04002D09 RID: 11529
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x04002D0A RID: 11530
		private global::System.Windows.Forms.DateTimePicker dateEndHMS2;

		// Token: 0x04002D0B RID: 11531
		private global::System.Windows.Forms.DateTimePicker dateEndHMS3;

		// Token: 0x04002D0C RID: 11532
		private global::System.Windows.Forms.DateTimePicker dateEndHMS4;

		// Token: 0x04002D0D RID: 11533
		private global::System.Windows.Forms.DateTimePicker dateEndHMS5;

		// Token: 0x04002D0E RID: 11534
		private global::System.Windows.Forms.DateTimePicker dateEndHMS6;

		// Token: 0x04002D0F RID: 11535
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit1;

		// Token: 0x04002D10 RID: 11536
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit2;

		// Token: 0x04002D11 RID: 11537
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit3;

		// Token: 0x04002D12 RID: 11538
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit4;

		// Token: 0x04002D13 RID: 11539
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit5;

		// Token: 0x04002D14 RID: 11540
		private global::System.Windows.Forms.DateTimePicker dateEndHMSExit6;

		// Token: 0x04002D15 RID: 11541
		private global::System.Windows.Forms.DataGridView dgvDoors;

		// Token: 0x04002D16 RID: 11542
		private global::System.Windows.Forms.DataGridView dgvSelectedDoors;

		// Token: 0x04002D17 RID: 11543
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName;

		// Token: 0x04002D18 RID: 11544
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControlSegName1;

		// Token: 0x04002D19 RID: 11545
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002D1A RID: 11546
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x04002D1B RID: 11547
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x04002D1C RID: 11548
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04002D1D RID: 11549
		private global::System.Windows.Forms.GroupBox groupBoxIn;

		// Token: 0x04002D1E RID: 11550
		private global::System.Windows.Forms.GroupBox groupBoxOut;

		// Token: 0x04002D1F RID: 11551
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002D20 RID: 11552
		private global::System.Windows.Forms.Label label10;

		// Token: 0x04002D21 RID: 11553
		private global::System.Windows.Forms.Label label11;

		// Token: 0x04002D22 RID: 11554
		private global::System.Windows.Forms.Label label12;

		// Token: 0x04002D23 RID: 11555
		private global::System.Windows.Forms.Label label13;

		// Token: 0x04002D24 RID: 11556
		private global::System.Windows.Forms.Label label14;

		// Token: 0x04002D25 RID: 11557
		private global::System.Windows.Forms.Label label15;

		// Token: 0x04002D26 RID: 11558
		private global::System.Windows.Forms.Label label16;

		// Token: 0x04002D27 RID: 11559
		private global::System.Windows.Forms.Label label17;

		// Token: 0x04002D28 RID: 11560
		private global::System.Windows.Forms.Label label18;

		// Token: 0x04002D29 RID: 11561
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002D2A RID: 11562
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04002D2B RID: 11563
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002D2C RID: 11564
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002D2D RID: 11565
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04002D2E RID: 11566
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04002D2F RID: 11567
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04002D30 RID: 11568
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04002D31 RID: 11569
		private global::System.Windows.Forms.Label label85;

		// Token: 0x04002D32 RID: 11570
		private global::System.Windows.Forms.Label label86;

		// Token: 0x04002D33 RID: 11571
		private global::System.Windows.Forms.Label label87;

		// Token: 0x04002D34 RID: 11572
		private global::System.Windows.Forms.Label label88;

		// Token: 0x04002D35 RID: 11573
		private global::System.Windows.Forms.Label label89;

		// Token: 0x04002D36 RID: 11574
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04002D37 RID: 11575
		private global::System.Windows.Forms.Label label90;

		// Token: 0x04002D38 RID: 11576
		private global::System.Windows.Forms.Label lblOptional;

		// Token: 0x04002D39 RID: 11577
		private global::System.Windows.Forms.Label lblSeleted;

		// Token: 0x04002D3A RID: 11578
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002D3B RID: 11579
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002D3C RID: 11580
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002D3D RID: 11581
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;

		// Token: 0x04002D3E RID: 11582
		internal global::System.Windows.Forms.CheckBox chkActiveAntibackShare;

		// Token: 0x04002D3F RID: 11583
		internal global::System.Windows.Forms.NumericUpDown nudTotal;
	}
}
