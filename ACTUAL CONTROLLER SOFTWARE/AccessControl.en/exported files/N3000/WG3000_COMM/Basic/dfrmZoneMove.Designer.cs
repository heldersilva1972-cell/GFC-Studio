namespace WG3000_COMM.Basic
{
	// Token: 0x0200003D RID: 61
	public partial class dfrmZoneMove : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x00074E58 File Offset: 0x00073E58
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00074E78 File Offset: 0x00073E78
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmZoneMove));
			this.Label5 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.cbof_GroupNew = new global::System.Windows.Forms.ComboBox();
			this.btnMoveAllToOne = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.Label5, "Label5");
			this.Label5.ForeColor = global::System.Drawing.Color.White;
			this.Label5.Name = "Label5";
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
			componentResourceManager.ApplyResources(this.btnMoveAllToOne, "btnMoveAllToOne");
			this.btnMoveAllToOne.BackColor = global::System.Drawing.Color.Transparent;
			this.btnMoveAllToOne.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnMoveAllToOne.ForeColor = global::System.Drawing.Color.White;
			this.btnMoveAllToOne.Name = "btnMoveAllToOne";
			this.btnMoveAllToOne.UseVisualStyleBackColor = false;
			this.btnMoveAllToOne.Click += new global::System.EventHandler(this.btnMoveAllToOne_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnMoveAllToOne);
			base.Controls.Add(this.Label5);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cbof_GroupID);
			base.Controls.Add(this.Label3);
			base.Controls.Add(this.cbof_GroupNew);
			base.Name = "dfrmZoneMove";
			base.Load += new global::System.EventHandler(this.dfrmDepartmentMove_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x040007EF RID: 2031
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040007F0 RID: 2032
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040007F1 RID: 2033
		internal global::System.Windows.Forms.Button btnMoveAllToOne;

		// Token: 0x040007F2 RID: 2034
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040007F3 RID: 2035
		internal global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x040007F4 RID: 2036
		internal global::System.Windows.Forms.ComboBox cbof_GroupNew;

		// Token: 0x040007F5 RID: 2037
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x040007F6 RID: 2038
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x040007F7 RID: 2039
		internal global::System.Windows.Forms.Label Label5;
	}
}
