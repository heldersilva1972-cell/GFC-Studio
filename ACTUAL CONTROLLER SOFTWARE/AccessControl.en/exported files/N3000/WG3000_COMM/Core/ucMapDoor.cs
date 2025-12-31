using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WG3000_COMM.Properties;

namespace WG3000_COMM.Core
{
	// Token: 0x020001E4 RID: 484
	public class ucMapDoor : UserControl
	{
		// Token: 0x06000B2A RID: 2858 RVA: 0x000EA798 File Offset: 0x000E9798
		public ucMapDoor()
		{
			this.InitializeComponent();
			this.txtDoorName.Text = this.m_doorName;
			this.doorLocation = base.Location;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x000EA7FF File Offset: 0x000E97FF
		private void picDoorState_MouseDown(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x000EA804 File Offset: 0x000E9804
		public void redraw()
		{
			try
			{
				this.picDoorState.Size = new Size(new Point((int)(24f * this.m_mapScale * this.m_doorScale), (int)(32f * this.m_mapScale * this.m_doorScale)));
				this.txtDoorName.Text = this.m_doorName;
				this.txtDoorName.Font = new Font("Arial", 9f * this.m_mapScale);
				this.txtDoorName.Location = new Point(0, this.picDoorState.Location.Y + this.picDoorState.Size.Height + 2);
				base.Location = new Point((int)((float)this.m_doorLocation.X * this.m_mapScale), (int)((float)this.m_doorLocation.Y * this.m_mapScale));
				base.Size = new Size(new Point(Math.Max(this.txtDoorName.Width, this.picDoorState.Size.Width), (int)((32f * this.doorScale + 16f) * this.m_mapScale)));
				this.picDoorState.Location = new Point((base.Size.Width - this.picDoorState.Width) / 2, 0);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000EA988 File Offset: 0x000E9988
		private void Timer1_Tick(object sender, EventArgs e)
		{
			this.picDoorState.Visible = !this.picDoorState.Visible;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000EA9A3 File Offset: 0x000E99A3
		private void ucMapDoor_Click(object sender, EventArgs e)
		{
			this.txtDoorName.ForeColor = Color.White;
			this.txtDoorName.BackColor = Color.DodgerBlue;
			base.ActiveControl = this.txtDoorName;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000EA9D1 File Offset: 0x000E99D1
		private void ucMapDoor_Leave(object sender, EventArgs e)
		{
			this.txtDoorName.ForeColor = Color.Black;
			this.txtDoorName.BackColor = Color.White;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000EA9F3 File Offset: 0x000E99F3
		private void ucMapDoor_Load(object sender, EventArgs e)
		{
			this.redraw();
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x000EA9FB File Offset: 0x000E99FB
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x000EAA03 File Offset: 0x000E9A03
		public PictureBox bindSource
		{
			get
			{
				return this.m_bindSource;
			}
			set
			{
				this.m_bindSource = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x000EAA0C File Offset: 0x000E9A0C
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x000EAA14 File Offset: 0x000E9A14
		public Point doorLocation
		{
			get
			{
				return this.m_doorLocation;
			}
			set
			{
				this.m_doorLocation = new Point((int)((float)value.X / this.mapScale), (int)((float)value.Y / this.mapScale));
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x000EAA41 File Offset: 0x000E9A41
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x000EAA49 File Offset: 0x000E9A49
		public string doorName
		{
			get
			{
				return this.m_doorName;
			}
			set
			{
				if (value != null && this.m_doorName != value)
				{
					this.m_doorName = value;
					this.txtDoorName.Text = this.m_doorName;
					this.redraw();
				}
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x000EAA7A File Offset: 0x000E9A7A
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x000EAA82 File Offset: 0x000E9A82
		public float doorScale
		{
			get
			{
				return this.m_doorScale;
			}
			set
			{
				if (this.m_doorScale != value)
				{
					this.m_doorScale = value;
					this.redraw();
				}
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x000EAA9A File Offset: 0x000E9A9A
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x000EAAA4 File Offset: 0x000E9AA4
		public int doorStatus
		{
			get
			{
				return this.m_doorStatus;
			}
			set
			{
				if (this.m_doorStatus != value)
				{
					this.m_doorStatus = 0;
					this.m_doorStatus = value;
					switch (this.m_doorStatus)
					{
					case 0:
						this.picDoorState.Image = Resources.pConsole_Door_Unknown;
						break;
					case 1:
						this.picDoorState.Image = Resources.pConsole_Door_NormalClose;
						break;
					case 2:
						if (!this.bActivateDisplayYellowWhenDoorOpen)
						{
							this.picDoorState.Image = Resources.pConsole_Door_NormalOpen;
						}
						else
						{
							this.picDoorState.Image = Resources.pConsole_Door_NormalOpenYellow;
						}
						break;
					case 3:
						this.picDoorState.Image = Resources.pConsole_Door_NotConnected;
						break;
					case 4:
						this.picDoorState.Image = Resources.pConsole_Door_WarnClose;
						break;
					case 5:
						this.picDoorState.Image = Resources.pConsole_Door_WarnOpen;
						break;
					case 6:
						this.picDoorState.Image = Resources.pConsole_Door_Unknown;
						break;
					case 7:
						this.picDoorState.Image = Resources.pConsole_Door_WarnClose;
						break;
					case 8:
						this.picDoorState.Image = Resources.pConsole_Door_WarnOpen;
						break;
					case 9:
						this.picDoorState.Image = Resources.pConsole_Door_NotConnected;
						break;
					default:
						this.picDoorState.Image = Resources.pConsole_Door_Unknown;
						break;
					}
					if (this.m_doorStatus == 4 || this.m_doorStatus == 5)
					{
						this.Timer1.Enabled = true;
						return;
					}
					this.Timer1.Enabled = false;
					this.picDoorState.Visible = true;
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x000EAC26 File Offset: 0x000E9C26
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x000EAC2E File Offset: 0x000E9C2E
		public float mapScale
		{
			get
			{
				return this.m_mapScale;
			}
			set
			{
				if (this.m_mapScale != value)
				{
					this.m_mapScale = value;
					this.redraw();
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000EAC46 File Offset: 0x000E9C46
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000EAC68 File Offset: 0x000E9C68
		private void InitializeComponent()
		{
			this.components = new Container();
			this.picDoorState = new PictureBox();
			this.imgDoor = new ImageList(this.components);
			this.txtDoorName = new Label();
			this.Timer1 = new Timer(this.components);
			((ISupportInitialize)this.picDoorState).BeginInit();
			base.SuspendLayout();
			this.picDoorState.Enabled = false;
			this.picDoorState.Image = Resources.pConsole_Door_Unknown;
			this.picDoorState.Location = new Point(0, 0);
			this.picDoorState.Name = "picDoorState";
			this.picDoorState.Size = new Size(24, 32);
			this.picDoorState.SizeMode = PictureBoxSizeMode.StretchImage;
			this.picDoorState.TabIndex = 0;
			this.picDoorState.TabStop = false;
			this.picDoorState.Leave += this.ucMapDoor_Leave;
			this.picDoorState.MouseDown += this.picDoorState_MouseDown;
			this.imgDoor.ColorDepth = ColorDepth.Depth16Bit;
			this.imgDoor.ImageSize = new Size(24, 32);
			this.imgDoor.TransparentColor = Color.Transparent;
			this.txtDoorName.AutoSize = true;
			this.txtDoorName.BackColor = Color.White;
			this.txtDoorName.Location = new Point(-2, 36);
			this.txtDoorName.Name = "txtDoorName";
			this.txtDoorName.Size = new Size(29, 12);
			this.txtDoorName.TabIndex = 1;
			this.txtDoorName.Text = "Name";
			this.txtDoorName.TextAlign = ContentAlignment.TopCenter;
			this.txtDoorName.Click += this.ucMapDoor_Click;
			this.Timer1.Interval = 500;
			this.Timer1.Tick += this.Timer1_Tick;
			this.AllowDrop = true;
			this.BackColor = Color.Transparent;
			base.Controls.Add(this.txtDoorName);
			base.Controls.Add(this.picDoorState);
			this.ForeColor = Color.Black;
			base.Name = "ucMapDoor";
			base.Size = new Size(32, 50);
			base.Load += this.ucMapDoor_Load;
			base.Click += this.ucMapDoor_Click;
			base.Leave += this.ucMapDoor_Leave;
			((ISupportInitialize)this.picDoorState).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040019ED RID: 6637
		private const int CONTROL_HEIGHT = 50;

		// Token: 0x040019EE RID: 6638
		private const int FONT_HEIGHT = 16;

		// Token: 0x040019EF RID: 6639
		private const int FONTSIZE = 9;

		// Token: 0x040019F0 RID: 6640
		private const int IMG_HEIGHT = 32;

		// Token: 0x040019F1 RID: 6641
		private const int IMG_WIDTH = 24;

		// Token: 0x040019F2 RID: 6642
		private Point m_doorLocation;

		// Token: 0x040019F3 RID: 6643
		private int m_doorStatus;

		// Token: 0x040019F4 RID: 6644
		private bool bActivateDisplayYellowWhenDoorOpen = wgAppConfig.getParamValBoolByNO(173);

		// Token: 0x040019F5 RID: 6645
		public int idoorWarnSource;

		// Token: 0x040019F6 RID: 6646
		private string m_doorName = "门名称";

		// Token: 0x040019F7 RID: 6647
		private float m_doorScale = 1f;

		// Token: 0x040019F8 RID: 6648
		private float m_mapScale = 1f;

		// Token: 0x040019F9 RID: 6649
		private IContainer components;

		// Token: 0x040019FA RID: 6650
		private PictureBox m_bindSource;

		// Token: 0x040019FB RID: 6651
		public ImageList imgDoor;

		// Token: 0x040019FC RID: 6652
		public PictureBox picDoorState;

		// Token: 0x040019FD RID: 6653
		internal Timer Timer1;

		// Token: 0x040019FE RID: 6654
		internal Label txtDoorName;
	}
}
