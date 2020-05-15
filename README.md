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

##2020.01.16
1.actions/checkout@v1变动，导致本地没有分支，checkout后处于detach状态，无法获取分支名，可以直接使用内置环境变量GITHUB_REF获取分支名
2.AWS Serverless可能由于升级原因，需要在action的workflow文件中显式设置AWS_REGION环境变量；
3.如果发现serverless命令执行失败，可以在serverless命令前面加上export SLS_DEBUG=*，开启debug。
