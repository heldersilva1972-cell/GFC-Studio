using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000058 RID: 88
	public partial class frmWatchingMoreRecords : frmN3000
	{
		// Token: 0x060006A6 RID: 1702 RVA: 0x000BB4D8 File Offset: 0x000BA4D8
		public frmWatchingMoreRecords()
		{
			this.InitializeComponent();
			Color keyColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.richTextBox1.BackColor = keyColor;
			this.richTextBox2.BackColor = keyColor;
			this.richTextBox3.BackColor = keyColor;
			this.richTextBox4.BackColor = keyColor;
			this.richTextBox5.BackColor = keyColor;
			this.richTextBox6.BackColor = keyColor;
			this.pictureBox1.BackColor = keyColor;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000BB579 File Offset: 0x000BA579
		private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_RGO_VALIDTIME", "");
			base.Close();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000BB590 File Offset: 0x000BA590
		private void enlargeFontToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			try
			{
				this.InfoFontSize += 1f;
				RichTextBox[] array = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
				for (int i = 0; i < 5; i++)
				{
					array[i].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, array[i].Font.Unit);
				}
				this.infoSizeChange++;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000BB648 File Offset: 0x000BA648
		private void enlargeInfoDisplayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				RichTextBox[] array = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
				PictureBox[] array2 = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
				if (array2[0].Height > 26)
				{
					this.richTextSizeChange++;
				}
				for (int i = 0; i < 5; i++)
				{
					if (array2[i].Height > 26)
					{
						array[i].Size = new Size(array[i].Width, array[i].Height + 26);
						array2[i].Location = new Point(array2[i].Location.X, array2[i].Location.Y + 26);
						array2[i].Size = new Size(array2[i].Width, array2[i].Height - 26);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000BB7A8 File Offset: 0x000BA7A8
		private void frmWatchingMoreRecords_FormClosing(object sender, FormClosingEventArgs e)
		{
			wgAppConfig.DisposeImage(this.pictureBox1.Image);
			wgAppConfig.DisposeImage(this.pictureBox2.Image);
			wgAppConfig.DisposeImage(this.pictureBox3.Image);
			wgAppConfig.DisposeImage(this.pictureBox4.Image);
			wgAppConfig.DisposeImage(this.pictureBox5.Image);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000BB808 File Offset: 0x000BA808
		private void frmWatchingMoreRecords_Load(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_RGO_VALIDTIME")))
				{
					this.bSpecialStyleA = wgAppConfig.GetKeyVal("KEY_RGO_VALIDTIME") == "1";
				}
				if (this.tbRunInfoLog != null)
				{
					string keyVal = wgAppConfig.GetKeyVal("WatchingMoreRecords_Display");
					int num = 0;
					int num2 = 0;
					if (!string.IsNullOrEmpty(keyVal))
					{
						try
						{
							string[] array = keyVal.Split(new char[] { ',' });
							base.Size = new Size(int.Parse(array[0]), int.Parse(array[1]));
							base.Location = new Point(int.Parse(array[2]), int.Parse(array[3]));
							this.groupMax = int.Parse(array[5]);
							if (array.Length >= 8)
							{
								num = int.Parse(array[7]);
								num2 = int.Parse(array[8]);
							}
						}
						catch (Exception ex)
						{
							wgAppConfig.wgLog(ex.ToString());
						}
					}
					GroupBox[] array2 = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
					for (int i = 0; i < 5; i++)
					{
						if (i >= this.groupMax)
						{
							array2[i].Visible = false;
						}
						else
						{
							array2[i].Visible = true;
						}
					}
					if (this.bDisplayCapturedPhoto)
					{
						this.groupBox6.Visible = true;
					}
					else
					{
						this.groupBox6.Visible = false;
					}
					this.lstSwipes_RowsAdded(null, null);
					this.frmWatchingMoreRecords_SizeChanged(null, null);
					if (num > 0)
					{
						for (int j = 0; j < num; j++)
						{
							this.enlargeInfoDisplayToolStripMenuItem_Click(null, null);
						}
					}
					else if (num < 0)
					{
						for (int k = 0; k < -num; k++)
						{
							this.ReduceInfoDisplaytoolStripMenuItem_Click(null, null);
						}
					}
					if (num2 > 0)
					{
						for (int l = 0; l < num2; l++)
						{
							this.enlargeFontToolStripMenuItem1_Click(null, null);
						}
					}
					else if (num2 < 0)
					{
						for (int m = 0; m < -num2; m++)
						{
							this.ReduceFontToolStripMenuItem_Click(null, null);
						}
					}
					this.frmWatchingMoreRecords_SizeChanged(null, null);
				}
				this.timer1.Enabled = true;
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000BBA5C File Offset: 0x000BAA5C
		private void frmWatchingMoreRecords_SizeChanged(object sender, EventArgs e)
		{
			GroupBox[] array = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
			if (wgAppConfig.IsActivateCameraManage)
			{
				for (int i = 0; i < this.groupMax; i++)
				{
					array[i].Size = new Size(this.flowLayoutPanel1.Width / (this.groupMax + 1) - 8, this.flowLayoutPanel1.Height - 18);
				}
				this.groupBox6.Size = new Size(this.flowLayoutPanel1.Width / (this.groupMax + 1) - 8, this.flowLayoutPanel1.Height - 18);
			}
			else
			{
				for (int j = 0; j < this.groupMax; j++)
				{
					array[j].Size = new Size(this.flowLayoutPanel1.Width / this.groupMax - 8, this.flowLayoutPanel1.Height - 18);
				}
			}
			RichTextBox[] array2 = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
			for (int k = 0; k < 5; k++)
			{
				if (k >= 3 && array2[k].Size.Width != array2[0].Size.Width)
				{
					array2[k].Size = new Size(array2[0].Size.Width, array2[0].Size.Height);
				}
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000BBC0C File Offset: 0x000BAC0C
		private void loadPhoto(long cardno, ref PictureBox box)
		{
			try
			{
				box.Visible = false;
				string photoFileName = wgAppConfig.getPhotoFileName(cardno);
				Image image = box.Image;
				wgAppConfig.ShowMyImage(photoFileName, ref image);
				if (image != null)
				{
					box.Image = image;
					box.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000BBC78 File Offset: 0x000BAC78
		private void loadPhotoByConsumerNO(string consumerNO, ref PictureBox box)
		{
			try
			{
				box.Visible = false;
				string photoFileNameByConsumerNO = wgAppConfig.getPhotoFileNameByConsumerNO(consumerNO);
				Image image = box.Image;
				wgAppConfig.ShowMyImage(photoFileNameByConsumerNO, ref image);
				if (image != null)
				{
					box.Image = image;
					box.Visible = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000BBCE4 File Offset: 0x000BACE4
		private void lstSwipes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (this.tbRunInfoLog != null)
			{
				this.lastCnt = this.tbRunInfoLog.Rows.Count;
				if (this.tbRunInfoLog.Rows.Count == 0)
				{
					this.lstSwipes_RowsRemoved(null, null);
					return;
				}
				base.SuspendLayout();
				if (this.tbRunInfoLog.Rows.Count > 0)
				{
					int num = 0;
					if (this.grp == null)
					{
						this.grp = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
						this.txtB = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
						this.picBox = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
					}
					for (int i = this.tbRunInfoLog.Rows.Count - 1; i >= 0; i--)
					{
						string text = this.tbRunInfoLog.Rows[i]["f_Detail"] as string;
						string text2 = this.tbRunInfoLog.Rows[i]["f_MjRecStr"] as string;
						if (!string.IsNullOrEmpty(text2))
						{
							MjRec mjRec = new MjRec(this.tbRunInfoLog.Rows[i]["f_MjRecStr"] as string);
							if (mjRec.IsSwipeRecord)
							{
								if (wgAppConfig.IsPhotoNameFromConsumerNO)
								{
									try
									{
										this.picBox[num].Visible = false;
										if (text.IndexOf(wgAppConfig.ReplaceWorkNO(CommonStr.strReplaceWorkNO)) > 0)
										{
											string[] array = text.Split(new char[] { '\r' });
											if (array.Length > 2)
											{
												string[] array2 = array[1].Split(new char[] { '\t' });
												if (array2.Length >= 2)
												{
													string text3 = array2[1];
													if (!string.IsNullOrEmpty(text3))
													{
														this.loadPhotoByConsumerNO(text3, ref this.picBox[num]);
													}
												}
											}
										}
										goto IL_027D;
									}
									catch (Exception ex)
									{
										wgAppConfig.wgLog(ex.ToString());
										goto IL_027D;
									}
									goto IL_0264;
								}
								goto IL_0264;
								IL_027D:
								this.txtB[num].Text = text;
								this.txtB[num].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, this.txtB[num].Font.Unit);
								if (num >= 3 && this.txtB[num].Size.Width != this.txtB[0].Size.Width)
								{
									this.txtB[num].Size = new Size(this.txtB[0].Size.Width, this.txtB[0].Size.Height);
								}
								this.grp[num].Text = this.tbRunInfoLog.Rows[i]["f_RecID"].ToString();
								this.grp[num].Visible = true;
								if (mjRec.IsPassed)
								{
									this.txtB[num].BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
								}
								else
								{
									this.txtB[num].BackColor = Color.Orange;
								}
								if (this.bSpecialStyleA)
								{
									try
									{
										if (mjRec.IsPassed)
										{
											this.txtB[num].BackColor = Color.Green;
										}
										else
										{
											this.txtB[num].BackColor = Color.Orange;
											if (mjRec.SwipeStatus >= 144 && mjRec.SwipeStatus <= 147)
											{
												this.txtB[num].BackColor = Color.Red;
											}
										}
										string valStringBySql = wgAppConfig.getValStringBySql(string.Format("SELECT f_BeginYMD FROM t_b_consumer where f_CardNO = {0} ", mjRec.CardID));
										string valStringBySql2 = wgAppConfig.getValStringBySql(string.Format("SELECT f_EndYMD FROM t_b_consumer where f_CardNO = {0} ", mjRec.CardID));
										if (!string.IsNullOrEmpty(valStringBySql) && !string.IsNullOrEmpty(valStringBySql2))
										{
											this.txtB[num].Text = this.txtB[num].Text + string.Format(CommonStr.strStyleA01, DateTime.Parse(valStringBySql).ToString(CommonStr.strStyleA02), DateTime.Parse(valStringBySql2).ToString(CommonStr.strStyleA02));
										}
									}
									catch (Exception ex2)
									{
										wgAppConfig.wgLogWithoutDB(ex2.ToString());
									}
								}
								if (wgAppConfig.IsActivateCameraManage && num == 0)
								{
									string text4;
									if (wgAppConfig.GetKeyVal("KEY_Video_DontCaputreOnThisPC") != "1")
									{
										text4 = string.Concat(new string[]
										{
											wgAppConfig.Path4AviJpg(),
											mjRec.ReadDate.ToString("yyyyMMdd_HHmmss_"),
											mjRec.CardID.ToString(),
											"_",
											mjRec.ToStringRaw(),
											".JPG"
										});
									}
									else
									{
										text4 = string.Concat(new string[]
										{
											wgAppConfig.Path4AviJpgOnlyView(),
											mjRec.ReadDate.ToString("yyyyMMdd_HHmmss_"),
											mjRec.CardID.ToString(),
											"_",
											mjRec.ToStringRaw(),
											".JPG"
										});
									}
									Image image = this.pictureBox6.Image;
									wgAppConfig.ShowMyImage(text4, ref image);
									if (image != null)
									{
										this.pictureBox6.Image = image;
										this.pictureBox6.Visible = true;
										this.needCheckFileName = "";
									}
									else
									{
										this.needCheckFileName = text4;
										this.triesNeedCheck = 5;
										this.pictureBox6.Visible = false;
									}
								}
								num++;
								if (num < this.groupMax)
								{
									goto IL_0610;
								}
								break;
								IL_0264:
								this.loadPhoto(mjRec.CardID, ref this.picBox[num]);
								goto IL_027D;
							}
						}
						IL_0610:;
					}
					this.richTextBox1.Text = this.txtB[0].Text;
				}
				base.ResumeLayout();
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000BC348 File Offset: 0x000BB348
		private void lstSwipes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			this.richTextBox1.Text = "";
			this.richTextBox2.Text = "";
			this.richTextBox3.Text = "";
			this.richTextBox4.Text = "";
			this.richTextBox5.Text = "";
			Color keyColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.richTextBox1.BackColor = keyColor;
			this.richTextBox2.BackColor = keyColor;
			this.richTextBox3.BackColor = keyColor;
			this.richTextBox4.BackColor = keyColor;
			this.richTextBox5.BackColor = keyColor;
			this.richTextBox6.BackColor = keyColor;
			this.groupBox1.Text = "";
			this.groupBox2.Text = "";
			this.groupBox3.Text = "";
			this.groupBox4.Text = "";
			this.groupBox5.Text = "";
			this.pictureBox1.Image = null;
			this.pictureBox2.Image = null;
			this.pictureBox3.Image = null;
			this.pictureBox4.Image = null;
			this.pictureBox5.Image = null;
			this.lastCnt = 0;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000BC490 File Offset: 0x000BB490
		public void ReallyCloseForm()
		{
			wgAppConfig.DisposeImage(this.pictureBox1.Image);
			wgAppConfig.DisposeImage(this.pictureBox2.Image);
			wgAppConfig.DisposeImage(this.pictureBox3.Image);
			wgAppConfig.DisposeImage(this.pictureBox4.Image);
			wgAppConfig.DisposeImage(this.pictureBox5.Image);
			base.Close();
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000BC4F4 File Offset: 0x000BB4F4
		private void ReduceFontToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.InfoFontSize > 9f)
				{
					this.InfoFontSize -= 1f;
					RichTextBox[] array = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
					for (int i = 0; i < 5; i++)
					{
						array[i].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, array[i].Font.Unit);
					}
					this.infoSizeChange--;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000BC5C0 File Offset: 0x000BB5C0
		private void ReduceInfoDisplaytoolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.richTextBox1.Height >= 26)
				{
					this.richTextSizeChange--;
					RichTextBox[] array = new RichTextBox[] { this.richTextBox1, this.richTextBox2, this.richTextBox3, this.richTextBox4, this.richTextBox5 };
					PictureBox[] array2 = new PictureBox[] { this.pictureBox1, this.pictureBox2, this.pictureBox3, this.pictureBox4, this.pictureBox5 };
					for (int i = 0; i < 5; i++)
					{
						array[i].Size = new Size(array[i].Width, array[i].Height - 26);
						array2[i].Location = new Point(array2[i].Location.X, array2[i].Location.Y - 26);
						array2[i].Size = new Size(array2[i].Width, array2[i].Height + 26);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000BC714 File Offset: 0x000BB714
		private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", "");
			base.Close();
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000BC72B File Offset: 0x000BB72B
		private void rGOValidTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_RGO_VALIDTIME", "1");
			base.Close();
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000BC744 File Offset: 0x000BB744
		private void saveDisplayStyleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = base.Size.Width.ToString() + "," + base.Size.Height.ToString() + ",";
			string text2 = string.Concat(new string[]
			{
				text,
				base.Location.X.ToString(),
				",",
				base.Location.Y.ToString(),
				",",
				this.richTextBox1.Height.ToString(),
				",",
				this.groupMax.ToString(),
				",",
				this.InfoFontSize.ToString(),
				",",
				this.richTextSizeChange.ToString(),
				",",
				this.infoSizeChange.ToString()
			});
			wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", text2);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000BC880 File Offset: 0x000BB880
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			try
			{
				if (this.tbRunInfoLog != null && this.lastCnt != this.tbRunInfoLog.Rows.Count)
				{
					this.lstSwipes_RowsAdded(null, null);
				}
			}
			catch
			{
			}
			try
			{
				if (!string.IsNullOrEmpty(this.needCheckFileName) && !this.pictureBox6.Visible && this.triesNeedCheck > 0)
				{
					this.triesNeedCheck--;
					Image image = this.pictureBox6.Image;
					wgAppConfig.ShowMyImage(this.needCheckFileName, ref image);
					if (image != null)
					{
						this.pictureBox6.Image = image;
						this.pictureBox6.Visible = true;
						this.needCheckFileName = "";
					}
				}
			}
			catch
			{
			}
			this.timer1.Enabled = true;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000BC964 File Offset: 0x000BB964
		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			int num;
			if (sender == this.toolStripMenuItem2)
			{
				num = 5;
			}
			else if (sender == this.toolStripMenuItem3)
			{
				num = 4;
			}
			else if (sender == this.toolStripMenuItem4)
			{
				num = 3;
			}
			else if (sender == this.toolStripMenuItem5)
			{
				num = 2;
			}
			else
			{
				if (sender != this.toolStripMenuItem6)
				{
					return;
				}
				num = 1;
			}
			GroupBox[] array = new GroupBox[] { this.groupBox1, this.groupBox2, this.groupBox3, this.groupBox4, this.groupBox5 };
			for (int i = 0; i < 5; i++)
			{
				if (i >= num)
				{
					array[i].Visible = false;
				}
				else
				{
					array[i].Visible = true;
				}
			}
			this.groupMax = num;
			this.frmWatchingMoreRecords_SizeChanged(null, null);
		}

		// Token: 0x04000C5B RID: 3163
		private bool bSpecialStyleA;

		// Token: 0x04000C5C RID: 3164
		private int infoSizeChange;

		// Token: 0x04000C5D RID: 3165
		private int richTextSizeChange;

		// Token: 0x04000C5E RID: 3166
		private int triesNeedCheck;

		// Token: 0x04000C5F RID: 3167
		public bool bDisplayCapturedPhoto;

		// Token: 0x04000C60 RID: 3168
		public int groupMax = 3;

		// Token: 0x04000C61 RID: 3169
		private GroupBox[] grp;

		// Token: 0x04000C62 RID: 3170
		public float InfoFontSize = 15f;

		// Token: 0x04000C63 RID: 3171
		private int lastCnt = -1;

		// Token: 0x04000C64 RID: 3172
		private string needCheckFileName = "";

		// Token: 0x04000C65 RID: 3173
		private PictureBox[] picBox;

		// Token: 0x04000C66 RID: 3174
		public DataTable tbRunInfoLog;

		// Token: 0x04000C67 RID: 3175
		private RichTextBox[] txtB;
	}
}
