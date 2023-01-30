namespace Desafio.Domain;

public static class Extensions
{
    public static T Get<T>(this IServiceProvider provider) where T : class =>
       (provider.GetService(typeof(T)) as T)!;


}
