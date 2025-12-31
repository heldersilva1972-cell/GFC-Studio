using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;

namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000315 RID: 789
	public partial class dfrmCheckAccessSetup : frmN3000
	{
		// Token: 0x0600183A RID: 6202 RVA: 0x001F9185 File Offset: 0x001F8185
		public dfrmCheckAccessSetup()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x001F91A8 File Offset: 0x001F81A8
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{
					this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\Photo\\";
				}
				catch (Exception)
				{
				}
				this.openFileDialog1.Filter = "(*.wav)|*.wav|(*.*)|*.*";
				this.openFileDialog1.FilterIndex = 1;
				this.openFileDialog1.RestoreDirectory = true;
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					this.newSoundFile = this.openFileDialog1.FileName;
					FileInfo fileInfo = new FileInfo(this.newSoundFile);
					this.txtFileName.Text = fileInfo.Name;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x001F9280 File Offset: 0x001F8280
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x001F9290 File Offset: 0x001F8290
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(this.newSoundFile))
				{
					FileInfo fileInfo = new FileInfo(this.newSoundFile);
					FileInfo fileInfo2 = new FileInfo(wgAppConfig.Path4PhotoDefault() + this.txtFileName.Text);
					if (fileInfo2.FullName.ToUpper() != this.newSoundFile.ToUpper())
					{
						try
						{
							if (fileInfo2.Exists)
							{
								fileInfo2.Delete();
							}
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						}
						fileInfo.CopyTo(wgAppConfig.Path4PhotoDefault() + this.txtFileName.Text, true);
					}
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
			}
			try
			{
				if (this.chkActive.Checked)
				{
					this.active = 1;
					this.morecards = (int)this.nudMoreCards.Value;
					this.soundfilename = this.txtFileName.Text;
				}
				else
				{
					this.active = 0;
					this.morecards = 1;
					this.soundfilename = "";
				}
				base.DialogResult = DialogResult.OK;
			}
			catch (Exception ex3)
			{
				wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x001F93E8 File Offset: 0x001F83E8
		private void chkActive_CheckedChanged(object sender, EventArgs e)
		{
			this.GroupBox1.Enabled = this.chkActive.Checked;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x001F9400 File Offset: 0x001F8400
		private void dfrmCheckAccessSetup_Load(object sender, EventArgs e)
		{
			try
			{
				this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
				this.txtGroupName.Text = this.groupname;
				if (this.active > 0)
				{
					this.chkActive.Checked = true;
					this.GroupBox1.Enabled = true;
				}
				else
				{
					this.chkActive.Checked = false;
					this.GroupBox1.Enabled = false;
				}
				this.nudMoreCards.Value = this.morecards;
				this.txtFileName.Text = this.soundfilename;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x040031B2 RID: 12722
		public int active;

		// Token: 0x040031B3 RID: 12723
		public string groupname;

		// Token: 0x040031B4 RID: 12724
		public int morecards = 1;

		// Token: 0x040031B5 RID: 12725
		private string newSoundFile = "";

		// Token: 0x040031B6 RID: 12726
		public string soundfilename;
	}
}
