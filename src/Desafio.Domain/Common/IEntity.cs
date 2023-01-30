namespace Desafio.Domain;

public interface IEntity<I> where I : IEquatable<I>
{
    I Id { get; }

    Audit Audit { get; }

    void SetAudit(string user, CRUD crud)
    {
        this.GetType().GetProperty(nameof(Audit))?
           .SetValue(this, new Audit(user, crud));
    }
}