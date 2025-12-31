namespace WG3000_COMM.ExtendFunc.Reader
{
	// Token: 0x02000321 RID: 801
	public partial class dfrmReaderConfig : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600190E RID: 6414 RVA: 0x0020BDE3 File Offset: 0x0020ADE3
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0020BE04 File Offset: 0x0020AE04
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Reader.dfrmReaderConfig));
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.nudValidLength = new global::System.Windows.Forms.NumericUpDown();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudStart = new global::System.Windows.Forms.NumericUpDown();
			this.label5 = new global::System.Windows.Forms.Label();
			this.nudTotal = new global::System.Windows.Forms.NumericUpDown();
			this.radioButton0 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidLength).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudStart).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).BeginInit();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.nudValidLength);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.nudStart);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.nudTotal);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			componentResourceManager.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.checkBox1.ForeColor = global::System.Drawing.Color.White;
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.nudValidLength, "nudValidLength");
			this.nudValidLength.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudValidLength;
			int[] array = new int[4];
			array[0] = 90;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudValidLength;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudValidLength.Name = "nudValidLength";
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudValidLength;
			int[] array3 = new int[4];
			array3[0] = 32;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.nudStart, "nudStart");
			this.nudStart.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudStart;
			int[] array4 = new int[4];
			array4[0] = 90;
			numericUpDown4.Maximum = new decimal(array4);
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudStart;
			int[] array5 = new int[4];
			array5[0] = 1;
			numericUpDown5.Minimum = new decimal(array5);
			this.nudStart.Name = "nudStart";
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudStart;
			int[] array6 = new int[4];
			array6[0] = 2;
			numericUpDown6.Value = new decimal(array6);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.nudTotal, "nudTotal");
			this.nudTotal.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudTotal;
			int[] array7 = new int[4];
			array7[0] = 98;
			numericUpDown7.Maximum = new decimal(array7);
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.nudTotal;
			int[] array8 = new int[4];
			array8[0] = 26;
			numericUpDown8.Minimum = new decimal(array8);
			this.nudTotal.Name = "nudTotal";
			global::System.Windows.Forms.NumericUpDown numericUpDown9 = this.nudTotal;
			int[] array9 = new int[4];
			array9[0] = 34;
			numericUpDown9.Value = new decimal(array9);
			componentResourceManager.ApplyResources(this.radioButton0, "radioButton0");
			this.radioButton0.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton0.Checked = true;
			this.radioButton0.ForeColor = global::System.Drawing.Color.White;
			this.radioButton0.Name = "radioButton0";
			this.radioButton0.TabStop = true;
			this.radioButton0.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton1.ForeColor = global::System.Drawing.Color.White;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.UseVisualStyleBackColor = false;
			this.radioButton1.CheckedChanged += new global::System.EventHandler(this.radioButton1_CheckedChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.radioButton0);
			base.Controls.Add(this.radioButton1);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Name = "dfrmReaderConfig";
			base.Load += new global::System.EventHandler(this.dfrmReaderConfig_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudValidLength).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudStart).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400330E RID: 13070
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400330F RID: 13071
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04003310 RID: 13072
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04003311 RID: 13073
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04003312 RID: 13074
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04003313 RID: 13075
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003314 RID: 13076
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04003315 RID: 13077
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04003316 RID: 13078
		private global::System.Windows.Forms.NumericUpDown nudStart;

		// Token: 0x04003317 RID: 13079
		private global::System.Windows.Forms.NumericUpDown nudTotal;

		// Token: 0x04003318 RID: 13080
		private global::System.Windows.Forms.NumericUpDown nudValidLength;

		// Token: 0x04003319 RID: 13081
		private global::System.Windows.Forms.RadioButton radioButton0;

		// Token: 0x0400331A RID: 13082
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x0400331B RID: 13083
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
