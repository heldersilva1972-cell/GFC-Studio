using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Core
{
	// Token: 0x020001E5 RID: 485
	public class UserControlFind : UserControl
	{
		// Token: 0x06000B3F RID: 2879 RVA: 0x000EAF00 File Offset: 0x000E9F00
		public UserControlFind()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x000EAF68 File Offset: 0x000E9F68
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
			wgTools.WriteLine("DoWork Starting ...");
			if (UserControlFind.lastLoadUsers == icConsumerShare.getUpdateLog() && UserControlFind.dvLastLoad != null)
			{
				Thread.Sleep(100);
				UserControlFind.dvLastLoad.RowFilter = "";
				e.Result = UserControlFind.dvLastLoad;
				return;
			}
			UserControlFind.lastLoadUsers = icConsumerShare.getUpdateLog();
			int num = (int)((object[])e.Argument)[0];
			int num2 = (int)((object[])e.Argument)[1];
			string text = (string)((object[])e.Argument)[2];
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			e.Result = this.loadUserData4BackWork(num, num2, text);
			if (backgroundWorker.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000EB038 File Offset: 0x000EA038
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				wgTools.WgDebugWrite("Users Operation was canceled", new object[0]);
				return;
			}
			if (e.Error != null)
			{
				wgTools.WgDebugWrite(string.Format("An error occurred: {0}", e.Error.Message), new object[0]);
				return;
			}
			this.loadUserData4BackWorkComplete(e.Result as DataView);
			wgTools.WriteLine("backgroundWorker1_RunWorkerCompleted");
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000EB0A4 File Offset: 0x000EA0A4
		private void btnClear_Click(object sender, EventArgs e)
		{
			this.txtFindCardID.Text = "";
			this.txtFindName.Text = "";
			this.cboUsers.Text = "";
			if (this.cboFindDept.Items.Count > 0)
			{
				this.cboFindDept.SelectedIndex = 0;
			}
			this.cboUsers.Text = "";
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000EB110 File Offset: 0x000EA110
		private void btnQuery_Click(object sender, EventArgs e)
		{
			this.SelectedConsumerID = 0;
			if (string.IsNullOrEmpty(this.cboUsers.Text))
			{
				this.txtFindName.Text = "";
				return;
			}
			if (this.cboUsers.SelectedIndex < 0)
			{
				this.txtFindName.Text = this.cboUsers.Text;
				return;
			}
			this.txtFindName.Text = ((DataRowView)this.cboUsers.SelectedItem).Row["f_ConsumerName"].ToString();
			this.SelectedConsumerID = (int)((DataRowView)this.cboUsers.SelectedItem).Row["f_ConsumerID"];
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000EB1C5 File Offset: 0x000EA1C5
		private void cboFindDept_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cboFindDept, this.cboUsers);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000EB1D8 File Offset: 0x000EA1D8
		private void cboFindDept_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.cboFindDept.ToolTipText = this.cboFindDept.Text;
				if (this.cboUsers.DataSource != null)
				{
					DataView dataView = (DataView)this.cboUsers.DataSource;
					if (this.cboFindDept.SelectedIndex < 0)
					{
						dataView.RowFilter = "";
						this.strGroupFilter = "";
					}
					if (this.cboFindDept.SelectedIndex == 0 && this.cboFindDept.Text == "")
					{
						dataView.RowFilter = "";
						this.strGroupFilter = "";
					}
					else
					{
						this.strGroupFilter = " f_GroupID =" + this.arrGroupID[this.cboFindDept.SelectedIndex];
						int num = (int)this.arrGroupID[this.cboFindDept.SelectedIndex];
						int num2 = (int)this.arrGroupNO[this.cboFindDept.SelectedIndex];
						int groupChildMaxNo = icGroup.getGroupChildMaxNo(this.cboFindDept.Text, this.arrGroupName, this.arrGroupNO);
						if (num2 > 0)
						{
							if (num2 >= groupChildMaxNo)
							{
								this.strGroupFilter = string.Format(" f_GroupID ={0:d} ", num);
							}
							else
							{
								string groupQuery = icGroup.getGroupQuery(num2, groupChildMaxNo, this.arrGroupNO, this.arrGroupID);
								this.strGroupFilter = string.Format("  {0} ", groupQuery);
							}
						}
						dataView.RowFilter = string.Format("{0}", this.strGroupFilter);
					}
					this.cboUsers.Text = "";
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000EB3A4 File Offset: 0x000EA3A4
		private void cboUsers_DropDown(object sender, EventArgs e)
		{
			Graphics graphics = null;
			try
			{
				int num = this.cboUsers.Width;
				graphics = this.cboUsers.CreateGraphics();
				Font font = this.cboUsers.Font;
				int num2 = ((this.cboUsers.Items.Count > this.cboUsers.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0);
				foreach (object obj in this.cboUsers.Items)
				{
					if (obj != null)
					{
						int num3 = (int)graphics.MeasureString(((DataRowView)obj).Row["f_ConsumerFull"].ToString(), font).Width + num2;
						if (num < num3)
						{
							num = num3;
						}
					}
				}
				this.cboUsers.DropDownWidth = num;
			}
			catch
			{
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000EB4C0 File Offset: 0x000EA4C0
		private void cboUsers_DropDownClosed(object sender, EventArgs e)
		{
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000EB4C2 File Offset: 0x000EA4C2
		private void cboUsers_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000EB4C4 File Offset: 0x000EA4C4
		private void cboUsers_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13)
			{
				this.btnQuery.PerformClick();
				return;
			}
			if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
			{
				if (this.inputCard.Length == 0)
				{
					this.timer2.Interval = 500;
					this.timer2.Enabled = true;
				}
				this.inputCard += (e.KeyValue - 48).ToString();
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000EB560 File Offset: 0x000EA560
		public void getSqlInfo(ref int groupMinNO, ref int groupIDOfMinNO, ref int groupMaxNO, ref string findName, ref long findCard, ref int findConsumerID)
		{
			try
			{
				this.btnQuery_Click(null, null);
				findConsumerID = this.SelectedConsumerID;
				if (this.cboFindDept.SelectedIndex < 0 || (this.cboFindDept.SelectedIndex == 0 && (int)this.arrGroupID[0] == 0))
				{
					groupMinNO = 0;
					groupMaxNO = 0;
				}
				else
				{
					groupIDOfMinNO = (int)this.arrGroupID[this.cboFindDept.SelectedIndex];
					groupMinNO = (int)this.arrGroupNO[this.cboFindDept.SelectedIndex];
					groupMaxNO = icGroup.getGroupChildMaxNo(this.cboFindDept.Text, this.arrGroupName, this.arrGroupNO);
				}
				if (string.IsNullOrEmpty(this.txtFindName.Text))
				{
					findName = "";
				}
				else
				{
					findName = this.txtFindName.Text.Trim();
				}
				findCard = 0L;
				if (long.TryParse(this.txtFindCardID.Text, out findCard) && findCard < 0L)
				{
					findCard = 0L;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000EB680 File Offset: 0x000EA680
		private DataView loadUserData4BackWork(int startIndex, int maxRecords, string strSql)
		{
			wgTools.WriteLine("loadUserData Start");
			if (strSql.ToUpper().IndexOf("SELECT ") > 0)
			{
				strSql = string.Format("SELECT TOP {0:d} ", maxRecords) + strSql.Substring(strSql.ToUpper().IndexOf("SELECT ") + "SELECT ".Length);
			}
			if (startIndex == 0)
			{
				this.recNOMax = "";
			}
			else if (strSql.ToUpper().IndexOf(" WHERE ") > 0)
			{
				strSql += string.Format(" AND f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			else
			{
				strSql += string.Format(" WHERE f_ConsumerNO > {0}", wgTools.PrepareStrNUnicode(this.recNOMax));
			}
			strSql += " ORDER BY f_ConsumerNO ";
			this.tb = new DataTable("users");
			this.dv = new DataView(this.tb);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(strSql, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(this.tb);
							if (this.tb.Rows.Count > 0)
							{
								this.recNOMax = this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerNO"].ToString();
							}
							wgTools.WriteLine("loadUserData End");
							return this.dv;
						}
					}
				}
			}
			DataView dataView;
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(strSql, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(this.tb);
						if (this.tb.Rows.Count > 0)
						{
							this.recNOMax = this.tb.Rows[this.tb.Rows.Count - 1]["f_ConsumerNO"].ToString();
						}
						wgTools.WriteLine("loadUserData End");
						dataView = this.dv;
					}
				}
			}
			return dataView;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x000EB924 File Offset: 0x000EA924
		private void loadUserData4BackWorkComplete(DataView dv)
		{
			if (this.cboUsers.DataSource == null)
			{
				this.cboUsers.BeginUpdate();
				this.cboUsers.DisplayMember = "f_ConsumerFull";
				this.cboUsers.ValueMember = "f_ConsumerID";
				this.cboUsers.DataSource = dv;
				UserControlFind.dvLastLoad = dv;
				this.cboUsers.EndUpdate();
				this.cboUsers.Text = "";
				this.cboFindDept_SelectedIndexChanged(null, null);
				return;
			}
			if (dv.Count > 0)
			{
				DataView dataView = this.cboUsers.DataSource as DataView;
				dataView.Table.Merge(dv.Table);
				if (dv.Count >= this.MaxRecord)
				{
					this.startRecordIndex += this.MaxRecord;
					this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000EBA24 File Offset: 0x000EAA24
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			this.startRecordIndex = 0;
			string text;
			if (wgTools.gbHideCardNO)
			{
				this.toolStripLabel2.Visible = false;
				this.txtFindCardID.Visible = false;
				if (wgAppConfig.IsAccessDB)
				{
					text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID,  LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + IIF(ISNULL( f_CardNO),'-','-')  As f_ConsumerFull ";
					text += " FROM t_b_Consumer ";
				}
				else
				{
					text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID,  LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + CASE WHEN f_CardNO IS NULL THEN '-' ELSE '-' END  As f_ConsumerFull ";
					text += " FROM t_b_Consumer ";
				}
			}
			else if (wgAppConfig.IsAccessDB)
			{
				text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID,  LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + IIF(ISNULL( f_CardNO),'-',CSTR(f_CardNO))  As f_ConsumerFull ";
				text += " FROM t_b_Consumer ";
			}
			else
			{
				text = " SELECT f_ConsumerID, f_ConsumerNO, f_ConsumerName, f_CardNO, 0 as f_Selected, f_GroupID,  LTRIM(f_ConsumerNO) + ')-' +  f_ConsumerName + '-' + CASE WHEN f_CardNO IS NULL THEN '-' ELSE CONVERT(nvarchar(50),f_CardNO) END  As f_ConsumerFull ";
				text += " FROM t_b_Consumer ";
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.dgvSql = text;
			}
			this.backgroundWorker1.RunWorkerAsync(new object[] { this.startRecordIndex, this.MaxRecord, this.dgvSql });
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x000EBB0C File Offset: 0x000EAB0C
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timer2.Enabled = false;
			if (this.inputCard.Length >= 8)
			{
				try
				{
					long num;
					if (long.TryParse(this.inputCard, out num))
					{
						this.inputCard = "";
						this.btnClear_Click(null, null);
						this.txtFindCardID.Text = num.ToString();
						this.btnQuery.PerformClick();
					}
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[0]);
				}
			}
			this.inputCard = "";
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000EBBA4 File Offset: 0x000EABA4
		private void txtFindCardID_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				this.btnQuery.PerformClick();
				return;
			}
			if (e.KeyChar == '\u0016')
			{
				if (this.txtFindCardID.Text.Length == 0)
				{
					return;
				}
				if (this.txtFindCardID.Text.Length == this.txtFindCardID.SelectedText.Length)
				{
					return;
				}
			}
			if (this.txtFindCardID.Text.Length == 0)
			{
				if (e.KeyChar == '\u0016')
				{
					return;
				}
			}
			else
			{
				if (e.KeyChar == '\u0003')
				{
					return;
				}
				if (e.KeyChar == '\u0018')
				{
					return;
				}
			}
			if (e.KeyChar != '\b')
			{
				int num;
				if (int.TryParse(e.KeyChar.ToString(), out num))
				{
					if (this.inputCard.Length == 0)
					{
						this.timer2.Interval = 500;
						this.timer2.Enabled = true;
					}
					this.inputCard += num.ToString();
					if (this.txtFindCardID.Text.Length > 19)
					{
						e.Handled = true;
						return;
					}
					if (this.txtFindCardID.Text.Length == 19 && this.txtFindCardID.SelectionLength == 0)
					{
						e.Handled = true;
						return;
					}
				}
				else
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x000EBCEC File Offset: 0x000EACEC
		private void UserControlFind_Load(object sender, EventArgs e)
		{
			if (UserControlFind.blogin)
			{
				try
				{
					this.cboFindDept.Items.Clear();
					new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
					int i = this.arrGroupID.Count;
					for (i = 0; i < this.arrGroupID.Count; i++)
					{
						this.cboFindDept.Items.Add(this.arrGroupName[i].ToString());
					}
					if (this.cboFindDept.Items.Count > 0)
					{
						this.cboFindDept.SelectedIndex = 0;
					}
					this.toolStripLabel3.Text = wgAppConfig.ReplaceFloorRoom(this.toolStripLabel3.Text);
					this.toolStripLabel4Namebk.Text = this.toolStripLabel1.Text;
					this.cboUsers.Location = new Point(this.toolStripLabel4Namebk.Size.Width + 10, 8);
					int num = this.cboUsers.Location.X + this.cboUsers.Size.Width - this.toolStripLabel1.Size.Width;
					this.cboUsers.Size = new Size(this.cboUsers.Size.Width - num + 10, this.cboUsers.Size.Height);
					this.toolStripLabel4Namebk.Visible = false;
					this.timer1.Enabled = true;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x000EBEAC File Offset: 0x000EAEAC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000EBECC File Offset: 0x000EAECC
		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserControlFind));
			this.toolFindUsers = new ToolStrip();
			this.toolStripLabel1 = new ToolStripLabel();
			this.txtFindName = new ToolStripTextBox();
			this.toolStripLabel2 = new ToolStripLabel();
			this.txtFindCardID = new ToolStripTextBox();
			this.toolStripLabel3 = new ToolStripLabel();
			this.cboFindDept = new ToolStripComboBox();
			this.btnQuery = new ToolStripButton();
			this.btnClear = new ToolStripButton();
			this.toolStripLabel4Namebk = new ToolStripLabel();
			this.backgroundWorker1 = new BackgroundWorker();
			this.cboUsers = new ComboBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.toolFindUsers.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.toolFindUsers, "toolFindUsers");
			this.toolFindUsers.BackgroundImage = Resources.pTools_third_title;
			this.toolFindUsers.Items.AddRange(new ToolStripItem[] { this.toolStripLabel1, this.txtFindName, this.toolStripLabel2, this.txtFindCardID, this.toolStripLabel3, this.cboFindDept, this.btnQuery, this.btnClear, this.toolStripLabel4Namebk });
			this.toolFindUsers.Name = "toolFindUsers";
			componentResourceManager.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
			this.toolStripLabel1.ForeColor = Color.White;
			this.toolStripLabel1.Name = "toolStripLabel1";
			componentResourceManager.ApplyResources(this.txtFindName, "txtFindName");
			this.txtFindName.BorderStyle = BorderStyle.FixedSingle;
			this.txtFindName.Name = "txtFindName";
			componentResourceManager.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
			this.toolStripLabel2.ForeColor = Color.White;
			this.toolStripLabel2.Name = "toolStripLabel2";
			componentResourceManager.ApplyResources(this.txtFindCardID, "txtFindCardID");
			this.txtFindCardID.Name = "txtFindCardID";
			this.txtFindCardID.KeyPress += this.txtFindCardID_KeyPress;
			componentResourceManager.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
			this.toolStripLabel3.ForeColor = Color.White;
			this.toolStripLabel3.Name = "toolStripLabel3";
			componentResourceManager.ApplyResources(this.cboFindDept, "cboFindDept");
			this.cboFindDept.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboFindDept.Name = "cboFindDept";
			this.cboFindDept.DropDown += this.cboFindDept_DropDown;
			this.cboFindDept.SelectedIndexChanged += this.cboFindDept_SelectedIndexChanged;
			componentResourceManager.ApplyResources(this.btnQuery, "btnQuery");
			this.btnQuery.ForeColor = Color.White;
			this.btnQuery.Image = Resources.pTools_Query;
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Click += this.btnQuery_Click;
			componentResourceManager.ApplyResources(this.btnClear, "btnClear");
			this.btnClear.ForeColor = Color.White;
			this.btnClear.Image = Resources.pTools_Clear_Condition;
			this.btnClear.Name = "btnClear";
			this.btnClear.Click += this.btnClear_Click;
			componentResourceManager.ApplyResources(this.toolStripLabel4Namebk, "toolStripLabel4Namebk");
			this.toolStripLabel4Namebk.ForeColor = Color.White;
			this.toolStripLabel4Namebk.Name = "toolStripLabel4Namebk";
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += this.backgroundWorker1_DoWork;
			this.backgroundWorker1.RunWorkerCompleted += this.backgroundWorker1_RunWorkerCompleted;
			componentResourceManager.ApplyResources(this.cboUsers, "cboUsers");
			this.cboUsers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.cboUsers.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.cboUsers.DropDownWidth = 200;
			this.cboUsers.FormattingEnabled = true;
			this.cboUsers.Name = "cboUsers";
			this.cboUsers.DropDown += this.cboUsers_DropDown;
			this.cboUsers.DropDownClosed += this.cboUsers_DropDownClosed;
			this.cboUsers.KeyUp += this.cboUsers_KeyUp;
			this.timer1.Tick += this.timer1_Tick;
			this.timer2.Tick += this.timer2_Tick;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.Transparent;
			this.BackgroundImage = Resources.pTools_third_title;
			base.Controls.Add(this.cboUsers);
			base.Controls.Add(this.toolFindUsers);
			this.DoubleBuffered = true;
			base.Name = "UserControlFind";
			base.Load += this.UserControlFind_Load;
			this.toolFindUsers.ResumeLayout(false);
			this.toolFindUsers.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040019FF RID: 6655
		private string dgvSql;

		// Token: 0x04001A00 RID: 6656
		private DataView dv;

		// Token: 0x04001A01 RID: 6657
		private int SelectedConsumerID;

		// Token: 0x04001A02 RID: 6658
		private int startRecordIndex;

		// Token: 0x04001A03 RID: 6659
		private DataTable tb;

		// Token: 0x04001A04 RID: 6660
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x04001A05 RID: 6661
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x04001A06 RID: 6662
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x04001A07 RID: 6663
		public static bool blogin = false;

		// Token: 0x04001A08 RID: 6664
		private static DataView dvLastLoad;

		// Token: 0x04001A09 RID: 6665
		private string inputCard = "";

		// Token: 0x04001A0A RID: 6666
		private static string lastLoadUsers = "";

		// Token: 0x04001A0B RID: 6667
		private int MaxRecord = 20000;

		// Token: 0x04001A0C RID: 6668
		private string recNOMax = "";

		// Token: 0x04001A0D RID: 6669
		private string strGroupFilter = "";

		// Token: 0x04001A0E RID: 6670
		private IContainer components;

		// Token: 0x04001A0F RID: 6671
		private BackgroundWorker backgroundWorker1;

		// Token: 0x04001A10 RID: 6672
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04001A11 RID: 6673
		private global::System.Windows.Forms.Timer timer2;

		// Token: 0x04001A12 RID: 6674
		private ToolStrip toolFindUsers;

		// Token: 0x04001A13 RID: 6675
		private ToolStripLabel toolStripLabel1;

		// Token: 0x04001A14 RID: 6676
		private ToolStripLabel toolStripLabel3;

		// Token: 0x04001A15 RID: 6677
		private ToolStripLabel toolStripLabel4Namebk;

		// Token: 0x04001A16 RID: 6678
		public ToolStripButton btnClear;

		// Token: 0x04001A17 RID: 6679
		public ToolStripButton btnQuery;

		// Token: 0x04001A18 RID: 6680
		public ToolStripComboBox cboFindDept;

		// Token: 0x04001A19 RID: 6681
		public ComboBox cboUsers;

		// Token: 0x04001A1A RID: 6682
		public ToolStripLabel toolStripLabel2;

		// Token: 0x04001A1B RID: 6683
		public ToolStripTextBox txtFindCardID;

		// Token: 0x04001A1C RID: 6684
		public ToolStripTextBox txtFindName;
	}
}
