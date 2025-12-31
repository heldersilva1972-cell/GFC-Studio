using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000056 RID: 86
	public partial class frmWatchingLCD : frmN3000
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x000B239C File Offset: 0x000B139C
		public frmWatchingLCD()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000B2407 File Offset: 0x000B1407
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000B2409 File Offset: 0x000B1409
		private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000B240B File Offset: 0x000B140B
		private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_RGO_VALIDTIME", "");
			base.Close();
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000B2424 File Offset: 0x000B1424
		private void departmentOrderEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.strNewName = this.departmentOrderToolStripMenuItem.Text;
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.UpdateKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT_ORDER", dfrmInputNewName.strNewName);
					this.departmentOrderToolStripMenuItem.Text = wgAppConfig.GetKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT_ORDER");
					base.Close();
				}
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000B249C File Offset: 0x000B149C
		private void departmentOrderToolStripMenuItem_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000B24A0 File Offset: 0x000B14A0
		private void displayNextPage()
		{
			try
			{
				if (this.dataGridView1.RowCount > this.dataGridView1.DisplayedRowCount(false))
				{
					this.currentPage4Leader += this.dataGridView1.DisplayedRowCount(false);
					if (this.currentPage4Leader > this.dataGridView1.RowCount)
					{
						this.currentPage4Leader = 0;
					}
					this.dataGridView1.FirstDisplayedScrollingRowIndex = this.currentPage4Leader;
				}
				if (this.dgvRunInfo.RowCount > this.dgvRunInfo.DisplayedRowCount(false))
				{
					this.currentPage4Worker += this.dgvRunInfo.DisplayedRowCount(false);
					if (this.currentPage4Worker > this.dgvRunInfo.RowCount)
					{
						this.currentPage4Worker = 0;
					}
					this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.currentPage4Worker;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				this.currentPage4Leader = 0;
				this.currentPage4Worker = 0;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000B2598 File Offset: 0x000B1598
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

		// Token: 0x06000668 RID: 1640 RVA: 0x000B2650 File Offset: 0x000B1650
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

		// Token: 0x06000669 RID: 1641 RVA: 0x000B27B0 File Offset: 0x000B17B0
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000B27B8 File Offset: 0x000B17B8
		private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000B27BC File Offset: 0x000B17BC
		private void frmWatchingMoreRecords_FormClosing(object sender, FormClosingEventArgs e)
		{
			wgAppConfig.DisposeImage(this.pictureBox1.Image);
			wgAppConfig.DisposeImage(this.pictureBox2.Image);
			wgAppConfig.DisposeImage(this.pictureBox3.Image);
			wgAppConfig.DisposeImage(this.pictureBox4.Image);
			wgAppConfig.DisposeImage(this.pictureBox5.Image);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000B281C File Offset: 0x000B181C
		private void frmWatchingMoreRecords_Load(object sender, EventArgs e)
		{
			try
			{
				string text;
				string text2;
				string text3;
				wgAppConfig.getSystemParamValue(36, out text, out text2, out text3);
				if (!string.IsNullOrEmpty(text2))
				{
					this.lblCompanyName.Text = text2;
				}
				this.groupBox6.Text = "";
				this.lblZone.Text = "";
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_RGO_VALIDTIME")))
				{
					this.bSpecialStyleA = wgAppConfig.GetKeyVal("KEY_RGO_VALIDTIME") == "1";
				}
				this.rGOValidTimeToolStripMenuItem.Checked = this.bSpecialStyleA;
				this.defaultToolStripMenuItem.Checked = !this.bSpecialStyleA;
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT_ORDER")))
				{
					this.departmentOrderToolStripMenuItem.Text = wgAppConfig.GetKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT_ORDER");
				}
				this.onlyDisplayDepartmentToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT").Equals("1");
				if (wgAppConfig.IsAccessControlBlue)
				{
					this.onlyDisplayDepartmentToolStripMenuItem.Visible = false;
					this.onlyDisplayDepartmentToolStripMenuItem.Checked = false;
					this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem.Visible = false;
				}
				this.optionForUser20191008ToolStripMenuItem.Checked = wgAppConfig.GetKeyVal("KEY_WATCHINGLCD_optionForUser20191008").Equals("1");
				if (this.optionForUser20191008ToolStripMenuItem.Checked)
				{
					this.lblCompanyName.Font = new Font("宋体", 56f, FontStyle.Bold, this.lblCompanyName.Font.Unit);
					this.dgvRunInfo.Columns[1].HeaderText = "部门1";
					this.dataGridView1.Columns[1].HeaderText = "部门2";
					this.panel5.Visible = false;
					this.flowLayoutPanel1.BackColor = Color.Black;
					this.flowLayoutPanel2.BackColor = Color.Black;
					this.flowLayoutPanel3.BackColor = Color.Black;
					this.panel1.BackColor = Color.Black;
					this.panel2.BackColor = Color.Black;
					this.panel3.BackColor = Color.Black;
					this.lblCompanyName.BackColor = Color.Black;
					this.dgvRunInfo.BackgroundColor = Color.Black;
					this.dataGridView1.BackgroundColor = Color.Black;
					this.dataGridView1.DefaultCellStyle.BackColor = Color.Black;
					this.dgvRunInfo.DefaultCellStyle.BackColor = Color.Black;
					this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = Color.Black;
					this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Black;
					this.dgvRunInfo.RowTemplate.DefaultCellStyle.BackColor = Color.Black;
					this.dgvRunInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Black;
				}
				if (this.onlyDisplayDepartmentToolStripMenuItem.Checked)
				{
					this.dgvRunInfo.Columns[1].HeaderText = wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment);
					this.splitter3.BackColor = Color.Blue;
					this.panel4.BackColor = Color.Blue;
					this.dgvRunInfo.BorderStyle = BorderStyle.None;
					base.FormBorderStyle = FormBorderStyle.None;
					base.WindowState = FormWindowState.Maximized;
					this.panel4.Visible = false;
					this.splitter3.Visible = false;
					this.panel3.Height = this.panel2.Height;
					this.onlyDisplayDepartmentToolStripMenuItem.Text = wgAppConfig.ReplaceFloorRoom(this.onlyDisplayDepartmentToolStripMenuItem.Text);
					this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem.Text = wgAppConfig.ReplaceFloorRoom(this.departmentOrderWhenOnlyDisplayDepartentToolStripMenuItem.Text);
				}
				else if (wgAppConfig.GetKeyVal("AutoLoginMode") == "5")
				{
					base.FormBorderStyle = FormBorderStyle.None;
					base.WindowState = FormWindowState.Maximized;
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
					if (this.bDisplayCapturedPhoto)
					{
						this.groupBox6.Visible = true;
					}
					else
					{
						this.groupBox6.Visible = false;
					}
					this.flowLayoutPanel1.Visible = false;
					this.richTextBox6.Visible = false;
					this.lstSwipes_RowsAdded(null, null);
					this.frmWatchingMoreRecords_SizeChanged(null, null);
					if (num > 0)
					{
						for (int i = 0; i < num; i++)
						{
							this.enlargeInfoDisplayToolStripMenuItem_Click(null, null);
						}
					}
					else if (num < 0)
					{
						for (int j = 0; j < -num; j++)
						{
							this.ReduceInfoDisplaytoolStripMenuItem_Click(null, null);
						}
					}
					if (num2 > 0)
					{
						for (int k = 0; k < num2; k++)
						{
							this.enlargeFontToolStripMenuItem1_Click(null, null);
						}
					}
					else if (num2 < 0)
					{
						for (int l = 0; l < -num2; l++)
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

		// Token: 0x0600066D RID: 1645 RVA: 0x000B2DB8 File Offset: 0x000B1DB8
		private void frmWatchingMoreRecords_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000B2DBC File Offset: 0x000B1DBC
		private void getGroupInfo()
		{
			try
			{
				if (this.bStarted)
				{
					this.bStarted = false;
					this.lblZone.Text = "";
					if (this.frmCall.frm4ShowPersonsInside.cboZone.SelectedIndex > 0)
					{
						this.lblZone.Text = this.frmCall.frm4ShowPersonsInside.cboZone.Text;
					}
					this.dtDealingMainGroup = new DataTable("MainGroup");
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.Int32");
					dataColumn.ColumnName = "f_GroupID";
					this.dtDealingMainGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupName";
					this.dtDealingMainGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.Int32");
					dataColumn.ColumnName = "f_Inside";
					this.dtDealingMainGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupNameDisplay";
					this.dtDealingMainGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupIDs";
					this.dtDealingMainGroup.Columns.Add(dataColumn);
					this.dtDealingMainGroup.AcceptChanges();
					this.dtDealingSubGroup = new DataTable("SubGroup");
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.Int32");
					dataColumn.ColumnName = "f_GroupID";
					this.dtDealingSubGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupName";
					this.dtDealingSubGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.Int32");
					dataColumn.ColumnName = "f_Inside";
					this.dtDealingSubGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupNameDisplay";
					this.dtDealingSubGroup.Columns.Add(dataColumn);
					dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType("System.String");
					dataColumn.ColumnName = "f_GroupIDs";
					this.dtDealingSubGroup.Columns.Add(dataColumn);
					this.dtDealingSubGroup.AcceptChanges();
					DataView dataView = new DataView(this.dtDealingSubGroup);
					this.dvDealingMainGroup = new DataView(this.dtDealingMainGroup);
					this.dvDealingSubGroup = new DataView(this.dtDealingSubGroup);
					this.dgvRunInfo.AutoGenerateColumns = false;
					this.dgvRunInfo.DataSource = this.dvDealingMainGroup;
					this.dgvRunInfo.Columns[0].DataPropertyName = "f_GroupID";
					this.dgvRunInfo.Columns[1].DataPropertyName = "f_GroupNameDisplay";
					this.dgvRunInfo.Columns[2].DataPropertyName = "f_Inside";
					this.dgvRunInfo.Columns[3].DataPropertyName = "f_GroupName";
					this.dgvRunInfo.Columns[4].DataPropertyName = "f_GroupIDs";
					this.dataGridView1.AutoGenerateColumns = false;
					this.dataGridView1.DataSource = this.dvDealingSubGroup;
					this.dataGridView1.Columns[0].DataPropertyName = "f_GroupID";
					this.dataGridView1.Columns[1].DataPropertyName = "f_GroupNameDisplay";
					this.dataGridView1.Columns[2].DataPropertyName = "f_Inside";
					this.dataGridView1.Columns[3].DataPropertyName = "f_GroupName";
					this.dataGridView1.Columns[4].DataPropertyName = "f_GroupIDs";
					this.dvDealingMainGroup = new DataView(this.dtDealingMainGroup);
					this.dvDealingSubGroup = new DataView(this.dtDealingSubGroup);
					if (this.onlyDisplayDepartmentToolStripMenuItem.Checked)
					{
						this.dvDealingMainGroup.Sort = "f_GroupNameDisplay";
						this.dvDealingSubGroup.Sort = "f_GroupNameDisplay";
					}
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < this.dtDealing.Rows.Count; i++)
					{
						if ((int)this.dtDealing.Rows[i]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[i]["f_GroupName"]).IndexOf(CommonStr.strDepartmentTempCard) < 0)
						{
							string text = (string)this.dtDealing.Rows[i]["f_GroupName"];
							int num3 = text.Length - text.Replace("\\", "").Length;
							if (num2 < num3)
							{
								num2 = num3;
							}
						}
					}
					if (num2 > 0)
					{
						num = num2 - 1;
					}
					bool flag = false;
					string[] array = new string[] { "小", "中", "大" };
					if (this.onlyDisplayDepartmentToolStripMenuItem.Checked)
					{
						ArrayList arrayList = new ArrayList();
						string[] array2;
						if (string.IsNullOrEmpty(this.departmentOrderToolStripMenuItem.Text))
						{
							array2 = new string[] { "" };
						}
						else
						{
							array2 = this.departmentOrderToolStripMenuItem.Text.Split(new char[] { ',' });
						}
						for (int j = 0; j < array2.Length; j++)
						{
							if (!string.IsNullOrEmpty(array2[j]))
							{
								for (int k = 0; k < this.dtDealing.Rows.Count; k++)
								{
									if ((int)this.dtDealing.Rows[k]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[k]["f_GroupName"]).IndexOf(array2[j]) == 0)
									{
										flag = true;
										arrayList.Add((int)this.dtDealing.Rows[k]["f_GroupID"]);
										string text = (string)this.dtDealing.Rows[k]["f_GroupName"];
										int num4 = text.Length - text.Replace("\\", "").Length;
										if (num4 == num)
										{
											DataRow dataRow = this.dtDealingMainGroup.NewRow();
											dataRow["f_GroupID"] = this.dtDealing.Rows[k]["f_GroupID"];
											dataRow["f_GroupName"] = text;
											dataRow["f_GroupNameDisplay"] = text;
											if (num > 0)
											{
												dataRow["f_GroupNameDisplay"] = text.Substring(text.LastIndexOf("\\") + 1);
											}
											dataRow["f_GroupIDs"] = dataRow["f_GroupID"];
											this.dtDealingMainGroup.Rows.Add(dataRow);
											this.dtDealingMainGroup.AcceptChanges();
										}
									}
								}
							}
						}
						if (arrayList.Count != this.dtDealing.Rows.Count)
						{
							for (int l = 0; l < this.dtDealing.Rows.Count; l++)
							{
								if ((int)this.dtDealing.Rows[l]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[l]["f_GroupName"]).IndexOf(CommonStr.strDepartmentTempCard) < 0 && arrayList.IndexOf((int)this.dtDealing.Rows[l]["f_GroupID"]) < 0)
								{
									flag = true;
									string text = (string)this.dtDealing.Rows[l]["f_GroupName"];
									int num4 = text.Length - text.Replace("\\", "").Length;
									if (num4 == num)
									{
										DataRow dataRow2 = this.dtDealingMainGroup.NewRow();
										dataRow2["f_GroupID"] = this.dtDealing.Rows[l]["f_GroupID"];
										dataRow2["f_GroupName"] = text;
										dataRow2["f_GroupNameDisplay"] = text;
										if (num > 0)
										{
											dataRow2["f_GroupNameDisplay"] = text.Substring(text.LastIndexOf("\\") + 1);
										}
										dataRow2["f_GroupIDs"] = dataRow2["f_GroupID"];
										this.dtDealingMainGroup.Rows.Add(dataRow2);
										this.dtDealingMainGroup.AcceptChanges();
									}
								}
							}
						}
					}
					if (!flag)
					{
						for (int m = 0; m < this.dtDealing.Rows.Count; m++)
						{
							if ((int)this.dtDealing.Rows[m]["f_GroupID"] > 0 && ((string)this.dtDealing.Rows[m]["f_GroupName"]).IndexOf(CommonStr.strDepartmentTempCard) < 0)
							{
								string text2 = "-2";
								string text = (string)this.dtDealing.Rows[m]["f_GroupName"];
								int num4 = text.Length - text.Replace("\\", "").Length;
								if (num4 == num)
								{
									DataRow dataRow3 = this.dtDealingMainGroup.NewRow();
									dataRow3["f_GroupID"] = this.dtDealing.Rows[m]["f_GroupID"];
									dataRow3["f_GroupName"] = text;
									dataRow3["f_GroupNameDisplay"] = text;
									if (num > 0)
									{
										dataRow3["f_GroupNameDisplay"] = text.Substring(text.LastIndexOf("\\") + 1);
									}
									dataRow3["f_GroupIDs"] = dataRow3["f_GroupID"];
									this.dtDealingMainGroup.Rows.Add(dataRow3);
									this.dtDealingMainGroup.AcceptChanges();
								}
								else if (num4 == num + 1)
								{
									DataRow dataRow4 = this.dtDealingMainGroup.Rows[this.dtDealingMainGroup.Rows.Count - 1];
									dataRow4["f_GroupIDs"] = string.Format("{0},{1}", (string)dataRow4["f_GroupIDs"], ((int)this.dtDealing.Rows[m]["f_GroupID"]).ToString());
									this.dtDealingMainGroup.AcceptChanges();
									string text3 = text.Substring(text.LastIndexOf("\\") + 1);
									dataView.RowFilter = "f_GroupName = " + wgTools.PrepareStr(text3);
									if (dataView.Count == 0)
									{
										dataRow4 = this.dtDealingSubGroup.NewRow();
										dataRow4["f_GroupID"] = this.dtDealing.Rows[m]["f_GroupID"];
										dataRow4["f_GroupName"] = text3;
										dataRow4["f_GroupNameDisplay"] = text3;
										dataRow4["f_GroupIDs"] = text2;
										this.dtDealingSubGroup.Rows.Add(dataRow4);
										this.dtDealingSubGroup.AcceptChanges();
									}
									if (dataView.Count > 0)
									{
										dataView[0]["f_GroupIDs"] = string.Format("{0},{1}", dataView[0]["f_GroupIDs"], ((int)this.dtDealing.Rows[m]["f_GroupID"]).ToString());
										this.dtDealingSubGroup.AcceptChanges();
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

		// Token: 0x0600066F RID: 1647 RVA: 0x000B3A60 File Offset: 0x000B2A60
		private void groupBox4_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000B3A62 File Offset: 0x000B2A62
		private void groupBox6_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(this.groupBox6.BackColor);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000B3A7A File Offset: 0x000B2A7A
		private void label1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x000B3A7C File Offset: 0x000B2A7C
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x000B3A80 File Offset: 0x000B2A80
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

		// Token: 0x06000674 RID: 1652 RVA: 0x000B3AEC File Offset: 0x000B2AEC
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

		// Token: 0x06000675 RID: 1653 RVA: 0x000B3B58 File Offset: 0x000B2B58
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
					num = 3;
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
										goto IL_027F;
									}
									catch (Exception ex)
									{
										wgAppConfig.wgLog(ex.ToString());
										goto IL_027F;
									}
								}
								this.loadPhoto(mjRec.CardID, ref this.picBox[num]);
								IL_027F:
								this.txtB[num].Text = text;
								this.txtB[num].Font = new Font("宋体", this.InfoFontSize, FontStyle.Bold, this.txtB[num].Font.Unit);
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
								if (this.bSpecialStyleA)
								{
									try
									{
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
								if (!wgAppConfig.IsActivateCameraManage)
								{
									break;
								}
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
									break;
								}
								this.needCheckFileName = text4;
								this.pictureBox6.Visible = false;
								break;
							}
						}
					}
					this.richTextBox1.Text = this.txtB[0].Text;
				}
				base.ResumeLayout();
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000B4134 File Offset: 0x000B3134
		private void lstSwipes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			this.richTextBox1.Text = "";
			this.richTextBox2.Text = "";
			this.richTextBox3.Text = "";
			this.richTextBox4.Text = "";
			this.richTextBox5.Text = "";
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

		// Token: 0x06000677 RID: 1655 RVA: 0x000B4224 File Offset: 0x000B3224
		private void onlyDisplayDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_WATCHINGLCD_ONLY_DEPARTMENT", this.onlyDisplayDepartmentToolStripMenuItem.Checked ? "0" : "1");
			base.Close();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000B424F File Offset: 0x000B324F
		private void optionForUser20191008ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_WATCHINGLCD_optionForUser20191008", this.optionForUser20191008ToolStripMenuItem.Checked ? "0" : "1");
			base.Close();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000B427A File Offset: 0x000B327A
		private void otherToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000B427C File Offset: 0x000B327C
		private void pictureBox4_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000B427E File Offset: 0x000B327E
		private void pictureBox7_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000B4280 File Offset: 0x000B3280
		public void ReallyCloseForm()
		{
			wgAppConfig.DisposeImage(this.pictureBox1.Image);
			wgAppConfig.DisposeImage(this.pictureBox2.Image);
			wgAppConfig.DisposeImage(this.pictureBox3.Image);
			wgAppConfig.DisposeImage(this.pictureBox4.Image);
			wgAppConfig.DisposeImage(this.pictureBox5.Image);
			base.Close();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000B42E4 File Offset: 0x000B32E4
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

		// Token: 0x0600067E RID: 1662 RVA: 0x000B43B0 File Offset: 0x000B33B0
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

		// Token: 0x0600067F RID: 1663 RVA: 0x000B4504 File Offset: 0x000B3504
		private void refreshData()
		{
			try
			{
				if (this.frmCall.frm4ShowPersonsInside != null && this.frmCall.frm4ShowPersonsInside.dtGroupsShare != null)
				{
					DataView dataView;
					lock (this.frmCall.frm4ShowPersonsInside.dtGroupsShare)
					{
						dataView = new DataView(this.frmCall.frm4ShowPersonsInside.dtGroupsShare);
						new DataView(this.frmCall.frm4ShowPersonsInside.dtGroupsShare);
						new DataView(this.frmCall.frm4ShowPersonsInside.dtGroupsShare);
						this.dtDealing = this.frmCall.frm4ShowPersonsInside.dtGroupsShare.Copy();
					}
					if (this.rowsLastCnt != dataView.Count)
					{
						this.bStarted = true;
						this.rowsLastCnt = dataView.Count;
					}
					if (dataView.Count >= 1)
					{
						dataView.RowFilter = string.Format("f_GroupName like '%{0}%'", CommonStr.strDepartmentTempCard);
						if (dataView.Count > 0)
						{
							this.lblTempCardTotal.Text = string.Format("临时卡人数\r\n{0}", dataView[0]["f_Inside"]);
							this.lblTempCardTotal.Visible = true;
						}
						else
						{
							this.lblTempCardTotal.Visible = false;
						}
						dataView.RowFilter = "f_GroupID >0";
						for (int i = 0; i < this.dtDealing.Rows.Count; i++)
						{
							int num = (int)this.dtDealing.Rows[i]["f_GroupID"];
							if (num == -1)
							{
								if (string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_strtotalUsersIndoors")))
								{
									this.lblAllTotal.Text = string.Format("当前在场人数\r\n{0}", this.dtDealing.Rows[i]["f_Inside"]);
								}
								else
								{
									this.lblAllTotal.Text = wgAppConfig.GetKeyVal("KEY_strtotalUsersIndoors") + string.Format("\r\n{0}", this.dtDealing.Rows[i]["f_Inside"]);
								}
							}
						}
						this.getGroupInfo();
						for (int j = 0; j < this.dtDealingMainGroup.Rows.Count; j++)
						{
							dataView.RowFilter = string.Format("f_GroupID IN ({0})", this.dtDealingMainGroup.Rows[j]["f_GroupIDs"]);
							int num2 = 0;
							for (int k = 0; k < dataView.Count; k++)
							{
								num2 += (int)dataView[k]["f_Inside"];
							}
							this.dtDealingMainGroup.Rows[j]["f_Inside"] = num2;
						}
						this.dtDealingMainGroup.AcceptChanges();
						for (int l = 0; l < this.dtDealingSubGroup.Rows.Count; l++)
						{
							dataView.RowFilter = string.Format("f_GroupID IN ({0})", this.dtDealingSubGroup.Rows[l]["f_GroupIDs"]);
							int num3 = 0;
							for (int m = 0; m < dataView.Count; m++)
							{
								num3 += (int)dataView[m]["f_Inside"];
							}
							this.dtDealingSubGroup.Rows[l]["f_Inside"] = num3;
						}
						this.dtDealingSubGroup.AcceptChanges();
						this.dgvRunInfo.ClearSelection();
						this.dataGridView1.ClearSelection();
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000B48D8 File Offset: 0x000B38D8
		private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("WatchingMoreRecords_Display", "");
			base.Close();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000B48EF File Offset: 0x000B38EF
		private void rGOValidTimeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			wgAppConfig.UpdateKeyVal("KEY_RGO_VALIDTIME", "1");
			base.Close();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000B4906 File Offset: 0x000B3906
		private void richTextBox6_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000B4908 File Offset: 0x000B3908
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

		// Token: 0x06000684 RID: 1668 RVA: 0x000B4A44 File Offset: 0x000B3A44
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			try
			{
				this.lblTime.Text = DateTime.Now.ToString("yyyy-MM-dd\r\ndddd\r\nHH:mm:ss\r\n ");
				if (this.tbRunInfoLog != null && this.lastCnt != this.tbRunInfoLog.Rows.Count)
				{
					this.lstSwipes_RowsAdded(null, null);
					this.refreshTimer = this.refreshTimeout;
				}
			}
			catch
			{
			}
			if (this.frmCall.frm4ShowPersonsInside != null && this.frmCall.frm4ShowPersonsInside.dataNeedRefresh4Lcd)
			{
				this.frmCall.frm4ShowPersonsInside.dataNeedRefresh4Lcd = false;
				this.refreshTimer = this.refreshTimeout;
			}
			this.refreshTimer++;
			if (this.refreshTimer >= this.refreshTimeout)
			{
				this.refreshTimer = 0;
				this.refreshData();
				if (this.currentPage4Leader < this.dataGridView1.RowCount)
				{
					this.dataGridView1.FirstDisplayedScrollingRowIndex = this.currentPage4Leader;
				}
				if (this.currentPage4Worker < this.dgvRunInfo.RowCount)
				{
					this.dgvRunInfo.FirstDisplayedScrollingRowIndex = this.currentPage4Worker;
				}
			}
			this.refreshPageTimer++;
			if (this.refreshPageTimer >= this.refreshPageTimeout)
			{
				this.refreshPageTimer = 0;
				this.displayNextPage();
			}
			this.timer1.Enabled = true;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000B4BA8 File Offset: 0x000B3BA8
		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x04000BA4 RID: 2980
		private bool bSpecialStyleA;

		// Token: 0x04000BA5 RID: 2981
		private int currentPage4Leader;

		// Token: 0x04000BA6 RID: 2982
		private int currentPage4Worker;

		// Token: 0x04000BA7 RID: 2983
		private DataTable dtDealing;

		// Token: 0x04000BA8 RID: 2984
		private DataTable dtDealingMainGroup;

		// Token: 0x04000BA9 RID: 2985
		private DataTable dtDealingSubGroup;

		// Token: 0x04000BAA RID: 2986
		private DataView dvDealingMainGroup;

		// Token: 0x04000BAB RID: 2987
		private DataView dvDealingSubGroup;

		// Token: 0x04000BAC RID: 2988
		private int infoSizeChange;

		// Token: 0x04000BAD RID: 2989
		private int richTextSizeChange;

		// Token: 0x04000BAE RID: 2990
		public bool bDisplayCapturedPhoto;

		// Token: 0x04000BAF RID: 2991
		private bool bStarted = true;

		// Token: 0x04000BB0 RID: 2992
		public frmConsole frmCall;

		// Token: 0x04000BB1 RID: 2993
		public int groupMax = 3;

		// Token: 0x04000BB2 RID: 2994
		private GroupBox[] grp;

		// Token: 0x04000BB3 RID: 2995
		public float InfoFontSize = 15f;

		// Token: 0x04000BB4 RID: 2996
		private int lastCnt = -1;

		// Token: 0x04000BB5 RID: 2997
		private string needCheckFileName = "";

		// Token: 0x04000BB6 RID: 2998
		private PictureBox[] picBox;

		// Token: 0x04000BB7 RID: 2999
		private int refreshPageTimeout = 16;

		// Token: 0x04000BB8 RID: 3000
		private int refreshPageTimer = 16;

		// Token: 0x04000BB9 RID: 3001
		private int refreshTimeout = 10;

		// Token: 0x04000BBA RID: 3002
		private int refreshTimer = 10;

		// Token: 0x04000BBB RID: 3003
		private int rowsLastCnt = -1;

		// Token: 0x04000BBC RID: 3004
		public DataTable tbRunInfoLog;

		// Token: 0x04000BBD RID: 3005
		private RichTextBox[] txtB;
	}
}
