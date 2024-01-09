using System;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports.TableBuilders;

public class SignatureBuilder
{
    private readonly XWPFTable _table;
    private readonly int _rows;
    private readonly int _columns;

    public SignatureBuilder(XWPFDocument document)
    {
        _rows = 2;
        _columns = 6;
        _table = document.CreateTable(_rows, _columns);
        var tblLayout1 = _table.GetCTTbl().tblPr.AddNewTblLayout();
        tblLayout1.type = ST_TblLayoutType.@fixed;
    }

    public void Create()
    {
        const int cellFirst = 1;
        const int cellSecond = 3;
        const int cellThird = 5;
        
       Subscribe(_table.GetRow(1).GetCell(cellFirst), "место подписи");
       Subscribe(_table.GetRow(1).GetCell(cellSecond), "дата");
       Subscribe(_table.GetRow(1).GetCell(cellThird), "ФИО подписывающего");
       _table.GetRow(0).GetCell(cellSecond).SetText(DateTime.Now.ToShortDateString());
       
       _table.SetColumnWidth(0, (ulong)WidthColumns.Blank);
       _table.SetColumnWidth(1, (ulong)WidthColumns.Subscribe);
       _table.SetColumnWidth(2, (ulong)WidthColumns.Space);
       _table.SetColumnWidth(3, (ulong)WidthColumns.Date);
       _table.SetColumnWidth(4, (ulong)WidthColumns.Space);
       _table.SetColumnWidth(5, (ulong)WidthColumns.FullName);

       for (int rowIndex = 0; rowIndex < _rows; rowIndex++)
       {
           for (int columnIndex = 0; columnIndex < _columns; columnIndex++)
           {
               var cell = _table.GetRow(rowIndex).GetCell(columnIndex);
               
               foreach (var paragraph in cell.Paragraphs)
               {
                   paragraph.SpacingAfter = 0;
               }
               
               if (columnIndex == cellFirst || columnIndex == cellSecond || columnIndex == cellThird)
               {
                   if (rowIndex == 1)
                   {
                       ClearBorders(cell, topClear: false);
                   }
                   else
                   {
                       ClearBorders(cell, bottomClear: false);
                   }
               }
               else
               {
                   ClearBorders(cell);
               }
           }
       }
    }

    private void Subscribe(XWPFTableCell cell, string name)
    {
        var paragraph = cell.Paragraphs[0];
        paragraph.Alignment = ParagraphAlignment.CENTER;
        var run = paragraph.CreateRun();
        run.SetText(name);
        run.IsBold = true;
        run.FontSize = 7;
    }
    
    private void ClearBorders(XWPFTableCell cell, bool topClear = true, bool bottomClear = true, bool rightClear = true, bool leftClear = true)
    {
        var borders = cell.GetCTTc().AddNewTcPr().AddNewTcBorders();
        
        var border = new CT_Border();
        
        if (topClear)
        {
            borders.top = border;
        }

        if (bottomClear)
        {
            borders.bottom = border;
        }
        
        if (rightClear)
        {
            borders.right = border;
        }
        
        if (leftClear)
        {
            borders.left = border;
        }
    }
}