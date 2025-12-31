namespace WG3000_COMM.Basic
{
	// Token: 0x0200000E RID: 14
	public partial class dfrmDateTimeSelect : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x0001CC94 File Offset: 0x0001BC94
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0001CCB4 File Offset: 0x0001BCB4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmDateTimeSelect));
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dtpBegin = new global::System.Windows.Forms.DateTimePicker();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.cmdOK = new global::System.Windows.Forms.Button();
			this.txtMoreTime = new global::System.Windows.Forms.TextBox();
			this.lblMoreTime = new global::System.Windows.Forms.Label();
			this.lblBest = new global::System.Windows.Forms.Label();
			this.btnOption = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2010, 1, 1, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dtpBegin, "dtpBegin");
			this.dtpBegin.Name = "dtpBegin";
			this.dtpBegin.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.UseVisualStyleBackColor = false;
			this.cmdCancel.Click += new global::System.EventHandler(this.cmdCancel_Click);
			componentResourceManager.ApplyResources(this.cmdOK, "cmdOK");
			this.cmdOK.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdOK.ForeColor = global::System.Drawing.Color.White;
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.UseVisualStyleBackColor = false;
			this.cmdOK.Click += new global::System.EventHandler(this.cmdOK_Click);
			componentResourceManager.ApplyResources(this.txtMoreTime, "txtMoreTime");
			this.txtMoreTime.Name = "txtMoreTime";
			componentResourceManager.ApplyResources(this.lblMoreTime, "lblMoreTime");
			this.lblMoreTime.ForeColor = global::System.Drawing.Color.White;
			this.lblMoreTime.Name = "lblMoreTime";
			componentResourceManager.ApplyResources(this.lblBest, "lblBest");
			this.lblBest.ForeColor = global::System.Drawing.Color.White;
			this.lblBest.Name = "lblBest";
			componentResourceManager.ApplyResources(this.btnOption, "btnOption");
			this.btnOption.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOption.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOption.ForeColor = global::System.Drawing.Color.White;
			this.btnOption.Name = "btnOption";
			this.btnOption.UseVisualStyleBackColor = false;
			this.btnOption.Click += new global::System.EventHandler(this.btnOption_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.btnOption);
			base.Controls.Add(this.lblBest);
			base.Controls.Add(this.lblMoreTime);
			base.Controls.Add(this.txtMoreTime);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.cmdOK);
			base.Controls.Add(this.dateBeginHMS1);
			base.Controls.Add(this.dtpBegin);
			base.Name = "dfrmDateTimeSelect";
			base.Load += new global::System.EventHandler(this.dfrmDateTimeSelect_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmDateTimeSelect_KeyDown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001CA RID: 458
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040001CB RID: 459
		private global::System.Windows.Forms.Label lblBest;

		// Token: 0x040001CC RID: 460
		private global::System.Windows.Forms.Label lblMoreTime;

		// Token: 0x040001CD RID: 461
		private global::System.Windows.Forms.TextBox txtMoreTime;

		// Token: 0x040001CE RID: 462
		internal global::System.Windows.Forms.Button btnOption;

		// Token: 0x040001CF RID: 463
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x040001D0 RID: 464
		internal global::System.Windows.Forms.Button cmdOK;

		// Token: 0x040001D1 RID: 465
		public global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040001D2 RID: 466
		public global::System.Windows.Forms.DateTimePicker dtpBegin;
	}
}
