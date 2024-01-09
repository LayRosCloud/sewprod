using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.XWPF.UserModel;
using StockAdmin.Models;
using StockAdmin.Scripts.Exports.Other;
using StockAdmin.Scripts.Exports.Outputs.Interfaces;
using StockAdmin.Scripts.Exports.TableBuilders;
using StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

namespace StockAdmin.Scripts.Exports.Outputs;

public sealed class PersonsOutput : HelperExport, IOutputTable
{
    private readonly List<PersonGroup> _personsGroups;
    private const int Empty = 0;
    
    public PersonsOutput(List<PersonGroup> personGroups)
    {
        _personsGroups = personGroups;
    }
    
    public void ExportTable(XWPFDocument document)
    {
        AddText(document, ExportConstants.PersonOutput.Title);
        foreach (var groupedPerson in _personsGroups)
        {
            FillPersonTable(document, groupedPerson.Person);
            AddText(document, ExportConstants.Enter);
            
            TryFillPackageTable(document, groupedPerson.Packages, groupedPerson.Person);
            
            AddText(document, ExportConstants.Enter);
            
            TryFillClothOperationsTable(document, groupedPerson.Operations, groupedPerson.Person);
            
            AddText(document, ExportConstants.Enter);
            
            AddSignature(document);
            AddText(document, ExportConstants.Enter);
            AddSignature(document);
            CreateBreakPage(document);
        }
    }

    private void FillPersonTable(XWPFDocument document, PersonEntity person)
    {
        ITableBuilder<PersonEntity> persons = new PersonTableBuilder(document);
        persons.FillHeaders();
        persons.FillBody(person);
    }
    
    private void AddSignature(XWPFDocument document)
    {
        var signatureBuilder = new SignatureBuilder(document);
        signatureBuilder.Create();
        AddText(document, ExportConstants.Enter);
        
        var paragraph = AddText(document, ExportConstants.PlaceOfPrinting);
        paragraph.Alignment = ParagraphAlignment.RIGHT;
    }
    
    private void TryFillPackageTable(XWPFDocument document, IReadOnlyList<PackageEntity> packages, PersonEntity person)
    {
        try
        {
            if (packages.Count == Empty)
            {
                string exceptionMessage = string.Format(ExportConstants.PersonOutput.NotCompetedCuttersException,
                    person.FullName);
                throw new ArgumentException(exceptionMessage);
            }

            FillPackageTable(document, packages);
        }
        catch (ArgumentException ex)
        {
            AddText(document, ex.Message);
        }
    }

    private void FillPackageTable(XWPFDocument document, IReadOnlyList<PackageEntity> packages)
    {
        AddText(document, ExportConstants.PersonOutput.TitleCutters);

        ITableBuilder<PackageEntity> builder = new PackagesTableBuilder(document, packages.Count);
            
        builder.FillHeaders();
            
        for (int position = 0; position < packages.Count; position++)
        {
            var package = packages[position];
            builder.FillBody(package, position + 1);
        }

        string conclusion = string.Format(ExportConstants.PersonOutput.ConclusionCutters, packages.Count,
            packages.Count(x => x.IsEnded));
        AddText(document, conclusion);
    }
    
    private void TryFillClothOperationsTable(XWPFDocument document, IReadOnlyList<ClothOperationEntity> clothOperationPersons, PersonEntity person)
    {
        try
        {
            if (clothOperationPersons.Count == Empty)
            {
                string exceptionMessage = string.Format(ExportConstants.PersonOutput.NotCompetedOperationsException,
                    person.FullName);
                throw new ArgumentException(exceptionMessage);
            }

            FillClothOperationsTable(document, clothOperationPersons);
        }
        catch (ArgumentException ex)
        {
            AddText(document, ex.Message);
        }
    }

    private void FillClothOperationsTable(XWPFDocument document, IReadOnlyList<ClothOperationEntity> clothOperationPersons)
    {
        AddText(document, ExportConstants.PersonOutput.TitleOperations);
        ITableBuilder<ClothOperationEntity> builder = new ClothOperationTableBuilder(document, clothOperationPersons.Count);
            
        builder.FillHeaders();
            
        for (int position = 0; position < clothOperationPersons.Count; position++)
        {
            var package = clothOperationPersons[position];
            builder.FillBody(package, position + 1);
        }

        string conclusion = string.Format(ExportConstants.PersonOutput.ConclusionOperations, 
            clothOperationPersons.Count, 
            clothOperationPersons.Count(x=>x.IsEnded));
        
        AddText(document, conclusion);
    }
}