namespace WG3000_COMM.ExtendFunc.Map
{
	// Token: 0x020002F3 RID: 755
	public partial class frmMaps : global::WG3000_COMM.Core.frmN3000
	{
		// Token: 0x06001601 RID: 5633 RVA: 0x001BBC28 File Offset: 0x001BAC28
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.photoMemoryStream != null)
			{
				this.photoMemoryStream.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x001BBC60 File Offset: 0x001BAC60
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::WG3000_COMM.ExtendFunc.Map.frmMaps));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.c1tabMaps = new global::System.Windows.Forms.TabControl();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.C1CmnuMap = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmdAddDoorByLoc = new global::System.Windows.Forms.ToolStripMenuItem();
			this.C1CmnuDoor = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.openDoorToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.Timer2 = new global::System.Windows.Forms.Timer(this.components);
			this.C1ToolBar4MapEdit = new global::System.Windows.Forms.ToolStrip();
			this.cmdAddMap = new global::System.Windows.Forms.ToolStripButton();
			this.cmdDeleteMap = new global::System.Windows.Forms.ToolStripButton();
			this.cmdChangeMapName = new global::System.Windows.Forms.ToolStripButton();
			this.cmdAddDoor = new global::System.Windows.Forms.ToolStripButton();
			this.cmdDeleteDoor = new global::System.Windows.Forms.ToolStripButton();
			this.cmdSaveMap = new global::System.Windows.Forms.ToolStripButton();
			this.cmdCancelAndExit = new global::System.Windows.Forms.ToolStripButton();
			this.C1ToolBar4MapOperate = new global::System.Windows.Forms.ToolStrip();
			this.cmdCloseMaps = new global::System.Windows.Forms.ToolStripButton();
			this.cmdZoomIn = new global::System.Windows.Forms.ToolStripButton();
			this.cmdZoomOut = new global::System.Windows.Forms.ToolStripButton();
			this.cmdEditMap = new global::System.Windows.Forms.ToolStripButton();
			this.cmdWatchCurrentMap = new global::System.Windows.Forms.ToolStripButton();
			this.cmdWatchAllMaps = new global::System.Windows.Forms.ToolStripButton();
			this.btnStopOthers = new global::System.Windows.Forms.ToolStripButton();
			this.dgvRunInfo = new global::System.Windows.Forms.DataGridView();
			this.f_Category = new global::System.Windows.Forms.DataGridViewImageColumn();
			this.f_RecID = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Time = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Desc = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Info = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_Detail = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.f_MjRecStr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuHide = new global::System.Windows.Forms.ToolStripMenuItem();
			this.c1tabMaps.SuspendLayout();
			this.C1CmnuMap.SuspendLayout();
			this.C1CmnuDoor.SuspendLayout();
			this.C1ToolBar4MapEdit.SuspendLayout();
			this.C1ToolBar4MapOperate.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.c1tabMaps, "c1tabMaps");
			this.c1tabMaps.Controls.Add(this.tabPage2);
			this.c1tabMaps.Controls.Add(this.tabPage1);
			this.c1tabMaps.Name = "c1tabMaps";
			this.c1tabMaps.SelectedIndex = 0;
			componentResourceManager.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.C1CmnuMap, "C1CmnuMap");
			this.C1CmnuMap.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.cmdAddDoorByLoc });
			this.C1CmnuMap.Name = "C1CmnuMap";
			componentResourceManager.ApplyResources(this.cmdAddDoorByLoc, "cmdAddDoorByLoc");
			this.cmdAddDoorByLoc.Name = "cmdAddDoorByLoc";
			this.cmdAddDoorByLoc.Click += new global::System.EventHandler(this.cmdAddDoor_Click);
			componentResourceManager.ApplyResources(this.C1CmnuDoor, "C1CmnuDoor");
			this.C1CmnuDoor.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.openDoorToolStripMenuItem });
			this.C1CmnuDoor.Name = "C1CmnuMap";
			componentResourceManager.ApplyResources(this.openDoorToolStripMenuItem, "openDoorToolStripMenuItem");
			this.openDoorToolStripMenuItem.Name = "openDoorToolStripMenuItem";
			this.Timer2.Tick += new global::System.EventHandler(this.Timer2_Tick);
			componentResourceManager.ApplyResources(this.C1ToolBar4MapEdit, "C1ToolBar4MapEdit");
			this.C1ToolBar4MapEdit.BackColor = global::System.Drawing.Color.Transparent;
			this.C1ToolBar4MapEdit.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.C1ToolBar4MapEdit.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.cmdAddMap, this.cmdDeleteMap, this.cmdChangeMapName, this.cmdAddDoor, this.cmdDeleteDoor, this.cmdSaveMap, this.cmdCancelAndExit });
			this.C1ToolBar4MapEdit.Name = "C1ToolBar4MapEdit";
			componentResourceManager.ApplyResources(this.cmdAddMap, "cmdAddMap");
			this.cmdAddMap.ForeColor = global::System.Drawing.Color.White;
			this.cmdAddMap.Image = global::WG3000_COMM.Properties.Resources.pTools_Add_Auto;
			this.cmdAddMap.Name = "cmdAddMap";
			this.cmdAddMap.Click += new global::System.EventHandler(this.cmdAddMap_Click);
			componentResourceManager.ApplyResources(this.cmdDeleteMap, "cmdDeleteMap");
			this.cmdDeleteMap.ForeColor = global::System.Drawing.Color.White;
			this.cmdDeleteMap.Image = global::WG3000_COMM.Properties.Resources.pTools_CardLost;
			this.cmdDeleteMap.Name = "cmdDeleteMap";
			this.cmdDeleteMap.Click += new global::System.EventHandler(this.cmdDeleteMap_Click);
			componentResourceManager.ApplyResources(this.cmdChangeMapName, "cmdChangeMapName");
			this.cmdChangeMapName.ForeColor = global::System.Drawing.Color.White;
			this.cmdChangeMapName.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit_Batch;
			this.cmdChangeMapName.Name = "cmdChangeMapName";
			this.cmdChangeMapName.Click += new global::System.EventHandler(this.cmdChangeMapName_Click);
			componentResourceManager.ApplyResources(this.cmdAddDoor, "cmdAddDoor");
			this.cmdAddDoor.ForeColor = global::System.Drawing.Color.White;
			this.cmdAddDoor.Image = global::WG3000_COMM.Properties.Resources.pTools_Add;
			this.cmdAddDoor.Name = "cmdAddDoor";
			this.cmdAddDoor.Click += new global::System.EventHandler(this.cmdAddDoor_Click);
			componentResourceManager.ApplyResources(this.cmdDeleteDoor, "cmdDeleteDoor");
			this.cmdDeleteDoor.ForeColor = global::System.Drawing.Color.White;
			this.cmdDeleteDoor.Image = global::WG3000_COMM.Properties.Resources.pTools_Del;
			this.cmdDeleteDoor.Name = "cmdDeleteDoor";
			this.cmdDeleteDoor.Click += new global::System.EventHandler(this.cmdDeleteDoor_Click);
			componentResourceManager.ApplyResources(this.cmdSaveMap, "cmdSaveMap");
			this.cmdSaveMap.ForeColor = global::System.Drawing.Color.White;
			this.cmdSaveMap.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Save;
			this.cmdSaveMap.Name = "cmdSaveMap";
			this.cmdSaveMap.Click += new global::System.EventHandler(this.cmdSaveMap_Click);
			componentResourceManager.ApplyResources(this.cmdCancelAndExit, "cmdCancelAndExit");
			this.cmdCancelAndExit.ForeColor = global::System.Drawing.Color.White;
			this.cmdCancelAndExit.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Cancel;
			this.cmdCancelAndExit.Name = "cmdCancelAndExit";
			this.cmdCancelAndExit.Click += new global::System.EventHandler(this.cmdCancelAndExit_Click);
			componentResourceManager.ApplyResources(this.C1ToolBar4MapOperate, "C1ToolBar4MapOperate");
			this.C1ToolBar4MapOperate.BackColor = global::System.Drawing.Color.Transparent;
			this.C1ToolBar4MapOperate.BackgroundImage = global::WG3000_COMM.Properties.Resources.pChild_title;
			this.C1ToolBar4MapOperate.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.cmdCloseMaps, this.cmdZoomIn, this.cmdZoomOut, this.cmdEditMap, this.cmdWatchCurrentMap, this.cmdWatchAllMaps, this.btnStopOthers });
			this.C1ToolBar4MapOperate.Name = "C1ToolBar4MapOperate";
			componentResourceManager.ApplyResources(this.cmdCloseMaps, "cmdCloseMaps");
			this.cmdCloseMaps.ForeColor = global::System.Drawing.Color.White;
			this.cmdCloseMaps.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_Close;
			this.cmdCloseMaps.Name = "cmdCloseMaps";
			this.cmdCloseMaps.Click += new global::System.EventHandler(this.cmdCloseMaps_Click);
			componentResourceManager.ApplyResources(this.cmdZoomIn, "cmdZoomIn");
			this.cmdZoomIn.ForeColor = global::System.Drawing.Color.White;
			this.cmdZoomIn.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_ZoomLarge;
			this.cmdZoomIn.Name = "cmdZoomIn";
			this.cmdZoomIn.Click += new global::System.EventHandler(this.cmdZoomIn_Click);
			componentResourceManager.ApplyResources(this.cmdZoomOut, "cmdZoomOut");
			this.cmdZoomOut.ForeColor = global::System.Drawing.Color.White;
			this.cmdZoomOut.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_ZoomSmall;
			this.cmdZoomOut.Name = "cmdZoomOut";
			this.cmdZoomOut.Click += new global::System.EventHandler(this.cmdZoomOut_Click);
			componentResourceManager.ApplyResources(this.cmdEditMap, "cmdEditMap");
			this.cmdEditMap.ForeColor = global::System.Drawing.Color.White;
			this.cmdEditMap.Image = global::WG3000_COMM.Properties.Resources.pTools_Edit;
			this.cmdEditMap.Name = "cmdEditMap";
			this.cmdEditMap.Click += new global::System.EventHandler(this.cmdEditMap_Click);
			componentResourceManager.ApplyResources(this.cmdWatchCurrentMap, "cmdWatchCurrentMap");
			this.cmdWatchCurrentMap.ForeColor = global::System.Drawing.Color.White;
			this.cmdWatchCurrentMap.Image = global::WG3000_COMM.Properties.Resources.pConsole_Monitor;
			this.cmdWatchCurrentMap.Name = "cmdWatchCurrentMap";
			this.cmdWatchCurrentMap.Click += new global::System.EventHandler(this.cmdWatchCurrentMap_Click);
			componentResourceManager.ApplyResources(this.cmdWatchAllMaps, "cmdWatchAllMaps");
			this.cmdWatchAllMaps.ForeColor = global::System.Drawing.Color.White;
			this.cmdWatchAllMaps.Image = global::WG3000_COMM.Properties.Resources.pTools_Maps_SelectAll;
			this.cmdWatchAllMaps.Name = "cmdWatchAllMaps";
			this.cmdWatchAllMaps.Click += new global::System.EventHandler(this.cmdWatchAllMaps_Click);
			componentResourceManager.ApplyResources(this.btnStopOthers, "btnStopOthers");
			this.btnStopOthers.ForeColor = global::System.Drawing.Color.White;
			this.btnStopOthers.Image = global::WG3000_COMM.Properties.Resources.pConsole_Stop;
			this.btnStopOthers.Name = "btnStopOthers";
			this.btnStopOthers.Click += new global::System.EventHandler(this.btnStopOthers_Click);
			componentResourceManager.ApplyResources(this.dgvRunInfo, "dgvRunInfo");
			this.dgvRunInfo.AllowUserToAddRows = false;
			this.dgvRunInfo.AllowUserToDeleteRows = false;
			this.dgvRunInfo.BackgroundColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(124, 125, 156);
			dataGridViewCellStyle.Font = new global::System.Drawing.Font("宋体", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle.ForeColor = global::System.Drawing.Color.White;
			dataGridViewCellStyle.Padding = new global::System.Windows.Forms.Padding(0, 0, 0, 2);
			dataGridViewCellStyle.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = global::System.Windows.Forms.DataGridViewTriState.True;
			this.dgvRunInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvRunInfo.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvRunInfo.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.f_Category, this.f_RecID, this.f_Time, this.f_Desc, this.f_Info, this.f_Detail, this.f_MjRecStr });
			this.dgvRunInfo.ContextMenuStrip = this.contextMenuStrip1;
			this.dgvRunInfo.EnableHeadersVisualStyles = false;
			this.dgvRunInfo.MultiSelect = false;
			this.dgvRunInfo.Name = "dgvRunInfo";
			this.dgvRunInfo.ReadOnly = true;
			this.dgvRunInfo.RowHeadersVisible = false;
			this.dgvRunInfo.RowTemplate.Height = 23;
			this.dgvRunInfo.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvRunInfo.CellFormatting += new global::System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRunInfo_CellFormatting);
			this.dgvRunInfo.DataError += new global::System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRunInfo_DataError);
			componentResourceManager.ApplyResources(this.f_Category, "f_Category");
			this.f_Category.Name = "f_Category";
			this.f_Category.ReadOnly = true;
			this.f_Category.Resizable = global::System.Windows.Forms.DataGridViewTriState.True;
			this.f_Category.SortMode = global::System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.f_RecID, "f_RecID");
			this.f_RecID.Name = "f_RecID";
			this.f_RecID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Time, "f_Time");
			this.f_Time.Name = "f_Time";
			this.f_Time.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Desc, "f_Desc");
			this.f_Desc.Name = "f_Desc";
			this.f_Desc.ReadOnly = true;
			this.f_Info.AutoSizeMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.f_Info, "f_Info");
			this.f_Info.Name = "f_Info";
			this.f_Info.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_Detail, "f_Detail");
			this.f_Detail.Name = "f_Detail";
			this.f_Detail.ReadOnly = true;
			componentResourceManager.ApplyResources(this.f_MjRecStr, "f_MjRecStr");
			this.f_MjRecStr.Name = "f_MjRecStr";
			this.f_MjRecStr.ReadOnly = true;
			componentResourceManager.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.mnuHide });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.mnuHide, "mnuHide");
			this.mnuHide.Name = "mnuHide";
			this.mnuHide.Click += new global::System.EventHandler(this.mnuHide_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.c1tabMaps);
			base.Controls.Add(this.dgvRunInfo);
			base.Controls.Add(this.C1ToolBar4MapEdit);
			base.Controls.Add(this.C1ToolBar4MapOperate);
			base.Name = "frmMaps";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmMaps_FormClosing);
			base.Load += new global::System.EventHandler(this.frmMaps_Load);
			this.c1tabMaps.ResumeLayout(false);
			this.C1CmnuMap.ResumeLayout(false);
			this.C1CmnuDoor.ResumeLayout(false);
			this.C1ToolBar4MapEdit.ResumeLayout(false);
			this.C1ToolBar4MapEdit.PerformLayout();
			this.C1ToolBar4MapOperate.ResumeLayout(false);
			this.C1ToolBar4MapOperate.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvRunInfo).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04002D6C RID: 11628
		private global::System.IO.MemoryStream photoMemoryStream;

		// Token: 0x04002D77 RID: 11639
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04002D78 RID: 11640
		private global::System.Windows.Forms.ToolStripButton btnStopOthers;

		// Token: 0x04002D79 RID: 11641
		private global::System.Windows.Forms.ContextMenuStrip C1CmnuDoor;

		// Token: 0x04002D7A RID: 11642
		private global::System.Windows.Forms.ContextMenuStrip C1CmnuMap;

		// Token: 0x04002D7B RID: 11643
		private global::System.Windows.Forms.TabControl c1tabMaps;

		// Token: 0x04002D7C RID: 11644
		private global::System.Windows.Forms.ToolStrip C1ToolBar4MapEdit;

		// Token: 0x04002D7D RID: 11645
		private global::System.Windows.Forms.ToolStrip C1ToolBar4MapOperate;

		// Token: 0x04002D7E RID: 11646
		private global::System.Windows.Forms.ToolStripButton cmdAddDoor;

		// Token: 0x04002D7F RID: 11647
		private global::System.Windows.Forms.ToolStripMenuItem cmdAddDoorByLoc;

		// Token: 0x04002D80 RID: 11648
		private global::System.Windows.Forms.ToolStripButton cmdAddMap;

		// Token: 0x04002D81 RID: 11649
		private global::System.Windows.Forms.ToolStripButton cmdCancelAndExit;

		// Token: 0x04002D82 RID: 11650
		private global::System.Windows.Forms.ToolStripButton cmdChangeMapName;

		// Token: 0x04002D83 RID: 11651
		private global::System.Windows.Forms.ToolStripButton cmdCloseMaps;

		// Token: 0x04002D84 RID: 11652
		private global::System.Windows.Forms.ToolStripButton cmdDeleteDoor;

		// Token: 0x04002D85 RID: 11653
		private global::System.Windows.Forms.ToolStripButton cmdDeleteMap;

		// Token: 0x04002D86 RID: 11654
		private global::System.Windows.Forms.ToolStripButton cmdEditMap;

		// Token: 0x04002D87 RID: 11655
		private global::System.Windows.Forms.ToolStripButton cmdSaveMap;

		// Token: 0x04002D88 RID: 11656
		private global::System.Windows.Forms.ToolStripButton cmdWatchAllMaps;

		// Token: 0x04002D89 RID: 11657
		private global::System.Windows.Forms.ToolStripButton cmdWatchCurrentMap;

		// Token: 0x04002D8A RID: 11658
		private global::System.Windows.Forms.ToolStripButton cmdZoomIn;

		// Token: 0x04002D8B RID: 11659
		private global::System.Windows.Forms.ToolStripButton cmdZoomOut;

		// Token: 0x04002D8C RID: 11660
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04002D8D RID: 11661
		private global::System.Windows.Forms.DataGridView dgvRunInfo;

		// Token: 0x04002D8E RID: 11662
		private global::System.Windows.Forms.DataGridViewImageColumn f_Category;

		// Token: 0x04002D8F RID: 11663
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Desc;

		// Token: 0x04002D90 RID: 11664
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Detail;

		// Token: 0x04002D91 RID: 11665
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Info;

		// Token: 0x04002D92 RID: 11666
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_MjRecStr;

		// Token: 0x04002D93 RID: 11667
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_RecID;

		// Token: 0x04002D94 RID: 11668
		private global::System.Windows.Forms.DataGridViewTextBoxColumn f_Time;

		// Token: 0x04002D95 RID: 11669
		private global::System.Windows.Forms.ToolStripMenuItem mnuHide;

		// Token: 0x04002D96 RID: 11670
		private global::System.Windows.Forms.ToolStripMenuItem openDoorToolStripMenuItem;

		// Token: 0x04002D97 RID: 11671
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04002D98 RID: 11672
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04002D99 RID: 11673
		private global::System.Windows.Forms.Timer Timer2;
	}
}
