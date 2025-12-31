namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x020002FD RID: 765
	public partial class dfrmMeetingAdr : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001685 RID: 5765 RVA: 0x001CCBE0 File Offset: 0x001CBBE0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x001CCC00 File Offset: 0x001CBC00
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.dfrmMeetingAdr));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.dgvSelected = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnSelectReader = new global::System.Windows.Forms.Button();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.lstMeetingAdr = new global::System.Windows.Forms.ListBox();
			this.btnAddMeetingAdr = new global::System.Windows.Forms.Button();
			this.btnDeleteMeetingAdr = new global::System.Windows.Forms.Button();
			this.GroupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).BeginInit();
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
			this.GroupBox1.Controls.Add(this.btnSelectReader);
			this.GroupBox1.Controls.Add(this.Label11);
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
			componentResourceManager.ApplyResources(this.btnSelectReader, "btnSelectReader");
			this.btnSelectReader.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectReader.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectReader.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectReader.Name = "btnSelectReader";
			this.btnSelectReader.UseVisualStyleBackColor = false;
			this.btnSelectReader.Click += new global::System.EventHandler(this.btnSelectReader_Click);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.BackColor = global::System.Drawing.Color.Transparent;
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.lstMeetingAdr, "lstMeetingAdr");
			this.lstMeetingAdr.Name = "lstMeetingAdr";
			this.lstMeetingAdr.SelectedIndexChanged += new global::System.EventHandler(this.lstMeetingAdr_SelectedIndexChanged);
			this.lstMeetingAdr.DoubleClick += new global::System.EventHandler(this.lstMeetingAdr_DoubleClick);
			componentResourceManager.ApplyResources(this.btnAddMeetingAdr, "btnAddMeetingAdr");
			this.btnAddMeetingAdr.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddMeetingAdr.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddMeetingAdr.ForeColor = global::System.Drawing.Color.White;
			this.btnAddMeetingAdr.Name = "btnAddMeetingAdr";
			this.btnAddMeetingAdr.UseVisualStyleBackColor = false;
			this.btnAddMeetingAdr.Click += new global::System.EventHandler(this.btnAddMeetingAdr_Click);
			componentResourceManager.ApplyResources(this.btnDeleteMeetingAdr, "btnDeleteMeetingAdr");
			this.btnDeleteMeetingAdr.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteMeetingAdr.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteMeetingAdr.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteMeetingAdr.Name = "btnDeleteMeetingAdr";
			this.btnDeleteMeetingAdr.UseVisualStyleBackColor = false;
			this.btnDeleteMeetingAdr.Click += new global::System.EventHandler(this.btnDeleteMeetingAdr_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnDeleteMeetingAdr);
			base.Controls.Add(this.btnAddMeetingAdr);
			base.Controls.Add(this.lstMeetingAdr);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMeetingAdr";
			base.Load += new global::System.EventHandler(this.dfrmMeetingAdr_Load);
			this.GroupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelected).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002EA5 RID: 11941
		private global::System.ComponentModel.Container components;

		// Token: 0x04002EA6 RID: 11942
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002EA7 RID: 11943
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002EA8 RID: 11944
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002EA9 RID: 11945
		private global::System.Windows.Forms.DataGridView dgvSelected;

		// Token: 0x04002EAA RID: 11946
		internal global::System.Windows.Forms.Button btnAddMeetingAdr;

		// Token: 0x04002EAB RID: 11947
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002EAC RID: 11948
		internal global::System.Windows.Forms.Button btnDeleteMeetingAdr;

		// Token: 0x04002EAD RID: 11949
		internal global::System.Windows.Forms.Button btnSelectReader;

		// Token: 0x04002EAE RID: 11950
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x04002EAF RID: 11951
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x04002EB0 RID: 11952
		internal global::System.Windows.Forms.ListBox lstMeetingAdr;
	}
}
