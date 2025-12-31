namespace WG3000_COMM
{
	// Token: 0x02000331 RID: 817
	public partial class frmTestController : global::System.Windows.Forms.Form
	{
		// Token: 0x060019D7 RID: 6615 RVA: 0x0021E1B0 File Offset: 0x0021D1B0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0021E1D0 File Offset: 0x0021D1D0
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 262);
			base.Name = "frmTestController";
			this.Text = "frmTestController";
			base.Load += new global::System.EventHandler(this.frmTestController_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x040034B8 RID: 13496
		private global::System.ComponentModel.IContainer components;
	}
}
