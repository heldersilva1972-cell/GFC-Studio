namespace WG3000_COMM.ExtendFunc._2019SwipeSendToCenter
{
	// Token: 0x0200032B RID: 811
	public partial class dfrmSendSwipeToCenterA : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060019A5 RID: 6565 RVA: 0x0021816A File Offset: 0x0021716A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0021818C File Offset: 0x0021718C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc._2019SwipeSendToCenter.dfrmSendSwipeToCenterA));
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.btnErrConnect = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudMaxLen = new global::System.Windows.Forms.NumericUpDown();
			this.txtServerURL = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.tabPage6 = new global::System.Windows.Forms.TabPage();
			this.btnSetAllrecordSent = new global::System.Windows.Forms.Button();
			this.btnRestoreAll = new global::System.Windows.Forms.Button();
			this.cmdStop = new global::System.Windows.Forms.Button();
			this.btnStartUpload = new global::System.Windows.Forms.Button();
			this.cmdCancel = new global::System.Windows.Forms.Button();
			this.txtRunInfo = new global::System.Windows.Forms.TextBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.lblRecID = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.nudMaxLen.BeginInit();
			this.tabPage6.SuspendLayout();
			base.SuspendLayout();
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Location = new global::System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new global::System.Drawing.Size(646, 155);
			this.tabControl1.TabIndex = 61;
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.btnErrConnect);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.nudMaxLen);
			this.tabPage3.Controls.Add(this.txtServerURL);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new global::System.Drawing.Size(638, 129);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "当前设置";
			this.btnErrConnect.BackgroundImage = global::WG3000_COMM.Properties.Resources.eventlogError;
			this.btnErrConnect.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnErrConnect.FlatAppearance.BorderSize = 0;
			this.btnErrConnect.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.btnErrConnect.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnErrConnect.Location = new global::System.Drawing.Point(23, 90);
			this.btnErrConnect.Name = "btnErrConnect";
			this.btnErrConnect.Size = new global::System.Drawing.Size(24, 23);
			this.btnErrConnect.TabIndex = 67;
			this.btnErrConnect.Visible = false;
			this.label1.AutoSize = true;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new global::System.Drawing.Point(12, 25);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(107, 12);
			this.label1.TabIndex = 65;
			this.label1.Text = "一次发送(个记录):";
			this.label1.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.nudMaxLen.Location = new global::System.Drawing.Point(118, 21);
			int[] array = new int[4];
			array[0] = 1000;
			this.nudMaxLen.Maximum = new decimal(array);
			int[] array2 = new int[4];
			array2[0] = 1;
			this.nudMaxLen.Minimum = new decimal(array2);
			this.nudMaxLen.Name = "nudMaxLen";
			this.nudMaxLen.Size = new global::System.Drawing.Size(120, 21);
			this.nudMaxLen.TabIndex = 64;
			int[] array3 = new int[4];
			array3[0] = 1;
			this.nudMaxLen.Value = new decimal(array3);
			this.txtServerURL.Location = new global::System.Drawing.Point(77, 64);
			this.txtServerURL.MaxLength = 200;
			this.txtServerURL.Name = "txtServerURL";
			this.txtServerURL.Size = new global::System.Drawing.Size(552, 21);
			this.txtServerURL.TabIndex = 62;
			this.txtServerURL.Text = "http://192.168.1.153:8000/service/sendata.do";
			this.label2.AutoSize = true;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new global::System.Drawing.Point(6, 67);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(65, 12);
			this.label2.TabIndex = 61;
			this.label2.Text = "服务器URL:";
			this.tabPage6.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage6.Controls.Add(this.btnSetAllrecordSent);
			this.tabPage6.Controls.Add(this.btnRestoreAll);
			this.tabPage6.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new global::System.Drawing.Size(638, 129);
			this.tabPage6.TabIndex = 3;
			this.tabPage6.Text = "扩展功能";
			this.btnSetAllrecordSent.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnSetAllrecordSent.BackColor = global::System.Drawing.Color.Transparent;
			this.btnSetAllrecordSent.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnSetAllrecordSent.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnSetAllrecordSent.ForeColor = global::System.Drawing.Color.White;
			this.btnSetAllrecordSent.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnSetAllrecordSent.Location = new global::System.Drawing.Point(199, 15);
			this.btnSetAllrecordSent.Name = "btnSetAllrecordSent";
			this.btnSetAllrecordSent.Size = new global::System.Drawing.Size(108, 27);
			this.btnSetAllrecordSent.TabIndex = 67;
			this.btnSetAllrecordSent.Text = "标识全部已上传";
			this.btnSetAllrecordSent.UseVisualStyleBackColor = false;
			this.btnSetAllrecordSent.Click += new global::System.EventHandler(this.btnSetAllrecordSent_Click);
			this.btnRestoreAll.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnRestoreAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnRestoreAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnRestoreAll.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnRestoreAll.ForeColor = global::System.Drawing.Color.White;
			this.btnRestoreAll.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnRestoreAll.Location = new global::System.Drawing.Point(20, 15);
			this.btnRestoreAll.Name = "btnRestoreAll";
			this.btnRestoreAll.Size = new global::System.Drawing.Size(130, 27);
			this.btnRestoreAll.TabIndex = 66;
			this.btnRestoreAll.Text = "恢复所有(重新上传)";
			this.btnRestoreAll.UseVisualStyleBackColor = false;
			this.btnRestoreAll.Click += new global::System.EventHandler(this.btnRestoreAll_Click);
			this.cmdStop.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.cmdStop.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdStop.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdStop.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.cmdStop.ForeColor = global::System.Drawing.Color.White;
			this.cmdStop.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.cmdStop.Location = new global::System.Drawing.Point(453, 239);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new global::System.Drawing.Size(75, 27);
			this.cmdStop.TabIndex = 62;
			this.cmdStop.Text = "停止";
			this.cmdStop.UseVisualStyleBackColor = false;
			this.cmdStop.Click += new global::System.EventHandler(this.cmdStop_Click);
			this.btnStartUpload.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnStartUpload.BackColor = global::System.Drawing.Color.Transparent;
			this.btnStartUpload.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnStartUpload.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnStartUpload.ForeColor = global::System.Drawing.Color.White;
			this.btnStartUpload.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnStartUpload.Location = new global::System.Drawing.Point(453, 197);
			this.btnStartUpload.Name = "btnStartUpload";
			this.btnStartUpload.Size = new global::System.Drawing.Size(75, 27);
			this.btnStartUpload.TabIndex = 63;
			this.btnStartUpload.Text = "开始上传";
			this.btnStartUpload.UseVisualStyleBackColor = false;
			this.btnStartUpload.Click += new global::System.EventHandler(this.btnStartUpload_Click);
			this.cmdCancel.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this.cmdCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCancel.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.cmdCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancel.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.cmdCancel.Location = new global::System.Drawing.Point(570, 239);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new global::System.Drawing.Size(75, 27);
			this.cmdCancel.TabIndex = 64;
			this.cmdCancel.Text = "退出";
			this.cmdCancel.UseVisualStyleBackColor = false;
			this.cmdCancel.Click += new global::System.EventHandler(this.cmdCancel_Click);
			this.txtRunInfo.BackColor = global::System.Drawing.Color.White;
			this.txtRunInfo.Location = new global::System.Drawing.Point(12, 177);
			this.txtRunInfo.Multiline = true;
			this.txtRunInfo.Name = "txtRunInfo";
			this.txtRunInfo.ReadOnly = true;
			this.txtRunInfo.Size = new global::System.Drawing.Size(300, 101);
			this.txtRunInfo.TabIndex = 64;
			this.timer1.Enabled = true;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.lblRecID.AutoSize = true;
			this.lblRecID.ForeColor = global::System.Drawing.Color.White;
			this.lblRecID.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.lblRecID.Location = new global::System.Drawing.Point(318, 180);
			this.lblRecID.Name = "lblRecID";
			this.lblRecID.Size = new global::System.Drawing.Size(41, 12);
			this.lblRecID.TabIndex = 66;
			this.lblRecID.Text = "RecID=";
			this.lblRecID.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			this.label3.AutoSize = true;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new global::System.Drawing.Point(116, 101);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(329, 12);
			this.label3.TabIndex = 68;
			this.label3.Text = "说明: 转发 已提取或实时提取的刷卡记录 (允许通过的记录)";
			this.label3.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.ClientSize = new global::System.Drawing.Size(670, 290);
			base.Controls.Add(this.lblRecID);
			base.Controls.Add(this.txtRunInfo);
			base.Controls.Add(this.cmdCancel);
			base.Controls.Add(this.btnStartUpload);
			base.Controls.Add(this.cmdStop);
			base.Controls.Add(this.tabControl1);
			base.Name = "dfrmSendSwipeToCenterA";
			this.Text = "DataCenter";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmSendSwipeToCenterA_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmSendSwipeToCenterA_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.nudMaxLen.EndInit();
			this.tabPage6.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003458 RID: 13400
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003459 RID: 13401
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400345A RID: 13402
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400345B RID: 13403
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400345C RID: 13404
		private global::System.Windows.Forms.Label lblRecID;

		// Token: 0x0400345D RID: 13405
		private global::System.Windows.Forms.NumericUpDown nudMaxLen;

		// Token: 0x0400345E RID: 13406
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x0400345F RID: 13407
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04003460 RID: 13408
		private global::System.Windows.Forms.TabPage tabPage6;

		// Token: 0x04003461 RID: 13409
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04003462 RID: 13410
		private global::System.Windows.Forms.TextBox txtRunInfo;

		// Token: 0x04003463 RID: 13411
		private global::System.Windows.Forms.TextBox txtServerURL;

		// Token: 0x04003464 RID: 13412
		internal global::System.Windows.Forms.Button btnErrConnect;

		// Token: 0x04003465 RID: 13413
		internal global::System.Windows.Forms.Button btnRestoreAll;

		// Token: 0x04003466 RID: 13414
		internal global::System.Windows.Forms.Button btnSetAllrecordSent;

		// Token: 0x04003467 RID: 13415
		internal global::System.Windows.Forms.Button btnStartUpload;

		// Token: 0x04003468 RID: 13416
		internal global::System.Windows.Forms.Button cmdCancel;

		// Token: 0x04003469 RID: 13417
		internal global::System.Windows.Forms.Button cmdStop;
	}
}
