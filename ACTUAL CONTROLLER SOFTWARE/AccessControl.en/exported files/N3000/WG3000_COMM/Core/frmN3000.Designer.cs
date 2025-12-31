namespace WG3000_COMM.Core
{
	// Token: 0x02000002 RID: 2
	public partial class frmN3000 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002143 File Offset: 0x00001143
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002164 File Offset: 0x00001164
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(18, 91, 168);
			this.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			base.ClientSize = new global::System.Drawing.Size(792, 566);
			this.DoubleBuffered = true;
			base.KeyPreview = true;
			base.Name = "frmN3000";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrm_FormClosing);
			base.Load += new global::System.EventHandler(this.frmN3000_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrm_KeyDown);
			base.ResumeLayout(false);
		}

		// Token: 0x04000003 RID: 3
		private global::System.ComponentModel.IContainer components;
	}
}
