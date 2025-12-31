using System;

namespace WG3000_COMM.ExtendFunc.FaceReader
{
	// Token: 0x02000255 RID: 597
	public class CardInfo
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0016B9F6 File Offset: 0x0016A9F6
		// (set) Token: 0x060012CF RID: 4815 RVA: 0x0016B9FE File Offset: 0x0016A9FE
		public string cardNumber
		{
			get
			{
				return this._cardNumber;
			}
			set
			{
				this._cardNumber = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0016BA07 File Offset: 0x0016AA07
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x0016BA0F File Offset: 0x0016AA0F
		public string cardPassword
		{
			get
			{
				return this._cardPassword;
			}
			set
			{
				this._cardPassword = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x0016BA18 File Offset: 0x0016AA18
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x0016BA20 File Offset: 0x0016AA20
		public string cardSerialNumber
		{
			get
			{
				return this._cardSerialNumber;
			}
			set
			{
				this._cardSerialNumber = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0016BA29 File Offset: 0x0016AA29
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x0016BA31 File Offset: 0x0016AA31
		public string cardType
		{
			get
			{
				return this._cardType;
			}
			set
			{
				this._cardType = value;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x0016BA3A File Offset: 0x0016AA3A
		// (set) Token: 0x060012D7 RID: 4823 RVA: 0x0016BA42 File Offset: 0x0016AA42
		public string cardValidityPeriod
		{
			get
			{
				return this._cardValidityPeriod;
			}
			set
			{
				this._cardValidityPeriod = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0016BA4B File Offset: 0x0016AA4B
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x0016BA53 File Offset: 0x0016AA53
		public string fingerPrintDescriptions
		{
			get
			{
				return this._fingerPrintDescriptions;
			}
			set
			{
				this._fingerPrintDescriptions = value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x0016BA5C File Offset: 0x0016AA5C
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x0016BA64 File Offset: 0x0016AA64
		public string fingerSerialNumber
		{
			get
			{
				return this._fingerSerialNumber;
			}
			set
			{
				this._fingerSerialNumber = value;
			}
		}

		// Token: 0x040022F5 RID: 8949
		private string _cardNumber;

		// Token: 0x040022F6 RID: 8950
		private string _cardPassword;

		// Token: 0x040022F7 RID: 8951
		private string _cardSerialNumber;

		// Token: 0x040022F8 RID: 8952
		private string _cardType;

		// Token: 0x040022F9 RID: 8953
		private string _cardValidityPeriod;

		// Token: 0x040022FA RID: 8954
		private string _fingerPrintDescriptions;

		// Token: 0x040022FB RID: 8955
		private string _fingerSerialNumber;
	}
}
