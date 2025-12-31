using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002DC RID: 732
	public partial class Snapshot : frmN3000
	{
		// Token: 0x060014A3 RID: 5283 RVA: 0x0019728A File Offset: 0x0019628A
		public Snapshot()
		{
			this.InitializeComponent();
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x001972C0 File Offset: 0x001962C0
		private void connectButton_Click(object sender, EventArgs e)
		{
			if (this.videoDevice != null)
			{
				if (this.videoCapabilities != null && this.videoCapabilities.Length != 0)
				{
					this.videoDevice.VideoResolution = this.videoCapabilities[this.videoResolutionsCombo.SelectedIndex];
					this.videoDevice.ProvideSnapshots = true;
					this.videoDevice.SnapshotResolution = this.videoCapabilities[this.videoResolutionsCombo.SelectedIndex];
					this.videoDevice.SnapshotFrame += new NewFrameEventHandler(this.videoDevice_SnapshotFrame);
				}
				this.EnableConnectionControls(false);
				this.videoSourcePlayer.VideoSource = this.videoDevice;
				this.videoSourcePlayer.Start();
			}
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0019736C File Offset: 0x0019636C
		private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.videoDevices.Count != 0)
			{
				this.videoDevice = new VideoCaptureDevice(this.videoDevices[this.devicesCombo.SelectedIndex].MonikerString);
				this.EnumeratedSupportedFrameSizes(this.videoDevice);
			}
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x001973B8 File Offset: 0x001963B8
		private void Disconnect()
		{
			if (this.videoSourcePlayer.VideoSource != null)
			{
				this.videoSourcePlayer.SignalToStop();
				this.videoSourcePlayer.WaitForStop();
				this.videoSourcePlayer.VideoSource = null;
				if (this.videoDevice.ProvideSnapshots)
				{
					this.videoDevice.SnapshotFrame -= new NewFrameEventHandler(this.videoDevice_SnapshotFrame);
				}
				this.EnableConnectionControls(true);
			}
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0019741F File Offset: 0x0019641F
		private void disconnectButton_Click(object sender, EventArgs e)
		{
			this.Disconnect();
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00197428 File Offset: 0x00196428
		private void EnableConnectionControls(bool enable)
		{
			this.devicesCombo.Enabled = enable;
			this.videoResolutionsCombo.Enabled = enable;
			this.snapshotResolutionsCombo.Enabled = enable;
			this.connectButton.Enabled = enable;
			this.disconnectButton.Enabled = !enable;
			this.triggerButton.Enabled = !enable;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00197484 File Offset: 0x00196484
		private void EnumeratedSupportedFrameSizes(VideoCaptureDevice videoDevice)
		{
			this.Cursor = Cursors.WaitCursor;
			this.videoResolutionsCombo.Items.Clear();
			this.snapshotResolutionsCombo.Items.Clear();
			try
			{
				this.videoCapabilities = videoDevice.VideoCapabilities;
				this.snapshotCapabilities = videoDevice.SnapshotCapabilities;
				foreach (VideoCapabilities videoCapabilities in this.videoCapabilities)
				{
					ComboBox.ObjectCollection items = this.videoResolutionsCombo.Items;
					string text = "{0} x {1}";
					Size frameSize = videoCapabilities.FrameSize;
					object obj = frameSize.Width;
					Size frameSize2 = videoCapabilities.FrameSize;
					items.Add(string.Format(text, obj, frameSize2.Height));
				}
				foreach (VideoCapabilities videoCapabilities2 in this.snapshotCapabilities)
				{
					ComboBox.ObjectCollection items2 = this.snapshotResolutionsCombo.Items;
					string text2 = "{0} x {1}";
					Size frameSize3 = videoCapabilities2.FrameSize;
					object obj2 = frameSize3.Width;
					Size frameSize4 = videoCapabilities2.FrameSize;
					items2.Add(string.Format(text2, obj2, frameSize4.Height));
				}
				if (this.videoCapabilities.Length == 0)
				{
					this.videoResolutionsCombo.Items.Add("Not supported");
				}
				if (this.snapshotCapabilities.Length == 0)
				{
					this.snapshotResolutionsCombo.Items.Add("Not supported");
				}
				this.videoResolutionsCombo.SelectedIndex = 0;
				this.snapshotResolutionsCombo.SelectedIndex = 0;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00197618 File Offset: 0x00196618
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Disconnect();
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00197620 File Offset: 0x00196620
		private void MainForm_Load(object sender, EventArgs e)
		{
			this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if (this.videoDevices.Count != 0)
			{
				using (IEnumerator enumerator = this.videoDevices.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						FilterInfo filterInfo = (FilterInfo)obj;
						this.devicesCombo.Items.Add(filterInfo.Name);
					}
					goto IL_0096;
				}
			}
			this.devicesCombo.Items.Add("No DirectShow devices found");
			this.triggerButton.Text = CommonStr.strCameraNotFound;
			IL_0096:
			this.devicesCombo.SelectedIndex = 0;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			string keyVal = wgAppConfig.GetKeyVal("SNAPSHOT_DEVICE");
			if (!string.IsNullOrEmpty(keyVal))
			{
				int i = 0;
				while (i < this.devicesCombo.Items.Count)
				{
					if (this.devicesCombo.Items[i].Equals(keyVal))
					{
						this.devicesCombo.SelectedIndex = i;
						wgAppConfig.wgLog("devicesCombo.SelectedIndex = " + i.ToString());
						if (this.videoDevices.Count != 0)
						{
							this.videoDevice = new VideoCaptureDevice(this.videoDevices[this.devicesCombo.SelectedIndex].MonikerString);
							this.EnumeratedSupportedFrameSizes(this.videoDevice);
							string text = wgAppConfig.GetKeyVal("SNAPSHOT_DEVICE_RESOLUTION");
							if (string.IsNullOrEmpty(text))
							{
								text = "640 x 480";
							}
							for (int j = 0; j < this.videoResolutionsCombo.Items.Count; j++)
							{
								if (text.Equals(this.videoResolutionsCombo.Items[j]))
								{
									this.videoResolutionsCombo.SelectedIndex = j;
									break;
								}
							}
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			this.EnableConnectionControls(true);
			this.connectButton_Click(null, null);
			base.TopMost = true;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00197820 File Offset: 0x00196820
		private void ShowSnapshot(Bitmap snapshot)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new Action<Bitmap>(this.ShowSnapshot), new object[] { snapshot });
			}
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00197854 File Offset: 0x00196854
		private void Snapshot_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Disconnect();
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0019785C File Offset: 0x0019685C
		private void snapshotForm_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00197860 File Offset: 0x00196860
		private void triggerButton_Click(object sender, EventArgs e)
		{
			if (this.videoDevice != null && this.videoDevice.ProvideSnapshots)
			{
				try
				{
					int num = 0;
					num++;
					string.Concat(new object[] { this.g_s_RequestNo, "-", num, ".bmp" });
					string text = this.g_s_AutoSavePath + this.g_s_RequestNo + "\\";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					Bitmap currentVideoFrame = this.videoSourcePlayer.GetCurrentVideoFrame();
					if (currentVideoFrame == null)
					{
						XMessageBox.Show(this.triggerButton.Text + " " + CommonStr.strFailed);
					}
					else
					{
						if (!Directory.Exists(Application.StartupPath + "\\Photo"))
						{
							Directory.CreateDirectory(Application.StartupPath + "\\Photo");
						}
						if (!Directory.Exists(Application.StartupPath + "\\Photo\\newphoto"))
						{
							Directory.CreateDirectory(Application.StartupPath + "\\Photo\\newphoto");
						}
						string text2 = Application.StartupPath + "\\Photo\\newphoto\\123.jpg";
						int width = this.panel1.Width;
						int height = this.panel1.Height;
						int num2 = (640 - width) / 2;
						int num3 = (480 - height) / 2;
						Application.StartupPath + "\\Photo\\newphoto\\123b.jpg";
						Bitmap bitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
						Graphics graphics = Graphics.FromImage(bitmap);
						graphics.DrawImage(currentVideoFrame, 0, 0, new Rectangle(num2, num3, width, height), GraphicsUnit.Pixel);
						Image image = Image.FromHbitmap(bitmap.GetHbitmap());
						image.Save(text2, ImageFormat.Jpeg);
						image.Dispose();
						graphics.Dispose();
						bitmap.Dispose();
						currentVideoFrame.Dispose();
						string keyVal = wgAppConfig.GetKeyVal("SNAPSHOT_DEVICE");
						string keyVal2 = wgAppConfig.GetKeyVal("SNAPSHOT_DEVICE_RESOLUTION");
						string text3 = (string)this.devicesCombo.Items[this.devicesCombo.SelectedIndex];
						if (string.IsNullOrEmpty(keyVal) || !keyVal.Equals(text3))
						{
							wgAppConfig.UpdateKeyVal("SNAPSHOT_DEVICE", text3);
						}
						string text4 = this.videoResolutionsCombo.Text;
						if (string.IsNullOrEmpty(keyVal2) || !keyVal2.Equals(text4))
						{
							wgAppConfig.UpdateKeyVal("SNAPSHOT_DEVICE_RESOLUTION", text4);
						}
					}
				}
				catch (Exception ex)
				{
					XMessageBox.Show(string.Concat(new string[]
					{
						this.triggerButton.Text,
						" ",
						CommonStr.strFailed,
						"\r\n",
						ex.Message
					}));
				}
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00197B30 File Offset: 0x00196B30
		private void videoDevice_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
		{
			Console.WriteLine(eventArgs.Frame.Size);
			this.ShowSnapshot((Bitmap)eventArgs.Frame.Clone());
		}

		// Token: 0x04002B3C RID: 11068
		private VideoCaptureDevice videoDevice;

		// Token: 0x04002B3D RID: 11069
		private FilterInfoCollection videoDevices;

		// Token: 0x04002B3E RID: 11070
		private string g_s_AutoSavePath = AppDomain.CurrentDomain.BaseDirectory + "Capture\\";

		// Token: 0x04002B3F RID: 11071
		private string g_s_RequestNo = "atest";

		// Token: 0x04002B40 RID: 11072
		private VideoCapabilities[] snapshotCapabilities;

		// Token: 0x04002B41 RID: 11073
		private VideoCapabilities[] videoCapabilities;
	}
}
