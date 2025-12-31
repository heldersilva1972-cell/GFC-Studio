namespace WG3000_COMM.Basic
{
	// Token: 0x02000019 RID: 25
	public partial class dfrmLogQuery : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000294BE File Offset: 0x000284BE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000294E0 File Offset: 0x000284E0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmLogQuery));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.btnExportExcel = new global::System.Windows.Forms.Button();
			this.btnFind = new global::System.Windows.Forms.Button();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_LogDateTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EventType = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EventDesc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this.btnExportExcel, "btnExportExcel");
			this.btnExportExcel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExportExcel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExportExcel.ForeColor = global::System.Drawing.Color.White;
			this.btnExportExcel.Name = "btnExportExcel";
			this.btnExportExcel.UseVisualStyleBackColor = false;
			this.btnExportExcel.Click += new global::System.EventHandler(this.btnExportExcel_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.AllowUserToOrderColumns = true;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_RecID, this.f_LogDateTime, this.f_EventType, this.f_EventDesc });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowHeadersVisible = false;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
			this.dgvMain.DoubleClick += new global::System.EventHandler(this.dgvMain_DoubleClick);
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_LogDateTime, "f_LogDateTime");
			this.f_LogDateTime.Name = "f_LogDateTime";
			this.f_LogDateTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_EventType, "f_EventType");
			this.f_EventType.Name = "f_EventType";
			this.f_EventType.ReadOnly = true;
			this.f_EventDesc.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_EventDesc, "f_EventDesc");
			this.f_EventDesc.Name = "f_EventDesc";
			this.f_EventDesc.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnClose;
			base.Controls.Add(this.button1);
			base.Controls.Add(this.btnExportExcel);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.dgvMain);
			base.Name = "dfrmLogQuery";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmLogQuery_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmLogQuery_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmLogQuery_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x040002ED RID: 749
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002EE RID: 750
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040002EF RID: 751
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x040002F0 RID: 752
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040002F1 RID: 753
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EventDesc;

		// Token: 0x040002F2 RID: 754
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EventType;

		// Token: 0x040002F3 RID: 755
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_LogDateTime;

		// Token: 0x040002F4 RID: 756
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x040002F5 RID: 757
		internal global::System.Windows.Forms.Button btnExportExcel;

		// Token: 0x040002F6 RID: 758
		internal global::System.Windows.Forms.Button btnFind;

		// Token: 0x040002F7 RID: 759
		internal global::System.Windows.Forms.Button button1;
	}
}
