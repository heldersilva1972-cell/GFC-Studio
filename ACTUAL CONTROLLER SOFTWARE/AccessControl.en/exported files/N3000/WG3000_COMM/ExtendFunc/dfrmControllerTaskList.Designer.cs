namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x0200023E RID: 574
	public partial class dfrmControllerTaskList : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x0014713C File Offset: 0x0014613C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.dfrmFind1 != null)
			{
				this.dfrmFind1.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00147174 File Offset: 0x00146174
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerTaskList));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.btnDel = new global::System.Windows.Forms.Button();
			this.dgvTaskList = new global::System.Windows.Forms.DataGridView();
			this.f_ID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_From = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_To = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OperateHMS1A = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Monday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Tuesday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Wednesday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Thursday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Friday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Saturday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_Sunday = new global::System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.f_AdaptTo = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorControlDesc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorControl = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_DoorID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAdd = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cboAccessMethod = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cboDoors = new global::System.Windows.Forms.ComboBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.checkBox49 = new global::System.Windows.Forms.CheckBox();
			this.checkBox48 = new global::System.Windows.Forms.CheckBox();
			this.checkBox47 = new global::System.Windows.Forms.CheckBox();
			this.checkBox46 = new global::System.Windows.Forms.CheckBox();
			this.checkBox45 = new global::System.Windows.Forms.CheckBox();
			this.checkBox44 = new global::System.Windows.Forms.CheckBox();
			this.checkBox43 = new global::System.Windows.Forms.CheckBox();
			this.label45 = new global::System.Windows.Forms.Label();
			this.dtpTime = new global::System.Windows.Forms.DateTimePicker();
			this.label43 = new global::System.Windows.Forms.Label();
			this.label44 = new global::System.Windows.Forms.Label();
			this.dtpEnd = new global::System.Windows.Forms.DateTimePicker();
			this.dtpBegin = new global::System.Windows.Forms.DateTimePicker();
			this.btnEdit = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dgvTaskList).BeginInit();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.BackColor = global::System.Drawing.Color.Transparent;
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.BackColor = global::System.Drawing.Color.Transparent;
			this.btnClose.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnClose.ForeColor = global::System.Drawing.Color.White;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new global::System.EventHandler(this.btnClose_Click);
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDel.ForeColor = global::System.Drawing.Color.White;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new global::System.EventHandler(this.btnDel_Click);
			componentResourceManager.ApplyResources(this.dgvTaskList, "dgvTaskList");
			this.dgvTaskList.AllowUserToAddRows = false;
			this.dgvTaskList.AllowUserToDeleteRows = false;
			this.dgvTaskList.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvTaskList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvTaskList.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvTaskList.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.f_ID, this.f_From, this.f_To, this.f_OperateHMS1A, this.f_Monday, this.f_Tuesday, this.f_Wednesday, this.f_Thursday, this.f_Friday, this.f_Saturday,
				this.f_Sunday, this.f_AdaptTo, this.f_DoorControlDesc, this.f_Note, this.f_DoorControl, this.f_DoorID
			});
			this.dgvTaskList.EnableHeadersVisualStyles = false;
			this.dgvTaskList.Name = "dgvTaskList";
			this.dgvTaskList.ReadOnly = true;
			this.dgvTaskList.RowTemplate.Height = 23;
			this.dgvTaskList.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvTaskList.DoubleClick += new global::System.EventHandler(this.dgvTaskList_DoubleClick);
			componentResourceManager.ApplyResources(this.f_ID, "f_ID");
			this.f_ID.Name = "f_ID";
			this.f_ID.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_From.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.f_From, "f_From");
			this.f_From.Name = "f_From";
			this.f_From.ReadOnly = true;
			this.f_From.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_From.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_To, "f_To");
			this.f_To.Name = "f_To";
			this.f_To.ReadOnly = true;
			this.f_To.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_To.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.f_OperateHMS1A.DefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.f_OperateHMS1A, "f_OperateHMS1A");
			this.f_OperateHMS1A.Name = "f_OperateHMS1A";
			this.f_OperateHMS1A.ReadOnly = true;
			this.f_OperateHMS1A.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_OperateHMS1A.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.f_Monday, "f_Monday");
			this.f_Monday.Name = "f_Monday";
			this.f_Monday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Tuesday, "f_Tuesday");
			this.f_Tuesday.Name = "f_Tuesday";
			this.f_Tuesday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Wednesday, "f_Wednesday");
			this.f_Wednesday.Name = "f_Wednesday";
			this.f_Wednesday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Thursday, "f_Thursday");
			this.f_Thursday.Name = "f_Thursday";
			this.f_Thursday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Friday, "f_Friday");
			this.f_Friday.Name = "f_Friday";
			this.f_Friday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Saturday, "f_Saturday");
			this.f_Saturday.Name = "f_Saturday";
			this.f_Saturday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Sunday, "f_Sunday");
			this.f_Sunday.Name = "f_Sunday";
			this.f_Sunday.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_AdaptTo, "f_AdaptTo");
			this.f_AdaptTo.Name = "f_AdaptTo";
			this.f_AdaptTo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorControlDesc, "f_DoorControlDesc");
			this.f_DoorControlDesc.Name = "f_DoorControlDesc";
			this.f_DoorControlDesc.ReadOnly = true;
			this.f_Note.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Note, "f_Note");
			this.f_Note.Name = "f_Note";
			this.f_Note.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorControl, "f_DoorControl");
			this.f_DoorControl.Name = "f_DoorControl";
			this.f_DoorControl.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_DoorID, "f_DoorID");
			this.f_DoorID.Name = "f_DoorID";
			this.f_DoorID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAdd.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAdd.ForeColor = global::System.Drawing.Color.White;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new global::System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.cboAccessMethod, "cboAccessMethod");
			this.cboAccessMethod.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAccessMethod.FormattingEnabled = true;
			this.cboAccessMethod.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboAccessMethod.Items"),
				componentResourceManager.GetString("cboAccessMethod.Items1"),
				componentResourceManager.GetString("cboAccessMethod.Items2"),
				componentResourceManager.GetString("cboAccessMethod.Items3"),
				componentResourceManager.GetString("cboAccessMethod.Items4"),
				componentResourceManager.GetString("cboAccessMethod.Items5"),
				componentResourceManager.GetString("cboAccessMethod.Items6"),
				componentResourceManager.GetString("cboAccessMethod.Items7"),
				componentResourceManager.GetString("cboAccessMethod.Items8"),
				componentResourceManager.GetString("cboAccessMethod.Items9"),
				componentResourceManager.GetString("cboAccessMethod.Items10"),
				componentResourceManager.GetString("cboAccessMethod.Items11"),
				componentResourceManager.GetString("cboAccessMethod.Items12")
			});
			this.cboAccessMethod.Name = "cboAccessMethod";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.cboDoors, "cboDoors");
			this.cboDoors.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoors.FormattingEnabled = true;
			this.cboDoors.Name = "cboDoors";
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.checkBox49);
			this.groupBox2.Controls.Add(this.checkBox48);
			this.groupBox2.Controls.Add(this.checkBox47);
			this.groupBox2.Controls.Add(this.checkBox46);
			this.groupBox2.Controls.Add(this.checkBox45);
			this.groupBox2.Controls.Add(this.checkBox44);
			this.groupBox2.Controls.Add(this.checkBox43);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBox49, "checkBox49");
			this.checkBox49.Checked = true;
			this.checkBox49.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox49.Name = "checkBox49";
			this.checkBox49.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox48, "checkBox48");
			this.checkBox48.Checked = true;
			this.checkBox48.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox48.Name = "checkBox48";
			this.checkBox48.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox47, "checkBox47");
			this.checkBox47.Checked = true;
			this.checkBox47.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox47.Name = "checkBox47";
			this.checkBox47.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox46, "checkBox46");
			this.checkBox46.Checked = true;
			this.checkBox46.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox46.Name = "checkBox46";
			this.checkBox46.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox45, "checkBox45");
			this.checkBox45.Checked = true;
			this.checkBox45.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox45.Name = "checkBox45";
			this.checkBox45.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox44, "checkBox44");
			this.checkBox44.Checked = true;
			this.checkBox44.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox44.Name = "checkBox44";
			this.checkBox44.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBox43, "checkBox43");
			this.checkBox43.Checked = true;
			this.checkBox43.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBox43.Name = "checkBox43";
			this.checkBox43.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label45, "label45");
			this.label45.BackColor = global::System.Drawing.Color.Transparent;
			this.label45.ForeColor = global::System.Drawing.Color.White;
			this.label45.Name = "label45";
			componentResourceManager.ApplyResources(this.dtpTime, "dtpTime");
			this.dtpTime.Name = "dtpTime";
			this.dtpTime.ShowUpDown = true;
			this.dtpTime.Value = new global::System.DateTime(2011, 11, 30, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.label43, "label43");
			this.label43.BackColor = global::System.Drawing.Color.Transparent;
			this.label43.ForeColor = global::System.Drawing.Color.White;
			this.label43.Name = "label43";
			componentResourceManager.ApplyResources(this.label44, "label44");
			this.label44.BackColor = global::System.Drawing.Color.Transparent;
			this.label44.ForeColor = global::System.Drawing.Color.White;
			this.label44.Name = "label44";
			componentResourceManager.ApplyResources(this.dtpEnd, "dtpEnd");
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Value = new global::System.DateTime(2099, 12, 31, 14, 44, 0, 0);
			componentResourceManager.ApplyResources(this.dtpBegin, "dtpBegin");
			this.dtpBegin.Name = "dtpBegin";
			this.dtpBegin.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnDel);
			base.Controls.Add(this.dgvTaskList);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cboAccessMethod);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cboDoors);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label45);
			base.Controls.Add(this.dtpTime);
			base.Controls.Add(this.label43);
			base.Controls.Add(this.label44);
			base.Controls.Add(this.dtpEnd);
			base.Controls.Add(this.dtpBegin);
			base.Name = "dfrmControllerTaskList";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerTaskList_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerTaskList_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerTaskList_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvTaskList).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001F50 RID: 8016
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001F51 RID: 8017
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04001F52 RID: 8018
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001F53 RID: 8019
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04001F54 RID: 8020
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04001F55 RID: 8021
		private global::System.Windows.Forms.ComboBox cboAccessMethod;

		// Token: 0x04001F56 RID: 8022
		private global::System.Windows.Forms.ComboBox cboDoors;

		// Token: 0x04001F57 RID: 8023
		private global::System.Windows.Forms.CheckBox checkBox43;

		// Token: 0x04001F58 RID: 8024
		private global::System.Windows.Forms.CheckBox checkBox44;

		// Token: 0x04001F59 RID: 8025
		private global::System.Windows.Forms.CheckBox checkBox45;

		// Token: 0x04001F5A RID: 8026
		private global::System.Windows.Forms.CheckBox checkBox46;

		// Token: 0x04001F5B RID: 8027
		private global::System.Windows.Forms.CheckBox checkBox47;

		// Token: 0x04001F5C RID: 8028
		private global::System.Windows.Forms.CheckBox checkBox48;

		// Token: 0x04001F5D RID: 8029
		private global::System.Windows.Forms.CheckBox checkBox49;

		// Token: 0x04001F5E RID: 8030
		private global::System.Windows.Forms.DataGridView dgvTaskList;

		// Token: 0x04001F5F RID: 8031
		private global::System.Windows.Forms.DateTimePicker dtpBegin;

		// Token: 0x04001F60 RID: 8032
		private global::System.Windows.Forms.DateTimePicker dtpEnd;

		// Token: 0x04001F61 RID: 8033
		private global::System.Windows.Forms.DateTimePicker dtpTime;

		// Token: 0x04001F62 RID: 8034
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AdaptTo;

		// Token: 0x04001F63 RID: 8035
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorControl;

		// Token: 0x04001F64 RID: 8036
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorControlDesc;

		// Token: 0x04001F65 RID: 8037
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorID;

		// Token: 0x04001F66 RID: 8038
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Friday;

		// Token: 0x04001F67 RID: 8039
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_From;

		// Token: 0x04001F68 RID: 8040
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x04001F69 RID: 8041
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Monday;

		// Token: 0x04001F6A RID: 8042
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x04001F6B RID: 8043
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OperateHMS1A;

		// Token: 0x04001F6C RID: 8044
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Saturday;

		// Token: 0x04001F6D RID: 8045
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Sunday;

		// Token: 0x04001F6E RID: 8046
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Thursday;

		// Token: 0x04001F6F RID: 8047
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_To;

		// Token: 0x04001F70 RID: 8048
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Tuesday;

		// Token: 0x04001F71 RID: 8049
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Wednesday;

		// Token: 0x04001F72 RID: 8050
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04001F73 RID: 8051
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001F74 RID: 8052
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001F75 RID: 8053
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001F76 RID: 8054
		private global::System.Windows.Forms.Label label43;

		// Token: 0x04001F77 RID: 8055
		private global::System.Windows.Forms.Label label44;

		// Token: 0x04001F78 RID: 8056
		private global::System.Windows.Forms.Label label45;

		// Token: 0x04001F79 RID: 8057
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
