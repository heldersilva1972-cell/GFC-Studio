namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030A RID: 778
	public partial class dfrmPatrolTaskDelete : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001791 RID: 6033 RVA: 0x001EA4D4 File Offset: 0x001E94D4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x001EA4F4 File Offset: 0x001E94F4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmPatrolTaskDelete));
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
			this.ProgressBar1 = new global::System.Windows.Forms.ProgressBar();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
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
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.dtpEndDate);
			this.GroupBox1.Controls.Add(this.dtpStartDate);
			this.GroupBox1.Controls.Add(this.cbof_Group);
			this.GroupBox1.Controls.Add(this.lblEndWeekday);
			this.GroupBox1.Controls.Add(this.lblStartWeekday);
			this.GroupBox1.Controls.Add(this.Label6);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.cbof_ConsumerName);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.GroupBox1, componentResourceManager.GetString("GroupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.ProgressBar1, "ProgressBar1");
			this.ProgressBar1.Name = "ProgressBar1";
			this.toolTip1.SetToolTip(this.ProgressBar1, componentResourceManager.GetString("ProgressBar1.ToolTip"));
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.ProgressBar1);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.GroupBox1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmPatrolTaskDelete";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmShiftDelete_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmAutoShift_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAutoShift_KeyDown);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04003084 RID: 12420
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003085 RID: 12421
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04003086 RID: 12422
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003087 RID: 12423
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003088 RID: 12424
		internal global::System.Windows.Forms.ComboBox cbof_ConsumerName;

		// Token: 0x04003089 RID: 12425
		internal global::System.Windows.Forms.ComboBox cbof_Group;

		// Token: 0x0400308A RID: 12426
		internal global::System.Windows.Forms.DateTimePicker dtpEndDate;

		// Token: 0x0400308B RID: 12427
		internal global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x0400308C RID: 12428
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x0400308D RID: 12429
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x0400308E RID: 12430
		internal global::System.Windows.Forms.Label Label4;

		// Token: 0x0400308F RID: 12431
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x04003090 RID: 12432
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x04003091 RID: 12433
		internal global::System.Windows.Forms.Label lblEndWeekday;

		// Token: 0x04003092 RID: 12434
		internal global::System.Windows.Forms.Label lblStartWeekday;

		// Token: 0x04003093 RID: 12435
		internal global::System.Windows.Forms.ProgressBar ProgressBar1;
	}
}
