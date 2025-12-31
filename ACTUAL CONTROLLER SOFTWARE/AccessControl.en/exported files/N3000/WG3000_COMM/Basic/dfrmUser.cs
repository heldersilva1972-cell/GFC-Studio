using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.ExtendFunc.FaceReader;
using WG3000_COMM.ExtendFunc.Finger;
using WG3000_COMM.ExtendFunc.QR2017;
using WG3000_COMM.Properties;
using WG3000_COMM.Reports.Shift;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000034 RID: 52
	public partial class dfrmUser : frmN3000
	{
		// Token: 0x06000382 RID: 898 RVA: 0x00065F3C File Offset: 0x00064F3C
		public dfrmUser()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00066014 File Offset: 0x00065014
		private int AddUser()
		{
			try
			{
				if (this.txtf_ConsumerNO.Text == null)
				{
					return -401;
				}
				if (this.txtf_ConsumerNO.Text == "")
				{
					return -401;
				}
				if (wgAppConfig.getValBySql(string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_ConsumerNO ={0} ", wgTools.PrepareStrNUnicode(this.txtf_ConsumerNO.Text.PadLeft(10, ' ')))) > 0)
				{
					return -401;
				}
			}
			catch (Exception)
			{
			}
			icConsumer icConsumer = new icConsumer();
			int num = icConsumer.addNew(this.txtf_ConsumerNO.Text, this.txtf_ConsumerName.Text, int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString()), this.chkAttendance.Checked ? ((this.cmbNormalShift.SelectedIndex == 0) ? 1 : ((byte)(this.cmbNormalShift.SelectedIndex + 1))) : 0, this.optNormal.Checked ? 0 : 1, this.chkDoorEnabled.Checked ? 1 : 0, this.dtpActivate.Value, this.dtpDeactivate.Value, (this.txtf_PIN.Text == "") ? 0 : int.Parse(this.txtf_PIN.Text), (this.txtf_CardNO.Text == "") ? 0L : long.Parse(this.txtf_CardNO.Text));
			if (num >= 0)
			{
				icConsumer.editUserOtherInfo(icConsumer.gConsumerID, this.txtf_Title.Text, this.txtf_Culture.Text, this.txtf_Hometown.Text, this.txtf_Birthday.Text, this.txtf_Marriage.Text, this.txtf_JoinDate.Text, this.txtf_LeaveDate.Text, this.txtf_CertificateType.Text, this.txtf_CertificateID.Text, this.txtf_SocialInsuranceNo.Text, this.txtf_Addr.Text, this.txtf_Postcode.Text, this.txtf_Sex.Text, this.txtf_Nationality.Text, this.txtf_Religion.Text, this.txtf_EnglishName.Text, this.txtf_Mobile.Text, this.txtf_HomePhone.Text, this.txtf_Telephone.Text, this.txtf_Email.Text, this.txtf_Political.Text, this.txtf_CorporationName.Text, this.txtf_TechGrade.Text, this.txtf_Note.Text);
				wgAppConfig.wgLog(string.Format("{0}:{1} [{2}]", CommonStr.strAddUsers, this.txtf_ConsumerName.Text, this.txtf_CardNO.Text), EventLogEntryType.Information, null);
			}
			return num;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000662F4 File Offset: 0x000652F4
		private void btnAddNext_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.txtf_ConsumerNO.Text))
			{
				this.txtf_ConsumerNO.Text = this.txtf_ConsumerNO.Text.Trim();
			}
			if (wgAppConfig.getParamValBoolByNO(153))
			{
				for (int i = 1; i <= 9; i++)
				{
					if (this.txtf_ConsumerNO.Text.IndexOf("-F" + i.ToString()) > 0)
					{
						this.chkAttendance.Checked = false;
					}
				}
			}
			int num = this.AddUser();
			if (num < 0)
			{
				XMessageBox.Show(this, icConsumer.getErrInfo(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.savePhoto();
			icConsumerShare.setUpdateLog();
			long num2 = 0L;
			long.TryParse(this.txtf_ConsumerNO.Text, out num2);
			if (num2 > 0L)
			{
				this.txtf_ConsumerNO.Text = (num2 + 1L).ToString();
			}
			else if (!string.IsNullOrEmpty(this.strStartCaption) && this.txtf_ConsumerNO.Text.StartsWith(this.strStartCaption))
			{
				long num3;
				if (long.TryParse(this.txtf_ConsumerNO.Text.Substring(this.txtf_ConsumerNO.Text.IndexOf(this.strStartCaption) + this.strStartCaption.Length), out num3))
				{
					num3 += 1L;
					string text;
					if (string.IsNullOrEmpty(this.strStartCaption))
					{
						text = num3.ToString();
					}
					else if (this.userIDlen - this.strStartCaption.Length > 0)
					{
						text = string.Format("{0}{1}", this.strStartCaption, num3.ToString().PadLeft(this.userIDlen - this.strStartCaption.Length, '0'));
					}
					else
					{
						text = string.Format("{0}{1}", this.strStartCaption, num3.ToString());
					}
					this.txtf_ConsumerNO.Text = text;
				}
				else
				{
					this.txtf_ConsumerNO.Text = "";
				}
			}
			else
			{
				this.txtf_ConsumerNO.Text = "";
			}
			this.txtf_ConsumerName.Text = "";
			this.txtf_CardNO.Text = "";
			this.txtf_CardNO.Focus();
			this.bContinued = true;
			this.checkFaceID();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00066534 File Offset: 0x00065534
		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (this.bContinued)
			{
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0006654C File Offset: 0x0006554C
		private void btnCreateQR_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.txtf_CardNO.Text))
				{
					XMessageBox.Show(this, CommonStr.strSetCardNO4CreateQR, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if (wgAppConfig.getParamValBoolByNO(153))
					{
						for (int i = 1; i <= 9; i++)
						{
							if (this.txtf_ConsumerNO.Text.IndexOf("-F" + i.ToString()) > 0)
							{
								this.chkAttendance.Checked = false;
							}
						}
					}
					int num;
					if (this.m_OperateNew)
					{
						num = this.AddUser();
						if (num >= 0)
						{
							this.m_OperateNew = false;
							string text = string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_ConsumerNO ={0} ", wgTools.PrepareStrNUnicode(this.txtf_ConsumerNO.Text.PadLeft(10, ' ')));
							this.consumerID = wgAppConfig.getValBySql(text);
							this.bContinued = true;
						}
					}
					else
					{
						num = this.EditUser();
					}
					if (num < 0)
					{
						XMessageBox.Show(this, icConsumer.getErrInfo(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.savePhoto();
						icConsumerShare.setUpdateLog();
						using (dfrmCreateQR dfrmCreateQR = new dfrmCreateQR())
						{
							dfrmCreateQR.Text = string.Format("{0}..[{1}]", dfrmCreateQR.Text, this.txtf_ConsumerName.Text);
							dfrmCreateQR.lblUser.Text = this.txtf_ConsumerName.Text;
							dfrmCreateQR.dtpActivate.Value = this.dtpActivate.Value;
							dfrmCreateQR.dateBeginHMS1.Value = this.dateBeginHMS1.Value;
							dfrmCreateQR.dtpDeactivate.Value = this.dtpDeactivate.Value;
							dfrmCreateQR.dateEndHMS1.Value = this.dateEndHMS1.Value;
							dfrmCreateQR.consumerCardNO = long.Parse(this.txtf_CardNO.Text);
							dfrmCreateQR.ShowDialog(this);
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00066758 File Offset: 0x00065758
		private void btnFingerAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.txtf_CardNO.Text))
				{
					XMessageBox.Show(this, CommonStr.strSetCardNO, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if (wgAppConfig.getParamValBoolByNO(153))
					{
						for (int i = 1; i <= 9; i++)
						{
							if (this.txtf_ConsumerNO.Text.IndexOf("-F" + i.ToString()) > 0)
							{
								this.chkAttendance.Checked = false;
							}
						}
					}
					int num;
					if (this.m_OperateNew)
					{
						num = this.AddUser();
						if (num >= 0)
						{
							this.m_OperateNew = false;
							string text = string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_ConsumerNO ={0} ", wgTools.PrepareStrNUnicode(this.txtf_ConsumerNO.Text.PadLeft(10, ' ')));
							this.consumerID = wgAppConfig.getValBySql(text);
							this.bContinued = true;
						}
					}
					else
					{
						num = this.EditUser();
					}
					if (num < 0)
					{
						XMessageBox.Show(this, icConsumer.getErrInfo(num), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						this.savePhoto();
						icConsumerShare.setUpdateLog();
						using (dfrmFingerEnroll dfrmFingerEnroll = new dfrmFingerEnroll())
						{
							dfrmFingerEnroll.consumerID = this.consumerID;
							dfrmFingerEnroll.consumerCardNO = long.Parse(this.txtf_CardNO.Text);
							if (dfrmFingerEnroll.ShowDialog() == DialogResult.OK)
							{
								this.strFingerPrint = BitConverter.ToString(dfrmFingerEnroll.dataFingerPrint);
								this.refreshFingerCnt();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000668F4 File Offset: 0x000658F4
		private void btnFingerClear_Click(object sender, EventArgs e)
		{
			if (XMessageBox.Show(this, string.Format(CommonStr.strAreYouSure + " {0}?", this.btnFingerClear.Text), wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
			{
				return;
			}
			new icConsumer().fingerprintClear(this.consumerID);
			wgAppConfig.wgLog(string.Format("{0}-{1}-{2}", this.btnFingerClear.Text, this.txtf_ConsumerName.Text, this.lblFingerCnt.Text.Replace(CommonStr.strFingerEnrolledNum, "")));
			this.refreshFingerCnt();
			byte[] array = new byte[1152];
			byte[] array2 = new byte[1152];
			for (int i = 0; i < 1152; i++)
			{
				array[i] = 0;
			}
			array[0] = 241;
			array[2] = 16;
			long num = long.Parse(this.txtf_CardNO.Text);
			array[5] = (byte)(num & 255L);
			array[6] = (byte)((num >> 8) & 255L);
			array[7] = (byte)((num >> 16) & 255L);
			array[8] = (byte)((num >> 24) & 255L);
			for (int j = 128; j < 640; j++)
			{
				array[j] = array2[j];
			}
			for (int k = 0; k < 1152; k++)
			{
				array2[k] = 0;
			}
			ArrayList arrayList = new ArrayList();
			string text = " SELECT ";
			text += " t_b_Controller_FingerPrint.f_ControllerID , f_FingerPrintName , 0 as f_Selected , f_ControllerSN , f_IP , f_Port , f_ReaderName , f_Notes   from t_b_Controller_FingerPrint LEFT JOIN t_b_Reader ON t_b_Reader.f_ReaderID = t_b_Controller_FingerPrint.f_ReaderID  ORDER BY f_FingerPrintName  ";
			DataTable dataTable = new DataTable();
			new DataView(dataTable);
			new DataView(dataTable);
			DataTable dataTable2 = new DataTable();
			new DataView(dataTable2);
			if (wgAppConfig.IsAccessDB)
			{
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
						{
							oleDbDataAdapter.Fill(dataTable);
						}
					}
					goto IL_0236;
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
			{
				using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
				{
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						sqlDataAdapter.Fill(dataTable);
					}
				}
			}
			IL_0236:
			for (int l = 0; l < dataTable.Rows.Count; l++)
			{
				arrayList.Add(dataTable.Rows[l]["f_FingerprintName"]);
			}
			if (arrayList.Count != 0)
			{
				lock (frmADCT3000.qfingerEnrollInfo.SyncRoot)
				{
					frmADCT3000.qfingerEnrollInfo.Enqueue(array);
					frmADCT3000.qfingerEnrollarrController.Enqueue(arrayList);
				}
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00066C04 File Offset: 0x00065C04
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(this.photoFileName))
				{
					if (!this.checkPhoto(this.photoFileName))
					{
						return;
					}
				}
				else if (!string.IsNullOrEmpty(this.photonameLoaded) && !this.checkPhoto(this.photonameLoaded))
				{
					return;
				}
				if (!string.IsNullOrEmpty(this.txtf_ConsumerNO.Text))
				{
					this.txtf_ConsumerNO.Text = this.txtf_ConsumerNO.Text.Trim();
				}
				if (wgAppConfig.getParamValBoolByNO(153))
				{
					for (int i = 1; i <= 9; i++)
					{
						if (this.txtf_ConsumerNO.Text.IndexOf("-F" + i.ToString()) > 0)
						{
							this.chkAttendance.Checked = false;
						}
					}
				}
				if (wgAppConfig.getParamValBoolByNO(200) && !string.IsNullOrEmpty(this.txtf_Telephone.Text.Trim()))
				{
					long num = 0L;
					if (!long.TryParse(this.txtf_Telephone.Text.Trim(), out num))
					{
						XMessageBox.Show(CommonStr.strWrongSecondCardNO);
						return;
					}
				}
				int num2;
				if (this.m_OperateNew)
				{
					num2 = this.AddUser();
				}
				else
				{
					num2 = this.EditUser();
				}
				if (num2 < 0)
				{
					XMessageBox.Show(this, icConsumer.getErrInfo(num2), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					this.savePhoto();
					icConsumerShare.setUpdateLog();
					base.DialogResult = DialogResult.OK;
					base.Close();
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00066D8C File Offset: 0x00065D8C
		private void btnReadSFZ_Click(object sender, EventArgs e)
		{
			try
			{
				this.USBSZFReaderLoad();
				if (this.iRetCOM == 1 || this.iRetUSB == 1)
				{
					if (dfrmUser.CVRSDK.CVR_Authenticate() == 1)
					{
						if (dfrmUser.CVRSDK.CVR_Read_Content(4) == 1)
						{
							this.FillData();
						}
						else
						{
							XMessageBox.Show(CommonStr.strSFZReaderPutCards, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else
					{
						XMessageBox.Show(CommonStr.strSFZReaderOperateFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
			catch (Exception ex)
			{
				XMessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00066E14 File Offset: 0x00065E14
		private void btnSelectPhoto_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{
					this.photoFileName = "";
				}
				catch (Exception ex)
				{
					wgTools.WriteLine(ex.ToString());
				}
				this.openFileDialog1.Filter = " (*.jpg)|*.jpg|(*.bmp)|*.bmp";
				this.openFileDialog1.FilterIndex = 1;
				if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					this.photoFileName = this.openFileDialog1.FileName;
					Image image = this.pictureBox1.Image;
					wgAppConfig.ShowMyImage(this.photoFileName, ref image);
					this.pictureBox1.Image = image;
					this.checkPhoto(this.photoFileName);
				}
			}
			catch (Exception ex2)
			{
				wgTools.WriteLine(ex2.ToString());
				XMessageBox.Show(ex2.ToString());
			}
			Directory.SetCurrentDirectory(Application.StartupPath);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00066EEC File Offset: 0x00065EEC
		private void cbof_GroupID_DropDown(object sender, EventArgs e)
		{
			wgAppConfig.AdjustComboBoxDropDownListWidth(this.cbof_GroupID);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00066EF9 File Offset: 0x00065EF9
		private void cbof_GroupID_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.toolTip1.SetToolTip(this.cbof_GroupID, this.cbof_GroupID.Text);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00066F18 File Offset: 0x00065F18
		private void checkFaceID()
		{
			try
			{
				if (!wgAppConfig.IsAccessDB && wgAppConfig.getParamValBoolByNO(201))
				{
					this.updateSFZLabel();
					using (dfrmFaceIDCheckInput dfrmFaceIDCheckInput = new dfrmFaceIDCheckInput())
					{
						if (dfrmFaceIDCheckInput.ShowDialog(this) == DialogResult.OK)
						{
							long lastID = dfrmFaceIDCheckInput.lastID;
							if (lastID > 0L)
							{
								DataTable dataTable = new DataTable();
								string text = "SELECT * FROM [tRecognize] WHERE id = " + lastID.ToString();
								using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
								{
									using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
									{
										using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
										{
											sqlDataAdapter.Fill(dataTable);
										}
									}
								}
								if (dataTable.Rows.Count > 0)
								{
									this.txtf_Note.Text = lastID.ToString();
									this.txtf_Addr.Text = wgTools.SetObjToStr(dataTable.Rows[0]["address"]);
									this.txtf_Sex.Text = wgTools.SetObjToStr(dataTable.Rows[0]["sex"]);
									this.txtf_Birthday.Text = wgTools.SetObjToStr(dataTable.Rows[0]["birthday"]);
									this.txtf_CertificateType.Text = CommonStr.strSFZCertificateID;
									this.txtf_CertificateID.Text = wgTools.SetObjToStr(dataTable.Rows[0]["idCardCode"]);
									if (wgAppConfig.getParamValBoolByNO(194) && !this.txtf_CardNO.ReadOnly)
									{
										this.txtf_CardNO.Text = this.txtf_CertificateID.Text.Replace("X", "");
									}
									this.txtf_ConsumerName.Text = wgTools.SetObjToStr(dataTable.Rows[0]["realName"]);
									this.txtf_Mobile.Text = wgTools.SetObjToStr(dataTable.Rows[0]["phone"]);
									this.txtf_JoinDate.Text = wgTools.SetObjToStr(dataTable.Rows[0]["recognizeTimeStr"]);
								}
							}
							else
							{
								base.Close();
							}
						}
						else
						{
							base.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000671F8 File Offset: 0x000661F8
		private bool checkPhoto(string fileName)
		{
			if (!wgAppConfig.IsAccessControlBlue)
			{
				if (dfrmUser.needToCheck < 0)
				{
					if (wgAppConfig.FileIsExisted(Application.StartupPath + "\\opencv_world341.dll") && wgAppConfig.FileIsExisted(Application.StartupPath + "\\n3k_face.dll"))
					{
						dfrmUser.needToCheck = 1;
					}
					else
					{
						dfrmUser.needToCheck = 0;
					}
				}
				if (string.IsNullOrEmpty(fileName))
				{
					return true;
				}
				if (wgAppConfig.FileIsExisted(fileName))
				{
					FileInfo fileInfo = new FileInfo(fileName);
					if (fileInfo.Exists && fileInfo.Length >= 512000L)
					{
						XMessageBox.Show(CommonStr.strPhotoInvalidOver500KB, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
				if (dfrmUser.needToCheck == 1)
				{
					try
					{
						IntPtr intPtr = dfrmUser.createApp();
						int num = dfrmUser.qualityTest4Image(intPtr, fileName);
						dfrmUser.deleteApp(intPtr);
						if (num != 0)
						{
							object[] array = new object[]
							{
								fileName,
								CommonStr.strPhotoInvalid,
								num,
								(num <= -1 && num >= -6) ? this.errPhotoInfo[-num - 1] : ""
							};
							wgAppConfig.wgLog(string.Format("{0} {1}: {2}.{3}", array));
							XMessageBox.Show(CommonStr.strPhotoInvalid, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
						return true;
					}
					catch (Exception ex)
					{
						wgAppConfig.wgLog(ex.ToString());
						XMessageBox.Show(ex.ToString(), wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00067360 File Offset: 0x00066360
		private void chkAttendance_CheckedChanged(object sender, EventArgs e)
		{
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				this.grpbAttendance.Visible = this.chkAttendance.Checked;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00067381 File Offset: 0x00066381
		private void chkDoorEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.grpbAccessControl.Visible = this.chkDoorEnabled.Checked;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0006739C File Offset: 0x0006639C
		private void clearOldBuffPhoto()
		{
			try
			{
				if (wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo() + "newPhoto"))
				{
					string text = wgAppConfig.Path4Photo() + "newPhoto\\123.jpg";
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
			this.timer3.Enabled = true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0006740C File Offset: 0x0006640C
		private void clearPhotoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(this.photonameLoaded))
				{
					this.photoFileNameToDelete = this.photonameLoaded;
				}
				this.photoFileName = "";
				Image image = this.pictureBox1.Image;
				wgAppConfig.ShowMyImage(this.photoFileName, ref image);
				this.pictureBox1.Image = image;
			}
			catch (Exception ex)
			{
				wgTools.WriteLine(ex.ToString());
				XMessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00067490 File Offset: 0x00066490
		private void CloseSFZ()
		{
			try
			{
				dfrmUser.CVRSDK.CVR_CloseComm();
			}
			catch
			{
			}
		}

		// Token: 0x06000395 RID: 917
		[DllImport("n3k_face.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr createApp();

		// Token: 0x06000396 RID: 918 RVA: 0x000674B8 File Offset: 0x000664B8
		private void dateBeginHMS1_ValueChanged(object sender, EventArgs e)
		{
			if (icConsumer.gTimeSecondEnabled)
			{
				this.dtpActivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd"), this.dateBeginHMS1.Value.ToString("HH:mm:ss")));
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00067518 File Offset: 0x00066518
		private void dateEndHMS1_ValueChanged(object sender, EventArgs e)
		{
			if (!icConsumer.gTimeSecondEnabled && this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
			{
				this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
			}
			this.dtpDeactivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpDeactivate.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
		}

		// Token: 0x06000398 RID: 920
		[DllImport("n3k_face.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void deleteApp(IntPtr h);

		// Token: 0x06000399 RID: 921 RVA: 0x000675C4 File Offset: 0x000665C4
		private void dfrmUser_FormClosing(object sender, FormClosingEventArgs e)
		{
			wgAppConfig.DisposeImage(this.pictureBox1.Image);
			if (this.bUSBSZFReaderIsLoaded)
			{
				this.CloseSFZ();
			}
			if (this.frmSnapshot != null)
			{
				this.frmSnapshot.Close();
				this.frmSnapshot.Dispose();
				this.frmSnapshot = null;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00067614 File Offset: 0x00066614
		private void dfrmUser_Load(object sender, EventArgs e)
		{
			if (wgAppConfig.IsActivateCard19)
			{
				this.txtf_CardNO.Mask = "9999999999999999999";
			}
			else
			{
				this.txtf_CardNO.Mask = "9999999999";
			}
			if (wgTools.gbHideCardNO)
			{
				this.txtf_CardNO.PasswordChar = '#';
			}
			this.cmbNormalShift.SelectedIndex = 0;
			if (wgAppConfig.getParamValBoolByNO(215))
			{
				this.cmbNormalShift.Visible = true;
			}
			else
			{
				this.cmbNormalShift.Visible = false;
			}
			this.txtf_PIN.Mask = "999999";
			this.txtf_PIN.Text = 345678.ToString();
			this.dtpActivate.Value = DateTime.Now.Date;
			this.tabPage1.BackColor = this.BackColor;
			this.tabPage2.BackColor = this.BackColor;
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			this.label1.Text = wgAppConfig.ReplaceWorkNO(this.label1.Text);
			new icGroup().getGroup(ref this.arrGroupName, ref this.arrGroupID, ref this.arrGroupNO);
			int i = this.arrGroupID.Count;
			for (i = 0; i < this.arrGroupID.Count; i++)
			{
				this.cbof_GroupID.Items.Add(this.arrGroupName[i].ToString());
			}
			if (this.cbof_GroupID.Items.Count > 0)
			{
				this.cbof_GroupID.SelectedIndex = 0;
			}
			this.loadUserAutoAdd();
			if (this.m_OperateNew)
			{
				this.loadData4New();
				this.btnAddNext.Visible = true;
			}
			else
			{
				this.loadData4Edit();
				this.btnAddNext.Visible = false;
			}
			this.chkAttendance_CheckedChanged(null, null);
			this.chkDoorEnabled_CheckedChanged(null, null);
			if (wgAppConfig.getParamValBoolByNO(113))
			{
				if (this.chkAttendance.Checked)
				{
					this.grpbAttendance.Visible = true;
				}
			}
			else
			{
				this.optNormal.Checked = true;
				this.grpbAttendance.Visible = false;
			}
			this.label7.Visible = wgAppConfig.getParamValBoolByNO(123);
			this.txtf_PIN.Visible = wgAppConfig.getParamValBoolByNO(123);
			wgAppConfig.setDisplayFormatDate(this.dtpActivate, wgTools.DisplayFormat_DateYMD);
			wgAppConfig.setDisplayFormatDate(this.dtpDeactivate, wgTools.DisplayFormat_DateYMD);
			if (icConsumer.gTimeSecondEnabled)
			{
				this.lblActivateTime.Visible = true;
				this.dateBeginHMS1.Visible = true;
				this.dateEndHMS1.Format = DateTimePickerFormat.Time;
				if (this.m_OperateNew)
				{
					this.dateBeginHMS1.Value = DateTime.Now;
					this.dateEndHMS1.Value = DateTime.Parse("2099-12-31 23:59:59");
				}
			}
			else
			{
				this.dateEndHMS1.CustomFormat = "HH:mm";
				this.dateEndHMS1.Format = DateTimePickerFormat.Custom;
				if (this.dateEndHMS1.Value.ToString("HH:mm").Equals("00:00"))
				{
					this.dateEndHMS1.Value = DateTime.Parse(this.dateEndHMS1.Value.ToString("yyyy-MM-dd 23:59:59"));
				}
			}
			this.btnReadSFZ.Visible = wgAppConfig.getParamValBoolByNO(168);
			if (wgAppConfig.getParamValBoolByNO(168))
			{
				this.updateSFZLabel();
			}
			if (!wgAppConfig.IsAccessControlBlue)
			{
				this.btnCreateQR.Visible = wgAppConfig.getParamValBoolByNO(195);
			}
			this.refreshFingerCnt();
			if (wgAppConfig.getParamValBoolByNO(200))
			{
				this.Label17.Text = CommonStr.strCardIDSecond + ":";
				this.txtf_Telephone.BackColor = Color.Yellow;
			}
			if (this.m_OperateNew)
			{
				this.checkFaceID();
			}
			else if (!wgAppConfig.IsAccessDB && wgAppConfig.getParamValBoolByNO(201))
			{
				this.updateSFZLabel();
			}
			this.clearOldBuffPhoto();
			this.checkPhoto("");
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000679EC File Offset: 0x000669EC
		private int EditUser()
		{
			try
			{
				if (this.txtf_ConsumerNO.Text == null)
				{
					return -401;
				}
				if (this.txtf_ConsumerNO.Text == "")
				{
					return -401;
				}
				if (wgAppConfig.getValBySql(string.Format("SELECT f_ConsumerID FROM t_b_Consumer WHERE f_ConsumerNO ={0} AND NOT ( f_ConsumerID ={1}) ", wgTools.PrepareStrNUnicode(this.txtf_ConsumerNO.Text.PadLeft(10, ' ')), this.m_consumerID)) > 0)
				{
					return -401;
				}
			}
			catch (Exception)
			{
			}
			icConsumer icConsumer = new icConsumer();
			int num = icConsumer.editUser(this.m_consumerID, this.txtf_ConsumerNO.Text, this.txtf_ConsumerName.Text, int.Parse(this.arrGroupID[this.cbof_GroupID.SelectedIndex].ToString()), this.chkAttendance.Checked ? ((this.cmbNormalShift.SelectedIndex == 0) ? 1 : ((byte)(this.cmbNormalShift.SelectedIndex + 1))) : 0, this.optNormal.Checked ? 0 : 1, this.chkDoorEnabled.Checked ? 1 : 0, this.dtpActivate.Value, this.dtpDeactivate.Value, (this.txtf_PIN.Text == "") ? 0 : int.Parse(this.txtf_PIN.Text), (this.txtf_CardNO.Text == "") ? 0L : long.Parse(this.txtf_CardNO.Text));
			if (num >= 0)
			{
				icConsumer.editUserOtherInfo(this.m_consumerID, this.txtf_Title.Text, this.txtf_Culture.Text, this.txtf_Hometown.Text, this.txtf_Birthday.Text, this.txtf_Marriage.Text, this.txtf_JoinDate.Text, this.txtf_LeaveDate.Text, this.txtf_CertificateType.Text, this.txtf_CertificateID.Text, this.txtf_SocialInsuranceNo.Text, this.txtf_Addr.Text, this.txtf_Postcode.Text, this.txtf_Sex.Text, this.txtf_Nationality.Text, this.txtf_Religion.Text, this.txtf_EnglishName.Text, this.txtf_Mobile.Text, this.txtf_HomePhone.Text, this.txtf_Telephone.Text, this.txtf_Email.Text, this.txtf_Political.Text, this.txtf_CorporationName.Text, this.txtf_TechGrade.Text, this.txtf_Note.Text);
			}
			string text = "{0}";
			for (int i = 1; i < 34; i++)
			{
				text = text + ",{" + i.ToString() + "}";
			}
			wgAppConfig.wgLog(CommonStr.strEdit + string.Format(text, new object[]
			{
				this.txtf_ConsumerNO.Text,
				this.txtf_ConsumerName.Text,
				this.arrGroupName[this.cbof_GroupID.SelectedIndex],
				this.chkAttendance.Checked ? 1 : 0,
				this.optNormal.Checked ? 0 : 1,
				this.chkDoorEnabled.Checked ? 1 : 0,
				this.dtpActivate.Value,
				this.dtpDeactivate.Value,
				(this.txtf_PIN.Text == "") ? 0 : int.Parse(this.txtf_PIN.Text),
				(this.txtf_CardNO.Text == "") ? 0L : long.Parse(this.txtf_CardNO.Text),
				this.txtf_Title.Text,
				this.txtf_Culture.Text,
				this.txtf_Hometown.Text,
				this.txtf_Birthday.Text,
				this.txtf_Marriage.Text,
				this.txtf_JoinDate.Text,
				this.txtf_LeaveDate.Text,
				this.txtf_CertificateType.Text,
				this.txtf_CertificateID.Text,
				this.txtf_SocialInsuranceNo.Text,
				this.txtf_Addr.Text,
				this.txtf_Postcode.Text,
				this.txtf_Sex.Text,
				this.txtf_Nationality.Text,
				this.txtf_Religion.Text,
				this.txtf_EnglishName.Text,
				this.txtf_Mobile.Text,
				this.txtf_HomePhone.Text,
				this.txtf_Telephone.Text,
				this.txtf_Email.Text,
				this.txtf_Political.Text,
				this.txtf_CorporationName.Text,
				this.txtf_TechGrade.Text,
				this.txtf_Note.Text
			}));
			return num;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00067F94 File Offset: 0x00066F94
		public void FillData()
		{
			try
			{
				this.photoFileName = Application.StartupPath + "\\zp.bmp";
				Image image = this.pictureBox1.Image;
				wgAppConfig.ShowMyImage(this.photoFileName, ref image);
				this.pictureBox1.Image = image;
				byte[] array = new byte[30];
				int num = 30;
				dfrmUser.CVRSDK.GetPeopleName(ref array[0], ref num);
				byte[] array2 = new byte[30];
				num = 36;
				dfrmUser.CVRSDK.GetPeopleIDCode(ref array2[0], ref num);
				byte[] array3 = new byte[30];
				num = 3;
				dfrmUser.CVRSDK.GetPeopleNation(ref array3[0], ref num);
				byte[] array4 = new byte[30];
				num = 16;
				dfrmUser.CVRSDK.GetStartDate(ref array4[0], ref num);
				byte[] array5 = new byte[30];
				num = 16;
				dfrmUser.CVRSDK.GetPeopleBirthday(ref array5[0], ref num);
				byte[] array6 = new byte[230];
				num = 70;
				dfrmUser.CVRSDK.GetPeopleAddress(ref array6[0], ref num);
				byte[] array7 = new byte[30];
				num = 16;
				dfrmUser.CVRSDK.GetEndDate(ref array7[0], ref num);
				byte[] array8 = new byte[30];
				num = 30;
				dfrmUser.CVRSDK.GetDepartment(ref array8[0], ref num);
				byte[] array9 = new byte[30];
				num = 3;
				dfrmUser.CVRSDK.GetPeopleSex(ref array9[0], ref num);
				byte[] array10 = new byte[32];
				dfrmUser.CVRSDK.CVR_GetSAMID(ref array10[0]);
				this.txtf_Addr.Text = Encoding.GetEncoding("GB2312").GetString(array6).Replace("\0", "")
					.Trim();
				this.txtf_Sex.Text = Encoding.GetEncoding("GB2312").GetString(array9).Replace("\0", "")
					.Trim();
				this.txtf_Birthday.Text = Encoding.GetEncoding("GB2312").GetString(array5).Replace("\0", "")
					.Trim();
				this.txtf_Email.Text = Encoding.GetEncoding("GB2312").GetString(array8).Replace("\0", "")
					.Trim();
				this.txtf_CertificateType.Text = CommonStr.strSFZCertificateID;
				this.txtf_CertificateID.Text = Encoding.GetEncoding("GB2312").GetString(array2).Replace("\0", "")
					.Trim();
				if (wgAppConfig.getParamValBoolByNO(194) && !this.txtf_CardNO.ReadOnly)
				{
					this.txtf_CardNO.Text = this.txtf_CertificateID.Text.Replace("X", "");
				}
				this.txtf_ConsumerName.Text = Encoding.GetEncoding("GB2312").GetString(array).Replace("\0", "")
					.Trim();
				this.txtf_Nationality.Text = Encoding.GetEncoding("GB2312").GetString(array3).Replace("\0", "")
					.Trim();
				this.txtf_Postcode.Text = Encoding.GetEncoding("GB2312").GetString(array4).Replace("\0", "")
					.Trim() + "-" + Encoding.GetEncoding("GB2312").GetString(array7).Replace("\0", "")
					.Trim();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00068318 File Offset: 0x00067318
		public int getSFZID()
		{
			try
			{
				this.USBSZFReaderLoad();
				if (this.iRetCOM == 1 || this.iRetUSB == 1)
				{
					if (dfrmUser.CVRSDK.CVR_Authenticate() == 1)
					{
						if (dfrmUser.CVRSDK.CVR_Read_Content(4) == 1)
						{
							byte[] array = new byte[30];
							int num = 30;
							dfrmUser.CVRSDK.GetPeopleName(ref array[0], ref num);
							byte[] array2 = new byte[30];
							num = 36;
							dfrmUser.CVRSDK.GetPeopleIDCode(ref array2[0], ref num);
							int valBySql = wgAppConfig.getValBySql("SELECT t_b_Consumer_Other.f_ConsumerID From t_b_Consumer_Other,t_b_Consumer  WHERE f_CertificateID =" + wgTools.PrepareStrNUnicode(Encoding.GetEncoding("GB2312").GetString(array2).Replace("\0", "")
								.Trim()) + " AND t_b_Consumer_Other.f_ConsumerID = t_b_Consumer.f_ConsumerID ");
							if (valBySql > 0)
							{
								this.consumerID = valBySql;
								return valBySql;
							}
							XMessageBox.Show(string.Format("{0}: {1} \r\n\r\n{2}: {3}\r\n\r\n{4}", new object[]
							{
								CommonStr.strName,
								Encoding.GetEncoding("GB2312").GetString(array).Replace("\0", "")
									.Trim(),
								CommonStr.strSFZCertificateIDCode,
								Encoding.GetEncoding("GB2312").GetString(array2).Replace("\0", "")
									.Trim(),
								CommonStr.strSFZNotFind
							}));
						}
						else
						{
							XMessageBox.Show(CommonStr.strSFZReaderPutCards, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else
					{
						XMessageBox.Show(CommonStr.strSFZReaderOperateFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
			catch (Exception ex)
			{
				XMessageBox.Show(ex.ToString());
			}
			return -1;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000684C0 File Offset: 0x000674C0
		private void grpbAccessControl_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000684C4 File Offset: 0x000674C4
		private void loadData4Edit()
		{
			if (wgAppConfig.IsAccessDB)
			{
				this.loadData4Edit_Acc();
				return;
			}
			SqlConnection sqlConnection = null;
			SqlCommand sqlCommand = null;
			try
			{
				sqlConnection = new SqlConnection(wgAppConfig.dbConString);
				sqlCommand = new SqlCommand("", sqlConnection);
				try
				{
					string text = " SELECT  t_b_Consumer.*,  f_GroupName ";
					text = text + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  where [f_ConsumerID]= " + this.m_consumerID.ToString();
					sqlCommand.CommandText = text;
					sqlCommand.Connection = sqlConnection;
					sqlConnection.Open();
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
					if (sqlDataReader.Read())
					{
						this.chkDoorEnabled.Checked = (byte)sqlDataReader["f_DoorEnabled"] > 0;
						this.chkAttendance.Checked = (byte)sqlDataReader["f_AttendEnabled"] > 0;
						if ((byte)sqlDataReader["f_AttendEnabled"] > 0)
						{
							this.cmbNormalShift.SelectedIndex = (int)((byte)sqlDataReader["f_AttendEnabled"] - 1);
						}
						this.txtf_ConsumerNO.Text = wgTools.SetObjToStr(sqlDataReader["f_ConsumerNO"]);
						this.txtf_ConsumerName.Text = wgTools.SetObjToStr(sqlDataReader["f_ConsumerName"]);
						this.txtf_CardNO.Text = wgTools.SetObjToStr(sqlDataReader["f_CardNO"]);
						if (this.txtf_CardNO.Text != "")
						{
							this.txtf_CardNO.ReadOnly = true;
							this.txtf_CardNO.Cursor = Cursors.Arrow;
						}
						this.txtf_PIN.Text = wgTools.SetObjToStr(sqlDataReader["f_PIN"]);
						this.dtpActivate.Value = (DateTime)sqlDataReader["f_BeginYMD"];
						this.dateBeginHMS1.Value = (DateTime)sqlDataReader["f_BeginYMD"];
						this.dtpDeactivate.Value = (DateTime)sqlDataReader["f_EndYMD"];
						this.dateEndHMS1.Value = (DateTime)sqlDataReader["f_EndYMD"];
						this.m_curGroup = wgTools.SetObjToStr(sqlDataReader["f_GroupName"]);
						this.cbof_GroupID.Text = this.m_curGroup;
						this.optNormal.Checked = true;
						this.optShift.Checked = (byte)sqlDataReader["f_ShiftEnabled"] > 0;
					}
					sqlDataReader.Close();
					text = " SELECT  * ";
					text = text + " FROM t_b_Consumer_Other   where [f_ConsumerID]= " + this.m_consumerID.ToString();
					sqlCommand.CommandText = text;
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.Default);
					if (sqlDataReader.Read())
					{
						this.txtf_Title.Text = wgTools.SetObjToStr(sqlDataReader["f_Title"]);
						this.txtf_Culture.Text = wgTools.SetObjToStr(sqlDataReader["f_Culture"]);
						this.txtf_Hometown.Text = wgTools.SetObjToStr(sqlDataReader["f_Hometown"]);
						this.txtf_Birthday.Text = wgTools.SetObjToStr(sqlDataReader["f_Birthday"]);
						this.txtf_Marriage.Text = wgTools.SetObjToStr(sqlDataReader["f_Marriage"]);
						this.txtf_JoinDate.Text = wgTools.SetObjToStr(sqlDataReader["f_JoinDate"]);
						this.txtf_LeaveDate.Text = wgTools.SetObjToStr(sqlDataReader["f_LeaveDate"]);
						this.txtf_CertificateType.Text = wgTools.SetObjToStr(sqlDataReader["f_CertificateType"]);
						this.txtf_CertificateID.Text = wgTools.SetObjToStr(sqlDataReader["f_CertificateID"]);
						this.txtf_SocialInsuranceNo.Text = wgTools.SetObjToStr(sqlDataReader["f_SocialInsuranceNo"]);
						this.txtf_Addr.Text = wgTools.SetObjToStr(sqlDataReader["f_Addr"]);
						this.txtf_Postcode.Text = wgTools.SetObjToStr(sqlDataReader["f_Postcode"]);
						this.txtf_Sex.Text = wgTools.SetObjToStr(sqlDataReader["f_Sex"]);
						this.txtf_Nationality.Text = wgTools.SetObjToStr(sqlDataReader["f_Nationality"]);
						this.txtf_Religion.Text = wgTools.SetObjToStr(sqlDataReader["f_Religion"]);
						this.txtf_EnglishName.Text = wgTools.SetObjToStr(sqlDataReader["f_EnglishName"]);
						this.txtf_Mobile.Text = wgTools.SetObjToStr(sqlDataReader["f_Mobile"]);
						this.txtf_HomePhone.Text = wgTools.SetObjToStr(sqlDataReader["f_HomePhone"]);
						this.txtf_Telephone.Text = wgTools.SetObjToStr(sqlDataReader["f_Telephone"]);
						this.txtf_Email.Text = wgTools.SetObjToStr(sqlDataReader["f_Email"]);
						this.txtf_Political.Text = wgTools.SetObjToStr(sqlDataReader["f_Political"]);
						this.txtf_CorporationName.Text = wgTools.SetObjToStr(sqlDataReader["f_CorporationName"]);
						this.txtf_TechGrade.Text = wgTools.SetObjToStr(sqlDataReader["f_TechGrade"]);
						this.txtf_Note.Text = wgTools.SetObjToStr(sqlDataReader["f_Note"]);
					}
					sqlDataReader.Close();
					this.loadPhoto();
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					if (sqlCommand != null)
					{
						sqlCommand.Dispose();
					}
					if (sqlConnection.State != ConnectionState.Closed)
					{
						sqlConnection.Close();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00068A54 File Offset: 0x00067A54
		private void loadData4Edit_Acc()
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			try
			{
				oleDbConnection = new OleDbConnection(wgAppConfig.dbConString);
				oleDbCommand = new OleDbCommand("", oleDbConnection);
				try
				{
					string text = " SELECT  t_b_Consumer.*,  f_GroupName ";
					text = text + " FROM t_b_Consumer LEFT OUTER JOIN t_b_Group ON t_b_Consumer.f_GroupID = t_b_Group.f_GroupID  where [f_ConsumerID]= " + this.m_consumerID.ToString();
					oleDbCommand.CommandText = text;
					oleDbCommand.Connection = oleDbConnection;
					oleDbConnection.Open();
					OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
					if (oleDbDataReader.Read())
					{
						this.chkDoorEnabled.Checked = (byte)oleDbDataReader["f_DoorEnabled"] > 0;
						this.chkAttendance.Checked = (byte)oleDbDataReader["f_AttendEnabled"] > 0;
						if ((byte)oleDbDataReader["f_AttendEnabled"] > 0)
						{
							this.cmbNormalShift.SelectedIndex = (int)((byte)oleDbDataReader["f_AttendEnabled"] - 1);
						}
						this.txtf_ConsumerNO.Text = wgTools.SetObjToStr(oleDbDataReader["f_ConsumerNO"]);
						this.txtf_ConsumerName.Text = wgTools.SetObjToStr(oleDbDataReader["f_ConsumerName"]);
						this.txtf_CardNO.Text = wgTools.SetObjToStr(oleDbDataReader["f_CardNO"]);
						if (this.txtf_CardNO.Text != "")
						{
							this.txtf_CardNO.ReadOnly = true;
						}
						this.txtf_PIN.Text = wgTools.SetObjToStr(oleDbDataReader["f_PIN"]);
						this.dtpActivate.Value = (DateTime)oleDbDataReader["f_BeginYMD"];
						this.dateBeginHMS1.Value = (DateTime)oleDbDataReader["f_BeginYMD"];
						this.dtpDeactivate.Value = (DateTime)oleDbDataReader["f_EndYMD"];
						this.dateEndHMS1.Value = (DateTime)oleDbDataReader["f_EndYMD"];
						this.m_curGroup = wgTools.SetObjToStr(oleDbDataReader["f_GroupName"]);
						this.cbof_GroupID.Text = this.m_curGroup;
						this.optNormal.Checked = true;
						this.optShift.Checked = (byte)oleDbDataReader["f_ShiftEnabled"] > 0;
					}
					oleDbDataReader.Close();
					text = " SELECT  * ";
					text = text + " FROM t_b_Consumer_Other   where [f_ConsumerID]= " + this.m_consumerID.ToString();
					oleDbCommand.CommandText = text;
					oleDbDataReader = oleDbCommand.ExecuteReader(CommandBehavior.Default);
					if (oleDbDataReader.Read())
					{
						this.txtf_Title.Text = wgTools.SetObjToStr(oleDbDataReader["f_Title"]);
						this.txtf_Culture.Text = wgTools.SetObjToStr(oleDbDataReader["f_Culture"]);
						this.txtf_Hometown.Text = wgTools.SetObjToStr(oleDbDataReader["f_Hometown"]);
						this.txtf_Birthday.Text = wgTools.SetObjToStr(oleDbDataReader["f_Birthday"]);
						this.txtf_Marriage.Text = wgTools.SetObjToStr(oleDbDataReader["f_Marriage"]);
						this.txtf_JoinDate.Text = wgTools.SetObjToStr(oleDbDataReader["f_JoinDate"]);
						this.txtf_LeaveDate.Text = wgTools.SetObjToStr(oleDbDataReader["f_LeaveDate"]);
						this.txtf_CertificateType.Text = wgTools.SetObjToStr(oleDbDataReader["f_CertificateType"]);
						this.txtf_CertificateID.Text = wgTools.SetObjToStr(oleDbDataReader["f_CertificateID"]);
						this.txtf_SocialInsuranceNo.Text = wgTools.SetObjToStr(oleDbDataReader["f_SocialInsuranceNo"]);
						this.txtf_Addr.Text = wgTools.SetObjToStr(oleDbDataReader["f_Addr"]);
						this.txtf_Postcode.Text = wgTools.SetObjToStr(oleDbDataReader["f_Postcode"]);
						this.txtf_Sex.Text = wgTools.SetObjToStr(oleDbDataReader["f_Sex"]);
						this.txtf_Nationality.Text = wgTools.SetObjToStr(oleDbDataReader["f_Nationality"]);
						this.txtf_Religion.Text = wgTools.SetObjToStr(oleDbDataReader["f_Religion"]);
						this.txtf_EnglishName.Text = wgTools.SetObjToStr(oleDbDataReader["f_EnglishName"]);
						this.txtf_Mobile.Text = wgTools.SetObjToStr(oleDbDataReader["f_Mobile"]);
						this.txtf_HomePhone.Text = wgTools.SetObjToStr(oleDbDataReader["f_HomePhone"]);
						this.txtf_Telephone.Text = wgTools.SetObjToStr(oleDbDataReader["f_Telephone"]);
						this.txtf_Email.Text = wgTools.SetObjToStr(oleDbDataReader["f_Email"]);
						this.txtf_Political.Text = wgTools.SetObjToStr(oleDbDataReader["f_Political"]);
						this.txtf_CorporationName.Text = wgTools.SetObjToStr(oleDbDataReader["f_CorporationName"]);
						this.txtf_TechGrade.Text = wgTools.SetObjToStr(oleDbDataReader["f_TechGrade"]);
						this.txtf_Note.Text = wgTools.SetObjToStr(oleDbDataReader["f_Note"]);
					}
					oleDbDataReader.Close();
					this.loadPhoto();
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
				}
				finally
				{
					if (oleDbCommand != null)
					{
						oleDbCommand.Dispose();
					}
					if (oleDbConnection.State != ConnectionState.Closed)
					{
						oleDbConnection.Close();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00068FC8 File Offset: 0x00067FC8
		private void loadData4New()
		{
			try
			{
				long num = new icConsumer().ConsumerNONext(this.strStartCaption);
				if (num < 0L)
				{
					num = 1L;
				}
				string text;
				if (string.IsNullOrEmpty(this.strStartCaption))
				{
					text = num.ToString();
				}
				else if (this.userIDlen - this.strStartCaption.Length > 0)
				{
					text = string.Format("{0}{1}", this.strStartCaption, num.ToString().PadLeft(this.userIDlen - this.strStartCaption.Length, '0'));
				}
				else
				{
					text = string.Format("{0}{1}", this.strStartCaption, num.ToString());
				}
				this.txtf_ConsumerNO.Text = text;
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00069098 File Offset: 0x00068098
		private void loadPhoto()
		{
			try
			{
				string text;
				if (wgAppConfig.IsPhotoNameFromConsumerNO)
				{
					if (this.txtf_ConsumerNO.Text.Trim() == "")
					{
						text = null;
					}
					else
					{
						text = wgAppConfig.getPhotoFileNameByConsumerNO(this.txtf_ConsumerNO.Text);
					}
					this.pictureAbstractNameFromConsumerNO = text;
				}
				else if (this.txtf_CardNO.Text.Trim() == "")
				{
					text = null;
				}
				else
				{
					text = wgAppConfig.getPhotoFileName(long.Parse(this.txtf_CardNO.Text));
				}
				Image image = this.pictureBox1.Image;
				this.photonameLoaded = text;
				wgAppConfig.ShowMyImage(text, ref image);
				this.pictureBox1.Image = image;
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00069170 File Offset: 0x00068170
		private void loadUserAutoAdd()
		{
			try
			{
				this.strUserAutoAddSet = wgAppConfig.GetKeyVal("UserAutoAddSet");
				if (!string.IsNullOrEmpty(this.strUserAutoAddSet) && this.strUserAutoAddSet.IndexOf(",") > 0)
				{
					string text = this.strUserAutoAddSet.Substring(0, this.strUserAutoAddSet.IndexOf(","));
					string text2 = this.strUserAutoAddSet.Substring(this.strUserAutoAddSet.IndexOf(",") + 1);
					if (int.Parse(text) > 0)
					{
						this.userIDlen = int.Parse(text);
					}
					this.strStartCaption = wgTools.SetObjToStr(text2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003A4 RID: 932
		[DllImport("n3k_face.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void qualityTest(IntPtr h, string dir);

		// Token: 0x060003A5 RID: 933
		[DllImport("n3k_face.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int qualityTest4Image(IntPtr h, string image_path);

		// Token: 0x060003A6 RID: 934 RVA: 0x00069220 File Offset: 0x00068220
		private void refreshFingerCnt()
		{
			try
			{
				if (!wgAppConfig.getParamValBoolByNO(188))
				{
					this.lblFingerCnt.Visible = false;
					this.btnFingerAdd.Visible = false;
					this.btnFingerClear.Visible = false;
				}
				else
				{
					this.lblFingerCnt.Visible = true;
					this.btnFingerAdd.Visible = true;
					this.btnFingerClear.Visible = true;
					int valBySql = wgAppConfig.getValBySql(string.Format("SELECT COUNT(f_FingerNO)  From t_b_Consumer_Fingerprint WHERE t_b_Consumer_Fingerprint.f_ConsumerID ={0}", this.consumerID));
					int valBySql2 = wgAppConfig.getValBySql("SELECT count(t_b_Consumer_Fingerprint.f_fingerNO) from t_b_Consumer_Fingerprint,t_b_consumer where t_b_Consumer_Fingerprint.f_consumerid = t_b_consumer.f_consumerid");
					if (valBySql > 0)
					{
						this.lblFingerCnt.Text = string.Format("{0}{1}{2}", CommonStr.strFingerEnrolledNum, valBySql.ToString(), CommonStr.strUnitFinger);
						this.btnFingerClear.Enabled = true;
						if (valBySql >= 10)
						{
							this.btnFingerAdd.Enabled = false;
						}
					}
					else
					{
						this.lblFingerCnt.Text = "";
						this.btnFingerClear.Enabled = false;
					}
					if (valBySql2 >= 1000)
					{
						this.btnFingerAdd.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00069348 File Offset: 0x00068348
		private void savePhoto()
		{
			if (wgAppConfig.IsPhotoNameFromConsumerNO)
			{
				this.savePhotoByConsumerNO();
				return;
			}
			if (this.photoFileName == "")
			{
				if (string.IsNullOrEmpty(this.photoFileNameToDelete))
				{
					return;
				}
				try
				{
					string text = this.photoFileNameToDelete;
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					return;
				}
				catch (Exception ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					return;
				}
			}
			if (!string.IsNullOrEmpty(this.txtf_CardNO.Text))
			{
				try
				{
					wgAppConfig.photoDirectoryLastWriteTime = DateTime.Parse("2012-6-12 18:57:08.531");
					if (wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo()))
					{
						FileInfo fileInfo = new FileInfo(this.photoFileName);
						FileInfo fileInfo2 = new FileInfo(wgAppConfig.Path4Photo() + this.txtf_CardNO.Text + fileInfo.Extension);
						if (fileInfo2.FullName.ToUpper() != this.photoFileName.ToUpper())
						{
							try
							{
								if (fileInfo2.Exists)
								{
									fileInfo2.Delete();
								}
							}
							catch (Exception ex2)
							{
								wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
							}
							fileInfo.CopyTo(wgAppConfig.Path4Photo() + this.txtf_CardNO.Text + ".jpg", true);
						}
						this.photoFileName = "";
						this.pictureBox1.Image = null;
					}
				}
				catch (Exception ex3)
				{
					wgTools.WgDebugWrite(ex3.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000694CC File Offset: 0x000684CC
		private void savePhotoByConsumerNO()
		{
			string text = this.photoFileName;
			if (this.photoFileName == "")
			{
				if (string.IsNullOrEmpty(this.pictureAbstractNameFromConsumerNO))
				{
					return;
				}
				text = this.pictureAbstractNameFromConsumerNO;
			}
			if (!string.IsNullOrEmpty(this.txtf_ConsumerNO.Text))
			{
				try
				{
					wgAppConfig.photoDirectoryLastWriteTime = DateTime.Parse("2012-6-12 18:57:08.531");
					if (wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo()))
					{
						string text2 = text;
						FileInfo fileInfo = new FileInfo(text2);
						FileInfo fileInfo2 = new FileInfo(wgAppConfig.Path4Photo() + this.txtf_ConsumerNO.Text + fileInfo.Extension);
						if (fileInfo2.FullName.ToUpper() != text.ToUpper())
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
							if (string.IsNullOrEmpty(this.photoFileName))
							{
								fileInfo.MoveTo(wgAppConfig.Path4Photo() + this.txtf_ConsumerNO.Text + ".jpg");
							}
							else
							{
								fileInfo.CopyTo(wgAppConfig.Path4Photo() + this.txtf_ConsumerNO.Text + ".jpg", true);
							}
						}
						this.pictureAbstractNameFromConsumerNO = "";
						this.photoFileName = "";
						this.pictureBox1.Image = null;
					}
				}
				catch (Exception ex2)
				{
					wgTools.WgDebugWrite(ex2.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00069650 File Offset: 0x00068650
		private void snapShotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\Snapshot.exe");
				if (fileInfo.Exists)
				{
					string startupPath = Application.StartupPath;
					Process.Start("Snapshot.exe", startupPath);
					return;
				}
				try
				{
					Process process = frmConsole.RunningInstance();
					if (process != null && (process.MainWindowTitle.Equals("Capture") || process.MainWindowTitle.Equals(CommonStr.strCameraPhoto)))
					{
						return;
					}
				}
				catch
				{
				}
				this.btnTakePhoto.Enabled = false;
				try
				{
					string text = wgAppConfig.getValStringBySql("SELECT f_Password FROM t_s_Operator WHERE f_OperatorID = " + icOperator.OperatorID);
					if (!string.IsNullOrEmpty(text))
					{
						text = Program.Dpt4Database(text);
					}
					wgAppConfig.wgDebugWrite(Process.Start(new ProcessStartInfo
					{
						FileName = Application.StartupPath + "\\N3000.exe",
						Arguments = string.Format(" -USER {0} -PASSWORD '{1}'  -SNAPSHOT", wgTools.PrepareStr(icOperator.OperatorName), text),
						UseShellExecute = true
					}).ToString());
				}
				catch (Exception ex)
				{
					wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
					wgAppConfig.wgLog(ex.ToString());
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgLog(ex2.ToString());
			}
			this.btnTakePhoto.Enabled = true;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000697EC File Offset: 0x000687EC
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			try
			{
				if (this.txtf_CardNO.Text.Length >= 8)
				{
					this.cbof_GroupID.Focus();
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0006984C File Offset: 0x0006884C
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
						if (!this.txtf_CardNO.ReadOnly)
						{
							this.txtf_CardNO.Text = num.ToString();
							this.txtf_CardNO.Focus();
						}
						else if (wgAppConfig.getParamValBoolByNO(200))
						{
							this.tabControl1.SelectedTab = this.tabPage2;
							this.txtf_Telephone.Text = num.ToString();
							this.txtf_Telephone.Focus();
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

		// Token: 0x060003AC RID: 940 RVA: 0x0006992C File Offset: 0x0006892C
		private void timer3_Tick(object sender, EventArgs e)
		{
			try
			{
				if (wgAppConfig.DirectoryIsExisted(wgAppConfig.Path4Photo() + "newPhoto"))
				{
					string text = wgAppConfig.Path4Photo() + "newPhoto\\123.jpg";
					if (wgAppConfig.FileIsExisted(text))
					{
						string text2 = wgAppConfig.Path4Photo() + "newPhoto\\123bk.jpg";
						if (File.Exists(text2))
						{
							File.Delete(text2);
						}
						File.Copy(text, text2, true);
						this.photoFileName = text2;
						Image image = this.pictureBox1.Image;
						wgAppConfig.ShowMyImage(this.photoFileName, ref image);
						this.pictureBox1.Image = image;
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						this.checkPhoto(this.photoFileName);
					}
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000699F8 File Offset: 0x000689F8
		private void txtf_CardNO_KeyPress(object sender, KeyPressEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtf_CardNO);
			wgTools.WgDebugWrite("txtf_CardNO_KeyPress(object sender, KeyPressEventArgs e)", new object[0]);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00069A18 File Offset: 0x00068A18
		private void txtf_CardNO_KeyUp(object sender, KeyEventArgs e)
		{
			wgAppConfig.CardIDInput(ref this.txtf_CardNO);
			wgTools.WgDebugWrite("txtf_CardNO_KeyUp(object sender, KeyEventArgs e)", new object[0]);
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

		// Token: 0x060003AF RID: 943 RVA: 0x00069AB8 File Offset: 0x00068AB8
		private void txtf_CardNO_TextChanged(object sender, EventArgs e)
		{
			if (this.txtf_CardNO.Text.Length == 1)
			{
				this.timer1.Interval = 500;
				this.timer1.Enabled = true;
			}
			if (this.txtf_CardNO.Text.Length == 0)
			{
				this.btnSelectPhoto.Enabled = false;
				return;
			}
			this.btnSelectPhoto.Enabled = true;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00069B20 File Offset: 0x00068B20
		private void updateSFZLabel()
		{
			this.Label16.Text = CommonStr.strSFZAddress + ":";
			this.Label22.Text = CommonStr.strSFZDept + ":";
			this.Label18.Text = CommonStr.strSFZValidDate + ":";
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00069B7C File Offset: 0x00068B7C
		private void USBSZFReaderLoad()
		{
			if (!this.bUSBSZFReaderIsLoaded)
			{
				try
				{
					for (int i = 1001; i <= 1016; i++)
					{
						this.iRetUSB = dfrmUser.CVRSDK.CVR_InitComm(i);
						if (this.iRetUSB == 1)
						{
							break;
						}
					}
					if (this.iRetUSB != 1)
					{
						for (int i = 1; i <= 4; i++)
						{
							this.iRetCOM = dfrmUser.CVRSDK.CVR_InitComm(i);
							if (this.iRetCOM == 1)
							{
								break;
							}
						}
					}
					if (this.iRetCOM == 1 || this.iRetUSB == 1)
					{
						this.bUSBSZFReaderIsLoaded = true;
					}
					else
					{
						XMessageBox.Show(CommonStr.strSFZReaderInitializeFailed, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				catch (Exception ex)
				{
					XMessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00069C38 File Offset: 0x00068C38
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00069C40 File Offset: 0x00068C40
		public int consumerID
		{
			get
			{
				return this.m_consumerID;
			}
			set
			{
				this.m_consumerID = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00069C49 File Offset: 0x00068C49
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00069C51 File Offset: 0x00068C51
		public string curGroup
		{
			get
			{
				return this.m_curGroup;
			}
			set
			{
				this.m_curGroup = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00069C5A File Offset: 0x00068C5A
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00069C62 File Offset: 0x00068C62
		public bool OperateNew
		{
			get
			{
				return this.m_OperateNew;
			}
			set
			{
				this.m_OperateNew = value;
			}
		}

		// Token: 0x040006BC RID: 1724
		private bool bContinued;

		// Token: 0x040006BD RID: 1725
		private bool bUSBSZFReaderIsLoaded;

		// Token: 0x040006BE RID: 1726
		private Snapshot frmSnapshot;

		// Token: 0x040006BF RID: 1727
		private int iRetCOM;

		// Token: 0x040006C0 RID: 1728
		private int iRetUSB;

		// Token: 0x040006C1 RID: 1729
		private int m_consumerID;

		// Token: 0x040006C2 RID: 1730
		private string strUserAutoAddSet;

		// Token: 0x040006C3 RID: 1731
		private int userIDlen;

		// Token: 0x040006C4 RID: 1732
		public string address;

		// Token: 0x040006C5 RID: 1733
		private ArrayList arrGroupID = new ArrayList();

		// Token: 0x040006C6 RID: 1734
		private ArrayList arrGroupName = new ArrayList();

		// Token: 0x040006C7 RID: 1735
		private ArrayList arrGroupNO = new ArrayList();

		// Token: 0x040006C8 RID: 1736
		public string birthday;

		// Token: 0x040006C9 RID: 1737
		private string[] errPhotoInfo = new string[] { "* -1 检测不到人脸", "* -2 检测到多张人脸", "* -3 人脸宽度小于等于80", "* -4 人脸越界", "* -5 角度不符合", "* -6 质量不符合" };

		// Token: 0x040006CA RID: 1738
		private string inputCard = "";

		// Token: 0x040006CB RID: 1739
		private string m_curGroup = "";

		// Token: 0x040006CC RID: 1740
		private bool m_OperateNew = true;

		// Token: 0x040006CD RID: 1741
		public StringBuilder name;

		// Token: 0x040006CE RID: 1742
		private static int needToCheck = -1;

		// Token: 0x040006CF RID: 1743
		public string number;

		// Token: 0x040006D0 RID: 1744
		public string people;

		// Token: 0x040006D1 RID: 1745
		private string photoFileName = "";

		// Token: 0x040006D2 RID: 1746
		private string photoFileNameToDelete = "";

		// Token: 0x040006D3 RID: 1747
		private string photonameLoaded = "";

		// Token: 0x040006D4 RID: 1748
		private string pictureAbstractNameFromConsumerNO = "";

		// Token: 0x040006D5 RID: 1749
		public string sex;

		// Token: 0x040006D6 RID: 1750
		public string signdate;

		// Token: 0x040006D7 RID: 1751
		private string strFingerPrint = "";

		// Token: 0x040006D8 RID: 1752
		private string strStartCaption = "";

		// Token: 0x040006D9 RID: 1753
		public string validtermOfEnd;

		// Token: 0x040006DA RID: 1754
		public string validtermOfStart;

		// Token: 0x02000035 RID: 53
		private class CVRSDK
		{
			// Token: 0x060003BB RID: 955
			[DllImport("termb.dll", CharSet = CharSet.Auto)]
			public static extern int CVR_Authenticate();

			// Token: 0x060003BC RID: 956
			[DllImport("termb.dll", CharSet = CharSet.Auto)]
			public static extern int CVR_CloseComm();

			// Token: 0x060003BD RID: 957
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int CVR_GetSAMID(ref byte strTmp);

			// Token: 0x060003BE RID: 958
			[DllImport("termb.dll", CharSet = CharSet.Auto)]
			public static extern int CVR_InitComm(int Port);

			// Token: 0x060003BF RID: 959
			[DllImport("termb.dll", CharSet = CharSet.Auto)]
			public static extern int CVR_Read_Content(int Active);

			// Token: 0x060003C0 RID: 960
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetDepartment(ref byte strTmp, ref int strLen);

			// Token: 0x060003C1 RID: 961
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetEndDate(ref byte strTmp, ref int strLen);

			// Token: 0x060003C2 RID: 962
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetManuID(ref byte strTmp);

			// Token: 0x060003C3 RID: 963
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetPeopleAddress(ref byte strTmp, ref int strLen);

			// Token: 0x060003C4 RID: 964
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetPeopleBirthday(ref byte strTmp, ref int strLen);

			// Token: 0x060003C5 RID: 965
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetPeopleIDCode(ref byte strTmp, ref int strLen);

			// Token: 0x060003C6 RID: 966
			[DllImport("termb.dll", CharSet = CharSet.Ansi)]
			public static extern int GetPeopleName(ref byte strTmp, ref int strLen);

			// Token: 0x060003C7 RID: 967
			[DllImport("termb.dll", CharSet = CharSet.Ansi)]
			public static extern int GetPeopleNation(ref byte strTmp, ref int strLen);

			// Token: 0x060003C8 RID: 968
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetPeopleSex(ref byte strTmp, ref int strLen);

			// Token: 0x060003C9 RID: 969
			[DllImport("termb.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern int GetStartDate(ref byte strTmp, ref int strLen);
		}

		// Token: 0x02000036 RID: 54
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct IDCARD_ALL
		{
			// Token: 0x0400073E RID: 1854
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
			public char name;

			// Token: 0x0400073F RID: 1855
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
			public char sex;

			// Token: 0x04000740 RID: 1856
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
			public char people;

			// Token: 0x04000741 RID: 1857
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public char birthday;

			// Token: 0x04000742 RID: 1858
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 70)]
			public char address;

			// Token: 0x04000743 RID: 1859
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
			public char number;

			// Token: 0x04000744 RID: 1860
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
			public char signdate;

			// Token: 0x04000745 RID: 1861
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public char validtermOfStart;

			// Token: 0x04000746 RID: 1862
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public char validtermOfEnd;
		}
	}
}
