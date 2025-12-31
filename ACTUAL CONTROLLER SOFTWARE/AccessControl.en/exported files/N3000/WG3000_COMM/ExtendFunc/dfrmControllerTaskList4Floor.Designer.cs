namespace WG3000_COMM.ExtendFunc
{
	// Token: 0x0200023F RID: 575
	public partial class dfrmControllerTaskList4Floor : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x00149380 File Offset: 0x00148380
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

		// Token: 0x0600119A RID: 4506 RVA: 0x001493B8 File Offset: 0x001483B8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.dfrmControllerTaskList4Floor));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnEdit = new global::System.Windows.Forms.Button();
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
			((global::System.ComponentModel.ISupportInitialize)this.dgvTaskList).BeginInit();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
			this.btnEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnEdit.ForeColor = global::System.Drawing.Color.White;
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new global::System.EventHandler(this.btnEdit_Click);
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
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvTaskList);
			base.Controls.Add(this.btnEdit);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.btnDel);
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
			base.Name = "dfrmControllerTaskList4Floor";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmControllerTaskList_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmControllerTaskList_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmControllerTaskList_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dgvTaskList).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001F7F RID: 8063
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001F80 RID: 8064
		private global::System.Windows.Forms.Button btnAdd;

		// Token: 0x04001F81 RID: 8065
		private global::System.Windows.Forms.Button btnClose;

		// Token: 0x04001F82 RID: 8066
		private global::System.Windows.Forms.Button btnDel;

		// Token: 0x04001F83 RID: 8067
		private global::System.Windows.Forms.Button btnEdit;

		// Token: 0x04001F84 RID: 8068
		private global::System.Windows.Forms.ComboBox cboAccessMethod;

		// Token: 0x04001F85 RID: 8069
		private global::System.Windows.Forms.ComboBox cboDoors;

		// Token: 0x04001F86 RID: 8070
		private global::System.Windows.Forms.CheckBox checkBox43;

		// Token: 0x04001F87 RID: 8071
		private global::System.Windows.Forms.CheckBox checkBox44;

		// Token: 0x04001F88 RID: 8072
		private global::System.Windows.Forms.CheckBox checkBox45;

		// Token: 0x04001F89 RID: 8073
		private global::System.Windows.Forms.CheckBox checkBox46;

		// Token: 0x04001F8A RID: 8074
		private global::System.Windows.Forms.CheckBox checkBox47;

		// Token: 0x04001F8B RID: 8075
		private global::System.Windows.Forms.CheckBox checkBox48;

		// Token: 0x04001F8C RID: 8076
		private global::System.Windows.Forms.CheckBox checkBox49;

		// Token: 0x04001F8D RID: 8077
		private global::System.Windows.Forms.DataGridView dgvTaskList;

		// Token: 0x04001F8E RID: 8078
		private global::System.Windows.Forms.DateTimePicker dtpBegin;

		// Token: 0x04001F8F RID: 8079
		private global::System.Windows.Forms.DateTimePicker dtpEnd;

		// Token: 0x04001F90 RID: 8080
		private global::System.Windows.Forms.DateTimePicker dtpTime;

		// Token: 0x04001F91 RID: 8081
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_AdaptTo;

		// Token: 0x04001F92 RID: 8082
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorControl;

		// Token: 0x04001F93 RID: 8083
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorControlDesc;

		// Token: 0x04001F94 RID: 8084
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_DoorID;

		// Token: 0x04001F95 RID: 8085
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Friday;

		// Token: 0x04001F96 RID: 8086
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_From;

		// Token: 0x04001F97 RID: 8087
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_ID;

		// Token: 0x04001F98 RID: 8088
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Monday;

		// Token: 0x04001F99 RID: 8089
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x04001F9A RID: 8090
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OperateHMS1A;

		// Token: 0x04001F9B RID: 8091
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Saturday;

		// Token: 0x04001F9C RID: 8092
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Sunday;

		// Token: 0x04001F9D RID: 8093
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Thursday;

		// Token: 0x04001F9E RID: 8094
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_To;

		// Token: 0x04001F9F RID: 8095
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Tuesday;

		// Token: 0x04001FA0 RID: 8096
		private global::System.Windows.Forms.DataGridViewCheckBoxColumn f_Wednesday;

		// Token: 0x04001FA1 RID: 8097
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04001FA2 RID: 8098
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04001FA3 RID: 8099
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04001FA4 RID: 8100
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04001FA5 RID: 8101
		private global::System.Windows.Forms.Label label43;

		// Token: 0x04001FA6 RID: 8102
		private global::System.Windows.Forms.Label label44;

		// Token: 0x04001FA7 RID: 8103
		private global::System.Windows.Forms.Label label45;

		// Token: 0x04001FA8 RID: 8104
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
