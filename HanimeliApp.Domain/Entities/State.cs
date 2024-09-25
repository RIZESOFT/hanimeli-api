using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class State : BaseEntity<int>
{
    public int CityId { get; set; }
    public string Name { get; set; } = null!;

    public virtual City City { get; set; } = null!;
}