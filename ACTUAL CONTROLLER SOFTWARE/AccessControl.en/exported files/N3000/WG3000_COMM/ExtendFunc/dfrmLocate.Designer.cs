namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000243 RID: 579
	public partial class dfrmLocate : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060011ED RID: 4589 RVA: 0x0015210E File Offset: 0x0015110E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.controller4Locate != null)
			{
				this.controller4Locate.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00152144 File Offset: 0x00151144
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmLocate));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.progressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnQuery = new global::System.Windows.Forms.Button();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.button1 = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.txtLocate = new global::System.Windows.Forms.RichTextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.backgroundWorker2 = new global::System.ComponentModel.BackgroundWorker();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.toolTip1.SetToolTip(this.progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnQuery, "btnQuery");
			this.btnQuery.BackColor = global::System.Drawing.Color.Transparent;
			this.btnQuery.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnQuery.ForeColor = global::System.Drawing.Color.White;
			this.btnQuery.Name = "btnQuery";
			this.toolTip1.SetToolTip(this.btnQuery, componentResourceManager.GetString("btnQuery.ToolTip"));
			this.btnQuery.UseVisualStyleBackColor = false;
			this.btnQuery.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.ForeColor = global::System.Drawing.Color.White;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.UserID, this.ConsumerName, this.CardNO, this.f_GroupID, this.f_SelectedUsers });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.MultiSelect = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dgvUsers_KeyDown);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.toolTip1.SetToolTip(this.button1, componentResourceManager.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.txtLocate, "txtLocate");
			this.txtLocate.BackColor = global::System.Drawing.Color.White;
			this.txtLocate.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtLocate.ForeColor = global::System.Drawing.Color.Black;
			this.txtLocate.Name = "txtLocate";
			this.toolTip1.SetToolTip(this.txtLocate, componentResourceManager.GetString("txtLocate.ToolTip"));
			this.txtLocate.TextChanged += new global::System.EventHandler(this.txtLocate_TextChanged);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.Yellow;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			this.backgroundWorker2.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
			this.backgroundWorker2.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txtLocate);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.btnQuery);
			base.Controls.Add(this.lblWait);
			base.Controls.Add(this.dgvUsers);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.btnExit);
			base.Name = "dfrmLocate";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmPrivilegeCopy_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeCopy_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmLocate_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002037 RID: 8247
		private global::WG3000_COMM.DataOper.icController controller4Locate = new global::WG3000_COMM.DataOper.icController();

		// Token: 0x0400203B RID: 8251
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400203C RID: 8252
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x0400203D RID: 8253
		private global::System.ComponentModel.BackgroundWorker backgroundWorker2;

		// Token: 0x0400203E RID: 8254
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x0400203F RID: 8255
		private global::System.Windows.Forms.Button btnQuery;

		// Token: 0x04002040 RID: 8256
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04002041 RID: 8257
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x04002042 RID: 8258
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04002043 RID: 8259
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x04002044 RID: 8260
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x04002045 RID: 8261
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04002046 RID: 8262
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04002047 RID: 8263
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x04002048 RID: 8264
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002049 RID: 8265
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400204A RID: 8266
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x0400204B RID: 8267
		private global::System.Windows.Forms.ProgressBar progressBar1;

		// Token: 0x0400204C RID: 8268
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400204D RID: 8269
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400204E RID: 8270
		private global::System.Windows.Forms.RichTextBox txtLocate;

		// Token: 0x0400204F RID: 8271
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;
	}
}
