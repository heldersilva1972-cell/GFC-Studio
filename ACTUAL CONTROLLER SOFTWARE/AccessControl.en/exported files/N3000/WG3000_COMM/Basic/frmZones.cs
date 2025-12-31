using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
	// Token: 0x02000059 RID: 89
	public partial class frmZones : frmN3000
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x000BDA66 File Offset: 0x000BCA66
		public frmZones()
		{
			this.InitializeComponent();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000BDA74 File Offset: 0x000BCA74
		private void btnAdd_Click(object sender, EventArgs e)
		{
			string text;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripButton).Text;
				dfrmInputNewName.label1.Text = CommonStr.strZone;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				text = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Trim() == "")
				{
					XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (text.LastIndexOf("\\") >= 0)
				{
					XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				text = text.Trim();
				string text2 = text;
				if (sender == this.btnAddSuper)
				{
					this.trvDepartments.SelectedNode = null;
				}
				if (this.trvDepartments.SelectedNode == null)
				{
					using (IEnumerator enumerator = this.trvDepartments.Nodes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							TreeNode treeNode = (TreeNode)obj;
							if (treeNode.Tag.ToString() == text2)
							{
								XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
						}
						goto IL_01CB;
					}
				}
				text2 = this.trvDepartments.SelectedNode.Tag + "\\" + text2;
				foreach (object obj2 in this.trvDepartments.SelectedNode.Nodes)
				{
					TreeNode treeNode2 = (TreeNode)obj2;
					if (treeNode2.Tag.ToString() == text2)
					{
						XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				IL_01CB:
				icControllerZone icControllerZone = new icControllerZone();
				if (icControllerZone.checkExisted(text2))
				{
					XMessageBox.Show(this, text2 + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icControllerZone.addNew(text2);
				TreeNode treeNode3 = new TreeNode();
				treeNode3.Text = text;
				treeNode3.Tag = text2;
				if (this.trvDepartments.SelectedNode == null)
				{
					this.trvDepartments.Nodes.Add(treeNode3);
					this.trvDepartments.ExpandAll();
					return;
				}
				this.trvDepartments.SelectedNode.Nodes.Add(treeNode3);
				this.trvDepartments.SelectedNode.Expand();
				return;
			}
			else
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000BDD30 File Offset: 0x000BCD30
		private void btnAddSuper_Click(object sender, EventArgs e)
		{
			this.trvDepartments.SelectedNode = null;
			this.btnAdd_Click(sender, e);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000BDD48 File Offset: 0x000BCD48
		private void btnDeleteDept_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this.btnDeleteDept.Text + "\r\n\r\n" + this.txtSelectedDept.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				new icControllerZone().delete(this.txtSelectedDept.Text);
				this.trvDepartments.Nodes.Remove(this.trvDepartments.SelectedNode);
				this.txtSelectedDept.Text = "";
				this.trvDepartments.SelectedNode = null;
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000BDDD8 File Offset: 0x000BCDD8
		private void btnEditDept_Click(object sender, EventArgs e)
		{
			string strNewName;
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripButton).Text;
				dfrmInputNewName.label1.Text = CommonStr.strZone;
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				strNewName = dfrmInputNewName.strNewName;
			}
			if (!string.IsNullOrEmpty(strNewName))
			{
				if (strNewName.Trim() == "")
				{
					XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (strNewName.LastIndexOf("\\") >= 0)
				{
					XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icControllerZone icControllerZone = new icControllerZone();
				string text;
				if (this.txtSelectedDept.Text.LastIndexOf("\\") < 0)
				{
					text = strNewName;
				}
				else
				{
					text = this.txtSelectedDept.Text.Substring(0, this.txtSelectedDept.Text.LastIndexOf("\\")) + "\\" + strNewName;
				}
				if (icControllerZone.checkExisted(text))
				{
					XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icControllerZone.Update(this.txtSelectedDept.Text, text);
				if (this.trvDepartments.SelectedNode != null)
				{
					this.trvDepartments.SelectedNode.Text = strNewName;
					this.trvDepartments.SelectedNode.Tag = text;
					this.txtSelectedDept.Text = text;
				}
				else
				{
					this.txtSelectedDept.Text = "";
				}
				this.loadZone();
				return;
			}
			else
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000BDF90 File Offset: 0x000BCF90
		private void btnExport_Click(object sender, EventArgs e)
		{
			string text = "SELECT f_ZoneNO,f_ZoneName FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC";
			try
			{
				DataSet dataSet = new DataSet();
				DataTable dataTable = new DataTable();
				using (DbConnection dbConnection = (wgAppConfig.IsAccessDB ? new OleDbConnection(wgAppConfig.dbConString) : new SqlConnection(wgAppConfig.dbConString)))
				{
					using (DbCommand dbCommand = (wgAppConfig.IsAccessDB ? new OleDbCommand(text, (OleDbConnection)dbConnection) : new SqlCommand(text, (SqlConnection)dbConnection)))
					{
						using (DataAdapter dataAdapter = (wgAppConfig.IsAccessDB ? new OleDbDataAdapter((OleDbCommand)dbCommand) : new SqlDataAdapter((SqlCommand)dbCommand)))
						{
							dataAdapter.Fill(dataSet);
						}
					}
				}
				dataTable = dataSet.Tables[0];
				this.dataGridView1.AutoGenerateColumns = false;
				this.dataGridView1.DataSource = dataSet.Tables[0];
				int num = 0;
				while (num < dataTable.Columns.Count && num < this.dataGridView1.ColumnCount)
				{
					this.dataGridView1.Columns[num].DataPropertyName = dataTable.Columns[num].ColumnName;
					num++;
				}
				this.dataGridView1.Columns[0].Visible = false;
				wgAppConfig.exportToExcel(this.dataGridView1, this.Text);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000BE16C File Offset: 0x000BD16C
		private void btnMoveDept_Click(object sender, EventArgs e)
		{
			using (dfrmZoneMove dfrmZoneMove = new dfrmZoneMove())
			{
				dfrmZoneMove.selectedGroupName = this.txtSelectedDept.Text;
				if (dfrmZoneMove.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.wgLog(this.btnMoveDept.Text + ":" + this.txtSelectedDept.Text);
					this.loadZone();
				}
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000BE1E4 File Offset: 0x000BD1E4
		private void FindRecursive(TreeNode treeNode, string ParentNodeText, out TreeNode foundNode)
		{
			foundNode = null;
			if (treeNode.Tag.ToString() == ParentNodeText)
			{
				foundNode = treeNode;
				return;
			}
			if (foundNode == null)
			{
				foreach (object obj in treeNode.Nodes)
				{
					TreeNode treeNode2 = (TreeNode)obj;
					if (foundNode != null)
					{
						break;
					}
					this.FindRecursive(treeNode2, ParentNodeText, out foundNode);
				}
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000BE264 File Offset: 0x000BD264
		private void frmDepartments_Load(object sender, EventArgs e)
		{
			this.txtSelectedDept_TextChanged(null, null);
			this.loadOperatorPrivilege();
			this.loadZone();
			this.txtSelectedDept.Text = "";
			this.trvDepartments.SelectedNode = null;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000BE298 File Offset: 0x000BD298
		private int getDeptId(string deptName)
		{
			icControllerZone icControllerZone = new icControllerZone();
			int num = icControllerZone.getZoneID(deptName);
			if (num > 0)
			{
				return num;
			}
			string text = deptName;
			while (text.IndexOf("\\\\") >= 0)
			{
				text = text.Replace("\\\\", "\\");
			}
			if (text.Substring(0, 1) == "\\")
			{
				text = text.Substring(1);
			}
			if (text.Substring(text.Length - 1) == "\\")
			{
				text = text.Substring(0, text.Length - 1);
			}
			num = icControllerZone.getZoneID(text);
			if (num > 0)
			{
				return num;
			}
			string[] array = text.Split(new char[] { '\\' });
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (text2 == "")
				{
					text2 = array[i];
				}
				else
				{
					text2 = text2 + "\\" + array[i];
				}
				if (flag || !icControllerZone.checkExisted(text2))
				{
					flag = true;
					icControllerZone.addNew4BatchExcel(text2);
				}
			}
			return icControllerZone.getZoneID(text);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000BE3B0 File Offset: 0x000BD3B0
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuZones";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnAddSuper.Visible = false;
				this.btnDeleteDept.Visible = false;
				this.btnEditDept.Visible = false;
				this.btnMoveDept.Visible = false;
				this.toolStrip1.Visible = false;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000BE41C File Offset: 0x000BD41C
		private void loadZone()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadZone_Acc();
				return;
			}
			this.trvDepartments.Nodes.Clear();
			bool flag = false;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC", sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
					while (sqlDataReader.Read())
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Text = wgTools.SetObjToStr(sqlDataReader[0]);
						treeNode.Tag = wgTools.SetObjToStr(sqlDataReader[0]);
						if (treeNode.Text.LastIndexOf("\\") > 0)
						{
							flag = true;
							string text = treeNode.Text.Substring(0, treeNode.Text.LastIndexOf("\\"));
							treeNode.Text = treeNode.Text.Substring(treeNode.Text.LastIndexOf("\\") + 1);
							using (IEnumerator enumerator = this.trvDepartments.Nodes.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									TreeNode treeNode2 = (TreeNode)obj;
									TreeNode treeNode3;
									this.FindRecursive(treeNode2, text, out treeNode3);
									if (treeNode3 != null)
									{
										treeNode3.Nodes.Add(treeNode);
									}
								}
								continue;
							}
						}
						this.trvDepartments.Nodes.Add(treeNode);
					}
					sqlDataReader.Close();
					this.trvDepartments.ExpandAll();
				}
			}
			if (this.trvDepartments.Nodes.Count == 0 && flag)
			{
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand2 = new SqlCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC", sqlConnection2))
					{
						sqlConnection2.Open();
						SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader(CommandBehavior.Default);
						while (sqlDataReader2.Read())
						{
							TreeNode treeNode4 = new TreeNode();
							treeNode4.Text = wgTools.SetObjToStr(sqlDataReader2[0]);
							treeNode4.Tag = wgTools.SetObjToStr(sqlDataReader2[0]);
							if (treeNode4.Text.LastIndexOf("\\") > 0)
							{
								string text2 = treeNode4.Text.Substring(0, treeNode4.Text.LastIndexOf("\\"));
								treeNode4.Text = treeNode4.Text.Substring(treeNode4.Text.LastIndexOf("\\") + 1);
								this.getDeptId(text2);
							}
						}
						sqlDataReader2.Close();
					}
				}
				new icControllerZone().updateZoneNO();
				using (SqlConnection sqlConnection3 = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand3 = new SqlCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC", sqlConnection3))
					{
						sqlConnection3.Open();
						SqlDataReader sqlDataReader3 = sqlCommand3.ExecuteReader(CommandBehavior.Default);
						while (sqlDataReader3.Read())
						{
							TreeNode treeNode5 = new TreeNode();
							treeNode5.Text = wgTools.SetObjToStr(sqlDataReader3[0]);
							treeNode5.Tag = wgTools.SetObjToStr(sqlDataReader3[0]);
							if (treeNode5.Text.LastIndexOf("\\") > 0)
							{
								flag = true;
								string text3 = treeNode5.Text.Substring(0, treeNode5.Text.LastIndexOf("\\"));
								treeNode5.Text = treeNode5.Text.Substring(treeNode5.Text.LastIndexOf("\\") + 1);
								using (IEnumerator enumerator2 = this.trvDepartments.Nodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										TreeNode treeNode6 = (TreeNode)obj2;
										TreeNode treeNode7;
										this.FindRecursive(treeNode6, text3, out treeNode7);
										if (treeNode7 != null)
										{
											treeNode7.Nodes.Add(treeNode5);
										}
									}
									continue;
								}
							}
							this.trvDepartments.Nodes.Add(treeNode5);
						}
						sqlDataReader3.Close();
						this.trvDepartments.ExpandAll();
					}
				}
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000BE8F0 File Offset: 0x000BD8F0
		private void loadZone_Acc()
		{
			this.trvDepartments.Nodes.Clear();
			using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("SELECT f_ZoneName,f_ZoneNO FROM t_b_Controller_Zone ORDER BY f_ZoneName  + '\\' ASC", oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
					while (oleDbDataReader.Read())
					{
						TreeNode treeNode = new TreeNode();
						treeNode.Text = wgTools.SetObjToStr(oleDbDataReader[0]);
						treeNode.Tag = wgTools.SetObjToStr(oleDbDataReader[0]);
						if (treeNode.Text.LastIndexOf("\\") > 0)
						{
							string text = treeNode.Text.Substring(0, treeNode.Text.LastIndexOf("\\"));
							treeNode.Text = treeNode.Text.Substring(treeNode.Text.LastIndexOf("\\") + 1);
							using (IEnumerator enumerator = this.trvDepartments.Nodes.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									TreeNode treeNode2 = (TreeNode)obj;
									TreeNode treeNode3;
									this.FindRecursive(treeNode2, text, out treeNode3);
									if (treeNode3 != null)
									{
										treeNode3.Nodes.Add(treeNode);
									}
								}
								continue;
							}
						}
						this.trvDepartments.Nodes.Add(treeNode);
					}
					oleDbDataReader.Close();
					this.trvDepartments.ExpandAll();
				}
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000BEAAC File Offset: 0x000BDAAC
		private void trvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.trvDepartments.SelectedNode != null)
			{
				this.txtSelectedDept.Text = this.trvDepartments.SelectedNode.Tag.ToString();
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000BEADC File Offset: 0x000BDADC
		private void txtSelectedDept_TextChanged(object sender, EventArgs e)
		{
			if (this.txtSelectedDept.Text.Length > 0)
			{
				this.btnDeleteDept.Enabled = true;
				this.btnAdd.Enabled = true;
				this.btnEditDept.Enabled = true;
				this.btnMoveDept.Visible = true;
				return;
			}
			this.btnDeleteDept.Enabled = false;
			this.btnAdd.Enabled = false;
			this.btnEditDept.Enabled = false;
			this.btnMoveDept.Visible = false;
		}
	}
}
