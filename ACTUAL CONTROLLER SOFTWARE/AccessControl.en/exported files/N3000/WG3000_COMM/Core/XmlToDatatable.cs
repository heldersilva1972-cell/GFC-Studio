using System;
using System.Data;
using System.IO;
using System.Xml;

namespace WG3000_COMM.Core
{
	// Token: 0x0200021B RID: 539
	public class XmlToDatatable
	{
		// Token: 0x06000F42 RID: 3906 RVA: 0x0010F0C4 File Offset: 0x0010E0C4
		public static DataSet CXmlFileToDataSet(string xmlFilePath)
		{
			if (!string.IsNullOrEmpty(xmlFilePath))
			{
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(xmlFilePath);
					using (DataSet dataSet = new DataSet())
					{
						using (XmlToDatatable.StrStream = new StringReader(xmlDocument.InnerXml))
						{
							using (XmlTextReader xmlTextReader = new XmlTextReader(XmlToDatatable.StrStream))
							{
								dataSet.ReadXml(xmlTextReader);
								return dataSet;
							}
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0010F174 File Offset: 0x0010E174
		public static DataSet CXmlToDataSet(string xmlStr)
		{
			if (!string.IsNullOrEmpty(xmlStr))
			{
				try
				{
					if (xmlStr.Substring(0, 2) == "?<")
					{
						xmlStr = xmlStr.Substring(1);
					}
					using (DataSet dataSet = new DataSet())
					{
						using (XmlToDatatable.StrStream = new StringReader(xmlStr))
						{
							using (XmlTextReader xmlTextReader = new XmlTextReader(XmlToDatatable.StrStream))
							{
								dataSet.ReadXml(xmlTextReader);
								return dataSet;
							}
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0010F228 File Offset: 0x0010E228
		public static DataTable CXmlToDataTable(string xmlFilePath)
		{
			return XmlToDatatable.CXmlFileToDataSet(xmlFilePath).Tables[0];
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0010F23B File Offset: 0x0010E23B
		public static DataTable CXmlToDataTable(string xmlFilePath, int tableIndex)
		{
			return XmlToDatatable.CXmlFileToDataSet(xmlFilePath).Tables[tableIndex];
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0010F24E File Offset: 0x0010E24E
		public static DataTable CXmlToDatatTable(string xmlStr)
		{
			return XmlToDatatable.CXmlToDataSet(xmlStr).Tables[0];
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0010F261 File Offset: 0x0010E261
		public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
		{
			return XmlToDatatable.CXmlToDataSet(xmlStr).Tables[tableIndex];
		}

		// Token: 0x04001C53 RID: 7251
		private static StringReader StrStream;
	}
}
