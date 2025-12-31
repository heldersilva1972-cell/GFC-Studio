using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Patrol
{
	// Token: 0x02000307 RID: 775
	public partial class dfrmPatrolReportFindOption : frmN3000
	{
		// Token: 0x06001747 RID: 5959 RVA: 0x001E4118 File Offset: 0x001E3118
		public dfrmPatrolReportFindOption()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x001E413D File Offset: 0x001E313D
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x001E4145 File Offset: 0x001E3145
		private void btnQuery_Click(object sender, EventArgs e)
		{
			if (base.Owner != null)
			{
				(base.Owner as frmPatrolReport).btnQuery_Click(null, null);
			}
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x001E4164 File Offset: 0x001E3164
		private void dfrmShiftAttReportFindOption_Load(object sender, EventArgs e)
		{
			this.checkedListBox1.Items.Clear();
			this.checkedListBox1.Items.Add(CommonStr.strPatrolEventAbsence, false);
			this.checkedListBox1.Items.Add(CommonStr.strPatrolEventEarly, false);
			this.checkedListBox1.Items.Add(CommonStr.strPatrolEventLate, false);
			this.checkedListBox1.Items.Add(CommonStr.strPatrolEventNormal, false);
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x001E41E0 File Offset: 0x001E31E0
		public string getStrSql()
		{
			string text = " (1 > 0) ";
			if (this.checkedListBox1.CheckedItems.Count != 0 && this.checkedListBox1.CheckedItems.Count != this.checkedListBox1.Items.Count)
			{
				string text2 = "";
				for (int i = 0; i <= this.checkedListBox1.CheckedItems.Count - 1; i++)
				{
					if (text2 == "")
					{
						text2 = text2 + "  t_d_PatrolDetailData.f_EventDesc= " + this.Event[this.checkedListBox1.CheckedIndices[i]];
					}
					else
					{
						text2 = text2 + " OR  t_d_PatrolDetailData.f_EventDesc= " + this.Event[this.checkedListBox1.CheckedIndices[i]];
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = " (" + text2 + " )  ";
				}
			}
			return text;
		}

		// Token: 0x04003006 RID: 12294
		private int[] Event = new int[] { 4, 2, 3, 1 };
	}
}
