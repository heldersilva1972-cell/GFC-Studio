using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x02000369 RID: 873
	public partial class dfrmHolidaySet : frmN3000
	{
		// Token: 0x06001C67 RID: 7271 RVA: 0x00256B0F File Offset: 0x00255B0F
		public dfrmHolidaySet()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvMain);
			wgAppConfig.custDataGridview(ref this.dgvMain2);
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00256B34 File Offset: 0x00255B34
		private void _dataTableLoad()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this._dataTableLoad_Acc();
				return;
			}
			this.con = new SqlConnection(wgAppConfig.dbConString);
			this.ds = new DataSet("Holiday");
			try
			{
				this.ds.Clear();
				string text = "";
				text = " SELECT  t_a_Holiday.f_Name,";
				text += " [f_Value] AS f_from,  [f_Value1] AS f_from1,  [f_Value2] AS f_to,  [f_Value3] AS f_to1,  t_a_Holiday.f_Note, t_a_Holiday.f_No,t_a_Holiday.f_Value,f_EName,f_Value1,f_Value3 FROM t_a_Holiday";
				string text2 = text + " WHERE [f_Type]='1'";
				this.cmd = new SqlCommand(text2, this.con);
				this.da = new SqlDataAdapter(this.cmd);
				this.da.Fill(this.ds, "Holiday1");
				this.dt = this.ds.Tables["Holiday1"];
				using (comShift comShift = new comShift())
				{
					comShift.localizedHoliday(this.ds.Tables["Holiday1"]);
				}
				text2 = text + " WHERE [f_Type]='2' ORDER BY [f_from] ASC ";
				this.cmd = new SqlCommand(text2, this.con);
				this.da = new SqlDataAdapter(this.cmd);
				this.da.Fill(this.ds, "Holiday2");
				using (comShift comShift2 = new comShift())
				{
					comShift2.localizedHoliday(this.ds.Tables["Holiday2"]);
				}
				text2 = text + " WHERE [f_Type]='3' ORDER BY [f_from] ASC ";
				this.cmd = new SqlCommand(text2, this.con);
				this.da = new SqlDataAdapter(this.cmd);
				this.da.Fill(this.ds, "NeedWork");
				using (comShift comShift3 = new comShift())
				{
					comShift3.localizedHoliday(this.ds.Tables["NeedWork"]);
				}
				this.dgvMain.AutoGenerateColumns = false;
				this.dgvMain2.AutoGenerateColumns = false;
				this.dgvMain.DataSource = this.ds.Tables["Holiday2"];
				this.dgvMain2.DataSource = this.ds.Tables["NeedWork"];
				for (int i = 0; i < this.dgvMain.Columns.Count; i++)
				{
					this.dgvMain.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvMain2.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvMain.Columns[i].Name = this.dt.Columns[i].ColumnName;
					this.dgvMain2.Columns[i].Name = this.dt.Columns[i].ColumnName + "_NeedWork";
				}
				for (int i = 0; i < this.dgvMain.Rows.Count; i++)
				{
					this.dgvMain.Rows[i].Cells["f_from"].Value = wgTools.wgDateTimeParse(this.dgvMain.Rows[i].Cells["f_from"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
					this.dgvMain.Rows[i].Cells["f_to"].Value = wgTools.wgDateTimeParse(this.dgvMain.Rows[i].Cells["f_to"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
				}
				for (int i = 0; i < this.dgvMain2.Rows.Count; i++)
				{
					this.dgvMain2.Rows[i].Cells["f_from_NeedWork"].Value = wgTools.wgDateTimeParse(this.dgvMain2.Rows[i].Cells["f_from_NeedWork"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
					this.dgvMain2.Rows[i].Cells["f_to_NeedWork"].Value = wgTools.wgDateTimeParse(this.dgvMain2.Rows[i].Cells["f_to_NeedWork"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
				}
				for (int j = 0; j <= this.dt.Rows.Count - 1; j++)
				{
					DataRow dataRow = this.dt.Rows[j];
					if (Convert.ToInt32(dataRow["f_NO"]) == 1)
					{
						if (Convert.ToString(dataRow["f_Value"]) == "0")
						{
							this.optSatWork0.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "1")
						{
							this.optSatWork1.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "3")
						{
							this.optSatWork2.Checked = true;
						}
						else
						{
							this.optSatWork0.Checked = true;
						}
					}
					else if (Convert.ToInt32(dataRow["f_NO"]) == 2)
					{
						if (Convert.ToString(dataRow["f_Value"]) == "0")
						{
							this.optSunWork0.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "1")
						{
							this.optSunWork1.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "3")
						{
							this.optSunWork2.Checked = true;
						}
						else
						{
							this.optSunWork0.Checked = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0025721C File Offset: 0x0025621C
		private void _dataTableLoad_Acc()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			this.ds = new DataSet("Holiday");
			try
			{
				this.ds.Clear();
				string text = "";
				text = " SELECT  t_a_Holiday.f_Name,";
				text += " [f_Value] AS f_from,  [f_Value1] AS f_from1,  [f_Value2] AS f_to,  [f_Value3] AS f_to1,  t_a_Holiday.f_Note, t_a_Holiday.f_No,t_a_Holiday.f_Value,f_EName,f_Value1,f_Value3 FROM t_a_Holiday";
				OleDbCommand oleDbCommand = new OleDbCommand(text + " WHERE [f_Type]='1'", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "Holiday1");
				this.dt = this.ds.Tables["Holiday1"];
				using (comShift_Acc comShift_Acc = new comShift_Acc())
				{
					comShift_Acc.localizedHoliday(this.ds.Tables["Holiday1"]);
				}
				oleDbCommand = new OleDbCommand(text + " WHERE [f_Type]='2' ORDER BY [f_Value] ASC ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "Holiday2");
				using (comShift_Acc comShift_Acc2 = new comShift_Acc())
				{
					comShift_Acc2.localizedHoliday(this.ds.Tables["Holiday2"]);
				}
				oleDbCommand = new OleDbCommand(text + " WHERE [f_Type]='3' ORDER BY [f_Value] ASC ", oleDbConnection);
				new OleDbDataAdapter(oleDbCommand).Fill(this.ds, "NeedWork");
				using (comShift_Acc comShift_Acc3 = new comShift_Acc())
				{
					comShift_Acc3.localizedHoliday(this.ds.Tables["NeedWork"]);
				}
				this.dgvMain.AutoGenerateColumns = false;
				this.dgvMain2.AutoGenerateColumns = false;
				this.dgvMain.DataSource = this.ds.Tables["Holiday2"];
				this.dgvMain2.DataSource = this.ds.Tables["NeedWork"];
				for (int i = 0; i < this.dgvMain.Columns.Count; i++)
				{
					this.dgvMain.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvMain2.Columns[i].DataPropertyName = this.dt.Columns[i].ColumnName;
					this.dgvMain.Columns[i].Name = this.dt.Columns[i].ColumnName;
					this.dgvMain2.Columns[i].Name = this.dt.Columns[i].ColumnName + "_NeedWork";
				}
				for (int i = 0; i < this.dgvMain.Rows.Count; i++)
				{
					this.dgvMain.Rows[i].Cells["f_from"].Value = wgTools.wgDateTimeParse(this.dgvMain.Rows[i].Cells["f_from"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
					this.dgvMain.Rows[i].Cells["f_to"].Value = wgTools.wgDateTimeParse(this.dgvMain.Rows[i].Cells["f_to"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
				}
				for (int i = 0; i < this.dgvMain2.Rows.Count; i++)
				{
					this.dgvMain2.Rows[i].Cells["f_from_NeedWork"].Value = wgTools.wgDateTimeParse(this.dgvMain2.Rows[i].Cells["f_from_NeedWork"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
					this.dgvMain2.Rows[i].Cells["f_to_NeedWork"].Value = wgTools.wgDateTimeParse(this.dgvMain2.Rows[i].Cells["f_to_NeedWork"].Value).ToString(wgTools.DisplayFormat_DateYMDWeek);
				}
				for (int j = 0; j <= this.dt.Rows.Count - 1; j++)
				{
					DataRow dataRow = this.dt.Rows[j];
					if (Convert.ToInt32(dataRow["f_NO"]) == 1)
					{
						if (Convert.ToString(dataRow["f_Value"]) == "0")
						{
							this.optSatWork0.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "1")
						{
							this.optSatWork1.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "3")
						{
							this.optSatWork2.Checked = true;
						}
						else
						{
							this.optSatWork0.Checked = true;
						}
					}
					else if (Convert.ToInt32(dataRow["f_NO"]) == 2)
					{
						if (Convert.ToString(dataRow["f_Value"]) == "0")
						{
							this.optSunWork0.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "1")
						{
							this.optSunWork1.Checked = true;
						}
						else if (Convert.ToString(dataRow["f_Value"]) == "3")
						{
							this.optSunWork2.Checked = true;
						}
						else
						{
							this.optSunWork0.Checked = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00257888 File Offset: 0x00256888
		private void btnAddHoliday_Click(object sender, EventArgs e)
		{
			using (dfrmHolidayAdd dfrmHolidayAdd = new dfrmHolidayAdd())
			{
				dfrmHolidayAdd.ShowDialog(this);
			}
			this._dataTableLoad();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x002578C8 File Offset: 0x002568C8
		private void btnAddNeedWork_Click(object sender, EventArgs e)
		{
			using (dfrmHolidayAdd dfrmHolidayAdd = new dfrmHolidayAdd())
			{
				dfrmHolidayAdd.holidayType = "3";
				dfrmHolidayAdd.Text = CommonStr.strNeedToWork;
				dfrmHolidayAdd.ShowDialog(this);
			}
			this._dataTableLoad();
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0025791C File Offset: 0x0025691C
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0025792C File Offset: 0x0025692C
		private void btnDelHoliday_Click(object sender, EventArgs e)
		{
			if (this.dgvMain.Rows.Count > 0)
			{
				int index = this.dgvMain.SelectedRows[0].Index;
				wgAppConfig.runUpdateSql(" DELETE FROM t_a_Holiday WHERE [f_NO]= " + (this.dgvMain.DataSource as DataTable).Rows[index]["f_NO"].ToString());
				this._dataTableLoad();
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x002579A4 File Offset: 0x002569A4
		private void btnDelNeedWork_Click(object sender, EventArgs e)
		{
			if (this.dgvMain2.Rows.Count > 0)
			{
				int index = this.dgvMain2.SelectedRows[0].Index;
				wgAppConfig.runUpdateSql(" DELETE FROM t_a_Holiday WHERE [f_NO]= " + (this.dgvMain2.DataSource as DataTable).Rows[index]["f_NO"].ToString());
				this._dataTableLoad();
			}
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00257A1C File Offset: 0x00256A1C
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = " UPDATE t_a_Holiday ";
			text += " SET [f_Value]=";
			if (this.optSatWork0.Checked)
			{
				text += wgTools.PrepareStrNUnicode(0);
			}
			else if (this.optSatWork1.Checked)
			{
				text += wgTools.PrepareStrNUnicode(1);
			}
			else if (this.optSatWork2.Checked)
			{
				text += wgTools.PrepareStrNUnicode(3);
			}
			else
			{
				text += wgTools.PrepareStrNUnicode(0);
			}
			wgAppConfig.runUpdateSql(text + " WHERE [f_NO]=1");
			text = " UPDATE t_a_Holiday ";
			text += " SET [f_Value]=";
			if (this.optSunWork0.Checked)
			{
				text += wgTools.PrepareStrNUnicode(0);
			}
			else if (this.optSunWork1.Checked)
			{
				text += wgTools.PrepareStrNUnicode(1);
			}
			else if (this.optSunWork2.Checked)
			{
				text += wgTools.PrepareStrNUnicode(3);
			}
			else
			{
				text += wgTools.PrepareStrNUnicode(0);
			}
			wgAppConfig.runUpdateSql(text + " WHERE [f_NO]=2");
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00257B68 File Offset: 0x00256B68
		private void dfrmHolidaySet_Load(object sender, EventArgs e)
		{
			this._dataTableLoad();
			bool flag = false;
			string text = "mnuHolidaySet";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAddHoliday.Visible = false;
				this.btnAddNeedWork.Visible = false;
				this.btnDelHoliday.Visible = false;
				this.btnDelNeedWork.Visible = false;
				this.btnOK.Visible = false;
			}
		}

		// Token: 0x040036AB RID: 13995
		private SqlCommand cmd;

		// Token: 0x040036AC RID: 13996
		private SqlConnection con;

		// Token: 0x040036AD RID: 13997
		private SqlDataAdapter da;

		// Token: 0x040036AE RID: 13998
		private DataSet ds;

		// Token: 0x040036AF RID: 13999
		private DataTable dt;
	}
}
