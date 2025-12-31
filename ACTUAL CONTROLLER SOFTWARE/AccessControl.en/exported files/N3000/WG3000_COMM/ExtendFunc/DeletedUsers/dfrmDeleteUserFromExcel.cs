using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.DeletedUsers
{
	// Token: 0x02000232 RID: 562
	public partial class dfrmDeleteUserFromExcel : frmN3000
	{
		// Token: 0x060010D8 RID: 4312 RVA: 0x00133CD6 File Offset: 0x00132CD6
		public dfrmDeleteUserFromExcel()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dgvSelectedUsers);
			wgAppConfig.custDataGridview(ref this.dgvUsers);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00133D10 File Offset: 0x00132D10
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00133D20 File Offset: 0x00132D20
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				string text = string.Format("{0}\r\n\r\n{1}=  {2}", this.btnOK.Text, CommonStr.strUsersNum, this.arrConsumerID.Count);
				if (XMessageBox.Show(string.Format(CommonStr.strAreYouSure + " {0} ?", text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					icConsumer icConsumer = new icConsumer();
					text = string.Format("{0} {1}=  {2}    ", this.Text, CommonStr.strUsersNum, this.arrConsumerID.Count);
					int count = this.arrConsumerID.Count;
					for (int i = 0; i < this.arrConsumerID.Count; i++)
					{
						int num = (int)this.arrConsumerID[i];
						icConsumer.dimissionUser(num);
						text = text + "," + this.arrConsumerNO[i];
					}
					wgAppConfig.wgLog(text);
					icConsumerShare.setUpdateLog();
					base.DialogResult = DialogResult.OK;
					XMessageBox.Show(CommonStr.strNeedUploadPrivilegeDeleteUser, wgTools.MSGTITLE, MessageBoxButtons.OK);
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00133E4C File Offset: 0x00132E4C
		private void dfrmDeleteUserFromExcel_Load(object sender, EventArgs e)
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
				this.openFileDialog1.Title = this.Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					wgTools.WriteLine("start");
					int num = 0;
					int num2 = 1;
					int num3 = 2;
					int num4 = 3;
					OleDbConnection oleDbConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
					bool flag = false;
					bool flag2 = false;
					int i = 0;
					while (i < 2)
					{
						try
						{
							oleDbConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source= " + fileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE'");
							string text = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES;IMEX=1;MAXSCANROWS=15;READONLY=FALSE\"";
							if (fileName != string.Empty)
							{
								FileInfo fileInfo = new FileInfo(fileName);
								if (fileInfo.Extension.Equals(".xls"))
								{
									oleDbConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									switch (i)
									{
									case 0:
										oleDbConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
										break;
									case 1:
										oleDbConnection = new OleDbConnection(string.Format(text, new object[] { "ACE", "12.0", fileName, "8.0" }));
										break;
									}
								}
								else if (fileInfo.Extension.Equals(".xlsx"))
								{
									oleDbConnection = new OleDbConnection(string.Format(text, new object[] { "Ace", "12.0", fileName, "12.0" }));
									i = 1;
								}
								else
								{
									oleDbConnection = new OleDbConnection(string.Format(text, new object[] { "Jet", "4.0", fileName, "8.0" }));
									i = 1;
								}
							}
							try
							{
								oleDbConnection.Open();
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
					DataSet dataSet = new DataSet();
					DataTable dataTable = null;
					if (flag)
					{
						dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						oleDbConnection.Close();
					}
					string text2 = "";
					if (dataTable.Rows.Count <= 0)
					{
						XMessageBox.Show(this.Text + ": " + 0);
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
						num = -1;
						num2 = -1;
						num3 = -1;
						num4 = -1;
						if (text2.IndexOf("$") <= 0)
						{
							text2 += "$";
						}
						try
						{
							OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter("select * from [" + text2 + "A1:Z1]", oleDbConnection);
							oleDbDataAdapter.Fill(dataSet, "userInfoTitle");
							string columnName = dataSet.Tables["userInfoTitle"].Columns[0].ColumnName;
							for (int k = 0; k <= dataSet.Tables["userInfoTitle"].Columns.Count - 1; k++)
							{
								object columnName2 = dataSet.Tables["userInfoTitle"].Columns[k].ColumnName;
								if (wgTools.SetObjToStr(columnName2) != "")
								{
									string text3;
									if (wgTools.SetObjToStr(columnName2).ToUpper() == "User ID".ToUpper())
									{
										num = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "User Name".ToUpper())
									{
										num2 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Card NO".ToUpper())
									{
										num3 = k;
									}
									else if (wgTools.SetObjToStr(columnName2).ToUpper() == "Department".ToUpper() || wgTools.SetObjToStr(columnName2).ToUpper() == CommonStr.strReplaceFloorRoom.ToUpper())
									{
										num4 = k;
									}
									else
										switch (text3 = wgTools.SetObjToStr(columnName2).ToUpper().Substring(0, 2))
										{
										case "NO":
										case "用户":
										case "用戶":
										case "编号":
										case "編號":
										case "WO":
										case "工号":
										case "工號":
											num = k;
											break;
										case "NA":
										case "姓名":
											num2 = k;
											break;
										case "CA":
										case "卡号":
										case "卡號":
											num3 = k;
											break;
										case "DE":
										case "部门":
										case "部門":
											num4 = k;
											break;
										}
								}
							}
						}
						catch (Exception ex2)
						{
							wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							wgAppConfig.wgLog(ex2.ToString());
						}
						if (num < 0)
						{
							XMessageBox.Show(CommonStr.strNeedIncludeConsumerNODeleteUser);
						}
						else
						{
							try
							{
								int num6 = Math.Max(Math.Max(Math.Max(num, num2), num3), num4);
								string text4 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
								if (num6 < text4.Length)
								{
									new OleDbDataAdapter(string.Concat(new string[]
									{
										"select * from [",
										text2,
										"A1:",
										text4.Substring(num6, 1),
										"65535]"
									}), oleDbConnection).Fill(dataSet, "userInfo");
								}
							}
							catch (Exception ex3)
							{
								wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
								wgAppConfig.wgLog(ex3.ToString());
							}
							new DataView(dataSet.Tables["userInfo"]);
							DataTable dataTable2 = dataSet.Tables["userInfo"];
							DataTable dataTable3 = dataTable2.Clone();
							DataTable dataTable4 = dataTable2.Clone();
							this.dgvUsers.Columns.Clear();
							this.dgvSelectedUsers.Columns.Clear();
							string text5 = "";
							for (int l = 0; l <= dataTable2.Rows.Count - 1; l++)
							{
								DataRow dataRow = dataTable2.Rows[l];
								if (num >= 0)
								{
									text5 = wgTools.SetObjToStr(dataRow[num]).Trim();
								}
								int valBySql = wgAppConfig.getValBySql("SELECT f_ConsumerID From t_b_consumer where  RTRIM(LTRIM(f_ConsumerNO)) = " + wgTools.PrepareStrNUnicode(text5));
								if (valBySql > 0)
								{
									DataRow dataRow2 = dataTable4.NewRow();
									for (int m = 0; m < dataTable4.Columns.Count; m++)
									{
										dataRow2[m] = dataRow[m];
									}
									dataTable4.Rows.Add(dataRow2);
									this.arrConsumerID.Add(valBySql);
									this.arrConsumerNO.Add(text5);
								}
								else
								{
									DataRow dataRow3 = dataTable3.NewRow();
									for (int n = 0; n < dataTable3.Columns.Count; n++)
									{
										dataRow3[n] = dataRow[n];
									}
									dataTable3.Rows.Add(dataRow3);
								}
							}
							if (dataTable4.Rows.Count > 0)
							{
								this.btnOK.Enabled = true;
							}
							else
							{
								this.btnOK.Enabled = false;
							}
							this.dgvSelectedUsers.DataSource = dataTable4;
							this.dgvUsers.DataSource = dataTable3;
							this.dgvUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
							this.dgvSelectedUsers.DefaultCellStyle.ForeColor = SystemColors.WindowText;
							if (dataTable3.Rows.Count > 0)
							{
								this.label1.Text = string.Format("{0}  [{1}]", this.label1.Text, dataTable3.Rows.Count);
							}
							if (dataTable4.Rows.Count > 0)
							{
								this.label3.Text = string.Format("{0}  [{1}]", this.label3.Text, dataTable4.Rows.Count);
							}
						}
					}
				}
				else
				{
					base.Close();
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
			}
		}

		// Token: 0x04001DFB RID: 7675
		private ArrayList arrConsumerID = new ArrayList();

		// Token: 0x04001DFC RID: 7676
		private ArrayList arrConsumerNO = new ArrayList();
	}
}
