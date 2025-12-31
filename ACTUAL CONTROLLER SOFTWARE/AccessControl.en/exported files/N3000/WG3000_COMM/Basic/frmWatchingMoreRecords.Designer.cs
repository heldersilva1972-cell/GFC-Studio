namespace WG3000_COMM.Basic
{
	// Token: 0x02000058 RID: 88
	public partial class frmWatchingMoreRecords : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x000BCA1C File Offset: 0x000BBA1C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000BCA3C File Offset: 0x000BBA3C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.frmWatchingMoreRecords));
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem2 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enlargeInfoDisplayToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ReduceInfoDisplaytoolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.enlargeFontToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ReduceFontToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveDisplayStyleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.optionToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.rGOValidTimeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.defaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.flowLayoutPanel1 = new global::System.Windows.Forms.FlowLayoutPanel();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox6 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox6 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox1 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox2 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox2 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox3 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox3 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox4 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox4 = new global::System.Windows.Forms.RichTextBox();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.pictureBox5 = new global::System.Windows.Forms.PictureBox();
			this.richTextBox5 = new global::System.Windows.Forms.RichTextBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			this.groupBox2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
			this.groupBox3.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
			this.groupBox4.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
			this.groupBox5.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.toolStripMenuItem2, this.toolStripMenuItem3, this.toolStripMenuItem4, this.toolStripMenuItem5, this.toolStripMenuItem6, this.enlargeInfoDisplayToolStripMenuItem, this.ReduceInfoDisplaytoolStripMenuItem, this.enlargeFontToolStripMenuItem, this.ReduceFontToolStripMenuItem, this.saveDisplayStyleToolStripMenuItem,
				this.restoreDefaultToolStripMenuItem, this.optionToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.toolTip1.SetToolTip(this.contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
			componentResourceManager.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Click += new global::System.EventHandler(this.toolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Click += new global::System.EventHandler(this.toolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Click += new global::System.EventHandler(this.toolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Click += new global::System.EventHandler(this.toolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Click += new global::System.EventHandler(this.toolStripMenuItem2_Click);
			componentResourceManager.ApplyResources(this.enlargeInfoDisplayToolStripMenuItem, "enlargeInfoDisplayToolStripMenuItem");
			this.enlargeInfoDisplayToolStripMenuItem.Name = "enlargeInfoDisplayToolStripMenuItem";
			this.enlargeInfoDisplayToolStripMenuItem.Click += new global::System.EventHandler(this.enlargeInfoDisplayToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.ReduceInfoDisplaytoolStripMenuItem, "ReduceInfoDisplaytoolStripMenuItem");
			this.ReduceInfoDisplaytoolStripMenuItem.Name = "ReduceInfoDisplaytoolStripMenuItem";
			this.ReduceInfoDisplaytoolStripMenuItem.Click += new global::System.EventHandler(this.ReduceInfoDisplaytoolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.enlargeFontToolStripMenuItem, "enlargeFontToolStripMenuItem");
			this.enlargeFontToolStripMenuItem.Name = "enlargeFontToolStripMenuItem";
			this.enlargeFontToolStripMenuItem.Click += new global::System.EventHandler(this.enlargeFontToolStripMenuItem1_Click);
			componentResourceManager.ApplyResources(this.ReduceFontToolStripMenuItem, "ReduceFontToolStripMenuItem");
			this.ReduceFontToolStripMenuItem.Name = "ReduceFontToolStripMenuItem";
			this.ReduceFontToolStripMenuItem.Click += new global::System.EventHandler(this.ReduceFontToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.saveDisplayStyleToolStripMenuItem, "saveDisplayStyleToolStripMenuItem");
			this.saveDisplayStyleToolStripMenuItem.Name = "saveDisplayStyleToolStripMenuItem";
			this.saveDisplayStyleToolStripMenuItem.Click += new global::System.EventHandler(this.saveDisplayStyleToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.restoreDefaultToolStripMenuItem, "restoreDefaultToolStripMenuItem");
			this.restoreDefaultToolStripMenuItem.Name = "restoreDefaultToolStripMenuItem";
			this.restoreDefaultToolStripMenuItem.Click += new global::System.EventHandler(this.restoreDefaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.optionToolStripMenuItem, "optionToolStripMenuItem");
			this.optionToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.rGOValidTimeToolStripMenuItem, this.defaultToolStripMenuItem });
			this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
			componentResourceManager.ApplyResources(this.rGOValidTimeToolStripMenuItem, "rGOValidTimeToolStripMenuItem");
			this.rGOValidTimeToolStripMenuItem.Name = "rGOValidTimeToolStripMenuItem";
			this.rGOValidTimeToolStripMenuItem.Click += new global::System.EventHandler(this.rGOValidTimeToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.defaultToolStripMenuItem, "defaultToolStripMenuItem");
			this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			this.defaultToolStripMenuItem.Click += new global::System.EventHandler(this.defaultToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.groupBox6);
			this.flowLayoutPanel1.Controls.Add(this.groupBox1);
			this.flowLayoutPanel1.Controls.Add(this.groupBox2);
			this.flowLayoutPanel1.Controls.Add(this.groupBox3);
			this.flowLayoutPanel1.Controls.Add(this.groupBox4);
			this.flowLayoutPanel1.Controls.Add(this.groupBox5);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.toolTip1.SetToolTip(this.flowLayoutPanel1, componentResourceManager.GetString("flowLayoutPanel1.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Controls.Add(this.pictureBox6);
			this.groupBox6.Controls.Add(this.richTextBox6);
			this.groupBox6.ForeColor = global::System.Drawing.Color.White;
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox6, componentResourceManager.GetString("groupBox6.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox6, "pictureBox6");
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox6, componentResourceManager.GetString("pictureBox6.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox6, "richTextBox6");
			this.richTextBox6.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox6.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox6.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox6.Name = "richTextBox6";
			this.toolTip1.SetToolTip(this.richTextBox6, componentResourceManager.GetString("richTextBox6.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.richTextBox1);
			this.groupBox1.ForeColor = global::System.Drawing.Color.White;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox1, componentResourceManager.GetString("pictureBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox1.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox1.Name = "richTextBox1";
			this.toolTip1.SetToolTip(this.richTextBox1, componentResourceManager.GetString("richTextBox1.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.pictureBox2);
			this.groupBox2.Controls.Add(this.richTextBox2);
			this.groupBox2.ForeColor = global::System.Drawing.Color.White;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox2, componentResourceManager.GetString("groupBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox2, "pictureBox2");
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox2, componentResourceManager.GetString("pictureBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox2, "richTextBox2");
			this.richTextBox2.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox2.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox2.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox2.Name = "richTextBox2";
			this.toolTip1.SetToolTip(this.richTextBox2, componentResourceManager.GetString("richTextBox2.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.pictureBox3);
			this.groupBox3.Controls.Add(this.richTextBox3);
			this.groupBox3.ForeColor = global::System.Drawing.Color.White;
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox3, componentResourceManager.GetString("groupBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox3, "pictureBox3");
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox3, componentResourceManager.GetString("pictureBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox3, "richTextBox3");
			this.richTextBox3.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox3.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox3.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox3.Name = "richTextBox3";
			this.toolTip1.SetToolTip(this.richTextBox3, componentResourceManager.GetString("richTextBox3.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.pictureBox4);
			this.groupBox4.Controls.Add(this.richTextBox4);
			this.groupBox4.ForeColor = global::System.Drawing.Color.White;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox4, componentResourceManager.GetString("groupBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox4, "pictureBox4");
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox4, componentResourceManager.GetString("pictureBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox4, "richTextBox4");
			this.richTextBox4.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox4.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox4.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox4.Name = "richTextBox4";
			this.toolTip1.SetToolTip(this.richTextBox4, componentResourceManager.GetString("richTextBox4.ToolTip"));
			componentResourceManager.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.pictureBox5);
			this.groupBox5.Controls.Add(this.richTextBox5);
			this.groupBox5.ForeColor = global::System.Drawing.Color.White;
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox5, componentResourceManager.GetString("groupBox5.ToolTip"));
			componentResourceManager.ApplyResources(this.pictureBox5, "pictureBox5");
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBox5, componentResourceManager.GetString("pictureBox5.ToolTip"));
			componentResourceManager.ApplyResources(this.richTextBox5, "richTextBox5");
			this.richTextBox5.BackColor = global::System.Drawing.Color.FromArgb(128, 131, 156);
			this.richTextBox5.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.richTextBox5.ForeColor = global::System.Drawing.Color.White;
			this.richTextBox5.Name = "richTextBox5";
			this.toolTip1.SetToolTip(this.richTextBox5, componentResourceManager.GetString("richTextBox5.ToolTip"));
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.flowLayoutPanel1);
			base.MinimizeBox = false;
			base.Name = "frmWatchingMoreRecords";
			this.toolTip1.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmWatchingMoreRecords_FormClosing);
			base.Load += new global::System.EventHandler(this.frmWatchingMoreRecords_Load);
			base.SizeChanged += new global::System.EventHandler(this.frmWatchingMoreRecords_SizeChanged);
			this.contextMenuStrip1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
			this.groupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			this.groupBox2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
			this.groupBox3.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
			this.groupBox4.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
			this.groupBox5.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
			base.ResumeLayout(false);
		}

		// Token: 0x04000C68 RID: 3176
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000C69 RID: 3177
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000C6A RID: 3178
		private global::System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;

		// Token: 0x04000C6B RID: 3179
		private global::System.Windows.Forms.ToolStripMenuItem enlargeFontToolStripMenuItem;

		// Token: 0x04000C6C RID: 3180
		private global::System.Windows.Forms.ToolStripMenuItem enlargeInfoDisplayToolStripMenuItem;

		// Token: 0x04000C6D RID: 3181
		private global::System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

		// Token: 0x04000C6E RID: 3182
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000C6F RID: 3183
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000C70 RID: 3184
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000C71 RID: 3185
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000C72 RID: 3186
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x04000C73 RID: 3187
		private global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x04000C74 RID: 3188
		private global::System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;

		// Token: 0x04000C75 RID: 3189
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000C76 RID: 3190
		private global::System.Windows.Forms.PictureBox pictureBox2;

		// Token: 0x04000C77 RID: 3191
		private global::System.Windows.Forms.PictureBox pictureBox3;

		// Token: 0x04000C78 RID: 3192
		private global::System.Windows.Forms.PictureBox pictureBox4;

		// Token: 0x04000C79 RID: 3193
		private global::System.Windows.Forms.PictureBox pictureBox5;

		// Token: 0x04000C7A RID: 3194
		private global::System.Windows.Forms.PictureBox pictureBox6;

		// Token: 0x04000C7B RID: 3195
		private global::System.Windows.Forms.ToolStripMenuItem ReduceFontToolStripMenuItem;

		// Token: 0x04000C7C RID: 3196
		private global::System.Windows.Forms.ToolStripMenuItem ReduceInfoDisplaytoolStripMenuItem;

		// Token: 0x04000C7D RID: 3197
		private global::System.Windows.Forms.ToolStripMenuItem restoreDefaultToolStripMenuItem;

		// Token: 0x04000C7E RID: 3198
		private global::System.Windows.Forms.ToolStripMenuItem rGOValidTimeToolStripMenuItem;

		// Token: 0x04000C7F RID: 3199
		private global::System.Windows.Forms.RichTextBox richTextBox1;

		// Token: 0x04000C80 RID: 3200
		private global::System.Windows.Forms.RichTextBox richTextBox2;

		// Token: 0x04000C81 RID: 3201
		private global::System.Windows.Forms.RichTextBox richTextBox3;

		// Token: 0x04000C82 RID: 3202
		private global::System.Windows.Forms.RichTextBox richTextBox4;

		// Token: 0x04000C83 RID: 3203
		private global::System.Windows.Forms.RichTextBox richTextBox5;

		// Token: 0x04000C84 RID: 3204
		private global::System.Windows.Forms.RichTextBox richTextBox6;

		// Token: 0x04000C85 RID: 3205
		private global::System.Windows.Forms.ToolStripMenuItem saveDisplayStyleToolStripMenuItem;

		// Token: 0x04000C86 RID: 3206
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000C87 RID: 3207
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

		// Token: 0x04000C88 RID: 3208
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;

		// Token: 0x04000C89 RID: 3209
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;

		// Token: 0x04000C8A RID: 3210
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;

		// Token: 0x04000C8B RID: 3211
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;

		// Token: 0x04000C8C RID: 3212
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
