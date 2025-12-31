namespace WG3000_COMM.Basic
{
	// Token: 0x02000031 RID: 49
	public partial class dfrmTranslate : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x0600036E RID: 878 RVA: 0x00064E60 File Offset: 0x00063E60
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00064E80 File Offset: 0x00063E80
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmTranslate));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_NO = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Name = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CName = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Value = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.btnLoad = new global::System.Windows.Forms.Button();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
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
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_NO, this.f_Name, this.f_CName, this.f_Value });
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
			componentResourceManager.ApplyResources(this.f_CName, "f_CName");
			this.f_CName.Name = "f_CName";
			this.f_CName.ReadOnly = true;
			this.f_Value.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Value, "f_Value");
			this.f_Value.Name = "f_Value";
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
			componentResourceManager.ApplyResources(this.btnLoad, "btnLoad");
			this.btnLoad.BackColor = global::System.Drawing.Color.Transparent;
			this.btnLoad.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnLoad.ForeColor = global::System.Drawing.Color.White;
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.UseVisualStyleBackColor = false;
			this.btnLoad.Click += new global::System.EventHandler(this.btnLoad_Click);
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnLoad);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmTranslate";
			base.Load += new global::System.EventHandler(this.dfrmSystemParam_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmSystemParam_KeyDown);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x0400069F RID: 1695
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040006A0 RID: 1696
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x040006A1 RID: 1697
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_CName;

		// Token: 0x040006A2 RID: 1698
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Name;

		// Token: 0x040006A3 RID: 1699
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_NO;

		// Token: 0x040006A4 RID: 1700
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Value;

		// Token: 0x040006A5 RID: 1701
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040006A6 RID: 1702
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040006A7 RID: 1703
		internal global::System.Windows.Forms.Button btnLoad;

		// Token: 0x040006A8 RID: 1704
		internal global::System.Windows.Forms.Button btnOK;
	}
}
