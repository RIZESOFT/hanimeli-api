namespace HanimeliApp.Domain.Entities.Abstract;

public abstract class BaseEntity<TPrimaryKey> : IEntity<TPrimaryKey>
{

    public virtual TPrimaryKey Id { get; set; } = default!;


    public override string ToString()
    {
        return $"[{GetType().Name} - {Id}]";
    }

    public override bool Equals(object? obj)
    {
        var other = obj as BaseEntity<TPrimaryKey>;

        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        var comparer = EqualityComparer<TPrimaryKey>.Default;

        if (comparer.Equals(Id, default) || comparer.Equals(other.Id, default))
            return false;

        return comparer.Equals(Id, other.Id);
    }

    public static bool operator ==(BaseEntity<TPrimaryKey>? a, BaseEntity<TPrimaryKey>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TPrimaryKey>? a, BaseEntity<TPrimaryKey>? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
