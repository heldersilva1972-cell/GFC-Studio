namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000233 RID: 563
	public partial class dfrmAntiBack : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060010E4 RID: 4324 RVA: 0x001358FE File Offset: 0x001348FE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00135920 File Offset: 0x00134920
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmAntiBack));
			this.nudTotal = new global::System.Windows.Forms.NumericUpDown();
			this.chkActiveAntibackShare = new global::System.Windows.Forms.CheckBox();
			this.radioButton0 = new global::System.Windows.Forms.RadioButton();
			this.radioButton4 = new global::System.Windows.Forms.RadioButton();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.checkBox22 = new global::System.Windows.Forms.CheckBox();
			this.checkBox21 = new global::System.Windows.Forms.CheckBox();
			this.checkBox11 = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.nudTotal, "nudTotal");
			this.nudTotal.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudTotal;
			int[] array = new int[4];
			array[0] = 1000;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudTotal;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudTotal.Name = "nudTotal";
			this.nudTotal.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudTotal;
			int[] array3 = new int[4];
			array3[0] = 2;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkActiveAntibackShare, "chkActiveAntibackShare");
			this.chkActiveAntibackShare.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActiveAntibackShare.ForeColor = global::System.Drawing.Color.White;
			this.chkActiveAntibackShare.Name = "chkActiveAntibackShare";
			this.chkActiveAntibackShare.UseVisualStyleBackColor = false;
			this.chkActiveAntibackShare.CheckedChanged += new global::System.EventHandler(this.chkActiveAntibackShare_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton0, "radioButton0");
			this.radioButton0.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton0.Checked = true;
			this.radioButton0.ForeColor = global::System.Drawing.Color.White;
			this.radioButton0.Name = "radioButton0";
			this.radioButton0.TabStop = true;
			this.radioButton0.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.radioButton4, "radioButton4");
			this.radioButton4.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton4.ForeColor = global::System.Drawing.Color.White;
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.radioButton3, "radioButton3");
			this.radioButton3.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton3.ForeColor = global::System.Drawing.Color.White;
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton2.ForeColor = global::System.Drawing.Color.White;
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton1.ForeColor = global::System.Drawing.Color.White;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.checkBox22, "checkBox22");
			this.checkBox22.BackColor = global::System.Drawing.Color.Transparent;
			this.checkBox22.ForeColor = global::System.Drawing.Color.White;
			this.checkBox22.Name = "checkBox22";
			this.checkBox22.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.checkBox21, "checkBox21");
			this.checkBox21.BackColor = global::System.Drawing.Color.Transparent;
			this.checkBox21.ForeColor = global::System.Drawing.Color.White;
			this.checkBox21.Name = "checkBox21";
			this.checkBox21.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.checkBox11, "checkBox11");
			this.checkBox11.BackColor = global::System.Drawing.Color.Transparent;
			this.checkBox11.ForeColor = global::System.Drawing.Color.White;
			this.checkBox11.Name = "checkBox11";
			this.checkBox11.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.nudTotal);
			base.Controls.Add(this.chkActiveAntibackShare);
			base.Controls.Add(this.radioButton0);
			base.Controls.Add(this.radioButton4);
			base.Controls.Add(this.radioButton3);
			base.Controls.Add(this.radioButton2);
			base.Controls.Add(this.radioButton1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.checkBox22);
			base.Controls.Add(this.checkBox21);
			base.Controls.Add(this.checkBox11);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmAntiBack";
			base.Load += new global::System.EventHandler(this.dfrmAntiBack_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmAntiBack_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001E15 RID: 7701
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001E16 RID: 7702
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001E17 RID: 7703
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001E18 RID: 7704
		private global::System.Windows.Forms.CheckBox checkBox11;

		// Token: 0x04001E19 RID: 7705
		private global::System.Windows.Forms.CheckBox checkBox21;

		// Token: 0x04001E1A RID: 7706
		private global::System.Windows.Forms.CheckBox checkBox22;

		// Token: 0x04001E1B RID: 7707
		private global::System.Windows.Forms.RadioButton radioButton0;

		// Token: 0x04001E1C RID: 7708
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x04001E1D RID: 7709
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x04001E1E RID: 7710
		private global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x04001E1F RID: 7711
		private global::System.Windows.Forms.RadioButton radioButton4;

		// Token: 0x04001E20 RID: 7712
		internal global::System.Windows.Forms.CheckBox chkActiveAntibackShare;

		// Token: 0x04001E21 RID: 7713
		internal global::System.Windows.Forms.NumericUpDown nudTotal;
	}
}
