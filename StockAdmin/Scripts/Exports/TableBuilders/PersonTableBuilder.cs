﻿using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.TableBuilders;

public class PersonTableBuilder : ITableBuilder<PersonEntity>
{
    private readonly XWPFTable _table;
    public PersonTableBuilder(XWPFDocument document)
    {
        _table = document.CreateTable(4, 2);
        _table.Width = 5000;
    }
    
    public void FillHeaders()
    {
        string[] headers =
        {
            "ФИО",
            "уник. инд.",
            "день рождение",
            "почта"
        };

        for (var cellPos = 0; cellPos < headers.Length; cellPos++)
        {
            string item = headers[cellPos];
            FillHeader(cellPos, item);
        }
    }

    public void FillBody(PersonEntity item, int index = 0)
    {
        string[] items =
        {
            item.FullName,
            item.Uid,
            item.BirthDay.ToShortDateString(),
            item.Email
        };

        for (int cellPos = 0; cellPos < items.Length; cellPos++)
        {
            string itemString = items[cellPos];
            FillCell(cellPos, itemString);
        }
    }
    private void FillHeader(int cellPos, string title)
    {
        var cell =_table.GetRow(cellPos).GetCell(0);
        
        var paragraph = cell.Paragraphs[0];
        paragraph.Alignment = ParagraphAlignment.CENTER;
        var run = paragraph.CreateRun();
        run.SetText(title);
        run.IsBold = true;
    }
    
    private void FillCell(int cellPos, string text)
    {
        _table.GetRow(cellPos).GetCell(1).SetText(text);
    }
}