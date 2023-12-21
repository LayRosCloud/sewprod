using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports;

public class HelperExport
{
    protected XWPFParagraph AddText(XWPFDocument document, string text)
    {
        var paragraph = document.CreateParagraph();
        var run = paragraph.CreateRun();
        run.SetText(text);
        return paragraph;
    }

    protected XWPFParagraph CreateBreakPage(XWPFDocument document)
    {
        var breakPage = document.CreateParagraph();
        breakPage.CreateRun().AddBreak(BreakType.PAGE);
        return breakPage;
    }
}