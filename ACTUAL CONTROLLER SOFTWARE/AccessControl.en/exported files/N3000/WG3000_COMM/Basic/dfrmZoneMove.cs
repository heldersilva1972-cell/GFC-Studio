using System;
using System.Collections;
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

namespace WG3000_COMM.Basic
{
	// Token: 0x0200003D RID: 61
	public partial class dfrmZoneMove : frmN3000
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x000744DC File Offset: 0x000734DC
		public dfrmZoneMove()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00074537 File Offset: 0x00073537
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00074548 File Offset: 0x00073548
		private void btnMoveAllToOne_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}:{1}?", this.btnMoveAllToOne.Text, this.cbof_GroupNew.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				new icGroup();
				icControllerZone icControllerZone = new icControllerZone();
				if (string.IsNullOrEmpty(this.cbof_GroupNew.Text))
				{
					XMessageBox.Show(this, CommonStr.strNeedSelectTopZone + "\r\n\r\n", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				string text = this.cbof_GroupNew.Text;
				if (text.LastIndexOf("\\") >= 0)
				{
					XMessageBox.Show(this, CommonStr.strNeedSelectTopZone + "\r\n\r\n", wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				for (int i = 0; i < this.cbof_GroupNew.Items.Count; i++)
				{
					string text2 = (string)this.cbof_GroupNew.Items[i];
					if (!string.IsNullOrEmpty(text2) && text2.IndexOf(text) != 0)
					{
						string text3 = text + "\\" + text2;
						if (!icControllerZone.checkExisted(text3))
						{
							icControllerZone.Update(text2, text3);
						}
					}
				}
				icControllerZone.updateZoneNO();
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00074684 File Offset: 0x00073684
		private void btnOK_Click(object sender, EventArgs e)
		{
			if ((!string.IsNullOrEmpty(this.cbof_GroupNew.Text) || XMessageBox.Show(this, CommonStr.strAreYouSure + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel) && XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}=>{1}?", this.cbof_GroupID.Text, this.cbof_GroupNew.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
			{
				if (this.cbof_GroupNew.Text.IndexOf(this.selectedGroupName + "\\") == 0)
				{
					XMessageBox.Show(this, CommonStr.strMoveZoneFailedA, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				new icGroup();
				icControllerZone icControllerZone = new icControllerZone();
				string text;
				if (string.IsNullOrEmpty(this.cbof_GroupNew.Text))
				{
					if (this.selectedGroupName.LastIndexOf("\\") < 0)
					{
						text = this.selectedGroupName;
					}
					else
					{
						text = this.selectedGroupName.Substring(this.selectedGroupName.LastIndexOf("\\") + 1);
					}
				}
				else if (this.selectedGroupName.LastIndexOf("\\") < 0)
				{
					text = this.cbof_GroupNew.Text + "\\" + this.selectedGroupName;
				}
				else
				{
					text = this.cbof_GroupNew.Text + "\\" + this.selectedGroupName.Substring(this.selectedGroupName.LastIndexOf("\\") + 1);
				}
				if (icControllerZone.checkExisted(text))
				{
					XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icControllerZone.Update(this.selectedGroupName, text);
				icControllerZone.updateZoneNO();
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00074848 File Offset: 0x00073848
		private void dfrmDepartmentMove_Load(object sender, EventArgs e)
		{
			try
			{
				new icGroup();
				this.getGroup(ref this.arrGroupNameWithSpace, ref this.arrGroupID, ref this.arrGroupNO);
				int i = this.arrGroupID.Count;
				for (i = 0; i < this.arrGroupID.Count; i++)
				{
					if (i == 0 && string.IsNullOrEmpty(this.arrGroupNameWithSpace[i].ToString()))
					{
						this.arrGroupName.Add(CommonStr.strAll);
					}
					else
					{
						this.arrGroupName.Add(this.arrGroupNameWithSpace[i].ToString());
					}
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
					this.cbof_GroupNew.Items.Add(this.arrGroupNameWithSpace[i].ToString());
				}
				int num = (int)this.arrGroupID[0];
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
					for (i = 0; i < this.cbof_GroupID.Items.Count; i++)
					{
						if ((string)this.cbof_GroupID.Items[i] == this.selectedGroupName)
						{
							this.cbof_GroupID.SelectedIndex = i;
							break;
						}
					}
				}
				if (this.cbof_GroupNew.Items.Count > 0)
				{
					this.cbof_GroupNew.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x000749F4 File Offset: 0x000739F4
		public int getGroup(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.getGroup_Acc(ref arrGroupName, ref arrGroupID, ref arrGroupNO);
			}
			int num = -9;
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					if (sqlConnection.State != ConnectionState.Open)
					{
						sqlConnection.Open();
					}
					arrGroupName.Clear();
					arrGroupID.Clear();
					arrGroupNO.Clear();
					ArrayList arrayList = new ArrayList();
					using (SqlCommand sqlCommand = new SqlCommand("SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC", sqlConnection))
					{
						SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
						while (sqlDataReader.Read())
						{
							arrayList.Add(sqlDataReader[0]);
						}
						sqlDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrGroupName.Add("");
							arrGroupID.Add(0);
							arrGroupNO.Add(0);
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						text += "  ORDER BY f_ZoneName  + '\\' ASC";
						sqlCommand.CommandText = text;
						sqlDataReader = sqlCommand.ExecuteReader();
						bool flag = true;
						while (sqlDataReader.Read())
						{
							if (arrayList.Count > 0)
							{
								flag = false;
							}
							for (int i = 0; i < arrayList.Count; i++)
							{
								string text2 = (string)sqlDataReader[1];
								if (text2 == arrayList[i].ToString() || text2.IndexOf(arrayList[i].ToString() + "\\") == 0)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								arrGroupID.Add(sqlDataReader[0]);
								arrGroupName.Add(sqlDataReader[1]);
								arrGroupNO.Add(sqlDataReader[2]);
							}
						}
						sqlDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00074C30 File Offset: 0x00073C30
		public int getGroup_Acc(ref ArrayList arrGroupName, ref ArrayList arrGroupID, ref ArrayList arrGroupNO)
		{
			int num = -9;
			try
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					if (oleDbConnection.State != ConnectionState.Open)
					{
						oleDbConnection.Open();
					}
					arrGroupName.Clear();
					arrGroupID.Clear();
					arrGroupNO.Clear();
					ArrayList arrayList = new ArrayList();
					using (OleDbCommand oleDbCommand = new OleDbCommand("SELECT f_ZoneName from t_b_Controller_Zone,t_b_Controller_Zone4Operator WHERE t_b_Controller_Zone4Operator.f_ZoneID = t_b_Controller_Zone.f_ZoneID  AND t_b_Controller_Zone4Operator.f_OperatorID = " + icOperator.OperatorID.ToString() + "  order by f_ZoneName  + '\\' ASC", oleDbConnection))
					{
						OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
						while (oleDbDataReader.Read())
						{
							arrayList.Add(oleDbDataReader[0]);
						}
						oleDbDataReader.Close();
						if (arrayList.Count == 0)
						{
							arrGroupName.Add("");
							arrGroupID.Add(0);
							arrGroupNO.Add(0);
						}
						string text = "SELECT f_ZoneID,f_ZoneName, f_ZoneNO from [t_b_Controller_Zone]  ";
						text += "  ORDER BY f_ZoneName  + '\\' ASC";
						oleDbCommand.CommandText = text;
						oleDbDataReader = oleDbCommand.ExecuteReader();
						bool flag = true;
						while (oleDbDataReader.Read())
						{
							if (arrayList.Count > 0)
							{
								flag = false;
							}
							for (int i = 0; i < arrayList.Count; i++)
							{
								string text2 = (string)oleDbDataReader[1];
								if (text2 == arrayList[i].ToString() || text2.IndexOf(arrayList[i].ToString() + "\\") == 0)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								arrGroupID.Add(oleDbDataReader[0]);
								arrGroupName.Add(oleDbDataReader[1]);
								arrGroupNO.Add(oleDbDataReader[2]);
							}
						}
						oleDbDataReader.Close();
					}
				}
				num = 1;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
			return num;
		}

		// Token: 0x040007E9 RID: 2025
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040007EA RID: 2026
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040007EB RID: 2027
		private ArrayList arrGroupNameWithSpace = new ArrayList();

		// Token: 0x040007EC RID: 2028
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040007ED RID: 2029
		public string selectedGroupName = "";

		// Token: 0x040007EE RID: 2030
		public string strSqlSelected = "";
	}
}
