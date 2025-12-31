using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C5 RID: 453
	public class DatatableToJson
	{
		// Token: 0x0600099B RID: 2459 RVA: 0x000DEC64 File Offset: 0x000DDC64
		public static bool CDataToXmlFile(DataView dv, string xmlFilePath)
		{
			string text = DatatableToJson.DataTableToJson(dv);
			bool flag;
			using (StreamWriter streamWriter = new StreamWriter(xmlFilePath))
			{
				streamWriter.WriteLine(text);
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x000DECA8 File Offset: 0x000DDCA8
		public static string DataTableToJson(DataTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (table.Rows.Count > 0)
			{
				stringBuilder.Append("[");
				for (int i = 0; i < table.Rows.Count; i++)
				{
					stringBuilder.Append("{");
					for (int j = 0; j < table.Columns.Count; j++)
					{
						if (j < table.Columns.Count - 1)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"\"",
								table.Columns[j].ColumnName.ToString(),
								"\":\"",
								table.Rows[i][j].ToString(),
								"\","
							}));
						}
						else if (j == table.Columns.Count - 1)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"\"",
								table.Columns[j].ColumnName.ToString(),
								"\":\"",
								table.Rows[i][j].ToString(),
								"\""
							}));
						}
					}
					if (i == table.Rows.Count - 1)
					{
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("},");
					}
				}
				stringBuilder.Append("]");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000DEE40 File Offset: 0x000DDE40
		public static string DataTableToJson(DataView dv)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DataTable table = dv.Table;
			if (dv.Count > 0)
			{
				stringBuilder.Append("[");
				for (int i = 0; i < dv.Count; i++)
				{
					stringBuilder.Append("{");
					for (int j = 0; j < table.Columns.Count; j++)
					{
						if (j < table.Columns.Count - 1)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"\"",
								table.Columns[j].ColumnName.ToString(),
								"\":\"",
								dv[i][j].ToString(),
								"\","
							}));
						}
						else if (j == table.Columns.Count - 1)
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"\"",
								table.Columns[j].ColumnName.ToString(),
								"\":\"",
								dv[i][j].ToString(),
								"\""
							}));
						}
					}
					if (i == dv.Count - 1)
					{
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("},");
					}
				}
				stringBuilder.Append("]");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000DEFCC File Offset: 0x000DDFCC
		public static DataTable JsonToDataTable(string strJson)
		{
			strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
			Regex regex = new Regex("(?<={)[^:]+(?=:\\[)", RegexOptions.IgnoreCase);
			string value = regex.Match(strJson).Value;
			DataTable dataTable = null;
			strJson = strJson.Substring(strJson.IndexOf("[") + 1);
			strJson = strJson.Substring(0, strJson.IndexOf("]"));
			MatchCollection matchCollection = new Regex("(?<={)[^}]+(?=})").Matches(strJson);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string[] array = matchCollection[i].Value.Split(new char[] { '*' });
				if (dataTable == null)
				{
					dataTable = new DataTable();
					dataTable.TableName = value;
					foreach (string text in array)
					{
						DataColumn dataColumn = new DataColumn();
						string[] array3 = text.Split(new char[] { '#' });
						if (array3[0].Substring(0, 1) == "\"")
						{
							int length = array3[0].Length;
							dataColumn.ColumnName = array3[0].Substring(1, length - 2);
						}
						else
						{
							dataColumn.ColumnName = array3[0];
						}
						dataTable.Columns.Add(dataColumn);
					}
					dataTable.AcceptChanges();
				}
				DataRow dataRow = dataTable.NewRow();
				for (int k = 0; k < array.Length; k++)
				{
					dataRow[k] = array[k].Split(new char[] { '#' })[1].Trim().Replace("，", ",").Replace("：", ":")
						.Replace("\"", "");
				}
				dataTable.Rows.Add(dataRow);
				dataTable.AcceptChanges();
			}
			return dataTable;
		}
	}
}
