namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x0200024A RID: 586
	public partial class dfrmFloors : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x0016400C File Offset: 0x0016300C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0016402C File Offset: 0x0016302C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Elevator.dfrmFloors));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnRemoteControl = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnRemoteControlNC = new global::System.Windows.Forms.ToolStripMenuItem();
			this.controllerTaskListToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.addTaskListToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.queryTaskListToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.btnChange = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.dgvFloorList = new global::System.Windows.Forms.DataGridView();
			this.f_floorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_floorFullName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_floorNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_ZoneID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_floorName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cboFloorNO = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cboElevator = new global::System.Windows.Forms.ComboBox();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvFloorList).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.btnRemoteControl, this.btnRemoteControlNC, this.controllerTaskListToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Opening += new global::System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			componentResourceManager.ApplyResources(this.btnRemoteControl, "btnRemoteControl");
			this.btnRemoteControl.Name = "btnRemoteControl";
			this.btnRemoteControl.Click += new global::System.EventHandler(this.btnRemoteControl_Click);
			componentResourceManager.ApplyResources(this.btnRemoteControlNC, "btnRemoteControlNC");
			this.btnRemoteControlNC.Name = "btnRemoteControlNC";
			this.btnRemoteControlNC.Click += new global::System.EventHandler(this.btnRemoteControlNC_Click);
			componentResourceManager.ApplyResources(this.controllerTaskListToolStripMenuItem, "controllerTaskListToolStripMenuItem");
			this.controllerTaskListToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.addTaskListToolStripMenuItem, this.queryTaskListToolStripMenuItem });
			this.controllerTaskListToolStripMenuItem.Name = "controllerTaskListToolStripMenuItem";
			componentResourceManager.ApplyResources(this.addTaskListToolStripMenuItem, "addTaskListToolStripMenuItem");
			this.addTaskListToolStripMenuItem.Name = "addTaskListToolStripMenuItem";
			this.addTaskListToolStripMenuItem.Click += new global::System.EventHandler(this.addTaskListToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.queryTaskListToolStripMenuItem, "queryTaskListToolStripMenuItem");
			this.queryTaskListToolStripMenuItem.Name = "queryTaskListToolStripMenuItem";
			this.queryTaskListToolStripMenuItem.Click += new global::System.EventHandler(this.queryTaskListToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.btnChange, "btnChange");
			this.btnChange.BackColor = global::System.Drawing.Color.Transparent;
			this.btnChange.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnChange.ForeColor = global::System.Drawing.Color.White;
			this.btnChange.Name = "btnChange";
			this.btnChange.UseVisualStyleBackColor = false;
			this.btnChange.Click += new global::System.EventHandler(this.btnChange_Click);
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.TextChanged += new global::System.EventHandler(this.textBox1_TextChanged);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.dgvFloorList, "dgvFloorList");
			this.dgvFloorList.AllowUserToAddRows = false;
			this.dgvFloorList.AllowUserToDeleteRows = false;
			this.dgvFloorList.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvFloorList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvFloorList.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvFloorList.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_floorID, this.f_floorFullName, this.f_DoorName, this.f_floorNO, this.f_ZoneID, this.f_floorName });
			this.dgvFloorList.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvFloorList.EnableHeadersVisualStyles = false;
			this.dgvFloorList.Name = "dgvFloorList";
			this.dgvFloorList.ReadOnly = true;
			this.dgvFloorList.RowTemplate.Height = 23;
			this.dgvFloorList.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvFloorList.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFloors_KeyDown);
			componentResourceManager.ApplyResources(this.f_floorID, "f_floorID");
			this.f_floorID.Name = "f_floorID";
			this.f_floorID.ReadOnly = true;
			this.f_floorID.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_floorFullName, "f_floorFullName");
			this.f_floorFullName.Name = "f_floorFullName";
			this.f_floorFullName.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_DoorName.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_DoorName, "f_DoorName");
			this.f_DoorName.Name = "f_DoorName";
			this.f_DoorName.ReadOnly = true;
			this.f_DoorName.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_floorNO.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_floorNO, "f_floorNO");
			this.f_floorNO.Name = "f_floorNO";
			this.f_floorNO.ReadOnly = true;
			this.f_floorNO.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			componentResourceManager.ApplyResources(this.f_ZoneID, "f_ZoneID");
			this.f_ZoneID.Name = "f_ZoneID";
			this.f_ZoneID.ReadOnly = true;
			this.f_ZoneID.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_floorName.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.f_floorName, "f_floorName");
			this.f_floorName.Name = "f_floorName";
			this.f_floorName.ReadOnly = true;
			this.f_floorName.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cboFloorNO, "cboFloorNO");
			this.cboFloorNO.DropDownHeight = 300;
			this.cboFloorNO.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFloorNO.FormattingEnabled = true;
			this.cboFloorNO.Name = "cboFloorNO";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.cboElevator, "cboElevator");
			this.cboElevator.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboElevator.FormattingEnabled = true;
			this.cboElevator.Name = "cboElevator";
			this.cboElevator.SelectedIndexChanged += new global::System.EventHandler(this.cboElevator_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnChange);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnDel);
			base.Controls.Add(this.dgvFloorList);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cboFloorNO);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cboElevator);
			base.Name = "dfrmFloors";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmFloors_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerTaskList_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmFloors_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvFloorList).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040021BD RID: 8637
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040021BE RID: 8638
		private global::System.Windows.Forms.ToolStripMenuItem addTaskListToolStripMenuItem;

		// Token: 0x040021BF RID: 8639
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x040021C0 RID: 8640
		private global::System.Windows.Forms.Button btnChange;

		// Token: 0x040021C1 RID: 8641
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x040021C2 RID: 8642
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x040021C3 RID: 8643
		private global::System.Windows.Forms.ToolStripMenuItem btnRemoteControl;

		// Token: 0x040021C4 RID: 8644
		private global::System.Windows.Forms.ToolStripMenuItem btnRemoteControlNC;

		// Token: 0x040021C5 RID: 8645
		private global::System.Windows.Forms.ComboBox cboElevator;

		// Token: 0x040021C6 RID: 8646
		private global::System.Windows.Forms.ComboBox cboFloorNO;

		// Token: 0x040021C7 RID: 8647
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040021C8 RID: 8648
		private global::System.Windows.Forms.ToolStripMenuItem controllerTaskListToolStripMenuItem;

		// Token: 0x040021C9 RID: 8649
		private global::System.Windows.Forms.DataGridView dgvFloorList;

		// Token: 0x040021CA RID: 8650
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorName;

		// Token: 0x040021CB RID: 8651
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_floorFullName;

		// Token: 0x040021CC RID: 8652
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_floorID;

		// Token: 0x040021CD RID: 8653
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_floorName;

		// Token: 0x040021CE RID: 8654
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_floorNO;

		// Token: 0x040021CF RID: 8655
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ZoneID;

		// Token: 0x040021D0 RID: 8656
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040021D1 RID: 8657
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040021D2 RID: 8658
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040021D3 RID: 8659
		private global::System.Windows.Forms.ToolStripMenuItem queryTaskListToolStripMenuItem;

		// Token: 0x040021D4 RID: 8660
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
