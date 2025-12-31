namespace WG3000_COMM.ExtendFunc.Meal
{
	// Token: 0x020002F4 RID: 756
	public partial class dfrmMealGroup : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001607 RID: 5639 RVA: 0x001BCC05 File Offset: 0x001BBC05
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x001BCC24 File Offset: 0x001BBC24
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Meal.dfrmMealGroup));
			this.nudOther = new global::System.Windows.Forms.NumericUpDown();
			this.nudEvening = new global::System.Windows.Forms.NumericUpDown();
			this.nudLunch = new global::System.Windows.Forms.NumericUpDown();
			this.label3 = new global::System.Windows.Forms.Label();
			this.nudMorning = new global::System.Windows.Forms.NumericUpDown();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.chkActive = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.nudOther).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudEvening).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLunch).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudMorning).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.nudOther, "nudOther");
			this.nudOther.DecimalPlaces = 2;
			this.nudOther.Name = "nudOther";
			componentResourceManager.ApplyResources(this.nudEvening, "nudEvening");
			this.nudEvening.DecimalPlaces = 2;
			this.nudEvening.Name = "nudEvening";
			componentResourceManager.ApplyResources(this.nudLunch, "nudLunch");
			this.nudLunch.DecimalPlaces = 2;
			this.nudLunch.Name = "nudLunch";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = global::System.Drawing.Color.White;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.nudMorning, "nudMorning");
			this.nudMorning.DecimalPlaces = 2;
			this.nudMorning.Name = "nudMorning";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = global::System.Drawing.Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = global::System.Drawing.Color.White;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = global::System.Drawing.Color.White;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackColor = global::System.Drawing.Color.Transparent;
			this.btnOK.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnOK.ForeColor = global::System.Drawing.Color.White;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackColor = global::System.Drawing.Color.Transparent;
			this.btnCancel.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ForeColor = global::System.Drawing.Color.White;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.chkActive, "chkActive");
			this.chkActive.Checked = true;
			this.chkActive.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.chkActive.ForeColor = global::System.Drawing.Color.White;
			this.chkActive.Name = "chkActive";
			this.chkActive.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.chkActive);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.nudOther);
			base.Controls.Add(this.nudEvening);
			base.Controls.Add(this.nudLunch);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.nudMorning);
			base.Name = "dfrmMealGroup";
			base.Load += new global::System.EventHandler(this.dfrmMealGroup_Load);
			((global::System.ComponentModel.ISupportInitialize)this.nudOther).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudEvening).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudLunch).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.nudMorning).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002DA2 RID: 11682
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002DA3 RID: 11683
		private global::System.Windows.Forms.CheckBox chkActive;

		// Token: 0x04002DA4 RID: 11684
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04002DA5 RID: 11685
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04002DA6 RID: 11686
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04002DA7 RID: 11687
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04002DA8 RID: 11688
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04002DA9 RID: 11689
		private global::System.Windows.Forms.NumericUpDown nudEvening;

		// Token: 0x04002DAA RID: 11690
		private global::System.Windows.Forms.NumericUpDown nudLunch;

		// Token: 0x04002DAB RID: 11691
		private global::System.Windows.Forms.NumericUpDown nudMorning;

		// Token: 0x04002DAC RID: 11692
		private global::System.Windows.Forms.NumericUpDown nudOther;

		// Token: 0x04002DAD RID: 11693
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002DAE RID: 11694
		internal global::System.Windows.Forms.Button btnOK;
	}
}
