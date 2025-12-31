namespace WG3000_COMM.Basic
{
	// Token: 0x0200001F RID: 31
	public partial class dfrmOperator : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x0003D724 File Offset: 0x0003C724
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0003D744 File Offset: 0x0003C744
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmOperator));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnSetPassword = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditPrivilege = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditDepartment = new global::System.Windows.Forms.ToolStripButton();
			this.btnEditZones = new global::System.Windows.Forms.ToolStripButton();
			this.dgvOperators = new global::System.Windows.Forms.DataGridView();
			this.f_OperatorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OperatorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOperators).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnSetPassword, this.btnEditPrivilege, this.btnEditDepartment, this.btnEditZones });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Image = global::WG3000_COMM.Properties.Resources.pTools_Add;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.ForeColor = global::System.Drawing.Color.White;
			this.btnDelete.Image = global::WG3000_COMM.Properties.Resources.pTools_Del;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Click += new global::System.EventHandler(this.btnDelete_Click);
			componentResourceManager.ApplyResources(this.btnSetPassword, "btnSetPassword");
			this.btnSetPassword.ForeColor = global::System.Drawing.Color.White;
			this.btnSetPassword.Image = global::WG3000_COMM.Properties.Resources.pTools_SetPwd;
			this.btnSetPassword.Name = "btnSetPassword";
			this.btnSetPassword.Click += new global::System.EventHandler(this.btnSetPassword_Click);
			componentResourceManager.ApplyResources(this.btnEditPrivilege, "btnEditPrivilege");
			this.btnEditPrivilege.ForeColor = global::System.Drawing.Color.White;
			this.btnEditPrivilege.Image = global::WG3000_COMM.Properties.Resources.pTools_EditPrivielge;
			this.btnEditPrivilege.Name = "btnEditPrivilege";
			this.btnEditPrivilege.Click += new global::System.EventHandler(this.btnEditPrivilege_Click);
			componentResourceManager.ApplyResources(this.btnEditDepartment, "btnEditDepartment");
			this.btnEditDepartment.ForeColor = global::System.Drawing.Color.White;
			this.btnEditDepartment.Image = global::WG3000_COMM.Properties.Resources.pTools_Operator_Group;
			this.btnEditDepartment.Name = "btnEditDepartment";
			this.btnEditDepartment.Click += new global::System.EventHandler(this.btnEditDepartment_Click);
			componentResourceManager.ApplyResources(this.btnEditZones, "btnEditZones");
			this.btnEditZones.ForeColor = global::System.Drawing.Color.White;
			this.btnEditZones.Image = global::WG3000_COMM.Properties.Resources.pTools_Operator_Zone;
			this.btnEditZones.Name = "btnEditZones";
			this.btnEditZones.Click += new global::System.EventHandler(this.btnEditZones_Click);
			componentResourceManager.ApplyResources(this.dgvOperators, "dgvOperators");
			this.dgvOperators.AllowUserToAddRows = false;
			this.dgvOperators.AllowUserToDeleteRows = false;
			this.dgvOperators.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvOperators.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvOperators.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvOperators.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_OperatorID, this.f_OperatorName });
			this.dgvOperators.EnableHeadersVisualStyles = false;
			this.dgvOperators.MultiSelect = false;
			this.dgvOperators.Name = "dgvOperators";
			this.dgvOperators.ReadOnly = true;
			this.dgvOperators.RowTemplate.Height = 23;
			this.dgvOperators.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_OperatorID, "f_OperatorID");
			this.f_OperatorID.Name = "f_OperatorID";
			this.f_OperatorID.ReadOnly = true;
			this.f_OperatorName.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_OperatorName, "f_OperatorName");
			this.f_OperatorName.Name = "f_OperatorName";
			this.f_OperatorName.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvOperators);
			base.Controls.Add(this.toolStrip1);
			base.Name = "dfrmOperator";
			base.Load += new global::System.EventHandler(this.dfrmOperator_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOperators).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040003DE RID: 990
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040003DF RID: 991
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x040003E0 RID: 992
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x040003E1 RID: 993
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x040003E2 RID: 994
		private global::System.Windows.Forms.ToolStripButton btnEditDepartment;

		// Token: 0x040003E3 RID: 995
		private global::System.Windows.Forms.ToolStripButton btnEditPrivilege;

		// Token: 0x040003E4 RID: 996
		private global::System.Windows.Forms.ToolStripButton btnEditZones;

		// Token: 0x040003E5 RID: 997
		private global::System.Windows.Forms.ToolStripButton btnSetPassword;

		// Token: 0x040003E6 RID: 998
		private global::System.Windows.Forms.DataGridView dgvOperators;

		// Token: 0x040003E7 RID: 999
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OperatorID;

		// Token: 0x040003E8 RID: 1000
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OperatorName;

		// Token: 0x040003E9 RID: 1001
		private global::System.Windows.Forms.ToolStrip toolStrip1;
	}
}
