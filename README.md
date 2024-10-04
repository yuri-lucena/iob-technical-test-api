# Iob.Bank.API

Esse projeto é uma API Restful para gerenciamento de uma conta bancaria. Foi desenvolvido utilizando .NET 8.0 e Entity Framework Core.

## Executando o projeto

Para executar o projeto, faça o seguinte:

## Iniciando projeto
1. Realize o download do Docker (https://www.docker.com/products/docker-desktop/)
2. Após a instalação, vá a pasta do projeto e execute: `docker compose up -d`
3. Execute: `dotnet ef database update  --project .\Iob.Bank.Infra\Iob.Bank.Infra.csproj --startup-project .\Iob.Bank.Host\Iob.Bank.Host.csproj --context Iob.Bank.Infra.DataContext --configuration Release`
4. Após isso acesse a seguinte url: https://localhost:8081/swagger

## Contas de exemplo:

yuri@iob.com : 123

tata@iob.com : 123

## Tecnologias utilizadas

- .NET 8.0
- EF Core
- Docker
- Swagger
- JWT
- AutoMapper
- FluentValidation
- xUnit

## Problemas na execução? 

Caso esteja enfrentando algum problema ao executar o projeto, por favor, contate-me: <a href="mailto:yplucena@outlook.com">Contato<a/>
