# GutenBank

A Bank API in ASP.NET Core, Entity Framework Core & MS Server

## Run

Run in Visual Studio or with `dotnet` cli.

## Migrations

Project is Code First and using Local Db Instance with Visual Studio Db Tools.

If you want to use an separate Instance of MS Server Update the `ConnectionString` in `appsettings.json`

Then Run:

In `Package Manager Console` :

```bash
Update-Database
```

In `dotnet` cli:

```bash
dotnet ef database update
```

Refer: [MS Doc](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)

## Stay in touch

-   Author - [Ahmad](http://shafiqahmad.com)

## License

Nest is [MIT licensed](LICENSE).
