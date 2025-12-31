namespace WG3000_COMM.ExtendFunc.Reader
{
	// Token: 0x02000323 RID: 803
	public partial class dfrmRs232Config : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600191D RID: 6429 RVA: 0x0020DAD4 File Offset: 0x0020CAD4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0020DAF4 File Offset: 0x0020CAF4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Reader.dfrmRs232Config));
			this.optDisableRS232_1 = new global::System.Windows.Forms.RadioButton();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.chkExtern6 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern5 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern4 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern3 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern1 = new global::System.Windows.Forms.CheckBox();
			this.optEnableSFZWithICCard = new global::System.Windows.Forms.RadioButton();
			this.groupBox7 = new global::System.Windows.Forms.GroupBox();
			this.optDoor2Out = new global::System.Windows.Forms.RadioButton();
			this.optDoor2In = new global::System.Windows.Forms.RadioButton();
			this.optDoor1Out = new global::System.Windows.Forms.RadioButton();
			this.optDoor1In = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput_8Byte = new global::System.Windows.Forms.RadioButton();
			this.optEnableSFZDirectOpen = new global::System.Windows.Forms.RadioButton();
			this.label2 = new global::System.Windows.Forms.Label();
			this.optEnabledRXToUDP = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput_32ByteCustomPwd = new global::System.Windows.Forms.RadioButton();
			this.optEnableSFZ = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput = new global::System.Windows.Forms.RadioButton();
			this.txtCustomPwd = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.chkExtern6_2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern5_2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern4_2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern3_2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern2_2 = new global::System.Windows.Forms.CheckBox();
			this.chkExtern1_2 = new global::System.Windows.Forms.CheckBox();
			this.optEnableSFZWithICCard_2 = new global::System.Windows.Forms.RadioButton();
			this.optDisableRS232_2 = new global::System.Windows.Forms.RadioButton();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.optDoor2Out_2 = new global::System.Windows.Forms.RadioButton();
			this.optDoor2In_2 = new global::System.Windows.Forms.RadioButton();
			this.optDoor1Out_2 = new global::System.Windows.Forms.RadioButton();
			this.optDoor1In_2 = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput_8Byte_2 = new global::System.Windows.Forms.RadioButton();
			this.optEnableSFZDirectOpen_2 = new global::System.Windows.Forms.RadioButton();
			this.label1 = new global::System.Windows.Forms.Label();
			this.optEnabledRXToUDP_2 = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput_32ByteCustomPwd_2 = new global::System.Windows.Forms.RadioButton();
			this.optEnableSFZ_2 = new global::System.Windows.Forms.RadioButton();
			this.optEnableCardInput_2 = new global::System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.optDisableRS232_1, "optDisableRS232_1");
			this.optDisableRS232_1.BackColor = global::System.Drawing.Color.Transparent;
			this.optDisableRS232_1.Checked = true;
			this.optDisableRS232_1.ForeColor = global::System.Drawing.Color.White;
			this.optDisableRS232_1.Name = "optDisableRS232_1";
			this.optDisableRS232_1.TabStop = true;
			this.optDisableRS232_1.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.optEnableSFZWithICCard);
			this.groupBox1.Controls.Add(this.optDisableRS232_1);
			this.groupBox1.Controls.Add(this.groupBox7);
			this.groupBox1.Controls.Add(this.optEnableCardInput_8Byte);
			this.groupBox1.Controls.Add(this.optEnableSFZDirectOpen);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.optEnabledRXToUDP);
			this.groupBox1.Controls.Add(this.optEnableCardInput_32ByteCustomPwd);
			this.groupBox1.Controls.Add(this.optEnableSFZ);
			this.groupBox1.Controls.Add(this.optEnableCardInput);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.chkExtern6);
			this.groupBox4.Controls.Add(this.chkExtern5);
			this.groupBox4.Controls.Add(this.chkExtern4);
			this.groupBox4.Controls.Add(this.chkExtern3);
			this.groupBox4.Controls.Add(this.chkExtern2);
			this.groupBox4.Controls.Add(this.chkExtern1);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.chkExtern6, "chkExtern6");
			this.chkExtern6.Name = "chkExtern6";
			this.chkExtern6.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern5, "chkExtern5");
			this.chkExtern5.Name = "chkExtern5";
			this.chkExtern5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern4, "chkExtern4");
			this.chkExtern4.Name = "chkExtern4";
			this.chkExtern4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern3, "chkExtern3");
			this.chkExtern3.Name = "chkExtern3";
			this.chkExtern3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern2, "chkExtern2");
			this.chkExtern2.Name = "chkExtern2";
			this.chkExtern2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern1, "chkExtern1");
			this.chkExtern1.Name = "chkExtern1";
			this.chkExtern1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZWithICCard, "optEnableSFZWithICCard");
			this.optEnableSFZWithICCard.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZWithICCard.Name = "optEnableSFZWithICCard";
			this.optEnableSFZWithICCard.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox7, "groupBox7");
			this.groupBox7.Controls.Add(this.optDoor2Out);
			this.groupBox7.Controls.Add(this.optDoor2In);
			this.groupBox7.Controls.Add(this.optDoor1Out);
			this.groupBox7.Controls.Add(this.optDoor1In);
			this.groupBox7.ForeColor = global::System.Drawing.Color.White;
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.TabStop = false;
			componentResourceManager.ApplyResources(this.optDoor2Out, "optDoor2Out");
			this.optDoor2Out.ForeColor = global::System.Drawing.Color.White;
			this.optDoor2Out.Name = "optDoor2Out";
			this.optDoor2Out.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor2In, "optDoor2In");
			this.optDoor2In.ForeColor = global::System.Drawing.Color.White;
			this.optDoor2In.Name = "optDoor2In";
			this.optDoor2In.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor1Out, "optDoor1Out");
			this.optDoor1Out.ForeColor = global::System.Drawing.Color.White;
			this.optDoor1Out.Name = "optDoor1Out";
			this.optDoor1Out.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor1In, "optDoor1In");
			this.optDoor1In.Checked = true;
			this.optDoor1In.ForeColor = global::System.Drawing.Color.White;
			this.optDoor1In.Name = "optDoor1In";
			this.optDoor1In.TabStop = true;
			this.optDoor1In.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput_8Byte, "optEnableCardInput_8Byte");
			this.optEnableCardInput_8Byte.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput_8Byte.Name = "optEnableCardInput_8Byte";
			this.optEnableCardInput_8Byte.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZDirectOpen, "optEnableSFZDirectOpen");
			this.optEnableSFZDirectOpen.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZDirectOpen.Name = "optEnableSFZDirectOpen";
			this.optEnableSFZDirectOpen.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.optEnabledRXToUDP, "optEnabledRXToUDP");
			this.optEnabledRXToUDP.ForeColor = global::System.Drawing.Color.White;
			this.optEnabledRXToUDP.Name = "optEnabledRXToUDP";
			this.optEnabledRXToUDP.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput_32ByteCustomPwd, "optEnableCardInput_32ByteCustomPwd");
			this.optEnableCardInput_32ByteCustomPwd.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput_32ByteCustomPwd.Name = "optEnableCardInput_32ByteCustomPwd";
			this.optEnableCardInput_32ByteCustomPwd.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZ, "optEnableSFZ");
			this.optEnableSFZ.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZ.Name = "optEnableSFZ";
			this.optEnableSFZ.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput, "optEnableCardInput");
			this.optEnableCardInput.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput.Name = "optEnableCardInput";
			this.optEnableCardInput.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtCustomPwd, "txtCustomPwd");
			this.txtCustomPwd.Name = "txtCustomPwd";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
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
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.groupBox5);
			this.groupBox2.Controls.Add(this.optEnableSFZWithICCard_2);
			this.groupBox2.Controls.Add(this.optDisableRS232_2);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this.optEnableCardInput_8Byte_2);
			this.groupBox2.Controls.Add(this.optEnableSFZDirectOpen_2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.optEnabledRXToUDP_2);
			this.groupBox2.Controls.Add(this.optEnableCardInput_32ByteCustomPwd_2);
			this.groupBox2.Controls.Add(this.optEnableSFZ_2);
			this.groupBox2.Controls.Add(this.optEnableCardInput_2);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.chkExtern6_2);
			this.groupBox5.Controls.Add(this.chkExtern5_2);
			this.groupBox5.Controls.Add(this.chkExtern4_2);
			this.groupBox5.Controls.Add(this.chkExtern3_2);
			this.groupBox5.Controls.Add(this.chkExtern2_2);
			this.groupBox5.Controls.Add(this.chkExtern1_2);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.chkExtern6_2, "chkExtern6_2");
			this.chkExtern6_2.Name = "chkExtern6_2";
			this.chkExtern6_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern5_2, "chkExtern5_2");
			this.chkExtern5_2.Name = "chkExtern5_2";
			this.chkExtern5_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern4_2, "chkExtern4_2");
			this.chkExtern4_2.Name = "chkExtern4_2";
			this.chkExtern4_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern3_2, "chkExtern3_2");
			this.chkExtern3_2.Name = "chkExtern3_2";
			this.chkExtern3_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern2_2, "chkExtern2_2");
			this.chkExtern2_2.Name = "chkExtern2_2";
			this.chkExtern2_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkExtern1_2, "chkExtern1_2");
			this.chkExtern1_2.Name = "chkExtern1_2";
			this.chkExtern1_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZWithICCard_2, "optEnableSFZWithICCard_2");
			this.optEnableSFZWithICCard_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZWithICCard_2.Name = "optEnableSFZWithICCard_2";
			this.optEnableSFZWithICCard_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDisableRS232_2, "optDisableRS232_2");
			this.optDisableRS232_2.BackColor = global::System.Drawing.Color.Transparent;
			this.optDisableRS232_2.Checked = true;
			this.optDisableRS232_2.ForeColor = global::System.Drawing.Color.White;
			this.optDisableRS232_2.Name = "optDisableRS232_2";
			this.optDisableRS232_2.TabStop = true;
			this.optDisableRS232_2.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.optDoor2Out_2);
			this.groupBox3.Controls.Add(this.optDoor2In_2);
			this.groupBox3.Controls.Add(this.optDoor1Out_2);
			this.groupBox3.Controls.Add(this.optDoor1In_2);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.optDoor2Out_2, "optDoor2Out_2");
			this.optDoor2Out_2.ForeColor = global::System.Drawing.Color.White;
			this.optDoor2Out_2.Name = "optDoor2Out_2";
			this.optDoor2Out_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor2In_2, "optDoor2In_2");
			this.optDoor2In_2.ForeColor = global::System.Drawing.Color.White;
			this.optDoor2In_2.Name = "optDoor2In_2";
			this.optDoor2In_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor1Out_2, "optDoor1Out_2");
			this.optDoor1Out_2.ForeColor = global::System.Drawing.Color.White;
			this.optDoor1Out_2.Name = "optDoor1Out_2";
			this.optDoor1Out_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDoor1In_2, "optDoor1In_2");
			this.optDoor1In_2.Checked = true;
			this.optDoor1In_2.ForeColor = global::System.Drawing.Color.White;
			this.optDoor1In_2.Name = "optDoor1In_2";
			this.optDoor1In_2.TabStop = true;
			this.optDoor1In_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput_8Byte_2, "optEnableCardInput_8Byte_2");
			this.optEnableCardInput_8Byte_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput_8Byte_2.Name = "optEnableCardInput_8Byte_2";
			this.optEnableCardInput_8Byte_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZDirectOpen_2, "optEnableSFZDirectOpen_2");
			this.optEnableSFZDirectOpen_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZDirectOpen_2.Name = "optEnableSFZDirectOpen_2";
			this.optEnableSFZDirectOpen_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.optEnabledRXToUDP_2, "optEnabledRXToUDP_2");
			this.optEnabledRXToUDP_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnabledRXToUDP_2.Name = "optEnabledRXToUDP_2";
			this.optEnabledRXToUDP_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput_32ByteCustomPwd_2, "optEnableCardInput_32ByteCustomPwd_2");
			this.optEnableCardInput_32ByteCustomPwd_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput_32ByteCustomPwd_2.Name = "optEnableCardInput_32ByteCustomPwd_2";
			this.optEnableCardInput_32ByteCustomPwd_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableSFZ_2, "optEnableSFZ_2");
			this.optEnableSFZ_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableSFZ_2.Name = "optEnableSFZ_2";
			this.optEnableSFZ_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optEnableCardInput_2, "optEnableCardInput_2");
			this.optEnableCardInput_2.ForeColor = global::System.Drawing.Color.White;
			this.optEnableCardInput_2.Name = "optEnableCardInput_2";
			this.optEnableCardInput_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.txtCustomPwd);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOK);
			base.Name = "dfrmRs232Config";
			base.Load += new global::System.EventHandler(this.dfrmReaderConfig_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmRs232Config_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400335A RID: 13146
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400335B RID: 13147
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400335C RID: 13148
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x0400335D RID: 13149
		private global::System.Windows.Forms.CheckBox chkExtern1;

		// Token: 0x0400335E RID: 13150
		private global::System.Windows.Forms.CheckBox chkExtern1_2;

		// Token: 0x0400335F RID: 13151
		private global::System.Windows.Forms.CheckBox chkExtern2;

		// Token: 0x04003360 RID: 13152
		private global::System.Windows.Forms.CheckBox chkExtern2_2;

		// Token: 0x04003361 RID: 13153
		private global::System.Windows.Forms.CheckBox chkExtern3;

		// Token: 0x04003362 RID: 13154
		private global::System.Windows.Forms.CheckBox chkExtern3_2;

		// Token: 0x04003363 RID: 13155
		private global::System.Windows.Forms.CheckBox chkExtern4;

		// Token: 0x04003364 RID: 13156
		private global::System.Windows.Forms.CheckBox chkExtern4_2;

		// Token: 0x04003365 RID: 13157
		private global::System.Windows.Forms.CheckBox chkExtern5;

		// Token: 0x04003366 RID: 13158
		private global::System.Windows.Forms.CheckBox chkExtern5_2;

		// Token: 0x04003367 RID: 13159
		private global::System.Windows.Forms.CheckBox chkExtern6;

		// Token: 0x04003368 RID: 13160
		private global::System.Windows.Forms.CheckBox chkExtern6_2;

		// Token: 0x04003369 RID: 13161
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400336A RID: 13162
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x0400336B RID: 13163
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400336C RID: 13164
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400336D RID: 13165
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400336E RID: 13166
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400336F RID: 13167
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04003370 RID: 13168
		private global::System.Windows.Forms.RadioButton optDisableRS232_1;

		// Token: 0x04003371 RID: 13169
		private global::System.Windows.Forms.RadioButton optDisableRS232_2;

		// Token: 0x04003372 RID: 13170
		private global::System.Windows.Forms.TextBox txtCustomPwd;

		// Token: 0x04003373 RID: 13171
		public global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04003374 RID: 13172
		public global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04003375 RID: 13173
		public global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04003376 RID: 13174
		public global::System.Windows.Forms.GroupBox groupBox7;

		// Token: 0x04003377 RID: 13175
		public global::System.Windows.Forms.RadioButton optDoor1In;

		// Token: 0x04003378 RID: 13176
		public global::System.Windows.Forms.RadioButton optDoor1In_2;

		// Token: 0x04003379 RID: 13177
		public global::System.Windows.Forms.RadioButton optDoor1Out;

		// Token: 0x0400337A RID: 13178
		public global::System.Windows.Forms.RadioButton optDoor1Out_2;

		// Token: 0x0400337B RID: 13179
		public global::System.Windows.Forms.RadioButton optDoor2In;

		// Token: 0x0400337C RID: 13180
		public global::System.Windows.Forms.RadioButton optDoor2In_2;

		// Token: 0x0400337D RID: 13181
		public global::System.Windows.Forms.RadioButton optDoor2Out;

		// Token: 0x0400337E RID: 13182
		public global::System.Windows.Forms.RadioButton optDoor2Out_2;

		// Token: 0x0400337F RID: 13183
		public global::System.Windows.Forms.RadioButton optEnableCardInput;

		// Token: 0x04003380 RID: 13184
		public global::System.Windows.Forms.RadioButton optEnableCardInput_2;

		// Token: 0x04003381 RID: 13185
		public global::System.Windows.Forms.RadioButton optEnableCardInput_32ByteCustomPwd;

		// Token: 0x04003382 RID: 13186
		public global::System.Windows.Forms.RadioButton optEnableCardInput_32ByteCustomPwd_2;

		// Token: 0x04003383 RID: 13187
		public global::System.Windows.Forms.RadioButton optEnableCardInput_8Byte;

		// Token: 0x04003384 RID: 13188
		public global::System.Windows.Forms.RadioButton optEnableCardInput_8Byte_2;

		// Token: 0x04003385 RID: 13189
		public global::System.Windows.Forms.RadioButton optEnabledRXToUDP;

		// Token: 0x04003386 RID: 13190
		public global::System.Windows.Forms.RadioButton optEnabledRXToUDP_2;

		// Token: 0x04003387 RID: 13191
		public global::System.Windows.Forms.RadioButton optEnableSFZ;

		// Token: 0x04003388 RID: 13192
		public global::System.Windows.Forms.RadioButton optEnableSFZ_2;

		// Token: 0x04003389 RID: 13193
		public global::System.Windows.Forms.RadioButton optEnableSFZDirectOpen;

		// Token: 0x0400338A RID: 13194
		public global::System.Windows.Forms.RadioButton optEnableSFZDirectOpen_2;

		// Token: 0x0400338B RID: 13195
		public global::System.Windows.Forms.RadioButton optEnableSFZWithICCard;

		// Token: 0x0400338C RID: 13196
		public global::System.Windows.Forms.RadioButton optEnableSFZWithICCard_2;
	}
}
