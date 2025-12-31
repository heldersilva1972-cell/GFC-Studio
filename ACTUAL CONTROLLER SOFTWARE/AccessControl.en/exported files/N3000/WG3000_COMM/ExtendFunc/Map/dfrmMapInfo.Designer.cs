namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F1 RID: 753
	public partial class dfrmMapInfo : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060015D0 RID: 5584 RVA: 0x001B7A88 File Offset: 0x001B6A88
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x001B7AA8 File Offset: 0x001B6AA8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Map.dfrmMapInfo));
			this.lblMapName = new global::System.Windows.Forms.Label();
			this.txtMapName = new global::System.Windows.Forms.TextBox();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.txtMapFileName = new global::System.Windows.Forms.TextBox();
			this.btnBrowse = new global::System.Windows.Forms.Button();
			this.OpenFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lblMapName, "lblMapName");
			this.lblMapName.BackColor = global::System.Drawing.Color.Transparent;
			this.lblMapName.ForeColor = global::System.Drawing.Color.White;
			this.lblMapName.Name = "lblMapName";
			componentResourceManager.ApplyResources(this.txtMapName, "txtMapName");
			this.txtMapName.Name = "txtMapName";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			componentResourceManager.ApplyResources(this.txtMapFileName, "txtMapFileName");
			this.txtMapFileName.Name = "txtMapFileName";
			componentResourceManager.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.BackColor = global::System.Drawing.Color.Transparent;
			this.btnBrowse.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnBrowse.ForeColor = global::System.Drawing.Color.White;
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new global::System.EventHandler(this.btnBrowse_Click);
			componentResourceManager.ApplyResources(this.OpenFileDialog1, "OpenFileDialog1");
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
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnBrowse);
			base.Controls.Add(this.Label2);
			base.Controls.Add(this.txtMapName);
			base.Controls.Add(this.lblMapName);
			base.Controls.Add(this.txtMapFileName);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmMapInfo";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002D43 RID: 11587
		private global::System.ComponentModel.Container components;

		// Token: 0x04002D44 RID: 11588
		internal global::System.Windows.Forms.Button btnBrowse;

		// Token: 0x04002D45 RID: 11589
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002D46 RID: 11590
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002D47 RID: 11591
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04002D48 RID: 11592
		internal global::System.Windows.Forms.Label lblMapName;

		// Token: 0x04002D49 RID: 11593
		internal global::System.Windows.Forms.OpenFileDialog OpenFileDialog1;

		// Token: 0x04002D4A RID: 11594
		internal global::System.Windows.Forms.TextBox txtMapFileName;

		// Token: 0x04002D4B RID: 11595
		internal global::System.Windows.Forms.TextBox txtMapName;
	}
}
