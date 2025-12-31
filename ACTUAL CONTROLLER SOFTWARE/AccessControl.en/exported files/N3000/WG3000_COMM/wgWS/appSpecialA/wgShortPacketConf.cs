using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using WG3000_COMM.Properties;

namespace WG3000_COMM.wgWS.appSpecialA
{
	// Token: 0x020003A1 RID: 929
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "wgShortPacketConfSoap", Namespace = "https://app.znsmart.com/wsWGShortPacketConf.asmx")]
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.6.1099.0")]
	public class wgShortPacketConf : SoapHttpClientProtocol
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060021A2 RID: 8610 RVA: 0x0027ED54 File Offset: 0x0027DD54
		// (remove) Token: 0x060021A3 RID: 8611 RVA: 0x0027ED8C File Offset: 0x0027DD8C
		public event getCmdCompletedEventHandler getCmdCompleted;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060021A4 RID: 8612 RVA: 0x0027EDC4 File Offset: 0x0027DDC4
		// (remove) Token: 0x060021A5 RID: 8613 RVA: 0x0027EDFC File Offset: 0x0027DDFC
		public event HelloWorldCompletedEventHandler HelloWorldCompleted;

		// Token: 0x060021A6 RID: 8614 RVA: 0x0027EE31 File Offset: 0x0027DE31
		public wgShortPacketConf()
		{
			this.Url = Settings.Default.N3000_cn_com_wiegand_app1_wgShortPacketConf;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0027EE6D File Offset: 0x0027DE6D
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0027EE78 File Offset: 0x0027DE78
		[SoapDocumentMethod("https://app.znsmart.com/wsWGShortPacketConf.asmx/getCmd", RequestNamespace = "https://app.znsmart.com/wsWGShortPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGShortPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string getCmd(string userid, string SN, string conContent)
		{
			return (string)base.Invoke("getCmd", new object[] { userid, SN, conContent })[0];
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0027EEAB File Offset: 0x0027DEAB
		public void getCmdAsync(string userid, string SN, string conContent)
		{
			this.getCmdAsync(userid, SN, conContent, null);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0027EEB8 File Offset: 0x0027DEB8
		public void getCmdAsync(string userid, string SN, string conContent, object userState)
		{
			if (this.getCmdOperationCompleted == null)
			{
				this.getCmdOperationCompleted = new SendOrPostCallback(this.OngetCmdOperationCompleted);
			}
			base.InvokeAsync("getCmd", new object[] { userid, SN, conContent }, this.getCmdOperationCompleted, userState);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0027EF06 File Offset: 0x0027DF06
		[SoapDocumentMethod("https://app.znsmart.com/wsWGShortPacketConf.asmx/HelloWorld", RequestNamespace = "https://app.znsmart.com/wsWGShortPacketConf.asmx", ResponseNamespace = "https://app.znsmart.com/wsWGShortPacketConf.asmx", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string HelloWorld()
		{
			return (string)base.Invoke("HelloWorld", new object[0])[0];
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0027EF20 File Offset: 0x0027DF20
		public void HelloWorldAsync()
		{
			this.HelloWorldAsync(null);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0027EF29 File Offset: 0x0027DF29
		public void HelloWorldAsync(object userState)
		{
			if (this.HelloWorldOperationCompleted == null)
			{
				this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
			}
			base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0027EF60 File Offset: 0x0027DF60
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0027EFAC File Offset: 0x0027DFAC
		private void OngetCmdOperationCompleted(object arg)
		{
			if (this.getCmdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.getCmdCompleted(this, new getCmdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0027EFF4 File Offset: 0x0027DFF4
		private void OnHelloWorldOperationCompleted(object arg)
		{
			if (this.HelloWorldCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x0027F039 File Offset: 0x0027E039
		// (set) Token: 0x060021B2 RID: 8626 RVA: 0x0027F041 File Offset: 0x0027E041
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

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x0027F070 File Offset: 0x0027E070
		// (set) Token: 0x060021B4 RID: 8628 RVA: 0x0027F078 File Offset: 0x0027E078
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

		// Token: 0x04003A31 RID: 14897
		private SendOrPostCallback getCmdOperationCompleted;

		// Token: 0x04003A32 RID: 14898
		private SendOrPostCallback HelloWorldOperationCompleted;

		// Token: 0x04003A33 RID: 14899
		private bool useDefaultCredentialsSetExplicitly;
	}
}
