using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.TableBuilders;

public class ClothOperationTableBuilder : ITableBuilder<ClothOperationEntity>
{
    private readonly XWPFTable _table;
    public ClothOperationTableBuilder(XWPFDocument document, int rowsLength)
    {
        _table = document.CreateTable(rowsLength + 1, 3);
        _table.Width = 5000;
    }
    
    public void FillHeaders()
    {
        string[] headers =
        {
            "операция",
            "цена",
            "закончена ли",
        };
    }

    public void FillBody(ClothOperationEntity item, int index = 0)
    {
        string[] fields =
        {
            item.Operation?.Name ?? "",
            item.Price?.Number.ToString("f2") ?? "0.00",
            ConvertBooleanToString(item.IsEnded),
        };
        for (int cellIndex = 0; cellIndex < fields.Length; cellIndex++)
        {
            string field = fields[cellIndex];
            FillRow(index, cellIndex, field);
        }
    }
    private void FillRow(int rowPos,int cellPos, string text)
    {
        _table.GetRow(rowPos).GetCell(cellPos).SetText(text);
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