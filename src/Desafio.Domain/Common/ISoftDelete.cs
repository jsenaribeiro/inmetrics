namespace Desafio.Domain;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}
