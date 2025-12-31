using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036A RID: 874
	public partial class dfrmHolidayType : frmN3000
	{
		// Token: 0x06001C73 RID: 7283 RVA: 0x002589D1 File Offset: 0x002579D1
		public dfrmHolidayType()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x002589EC File Offset: 0x002579EC
		private void _loadData()
		{
			string text = "SELECT * FROM t_a_HolidayType ORDER BY f_NO ";
			this.dt = new DataTable("t_a_HolidayType");
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
				}
				comShift_Acc.localizedHolidayType(this.dt);
			}
			else
			{
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
				comShift.localizedHolidayType(this.dt);
			}
			this.arrDefaultList.Clear();
			this.arrDefaultList.Add("出差");
			this.arrDefaultList.Add("Business Trip");
			this.arrDefaultList.Add("病假");
			this.arrDefaultList.Add("Sick Leave");
			this.arrDefaultList.Add("事假");
			this.arrDefaultList.Add("Private Leave");
			this.arrDefaultList.Add(CommonStr.strAbsence);
			this.arrDefaultList.Add(CommonStr.strLateness);
			this.arrDefaultList.Add(CommonStr.strNotReadCard);
			this.arrDefaultList.Add(CommonStr.strLeaveEarly);
			this.arrDefaultList.Add(CommonStr.strRest);
			this.arrDefaultList.Add(CommonStr.strOvertime);
			this.arrDefaultList.Add(CommonStr.strSignIn);
			this.arrDefaultList.Add(CommonStr.strPrivateLeave);
			this.arrDefaultList.Add(CommonStr.strSickLeave);
			this.arrDefaultList.Add(CommonStr.strBusinessTrip);
			this.arrDefaultList.Add(CommonStr.strPatrolEventAbsence);
			this.arrDefaultList.Add(CommonStr.strPatrolEventEarly);
			this.arrDefaultList.Add(CommonStr.strPatrolEventLate);
			this.arrDefaultList.Add(CommonStr.strPatrolEventNormal);
			this.arrDefaultList.Add(CommonStr.strPatrolEventRest);
			this.arrDefaultList.Add(CommonStr.strPatrolEventLate);
			if (this.dt.Rows.Count > 0)
			{
				this.btnDel.Enabled = true;
				this.btnEdit.Enabled = true;
			}
			else
			{
				this.btnDel.Enabled = false;
				this.btnEdit.Enabled = false;
			}
			if (this.dt.Rows.Count >= 32)
			{
				this.btnAdd.Enabled = false;
			}
			else
			{
				this.btnAdd.Enabled = true;
			}
			this.lstHolidayType.DataSource = this.dt;
			this.lstHolidayType.DisplayMember = "f_HolidayType";
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00258D34 File Offset: 0x00257D34
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					string strNewName = dfrmInputNewName.strNewName;
					if (this.arrDefaultList.IndexOf(strNewName) >= 0)
					{
						XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						using (DataView dataView = new DataView(this.lstHolidayType.DataSource as DataTable))
						{
							dataView.RowFilter = " f_HolidayType= " + wgTools.PrepareStr(strNewName);
							if (dataView.Count > 0)
							{
								XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
						}
						wgAppConfig.runUpdateSql(" INSERT INTO  t_a_HolidayType (f_HolidayType) VALUES(" + wgTools.PrepareStrNUnicode(strNewName.ToString()) + ")");
						this._loadData();
					}
				}
			}
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00258E28 File Offset: 0x00257E28
		private void btnDel_Click(object sender, EventArgs e)
		{
			if (this.arrDefaultList.IndexOf(this.lstHolidayType.Text) >= 0)
			{
				XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string text = string.Format("{0}", this.btnDel.Text);
			if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				wgAppConfig.runUpdateSql(" DELETE FROM t_a_HolidayType  WHERE f_HolidayType = " + wgTools.PrepareStrNUnicode(this.lstHolidayType.Text));
				this._loadData();
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00258EC4 File Offset: 0x00257EC4
		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.arrDefaultList.IndexOf(this.lstHolidayType.Text) >= 0)
			{
				XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				if (dfrmInputNewName.ShowDialog(this) == DialogResult.OK)
				{
					string strNewName = dfrmInputNewName.strNewName;
					if (this.lstHolidayType.Text != strNewName)
					{
						if (this.arrDefaultList.IndexOf(strNewName) >= 0)
						{
							XMessageBox.Show(this, CommonStr.strDefaultType, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							using (DataView dataView = new DataView(this.lstHolidayType.DataSource as DataTable))
							{
								dataView.RowFilter = " f_HolidayType= " + wgTools.PrepareStr(strNewName);
								if (dataView.Count > 0)
								{
									XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									return;
								}
							}
							wgAppConfig.runUpdateSql(" UPDATE t_a_HolidayType SET f_HolidayType = " + wgTools.PrepareStrNUnicode(strNewName.ToString()) + " WHERE f_HolidayType = " + wgTools.PrepareStrNUnicode(this.lstHolidayType.Text));
							this._loadData();
						}
					}
				}
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0025900C File Offset: 0x0025800C
		private void btnExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00259014 File Offset: 0x00258014
		private void btnFullAttendance_Click(object sender, EventArgs e)
		{
			using (dfrmFullAttendanceSet dfrmFullAttendanceSet = new dfrmFullAttendanceSet())
			{
				dfrmFullAttendanceSet.ShowDialog();
			}
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0025904C File Offset: 0x0025804C
		private void dfrmHolidayType_Load(object sender, EventArgs e)
		{
			this._loadData();
		}

		// Token: 0x040036D3 RID: 14035
		private DataTable dt;

		// Token: 0x040036D4 RID: 14036
		private ArrayList arrDefaultList = new ArrayList();
	}
}
