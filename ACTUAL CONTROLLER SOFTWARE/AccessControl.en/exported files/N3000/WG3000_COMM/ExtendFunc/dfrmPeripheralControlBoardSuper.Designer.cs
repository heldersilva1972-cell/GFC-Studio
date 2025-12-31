namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000246 RID: 582
	public partial class dfrmPeripheralControlBoardSuper : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600121D RID: 4637 RVA: 0x00158DB2 File Offset: 0x00157DB2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00158DD4 File Offset: 0x00157DD4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmPeripheralControlBoardSuper));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.chkForceOutputTimeRemains = new global::System.Windows.Forms.CheckBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.checkBox83 = new global::System.Windows.Forms.CheckBox();
			this.checkBox82 = new global::System.Windows.Forms.CheckBox();
			this.checkBox81 = new global::System.Windows.Forms.CheckBox();
			this.checkBox80 = new global::System.Windows.Forms.CheckBox();
			this.checkBox79 = new global::System.Windows.Forms.CheckBox();
			this.checkBox78 = new global::System.Windows.Forms.CheckBox();
			this.checkBox77 = new global::System.Windows.Forms.CheckBox();
			this.checkBox76 = new global::System.Windows.Forms.CheckBox();
			this.radioButton18 = new global::System.Windows.Forms.RadioButton();
			this.radioButton17 = new global::System.Windows.Forms.RadioButton();
			this.radioButton16 = new global::System.Windows.Forms.RadioButton();
			this.radioButton15 = new global::System.Windows.Forms.RadioButton();
			this.radioButton14 = new global::System.Windows.Forms.RadioButton();
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
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.chkForceOutputTimeRemains, "chkForceOutputTimeRemains");
			this.chkForceOutputTimeRemains.ForeColor = global::System.Drawing.Color.White;
			this.chkForceOutputTimeRemains.Name = "chkForceOutputTimeRemains";
			this.chkForceOutputTimeRemains.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.checkBox83, "checkBox83");
			this.checkBox83.ForeColor = global::System.Drawing.Color.White;
			this.checkBox83.Name = "checkBox83";
			this.checkBox83.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox82, "checkBox82");
			this.checkBox82.ForeColor = global::System.Drawing.Color.White;
			this.checkBox82.Name = "checkBox82";
			this.checkBox82.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox81, "checkBox81");
			this.checkBox81.ForeColor = global::System.Drawing.Color.White;
			this.checkBox81.Name = "checkBox81";
			this.checkBox81.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox80, "checkBox80");
			this.checkBox80.ForeColor = global::System.Drawing.Color.White;
			this.checkBox80.Name = "checkBox80";
			this.checkBox80.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox79, "checkBox79");
			this.checkBox79.ForeColor = global::System.Drawing.Color.White;
			this.checkBox79.Name = "checkBox79";
			this.checkBox79.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox78, "checkBox78");
			this.checkBox78.ForeColor = global::System.Drawing.Color.White;
			this.checkBox78.Name = "checkBox78";
			this.checkBox78.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox77, "checkBox77");
			this.checkBox77.ForeColor = global::System.Drawing.Color.White;
			this.checkBox77.Name = "checkBox77";
			this.checkBox77.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox76, "checkBox76");
			this.checkBox76.ForeColor = global::System.Drawing.Color.White;
			this.checkBox76.Name = "checkBox76";
			this.checkBox76.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton18, "radioButton18");
			this.radioButton18.ForeColor = global::System.Drawing.Color.White;
			this.radioButton18.Name = "radioButton18";
			this.radioButton18.UseVisualStyleBackColor = true;
			this.radioButton18.CheckedChanged += new global::System.EventHandler(this.radioButton14_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton17, "radioButton17");
			this.radioButton17.ForeColor = global::System.Drawing.Color.White;
			this.radioButton17.Name = "radioButton17";
			this.radioButton17.UseVisualStyleBackColor = true;
			this.radioButton17.CheckedChanged += new global::System.EventHandler(this.radioButton14_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton16, "radioButton16");
			this.radioButton16.ForeColor = global::System.Drawing.Color.White;
			this.radioButton16.Name = "radioButton16";
			this.radioButton16.UseVisualStyleBackColor = true;
			this.radioButton16.CheckedChanged += new global::System.EventHandler(this.radioButton14_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton15, "radioButton15");
			this.radioButton15.ForeColor = global::System.Drawing.Color.White;
			this.radioButton15.Name = "radioButton15";
			this.radioButton15.UseVisualStyleBackColor = true;
			this.radioButton15.CheckedChanged += new global::System.EventHandler(this.radioButton14_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton14, "radioButton14");
			this.radioButton14.ForeColor = global::System.Drawing.Color.White;
			this.radioButton14.Name = "radioButton14";
			this.radioButton14.UseVisualStyleBackColor = true;
			this.radioButton14.CheckedChanged += new global::System.EventHandler(this.radioButton14_CheckedChanged);
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.chkForceOutputTimeRemains);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.checkBox83);
			base.Controls.Add(this.checkBox82);
			base.Controls.Add(this.checkBox81);
			base.Controls.Add(this.checkBox80);
			base.Controls.Add(this.checkBox79);
			base.Controls.Add(this.checkBox78);
			base.Controls.Add(this.checkBox77);
			base.Controls.Add(this.checkBox76);
			base.Controls.Add(this.radioButton18);
			base.Controls.Add(this.radioButton17);
			base.Controls.Add(this.radioButton16);
			base.Controls.Add(this.radioButton15);
			base.Controls.Add(this.radioButton14);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "dfrmPeripheralControlBoardSuper";
			base.Load += new global::System.EventHandler(this.dfrmPeripheralControlBoardSuper_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmPeripheralControlBoardSuper_KeyDown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040020D5 RID: 8405
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040020D6 RID: 8406
		private global::System.Windows.Forms.CheckBox checkBox76;

		// Token: 0x040020D7 RID: 8407
		private global::System.Windows.Forms.CheckBox checkBox77;

		// Token: 0x040020D8 RID: 8408
		private global::System.Windows.Forms.CheckBox checkBox78;

		// Token: 0x040020D9 RID: 8409
		private global::System.Windows.Forms.CheckBox checkBox79;

		// Token: 0x040020DA RID: 8410
		private global::System.Windows.Forms.CheckBox checkBox80;

		// Token: 0x040020DB RID: 8411
		private global::System.Windows.Forms.CheckBox checkBox81;

		// Token: 0x040020DC RID: 8412
		private global::System.Windows.Forms.CheckBox checkBox82;

		// Token: 0x040020DD RID: 8413
		private global::System.Windows.Forms.CheckBox checkBox83;

		// Token: 0x040020DE RID: 8414
		private global::System.Windows.Forms.CheckBox chkForceOutputTimeRemains;

		// Token: 0x040020DF RID: 8415
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040020E0 RID: 8416
		private global::System.Windows.Forms.RadioButton radioButton14;

		// Token: 0x040020E1 RID: 8417
		private global::System.Windows.Forms.RadioButton radioButton15;

		// Token: 0x040020E2 RID: 8418
		private global::System.Windows.Forms.RadioButton radioButton16;

		// Token: 0x040020E3 RID: 8419
		private global::System.Windows.Forms.RadioButton radioButton17;

		// Token: 0x040020E4 RID: 8420
		private global::System.Windows.Forms.RadioButton radioButton18;

		// Token: 0x040020E5 RID: 8421
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040020E6 RID: 8422
		internal global::System.Windows.Forms.Button btnOK;
	}
}
