namespace WG3000_COMM.Basic
{
	// Token: 0x0200004D RID: 77
	public partial class frmControllers : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0009C6DC File Offset: 0x0009B6DC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.dtController != null)
				{
					this.dtController.Dispose();
				}
				if (this.dv != null)
				{
					this.dv.Dispose();
				}
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0009C730 File Offset: 0x0009B730
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmControllers));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.batchUpdateSelectToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.doorsExportToExcelToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.cboUsers = new global::System.Windows.Forms.ComboBox();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.GroupNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorEnabled = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.DoorDelay = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ControllerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DoorNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvControllers = new global::System.Windows.Forms.DataGridView();
			this.f_ControllerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ControllerSN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Enabled = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_IP = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_PORT = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorNames = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip1 = new global::System.Windows.Forms.ToolStrip();
			this.btnSearchController = new global::System.Windows.Forms.ToolStripButton();
			this.btnAdd = new global::System.Windows.Forms.ToolStripButton();
			this.btnEdit = new global::System.Windows.Forms.ToolStripButton();
			this.btnDelete = new global::System.Windows.Forms.ToolStripButton();
			this.btnPrint = new global::System.Windows.Forms.ToolStripButton();
			this.btnExportToExcel = new global::System.Windows.Forms.ToolStripButton();
			this.btnFind = new global::System.Windows.Forms.ToolStripButton();
			this.cboZone = new global::System.Windows.Forms.ToolStripComboBox();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvControllers).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.batchUpdateSelectToolStripMenuItem, this.doorsExportToExcelToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.batchUpdateSelectToolStripMenuItem, "batchUpdateSelectToolStripMenuItem");
			this.batchUpdateSelectToolStripMenuItem.Name = "batchUpdateSelectToolStripMenuItem";
			this.batchUpdateSelectToolStripMenuItem.Click += new global::System.EventHandler(this.batchUpdateSelectToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.doorsExportToExcelToolStripMenuItem, "doorsExportToExcelToolStripMenuItem");
			this.doorsExportToExcelToolStripMenuItem.Name = "doorsExportToExcelToolStripMenuItem";
			this.doorsExportToExcelToolStripMenuItem.Click += new global::System.EventHandler(this.doorsExportToExcelToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.cboUsers, "cboUsers");
			this.cboUsers.DropDownWidth = 200;
			this.cboUsers.FormattingEnabled = true;
			this.cboUsers.Name = "cboUsers";
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.GroupNO, this.Column1, this.f_DoorEnabled, this.DoorDelay, this.ControllerNO, this.ControllerSN, this.DoorNO });
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Tag = "";
			componentResourceManager.ApplyResources(this.GroupNO, "GroupNO");
			this.GroupNO.Name = "GroupNO";
			this.GroupNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorEnabled, "f_DoorEnabled");
			this.f_DoorEnabled.Name = "f_DoorEnabled";
			this.f_DoorEnabled.ReadOnly = true;
			this.f_DoorEnabled.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_DoorEnabled.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.DoorDelay, "DoorDelay");
			this.DoorDelay.Name = "DoorDelay";
			this.DoorDelay.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ControllerNO, "ControllerNO");
			this.ControllerNO.Name = "ControllerNO";
			this.ControllerNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ControllerSN, "ControllerSN");
			this.ControllerSN.Name = "ControllerSN";
			this.ControllerSN.ReadOnly = true;
			componentResourceManager.ApplyResources(this.DoorNO, "DoorNO");
			this.DoorNO.Name = "DoorNO";
			this.DoorNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvControllers, "dgvControllers");
			this.dgvControllers.AllowUserToAddRows = false;
			this.dgvControllers.AllowUserToDeleteRows = false;
			this.dgvControllers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvControllers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvControllers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_ControllerID, this.f_ControllerNO, this.f_ControllerSN, this.f_Enabled, this.f_IP, this.f_PORT, this.f_ZoneName, this.f_Note, this.f_DoorNames });
			this.dgvControllers.EnableHeadersVisualStyles = false;
			this.dgvControllers.Name = "dgvControllers";
			this.dgvControllers.ReadOnly = true;
			this.dgvControllers.RowHeadersBorderStyle = global::System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(146, 150, 177);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Pixel, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvControllers.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvControllers.RowTemplate.Height = 23;
			this.dgvControllers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvControllers.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.dgvControllers_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.f_ControllerID, "f_ControllerID");
			this.f_ControllerID.Name = "f_ControllerID";
			this.f_ControllerID.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerNO.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_ControllerNO, "f_ControllerNO");
			this.f_ControllerNO.Name = "f_ControllerNO";
			this.f_ControllerNO.ReadOnly = true;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_ControllerSN, "f_ControllerSN");
			this.f_ControllerSN.Name = "f_ControllerSN";
			this.f_ControllerSN.ReadOnly = true;
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.Black;
			dataGridViewCellStyle5.NullValue = false;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			this.f_Enabled.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.f_Enabled, "f_Enabled");
			this.f_Enabled.Name = "f_Enabled";
			this.f_Enabled.ReadOnly = true;
			this.f_Enabled.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Enabled.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
			componentResourceManager.ApplyResources(this.f_Note, "f_Note");
			this.f_Note.Name = "f_Note";
			this.f_Note.ReadOnly = true;
			this.f_DoorNames.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_DoorNames, "f_DoorNames");
			this.f_DoorNames.Name = "f_DoorNames";
			this.f_DoorNames.ReadOnly = true;
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.BackColor = global::System.Drawing.Color.Transparent;
			this.toolStrip1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.toolStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnSearchController, this.btnAdd, this.btnEdit, this.btnDelete, this.btnPrint, this.btnExportToExcel, this.btnFind, this.cboZone });
			this.toolStrip1.Name = "toolStrip1";
			componentResourceManager.ApplyResources(this.btnSearchController, "btnSearchController");
			this.btnSearchController.ForeColor = global::System.Drawing.Color.White;
			this.btnSearchController.Image = global::WG3000_COMM.Properties.Resources.pTools_SearchNet;
			this.btnSearchController.Name = "btnSearchController";
			this.btnSearchController.Click += new global::System.EventHandler(this.btnSearchController_Click);
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
			componentResourceManager.ApplyResources(this.cboZone, "cboZone");
			this.cboZone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZone.Name = "cboZone";
			this.cboZone.DropDown += new global::System.EventHandler(this.cboZone_DropDown);
			this.cboZone.SelectedIndexChanged += new global::System.EventHandler(this.cboZone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.cboUsers);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.dgvControllers);
			base.Controls.Add(this.toolStrip1);
			base.Name = "frmControllers";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmControllers_FormClosing);
			base.Load += new global::System.EventHandler(this.frmControllers_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmControllers_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvControllers).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000A6B RID: 2667
		private global::System.Data.DataTable dtController;

		// Token: 0x04000A6C RID: 2668
		private global::System.Data.DataView dv;

		// Token: 0x04000A6D RID: 2669
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000A6E RID: 2670
		private global::System.Windows.Forms.ToolStripMenuItem batchUpdateSelectToolStripMenuItem;

		// Token: 0x04000A6F RID: 2671
		private global::System.Windows.Forms.ToolStripButton btnAdd;

		// Token: 0x04000A70 RID: 2672
		private global::System.Windows.Forms.ToolStripButton btnDelete;

		// Token: 0x04000A71 RID: 2673
		private global::System.Windows.Forms.ToolStripButton btnEdit;

		// Token: 0x04000A72 RID: 2674
		private global::System.Windows.Forms.ToolStripButton btnExportToExcel;

		// Token: 0x04000A73 RID: 2675
		private global::System.Windows.Forms.ToolStripButton btnFind;

		// Token: 0x04000A74 RID: 2676
		private global::System.Windows.Forms.ToolStripButton btnPrint;

		// Token: 0x04000A75 RID: 2677
		private global::System.Windows.Forms.ToolStripButton btnSearchController;

		// Token: 0x04000A76 RID: 2678
		private global::System.Windows.Forms.ComboBox cboUsers;

		// Token: 0x04000A77 RID: 2679
		private global::System.Windows.Forms.ToolStripComboBox cboZone;

		// Token: 0x04000A78 RID: 2680
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04000A79 RID: 2681
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000A7A RID: 2682
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ControllerNO;

		// Token: 0x04000A7B RID: 2683
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ControllerSN;

		// Token: 0x04000A7C RID: 2684
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000A7D RID: 2685
		private global::System.Windows.Forms.DataGridView dgvControllers;

		// Token: 0x04000A7E RID: 2686
		private global::System.Windows.Forms.DataGridViewTextBoxColumn DoorDelay;

		// Token: 0x04000A7F RID: 2687
		private global::System.Windows.Forms.DataGridViewTextBoxColumn DoorNO;

		// Token: 0x04000A80 RID: 2688
		private global::System.Windows.Forms.ToolStripMenuItem doorsExportToExcelToolStripMenuItem;

		// Token: 0x04000A81 RID: 2689
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerID;

		// Token: 0x04000A82 RID: 2690
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerNO;

		// Token: 0x04000A83 RID: 2691
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;

		// Token: 0x04000A84 RID: 2692
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_DoorEnabled;

		// Token: 0x04000A85 RID: 2693
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorNames;

		// Token: 0x04000A86 RID: 2694
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Enabled;

		// Token: 0x04000A87 RID: 2695
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_IP;

		// Token: 0x04000A88 RID: 2696
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x04000A89 RID: 2697
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

		// Token: 0x04000A8A RID: 2698
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneName;

		// Token: 0x04000A8B RID: 2699
		private global::System.Windows.Forms.DataGridViewTextBoxColumn GroupNO;

		// Token: 0x04000A8C RID: 2700
		private global::System.Windows.Forms.ToolStrip toolStrip1;
	}
}
