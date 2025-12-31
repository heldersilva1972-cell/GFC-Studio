using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WG3000_COMM.Properties;

namespace WG3000_COMM.wgWS.appSpecailBLong
{
	// Token: 0x0200039C RID: 924
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	[WebServiceBinding(Name = "wsWGLongPacketConfSoap", Namespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx")]
	[DesignerCategory("code")]
	public class wsWGLongPacketConf : SoapHttpClientProtocol
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600217D RID: 8573 RVA: 0x0027E88C File Offset: 0x0027D88C
		// (remove) Token: 0x0600217E RID: 8574 RVA: 0x0027E8C4 File Offset: 0x0027D8C4
		public event getCusttypeCompletedEventHandler getCusttypeCompleted;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600217F RID: 8575 RVA: 0x0027E8FC File Offset: 0x0027D8FC
		// (remove) Token: 0x06002180 RID: 8576 RVA: 0x0027E934 File Offset: 0x0027D934
		public event getLongCmdCompletedEventHandler getLongCmdCompleted;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002181 RID: 8577 RVA: 0x0027E96C File Offset: 0x0027D96C
		// (remove) Token: 0x06002182 RID: 8578 RVA: 0x0027E9A4 File Offset: 0x0027D9A4
		public event HelloWorldCompletedEventHandler HelloWorldCompleted;

		// Token: 0x06002183 RID: 8579 RVA: 0x0027E9D9 File Offset: 0x0027D9D9
		public wsWGLongPacketConf()
		{
			this.Url = Settings.Default.N3000_wgWS_appSpecailBLong_wsWGLongPacketConf;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0027EA15 File Offset: 0x0027DA15
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0027EA20 File Offset: 0x0027DA20
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/getCusttype", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getCusttype(string userid, string SN, string conContent)
		{
			return (string)base.Invoke("getCusttype", new object[] { userid, SN, conContent })[0];
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0027EA53 File Offset: 0x0027DA53
		public void getCusttypeAsync(string userid, string SN, string conContent)
		{
			this.getCusttypeAsync(userid, SN, conContent, null);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0027EA60 File Offset: 0x0027DA60
		public void getCusttypeAsync(string userid, string SN, string conContent, object userState)
		{
			if (this.getCusttypeOperationCompleted == null)
			{
				this.getCusttypeOperationCompleted = new SendOrPostCallback(this.OngetCusttypeOperationCompleted);
			}
			base.InvokeAsync("getCusttype", new object[] { userid, SN, conContent }, this.getCusttypeOperationCompleted, userState);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0027EAB0 File Offset: 0x0027DAB0
		private void OngetCusttypeOperationCompleted(object arg)
		{
			if (this.getCusttypeCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getCusttypeCompleted(this, new getCusttypeCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0027EAF8 File Offset: 0x0027DAF8
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/getLongCmd", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getLongCmd(string userid, string SN, string conContent)
		{
			return (string)base.Invoke("getLongCmd", new object[] { userid, SN, conContent })[0];
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0027EB2B File Offset: 0x0027DB2B
		public void getLongCmdAsync(string userid, string SN, string conContent)
		{
			this.getLongCmdAsync(userid, SN, conContent, null);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0027EB38 File Offset: 0x0027DB38
		public void getLongCmdAsync(string userid, string SN, string conContent, object userState)
		{
			if (this.getLongCmdOperationCompleted == null)
			{
				this.getLongCmdOperationCompleted = new SendOrPostCallback(this.OngetLongCmdOperationCompleted);
			}
			base.InvokeAsync("getLongCmd", new object[] { userid, SN, conContent }, this.getLongCmdOperationCompleted, userState);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0027EB88 File Offset: 0x0027DB88
		private void OngetLongCmdOperationCompleted(object arg)
		{
			if (this.getLongCmdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getLongCmdCompleted(this, new getLongCmdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0027EBCD File Offset: 0x0027DBCD
		[SoapDocumentMethod("https://app.znsmart.com/wsWGLongPacketConf.asmx/HelloWorld", RequestNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGLongPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string HelloWorld()
		{
			return (string)base.Invoke("HelloWorld", new object[0])[0];
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0027EBE7 File Offset: 0x0027DBE7
		public void HelloWorldAsync()
		{
			this.HelloWorldAsync(null);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0027EBF0 File Offset: 0x0027DBF0
		public void HelloWorldAsync(object userState)
		{
			if (this.HelloWorldOperationCompleted == null)
			{
				this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
			}
			base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0027EC24 File Offset: 0x0027DC24
		private void OnHelloWorldOperationCompleted(object arg)
		{
			if (this.HelloWorldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0027EC6C File Offset: 0x0027DC6C
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0027ECB5 File Offset: 0x0027DCB5
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x0027ECBD File Offset: 0x0027DCBD
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

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x0027ECEC File Offset: 0x0027DCEC
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x0027ECF4 File Offset: 0x0027DCF4
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

		// Token: 0x04003A28 RID: 14888
		private SendOrPostCallback getCusttypeOperationCompleted;

		// Token: 0x04003A29 RID: 14889
		private SendOrPostCallback getLongCmdOperationCompleted;

		// Token: 0x04003A2A RID: 14890
		private SendOrPostCallback HelloWorldOperationCompleted;

		// Token: 0x04003A2B RID: 14891
		private bool useDefaultCredentialsSetExplicitly;
	}
}
