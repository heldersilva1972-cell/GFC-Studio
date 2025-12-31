namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x02000244 RID: 580
	public partial class dfrmMultiCards : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001205 RID: 4613 RVA: 0x00154D43 File Offset: 0x00153D43
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00154D64 File Offset: 0x00153D64
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmMultiCards));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.grpOptInOut = new global::System.Windows.Forms.GroupBox();
			this.chkReaderIn = new global::System.Windows.Forms.CheckBox();
			this.chkReaderOut = new global::System.Windows.Forms.CheckBox();
			this.grpOption = new global::System.Windows.Forms.GroupBox();
			this.nudGrpStartOfSingle = new global::System.Windows.Forms.NumericUpDown();
			this.chkReadByOrder = new global::System.Windows.Forms.CheckBox();
			this.chkSingleGroup = new global::System.Windows.Forms.CheckBox();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			this.grpNeeded = new global::System.Windows.Forms.GroupBox();
			this.nudGrp3 = new global::System.Windows.Forms.NumericUpDown();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.nudGrp8 = new global::System.Windows.Forms.NumericUpDown();
			this.Label9 = new global::System.Windows.Forms.Label();
			this.nudGrp6 = new global::System.Windows.Forms.NumericUpDown();
			this.Label8 = new global::System.Windows.Forms.Label();
			this.nudGrp5 = new global::System.Windows.Forms.NumericUpDown();
			this.label1 = new global::System.Windows.Forms.Label();
			this.nudGrp4 = new global::System.Windows.Forms.NumericUpDown();
			this.Label6 = new global::System.Windows.Forms.Label();
			this.nudGrp2 = new global::System.Windows.Forms.NumericUpDown();
			this.label2 = new global::System.Windows.Forms.Label();
			this.nudGrp1 = new global::System.Windows.Forms.NumericUpDown();
			this.label11 = new global::System.Windows.Forms.Label();
			this.label12 = new global::System.Windows.Forms.Label();
			this.nudGrp7 = new global::System.Windows.Forms.NumericUpDown();
			this.label13 = new global::System.Windows.Forms.Label();
			this.nudTotal = new global::System.Windows.Forms.NumericUpDown();
			this.label14 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.lblWait = new global::System.Windows.Forms.Label();
			this.nudGroupToAdd = new global::System.Windows.Forms.NumericUpDown();
			this.lblControlTimeSeg = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.dgvSelectedUsers = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MoreCards_GrpID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_SelectedGroup = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvUsers = new global::System.Windows.Forms.DataGridView();
			this.ConsumerID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MoreCards_GrpID_1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UserID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ConsumerName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CardNO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SelectedUsers = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_GroupID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDelAllUsers = new global::System.Windows.Forms.Button();
			this.btnDelOneUser = new global::System.Windows.Forms.Button();
			this.btnAddOneUser = new global::System.Windows.Forms.Button();
			this.btnAddAllUsers = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.backgroundWorker1 = new global::System.ComponentModel.BackgroundWorker();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.grpOptInOut.SuspendLayout();
			this.grpOption.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrpStartOfSingle).BeginInit();
			this.grpNeeded.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp3).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp8).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp6).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp5).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp4).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp2).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp7).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudGroupToAdd).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.grpOptInOut, "grpOptInOut");
			this.grpOptInOut.BackColor = global::System.Drawing.Color.Transparent;
			this.grpOptInOut.Controls.Add(this.chkReaderIn);
			this.grpOptInOut.Controls.Add(this.chkReaderOut);
			this.grpOptInOut.ForeColor = global::System.Drawing.Color.White;
			this.grpOptInOut.Name = "grpOptInOut";
			this.grpOptInOut.TabStop = false;
			this.toolTip1.SetToolTip(this.grpOptInOut, componentResourceManager.GetString("grpOptInOut.ToolTip"));
			componentResourceManager.ApplyResources(this.chkReaderIn, "chkReaderIn");
			this.chkReaderIn.Checked = true;
			this.chkReaderIn.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkReaderIn.Name = "chkReaderIn";
			this.toolTip1.SetToolTip(this.chkReaderIn, componentResourceManager.GetString("chkReaderIn.ToolTip"));
			componentResourceManager.ApplyResources(this.chkReaderOut, "chkReaderOut");
			this.chkReaderOut.Name = "chkReaderOut";
			this.toolTip1.SetToolTip(this.chkReaderOut, componentResourceManager.GetString("chkReaderOut.ToolTip"));
			componentResourceManager.ApplyResources(this.grpOption, "grpOption");
			this.grpOption.BackColor = global::System.Drawing.Color.Transparent;
			this.grpOption.Controls.Add(this.nudGrpStartOfSingle);
			this.grpOption.Controls.Add(this.chkReadByOrder);
			this.grpOption.Controls.Add(this.chkSingleGroup);
			this.grpOption.ForeColor = global::System.Drawing.Color.White;
			this.grpOption.Name = "grpOption";
			this.grpOption.TabStop = false;
			this.toolTip1.SetToolTip(this.grpOption, componentResourceManager.GetString("grpOption.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrpStartOfSingle, "nudGrpStartOfSingle");
			this.nudGrpStartOfSingle.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudGrpStartOfSingle;
			int[] array = new int[4];
			array[0] = 8;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudGrpStartOfSingle;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudGrpStartOfSingle.Name = "nudGrpStartOfSingle";
			this.nudGrpStartOfSingle.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrpStartOfSingle, componentResourceManager.GetString("nudGrpStartOfSingle.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudGrpStartOfSingle;
			int[] array3 = new int[4];
			array3[0] = 8;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkReadByOrder, "chkReadByOrder");
			this.chkReadByOrder.Name = "chkReadByOrder";
			this.toolTip1.SetToolTip(this.chkReadByOrder, componentResourceManager.GetString("chkReadByOrder.ToolTip"));
			componentResourceManager.ApplyResources(this.chkSingleGroup, "chkSingleGroup");
			this.chkSingleGroup.Name = "chkSingleGroup";
			this.toolTip1.SetToolTip(this.chkSingleGroup, componentResourceManager.GetString("chkSingleGroup.ToolTip"));
			this.chkSingleGroup.CheckedChanged += new global::System.EventHandler(this.chkSingleGroup_CheckedChanged);
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.BackColor = global::System.Drawing.Color.Transparent;
			this.chkActive.ForeColor = global::System.Drawing.Color.White;
			this.chkActive.Name = "chkActive";
			this.toolTip1.SetToolTip(this.chkActive, componentResourceManager.GetString("chkActive.ToolTip"));
			this.chkActive.UseVisualStyleBackColor = false;
			this.chkActive.CheckedChanged += new global::System.EventHandler(this.chkActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.grpNeeded, "grpNeeded");
			this.grpNeeded.BackColor = global::System.Drawing.Color.Transparent;
			this.grpNeeded.Controls.Add(this.nudGrp3);
			this.grpNeeded.Controls.Add(this.Label10);
			this.grpNeeded.Controls.Add(this.nudGrp8);
			this.grpNeeded.Controls.Add(this.Label9);
			this.grpNeeded.Controls.Add(this.nudGrp6);
			this.grpNeeded.Controls.Add(this.Label8);
			this.grpNeeded.Controls.Add(this.nudGrp5);
			this.grpNeeded.Controls.Add(this.label1);
			this.grpNeeded.Controls.Add(this.nudGrp4);
			this.grpNeeded.Controls.Add(this.Label6);
			this.grpNeeded.Controls.Add(this.nudGrp2);
			this.grpNeeded.Controls.Add(this.label2);
			this.grpNeeded.Controls.Add(this.nudGrp1);
			this.grpNeeded.Controls.Add(this.label11);
			this.grpNeeded.Controls.Add(this.label12);
			this.grpNeeded.Controls.Add(this.nudGrp7);
			this.grpNeeded.Controls.Add(this.label13);
			this.grpNeeded.Controls.Add(this.nudTotal);
			this.grpNeeded.Controls.Add(this.label14);
			this.grpNeeded.ForeColor = global::System.Drawing.Color.White;
			this.grpNeeded.Name = "grpNeeded";
			this.grpNeeded.TabStop = false;
			this.toolTip1.SetToolTip(this.grpNeeded, componentResourceManager.GetString("grpNeeded.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp3, "nudGrp3");
			this.nudGrp3.BackColor = global::System.Drawing.Color.White;
			this.nudGrp3.Name = "nudGrp3";
			this.nudGrp3.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp3, componentResourceManager.GetString("nudGrp3.ToolTip"));
			componentResourceManager.ApplyResources(this.Label10, "Label10");
			this.Label10.Name = "Label10";
			this.toolTip1.SetToolTip(this.Label10, componentResourceManager.GetString("Label10.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp8, "nudGrp8");
			this.nudGrp8.BackColor = global::System.Drawing.Color.White;
			this.nudGrp8.Name = "nudGrp8";
			this.nudGrp8.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp8, componentResourceManager.GetString("nudGrp8.ToolTip"));
			componentResourceManager.ApplyResources(this.Label9, "Label9");
			this.Label9.Name = "Label9";
			this.toolTip1.SetToolTip(this.Label9, componentResourceManager.GetString("Label9.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp6, "nudGrp6");
			this.nudGrp6.BackColor = global::System.Drawing.Color.White;
			this.nudGrp6.Name = "nudGrp6";
			this.nudGrp6.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp6, componentResourceManager.GetString("nudGrp6.ToolTip"));
			componentResourceManager.ApplyResources(this.Label8, "Label8");
			this.Label8.Name = "Label8";
			this.toolTip1.SetToolTip(this.Label8, componentResourceManager.GetString("Label8.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp5, "nudGrp5");
			this.nudGrp5.BackColor = global::System.Drawing.Color.White;
			this.nudGrp5.Name = "nudGrp5";
			this.nudGrp5.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp5, componentResourceManager.GetString("nudGrp5.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp4, "nudGrp4");
			this.nudGrp4.BackColor = global::System.Drawing.Color.White;
			this.nudGrp4.Name = "nudGrp4";
			this.nudGrp4.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp4, componentResourceManager.GetString("nudGrp4.ToolTip"));
			componentResourceManager.ApplyResources(this.Label6, "Label6");
			this.Label6.Name = "Label6";
			this.toolTip1.SetToolTip(this.Label6, componentResourceManager.GetString("Label6.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp2, "nudGrp2");
			this.nudGrp2.BackColor = global::System.Drawing.Color.White;
			this.nudGrp2.Name = "nudGrp2";
			this.nudGrp2.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp2, componentResourceManager.GetString("nudGrp2.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp1, "nudGrp1");
			this.nudGrp1.BackColor = global::System.Drawing.Color.White;
			this.nudGrp1.Name = "nudGrp1";
			this.nudGrp1.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp1, componentResourceManager.GetString("nudGrp1.ToolTip"));
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			this.toolTip1.SetToolTip(this.label11, componentResourceManager.GetString("label11.ToolTip"));
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			this.toolTip1.SetToolTip(this.label12, componentResourceManager.GetString("label12.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGrp7, "nudGrp7");
			this.nudGrp7.BackColor = global::System.Drawing.Color.White;
			this.nudGrp7.Name = "nudGrp7";
			this.nudGrp7.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGrp7, componentResourceManager.GetString("nudGrp7.ToolTip"));
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			this.toolTip1.SetToolTip(this.label13, componentResourceManager.GetString("label13.ToolTip"));
			componentResourceManager.ApplyResources(this.nudTotal, "nudTotal");
			this.nudTotal.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudTotal;
			int[] array4 = new int[4];
			array4[0] = 2;
			numericUpDown4.Minimum = new decimal(array4);
			this.nudTotal.Name = "nudTotal";
			this.nudTotal.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudTotal, componentResourceManager.GetString("nudTotal.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.nudTotal;
			int[] array5 = new int[4];
			array5[0] = 2;
			numericUpDown5.Value = new decimal(array5);
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			this.toolTip1.SetToolTip(this.label14, componentResourceManager.GetString("label14.ToolTip"));
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.lblWait);
			this.groupBox1.Controls.Add(this.nudGroupToAdd);
			this.groupBox1.Controls.Add(this.lblControlTimeSeg);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.dgvSelectedUsers);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.Controls.Add(this.btnDelAllUsers);
			this.groupBox1.Controls.Add(this.btnDelOneUser);
			this.groupBox1.Controls.Add(this.btnAddOneUser);
			this.groupBox1.Controls.Add(this.btnAddAllUsers);
			this.groupBox1.Controls.Add(this.cbof_GroupID);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.lblWait, "lblWait");
			this.lblWait.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblWait.Name = "lblWait";
			this.toolTip1.SetToolTip(this.lblWait, componentResourceManager.GetString("lblWait.ToolTip"));
			componentResourceManager.ApplyResources(this.nudGroupToAdd, "nudGroupToAdd");
			this.nudGroupToAdd.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.nudGroupToAdd;
			int[] array6 = new int[4];
			array6[0] = 9;
			numericUpDown6.Maximum = new decimal(array6);
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.nudGroupToAdd;
			int[] array7 = new int[4];
			array7[0] = 1;
			numericUpDown7.Minimum = new decimal(array7);
			this.nudGroupToAdd.Name = "nudGroupToAdd";
			this.nudGroupToAdd.ReadOnly = true;
			this.toolTip1.SetToolTip(this.nudGroupToAdd, componentResourceManager.GetString("nudGroupToAdd.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.nudGroupToAdd;
			int[] array8 = new int[4];
			array8[0] = 1;
			numericUpDown8.Value = new decimal(array8);
			componentResourceManager.ApplyResources(this.lblControlTimeSeg, "lblControlTimeSeg");
			this.lblControlTimeSeg.Name = "lblControlTimeSeg";
			this.toolTip1.SetToolTip(this.lblControlTimeSeg, componentResourceManager.GetString("lblControlTimeSeg.ToolTip"));
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.dgvSelectedUsers, "dgvSelectedUsers");
			this.dgvSelectedUsers.AllowUserToAddRows = false;
			this.dgvSelectedUsers.AllowUserToDeleteRows = false;
			this.dgvSelectedUsers.AllowUserToOrderColumns = true;
			this.dgvSelectedUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvSelectedUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSelectedUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSelectedUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.f_MoreCards_GrpID, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewCheckBoxColumn1, this.f_SelectedGroup });
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvSelectedUsers.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSelectedUsers.EnableHeadersVisualStyles = false;
			this.dgvSelectedUsers.Name = "dgvSelectedUsers";
			this.dgvSelectedUsers.ReadOnly = true;
			this.dgvSelectedUsers.RowTemplate.Height = 23;
			this.dgvSelectedUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvSelectedUsers, componentResourceManager.GetString("dgvSelectedUsers.ToolTip"));
			this.dgvSelectedUsers.DoubleClick += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MoreCards_GrpID, "f_MoreCards_GrpID");
			this.f_MoreCards_GrpID.Name = "f_MoreCards_GrpID";
			this.f_MoreCards_GrpID.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedGroup, "f_SelectedGroup");
			this.f_SelectedGroup.Name = "f_SelectedGroup";
			this.f_SelectedGroup.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvUsers, "dgvUsers");
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AllowUserToOrderColumns = true;
			this.dgvUsers.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle4.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvUsers.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUsers.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.ConsumerID, this.f_MoreCards_GrpID_1, this.UserID, this.ConsumerName, this.CardNO, this.f_SelectedUsers, this.f_GroupID });
			dataGridViewCellStyle5.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle5.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvUsers.EnableHeadersVisualStyles = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowTemplate.Height = 23;
			this.dgvUsers.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dgvUsers, componentResourceManager.GetString("dgvUsers.ToolTip"));
			this.dgvUsers.DoubleClick += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.ConsumerID, "ConsumerID");
			this.ConsumerID.Name = "ConsumerID";
			this.ConsumerID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MoreCards_GrpID_1, "f_MoreCards_GrpID_1");
			this.f_MoreCards_GrpID_1.Name = "f_MoreCards_GrpID_1";
			this.f_MoreCards_GrpID_1.ReadOnly = true;
			dataGridViewCellStyle6.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.UserID.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.UserID, "UserID");
			this.UserID.Name = "UserID";
			this.UserID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.ConsumerName, "ConsumerName");
			this.ConsumerName.Name = "ConsumerName";
			this.ConsumerName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.CardNO, "CardNO");
			this.CardNO.Name = "CardNO";
			this.CardNO.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_SelectedUsers, "f_SelectedUsers");
			this.f_SelectedUsers.Name = "f_SelectedUsers";
			this.f_SelectedUsers.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_GroupID, "f_GroupID");
			this.f_GroupID.Name = "f_GroupID";
			this.f_GroupID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnDelAllUsers, "btnDelAllUsers");
			this.btnDelAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelAllUsers.Name = "btnDelAllUsers";
			this.toolTip1.SetToolTip(this.btnDelAllUsers, componentResourceManager.GetString("btnDelAllUsers.ToolTip"));
			this.btnDelAllUsers.UseVisualStyleBackColor = true;
			this.btnDelAllUsers.Click += new global::System.EventHandler(this.btnDelAllUsers_Click);
			componentResourceManager.ApplyResources(this.btnDelOneUser, "btnDelOneUser");
			this.btnDelOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelOneUser.Name = "btnDelOneUser";
			this.toolTip1.SetToolTip(this.btnDelOneUser, componentResourceManager.GetString("btnDelOneUser.ToolTip"));
			this.btnDelOneUser.UseVisualStyleBackColor = true;
			this.btnDelOneUser.Click += new global::System.EventHandler(this.btnDelOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddOneUser, "btnAddOneUser");
			this.btnAddOneUser.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneUser.Name = "btnAddOneUser";
			this.toolTip1.SetToolTip(this.btnAddOneUser, componentResourceManager.GetString("btnAddOneUser.ToolTip"));
			this.btnAddOneUser.UseVisualStyleBackColor = true;
			this.btnAddOneUser.Click += new global::System.EventHandler(this.btnAddOneUser_Click);
			componentResourceManager.ApplyResources(this.btnAddAllUsers, "btnAddAllUsers");
			this.btnAddAllUsers.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllUsers.Name = "btnAddAllUsers";
			this.toolTip1.SetToolTip(this.btnAddAllUsers, componentResourceManager.GetString("btnAddAllUsers.ToolTip"));
			this.btnAddAllUsers.UseVisualStyleBackColor = true;
			this.btnAddAllUsers.Click += new global::System.EventHandler(this.btnAddAllUsers_Click);
			componentResourceManager.ApplyResources(this.cbof_GroupID, "cbof_GroupID");
			this.cbof_GroupID.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbof_GroupID.FormattingEnabled = true;
			this.cbof_GroupID.Name = "cbof_GroupID";
			this.toolTip1.SetToolTip(this.cbof_GroupID, componentResourceManager.GetString("cbof_GroupID.ToolTip"));
			this.cbof_GroupID.DropDown += new global::System.EventHandler(this.cbof_GroupID_DropDown);
			this.cbof_GroupID.SelectedIndexChanged += new global::System.EventHandler(this.cbof_GroupID_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.toolTip1.SetToolTip(this.label4, componentResourceManager.GetString("label4.ToolTip"));
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.grpOptInOut);
			base.Controls.Add(this.grpOption);
			base.Controls.Add(this.chkActive);
			base.Controls.Add(this.grpNeeded);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.groupBox1);
			base.Name = "dfrmMultiCards";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrm_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmMultiCards_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmMultiCards_KeyDown);
			this.grpOptInOut.ResumeLayout(false);
			this.grpOption.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.nudGrpStartOfSingle).EndInit();
			this.grpNeeded.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp3).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp8).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp6).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp5).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp4).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp2).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudGrp7).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudTotal).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudGroupToAdd).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvSelectedUsers).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002065 RID: 8293
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002066 RID: 8294
		private global::System.ComponentModel.BackgroundWorker backgroundWorker1;

		// Token: 0x04002067 RID: 8295
		private global::System.Windows.Forms.Button btnAddAllUsers;

		// Token: 0x04002068 RID: 8296
		private global::System.Windows.Forms.Button btnAddOneUser;

		// Token: 0x04002069 RID: 8297
		private global::System.Windows.Forms.Button btnDelAllUsers;

		// Token: 0x0400206A RID: 8298
		private global::System.Windows.Forms.Button btnDelOneUser;

		// Token: 0x0400206B RID: 8299
		private global::System.Windows.Forms.DataGridViewTextBoxColumn CardNO;

		// Token: 0x0400206C RID: 8300
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x0400206D RID: 8301
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerID;

		// Token: 0x0400206E RID: 8302
		private global::System.Windows.Forms.DataGridViewTextBoxColumn ConsumerName;

		// Token: 0x0400206F RID: 8303
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

		// Token: 0x04002070 RID: 8304
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x04002071 RID: 8305
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x04002072 RID: 8306
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x04002073 RID: 8307
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x04002074 RID: 8308
		private global::System.Windows.Forms.DataGridView dgvSelectedUsers;

		// Token: 0x04002075 RID: 8309
		private global::System.Windows.Forms.DataGridView dgvUsers;

		// Token: 0x04002076 RID: 8310
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_GroupID;

		// Token: 0x04002077 RID: 8311
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MoreCards_GrpID;

		// Token: 0x04002078 RID: 8312
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MoreCards_GrpID_1;

		// Token: 0x04002079 RID: 8313
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SelectedGroup;

		// Token: 0x0400207A RID: 8314
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_SelectedUsers;

		// Token: 0x0400207B RID: 8315
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400207C RID: 8316
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400207D RID: 8317
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400207E RID: 8318
		private global::System.Windows.Forms.Label lblWait;

		// Token: 0x0400207F RID: 8319
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04002080 RID: 8320
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04002081 RID: 8321
		private global::System.Windows.Forms.DataGridViewTextBoxColumn UserID;

		// Token: 0x04002082 RID: 8322
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002083 RID: 8323
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002084 RID: 8324
		internal global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04002085 RID: 8325
		internal global::System.Windows.Forms.CheckBox chkReadByOrder;

		// Token: 0x04002086 RID: 8326
		internal global::System.Windows.Forms.CheckBox chkReaderIn;

		// Token: 0x04002087 RID: 8327
		internal global::System.Windows.Forms.CheckBox chkReaderOut;

		// Token: 0x04002088 RID: 8328
		internal global::System.Windows.Forms.CheckBox chkSingleGroup;

		// Token: 0x04002089 RID: 8329
		internal global::System.Windows.Forms.GroupBox grpNeeded;

		// Token: 0x0400208A RID: 8330
		internal global::System.Windows.Forms.GroupBox grpOptInOut;

		// Token: 0x0400208B RID: 8331
		internal global::System.Windows.Forms.GroupBox grpOption;

		// Token: 0x0400208C RID: 8332
		internal global::System.Windows.Forms.Label label1;

		// Token: 0x0400208D RID: 8333
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x0400208E RID: 8334
		internal global::System.Windows.Forms.Label label11;

		// Token: 0x0400208F RID: 8335
		internal global::System.Windows.Forms.Label label12;

		// Token: 0x04002090 RID: 8336
		internal global::System.Windows.Forms.Label label13;

		// Token: 0x04002091 RID: 8337
		internal global::System.Windows.Forms.Label label14;

		// Token: 0x04002092 RID: 8338
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x04002093 RID: 8339
		internal global::System.Windows.Forms.Label Label6;

		// Token: 0x04002094 RID: 8340
		internal global::System.Windows.Forms.Label Label8;

		// Token: 0x04002095 RID: 8341
		internal global::System.Windows.Forms.Label Label9;

		// Token: 0x04002096 RID: 8342
		internal global::System.Windows.Forms.Label lblControlTimeSeg;

		// Token: 0x04002097 RID: 8343
		internal global::System.Windows.Forms.NumericUpDown nudGroupToAdd;

		// Token: 0x04002098 RID: 8344
		internal global::System.Windows.Forms.NumericUpDown nudGrp1;

		// Token: 0x04002099 RID: 8345
		internal global::System.Windows.Forms.NumericUpDown nudGrp2;

		// Token: 0x0400209A RID: 8346
		internal global::System.Windows.Forms.NumericUpDown nudGrp3;

		// Token: 0x0400209B RID: 8347
		internal global::System.Windows.Forms.NumericUpDown nudGrp4;

		// Token: 0x0400209C RID: 8348
		internal global::System.Windows.Forms.NumericUpDown nudGrp5;

		// Token: 0x0400209D RID: 8349
		internal global::System.Windows.Forms.NumericUpDown nudGrp6;

		// Token: 0x0400209E RID: 8350
		internal global::System.Windows.Forms.NumericUpDown nudGrp7;

		// Token: 0x0400209F RID: 8351
		internal global::System.Windows.Forms.NumericUpDown nudGrp8;

		// Token: 0x040020A0 RID: 8352
		internal global::System.Windows.Forms.NumericUpDown nudGrpStartOfSingle;

		// Token: 0x040020A1 RID: 8353
		internal global::System.Windows.Forms.NumericUpDown nudTotal;
	}
}
