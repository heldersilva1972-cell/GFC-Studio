using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WgiCCard
{
	// Token: 0x02000384 RID: 900
	public class Rs232
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06002119 RID: 8473 RVA: 0x0027DE5C File Offset: 0x0027CE5C
		// (remove) Token: 0x0600211A RID: 8474 RVA: 0x0027DE75 File Offset: 0x0027CE75
		public event Rs232.CommEventEventHandler CommEvent;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600211B RID: 8475 RVA: 0x0027DE8E File Offset: 0x0027CE8E
		// (remove) Token: 0x0600211C RID: 8476 RVA: 0x0027DEA7 File Offset: 0x0027CEA7
		public event Rs232.DataReceivedEventHandler DataReceived;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600211D RID: 8477 RVA: 0x0027DEC0 File Offset: 0x0027CEC0
		// (remove) Token: 0x0600211E RID: 8478 RVA: 0x0027DED9 File Offset: 0x0027CED9
		public event Rs232.TxCompletedEventHandler TxCompleted;

		// Token: 0x0600211F RID: 8479 RVA: 0x0027DEF2 File Offset: 0x0027CEF2
		public void _R()
		{
			this.Read(this.miTmpBytes2Read);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0027DF01 File Offset: 0x0027CF01
		public void AsyncRead(int Bytes2Read)
		{
			if (this.meMode != Rs232.Mode.Overlapped)
			{
				throw new ApplicationException("Async Methods allowed only when WorkingMode=Overlapped");
			}
			this.miTmpBytes2Read = Bytes2Read;
			this.moThreadTx = new Thread(new ThreadStart(this._R));
			this.moThreadTx.Start();
		}

		// Token: 0x06002121 RID: 8481
		[DllImport("kernel32.dll")]
		private static extern int BuildCommDCB(string lpDef, ref Rs232.DCB lpDCB);

		// Token: 0x06002122 RID: 8482
		[DllImport("kernel32.dll")]
		private static extern int ClearCommError(int hFile, int lpErrors, int l);

		// Token: 0x06002123 RID: 8483 RVA: 0x0027DF40 File Offset: 0x0027CF40
		public void ClearInputBuffer()
		{
			if (this.mhRS != -1)
			{
				Rs232.PurgeComm(this.mhRS, 8);
			}
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0027DF58 File Offset: 0x0027CF58
		public void Close()
		{
			if (this.mhRS != -1)
			{
				Rs232.CloseHandle(this.mhRS);
				this.mhRS = -1;
			}
		}

		// Token: 0x06002125 RID: 8485
		[DllImport("kernel32.dll")]
		private static extern int CloseHandle(int hObject);

		// Token: 0x06002126 RID: 8486
		[DllImport("kernel32.dll")]
		private static extern int CreateEvent(int lpEventAttributes, int bManualReset, int bInitialState, string lpName);

		// Token: 0x06002127 RID: 8487
		[DllImport("kernel32.dll")]
		private static extern int CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);

		// Token: 0x06002128 RID: 8488
		[DllImport("kernel32.dll")]
		private static extern bool EscapeCommFunction(int hFile, long ifunc);

		// Token: 0x06002129 RID: 8489
		[DllImport("kernel32.dll")]
		private static extern int FormatMessage(int dwFlags, int lpSource, int dwMessageId, int dwLanguageId, string lpBuffer, int nSize, int Arguments);

		// Token: 0x0600212A RID: 8490
		[DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "FormatMessageA", ExactSpelling = true, SetLastError = true)]
		private static extern int FormatMessage(int dwFlags, int lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, int Arguments);

		// Token: 0x0600212B RID: 8491
		[DllImport("kernel32.dll")]
		public static extern bool GetCommModemStatus(int hFile, ref int lpModemStatus);

		// Token: 0x0600212C RID: 8492
		[DllImport("kernel32.dll")]
		private static extern int GetCommState(int hCommDev, ref Rs232.DCB lpDCB);

		// Token: 0x0600212D RID: 8493
		[DllImport("kernel32.dll")]
		private static extern int GetCommTimeouts(int hFile, ref Rs232.COMMTIMEOUTS lpCommTimeouts);

		// Token: 0x0600212E RID: 8494
		[DllImport("kernel32.dll")]
		private static extern int GetLastError();

		// Token: 0x0600212F RID: 8495
		[DllImport("kernel32.dll")]
		private static extern int GetOverlappedResult(int hFile, ref Rs232.OVERLAPPED lpOverlapped, ref int lpNumberOfBytesTransferred, int bWait);

		// Token: 0x06002130 RID: 8496 RVA: 0x0027DF78 File Offset: 0x0027CF78
		public void Open()
		{
			Rs232.DCB dcb = default(Rs232.DCB);
			int num = Convert.ToInt32((this.meMode == Rs232.Mode.Overlapped) ? 1073741824 : 0);
			if (this.miPort > 0)
			{
				try
				{
					this.mhRS = Rs232.CreateFile("\\\\.\\COM" + this.miPort.ToString(), -1073741824, 0, 0, 3, num, 0);
					if (this.mhRS == -1)
					{
						throw new Rs232.CIOChannelException("Unable to open COM" + this.miPort.ToString());
					}
					int num2 = 0;
					Rs232.ClearCommError(this.mhRS, num2, 0);
					Rs232.PurgeComm(this.mhRS, Convert.ToInt32((Rs232.PurgeBuffers)12));
					Rs232.GetCommState(this.mhRS, ref dcb);
					string text = "NOEM";
					text = text.Substring((int)this.meParity, 1);
					Rs232.BuildCommDCB(string.Format("baud={0} parity={1} data={2} stop={3}", new object[]
					{
						this.miBaudRate,
						text,
						this.miDataBit,
						Convert.ToInt32(this.meStopBit)
					}), ref dcb);
					if (Rs232.SetCommState(this.mhRS, ref dcb) == 0)
					{
						string text2 = this.pErr2Text(Rs232.GetLastError());
						throw new Rs232.CIOChannelException("Unable to set COM state0" + text2);
					}
					Rs232.SetupComm(this.mhRS, this.miBufferSize, this.miBufferSize);
					this.pSetTimeout();
					return;
				}
				catch (Exception ex)
				{
					throw new Rs232.CIOChannelException(ex.Message, ex);
				}
			}
			throw new ApplicationException("COM Port not defined, use Port property to set it before invoking InitPort");
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0027E128 File Offset: 0x0027D128
		public void Open(int Port, int BaudRate, int DataBit, Rs232.DataParity Parity, Rs232.DataStopBit StopBit, int BufferSize)
		{
			this.Port = Port;
			this.BaudRate = BaudRate;
			this.DataBit = DataBit;
			this.Parity = Parity;
			this.StopBit = StopBit;
			this.BufferSize = BufferSize;
			this.Open();
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x0027E160 File Offset: 0x0027D160
		private string pErr2Text(int lCode)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			if (Convert.ToInt32(Rs232.FormatMessage(4096, 0, lCode, 0, stringBuilder, 256, 0)) > 0)
			{
				return stringBuilder.ToString();
			}
			return "Error not found.";
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0027E1A0 File Offset: 0x0027D1A0
		private void pHandleOverlappedRead(int Bytes2Read)
		{
			int num = 0;
			this.muOverlapped.hEvent = Rs232.CreateEvent(0, 1, 0, null);
			if (this.muOverlapped.hEvent == 0)
			{
				throw new ApplicationException("Error creating event for overlapped read.");
			}
			if (!this.mbWaitOnRead)
			{
				this.mabtRxBuf = new byte[Bytes2Read - 1 + 1];
				if (Rs232.ReadFile(this.mhRS, this.mabtRxBuf, Bytes2Read, ref num, ref this.muOverlapped) == 0)
				{
					int num2 = Rs232.GetLastError();
					if (num2 != 997)
					{
						throw new ArgumentException("Overlapped Read Error: " + this.pErr2Text(num2));
					}
					this.mbWaitOnRead = true;
				}
				else if (this.DataReceivedEvent != null)
				{
					this.DataReceivedEvent(this, this.mabtRxBuf);
				}
			}
			if (!this.mbWaitOnRead)
			{
				return;
			}
			int num3 = Rs232.WaitForSingleObject(this.muOverlapped.hEvent, this.miTimeout);
			if (num3 != 0)
			{
				if (num3 != 258)
				{
					throw new ApplicationException("Overlapped read error");
				}
				throw new Rs232.IOTimeoutException("Timeout error");
			}
			else
			{
				if (Rs232.GetOverlappedResult(this.mhRS, ref this.muOverlapped, ref num, 0) != 0)
				{
					if (this.DataReceivedEvent != null)
					{
						this.DataReceivedEvent(this, this.mabtRxBuf);
					}
					this.mbWaitOnRead = false;
					return;
				}
				int num2 = Rs232.GetLastError();
				if (num2 == 996)
				{
					throw new ApplicationException("Read operation incomplete");
				}
				throw new ApplicationException("Read operation error " + num2.ToString());
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0027E308 File Offset: 0x0027D308
		private bool pHandleOverlappedWrite(byte[] Buffer)
		{
			int num = 0;
			bool flag = false;
			this.muOverlappedW.hEvent = Rs232.CreateEvent(0, 1, 0, null);
			if (this.muOverlappedW.hEvent == 0)
			{
				throw new ApplicationException("Error creating event for overlapped write.");
			}
			Rs232.PurgeComm(this.mhRS, 12);
			this.mbWaitOnRead = true;
			if (Rs232.WriteFile(this.mhRS, Buffer, Buffer.Length, ref num, ref this.muOverlappedW) == 0)
			{
				int lastError = Rs232.GetLastError();
				if (lastError != 997)
				{
					throw new ArgumentException("Overlapped Read Error: " + this.pErr2Text(lastError));
				}
				if (Rs232.WaitForSingleObject(this.muOverlappedW.hEvent, -1) == 0)
				{
					if (Rs232.GetOverlappedResult(this.mhRS, ref this.muOverlappedW, ref num, 0) == 0)
					{
						flag = true;
					}
					else
					{
						this.mbWaitOnRead = false;
						if (this.TxCompletedEvent != null)
						{
							this.TxCompletedEvent(this);
						}
					}
				}
			}
			else
			{
				flag = false;
			}
			Rs232.CloseHandle(this.muOverlappedW.hEvent);
			return flag;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0027E3F8 File Offset: 0x0027D3F8
		private void pSetTimeout()
		{
			Rs232.COMMTIMEOUTS commtimeouts = default(Rs232.COMMTIMEOUTS);
			if (this.mhRS != -1)
			{
				commtimeouts.ReadIntervalTimeout = 0;
				commtimeouts.ReadTotalTimeoutMultiplier = 0;
				commtimeouts.ReadTotalTimeoutConstant = this.miTimeout;
				commtimeouts.WriteTotalTimeoutMultiplier = 0;
				commtimeouts.WriteTotalTimeoutConstant = 0;
				Rs232.SetCommTimeouts(this.mhRS, ref commtimeouts);
			}
		}

		// Token: 0x06002136 RID: 8502
		[DllImport("kernel32.dll")]
		private static extern int PurgeComm(int hFile, int dwFlags);

		// Token: 0x06002137 RID: 8503 RVA: 0x0027E454 File Offset: 0x0027D454
		public int Read(int Bytes2Read)
		{
			int num = 0;
			if (Bytes2Read == 0)
			{
				Bytes2Read = this.miBufferSize;
			}
			if (this.mhRS == -1)
			{
				throw new ApplicationException("Please initialize and open port before using this method");
			}
			try
			{
				if (this.meMode == Rs232.Mode.Overlapped)
				{
					this.pHandleOverlappedRead(Bytes2Read);
				}
				else
				{
					this.mabtRxBuf = new byte[Bytes2Read - 1 + 1];
					Rs232.OVERLAPPED overlapped = default(Rs232.OVERLAPPED);
					int num2 = Rs232.ReadFile(this.mhRS, this.mabtRxBuf, Bytes2Read, ref num, ref overlapped);
					if (num2 == 0)
					{
						throw new ApplicationException("ReadFile error " + num2.ToString());
					}
					if (num < Bytes2Read)
					{
						return -13;
					}
					this.mbWaitOnRead = true;
					return num;
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Read Error: " + ex.Message, ex);
			}
			return -1;
		}

		// Token: 0x06002138 RID: 8504
		[DllImport("kernel32.dll")]
		private static extern int ReadFile(int hFile, byte[] Buffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref Rs232.OVERLAPPED lpOverlapped);

		// Token: 0x06002139 RID: 8505
		[DllImport("kernel32.dll")]
		private static extern int SetCommMask(int hFile, int lpEvtMask);

		// Token: 0x0600213A RID: 8506
		[DllImport("kernel32.dll")]
		private static extern int SetCommState(int hCommDev, ref Rs232.DCB lpDCB);

		// Token: 0x0600213B RID: 8507
		[DllImport("kernel32.dll")]
		private static extern int SetCommTimeouts(int hFile, ref Rs232.COMMTIMEOUTS lpCommTimeouts);

		// Token: 0x0600213C RID: 8508
		[DllImport("kernel32.dll")]
		private static extern int SetupComm(int hFile, int dwInQueue, int dwOutQueue);

		// Token: 0x0600213D RID: 8509
		[DllImport("kernel32.dll")]
		private static extern int WaitCommEvent(int hFile, ref Rs232.EventMasks Mask, ref Rs232.OVERLAPPED lpOverlap);

		// Token: 0x0600213E RID: 8510
		[DllImport("kernel32.dll")]
		private static extern int WaitForSingleObject(int hHandle, int dwMilliseconds);

		// Token: 0x0600213F RID: 8511 RVA: 0x0027E524 File Offset: 0x0027D524
		public void Write(byte[] Buffer)
		{
			int num = 0;
			if (this.mhRS == -1)
			{
				throw new ApplicationException("Please initialize and open port before using this method");
			}
			try
			{
				if (this.meMode == Rs232.Mode.Overlapped)
				{
					if (this.pHandleOverlappedWrite(Buffer))
					{
						throw new ApplicationException("Error in overllapped write");
					}
				}
				else
				{
					Rs232.PurgeComm(this.mhRS, 12);
					Rs232.OVERLAPPED overlapped = default(Rs232.OVERLAPPED);
					if (Rs232.WriteFile(this.mhRS, Buffer, Buffer.Length, ref num, ref overlapped) == 0)
					{
						int num2 = Buffer.Length;
						throw new ApplicationException("Write Error - Bytes Written " + num.ToString() + " of " + num2.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0027E5CC File Offset: 0x0027D5CC
		public void Write(string Buffer)
		{
			byte[] bytes = new ASCIIEncoding().GetBytes(Buffer);
			this.Write(bytes);
		}

		// Token: 0x06002141 RID: 8513
		[DllImport("kernel32.dll")]
		private static extern int WriteFile(int hFile, byte[] Buffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, ref Rs232.OVERLAPPED lpOverlapped);

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x0027E5EC File Offset: 0x0027D5EC
		// (set) Token: 0x06002143 RID: 8515 RVA: 0x0027E5F4 File Offset: 0x0027D5F4
		public int BaudRate
		{
			get
			{
				return this.miBaudRate;
			}
			set
			{
				this.miBaudRate = value;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x0027E5FD File Offset: 0x0027D5FD
		// (set) Token: 0x06002145 RID: 8517 RVA: 0x0027E605 File Offset: 0x0027D605
		public int BufferSize
		{
			get
			{
				return this.miBufferSize;
			}
			set
			{
				this.miBufferSize = value;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x0027E60E File Offset: 0x0027D60E
		// (set) Token: 0x06002147 RID: 8519 RVA: 0x0027E616 File Offset: 0x0027D616
		public int DataBit
		{
			get
			{
				return this.miDataBit;
			}
			set
			{
				this.miDataBit = value;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x0027E61F File Offset: 0x0027D61F
		public virtual byte[] InputStream
		{
			get
			{
				return this.mabtRxBuf;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x0027E628 File Offset: 0x0027D628
		public virtual string InputStreamString
		{
			get
			{
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				return asciiencoding.GetString(this.InputStream);
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x0027E647 File Offset: 0x0027D647
		public bool IsOpen
		{
			get
			{
				return this.mhRS != -1;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x0027E658 File Offset: 0x0027D658
		public Rs232.ModemStatusBits ModemStatus
		{
			get
			{
				if (this.mhRS == -1)
				{
					throw new ApplicationException("Please initialize and open port before using this method");
				}
				int num = 0;
				if (!Rs232.GetCommModemStatus(this.mhRS, ref num))
				{
					throw new ApplicationException("Unable to get modem status");
				}
				return (Rs232.ModemStatusBits)num;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x0027E696 File Offset: 0x0027D696
		// (set) Token: 0x0600214D RID: 8525 RVA: 0x0027E69E File Offset: 0x0027D69E
		public Rs232.DataParity Parity
		{
			get
			{
				return this.meParity;
			}
			set
			{
				this.meParity = value;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x0027E6A7 File Offset: 0x0027D6A7
		// (set) Token: 0x0600214F RID: 8527 RVA: 0x0027E6AF File Offset: 0x0027D6AF
		public int Port
		{
			get
			{
				return this.miPort;
			}
			set
			{
				this.miPort = value;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x0027E6B8 File Offset: 0x0027D6B8
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x0027E6C0 File Offset: 0x0027D6C0
		public Rs232.DataStopBit StopBit
		{
			get
			{
				return this.meStopBit;
			}
			set
			{
				this.meStopBit = value;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x0027E6C9 File Offset: 0x0027D6C9
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x0027E6D1 File Offset: 0x0027D6D1
		public virtual int Timeout
		{
			get
			{
				return this.miTimeout;
			}
			set
			{
				this.miTimeout = Convert.ToInt32((value == 0) ? 500 : value);
				this.pSetTimeout();
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x0027E6EF File Offset: 0x0027D6EF
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x0027E6F7 File Offset: 0x0027D6F7
		public Rs232.Mode WorkingMode
		{
			get
			{
				return this.meMode;
			}
			set
			{
				this.meMode = value;
			}
		}

		// Token: 0x04003957 RID: 14679
		private const int ERROR_IO_INCOMPLETE = 996;

		// Token: 0x04003958 RID: 14680
		private const int ERROR_IO_PENDING = 997;

		// Token: 0x04003959 RID: 14681
		private const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x0400395A RID: 14682
		private const int GENERIC_READ = -2147483648;

		// Token: 0x0400395B RID: 14683
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x0400395C RID: 14684
		private const int INFINITE = -1;

		// Token: 0x0400395D RID: 14685
		private const int INVALID_HANDLE_VALUE = -1;

		// Token: 0x0400395E RID: 14686
		private const int IO_BUFFER_SIZE = 1024;

		// Token: 0x0400395F RID: 14687
		private const int OPEN_EXISTING = 3;

		// Token: 0x04003960 RID: 14688
		private const int PURGE_RXABORT = 2;

		// Token: 0x04003961 RID: 14689
		private const int PURGE_RXCLEAR = 8;

		// Token: 0x04003962 RID: 14690
		private const int PURGE_TXABORT = 1;

		// Token: 0x04003963 RID: 14691
		private const int PURGE_TXCLEAR = 4;

		// Token: 0x04003964 RID: 14692
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x04003965 RID: 14693
		private const int WAIT_TIMEOUT = 258;

		// Token: 0x04003966 RID: 14694
		public const int WGCOMM_BADPARM = -7;

		// Token: 0x04003967 RID: 14695
		public const int WGCOMM_BADPORT = -1;

		// Token: 0x04003968 RID: 14696
		public const int WGCOMM_NOCONNECT = -13;

		// Token: 0x04003969 RID: 14697
		public const int WGCOMM_OK = 0;

		// Token: 0x0400396A RID: 14698
		public const int WGCOMM_OPENFAIL = -5;

		// Token: 0x0400396B RID: 14699
		public const int WGCOMM_READTIMEOUT = -13;

		// Token: 0x0400396C RID: 14700
		public const int WGCOMM_WIN32FAIL = -8;

		// Token: 0x0400396D RID: 14701
		public const int WGCOMM_WRITETIMEOUT = -12;

		// Token: 0x0400396E RID: 14702
		private byte[] mabtRxBuf;

		// Token: 0x0400396F RID: 14703
		private bool mbWaitOnRead;

		// Token: 0x04003970 RID: 14704
		private Rs232.Mode meMode;

		// Token: 0x04003971 RID: 14705
		private Rs232.DataParity meParity;

		// Token: 0x04003972 RID: 14706
		private Rs232.DataStopBit meStopBit;

		// Token: 0x04003973 RID: 14707
		private int mhRS = -1;

		// Token: 0x04003974 RID: 14708
		private int miBaudRate = 9600;

		// Token: 0x04003975 RID: 14709
		private int miBufferSize = 512;

		// Token: 0x04003976 RID: 14710
		private int miDataBit = 8;

		// Token: 0x04003977 RID: 14711
		private int miPort = 1;

		// Token: 0x04003978 RID: 14712
		private int miTimeout = 70;

		// Token: 0x04003979 RID: 14713
		private int miTmpBytes2Read;

		// Token: 0x0400397A RID: 14714
		private Thread moThreadTx;

		// Token: 0x0400397B RID: 14715
		private Rs232.OVERLAPPED muOverlapped;

		// Token: 0x0400397C RID: 14716
		private Rs232.OVERLAPPED muOverlappedW;

		// Token: 0x02000385 RID: 901
		// (Invoke) Token: 0x06002158 RID: 8536
		public delegate void CommEventEventHandler(Rs232 Source, Rs232.EventMasks Mask);

		// Token: 0x02000386 RID: 902
		// (Invoke) Token: 0x0600215C RID: 8540
		public delegate void DataReceivedEventHandler(Rs232 Source, byte[] DataBuffer);

		// Token: 0x02000387 RID: 903
		// (Invoke) Token: 0x06002160 RID: 8544
		public delegate void TxCompletedEventHandler(Rs232 Source);

		// Token: 0x02000388 RID: 904
		public class CIOChannelException : ApplicationException
		{
			// Token: 0x06002163 RID: 8547 RVA: 0x0027E73B File Offset: 0x0027D73B
			public CIOChannelException(string Message)
				: base(Message)
			{
			}

			// Token: 0x06002164 RID: 8548 RVA: 0x0027E744 File Offset: 0x0027D744
			public CIOChannelException(string Message, Exception InnerException)
				: base(Message, InnerException)
			{
			}
		}

		// Token: 0x02000389 RID: 905
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct COMMCONFIG
		{
			// Token: 0x04003980 RID: 14720
			public int dwSize;

			// Token: 0x04003981 RID: 14721
			public short wVersion;

			// Token: 0x04003982 RID: 14722
			public short wReserved;

			// Token: 0x04003983 RID: 14723
			public Rs232.DCB dcbx;

			// Token: 0x04003984 RID: 14724
			public int dwProviderSubType;

			// Token: 0x04003985 RID: 14725
			public int dwProviderOffset;

			// Token: 0x04003986 RID: 14726
			public int dwProviderSize;

			// Token: 0x04003987 RID: 14727
			public byte wcProviderData;
		}

		// Token: 0x0200038A RID: 906
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct COMMTIMEOUTS
		{
			// Token: 0x04003988 RID: 14728
			public int ReadIntervalTimeout;

			// Token: 0x04003989 RID: 14729
			public int ReadTotalTimeoutMultiplier;

			// Token: 0x0400398A RID: 14730
			public int ReadTotalTimeoutConstant;

			// Token: 0x0400398B RID: 14731
			public int WriteTotalTimeoutMultiplier;

			// Token: 0x0400398C RID: 14732
			public int WriteTotalTimeoutConstant;
		}

		// Token: 0x0200038B RID: 907
		public enum DataParity
		{
			// Token: 0x0400398E RID: 14734
			Parity_None,
			// Token: 0x0400398F RID: 14735
			Pariti_Odd,
			// Token: 0x04003990 RID: 14736
			Parity_Even,
			// Token: 0x04003991 RID: 14737
			Parity_Mark
		}

		// Token: 0x0200038C RID: 908
		public enum DataStopBit
		{
			// Token: 0x04003993 RID: 14739
			StopBit_1 = 1,
			// Token: 0x04003994 RID: 14740
			StopBit_2
		}

		// Token: 0x0200038D RID: 909
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct DCB
		{
			// Token: 0x04003995 RID: 14741
			public int DCBlength;

			// Token: 0x04003996 RID: 14742
			public int BaudRate;

			// Token: 0x04003997 RID: 14743
			public int Bits1;

			// Token: 0x04003998 RID: 14744
			public short wReserved;

			// Token: 0x04003999 RID: 14745
			public short XonLim;

			// Token: 0x0400399A RID: 14746
			public short XoffLim;

			// Token: 0x0400399B RID: 14747
			public byte ByteSize;

			// Token: 0x0400399C RID: 14748
			public byte Parity;

			// Token: 0x0400399D RID: 14749
			public byte StopBits;

			// Token: 0x0400399E RID: 14750
			public byte XonChar;

			// Token: 0x0400399F RID: 14751
			public byte XoffChar;

			// Token: 0x040039A0 RID: 14752
			public byte ErrorChar;

			// Token: 0x040039A1 RID: 14753
			public byte EofChar;

			// Token: 0x040039A2 RID: 14754
			public byte EvtChar;

			// Token: 0x040039A3 RID: 14755
			public short wReserved2;
		}

		// Token: 0x0200038E RID: 910
		[Flags]
		public enum EventMasks
		{
			// Token: 0x040039A5 RID: 14757
			Break = 64,
			// Token: 0x040039A6 RID: 14758
			ClearToSend = 8,
			// Token: 0x040039A7 RID: 14759
			DataSetReady = 16,
			// Token: 0x040039A8 RID: 14760
			ReceiveLine = 32,
			// Token: 0x040039A9 RID: 14761
			Ring = 256,
			// Token: 0x040039AA RID: 14762
			RxChar = 1,
			// Token: 0x040039AB RID: 14763
			RXFlag = 2,
			// Token: 0x040039AC RID: 14764
			StatusError = 128,
			// Token: 0x040039AD RID: 14765
			TxBufferEmpty = 4
		}

		// Token: 0x0200038F RID: 911
		public class IOTimeoutException : Rs232.CIOChannelException
		{
			// Token: 0x06002165 RID: 8549 RVA: 0x0027E74E File Offset: 0x0027D74E
			public IOTimeoutException(string Message)
				: base(Message)
			{
			}

			// Token: 0x06002166 RID: 8550 RVA: 0x0027E757 File Offset: 0x0027D757
			public IOTimeoutException(string Message, Exception InnerException)
				: base(Message, InnerException)
			{
			}
		}

		// Token: 0x02000390 RID: 912
		private enum Lines
		{
			// Token: 0x040039AF RID: 14767
			ClearBreak = 9,
			// Token: 0x040039B0 RID: 14768
			ClearDtr = 6,
			// Token: 0x040039B1 RID: 14769
			ClearRts = 4,
			// Token: 0x040039B2 RID: 14770
			ResetDev = 7,
			// Token: 0x040039B3 RID: 14771
			SetBreak,
			// Token: 0x040039B4 RID: 14772
			SetDtr = 5,
			// Token: 0x040039B5 RID: 14773
			SetRts = 3
		}

		// Token: 0x02000391 RID: 913
		public enum Mode
		{
			// Token: 0x040039B7 RID: 14775
			NonOverlapped,
			// Token: 0x040039B8 RID: 14776
			Overlapped
		}

		// Token: 0x02000392 RID: 914
		[Flags]
		public enum ModemStatusBits
		{
			// Token: 0x040039BA RID: 14778
			CarrierDetect = 128,
			// Token: 0x040039BB RID: 14779
			ClearToSendOn = 16,
			// Token: 0x040039BC RID: 14780
			DataSetReadyOn = 32,
			// Token: 0x040039BD RID: 14781
			RingIndicatorOn = 64
		}

		// Token: 0x02000393 RID: 915
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct OVERLAPPED
		{
			// Token: 0x040039BE RID: 14782
			public int Internal;

			// Token: 0x040039BF RID: 14783
			public int InternalHigh;

			// Token: 0x040039C0 RID: 14784
			public int Offset;

			// Token: 0x040039C1 RID: 14785
			public int OffsetHigh;

			// Token: 0x040039C2 RID: 14786
			public int hEvent;
		}

		// Token: 0x02000394 RID: 916
		private enum PurgeBuffers
		{
			// Token: 0x040039C4 RID: 14788
			RXAbort = 2,
			// Token: 0x040039C5 RID: 14789
			RXClear = 8,
			// Token: 0x040039C6 RID: 14790
			TxAbort = 1,
			// Token: 0x040039C7 RID: 14791
			TxClear = 4
		}
	}
}
