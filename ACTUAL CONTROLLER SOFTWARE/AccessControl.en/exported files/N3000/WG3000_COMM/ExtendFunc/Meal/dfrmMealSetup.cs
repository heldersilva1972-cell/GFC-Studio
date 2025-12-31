using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F9 RID: 761
	public partial class dfrmMealSetup : frmN3000
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x001C0C44 File Offset: 0x001BFC44
		public dfrmMealSetup()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvOptional);
			wgAppConfig.custDataGridview(ref this.dgvSelected);
			this.tabPage1.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage2.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
			this.tabPage3.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x001C0CD4 File Offset: 0x001BFCD4
		private void btnAddAllReaders_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 1;
				}
				this.dt.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x001C0D50 File Offset: 0x001BFD50
		private void btnAddOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.selectObject(this.dgvOptional);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x001C0D60 File Offset: 0x001BFD60
		private void btnByDepartment_Click(object sender, EventArgs e)
		{
			using (dfrmMealGroupConfigure dfrmMealGroupConfigure = new dfrmMealGroupConfigure())
			{
				dfrmMealGroupConfigure.ShowDialog(this);
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x001C0D98 File Offset: 0x001BFD98
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x001C0DA0 File Offset: 0x001BFDA0
		private void btnDeleteAllReaders_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < this.dt.Rows.Count; i++)
				{
					this.dt.Rows[i]["f_Selected"] = 0;
				}
				this.dt.AcceptChanges();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x001C0E1C File Offset: 0x001BFE1C
		private void btnDeleteOneReader_Click(object sender, EventArgs e)
		{
			wgAppConfig.deselectObject(this.dgvSelected);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x001C0E2C File Offset: 0x001BFE2C
		private void btnLimitPersonByGroups_Click(object sender, EventArgs e)
		{
			using (dfrmMealGroupLimitConfigure dfrmMealGroupLimitConfigure = new dfrmMealGroupLimitConfigure())
			{
				dfrmMealGroupLimitConfigure.ShowDialog(this);
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x001C0E64 File Offset: 0x001BFE64
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				if (this.chkMorningMeal.Checked)
				{
					text = this.dateBeginHMS1.Value.ToString("HH:mm");
					text2 = this.dateEndHMS1.Value.ToString("HH:mm");
					text3 = this.dateBeginHMS1.Value.ToString("HH:mm");
					text4 = this.dateEndHMS1.Value.ToString("HH:mm");
					if (this.dateBeginHMS1.Value > this.dateEndHMS1.Value)
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strWrongTimeSegment,
							"\r\n\r\n",
							this.dateBeginHMS1.Value.ToString("HH:mm"),
							"\r\n\r\n",
							this.dateEndHMS1.Value.ToString("HH:mm")
						}));
						return;
					}
				}
				if (this.chkLunchMeal.Checked)
				{
					if (this.dateBeginHMS2.Value > this.dateEndHMS2.Value)
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strWrongTimeSegment,
							"\r\n\r\n",
							this.dateBeginHMS2.Value.ToString("HH:mm"),
							"\r\n\r\n",
							this.dateEndHMS2.Value.ToString("HH:mm")
						}));
						return;
					}
					if (text4 != "" && string.Compare(this.dateBeginHMS2.Value.ToString("HH:mm"), text4) < 0)
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strWrongTimeSegment,
							"\r\n\r\n",
							this.dateBeginHMS2.Value.ToString("HH:mm"),
							"\r\n\r\n",
							text4
						}));
						return;
					}
					if (text == "")
					{
						text = this.dateBeginHMS2.Value.ToString("HH:mm");
					}
					if (text2 == "")
					{
						text2 = this.dateEndHMS2.Value.ToString("HH:mm");
					}
					text3 = this.dateBeginHMS2.Value.ToString("HH:mm");
					text4 = this.dateEndHMS2.Value.ToString("HH:mm");
				}
				if (this.chkEveningMeal.Checked)
				{
					if (this.dateBeginHMS3.Value > this.dateEndHMS3.Value)
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strWrongTimeSegment,
							"\r\n\r\n",
							this.dateBeginHMS3.Value.ToString("HH:mm"),
							"\r\n\r\n",
							this.dateEndHMS3.Value.ToString("HH:mm")
						}));
						return;
					}
					if (text4 != "" && string.Compare(this.dateBeginHMS3.Value.ToString("HH:mm"), text4) < 0)
					{
						XMessageBox.Show(string.Concat(new string[]
						{
							CommonStr.strWrongTimeSegment,
							"\r\n\r\n",
							this.dateBeginHMS3.Value.ToString("HH:mm"),
							"\r\n\r\n",
							text4
						}));
						return;
					}
					if (text == "")
					{
						text = this.dateBeginHMS3.Value.ToString("HH:mm");
					}
					if (text2 == "")
					{
						text2 = this.dateEndHMS3.Value.ToString("HH:mm");
					}
					text3 = this.dateBeginHMS3.Value.ToString("HH:mm");
					text4 = this.dateEndHMS3.Value.ToString("HH:mm");
				}
				if (this.chkOtherMeal.Checked && text != "")
				{
					if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), text4) > 0)
					{
						if (string.Compare(this.dateEndHMS4.Value.ToString("HH:mm"), text) >= 0 && this.dateEndHMS4.Value < this.dateBeginHMS4.Value)
						{
							XMessageBox.Show(string.Concat(new string[]
							{
								CommonStr.strWrongTimeSegment,
								"\r\n\r\n",
								this.dateEndHMS4.Value.ToString("HH:mm"),
								"\r\n\r\n",
								text
							}));
							return;
						}
					}
					else
					{
						if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), text) >= 0)
						{
							if (string.Compare(this.dateBeginHMS4.Value.ToString("HH:mm"), text3) >= 0)
							{
								XMessageBox.Show(string.Concat(new string[]
								{
									CommonStr.strWrongTimeSegment,
									"\r\n\r\n",
									this.dateBeginHMS4.Value.ToString("HH:mm"),
									"\r\n\r\n",
									text4
								}));
							}
							else
							{
								XMessageBox.Show(string.Concat(new string[]
								{
									CommonStr.strWrongTimeSegment,
									"\r\n\r\n",
									this.dateBeginHMS4.Value.ToString("HH:mm"),
									"\r\n\r\n",
									text
								}));
							}
							return;
						}
						if (string.Compare(this.dateEndHMS4.Value.ToString("HH:mm"), text) >= 0)
						{
							XMessageBox.Show(string.Concat(new string[]
							{
								CommonStr.strWrongTimeSegment,
								"\r\n\r\n",
								this.dateEndHMS4.Value.ToString("HH:mm"),
								"\r\n\r\n",
								text
							}));
							return;
						}
					}
					text3 = this.dateBeginHMS4.Value.ToString("HH:mm");
					text4 = this.dateEndHMS4.Value.ToString("HH:mm");
				}
				Cursor cursor = Cursor.Current;
				string text6;
				if (this.dvSelected.Count > 0)
				{
					string text5 = "";
					for (int i = 0; i <= this.dvSelected.Count - 1; i++)
					{
						if (text5 == "")
						{
							text5 += this.dvSelected[i]["f_ReaderID"];
						}
						else
						{
							text5 = text5 + "," + this.dvSelected[i]["f_ReaderID"];
						}
					}
					wgAppConfig.runUpdateSql(string.Format("DELETE FROM t_d_Reader4Meal WHERE f_ReaderID NOT IN ({0})", text5));
					wgAppConfig.runUpdateSql(string.Format("INSERT INTO t_d_Reader4Meal (f_ReaderID) SELECT f_ReaderID from t_b_Reader WHERE f_ReaderID  IN ({0}) AND f_ReaderID NOT IN (SELECT f_ReaderID From t_d_Reader4Meal)  ", text5));
				}
				else
				{
					text6 = " DELETE FROM t_d_Reader4Meal ";
					wgAppConfig.runUpdateSql(text6);
				}
				int num = 60;
				int num2 = 0;
				if (this.radioButton1.Checked)
				{
					num2 = 0;
				}
				if (this.radioButton2.Checked)
				{
					num2 = 1;
				}
				if (this.radioButton3.Checked)
				{
					num2 = 2;
					num = (int)this.nudRuleSeconds.Value;
				}
				wgAppConfig.runUpdateSql(string.Concat(new object[] { "UPDATE t_b_MealSetup SET f_Value = ", num2, ", f_ParamVal=", num, " WHERE f_ID=1 " }));
				text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=2 ";
				if (this.chkMorningMeal.Checked)
				{
					text6 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
					text6 = string.Concat(new object[]
					{
						text6,
						",f_BeginHMS =",
						wgTools.PrepareStr(this.dateBeginHMS1.Value, true, "HH:mm"),
						",f_EndHMS =",
						wgTools.PrepareStr(this.dateEndHMS1.Value, true, "HH:mm"),
						", f_ParamVal= ",
						this.nudMorning.Value,
						" WHERE f_ID=2 "
					});
				}
				wgAppConfig.runUpdateSql(text6);
				text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=3 ";
				if (this.chkLunchMeal.Checked)
				{
					text6 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
					text6 = string.Concat(new object[]
					{
						text6,
						",f_BeginHMS =",
						wgTools.PrepareStr(this.dateBeginHMS2.Value, true, "HH:mm"),
						",f_EndHMS =",
						wgTools.PrepareStr(this.dateEndHMS2.Value, true, "HH:mm"),
						", f_ParamVal= ",
						this.nudLunch.Value,
						" WHERE f_ID=3 "
					});
				}
				wgAppConfig.runUpdateSql(text6);
				text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=4 ";
				if (this.chkEveningMeal.Checked)
				{
					text6 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
					text6 = string.Concat(new object[]
					{
						text6,
						",f_BeginHMS =",
						wgTools.PrepareStr(this.dateBeginHMS3.Value, true, "HH:mm"),
						",f_EndHMS =",
						wgTools.PrepareStr(this.dateEndHMS3.Value, true, "HH:mm"),
						", f_ParamVal= ",
						this.nudEvening.Value,
						" WHERE f_ID=4 "
					});
				}
				wgAppConfig.runUpdateSql(text6);
				text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=5 ";
				if (this.chkOtherMeal.Checked)
				{
					text6 = "UPDATE t_b_MealSetup SET f_Value = 1 ";
					text6 = string.Concat(new object[]
					{
						text6,
						",f_BeginHMS =",
						wgTools.PrepareStr(this.dateBeginHMS4.Value, true, "HH:mm"),
						",f_EndHMS =",
						wgTools.PrepareStr(this.dateEndHMS4.Value, true, "HH:mm"),
						", f_ParamVal= ",
						this.nudOther.Value,
						" WHERE f_ID=5 "
					});
				}
				wgAppConfig.runUpdateSql(text6);
				if (wgAppConfig.IsAccessDB)
				{
					OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
					OleDbCommand oleDbCommand = new OleDbCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=6 ", oleDbConnection);
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
					bool flag = !oleDbDataReader.HasRows;
					oleDbDataReader.Close();
					oleDbConnection.Close();
					if (flag)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(6,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=6 ";
					if (this.chkAllowableSwipe.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=6 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				else
				{
					SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
					SqlCommand sqlCommand = new SqlCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=6 ", sqlConnection);
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
					bool flag2 = !sqlDataReader.HasRows;
					sqlDataReader.Close();
					sqlConnection.Close();
					if (flag2)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(6,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=6 ";
					if (this.chkAllowableSwipe.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=6 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				if (wgAppConfig.IsAccessDB)
				{
					OleDbConnection oleDbConnection2 = new OleDbConnection(wgAppConfig.dbConString);
					OleDbCommand oleDbCommand2 = new OleDbCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=7 ", oleDbConnection2);
					oleDbConnection2.Open();
					OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader(CommandBehavior.Default);
					bool flag3 = !oleDbDataReader2.HasRows;
					oleDbDataReader2.Close();
					oleDbConnection2.Close();
					if (flag3)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(7,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=7 ";
					if (this.chkByGroup.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=7 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				else
				{
					SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString);
					SqlCommand sqlCommand2 = new SqlCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=7 ", sqlConnection2);
					sqlConnection2.Open();
					SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader(CommandBehavior.Default);
					bool flag4 = !sqlDataReader2.HasRows;
					sqlDataReader2.Close();
					sqlConnection2.Close();
					if (flag4)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(7,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=7 ";
					if (this.chkByGroup.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=7 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				if (wgAppConfig.IsAccessDB)
				{
					OleDbConnection oleDbConnection3 = new OleDbConnection(wgAppConfig.dbConString);
					OleDbCommand oleDbCommand3 = new OleDbCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=8 ", oleDbConnection3);
					oleDbConnection3.Open();
					OleDbDataReader oleDbDataReader3 = oleDbCommand3.ExecuteReader(CommandBehavior.Default);
					bool flag5 = !oleDbDataReader3.HasRows;
					oleDbDataReader3.Close();
					oleDbConnection3.Close();
					if (flag5)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(7,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=8 ";
					if (this.chkByGroupLimit.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=8 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				else
				{
					SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString);
					SqlCommand sqlCommand3 = new SqlCommand("SELECT f_ID from t_b_MealSetup WHERE f_ID=8 ", sqlConnection3);
					sqlConnection3.Open();
					SqlDataReader sqlDataReader3 = sqlCommand3.ExecuteReader(CommandBehavior.Default);
					bool flag6 = !sqlDataReader3.HasRows;
					sqlDataReader3.Close();
					sqlConnection3.Close();
					if (flag6)
					{
						text6 = "INSERT INTO t_b_MealSetup (f_ID, f_Value, f_BeginHMS, f_EndHMS, f_ParamVal) VALUES(8,0,NULL,NULL,0) ";
						wgAppConfig.runUpdateSql(text6);
					}
					text6 = "UPDATE t_b_MealSetup SET f_Value = 0 WHERE f_ID=8 ";
					if (this.chkByGroupLimit.Checked)
					{
						text6 = "UPDATE t_b_MealSetup SET f_Value = 1 WHERE f_ID=8 ";
					}
					wgAppConfig.runUpdateSql(text6);
				}
				base.Close();
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x001C1CFC File Offset: 0x001C0CFC
		private void btnOption0_Click(object sender, EventArgs e)
		{
			using (dfrmMealOption dfrmMealOption = new dfrmMealOption())
			{
				dfrmMealOption.mealNo = 0;
				dfrmMealOption.Text = dfrmMealOption.Text + "--" + this.chkMorningMeal.Text;
				dfrmMealOption.ShowDialog();
			}
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x001C1D5C File Offset: 0x001C0D5C
		private void btnOption1_Click(object sender, EventArgs e)
		{
			using (dfrmMealOption dfrmMealOption = new dfrmMealOption())
			{
				dfrmMealOption.mealNo = 1;
				dfrmMealOption.Text = dfrmMealOption.Text + "--" + this.chkLunchMeal.Text;
				dfrmMealOption.ShowDialog();
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x001C1DBC File Offset: 0x001C0DBC
		private void btnOption2_Click(object sender, EventArgs e)
		{
			using (dfrmMealOption dfrmMealOption = new dfrmMealOption())
			{
				dfrmMealOption.mealNo = 2;
				dfrmMealOption.Text = dfrmMealOption.Text + "--" + this.chkEveningMeal.Text;
				dfrmMealOption.ShowDialog();
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x001C1E1C File Offset: 0x001C0E1C
		private void btnOption3_Click(object sender, EventArgs e)
		{
			using (dfrmMealOption dfrmMealOption = new dfrmMealOption())
			{
				dfrmMealOption.mealNo = 3;
				dfrmMealOption.Text = dfrmMealOption.Text + "--" + this.chkOtherMeal.Text;
				dfrmMealOption.ShowDialog();
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x001C1E7C File Offset: 0x001C0E7C
		private void chkByGroup_CheckedChanged(object sender, EventArgs e)
		{
			this.btnByDepartment.Visible = this.chkByGroup.Checked;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x001C1E94 File Offset: 0x001C0E94
		private void chkByGroupLimit_CheckedChanged(object sender, EventArgs e)
		{
			this.btnLimitPersonByGroups.Visible = this.chkByGroupLimit.Checked;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x001C1EAC File Offset: 0x001C0EAC
		private void chkMeal_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
				this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
				this.nudMorning.Visible = this.chkMorningMeal.Checked;
				this.lblMorning.Visible = this.chkMorningMeal.Checked;
				this.btnOption0.Visible = this.chkMorningMeal.Checked;
				this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
				this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
				this.nudLunch.Visible = this.chkLunchMeal.Checked;
				this.lblLunch.Visible = this.chkLunchMeal.Checked;
				this.btnOption1.Visible = this.chkLunchMeal.Checked;
				this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
				this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
				this.nudEvening.Visible = this.chkEveningMeal.Checked;
				this.lblEvening.Visible = this.chkEveningMeal.Checked;
				this.btnOption2.Visible = this.chkEveningMeal.Checked;
				this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
				this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
				this.nudOther.Visible = this.chkOtherMeal.Checked;
				this.lblOther.Visible = this.chkOtherMeal.Checked;
				this.btnOption3.Visible = this.chkOtherMeal.Checked;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x001C20A4 File Offset: 0x001C10A4
		private void dfrmMealSetup_Load(object sender, EventArgs e)
		{
			this.dateBeginHMS1.CustomFormat = "HH:mm";
			this.dateBeginHMS1.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS1.Value = DateTime.Parse("04:00:00");
			this.dateEndHMS1.CustomFormat = "HH:mm";
			this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS1.Value = DateTime.Parse("9:59:59");
			this.dateBeginHMS2.CustomFormat = "HH:mm";
			this.dateBeginHMS2.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS2.Value = DateTime.Parse("10:00:00");
			this.dateEndHMS2.CustomFormat = "HH:mm";
			this.dateEndHMS2.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS2.Value = DateTime.Parse("15:59:59");
			this.dateBeginHMS3.CustomFormat = "HH:mm";
			this.dateBeginHMS3.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS3.Value = DateTime.Parse("16:00:00");
			this.dateEndHMS3.CustomFormat = "HH:mm";
			this.dateEndHMS3.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS3.Value = DateTime.Parse("21:59:59");
			this.dateBeginHMS4.CustomFormat = "HH:mm";
			this.dateBeginHMS4.Format = DateTimePickerFormat.Custom;
			this.dateBeginHMS4.Value = DateTime.Parse("22:00:00");
			this.dateEndHMS4.CustomFormat = "HH:mm";
			this.dateEndHMS4.Format = DateTimePickerFormat.Custom;
			this.dateEndHMS4.Value = DateTime.Parse("23:59:59");
			this.loadData();
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x001C2240 File Offset: 0x001C1240
		public void loadData()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadData_Acc();
				return;
			}
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				SqlCommand sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID NOT IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", sqlConnection);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
				sqlDataAdapter.Fill(this.ds, "optionalReader");
				sqlCommand = new SqlCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID  IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(this.ds, "optionalReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
				sqlCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID=1 ";
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
				if (sqlDataReader.Read())
				{
					if (int.Parse(sqlDataReader["f_Value"].ToString()) == 1)
					{
						this.radioButton2.Checked = true;
					}
					else
					{
						if (int.Parse(sqlDataReader["f_Value"].ToString()) == 2)
						{
							this.radioButton3.Checked = true;
							try
							{
								this.nudRuleSeconds.Value = (int)decimal.Parse(sqlDataReader["f_ParamVal"].ToString(), CultureInfo.InvariantCulture);
								goto IL_02D0;
							}
							catch (Exception ex)
							{
								wgTools.WgDebugWrite(ex.ToString(), new object[0]);
								goto IL_02D0;
							}
						}
						this.radioButton1.Checked = true;
					}
				}
				IL_02D0:
				sqlDataReader.Close();
				sqlCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.Open();
				}
				sqlDataReader = sqlCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					if (int.Parse(sqlDataReader["f_ID"].ToString()) == 2)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkMorningMeal.Checked = true;
							this.dateBeginHMS1.Value = (DateTime)sqlDataReader["f_BeginHMS"];
							this.dateEndHMS1.Value = (DateTime)sqlDataReader["f_EndHMS"];
							this.nudMorning.Value = decimal.Parse(wgTools.SetObjToStr(sqlDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkMorningMeal.Checked = false;
						}
						this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
						this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
						this.nudMorning.Visible = this.chkMorningMeal.Checked;
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 3)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkLunchMeal.Checked = true;
							this.dateBeginHMS2.Value = (DateTime)sqlDataReader["f_BeginHMS"];
							this.dateEndHMS2.Value = (DateTime)sqlDataReader["f_EndHMS"];
							this.nudLunch.Value = decimal.Parse(wgTools.SetObjToStr(sqlDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkLunchMeal.Checked = false;
						}
						this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
						this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
						this.nudLunch.Visible = this.chkLunchMeal.Checked;
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 4)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkEveningMeal.Checked = true;
							this.dateBeginHMS3.Value = (DateTime)sqlDataReader["f_BeginHMS"];
							this.dateEndHMS3.Value = (DateTime)sqlDataReader["f_EndHMS"];
							this.nudEvening.Value = decimal.Parse(wgTools.SetObjToStr(sqlDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkEveningMeal.Checked = false;
						}
						this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
						this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
						this.nudEvening.Visible = this.chkEveningMeal.Checked;
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 5)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkOtherMeal.Checked = true;
							this.dateBeginHMS4.Value = (DateTime)sqlDataReader["f_BeginHMS"];
							this.dateEndHMS4.Value = (DateTime)sqlDataReader["f_EndHMS"];
							this.nudOther.Value = decimal.Parse(wgTools.SetObjToStr(sqlDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkOtherMeal.Checked = false;
						}
						this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
						this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
						this.nudOther.Visible = this.chkOtherMeal.Checked;
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 6)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkAllowableSwipe.Checked = true;
						}
						else
						{
							this.chkAllowableSwipe.Checked = false;
						}
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 7)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkByGroup.Checked = true;
						}
						else
						{
							this.chkByGroup.Checked = false;
						}
					}
					else if (int.Parse(sqlDataReader["f_ID"].ToString()) == 8)
					{
						if (int.Parse(wgTools.SetObjToStr(sqlDataReader["f_Value"])) > 0)
						{
							this.chkByGroupLimit.Checked = true;
						}
						else
						{
							this.chkByGroupLimit.Checked = false;
						}
					}
				}
				sqlDataReader.Close();
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x001C2A80 File Offset: 0x001C1A80
		public void loadData_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				OleDbCommand oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 0 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID NOT IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				oleDbCommand = new OleDbCommand("Select f_ReaderID, f_ReaderName, 1 as f_Selected from t_b_reader  INNER JOIN t_b_Controller ON ( t_b_Reader.f_ControllerID = t_b_Controller.f_ControllerID )  WHERE   t_b_reader.f_ReaderID  IN (SELECT t_d_Reader4Meal.f_ReaderID FROM t_d_Reader4Meal  ) ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "optionalReader");
				this.dv = new DataView(this.ds.Tables["optionalReader"]);
				this.dv.RowFilter = " f_Selected = 0";
				this.dvSelected = new DataView(this.ds.Tables["optionalReader"]);
				this.dvSelected.RowFilter = " f_Selected = 1";
				this.dt = this.ds.Tables["optionalReader"];
				try
				{
					this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns[0] };
				}
				catch (Exception)
				{
					throw;
				}
				for (int i = 0; i < this.dgvOptional.Columns.Count; i++)
				{
					this.dgvOptional.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvSelected.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
				}
				this.dv.RowFilter = "f_Selected = 0";
				this.dvSelected.RowFilter = "f_Selected > 0";
				this.dgvOptional.AutoGenerateColumns = false;
				this.dgvOptional.DataSource = this.dv;
				this.dgvSelected.AutoGenerateColumns = false;
				this.dgvSelected.DataSource = this.dvSelected;
				this.dgvOptional.DefaultCellStyle.ForeColor = Color.Black;
				this.dgvSelected.DefaultCellStyle.ForeColor = Color.Black;
				oleDbCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID=1 ";
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
				if (oleDbDataReader.Read())
				{
					if (int.Parse(oleDbDataReader["f_Value"].ToString()) == 1)
					{
						this.radioButton2.Checked = true;
					}
					else
					{
						if (int.Parse(oleDbDataReader["f_Value"].ToString()) == 2)
						{
							this.radioButton3.Checked = true;
							try
							{
								this.nudRuleSeconds.Value = (int)decimal.Parse(oleDbDataReader["f_ParamVal"].ToString(), CultureInfo.InvariantCulture);
								goto IL_02BB;
							}
							catch (Exception ex)
							{
								wgTools.WgDebugWrite(ex.ToString(), new object[0]);
								goto IL_02BB;
							}
						}
						this.radioButton1.Checked = true;
					}
				}
				IL_02BB:
				oleDbDataReader.Close();
				oleDbCommand.CommandText = "SELECT * from t_b_MealSetup WHERE f_ID > 1 ORDER BY f_ID ASC";
				if (oleDbConnection.State != ConnectionState.Open)
				{
					oleDbConnection.Open();
				}
				oleDbDataReader = oleDbCommand.ExecuteReader();
				while (oleDbDataReader.Read())
				{
					if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 2)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkMorningMeal.Checked = true;
							this.dateBeginHMS1.Value = (DateTime)oleDbDataReader["f_BeginHMS"];
							this.dateEndHMS1.Value = (DateTime)oleDbDataReader["f_EndHMS"];
							this.nudMorning.Value = decimal.Parse(wgTools.SetObjToStr(oleDbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkMorningMeal.Checked = false;
						}
						this.dateBeginHMS1.Visible = this.chkMorningMeal.Checked;
						this.dateEndHMS1.Visible = this.chkMorningMeal.Checked;
						this.nudMorning.Visible = this.chkMorningMeal.Checked;
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 3)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkLunchMeal.Checked = true;
							this.dateBeginHMS2.Value = (DateTime)oleDbDataReader["f_BeginHMS"];
							this.dateEndHMS2.Value = (DateTime)oleDbDataReader["f_EndHMS"];
							this.nudLunch.Value = decimal.Parse(wgTools.SetObjToStr(oleDbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkLunchMeal.Checked = false;
						}
						this.dateBeginHMS2.Visible = this.chkLunchMeal.Checked;
						this.dateEndHMS2.Visible = this.chkLunchMeal.Checked;
						this.nudLunch.Visible = this.chkLunchMeal.Checked;
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 4)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkEveningMeal.Checked = true;
							this.dateBeginHMS3.Value = (DateTime)oleDbDataReader["f_BeginHMS"];
							this.dateEndHMS3.Value = (DateTime)oleDbDataReader["f_EndHMS"];
							this.nudEvening.Value = decimal.Parse(wgTools.SetObjToStr(oleDbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkEveningMeal.Checked = false;
						}
						this.dateBeginHMS3.Visible = this.chkEveningMeal.Checked;
						this.dateEndHMS3.Visible = this.chkEveningMeal.Checked;
						this.nudEvening.Visible = this.chkEveningMeal.Checked;
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 5)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkOtherMeal.Checked = true;
							this.dateBeginHMS4.Value = (DateTime)oleDbDataReader["f_BeginHMS"];
							this.dateEndHMS4.Value = (DateTime)oleDbDataReader["f_EndHMS"];
							this.nudOther.Value = decimal.Parse(wgTools.SetObjToStr(oleDbDataReader["f_ParamVal"]), CultureInfo.InvariantCulture);
						}
						else
						{
							this.chkOtherMeal.Checked = false;
						}
						this.dateBeginHMS4.Visible = this.chkOtherMeal.Checked;
						this.dateEndHMS4.Visible = this.chkOtherMeal.Checked;
						this.nudOther.Visible = this.chkOtherMeal.Checked;
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 6)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkAllowableSwipe.Checked = true;
						}
						else
						{
							this.chkAllowableSwipe.Checked = false;
						}
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 7)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkByGroup.Checked = true;
						}
						else
						{
							this.chkByGroup.Checked = false;
						}
					}
					else if (int.Parse(oleDbDataReader["f_ID"].ToString()) == 8)
					{
						if (int.Parse(wgTools.SetObjToStr(oleDbDataReader["f_Value"])) > 0)
						{
							this.chkByGroupLimit.Checked = true;
						}
						else
						{
							this.chkByGroupLimit.Checked = false;
						}
					}
				}
				oleDbDataReader.Close();
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
		}

		// Token: 0x04002DFD RID: 11773
		private DataTable dt;

		// Token: 0x04002DFE RID: 11774
		private DataView dv;

		// Token: 0x04002DFF RID: 11775
		private DataView dvSelected;

		// Token: 0x04002E00 RID: 11776
		private DataSet ds = new DataSet("dsMeal");
	}
}
