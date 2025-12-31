namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000239 RID: 569
	public partial class dfrmControllerMultiCards : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600113A RID: 4410 RVA: 0x0013C3EA File Offset: 0x0013B3EA
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

		// Token: 0x0600113B RID: 4411 RVA: 0x0013C420 File Offset: 0x0013B420
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerMultiCards));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_DoorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorNo = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MoreCards_Total = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
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
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_DoorID, this.f_ControllerSN, this.f_DoorNo, this.f_DoorName, this.f_MoreCards_Total });
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Click += new global::System.EventHandler(this.dataGridView1_DoubleClick);
			componentResourceManager.ApplyResources(this.f_DoorID, "f_DoorID");
			this.f_DoorID.Name = "f_DoorID";
			this.f_DoorID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorNo, "f_DoorNo");
			this.f_DoorNo.Name = "f_DoorNo";
			this.f_DoorNo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MoreCards_Total, "f_MoreCards_Total");
			this.f_MoreCards_Total.Name = "f_MoreCards_Total";
			this.f_MoreCards_Total.ReadOnly = true;
			this.f_MoreCards_Total.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmControllerMultiCards";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerMultiCards_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerMultiCards_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerMultiCards_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001E95 RID: 7829
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E96 RID: 7830
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001E97 RID: 7831
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04001E98 RID: 7832
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001E99 RID: 7833
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001E9A RID: 7834
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorID;

		// Token: 0x04001E9B RID: 7835
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x04001E9C RID: 7836
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNo;

		// Token: 0x04001E9D RID: 7837
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MoreCards_Total;
	}
}
