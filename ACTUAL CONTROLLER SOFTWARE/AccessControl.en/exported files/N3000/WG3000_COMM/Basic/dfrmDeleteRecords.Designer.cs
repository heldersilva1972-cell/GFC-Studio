namespace WG3000_COMM.Basic
{
	// Token: 0x0200000F RID: 15
	public partial class dfrmDeleteRecords : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060000BD RID: 189 RVA: 0x0001D6C2 File Offset: 0x0001C6C2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0001D6E4 File Offset: 0x0001C6E4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmDeleteRecords));
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnExit = new global::System.Windows.Forms.Button();
			this.btnDeleteAllSwipeRecords = new global::System.Windows.Forms.Button();
			this.btnDeleteLog = new global::System.Windows.Forms.Button();
			this.btnDeleteOldSwipeRecords = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.nudIndexMin = new global::System.Windows.Forms.NumericUpDown();
			this.lblIndexMin = new global::System.Windows.Forms.Label();
			this.nudSwipeRecordIndex = new global::System.Windows.Forms.NumericUpDown();
			this.lblIndex = new global::System.Windows.Forms.Label();
			this.btnBackupDatabase = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.numericUpDown2 = new global::System.Windows.Forms.NumericUpDown();
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnKeepSwipeRecords = new global::System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudIndexMin).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudSwipeRecordIndex).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown2).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.BackColor = global::System.Drawing.Color.Transparent;
			this.label1.ForeColor = global::System.Drawing.Color.Yellow;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.btnExit, "btnExit");
			this.btnExit.BackColor = global::System.Drawing.Color.Transparent;
			this.btnExit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnExit.ForeColor = global::System.Drawing.Color.White;
			this.btnExit.Name = "btnExit";
			this.btnExit.UseVisualStyleBackColor = false;
			this.btnExit.Click += new global::System.EventHandler(this.btnExit_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAllSwipeRecords, "btnDeleteAllSwipeRecords");
			this.btnDeleteAllSwipeRecords.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteAllSwipeRecords.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAllSwipeRecords.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAllSwipeRecords.Name = "btnDeleteAllSwipeRecords";
			this.btnDeleteAllSwipeRecords.UseVisualStyleBackColor = false;
			this.btnDeleteAllSwipeRecords.Click += new global::System.EventHandler(this.btnDeleteAllSwipeRecords_Click);
			componentResourceManager.ApplyResources(this.btnDeleteLog, "btnDeleteLog");
			this.btnDeleteLog.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteLog.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteLog.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteLog.Name = "btnDeleteLog";
			this.btnDeleteLog.UseVisualStyleBackColor = false;
			this.btnDeleteLog.Click += new global::System.EventHandler(this.btnDeleteLog_Click);
			componentResourceManager.ApplyResources(this.btnDeleteOldSwipeRecords, "btnDeleteOldSwipeRecords");
			this.btnDeleteOldSwipeRecords.BackColor = global::System.Drawing.Color.Transparent;
			this.btnDeleteOldSwipeRecords.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOldSwipeRecords.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOldSwipeRecords.Name = "btnDeleteOldSwipeRecords";
			this.btnDeleteOldSwipeRecords.UseVisualStyleBackColor = false;
			this.btnDeleteOldSwipeRecords.Click += new global::System.EventHandler(this.btnDeleteOldSwipeRecords_Click);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.nudIndexMin);
			this.groupBox2.Controls.Add(this.lblIndexMin);
			this.groupBox2.Controls.Add(this.nudSwipeRecordIndex);
			this.groupBox2.Controls.Add(this.lblIndex);
			this.groupBox2.Controls.Add(this.btnDeleteOldSwipeRecords);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.nudIndexMin, "nudIndexMin");
			this.nudIndexMin.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.nudIndexMin;
			int[] array = new int[4];
			array[0] = 1000;
			numericUpDown.Increment = new decimal(array);
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.nudIndexMin;
			int[] array2 = new int[4];
			array2[0] = -1;
			array2[1] = -1;
			numericUpDown2.Maximum = new decimal(array2);
			this.nudIndexMin.Name = "nudIndexMin";
			componentResourceManager.ApplyResources(this.lblIndexMin, "lblIndexMin");
			this.lblIndexMin.Name = "lblIndexMin";
			componentResourceManager.ApplyResources(this.nudSwipeRecordIndex, "nudSwipeRecordIndex");
			this.nudSwipeRecordIndex.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown3 = this.nudSwipeRecordIndex;
			int[] array3 = new int[4];
			array3[0] = 1000;
			numericUpDown3.Increment = new decimal(array3);
			global::System.Windows.Forms.NumericUpDown numericUpDown4 = this.nudSwipeRecordIndex;
			int[] array4 = new int[4];
			array4[0] = -1;
			array4[1] = -1;
			numericUpDown4.Maximum = new decimal(array4);
			this.nudSwipeRecordIndex.Name = "nudSwipeRecordIndex";
			componentResourceManager.ApplyResources(this.lblIndex, "lblIndex");
			this.lblIndex.Name = "lblIndex";
			componentResourceManager.ApplyResources(this.btnBackupDatabase, "btnBackupDatabase");
			this.btnBackupDatabase.BackColor = global::System.Drawing.Color.Transparent;
			this.btnBackupDatabase.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnBackupDatabase.ForeColor = global::System.Drawing.Color.White;
			this.btnBackupDatabase.Name = "btnBackupDatabase";
			this.btnBackupDatabase.UseVisualStyleBackColor = false;
			this.btnBackupDatabase.Click += new global::System.EventHandler(this.btnBackupDatabase_Click);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = global::System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.numericUpDown2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btnKeepSwipeRecords);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.numericUpDown2, "numericUpDown2");
			this.numericUpDown2.BackColor = global::System.Drawing.Color.White;
			global::System.Windows.Forms.NumericUpDown numericUpDown5 = this.numericUpDown2;
			int[] array5 = new int[4];
			array5[0] = 10000;
			numericUpDown5.Increment = new decimal(array5);
			global::System.Windows.Forms.NumericUpDown numericUpDown6 = this.numericUpDown2;
			int[] array6 = new int[4];
			array6[0] = 10000000;
			numericUpDown6.Maximum = new decimal(array6);
			global::System.Windows.Forms.NumericUpDown numericUpDown7 = this.numericUpDown2;
			int[] array7 = new int[4];
			array7[0] = 10000;
			numericUpDown7.Minimum = new decimal(array7);
			this.numericUpDown2.Name = "numericUpDown2";
			global::System.Windows.Forms.NumericUpDown numericUpDown8 = this.numericUpDown2;
			int[] array8 = new int[4];
			array8[0] = 50000;
			numericUpDown8.Value = new decimal(array8);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.btnKeepSwipeRecords, "btnKeepSwipeRecords");
			this.btnKeepSwipeRecords.BackColor = global::System.Drawing.Color.Transparent;
			this.btnKeepSwipeRecords.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnKeepSwipeRecords.ForeColor = global::System.Drawing.Color.White;
			this.btnKeepSwipeRecords.Name = "btnKeepSwipeRecords";
			this.btnKeepSwipeRecords.UseVisualStyleBackColor = false;
			this.btnKeepSwipeRecords.Click += new global::System.EventHandler(this.btnKeepSwipeRecords_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnExit);
			base.Controls.Add(this.btnDeleteLog);
			base.Controls.Add(this.btnBackupDatabase);
			base.Controls.Add(this.btnDeleteAllSwipeRecords);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmDeleteRecords";
			base.Load += new global::System.EventHandler(this.dfrmDeleteRecords_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmDeleteRecords_KeyDown);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.nudIndexMin).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudSwipeRecordIndex).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.numericUpDown2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001D3 RID: 467
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040001D4 RID: 468
		private global::System.Windows.Forms.Button btnBackupDatabase;

		// Token: 0x040001D5 RID: 469
		private global::System.Windows.Forms.Button btnDeleteAllSwipeRecords;

		// Token: 0x040001D6 RID: 470
		private global::System.Windows.Forms.Button btnDeleteLog;

		// Token: 0x040001D7 RID: 471
		private global::System.Windows.Forms.Button btnDeleteOldSwipeRecords;

		// Token: 0x040001D8 RID: 472
		private global::System.Windows.Forms.Button btnExit;

		// Token: 0x040001D9 RID: 473
		private global::System.Windows.Forms.Button btnKeepSwipeRecords;

		// Token: 0x040001DA RID: 474
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040001DB RID: 475
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x040001DC RID: 476
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040001DD RID: 477
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040001DE RID: 478
		private global::System.Windows.Forms.Label lblIndex;

		// Token: 0x040001DF RID: 479
		private global::System.Windows.Forms.Label lblIndexMin;

		// Token: 0x040001E0 RID: 480
		internal global::System.Windows.Forms.NumericUpDown nudIndexMin;

		// Token: 0x040001E1 RID: 481
		internal global::System.Windows.Forms.NumericUpDown nudSwipeRecordIndex;

		// Token: 0x040001E2 RID: 482
		internal global::System.Windows.Forms.NumericUpDown numericUpDown2;
	}
}
