using System.Collections.Generic;
using System.Linq;
using NPOI.XWPF.UserModel;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Exports;

public class PackagesOutput : HelperExport, IOutputTable
{
    private readonly List<PackageEntity> _packages;
    public PackagesOutput(List<PackageEntity> packages)
    {
        _packages = packages;
    }
    
    public void ExportTable(XWPFDocument document)
    {
        var list = _packages.GroupBy(x => x.Party).ToList();
        
        foreach (var groupedItem in list)
        {
            var key = groupedItem.Key!;
            
            AddTitle(key, document);

            FillTableParties(key, document);
            
            AddText(document, "");
            
            FillTablePackages(groupedItem, document);
            
            AddText(document, "");
            
            AddSignature(document);
            
            CreateBreakPage(document);
        }
    }

    private void AddTitle(PartyEntity key, XWPFDocument document)
    {
        var paragraphTitle = document.CreateParagraph();
        var title =  paragraphTitle.CreateRun();
        var titleTwo =  paragraphTitle.CreateRun();
        title.SetText("Отчет о крое ");
        titleTwo.SetText($"{key.CutNumber}/{key.Person.Uid}");
        paragraphTitle.Alignment = ParagraphAlignment.CENTER;
        titleTwo.IsBold = true;
    }

    private void FillTableParties(PartyEntity key, XWPFDocument document)
    {
        ITableBuilder<PartyEntity> partyTableBuilderBuilder = new PartyTableBuilder(document);
        partyTableBuilderBuilder.FillHeaders();
        partyTableBuilderBuilder.FillBody(key);
    }

    private void AddSignature(XWPFDocument document)
    {
        var builder = new SignatureBuilder(document);
        builder.Create();
        AddText(document, "");
        var paragraph = AddText(document, "МП");
        paragraph.Alignment = ParagraphAlignment.RIGHT;
    }
    
    private void FillTablePackages(IGrouping<PartyEntity?, PackageEntity> groupedItem, XWPFDocument document)
    {
        var length = groupedItem.ToList().Count;
        
        ITableBuilder<PackageEntity> packageTableBuilderBuilder = new PackagesTableBuilder(document, length + 1);
        packageTableBuilderBuilder.FillHeaders();
        
        for (int rowPosition = 1; rowPosition <= length; rowPosition++)
        {
            var item = groupedItem.ToList()[rowPosition - 1];
            packageTableBuilderBuilder.FillBody(item, rowPosition);
        }
    }
}