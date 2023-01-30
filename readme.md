# Desafio Inmetrics

Esse projeto faz parte do desafio de recrutamento da inmetrics. 

## Introdução

O projeto desafio é um serviço webapi utilizando protocolo REST para consulta de caixa e lançamentos escrito em C# na plataforma .NET 7.0.  Nesta seção contém as orientações para os prerequisitos, instalação, execução, implantação e testes.

### Prerequisitos

Como pre-requisitos de execução depende

| Artefato | Contexto | Versão |
|-|-|-|
| Docker | execução | 20+ |
| .NET SDK | desenvolvimento | 7.0+ |
| Visual Studio | desenvolvimento | 2022 |

### Instalação

A instalação do Docker já vem acompanhada das versões do Windows 10+ através do Docker Desktop e o WSL2 (Windows Subsystem for Linux).  O .NET SDK pode baixado e instalado através do side oficial da microsoft em [.NET SDK](https://dotnet.microsoft.com/en-us/download). Enquanto o Visual Studio pode ser instalado com a versão gratuíta Community ou ainda ser utilizado com o Visual Code.

### Execução

A pasta **run** contém alguns scripts e arquivos de conteiners de Docker e Docker Compose. Para executar o sistema basta entrar na pasta e digitar o seguinte comando.

```
cd src
docker-compose up
```

### Implantação

Por utilizar um conteiner docker, depende de um servidor configurado para executar containers Docker, de preferência com ferramentas de integração contínua para automatizar a implantação. Além de serviços de análise estática para garantia de qualidade de código, como o SonarQube.

### Testes

Para executar os testes pode ser executada via linha de comando **dotnet test** na pasta src, via script de atalho em **run/test.bat** ou via Visual Studio com a extensão do SpecFlow.

```prompt
cd src && dotnet test
cd run && test.bat
```


## Projeto

Desafio consiste em uma solução de controle de fluxo de caixa com lançamentos (débito e crédito) e com relatório com o saldo do caixa consolidado diário.

### Requisitos

* Serviço com lançamentos
* Serviço de consolidado diário

### Modelo

Como modelo de domínio foi projetada a seguinte solução seguindos a abordagem Domain-Driven Design, conjuntamente dos princípios XP de Simple Design.


![[domain.svg]]


### Princípios

* DKY (DRY, KISS, YAGNI)
* SOLID (SRP, OCP, LKV, ISP, DIP)
* High Coehsion x Low Coupling

### Solução

Como solução técnica foi utilizada uma arquitetura em camadas com isolamento do domínio utilizando serviço RESTful com banco de dados conteinerizados. O projeto visou reduzir as dependências externas sem comprometer a qualidade técnica da solução. Segue um resumo dos pacotes, padrões e abordagens utilizados:

* Arquitetura MS em DDD com suporte a EDA
* Banco de dados com SqlServer (suporta MongoDb e Sqlite )
* ORM com Entity Framework Core com Code First (migrations)
* Documentação da API com Swagger
* Suporta i18n com Resources
* Suporta a Event-Driven via reflexão e autenticação JWT Bearer
* Dynamic dispatch customizado utilizando reflexão
* Algoritmo de validação utilizando Notification pattern
* Padrão de Dependency Inversion  nativo do .NET
* Padrões de domínio com Entity, Repository, Factory, Aggregate, Value, Module
* Padrões de design com ViewModel, UnitOfWork, SoftDelete
* TDD/BDD  com cenários em cucumber via SpecFlow
* Configuração via variáveis de ambiente
* Asserções com FluentAssertions
* Mocking com NSubstitue
* Suporta Soft Delete pattern
* UnitOfWork pattern
* ViewModel pattern
* Logs com Serilog



