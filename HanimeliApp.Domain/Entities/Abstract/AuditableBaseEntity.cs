namespace HanimeliApp.Domain.Entities.Abstract;

public abstract class AuditableBaseEntity<TPrimaryKey, TUserPrimaryKey> : BaseEntity<TPrimaryKey>, IAuditableEntity<TPrimaryKey, TUserPrimaryKey>
{
    public TUserPrimaryKey? UserId_CreatedBy { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public TUserPrimaryKey? UserId_LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
}
