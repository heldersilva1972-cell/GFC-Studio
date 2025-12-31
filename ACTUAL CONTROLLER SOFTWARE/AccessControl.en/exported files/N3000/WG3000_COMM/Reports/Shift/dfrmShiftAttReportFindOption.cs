using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Reports.Shift
{
	// Token: 0x0200036F RID: 879
	public partial class dfrmShiftAttReportFindOption : frmN3000
	{
		// Token: 0x06001CC1 RID: 7361 RVA: 0x0025FAC8 File Offset: 0x0025EAC8
		public dfrmShiftAttReportFindOption()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0025FAD6 File Offset: 0x0025EAD6
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x0025FADE File Offset: 0x0025EADE
		private void btnQuery_Click(object sender, EventArgs e)
		{
			if (base.Owner != null)
			{
				(base.Owner as frmShiftAttReport).btnQuery_Click(null, null);
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x0025FAFC File Offset: 0x0025EAFC
		private void dfrmShiftAttReportFindOption_Load(object sender, EventArgs e)
		{
			this.checkedListBox1.Items.Clear();
			this.checkedListBox1.Items.Add(CommonStr.strLateness, false);
			this.checkedListBox1.Items.Add(CommonStr.strLeaveEarly, false);
			this.checkedListBox1.Items.Add(CommonStr.strAbsence, false);
			this.checkedListBox1.Items.Add(CommonStr.strSignIn, false);
			this.checkedListBox1.Items.Add(CommonStr.strNotReadCard, false);
			this.checkedListBox1.Items.Add(CommonStr.strOvertime, false);
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0025FBA4 File Offset: 0x0025EBA4
		public string getStrSql()
		{
			string text = " (1 < 0) ";
			if (this.checkedListBox1.CheckedItems.Count == 0)
			{
				return text;
			}
			string text2 = "";
			for (int i = 0; i <= this.checkedListBox1.CheckedItems.Count - 1; i++)
			{
				if (text2 == "")
				{
					text2 = text2 + " t_d_shift_AttReport.[f_OnDuty1AttDesc]= " + wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]);
				}
				else
				{
					text2 = text2 + " OR t_d_shift_AttReport.[f_OnDuty1AttDesc]= " + wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]);
				}
				text2 = string.Concat(new string[]
				{
					text2,
					" OR t_d_shift_AttReport.[f_OnDuty1CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty1AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty1CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty2AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty2CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty2AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty2CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty3AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty3CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty3AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty3CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty4AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OnDuty4CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty4AttDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i]),
					" OR t_d_shift_AttReport.[f_OffDuty4CardRecordDesc]= ",
					wgTools.PrepareStrNUnicode(this.checkedListBox1.CheckedItems[i])
				});
			}
			return " (" + text2 + " )  ";
		}
	}
}
