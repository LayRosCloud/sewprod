using System.IO;
using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports;

public class WordController : HelperExport
{
    private readonly XWPFDocument _document;
    
    public WordController()
    {
        _document = new XWPFDocument();
    }

    public void ExportOnTemplateData(IOutputTable outputTable)
    {
        outputTable.ExportTable(_document);
    }

    public void Save(string? filePath)
    {
        using FileStream stream = new FileStream(filePath, FileMode.Create);
        
        _document.Write(stream);
    }
}