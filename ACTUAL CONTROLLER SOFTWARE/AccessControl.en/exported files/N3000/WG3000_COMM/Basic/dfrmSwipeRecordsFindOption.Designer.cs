namespace WG3000_COMM.Basic
{
	// Token: 0x0200002D RID: 45
	public partial class dfrmSwipeRecordsFindOption : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0005E48A File Offset: 0x0005D48A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0005E4AC File Offset: 0x0005D4AC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmSwipeRecordsFindOption));
			this.chkRecordTypes = new global::System.Windows.Forms.CheckBox();
			this.chkDoors = new global::System.Windows.Forms.CheckBox();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnQuery = new global::System.Windows.Forms.Button();
			this.grpAddr = new global::System.Windows.Forms.GroupBox();
			this.chkListDoors = new global::System.Windows.Forms.CheckedListBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.cboZone = new global::System.Windows.Forms.ComboBox();
			this.btnSelectNone = new global::System.Windows.Forms.Button();
			this.btnSelectAll = new global::System.Windows.Forms.Button();
			this.grpRecordType = new global::System.Windows.Forms.GroupBox();
			this.chkRecType = new global::System.Windows.Forms.CheckedListBox();
			this.grpAddr.SuspendLayout();
			this.grpRecordType.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.chkRecordTypes, "chkRecordTypes");
			this.chkRecordTypes.ForeColor = global::System.Drawing.Color.White;
			this.chkRecordTypes.Name = "chkRecordTypes";
			this.chkRecordTypes.UseVisualStyleBackColor = true;
			this.chkRecordTypes.CheckedChanged += new global::System.EventHandler(this.chkRecordTypes_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkDoors, "chkDoors");
			this.chkDoors.ForeColor = global::System.Drawing.Color.White;
			this.chkDoors.Name = "chkDoors";
			this.chkDoors.UseVisualStyleBackColor = true;
			this.chkDoors.CheckedChanged += new global::System.EventHandler(this.chkDoors_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.btnQuery, "btnQuery");
			this.btnQuery.BackColor = global::System.Drawing.Color.Transparent;
			this.btnQuery.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnQuery.ForeColor = global::System.Drawing.Color.White;
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.UseVisualStyleBackColor = false;
			this.btnQuery.Click += new global::System.EventHandler(this.btnQuery_Click);
			componentResourceManager.ApplyResources(this.grpAddr, "grpAddr");
			this.grpAddr.Controls.Add(this.chkListDoors);
			this.grpAddr.Controls.Add(this.label25);
			this.grpAddr.Controls.Add(this.cboZone);
			this.grpAddr.Controls.Add(this.btnSelectNone);
			this.grpAddr.Controls.Add(this.btnSelectAll);
			this.grpAddr.Name = "grpAddr";
			this.grpAddr.TabStop = false;
			componentResourceManager.ApplyResources(this.chkListDoors, "chkListDoors");
			this.chkListDoors.CheckOnClick = true;
			this.chkListDoors.FormattingEnabled = true;
			this.chkListDoors.MultiColumn = true;
			this.chkListDoors.Name = "chkListDoors";
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.BackColor = global::System.Drawing.Color.Transparent;
			this.label25.ForeColor = global::System.Drawing.Color.White;
			this.label25.Name = "label25";
			componentResourceManager.ApplyResources(this.cboZone, "cboZone");
			this.cboZone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboZone.FormattingEnabled = true;
			this.cboZone.Name = "cboZone";
			this.cboZone.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.btnSelectNone, "btnSelectNone");
			this.btnSelectNone.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectNone.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectNone.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectNone.Name = "btnSelectNone";
			this.btnSelectNone.UseVisualStyleBackColor = false;
			this.btnSelectNone.Click += new global::System.EventHandler(this.btnSelectNone_Click);
			componentResourceManager.ApplyResources(this.btnSelectAll, "btnSelectAll");
			this.btnSelectAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSelectAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSelectAll.ForeColor = global::System.Drawing.Color.White;
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.UseVisualStyleBackColor = false;
			this.btnSelectAll.Click += new global::System.EventHandler(this.btnSelectAll_Click);
			componentResourceManager.ApplyResources(this.grpRecordType, "grpRecordType");
			this.grpRecordType.Controls.Add(this.chkRecType);
			this.grpRecordType.Name = "grpRecordType";
			this.grpRecordType.TabStop = false;
			componentResourceManager.ApplyResources(this.chkRecType, "chkRecType");
			this.chkRecType.CheckOnClick = true;
			this.chkRecType.FormattingEnabled = true;
			this.chkRecType.MultiColumn = true;
			this.chkRecType.Name = "chkRecType";
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.chkRecordTypes);
			base.Controls.Add(this.chkDoors);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnQuery);
			base.Controls.Add(this.grpAddr);
			base.Controls.Add(this.grpRecordType);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmSwipeRecordsFindOption";
			base.Load += new global::System.EventHandler(this.dfrmSwipeRecordsFindOption_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmSwipeRecordsFindOption_KeyDown);
			this.grpAddr.ResumeLayout(false);
			this.grpAddr.PerformLayout();
			this.grpRecordType.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400061B RID: 1563
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400061C RID: 1564
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x0400061D RID: 1565
		private global::System.Windows.Forms.Button btnQuery;

		// Token: 0x0400061E RID: 1566
		private global::System.Windows.Forms.Button btnSelectAll;

		// Token: 0x0400061F RID: 1567
		private global::System.Windows.Forms.Button btnSelectNone;

		// Token: 0x04000620 RID: 1568
		private global::System.Windows.Forms.ComboBox cboZone;

		// Token: 0x04000621 RID: 1569
		private global::System.Windows.Forms.CheckBox chkDoors;

		// Token: 0x04000622 RID: 1570
		private global::System.Windows.Forms.CheckedListBox chkListDoors;

		// Token: 0x04000623 RID: 1571
		private global::System.Windows.Forms.CheckBox chkRecordTypes;

		// Token: 0x04000624 RID: 1572
		private global::System.Windows.Forms.CheckedListBox chkRecType;

		// Token: 0x04000625 RID: 1573
		private global::System.Windows.Forms.GroupBox grpAddr;

		// Token: 0x04000626 RID: 1574
		private global::System.Windows.Forms.GroupBox grpRecordType;

		// Token: 0x04000627 RID: 1575
		private global::System.Windows.Forms.Label label25;
	}
}
