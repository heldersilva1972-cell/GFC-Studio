namespace WG3000_COMM.Basic
{
	// Token: 0x0200002E RID: 46
	public partial class dfrmSystemParam : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0005EF30 File Offset: 0x0005DF30
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0005EF50 File Offset: 0x0005DF50
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmSystemParam));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_NO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Name = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Value = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_EName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Notes = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Modified = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_OldValue = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.dataGridView1, "dataGridView1");
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(192, 255, 255);
			this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_NO, this.f_Name, this.f_Value, this.f_EName, this.f_Notes, this.f_Modified, this.f_OldValue });
			dataGridViewCellStyle3.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle3.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			dataGridViewCellStyle4.BackColor = global::System.Drawing.Color.FromArgb(224, 224, 224);
			this.f_NO.DefaultCellStyle = dataGridViewCellStyle4;
			this.f_NO.Frozen = true;
			componentResourceManager.ApplyResources(this.f_NO, "f_NO");
			this.f_NO.Name = "f_NO";
			this.f_NO.ReadOnly = true;
			dataGridViewCellStyle5.BackColor = global::System.Drawing.Color.FromArgb(224, 224, 224);
			this.f_Name.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.f_Name, "f_Name");
			this.f_Name.Name = "f_Name";
			this.f_Name.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Value, "f_Value");
			this.f_Value.Name = "f_Value";
			componentResourceManager.ApplyResources(this.f_EName, "f_EName");
			this.f_EName.Name = "f_EName";
			componentResourceManager.ApplyResources(this.f_Notes, "f_Notes");
			this.f_Notes.Name = "f_Notes";
			this.f_Notes.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Modified, "f_Modified");
			this.f_Modified.Name = "f_Modified";
			componentResourceManager.ApplyResources(this.f_OldValue, "f_OldValue");
			this.f_OldValue.Name = "f_OldValue";
			this.f_OldValue.ReadOnly = true;
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmSystemParam";
			base.Load += new global::System.EventHandler(this.dfrmSystemParam_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmSystemParam_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400062A RID: 1578
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400062B RID: 1579
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x0400062C RID: 1580
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_EName;

		// Token: 0x0400062D RID: 1581
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Modified;

		// Token: 0x0400062E RID: 1582
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Name;

		// Token: 0x0400062F RID: 1583
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NO;

		// Token: 0x04000630 RID: 1584
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Notes;

		// Token: 0x04000631 RID: 1585
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_OldValue;

		// Token: 0x04000632 RID: 1586
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Value;

		// Token: 0x04000633 RID: 1587
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04000634 RID: 1588
		internal global::System.Windows.Forms.Button btnOK;
	}
}
