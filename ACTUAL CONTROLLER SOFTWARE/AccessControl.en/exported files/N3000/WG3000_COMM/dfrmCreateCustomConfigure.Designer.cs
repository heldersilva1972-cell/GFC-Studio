namespace WG3000_COMM
{
	// Token: 0x02000228 RID: 552
	public partial class dfrmCreateCustomConfigure : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x001215E4 File Offset: 0x001205E4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00121604 File Offset: 0x00120604
		private void InitializeComponent()
		{
			new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.dfrmCreateCustomConfigure));
			this.label5 = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label13 = new global::System.Windows.Forms.Label();
			this.txtBackColor15 = new global::System.Windows.Forms.TextBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.txtBackColor14 = new global::System.Windows.Forms.TextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.txtBackColor13 = new global::System.Windows.Forms.TextBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtBackColor5 = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.txtBackColor4 = new global::System.Windows.Forms.TextBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.txtBackColor2 = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.txtBackColor1 = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.txtPasswordNew = new global::System.Windows.Forms.TextBox();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.Label2 = new global::System.Windows.Forms.Label();
			this.txtPasswordNewConfirm = new global::System.Windows.Forms.TextBox();
			this.txtTitle = new global::System.Windows.Forms.TextBox();
			this.txtLogo = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.lblLogo = new global::System.Windows.Forms.Label();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.chkHideLogo = new global::System.Windows.Forms.CheckBox();
			this.txtCustType = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.chkHideMainMenu = new global::System.Windows.Forms.CheckBox();
			this.chkHideStatusBar = new global::System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label5.AutoSize = true;
			this.label5.BackColor = global::System.Drawing.Color.Transparent;
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new global::System.Drawing.Point(125, 212);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(317, 156);
			this.label5.TabIndex = 23;
			this.label5.Text = "根据需要设置定制信息.\r\n\r\n先选择安装要定制的语言版本安装软件. \r\n\r\n(如果是中文 就安装 简体中文版本)\r\n\r\n\r\n\r\n生成的n3k_cust.xmlAA 文件在当前目录下, \r\n\r\n用于替换安装包的 zh\\PHOTO目录下的n3k_cust.xmlAA文件.\r\n\r\n如有疑问 请与技术支持联系...";
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Location = new global::System.Drawing.Point(897, 378);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new global::System.Drawing.Size(117, 100);
			this.groupBox3.TabIndex = 22;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "groupBox3";
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.txtBackColor15);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.txtBackColor14);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.txtBackColor13);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.txtBackColor5);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.txtBackColor4);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtBackColor2);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.txtBackColor1);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Location = new global::System.Drawing.Point(469, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(345, 330);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "背景色部分值设定 (Background Color)";
			this.label13.AutoSize = true;
			this.label13.BackColor = global::System.Drawing.Color.Transparent;
			this.label13.ForeColor = global::System.Drawing.Color.White;
			this.label13.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label13.Location = new global::System.Drawing.Point(26, 20);
			this.label13.Name = "label13";
			this.label13.Size = new global::System.Drawing.Size(287, 36);
			this.label13.TabIndex = 33;
			this.label13.Text = "* 背景色值的输入 (格式为RGB,类似: 255,255,255) \r\n\r\n* 背景图片的修改 请看使用说明文档";
			this.txtBackColor15.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor15.Location = new global::System.Drawing.Point(137, 289);
			this.txtBackColor15.MaxLength = 16;
			this.txtBackColor15.Name = "txtBackColor15";
			this.txtBackColor15.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor15.TabIndex = 32;
			this.label12.BackColor = global::System.Drawing.Color.Transparent;
			this.label12.ForeColor = global::System.Drawing.Color.White;
			this.label12.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label12.Location = new global::System.Drawing.Point(5, 288);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(112, 27);
			this.label12.TabIndex = 31;
			this.label12.Text = "背景色15:\r\nBackcolor 15:";
			this.label12.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor14.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor14.Location = new global::System.Drawing.Point(137, 254);
			this.txtBackColor14.MaxLength = 16;
			this.txtBackColor14.Name = "txtBackColor14";
			this.txtBackColor14.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor14.TabIndex = 30;
			this.label11.BackColor = global::System.Drawing.Color.Transparent;
			this.label11.ForeColor = global::System.Drawing.Color.White;
			this.label11.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label11.Location = new global::System.Drawing.Point(5, 253);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(112, 27);
			this.label11.TabIndex = 29;
			this.label11.Text = "背景色14:\r\nBackcolor 14:";
			this.label11.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor13.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor13.Location = new global::System.Drawing.Point(137, 219);
			this.txtBackColor13.MaxLength = 16;
			this.txtBackColor13.Name = "txtBackColor13";
			this.txtBackColor13.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor13.TabIndex = 28;
			this.label10.BackColor = global::System.Drawing.Color.Transparent;
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label10.Location = new global::System.Drawing.Point(5, 218);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(112, 27);
			this.label10.TabIndex = 27;
			this.label10.Text = "背景色13:\r\nBackcolor 13:";
			this.label10.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor5.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor5.Location = new global::System.Drawing.Point(137, 184);
			this.txtBackColor5.MaxLength = 16;
			this.txtBackColor5.Name = "txtBackColor5";
			this.txtBackColor5.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor5.TabIndex = 26;
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label9.Location = new global::System.Drawing.Point(5, 183);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(112, 27);
			this.label9.TabIndex = 25;
			this.label9.Text = "背景色5:\r\nBackcolor 5:";
			this.label9.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor4.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor4.Location = new global::System.Drawing.Point(137, 149);
			this.txtBackColor4.MaxLength = 16;
			this.txtBackColor4.Name = "txtBackColor4";
			this.txtBackColor4.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor4.TabIndex = 24;
			this.label8.BackColor = global::System.Drawing.Color.Transparent;
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new global::System.Drawing.Point(5, 148);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(112, 27);
			this.label8.TabIndex = 23;
			this.label8.Text = "背景色4:\r\nBackcolor 4:";
			this.label8.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor2.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor2.Location = new global::System.Drawing.Point(137, 114);
			this.txtBackColor2.MaxLength = 16;
			this.txtBackColor2.Name = "txtBackColor2";
			this.txtBackColor2.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor2.TabIndex = 22;
			this.label7.BackColor = global::System.Drawing.Color.Transparent;
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label7.Location = new global::System.Drawing.Point(5, 113);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(112, 27);
			this.label7.TabIndex = 21;
			this.label7.Text = "背景色2:\r\nBackcolor 2:";
			this.label7.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtBackColor1.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtBackColor1.Location = new global::System.Drawing.Point(137, 79);
			this.txtBackColor1.MaxLength = 16;
			this.txtBackColor1.Name = "txtBackColor1";
			this.txtBackColor1.Size = new global::System.Drawing.Size(176, 21);
			this.txtBackColor1.TabIndex = 20;
			this.label6.BackColor = global::System.Drawing.Color.Transparent;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new global::System.Drawing.Point(6, 78);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(112, 27);
			this.label6.TabIndex = 19;
			this.label6.Text = "背景色1:\r\nBackcolor 1:";
			this.label6.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.groupBox1.Controls.Add(this.txtPasswordNew);
			this.groupBox1.Controls.Add(this.Label3);
			this.groupBox1.Controls.Add(this.Label2);
			this.groupBox1.Controls.Add(this.txtPasswordNewConfirm);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Location = new global::System.Drawing.Point(900, 285);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(386, 100);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "通信密码设置 Communication Password";
			this.txtPasswordNew.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtPasswordNew.Location = new global::System.Drawing.Point(196, 30);
			this.txtPasswordNew.MaxLength = 16;
			this.txtPasswordNew.Name = "txtPasswordNew";
			this.txtPasswordNew.Size = new global::System.Drawing.Size(176, 21);
			this.txtPasswordNew.TabIndex = 12;
			this.Label3.BackColor = global::System.Drawing.Color.Transparent;
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.Label3.Location = new global::System.Drawing.Point(8, 62);
			this.Label3.Name = "Label3";
			this.Label3.Size = new global::System.Drawing.Size(180, 27);
			this.Label3.TabIndex = 11;
			this.Label3.Text = "确认通信密码:\r\nConfirm Current Password:";
			this.Label3.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.Label2.Location = new global::System.Drawing.Point(76, 30);
			this.Label2.Name = "Label2";
			this.Label2.Size = new global::System.Drawing.Size(112, 27);
			this.Label2.TabIndex = 10;
			this.Label2.Text = "通信密码:\r\nCurrent Password:\r\n";
			this.Label2.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.txtPasswordNewConfirm.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtPasswordNewConfirm.Location = new global::System.Drawing.Point(196, 62);
			this.txtPasswordNewConfirm.MaxLength = 16;
			this.txtPasswordNewConfirm.Name = "txtPasswordNewConfirm";
			this.txtPasswordNewConfirm.Size = new global::System.Drawing.Size(176, 21);
			this.txtPasswordNewConfirm.TabIndex = 13;
			this.txtTitle.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtTitle.Location = new global::System.Drawing.Point(170, 29);
			this.txtTitle.MaxLength = 16;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new global::System.Drawing.Size(176, 21);
			this.txtTitle.TabIndex = 18;
			this.txtLogo.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtLogo.Location = new global::System.Drawing.Point(170, 68);
			this.txtLogo.MaxLength = 16;
			this.txtLogo.Name = "txtLogo";
			this.txtLogo.Size = new global::System.Drawing.Size(176, 21);
			this.txtLogo.TabIndex = 19;
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new global::System.Drawing.Point(50, 29);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(112, 27);
			this.label1.TabIndex = 16;
			this.label1.Text = "抬头:\r\nTitle:";
			this.label1.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.lblLogo.BackColor = global::System.Drawing.Color.Transparent;
			this.lblLogo.ForeColor = global::System.Drawing.Color.White;
			this.lblLogo.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.lblLogo.Location = new global::System.Drawing.Point(-18, 68);
			this.lblLogo.Name = "lblLogo";
			this.lblLogo.Size = new global::System.Drawing.Size(180, 27);
			this.lblLogo.TabIndex = 17;
			this.lblLogo.Text = "登录Logo:\r\nLogo:";
			this.lblLogo.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.btnOk.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOk.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOk.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnOk.ForeColor = global::System.Drawing.Color.White;
			this.btnOk.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnOk.Location = new global::System.Drawing.Point(190, 404);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(205, 68);
			this.btnOk.TabIndex = 14;
			this.btnOk.Text = "生成文件 n3k_cust.xmlAA\r\n\r\nCreate File";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new global::System.Drawing.Point(455, 404);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(205, 68);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "关闭 \r\n\r\nClose";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.chkHideLogo.AutoSize = true;
			this.chkHideLogo.ForeColor = global::System.Drawing.Color.White;
			this.chkHideLogo.Location = new global::System.Drawing.Point(352, 67);
			this.chkHideLogo.Name = "chkHideLogo";
			this.chkHideLogo.Size = new global::System.Drawing.Size(90, 28);
			this.chkHideLogo.TabIndex = 24;
			this.chkHideLogo.Text = "隐藏 (Hide)\r\nLogo";
			this.chkHideLogo.UseVisualStyleBackColor = true;
			this.txtCustType.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.txtCustType.Location = new global::System.Drawing.Point(615, 353);
			this.txtCustType.MaxLength = 8;
			this.txtCustType.Name = "txtCustType";
			this.txtCustType.Size = new global::System.Drawing.Size(183, 21);
			this.txtCustType.TabIndex = 26;
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new global::System.Drawing.Point(452, 353);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(162, 27);
			this.label4.TabIndex = 25;
			this.label4.Text = "设置的产品密码type:\r\ntype:";
			this.label4.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.chkHideMainMenu.AutoSize = true;
			this.chkHideMainMenu.ForeColor = global::System.Drawing.Color.White;
			this.chkHideMainMenu.Location = new global::System.Drawing.Point(127, 126);
			this.chkHideMainMenu.Name = "chkHideMainMenu";
			this.chkHideMainMenu.Size = new global::System.Drawing.Size(138, 16);
			this.chkHideMainMenu.TabIndex = 27;
			this.chkHideMainMenu.Text = "隐藏 (Hide)  菜单栏";
			this.chkHideMainMenu.UseVisualStyleBackColor = true;
			this.chkHideStatusBar.AutoSize = true;
			this.chkHideStatusBar.ForeColor = global::System.Drawing.Color.White;
			this.chkHideStatusBar.Location = new global::System.Drawing.Point(127, 148);
			this.chkHideStatusBar.Name = "chkHideStatusBar";
			this.chkHideStatusBar.Size = new global::System.Drawing.Size(138, 16);
			this.chkHideStatusBar.TabIndex = 28;
			this.chkHideStatusBar.Text = "隐藏 (Hide)  状态栏";
			this.chkHideStatusBar.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.CancelButton = this.btnCancel;
			base.ClientSize = new global::System.Drawing.Size(878, 482);
			base.Controls.Add(this.chkHideStatusBar);
			base.Controls.Add(this.chkHideMainMenu);
			base.Controls.Add(this.txtCustType);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.chkHideLogo);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.txtTitle);
			base.Controls.Add(this.txtLogo);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.lblLogo);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.btnCancel);
			base.Name = "dfrmCreateCustomConfigure";
			this.Text = "定制配置文件(Create Custom Configure)";
			base.Load += new global::System.EventHandler(this.dfrmCreateCustomConfigure_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmCreateCustomConfigure_KeyDown);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001C9C RID: 7324
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001C9D RID: 7325
		private global::System.Windows.Forms.CheckBox chkHideLogo;

		// Token: 0x04001C9E RID: 7326
		private global::System.Windows.Forms.CheckBox chkHideMainMenu;

		// Token: 0x04001C9F RID: 7327
		private global::System.Windows.Forms.CheckBox chkHideStatusBar;

		// Token: 0x04001CA0 RID: 7328
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04001CA1 RID: 7329
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04001CA2 RID: 7330
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04001CA3 RID: 7331
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001CA4 RID: 7332
		internal global::System.Windows.Forms.Button btnOk;

		// Token: 0x04001CA5 RID: 7333
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x04001CA6 RID: 7334
		internal global::System.Windows.Forms.Label label10;

		// Token: 0x04001CA7 RID: 7335
		internal global::System.Windows.Forms.Label label11;

		// Token: 0x04001CA8 RID: 7336
		internal global::System.Windows.Forms.Label label12;

		// Token: 0x04001CA9 RID: 7337
		internal global::System.Windows.Forms.Label label13;

		// Token: 0x04001CAA RID: 7338
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04001CAB RID: 7339
		internal global::System.Windows.Forms.Label Label3;

		// Token: 0x04001CAC RID: 7340
		internal global::System.Windows.Forms.Label label4;

		// Token: 0x04001CAD RID: 7341
		internal global::System.Windows.Forms.Label label5;

		// Token: 0x04001CAE RID: 7342
		internal global::System.Windows.Forms.Label label6;

		// Token: 0x04001CAF RID: 7343
		internal global::System.Windows.Forms.Label label7;

		// Token: 0x04001CB0 RID: 7344
		internal global::System.Windows.Forms.Label label8;

		// Token: 0x04001CB1 RID: 7345
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x04001CB2 RID: 7346
		internal global::System.Windows.Forms.Label lblLogo;

		// Token: 0x04001CB3 RID: 7347
		internal global::System.Windows.Forms.TextBox txtBackColor1;

		// Token: 0x04001CB4 RID: 7348
		internal global::System.Windows.Forms.TextBox txtBackColor13;

		// Token: 0x04001CB5 RID: 7349
		internal global::System.Windows.Forms.TextBox txtBackColor14;

		// Token: 0x04001CB6 RID: 7350
		internal global::System.Windows.Forms.TextBox txtBackColor15;

		// Token: 0x04001CB7 RID: 7351
		internal global::System.Windows.Forms.TextBox txtBackColor2;

		// Token: 0x04001CB8 RID: 7352
		internal global::System.Windows.Forms.TextBox txtBackColor4;

		// Token: 0x04001CB9 RID: 7353
		internal global::System.Windows.Forms.TextBox txtBackColor5;

		// Token: 0x04001CBA RID: 7354
		internal global::System.Windows.Forms.TextBox txtCustType;

		// Token: 0x04001CBB RID: 7355
		internal global::System.Windows.Forms.TextBox txtLogo;

		// Token: 0x04001CBC RID: 7356
		internal global::System.Windows.Forms.TextBox txtPasswordNew;

		// Token: 0x04001CBD RID: 7357
		internal global::System.Windows.Forms.TextBox txtPasswordNewConfirm;

		// Token: 0x04001CBE RID: 7358
		internal global::System.Windows.Forms.TextBox txtTitle;
	}
}
