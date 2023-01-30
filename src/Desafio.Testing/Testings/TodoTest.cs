using Microsoft.VisualStudio.TestTools.UnitTesting;
using Desafio.Application;
using Desafio.Domain;

namespace Desafio.Testing;

[TestClass]
public class TodoTest : AbstractSteps<TodoController>
{
   public enum Scenario { Success, Required, NotFound, TaskConflict }

   public TodoTest()
   {      
      var todo = new Domain.Todo
      {
         Task = "task",
         User = "test@email.com",
         Done = false
      };

      var account = new Domain.Account
      {
         Name= "Test",
         Email = "test@email.com",
         Role = "admin",
         Password = "123"
      };

      var todoId = unitOfWork.Todos.SaveAsync(todo).Result;
      var accountId = unitOfWork.Accounts.SaveAsync(account).Result;

      ID = (todoId, accountId);
   }

   [TestMethod]
   [DataRow(Scenario.Required, "")]
   [DataRow(Scenario.NotFound, "...")]
   [DataRow(Scenario.Success, "test@email.com")]
   public void Search(Scenario scenario, string email)
   {
      var request = new SearchTodoRequest();
      var content = GetControllerResult(request);
      var success = content?.StatusCode <= 300;

      if (scenario == Scenario.Success) 
      {
         var todos = content?.Value as TodoListModel;

         Assert.IsTrue(success);
         Assert.IsTrue(todos?.Any() ?? false);
      }

      if (scenario == Scenario.NotFound)
         AssertContains(content!, new NotFoundException("Email").Message);

      if (scenario == Scenario.Required)
         AssertContains(content!, new RequiredException("Email").Message);
   }

   [TestMethod]
   [DataRow(Scenario.Required, "")]
   [DataRow(Scenario.TaskConflict, "task")]
   [DataRow(Scenario.Success, "task-another")]
   public void Create(Scenario scenario, string task)
   {
      var request = new CreateTodoRequest(task);
      var content = GetControllerResult(request)!;
      var created = content.Value as TodoCreatedEvent;
      var success = content.StatusCode <= 300;

      if (scenario == Scenario.Success)
      {
         Assert.IsTrue(success);
         Assert.IsTrue(created is not null);
      }

      if (scenario == Scenario.TaskConflict)
         AssertContains(content, new ConflictException("Task").Message);

      if (scenario == Scenario.Required)
         AssertContains(content, new RequiredException("Task").Message);
   }

   [TestMethod]
   [DataRow(Scenario.Required, "", false)]
   [DataRow(Scenario.NotFound, "testing", false)]
   [DataRow(Scenario.Success, "task", true)]
   public void Update(Scenario scenario, string task, bool done)
   {
      var id = scenario switch
      {
         Scenario.Required => 0,
         Scenario.NotFound => 99999999,
         _ => ID.Todo
      };

      var request = new UpdateTodoRequest(id, task, done);
      var content = GetControllerResult(request);
      var success = content!.StatusCode <= 300;

      if (scenario == Scenario.Success)
         Assert.IsTrue(success);

      if (scenario == Scenario.NotFound)
      {
         AssertContains(content, new NotFoundException("Id").Message);
         AssertContains(content, new NotFoundException("Task").Message);
      }

      if (scenario == Scenario.Required)
      {
         AssertContains(content, new RequiredException("Id").Message);
         AssertContains(content, new RequiredException("Task").Message);
      }
   }

   [TestMethod]
   [DataRow(Scenario.Required)]
   [DataRow(Scenario.NotFound)]
   [DataRow(Scenario.Success)]
   public void Delete(Scenario scenario)
   {
      var id = scenario switch
      {
         Scenario.Required => 0,
         Scenario.NotFound => 999999,
         _ => ID.Todo
      };

      var request = new DeleteTodoRequest(id);
      var content = GetControllerResult(request);
      var success = content!.StatusCode <= 300;

      if (scenario == Scenario.Success)
         Assert.IsTrue(success);

      if (scenario == Scenario.NotFound)
         AssertContains(content, new NotFoundException("Id").Message);

      if (scenario == Scenario.Required)
         AssertContains(content, new RequiredException("Id").Message);
   }
}