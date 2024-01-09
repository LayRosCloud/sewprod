using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.TableBuilders;

public class PartyTableBuilder : ITableBuilder<PartyEntity>
{
    private readonly XWPFTable _table;
    public PartyTableBuilder(XWPFDocument document)
    {
        _table = document.CreateTable(6, 2);
    }
    public void FillHeaders()
    {
        string[] fields =
        {
            "Номер кроя",
            "ФИО закройщика",
            "Модель",
            "Цена модели",
            "Дата начала",
            "Дата конца"
        };
        
        for (int rowPos = 0; rowPos < fields.Length; rowPos++)
        {
            string value = fields[rowPos];
            SetCellTitle(rowPos, value);
        }
    }

    public void FillBody(PartyEntity item, int index = 0)
    {
        PersonEntity? person = item.Person;
        string fullName = $"{person?.LastName} {person?.FirstName} {person?.Patronymic}";
        string[] fields =
        {
            item.CutNumber,
            fullName,
            item.Model?.Title!,
            item.Price?.Number.ToString("F2") ?? "0.00",
            item.DateStart.ToShortDateString(),
            item.DateEnd?.ToShortDateString() ?? "Не закончена"
        };
        
        for (int rowPos = 0; rowPos < fields.Length; rowPos++)
        {
            string value = fields[rowPos];
            SetCellValue(rowPos, value);
        }
        
    }
    
    private void SetCellTitle(int rowPos, string title)
    {
        var cell = _table.GetRow(rowPos).GetCell(0);

        var paragraph = cell.Paragraphs[0];
       
        var run = paragraph.CreateRun();
        run.SetText(title);
        run.IsBold = true;
    }
    
    private void SetCellValue(int rowPosition, string value)
    {
        _table.GetRow(rowPosition).GetCell(1).SetText(value);
    }
}