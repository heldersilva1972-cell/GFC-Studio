using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WG3000_COMM.Properties;

namespace WG3000_COMM.localhostAA
{
	// Token: 0x02000338 RID: 824
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "wsWGLongPacketConfSoap", Namespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx")]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class wsWGLongPacketConf : SoapHttpClientProtocol
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060019EB RID: 6635 RVA: 0x0021E2BC File Offset: 0x0021D2BC
		// (remove) Token: 0x060019EC RID: 6636 RVA: 0x0021E2F4 File Offset: 0x0021D2F4
		public event getCusttypeCompletedEventHandler getCusttypeCompleted;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060019ED RID: 6637 RVA: 0x0021E32C File Offset: 0x0021D32C
		// (remove) Token: 0x060019EE RID: 6638 RVA: 0x0021E364 File Offset: 0x0021D364
		public event getLongCmdCompletedEventHandler getLongCmdCompleted;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060019EF RID: 6639 RVA: 0x0021E39C File Offset: 0x0021D39C
		// (remove) Token: 0x060019F0 RID: 6640 RVA: 0x0021E3D4 File Offset: 0x0021D3D4
		public event HelloWorldCompletedEventHandler HelloWorldCompleted;

		// Token: 0x060019F1 RID: 6641 RVA: 0x0021E409 File Offset: 0x0021D409
		public wsWGLongPacketConf()
		{
			this.Url = Settings.Default.N3000_localhostAA_wsWGLongPacketConf;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0021E445 File Offset: 0x0021D445
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0021E450 File Offset: 0x0021D450
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/getCusttype", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getCusttype(string userid, string SN, string conContent)
		{
			return (string)base.Invoke("getCusttype", new object[] { userid, SN, conContent })[0];
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0021E483 File Offset: 0x0021D483
		public void getCusttypeAsync(string userid, string SN, string conContent)
		{
			this.getCusttypeAsync(userid, SN, conContent, null);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0021E490 File Offset: 0x0021D490
		public void getCusttypeAsync(string userid, string SN, string conContent, object userState)
		{
			if (this.getCusttypeOperationCompleted == null)
			{
				this.getCusttypeOperationCompleted = new SendOrPostCallback(this.OngetCusttypeOperationCompleted);
			}
			base.InvokeAsync("getCusttype", new object[] { userid, SN, conContent }, this.getCusttypeOperationCompleted, userState);
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0021E4E0 File Offset: 0x0021D4E0
		private void OngetCusttypeOperationCompleted(object arg)
		{
			if (this.getCusttypeCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getCusttypeCompleted(this, new getCusttypeCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0021E528 File Offset: 0x0021D528
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/getLongCmd", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getLongCmd(string userid, string SN, string conContent)
		{
			return (string)base.Invoke("getLongCmd", new object[] { userid, SN, conContent })[0];
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0021E55B File Offset: 0x0021D55B
		public void getLongCmdAsync(string userid, string SN, string conContent)
		{
			this.getLongCmdAsync(userid, SN, conContent, null);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0021E568 File Offset: 0x0021D568
		public void getLongCmdAsync(string userid, string SN, string conContent, object userState)
		{
			if (this.getLongCmdOperationCompleted == null)
			{
				this.getLongCmdOperationCompleted = new SendOrPostCallback(this.OngetLongCmdOperationCompleted);
			}
			base.InvokeAsync("getLongCmd", new object[] { userid, SN, conContent }, this.getLongCmdOperationCompleted, userState);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0021E5B8 File Offset: 0x0021D5B8
		private void OngetLongCmdOperationCompleted(object arg)
		{
			if (this.getLongCmdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getLongCmdCompleted(this, new getLongCmdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0021E5FD File Offset: 0x0021D5FD
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/HelloWorld", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string HelloWorld()
		{
			return (string)base.Invoke("HelloWorld", new object[0])[0];
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0021E617 File Offset: 0x0021D617
		public void HelloWorldAsync()
		{
			this.HelloWorldAsync(null);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0021E620 File Offset: 0x0021D620
		public void HelloWorldAsync(object userState)
		{
			if (this.HelloWorldOperationCompleted == null)
			{
				this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
			}
			base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0021E654 File Offset: 0x0021D654
		private void OnHelloWorldOperationCompleted(object arg)
		{
			if (this.HelloWorldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0021E69C File Offset: 0x0021D69C
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x0021E6E5 File Offset: 0x0021D6E5
		// (set) Token: 0x06001A01 RID: 6657 RVA: 0x0021E6ED File Offset: 0x0021D6ED
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

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0021E71C File Offset: 0x0021D71C
		// (set) Token: 0x06001A03 RID: 6659 RVA: 0x0021E724 File Offset: 0x0021D724
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

		// Token: 0x040034BC RID: 13500
		private SendOrPostCallback getCusttypeOperationCompleted;

		// Token: 0x040034BD RID: 13501
		private SendOrPostCallback getLongCmdOperationCompleted;

		// Token: 0x040034BE RID: 13502
		private SendOrPostCallback HelloWorldOperationCompleted;

		// Token: 0x040034BF RID: 13503
		private bool useDefaultCredentialsSetExplicitly;
	}
}
