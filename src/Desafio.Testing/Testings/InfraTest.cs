using Microsoft.VisualStudio.TestTools.UnitTesting;
using Desafio.Application;
using Desafio.Domain;
using Desafio.Infrastructure;

namespace Desafio.Testing;

[TestClass]
public class InfraTest : AbstractSteps<TodoController>
{
   public enum Scenario { InMemory, Sqlite, SqlServer, MySql, Mongo, Postgres }

   [TestMethod]
   //[Ignore("INFRASTRUCTURE")]
   [DataRow(Scenario.MySql)]
   [DataRow(Scenario.Sqlite)]
   [DataRow(Scenario.InMemory)]
   [DataRow(Scenario.SqlServer)]
   [DataRow(Scenario.Postgres)]
   public void Database(Scenario scenario)
   {
      var database = scenario switch
      {
         Scenario.MySql => SGBD.MySql,
         Scenario.Sqlite => SGBD.Sqlite,
         Scenario.SqlServer => SGBD.SqlServer,
         Scenario.Postgres => SGBD.Postgres,
         Scenario.Mongo => SGBD.Mongo,
         _ => SGBD.InMemory
      };

      Current.Database = database;

      var todo = new Domain.Todo
      {
         Task = "task",
         User = "test@email.com",
         Done = false
      };

      var account = new Domain.Account
      {
         Name = "Test",
         Email = "test@email.com",
         Role = "admin",
         Password = "123"
      };

      unitOfWork.Todos.SaveAsync(todo).Wait();
      unitOfWork.Accounts.SaveAsync(account).Wait();

      var request = new SearchTodoRequest();
      var content = GetControllerResult(request);
      var success = content?.StatusCode <= 300;

      Assert.IsTrue(success);
   }

   public void MessageQueue(Scenario scenario)
   {
      var request = new SignUpRequest("Test", "test@mail.com", "123", "Admin");
      var content = GetControllerResult(request);
   }
}