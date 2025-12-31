namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F9 RID: 761
	public partial class dfrmMealSetup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600163F RID: 5695 RVA: 0x001C3290 File Offset: 0x001C2290
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x001C32B0 File Offset: 0x001C22B0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meal.dfrmMealSetup));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
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
			this.chkByGroupLimit = new global::System.Windows.Forms.CheckBox();
			this.btnLimitPersonByGroups = new global::System.Windows.Forms.Button();
			this.chkAllowableSwipe = new global::System.Windows.Forms.CheckBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudRuleSeconds = new global::System.Windows.Forms.NumericUpDown();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.chkByGroup = new global::System.Windows.Forms.CheckBox();
			this.btnByDepartment = new global::System.Windows.Forms.Button();
			this.btnOption3 = new global::System.Windows.Forms.Button();
			this.btnOption2 = new global::System.Windows.Forms.Button();
			this.btnOption1 = new global::System.Windows.Forms.Button();
			this.btnOption0 = new global::System.Windows.Forms.Button();
			this.chkOtherMeal = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS4 = new global::System.Windows.Forms.DateTimePicker();
			this.nudOther = new global::System.Windows.Forms.NumericUpDown();
			this.lblOther = new global::System.Windows.Forms.Label();
			this.chkEveningMeal = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS3 = new global::System.Windows.Forms.DateTimePicker();
			this.nudEvening = new global::System.Windows.Forms.NumericUpDown();
			this.lblEvening = new global::System.Windows.Forms.Label();
			this.chkLunchMeal = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.nudLunch = new global::System.Windows.Forms.NumericUpDown();
			this.lblLunch = new global::System.Windows.Forms.Label();
			this.chkMorningMeal = new global::System.Windows.Forms.CheckBox();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.nudMorning = new global::System.Windows.Forms.NumericUpDown();
			this.lblMorning = new global::System.Windows.Forms.Label();
			this.label85 = new global::System.Windows.Forms.Label();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).BeginInit();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudRuleSeconds).BeginInit();
			this.tabPage3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudOther).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudEvening).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLunch).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudMorning).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
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
			this.btnDeleteAllReaders.UseVisualStyleBackColor = false;
			this.btnDeleteAllReaders.Click += new global::System.EventHandler(this.btnDeleteAllReaders_Click);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.BackColor = global::System.Drawing.Color.Transparent;
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.btnDeleteOneReader, "btnDeleteOneReader");
			this.btnDeleteOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOneReader.Name = "btnDeleteOneReader";
			this.btnDeleteOneReader.UseVisualStyleBackColor = false;
			this.btnDeleteOneReader.Click += new global::System.EventHandler(this.btnDeleteOneReader_Click);
			componentResourceManager.ApplyResources(this.btnAddAllReaders, "btnAddAllReaders");
			this.btnAddAllReaders.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllReaders.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllReaders.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllReaders.Name = "btnAddAllReaders";
			this.btnAddAllReaders.UseVisualStyleBackColor = false;
			this.btnAddAllReaders.Click += new global::System.EventHandler(this.btnAddAllReaders_Click);
			componentResourceManager.ApplyResources(this.btnAddOneReader, "btnAddOneReader");
			this.btnAddOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneReader.Name = "btnAddOneReader";
			this.btnAddOneReader.UseVisualStyleBackColor = false;
			this.btnAddOneReader.Click += new global::System.EventHandler(this.btnAddOneReader_Click);
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.chkByGroupLimit);
			this.tabPage2.Controls.Add(this.btnLimitPersonByGroups);
			this.tabPage2.Controls.Add(this.chkAllowableSwipe);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.nudRuleSeconds);
			this.tabPage2.Controls.Add(this.radioButton3);
			this.tabPage2.Controls.Add(this.radioButton2);
			this.tabPage2.Controls.Add(this.radioButton1);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkByGroupLimit, "chkByGroupLimit");
			this.chkByGroupLimit.ForeColor = global::System.Drawing.Color.White;
			this.chkByGroupLimit.Name = "chkByGroupLimit";
			this.chkByGroupLimit.UseVisualStyleBackColor = true;
			this.chkByGroupLimit.CheckedChanged += new global::System.EventHandler(this.chkByGroupLimit_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnLimitPersonByGroups, "btnLimitPersonByGroups");
			this.btnLimitPersonByGroups.BackColor = global::System.Drawing.Color.Transparent;
			this.btnLimitPersonByGroups.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnLimitPersonByGroups.ForeColor = global::System.Drawing.Color.White;
			this.btnLimitPersonByGroups.Name = "btnLimitPersonByGroups";
			this.btnLimitPersonByGroups.UseVisualStyleBackColor = false;
			this.btnLimitPersonByGroups.Click += new global::System.EventHandler(this.btnLimitPersonByGroups_Click);
			componentResourceManager.ApplyResources(this.chkAllowableSwipe, "chkAllowableSwipe");
			this.chkAllowableSwipe.Name = "chkAllowableSwipe";
			this.chkAllowableSwipe.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.nudRuleSeconds, "nudRuleSeconds");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudRuleSeconds;
			int[] array = new int[4];
			array[0] = 86400;
			numericUpDown.Maximum = new decimal(array);
			this.nudRuleSeconds.Name = "nudRuleSeconds";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudRuleSeconds;
			int[] array2 = new int[4];
			array2[0] = 60;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.radioButton3, "radioButton3");
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Checked = true;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabStop = true;
			this.radioButton1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.chkByGroup);
			this.tabPage3.Controls.Add(this.btnByDepartment);
			this.tabPage3.Controls.Add(this.btnOption3);
			this.tabPage3.Controls.Add(this.btnOption2);
			this.tabPage3.Controls.Add(this.btnOption1);
			this.tabPage3.Controls.Add(this.btnOption0);
			this.tabPage3.Controls.Add(this.chkOtherMeal);
			this.tabPage3.Controls.Add(this.dateBeginHMS4);
			this.tabPage3.Controls.Add(this.dateEndHMS4);
			this.tabPage3.Controls.Add(this.nudOther);
			this.tabPage3.Controls.Add(this.lblOther);
			this.tabPage3.Controls.Add(this.chkEveningMeal);
			this.tabPage3.Controls.Add(this.dateBeginHMS3);
			this.tabPage3.Controls.Add(this.dateEndHMS3);
			this.tabPage3.Controls.Add(this.nudEvening);
			this.tabPage3.Controls.Add(this.lblEvening);
			this.tabPage3.Controls.Add(this.chkLunchMeal);
			this.tabPage3.Controls.Add(this.dateBeginHMS2);
			this.tabPage3.Controls.Add(this.dateEndHMS2);
			this.tabPage3.Controls.Add(this.nudLunch);
			this.tabPage3.Controls.Add(this.lblLunch);
			this.tabPage3.Controls.Add(this.chkMorningMeal);
			this.tabPage3.Controls.Add(this.dateBeginHMS1);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.dateEndHMS1);
			this.tabPage3.Controls.Add(this.nudMorning);
			this.tabPage3.Controls.Add(this.lblMorning);
			this.tabPage3.Controls.Add(this.label85);
			this.tabPage3.Controls.Add(this.btnEdit);
			this.tabPage3.Controls.Add(this.btnDel);
			this.tabPage3.Controls.Add(this.btnAdd);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkByGroup, "chkByGroup");
			this.chkByGroup.ForeColor = global::System.Drawing.Color.White;
			this.chkByGroup.Name = "chkByGroup";
			this.chkByGroup.UseVisualStyleBackColor = true;
			this.chkByGroup.CheckedChanged += new global::System.EventHandler(this.chkByGroup_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnByDepartment, "btnByDepartment");
			this.btnByDepartment.BackColor = global::System.Drawing.Color.Transparent;
			this.btnByDepartment.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnByDepartment.ForeColor = global::System.Drawing.Color.White;
			this.btnByDepartment.Name = "btnByDepartment";
			this.btnByDepartment.UseVisualStyleBackColor = false;
			this.btnByDepartment.Click += new global::System.EventHandler(this.btnByDepartment_Click);
			componentResourceManager.ApplyResources(this.btnOption3, "btnOption3");
			this.btnOption3.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption3.ForeColor = global::System.Drawing.Color.White;
			this.btnOption3.Name = "btnOption3";
			this.btnOption3.UseVisualStyleBackColor = false;
			this.btnOption3.Click += new global::System.EventHandler(this.btnOption3_Click);
			componentResourceManager.ApplyResources(this.btnOption2, "btnOption2");
			this.btnOption2.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption2.ForeColor = global::System.Drawing.Color.White;
			this.btnOption2.Name = "btnOption2";
			this.btnOption2.UseVisualStyleBackColor = false;
			this.btnOption2.Click += new global::System.EventHandler(this.btnOption2_Click);
			componentResourceManager.ApplyResources(this.btnOption1, "btnOption1");
			this.btnOption1.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption1.ForeColor = global::System.Drawing.Color.White;
			this.btnOption1.Name = "btnOption1";
			this.btnOption1.UseVisualStyleBackColor = false;
			this.btnOption1.Click += new global::System.EventHandler(this.btnOption1_Click);
			componentResourceManager.ApplyResources(this.btnOption0, "btnOption0");
			this.btnOption0.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption0.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption0.ForeColor = global::System.Drawing.Color.White;
			this.btnOption0.Name = "btnOption0";
			this.btnOption0.UseVisualStyleBackColor = false;
			this.btnOption0.Click += new global::System.EventHandler(this.btnOption0_Click);
			componentResourceManager.ApplyResources(this.chkOtherMeal, "chkOtherMeal");
			this.chkOtherMeal.Checked = true;
			this.chkOtherMeal.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkOtherMeal.ForeColor = global::System.Drawing.Color.White;
			this.chkOtherMeal.Name = "chkOtherMeal";
			this.chkOtherMeal.UseVisualStyleBackColor = true;
			this.chkOtherMeal.CheckedChanged += new global::System.EventHandler(this.chkMeal_CheckedChanged);
			componentResourceManager.ApplyResources(this.dateBeginHMS4, "dateBeginHMS4");
			this.dateBeginHMS4.Name = "dateBeginHMS4";
			this.dateBeginHMS4.ShowUpDown = true;
			this.dateBeginHMS4.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS4, "dateEndHMS4");
			this.dateEndHMS4.Name = "dateEndHMS4";
			this.dateEndHMS4.ShowUpDown = true;
			this.dateEndHMS4.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.nudOther, "nudOther");
			this.nudOther.DecimalPlaces = 2;
			this.nudOther.Name = "nudOther";
			componentResourceManager.ApplyResources(this.lblOther, "lblOther");
			this.lblOther.ForeColor = global::System.Drawing.Color.White;
			this.lblOther.Name = "lblOther";
			componentResourceManager.ApplyResources(this.chkEveningMeal, "chkEveningMeal");
			this.chkEveningMeal.Checked = true;
			this.chkEveningMeal.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkEveningMeal.ForeColor = global::System.Drawing.Color.White;
			this.chkEveningMeal.Name = "chkEveningMeal";
			this.chkEveningMeal.UseVisualStyleBackColor = true;
			this.chkEveningMeal.CheckedChanged += new global::System.EventHandler(this.chkMeal_CheckedChanged);
			componentResourceManager.ApplyResources(this.dateBeginHMS3, "dateBeginHMS3");
			this.dateBeginHMS3.Name = "dateBeginHMS3";
			this.dateBeginHMS3.ShowUpDown = true;
			this.dateBeginHMS3.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS3, "dateEndHMS3");
			this.dateEndHMS3.Name = "dateEndHMS3";
			this.dateEndHMS3.ShowUpDown = true;
			this.dateEndHMS3.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.nudEvening, "nudEvening");
			this.nudEvening.DecimalPlaces = 2;
			this.nudEvening.Name = "nudEvening";
			componentResourceManager.ApplyResources(this.lblEvening, "lblEvening");
			this.lblEvening.ForeColor = global::System.Drawing.Color.White;
			this.lblEvening.Name = "lblEvening";
			componentResourceManager.ApplyResources(this.chkLunchMeal, "chkLunchMeal");
			this.chkLunchMeal.Checked = true;
			this.chkLunchMeal.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkLunchMeal.ForeColor = global::System.Drawing.Color.White;
			this.chkLunchMeal.Name = "chkLunchMeal";
			this.chkLunchMeal.UseVisualStyleBackColor = true;
			this.chkLunchMeal.CheckedChanged += new global::System.EventHandler(this.chkMeal_CheckedChanged);
			componentResourceManager.ApplyResources(this.dateBeginHMS2, "dateBeginHMS2");
			this.dateBeginHMS2.Name = "dateBeginHMS2";
			this.dateBeginHMS2.ShowUpDown = true;
			this.dateBeginHMS2.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
			this.dateEndHMS2.Name = "dateEndHMS2";
			this.dateEndHMS2.ShowUpDown = true;
			this.dateEndHMS2.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.nudLunch, "nudLunch");
			this.nudLunch.DecimalPlaces = 2;
			this.nudLunch.Name = "nudLunch";
			componentResourceManager.ApplyResources(this.lblLunch, "lblLunch");
			this.lblLunch.ForeColor = global::System.Drawing.Color.White;
			this.lblLunch.Name = "lblLunch";
			componentResourceManager.ApplyResources(this.chkMorningMeal, "chkMorningMeal");
			this.chkMorningMeal.Checked = true;
			this.chkMorningMeal.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkMorningMeal.ForeColor = global::System.Drawing.Color.White;
			this.chkMorningMeal.Name = "chkMorningMeal";
			this.chkMorningMeal.UseVisualStyleBackColor = true;
			this.chkMorningMeal.CheckedChanged += new global::System.EventHandler(this.chkMeal_CheckedChanged);
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2010, 1, 1, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.nudMorning, "nudMorning");
			this.nudMorning.DecimalPlaces = 2;
			this.nudMorning.Name = "nudMorning";
			componentResourceManager.ApplyResources(this.lblMorning, "lblMorning");
			this.lblMorning.ForeColor = global::System.Drawing.Color.White;
			this.lblMorning.Name = "lblMorning";
			componentResourceManager.ApplyResources(this.label85, "label85");
			this.label85.ForeColor = global::System.Drawing.Color.White;
			this.label85.Name = "label85";
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.tabControl1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMealSetup";
			base.Load += new global::System.EventHandler(this.dfrmMealSetup_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudRuleSeconds).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudOther).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudEvening).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLunch).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudMorning).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002E01 RID: 11777
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002E02 RID: 11778
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04002E03 RID: 11779
		private global::System.Windows.Forms.Button btnByDepartment;

		// Token: 0x04002E04 RID: 11780
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04002E05 RID: 11781
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04002E06 RID: 11782
		private global::System.Windows.Forms.Button btnLimitPersonByGroups;

		// Token: 0x04002E07 RID: 11783
		private global::System.Windows.Forms.Button btnOption0;

		// Token: 0x04002E08 RID: 11784
		private global::System.Windows.Forms.Button btnOption1;

		// Token: 0x04002E09 RID: 11785
		private global::System.Windows.Forms.Button btnOption2;

		// Token: 0x04002E0A RID: 11786
		private global::System.Windows.Forms.Button btnOption3;

		// Token: 0x04002E0B RID: 11787
		private global::System.Windows.Forms.CheckBox chkAllowableSwipe;

		// Token: 0x04002E0C RID: 11788
		private global::System.Windows.Forms.CheckBox chkByGroup;

		// Token: 0x04002E0D RID: 11789
		private global::System.Windows.Forms.CheckBox chkByGroupLimit;

		// Token: 0x04002E0E RID: 11790
		private global::System.Windows.Forms.CheckBox chkEveningMeal;

		// Token: 0x04002E0F RID: 11791
		private global::System.Windows.Forms.CheckBox chkLunchMeal;

		// Token: 0x04002E10 RID: 11792
		private global::System.Windows.Forms.CheckBox chkMorningMeal;

		// Token: 0x04002E11 RID: 11793
		private global::System.Windows.Forms.CheckBox chkOtherMeal;

		// Token: 0x04002E12 RID: 11794
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002E13 RID: 11795
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002E14 RID: 11796
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002E15 RID: 11797
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002E16 RID: 11798
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002E17 RID: 11799
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x04002E18 RID: 11800
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS2;

		// Token: 0x04002E19 RID: 11801
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS3;

		// Token: 0x04002E1A RID: 11802
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS4;

		// Token: 0x04002E1B RID: 11803
		private global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x04002E1C RID: 11804
		private global::System.Windows.Forms.DateTimePicker dateEndHMS2;

		// Token: 0x04002E1D RID: 11805
		private global::System.Windows.Forms.DateTimePicker dateEndHMS3;

		// Token: 0x04002E1E RID: 11806
		private global::System.Windows.Forms.DateTimePicker dateEndHMS4;

		// Token: 0x04002E1F RID: 11807
		private global::System.Windows.Forms.DataGridView dgvOptional;

		// Token: 0x04002E20 RID: 11808
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x04002E21 RID: 11809
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002E22 RID: 11810
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002E23 RID: 11811
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002E24 RID: 11812
		private global::System.Windows.Forms.Label label85;

		// Token: 0x04002E25 RID: 11813
		private global::System.Windows.Forms.Label lblEvening;

		// Token: 0x04002E26 RID: 11814
		private global::System.Windows.Forms.Label lblLunch;

		// Token: 0x04002E27 RID: 11815
		private global::System.Windows.Forms.Label lblMorning;

		// Token: 0x04002E28 RID: 11816
		private global::System.Windows.Forms.Label lblOther;

		// Token: 0x04002E29 RID: 11817
		private global::System.Windows.Forms.NumericUpDown nudEvening;

		// Token: 0x04002E2A RID: 11818
		private global::System.Windows.Forms.NumericUpDown nudLunch;

		// Token: 0x04002E2B RID: 11819
		private global::System.Windows.Forms.NumericUpDown nudMorning;

		// Token: 0x04002E2C RID: 11820
		private global::System.Windows.Forms.NumericUpDown nudOther;

		// Token: 0x04002E2D RID: 11821
		private global::System.Windows.Forms.NumericUpDown nudRuleSeconds;

		// Token: 0x04002E2E RID: 11822
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x04002E2F RID: 11823
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x04002E30 RID: 11824
		private global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x04002E31 RID: 11825
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002E32 RID: 11826
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002E33 RID: 11827
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002E34 RID: 11828
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04002E35 RID: 11829
		internal global::System.Windows.Forms.Button btnAddAllReaders;

		// Token: 0x04002E36 RID: 11830
		internal global::System.Windows.Forms.Button btnAddOneReader;

		// Token: 0x04002E37 RID: 11831
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002E38 RID: 11832
		internal global::System.Windows.Forms.Button btnDeleteAllReaders;

		// Token: 0x04002E39 RID: 11833
		internal global::System.Windows.Forms.Button btnDeleteOneReader;

		// Token: 0x04002E3A RID: 11834
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002E3B RID: 11835
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x04002E3C RID: 11836
		internal global::System.Windows.Forms.Label Label11;
	}
}
