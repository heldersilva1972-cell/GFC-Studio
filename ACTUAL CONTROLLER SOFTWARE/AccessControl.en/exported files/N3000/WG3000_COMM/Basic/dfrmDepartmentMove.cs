using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000010 RID: 16
	public partial class dfrmDepartmentMove : frmN3000
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0001DF48 File Offset: 0x0001CF48
		public dfrmDepartmentMove()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0001DFA3 File Offset: 0x0001CFA3
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0001DFB4 File Offset: 0x0001CFB4
		private void btnOK_Click(object sender, EventArgs e)
		{
			icGroup icGroup = new icGroup();
			string text;
			if (string.IsNullOrEmpty(this.cbof_GroupNew.Text))
			{
				if (this.selectedGroupName.LastIndexOf("\\") < 0)
				{
					text = this.selectedGroupName;
				}
				else
				{
					text = this.selectedGroupName.Substring(this.selectedGroupName.LastIndexOf("\\") + 1);
				}
			}
			else if (this.selectedGroupName.LastIndexOf("\\") < 0)
			{
				text = this.cbof_GroupNew.Text + "\\" + this.selectedGroupName;
			}
			else
			{
				text = this.cbof_GroupNew.Text + "\\" + this.selectedGroupName.Substring(this.selectedGroupName.LastIndexOf("\\") + 1);
			}
			if (icGroup.checkExisted(text))
			{
				XMessageBox.Show(this, text + "\r\n\r\n" + CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.cbof_GroupNew.Text.IndexOf(this.selectedGroupName + "\\") == 0)
			{
				XMessageBox.Show(this, CommonStr.strMoveDepartmentFailedA, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			icGroup.Update(this.selectedGroupName, text);
			icGroup.updateGroupNO();
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0001E104 File Offset: 0x0001D104
		private void dfrmDepartmentMove_Load(object sender, EventArgs e)
		{
			try
			{
				new icGroup().getGroup(ref this.arrGroupNameWithSpace, ref this.arrGroupID, ref this.arrGroupNO);
				int i = this.arrGroupID.Count;
				for (i = 0; i < this.arrGroupID.Count; i++)
				{
					if (i == 0 && string.IsNullOrEmpty(this.arrGroupNameWithSpace[i].ToString()))
					{
						this.arrGroupName.Add(CommonStr.strAll);
					}
					else
					{
						this.arrGroupName.Add(this.arrGroupNameWithSpace[i].ToString());
					}
					this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
					this.cbof_GroupNew.Items.Add(this.arrGroupNameWithSpace[i].ToString());
				}
				if ((int)this.arrGroupID[0] == 0)
				{
					this.cbof_GroupID.Items.Insert(1, wgAppConfig.ReplaceFloorRoom(CommonStr.strDepartmentIsEmpty));
				}
				if (this.cbof_GroupID.Items.Count > 0)
				{
					this.cbof_GroupID.SelectedIndex = 0;
					for (i = 0; i < this.cbof_GroupID.Items.Count; i++)
					{
						if ((string)this.cbof_GroupID.Items[i] == this.selectedGroupName)
						{
							this.cbof_GroupID.SelectedIndex = i;
							break;
						}
					}
				}
				if (this.cbof_GroupNew.Items.Count > 0)
				{
					this.cbof_GroupNew.SelectedIndex = 0;
				}
				this.Text = wgAppConfig.ReplaceFloorRoom(this.Text);
				this.label1.Text = wgAppConfig.ReplaceFloorRoom(this.label1.Text);
				this.Label3.Text = wgAppConfig.ReplaceFloorRoom(this.Label3.Text);
				this.Label5.Text = wgAppConfig.ReplaceFloorRoom(this.Label5.Text);
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0001E32C File Offset: 0x0001D32C
		private int getDeptId(string deptName)
		{
			icGroup icGroup = new icGroup();
			int num = icGroup.getGroupID(deptName);
			if (num > 0)
			{
				return num;
			}
			string text = deptName;
			while (text.IndexOf("\\\\") >= 0)
			{
				text = text.Replace("\\\\", "\\");
			}
			if (text.Substring(0, 1) == "\\")
			{
				text = text.Substring(1);
			}
			if (text.Substring(text.Length - 1) == "\\")
			{
				text = text.Substring(0, text.Length - 1);
			}
			num = icGroup.getGroupID(text);
			if (num > 0)
			{
				return num;
			}
			string[] array = text.Split(new char[] { '\\' });
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (text2 == "")
				{
					text2 = array[i];
				}
				else
				{
					text2 = text2 + "\\" + array[i];
				}
				if (flag || !icGroup.checkExisted(text2))
				{
					flag = true;
					icGroup.addNew4BatchExcel(text2);
				}
			}
			return icGroup.getGroupID(text);
		}

		// Token: 0x040001E3 RID: 483
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040001E4 RID: 484
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040001E5 RID: 485
		private ArrayList arrGroupNameWithSpace = new ArrayList();

		// Token: 0x040001E6 RID: 486
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040001E7 RID: 487
		public string selectedGroupName = "";

		// Token: 0x040001E8 RID: 488
		public string strSqlSelected = "";
	}
}
