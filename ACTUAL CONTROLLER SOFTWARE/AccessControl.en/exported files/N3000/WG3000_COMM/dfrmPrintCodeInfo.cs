using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM
{
	// Token: 0x0200022C RID: 556
	public partial class dfrmPrintCodeInfo : frmN3000
	{
		// Token: 0x06001094 RID: 4244 RVA: 0x0012C7C4 File Offset: 0x0012B7C4
		public dfrmPrintCodeInfo()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0012CE7B File Offset: 0x0012BE7B
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0012CE8C File Offset: 0x0012BE8C
		private void btnOK_Click(object sender, EventArgs e)
		{
			DataTable dataTable = this.dataGridView1.DataSource as DataTable;
			if (dataTable.Rows.Count > 0)
			{
				dataTable.AcceptChanges();
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					if (string.IsNullOrEmpty(dataTable.Rows[i][this.PRODUCT_CODE].ToString()))
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查条码", "不能空"));
						return;
					}
					if (dataTable.Rows[i][this.PRODUCT_CODE].ToString().Length != "KT0L01*******".Length)
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查条码", dataTable.Rows[i][this.PRODUCT_CODE].ToString()));
						return;
					}
					if (dataTable.Rows[i][this.PRODUCT_CODE].ToString().IndexOf("*******") < 0)
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查条码", dataTable.Rows[i][this.PRODUCT_CODE].ToString()));
						return;
					}
					if (string.IsNullOrEmpty(dataTable.Rows[i][this.PRODUCT_SN].ToString()))
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查 序列号", "不能空"));
						return;
					}
					if (dataTable.Rows[i][this.PRODUCT_SN].ToString().Length != "123******".Length)
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查 序列号", dataTable.Rows[i][this.PRODUCT_SN].ToString()));
						return;
					}
					if (dataTable.Rows[i][this.PRODUCT_SN].ToString().IndexOf("******") < 0)
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}\r\n{2}", i + 1, "核查 序列号", dataTable.Rows[i][this.PRODUCT_SN].ToString()));
						return;
					}
					if (string.IsNullOrEmpty(dataTable.Rows[i][this.PRODUCT_MODEL].ToString()))
					{
						XMessageBox.Show(string.Format("有问题: 第{0} 行: {1}", i + 1, "要选择模块芯片"));
						return;
					}
				}
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					if (string.Compare(dataTable.Rows[j][this.PRODUCT_MODEL] as string, "6911") == 0)
					{
						dataTable.Rows[j][this.PRODUCT_MAC] = "00-69-********";
					}
					else
					{
						dataTable.Rows[j][this.PRODUCT_MAC] = "00-66-********";
					}
				}
				dataTable.AcceptChanges();
				string text = "";
				for (int k = 0; k < dataTable.Rows.Count; k++)
				{
					for (int l = 0; l < dataTable.Columns.Count; l++)
					{
						if (string.IsNullOrEmpty(text))
						{
							text = dataTable.Rows[k][l] as string;
						}
						else
						{
							text = text + "," + dataTable.Rows[k][l];
						}
					}
				}
				wgAppConfig.UpdateKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO", text);
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0012D254 File Offset: 0x0012C254
		private void dfrmPrintCodeInfo_Load(object sender, EventArgs e)
		{
			DataTable dataTable = new DataTable("Print Code");
			dataTable.Columns.Add("产品类别");
			dataTable.Columns.Add("型号");
			dataTable.Columns.Add("条码");
			dataTable.Columns.Add("序列号");
			dataTable.Columns.Add("模块芯片");
			dataTable.Columns.Add("MAC地址");
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = dataTable;
			int num = 0;
			while (num < dataTable.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = dataTable.Columns[num].ColumnName;
				num++;
			}
			if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO")))
			{
				this.productTypeInfo = wgAppConfig.GetKeyVal("PRODUCT_SPECIAL_PRINTCODEINFO").Split(new char[] { ',' });
			}
			int num2 = 0;
			while (num2 < this.productTypeInfo.Length && !string.IsNullOrEmpty(this.productTypeInfo[num2 + this.PRODUCT_SN]))
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[this.PRODUCT_BRAND] = this.productTypeInfo[num2 + this.PRODUCT_BRAND];
				dataRow[this.PRODUCT_TYPE] = this.productTypeInfo[num2 + this.PRODUCT_TYPE];
				dataRow[this.PRODUCT_SN] = this.productTypeInfo[num2 + this.PRODUCT_SN];
				dataRow[this.PRODUCT_MAC] = this.productTypeInfo[num2 + this.PRODUCT_MAC];
				dataRow[this.PRODUCT_MODEL] = this.productTypeInfo[num2 + this.PRODUCT_MODEL];
				dataRow[this.PRODUCT_CODE] = this.productTypeInfo[num2 + this.PRODUCT_CODE];
				dataTable.Rows.Add(dataRow);
				num2 += this.INFO_UNIT_LENGTH;
			}
			dataTable.AcceptChanges();
		}

		// Token: 0x04001D6F RID: 7535
		private int PRODUCT_BRAND;

		// Token: 0x04001D70 RID: 7536
		private int INFO_UNIT_LENGTH = 6;

		// Token: 0x04001D71 RID: 7537
		private int PRODUCT_CODE = 2;

		// Token: 0x04001D72 RID: 7538
		private int PRODUCT_MAC = 5;

		// Token: 0x04001D73 RID: 7539
		private int PRODUCT_MODEL = 4;

		// Token: 0x04001D74 RID: 7540
		private int PRODUCT_SN = 3;

		// Token: 0x04001D75 RID: 7541
		private int PRODUCT_TYPE = 1;

		// Token: 0x04001D76 RID: 7542
		private string[] productTypeInfo = new string[]
		{
			"中性蓝板", "L01", "KT0L01*******", "122******", "6911", "00-69-********", "中性蓝板", "L01", "KT0L01*******", "123******",
			"1766", "00-66-********", "中性蓝板", "L02", "KT0L02*******", "222******", "6911", "00-69-********", "中性蓝板", "L02",
			"KT0L02*******", "223******", "1766", "00-66-********", "中性蓝板", "L04", "KT0L04*******", "422******", "6911", "00-69-********",
			"中性蓝板", "L04", "KT0L04*******", "423******", "1766", "00-66-********", "Adroitor金板", "AT8001", "KT8001*******", "131******",
			"6911", "00-69-********", "Adroitor金板", "AT8001", "KT8001*******", "133******", "1766", "00-66-********", "Adroitor金板", "AT8002",
			"KT8002*******", "231******", "6911", "00-69-********", "Adroitor金板", "AT8002", "KT8002*******", "233******", "1766", "00-66-********",
			"Adroitor金板", "AT8004", "KT8004*******", "431******", "6911", "00-69-********", "Adroitor金板", "AT8004", "KT8004*******", "433******",
			"1766", "00-66-********", "品牌绿板", "WG2001.NET-12", "KW2101*******", "151******", "6911", "00-69-********", "品牌绿板", "WG2001.NET-12",
			"KW2101*******", "153******", "1766", "00-66-********", "品牌绿板", "WG2002.NET-12", "KW2102*******", "251******", "6911", "00-69-********",
			"品牌绿板", "WG2002.NET-12", "KW2102*******", "253******", "1766", "00-66-********", "品牌绿板", "WG2004.NET-12", "KW2104*******", "451******",
			"6911", "00-69-********", "品牌绿板", "WG2004.NET-12", "KW2104*******", "453******", "1766", "00-66-********", "ADCT黑板", "ADCT3000-1",
			"KA3001*******", "111******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-1", "KA3001*******", "113******", "1766", "00-66-********",
			"ADCT黑板", "ADCT3000-2", "KA3002*******", "211******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-2", "KA3002*******", "213******",
			"1766", "00-66-********", "ADCT黑板", "ADCT3000-4", "KA3004*******", "411******", "6911", "00-69-********", "ADCT黑板", "ADCT3000-4",
			"KA3004*******", "413******", "1766", "00-66-********", "梯控绿板", "DT20M", "KTDT20*******", "171******", "6911", "00-69-********",
			"梯控绿板", "DT20M", "KTDT20*******", "173******", "1766", "00-66-********", "", "", "", "",
			"", "", "", "", "", "", "", ""
		};
	}
}
