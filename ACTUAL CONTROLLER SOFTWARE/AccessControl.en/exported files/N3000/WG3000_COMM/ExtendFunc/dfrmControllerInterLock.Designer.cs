namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000238 RID: 568
	public partial class dfrmControllerInterLock : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x0013BB32 File Offset: 0x0013AB32
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

		// Token: 0x06001130 RID: 4400 RVA: 0x0013BB68 File Offset: 0x0013AB68
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerInterLock));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.chkGrouped = new global::System.Windows.Forms.CheckBox();
			this.chkActiveInterlockShare = new global::System.Windows.Forms.CheckBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_ControllerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_InterLock12 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_InterLock34 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_InterLock123 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_InterLock1234 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_DoorNames = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkGrouped, "chkGrouped");
			this.chkGrouped.BackColor = global::System.Drawing.Color.Transparent;
			this.chkGrouped.ForeColor = global::System.Drawing.Color.White;
			this.chkGrouped.Name = "chkGrouped";
			this.chkGrouped.UseVisualStyleBackColor = false;
			this.chkGrouped.CheckedChanged += new global::System.EventHandler(this.chkGrouped_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActiveInterlockShare, "chkActiveInterlockShare");
			this.chkActiveInterlockShare.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveInterlockShare.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveInterlockShare.Name = "chkActiveInterlockShare";
			this.chkActiveInterlockShare.UseVisualStyleBackColor = false;
			this.chkActiveInterlockShare.CheckedChanged += new global::System.EventHandler(this.chkActiveInterlockShare_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
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
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ControllerID, this.f_ControllerSN, this.f_InterLock12, this.f_InterLock34, this.f_InterLock123, this.f_InterLock1234, this.f_DoorNames });
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			componentResourceManager.ApplyResources(this.f_ControllerID, "f_ControllerID");
			this.f_ControllerID.Name = "f_ControllerID";
			this.f_ControllerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_InterLock12, "f_InterLock12");
			this.f_InterLock12.Name = "f_InterLock12";
			this.f_InterLock12.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_InterLock12.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_InterLock34, "f_InterLock34");
			this.f_InterLock34.Name = "f_InterLock34";
			componentResourceManager.ApplyResources(this.f_InterLock123, "f_InterLock123");
			this.f_InterLock123.Name = "f_InterLock123";
			componentResourceManager.ApplyResources(this.f_InterLock1234, "f_InterLock1234");
			this.f_InterLock1234.Name = "f_InterLock1234";
			this.f_DoorNames.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_DoorNames, "f_DoorNames");
			this.f_DoorNames.Name = "f_DoorNames";
			this.f_DoorNames.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkGrouped);
			base.Controls.Add(this.chkActiveInterlockShare);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmControllerInterLock";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerInterLock_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerInterLock_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerInterLock_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001E88 RID: 7816
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E89 RID: 7817
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001E8A RID: 7818
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001E8B RID: 7819
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001E8C RID: 7820
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerID;

		// Token: 0x04001E8D RID: 7821
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001E8E RID: 7822
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNames;

		// Token: 0x04001E8F RID: 7823
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock12;

		// Token: 0x04001E90 RID: 7824
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock123;

		// Token: 0x04001E91 RID: 7825
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock1234;

		// Token: 0x04001E92 RID: 7826
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_InterLock34;

		// Token: 0x04001E93 RID: 7827
		internal global::System.Windows.Forms.CheckBox chkActiveInterlockShare;

		// Token: 0x04001E94 RID: 7828
		internal global::System.Windows.Forms.CheckBox chkGrouped;
	}
}
