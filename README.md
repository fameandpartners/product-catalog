# Fame Server

Fame Server is home of the Fame and Partners product catalogue. The catalogue pulls in data from a Google sheets spreadsheet so that new versions of the product catalogue can be generated whenever the data changes in the spreadsheet.

## Dependencies

- ElasticSearch
- Redis Server

## Config
- create appsettings.Development.json in Fame.Web folder and add all sensitive/local config variables:

```
{
  "FameConfig": {
    "DataProtectionKey": "required",
    "Dropbox": {
      "AccessKey": "required"
    },
    "Cache": {
      "Server": "localhost",
      "Port": "6379",
      "InstanceName": "LocalProductCatalog__"
    }
  },
  "ConnectionStrings": {
    "FameConnection": "Data Source=.\\;User Id=sa; Password=YourPassword; Initial Catalog=Fame;Integrated Security=false;MultipleActiveResultSets=True;Persist Security Info=True;"
  },
  "Auth0": {
    "ClientSecret": "get from Auth0"
  }
}
```

## Resources

- Product Cat Develop: https://docs.google.com/spreadsheets/d/1SGXkZhczZBSb15M9VNhRJCMdIBPIDMx6hXpozZUgtYI/edit
- Product Cat Prod   : https://docs.google.com/spreadsheets/d/1sqMMWQrp7gfEwtTHNzVeDP91vmjJjcxRTpgIt4koNyo/edit
