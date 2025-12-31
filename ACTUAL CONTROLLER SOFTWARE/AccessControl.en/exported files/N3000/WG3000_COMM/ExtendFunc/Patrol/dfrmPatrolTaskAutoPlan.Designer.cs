namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000309 RID: 777
	public partial class dfrmPatrolTaskAutoPlan : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600177E RID: 6014 RVA: 0x001E8B32 File Offset: 0x001E7B32
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x001E8B54 File Offset: 0x001E7B54
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmPatrolTaskAutoPlan));
			this.cbof_Group = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.Label4 = new global::System.Windows.Forms.Label();
			this.cbof_ConsumerName = new global::System.Windows.Forms.ComboBox();
			this.dtpStartDate = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.dtpEndDate = new global::System.Windows.Forms.DateTimePicker();
			this.lblStartWeekday = new global::System.Windows.Forms.Label();
			this.lblEndWeekday = new global::System.Windows.Forms.Label();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnAddOne = new global::System.Windows.Forms.Button();
			this.btnDeleteOne = new global::System.Windows.Forms.Button();
			this.btnDeleteAll = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.lstOptionalShifts = new global::System.Windows.Forms.ListBox();
			this.lstSelectedShifts = new global::System.Windows.Forms.ListBox();
			this.Label7 = new global::System.Windows.Forms.Label();
			this.lstShiftWeekday = new global::System.Windows.Forms.ListBox();
			this.ProgressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.label8 = new global::System.Windows.Forms.Label();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.GroupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cbof_Group, "cbof_Group");
			this.cbof_Group.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_Group.Name = "cbof_Group";
			this.toolTip1.SetToolTip(this.cbof_Group, componentResourceManager.GetString("cbof_Group.ToolTip"));
			this.cbof_Group.DropDown += new global::System.EventHandler(this.cbof_Group_DropDown);
			this.cbof_Group.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Group_SelectedIndexChanged);
			this.cbof_Group.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.Name = "Label3";
			this.toolTip1.SetToolTip(this.Label3, componentResourceManager.GetString("Label3.ToolTip"));
			componentResourceManager.ApplyResources(this.Label4, "Label4");
			this.Label4.Name = "Label4";
			this.toolTip1.SetToolTip(this.Label4, componentResourceManager.GetString("Label4.ToolTip"));
			componentResourceManager.ApplyResources(this.cbof_ConsumerName, "cbof_ConsumerName");
			this.cbof_ConsumerName.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_ConsumerName.Name = "cbof_ConsumerName";
			this.toolTip1.SetToolTip(this.cbof_ConsumerName, componentResourceManager.GetString("cbof_ConsumerName.ToolTip"));
			this.cbof_ConsumerName.DropDown += new global::System.EventHandler(this.cbof_ConsumerName_DropDown);
			this.cbof_ConsumerName.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
			this.cbof_ConsumerName.Leave += new global::System.EventHandler(this.cbof_ConsumerName_Leave);
			componentResourceManager.ApplyResources(this.dtpStartDate, "dtpStartDate");
			this.dtpStartDate.Name = "dtpStartDate";
			this.toolTip1.SetToolTip(this.dtpStartDate, componentResourceManager.GetString("dtpStartDate.ToolTip"));
			this.dtpStartDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			this.dtpStartDate.ValueChanged += new global::System.EventHandler(this.dtpStartDate_ValueChanged);
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.Name = "Label5";
			this.toolTip1.SetToolTip(this.Label5, componentResourceManager.GetString("Label5.ToolTip"));
			componentResourceManager.ApplyResources(this.Label6, "Label6");
			this.Label6.Name = "Label6";
			this.toolTip1.SetToolTip(this.Label6, componentResourceManager.GetString("Label6.ToolTip"));
			componentResourceManager.ApplyResources(this.dtpEndDate, "dtpEndDate");
			this.dtpEndDate.Name = "dtpEndDate";
			this.toolTip1.SetToolTip(this.dtpEndDate, componentResourceManager.GetString("dtpEndDate.ToolTip"));
			this.dtpEndDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			this.dtpEndDate.ValueChanged += new global::System.EventHandler(this.dtpEndDate_ValueChanged);
			componentResourceManager.ApplyResources(this.lblStartWeekday, "lblStartWeekday");
			this.lblStartWeekday.Name = "lblStartWeekday";
			this.toolTip1.SetToolTip(this.lblStartWeekday, componentResourceManager.GetString("lblStartWeekday.ToolTip"));
			componentResourceManager.ApplyResources(this.lblEndWeekday, "lblEndWeekday");
			this.lblEndWeekday.Name = "lblEndWeekday";
			this.toolTip1.SetToolTip(this.lblEndWeekday, componentResourceManager.GetString("lblEndWeekday.ToolTip"));
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.cbof_Group);
			this.GroupBox1.Controls.Add(this.dtpEndDate);
			this.GroupBox1.Controls.Add(this.Label6);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.dtpStartDate);
			this.GroupBox1.Controls.Add(this.cbof_ConsumerName);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.lblEndWeekday);
			this.GroupBox1.Controls.Add(this.lblStartWeekday);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox1, componentResourceManager.GetString("GroupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			this.toolTip1.SetToolTip(this.Label1, componentResourceManager.GetString("Label1.ToolTip"));
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			this.toolTip1.SetToolTip(this.Label2, componentResourceManager.GetString("Label2.ToolTip"));
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnAddOne, "btnAddOne");
			this.btnAddOne.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddOne.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOne.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOne.Name = "btnAddOne";
			this.toolTip1.SetToolTip(this.btnAddOne, componentResourceManager.GetString("btnAddOne.ToolTip"));
			this.btnAddOne.UseVisualStyleBackColor = false;
			this.btnAddOne.Click += new global::System.EventHandler(this.btnAddOne_Click);
			componentResourceManager.ApplyResources(this.btnDeleteOne, "btnDeleteOne");
			this.btnDeleteOne.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteOne.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOne.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOne.Name = "btnDeleteOne";
			this.toolTip1.SetToolTip(this.btnDeleteOne, componentResourceManager.GetString("btnDeleteOne.ToolTip"));
			this.btnDeleteOne.UseVisualStyleBackColor = false;
			this.btnDeleteOne.Click += new global::System.EventHandler(this.btnDeleteOne_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
			this.btnDeleteAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAll.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAll.Name = "btnDeleteAll";
			this.toolTip1.SetToolTip(this.btnDeleteAll, componentResourceManager.GetString("btnDeleteAll.ToolTip"));
			this.btnDeleteAll.UseVisualStyleBackColor = false;
			this.btnDeleteAll.Click += new global::System.EventHandler(this.btnDeleteAll_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.lstOptionalShifts, "lstOptionalShifts");
			this.lstOptionalShifts.Name = "lstOptionalShifts";
			this.toolTip1.SetToolTip(this.lstOptionalShifts, componentResourceManager.GetString("lstOptionalShifts.ToolTip"));
			this.lstOptionalShifts.DoubleClick += new global::System.EventHandler(this.lstOptionalShifts_DoubleClick);
			componentResourceManager.ApplyResources(this.lstSelectedShifts, "lstSelectedShifts");
			this.lstSelectedShifts.Name = "lstSelectedShifts";
			this.toolTip1.SetToolTip(this.lstSelectedShifts, componentResourceManager.GetString("lstSelectedShifts.ToolTip"));
			this.lstSelectedShifts.DoubleClick += new global::System.EventHandler(this.lstSelectedShifts_DoubleClick);
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.BackColor = global::System.Drawing.SystemColors.ControlLight;
			this.Label7.Name = "Label7";
			this.toolTip1.SetToolTip(this.Label7, componentResourceManager.GetString("Label7.ToolTip"));
			componentResourceManager.ApplyResources(this.lstShiftWeekday, "lstShiftWeekday");
			this.lstShiftWeekday.BackColor = global::System.Drawing.SystemColors.ControlLight;
			this.lstShiftWeekday.Name = "lstShiftWeekday";
			this.lstShiftWeekday.TabStop = false;
			this.toolTip1.SetToolTip(this.lstShiftWeekday, componentResourceManager.GetString("lstShiftWeekday.ToolTip"));
			componentResourceManager.ApplyResources(this.ProgressBar1, "ProgressBar1");
			this.ProgressBar1.Name = "ProgressBar1";
			this.toolTip1.SetToolTip(this.ProgressBar1, componentResourceManager.GetString("ProgressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.BackColor = global::System.Drawing.SystemColors.ControlLight;
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, componentResourceManager.GetString("label8.ToolTip"));
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lstSelectedShifts);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.ProgressBar1);
			base.Controls.Add(this.Label7);
			base.Controls.Add(this.lstOptionalShifts);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnAddOne);
			base.Controls.Add(this.btnDeleteOne);
			base.Controls.Add(this.btnDeleteAll);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.lstShiftWeekday);
			base.Name = "dfrmPatrolTaskAutoPlan";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmAutoShift_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmAutoShift_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
			this.GroupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04003061 RID: 12385
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003062 RID: 12386
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04003063 RID: 12387
		internal global::System.Windows.Forms.Button btnAddOne;

		// Token: 0x04003064 RID: 12388
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003065 RID: 12389
		internal global::System.Windows.Forms.Button btnDeleteAll;

		// Token: 0x04003066 RID: 12390
		internal global::System.Windows.Forms.Button btnDeleteOne;

		// Token: 0x04003067 RID: 12391
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003068 RID: 12392
		internal global::System.Windows.Forms.ComboBox cbof_ConsumerName;

		// Token: 0x04003069 RID: 12393
		internal global::System.Windows.Forms.ComboBox cbof_Group;

		// Token: 0x0400306A RID: 12394
		internal global::System.Windows.Forms.DateTimePicker dtpEndDate;

		// Token: 0x0400306B RID: 12395
		internal global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x0400306C RID: 12396
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x0400306D RID: 12397
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x0400306E RID: 12398
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x0400306F RID: 12399
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x04003070 RID: 12400
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x04003071 RID: 12401
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x04003072 RID: 12402
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x04003073 RID: 12403
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x04003074 RID: 12404
		internal global::System.Windows.Forms.Label label8;

		// Token: 0x04003075 RID: 12405
		internal global::System.Windows.Forms.Label lblEndWeekday;

		// Token: 0x04003076 RID: 12406
		internal global::System.Windows.Forms.Label lblStartWeekday;

		// Token: 0x04003077 RID: 12407
		internal global::System.Windows.Forms.ListBox lstOptionalShifts;

		// Token: 0x04003078 RID: 12408
		internal global::System.Windows.Forms.ListBox lstSelectedShifts;

		// Token: 0x04003079 RID: 12409
		internal global::System.Windows.Forms.ListBox lstShiftWeekday;

		// Token: 0x0400307A RID: 12410
		internal global::System.Windows.Forms.ProgressBar ProgressBar1;
	}
}
