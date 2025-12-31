using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;

namespace WG3000_COMM.ExtendFunc.TCPServer
{
	// Token: 0x02000326 RID: 806
	public class TcpServer : Component
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600194E RID: 6478 RVA: 0x00215894 File Offset: 0x00214894
		// (remove) Token: 0x0600194F RID: 6479 RVA: 0x002158CC File Offset: 0x002148CC
		public event tcpServerConnectionChanged OnConnect;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001950 RID: 6480 RVA: 0x00215904 File Offset: 0x00214904
		// (remove) Token: 0x06001951 RID: 6481 RVA: 0x0021593C File Offset: 0x0021493C
		public event tcpServerConnectionChanged OnDataAvailable;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001952 RID: 6482 RVA: 0x00215974 File Offset: 0x00214974
		// (remove) Token: 0x06001953 RID: 6483 RVA: 0x002159AC File Offset: 0x002149AC
		public event tcpServerError OnError;

		// Token: 0x06001954 RID: 6484 RVA: 0x002159E1 File Offset: 0x002149E1
		public TcpServer()
		{
			this.activeThreadsLock = new object();
			this.InitializeComponent();
			this.initialise();
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00215A00 File Offset: 0x00214A00
		public TcpServer(IContainer container)
		{
			this.activeThreadsLock = new object();
			container.Add(this);
			this.InitializeComponent();
			this.initialise();
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00215A28 File Offset: 0x00214A28
		public void Close()
		{
			if (this.m_isOpen)
			{
				lock (this)
				{
					this.m_isOpen = false;
					foreach (TcpServerConnection tcpServerConnection in this.connections)
					{
						tcpServerConnection.forceDisconnect();
					}
					try
					{
						if (this.listenThread.IsAlive)
						{
							this.listenThread.Interrupt();
							Thread.Sleep(1);
							if (this.listenThread.IsAlive)
							{
								this.listenThread.Abort();
							}
						}
					}
					catch (SecurityException)
					{
					}
					try
					{
						if (this.sendThread.IsAlive)
						{
							this.sendThread.Interrupt();
							Thread.Sleep(1);
							if (this.sendThread.IsAlive)
							{
								this.sendThread.Abort();
							}
						}
					}
					catch (SecurityException)
					{
					}
				}
				this.listener.Stop();
				lock (this.connections)
				{
					this.connections.Clear();
				}
				this.listenThread = null;
				this.sendThread = null;
				GC.Collect();
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00215B84 File Offset: 0x00214B84
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00215BA4 File Offset: 0x00214BA4
		private void initialise()
		{
			this.connections = new List<TcpServerConnection>();
			this.listener = null;
			this.listenThread = null;
			this.sendThread = null;
			this.m_port = -1;
			this.m_maxSendAttempts = 3;
			this.m_isOpen = false;
			this.m_idleTime = 50;
			this.m_maxCallbackThreads = 100;
			this.m_verifyConnectionInterval = 100;
			this.m_encoding = Encoding.ASCII;
			this.sem = new Semaphore(0, 100);
			this.waiting = false;
			this.activeThreads = 0;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00215C25 File Offset: 0x00214C25
		private void InitializeComponent()
		{
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00215C28 File Offset: 0x00214C28
		public void Open()
		{
			lock (this)
			{
				if (!this.m_isOpen)
				{
					if (this.m_port < 0)
					{
						throw new Exception("Invalid port");
					}
					try
					{
						this.listener.Start(5);
					}
					catch (Exception)
					{
						this.listener.Stop();
						this.listener = new TcpListener(IPAddress.Any, this.m_port);
						this.listener.Start(5);
					}
					this.m_isOpen = true;
					this.listenThread = new Thread(new ThreadStart(this.runListener));
					this.listenThread.Start();
					this.sendThread = new Thread(new ThreadStart(this.runSender));
					this.sendThread.Start();
				}
			}
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00215D2C File Offset: 0x00214D2C
		private bool processConnection(TcpServerConnection conn)
		{
			ThreadStart threadStart = null;
			bool flag = false;
			if (conn.processOutgoing(this.m_maxSendAttempts))
			{
				flag = true;
			}
			if (this.OnDataAvailable != null && this.activeThreads < this.m_maxCallbackThreads && conn.Socket.Available > 0)
			{
				lock (this.activeThreadsLock)
				{
					this.activeThreads++;
				}
				if (threadStart == null)
				{
					threadStart = delegate
					{
						this.OnDataAvailable(conn);
					};
				}
				conn.CallbackThread = new Thread(threadStart);
				conn.CallbackThread.Start();
				Thread.Sleep(1);
			}
			return flag;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00215E28 File Offset: 0x00214E28
		private void runListener()
		{
			while (this.m_isOpen && this.m_port >= 0)
			{
				try
				{
					if (this.listener.Pending())
					{
						ThreadStart threadStart = null;
						TcpClient tcpClient = this.listener.AcceptTcpClient();
						TcpServerConnection conn = new TcpServerConnection(tcpClient, this.m_encoding);
						if (this.OnConnect != null)
						{
							lock (this.activeThreadsLock)
							{
								this.activeThreads++;
							}
							if (threadStart == null)
							{
								threadStart = delegate
								{
									this.OnConnect(conn);
								};
							}
							conn.CallbackThread = new Thread(threadStart);
							conn.CallbackThread.Start();
						}
						lock (this.connections)
						{
							this.connections.Add(conn);
							continue;
						}
					}
					Thread.Sleep(this.m_idleTime);
				}
				catch (ThreadInterruptedException)
				{
				}
				catch (Exception ex)
				{
					if (this.m_isOpen && this.OnError != null)
					{
						this.OnError(this, ex);
					}
				}
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00215F80 File Offset: 0x00214F80
		private void runSender()
		{
			while (this.m_isOpen && this.m_port >= 0)
			{
				try
				{
					bool flag = false;
					for (int i = 0; i < this.connections.Count; i++)
					{
						if (this.connections[i].CallbackThread != null)
						{
							try
							{
								this.connections[i].CallbackThread = null;
								lock (this.activeThreadsLock)
								{
									this.activeThreads--;
								}
							}
							catch (Exception)
							{
							}
						}
						if (this.connections[i].CallbackThread == null)
						{
							if (this.connections[i].connected() && (this.connections[i].LastVerifyTime.AddMilliseconds((double)this.m_verifyConnectionInterval) > DateTime.UtcNow || this.connections[i].verifyConnected()))
							{
								flag = flag || this.processConnection(this.connections[i]);
							}
							else
							{
								lock (this.connections)
								{
									this.connections.RemoveAt(i);
									i--;
								}
							}
						}
					}
					if (!flag)
					{
						Thread.Sleep(1);
						lock (this.sem)
						{
							foreach (TcpServerConnection tcpServerConnection in this.connections)
							{
								if (tcpServerConnection.hasMoreWork())
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							this.waiting = true;
							this.sem.WaitOne(this.m_idleTime);
							this.waiting = false;
						}
					}
				}
				catch (ThreadInterruptedException)
				{
				}
				catch (Exception ex)
				{
					if (this.m_isOpen && this.OnError != null)
					{
						this.OnError(this, ex);
					}
				}
			}
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00216218 File Offset: 0x00215218
		public void Send(string data)
		{
			lock (this.sem)
			{
				foreach (TcpServerConnection tcpServerConnection in this.connections)
				{
					tcpServerConnection.sendData(data);
				}
				Thread.Sleep(1);
				if (this.waiting)
				{
					this.sem.Release();
					this.waiting = false;
				}
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x002162B0 File Offset: 0x002152B0
		public void Send(string data, TcpServerConnection conn)
		{
			lock (this.sem)
			{
				conn.sendData(data);
				Thread.Sleep(1);
				if (this.waiting)
				{
					this.sem.Release();
					this.waiting = false;
				}
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0021630C File Offset: 0x0021530C
		public void Send(byte[] data, TcpServerConnection conn)
		{
			lock (this.sem)
			{
				conn.sendData(data);
				Thread.Sleep(1);
				if (this.waiting)
				{
					this.sem.Release();
					this.waiting = false;
				}
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00216368 File Offset: 0x00215368
		public void setEncoding(Encoding encoding, bool changeAllClients)
		{
			this.m_encoding = encoding;
			if (changeAllClients)
			{
				foreach (TcpServerConnection tcpServerConnection in this.connections)
				{
					tcpServerConnection.Encoding = this.m_encoding;
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x002163CC File Offset: 0x002153CC
		public List<TcpServerConnection> Connections
		{
			get
			{
				List<TcpServerConnection> list = new List<TcpServerConnection>();
				list.AddRange(this.connections);
				return list;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x002163EC File Offset: 0x002153EC
		// (set) Token: 0x06001964 RID: 6500 RVA: 0x002163F4 File Offset: 0x002153F4
		public Encoding Encoding
		{
			get
			{
				return this.m_encoding;
			}
			set
			{
				Encoding encoding = this.m_encoding;
				this.m_encoding = value;
				foreach (TcpServerConnection tcpServerConnection in this.connections)
				{
					if (tcpServerConnection.Encoding == encoding)
					{
						tcpServerConnection.Encoding = this.m_encoding;
					}
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x00216464 File Offset: 0x00215464
		// (set) Token: 0x06001966 RID: 6502 RVA: 0x0021646C File Offset: 0x0021546C
		public int IdleTime
		{
			get
			{
				return this.m_idleTime;
			}
			set
			{
				this.m_idleTime = value;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00216475 File Offset: 0x00215475
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x0021647D File Offset: 0x0021547D
		[Browsable(false)]
		public bool IsOpen
		{
			get
			{
				return this.m_isOpen;
			}
			set
			{
				if (this.m_isOpen != value)
				{
					if (value)
					{
						this.Open();
						return;
					}
					this.Close();
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00216498 File Offset: 0x00215498
		// (set) Token: 0x0600196A RID: 6506 RVA: 0x002164A0 File Offset: 0x002154A0
		public int MaxCallbackThreads
		{
			get
			{
				return this.m_maxCallbackThreads;
			}
			set
			{
				this.m_maxCallbackThreads = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x002164A9 File Offset: 0x002154A9
		// (set) Token: 0x0600196C RID: 6508 RVA: 0x002164B1 File Offset: 0x002154B1
		public int MaxSendAttempts
		{
			get
			{
				return this.m_maxSendAttempts;
			}
			set
			{
				this.m_maxSendAttempts = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x002164BA File Offset: 0x002154BA
		// (set) Token: 0x0600196E RID: 6510 RVA: 0x002164C4 File Offset: 0x002154C4
		public int Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (value >= 0 && this.m_port != value)
				{
					if (this.m_isOpen)
					{
						throw new Exception("Invalid attempt to change port while still open.\nPlease close port before changing.");
					}
					this.m_port = value;
					if (this.listener == null)
					{
						this.listener = new TcpListener(IPAddress.Any, this.m_port);
						return;
					}
					this.listener.Server.Bind(new IPEndPoint(IPAddress.Any, this.m_port));
				}
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00216537 File Offset: 0x00215537
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x0021653F File Offset: 0x0021553F
		public int VerifyConnectionInterval
		{
			get
			{
				return this.m_verifyConnectionInterval;
			}
			set
			{
				this.m_verifyConnectionInterval = value;
			}
		}

		// Token: 0x04003428 RID: 13352
		private int activeThreads;

		// Token: 0x04003429 RID: 13353
		private object activeThreadsLock;

		// Token: 0x0400342A RID: 13354
		private IContainer components;

		// Token: 0x0400342B RID: 13355
		private List<TcpServerConnection> connections;

		// Token: 0x0400342C RID: 13356
		private TcpListener listener;

		// Token: 0x0400342D RID: 13357
		private Thread listenThread;

		// Token: 0x0400342E RID: 13358
		private Encoding m_encoding;

		// Token: 0x0400342F RID: 13359
		private int m_idleTime;

		// Token: 0x04003430 RID: 13360
		private bool m_isOpen;

		// Token: 0x04003431 RID: 13361
		private int m_maxCallbackThreads;

		// Token: 0x04003432 RID: 13362
		private int m_maxSendAttempts;

		// Token: 0x04003433 RID: 13363
		private int m_port;

		// Token: 0x04003434 RID: 13364
		private int m_verifyConnectionInterval;

		// Token: 0x04003435 RID: 13365
		private Semaphore sem;

		// Token: 0x04003436 RID: 13366
		private Thread sendThread;

		// Token: 0x04003437 RID: 13367
		private bool waiting;
	}
}
