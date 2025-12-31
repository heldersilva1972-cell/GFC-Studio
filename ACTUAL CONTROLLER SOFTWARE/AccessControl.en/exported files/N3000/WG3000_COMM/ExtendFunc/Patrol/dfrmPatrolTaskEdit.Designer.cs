namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x0200030B RID: 779
	public partial class dfrmPatrolTaskEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001797 RID: 6039 RVA: 0x001EAF8C File Offset: 0x001E9F8C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x001EAFAC File Offset: 0x001E9FAC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Patrol.dfrmPatrolTaskEdit));
			this.cbof_route = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cbof_route, "cbof_route");
			this.cbof_route.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_route.Name = "cbof_route";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.cbof_route);
			base.Controls.Add(this.Label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmPatrolTaskEdit";
			base.Load += new global::System.EventHandler(this.dfrmShiftEdit_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x0400309B RID: 12443
		private global::System.ComponentModel.Container components;

		// Token: 0x0400309C RID: 12444
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400309D RID: 12445
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x0400309E RID: 12446
		internal global::System.Windows.Forms.ComboBox cbof_route;

		// Token: 0x0400309F RID: 12447
		internal global::System.Windows.Forms.Label Label3;
	}
}
