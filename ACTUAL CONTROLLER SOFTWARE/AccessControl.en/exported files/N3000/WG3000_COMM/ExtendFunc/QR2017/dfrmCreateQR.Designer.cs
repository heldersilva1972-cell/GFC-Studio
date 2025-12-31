namespace WG3000_COMM.ExtendFunc.QR2017
{
	// Token: 0x0200031F RID: 799
	public partial class dfrmCreateQR : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060018F6 RID: 6390 RVA: 0x00209BAC File Offset: 0x00208BAC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00209BCC File Offset: 0x00208BCC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.QR2017.dfrmCreateQR));
			this.printDocument1 = new global::System.Drawing.Printing.PrintDocument();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.scaleToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.valueToolStripMenuItem = new global::System.Windows.Forms.ToolStripTextBox();
			this.saveAsFileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.batchCreateFilesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setDefaultValidTimeHoursToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripValidTimeHours = new global::System.Windows.Forms.ToolStripTextBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.stbRunInfo = new global::System.Windows.Forms.StatusStrip();
			this.statRuninfo1 = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statTimeDate = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.btnCopy = new global::System.Windows.Forms.Button();
			this.btnPrint = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.grpbAccessControl = new global::System.Windows.Forms.GroupBox();
			this.lblNote = new global::System.Windows.Forms.Label();
			this.btnOneDay = new global::System.Windows.Forms.Button();
			this.btnOneYear = new global::System.Windows.Forms.Button();
			this.btnCurrentYear = new global::System.Windows.Forms.Button();
			this.btnOneMonth = new global::System.Windows.Forms.Button();
			this.btnOneWeek = new global::System.Windows.Forms.Button();
			this.btnCurrentDay = new global::System.Windows.Forms.Button();
			this.btnOneHour = new global::System.Windows.Forms.Button();
			this.btnHalfHour = new global::System.Windows.Forms.Button();
			this.dateBeginHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dateEndHMS1 = new global::System.Windows.Forms.DateTimePicker();
			this.dtpDeactivate = new global::System.Windows.Forms.DateTimePicker();
			this.dtpActivate = new global::System.Windows.Forms.DateTimePicker();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.textBoxText = new global::System.Windows.Forms.TextBox();
			this.btnCreateQR = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.txtInfo = new global::System.Windows.Forms.TextBox();
			this.lblUser = new global::System.Windows.Forms.Label();
			this.pictureBoxQr = new global::System.Windows.Forms.PictureBox();
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			this.stbRunInfo.SuspendLayout();
			this.grpbAccessControl.SuspendLayout();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxQr).BeginInit();
			base.SuspendLayout();
			this.printDocument1.PrintPage += new global::System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.scaleToolStripMenuItem, this.saveAsFileToolStripMenuItem, this.batchCreateFilesToolStripMenuItem, this.setDefaultValidTimeHoursToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.scaleToolStripMenuItem, "scaleToolStripMenuItem");
			this.scaleToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.valueToolStripMenuItem });
			this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
			componentResourceManager.ApplyResources(this.valueToolStripMenuItem, "valueToolStripMenuItem");
			this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
			this.valueToolStripMenuItem.TextChanged += new global::System.EventHandler(this.valueToolStripMenuItem_TextChanged);
			componentResourceManager.ApplyResources(this.saveAsFileToolStripMenuItem, "saveAsFileToolStripMenuItem");
			this.saveAsFileToolStripMenuItem.Name = "saveAsFileToolStripMenuItem";
			this.saveAsFileToolStripMenuItem.Click += new global::System.EventHandler(this.saveAsFileToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.batchCreateFilesToolStripMenuItem, "batchCreateFilesToolStripMenuItem");
			this.batchCreateFilesToolStripMenuItem.Name = "batchCreateFilesToolStripMenuItem";
			this.batchCreateFilesToolStripMenuItem.Click += new global::System.EventHandler(this.batchCreateFilesToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.setDefaultValidTimeHoursToolStripMenuItem, "setDefaultValidTimeHoursToolStripMenuItem");
			this.setDefaultValidTimeHoursToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.toolStripValidTimeHours });
			this.setDefaultValidTimeHoursToolStripMenuItem.Name = "setDefaultValidTimeHoursToolStripMenuItem";
			componentResourceManager.ApplyResources(this.toolStripValidTimeHours, "toolStripValidTimeHours");
			this.toolStripValidTimeHours.Name = "toolStripValidTimeHours";
			this.toolStripValidTimeHours.TextChanged += new global::System.EventHandler(this.toolStripValidTimeHours_TextChanged);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			componentResourceManager.ApplyResources(this.stbRunInfo, "stbRunInfo");
			this.stbRunInfo.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_bottom;
			this.stbRunInfo.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.statRuninfo1, this.statTimeDate });
			this.stbRunInfo.Name = "stbRunInfo";
			componentResourceManager.ApplyResources(this.statRuninfo1, "statRuninfo1");
			this.statRuninfo1.BackColor = global::System.Drawing.Color.Transparent;
			this.statRuninfo1.ForeColor = global::System.Drawing.Color.White;
			this.statRuninfo1.Name = "statRuninfo1";
			this.statRuninfo1.Spring = true;
			componentResourceManager.ApplyResources(this.statTimeDate, "statTimeDate");
			this.statTimeDate.BackColor = global::System.Drawing.Color.Transparent;
			this.statTimeDate.ForeColor = global::System.Drawing.Color.White;
			this.statTimeDate.Image = global::WG3000_COMM.Properties.Resources.timequery;
			this.statTimeDate.Name = "statTimeDate";
			componentResourceManager.ApplyResources(this.btnCopy, "btnCopy");
			this.btnCopy.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCopy.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCopy.ForeColor = global::System.Drawing.Color.White;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.UseVisualStyleBackColor = false;
			this.btnCopy.Click += new global::System.EventHandler(this.btnCopy_Click);
			componentResourceManager.ApplyResources(this.btnPrint, "btnPrint");
			this.btnPrint.BackColor = global::System.Drawing.Color.Transparent;
			this.btnPrint.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnPrint.ForeColor = global::System.Drawing.Color.White;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.UseVisualStyleBackColor = false;
			this.btnPrint.Click += new global::System.EventHandler(this.btnPrint_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.grpbAccessControl, "grpbAccessControl");
			this.grpbAccessControl.Controls.Add(this.lblNote);
			this.grpbAccessControl.Controls.Add(this.btnOneDay);
			this.grpbAccessControl.Controls.Add(this.btnOneYear);
			this.grpbAccessControl.Controls.Add(this.btnCurrentYear);
			this.grpbAccessControl.Controls.Add(this.btnOneMonth);
			this.grpbAccessControl.Controls.Add(this.btnOneWeek);
			this.grpbAccessControl.Controls.Add(this.btnCurrentDay);
			this.grpbAccessControl.Controls.Add(this.btnOneHour);
			this.grpbAccessControl.Controls.Add(this.btnHalfHour);
			this.grpbAccessControl.Controls.Add(this.dateBeginHMS1);
			this.grpbAccessControl.Controls.Add(this.dateEndHMS1);
			this.grpbAccessControl.Controls.Add(this.dtpDeactivate);
			this.grpbAccessControl.Controls.Add(this.dtpActivate);
			this.grpbAccessControl.Controls.Add(this.label5);
			this.grpbAccessControl.Controls.Add(this.label6);
			this.grpbAccessControl.Name = "grpbAccessControl";
			this.grpbAccessControl.TabStop = false;
			componentResourceManager.ApplyResources(this.lblNote, "lblNote");
			this.lblNote.ForeColor = global::System.Drawing.Color.White;
			this.lblNote.Name = "lblNote";
			componentResourceManager.ApplyResources(this.btnOneDay, "btnOneDay");
			this.btnOneDay.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOneDay.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOneDay.ForeColor = global::System.Drawing.Color.White;
			this.btnOneDay.Name = "btnOneDay";
			this.btnOneDay.UseVisualStyleBackColor = false;
			this.btnOneDay.Click += new global::System.EventHandler(this.btnOneDay_Click);
			componentResourceManager.ApplyResources(this.btnOneYear, "btnOneYear");
			this.btnOneYear.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOneYear.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOneYear.ForeColor = global::System.Drawing.Color.White;
			this.btnOneYear.Name = "btnOneYear";
			this.btnOneYear.UseVisualStyleBackColor = false;
			this.btnOneYear.Click += new global::System.EventHandler(this.btnOneYear_Click);
			componentResourceManager.ApplyResources(this.btnCurrentYear, "btnCurrentYear");
			this.btnCurrentYear.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCurrentYear.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCurrentYear.ForeColor = global::System.Drawing.Color.White;
			this.btnCurrentYear.Name = "btnCurrentYear";
			this.btnCurrentYear.UseVisualStyleBackColor = false;
			this.btnCurrentYear.Click += new global::System.EventHandler(this.btnCurrentYear_Click);
			componentResourceManager.ApplyResources(this.btnOneMonth, "btnOneMonth");
			this.btnOneMonth.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOneMonth.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOneMonth.ForeColor = global::System.Drawing.Color.White;
			this.btnOneMonth.Name = "btnOneMonth";
			this.btnOneMonth.UseVisualStyleBackColor = false;
			this.btnOneMonth.Click += new global::System.EventHandler(this.btnOneMonth_Click);
			componentResourceManager.ApplyResources(this.btnOneWeek, "btnOneWeek");
			this.btnOneWeek.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOneWeek.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOneWeek.ForeColor = global::System.Drawing.Color.White;
			this.btnOneWeek.Name = "btnOneWeek";
			this.btnOneWeek.UseVisualStyleBackColor = false;
			this.btnOneWeek.Click += new global::System.EventHandler(this.btnOneWeek_Click);
			componentResourceManager.ApplyResources(this.btnCurrentDay, "btnCurrentDay");
			this.btnCurrentDay.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCurrentDay.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCurrentDay.ForeColor = global::System.Drawing.Color.White;
			this.btnCurrentDay.Name = "btnCurrentDay";
			this.btnCurrentDay.UseVisualStyleBackColor = false;
			this.btnCurrentDay.Click += new global::System.EventHandler(this.btnCurrentDay_Click);
			componentResourceManager.ApplyResources(this.btnOneHour, "btnOneHour");
			this.btnOneHour.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOneHour.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOneHour.ForeColor = global::System.Drawing.Color.White;
			this.btnOneHour.Name = "btnOneHour";
			this.btnOneHour.UseVisualStyleBackColor = false;
			this.btnOneHour.Click += new global::System.EventHandler(this.btnOneHour_Click);
			componentResourceManager.ApplyResources(this.btnHalfHour, "btnHalfHour");
			this.btnHalfHour.BackColor = global::System.Drawing.Color.Transparent;
			this.btnHalfHour.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnHalfHour.ForeColor = global::System.Drawing.Color.White;
			this.btnHalfHour.Name = "btnHalfHour";
			this.btnHalfHour.UseVisualStyleBackColor = false;
			this.btnHalfHour.Click += new global::System.EventHandler(this.btnHalfHour_Click);
			componentResourceManager.ApplyResources(this.dateBeginHMS1, "dateBeginHMS1");
			this.dateBeginHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateBeginHMS1.Name = "dateBeginHMS1";
			this.dateBeginHMS1.ShowUpDown = true;
			this.dateBeginHMS1.Value = new global::System.DateTime(2016, 9, 2, 0, 0, 0, 0);
			componentResourceManager.ApplyResources(this.dateEndHMS1, "dateEndHMS1");
			this.dateEndHMS1.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateEndHMS1.Name = "dateEndHMS1";
			this.dateEndHMS1.ShowUpDown = true;
			this.dateEndHMS1.Value = new global::System.DateTime(2016, 9, 2, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dtpDeactivate, "dtpDeactivate");
			this.dtpDeactivate.Name = "dtpDeactivate";
			this.dtpDeactivate.Value = new global::System.DateTime(2099, 12, 31, 23, 59, 0, 0);
			componentResourceManager.ApplyResources(this.dtpActivate, "dtpActivate");
			this.dtpActivate.Name = "dtpActivate";
			this.dtpActivate.Value = new global::System.DateTime(2010, 1, 1, 18, 18, 0, 0);
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = global::System.Drawing.Color.White;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.textBoxText, "textBoxText");
			this.textBoxText.Name = "textBoxText";
			componentResourceManager.ApplyResources(this.btnCreateQR, "btnCreateQR");
			this.btnCreateQR.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCreateQR.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCreateQR.ForeColor = global::System.Drawing.Color.White;
			this.btnCreateQR.Name = "btnCreateQR";
			this.btnCreateQR.UseVisualStyleBackColor = false;
			this.btnCreateQR.Click += new global::System.EventHandler(this.btnCreateQR_Click);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = global::System.Drawing.Color.White;
			this.panel1.ContextMenuStrip = this.contextMenuStrip1;
			this.panel1.Controls.Add(this.txtInfo);
			this.panel1.Controls.Add(this.lblUser);
			this.panel1.Controls.Add(this.pictureBoxQr);
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.txtInfo, "txtInfo");
			this.txtInfo.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.txtInfo.Name = "txtInfo";
			componentResourceManager.ApplyResources(this.lblUser, "lblUser");
			this.lblUser.BackColor = global::System.Drawing.Color.White;
			this.lblUser.Name = "lblUser";
			componentResourceManager.ApplyResources(this.pictureBoxQr, "pictureBoxQr");
			this.pictureBoxQr.BackColor = global::System.Drawing.Color.White;
			this.pictureBoxQr.Name = "pictureBoxQr";
			this.pictureBoxQr.TabStop = false;
			componentResourceManager.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.BackColor = global::System.Drawing.Color.White;
			this.lblInfo.Name = "lblInfo";
			componentResourceManager.ApplyResources(this, "$this");
			this.ContextMenuStrip = this.contextMenuStrip1;
			base.Controls.Add(this.stbRunInfo);
			base.Controls.Add(this.lblInfo);
			base.Controls.Add(this.btnCopy);
			base.Controls.Add(this.btnPrint);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.grpbAccessControl);
			base.Controls.Add(this.textBoxText);
			base.Controls.Add(this.btnCreateQR);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmCreateQR";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmCreateQR_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmCreateQR_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.stbRunInfo.ResumeLayout(false);
			this.stbRunInfo.PerformLayout();
			this.grpbAccessControl.ResumeLayout(false);
			this.grpbAccessControl.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxQr).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040032C9 RID: 13001
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040032CA RID: 13002
		private global::System.Windows.Forms.ToolStripMenuItem batchCreateFilesToolStripMenuItem;

		// Token: 0x040032CB RID: 13003
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040032CC RID: 13004
		private global::System.Windows.Forms.Button btnCopy;

		// Token: 0x040032CD RID: 13005
		private global::System.Windows.Forms.Button btnCreateQR;

		// Token: 0x040032CE RID: 13006
		private global::System.Windows.Forms.Button btnCurrentDay;

		// Token: 0x040032CF RID: 13007
		private global::System.Windows.Forms.Button btnCurrentYear;

		// Token: 0x040032D0 RID: 13008
		private global::System.Windows.Forms.Button btnHalfHour;

		// Token: 0x040032D1 RID: 13009
		private global::System.Windows.Forms.Button btnOneDay;

		// Token: 0x040032D2 RID: 13010
		private global::System.Windows.Forms.Button btnOneHour;

		// Token: 0x040032D3 RID: 13011
		private global::System.Windows.Forms.Button btnOneMonth;

		// Token: 0x040032D4 RID: 13012
		private global::System.Windows.Forms.Button btnOneWeek;

		// Token: 0x040032D5 RID: 13013
		private global::System.Windows.Forms.Button btnOneYear;

		// Token: 0x040032D6 RID: 13014
		private global::System.Windows.Forms.Button btnPrint;

		// Token: 0x040032D7 RID: 13015
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x040032D8 RID: 13016
		private global::System.Windows.Forms.GroupBox grpbAccessControl;

		// Token: 0x040032D9 RID: 13017
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040032DA RID: 13018
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040032DB RID: 13019
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x040032DC RID: 13020
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040032DD RID: 13021
		private global::System.Windows.Forms.PictureBox pictureBoxQr;

		// Token: 0x040032DE RID: 13022
		private global::System.Drawing.Printing.PrintDocument printDocument1;

		// Token: 0x040032DF RID: 13023
		private global::System.Windows.Forms.ToolStripMenuItem saveAsFileToolStripMenuItem;

		// Token: 0x040032E0 RID: 13024
		private global::System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;

		// Token: 0x040032E1 RID: 13025
		private global::System.Windows.Forms.ToolStripMenuItem setDefaultValidTimeHoursToolStripMenuItem;

		// Token: 0x040032E2 RID: 13026
		private global::System.Windows.Forms.ToolStripStatusLabel statRuninfo1;

		// Token: 0x040032E3 RID: 13027
		private global::System.Windows.Forms.ToolStripStatusLabel statTimeDate;

		// Token: 0x040032E4 RID: 13028
		private global::System.Windows.Forms.StatusStrip stbRunInfo;

		// Token: 0x040032E5 RID: 13029
		private global::System.Windows.Forms.TextBox textBoxText;

		// Token: 0x040032E6 RID: 13030
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040032E7 RID: 13031
		private global::System.Windows.Forms.ToolStripTextBox toolStripValidTimeHours;

		// Token: 0x040032E8 RID: 13032
		private global::System.Windows.Forms.TextBox txtInfo;

		// Token: 0x040032E9 RID: 13033
		private global::System.Windows.Forms.ToolStripTextBox valueToolStripMenuItem;

		// Token: 0x040032EA RID: 13034
		public global::System.Windows.Forms.DateTimePicker dateBeginHMS1;

		// Token: 0x040032EB RID: 13035
		public global::System.Windows.Forms.DateTimePicker dateEndHMS1;

		// Token: 0x040032EC RID: 13036
		public global::System.Windows.Forms.DateTimePicker dtpActivate;

		// Token: 0x040032ED RID: 13037
		public global::System.Windows.Forms.DateTimePicker dtpDeactivate;

		// Token: 0x040032EE RID: 13038
		public global::System.Windows.Forms.Label lblNote;

		// Token: 0x040032EF RID: 13039
		public global::System.Windows.Forms.Label lblUser;
	}
}
