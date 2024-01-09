using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.TableBuilders;

public class PackagesTableBuilder : ITableBuilder<PackageEntity>
{
    private readonly XWPFTable _table;
    public PackagesTableBuilder(XWPFDocument document, int rowsLength)
    {
        _table = document.CreateTable(rowsLength + 1, 8);
        _table.Width = 5000;
    }
    
    public void FillHeaders()
    {
        string[] headers = {
            "Размер", 
            "Ростовка", 
            "Человек",
            "Материал", 
            "Количество", 
            "Повтор",
            "Обновлена",
            "Закончена"
        };
        
        for (var cellPosition = 0; cellPosition < headers.Length; cellPosition++)
        {
            string header = headers[cellPosition];
            FillHeader(cellPosition, header);
        }
    }

    private void FillHeader(int cellPos, string title)
    {
        var cell =_table.GetRow(0).GetCell(cellPos);
        var paragraph = cell.Paragraphs[0];
        paragraph.Alignment = ParagraphAlignment.CENTER;
        var run = paragraph.CreateRun();
        run.SetText(title);
        run.IsBold = true;
    }
    
    private void FillRow(int rowPos,int cellPos, string text)
    {
        _table.GetRow(rowPos).GetCell(cellPos).SetText(text);
    }

    public void FillBody(PackageEntity item, int rowPosition)
    {
        string[] items = {
            item.Size?.Number!, 
            item.Size?.Age?.FullName!, 
            item.Person?.FullName!,
            item.Material?.Name!, 
            item.Count.ToString(), 
            ConvertBooleanToString(item.IsRepeat),
            ConvertBooleanToString(item.IsUpdated), 
            ConvertBooleanToString(item.IsEnded)
        };
        for (int cellPos = 0; cellPos < items.Length; cellPos++)
        {
            string value = items[cellPos];
            FillRow(rowPosition, cellPos, value);
        }
    }

    private string ConvertBooleanToString(bool condition)
    {
        return ConvertBooleanToString(condition, "Да", "Нет");
    }
    
    private string ConvertBooleanToString(bool condition, string yes, string no)
    {
        return condition ? yes : no;
    }
}