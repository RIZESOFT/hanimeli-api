namespace HanimeliApp.Domain.Entities.Abstract;

public interface ISoftDeletableEntity
{
    bool IsDeleted { get; set; }
}

