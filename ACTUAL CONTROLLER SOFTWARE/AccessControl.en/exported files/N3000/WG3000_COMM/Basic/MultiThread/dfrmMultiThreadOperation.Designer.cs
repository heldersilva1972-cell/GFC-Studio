namespace WG3000_COMM.Basic.MultiThread
{
	// Token: 0x0200005A RID: 90
	public partial class dfrmMultiThreadOperation : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x000CB296 File Offset: 0x000CA296
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.wgudpOperateControllerCardLost != null)
			{
				this.wgudpOperateControllerCardLost.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000CB2CC File Offset: 0x000CA2CC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.MultiThread.dfrmMultiThreadOperation));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.exportToExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new global::System.Windows.Forms.StatusStrip();
			this.statSoftStarttime = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statTimeDate = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.lblDealingInfo = new global::System.Windows.Forms.Label();
			this.grpDoors = new global::System.Windows.Forms.GroupBox();
			this.txtDealingDoors = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.txtTotalDoors = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.txtFailedDoors = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.txtDownloadedDoors = new global::System.Windows.Forms.TextBox();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_DoorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Process = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Total = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Start = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EndTime = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Enabled = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Delay = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PORT = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_RunStatus = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorControl = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_cloudipinfo = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_time = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnOtherInfo = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnRetryMultithreadOperation = new global::System.Windows.Forms.Button();
			this.grpControllers = new global::System.Windows.Forms.GroupBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.txtTotalItems = new global::System.Windows.Forms.TextBox();
			this.txtAllControllerCnt = new global::System.Windows.Forms.TextBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.txtDealingControllers = new global::System.Windows.Forms.TextBox();
			this.txtFailedControllers = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.txtCompleteOK = new global::System.Windows.Forms.TextBox();
			this.contextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.grpDoors.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.grpControllers.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.exportToExcelToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.exportToExcelToolStripMenuItem, "exportToExcelToolStripMenuItem");
			this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
			this.exportToExcelToolStripMenuItem.Click += new global::System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.statusStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.statusStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.statSoftStarttime, this.toolStripStatusLabel2, this.statTimeDate });
			this.statusStrip1.Name = "statusStrip1";
			componentResourceManager.ApplyResources(this.statSoftStarttime, "statSoftStarttime");
			this.statSoftStarttime.BackColor = global::System.Drawing.Color.Transparent;
			this.statSoftStarttime.ForeColor = global::System.Drawing.Color.White;
			this.statSoftStarttime.Name = "statSoftStarttime";
			componentResourceManager.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
			this.toolStripStatusLabel2.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStripStatusLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Spring = true;
			componentResourceManager.ApplyResources(this.statTimeDate, "statTimeDate");
			this.statTimeDate.BackColor = global::System.Drawing.Color.Transparent;
			this.statTimeDate.ForeColor = global::System.Drawing.Color.White;
			this.statTimeDate.Image = global::WG3000_COMM.Properties.Resources.timequery;
			this.statTimeDate.Name = "statTimeDate";
			componentResourceManager.ApplyResources(this.lblDealingInfo, "lblDealingInfo");
			this.lblDealingInfo.BackColor = global::System.Drawing.Color.Transparent;
			this.lblDealingInfo.ForeColor = global::System.Drawing.Color.White;
			this.lblDealingInfo.Name = "lblDealingInfo";
			componentResourceManager.ApplyResources(this.grpDoors, "grpDoors");
			this.grpDoors.Controls.Add(this.txtDealingDoors);
			this.grpDoors.Controls.Add(this.label9);
			this.grpDoors.Controls.Add(this.txtTotalDoors);
			this.grpDoors.Controls.Add(this.label6);
			this.grpDoors.Controls.Add(this.txtFailedDoors);
			this.grpDoors.Controls.Add(this.label5);
			this.grpDoors.Controls.Add(this.label4);
			this.grpDoors.Controls.Add(this.txtDownloadedDoors);
			this.grpDoors.ForeColor = global::System.Drawing.Color.White;
			this.grpDoors.Name = "grpDoors";
			this.grpDoors.TabStop = false;
			componentResourceManager.ApplyResources(this.txtDealingDoors, "txtDealingDoors");
			this.txtDealingDoors.Name = "txtDealingDoors";
			this.txtDealingDoors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.txtTotalDoors, "txtTotalDoors");
			this.txtTotalDoors.Name = "txtTotalDoors";
			this.txtTotalDoors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.BackColor = global::System.Drawing.Color.Transparent;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.txtFailedDoors, "txtFailedDoors");
			this.txtFailedDoors.Name = "txtFailedDoors";
			this.txtFailedDoors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.BackColor = global::System.Drawing.Color.Transparent;
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.txtDownloadedDoors, "txtDownloadedDoors");
			this.txtDownloadedDoors.Name = "txtDownloadedDoors";
			this.txtDownloadedDoors.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_DoorID, this.f_DoorName, this.f_Process, this.f_Total, this.f_Start, this.f_EndTime, this.f_Enabled, this.f_Delay, this.f_ControllerNO, this.f_ControllerSN,
				this.f_DoorNO, this.f_IP, this.f_PORT, this.f_ZoneName, this.f_RunStatus, this.f_DoorControl, this.f_ControllerID, this.f_RecID, this.f_cloudipinfo, this.f_time
			});
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersBorderStyle = global::System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(146, 150, 177);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Pixel, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_DoorID, "f_DoorID");
			this.f_DoorID.Name = "f_DoorID";
			this.f_DoorID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			this.f_Process.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Process, "f_Process");
			this.f_Process.Name = "f_Process";
			this.f_Process.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Total, "f_Total");
			this.f_Total.Name = "f_Total";
			this.f_Total.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Start, "f_Start");
			this.f_Start.Name = "f_Start";
			this.f_Start.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_EndTime, "f_EndTime");
			this.f_EndTime.Name = "f_EndTime";
			this.f_EndTime.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.Black;
			dataGridViewCellStyle3.NullValue = false;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			this.f_Enabled.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_Enabled, "f_Enabled");
			this.f_Enabled.Name = "f_Enabled";
			this.f_Enabled.ReadOnly = true;
			this.f_Enabled.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Enabled.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_Delay, "f_Delay");
			this.f_Delay.Name = "f_Delay";
			this.f_Delay.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerNO.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_ControllerNO, "f_ControllerNO");
			this.f_ControllerNO.Name = "f_ControllerNO";
			this.f_ControllerNO.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorNO, "f_DoorNO");
			this.f_DoorNO.Name = "f_DoorNO";
			this.f_DoorNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_IP, "f_IP");
			this.f_IP.Name = "f_IP";
			this.f_IP.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_PORT.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.f_PORT, "f_PORT");
			this.f_PORT.Name = "f_PORT";
			this.f_PORT.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ZoneName, "f_ZoneName");
			this.f_ZoneName.Name = "f_ZoneName";
			this.f_ZoneName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_RunStatus, "f_RunStatus");
			this.f_RunStatus.Name = "f_RunStatus";
			this.f_RunStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorControl, "f_DoorControl");
			this.f_DoorControl.Name = "f_DoorControl";
			this.f_DoorControl.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ControllerID, "f_ControllerID");
			this.f_ControllerID.Name = "f_ControllerID";
			this.f_ControllerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_cloudipinfo, "f_cloudipinfo");
			this.f_cloudipinfo.Name = "f_cloudipinfo";
			this.f_cloudipinfo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_time, "f_time");
			this.f_time.Name = "f_time";
			this.f_time.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnOtherInfo, "btnOtherInfo");
			this.btnOtherInfo.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOtherInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOtherInfo.ForeColor = global::System.Drawing.Color.White;
			this.btnOtherInfo.Name = "btnOtherInfo";
			this.btnOtherInfo.UseVisualStyleBackColor = false;
			this.btnOtherInfo.Click += new global::System.EventHandler(this.btnOtherInfo_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnRetryMultithreadOperation, "btnRetryMultithreadOperation");
			this.btnRetryMultithreadOperation.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRetryMultithreadOperation.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRetryMultithreadOperation.ForeColor = global::System.Drawing.Color.White;
			this.btnRetryMultithreadOperation.Name = "btnRetryMultithreadOperation";
			this.btnRetryMultithreadOperation.UseVisualStyleBackColor = false;
			this.btnRetryMultithreadOperation.Click += new global::System.EventHandler(this.btnRetryMultithreadOperation_Click);
			componentResourceManager.ApplyResources(this.grpControllers, "grpControllers");
			this.grpControllers.Controls.Add(this.label1);
			this.grpControllers.Controls.Add(this.label2);
			this.grpControllers.Controls.Add(this.label3);
			this.grpControllers.Controls.Add(this.txtTotalItems);
			this.grpControllers.Controls.Add(this.txtAllControllerCnt);
			this.grpControllers.Controls.Add(this.label8);
			this.grpControllers.Controls.Add(this.txtDealingControllers);
			this.grpControllers.Controls.Add(this.txtFailedControllers);
			this.grpControllers.Controls.Add(this.label7);
			this.grpControllers.Controls.Add(this.txtCompleteOK);
			this.grpControllers.ForeColor = global::System.Drawing.Color.White;
			this.grpControllers.Name = "grpControllers";
			this.grpControllers.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtTotalItems, "txtTotalItems");
			this.txtTotalItems.Name = "txtTotalItems";
			this.txtTotalItems.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtAllControllerCnt, "txtAllControllerCnt");
			this.txtAllControllerCnt.Name = "txtAllControllerCnt";
			this.txtAllControllerCnt.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.BackColor = global::System.Drawing.Color.Transparent;
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.txtDealingControllers, "txtDealingControllers");
			this.txtDealingControllers.Name = "txtDealingControllers";
			this.txtDealingControllers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtFailedControllers, "txtFailedControllers");
			this.txtFailedControllers.Name = "txtFailedControllers";
			this.txtFailedControllers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.BackColor = global::System.Drawing.Color.Transparent;
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.txtCompleteOK, "txtCompleteOK");
			this.txtCompleteOK.Name = "txtCompleteOK";
			this.txtCompleteOK.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.lblDealingInfo);
			base.Controls.Add(this.grpDoors);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.btnOtherInfo);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnRetryMultithreadOperation);
			base.Controls.Add(this.grpControllers);
			base.Name = "dfrmMultiThreadOperation";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmMultiThreadOperation_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmMultiThreadOperation_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmMultiThreadOperation_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmMultiThreadOperation_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.grpDoors.ResumeLayout(false);
			this.grpDoors.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.grpControllers.ResumeLayout(false);
			this.grpControllers.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000CA6 RID: 3238
		private global::WG3000_COMM.Core.wgUdpComm wgudpOperateControllerCardLost;

		// Token: 0x04000CD7 RID: 3287
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000CD8 RID: 3288
		private global::System.Windows.Forms.Button btnRetryMultithreadOperation;

		// Token: 0x04000CD9 RID: 3289
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000CDA RID: 3290
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000CDB RID: 3291
		private global::System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;

		// Token: 0x04000CDC RID: 3292
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_cloudipinfo;

		// Token: 0x04000CDD RID: 3293
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerID;

		// Token: 0x04000CDE RID: 3294
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerNO;

		// Token: 0x04000CDF RID: 3295
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04000CE0 RID: 3296
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Delay;

		// Token: 0x04000CE1 RID: 3297
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorControl;

		// Token: 0x04000CE2 RID: 3298
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorID;

		// Token: 0x04000CE3 RID: 3299
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x04000CE4 RID: 3300
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNO;

		// Token: 0x04000CE5 RID: 3301
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Enabled;

		// Token: 0x04000CE6 RID: 3302
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EndTime;

		// Token: 0x04000CE7 RID: 3303
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04000CE8 RID: 3304
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x04000CE9 RID: 3305
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Process;

		// Token: 0x04000CEA RID: 3306
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04000CEB RID: 3307
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RunStatus;

		// Token: 0x04000CEC RID: 3308
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Start;

		// Token: 0x04000CED RID: 3309
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_time;

		// Token: 0x04000CEE RID: 3310
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Total;

		// Token: 0x04000CEF RID: 3311
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneName;

		// Token: 0x04000CF0 RID: 3312
		private global::System.Windows.Forms.GroupBox grpControllers;

		// Token: 0x04000CF1 RID: 3313
		private global::System.Windows.Forms.GroupBox grpDoors;

		// Token: 0x04000CF2 RID: 3314
		private global::System.Windows.Forms.ToolStripStatusLabel statSoftStarttime;

		// Token: 0x04000CF3 RID: 3315
		private global::System.Windows.Forms.ToolStripStatusLabel statTimeDate;

		// Token: 0x04000CF4 RID: 3316
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x04000CF5 RID: 3317
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000CF6 RID: 3318
		private global::System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;

		// Token: 0x04000CF7 RID: 3319
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000CF8 RID: 3320
		internal global::System.Windows.Forms.Button btnOtherInfo;

		// Token: 0x04000CF9 RID: 3321
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x04000CFA RID: 3322
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x04000CFB RID: 3323
		internal global::System.Windows.Forms.Label label3;

		// Token: 0x04000CFC RID: 3324
		internal global::System.Windows.Forms.Label label4;

		// Token: 0x04000CFD RID: 3325
		internal global::System.Windows.Forms.Label label5;

		// Token: 0x04000CFE RID: 3326
		internal global::System.Windows.Forms.Label label6;

		// Token: 0x04000CFF RID: 3327
		internal global::System.Windows.Forms.Label label7;

		// Token: 0x04000D00 RID: 3328
		internal global::System.Windows.Forms.Label label8;

		// Token: 0x04000D01 RID: 3329
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x04000D02 RID: 3330
		internal global::System.Windows.Forms.Label lblDealingInfo;

		// Token: 0x04000D03 RID: 3331
		internal global::System.Windows.Forms.TextBox txtAllControllerCnt;

		// Token: 0x04000D04 RID: 3332
		internal global::System.Windows.Forms.TextBox txtCompleteOK;

		// Token: 0x04000D05 RID: 3333
		internal global::System.Windows.Forms.TextBox txtDealingControllers;

		// Token: 0x04000D06 RID: 3334
		internal global::System.Windows.Forms.TextBox txtDealingDoors;

		// Token: 0x04000D07 RID: 3335
		internal global::System.Windows.Forms.TextBox txtDownloadedDoors;

		// Token: 0x04000D08 RID: 3336
		internal global::System.Windows.Forms.TextBox txtFailedControllers;

		// Token: 0x04000D09 RID: 3337
		internal global::System.Windows.Forms.TextBox txtFailedDoors;

		// Token: 0x04000D0A RID: 3338
		internal global::System.Windows.Forms.TextBox txtTotalDoors;

		// Token: 0x04000D0B RID: 3339
		internal global::System.Windows.Forms.TextBox txtTotalItems;
	}
}
