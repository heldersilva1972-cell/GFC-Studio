using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x0200004F RID: 79
	public partial class frmDepartments : frmN3000
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x0009E768 File Offset: 0x0009D768
		public frmDepartments()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0009E7C4 File Offset: 0x0009D7C4
		private void btnAdd_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripButton).Text;
				dfrmInputNewName.label1.Text = wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment);
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				this.txtDeptName.Text = dfrmInputNewName.strNewName;
			}
			if (!(this.txtDeptName.Text.Trim() == ""))
			{
				if (this.txtDeptName.Text.LastIndexOf("\\") >= 0)
				{
					XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtDeptName.Text = this.txtDeptName.Text.Trim();
				string text = this.txtDeptName.Text;
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
							if (treeNode.Tag.ToString() == text)
							{
								XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
						}
						goto IL_01ED;
					}
				}
				text = this.trvDepartments.SelectedNode.Tag + "\\" + text;
				foreach (object obj2 in this.trvDepartments.SelectedNode.Nodes)
				{
					TreeNode treeNode2 = (TreeNode)obj2;
					if (treeNode2.Tag.ToString() == text)
					{
						XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				IL_01ED:
				icGroup icGroup = new icGroup();
				if (icGroup.checkExisted(text))
				{
					XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icGroup.addNew(text);
				TreeNode treeNode3 = new TreeNode();
				treeNode3.Text = this.txtDeptName.Text;
				treeNode3.Tag = text;
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

		// Token: 0x060005A1 RID: 1441 RVA: 0x0009EAAC File Offset: 0x0009DAAC
		private void btnAddSuper_Click(object sender, EventArgs e)
		{
			this.trvDepartments.SelectedNode = null;
			this.btnAdd_Click(sender, e);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0009EAC4 File Offset: 0x0009DAC4
		private void btnDeleteDept_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this.btnDeleteDept.Text + "\r\n\r\n" + this.txtSelectedDept.Text + "?", wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				new icGroup().delete(this.txtSelectedDept.Text);
				wgAppConfig.wgLog(this.btnDeleteDept.Text + ":" + this.txtSelectedDept.Text);
				this.trvDepartments.Nodes.Remove(this.trvDepartments.SelectedNode);
				this.txtSelectedDept.Text = "";
				this.txtDeptName.Text = "";
				this.trvDepartments.SelectedNode = null;
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0009EB8C File Offset: 0x0009DB8C
		private void btnEditDept_Click(object sender, EventArgs e)
		{
			using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
			{
				dfrmInputNewName.Text = (sender as ToolStripButton).Text;
				dfrmInputNewName.label1.Text = wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartment);
				dfrmInputNewName.strNewName = this.txtSelectedDept.Text;
				if (this.txtSelectedDept.Text.LastIndexOf("\\") > 0)
				{
					dfrmInputNewName.strNewName = this.txtSelectedDept.Text.Substring(this.txtSelectedDept.Text.LastIndexOf("\\") + 1);
				}
				if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				this.txtDeptName.Text = dfrmInputNewName.strNewName;
			}
			if (!(this.txtDeptName.Text.Trim() == ""))
			{
				if (this.txtDeptName.Text.LastIndexOf("\\") >= 0)
				{
					XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icGroup icGroup = new icGroup();
				string text;
				if (this.txtSelectedDept.Text.LastIndexOf("\\") < 0)
				{
					text = this.txtDeptName.Text;
				}
				else
				{
					text = this.txtSelectedDept.Text.Substring(0, this.txtSelectedDept.Text.LastIndexOf("\\")) + "\\" + this.txtDeptName.Text;
				}
				if (icGroup.checkExisted(text))
				{
					XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				icGroup.Update(this.txtSelectedDept.Text, text);
				wgAppConfig.wgLog(string.Concat(new string[]
				{
					this.btnEditDept.Text,
					":",
					this.txtSelectedDept.Text,
					" =>",
					text
				}));
				if (this.trvDepartments.SelectedNode != null)
				{
					this.trvDepartments.SelectedNode.Text = this.txtDeptName.Text;
					this.trvDepartments.SelectedNode.Tag = text;
					this.txtSelectedDept.Text = text;
				}
				else
				{
					this.txtSelectedDept.Text = "";
				}
				this.loadDepartment();
				return;
			}
			else
			{
				XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0009EE00 File Offset: 0x0009DE00
		private void btnMoveDept_Click(object sender, EventArgs e)
		{
			using (dfrmDepartmentMove dfrmDepartmentMove = new dfrmDepartmentMove())
			{
				dfrmDepartmentMove.selectedGroupName = this.txtSelectedDept.Text;
				if (dfrmDepartmentMove.ShowDialog(this) == DialogResult.OK)
				{
					wgAppConfig.wgLog(this.btnMoveDept.Text + ":" + this.txtSelectedDept.Text);
					this.loadDepartment();
				}
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0009EE78 File Offset: 0x0009DE78
		private void copySelectedDeptToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = this.txtSelectedDept.Text;
			if (!string.IsNullOrEmpty(text))
			{
				using (dfrmInputNewName dfrmInputNewName = new dfrmInputNewName())
				{
					dfrmInputNewName.Text = (sender as ToolStripButton).Text + "---" + text;
					dfrmInputNewName.label1.Text = wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartmentNew);
					if (dfrmInputNewName.ShowDialog(this) != DialogResult.OK)
					{
						return;
					}
					this.txtDeptName.Text = dfrmInputNewName.strNewName;
				}
				if (!(this.txtDeptName.Text.Trim() == ""))
				{
					if (this.txtDeptName.Text.LastIndexOf("\\") >= 0)
					{
						XMessageBox.Show(this, CommonStr.strNotIncludeBackSlash, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					this.txtDeptName.Text = this.txtDeptName.Text.Trim();
					string text2 = this.txtDeptName.Text;
					if (text.IndexOf("\\") > 0)
					{
						text2 = text.Substring(0, text.LastIndexOf("\\") + 1) + text2;
					}
					icGroup icGroup = new icGroup();
					if (icGroup.checkExisted(text2))
					{
						XMessageBox.Show(this, text2 + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					ArrayList arrayList = new ArrayList();
					ArrayList arrayList2 = new ArrayList();
					ArrayList arrayList3 = new ArrayList();
					icGroup.getGroup(ref arrayList, ref arrayList2, ref arrayList3);
					icGroup.addNew(text2);
					foreach (object obj in arrayList)
					{
						string text3 = (string)obj;
						if (text3.IndexOf(text + "\\") == 0)
						{
							icGroup.addNew(text2 + "\\" + text3.Substring(text.Length + 1));
						}
					}
					this.loadDepartment();
					return;
				}
				else
				{
					XMessageBox.Show(this, CommonStr.strNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0009F09C File Offset: 0x0009E09C
		private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "SELECT f_GroupNO,  f_GroupName FROM t_b_Group ORDER BY f_GroupName  + '\\' ASC";
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

		// Token: 0x060005A7 RID: 1447 RVA: 0x0009F274 File Offset: 0x0009E274
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

		// Token: 0x060005A8 RID: 1448 RVA: 0x0009F2F4 File Offset: 0x0009E2F4
		private void frmDepartments_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				if (this.dfrmWait1 != null)
				{
					this.dfrmWait1.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0009F32C File Offset: 0x0009E32C
		private void frmDepartments_Load(object sender, EventArgs e)
		{
			this.bFindActive = false;
			this.txtDeptName_TextChanged(null, null);
			this.txtSelectedDept_TextChanged(null, null);
			this.loadOperatorPrivilege();
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			if (this.arrGroupID.Count > 0 && (int)this.arrGroupID[0] != 0)
			{
				this.strDepartmentControl = (string)this.arrGroupName[0];
				this.btnAddSuper.Enabled = false;
			}
			this.loadDepartment();
			this.txtSelectedDept.Text = "";
			this.txtDeptName.Text = "";
			this.trvDepartments.SelectedNode = null;
			this.Text = wgAppConfig.ReplaceFloorRoom(this.Text);
			this.toolStripLabel1.Text = wgAppConfig.ReplaceFloorRoom(this.toolStripLabel1.Text);
			this.btnAddSuper.Text = wgAppConfig.ReplaceFloorRoom(this.btnAddSuper.Text);
			this.btnAdd.Text = wgAppConfig.ReplaceFloorRoom(this.btnAdd.Text);
			this.btnEditDept.Text = wgAppConfig.ReplaceFloorRoom(this.btnEditDept.Text);
			this.btnDeleteDept.Text = wgAppConfig.ReplaceFloorRoom(this.btnDeleteDept.Text);
			this.toolStripLabel2.Text = wgAppConfig.ReplaceFloorRoom(this.toolStripLabel2.Text);
			this.btnMoveDept.Text = wgAppConfig.ReplaceFloorRoom(this.btnMoveDept.Text);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0009F4B8 File Offset: 0x0009E4B8
		private int getDeptId(string deptName)
		{
			icGroup icGroup = new icGroup();
			int num = icGroup.getGroupID(deptName);
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
			num = icGroup.getGroupID(text);
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
				if (flag || !icGroup.checkExisted(text2))
				{
					flag = true;
					icGroup.addNew4BatchExcel(text2);
				}
			}
			return icGroup.getGroupID(text);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0009F5D0 File Offset: 0x0009E5D0
		private void importFromExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				this.openFileDialog1.Filter = " (*.xls)|*.xls| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				try
				{
					this.openFileDialog1.InitialDirectory = ".\\REPORT";
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				this.openFileDialog1.Title = this.importFromExcelToolStripMenuItem.Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					this.dfrmWait1.Show();
					this.dfrmWait1.Refresh();
					wgTools.WriteLine("start");
					int num = 0;
					int num2 = 1;
					int num3 = 2;
					int num4 = 3;
					bool flag = false;
					bool flag2 = false;
					int i = 0;
					while (i < 2)
					{
						try
						{
							this.MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
							string text = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE\"";
							if (fileName != string.Empty)
							{
								FileInfo fileInfo = new FileInfo(fileName);
								if (fileInfo.Extension.Equals(".xls"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									switch (i)
									{
									case 0:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
										break;
									case 1:
										this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "ACE", "12.0", fileName, "8.0" }));
										break;
									}
								}
								else if (fileInfo.Extension.Equals(".xlsx"))
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Ace", "12.0", fileName, "12.0" }));
									i = 1;
								}
								else
								{
									this.MyConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									i = 1;
								}
							}
							try
							{
								this.MyConnection.Open();
								flag2 = false;
								flag = true;
							}
							catch (OleDbException)
							{
								flag2 = true;
								throw;
							}
							catch
							{
								throw;
							}
						}
						catch
						{
							if (i == 1)
							{
								if (flag2)
								{
									XMessageBox.Show(CommonStr.strSaveAsXLSFile);
								}
								throw;
							}
						}
						i++;
						if (flag)
						{
							break;
						}
					}
					this.DS = new DataSet();
					DataTable dataTable = null;
					if (flag)
					{
						dataTable = this.MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						this.MyConnection.Close();
					}
					string text2 = "";
					if (dataTable.Rows.Count <= 0)
					{
						XMessageBox.Show(this.importFromExcelToolStripMenuItem.Text + ": " + 0);
					}
					else
					{
						text2 = wgTools.SetObjToStr(dataTable.Rows[0][2]);
						for (int j = 0; j <= dataTable.Rows.Count - 1; j++)
						{
							if (wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用户" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "用戶" || wgTools.SetObjToStr(dataTable.Rows[j][2]) == "Users")
							{
								text2 = wgTools.SetObjToStr(dataTable.Rows[j][2]);
								break;
							}
						}
						num4 = -1;
						if (text2.IndexOf("$") <= 0)
						{
							text2 += "$";
						}
						try
						{
							this.MyCommand = new OleDbDataAdapter("select * from [" + text2 + "A1:Z1]", this.MyConnection);
							this.MyCommand.Fill(this.DS, "userInfoTitle");
							string columnName = this.DS.Tables["userInfoTitle"].Columns[0].ColumnName;
							for (int k = 0; k <= this.DS.Tables["userInfoTitle"].Columns.Count - 1; k++)
							{
								object columnName2 = this.DS.Tables["userInfoTitle"].Columns[k].ColumnName;
								if (wgTools.SetObjToStr(columnName2) != "")
								{
									string text3;
									if (wgTools.SetObjToStr(columnName2).ToUpper() == "Department".ToUpper() || wgTools.SetObjToStr(columnName2).ToUpper() == CommonStr.strReplaceFloorRoom.ToUpper())
									{
										num4 = k;
									}
									else if ((text3 = wgTools.SetObjToStr(columnName2).ToUpper().Substring(0, 2)) != null && (text3 == "DE" || text3 == "部门" || text3 == "部門"))
									{
										num4 = k;
									}
								}
							}
						}
						catch (Exception ex2)
						{
							wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							wgAppConfig.wgLog(ex2.ToString());
						}
						string text4 = "";
						int num5 = 0;
						try
						{
							int num6 = Math.Max(Math.Max(Math.Max(num, num2), num3), num4);
							string text5 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
							if (num6 < text5.Length)
							{
								this.MyCommand = new OleDbDataAdapter(string.Concat(new string[]
								{
									"select * from [",
									text2,
									"A1:",
									text5.Substring(num6, 1),
									"65535]"
								}), this.MyConnection);
								this.MyCommand.Fill(this.DS, "userInfo");
							}
						}
						catch (Exception ex3)
						{
							wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
							wgAppConfig.wgLog(ex3.ToString());
						}
						DataView dataView = new DataView(this.DS.Tables["userInfo"]);
						int num7 = 0;
						icGroup icGroup = new icGroup();
						for (int l = 0; l <= dataView.Count - 1; l++)
						{
							if (num4 >= 0)
							{
								string text6 = wgTools.SetObjToStr(dataView[l][num4]).Trim();
								if (!string.IsNullOrEmpty(text6))
								{
									if (icGroup.getGroupID(text6) > 0)
									{
										text4 = text4 + text6 + ",";
										num5++;
									}
									else if (this.getDeptId(text6) > 0)
									{
										num7++;
									}
									else
									{
										text4 = text4 + text6 + ",";
										num5++;
									}
								}
							}
							if (l >= 65535)
							{
								break;
							}
							wgAppRunInfo.raiseAppRunInfoLoadNums((num5 + num7).ToString() + " / " + dataView.Count.ToString());
							this.dfrmWait1.Text = (num5 + num7).ToString() + " / " + dataView.Count.ToString();
							Application.DoEvents();
						}
						wgAppRunInfo.raiseAppRunInfoLoadNums((num5 + num7).ToString() + " / " + dataView.Count.ToString());
						this.dfrmWait1.Text = (num5 + num7).ToString() + " / " + dataView.Count.ToString();
						icGroup.updateGroupNO();
						wgTools.WriteLine("Import end");
						if (!(text4 == ""))
						{
							this.dfrmWait1.Hide();
							wgTools.WgDebugWrite(CommonStr.strNotImportedDepartment + num5.ToString() + "\r\n" + text4, new object[0]);
							XMessageBox.Show(CommonStr.strNotImportedDepartment + num5.ToString() + "\r\n\r\n" + text4);
							wgAppConfig.wgLog(CommonStr.strNotImportedDepartment + num5.ToString() + "\r\n" + text4);
						}
						this.dfrmWait1.Hide();
						wgAppConfig.wgLog(this.importFromExcelToolStripMenuItem.Text + ": " + num7);
						XMessageBox.Show(this.importFromExcelToolStripMenuItem.Text + ": " + num7);
						this.frmDepartments_Load(null, null);
					}
				}
			}
			catch (Exception ex4)
			{
				wgTools.WgDebugWrite(ex4.ToString(), new object[0]);
				wgAppConfig.wgLog(ex4.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
				Cursor.Current = Cursors.Default;
				this.dfrmWait1.Hide();
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0009FFC4 File Offset: 0x0009EFC4
		private bool isChildDepartment(string deptname)
		{
			if (!string.IsNullOrEmpty(this.strDepartmentControl))
			{
				foreach (object obj in this.arrGroupName)
				{
					string text = (string)obj;
					if (wgTools.SetObjToStr(deptname).IndexOf(text + "\\") == 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000A0044 File Offset: 0x0009F044
		private bool isCurrentDepartment(string deptname)
		{
			if (!string.IsNullOrEmpty(this.strDepartmentControl))
			{
				foreach (object obj in this.arrGroupName)
				{
					string text = (string)obj;
					if (text.Equals(wgTools.SetObjToStr(deptname)))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000A00B8 File Offset: 0x0009F0B8
		private bool isValidDepartment(string deptname)
		{
			if (!string.IsNullOrEmpty(this.strDepartmentControl))
			{
				foreach (object obj in this.arrGroupName)
				{
					string text = (string)obj;
					if (text.IndexOf(wgTools.SetObjToStr(deptname) + "\\") == 0 || text.Equals(wgTools.SetObjToStr(deptname)) || wgTools.SetObjToStr(deptname).IndexOf(text + "\\") == 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000A015C File Offset: 0x0009F15C
		private void loadDepartment()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadDepartment_Acc();
				return;
			}
			this.trvDepartments.Nodes.Clear();
			SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
			try
			{
				using (SqlCommand sqlCommand = new SqlCommand("SELECT f_GroupName,f_GroupNO FROM t_b_Group ORDER BY f_GroupName  + '\\' ASC", sqlConnection))
				{
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
					while (sqlDataReader.Read())
					{
						if (string.IsNullOrEmpty(this.strDepartmentControl) || this.isValidDepartment(wgTools.SetObjToStr(sqlDataReader[0])))
						{
							TreeNode treeNode = new TreeNode();
							treeNode.Text = wgTools.SetObjToStr(sqlDataReader[0]);
							treeNode.Tag = wgTools.SetObjToStr(sqlDataReader[0]);
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
					}
					sqlDataReader.Close();
					this.trvDepartments.ExpandAll();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000A0370 File Offset: 0x0009F370
		private void loadDepartment_Acc()
		{
			this.trvDepartments.Nodes.Clear();
			OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
			try
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand("SELECT f_GroupName,f_GroupNO FROM t_b_Group ORDER BY f_GroupName  + '\\' ASC", oleDbConnection))
				{
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
					while (oleDbDataReader.Read())
					{
						if (string.IsNullOrEmpty(this.strDepartmentControl) || this.isValidDepartment(wgTools.SetObjToStr(oleDbDataReader[0])))
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
					}
					oleDbDataReader.Close();
					this.trvDepartments.ExpandAll();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			finally
			{
				oleDbConnection.Close();
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000A0578 File Offset: 0x0009F578
		private void loadOperatorPrivilege()
		{
			bool flag = false;
			string text = "mnuGroups";
			if (icOperator.OperatePrivilegeVisible(text, ref flag) && flag)
			{
				this.btnAdd.Visible = false;
				this.btnAddSuper.Visible = false;
				this.btnDeleteDept.Visible = false;
				this.toolStripButton2.Visible = false;
				this.btnMoveDept.Visible = false;
				this.btnEditDept.Visible = false;
				this.toolStrip1.Visible = false;
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000A05EE File Offset: 0x0009F5EE
		private void trvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.trvDepartments.SelectedNode != null)
			{
				this.txtSelectedDept.Text = this.trvDepartments.SelectedNode.Tag.ToString();
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000A061D File Offset: 0x0009F61D
		private void txtDeptName_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000A0620 File Offset: 0x0009F620
		private void txtSelectedDept_TextChanged(object sender, EventArgs e)
		{
			if (this.txtSelectedDept.Text.Length > 0)
			{
				this.btnDeleteDept.Enabled = true;
				this.toolStripButton2.Enabled = true;
				this.btnMoveDept.Enabled = true;
				this.btnAdd.Enabled = true;
				this.btnEditDept.Enabled = true;
				if (!string.IsNullOrEmpty(this.strDepartmentControl) && !this.isChildDepartment(this.txtSelectedDept.Text))
				{
					if (this.isCurrentDepartment(this.txtSelectedDept.Text))
					{
						this.btnDeleteDept.Enabled = false;
						this.toolStripButton2.Enabled = false;
						this.btnMoveDept.Enabled = false;
						this.btnAdd.Enabled = true;
						this.btnEditDept.Enabled = false;
						return;
					}
					this.btnDeleteDept.Enabled = false;
					this.toolStripButton2.Enabled = false;
					this.btnMoveDept.Enabled = false;
					this.btnAdd.Enabled = false;
					this.btnEditDept.Enabled = false;
					return;
				}
			}
			else
			{
				this.btnDeleteDept.Enabled = false;
				this.toolStripButton2.Enabled = false;
				this.btnMoveDept.Enabled = false;
				this.btnAdd.Enabled = false;
				this.btnEditDept.Enabled = false;
			}
		}

		// Token: 0x04000AAF RID: 2735
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04000AB0 RID: 2736
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04000AB1 RID: 2737
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04000AB2 RID: 2738
		private dfrmWait dfrmWait1 = new dfrmWait();

		// Token: 0x04000AB3 RID: 2739
		private string strDepartmentControl = "";

		// Token: 0x04000AB4 RID: 2740
		private DataSet DS;

		// Token: 0x04000AB5 RID: 2741
		private OleDbDataAdapter MyCommand;

		// Token: 0x04000AB6 RID: 2742
		private OleDbConnection MyConnection;
	}
}
