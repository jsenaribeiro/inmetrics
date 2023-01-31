# Desafio Inmetrics

Esse projeto faz parte do desafio de recrutamento da inmetrics. 

## 1. Produto

Desafio consiste em uma solução de controle de fluxo de caixa com lançamentos (débito e crédito) e com relatório com o saldo do caixa consolidado diário.

### Requisitos

* Serviço com lançamentos
* Serviço de consolidado diário

## 2. Processo

Nesta seção contém as orientações para os processos de implantação e execução do software que depende da instalação dos seguintes componentes para sua execução e desenvolvimento:

* Docker
* .NET SDK 7.0+
* Visual Studio Community
* Visual Studio Specflow Extension

### 2.1 Implantação

A instalação do Docker já vem acompanhada das versões do Windows 10+ através do Docker Desktop e o WSL2 (Windows Subsystem for Linux).  O .NET SDK pode baixado e instalado através do side oficial da microsoft em [.NET SDK](https://dotnet.microsoft.com/en-us/download). Enquanto o Visual Studio pode ser instalado com a versão gratuíta Community ou ainda ser utilizado com o Visual Code.

### 2.2 Execução 

A pasta **run** contém alguns scripts com os arquivos de configuração de containers Docker . Para executar o serviço REST da aplicação com seu banco de dados o script abaixo.

```bash
run\environment.bat --build # ao executar a primeira vez
run\environment.bat # com a imagem já criada, apenas executa
```

Os testes podem ser executados via Visual Studio com a extensão SpecFlow ou via linha de comando com o **dotnet test** na pasta do projeto (**src**). Há também um script de atalho na pasta run que pode ser executado com o comando abaixo.

```bash
run\test.bat # executa o dotnet test com uma melhor formatação visual
```

## 3. Projeto

Nesta seção é apresentado o modelo de domínio e as decisões arquiteturais do projeto. O projeto é um software de serviço utilizando a plataforma do .NET 7.0+ na linguagem C#

### 3.1 Modelo

Como modelo de domínio foi projetada a seguinte solução seguindos a abordagem Domain-Driven Design, conjuntamente dos princípios XP de Simple Design.


![[domain.svg]]


### Princípios

* DKY (DRY, KISS, YAGNI)
* SOLID (SRP, OCP, LKV, ISP, DIP)
* High Coehsion x Low Coupling
* Clean Code 

### Arquitetura

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



