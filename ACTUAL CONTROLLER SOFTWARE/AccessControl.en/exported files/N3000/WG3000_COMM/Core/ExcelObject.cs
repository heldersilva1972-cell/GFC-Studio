using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WG3000_COMM.Core
{
	// Token: 0x020001CE RID: 462
	public sealed class ExcelObject : IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000A33 RID: 2611 RVA: 0x000E317C File Offset: 0x000E217C
		// (remove) Token: 0x06000A34 RID: 2612 RVA: 0x000E31B4 File Offset: 0x000E21B4
		private event EventHandler connectionStringChange;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000A35 RID: 2613 RVA: 0x000E31E9 File Offset: 0x000E21E9
		// (remove) Token: 0x06000A36 RID: 2614 RVA: 0x000E31F2 File Offset: 0x000E21F2
		public event EventHandler ConnectionStringChanged
		{
			add
			{
				this.connectionStringChange += value;
			}
			remove
			{
				this.connectionStringChange -= value;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000A37 RID: 2615 RVA: 0x000E31FC File Offset: 0x000E21FC
		// (remove) Token: 0x06000A38 RID: 2616 RVA: 0x000E3234 File Offset: 0x000E2234
		private event ExcelObject.ProgressWork Reading;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000A39 RID: 2617 RVA: 0x000E3269 File Offset: 0x000E2269
		// (remove) Token: 0x06000A3A RID: 2618 RVA: 0x000E3272 File Offset: 0x000E2272
		public event ExcelObject.ProgressWork ReadProgress
		{
			add
			{
				this.Reading += value;
			}
			remove
			{
				this.Reading -= value;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000A3B RID: 2619 RVA: 0x000E327B File Offset: 0x000E227B
		// (remove) Token: 0x06000A3C RID: 2620 RVA: 0x000E3284 File Offset: 0x000E2284
		public event ExcelObject.ProgressWork WriteProgress
		{
			add
			{
				this.Writing += value;
			}
			remove
			{
				this.Writing -= value;
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000A3D RID: 2621 RVA: 0x000E3290 File Offset: 0x000E2290
		// (remove) Token: 0x06000A3E RID: 2622 RVA: 0x000E32C8 File Offset: 0x000E22C8
		private event ExcelObject.ProgressWork Writing;

		// Token: 0x06000A3F RID: 2623 RVA: 0x000E32FD File Offset: 0x000E22FD
		public ExcelObject(string path)
		{
			this.excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES\"";
			this.filepath = string.Empty;
			this.bForceACE = false;
			this.filepath = path;
			this.onConnectionStringChanged();
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x000E332F File Offset: 0x000E232F
		public ExcelObject(string path, bool ForceACE)
		{
			this.excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES\"";
			this.filepath = string.Empty;
			this.bForceACE = ForceACE;
			this.filepath = path;
			this.onConnectionStringChanged();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000E3364 File Offset: 0x000E2364
		public bool AddNewRow(DataRow dr)
		{
			using (OleDbCommand oleDbCommand = new OleDbCommand(this.GenerateInsertStatement(dr), this.Connection))
			{
				oleDbCommand.ExecuteNonQuery();
			}
			return true;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000E33A8 File Offset: 0x000E23A8
		public bool AddNewRow(DataGridViewRow dgvdr, DataGridView dgv)
		{
			using (OleDbCommand oleDbCommand = new OleDbCommand(this.GenerateInsertStatement(dgvdr, dgv), this.Connection))
			{
				oleDbCommand.ExecuteNonQuery();
			}
			return true;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000E33F0 File Offset: 0x000E23F0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000E3400 File Offset: 0x000E2400
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.con != null && this.con.State == ConnectionState.Open)
				{
					this.con.Close();
				}
				if (this.con != null)
				{
					this.con.Dispose();
					this.con = null;
				}
				if (this.filepath != null)
				{
					this.filepath = string.Empty;
				}
				if (this.ds != null)
				{
					this.ds.Dispose();
				}
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000E3474 File Offset: 0x000E2474
		public bool DropTable(string tablename)
		{
			bool flag;
			try
			{
				if (this.Connection.State != ConnectionState.Open)
				{
					this.Connection.Open();
					this.onWriteProgress(10f);
				}
				string text = "Drop Table [{0}]";
				using (OleDbCommand oleDbCommand = new OleDbCommand(string.Format(text, tablename), this.Connection))
				{
					this.onWriteProgress(30f);
					oleDbCommand.ExecuteNonQuery();
					this.onWriteProgress(80f);
				}
				this.Connection.Close();
				this.onWriteProgress(100f);
				flag = true;
			}
			catch (Exception ex)
			{
				this.onWriteProgress(0f);
				XMessageBox.Show(ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x000E353C File Offset: 0x000E253C
		private string GenerateCreateTable(DataView dv)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CREATE TABLE [{0}](", dv.Table.TableName);
			bool flag = true;
			foreach (object obj in dv.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				flag = false;
				if (dataColumn.DataType.ToString().IndexOf("System.Int") >= 0)
				{
					stringBuilder.AppendFormat("{0} {1}", dataColumn.ColumnName.ToString(), "Int");
				}
				else
				{
					stringBuilder.AppendFormat("{0} {1}", dataColumn.ColumnName.ToString(), dataColumn.DataType.ToString().Replace("System.", ""));
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString().Replace("\r\n", " ");
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x000E3658 File Offset: 0x000E2658
		private string GenerateCreateTable(DataGridView dgv)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CREATE TABLE [{0}](", "ExcelData");
			bool flag = true;
			foreach (object obj in dgv.Columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
				if (dataGridViewColumn.Visible)
				{
					string text = dataGridViewColumn.HeaderText.ToString().Replace("[", "(").Replace("]", ")")
						.Replace(".", " ")
						.Replace("\r\n", " ");
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					flag = false;
					if (dataGridViewColumn.Name.ToUpper().IndexOf("f_CardNO".ToUpper()) >= 0 && wgAppConfig.IsActivateCard19)
					{
						if (stringBuilder.ToString().IndexOf(string.Format("[{0}]", text)) >= 0)
						{
							stringBuilder.AppendFormat("[{0}] {1}", text + dataGridViewColumn.Index.ToString(), "String");
						}
						else
						{
							stringBuilder.AppendFormat("[{0}] {1}", text, "String");
						}
					}
					else if (dataGridViewColumn.ValueType.Name.ToString().IndexOf("Int") >= 0)
					{
						if (stringBuilder.ToString().IndexOf(string.Format("[{0}]", text)) >= 0)
						{
							stringBuilder.AppendFormat("[{0}] {1}", text + dataGridViewColumn.Index.ToString(), "Int");
						}
						else
						{
							stringBuilder.AppendFormat("[{0}] {1}", text, "Int");
						}
					}
					else if (dataGridViewColumn.ValueType.Name.ToString().IndexOf("DateTime") >= 0)
					{
						if (stringBuilder.ToString().IndexOf(string.Format("[{0}]", text)) >= 0)
						{
							stringBuilder.AppendFormat("[{0}] {1}", text + dataGridViewColumn.Index.ToString(), "String");
						}
						else
						{
							stringBuilder.AppendFormat("[{0}] {1}", text, "String");
						}
					}
					else if (stringBuilder.ToString().IndexOf(string.Format("[{0}]", text)) >= 0)
					{
						stringBuilder.AppendFormat("[{0}] {1}", text + dataGridViewColumn.Index.ToString(), dataGridViewColumn.ValueType.Name.ToString());
					}
					else
					{
						stringBuilder.AppendFormat("[{0}] {1}", text, dataGridViewColumn.ValueType.Name.ToString());
					}
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString().Replace("\r\n", " ").Replace(".", " ");
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000E394C File Offset: 0x000E294C
		private string GenerateCreateTable(string tableName, Dictionary<string, string> tableDefination)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CREATE TABLE [{0}](", tableName);
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in tableDefination)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				flag = false;
				stringBuilder.AppendFormat("{0} {1}", keyValuePair.Key, keyValuePair.Value);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString().Replace("\r\n", " ");
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x000E39F8 File Offset: 0x000E29F8
		private string GenerateInsertStatement(DataRow dr)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			stringBuilder.AppendFormat("INSERT INTO [{0}](", dr.Table.TableName);
			foreach (object obj in dr.Table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				flag = false;
				stringBuilder.Append(dataColumn.Caption);
			}
			stringBuilder.Append(") VALUES(");
			for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
			{
				if (!object.ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
				{
					stringBuilder.Append("'");
					stringBuilder.Append(dr[i].ToString().Replace("'", "''"));
					stringBuilder.Append("'");
				}
				else
				{
					stringBuilder.Append(dr[i].ToString().Replace("'", "''"));
				}
				if (i != dr.Table.Columns.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString().Replace("\r\n", " ");
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000E3B8C File Offset: 0x000E2B8C
		private string GenerateInsertStatement(DataGridViewRow dgvdr, DataGridView dgv)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("INSERT INTO [{0}](", "ExcelData");
			bool flag = true;
			foreach (object obj in dgv.Columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
				if (dataGridViewColumn.Visible)
				{
					string text = dataGridViewColumn.HeaderText.ToString().Replace("[", "(").Replace("]", ")")
						.Replace(".", " ")
						.Replace("\r\n", " ");
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					flag = false;
					if (stringBuilder.ToString().IndexOf(string.Format("[{0}]", text)) >= 0)
					{
						stringBuilder.AppendFormat(string.Format("[{0}]", text + dataGridViewColumn.Index.ToString()), new object[0]);
					}
					else
					{
						stringBuilder.AppendFormat(string.Format("[{0}]", text), new object[0]);
					}
				}
			}
			stringBuilder.Append(") VALUES(");
			string text2 = stringBuilder.ToString().Replace("\r\n", " ").Replace(".", " ");
			stringBuilder = null;
			stringBuilder = new StringBuilder();
			stringBuilder.Append(text2);
			flag = true;
			for (int i = 0; i <= dgv.Columns.Count - 1; i++)
			{
				if (dgv.Columns[i].Visible)
				{
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					flag = false;
					if (dgvdr.Cells[i].Value == null)
					{
						stringBuilder.Append("NULL");
					}
					else if (dgvdr.Cells[i].Value == DBNull.Value)
					{
						stringBuilder.Append("NULL");
					}
					else if (dgvdr.Cells[i].Value.ToString().Trim() == "")
					{
						stringBuilder.Append("NULL");
					}
					else if (dgv.Columns[i].ValueType.Name.ToString().IndexOf("Int") < 0)
					{
						if (dgv.Columns[i].ValueType.Name.ToString().IndexOf("DateTime") >= 0)
						{
							stringBuilder.Append("'");
							if (string.IsNullOrEmpty(dgv.Columns[i].DefaultCellStyle.Format))
							{
								stringBuilder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
							}
							else
							{
								stringBuilder.Append(((DateTime)dgvdr.Cells[i].Value).ToString(dgv.Columns[i].DefaultCellStyle.Format).Replace("'", "''"));
							}
							stringBuilder.Append("'");
						}
						else
						{
							stringBuilder.Append("'");
							stringBuilder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
							stringBuilder.Append("'");
						}
					}
					else
					{
						stringBuilder.Append(dgvdr.Cells[i].Value.ToString().Replace("'", "''"));
					}
				}
			}
			if (ExcelObject.bOnly300ExcelData && stringBuilder.Length > 300)
			{
				stringBuilder.Remove(300, stringBuilder.Length - 300).Append("......'");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString().Replace("\r\n", "\n");
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000E3FC0 File Offset: 0x000E2FC0
		public DataTable GetSchema()
		{
			if (this.Connection.State != ConnectionState.Open)
			{
				this.Connection.Open();
			}
			object[] array = new object[] { null, null, null, "TABLE" };
			return this.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, array);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000E4008 File Offset: 0x000E3008
		public void onConnectionStringChanged()
		{
			if (this.Connection != null && !this.Connection.ConnectionString.Equals(this.ConnectionString))
			{
				if (this.Connection.State == ConnectionState.Open)
				{
					this.Connection.Close();
				}
				this.Connection.Dispose();
				this.con = null;
			}
			if (this.connectionStringChange != null)
			{
				this.connectionStringChange(this, new EventArgs());
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000E4079 File Offset: 0x000E3079
		public void onReadProgress(float percentage)
		{
			if (this.Reading != null)
			{
				this.Reading(percentage);
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000E408F File Offset: 0x000E308F
		public void onWriteProgress(float percentage)
		{
			if (this.Writing != null)
			{
				this.Writing(percentage);
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000E40A5 File Offset: 0x000E30A5
		public DataTable ReadTable(string tableName)
		{
			return this.ReadTable(tableName, "");
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000E40B4 File Offset: 0x000E30B4
		public DataTable ReadTable(string tableName, string criteria)
		{
			DataTable dataTable;
			try
			{
				if (this.Connection.State != ConnectionState.Open)
				{
					this.Connection.Open();
					this.onReadProgress(10f);
				}
				string text = "Select * from [{0}]";
				if (!string.IsNullOrEmpty(criteria))
				{
					text = text + " Where " + criteria;
				}
				using (OleDbCommand oleDbCommand = new OleDbCommand(string.Format(text, tableName)))
				{
					oleDbCommand.Connection = this.Connection;
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
					{
						this.onReadProgress(30f);
						this.ds = new DataSet();
						this.onReadProgress(50f);
						oleDbDataAdapter.Fill(this.ds, tableName);
						this.onReadProgress(100f);
						if (this.ds.Tables.Count == 1)
						{
							return this.ds.Tables[0];
						}
						dataTable = null;
					}
				}
			}
			catch
			{
				XMessageBox.Show("Table Cannot be read");
				dataTable = null;
			}
			return dataTable;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000E41DC File Offset: 0x000E31DC
		public bool test_WriteTable()
		{
			bool flag;
			try
			{
				string text = "CREATE TABLE [users](f_ConsumerID Int,编号 CHAR(50),姓名 CHAR(100),卡号 Int,部门 String,考勤 Byte,倒班 Byte,门禁 Byte,起始日期 DateTime,截止日期 DateTime)";
				using (OleDbCommand oleDbCommand = new OleDbCommand(text, this.Connection))
				{
					if (this.Connection.State != ConnectionState.Open)
					{
						this.Connection.Open();
					}
					oleDbCommand.ExecuteNonQuery();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000E425C File Offset: 0x000E325C
		public bool WriteTable(DataView dv)
		{
			bool flag;
			try
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(this.GenerateCreateTable(dv), this.Connection))
				{
					if (this.Connection.State != ConnectionState.Open)
					{
						this.Connection.Open();
					}
					oleDbCommand.ExecuteNonQuery();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000E42DC File Offset: 0x000E32DC
		public bool WriteTable(DataGridView dgv)
		{
			bool flag;
			try
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(this.GenerateCreateTable(dgv), this.Connection))
				{
					if (this.Connection.State != ConnectionState.Open)
					{
						this.Connection.Open();
					}
					oleDbCommand.ExecuteNonQuery();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000E435C File Offset: 0x000E335C
		public bool WriteTable(string tableName, Dictionary<string, string> tableDefination)
		{
			bool flag;
			try
			{
				using (OleDbCommand oleDbCommand = new OleDbCommand(this.GenerateCreateTable(tableName, tableDefination), this.Connection))
				{
					if (this.Connection.State != ConnectionState.Open)
					{
						this.Connection.Open();
					}
					oleDbCommand.ExecuteNonQuery();
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x000E43D0 File Offset: 0x000E33D0
		public OleDbConnection Connection
		{
			get
			{
				if (this.con == null)
				{
					OleDbConnection oleDbConnection = new OleDbConnection(this.ConnectionString);
					this.con = oleDbConnection;
				}
				return this.con;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x000E4400 File Offset: 0x000E3400
		public string ConnectionString
		{
			get
			{
				if (this.filepath == string.Empty)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("KEY_EXCELMODE_STRING")))
				{
					return string.Format(wgAppConfig.GetKeyVal("KEY_EXCELMODE_STRING"), this.filepath);
				}
				FileInfo fileInfo = new FileInfo(this.filepath);
				if (fileInfo.Extension.Equals(".xls"))
				{
					if (this.bForceACE)
					{
						return string.Format(this.excelObject, new object[] { "Ace", "12.0", this.filepath, "8.0" });
					}
					return string.Format(this.excelObject, new object[] { "Jet", "4.0", this.filepath, "8.0" });
				}
				else
				{
					if (fileInfo.Extension.Equals(".xlsx"))
					{
						return string.Format(this.excelObject, new object[] { "Ace", "12.0", this.filepath, "12.0" });
					}
					return string.Format(this.excelObject, new object[] { "Jet", "4.0", this.filepath, "8.0" });
				}
			}
		}

		// Token: 0x040018D8 RID: 6360
		private bool bForceACE;

		// Token: 0x040018D9 RID: 6361
		public static bool bOnly300ExcelData;

		// Token: 0x040018DA RID: 6362
		private OleDbConnection con;

		// Token: 0x040018DB RID: 6363
		private DataSet ds;

		// Token: 0x040018DC RID: 6364
		private string excelObject;

		// Token: 0x040018DD RID: 6365
		private string filepath;

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x06000A58 RID: 2648
		public delegate void ProgressWork(float percentage);
	}
}
