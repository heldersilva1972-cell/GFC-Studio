namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000312 RID: 786
	public partial class frmPatrolTaskData : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001815 RID: 6165 RVA: 0x001F4616 File Offset: 0x001F3616
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x001F4638 File Offset: 0x001F3638
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.frmPatrolTaskData));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DepartmentName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DateYM = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_01 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_02 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OnDuty1Short = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_04 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_05 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_06 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_07 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_08 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_09 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_10 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_11 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_12 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_13 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_14 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_15 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_16 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_17 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_18 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_19 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_20 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_21 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_22 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_23 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_24 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_25 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_26 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_27 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_28 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_29 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_30 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ShiftID_31 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip3 = new global::System.Windows.Forms.ToolStrip();
			this.toolStripLabel2 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel3 = new global::System.Windows.Forms.ToolStripLabel();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnClear = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnExit = new global::System.Windows.Forms.ToolStripButton();
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFindSecond();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			this.toolStrip3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_RecID, this.f_DepartmentName, this.f_ConsumerNO, this.f_ConsumerName, this.f_DateYM, this.f_ShiftID_01, this.f_ShiftID_02, this.f_OnDuty1Short, this.f_ShiftID_04, this.f_ShiftID_05,
				this.f_ShiftID_06, this.f_ShiftID_07, this.f_ShiftID_08, this.f_ShiftID_09, this.f_ShiftID_10, this.f_ShiftID_11, this.f_ShiftID_12, this.f_ShiftID_13, this.f_ShiftID_14, this.f_ShiftID_15,
				this.f_ShiftID_16, this.f_ShiftID_17, this.f_ShiftID_18, this.f_ShiftID_19, this.f_ShiftID_20, this.f_ShiftID_21, this.f_ShiftID_22, this.f_ShiftID_23, this.f_ShiftID_24, this.f_ShiftID_25,
				this.f_ShiftID_26, this.f_ShiftID_27, this.f_ShiftID_28, this.f_ShiftID_29, this.f_ShiftID_30, this.f_ShiftID_31, this.f_ConsumerID
			});
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgvMain.CellDoubleClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMain_CellDoubleClick);
			this.dgvMain.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMain_CellFormatting);
			this.dgvMain.Scroll += new global::System.Windows.Forms.ScrollEventHandler(this.dgvMain_Scroll);
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_RecID.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DepartmentName, "f_DepartmentName");
			this.f_DepartmentName.Name = "f_DepartmentName";
			this.f_DepartmentName.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ConsumerNO.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_ConsumerNO, "f_ConsumerNO");
			this.f_ConsumerNO.Name = "f_ConsumerNO";
			this.f_ConsumerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerName, "f_ConsumerName");
			this.f_ConsumerName.Name = "f_ConsumerName";
			this.f_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DateYM, "f_DateYM");
			this.f_DateYM.Name = "f_DateYM";
			this.f_DateYM.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_01, "f_ShiftID_01");
			this.f_ShiftID_01.Name = "f_ShiftID_01";
			this.f_ShiftID_01.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_02, "f_ShiftID_02");
			this.f_ShiftID_02.Name = "f_ShiftID_02";
			this.f_ShiftID_02.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_OnDuty1Short, "f_OnDuty1Short");
			this.f_OnDuty1Short.Name = "f_OnDuty1Short";
			this.f_OnDuty1Short.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_04, "f_ShiftID_04");
			this.f_ShiftID_04.Name = "f_ShiftID_04";
			this.f_ShiftID_04.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_05, "f_ShiftID_05");
			this.f_ShiftID_05.Name = "f_ShiftID_05";
			this.f_ShiftID_05.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_06, "f_ShiftID_06");
			this.f_ShiftID_06.Name = "f_ShiftID_06";
			this.f_ShiftID_06.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_07, "f_ShiftID_07");
			this.f_ShiftID_07.Name = "f_ShiftID_07";
			this.f_ShiftID_07.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_08, "f_ShiftID_08");
			this.f_ShiftID_08.Name = "f_ShiftID_08";
			this.f_ShiftID_08.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_09, "f_ShiftID_09");
			this.f_ShiftID_09.Name = "f_ShiftID_09";
			this.f_ShiftID_09.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_10, "f_ShiftID_10");
			this.f_ShiftID_10.Name = "f_ShiftID_10";
			this.f_ShiftID_10.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_11, "f_ShiftID_11");
			this.f_ShiftID_11.Name = "f_ShiftID_11";
			this.f_ShiftID_11.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_12, "f_ShiftID_12");
			this.f_ShiftID_12.Name = "f_ShiftID_12";
			this.f_ShiftID_12.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_13, "f_ShiftID_13");
			this.f_ShiftID_13.Name = "f_ShiftID_13";
			this.f_ShiftID_13.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_14, "f_ShiftID_14");
			this.f_ShiftID_14.Name = "f_ShiftID_14";
			this.f_ShiftID_14.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_15, "f_ShiftID_15");
			this.f_ShiftID_15.Name = "f_ShiftID_15";
			this.f_ShiftID_15.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_16, "f_ShiftID_16");
			this.f_ShiftID_16.Name = "f_ShiftID_16";
			this.f_ShiftID_16.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_17, "f_ShiftID_17");
			this.f_ShiftID_17.Name = "f_ShiftID_17";
			this.f_ShiftID_17.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_18, "f_ShiftID_18");
			this.f_ShiftID_18.Name = "f_ShiftID_18";
			this.f_ShiftID_18.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_19, "f_ShiftID_19");
			this.f_ShiftID_19.Name = "f_ShiftID_19";
			this.f_ShiftID_19.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_20, "f_ShiftID_20");
			this.f_ShiftID_20.Name = "f_ShiftID_20";
			this.f_ShiftID_20.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_21, "f_ShiftID_21");
			this.f_ShiftID_21.Name = "f_ShiftID_21";
			this.f_ShiftID_21.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_22, "f_ShiftID_22");
			this.f_ShiftID_22.Name = "f_ShiftID_22";
			this.f_ShiftID_22.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_23, "f_ShiftID_23");
			this.f_ShiftID_23.Name = "f_ShiftID_23";
			this.f_ShiftID_23.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_24, "f_ShiftID_24");
			this.f_ShiftID_24.Name = "f_ShiftID_24";
			this.f_ShiftID_24.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_25, "f_ShiftID_25");
			this.f_ShiftID_25.Name = "f_ShiftID_25";
			this.f_ShiftID_25.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_26, "f_ShiftID_26");
			this.f_ShiftID_26.Name = "f_ShiftID_26";
			this.f_ShiftID_26.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_27, "f_ShiftID_27");
			this.f_ShiftID_27.Name = "f_ShiftID_27";
			this.f_ShiftID_27.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_28, "f_ShiftID_28");
			this.f_ShiftID_28.Name = "f_ShiftID_28";
			this.f_ShiftID_28.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_29, "f_ShiftID_29");
			this.f_ShiftID_29.Name = "f_ShiftID_29";
			this.f_ShiftID_29.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_30, "f_ShiftID_30");
			this.f_ShiftID_30.Name = "f_ShiftID_30";
			this.f_ShiftID_30.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ShiftID_31, "f_ShiftID_31");
			this.f_ShiftID_31.Name = "f_ShiftID_31";
			this.f_ShiftID_31.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_ConsumerID, "f_ConsumerID");
			this.f_ConsumerID.Name = "f_ConsumerID";
			this.f_ConsumerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip3, "toolStrip3");
			this.toolStrip3.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.toolStrip3.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripLabel2, this.toolStripLabel3 });
			this.toolStrip3.Name = "toolStrip3";
			componentResourceManager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
			this.toolStripLabel2.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel2.Name = "toolStripLabel2";
			componentResourceManager.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
			this.toolStripLabel3.ForeColor = global::System.Drawing.Color.White;
			this.toolStripLabel3.Name = "toolStripLabel3";
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnClear, this.btnPrint, this.btnExportToExcel, this.btnExit });
			this.toolStrip1.Name = "toolStrip1";
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
			componentResourceManager.ApplyResources(this.btnClear, "btnClear");
			this.btnClear.ForeColor = global::System.Drawing.Color.White;
			this.btnClear.Image = global::WG3000_COMM.Properties.Resources.pTools_Clear_Condition;
			this.btnClear.Name = "btnClear";
			this.btnClear.Click += new global::System.EventHandler(this.btnClear_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Image = global::WG3000_COMM.Properties.Resources.pTools_Print;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
			this.btnExportToExcel.ForeColor = global::System.Drawing.Color.White;
			this.btnExportToExcel.Image = global::WG3000_COMM.Properties.Resources.pTools_ExportToExcel;
			this.btnExportToExcel.Name = "btnExportToExcel";
			this.btnExportToExcel.Click += new global::System.EventHandler(this.btnExportToExcel_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.btnExit.Name = "btnExit";
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
			this.userControlFind1.Name = "userControlFind1";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmPatrolTaskData";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmShiftAttReport_FormClosing);
			base.Load += new global::System.EventHandler(this.frmShiftOtherData_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400313B RID: 12603
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400313C RID: 12604
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x0400313D RID: 12605
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x0400313E RID: 12606
		private global::System.Windows.Forms.ToolStripButton btnClear;

		// Token: 0x0400313F RID: 12607
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04003140 RID: 12608
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04003141 RID: 12609
		private global::System.Windows.Forms.ToolStripButton btnExit;

		// Token: 0x04003142 RID: 12610
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04003143 RID: 12611
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04003144 RID: 12612
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x04003147 RID: 12615
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerID;

		// Token: 0x04003148 RID: 12616
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x04003149 RID: 12617
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x0400314A RID: 12618
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DateYM;

		// Token: 0x0400314B RID: 12619
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x0400314C RID: 12620
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty1Short;

		// Token: 0x0400314D RID: 12621
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x0400314E RID: 12622
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_01;

		// Token: 0x0400314F RID: 12623
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_02;

		// Token: 0x04003150 RID: 12624
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_04;

		// Token: 0x04003151 RID: 12625
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_05;

		// Token: 0x04003152 RID: 12626
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_06;

		// Token: 0x04003153 RID: 12627
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_07;

		// Token: 0x04003154 RID: 12628
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_08;

		// Token: 0x04003155 RID: 12629
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_09;

		// Token: 0x04003156 RID: 12630
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_10;

		// Token: 0x04003157 RID: 12631
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_11;

		// Token: 0x04003158 RID: 12632
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_12;

		// Token: 0x04003159 RID: 12633
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_13;

		// Token: 0x0400315A RID: 12634
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_14;

		// Token: 0x0400315B RID: 12635
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_15;

		// Token: 0x0400315C RID: 12636
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_16;

		// Token: 0x0400315D RID: 12637
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_17;

		// Token: 0x0400315E RID: 12638
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_18;

		// Token: 0x0400315F RID: 12639
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_19;

		// Token: 0x04003160 RID: 12640
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_20;

		// Token: 0x04003161 RID: 12641
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_21;

		// Token: 0x04003162 RID: 12642
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_22;

		// Token: 0x04003163 RID: 12643
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_23;

		// Token: 0x04003164 RID: 12644
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_24;

		// Token: 0x04003165 RID: 12645
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_25;

		// Token: 0x04003166 RID: 12646
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_26;

		// Token: 0x04003167 RID: 12647
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_27;

		// Token: 0x04003168 RID: 12648
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_28;

		// Token: 0x04003169 RID: 12649
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_29;

		// Token: 0x0400316A RID: 12650
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_30;

		// Token: 0x0400316B RID: 12651
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_31;

		// Token: 0x0400316C RID: 12652
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x0400316D RID: 12653
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x0400316E RID: 12654
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x0400316F RID: 12655
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x04003170 RID: 12656
		private global::WG3000_COMM.Core.UserControlFindSecond userControlFind1;
	}
}
