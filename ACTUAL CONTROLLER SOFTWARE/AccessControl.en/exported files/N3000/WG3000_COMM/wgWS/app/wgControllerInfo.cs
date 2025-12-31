using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WG3000_COMM.Properties;

namespace WG3000_COMM.wgWS.app
{
	// Token: 0x020003AC RID: 940
	[DebuggerStepThrough]
	[WebServiceBinding(Name = "wgControllerInfoSoap", Namespace = "https://app.znsmart.com/wsWGController.asmx")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[DesignerCategory("code")]
	public class wgControllerInfo : SoapHttpClientProtocol
	{
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060021D3 RID: 8659 RVA: 0x0027F150 File Offset: 0x0027E150
		// (remove) Token: 0x060021D4 RID: 8660 RVA: 0x0027F188 File Offset: 0x0027E188
		public event getCmdCompletedEventHandler getCmdCompleted;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060021D5 RID: 8661 RVA: 0x0027F1C0 File Offset: 0x0027E1C0
		// (remove) Token: 0x060021D6 RID: 8662 RVA: 0x0027F1F8 File Offset: 0x0027E1F8
		public event getPassInfoWithRSACompletedEventHandler getPassInfoWithRSACompleted;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060021D7 RID: 8663 RVA: 0x0027F230 File Offset: 0x0027E230
		// (remove) Token: 0x060021D8 RID: 8664 RVA: 0x0027F268 File Offset: 0x0027E268
		public event HelloWorldCompletedEventHandler HelloWorldCompleted;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060021D9 RID: 8665 RVA: 0x0027F2A0 File Offset: 0x0027E2A0
		// (remove) Token: 0x060021DA RID: 8666 RVA: 0x0027F2D8 File Offset: 0x0027E2D8
		public event validateCompletedEventHandler validateCompleted;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060021DB RID: 8667 RVA: 0x0027F310 File Offset: 0x0027E310
		// (remove) Token: 0x060021DC RID: 8668 RVA: 0x0027F348 File Offset: 0x0027E348
		public event validateStringCompletedEventHandler validateStringCompleted;

		// Token: 0x060021DD RID: 8669 RVA: 0x0027F37D File Offset: 0x0027E37D
		public wgControllerInfo()
		{
			this.Url = Settings.Default.N3000_cn_com_wiegand_app_wgControllerInfo;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0027F3B9 File Offset: 0x0027E3B9
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0027F3C4 File Offset: 0x0027E3C4
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/getCmd", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getCmd(string conContent)
		{
			return (string)base.Invoke("getCmd", new object[] { conContent })[0];
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0027F3EF File Offset: 0x0027E3EF
		public void getCmdAsync(string conContent)
		{
			this.getCmdAsync(conContent, null);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0027F3FC File Offset: 0x0027E3FC
		public void getCmdAsync(string conContent, object userState)
		{
			if (this.getCmdOperationCompleted == null)
			{
				this.getCmdOperationCompleted = new SendOrPostCallback(this.OngetCmdOperationCompleted);
			}
			base.InvokeAsync("getCmd", new object[] { conContent }, this.getCmdOperationCompleted, userState);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0027F444 File Offset: 0x0027E444
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/getPassInfoWithRSA", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getPassInfoWithRSA(string conContent)
		{
			return (string)base.Invoke("getPassInfoWithRSA", new object[] { conContent })[0];
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0027F46F File Offset: 0x0027E46F
		public void getPassInfoWithRSAAsync(string conContent)
		{
			this.getPassInfoWithRSAAsync(conContent, null);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0027F47C File Offset: 0x0027E47C
		public void getPassInfoWithRSAAsync(string conContent, object userState)
		{
			if (this.getPassInfoWithRSAOperationCompleted == null)
			{
				this.getPassInfoWithRSAOperationCompleted = new SendOrPostCallback(this.OngetPassInfoWithRSAOperationCompleted);
			}
			base.InvokeAsync("getPassInfoWithRSA", new object[] { conContent }, this.getPassInfoWithRSAOperationCompleted, userState);
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0027F4C1 File Offset: 0x0027E4C1
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/HelloWorld", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string HelloWorld()
		{
			return (string)base.Invoke("HelloWorld", new object[0])[0];
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0027F4DB File Offset: 0x0027E4DB
		public void HelloWorldAsync()
		{
			this.HelloWorldAsync(null);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0027F4E4 File Offset: 0x0027E4E4
		public void HelloWorldAsync(object userState)
		{
			if (this.HelloWorldOperationCompleted == null)
			{
				this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
			}
			base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0027F518 File Offset: 0x0027E518
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x0027F564 File Offset: 0x0027E564
		private void OngetCmdOperationCompleted(object arg)
		{
			if (this.getCmdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getCmdCompleted(this, new getCmdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0027F5AC File Offset: 0x0027E5AC
		private void OngetPassInfoWithRSAOperationCompleted(object arg)
		{
			if (this.getPassInfoWithRSACompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getPassInfoWithRSACompleted(this, new getPassInfoWithRSACompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0027F5F4 File Offset: 0x0027E5F4
		private void OnHelloWorldOperationCompleted(object arg)
		{
			if (this.HelloWorldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0027F63C File Offset: 0x0027E63C
		private void OnvalidateOperationCompleted(object arg)
		{
			if (this.validateCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.validateCompleted(this, new validateCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0027F684 File Offset: 0x0027E684
		private void OnvalidateStringOperationCompleted(object arg)
		{
			if (this.validateStringCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.validateStringCompleted(this, new validateStringCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0027F6CC File Offset: 0x0027E6CC
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/validate", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public int validate(string conContent)
		{
			return (int)base.Invoke("validate", new object[] { conContent })[0];
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0027F6F7 File Offset: 0x0027E6F7
		public void validateAsync(string conContent)
		{
			this.validateAsync(conContent, null);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0027F704 File Offset: 0x0027E704
		public void validateAsync(string conContent, object userState)
		{
			if (this.validateOperationCompleted == null)
			{
				this.validateOperationCompleted = new SendOrPostCallback(this.OnvalidateOperationCompleted);
			}
			base.InvokeAsync("validate", new object[] { conContent }, this.validateOperationCompleted, userState);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0027F74C File Offset: 0x0027E74C
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/validateString", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string validateString(string conContent)
		{
			return (string)base.Invoke("validateString", new object[] { conContent })[0];
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0027F777 File Offset: 0x0027E777
		public void validateStringAsync(string conContent)
		{
			this.validateStringAsync(conContent, null);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0027F784 File Offset: 0x0027E784
		public void validateStringAsync(string conContent, object userState)
		{
			if (this.validateStringOperationCompleted == null)
			{
				this.validateStringOperationCompleted = new SendOrPostCallback(this.OnvalidateStringOperationCompleted);
			}
			base.InvokeAsync("validateString", new object[] { conContent }, this.validateStringOperationCompleted, userState);
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x0027F7C9 File Offset: 0x0027E7C9
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x0027F7D1 File Offset: 0x0027E7D1
		public new string Url
		{
			get
			{
				return base.Url;
			}
			set
			{
				if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
				{
					base.UseDefaultCredentials = false;
				}
				base.Url = value;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x0027F800 File Offset: 0x0027E800
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x0027F808 File Offset: 0x0027E808
		public new bool UseDefaultCredentials
		{
			get
			{
				return base.UseDefaultCredentials;
			}
			set
			{
				base.UseDefaultCredentials = value;
				this.useDefaultCredentialsSetExplicitly = true;
			}
		}

		// Token: 0x04003A3B RID: 14907
		private SendOrPostCallback getCmdOperationCompleted;

		// Token: 0x04003A3C RID: 14908
		private SendOrPostCallback getPassInfoWithRSAOperationCompleted;

		// Token: 0x04003A3D RID: 14909
		private SendOrPostCallback HelloWorldOperationCompleted;

		// Token: 0x04003A3E RID: 14910
		private bool useDefaultCredentialsSetExplicitly;

		// Token: 0x04003A3F RID: 14911
		private SendOrPostCallback validateOperationCompleted;

		// Token: 0x04003A40 RID: 14912
		private SendOrPostCallback validateStringOperationCompleted;
	}
}
