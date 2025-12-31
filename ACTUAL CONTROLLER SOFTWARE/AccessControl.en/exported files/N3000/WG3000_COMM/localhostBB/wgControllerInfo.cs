using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WG3000_COMM.Properties;

namespace WG3000_COMM.localhostBB
{
	// Token: 0x02000343 RID: 835
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "wgControllerInfoSoap", Namespace = "https://app.znsmart.com/wsWGController.asmx")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class wgControllerInfo : SoapHttpClientProtocol
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001A22 RID: 6690 RVA: 0x0021E7FC File Offset: 0x0021D7FC
		// (remove) Token: 0x06001A23 RID: 6691 RVA: 0x0021E834 File Offset: 0x0021D834
		public event getCmdCompletedEventHandler getCmdCompleted;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001A24 RID: 6692 RVA: 0x0021E86C File Offset: 0x0021D86C
		// (remove) Token: 0x06001A25 RID: 6693 RVA: 0x0021E8A4 File Offset: 0x0021D8A4
		public event getPassInfoWithRSACompletedEventHandler getPassInfoWithRSACompleted;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001A26 RID: 6694 RVA: 0x0021E8DC File Offset: 0x0021D8DC
		// (remove) Token: 0x06001A27 RID: 6695 RVA: 0x0021E914 File Offset: 0x0021D914
		public event HelloWorldCompletedEventHandler HelloWorldCompleted;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001A28 RID: 6696 RVA: 0x0021E94C File Offset: 0x0021D94C
		// (remove) Token: 0x06001A29 RID: 6697 RVA: 0x0021E984 File Offset: 0x0021D984
		public event validateCompletedEventHandler validateCompleted;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001A2A RID: 6698 RVA: 0x0021E9BC File Offset: 0x0021D9BC
		// (remove) Token: 0x06001A2B RID: 6699 RVA: 0x0021E9F4 File Offset: 0x0021D9F4
		public event validateStringCompletedEventHandler validateStringCompleted;

		// Token: 0x06001A2C RID: 6700 RVA: 0x0021EA29 File Offset: 0x0021DA29
		public wgControllerInfo()
		{
			this.Url = Settings.Default.N3000_localhostBB_wgControllerInfo;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0021EA65 File Offset: 0x0021DA65
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0021EA70 File Offset: 0x0021DA70
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/getCmd", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getCmd(string conContent)
		{
			return (string)base.Invoke("getCmd", new object[] { conContent })[0];
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0021EA9B File Offset: 0x0021DA9B
		public void getCmdAsync(string conContent)
		{
			this.getCmdAsync(conContent, null);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0021EAA8 File Offset: 0x0021DAA8
		public void getCmdAsync(string conContent, object userState)
		{
			if (this.getCmdOperationCompleted == null)
			{
				this.getCmdOperationCompleted = new SendOrPostCallback(this.OngetCmdOperationCompleted);
			}
			base.InvokeAsync("getCmd", new object[] { conContent }, this.getCmdOperationCompleted, userState);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0021EAF0 File Offset: 0x0021DAF0
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/getPassInfoWithRSA", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getPassInfoWithRSA(string conContent)
		{
			return (string)base.Invoke("getPassInfoWithRSA", new object[] { conContent })[0];
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0021EB1B File Offset: 0x0021DB1B
		public void getPassInfoWithRSAAsync(string conContent)
		{
			this.getPassInfoWithRSAAsync(conContent, null);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0021EB28 File Offset: 0x0021DB28
		public void getPassInfoWithRSAAsync(string conContent, object userState)
		{
			if (this.getPassInfoWithRSAOperationCompleted == null)
			{
				this.getPassInfoWithRSAOperationCompleted = new SendOrPostCallback(this.OngetPassInfoWithRSAOperationCompleted);
			}
			base.InvokeAsync("getPassInfoWithRSA", new object[] { conContent }, this.getPassInfoWithRSAOperationCompleted, userState);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x0021EB6D File Offset: 0x0021DB6D
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/HelloWorld", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string HelloWorld()
		{
			return (string)base.Invoke("HelloWorld", new object[0])[0];
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0021EB87 File Offset: 0x0021DB87
		public void HelloWorldAsync()
		{
			this.HelloWorldAsync(null);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x0021EB90 File Offset: 0x0021DB90
		public void HelloWorldAsync(object userState)
		{
			if (this.HelloWorldOperationCompleted == null)
			{
				this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
			}
			base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0021EBC4 File Offset: 0x0021DBC4
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0021EC10 File Offset: 0x0021DC10
		private void OngetCmdOperationCompleted(object arg)
		{
			if (this.getCmdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getCmdCompleted(this, new getCmdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0021EC58 File Offset: 0x0021DC58
		private void OngetPassInfoWithRSAOperationCompleted(object arg)
		{
			if (this.getPassInfoWithRSACompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getPassInfoWithRSACompleted(this, new getPassInfoWithRSACompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0021ECA0 File Offset: 0x0021DCA0
		private void OnHelloWorldOperationCompleted(object arg)
		{
			if (this.HelloWorldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0021ECE8 File Offset: 0x0021DCE8
		private void OnvalidateOperationCompleted(object arg)
		{
			if (this.validateCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.validateCompleted(this, new validateCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0021ED30 File Offset: 0x0021DD30
		private void OnvalidateStringOperationCompleted(object arg)
		{
			if (this.validateStringCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.validateStringCompleted(this, new validateStringCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0021ED78 File Offset: 0x0021DD78
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/validate", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public int validate(string conContent)
		{
			return (int)base.Invoke("validate", new object[] { conContent })[0];
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0021EDA3 File Offset: 0x0021DDA3
		public void validateAsync(string conContent)
		{
			this.validateAsync(conContent, null);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0021EDB0 File Offset: 0x0021DDB0
		public void validateAsync(string conContent, object userState)
		{
			if (this.validateOperationCompleted == null)
			{
				this.validateOperationCompleted = new SendOrPostCallback(this.OnvalidateOperationCompleted);
			}
			base.InvokeAsync("validate", new object[] { conContent }, this.validateOperationCompleted, userState);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0021EDF8 File Offset: 0x0021DDF8
		[SoapDocumentMethod("https://app.znsmart.com/wsWGController.asmx/validateString", RequestNamespace = "https://app.znsmart.com/wsWGController.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGController.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string validateString(string conContent)
		{
			return (string)base.Invoke("validateString", new object[] { conContent })[0];
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0021EE23 File Offset: 0x0021DE23
		public void validateStringAsync(string conContent)
		{
			this.validateStringAsync(conContent, null);
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0021EE30 File Offset: 0x0021DE30
		public void validateStringAsync(string conContent, object userState)
		{
			if (this.validateStringOperationCompleted == null)
			{
				this.validateStringOperationCompleted = new SendOrPostCallback(this.OnvalidateStringOperationCompleted);
			}
			base.InvokeAsync("validateString", new object[] { conContent }, this.validateStringOperationCompleted, userState);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0021EE75 File Offset: 0x0021DE75
		// (set) Token: 0x06001A44 RID: 6724 RVA: 0x0021EE7D File Offset: 0x0021DE7D
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x0021EEAC File Offset: 0x0021DEAC
		// (set) Token: 0x06001A46 RID: 6726 RVA: 0x0021EEB4 File Offset: 0x0021DEB4
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

		// Token: 0x040034C8 RID: 13512
		private SendOrPostCallback getCmdOperationCompleted;

		// Token: 0x040034C9 RID: 13513
		private SendOrPostCallback getPassInfoWithRSAOperationCompleted;

		// Token: 0x040034CA RID: 13514
		private SendOrPostCallback HelloWorldOperationCompleted;

		// Token: 0x040034CB RID: 13515
		private bool useDefaultCredentialsSetExplicitly;

		// Token: 0x040034CC RID: 13516
		private SendOrPostCallback validateOperationCompleted;

		// Token: 0x040034CD RID: 13517
		private SendOrPostCallback validateStringOperationCompleted;
	}
}
