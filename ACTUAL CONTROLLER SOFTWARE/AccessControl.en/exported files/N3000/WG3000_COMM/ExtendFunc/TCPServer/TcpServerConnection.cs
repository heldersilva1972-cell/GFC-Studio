using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WG3000_COMM.ExtendFunc.TCPServer
{
	// Token: 0x02000327 RID: 807
	public class TcpServerConnection
	{
		// Token: 0x06001971 RID: 6513 RVA: 0x00216548 File Offset: 0x00215548
		public TcpServerConnection(TcpClient sock, Encoding encoding)
		{
			this.m_socket = sock;
			this.messagesToSend = new List<byte[]>();
			this.attemptCount = 0;
			this.m_lastVerifyTime = DateTime.UtcNow;
			this.m_encoding = encoding;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0021657B File Offset: 0x0021557B
		private bool canStartNewThread()
		{
			return this.m_thread == null || ((this.m_thread.ThreadState & (ThreadState.Stopped | ThreadState.Aborted)) != ThreadState.Running && (this.m_thread.ThreadState & ThreadState.Unstarted) == ThreadState.Running);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x002165AC File Offset: 0x002155AC
		public bool connected()
		{
			bool flag;
			try
			{
				flag = this.m_socket.Connected;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x002165E0 File Offset: 0x002155E0
		public void forceDisconnect()
		{
			lock (this.m_socket)
			{
				this.m_socket.Close();
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00216620 File Offset: 0x00215620
		public bool hasMoreWork()
		{
			return this.messagesToSend.Count > 0 || (this.Socket.Available > 0 && this.canStartNewThread());
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00216648 File Offset: 0x00215648
		public bool processOutgoing(int maxSendAttempts)
		{
			lock (this.m_socket)
			{
				if (!this.m_socket.Connected)
				{
					this.messagesToSend.Clear();
					return false;
				}
				if (this.messagesToSend.Count == 0)
				{
					return false;
				}
				NetworkStream stream = this.m_socket.GetStream();
				try
				{
					stream.Write(this.messagesToSend[0], 0, this.messagesToSend[0].Length);
					lock (this.messagesToSend)
					{
						this.messagesToSend.RemoveAt(0);
					}
					this.attemptCount = 0;
				}
				catch (IOException)
				{
					this.attemptCount++;
					if (this.attemptCount >= maxSendAttempts)
					{
						lock (this.messagesToSend)
						{
							this.messagesToSend.RemoveAt(0);
						}
						this.attemptCount = 0;
					}
				}
				catch (ObjectDisposedException)
				{
					this.m_socket.Close();
					return false;
				}
			}
			return this.messagesToSend.Count != 0;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x002167A0 File Offset: 0x002157A0
		public void sendData(string data)
		{
			byte[] bytes = this.m_encoding.GetBytes(data);
			lock (this.messagesToSend)
			{
				this.messagesToSend.Add(bytes);
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x002167EC File Offset: 0x002157EC
		public void sendData(byte[] data)
		{
			lock (this.messagesToSend)
			{
				this.messagesToSend.Add(data);
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0021682C File Offset: 0x0021582C
		public bool verifyConnected()
		{
			bool flag = this.m_socket.Client.Available != 0 || !this.m_socket.Client.Poll(1, SelectMode.SelectRead) || this.m_socket.Client.Available != 0;
			this.m_lastVerifyTime = DateTime.UtcNow;
			return flag;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x00216885 File Offset: 0x00215885
		// (set) Token: 0x0600197B RID: 6523 RVA: 0x0021688D File Offset: 0x0021588D
		public Thread CallbackThread
		{
			get
			{
				return this.m_thread;
			}
			set
			{
				if (!this.canStartNewThread())
				{
					throw new Exception("Cannot override TcpServerConnection Callback Thread. The old thread is still running.");
				}
				this.m_thread = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x002168A9 File Offset: 0x002158A9
		// (set) Token: 0x0600197D RID: 6525 RVA: 0x002168B1 File Offset: 0x002158B1
		public Encoding Encoding
		{
			get
			{
				return this.m_encoding;
			}
			set
			{
				this.m_encoding = value;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x002168BA File Offset: 0x002158BA
		public DateTime LastVerifyTime
		{
			get
			{
				return this.m_lastVerifyTime;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x002168C2 File Offset: 0x002158C2
		// (set) Token: 0x06001980 RID: 6528 RVA: 0x002168CA File Offset: 0x002158CA
		public TcpClient Socket
		{
			get
			{
				return this.m_socket;
			}
			set
			{
				this.m_socket = value;
			}
		}

		// Token: 0x0400343B RID: 13371
		private int attemptCount;

		// Token: 0x0400343C RID: 13372
		private Encoding m_encoding;

		// Token: 0x0400343D RID: 13373
		private DateTime m_lastVerifyTime;

		// Token: 0x0400343E RID: 13374
		private TcpClient m_socket;

		// Token: 0x0400343F RID: 13375
		private Thread m_thread;

		// Token: 0x04003440 RID: 13376
		private List<byte[]> messagesToSend;
	}
}
