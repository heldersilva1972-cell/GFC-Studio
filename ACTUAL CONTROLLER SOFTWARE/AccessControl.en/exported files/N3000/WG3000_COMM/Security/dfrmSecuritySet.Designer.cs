namespace WG3000_COMM.Security
{
	// Token: 0x02000383 RID: 899
	public partial class dfrmSecuritySet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06002117 RID: 8471 RVA: 0x0027D487 File Offset: 0x0027C487
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0027D4A8 File Offset: 0x0027C4A8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Security.dfrmSecuritySet));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.label10 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.txtf_ControllerSN = new global::System.Windows.Forms.TextBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.btnChangeAndUpdate = new global::System.Windows.Forms.Button();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.btnClearAll = new global::System.Windows.Forms.Button();
			this.btnAddCurrentPCIP = new global::System.Windows.Forms.Button();
			this.btnAddIP = new global::System.Windows.Forms.Button();
			this.txtf_IP = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.lstIP = new global::System.Windows.Forms.ListBox();
			this.txtPasswordPrev = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.txtPasswordNew = new global::System.Windows.Forms.TextBox();
			this.txtPasswordNewConfirm = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripMenuItem1 });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Click += new global::System.EventHandler(this.toolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
			this.txtf_ControllerSN.Name = "txtf_ControllerSN";
			this.txtf_ControllerSN.ReadOnly = true;
			this.txtf_ControllerSN.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Image = global::WG3000_COMM.Properties.Resources.resetjpgNormal;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.btnChangeAndUpdate, "btnChangeAndUpdate");
			this.btnChangeAndUpdate.BackColor = global::System.Drawing.Color.Transparent;
			this.btnChangeAndUpdate.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnChangeAndUpdate.ForeColor = global::System.Drawing.Color.White;
			this.btnChangeAndUpdate.Name = "btnChangeAndUpdate";
			this.btnChangeAndUpdate.UseVisualStyleBackColor = false;
			this.btnChangeAndUpdate.Click += new global::System.EventHandler(this.btnChangeAndUpdate_Click);
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.BackColor = global::System.Drawing.Color.Transparent;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.btnClearAll, "btnClearAll");
			this.btnClearAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClearAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClearAll.ForeColor = global::System.Drawing.Color.White;
			this.btnClearAll.Name = "btnClearAll";
			this.btnClearAll.UseVisualStyleBackColor = false;
			this.btnClearAll.Click += new global::System.EventHandler(this.btnClearAll_Click);
			componentResourceManager.ApplyResources(this.btnAddCurrentPCIP, "btnAddCurrentPCIP");
			this.btnAddCurrentPCIP.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddCurrentPCIP.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddCurrentPCIP.ForeColor = global::System.Drawing.Color.White;
			this.btnAddCurrentPCIP.Name = "btnAddCurrentPCIP";
			this.btnAddCurrentPCIP.UseVisualStyleBackColor = false;
			this.btnAddCurrentPCIP.Click += new global::System.EventHandler(this.btnAddCurrentPCIP_Click);
			componentResourceManager.ApplyResources(this.btnAddIP, "btnAddIP");
			this.btnAddIP.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddIP.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddIP.ForeColor = global::System.Drawing.Color.White;
			this.btnAddIP.Name = "btnAddIP";
			this.btnAddIP.UseVisualStyleBackColor = false;
			this.btnAddIP.Click += new global::System.EventHandler(this.btnAddIP_Click);
			componentResourceManager.ApplyResources(this.txtf_IP, "txtf_IP");
			this.txtf_IP.Name = "txtf_IP";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.lstIP, "lstIP");
			this.lstIP.ContextMenuStrip = this.contextMenuStrip1;
			this.lstIP.FormattingEnabled = true;
			this.lstIP.Name = "lstIP";
			componentResourceManager.ApplyResources(this.txtPasswordPrev, "txtPasswordPrev");
			this.txtPasswordPrev.Name = "txtPasswordPrev";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOk.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOk.ForeColor = global::System.Drawing.Color.White;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.txtPasswordNew, "txtPasswordNew");
			this.txtPasswordNew.Name = "txtPasswordNew";
			componentResourceManager.ApplyResources(this.txtPasswordNewConfirm, "txtPasswordNewConfirm");
			this.txtPasswordNewConfirm.Name = "txtPasswordNewConfirm";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label10);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.txtf_ControllerSN);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.btnChangeAndUpdate);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.btnClearAll);
			base.Controls.Add(this.btnAddCurrentPCIP);
			base.Controls.Add(this.btnAddIP);
			base.Controls.Add(this.txtf_IP);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.lstIP);
			base.Controls.Add(this.txtPasswordPrev);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.txtPasswordNew);
			base.Controls.Add(this.txtPasswordNewConfirm);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.Label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmSecuritySet";
			base.Load += new global::System.EventHandler(this.dfrmCommPSet_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCommPSet_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400393E RID: 14654
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400393F RID: 14655
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04003940 RID: 14656
		private global::System.Windows.Forms.Label label10;

		// Token: 0x04003941 RID: 14657
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04003942 RID: 14658
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04003943 RID: 14659
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04003944 RID: 14660
		private global::System.Windows.Forms.ListBox lstIP;

		// Token: 0x04003945 RID: 14661
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04003946 RID: 14662
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x04003947 RID: 14663
		private global::System.Windows.Forms.TextBox txtf_ControllerSN;

		// Token: 0x04003948 RID: 14664
		internal global::System.Windows.Forms.Button btnAddCurrentPCIP;

		// Token: 0x04003949 RID: 14665
		internal global::System.Windows.Forms.Button btnAddIP;

		// Token: 0x0400394A RID: 14666
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400394B RID: 14667
		internal global::System.Windows.Forms.Button btnChangeAndUpdate;

		// Token: 0x0400394C RID: 14668
		internal global::System.Windows.Forms.Button btnClearAll;

		// Token: 0x0400394D RID: 14669
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x0400394E RID: 14670
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x0400394F RID: 14671
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04003950 RID: 14672
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x04003951 RID: 14673
		internal global::System.Windows.Forms.Label label6;

		// Token: 0x04003952 RID: 14674
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x04003953 RID: 14675
		public global::System.Windows.Forms.TextBox txtf_IP;

		// Token: 0x04003954 RID: 14676
		internal global::System.Windows.Forms.TextBox txtPasswordNew;

		// Token: 0x04003955 RID: 14677
		internal global::System.Windows.Forms.TextBox txtPasswordNewConfirm;

		// Token: 0x04003956 RID: 14678
		internal global::System.Windows.Forms.TextBox txtPasswordPrev;
	}
}
