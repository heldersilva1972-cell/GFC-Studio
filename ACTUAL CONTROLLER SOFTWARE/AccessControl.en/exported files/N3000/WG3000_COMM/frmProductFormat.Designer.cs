namespace WG3000_COMM
{
	// Token: 0x0200032D RID: 813
	public partial class frmProductFormat : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060019C5 RID: 6597 RVA: 0x0021BDC8 File Offset: 0x0021ADC8
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0021BDE8 File Offset: 0x0021ADE8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.frmProductFormat));
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.btnPing = new global::System.Windows.Forms.Button();
			this.lblCommLose = new global::System.Windows.Forms.Label();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.checkBox2 = new global::System.Windows.Forms.CheckBox();
			this.lblMultiInputInfo4Connected = new global::System.Windows.Forms.Label();
			this.label13 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.optNO = new global::System.Windows.Forms.RadioButton();
			this.label11 = new global::System.Windows.Forms.Label();
			this.numericUpDown2 = new global::System.Windows.Forms.NumericUpDown();
			this.label10 = new global::System.Windows.Forms.Label();
			this.numericUpDown1 = new global::System.Windows.Forms.NumericUpDown();
			this.btnStop = new global::System.Windows.Forms.Button();
			this.lblFloor = new global::System.Windows.Forms.Label();
			this.label143 = new global::System.Windows.Forms.Label();
			this.numericUpDown22 = new global::System.Windows.Forms.NumericUpDown();
			this.button72 = new global::System.Windows.Forms.Button();
			this.checkBox131 = new global::System.Windows.Forms.CheckBox();
			this.checkBox130 = new global::System.Windows.Forms.CheckBox();
			this.button70 = new global::System.Windows.Forms.Button();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.label146 = new global::System.Windows.Forms.Label();
			this.textBox32 = new global::System.Windows.Forms.TextBox();
			this.button57 = new global::System.Windows.Forms.Button();
			this.checkBox117 = new global::System.Windows.Forms.CheckBox();
			this.checkBox116 = new global::System.Windows.Forms.CheckBox();
			this.checkBox113 = new global::System.Windows.Forms.CheckBox();
			this.txtRunInfo = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.lblFailDetail = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.btnConnected = new global::System.Windows.Forms.Button();
			this.chkAutoFormat = new global::System.Windows.Forms.CheckBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			this.btnFormat = new global::System.Windows.Forms.Button();
			this.btnAdjustTime = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.lblTime = new global::System.Windows.Forms.Label();
			this.txtTime = new global::System.Windows.Forms.TextBox();
			this.txtSN = new global::System.Windows.Forms.TextBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.btnEditCode = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.label147 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.test100LosePacketToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.numericUpDown2.BeginInit();
			this.numericUpDown1.BeginInit();
			this.numericUpDown22.BeginInit();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			this.btnPing.Location = new global::System.Drawing.Point(827, 97);
			this.btnPing.Name = "btnPing";
			this.btnPing.Size = new global::System.Drawing.Size(75, 23);
			this.btnPing.TabIndex = 106;
			this.btnPing.Text = "检测丢包";
			this.btnPing.UseVisualStyleBackColor = true;
			this.btnPing.Click += new global::System.EventHandler(this.btnPing_Click);
			this.lblCommLose.AutoSize = true;
			this.lblCommLose.BackColor = global::System.Drawing.Color.Red;
			this.lblCommLose.ForeColor = global::System.Drawing.Color.White;
			this.lblCommLose.Location = new global::System.Drawing.Point(350, 75);
			this.lblCommLose.Name = "lblCommLose";
			this.lblCommLose.Size = new global::System.Drawing.Size(65, 12);
			this.lblCommLose.TabIndex = 105;
			this.lblCommLose.Text = "通信有丢包";
			this.lblCommLose.Visible = false;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new global::System.Drawing.Point(12, 405);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new global::System.Drawing.Size(946, 204);
			this.tabControl1.TabIndex = 10;
			this.tabPage1.Controls.Add(this.checkBox2);
			this.tabPage1.Controls.Add(this.lblMultiInputInfo4Connected);
			this.tabPage1.Controls.Add(this.label13);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.label11);
			this.tabPage1.Controls.Add(this.numericUpDown2);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Controls.Add(this.numericUpDown1);
			this.tabPage1.Controls.Add(this.btnStop);
			this.tabPage1.Controls.Add(this.lblFloor);
			this.tabPage1.Controls.Add(this.label143);
			this.tabPage1.Controls.Add(this.numericUpDown22);
			this.tabPage1.Controls.Add(this.button72);
			this.tabPage1.Controls.Add(this.checkBox131);
			this.tabPage1.Controls.Add(this.checkBox130);
			this.tabPage1.Controls.Add(this.button70);
			this.tabPage1.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new global::System.Drawing.Size(938, 178);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "电梯";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.checkBox2.AutoSize = true;
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new global::System.Drawing.Point(702, 11);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new global::System.Drawing.Size(216, 16);
			this.checkBox2.TabIndex = 110;
			this.checkBox2.Text = "2_对应继电器动作(只有一个连通时)";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.lblMultiInputInfo4Connected.AutoSize = true;
			this.lblMultiInputInfo4Connected.Font = new global::System.Drawing.Font("宋体", 72f);
			this.lblMultiInputInfo4Connected.Location = new global::System.Drawing.Point(655, 30);
			this.lblMultiInputInfo4Connected.Name = "lblMultiInputInfo4Connected";
			this.lblMultiInputInfo4Connected.Size = new global::System.Drawing.Size(138, 97);
			this.lblMultiInputInfo4Connected.TabIndex = 109;
			this.lblMultiInputInfo4Connected.Text = "--";
			this.label13.AutoSize = true;
			this.label13.Location = new global::System.Drawing.Point(600, 13);
			this.label13.Name = "label13";
			this.label13.Size = new global::System.Drawing.Size(83, 12);
			this.label13.TabIndex = 108;
			this.label13.Text = "短接端子信息:";
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.optNO);
			this.groupBox1.Location = new global::System.Drawing.Point(20, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(150, 35);
			this.groupBox1.TabIndex = 107;
			this.groupBox1.TabStop = false;
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new global::System.Drawing.Point(99, 12);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new global::System.Drawing.Size(35, 16);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "NC";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.optNO.AutoSize = true;
			this.optNO.Checked = true;
			this.optNO.Location = new global::System.Drawing.Point(12, 12);
			this.optNO.Name = "optNO";
			this.optNO.Size = new global::System.Drawing.Size(35, 16);
			this.optNO.TabIndex = 0;
			this.optNO.TabStop = true;
			this.optNO.Text = "NO";
			this.optNO.UseVisualStyleBackColor = true;
			this.label11.AutoSize = true;
			this.label11.Location = new global::System.Drawing.Point(239, 111);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(95, 12);
			this.label11.TabIndex = 106;
			this.label11.Text = "21-40的开始楼层";
			this.numericUpDown2.Location = new global::System.Drawing.Point(337, 106);
			int[] array = new int[4];
			array[0] = 40;
			this.numericUpDown2.Maximum = new decimal(array);
			int[] array2 = new int[4];
			array2[0] = 21;
			this.numericUpDown2.Minimum = new decimal(array2);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new global::System.Drawing.Size(53, 21);
			this.numericUpDown2.TabIndex = 105;
			int[] array3 = new int[4];
			array3[0] = 21;
			this.numericUpDown2.Value = new decimal(array3);
			this.label10.AutoSize = true;
			this.label10.Location = new global::System.Drawing.Point(239, 78);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(89, 12);
			this.label10.TabIndex = 104;
			this.label10.Text = "1-20的开始楼层";
			this.numericUpDown1.Location = new global::System.Drawing.Point(337, 73);
			int[] array4 = new int[4];
			array4[0] = 20;
			this.numericUpDown1.Maximum = new decimal(array4);
			int[] array5 = new int[4];
			array5[0] = 1;
			this.numericUpDown1.Minimum = new decimal(array5);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new global::System.Drawing.Size(53, 21);
			this.numericUpDown1.TabIndex = 103;
			int[] array6 = new int[4];
			array6[0] = 1;
			this.numericUpDown1.Value = new decimal(array6);
			this.btnStop.Location = new global::System.Drawing.Point(437, 73);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new global::System.Drawing.Size(192, 50);
			this.btnStop.TabIndex = 102;
			this.btnStop.Text = "停止远程电梯";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new global::System.EventHandler(this.btnStop_Click);
			this.lblFloor.AutoSize = true;
			this.lblFloor.Location = new global::System.Drawing.Point(97, 51);
			this.lblFloor.Name = "lblFloor";
			this.lblFloor.Size = new global::System.Drawing.Size(29, 12);
			this.lblFloor.TabIndex = 101;
			this.lblFloor.Text = "----";
			this.label143.AutoSize = true;
			this.label143.Location = new global::System.Drawing.Point(176, 21);
			this.label143.Name = "label143";
			this.label143.Size = new global::System.Drawing.Size(83, 12);
			this.label143.TabIndex = 100;
			this.label143.Text = "22_间隔(毫秒)";
			this.numericUpDown22.Location = new global::System.Drawing.Point(261, 16);
			int[] array7 = new int[4];
			array7[0] = 20000;
			this.numericUpDown22.Maximum = new decimal(array7);
			this.numericUpDown22.Name = "numericUpDown22";
			this.numericUpDown22.Size = new global::System.Drawing.Size(53, 21);
			this.numericUpDown22.TabIndex = 99;
			int[] array8 = new int[4];
			array8[0] = 2500;
			this.numericUpDown22.Value = new decimal(array8);
			this.button72.Location = new global::System.Drawing.Point(20, 107);
			this.button72.Name = "button72";
			this.button72.Size = new global::System.Drawing.Size(192, 20);
			this.button72.TabIndex = 98;
			this.button72.Text = "72 远程到21-40楼层[IP]";
			this.button72.UseVisualStyleBackColor = true;
			this.button72.Click += new global::System.EventHandler(this.button70_Click);
			this.checkBox131.AutoSize = true;
			this.checkBox131.Checked = true;
			this.checkBox131.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox131.Location = new global::System.Drawing.Point(467, 13);
			this.checkBox131.Name = "checkBox131";
			this.checkBox131.Size = new global::System.Drawing.Size(60, 16);
			this.checkBox131.TabIndex = 97;
			this.checkBox131.Text = "131_NC";
			this.checkBox131.UseVisualStyleBackColor = true;
			this.checkBox131.Visible = false;
			this.checkBox130.AutoSize = true;
			this.checkBox130.Checked = true;
			this.checkBox130.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox130.Location = new global::System.Drawing.Point(388, 13);
			this.checkBox130.Name = "checkBox130";
			this.checkBox130.Size = new global::System.Drawing.Size(60, 16);
			this.checkBox130.TabIndex = 96;
			this.checkBox130.Text = "130_NO";
			this.checkBox130.UseVisualStyleBackColor = true;
			this.checkBox130.Visible = false;
			this.button70.Location = new global::System.Drawing.Point(20, 71);
			this.button70.Name = "button70";
			this.button70.Size = new global::System.Drawing.Size(192, 20);
			this.button70.TabIndex = 95;
			this.button70.Text = "70 远程到1-20楼层[IP]";
			this.button70.UseVisualStyleBackColor = true;
			this.button70.Click += new global::System.EventHandler(this.button70_Click);
			this.tabPage2.Controls.Add(this.label146);
			this.tabPage2.Controls.Add(this.textBox32);
			this.tabPage2.Controls.Add(this.button57);
			this.tabPage2.Controls.Add(this.checkBox117);
			this.tabPage2.Controls.Add(this.checkBox116);
			this.tabPage2.Controls.Add(this.checkBox113);
			this.tabPage2.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new global::System.Drawing.Size(938, 178);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.label146.AutoSize = true;
			this.label146.Location = new global::System.Drawing.Point(375, 12);
			this.label146.Name = "label146";
			this.label146.Size = new global::System.Drawing.Size(101, 12);
			this.label146.TabIndex = 25;
			this.label146.Text = "用 逗号 分开卡号";
			this.textBox32.Location = new global::System.Drawing.Point(376, 27);
			this.textBox32.Multiline = true;
			this.textBox32.Name = "textBox32";
			this.textBox32.Size = new global::System.Drawing.Size(187, 140);
			this.textBox32.TabIndex = 24;
			this.textBox32.Text = "7314494,  3659085, 707080, 3654261, 20760517, 3660918";
			this.button57.Location = new global::System.Drawing.Point(204, 17);
			this.button57.Name = "button57";
			this.button57.Size = new global::System.Drawing.Size(126, 38);
			this.button57.TabIndex = 22;
			this.button57.Text = "57 先作参数初始化 再作特殊设置 ";
			this.button57.UseVisualStyleBackColor = true;
			this.button57.Click += new global::System.EventHandler(this.button57_Click);
			this.checkBox117.AutoSize = true;
			this.checkBox117.Checked = true;
			this.checkBox117.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox117.Location = new global::System.Drawing.Point(26, 123);
			this.checkBox117.Name = "checkBox117";
			this.checkBox117.Size = new global::System.Drawing.Size(258, 16);
			this.checkBox117.TabIndex = 21;
			this.checkBox117.Text = "117 (恢复默认参数后) 门磁对应扩展板输出";
			this.checkBox117.UseVisualStyleBackColor = true;
			this.checkBox116.AutoSize = true;
			this.checkBox116.Checked = true;
			this.checkBox116.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox116.Location = new global::System.Drawing.Point(26, 20);
			this.checkBox116.Name = "checkBox116";
			this.checkBox116.Size = new global::System.Drawing.Size(120, 16);
			this.checkBox116.TabIndex = 20;
			this.checkBox116.Text = "116 加入卡号权限";
			this.checkBox116.UseVisualStyleBackColor = true;
			this.checkBox113.AutoSize = true;
			this.checkBox113.Checked = true;
			this.checkBox113.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox113.Location = new global::System.Drawing.Point(26, 101);
			this.checkBox113.Name = "checkBox113";
			this.checkBox113.Size = new global::System.Drawing.Size(120, 16);
			this.checkBox113.TabIndex = 19;
			this.checkBox113.Text = "113 同时校准时间";
			this.checkBox113.UseVisualStyleBackColor = true;
			this.txtRunInfo.BackColor = global::System.Drawing.Color.White;
			this.txtRunInfo.Location = new global::System.Drawing.Point(12, 6);
			this.txtRunInfo.Multiline = true;
			this.txtRunInfo.Name = "txtRunInfo";
			this.txtRunInfo.ReadOnly = true;
			this.txtRunInfo.Size = new global::System.Drawing.Size(225, 255);
			this.txtRunInfo.TabIndex = 9;
			this.label4.AutoSize = true;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Location = new global::System.Drawing.Point(561, 69);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(17, 12);
			this.label4.TabIndex = 8;
			this.label4.Text = "00";
			this.label7.AutoSize = true;
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Location = new global::System.Drawing.Point(562, 6);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(17, 12);
			this.label7.TabIndex = 8;
			this.label7.Text = "00";
			this.label3.AutoSize = true;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Location = new global::System.Drawing.Point(561, 47);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(17, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "00";
			this.label5.AutoSize = true;
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Location = new global::System.Drawing.Point(481, 69);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(47, 12);
			this.label5.TabIndex = 8;
			this.label5.Text = "故障号:";
			this.label6.AutoSize = true;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Location = new global::System.Drawing.Point(481, 6);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(59, 12);
			this.label6.TabIndex = 8;
			this.label6.Text = "驱动版本:";
			this.label9.AutoSize = true;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Location = new global::System.Drawing.Point(735, 190);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(167, 48);
			this.label9.TabIndex = 8;
			this.label9.Text = "104 时表示只是时钟问题; \r\n103 时表示时钟和100脚有问题\r\n其他百位=1表示时钟问题\r\n    十个=相应管脚问题";
			this.label9.Visible = false;
			this.label8.AutoSize = true;
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.Location = new global::System.Drawing.Point(481, 25);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(35, 12);
			this.label8.TabIndex = 8;
			this.label8.Text = "时钟:";
			this.lblFailDetail.AutoSize = true;
			this.lblFailDetail.ForeColor = global::System.Drawing.Color.White;
			this.lblFailDetail.Location = new global::System.Drawing.Point(610, 47);
			this.lblFailDetail.Name = "lblFailDetail";
			this.lblFailDetail.Size = new global::System.Drawing.Size(35, 12);
			this.lblFailDetail.TabIndex = 8;
			this.lblFailDetail.Text = "说明:";
			this.label2.AutoSize = true;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Location = new global::System.Drawing.Point(481, 47);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(83, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "有问题管脚号:";
			this.btnConnected.BackColor = global::System.Drawing.Color.Red;
			this.btnConnected.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.btnConnected.ForeColor = global::System.Drawing.Color.White;
			this.btnConnected.Location = new global::System.Drawing.Point(345, 25);
			this.btnConnected.Name = "btnConnected";
			this.btnConnected.Size = new global::System.Drawing.Size(126, 42);
			this.btnConnected.TabIndex = 7;
			this.btnConnected.UseVisualStyleBackColor = false;
			this.chkAutoFormat.AutoSize = true;
			this.chkAutoFormat.ForeColor = global::System.Drawing.Color.White;
			this.chkAutoFormat.Location = new global::System.Drawing.Point(244, 5);
			this.chkAutoFormat.Name = "chkAutoFormat";
			this.chkAutoFormat.Size = new global::System.Drawing.Size(156, 16);
			this.chkAutoFormat.TabIndex = 6;
			this.chkAutoFormat.Text = "全部通过时, 执行格式化";
			this.chkAutoFormat.UseVisualStyleBackColor = true;
			this.chkAutoFormat.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			this.checkBox1.AutoSize = true;
			this.checkBox1.ForeColor = global::System.Drawing.Color.White;
			this.checkBox1.Location = new global::System.Drawing.Point(814, 35);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new global::System.Drawing.Size(144, 16);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "停止搜索, 执行格式化";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			this.btnFormat.Location = new global::System.Drawing.Point(827, 64);
			this.btnFormat.Name = "btnFormat";
			this.btnFormat.Size = new global::System.Drawing.Size(75, 23);
			this.btnFormat.TabIndex = 5;
			this.btnFormat.Text = "格式化";
			this.btnFormat.UseVisualStyleBackColor = true;
			this.btnFormat.Visible = false;
			this.btnFormat.Click += new global::System.EventHandler(this.button2_Click);
			this.btnAdjustTime.Location = new global::System.Drawing.Point(738, 64);
			this.btnAdjustTime.Name = "btnAdjustTime";
			this.btnAdjustTime.Size = new global::System.Drawing.Size(75, 23);
			this.btnAdjustTime.TabIndex = 4;
			this.btnAdjustTime.Text = "校准时间";
			this.btnAdjustTime.UseVisualStyleBackColor = true;
			this.btnAdjustTime.Click += new global::System.EventHandler(this.btnAdjustTime_Click);
			this.button1.Location = new global::System.Drawing.Point(238, 27);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(106, 52);
			this.button1.TabIndex = 4;
			this.button1.Text = "清空运行信息\r\n\r\n允许重新格式化";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Location = new global::System.Drawing.Point(733, 97);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(94, 21);
			this.label1.TabIndex = 3;
			this.label1.Text = "电脑时间";
			this.lblTime.AutoSize = true;
			this.lblTime.Font = new global::System.Drawing.Font("宋体", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.lblTime.ForeColor = global::System.Drawing.Color.White;
			this.lblTime.Location = new global::System.Drawing.Point(733, 128);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new global::System.Drawing.Size(76, 21);
			this.lblTime.TabIndex = 2;
			this.lblTime.Text = "label1";
			this.txtTime.BackColor = global::System.Drawing.Color.White;
			this.txtTime.Font = new global::System.Drawing.Font("宋体", 72f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.txtTime.ForeColor = global::System.Drawing.Color.Black;
			this.txtTime.Location = new global::System.Drawing.Point(12, 264);
			this.txtTime.Name = "txtTime";
			this.txtTime.ReadOnly = true;
			this.txtTime.Size = new global::System.Drawing.Size(946, 117);
			this.txtTime.TabIndex = 1;
			this.txtTime.Text = "2010-10-28 12:59:59";
			this.txtTime.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.txtSN.BackColor = global::System.Drawing.Color.White;
			this.txtSN.Font = new global::System.Drawing.Font("宋体", 72f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.txtSN.ForeColor = global::System.Drawing.Color.Black;
			this.txtSN.Location = new global::System.Drawing.Point(243, 96);
			this.txtSN.Name = "txtSN";
			this.txtSN.ReadOnly = true;
			this.txtSN.Size = new global::System.Drawing.Size(490, 117);
			this.txtSN.TabIndex = 0;
			this.txtSN.Text = "999999999";
			this.txtSN.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.label12.AutoSize = true;
			this.label12.BackColor = global::System.Drawing.Color.Red;
			this.label12.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label12.ForeColor = global::System.Drawing.Color.White;
			this.label12.Location = new global::System.Drawing.Point(239, 241);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(609, 20);
			this.label12.TabIndex = 107;
			this.label12.Text = "格式化时, 必须控制器格式化完成后(CPU灯正常闪烁), 才能断电!!!";
			this.label12.Visible = false;
			this.btnEditCode.Location = new global::System.Drawing.Point(865, 4);
			this.btnEditCode.Name = "btnEditCode";
			this.btnEditCode.Size = new global::System.Drawing.Size(89, 23);
			this.btnEditCode.TabIndex = 108;
			this.btnEditCode.Text = "修改条码关系";
			this.btnEditCode.UseVisualStyleBackColor = true;
			this.btnEditCode.Visible = false;
			this.btnEditCode.Click += new global::System.EventHandler(this.btnEditCode_Click);
			this.panel1.Controls.Add(this.label147);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(971, 391);
			this.panel1.TabIndex = 104;
			this.label147.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.label147.AutoSize = true;
			this.label147.ForeColor = global::System.Drawing.Color.White;
			this.label147.Location = new global::System.Drawing.Point(42, 18);
			this.label147.Name = "label147";
			this.label147.Size = new global::System.Drawing.Size(389, 156);
			this.label147.TabIndex = 0;
			this.label147.Text = componentResourceManager.GetString("label147.Text");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.test100LosePacketToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(195, 48);
			this.test100LosePacketToolStripMenuItem.Name = "test100LosePacketToolStripMenuItem";
			this.test100LosePacketToolStripMenuItem.Size = new global::System.Drawing.Size(194, 22);
			this.test100LosePacketToolStripMenuItem.Text = "Test 100 LosePacket";
			this.test100LosePacketToolStripMenuItem.Click += new global::System.EventHandler(this.test100LosePacketToolStripMenuItem_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.ClientSize = new global::System.Drawing.Size(971, 391);
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.btnPing);
			base.Controls.Add(this.lblCommLose);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.txtRunInfo);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.lblFailDetail);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.btnConnected);
			base.Controls.Add(this.chkAutoFormat);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.btnFormat);
			base.Controls.Add(this.btnAdjustTime);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.lblTime);
			base.Controls.Add(this.txtTime);
			base.Controls.Add(this.txtSN);
			base.Controls.Add(this.label12);
			base.Controls.Add(this.btnEditCode);
			base.Controls.Add(this.panel1);
			base.Name = "frmProductFormat";
			this.Text = "Search And Format ";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmProductFormat_FormClosing);
			base.Load += new global::System.EventHandler(this.frmProductFormat_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.frmProductFormat_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.numericUpDown2.EndInit();
			this.numericUpDown1.EndInit();
			this.numericUpDown22.EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400347D RID: 13437
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400347E RID: 13438
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x0400347F RID: 13439
		private global::System.Windows.Forms.Button btnAdjustTime;

		// Token: 0x04003480 RID: 13440
		private global::System.Windows.Forms.Button btnConnected;

		// Token: 0x04003481 RID: 13441
		private global::System.Windows.Forms.Button btnEditCode;

		// Token: 0x04003482 RID: 13442
		private global::System.Windows.Forms.Button btnFormat;

		// Token: 0x04003483 RID: 13443
		private global::System.Windows.Forms.Button btnPing;

		// Token: 0x04003484 RID: 13444
		private global::System.Windows.Forms.Button btnStop;

		// Token: 0x04003485 RID: 13445
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04003486 RID: 13446
		private global::System.Windows.Forms.Button button57;

		// Token: 0x04003487 RID: 13447
		private global::System.Windows.Forms.Button button70;

		// Token: 0x04003488 RID: 13448
		private global::System.Windows.Forms.Button button72;

		// Token: 0x04003489 RID: 13449
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x0400348A RID: 13450
		private global::System.Windows.Forms.CheckBox checkBox113;

		// Token: 0x0400348B RID: 13451
		private global::System.Windows.Forms.CheckBox checkBox116;

		// Token: 0x0400348C RID: 13452
		private global::System.Windows.Forms.CheckBox checkBox117;

		// Token: 0x0400348D RID: 13453
		private global::System.Windows.Forms.CheckBox checkBox130;

		// Token: 0x0400348E RID: 13454
		private global::System.Windows.Forms.CheckBox checkBox131;

		// Token: 0x0400348F RID: 13455
		private global::System.Windows.Forms.CheckBox checkBox2;

		// Token: 0x04003490 RID: 13456
		private global::System.Windows.Forms.CheckBox chkAutoFormat;

		// Token: 0x04003491 RID: 13457
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04003492 RID: 13458
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04003493 RID: 13459
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04003494 RID: 13460
		private global::System.Windows.Forms.Label label10;

		// Token: 0x04003495 RID: 13461
		private global::System.Windows.Forms.Label label11;

		// Token: 0x04003496 RID: 13462
		private global::System.Windows.Forms.Label label12;

		// Token: 0x04003497 RID: 13463
		private global::System.Windows.Forms.Label label13;

		// Token: 0x04003498 RID: 13464
		private global::System.Windows.Forms.Label label143;

		// Token: 0x04003499 RID: 13465
		private global::System.Windows.Forms.Label label146;

		// Token: 0x0400349A RID: 13466
		private global::System.Windows.Forms.Label label147;

		// Token: 0x0400349B RID: 13467
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400349C RID: 13468
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400349D RID: 13469
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400349E RID: 13470
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400349F RID: 13471
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040034A0 RID: 13472
		private global::System.Windows.Forms.Label label7;

		// Token: 0x040034A1 RID: 13473
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040034A2 RID: 13474
		private global::System.Windows.Forms.Label label9;

		// Token: 0x040034A3 RID: 13475
		private global::System.Windows.Forms.Label lblCommLose;

		// Token: 0x040034A4 RID: 13476
		private global::System.Windows.Forms.Label lblFailDetail;

		// Token: 0x040034A5 RID: 13477
		private global::System.Windows.Forms.Label lblFloor;

		// Token: 0x040034A6 RID: 13478
		private global::System.Windows.Forms.Label lblMultiInputInfo4Connected;

		// Token: 0x040034A7 RID: 13479
		private global::System.Windows.Forms.Label lblTime;

		// Token: 0x040034A8 RID: 13480
		private global::System.Windows.Forms.NumericUpDown numericUpDown1;

		// Token: 0x040034A9 RID: 13481
		private global::System.Windows.Forms.NumericUpDown numericUpDown2;

		// Token: 0x040034AA RID: 13482
		private global::System.Windows.Forms.NumericUpDown numericUpDown22;

		// Token: 0x040034AB RID: 13483
		private global::System.Windows.Forms.RadioButton optNO;

		// Token: 0x040034AC RID: 13484
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040034AD RID: 13485
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x040034AE RID: 13486
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040034AF RID: 13487
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040034B0 RID: 13488
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040034B1 RID: 13489
		private global::System.Windows.Forms.ToolStripMenuItem test100LosePacketToolStripMenuItem;

		// Token: 0x040034B2 RID: 13490
		private global::System.Windows.Forms.TextBox textBox32;

		// Token: 0x040034B3 RID: 13491
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040034B4 RID: 13492
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x040034B5 RID: 13493
		private global::System.Windows.Forms.TextBox txtRunInfo;

		// Token: 0x040034B6 RID: 13494
		private global::System.Windows.Forms.TextBox txtSN;

		// Token: 0x040034B7 RID: 13495
		private global::System.Windows.Forms.TextBox txtTime;
	}
}
