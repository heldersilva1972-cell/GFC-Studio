using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F1 RID: 753
	public partial class dfrmMapInfo : frmN3000
	{
		// Token: 0x060015CC RID: 5580 RVA: 0x001B77A8 File Offset: 0x001B67A8
		public dfrmMapInfo()
		{
			this.InitializeComponent();
			this.resStr = new ResourceManager("WgiCCard." + base.Name + "Str", Assembly.GetExecutingAssembly());
			this.resStr.IgnoreCase = true;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x001B77E8 File Offset: 0x001B67E8
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{
					if (this.txtMapFileName.Text != "")
					{
						this.OpenFileDialog1.InitialDirectory = this.txtMapFileName.Text;
					}
				}
				catch (Exception)
				{
				}
				this.OpenFileDialog1.FilterIndex = 1;
				if (this.OpenFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					this.txtMapFileName.Text = this.OpenFileDialog1.FileName;
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

		// Token: 0x060015CE RID: 5582 RVA: 0x001B78A0 File Offset: 0x001B68A0
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x001B78B0 File Offset: 0x001B68B0
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				this.txtMapName.Text = this.txtMapName.Text.Trim();
				this.txtMapFileName.Text = this.txtMapFileName.Text.Trim();
				if (this.txtMapName.Text == "")
				{
					XMessageBox.Show(CommonStr.strMapNameNull);
				}
				else if (this.txtMapFileName.Text == "")
				{
					XMessageBox.Show(CommonStr.strMapFileNull);
				}
				else
				{
					FileInfo fileInfo = new FileInfo(this.txtMapFileName.Text);
					if (!fileInfo.Exists)
					{
						fileInfo = new FileInfo(wgAppConfig.Path4PhotoDefault() + fileInfo.Name);
						if (!fileInfo.Exists)
						{
							XMessageBox.Show(CommonStr.strMapFileNotExist);
							return;
						}
					}
					FileInfo fileInfo2 = new FileInfo(wgAppConfig.Path4PhotoDefault() + fileInfo.Name);
					if (fileInfo2.FullName.ToUpper() != fileInfo.FullName.ToUpper())
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
							wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
						}
						fileInfo.CopyTo(wgAppConfig.Path4PhotoDefault() + fileInfo.Name, true);
					}
					this.mapName = this.txtMapName.Text;
					this.mapFile = fileInfo.Name;
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex2)
			{
				wgTools.WgDebugWrite(ex2.ToString(), new object[] { EventLogEntryType.Error });
			}
		}

		// Token: 0x04002D40 RID: 11584
		private ResourceManager resStr;

		// Token: 0x04002D41 RID: 11585
		public string mapFile;

		// Token: 0x04002D42 RID: 11586
		public string mapName;
	}
}
