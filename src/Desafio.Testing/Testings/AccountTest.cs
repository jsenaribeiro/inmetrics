using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Desafio.Application;
using Desafio.Domain;
using Desafio.Infrastructure;

namespace Desafio.Testing;

[TestClass]
public class AccountTest : AbstractSteps
{
   public enum Scenario { Success, Required, EmailConflict, NotFound }

   public AccountTest()
   {
      var unitOfWork = new UnitOfWork(provider);
      var initial = new Domain.Account
      {
         Name = "test",
         Email = "test@email.com",
         Password = "123",
         Role = "admin"
      };

      unitOfWork.Accounts.SaveAsync(initial).Wait();
   }

   [TestMethod]
   [DataRow(Scenario.Required, "", "", "", "")]
   [DataRow(Scenario.EmailConflict, "test", "test@email.com", "123", "admin")]
   [DataRow(Scenario.Success, "test", "admin@email.com", "12345678", "admin")]
   public void Create(Scenario scenario, string name, string email, string pass, string role)
   {
      var request = new SignUpRequest(name, email, pass, role);
      var result = GetControllerResult(request);
      var success = result!.StatusCode <= 300;

      if (scenario == Scenario.Success)
         Assert.IsTrue(success);

      if (scenario == Scenario.EmailConflict)
         AssertContains(result, new ConflictException("Email").Message);

      if (scenario == Scenario.Required)
      {
         AssertContains(result, new RequiredException("Name").Message);
         AssertContains(result, new RequiredException("Email").Message);
         AssertContains(result, new RequiredException("Password").Message);
      }
   }

   //[TestMethod]
   //[DataRow(Scenario.Required, "", "", "", "")]
   //[DataRow(Scenario.EmailConflict, "test", "fail@email.com", "111", "admin")]
   //[DataRow(Scenario.Success, "test", "test@email.com", "123", "admin")]
   //public void Update(Scenario scenario, string name, string email, string pass, string role)
   //{
   //   var request = new UpdateAccountRequest(name, email, pass, role);
   //   var result = GetControllerResult(request);
   //   var success = result!.StatusCode <= 300;

   //   if (scenario == Scenario.Success)
   //   {
   //      Assert.IsTrue(success);
   //      Assert.AreEqual(scenario, Scenario.Success);
   //   }

   //   if (scenario == Scenario.Required)
   //   {
   //      AssertContains(result, new RequiredException("Name").Message);
   //      AssertContains(result, new RequiredException("Email").Message);
   //      AssertContains(result, new RequiredException("Password").Message);
   //   }

   //   if (scenario == Scenario.NotFound)
   //      AssertContains(result, new NotFoundException("Email").Message);
   //}

   [TestMethod]
   [DataRow(Scenario.Required, "", "", "", "")]
   [DataRow(Scenario.NotFound, "test", "fail@email.com", "000", "admin")]
   [DataRow(Scenario.Success, "test", "test@email.com", "123", "admin")]
   public void Delete(Scenario scenario, string name, string email, string pass, string role)
   {
      var request = new SignOutRequest(email, pass);
      var result = GetControllerResult(request);
      var success = result!.StatusCode <= 300;

      if (scenario == Scenario.Success)
      {
         Assert.IsTrue(success);
         Assert.AreEqual(scenario, Scenario.Success);
      }

      if (scenario == Scenario.Required)
      {
         AssertContains(result, new RequiredException("Email").Message);
         AssertContains(result, new RequiredException("Password").Message);
      }

      if (scenario == Scenario.NotFound)
         AssertContains(result, new NotFoundException("Email").Message);
   }
}