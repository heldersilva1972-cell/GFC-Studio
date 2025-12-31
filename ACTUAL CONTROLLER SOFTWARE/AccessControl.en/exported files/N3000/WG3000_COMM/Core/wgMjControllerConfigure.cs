using System;
using System.Net;
using System.Text;

namespace WG3000_COMM.Core
{
	// Token: 0x020001F5 RID: 501
	public class wgMjControllerConfigure
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x00102458 File Offset: 0x00101458
		public wgMjControllerConfigure()
		{
			this.m_paramDesc = new string[]
			{
				"起始标识0D", "起始标识7E", "1号门开门延时默认30为3秒 低位", "高位", "2号门开门延时默认30为3秒 低位", "高位", "3号门开门延时默认30为3秒 低位", "高位", "4号门开门延时默认30为3秒 低位", "高位",
				"1号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "2号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "3号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "4号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "1号门互锁", "2号门互锁", "3号门互锁", "4号门互锁", "--停用", "--停用",
				"--停用", "--停用", "1号读卡器启用密码键盘", "2号读卡器启用密码键盘", "3号读卡器启用密码键盘", "4号读卡器启用密码键盘", "按钮缺省状态为FF[为高平]", "停用 门磁缺省状态0x00", "电锁正常状态值0xff", "记录按钮事件",
				"记录报警事件", "启用反潜回", "启用报警", "启用顺序刷卡", "读卡器指示灯亮0.8秒", "门打开后, 在规定的延时时间之后, (250)延长25秒, 超过了25秒则启用报警", "反潜时先进门后出门 (=0xa5时,可以先进, 也可先出)", "两张连续读卡的最长间隔100=10秒", "启用定时操作列表", "定时: 8个口触发动作4个门,4个扩展口",
				"01010101 单个读卡器, 进门读卡器启用 多卡同时开门", "报警自动复位 默认0x01", "报警的最小输出时间3秒(30)", "此通信时间内由电脑控制刷卡的开门(秒)", "假期约束, 默认00不启用", "防盗报警控制基本参数 ALARM_CONTROL_MODE_DEFAULT", "设防延时 30秒(30)", "撤防延时 30秒(30)", "门内最多人数 低位", "门内最多人数 高位",
				"门内最少人数 低位", "门内最少人数 高位", "有效刷卡间隔 低位(单位:*2秒)", "刷卡有效间隔 高位", "心跳最大周期 低位(单位: 分钟)", "心跳最大周期 高位", "通信—接收火警信号", "通信—发送火警信号", "通信—接收互锁信号", "通信—发送互锁信号",
				"记录设防状态 默认是撤防 0x00", "单层延时", "多层延时", "通信—发送反潜回卡号信号", "--停用", "--停用", "--停用", "--停用", "--停用", "--停用",
				"--停用", "--停用", "--停用", "--停用", "胁迫密码字节1", "胁迫密码字节2", "1号#1 17组开门密码(2个一组, 0xffff无效)", "1号读卡器组1 开门密码", "1号读卡器组2 开门密码", "1号读卡器组2 开门密码",
				"1号读卡器组3 开门密码", "1号读卡器组3 开门密码", "1号读卡器组4 开门密码", "1号读卡器组4 开门密码", "2号读卡器组1 开门密码", "2号读卡器组1 开门密码", "2号读卡器组2 开门密码", "2号读卡器组2 开门密码", "2号读卡器组3 开门密码", "2号读卡器组3 开门密码",
				"2号读卡器组4 开门密码", "2号读卡器组4 开门密码", "3号读卡器组1 开门密码", "3号读卡器组1 开门密码", "3号读卡器组2 开门密码", "3号读卡器组2 开门密码", "3号读卡器组3 开门密码", "3号读卡器组3 开门密码", "3号读卡器组4 开门密码", "3号读卡器组4 开门密码",
				"4号读卡器组1 开门密码", "4号读卡器组1 开门密码", "4号读卡器组2 开门密码", "4号读卡器组2 开门密码", "4号读卡器组3 开门密码", "4号读卡器组3 开门密码", "4号读卡器组4 开门密码", "4号读卡器组4 开门密码", "开门密码", "开门密码",
				"--停用", "--停用", "--停用", "--停用", "--停用", "--停用", "IP地址", "IP地址", "IP地址", "IP地址",
				"掩码", "掩码", "掩码", "掩码", "网关", "网关", "网关", "网关", "端口低位 默认60000=0xEA60", "端口高位",
				"启用DHCP(=0xa5时表示启用) 2010-6-18", "启用 禁止10M尝试(=0xa5时表示启用) 2014-03-17 [V5.52 =0xa6强制为10M半双工]", "停用 通信密码字节3", "停用 通信密码字节4", "停用 通信密码字节5", "停用 通信密码字节6", "扩展板1的参数表 默认四个继电器一一对应", "", "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
				"动作时间缺省是10秒", "动作时间 高位", "扩展板2的参数表 默认四个继电器一一对应", "", "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "", "动作时间缺省是10秒", "动作时间 高位", "扩展板3的参数表 默认四个继电器一一对应", "",
				"默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "", "动作时间缺省是10秒", "动作时间 高位", "扩展板4的参数表 默认四个继电器一一对应", "", "", "防盗(刷卡,24小时,紧急", "动作时间缺省是300秒", "动作时间 高位",
				"读卡器性质", "通过密码键盘直接输入卡号,方式是:*卡号*", "门打开时间过长的高位[单位是25.6秒]", "胁迫密码字节3", "1号读卡器首卡开门", "2号读卡器首卡开门", "3号读卡器首卡开门", "4号读卡器首卡开门", "1号读卡器必须要到的人数", "2号读卡器必须要到的人数",
				"3号读卡器必须要到的人数", "4号读卡器必须要到的人数", "1号读卡器群1", "1号读卡器群2", "1号读卡器群3", "1号读卡器群4", "1号读卡器群5", "1号读卡器群6", "1号读卡器群7", "1号读卡器群8",
				"2号读卡器群1", "2号读卡器群2", "2号读卡器群3", "2号读卡器群4", "2号读卡器群5", "2号读卡器群6", "2号读卡器群7", "2号读卡器群8", "3号读卡器群1", "3号读卡器群2",
				"3号读卡器群3", "3号读卡器群4", "3号读卡器群5", "3号读卡器群6", "3号读卡器群7", "3号读卡器群8", "4号读卡器群1", "4号读卡器群2", "4号读卡器群3", "4号读卡器群4",
				"4号读卡器群5", "4号读卡器群6", "4号读卡器群7", "4号读卡器群8", "记录门磁事件[开门或关门的时间]", "无效刷卡时, 禁止读卡器LED灯光/BEEP声音输出 (驱动V5.48或以上支持) =0xa5启用", "禁止手机WEB管理自动设置控制器IP功能(驱动V5.45以上) =0xa5启用", "刷一次卡(驱动V5.48以上)", "目标主机IP1 用于发送新的记录 默认广播全F", "目标主机IP2",
				"目标主机IP3", "目标主机IP4", "PC机 SERVER 工作口低位(61001)", "PC机 SERVER 工作口高位(61001)", "UDP CLIENT 控制端工作口低位(61002)", "UDP CLIENT 控制端工作口高位(61002)", "控制器主动发送的周期 低位. 默认不发", "控制器主动发送的周期 高位. 默认不发", "1号读卡器多卡选项", "2号读卡器多卡选项",
				"3号读卡器多卡选项", "4号读卡器多卡选项", " 开关模式 LOCK_SWITCH_OPTION", "1号读卡器组1 开门密码第3字节", "1号读卡器组2 开门密码第3字节", "1号读卡器组3 开门密码第3字节", "1号读卡器组4 开门密码第3字节", "2号读卡器组1 开门密码第3字节", "2号读卡器组2 开门密码第3字节", "2号读卡器组3 开门密码第3字节",
				"2号读卡器组4 开门密码第3字节", "3号读卡器组1 开门密码第3字节", "3号读卡器组2 开门密码第3字节", "3号读卡器组3 开门密码第3字节", "3号读卡器组4 开门密码第3字节", "4号读卡器组1 开门密码第3字节", "4号读卡器组2 开门密码第3字节", "4号读卡器组3 开门密码第3字节", "4号读卡器组4 开门密码第3字节", "手机开门模拟刷卡输入(设备驱动V5.40以上) =0xa5启用; ",
				"1号门 开始禁用的时段(不含0,1)", "2号门 开始禁用的时段(不含0,1)", "3号门 开始禁用的时段(不含0,1)", "4号门 开始禁用的时段(不含0,1)", "(V5.52)刷卡自动延长截止日期(=0不启用, >0 表示延长天数)", "(V5.52)刷卡自动延长截止日期 高字节(=0不启用, >0 表示延长天数)", "(V5.52)按钮1失效(=0不启用, >0)", "(V5.52)按钮2失效(=0不启用, >0)", "(V5.52)按钮3失效(=0不启用, >0)", "(V5.52)按钮4失效(=0不启用, >0)",
				"(V5.52)合法卡连续刷4次切换常开或在线 高4字节时段约束(0所有用户). 低4字节对应4个读卡器"
			};
			this.m_needUpdateBits = new byte[128];
			this.m_param_Special = new byte[1024];
			byte[] array = new byte[]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				3, 3, 3, 3, 0, 0, 0, 0, 1, 2,
				3, 4, 0, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue, 0,
				0, 0, 0, 0, 8, 250, 0, 100, 0, byte.MaxValue,
				85, 1, 30, 0, 0, 126, 30, 30, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				0, 4, 50, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 192, 168, 0, 0,
				byte.MaxValue, byte.MaxValue, 0, 0, 192, 168, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 1, 0, 39, 0,
				0, 0, 2, 0, 39, 0, 0, 0, 4, 0,
				39, 0, 0, 0, 16, 0, 0, 56, 0, 0,
				10, 0, 0, 13, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0
			};
			array[0] = wgMjControllerConfigure.LowByte(32269);
			array[1] = wgMjControllerConfigure.HighByte(32269);
			array[2] = wgMjControllerConfigure.LowByte(30);
			array[3] = wgMjControllerConfigure.HighByte(30);
			array[4] = wgMjControllerConfigure.LowByte(30);
			array[5] = wgMjControllerConfigure.HighByte(30);
			array[6] = wgMjControllerConfigure.LowByte(30);
			array[7] = wgMjControllerConfigure.HighByte(30);
			array[8] = wgMjControllerConfigure.LowByte(30);
			array[9] = wgMjControllerConfigure.HighByte(30);
			array[74] = wgMjControllerConfigure.LowByte(889988);
			array[75] = wgMjControllerConfigure.HighByte(889988);
			array[128] = wgMjControllerConfigure.LowByte(60000);
			array[129] = wgMjControllerConfigure.HighByte(60000);
			array[140] = wgMjControllerConfigure.LowByte(30);
			array[141] = wgMjControllerConfigure.HighByte(30);
			array[146] = wgMjControllerConfigure.LowByte(30);
			array[147] = wgMjControllerConfigure.HighByte(30);
			array[152] = wgMjControllerConfigure.LowByte(30);
			array[153] = wgMjControllerConfigure.HighByte(30);
			array[158] = wgMjControllerConfigure.LowByte(3000);
			array[159] = wgMjControllerConfigure.HighByte(3000);
			array[212] = wgMjControllerConfigure.LowByte(61001);
			array[213] = wgMjControllerConfigure.HighByte(61001);
			array[214] = wgMjControllerConfigure.LowByte(61002);
			array[215] = wgMjControllerConfigure.HighByte(61002);
			array[216] = wgMjControllerConfigure.LowByte(0);
			array[217] = wgMjControllerConfigure.HighByte(0);
			this.m_param = array;
			this.Clear();
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00103268 File Offset: 0x00102268
		public wgMjControllerConfigure(byte[] pkt, int startIndex)
		{
			this.m_paramDesc = new string[]
			{
				"起始标识0D", "起始标识7E", "1号门开门延时默认30为3秒 低位", "高位", "2号门开门延时默认30为3秒 低位", "高位", "3号门开门延时默认30为3秒 低位", "高位", "4号门开门延时默认30为3秒 低位", "高位",
				"1号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "2号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "3号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "4号门控制 默认3在线, 2=常闭, 1=常开 0=NONE", "1号门互锁", "2号门互锁", "3号门互锁", "4号门互锁", "--停用", "--停用",
				"--停用", "--停用", "1号读卡器启用密码键盘", "2号读卡器启用密码键盘", "3号读卡器启用密码键盘", "4号读卡器启用密码键盘", "按钮缺省状态为FF[为高平]", "停用 门磁缺省状态0x00", "电锁正常状态值0xff", "记录按钮事件",
				"记录报警事件", "启用反潜回", "启用报警", "启用顺序刷卡", "读卡器指示灯亮0.8秒", "门打开后, 在规定的延时时间之后, (250)延长25秒, 超过了25秒则启用报警", "反潜时先进门后出门 (=0xa5时,可以先进, 也可先出)", "两张连续读卡的最长间隔100=10秒", "启用定时操作列表", "定时: 8个口触发动作4个门,4个扩展口",
				"01010101 单个读卡器, 进门读卡器启用 多卡同时开门", "报警自动复位 默认0x01", "报警的最小输出时间3秒(30)", "此通信时间内由电脑控制刷卡的开门(秒)", "假期约束, 默认00不启用", "防盗报警控制基本参数 ALARM_CONTROL_MODE_DEFAULT", "设防延时 30秒(30)", "撤防延时 30秒(30)", "门内最多人数 低位", "门内最多人数 高位",
				"门内最少人数 低位", "门内最少人数 高位", "有效刷卡间隔 低位(单位:*2秒)", "刷卡有效间隔 高位", "心跳最大周期 低位(单位: 分钟)", "心跳最大周期 高位", "通信—接收火警信号", "通信—发送火警信号", "通信—接收互锁信号", "通信—发送互锁信号",
				"记录设防状态 默认是撤防 0x00", "单层延时", "多层延时", "通信—发送反潜回卡号信号", "--停用", "--停用", "--停用", "--停用", "--停用", "--停用",
				"--停用", "--停用", "--停用", "--停用", "胁迫密码字节1", "胁迫密码字节2", "1号#1 17组开门密码(2个一组, 0xffff无效)", "1号读卡器组1 开门密码", "1号读卡器组2 开门密码", "1号读卡器组2 开门密码",
				"1号读卡器组3 开门密码", "1号读卡器组3 开门密码", "1号读卡器组4 开门密码", "1号读卡器组4 开门密码", "2号读卡器组1 开门密码", "2号读卡器组1 开门密码", "2号读卡器组2 开门密码", "2号读卡器组2 开门密码", "2号读卡器组3 开门密码", "2号读卡器组3 开门密码",
				"2号读卡器组4 开门密码", "2号读卡器组4 开门密码", "3号读卡器组1 开门密码", "3号读卡器组1 开门密码", "3号读卡器组2 开门密码", "3号读卡器组2 开门密码", "3号读卡器组3 开门密码", "3号读卡器组3 开门密码", "3号读卡器组4 开门密码", "3号读卡器组4 开门密码",
				"4号读卡器组1 开门密码", "4号读卡器组1 开门密码", "4号读卡器组2 开门密码", "4号读卡器组2 开门密码", "4号读卡器组3 开门密码", "4号读卡器组3 开门密码", "4号读卡器组4 开门密码", "4号读卡器组4 开门密码", "开门密码", "开门密码",
				"--停用", "--停用", "--停用", "--停用", "--停用", "--停用", "IP地址", "IP地址", "IP地址", "IP地址",
				"掩码", "掩码", "掩码", "掩码", "网关", "网关", "网关", "网关", "端口低位 默认60000=0xEA60", "端口高位",
				"启用DHCP(=0xa5时表示启用) 2010-6-18", "启用 禁止10M尝试(=0xa5时表示启用) 2014-03-17 [V5.52 =0xa6强制为10M半双工]", "停用 通信密码字节3", "停用 通信密码字节4", "停用 通信密码字节5", "停用 通信密码字节6", "扩展板1的参数表 默认四个继电器一一对应", "", "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
				"动作时间缺省是10秒", "动作时间 高位", "扩展板2的参数表 默认四个继电器一一对应", "", "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "", "动作时间缺省是10秒", "动作时间 高位", "扩展板3的参数表 默认四个继电器一一对应", "",
				"默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "", "动作时间缺省是10秒", "动作时间 高位", "扩展板4的参数表 默认四个继电器一一对应", "", "", "防盗(刷卡,24小时,紧急", "动作时间缺省是300秒", "动作时间 高位",
				"读卡器性质", "通过密码键盘直接输入卡号,方式是:*卡号*", "门打开时间过长的高位[单位是25.6秒]", "胁迫密码字节3", "1号读卡器首卡开门", "2号读卡器首卡开门", "3号读卡器首卡开门", "4号读卡器首卡开门", "1号读卡器必须要到的人数", "2号读卡器必须要到的人数",
				"3号读卡器必须要到的人数", "4号读卡器必须要到的人数", "1号读卡器群1", "1号读卡器群2", "1号读卡器群3", "1号读卡器群4", "1号读卡器群5", "1号读卡器群6", "1号读卡器群7", "1号读卡器群8",
				"2号读卡器群1", "2号读卡器群2", "2号读卡器群3", "2号读卡器群4", "2号读卡器群5", "2号读卡器群6", "2号读卡器群7", "2号读卡器群8", "3号读卡器群1", "3号读卡器群2",
				"3号读卡器群3", "3号读卡器群4", "3号读卡器群5", "3号读卡器群6", "3号读卡器群7", "3号读卡器群8", "4号读卡器群1", "4号读卡器群2", "4号读卡器群3", "4号读卡器群4",
				"4号读卡器群5", "4号读卡器群6", "4号读卡器群7", "4号读卡器群8", "记录门磁事件[开门或关门的时间]", "无效刷卡时, 禁止读卡器LED灯光/BEEP声音输出 (驱动V5.48或以上支持) =0xa5启用", "禁止手机WEB管理自动设置控制器IP功能(驱动V5.45以上) =0xa5启用", "刷一次卡(驱动V5.48以上)", "目标主机IP1 用于发送新的记录 默认广播全F", "目标主机IP2",
				"目标主机IP3", "目标主机IP4", "PC机 SERVER 工作口低位(61001)", "PC机 SERVER 工作口高位(61001)", "UDP CLIENT 控制端工作口低位(61002)", "UDP CLIENT 控制端工作口高位(61002)", "控制器主动发送的周期 低位. 默认不发", "控制器主动发送的周期 高位. 默认不发", "1号读卡器多卡选项", "2号读卡器多卡选项",
				"3号读卡器多卡选项", "4号读卡器多卡选项", " 开关模式 LOCK_SWITCH_OPTION", "1号读卡器组1 开门密码第3字节", "1号读卡器组2 开门密码第3字节", "1号读卡器组3 开门密码第3字节", "1号读卡器组4 开门密码第3字节", "2号读卡器组1 开门密码第3字节", "2号读卡器组2 开门密码第3字节", "2号读卡器组3 开门密码第3字节",
				"2号读卡器组4 开门密码第3字节", "3号读卡器组1 开门密码第3字节", "3号读卡器组2 开门密码第3字节", "3号读卡器组3 开门密码第3字节", "3号读卡器组4 开门密码第3字节", "4号读卡器组1 开门密码第3字节", "4号读卡器组2 开门密码第3字节", "4号读卡器组3 开门密码第3字节", "4号读卡器组4 开门密码第3字节", "手机开门模拟刷卡输入(设备驱动V5.40以上) =0xa5启用; ",
				"1号门 开始禁用的时段(不含0,1)", "2号门 开始禁用的时段(不含0,1)", "3号门 开始禁用的时段(不含0,1)", "4号门 开始禁用的时段(不含0,1)", "(V5.52)刷卡自动延长截止日期(=0不启用, >0 表示延长天数)", "(V5.52)刷卡自动延长截止日期 高字节(=0不启用, >0 表示延长天数)", "(V5.52)按钮1失效(=0不启用, >0)", "(V5.52)按钮2失效(=0不启用, >0)", "(V5.52)按钮3失效(=0不启用, >0)", "(V5.52)按钮4失效(=0不启用, >0)",
				"(V5.52)合法卡连续刷4次切换常开或在线 高4字节时段约束(0所有用户). 低4字节对应4个读卡器"
			};
			this.m_needUpdateBits = new byte[128];
			this.m_param_Special = new byte[1024];
			byte[] array = new byte[]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				3, 3, 3, 3, 0, 0, 0, 0, 1, 2,
				3, 4, 0, 0, 0, 0, byte.MaxValue, 0, byte.MaxValue, 0,
				0, 0, 0, 0, 8, 250, 0, 100, 0, byte.MaxValue,
				85, 1, 30, 0, 0, 126, 30, 30, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				0, 4, 50, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 192, 168, 0, 0,
				byte.MaxValue, byte.MaxValue, 0, 0, 192, 168, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 1, 0, 39, 0,
				0, 0, 2, 0, 39, 0, 0, 0, 4, 0,
				39, 0, 0, 0, 16, 0, 0, 56, 0, 0,
				10, 0, 0, 13, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0
			};
			array[0] = wgMjControllerConfigure.LowByte(32269);
			array[1] = wgMjControllerConfigure.HighByte(32269);
			array[2] = wgMjControllerConfigure.LowByte(30);
			array[3] = wgMjControllerConfigure.HighByte(30);
			array[4] = wgMjControllerConfigure.LowByte(30);
			array[5] = wgMjControllerConfigure.HighByte(30);
			array[6] = wgMjControllerConfigure.LowByte(30);
			array[7] = wgMjControllerConfigure.HighByte(30);
			array[8] = wgMjControllerConfigure.LowByte(30);
			array[9] = wgMjControllerConfigure.HighByte(30);
			array[74] = wgMjControllerConfigure.LowByte(889988);
			array[75] = wgMjControllerConfigure.HighByte(889988);
			array[128] = wgMjControllerConfigure.LowByte(60000);
			array[129] = wgMjControllerConfigure.HighByte(60000);
			array[140] = wgMjControllerConfigure.LowByte(30);
			array[141] = wgMjControllerConfigure.HighByte(30);
			array[146] = wgMjControllerConfigure.LowByte(30);
			array[147] = wgMjControllerConfigure.HighByte(30);
			array[152] = wgMjControllerConfigure.LowByte(30);
			array[153] = wgMjControllerConfigure.HighByte(30);
			array[158] = wgMjControllerConfigure.LowByte(3000);
			array[159] = wgMjControllerConfigure.HighByte(3000);
			array[212] = wgMjControllerConfigure.LowByte(61001);
			array[213] = wgMjControllerConfigure.HighByte(61001);
			array[214] = wgMjControllerConfigure.LowByte(61002);
			array[215] = wgMjControllerConfigure.HighByte(61002);
			array[216] = wgMjControllerConfigure.LowByte(0);
			array[217] = wgMjControllerConfigure.HighByte(0);
			this.m_param = array;
			this.Clear();
			try
			{
				if (pkt[0] == 36)
				{
					if (pkt[1] == 65)
					{
						this.m_controllerSN = (int)pkt[8] + ((int)pkt[9] << 8) + ((int)pkt[10] << 16) + ((int)pkt[11] << 24);
						Array.Copy(pkt, startIndex, this.m_param_Special, 384, 4);
						Array.Copy(pkt, startIndex, this.m_param, 116, 4);
						Array.Copy(pkt, startIndex + 4, this.m_param_Special, 388, 4);
						Array.Copy(pkt, startIndex + 4, this.m_param, 120, 4);
						Array.Copy(pkt, startIndex + 8, this.m_param_Special, 392, 4);
						Array.Copy(pkt, startIndex + 8, this.m_param, 124, 4);
						Array.Copy(pkt, startIndex + 12, this.m_param_Special, 128, 2);
						Array.Copy(pkt, startIndex + 12, this.m_param, 128, 2);
						Array.Copy(pkt, startIndex + 14, this.m_param_Special, 110, 6);
						Array.Copy(pkt, startIndex + 14, this.m_param, 110, 6);
						Array.Copy(pkt, startIndex + 20, this.m_param_Special, 468, 4);
						Array.Copy(pkt, startIndex + 20, this.m_param, 468, 2);
						Array.Copy(pkt, startIndex + 24, this.m_param_Special, 464, 2);
						Array.Copy(pkt, startIndex + 24, this.m_param, 464, 2);
						Array.Copy(pkt, startIndex + 26, this.m_param_Special, 466, 1);
						Array.Copy(pkt, startIndex + 26, this.m_param, 466, 1);
						Array.Copy(pkt, startIndex + 27, this.m_param_Special, 467, 1);
						Array.Copy(pkt, startIndex + 27, this.m_param, 467, 1);
					}
					else
					{
						if (pkt[1] == 17)
						{
							Array.Copy(pkt, startIndex, this.m_param, 0, this.m_param.Length);
							this.m_controllerSN = (int)pkt[8] + ((int)pkt[9] << 8) + ((int)pkt[10] << 16) + ((int)pkt[11] << 24);
							Array.Copy(pkt, startIndex, this.m_param_Special, 0, (this.m_param_Special.Length < pkt.Length) ? this.m_param_Special.Length : pkt.Length);
						}
						if (pkt[1] == 81)
						{
							Array.Copy(pkt, startIndex, this.m_param, 0, this.m_param.Length);
							this.m_controllerSN = (int)pkt[8] + ((int)pkt[9] << 8) + ((int)pkt[10] << 16) + ((int)pkt[11] << 24);
							Array.Copy(pkt, startIndex, this.m_param_Special, 0, (this.m_param_Special.Length < pkt.Length) ? this.m_param_Special.Length : pkt.Length);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00104130 File Offset: 0x00103130
		private void ByteBitSet(ref byte param, int bitloc, bool value)
		{
			if (bitloc >= 0 && bitloc <= 7)
			{
				param = (byte)((int)param & ~(1 << bitloc) & 255);
				if (value)
				{
					param |= (byte)((1 << bitloc) & 255);
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00104163 File Offset: 0x00103163
		private bool ByteBitValue(byte param, int bitloc)
		{
			return bitloc >= 0 && bitloc <= 7 && ((int)param & (1 << bitloc)) > 0;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0010417C File Offset: 0x0010317C
		public void Clear()
		{
			for (int i = 0; i < this.m_needUpdateBits.Length; i++)
			{
				this.m_needUpdateBits[i] = 0;
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x001041A5 File Offset: 0x001031A5
		public int disablePushbuttonGet(int DoorNO)
		{
			return (int)this.m_param[246 + (DoorNO - 1)];
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x001041B7 File Offset: 0x001031B7
		public void disablePushbuttonSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.m_param[246 + (DoorNO - 1)] = (byte)(value & 255);
				this.SetUpdateBits(246 + (DoorNO - 1), 1);
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x001041EA File Offset: 0x001031EA
		public int DoorControlGet(int DoorNO)
		{
			return (int)this.m_param[10 + (DoorNO - 1)];
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x001041F9 File Offset: 0x001031F9
		public void DoorControlSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.m_param[10 + (DoorNO - 1)] = (byte)(value & 255);
				this.SetUpdateBits(10 + (DoorNO - 1), 1);
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00104226 File Offset: 0x00103226
		public int DoorDelayGet(int DoorNO)
		{
			return this.GetIntValue(2 + (DoorNO - 1) * 2) / 10;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00104238 File Offset: 0x00103238
		public void DoorDelaySet(int DoorNO, int value)
		{
			if (DoorNO <= 4)
			{
				this.SetIntValue(2 + (DoorNO - 1) * 2, value * 10);
			}
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0010424F File Offset: 0x0010324F
		public int DoorDisableTimesegMinGet(int DoorNO)
		{
			return (int)this.m_param[240 + (DoorNO - 1)];
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00104261 File Offset: 0x00103261
		public void DoorDisableTimesegMinSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.m_param[240 + (DoorNO - 1)] = (byte)(value & 255);
				this.SetUpdateBits(240 + (DoorNO - 1), 1);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00104294 File Offset: 0x00103294
		public int DoorInterlockGet(int DoorNO)
		{
			return (int)this.m_param[14 + (DoorNO - 1)];
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x001042A3 File Offset: 0x001032A3
		public void DoorInterlockSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.m_param[14 + (DoorNO - 1)] = (byte)(value & 255);
				this.SetUpdateBits(14 + (DoorNO - 1), 1);
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x001042D0 File Offset: 0x001032D0
		public string doorNameGet(int DoorNO)
		{
			if (DoorNO < 1 || DoorNO > 4)
			{
				return "";
			}
			int num = 292 + (DoorNO - 1) * 32 - 16;
			if ((this.m_param[num] == 255 && this.m_param[num + 1] == 255) || this.m_param[num] == 0)
			{
				return "";
			}
			int num2 = 0;
			int num3 = 0;
			while (num3 < 32 && this.m_param[num + num3] != 0)
			{
				num2++;
				num3++;
			}
			return Encoding.GetEncoding("utf-8").GetString(this.m_param, num, num2);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00104364 File Offset: 0x00103364
		public int doorNameSet(int DoorNO, string value)
		{
			if (DoorNO < 1 || DoorNO > 4)
			{
				return 0;
			}
			int num = 292 + (DoorNO - 1) * 32 - 16;
			byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(value);
			for (int i = 0; i < 32; i++)
			{
				this.m_param[num + i] = 0;
				this.SetUpdateBits(num + i, 1);
			}
			int num2 = 0;
			while (num2 < 32 && num2 < bytes.Length)
			{
				this.m_param[num + num2] = bytes[num2];
				num2++;
			}
			return 1;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x001043DF File Offset: 0x001033DF
		public int Ext_controlGet(int extPortNum)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				return (int)this.m_param[136 + extPortNum * 6 + 1];
			}
			return 0;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x001043FD File Offset: 0x001033FD
		public void Ext_controlSet(int extPortNum, int value)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				this.m_param[136 + extPortNum * 6 + 1] = (byte)(value & 255);
				this.SetUpdateBits(136 + extPortNum * 6 + 1, 1);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00104433 File Offset: 0x00103433
		public int Ext_doorGet(int extPortNum)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				return (int)this.m_param[136 + extPortNum * 6];
			}
			return 0;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0010444F File Offset: 0x0010344F
		public void Ext_doorSet(int extPortNum, int value)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				this.m_param[136 + extPortNum * 6] = (byte)(value & 255);
				this.SetUpdateBits(136 + extPortNum * 6, 1);
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00104481 File Offset: 0x00103481
		public int Ext_timeoutGet(int extPortNum)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				return this.GetIntValue(136 + extPortNum * 6 + 4) / 10;
			}
			return 0;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x001044A1 File Offset: 0x001034A1
		public void Ext_timeoutSet(int extPortNum, int value)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				this.SetIntValue(136 + extPortNum * 6 + 4, value * 10);
				this.SetUpdateBits(136 + extPortNum * 6 + 4, 2);
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x001044D2 File Offset: 0x001034D2
		public int Ext_warnSignalEnabled2Get(int extPortNum)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				return (int)this.m_param[136 + extPortNum * 6 + 3];
			}
			return 0;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x001044F0 File Offset: 0x001034F0
		public void Ext_warnSignalEnabled2Set(int extPortNum, int value)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				this.m_param[136 + extPortNum * 6 + 3] = (byte)(value & 255);
				this.SetUpdateBits(136 + extPortNum * 6 + 3, 1);
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00104526 File Offset: 0x00103526
		public int Ext_warnSignalEnabledGet(int extPortNum)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				return (int)this.m_param[136 + extPortNum * 6 + 2];
			}
			return 0;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00104544 File Offset: 0x00103544
		public void Ext_warnSignalEnabledSet(int extPortNum, int value)
		{
			if (extPortNum >= 0 && extPortNum <= 3)
			{
				this.m_param[136 + extPortNum * 6 + 2] = (byte)(value & 255);
				this.SetUpdateBits(136 + extPortNum * 6 + 2, 1);
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0010457A File Offset: 0x0010357A
		public int FirstCardInfoGet(int DoorNO)
		{
			return (int)this.m_param[164 + (DoorNO - 1)];
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0010458C File Offset: 0x0010358C
		public void FirstCardInfoSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.m_param[164 + (DoorNO - 1)] = (byte)(value & 255);
				this.SetUpdateBits(164 + (DoorNO - 1), 1);
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x001045BF File Offset: 0x001035BF
		private int GetIntValue(int paramLoc)
		{
			if (paramLoc + 1 < this.m_param.Length)
			{
				return (int)this.m_param[paramLoc] + ((int)this.m_param[paramLoc + 1] << 8);
			}
			return (int)this.m_param_Special[paramLoc] + ((int)this.m_param_Special[paramLoc + 1] << 8);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x001045F9 File Offset: 0x001035F9
		private static byte HighByte(int i)
		{
			return (byte)((i >> 8) & 255);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00104605 File Offset: 0x00103605
		public int InputCardNOOpenGet(int ReaderNO)
		{
			if (this.IsValidNO_ReaderDoor(ReaderNO) && (this.input_cardno & (1 << ReaderNO - 1)) > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00104625 File Offset: 0x00103625
		public void InputCardNOOpenSet(int ReaderNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(ReaderNO))
			{
				if (value > 0)
				{
					this.input_cardno |= 1 << ReaderNO - 1;
					return;
				}
				this.input_cardno &= ~(1 << ReaderNO - 1);
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00104660 File Offset: 0x00103660
		private bool IsValidNO_ReaderDoor(int value)
		{
			return value >= 1 && value <= 4;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0010466F File Offset: 0x0010366F
		private static byte LowByte(int i)
		{
			return (byte)(i & 255);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00104679 File Offset: 0x00103679
		public int MorecardGroupNeedCardsGet(int DoorNO, int GroupNO)
		{
			return (int)this.m_param[172 + (DoorNO - 1) * 8 + GroupNO - 1];
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00104694 File Offset: 0x00103694
		public void MorecardGroupNeedCardsSet(int DoorNO, int GroupNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO) && GroupNO >= 1 && GroupNO <= 8 && value >= 0 && value <= 255)
			{
				this.m_param[172 + (DoorNO - 1) * 8 + GroupNO - 1] = (byte)(value & 255);
				this.SetUpdateBits(172 + (DoorNO - 1) * 8 + GroupNO - 1, 1);
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x001046F2 File Offset: 0x001036F2
		public int MorecardNeedCardsGet(int DoorNo)
		{
			return (int)this.m_param[168 + DoorNo - 1];
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00104704 File Offset: 0x00103704
		public void MorecardNeedCardsSet(int DoorNo, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNo) && value >= 0 && value <= 255)
			{
				this.m_param[168 + DoorNo - 1] = (byte)(value & 255);
				this.SetUpdateBits(168 + DoorNo - 1, 1);
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00104743 File Offset: 0x00103743
		public bool MorecardSequenceInputGet(int DoorNO)
		{
			return this.ByteBitValue(this.m_param[218 + DoorNO - 1], 4);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0010475C File Offset: 0x0010375C
		public void MorecardSequenceInputSet(int DoorNO, bool value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.ByteBitSet(ref this.m_param[218 + DoorNO - 1], 4, value);
				this.SetUpdateBits(218 + DoorNO - 1, 1);
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00104793 File Offset: 0x00103793
		public bool MorecardSingleGroupEnableGet(int DoorNO)
		{
			return this.ByteBitValue(this.m_param[218 + DoorNO - 1], 3);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x001047AC File Offset: 0x001037AC
		public void MorecardSingleGroupEnableSet(int DoorNO, bool value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO))
			{
				this.ByteBitSet(ref this.m_param[218 + DoorNO - 1], 3, value);
				this.SetUpdateBits(218 + DoorNO - 1, 1);
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x001047E3 File Offset: 0x001037E3
		public int MorecardSingleGroupStartNOGet(int DoorNO)
		{
			return (int)((this.m_param[218 + DoorNO - 1] & 7) + 1);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x001047FC File Offset: 0x001037FC
		public void MorecardSingleGroupStartNOSet(int DoorNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(DoorNO) && value >= 1 && value <= 8)
			{
				this.m_param[218 + DoorNO - 1] = this.m_param[218 + DoorNO - 1] & 248;
				this.m_param[218 + DoorNO - 1] = this.m_param[218 + DoorNO - 1] | (byte)((value - 1) & 7);
				this.SetUpdateBits(218 + DoorNO - 1, 1);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0010487C File Offset: 0x0010387C
		public void MultiOutputNormalOpen(long value1)
		{
			this.m_param[4] = (byte)(value1 & 255L);
			this.m_param[5] = (byte)((value1 >> 8) & 255L);
			this.m_param[6] = (byte)((value1 >> 16) & 255L);
			this.m_param[7] = (byte)((value1 >> 24) & 255L);
			this.m_param[8] = (byte)((value1 >> 32) & 255L);
			this.SetUpdateBits(4, 1);
			this.SetUpdateBits(5, 1);
			this.SetUpdateBits(6, 1);
			this.SetUpdateBits(7, 1);
			this.SetUpdateBits(8, 1);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00104911 File Offset: 0x00103911
		public int ReaderPasswordGet(int ReaderNO)
		{
			return (int)this.m_param[22 + (ReaderNO - 1)];
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00104920 File Offset: 0x00103920
		public void ReaderPasswordSet(int ReaderNO, int value)
		{
			if (this.IsValidNO_ReaderDoor(ReaderNO))
			{
				this.m_param[22 + (ReaderNO - 1)] = (byte)(value & 255);
				if (wgTools.gbInputKeyPasswordControl && value > 0)
				{
					this.m_param[22 + (ReaderNO - 1)] = (byte)((value | 2) & 255);
				}
				this.SetUpdateBits(22 + (ReaderNO - 1), 1);
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0010497C File Offset: 0x0010397C
		public void RestoreDefault()
		{
			int i;
			for (i = 0; i <= this.m_param.Length >> 3; i++)
			{
				this.m_needUpdateBits[i] = byte.MaxValue;
			}
			this.m_needUpdateBits[i] = byte.MaxValue;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x001049B8 File Offset: 0x001039B8
		private void SetIntValue(int paramLoc, int value)
		{
			if (paramLoc < this.m_param.Length)
			{
				this.m_param[paramLoc] = wgMjControllerConfigure.LowByte(value);
				this.m_param[paramLoc + 1] = wgMjControllerConfigure.HighByte(value);
			}
			this.SetUpdateBits(paramLoc, 2);
			this.m_param_Special[paramLoc] = wgMjControllerConfigure.LowByte(value);
			this.m_param_Special[paramLoc + 1] = wgMjControllerConfigure.HighByte(value);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00104A14 File Offset: 0x00103A14
		private void SetUpdateBits(int paramloc, int paramByteLen)
		{
			if (paramloc < 1024 && paramloc + paramByteLen < 1024)
			{
				for (int i = paramloc; i < paramloc + paramByteLen; i++)
				{
					this.m_needUpdateBits[i >> 3] = this.m_needUpdateBits[i >> 3] | (byte)(1 << (i & 7));
				}
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00104A60 File Offset: 0x00103A60
		public int SuperpasswordGet(int pwdNO)
		{
			return (int)this.m_param[223 + (pwdNO - 1)] << 16 + this.GetIntValue(76 + (pwdNO - 1) * 2);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00104A88 File Offset: 0x00103A88
		public void SuperpasswordSet(int pwdNO, int value)
		{
			if (pwdNO <= 17)
			{
				this.SetIntValue(76 + (pwdNO - 1) * 2, value);
				this.SetUpdateBits(76 + (pwdNO - 1) * 2, 2);
				this.m_param[223 + (pwdNO - 1)] = (byte)((value >> 16) & 255);
				this.SetUpdateBits(223 + (pwdNO - 1), 1);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00104AE3 File Offset: 0x00103AE3
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00104AF0 File Offset: 0x00103AF0
		public int antiback
		{
			get
			{
				return (int)this.m_param[31];
			}
			set
			{
				this.m_param[31] = (byte)(value & 255);
				this.SetUpdateBits(31, 1);
				switch (this.m_param[31])
				{
				case 1:
					this.m_param[160] = 250;
					break;
				case 2:
					this.m_param[160] = 250;
					break;
				case 3:
					this.m_param[160] = 126;
					break;
				case 4:
					this.m_param[160] = 254;
					break;
				default:
					this.m_param[160] = 0;
					break;
				}
				this.SetUpdateBits(160, 1);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00104B9D File Offset: 0x00103B9D
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00104BBA File Offset: 0x00103BBA
		public int antiback_broadcast_send
		{
			get
			{
				if (this.m_param[63] != 255)
				{
					return (int)this.m_param[63];
				}
				return 0;
			}
			set
			{
				this.m_param[63] = (byte)(value & 255);
				this.SetUpdateBits(63, 1);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00104BD6 File Offset: 0x00103BD6
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x00104BE4 File Offset: 0x00103BE4
		public int antiback_validtime
		{
			get
			{
				return (int)this.m_param[478];
			}
			set
			{
				this.m_param[478] = (byte)(value & 255);
				this.SetUpdateBits(478, 1);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00104C06 File Offset: 0x00103C06
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x00104C11 File Offset: 0x00103C11
		public int autiback_allow_firstout_enable
		{
			get
			{
				return (int)this.m_param[36];
			}
			set
			{
				this.m_param[36] = (byte)(value & 255);
				this.SetUpdateBits(36, 1);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00104C2D File Offset: 0x00103C2D
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00104C3B File Offset: 0x00103C3B
		public int auto_negotiation_enable
		{
			get
			{
				return (int)this.m_param[131];
			}
			set
			{
				this.m_param[131] = (byte)(value & 255);
				this.SetUpdateBits(131, 1);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00104C5D File Offset: 0x00103C5D
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x00104C6B File Offset: 0x00103C6B
		public int auto_try10M_disable
		{
			get
			{
				return (int)this.m_param[131];
			}
			set
			{
				this.m_param[131] = (byte)(value & 255);
				this.SetUpdateBits(131, 1);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00104C8D File Offset: 0x00103C8D
		// (set) Token: 0x06000D50 RID: 3408 RVA: 0x00104CAC File Offset: 0x00103CAC
		public int autoDelayValidDateDays
		{
			get
			{
				return (int)this.m_param[244] + ((int)this.m_param[245] << 8);
			}
			set
			{
				this.m_param[244] = (byte)(value & 255);
				this.m_param[245] = (byte)((value >> 8) & 255);
				this.SetUpdateBits(244, 1);
				this.SetUpdateBits(245, 1);
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00104CFB File Offset: 0x00103CFB
		// (set) Token: 0x06000D52 RID: 3410 RVA: 0x00104D05 File Offset: 0x00103D05
		public int check_controller_online_timeout
		{
			get
			{
				return this.GetIntValue(54);
			}
			set
			{
				this.SetIntValue(54, value);
				this.SetUpdateBits(54, 2);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00104D19 File Offset: 0x00103D19
		// (set) Token: 0x06000D54 RID: 3412 RVA: 0x00104D27 File Offset: 0x00103D27
		public int check_two_cards
		{
			get
			{
				return (int)this.m_param[475];
			}
			set
			{
				this.m_param[475] = (byte)(value & 255);
				this.SetUpdateBits(475, 1);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00104D49 File Offset: 0x00103D49
		// (set) Token: 0x06000D56 RID: 3414 RVA: 0x00104D65 File Offset: 0x00103D65
		public int controllerServer
		{
			get
			{
				return this.GetIntValue(460) + (this.GetIntValue(462) << 16);
			}
			set
			{
				this.SetIntValue(460, value);
				this.SetIntValue(462, value >> 16);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00104D82 File Offset: 0x00103D82
		public int controllerSN
		{
			get
			{
				return this.m_controllerSN;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00104D8A File Offset: 0x00103D8A
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x00104D95 File Offset: 0x00103D95
		public int controlTaskList_enabled
		{
			get
			{
				return (int)this.m_param[38];
			}
			set
			{
				this.m_param[38] = (byte)(value & 255);
				this.SetUpdateBits(38, 1);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00104DB1 File Offset: 0x00103DB1
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00104DBF File Offset: 0x00103DBF
		public int custom_cardformat_startloc
		{
			get
			{
				return (int)this.m_param[133];
			}
			set
			{
				this.m_param[133] = (byte)(value & 255);
				this.SetUpdateBits(133, 1);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00104DE1 File Offset: 0x00103DE1
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00104DEF File Offset: 0x00103DEF
		public int custom_cardformat_sumcheck
		{
			get
			{
				return (int)this.m_param[135];
			}
			set
			{
				this.m_param[135] = (byte)(value & 255);
				this.SetUpdateBits(135, 1);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00104E11 File Offset: 0x00103E11
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00104E1F File Offset: 0x00103E1F
		public int custom_cardformat_totalbits
		{
			get
			{
				return (int)this.m_param[132];
			}
			set
			{
				this.m_param[132] = (byte)(value & 255);
				this.SetUpdateBits(132, 1);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00104E41 File Offset: 0x00103E41
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00104E4F File Offset: 0x00103E4F
		public int custom_cardformat_validbits
		{
			get
			{
				return (int)this.m_param[134];
			}
			set
			{
				this.m_param[134] = (byte)(value & 255);
				this.SetUpdateBits(134, 1);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00104E74 File Offset: 0x00103E74
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00104EE8 File Offset: 0x00103EE8
		public IPAddress dataServer3ShortIP
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[464],
					this.m_param[465],
					this.m_param[466],
					this.m_param[467]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 464);
				this.SetUpdateBits(464, 4);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00104F0C File Offset: 0x00103F0C
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00104F19 File Offset: 0x00103F19
		public int dataServer3ShortPort
		{
			get
			{
				return this.GetIntValue(468);
			}
			set
			{
				this.SetIntValue(468, value);
				this.SetUpdateBits(468, 2);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00104F33 File Offset: 0x00103F33
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x00104F40 File Offset: 0x00103F40
		public int dataServerCycle
		{
			get
			{
				return this.GetIntValue(216);
			}
			set
			{
				this.SetIntValue(216, value);
				this.SetUpdateBits(216, 2);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00104F5C File Offset: 0x00103F5C
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00104FD0 File Offset: 0x00103FD0
		public IPAddress dataServerIP
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[208],
					this.m_param[209],
					this.m_param[210],
					this.m_param[211]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 208);
				this.SetUpdateBits(208, 4);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00104FF4 File Offset: 0x00103FF4
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00105001 File Offset: 0x00104001
		public int dataServerPort
		{
			get
			{
				return this.GetIntValue(212);
			}
			set
			{
				this.SetIntValue(212, value);
				this.SetUpdateBits(212, 2);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0010501C File Offset: 0x0010401C
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00105090 File Offset: 0x00104090
		public IPAddress dataServerShort2IP
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[412],
					this.m_param[413],
					this.m_param[414],
					this.m_param[415]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 412);
				this.SetUpdateBits(412, 4);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x001050B4 File Offset: 0x001040B4
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x001050C2 File Offset: 0x001040C2
		public int dataServerShort2Option
		{
			get
			{
				return (int)this.m_param[418];
			}
			set
			{
				this.m_param[418] = (byte)value;
				this.SetUpdateBits(418, 1);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x001050DE File Offset: 0x001040DE
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x001050EB File Offset: 0x001040EB
		public int dataServerShort2Port
		{
			get
			{
				return this.GetIntValue(416);
			}
			set
			{
				this.SetIntValue(416, value);
				this.SetUpdateBits(416, 2);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00105105 File Offset: 0x00104105
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00105113 File Offset: 0x00104113
		public int dataServerShortCycle
		{
			get
			{
				return (int)this.m_param[411];
			}
			set
			{
				this.m_param[411] = (byte)value;
				this.SetUpdateBits(411, 1);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00105130 File Offset: 0x00104130
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x001051A4 File Offset: 0x001041A4
		public IPAddress dataServerShortIP
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[404],
					this.m_param[405],
					this.m_param[406],
					this.m_param[407]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 404);
				this.SetUpdateBits(404, 4);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x001051C8 File Offset: 0x001041C8
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x001051D6 File Offset: 0x001041D6
		public int dataServerShortOption
		{
			get
			{
				return (int)this.m_param[410];
			}
			set
			{
				this.m_param[410] = (byte)value;
				this.SetUpdateBits(410, 1);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x001051F2 File Offset: 0x001041F2
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x001051FF File Offset: 0x001041FF
		public int dataServerShortPort
		{
			get
			{
				return this.GetIntValue(408);
			}
			set
			{
				this.SetIntValue(408, value);
				this.SetUpdateBits(408, 2);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00105219 File Offset: 0x00104219
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00105227 File Offset: 0x00104227
		public int dhcpEnable
		{
			get
			{
				return (int)this.m_param[130];
			}
			set
			{
				this.m_param[130] = (byte)(value & 255);
				this.SetUpdateBits(130, 1);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00105249 File Offset: 0x00104249
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00105257 File Offset: 0x00104257
		public int disable_pushbutton
		{
			get
			{
				return (int)this.m_param[246];
			}
			set
			{
				this.m_param[246] = (byte)(value & 255);
				this.SetUpdateBits(246, 1);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00105279 File Offset: 0x00104279
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00105296 File Offset: 0x00104296
		public int doorOpenTimeout
		{
			get
			{
				return ((int)this.m_param[35] + ((int)this.m_param[162] << 8)) / 10;
			}
			set
			{
				this.m_param[35] = (byte)((value * 10) & 255);
				this.SetUpdateBits(35, 1);
				this.m_param[162] = wgMjControllerConfigure.HighByte(value * 10);
				this.SetUpdateBits(162, 1);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x001052D6 File Offset: 0x001042D6
		public int doorstatusNormal
		{
			get
			{
				return (int)this.m_param[27];
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x001052E1 File Offset: 0x001042E1
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x001052EF File Offset: 0x001042EF
		public int elevator_noasnc
		{
			get
			{
				return (int)this.m_param[483];
			}
			set
			{
				this.m_param[483] = (byte)(value & 255);
				this.SetUpdateBits(483, 1);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00105311 File Offset: 0x00104311
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0010531F File Offset: 0x0010431F
		public int elevator_switch_delay
		{
			get
			{
				return (int)this.m_param[487];
			}
			set
			{
				this.m_param[487] = (byte)(value & 255);
				this.SetUpdateBits(487, 1);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00105341 File Offset: 0x00104341
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0010534F File Offset: 0x0010434F
		public int elevator_switch_frame_floorloc
		{
			get
			{
				return (int)this.m_param[486];
			}
			set
			{
				this.m_param[486] = (byte)(value & 255);
				this.SetUpdateBits(486, 1);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00105371 File Offset: 0x00104371
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x0010537F File Offset: 0x0010437F
		public int elevator_switch_frame_length
		{
			get
			{
				return (int)this.m_param[485];
			}
			set
			{
				this.m_param[485] = (byte)(value & 255);
				this.SetUpdateBits(485, 1);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x001053A1 File Offset: 0x001043A1
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x001053AF File Offset: 0x001043AF
		public int elevator_switch_frame_startflag
		{
			get
			{
				return (int)this.m_param[484];
			}
			set
			{
				this.m_param[484] = (byte)(value & 255);
				this.SetUpdateBits(484, 1);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x001053D1 File Offset: 0x001043D1
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x001053E3 File Offset: 0x001043E3
		public float elevatorMultioutputDelay
		{
			get
			{
				return (float)this.m_param[62] / 10f;
			}
			set
			{
				this.m_param[62] = (byte)((int)((double)(value * 10f) + 0.1) & 255);
				this.SetUpdateBits(62, 1);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00105411 File Offset: 0x00104411
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00105423 File Offset: 0x00104423
		public float elevatorSingleDelay
		{
			get
			{
				return (float)this.m_param[61] / 10f;
			}
			set
			{
				this.m_param[61] = (byte)((int)((double)(value * 10f) + 0.1) & 255);
				this.SetUpdateBits(61, 1);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00105451 File Offset: 0x00104451
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x0010545C File Offset: 0x0010445C
		public int ext_Alarm_Status
		{
			get
			{
				return (int)this.m_param[60];
			}
			set
			{
				this.m_param[60] = (byte)(value & 255);
				this.SetUpdateBits(60, 1);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00105478 File Offset: 0x00104478
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x00105483 File Offset: 0x00104483
		public int ext_AlarmControlMode
		{
			get
			{
				return (int)this.m_param[45];
			}
			set
			{
				this.m_param[45] = (byte)(value & 255);
				this.SetUpdateBits(45, 1);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0010549F File Offset: 0x0010449F
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x001054AA File Offset: 0x001044AA
		public int ext_SetAlarmOffDelay
		{
			get
			{
				return (int)this.m_param[47];
			}
			set
			{
				this.m_param[47] = (byte)(value & 255);
				this.SetUpdateBits(47, 1);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x001054C6 File Offset: 0x001044C6
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x001054D1 File Offset: 0x001044D1
		public int ext_SetAlarmOnDelay
		{
			get
			{
				return (int)this.m_param[46];
			}
			set
			{
				this.m_param[46] = (byte)(value & 255);
				this.SetUpdateBits(46, 1);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x001054ED File Offset: 0x001044ED
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0010550A File Offset: 0x0010450A
		public int fire_broadcast_receive
		{
			get
			{
				if (this.m_param[56] != 255)
				{
					return (int)this.m_param[56];
				}
				return 0;
			}
			set
			{
				this.m_param[56] = (byte)(value & 255);
				this.SetUpdateBits(56, 1);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00105526 File Offset: 0x00104526
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00105543 File Offset: 0x00104543
		public int fire_broadcast_send
		{
			get
			{
				if (this.m_param[57] != 255)
				{
					return (int)this.m_param[57];
				}
				return 0;
			}
			set
			{
				this.m_param[57] = (byte)(value & 255);
				this.SetUpdateBits(57, 1);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0010555F File Offset: 0x0010455F
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0010556D File Offset: 0x0010456D
		public int fourtimes_set_no
		{
			get
			{
				return (int)this.m_param[250];
			}
			set
			{
				this.m_param[250] = (byte)(value & 255);
				this.SetUpdateBits(250, 1);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00105590 File Offset: 0x00104590
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x001055F8 File Offset: 0x001045F8
		public IPAddress gateway
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[124],
					this.m_param[125],
					this.m_param[126],
					this.m_param[127]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 124);
				this.SetUpdateBits(124, 4);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00105616 File Offset: 0x00104616
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00105639 File Offset: 0x00104639
		public int hide_ffffffff
		{
			get
			{
				if (this.m_param[419] != 255)
				{
					return (int)this.m_param[419];
				}
				return 0;
			}
			set
			{
				this.m_param[419] = (byte)(value & 255);
				this.SetUpdateBits(419, 1);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0010565B File Offset: 0x0010465B
		// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x00105666 File Offset: 0x00104666
		public int holidayControl
		{
			get
			{
				return (int)this.m_param[44];
			}
			set
			{
				this.m_param[44] = (byte)(value & 255);
				this.SetUpdateBits(44, 1);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00105682 File Offset: 0x00104682
		public int http_onlyquery
		{
			get
			{
				return (int)this.m_param[467];
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00105690 File Offset: 0x00104690
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x001056AB File Offset: 0x001046AB
		public int indoorPersonsMax
		{
			get
			{
				if (this.GetIntValue(48) != 65535)
				{
					return this.GetIntValue(48);
				}
				return 0;
			}
			set
			{
				this.SetIntValue(48, value);
				this.SetUpdateBits(48, 2);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x001056BF File Offset: 0x001046BF
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x001056DA File Offset: 0x001046DA
		public int indoorPersonsMin
		{
			get
			{
				if (this.GetIntValue(50) != 65535)
				{
					return this.GetIntValue(50);
				}
				return 0;
			}
			set
			{
				this.SetIntValue(50, value);
				this.SetUpdateBits(50, 2);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x001056EE File Offset: 0x001046EE
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x001056FC File Offset: 0x001046FC
		private int input_cardno
		{
			get
			{
				return (int)this.m_param[161];
			}
			set
			{
				this.m_param[161] = (byte)(value & 255);
				this.SetUpdateBits(161, 1);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0010571E File Offset: 0x0010471E
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x0010573B File Offset: 0x0010473B
		public int interlock_broadcast_receive
		{
			get
			{
				if (this.m_param[58] != 255)
				{
					return (int)this.m_param[58];
				}
				return 0;
			}
			set
			{
				this.m_param[58] = (byte)(value & 255);
				this.SetUpdateBits(58, 1);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00105757 File Offset: 0x00104757
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00105774 File Offset: 0x00104774
		public int interlock_broadcast_send
		{
			get
			{
				if (this.m_param[59] != 255)
				{
					return (int)this.m_param[59];
				}
				return 0;
			}
			set
			{
				this.m_param[59] = (byte)(value & 255);
				this.SetUpdateBits(59, 1);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00105790 File Offset: 0x00104790
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x0010579E File Offset: 0x0010479E
		public int invalid_swipe_opendoor
		{
			get
			{
				return (int)this.m_param[244];
			}
			set
			{
				this.m_param[244] = (byte)(value & 255);
				this.SetUpdateBits(244, 1);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x001057C0 File Offset: 0x001047C0
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x001057CE File Offset: 0x001047CE
		public int invalid_swipe_warntimeout
		{
			get
			{
				return (int)this.m_param[245];
			}
			set
			{
				this.m_param[245] = (byte)(value & 255);
				this.SetUpdateBits(245, 1);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x001057F0 File Offset: 0x001047F0
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00105813 File Offset: 0x00104813
		public int invalidCard_ledbeep_output_disable
		{
			get
			{
				if (this.m_param[205] != 255)
				{
					return (int)this.m_param[205];
				}
				return 0;
			}
			set
			{
				this.m_param[205] = (byte)(value & 255);
				this.SetUpdateBits(205, 1);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x00105838 File Offset: 0x00104838
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x001058A0 File Offset: 0x001048A0
		public IPAddress ip
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[116],
					this.m_param[117],
					this.m_param[118],
					this.m_param[119]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 116);
				this.SetUpdateBits(116, 4);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x001058BE File Offset: 0x001048BE
		public int IPListControl
		{
			get
			{
				return (int)this.m_param[396];
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x001058CC File Offset: 0x001048CC
		public int lockNormal
		{
			get
			{
				return (int)this.m_param[28];
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x001058D7 File Offset: 0x001048D7
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x001058F1 File Offset: 0x001048F1
		public int lockSwitchOption
		{
			get
			{
				return (int)((this.m_param[222] ^ byte.MaxValue) & byte.MaxValue);
			}
			set
			{
				this.m_param[222] = (byte)((value ^ 255) & 255);
				this.SetUpdateBits(222, 1);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0010591C File Offset: 0x0010491C
		public string MACAddr
		{
			get
			{
				return string.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}", new object[]
				{
					this.m_param[110],
					this.m_param[111],
					this.m_param[112],
					this.m_param[113],
					this.m_param[114],
					this.m_param[115]
				});
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x001059A4 File Offset: 0x001049A4
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x00105A0C File Offset: 0x00104A0C
		public IPAddress mask
		{
			get
			{
				return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.m_param[120],
					this.m_param[121],
					this.m_param[122],
					this.m_param[123]
				}));
			}
			set
			{
				value.GetAddressBytes().CopyTo(this.m_param, 120);
				this.SetUpdateBits(120, 4);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00105A2A File Offset: 0x00104A2A
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x00105A4D File Offset: 0x00104A4D
		public int mobile_as_card_input
		{
			get
			{
				if (this.m_param[239] != 255)
				{
					return (int)this.m_param[239];
				}
				return 0;
			}
			set
			{
				this.m_param[239] = (byte)(value & 255);
				this.SetUpdateBits(239, 1);
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00105A6F File Offset: 0x00104A6F
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x00105A92 File Offset: 0x00104A92
		public int mobile_web_autoip_disable
		{
			get
			{
				if (this.m_param[206] != 255)
				{
					return (int)this.m_param[206];
				}
				return 0;
			}
			set
			{
				this.m_param[206] = (byte)(value & 255);
				this.SetUpdateBits(206, 1);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00105AB4 File Offset: 0x00104AB4
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x00105ABF File Offset: 0x00104ABF
		public int moreCardRead4Reader
		{
			get
			{
				return (int)this.m_param[40];
			}
			set
			{
				this.m_param[40] = (byte)(value & 255);
				this.SetUpdateBits(40, 1);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00105ADB File Offset: 0x00104ADB
		internal byte[] needUpdate
		{
			get
			{
				return this.m_needUpdateBits;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00105AE3 File Offset: 0x00104AE3
		public byte[] needUpdateBits
		{
			get
			{
				return this.m_needUpdateBits;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00105AEB File Offset: 0x00104AEB
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00105AF9 File Offset: 0x00104AF9
		public int not_comm_doorno
		{
			get
			{
				return (int)this.m_param[474];
			}
			set
			{
				this.m_param[474] = (byte)(value & 255);
				this.SetUpdateBits(474, 1);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00105B1B File Offset: 0x00104B1B
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00105B3E File Offset: 0x00104B3E
		public int one_swipe_limitted_mode
		{
			get
			{
				if (this.m_param[207] != 255)
				{
					return (int)this.m_param[207];
				}
				return 0;
			}
			set
			{
				this.m_param[207] = (byte)(value & 255);
				this.SetUpdateBits(207, 1);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00105B60 File Offset: 0x00104B60
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00105B6E File Offset: 0x00104B6E
		public int open_too_long
		{
			get
			{
				return (int)this.m_param[252];
			}
			set
			{
				this.m_param[252] = (byte)(value & 255);
				this.SetUpdateBits(252, 1);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00105B90 File Offset: 0x00104B90
		public byte[] param
		{
			get
			{
				return this.m_param;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x00105B98 File Offset: 0x00104B98
		internal byte[] paramData
		{
			get
			{
				return this.m_param;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00105BA0 File Offset: 0x00104BA0
		public string[] paramDesc
		{
			get
			{
				return this.m_paramDesc;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00105BA8 File Offset: 0x00104BA8
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x00105BB3 File Offset: 0x00104BB3
		public int pcControlSwipeTimeout
		{
			get
			{
				return (int)this.m_param[43];
			}
			set
			{
				this.m_param[43] = (byte)(value & 255);
				this.SetUpdateBits(43, 1);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00105BCF File Offset: 0x00104BCF
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00105BD7 File Offset: 0x00104BD7
		public string pcIPAddr
		{
			get
			{
				return this._pcIPAddr;
			}
			set
			{
				this._pcIPAddr = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00105BE0 File Offset: 0x00104BE0
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00105BED File Offset: 0x00104BED
		public int port
		{
			get
			{
				return this.GetIntValue(128);
			}
			set
			{
				this.SetIntValue(128, value);
				this.SetUpdateBits(128, 2);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00105C07 File Offset: 0x00104C07
		public int pushbuttonNormal
		{
			get
			{
				return (int)this.m_param[26];
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00105C12 File Offset: 0x00104C12
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x00105C20 File Offset: 0x00104C20
		public int pwd468_Check
		{
			get
			{
				return (int)this.m_param[477];
			}
			set
			{
				this.m_param[477] = (byte)(value & 255);
				this.SetUpdateBits(477, 1);
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00105C42 File Offset: 0x00104C42
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x00105C4D File Offset: 0x00104C4D
		public int readerTimeout
		{
			get
			{
				return (int)this.m_param[34];
			}
			set
			{
				this.m_param[34] = (byte)(value & 255);
				this.SetUpdateBits(34, 1);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00105C69 File Offset: 0x00104C69
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00105C77 File Offset: 0x00104C77
		public int receventDS
		{
			get
			{
				return (int)this.m_param[204];
			}
			set
			{
				this.m_param[204] = (byte)(value & 255);
				this.SetUpdateBits(204, 1);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00105C99 File Offset: 0x00104C99
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x00105CA4 File Offset: 0x00104CA4
		public int receventPB
		{
			get
			{
				return (int)this.m_param[29];
			}
			set
			{
				this.m_param[29] = (byte)(value & 255);
				this.SetUpdateBits(29, 1);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00105CC0 File Offset: 0x00104CC0
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00105CCB File Offset: 0x00104CCB
		public int receventWarn
		{
			get
			{
				return (int)this.m_param[30];
			}
			set
			{
				this.m_param[30] = (byte)(value & 255);
				this.SetUpdateBits(30, 1);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00105CE7 File Offset: 0x00104CE7
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00105CF5 File Offset: 0x00104CF5
		public int rs232_1_extern
		{
			get
			{
				return (int)this.m_param[471];
			}
			set
			{
				this.m_param[471] = (byte)(value & 255);
				this.SetUpdateBits(471, 1);
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00105D17 File Offset: 0x00104D17
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00105D25 File Offset: 0x00104D25
		public int rs232_1_option
		{
			get
			{
				return (int)this.m_param[470];
			}
			set
			{
				this.m_param[470] = (byte)(value & 255);
				this.SetUpdateBits(470, 1);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00105D47 File Offset: 0x00104D47
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x00105D55 File Offset: 0x00104D55
		public int rs232_2_extern
		{
			get
			{
				return (int)this.m_param[472];
			}
			set
			{
				this.m_param[472] = (byte)(value & 255);
				this.SetUpdateBits(472, 1);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00105D77 File Offset: 0x00104D77
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x00105D85 File Offset: 0x00104D85
		public int rs232_2_option
		{
			get
			{
				return (int)this.m_param[476];
			}
			set
			{
				this.m_param[476] = (byte)(value & 255);
				this.SetUpdateBits(476, 1);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00105DA7 File Offset: 0x00104DA7
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x00105DB5 File Offset: 0x00104DB5
		public int rs232_qr_validtime_max
		{
			get
			{
				return (int)this.m_param[473];
			}
			set
			{
				this.m_param[473] = (byte)(value & 255);
				this.SetUpdateBits(473, 1);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00105DD7 File Offset: 0x00104DD7
		public long SpecialCard_Mother1
		{
			get
			{
				return (long)this.GetIntValue(480) + (long)this.GetIntValue(482) * 256L * 256L;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00105E00 File Offset: 0x00104E00
		public long SpecialCard_Mother2
		{
			get
			{
				return (long)this.GetIntValue(488) + (long)this.GetIntValue(490) * 256L * 256L;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00105E29 File Offset: 0x00104E29
		public long SpecialCard_OnlyOpen1
		{
			get
			{
				return (long)this.GetIntValue(512) + (long)this.GetIntValue(514) * 256L * 256L;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00105E52 File Offset: 0x00104E52
		public long SpecialCard_OnlyOpen2
		{
			get
			{
				return (long)this.GetIntValue(520) + (long)this.GetIntValue(522) * 256L * 256L;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00105E7B File Offset: 0x00104E7B
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00105E89 File Offset: 0x00104E89
		public int swipe_auto_addcard
		{
			get
			{
				return (int)this.m_param[251];
			}
			set
			{
				this.m_param[251] = (byte)(value & 255);
				this.SetUpdateBits(251, 1);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00105EAB File Offset: 0x00104EAB
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00105EC8 File Offset: 0x00104EC8
		public int swipeGap
		{
			get
			{
				if (this.GetIntValue(52) != 65535)
				{
					return this.GetIntValue(52) * 2;
				}
				return 0;
			}
			set
			{
				this.SetIntValue(52, value / 2);
				this.SetUpdateBits(52, 2);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00105EDE File Offset: 0x00104EDE
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00105EE9 File Offset: 0x00104EE9
		public int swipeOrderMode
		{
			get
			{
				return (int)this.m_param[33];
			}
			set
			{
				this.m_param[33] = (byte)(value & 255);
				this.SetUpdateBits(33, 1);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00105F05 File Offset: 0x00104F05
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x00105F10 File Offset: 0x00104F10
		public int twoCardReadTimeout
		{
			get
			{
				return (int)this.m_param[37];
			}
			set
			{
				this.m_param[37] = (byte)(value & 255);
				this.SetUpdateBits(37, 1);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x00105F2C File Offset: 0x00104F2C
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x00105F3A File Offset: 0x00104F3A
		public int uart1_baud
		{
			get
			{
				return (int)this.m_param[479];
			}
			set
			{
				this.m_param[479] = (byte)(value & 255);
				this.SetUpdateBits(479, 1);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x00105F5C File Offset: 0x00104F5C
		// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x00105F6A File Offset: 0x00104F6A
		public int uart2_baud
		{
			get
			{
				return (int)this.m_param[480];
			}
			set
			{
				this.m_param[480] = (byte)(value & 255);
				this.SetUpdateBits(480, 1);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00105F8C File Offset: 0x00104F8C
		// (set) Token: 0x06000DFA RID: 3578 RVA: 0x00105F97 File Offset: 0x00104F97
		public int warnAutoReset
		{
			get
			{
				return (int)this.m_param[41];
			}
			set
			{
				this.m_param[41] = (byte)(value & 255);
				this.SetUpdateBits(41, 1);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00105FB3 File Offset: 0x00104FB3
		// (set) Token: 0x06000DFC RID: 3580 RVA: 0x00105FBE File Offset: 0x00104FBE
		public int warnSetup
		{
			get
			{
				return (int)this.m_param[32];
			}
			set
			{
				this.m_param[32] = (byte)(value & 255);
				this.SetUpdateBits(32, 1);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00105FDC File Offset: 0x00104FDC
		public string webDateDisplayFormat
		{
			get
			{
				switch (this.m_param[466])
				{
				case 1:
					return "M-d-yyyy";
				case 2:
					return "d-M-yyyy";
				case 3:
					return "yyyy/M/d";
				case 4:
					return "M/d/yyyy";
				case 5:
					return "d/M/yyyy";
				default:
					return "yyyy-MM-dd";
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x00106038 File Offset: 0x00105038
		public string webDateDisplayFormatCHS
		{
			get
			{
				switch (this.m_param[466])
				{
				case 1:
					return "月-日-年";
				case 2:
					return "日-月-年";
				case 3:
					return "年/月/日";
				case 4:
					return "月/日/年";
				case 5:
					return "日/月/年";
				default:
					return "年-月-日";
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00106094 File Offset: 0x00105094
		public string webDeviceName
		{
			get
			{
				if ((this.m_param[244] == 255 && this.m_param[245] == 255) || this.m_param[244] == 0)
				{
					return "";
				}
				int num = 0;
				int num2 = 0;
				while (num2 < 32 && this.m_param[244 + num2] != 0)
				{
					num++;
					num2++;
				}
				return Encoding.GetEncoding("utf-8").GetString(this.m_param, 244, num);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0010611C File Offset: 0x0010511C
		public string webLanguage
		{
			get
			{
				int num = this.GetIntValue(468) + (this.GetIntValue(470) << 16);
				if (num == 8192)
				{
					return "中文[zh-CHS]";
				}
				if (num == 12288)
				{
					return "English";
				}
				if (num != 229376)
				{
					return "English";
				}
				return "Other";
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00106175 File Offset: 0x00105175
		public int webPort
		{
			get
			{
				return this.GetIntValue(464);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x00106182 File Offset: 0x00105182
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x00106190 File Offset: 0x00105190
		public int wgqr_extern
		{
			get
			{
				return (int)this.m_param[482];
			}
			set
			{
				this.m_param[482] = (byte)(value & 255);
				this.SetUpdateBits(482, 1);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x001061B2 File Offset: 0x001051B2
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x001061C0 File Offset: 0x001051C0
		public int wgqr_option
		{
			get
			{
				return (int)this.m_param[481];
			}
			set
			{
				this.m_param[481] = (byte)(value & 255);
				this.SetUpdateBits(481, 1);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x001061E2 File Offset: 0x001051E2
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x001061FC File Offset: 0x001051FC
		public int xpPassword
		{
			get
			{
				return this.GetIntValue(74) + ((int)this.m_param[163] << 16);
			}
			set
			{
				this.SetIntValue(74, value);
				this.SetUpdateBits(74, 1);
				this.m_param[163] = (byte)((value >> 16) & 255);
				this.SetUpdateBits(163, 1);
			}
		}

		// Token: 0x04001B50 RID: 6992
		public const int ALARMOUT_IF_24HOUR_PIN = 4;

		// Token: 0x04001B51 RID: 6993
		public const int ALARMOUT_IF_HELP_PIN = 5;

		// Token: 0x04001B52 RID: 6994
		public const int ALARMOUT_IF_SETBYCARD_PIN = 3;

		// Token: 0x04001B53 RID: 6995
		public const int WARN_MIN_TIMEOUT_DEFAULT = 30;

		// Token: 0x04001B54 RID: 6996
		public const int WARNBIT_ALARM = 6;

		// Token: 0x04001B55 RID: 6997
		public const int WARNBIT_DOORINVALIDOPEN = 2;

		// Token: 0x04001B56 RID: 6998
		public const int WARNBIT_DOORINVALIDREAD = 4;

		// Token: 0x04001B57 RID: 6999
		public const int WARNBIT_DOOROPENTOOLONG = 1;

		// Token: 0x04001B58 RID: 7000
		public const int WARNBIT_FIRELINK = 5;

		// Token: 0x04001B59 RID: 7001
		public const int WARNBIT_FORCE = 0;

		// Token: 0x04001B5A RID: 7002
		public const int WARNBIT_FORCE_WITHCARD = 7;

		// Token: 0x04001B5B RID: 7003
		public const int WARNBIT_FORCECLOSE = 3;

		// Token: 0x04001B5C RID: 7004
		public const int WARNOUT_IF_DOOROPENTOOLONG = 1;

		// Token: 0x04001B5D RID: 7005
		public const int WARNOUT_IF_FORCE = 0;

		// Token: 0x04001B5E RID: 7006
		public const int WARNOUT_NODELAY_IF_DOOROPENTOOLONG = 2;

		// Token: 0x04001B5F RID: 7007
		private string _pcIPAddr;

		// Token: 0x04001B60 RID: 7008
		private int m_controllerSN;

		// Token: 0x04001B61 RID: 7009
		private byte[] m_needUpdateBits;

		// Token: 0x04001B62 RID: 7010
		private byte[] m_param;

		// Token: 0x04001B63 RID: 7011
		private byte[] m_param_Special;

		// Token: 0x04001B64 RID: 7012
		private string[] m_paramDesc;
	}
}
