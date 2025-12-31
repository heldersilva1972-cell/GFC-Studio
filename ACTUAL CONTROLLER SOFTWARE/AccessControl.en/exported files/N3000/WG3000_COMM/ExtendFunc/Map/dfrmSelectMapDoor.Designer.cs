namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F2 RID: 754
	public partial class dfrmSelectMapDoor : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x001B7FC5 File Offset: 0x001B6FC5
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x001B7FE4 File Offset: 0x001B6FE4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Map.dfrmSelectMapDoor));
			this.btnOK = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.lstUnMappedDoors = new global::System.Windows.Forms.ListBox();
			this.lstMappedDoors = new global::System.Windows.Forms.ListBox();
			this.Label1 = new global::System.Windows.Forms.Label();
			this.Label2 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.lstUnMappedDoors, "lstUnMappedDoors");
			this.lstUnMappedDoors.Name = "lstUnMappedDoors";
			this.lstUnMappedDoors.SelectedIndexChanged += new global::System.EventHandler(this.lstUnMappedDoors_SelectedIndexChanged);
			this.lstUnMappedDoors.DoubleClick += new global::System.EventHandler(this.lstUnMappedDoors_DoubleClick);
			this.lstUnMappedDoors.MouseDoubleClick += new global::System.Windows.Forms.MouseEventHandler(this.lstUnMappedDoors_MouseDoubleClick);
			componentResourceManager.ApplyResources(this.lstMappedDoors, "lstMappedDoors");
			this.lstMappedDoors.Name = "lstMappedDoors";
			this.lstMappedDoors.SelectedIndexChanged += new global::System.EventHandler(this.lstMappedDoors_SelectedIndexChanged);
			this.lstMappedDoors.DoubleClick += new global::System.EventHandler(this.lstMappedDoors_DoubleClick);
			componentResourceManager.ApplyResources(this.Label1, "Label1");
			this.Label1.BackColor = global::System.Drawing.Color.Transparent;
			this.Label1.ForeColor = global::System.Drawing.Color.White;
			this.Label1.Name = "Label1";
			componentResourceManager.ApplyResources(this.Label2, "Label2");
			this.Label2.BackColor = global::System.Drawing.Color.Transparent;
			this.Label2.ForeColor = global::System.Drawing.Color.White;
			this.Label2.Name = "Label2";
			base.AcceptButton = this.btnOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.CancelButton = this.btnCancel;
			base.Controls.Add(this.Label1);
			base.Controls.Add(this.lstUnMappedDoors);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.lstMappedDoors);
			base.Controls.Add(this.Label2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dfrmSelectMapDoor";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.dfrmSelectMapDoor_FormClosing);
			base.Load += new global::System.EventHandler(this.dfrmSelectMapDoor_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.dfrmSelectMapDoor_KeyDown);
			base.ResumeLayout(false);
		}

		// Token: 0x04002D4E RID: 11598
		private global::System.ComponentModel.Container components;

		// Token: 0x04002D4F RID: 11599
		internal global::System.Windows.Forms.Button btnCancel;

		// Token: 0x04002D50 RID: 11600
		internal global::System.Windows.Forms.Button btnOK;

		// Token: 0x04002D51 RID: 11601
		internal global::System.Windows.Forms.Label Label1;

		// Token: 0x04002D52 RID: 11602
		internal global::System.Windows.Forms.Label Label2;

		// Token: 0x04002D53 RID: 11603
		internal global::System.Windows.Forms.ListBox lstMappedDoors;

		// Token: 0x04002D54 RID: 11604
		internal global::System.Windows.Forms.ListBox lstUnMappedDoors;
	}
}
