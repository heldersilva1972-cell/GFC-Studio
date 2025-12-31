namespace WG3000_COMM.Basic
{
	// Token: 0x0200001E RID: 30
	public partial class dfrmOperatePrivilege : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0003C734 File Offset: 0x0003B734
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0003C754 File Offset: 0x0003B754
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmOperatePrivilege));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvOperatePrivilege = new global::System.Windows.Forms.DataGridView();
			this.f_FunctionID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_FunctionName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_FunctionDisplayName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ReadOnly = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_FullControl = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_DisplayID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnReadAllOff = new global::System.Windows.Forms.Button();
			this.btnFullControlOff = new global::System.Windows.Forms.Button();
			this.btnReadAllOn = new global::System.Windows.Forms.Button();
			this.btnFullControlAllOn = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvOperatePrivilege).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dgvOperatePrivilege, "dgvOperatePrivilege");
			this.dgvOperatePrivilege.AllowUserToAddRows = false;
			this.dgvOperatePrivilege.AllowUserToDeleteRows = false;
			this.dgvOperatePrivilege.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvOperatePrivilege.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvOperatePrivilege.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvOperatePrivilege.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_FunctionID, this.f_FunctionName, this.f_FunctionDisplayName, this.f_ReadOnly, this.f_FullControl, this.f_DisplayID });
			this.dgvOperatePrivilege.EnableHeadersVisualStyles = false;
			this.dgvOperatePrivilege.Name = "dgvOperatePrivilege";
			this.dgvOperatePrivilege.RowHeadersVisible = false;
			this.dgvOperatePrivilege.RowTemplate.Height = 23;
			this.dgvOperatePrivilege.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOperatePrivilege_CellFormatting);
			componentResourceManager.ApplyResources(this.f_FunctionID, "f_FunctionID");
			this.f_FunctionID.Name = "f_FunctionID";
			componentResourceManager.ApplyResources(this.f_FunctionName, "f_FunctionName");
			this.f_FunctionName.Name = "f_FunctionName";
			componentResourceManager.ApplyResources(this.f_FunctionDisplayName, "f_FunctionDisplayName");
			this.f_FunctionDisplayName.Name = "f_FunctionDisplayName";
			this.f_FunctionDisplayName.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_ReadOnly, "f_ReadOnly");
			this.f_ReadOnly.Name = "f_ReadOnly";
			componentResourceManager.ApplyResources(this.f_FullControl, "f_FullControl");
			this.f_FullControl.Name = "f_FullControl";
			componentResourceManager.ApplyResources(this.f_DisplayID, "f_DisplayID");
			this.f_DisplayID.Name = "f_DisplayID";
			componentResourceManager.ApplyResources(this.btnReadAllOff, "btnReadAllOff");
			this.btnReadAllOff.BackColor = global::System.Drawing.Color.Transparent;
			this.btnReadAllOff.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnReadAllOff.ForeColor = global::System.Drawing.Color.White;
			this.btnReadAllOff.Name = "btnReadAllOff";
			this.btnReadAllOff.UseVisualStyleBackColor = false;
			this.btnReadAllOff.Click += new global::System.EventHandler(this.btnReadAllOff_Click);
			componentResourceManager.ApplyResources(this.btnFullControlOff, "btnFullControlOff");
			this.btnFullControlOff.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFullControlOff.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFullControlOff.ForeColor = global::System.Drawing.Color.White;
			this.btnFullControlOff.Name = "btnFullControlOff";
			this.btnFullControlOff.UseVisualStyleBackColor = false;
			this.btnFullControlOff.Click += new global::System.EventHandler(this.btnFullControlOff_Click);
			componentResourceManager.ApplyResources(this.btnReadAllOn, "btnReadAllOn");
			this.btnReadAllOn.BackColor = global::System.Drawing.Color.Transparent;
			this.btnReadAllOn.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnReadAllOn.ForeColor = global::System.Drawing.Color.White;
			this.btnReadAllOn.Name = "btnReadAllOn";
			this.btnReadAllOn.UseVisualStyleBackColor = false;
			this.btnReadAllOn.Click += new global::System.EventHandler(this.btnReadAllOn_Click);
			componentResourceManager.ApplyResources(this.btnFullControlAllOn, "btnFullControlAllOn");
			this.btnFullControlAllOn.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFullControlAllOn.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFullControlAllOn.ForeColor = global::System.Drawing.Color.White;
			this.btnFullControlAllOn.Name = "btnFullControlAllOn";
			this.btnFullControlAllOn.UseVisualStyleBackColor = false;
			this.btnFullControlAllOn.Click += new global::System.EventHandler(this.btnFullControlAllOn_Click);
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnFullControlAllOn);
			base.Controls.Add(this.btnReadAllOn);
			base.Controls.Add(this.dgvOperatePrivilege);
			base.Controls.Add(this.btnFullControlOff);
			base.Controls.Add(this.btnReadAllOff);
			base.Name = "dfrmOperatePrivilege";
			base.Load += new global::System.EventHandler(this.dfrmOperatePrivilege_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvOperatePrivilege).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x040003CE RID: 974
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040003CF RID: 975
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040003D0 RID: 976
		private global::System.Windows.Forms.Button btnFullControlAllOn;

		// Token: 0x040003D1 RID: 977
		private global::System.Windows.Forms.Button btnFullControlOff;

		// Token: 0x040003D2 RID: 978
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040003D3 RID: 979
		private global::System.Windows.Forms.Button btnReadAllOff;

		// Token: 0x040003D4 RID: 980
		private global::System.Windows.Forms.Button btnReadAllOn;

		// Token: 0x040003D5 RID: 981
		private global::System.Windows.Forms.DataGridView dgvOperatePrivilege;

		// Token: 0x040003D6 RID: 982
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DisplayID;

		// Token: 0x040003D7 RID: 983
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_FullControl;

		// Token: 0x040003D8 RID: 984
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_FunctionDisplayName;

		// Token: 0x040003D9 RID: 985
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_FunctionID;

		// Token: 0x040003DA RID: 986
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_FunctionName;

		// Token: 0x040003DB RID: 987
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_ReadOnly;
	}
}
