namespace WG3000_COMM.ExtendFunc.PrivilegeType
{
	// Token: 0x0200031B RID: 795
	public partial class dfrmPrivilegeTypeEdit : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600188F RID: 6287 RVA: 0x002011FF File Offset: 0x002001FF
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00201220 File Offset: 0x00200220
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.PrivilegeType.dfrmPrivilegeTypeEdit));
			this.label26 = new global::System.Windows.Forms.Label();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.txtPrivilegeName = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label26, "label26");
			this.label26.ForeColor = global::System.Drawing.Color.White;
			this.label26.Name = "label26";
			componentResourceManager.ApplyResources(this.txtNote, "txtNote");
			this.txtNote.Name = "txtNote";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.txtPrivilegeName, "txtPrivilegeName");
			this.txtPrivilegeName.Name = "txtPrivilegeName";
			this.txtPrivilegeName.TextChanged += new global::System.EventHandler(this.txtPrivilegeName_TextChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.txtNote);
			base.Controls.Add(this.txtPrivilegeName);
			base.Controls.Add(this.label26);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.btnOK);
			this.ForeColor = global::System.Drawing.Color.White;
			base.Name = "dfrmPrivilegeTypeEdit";
			base.Load += new global::System.EventHandler(this.dfrmPrivilegeSingle_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003255 RID: 12885
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003256 RID: 12886
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04003257 RID: 12887
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003258 RID: 12888
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04003259 RID: 12889
		private global::System.Windows.Forms.Label label26;

		// Token: 0x0400325A RID: 12890
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x0400325B RID: 12891
		internal global::System.Windows.Forms.TextBox txtPrivilegeName;
	}
}
