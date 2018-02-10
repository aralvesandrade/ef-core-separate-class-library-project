## Using EF Core in a Separate Class Library project

Nesse projeto iremos utilizar `Docker` para subir um container do `SQL Express` e criar um projeto utilizando `Entity Framework Core` em classes de bibliotecas separadas, iremos aprender a fazer as referencias entre os projetos, depois iremos utilizar o `migrations` para criar o banco de dados e suas respectivas tabelas.

Boa codificacao!

## Pré-requisitos

Precisa ter instalado os pacotes abaixo:

Docker
.NET Core 2.1.4 SDK
Visual Studio Code

## Adicionando SQL Server

Graças ao `Docker`, é super rápido e fácil de começar com isso. Do terminal, vamos baixar e executar uma nova instância do SQL Server para Linux como um novo container `Docker`.

```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=SqlExpress123' -e 'MSSQL_PID=Express' -p 1433:1433 --name sqlexpress -d microsoft/mssql-server-linux
```

Verificar se o container `Docker` do SQL Server `sqlexpress` está no ar:

```
docker ps -a
```

## Criando os projetos .Net

A partir do terminal `Windows PowerShell`, `Git Bash` ou próprio `Prompt de Comando` do `Windows`, vamos criar os diretorios dos projetos:

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
cd hubMarket-domain
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
cd..
cd hubMarket-data
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

Criar arquivo no projeto `hubMarket-domain` com o nome `User.cs`:

```c#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hubMarket_domain
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
```

Criar arquivo no projeto `hubMarket-data` com o nome `Context.cs`:

```c#
using System;
using hubMarket_domain;
using Microsoft.EntityFrameworkCore;

namespace hubMarket_data
{
    public class Context: DbContext
    {
        public Context()
        {
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=hubMarket;User ID=sa;Password=SqlExpress123;");
        }

        public DbSet<User> Users { get; set; }
    }
}
```

Criar arquivo no projeto `hubMarket-app` com o nome `UserApp.cs`:

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using hubMarket_data;
using hubMarket_domain;

namespace hubMarket_app
{
    public class UserApp
    {
        private readonly Context _context;

        public UserApp()
        {
            _context = new Context();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
    }
}
```

Editar metodo `Main` do arquivo `Program.cs`, deixando assim:

```c#
using System;
using hubMarket_app;
using hubMarket_domain;

namespace hubMarket_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userApp = new UserApp();

            userApp.Add(new User { Login = "Teste1", Password = "123" });
            userApp.Add(new User { Login = "Teste2", Password = "123" });
            userApp.Add(new User { Login = "Teste3", Password = "123" });

            var users = userApp.GetAll();

            foreach (var item in users)
            {
                Console.WriteLine($"Login: {item.Id}-{item.Login}");
            }

            Console.ReadLine();

        }
    }
}
```

## Trabalhando com Entity Framework Core

Dropar tabela usando `dotnet ef`, caso existir:

```
cd hubMarket-data
dotnet ef --startup-project ../hubMarket-console database drop
```

Adicionar o `migrations` ao projeto:

```
dotnet ef --startup-project ../hubMarket-console migrations add Init
```

Agora vamos criar o banco de dados `hubMarket` e suas respectivas tabelas:

```
dotnet ef --startup-project ../hubMarket-console database update
```

Agora vamos compilar o projeto e depois executar:

```
cd hubMarket
dotnet build
dotnet run --project hubMarket-console/
```

Pronto, devera mostrar na tela o resultado abaixo:

```
Login: 1-Teste1
Login: 2-Teste2
Login: 3-Teste3
```
