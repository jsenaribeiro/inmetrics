import os
import sys

EX_SELECTS = 0
EX_CAPTION = "project"
EX_OPTIONS = ['Keep all samples', 
   'Keep only Account with JWT', 
   'Keep only Caixa without JWT', 
   'Discard all sample entities']

DB_SELECTS = 0
DB_CAPTION = "ORM implementations"
DB_OPTIONS = ['Keep all (Oracle, Postgres, Sqlite, SqlServer and MongoDb)',
    'Keep Sqlite, SqlServer and MongoDb',
    'Only SqlServer',
    'Only Sqlite',
    'Only MySql',
    'Only MongoDb',
    'Only Postgres',
    'Only Oracle']

MQ_SELECTS = 0
MQ_CAPTION = "MQ integration"
MQ_OPTIONS = ['None', 'Kafka', 'RabbitMq']

EV_SELECTS = 0
EV_CAPTION = "patterns"
EV_OPTIONS = ['CQRS', 'RR']

EXAMPLE = "   ex.: template.bat [project-name]"

IGNORE_FOLDERS = ['.git', '.vs', '.backup']
REMOVE_FOLDERS = ["obj", "bin", "TestResults"]
CHANGING_FILES = ['txt', 'ini', 'json', 'csproj', 'yuml', 'yml', 'cs', 'md', 'user', 'sln', 'bat']
INPUTING_TEXT = "Digite sua opção (0 = default): "

CLEAN_TODOS = """unitOfWork.Todos
         .DropAsync(x => x.Id == TodoId
                      || x.User == emailTest
                      || x.Task.Contains("test")
                      || x.User == accountEmail).Wait();"""

CLEAN_ACCOUNT = """unitOfWork.Accounts
         .DropAsync(x => x.Id == AccountId
                      || x.Email == emailTest
                      || x.Email == accountEmail).Wait();"""

CLEAN_LOGIN_SRV = """
      if (testing) services.AddScoped(p => loginService!);
      else services.AddScoped<ILoginService, LoginService>();"""

DEFAULT_LOGIN_SERVICE = "services.AddScoped<ILoginService, LoginService>();"

CREATE_TEST_USER = """protected Login Createlogin(IUnitOfWork unitOfWork)
   {
      var account = new Domain.Account("test", "admin", "test@email.com", "123");

      unitOfWork.Accounts.SaveAsync(account).Wait();

      return new Login(account.Email, account.Role);
   }"""

PROJECT = sys.argv[1]
project = PROJECT[0].lower() + PROJECT[1:]
CURRENT = os.path.abspath(os.getcwd())