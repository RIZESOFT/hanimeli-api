using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class City : BaseEntity<int>
{
    public int CountryId { get; set; }
    public string Name { get; set; } = null!;
    
    public virtual Country Country { get; set; } = null!;
}