using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports.Outputs.Interfaces;

public interface IOutputTable
{
    void ExportTable(XWPFDocument document);
}