using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports.Other;

public class TableDecorator
{
    public void FillCellHeader(XWPFTableCell cell, string title)
    {
        var paragraph = cell.Paragraphs[0];
        paragraph.Alignment = ParagraphAlignment.CENTER;
        
        var run = paragraph.CreateRun();
        run.SetText(title);
        run.IsBold = true;
    }
    
    public void FillCell(XWPFTableCell cell, string text)
    {
        cell.SetText(text);
    }

    public string ConvertBooleanToString(bool condition, string yes = "Да", string no = "Нет")
    {
        return condition ? yes : no;
    }
}