namespace WG3000_COMM.ExtendFunc.Cloud2017
{
	// Token: 0x02000230 RID: 560
	public partial class dfrmCloudServers : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010CE RID: 4302 RVA: 0x00132929 File Offset: 0x00131929
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00132948 File Offset: 0x00131948
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Cloud2017.dfrmCloudServers));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnAddToSystem = new global::System.Windows.Forms.Button();
			this.dgvFoundControllers = new global::System.Windows.Forms.DataGridView();
			this.f_ID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PORT = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RefreshTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CreateTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnRefresh = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFoundControllers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnAddToSystem, "btnAddToSystem");
			this.btnAddToSystem.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddToSystem.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddToSystem.ForeColor = global::System.Drawing.Color.White;
			this.btnAddToSystem.Name = "btnAddToSystem";
			this.btnAddToSystem.UseVisualStyleBackColor = false;
			this.btnAddToSystem.Click += new global::System.EventHandler(this.btnAddToSystem_Click);
			componentResourceManager.ApplyResources(this.dgvFoundControllers, "dgvFoundControllers");
			this.dgvFoundControllers.AllowUserToAddRows = false;
			this.dgvFoundControllers.AllowUserToDeleteRows = false;
			this.dgvFoundControllers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvFoundControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvFoundControllers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvFoundControllers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ID, this.f_ControllerSN, this.f_IP, this.f_PORT, this.RefreshTime, this.CreateTime });
			this.dgvFoundControllers.EnableHeadersVisualStyles = false;
			this.dgvFoundControllers.Name = "dgvFoundControllers";
			this.dgvFoundControllers.ReadOnly = true;
			this.dgvFoundControllers.RowHeadersVisible = false;
			this.dgvFoundControllers.RowTemplate.Height = 23;
			this.dgvFoundControllers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_ID, "f_ID");
			this.f_ID.Name = "f_ID";
			this.f_ID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_IP, "f_IP");
			this.f_IP.Name = "f_IP";
			this.f_IP.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_PORT.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_PORT, "f_PORT");
			this.f_PORT.Name = "f_PORT";
			this.f_PORT.ReadOnly = true;
			componentResourceManager.ApplyResources(this.RefreshTime, "RefreshTime");
			this.RefreshTime.Name = "RefreshTime";
			this.RefreshTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CreateTime, "CreateTime");
			this.CreateTime.Name = "CreateTime";
			this.CreateTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnRefresh, "btnRefresh");
			this.btnRefresh.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRefresh.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRefresh.ForeColor = global::System.Drawing.Color.White;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new global::System.EventHandler(this.btnRefresh_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnRefresh);
			base.Controls.Add(this.dgvFoundControllers);
			base.Controls.Add(this.btnAddToSystem);
			base.Name = "dfrmCloudServers";
			base.Load += new global::System.EventHandler(this.dfrmCloudServers_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvFoundControllers).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001DDC RID: 7644
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001DDD RID: 7645
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;

		// Token: 0x04001DDE RID: 7646
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04001DDF RID: 7647
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x04001DE0 RID: 7648
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04001DE1 RID: 7649
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x04001DE2 RID: 7650
		private global::System.Windows.Forms.DataGridViewTextBoxColumn RefreshTime;

		// Token: 0x04001DE3 RID: 7651
		public global::System.Windows.Forms.Button btnAddToSystem;

		// Token: 0x04001DE4 RID: 7652
		public global::System.Windows.Forms.Button btnRefresh;

		// Token: 0x04001DE5 RID: 7653
		public global::System.Windows.Forms.DataGridView dgvFoundControllers;
	}
}
