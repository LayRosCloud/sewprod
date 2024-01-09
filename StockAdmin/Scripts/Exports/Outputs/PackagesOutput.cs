using System.Collections.Generic;
using System.Linq;
using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.Other;
using StockAdmin.Scripts.Exports.Outputs.Interfaces;
using StockAdmin.Scripts.Exports.TableBuilders;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.Outputs;

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
            PartyEntity key = groupedItem.Key!;
            
            AddTitle(key, document);

            FillTableParties(key, document);
            
            AddText(document, ExportConstants.Enter);
            
            FillTablePackages(groupedItem, document);
            
            AddText(document, ExportConstants.Enter);
            
            AddSignature(document);
            
            CreateBreakPage(document);
        }
    }

    private void AddTitle(PartyEntity key, XWPFDocument document)
    {
        var paragraphTitle = document.CreateParagraph();
        paragraphTitle.Alignment = ParagraphAlignment.CENTER;

        var additionalInfo =  paragraphTitle.CreateRun();
        additionalInfo.SetText(ExportConstants.PackagesOutput.Title);

        var span =  paragraphTitle.CreateRun();
        span.SetText($"{key.CutNumber}/{key.Person?.Uid}");
        span.IsBold = true;
    }

    private void FillTableParties(PartyEntity key, XWPFDocument document)
    {
        ITableBuilder<PartyEntity> partyTableBuilder = new PartyTableBuilder(document);
        partyTableBuilder.FillHeaders();
        partyTableBuilder.FillBody(key);
    }

    private void AddSignature(XWPFDocument document)
    {
        var signatureBuilder = new SignatureBuilder(document);
        signatureBuilder.Create();
        
        AddText(document, ExportConstants.Enter);
        
        var paragraph = AddText(document, ExportConstants.PlaceOfPrinting);
        paragraph.Alignment = ParagraphAlignment.RIGHT;
    }
    
    private void FillTablePackages(IGrouping<PartyEntity?, PackageEntity> groupedItem, XWPFDocument document)
    {
        const int firstBodyRow = 1;
        var length = groupedItem.ToList().Count;
        
        ITableBuilder<PackageEntity> packageTableBuilderBuilder = new PackagesTableBuilder(document, length);
        packageTableBuilderBuilder.FillHeaders();
        
        for (int rowPosition = firstBodyRow; rowPosition <= length; rowPosition++)
        {
            var item = groupedItem.ToList()[rowPosition - 1];
            packageTableBuilderBuilder.FillBody(item, rowPosition);
        }
    }
}