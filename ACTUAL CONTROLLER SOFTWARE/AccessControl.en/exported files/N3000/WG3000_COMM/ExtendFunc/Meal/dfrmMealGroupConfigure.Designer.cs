namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F5 RID: 757
	public partial class dfrmMealGroupConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x001BD9DD File Offset: 0x001BC9DD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x001BD9FC File Offset: 0x001BC9FC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meal.dfrmMealGroupConfigure));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.dgvGroups = new global::System.Windows.Forms.DataGridView();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.GroupName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Enable = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.MoreCards = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SoundFile = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Evening = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Other = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.label3 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroups).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.toolTip1.SetToolTip(this.btnEdit, componentResourceManager.GetString("btnEdit.ToolTip"));
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.dgvGroups, "dgvGroups");
			this.dgvGroups.AllowUserToAddRows = false;
			this.dgvGroups.AllowUserToDeleteRows = false;
			this.dgvGroups.AllowUserToOrderColumns = true;
			this.dgvGroups.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvGroups.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvGroups.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_GroupID, this.GroupName, this.Enable, this.MoreCards, this.f_SoundFile, this.Evening, this.Other });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvGroups.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvGroups.EnableHeadersVisualStyles = false;
			this.dgvGroups.Name = "dgvGroups";
			this.dgvGroups.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvGroups.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvGroups.RowTemplate.Height = 23;
			this.dgvGroups.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvGroups, componentResourceManager.GetString("dgvGroups.ToolTip"));
			this.dgvGroups.DoubleClick += new global::System.EventHandler(this.dgvGroups_DoubleClick);
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.GroupName, "GroupName");
			this.GroupName.Name = "GroupName";
			this.GroupName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Enable, "Enable");
			this.Enable.Name = "Enable";
			this.Enable.ReadOnly = true;
			this.Enable.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.Enable.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.MoreCards, "MoreCards");
			this.MoreCards.Name = "MoreCards";
			this.MoreCards.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SoundFile, "f_SoundFile");
			this.f_SoundFile.Name = "f_SoundFile";
			this.f_SoundFile.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Evening, "Evening");
			this.Evening.Name = "Evening";
			this.Evening.ReadOnly = true;
			this.Other.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Other, "Other");
			this.Other.Name = "Other";
			this.Other.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.dgvGroups);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMealGroupConfigure";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Load += new global::System.EventHandler(this.dfrmCheckAccessSetup_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvGroups).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002DB0 RID: 11696
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002DB1 RID: 11697
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002DB2 RID: 11698
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04002DB3 RID: 11699
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002DB4 RID: 11700
		private global::System.Windows.Forms.DataGridView dgvGroups;

		// Token: 0x04002DB6 RID: 11702
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn Enable;

		// Token: 0x04002DB7 RID: 11703
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Evening;

		// Token: 0x04002DB8 RID: 11704
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04002DB9 RID: 11705
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SoundFile;

		// Token: 0x04002DBA RID: 11706
		private global::System.Windows.Forms.DataGridViewTextBoxColumn GroupName;

		// Token: 0x04002DBB RID: 11707
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002DBC RID: 11708
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MoreCards;

		// Token: 0x04002DBD RID: 11709
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Other;

		// Token: 0x04002DBE RID: 11710
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
