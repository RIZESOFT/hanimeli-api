using HanimeliApp.Domain.Entities.Abstract;

namespace HanimeliApp.Domain.Entities;

public class Address : BaseEntity<int>
{
    public int UserId { get; set; }
    public string AddressLine { get; set; } = null!;
    public int StateId { get; set; }
    public int CityId { get; set; }
    public int CountryId { get; set; }
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual State State { get; set; } = null!;
    public virtual City City { get; set; } = null!;
    public virtual Country Country { get; set; } = null!;
}
