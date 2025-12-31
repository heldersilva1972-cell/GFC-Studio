namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FE RID: 766
	public partial class dfrmMeetingAdrSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x001CDEA6 File Offset: 0x001CCEA6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x001CDEC8 File Offset: 0x001CCEC8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.dfrmMeetingAdrSet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvOptional = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.btnAddAllReaders = new global::System.Windows.Forms.Button();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.btnAddOneReader = new global::System.Windows.Forms.Button();
			this.btnDeleteOneReader = new global::System.Windows.Forms.Button();
			this.btnDeleteAllReaders = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.txtMeetingAdr = new global::System.Windows.Forms.TextBox();
			this.GroupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.Controls.Add(this.dgvSelected);
			this.GroupBox1.Controls.Add(this.dgvOptional);
			this.GroupBox1.Controls.Add(this.Label11);
			this.GroupBox1.Controls.Add(this.btnAddAllReaders);
			this.GroupBox1.Controls.Add(this.Label10);
			this.GroupBox1.Controls.Add(this.btnAddOneReader);
			this.GroupBox1.Controls.Add(this.btnDeleteOneReader);
			this.GroupBox1.Controls.Add(this.btnDeleteAllReaders);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
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
			this.dgvSelected.DoubleClick += new global::System.EventHandler(this.dgvSelected_DoubleClick);
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
			this.dgvOptional.DoubleClick += new global::System.EventHandler(this.dgvOptional_DoubleClick);
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
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.BackColor = global::System.Drawing.Color.Transparent;
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.btnAddAllReaders, "btnAddAllReaders");
			this.btnAddAllReaders.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllReaders.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllReaders.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllReaders.Name = "btnAddAllReaders";
			this.btnAddAllReaders.UseVisualStyleBackColor = false;
			this.btnAddAllReaders.Click += new global::System.EventHandler(this.btnAddAllReaders_Click);
			componentResourceManager.ApplyResources(this.Label10, "Label10");
			this.Label10.BackColor = global::System.Drawing.Color.Transparent;
			this.Label10.ForeColor = global::System.Drawing.Color.White;
			this.Label10.Name = "Label10";
			componentResourceManager.ApplyResources(this.btnAddOneReader, "btnAddOneReader");
			this.btnAddOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneReader.Name = "btnAddOneReader";
			this.btnAddOneReader.UseVisualStyleBackColor = false;
			this.btnAddOneReader.Click += new global::System.EventHandler(this.btnAddOneReader_Click);
			componentResourceManager.ApplyResources(this.btnDeleteOneReader, "btnDeleteOneReader");
			this.btnDeleteOneReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteOneReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOneReader.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOneReader.Name = "btnDeleteOneReader";
			this.btnDeleteOneReader.UseVisualStyleBackColor = false;
			this.btnDeleteOneReader.Click += new global::System.EventHandler(this.btnDeleteOneReader_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAllReaders, "btnDeleteAllReaders");
			this.btnDeleteAllReaders.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAllReaders.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAllReaders.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAllReaders.Name = "btnDeleteAllReaders";
			this.btnDeleteAllReaders.UseVisualStyleBackColor = false;
			this.btnDeleteAllReaders.Click += new global::System.EventHandler(this.btnDeleteAllReaders_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.txtMeetingAdr, "txtMeetingAdr");
			this.txtMeetingAdr.Name = "txtMeetingAdr";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtMeetingAdr);
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMeetingAdrSet";
			base.Load += new global::System.EventHandler(this.dfrmMeetingAdr_Load);
			this.GroupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOptional).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002EB6 RID: 11958
		private global::System.ComponentModel.Container components;

		// Token: 0x04002EB7 RID: 11959
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002EB8 RID: 11960
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002EB9 RID: 11961
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002EBA RID: 11962
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04002EBB RID: 11963
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04002EBC RID: 11964
		private global::System.Windows.Forms.DataGridView dgvOptional;

		// Token: 0x04002EBD RID: 11965
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x04002EBE RID: 11966
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x04002EBF RID: 11967
		internal global::System.Windows.Forms.Button btnAddAllReaders;

		// Token: 0x04002EC0 RID: 11968
		internal global::System.Windows.Forms.Button btnAddOneReader;

		// Token: 0x04002EC1 RID: 11969
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002EC2 RID: 11970
		internal global::System.Windows.Forms.Button btnDeleteAllReaders;

		// Token: 0x04002EC3 RID: 11971
		internal global::System.Windows.Forms.Button btnDeleteOneReader;

		// Token: 0x04002EC4 RID: 11972
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002EC5 RID: 11973
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x04002EC6 RID: 11974
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x04002EC7 RID: 11975
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x04002EC8 RID: 11976
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x04002EC9 RID: 11977
		internal global::System.Windows.Forms.TextBox txtMeetingAdr;
	}
}
