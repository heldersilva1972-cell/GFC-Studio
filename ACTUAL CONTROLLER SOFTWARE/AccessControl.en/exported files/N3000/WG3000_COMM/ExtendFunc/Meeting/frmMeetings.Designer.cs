namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x02000302 RID: 770
	public partial class frmMeetings : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060016FA RID: 5882 RVA: 0x001DD1FE File Offset: 0x001DC1FE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x001DD220 File Offset: 0x001DC220
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.frmMeetings));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.MeetingNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MeetingName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MeetingTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Addr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Content = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAddress = new global::System.Windows.Forms.ToolStripButton();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExport = new global::System.Windows.Forms.ToolStripButton();
			this.btnStat = new global::System.Windows.Forms.ToolStripButton();
			this.btnRealtimeSign = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
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
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.MeetingNO, this.MeetingName, this.MeetingTime, this.Addr, this.Content, this.Notes });
			this.dgvMain.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.MultiSelect = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMain.DoubleClick += new global::System.EventHandler(this.dgvMain_DoubleClick);
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.MeetingNO.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.MeetingNO, "MeetingNO");
			this.MeetingNO.Name = "MeetingNO";
			this.MeetingNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.MeetingName, "MeetingName");
			this.MeetingName.Name = "MeetingName";
			this.MeetingName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.MeetingTime, "MeetingTime");
			this.MeetingTime.Name = "MeetingTime";
			this.MeetingTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Addr, "Addr");
			this.Addr.Name = "Addr";
			this.Addr.ReadOnly = true;
			this.Content.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Content, "Content");
			this.Content.Name = "Content";
			this.Content.ReadOnly = true;
			this.Notes.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Notes, "Notes");
			this.Notes.Name = "Notes";
			this.Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.copyToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Click += new global::System.EventHandler(this.copyToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAddress, this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExport, this.btnStat, this.btnRealtimeSign, this.btnFind, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnAddress, "btnAddress");
			this.btnAddress.ForeColor = global::System.Drawing.Color.White;
			this.btnAddress.Image = global::WG3000_COMM.Properties.Resources.pTools_TypeSetup;
			this.btnAddress.Name = "btnAddress";
			this.btnAddress.Click += new global::System.EventHandler(this.btnAddress_Click);
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
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Image = global::WG3000_COMM.Properties.Resources.pTools_Print;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExport.Name = "btnExport";
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.btnStat, "btnStat");
			this.btnStat.ForeColor = global::System.Drawing.Color.White;
			this.btnStat.Image = global::WG3000_COMM.Properties.Resources.pTools_StatisticsReport;
			this.btnStat.Name = "btnStat";
			this.btnStat.Click += new global::System.EventHandler(this.btnStat_Click);
			componentResourceManager.ApplyResources(this.btnRealtimeSign, "btnRealtimeSign");
			this.btnRealtimeSign.ForeColor = global::System.Drawing.Color.White;
			this.btnRealtimeSign.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit_Batch;
			this.btnRealtimeSign.Name = "btnRealtimeSign";
			this.btnRealtimeSign.Click += new global::System.EventHandler(this.btnRealtimeSign_Click);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.btnExit.Name = "btnExit";
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmMeetings";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmMeetings_FormClosing);
			base.Load += new global::System.EventHandler(this.frmMeetings_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmMeetings_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002FA3 RID: 12195
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002FA4 RID: 12196
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Addr;

		// Token: 0x04002FA5 RID: 12197
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04002FA6 RID: 12198
		private global::System.Windows.Forms.ToolStripButton btnAddress;

		// Token: 0x04002FA7 RID: 12199
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04002FA8 RID: 12200
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04002FA9 RID: 12201
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x04002FAA RID: 12202
		private global::System.Windows.Forms.ToolStripButton btnExport;

		// Token: 0x04002FAB RID: 12203
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04002FAC RID: 12204
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04002FAD RID: 12205
		private global::System.Windows.Forms.ToolStripButton btnRealtimeSign;

		// Token: 0x04002FAE RID: 12206
		private global::System.Windows.Forms.ToolStripButton btnStat;

		// Token: 0x04002FAF RID: 12207
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Content;

		// Token: 0x04002FB0 RID: 12208
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002FB1 RID: 12209
		private global::System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;

		// Token: 0x04002FB2 RID: 12210
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04002FB3 RID: 12211
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MeetingName;

		// Token: 0x04002FB4 RID: 12212
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MeetingNO;

		// Token: 0x04002FB5 RID: 12213
		private global::System.Windows.Forms.DataGridViewTextBoxColumn MeetingTime;

		// Token: 0x04002FB6 RID: 12214
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Notes;

		// Token: 0x04002FB7 RID: 12215
		private global::System.Windows.Forms.ToolStrip toolStrip1;
	}
}
