using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM
{
	// Token: 0x02000227 RID: 551
	public partial class dfrmAccessDbRestore : frmN3000
	{
		// Token: 0x06001013 RID: 4115 RVA: 0x00120545 File Offset: 0x0011F545
		public dfrmAccessDbRestore()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00120553 File Offset: 0x0011F553
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0012055B File Offset: 0x0011F55B
		private void button1_Click(object sender, EventArgs e)
		{
			this.txtBakFile.Text = Environment.CurrentDirectory + "\\backup\\iCCard3000_000.bak";
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00120578 File Offset: 0x0011F578
		private void button2_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.Filter = " (*.bak)|*.bak| (*.*)|*.*";
			this.openFileDialog1.FilterIndex = 1;
			this.openFileDialog1.RestoreDirectory = true;
			this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\backup";
			this.openFileDialog1.Title = this.button2.Text;
			this.openFileDialog1.FileName = "";
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				string fileName = this.openFileDialog1.FileName;
				this.txtBakFile.Text = this.openFileDialog1.FileName;
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00120620 File Offset: 0x0011F620
		public void cmdCompactDatabase_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtBakFile.Text))
			{
				this.button2_Click(null, null);
			}
			if (!string.IsNullOrEmpty(this.txtBakFile.Text) && XMessageBox.Show(this, string.Format(CommonStr.strAreYouSureAgain + " {0}?", this.cmdRestoreDatabase.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK && wgAppConfig.IsAccessDB)
			{
				wgAppConfig.wgLog(this.Text + " ......");
				this.txtBakFile.Text = this.txtBakFile.Text.Trim();
				if (!string.IsNullOrEmpty(this.txtBakFile.Text))
				{
					Cursor cursor = Cursor.Current;
					Cursor.Current = Cursors.WaitCursor;
					try
					{
						string text;
						try
						{
							FileInfo fileInfo = new FileInfo(".\\iCCard3000.mdb");
							text = "HYBF_iCCard3000" + DateTime.Now.ToString("_yyyyMMdd-HHmmss") + ".bak";
							fileInfo.CopyTo(".\\backup\\" + text, true);
						}
						catch (Exception ex)
						{
							wgTools.WriteLine(ex.ToString());
						}
						try
						{
							FileInfo fileInfo = new FileInfo(".\\backup\\iCCard3000_000.bak");
							text = "HYBF_iCCard3000_000" + DateTime.Now.ToString("_yyyyMMdd") + ".bak";
							fileInfo.CopyTo(".\\backup\\" + text, false);
						}
						catch (Exception ex2)
						{
							wgTools.WriteLine(ex2.ToString());
						}
						try
						{
							FileInfo fileInfo = new FileInfo(".\\backup\\iCCard3000_001.bak");
							text = "HYBF_iCCard3000_001" + DateTime.Now.ToString("_yyyyMMdd") + ".bak";
							fileInfo.CopyTo(".\\backup\\" + text, false);
						}
						catch (Exception ex3)
						{
							wgTools.WriteLine(ex3.ToString());
						}
						text = this.txtBakFile.Text;
						FileInfo fileInfo2 = new FileInfo(text);
						fileInfo2.CopyTo(".\\iCCard3000.mdb", true);
						if (fileInfo2.Exists)
						{
							wgAppConfig.wgLogWithoutDB(string.Format("{0}: {1}---{2}", this.cmdRestoreDatabase.Text, text, CommonStr.strSuccessfully));
							XMessageBox.Show(this.cmdRestoreDatabase.Text + " " + CommonStr.strSuccessfully);
							base.DialogResult = DialogResult.OK;
							base.Close();
						}
						else
						{
							wgAppConfig.wgLogWithoutDB(string.Format("{0}: {1}---{2}", this.cmdRestoreDatabase.Text, text, CommonStr.strFailed));
							XMessageBox.Show(this.cmdRestoreDatabase.Text + " " + CommonStr.strFailed);
						}
					}
					catch (Exception ex4)
					{
						XMessageBox.Show(ex4.ToString());
					}
					finally
					{
						Cursor.Current = cursor;
					}
				}
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00120938 File Offset: 0x0011F938
		private void dfrmDbCompact_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x04001C8D RID: 7309
		public Form callForm;
	}
}
