namespace WG3000_COMM.Basic
{
	// Token: 0x0200000A RID: 10
	public partial class dfrmController : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00013760 File Offset: 0x00012760
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.m_Controller != null)
			{
				this.m_Controller.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00013798 File Offset: 0x00012798
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmController));
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnNext = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.grpbController = new global::System.Windows.Forms.GroupBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.btnZoneManage = new global::System.Windows.Forms.Button();
			this.cbof_Zone = new global::System.Windows.Forms.ComboBox();
			this.label25 = new global::System.Windows.Forms.Label();
			this.grpbIP = new global::System.Windows.Forms.GroupBox();
			this.nudPort = new global::System.Windows.Forms.NumericUpDown();
			this.txtControllerIP = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.chkControllerActive = new global::System.Windows.Forms.CheckBox();
			this.label26 = new global::System.Windows.Forms.Label();
			this.txtNote = new global::System.Windows.Forms.TextBox();
			this.mtxtbControllerNO = new global::System.Windows.Forms.MaskedTextBox();
			this.mtxtbControllerSN = new global::System.Windows.Forms.MaskedTextBox();
			this.optIPLarge = new global::System.Windows.Forms.RadioButton();
			this.optIPSmall = new global::System.Windows.Forms.RadioButton();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.grpbDoorReader = new global::System.Windows.Forms.GroupBox();
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.nudDoorDelay4D = new global::System.Windows.Forms.NumericUpDown();
			this.nudDoorDelay3D = new global::System.Windows.Forms.NumericUpDown();
			this.nudDoorDelay2D = new global::System.Windows.Forms.NumericUpDown();
			this.nudDoorDelay1D = new global::System.Windows.Forms.NumericUpDown();
			this.chkDoorActive4D = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName4D = new global::System.Windows.Forms.TextBox();
			this.label11 = new global::System.Windows.Forms.Label();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.optNC4D = new global::System.Windows.Forms.RadioButton();
			this.optNO4D = new global::System.Windows.Forms.RadioButton();
			this.optOnline4D = new global::System.Windows.Forms.RadioButton();
			this.chkDoorActive3D = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName3D = new global::System.Windows.Forms.TextBox();
			this.label12 = new global::System.Windows.Forms.Label();
			this.groupBox7 = new global::System.Windows.Forms.GroupBox();
			this.optNC3D = new global::System.Windows.Forms.RadioButton();
			this.optNO3D = new global::System.Windows.Forms.RadioButton();
			this.optOnline3D = new global::System.Windows.Forms.RadioButton();
			this.label13 = new global::System.Windows.Forms.Label();
			this.label14 = new global::System.Windows.Forms.Label();
			this.groupBox8 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff4D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn4D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff4D = new global::System.Windows.Forms.RadioButton();
			this.chkAttend4D = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName4D = new global::System.Windows.Forms.TextBox();
			this.label15 = new global::System.Windows.Forms.Label();
			this.groupBox9 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff3D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn3D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff3D = new global::System.Windows.Forms.RadioButton();
			this.chkAttend3D = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName3D = new global::System.Windows.Forms.TextBox();
			this.label16 = new global::System.Windows.Forms.Label();
			this.groupBox10 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff2D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn2D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff2D = new global::System.Windows.Forms.RadioButton();
			this.chkAttend2D = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName2D = new global::System.Windows.Forms.TextBox();
			this.label17 = new global::System.Windows.Forms.Label();
			this.chkDoorActive2D = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName2D = new global::System.Windows.Forms.TextBox();
			this.label18 = new global::System.Windows.Forms.Label();
			this.groupBox11 = new global::System.Windows.Forms.GroupBox();
			this.optNC2D = new global::System.Windows.Forms.RadioButton();
			this.optNO2D = new global::System.Windows.Forms.RadioButton();
			this.optOnline2D = new global::System.Windows.Forms.RadioButton();
			this.groupBox12 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff1D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn1D = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff1D = new global::System.Windows.Forms.RadioButton();
			this.chkAttend1D = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName1D = new global::System.Windows.Forms.TextBox();
			this.label19 = new global::System.Windows.Forms.Label();
			this.label20 = new global::System.Windows.Forms.Label();
			this.label21 = new global::System.Windows.Forms.Label();
			this.chkDoorActive1D = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName1D = new global::System.Windows.Forms.TextBox();
			this.label40 = new global::System.Windows.Forms.Label();
			this.label41 = new global::System.Windows.Forms.Label();
			this.groupBox13 = new global::System.Windows.Forms.GroupBox();
			this.optNC1D = new global::System.Windows.Forms.RadioButton();
			this.optNO1D = new global::System.Windows.Forms.RadioButton();
			this.optOnline1D = new global::System.Windows.Forms.RadioButton();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.groupBox15 = new global::System.Windows.Forms.GroupBox();
			this.nudDoorDelay2B = new global::System.Windows.Forms.NumericUpDown();
			this.nudDoorDelay1B = new global::System.Windows.Forms.NumericUpDown();
			this.label39 = new global::System.Windows.Forms.Label();
			this.label34 = new global::System.Windows.Forms.Label();
			this.groupBox16 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff4B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn4B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff4B = new global::System.Windows.Forms.RadioButton();
			this.chkAttend4B = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName4B = new global::System.Windows.Forms.TextBox();
			this.label22 = new global::System.Windows.Forms.Label();
			this.groupBox17 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff3B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn3B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff3B = new global::System.Windows.Forms.RadioButton();
			this.chkAttend3B = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName3B = new global::System.Windows.Forms.TextBox();
			this.label23 = new global::System.Windows.Forms.Label();
			this.groupBox18 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff2B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn2B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff2B = new global::System.Windows.Forms.RadioButton();
			this.chkAttend2B = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName2B = new global::System.Windows.Forms.TextBox();
			this.label24 = new global::System.Windows.Forms.Label();
			this.chkDoorActive2B = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName2B = new global::System.Windows.Forms.TextBox();
			this.label27 = new global::System.Windows.Forms.Label();
			this.groupBox21 = new global::System.Windows.Forms.GroupBox();
			this.optNC2B = new global::System.Windows.Forms.RadioButton();
			this.optNO2B = new global::System.Windows.Forms.RadioButton();
			this.optOnline2B = new global::System.Windows.Forms.RadioButton();
			this.gpbAttend1B = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff1B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn1B = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff1B = new global::System.Windows.Forms.RadioButton();
			this.chkAttend1B = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName1B = new global::System.Windows.Forms.TextBox();
			this.label28 = new global::System.Windows.Forms.Label();
			this.label29 = new global::System.Windows.Forms.Label();
			this.label30 = new global::System.Windows.Forms.Label();
			this.chkDoorActive1B = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName1B = new global::System.Windows.Forms.TextBox();
			this.label31 = new global::System.Windows.Forms.Label();
			this.label32 = new global::System.Windows.Forms.Label();
			this.groupBox23 = new global::System.Windows.Forms.GroupBox();
			this.optNC1B = new global::System.Windows.Forms.RadioButton();
			this.optNO1B = new global::System.Windows.Forms.RadioButton();
			this.optOnline1B = new global::System.Windows.Forms.RadioButton();
			this.tabPage3 = new global::System.Windows.Forms.TabPage();
			this.groupBox19 = new global::System.Windows.Forms.GroupBox();
			this.nudDoorDelay1A = new global::System.Windows.Forms.NumericUpDown();
			this.label33 = new global::System.Windows.Forms.Label();
			this.label35 = new global::System.Windows.Forms.Label();
			this.groupBox14 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff2A = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn2A = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff2A = new global::System.Windows.Forms.RadioButton();
			this.chkAttend2A = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName2A = new global::System.Windows.Forms.TextBox();
			this.label36 = new global::System.Windows.Forms.Label();
			this.groupBox20 = new global::System.Windows.Forms.GroupBox();
			this.optDutyOff1A = new global::System.Windows.Forms.RadioButton();
			this.optDutyOn1A = new global::System.Windows.Forms.RadioButton();
			this.optDutyOnOff1A = new global::System.Windows.Forms.RadioButton();
			this.chkAttend1A = new global::System.Windows.Forms.CheckBox();
			this.txtReaderName1A = new global::System.Windows.Forms.TextBox();
			this.label37 = new global::System.Windows.Forms.Label();
			this.label38 = new global::System.Windows.Forms.Label();
			this.label42 = new global::System.Windows.Forms.Label();
			this.chkDoorActive1A = new global::System.Windows.Forms.CheckBox();
			this.txtDoorName1A = new global::System.Windows.Forms.TextBox();
			this.label43 = new global::System.Windows.Forms.Label();
			this.label44 = new global::System.Windows.Forms.Label();
			this.groupBox22 = new global::System.Windows.Forms.GroupBox();
			this.optNC1A = new global::System.Windows.Forms.RadioButton();
			this.optNO1A = new global::System.Windows.Forms.RadioButton();
			this.optOnline1A = new global::System.Windows.Forms.RadioButton();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel2 = new global::System.Windows.Forms.Button();
			this.grpbController.SuspendLayout();
			this.grpbIP.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).BeginInit();
			this.grpbDoorReader.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay4D).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay3D).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay2D).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1D).BeginInit();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox15.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay2B).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1B).BeginInit();
			this.groupBox16.SuspendLayout();
			this.groupBox17.SuspendLayout();
			this.groupBox18.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.gpbAttend1B.SuspendLayout();
			this.groupBox23.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox19.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1A).BeginInit();
			this.groupBox14.SuspendLayout();
			this.groupBox20.SuspendLayout();
			this.groupBox22.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnNext, "btnNext");
			this.btnNext.BackColor = global::System.Drawing.Color.Transparent;
			this.btnNext.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnNext.ForeColor = global::System.Drawing.Color.White;
			this.btnNext.Name = "btnNext";
			this.toolTip1.SetToolTip(this.btnNext, componentResourceManager.GetString("btnNext.ToolTip"));
			this.btnNext.UseVisualStyleBackColor = false;
			this.btnNext.Click += new global::System.EventHandler(this.btnNext_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.grpbController, "grpbController");
			this.grpbController.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbController.Controls.Add(this.label8);
			this.grpbController.Controls.Add(this.btnZoneManage);
			this.grpbController.Controls.Add(this.cbof_Zone);
			this.grpbController.Controls.Add(this.label25);
			this.grpbController.Controls.Add(this.grpbIP);
			this.grpbController.Controls.Add(this.chkControllerActive);
			this.grpbController.Controls.Add(this.label26);
			this.grpbController.Controls.Add(this.txtNote);
			this.grpbController.Controls.Add(this.mtxtbControllerNO);
			this.grpbController.Controls.Add(this.mtxtbControllerSN);
			this.grpbController.Controls.Add(this.optIPLarge);
			this.grpbController.Controls.Add(this.optIPSmall);
			this.grpbController.Controls.Add(this.label2);
			this.grpbController.Controls.Add(this.label1);
			this.grpbController.ForeColor = global::System.Drawing.Color.White;
			this.grpbController.Name = "grpbController";
			this.grpbController.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbController, componentResourceManager.GetString("grpbController.ToolTip"));
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, componentResourceManager.GetString("label8.ToolTip"));
			componentResourceManager.ApplyResources(this.btnZoneManage, "btnZoneManage");
			this.btnZoneManage.BackColor = global::System.Drawing.Color.Transparent;
			this.btnZoneManage.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnZoneManage.ForeColor = global::System.Drawing.Color.White;
			this.btnZoneManage.Name = "btnZoneManage";
			this.toolTip1.SetToolTip(this.btnZoneManage, componentResourceManager.GetString("btnZoneManage.ToolTip"));
			this.btnZoneManage.UseVisualStyleBackColor = false;
			this.btnZoneManage.Click += new global::System.EventHandler(this.btnZoneManage_Click);
			componentResourceManager.ApplyResources(this.cbof_Zone, "cbof_Zone");
			this.cbof_Zone.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_Zone.FormattingEnabled = true;
			this.cbof_Zone.Name = "cbof_Zone";
			this.toolTip1.SetToolTip(this.cbof_Zone, componentResourceManager.GetString("cbof_Zone.ToolTip"));
			this.cbof_Zone.DropDown += new global::System.EventHandler(this.cbof_Zone_DropDown);
			this.cbof_Zone.SelectedIndexChanged += new global::System.EventHandler(this.cbof_Zone_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label25, "label25");
			this.label25.Name = "label25";
			this.toolTip1.SetToolTip(this.label25, componentResourceManager.GetString("label25.ToolTip"));
			componentResourceManager.ApplyResources(this.grpbIP, "grpbIP");
			this.grpbIP.Controls.Add(this.nudPort);
			this.grpbIP.Controls.Add(this.txtControllerIP);
			this.grpbIP.Controls.Add(this.label4);
			this.grpbIP.Controls.Add(this.label3);
			this.grpbIP.Name = "grpbIP";
			this.grpbIP.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbIP, componentResourceManager.GetString("grpbIP.ToolTip"));
			componentResourceManager.ApplyResources(this.nudPort, "nudPort");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudPort;
			int[] array = new int[4];
			array[0] = 65534;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudPort;
			int[] array2 = new int[4];
			array2[0] = 1024;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudPort.Name = "nudPort";
			this.toolTip1.SetToolTip(this.nudPort, componentResourceManager.GetString("nudPort.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudPort;
			int[] array3 = new int[4];
			array3[0] = 60000;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.txtControllerIP, "txtControllerIP");
			this.txtControllerIP.Name = "txtControllerIP";
			this.toolTip1.SetToolTip(this.txtControllerIP, componentResourceManager.GetString("txtControllerIP.ToolTip"));
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.chkControllerActive, "chkControllerActive");
			this.chkControllerActive.Checked = true;
			this.chkControllerActive.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkControllerActive.Name = "chkControllerActive";
			this.toolTip1.SetToolTip(this.chkControllerActive, componentResourceManager.GetString("chkControllerActive.ToolTip"));
			this.chkControllerActive.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label26, "label26");
			this.label26.Name = "label26";
			this.toolTip1.SetToolTip(this.label26, componentResourceManager.GetString("label26.ToolTip"));
			componentResourceManager.ApplyResources(this.txtNote, "txtNote");
			this.txtNote.Name = "txtNote";
			this.toolTip1.SetToolTip(this.txtNote, componentResourceManager.GetString("txtNote.ToolTip"));
			componentResourceManager.ApplyResources(this.mtxtbControllerNO, "mtxtbControllerNO");
			this.mtxtbControllerNO.Name = "mtxtbControllerNO";
			this.toolTip1.SetToolTip(this.mtxtbControllerNO, componentResourceManager.GetString("mtxtbControllerNO.ToolTip"));
			this.mtxtbControllerNO.ValidatingType = typeof(int);
			this.mtxtbControllerNO.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.mtxtbControllerNO_KeyPress);
			this.mtxtbControllerNO.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.mtxtbControllerNO_KeyUp);
			componentResourceManager.ApplyResources(this.mtxtbControllerSN, "mtxtbControllerSN");
			this.mtxtbControllerSN.Name = "mtxtbControllerSN";
			this.mtxtbControllerSN.RejectInputOnFirstFailure = true;
			this.mtxtbControllerSN.ResetOnSpace = false;
			this.toolTip1.SetToolTip(this.mtxtbControllerSN, componentResourceManager.GetString("mtxtbControllerSN.ToolTip"));
			this.mtxtbControllerSN.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.mtxtbControllerSN_KeyPress);
			this.mtxtbControllerSN.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.mtxtbControllerSN_KeyUp);
			componentResourceManager.ApplyResources(this.optIPLarge, "optIPLarge");
			this.optIPLarge.Name = "optIPLarge";
			this.toolTip1.SetToolTip(this.optIPLarge, componentResourceManager.GetString("optIPLarge.ToolTip"));
			this.optIPLarge.UseVisualStyleBackColor = true;
			this.optIPLarge.CheckedChanged += new global::System.EventHandler(this.optIPLarge_CheckedChanged);
			componentResourceManager.ApplyResources(this.optIPSmall, "optIPSmall");
			this.optIPSmall.Checked = true;
			this.optIPSmall.Name = "optIPSmall";
			this.optIPSmall.TabStop = true;
			this.toolTip1.SetToolTip(this.optIPSmall, componentResourceManager.GetString("optIPSmall.ToolTip"));
			this.optIPSmall.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.grpbDoorReader, "grpbDoorReader");
			this.grpbDoorReader.BackColor = global::System.Drawing.Color.Transparent;
			this.grpbDoorReader.Controls.Add(this.btnCancel);
			this.grpbDoorReader.Controls.Add(this.tabControl1);
			this.grpbDoorReader.Controls.Add(this.btnOK);
			this.grpbDoorReader.Name = "grpbDoorReader";
			this.grpbDoorReader.TabStop = false;
			this.toolTip1.SetToolTip(this.grpbDoorReader, componentResourceManager.GetString("grpbDoorReader.ToolTip"));
			componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.toolTip1.SetToolTip(this.tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.ForeColor = global::System.Drawing.Color.White;
			this.tabPage1.Name = "tabPage1";
			this.toolTip1.SetToolTip(this.tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.nudDoorDelay4D);
			this.groupBox1.Controls.Add(this.nudDoorDelay3D);
			this.groupBox1.Controls.Add(this.nudDoorDelay2D);
			this.groupBox1.Controls.Add(this.nudDoorDelay1D);
			this.groupBox1.Controls.Add(this.chkDoorActive4D);
			this.groupBox1.Controls.Add(this.txtDoorName4D);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.groupBox6);
			this.groupBox1.Controls.Add(this.chkDoorActive3D);
			this.groupBox1.Controls.Add(this.txtDoorName3D);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.groupBox7);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.groupBox8);
			this.groupBox1.Controls.Add(this.chkAttend4D);
			this.groupBox1.Controls.Add(this.txtReaderName4D);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.groupBox9);
			this.groupBox1.Controls.Add(this.chkAttend3D);
			this.groupBox1.Controls.Add(this.txtReaderName3D);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.groupBox10);
			this.groupBox1.Controls.Add(this.chkAttend2D);
			this.groupBox1.Controls.Add(this.txtReaderName2D);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.chkDoorActive2D);
			this.groupBox1.Controls.Add(this.txtDoorName2D);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.groupBox11);
			this.groupBox1.Controls.Add(this.groupBox12);
			this.groupBox1.Controls.Add(this.chkAttend1D);
			this.groupBox1.Controls.Add(this.txtReaderName1D);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.chkDoorActive1D);
			this.groupBox1.Controls.Add(this.txtDoorName1D);
			this.groupBox1.Controls.Add(this.label40);
			this.groupBox1.Controls.Add(this.label41);
			this.groupBox1.Controls.Add(this.groupBox13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.nudDoorDelay4D, "nudDoorDelay4D");
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudDoorDelay4D;
			int[] array4 = new int[4];
			array4[0] = 6000;
			numericUpDown4.Maximum = new decimal(array4);
			this.nudDoorDelay4D.Name = "nudDoorDelay4D";
			this.toolTip1.SetToolTip(this.nudDoorDelay4D, componentResourceManager.GetString("nudDoorDelay4D.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudDoorDelay4D;
			int[] array5 = new int[4];
			array5[0] = 3;
			numericUpDown5.Value = new decimal(array5);
			componentResourceManager.ApplyResources(this.nudDoorDelay3D, "nudDoorDelay3D");
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudDoorDelay3D;
			int[] array6 = new int[4];
			array6[0] = 6000;
			numericUpDown6.Maximum = new decimal(array6);
			this.nudDoorDelay3D.Name = "nudDoorDelay3D";
			this.toolTip1.SetToolTip(this.nudDoorDelay3D, componentResourceManager.GetString("nudDoorDelay3D.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudDoorDelay3D;
			int[] array7 = new int[4];
			array7[0] = 3;
			numericUpDown7.Value = new decimal(array7);
			componentResourceManager.ApplyResources(this.nudDoorDelay2D, "nudDoorDelay2D");
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.nudDoorDelay2D;
			int[] array8 = new int[4];
			array8[0] = 6000;
			numericUpDown8.Maximum = new decimal(array8);
			this.nudDoorDelay2D.Name = "nudDoorDelay2D";
			this.toolTip1.SetToolTip(this.nudDoorDelay2D, componentResourceManager.GetString("nudDoorDelay2D.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown9 = this.nudDoorDelay2D;
			int[] array9 = new int[4];
			array9[0] = 3;
			numericUpDown9.Value = new decimal(array9);
			componentResourceManager.ApplyResources(this.nudDoorDelay1D, "nudDoorDelay1D");
			global::System.Windows.Forms.NumericUpDown numericUpDown10 = this.nudDoorDelay1D;
			int[] array10 = new int[4];
			array10[0] = 6000;
			numericUpDown10.Maximum = new decimal(array10);
			this.nudDoorDelay1D.Name = "nudDoorDelay1D";
			this.toolTip1.SetToolTip(this.nudDoorDelay1D, componentResourceManager.GetString("nudDoorDelay1D.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown11 = this.nudDoorDelay1D;
			int[] array11 = new int[4];
			array11[0] = 3;
			numericUpDown11.Value = new decimal(array11);
			componentResourceManager.ApplyResources(this.chkDoorActive4D, "chkDoorActive4D");
			this.chkDoorActive4D.Checked = true;
			this.chkDoorActive4D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive4D.Name = "chkDoorActive4D";
			this.toolTip1.SetToolTip(this.chkDoorActive4D, componentResourceManager.GetString("chkDoorActive4D.ToolTip"));
			this.chkDoorActive4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName4D, "txtDoorName4D");
			this.txtDoorName4D.Name = "txtDoorName4D";
			this.toolTip1.SetToolTip(this.txtDoorName4D, componentResourceManager.GetString("txtDoorName4D.ToolTip"));
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			this.toolTip1.SetToolTip(this.label11, componentResourceManager.GetString("label11.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Controls.Add(this.optNC4D);
			this.groupBox6.Controls.Add(this.optNO4D);
			this.groupBox6.Controls.Add(this.optOnline4D);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox6, componentResourceManager.GetString("groupBox6.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC4D, "optNC4D");
			this.optNC4D.Name = "optNC4D";
			this.toolTip1.SetToolTip(this.optNC4D, componentResourceManager.GetString("optNC4D.ToolTip"));
			this.optNC4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO4D, "optNO4D");
			this.optNO4D.Name = "optNO4D";
			this.toolTip1.SetToolTip(this.optNO4D, componentResourceManager.GetString("optNO4D.ToolTip"));
			this.optNO4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline4D, "optOnline4D");
			this.optOnline4D.Checked = true;
			this.optOnline4D.Name = "optOnline4D";
			this.optOnline4D.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline4D, componentResourceManager.GetString("optOnline4D.ToolTip"));
			this.optOnline4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkDoorActive3D, "chkDoorActive3D");
			this.chkDoorActive3D.Checked = true;
			this.chkDoorActive3D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive3D.Name = "chkDoorActive3D";
			this.toolTip1.SetToolTip(this.chkDoorActive3D, componentResourceManager.GetString("chkDoorActive3D.ToolTip"));
			this.chkDoorActive3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName3D, "txtDoorName3D");
			this.txtDoorName3D.Name = "txtDoorName3D";
			this.toolTip1.SetToolTip(this.txtDoorName3D, componentResourceManager.GetString("txtDoorName3D.ToolTip"));
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			this.toolTip1.SetToolTip(this.label12, componentResourceManager.GetString("label12.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox7, "groupBox7");
			this.groupBox7.Controls.Add(this.optNC3D);
			this.groupBox7.Controls.Add(this.optNO3D);
			this.groupBox7.Controls.Add(this.optOnline3D);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox7, componentResourceManager.GetString("groupBox7.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC3D, "optNC3D");
			this.optNC3D.Name = "optNC3D";
			this.toolTip1.SetToolTip(this.optNC3D, componentResourceManager.GetString("optNC3D.ToolTip"));
			this.optNC3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO3D, "optNO3D");
			this.optNO3D.Name = "optNO3D";
			this.toolTip1.SetToolTip(this.optNO3D, componentResourceManager.GetString("optNO3D.ToolTip"));
			this.optNO3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline3D, "optOnline3D");
			this.optOnline3D.Checked = true;
			this.optOnline3D.Name = "optOnline3D";
			this.optOnline3D.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline3D, componentResourceManager.GetString("optOnline3D.ToolTip"));
			this.optOnline3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			this.toolTip1.SetToolTip(this.label13, componentResourceManager.GetString("label13.ToolTip"));
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			this.toolTip1.SetToolTip(this.label14, componentResourceManager.GetString("label14.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox8, "groupBox8");
			this.groupBox8.Controls.Add(this.optDutyOff4D);
			this.groupBox8.Controls.Add(this.optDutyOn4D);
			this.groupBox8.Controls.Add(this.optDutyOnOff4D);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox8, componentResourceManager.GetString("groupBox8.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff4D, "optDutyOff4D");
			this.optDutyOff4D.Name = "optDutyOff4D";
			this.toolTip1.SetToolTip(this.optDutyOff4D, componentResourceManager.GetString("optDutyOff4D.ToolTip"));
			this.optDutyOff4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn4D, "optDutyOn4D");
			this.optDutyOn4D.Name = "optDutyOn4D";
			this.toolTip1.SetToolTip(this.optDutyOn4D, componentResourceManager.GetString("optDutyOn4D.ToolTip"));
			this.optDutyOn4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff4D, "optDutyOnOff4D");
			this.optDutyOnOff4D.Checked = true;
			this.optDutyOnOff4D.Name = "optDutyOnOff4D";
			this.optDutyOnOff4D.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff4D, componentResourceManager.GetString("optDutyOnOff4D.ToolTip"));
			this.optDutyOnOff4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend4D, "chkAttend4D");
			this.chkAttend4D.Checked = true;
			this.chkAttend4D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend4D.Name = "chkAttend4D";
			this.toolTip1.SetToolTip(this.chkAttend4D, componentResourceManager.GetString("chkAttend4D.ToolTip"));
			this.chkAttend4D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName4D, "txtReaderName4D");
			this.txtReaderName4D.Name = "txtReaderName4D";
			this.toolTip1.SetToolTip(this.txtReaderName4D, componentResourceManager.GetString("txtReaderName4D.ToolTip"));
			componentResourceManager.ApplyResources(this.label15, "label15");
			this.label15.Name = "label15";
			this.toolTip1.SetToolTip(this.label15, componentResourceManager.GetString("label15.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox9, "groupBox9");
			this.groupBox9.Controls.Add(this.optDutyOff3D);
			this.groupBox9.Controls.Add(this.optDutyOn3D);
			this.groupBox9.Controls.Add(this.optDutyOnOff3D);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox9, componentResourceManager.GetString("groupBox9.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff3D, "optDutyOff3D");
			this.optDutyOff3D.Name = "optDutyOff3D";
			this.toolTip1.SetToolTip(this.optDutyOff3D, componentResourceManager.GetString("optDutyOff3D.ToolTip"));
			this.optDutyOff3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn3D, "optDutyOn3D");
			this.optDutyOn3D.Name = "optDutyOn3D";
			this.toolTip1.SetToolTip(this.optDutyOn3D, componentResourceManager.GetString("optDutyOn3D.ToolTip"));
			this.optDutyOn3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff3D, "optDutyOnOff3D");
			this.optDutyOnOff3D.Checked = true;
			this.optDutyOnOff3D.Name = "optDutyOnOff3D";
			this.optDutyOnOff3D.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff3D, componentResourceManager.GetString("optDutyOnOff3D.ToolTip"));
			this.optDutyOnOff3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend3D, "chkAttend3D");
			this.chkAttend3D.Checked = true;
			this.chkAttend3D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend3D.Name = "chkAttend3D";
			this.toolTip1.SetToolTip(this.chkAttend3D, componentResourceManager.GetString("chkAttend3D.ToolTip"));
			this.chkAttend3D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName3D, "txtReaderName3D");
			this.txtReaderName3D.Name = "txtReaderName3D";
			this.toolTip1.SetToolTip(this.txtReaderName3D, componentResourceManager.GetString("txtReaderName3D.ToolTip"));
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.Name = "label16";
			this.toolTip1.SetToolTip(this.label16, componentResourceManager.GetString("label16.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox10, "groupBox10");
			this.groupBox10.Controls.Add(this.optDutyOff2D);
			this.groupBox10.Controls.Add(this.optDutyOn2D);
			this.groupBox10.Controls.Add(this.optDutyOnOff2D);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox10, componentResourceManager.GetString("groupBox10.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff2D, "optDutyOff2D");
			this.optDutyOff2D.Name = "optDutyOff2D";
			this.toolTip1.SetToolTip(this.optDutyOff2D, componentResourceManager.GetString("optDutyOff2D.ToolTip"));
			this.optDutyOff2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn2D, "optDutyOn2D");
			this.optDutyOn2D.Name = "optDutyOn2D";
			this.toolTip1.SetToolTip(this.optDutyOn2D, componentResourceManager.GetString("optDutyOn2D.ToolTip"));
			this.optDutyOn2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff2D, "optDutyOnOff2D");
			this.optDutyOnOff2D.Checked = true;
			this.optDutyOnOff2D.Name = "optDutyOnOff2D";
			this.optDutyOnOff2D.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff2D, componentResourceManager.GetString("optDutyOnOff2D.ToolTip"));
			this.optDutyOnOff2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend2D, "chkAttend2D");
			this.chkAttend2D.Checked = true;
			this.chkAttend2D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend2D.Name = "chkAttend2D";
			this.toolTip1.SetToolTip(this.chkAttend2D, componentResourceManager.GetString("chkAttend2D.ToolTip"));
			this.chkAttend2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName2D, "txtReaderName2D");
			this.txtReaderName2D.Name = "txtReaderName2D";
			this.toolTip1.SetToolTip(this.txtReaderName2D, componentResourceManager.GetString("txtReaderName2D.ToolTip"));
			componentResourceManager.ApplyResources(this.label17, "label17");
			this.label17.Name = "label17";
			this.toolTip1.SetToolTip(this.label17, componentResourceManager.GetString("label17.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorActive2D, "chkDoorActive2D");
			this.chkDoorActive2D.Checked = true;
			this.chkDoorActive2D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive2D.Name = "chkDoorActive2D";
			this.toolTip1.SetToolTip(this.chkDoorActive2D, componentResourceManager.GetString("chkDoorActive2D.ToolTip"));
			this.chkDoorActive2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName2D, "txtDoorName2D");
			this.txtDoorName2D.Name = "txtDoorName2D";
			this.toolTip1.SetToolTip(this.txtDoorName2D, componentResourceManager.GetString("txtDoorName2D.ToolTip"));
			componentResourceManager.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			this.toolTip1.SetToolTip(this.label18, componentResourceManager.GetString("label18.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox11, "groupBox11");
			this.groupBox11.Controls.Add(this.optNC2D);
			this.groupBox11.Controls.Add(this.optNO2D);
			this.groupBox11.Controls.Add(this.optOnline2D);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox11, componentResourceManager.GetString("groupBox11.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC2D, "optNC2D");
			this.optNC2D.Name = "optNC2D";
			this.toolTip1.SetToolTip(this.optNC2D, componentResourceManager.GetString("optNC2D.ToolTip"));
			this.optNC2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO2D, "optNO2D");
			this.optNO2D.Name = "optNO2D";
			this.toolTip1.SetToolTip(this.optNO2D, componentResourceManager.GetString("optNO2D.ToolTip"));
			this.optNO2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline2D, "optOnline2D");
			this.optOnline2D.Checked = true;
			this.optOnline2D.Name = "optOnline2D";
			this.optOnline2D.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline2D, componentResourceManager.GetString("optOnline2D.ToolTip"));
			this.optOnline2D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox12, "groupBox12");
			this.groupBox12.Controls.Add(this.optDutyOff1D);
			this.groupBox12.Controls.Add(this.optDutyOn1D);
			this.groupBox12.Controls.Add(this.optDutyOnOff1D);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox12, componentResourceManager.GetString("groupBox12.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff1D, "optDutyOff1D");
			this.optDutyOff1D.Name = "optDutyOff1D";
			this.toolTip1.SetToolTip(this.optDutyOff1D, componentResourceManager.GetString("optDutyOff1D.ToolTip"));
			this.optDutyOff1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn1D, "optDutyOn1D");
			this.optDutyOn1D.Name = "optDutyOn1D";
			this.toolTip1.SetToolTip(this.optDutyOn1D, componentResourceManager.GetString("optDutyOn1D.ToolTip"));
			this.optDutyOn1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff1D, "optDutyOnOff1D");
			this.optDutyOnOff1D.Checked = true;
			this.optDutyOnOff1D.Name = "optDutyOnOff1D";
			this.optDutyOnOff1D.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff1D, componentResourceManager.GetString("optDutyOnOff1D.ToolTip"));
			this.optDutyOnOff1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend1D, "chkAttend1D");
			this.chkAttend1D.Checked = true;
			this.chkAttend1D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend1D.Name = "chkAttend1D";
			this.toolTip1.SetToolTip(this.chkAttend1D, componentResourceManager.GetString("chkAttend1D.ToolTip"));
			this.chkAttend1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName1D, "txtReaderName1D");
			this.txtReaderName1D.Name = "txtReaderName1D";
			this.toolTip1.SetToolTip(this.txtReaderName1D, componentResourceManager.GetString("txtReaderName1D.ToolTip"));
			componentResourceManager.ApplyResources(this.label19, "label19");
			this.label19.Name = "label19";
			this.toolTip1.SetToolTip(this.label19, componentResourceManager.GetString("label19.ToolTip"));
			componentResourceManager.ApplyResources(this.label20, "label20");
			this.label20.Name = "label20";
			this.toolTip1.SetToolTip(this.label20, componentResourceManager.GetString("label20.ToolTip"));
			componentResourceManager.ApplyResources(this.label21, "label21");
			this.label21.Name = "label21";
			this.toolTip1.SetToolTip(this.label21, componentResourceManager.GetString("label21.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorActive1D, "chkDoorActive1D");
			this.chkDoorActive1D.Checked = true;
			this.chkDoorActive1D.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive1D.Name = "chkDoorActive1D";
			this.toolTip1.SetToolTip(this.chkDoorActive1D, componentResourceManager.GetString("chkDoorActive1D.ToolTip"));
			this.chkDoorActive1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName1D, "txtDoorName1D");
			this.txtDoorName1D.Name = "txtDoorName1D";
			this.toolTip1.SetToolTip(this.txtDoorName1D, componentResourceManager.GetString("txtDoorName1D.ToolTip"));
			componentResourceManager.ApplyResources(this.label40, "label40");
			this.label40.Name = "label40";
			this.toolTip1.SetToolTip(this.label40, componentResourceManager.GetString("label40.ToolTip"));
			componentResourceManager.ApplyResources(this.label41, "label41");
			this.label41.Name = "label41";
			this.toolTip1.SetToolTip(this.label41, componentResourceManager.GetString("label41.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox13, "groupBox13");
			this.groupBox13.Controls.Add(this.optNC1D);
			this.groupBox13.Controls.Add(this.optNO1D);
			this.groupBox13.Controls.Add(this.optOnline1D);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox13, componentResourceManager.GetString("groupBox13.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC1D, "optNC1D");
			this.optNC1D.Name = "optNC1D";
			this.toolTip1.SetToolTip(this.optNC1D, componentResourceManager.GetString("optNC1D.ToolTip"));
			this.optNC1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO1D, "optNO1D");
			this.optNO1D.Name = "optNO1D";
			this.toolTip1.SetToolTip(this.optNO1D, componentResourceManager.GetString("optNO1D.ToolTip"));
			this.optNO1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline1D, "optOnline1D");
			this.optOnline1D.Checked = true;
			this.optOnline1D.Name = "optOnline1D";
			this.optOnline1D.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline1D, componentResourceManager.GetString("optOnline1D.ToolTip"));
			this.optOnline1D.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage2.Controls.Add(this.groupBox15);
			this.tabPage2.ForeColor = global::System.Drawing.Color.White;
			this.tabPage2.Name = "tabPage2";
			this.toolTip1.SetToolTip(this.tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.groupBox15, "groupBox15");
			this.groupBox15.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox15.Controls.Add(this.nudDoorDelay2B);
			this.groupBox15.Controls.Add(this.nudDoorDelay1B);
			this.groupBox15.Controls.Add(this.label39);
			this.groupBox15.Controls.Add(this.label34);
			this.groupBox15.Controls.Add(this.groupBox16);
			this.groupBox15.Controls.Add(this.chkAttend4B);
			this.groupBox15.Controls.Add(this.txtReaderName4B);
			this.groupBox15.Controls.Add(this.label22);
			this.groupBox15.Controls.Add(this.groupBox17);
			this.groupBox15.Controls.Add(this.chkAttend3B);
			this.groupBox15.Controls.Add(this.txtReaderName3B);
			this.groupBox15.Controls.Add(this.label23);
			this.groupBox15.Controls.Add(this.groupBox18);
			this.groupBox15.Controls.Add(this.chkAttend2B);
			this.groupBox15.Controls.Add(this.txtReaderName2B);
			this.groupBox15.Controls.Add(this.label24);
			this.groupBox15.Controls.Add(this.chkDoorActive2B);
			this.groupBox15.Controls.Add(this.txtDoorName2B);
			this.groupBox15.Controls.Add(this.label27);
			this.groupBox15.Controls.Add(this.groupBox21);
			this.groupBox15.Controls.Add(this.gpbAttend1B);
			this.groupBox15.Controls.Add(this.chkAttend1B);
			this.groupBox15.Controls.Add(this.txtReaderName1B);
			this.groupBox15.Controls.Add(this.label28);
			this.groupBox15.Controls.Add(this.label29);
			this.groupBox15.Controls.Add(this.label30);
			this.groupBox15.Controls.Add(this.chkDoorActive1B);
			this.groupBox15.Controls.Add(this.txtDoorName1B);
			this.groupBox15.Controls.Add(this.label31);
			this.groupBox15.Controls.Add(this.label32);
			this.groupBox15.Controls.Add(this.groupBox23);
			this.groupBox15.Name = "groupBox15";
			this.groupBox15.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox15, componentResourceManager.GetString("groupBox15.ToolTip"));
			componentResourceManager.ApplyResources(this.nudDoorDelay2B, "nudDoorDelay2B");
			global::System.Windows.Forms.NumericUpDown numericUpDown12 = this.nudDoorDelay2B;
			int[] array12 = new int[4];
			array12[0] = 6000;
			numericUpDown12.Maximum = new decimal(array12);
			this.nudDoorDelay2B.Name = "nudDoorDelay2B";
			this.toolTip1.SetToolTip(this.nudDoorDelay2B, componentResourceManager.GetString("nudDoorDelay2B.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown13 = this.nudDoorDelay2B;
			int[] array13 = new int[4];
			array13[0] = 3;
			numericUpDown13.Value = new decimal(array13);
			componentResourceManager.ApplyResources(this.nudDoorDelay1B, "nudDoorDelay1B");
			global::System.Windows.Forms.NumericUpDown numericUpDown14 = this.nudDoorDelay1B;
			int[] array14 = new int[4];
			array14[0] = 6000;
			numericUpDown14.Maximum = new decimal(array14);
			this.nudDoorDelay1B.Name = "nudDoorDelay1B";
			this.toolTip1.SetToolTip(this.nudDoorDelay1B, componentResourceManager.GetString("nudDoorDelay1B.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown15 = this.nudDoorDelay1B;
			int[] array15 = new int[4];
			array15[0] = 3;
			numericUpDown15.Value = new decimal(array15);
			componentResourceManager.ApplyResources(this.label39, "label39");
			this.label39.Name = "label39";
			this.toolTip1.SetToolTip(this.label39, componentResourceManager.GetString("label39.ToolTip"));
			componentResourceManager.ApplyResources(this.label34, "label34");
			this.label34.Name = "label34";
			this.toolTip1.SetToolTip(this.label34, componentResourceManager.GetString("label34.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox16, "groupBox16");
			this.groupBox16.Controls.Add(this.optDutyOff4B);
			this.groupBox16.Controls.Add(this.optDutyOn4B);
			this.groupBox16.Controls.Add(this.optDutyOnOff4B);
			this.groupBox16.Name = "groupBox16";
			this.groupBox16.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox16, componentResourceManager.GetString("groupBox16.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff4B, "optDutyOff4B");
			this.optDutyOff4B.Name = "optDutyOff4B";
			this.toolTip1.SetToolTip(this.optDutyOff4B, componentResourceManager.GetString("optDutyOff4B.ToolTip"));
			this.optDutyOff4B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn4B, "optDutyOn4B");
			this.optDutyOn4B.Name = "optDutyOn4B";
			this.toolTip1.SetToolTip(this.optDutyOn4B, componentResourceManager.GetString("optDutyOn4B.ToolTip"));
			this.optDutyOn4B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff4B, "optDutyOnOff4B");
			this.optDutyOnOff4B.Checked = true;
			this.optDutyOnOff4B.Name = "optDutyOnOff4B";
			this.optDutyOnOff4B.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff4B, componentResourceManager.GetString("optDutyOnOff4B.ToolTip"));
			this.optDutyOnOff4B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend4B, "chkAttend4B");
			this.chkAttend4B.Checked = true;
			this.chkAttend4B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend4B.Name = "chkAttend4B";
			this.toolTip1.SetToolTip(this.chkAttend4B, componentResourceManager.GetString("chkAttend4B.ToolTip"));
			this.chkAttend4B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName4B, "txtReaderName4B");
			this.txtReaderName4B.Name = "txtReaderName4B";
			this.toolTip1.SetToolTip(this.txtReaderName4B, componentResourceManager.GetString("txtReaderName4B.ToolTip"));
			componentResourceManager.ApplyResources(this.label22, "label22");
			this.label22.Name = "label22";
			this.toolTip1.SetToolTip(this.label22, componentResourceManager.GetString("label22.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox17, "groupBox17");
			this.groupBox17.Controls.Add(this.optDutyOff3B);
			this.groupBox17.Controls.Add(this.optDutyOn3B);
			this.groupBox17.Controls.Add(this.optDutyOnOff3B);
			this.groupBox17.Name = "groupBox17";
			this.groupBox17.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox17, componentResourceManager.GetString("groupBox17.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff3B, "optDutyOff3B");
			this.optDutyOff3B.Name = "optDutyOff3B";
			this.toolTip1.SetToolTip(this.optDutyOff3B, componentResourceManager.GetString("optDutyOff3B.ToolTip"));
			this.optDutyOff3B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn3B, "optDutyOn3B");
			this.optDutyOn3B.Name = "optDutyOn3B";
			this.toolTip1.SetToolTip(this.optDutyOn3B, componentResourceManager.GetString("optDutyOn3B.ToolTip"));
			this.optDutyOn3B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff3B, "optDutyOnOff3B");
			this.optDutyOnOff3B.Checked = true;
			this.optDutyOnOff3B.Name = "optDutyOnOff3B";
			this.optDutyOnOff3B.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff3B, componentResourceManager.GetString("optDutyOnOff3B.ToolTip"));
			this.optDutyOnOff3B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend3B, "chkAttend3B");
			this.chkAttend3B.Checked = true;
			this.chkAttend3B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend3B.Name = "chkAttend3B";
			this.toolTip1.SetToolTip(this.chkAttend3B, componentResourceManager.GetString("chkAttend3B.ToolTip"));
			this.chkAttend3B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName3B, "txtReaderName3B");
			this.txtReaderName3B.Name = "txtReaderName3B";
			this.toolTip1.SetToolTip(this.txtReaderName3B, componentResourceManager.GetString("txtReaderName3B.ToolTip"));
			componentResourceManager.ApplyResources(this.label23, "label23");
			this.label23.Name = "label23";
			this.toolTip1.SetToolTip(this.label23, componentResourceManager.GetString("label23.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox18, "groupBox18");
			this.groupBox18.Controls.Add(this.optDutyOff2B);
			this.groupBox18.Controls.Add(this.optDutyOn2B);
			this.groupBox18.Controls.Add(this.optDutyOnOff2B);
			this.groupBox18.Name = "groupBox18";
			this.groupBox18.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox18, componentResourceManager.GetString("groupBox18.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff2B, "optDutyOff2B");
			this.optDutyOff2B.Name = "optDutyOff2B";
			this.toolTip1.SetToolTip(this.optDutyOff2B, componentResourceManager.GetString("optDutyOff2B.ToolTip"));
			this.optDutyOff2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn2B, "optDutyOn2B");
			this.optDutyOn2B.Name = "optDutyOn2B";
			this.toolTip1.SetToolTip(this.optDutyOn2B, componentResourceManager.GetString("optDutyOn2B.ToolTip"));
			this.optDutyOn2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff2B, "optDutyOnOff2B");
			this.optDutyOnOff2B.Checked = true;
			this.optDutyOnOff2B.Name = "optDutyOnOff2B";
			this.optDutyOnOff2B.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff2B, componentResourceManager.GetString("optDutyOnOff2B.ToolTip"));
			this.optDutyOnOff2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend2B, "chkAttend2B");
			this.chkAttend2B.Checked = true;
			this.chkAttend2B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend2B.Name = "chkAttend2B";
			this.toolTip1.SetToolTip(this.chkAttend2B, componentResourceManager.GetString("chkAttend2B.ToolTip"));
			this.chkAttend2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName2B, "txtReaderName2B");
			this.txtReaderName2B.Name = "txtReaderName2B";
			this.toolTip1.SetToolTip(this.txtReaderName2B, componentResourceManager.GetString("txtReaderName2B.ToolTip"));
			componentResourceManager.ApplyResources(this.label24, "label24");
			this.label24.Name = "label24";
			this.toolTip1.SetToolTip(this.label24, componentResourceManager.GetString("label24.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorActive2B, "chkDoorActive2B");
			this.chkDoorActive2B.Checked = true;
			this.chkDoorActive2B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive2B.Name = "chkDoorActive2B";
			this.toolTip1.SetToolTip(this.chkDoorActive2B, componentResourceManager.GetString("chkDoorActive2B.ToolTip"));
			this.chkDoorActive2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName2B, "txtDoorName2B");
			this.txtDoorName2B.Name = "txtDoorName2B";
			this.toolTip1.SetToolTip(this.txtDoorName2B, componentResourceManager.GetString("txtDoorName2B.ToolTip"));
			componentResourceManager.ApplyResources(this.label27, "label27");
			this.label27.Name = "label27";
			this.toolTip1.SetToolTip(this.label27, componentResourceManager.GetString("label27.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox21, "groupBox21");
			this.groupBox21.Controls.Add(this.optNC2B);
			this.groupBox21.Controls.Add(this.optNO2B);
			this.groupBox21.Controls.Add(this.optOnline2B);
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox21, componentResourceManager.GetString("groupBox21.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC2B, "optNC2B");
			this.optNC2B.Name = "optNC2B";
			this.toolTip1.SetToolTip(this.optNC2B, componentResourceManager.GetString("optNC2B.ToolTip"));
			this.optNC2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO2B, "optNO2B");
			this.optNO2B.Name = "optNO2B";
			this.toolTip1.SetToolTip(this.optNO2B, componentResourceManager.GetString("optNO2B.ToolTip"));
			this.optNO2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline2B, "optOnline2B");
			this.optOnline2B.Checked = true;
			this.optOnline2B.Name = "optOnline2B";
			this.optOnline2B.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline2B, componentResourceManager.GetString("optOnline2B.ToolTip"));
			this.optOnline2B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.gpbAttend1B, "gpbAttend1B");
			this.gpbAttend1B.Controls.Add(this.optDutyOff1B);
			this.gpbAttend1B.Controls.Add(this.optDutyOn1B);
			this.gpbAttend1B.Controls.Add(this.optDutyOnOff1B);
			this.gpbAttend1B.Name = "gpbAttend1B";
			this.gpbAttend1B.TabStop = false;
			this.toolTip1.SetToolTip(this.gpbAttend1B, componentResourceManager.GetString("gpbAttend1B.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff1B, "optDutyOff1B");
			this.optDutyOff1B.Name = "optDutyOff1B";
			this.toolTip1.SetToolTip(this.optDutyOff1B, componentResourceManager.GetString("optDutyOff1B.ToolTip"));
			this.optDutyOff1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn1B, "optDutyOn1B");
			this.optDutyOn1B.Name = "optDutyOn1B";
			this.toolTip1.SetToolTip(this.optDutyOn1B, componentResourceManager.GetString("optDutyOn1B.ToolTip"));
			this.optDutyOn1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff1B, "optDutyOnOff1B");
			this.optDutyOnOff1B.Checked = true;
			this.optDutyOnOff1B.Name = "optDutyOnOff1B";
			this.optDutyOnOff1B.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff1B, componentResourceManager.GetString("optDutyOnOff1B.ToolTip"));
			this.optDutyOnOff1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend1B, "chkAttend1B");
			this.chkAttend1B.Checked = true;
			this.chkAttend1B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend1B.Name = "chkAttend1B";
			this.toolTip1.SetToolTip(this.chkAttend1B, componentResourceManager.GetString("chkAttend1B.ToolTip"));
			this.chkAttend1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName1B, "txtReaderName1B");
			this.txtReaderName1B.Name = "txtReaderName1B";
			this.toolTip1.SetToolTip(this.txtReaderName1B, componentResourceManager.GetString("txtReaderName1B.ToolTip"));
			componentResourceManager.ApplyResources(this.label28, "label28");
			this.label28.Name = "label28";
			this.toolTip1.SetToolTip(this.label28, componentResourceManager.GetString("label28.ToolTip"));
			componentResourceManager.ApplyResources(this.label29, "label29");
			this.label29.Name = "label29";
			this.toolTip1.SetToolTip(this.label29, componentResourceManager.GetString("label29.ToolTip"));
			componentResourceManager.ApplyResources(this.label30, "label30");
			this.label30.Name = "label30";
			this.toolTip1.SetToolTip(this.label30, componentResourceManager.GetString("label30.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorActive1B, "chkDoorActive1B");
			this.chkDoorActive1B.Checked = true;
			this.chkDoorActive1B.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive1B.Name = "chkDoorActive1B";
			this.toolTip1.SetToolTip(this.chkDoorActive1B, componentResourceManager.GetString("chkDoorActive1B.ToolTip"));
			this.chkDoorActive1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName1B, "txtDoorName1B");
			this.txtDoorName1B.Name = "txtDoorName1B";
			this.toolTip1.SetToolTip(this.txtDoorName1B, componentResourceManager.GetString("txtDoorName1B.ToolTip"));
			componentResourceManager.ApplyResources(this.label31, "label31");
			this.label31.Name = "label31";
			this.toolTip1.SetToolTip(this.label31, componentResourceManager.GetString("label31.ToolTip"));
			componentResourceManager.ApplyResources(this.label32, "label32");
			this.label32.Name = "label32";
			this.toolTip1.SetToolTip(this.label32, componentResourceManager.GetString("label32.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox23, "groupBox23");
			this.groupBox23.Controls.Add(this.optNC1B);
			this.groupBox23.Controls.Add(this.optNO1B);
			this.groupBox23.Controls.Add(this.optOnline1B);
			this.groupBox23.Name = "groupBox23";
			this.groupBox23.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox23, componentResourceManager.GetString("groupBox23.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC1B, "optNC1B");
			this.optNC1B.Name = "optNC1B";
			this.toolTip1.SetToolTip(this.optNC1B, componentResourceManager.GetString("optNC1B.ToolTip"));
			this.optNC1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO1B, "optNO1B");
			this.optNO1B.Name = "optNO1B";
			this.toolTip1.SetToolTip(this.optNO1B, componentResourceManager.GetString("optNO1B.ToolTip"));
			this.optNO1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline1B, "optOnline1B");
			this.optOnline1B.Checked = true;
			this.optOnline1B.Name = "optOnline1B";
			this.optOnline1B.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline1B, componentResourceManager.GetString("optOnline1B.ToolTip"));
			this.optOnline1B.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.tabPage3.Controls.Add(this.groupBox19);
			this.tabPage3.ForeColor = global::System.Drawing.Color.White;
			this.tabPage3.Name = "tabPage3";
			this.toolTip1.SetToolTip(this.tabPage3, componentResourceManager.GetString("tabPage3.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox19, "groupBox19");
			this.groupBox19.Controls.Add(this.nudDoorDelay1A);
			this.groupBox19.Controls.Add(this.label33);
			this.groupBox19.Controls.Add(this.label35);
			this.groupBox19.Controls.Add(this.groupBox14);
			this.groupBox19.Controls.Add(this.chkAttend2A);
			this.groupBox19.Controls.Add(this.txtReaderName2A);
			this.groupBox19.Controls.Add(this.label36);
			this.groupBox19.Controls.Add(this.groupBox20);
			this.groupBox19.Controls.Add(this.chkAttend1A);
			this.groupBox19.Controls.Add(this.txtReaderName1A);
			this.groupBox19.Controls.Add(this.label37);
			this.groupBox19.Controls.Add(this.label38);
			this.groupBox19.Controls.Add(this.label42);
			this.groupBox19.Controls.Add(this.chkDoorActive1A);
			this.groupBox19.Controls.Add(this.txtDoorName1A);
			this.groupBox19.Controls.Add(this.label43);
			this.groupBox19.Controls.Add(this.label44);
			this.groupBox19.Controls.Add(this.groupBox22);
			this.groupBox19.Name = "groupBox19";
			this.groupBox19.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox19, componentResourceManager.GetString("groupBox19.ToolTip"));
			componentResourceManager.ApplyResources(this.nudDoorDelay1A, "nudDoorDelay1A");
			global::System.Windows.Forms.NumericUpDown numericUpDown16 = this.nudDoorDelay1A;
			int[] array16 = new int[4];
			array16[0] = 6000;
			numericUpDown16.Maximum = new decimal(array16);
			this.nudDoorDelay1A.Name = "nudDoorDelay1A";
			this.toolTip1.SetToolTip(this.nudDoorDelay1A, componentResourceManager.GetString("nudDoorDelay1A.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown17 = this.nudDoorDelay1A;
			int[] array17 = new int[4];
			array17[0] = 3;
			numericUpDown17.Value = new decimal(array17);
			componentResourceManager.ApplyResources(this.label33, "label33");
			this.label33.Name = "label33";
			this.toolTip1.SetToolTip(this.label33, componentResourceManager.GetString("label33.ToolTip"));
			componentResourceManager.ApplyResources(this.label35, "label35");
			this.label35.Name = "label35";
			this.toolTip1.SetToolTip(this.label35, componentResourceManager.GetString("label35.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox14, "groupBox14");
			this.groupBox14.Controls.Add(this.optDutyOff2A);
			this.groupBox14.Controls.Add(this.optDutyOn2A);
			this.groupBox14.Controls.Add(this.optDutyOnOff2A);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox14, componentResourceManager.GetString("groupBox14.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff2A, "optDutyOff2A");
			this.optDutyOff2A.Name = "optDutyOff2A";
			this.toolTip1.SetToolTip(this.optDutyOff2A, componentResourceManager.GetString("optDutyOff2A.ToolTip"));
			this.optDutyOff2A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn2A, "optDutyOn2A");
			this.optDutyOn2A.Name = "optDutyOn2A";
			this.toolTip1.SetToolTip(this.optDutyOn2A, componentResourceManager.GetString("optDutyOn2A.ToolTip"));
			this.optDutyOn2A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff2A, "optDutyOnOff2A");
			this.optDutyOnOff2A.Checked = true;
			this.optDutyOnOff2A.Name = "optDutyOnOff2A";
			this.optDutyOnOff2A.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff2A, componentResourceManager.GetString("optDutyOnOff2A.ToolTip"));
			this.optDutyOnOff2A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend2A, "chkAttend2A");
			this.chkAttend2A.Checked = true;
			this.chkAttend2A.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend2A.Name = "chkAttend2A";
			this.toolTip1.SetToolTip(this.chkAttend2A, componentResourceManager.GetString("chkAttend2A.ToolTip"));
			this.chkAttend2A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName2A, "txtReaderName2A");
			this.txtReaderName2A.Name = "txtReaderName2A";
			this.toolTip1.SetToolTip(this.txtReaderName2A, componentResourceManager.GetString("txtReaderName2A.ToolTip"));
			componentResourceManager.ApplyResources(this.label36, "label36");
			this.label36.Name = "label36";
			this.toolTip1.SetToolTip(this.label36, componentResourceManager.GetString("label36.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox20, "groupBox20");
			this.groupBox20.Controls.Add(this.optDutyOff1A);
			this.groupBox20.Controls.Add(this.optDutyOn1A);
			this.groupBox20.Controls.Add(this.optDutyOnOff1A);
			this.groupBox20.Name = "groupBox20";
			this.groupBox20.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox20, componentResourceManager.GetString("groupBox20.ToolTip"));
			componentResourceManager.ApplyResources(this.optDutyOff1A, "optDutyOff1A");
			this.optDutyOff1A.Name = "optDutyOff1A";
			this.toolTip1.SetToolTip(this.optDutyOff1A, componentResourceManager.GetString("optDutyOff1A.ToolTip"));
			this.optDutyOff1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOn1A, "optDutyOn1A");
			this.optDutyOn1A.Name = "optDutyOn1A";
			this.toolTip1.SetToolTip(this.optDutyOn1A, componentResourceManager.GetString("optDutyOn1A.ToolTip"));
			this.optDutyOn1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optDutyOnOff1A, "optDutyOnOff1A");
			this.optDutyOnOff1A.Checked = true;
			this.optDutyOnOff1A.Name = "optDutyOnOff1A";
			this.optDutyOnOff1A.TabStop = true;
			this.toolTip1.SetToolTip(this.optDutyOnOff1A, componentResourceManager.GetString("optDutyOnOff1A.ToolTip"));
			this.optDutyOnOff1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkAttend1A, "chkAttend1A");
			this.chkAttend1A.Checked = true;
			this.chkAttend1A.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkAttend1A.Name = "chkAttend1A";
			this.toolTip1.SetToolTip(this.chkAttend1A, componentResourceManager.GetString("chkAttend1A.ToolTip"));
			this.chkAttend1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtReaderName1A, "txtReaderName1A");
			this.txtReaderName1A.Name = "txtReaderName1A";
			this.toolTip1.SetToolTip(this.txtReaderName1A, componentResourceManager.GetString("txtReaderName1A.ToolTip"));
			componentResourceManager.ApplyResources(this.label37, "label37");
			this.label37.Name = "label37";
			this.toolTip1.SetToolTip(this.label37, componentResourceManager.GetString("label37.ToolTip"));
			componentResourceManager.ApplyResources(this.label38, "label38");
			this.label38.Name = "label38";
			this.toolTip1.SetToolTip(this.label38, componentResourceManager.GetString("label38.ToolTip"));
			componentResourceManager.ApplyResources(this.label42, "label42");
			this.label42.Name = "label42";
			this.toolTip1.SetToolTip(this.label42, componentResourceManager.GetString("label42.ToolTip"));
			componentResourceManager.ApplyResources(this.chkDoorActive1A, "chkDoorActive1A");
			this.chkDoorActive1A.Checked = true;
			this.chkDoorActive1A.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkDoorActive1A.Name = "chkDoorActive1A";
			this.toolTip1.SetToolTip(this.chkDoorActive1A, componentResourceManager.GetString("chkDoorActive1A.ToolTip"));
			this.chkDoorActive1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtDoorName1A, "txtDoorName1A");
			this.txtDoorName1A.Name = "txtDoorName1A";
			this.toolTip1.SetToolTip(this.txtDoorName1A, componentResourceManager.GetString("txtDoorName1A.ToolTip"));
			componentResourceManager.ApplyResources(this.label43, "label43");
			this.label43.Name = "label43";
			this.toolTip1.SetToolTip(this.label43, componentResourceManager.GetString("label43.ToolTip"));
			componentResourceManager.ApplyResources(this.label44, "label44");
			this.label44.Name = "label44";
			this.toolTip1.SetToolTip(this.label44, componentResourceManager.GetString("label44.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox22, "groupBox22");
			this.groupBox22.Controls.Add(this.optNC1A);
			this.groupBox22.Controls.Add(this.optNO1A);
			this.groupBox22.Controls.Add(this.optOnline1A);
			this.groupBox22.Name = "groupBox22";
			this.groupBox22.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox22, componentResourceManager.GetString("groupBox22.ToolTip"));
			componentResourceManager.ApplyResources(this.optNC1A, "optNC1A");
			this.optNC1A.Name = "optNC1A";
			this.toolTip1.SetToolTip(this.optNC1A, componentResourceManager.GetString("optNC1A.ToolTip"));
			this.optNC1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optNO1A, "optNO1A");
			this.optNO1A.Name = "optNO1A";
			this.toolTip1.SetToolTip(this.optNO1A, componentResourceManager.GetString("optNO1A.ToolTip"));
			this.optNO1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.optOnline1A, "optOnline1A");
			this.optOnline1A.Checked = true;
			this.optOnline1A.Name = "optOnline1A";
			this.optOnline1A.TabStop = true;
			this.toolTip1.SetToolTip(this.optOnline1A, componentResourceManager.GetString("optOnline1A.ToolTip"));
			this.optOnline1A.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel2, "btnCancel2");
			this.btnCancel2.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel2.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel2.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel2.Name = "btnCancel2";
			this.toolTip1.SetToolTip(this.btnCancel2, componentResourceManager.GetString("btnCancel2.ToolTip"));
			this.btnCancel2.UseVisualStyleBackColor = false;
			this.btnCancel2.Click += new global::System.EventHandler(this.btnCancel2_Click);
			base.AcceptButton = this.btnNext;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.grpbDoorReader);
			base.Controls.Add(this.btnNext);
			base.Controls.Add(this.btnCancel2);
			base.Controls.Add(this.grpbController);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmController";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Load += new global::System.EventHandler(this.dfrmController_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmController_KeyDown);
			base.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.dfrmController_KeyPress);
			this.grpbController.ResumeLayout(false);
			this.grpbController.PerformLayout();
			this.grpbIP.ResumeLayout(false);
			this.grpbIP.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudPort).EndInit();
			this.grpbDoorReader.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay4D).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay3D).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay2D).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1D).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.groupBox15.ResumeLayout(false);
			this.groupBox15.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay2B).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1B).EndInit();
			this.groupBox16.ResumeLayout(false);
			this.groupBox16.PerformLayout();
			this.groupBox17.ResumeLayout(false);
			this.groupBox17.PerformLayout();
			this.groupBox18.ResumeLayout(false);
			this.groupBox18.PerformLayout();
			this.groupBox21.ResumeLayout(false);
			this.groupBox21.PerformLayout();
			this.gpbAttend1B.ResumeLayout(false);
			this.gpbAttend1B.PerformLayout();
			this.groupBox23.ResumeLayout(false);
			this.groupBox23.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.groupBox19.ResumeLayout(false);
			this.groupBox19.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudDoorDelay1A).EndInit();
			this.groupBox14.ResumeLayout(false);
			this.groupBox14.PerformLayout();
			this.groupBox20.ResumeLayout(false);
			this.groupBox20.PerformLayout();
			this.groupBox22.ResumeLayout(false);
			this.groupBox22.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040000C7 RID: 199
		private global::WG3000_COMM.DataOper.icController m_Controller;

		// Token: 0x040000CF RID: 207
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000D0 RID: 208
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040000D1 RID: 209
		private global::System.Windows.Forms.Button btnCancel2;

		// Token: 0x040000D2 RID: 210
		private global::System.Windows.Forms.Button btnZoneManage;

		// Token: 0x040000D3 RID: 211
		private global::System.Windows.Forms.ComboBox cbof_Zone;

		// Token: 0x040000D4 RID: 212
		private global::System.Windows.Forms.CheckBox chkAttend1A;

		// Token: 0x040000D5 RID: 213
		private global::System.Windows.Forms.CheckBox chkAttend1B;

		// Token: 0x040000D6 RID: 214
		private global::System.Windows.Forms.CheckBox chkAttend1D;

		// Token: 0x040000D7 RID: 215
		private global::System.Windows.Forms.CheckBox chkAttend2A;

		// Token: 0x040000D8 RID: 216
		private global::System.Windows.Forms.CheckBox chkAttend2B;

		// Token: 0x040000D9 RID: 217
		private global::System.Windows.Forms.CheckBox chkAttend2D;

		// Token: 0x040000DA RID: 218
		private global::System.Windows.Forms.CheckBox chkAttend3B;

		// Token: 0x040000DB RID: 219
		private global::System.Windows.Forms.CheckBox chkAttend3D;

		// Token: 0x040000DC RID: 220
		private global::System.Windows.Forms.CheckBox chkAttend4B;

		// Token: 0x040000DD RID: 221
		private global::System.Windows.Forms.CheckBox chkAttend4D;

		// Token: 0x040000DE RID: 222
		private global::System.Windows.Forms.CheckBox chkControllerActive;

		// Token: 0x040000DF RID: 223
		private global::System.Windows.Forms.CheckBox chkDoorActive1A;

		// Token: 0x040000E0 RID: 224
		private global::System.Windows.Forms.CheckBox chkDoorActive1B;

		// Token: 0x040000E1 RID: 225
		private global::System.Windows.Forms.CheckBox chkDoorActive1D;

		// Token: 0x040000E2 RID: 226
		private global::System.Windows.Forms.CheckBox chkDoorActive2B;

		// Token: 0x040000E3 RID: 227
		private global::System.Windows.Forms.CheckBox chkDoorActive2D;

		// Token: 0x040000E4 RID: 228
		private global::System.Windows.Forms.CheckBox chkDoorActive3D;

		// Token: 0x040000E5 RID: 229
		private global::System.Windows.Forms.CheckBox chkDoorActive4D;

		// Token: 0x040000E6 RID: 230
		private global::System.Windows.Forms.GroupBox gpbAttend1B;

		// Token: 0x040000E7 RID: 231
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000E8 RID: 232
		private global::System.Windows.Forms.GroupBox groupBox10;

		// Token: 0x040000E9 RID: 233
		private global::System.Windows.Forms.GroupBox groupBox11;

		// Token: 0x040000EA RID: 234
		private global::System.Windows.Forms.GroupBox groupBox12;

		// Token: 0x040000EB RID: 235
		private global::System.Windows.Forms.GroupBox groupBox13;

		// Token: 0x040000EC RID: 236
		private global::System.Windows.Forms.GroupBox groupBox14;

		// Token: 0x040000ED RID: 237
		private global::System.Windows.Forms.GroupBox groupBox15;

		// Token: 0x040000EE RID: 238
		private global::System.Windows.Forms.GroupBox groupBox16;

		// Token: 0x040000EF RID: 239
		private global::System.Windows.Forms.GroupBox groupBox17;

		// Token: 0x040000F0 RID: 240
		private global::System.Windows.Forms.GroupBox groupBox18;

		// Token: 0x040000F1 RID: 241
		private global::System.Windows.Forms.GroupBox groupBox19;

		// Token: 0x040000F2 RID: 242
		private global::System.Windows.Forms.GroupBox groupBox20;

		// Token: 0x040000F3 RID: 243
		private global::System.Windows.Forms.GroupBox groupBox21;

		// Token: 0x040000F4 RID: 244
		private global::System.Windows.Forms.GroupBox groupBox22;

		// Token: 0x040000F5 RID: 245
		private global::System.Windows.Forms.GroupBox groupBox23;

		// Token: 0x040000F6 RID: 246
		private global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x040000F7 RID: 247
		private global::System.Windows.Forms.GroupBox groupBox7;

		// Token: 0x040000F8 RID: 248
		private global::System.Windows.Forms.GroupBox groupBox8;

		// Token: 0x040000F9 RID: 249
		private global::System.Windows.Forms.GroupBox groupBox9;

		// Token: 0x040000FA RID: 250
		private global::System.Windows.Forms.GroupBox grpbController;

		// Token: 0x040000FB RID: 251
		private global::System.Windows.Forms.GroupBox grpbDoorReader;

		// Token: 0x040000FC RID: 252
		private global::System.Windows.Forms.GroupBox grpbIP;

		// Token: 0x040000FD RID: 253
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000FE RID: 254
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040000FF RID: 255
		private global::System.Windows.Forms.Label label12;

		// Token: 0x04000100 RID: 256
		private global::System.Windows.Forms.Label label13;

		// Token: 0x04000101 RID: 257
		private global::System.Windows.Forms.Label label14;

		// Token: 0x04000102 RID: 258
		private global::System.Windows.Forms.Label label15;

		// Token: 0x04000103 RID: 259
		private global::System.Windows.Forms.Label label16;

		// Token: 0x04000104 RID: 260
		private global::System.Windows.Forms.Label label17;

		// Token: 0x04000105 RID: 261
		private global::System.Windows.Forms.Label label18;

		// Token: 0x04000106 RID: 262
		private global::System.Windows.Forms.Label label19;

		// Token: 0x04000107 RID: 263
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000108 RID: 264
		private global::System.Windows.Forms.Label label20;

		// Token: 0x04000109 RID: 265
		private global::System.Windows.Forms.Label label21;

		// Token: 0x0400010A RID: 266
		private global::System.Windows.Forms.Label label22;

		// Token: 0x0400010B RID: 267
		private global::System.Windows.Forms.Label label23;

		// Token: 0x0400010C RID: 268
		private global::System.Windows.Forms.Label label24;

		// Token: 0x0400010D RID: 269
		private global::System.Windows.Forms.Label label25;

		// Token: 0x0400010E RID: 270
		private global::System.Windows.Forms.Label label26;

		// Token: 0x0400010F RID: 271
		private global::System.Windows.Forms.Label label27;

		// Token: 0x04000110 RID: 272
		private global::System.Windows.Forms.Label label28;

		// Token: 0x04000111 RID: 273
		private global::System.Windows.Forms.Label label29;

		// Token: 0x04000112 RID: 274
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000113 RID: 275
		private global::System.Windows.Forms.Label label30;

		// Token: 0x04000114 RID: 276
		private global::System.Windows.Forms.Label label31;

		// Token: 0x04000115 RID: 277
		private global::System.Windows.Forms.Label label32;

		// Token: 0x04000116 RID: 278
		private global::System.Windows.Forms.Label label33;

		// Token: 0x04000117 RID: 279
		private global::System.Windows.Forms.Label label34;

		// Token: 0x04000118 RID: 280
		private global::System.Windows.Forms.Label label35;

		// Token: 0x04000119 RID: 281
		private global::System.Windows.Forms.Label label36;

		// Token: 0x0400011A RID: 282
		private global::System.Windows.Forms.Label label37;

		// Token: 0x0400011B RID: 283
		private global::System.Windows.Forms.Label label38;

		// Token: 0x0400011C RID: 284
		private global::System.Windows.Forms.Label label39;

		// Token: 0x0400011D RID: 285
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400011E RID: 286
		private global::System.Windows.Forms.Label label40;

		// Token: 0x0400011F RID: 287
		private global::System.Windows.Forms.Label label41;

		// Token: 0x04000120 RID: 288
		private global::System.Windows.Forms.Label label42;

		// Token: 0x04000121 RID: 289
		private global::System.Windows.Forms.Label label43;

		// Token: 0x04000122 RID: 290
		private global::System.Windows.Forms.Label label44;

		// Token: 0x04000123 RID: 291
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000124 RID: 292
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay1A;

		// Token: 0x04000125 RID: 293
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay1B;

		// Token: 0x04000126 RID: 294
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay1D;

		// Token: 0x04000127 RID: 295
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay2B;

		// Token: 0x04000128 RID: 296
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay2D;

		// Token: 0x04000129 RID: 297
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay3D;

		// Token: 0x0400012A RID: 298
		private global::System.Windows.Forms.NumericUpDown nudDoorDelay4D;

		// Token: 0x0400012B RID: 299
		private global::System.Windows.Forms.NumericUpDown nudPort;

		// Token: 0x0400012C RID: 300
		private global::System.Windows.Forms.RadioButton optDutyOff1A;

		// Token: 0x0400012D RID: 301
		private global::System.Windows.Forms.RadioButton optDutyOff1B;

		// Token: 0x0400012E RID: 302
		private global::System.Windows.Forms.RadioButton optDutyOff1D;

		// Token: 0x0400012F RID: 303
		private global::System.Windows.Forms.RadioButton optDutyOff2A;

		// Token: 0x04000130 RID: 304
		private global::System.Windows.Forms.RadioButton optDutyOff2B;

		// Token: 0x04000131 RID: 305
		private global::System.Windows.Forms.RadioButton optDutyOff2D;

		// Token: 0x04000132 RID: 306
		private global::System.Windows.Forms.RadioButton optDutyOff3B;

		// Token: 0x04000133 RID: 307
		private global::System.Windows.Forms.RadioButton optDutyOff3D;

		// Token: 0x04000134 RID: 308
		private global::System.Windows.Forms.RadioButton optDutyOff4B;

		// Token: 0x04000135 RID: 309
		private global::System.Windows.Forms.RadioButton optDutyOff4D;

		// Token: 0x04000136 RID: 310
		private global::System.Windows.Forms.RadioButton optDutyOn1A;

		// Token: 0x04000137 RID: 311
		private global::System.Windows.Forms.RadioButton optDutyOn1B;

		// Token: 0x04000138 RID: 312
		private global::System.Windows.Forms.RadioButton optDutyOn1D;

		// Token: 0x04000139 RID: 313
		private global::System.Windows.Forms.RadioButton optDutyOn2A;

		// Token: 0x0400013A RID: 314
		private global::System.Windows.Forms.RadioButton optDutyOn2B;

		// Token: 0x0400013B RID: 315
		private global::System.Windows.Forms.RadioButton optDutyOn2D;

		// Token: 0x0400013C RID: 316
		private global::System.Windows.Forms.RadioButton optDutyOn3B;

		// Token: 0x0400013D RID: 317
		private global::System.Windows.Forms.RadioButton optDutyOn3D;

		// Token: 0x0400013E RID: 318
		private global::System.Windows.Forms.RadioButton optDutyOn4B;

		// Token: 0x0400013F RID: 319
		private global::System.Windows.Forms.RadioButton optDutyOn4D;

		// Token: 0x04000140 RID: 320
		private global::System.Windows.Forms.RadioButton optDutyOnOff1A;

		// Token: 0x04000141 RID: 321
		private global::System.Windows.Forms.RadioButton optDutyOnOff1B;

		// Token: 0x04000142 RID: 322
		private global::System.Windows.Forms.RadioButton optDutyOnOff1D;

		// Token: 0x04000143 RID: 323
		private global::System.Windows.Forms.RadioButton optDutyOnOff2A;

		// Token: 0x04000144 RID: 324
		private global::System.Windows.Forms.RadioButton optDutyOnOff2B;

		// Token: 0x04000145 RID: 325
		private global::System.Windows.Forms.RadioButton optDutyOnOff2D;

		// Token: 0x04000146 RID: 326
		private global::System.Windows.Forms.RadioButton optDutyOnOff3B;

		// Token: 0x04000147 RID: 327
		private global::System.Windows.Forms.RadioButton optDutyOnOff3D;

		// Token: 0x04000148 RID: 328
		private global::System.Windows.Forms.RadioButton optDutyOnOff4B;

		// Token: 0x04000149 RID: 329
		private global::System.Windows.Forms.RadioButton optDutyOnOff4D;

		// Token: 0x0400014A RID: 330
		private global::System.Windows.Forms.RadioButton optIPSmall;

		// Token: 0x0400014B RID: 331
		private global::System.Windows.Forms.RadioButton optNC1A;

		// Token: 0x0400014C RID: 332
		private global::System.Windows.Forms.RadioButton optNC1B;

		// Token: 0x0400014D RID: 333
		private global::System.Windows.Forms.RadioButton optNC1D;

		// Token: 0x0400014E RID: 334
		private global::System.Windows.Forms.RadioButton optNC2B;

		// Token: 0x0400014F RID: 335
		private global::System.Windows.Forms.RadioButton optNC2D;

		// Token: 0x04000150 RID: 336
		private global::System.Windows.Forms.RadioButton optNC3D;

		// Token: 0x04000151 RID: 337
		private global::System.Windows.Forms.RadioButton optNC4D;

		// Token: 0x04000152 RID: 338
		private global::System.Windows.Forms.RadioButton optNO1A;

		// Token: 0x04000153 RID: 339
		private global::System.Windows.Forms.RadioButton optNO1B;

		// Token: 0x04000154 RID: 340
		private global::System.Windows.Forms.RadioButton optNO1D;

		// Token: 0x04000155 RID: 341
		private global::System.Windows.Forms.RadioButton optNO2B;

		// Token: 0x04000156 RID: 342
		private global::System.Windows.Forms.RadioButton optNO2D;

		// Token: 0x04000157 RID: 343
		private global::System.Windows.Forms.RadioButton optNO3D;

		// Token: 0x04000158 RID: 344
		private global::System.Windows.Forms.RadioButton optNO4D;

		// Token: 0x04000159 RID: 345
		private global::System.Windows.Forms.RadioButton optOnline1A;

		// Token: 0x0400015A RID: 346
		private global::System.Windows.Forms.RadioButton optOnline1B;

		// Token: 0x0400015B RID: 347
		private global::System.Windows.Forms.RadioButton optOnline1D;

		// Token: 0x0400015C RID: 348
		private global::System.Windows.Forms.RadioButton optOnline2B;

		// Token: 0x0400015D RID: 349
		private global::System.Windows.Forms.RadioButton optOnline2D;

		// Token: 0x0400015E RID: 350
		private global::System.Windows.Forms.RadioButton optOnline3D;

		// Token: 0x0400015F RID: 351
		private global::System.Windows.Forms.RadioButton optOnline4D;

		// Token: 0x04000160 RID: 352
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04000161 RID: 353
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000162 RID: 354
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04000163 RID: 355
		private global::System.Windows.Forms.TabPage tabPage3;

		// Token: 0x04000164 RID: 356
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04000165 RID: 357
		private global::System.Windows.Forms.TextBox txtDoorName1A;

		// Token: 0x04000166 RID: 358
		private global::System.Windows.Forms.TextBox txtDoorName1B;

		// Token: 0x04000167 RID: 359
		private global::System.Windows.Forms.TextBox txtDoorName1D;

		// Token: 0x04000168 RID: 360
		private global::System.Windows.Forms.TextBox txtDoorName2B;

		// Token: 0x04000169 RID: 361
		private global::System.Windows.Forms.TextBox txtDoorName2D;

		// Token: 0x0400016A RID: 362
		private global::System.Windows.Forms.TextBox txtDoorName3D;

		// Token: 0x0400016B RID: 363
		private global::System.Windows.Forms.TextBox txtDoorName4D;

		// Token: 0x0400016C RID: 364
		private global::System.Windows.Forms.TextBox txtNote;

		// Token: 0x0400016D RID: 365
		private global::System.Windows.Forms.TextBox txtReaderName1A;

		// Token: 0x0400016E RID: 366
		private global::System.Windows.Forms.TextBox txtReaderName1B;

		// Token: 0x0400016F RID: 367
		private global::System.Windows.Forms.TextBox txtReaderName1D;

		// Token: 0x04000170 RID: 368
		private global::System.Windows.Forms.TextBox txtReaderName2A;

		// Token: 0x04000171 RID: 369
		private global::System.Windows.Forms.TextBox txtReaderName2B;

		// Token: 0x04000172 RID: 370
		private global::System.Windows.Forms.TextBox txtReaderName2D;

		// Token: 0x04000173 RID: 371
		private global::System.Windows.Forms.TextBox txtReaderName3B;

		// Token: 0x04000174 RID: 372
		private global::System.Windows.Forms.TextBox txtReaderName3D;

		// Token: 0x04000175 RID: 373
		private global::System.Windows.Forms.TextBox txtReaderName4B;

		// Token: 0x04000176 RID: 374
		private global::System.Windows.Forms.TextBox txtReaderName4D;

		// Token: 0x04000177 RID: 375
		public global::System.Windows.Forms.Button btnNext;

		// Token: 0x04000178 RID: 376
		public global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000179 RID: 377
		public global::System.Windows.Forms.MaskedTextBox mtxtbControllerNO;

		// Token: 0x0400017A RID: 378
		public global::System.Windows.Forms.MaskedTextBox mtxtbControllerSN;

		// Token: 0x0400017B RID: 379
		public global::System.Windows.Forms.RadioButton optIPLarge;

		// Token: 0x0400017C RID: 380
		public global::System.Windows.Forms.TextBox txtControllerIP;
	}
}
