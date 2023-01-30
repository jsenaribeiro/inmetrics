# Arquitetura



## Projeto

### Conhecimento

* CleanCode
* Domain-Driven Design
* Inversion Of Control
* Object-Relational Mapping
* Request-Request Responsability Segregation
* Microservices Architecture
* Event-Driven Architecture
* RESTful API

### Bibliotecas (dependências)

* C# .NET 7 
* 

### Camadas

A solução é organizada em 3 camadas/projetos (tier) seguido a abordagem Domain-Driven Design, com uma camada de domínio isolada (biblioteca) além da separação das implementações de infraestrutura em uma camada própria, para prover inversão de dependência das tecnologias de persistência.

* Domain

A bibliteoca de domínio é um projeto contendo todo o segmento de conhecimento aplicado ao software, aplicando os padrões DDD como Entity, Repository, Value Object além do padrão CQRS com handlers, commands, queries, além de suporte a arquitetura orientado a evento com os events, além de ViewModel. O projeto implementa globalização (i18n) utilizando Resource associado a Exceções de domínio e validação.

A classe ValidationException implementa o padrão Notification que permite agregar várias exceções (validações) em um única resposta, representado em um objeto contendo o campo (para destaque no frontend) e mensagem de erro.

A API da interface IRepository foi projetada para ser versátil, maximizando seu reuso através do suporte ao uso de Expression Linq, permitindo uma semântica de consultas.

| Pastas    | Descrições                                                   |      |
| :-------- | ------------------------------------------------------------ | ---- |
| Abstract  | Interfaces e classes abstratas utilizadas pela camada de domínio |      |
| Account   | Módulo de domínio relacionado com eventos, comandos, consultas, repositorios, entidade, modelos, handler, etc |      |
| Todo      | Módulo de domínio relacionado com eventos, comandos, consultas, repositorios, entidade, modelos, handler, etc |      |
| Exception | Contém as exceções de domínio (herdam de DomainException) e a ValidationException (múltiplas exceções) |      |

* Infrastructure

A camada de infraestrutura principalmente implementa os repositories, além de outras dependências técnicas como serviços de filas e outros crosscutting concernes, como auditoria, logging, segurança, etc.

Aqui um mediator é implementado manualmente via programação reflexiva, evitando a utilização de bibliotecas como **mediatr** dado que comprometeria o isolamento da camada de domínio, pois, dado que o dynamic dispatch da biblioteca depende da implementar suas interfaces, a camada de domínio precisaria importar esse pacote ou depender de uma camada extra (crosscuting) para o mesmo fim, comprometendo a SoC orientada ao domínio. Esse mediator implementado é simples, fácil de manter e fácil de integrar com serviços de filas.

| Pastas      | Descrições                                                   |      |
| :---------- | ------------------------------------------------------------ | ---- |
| Auditing    | Objeto para informação de auditoria que é independente do domínio (aplicado apenas na camada de infraestrutura) |      |
| Connections | Implementações da interface IConnection, que permite abstrair diversos sistemas gerenciadores do bancos de dados na implementação do DbContext do Entity Framework |      |
| Contexts    | Classes de contextos para EntityFramework Core e Mongo Driver |      |
| Eventigs    | Implementa o mediator com programação reflexiva e classe de eventos de integração (MQEvent) |      |
| Factories   | Contém as fábricas abstrata de repositório para EntityFramework e MongoDriver, com a API sugerida para IRepository na camada de domínio (Abstract), as implementações dos repositórios |      |
| Mappings    | Classes de mapeamento ORM contendo mapeamento para Mongo e EfCore |      |
| Parsers     | Tratamento de tipos não suportados no ORM de EntityFramework (converters) e MongoDriver (serializers) |      |
| Repositores | Implementação dos repositórios de domínio                    |      |

* Application

O projeto de aplicação é um micro-serviço REST com Swagger com extensões (suporte) para autenticação JWT e  fila de mensagens com Rabbit MQ. Implementa conjuntamento testes unitários com mstest, dado que o escopo dessa camada é a entrada ideal para um teste de maior cobertura, pois cobre todo o ciclo de vida da requisição e resposta, passando por todas as camadas.

| Pastas      | Descrições                                                   |      |
| :---------- | ------------------------------------------------------------ | ---- |
| Properties  | Arquivos de propriedade padrão de execução do template de WebApi do .NET7 |      |
| Controllers | Controladores contendo as rotas e as API seguindo o padrão REST |      |
| Testings    | Testing unitários implementados no projeto de API            |      |