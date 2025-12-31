namespace WG3000_COMM.Basic
{
	// Token: 0x02000037 RID: 55
	public partial class dfrmUserAutoAdd : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0006E480 File Offset: 0x0006D480
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmWait1 != null)
			{
				this.dfrmWait1.Dispose();
			}
			if (disposing && this.control != null)
			{
				this.control.Dispose();
			}
			if (this.bDisposeWatching && disposing && this.watching != null && global::WG3000_COMM.Core.wgTools.bUDPOnly64 <= 0)
			{
				this.watching.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0006E4FC File Offset: 0x0006D4FC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmUserAutoAdd));
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.optManualInput = new global::System.Windows.Forms.RadioButton();
			this.cboDoors = new global::System.Windows.Forms.ComboBox();
			this.optController = new global::System.Windows.Forms.RadioButton();
			this.optUSBReader = new global::System.Windows.Forms.RadioButton();
			this.btnNext = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.cbof_GroupID = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.chkOption = new global::System.Windows.Forms.CheckBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.txtNOStartCaption = new global::System.Windows.Forms.TextBox();
			this.nudNOLength = new global::System.Windows.Forms.NumericUpDown();
			this.chkConst = new global::System.Windows.Forms.CheckBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.btnDirectGetFromtheController = new global::System.Windows.Forms.Button();
			this.lblCount = new global::System.Windows.Forms.Label();
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.txtEndNO = new global::System.Windows.Forms.MaskedTextBox();
			this.txtStartNO = new global::System.Windows.Forms.MaskedTextBox();
			this.lstSwipe = new global::System.Windows.Forms.ListBox();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.button1 = new global::System.Windows.Forms.Button();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudNOLength).BeginInit();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.optManualInput);
			this.groupBox1.Controls.Add(this.cboDoors);
			this.groupBox1.Controls.Add(this.optController);
			this.groupBox1.Controls.Add(this.optUSBReader);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.optManualInput, "optManualInput");
			this.optManualInput.Name = "optManualInput";
			this.toolTip1.SetToolTip(this.optManualInput, componentResourceManager.GetString("optManualInput.ToolTip"));
			this.optManualInput.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cboDoors, "cboDoors");
			this.cboDoors.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoors.FormattingEnabled = true;
			this.cboDoors.Name = "cboDoors";
			this.toolTip1.SetToolTip(this.cboDoors, componentResourceManager.GetString("cboDoors.ToolTip"));
			this.cboDoors.DropDown += new global::System.EventHandler(this.cboDoors_DropDown);
			componentResourceManager.ApplyResources(this.optController, "optController");
			this.optController.Name = "optController";
			this.toolTip1.SetToolTip(this.optController, componentResourceManager.GetString("optController.ToolTip"));
			this.optController.UseVisualStyleBackColor = true;
			this.optController.CheckedChanged += new global::System.EventHandler(this.optController_CheckedChanged);
			componentResourceManager.ApplyResources(this.optUSBReader, "optUSBReader");
			this.optUSBReader.Checked = true;
			this.optUSBReader.Name = "optUSBReader";
			this.optUSBReader.TabStop = true;
			this.toolTip1.SetToolTip(this.optUSBReader, componentResourceManager.GetString("optUSBReader.ToolTip"));
			this.optUSBReader.UseVisualStyleBackColor = true;
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
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
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
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.chkOption);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.groupBox4);
			this.groupBox2.Controls.Add(this.btnDirectGetFromtheController);
			this.groupBox2.Controls.Add(this.lblCount);
			this.groupBox2.Controls.Add(this.lblInfo);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this.lstSwipe);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.cbof_GroupID);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox2, componentResourceManager.GetString("groupBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.chkOption, "chkOption");
			this.chkOption.Name = "chkOption";
			this.toolTip1.SetToolTip(this.chkOption, componentResourceManager.GetString("chkOption.ToolTip"));
			this.chkOption.UseVisualStyleBackColor = true;
			this.chkOption.CheckedChanged += new global::System.EventHandler(this.checkBox2_CheckedChanged);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, componentResourceManager.GetString("label3.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.txtNOStartCaption);
			this.groupBox4.Controls.Add(this.nudNOLength);
			this.groupBox4.Controls.Add(this.chkConst);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox4, componentResourceManager.GetString("groupBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.txtNOStartCaption, "txtNOStartCaption");
			this.txtNOStartCaption.Name = "txtNOStartCaption";
			this.toolTip1.SetToolTip(this.txtNOStartCaption, componentResourceManager.GetString("txtNOStartCaption.ToolTip"));
			componentResourceManager.ApplyResources(this.nudNOLength, "nudNOLength");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudNOLength;
			int[] array = new int[4];
			array[0] = 20;
			numericUpDown.Maximum = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudNOLength;
			int[] array2 = new int[4];
			array2[0] = 1;
			numericUpDown2.Minimum = new decimal(array2);
			this.nudNOLength.Name = "nudNOLength";
			this.toolTip1.SetToolTip(this.nudNOLength, componentResourceManager.GetString("nudNOLength.ToolTip"));
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudNOLength;
			int[] array3 = new int[4];
			array3[0] = 8;
			numericUpDown3.Value = new decimal(array3);
			componentResourceManager.ApplyResources(this.chkConst, "chkConst");
			this.chkConst.Name = "chkConst";
			this.toolTip1.SetToolTip(this.chkConst, componentResourceManager.GetString("chkConst.ToolTip"));
			this.chkConst.UseVisualStyleBackColor = true;
			this.chkConst.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.toolTip1.SetToolTip(this.label6, componentResourceManager.GetString("label6.ToolTip"));
			componentResourceManager.ApplyResources(this.btnDirectGetFromtheController, "btnDirectGetFromtheController");
			this.btnDirectGetFromtheController.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDirectGetFromtheController.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDirectGetFromtheController.ForeColor = global::System.Drawing.Color.White;
			this.btnDirectGetFromtheController.Name = "btnDirectGetFromtheController";
			this.toolTip1.SetToolTip(this.btnDirectGetFromtheController, componentResourceManager.GetString("btnDirectGetFromtheController.ToolTip"));
			this.btnDirectGetFromtheController.UseVisualStyleBackColor = false;
			this.btnDirectGetFromtheController.Click += new global::System.EventHandler(this.btnDirectGetFromtheController_Click);
			componentResourceManager.ApplyResources(this.lblCount, "lblCount");
			this.lblCount.Name = "lblCount";
			this.toolTip1.SetToolTip(this.lblCount, componentResourceManager.GetString("lblCount.ToolTip"));
			componentResourceManager.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.Name = "lblInfo";
			this.toolTip1.SetToolTip(this.lblInfo, componentResourceManager.GetString("lblInfo.ToolTip"));
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.toolTip1.SetToolTip(this.label5, componentResourceManager.GetString("label5.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.txtEndNO);
			this.groupBox3.Controls.Add(this.txtStartNO);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox3, componentResourceManager.GetString("groupBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, componentResourceManager.GetString("label1.ToolTip"));
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, componentResourceManager.GetString("label2.ToolTip"));
			componentResourceManager.ApplyResources(this.txtEndNO, "txtEndNO");
			this.txtEndNO.Name = "txtEndNO";
			this.toolTip1.SetToolTip(this.txtEndNO, componentResourceManager.GetString("txtEndNO.ToolTip"));
			this.txtEndNO.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtEndNO_KeyPress);
			this.txtEndNO.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtEndNO_KeyUp);
			componentResourceManager.ApplyResources(this.txtStartNO, "txtStartNO");
			this.txtStartNO.Name = "txtStartNO";
			this.toolTip1.SetToolTip(this.txtStartNO, componentResourceManager.GetString("txtStartNO.ToolTip"));
			this.txtStartNO.KeyPress += new global::System.Windows.Forms.KeyPressEventHandler(this.txtStartNO_KeyPress);
			this.txtStartNO.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.txtStartNO_KeyUp);
			componentResourceManager.ApplyResources(this.lstSwipe, "lstSwipe");
			this.lstSwipe.FormattingEnabled = true;
			this.lstSwipe.Name = "lstSwipe";
			this.toolTip1.SetToolTip(this.lstSwipe, componentResourceManager.GetString("lstSwipe.ToolTip"));
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.toolTip1.SetToolTip(this.btnExit, componentResourceManager.GetString("btnExit.ToolTip"));
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnCancel2_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.toolTip1.SetToolTip(this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.toolTip1.SetToolTip(this.button1, componentResourceManager.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnNext);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmUserAutoAdd";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmUserAutoAdd_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.dfrmUserAutoAdd_FormClosed);
			base.Load += new global::System.EventHandler(this.dfrmUserAutoAdd_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmUserAutoAdd_KeyDown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudNOLength).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x0400074A RID: 1866
		private bool bDisposeWatching;

		// Token: 0x0400074F RID: 1871
		private global::WG3000_COMM.DataOper.icController control;

		// Token: 0x04000759 RID: 1881
		private global::WG3000_COMM.Basic.dfrmWait dfrmWait1 = new global::WG3000_COMM.Basic.dfrmWait();

		// Token: 0x0400075C RID: 1884
		public global::WG3000_COMM.Core.WatchingService watching;

		// Token: 0x0400075D RID: 1885
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400075E RID: 1886
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x0400075F RID: 1887
		private global::System.Windows.Forms.Button btnDirectGetFromtheController;

		// Token: 0x04000760 RID: 1888
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x04000761 RID: 1889
		private global::System.Windows.Forms.Button btnNext;

		// Token: 0x04000762 RID: 1890
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000763 RID: 1891
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000764 RID: 1892
		private global::System.Windows.Forms.ComboBox cboDoors;

		// Token: 0x04000765 RID: 1893
		private global::System.Windows.Forms.ComboBox cbof_GroupID;

		// Token: 0x04000766 RID: 1894
		private global::System.Windows.Forms.CheckBox chkConst;

		// Token: 0x04000767 RID: 1895
		private global::System.Windows.Forms.CheckBox chkOption;

		// Token: 0x04000768 RID: 1896
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000769 RID: 1897
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x0400076A RID: 1898
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x0400076B RID: 1899
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x0400076C RID: 1900
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400076D RID: 1901
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400076E RID: 1902
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400076F RID: 1903
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000770 RID: 1904
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000771 RID: 1905
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04000772 RID: 1906
		private global::System.Windows.Forms.Label lblCount;

		// Token: 0x04000773 RID: 1907
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x04000774 RID: 1908
		private global::System.Windows.Forms.ListBox lstSwipe;

		// Token: 0x04000775 RID: 1909
		private global::System.Windows.Forms.NumericUpDown nudNOLength;

		// Token: 0x04000776 RID: 1910
		private global::System.Windows.Forms.RadioButton optController;

		// Token: 0x04000777 RID: 1911
		private global::System.Windows.Forms.RadioButton optManualInput;

		// Token: 0x04000778 RID: 1912
		private global::System.Windows.Forms.RadioButton optUSBReader;

		// Token: 0x04000779 RID: 1913
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400077A RID: 1914
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x0400077B RID: 1915
		private global::System.Windows.Forms.MaskedTextBox txtEndNO;

		// Token: 0x0400077C RID: 1916
		private global::System.Windows.Forms.TextBox txtNOStartCaption;

		// Token: 0x0400077D RID: 1917
		private global::System.Windows.Forms.MaskedTextBox txtStartNO;
	}
}
