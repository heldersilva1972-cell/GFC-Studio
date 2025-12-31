using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C6 RID: 454
	public class DatatableToXml
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x000DF1CC File Offset: 0x000DE1CC
		public static string CDataToXml(DataSet ds)
		{
			return DatatableToXml.CDataToXml(ds, -1);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000DF1D8 File Offset: 0x000DE1D8
		public static string CDataToXml(DataTable dt)
		{
			if (dt != null)
			{
				try
				{
					using (DatatableToXml.ms = new MemoryStream())
					{
						using (XmlTextWriter xmlTextWriter = new XmlTextWriter(DatatableToXml.ms, Encoding.Unicode))
						{
							dt.WriteXml(xmlTextWriter);
							int num = (int)DatatableToXml.ms.Length;
							byte[] array = new byte[num];
							DatatableToXml.ms.Seek(0L, SeekOrigin.Begin);
							DatatableToXml.ms.Read(array, 0, num);
							UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
							return unicodeEncoding.GetString(array).Trim();
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
			return "";
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000DF2A0 File Offset: 0x000DE2A0
		public static string CDataToXml(DataView dv)
		{
			DataTable dataTable = dv.Table.Clone();
			for (int i = 0; i < dv.Count; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					dataRow[j] = dv[i][j];
				}
				dataTable.Rows.Add(dataRow);
			}
			dataTable.AcceptChanges();
			string text = DatatableToXml.CDataToXml(dataTable);
			dataTable.Dispose();
			return text;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000DF31D File Offset: 0x000DE31D
		public static string CDataToXml(DataSet ds, int tableIndex)
		{
			if (tableIndex != -1)
			{
				return DatatableToXml.CDataToXml(ds.Tables[tableIndex]);
			}
			return DatatableToXml.CDataToXml(ds.Tables[0]);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000DF346 File Offset: 0x000DE346
		public static bool CDataToXmlFile(DataSet ds, string xmlFilePath)
		{
			return DatatableToXml.CDataToXmlFile(ds, -1, xmlFilePath);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000DF350 File Offset: 0x000DE350
		public static bool CDataToXmlFile(DataTable dt, string xmlFilePath)
		{
			if (dt != null && !string.IsNullOrEmpty(xmlFilePath))
			{
				try
				{
					using (DatatableToXml.ms = new MemoryStream())
					{
						using (XmlTextWriter xmlTextWriter = new XmlTextWriter(DatatableToXml.ms, Encoding.Unicode))
						{
							dt.WriteXml(xmlTextWriter);
							int num = (int)DatatableToXml.ms.Length;
							byte[] array = new byte[num];
							DatatableToXml.ms.Seek(0L, SeekOrigin.Begin);
							DatatableToXml.ms.Read(array, 0, num);
							UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
							using (StreamWriter streamWriter = new StreamWriter(xmlFilePath))
							{
								streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
								streamWriter.WriteLine(unicodeEncoding.GetString(array).Trim());
								return true;
							}
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
				return false;
			}
			return false;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000DF454 File Offset: 0x000DE454
		public static bool CDataToXmlFile(DataView dv, string xmlFilePath)
		{
			DataTable dataTable = dv.Table.Clone();
			for (int i = 0; i < dv.Count; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					dataRow[j] = dv[i][j];
				}
				dataTable.Rows.Add(dataRow);
			}
			dataTable.AcceptChanges();
			bool flag = DatatableToXml.CDataToXmlFile(dataTable, xmlFilePath);
			dataTable.Dispose();
			return flag;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x000DF4D2 File Offset: 0x000DE4D2
		public static bool CDataToXmlFile(DataSet ds, int tableIndex, string xmlFilePath)
		{
			if (tableIndex != -1)
			{
				return DatatableToXml.CDataToXmlFile(ds.Tables[tableIndex], xmlFilePath);
			}
			return DatatableToXml.CDataToXmlFile(ds.Tables[0], xmlFilePath);
		}

		// Token: 0x04001870 RID: 6256
		private static MemoryStream ms;
	}
}
