using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports;

public interface IOutputTable
{
    void ExportTable(XWPFDocument document);
}