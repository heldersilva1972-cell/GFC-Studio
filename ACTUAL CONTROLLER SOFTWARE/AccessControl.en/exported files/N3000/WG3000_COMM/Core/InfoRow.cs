using System;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Core
{
	// Token: 0x020001D3 RID: 467
	public class InfoRow
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x000E4FA0 File Offset: 0x000E3FA0
		public static object getImage(string stringValue, ref DataGridViewRow dgvr)
		{
			if (InfoRow.styleRed == null)
			{
				InfoRow.loadStyle();
			}
			switch (stringValue)
			{
			case "0":
			case "2":
			{
				object obj = Resources.Rec1Pass;
				dgvr.DefaultCellStyle = InfoRow.styleGreen;
				return obj;
			}
			case "1":
			case "3":
			{
				object obj = Resources.Rec2NoPass;
				dgvr.DefaultCellStyle = InfoRow.styleOrange;
				return obj;
			}
			case "4":
			{
				object obj = Resources.Rec3Warn;
				dgvr.DefaultCellStyle = InfoRow.styleYellow;
				return obj;
			}
			case "6":
			{
				object obj;
				if (!InfoRow.IsGlobalAntiBackOpen)
				{
					obj = Resources.Rec3Warn;
					dgvr.DefaultCellStyle = InfoRow.styleYellow;
					return obj;
				}
				obj = Resources.Rec1Pass;
				dgvr.DefaultCellStyle = InfoRow.styleGreen;
				return obj;
			}
			case "5":
			{
				object obj = Resources.Rec4Falt;
				dgvr.DefaultCellStyle = InfoRow.styleRed;
				return obj;
			}
			case "101":
				return Resources.Rec4Falt;
			case "501":
			{
				object obj = Resources.Rec3Warn;
				dgvr.DefaultCellStyle = InfoRow.styleYellow;
				return obj;
			}
			}
			return Resources.eventlogInfo;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x000E5128 File Offset: 0x000E4128
		private static void loadStyle()
		{
			InfoRow.styleRed = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.Red,
				Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 134),
				ForeColor = Color.White,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				WrapMode = DataGridViewTriState.False
			};
			InfoRow.styleGreen = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.Green,
				Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 134),
				ForeColor = Color.White,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				WrapMode = DataGridViewTriState.False
			};
			InfoRow.styleYellow = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.Yellow,
				Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 134),
				ForeColor = Color.Blue,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				WrapMode = DataGridViewTriState.False
			};
			InfoRow.styleOrange = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft,
				BackColor = Color.Orange,
				Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 134),
				ForeColor = Color.Blue,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				WrapMode = DataGridViewTriState.False
			};
			InfoRow.IsGlobalAntiBackOpen = wgAppConfig.getParamValBoolByNO(181);
		}

		// Token: 0x040018F3 RID: 6387
		public int category = 100;

		// Token: 0x040018F4 RID: 6388
		public string desc = "";

		// Token: 0x040018F5 RID: 6389
		public string detail = "";

		// Token: 0x040018F6 RID: 6390
		public string information = "";

		// Token: 0x040018F7 RID: 6391
		private static bool IsGlobalAntiBackOpen;

		// Token: 0x040018F8 RID: 6392
		public string MjRecStr = "";

		// Token: 0x040018F9 RID: 6393
		private static DataGridViewCellStyle styleGreen = new DataGridViewCellStyle();

		// Token: 0x040018FA RID: 6394
		private static DataGridViewCellStyle styleOrange = new DataGridViewCellStyle();

		// Token: 0x040018FB RID: 6395
		private static DataGridViewCellStyle styleRed = null;

		// Token: 0x040018FC RID: 6396
		private static DataGridViewCellStyle styleYellow = new DataGridViewCellStyle();
	}
}
