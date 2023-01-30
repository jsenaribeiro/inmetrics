using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Desafio.Infrastructure;

public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
       timeOnly => timeOnly.ToTimeSpan(),
       timeSpan => TimeOnly.FromTimeSpan(timeSpan))
    { }
}

public class TimeOnlyComparer : ValueComparer<TimeOnly>
{
    public TimeOnlyComparer() : base(
        (t1, t2) => t1.Ticks == t2.Ticks,
        t => t.GetHashCode())
    { }
}

public static class TimeOnlyExtensions
{
    public static PropertyBuilder<T> AsTimeOnly<T>(this PropertyBuilder<T> builder) =>
       builder.HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
}