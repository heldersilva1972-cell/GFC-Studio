using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using JRO;
using Microsoft.VisualBasic;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM
{
	// Token: 0x02000229 RID: 553
	public partial class dfrmDbCompact : frmN3000
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x00122C24 File Offset: 0x00121C24
		public dfrmDbCompact()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00122C40 File Offset: 0x00121C40
		private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.cmdCompactDatabase_Click_Acc(sender, e);
				return;
			}
			this.saveFileDialog1.Filter = " (*.bak)|*.bak| (*.*)|*.*";
			this.saveFileDialog1.FilterIndex = 1;
			this.saveFileDialog1.RestoreDirectory = true;
			try
			{
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				string text = "AccessData";
				try
				{
					text = sqlConnection.Database;
				}
				catch (Exception)
				{
				}
				sqlConnection.Close();
				this.saveFileDialog1.InitialDirectory = ".\\REPORT";
				this.saveFileDialog1.FileName = string.Format("{0}_sql_{1}.bak", text, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.saveFileDialog1.Title = this.backUpToolStripMenuItem.Text;
			if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				string fileName = this.saveFileDialog1.FileName;
				this.sqlBackup2010(fileName);
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00122D48 File Offset: 0x00121D48
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00122D50 File Offset: 0x00121D50
		public void cmdCompactDatabase_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSureAgain + " {0}?", this.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
			{
				wgAppConfig.wgLog(this.Text + " ......");
				if (wgAppConfig.IsAccessDB)
				{
					this.cmdCompactDatabase_Click_Acc(sender, e);
					return;
				}
				this.sqlBackup2010();
			}
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00122DB4 File Offset: 0x00121DB4
		public void cmdCompactDatabase_Click_Acc(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				JetEngine jetEngine;
				try
				{
					jetEngine = new JetEngineClass();
				}
				catch
				{
					using (Process process = new Process())
					{
						process.StartInfo.FileName = "regsvr32";
						process.StartInfo.Arguments = "/s \"" + Application.StartupPath + "\\msjro.dll\"";
						process.Start();
						process.WaitForExit();
					}
					jetEngine = new JetEngineClass();
				}
				Thread.Sleep(500);
				string text = "";
				string accessDbName = wgAppConfig.accessDbName;
				string text2;
				if (string.IsNullOrEmpty(accessDbName))
				{
					text2 = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_ff") + ".mdb";
				}
				else
				{
					text2 = accessDbName + DateTime.Now.ToString("-yyyy-MM-dd_HHmmss_ff") + ".mdb";
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.FileName = text2;
					saveFileDialog.Filter = " (*.mdb)|*.mdb";
					saveFileDialog.InitialDirectory = Application.StartupPath + ".\\BACKUP";
					string keyVal = wgAppConfig.GetKeyVal("BackupPathOfAccessDB");
					if (!string.IsNullOrEmpty(keyVal))
					{
						try
						{
							saveFileDialog.InitialDirectory = keyVal;
						}
						catch
						{
						}
					}
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text = saveFileDialog.FileName;
						wgAppConfig.UpdateKeyVal("BackupPathOfAccessDB", text);
						using (dfrmWait dfrmWait = new dfrmWait())
						{
							dfrmWait.Show();
							dfrmWait.Refresh();
							wgAppConfig.backupBeforeExitByJustCopy();
							jetEngine.CompactDatabase(wgAppConfig.dbConString, string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID=admin;Password=;JET OLEDB:Database Password=168168;Jet OLEDB:Engine Type=5", text));
							new FileInfo(text).CopyTo(Application.StartupPath + string.Format("\\{0}.mdb", wgAppConfig.accessDbName), true);
							string text3 = "DELETE FROM t_d_SwipeRecord Where f_RecordAll = '################'";
							wgAppConfig.runSql(text3);
							dfrmWait.Hide();
						}
						XMessageBox.Show(string.Concat(new string[]
						{
							this.Text,
							"  ",
							CommonStr.strSuccessfully,
							"\r\n\r\n",
							text
						}));
						base.Close();
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WriteLine("Backup Access: " + ex.ToString());
				wgAppConfig.wgLog("Backup Access: " + ex.ToString());
				XMessageBox.Show(string.Concat(new string[]
				{
					this.Text,
					" ",
					CommonStr.strFailed,
					" ",
					ex.ToString()
				}));
			}
			finally
			{
				Directory.SetCurrentDirectory(Application.StartupPath);
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00123104 File Offset: 0x00122104
		private void dfrmDbCompact_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyValue == 81)
			{
				if (icOperator.OperatorID != 1)
				{
					XMessageBox.Show(this, CommonStr.strNeedSuperPrivilege, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				this.txtDirectory.Visible = true;
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00123154 File Offset: 0x00122154
		private void dfrmDbCompact_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.restoreToolStripMenuItem.Visible = false;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0012316C File Offset: 0x0012216C
		private bool restoreDatabase2011(string databaseName, string oldDBBackupFile)
		{
			bool flag = false;
			if (databaseName != "")
			{
				bool flag2 = false;
				try
				{
					SqlConnection.ClearAllPools();
					string text = wgAppConfig.dbConString.Replace(string.Format("initial catalog={0}", this.wgDatabaseName), string.Format("initial catalog={0}", "master"));
					SqlCommand sqlCommand = new SqlCommand();
					SqlConnection sqlConnection = new SqlConnection(text);
					string text2 = " SELECT name FROM sysdatabases ";
					sqlCommand = new SqlCommand(text2, sqlConnection);
					sqlConnection.Open();
					object obj = sqlCommand.ExecuteScalar();
					sqlConnection.Close();
					if (obj == null)
					{
						XMessageBox.Show(CommonStr.strFailed);
						return flag;
					}
					text2 = " SELECT  convert( int, LEFT(convert(nvarchar,SERVERPROPERTY('ProductVersion')),CHARINDEX('.',convert(nvarchar,SERVERPROPERTY('ProductVersion')))-1)) ";
					sqlCommand = new SqlCommand(text2, sqlConnection);
					sqlConnection.Open();
					object obj2 = sqlCommand.ExecuteScalar();
					sqlConnection.Close();
					if (obj2 == null)
					{
						XMessageBox.Show(CommonStr.strFailed);
						return flag;
					}
					sqlConnection.Open();
					new SqlCommand(string.Format("IF NOT EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE name = {0})", wgTools.PrepareStrNUnicode(this.wgDatabaseName)) + "\r\n CREATE DATABASE [" + this.wgDatabaseName + "] ", sqlConnection)
					{
						CommandTimeout = 300
					}.ExecuteNonQuery();
					sqlCommand = new SqlCommand(string.Format("SELECT * FROM master.dbo.sysdatabases WHERE name = {0}", wgTools.PrepareStrNUnicode(this.wgDatabaseName)), sqlConnection);
					sqlCommand.CommandTimeout = 300;
					string text3 = "";
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						text3 = wgTools.SetObjToStr(sqlDataReader["filename"]);
					}
					sqlDataReader.Close();
					using (dfrmWait dfrmWait = new dfrmWait())
					{
						dfrmWait.Show();
						dfrmWait.Refresh();
						sqlCommand = new SqlCommand(string.Format("RESTORE FILELISTONLY FROM DISK = '{1}'  ", this.wgDatabaseName, oldDBBackupFile), sqlConnection);
						sqlCommand.CommandTimeout = 300;
						string text4 = "";
						string text5 = "";
						try
						{
							sqlDataReader = sqlCommand.ExecuteReader();
							while (sqlDataReader.Read())
							{
								if (sqlDataReader["type"].ToString() == "D")
								{
									text4 = wgTools.SetObjToStr(sqlDataReader["LogicalName"]);
								}
								else if (sqlDataReader["type"].ToString() == "L")
								{
									text5 = wgTools.SetObjToStr(sqlDataReader["LogicalName"]);
								}
							}
							sqlDataReader.Close();
						}
						catch (Exception ex)
						{
							wgTools.WgDebugWrite(ex.ToString(), new object[0]);
						}
						if (text4 != "" && text5 != "")
						{
							text2 = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}', MOVE '{4}' TO '{5}'  ", new object[]
							{
								this.wgDatabaseName,
								oldDBBackupFile,
								text4,
								text3,
								text5,
								text3.Replace(".mdf", "_log.ldf")
							});
						}
						else if (text4 != "")
						{
							text2 = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}' ", new object[] { this.wgDatabaseName, oldDBBackupFile, text4, text3 });
						}
						else if (text5 != "")
						{
							text2 = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}', MOVE '{4}' TO '{5}'  ", new object[]
							{
								this.wgDatabaseName,
								oldDBBackupFile,
								text5,
								text3.Replace(".mdf", "_log.ldf")
							});
						}
						else
						{
							text2 = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE ", this.wgDatabaseName, oldDBBackupFile);
						}
						try
						{
							sqlCommand = new SqlCommand(text2, sqlConnection);
							sqlCommand.CommandTimeout = 300;
							Cursor.Current = Cursors.WaitCursor;
							sqlCommand.ExecuteNonQuery();
						}
						catch (Exception ex2)
						{
							wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							sqlCommand = new SqlCommand(string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE ", this.wgDatabaseName, oldDBBackupFile), sqlConnection);
							sqlCommand.CommandTimeout = 300;
							Cursor.Current = Cursors.WaitCursor;
							sqlCommand.ExecuteNonQuery();
						}
						dfrmWait.Hide();
					}
					flag2 = true;
					Cursor.Current = Cursors.Default;
					wgAppConfig.wgLog(this.restoreToolStripMenuItem.Text + "\t" + oldDBBackupFile);
					XMessageBox.Show("OK");
				}
				catch (Exception ex3)
				{
					XMessageBox.Show(CommonStr.strFailed + "\r\n\r\n" + ex3.ToString());
				}
				finally
				{
					flag = flag2;
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00123638 File Offset: 0x00122638
		private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.Filter = " (*.bak)|*.bak| (*.*)|*.*";
			this.openFileDialog1.FilterIndex = 1;
			this.openFileDialog1.RestoreDirectory = true;
			this.openFileDialog1.Title = this.restoreToolStripMenuItem.Text;
			this.openFileDialog1.FileName = "";
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				string fileName = this.openFileDialog1.FileName;
				try
				{
					SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
					try
					{
						this.wgDatabaseName = sqlConnection.Database;
					}
					catch (Exception)
					{
					}
					sqlConnection.Close();
					if (this.wgDatabaseName != null)
					{
						if (this.restoreDatabase2011(this.wgDatabaseName, fileName))
						{
							frmADCT3000.bConfirmClose = true;
							wgAppConfig.gRestart = true;
							(this.callForm as frmADCT3000).Close();
							base.Close();
						}
						else
						{
							XMessageBox.Show(CommonStr.strFailed + " " + this.restoreToolStripMenuItem.Text);
						}
					}
				}
				catch (Exception ex)
				{
					XMessageBox.Show(CommonStr.strFailed + "\r\n\r\n" + ex.Message);
				}
				return;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0012376C File Offset: 0x0012276C
		private bool sqlBackup2010()
		{
			string text = null;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				SqlConnection.ClearAllPools();
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				try
				{
					text = sqlConnection.Database;
				}
				catch (Exception)
				{
				}
				sqlConnection.Close();
				if (text == null)
				{
					return false;
				}
				string text2;
				if (this.txtDirectory.Visible)
				{
					if (this.txtDirectory.Text.Length <= 0 || this.txtDirectory.Text.Substring(0, 2) != "\\\\")
					{
						return false;
					}
					if (this.txtDirectory.Text.Substring(this.txtDirectory.Text.Length - 1, 1) == "\\")
					{
						text2 = string.Format("{0}{1}_sql_{2}.bak", this.txtDirectory.Text, text, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
					}
					else
					{
						text2 = string.Format("{0}\\{1}_sql_{2}.bak", this.txtDirectory.Text, text, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
					}
				}
				else
				{
					text2 = string.Format("{0}_sql_{1}.bak", text, DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
				}
				using (SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString.Replace(string.Format("initial catalog={0}", text), string.Format("initial catalog={0}", "master"))))
				{
					try
					{
						sqlConnection2.Open();
						string text3 = "SELECT  SERVERPROPERTY('productversion'), SERVERPROPERTY ('productlevel'), SERVERPROPERTY ('edition')";
						using (SqlCommand sqlCommand = new SqlCommand(text3, sqlConnection2))
						{
							sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
							object obj = sqlCommand.ExecuteScalar();
							if (obj != null)
							{
								if (this.txtDirectory.Visible)
								{
									if (this.txtDirectory.Text.Length <= 0 || this.txtDirectory.Text.Substring(0, 2) != "\\\\")
									{
										return false;
									}
									if (this.txtDirectory.Text.Substring(this.txtDirectory.Text.Length - 1, 1) == "\\")
									{
										text2 = string.Format("{0}{1}_sql_{2}_{3}.bak", new object[]
										{
											this.txtDirectory.Text,
											text,
											wgTools.SetObjToStr(obj),
											DateAndTime.Now.ToString("yyyyMMdd_HHmmss")
										});
									}
									else
									{
										text2 = string.Format("{0}\\{1}_sql_{2}_{3}.bak", new object[]
										{
											this.txtDirectory.Text,
											text,
											wgTools.SetObjToStr(obj),
											DateAndTime.Now.ToString("yyyyMMdd_HHmmss")
										});
									}
								}
								else
								{
									text2 = string.Format("{0}_sql_{1}_{2}.bak", text, wgTools.SetObjToStr(obj), DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
								}
							}
						}
						using (SqlCommand sqlCommand2 = new SqlCommand(string.Format("BACKUP DATABASE [{0}] TO DISK = {1}", text, wgTools.PrepareStrNUnicode(text2)), sqlConnection2))
						{
							sqlCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlCommand2.ExecuteNonQuery();
						}
						using (SqlCommand sqlCommand3 = new SqlCommand(string.Format("DBCC SHRINKDATABASE({0})", text), sqlConnection2))
						{
							sqlCommand3.CommandTimeout = wgAppConfig.dbCommandTimeout;
							sqlCommand3.ExecuteNonQuery();
						}
						XMessageBox.Show("OK!\r\n\r\n" + text2);
						wgAppConfig.wgLog(this.Text + " OK :  " + text2);
						base.Close();
					}
					catch (Exception ex)
					{
						wgTools.WgDebugWrite(ex.ToString(), new object[0]);
					}
				}
			}
			catch (Exception ex2)
			{
				string cultureInfoStr = wgAppConfig.CultureInfoStr;
				if (cultureInfoStr == null)
				{
					XMessageBox.Show("Failed.  \r\n\r\n" + ex2.ToString());
				}
				else if (!(cultureInfoStr == "zh-CHS"))
				{
					if (!(cultureInfoStr == "zh-CHT"))
					{
						XMessageBox.Show("Failed.  \r\n\r\n" + ex2.ToString());
					}
					else
					{
						XMessageBox.Show("失敗.\r\n\r\n" + ex2.ToString());
					}
				}
				else
				{
					XMessageBox.Show("失败.\r\n\r\n" + ex2.ToString());
				}
				wgAppConfig.wgLog(this.Text + "  Failed. :  \r\n\r\n" + ex2.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
			return true;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00123C88 File Offset: 0x00122C88
		private bool sqlBackup2010(string bkFileName)
		{
			string text = "AccessData";
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				SqlConnection.ClearAllPools();
				SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				try
				{
					text = sqlConnection.Database;
				}
				catch (Exception)
				{
				}
				sqlConnection.Close();
				using (dfrmWait dfrmWait = new dfrmWait())
				{
					dfrmWait.Show();
					dfrmWait.Refresh();
					SqlConnection sqlConnection2 = new SqlConnection(wgAppConfig.dbConString.Replace(string.Format("initial catalog={0}", text), string.Format("initial catalog={0}", "master")));
					string text2 = string.Format("BACKUP DATABASE [{0}] TO DISK = {1}", text, wgTools.PrepareStrNUnicode(bkFileName));
					sqlConnection2.Open();
					using (SqlCommand sqlCommand = new SqlCommand(text2, sqlConnection2))
					{
						sqlCommand.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlCommand.ExecuteNonQuery();
					}
					using (SqlCommand sqlCommand2 = new SqlCommand(string.Format("DBCC SHRINKDATABASE({0})", text), sqlConnection2))
					{
						sqlCommand2.CommandTimeout = wgAppConfig.dbCommandTimeout;
						sqlCommand2.ExecuteNonQuery();
					}
					sqlConnection2.Close();
					dfrmWait.Hide();
				}
				wgAppConfig.wgLog(this.backUpToolStripMenuItem.Text + " OK :  " + bkFileName);
				XMessageBox.Show("OK!\r\n\r\n" + bkFileName);
				base.Close();
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(this.Text + "  Failed. :  \r\n\r\n" + ex.ToString());
				XMessageBox.Show(CommonStr.strFailed + "\r\n\r\n" + ex.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
			return true;
		}

		// Token: 0x04001CBF RID: 7359
		private const string wgDatabaseDefaultName = "AccessData";

		// Token: 0x04001CC0 RID: 7360
		private const string wgDatabaseDefaultNameOfAdroitor = "AccessData";

		// Token: 0x04001CC1 RID: 7361
		public Form callForm;

		// Token: 0x04001CC2 RID: 7362
		private string wgDatabaseName = "AccessData";
	}
}
