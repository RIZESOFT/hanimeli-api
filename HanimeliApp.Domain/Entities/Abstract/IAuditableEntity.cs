namespace HanimeliApp.Domain.Entities.Abstract;

public interface IAuditableEntity<TPrimaryKey, TUserPrimaryKey> : IEntity<TPrimaryKey>
{
    TUserPrimaryKey? UserId_CreatedBy { get; set; }
    DateTime? CreatedDateTime { get; set; }
    TUserPrimaryKey? UserId_LastUpdatedBy { get; set; }
    DateTime? LastUpdatedDateTime { get; set; }
}

