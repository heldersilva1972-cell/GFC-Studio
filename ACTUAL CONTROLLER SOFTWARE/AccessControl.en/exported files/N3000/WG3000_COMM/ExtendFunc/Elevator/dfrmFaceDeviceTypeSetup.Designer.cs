namespace WG3000_COMM.ExtendFunc.Elevator
{
	// Token: 0x02000249 RID: 585
	public partial class dfrmFaceDeviceTypeSetup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001265 RID: 4709 RVA: 0x00162157 File Offset: 0x00161157
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00162178 File Offset: 0x00161178
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Elevator.dfrmFaceDeviceTypeSetup));
			this.textBox7 = new global::System.Windows.Forms.TextBox();
			this.textBox6 = new global::System.Windows.Forms.TextBox();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			this.chkDeactiveNO = new global::System.Windows.Forms.CheckBox();
			this.numericUpDown1 = new global::System.Windows.Forms.NumericUpDown();
			this.label3 = new global::System.Windows.Forms.Label();
			this.numericUpDown20 = new global::System.Windows.Forms.NumericUpDown();
			this.label141 = new global::System.Windows.Forms.Label();
			this.numericUpDown21 = new global::System.Windows.Forms.NumericUpDown();
			this.label142 = new global::System.Windows.Forms.Label();
			this.textBox5 = new global::System.Windows.Forms.TextBox();
			this.textBox4 = new global::System.Windows.Forms.TextBox();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox0 = new global::System.Windows.Forms.TextBox();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.radioButton0 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown20).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown21).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBox7, "textBox7");
			this.textBox7.Name = "textBox7";
			componentResourceManager.ApplyResources(this.textBox6, "textBox6");
			this.textBox6.Name = "textBox6";
			componentResourceManager.ApplyResources(this.radioButton3, "radioButton3");
			this.radioButton3.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton3.ForeColor = global::System.Drawing.Color.White;
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.UseVisualStyleBackColor = false;
			this.radioButton3.CheckedChanged += new global::System.EventHandler(this.radioButton0_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkDeactiveNO, "chkDeactiveNO");
			this.chkDeactiveNO.ForeColor = global::System.Drawing.Color.White;
			this.chkDeactiveNO.Name = "chkDeactiveNO";
			this.chkDeactiveNO.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.numericUpDown1, "numericUpDown1");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = 9;
			numericUpDown.Maximum = new decimal(array);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			this.label3.Click += new global::System.EventHandler(this.label3_Click);
			componentResourceManager.ApplyResources(this.numericUpDown20, "numericUpDown20");
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.numericUpDown20;
			int[] array2 = new int[4];
			array2[0] = 24;
			numericUpDown2.Maximum = new decimal(array2);
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.numericUpDown20;
			int[] array3 = new int[4];
			array3[0] = 1;
			numericUpDown3.Minimum = new decimal(array3);
			this.numericUpDown20.Name = "numericUpDown20";
			this.numericUpDown20.ReadOnly = true;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.numericUpDown20;
			int[] array4 = new int[4];
			array4[0] = 5;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.label141, "label141");
			this.label141.ForeColor = global::System.Drawing.Color.White;
			this.label141.Name = "label141";
			componentResourceManager.ApplyResources(this.numericUpDown21, "numericUpDown21");
			this.numericUpDown21.DecimalPlaces = 1;
			this.numericUpDown21.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.numericUpDown21;
			int[] array5 = new int[4];
			array5[0] = 25;
			numericUpDown5.Maximum = new decimal(array5);
			this.numericUpDown21.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
			this.numericUpDown21.Name = "numericUpDown21";
			this.numericUpDown21.ReadOnly = true;
			this.numericUpDown21.Value = new decimal(new int[] { 4, 0, 0, 65536 });
			componentResourceManager.ApplyResources(this.label142, "label142");
			this.label142.ForeColor = global::System.Drawing.Color.White;
			this.label142.Name = "label142";
			componentResourceManager.ApplyResources(this.textBox5, "textBox5");
			this.textBox5.Name = "textBox5";
			componentResourceManager.ApplyResources(this.textBox4, "textBox4");
			this.textBox4.Name = "textBox4";
			componentResourceManager.ApplyResources(this.textBox3, "textBox3");
			this.textBox3.Name = "textBox3";
			componentResourceManager.ApplyResources(this.textBox2, "textBox2");
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.textBox0, "textBox0");
			this.textBox0.Name = "textBox0";
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.radioButton0, "radioButton0");
			this.radioButton0.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton0.Checked = true;
			this.radioButton0.ForeColor = global::System.Drawing.Color.White;
			this.radioButton0.Name = "radioButton0";
			this.radioButton0.TabStop = true;
			this.radioButton0.Tag = "";
			this.radioButton0.UseVisualStyleBackColor = false;
			this.radioButton0.CheckedChanged += new global::System.EventHandler(this.radioButton0_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton2.ForeColor = global::System.Drawing.Color.White;
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.UseVisualStyleBackColor = false;
			this.radioButton2.CheckedChanged += new global::System.EventHandler(this.radioButton0_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.radioButton1.ForeColor = global::System.Drawing.Color.White;
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.UseVisualStyleBackColor = false;
			this.radioButton1.CheckedChanged += new global::System.EventHandler(this.radioButton0_CheckedChanged);
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.textBox7);
			base.Controls.Add(this.textBox6);
			base.Controls.Add(this.radioButton3);
			base.Controls.Add(this.chkDeactiveNO);
			base.Controls.Add(this.numericUpDown1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.numericUpDown20);
			base.Controls.Add(this.label141);
			base.Controls.Add(this.numericUpDown21);
			base.Controls.Add(this.label142);
			base.Controls.Add(this.textBox5);
			base.Controls.Add(this.textBox4);
			base.Controls.Add(this.textBox3);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.textBox0);
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.radioButton0);
			base.Controls.Add(this.radioButton2);
			base.Controls.Add(this.radioButton1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmFaceDeviceTypeSetup";
			base.Load += new global::System.EventHandler(this.dfrmOneToMoreSetup_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmOneToMoreSetup_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown20).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown21).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400219D RID: 8605
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400219E RID: 8606
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400219F RID: 8607
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x040021A0 RID: 8608
		private global::System.Windows.Forms.Label label141;

		// Token: 0x040021A1 RID: 8609
		private global::System.Windows.Forms.Label label142;

		// Token: 0x040021A2 RID: 8610
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040021A3 RID: 8611
		public global::System.Windows.Forms.CheckBox chkDeactiveNO;

		// Token: 0x040021A4 RID: 8612
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x040021A5 RID: 8613
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x040021A6 RID: 8614
		public global::System.Windows.Forms.NumericUpDown numericUpDown1;

		// Token: 0x040021A7 RID: 8615
		public global::System.Windows.Forms.NumericUpDown numericUpDown20;

		// Token: 0x040021A8 RID: 8616
		public global::System.Windows.Forms.NumericUpDown numericUpDown21;

		// Token: 0x040021A9 RID: 8617
		public global::System.Windows.Forms.RadioButton radioButton0;

		// Token: 0x040021AA RID: 8618
		public global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x040021AB RID: 8619
		public global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x040021AC RID: 8620
		public global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x040021AD RID: 8621
		internal global::System.Windows.Forms.TextBox textBox0;

		// Token: 0x040021AE RID: 8622
		internal global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040021AF RID: 8623
		internal global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x040021B0 RID: 8624
		internal global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x040021B1 RID: 8625
		internal global::System.Windows.Forms.TextBox textBox4;

		// Token: 0x040021B2 RID: 8626
		internal global::System.Windows.Forms.TextBox textBox5;

		// Token: 0x040021B3 RID: 8627
		internal global::System.Windows.Forms.TextBox textBox6;

		// Token: 0x040021B4 RID: 8628
		internal global::System.Windows.Forms.TextBox textBox7;
	}
}
