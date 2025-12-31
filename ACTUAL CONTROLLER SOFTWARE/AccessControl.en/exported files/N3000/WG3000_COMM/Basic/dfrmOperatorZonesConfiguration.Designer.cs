namespace WG3000_COMM.Basic
{
	// Token: 0x02000022 RID: 34
	public partial class dfrmOperatorZonesConfiguration : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060001EC RID: 492 RVA: 0x00040264 File Offset: 0x0003F264
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00040284 File Offset: 0x0003F284
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.Basic.dfrmOperatorZonesConfiguration));
			this.btnZoneManage = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.btnDeleteAllGroups = new global::System.Windows.Forms.Button();
			this.btnDeleteOneGroup = new global::System.Windows.Forms.Button();
			this.btnAddOneGroup = new global::System.Windows.Forms.Button();
			this.btnAddAllGroups = new global::System.Windows.Forms.Button();
			this.Label11 = new global::System.Windows.Forms.Label();
			this.Label10 = new global::System.Windows.Forms.Label();
			this.lstSelectedGroups = new global::System.Windows.Forms.ListBox();
			this.lstOptionalGroups = new global::System.Windows.Forms.ListBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnZoneManage, "btnZoneManage");
			this.btnZoneManage.BackColor = global::System.Drawing.Color.Transparent;
			this.btnZoneManage.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnZoneManage.ForeColor = global::System.Drawing.Color.White;
			this.btnZoneManage.Name = "btnZoneManage";
			this.btnZoneManage.UseVisualStyleBackColor = false;
			this.btnZoneManage.Click += new global::System.EventHandler(this.btnZoneManage_Click);
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.BackColor = global::System.Drawing.Color.Transparent;
			this.button1.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button1.ForeColor = global::System.Drawing.Color.White;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.btnOk_Click);
			componentResourceManager.ApplyResources(this.button2, "button2");
			this.button2.BackColor = global::System.Drawing.Color.Transparent;
			this.button2.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.button2.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.button2.ForeColor = global::System.Drawing.Color.White;
			this.button2.Name = "button2";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new global::System.EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.btnDeleteAllGroups, "btnDeleteAllGroups");
			this.btnDeleteAllGroups.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteAllGroups.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteAllGroups.Name = "btnDeleteAllGroups";
			this.btnDeleteAllGroups.UseVisualStyleBackColor = true;
			this.btnDeleteAllGroups.Click += new global::System.EventHandler(this.btnDeleteAllGroups_Click);
			componentResourceManager.ApplyResources(this.btnDeleteOneGroup, "btnDeleteOneGroup");
			this.btnDeleteOneGroup.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnDeleteOneGroup.ForeColor = global::System.Drawing.Color.White;
			this.btnDeleteOneGroup.Name = "btnDeleteOneGroup";
			this.btnDeleteOneGroup.UseVisualStyleBackColor = true;
			this.btnDeleteOneGroup.Click += new global::System.EventHandler(this.btnDeleteOneGroup_Click);
			componentResourceManager.ApplyResources(this.btnAddOneGroup, "btnAddOneGroup");
			this.btnAddOneGroup.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddOneGroup.ForeColor = global::System.Drawing.Color.White;
			this.btnAddOneGroup.Name = "btnAddOneGroup";
			this.btnAddOneGroup.UseVisualStyleBackColor = true;
			this.btnAddOneGroup.Click += new global::System.EventHandler(this.btnAddOneGroup_Click);
			componentResourceManager.ApplyResources(this.btnAddAllGroups, "btnAddAllGroups");
			this.btnAddAllGroups.BackgroundImage = global::WG3000_COMM.Properties.Resources.pMain_button_normal;
			this.btnAddAllGroups.ForeColor = global::System.Drawing.Color.White;
			this.btnAddAllGroups.Name = "btnAddAllGroups";
			this.btnAddAllGroups.UseVisualStyleBackColor = true;
			this.btnAddAllGroups.Click += new global::System.EventHandler(this.btnAddAllGroups_Click);
			componentResourceManager.ApplyResources(this.Label11, "Label11");
			this.Label11.ForeColor = global::System.Drawing.Color.White;
			this.Label11.Name = "Label11";
			componentResourceManager.ApplyResources(this.Label10, "Label10");
			this.Label10.ForeColor = global::System.Drawing.Color.White;
			this.Label10.Name = "Label10";
			componentResourceManager.ApplyResources(this.lstSelectedGroups, "lstSelectedGroups");
			this.lstSelectedGroups.Name = "lstSelectedGroups";
			this.lstSelectedGroups.DoubleClick += new global::System.EventHandler(this.btnDeleteOneGroup_Click);
			componentResourceManager.ApplyResources(this.lstOptionalGroups, "lstOptionalGroups");
			this.lstOptionalGroups.Name = "lstOptionalGroups";
			this.lstOptionalGroups.DoubleClick += new global::System.EventHandler(this.btnAddOneGroup_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnZoneManage);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.btnDeleteAllGroups);
			base.Controls.Add(this.btnDeleteOneGroup);
			base.Controls.Add(this.btnAddOneGroup);
			base.Controls.Add(this.btnAddAllGroups);
			base.Controls.Add(this.Label11);
			base.Controls.Add(this.Label10);
			base.Controls.Add(this.lstSelectedGroups);
			base.Controls.Add(this.lstOptionalGroups);
			base.MinimizeBox = false;
			base.Name = "dfrmOperatorZonesConfiguration";
			base.Load += new global::System.EventHandler(this.dfrmSwitchGroupsConfiguration_Load);
			base.ResumeLayout(false);
		}

		// Token: 0x0400040D RID: 1037
		private global::System.ComponentModel.Container components;

		// Token: 0x0400040E RID: 1038
		private global::System.Windows.Forms.Button btnAddAllGroups;

		// Token: 0x0400040F RID: 1039
		private global::System.Windows.Forms.Button btnAddOneGroup;

		// Token: 0x04000410 RID: 1040
		private global::System.Windows.Forms.Button btnDeleteAllGroups;

		// Token: 0x04000411 RID: 1041
		private global::System.Windows.Forms.Button btnDeleteOneGroup;

		// Token: 0x04000412 RID: 1042
		private global::System.Windows.Forms.Button btnZoneManage;

		// Token: 0x04000413 RID: 1043
		internal global::System.Windows.Forms.Button button1;

		// Token: 0x04000414 RID: 1044
		internal global::System.Windows.Forms.Button button2;

		// Token: 0x04000415 RID: 1045
		internal global::System.Windows.Forms.Label Label10;

		// Token: 0x04000416 RID: 1046
		internal global::System.Windows.Forms.Label Label11;

		// Token: 0x04000417 RID: 1047
		internal global::System.Windows.Forms.ListBox lstOptionalGroups;

		// Token: 0x04000418 RID: 1048
		internal global::System.Windows.Forms.ListBox lstSelectedGroups;
	}
}
