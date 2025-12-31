using System;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x020002DE RID: 734
	public class UserInfo
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00198326 File Offset: 0x00197326
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0019832E File Offset: 0x0019732E
		public string cardNum
		{
			get
			{
				return this._cardNum;
			}
			set
			{
				this._cardNum = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00198337 File Offset: 0x00197337
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0019833F File Offset: 0x0019733F
		public CardInfo[] cards
		{
			get
			{
				return this._cards;
			}
			set
			{
				this._cards = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x00198348 File Offset: 0x00197348
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x00198350 File Offset: 0x00197350
		public string fingerPrintNum
		{
			get
			{
				return this._fingerPrintNum;
			}
			set
			{
				this._fingerPrintNum = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00198359 File Offset: 0x00197359
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x00198361 File Offset: 0x00197361
		public string memberName
		{
			get
			{
				return this._memberName;
			}
			set
			{
				this._memberName = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0019836A File Offset: 0x0019736A
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x00198372 File Offset: 0x00197372
		public string memberNo
		{
			get
			{
				return this._memberNo;
			}
			set
			{
				this._memberNo = value;
			}
		}

		// Token: 0x04002B56 RID: 11094
		private string _cardNum;

		// Token: 0x04002B57 RID: 11095
		private CardInfo[] _cards;

		// Token: 0x04002B58 RID: 11096
		private string _fingerPrintNum;

		// Token: 0x04002B59 RID: 11097
		private string _memberName;

		// Token: 0x04002B5A RID: 11098
		private string _memberNo;
	}
}
