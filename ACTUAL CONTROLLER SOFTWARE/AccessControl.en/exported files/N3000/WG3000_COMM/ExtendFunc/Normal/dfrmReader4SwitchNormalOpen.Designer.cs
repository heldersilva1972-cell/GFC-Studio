namespace WG3000_COMM.ExtendFunc.Normal
{
	// Token: 0x02000303 RID: 771
	public partial class dfrmReader4SwitchNormalOpen : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001705 RID: 5893 RVA: 0x001DE2FD File Offset: 0x001DD2FD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x001DE31C File Offset: 0x001DD31C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Normal.dfrmReader4SwitchNormalOpen));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.chkSelectedUsers = new global::System.Windows.Forms.CheckBox();
			this.cbof_ControlSegID = new global::System.Windows.Forms.ComboBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_ReaderID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReaderNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReaderName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PasswordEnabled = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkSelectedUsers, "chkSelectedUsers");
			this.chkSelectedUsers.ForeColor = global::System.Drawing.Color.White;
			this.chkSelectedUsers.Name = "chkSelectedUsers";
			this.chkSelectedUsers.UseVisualStyleBackColor = true;
			this.chkSelectedUsers.CheckedChanged += new global::System.EventHandler(this.chkSelectedUsers_CheckedChanged);
			componentResourceManager.ApplyResources(this.cbof_ControlSegID, "cbof_ControlSegID");
			this.cbof_ControlSegID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ControlSegID.FormattingEnabled = true;
			this.cbof_ControlSegID.Name = "cbof_ControlSegID";
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
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ReaderID, this.f_ControllerSN, this.f_ReaderNO, this.f_ReaderName, this.f_PasswordEnabled });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			componentResourceManager.ApplyResources(this.f_ReaderID, "f_ReaderID");
			this.f_ReaderID.Name = "f_ReaderID";
			this.f_ReaderID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReaderNO, "f_ReaderNO");
			this.f_ReaderNO.Name = "f_ReaderNO";
			this.f_ReaderNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ReaderName, "f_ReaderName");
			this.f_ReaderName.Name = "f_ReaderName";
			this.f_ReaderName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_PasswordEnabled, "f_PasswordEnabled");
			this.f_PasswordEnabled.Name = "f_PasswordEnabled";
			this.f_PasswordEnabled.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_PasswordEnabled.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkSelectedUsers);
			base.Controls.Add(this.cbof_ControlSegID);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmReader4SwitchNormalOpen";
			base.Load += new global::System.EventHandler(this.dfrmReader4SwitchNormalOpen_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002FBD RID: 12221
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002FBE RID: 12222
		private global::System.Windows.Forms.ComboBox cbof_ControlSegID;

		// Token: 0x04002FBF RID: 12223
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04002FC0 RID: 12224
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04002FC1 RID: 12225
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_PasswordEnabled;

		// Token: 0x04002FC2 RID: 12226
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderID;

		// Token: 0x04002FC3 RID: 12227
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderName;

		// Token: 0x04002FC4 RID: 12228
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ReaderNO;

		// Token: 0x04002FC5 RID: 12229
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002FC6 RID: 12230
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002FC7 RID: 12231
		public global::System.Windows.Forms.CheckBox chkSelectedUsers;
	}
}
