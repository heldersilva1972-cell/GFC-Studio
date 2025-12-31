namespace WG3000_COMM
{
	// Token: 0x02000229 RID: 553
	public partial class dfrmDbCompact : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x00123EB0 File Offset: 0x00122EB0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00123ED0 File Offset: 0x00122ED0
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.dfrmDbCompact));
			this.cmdCompactDatabase = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.txtDirectory = new global::System.Windows.Forms.TextBox();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.backUpToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveFileDialog1 = new global::System.Windows.Forms.SaveFileDialog();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cmdCompactDatabase, "cmdCompactDatabase");
			this.cmdCompactDatabase.BackColor = global::System.Drawing.Color.Transparent;
			this.cmdCompactDatabase.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.cmdCompactDatabase.ForeColor = global::System.Drawing.Color.White;
			this.cmdCompactDatabase.Name = "cmdCompactDatabase";
			this.cmdCompactDatabase.UseVisualStyleBackColor = false;
			this.cmdCompactDatabase.Click += new global::System.EventHandler(this.cmdCompactDatabase_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.txtDirectory, "txtDirectory");
			this.txtDirectory.Name = "txtDirectory";
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.backUpToolStripMenuItem, this.restoreToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.backUpToolStripMenuItem, "backUpToolStripMenuItem");
			this.backUpToolStripMenuItem.Name = "backUpToolStripMenuItem";
			this.backUpToolStripMenuItem.Click += new global::System.EventHandler(this.backUpToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreToolStripMenuItem, "restoreToolStripMenuItem");
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			this.restoreToolStripMenuItem.Click += new global::System.EventHandler(this.restoreToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
			this.openFileDialog1.FileName = "openFileDialog1";
			componentResourceManager.ApplyResources(this.openFileDialog1, "openFileDialog1");
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.txtDirectory);
			base.Controls.Add(this.cmdCompactDatabase);
			base.Controls.Add(this.btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmDbCompact";
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Hide;
			base.Load += new global::System.EventHandler(this.dfrmDbCompact_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmDbCompact_KeyDown);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04001CC3 RID: 7363
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04001CC4 RID: 7364
		private global::System.Windows.Forms.ToolStripMenuItem backUpToolStripMenuItem;

		// Token: 0x04001CC5 RID: 7365
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04001CC6 RID: 7366
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04001CC7 RID: 7367
		private global::System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;

		// Token: 0x04001CC8 RID: 7368
		private global::System.Windows.Forms.SaveFileDialog saveFileDialog1;

		// Token: 0x04001CC9 RID: 7369
		private global::System.Windows.Forms.TextBox txtDirectory;

		// Token: 0x04001CCA RID: 7370
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04001CCB RID: 7371
		internal global::System.Windows.Forms.Button cmdCompactDatabase;
	}
}
