namespace HanimeliApp.Domain.Entities.Abstract;

public interface IEntity
{
}

public interface IEntity<TPrimaryKey> : IEntity
{
    TPrimaryKey Id { get; }
}

