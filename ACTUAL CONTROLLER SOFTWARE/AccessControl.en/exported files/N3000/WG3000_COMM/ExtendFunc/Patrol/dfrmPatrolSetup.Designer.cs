namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000308 RID: 776
	public partial class dfrmPatrolSetup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001765 RID: 5989 RVA: 0x001E5A9B File Offset: 0x001E4A9B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x001E5ABC File Offset: 0x001E4ABC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmPatrolSetup));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.dgvOptional = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDeleteAllReaders = new global::System.Windows.Forms.Button();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.btnDeleteOneReader = new global::System.Windows.Forms.Button();
			this.btnAddAllReaders = new global::System.Windows.Forms.Button();
			this.btnAddOneReader = new global::System.Windows.Forms.Button();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.nudPatrolAbsentTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudPatrolAllowTimeout = new global::System.Windows.Forms.NumericUpDown();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.grpUsers = new global::System.Windows.Forms.GroupBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).BeginInit();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPatrolAbsentTimeout).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudPatrolAllowTimeout).BeginInit();
			this.tabPage3.SuspendLayout();
			this.grpUsers.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.dgvSelected);
			this.tabPage1.Controls.Add(this.Label10);
			this.tabPage1.Controls.Add(this.dgvOptional);
			this.tabPage1.Controls.Add(this.btnDeleteAllReaders);
			this.tabPage1.Controls.Add(this.Label11);
			this.tabPage1.Controls.Add(this.btnDeleteOneReader);
			this.tabPage1.Controls.Add(this.btnAddAllReaders);
			this.tabPage1.Controls.Add(this.btnAddOneReader);
			this.tabPage1.Name = "tabPage1";
			this.toolTip1.SetToolTip(this.tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dgvSelected, "dgvSelected");
			this.dgvSelected.AllowUserToAddRows = false;
			this.dgvSelected.AllowUserToDeleteRows = false;
			this.dgvSelected.AllowUserToOrderColumns = true;
			this.dgvSelected.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelected.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelected.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3 });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelected.EnableHeadersVisualStyles = false;
			this.dgvSelected.Name = "dgvSelected";
			this.dgvSelected.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelected.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSelected.RowTemplate.Height = 23;
			this.dgvSelected.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelected, componentResourceManager.GetString("dgvSelected.ToolTip"));
			this.dgvSelected.DoubleClick += new global::System.EventHandler(this.btnDeleteOneReader_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Label10, "Label10");
			this.Label10.BackColor = global::System.Drawing.Color.Transparent;
			this.Label10.ForeColor = global::System.Drawing.Color.White;
			this.Label10.Name = "Label10";
			this.toolTip1.SetToolTip(this.Label10, componentResourceManager.GetString("Label10.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvOptional, "dgvOptional");
			this.dgvOptional.AllowUserToAddRows = false;
			this.dgvOptional.AllowUserToDeleteRows = false;
			this.dgvOptional.AllowUserToOrderColumns = true;
			this.dgvOptional.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvOptional.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvOptional.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvOptional.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvOptional.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvOptional.EnableHeadersVisualStyles = false;
			this.dgvOptional.Name = "dgvOptional";
			this.dgvOptional.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvOptional.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvOptional.RowTemplate.Height = 23;
			this.dgvOptional.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvOptional, componentResourceManager.GetString("dgvOptional.ToolTip"));
			this.dgvOptional.DoubleClick += new global::System.EventHandler(this.btnAddOneReader_Click);
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
			componentResourceManager.ApplyResources(this.btnDeleteAllReaders, "btnDeleteAllReaders");
			this.btnDeleteAllReaders.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAllReaders.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAllReaders.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAllReaders.Name = "btnDeleteAllReaders";
			this.toolTip1.SetToolTip(this.btnDeleteAllReaders, componentResourceManager.GetString("btnDeleteAllReaders.ToolTip"));
			this.btnDeleteAllReaders.UseVisualStyleBackColor = false;
			this.btnDeleteAllReaders.Click += new global::System.EventHandler(this.btnDeleteAllReaders_Click);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.BackColor = global::System.Drawing.Color.Transparent;
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			this.toolTip1.SetToolTip(this.Label11, componentResourceManager.GetString("Label11.ToolTip"));
			componentResourceManager.ApplyResources(this.btnDeleteOneReader, "btnDeleteOneReader");
			this.btnDeleteOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOneReader.Name = "btnDeleteOneReader";
			this.toolTip1.SetToolTip(this.btnDeleteOneReader, componentResourceManager.GetString("btnDeleteOneReader.ToolTip"));
			this.btnDeleteOneReader.UseVisualStyleBackColor = false;
			this.btnDeleteOneReader.Click += new global::System.EventHandler(this.btnDeleteOneReader_Click);
			componentResourceManager.ApplyResources(this.btnAddAllReaders, "btnAddAllReaders");
			this.btnAddAllReaders.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllReaders.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllReaders.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllReaders.Name = "btnAddAllReaders";
			this.toolTip1.SetToolTip(this.btnAddAllReaders, componentResourceManager.GetString("btnAddAllReaders.ToolTip"));
			this.btnAddAllReaders.UseVisualStyleBackColor = false;
			this.btnAddAllReaders.Click += new global::System.EventHandler(this.btnAddAllReaders_Click);
			componentResourceManager.ApplyResources(this.btnAddOneReader, "btnAddOneReader");
			this.btnAddOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneReader.Name = "btnAddOneReader";
			this.toolTip1.SetToolTip(this.btnAddOneReader, componentResourceManager.GetString("btnAddOneReader.ToolTip"));
			this.btnAddOneReader.UseVisualStyleBackColor = false;
			this.btnAddOneReader.Click += new global::System.EventHandler(this.btnAddOneReader_Click);
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.nudPatrolAbsentTimeout);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.nudPatrolAllowTimeout);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.toolTip1.SetToolTip(this.tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.nudPatrolAbsentTimeout, "nudPatrolAbsentTimeout");
			this.nudPatrolAbsentTimeout.Name = "nudPatrolAbsentTimeout";
			this.toolTip1.SetToolTip(this.nudPatrolAbsentTimeout, componentResourceManager.GetString("nudPatrolAbsentTimeout.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudPatrolAbsentTimeout;
			int[] array = new int[4];
			array[0] = 30;
			numericUpDown.Value = new decimal(array);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.nudPatrolAllowTimeout, "nudPatrolAllowTimeout");
			this.nudPatrolAllowTimeout.Name = "nudPatrolAllowTimeout";
			this.toolTip1.SetToolTip(this.nudPatrolAllowTimeout, componentResourceManager.GetString("nudPatrolAllowTimeout.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudPatrolAllowTimeout;
			int[] array2 = new int[4];
			array2[0] = 10;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.grpUsers);
			this.tabPage3.Name = "tabPage3";
			this.toolTip1.SetToolTip(this.tabPage3, componentResourceManager.GetString("tabPage3.ToolTip"));
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.grpUsers, "grpUsers");
			this.grpUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.grpUsers.Controls.Add(this.lblWait);
			this.grpUsers.Controls.Add(this.label3);
			this.grpUsers.Controls.Add(this.dgvSelectedUsers);
			this.grpUsers.Controls.Add(this.dgvUsers);
			this.grpUsers.Controls.Add(this.btnDelAllUsers);
			this.grpUsers.Controls.Add(this.btnDelOneUser);
			this.grpUsers.Controls.Add(this.btnAddOneUser);
			this.grpUsers.Controls.Add(this.btnAddAllUsers);
			this.grpUsers.Controls.Add(this.cbof_GroupID);
			this.grpUsers.Controls.Add(this.label6);
			this.grpUsers.ForeColor = global::System.Drawing.Color.White;
			this.grpUsers.Name = "grpUsers";
			this.grpUsers.TabStop = false;
			this.toolTip1.SetToolTip(this.grpUsers, componentResourceManager.GetString("grpUsers.ToolTip"));
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
			this.dgvSelectedUsers.AllowUserToAddRows = false;
			this.dgvSelectedUsers.AllowUserToDeleteRows = false;
			this.dgvSelectedUsers.AllowUserToOrderColumns = true;
			this.dgvSelectedUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle7.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle7.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle7.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn4, this.UserID2, this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup });
			dataGridViewCellStyle8.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle8.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle8.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle8.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle8;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.DoubleClick += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			dataGridViewCellStyle9.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID2.DefaultCellStyle = dataGridViewCellStyle9;
			componentResourceManager.ApplyResources(this.UserID2, "UserID2");
			this.UserID2.Name = "UserID2";
			this.UserID2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle10.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle10.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle10.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle10.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.UserID, this.ConsumerName, this.CardNO, this.f_SelectedUsers, this.f_GroupID });
			dataGridViewCellStyle11.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle11.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle11.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle11.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle11;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle12.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle12;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
			this.btnDelAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllUsers.Name = "btnDelAllUsers";
			this.toolTip1.SetToolTip(this.btnDelAllUsers, componentResourceManager.GetString("btnDelAllUsers.ToolTip"));
			this.btnDelAllUsers.UseVisualStyleBackColor = true;
			this.btnDelAllUsers.Click += new global::System.EventHandler(this.btnDelAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
			this.btnDelOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneUser.Name = "btnDelOneUser";
			this.toolTip1.SetToolTip(this.btnDelOneUser, componentResourceManager.GetString("btnDelOneUser.ToolTip"));
			this.btnDelOneUser.UseVisualStyleBackColor = true;
			this.btnDelOneUser.Click += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
			this.btnAddOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser.Name = "btnAddOneUser";
			this.toolTip1.SetToolTip(this.btnAddOneUser, componentResourceManager.GetString("btnAddOneUser.ToolTip"));
			this.btnAddOneUser.UseVisualStyleBackColor = true;
			this.btnAddOneUser.Click += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
			this.btnAddAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllUsers.Name = "btnAddAllUsers";
			this.toolTip1.SetToolTip(this.btnAddAllUsers, componentResourceManager.GetString("btnAddAllUsers.ToolTip"));
			this.btnAddAllUsers.UseVisualStyleBackColor = true;
			this.btnAddAllUsers.Click += new global::System.EventHandler(this.btnAddAllUsers_Click);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
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
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.tabControl1);
			base.Name = "dfrmPatrolSetup";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrm_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmPatrolSetup_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrm_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPatrolAbsentTimeout).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudPatrolAllowTimeout).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.grpUsers.ResumeLayout(false);
			this.grpUsers.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400301F RID: 12319
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003020 RID: 12320
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04003021 RID: 12321
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x04003022 RID: 12322
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x04003023 RID: 12323
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x04003024 RID: 12324
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x04003025 RID: 12325
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04003026 RID: 12326
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04003027 RID: 12327
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04003028 RID: 12328
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04003029 RID: 12329
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x0400302A RID: 12330
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x0400302B RID: 12331
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x0400302C RID: 12332
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x0400302D RID: 12333
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x0400302E RID: 12334
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x0400302F RID: 12335
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04003030 RID: 12336
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x04003031 RID: 12337
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x04003032 RID: 12338
		private global::System.Windows.Forms.DataGridView dgvOptional;

		// Token: 0x04003033 RID: 12339
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x04003034 RID: 12340
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x04003035 RID: 12341
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04003036 RID: 12342
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04003037 RID: 12343
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04003038 RID: 12344
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x04003039 RID: 12345
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x0400303A RID: 12346
		private global::System.Windows.Forms.GroupBox grpUsers;

		// Token: 0x0400303B RID: 12347
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400303C RID: 12348
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400303D RID: 12349
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400303E RID: 12350
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400303F RID: 12351
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04003040 RID: 12352
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04003041 RID: 12353
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x04003042 RID: 12354
		private global::System.Windows.Forms.NumericUpDown nudPatrolAbsentTimeout;

		// Token: 0x04003043 RID: 12355
		private global::System.Windows.Forms.NumericUpDown nudPatrolAllowTimeout;

		// Token: 0x04003044 RID: 12356
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04003045 RID: 12357
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04003046 RID: 12358
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04003047 RID: 12359
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04003048 RID: 12360
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04003049 RID: 12361
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400304A RID: 12362
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x0400304B RID: 12363
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID2;

		// Token: 0x0400304C RID: 12364
		internal global::System.Windows.Forms.Button btnAddAllReaders;

		// Token: 0x0400304D RID: 12365
		internal global::System.Windows.Forms.Button btnAddOneReader;

		// Token: 0x0400304E RID: 12366
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400304F RID: 12367
		internal global::System.Windows.Forms.Button btnDeleteAllReaders;

		// Token: 0x04003050 RID: 12368
		internal global::System.Windows.Forms.Button btnDeleteOneReader;

		// Token: 0x04003051 RID: 12369
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003052 RID: 12370
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x04003053 RID: 12371
		internal global::System.Windows.Forms.Label Label11;
	}
}
