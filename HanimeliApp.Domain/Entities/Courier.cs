using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Courier : BaseEntity<int>
{
    public string Name { get; set; }

}