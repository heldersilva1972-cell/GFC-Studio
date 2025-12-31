namespace WG3000_COMM.Basic
{
	// Token: 0x02000009 RID: 9
	public partial class dfrmControlHolidaySet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600007A RID: 122 RVA: 0x0000FEF4 File Offset: 0x0000EEF4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000FF14 File Offset: 0x0000EF14
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmControlHolidaySet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.dgvNeedWork = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAddNeedWorkDay = new global::System.Windows.Forms.Button();
			this.btnDelNeedWorkDay = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_No = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_from = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_to = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAddHoliday = new global::System.Windows.Forms.Button();
			this.btnDelHoliday = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvNeedWork).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.dgvNeedWork);
			this.groupBox2.Controls.Add(this.btnAddNeedWorkDay);
			this.groupBox2.Controls.Add(this.btnDelNeedWorkDay);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.dgvNeedWork, "dgvNeedWork");
			this.dgvNeedWork.AllowUserToAddRows = false;
			this.dgvNeedWork.AllowUserToDeleteRows = false;
			this.dgvNeedWork.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvNeedWork.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvNeedWork.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvNeedWork.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4 });
			this.dgvNeedWork.EnableHeadersVisualStyles = false;
			this.dgvNeedWork.Name = "dgvNeedWork";
			this.dgvNeedWork.ReadOnly = true;
			this.dgvNeedWork.RowTemplate.Height = 23;
			this.dgvNeedWork.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnAddNeedWorkDay, "btnAddNeedWorkDay");
			this.btnAddNeedWorkDay.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddNeedWorkDay.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddNeedWorkDay.ForeColor = global::System.Drawing.Color.White;
			this.btnAddNeedWorkDay.Name = "btnAddNeedWorkDay";
			this.btnAddNeedWorkDay.UseVisualStyleBackColor = false;
			this.btnAddNeedWorkDay.Click += new global::System.EventHandler(this.btnAddNeedWorkDay_Click);
			componentResourceManager.ApplyResources(this.btnDelNeedWorkDay, "btnDelNeedWorkDay");
			this.btnDelNeedWorkDay.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelNeedWorkDay.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelNeedWorkDay.ForeColor = global::System.Drawing.Color.White;
			this.btnDelNeedWorkDay.Name = "btnDelNeedWorkDay";
			this.btnDelNeedWorkDay.UseVisualStyleBackColor = false;
			this.btnDelNeedWorkDay.Click += new global::System.EventHandler(this.btnDelNeedWorkDay_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.dgvMain);
			this.groupBox1.Controls.Add(this.btnAddHoliday);
			this.groupBox1.Controls.Add(this.btnDelHoliday);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_No, this.f_from, this.f_to, this.f_Note });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_No, "f_No");
			this.f_No.Name = "f_No";
			this.f_No.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_from, "f_from");
			this.f_from.Name = "f_from";
			this.f_from.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_to, "f_to");
			this.f_to.Name = "f_to";
			this.f_to.ReadOnly = true;
			this.f_Note.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Note, "f_Note");
			this.f_Note.Name = "f_Note";
			this.f_Note.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnAddHoliday, "btnAddHoliday");
			this.btnAddHoliday.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddHoliday.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddHoliday.ForeColor = global::System.Drawing.Color.White;
			this.btnAddHoliday.Name = "btnAddHoliday";
			this.btnAddHoliday.UseVisualStyleBackColor = false;
			this.btnAddHoliday.Click += new global::System.EventHandler(this.btnAddHoliday_Click);
			componentResourceManager.ApplyResources(this.btnDelHoliday, "btnDelHoliday");
			this.btnDelHoliday.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelHoliday.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelHoliday.ForeColor = global::System.Drawing.Color.White;
			this.btnDelHoliday.Name = "btnDelHoliday";
			this.btnDelHoliday.UseVisualStyleBackColor = false;
			this.btnDelHoliday.Click += new global::System.EventHandler(this.btnDelHoliday_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmControlHolidaySet";
			base.Load += new global::System.EventHandler(this.dfrmHolidaySet_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvNeedWork).EndInit();
			this.groupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000B3 RID: 179
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.DataGridView dgvNeedWork;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_from;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_No;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x040000BD RID: 189
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_to;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000BF RID: 191
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040000C0 RID: 192
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000C1 RID: 193
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000C2 RID: 194
		internal global::System.Windows.Forms.Button btnAddHoliday;

		// Token: 0x040000C3 RID: 195
		internal global::System.Windows.Forms.Button btnAddNeedWorkDay;

		// Token: 0x040000C4 RID: 196
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040000C5 RID: 197
		internal global::System.Windows.Forms.Button btnDelHoliday;

		// Token: 0x040000C6 RID: 198
		internal global::System.Windows.Forms.Button btnDelNeedWorkDay;
	}
}
