using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Media;
using System.Windows.Forms;
using WG3000_COMM.Basic;
using WG3000_COMM.Core;
using WG3000_COMM.DataOper;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using WgiCCard;

namespace WG3000_COMM.ExtendFunc.PCCheck
{
	// Token: 0x02000316 RID: 790
	public partial class dfrmPCCheckAccess : frmN3000
	{
		// Token: 0x06001842 RID: 6210 RVA: 0x001F9A10 File Offset: 0x001F8A10
		public dfrmPCCheckAccess()
		{
			this.InitializeComponent();
			this.player = new SoundPlayer();
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x001F9ADB File Offset: 0x001F8ADB
		public void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			this.player.Stop();
			base.Hide();
			this.bDealing = false;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x001F9AFC File Offset: 0x001F8AFC
		private void checkVideoCH()
		{
			if (!wgAppConfig.IsActivateCameraManage)
			{
				this.existedVideoCHDll = 0;
				return;
			}
			if (!icOperator.OperatePrivilegeVisible("mnuCameraMonitor"))
			{
				this.existedVideoCHDll = 0;
				return;
			}
			if (this.existedVideoCHDll < 0)
			{
				try
				{
					CHCNetSDK.NET_DVR_Init();
					this.existedVideoCHDll = 1;
				}
				catch
				{
					this.existedVideoCHDll = 0;
				}
			}
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x001F9B60 File Offset: 0x001F8B60
		private int CmdComm(ref byte[] p, int len)
		{
			int num = this.CmdComm_Realize(ref p, len);
			if (num < 0)
			{
				wgTools.WriteLine("CmdComm  ???");
			}
			return num;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x001F9B88 File Offset: 0x001F8B88
		private int CmdComm_Realize(ref byte[] p, int len)
		{
			byte[] array = new byte[1];
			if (len > 128)
			{
				return this.CmdCommPut128(ref p, len);
			}
			if (len == 0 || len > 11)
			{
				return -1;
			}
			long ticks = DateTime.Now.Ticks;
			byte[] array2 = new byte[this.BAGHEAD.Length + len];
			Array.Copy(this.BAGHEAD, 0, array2, 0, this.BAGHEAD.Length);
			Array.Copy(p, 0, array2, this.BAGHEAD.Length, len);
			this.wgSerial.wgRs232.Write(array2);
			if (Math.Abs(Convert.ToInt32(DateTime.Now.Ticks - ticks) / 10000) > 1000)
			{
				wgTools.WriteLine("wgSerial.wgRs232.Write(p);  ???");
				return -1;
			}
			int num = 1000;
			long num2 = this.msToTicks((long)num);
			long num3 = Convert.ToInt64(DateTime.Now.Ticks + num2);
			if (this.bDealing)
			{
				for (long num4 = 0L; num4 < 16L; num4 += 1L)
				{
					this.RBF[(int)((IntPtr)num4)] = 0;
				}
				for (long num4 = 0L; num4 < 9L; num4 += 1L)
				{
					int num5 = 0;
					while (num5 == 0)
					{
						if (DateTime.Now.Ticks > num3)
						{
							wgTools.WriteLine("接收数据  ???");
							return -1;
						}
						if (this.wgSerial.wgRs232.Read(1) > 0)
						{
							array[0] = this.wgSerial.wgRs232.InputStream[0];
							num5 = 1;
						}
					}
					this.RBF[(int)((IntPtr)num4)] = array[0];
				}
				if (this.RBF[0] == 239)
				{
					if (this.RBF[1] != 1)
					{
						return -1;
					}
					if (this.RBF[2] != 255)
					{
						return -1;
					}
					if (this.RBF[3] != 255)
					{
						return -1;
					}
					if (this.RBF[4] != 255)
					{
						return -1;
					}
					if (this.RBF[5] != 255)
					{
						return -1;
					}
					if (this.RBF[6] != 7)
					{
						return -1;
					}
					int num6 = (int)this.RBF[7] * 256 + (int)this.RBF[8];
					for (long num4 = 0L; num4 < (long)num6; num4 += 1L)
					{
						int num5 = 0;
						while (num5 == 0)
						{
							if (DateTime.Now.Ticks > num3)
							{
								return -1;
							}
							if (this.wgSerial.wgRs232.Read(1) > 0)
							{
								array[0] = this.wgSerial.wgRs232.InputStream[0];
								num5 = 1;
							}
						}
						this.RBF[(int)((IntPtr)(num4 + 9L))] = array[0];
					}
					int sum = this.GetSum(this.RBF, 6, num6 + 1);
					if (((sum >> 8) & 255) != (int)this.RBF[num6 + 7])
					{
						return -1;
					}
					if ((sum & 255) == (int)this.RBF[num6 + 8])
					{
						return 1;
					}
				}
				return -1;
			}
			return 1;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x001F9E68 File Offset: 0x001F8E68
		private int CmdCommGet512(ref byte[] p, int len, ref byte[] data)
		{
			if (p[0] != 0)
			{
				if (len == 0 || len > 11)
				{
					return -1;
				}
				long ticks = DateTime.Now.Ticks;
				byte[] array = new byte[this.BAGHEAD.Length + len];
				Array.Copy(this.BAGHEAD, 0, array, 0, this.BAGHEAD.Length);
				Array.Copy(p, 0, array, this.BAGHEAD.Length, len);
				this.wgSerial.wgRs232.Write(array);
				if (Math.Abs(Convert.ToInt32(DateTime.Now.Ticks - ticks) / 10000) > 1000)
				{
					return -1;
				}
				byte[] array2 = new byte[1];
				byte[] array3 = new byte[148];
				int num = 0;
				this.RBF[9] = 0;
				this.RBF[7] = (byte)(num & 255);
				this.RBF[8] = (byte)((num >> 8) & 255);
				long num2 = this.msToTicks(1000L);
				long num3 = Convert.ToInt64(DateTime.Now.Ticks + num2);
				while (this.bDealing)
				{
					for (int i = 0; i < array3.Length; i++)
					{
						array3[i] = 0;
					}
					for (int i = 0; i < 9; i++)
					{
						int num4 = 0;
						while (num4 == 0)
						{
							if (DateTime.Now.Ticks > num3)
							{
								return -1;
							}
							if (this.wgSerial.wgRs232.Read(1) > 0)
							{
								array2[0] = this.wgSerial.wgRs232.InputStream[0];
								num4 = 1;
							}
						}
						array3[i] = array2[0];
					}
					if (array3[0] != 239)
					{
						return -1;
					}
					if (array3[1] != 1)
					{
						return -1;
					}
					if (array3[2] != 255)
					{
						return -1;
					}
					if (array3[3] != 255)
					{
						return -1;
					}
					if (array3[4] != 255)
					{
						return -1;
					}
					if (array3[5] != 255)
					{
						return -1;
					}
					if (array3[6] != 7 && array3[6] != 2 && array3[6] != 8)
					{
						return -1;
					}
					int num5 = (int)array3[7] * 256 + (int)array3[8];
					for (int i = 0; i < num5; i++)
					{
						int num4 = 0;
						while (num4 == 0)
						{
							if (DateTime.Now.Ticks > num3)
							{
								return -1;
							}
							if (this.wgSerial.wgRs232.Read(1) > 0)
							{
								array2[0] = this.wgSerial.wgRs232.InputStream[0];
								num4 = 1;
							}
						}
						array3[i + 9] = array2[0];
					}
					int sum = this.GetSum(array3, 6, num5 + 1);
					if (((sum >> 8) & 255) != (int)array3[num5 + 7])
					{
						return -1;
					}
					if ((sum & 255) != (int)array3[num5 + 8])
					{
						return -1;
					}
					if (array3[6] != 7 || array3[9] != 0)
					{
						if (array3[6] == 7 && array3[9] != 0)
						{
							this.RBF[9] = array3[9];
							break;
						}
						Array.Copy(array3, 9, data, num, num5 - 2);
						num = num + num5 - 2;
						this.RBF[7] = (byte)(num & 255);
						this.RBF[8] = (byte)((num >> 8) & 255);
						if (array3[6] == 8)
						{
							break;
						}
					}
				}
				if (num == 512)
				{
					return 1;
				}
			}
			return -1;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x001FA194 File Offset: 0x001F9194
		private int CmdCommPut128(ref byte[] p, int len)
		{
			byte[] array = new byte[this.BAGHEAD.Length + len];
			Array.Copy(this.BAGHEAD, 0, array, 0, this.BAGHEAD.Length);
			Array.Copy(p, 0, array, this.BAGHEAD.Length, len);
			this.wgSerial.wgRs232.Write(array);
			return 1;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x001FA1EC File Offset: 0x001F91EC
		private int dealFingerEnroll(ref byte[] resultData)
		{
			byte[] array = new byte[11];
			if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_none)
			{
				return -1;
			}
			wgTools.WriteLine("fingerEnrollStatus= " + this.fingerEnrollStatus);
			if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_getImage1 || this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_getImage1b)
			{
				array[0] = 1;
				array[1] = 0;
				array[2] = 3;
				array[3] = 1;
				int num = this.GetSum(array, 0, 4);
				array[4] = (byte)((num >> 8) & 255);
				array[5] = (byte)(num & 255);
				int num2 = this.CmdComm(ref array, 6);
				wgTools.WriteLine("tmp = CmdComm(ref buf,5+1); ");
				if (num2 == 1 && this.RBF[9] == 0)
				{
					this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_tzM1;
					return num2;
				}
				this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_getImage1;
				return num2;
			}
			else if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_tzM1)
			{
				array[0] = 1;
				array[1] = 0;
				array[2] = 4;
				array[3] = 2;
				array[4] = 1;
				int num = this.GetSum(array, 0, 5);
				array[5] = (byte)((num >> 8) & 255);
				array[6] = (byte)(num & 255);
				int num2 = this.CmdComm(ref array, 7);
				if (num2 == 1 && this.RBF[9] == 0)
				{
					this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_tzMSearch;
					return num2;
				}
				this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_getImage1;
				return num2;
			}
			else
			{
				if (this.fingerEnrollStatus != dfrmPCCheckAccess.finger_status.fs_tzMSearch)
				{
					return 1;
				}
				array[0] = 1;
				array[1] = 0;
				array[2] = 8;
				array[3] = 4;
				array[4] = 1;
				array[5] = 0;
				array[6] = 0;
				array[7] = 3;
				array[8] = byte.MaxValue;
				int num = this.GetSum(array, 0, 9);
				array[9] = (byte)((num >> 8) & 255);
				array[10] = (byte)(num & 255);
				int num2 = this.CmdComm(ref array, 11);
				this.foundIndexNew = -1;
				if (num2 == 1 && this.RBF[9] == 0)
				{
					this.foundIndexNew = ((int)this.RBF[10] << 8) + (int)this.RBF[11];
				}
				if (num2 > 0)
				{
					this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_tzMGetOK;
				}
				return num2;
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x001FA3AC File Offset: 0x001F93AC
		private void dfrmPCCheckAccess_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.bDealing = false;
			this.inputCardInside.Clear();
			this.ConsumerNameInside = "";
			this.SwipeInsideCnt = 0;
			try
			{
				if ((this.frmCaller as frmConsole).photoaviSingle != null)
				{
					if (!(this.frmCaller as frmConsole).photoaviSingle.IsDisposed)
					{
						(this.frmCaller as frmConsole).photoaviSingle.bWatching = false;
						(this.frmCaller as frmConsole).photoaviSingle.stopVideo();
						(this.frmCaller as frmConsole).photoaviSingle.Close();
					}
					(this.frmCaller as frmConsole).photoaviSingle = null;
				}
			}
			catch (Exception ex)
			{
				wgAppConfig.wgLog(ex.ToString());
			}
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x001FA478 File Offset: 0x001F9478
		private void dfrmPCCheckAccess_KeyDown(object sender, KeyEventArgs e)
		{
			foreach (object obj in base.Controls)
			{
				try
				{
					(obj as Control).ImeMode = ImeMode.Off;
				}
				catch (Exception)
				{
				}
			}
			if (!e.Control && !e.Alt && !e.Shift && e.KeyValue >= 48 && e.KeyValue <= 57)
			{
				if (this.inputCard.Length == 0)
				{
					this.inputCardDate = DateTime.Now;
					this.timer1.Interval = 500;
					this.timer1.Enabled = true;
				}
				this.inputCard += (e.KeyValue - 48).ToString();
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x001FA564 File Offset: 0x001F9564
		private void dfrmPCCheckAccess_Load(object sender, EventArgs e)
		{
			this.label4.Text = wgAppConfig.ReplaceFloorRoom(this.label4.Text);
			try
			{
				base.ImeMode = ImeMode.Disable;
				this.btnCancel.ImeMode = ImeMode.Disable;
				foreach (object obj in base.Controls)
				{
					try
					{
						(obj as Control).ImeMode = ImeMode.Off;
					}
					catch (Exception)
					{
					}
				}
				string text = " SELECT a.f_ConsumerName, a.f_ConsumerID, a.f_CardNO,b.f_CheckAccessActive from t_b_consumer a ,t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and b.f_GroupType=1 ";
				if (wgAppConfig.IsAccessDB)
				{
					using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
					{
						using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
						{
							using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
							{
								oleDbDataAdapter.Fill(this.ds, "groups");
							}
						}
						goto IL_0133;
					}
				}
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
						{
							sqlDataAdapter.Fill(this.ds, "groups");
						}
					}
				}
				IL_0133:
				this.dv = new DataView(this.ds.Tables["groups"]);
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[] { EventLogEntryType.Error });
			}
			this.checkVideoCH();
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x001FA7CC File Offset: 0x001F97CC
		public void dfrmPCCheckAccess_OutActive(object sender, KeyEventArgs e)
		{
			this.dfrmPCCheckAccess_KeyDown(sender, e);
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x001FA7D8 File Offset: 0x001F97D8
		private void dfrmPCCheckAccess_VisibleChanged(object sender, EventArgs e)
		{
			try
			{
				if (base.Visible)
				{
					this.openCamera();
					if (!string.IsNullOrEmpty(this.strGroupname) || !string.IsNullOrEmpty(this.strConsumername))
					{
						this.txtA0.Text = this.strGroupname;
						this.txtB0.Text = "";
						DateTime dateTime;
						if (DateTime.TryParse(this.strNow, out dateTime))
						{
							this.txtB0.Text = dateTime.ToString("HH:mm:ss");
						}
						this.txtC0.Text = this.strDoorFullName;
						this.txtConsumers.Text = this.strConsumername;
						if (!string.IsNullOrEmpty(this.strGroupname))
						{
							string text = " SELECT a.f_GroupID,a.f_GroupName,b.f_GroupType,b.f_CheckAccessActive,b.f_MoreCards, b.f_SoundFileName   from t_b_Group a ,t_b_group4PCCheckAccess b where a.f_GroupID = b.f_GroupID and a.f_GroupName =" + wgTools.PrepareStrNUnicode(this.strGroupname);
							if (wgAppConfig.IsAccessDB)
							{
								using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
								{
									using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
									{
										oleDbConnection.Open();
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										if (oleDbDataReader.Read())
										{
											this.wavfile = oleDbDataReader["f_SoundFileName"].ToString();
										}
										oleDbDataReader.Close();
									}
									goto IL_018F;
								}
							}
							using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
							{
								using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
								{
									sqlConnection.Open();
									SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
									if (sqlDataReader.Read())
									{
										this.wavfile = sqlDataReader["f_SoundFileName"].ToString();
									}
									sqlDataReader.Close();
								}
							}
						}
						IL_018F:
						if (this.wavfile == "")
						{
							this.wavfile = "DoorBell.wav";
						}
						else if (wgAppConfig.FileIsExisted(wgAppConfig.Path4PhotoDefault() + this.wavfile))
						{
							this.player.SoundLocation = wgAppConfig.Path4PhotoDefault() + this.wavfile;
							this.player.PlayLooping();
						}
						this.strGroupname = "";
						this.strConsumername = "";
					}
					if (wgAppConfig.getParamValBoolByNO(188))
					{
						this.timer2.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				wgTools.WgDebugWrite(ex.ToString(), new object[0]);
			}
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x001FAA94 File Offset: 0x001F9A94
		private int GetSum(byte[] p, int startloc, int len)
		{
			int num = (int)p[startloc];
			for (int i = 1; i < len; i++)
			{
				num += (int)p[i + startloc];
			}
			return num;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x001FAABA File Offset: 0x001F9ABA
		private long msToTicks(long ms)
		{
			return Convert.ToInt64(Convert.ToInt32(ms * 1000L) * 10);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x001FAAD4 File Offset: 0x001F9AD4
		public void openCamera()
		{
			int num = 0;
			int.TryParse("0" + wgTools.SetObjToStr(wgAppConfig.getSystemParamByNO(172)), out num);
			if (num >= 1)
			{
				if (!icOperator.OperatePrivilegeVisible("mnuCameraMonitor"))
				{
					this.existedVideoCHDll = 0;
					return;
				}
				if (this.existedVideoCHDll <= 0)
				{
					if (this.bFalseShowOnce)
					{
						this.bFalseShowOnce = false;
						if (wgAppConfig.GetKeyVal("KEY_Video_DontDisplayErrorInfo") != "1" && XMessageBox.Show(CommonStr.strVideoDllExisted, wgTools.MSGTITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
						{
							wgAppConfig.UpdateKeyVal("KEY_Video_DontDisplayErrorInfo", "1");
							return;
						}
					}
				}
				else
				{
					try
					{
						this.selectedCameraID = this.selectDoorCamera();
						Point point = new Point(0, 0);
						Size size = new Size(333, 296);
						if ((this.frmCaller as frmConsole).photoaviSingle == null || (this.frmCaller as frmConsole).photoaviSingle.IsDisposed)
						{
							if (!string.IsNullOrEmpty(this.selectedCameraID))
							{
								(this.frmCaller as frmConsole).photoaviSingle = null;
								(this.frmCaller as frmConsole).photoaviSingle = new dfrmPhotoAviSingle();
								if (wgAppConfig.GetKeyVal("KEY_Video_Location") != string.Format("{0},{1}", point.X, point.Y))
								{
									string keyVal = wgAppConfig.GetKeyVal("KEY_Video_Location");
									if (!string.IsNullOrEmpty(keyVal))
									{
										string[] array = keyVal.Split(new char[] { ',' });
										try
										{
											point = new Point(int.Parse(array[0]), int.Parse(array[1]));
										}
										catch
										{
										}
									}
								}
								if (wgAppConfig.GetKeyVal("KEY_Video_Size") != string.Format("{0},{1}", size.Width, size.Height))
								{
									string keyVal2 = wgAppConfig.GetKeyVal("KEY_Video_Size");
									if (!string.IsNullOrEmpty(keyVal2))
									{
										string[] array2 = keyVal2.Split(new char[] { ',' });
										try
										{
											size = new Size(int.Parse(array2[0]), int.Parse(array2[1]));
										}
										catch
										{
										}
									}
								}
								(this.frmCaller as frmConsole).photoaviSingle.Location = point;
								(this.frmCaller as frmConsole).photoaviSingle.Size = size;
								(this.frmCaller as frmConsole).photoaviSingle.selectedCameraID = this.selectedCameraID;
								this.prevSelectedCameraID = this.selectedCameraID;
								(this.frmCaller as frmConsole).photoaviSingle.frmCaller = this.frmCaller;
								(this.frmCaller as frmConsole).photoaviSingle.Show(this);
							}
						}
						else if (this.prevSelectedCameraID == this.selectedCameraID)
						{
							point = (this.frmCaller as frmConsole).photoaviSingle.Location;
							if (wgAppConfig.GetKeyVal("KEY_Video_Location") != string.Format("{0},{1}", point.X, point.Y))
							{
								wgAppConfig.UpdateKeyVal("KEY_Video_Location", string.Format("{0},{1}", point.X, point.Y));
							}
							(this.frmCaller as frmConsole).photoaviSingle.Show(this);
						}
						else
						{
							if (!(this.frmCaller as frmConsole).photoaviSingle.IsDisposed)
							{
								point = (this.frmCaller as frmConsole).photoaviSingle.Location;
								size = (this.frmCaller as frmConsole).photoaviSingle.Size;
								if (wgAppConfig.GetKeyVal("KEY_Video_Location") != string.Format("{0},{1}", point.X, point.Y))
								{
									wgAppConfig.UpdateKeyVal("KEY_Video_Location", string.Format("{0},{1}", point.X, point.Y));
								}
								if (wgAppConfig.GetKeyVal("KEY_Video_Size") != string.Format("{0},{1}", size.Width, size.Height))
								{
									wgAppConfig.UpdateKeyVal("KEY_Video_Size", string.Format("{0},{1}", size.Width, size.Height));
								}
								(this.frmCaller as frmConsole).photoaviSingle.bWatching = false;
								(this.frmCaller as frmConsole).photoaviSingle.stopVideo();
								(this.frmCaller as frmConsole).photoaviSingle.Close();
							}
							(this.frmCaller as frmConsole).photoaviSingle = null;
							if (!string.IsNullOrEmpty(this.selectedCameraID))
							{
								(this.frmCaller as frmConsole).photoaviSingle = new dfrmPhotoAviSingle();
								if (wgAppConfig.GetKeyVal("KEY_Video_Location") != string.Format("{0},{1}", point.X, point.Y))
								{
									string keyVal3 = wgAppConfig.GetKeyVal("KEY_Video_Location");
									if (!string.IsNullOrEmpty(keyVal3))
									{
										string[] array3 = keyVal3.Split(new char[] { ',' });
										try
										{
											point = new Point(int.Parse(array3[0]), int.Parse(array3[1]));
										}
										catch
										{
										}
									}
								}
								if (wgAppConfig.GetKeyVal("KEY_Video_Size") != string.Format("{0},{1}", size.Width, size.Height))
								{
									string keyVal4 = wgAppConfig.GetKeyVal("KEY_Video_Size");
									if (!string.IsNullOrEmpty(keyVal4))
									{
										string[] array4 = keyVal4.Split(new char[] { ',' });
										try
										{
											size = new Size(int.Parse(array4[0]), int.Parse(array4[1]));
										}
										catch
										{
										}
									}
								}
								(this.frmCaller as frmConsole).photoaviSingle.Location = point;
								(this.frmCaller as frmConsole).photoaviSingle.Size = size;
								(this.frmCaller as frmConsole).photoaviSingle.selectedCameraID = this.selectedCameraID;
								this.prevSelectedCameraID = this.selectedCameraID;
								(this.frmCaller as frmConsole).photoaviSingle.frmCaller = this.frmCaller;
								(this.frmCaller as frmConsole).photoaviSingle.Show(this);
							}
						}
					}
					catch (Exception ex)
					{
						wgAppConfig.wgDebugWrite(ex.ToString(), EventLogEntryType.Error);
					}
				}
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x001FB1B4 File Offset: 0x001FA1B4
		private string selectDoorCamera()
		{
			string text = "";
			try
			{
				this.contr4PCCheckAccess.GetInfoFromDBByDoorName(this.strDoorFullName);
				string text2;
				if (wgMjController.GetControllerType(this.contr4PCCheckAccess.ControllerSN) == 4)
				{
					text2 = "SELECT f_CameraId FROM t_b_CameraTriggerSource,t_b_Reader a, t_b_Door b  where   t_b_CameraTriggerSource.f_readerid = a.f_ReaderID and  a.f_ControllerID=b.f_ControllerID ";
					text2 = text2 + "  AND   b.[f_DoorID] =  " + this.strDoorId.ToString() + "   AND  a.[f_ReaderNO] = b.[f_DoorNO] ";
				}
				else
				{
					text2 = "SELECT f_CameraId FROM t_b_CameraTriggerSource,t_b_Reader a, t_b_Door b  where   t_b_CameraTriggerSource.f_readerid = a.f_ReaderID and  a.f_ControllerID=b.f_ControllerID ";
					text2 = text2 + "  AND   b.[f_DoorID] =  " + this.strDoorId.ToString() + "   AND ( a.[f_ReaderNO] = (b.[f_DoorNO]*2-1)  OR a.[f_ReaderNO] = (b.[f_DoorNO]*2) )";
				}
				string text3 = wgTools.SetObjToStr(wgAppConfig.getValBySql(text2));
				if (!string.IsNullOrEmpty(text3) && int.Parse(text3) > 0)
				{
					text = text3;
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x001FB26C File Offset: 0x001FA26C
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Enabled = false;
			try
			{
				if (this.inputCard.Length >= 8)
				{
					SystemSounds.Beep.Play();
					this.dv.RowFilter = " f_CardNO = " + this.inputCard.ToString();
					if (this.dv.Count > 0)
					{
						if ((int)this.dv[0]["f_CheckAccessActive"] == 0 || (int)this.dv[0]["f_CheckAccessActive"] == 1)
						{
							this.SwipeInsideCnt++;
							this.ConsumerNameInside = this.ConsumerNameInside + (string.IsNullOrEmpty(this.ConsumerNameInside) ? "" : ",") + this.dv[0]["f_ConsumerName"];
						}
						else if (this.inputCardInside.IndexOf(this.inputCard) < 0)
						{
							this.inputCardInside.Add(this.inputCard);
							this.ConsumerNameInside = this.ConsumerNameInside + (string.IsNullOrEmpty(this.ConsumerNameInside) ? "" : ",") + this.dv[0]["f_ConsumerName"];
							this.SwipeInsideCnt++;
						}
						if (this.SwipeInsideCnt >= (int)this.dv[0]["f_CheckAccessActive"])
						{
							this.contr4PCCheckAccess.GetInfoFromDBByDoorName(this.strDoorFullName);
							if (this.contr4PCCheckAccess.RemoteOpenDoorIP(this.strDoorFullName, (uint)icOperator.OperatorID, long.Parse(this.inputCard)) > 0)
							{
								wgRunInfoLog.addEvent(new InfoRow
								{
									desc = string.Format("{0}[{1:d}]", this.strDoorFullName, this.contr4PCCheckAccess.ControllerSN),
									information = string.Format("{0} {1}--[{2}]", this.ConsumerNameInside, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strSendRemoteOpenDoor), this.strConsumername.Replace("\r\n", ","))
								});
							}
							this.strGroupname = "";
							base.Hide();
							this.player.Stop();
							this.bDealing = false;
							this.inputCardInside.Clear();
							this.ConsumerNameInside = "";
							this.SwipeInsideCnt = 0;
						}
					}
					else
					{
						SystemSounds.Beep.Play();
					}
				}
			}
			catch (Exception)
			{
			}
			this.inputCard = "";
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x001FB50C File Offset: 0x001FA50C
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timer2.Enabled = false;
			if (!this.bDealingFinger)
			{
				this.bDealingFinger = true;
				this.usbReaderEnroll();
				this.bDealingFinger = false;
			}
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x001FB538 File Offset: 0x001FA538
		private void usbReaderEnroll()
		{
			try
			{
				string[] portNames = SerialPort.GetPortNames();
				string text = "";
				for (int i = 0; i <= portNames.Length - 1; i++)
				{
					if (string.IsNullOrEmpty(frmADCT3000.portsNotUSB) || frmADCT3000.portsNotUSB.IndexOf("(" + portNames[i] + ")") < 0)
					{
						text = portNames[i];
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_none)
					{
						this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_getImage1;
					}
					int num = 0;
					byte[] array = new byte[1152];
					this.wgSerial.ClosePort();
					string text2 = text;
					this.wgSerial.Baudrate = 57600L;
					int num2;
					if (int.TryParse(text2.Replace("COM", ""), out num2))
					{
						this.wgSerial.CommPort = byte.Parse(text2.Replace("COM", ""));
					}
					else
					{
						this.wgSerial.CommPort = 1;
					}
					this.wgSerial.ClosePort();
					if (this.wgSerial.OpenPort() == 0L)
					{
						while (this.bDealing && !frmADCT3000.bConfirmClose)
						{
							try
							{
								long num3 = (long)this.dealFingerEnroll(ref array);
								if (num3 > 0L)
								{
									this.btnErrConnect.Visible = false;
									num = 0;
									if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_tzMGetOK)
									{
										this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_getImage1;
										if (this.foundIndexNew >= 0)
										{
											int num4 = this.foundIndexNew;
											if (num4 >= 0 && num4 < 1024)
											{
												long num5 = 0L;
												string valStringBySql = wgAppConfig.getValStringBySql(string.Format("SELECT t_b_Consumer.f_CardNO From t_b_Consumer_Fingerprint,t_b_Consumer WHERE t_b_Consumer_Fingerprint.f_ConsumerID= t_b_Consumer.f_ConsumerID and f_FingerNO ={0}", num4 + 1));
												if (!string.IsNullOrEmpty(valStringBySql))
												{
													long.TryParse(valStringBySql, out num5);
												}
												try
												{
													if (num5 > 0L)
													{
														SystemSounds.Beep.Play();
														this.dv.RowFilter = " f_CardNO = " + num5.ToString();
														if (this.dv.Count > 0)
														{
															if ((int)this.dv[0]["f_CheckAccessActive"] == 0 || (int)this.dv[0]["f_CheckAccessActive"] == 1)
															{
																this.SwipeInsideCnt++;
																this.ConsumerNameInside = this.ConsumerNameInside + (string.IsNullOrEmpty(this.ConsumerNameInside) ? "" : ",") + this.dv[0]["f_ConsumerName"];
															}
															else if (this.inputCardInside.IndexOf(num5) < 0)
															{
																this.inputCardInside.Add(num5);
																this.ConsumerNameInside = this.ConsumerNameInside + (string.IsNullOrEmpty(this.ConsumerNameInside) ? "" : ",") + this.dv[0]["f_ConsumerName"];
																this.SwipeInsideCnt++;
															}
															if (this.SwipeInsideCnt >= (int)this.dv[0]["f_CheckAccessActive"])
															{
																this.contr4PCCheckAccess.GetInfoFromDBByDoorName(this.strDoorFullName);
																if (this.contr4PCCheckAccess.RemoteOpenDoorIP(this.strDoorFullName, (uint)icOperator.OperatorID, num5) > 0)
																{
																	wgRunInfoLog.addEvent(new InfoRow
																	{
																		desc = string.Format("{0}[{1:d}]", this.strDoorFullName, this.contr4PCCheckAccess.ControllerSN),
																		information = string.Format("{0} {1}--[{2}]", this.ConsumerNameInside, wgAppConfig.ReplaceRemoteOpenDoor(CommonStr.strSendRemoteOpenDoor), this.strConsumername.Replace("\r\n", ","))
																	});
																}
																this.strGroupname = "";
																base.Hide();
																this.player.Stop();
																this.bDealing = false;
																this.inputCardInside.Clear();
																this.ConsumerNameInside = "";
																this.SwipeInsideCnt = 0;
																this.fingerEnrollStatus = dfrmPCCheckAccess.finger_status.fs_none;
															}
														}
														else
														{
															SystemSounds.Beep.Play();
														}
													}
												}
												catch (Exception)
												{
												}
											}
										}
									}
								}
								else
								{
									num++;
									if (num > 5)
									{
										this.btnErrConnect.Visible = true;
									}
								}
								Application.DoEvents();
								if (this.fingerEnrollStatus == dfrmPCCheckAccess.finger_status.fs_none)
								{
									break;
								}
							}
							catch (Exception ex)
							{
								wgAppConfig.wgDebugWrite(ex.ToString());
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				wgAppConfig.wgDebugWrite(ex2.ToString());
			}
			finally
			{
				this.wgSerial.ClosePort();
			}
		}

		// Token: 0x040031C4 RID: 12740
		private const int RBUF_LEN = 16;

		// Token: 0x040031C5 RID: 12741
		private bool bDealingFinger;

		// Token: 0x040031C6 RID: 12742
		private DataView dv;

		// Token: 0x040031C7 RID: 12743
		private dfrmPCCheckAccess.finger_status fingerEnrollStatus;

		// Token: 0x040031C8 RID: 12744
		private SoundPlayer player;

		// Token: 0x040031C9 RID: 12745
		private int SwipeInsideCnt;

		// Token: 0x040031CA RID: 12746
		private string wavfile;

		// Token: 0x040031CB RID: 12747
		private ArrayList arrReaderId4Camera = new ArrayList();

		// Token: 0x040031CC RID: 12748
		private byte[] BAGHEAD = new byte[] { 239, 1, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue };

		// Token: 0x040031CD RID: 12749
		public bool bDealing;

		// Token: 0x040031CE RID: 12750
		private bool bFalseShowOnce = true;

		// Token: 0x040031D0 RID: 12752
		private string ConsumerNameInside = "";

		// Token: 0x040031D2 RID: 12754
		private DataSet ds = new DataSet();

		// Token: 0x040031D3 RID: 12755
		private int existedVideoCHDll = -1;

		// Token: 0x040031D4 RID: 12756
		private int foundIndexNew = -1;

		// Token: 0x040031D5 RID: 12757
		public Form frmCaller;

		// Token: 0x040031D7 RID: 12759
		private string inputCard = "";

		// Token: 0x040031D8 RID: 12760
		private DateTime inputCardDate = DateTime.Now;

		// Token: 0x040031D9 RID: 12761
		private ArrayList inputCardInside = new ArrayList();

		// Token: 0x040031DA RID: 12762
		private string prevSelectedCameraID = "";

		// Token: 0x040031DB RID: 12763
		private byte[] RBF = new byte[16];

		// Token: 0x040031DC RID: 12764
		private string selectedCameraID = "";

		// Token: 0x040031DD RID: 12765
		public string strConsumername;

		// Token: 0x040031DE RID: 12766
		public string strDoorFullName;

		// Token: 0x040031DF RID: 12767
		public string strDoorId;

		// Token: 0x040031E0 RID: 12768
		public string strGroupname;

		// Token: 0x040031E1 RID: 12769
		public string strNow;

		// Token: 0x040031E2 RID: 12770
		private wgSerialComm wgSerial = new wgSerialComm();

		// Token: 0x02000317 RID: 791
		private enum finger_status
		{
			// Token: 0x040031F1 RID: 12785
			fs_ClearAll = 128,
			// Token: 0x040031F2 RID: 12786
			fs_DownloadAll = 130,
			// Token: 0x040031F3 RID: 12787
			fs_END = 255,
			// Token: 0x040031F4 RID: 12788
			fs_getImage1 = 1,
			// Token: 0x040031F5 RID: 12789
			fs_getImage1b = 18,
			// Token: 0x040031F6 RID: 12790
			fs_getImage2 = 3,
			// Token: 0x040031F7 RID: 12791
			fs_getImage2b = 50,
			// Token: 0x040031F8 RID: 12792
			fs_none = 0,
			// Token: 0x040031F9 RID: 12793
			fs_tzM1 = 2,
			// Token: 0x040031FA RID: 12794
			fs_tzM2 = 4,
			// Token: 0x040031FB RID: 12795
			fs_tzMCreate,
			// Token: 0x040031FC RID: 12796
			fs_tzMGetOK = 16,
			// Token: 0x040031FD RID: 12797
			fs_tzMSearch = 7,
			// Token: 0x040031FE RID: 12798
			fs_tzMWaitGet = 6,
			// Token: 0x040031FF RID: 12799
			fs_UploadAll = 129
		}
	}
}
