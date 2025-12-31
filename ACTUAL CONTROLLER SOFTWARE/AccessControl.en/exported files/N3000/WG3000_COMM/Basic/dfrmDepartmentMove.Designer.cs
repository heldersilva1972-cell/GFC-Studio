namespace WG3000_COMM.Basic
{
	// Token: 0x02000010 RID: 16
	public partial class dfrmDepartmentMove : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x0001E442 File Offset: 0x0001D442
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0001E464 File Offset: 0x0001D464
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmDepartmentMove));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.cbof_GroupNew = new global::System.Windows.Forms.ComboBox();
			this.Label5 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.Name = "cbof_GroupID";
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.cbof_GroupNew, "cbof_GroupNew");
			this.cbof_GroupNew.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupNew.Name = "cbof_GroupNew";
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.ForeColor = global::System.Drawing.Color.White;
			this.Label5.Name = "Label5";
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.Label3);
			base.Controls.Add(this.cbof_GroupNew);
			base.Name = "dfrmDepartmentMove";
			base.Load += new global::System.EventHandler(this.dfrmDepartmentMove_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x040001E9 RID: 489
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040001EA RID: 490
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040001EB RID: 491
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040001EC RID: 492
		internal global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x040001ED RID: 493
		internal global::System.Windows.Forms.ComboBox cbof_GroupNew;

		// Token: 0x040001EE RID: 494
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x040001EF RID: 495
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040001F0 RID: 496
		internal global::System.Windows.Forms.Label Label5;
	}
}
