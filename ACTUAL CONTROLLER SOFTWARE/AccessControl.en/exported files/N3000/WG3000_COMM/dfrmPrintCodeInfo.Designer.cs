namespace WG3000_COMM
{
	// Token: 0x0200022C RID: 556
	public partial class dfrmPrintCodeInfo : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001098 RID: 4248 RVA: 0x0012D45D File Offset: 0x0012C45D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0012D47C File Offset: 0x0012C47C
		private void InitializeComponent()
		{
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.dfrmPrintCodeInfo));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.f_producttype = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_mode = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Code = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_SN = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_CPU = new global::System.Windows.Forms.DataGridViewComboBoxColumn();
			this.f_MAC = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnOK.Location = new global::System.Drawing.Point(375, 20);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new global::System.Drawing.Size(75, 27);
			this.btnOK.TabIndex = 19;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new global::System.Drawing.Point(511, 20);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 27);
			this.btnCancel.TabIndex = 18;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.dataGridView1.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.dataGridView1.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_producttype, this.f_mode, this.f_Code, this.f_SN, this.f_CPU, this.f_MAC });
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Location = new global::System.Drawing.Point(12, 53);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersBorderStyle = global::System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.Color.FromArgb(146, 150, 177);
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Pixel, 134);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.RowHeadersWidth = 20;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new global::System.Drawing.Size(900, 501);
			this.dataGridView1.TabIndex = 17;
			this.f_producttype.HeaderText = "产品类别";
			this.f_producttype.Name = "f_producttype";
			this.f_producttype.Width = 200;
			this.f_mode.HeaderText = "型号";
			this.f_mode.Name = "f_mode";
			this.f_mode.Width = 200;
			this.f_Code.HeaderText = "条码";
			this.f_Code.Name = "f_Code";
			this.f_Code.Width = 200;
			this.f_SN.HeaderText = "序列号";
			this.f_SN.Name = "f_SN";
			this.f_SN.Width = 200;
			this.f_CPU.DisplayStyle = global::System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.f_CPU.HeaderText = "模块芯片";
			this.f_CPU.Items.AddRange(new object[] { "1766", "6911" });
			this.f_CPU.Name = "f_CPU";
			this.f_CPU.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_CPU.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.f_CPU.Width = 60;
			this.f_MAC.HeaderText = "MAC Addr";
			this.f_MAC.Name = "f_MAC";
			this.f_MAC.Visible = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.ClientSize = new global::System.Drawing.Size(924, 566);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.dataGridView1);
			base.Name = "dfrmPrintCodeInfo";
			this.Text = "条码与序列号对应关系";
			base.Load += new global::System.EventHandler(this.dfrmPrintCodeInfo_Load);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04001D77 RID: 7543
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001D78 RID: 7544
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001D79 RID: 7545
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04001D7A RID: 7546
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04001D7B RID: 7547
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Code;

		// Token: 0x04001D7C RID: 7548
		private global::System.Windows.Forms.DataGridViewComboBoxColumn f_CPU;

		// Token: 0x04001D7D RID: 7549
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MAC;

		// Token: 0x04001D7E RID: 7550
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_mode;

		// Token: 0x04001D7F RID: 7551
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_producttype;

		// Token: 0x04001D80 RID: 7552
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_SN;
	}
}
