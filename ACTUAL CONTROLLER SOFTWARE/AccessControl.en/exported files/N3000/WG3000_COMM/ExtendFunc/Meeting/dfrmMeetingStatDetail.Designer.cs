namespace WG3000_COMM.ExtendFunc.Meeting
{
	// Token: 0x02000301 RID: 769
	public partial class dfrmMeetingStatDetail : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060016E4 RID: 5860 RVA: 0x001DB674 File Offset: 0x001DA674
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x001DB694 File Offset: 0x001DA694
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meeting.dfrmMeetingStatDetail));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.tabPage4 = new global::System.Windows.Forms.TabPage();
			this.tabPage5 = new global::System.Windows.Forms.TabPage();
			this.tabPage6 = new global::System.Windows.Forms.TabPage();
			this.btnLeave = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnManualSign = new global::System.Windows.Forms.Button();
			this.btnRecreate = new global::System.Windows.Forms.Button();
			this.btnPrint = new global::System.Windows.Forms.Button();
			this.btnExport = new global::System.Windows.Forms.Button();
			this.btnRefresh = new global::System.Windows.Forms.Button();
			this.grpExt = new global::System.Windows.Forms.GroupBox();
			this.dgvStat = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.InFact = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_ManualCardRecordID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Identity = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SeatNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SignRealTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl1.SuspendLayout();
			this.grpExt.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvStat).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.SelectedIndexChanged += new global::System.EventHandler(this.tabControl1_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.Transparent;
			this.tabPage1.Name = "tabPage1";
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage6, "tabPage6");
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnLeave, "btnLeave");
			this.btnLeave.BackColor = global::System.Drawing.Color.Transparent;
			this.btnLeave.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnLeave.ForeColor = global::System.Drawing.Color.White;
			this.btnLeave.Name = "btnLeave";
			this.btnLeave.UseVisualStyleBackColor = false;
			this.btnLeave.Click += new global::System.EventHandler(this.btnLeave_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.btnManualSign, "btnManualSign");
			this.btnManualSign.BackColor = global::System.Drawing.Color.Transparent;
			this.btnManualSign.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnManualSign.ForeColor = global::System.Drawing.Color.White;
			this.btnManualSign.Name = "btnManualSign";
			this.btnManualSign.UseVisualStyleBackColor = false;
			this.btnManualSign.Click += new global::System.EventHandler(this.btnManualSign_Click);
			componentResourceManager.ApplyResources(this.btnRecreate, "btnRecreate");
			this.btnRecreate.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRecreate.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRecreate.ForeColor = global::System.Drawing.Color.White;
			this.btnRecreate.Name = "btnRecreate";
			this.btnRecreate.UseVisualStyleBackColor = false;
			this.btnRecreate.Click += new global::System.EventHandler(this.btnRecreate_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.BackColor = global::System.Drawing.Color.Transparent;
			this.btnPrint.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.UseVisualStyleBackColor = false;
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExport.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Name = "btnExport";
			this.btnExport.UseVisualStyleBackColor = false;
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.btnRefresh, "btnRefresh");
			this.btnRefresh.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRefresh.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRefresh.ForeColor = global::System.Drawing.Color.White;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new global::System.EventHandler(this.btnRefresh_Click);
			componentResourceManager.ApplyResources(this.grpExt, "grpExt");
			this.grpExt.BackColor = global::System.Drawing.Color.Transparent;
			this.grpExt.Controls.Add(this.dgvStat);
			this.grpExt.Controls.Add(this.dgvMain);
			this.grpExt.ForeColor = global::System.Drawing.Color.White;
			this.grpExt.Name = "grpExt";
			this.grpExt.TabStop = false;
			componentResourceManager.ApplyResources(this.dgvStat, "dgvStat");
			this.dgvStat.AllowUserToAddRows = false;
			this.dgvStat.AllowUserToDeleteRows = false;
			this.dgvStat.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvStat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvStat.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.InFact, this.Column1, this.Column2, this.Column3, this.Column4 });
			this.dgvStat.EnableHeadersVisualStyles = false;
			this.dgvStat.Name = "dgvStat";
			this.dgvStat.ReadOnly = true;
			this.dgvStat.RowHeadersVisible = false;
			this.dgvStat.RowTemplate.Height = 23;
			this.dgvStat.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.InFact, "InFact");
			this.InFact.Name = "InFact";
			this.InFact.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ManualCardRecordID, this.f_ConsumerName, this.f_Identity, this.f_SeatNO, this.f_SignRealTime, this.f_Notes });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowHeadersVisible = false;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ManualCardRecordID.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_ManualCardRecordID, "f_ManualCardRecordID");
			this.f_ManualCardRecordID.Name = "f_ManualCardRecordID";
			this.f_ManualCardRecordID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Identity, "f_Identity");
			this.f_Identity.Name = "f_Identity";
			this.f_Identity.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_SeatNO.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_SeatNO, "f_SeatNO");
			this.f_SeatNO.Name = "f_SeatNO";
			this.f_SeatNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SignRealTime, "f_SignRealTime");
			this.f_SignRealTime.Name = "f_SignRealTime";
			this.f_SignRealTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.btnLeave);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnManualSign);
			base.Controls.Add(this.btnRecreate);
			base.Controls.Add(this.btnPrint);
			base.Controls.Add(this.btnExport);
			base.Controls.Add(this.btnRefresh);
			base.Controls.Add(this.grpExt);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMeetingStatDetail";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmMeetingStatDetail_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmStd_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmMeetingStatDetail_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.grpExt.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvStat).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04002F82 RID: 12162
		private global::System.ComponentModel.Container components;

		// Token: 0x04002F83 RID: 12163
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04002F84 RID: 12164
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04002F85 RID: 12165
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04002F86 RID: 12166
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x04002F87 RID: 12167
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002F88 RID: 12168
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002F89 RID: 12169
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04002F8A RID: 12170
		private global::System.Windows.Forms.DataGridView dgvStat;

		// Token: 0x04002F8B RID: 12171
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04002F8C RID: 12172
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Identity;

		// Token: 0x04002F8D RID: 12173
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ManualCardRecordID;

		// Token: 0x04002F8E RID: 12174
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04002F8F RID: 12175
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SeatNO;

		// Token: 0x04002F90 RID: 12176
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SignRealTime;

		// Token: 0x04002F91 RID: 12177
		private global::System.Windows.Forms.GroupBox grpExt;

		// Token: 0x04002F92 RID: 12178
		private global::System.Windows.Forms.DataGridViewTextBoxColumn InFact;

		// Token: 0x04002F93 RID: 12179
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002F94 RID: 12180
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002F95 RID: 12181
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04002F96 RID: 12182
		private global::System.Windows.Forms.TabPage tabPage4;

		// Token: 0x04002F97 RID: 12183
		private global::System.Windows.Forms.TabPage tabPage5;

		// Token: 0x04002F98 RID: 12184
		private global::System.Windows.Forms.TabPage tabPage6;

		// Token: 0x04002F99 RID: 12185
		internal global::System.Windows.Forms.Button btnExit;

		// Token: 0x04002F9A RID: 12186
		internal global::System.Windows.Forms.Button btnExport;

		// Token: 0x04002F9B RID: 12187
		internal global::System.Windows.Forms.Button btnLeave;

		// Token: 0x04002F9C RID: 12188
		internal global::System.Windows.Forms.Button btnManualSign;

		// Token: 0x04002F9D RID: 12189
		internal global::System.Windows.Forms.Button btnPrint;

		// Token: 0x04002F9E RID: 12190
		internal global::System.Windows.Forms.Button btnRecreate;

		// Token: 0x04002F9F RID: 12191
		internal global::System.Windows.Forms.Button btnRefresh;

		// Token: 0x04002FA0 RID: 12192
		public global::System.Windows.Forms.TabControl tabControl1;
	}
}
