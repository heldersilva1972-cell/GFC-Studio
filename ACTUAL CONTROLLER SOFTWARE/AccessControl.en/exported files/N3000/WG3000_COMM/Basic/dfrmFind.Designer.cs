namespace WG3000_COMM.Basic
{
	// Token: 0x02000016 RID: 22
	public partial class dfrmFind : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00027D57 File Offset: 0x00026D57
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00027D78 File Offset: 0x00026D78
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmFind));
			this.label1 = new global::System.Windows.Forms.Label();
			this.txtFind = new global::System.Windows.Forms.TextBox();
			this.btnFind = new global::System.Windows.Forms.Button();
			this.btnMarkAll = new global::System.Windows.Forms.Button();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.Found = new global::System.Windows.Forms.Label();
			this.lblCount = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtInfo = new global::System.Windows.Forms.TextBox();
			this.optDept = new global::System.Windows.Forms.RadioButton();
			this.optName = new global::System.Windows.Forms.RadioButton();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.txtFind, "txtFind");
			this.txtFind.Name = "txtFind";
			this.txtFind.TextChanged += new global::System.EventHandler(this.txtFind_TextChanged);
			this.txtFind.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
			componentResourceManager.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFind.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFind.ForeColor = global::System.Drawing.Color.White;
			this.btnFind.Name = "btnFind";
			this.btnFind.UseVisualStyleBackColor = false;
			this.btnFind.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnMarkAll, "btnMarkAll");
			this.btnMarkAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnMarkAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnMarkAll.ForeColor = global::System.Drawing.Color.White;
			this.btnMarkAll.Name = "btnMarkAll";
			this.btnMarkAll.UseVisualStyleBackColor = false;
			this.btnMarkAll.Click += new global::System.EventHandler(this.btnFind_Click);
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.Found, "Found");
			this.Found.BackColor = global::System.Drawing.Color.Transparent;
			this.Found.ForeColor = global::System.Drawing.Color.White;
			this.Found.Name = "Found";
			componentResourceManager.ApplyResources(this.lblCount, "lblCount");
			this.lblCount.BackColor = global::System.Drawing.Color.Transparent;
			this.lblCount.ForeColor = global::System.Drawing.Color.White;
			this.lblCount.Name = "lblCount";
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtInfo, "txtInfo");
			this.txtInfo.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.txtInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtInfo.ForeColor = global::System.Drawing.Color.White;
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.optDept, "optDept");
			this.optDept.ForeColor = global::System.Drawing.Color.White;
			this.optDept.Name = "optDept";
			this.optDept.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optName, "optName");
			this.optName.Checked = true;
			this.optName.ForeColor = global::System.Drawing.Color.White;
			this.optName.Name = "optName";
			this.optName.TabStop = true;
			this.optName.UseVisualStyleBackColor = true;
			base.AcceptButton = this.btnFind;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnClose;
			base.Controls.Add(this.optName);
			base.Controls.Add(this.optDept);
			base.Controls.Add(this.txtInfo);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.lblCount);
			base.Controls.Add(this.Found);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnMarkAll);
			base.Controls.Add(this.btnFind);
			base.Controls.Add(this.txtFind);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "dfrmFind";
			base.TopMost = true;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmFind_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmFind_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002C8 RID: 712
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002C9 RID: 713
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x040002CA RID: 714
		private global::System.Windows.Forms.Button btnFind;

		// Token: 0x040002CB RID: 715
		private global::System.Windows.Forms.Label Found;

		// Token: 0x040002CC RID: 716
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002CD RID: 717
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002CE RID: 718
		private global::System.Windows.Forms.Label lblCount;

		// Token: 0x040002CF RID: 719
		private global::System.Windows.Forms.RadioButton optDept;

		// Token: 0x040002D0 RID: 720
		private global::System.Windows.Forms.RadioButton optName;

		// Token: 0x040002D1 RID: 721
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040002D2 RID: 722
		private global::System.Windows.Forms.TextBox txtFind;

		// Token: 0x040002D3 RID: 723
		private global::System.Windows.Forms.TextBox txtInfo;

		// Token: 0x040002D4 RID: 724
		public global::System.Windows.Forms.Button btnMarkAll;
	}
}
