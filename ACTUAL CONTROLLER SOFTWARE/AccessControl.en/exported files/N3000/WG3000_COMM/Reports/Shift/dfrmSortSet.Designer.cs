namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000376 RID: 886
	public partial class dfrmSortSet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001D1E RID: 7454 RVA: 0x0026897F File Offset: 0x0026797F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x002689A0 File Offset: 0x002679A0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmSortSet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.cbof_Sort = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.dgvSelectedColumns = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeProfile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvColumns = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Selected = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAddAllDoors = new global::System.Windows.Forms.Button();
			this.btnDelOneDoor = new global::System.Windows.Forms.Button();
			this.btnDelAllDoors = new global::System.Windows.Forms.Button();
			this.btnAddOneDoor = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedColumns).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvColumns).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.cbof_Sort, "cbof_Sort");
			this.cbof_Sort.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_Sort.FormattingEnabled = true;
			this.cbof_Sort.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbof_Sort.Items"),
				componentResourceManager.GetString("cbof_Sort.Items1")
			});
			this.cbof_Sort.Name = "cbof_Sort";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.dgvSelectedColumns, "dgvSelectedColumns");
			this.dgvSelectedColumns.AllowUserToAddRows = false;
			this.dgvSelectedColumns.AllowUserToDeleteRows = false;
			this.dgvSelectedColumns.AllowUserToOrderColumns = true;
			this.dgvSelectedColumns.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedColumns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedColumns.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedColumns.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.f_Selected2, this.TimeProfile, this.Column3, this.Column5 });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedColumns.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedColumns.EnableHeadersVisualStyles = false;
			this.dgvSelectedColumns.Name = "dgvSelectedColumns";
			this.dgvSelectedColumns.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedColumns.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSelectedColumns.RowTemplate.Height = 23;
			this.dgvSelectedColumns.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Selected2, "f_Selected2");
			this.f_Selected2.Name = "f_Selected2";
			this.f_Selected2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TimeProfile, "TimeProfile");
			this.TimeProfile.Name = "TimeProfile";
			this.TimeProfile.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column5, "Column5");
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvColumns, "dgvColumns");
			this.dgvColumns.AllowUserToAddRows = false;
			this.dgvColumns.AllowUserToDeleteRows = false;
			this.dgvColumns.AllowUserToOrderColumns = true;
			this.dgvColumns.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvColumns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvColumns.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvColumns.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.f_Selected, this.Column2, this.Column1, this.Column4 });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvColumns.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvColumns.EnableHeadersVisualStyles = false;
			this.dgvColumns.Name = "dgvColumns";
			this.dgvColumns.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle6.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle6.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvColumns.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgvColumns.RowTemplate.Height = 23;
			this.dgvColumns.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnAddAllDoors, "btnAddAllDoors");
			this.btnAddAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllDoors.Name = "btnAddAllDoors";
			this.btnAddAllDoors.UseVisualStyleBackColor = false;
			this.btnAddAllDoors.Click += new global::System.EventHandler(this.btnAddAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnDelOneDoor, "btnDelOneDoor");
			this.btnDelOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnDelOneDoor.Name = "btnDelOneDoor";
			this.btnDelOneDoor.UseVisualStyleBackColor = false;
			this.btnDelOneDoor.Click += new global::System.EventHandler(this.btnDelOneDoor_Click);
			componentResourceManager.ApplyResources(this.btnDelAllDoors, "btnDelAllDoors");
			this.btnDelAllDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelAllDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnDelAllDoors.Name = "btnDelAllDoors";
			this.btnDelAllDoors.UseVisualStyleBackColor = false;
			this.btnDelAllDoors.Click += new global::System.EventHandler(this.btnDelAllDoors_Click);
			componentResourceManager.ApplyResources(this.btnAddOneDoor, "btnAddOneDoor");
			this.btnAddOneDoor.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOneDoor.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneDoor.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneDoor.Name = "btnAddOneDoor";
			this.btnAddOneDoor.UseVisualStyleBackColor = false;
			this.btnAddOneDoor.Click += new global::System.EventHandler(this.btnAddOneDoor_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.cbof_Sort);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.dgvSelectedColumns);
			base.Controls.Add(this.dgvColumns);
			base.Controls.Add(this.btnAddAllDoors);
			base.Controls.Add(this.btnDelOneDoor);
			base.Controls.Add(this.btnDelAllDoors);
			base.Controls.Add(this.btnAddOneDoor);
			base.Name = "dfrmSortSet";
			base.Load += new global::System.EventHandler(this.dfrmSortSet_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedColumns).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvColumns).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400380A RID: 14346
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400380B RID: 14347
		private global::System.Windows.Forms.Button btnAddAllDoors;

		// Token: 0x0400380C RID: 14348
		private global::System.Windows.Forms.Button btnAddOneDoor;

		// Token: 0x0400380D RID: 14349
		private global::System.Windows.Forms.Button btnDelAllDoors;

		// Token: 0x0400380E RID: 14350
		private global::System.Windows.Forms.Button btnDelOneDoor;

		// Token: 0x0400380F RID: 14351
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04003810 RID: 14352
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003811 RID: 14353
		private global::System.Windows.Forms.ComboBox cbof_Sort;

		// Token: 0x04003812 RID: 14354
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04003813 RID: 14355
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04003814 RID: 14356
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04003815 RID: 14357
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x04003816 RID: 14358
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column5;

		// Token: 0x04003817 RID: 14359
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x04003818 RID: 14360
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		// Token: 0x04003819 RID: 14361
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		// Token: 0x0400381A RID: 14362
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		// Token: 0x0400381B RID: 14363
		private global::System.Windows.Forms.DataGridView dgvColumns;

		// Token: 0x0400381C RID: 14364
		private global::System.Windows.Forms.DataGridView dgvSelectedColumns;

		// Token: 0x0400381D RID: 14365
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected;

		// Token: 0x0400381E RID: 14366
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Selected2;

		// Token: 0x0400381F RID: 14367
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003820 RID: 14368
		private global::System.Windows.Forms.DataGridViewTextBoxColumn TimeProfile;
	}
}
