namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030C RID: 780
	public partial class dfrmRouteEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x001ECAAF File Offset: 0x001EBAAF
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x001ECAD0 File Offset: 0x001EBAD0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmRouteEdit));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.txtName = new global::System.Windows.Forms.TextBox();
			this.cbof_RouteID = new global::System.Windows.Forms.ComboBox();
			this.Label8 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnCopyFromOtherRoute = new global::System.Windows.Forms.Button();
			this.btnStartTimeUpdate = new global::System.Windows.Forms.Button();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.label1 = new global::System.Windows.Forms.Label();
			this.chkAutoAdd = new global::System.Windows.Forms.CheckBox();
			this.dtpTime = new global::System.Windows.Forms.DateTimePicker();
			this.label45 = new global::System.Windows.Forms.Label();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.nudMinute = new global::System.Windows.Forms.NumericUpDown();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.NextDay = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Cost = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SN2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.dgvOptional = new global::System.Windows.Forms.DataGridView();
			this.f_NextDay1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_patroltime1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Sn = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDeleteAllReaders = new global::System.Windows.Forms.Button();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.btnDeleteOneReader = new global::System.Windows.Forms.Button();
			this.btnAddAllReaders = new global::System.Windows.Forms.Button();
			this.btnAddOneReader = new global::System.Windows.Forms.Button();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudMinute).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			componentResourceManager.ApplyResources(this.cbof_RouteID, "cbof_RouteID");
			this.cbof_RouteID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_RouteID.Name = "cbof_RouteID";
			componentResourceManager.ApplyResources(this.Label8, "Label8");
			this.Label8.BackColor = global::System.Drawing.Color.Transparent;
			this.Label8.ForeColor = global::System.Drawing.Color.White;
			this.Label8.Name = "Label8";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.btnCopyFromOtherRoute);
			this.tabPage1.Controls.Add(this.btnStartTimeUpdate);
			this.tabPage1.Controls.Add(this.radioButton2);
			this.tabPage1.Controls.Add(this.radioButton1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.chkAutoAdd);
			this.tabPage1.Controls.Add(this.dtpTime);
			this.tabPage1.Controls.Add(this.label45);
			this.tabPage1.Controls.Add(this.dateBeginHMS1);
			this.tabPage1.Controls.Add(this.nudMinute);
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
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.btnCopyFromOtherRoute, "btnCopyFromOtherRoute");
			this.btnCopyFromOtherRoute.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCopyFromOtherRoute.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCopyFromOtherRoute.ForeColor = global::System.Drawing.Color.White;
			this.btnCopyFromOtherRoute.Name = "btnCopyFromOtherRoute";
			this.btnCopyFromOtherRoute.UseVisualStyleBackColor = false;
			this.btnCopyFromOtherRoute.Click += new global::System.EventHandler(this.btnCopyFromOtherRoute_Click);
			componentResourceManager.ApplyResources(this.btnStartTimeUpdate, "btnStartTimeUpdate");
			this.btnStartTimeUpdate.BackColor = global::System.Drawing.Color.Transparent;
			this.btnStartTimeUpdate.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnStartTimeUpdate.ForeColor = global::System.Drawing.Color.White;
			this.btnStartTimeUpdate.Name = "btnStartTimeUpdate";
			this.btnStartTimeUpdate.UseVisualStyleBackColor = false;
			this.btnStartTimeUpdate.Click += new global::System.EventHandler(this.btnStartTimeUpdate_Click);
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.ForeColor = global::System.Drawing.Color.White;
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Checked = true;
			this.radioButton1.ForeColor = global::System.Drawing.Color.White;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabStop = true;
			this.radioButton1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.chkAutoAdd, "chkAutoAdd");
			this.chkAutoAdd.Checked = true;
			this.chkAutoAdd.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAutoAdd.ForeColor = global::System.Drawing.Color.White;
			this.chkAutoAdd.Name = "chkAutoAdd";
			this.chkAutoAdd.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dtpTime, "dtpTime");
			this.dtpTime.Name = "dtpTime";
			this.dtpTime.ShowUpDown = true;
			this.dtpTime.Value = new global::System.DateTime(2012, 6, 12, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label45, "label45");
			this.label45.BackColor = global::System.Drawing.Color.Transparent;
			this.label45.ForeColor = global::System.Drawing.Color.White;
			this.label45.Name = "label45";
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.nudMinute, "nudMinute");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudMinute;
			int[] array = new int[4];
			array[0] = 2400;
			numericUpDown.Maximum = new decimal(array);
			this.nudMinute.Name = "nudMinute";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudMinute;
			int[] array2 = new int[4];
			array2[0] = 30;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.dgvSelected, "dgvSelected");
			this.dgvSelected.AllowUserToAddRows = false;
			this.dgvSelected.AllowUserToDeleteRows = false;
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
			this.dgvSelected.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.NextDay, this.Cost, this.f_SN2, this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3 });
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
			this.NextDay.Frozen = true;
			componentResourceManager.ApplyResources(this.NextDay, "NextDay");
			this.NextDay.Name = "NextDay";
			this.NextDay.ReadOnly = true;
			this.NextDay.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			componentResourceManager.ApplyResources(this.Cost, "Cost");
			this.Cost.Name = "Cost";
			this.Cost.ReadOnly = true;
			this.Cost.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_SN2, "f_SN2");
			this.f_SN2.Name = "f_SN2";
			this.f_SN2.ReadOnly = true;
			this.f_SN2.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn2.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
			this.dgvOptional.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_NextDay1, this.f_patroltime1, this.f_Sn, this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected });
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
			componentResourceManager.ApplyResources(this.f_NextDay1, "f_NextDay1");
			this.f_NextDay1.Name = "f_NextDay1";
			this.f_NextDay1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_patroltime1, "f_patroltime1");
			this.f_patroltime1.Name = "f_patroltime1";
			this.f_patroltime1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Sn, "f_Sn");
			this.f_Sn.Name = "f_Sn";
			this.f_Sn.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			this.cmdCancel.Click += new global::System.EventHandler(this.cmdCancel_Click);
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtName);
			base.Controls.Add(this.cbof_RouteID);
			base.Controls.Add(this.Label8);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Name = "dfrmRouteEdit";
			base.Load += new global::System.EventHandler(this.dfrmMealOption_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudMinute).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040030A2 RID: 12450
		private global::System.Windows.Forms.DateTimePicker dtpTime;

		// Token: 0x040030AA RID: 12458
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040030AB RID: 12459
		private global::System.Windows.Forms.CheckBox chkAutoAdd;

		// Token: 0x040030AC RID: 12460
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Cost;

		// Token: 0x040030AD RID: 12461
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x040030AE RID: 12462
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x040030AF RID: 12463
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x040030B0 RID: 12464
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x040030B1 RID: 12465
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x040030B2 RID: 12466
		private global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040030B3 RID: 12467
		private global::System.Windows.Forms.DataGridView dgvOptional;

		// Token: 0x040030B4 RID: 12468
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x040030B5 RID: 12469
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NextDay1;

		// Token: 0x040030B6 RID: 12470
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_patroltime1;

		// Token: 0x040030B7 RID: 12471
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x040030B8 RID: 12472
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Sn;

		// Token: 0x040030B9 RID: 12473
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SN2;

		// Token: 0x040030BA RID: 12474
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040030BB RID: 12475
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040030BC RID: 12476
		private global::System.Windows.Forms.Label label45;

		// Token: 0x040030BD RID: 12477
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn NextDay;

		// Token: 0x040030BE RID: 12478
		private global::System.Windows.Forms.NumericUpDown nudMinute;

		// Token: 0x040030BF RID: 12479
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x040030C0 RID: 12480
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x040030C1 RID: 12481
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040030C2 RID: 12482
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040030C3 RID: 12483
		internal global::System.Windows.Forms.Button btnAddAllReaders;

		// Token: 0x040030C4 RID: 12484
		internal global::System.Windows.Forms.Button btnAddOneReader;

		// Token: 0x040030C5 RID: 12485
		internal global::System.Windows.Forms.Button btnCopyFromOtherRoute;

		// Token: 0x040030C6 RID: 12486
		internal global::System.Windows.Forms.Button btnDeleteAllReaders;

		// Token: 0x040030C7 RID: 12487
		internal global::System.Windows.Forms.Button btnDeleteOneReader;

		// Token: 0x040030C8 RID: 12488
		internal global::System.Windows.Forms.Button btnStartTimeUpdate;

		// Token: 0x040030C9 RID: 12489
		internal global::System.Windows.Forms.ComboBox cbof_RouteID;

		// Token: 0x040030CA RID: 12490
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x040030CB RID: 12491
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x040030CC RID: 12492
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x040030CD RID: 12493
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x040030CE RID: 12494
		internal global::System.Windows.Forms.Label label3;

		// Token: 0x040030CF RID: 12495
		internal global::System.Windows.Forms.Label Label8;

		// Token: 0x040030D0 RID: 12496
		internal global::System.Windows.Forms.TextBox txtName;
	}
}
