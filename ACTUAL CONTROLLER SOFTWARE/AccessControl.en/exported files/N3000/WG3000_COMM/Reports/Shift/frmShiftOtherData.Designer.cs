namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200037F RID: 895
	public partial class frmShiftOtherData : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001DD5 RID: 7637 RVA: 0x00276302 File Offset: 0x00275302
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00276324 File Offset: 0x00275324
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.frmShiftOtherData));
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
			this.userControlFind1 = new global::WG3000_COMM.Core.UserControlFind4Shift();
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
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
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
			componentResourceManager.ApplyResources(this.userControlFind1, "userControlFind1");
			this.userControlFind1.BackColor = global::System.Drawing.Color.Transparent;
			this.userControlFind1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pTools_second_title;
			this.userControlFind1.ForeColor = global::System.Drawing.Color.White;
			this.userControlFind1.Name = "userControlFind1";
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
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnAdd, this.btnEdit, this.btnDelete, this.btnClear, this.btnPrint, this.btnExportToExcel, this.btnFind });
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
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Image = global::WG3000_COMM.Properties.Resources.pTools_Query;
			this.btnFind.Name = "btnFind";
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.userControlFind1);
			base.Controls.Add(this.toolStrip3);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmShiftOtherData";
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

		// Token: 0x040038E5 RID: 14565
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040038E6 RID: 14566
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x040038E7 RID: 14567
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x040038E8 RID: 14568
		private global::System.Windows.Forms.ToolStripButton btnClear;

		// Token: 0x040038E9 RID: 14569
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x040038EA RID: 14570
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x040038EB RID: 14571
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x040038EC RID: 14572
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x040038ED RID: 14573
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x040038EE RID: 14574
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040038F1 RID: 14577
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerID;

		// Token: 0x040038F2 RID: 14578
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerName;

		// Token: 0x040038F3 RID: 14579
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ConsumerNO;

		// Token: 0x040038F4 RID: 14580
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DateYM;

		// Token: 0x040038F5 RID: 14581
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DepartmentName;

		// Token: 0x040038F6 RID: 14582
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OnDuty1Short;

		// Token: 0x040038F7 RID: 14583
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x040038F8 RID: 14584
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_01;

		// Token: 0x040038F9 RID: 14585
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_02;

		// Token: 0x040038FA RID: 14586
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_04;

		// Token: 0x040038FB RID: 14587
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_05;

		// Token: 0x040038FC RID: 14588
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_06;

		// Token: 0x040038FD RID: 14589
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_07;

		// Token: 0x040038FE RID: 14590
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_08;

		// Token: 0x040038FF RID: 14591
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_09;

		// Token: 0x04003900 RID: 14592
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_10;

		// Token: 0x04003901 RID: 14593
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_11;

		// Token: 0x04003902 RID: 14594
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_12;

		// Token: 0x04003903 RID: 14595
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_13;

		// Token: 0x04003904 RID: 14596
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_14;

		// Token: 0x04003905 RID: 14597
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_15;

		// Token: 0x04003906 RID: 14598
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_16;

		// Token: 0x04003907 RID: 14599
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_17;

		// Token: 0x04003908 RID: 14600
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_18;

		// Token: 0x04003909 RID: 14601
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_19;

		// Token: 0x0400390A RID: 14602
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_20;

		// Token: 0x0400390B RID: 14603
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_21;

		// Token: 0x0400390C RID: 14604
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_22;

		// Token: 0x0400390D RID: 14605
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_23;

		// Token: 0x0400390E RID: 14606
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_24;

		// Token: 0x0400390F RID: 14607
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_25;

		// Token: 0x04003910 RID: 14608
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_26;

		// Token: 0x04003911 RID: 14609
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_27;

		// Token: 0x04003912 RID: 14610
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_28;

		// Token: 0x04003913 RID: 14611
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_29;

		// Token: 0x04003914 RID: 14612
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_30;

		// Token: 0x04003915 RID: 14613
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ShiftID_31;

		// Token: 0x04003916 RID: 14614
		private global::System.Windows.Forms.ToolStrip toolStrip1;

		// Token: 0x04003917 RID: 14615
		private global::System.Windows.Forms.ToolStrip toolStrip3;

		// Token: 0x04003918 RID: 14616
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel2;

		// Token: 0x04003919 RID: 14617
		private global::System.Windows.Forms.ToolStripLabel toolStripLabel3;

		// Token: 0x0400391A RID: 14618
		private global::WG3000_COMM.Core.UserControlFind4Shift userControlFind1;
	}
}
