using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM
{
	// Token: 0x0200022A RID: 554
	public partial class dfrmPhotoAvi : frmN3000
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x00124208 File Offset: 0x00123208
		public dfrmPhotoAvi()
		{
			this.InitializeComponent();
			Color keyColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabP4PanelA.BackColor = keyColor;
			this.richTxtCardInfo.BackColor = keyColor;
			this.TabPage1.BackColor = keyColor;
			this.TabPage2.BackColor = keyColor;
			try
			{
				string systemParamByNO = wgAppConfig.getSystemParamByNO(44);
				if (!string.IsNullOrEmpty(systemParamByNO))
				{
					this.aviTimelength = int.Parse(systemParamByNO);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00124394 File Offset: 0x00123394
		private void _loadPhoto()
		{
			this.picPhoto.Visible = false;
			try
			{
				string photoFileName = wgAppConfig.getPhotoFileName(this.newCardNo);
				Image image = this.picPhoto.Image;
				wgAppConfig.ShowMyImage(photoFileName, ref image);
				if (image != null)
				{
					this.picPhoto.Image = image;
					this.picPhoto.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0012440C File Offset: 0x0012340C
		private void activeDisplay4WarnEventToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_Display4WarnEvent", "1");
			this.activeDisplay4WarnEventToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "1";
			this.deactiveDisplay4WarnEventToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "0";
			if (this.frmCaller.GetType().Name == "frmConsole")
			{
				(this.frmCaller as frmConsole).activateWarnAvi = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "1";
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x001244A8 File Offset: 0x001234A8
		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				if (!this.bWatching)
				{
					base.Hide();
					this.Timer1.Enabled = false;
					base.Close();
				}
				else
				{
					base.Hide();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00124500 File Offset: 0x00123500
		private void btnCloseW_Click(object sender, EventArgs e)
		{
			this.btnClose.PerformClick();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0012450D File Offset: 0x0012350D
		private void btnEnd_Click(object sender, EventArgs e)
		{
			this.indexArr = this.arrFileName.Count - 1;
			this.getChangedFile();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00124528 File Offset: 0x00123528
		private void btnFirst_Click(object sender, EventArgs e)
		{
			this.indexArr = 0;
			this.getChangedFile();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00124537 File Offset: 0x00123537
		private void btnFullVideo_Click(object sender, EventArgs e)
		{
			this.picJpg_DoubleClick(null, null);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00124544 File Offset: 0x00123544
		private void btnNext_Click(object sender, EventArgs e)
		{
			if (this.bWatching)
			{
				this.indexArr++;
				this.getChangedFile();
				return;
			}
			try
			{
				if (this.frmCaller is frmSwipeRecords)
				{
					((frmSwipeRecords)this.frmCaller).nextRecord();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x001245A0 File Offset: 0x001235A0
		private void btnPrev_Click(object sender, EventArgs e)
		{
			if (this.bWatching)
			{
				this.indexArr--;
				this.getChangedFile();
				return;
			}
			try
			{
				if (this.frmCaller is frmSwipeRecords)
				{
					((frmSwipeRecords)this.frmCaller).prevRecord();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x001245FC File Offset: 0x001235FC
		private void btnRefreshAVI_Click(object sender, EventArgs e)
		{
			try
			{
				this.updateHIKVideo(false);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00124634 File Offset: 0x00123634
		public bool CaptureNewCardRecord(int readerID, string RecordAll, ref bool bToCapturePhoto)
		{
			bool flag = false;
			try
			{
				flag = this.hkVideo4contr.CaptureNewCardRecordMultiThread(readerID, RecordAll, ref bToCapturePhoto);
				if (flag)
				{
					this.waitTime = (long)Conversion.Int(3500 / this.Timer1.Interval);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
			return flag;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00124694 File Offset: 0x00123694
		private void deactiveDisplay4WarnEventToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_Display4WarnEvent", "0");
			this.activeDisplay4WarnEventToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "1";
			this.deactiveDisplay4WarnEventToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") != "1";
			if (this.frmCaller.GetType().Name == "frmConsole")
			{
				(this.frmCaller as frmConsole).activateWarnAvi = wgAppConfig.GetKeyVal("KEY_Display4WarnEvent") == "1";
			}
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0012472E File Offset: 0x0012372E
		private void dfrmPhotoAvi_Closed(object sender, EventArgs e)
		{
			this.stopVideo();
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00124738 File Offset: 0x00123738
		private void dfrmPhotoAvi_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				if (this.bWatching)
				{
					base.Hide();
					e.Cancel = true;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0012477C File Offset: 0x0012377C
		private void dfrmPhotoAvi_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (this.frmCaller != null && this.frmCaller is frmConsole && ((frmConsole)this.frmCaller).frm4PCCheckAccess != null && ((frmConsole)this.frmCaller).frm4PCCheckAccess.Visible)
				{
					((frmConsole)this.frmCaller).frm4PCCheckAccess.dfrmPCCheckAccess_OutActive(null, e);
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00124800 File Offset: 0x00123800
		private void dfrmPhotoAvi_Load(object sender, EventArgs e)
		{
			this.hideToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_HIDE_PHOTOAVI") == "1";
			this.bHide = wgAppConfig.GetKeyVal("KEY_HIDE_PHOTOAVI") == "1" && this.frmCaller.GetType().Name == "frmConsole";
			if (this.bHide)
			{
				base.Hide();
			}
			int num = 0;
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(172)), out num);
			if (num == 1 && this.bWatching)
			{
				base.Close();
			}
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)), out this.onlyCapturePhoto);
			try
			{
				if (!(wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)) == "1") && !(wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(157)) == "0"))
				{
					this.TabControl1.Controls.Remove(this.TabPage2);
					this.TabPage2.Dispose();
				}
				if (!this.bWatching)
				{
					this.txtCardInfo.Text = "";
					this.richTxtCardInfo.Text = "";
				}
				this.txtCardInfo.BackColor = Color.White;
				this.richTxtCardInfo.BackColor = Color.White;
				if (this.bWatching)
				{
					this.txtCardInfo.ForeColor = Color.White;
					this.richTxtCardInfo.ForeColor = Color.White;
				}
				this.Timer3.Enabled = true;
				base.KeyPreview = true;
				if (this.bWatching)
				{
					this.hkVideo4contr = new comHIKVideo();
					if (this.selectedCameraID != "")
					{
						int num2 = this.selectedCameraID.Length - this.selectedCameraID.Replace(",", "").Length + 1;
						if (num2 <= this.PanelMaxONOneTABPAGE)
						{
							this.TabControl2.TabPages[0].Text = "";
							this.TabControl2.ItemSize = new Size(new Point(1, 1));
						}
						else
						{
							this.TabControl2.TabPages[0].Text = "1";
						}
						this.TabControl2.TabPages[0].Controls.Clear();
						for (int i = 0; i < num2; i += this.PanelMaxONOneTABPAGE)
						{
							if (i == 0)
							{
								this.TabPageCamera = this.TabControl2.TabPages[0];
							}
							else
							{
								this.TabPageCamera = new TabPage();
								this.TabPageCamera.Text = (i / this.PanelMaxONOneTABPAGE + 1).ToString();
								this.TabPageCamera.BorderStyle = BorderStyle.Fixed3D;
							}
							Point[] array = new Point[]
							{
								new Point(18, 0),
								new Point(344, 0),
								new Point(670, 0),
								new Point(18, 244),
								new Point(344, 244),
								new Point(670, 244)
							};
							for (int j = 1; j <= this.PanelMaxONOneTABPAGE; j++)
							{
								this.P4Player = new Panel();
								this.P4Player.Name = "P4Player" + (i + j);
								this.TabPageCamera.Controls.Add(this.P4Player);
								this.P4Player.Size = new Size(new Point(300, 240));
								this.P4Player.BackColor = Color.Black;
								this.P4Player.Location = array[j - 1];
								this.P4Player.Tag = i + j;
								this.lblNotConnect = new Label();
								this.P4Player.Controls.Add(this.lblNotConnect);
								this.lblNotConnect.Name = "lbl" + (i + j);
								this.lblNotConnect.Size = new Size(new Point(320, this.lblErr.Size.Height));
								this.lblNotConnect.Location = new Point(32, 24);
								this.lblNotConnect.Font = this.lblErr.Font;
								this.lblNotConnect.Text = this.lblErr.Text;
								this.lblNotConnect.ForeColor = Color.White;
								this.P4Player.Visible = false;
								this.lblNotConnect.Visible = false;
								this.P4Player.Click += this.tabP4PanelA_Click;
								this.P4Player.DoubleClick += this.P4Player_DoubleClick;
								this.hkVideo4contr.p4playersAppend(this.P4Player);
							}
							if (i != 0)
							{
								this.TabPageCamera.Click += this.tabP4PanelA_Click;
								this.TabPageCamera.SizeChanged += this.tabP4PanelA_SizeChanged;
								this.TabControl2.TabPages.Add(this.TabPageCamera);
							}
						}
						this.video_play(num2);
					}
					this.startSlowThread = new Thread(new ThreadStart(this.startSlow));
					this.startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
					this.startSlowThread.IsBackground = true;
					this.startSlowThread.Start();
				}
				else
				{
					this.btnPrev.Enabled = true;
					this.btnNext.Enabled = true;
					this.lblCount.Visible = true;
					this.tabP4PanelA.Visible = false;
					this.TimerStartSlow.Enabled = false;
					Cursor.Current = Cursors.Default;
				}
				if (!PlayCtrl.PlayM4_GetPort(ref this.USED_PORT))
				{
					wgAppConfig.wgDebugWrite(" m4_getport Failed ! Err= " + this.USED_PORT, EventLogEntryType.Error);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00124E6C File Offset: 0x00123E6C
		private void dfrmPhotoAvi_SizeChanged(object sender, EventArgs e)
		{
			if (!this.bGroup2)
			{
				this.bGroup2 = true;
				this.group2OldLocation = new Point(this.grpTop.Location.X, this.grpTop.Location.Y);
			}
			this.grpTop.Location = new Point(this.group2OldLocation.X + (base.Size.Width - 1024) / 2, this.group2OldLocation.Y);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00124EF8 File Offset: 0x00123EF8
		public void getChangedFile()
		{
			this.updateButtonState();
			this.txtCardInfo.Text = this.arrCardInfo[this.indexArr].ToString();
			this.richTxtCardInfo.Text = this.arrCardInfo[this.indexArr].ToString();
			this.richTxtCardInfo.ForeColor = SystemColors.WindowText;
			this.fileName = this.arrFileName[this.indexArr].ToString();
			long.TryParse(this.arrnewCardNo[this.indexArr].ToString(), out this.newCardNo);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00124F9B File Offset: 0x00123F9B
		private void grpCaptured_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00124FA0 File Offset: 0x00123FA0
		private void hideToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.hideToolStripMenuItem.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				wgAppConfig.UpdateKeyVal("KEY_HIDE_PHOTOAVI", "1");
				this.bHide = wgAppConfig.GetKeyVal("KEY_HIDE_PHOTOAVI") == "1" && this.frmCaller.GetType().Name == "frmConsole";
				this.hideToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_HIDE_PHOTOAVI") == "1";
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00125044 File Offset: 0x00124044
		public bool onevideo_play6()
		{
			try
			{
				if (this.selectedPanel == null)
				{
					return false;
				}
				Panel panel = this.selectedPanel;
				double num = 992.0;
				double num2 = 485.0;
				double num3 = 1.2222222222222223;
				Size size = this.TabControl2.SelectedTab.Size;
				double num5;
				double num6;
				Size size2;
				if ((double)size.Width / (double)size.Height >= num3)
				{
					double num4 = 1.0 * (double)size.Height / num2;
					num2 = (double)size.Height;
					num = (double)((int)((double)size.Width * num4));
					num5 = num2;
					num6 = (double)((int)(num5 * num3));
					size2 = new Size(new Point((int)num6, (int)num5));
					num = (double)size.Width;
				}
				else
				{
					double num4 = 1.0 * (double)size.Width / num;
					num = (double)size.Width;
					num2 = (double)((int)((double)size.Height * num4));
					num6 = num;
					num5 = (double)((int)(num6 / num3));
					size2 = new Size(new Point((int)num6, (int)num5));
					num2 = (double)size.Height;
				}
				int num7 = (size.Width - size2.Width) / 2;
				int num8 = (size.Height - size2.Height) / 2;
				this.setP4Wnd(panel, num7, num8, (int)num6, (int)num5);
				panel.Visible = true;
			}
			catch (Exception)
			{
			}
			return true;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x001251C8 File Offset: 0x001241C8
		public void onlyView()
		{
			try
			{
				this.prevFileName = this.fileName;
				this.txtCardInfo.Text = this.newCardInfo;
				this.richTxtCardInfo.Text = this.newCardInfo;
				this.richTxtCardInfo.ForeColor = SystemColors.WindowText;
				this.updateHIKVideo(false);
				this._loadPhoto();
				this.ShowCapturePhoto(wgAppConfig.Path4AviJpgOnlyView() + this.prevFileName + ".jpg");
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0012525C File Offset: 0x0012425C
		private void P4Player_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (!this.bStartSlow)
				{
					if (this.selectedPanel == null || this.selectedPanel != sender)
					{
						this.selectedPanel = (Panel)sender;
						this.bEnlargeOne = true;
						this.video_play(this.hkVideo4contr.CameraNum());
					}
					else
					{
						this.bEnlargeOne = !this.bEnlargeOne;
						this.video_play(this.hkVideo4contr.CameraNum());
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x001252EC File Offset: 0x001242EC
		private void picJpg_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				this.txtPhotoInfo.Visible = false;
				if (this.bEnlarged)
				{
					base.Size = new Size(new Point(298, 268));
					this.PanelMp4.Size = new Size(new Point(300, 240));
					this.picJpg.Size = new Size(new Point(300, 240));
				}
				else
				{
					base.Size = new Size(new Point(800, 668));
					this.PanelMp4.Size = new Size(new Point(800, 640));
					this.picJpg.Size = new Size(new Point(800, 640));
				}
				this.bEnlarged = !this.bEnlarged;
				if (this.TabControl1.SelectedTab == this.TabPage2)
				{
					this.btnRefreshAVI.PerformClick();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00125404 File Offset: 0x00124404
		private void picJpg_DoubleClick_old(object sender, EventArgs e)
		{
			try
			{
				if (!this.bWatching || this.onlyCapturePhoto >= 2)
				{
					this.txtPhotoInfo.Visible = false;
					if (this.bEnlarged)
					{
						base.Size = new Size(new Point(1024, 275));
						this.grpTop.Size = new Size(new Point(2048, 280));
						this.Panel1.Size = new Size(new Point(1018, 248));
						this.TabControl1.Size = new Size(new Point(300, 246));
						this.TabControl1.Location = new Point(1160, 8);
						this.PanelMp4.Size = new Size(new Point(300, 240));
						this.picJpg.Size = new Size(new Point(300, 240));
						this.txtCardInfo.Visible = true;
						this.richTxtCardInfo.Visible = true;
					}
					else
					{
						this.txtCardInfo.Visible = false;
						this.richTxtCardInfo.Visible = false;
						this.grpTop.Size = new Size(new Point(2048, 636));
						this.Panel1.Size = new Size(new Point(1018, 620));
						base.Size = new Size(new Point(1024, 648));
						this.TabControl1.Size = new Size(new Point(824, 612));
						this.TabControl1.Location = new Point(656, 8);
						this.PanelMp4.Size = new Size(new Point(800, 600));
						this.picJpg.Size = new Size(new Point(800, 600));
					}
					this.bEnlarged = !this.bEnlarged;
					if (this.TabControl1.SelectedTab == this.TabPage2)
					{
						this.btnRefreshAVI.PerformClick();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00125658 File Offset: 0x00124658
		public void reload()
		{
			try
			{
				if (this.bHide)
				{
					base.WindowState = FormWindowState.Minimized;
					base.Hide();
				}
				else if (this.fileName != "" && this.prevFileName != this.fileName)
				{
					if (!this.bWatching)
					{
						this.Timer1.Enabled = false;
						this.prevFileName = this.fileName;
						this.onlyView();
					}
					else
					{
						this.Timer1.Enabled = false;
						this.prevFileName = this.fileName;
						if (this.arrFileName.IndexOf(this.prevFileName) >= 0)
						{
							this.indexArr = this.arrFileName.IndexOf(this.prevFileName);
							this.updateButtonState();
						}
						else
						{
							this.arrFileName.Add(this.prevFileName);
							this.arrCardInfo.Add(this.newCardInfo);
							this.arrnewCardNo.Add(this.newCardNo);
							this.indexArr = this.arrFileName.Count - 1;
							this.updateButtonState();
						}
						this.txtCardInfo.Text = this.arrCardInfo[this.indexArr].ToString();
						this.richTxtCardInfo.Text = this.arrCardInfo[this.indexArr].ToString();
						this.richTxtCardInfo.ForeColor = SystemColors.WindowText;
						this.bNeedLoadAvi = true;
						this.updateHIKVideo(true);
						this.bNeedLoadPhoto = true;
						this._loadPhoto();
						this.ShowCapturePhoto(wgAppConfig.Path4AviJpg() + this.prevFileName + ".jpg");
						this.Timer1.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00125838 File Offset: 0x00124838
		public void setP4Wnd(Panel p4panel, int x, int y, int width, int height)
		{
			try
			{
				p4panel.Location = new Point(x, y);
				p4panel.Width = width;
				p4panel.Height = height;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00125878 File Offset: 0x00124878
		public void ShowCapturePhoto(string fileToDisplay)
		{
			wgAppConfig.wgLogWithoutDB(fileToDisplay);
			if (!string.IsNullOrEmpty(fileToDisplay))
			{
				if (this.dtShowByClick.AddMilliseconds(500.0) > DateTime.Now)
				{
					this.arrStrNotDisplayedFileName.Clear();
				}
				else if (wgAppConfig.FileIsExisted(fileToDisplay))
				{
					this.arrStrNotDisplayedFileName.Clear();
				}
				else if (DateTime.Now <= this.timeCapturePhoto.AddMilliseconds(1000.0))
				{
					for (int i = 0; i < this.arrStrNotDisplayedFileName.Count; i++)
					{
						if (wgAppConfig.FileIsExisted((string)this.arrStrNotDisplayedFileName[i]))
						{
							this.ShowCapturePhotoLast((string)this.arrStrNotDisplayedFileName[i]);
							this.arrStrNotDisplayedFileName.RemoveRange(0, i + 1);
							return;
						}
					}
					if (this.arrStrNotDisplayedFileName.IndexOf(fileToDisplay) < 0)
					{
						this.arrStrNotDisplayedFileName.Add(fileToDisplay);
					}
					return;
				}
			}
			PictureBox pictureBox = this.picJpg;
			string text = fileToDisplay;
			this.toolTip1.SetToolTip(pictureBox, "");
			this.currentDisplayedJPGFileName = "";
			try
			{
				pictureBox.Visible = false;
				if (text != null)
				{
					if (!wgAppConfig.FileIsExisted(text))
					{
						text = text.ToLower().Replace(".jpg", ".bmp");
						if (wgAppConfig.FileIsExisted(text))
						{
							Image image = Image.FromFile(text);
							image.Save(text.Replace("bmp", "jpg"), ImageFormat.Jpeg);
							image.Dispose();
							if (wgAppConfig.FileIsExisted(text.Replace("bmp", "jpg")))
							{
								new FileInfo(text).Delete();
								text = text.ToLower().Replace(".bmp", ".jpg");
							}
						}
					}
					if (wgAppConfig.FileIsExisted(text))
					{
						pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
						FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read);
						this.CapturePhotoImageData = new byte[fileStream.Length + 1L];
						fileStream.Read(this.CapturePhotoImageData, 0, (int)fileStream.Length);
						fileStream.Close();
						if (this.CapturePhotoMemoryStream != null)
						{
							try
							{
								this.CapturePhotoMemoryStream.Close();
							}
							catch (Exception)
							{
							}
							this.CapturePhotoMemoryStream = null;
						}
						this.CapturePhotoMemoryStream = new MemoryStream(this.CapturePhotoImageData);
						try
						{
							if (pictureBox.Image != null)
							{
								pictureBox.Image.Dispose();
							}
						}
						catch (Exception)
						{
						}
						pictureBox.Image = Image.FromStream(this.CapturePhotoMemoryStream);
						pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
						this.bNeedLoadPhoto = false;
						pictureBox.Visible = true;
						this.currentDisplayedJPGFileName = text;
						if (text.LastIndexOf("\\") > 0)
						{
							this.toolTip1.SetToolTip(pictureBox, text.Substring(text.LastIndexOf("\\") + 1));
						}
						else
						{
							this.toolTip1.SetToolTip(pictureBox, text);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00125B7C File Offset: 0x00124B7C
		public void ShowCapturePhotoLast(string fileToDisplay)
		{
			PictureBox pictureBox = this.picJpg;
			string text = fileToDisplay;
			this.toolTip1.SetToolTip(pictureBox, "");
			this.currentDisplayedJPGFileName = "";
			try
			{
				pictureBox.Visible = false;
				if (text != null)
				{
					if (!wgAppConfig.FileIsExisted(text))
					{
						text = text.ToLower().Replace(".jpg", ".bmp");
						if (wgAppConfig.FileIsExisted(text))
						{
							Image image = Image.FromFile(text);
							image.Save(text.Replace("bmp", "jpg"), ImageFormat.Jpeg);
							image.Dispose();
							if (wgAppConfig.FileIsExisted(text.Replace("bmp", "jpg")))
							{
								new FileInfo(text).Delete();
								text = text.ToLower().Replace(".bmp", ".jpg");
							}
						}
					}
					if (wgAppConfig.FileIsExisted(text))
					{
						pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
						FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read);
						this.CapturePhotoImageData = new byte[fileStream.Length + 1L];
						fileStream.Read(this.CapturePhotoImageData, 0, (int)fileStream.Length);
						fileStream.Close();
						if (this.CapturePhotoMemoryStream != null)
						{
							try
							{
								this.CapturePhotoMemoryStream.Close();
							}
							catch (Exception)
							{
							}
							this.CapturePhotoMemoryStream = null;
						}
						this.CapturePhotoMemoryStream = new MemoryStream(this.CapturePhotoImageData);
						try
						{
							if (pictureBox.Image != null)
							{
								pictureBox.Image.Dispose();
							}
						}
						catch (Exception)
						{
						}
						pictureBox.Image = Image.FromStream(this.CapturePhotoMemoryStream);
						pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
						this.bNeedLoadPhoto = false;
						pictureBox.Visible = true;
						this.currentDisplayedJPGFileName = text;
						if (text.LastIndexOf("\\") > 0)
						{
							this.toolTip1.SetToolTip(pictureBox, text.Substring(text.LastIndexOf("\\") + 1));
						}
						else
						{
							this.toolTip1.SetToolTip(pictureBox, text);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00125D90 File Offset: 0x00124D90
		private void showWhenOnConsoleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_HIDE_PHOTOAVI", "0");
			this.hideToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_HIDE_PHOTOAVI") == "1";
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00125DC0 File Offset: 0x00124DC0
		public void startSlow()
		{
			MethodInvoker methodInvoker = new MethodInvoker(this.startSlow_Invoker);
			try
			{
				if (this.selectedCameraID == "")
				{
					this.hkVideo4contr.loadCamera(false, "");
				}
				else
				{
					this.hkVideo4contr.loadCamera(true, this.selectedCameraID);
				}
				base.BeginInvoke(methodInvoker);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00125E30 File Offset: 0x00124E30
		public void startSlow_Invoker()
		{
			Application.DoEvents();
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				Cursor.Current = Cursors.Default;
				this.video_play(this.hkVideo4contr.CameraNum());
				this.txtCardInfo.Text = comHIKVideo.videoErrInfo;
				this.richTxtCardInfo.Text = comHIKVideo.videoErrInfo;
				this.TimerStartSlow.Enabled = false;
				this.updatePanelInfo();
				if (comHIKVideo.videoErrInfo != "")
				{
					wgRunInfoLog.addEvent(new InfoRow
					{
						desc = string.Format("{0}", this.Text),
						information = string.Format("{0}", comHIKVideo.videoErrInfo)
					});
				}
			}
			catch (Exception)
			{
			}
			this.TimerStartSlow.Enabled = false;
			Cursor.Current = Cursors.Default;
			this.bStartSlow = false;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00125F14 File Offset: 0x00124F14
		public void startTimer1()
		{
			this.Timer1_Tick(null, null);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00125F20 File Offset: 0x00124F20
		public void stopVideo()
		{
			try
			{
				try
				{
					PlayCtrl.PlayM4_Stop(this.USED_PORT);
					PlayCtrl.PlayM4_CloseFile(this.USED_PORT);
					PlayCtrl.PlayM4_FreePort(this.USED_PORT);
				}
				catch (Exception)
				{
				}
				if (this.hkVideo4contr != null)
				{
					try
					{
						this.hkVideo4contr.stopAllCamera();
					}
					catch (Exception)
					{
					}
					this.hkVideo4contr = null;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00125FA4 File Offset: 0x00124FA4
		private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TabControl1.SelectedTab == this.TabPage2)
			{
				this.btnRefreshAVI.Visible = true;
				this.btnRefreshAVI.PerformClick();
				return;
			}
			this.btnRefreshAVI.Visible = false;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00125FE0 File Offset: 0x00124FE0
		private void TabControl2_DrawItem(object sender, DrawItemEventArgs e)
		{
			this.TabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;
			Brush gray = Brushes.Gray;
			e.Graphics.DrawString(this.TabControl2.TabPages[e.Index].Text, e.Font, new SolidBrush(Color.Black), new RectangleF((float)e.Bounds.X, (float)(e.Bounds.Y + 12), (float)e.Bounds.Width, (float)e.Bounds.Height));
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00126079 File Offset: 0x00125079
		private void tabP4PanelA_Click(object sender, EventArgs e)
		{
			if (!this.Panel1.Visible)
			{
				this.Panel1.Visible = true;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00126094 File Offset: 0x00125094
		private void tabP4PanelA_SizeChanged(object sender, EventArgs e)
		{
			if (!this.bStartSlow)
			{
				this.video_play(this.hkVideo4contr.CameraNum());
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x001260B0 File Offset: 0x001250B0
		private void Timer1_Tick(object sender, EventArgs e)
		{
			this.Timer1.Enabled = false;
			this.reload();
			this.Timer1.Enabled = false;
			if (this.bStartSlow && this.bWatching && comHIKVideo.videoErrInfo != "")
			{
				this.txtCardInfo.Text = comHIKVideo.videoErrInfo;
				this.richTxtCardInfo.Text = comHIKVideo.videoErrInfo;
			}
			if (this.bDisplayWarnAvi)
			{
				if (!this.bLoadedAVI)
				{
					this.bNeedLoadAvi = true;
				}
				else if (this.nextRefreshTime > 0)
				{
					this.nextRefreshTime--;
					if (this.nextRefreshTime == 0)
					{
						this.bNeedLoadAvi = true;
					}
				}
				else
				{
					this.nextRefreshTime = Conversion.Int((this.aviTimelength + 500) / this.Timer1.Interval);
				}
			}
			if (this.bNeedLoadAvi)
			{
				if (this.waitTime > 0L)
				{
					this.waitTime -= 1L;
				}
				else
				{
					this.updateHIKVideo(false);
				}
			}
			if (this.bWatching && this.bNeedLoadPhoto)
			{
				this.ShowCapturePhoto(wgAppConfig.Path4AviJpg() + this.prevFileName + ".jpg");
			}
			this.Timer1.Enabled = true;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x001261E4 File Offset: 0x001251E4
		private void Timer2_Tick(object sender, EventArgs e)
		{
			try
			{
				this.txtTime.Text = Strings.Format(DateTime.Now, "HH:mm:ss");
				if (this.bHide)
				{
					base.Hide();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00126234 File Offset: 0x00125234
		private void Timer3_Tick(object sender, EventArgs e)
		{
			this.Timer3.Enabled = false;
			this.txtCardInfo.ForeColor = SystemColors.WindowText;
			this.richTxtCardInfo.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00126262 File Offset: 0x00125262
		private void TimerStartSlow_Tick(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00126270 File Offset: 0x00125270
		public void updateButtonState()
		{
			this.lblCount.Text = this.indexArr + 1 + "/" + this.arrFileName.Count;
			if (this.indexArr < 0)
			{
				this.btnFirst.Enabled = false;
				this.btnPrev.Enabled = false;
				this.btnNext.Enabled = false;
				this.btnEnd.Enabled = false;
				return;
			}
			if (this.indexArr == 0)
			{
				this.btnFirst.Enabled = false;
				this.btnPrev.Enabled = false;
			}
			else
			{
				this.btnFirst.Enabled = true;
				this.btnPrev.Enabled = true;
			}
			if (this.indexArr == this.arrFileName.Count - 1)
			{
				this.btnNext.Enabled = false;
				this.btnEnd.Enabled = false;
				return;
			}
			this.btnNext.Enabled = true;
			this.btnEnd.Enabled = true;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0012636C File Offset: 0x0012536C
		public void updateHIKVideo(bool bSetWaitTime)
		{
			try
			{
				string text;
				if (!this.bWatching)
				{
					text = wgAppConfig.Path4AviJpgOnlyView();
				}
				else
				{
					text = wgAppConfig.Path4AviJpg();
				}
				if (wgAppConfig.FileIsExisted(text + this.prevFileName + ".MP4"))
				{
					FileInfo fileInfo = new FileInfo(text + this.prevFileName + ".MP4");
					if (fileInfo.Length <= 0L)
					{
						if (bSetWaitTime)
						{
							this.waitTime = (long)Conversion.Int(3500 / this.Timer1.Interval);
						}
					}
					else
					{
						StreamReader streamReader = null;
						try
						{
							streamReader = new StreamReader(fileInfo.FullName);
						}
						catch (Exception)
						{
						}
						if (streamReader == null)
						{
							this.waitTime = (long)Conversion.Int(3500 / this.Timer1.Interval);
						}
						else
						{
							streamReader.Close();
							string fullName = fileInfo.FullName;
							bool flag = false;
							if (this.USED_PORT >= 0)
							{
								flag = PlayCtrl.PlayM4_Stop(this.USED_PORT);
								if (!flag && PlayCtrl.PlayM4_GetLastError(this.USED_PORT) != 2U)
								{
									wgAppConfig.wgDebugWrite(DateTime.Now + " Stop failed!  Errnum=" + PlayCtrl.PlayM4_GetLastError(this.USED_PORT).ToString(), EventLogEntryType.Error);
								}
								if (flag)
								{
									flag = PlayCtrl.PlayM4_CloseFile(this.USED_PORT);
									if (!flag)
									{
										wgAppConfig.wgDebugWrite(DateTime.Now + " Close file failed!  Errnum=" + PlayCtrl.PlayM4_GetLastError(this.USED_PORT).ToString(), EventLogEntryType.Error);
									}
								}
							}
							if (!flag)
							{
								PlayCtrl.PlayM4_FreePort(this.USED_PORT);
								this.USED_PORT = -1;
								flag = PlayCtrl.PlayM4_GetPort(ref this.USED_PORT);
							}
							if (!flag)
							{
								wgAppConfig.wgDebugWrite(DateTime.Now + " get port failed!  Errnum=" + PlayCtrl.PlayM4_GetLastError(this.USED_PORT).ToString(), EventLogEntryType.Error);
							}
							else if (!PlayCtrl.PlayM4_OpenFile(this.USED_PORT, fullName))
							{
								wgAppConfig.wgDebugWrite(DateTime.Now + " open file failed!  Errnum=" + PlayCtrl.PlayM4_GetLastError(this.USED_PORT).ToString(), EventLogEntryType.Error);
								if (PlayCtrl.PlayM4_GetLastError(this.USED_PORT) == 7U)
								{
									PlayCtrl.PlayM4_FreePort(this.USED_PORT);
									this.USED_PORT = -1;
									this.PanelMp4.Refresh();
								}
							}
							else if (!PlayCtrl.PlayM4_Play(this.USED_PORT, this.PanelMp4.Handle))
							{
								wgAppConfig.wgDebugWrite(DateTime.Now + " play file failed!  Errnum=" + PlayCtrl.PlayM4_GetLastError(this.USED_PORT).ToString(), EventLogEntryType.Error);
							}
							else
							{
								if (this.bDisplayWarnAvi)
								{
									if (this.TabControl1.SelectedTab != this.TabPage2)
									{
										this.TabControl1.SelectedTab = this.TabPage2;
									}
									this.bLoadedAVI = true;
									this.nextRefreshTime = Conversion.Int((this.aviTimelength + 500) / this.Timer1.Interval);
								}
								this.bNeedLoadAvi = false;
							}
						}
					}
				}
				else
				{
					PlayCtrl.PlayM4_Stop(this.USED_PORT);
					PlayCtrl.PlayM4_CloseFile(this.USED_PORT);
					this.PanelMp4.Refresh();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x001266BC File Offset: 0x001256BC
		public void updatePanelInfo()
		{
			try
			{
				for (int i = 0; i < this.TabControl2.TabPages.Count; i++)
				{
					foreach (object obj in this.TabControl2.TabPages[i].Controls)
					{
						object obj2 = obj;
						if (obj2 is Panel)
						{
							int num = (int)((Panel)obj2).Tag;
							num--;
							if (num < this.hkVideo4contr.panelToolTips.Length && !string.IsNullOrEmpty(this.hkVideo4contr.panelToolTips[num]))
							{
								this.toolTip1.SetToolTip((Panel)obj2, this.hkVideo4contr.panelToolTips[num]);
								if (this.hkVideo4contr.panelErr[num])
								{
									foreach (object obj3 in ((Panel)obj2).Controls)
									{
										if (obj3 is Label)
										{
											((Label)obj3).Visible = true;
											((Label)obj3).Text = this.hkVideo4contr.panelToolTips[num] + "  " + ((Label)obj3).Text;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0012689C File Offset: 0x0012589C
		public bool video_play(int CameraNum)
		{
			int num = 0;
			for (int i = 1; i <= CameraNum; i += this.PanelMaxONOneTABPAGE)
			{
				if (i + this.PanelMaxONOneTABPAGE > CameraNum)
				{
					if (CameraNum <= this.PanelMaxONOneTABPAGE)
					{
						this.video_play6(CameraNum - i + 1, num);
					}
					else
					{
						this.video_play6(this.PanelMaxONOneTABPAGE, num);
					}
				}
				else
				{
					this.video_play6(this.PanelMaxONOneTABPAGE, num);
				}
				num++;
			}
			return true;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00126904 File Offset: 0x00125904
		public bool video_play6(int CameraNum, int tabpageIndex)
		{
			try
			{
				if (this.bEnlargeOne && Conversion.Int(((int)this.selectedPanel.Tag - 1) / this.PanelMaxONOneTABPAGE) == tabpageIndex)
				{
					foreach (object obj in this.TabControl2.TabPages[tabpageIndex].Controls)
					{
						object obj2 = obj;
						if (obj2 is Panel)
						{
							((Panel)obj2).Visible = false;
						}
					}
					return this.onevideo_play6();
				}
				Panel[] array = new Panel[6];
				foreach (object obj3 in this.TabControl2.TabPages[tabpageIndex].Controls)
				{
					object obj2 = obj3;
					if (obj2 is Panel)
					{
						array[((int)((Panel)obj2).Tag - 1) % this.PanelMaxONOneTABPAGE] = (Panel)obj2;
					}
				}
				if (CameraNum > 0)
				{
					int num = 992;
					int num2 = 485;
					double num3 = 1.2222222222222223;
					int num4 = 4;
					Size size = this.TabControl2.SelectedTab.Size;
					switch (CameraNum)
					{
					case 1:
					{
						int num6;
						int num7;
						Size size2;
						if ((double)size.Width / (double)size.Height < num3)
						{
							double num5 = 1.0 * (double)size.Width / (double)num;
							num = size.Width;
							num2 = (int)((double)size.Height * num5);
							num6 = num;
							num7 = (int)((double)num6 / num3);
							size2 = new Size(new Point(num6, num7));
							num2 = size.Height;
						}
						else
						{
							double num5 = 1.0 * (double)size.Height / (double)num2;
							num2 = size.Height;
							num = (int)((double)size.Width * num5);
							num7 = num2;
							num6 = (int)((double)num7 * num3);
							size2 = new Size(new Point(num6, num7));
							num = size.Width;
						}
						int num8 = (size.Width - size2.Width) / 2;
						int num9 = (size.Height - size2.Height) / 2;
						this.setP4Wnd(array[0], num8, num9, num6, num7);
						break;
					}
					case 2:
					{
						int num6;
						int num7;
						Size size2;
						if ((double)size.Width / (double)size.Height < num3 * 2.0)
						{
							double num5 = 1.0 * (double)(size.Width / num) / 2.0;
							num = size.Width / 2;
							num2 = (int)((double)size.Height * num5);
							num6 = num;
							num7 = (int)((double)num6 / num3);
							size2 = new Size(new Point(num6 * 2, num7));
							num2 = size.Height;
						}
						else
						{
							double num5 = 1.0 * (double)size.Height / (double)num2;
							num2 = size.Height;
							num = (int)((double)size.Width * num5);
							num7 = num2;
							num6 = (int)((double)num7 * num3);
							size2 = new Size(new Point(num6 * 2, num7));
							num = size.Width / 2;
						}
						int num8 = (size.Width - size2.Width - num4) / 2;
						int num9 = (size.Height - size2.Height) / 2;
						this.setP4Wnd(array[0], num8, num9, num6, num7);
						this.setP4Wnd(array[1], num8 + size2.Width / 2 + num4, num9, num6, num7);
						break;
					}
					case 3:
					case 4:
					{
						int num6;
						int num7;
						Size size2;
						if ((double)size.Width / (double)size.Height < num3)
						{
							double num5 = 1.0 * (double)size.Width / (double)num;
							num = size.Width / 2;
							num2 = (int)((double)size.Height * num5 / 2.0);
							num6 = num;
							num7 = (int)((double)num6 / num3);
							size2 = new Size(new Point(num6 * 2, num7 * 2));
							num2 = size.Height / 2;
						}
						else
						{
							double num5 = 1.0 * (double)size.Height / (double)num2;
							num2 = size.Height / 2;
							num = (int)((double)size.Width * num5 / 2.0);
							num7 = num2 - num4 / 2;
							num6 = (int)((double)num7 * num3);
							size2 = new Size(new Point(num6 * 2, num7 * 2));
							num = size.Width / 2;
						}
						int num8 = (size.Width - size2.Width - num4) / 2;
						int num9 = (size.Height - size2.Height - num4) / 2;
						this.setP4Wnd(array[0], num8, num9, num6, num7);
						this.setP4Wnd(array[1], num8 + size2.Width / 2 + num4, num9, num6, num7);
						this.setP4Wnd(array[2], num8, num9 + num2 + num4 / 2, num6, num7);
						this.setP4Wnd(array[3], num8 + size2.Width / 2 + num4, num9 + size2.Height / 2 + num4, num6, num7);
						break;
					}
					default:
					{
						int num6;
						int num7;
						Size size2;
						if ((double)size.Width / (double)size.Height >= num3 * 1.5)
						{
							double num5 = 1.0 * (double)size.Height / (double)num2;
							num2 = size.Height / 2;
							num = (int)((double)size.Width * num5 / 3.0);
							num7 = num2 - num4 / 2;
							num6 = (int)((double)num7 * num3);
							size2 = new Size(new Point(num6 * 3, num7 * 2));
							num = size.Width / 3;
						}
						else
						{
							double num5 = 1.0 * (double)size.Width / (double)num;
							num = size.Width / 3;
							num2 = (int)((double)size.Height * num5 / 2.0);
							num6 = num;
							num7 = (int)((double)num6 / num3);
							size2 = new Size(new Point(num6 * 3, num7 * 2));
							num2 = size.Height / 2;
						}
						int num8 = (size.Width - size2.Width - 2 * num4) / 2;
						int num9 = (size.Height - size2.Height - num4) / 2;
						this.setP4Wnd(array[0], num8, num9, num6, num7);
						this.setP4Wnd(array[1], num8 + size2.Width / 3 + num4, num9, num6, num7);
						this.setP4Wnd(array[2], num8 + 2 * (size2.Width / 3 + num4), num9, num6, num7);
						this.setP4Wnd(array[3], num8, num9 + size2.Height / 2 + num4, num6, num7);
						this.setP4Wnd(array[4], num8 + size2.Width / 3 + num4, num9 + size2.Height / 2 + num4, num6, num7);
						this.setP4Wnd(array[5], num8 + 2 * (size2.Width / 3 + num4), num9 + size2.Height / 2 + num4, num6, num7);
						break;
					}
					}
				}
				for (int i = 0; i <= 5; i++)
				{
					array[i].Visible = false;
				}
				for (int i = 0; i <= CameraNum - 1; i++)
				{
					array[i].Visible = true;
				}
				return true;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
			}
			return false;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0012708C File Offset: 0x0012608C
		private void viewJPEGByDefaultProgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(this.currentDisplayedJPGFileName) && wgAppConfig.FileIsExisted(this.currentDisplayedJPGFileName))
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = this.currentDisplayedJPGFileName,
						UseShellExecute = true
					});
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00127104 File Offset: 0x00126104
		private void viewVideoByDefaultProgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(this.currentDisplayedJPGFileName))
				{
					string text = this.currentDisplayedJPGFileName.ToUpper().Replace(".JPG", ".MP4");
					if (wgAppConfig.FileIsExisted(text))
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = text,
							UseShellExecute = true
						});
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x04001CCC RID: 7372
		public const int PHOTO_XSIZE = 96;

		// Token: 0x04001CCD RID: 7373
		public const int PHOTO_YSIZE = 120;

		// Token: 0x04001CCE RID: 7374
		private bool bEnlarged;

		// Token: 0x04001CCF RID: 7375
		private bool bEnlargeOne;

		// Token: 0x04001CD0 RID: 7376
		private bool bGroup2;

		// Token: 0x04001CD1 RID: 7377
		private bool bHide;

		// Token: 0x04001CD2 RID: 7378
		private bool bNeedLoadAvi;

		// Token: 0x04001CD3 RID: 7379
		private bool bNeedLoadPhoto;

		// Token: 0x04001CD5 RID: 7381
		private Point group2OldLocation;

		// Token: 0x04001CD6 RID: 7382
		private comHIKVideo hkVideo4contr;

		// Token: 0x04001CD7 RID: 7383
		private int onlyCapturePhoto;

		// Token: 0x04001CD8 RID: 7384
		private Thread startSlowThread;

		// Token: 0x04001CD9 RID: 7385
		private long waitTime;

		// Token: 0x04001CDA RID: 7386
		private string prevFileName;

		// Token: 0x04001CDB RID: 7387
		private ArrayList arrCardInfo = new ArrayList();

		// Token: 0x04001CDC RID: 7388
		private ArrayList arrFileName = new ArrayList();

		// Token: 0x04001CDD RID: 7389
		private ArrayList arrnewCardNo = new ArrayList();

		// Token: 0x04001CDE RID: 7390
		private ArrayList arrStrNotDisplayedFileName = new ArrayList();

		// Token: 0x04001CDF RID: 7391
		public int aviTimelength = 6000;

		// Token: 0x04001CE0 RID: 7392
		public bool bDisplayWarnAvi;

		// Token: 0x04001CE1 RID: 7393
		public bool bLoadedAVI;

		// Token: 0x04001CE2 RID: 7394
		private bool bStartSlow = true;

		// Token: 0x04001CE3 RID: 7395
		public bool bWatching = true;

		// Token: 0x04001CE4 RID: 7396
		private byte[] CapturePhotoImageData;

		// Token: 0x04001CE5 RID: 7397
		private string currentDisplayedJPGFileName = "";

		// Token: 0x04001CE6 RID: 7398
		public DateTime dtShowByClick = DateTime.Now.AddMinutes(-1.0);

		// Token: 0x04001CE7 RID: 7399
		public string fileName = "";

		// Token: 0x04001CE8 RID: 7400
		public Form frmCaller;

		// Token: 0x04001CE9 RID: 7401
		private int indexArr = -1;

		// Token: 0x04001CEA RID: 7402
		private Label lblNotConnect = new Label();

		// Token: 0x04001CEB RID: 7403
		public string newCardInfo = "";

		// Token: 0x04001CEC RID: 7404
		public long newCardNo;

		// Token: 0x04001CED RID: 7405
		public int nextRefreshTime;

		// Token: 0x04001CEE RID: 7406
		public int PanelMaxONOneTABPAGE = 6;

		// Token: 0x04001CEF RID: 7407
		private Size preVideoSize = new Size(new Point(0, 0));

		// Token: 0x04001CF0 RID: 7408
		public string selectedCameraID = "";

		// Token: 0x04001CF1 RID: 7409
		public DateTime timeCapturePhoto = DateTime.Now.AddMinutes(-1.0);

		// Token: 0x04001CF2 RID: 7410
		public int USED_PORT = 99;

		// Token: 0x04001CFA RID: 7418
		private Panel P4Player;

		// Token: 0x04001CFC RID: 7420
		private Panel selectedPanel;

		// Token: 0x04001CFF RID: 7423
		private TabPage TabPageCamera;
	}
}
