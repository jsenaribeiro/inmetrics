using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Desafio.Infrastructure;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
       dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
       dateTime => DateOnly.FromDateTime(dateTime))
    { }
}

public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
       (d1, d2) => d1.DayNumber == d2.DayNumber,
       d => d.GetHashCode())
    { }
}

public static class DateOnlyExtensions
{
    public static PropertyBuilder<T> AsDateOnly<T>(this PropertyBuilder<T> builder) =>
       builder.HasConversion<DateOnlyConverter, DateOnlyComparer>();
}