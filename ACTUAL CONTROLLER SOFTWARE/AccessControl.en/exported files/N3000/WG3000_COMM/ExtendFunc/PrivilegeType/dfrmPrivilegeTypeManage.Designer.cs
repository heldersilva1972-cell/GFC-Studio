namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031C RID: 796
	public partial class dfrmPrivilegeTypeManage : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x00202920 File Offset: 0x00201920
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00202940 File Offset: 0x00201940
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PrivilegeType.dfrmPrivilegeTypeManage));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.comboBox2 = new global::System.Windows.Forms.ComboBox();
			this.dgvPrivilegeTypes = new global::System.Windows.Forms.DataGridView();
			this.f_PrivilegeTypeID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PrivilegeTypeName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label25 = new global::System.Windows.Forms.Label();
			this.btnSetSelected = new global::System.Windows.Forms.Button();
			this.btnSetNone = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.btnEditUsers = new global::System.Windows.Forms.Button();
			this.btnEditDoors = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnQueryDoors = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.comboBox2, "comboBox2");
			this.comboBox2.AutoCompleteMode = global::System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.comboBox2.AutoCompleteSource = global::System.Windows.Forms.AutoCompleteSource.ListItems;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.comboBox2_KeyUp);
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
			this.dgvPrivilegeTypes.SelectionChanged += new global::System.EventHandler(this.dgvPrivilegeTypes_SelectionChanged);
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
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this.btnSetSelected, "btnSetSelected");
			this.btnSetSelected.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetSelected.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetSelected.ForeColor = global::System.Drawing.Color.White;
			this.btnSetSelected.Name = "btnSetSelected";
			this.btnSetSelected.UseVisualStyleBackColor = false;
			this.btnSetSelected.Click += new global::System.EventHandler(this.btnSetSelected_Click);
			componentResourceManager.ApplyResources(this.btnSetNone, "btnSetNone");
			this.btnSetNone.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetNone.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetNone.ForeColor = global::System.Drawing.Color.White;
			this.btnSetNone.Name = "btnSetNone";
			this.btnSetNone.UseVisualStyleBackColor = false;
			this.btnSetNone.Click += new global::System.EventHandler(this.btnSetNone_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnEditUsers, "btnEditUsers");
			this.btnEditUsers.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditUsers.ForeColor = global::System.Drawing.Color.White;
			this.btnEditUsers.Name = "btnEditUsers";
			this.btnEditUsers.UseVisualStyleBackColor = false;
			this.btnEditUsers.Click += new global::System.EventHandler(this.btnEditUsers_Click);
			componentResourceManager.ApplyResources(this.btnEditDoors, "btnEditDoors");
			this.btnEditDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEditDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEditDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnEditDoors.Name = "btnEditDoors";
			this.btnEditDoors.UseVisualStyleBackColor = false;
			this.btnEditDoors.Click += new global::System.EventHandler(this.btnEditDoors_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnQueryDoors, "btnQueryDoors");
			this.btnQueryDoors.BackColor = global::System.Drawing.Color.Transparent;
			this.btnQueryDoors.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnQueryDoors.ForeColor = global::System.Drawing.Color.White;
			this.btnQueryDoors.Name = "btnQueryDoors";
			this.btnQueryDoors.UseVisualStyleBackColor = false;
			this.btnQueryDoors.Click += new global::System.EventHandler(this.btnQueryDoors_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnQueryDoors);
			base.Controls.Add(this.comboBox2);
			base.Controls.Add(this.dgvPrivilegeTypes);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.btnSetSelected);
			base.Controls.Add(this.btnSetNone);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnDel);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.btnEditUsers);
			base.Controls.Add(this.btnEditDoors);
			base.Controls.Add(this.btnExit);
			base.MinimizeBox = false;
			base.Name = "dfrmPrivilegeTypeManage";
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeType_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvPrivilegeTypes).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003262 RID: 12898
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003263 RID: 12899
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04003264 RID: 12900
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003265 RID: 12901
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04003266 RID: 12902
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04003267 RID: 12903
		private global::System.Windows.Forms.Button btnEditDoors;

		// Token: 0x04003268 RID: 12904
		private global::System.Windows.Forms.Button btnEditUsers;

		// Token: 0x04003269 RID: 12905
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x0400326A RID: 12906
		private global::System.Windows.Forms.Button btnQueryDoors;

		// Token: 0x0400326B RID: 12907
		private global::System.Windows.Forms.Button btnSetNone;

		// Token: 0x0400326C RID: 12908
		private global::System.Windows.Forms.Button btnSetSelected;

		// Token: 0x0400326D RID: 12909
		private global::System.Windows.Forms.ComboBox comboBox2;

		// Token: 0x0400326E RID: 12910
		private global::System.Windows.Forms.DataGridView dgvPrivilegeTypes;

		// Token: 0x0400326F RID: 12911
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04003270 RID: 12912
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PrivilegeTypeID;

		// Token: 0x04003271 RID: 12913
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04003272 RID: 12914
		private global::System.Windows.Forms.DataGridViewTextBoxColumn PrivilegeTypeName;
	}
}
