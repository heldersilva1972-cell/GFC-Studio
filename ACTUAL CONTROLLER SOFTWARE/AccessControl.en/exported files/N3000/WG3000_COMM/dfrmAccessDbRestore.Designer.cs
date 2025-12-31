namespace WG3000_COMM
{
	// Token: 0x02000227 RID: 551
	public partial class dfrmAccessDbRestore : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001019 RID: 4121 RVA: 0x0012093A File Offset: 0x0011F93A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0012095C File Offset: 0x0011F95C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.dfrmAccessDbRestore));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.backUpToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveFileDialog1 = new global::System.Windows.Forms.SaveFileDialog();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.Label3 = new global::System.Windows.Forms.Label();
			this.txtBakFile = new global::System.Windows.Forms.TextBox();
			this.cmdRestoreDatabase = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.backUpToolStripMenuItem, this.restoreToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.backUpToolStripMenuItem, "backUpToolStripMenuItem");
			this.backUpToolStripMenuItem.Name = "backUpToolStripMenuItem";
			componentResourceManager.ApplyResources(this.restoreToolStripMenuItem, "restoreToolStripMenuItem");
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			componentResourceManager.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			componentResourceManager.ApplyResources(this.button2, "button2");
			this.button2.BackColor = global::System.Drawing.Color.Transparent;
			this.button2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button2.ForeColor = global::System.Drawing.Color.White;
			this.button2.Name = "button2";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			componentResourceManager.ApplyResources(this.Label3, "Label3");
			this.Label3.ForeColor = global::System.Drawing.Color.White;
			this.Label3.Name = "Label3";
			componentResourceManager.ApplyResources(this.txtBakFile, "txtBakFile");
			this.txtBakFile.Name = "txtBakFile";
			componentResourceManager.ApplyResources(this.cmdRestoreDatabase, "cmdRestoreDatabase");
			this.cmdRestoreDatabase.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdRestoreDatabase.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdRestoreDatabase.ForeColor = global::System.Drawing.Color.White;
			this.cmdRestoreDatabase.Name = "cmdRestoreDatabase";
			this.cmdRestoreDatabase.UseVisualStyleBackColor = false;
			this.cmdRestoreDatabase.Click += new global::System.EventHandler(this.cmdCompactDatabase_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.Label3);
			base.Controls.Add(this.txtBakFile);
			base.Controls.Add(this.cmdRestoreDatabase);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmAccessDbRestore";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.Load += new global::System.EventHandler(this.dfrmDbCompact_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001C8E RID: 7310
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001C8F RID: 7311
		private global::System.Windows.Forms.ToolStripMenuItem backUpToolStripMenuItem;

		// Token: 0x04001C90 RID: 7312
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04001C91 RID: 7313
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04001C92 RID: 7314
		private global::System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;

		// Token: 0x04001C93 RID: 7315
		private global::System.Windows.Forms.SaveFileDialog saveFileDialog1;

		// Token: 0x04001C94 RID: 7316
		private global::System.Windows.Forms.TextBox txtBakFile;

		// Token: 0x04001C95 RID: 7317
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001C96 RID: 7318
		internal global::System.Windows.Forms.Button button1;

		// Token: 0x04001C97 RID: 7319
		internal global::System.Windows.Forms.Button button2;

		// Token: 0x04001C98 RID: 7320
		internal global::System.Windows.Forms.Button cmdRestoreDatabase;

		// Token: 0x04001C99 RID: 7321
		internal global::System.Windows.Forms.Label Label3;
	}
}
