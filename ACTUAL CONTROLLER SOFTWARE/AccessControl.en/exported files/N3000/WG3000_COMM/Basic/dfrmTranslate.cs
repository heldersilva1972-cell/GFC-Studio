using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000031 RID: 49
	public partial class dfrmTranslate : frmN3000
	{
		// Token: 0x06000367 RID: 871 RVA: 0x000639A1 File Offset: 0x000629A1
		public dfrmTranslate()
		{
			this.InitializeComponent();
			wgAppConfig.custDataGridview(ref this.dataGridView1);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000639BA File Offset: 0x000629BA
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000639CC File Offset: 0x000629CC
		private void btnLoad_Click(object sender, EventArgs e)
		{
			this.fillSystemParam();
			try
			{
				this.openFileDialog1.Filter = " (*.xml)|*.xml| (*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				this.openFileDialog1.Title = (sender as Button).Text;
				this.openFileDialog1.FileName = "";
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					string fileName = this.openFileDialog1.FileName;
					string text = fileName;
					if (File.Exists(text))
					{
						this.tb = new DataTable();
						this.tb.TableName = "WEBString";
						this.tb.Columns.Add("f_NO");
						this.tb.Columns.Add("f_Name");
						this.tb.Columns.Add("f_Value");
						this.tb.Columns.Add("f_CName");
						this.tb.ReadXml(text);
						this.tb.AcceptChanges();
						this.dv = new DataView(this.tb);
						this.dv.RowFilter = "f_NO > 0";
						this.dvDefault = (DataView)this.dataGridView1.DataSource;
						for (int i = 0; i < this.dv.Count; i++)
						{
							this.dvDefault.RowFilter = " f_NO = " + this.dv[i]["f_NO"];
							if (this.dvDefault.Count > 0)
							{
								this.dvDefault[0]["f_Value"] = this.dv[i]["f_Value"];
							}
						}
						this.dvDefault.RowFilter = "f_NO > 0";
						this.dataGridView1.Refresh();
						XMessageBox.Show((sender as Button).Text + " " + CommonStr.strSuccessfully);
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00063C08 File Offset: 0x00062C08
		private void btnOK_Click(object sender, EventArgs e)
		{
			string text = wgAppConfig.Path4Doc() + "OtherLang_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
			using (StringWriter stringWriter = new StringWriter())
			{
				(this.dataGridView1.DataSource as DataView).Table.WriteXml(stringWriter, XmlWriteMode.WriteSchema, true);
				using (StreamWriter streamWriter = new StreamWriter(text, false))
				{
					streamWriter.Write(stringWriter.ToString());
				}
			}
			XMessageBox.Show((sender as Button).Text + "\r\n\r\n" + text);
			base.Close();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00063CCC File Offset: 0x00062CCC
		private void dfrmSystemParam_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
				{
					this.dataGridView1.Columns[i].Visible = true;
				}
				(this.dataGridView1.DataSource as DataView).RowFilter = "";
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00063D57 File Offset: 0x00062D57
		private void dfrmSystemParam_Load(object sender, EventArgs e)
		{
			this.fillSystemParam();
			if (!wgAppConfig.IsChineseSet(wgAppConfig.CultureInfoStr))
			{
				this.dataGridView1.Columns[2].Visible = false;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00063D84 File Offset: 0x00062D84
		private void fillSystemParam()
		{
			if (this.dtWEBString != null)
			{
				this.dtWEBString.Rows.Clear();
				this.dtWEBString.Dispose();
				GC.Collect();
			}
			this.dtWEBString = new DataTable("WEBString");
			this.dtWEBString.Columns.Add("f_NO", Type.GetType("System.UInt32"));
			this.dtWEBString.Columns.Add("f_Name", Type.GetType("System.String"));
			this.dtWEBString.Columns.Add("f_CName", Type.GetType("System.String"));
			this.dtWEBString.Columns.Add("f_Value", Type.GetType("System.String"));
			string[] array = new string[]
			{
				"UTF-8", "Web Controller", "AddCard", "Users", "Swipe", "Configure", "Exit", "Please Confirm again", "Confirm Auto AddCard By Swiping", "Cancel",
				"Now you can add card by swiping. Please swipe...", "If finished, Then click Exit Button.", "Exit", "Auto add", "cards", "Card NO", "Name", "Add", "Auto Add by Swiping", "[User]->[Edit]",
				"User ID", "Operation", "Save", "[User]->[Delete]", "OK", "Edit", "Delete", "Found Users' Count", "Countinue next", "Search Finished.",
				"Keyword", "Search", "First", "Prev", "Next", "Last", "Total Users", "Keyword (Card NO)", "Go To Record ID", "Query",
				"Record ID", "Status", "DateTime", "To Search", "Found Records' Count", "Searched", "Already Searched ID", "Refresh", "Page", "Of",
				"Page", "[Configure]->[Edit]", "Item", "Value", "Open Door Delay(second)", "MAC Address", " Obtain an IP address automatically", " Use the following IP address", "Subnet mask", "Gateway",
				"Old Name", "Old Password", "New Name", "New Password", "Confirmed New Password", "ID", "Open-Door Password", "Please Confirm! Then close the brower and login again after one minute.", "Reboot", "Year",
				"Month", "Day", "Hour", "Minute", "Second", "Record PushButton And DoorStatus Events", "Enabled", "Disabled", "Device Status", "Reboot",
				"Device NO", "Driver Version", "Realtime Clock", "Adjust Time", "Door Control Parameter", "Remote Open", "Remote Open", "Door", "Edit", "Total",
				"Edit", "Users Total", "Door Status", "Open", "Close", "Enabled", "Disabled", "Edit", "Network Parameters", "Edit",
				"System Manager", "Edit", "Format successfully. Please Reboot!!!", "Password For Format", "Format", "UserName", "Password", "Login", "Fail to Login.", "CardNO invalid!",
				"CardNO: ", "  already used!", "name's length too long!", " Add Successfully", ", maybe no space for user. fail to add!", "ID: ", "  user is deleted", "name's length too long!", "  user is edited successfully", "Door Delay invalid",
				"Successfully", " Fail to edit", " Successfully", " Fail", "IP Address invalid!", "Gateway invalid!", "Subnet mask invlaid!", "Successfully. Please Reboot the device", "Successfully.", "Fail",
				"Adjust Time Successfully", "Adjust Time Failed!", "Remote Open Door", "DOOR", "Push Button", "Door Open", "Door Closed", "Super Password Open Door", "Fire", "Forced Close",
				"Other Events", "Allow", "Forbid", "OUT", "IN", "Super Card", "Super Card[AutoAdd Card]", "Super Card NO invalid!", "Date", "Not Found Records!",
				"Device Name", "Successfully", "Failed", "HTTP PORT invalid!", "Name", "Successfully", "Failed", "Home", "successfully", "Language",
				"English", "Chinese", "Other", "Go To AutoAdd Card", "Exit From AutoAdding Card", "1: Manual Input", "2: AutoAddBySwiping", "Privilege", "Mobile Open Door", "Mobile IP Configure",
				"Allow All Doors/Floors", "Allow Selected Doors/Floors", "More Doors/Floors (eg: 10.11.18.40)", "Option", ""
			};
			string[] array2 = new string[]
			{
				"UTF-8", "门禁专家", "加&nbsp;卡", "用&nbsp;户", "记&nbsp;录", "配&nbsp;置", "退&nbsp;出", "请再次确定", "确定要自动添加", "取消",
				"已进入自动添加卡状态. 请刷卡自动添加.", "完成后, 按退出按钮.", "退出自动添加", "自动添加了", "张卡", "卡号", "姓名", "添加", "进入", "[用户]->[修改]",
				"用户编号", "操作", "保存", "[用户]->[删除]", "确定删除", "修改", "删除", "此次查到用户数", "还可继续查", "已查完",
				"搜索卡号或姓名", "搜索", "最前页", "上一页", "下一页", "最后页", "总人数", "搜索卡号或姓名", "跳转到指定序号", "查询",
				"记录序号", "状态", "时间", "搜索", "此次查到记录", "已查记录", "已查记录序号", "刷新最新记录", "第", "页/共",
				"页", "[配置]->[修改]", "名称", "值", "开门延时(秒)", "MAC地址", "自动获取IP地址", "使用下面的IP地址", "掩码", "网关",
				"原登录名", "原密码", "新登录名", "新密码", "确认新密码", "序号", "开门密码", "请确定! 确定后, 关闭浏览器, 1分钟后再登录.", "确定要重启设备", "年",
				"月", "日", "时", "分", "秒", "记录按钮开门事件", "启用", "不启用", "设备状态", "重启设备",
				"设备号", "当前驱动版本", "设备时钟", "校准时间", "门禁参数", "远程开门", "远程开", "号门", "修改开门延时", "总共个数",
				"修改开门密码", "用户数", "门状态", "开", "关", "已启用", "未启用", "设置", "网络参数", "修改网络设置",
				"系统管理员", "修改登录名和密码", "格式化成功, 请重启设备!!!", "格式化密码", "格式化", "登录名", "密&nbsp;&nbsp;码", "登录", "登录失败. 请检查登录名和密码.", "输入的卡号无效. 添加不成功!",
				"卡号: ", "  已在用. 添加不成功!", "输入的用户名字过长. 添加不成功!", "  添加成功", ", 可能用户已满, 添加失败", "用户编号: ", "  用户删除成功", "输入的用户名字过长.(最多10个汉字). 修改不成功!", "  用户修改成功", "输入的延时无效. 修改不成功!",
				"门延时 修改成功!", " 修改失败. 密码必须为数字, 不能为0, 不能有字母.", " 修改成功!", " 选择自动获取IP方式不正确. 修改不成功!", "输入的IP地址无效. 修改不成功!", "输入的网关地址无效. 修改不成功!", "输入的掩码地址无效. 修改不成功!", "网络设置修改成功! 请重启设备让设置立即生效! 点击\"确定要重启设备\"后, 关闭浏览器, 1分钟后再登录.", "登录名和密码修改成功!", "输入值无效.(最多5个汉字) 登录名和密码修改不成功!",
				"校准时间成功!", "校准时间不成功!", "远程开门", "号门", "按钮开门", "门开[门磁信号]", "门关[门磁信号]", "超级密码开门", "火警", "强制锁门",
				"其他事件", "允许", "禁止", "出门", "进门", "母卡", "母卡[专用于发卡]", "输入的卡号无效. 设置不成功!", "指定日期", "没有找到记录!",
				"设备名", "设备名修改成功", "输入的设备名过长.(最多10个汉字). 修改不成功!", "输入的HTTP PORT无效. 修改不成功!", "名称", "门名称修改成功", "输入的门名称过长.(最多10个汉字). 修改不成功!", "首&nbsp;页", "成功", "语言",
				"英文", "中文", "其他", "进入发卡状态", "退出发卡状态", "加卡方式一: 手动输入", "加卡方式二: 刷卡自动添加", "开门权限", "手机开门", "允许手机更改网络参数",
				"允许开所有门/楼层", "允许开指定门/楼层", "更多请直接输入(用点隔开, 如: 10.11.18.40)", "可选", ""
			};
			for (int i = 0; i < array.Length - 1; i++)
			{
				DataRow dataRow = this.dtWEBString.NewRow();
				dataRow["f_NO"] = i.ToString();
				dataRow["f_Name"] = array[i];
				dataRow["f_Value"] = "";
				dataRow["f_Value"] = dataRow["f_Name"];
				dataRow["f_CName"] = array2[i];
				this.dtWEBString.Rows.Add(dataRow);
			}
			this.dtWEBString.AcceptChanges();
			this.dv = new DataView(this.dtWEBString);
			this.dv.RowFilter = "f_NO > 0";
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.DataSource = this.dv;
			int num = 0;
			while (num < this.dv.Table.Columns.Count && num < this.dataGridView1.ColumnCount)
			{
				this.dataGridView1.Columns[num].DataPropertyName = this.dv.Table.Columns[num].ColumnName;
				num++;
			}
			this.dataGridView1.DefaultCellStyle.ForeColor = SystemColors.WindowText;
		}

		// Token: 0x0400069B RID: 1691
		private DataTable dtWEBString;

		// Token: 0x0400069C RID: 1692
		private DataView dv;

		// Token: 0x0400069D RID: 1693
		private DataView dvDefault;

		// Token: 0x0400069E RID: 1694
		private DataTable tb;
	}
}
