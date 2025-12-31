namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F8 RID: 760
	public partial class dfrmMealOption : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600162A RID: 5674 RVA: 0x001BFF08 File Offset: 0x001BEF08
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x001BFF28 File Offset: 0x001BEF28
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meal.dfrmMealOption));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.label4 = new global::System.Windows.Forms.Label();
			this.nudCost = new global::System.Windows.Forms.NumericUpDown();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Cost = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.dgvOptional = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Cost = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDeleteAllReaders = new global::System.Windows.Forms.Button();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.btnDeleteOneReader = new global::System.Windows.Forms.Button();
			this.btnAddAllReaders = new global::System.Windows.Forms.Button();
			this.btnAddOneReader = new global::System.Windows.Forms.Button();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCost).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.nudCost);
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
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.nudCost, "nudCost");
			this.nudCost.DecimalPlaces = 2;
			this.nudCost.Name = "nudCost";
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudCost;
			int[] array = new int[4];
			array[0] = 5;
			numericUpDown.Value = new decimal(array);
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
			this.dgvSelected.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.Cost });
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
			componentResourceManager.ApplyResources(this.Cost, "Cost");
			this.Cost.Name = "Cost";
			this.Cost.ReadOnly = true;
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
			this.dgvOptional.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.f_Cost });
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
			componentResourceManager.ApplyResources(this.f_Cost, "f_Cost");
			this.f_Cost.Name = "f_Cost";
			this.f_Cost.ReadOnly = true;
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
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Name = "dfrmMealOption";
			base.Load += new global::System.EventHandler(this.dfrmMealOption_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudCost).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002DE6 RID: 11750
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002DE7 RID: 11751
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Cost;

		// Token: 0x04002DE8 RID: 11752
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002DE9 RID: 11753
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002DEA RID: 11754
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002DEB RID: 11755
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002DEC RID: 11756
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002DED RID: 11757
		private global::System.Windows.Forms.DataGridView dgvOptional;

		// Token: 0x04002DEE RID: 11758
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x04002DEF RID: 11759
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Cost;

		// Token: 0x04002DF0 RID: 11760
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002DF1 RID: 11761
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002DF2 RID: 11762
		private global::System.Windows.Forms.NumericUpDown nudCost;

		// Token: 0x04002DF3 RID: 11763
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04002DF4 RID: 11764
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002DF5 RID: 11765
		internal global::System.Windows.Forms.Button btnAddAllReaders;

		// Token: 0x04002DF6 RID: 11766
		internal global::System.Windows.Forms.Button btnAddOneReader;

		// Token: 0x04002DF7 RID: 11767
		internal global::System.Windows.Forms.Button btnDeleteAllReaders;

		// Token: 0x04002DF8 RID: 11768
		internal global::System.Windows.Forms.Button btnDeleteOneReader;

		// Token: 0x04002DF9 RID: 11769
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x04002DFA RID: 11770
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x04002DFB RID: 11771
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x04002DFC RID: 11772
		internal global::System.Windows.Forms.Label Label11;
	}
}
