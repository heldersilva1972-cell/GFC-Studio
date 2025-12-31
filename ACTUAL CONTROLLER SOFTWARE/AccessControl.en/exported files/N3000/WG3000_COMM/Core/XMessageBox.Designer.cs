namespace WG3000_COMM.Core
{
	// Token: 0x02000218 RID: 536
	public partial class XMessageBox : global::System.Windows.Forms.Form
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x0010ED1C File Offset: 0x0010DD1C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0010ED3C File Offset: 0x0010DD3C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Core.XMessageBox));
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.leftPanel = new global::System.Windows.Forms.Panel();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.TextBox1 = new global::WG3000_COMM.Core.XTextBox();
			this.leftPanel.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.button2, "button2");
			this.button2.BackColor = global::System.Drawing.Color.Transparent;
			this.button2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button2.ForeColor = global::System.Drawing.Color.White;
			this.button2.Name = "button2";
			this.button2.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.button3, "button3");
			this.button3.BackColor = global::System.Drawing.Color.Transparent;
			this.button3.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button3.ForeColor = global::System.Drawing.Color.White;
			this.button3.Name = "button3";
			this.button3.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.leftPanel, "leftPanel");
			this.leftPanel.Controls.Add(this.pictureBox1);
			this.leftPanel.Name = "leftPanel";
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.TextBox1, "TextBox1");
			this.TextBox1.BackColor = global::System.Drawing.SystemColors.Control;
			this.TextBox1.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.TextBox1.Cursor = global::System.Windows.Forms.Cursors.IBeam;
			this.TextBox1.Name = "TextBox1";
			this.TextBox1.ReadOnly = true;
			this.TextBox1.TabStop = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.SystemColors.Control;
			base.ControlBox = false;
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.TextBox1);
			base.Controls.Add(this.leftPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "XMessageBox";
			base.ShowInTaskbar = false;
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.XMessageBox_Load);
			base.BackColorChanged += new global::System.EventHandler(this.XMessageBox_BackColorChanged);
			base.ForeColorChanged += new global::System.EventHandler(this.XMessageBox_ForeColorChanged);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.XMessageBox_KeyDown);
			base.Resize += new global::System.EventHandler(this.XMessageBox_Resize);
			this.leftPanel.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001C40 RID: 7232
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001C41 RID: 7233
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04001C42 RID: 7234
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04001C43 RID: 7235
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04001C44 RID: 7236
		private global::System.Windows.Forms.Panel leftPanel;

		// Token: 0x04001C45 RID: 7237
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04001C46 RID: 7238
		private global::WG3000_COMM.Core.XTextBox TextBox1;
	}
}
