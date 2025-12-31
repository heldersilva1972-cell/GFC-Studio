namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000307 RID: 775
	public partial class dfrmPatrolReportFindOption : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600174C RID: 5964 RVA: 0x001E42CB File Offset: 0x001E32CB
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x001E42EC File Offset: 0x001E32EC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmPatrolReportFindOption));
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnQuery = new global::System.Windows.Forms.Button();
			this.checkedListBox1 = new global::System.Windows.Forms.CheckedListBox();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.checkedListBox1, "checkedListBox1");
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.MultiColumn = true;
			this.checkedListBox1.Name = "checkedListBox1";
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnQuery);
			base.Controls.Add(this.checkedListBox1);
			base.Name = "dfrmPatrolReportFindOption";
			base.Load += new global::System.EventHandler(this.dfrmShiftAttReportFindOption_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x04003007 RID: 12295
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003008 RID: 12296
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04003009 RID: 12297
		private global::System.Windows.Forms.Button btnQuery;

		// Token: 0x0400300A RID: 12298
		private global::System.Windows.Forms.CheckedListBox checkedListBox1;
	}
}
