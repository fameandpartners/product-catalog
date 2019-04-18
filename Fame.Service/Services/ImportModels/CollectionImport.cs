namespace Fame.Service.Services
{
    public partial class ImportService
    {
        private class CollectionImport : IDataImport<CollectionImport>
        {
            public string CollectionId { get; set; }
            public string CollectionName { get; set; }

            public CollectionImport FromCsv(string csvLine)
            {
                var values = csvLine.Split(',');
                return new CollectionImport
                {
                    CollectionId = values[0],
                    CollectionName = values[1]
                };
            }

            public string TabTitle => "Collection";
        }
    }
}