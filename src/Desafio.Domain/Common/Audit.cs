namespace Desafio.Domain;

public sealed class Audit
{
    public static Audit Empty => new Audit(string.Empty, DateTime.UtcNow, CRUD.Empty);

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string User { get; set; } = string.Empty;

    public CRUD CRUD { get; set; } = CRUD.Empty;

    public Audit(string user, CRUD crud) : this(user, DateTime.UtcNow, crud) { }

    public Audit(string User, DateTime Date, CRUD CRUD)
    {
        this.User = User;
        this.Date = Date;
        this.CRUD = CRUD;
    }

    public Audit() { }
}