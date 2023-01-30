using Desafio.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Desafio.Testing;

public static class Extensions
{
    public static void ShouldContains(this List<Invalid> invalids, string message) =>
       invalids.Select(x => x.Error).Contains(message).Should().BeTrue();

    public static void ShouldBeSuccess(this ObjectResult result) =>
       result.StatusCode.Should().BeLessThan(300);

    public static void ShouldBeAnExpectedResult(this ObjectResult result) =>
       result.StatusCode.Should().NotBe(500, result.Value?.ToString());

    public static void Should<T>(this ObjectResult result) where T : Exception => Should<T>(result, string.Empty);

    public static void Should<T>(this ObjectResult result, string field) where T : Exception
    {
        var content = result?.Value;

        var message = string.IsNullOrWhiteSpace(field)
           ? (Activator.CreateInstance(typeof(T)) as Exception)!.Message
           : (Activator.CreateInstance(typeof(T), field) as Exception)!.Message;

        if (content is List<Invalid> invalids) invalids.ShouldContains(message);
        else if (content is string failure) failure.Should().Be(message);
        else throw new NotImplementedException("Failed assertion");
    }

    public static void Set(this object instance, string field, object? value)
    {
        long number = 0;
        bool logic = false;

        if (instance is null) return;

        var property = instance.GetType().GetProperties()
           .FirstOrDefault(x => x.Name.ToLower() == field.ToLower());

        if (property is null) return;

        var numeric = Regex.IsMatch(value?.ToString() ?? "", @"^\d+$");
        if (numeric) number = long.Parse(value!.ToString()!);

        var boolean = Regex.IsMatch(value?.ToString() ?? "", @"true|false");
        if (boolean) logic = bool.Parse(value!.ToString()!);

        if (property.PropertyType == typeof(bool))
            property.SetValue(instance, logic);

        else if (property.PropertyType == typeof(long))
            property.SetValue(instance, number);

        else property.SetValue(instance, value);
    }

    public static object? Get(this object instance, string field)
    {
        if (instance is null) return null;

        var property = instance.GetType().GetProperties()
           .FirstOrDefault(x => x.Name.ToLower() == field.ToLower());

        if (property is null) return null;

        return property?.GetValue(instance);
    }

    public static bool IsLike(this string? text, object? other) =>
       text?.ToLower().Contains(other?.ToString()?.ToLower() ?? "") ?? false;
}