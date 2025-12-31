namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031A RID: 794
	public partial class dfrmPrivilegeTypeDoorsType : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001888 RID: 6280 RVA: 0x0020092C File Offset: 0x001FF92C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0020094C File Offset: 0x001FF94C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PrivilegeType.dfrmPrivilegeTypeDoorsType));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.cboDoor = new global::System.Windows.Forms.ComboBox();
			this.dgvPrivilegeTypes = new global::System.Windows.Forms.DataGridView();
			this.f_PrivilegeTypeID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PrivilegeTypeName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.label25 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cboDoor, "cboDoor");
			this.cboDoor.AutoCompleteMode = global::System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cboDoor.AutoCompleteSource = global::System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboDoor.FormattingEnabled = true;
			this.cboDoor.Name = "cboDoor";
			this.cboDoor.SelectedIndexChanged += new global::System.EventHandler(this.cboDoor_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.dgvPrivilegeTypes, "dgvPrivilegeTypes");
			this.dgvPrivilegeTypes.AllowUserToAddRows = false;
			this.dgvPrivilegeTypes.AllowUserToDeleteRows = false;
			this.dgvPrivilegeTypes.AllowUserToOrderColumns = true;
			this.dgvPrivilegeTypes.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvPrivilegeTypes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvPrivilegeTypes.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvPrivilegeTypes.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_PrivilegeTypeID, this.PrivilegeTypeName, this.f_Notes });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvPrivilegeTypes.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvPrivilegeTypes.EnableHeadersVisualStyles = false;
			this.dgvPrivilegeTypes.MultiSelect = false;
			this.dgvPrivilegeTypes.Name = "dgvPrivilegeTypes";
			this.dgvPrivilegeTypes.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvPrivilegeTypes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvPrivilegeTypes.RowTemplate.Height = 23;
			this.dgvPrivilegeTypes.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_PrivilegeTypeID, "f_PrivilegeTypeID");
			this.f_PrivilegeTypeID.Name = "f_PrivilegeTypeID";
			this.f_PrivilegeTypeID.ReadOnly = true;
			this.PrivilegeTypeName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.PrivilegeTypeName, "PrivilegeTypeName");
			this.PrivilegeTypeName.Name = "PrivilegeTypeName";
			this.PrivilegeTypeName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label25);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.cboDoor);
			base.Controls.Add(this.dgvPrivilegeTypes);
			base.Name = "dfrmPrivilegeTypeDoorsType";
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeTypeDoorsType_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400324C RID: 12876
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400324D RID: 12877
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x0400324E RID: 12878
		private global::System.Windows.Forms.ComboBox cboDoor;

		// Token: 0x0400324F RID: 12879
		private global::System.Windows.Forms.DataGridView dgvPrivilegeTypes;

		// Token: 0x04003250 RID: 12880
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04003251 RID: 12881
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PrivilegeTypeID;

		// Token: 0x04003252 RID: 12882
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04003253 RID: 12883
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PrivilegeTypeName;
	}
}
