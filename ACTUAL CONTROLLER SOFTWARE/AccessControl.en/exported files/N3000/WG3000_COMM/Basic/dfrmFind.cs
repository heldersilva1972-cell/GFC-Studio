using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000016 RID: 22
	public partial class dfrmFind : frmN3000
	{
		// Token: 0x0600010B RID: 267 RVA: 0x000267DC File Offset: 0x000257DC
		public dfrmFind()
		{
			this.InitializeComponent();
			this.txtInfo.BackColor = wgAppConfig.GetKeyColor("KeyWindows_Backcolor2", "128, 131, 156");
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00026830 File Offset: 0x00025830
		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00026838 File Offset: 0x00025838
		private void btnFind_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.curfrm != null && (this.curfrm as Form).ActiveControl != this.curObjtofind)
				{
					this.setObjtoFind((this.curfrm as Form).ActiveControl, this.curfrm);
				}
				this.curTexttofind = this.txtFind.Text.Trim().ToUpper();
				if (!(this.curTexttofind == ""))
				{
					bool flag = false;
					if (this.prevObjtofind == this.curObjtofind && wgTools.SetObjToStr(this.curTexttofind).ToUpper() == wgTools.SetObjToStr(this.prevTexttofind).ToUpper() && sender == this.btnFind)
					{
						flag = true;
					}
					if (!flag)
					{
						this.curRow = 0;
						this.curCol = 0;
						this.cntFound = 0L;
						this.prevObjtofind = this.curObjtofind;
						this.prevTexttofind = this.curTexttofind;
						this.bFound = false;
					}
					string text = "";
					if (this.curObjtofind is DataGridView)
					{
						DataGridView dataGridView = (DataGridView)this.curObjtofind;
						int i = this.curRow;
						int j = this.curCol;
						dataGridView.ClearSelection();
						while (i < dataGridView.Rows.Count)
						{
							while (j < dataGridView.ColumnCount)
							{
								if (dataGridView.Columns[j].Visible)
								{
									if (string.IsNullOrEmpty(text))
									{
										text = dataGridView.Columns[j].HeaderText;
									}
									object value = dataGridView.Rows[i].Cells[j].Value;
									if (wgTools.SetObjToStr(value).ToUpper().IndexOf(this.curTexttofind) >= 0)
									{
										dataGridView.FirstDisplayedScrollingRowIndex = i;
										dataGridView.Rows[i].Selected = true;
										this.bFound = true;
										this.curRow = i + 1;
										this.curCol = 0;
										this.cntFound += 1L;
										if (sender == this.btnFind)
										{
											this.lblCount.Text = this.cntFound.ToString();
											this.txtInfo.Text = wgTools.SetObjToStr(value);
											return;
										}
										if (sender == this.btnMarkAll)
										{
											this.lblCount.Text = this.cntFound.ToString();
											break;
										}
									}
								}
								j++;
							}
							j = 0;
							i++;
						}
						this.curRow = 0;
						this.curCol = 0;
						this.lblCount.Text = this.cntFound.ToString();
						this.txtInfo.Text = "";
						if (this.bFound)
						{
							XMessageBox.Show(CommonStr.strFindComplete);
						}
						else
						{
							this.cntFound = 0L;
							this.lblCount.Text = this.cntFound.ToString();
							if (string.IsNullOrEmpty(text))
							{
								XMessageBox.Show(CommonStr.strNotFind);
							}
							else
							{
								XMessageBox.Show(CommonStr.strNotFindWithZone + text);
							}
						}
						this.cntFound = 0L;
						this.lblCount.Text = this.cntFound.ToString();
						this.selectTxtFind();
					}
					else if (this.curObjtofind is ComboBox)
					{
						ComboBox comboBox = (ComboBox)this.curObjtofind;
						for (int k = this.curRow; k < comboBox.Items.Count; k++)
						{
							object obj = comboBox.Items[k];
							if (wgTools.SetObjToStr(obj).ToUpper().IndexOf(this.curTexttofind) >= 0)
							{
								comboBox.SelectedItem = comboBox.Items[k];
								comboBox.SelectedIndex = k;
								this.bFound = true;
								this.curRow = k + 1;
								this.curCol = 0;
								this.cntFound += 1L;
								this.lblCount.Text = this.cntFound.ToString();
								this.txtInfo.Text = wgTools.SetObjToStr(obj);
								return;
							}
						}
						this.curRow = 0;
						this.curCol = 0;
						this.lblCount.Text = this.cntFound.ToString();
						this.txtInfo.Text = "";
						if (this.bFound)
						{
							XMessageBox.Show(CommonStr.strFindComplete);
						}
						else
						{
							this.cntFound = 0L;
							this.lblCount.Text = this.cntFound.ToString();
							XMessageBox.Show(CommonStr.strNotFind);
						}
						this.cntFound = 0L;
						this.selectTxtFind();
						this.lblCount.Text = this.cntFound.ToString();
					}
					else if (!(this.curObjtofind is CheckedListBox) && this.curObjtofind is ListBox)
					{
						ListBox listBox = (ListBox)this.curObjtofind;
						int l = this.curRow;
						listBox.ClearSelected();
						listBox.ClearSelected();
						while (l < listBox.Items.Count)
						{
							if (listBox.DisplayMember == "")
							{
								object obj2 = listBox.Items[l];
								if (wgTools.SetObjToStr(obj2).ToUpper().IndexOf(this.curTexttofind) >= 0)
								{
									listBox.SetSelected(l, true);
									this.bFound = true;
									this.curRow = l + 1;
									this.curCol = 0;
									this.cntFound += 1L;
									if (sender == this.btnFind)
									{
										this.lblCount.Text = this.cntFound.ToString();
										this.txtInfo.Text = wgTools.SetObjToStr(obj2);
										return;
									}
								}
								l++;
							}
							else
							{
								l++;
							}
						}
						this.curRow = 0;
						this.curCol = 0;
						this.lblCount.Text = this.cntFound.ToString();
						this.txtInfo.Text = "";
						if (this.bFound)
						{
							XMessageBox.Show(CommonStr.strFindComplete);
						}
						else
						{
							this.cntFound = 0L;
							this.lblCount.Text = this.cntFound.ToString();
							XMessageBox.Show(CommonStr.strNotFind);
						}
						this.cntFound = 0L;
						this.selectTxtFind();
						this.lblCount.Text = this.cntFound.ToString();
					}
					else if (this.curObjtofind is CheckedListBox)
					{
						CheckedListBox checkedListBox = (CheckedListBox)this.curObjtofind;
						int m = this.curRow;
						checkedListBox.ClearSelected();
						checkedListBox.ClearSelected();
						while (m < checkedListBox.Items.Count)
						{
							if (checkedListBox.DisplayMember == "")
							{
								object obj3 = checkedListBox.Items[m];
								if (wgTools.SetObjToStr(obj3).ToUpper().IndexOf(this.curTexttofind) >= 0)
								{
									checkedListBox.SetSelected(m, true);
									this.bFound = true;
									this.curRow = m + 1;
									this.curCol = 0;
									this.cntFound += 1L;
									if (sender == this.btnFind)
									{
										this.lblCount.Text = this.cntFound.ToString();
										this.txtInfo.Text = wgTools.SetObjToStr(obj3);
										return;
									}
									if (sender == this.btnMarkAll)
									{
										checkedListBox.SetItemChecked(m, true);
									}
								}
								m++;
							}
							else
							{
								m++;
							}
						}
						this.curRow = 0;
						this.curCol = 0;
						this.lblCount.Text = this.cntFound.ToString();
						this.txtInfo.Text = "";
						if (this.bFound)
						{
							XMessageBox.Show(CommonStr.strFindComplete);
						}
						else
						{
							this.cntFound = 0L;
							this.lblCount.Text = this.cntFound.ToString();
							XMessageBox.Show(CommonStr.strNotFind);
						}
						this.cntFound = 0L;
						this.selectTxtFind();
						this.lblCount.Text = this.cntFound.ToString();
					}
					else
					{
						if ((this.curObjtofind is UserControlFind || this.curObjtofind is UserControlFindSecond || this.curObjtofind is UserControlFind4Shift || this.curObjtofind is UserControlFindSecond4Shift) && this.optDept.Checked)
						{
							ToolStripComboBox toolStripComboBox = null;
							if (this.curObjtofind is UserControlFind)
							{
								toolStripComboBox = ((UserControlFind)this.curObjtofind).cboFindDept;
							}
							else if (this.curObjtofind is UserControlFindSecond)
							{
								toolStripComboBox = ((UserControlFindSecond)this.curObjtofind).cboFindDept;
							}
							else if (this.curObjtofind is UserControlFind4Shift)
							{
								toolStripComboBox = ((UserControlFind4Shift)this.curObjtofind).cboFindDept;
							}
							else if (this.curObjtofind is UserControlFindSecond4Shift)
							{
								toolStripComboBox = ((UserControlFindSecond4Shift)this.curObjtofind).cboFindDept;
							}
							if (toolStripComboBox != null)
							{
								for (int n = this.curRow; n < toolStripComboBox.Items.Count; n++)
								{
									object obj4 = toolStripComboBox.Items[n];
									if (wgTools.SetObjToStr(obj4).ToUpper().IndexOf(this.curTexttofind) >= 0)
									{
										toolStripComboBox.SelectedItem = toolStripComboBox.Items[n];
										toolStripComboBox.SelectedIndex = n;
										this.bFound = true;
										this.curRow = n + 1;
										this.curCol = 0;
										this.cntFound += 1L;
										this.lblCount.Text = this.cntFound.ToString();
										this.txtInfo.Text = wgTools.SetObjToStr(obj4);
										return;
									}
								}
								this.curRow = 0;
								this.curCol = 0;
								this.lblCount.Text = this.cntFound.ToString();
								this.txtInfo.Text = "";
								if (this.bFound)
								{
									XMessageBox.Show(CommonStr.strFindComplete);
								}
								else
								{
									this.cntFound = 0L;
									this.lblCount.Text = this.cntFound.ToString();
									XMessageBox.Show(CommonStr.strNotFind);
								}
								this.cntFound = 0L;
								this.selectTxtFind();
								this.lblCount.Text = this.cntFound.ToString();
								return;
							}
						}
						if ((this.curObjtofind is UserControlFind || this.curObjtofind is UserControlFindSecond || this.curObjtofind is UserControlFind4Shift || this.curObjtofind is UserControlFindSecond4Shift) && this.optName.Checked)
						{
							ComboBox comboBox2 = null;
							if (this.curObjtofind is UserControlFind)
							{
								comboBox2 = ((UserControlFind)this.curObjtofind).cboUsers;
							}
							else if (this.curObjtofind is UserControlFindSecond)
							{
								comboBox2 = ((UserControlFindSecond)this.curObjtofind).cboUsers;
							}
							else if (this.curObjtofind is UserControlFind4Shift)
							{
								comboBox2 = ((UserControlFind4Shift)this.curObjtofind).cboUsers;
							}
							else if (this.curObjtofind is UserControlFindSecond4Shift)
							{
								comboBox2 = ((UserControlFindSecond4Shift)this.curObjtofind).cboUsers;
							}
							int num = this.curRow;
							if (comboBox2 != null && comboBox2.Items.Count > 0 && comboBox2.Items[0] is DataRowView)
							{
								while (num < comboBox2.Items.Count)
								{
									object obj5 = ((DataRowView)comboBox2.Items[num])["f_consumerFull"];
									if (wgTools.SetObjToStr(obj5).ToUpper().IndexOf(this.curTexttofind) >= 0)
									{
										comboBox2.SelectedItem = comboBox2.Items[num];
										comboBox2.SelectedIndex = num;
										this.bFound = true;
										this.curRow = num + 1;
										this.curCol = 0;
										this.cntFound += 1L;
										this.lblCount.Text = this.cntFound.ToString();
										this.txtInfo.Text = wgTools.SetObjToStr(obj5);
										return;
									}
									num++;
								}
							}
							this.curRow = 0;
							this.curCol = 0;
							this.lblCount.Text = this.cntFound.ToString();
							this.txtInfo.Text = "";
							if (this.bFound)
							{
								XMessageBox.Show(CommonStr.strFindComplete);
							}
							else
							{
								this.cntFound = 0L;
								this.lblCount.Text = this.cntFound.ToString();
								XMessageBox.Show(CommonStr.strNotFind);
							}
							this.cntFound = 0L;
							this.selectTxtFind();
							this.lblCount.Text = this.cntFound.ToString();
						}
						else if (this.curObjtofind is ListView)
						{
							ListView listView = (ListView)this.curObjtofind;
							int num2 = this.curRow;
							listView.SelectedItems.Clear();
							while (num2 < listView.Items.Count)
							{
								object obj6;
								if (listView.View == View.Details)
								{
									obj6 = "";
									for (int num3 = 0; num3 < listView.Items[num2].SubItems.Count - 1; num3++)
									{
										obj6 = obj6 + "    " + listView.Items[num2].SubItems[num3].Text;
									}
								}
								else
								{
									obj6 = listView.Items[num2].Text;
								}
								if (wgTools.SetObjToStr(obj6).ToUpper().IndexOf(this.curTexttofind) >= 0)
								{
									listView.Items[num2].Selected = true;
									listView.Items[num2].EnsureVisible();
									listView.Focus();
									this.bFound = true;
									this.curRow = num2 + 1;
									this.curCol = 0;
									this.cntFound += 1L;
									if (sender == this.btnFind)
									{
										this.lblCount.Text = this.cntFound.ToString();
										this.txtInfo.Text = wgTools.SetObjToStr(obj6);
										return;
									}
								}
								num2++;
							}
							this.curRow = 0;
							this.curCol = 0;
							this.lblCount.Text = this.cntFound.ToString();
							this.txtInfo.Text = "";
							if (this.bFound)
							{
								XMessageBox.Show(CommonStr.strFindComplete);
							}
							else
							{
								this.cntFound = 0L;
								this.lblCount.Text = this.cntFound.ToString();
								XMessageBox.Show(CommonStr.strNotFind);
							}
							this.cntFound = 0L;
							this.lblCount.Text = this.cntFound.ToString();
							this.selectTxtFind();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000276F4 File Offset: 0x000266F4
		private void dfrmFind_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.bClose)
			{
				base.Hide();
				e.Cancel = true;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0002770B File Offset: 0x0002670B
		private void dfrmFind_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00027710 File Offset: 0x00026710
		private TreeNode FindNode(TreeNode tnParent, string strValue)
		{
			if (tnParent == null)
			{
				return null;
			}
			if (tnParent.Text == strValue)
			{
				return tnParent;
			}
			if (tnParent.Nodes.Count == 0)
			{
				return null;
			}
			TreeNode treeNode = tnParent;
			TreeNode treeNode2 = treeNode.FirstNode;
			IL_008E:
			while (treeNode2 != null && treeNode2 != tnParent)
			{
				while (treeNode2 != null)
				{
					if (treeNode2.Text == strValue)
					{
						return treeNode2;
					}
					if (treeNode2.Nodes.Count > 0)
					{
						treeNode = treeNode2;
						treeNode2 = treeNode2.FirstNode;
					}
					else if (treeNode2 != treeNode.LastNode)
					{
						treeNode2 = treeNode2.NextNode;
					}
					else
					{
						IL_0076:
						while (treeNode2 != tnParent && treeNode2 == treeNode.LastNode)
						{
							treeNode2 = treeNode;
							treeNode = treeNode.Parent;
						}
						if (treeNode2 != tnParent)
						{
							treeNode2 = treeNode2.NextNode;
							goto IL_008E;
						}
						goto IL_008E;
					}
				}
				goto IL_0076;
			}
			return null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000277B3 File Offset: 0x000267B3
		public void ReallyCloseForm()
		{
			this.bClose = true;
			base.Close();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000277C4 File Offset: 0x000267C4
		private void selectTxtFind()
		{
			try
			{
				base.ActiveControl = this.btnFind;
				if (this.txtFind.Text.Length > 0)
				{
					this.txtFind.SelectionStart = 0;
					this.txtFind.SelectionLength = this.txtFind.Text.Length;
				}
				base.ActiveControl = this.txtFind;
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00027844 File Offset: 0x00026844
		public void setObjtoFind(object obj, object frm)
		{
			try
			{
				bool flag = false;
				object obj2 = this.curObjtofind;
				if (this.curObjtofind != null)
				{
					flag = true;
				}
				this.curObjtofind = obj;
				if (this.curObjtofind is TabControl || this.curObjtofind is TabPage)
				{
					TabPage tabPage;
					if (this.curObjtofind is TabControl)
					{
						tabPage = (this.curObjtofind as TabControl).SelectedTab;
					}
					else
					{
						tabPage = this.curObjtofind as TabPage;
					}
					bool flag2 = false;
					if (!flag2)
					{
						foreach (object obj3 in tabPage.Controls)
						{
							Control control = (Control)obj3;
							if (control is DataGridView)
							{
								this.curObjtofind = control;
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						foreach (object obj4 in tabPage.Controls)
						{
							Control control2 = (Control)obj4;
							if (control2 is ListBox || control2 is ComboBox || control2 is ListView || control2 is CheckedListBox)
							{
								this.curObjtofind = control2;
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						this.curObjtofind = null;
					}
				}
				if (this.curObjtofind is DataGridView)
				{
					this.btnMarkAll.Visible = true;
					this.btnMarkAll.Enabled = true;
					flag = true;
				}
				else if (this.curObjtofind is ListBox)
				{
					this.btnMarkAll.Visible = true;
					this.btnMarkAll.Enabled = true;
					flag = true;
				}
				else if (this.curObjtofind is ComboBox)
				{
					this.btnMarkAll.Visible = false;
					this.btnMarkAll.Enabled = false;
					flag = true;
				}
				else if (this.curObjtofind is ListView || this.curObjtofind is TextBox || this.curObjtofind is MaskedTextBox)
				{
					this.btnMarkAll.Visible = false;
					this.btnMarkAll.Enabled = false;
					flag = true;
				}
				else if (this.curObjtofind is CheckedListBox)
				{
					this.btnMarkAll.Visible = false;
					this.btnMarkAll.Enabled = false;
					flag = true;
				}
				else if (this.curObjtofind is UserControlFind || this.curObjtofind is UserControlFindSecond || this.curObjtofind is UserControlFind4Shift || this.curObjtofind is UserControlFindSecond4Shift)
				{
					this.btnMarkAll.Visible = false;
					this.btnMarkAll.Enabled = false;
					flag = true;
				}
				else
				{
					this.curObjtofind = obj2;
				}
				if (this.curObjtofind is UserControlFind || this.curObjtofind is UserControlFindSecond || this.curObjtofind is UserControlFind4Shift || this.curObjtofind is UserControlFindSecond4Shift)
				{
					this.optDept.Visible = true;
					this.optName.Visible = true;
				}
				else
				{
					this.optDept.Visible = false;
					this.optName.Visible = false;
				}
				this.cntFound = 0L;
				this.lblCount.Text = this.cntFound.ToString();
				this.txtInfo.Text = "";
				this.selectTxtFind();
				if (flag)
				{
					this.curfrm = frm;
					base.Show();
					base.Focus();
				}
				else
				{
					base.Hide();
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00027BE8 File Offset: 0x00026BE8
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			if (this.inputCard.Length >= 8)
			{
				try
				{
					long num;
					if (long.TryParse(this.inputCard, out num))
					{
						this.inputCard = "";
						this.txtFind.Text = num.ToString();
						if (this.btnFind.Enabled)
						{
							this.btnFind.PerformClick();
						}
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			this.inputCard = "";
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00027C84 File Offset: 0x00026C84
		private void txtFind_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13 && this.btnFind.Enabled)
			{
				this.btnFind.PerformClick();
			}
			if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
			{
				if (this.inputCard.Length == 0)
				{
					this.timer1.Interval = 500;
					this.timer1.Enabled = true;
				}
				this.inputCard += (e.KeyValue - 48).ToString();
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00027D2A File Offset: 0x00026D2A
		private void txtFind_TextChanged(object sender, EventArgs e)
		{
			if (this.txtFind.Text.Length == 0)
			{
				this.btnFind.Enabled = false;
				return;
			}
			this.btnFind.Enabled = true;
		}

		// Token: 0x040002BD RID: 701
		private bool bFound;

		// Token: 0x040002BE RID: 702
		private long cntFound;

		// Token: 0x040002BF RID: 703
		private int curCol;

		// Token: 0x040002C0 RID: 704
		private object curfrm;

		// Token: 0x040002C1 RID: 705
		private object curObjtofind;

		// Token: 0x040002C2 RID: 706
		private int curRow;

		// Token: 0x040002C3 RID: 707
		private object prevObjtofind;

		// Token: 0x040002C4 RID: 708
		public bool bClose;

		// Token: 0x040002C5 RID: 709
		private string curTexttofind = "";

		// Token: 0x040002C6 RID: 710
		private string inputCard = "";

		// Token: 0x040002C7 RID: 711
		private string prevTexttofind = "";
	}
}
