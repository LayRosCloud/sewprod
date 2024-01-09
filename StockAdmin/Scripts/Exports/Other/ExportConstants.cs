namespace StockAdmin.Scripts.Exports.Other;

public sealed class ExportConstants
{
    public const string PlaceOfPrinting = "МП";
    public const string Enter = "";

    
    public sealed class PersonOutput
    {
        private const string NotCompletedException = "Сотрудником {0} не было выполнено";
        public const string NotCompetedCuttersException = $"{NotCompletedException} выкроек";
        public const string NotCompetedOperationsException = $"{NotCompletedException} операций";
        
        public const string Title = "Отчет о сотрудниках";
        public const string TitleCutters = "Все выкройки, сделанные человеком";
        public const string TitleOperations = "Все операции, сделанные человеком";
        public const string ConclusionCutters = "Количество выкроек: {0} из которых успешно выполнено: {1}";
        public const string ConclusionOperations = "Количество операций: {0} из которых успешно выполнено: {1}";
    }

    public sealed class PackagesOutput
    {
        public const string Title = "Отчет о крое ";
    }
    
}