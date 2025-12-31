namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000316 RID: 790
	public partial class dfrmPCCheckAccess : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001856 RID: 6230 RVA: 0x001FBA0C File Offset: 0x001FAA0C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.contr4PCCheckAccess != null)
			{
				this.contr4PCCheckAccess.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x001FBA44 File Offset: 0x001FAA44
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PCCheck.dfrmPCCheckAccess));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtConsumers = new global::System.Windows.Forms.RichTextBox();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.txtB0 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.txtC0 = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtA0 = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.btnErrConnect = new global::System.Windows.Forms.Button();
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.GroupBox1.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.txtConsumers);
			this.GroupBox1.Controls.Add(this.textBox3);
			this.GroupBox1.Controls.Add(this.txtB0);
			this.GroupBox1.Controls.Add(this.label1);
			this.GroupBox1.Controls.Add(this.txtC0);
			this.GroupBox1.Controls.Add(this.label3);
			this.GroupBox1.Controls.Add(this.label2);
			this.GroupBox1.Controls.Add(this.txtA0);
			this.GroupBox1.Controls.Add(this.label4);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.txtConsumers, "txtConsumers");
			this.txtConsumers.BackColor = global::System.Drawing.Color.White;
			this.txtConsumers.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtConsumers.ForeColor = global::System.Drawing.Color.Black;
			this.txtConsumers.Name = "txtConsumers";
			this.txtConsumers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBox3, "textBox3");
			this.textBox3.ForeColor = global::System.Drawing.Color.FromArgb(0, 0, 192);
			this.textBox3.Name = "textBox3";
			this.textBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.txtB0, "txtB0");
			this.txtB0.BackColor = global::System.Drawing.Color.White;
			this.txtB0.Name = "txtB0";
			this.txtB0.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.txtC0, "txtC0");
			this.txtC0.BackColor = global::System.Drawing.Color.White;
			this.txtC0.Name = "txtC0";
			this.txtC0.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.txtA0, "txtA0");
			this.txtA0.BackColor = global::System.Drawing.Color.White;
			this.txtA0.Name = "txtA0";
			this.txtA0.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.btnErrConnect, "btnErrConnect");
			this.btnErrConnect.BackgroundImage = global::WG3000_COMM.Properties.Resources.eventlogError;
			this.btnErrConnect.FlatAppearance.BorderSize = 0;
			this.btnErrConnect.Name = "btnErrConnect";
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.btnErrConnect);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.GroupBox1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmPCCheckAccess";
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmPCCheckAccess_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmPCCheckAccess_Load);
			base.VisibleChanged += new global::System.EventHandler(this.dfrmPCCheckAccess_VisibleChanged);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPCCheckAccess_KeyDown);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040031CF RID: 12751
		internal global::System.Windows.Forms.Button btnErrConnect;

		// Token: 0x040031D1 RID: 12753
		private global::WG3000_COMM.DataOper.icController contr4PCCheckAccess = new global::WG3000_COMM.DataOper.icController();

		// Token: 0x040031D6 RID: 12758
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x040031E3 RID: 12771
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040031E4 RID: 12772
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040031E5 RID: 12773
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040031E6 RID: 12774
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040031E7 RID: 12775
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040031E8 RID: 12776
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040031E9 RID: 12777
		private global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x040031EA RID: 12778
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040031EB RID: 12779
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x040031EC RID: 12780
		private global::System.Windows.Forms.TextBox txtA0;

		// Token: 0x040031ED RID: 12781
		private global::System.Windows.Forms.TextBox txtB0;

		// Token: 0x040031EE RID: 12782
		private global::System.Windows.Forms.TextBox txtC0;

		// Token: 0x040031EF RID: 12783
		private global::System.Windows.Forms.RichTextBox txtConsumers;
	}
}
