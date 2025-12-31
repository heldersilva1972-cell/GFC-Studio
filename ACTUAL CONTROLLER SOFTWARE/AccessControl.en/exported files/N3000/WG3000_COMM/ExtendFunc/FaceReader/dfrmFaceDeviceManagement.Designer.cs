namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002D1 RID: 721
	public partial class dfrmFaceDeviceManagement : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001360 RID: 4960 RVA: 0x0016E455 File Offset: 0x0016D455
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0016E474 File Offset: 0x0016D474
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.FaceReader.dfrmFaceDeviceManagement));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.dgvDevices = new global::System.Windows.Forms.DataGridView();
			this.f_DeviceID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DeviceName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.grpbController.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvDevices).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.grpbController, "grpbController");
			this.grpbController.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbController.Controls.Add(this.dgvDevices);
			this.grpbController.Controls.Add(this.btnEdit);
			this.grpbController.Controls.Add(this.btnAdd);
			this.grpbController.Controls.Add(this.btnDel);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			componentResourceManager.ApplyResources(this.dgvDevices, "dgvDevices");
			this.dgvDevices.AllowUserToAddRows = false;
			this.dgvDevices.AllowUserToDeleteRows = false;
			this.dgvDevices.AllowUserToOrderColumns = true;
			this.dgvDevices.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvDevices.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDevices.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_DeviceID, this.f_DeviceName, this.Column1, this.f_IP, this.f_Port, this.f_Notes });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvDevices.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvDevices.EnableHeadersVisualStyles = false;
			this.dgvDevices.MultiSelect = false;
			this.dgvDevices.Name = "dgvDevices";
			this.dgvDevices.ReadOnly = true;
			this.dgvDevices.RowTemplate.Height = 23;
			this.dgvDevices.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvDevices.DoubleClick += new global::System.EventHandler(this.dgvDevices_DoubleClick);
			componentResourceManager.ApplyResources(this.f_DeviceID, "f_DeviceID");
			this.f_DeviceID.Name = "f_DeviceID";
			this.f_DeviceID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DeviceName, "f_DeviceName");
			this.f_DeviceName.Name = "f_DeviceName";
			this.f_DeviceName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_IP.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_IP, "f_IP");
			this.f_IP.Name = "f_IP";
			this.f_IP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Port, "f_Port");
			this.f_Port.Name = "f_Port";
			this.f_Port.ReadOnly = true;
			this.f_Notes.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
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
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.grpbController);
			base.Name = "dfrmFaceDeviceManagement";
			base.Load += new global::System.EventHandler(this.dfrmFaceDeviceManage_Load);
			this.grpbController.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvDevices).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400297C RID: 10620
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400297D RID: 10621
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x0400297E RID: 10622
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x0400297F RID: 10623
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04002980 RID: 10624
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04002981 RID: 10625
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002982 RID: 10626
		private global::System.Windows.Forms.DataGridView dgvDevices;

		// Token: 0x04002983 RID: 10627
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DeviceID;

		// Token: 0x04002984 RID: 10628
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DeviceName;

		// Token: 0x04002985 RID: 10629
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04002986 RID: 10630
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04002987 RID: 10631
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Port;

		// Token: 0x04002988 RID: 10632
		private global::System.Windows.Forms.GroupBox grpbController;
	}
}
