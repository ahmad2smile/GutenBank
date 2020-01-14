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

## Testing

1. Unit Tests Run `GutenBank.Test`
2. E2E Tests Run `GutenBank` API & `GutenBank.E2E` Tester

E2E Testing is setup with custom implementation with random inputs for Concurrency Error with *HTTP 409* using Console based UI with [Konsole](https://github.com/goblinfactory/konsole).

<img src="https://user-images.githubusercontent.com/6108922/72387516-8745ef80-3745-11ea-8dc0-39365b508fed.png" />

## Stay in touch

-   Author - [Ahmad](http://shafiqahmad.com)

## License

Nest is [MIT licensed](LICENSE).
