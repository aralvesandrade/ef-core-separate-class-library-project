Using EF Core in a Separate Class Library project

```
mkdir hubMarket
cd hubMarket
dotnet new sln
mkdir hubMarket-domain
mkdir hubMarket-data
mkdir hubMarket-app
mkdir hubMarket-console
mkdir hubMarket-web
cd hubMarket-domain
dotnet new classlib
cd..
cd hubMarket-data
dotnet new classlib
cd..
cd hubMarket-app
dotnet new classlib
cd..
cd hubMarket-console
dotnet new console
cd..
cd hubMarket-web
dotnet new mvc
cd..
```

Adicionar projetos a solution principal `hubMarket`:

```
dotnet sln add hubMarket-domain/hubMarket-domain.csproj
dotnet sln add hubMarket-data/hubMarket-data.csproj
dotnet sln add hubMarket-app/hubMarket-app.csproj
dotnet sln add hubMarket-console/hubMarket-console.csproj
dotnet sln add hubMarket-web/hubMarket-web.csproj
```

Adicionando package aos projetos `hubMarket-domain` e `hubMarket-data`:

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Adicionando package ao projeto `hubMarket-data`:

```
dotnet add package Microsoft.EntityFrameworkCore.Design --version 2.0.0
```

Editar o arquivo `hubMarket-data.csproj`, deixando igual abaixo:

```c#
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.0.0" PrivateAssets="All" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="2.0.1" />
  </ItemGroup>

  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
  </ItemGroup>

</Project>
```

Adicionando referencia ao projeto `hubMarket-data`:

```
dotnet add reference ../hubMarket-domain/hubMarket-domain.csproj
```

Adicionando referencia ao projeto `hubMarket-app`:

```
dotnet add reference ../hubMarket-domain/hubMarket-domain.csproj
dotnet add reference ../hubMarket-data/hubMarket-data.csproj
```

Adicionando referencia ao projeto `hubMarket-console`:

```
dotnet add reference ../hubMarket-app/hubMarket-app.csproj
```
