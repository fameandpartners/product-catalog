namespace Fame.Service.Services
{

    public partial class ImportService
    {
        private interface IDataImport<out T>
        {
            T FromCsv(string csvLine);
            string TabTitle { get; }
        }
    }
}