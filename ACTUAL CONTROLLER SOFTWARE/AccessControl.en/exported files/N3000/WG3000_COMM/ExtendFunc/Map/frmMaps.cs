using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F3 RID: 755
	public partial class frmMaps : frmN3000
	{
		// Token: 0x060015DE RID: 5598 RVA: 0x001B8340 File Offset: 0x001B7340
		public frmMaps()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvRunInfo);
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x001B83A8 File Offset: 0x001B73A8
		private void btnStopOthers_Click(object sender, EventArgs e)
		{
			if (this.btnStop != null)
			{
				this.btnStop.PerformClick();
			}
			this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
			this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
			this.cmdWatchCurrentMap.BackColor = Color.Transparent;
			this.cmdWatchAllMaps.BackColor = Color.Transparent;
			this.btnStopOthers.BackColor = Color.Transparent;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x001B841C File Offset: 0x001B741C
		private void cmdAddDoor_Click(object sender, EventArgs e)
		{
			float num = 1f;
			float.TryParse(wgAppConfig.getSystemParamByNO(22), out num);
			try
			{
				ToolStripMenuItem toolStripMenuItem = this.cmdAddDoorByLoc;
				this.dvMapDoor = new DataView(this.dvDoors.Table);
				if (this.dvMapDoor.Count > 0 && this.c1tabMaps.TabPages.Count > 0)
				{
					using (dfrmSelectMapDoor dfrmSelectMapDoor = new dfrmSelectMapDoor())
					{
						for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
						{
							foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
							{
								if (obj is Panel)
								{
									foreach (object obj2 in ((Panel)obj).Controls)
									{
										if (obj2 is PictureBox)
										{
											foreach (object obj3 in ((PictureBox)obj2).Controls)
											{
												ucMapDoor ucMapDoor = (ucMapDoor)obj3;
												dfrmSelectMapDoor.lstMappedDoors.Items.Add(ucMapDoor.doorName);
											}
										}
									}
								}
							}
						}
						for (int j = 0; j <= this.dvMapDoor.Count - 1; j++)
						{
							if (dfrmSelectMapDoor.lstMappedDoors.FindStringExact(this.dvMapDoor[j]["f_DoorName"].ToString()) == -1)
							{
								dfrmSelectMapDoor.lstUnMappedDoors.Items.Add(this.dvMapDoor[j]["f_DoorName"]);
							}
						}
						if (dfrmSelectMapDoor.ShowDialog(this) == DialogResult.OK)
						{
							string doorName = dfrmSelectMapDoor.doorName;
							if (!dfrmSelectMapDoor.bAddDoor)
							{
								for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
								{
									foreach (object obj4 in this.c1tabMaps.TabPages[i].Controls)
									{
										if (obj4 is Panel)
										{
											foreach (object obj5 in ((Panel)obj4).Controls)
											{
												if (obj5 is PictureBox)
												{
													foreach (object obj6 in ((PictureBox)obj5).Controls)
													{
														ucMapDoor ucMapDoor2 = (ucMapDoor)obj6;
														if (ucMapDoor2.doorName == doorName)
														{
															ucMapDoor2.Dispose();
															((PictureBox)obj5).Controls.Remove(ucMapDoor2);
															break;
														}
													}
												}
											}
										}
									}
								}
							}
							this.uc1door = new ucMapDoor();
							this.uc1door.doorName = doorName;
							this.uc1door.doorScale = num;
							int num2 = this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text);
							if (num2 < 0)
							{
								this.uc1door.mapScale = 1f;
							}
							else
							{
								this.uc1door.mapScale = (float)this.arrZoomScale[num2];
							}
							this.uc1door.MouseDown += this.UcMapDoor_MouseDown;
							this.uc1door.MouseMove += this.UcMapDoor_MouseMove;
							this.uc1door.MouseUp += this.UcMapDoor_MouseUp;
							this.uc1door.Click += this.ucMapDoor_Click;
							this.uc1door.imgDoor = this.imgDoor2;
							this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
							this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
							foreach (object obj7 in this.c1tabMaps.SelectedTab.Controls)
							{
								if (obj7 is Panel)
								{
									foreach (object obj8 in ((Panel)obj7).Controls)
									{
										if (obj8 is PictureBox)
										{
											this.uc1door.bindSource = (PictureBox)obj8;
											((PictureBox)obj8).Controls.Add(this.uc1door);
											if (sender == this.cmdAddDoorByLoc)
											{
												this.uc1door.Location = this.lastMouseP;
											}
											else
											{
												this.uc1door.Location = new Point(-((PictureBox)obj8).Location.X, -((PictureBox)obj8).Location.Y);
											}
											return;
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

		// Token: 0x060015E1 RID: 5601 RVA: 0x001B8AB8 File Offset: 0x001B7AB8
		private void cmdAddMap_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmMapInfo dfrmMapInfo = new dfrmMapInfo())
				{
					if (dfrmMapInfo.ShowDialog(this) == DialogResult.OK)
					{
						string mapName = dfrmMapInfo.mapName;
						string mapFile = dfrmMapInfo.mapFile;
						for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
						{
							if (this.c1tabMaps.TabPages[i].Text == mapName)
							{
								XMessageBox.Show(CommonStr.strMapNameDuplicated);
								return;
							}
						}
						this.mapPage = new TabPage();
						this.mapPage.Text = mapName;
						this.mapPage.Tag = mapFile;
						this.c1tabMaps.TabPages.Add(this.mapPage);
						this.c1tabMaps.SelectedTab = this.mapPage;
						this.mapPanel = new Panel();
						this.mapPicture = new PictureBox();
						this.mapPanel.Dock = DockStyle.Fill;
						this.mapPanel.BackColor = Color.White;
						this.mapPanel.AutoScroll = true;
						this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
						this.ShowMap(mapFile, this.mapPicture);
						this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
						this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
						this.mapPanel.Controls.Add(this.mapPicture);
						this.mapPage.Controls.Add(this.mapPanel);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x001B8C74 File Offset: 0x001B7C74
		private void cmdCancelAndExit_Click(object sender, EventArgs e)
		{
			try
			{
				this.c1tabMaps.Visible = false;
				this.C1ToolBar4MapEdit.Visible = false;
				this.bEditing = false;
				this.C1ToolBar4MapOperate.Visible = true;
				this.cmdAddDoorByLoc.Visible = false;
				this.loadmapFromDB();
				this.Timer2.Enabled = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x001B8CF0 File Offset: 0x001B7CF0
		private void cmdChangeMapName_Click(object sender, EventArgs e)
		{
			try
			{
				using (dfrmMapInfo dfrmMapInfo = new dfrmMapInfo())
				{
					dfrmMapInfo.txtMapName.Text = this.c1tabMaps.SelectedTab.Text;
					dfrmMapInfo.txtMapFileName.Text = this.c1tabMaps.SelectedTab.Tag.ToString();
					if (dfrmMapInfo.ShowDialog(this) == DialogResult.OK)
					{
						string mapName = dfrmMapInfo.mapName;
						string mapFile = dfrmMapInfo.mapFile;
						for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
						{
							if (this.c1tabMaps.TabPages[i].Text == mapName && i != this.c1tabMaps.SelectedTab.TabIndex)
							{
								XMessageBox.Show(CommonStr.strMapNameDuplicated4Edit);
								return;
							}
						}
						TabPage selectedTab = this.c1tabMaps.SelectedTab;
						selectedTab.Text = mapName;
						selectedTab.Tag = mapFile;
						foreach (object obj in selectedTab.Controls)
						{
							if (obj is Panel)
							{
								foreach (object obj2 in ((Panel)obj).Controls)
								{
									if (obj2 is PictureBox)
									{
										((PictureBox)obj2).SizeMode = PictureBoxSizeMode.AutoSize;
										this.ShowMap(mapFile, (PictureBox)obj2);
										((PictureBox)obj2).SizeMode = PictureBoxSizeMode.StretchImage;
										return;
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

		// Token: 0x060015E4 RID: 5604 RVA: 0x001B8F18 File Offset: 0x001B7F18
		private void cmdCloseMaps_Click(object sender, EventArgs e)
		{
			try
			{
				this.saveEmapInfoLocation();
				for (int i = 0; i < this.c1tabMaps.TabPages.Count; i++)
				{
					foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
					{
						if (obj is Panel)
						{
							foreach (object obj2 in ((Panel)obj).Controls)
							{
								if (obj2 is PictureBox)
								{
									wgAppConfig.DisposeImage((obj2 as PictureBox).Image);
								}
							}
						}
					}
					wgAppConfig.DisposeImage(this.c1tabMaps.TabPages[i].BackgroundImage);
				}
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x001B9050 File Offset: 0x001B8050
		private void cmdDeleteDoor_Click(object sender, EventArgs e)
		{
			try
			{
				bool flag = false;
				foreach (object obj in this.c1tabMaps.SelectedTab.Controls)
				{
					if (obj is Panel)
					{
						foreach (object obj2 in ((Panel)obj).Controls)
						{
							if (obj2 is PictureBox)
							{
								foreach (object obj3 in ((PictureBox)obj2).Controls)
								{
									ucMapDoor ucMapDoor = (ucMapDoor)obj3;
									if (ucMapDoor.txtDoorName == ucMapDoor.ActiveControl)
									{
										if (!flag)
										{
											if (XMessageBox.Show(sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
											{
												return;
											}
											flag = true;
										}
										ucMapDoor.Dispose();
										((PictureBox)obj2).Controls.Remove(ucMapDoor);
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

		// Token: 0x060015E6 RID: 5606 RVA: 0x001B9208 File Offset: 0x001B8208
		private void cmdDeleteMap_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.c1tabMaps.SelectedTab != null && XMessageBox.Show(sender.ToString() + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) != DialogResult.Cancel)
				{
					this.c1tabMaps.TabPages.Remove(this.c1tabMaps.SelectedTab);
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x001B9284 File Offset: 0x001B8284
		private void cmdEditMap_Click(object sender, EventArgs e)
		{
			try
			{
				this.C1ToolBar4MapEdit.Visible = true;
				this.bEditing = true;
				this.C1ToolBar4MapOperate.Visible = false;
				this.cmdAddDoorByLoc.Visible = true;
				this.Timer2.Enabled = false;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x001B92F0 File Offset: 0x001B82F0
		private void cmdSaveMap_Click(object sender, EventArgs e)
		{
			try
			{
				this.dvMapDoor = new DataView(this.dvDoors.Table);
				string text = " DELETE FROM t_d_maps ";
				wgAppConfig.runUpdateSql(text);
				text = " DELETE FROM t_d_mapdoors ";
				wgAppConfig.runUpdateSql(text);
				this.cmdZoomIn.Enabled = false;
				this.cmdZoomOut.Enabled = false;
				this.cmdWatchCurrentMap.Enabled = false;
				this.cmdWatchAllMaps.Enabled = false;
				if (this.dvMapDoor.Count > 0 && this.c1tabMaps.TabPages.Count > 0)
				{
					this.cmdZoomIn.Enabled = true;
					this.cmdZoomOut.Enabled = true;
					this.cmdWatchCurrentMap.Enabled = true;
					this.cmdWatchAllMaps.Enabled = true;
					for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
					{
						foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
						{
							if (obj is Panel)
							{
								foreach (object obj2 in ((Panel)obj).Controls)
								{
									if (obj2 is PictureBox)
									{
										text = " INSERT INTO t_d_maps";
										wgAppConfig.runUpdateSql(string.Concat(new object[]
										{
											text,
											" (f_MapName, f_MapPageIndex, f_MapFile)  Values(",
											wgTools.PrepareStrNUnicode(this.c1tabMaps.TabPages[i].Text),
											" ,",
											i,
											" ,",
											wgTools.PrepareStrNUnicode(this.c1tabMaps.TabPages[i].Tag),
											" )"
										}));
										text = "SELECT f_MapID from t_d_maps where f_MapName = " + wgTools.PrepareStrNUnicode(this.c1tabMaps.TabPages[i].Text);
										long num = (long)int.Parse("0" + wgTools.SetObjToStr(wgAppConfig.getValBySql(text)));
										foreach (object obj3 in ((PictureBox)obj2).Controls)
										{
											ucMapDoor ucMapDoor = (ucMapDoor)obj3;
											this.dvMapDoor.RowFilter = " f_DoorName = " + wgTools.PrepareStr(ucMapDoor.doorName);
											wgAppConfig.runUpdateSql(string.Concat(new object[]
											{
												" INSERT INTO t_d_mapdoors (f_DoorID, f_MapID, f_DoorLocationX, f_DoorLocationY)  Values(",
												this.dvMapDoor[0]["f_DoorID"],
												" ,",
												num,
												" ,",
												ucMapDoor.doorLocation.X,
												",",
												ucMapDoor.doorLocation.Y,
												" )"
											}));
										}
									}
								}
							}
						}
					}
				}
				this.C1ToolBar4MapEdit.Visible = false;
				this.bEditing = false;
				this.C1ToolBar4MapOperate.Visible = true;
				this.cmdAddDoorByLoc.Visible = false;
				this.Timer2.Enabled = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x001B9708 File Offset: 0x001B8708
		private void cmdWatchAllMaps_Click(object sender, EventArgs e)
		{
			try
			{
				this.lstDoors.SelectedItems.Clear();
				for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
				{
					foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
					{
						if (obj is Panel)
						{
							foreach (object obj2 in ((Panel)obj).Controls)
							{
								if (obj2 is PictureBox)
								{
									foreach (object obj3 in ((PictureBox)obj2).Controls)
									{
										ucMapDoor ucMapDoor = (ucMapDoor)obj3;
										for (int j = 0; j <= this.lstDoors.Items.Count - 1; j++)
										{
											if (this.lstDoors.Items[j].Text == ucMapDoor.doorName)
											{
												this.lstDoors.Items[j].Selected = true;
											}
										}
									}
								}
							}
						}
					}
				}
				if (this.btnMonitor != null)
				{
					this.btnMonitor.PerformClick();
					this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
					this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
					this.cmdWatchCurrentMap.BackColor = Color.Transparent;
					this.cmdWatchAllMaps.BackColor = Color.Transparent;
					if (this.lstDoors.SelectedItems.Count > 0)
					{
						(sender as ToolStripButton).BackColor = Color.Green;
						this.btnStopOthers.BackColor = Color.Red;
						(sender as ToolStripButton).Text = CommonStr.strMonitoring;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x001B99A4 File Offset: 0x001B89A4
		private void cmdWatchCurrentMap_Click(object sender, EventArgs e)
		{
			try
			{
				this.lstDoors.SelectedItems.Clear();
				int tabIndex = this.c1tabMaps.SelectedTab.TabIndex;
				foreach (object obj in this.c1tabMaps.TabPages[tabIndex].Controls)
				{
					if (obj is Panel)
					{
						foreach (object obj2 in ((Panel)obj).Controls)
						{
							if (obj2 is PictureBox)
							{
								foreach (object obj3 in ((PictureBox)obj2).Controls)
								{
									ucMapDoor ucMapDoor = (ucMapDoor)obj3;
									for (int i = 0; i <= this.lstDoors.Items.Count - 1; i++)
									{
										if (this.lstDoors.Items[i].Text == ucMapDoor.doorName)
										{
											this.lstDoors.Items[i].Selected = true;
										}
									}
								}
							}
						}
					}
				}
				if (this.btnMonitor != null)
				{
					this.btnMonitor.PerformClick();
					this.cmdWatchCurrentMap.Text = this.strcmdWatchCurrentMapBk;
					this.cmdWatchAllMaps.Text = this.strcmdWatchAllMapsBk;
					this.cmdWatchCurrentMap.BackColor = Color.Transparent;
					this.cmdWatchAllMaps.BackColor = Color.Transparent;
					if (this.lstDoors.SelectedItems.Count > 0)
					{
						(sender as ToolStripButton).BackColor = Color.Green;
						this.btnStopOthers.BackColor = Color.Red;
						(sender as ToolStripButton).Text = CommonStr.strMonitoring;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x001B9C2C File Offset: 0x001B8C2C
		private void cmdZoomIn_Click(object sender, EventArgs e)
		{
			this.mapZoom(1.25f);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x001B9C39 File Offset: 0x001B8C39
		private void cmdZoomOut_Click(object sender, EventArgs e)
		{
			this.mapZoom(0.8f);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x001B9C48 File Offset: 0x001B8C48
		private void dgvRunInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && this.dgvRunInfo.Columns[e.ColumnIndex].Name.Equals("f_Category"))
				{
					string text = e.Value as string;
					if (text != null)
					{
						DataGridViewCell dataGridViewCell = this.dgvRunInfo[e.ColumnIndex, e.RowIndex];
						dataGridViewCell.ToolTipText = text;
						DataGridViewRow dataGridViewRow = this.dgvRunInfo.Rows[e.RowIndex];
						e.Value = InfoRow.getImage(text, ref dataGridViewRow);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x001B9CF8 File Offset: 0x001B8CF8
		private void dgvRunInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (!this.bDataErrorExist)
			{
				this.bDataErrorExist = true;
				wgAppConfig.wgLog(string.Format("dgvRunInfo_DataError  ColumnIndex ={0}, RowIndex ={1}, exception={2}, Context ={3} ", new object[]
				{
					e.ColumnIndex,
					e.RowIndex,
					e.Exception.ToString(),
					e.Context
				}));
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x001B9D64 File Offset: 0x001B8D64
		private void displayNewestLog()
		{
			if (this.dgvRunInfo.Rows.Count > 0)
			{
				this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.dgvRunInfo.Rows.Count - 1;
				this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = true;
				this.dgvRunInfo.Rows[this.dgvRunInfo.Rows.Count - 1].Selected = false;
				Application.DoEvents();
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x001B9DF8 File Offset: 0x001B8DF8
		private void frmMaps_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				for (int i = 0; i < this.c1tabMaps.TabPages.Count; i++)
				{
					foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
					{
						if (obj is Panel)
						{
							foreach (object obj2 in ((Panel)obj).Controls)
							{
								if (obj2 is PictureBox)
								{
									wgAppConfig.DisposeImage((obj2 as PictureBox).Image);
								}
							}
						}
					}
					wgAppConfig.DisposeImage(this.c1tabMaps.TabPages[i].BackgroundImage);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x001B9F10 File Offset: 0x001B8F10
		private void frmMaps_Load(object sender, EventArgs e)
		{
			try
			{
				this.dv = new DataView(this.tbRunInfoLog);
				this.dgvRunInfo.AutoGenerateColumns = false;
				this.dgvRunInfo.DataSource = this.dv;
				this.dgvRunInfo.Columns[0].DataPropertyName = "f_Category";
				this.dgvRunInfo.Columns[1].DataPropertyName = "f_RecID";
				this.dgvRunInfo.Columns[2].DataPropertyName = "f_Time";
				this.dgvRunInfo.Columns[3].DataPropertyName = "f_Desc";
				this.dgvRunInfo.Columns[4].DataPropertyName = "f_Info";
				this.dgvRunInfo.Columns[5].DataPropertyName = "f_Detail";
				this.dgvRunInfo.Columns[6].DataPropertyName = "f_MjRecStr";
				for (int i = 0; i < this.dgvRunInfo.ColumnCount; i++)
				{
					this.dgvRunInfo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				}
				this.C1ToolBar4MapEdit.Visible = false;
				this.cmdAddDoorByLoc.Visible = false;
				this.bEditing = false;
				this.c1tabMaps.TabPages.Clear();
				this.loadDoorData();
				this.strcmdWatchCurrentMapBk = this.cmdWatchCurrentMap.Text;
				this.strcmdWatchAllMapsBk = this.cmdWatchAllMaps.Text;
				this.loadmapFromDB();
				bool flag = false;
				string text = "btnMaps";
				if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
				{
					this.cmdEditMap.Visible = false;
				}
				this.Timer2.Enabled = true;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x001BA0F4 File Offset: 0x001B90F4
		public PictureBox getPicture(TabPage tabpage)
		{
			PictureBox pictureBox = null;
			try
			{
				foreach (object obj in tabpage.Controls)
				{
					if (obj is Panel)
					{
						foreach (object obj2 in ((Panel)obj).Controls)
						{
							if (obj2 is PictureBox)
							{
								pictureBox = (PictureBox)obj2;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return pictureBox;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x001BA1CC File Offset: 0x001B91CC
		private void loadDoorData()
		{
			string text = " SELECT a.f_DoorID, a.f_DoorName , a.f_DoorNO, b.f_ControllerSN, b.f_IP,b.f_PORT, 0 as f_ConnectState, b.f_ZoneID ";
			text += " FROM t_b_Door a, t_b_Controller b WHERE a.f_DoorEnabled > 0 and b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID  ORDER BY  a.f_DoorName ";
			this.dt = new DataTable();
			this.dvDoors = new DataView(this.dt);
			this.dvDoors4Watching = new DataView(this.dt);
			this.dvSelected = new DataView(this.dt);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.dt);
						}
					}
					goto IL_00FD;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.dt);
					}
				}
			}
			IL_00FD:
			icControllerZone icControllerZone = new icControllerZone();
			icControllerZone.getAllowedControllers(ref this.dt);
			try
			{
				this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
			}
			catch (Exception)
			{
				throw;
			}
			this.imgDoor2 = new ImageList();
			this.imgDoor2.ImageSize = new Size(24, 32);
			this.imgDoor2.TransparentColor = SystemColors.Window;
			string systemParamByNO = wgAppConfig.getSystemParamByNO(22);
			if (!string.IsNullOrEmpty(systemParamByNO))
			{
				decimal num = decimal.Parse(systemParamByNO, CultureInfo.InvariantCulture);
				if (num != 1m && num > 0m && num < 100m)
				{
					this.imgDoor2.ImageSize = new Size((int)(24m * num), (int)(32m * num));
				}
			}
			text = " SELECT a.f_ReaderNO, a.f_ReaderName , b.f_ControllerSN ";
			text += " FROM t_b_Reader a, t_b_Controller b WHERE  b.f_Enabled >0 and a.f_ControllerID=b.f_ControllerID ";
			this.dtReader = new DataTable();
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand2 = new OleDbCommand(text, oleDbConnection2))
					{
						using (OleDbDataAdapter oleDbDataAdapter2 = new OleDbDataAdapter(oleDbCommand2))
						{
							oleDbDataAdapter2.Fill(this.dtReader);
						}
					}
					goto IL_02CB;
				}
			}
			using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand2 = new SqlCommand(text, sqlConnection2))
				{
					using (SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2))
					{
						sqlDataAdapter2.Fill(this.dtReader);
					}
				}
			}
			IL_02CB:
			if (this.dtReader.Rows.Count > 0)
			{
				for (int i = 0; i < this.dtReader.Rows.Count; i++)
				{
					this.ReaderName.Add(string.Format("{0}-{1}", this.dtReader.Rows[i]["f_ControllerSN"].ToString(), this.dtReader.Rows[i]["f_ReaderNO"].ToString()), this.dtReader.Rows[i]["f_ReaderName"].ToString());
				}
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x001BA5F4 File Offset: 0x001B95F4
		public void loadEmapInfoLocation()
		{
			try
			{
				string keyVal = wgAppConfig.GetKeyVal("EMapLocInfo");
				string keyVal2 = wgAppConfig.GetKeyVal("EMapZoomInfo");
				if (keyVal != "" && keyVal2 != "")
				{
					string[] array = keyVal.Split(new char[] { ',' });
					string[] array2 = keyVal2.Split(new char[] { ',' });
					if (array.Length * 2 == array2.Length * 3)
					{
						for (int i = 0; i <= array.Length / 3 - 1; i++)
						{
							if (array[i * 3] != array2[i * 2])
							{
								return;
							}
						}
						foreach (object obj in this.c1tabMaps.TabPages)
						{
							TabPage tabPage = (TabPage)obj;
							for (int i = 0; i <= array.Length / 3 - 1; i++)
							{
								if (array[i * 3] == tabPage.Text)
								{
									this.c1tabMaps.SelectedTab = tabPage;
									this.mapZoom(float.Parse(array2[i * 2 + 1]));
									break;
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

		// Token: 0x060015F5 RID: 5621 RVA: 0x001BA77C File Offset: 0x001B977C
		private void loadmapFromDB()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadmapFromDB_Acc();
				return;
			}
			this.c1tabMaps.Visible = false;
			float num = 1f;
			float.TryParse(wgAppConfig.getSystemParamByNO(22), out num);
			try
			{
				this.dstemp = new DataSet("mapInfo");
				this.cn = new SqlConnection(wgAppConfig.dbConString);
				string text = "SELECT * FROM  t_d_maps ORDER BY f_MapPageIndex";
				this.da = new SqlDataAdapter(text, this.cn);
				this.da.Fill(this.dstemp, "t_d_maps");
				this.dvMap = new DataView(this.dstemp.Tables["t_d_maps"]);
				string text2 = "";
				text = " SELECT t_d_mapdoors.*, t_d_maps.f_MapName, t_d_maps.f_MapPageIndex, t_b_Door.f_DoorName, t_b_Controller.f_ZoneID ";
				if (text2 == "")
				{
					text += " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ";
				}
				else
				{
					text = text + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID  WHERE t_b_door.f_DoorID IN " + text2;
				}
				this.da = new SqlDataAdapter(text, this.cn);
				this.da.Fill(this.dstemp, "v_d_mapdoors");
				this.dt = this.dstemp.Tables["v_d_mapdoors"];
				new icControllerZone().getAllowedControllers(ref this.dt);
				this.dvMapDoors = new DataView(this.dstemp.Tables["v_d_mapdoors"]);
				this.c1tabMaps.TabPages.Clear();
				if (this.dvMap.Count > 0)
				{
					this.cmdZoomIn.Enabled = true;
					this.cmdZoomOut.Enabled = true;
					this.cmdWatchCurrentMap.Enabled = true;
					this.cmdWatchAllMaps.Enabled = true;
					for (int i = 0; i <= this.dvMap.Count - 1; i++)
					{
						this.mapPage = new TabPage();
						this.mapPage.Text = this.dvMap[i]["f_MapName"].ToString();
						this.mapPage.Tag = this.dvMap[i]["f_MapFile"];
						this.c1tabMaps.TabPages.Add(this.mapPage);
						this.c1tabMaps.SelectedTab = this.mapPage;
						this.mapPanel = new Panel();
						this.mapPicture = new PictureBox();
						this.mapPanel.Dock = DockStyle.Fill;
						this.mapPanel.BackColor = Color.White;
						this.mapPanel.AutoScroll = true;
						this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
						this.ShowMap(this.mapPage.Tag.ToString(), this.mapPicture);
						this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
						this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
						this.mapPicture.MouseDown += this.mapPicture_MouseDown;
						this.mapPanel.Controls.Add(this.mapPicture);
						this.mapPage.Controls.Add(this.mapPanel);
						this.dvMapDoors.RowFilter = " f_MapID= " + this.dvMap[i]["f_MapID"];
						for (int j = 0; j <= this.dvMapDoors.Count - 1; j++)
						{
							this.uc1door = new ucMapDoor();
							this.uc1door.doorName = this.dvMapDoors[j]["f_DoorName"].ToString();
							this.uc1door.doorScale = num;
							this.uc1door.bindSource = this.mapPicture;
							this.uc1door.doorLocation = new Point(int.Parse(this.dvMapDoors[j]["f_DoorLocationX"].ToString()), int.Parse(this.dvMapDoors[j]["f_DoorLocationY"].ToString()));
							this.uc1door.MouseDown += this.UcMapDoor_MouseDown;
							this.uc1door.MouseMove += this.UcMapDoor_MouseMove;
							this.uc1door.MouseUp += this.UcMapDoor_MouseUp;
							this.uc1door.Click += this.ucMapDoor_Click;
							this.uc1door.imgDoor = this.imgDoor2;
							this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
							this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
							this.mapPicture.Controls.Add(this.uc1door);
						}
					}
				}
				else
				{
					this.cmdZoomIn.Enabled = false;
					this.cmdZoomOut.Enabled = false;
					this.cmdWatchCurrentMap.Enabled = false;
					this.cmdWatchAllMaps.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			this.loadEmapInfoLocation();
			this.c1tabMaps.Visible = true;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x001BAC90 File Offset: 0x001B9C90
		private void loadmapFromDB_Acc()
		{
			this.c1tabMaps.Visible = false;
			float num = 1f;
			float.TryParse(wgAppConfig.getSystemParamByNO(22), out num);
			try
			{
				this.dstemp = new DataSet("mapInfo");
				OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				string text = "SELECT * FROM  t_d_maps ORDER BY f_MapPageIndex";
				new OleDbDataAdapter(text, oleDbConnection).Fill(this.dstemp, "t_d_maps");
				this.dvMap = new DataView(this.dstemp.Tables["t_d_maps"]);
				string text2 = "";
				text = " SELECT t_d_mapdoors.*, t_d_maps.f_MapName, t_d_maps.f_MapPageIndex, t_b_Door.f_DoorName, t_b_Controller.f_ZoneID ";
				if (text2 == "")
				{
					text += " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID ";
				}
				else
				{
					text = text + " FROM ((t_d_mapdoors INNER JOIN t_d_maps ON t_d_mapdoors.f_MapId = t_d_maps.f_MapId) INNER JOIN t_b_Door ON t_d_mapdoors.f_DoorID = t_b_Door.f_DoorID) INNER JOIN t_b_Controller ON t_b_Door.f_ControllerID = t_b_Controller.f_ControllerID  WHERE t_b_door.f_DoorID IN " + text2;
				}
				new OleDbDataAdapter(text, oleDbConnection).Fill(this.dstemp, "v_d_mapdoors");
				this.dt = this.dstemp.Tables["v_d_mapdoors"];
				new icControllerZone().getAllowedControllers(ref this.dt);
				this.dvMapDoors = new DataView(this.dstemp.Tables["v_d_mapdoors"]);
				this.c1tabMaps.TabPages.Clear();
				if (this.dvMap.Count > 0)
				{
					this.cmdZoomIn.Enabled = true;
					this.cmdZoomOut.Enabled = true;
					this.cmdWatchCurrentMap.Enabled = true;
					this.cmdWatchAllMaps.Enabled = true;
					for (int i = 0; i <= this.dvMap.Count - 1; i++)
					{
						this.mapPage = new TabPage();
						this.mapPage.Text = this.dvMap[i]["f_MapName"].ToString();
						this.mapPage.Tag = this.dvMap[i]["f_MapFile"];
						this.c1tabMaps.TabPages.Add(this.mapPage);
						this.c1tabMaps.SelectedTab = this.mapPage;
						this.mapPanel = new Panel();
						this.mapPicture = new PictureBox();
						this.mapPanel.Dock = DockStyle.Fill;
						this.mapPanel.BackColor = Color.White;
						this.mapPanel.AutoScroll = true;
						this.mapPicture.SizeMode = PictureBoxSizeMode.AutoSize;
						this.ShowMap(this.mapPage.Tag.ToString(), this.mapPicture);
						this.mapPicture.SizeMode = PictureBoxSizeMode.StretchImage;
						this.mapPicture.ContextMenuStrip = this.C1CmnuMap;
						this.mapPicture.MouseDown += this.mapPicture_MouseDown;
						this.mapPanel.Controls.Add(this.mapPicture);
						this.mapPage.Controls.Add(this.mapPanel);
						this.dvMapDoors.RowFilter = " f_MapID= " + this.dvMap[i]["f_MapID"];
						for (int j = 0; j <= this.dvMapDoors.Count - 1; j++)
						{
							this.uc1door = new ucMapDoor();
							this.uc1door.doorName = this.dvMapDoors[j]["f_DoorName"].ToString();
							this.uc1door.doorScale = num;
							this.uc1door.bindSource = this.mapPicture;
							this.uc1door.doorLocation = new Point(int.Parse(this.dvMapDoors[j]["f_DoorLocationX"].ToString()), int.Parse(this.dvMapDoors[j]["f_DoorLocationY"].ToString()));
							this.uc1door.MouseDown += this.UcMapDoor_MouseDown;
							this.uc1door.MouseMove += this.UcMapDoor_MouseMove;
							this.uc1door.MouseUp += this.UcMapDoor_MouseUp;
							this.uc1door.Click += this.ucMapDoor_Click;
							this.uc1door.imgDoor = this.imgDoor2;
							this.uc1door.ContextMenuStrip = this.contextMenuStrip1Doors;
							this.uc1door.picDoorState.ContextMenuStrip = this.C1CmnuDoor;
							this.mapPicture.Controls.Add(this.uc1door);
						}
					}
				}
				else
				{
					this.cmdZoomIn.Enabled = false;
					this.cmdZoomOut.Enabled = false;
					this.cmdWatchCurrentMap.Enabled = false;
					this.cmdWatchAllMaps.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			this.loadEmapInfoLocation();
			this.c1tabMaps.Visible = true;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x001BB178 File Offset: 0x001BA178
		private void mapPicture_MouseDown(object sender, MouseEventArgs e)
		{
			this.lastMouseP = e.Location;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x001BB188 File Offset: 0x001BA188
		private void mapZoom(float zoomScale)
		{
			try
			{
				if (this.c1tabMaps.SelectedTab != null)
				{
					PictureBox picture = this.getPicture(this.c1tabMaps.SelectedTab);
					if (picture != null)
					{
						float num = 1f;
						if (this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text) < 0)
						{
							this.arrZoomScaleTabpageName.Add(this.c1tabMaps.SelectedTab.Text);
							this.arrZoomScale.Add(1.0);
						}
						int num2 = this.arrZoomScaleTabpageName.IndexOf(this.c1tabMaps.SelectedTab.Text);
						if (num2 >= 0)
						{
							float.TryParse(this.arrZoomScale[num2].ToString(), out num);
							if ((float)picture.Width * zoomScale >= 10f && (float)picture.Width * zoomScale <= 10000f && (float)picture.Height * zoomScale >= 10f && (float)picture.Height * zoomScale <= 10000f)
							{
								num *= zoomScale;
								this.arrZoomScale[num2] = num;
								picture.Size = new Size(new Point((int)((float)picture.Width * zoomScale), (int)((float)picture.Height * zoomScale)));
								foreach (object obj in picture.Controls)
								{
									if (obj is ucMapDoor)
									{
										((ucMapDoor)obj).mapScale *= zoomScale;
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

		// Token: 0x060015F9 RID: 5625 RVA: 0x001BB378 File Offset: 0x001BA378
		private void mnuHide_Click(object sender, EventArgs e)
		{
			this.dgvRunInfo.Visible = false;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x001BB388 File Offset: 0x001BA388
		public void saveEmapInfoLocation()
		{
			try
			{
				string text = "";
				string text2 = "";
				foreach (object obj in this.c1tabMaps.TabPages)
				{
					TabPage tabPage = (TabPage)obj;
					int num = this.arrZoomScaleTabpageName.IndexOf(tabPage.Text);
					float num2 = 1f;
					if (num >= 0)
					{
						num2 = (float)this.arrZoomScale[num];
					}
					if (text2 != "")
					{
						text2 += ",";
					}
					object obj2 = text2;
					text2 = string.Concat(new object[] { obj2, tabPage.Text, ",", num2 });
					PictureBox picture = this.getPicture(tabPage);
					if (text != "")
					{
						text += ",";
					}
					if (picture != null)
					{
						object obj3 = text;
						text = string.Concat(new object[]
						{
							obj3,
							tabPage.Text,
							",",
							picture.Location.X,
							",",
							picture.Location.Y
						});
					}
					else
					{
						text = text + tabPage.Text + ",0,0";
					}
				}
				wgAppConfig.UpdateKeyVal("EMapZoomInfo", text2);
				wgAppConfig.UpdateKeyVal("EMapLocInfo", text);
				this.arrZoomScaleTabpageName.Clear();
				this.arrZoomScale.Clear();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x001BB584 File Offset: 0x001BA584
		public void ShowMap(string fileToDisplay, PictureBox obj)
		{
			try
			{
				obj.Visible = false;
				if (fileToDisplay != null)
				{
					FileInfo fileInfo = new FileInfo(wgAppConfig.Path4PhotoDefault() + fileToDisplay);
					if (fileInfo.Exists)
					{
						using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
						{
							this.photoImageData = new byte[fileStream.Length + 1L];
							fileStream.Read(this.photoImageData, 0, (int)fileStream.Length);
						}
						if (this.photoMemoryStream != null)
						{
							try
							{
								this.photoMemoryStream.Close();
							}
							catch (Exception)
							{
							}
							this.photoMemoryStream = null;
						}
						this.photoMemoryStream = new MemoryStream(this.photoImageData);
						try
						{
							if (obj.Image != null)
							{
								obj.Image.Dispose();
							}
						}
						catch (Exception)
						{
						}
						obj.Image = Image.FromStream(this.photoMemoryStream);
						obj.Visible = true;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x001BB698 File Offset: 0x001BA698
		private void Timer2_Tick(object sender, EventArgs e)
		{
			if (!this.bEditing)
			{
				this.Timer2.Enabled = false;
				try
				{
					bool flag = false;
					for (int i = 0; i <= this.c1tabMaps.TabPages.Count - 1; i++)
					{
						foreach (object obj in this.c1tabMaps.TabPages[i].Controls)
						{
							if (obj is Panel)
							{
								foreach (object obj2 in ((Panel)obj).Controls)
								{
									if (obj2 is PictureBox)
									{
										foreach (object obj3 in ((PictureBox)obj2).Controls)
										{
											ucMapDoor ucMapDoor = (ucMapDoor)obj3;
											for (int j = 0; j <= this.lstDoors.Items.Count - 1; j++)
											{
												if (this.lstDoors.Items[j].Text == ucMapDoor.doorName && this.lstDoors.Items[j].ImageIndex != ucMapDoor.doorStatus)
												{
													ucMapDoor.doorStatus = this.lstDoors.Items[j].ImageIndex;
													if (!flag)
													{
														if (ucMapDoor.doorStatus >= 4)
														{
															this.c1tabMaps.SelectedTab = this.c1tabMaps.TabPages[i];
														}
														flag = true;
													}
												}
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
				try
				{
					if (this.lastlogInt != this.dgvRunInfo.RowCount)
					{
						if (this.bAllow)
						{
							this.displayNewestLog();
							this.lastlogInt = this.dgvRunInfo.RowCount;
							this.bAllow = false;
						}
						else
						{
							this.bAllow = true;
						}
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				}
				this.Timer2.Enabled = true;
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x001BB980 File Offset: 0x001BA980
		private void ucMapDoor_Click(object sender, EventArgs e)
		{
			try
			{
				ucMapDoor ucMapDoor = (ucMapDoor)sender;
				for (int i = 0; i <= this.lstDoors.Items.Count - 1; i++)
				{
					if (this.lstDoors.Items[i].Text == ucMapDoor.doorName)
					{
						this.lstDoors.SelectedItems.Clear();
						this.lstDoors.Items[i].Selected = true;
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x001BBA14 File Offset: 0x001BAA14
		private void UcMapDoor_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				PictureBox bindSource = ((ucMapDoor)sender).bindSource;
				this.t = bindSource.PointToClient(Control.MousePosition).Y - (sender as Control).Top;
				this.l = bindSource.PointToClient(Control.MousePosition).X - (sender as Control).Left;
				ucMapDoor ucMapDoor = (ucMapDoor)sender;
				this.currentUcMapDoor = ucMapDoor;
				for (int i = 0; i <= this.lstDoors.Items.Count - 1; i++)
				{
					if (this.lstDoors.Items[i].Text == ucMapDoor.doorName)
					{
						this.lstDoors.SelectedItems.Clear();
						this.lstDoors.Items[i].Selected = true;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x001BBB18 File Offset: 0x001BAB18
		private void UcMapDoor_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && this.cmdAddMap.Visible)
			{
				try
				{
					PictureBox bindSource = ((ucMapDoor)sender).bindSource;
					int num = bindSource.PointToClient(Control.MousePosition).Y - this.t;
					int num2 = bindSource.PointToClient(Control.MousePosition).X - this.l;
					(sender as Control).Top = num;
					(sender as Control).Left = num2;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x001BBBC4 File Offset: 0x001BABC4
		private void UcMapDoor_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && this.cmdAddMap.Visible)
			{
				try
				{
					((ucMapDoor)sender).doorLocation = ((ucMapDoor)sender).Location;
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x04002D55 RID: 11605
		private bool bAllow;

		// Token: 0x04002D56 RID: 11606
		private bool bDataErrorExist;

		// Token: 0x04002D57 RID: 11607
		private bool bEditing;

		// Token: 0x04002D58 RID: 11608
		private SqlConnection cn;

		// Token: 0x04002D59 RID: 11609
		private ucMapDoor currentUcMapDoor;

		// Token: 0x04002D5A RID: 11610
		private SqlDataAdapter da;

		// Token: 0x04002D5B RID: 11611
		private DataSet dstemp;

		// Token: 0x04002D5C RID: 11612
		private DataTable dt;

		// Token: 0x04002D5D RID: 11613
		private DataTable dtReader;

		// Token: 0x04002D5E RID: 11614
		private DataView dv;

		// Token: 0x04002D5F RID: 11615
		private DataView dvDoors;

		// Token: 0x04002D60 RID: 11616
		private DataView dvDoors4Watching;

		// Token: 0x04002D61 RID: 11617
		private DataView dvMap;

		// Token: 0x04002D62 RID: 11618
		private DataView dvMapDoor;

		// Token: 0x04002D63 RID: 11619
		private DataView dvMapDoors;

		// Token: 0x04002D64 RID: 11620
		private DataView dvSelected;

		// Token: 0x04002D65 RID: 11621
		private ImageList imgDoor2;

		// Token: 0x04002D66 RID: 11622
		private int l;

		// Token: 0x04002D67 RID: 11623
		private int lastlogInt;

		// Token: 0x04002D68 RID: 11624
		private Point lastMouseP;

		// Token: 0x04002D69 RID: 11625
		private TabPage mapPage;

		// Token: 0x04002D6A RID: 11626
		private Panel mapPanel;

		// Token: 0x04002D6B RID: 11627
		private PictureBox mapPicture;

		// Token: 0x04002D6D RID: 11629
		private int t;

		// Token: 0x04002D6E RID: 11630
		private ucMapDoor uc1door;

		// Token: 0x04002D6F RID: 11631
		private ArrayList arrZoomScale = new ArrayList();

		// Token: 0x04002D70 RID: 11632
		private ArrayList arrZoomScaleTabpageName = new ArrayList();

		// Token: 0x04002D71 RID: 11633
		public ListView lstDoors = new ListView();

		// Token: 0x04002D72 RID: 11634
		private byte[] photoImageData;

		// Token: 0x04002D73 RID: 11635
		private Dictionary<string, string> ReaderName = new Dictionary<string, string>();

		// Token: 0x04002D74 RID: 11636
		private string strcmdWatchAllMapsBk = "";

		// Token: 0x04002D75 RID: 11637
		private string strcmdWatchCurrentMapBk = "";

		// Token: 0x04002D76 RID: 11638
		public DataTable tbRunInfoLog;

		// Token: 0x04002D9A RID: 11674
		public ToolStripButton btnMonitor;

		// Token: 0x04002D9B RID: 11675
		public ToolStripButton btnStop;

		// Token: 0x04002D9C RID: 11676
		public ContextMenuStrip contextMenuStrip1Doors;
	}
}
