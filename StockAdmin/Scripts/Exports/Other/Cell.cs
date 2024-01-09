using System;

namespace StockAdmin.Scripts.Exports.Other;

public struct Cell
{
    private int _rowPosition;
    private int _columnPosition;

    public Cell(int rowPosition, int columnPosition) : this()
    {
        RowPosition = rowPosition;
        ColumnPosition = columnPosition;
    }

    public int RowPosition
    {
        get => _rowPosition;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            _rowPosition = value;
        }
    }
    public int ColumnPosition
    {
        get => _columnPosition;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            _columnPosition = value;
        }
    }
}