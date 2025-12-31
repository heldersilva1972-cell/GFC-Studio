namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000369 RID: 873
	public partial class dfrmHolidaySet : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001C71 RID: 7281 RVA: 0x00257BCC File Offset: 0x00256BCC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00257BEC File Offset: 0x00256BEC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Reports.Shift.dfrmHolidaySet));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.btnAddHoliday = new global::System.Windows.Forms.Button();
			this.btnDelHoliday = new global::System.Windows.Forms.Button();
			this.btnAddNeedWork = new global::System.Windows.Forms.Button();
			this.btnDelNeedWork = new global::System.Windows.Forms.Button();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.optSunWork2 = new global::System.Windows.Forms.RadioButton();
			this.optSunWork0 = new global::System.Windows.Forms.RadioButton();
			this.optSunWork1 = new global::System.Windows.Forms.RadioButton();
			this.GroupBox1 = new global::System.Windows.Forms.GroupBox();
			this.optSatWork2 = new global::System.Windows.Forms.RadioButton();
			this.optSatWork0 = new global::System.Windows.Forms.RadioButton();
			this.optSatWork1 = new global::System.Windows.Forms.RadioButton();
			this.GroupBox2 = new global::System.Windows.Forms.GroupBox();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.dgvMain = new global::System.Windows.Forms.DataGridView();
			this.f_Name = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_from = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.From1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_to = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.To1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Note = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_No = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Value = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.dgvMain2 = new global::System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.From2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.To2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain2).BeginInit();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.btnAddNeedWork, "btnAddNeedWork");
			this.btnAddNeedWork.BackColor = global::System.Drawing.Color.Transparent;
			this.btnAddNeedWork.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddNeedWork.ForeColor = global::System.Drawing.Color.White;
			this.btnAddNeedWork.Name = "btnAddNeedWork";
			this.btnAddNeedWork.UseVisualStyleBackColor = false;
			this.btnAddNeedWork.Click += new global::System.EventHandler(this.btnAddNeedWork_Click);
			componentResourceManager.ApplyResources(this.btnDelNeedWork, "btnDelNeedWork");
			this.btnDelNeedWork.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDelNeedWork.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDelNeedWork.ForeColor = global::System.Drawing.Color.White;
			this.btnDelNeedWork.Name = "btnDelNeedWork";
			this.btnDelNeedWork.UseVisualStyleBackColor = false;
			this.btnDelNeedWork.Click += new global::System.EventHandler(this.btnDelNeedWork_Click);
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.optSunWork2, "optSunWork2");
			this.optSunWork2.Name = "optSunWork2";
			componentResourceManager.ApplyResources(this.optSunWork0, "optSunWork0");
			this.optSunWork0.Checked = true;
			this.optSunWork0.Name = "optSunWork0";
			this.optSunWork0.TabStop = true;
			componentResourceManager.ApplyResources(this.optSunWork1, "optSunWork1");
			this.optSunWork1.Name = "optSunWork1";
			componentResourceManager.ApplyResources(this.GroupBox1, "GroupBox1");
			this.GroupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox1.Controls.Add(this.optSatWork2);
			this.GroupBox1.Controls.Add(this.optSatWork0);
			this.GroupBox1.Controls.Add(this.optSatWork1);
			this.GroupBox1.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.optSatWork2, "optSatWork2");
			this.optSatWork2.Name = "optSatWork2";
			componentResourceManager.ApplyResources(this.optSatWork0, "optSatWork0");
			this.optSatWork0.Checked = true;
			this.optSatWork0.Name = "optSatWork0";
			this.optSatWork0.TabStop = true;
			componentResourceManager.ApplyResources(this.optSatWork1, "optSatWork1");
			this.optSatWork1.Name = "optSatWork1";
			componentResourceManager.ApplyResources(this.GroupBox2, "GroupBox2");
			this.GroupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.GroupBox2.Controls.Add(this.optSunWork2);
			this.GroupBox2.Controls.Add(this.optSunWork0);
			this.GroupBox2.Controls.Add(this.optSunWork1);
			this.GroupBox2.ForeColor = global::System.Drawing.Color.White;
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.dgvMain, "dgvMain");
			this.dgvMain.AllowUserToAddRows = false;
			this.dgvMain.AllowUserToDeleteRows = false;
			this.dgvMain.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvMain.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_Name, this.f_from, this.From1, this.f_to, this.To1, this.f_Note, this.f_No, this.f_Value });
			this.dgvMain.EnableHeadersVisualStyles = false;
			this.dgvMain.Name = "dgvMain";
			this.dgvMain.ReadOnly = true;
			this.dgvMain.RowTemplate.Height = 23;
			this.dgvMain.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.f_Name, "f_Name");
			this.f_Name.Name = "f_Name";
			this.f_Name.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_from, "f_from");
			this.f_from.Name = "f_from";
			this.f_from.ReadOnly = true;
			componentResourceManager.ApplyResources(this.From1, "From1");
			this.From1.Name = "From1";
			this.From1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_to, "f_to");
			this.f_to.Name = "f_to";
			this.f_to.ReadOnly = true;
			componentResourceManager.ApplyResources(this.To1, "To1");
			this.To1.Name = "To1";
			this.To1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Note, "f_Note");
			this.f_Note.Name = "f_Note";
			this.f_Note.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_No, "f_No");
			this.f_No.Name = "f_No";
			this.f_No.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Value, "f_Value");
			this.f_Value.Name = "f_Value";
			this.f_Value.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = global::System.Drawing.Color.Transparent;
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.dgvMain2, "dgvMain2");
			this.dgvMain2.AllowUserToAddRows = false;
			this.dgvMain2.AllowUserToDeleteRows = false;
			this.dgvMain2.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMain2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvMain2.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvMain2.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.From2, this.dataGridViewTextBoxColumn3, this.To2, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn5, this.dataGridViewTextBoxColumn6 });
			this.dgvMain2.EnableHeadersVisualStyles = false;
			this.dgvMain2.Name = "dgvMain2";
			this.dgvMain2.ReadOnly = true;
			this.dgvMain2.RowTemplate.Height = 23;
			this.dgvMain2.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.From2, "From2");
			this.From2.Name = "From2";
			this.From2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.To2, "To2");
			this.To2.Name = "To2";
			this.To2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.dgvMain2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.dgvMain);
			base.Controls.Add(this.btnAddHoliday);
			base.Controls.Add(this.btnDelHoliday);
			base.Controls.Add(this.btnAddNeedWork);
			base.Controls.Add(this.btnDelNeedWork);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.GroupBox1);
			base.Controls.Add(this.GroupBox2);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmHolidaySet";
			base.Load += new global::System.EventHandler(this.dfrmHolidaySet_Load);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.dgvMain2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040036B0 RID: 14000
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040036B1 RID: 14001
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		// Token: 0x040036B2 RID: 14002
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		// Token: 0x040036B3 RID: 14003
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		// Token: 0x040036B4 RID: 14004
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		// Token: 0x040036B5 RID: 14005
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		// Token: 0x040036B6 RID: 14006
		private global::System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		// Token: 0x040036B7 RID: 14007
		private global::System.Windows.Forms.DataGridView dgvMain;

		// Token: 0x040036B8 RID: 14008
		private global::System.Windows.Forms.DataGridView dgvMain2;

		// Token: 0x040036B9 RID: 14009
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_from;

		// Token: 0x040036BA RID: 14010
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Name;

		// Token: 0x040036BB RID: 14011
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_No;

		// Token: 0x040036BC RID: 14012
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Note;

		// Token: 0x040036BD RID: 14013
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_to;

		// Token: 0x040036BE RID: 14014
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Value;

		// Token: 0x040036BF RID: 14015
		private global::System.Windows.Forms.DataGridViewTextBoxColumn From1;

		// Token: 0x040036C0 RID: 14016
		private global::System.Windows.Forms.DataGridViewTextBoxColumn From2;

		// Token: 0x040036C1 RID: 14017
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040036C2 RID: 14018
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040036C3 RID: 14019
		private global::System.Windows.Forms.DataGridViewTextBoxColumn To1;

		// Token: 0x040036C4 RID: 14020
		private global::System.Windows.Forms.DataGridViewTextBoxColumn To2;

		// Token: 0x040036C5 RID: 14021
		internal global::System.Windows.Forms.Button btnAddHoliday;

		// Token: 0x040036C6 RID: 14022
		internal global::System.Windows.Forms.Button btnAddNeedWork;

		// Token: 0x040036C7 RID: 14023
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040036C8 RID: 14024
		internal global::System.Windows.Forms.Button btnDelHoliday;

		// Token: 0x040036C9 RID: 14025
		internal global::System.Windows.Forms.Button btnDelNeedWork;

		// Token: 0x040036CA RID: 14026
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x040036CB RID: 14027
		internal global::System.Windows.Forms.GroupBox GroupBox1;

		// Token: 0x040036CC RID: 14028
		internal global::System.Windows.Forms.GroupBox GroupBox2;

		// Token: 0x040036CD RID: 14029
		internal global::System.Windows.Forms.RadioButton optSatWork0;

		// Token: 0x040036CE RID: 14030
		internal global::System.Windows.Forms.RadioButton optSatWork1;

		// Token: 0x040036CF RID: 14031
		internal global::System.Windows.Forms.RadioButton optSatWork2;

		// Token: 0x040036D0 RID: 14032
		internal global::System.Windows.Forms.RadioButton optSunWork0;

		// Token: 0x040036D1 RID: 14033
		internal global::System.Windows.Forms.RadioButton optSunWork1;

		// Token: 0x040036D2 RID: 14034
		internal global::System.Windows.Forms.RadioButton optSunWork2;
	}
}
