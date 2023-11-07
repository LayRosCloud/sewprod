using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NPOI.Util;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using Zen.Barcode;

namespace StockAdmin.Scripts.Exports;

public class WordController
{
    private readonly XWPFDocument _document;
    
    public WordController()
    {
        _document = new XWPFDocument();
    }

    public XWPFParagraph AddText(string head)
    {
        var paragraph = _document.CreateParagraph();
        var run = paragraph.CreateRun();
        run.SetText(head);
        return paragraph;
    }

    public void AddRange(List<string> codeVendors, Party party, List<Package> packages)
    {
        for (int i = 0; i < codeVendors.Count; i++)
        {
            string codeVendor = codeVendors[i];
            Package package = packages[i];
            var descriptionParagraph = AddText($"модель: {party.model.title}, материал: {package.material.name}, размер: {package.size.name}, цвет: {package.color.name}");
            descriptionParagraph.Alignment = ParagraphAlignment.CENTER;
            Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
            Image image = barcode.Draw(codeVendor, 200);
        
            using MemoryStream stream = new MemoryStream();
        
            image.Save(stream, ImageFormat.Jpeg);
        
            byte[] imageBytes = stream.ToArray();

            var paragraph = _document.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;
            var run = paragraph.CreateRun();
            run.AddPicture(new MemoryStream(imageBytes), (int)PictureType.JPEG, codeVendor, Units.ToEMU(300),
                Units.ToEMU(200));
            var codeParagraph = AddText(codeVendor);
            codeParagraph.Alignment = ParagraphAlignment.CENTER;
        }
        
    }

    public void Save(string filePath)
    {
        using FileStream stream = new FileStream(filePath, FileMode.Create);
        
        _document.Write(stream);
    }
}