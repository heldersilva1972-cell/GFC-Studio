using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Basic
{
	// Token: 0x02000021 RID: 33
	public partial class dfrmOperatorUpdate : frmN3000
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0003EC04 File Offset: 0x0003DC04
		public dfrmOperatorUpdate()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0003EC24 File Offset: 0x0003DC24
		private int AddOperator()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.AddOperator_Acc();
			}
			int num = -1;
			try
			{
				string text = " SELECT f_OperatorID FROM [t_s_Operator] ";
				text = string.Concat(new string[]
				{
					text,
					"WHERE [f_OperatorName]=",
					wgTools.PrepareStrNUnicode(this.txtName.Text),
					" AND NOT [f_OperatorID]=",
					this.operatorID.ToString()
				});
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (sqlCommand.ExecuteScalar() != null)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return num;
						}
						text = " INSERT INTO [t_s_Operator] ";
						using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new string[]
						{
							text,
							"([f_OperatorName],  [f_Password]) Values(",
							wgTools.PrepareStrNUnicode(this.txtName.Text),
							" , ",
							wgTools.PrepareStrNUnicode(Program.Ept4Database(this.txtPassword.Text.Trim())),
							")"
						}), sqlConnection))
						{
							sqlCommand2.ExecuteNonQuery();
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0003EDE4 File Offset: 0x0003DDE4
		private int AddOperator_Acc()
		{
			int num = -1;
			try
			{
				string text = " SELECT f_OperatorID FROM [t_s_Operator] ";
				text = string.Concat(new string[]
				{
					text,
					"WHERE [f_OperatorName]=",
					wgTools.PrepareStrNUnicode(this.txtName.Text),
					" AND NOT [f_OperatorID]=",
					this.operatorID.ToString()
				});
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (oleDbCommand.ExecuteScalar() != null)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return num;
						}
						text = " INSERT INTO [t_s_Operator] ";
						using (OleDbCommand oleDbCommand2 = new OleDbCommand(string.Concat(new string[]
						{
							text,
							"([f_OperatorName],  [f_Password]) Values(",
							wgTools.PrepareStrNUnicode(this.txtName.Text),
							" , ",
							wgTools.PrepareStrNUnicode(Program.Ept4Database(this.txtPassword.Text.Trim())),
							")"
						}), oleDbConnection))
						{
							oleDbCommand2.ExecuteNonQuery();
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0003EF94 File Offset: 0x0003DF94
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0003EFA4 File Offset: 0x0003DFA4
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
			{
				XMessageBox.Show(this, CommonStr.strPersonNameNotEmpty, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.txtPassword.Text.Trim() != this.txtConfirmedPassword.Text.Trim())
			{
				XMessageBox.Show(this, CommonStr.strPasswordNotSame, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (wgAppConfig.getParamValBoolByNO(220))
			{
				Regex regex = new Regex("\r\n(?=.*[0-9])                     #必须包含数字\r\n(?=.*[a-zA-Z])                  #必须包含字母\r\n.{5,30}                         #至少5个字符，最多30个字符\r\n", RegexOptions.IgnorePatternWhitespace);
				if (!regex.IsMatch(this.txtPassword.Text.Trim()))
				{
					XMessageBox.Show(this, CommonStr.strPasswordInvalidFormat, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			if (this.operateMode == 1 || this.operateMode == 2)
			{
				if (this.EditOperator() >= 0)
				{
					base.DialogResult = DialogResult.OK;
					base.Close();
					return;
				}
			}
			else if (this.AddOperator() >= 0)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0003F0A0 File Offset: 0x0003E0A0
		private void dfrmOperatorUpdate_Load(object sender, EventArgs e)
		{
			if (this.operateMode == 2)
			{
				this.txtName.ReadOnly = true;
				this.txtName.TabStop = false;
			}
			this.txtName.Text = this.operatorName;
			this.txtPassword.MaxLength = wgAppConfig.PasswordMaxLenght;
			this.txtConfirmedPassword.MaxLength = wgAppConfig.PasswordMaxLenght;
			try
			{
				if (this.operateMode == 1)
				{
					base.ActiveControl = this.txtPassword;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0003F12C File Offset: 0x0003E12C
		private int EditOperator()
		{
			if (wgAppConfig.IsAccessDB)
			{
				return this.EditOperator_Acc();
			}
			int num = -1;
			try
			{
				string text = " SELECT f_OperatorID FROM [t_s_Operator] ";
				text = string.Concat(new string[]
				{
					text,
					"WHERE [f_OperatorName]=",
					wgTools.PrepareStrNUnicode(this.txtName.Text),
					" AND  NOT [f_OperatorID]=",
					this.operatorID.ToString()
				});
				using (SqlConnection sqlConnection = new SqlConnection(wgAppConfig.dbConString))
				{
					using (SqlCommand sqlCommand = new SqlCommand(text, sqlConnection))
					{
						sqlConnection.Open();
						if (sqlCommand.ExecuteScalar() != null)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return num;
						}
						text = " UPDATE [t_s_Operator] ";
						using (SqlCommand sqlCommand2 = new SqlCommand(string.Concat(new string[]
						{
							text,
							"SET [f_OperatorName]=",
							wgTools.PrepareStrNUnicode(this.txtName.Text),
							" , [f_Password]= ",
							wgTools.PrepareStrNUnicode(Program.Ept4Database(this.txtPassword.Text.Trim())),
							" WHERE [f_OperatorID]=",
							this.operatorID.ToString()
						}), sqlConnection))
						{
							sqlCommand2.ExecuteNonQuery();
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0003F2F8 File Offset: 0x0003E2F8
		private int EditOperator_Acc()
		{
			int num = -1;
			try
			{
				string text = " SELECT f_OperatorID FROM [t_s_Operator] ";
				text = string.Concat(new string[]
				{
					text,
					"WHERE [f_OperatorName]=",
					wgTools.PrepareStrNUnicode(this.txtName.Text),
					" AND NOT [f_OperatorID]=",
					this.operatorID.ToString()
				});
				using (OleDbConnection oleDbConnection = new OleDbConnection(wgAppConfig.dbConString))
				{
					using (OleDbCommand oleDbCommand = new OleDbCommand(text, oleDbConnection))
					{
						oleDbConnection.Open();
						if (oleDbCommand.ExecuteScalar() != null)
						{
							XMessageBox.Show(this, CommonStr.strNameDuplicated, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return num;
						}
						text = " UPDATE [t_s_Operator] ";
						using (OleDbCommand oleDbCommand2 = new OleDbCommand(string.Concat(new string[]
						{
							text,
							"SET [f_OperatorName]=",
							wgTools.PrepareStrNUnicode(this.txtName.Text),
							" , [f_Password]= ",
							wgTools.PrepareStrNUnicode(Program.Ept4Database(this.txtPassword.Text.Trim())),
							" WHERE [f_OperatorID]=",
							this.operatorID.ToString()
						}), oleDbConnection))
						{
							oleDbCommand2.ExecuteNonQuery();
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x040003FB RID: 1019
		public int operateMode;

		// Token: 0x040003FC RID: 1020
		public int operatorID = -1;

		// Token: 0x040003FD RID: 1021
		public string operatorName = "";
	}
}
