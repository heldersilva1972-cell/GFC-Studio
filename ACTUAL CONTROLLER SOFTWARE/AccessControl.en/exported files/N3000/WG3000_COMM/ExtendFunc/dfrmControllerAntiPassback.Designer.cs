namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000235 RID: 565
	public partial class dfrmControllerAntiPassback : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x00136BE2 File Offset: 0x00135BE2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmFind1 != null)
			{
				this.dfrmFind1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00136C18 File Offset: 0x00135C18
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerAntiPassback));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_ControllerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_AntiBack = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_DoorNames = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.chkGrouped = new global::System.Windows.Forms.CheckBox();
			this.chkActiveAntibackShare = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
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
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ControllerID, this.f_ControllerSN, this.f_AntiBack, this.f_DoorNames });
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Click += new global::System.EventHandler(this.dataGridView1_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ControllerID, "f_ControllerID");
			this.f_ControllerID.Name = "f_ControllerID";
			this.f_ControllerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_AntiBack, "f_AntiBack");
			this.f_AntiBack.Name = "f_AntiBack";
			this.f_AntiBack.ReadOnly = true;
			this.f_AntiBack.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_AntiBack.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.f_DoorNames.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_DoorNames, "f_DoorNames");
			this.f_DoorNames.Name = "f_DoorNames";
			this.f_DoorNames.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.chkGrouped, "chkGrouped");
			this.chkGrouped.BackColor = global::System.Drawing.Color.Transparent;
			this.chkGrouped.ForeColor = global::System.Drawing.Color.White;
			this.chkGrouped.Name = "chkGrouped";
			this.chkGrouped.UseVisualStyleBackColor = false;
			this.chkGrouped.CheckedChanged += new global::System.EventHandler(this.chkGrouped_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActiveAntibackShare, "chkActiveAntibackShare");
			this.chkActiveAntibackShare.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveAntibackShare.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveAntibackShare.Name = "chkActiveAntibackShare";
			this.chkActiveAntibackShare.UseVisualStyleBackColor = false;
			this.chkActiveAntibackShare.CheckedChanged += new global::System.EventHandler(this.chkActiveAntibackShare_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkGrouped);
			base.Controls.Add(this.chkActiveAntibackShare);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmControllerAntiPassback";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerAntiPassback_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerAntiPassback_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerAntiPassback_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001E32 RID: 7730
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E33 RID: 7731
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001E34 RID: 7732
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04001E35 RID: 7733
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001E36 RID: 7734
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_AntiBack;

		// Token: 0x04001E37 RID: 7735
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerID;

		// Token: 0x04001E38 RID: 7736
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001E39 RID: 7737
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNames;

		// Token: 0x04001E3A RID: 7738
		internal global::System.Windows.Forms.CheckBox chkActiveAntibackShare;

		// Token: 0x04001E3B RID: 7739
		internal global::System.Windows.Forms.CheckBox chkGrouped;
	}
}
