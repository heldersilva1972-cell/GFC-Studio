namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036A RID: 874
	public partial class dfrmHolidayType : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x00259054 File Offset: 0x00258054
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00259074 File Offset: 0x00258074
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmHolidayType));
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.btnEdit = new global::System.Windows.Forms.Button();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.lstHolidayType = new global::System.Windows.Forms.ListBox();
			this.btnFullAttendance = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.lstHolidayType, "lstHolidayType");
			this.lstHolidayType.FormattingEnabled = true;
			this.lstHolidayType.Name = "lstHolidayType";
			componentResourceManager.ApplyResources(this.btnFullAttendance, "btnFullAttendance");
			this.btnFullAttendance.BackColor = global::System.Drawing.Color.Transparent;
			this.btnFullAttendance.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnFullAttendance.ForeColor = global::System.Drawing.Color.White;
			this.btnFullAttendance.Name = "btnFullAttendance";
			this.btnFullAttendance.UseVisualStyleBackColor = false;
			this.btnFullAttendance.Click += new global::System.EventHandler(this.btnFullAttendance_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnFullAttendance);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnDel);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.lstHolidayType);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmHolidayType";
			base.Load += new global::System.EventHandler(this.dfrmHolidayType_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x040036D5 RID: 14037
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040036D6 RID: 14038
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x040036D7 RID: 14039
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x040036D8 RID: 14040
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x040036D9 RID: 14041
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x040036DA RID: 14042
		private global::System.Windows.Forms.Button btnFullAttendance;

		// Token: 0x040036DB RID: 14043
		private global::System.Windows.Forms.ListBox lstHolidayType;
	}
}
