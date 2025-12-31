using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using WG3000_COMM.ResStrings;

namespace WG3000_COMM.Core
{
	// Token: 0x020001C8 RID: 456
	internal class DGVPrinter : IDisposable
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x000DF7A0 File Offset: 0x000DE7A0
		public DGVPrinter()
		{
			this.printDoc.PrintPage += this.printDoc_PrintPage;
			this.printDoc.BeginPrint += this.printDoc_BeginPrint;
			this.PrintMargins = new Margins(60, 60, 40, 40);
			this.pagenofont = new Font("Tahoma", 8f, FontStyle.Regular, GraphicsUnit.Point);
			this.pagenocolor = Color.Black;
			this.titlefont = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point);
			this.titlecolor = Color.Black;
			this.subtitlefont = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point);
			this.subtitlecolor = Color.Black;
			this.footerfont = new Font("Tahoma", 10f, FontStyle.Bold, GraphicsUnit.Point);
			this.footercolor = Color.Black;
			this.buildstringformat(ref this.titleformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
			this.buildstringformat(ref this.subtitleformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
			this.buildstringformat(ref this.footerformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
			this.buildstringformat(ref this.pagenumberformat, null, StringAlignment.Far, StringAlignment.Center, StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
			this.headercellformat = null;
			this.cellformat = null;
			this.Owner = null;
			this.PrintPreviewZoom = 1.0;
			this.headercellalignment = StringAlignment.Near;
			this.headercellformatflags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
			this.cellalignment = StringAlignment.Near;
			this.cellformatflags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000DF9B8 File Offset: 0x000DE9B8
		private void AdjustPageSets(Graphics g, DGVPrinter.PageDef pageset)
		{
			float num = this.rowheaderwidth;
			float num2 = 0f;
			for (int i = 0; i < pageset.colwidthsoverride.Count; i++)
			{
				if (pageset.colwidthsoverride[i] >= 0f)
				{
					num += pageset.colwidthsoverride[i];
				}
			}
			for (int i = 0; i < pageset.colwidths.Count; i++)
			{
				if (pageset.colwidthsoverride[i] < 0f)
				{
					num2 += pageset.colwidths[i];
				}
			}
			float num3;
			if (this.porportionalcolumns && 0f < num2)
			{
				num3 = ((float)this.printWidth - num) / num2;
			}
			else
			{
				num3 = 1f;
			}
			pageset.coltotalwidth = this.rowheaderwidth;
			for (int i = 0; i < pageset.colwidths.Count; i++)
			{
				if (pageset.colwidthsoverride[i] >= 0f)
				{
					pageset.colwidths[i] = pageset.colwidthsoverride[i];
				}
				else
				{
					List<float> list;
					int num4;
					(list = pageset.colwidths)[num4 = i] = list[num4] * num3;
				}
				pageset.coltotalwidth += pageset.colwidths[i];
			}
			if (DGVPrinter.Alignment.Left == this.tablealignment)
			{
				pageset.margins.Right = this.pageWidth - pageset.margins.Left - (int)pageset.coltotalwidth;
				if (0 > pageset.margins.Right)
				{
					pageset.margins.Right = 0;
					return;
				}
			}
			else if (DGVPrinter.Alignment.Right == this.tablealignment)
			{
				pageset.margins.Left = this.pageWidth - pageset.margins.Right - (int)pageset.coltotalwidth;
				if (0 > pageset.margins.Left)
				{
					pageset.margins.Left = 0;
					return;
				}
			}
			else if (DGVPrinter.Alignment.Center == this.tablealignment)
			{
				pageset.margins.Left = (this.pageWidth - (int)pageset.coltotalwidth) / 2;
				if (0 > pageset.margins.Left)
				{
					pageset.margins.Left = 0;
				}
				pageset.margins.Right = pageset.margins.Left;
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000DFBD4 File Offset: 0x000DEBD4
		private void buildstringformat(ref StringFormat format, DataGridViewCellStyle controlstyle, StringAlignment alignment, StringAlignment linealignment, StringFormatFlags flags, StringTrimming trim)
		{
			if (format == null)
			{
				format = new StringFormat();
			}
			format.Alignment = alignment;
			format.LineAlignment = linealignment;
			format.FormatFlags = flags;
			format.Trimming = trim;
			if (controlstyle != null)
			{
				DataGridViewContentAlignment alignment2 = controlstyle.Alignment;
				if (alignment2.ToString().Contains("Center"))
				{
					format.Alignment = StringAlignment.Center;
				}
				else if (alignment2.ToString().Contains("Left"))
				{
					format.Alignment = StringAlignment.Near;
				}
				else if (alignment2.ToString().Contains("Right"))
				{
					format.Alignment = StringAlignment.Far;
				}
				if (alignment2.ToString().Contains("Top"))
				{
					format.LineAlignment = StringAlignment.Near;
					return;
				}
				if (alignment2.ToString().Contains("Middle"))
				{
					format.LineAlignment = StringAlignment.Center;
					return;
				}
				if (alignment2.ToString().Contains("Bottom"))
				{
					format.LineAlignment = StringAlignment.Far;
				}
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000DFCDC File Offset: 0x000DECDC
		private bool DetermineHasMorePages()
		{
			this.currentpageset++;
			return this.currentpageset < this.pagesets.Count;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000DFD00 File Offset: 0x000DED00
		public DialogResult DisplayPrintDialog()
		{
			try
			{
				using (PrintDialog printDialog = new PrintDialog())
				{
					printDialog.UseEXDialog = this.printDialogSettings.UseEXDialog;
					printDialog.AllowSelection = this.printDialogSettings.AllowSelection;
					printDialog.AllowSomePages = this.printDialogSettings.AllowSomePages;
					printDialog.AllowCurrentPage = this.printDialogSettings.AllowCurrentPage;
					printDialog.AllowPrintToFile = this.printDialogSettings.AllowPrintToFile;
					printDialog.ShowHelp = this.printDialogSettings.ShowHelp;
					printDialog.ShowNetwork = this.printDialogSettings.ShowNetwork;
					printDialog.Document = this.printDoc;
					if (!string.IsNullOrEmpty(this.printerName))
					{
						this.printDoc.PrinterSettings.PrinterName = this.printerName;
					}
					this.printDoc.DefaultPageSettings.Landscape = printDialog.PrinterSettings.DefaultPageSettings.Landscape;
					this.printDoc.DefaultPageSettings.PaperSize = new PaperSize(printDialog.PrinterSettings.DefaultPageSettings.PaperSize.PaperName, printDialog.PrinterSettings.DefaultPageSettings.PaperSize.Width, printDialog.PrinterSettings.DefaultPageSettings.PaperSize.Height);
					return printDialog.ShowDialog();
				}
			}
			catch (InvalidPrinterException ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				XMessageBox.Show(CommonStr.strNotInstallPrinter, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return DialogResult.Cancel;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000DFE98 File Offset: 0x000DEE98
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000DFEA8 File Offset: 0x000DEEA8
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.SolidBrush1 != null)
				{
					this.SolidBrush1.Dispose();
				}
				if (this.lines != null)
				{
					this.lines.Dispose();
				}
				if (this.headercellformat != null)
				{
					this.headercellformat.Dispose();
				}
				if (this.cellformat != null)
				{
					this.cellformat.Dispose();
				}
				if (this.footerfont != null)
				{
					this.footerfont.Dispose();
				}
				if (this.pagenofont != null)
				{
					this.pagenofont.Dispose();
				}
				if (this.printDoc != null)
				{
					this.printDoc.Dispose();
				}
				if (this.subtitlefont != null)
				{
					this.subtitlefont.Dispose();
				}
				if (this.titlefont != null)
				{
					this.titlefont.Dispose();
				}
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000DFF68 File Offset: 0x000DEF68
		private void DrawImageCell(Graphics g, DataGridViewImageCell imagecell, RectangleF rectf)
		{
			Image image = (Image)imagecell.Value;
			Rectangle rectangle = default(Rectangle);
			int num;
			int num2;
			if (DataGridViewImageCellLayout.Normal == imagecell.ImageLayout || imagecell.ImageLayout == DataGridViewImageCellLayout.NotSet)
			{
				num = image.Width - (int)rectf.Width;
				num2 = image.Height - (int)rectf.Height;
				if (0 > num)
				{
					rectf.Width = (float)(rectangle.Width = image.Width);
				}
				else
				{
					rectangle.Width = (int)rectf.Width;
				}
				if (0 > num2)
				{
					rectf.Height = (float)(rectangle.Height = image.Height);
				}
				else
				{
					rectangle.Height = (int)rectf.Height;
				}
			}
			else if (DataGridViewImageCellLayout.Stretch == imagecell.ImageLayout)
			{
				rectangle.Width = image.Width;
				rectangle.Height = image.Height;
				num = 0;
				num2 = 0;
			}
			else
			{
				rectangle.Width = image.Width;
				rectangle.Height = image.Height;
				float num3 = rectf.Height / (float)rectangle.Height;
				float num4 = rectf.Width / (float)rectangle.Width;
				float num5;
				if (num3 > num4)
				{
					num5 = num4;
					num = 0;
					num2 = (int)((float)rectangle.Height * num5 - rectf.Height);
				}
				else
				{
					num5 = num3;
					num2 = 0;
					num = (int)((float)rectangle.Width * num5 - rectf.Width);
				}
				rectf.Width = (float)rectangle.Width * num5;
				rectf.Height = (float)rectangle.Height * num5;
			}
			DataGridViewContentAlignment alignment = imagecell.InheritedStyle.Alignment;
			if (alignment <= DataGridViewContentAlignment.MiddleCenter)
			{
				switch (alignment)
				{
				case DataGridViewContentAlignment.NotSet:
					if (0 <= num2)
					{
						rectangle.Y = num2 / 2;
					}
					else
					{
						rectf.Y -= (float)(num2 / 2);
					}
					if (0 > num)
					{
						rectf.X -= (float)(num / 2);
					}
					else
					{
						rectangle.X = num / 2;
					}
					break;
				case DataGridViewContentAlignment.TopLeft:
					rectangle.Y = 0;
					rectangle.X = 0;
					break;
				case DataGridViewContentAlignment.TopCenter:
					rectangle.Y = 0;
					if (0 <= num)
					{
						rectangle.X = num / 2;
					}
					else
					{
						rectf.X -= (float)(num / 2);
					}
					break;
				case (DataGridViewContentAlignment)3:
					break;
				case DataGridViewContentAlignment.TopRight:
					rectangle.Y = 0;
					if (0 <= num)
					{
						rectangle.X = num;
					}
					else
					{
						rectf.X -= (float)num;
					}
					break;
				default:
					if (alignment != DataGridViewContentAlignment.MiddleLeft)
					{
						if (alignment == DataGridViewContentAlignment.MiddleCenter)
						{
							if (0 > num2)
							{
								rectf.Y -= (float)(num2 / 2);
							}
							else
							{
								rectangle.Y = num2 / 2;
							}
							if (0 > num)
							{
								rectf.X -= (float)(num / 2);
							}
							else
							{
								rectangle.X = num / 2;
							}
						}
					}
					else
					{
						if (0 > num2)
						{
							rectf.Y -= (float)(num2 / 2);
						}
						else
						{
							rectangle.Y = num2 / 2;
						}
						rectangle.X = 0;
					}
					break;
				}
			}
			else if (alignment <= DataGridViewContentAlignment.BottomLeft)
			{
				if (alignment != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment == DataGridViewContentAlignment.BottomLeft)
					{
						if (0 > num2)
						{
							rectf.Y -= (float)num2;
						}
						else
						{
							rectangle.Y = num2;
						}
						rectangle.X = 0;
					}
				}
				else
				{
					if (0 > num2)
					{
						rectf.Y -= (float)(num2 / 2);
					}
					else
					{
						rectangle.Y = num2 / 2;
					}
					if (0 > num)
					{
						rectf.X -= (float)num;
					}
					else
					{
						rectangle.X = num;
					}
				}
			}
			else if (alignment != DataGridViewContentAlignment.BottomCenter)
			{
				if (alignment == DataGridViewContentAlignment.BottomRight)
				{
					if (0 > num2)
					{
						rectf.Y -= (float)num2;
					}
					else
					{
						rectangle.Y = num2;
					}
					if (0 > num)
					{
						rectf.X -= (float)num;
					}
					else
					{
						rectangle.X = num;
					}
				}
			}
			else
			{
				if (0 > num2)
				{
					rectf.Y -= (float)num2;
				}
				else
				{
					rectangle.Y = num2;
				}
				if (0 > num)
				{
					rectf.X -= (float)(num / 2);
				}
				else
				{
					rectangle.X = num / 2;
				}
			}
			g.DrawImage(image, rectf, rectangle, GraphicsUnit.Pixel);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x000E03B4 File Offset: 0x000DF3B4
		public bool EmbeddedPrint(DataGridView dgv, Graphics g, Rectangle area)
		{
			if (dgv == null || g == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			Margins printMargins = this.PrintMargins;
			this.PrintMargins.Top = area.Top;
			this.PrintMargins.Bottom = 0;
			this.PrintMargins.Left = area.Left;
			this.PrintMargins.Right = 0;
			this.pageHeight = area.Height + area.Top;
			this.printWidth = area.Width;
			this.pageWidth = area.Width + area.Left;
			this.fromPage = 0;
			this.toPage = int.MaxValue;
			this.PrintHeader = false;
			this.PrintFooter = false;
			if (this.cellformat == null)
			{
				this.buildstringformat(ref this.cellformat, dgv.DefaultCellStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
			}
			this.rowstoprint = new List<object>(dgv.Rows.Count);
			foreach (object obj in ((IEnumerable)dgv.Rows))
			{
				DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
				if (dataGridViewRow.Visible)
				{
					this.rowstoprint.Add(dataGridViewRow);
				}
			}
			this.colstoprint = new List<object>(dgv.Columns.Count);
			foreach (object obj2 in dgv.Columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj2;
				if (dataGridViewColumn.Visible)
				{
					this.colstoprint.Add(dataGridViewColumn);
				}
			}
			SortedList sortedList = new SortedList(this.colstoprint.Count);
			foreach (object obj3 in this.colstoprint)
			{
				DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)obj3;
				sortedList.Add(dataGridViewColumn2.DisplayIndex, dataGridViewColumn2);
			}
			this.colstoprint.Clear();
			foreach (object obj4 in sortedList.Values)
			{
				this.colstoprint.Add(obj4);
			}
			foreach (object obj5 in this.colstoprint)
			{
				DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn)obj5;
				if (this.publicwidthoverrides.ContainsKey(dataGridViewColumn3.Name))
				{
					this.colwidthsoverride.Add(this.publicwidthoverrides[dataGridViewColumn3.Name]);
				}
				else
				{
					this.colwidthsoverride.Add(-1f);
				}
			}
			this.measureprintarea(g);
			this.totalpages = this.TotalPages();
			this.currentpageset = 0;
			this.lastrowprinted = -1;
			this.CurrentPage = 0;
			return this.PrintPage(g);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x000E0708 File Offset: 0x000DF708
		public StringFormat GetCellFormat(DataGridView grid)
		{
			if (grid != null && this.cellformat == null)
			{
				this.buildstringformat(ref this.cellformat, grid.Rows[0].Cells[0].InheritedStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
			}
			if (this.cellformat == null)
			{
				this.cellformat = new StringFormat(this.cellformatflags);
			}
			return this.cellformat;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x000E0778 File Offset: 0x000DF778
		public StringFormat GetHeaderCellFormat(DataGridView grid)
		{
			if (grid != null && this.headercellformat == null)
			{
				this.buildstringformat(ref this.headercellformat, grid.Columns[0].HeaderCell.InheritedStyle, this.headercellalignment, StringAlignment.Near, this.headercellformatflags, StringTrimming.Word);
			}
			if (this.headercellformat == null)
			{
				this.headercellformat = new StringFormat(this.headercellformatflags);
			}
			return this.headercellformat;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000E07E0 File Offset: 0x000DF7E0
		private void measureprintarea(Graphics g)
		{
			this.rowheights = new List<float>(this.rowstoprint.Count);
			this.colwidths = new List<float>(this.colstoprint.Count);
			this.headerHeight = 0f;
			this.footerHeight = 0f;
			Font font = this.dgv.ColumnHeadersDefaultCellStyle.Font;
			if (font == null)
			{
				font = this.dgv.DefaultCellStyle.Font;
			}
			for (int i = 0; i < this.colstoprint.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.colstoprint[i];
				float num;
				if (0f < this.colwidthsoverride[i])
				{
					num = this.colwidthsoverride[i];
				}
				else
				{
					num = (float)this.printWidth;
				}
				SizeF sizeF = g.MeasureString(dataGridViewColumn.HeaderText, font, new SizeF(num, 2.147484E+09f), this.headercellformat);
				this.colwidths.Add(sizeF.Width);
				this.colheaderheight = ((this.colheaderheight < sizeF.Height) ? sizeF.Height : this.colheaderheight);
			}
			this.colheaderheight = ((this.colheaderheight < (float)this.dgv.ColumnHeadersHeight) ? ((float)this.dgv.ColumnHeadersHeight) : this.colheaderheight);
			if (this.pageno)
			{
				this.pagenumberHeight = g.MeasureString("Page", this.pagenofont, this.printWidth, this.pagenumberformat).Height;
			}
			if (this.PrintHeader)
			{
				if (this.pagenumberontop && !this.pagenumberonseparateline)
				{
					this.headerHeight += this.pagenumberHeight;
				}
				if (!string.IsNullOrEmpty(this.title))
				{
					this.headerHeight += g.MeasureString(this.title, this.titlefont, this.printWidth, this.titleformat).Height;
				}
				if (!string.IsNullOrEmpty(this.subtitle))
				{
					this.headerHeight += g.MeasureString(this.subtitle, this.subtitlefont, this.printWidth, this.subtitleformat).Height;
				}
				this.headerHeight += this.colheaderheight;
			}
			if (this.PrintFooter)
			{
				if (!string.IsNullOrEmpty(this.footer))
				{
					this.footerHeight += g.MeasureString(this.footer, this.footerfont, this.printWidth, this.footerformat).Height;
				}
				if (!this.pagenumberontop && (this.pagenumberonseparateline || this.footerHeight == 0f))
				{
					this.footerHeight += this.pagenumberHeight;
				}
				this.footerHeight += this.footerspacing;
			}
			for (int i = 0; i < this.rowstoprint.Count; i++)
			{
				DataGridViewRow dataGridViewRow = (DataGridViewRow)this.rowstoprint[i];
				this.rowheights.Add(0f);
				if (this.dgv.RowHeadersVisible)
				{
					SizeF sizeF2 = g.MeasureString(dataGridViewRow.HeaderCell.EditedFormattedValue.ToString(), font);
					this.rowheaderwidth = ((this.rowheaderwidth < sizeF2.Width) ? sizeF2.Width : this.rowheaderwidth);
				}
				float num2 = 0f;
				for (int j = 0; j < this.colstoprint.Count; j++)
				{
					DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this.colstoprint[j];
					string text = dataGridViewRow.Cells[dataGridViewColumn.Index].EditedFormattedValue.ToString();
					StringFormat stringFormat = null;
					DataGridViewCellStyle dataGridViewCellStyle;
					if (this.ColumnStyles.ContainsKey(dataGridViewColumn.Name))
					{
						dataGridViewCellStyle = this.colstyles[dataGridViewColumn.Name];
						this.buildstringformat(ref stringFormat, dataGridViewCellStyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
					}
					else if (dataGridViewColumn.HasDefaultCellStyle || dataGridViewRow.Cells[dataGridViewColumn.Index].HasStyle)
					{
						dataGridViewCellStyle = dataGridViewRow.Cells[dataGridViewColumn.Index].InheritedStyle;
						this.buildstringformat(ref stringFormat, dataGridViewCellStyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
					}
					else
					{
						stringFormat = this.cellformat;
						dataGridViewCellStyle = this.dgv.DefaultCellStyle;
					}
					SizeF sizeF3;
					if (DGVPrinter.RowHeightSetting.CellHeight == this.RowHeight)
					{
						sizeF3 = dataGridViewRow.Cells[dataGridViewColumn.Index].Size;
					}
					else
					{
						sizeF3 = g.MeasureString(text, dataGridViewCellStyle.Font);
					}
					if (0f < this.colwidthsoverride[j] || sizeF3.Width > (float)this.printWidth || (j == this.colstoprint.Count - 1 && dataGridViewColumn.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill))
					{
						if (0f < this.colwidthsoverride[j])
						{
							this.colwidths[j] = this.colwidthsoverride[j];
						}
						else if (sizeF3.Width > (float)this.printWidth)
						{
							this.colwidths[j] = (float)this.printWidth;
						}
						if (j == this.colstoprint.Count - 1 && num2 < (float)this.printWidth)
						{
							this.colwidths[j] = (float)this.printWidth - num2;
						}
						int num3;
						int num4;
						float height = g.MeasureString(text, dataGridViewCellStyle.Font, new SizeF(this.colwidths[j], 2.147484E+09f), stringFormat, out num3, out num4).Height;
						this.rowheights[i] = ((this.rowheights[i] < height) ? height : this.rowheights[i]);
					}
					else
					{
						this.colwidths[j] = ((this.colwidths[j] < sizeF3.Width) ? sizeF3.Width : this.colwidths[j]);
						this.rowheights[i] = ((this.rowheights[i] < sizeF3.Height) ? sizeF3.Height : this.rowheights[i]);
					}
					this.rowheights[i] = ((this.rowheights[i] < (float)this.dgv.RowTemplate.Height) ? ((float)this.dgv.RowTemplate.Height) : this.rowheights[i]);
					num2 += this.colwidths[j];
				}
			}
			this.pagesets = new List<DGVPrinter.PageDef>();
			this.pagesets.Add(new DGVPrinter.PageDef(this.PrintMargins, this.colstoprint.Count));
			int num5 = 0;
			this.pagesets[num5].coltotalwidth = this.rowheaderwidth;
			for (int i = 0; i < this.colstoprint.Count; i++)
			{
				float num6 = ((this.colwidthsoverride[i] >= 0f) ? this.colwidthsoverride[i] : this.colwidths[i]);
				if ((float)this.printWidth < this.pagesets[num5].coltotalwidth + num6 && i != 0)
				{
					this.pagesets.Add(new DGVPrinter.PageDef(this.PrintMargins, this.colstoprint.Count));
					num5++;
					this.pagesets[num5].coltotalwidth = this.rowheaderwidth;
				}
				this.pagesets[num5].colstoprint.Add(this.colstoprint[i]);
				this.pagesets[num5].colwidths.Add(this.colwidths[i]);
				this.pagesets[num5].colwidthsoverride.Add(this.colwidthsoverride[i]);
				DGVPrinter.PageDef pageDef = this.pagesets[num5];
				pageDef.coltotalwidth += num6;
			}
			for (int i = 0; i < this.pagesets.Count; i++)
			{
				this.AdjustPageSets(g, this.pagesets[i]);
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000E1068 File Offset: 0x000E0068
		private int PreviewDisplayHeight()
		{
			double num = (double)((float)this.printDoc.DefaultPageSettings.Bounds.Height + 3f * this.printDoc.DefaultPageSettings.HardMarginX);
			return (int)(num * this.PrintPreviewZoom);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000E10B0 File Offset: 0x000E00B0
		private int PreviewDisplayWidth()
		{
			double num = (double)((float)this.printDoc.DefaultPageSettings.Bounds.Width + 3f * this.printDoc.DefaultPageSettings.HardMarginY);
			return (int)(num * this.PrintPreviewZoom);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000E10F8 File Offset: 0x000E00F8
		private void printcolumnheaders(Graphics g, ref float pos, DGVPrinter.PageDef pageset)
		{
			float num = (float)pageset.margins.Left + this.rowheaderwidth;
			this.lines = new Pen(this.dgv.GridColor, 1f);
			for (int i = 0; i < pageset.colstoprint.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)pageset.colstoprint[i];
				float num2 = ((pageset.colwidths[i] > (float)this.printWidth - this.rowheaderwidth) ? ((float)this.printWidth - this.rowheaderwidth) : pageset.colwidths[i]);
				DataGridViewCellStyle inheritedStyle = dataGridViewColumn.HeaderCell.InheritedStyle;
				RectangleF rectangleF = new RectangleF(num, pos, num2, this.colheaderheight);
				g.FillRectangle(this.SolidBrush1 = new SolidBrush(inheritedStyle.BackColor), rectangleF);
				g.DrawString(dataGridViewColumn.HeaderText, inheritedStyle.Font, this.SolidBrush1 = new SolidBrush(inheritedStyle.ForeColor), rectangleF, this.headercellformat);
				if (this.dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None)
				{
					g.DrawRectangle(this.lines, num, pos, num2, this.colheaderheight);
				}
				num += pageset.colwidths[i];
			}
			pos += this.colheaderheight + ((this.dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None) ? this.lines.Width : 0f);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000E126C File Offset: 0x000E026C
		public void PrintDataGridView(DataGridView dgv)
		{
			if (dgv == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			if (typeof(DataGridView) != dgv.GetType())
			{
				throw new Exception("Invalid Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			if (DialogResult.OK == this.DisplayPrintDialog())
			{
				this.SetupPrint();
				try
				{
					this.printDoc.Print();
				}
				catch (InvalidPrinterException ex)
				{
					wgAppConfig.wgLog(ex.ToString());
					XMessageBox.Show(CommonStr.strSelectDefaultPrinter, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000E12F8 File Offset: 0x000E02F8
		public int PrintDataGridViewWithoutPrintDialog(DataGridView dgv)
		{
			if (dgv == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			if (typeof(DataGridView) != dgv.GetType())
			{
				throw new Exception("Invalid Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			this.SetupPrint();
			try
			{
				this.printDoc.Print();
				return 1;
			}
			catch (InvalidPrinterException ex)
			{
				wgAppConfig.wgLog(ex.ToString());
				XMessageBox.Show(CommonStr.strSelectDefaultPrinter, wgTools.MSGTITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return 0;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000E1380 File Offset: 0x000E0380
		private void printDoc_BeginPrint(object sender, PrintEventArgs e)
		{
			this.currentpageset = 0;
			this.lastrowprinted = -1;
			this.CurrentPage = 0;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000E1397 File Offset: 0x000E0397
		private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			e.HasMorePages = this.PrintPage(e.Graphics);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000E13AC File Offset: 0x000E03AC
		private void printfooter(Graphics g, ref float pos, Margins margins)
		{
			pos = (float)this.pageHeight - this.footerHeight - (float)margins.Bottom;
			pos += this.footerspacing;
			this.printsection(g, ref pos, this.footer, this.footerfont, this.footercolor, this.footerformat, this.overridefooterformat, margins);
			if (!this.pagenumberontop && this.pageno)
			{
				string text = "";
				try
				{
					text = string.Format(this.pagetext, this.CurrentPage.ToString(CultureInfo.CurrentCulture));
				}
				catch
				{
				}
				if (this.showtotalpagenumber)
				{
					text = text + this.pageseparator + this.totalpages.ToString();
				}
				if (1 < this.pagesets.Count)
				{
					text = text + this.parttext + (this.currentpageset + 1).ToString(CultureInfo.CurrentCulture);
				}
				if (!this.pagenumberonseparateline)
				{
					pos -= this.pagenumberHeight;
				}
				this.printsection(g, ref pos, text, this.pagenofont, this.pagenocolor, this.pagenumberformat, this.overridepagenumberformat, margins);
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000E14D8 File Offset: 0x000E04D8
		private void printfooter4LastPage(Graphics g, ref float pos, Margins margins)
		{
			if (pos + this.footerHeight + this.footerspacing > (float)this.pageHeight - this.footerHeight - (float)margins.Bottom)
			{
				pos = (float)this.pageHeight - this.footerHeight - (float)margins.Bottom;
			}
			else
			{
				pos = pos + this.footerHeight + this.footerspacing;
			}
			pos += this.footerspacing;
			this.printsection(g, ref pos, this.footer, this.footerfont, this.footercolor, this.footerformat, this.overridefooterformat, margins);
			if (!this.pagenumberontop)
			{
				pos = (float)this.pageHeight - this.footerHeight - (float)margins.Bottom;
				if (this.pageno)
				{
					string text = "";
					try
					{
						text = string.Format(this.pagetext, this.CurrentPage.ToString(CultureInfo.CurrentCulture));
					}
					catch
					{
					}
					if (this.showtotalpagenumber)
					{
						text = text + this.pageseparator + this.totalpages.ToString();
					}
					if (1 < this.pagesets.Count)
					{
						text = text + this.parttext + (this.currentpageset + 1).ToString(CultureInfo.CurrentCulture);
					}
					if (!this.pagenumberonseparateline)
					{
						pos -= this.pagenumberHeight;
					}
					this.printsection(g, ref pos, text, this.pagenofont, this.pagenocolor, this.pagenumberformat, this.overridepagenumberformat, margins);
				}
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000E1658 File Offset: 0x000E0658
		public void PrintNoDisplay(DataGridView dgv)
		{
			if (dgv == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			if (typeof(DataGridView) != dgv.GetType())
			{
				throw new Exception("Invalid Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			this.SetupPrint();
			this.printDoc.Print();
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000E16A8 File Offset: 0x000E06A8
		private bool PrintPage(Graphics g)
		{
			bool flag = false;
			float num = (float)this.pagesets[this.currentpageset].margins.Top;
			this.CurrentPage++;
			if (this.CurrentPage >= this.fromPage && this.CurrentPage <= this.toPage)
			{
				flag = true;
			}
			float num2 = (float)this.pageHeight - this.footerHeight * 2f - (float)this.pagesets[this.currentpageset].margins.Bottom;
			float num3;
			while (!flag)
			{
				num = (float)this.pagesets[this.currentpageset].margins.Top + this.headerHeight;
				bool flag2 = false;
				num3 = ((this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f);
				while (!flag2)
				{
					if (this.lastrowprinted >= this.rowstoprint.Count - 1)
					{
						flag2 = true;
					}
					else if (num + num3 >= num2)
					{
						flag2 = true;
					}
					else
					{
						this.lastrowprinted++;
						num += this.rowheights[this.lastrowprinted];
						num3 = ((this.lastrowprinted + 1 < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f);
					}
				}
				this.CurrentPage++;
				if (this.CurrentPage >= this.fromPage && this.CurrentPage <= this.toPage)
				{
					flag = true;
				}
				if (this.lastrowprinted >= this.rowstoprint.Count - 1 || this.CurrentPage > this.toPage)
				{
					bool flag3 = this.DetermineHasMorePages();
					this.lastrowprinted = -1;
					this.CurrentPage = 0;
					return flag3;
				}
			}
			num = (float)this.pagesets[this.currentpageset].margins.Top;
			if (this.PrintHeader)
			{
				if (this.pagenumberontop && this.pageno)
				{
					string text = "";
					try
					{
						text = string.Format(this.pagetext, this.CurrentPage.ToString(CultureInfo.CurrentCulture));
					}
					catch
					{
					}
					if (this.showtotalpagenumber)
					{
						text = text + this.pageseparator + this.totalpages.ToString();
					}
					if (1 < this.pagesets.Count)
					{
						text = text + this.parttext + (this.currentpageset + 1).ToString(CultureInfo.CurrentCulture);
					}
					this.printsection(g, ref num, text, this.pagenofont, this.pagenocolor, this.pagenumberformat, this.overridepagenumberformat, this.pagesets[this.currentpageset].margins);
					if (!this.pagenumberonseparateline)
					{
						num -= this.pagenumberHeight;
					}
				}
				if (!string.IsNullOrEmpty(this.title))
				{
					this.printsection(g, ref num, this.title, this.titlefont, this.titlecolor, this.titleformat, this.overridetitleformat, this.pagesets[this.currentpageset].margins);
				}
				if (!string.IsNullOrEmpty(this.subtitle))
				{
					this.printsection(g, ref num, this.subtitle, this.subtitlefont, this.subtitlecolor, this.subtitleformat, this.overridesubtitleformat, this.pagesets[this.currentpageset].margins);
				}
			}
			if (this.PrintColumnHeaders)
			{
				this.printcolumnheaders(g, ref num, this.pagesets[this.currentpageset]);
			}
			num3 = ((this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f);
			while (num + num3 < num2)
			{
				this.lastrowprinted++;
				this.printrow(g, ref num, (DataGridViewRow)this.rowstoprint[this.lastrowprinted], this.pagesets[this.currentpageset]);
				if (this.lastrowprinted >= this.rowstoprint.Count - 1)
				{
					if (this.currentpageset + 1 >= this.pagesets.Count)
					{
						this.printfooter4LastPage(g, ref num, this.pagesets[this.currentpageset].margins);
					}
					else
					{
						this.printfooter(g, ref num, this.pagesets[this.currentpageset].margins);
					}
					bool flag3 = this.DetermineHasMorePages();
					this.lastrowprinted = -1;
					this.CurrentPage = 0;
					return flag3;
				}
				num3 = ((this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f);
			}
			if (this.PrintFooter)
			{
				if (this.CurrentPage >= this.toPage && this.currentpageset + 1 >= this.pagesets.Count)
				{
					this.printfooter4LastPage(g, ref num, this.pagesets[this.currentpageset].margins);
				}
				else
				{
					this.printfooter(g, ref num, this.pagesets[this.currentpageset].margins);
				}
			}
			if (this.CurrentPage >= this.toPage)
			{
				bool flag3 = this.DetermineHasMorePages();
				this.lastrowprinted = -1;
				this.CurrentPage = 0;
				return flag3;
			}
			return true;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000E1C00 File Offset: 0x000E0C00
		public void PrintPreviewDataGridView(DataGridView dgv)
		{
			if (dgv == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			if (typeof(DataGridView) != dgv.GetType())
			{
				throw new Exception("Invalid Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			if (DialogResult.OK == this.DisplayPrintDialog())
			{
				this.PrintPreviewNoDisplay(dgv);
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000E1C50 File Offset: 0x000E0C50
		public void PrintPreviewNoDisplay(DataGridView dgv)
		{
			if (dgv == null)
			{
				throw new Exception("Null Parameter passed to DGVPrinter.");
			}
			if (typeof(DataGridView) != dgv.GetType())
			{
				throw new Exception("Invalid Parameter passed to DGVPrinter.");
			}
			this.dgv = dgv;
			this.SetupPrint();
			using (PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog())
			{
				printPreviewDialog.Document = this.printDoc;
				printPreviewDialog.UseAntiAlias = true;
				printPreviewDialog.Owner = this.Owner;
				printPreviewDialog.PrintPreviewControl.Zoom = this.PrintPreviewZoom;
				printPreviewDialog.Width = this.PreviewDisplayWidth();
				printPreviewDialog.Height = this.PreviewDisplayHeight();
				if (this.ppvIcon != null)
				{
					printPreviewDialog.Icon = this.ppvIcon;
				}
				printPreviewDialog.ShowDialog();
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000E1D1C File Offset: 0x000E0D1C
		private void printrow(Graphics g, ref float pos, DataGridViewRow row, DGVPrinter.PageDef pageset)
		{
			float num = (float)pageset.margins.Left;
			this.lines = new Pen(this.dgv.GridColor, 1f);
			DataGridViewCellStyle inheritedStyle = row.InheritedStyle;
			float num2 = ((pageset.coltotalwidth > (float)this.printWidth) ? ((float)this.printWidth) : pageset.coltotalwidth);
			RectangleF rectangleF = new RectangleF(num, pos, num2, this.rowheights[this.lastrowprinted]);
			this.SolidBrush1 = new SolidBrush(inheritedStyle.BackColor);
			g.FillRectangle(this.SolidBrush1, rectangleF);
			if (this.dgv.RowHeadersVisible)
			{
				DataGridViewCellStyle inheritedStyle2 = row.HeaderCell.InheritedStyle;
				RectangleF rectangleF2 = new RectangleF(num, pos, this.rowheaderwidth, this.rowheights[this.lastrowprinted]);
				this.SolidBrush1 = new SolidBrush(inheritedStyle2.BackColor);
				g.FillRectangle(this.SolidBrush1, rectangleF2);
				g.DrawString(row.HeaderCell.EditedFormattedValue.ToString(), inheritedStyle2.Font, this.SolidBrush1 = new SolidBrush(inheritedStyle2.ForeColor), rectangleF2, this.headercellformat);
				if (this.dgv.RowHeadersBorderStyle != DataGridViewHeaderBorderStyle.None)
				{
					g.DrawRectangle(this.lines, num, pos, this.rowheaderwidth, this.rowheights[this.lastrowprinted]);
				}
				num += this.rowheaderwidth;
			}
			for (int i = 0; i < pageset.colstoprint.Count; i++)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)pageset.colstoprint[i];
				string text = row.Cells[dataGridViewColumn.Index].EditedFormattedValue.ToString();
				float num3 = ((pageset.colwidths[i] > (float)this.printWidth - this.rowheaderwidth) ? ((float)this.printWidth - this.rowheaderwidth) : pageset.colwidths[i]);
				StringFormat stringFormat = null;
				DataGridViewCellStyle dataGridViewCellStyle;
				if (this.ColumnStyles.ContainsKey(dataGridViewColumn.Name))
				{
					dataGridViewCellStyle = this.colstyles[dataGridViewColumn.Name];
					this.buildstringformat(ref stringFormat, dataGridViewCellStyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
					Font font = dataGridViewCellStyle.Font;
				}
				else if (dataGridViewColumn.HasDefaultCellStyle || row.Cells[dataGridViewColumn.Index].HasStyle)
				{
					dataGridViewCellStyle = row.Cells[dataGridViewColumn.Index].InheritedStyle;
					this.buildstringformat(ref stringFormat, dataGridViewCellStyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
					Font font2 = dataGridViewCellStyle.Font;
				}
				else
				{
					stringFormat = this.cellformat;
					dataGridViewCellStyle = row.Cells[dataGridViewColumn.Index].InheritedStyle;
				}
				RectangleF rectangleF3 = new RectangleF(num, pos, num3, this.rowheights[this.lastrowprinted]);
				g.FillRectangle(this.SolidBrush1 = new SolidBrush(dataGridViewCellStyle.BackColor), rectangleF3);
				if ("DataGridViewImageCell" == dataGridViewColumn.CellType.Name)
				{
					this.DrawImageCell(g, (DataGridViewImageCell)row.Cells[dataGridViewColumn.Index], rectangleF3);
				}
				else
				{
					if ("DataGridViewCheckBoxCell" == dataGridViewColumn.CellType.Name)
					{
						if (text == "True")
						{
							text = "√";
						}
						else
						{
							text = " ";
						}
					}
					g.DrawString(text, dataGridViewCellStyle.Font, this.SolidBrush1 = new SolidBrush(dataGridViewCellStyle.ForeColor), rectangleF3, stringFormat);
				}
				if (this.dgv.CellBorderStyle != DataGridViewCellBorderStyle.None)
				{
					g.DrawRectangle(this.lines, num, pos, num3, this.rowheights[this.lastrowprinted]);
				}
				num += pageset.colwidths[i];
			}
			pos += this.rowheights[this.lastrowprinted];
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000E2158 File Offset: 0x000E1158
		private void printsection(Graphics g, ref float pos, string text, Font font, Color color, StringFormat format, bool useroverride, Margins margins)
		{
			SizeF sizeF = g.MeasureString(text, font, this.printWidth, format);
			RectangleF rectangleF = new RectangleF((float)margins.Left, pos, (float)this.printWidth, sizeF.Height);
			this.SolidBrush1 = new SolidBrush(color);
			g.DrawString(text, font, this.SolidBrush1, rectangleF, format);
			pos += sizeF.Height;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000E21C4 File Offset: 0x000E11C4
		private void SetupPrint()
		{
			if (this.headercellformat == null)
			{
				this.buildstringformat(ref this.headercellformat, this.dgv.Columns[0].HeaderCell.InheritedStyle, this.headercellalignment, StringAlignment.Near, this.headercellformatflags, StringTrimming.Word);
			}
			if (this.cellformat == null)
			{
				this.buildstringformat(ref this.cellformat, this.dgv.DefaultCellStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
			}
			int num = (int)Math.Round((double)this.printDoc.DefaultPageSettings.HardMarginX);
			int num2 = (int)Math.Round((double)this.printDoc.DefaultPageSettings.HardMarginY);
			int num3;
			if (this.printDoc.DefaultPageSettings.Landscape)
			{
				num3 = (int)Math.Round((double)this.printDoc.DefaultPageSettings.PrintableArea.Height);
			}
			else
			{
				num3 = (int)Math.Round((double)this.printDoc.DefaultPageSettings.PrintableArea.Width);
			}
			this.pageHeight = this.printDoc.DefaultPageSettings.Bounds.Height;
			this.pageWidth = this.printDoc.DefaultPageSettings.Bounds.Width;
			this.PrintMargins = this.printDoc.DefaultPageSettings.Margins;
			this.PrintMargins.Right = ((num > this.PrintMargins.Right) ? num : this.PrintMargins.Right);
			this.PrintMargins.Left = ((num > this.PrintMargins.Left) ? num : this.PrintMargins.Left);
			this.PrintMargins.Top = ((num2 > this.PrintMargins.Top) ? num2 : this.PrintMargins.Top);
			this.PrintMargins.Bottom = ((num2 > this.PrintMargins.Bottom) ? num2 : this.PrintMargins.Bottom);
			this.printWidth = this.pageWidth - this.PrintMargins.Left - this.PrintMargins.Right;
			this.printWidth = ((this.printWidth > num3) ? num3 : this.printWidth);
			this.printRange = this.printDoc.PrinterSettings.PrintRange;
			if (PrintRange.SomePages == this.printRange)
			{
				this.fromPage = this.printDoc.PrinterSettings.FromPage;
				this.toPage = this.printDoc.PrinterSettings.ToPage;
			}
			else
			{
				this.fromPage = 0;
				this.toPage = int.MaxValue;
			}
			if (PrintRange.Selection == this.printRange)
			{
				SortedList sortedList;
				if (this.dgv.SelectedRows.Count != 0)
				{
					sortedList = new SortedList(this.dgv.SelectedRows.Count);
					foreach (object obj in this.dgv.SelectedRows)
					{
						DataGridViewRow dataGridViewRow = (DataGridViewRow)obj;
						sortedList.Add(dataGridViewRow.Index, dataGridViewRow);
					}
					sortedList.Values.GetEnumerator();
					this.rowstoprint = new List<object>(sortedList.Count);
					foreach (object obj2 in sortedList.Values)
					{
						this.rowstoprint.Add(obj2);
					}
					this.colstoprint = new List<object>(this.dgv.Columns.Count);
					using (IEnumerator enumerator3 = this.dgv.Columns.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj3;
							if (dataGridViewColumn.Visible)
							{
								this.colstoprint.Add(dataGridViewColumn);
							}
						}
						goto IL_0864;
					}
				}
				SortedList sortedList2;
				if (this.dgv.SelectedColumns.Count != 0)
				{
					this.rowstoprint = this.dgv.Rows;
					sortedList2 = new SortedList(this.dgv.SelectedColumns.Count);
					foreach (object obj4 in this.dgv.SelectedColumns)
					{
						DataGridViewRow dataGridViewRow2 = (DataGridViewRow)obj4;
						sortedList2.Add(dataGridViewRow2.Index, dataGridViewRow2);
					}
					this.colstoprint = new List<object>(sortedList2.Count);
					using (IEnumerator enumerator5 = sortedList2.Values.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							object obj5 = enumerator5.Current;
							this.colstoprint.Add(obj5);
						}
						goto IL_0864;
					}
				}
				sortedList = new SortedList(this.dgv.SelectedCells.Count);
				sortedList2 = new SortedList(this.dgv.SelectedCells.Count);
				foreach (object obj6 in this.dgv.SelectedCells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj6;
					int columnIndex = dataGridViewCell.ColumnIndex;
					int rowIndex = dataGridViewCell.RowIndex;
					if (!sortedList.Contains(rowIndex))
					{
						sortedList.Add(rowIndex, this.dgv.Rows[rowIndex]);
					}
					if (!sortedList2.Contains(columnIndex))
					{
						sortedList2.Add(columnIndex, this.dgv.Columns[columnIndex]);
					}
				}
				this.rowstoprint = new List<object>(sortedList.Count);
				foreach (object obj7 in sortedList.Values)
				{
					this.rowstoprint.Add(obj7);
				}
				this.colstoprint = new List<object>(sortedList2.Count);
				using (IEnumerator enumerator8 = sortedList2.Values.GetEnumerator())
				{
					while (enumerator8.MoveNext())
					{
						object obj8 = enumerator8.Current;
						this.colstoprint.Add(obj8);
					}
					goto IL_0864;
				}
			}
			if (PrintRange.CurrentPage == this.printRange)
			{
				this.rowstoprint = new List<object>(this.dgv.DisplayedRowCount(true));
				this.colstoprint = new List<object>(this.dgv.Columns.Count);
				for (int i = this.dgv.FirstDisplayedScrollingRowIndex; i < this.dgv.FirstDisplayedScrollingRowIndex + this.dgv.DisplayedRowCount(true); i++)
				{
					DataGridViewRow dataGridViewRow3 = this.dgv.Rows[i];
					if (dataGridViewRow3.Visible)
					{
						this.rowstoprint.Add(dataGridViewRow3);
					}
				}
				this.colstoprint = new List<object>(this.dgv.Columns.Count);
				using (IEnumerator enumerator9 = this.dgv.Columns.GetEnumerator())
				{
					while (enumerator9.MoveNext())
					{
						object obj9 = enumerator9.Current;
						DataGridViewColumn dataGridViewColumn2 = (DataGridViewColumn)obj9;
						if (dataGridViewColumn2.Visible)
						{
							this.colstoprint.Add(dataGridViewColumn2);
						}
					}
					goto IL_0864;
				}
			}
			this.rowstoprint = new List<object>(this.dgv.Rows.Count);
			foreach (object obj10 in ((IEnumerable)this.dgv.Rows))
			{
				DataGridViewRow dataGridViewRow4 = (DataGridViewRow)obj10;
				if (dataGridViewRow4.Visible)
				{
					this.rowstoprint.Add(dataGridViewRow4);
				}
			}
			this.colstoprint = new List<object>(this.dgv.Columns.Count);
			foreach (object obj11 in this.dgv.Columns)
			{
				DataGridViewColumn dataGridViewColumn3 = (DataGridViewColumn)obj11;
				if (dataGridViewColumn3.Visible)
				{
					this.colstoprint.Add(dataGridViewColumn3);
				}
			}
			IL_0864:
			SortedList sortedList3 = new SortedList(this.colstoprint.Count);
			foreach (object obj12 in this.colstoprint)
			{
				DataGridViewColumn dataGridViewColumn4 = (DataGridViewColumn)obj12;
				sortedList3.Add(dataGridViewColumn4.DisplayIndex, dataGridViewColumn4);
			}
			this.colstoprint.Clear();
			foreach (object obj13 in sortedList3.Values)
			{
				this.colstoprint.Add(obj13);
			}
			foreach (object obj14 in this.colstoprint)
			{
				DataGridViewColumn dataGridViewColumn5 = (DataGridViewColumn)obj14;
				if (this.publicwidthoverrides.ContainsKey(dataGridViewColumn5.Name))
				{
					this.colwidthsoverride.Add(this.publicwidthoverrides[dataGridViewColumn5.Name]);
				}
				else
				{
					this.colwidthsoverride.Add(-1f);
				}
			}
			this.measureprintarea(this.printDoc.PrinterSettings.CreateMeasurementGraphics());
			this.totalpages = this.TotalPages();
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x000E2C3C File Offset: 0x000E1C3C
		private int TotalPages()
		{
			int num = 1;
			float num2 = 0f;
			float num3 = (float)this.pageHeight - this.headerHeight - this.footerHeight - (float)this.PrintMargins.Top - (float)this.PrintMargins.Bottom;
			for (int i = 0; i < this.rowheights.Count; i++)
			{
				if (num2 + this.rowheights[i] > num3)
				{
					num++;
					num2 = 0f;
				}
				num2 += this.rowheights[i];
			}
			return num;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x000E2CC2 File Offset: 0x000E1CC2
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x000E2CCA File Offset: 0x000E1CCA
		public StringAlignment CellAlignment
		{
			get
			{
				return this.cellalignment;
			}
			set
			{
				this.cellalignment = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x000E2CD3 File Offset: 0x000E1CD3
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x000E2CDB File Offset: 0x000E1CDB
		public StringFormatFlags CellFormatFlags
		{
			get
			{
				return this.cellformatflags;
			}
			set
			{
				this.cellformatflags = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x000E2CE4 File Offset: 0x000E1CE4
		public Dictionary<string, DataGridViewCellStyle> ColumnStyles
		{
			get
			{
				return this.colstyles;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x000E2CEC File Offset: 0x000E1CEC
		public Dictionary<string, float> ColumnWidths
		{
			get
			{
				return this.publicwidthoverrides;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x000E2CF4 File Offset: 0x000E1CF4
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x000E2CFC File Offset: 0x000E1CFC
		public string DocName
		{
			get
			{
				return this.docName;
			}
			set
			{
				this.printDoc.DocumentName = value;
				this.docName = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x000E2D11 File Offset: 0x000E1D11
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x000E2D19 File Offset: 0x000E1D19
		public string Footer
		{
			get
			{
				return this.footer;
			}
			set
			{
				this.footer = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x000E2D22 File Offset: 0x000E1D22
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x000E2D2F File Offset: 0x000E1D2F
		public StringAlignment FooterAlignment
		{
			get
			{
				return this.footerformat.Alignment;
			}
			set
			{
				this.footerformat.Alignment = value;
				this.overridefooterformat = true;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000E2D44 File Offset: 0x000E1D44
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x000E2D4C File Offset: 0x000E1D4C
		public Color FooterColor
		{
			get
			{
				return this.footercolor;
			}
			set
			{
				this.footercolor = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x000E2D55 File Offset: 0x000E1D55
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x000E2D5D File Offset: 0x000E1D5D
		public Font FooterFont
		{
			get
			{
				return this.footerfont;
			}
			set
			{
				this.footerfont = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x000E2D66 File Offset: 0x000E1D66
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x000E2D6E File Offset: 0x000E1D6E
		public StringFormat FooterFormat
		{
			get
			{
				return this.footerformat;
			}
			set
			{
				this.footerformat = value;
				this.overridefooterformat = true;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x000E2D7E File Offset: 0x000E1D7E
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x000E2D8B File Offset: 0x000E1D8B
		public StringFormatFlags FooterFormatFlags
		{
			get
			{
				return this.footerformat.FormatFlags;
			}
			set
			{
				this.footerformat.FormatFlags = value;
				this.overridefooterformat = true;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x000E2DA0 File Offset: 0x000E1DA0
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x000E2DA8 File Offset: 0x000E1DA8
		public float FooterSpacing
		{
			get
			{
				return this.footerspacing;
			}
			set
			{
				this.footerspacing = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x000E2DB1 File Offset: 0x000E1DB1
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x000E2DB9 File Offset: 0x000E1DB9
		public StringAlignment HeaderCellAlignment
		{
			get
			{
				return this.headercellalignment;
			}
			set
			{
				this.headercellalignment = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x000E2DC2 File Offset: 0x000E1DC2
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x000E2DCA File Offset: 0x000E1DCA
		public StringFormatFlags HeaderCellFormatFlags
		{
			get
			{
				return this.headercellformatflags;
			}
			set
			{
				this.headercellformatflags = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x000E2DD3 File Offset: 0x000E1DD3
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x000E2DDB File Offset: 0x000E1DDB
		public Form Owner
		{
			get
			{
				return this._Owner;
			}
			set
			{
				this._Owner = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x000E2DE4 File Offset: 0x000E1DE4
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x000E2DF1 File Offset: 0x000E1DF1
		public StringAlignment PageNumberAlignment
		{
			get
			{
				return this.pagenumberformat.Alignment;
			}
			set
			{
				this.pagenumberformat.Alignment = value;
				this.overridepagenumberformat = true;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x000E2E06 File Offset: 0x000E1E06
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x000E2E0E File Offset: 0x000E1E0E
		public Color PageNumberColor
		{
			get
			{
				return this.pagenocolor;
			}
			set
			{
				this.pagenocolor = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x000E2E17 File Offset: 0x000E1E17
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x000E2E1F File Offset: 0x000E1E1F
		public Font PageNumberFont
		{
			get
			{
				return this.pagenofont;
			}
			set
			{
				this.pagenofont = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x000E2E28 File Offset: 0x000E1E28
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x000E2E30 File Offset: 0x000E1E30
		public StringFormat PageNumberFormat
		{
			get
			{
				return this.pagenumberformat;
			}
			set
			{
				this.pagenumberformat = value;
				this.overridepagenumberformat = true;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x000E2E40 File Offset: 0x000E1E40
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x000E2E4D File Offset: 0x000E1E4D
		public StringFormatFlags PageNumberFormatFlags
		{
			get
			{
				return this.pagenumberformat.FormatFlags;
			}
			set
			{
				this.pagenumberformat.FormatFlags = value;
				this.overridepagenumberformat = true;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x000E2E62 File Offset: 0x000E1E62
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x000E2E6A File Offset: 0x000E1E6A
		public bool PageNumberInHeader
		{
			get
			{
				return this.pagenumberontop;
			}
			set
			{
				this.pagenumberontop = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x000E2E73 File Offset: 0x000E1E73
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x000E2E7B File Offset: 0x000E1E7B
		public bool PageNumberOnSeparateLine
		{
			get
			{
				return this.pagenumberonseparateline;
			}
			set
			{
				this.pagenumberonseparateline = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x000E2E84 File Offset: 0x000E1E84
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x000E2E8C File Offset: 0x000E1E8C
		public bool PageNumbers
		{
			get
			{
				return this.pageno;
			}
			set
			{
				this.pageno = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x000E2E95 File Offset: 0x000E1E95
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x000E2E9D File Offset: 0x000E1E9D
		public string PageSeparator
		{
			get
			{
				return this.pageseparator;
			}
			set
			{
				this.pageseparator = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x000E2EA6 File Offset: 0x000E1EA6
		public PageSettings PageSettings
		{
			get
			{
				return this.printDoc.DefaultPageSettings;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x000E2EB3 File Offset: 0x000E1EB3
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x000E2EBB File Offset: 0x000E1EBB
		public string PageText
		{
			get
			{
				return this.pagetext;
			}
			set
			{
				this.pagetext = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x000E2EC4 File Offset: 0x000E1EC4
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x000E2ECC File Offset: 0x000E1ECC
		public string PartText
		{
			get
			{
				return this.parttext;
			}
			set
			{
				this.parttext = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x000E2ED5 File Offset: 0x000E1ED5
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x000E2EDD File Offset: 0x000E1EDD
		public bool PorportionalColumns
		{
			get
			{
				return this.porportionalcolumns;
			}
			set
			{
				this.porportionalcolumns = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000E2EE6 File Offset: 0x000E1EE6
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x000E2EEE File Offset: 0x000E1EEE
		public Icon PreviewDialogIcon
		{
			get
			{
				return this.ppvIcon;
			}
			set
			{
				this.ppvIcon = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x000E2EF7 File Offset: 0x000E1EF7
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x000E2EFF File Offset: 0x000E1EFF
		public bool PrintColumnHeaders
		{
			get
			{
				return this.printColumnHeaders;
			}
			set
			{
				this.printColumnHeaders = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x000E2F08 File Offset: 0x000E1F08
		public DGVPrinter.PrintDialogSettingsClass PrintDialogSettings
		{
			get
			{
				return this.printDialogSettings;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x000E2F10 File Offset: 0x000E1F10
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x000E2F18 File Offset: 0x000E1F18
		public PrintDocument printDocument
		{
			get
			{
				return this.printDoc;
			}
			set
			{
				this.printDoc = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000E2F21 File Offset: 0x000E1F21
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x000E2F29 File Offset: 0x000E1F29
		public string PrinterName
		{
			get
			{
				return this.printerName;
			}
			set
			{
				this.printerName = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000E2F32 File Offset: 0x000E1F32
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x000E2F3A File Offset: 0x000E1F3A
		public bool PrintFooter
		{
			get
			{
				return this.printFooter;
			}
			set
			{
				this.printFooter = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x000E2F43 File Offset: 0x000E1F43
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x000E2F4B File Offset: 0x000E1F4B
		public bool PrintHeader
		{
			get
			{
				return this.printHeader;
			}
			set
			{
				this.printHeader = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x000E2F54 File Offset: 0x000E1F54
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x000E2F61 File Offset: 0x000E1F61
		public Margins PrintMargins
		{
			get
			{
				return this.PageSettings.Margins;
			}
			set
			{
				this.PageSettings.Margins = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x000E2F6F File Offset: 0x000E1F6F
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x000E2F77 File Offset: 0x000E1F77
		public double PrintPreviewZoom
		{
			get
			{
				return this._PrintPreviewZoom;
			}
			set
			{
				this._PrintPreviewZoom = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x000E2F80 File Offset: 0x000E1F80
		public PrinterSettings PrintSettings
		{
			get
			{
				return this.printDoc.PrinterSettings;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x000E2F8D File Offset: 0x000E1F8D
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x000E2F95 File Offset: 0x000E1F95
		public DGVPrinter.RowHeightSetting RowHeight
		{
			get
			{
				return this._rowheight;
			}
			set
			{
				this._rowheight = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x000E2F9E File Offset: 0x000E1F9E
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x000E2FA6 File Offset: 0x000E1FA6
		public bool ShowTotalPageNumber
		{
			get
			{
				return this.showtotalpagenumber;
			}
			set
			{
				this.showtotalpagenumber = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x000E2FAF File Offset: 0x000E1FAF
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x000E2FB7 File Offset: 0x000E1FB7
		public string SubTitle
		{
			get
			{
				return this.subtitle;
			}
			set
			{
				this.subtitle = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x000E2FC0 File Offset: 0x000E1FC0
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x000E2FCD File Offset: 0x000E1FCD
		public StringAlignment SubTitleAlignment
		{
			get
			{
				return this.subtitleformat.Alignment;
			}
			set
			{
				this.subtitleformat.Alignment = value;
				this.overridesubtitleformat = true;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x000E2FE2 File Offset: 0x000E1FE2
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x000E2FEA File Offset: 0x000E1FEA
		public Color SubTitleColor
		{
			get
			{
				return this.subtitlecolor;
			}
			set
			{
				this.subtitlecolor = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x000E2FF3 File Offset: 0x000E1FF3
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x000E2FFB File Offset: 0x000E1FFB
		public Font SubTitleFont
		{
			get
			{
				return this.subtitlefont;
			}
			set
			{
				this.subtitlefont = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x000E3004 File Offset: 0x000E2004
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x000E300C File Offset: 0x000E200C
		public StringFormat SubTitleFormat
		{
			get
			{
				return this.subtitleformat;
			}
			set
			{
				this.subtitleformat = value;
				this.overridesubtitleformat = true;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x000E301C File Offset: 0x000E201C
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x000E3029 File Offset: 0x000E2029
		public StringFormatFlags SubTitleFormatFlags
		{
			get
			{
				return this.subtitleformat.FormatFlags;
			}
			set
			{
				this.subtitleformat.FormatFlags = value;
				this.overridesubtitleformat = true;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x000E303E File Offset: 0x000E203E
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x000E3046 File Offset: 0x000E2046
		public DGVPrinter.Alignment TableAlignment
		{
			get
			{
				return this.tablealignment;
			}
			set
			{
				this.tablealignment = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x000E304F File Offset: 0x000E204F
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x000E3057 File Offset: 0x000E2057
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				if (this.docName == null)
				{
					this.printDoc.DocumentName = value;
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x000E3074 File Offset: 0x000E2074
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x000E3081 File Offset: 0x000E2081
		public StringAlignment TitleAlignment
		{
			get
			{
				return this.titleformat.Alignment;
			}
			set
			{
				this.titleformat.Alignment = value;
				this.overridetitleformat = true;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x000E3096 File Offset: 0x000E2096
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x000E309E File Offset: 0x000E209E
		public Color TitleColor
		{
			get
			{
				return this.titlecolor;
			}
			set
			{
				this.titlecolor = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x000E30A7 File Offset: 0x000E20A7
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x000E30AF File Offset: 0x000E20AF
		public Font TitleFont
		{
			get
			{
				return this.titlefont;
			}
			set
			{
				this.titlefont = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x000E30B8 File Offset: 0x000E20B8
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x000E30C0 File Offset: 0x000E20C0
		public StringFormat TitleFormat
		{
			get
			{
				return this.titleformat;
			}
			set
			{
				this.titleformat = value;
				this.overridetitleformat = true;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x000E30D0 File Offset: 0x000E20D0
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x000E30DD File Offset: 0x000E20DD
		public StringFormatFlags TitleFormatFlags
		{
			get
			{
				return this.titleformat.FormatFlags;
			}
			set
			{
				this.titleformat.FormatFlags = value;
				this.overridetitleformat = true;
			}
		}

		// Token: 0x04001876 RID: 6262
		protected Form _Owner;

		// Token: 0x04001877 RID: 6263
		protected double _PrintPreviewZoom = 1.0;

		// Token: 0x04001878 RID: 6264
		private DGVPrinter.RowHeightSetting _rowheight;

		// Token: 0x04001879 RID: 6265
		private StringAlignment cellalignment;

		// Token: 0x0400187A RID: 6266
		private StringFormat cellformat;

		// Token: 0x0400187B RID: 6267
		private StringFormatFlags cellformatflags;

		// Token: 0x0400187C RID: 6268
		private float colheaderheight;

		// Token: 0x0400187D RID: 6269
		private IList colstoprint;

		// Token: 0x0400187E RID: 6270
		private Dictionary<string, DataGridViewCellStyle> colstyles = new Dictionary<string, DataGridViewCellStyle>();

		// Token: 0x0400187F RID: 6271
		private List<float> colwidths;

		// Token: 0x04001880 RID: 6272
		private List<float> colwidthsoverride = new List<float>();

		// Token: 0x04001881 RID: 6273
		private int CurrentPage;

		// Token: 0x04001882 RID: 6274
		private int currentpageset;

		// Token: 0x04001883 RID: 6275
		private DataGridView dgv;

		// Token: 0x04001884 RID: 6276
		private string docName;

		// Token: 0x04001885 RID: 6277
		private string footer;

		// Token: 0x04001886 RID: 6278
		private Color footercolor;

		// Token: 0x04001887 RID: 6279
		private Font footerfont;

		// Token: 0x04001888 RID: 6280
		private StringFormat footerformat;

		// Token: 0x04001889 RID: 6281
		private float footerHeight;

		// Token: 0x0400188A RID: 6282
		private float footerspacing;

		// Token: 0x0400188B RID: 6283
		private int fromPage;

		// Token: 0x0400188C RID: 6284
		private StringAlignment headercellalignment;

		// Token: 0x0400188D RID: 6285
		private StringFormat headercellformat;

		// Token: 0x0400188E RID: 6286
		private StringFormatFlags headercellformatflags;

		// Token: 0x0400188F RID: 6287
		private float headerHeight;

		// Token: 0x04001890 RID: 6288
		private int lastrowprinted = -1;

		// Token: 0x04001891 RID: 6289
		private Pen lines;

		// Token: 0x04001892 RID: 6290
		private bool overridefooterformat;

		// Token: 0x04001893 RID: 6291
		private bool overridepagenumberformat;

		// Token: 0x04001894 RID: 6292
		private bool overridesubtitleformat;

		// Token: 0x04001895 RID: 6293
		private bool overridetitleformat;

		// Token: 0x04001896 RID: 6294
		private int pageHeight;

		// Token: 0x04001897 RID: 6295
		private bool pageno = true;

		// Token: 0x04001898 RID: 6296
		private Color pagenocolor;

		// Token: 0x04001899 RID: 6297
		private Font pagenofont;

		// Token: 0x0400189A RID: 6298
		private StringFormat pagenumberformat;

		// Token: 0x0400189B RID: 6299
		private float pagenumberHeight;

		// Token: 0x0400189C RID: 6300
		private bool pagenumberonseparateline;

		// Token: 0x0400189D RID: 6301
		private bool pagenumberontop;

		// Token: 0x0400189E RID: 6302
		private string pageseparator = " of ";

		// Token: 0x0400189F RID: 6303
		private IList<DGVPrinter.PageDef> pagesets;

		// Token: 0x040018A0 RID: 6304
		private string pagetext = CommonStr.strPageNumerHead + " {0} " + CommonStr.strPageNumberTail;

		// Token: 0x040018A1 RID: 6305
		private int pageWidth;

		// Token: 0x040018A2 RID: 6306
		private string parttext = " - Part ";

		// Token: 0x040018A3 RID: 6307
		private bool porportionalcolumns;

		// Token: 0x040018A4 RID: 6308
		private Icon ppvIcon;

		// Token: 0x040018A5 RID: 6309
		private bool printColumnHeaders = true;

		// Token: 0x040018A6 RID: 6310
		private DGVPrinter.PrintDialogSettingsClass printDialogSettings = new DGVPrinter.PrintDialogSettingsClass();

		// Token: 0x040018A7 RID: 6311
		private PrintDocument printDoc = new PrintDocument();

		// Token: 0x040018A8 RID: 6312
		private string printerName;

		// Token: 0x040018A9 RID: 6313
		private bool printFooter = true;

		// Token: 0x040018AA RID: 6314
		private bool printHeader = true;

		// Token: 0x040018AB RID: 6315
		private PrintRange printRange;

		// Token: 0x040018AC RID: 6316
		private int printWidth;

		// Token: 0x040018AD RID: 6317
		private Dictionary<string, float> publicwidthoverrides = new Dictionary<string, float>();

		// Token: 0x040018AE RID: 6318
		private float rowheaderwidth;

		// Token: 0x040018AF RID: 6319
		private List<float> rowheights;

		// Token: 0x040018B0 RID: 6320
		private IList rowstoprint;

		// Token: 0x040018B1 RID: 6321
		private bool showtotalpagenumber;

		// Token: 0x040018B2 RID: 6322
		private SolidBrush SolidBrush1;

		// Token: 0x040018B3 RID: 6323
		private string subtitle;

		// Token: 0x040018B4 RID: 6324
		private Color subtitlecolor;

		// Token: 0x040018B5 RID: 6325
		private Font subtitlefont;

		// Token: 0x040018B6 RID: 6326
		private StringFormat subtitleformat;

		// Token: 0x040018B7 RID: 6327
		private DGVPrinter.Alignment tablealignment;

		// Token: 0x040018B8 RID: 6328
		private string title;

		// Token: 0x040018B9 RID: 6329
		private Color titlecolor;

		// Token: 0x040018BA RID: 6330
		private Font titlefont;

		// Token: 0x040018BB RID: 6331
		private StringFormat titleformat;

		// Token: 0x040018BC RID: 6332
		private int toPage = -1;

		// Token: 0x040018BD RID: 6333
		private int totalpages;

		// Token: 0x020001C9 RID: 457
		public enum Alignment
		{
			// Token: 0x040018BF RID: 6335
			NotSet,
			// Token: 0x040018C0 RID: 6336
			Left,
			// Token: 0x040018C1 RID: 6337
			Right,
			// Token: 0x040018C2 RID: 6338
			Center
		}

		// Token: 0x020001CA RID: 458
		private class PageDef
		{
			// Token: 0x06000A30 RID: 2608 RVA: 0x000E30F4 File Offset: 0x000E20F4
			public PageDef(Margins m, int count)
			{
				this.colstoprint = new List<object>(count);
				this.colwidths = new List<float>(count);
				this.colwidthsoverride = new List<float>(count);
				this.coltotalwidth = 0f;
				this.margins = (Margins)m.Clone();
			}

			// Token: 0x040018C3 RID: 6339
			public IList colstoprint;

			// Token: 0x040018C4 RID: 6340
			public float coltotalwidth;

			// Token: 0x040018C5 RID: 6341
			public List<float> colwidths;

			// Token: 0x040018C6 RID: 6342
			public List<float> colwidthsoverride;

			// Token: 0x040018C7 RID: 6343
			public Margins margins;
		}

		// Token: 0x020001CB RID: 459
		public class PrintDialogSettingsClass
		{
			// Token: 0x040018C8 RID: 6344
			public bool AllowCurrentPage = true;

			// Token: 0x040018C9 RID: 6345
			public bool AllowPrintToFile;

			// Token: 0x040018CA RID: 6346
			public bool AllowSelection = true;

			// Token: 0x040018CB RID: 6347
			public bool AllowSomePages = true;

			// Token: 0x040018CC RID: 6348
			public bool ShowHelp = true;

			// Token: 0x040018CD RID: 6349
			public bool ShowNetwork = true;

			// Token: 0x040018CE RID: 6350
			public bool UseEXDialog;
		}

		// Token: 0x020001CC RID: 460
		public enum RowHeightSetting
		{
			// Token: 0x040018D0 RID: 6352
			StringHeight,
			// Token: 0x040018D1 RID: 6353
			CellHeight
		}
	}
}
