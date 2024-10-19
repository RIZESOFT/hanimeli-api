using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Country : BaseEntity<int>
{
    public string Name { get; set; } = null!;
}