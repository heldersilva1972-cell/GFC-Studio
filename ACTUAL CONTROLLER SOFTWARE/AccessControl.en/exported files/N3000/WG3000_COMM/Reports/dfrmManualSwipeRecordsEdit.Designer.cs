namespace WG3000_COMM.Reports
{
	// Token: 0x0200035C RID: 860
	public partial class dfrmManualSwipeRecordsEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001B9B RID: 7067 RVA: 0x00225205 File Offset: 0x00224205
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00225224 File Offset: 0x00224224
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.dfrmManualSwipeRecordsEdit));
			this.txtf_ConsumerName = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.dateEndHMS2 = new global::System.Windows.Forms.DateTimePicker();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.txtf_Notes = new global::System.Windows.Forms.TextBox();
			this.dtpStartDate = new global::System.Windows.Forms.DateTimePicker();
			this.Label5 = new global::System.Windows.Forms.Label();
			this.Label7 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtf_ConsumerName, "txtf_ConsumerName");
			this.txtf_ConsumerName.Name = "txtf_ConsumerName";
			this.txtf_ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.dateEndHMS2, "dateEndHMS2");
			this.dateEndHMS2.Name = "dateEndHMS2";
			this.dateEndHMS2.ShowUpDown = true;
			this.dateEndHMS2.Value = new global::System.DateTime(2010, 1, 1, 8, 0, 0, 0);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.txtf_Notes, "txtf_Notes");
			this.txtf_Notes.Name = "txtf_Notes";
			componentResourceManager.ApplyResources(this.dtpStartDate, "dtpStartDate");
			this.dtpStartDate.Name = "dtpStartDate";
			this.dtpStartDate.Value = new global::System.DateTime(2004, 7, 19, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.BackColor = global::System.Drawing.Color.Transparent;
			this.Label5.ForeColor = global::System.Drawing.Color.White;
			this.Label5.Name = "Label5";
			componentResourceManager.ApplyResources(this.Label7, "Label7");
			this.Label7.BackColor = global::System.Drawing.Color.Transparent;
			this.Label7.ForeColor = global::System.Drawing.Color.White;
			this.Label7.Name = "Label7";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtf_ConsumerName);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.dateEndHMS2);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.txtf_Notes);
			base.Controls.Add(this.dtpStartDate);
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.Label7);
			base.Name = "dfrmManualSwipeRecordsEdit";
			base.Load += new global::System.EventHandler(this.dfrmManualSwipeRecordsEdit_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003561 RID: 13665
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003562 RID: 13666
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04003563 RID: 13667
		internal global::System.Windows.Forms.Button btnClose;

		// Token: 0x04003564 RID: 13668
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003565 RID: 13669
		public global::System.Windows.Forms.DateTimePicker dateEndHMS2;

		// Token: 0x04003566 RID: 13670
		public global::System.Windows.Forms.DateTimePicker dtpStartDate;

		// Token: 0x04003567 RID: 13671
		internal global::System.Windows.Forms.Label Label5;

		// Token: 0x04003568 RID: 13672
		internal global::System.Windows.Forms.Label Label7;

		// Token: 0x04003569 RID: 13673
		public global::System.Windows.Forms.TextBox txtf_ConsumerName;

		// Token: 0x0400356A RID: 13674
		public global::System.Windows.Forms.TextBox txtf_Notes;
	}
}
