namespace WG3000_COMM.Basic
{
	// Token: 0x02000012 RID: 18
	public partial class dfrmExportTextStyle4SwipeRecord : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00021233 File Offset: 0x00020233
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00021254 File Offset: 0x00020254
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmExportTextStyle4SwipeRecord));
			this.label10 = new global::System.Windows.Forms.Label();
			this.txtExport = new global::System.Windows.Forms.TextBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.groupBox12 = new global::System.Windows.Forms.GroupBox();
			this.label13 = new global::System.Windows.Forms.Label();
			this.btnAddWeekday = new global::System.Windows.Forms.Button();
			this.cboWeekdayFormat = new global::System.Windows.Forms.ComboBox();
			this.groupBox11 = new global::System.Windows.Forms.GroupBox();
			this.label14 = new global::System.Windows.Forms.Label();
			this.cboDateSeparator = new global::System.Windows.Forms.ComboBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.btnAddDate = new global::System.Windows.Forms.Button();
			this.cboDateFormat = new global::System.Windows.Forms.ComboBox();
			this.groupBox10 = new global::System.Windows.Forms.GroupBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.btnAddTime = new global::System.Windows.Forms.Button();
			this.cboTimeFormat = new global::System.Windows.Forms.ComboBox();
			this.groupBox9 = new global::System.Windows.Forms.GroupBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.btnAddValid = new global::System.Windows.Forms.Button();
			this.groupBox8 = new global::System.Windows.Forms.GroupBox();
			this.cboDescription = new global::System.Windows.Forms.NumericUpDown();
			this.label8 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfDescription = new global::System.Windows.Forms.CheckBox();
			this.btnAddDescription = new global::System.Windows.Forms.Button();
			this.groupBox7 = new global::System.Windows.Forms.GroupBox();
			this.cboAddr = new global::System.Windows.Forms.NumericUpDown();
			this.label7 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfAddr = new global::System.Windows.Forms.CheckBox();
			this.btnAddAddr = new global::System.Windows.Forms.Button();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.cboDepartment = new global::System.Windows.Forms.NumericUpDown();
			this.label6 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfDepartment = new global::System.Windows.Forms.CheckBox();
			this.btnAddDepartment = new global::System.Windows.Forms.Button();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.cboUserName = new global::System.Windows.Forms.NumericUpDown();
			this.label5 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfUserName = new global::System.Windows.Forms.CheckBox();
			this.btnAddUserName = new global::System.Windows.Forms.Button();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfUserID = new global::System.Windows.Forms.CheckBox();
			this.btnAddUserID = new global::System.Windows.Forms.Button();
			this.cboUserID = new global::System.Windows.Forms.ComboBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfCardNO = new global::System.Windows.Forms.CheckBox();
			this.btnAddCardNO = new global::System.Windows.Forms.Button();
			this.cboCardNO = new global::System.Windows.Forms.ComboBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.chkFixedLenthOfRecID = new global::System.Windows.Forms.CheckBox();
			this.btnAddRecID = new global::System.Windows.Forms.Button();
			this.cboRecID = new global::System.Windows.Forms.ComboBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.btnAddSeparator = new global::System.Windows.Forms.Button();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.cboSeparator = new global::System.Windows.Forms.ComboBox();
			this.lstSelected = new global::System.Windows.Forms.ListBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnDeleteAll = new global::System.Windows.Forms.Button();
			this.groupBox12.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboDescription).BeginInit();
			this.groupBox7.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboAddr).BeginInit();
			this.groupBox6.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboDepartment).BeginInit();
			this.groupBox5.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboUserName).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.BackColor = global::System.Drawing.Color.Transparent;
			this.label10.ForeColor = global::System.Drawing.Color.White;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.txtExport, "txtExport");
			this.txtExport.Name = "txtExport";
			this.txtExport.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.groupBox12, "groupBox12");
			this.groupBox12.Controls.Add(this.label13);
			this.groupBox12.Controls.Add(this.btnAddWeekday);
			this.groupBox12.Controls.Add(this.cboWeekdayFormat);
			this.groupBox12.ForeColor = global::System.Drawing.Color.White;
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.TabStop = false;
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.BackColor = global::System.Drawing.Color.Transparent;
			this.label13.ForeColor = global::System.Drawing.Color.White;
			this.label13.Name = "label13";
			componentResourceManager.ApplyResources(this.btnAddWeekday, "btnAddWeekday");
			this.btnAddWeekday.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddWeekday.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddWeekday.ForeColor = global::System.Drawing.Color.White;
			this.btnAddWeekday.Name = "btnAddWeekday";
			this.btnAddWeekday.UseVisualStyleBackColor = true;
			this.btnAddWeekday.Click += new global::System.EventHandler(this.btnAddWeekday_Click);
			componentResourceManager.ApplyResources(this.cboWeekdayFormat, "cboWeekdayFormat");
			this.cboWeekdayFormat.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboWeekdayFormat.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboWeekdayFormat.Items"),
				componentResourceManager.GetString("cboWeekdayFormat.Items1")
			});
			this.cboWeekdayFormat.Name = "cboWeekdayFormat";
			componentResourceManager.ApplyResources(this.groupBox11, "groupBox11");
			this.groupBox11.Controls.Add(this.label14);
			this.groupBox11.Controls.Add(this.cboDateSeparator);
			this.groupBox11.Controls.Add(this.label12);
			this.groupBox11.Controls.Add(this.btnAddDate);
			this.groupBox11.Controls.Add(this.cboDateFormat);
			this.groupBox11.ForeColor = global::System.Drawing.Color.White;
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.TabStop = false;
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.BackColor = global::System.Drawing.Color.Transparent;
			this.label14.ForeColor = global::System.Drawing.Color.White;
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.cboDateSeparator, "cboDateSeparator");
			this.cboDateSeparator.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDateSeparator.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboDateSeparator.Items"),
				componentResourceManager.GetString("cboDateSeparator.Items1"),
				componentResourceManager.GetString("cboDateSeparator.Items2"),
				componentResourceManager.GetString("cboDateSeparator.Items3")
			});
			this.cboDateSeparator.Name = "cboDateSeparator";
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.BackColor = global::System.Drawing.Color.Transparent;
			this.label12.ForeColor = global::System.Drawing.Color.White;
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.btnAddDate, "btnAddDate");
			this.btnAddDate.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddDate.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddDate.ForeColor = global::System.Drawing.Color.White;
			this.btnAddDate.Name = "btnAddDate";
			this.btnAddDate.UseVisualStyleBackColor = true;
			this.btnAddDate.Click += new global::System.EventHandler(this.btnAddDate_Click);
			componentResourceManager.ApplyResources(this.cboDateFormat, "cboDateFormat");
			this.cboDateFormat.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDateFormat.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboDateFormat.Items"),
				componentResourceManager.GetString("cboDateFormat.Items1"),
				componentResourceManager.GetString("cboDateFormat.Items2")
			});
			this.cboDateFormat.Name = "cboDateFormat";
			componentResourceManager.ApplyResources(this.groupBox10, "groupBox10");
			this.groupBox10.Controls.Add(this.label11);
			this.groupBox10.Controls.Add(this.btnAddTime);
			this.groupBox10.Controls.Add(this.cboTimeFormat);
			this.groupBox10.ForeColor = global::System.Drawing.Color.White;
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.TabStop = false;
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.BackColor = global::System.Drawing.Color.Transparent;
			this.label11.ForeColor = global::System.Drawing.Color.White;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.btnAddTime, "btnAddTime");
			this.btnAddTime.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddTime.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddTime.ForeColor = global::System.Drawing.Color.White;
			this.btnAddTime.Name = "btnAddTime";
			this.btnAddTime.UseVisualStyleBackColor = true;
			this.btnAddTime.Click += new global::System.EventHandler(this.btnAddTime_Click);
			componentResourceManager.ApplyResources(this.cboTimeFormat, "cboTimeFormat");
			this.cboTimeFormat.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTimeFormat.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboTimeFormat.Items"),
				componentResourceManager.GetString("cboTimeFormat.Items1")
			});
			this.cboTimeFormat.Name = "cboTimeFormat";
			componentResourceManager.ApplyResources(this.groupBox9, "groupBox9");
			this.groupBox9.Controls.Add(this.label9);
			this.groupBox9.Controls.Add(this.btnAddValid);
			this.groupBox9.ForeColor = global::System.Drawing.Color.White;
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.BackColor = global::System.Drawing.Color.Transparent;
			this.label9.ForeColor = global::System.Drawing.Color.White;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.btnAddValid, "btnAddValid");
			this.btnAddValid.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddValid.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddValid.ForeColor = global::System.Drawing.Color.White;
			this.btnAddValid.Name = "btnAddValid";
			this.btnAddValid.UseVisualStyleBackColor = true;
			this.btnAddValid.Click += new global::System.EventHandler(this.btnAddValid_Click);
			componentResourceManager.ApplyResources(this.groupBox8, "groupBox8");
			this.groupBox8.Controls.Add(this.cboDescription);
			this.groupBox8.Controls.Add(this.label8);
			this.groupBox8.Controls.Add(this.chkFixedLenthOfDescription);
			this.groupBox8.Controls.Add(this.btnAddDescription);
			this.groupBox8.ForeColor = global::System.Drawing.Color.White;
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.TabStop = false;
			componentResourceManager.ApplyResources(this.cboDescription, "cboDescription");
			this.cboDescription.Name = "cboDescription";
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.cboDescription;
			int[] array = new int[4];
			array[0] = 10;
			numericUpDown.Value = new decimal(array);
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.BackColor = global::System.Drawing.Color.Transparent;
			this.label8.ForeColor = global::System.Drawing.Color.White;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfDescription, "chkFixedLenthOfDescription");
			this.chkFixedLenthOfDescription.Name = "chkFixedLenthOfDescription";
			this.chkFixedLenthOfDescription.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddDescription, "btnAddDescription");
			this.btnAddDescription.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddDescription.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddDescription.ForeColor = global::System.Drawing.Color.White;
			this.btnAddDescription.Name = "btnAddDescription";
			this.btnAddDescription.UseVisualStyleBackColor = true;
			this.btnAddDescription.Click += new global::System.EventHandler(this.btnAddDescription_Click);
			componentResourceManager.ApplyResources(this.groupBox7, "groupBox7");
			this.groupBox7.Controls.Add(this.cboAddr);
			this.groupBox7.Controls.Add(this.label7);
			this.groupBox7.Controls.Add(this.chkFixedLenthOfAddr);
			this.groupBox7.Controls.Add(this.btnAddAddr);
			this.groupBox7.ForeColor = global::System.Drawing.Color.White;
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.TabStop = false;
			componentResourceManager.ApplyResources(this.cboAddr, "cboAddr");
			this.cboAddr.Name = "cboAddr";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.cboAddr;
			int[] array2 = new int[4];
			array2[0] = 10;
			numericUpDown2.Value = new decimal(array2);
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.BackColor = global::System.Drawing.Color.Transparent;
			this.label7.ForeColor = global::System.Drawing.Color.White;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfAddr, "chkFixedLenthOfAddr");
			this.chkFixedLenthOfAddr.Name = "chkFixedLenthOfAddr";
			this.chkFixedLenthOfAddr.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddAddr, "btnAddAddr");
			this.btnAddAddr.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddAddr.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAddr.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAddr.Name = "btnAddAddr";
			this.btnAddAddr.UseVisualStyleBackColor = true;
			this.btnAddAddr.Click += new global::System.EventHandler(this.btnAddAddr_Click);
			componentResourceManager.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Controls.Add(this.cboDepartment);
			this.groupBox6.Controls.Add(this.label6);
			this.groupBox6.Controls.Add(this.chkFixedLenthOfDepartment);
			this.groupBox6.Controls.Add(this.btnAddDepartment);
			this.groupBox6.ForeColor = global::System.Drawing.Color.White;
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			componentResourceManager.ApplyResources(this.cboDepartment, "cboDepartment");
			this.cboDepartment.Name = "cboDepartment";
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.cboDepartment;
			int[] array3 = new int[4];
			array3[0] = 10;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.BackColor = global::System.Drawing.Color.Transparent;
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfDepartment, "chkFixedLenthOfDepartment");
			this.chkFixedLenthOfDepartment.Name = "chkFixedLenthOfDepartment";
			this.chkFixedLenthOfDepartment.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddDepartment, "btnAddDepartment");
			this.btnAddDepartment.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddDepartment.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddDepartment.ForeColor = global::System.Drawing.Color.White;
			this.btnAddDepartment.Name = "btnAddDepartment";
			this.btnAddDepartment.UseVisualStyleBackColor = true;
			this.btnAddDepartment.Click += new global::System.EventHandler(this.btnAddDepartment_Click);
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.cboUserName);
			this.groupBox5.Controls.Add(this.label5);
			this.groupBox5.Controls.Add(this.chkFixedLenthOfUserName);
			this.groupBox5.Controls.Add(this.btnAddUserName);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			componentResourceManager.ApplyResources(this.cboUserName, "cboUserName");
			this.cboUserName.Name = "cboUserName";
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.cboUserName;
			int[] array4 = new int[4];
			array4[0] = 10;
			numericUpDown4.Value = new decimal(array4);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.BackColor = global::System.Drawing.Color.Transparent;
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfUserName, "chkFixedLenthOfUserName");
			this.chkFixedLenthOfUserName.Name = "chkFixedLenthOfUserName";
			this.chkFixedLenthOfUserName.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddUserName, "btnAddUserName");
			this.btnAddUserName.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddUserName.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddUserName.ForeColor = global::System.Drawing.Color.White;
			this.btnAddUserName.Name = "btnAddUserName";
			this.btnAddUserName.UseVisualStyleBackColor = true;
			this.btnAddUserName.Click += new global::System.EventHandler(this.btnAddUserName_Click);
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.chkFixedLenthOfUserID);
			this.groupBox4.Controls.Add(this.btnAddUserID);
			this.groupBox4.Controls.Add(this.cboUserID);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.BackColor = global::System.Drawing.Color.Transparent;
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfUserID, "chkFixedLenthOfUserID");
			this.chkFixedLenthOfUserID.Checked = true;
			this.chkFixedLenthOfUserID.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkFixedLenthOfUserID.Name = "chkFixedLenthOfUserID";
			this.chkFixedLenthOfUserID.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddUserID, "btnAddUserID");
			this.btnAddUserID.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddUserID.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddUserID.ForeColor = global::System.Drawing.Color.White;
			this.btnAddUserID.Name = "btnAddUserID";
			this.btnAddUserID.UseVisualStyleBackColor = true;
			this.btnAddUserID.Click += new global::System.EventHandler(this.btnAddUserID_Click);
			componentResourceManager.ApplyResources(this.cboUserID, "cboUserID");
			this.cboUserID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUserID.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboUserID.Items"),
				componentResourceManager.GetString("cboUserID.Items1"),
				componentResourceManager.GetString("cboUserID.Items2"),
				componentResourceManager.GetString("cboUserID.Items3"),
				componentResourceManager.GetString("cboUserID.Items4"),
				componentResourceManager.GetString("cboUserID.Items5"),
				componentResourceManager.GetString("cboUserID.Items6")
			});
			this.cboUserID.Name = "cboUserID";
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.chkFixedLenthOfCardNO);
			this.groupBox3.Controls.Add(this.btnAddCardNO);
			this.groupBox3.Controls.Add(this.cboCardNO);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfCardNO, "chkFixedLenthOfCardNO");
			this.chkFixedLenthOfCardNO.Checked = true;
			this.chkFixedLenthOfCardNO.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkFixedLenthOfCardNO.Name = "chkFixedLenthOfCardNO";
			this.chkFixedLenthOfCardNO.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddCardNO, "btnAddCardNO");
			this.btnAddCardNO.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddCardNO.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddCardNO.ForeColor = global::System.Drawing.Color.White;
			this.btnAddCardNO.Name = "btnAddCardNO";
			this.btnAddCardNO.UseVisualStyleBackColor = true;
			this.btnAddCardNO.Click += new global::System.EventHandler(this.btnAddCardNO_Click);
			componentResourceManager.ApplyResources(this.cboCardNO, "cboCardNO");
			this.cboCardNO.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCardNO.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboCardNO.Items"),
				componentResourceManager.GetString("cboCardNO.Items1"),
				componentResourceManager.GetString("cboCardNO.Items2"),
				componentResourceManager.GetString("cboCardNO.Items3"),
				componentResourceManager.GetString("cboCardNO.Items4"),
				componentResourceManager.GetString("cboCardNO.Items5"),
				componentResourceManager.GetString("cboCardNO.Items6")
			});
			this.cboCardNO.Name = "cboCardNO";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.chkFixedLenthOfRecID);
			this.groupBox2.Controls.Add(this.btnAddRecID);
			this.groupBox2.Controls.Add(this.cboRecID);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.chkFixedLenthOfRecID, "chkFixedLenthOfRecID");
			this.chkFixedLenthOfRecID.Checked = true;
			this.chkFixedLenthOfRecID.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkFixedLenthOfRecID.Name = "chkFixedLenthOfRecID";
			this.chkFixedLenthOfRecID.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnAddRecID, "btnAddRecID");
			this.btnAddRecID.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddRecID.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddRecID.ForeColor = global::System.Drawing.Color.White;
			this.btnAddRecID.Name = "btnAddRecID";
			this.btnAddRecID.UseVisualStyleBackColor = true;
			this.btnAddRecID.Click += new global::System.EventHandler(this.btnAddRecID_Click);
			componentResourceManager.ApplyResources(this.cboRecID, "cboRecID");
			this.cboRecID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRecID.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboRecID.Items"),
				componentResourceManager.GetString("cboRecID.Items1"),
				componentResourceManager.GetString("cboRecID.Items2"),
				componentResourceManager.GetString("cboRecID.Items3"),
				componentResourceManager.GetString("cboRecID.Items4"),
				componentResourceManager.GetString("cboRecID.Items5"),
				componentResourceManager.GetString("cboRecID.Items6")
			});
			this.cboRecID.Name = "cboRecID";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.btnAddSeparator);
			this.groupBox1.Controls.Add(this.Label1);
			this.groupBox1.Controls.Add(this.cboSeparator);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.btnAddSeparator, "btnAddSeparator");
			this.btnAddSeparator.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddSeparator.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddSeparator.ForeColor = global::System.Drawing.Color.White;
			this.btnAddSeparator.Name = "btnAddSeparator";
			this.btnAddSeparator.UseVisualStyleBackColor = true;
			this.btnAddSeparator.Click += new global::System.EventHandler(this.btnAddSeparator_Click);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.cboSeparator, "cboSeparator");
			this.cboSeparator.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSeparator.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboSeparator.Items"),
				componentResourceManager.GetString("cboSeparator.Items1"),
				componentResourceManager.GetString("cboSeparator.Items2"),
				componentResourceManager.GetString("cboSeparator.Items3"),
				componentResourceManager.GetString("cboSeparator.Items4"),
				componentResourceManager.GetString("cboSeparator.Items5"),
				componentResourceManager.GetString("cboSeparator.Items6"),
				componentResourceManager.GetString("cboSeparator.Items7"),
				componentResourceManager.GetString("cboSeparator.Items8"),
				componentResourceManager.GetString("cboSeparator.Items9")
			});
			this.cboSeparator.Name = "cboSeparator";
			componentResourceManager.ApplyResources(this.lstSelected, "lstSelected");
			this.lstSelected.Name = "lstSelected";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAll, "btnDeleteAll");
			this.btnDeleteAll.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAll.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAll.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAll.Name = "btnDeleteAll";
			this.btnDeleteAll.UseVisualStyleBackColor = false;
			this.btnDeleteAll.Click += new global::System.EventHandler(this.btnDeleteAll_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label10);
			base.Controls.Add(this.txtExport);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.groupBox12);
			base.Controls.Add(this.groupBox11);
			base.Controls.Add(this.groupBox10);
			base.Controls.Add(this.groupBox9);
			base.Controls.Add(this.groupBox8);
			base.Controls.Add(this.groupBox7);
			base.Controls.Add(this.groupBox6);
			base.Controls.Add(this.groupBox5);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.lstSelected);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnDeleteAll);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "dfrmExportTextStyle4SwipeRecord";
			base.Load += new global::System.EventHandler(this.dfrmExportTextStyle4SwipeRecord_Load);
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboDescription).EndInit();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboAddr).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboDepartment).EndInit();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.cboUserName).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400021F RID: 543
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000220 RID: 544
		private global::System.Windows.Forms.NumericUpDown cboAddr;

		// Token: 0x04000221 RID: 545
		private global::System.Windows.Forms.NumericUpDown cboDepartment;

		// Token: 0x04000222 RID: 546
		private global::System.Windows.Forms.NumericUpDown cboDescription;

		// Token: 0x04000223 RID: 547
		private global::System.Windows.Forms.NumericUpDown cboUserName;

		// Token: 0x04000224 RID: 548
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfAddr;

		// Token: 0x04000225 RID: 549
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfCardNO;

		// Token: 0x04000226 RID: 550
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfDepartment;

		// Token: 0x04000227 RID: 551
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfDescription;

		// Token: 0x04000228 RID: 552
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfRecID;

		// Token: 0x04000229 RID: 553
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfUserID;

		// Token: 0x0400022A RID: 554
		private global::System.Windows.Forms.CheckBox chkFixedLenthOfUserName;

		// Token: 0x0400022B RID: 555
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400022C RID: 556
		private global::System.Windows.Forms.GroupBox groupBox10;

		// Token: 0x0400022D RID: 557
		private global::System.Windows.Forms.GroupBox groupBox11;

		// Token: 0x0400022E RID: 558
		private global::System.Windows.Forms.GroupBox groupBox12;

		// Token: 0x0400022F RID: 559
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000230 RID: 560
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000231 RID: 561
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000232 RID: 562
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04000233 RID: 563
		private global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x04000234 RID: 564
		private global::System.Windows.Forms.GroupBox groupBox7;

		// Token: 0x04000235 RID: 565
		private global::System.Windows.Forms.GroupBox groupBox8;

		// Token: 0x04000236 RID: 566
		private global::System.Windows.Forms.GroupBox groupBox9;

		// Token: 0x04000237 RID: 567
		private global::System.Windows.Forms.TextBox txtExport;

		// Token: 0x04000238 RID: 568
		internal global::System.Windows.Forms.Button btnAddAddr;

		// Token: 0x04000239 RID: 569
		internal global::System.Windows.Forms.Button btnAddCardNO;

		// Token: 0x0400023A RID: 570
		internal global::System.Windows.Forms.Button btnAddDate;

		// Token: 0x0400023B RID: 571
		internal global::System.Windows.Forms.Button btnAddDepartment;

		// Token: 0x0400023C RID: 572
		internal global::System.Windows.Forms.Button btnAddDescription;

		// Token: 0x0400023D RID: 573
		internal global::System.Windows.Forms.Button btnAddRecID;

		// Token: 0x0400023E RID: 574
		internal global::System.Windows.Forms.Button btnAddSeparator;

		// Token: 0x0400023F RID: 575
		internal global::System.Windows.Forms.Button btnAddTime;

		// Token: 0x04000240 RID: 576
		internal global::System.Windows.Forms.Button btnAddUserID;

		// Token: 0x04000241 RID: 577
		internal global::System.Windows.Forms.Button btnAddUserName;

		// Token: 0x04000242 RID: 578
		internal global::System.Windows.Forms.Button btnAddValid;

		// Token: 0x04000243 RID: 579
		internal global::System.Windows.Forms.Button btnAddWeekday;

		// Token: 0x04000244 RID: 580
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000245 RID: 581
		internal global::System.Windows.Forms.Button btnDeleteAll;

		// Token: 0x04000246 RID: 582
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000247 RID: 583
		internal global::System.Windows.Forms.ComboBox cboCardNO;

		// Token: 0x04000248 RID: 584
		internal global::System.Windows.Forms.ComboBox cboDateFormat;

		// Token: 0x04000249 RID: 585
		internal global::System.Windows.Forms.ComboBox cboDateSeparator;

		// Token: 0x0400024A RID: 586
		internal global::System.Windows.Forms.ComboBox cboRecID;

		// Token: 0x0400024B RID: 587
		internal global::System.Windows.Forms.ComboBox cboSeparator;

		// Token: 0x0400024C RID: 588
		internal global::System.Windows.Forms.ComboBox cboTimeFormat;

		// Token: 0x0400024D RID: 589
		internal global::System.Windows.Forms.ComboBox cboUserID;

		// Token: 0x0400024E RID: 590
		internal global::System.Windows.Forms.ComboBox cboWeekdayFormat;

		// Token: 0x0400024F RID: 591
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x04000250 RID: 592
		internal global::System.Windows.Forms.Label label10;

		// Token: 0x04000251 RID: 593
		internal global::System.Windows.Forms.Label label11;

		// Token: 0x04000252 RID: 594
		internal global::System.Windows.Forms.Label label12;

		// Token: 0x04000253 RID: 595
		internal global::System.Windows.Forms.Label label13;

		// Token: 0x04000254 RID: 596
		internal global::System.Windows.Forms.Label label14;

		// Token: 0x04000255 RID: 597
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x04000256 RID: 598
		internal global::System.Windows.Forms.Label label3;

		// Token: 0x04000257 RID: 599
		internal global::System.Windows.Forms.Label label4;

		// Token: 0x04000258 RID: 600
		internal global::System.Windows.Forms.Label label5;

		// Token: 0x04000259 RID: 601
		internal global::System.Windows.Forms.Label label6;

		// Token: 0x0400025A RID: 602
		internal global::System.Windows.Forms.Label label7;

		// Token: 0x0400025B RID: 603
		internal global::System.Windows.Forms.Label label8;

		// Token: 0x0400025C RID: 604
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x0400025D RID: 605
		internal global::System.Windows.Forms.ListBox lstSelected;
	}
}
